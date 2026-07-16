using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using POS.BLL.Accounts;
using POS.Core;
using POS.Core.Accounts;
using POS.Core.POS;
using POS.DLL.Accounts;
using pos.UI;
using pos.UI.Busy;

namespace pos.Accounts
{
    public partial class frm_import_data : Form
    {
        private readonly ImportBLL _bll = new ImportBLL();
        private readonly ImportDLL _dll = new ImportDLL();

        // Current uploaded data
        private List<ChartOfAccountsImportRow> _coaRows;
        private List<OpeningBalanceImportRow> _obRows;
        private List<PartyBalanceImportRow> _partyRows;
        private List<JournalEntryImportRow> _journalRows;
        private List<JournalVoucherGroup> _journalVouchers;

        private string _currentFileName;

        public frm_import_data()
        {
            InitializeComponent();
        }

        private void frm_import_data_Load(object sender, EventArgs e)
        {
            // Apply theme
            AppTheme.Apply(this);

            // Set default date
            dtpOBDate.Value = DateTime.Today;

            // Load import history
            LoadImportHistory();

            // Permission check (Admin only)
            if (UsersModal.logged_in_user_role != "Admin" && UsersModal.logged_in_user_role != "Administrator")
            {
                UiMessagesEx.Warning("Only administrators can access the import functionality.", "Access Denied");
                this.Close();
                return;
            }
        }

        #region Tab 1: Chart of Accounts

