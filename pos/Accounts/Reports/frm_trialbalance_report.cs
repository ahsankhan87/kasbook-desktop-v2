using DGVPrinterHelper;
using pos.Accounts.Reports;
using POS.BLL;
using POS.Core;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pos.UI;

namespace pos
{
    public partial class frm_trialbalance_report : Form
    {
        public frm_trialbalance_report()
        {
            InitializeComponent();
           
        }

        private void frm_trialbalance_report_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            CmbCondition.Items.AddRange(new string[]
            {
                "Custom", "Today", "Yesterday", "This Week", "Last Week",
                "This Month", "Last Month", "This Quarter", "Last Quarter",
                "This Year", "Last Year", "Year to Date (YTD)", "Last 7 Days",
                "Last 30 Days", "Last 90 Days", "Last 6 Months",
                "Previous Fiscal Year", "Next Fiscal Year"
            });
            CmbCondition.SelectedIndex = 0;
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(panel1, null, panel2, grid_trialbalance_report, AccountID);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            DateTime from_date = txt_from_date.Value.Date;
            DateTime to_date = txt_to_date.Value.Date;
            int branchID = UsersModal.logged_in_branch_id;

            //Load_trialbalance_report(from_date, to_date);
            LoadTrialBalanceReport(branchID, from_date, to_date);
        }
        private void LoadTrialBalanceReport(int branchID, DateTime from_date, DateTime to_date)
        {
            try
            {
                DataTable dataTable = AccountReportDLL.TrialBalanceReport(branchID, from_date, to_date);

                // Add a summary row
                AddSummaryRow(dataTable);

                // Bind to DataGridView
                grid_trialbalance_report.AutoGenerateColumns = false;
                grid_trialbalance_report.DataSource = dataTable;

                CustomizeDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
}
        private void AddSummaryRow(DataTable dataTable)
        {
            decimal totalDebit = dataTable.AsEnumerable().Sum(row => row.Field<decimal?>("TotalDebit") ?? 0);
            decimal totalCredit = dataTable.AsEnumerable().Sum(row => row.Field<decimal?>("TotalCredit") ?? 0);
            decimal closingBalance = dataTable.AsEnumerable().Sum(row => row.Field<decimal?>("ClosingBalance") ?? 0);

            DataRow summaryRow = dataTable.NewRow();
            summaryRow["AccountName"] = "TOTALS";
            summaryRow["TotalDebit"] = totalDebit;
            summaryRow["TotalCredit"] = totalCredit;
            summaryRow["ClosingBalance"] = closingBalance;
            dataTable.Rows.Add(summaryRow);

            // Style the summary row in DataGridView
            foreach (DataGridViewRow row in grid_trialbalance_report.Rows)
            {
                if (row.Cells["AccountName"].Value?.ToString() == "TOTALS")
                {
                    row.DefaultCellStyle.Font = new Font(grid_trialbalance_report.Font, FontStyle.Bold);
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
        }
        
        private void Load_trialbalance_report(DateTime from_date, DateTime to_date)
        {
            try
            {
                grid_trialbalance_report.DataSource = null;
                grid_trialbalance_report.AutoGenerateColumns = false;

                AccountsBLL objtrialbalanceBLL = new AccountsBLL();

                DataTable trialbalances_dt = new DataTable();
                trialbalances_dt = objtrialbalanceBLL.TrialBalanceReport(from_date, to_date);

                double _dr_total = 0;
                double _cr_total = 0;
                //double _balance_total = 0;

                foreach (DataRow dr in trialbalances_dt.Rows)
                {
                    _dr_total += Convert.ToDouble(dr["debit"].ToString());
                    _cr_total += Convert.ToDouble(dr["credit"].ToString());
                }

                DataRow newRow = trialbalances_dt.NewRow();
                newRow[0] = "Total";
                newRow[1] = _dr_total;
                newRow[2] = _cr_total;
                newRow[3] = (_dr_total-_cr_total);
                trialbalances_dt.Rows.InsertAt(newRow, trialbalances_dt.Rows.Count);

                grid_trialbalance_report.DataSource = trialbalances_dt;
                CustomizeDataGridView();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_trialbalance_report.Rows[grid_trialbalance_report.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_trialbalance_report.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }

        private void frm_trialbalance_report_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                if (e.KeyData == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }
                if (e.Control && e.KeyCode == Keys.P)
                {
                    btn_print.PerformClick();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Trial Balance";
            printer.SubTitle = string.Format("{0} To {1}", txt_from_date.Value.Date.ToShortDateString(), txt_to_date.Value.Date.ToShortDateString());
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            //printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.CellWidth;
            printer.RowHeight = DGVPrinter.RowHeightSetting.CellHeight;
            printer.ColumnWidth = DGVPrinter.ColumnWidthSetting.CellWidth;
            //printer.EmbeddedPrint(RequestList, e.Graphics, new Rectangle(180, 480, 300, 120));

            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "kasbook app";
            printer.FooterSpacing = 15;
            printer.PorportionalColumns = false;
            printer.PrintPreviewDataGridView(grid_trialbalance_report);
            //printer.PrintDataGridView(grid_sales_report);
        }

        private void CmbCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime startDate = today;
            DateTime endDate = today;

            switch (CmbCondition.SelectedItem.ToString())
            {
                case "Custom":
                    return;

                case "Today":
                    startDate = endDate = today;
                    break;

                case "Yesterday":
                    startDate = endDate = today.AddDays(-1);
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

                case "This Quarter":
                    startDate = new DateTime(today.Year, ((today.Month - 1) / 3) * 3 + 1, 1);
                    endDate = startDate.AddMonths(3).AddDays(-1);
                    break;

                case "Last Quarter":
                    startDate = new DateTime(today.Year, ((today.Month - 1) / 3) * 3 + 1, 1).AddMonths(-3);
                    endDate = startDate.AddMonths(3).AddDays(-1);
                    break;

                case "Year to Date (YTD)":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = today;
                    break;

                case "Last 7 Days":
                    startDate = today.AddDays(-6);
                    break;

                case "Last 30 Days":
                    startDate = today.AddDays(-29);
                    break;

                case "Last 90 Days":
                    startDate = today.AddDays(-89);
                    break;

                case "Last 6 Months":
                    startDate = today.AddMonths(-6);
                    break;

                case "This Year":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = new DateTime(today.Year, 12, 31);
                    break;

                case "Last Year":
                    startDate = new DateTime(today.Year - 1, 1, 1);
                    endDate = new DateTime(today.Year - 1, 12, 31);
                    break;

                case "Previous Fiscal Year":
                    startDate = new DateTime(today.Year - 1, 1, 1);
                    endDate = new DateTime(today.Year - 1, 12, 31);
                    break;

                case "Next Fiscal Year":
                    startDate = new DateTime(today.Year + 1, 1, 1);
                    endDate = new DateTime(today.Year + 1, 12, 31);
                    break;
            }

            txt_from_date.Value = startDate;
            txt_to_date.Value = endDate;
        }

        private void grid_trialbalance_report_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var AccountID = grid_trialbalance_report.CurrentRow.Cells["AccountID"].Value.ToString(); // retreive the current row
            
            LoadAccountDetail(Convert.ToInt32(AccountID), txt_from_date.Value.Date, txt_to_date.Value.Date);
        }

        private void LoadAccountDetail(int accountID, DateTime from_date, DateTime to_date)
        {
            FrmAccountDetail frmAccountDetail = new FrmAccountDetail(accountID, from_date, to_date);
            //frm_sales_detail_obj.load_sales_detail_grid();
            frmAccountDetail.ShowDialog();
        }
    }
}
