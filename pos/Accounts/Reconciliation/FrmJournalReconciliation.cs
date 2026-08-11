using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;
using pos.Security.Authorization;
using System.Text;

namespace pos.Accounts.Reconciliation
{
    public partial class FrmJournalReconciliation : Form
    {
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private readonly UserIdentity _currentUser = AppSecurityContext.User;
        private readonly AccountsBLL _accountsBll = new AccountsBLL();
        private readonly JournalsBLL _journalsBll = new JournalsBLL();
        private readonly SalesBLL _salesBll = new SalesBLL();
        private readonly PurchasesBLL _purchasesBll = new PurchasesBLL();

        private DataTable _journalEntries = new DataTable();
        private DataTable _salesEntries = new DataTable();
        private DataTable _purchaseEntries = new DataTable();
        private DataTable _lookupEntries = new DataTable();
        private DataTable _unreconciled = new DataTable();

        private Dictionary<string, bool> _reconciliationStatus = new Dictionary<string, bool>();

        public FrmJournalReconciliation()
        {
            InitializeComponent();
            WireEvents();
        }

        private void WireEvents()
        {
            Load += FrmJournalReconciliation_Load;
            btnLoad.Click += (s, e) => LoadReconciliationData();
            btnReconcile.Click += (s, e) => ReconcileSelectedEntries();
            btnReverseReconciliation.Click += (s, e) => ReverseReconciliation();
            btnExportReport.Click += (s, e) => ExportReconciliationReport();
            btnRefresh.Click += (s, e) => LoadReconciliationData();
            btnAdvancedMatch.Click += (s, e) => ShowAdvancedMatchingDialog();

            dgvJournalEntries.CellClick += DgvJournalEntries_CellClick;
            dgvSalesEntries.CellClick += DgvSalesEntries_CellClick;
            dgvPurchaseEntries.CellClick += DgvPurchaseEntries_CellClick;
            dgvUnreconciled.CellClick += DgvUnreconciled_CellClick;

            // Format status column to show friendly text
            dgvJournalEntries.CellFormatting += (s, e) =>
            {
                if (e.ColumnIndex == dgvJournalEntries.Columns["colStatus"].Index && e.Value != null)
                {
                    int value = Convert.ToInt32(e.Value);
                    e.Value = value == 1 ? "Reconciled" : "Unreconciled";
                    e.FormattingApplied = true;
                }
            };

            // Note: Account filter is disabled because journal entries span multiple accounts.
            // Only status, date range, and search filters are applied.
            // cmbAccountFilter.SelectedIndexChanged += (s, e) => ApplyFilters();

            cmbStatusFilter.SelectedIndexChanged += (s, e) => ApplyFilters();
            dtpFromDate.ValueChanged += (s, e) => ApplyFilters();
            dtpToDate.ValueChanged += (s, e) => ApplyFilters();

            txtSearchJournal.TextChanged += (s, e) => FilterGridBySearch(dgvJournalEntries, txtSearchJournal.Text);
            txtSearchSales.TextChanged += (s, e) => FilterGridBySearch(dgvSalesEntries, txtSearchSales.Text);
            txtSearchPurchase.TextChanged += (s, e) => FilterGridBySearch(dgvPurchaseEntries, txtSearchPurchase.Text);
        }

        private void FrmJournalReconciliation_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            ConfigureGrids();
            ConfigureControls();
            LoadFilterData();
            LoadReconciliationData();
        }

