using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DGVPrinterHelper;
using POS.BLL;
using POS.Core;
using pos.UI;
using pos.UI.Busy;

namespace pos.Reports.Financial
{
    /// <summary>
    /// Professional Trial Balance Report
    /// Displays opening balance, total debits, credits, and closing balance for all accounts
    /// </summary>
    public partial class FrmTrialBalanceReport : Form
    {
        private readonly AccountsBLL _accountsBll = new AccountsBLL();
        private DataTable _reportData;

        public FrmTrialBalanceReport()
        {
            InitializeComponent();
            WireEvents();
        }

        private void WireEvents()
        {
            Load += FrmTrialBalanceReport_Load;
            btnLoad.Click += (s, e) => LoadReport();
            btnPrint.Click += (s, e) => PrintReport();
            btnExport.Click += (s, e) => ExportReport();
            cmbDateRange.SelectedIndexChanged += (s, e) => OnDateRangeChanged();
            dtpFromDate.ValueChanged += (s, e) => dtpToDate.MinDate = dtpFromDate.Value;
        }

        private void FrmTrialBalanceReport_Load(object sender, EventArgs e)
        {
            try
            {
                //AppTheme.Apply(this);
                InitializeForm();
                LoadReport();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Error loading form", ex.Message);
            }
        }

        private void InitializeForm()
        {
            // Set default dates
            dtpFromDate.Value = DateTime.Today.AddMonths(-1);
            dtpToDate.Value = DateTime.Today;

            // Initialize date range combo
            cmbDateRange.Items.AddRange(new string[]
            {
                "Custom", "Today", "This Week", "Last Week",
                "This Month", "Last Month", "Last 3 Months", "Last 6 Months",
                "This Year", "Year to Date (YTD)"
            });
            cmbDateRange.SelectedIndex = 4; // This Month

            // Initialize grid
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.ReadOnly = true;
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.EnableHeadersVisualStyles = false;
            dgvReport.CellBorderStyle = DataGridViewCellBorderStyle.None;

            // Style header - light professional blue
            dgvReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(237, 242, 247);
            dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(52, 73, 94);
            dgvReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.ColumnHeadersHeight = 40;

            // Style alternating rows - very light gray
            dgvReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvReport.DefaultCellStyle.BackColor = Color.White;
            dgvReport.DefaultCellStyle.ForeColor = Color.FromArgb(52, 73, 94);
            dgvReport.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvReport.DefaultCellStyle.SelectionBackColor = Color.FromArgb(155, 195, 228);
            dgvReport.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvReport.RowTemplate.Height = 28;
            dgvReport.BorderStyle = BorderStyle.None;
            dgvReport.GridColor = Color.FromArgb(230, 235, 240);
        }

        private void OnDateRangeChanged()
        {
            DateTime today = DateTime.Today;
            DateTime startDate = today;
            DateTime endDate = today;

            switch (cmbDateRange.SelectedItem?.ToString() ?? "Custom")
            {
                case "Custom":
                    pnlDatePickers.Visible = true;
                    return;

                case "Today":
                    startDate = endDate = today;
                    break;

                case "This Week":
                    startDate = today.AddDays(-(int)today.DayOfWeek);
                    endDate = startDate.AddDays(6);
                    break;

                case "Last Week":
                    startDate = today.AddDays(-(int)today.DayOfWeek - 7);
                    endDate = startDate.AddDays(6);
                    break;

                case "This Month":
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;

                case "Last Month":
                    startDate = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;

                case "Last 3 Months":
                    startDate = today.AddMonths(-3);
                    endDate = today;
                    break;

                case "Last 6 Months":
                    startDate = today.AddMonths(-6);
                    endDate = today;
                    break;

                case "This Year":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = new DateTime(today.Year, 12, 31);
                    break;

                case "Year to Date (YTD)":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = today;
                    break;
            }

            dtpFromDate.Value = startDate;
            dtpToDate.Value = endDate;
            pnlDatePickers.Visible = (cmbDateRange.SelectedItem?.ToString() ?? "") == "Custom";
        }

