using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using POS.DLL;
using System.Drawing;

namespace POS.Reports.Finance
{
    public partial class frm_salesExpensesReport : Form
    {
        
        public frm_salesExpensesReport()
        {
            InitializeComponent();
        }

        private void frm_salesExpensesReport_Load(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today.AddDays(-30); // Last 30 days
            dtpTo.Value = DateTime.Today;
            GenerateReport();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void GenerateReport()
        {
            try
            {
                DateTime startDate =  dtpFrom.Value.Date;
                DateTime endDate  = dtpTo.Value.Date.AddDays(1).AddSeconds(-1);

                SalesDLL salesDLL = new SalesDLL();
                DataTable dt = salesDLL.GenerateDetailedReport(startDate, endDate);
                dgvReport.DataSource = dt;
                FormatDataGridViewDetailed();
                
                // Calculate totals
                //CalculateTotals(dt);
                CalculateTotalsDetailed(dt);

                //using (SqlConnection conn = new SqlConnection(connectionString))
                //{
                //    conn.Open();

                //    // Combined query for sales and expenses
                //    string query = @"
                //        -- Sales Data
                //        SELECT 
                //            'Sale' as Type,
                //            s.sale_date as Date,
                //            'Sale: ' + s.customer_name as Description,
                //            s.total_amount as Amount,
                //            0 as ExpenseAmount,
                //            s.total_amount as SalesAmount
                //        FROM pos_sales s
                //        WHERE s.sale_date BETWEEN @FromDate AND @ToDate

                //        UNION ALL

                //        -- Expense Data
                //        SELECT 
                //            'Expense' as Type,
                //            p.payment_date as Date,
                //            'Expense: ' + p.description as Description,
                //            0 as Amount,
                //            p.amount as ExpenseAmount,
                //            0 as SalesAmount
                //        FROM acc_payment p
                //        WHERE p.payment_date BETWEEN @FromDate AND @ToDate

                //        ORDER BY Date DESC";

                //    using (SqlCommand cmd = new SqlCommand(query, conn))
                //    {
                //        cmd.Parameters.AddWithValue("@FromDate", dtpFrom.Value.Date);
                //        cmd.Parameters.AddWithValue("@ToDate", dtpTo.Value.Date.AddDays(1).AddSeconds(-1));

                //        DataTable dt = new DataTable();
                //        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                //        {
                //            da.Fill(dt);
                //        }

                //        dgvReport.DataSource = dt;

                //        // Calculate totals
                //        CalculateTotals(dt);
                //    }
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FormatDataGridView()
        {
            if (dgvReport.Columns.Count > 0)
            {
                dgvReport.Columns["SortOrder"].Visible = false;

                // Format headers
                dgvReport.Columns["RecordType"].HeaderText = "Type";
                dgvReport.Columns["TransactionDate"].HeaderText = "Date";
                dgvReport.Columns["SalesAmount"].HeaderText = "Sales Amount";
                dgvReport.Columns["ExpenseAmount"].HeaderText = "Expense Amount";

                // Format currency columns
                dgvReport.Columns["Amount"].DefaultCellStyle.Format = "C2";
                dgvReport.Columns["SalesAmount"].DefaultCellStyle.Format = "C2";
                dgvReport.Columns["ExpenseAmount"].DefaultCellStyle.Format = "C2";

                // Format date column
                dgvReport.Columns["TransactionDate"].DefaultCellStyle.Format = "MM/dd/yyyy HH:mm";
            }
        }

        private void FormatDataGridViewDetailed()
        {
            if (dgvReport.Columns.Count > 0)
            {
                dgvReport.Columns["SortOrder"].Visible = false;

                // Format headers
                dgvReport.Columns["Category"].HeaderText = "Category";
                dgvReport.Columns["TransactionDate"].HeaderText = "Date";
                dgvReport.Columns["Description"].HeaderText = "Description";
                dgvReport.Columns["SalesAmount"].HeaderText = "Sales Amount";
                dgvReport.Columns["ExpenseAmount"].HeaderText = "Expense Amount";

                // Format currency columns
                dgvReport.Columns["SalesAmount"].DefaultCellStyle.Format = "C2";
                dgvReport.Columns["ExpenseAmount"].DefaultCellStyle.Format = "C2";

                // Color coding for categories
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (row.Cells["Category"].Value?.ToString() == "SALES SUMMARY")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightBlue;
                        row.DefaultCellStyle.Font = new Font(dgvReport.Font, FontStyle.Bold);
                    }
                    else if (row.Cells["Category"].Value?.ToString() == "EXPENSES SUMMARY")
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                        row.DefaultCellStyle.Font = new Font(dgvReport.Font, FontStyle.Bold);
                    }
                }
            }
        }

        private void CalculateTotalsDetailed(DataTable data)
        {
            decimal totalSales = 0;
            decimal totalExpenses = 0;

            foreach (DataRow row in data.Rows)
            {
                string category = row["Category"].ToString();

                if (category == "Sale" || category == "SALES SUMMARY")
                {
                    totalSales += Convert.ToDecimal(row["SalesAmount"]);
                }
                else if (category == "Expense" || category == "EXPENSES SUMMARY")
                {
                    totalExpenses += Convert.ToDecimal(row["ExpenseAmount"]);
                }
            }

            decimal profit = totalSales - totalExpenses;

            txtTotalSales.Text = totalSales.ToString("C2");
            txtTotalExpenses.Text = totalExpenses.ToString("C2");
            txtProfit.Text = profit.ToString("C2");

            // Color code profit
            txtProfit.ForeColor = profit >= 0 ? Color.Green : Color.Red;
        }

        private void CalculateTotals(DataTable data)
        {
            decimal totalSales = 0;
            decimal totalExpenses = 0;

            foreach (DataRow row in data.Rows)
            {
                totalSales += Convert.ToDecimal(row["SalesAmount"]);
                totalExpenses += Convert.ToDecimal(row["ExpenseAmount"]);
            }

            decimal profit = totalSales - totalExpenses;

            txtTotalSales.Text = totalSales.ToString("C2");
            txtTotalExpenses.Text = totalExpenses.ToString("C2");
            txtProfit.Text = profit.ToString("C2");

            // Color code profit
            txtProfit.ForeColor = profit >= 0 ? System.Drawing.Color.Green : System.Drawing.Color.Red;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    // Implement printing logic here
                    MessageBox.Show("Print functionality would be implemented here",
                        "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
                saveFileDialog.Title = "Export Report";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportToCsv(saveFileDialog.FileName);
                    MessageBox.Show("Report exported successfully!", "Export",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCsv(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                // Write headers
                sw.WriteLine("Type,Date,Description,Sales Amount,Expense Amount");

                // Write data
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string type = row.Cells["Type"].Value?.ToString() ?? "";
                        string date = Convert.ToDateTime(row.Cells["Date"].Value).ToString("yyyy-MM-dd");
                        string description = row.Cells["Description"].Value?.ToString() ?? "";
                        string salesAmount = row.Cells["SalesAmount"].Value?.ToString() ?? "0";
                        string expenseAmount = row.Cells["ExpenseAmount"].Value?.ToString() ?? "0";

                        sw.WriteLine($"\"{type}\",\"{date}\",\"{description}\",{salesAmount},{expenseAmount}");
                    }
                }

                // Write summary
                sw.WriteLine();
                sw.WriteLine($"Total Sales,{txtTotalSales.Text}");
                sw.WriteLine($"Total Expenses,{txtTotalExpenses.Text}");
                sw.WriteLine($"Profit,{txtProfit.Text}");
            }
        }
    }
}