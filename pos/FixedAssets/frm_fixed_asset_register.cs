using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using POS.BLL;
using POS.BLL.FixedAssets;
using POS.Core;
using POS.Core.POS;
using pos.UI;
using pos.UI.Busy;

namespace pos.FixedAssets
{
    public partial class frm_fixed_asset_register : Form
    {
        private FixedAssetBLL _assetBLL;
        private DepreciationEngine _depEngine;
        private FixedAssetModel _currentAsset;
        private List<FixedAssetModel> _allAssets;
        private List<FixedAssetModel> _filteredAssets;

        public frm_fixed_asset_register()
        {
            InitializeComponent();
            _assetBLL = new FixedAssetBLL();
            _depEngine = new DepreciationEngine();
            _allAssets = new List<FixedAssetModel>();
        }

        private void frm_fixed_asset_register_Load(object sender, EventArgs e)
        {
            try
            {
                AppTheme.Apply(this);

                // Build tab contents first so runtime controls (ddlAssetCategory,
                // ddlAssetLocation, ddlSupplier, etc.) exist before data loaders run.
                BuildAssetInfoTab(this.tabAssetInfo);
                BuildDepreciationSetupTab(this.tabDepreciationSetup);
                BuildDepreciationHistoryTab(this.tabDepreciationHistory);
                BuildDisposalRevaluationTab(this.tabDisposalRevaluation);

                LoadCategories();
                LoadLocations();
                LoadSuppliers();
                LoadDepreciationAccounts();
                LoadAssets();

                // Wire up event handlers
                dgvAssets.SelectionChanged += DgvAssets_SelectionChanged;
                txtSearch.TextChanged += TxtSearch_TextChanged;
                ddlStatus.SelectedIndexChanged += FilterAssets;
                ddlCategory.SelectedIndexChanged += FilterAssets;
                ddlLocation.SelectedIndexChanged += FilterAssets;
                btnAddAsset.Click += BtnAddAsset_Click;
                btnRunDepreciation.Click += BtnRunDepreciation_Click;
                btnPostDisposal.Click += BtnPostDisposal_Click;
                btnPostRevaluation.Click += BtnPostRevaluation_Click;
                txtCost.TextChanged += TxtCost_TextChanged;
                txtDisposalProceeds.TextChanged += TxtDisposalProceeds_TextChanged;
                ddlDepMethod.SelectedIndexChanged += DdlDepMethod_SelectedIndexChanged;

                // Initialize form theme for list
                ApplyAssetListTheme();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void ApplyAssetListTheme()
        {
            dgvAssets.BackgroundColor = Color.White;
            dgvAssets.GridColor = Color.FromArgb(220, 220, 220);
            dgvAssets.RowHeadersVisible = false;
            dgvAssets.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvAssets.DefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            dgvAssets.ColumnHeadersDefaultCellStyle.BackColor = AppTheme.GridHeader;
            dgvAssets.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            dgvAssets.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _assetBLL.GetCategories(activeOnly: true);

                ddlCategory.Items.Clear();
                ddlAssetCategory.Items.Clear();
                ddlCategory.Items.Add("All");

                foreach (var cat in categories)
                {
                    if (!string.IsNullOrEmpty(cat.CategoryName))
                    {
                        ddlCategory.Items.Add(cat.CategoryName);
                        ddlAssetCategory.Items.Add(cat.CategoryName);
                    }
                }

                ddlCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading categories: {ex.Message}",
                    $"خطأ في تحميل الفئات: {ex.Message}",
                    "Error", "خطأ");
            }
        }