        private void LoadReport()
        {
            try
            {
                using (BusyScope.Show(this, "Loading Trial Balance Report..."))
                {
                    DateTime fromDate = dtpFromDate.Value.Date;
                    DateTime toDate = dtpToDate.Value.Date;

                    if (fromDate > toDate)
                    {
                        UiMessages.ShowWarning("Invalid Date Range", "From date cannot be after To date");
                        return;
                    }

                    // Load data from BLL
                    _reportData = _accountsBll.TrialBalanceReport(fromDate, toDate);

                    if (_reportData == null || _reportData.Rows.Count == 0)
                    {
                        UiMessages.ShowInfo("No Data", "No trial balance data found for the selected period");
                        dgvReport.DataSource = null;
                        return;
                    }

                    // Add totals row
                    AddTotalsRow(_reportData);

                    // Bind to grid
                    BindGrid();

                    // Log action
                    POS.DLL.Log.LogAction("View Trial Balance Report",
                        $"From: {fromDate:yyyy-MM-dd}, To: {toDate:yyyy-MM-dd}",
                        UsersModal.logged_in_userid,
                        UsersModal.logged_in_branch_id);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Error loading report", ex.Message);
            }
        }

        private void AddTotalsRow(DataTable dataTable)
        {
            decimal totalDebit = 0;
            decimal totalCredit = 0;
            decimal totalBalance = 0;

            foreach (DataRow row in dataTable.Rows)
            {
                if (row["TotalDebit"] != DBNull.Value)
                    totalDebit += Convert.ToDecimal(row["TotalDebit"]);
                if (row["TotalCredit"] != DBNull.Value)
                    totalCredit += Convert.ToDecimal(row["TotalCredit"]);
                if (row["ClosingBalance"] != DBNull.Value)
                    totalBalance += Convert.ToDecimal(row["ClosingBalance"]);
            }

            DataRow totalsRow = dataTable.NewRow();
            totalsRow["AccountName"] = "═══ TOTALS ═══";
            totalsRow["TotalDebit"] = totalDebit;
            totalsRow["TotalCredit"] = totalCredit;
            totalsRow["ClosingBalance"] = totalBalance;
            dataTable.Rows.Add(totalsRow);
        }

        private void BindGrid()
        {
            if (_reportData == null) return;

            dgvReport.DataSource = null;
            dgvReport.DataSource = _reportData;

            // Configure columns with AutoFit for content-based sizing
            if (dgvReport.Columns.Count > 0)
            {
                dgvReport.Columns["AccountName"].HeaderText = "Account Name";
                dgvReport.Columns["AccountName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvReport.Columns["AccountName"].DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                dgvReport.Columns["AccountName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                if (dgvReport.Columns.Contains("TotalDebit"))
                {
                    dgvReport.Columns["TotalDebit"].HeaderText = "Debit";
                    dgvReport.Columns["TotalDebit"].DefaultCellStyle.Format = "N2";
                    dgvReport.Columns["TotalDebit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvReport.Columns["TotalDebit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                if (dgvReport.Columns.Contains("TotalCredit"))
                {
                    dgvReport.Columns["TotalCredit"].HeaderText = "Credit";
                    dgvReport.Columns["TotalCredit"].DefaultCellStyle.Format = "N2";
                    dgvReport.Columns["TotalCredit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvReport.Columns["TotalCredit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                if (dgvReport.Columns.Contains("ClosingBalance"))
                {
                    dgvReport.Columns["ClosingBalance"].HeaderText = "Balance";
                    dgvReport.Columns["ClosingBalance"].DefaultCellStyle.Format = "N2";
                    dgvReport.Columns["ClosingBalance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvReport.Columns["ClosingBalance"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                // Hide ID columns
                foreach (DataGridViewColumn col in dgvReport.Columns)
                {
                    if (col.Name.Contains("ID") || col.Name.Contains("Code"))
                        col.Visible = false;
                }
            }

            // Format totals row
            FormatTotalsRow();
        }

        private void FormatTotalsRow()
        {
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (row.Cells["AccountName"].Value?.ToString().Contains("TOTALS") == true)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
                    row.DefaultCellStyle.ForeColor = Color.White;
                    row.DefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
                    row.DefaultCellStyle.SelectionForeColor = Color.White;
                }
            }
        }

        private void PrintReport()
        {
            try
            {
                if (dgvReport.Rows.Count == 0)
                {
                    UiMessages.ShowWarning("No Data", "Please load data first");
                    return;
                }

                DGVPrinter printer = new DGVPrinter();
                printer.Title = "Trial Balance Report";
                printer.SubTitle = string.Format("From {0:dd-MM-yyyy} To {1:dd-MM-yyyy}",
                    dtpFromDate.Value, dtpToDate.Value);
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.HeaderCellAlignment = StringAlignment.Center;
                printer.CellAlignment = StringAlignment.Near;

                // Print settings for compact single-page layout
                printer.RowHeight = DGVPrinter.RowHeightSetting.DataHeight;
                printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.DataWidth;

                printer.PrintMargins = new System.Drawing.Printing.Margins(20, 0, 20, 0);
                printer.PageSettings.Landscape = false;

                printer.Footer = "Generated by Kasbook - " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                printer.FooterSpacing = 10;

                // Set specific column widths for consistency
                printer.ColumnWidths.Clear();
                printer.ColumnWidths.Add("AccountName", 280);
                printer.ColumnWidths.Add("Debit", 100);
                printer.ColumnWidths.Add("Credit", 100);
                printer.ColumnWidths.Add("Balance", 100);

                printer.PrintPreviewDataGridView(dgvReport);

                // Log action
                POS.DLL.Log.LogAction("Print Trial Balance Report",
                    string.Format("From: {0:yyyy-MM-dd}, To: {1:yyyy-MM-dd}", dtpFromDate.Value, dtpToDate.Value),
                    UsersModal.logged_in_userid,
                    UsersModal.logged_in_branch_id);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Print Error", ex.Message);
            }
        }

        private void ExportReport()
        {
            try
            {
                if (dgvReport.Rows.Count == 0)
                {
                    UiMessages.ShowWarning("No Data", "Please load data first");
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|Excel files (*.xlsx)|*.xlsx",
                    FileName = $"TrialBalance_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportToCsv(sfd.FileName);
                    UiMessages.ShowInfo("Success", "Data exported successfully");

                    // Log action
                    POS.DLL.Log.LogAction("Export Trial Balance Report",
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
                    if (dgvReport.Columns[i].Visible)
                    {
                        writer.Write(dgvReport.Columns[i].HeaderText);
                        if (i < dgvReport.Columns.Count - 1) writer.Write(",");
                    }
                }
                writer.WriteLine();

                // Write data
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    for (int i = 0; i < dgvReport.Columns.Count; i++)
                    {
                        if (dgvReport.Columns[i].Visible)
                        {
                            string value = row.Cells[i].Value?.ToString() ?? "";
                            value = value.Replace("\"", "\"\"");
                            if (value.Contains(",") || value.Contains("\""))
                                value = "\"" + value + "\"";

                            writer.Write(value);
                            if (i < dgvReport.Columns.Count - 1) writer.Write(",");
                        }
                    }
                    writer.WriteLine();
                }
            }
        }
    }
}
