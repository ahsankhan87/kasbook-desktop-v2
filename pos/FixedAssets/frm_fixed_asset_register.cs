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
        // Maps account name → account_id for dep-account dropdowns
        private Dictionary<string, int> _accountNameToId = new Dictionary<string, int>();

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
                //AppTheme.Apply(this);

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

                // Wire up event handlers (idempotent: avoid duplicate subscriptions)
                dgvAssets.SelectionChanged -= DgvAssets_SelectionChanged;
                dgvAssets.SelectionChanged += DgvAssets_SelectionChanged;

                dgvAssets.CellClick -= DgvAssets_CellClick;
                dgvAssets.CellClick += DgvAssets_CellClick;

                txtSearch.TextChanged -= TxtSearch_TextChanged;
                txtSearch.TextChanged += TxtSearch_TextChanged;

                ddlStatus.SelectedIndexChanged -= FilterAssets;
                ddlStatus.SelectedIndexChanged += FilterAssets;

                ddlCategory.SelectedIndexChanged -= FilterAssets;
                ddlCategory.SelectedIndexChanged += FilterAssets;

                ddlLocation.SelectedIndexChanged -= FilterAssets;
                ddlLocation.SelectedIndexChanged += FilterAssets;

                btnAddAsset.Click -= BtnAddAsset_Click;
                btnAddAsset.Click += BtnAddAsset_Click;

                btnEditAsset.Click -= BtnEditAsset_Click;
                btnEditAsset.Click += BtnEditAsset_Click;

                btnDeleteAsset.Click -= BtnDeleteAsset_Click;
                btnDeleteAsset.Click += BtnDeleteAsset_Click;

                btnRefreshGrid.Click -= BtnRefreshGrid_Click;
                btnRefreshGrid.Click += BtnRefreshGrid_Click;

                btnSaveAssetInfo.Click -= BtnSaveAssetInfo_Click;
                btnSaveAssetInfo.Click += BtnSaveAssetInfo_Click;

                btnSaveDepSetup.Click -= BtnSaveDepSetup_Click;
                btnSaveDepSetup.Click += BtnSaveDepSetup_Click;

                btnRunDepreciation.Click -= BtnRunDepreciation_Click;
                btnRunDepreciation.Click += BtnRunDepreciation_Click;

                btnPostDisposal.Click -= BtnPostDisposal_Click;
                btnPostDisposal.Click += BtnPostDisposal_Click;

                btnPostRevaluation.Click -= BtnPostRevaluation_Click;
                btnPostRevaluation.Click += BtnPostRevaluation_Click;

                txtCost.TextChanged -= TxtCost_TextChanged;
                txtCost.TextChanged += TxtCost_TextChanged;

                txtDisposalProceeds.TextChanged -= TxtDisposalProceeds_TextChanged;
                txtDisposalProceeds.TextChanged += TxtDisposalProceeds_TextChanged;

                ddlDepMethod.SelectedIndexChanged -= DdlDepMethod_SelectedIndexChanged;
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
                    string supplierName = row["first_name"]?.ToString() ?? "";
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
                DataTable allAccounts = accountsBll.GetAccountsWithAccountType();

                ddlDepAccount.Items.Clear();
                ddlAccumDepAccount.Items.Clear();
                ddlRevaluationAccount.Items.Clear();
                ddlRevaluationAssetAccount.Items.Clear();
                ddlDisposalReceiptAccount.Items.Clear();
                ddlDisposalAssetAccount.Items.Clear();
                ddlDisposalGainAccount.Items.Clear();
                ddlDisposalLossAccount.Items.Clear();
                _accountNameToId.Clear();

                // Build name→id map for all accounts so we can look up IDs when saving/posting
                foreach (DataRow row in allAccounts.Rows)
                {
                    string accountName = row["name"]?.ToString() ?? "";
                    int accountId = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
                    if (!string.IsNullOrEmpty(accountName) && accountId > 0 && !_accountNameToId.ContainsKey(accountName))
                    {
                        _accountNameToId[accountName] = accountId;
                    }
                }

                foreach (DataRow row in allAccounts.Rows)
                {
                    string accountName = row["name"]?.ToString() ?? "";
                    string accountType = row["account_type"]?.ToString() ?? "";

                    if (string.IsNullOrEmpty(accountName))
                    {
                        continue;
                    }

                    // Disposal/Revaluation account dropdowns
                    ddlDisposalReceiptAccount.Items.Add(accountName);
                    ddlDisposalAssetAccount.Items.Add(accountName);
                    ddlRevaluationAssetAccount.Items.Add(accountName);

                    // Depreciation Expense accounts (expense type)
                    if (accountType == "5" || accountName.IndexOf("Depreciation Expense", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ddlDepAccount.Items.Add(accountName);
                    }
 
                    // Accumulated Depreciation / Contra-Asset accounts
                    if (accountName.IndexOf("Accumulated Depreciation", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        accountName.IndexOf("Accumulated", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ddlAccumDepAccount.Items.Add(accountName);
                    }

                    // Revaluation Reserve accounts
                    if (accountName.IndexOf("Revaluation", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        accountName.IndexOf("Reserve", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ddlRevaluationAccount.Items.Add(accountName);
                    }

                    // Gain / loss accounts for disposal
                    if (accountType == "4" || accountName.IndexOf("Gain", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ddlDisposalGainAccount.Items.Add(accountName);
                    }

                    if (accountType == "5" || accountName.IndexOf("Loss", StringComparison.OrdinalIgnoreCase) >= 0 || accountName.IndexOf("Expense", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ddlDisposalLossAccount.Items.Add(accountName);
                    }
                }

                // Set defaults if available
                if (ddlDepAccount.Items.Count > 0) ddlDepAccount.SelectedIndex = 0;
                if (ddlAccumDepAccount.Items.Count > 0) ddlAccumDepAccount.SelectedIndex = 0;
                if (ddlRevaluationAccount.Items.Count > 0) ddlRevaluationAccount.SelectedIndex = 0;
                if (ddlRevaluationAssetAccount.Items.Count > 0) ddlRevaluationAssetAccount.SelectedIndex = 0;
                if (ddlDisposalReceiptAccount.Items.Count > 0) ddlDisposalReceiptAccount.SelectedIndex = 0;
                if (ddlDisposalAssetAccount.Items.Count > 0) ddlDisposalAssetAccount.SelectedIndex = 0;
                if (ddlDisposalGainAccount.Items.Count > 0) ddlDisposalGainAccount.SelectedIndex = 0;
                if (ddlDisposalLossAccount.Items.Count > 0) ddlDisposalLossAccount.SelectedIndex = 0;

                SelectFirstMatchingAccount(ddlDisposalReceiptAccount, "Cash", "Bank");
                SelectFirstMatchingAccount(ddlDisposalAssetAccount, "Fixed Asset", "Property", "Plant", "Equipment", "Vehicle", "Machinery", "Furniture");
                SelectFirstMatchingAccount(ddlDisposalGainAccount, "Gain on Disposal", "Disposal Gain", "Gain");
                SelectFirstMatchingAccount(ddlDisposalLossAccount, "Loss on Disposal", "Disposal Loss", "Loss");
                SelectFirstMatchingAccount(ddlRevaluationAssetAccount, "Fixed Asset", "Property", "Plant", "Equipment", "Vehicle", "Machinery", "Furniture");
                SelectFirstMatchingAccount(ddlRevaluationAccount, "Revaluation Reserve", "Revaluation", "Reserve");
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
            LoadSelectedAssetFromGrid();
        }

        private void DgvAssets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadSelectedAssetFromGrid();
        }

        private void LoadSelectedAssetFromGrid()
        {
            if (dgvAssets.SelectedRows.Count <= 0)
            {
                return;
            }

            var selectedRow = dgvAssets.SelectedRows[0];
            var assetCode = selectedRow.Cells[0].Value?.ToString();
            if (string.IsNullOrEmpty(assetCode))
            {
                return;
            }

            _currentAsset = _allAssets.FirstOrDefault(a => a.AssetCode == assetCode);
            if (_currentAsset == null)
            {
                return;
            }

            LoadAssetDetail();
            UpdateSummaryCard();
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
            numUsefulLifeYears.Value = (decimal)Math.Min(_currentAsset.UsefulLifeYears, numUsefulLifeYears.Maximum);
            numUsefulLifeMonths.Value = Math.Min(_currentAsset.UsefulLifeMonths, (int)numUsefulLifeMonths.Maximum);
            txtResidualValue.Text = _currentAsset.ResidualValue.ToString("N2");
            txtDepRate.Text = _currentAsset.DepreciationRate.ToString("N2");
            dtStartDepreciationDate.Value = _currentAsset.StartDepreciationFrom;

            // Pre-select dep accounts based on the asset's stored account IDs
            SelectDropdownByAccountId(ddlDepAccount, _currentAsset.DepAccountId);
            SelectDropdownByAccountId(ddlAccumDepAccount, _currentAsset.AccumDepAccountId);

            // Disposal/Revaluation posting account defaults
            SelectFirstMatchingAccount(ddlDisposalAssetAccount, _currentAsset.AssetName, _currentAsset.CategoryName, "Fixed Asset");
            SelectFirstMatchingAccount(ddlRevaluationAssetAccount, _currentAsset.AssetName, _currentAsset.CategoryName, "Fixed Asset");
            txtNewRevaluedAmount.Text = _currentAsset.Cost.ToString("N2");

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

        private void LoadDepreciationSchedulePreview(decimal? previewCost = null)
        {
            if (_currentAsset == null) return;

            dgvDepSchedule.Rows.Clear();

            try
            {
                var scheduleAsset = _currentAsset;

                if (previewCost.HasValue)
                {
                    scheduleAsset = new FixedAssetModel
                    {
                        AssetId = _currentAsset.AssetId,
                        AssetCode = _currentAsset.AssetCode,
                        AssetName = _currentAsset.AssetName,
                        PurchaseDate = _currentAsset.PurchaseDate,
                        Cost = previewCost.Value,
                        ResidualValue = _currentAsset.ResidualValue,
                        UsefulLifeMonths = _currentAsset.UsefulLifeMonths,
                        DepMethod = _currentAsset.DepMethod,
                        DepRate = _currentAsset.DepRate,
                        CurrentWDV = _currentAsset.CurrentWDV,
                        AccumulatedDepreciation = _currentAsset.AccumulatedDepreciation
                    };
                }

                // Generate the full depreciation schedule for this asset
                List<DepScheduleLine> schedule = _depEngine.GenerateDepreciationSchedule(scheduleAsset);

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
                        line.DepMethod,
                        line.DepreciationAmount.ToString("N2"),
                        line.ClosingWDV.ToString("N2"),
                        line.VoucherId.HasValue ? line.VoucherId.Value.ToString() : "-"
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

        private void SelectDropdownByAccountId(ComboBox ddl, int accountId)
        {
            if (accountId <= 0) return;
            foreach (var pair in _accountNameToId)
            {
                if (pair.Value == accountId)
                {
                    int idx = ddl.Items.IndexOf(pair.Key);
                    if (idx >= 0) ddl.SelectedIndex = idx;
                    return;
                }
            }
        }

        private void SelectFirstMatchingAccount(ComboBox ddl, params string[] keywords)
        {
            if (ddl == null || ddl.Items.Count == 0 || keywords == null || keywords.Length == 0)
            {
                return;
            }

            for (int i = 0; i < ddl.Items.Count; i++)
            {
                string accountName = ddl.Items[i]?.ToString() ?? string.Empty;
                foreach (string keyword in keywords)
                {
                    if (!string.IsNullOrWhiteSpace(keyword) && accountName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ddl.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        private bool TryGetSelectedAccountId(ComboBox ddl, out int accountId)
        {
            accountId = 0;
            string selectedName = ddl?.SelectedItem?.ToString();
            return !string.IsNullOrWhiteSpace(selectedName)
                   && _accountNameToId.TryGetValue(selectedName, out accountId)
                   && accountId > 0;
        }

        private void BtnSaveDepSetup_Click(object sender, EventArgs e)
        {
            if (_currentAsset == null)
            {
                UiMessages.ShowWarning("Please select an asset first.", "يرجى تحديد أصل أولا");
                return;
            }

            string depAccountName = ddlDepAccount.SelectedItem?.ToString();
            string accumDepAccountName = ddlAccumDepAccount.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(depAccountName) || !_accountNameToId.ContainsKey(depAccountName))
            {
                UiMessages.ShowWarning(
                    "Please select a valid Depreciation Expense account.",
                    "يرجى تحديد حساب مصروف استهلاك صحيح");
                ddlDepAccount.Focus();
                return;
            }

            if (string.IsNullOrEmpty(accumDepAccountName) || !_accountNameToId.ContainsKey(accumDepAccountName))
            {
                UiMessages.ShowWarning(
                    "Please select a valid Accumulated Depreciation account.",
                    "يرجى تحديد حساب مجمع استهلاك صحيح");
                ddlAccumDepAccount.Focus();
                return;
            }

            try
            {
                using (BusyScope.Show(this, "Saving depreciation setup..."))
                {
                    int depAccountId = _accountNameToId[depAccountName];
                    int accumDepAccountId = _accountNameToId[accumDepAccountName];

                    _assetBLL.UpdateAssetDepreciationAccounts(_currentAsset.AssetId, depAccountId, accumDepAccountId);

                    // Update in-memory model so the engine picks up the new IDs immediately
                    _currentAsset.DepAccountId = depAccountId;
                    _currentAsset.AccumDepAccountId = accumDepAccountId;

                    // Reflect change in the master list
                    var master = _allAssets.FirstOrDefault(a => a.AssetId == _currentAsset.AssetId);
                    if (master != null)
                    {
                        master.DepAccountId = depAccountId;
                        master.AccumDepAccountId = accumDepAccountId;
                    }
                }

                UiMessages.ShowInfo(
                    "Depreciation accounts saved. You can now run depreciation for this asset.",
                    "تم حفظ حسابات الاستهلاك. يمكنك الآن تشغيل الاستهلاك لهذا الأصل",
                    "Saved", "تم الحفظ");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error saving depreciation setup: {ex.Message}",
                    $"خطأ في حفظ إعداد الاستهلاك: {ex.Message}");
            }
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

        private void BtnEditAsset_Click(object sender, EventArgs e)
        {
            if (_currentAsset == null)
            {
                UiMessages.ShowWarning("Please select an asset to edit.", "يرجى تحديد أصل للتعديل");
                return;
            }

            try
            {
                var editForm = new frm_addFixedAsset(_currentAsset);
                if (editForm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadAssets();
                    // Re-select the same asset after reload
                    _currentAsset = _allAssets.FirstOrDefault(a => a.AssetId == _currentAsset.AssetId);
                    if (_currentAsset != null)
                    {
                        LoadAssetDetail();
                        UpdateSummaryCard();
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void BtnDeleteAsset_Click(object sender, EventArgs e)
        {
            if (_currentAsset == null)
            {
                UiMessages.ShowWarning("Please select an asset to delete.", "يرجى تحديد أصل للحذف");
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this asset? It will only be deleted if no depreciation/accounting transactions exist.",
                "هل تريد حذف هذا الأصل؟ سيتم الحذف فقط إذا لم توجد معاملات استهلاك/محاسبة مرتبطة به.",
                "Confirm Delete",
                "تأكيد الحذف");

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (BusyScope.Show(this, "Deleting asset..."))
                {
                    string reason;
                    bool deleted = _assetBLL.TryDeleteAssetIfNoTransactions(_currentAsset.AssetId, out reason);
                    if (!deleted)
                    {
                        UiMessages.ShowWarning(
                            string.IsNullOrWhiteSpace(reason)
                                ? "Asset cannot be deleted because it has linked transactions."
                                : reason,
                            "لا يمكن حذف الأصل لأنه يحتوي على معاملات مرتبطة.");
                        return;
                    }
                }

                UiMessages.ShowInfo("Asset deleted successfully.", "تم حذف الأصل بنجاح", "Deleted", "تم الحذف");

                _currentAsset = null;
                LoadAssets();
                UpdateSummaryCard();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void BtnRefreshGrid_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, "Refreshing assets..."))
                {
                    LoadAssets();
                    FilterAssets(null, null);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Error", "خطأ");
            }
        }

        private void BtnSaveAssetInfo_Click(object sender, EventArgs e)
        {
            if (_currentAsset == null)
            {
                UiMessages.ShowWarning("Please select an asset first.", "يرجى تحديد أصل أولا");
                return;
            }

            if (!ValidateAssetDetailsTab())
            {
                return;
            }

            try
            {
                using (BusyScope.Show(this, "Saving asset information..."))
                {
                    int? locationId = null;
                    var selectedLocationName = ddlAssetLocation.SelectedItem?.ToString();
                    if (!string.IsNullOrWhiteSpace(selectedLocationName))
                    {
                        var selectedLocation = _assetBLL.GetLocations(activeOnly: false)
                            .FirstOrDefault(l => string.Equals(l.LocationName, selectedLocationName, StringComparison.OrdinalIgnoreCase));
                        locationId = selectedLocation != null ? (int?)selectedLocation.LocationId : null;
                    }

                    _assetBLL.UpdateAssetInfoTabDetails(
                        _currentAsset.AssetId,
                        txtAssetName.Text,
                        txtDescription.Text,
                        dtPurchaseDate.Value.Date,
                        ddlSupplier.Text,
                        txtInvoiceNo.Text,
                        locationId,
                        txtSerialNumber.Text,
                        txtModelNumber.Text,
                        _currentAsset.Status);
                }

                LoadAssets();
                _currentAsset = _allAssets.FirstOrDefault(a => a.AssetId == _currentAsset.AssetId);
                if (_currentAsset != null)
                {
                    LoadAssetDetail();
                    UpdateSummaryCard();
                }

                UiMessages.ShowInfo("Asset information updated successfully.", "تم تحديث معلومات الأصل بنجاح", "Saved", "تم الحفظ");
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

                        if (result.Messages != null && result.Messages.Count > 0)
                        {
                            message += "\n\nDetails:\n" + string.Join("\n", result.Messages);
                        }

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

            // Prevent duplicate disposal
            if (_currentAsset.IsDisposed ||
                string.Equals(_currentAsset.Status, "Disposed", StringComparison.OrdinalIgnoreCase) ||
                _currentAsset.DisposalDate.HasValue)
            {
                UiMessages.ShowWarning(
                    "This asset is already disposed. Disposal can only be posted once.",
                    "تم استبعاد هذا الأصل مسبقاً. يمكن تسجيل الاستبعاد مرة واحدة فقط.");
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

            if (_currentAsset.AccumDepAccountId <= 0)
            {
                UiMessages.ShowWarning(
                    "Please save depreciation setup first (Accumulated Depreciation account is required).",
                    "يرجى حفظ إعدادات الاستهلاك أولاً (حساب مجمع الاستهلاك مطلوب)");
                return;
            }

            if (!TryGetSelectedAccountId(ddlDisposalAssetAccount, out int assetAccountId))
            {
                UiMessages.ShowWarning("Please select a valid Asset account.", "يرجى تحديد حساب أصل صحيح");
                ddlDisposalAssetAccount.Focus();
                return;
            }

            int receiptAccountId = 0;
            if (disposalProceeds > 0m && !TryGetSelectedAccountId(ddlDisposalReceiptAccount, out receiptAccountId))
            {
                UiMessages.ShowWarning("Please select a valid Receipt account.", "يرجى تحديد حساب تحصيل صحيح");
                ddlDisposalReceiptAccount.Focus();
                return;
            }

            int gainAccountId = 0;
            int lossAccountId = 0;

            try
            {
                using (BusyScope.Show(this, "Posting disposal..."))
                {
                    DateTime disposalDate = dtDisposalDate.Value.Date;

                    // Calculate disposal amounts
                    decimal assetCost = RoundMoney(Math.Max(0m, _currentAsset.Cost));
                    decimal bookValue = RoundMoney(Math.Max(0m, _currentAsset.CurrentWDV));
                    decimal accumulatedDepreciation = RoundMoney(Math.Max(0m, assetCost - bookValue));
                    decimal gainLoss = RoundMoney(disposalProceeds - bookValue);

                    if (gainLoss > 0m && !TryGetSelectedAccountId(ddlDisposalGainAccount, out gainAccountId))
                    {
                        UiMessages.ShowWarning("Please select a valid Gain account.", "يرجى تحديد حساب أرباح صحيح");
                        ddlDisposalGainAccount.Focus();
                        return;
                    }

                    if (gainLoss < 0m && !TryGetSelectedAccountId(ddlDisposalLossAccount, out lossAccountId))
                    {
                        UiMessages.ShowWarning("Please select a valid Loss account.", "يرجى تحديد حساب خسائر صحيح");
                        ddlDisposalLossAccount.Focus();
                        return;
                    }

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

                    // Debit receipt account by proceeds (if any)
                    if (disposalProceeds > 0m)
                    {
                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = receiptAccountId,
                            AccountName = ddlDisposalReceiptAccount.SelectedItem?.ToString() ?? "",
                            Debit = disposalProceeds,
                            Credit = 0,
                            Narration = $"Receipt from disposal of {_currentAsset.AssetName}"
                        });
                    }

                    // Debit accumulated depreciation
                    if (accumulatedDepreciation > 0m)
                    {
                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = _currentAsset.AccumDepAccountId,
                            AccountName = ddlAccumDepAccount.SelectedItem?.ToString() ?? "Accumulated Depreciation",
                            Debit = accumulatedDepreciation,
                            Credit = 0,
                            Narration = "Accumulated depreciation reversal on disposal"
                        });
                    }

                    // Credit fixed asset at historical cost
                    if (assetCost > 0m)
                    {
                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = assetAccountId,
                            AccountName = ddlDisposalAssetAccount.SelectedItem?.ToString() ?? _currentAsset.AssetName,
                            Debit = 0,
                            Credit = assetCost,
                            Narration = "Derecognition of fixed asset cost"
                        });
                    }

                    // Gain or loss line
                    if (gainLoss > 0m)
                    {
                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = gainAccountId,
                            AccountName = ddlDisposalGainAccount.SelectedItem?.ToString() ?? "Gain on Disposal",
                            Debit = 0,
                            Credit = gainLoss,
                            Narration = "Gain on disposal of asset"
                        });
                    }
                    else if (gainLoss < 0m)
                    {
                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = lossAccountId,
                            AccountName = ddlDisposalLossAccount.SelectedItem?.ToString() ?? "Loss on Disposal",
                            Debit = Math.Abs(gainLoss),
                            Credit = 0,
                            Narration = "Loss on disposal of asset"
                        });
                    }

                    // Post the journal entry
                    JournalsBLL journalsBll = new JournalsBLL();
                    PostResult result = journalsBll.PostAutoJournalEntry(journal, UsersModal.logged_in_userid);

                    if (result != null && result.Success)
                    {
                        // Determine disposal method from dropdown (default Write-Off if nothing selected)
                        string disposalMethod = ddlDisposalMethod.SelectedItem?.ToString() ?? "Write-Off";

                        // First update accumulated depreciation / WDV state
                        _assetBLL.UpdateAssetDepreciationState(
                            _currentAsset.AssetId,
                            accumulatedDepreciation,
                            0, // WDV becomes 0 after disposal
                            disposalDate,
                            "Disposed");

                        // Then insert into fa_asset_disposals and force final disposal flags/status in fa_assets
                        _assetBLL.RecordDisposal(
                            _currentAsset.AssetId,
                            disposalDate,
                            disposalMethod,
                            disposalProceeds,
                            assetCost,
                            accumulatedDepreciation,
                            bookValue,
                            $"Disposal voucher {result.VoucherNo}");

                        // Update in-memory model
                        _currentAsset.DisposalDate = disposalDate;
                        _currentAsset.DisposalProceeds = disposalProceeds;
                        _currentAsset.Status = "Disposed";

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

            // Prevent duplicate revaluation
            if (_assetBLL.HasAssetRevaluation(_currentAsset.AssetId))
            {
                UiMessages.ShowWarning(
                    "This asset is already revalued. Revaluation can only be posted once.",
                    "تمت إعادة تقييم هذا الأصل مسبقاً. يمكن تسجيل إعادة التقييم مرة واحدة فقط.");
                return;
            }

            DateTime revaluationDate = dtRevaluationDate.Value.Date;

            // Validate revaluation amount
            if (!decimal.TryParse(txtNewRevaluedAmount.Text, out var newRevaluationCost) || newRevaluationCost < 0)
            {
                UiMessages.ShowWarning("Please enter a valid revaluation amount.", "يرجى إدخال مبلغ إعادة تقييم صحيح");
                txtNewRevaluedAmount.Focus();
                return;
            }

            // Check if there's a change in cost
            if (newRevaluationCost == _currentAsset.Cost)
            {
                UiMessages.ShowWarning("The revaluation amount must be different from current cost.", "يجب أن يكون مبلغ إعادة التقييم مختلفاً عن التكلفة الحالية");
                return;
            }

            if (!TryGetSelectedAccountId(ddlRevaluationAssetAccount, out int assetAccountId))
            {
                UiMessages.ShowWarning("Please select a valid Revaluation Asset account.", "يرجى تحديد حساب أصل إعادة تقييم صحيح");
                ddlRevaluationAssetAccount.Focus();
                return;
            }

            if (!TryGetSelectedAccountId(ddlRevaluationAccount, out int revaluationReserveAccountId))
            {
                UiMessages.ShowWarning("Please select a valid Revaluation Reserve account.", "يرجى تحديد حساب احتياطي إعادة تقييم صحيح");
                ddlRevaluationAccount.Focus();
                return;
            }

            if (_currentAsset.AccumDepAccountId <= 0)
            {
                UiMessages.ShowWarning(
                    "Please save depreciation setup first (Accumulated Depreciation account is required).",
                    "يرجى حفظ إعدادات الاستهلاك أولاً (حساب مجمع الاستهلاك مطلوب)");
                return;
            }

            try
            {
                using (BusyScope.Show(this, "Posting revaluation..."))
                {
                    decimal oldCost = RoundMoney(Math.Max(0m, _currentAsset.Cost));
                    decimal costDifference = RoundMoney(newRevaluationCost - oldCost);
                    decimal oldAccumulatedDep = RoundMoney(Math.Max(0m, _currentAsset.AccumulatedDepreciation));

                    // Calculate new accumulated depreciation proportionately
                    decimal newAccumulatedDep = 0m;
                    if (oldCost > 0m && newRevaluationCost > 0m)
                    {
                        newAccumulatedDep = RoundMoney(oldAccumulatedDep * (newRevaluationCost / oldCost));
                    }

                    decimal depreciationDifference = RoundMoney(newAccumulatedDep - oldAccumulatedDep);

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

                    // Revalue cost: asset vs revaluation reserve
                    if (costDifference > 0m)
                    {
                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = assetAccountId,
                            AccountName = ddlRevaluationAssetAccount.SelectedItem?.ToString() ?? _currentAsset.AssetName,
                            Debit = costDifference,
                            Credit = 0m,
                            Narration = "Upward revaluation of asset cost"
                        });

                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = revaluationReserveAccountId,
                            AccountName = ddlRevaluationAccount.SelectedItem?.ToString() ?? "Revaluation Reserve",
                            Debit = 0m,
                            Credit = costDifference,
                            Narration = "Upward revaluation reserve"
                        });
                    }
                    else
                    {
                        decimal amount = Math.Abs(costDifference);

                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = revaluationReserveAccountId,
                            AccountName = ddlRevaluationAccount.SelectedItem?.ToString() ?? "Revaluation Reserve",
                            Debit = amount,
                            Credit = 0m,
                            Narration = "Downward revaluation reserve"
                        });

                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = assetAccountId,
                            AccountName = ddlRevaluationAssetAccount.SelectedItem?.ToString() ?? _currentAsset.AssetName,
                            Debit = 0m,
                            Credit = amount,
                            Narration = "Downward revaluation of asset cost"
                        });
                    }

                    // Revalue accumulated depreciation vs reserve
                    if (depreciationDifference > 0m)
                    {
                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = revaluationReserveAccountId,
                            AccountName = ddlRevaluationAccount.SelectedItem?.ToString() ?? "Revaluation Reserve",
                            Debit = depreciationDifference,
                            Credit = 0m,
                            Narration = "Revaluation adjustment for accumulated depreciation"
                        });

                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = _currentAsset.AccumDepAccountId,
                            AccountName = ddlAccumDepAccount.SelectedItem?.ToString() ?? "Accumulated Depreciation",
                            Debit = 0m,
                            Credit = depreciationDifference,
                            Narration = "Increase accumulated depreciation after revaluation"
                        });
                    }
                    else if (depreciationDifference < 0m)
                    {
                        decimal amount = Math.Abs(depreciationDifference);

                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = _currentAsset.AccumDepAccountId,
                            AccountName = ddlAccumDepAccount.SelectedItem?.ToString() ?? "Accumulated Depreciation",
                            Debit = amount,
                            Credit = 0m,
                            Narration = "Decrease accumulated depreciation after revaluation"
                        });

                        journal.Lines.Add(new JVLineModel
                        {
                            AccountId = revaluationReserveAccountId,
                            AccountName = ddlRevaluationAccount.SelectedItem?.ToString() ?? "Revaluation Reserve",
                            Debit = 0m,
                            Credit = amount,
                            Narration = "Revaluation adjustment for accumulated depreciation"
                        });
                    }

                    // Post the journal entry
                    JournalsBLL journalsBll = new JournalsBLL();
                    PostResult result = journalsBll.PostAutoJournalEntry(journal, UsersModal.logged_in_userid);

                    if (result != null && result.Success)
                    {
                        decimal oldWdv = RoundMoney(oldCost - oldAccumulatedDep);
                        decimal newWdv = RoundMoney(newRevaluationCost - newAccumulatedDep);

                        // Save to revaluation table and update fa_assets values
                        _assetBLL.RecordRevaluation(
                            _currentAsset.AssetId,
                            revaluationDate,
                            oldCost,
                            newRevaluationCost,
                            oldAccumulatedDep,
                            newAccumulatedDep,
                            oldWdv,
                            newWdv,
                            $"Revaluation voucher {result.VoucherNo}");

                        // Update in-memory asset values
                        _currentAsset.Cost = newRevaluationCost;
                        _currentAsset.AccumulatedDepreciation = newAccumulatedDep;
                        _currentAsset.CurrentWDV = newWdv;

                        string message = string.Format(
                            "Revaluation posted successfully:\n" +
                            "Revaluation Date: {0:yyyy-MM-dd}\n" +
                            "Old Cost: PKR {1:N2}\n" +
                            "New Cost: PKR {2:N2}\n" +
                            "Cost Difference: PKR {3:N2}\n" +
                            "New Book Value: PKR {4:N2}\n" +
                            "Voucher ID: {5}",
                            revaluationDate, oldCost, newRevaluationCost, costDifference, _currentAsset.CurrentWDV, result.VoucherId);

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
                if (numUsefulLifeYears.Value > 0)
                {
                    var depAmount = cost / (int)numUsefulLifeYears.Value;
                    txtDepRate.Text = ((depAmount / cost) * 100).ToString("N2");
                }

                // Use edited cost for preview only; keep persisted asset cost unchanged
                // until explicit save/revaluation action is posted.
                LoadDepreciationSchedulePreview(cost);
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