        private void ConfigureGrids()
        {
            // Journal Entries Grid
            dgvJournalEntries.AutoGenerateColumns = false;
            dgvJournalEntries.AllowUserToAddRows = false;
            dgvJournalEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvJournalEntries.MultiSelect = true;

            // Sales Entries Grid
            dgvSalesEntries.AutoGenerateColumns = false;
            dgvSalesEntries.AllowUserToAddRows = false;
            dgvSalesEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Purchase Entries Grid
            dgvPurchaseEntries.AutoGenerateColumns = false;
            dgvPurchaseEntries.AllowUserToAddRows = false;
            dgvPurchaseEntries.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Unreconciled Grid
            dgvUnreconciled.AutoGenerateColumns = false;
            dgvUnreconciled.AllowUserToAddRows = false;
            dgvUnreconciled.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUnreconciled.MultiSelect = true;

            ApplyNumericFormatting(dgvJournalEntries);
            ApplyNumericFormatting(dgvSalesEntries);
            ApplyNumericFormatting(dgvPurchaseEntries);
            ApplyNumericFormatting(dgvUnreconciled);
        }

        private void ApplyNumericFormatting(DataGridView grid)
        {
            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (col.Name.Contains("Amount") || col.Name.Contains("Debit") || col.Name.Contains("Credit") || col.Name.Contains("Total"))
                {
                    col.DefaultCellStyle.Format = "N2";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        private void ConfigureControls()
        {
            dtpFromDate.Value = DateTime.Today.AddMonths(-1);
            dtpToDate.Value = DateTime.Today;
            cmbStatusFilter.Items.AddRange(new[] { "All", "Reconciled", "Unreconciled", "Pending" });
            cmbStatusFilter.SelectedIndex = 0;
        }

        private void LoadFilterData()
        {
            try
            {
                // NOTE: Account filtering is not applicable for journal reconciliation
                // because journal entries span multiple accounts across line items.
                // The reconciliation view works at the header level (one row per journal entry).
                // Individual line-level filtering should be done in the detailed view,
                // or users can search by invoice number instead.

                // For now, populate the account dropdown for potential future use
                // but it won't filter the reconciliation grid
                DataTable accounts = _accountsBll.GetAll();
                cmbAccountFilter.DataSource = accounts;
                cmbAccountFilter.DisplayMember = "name";
                cmbAccountFilter.ValueMember = "id";

                if (cmbAccountFilter.Items.Count > 0)
                {
                    cmbAccountFilter.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error loading accounts: {ex.Message}", "خطأ في تحميل الحسابات");
            }
        }

        private void LoadReconciliationData()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                // Load journal entries
                _journalEntries = _journalsBll.GetJournalEntriesByDateRange(dtpFromDate.Value, dtpToDate.Value);
                BindJournalEntries(_journalEntries);

                // Load sales entries
                _salesEntries = _salesBll.GetSalesEntriesByDateRange(dtpFromDate.Value, dtpToDate.Value);
                BindSalesEntries(_salesEntries);

                // Load purchase entries
                _purchaseEntries = _purchasesBll.GetPurchaseEntriesByDateRange(dtpFromDate.Value, dtpToDate.Value);
                BindPurchaseEntries(_purchaseEntries);

                // Load unreconciled entries
                LoadUnreconciledEntries();

                RefreshSummary();
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UiMessages.ShowError($"Error loading data: {ex.Message}", "خطأ في تحميل البيانات");
            }
        }

        private void BindJournalEntries(DataTable dt)
        {
            try
            {
                // Debug: Log column names for troubleshooting
                var columnNames = string.Join(", ", dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                System.Diagnostics.Debug.WriteLine($"Journal DataTable columns: {columnNames}");

                dgvJournalEntries.DataSource = dt;
                UpdateReconciliationStatus();
                ApplyConditionalFormatting(dgvJournalEntries);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error binding journal entries: {ex.Message}", "خطأ في ربط البيانات");
            }
        }

        private void BindSalesEntries(DataTable dt)
        {
            dgvSalesEntries.DataSource = dt;
            ApplyConditionalFormatting(dgvSalesEntries);
        }

        private void BindPurchaseEntries(DataTable dt)
        {
            dgvPurchaseEntries.DataSource = dt;
            ApplyConditionalFormatting(dgvPurchaseEntries);
        }

        private void LoadUnreconciledEntries()
        {
            try
            {
                _unreconciled = _journalsBll.GetUnreconciledEntries(dtpFromDate.Value, dtpToDate.Value);
                dgvUnreconciled.DataSource = _unreconciled;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error loading unreconciled entries: {ex.Message}", "خطأ في تحميل البيانات غير المسددة");
            }
        }

        private void ApplyConditionalFormatting(DataGridView grid)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Cells.Count > 0)
                {
                    try
                    {
                        // Check if this is the journal grid (has colStatus with is_reconciled values)
                        if (grid == dgvJournalEntries && row.Cells.Contains(row.Cells["colStatus"]))
                        {
                            var statusCell = row.Cells["colStatus"];
                            string statusText = "";

                            // is_reconciled is stored as int (0 or 1)
                            if (statusCell.Value != null && statusCell.Value != DBNull.Value)
                            {
                                int statusValue = Convert.ToInt32(statusCell.Value);
                                statusText = statusValue == 1 ? "Reconciled" : "Unreconciled";
                            }
                            else
                            {
                                statusText = "Unreconciled";
                            }

                            if (statusText == "Reconciled")
                            {
                                row.DefaultCellStyle.BackColor = Color.LightGreen;
                            }
                            else if (statusText == "Pending")
                            {
                                row.DefaultCellStyle.BackColor = Color.LightYellow;
                            }
                            else
                            {
                                row.DefaultCellStyle.BackColor = Color.White;
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        private void UpdateReconciliationStatus()
        {
            _reconciliationStatus.Clear();

            foreach (DataRow row in _journalEntries.Rows)
            {
                string invoiceNo = row["invoice_no"]?.ToString() ?? "";
                if (!string.IsNullOrEmpty(invoiceNo))
                {
                    // is_reconciled is a SQL bit (int), not bool
                    bool isReconciled = row["is_reconciled"] != DBNull.Value && Convert.ToInt32(row["is_reconciled"]) != 0;
                    _reconciliationStatus[invoiceNo] = isReconciled;
                }
            }
        }

        private void ApplyFilters()
        {
            try
            {
                // Instead of using RowFilter with date comparisons, filter the data before binding
                RefreshGridsWithFilters();
                RefreshSummary();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Filter error: {ex.Message}", "خطأ في التصفية");
            }
        }

        private void RefreshGridsWithFilters()
        {
            try
            {
                // Filter journal entries
                if (_journalEntries != null && _journalEntries.Rows.Count > 0)
                {
                    DataView journalView = _journalEntries.DefaultView;
                    journalView.RowFilter = ""; // Clear any existing filter first

                    // Now apply the actual filter
                    string journalFilter = BuildJournalFilterString();
                    if (!string.IsNullOrEmpty(journalFilter))
                    {
                        journalView.RowFilter = journalFilter;
                    }

                    dgvJournalEntries.DataSource = journalView.ToTable();
                }

                // Filter sales entries
                if (_salesEntries != null && _salesEntries.Rows.Count > 0)
                {
                    DataView salesView = _salesEntries.DefaultView;
                    salesView.RowFilter = ""; // Clear any existing filter first

                    string salesFilter = BuildSalesFilterString();
                    if (!string.IsNullOrEmpty(salesFilter))
                    {
                        salesView.RowFilter = salesFilter;
                    }

                    dgvSalesEntries.DataSource = salesView.ToTable();
                }

                // Filter purchase entries
                if (_purchaseEntries != null && _purchaseEntries.Rows.Count > 0)
                {
                    DataView purchaseView = _purchaseEntries.DefaultView;
                    purchaseView.RowFilter = ""; // Clear any existing filter first

                    string purchaseFilter = BuildPurchaseFilterString();
                    if (!string.IsNullOrEmpty(purchaseFilter))
                    {
                        purchaseView.RowFilter = purchaseFilter;
                    }

                    dgvPurchaseEntries.DataSource = purchaseView.ToTable();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error applying filters: {ex.Message}", "خطأ في تطبيق التصفية");
            }
        }

        private string BuildJournalFilterString()
        {
            try
            {
                List<string> conditions = new List<string>();

                // The data is already filtered by date range at the DAL level,
                // so we only filter by status on the client side
                string statusFilter = cmbStatusFilter.SelectedItem?.ToString() ?? "All";
                if (statusFilter == "Reconciled")
                {
                    conditions.Add("is_reconciled = 1");
                }
                else if (statusFilter == "Unreconciled")
                {
                    conditions.Add("is_reconciled = 0");
                }

                return conditions.Count > 0 ? string.Join(" AND ", conditions) : "";
            }
            catch
            {
                return "";
            }
        }

        private string BuildSalesFilterString()
        {
            try
            {
                // No additional filters needed; already filtered by date at DAL
                return "";
            }
            catch
            {
                return "";
            }
        }

        private string BuildPurchaseFilterString()
        {
            try
            {
                // No additional filters needed; already filtered by date at DAL
                return "";
            }
            catch
            {
                return "";
            }
        }

        private void FilterGridBySearch(DataGridView grid, string searchText)
        {
            if (grid.DataSource is DataTable dt)
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    grid.DataSource = dt;
                    return;
                }

                DataView dv = dt.DefaultView;
                var filterConditions = new List<string>();

                if (dt.Columns.Contains("invoice_no"))
                    filterConditions.Add($"invoice_no LIKE '%{searchText}%'");

                if (dt.Columns.Contains("reference_no"))
                    filterConditions.Add($"reference_no LIKE '%{searchText}%'");

                if (filterConditions.Count > 0)
                    dv.RowFilter = string.Join(" OR ", filterConditions);

                grid.DataSource = dv.ToTable();
            }
        }

        private void DgvJournalEntries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string invoiceNo = dgvJournalEntries.Rows[e.RowIndex].Cells["colInvoiceNo"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(invoiceNo))
                {
                    ShowJournalEntryDetails(invoiceNo);
                }
            }
        }

        private void DgvSalesEntries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string invoiceNo = dgvSalesEntries.Rows[e.RowIndex].Cells["colSalesInvoiceNo"].Value?.ToString() ?? "";
                ShowSalesDetails(invoiceNo);
            }
        }

