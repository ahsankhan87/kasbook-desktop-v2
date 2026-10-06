using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using POS.BLL;
using pos.UI;
using pos.UI.Busy;
using POS.Core;

namespace pos.Reports.Accounts
{
    /// <summary>
    /// Daily Cash and Bank Opening/Closing Report
    /// Shows opening balance, receipts, payments, and closing balance for cash and bank accounts
    /// Supports single-day and date-range modes with variance tracking
    /// </summary>
    public partial class FrmDailyCashBankReport : Form
    {
        private readonly AccountsBLL _accountsBll = new AccountsBLL();
        private DataSet _reportData;
        private bool _isSingleDayMode = true;
        private bool _isConsolidatedView = true;

        public FrmDailyCashBankReport()
        {
            InitializeComponent();
            WireEvents();
        }

        private void WireEvents()
        {
            Load += FrmDailyCashBankReport_Load;
            btnLoad.Click += (s, e) => LoadReport();
            btnPrint.Click += (s, e) => PrintReport();
            btnExport.Click += (s, e) => ExportReport();
            rbSingleDay.CheckedChanged += (s, e) => OnModeChanged();
            rbDateRange.CheckedChanged += (s, e) => OnModeChanged();
            rbConsolidated.CheckedChanged += (s, e) => OnViewChanged();
            rbByAccount.CheckedChanged += (s, e) => OnViewChanged();
            txtActualCash.TextChanged += (s, e) => CalculateVariance();
            txtActualBank.TextChanged += (s, e) => CalculateVariance();
        }

        private void FrmDailyCashBankReport_Load(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Initializing...", "جاري التهيئة...")))
                {
                    AppTheme.Apply(this);
                    InitializeForm();
                    LoadReport();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(UiMessages.T("Error loading form", "خطأ في تحميل النموذج"), ex.Message);
            }
        }

        private void InitializeForm()
        {
            // Set default dates
            dtpSingleDate.Value = DateTime.Today;
            dtpFromDate.Value = DateTime.Today.AddDays(-7);
            dtpToDate.Value = DateTime.Today;

            // Set default mode
            rbSingleDay.Checked = true;
            rbConsolidated.Checked = true;

            // Initialize grid
            InitializeGrid();

            // Style variance panel
            pnlVariance.BackColor = Color.FromArgb(240, 248, 255);
            pnlVariance.BorderStyle = BorderStyle.FixedSingle;
        }

        private void InitializeGrid()
        {
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.ReadOnly = true;
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvReport.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void OnModeChanged()
        {
            _isSingleDayMode = rbSingleDay.Checked;

            // Show/hide appropriate date controls
            dtpSingleDate.Visible = _isSingleDayMode;
            lblSingleDate.Visible = _isSingleDayMode;

            dtpFromDate.Visible = !_isSingleDayMode;
            dtpToDate.Visible = !_isSingleDayMode;
            lblFromDate.Visible = !_isSingleDayMode;
            lblToDate.Visible = !_isSingleDayMode;

            // Variance panel only meaningful in single-day mode
            pnlVariance.Visible = _isSingleDayMode;
        }

        private void OnViewChanged()
        {
            _isConsolidatedView = rbConsolidated.Checked;

            // Reload grid with appropriate data
            if (_reportData != null)
            {
                BindGrid();
            }
        }

        private void LoadReport()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading report...", "جاري تحميل التقرير...")))
                {
                    DateTime fromDate, toDate;

                    if (_isSingleDayMode)
                    {
                        fromDate = dtpSingleDate.Value.Date;
                        toDate = dtpSingleDate.Value.Date;
                    }
                    else
                    {
                        fromDate = dtpFromDate.Value.Date;
                        toDate = dtpToDate.Value.Date;
                    }

                    if (fromDate > toDate)
                    {
                        UiMessages.ShowWarning(UiMessages.T("Invalid Date Range", "نطاق تاريخ غير صالح"), UiMessages.T("From date cannot be after To date", "لا يمكن أن يكون تاريخ البداية بعد تاريخ النهاية"));
                        return;
                    }

                    // Load data from BLL
                    _reportData = _accountsBll.GetDailyCashBankReport(fromDate, toDate, null, UsersModal.logged_in_branch_id);

                    if (_reportData == null || _reportData.Tables.Count == 0)
                    {
                        UiMessages.ShowInfo(UiMessages.T("No Data", "لا توجد بيانات"), UiMessages.T("No data found for the selected period", "لم يتم العثور على بيانات للفترة المحددة"));
                        dgvReport.DataSource = null;
                        return;
                    }

                    // Bind grid
                    BindGrid();

                    // Update variance panel if in single-day mode
                    if (_isSingleDayMode)
                    {
                        UpdateVariancePanel();
                    }

                    // Log action
                    POS.DLL.Log.LogAction("View Daily Cash/Bank Report", 
                        $"Mode: {(_isSingleDayMode ? "Single Day" : "Range")}, View: {(_isConsolidatedView ? "Consolidated" : "By Account")}, From: {fromDate:yyyy-MM-dd}, To: {toDate:yyyy-MM-dd}", 
                        UsersModal.logged_in_userid, 
                        UsersModal.logged_in_branch_id);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(UiMessages.T("Error loading report", "خطأ في تحميل التقرير"), ex.Message);
            }
        }