        private void LoadLocations()
        {
            try
            {
                var locations = _assetBLL.GetLocations(activeOnly: true);

                ddlLocation.Items.Clear();
                ddlAssetLocation.Items.Clear();
                ddlLocation.Items.Add("All");

                foreach (var loc in locations)
                {
                    if (!string.IsNullOrEmpty(loc.LocationName))
                    {
                        ddlLocation.Items.Add(loc.LocationName);
                        ddlAssetLocation.Items.Add(loc.LocationName);
                    }
                }

                ddlLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading locations: {ex.Message}",
                    $"خطأ في تحميل المواقع: {ex.Message}",
                    "Error", "خطأ");
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                SupplierBLL supplierBll = new SupplierBLL();
                DataTable dt = supplierBll.GetAll();

                ddlSupplier.Items.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    string supplierName = row["supplier_name"]?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(supplierName))
                    {
                        ddlSupplier.Items.Add(supplierName);
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading suppliers: {ex.Message}",
                    $"خطأ في تحميل الموردين: {ex.Message}",
                    "Error", "خطأ");
            }
        }

        private void LoadDepreciationAccounts()
        {
            try
            {
                AccountsBLL accountsBll = new AccountsBLL();

                // Load all accounts
                DataTable allAccounts = accountsBll.GetAll();

                ddlDepAccount.Items.Clear();
                ddlAccumDepAccount.Items.Clear();
                ddlRevaluationAccount.Items.Clear();

                // Filter for Depreciation Expense accounts (typically account_type = 5 or group = "Depreciation Expense")
                // Filter for Accumulated Depreciation / Contra-Asset accounts
                // Filter for Revaluation Reserve accounts

                foreach (DataRow row in allAccounts.Rows)
                {
                    string accountName = row["account_name"]?.ToString() ?? "";
                    string accountType = row["account_type"]?.ToString() ?? "";

                    if (!string.IsNullOrEmpty(accountName))
                    {
                        // Depreciation Expense accounts (expense type)
                        if (accountType == "5" || accountName.Contains("Depreciation Expense"))
                        {
                            ddlDepAccount.Items.Add(accountName);
                        }

                        // Accumulated Depreciation / Contra-Asset accounts
                        if (accountName.Contains("Accumulated Depreciation") || accountName.Contains("Accumulated"))
                        {
                            ddlAccumDepAccount.Items.Add(accountName);
                        }

                        // Revaluation Reserve accounts
                        if (accountName.Contains("Revaluation") || accountName.Contains("Reserve"))
                        {
                            ddlRevaluationAccount.Items.Add(accountName);
                        }
                    }
                }

                // Set defaults if available
                if (ddlDepAccount.Items.Count > 0) ddlDepAccount.SelectedIndex = 0;
                if (ddlAccumDepAccount.Items.Count > 0) ddlAccumDepAccount.SelectedIndex = 0;
                if (ddlRevaluationAccount.Items.Count > 0) ddlRevaluationAccount.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading accounts: {ex.Message}",
                    $"خطأ في تحميل الحسابات: {ex.Message}",
                    "Error", "خطأ");
            }
        }

        private void LoadAssets()
        {
            try
            {
                using (BusyScope.Show(this, "Loading assets..."))
                {
                    FixedAssetBLL assetBll = new FixedAssetBLL();

                    // Get all active assets for display
                    var allAssets = assetBll.GetAllAssets();

                    _allAssets = allAssets ?? new List<FixedAssetModel>();
                    _filteredAssets = new List<FixedAssetModel>(_allAssets);
                    RefreshAssetGrid();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Failed to load assets: {ex.Message}\n\n{ex.GetType().Name}",
                    $"فشل تحميل الأصول: {ex.Message}",
                    "Load Error", "خطأ");
            }
        }

        private void RefreshAssetGrid()
        {
            dgvAssets.Rows.Clear();
            foreach (var asset in _filteredAssets)
            {
                int rowIdx = dgvAssets.Rows.Add(
                    asset.AssetCode,
                    asset.AssetName,
                    asset.CategoryName,
                    asset.PurchaseDate.ToString("yyyy-MM-dd"),
                    asset.Cost.ToString("N2"),
                    asset.CurrentWDV.ToString("N2"),
                    asset.Status
                );

                // Color code by status
                var row = dgvAssets.Rows[rowIdx];
                switch (asset.Status)
                {
                    case "Active":
                        row.DefaultCellStyle.BackColor = Color.White;
                        break;
                    case "Disposed":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200); // Light red
                        break;
                    case "Under Repair":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 150); // Light amber
                        break;
                    case "Fully Depreciated":
                        row.DefaultCellStyle.BackColor = Color.FromArgb(200, 255, 200); // Light green
                        break;
                }
            }
        }

        private void FilterAssets(object sender, EventArgs e)
        {
            var filtered = _allAssets.AsEnumerable();

            // Filter by search text
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var searchTerm = txtSearch.Text.ToLower();
                filtered = filtered.Where(a =>
                    a.AssetName.ToLower().Contains(searchTerm) ||
                    a.AssetCode.ToLower().Contains(searchTerm) ||
                    a.CategoryName.ToLower().Contains(searchTerm) ||
                    (a.LocationName != null && a.LocationName.ToLower().Contains(searchTerm))
                );
            }

            // Filter by category
            if (ddlCategory.SelectedIndex > 0 && ddlCategory.SelectedItem != null)
            {
                var selectedCategory = ddlCategory.SelectedItem.ToString();
                filtered = filtered.Where(a => a.CategoryName == selectedCategory);
            }

            // Filter by status
            if (ddlStatus.SelectedIndex > 0 && ddlStatus.SelectedItem != null)
            {
                var selectedStatus = ddlStatus.SelectedItem.ToString();
                filtered = filtered.Where(a => a.Status == selectedStatus);
            }

            // Filter by location
            if (ddlLocation.SelectedIndex > 0 && ddlLocation.SelectedItem != null)
            {
                var selectedLocation = ddlLocation.SelectedItem.ToString();
                filtered = filtered.Where(a => a.LocationName == selectedLocation);
            }

            _filteredAssets = filtered.ToList();
            RefreshAssetGrid();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            FilterAssets(null, null);
        }

        private void DgvAssets_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAssets.SelectedRows.Count > 0)
            {
                var selectedRow = dgvAssets.SelectedRows[0];
                var assetCode = selectedRow.Cells[0].Value?.ToString();

                if (!string.IsNullOrEmpty(assetCode))
                {
                    _currentAsset = _allAssets.FirstOrDefault(a => a.AssetCode == assetCode);
                    if (_currentAsset != null)
                    {
                        LoadAssetDetail();
                        UpdateSummaryCard();
                    }
                }
            }
        }

        private void LoadAssetDetail()
        {
            if (_currentAsset == null) return;

            // Asset Information Tab
            txtAssetCode.Text = _currentAsset.AssetCode;
            txtAssetName.Text = _currentAsset.AssetName;
            txtDescription.Text = _currentAsset.AssetDescription;
            ddlAssetCategory.SelectedItem = _currentAsset.CategoryName;
            dtPurchaseDate.Value = _currentAsset.PurchaseDate;
            txtInvoiceNo.Text = _currentAsset.PurchaseInvoiceNo;
            txtCost.Text = _currentAsset.Cost.ToString("N2");
            ddlAssetLocation.SelectedItem = _currentAsset.LocationName;
            txtSerialNumber.Text = _currentAsset.SerialNumber;
            txtModelNumber.Text = _currentAsset.ModelNumber;
            ddlAssetStatus.SelectedItem = _currentAsset.Status;

            // Depreciation Setup Tab
            ddlDepMethod.SelectedItem = _currentAsset.DepreciationMethod;
            numUsefulLifeYears.Value = _currentAsset.UsefulLifeYears;
            numUsefulLifeMonths.Value = _currentAsset.UsefulLifeMonths;
            txtResidualValue.Text = _currentAsset.ResidualValue.ToString("N2");
            txtDepRate.Text = _currentAsset.DepreciationRate.ToString("N2");
            dtStartDepreciationDate.Value = _currentAsset.StartDepreciationFrom;

            // Load depreciation schedule preview
            LoadDepreciationSchedulePreview();

            // Depreciation History Tab
            LoadDepreciationHistory();

            // Disposal Tab
            if (_currentAsset.DisposalDate.HasValue)
            {
                dtDisposalDate.Value = _currentAsset.DisposalDate.Value;
                txtDisposalProceeds.Text = _currentAsset.DisposalProceeds.ToString("N2");
            }
        }

        private void LoadDepreciationSchedulePreview()
        {
            if (_currentAsset == null) return;

            dgvDepSchedule.Rows.Clear();

            try
            {
                // Generate the full depreciation schedule for this asset
                List<DepScheduleLine> schedule = _depEngine.GenerateDepreciationSchedule(_currentAsset);

                foreach (var line in schedule)
                {
                    dgvDepSchedule.Rows.Add(
                        line.PeriodDate.ToString("yyyy-MM"),
                        line.OpeningWDV.ToString("N2"),
                        line.DepreciationAmount.ToString("N2"),
                        line.AccumulatedDepreciation.ToString("N2"),
                        line.ClosingWDV.ToString("N2")
                    );
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error generating depreciation schedule: {ex.Message}",
                    $"خطأ في إنشاء جدول الاستهلاك: {ex.Message}",
                    "Error", "خطأ");
            }
        }

        private void LoadDepreciationHistory()
        {
            if (_currentAsset == null) return;

            dgvDepreciationHistory.Rows.Clear();

            try
            {
                // Get depreciation history for this asset
                List<DepScheduleLine> history = _assetBLL.GetDepreciationHistory(_currentAsset.AssetId);

                foreach (var line in history)
                {
                    dgvDepreciationHistory.Rows.Add(
                        line.PeriodDate.ToString("yyyy-MM"),
                        line.OpeningWDV.ToString("N2"),
                        line.DepreciationAmount.ToString("N2"),
                        line.AccumulatedDepreciation.ToString("N2"),
                        line.ClosingWDV.ToString("N2")
                    );
                }

                lblCurrentWDV.Text = $"Current Book Value (WDV): PKR {_currentAsset.CurrentWDV:N2}";
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading depreciation history: {ex.Message}",
                    $"خطأ في تحميل سجل الاستهلاك: {ex.Message}",
                    "Error", "خطأ");
            }
        }

        private void UpdateSummaryCard()
        {
            if (_currentAsset == null) return;

            var age = DateTime.Now.Subtract(_currentAsset.PurchaseDate);
            var years = age.Days / 365;
            var months = (age.Days % 365) / 30;
            var depreciation = _currentAsset.Cost - _currentAsset.CurrentWDV;

            lblAssetNameSummary.Text = _currentAsset.AssetName;
            lblCostSummary.Text = $"Cost: PKR {_currentAsset.Cost:N2} | Book Value: PKR {_currentAsset.CurrentWDV:N2} | Age: {years} years {months} months | Depreciation to Date: PKR {depreciation:N2}";
        }

        private void BtnAddAsset_Click(object sender, EventArgs e)
        {
            try
            {
                frm_addFixedAsset addForm = new frm_addFixedAsset();
                if (addForm.ShowDialog(this) == DialogResult.OK)
                {
                    // Reload assets to show the newly added asset
                    LoadAssets();

                    UiMessages.ShowInfo("Asset added successfully with ID: " + addForm.NewAssetId, "تم إضافة الأصل بنجاح", "Success", "نجاح");
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void BtnManageCategories_Click(object sender, EventArgs e)
        {
            try
            {
                frm_fixedAssetCategories categoryForm = new frm_fixedAssetCategories();
                if (categoryForm.ShowDialog(this) == DialogResult.OK)
                {
                    // Reload categories if any changes were made
                    LoadCategories();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void BtnManageLocations_Click(object sender, EventArgs e)
        {
            try
            {
                frm_fixedAssetLocations locationForm = new frm_fixedAssetLocations();
                if (locationForm.ShowDialog(this) == DialogResult.OK)
                {
                    // Reload locations if any changes were made
                    LoadLocations();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void BtnRunDepreciation_Click(object sender, EventArgs e)
        {
            if (_currentAsset == null)
            {
                UiMessages.ShowWarning("Please select an asset first.", "يرجى تحديد أصل أولا");
                return;
            }

            try
            {
                using (BusyScope.Show(this, "Running depreciation..."))
                {
                    // Run depreciation for the current month
                    DateTime currentPeriod = DateTime.Now;
                    int userId = UsersModal.logged_in_userid;

                    DepreciationRunSummary result = _depEngine.RunMonthlyDepreciation(currentPeriod, userId);

                    if (result != null)
                    {
                        string message = string.Format(
                            "Depreciation run completed:\n" +
                            "Period: {0:yyyy-MM}\n" +
                            "Assets Evaluated: {1}\n" +
                            "Posted: {2}\n" +
                            "Skipped: {3}\n" +
                            "Errors: {4}\n" +
                            "Total Depreciation: PKR {5:N2}",
                            result.PeriodDate, result.EvaluatedAssets, result.PostedCount,
                            result.SkippedCount, result.ErrorCount, result.TotalDepreciation);

                        UiMessages.ShowInfo(message, "تم تشغيل الاستهلاك بنجاح");

                        // Reload the current asset to show updated depreciation state
                        _currentAsset = _assetBLL.GetEligibleAssetsForDepreciation(currentPeriod)
                            .FirstOrDefault(a => a.AssetId == _currentAsset.AssetId) ?? _currentAsset;

                        LoadDepreciationHistory();
                        UpdateSummaryCard();
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error running depreciation: {ex.Message}",
                    $"خطأ في تشغيل الاستهلاك: {ex.Message}");
            }
        }

        private void BtnPostDisposal_Click(object sender, EventArgs e)
        {
            if (_currentAsset == null)
            {
                UiMessages.ShowWarning("Please select an asset first.", "يرجى تحديد أصل أولا");
                return;
            }

            // Validate disposal date
            if (dtDisposalDate.Value == null)
            {
                UiMessages.ShowWarning("Please specify a disposal date.", "يرجى تحديد تاريخ الاستبعاد");
                return;
            }

            // Validate disposal proceeds
            if (!decimal.TryParse(txtDisposalProceeds.Text, out var disposalProceeds) || disposalProceeds < 0)
            {
                UiMessages.ShowWarning("Please enter valid disposal proceeds amount.", "يرجى إدخال مبلغ صحيح للاستبعاد");
                return;
            }

            try
            {
                using (BusyScope.Show(this, "Posting disposal..."))
                {
                    DateTime disposalDate = dtDisposalDate.Value.Date;

                    // Calculate gain/loss on disposal
                    decimal bookValue = _currentAsset.CurrentWDV;
                    decimal gainLoss = disposalProceeds - bookValue;

                    // Create journal entries for disposal
                    AutoJVModel journal = new AutoJVModel
                    {
                        VoucherDate = disposalDate,
                        ReferenceNo = _currentAsset.AssetCode,
                        Narration = $"Disposal of asset {_currentAsset.AssetCode} - {_currentAsset.AssetName}",
                        ModuleName = "FixedAssets",
                        RefModule = "Disposal",
                        RefId = _currentAsset.AssetId,
                        Lines = new List<JVLineModel>()
                    };

                    // Debit Cash/Bank Account (placeholder - should be configurable)
                    journal.Lines.Add(new JVLineModel
                    {
                        AccountId = 0, // Placeholder
                        AccountName = "Bank / Cash",
                        Debit = disposalProceeds,
                        Credit = 0,
                        Narration = $"Cash received from disposal of {_currentAsset.AssetName}"
                    });

                    // Credit Asset Account (Fixed Asset)
                    journal.Lines.Add(new JVLineModel
                    {
                        AccountId = 0, // Placeholder
                        AccountName = _currentAsset.AssetName,
                        Debit = 0,
                        Credit = bookValue,
                        Narration = $"Book value of disposed asset"
                    });

                    // If there's a gain, credit gain account; if loss, debit loss account
                    if (gainLoss != 0)
                    {
                        if (gainLoss > 0)
                        {
                            journal.Lines.Add(new JVLineModel
                            {
                                AccountId = 0,
                                AccountName = "Gain on Disposal",
                                Debit = 0,
                                Credit = gainLoss,
                                Narration = $"Gain on disposal of asset"
                            });
                        }
                        else
                        {
                            journal.Lines.Add(new JVLineModel
                            {
                                AccountId = 0,
                                AccountName = "Loss on Disposal",
                                Debit = Math.Abs(gainLoss),
                                Credit = 0,
                                Narration = $"Loss on disposal of asset"
                            });
                        }
                    }

                    // Post the journal entry
                    JournalsBLL journalsBll = new JournalsBLL();
                    PostResult result = journalsBll.PostAutoJournalEntry(journal, UsersModal.logged_in_userid);

                    if (result != null && result.Success)
                    {
                        // Update asset status to Disposed
                        _currentAsset.DisposalDate = disposalDate;
                        _currentAsset.DisposalProceeds = disposalProceeds;
                        _currentAsset.Status = "Disposed";

                        _assetBLL.UpdateAssetDepreciationState(
                            _currentAsset.AssetId,
                            _currentAsset.AccumulatedDepreciation,
                            0, // WDV becomes 0 after disposal
                            disposalDate,
                            "Disposed");

                        string message = string.Format(
                            "Disposal posted successfully:\n" +
                            "Disposal Date: {0:yyyy-MM-dd}\n" +
                            "Book Value: PKR {1:N2}\n" +
                            "Disposal Proceeds: PKR {2:N2}\n" +
                            "Gain/Loss: PKR {3:N2}\n" +
                            "Voucher ID: {4}",
                            disposalDate, bookValue, disposalProceeds, gainLoss, result.VoucherId);

                        UiMessages.ShowInfo(message, "تم نشر الاستبعاد بنجاح");
                        LoadAssetDetail();
                        UpdateSummaryCard();
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "Failed to post disposal journal entry.",
                            "فشل في نشر قيد الاستبعاد");
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error posting disposal: {ex.Message}",
                    $"خطأ في نشر الاستبعاد: {ex.Message}");
            }
        }

        private void BtnPostRevaluation_Click(object sender, EventArgs e)
        {
            if (_currentAsset == null)
            {
                UiMessages.ShowWarning("Please select an asset first.", "يرجى تحديد أصل أولا");
                return;
            }

            // Validate revaluation cost
            if (!decimal.TryParse(txtCost.Text, out var newRevaluationCost) || newRevaluationCost < 0)
            {
                UiMessages.ShowWarning("Please enter a valid revaluation cost.", "يرجى إدخال تكلفة إعادة تقييم صحيحة");
                return;
            }

            // Check if there's a change in cost
            if (newRevaluationCost == _currentAsset.Cost)
            {
                UiMessages.ShowWarning("The revaluation cost must be different from the current cost.", "يجب أن تكون تكلفة إعادة التقييم مختلفة عن التكلفة الحالية");
                return;
            }

            try
            {
                using (BusyScope.Show(this, "Posting revaluation..."))
                {
                    DateTime revaluationDate = DateTime.Now;
                    decimal oldCost = _currentAsset.Cost;
                    decimal costDifference = newRevaluationCost - oldCost;
                    decimal oldAccumulatedDep = _currentAsset.AccumulatedDepreciation;

                    // Calculate new accumulated depreciation proportionately
                    decimal newAccumulatedDep = 0m;
                    if (oldCost > 0 && newRevaluationCost > 0)
                    {
                        newAccumulatedDep = RoundMoney(oldAccumulatedDep * (newRevaluationCost / oldCost));
                    }

                    decimal depreciationDifference = newAccumulatedDep - oldAccumulatedDep;

                    // Create journal entries for revaluation
                    AutoJVModel journal = new AutoJVModel
                    {
                        VoucherDate = revaluationDate,
                        ReferenceNo = _currentAsset.AssetCode,
                        Narration = $"Revaluation of asset {_currentAsset.AssetCode} - {_currentAsset.AssetName}",
                        ModuleName = "FixedAssets",
                        RefModule = "Revaluation",
                        RefId = _currentAsset.AssetId,
                        Lines = new List<JVLineModel>()
                    };

                    // Debit/Credit Asset Account with the cost difference
                    if (costDifference > 0)
                    {
                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = 0, // Placeholder
                            AccountName = _currentAsset.AssetName,
                            Debit = costDifference,
                            Credit = 0,
                            Narration = $"Upward revaluation of asset"
                        });

                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = 0,
                            AccountName = "Revaluation Reserve",
                            Debit = 0,
                            Credit = costDifference,
                            Narration = $"Upward revaluation reserve"
                        });
                    }
                    else
                    {
                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = 0,
                            AccountName = "Revaluation Reserve",
                            Debit = Math.Abs(costDifference),
                            Credit = 0,
                            Narration = $"Downward revaluation reserve"
                        });

                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = 0,
                            AccountName = _currentAsset.AssetName,
                            Debit = 0,
                            Credit = Math.Abs(costDifference),
                            Narration = $"Downward revaluation of asset"
                        });
                    }

                    // Handle accumulated depreciation adjustment if applicable
                    if (depreciationDifference != 0)
                    {
                        if (depreciationDifference > 0)
                        {
                            journal.Lines.Add(new JVLineModel
                            {
                                AccountId = 0,
                                AccountName = "Accumulated Depreciation",
                                Debit = depreciationDifference,
                                Credit = 0,
                                Narration = $"Adjust accumulated depreciation"
                            });

                            journal.Lines.Add(new JVLineModel
                            {
                                AccountId = 0,
                                AccountName = "Revaluation Reserve",
                                Debit = 0,
                                Credit = depreciationDifference,
                                Narration = $"Revaluation adjustment for accumulated depreciation"
                            });
                        }
                        else
                        {
                            journal.Lines.Add(new JVLineModel
                            {
                                AccountId = 0,
                                AccountName = "Revaluation Reserve",
                                Debit = Math.Abs(depreciationDifference),
                                Credit = 0,
                                Narration = $"Revaluation adjustment for accumulated depreciation"
                            });

                            journal.Lines.Add(new JVLineModel
                            {
                                AccountId = 0,
                                AccountName = "Accumulated Depreciation",
                                Debit = 0,
                                Credit = Math.Abs(depreciationDifference),
                                Narration = $"Adjust accumulated depreciation"
                            });
                        }
                    }

                    // Post the journal entry
                    JournalsBLL journalsBll = new JournalsBLL();
                    PostResult result = journalsBll.PostAutoJournalEntry(journal, UsersModal.logged_in_userid);

                    if (result != null && result.Success)
                    {
                        // Update asset with new cost
                        _currentAsset.Cost = newRevaluationCost;
                        _currentAsset.AccumulatedDepreciation = newAccumulatedDep;
                        _currentAsset.CurrentWDV = RoundMoney(newRevaluationCost - newAccumulatedDep);

                        // Note: In a full implementation, you would want to regenerate the depreciation schedule
                        // based on the new cost and remaining useful life

                        string message = string.Format(
                            "Revaluation posted successfully:\n" +
                            "Old Cost: PKR {0:N2}\n" +
                            "New Cost: PKR {1:N2}\n" +
                            "Cost Difference: PKR {2:N2}\n" +
                            "New Book Value: PKR {3:N2}\n" +
                            "Voucher ID: {4}",
                            oldCost, newRevaluationCost, costDifference, _currentAsset.CurrentWDV, result.VoucherId);

                        UiMessages.ShowInfo(message, "تم نشر إعادة التقييم بنجاح");
                        LoadAssetDetail();
                        UpdateSummaryCard();
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "Failed to post revaluation journal entry.",
                            "فشل في نشر قيد إعادة التقييم");
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error posting revaluation: {ex.Message}",
                    $"خطأ في نشر إعادة التقييم: {ex.Message}");
            }
        }

        private decimal RoundMoney(decimal value)
        {
            return Math.Round(value, 2);
        }

        private void TxtCost_TextChanged(object sender, EventArgs e)
        {
            // Auto-recalculate depreciation based on new cost
            if (decimal.TryParse(txtCost.Text, out var cost) && _currentAsset != null)
            {
                _currentAsset.Cost = cost;
                if (numUsefulLifeYears.Value > 0)
                {
                    var depAmount = cost / (int)numUsefulLifeYears.Value;
                    txtDepRate.Text = ((depAmount / cost) * 100).ToString("N2");
                }
                LoadDepreciationSchedulePreview();
            }
        }

        private void TxtDisposalProceeds_TextChanged(object sender, EventArgs e)
        {
            // Auto-calculate Gain/Loss on Disposal
            if (decimal.TryParse(txtDisposalProceeds.Text, out var proceeds) && _currentAsset != null)
            {
                var gainLoss = proceeds - _currentAsset.CurrentWDV;
                lblGainLossDisplay.Text = $"PKR {gainLoss:N2}";
                lblGainLossDisplay.ForeColor = gainLoss >= 0 ? Color.Green : Color.Red;
            }
        }

        private void DdlDepMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Recalculate depreciation rate based on method
            LoadDepreciationSchedulePreview();
        }

        #region Input Validation Methods

        private bool ValidateSearchInput()
        {
            // Search text is optional - any value is acceptable
            // Trim whitespace to avoid unnecessary filtering
            txtSearch.Text = txtSearch.Text.Trim();
            return true;
        }

        private bool ValidateAssetDetailsTab()
        {
            // Validate Asset Information tab
            if (string.IsNullOrWhiteSpace(txtAssetCode.Text))
            {
                UiMessages.ShowWarning("Asset code is required.", "رمز الأصل مطلوب");
                txtAssetCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAssetName.Text))
            {
                UiMessages.ShowWarning("Asset name is required.", "اسم الأصل مطلوب");
                txtAssetName.Focus();
                return false;
            }

            if (!decimal.TryParse(txtCost.Text, out var cost) || cost < 0)
            {
                UiMessages.ShowWarning("Please enter a valid cost amount.", "يرجى إدخال مبلغ تكلفة صحيح");
                txtCost.Focus();
                return false;
            }

            if (ddlAssetCategory.SelectedIndex < 0)
            {
                UiMessages.ShowWarning("Please select an asset category.", "يرجى تحديد فئة أصل");
                ddlAssetCategory.Focus();
                return false;
            }

            if (ddlAssetStatus.SelectedIndex < 0)
            {
                UiMessages.ShowWarning("Please select an asset status.", "يرجى تحديد حالة الأصل");
                ddlAssetStatus.Focus();
                return false;
            }

            return true;
        }

        private bool ValidateDepreciationSetupTab()
        {
            // Validate Depreciation Setup tab
            if (ddlDepMethod.SelectedIndex < 0)
            {
                UiMessages.ShowWarning("Please select a depreciation method.", "يرجى تحديد طريقة الاستهلاك");
                ddlDepMethod.Focus();
                return false;
            }

            if (numUsefulLifeYears.Value <= 0)
            {
                UiMessages.ShowWarning("Useful life in years must be greater than zero.", "يجب أن تكون السنوات المفيدة أكبر من صفر");
                numUsefulLifeYears.Focus();
                return false;
            }

            if (numUsefulLifeMonths.Value <= 0)
            {
                UiMessages.ShowWarning("Useful life in months must be greater than zero.", "يجب أن تكون الأشهر المفيدة أكبر من صفر");
                numUsefulLifeMonths.Focus();
                return false;
            }

            if (!decimal.TryParse(txtResidualValue.Text, out var residual) || residual < 0)
            {
                UiMessages.ShowWarning("Please enter a valid residual/salvage value.", "يرجى إدخال قيمة خردة صحيحة");
                txtResidualValue.Focus();
                return false;
            }

            if (!decimal.TryParse(txtDepRate.Text, out var depRate) || depRate < 0)
            {
                UiMessages.ShowWarning("Please enter a valid depreciation rate.", "يرجى إدخال معدل استهلاك صحيح");
                txtDepRate.Focus();
                return false;
            }

            return true;
        }

        private bool ValidateDisposalTab()
        {
            // Validate Disposal tab (only if disposal fields have values)
            if (dtDisposalDate.Value != null && !string.IsNullOrWhiteSpace(txtDisposalProceeds.Text))
            {
                if (!decimal.TryParse(txtDisposalProceeds.Text, out var proceeds) || proceeds < 0)
                {
                    UiMessages.ShowWarning("Please enter a valid disposal proceeds amount.", "يرجى إدخال مبلغ الاستبعاد صحيح");
                    txtDisposalProceeds.Focus();
                    return false;
                }
            }

            return true;
        }

        private bool ValidateFilterControls()
        {
            // Validate filter dropdowns - all have "All" as default so always valid
            return true;
        }

        #endregion
    }
}