        private void DgvPurchaseEntries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string invoiceNo = dgvPurchaseEntries.Rows[e.RowIndex].Cells["colPurchaseInvoiceNo"].Value?.ToString() ?? "";
                ShowPurchaseDetails(invoiceNo);
            }
        }

        private void DgvUnreconciled_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Display unreconciled entry details
        }

        private void ShowJournalEntryDetails(string invoiceNo)
        {
            try
            {
                DataTable entryDetails = _journalsBll.GetVoucherDetailsByInvoiceNo(invoiceNo);

                using (var detailForm = new Form())
                {
                    detailForm.Text = $"Journal Entry Details - {invoiceNo}";
                    detailForm.Width = 800;
                    detailForm.Height = 500;
                    detailForm.StartPosition = FormStartPosition.CenterParent;

                    DataGridView dgv = new DataGridView
                    {
                        Dock = DockStyle.Fill,
                        AutoGenerateColumns = false,
                        ReadOnly = true,
                        AllowUserToAddRows = false,
                        DataSource = entryDetails
                    };

                    detailForm.Controls.Add(dgv);
                    detailForm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error loading details: {ex.Message}", "خطأ في تحميل التفاصيل");
            }
        }

        private void ShowSalesDetails(string invoiceNo)
        {
            try
            {
                DataTable saleItems = _salesBll.GetAllSalesItems(invoiceNo);

                using (var detailForm = new Form())
                {
                    detailForm.Text = $"Sales Invoice Details - {invoiceNo}";
                    detailForm.Width = 900;
                    detailForm.Height = 500;
                    detailForm.StartPosition = FormStartPosition.CenterParent;

                    DataGridView dgv = new DataGridView
                    {
                        Dock = DockStyle.Fill,
                        AutoGenerateColumns = false,
                        ReadOnly = true,
                        AllowUserToAddRows = false,
                        DataSource = saleItems
                    };

                    detailForm.Controls.Add(dgv);
                    detailForm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error loading sales details: {ex.Message}", "خطأ في تحميل تفاصيل المبيعات");
            }
        }

        private void ShowPurchaseDetails(string invoiceNo)
        {
            try
            {
                DataTable purchaseItems = _purchasesBll.GetAllPurchasesItems(invoiceNo);

                using (var detailForm = new Form())
                {
                    detailForm.Text = $"Purchase Invoice Details - {invoiceNo}";
                    detailForm.Width = 900;
                    detailForm.Height = 500;
                    detailForm.StartPosition = FormStartPosition.CenterParent;

                    DataGridView dgv = new DataGridView
                    {
                        Dock = DockStyle.Fill,
                        AutoGenerateColumns = false,
                        ReadOnly = true,
                        AllowUserToAddRows = false,
                        DataSource = purchaseItems
                    };

                    detailForm.Controls.Add(dgv);
                    detailForm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error loading purchase details: {ex.Message}", "خطأ في تحميل تفاصيل الشراء");
            }
        }

        private void ReconcileSelectedEntries()
        {
            try
            {
                if (dgvJournalEntries.SelectedRows.Count == 0)
                {
                    UiMessages.ShowWarning("Please select journal entries to reconcile.", "الرجاء تحديد قيود دفتر اليومية للتسوية");
                    return;
                }

                // Build summary of selected entries with match suggestions
                var selectedEntries = new List<DataRow>();
                var matchSummary = new StringBuilder();
                matchSummary.AppendLine("Summary of entries to be reconciled:\n");

                foreach (DataGridViewRow row in dgvJournalEntries.SelectedRows)
                {
                    int rowIndex = _journalEntries.Rows.IndexOf(_journalEntries.Rows.Cast<DataRow>()
                        .FirstOrDefault(r => r["invoice_no"].ToString() == row.Cells["colInvoiceNo"].Value?.ToString()));

                    if (rowIndex >= 0)
                    {
                        var journalRow = _journalEntries.Rows[rowIndex];
                        selectedEntries.Add(journalRow);

                        string invoiceNo = journalRow["invoice_no"]?.ToString() ?? "Unknown";

                        // Get suggested matches using reference-first strategy
                        var (salesMatch, purchaseMatch) = GetSuggestedMatches(journalRow);
                        string matchType = salesMatch != null ? "Sales" : (purchaseMatch != null ? "Purchase" : "Not Found");

                        matchSummary.AppendLine($"  • {invoiceNo} → Match Type: {matchType}");
                    }
                }

                // Show confirmation with match suggestions
                if (MessageBox.Show(
                    matchSummary.ToString() + "\nProceed with reconciliation?", 
                    "Confirm Reconciliation", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    int successCount = 0;
                    int failCount = 0;

                    foreach (var journalRow in selectedEntries)
                    {
                        string invoiceNo = journalRow["invoice_no"]?.ToString() ?? "";
                        if (!string.IsNullOrEmpty(invoiceNo))
                        {
                            try
                            {
                                _journalsBll.UpdateReconciliationStatus(invoiceNo, true, _currentUser.UserId, DateTime.Now);
                                successCount++;
                            }
                            catch
                            {
                                failCount++;
                            }
                        }
                    }

                    Cursor = Cursors.Default;

                    if (failCount > 0)
                    {
                        UiMessages.ShowWarning(
                            $"Reconciliation completed. Success: {successCount}, Failed: {failCount}", 
                            "تحذير التسوية");
                    }
                    else
                    {
                        UiMessages.ShowInfo(
                            $"Reconciliation completed successfully. {successCount} entries reconciled.", 
                            "تم إكمال التسوية بنجاح");
                    }

                    LoadReconciliationData();
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UiMessages.ShowError($"Reconciliation error: {ex.Message}", "خطأ في التسوية");
            }
        }

        private void ReverseReconciliation()
        {
            try
            {
                if (dgvJournalEntries.SelectedRows.Count == 0)
                {
                    UiMessages.ShowWarning("Please select entries to reverse reconciliation.", "الرجاء تحديد البيانات لعكس التسوية");
                    return;
                }

                if (MessageBox.Show("Reverse reconciliation for selected entries?", "Confirm Reversal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    foreach (DataGridViewRow row in dgvJournalEntries.SelectedRows)
                    {
                        string invoiceNo = row.Cells["colInvoiceNo"].Value?.ToString() ?? "";
                        if (!string.IsNullOrEmpty(invoiceNo))
                        {
                            _journalsBll.UpdateReconciliationStatus(invoiceNo, false, _currentUser.UserId, DateTime.Now);
                        }
                    }

                    Cursor = Cursors.Default;
                    UiMessages.ShowInfo("Reconciliation reversed successfully.", "تم عكس التسوية بنجاح");
                    LoadReconciliationData();
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UiMessages.ShowError($"Reversal error: {ex.Message}", "خطأ في العكس");
            }
        }

        private void ShowAdvancedMatchingDialog()
        {
            try
            {
                using (var advancedForm = new FrmAdvancedReconciliationMatcher(_journalEntries, _salesEntries, _purchaseEntries, _journalsBll))
                {
                    if (advancedForm.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadReconciliationData();
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error opening advanced matcher: {ex.Message}", "خطأ في فتح أداة المطابقة المتقدمة");
            }
        }

        /// <summary>
        /// Find matching sales entry by reference number (invoice_no)
        /// This is the PRIMARY matching method using exact database relationships
        /// </summary>
        private DataRow FindSalesMatchByReference(string journalInvoiceNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(journalInvoiceNo) || _salesEntries == null)
                    return null;

                return _salesEntries.Rows.Cast<DataRow>()
                    .FirstOrDefault(r => r["invoice_no"].ToString()
                        .Equals(journalInvoiceNo, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Find matching purchase entry by reference number (invoice_no)
        /// This is the PRIMARY matching method using exact database relationships
        /// </summary>
        private DataRow FindPurchaseMatchByReference(string journalInvoiceNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(journalInvoiceNo) || _purchaseEntries == null)
                    return null;

                return _purchaseEntries.Rows.Cast<DataRow>()
                    .FirstOrDefault(r => r["invoice_no"].ToString()
                        .Equals(journalInvoiceNo, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Find matching sales entry by amount (fallback when reference match fails)
        /// </summary>
        private DataRow FindSalesMatchByAmount(double journalAmount, double tolerance = 1.0)
        {
            try
            {
                if (_salesEntries == null)
                    return null;

                return _salesEntries.Rows.Cast<DataRow>()
                    .FirstOrDefault(r => Math.Abs(Convert.ToDouble(r["total_amount"] ?? 0) - journalAmount) < tolerance);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Find matching purchase entry by amount (fallback when reference match fails)
        /// </summary>
        private DataRow FindPurchaseMatchByAmount(double journalAmount, double tolerance = 1.0)
        {
            try
            {
                if (_purchaseEntries == null)
                    return null;

                return _purchaseEntries.Rows.Cast<DataRow>()
                    .FirstOrDefault(r => Math.Abs(Convert.ToDouble(r["total_amount"] ?? 0) - journalAmount) < tolerance);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get suggested matches for a journal entry
        /// Returns sales/purchase matches using reference-first, amount-fallback strategy
        /// </summary>
        public (DataRow SalesMatch, DataRow PurchaseMatch) GetSuggestedMatches(DataRow journalRow, double tolerance = 1.0)
        {
            try
            {
                string invoiceNo = journalRow["invoice_no"]?.ToString() ?? "";
                double debit = Convert.ToDouble(journalRow["debit"] ?? 0);
                double credit = Convert.ToDouble(journalRow["credit"] ?? 0);
                double amount = debit > 0 ? debit : credit;

                DataRow salesMatch = null;
                DataRow purchaseMatch = null;

                // Tier 1: Try Reference Number Matching (most accurate)
                salesMatch = FindSalesMatchByReference(invoiceNo);
                if (salesMatch == null)
                {
                    // Tier 2: Fall back to Amount-based Matching
                    salesMatch = FindSalesMatchByAmount(amount, tolerance);
                }

                // Same for purchases
                purchaseMatch = FindPurchaseMatchByReference(invoiceNo);
                if (purchaseMatch == null)
                {
                    purchaseMatch = FindPurchaseMatchByAmount(amount, tolerance);
                }

                return (salesMatch, purchaseMatch);
            }
            catch
            {
                return (null, null);
            }
        }

        private void ExportReconciliationReport()
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "CSV Files (*.csv)|*.csv";
                    saveDialog.FileName = $"Reconciliation_Report_{DateTime.Now:yyyyMMdd_HHmmss}";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        Cursor = Cursors.WaitCursor;
                        ExportToFile(saveDialog.FileName, System.IO.Path.GetExtension(saveDialog.FileName));
                        Cursor = Cursors.Default;
                        UiMessages.ShowInfo("Report exported successfully.", "تم تصدير التقرير بنجاح");
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UiMessages.ShowError($"Export error: {ex.Message}", "خطأ في التصدير");
            }
        }

        private void ExportToFile(string filePath, string extension)
        {
            try
            {
                if (extension == ".csv")
                {
                    ExportToCsv(filePath);
                }
                else
                {
                    // Excel/PDF export logic - integrate with your reporting library
                    MessageBox.Show($"Export to {extension} requires additional configuration.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error exporting to {extension}: {ex.Message}");
            }
        }

        private void ExportToCsv(string filePath)
        {
            StringBuilder csv = new StringBuilder();

            // Add headers
            csv.AppendLine("Reconciliation Report");
            csv.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            csv.AppendLine($"Period: {dtpFromDate.Value:yyyy-MM-dd} to {dtpToDate.Value:yyyy-MM-dd}");
            csv.AppendLine();

            csv.AppendLine("Journal Entries:");
            ExportGridToCsv(dgvJournalEntries, csv);
            csv.AppendLine();

            csv.AppendLine("Sales Entries:");
            ExportGridToCsv(dgvSalesEntries, csv);
            csv.AppendLine();

            csv.AppendLine("Purchase Entries:");
            ExportGridToCsv(dgvPurchaseEntries, csv);

            System.IO.File.WriteAllText(filePath, csv.ToString());
        }

        private void ExportGridToCsv(DataGridView grid, StringBuilder csv)
        {
            // Export column headers
            foreach (DataGridViewColumn column in grid.Columns)
            {
                csv.Append(column.HeaderText.Replace(",", " ")).Append(",");
            }
            csv.AppendLine();

            // Export rows
            foreach (DataGridViewRow row in grid.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    csv.Append(cell.Value?.ToString()?.Replace(",", " ") ?? "").Append(",");
                }
                csv.AppendLine();
            }
        }

        private void RefreshSummary()
        {
            try
            {
                int totalJournal = dgvJournalEntries.Rows.Count;
                int totalSales = dgvSalesEntries.Rows.Count;
                int totalPurchase = dgvPurchaseEntries.Rows.Count;
                int totalUnreconciled = dgvUnreconciled.Rows.Count;

                int reconciledCount = dgvJournalEntries.Rows.Cast<DataGridViewRow>()
                    .Where(r => r.Cells["colStatus"]?.Value?.ToString() == "Reconciled").Count();

                lblSummary.Text = $"Journal Entries: {totalJournal} | Sales: {totalSales} | Purchase: {totalPurchase} | " +
                                  $"Reconciled: {reconciledCount} | Pending: {totalUnreconciled}";
            }
            catch { }
        }
    }
}
