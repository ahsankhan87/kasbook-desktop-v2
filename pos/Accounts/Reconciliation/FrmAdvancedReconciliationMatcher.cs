using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;
using pos.Security.Authorization;

namespace pos.Accounts.Reconciliation
{
    public partial class FrmAdvancedReconciliationMatcher : Form
    {
        private readonly DataTable _journalEntries;
        private readonly DataTable _salesEntries;
        private readonly DataTable _purchaseEntries;
        private readonly JournalsBLL _journalsBll;

        private List<ReconciliationMatch> _matches = new List<ReconciliationMatch>();

        public FrmAdvancedReconciliationMatcher(DataTable journals, DataTable sales, DataTable purchases, JournalsBLL journalsBll)
        {
            InitializeComponent();
            _journalEntries = journals;
            _salesEntries = sales;
            _purchaseEntries = purchases;
            _journalsBll = journalsBll;
            WireEvents();
        }

        private void WireEvents()
        {
            Load += (s, e) => OnFormLoad();
            btnAutoMatch.Click += (s, e) => AutoMatchEntries();
            btnManualMatch.Click += (s, e) => ManuallyMatchSelected();
            btnApplyMatches.Click += (s, e) => ApplyMatches();
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            dgvJournalUnmatched.CellClick += (s, e) => OnJournalSelected(e);
            dgvSalesMatches.CellClick += (s, e) => OnPotentialMatch(dgvSalesMatches, e);
            dgvPurchaseMatches.CellClick += (s, e) => OnPotentialMatch(dgvPurchaseMatches, e);

            chkAutoTolerance.CheckedChanged += (s, e) => EnableToleranceControls();
            numTolerance.ValueChanged += (s, e) => numTolerance_ValueChanged();
        }

        private void OnFormLoad()
        {
            AppTheme.Apply(this);
            ConfigureGrids();
            LoadUnmatchedJournalEntries();
            LoadInitialSalesAndPurchases();
            EnableToleranceControls();
        }

