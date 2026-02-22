using DGVPrinterHelper;
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
    public partial class frm_profitandloss_report : Form
    {
        public frm_profitandloss_report()
        {
            InitializeComponent();
           
        }

        private void frm_profitandloss_report_Load(object sender, EventArgs e)
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
            AppTheme.ApplyListFormStyleLightHeader(panel1, null, panel2, grid_profitandloss_report);
        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime from_date = txt_from_date.Value.Date;
                DateTime to_date = txt_to_date.Value.Date;
                int branchID = UsersModal.logged_in_branch_id;

                //Load_profitandloss_report(from_date, to_date);
                LoadProfitAndLoss(branchID, from_date, to_date);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadProfitAndLoss(int branchID, DateTime startDate, DateTime endDate)
        {
            DataTable dataTable = AccountReportDLL.ProfitAndLossReport(branchID, startDate, endDate);

            // Use LINQ to safely handle null sums
            decimal totalRevenue = dataTable.AsEnumerable()
                .Where(row => row.Field<string>("Category") == "Revenue" && row.Field<string>("AccountName").StartsWith("Total"))
                .Sum(row => row.Field<decimal?>("Amount") ?? 0);

            decimal totalCOGS = dataTable.AsEnumerable()
                .Where(row => row.Field<string>("Category") == "COGS" && row.Field<string>("AccountName").StartsWith("Total"))
                .Sum(row => row.Field<decimal?>("Amount") ?? 0);

            decimal totalExpenses = dataTable.AsEnumerable()
                .Where(row => row.Field<string>("Category") == "Expenses" && row.Field<string>("AccountName").StartsWith("Total"))
                .Sum(row => row.Field<decimal?>("Amount") ?? 0);

            decimal grossProfit = totalRevenue - totalCOGS;
            decimal netProfit = grossProfit - totalExpenses;

            // Add Gross Profit
            DataRow grossProfitRow = dataTable.NewRow();
            grossProfitRow["Category"] = "Summary";
            grossProfitRow["AccountName"] = "Gross Profit";
            grossProfitRow["Amount"] = grossProfit;
            dataTable.Rows.Add(grossProfitRow);

            // Add Net Profit
            DataRow netProfitRow = dataTable.NewRow();
            netProfitRow["Category"] = "Summary";
            netProfitRow["AccountName"] = "Net Profit";
            netProfitRow["Amount"] = netProfit;
            dataTable.Rows.Add(netProfitRow);


            // Bind to DataGridView
            grid_profitandloss_report.AutoGenerateColumns = false;
            grid_profitandloss_report.DataSource = dataTable;
            CustomizeDataGridView();
        }

        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_profitandloss_report.Rows[grid_profitandloss_report.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_profitandloss_report.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }

        private void frm_profitandloss_report_KeyDown(object sender, KeyEventArgs e)
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

        

        private void Load_profitandloss_report(DateTime from_date, DateTime to_date)
        {
            try
            {
                grid_profitandloss_report.DataSource = null;
                grid_profitandloss_report.Rows.Clear();
                grid_profitandloss_report.AutoGenerateColumns = false;

                AccountsBLL objprofitandlossBLL = new AccountsBLL();

                ///////////////////////Income start
                DataTable profitandlosss_dt = new DataTable();
                double _dr_total_income = 0;
                double _cr_total_income = 0;
                double _balance_total_income = 0;

                DataTable dt = objprofitandlossBLL.GetGroupsByAccountType(3); //3 is id of Revenue account type
                foreach (DataRow dr in dt.Rows)
                {
                    profitandlosss_dt = objprofitandlossBLL.GroupReport(from_date, to_date,int.Parse(dr["id"].ToString()));
                    string[] row00 = { dr["name"].ToString(), "", "", "" };
                    grid_profitandloss_report.Rows.Add(row00);

                    foreach (DataRow dr_2 in profitandlosss_dt.Rows)
                    {
                        //MessageBox.Show(dr_2["account_name"].ToString());

                        var account_name = dr_2["account_name"].ToString();
                        var debit = dr_2["debit"].ToString();
                        var credit = dr_2["credit"].ToString();
                        var balance = Math.Abs(Convert.ToDecimal(dr_2["balance"].ToString())); //remove negative sign
                        
                        string[] row0 = { account_name, "",  balance.ToString(),"" };
                        grid_profitandloss_report.Rows.Add(row0);

                        _dr_total_income += Convert.ToDouble(dr_2["debit"].ToString());
                        _cr_total_income += Convert.ToDouble(dr_2["credit"].ToString());

                    }
                    
                }

                DataRow newRow = profitandlosss_dt.NewRow();
                _balance_total_income = (_cr_total_income-_dr_total_income);
                profitandlosss_dt.Rows.InsertAt(newRow, profitandlosss_dt.Rows.Count+1);

                string[] row1 = { "Total", "", _balance_total_income.ToString(), "" };
                grid_profitandloss_report.Rows.Add(row1);
                grid_profitandloss_report.Rows.Add();
                ////////////////Income end here


                ///////////////////////COGS start
                DataTable cogs_dt = new DataTable();
                double _dr_total_cogs = 0;
                double _cr_total_cogs = 0;
                double _balance_total_cogs = 0;

                DataTable dt_main = objprofitandlossBLL.GetGroupsByAccountType(6); //6 is id of Cost of Goods Sold (COGS) account type
                foreach (DataRow dr in dt_main.Rows)
                {
                    cogs_dt = objprofitandlossBLL.GroupReport(from_date, to_date, int.Parse(dr["id"].ToString()));
                    string[] row00 = { dr["name"].ToString(), "", "", "" };
                    grid_profitandloss_report.Rows.Add(row00);

                    foreach (DataRow dr_cogs in cogs_dt.Rows)
                    {
                        //MessageBox.Show(dr_2["account_name"].ToString());

                        var account_name = dr_cogs["account_name"].ToString();
                        var debit = dr_cogs["debit"].ToString();
                        var credit = dr_cogs["credit"].ToString();
                        var balance = Convert.ToDecimal(dr_cogs["balance"].ToString()); //remove negative sign

                        string[] row0 = { account_name, "", balance.ToString(), "" };
                        grid_profitandloss_report.Rows.Add(row0);

                        _dr_total_cogs += Convert.ToDouble(dr_cogs["debit"].ToString());
                        _cr_total_cogs += Convert.ToDouble(dr_cogs["credit"].ToString());

                    }

                }

                DataRow newRow11 = cogs_dt.NewRow();
                _balance_total_cogs = (_dr_total_cogs - _cr_total_cogs);
                cogs_dt.Rows.InsertAt(newRow11, cogs_dt.Rows.Count + 1);

                string[] row12 = { "Total", "", _balance_total_cogs.ToString(), "" };
                grid_profitandloss_report.Rows.Add(row12);
                grid_profitandloss_report.Rows.Add();
                ////////////////COGS end here

                double gross_total = _balance_total_income - _balance_total_cogs;

                string[] gross_total_row = { "Gross Total", "", "", gross_total.ToString() };
                grid_profitandloss_report.Rows.Add(gross_total_row);
                
                ///////////// Expense
                ////////////////////
                DataTable dt_expense = objprofitandlossBLL.GetGroupsByAccountType(4); //3 is id of expense account type
                DataTable expense_dt = new DataTable();
                double _dr_total_expense = 0;
                double _cr_total_expense = 0;
                double _balance_total_expense = 0;

                foreach (DataRow dr_exp in dt_expense.Rows)
                {
                    string[] row0 = { dr_exp["name"].ToString(), "", "", "" };
                    grid_profitandloss_report.Rows.Add(row0);
                    
                    expense_dt = objprofitandlossBLL.GroupReport(from_date, to_date, int.Parse(dr_exp["id"].ToString()));
                    
                    foreach (DataRow dr_2 in expense_dt.Rows)
                    {
                        
                        var account_name = dr_2["account_name"].ToString();
                        var debit = dr_2["debit"].ToString();
                        var credit = dr_2["credit"].ToString();
                        var balance = dr_2["balance"].ToString();

                        string[] expense_row0 = { account_name, "", balance, "" };
                        grid_profitandloss_report.Rows.Add(expense_row0);

                        _dr_total_expense += Convert.ToDouble(dr_2["debit"].ToString());
                        _cr_total_expense += Convert.ToDouble(dr_2["credit"].ToString());
                    }

                }

                DataRow newRow_2 = expense_dt.NewRow();
                _balance_total_expense = (_dr_total_expense - _cr_total_expense);
                expense_dt.Rows.InsertAt(newRow_2, expense_dt.Rows.Count + 1);

                string[] expense_row1 = { "Total", "", _balance_total_expense.ToString(), "" };
                grid_profitandloss_report.Rows.Add(expense_row1);

                double net_total = gross_total - _balance_total_expense;

                string[] net_total_row = { "Net Total", "", "", net_total.ToString() };
                grid_profitandloss_report.Rows.Add(net_total_row);

                CustomizeDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Profit & Loss";
            printer.SubTitle = string.Format("{0} To {1}", txt_from_date.Value.Date.ToShortDateString(), txt_to_date.Value.Date.ToShortDateString());
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = false;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "kasbook app";
            printer.PorportionalColumns = false; printer.FooterSpacing = 15;
            printer.PrintPreviewDataGridView(grid_profitandloss_report);
            //printer.PrintDataGridView(grid_sales_report);
        }

        private void grid_profitandloss_report_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == grid_profitandloss_report.Columns["Category"].Index)
            {
                if (e.RowIndex > 0 &&
                    grid_profitandloss_report.Rows[e.RowIndex - 1].Cells["Category"].Value.ToString() == e.Value.ToString())
                {
                    e.Value = ""; // Avoid repeating category names
                }
            }

            // Highlight Total Rows
            //if (e.ColumnIndex == grid_profitandloss_report.Columns["AccountName"].Index)
            //{
            //    if (grid_profitandloss_report.Rows[e.RowIndex].Cells["AccountName"].Value.ToString().StartsWith("Total"))
            //    {
            //        e.CellStyle.Font = new Font(grid_profitandloss_report.Font, FontStyle.Bold);
            //    }
            //}
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
    }
}