        private void BindGrid()
        {
            if (_reportData == null) return;

            DataTable dataTable = _isConsolidatedView 
                ? _reportData.Tables["ConsolidatedDaily"] 
                : _reportData.Tables["ByAccountDetail"];

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                dgvReport.DataSource = null;
                return;
            }

            dgvReport.DataSource = dataTable;
            FormatGridColumns();
        }

        private void FormatGridColumns()
        {
            if (dgvReport.Columns.Count == 0) return;

            // Format date columns
            if (dgvReport.Columns.Contains("transaction_day"))
            {
                dgvReport.Columns["transaction_day"].HeaderText = "Date";
                dgvReport.Columns["transaction_day"].DefaultCellStyle.Format = "yyyy-MM-dd";
                dgvReport.Columns["transaction_day"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Format currency columns
            string[] currencyColumns = { 
                "cash_opening_balance", "cash_receipts", "cash_payments", "cash_closing_balance",
                "bank_opening_balance", "bank_receipts", "bank_payments", "bank_closing_balance",
                "total_opening_balance", "total_receipts", "total_payments", "total_closing_balance",
                "opening_balance", "receipts", "payments", "closing_balance"
            };

            foreach (string colName in currencyColumns)
            {
                if (dgvReport.Columns.Contains(colName))
                {
                    dgvReport.Columns[colName].DefaultCellStyle.Format = "N2";
                    dgvReport.Columns[colName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvReport.Columns[colName].HeaderText = FormatColumnHeader(colName);
                }
            }

            // Format account columns if in by-account view
            if (!_isConsolidatedView)
            {
                if (dgvReport.Columns.Contains("account_code"))
                    dgvReport.Columns["account_code"].HeaderText = UiMessages.T("Account Code", "كود الحساب");
                if (dgvReport.Columns.Contains("account_name"))
                    dgvReport.Columns["account_name"].HeaderText = UiMessages.T("Account Name", "اسم الحساب");
                if (dgvReport.Columns.Contains("account_type"))
                    dgvReport.Columns["account_type"].HeaderText = UiMessages.T("Type", "النوع");
            }

            dgvReport.AutoResizeColumns();
        }

        private string FormatColumnHeader(string columnName)
        {
            // Use bilingual header translation
            string header = columnName
                .Replace("_", " ")
                .Replace("cash ", "Cash ")
                .Replace("bank ", "Bank ")
                .Replace("total ", "Total ")
                .Replace("opening", "Opening")
                .Replace("closing", "Closing")
                .Replace("receipts", "Receipts")
                .Replace("payments", "Payments")
                .Replace("balance", "Balance");

            // Map to bilingual headers
            string[] parts = header.Split(new[] { " " }, StringSplitOptions.None);
            string result = header;

            if (header.Contains("Cash Opening Balance"))
                result = UiMessages.T("Cash Opening Balance", "رصيد النقد الافتتاحي");
            else if (header.Contains("Cash Receipts"))
                result = UiMessages.T("Cash Receipts", "إيصالات النقد");
            else if (header.Contains("Cash Payments"))
                result = UiMessages.T("Cash Payments", "دفعات النقد");
            else if (header.Contains("Cash Closing Balance"))
                result = UiMessages.T("Cash Closing Balance", "رصيد النقد الختامي");
            else if (header.Contains("Bank Opening Balance"))
                result = UiMessages.T("Bank Opening Balance", "رصيد البنك الافتتاحي");
            else if (header.Contains("Bank Receipts"))
                result = UiMessages.T("Bank Receipts", "إيصالات البنك");
            else if (header.Contains("Bank Payments"))
                result = UiMessages.T("Bank Payments", "دفعات البنك");
            else if (header.Contains("Bank Closing Balance"))
                result = UiMessages.T("Bank Closing Balance", "رصيد البنك الختامي");
            else if (header.Contains("Total Opening Balance"))
                result = UiMessages.T("Total Opening Balance", "إجمالي الرصيد الافتتاحي");
            else if (header.Contains("Total Receipts"))
                result = UiMessages.T("Total Receipts", "إجمالي الإيصالات");
            else if (header.Contains("Total Payments"))
                result = UiMessages.T("Total Payments", "إجمالي الدفعات");
            else if (header.Contains("Total Closing Balance"))
                result = UiMessages.T("Total Closing Balance", "إجمالي الرصيد الختامي");

            return result;
        }

        private void UpdateVariancePanel()
        {
            if (_reportData == null || !_reportData.Tables.Contains("Summary")) return;

            DataTable summary = _reportData.Tables["Summary"];
            if (summary.Rows.Count == 0) return;

            DataRow row = summary.Rows[0];

            decimal cashClosing = row["cash_closing_balance"] != DBNull.Value ? Convert.ToDecimal(row["cash_closing_balance"]) : 0m;
            decimal bankClosing = row["bank_closing_balance"] != DBNull.Value ? Convert.ToDecimal(row["bank_closing_balance"]) : 0m;

            lblSystemCashClosing.Text = cashClosing.ToString("N2");
            lblSystemBankClosing.Text = bankClosing.ToString("N2");

            txtActualCash.Tag = cashClosing;
            txtActualBank.Tag = bankClosing;

            // Clear manual inputs
            txtActualCash.Text = "";
            txtActualBank.Text = "";
        }

        private void CalculateVariance()
        {
            decimal systemCash = txtActualCash.Tag != null ? Convert.ToDecimal(txtActualCash.Tag) : 0m;
            decimal systemBank = txtActualBank.Tag != null ? Convert.ToDecimal(txtActualBank.Tag) : 0m;

            decimal actualCash = 0m;
            decimal actualBank = 0m;

            decimal.TryParse(txtActualCash.Text, out actualCash);
            decimal.TryParse(txtActualBank.Text, out actualBank);

            decimal cashVariance = actualCash - systemCash;
            decimal bankVariance = actualBank - systemBank;
            decimal totalVariance = cashVariance + bankVariance;

            lblCashVariance.Text = cashVariance.ToString("N2");
            lblBankVariance.Text = bankVariance.ToString("N2");
            lblTotalVariance.Text = totalVariance.ToString("N2");

            // Color code variances
            lblCashVariance.ForeColor = cashVariance >= 0 ? Color.Green : Color.Red;
            lblBankVariance.ForeColor = bankVariance >= 0 ? Color.Green : Color.Red;
            lblTotalVariance.ForeColor = totalVariance >= 0 ? Color.Green : Color.Red;
        }

        private void PrintReport()
        {
            try
            {
                if (dgvReport.Rows.Count == 0)
                {
                    UiMessages.ShowWarning(UiMessages.T("No Data", "لا توجد بيانات"), UiMessages.T("Please load data first", "يرجى تحميل البيانات أولاً"));
                    return;
                }

                // TODO: Implement printing using DGVPrinterHelper
                UiMessages.ShowInfo(UiMessages.T("Print", "طباعة"), UiMessages.T("Print functionality will be implemented", "سيتم تنفيذ وظيفة الطباعة"));

                // Log action
                POS.DLL.Log.LogAction(UiMessages.T("Print Daily Cash/Bank Report", "طباعة تقرير النقد والبنك اليومي"),
                    $"Mode: {(_isSingleDayMode ? "Single Day" : "Range")}", 
                    UsersModal.logged_in_userid, 
                    UsersModal.logged_in_branch_id);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(UiMessages.T("Print Error", "خطأ في الطباعة"), ex.Message);
            }
        }

        private void ExportReport()
        {
            try
            {
                if (dgvReport.Rows.Count == 0)
                {
                    UiMessages.ShowWarning(UiMessages.T("No Data", "لا توجد بيانات"), UiMessages.T("Please load data first", "يرجى تحميل البيانات أولاً"));
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx)|*.xlsx",
                    FileName = $"DailyCashBank_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportToCsv(sfd.FileName);
                    UiMessages.ShowInfo("Success", "Data exported successfully");

                    // Log action
                    POS.DLL.Log.LogAction("Export Daily Cash/Bank Report", 
                        $"File: {sfd.FileName}", 
                        UsersModal.logged_in_userid, 
                        UsersModal.logged_in_branch_id);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Export Error", ex.Message);
            }
        }

        private void ExportToCsv(string filePath)
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath))
            {
                // Write headers
                for (int i = 0; i < dgvReport.Columns.Count; i++)
                {
                    writer.Write(dgvReport.Columns[i].HeaderText);
                    if (i < dgvReport.Columns.Count - 1) writer.Write(",");
                }
                writer.WriteLine();

                // Write data
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    for (int i = 0; i < dgvReport.Columns.Count; i++)
                    {
                        string value = row.Cells[i].Value?.ToString() ?? "";
                        value = value.Replace("\"", "\"\"");
                        if (value.Contains(",") || value.Contains("\""))
                            value = "\"" + value + "\"";

                        writer.Write(value);
                        if (i < dgvReport.Columns.Count - 1) writer.Write(",");
                    }
                    writer.WriteLine();
                }
            }
        }
    }
}