        private void LoadInitialSalesAndPurchases()
        {
            try
            {
                // Debug: Log column names
                if (_salesEntries != null && _salesEntries.Rows.Count > 0)
                {
                    var colNames = string.Join(", ", _salesEntries.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                    System.Diagnostics.Debug.WriteLine($"Sales columns: {colNames}");
                }

                // Load all sales entries initially
                if (_salesEntries != null && _salesEntries.Rows.Count > 0)
                {
                    dgvSalesMatches.DataSource = _salesEntries.Copy();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No sales entries available");
                }

                // Debug: Log column names
                if (_purchaseEntries != null && _purchaseEntries.Rows.Count > 0)
                {
                    var colNames = string.Join(", ", _purchaseEntries.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
                    System.Diagnostics.Debug.WriteLine($"Purchase columns: {colNames}");
                }

                // Load all purchase entries initially
                if (_purchaseEntries != null && _purchaseEntries.Rows.Count > 0)
                {
                    dgvPurchaseMatches.DataSource = _purchaseEntries.Copy();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No purchase entries available");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading initial data: {ex.Message}");
            }
        }

        private void ConfigureGrids()
        {
            dgvJournalUnmatched.AutoGenerateColumns = false;
            dgvJournalUnmatched.AllowUserToAddRows = false;
            dgvJournalUnmatched.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvSalesMatches.AutoGenerateColumns = false;
            dgvSalesMatches.AllowUserToAddRows = false;

            dgvPurchaseMatches.AutoGenerateColumns = false;
            dgvPurchaseMatches.AllowUserToAddRows = false;

            dgvMatchResults.AutoGenerateColumns = false;
            dgvMatchResults.AllowUserToAddRows = false;
            dgvMatchResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMatchResults.MultiSelect = true;
        }

        private void LoadUnmatchedJournalEntries()
        {
            try
            {
                // Filter unreconciled entries (is_reconciled is int, so compare as integer)
                DataView dv = _journalEntries.DefaultView;

                // Create a filtered DataTable with unreconciled entries
                DataTable unmatched = _journalEntries.Clone();
                foreach (DataRow row in _journalEntries.Rows)
                {
                    try
                    {
                        int isReconciled = Convert.ToInt32(row["is_reconciled"] ?? 0);
                        if (isReconciled == 0)
                        {
                            unmatched.ImportRow(row);
                        }
                    }
                    catch { }
                }

                dgvJournalUnmatched.DataSource = unmatched;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error: {ex.Message}", "خطأ");
            }
        }

        private void OnJournalSelected(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                string journalInvoice = dgvJournalUnmatched.Rows[e.RowIndex].Cells["colJournalInvoice"].Value?.ToString() ?? "";
                double journalAmount = Convert.ToDouble(dgvJournalUnmatched.Rows[e.RowIndex].Cells["colJournalAmount"].Value ?? 0);

                LoadPotentialMatches(journalInvoice, journalAmount);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error: {ex.Message}", "خطأ");
            }
        }

        private void LoadPotentialMatches(string journalInvoice, double journalAmount)
        {
            try
            {
                dgvSalesMatches.DataSource = null;
                dgvPurchaseMatches.DataSource = null;

                // First, try to find matches by reference number (if available in journal)
                DataTable refMatchedSales = FindMatchesByReferenceNumber(journalInvoice, "Sales");
                DataTable refMatchedPurchases = FindMatchesByReferenceNumber(journalInvoice, "Purchase");

                // If no reference match found, fall back to amount-based matching
                if (refMatchedSales == null || refMatchedSales.Rows.Count == 0)
                {
                    var salesMatchRows = _salesEntries.Rows.Cast<DataRow>()
                        .Where(r => Math.Abs(Convert.ToDouble(r["total_amount"] ?? 0) - journalAmount) < (double)numTolerance.Value)
                        .ToList();

                    if (salesMatchRows.Count > 0)
                    {
                        refMatchedSales = salesMatchRows.CopyToDataTable();
                    }
                }

                if (refMatchedSales != null && refMatchedSales.Rows.Count > 0)
                {
                    dgvSalesMatches.DataSource = refMatchedSales;
                }

                // Load matching purchase entries
                if (refMatchedPurchases == null || refMatchedPurchases.Rows.Count == 0)
                {
                    var purchaseMatchRows = _purchaseEntries.Rows.Cast<DataRow>()
                        .Where(r => Math.Abs(Convert.ToDouble(r["total_amount"] ?? 0) - journalAmount) < (double)numTolerance.Value)
                        .ToList();

                    if (purchaseMatchRows.Count > 0)
                    {
                        refMatchedPurchases = purchaseMatchRows.CopyToDataTable();
                    }
                }

                if (refMatchedPurchases != null && refMatchedPurchases.Rows.Count > 0)
                {
                    dgvPurchaseMatches.DataSource = refMatchedPurchases;
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error loading matches: {ex.Message}", "خطأ في تحميل المطابقات");
            }
        }

        /// <summary>
        /// Find sales/purchase entries by reference number
        /// Sales/Purchase invoice_no is stored in acc_entries_header.reference_no or acc_entries.payment_ref_invoice_no
        /// </summary>
        private DataTable FindMatchesByReferenceNumber(string invoiceNo, string type)
        {
            try
            {
                DataTable sourceData = type == "Sales" ? _salesEntries : _purchaseEntries;
                if (sourceData == null || sourceData.Rows.Count == 0)
                    return null;

                // Try to find the invoice in the source data
                var matchingRows = sourceData.Rows.Cast<DataRow>()
                    .Where(r => r["invoice_no"].ToString().Equals(invoiceNo, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (matchingRows.Count > 0)
                {
                    return matchingRows.CopyToDataTable();
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error finding by reference: {ex.Message}");
                return null;
            }
        }

        private void OnPotentialMatch(DataGridView sourceGrid, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvJournalUnmatched.SelectedRows.Count == 0) return;

            try
            {
                string journalInvoice = dgvJournalUnmatched.SelectedRows[0].Cells["colJournalInvoice"].Value?.ToString() ?? "";
                double journalAmount = Convert.ToDouble(dgvJournalUnmatched.SelectedRows[0].Cells["colJournalAmount"].Value ?? 0);

                // Get the invoice number from the first column of the selected match row
                string matchInvoice = sourceGrid.Rows[e.RowIndex].Cells[0].Value?.ToString() ?? "";

                // Get amount from the correct column based on grid type
                string amountColumnName = sourceGrid == dgvSalesMatches ? "colSalesAmount" : "colPurchaseAmount";
                double matchAmount = Convert.ToDouble(sourceGrid.Rows[e.RowIndex].Cells[amountColumnName].Value ?? 0);

                string matchType = sourceGrid == dgvSalesMatches ? "Sales" : "Purchase";

                // Add to matches list
                var match = new ReconciliationMatch
                {
                    JournalInvoice = journalInvoice,
                    JournalAmount = journalAmount,
                    MatchInvoice = matchInvoice,
                    MatchAmount = matchAmount,
                    MatchType = matchType,
                    MatchScore = CalculateMatchScore(journalAmount, matchAmount)
                };

                _matches.Add(match);
                DisplayMatches();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error: {ex.Message}", "خطأ");
            }
        }

        private double CalculateMatchScore(double journalAmount, double matchAmount)
        {
            if (journalAmount == 0) return 0;
            return (1 - Math.Abs(journalAmount - matchAmount) / journalAmount) * 100;
        }

        private void AutoMatchEntries()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _matches.Clear();

                foreach (DataRow journal in _journalEntries.Rows)
                {
                    // is_reconciled is int (bit), so check if it's 0 (unreconciled)
                    int isReconciled = Convert.ToInt32(journal["is_reconciled"] ?? 0);
                    if (isReconciled != 0)
                        continue;

                    string journalInvoice = journal["invoice_no"].ToString();
                    double journalAmount = Convert.ToDouble(journal["debit"] ?? 0) + Convert.ToDouble(journal["credit"] ?? 0);

                    // First, try to match by reference number (more accurate)
                    var salesMatch = _salesEntries.Rows.Cast<DataRow>()
                        .FirstOrDefault(r => r["invoice_no"].ToString().Equals(journalInvoice, StringComparison.OrdinalIgnoreCase));

                    if (salesMatch != null)
                    {
                        _matches.Add(new ReconciliationMatch
                        {
                            JournalInvoice = journalInvoice,
                            JournalAmount = journalAmount,
                            MatchInvoice = salesMatch["invoice_no"].ToString(),
                            MatchAmount = Convert.ToDouble(salesMatch["total_amount"] ?? 0),
                            MatchType = "Sales",
                            MatchScore = CalculateMatchScore(journalAmount, Convert.ToDouble(salesMatch["total_amount"] ?? 0))
                        });
                    }
                    else
                    {
                        // Fall back to amount-based matching
                        var salesMatchByAmount = _salesEntries.Rows.Cast<DataRow>()
                            .FirstOrDefault(r => Math.Abs(Convert.ToDouble(r["total_amount"] ?? 0) - journalAmount) < (double)numTolerance.Value);

                        if (salesMatchByAmount != null)
                        {
                            _matches.Add(new ReconciliationMatch
                            {
                                JournalInvoice = journalInvoice,
                                JournalAmount = journalAmount,
                                MatchInvoice = salesMatchByAmount["invoice_no"].ToString(),
                                MatchAmount = Convert.ToDouble(salesMatchByAmount["total_amount"] ?? 0),
                                MatchType = "Sales",
                                MatchScore = CalculateMatchScore(journalAmount, Convert.ToDouble(salesMatchByAmount["total_amount"] ?? 0))
                            });
                        }
                        else
                        {
                            // Try to match purchase by reference number
                            var purchaseMatch = _purchaseEntries.Rows.Cast<DataRow>()
                                .FirstOrDefault(r => r["invoice_no"].ToString().Equals(journalInvoice, StringComparison.OrdinalIgnoreCase));

                            if (purchaseMatch != null)
                            {
                                _matches.Add(new ReconciliationMatch
                                {
                                    JournalInvoice = journalInvoice,
                                    JournalAmount = journalAmount,
                                    MatchInvoice = purchaseMatch["invoice_no"].ToString(),
                                    MatchAmount = Convert.ToDouble(purchaseMatch["total_amount"] ?? 0),
                                    MatchType = "Purchase",
                                    MatchScore = CalculateMatchScore(journalAmount, Convert.ToDouble(purchaseMatch["total_amount"] ?? 0))
                                });
                            }
                            else
                            {
                                // Fall back to amount-based matching for purchases
                                var purchaseMatchByAmount = _purchaseEntries.Rows.Cast<DataRow>()
                                    .FirstOrDefault(r => Math.Abs(Convert.ToDouble(r["total_amount"] ?? 0) - journalAmount) < (double)numTolerance.Value);

                                if (purchaseMatchByAmount != null)
                                {
                                    _matches.Add(new ReconciliationMatch
                                    {
                                        JournalInvoice = journalInvoice,
                                        JournalAmount = journalAmount,
                                        MatchInvoice = purchaseMatchByAmount["invoice_no"].ToString(),
                                        MatchAmount = Convert.ToDouble(purchaseMatchByAmount["total_amount"] ?? 0),
                                        MatchType = "Purchase",
                                        MatchScore = CalculateMatchScore(journalAmount, Convert.ToDouble(purchaseMatchByAmount["total_amount"] ?? 0))
                                    });
                                }
                            }
                        }
                    }
                }

                DisplayMatches();
                Cursor = Cursors.Default;
                UiMessages.ShowInfo($"Auto-matching completed. Found {_matches.Count} matches.", "اكتملت المطابقة التلقائية");
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UiMessages.ShowError($"Auto-match error: {ex.Message}", "خطأ في المطابقة التلقائية");
            }
        }

        private void ManuallyMatchSelected()
        {
            try
            {
                if (dgvJournalUnmatched.SelectedRows.Count == 0 || (dgvSalesMatches.SelectedRows.Count == 0 && dgvPurchaseMatches.SelectedRows.Count == 0))
                {
                    UiMessages.ShowWarning("Please select a journal entry and a match.", "الرجاء تحديد قيد ومطابقة");
                    return;
                }

                string journalInvoice = dgvJournalUnmatched.SelectedRows[0].Cells["colJournalInvoice"].Value?.ToString() ?? "";
                double journalAmount = Convert.ToDouble(dgvJournalUnmatched.SelectedRows[0].Cells["colJournalAmount"].Value ?? 0);

                if (dgvSalesMatches.SelectedRows.Count > 0)
                {
                    AddManualMatch("Sales", dgvSalesMatches, journalInvoice, journalAmount);
                }
                else if (dgvPurchaseMatches.SelectedRows.Count > 0)
                {
                    AddManualMatch("Purchase", dgvPurchaseMatches, journalInvoice, journalAmount);
                }

                DisplayMatches();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error: {ex.Message}", "خطأ");
            }
        }

        private void AddManualMatch(string type, DataGridView sourceGrid, string journalInvoice, double journalAmount)
        {
            string matchInvoice = sourceGrid.SelectedRows[0].Cells[0].Value?.ToString() ?? "";

            // Get amount from the correct column based on grid type
            string amountColumnName = type == "Sales" ? "colSalesAmount" : "colPurchaseAmount";
            double matchAmount = Convert.ToDouble(sourceGrid.SelectedRows[0].Cells[amountColumnName].Value ?? 0);

            var match = new ReconciliationMatch
            {
                JournalInvoice = journalInvoice,
                JournalAmount = journalAmount,
                MatchInvoice = matchInvoice,
                MatchAmount = matchAmount,
                MatchType = type,
                MatchScore = CalculateMatchScore(journalAmount, matchAmount)
            };

            _matches.Add(match);
        }

        private void DisplayMatches()
        {
            try
            {
                DataTable matchesTable = new DataTable();
                matchesTable.Columns.Add("Journal Invoice");
                matchesTable.Columns.Add("Journal Amount", typeof(double));
                matchesTable.Columns.Add("Match Type");
                matchesTable.Columns.Add("Match Invoice");
                matchesTable.Columns.Add("Match Amount", typeof(double));
                matchesTable.Columns.Add("Match Score", typeof(double));

                foreach (var match in _matches)
                {
                    matchesTable.Rows.Add(
                        match.JournalInvoice,
                        match.JournalAmount,
                        match.MatchType,
                        match.MatchInvoice,
                        match.MatchAmount,
                        Math.Round(match.MatchScore, 2)
                    );
                }

                dgvMatchResults.DataSource = matchesTable;

                // Format numeric columns
                foreach (DataGridViewColumn col in dgvMatchResults.Columns)
                {
                    if (col.Name.Contains("Amount") || col.Name.Contains("Score"))
                    {
                        col.DefaultCellStyle.Format = "N2";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError($"Error displaying matches: {ex.Message}", "خطأ في عرض المطابقات");
            }
        }

        private void ApplyMatches()
        {
            try
            {
                if (_matches.Count == 0)
                {
                    UiMessages.ShowWarning("No matches to apply.", "لا توجد مطابقات للتطبيق");
                    return;
                }

                if (MessageBox.Show($"Apply {_matches.Count} matches?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    int successCount = 0;
                    foreach (var match in _matches)
                    {
                        try
                        {
                            _journalsBll.UpdateReconciliationStatus(match.JournalInvoice, true, AppSecurityContext.User.UserId, DateTime.Now);
                            successCount++;
                        }
                        catch
                        {
                            // Log and continue
                        }
                    }

                    Cursor = Cursors.Default;
                    UiMessages.ShowInfo($"Applied {successCount} matches successfully.", "تم تطبيق المطابقات بنجاح");
                    DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                UiMessages.ShowError($"Error applying matches: {ex.Message}", "خطأ في تطبيق المطابقات");
            }
        }

        private void EnableToleranceControls()
        {
            numTolerance.Enabled = chkAutoTolerance.Checked;
        }

        private void numTolerance_ValueChanged()
        {
            // Auto-refresh matches based on new tolerance
            if (dgvJournalUnmatched.SelectedRows.Count > 0)
            {
                try
                {
                    string journalInvoice = dgvJournalUnmatched.SelectedRows[0].Cells["colJournalInvoice"].Value?.ToString() ?? "";
                    double journalAmount = Convert.ToDouble(dgvJournalUnmatched.SelectedRows[0].Cells["colJournalAmount"].Value ?? 0);
                    LoadPotentialMatches(journalInvoice, journalAmount);
                }
                catch { }
            }
        }

        private class ReconciliationMatch
        {
            public string JournalInvoice { get; set; }
            public double JournalAmount { get; set; }
            public string MatchInvoice { get; set; }
            public double MatchAmount { get; set; }
            public string MatchType { get; set; }
            public double MatchScore { get; set; }
        }
    }
}