        private void btnDownloadCOATemplate_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, "Generating template..."))
                {
                    string templatePath = ImportTemplateGenerator.CreateImportTemplate("COA");

                    if (MessageBox.Show(
                        $"Template created successfully!\n\nPath: {templatePath}\n\nDo you want to open it now?",
                        "Template Ready",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(templatePath);
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessagesEx.Error("Template Error", "Failed to generate template: " + ex.Message);
            }
        }

        private void btnUploadCOA_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files|*.xls;*.xlsx";
                ofd.Title = "Select Chart of Accounts Import File";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    using (BusyScope.Show(this, "Loading and validating file..."))
                    {
                        _currentFileName = Path.GetFileName(ofd.FileName);

                        // Read Excel
                        var dataTable = _dll.ReadExcelFile(ofd.FileName);

                        // Parse rows
                        _coaRows = _bll.ParseChartOfAccounts(dataTable);

                        // Validate
                        _bll.ValidateChartOfAccounts(_coaRows);

                        // Display in grid
                        DisplayCOAPreview();

                        btnImportCOA.Enabled = _coaRows.Any(r => r.IsValid);

                        var validCount = _coaRows.Count(r => r.IsValid);
                        var invalidCount = _coaRows.Count - validCount;

                        UiMessagesEx.Information("Validation Complete",
                            $"Total rows: {_coaRows.Count}\n" +
                            $"Valid: {validCount}\n" +
                            $"Invalid: {invalidCount}\n\n" +
                            (invalidCount > 0 ? "Invalid rows are highlighted in red." : "All rows are valid!"));
                    }
                }
                catch (Exception ex)
                {
                    UiMessagesEx.Error("Upload Error", "Failed to read file: " + ex.Message);
                }
            }
        }

        private void DisplayCOAPreview()
        {
            var dt = new DataTable();
            dt.Columns.Add("Row", typeof(int));
            dt.Columns.Add("Valid", typeof(bool));
            dt.Columns.Add("Account Code", typeof(string));
            dt.Columns.Add("Account Name", typeof(string));
            dt.Columns.Add("Account Type", typeof(string));
            dt.Columns.Add("Parent Code", typeof(string));
            dt.Columns.Add("Normal Balance", typeof(string));
            dt.Columns.Add("Opening Balance", typeof(decimal));
            dt.Columns.Add("Validation Error", typeof(string));

            foreach (var row in _coaRows)
            {
                dt.Rows.Add(
                    row.RowNumber,
                    row.IsValid,
                    row.AccountCode,
                    row.AccountName,
                    row.AccountType,
                    row.ParentCode,
                    row.NormalBalance,
                    row.OpeningBalance ?? 0,
                    row.ValidationError
                );
            }

            gridCOA.DataSource = dt;

            // Hide Valid column, make it first for row coloring
            gridCOA.Columns["Valid"].Visible = false;
            gridCOA.Columns["Row"].Width = 60;
            gridCOA.Columns["Account Code"].Width = 100;
            gridCOA.Columns["Account Name"].Width = 200;
            gridCOA.Columns["Validation Error"].Width = 300;

            // Color invalid rows
            foreach (DataGridViewRow gridRow in gridCOA.Rows)
            {
                if (gridRow.Cells["Valid"].Value != null && !(bool)gridRow.Cells["Valid"].Value)
                {
                    gridRow.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238); // Light red
                }
            }
        }

        private void btnImportCOA_Click(object sender, EventArgs e)
        {
            if (_coaRows == null || !_coaRows.Any())
            {
                UiMessagesEx.Warning("No Data", "Please upload a file first.");
                return;
            }

            var validRows = _coaRows.Where(r => r.IsValid).ToList();
            if (!validRows.Any())
            {
                UiMessagesEx.Warning("No Valid Rows", "No valid rows to import.");
                return;
            }

            var confirmMessage = chkDryRun.Checked
                ? $"DRY RUN: Validate {validRows.Count} accounts?\n\nNo data will be imported."
                : $"Import {validRows.Count} valid accounts?\n\nThis will add new accounts to the Chart of Accounts.";

            if (MessageBox.Show(confirmMessage, "Confirm Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                ImportExecutionResult result;

                // Show progress dialog for non-dry-run imports
                if (!chkDryRun.Checked)
                {
                    using (var progressForm = new ImportProgressForm())
                    {
                        progressForm.Show();
                        progressForm.StartProgress("Importing Chart of Accounts...");

                        var config = new ImportConfigModal
                        {
                            DryRunMode = false,
                            RollbackHours = 24
                        };

                        result = _bll.ExecuteCOAImport(_coaRows, _currentFileName, config,
                            (sender2, args) =>
                            {
                                progressForm.UpdateProgress(args);
                            });

                        progressForm.CompleteProgress(result.Success, result.Success ? "Import completed!" : "Import failed");
                        System.Threading.Thread.Sleep(500); // Show final status briefly
                    }
                }
                else
                {
                    // Dry run without progress dialog
                    var config = new ImportConfigModal
                    {
                        DryRunMode = true,
                        RollbackHours = 24
                    };

                    result = _bll.ExecuteCOAImport(_coaRows, _currentFileName, config);
                }

                if (result.Success)
                {
                    var message = result.Message + $"\n\nDuration: {result.Duration.TotalSeconds:F1} seconds";
                    UiMessagesEx.Success("Import Complete", message);

                    if (!chkDryRun.Checked)
                    {
                        _coaRows = null;
                        gridCOA.DataSource = null;
                        btnImportCOA.Enabled = false;
                        LoadImportHistory();
                    }
                }
                else
                {
                    var errorDetails = result.Errors != null && result.Errors.Any()
                        ? $"\n\nErrors:\n{string.Join("\n", result.Errors.Take(10).Select(err => $"• Row {err.RowNumber}: {err.ErrorMessage}"))}"
                        : "";

                    if (result.Errors != null && result.Errors.Count > 10)
                        errorDetails += $"\n... and {result.Errors.Count - 10} more errors";

                    UiMessagesEx.Error("Import Failed", result.Message + errorDetails);
                }
            }
            catch (Exception ex)
            {
                UiMessagesEx.Error("Import Error", ex.Message);
            }
        }

        #endregion

        #region Tab 2: Opening Balance

        private void btnDownloadOBTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, "Generating template..."))
                {
                    string templatePath = ImportTemplateGenerator.CreateImportTemplate("OPENING_BALANCE");

                    if (MessageBox.Show(
                        $"Template created successfully!\n\nPath: {templatePath}\n\nDo you want to open it now?",
                        "Template Ready",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(templatePath);
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessagesEx.Error("Template Error", "Failed to generate template: " + ex.Message);
            }
        }

        private void btnUploadOB_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files|*.xls;*.xlsx";
                ofd.Title = "Select Opening Balance Import File";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    using (BusyScope.Show(this, "Loading and validating file..."))
                    {
                        _currentFileName = Path.GetFileName(ofd.FileName);

                        // Read Excel
                        var dataTable = _dll.ReadExcelFile(ofd.FileName);

                        // Parse rows
                        _obRows = _bll.ParseOpeningBalance(dataTable);

                        // Validate
                        var validationResult = _bll.ValidateOpeningBalance(_obRows);

                        // Display in grid
                        DisplayOBPreview(validationResult);

                        btnPostOB.Enabled = validationResult.IsValid;

                        if (validationResult.IsValid)
                        {
                            UiMessagesEx.Information("Validation Complete",
                                $"Total rows: {_obRows.Count}\n" +
                                $"Total Debit: {validationResult.TotalDebit:N2}\n" +
                                $"Total Credit: {validationResult.TotalCredit:N2}\n" +
                                $"Difference: {validationResult.Difference:N2}\n\n" +
                                "Trial balance is balanced! Ready to post.");
                        }
                        else
                        {
                            var errorMsg = $"Validation FAILED:\n\n" +
                                          $"Total Debit: {validationResult.TotalDebit:N2}\n" +
                                          $"Total Credit: {validationResult.TotalCredit:N2}\n" +
                                          $"Difference: {validationResult.Difference:N2}\n\n" +
                                          string.Join("\n", validationResult.Errors.Select(err => $"• {err.ErrorMessage}"));

                            UiMessagesEx.Error("Validation Failed", errorMsg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UiMessagesEx.Error("Upload Error", "Failed to read file: " + ex.Message);
                }
            }
        }

        private void DisplayOBPreview(OpeningBalanceValidationResult validationResult)
        {
            var dt = new DataTable();
            dt.Columns.Add("Row", typeof(int));
            dt.Columns.Add("Valid", typeof(bool));
            dt.Columns.Add("Account Code", typeof(string));
            dt.Columns.Add("Account Name", typeof(string));
            dt.Columns.Add("Debit Amount", typeof(decimal));
            dt.Columns.Add("Credit Amount", typeof(decimal));
            dt.Columns.Add("Remarks", typeof(string));
            dt.Columns.Add("Validation Error", typeof(string));

            foreach (var row in _obRows)
            {
                dt.Rows.Add(
                    row.RowNumber,
                    row.IsValid,
                    row.AccountCode,
                    row.AccountName,
                    row.DebitAmount,
                    row.CreditAmount,
                    row.Remarks,
                    row.ValidationError
                );
            }

            gridOB.DataSource = dt;
            gridOB.Columns["Valid"].Visible = false;
            gridOB.Columns["Row"].Width = 60;
            gridOB.Columns["Account Code"].Width = 120;
            gridOB.Columns["Debit Amount"].DefaultCellStyle.Format = "N2";
            gridOB.Columns["Debit Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridOB.Columns["Credit Amount"].DefaultCellStyle.Format = "N2";
            gridOB.Columns["Credit Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Color invalid rows
            foreach (DataGridViewRow gridRow in gridOB.Rows)
            {
                if (gridRow.Cells["Valid"].Value != null && !(bool)gridRow.Cells["Valid"].Value)
                {
                    gridRow.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
                }
            }

            // Update balance label
            lblOBBalance.Text = validationResult.IsValid
                ? $"✓ Dr: {validationResult.TotalDebit:N2} | Cr: {validationResult.TotalCredit:N2} | Diff: {validationResult.Difference:N2} (BALANCED)"
                : $"✗ Dr: {validationResult.TotalDebit:N2} | Cr: {validationResult.TotalCredit:N2} | Diff: {validationResult.Difference:N2} (NOT BALANCED)";

            lblOBBalance.ForeColor = validationResult.IsValid ? Color.FromArgb(46, 125, 50) : Color.FromArgb(211, 47, 47);
        }

        private void btnPostOB_Click(object sender, EventArgs e)
        {
            if (_obRows == null || !_obRows.Any())
            {
                UiMessagesEx.Warning("No Data", "Please upload a file first.");
                return;
            }

            var confirmMessage = chkDryRun.Checked
                ? $"DRY RUN: Validate Opening Balance?\n\nNo voucher will be created."
                : $"Post Opening Balance as of {dtpOBDate.Value:dd-MMM-yyyy}?\n\nThis will create a journal voucher.";

            if (MessageBox.Show(confirmMessage, "Confirm Post", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                ImportExecutionResult result;

                // Show progress dialog for non-dry-run imports
                if (!chkDryRun.Checked)
                {
                    using (var progressForm = new ImportProgressForm())
                    {
                        progressForm.Show();
                        progressForm.StartProgress("Posting Opening Balance...");

                        var config = new ImportConfigModal
                        {
                            DryRunMode = false,
                            RollbackHours = 24
                        };

                        result = _bll.ExecuteOpeningBalanceImport(_obRows, dtpOBDate.Value, _currentFileName, config,
                            (sender2, args) =>
                            {
                                progressForm.UpdateProgress(args);
                            });

                        progressForm.CompleteProgress(result.Success, result.Success ? "Opening balance posted!" : "Post failed");
                        System.Threading.Thread.Sleep(500);
                    }
                }
                else
                {
                    // Dry run without progress dialog
                    var config = new ImportConfigModal
                    {
                        DryRunMode = true,
                        RollbackHours = 24
                    };

                    result = _bll.ExecuteOpeningBalanceImport(_obRows, dtpOBDate.Value, _currentFileName, config);
                }

                if (result.Success)
                {
                    var message = result.Message + $"\n\nDuration: {result.Duration.TotalSeconds:F1} seconds";
                    if (result.VouchersCreated > 0)
                        message += $"\nVouchers created: {result.VouchersCreated}";

                    UiMessagesEx.Success("Post Complete", message);

                    if (!chkDryRun.Checked)
                    {
                        _obRows = null;
                        gridOB.DataSource = null;
                        btnPostOB.Enabled = false;
                        lblOBBalance.Text = "Dr: 0.00 | Cr: 0.00 | Diff: 0.00";
                        LoadImportHistory();
                    }
                }
                else
                {
                    var errorDetails = result.Errors != null && result.Errors.Any()
                        ? $"\n\nErrors:\n{string.Join("\n", result.Errors.Take(10).Select(err => $"• {err.ErrorMessage}"))}"
                        : "";

                    if (result.Errors != null && result.Errors.Count > 10)
                        errorDetails += $"\n... and {result.Errors.Count - 10} more errors";

                    UiMessagesEx.Error("Post Failed", result.Message + errorDetails);
                }
            }
            catch (Exception ex)
            {
                UiMessagesEx.Error("Post Error", ex.Message);
            }
        }

        #endregion

        #region Tab 4: Historical Journal Entries

        private void btnDownloadJournalTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, "Generating template..."))
                {
                    string templatePath = ImportTemplateGenerator.CreateImportTemplate("JOURNAL_HISTORY");

                    if (MessageBox.Show(
                        $"Template created successfully!\n\nPath: {templatePath}\n\nDo you want to open it now?",
                        "Template Ready",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(templatePath);
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessagesEx.Error("Template Error", "Failed to generate template: " + ex.Message);
            }
        }

        private void btnUploadJournal_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files|*.xls;*.xlsx";
                ofd.Title = "Select Historical Journal Import File";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    using (BusyScope.Show(this, "Loading and validating file..."))
                    {
                        _currentFileName = Path.GetFileName(ofd.FileName);

                        // Read Excel
                        var dataTable = _dll.ReadExcelFile(ofd.FileName);

                        // Parse rows
                        _journalRows = _bll.ParseJournalEntries(dataTable);

                        // Validate and group
                        _journalVouchers = _bll.ValidateAndGroupJournals(_journalRows);

                        // Display in grid
                        DisplayJournalPreview();

                        var balancedCount = _journalVouchers.Count(v => v.IsBalanced);
                        btnImportJournal.Enabled = _journalVouchers.Any() && balancedCount == _journalVouchers.Count;

                        var validEntries = _journalRows.Count(r => r.IsValid);
                        var unbalancedCount = _journalVouchers.Count - balancedCount;

                        var message = $"Total vouchers: {_journalVouchers.Count}\n" +
                                     $"Balanced: {balancedCount}\n" +
                                     $"Unbalanced: {unbalancedCount}\n" +
                                     $"Total entries: {_journalRows.Count}\n" +
                                     $"Valid entries: {validEntries}";

                        if (unbalancedCount > 0)
                        {
                            UiMessagesEx.Warning("Validation Warning", message + "\n\nUnbalanced vouchers are highlighted in red.");
                        }
                        else
                        {
                            UiMessagesEx.Information("Validation Complete", message + "\n\nAll vouchers are balanced!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    UiMessagesEx.Error("Upload Error", "Failed to read file: " + ex.Message);
                }
            }
        }

        private void DisplayJournalPreview()
        {
            var dt = new DataTable();
            dt.Columns.Add("Row", typeof(int));
            dt.Columns.Add("Valid", typeof(bool));
            dt.Columns.Add("Voucher No", typeof(string));
            dt.Columns.Add("Voucher Date", typeof(DateTime));
            dt.Columns.Add("Account Code", typeof(string));
            dt.Columns.Add("Debit", typeof(decimal));
            dt.Columns.Add("Credit", typeof(decimal));
            dt.Columns.Add("Narration", typeof(string));
            dt.Columns.Add("Validation Error", typeof(string));

            // Build a dictionary of unbalanced vouchers
            var unbalancedVoucherNos = _journalVouchers
                .Where(v => !v.IsBalanced)
                .Select(v => v.VoucherNo)
                .ToHashSet();

            foreach (var row in _journalRows)
            {
                bool isUnbalanced = unbalancedVoucherNos.Contains(row.VoucherNo);

                dt.Rows.Add(
                    row.RowNumber,
                    row.IsValid && !isUnbalanced,
                    row.VoucherNo,
                    row.VoucherDate,
                    row.AccountCode,
                    row.DebitAmount,
                    row.CreditAmount,
                    row.Narration,
                    !row.IsValid ? row.ValidationError : (isUnbalanced ? "Voucher not balanced" : "")
                );
            }

            gridJournal.DataSource = dt;
            gridJournal.Columns["Valid"].Visible = false;
            gridJournal.Columns["Row"].Width = 60;
            gridJournal.Columns["Voucher No"].Width = 120;
            gridJournal.Columns["Debit"].DefaultCellStyle.Format = "N2";
            gridJournal.Columns["Debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridJournal.Columns["Credit"].DefaultCellStyle.Format = "N2";
            gridJournal.Columns["Credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Color invalid/unbalanced rows
            foreach (DataGridViewRow gridRow in gridJournal.Rows)
            {
                if (gridRow.Cells["Valid"].Value != null && !(bool)gridRow.Cells["Valid"].Value)
                {
                    gridRow.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
                }
            }

            // Update summary label
            var balancedCount = _journalVouchers.Count(v => v.IsBalanced);
            lblJournalSummary.Text = $"Vouchers: {_journalVouchers.Count} | Entries: {_journalRows.Count} | Balanced: {balancedCount}";
            lblJournalSummary.ForeColor = balancedCount == _journalVouchers.Count
                ? Color.FromArgb(46, 125, 50)
                : Color.FromArgb(211, 47, 47);
        }

        private void btnImportJournal_Click(object sender, EventArgs e)
        {
            if (_journalRows == null || !_journalRows.Any())
            {
                UiMessagesEx.Warning("No Data", "Please upload a file first.");
                return;
            }

            var balancedCount = _journalVouchers.Count(v => v.IsBalanced);
            if (balancedCount != _journalVouchers.Count)
            {
                UiMessagesEx.Warning("Unbalanced Vouchers", "All vouchers must be balanced before import.");
                return;
            }

            var confirmMessage = chkDryRun.Checked
                ? $"DRY RUN: Validate {_journalVouchers.Count} vouchers?\n\nNo data will be imported."
                : $"Import {_journalVouchers.Count} vouchers with {_journalRows.Count(r => r.IsValid)} entries?\n\nThis will create historical journal entries.";

            if (MessageBox.Show(confirmMessage, "Confirm Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                ImportExecutionResult result;

                // Show progress dialog for non-dry-run imports
                if (!chkDryRun.Checked)
                {
                    using (var progressForm = new ImportProgressForm())
                    {
                        progressForm.Show();
                        progressForm.StartProgress("Importing Historical Journal Entries...");

                        var config = new ImportConfigModal
                        {
                            DryRunMode = false,
                            RollbackHours = 24,
                            BatchSize = 1000
                        };

                        result = _bll.ExecuteJournalImport(_journalRows, _currentFileName, config,
                            (sender2, args) =>
                            {
                                progressForm.UpdateProgress(args);
                            },
                            () => progressForm.ShouldCancel());

                        if (result.WasCancelled)
                        {
                            progressForm.CompleteProgress(false, "Import cancelled and rolled back");
                        }
                        else
                        {
                            progressForm.CompleteProgress(result.Success, result.Success ? "Import completed!" : "Import failed");
                        }
                        System.Threading.Thread.Sleep(500);
                    }
                }
                else
                {
                    // Dry run without progress dialog
                    var config = new ImportConfigModal
                    {
                        DryRunMode = true,
                        RollbackHours = 24,
                        BatchSize = 1000
                    };

                    result = _bll.ExecuteJournalImport(_journalRows, _currentFileName, config);
                }

                if (result.WasCancelled)
                {
                    UiMessagesEx.Warning("Import Cancelled", "The import was cancelled by user.\n\nAll changes have been rolled back.");
                    LoadImportHistory();
                }
                else if (result.Success)
                {
                    var message = result.Message + $"\n\nDuration: {result.Duration.TotalSeconds:F1} seconds";
                    if (result.VouchersCreated > 0)
                        message += $"\nVouchers created: {result.VouchersCreated}";

                    UiMessagesEx.Success("Import Complete", message);

                    if (!chkDryRun.Checked)
                    {
                        _journalRows = null;
                        _journalVouchers = null;
                        gridJournal.DataSource = null;
                        btnImportJournal.Enabled = false;
                        lblJournalSummary.Text = "Vouchers: 0 | Entries: 0 | Balanced: 0";
                        LoadImportHistory();
                    }
                }
                else
                {
                    var errorDetails = result.Errors != null && result.Errors.Any()
                        ? $"\n\nErrors:\n{string.Join("\n", result.Errors.Take(10).Select(err => $"• {err.ErrorMessage}"))}"
                        : "";

                    if (result.Errors != null && result.Errors.Count > 10)
                        errorDetails += $"\n... and {result.Errors.Count - 10} more errors";

                    UiMessagesEx.Error("Import Failed", result.Message + errorDetails);
                }
            }
            catch (Exception ex)
            {
                UiMessagesEx.Error("Import Error", ex.Message);
            }
        }

        #endregion

        #region Tab 5: Import History & Rollback

        private void LoadImportHistory()
        {
            try
            {
                var history = _bll.GetImportHistory();
                gridHistory.DataSource = history;

                if (gridHistory.Columns.Count > 0)
                {
                    gridHistory.Columns["session_id"].Visible = false;
                    gridHistory.Columns["import_type"].HeaderText = "Type";
                    gridHistory.Columns["file_name"].HeaderText = "File Name";
                    gridHistory.Columns["imported_at"].HeaderText = "Imported At";
                    gridHistory.Columns["imported_at"].DefaultCellStyle.Format = "dd-MMM-yyyy HH:mm";
                    gridHistory.Columns["status"].HeaderText = "Status";
                    gridHistory.Columns["total_rows"].HeaderText = "Total";
                    gridHistory.Columns["imported_rows"].HeaderText = "Imported";
                    gridHistory.Columns["skipped_rows"].HeaderText = "Skipped";
                    gridHistory.Columns["error_rows"].HeaderText = "Errors";
                    gridHistory.Columns["voucher_count"].HeaderText = "Vouchers";
                    gridHistory.Columns["can_rollback"].HeaderText = "Can Rollback";
                    gridHistory.Columns["rollback_available_until"].HeaderText = "Rollback Until";
                    gridHistory.Columns["rollback_available_until"].DefaultCellStyle.Format = "dd-MMM-yyyy HH:mm";

                    // Color code by status
                    foreach (DataGridViewRow row in gridHistory.Rows)
                    {
                        if (row.Cells["status"].Value != null)
                        {
                            var status = row.Cells["status"].Value.ToString();
                            switch (status)
                            {
                                case "Completed":
                                    row.DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233); // Light green
                                    break;
                                case "Failed":
                                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238); // Light red
                                    break;
                                case "RolledBack":
                                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 243, 224); // Light orange
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessagesEx.Error("Load Error", "Failed to load import history: " + ex.Message);
            }
        }

        private void btnRefreshHistory_Click(object sender, EventArgs e)
        {
            LoadImportHistory();
        }

        private void btnRollback_Click(object sender, EventArgs e)
        {
            if (gridHistory.SelectedRows.Count == 0)
            {
                UiMessagesEx.Warning("No Selection", "Please select an import session to rollback.");
                return;
            }

            var row = gridHistory.SelectedRows[0];
            var sessionId = Convert.ToInt32(row.Cells["session_id"].Value);
            var canRollback = Convert.ToBoolean(row.Cells["can_rollback"].Value);
            var status = row.Cells["status"].Value.ToString();
            var importType = row.Cells["import_type"].Value.ToString();
            var fileName = row.Cells["file_name"].Value.ToString();

            if (!canRollback)
            {
                UiMessagesEx.Warning("Rollback Not Available", "This import session cannot be rolled back.\n\nReasons:\n• Status is not 'Completed'\n• Rollback time window has expired\n• Already rolled back");
                return;
            }

            if (MessageBox.Show(
                $"ROLLBACK IMPORT SESSION?\n\n" +
                $"Type: {importType}\n" +
                $"File: {fileName}\n" +
                $"Status: {status}\n\n" +
                $"This will DELETE all vouchers and entries created by this import.\n\n" +
                $"Are you sure?",
                "Confirm Rollback",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                ImportResultModal result;

                using (BusyScope.Show(this, "Rolling back import..."))
                {
                    result = _bll.RollbackImport(sessionId);
                }

                if (result.Success)
                {
                    UiMessagesEx.Success("Rollback Complete", result.Message);
                    LoadImportHistory();
                }
                else
                {
                    UiMessagesEx.Error("Rollback Failed", result.Message);
                }
            }
            catch (Exception ex)
            {
                UiMessagesEx.Error("Rollback Error", ex.Message);
            }
        }

        #endregion
    }
}
