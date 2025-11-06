using POS.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Reports.Dashboard
{
    public partial class frm_branchWiseSummary : Form
    {
        public frm_branchWiseSummary()
        {
            InitializeComponent();
        }

        private async void frm_branchWiseSummary_Load(object sender, EventArgs e)
        {
            EnableDoubleBuffer(dataGridViewBranchSummary);
            ApplyGridTheme();
            await LoadBranchSummaryAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadBranchSummaryAsync();
        }

        private async Task LoadBranchSummaryAsync()
        {
            try
            {
                UseWaitCursor = true;
                btnRefresh.Enabled = false;
                lblStatus.Text = "Loading...";

                var dt = await Task.Run(() =>
                {
                    var bll = new SalesReportBLL();
                    return bll.GetBranchSummary(); // calls usp_Report_BranchSummary
                });

                if (dt == null || dt.Rows.Count == 0)
                {
                    dataGridViewBranchSummary.DataSource = null;
                    SetKpiValues(0m, 0m, 0m, 0m);
                    lblStatus.Text = "No data";
                    return;
                }

                // Compute totals
                decimal totalSales = SumColumn(dt, "TotalSales");
                decimal totalSalesTax = SumColumn(dt, "TotalSalesTax");
                decimal totalPurchases = SumColumn(dt, "TotalPurchases");
                decimal totalPurchasesTax = SumColumn(dt, "TotalPurchasesTax");
                decimal netIncome = totalSales - totalPurchases;

                // Append a grand total row
                var totalRow = dt.NewRow();
                if (dt.Columns.Contains("BranchName")) totalRow["BranchName"] = "Grand Total";
                if (dt.Columns.Contains("TotalSales")) totalRow["TotalSales"] = totalSales;
                if (dt.Columns.Contains("TotalSalesTax")) totalRow["TotalSalesTax"] = totalSalesTax;
                if (dt.Columns.Contains("TotalPurchases")) totalRow["TotalPurchases"] = totalPurchases;
                if (dt.Columns.Contains("TotalPurchasesTax")) totalRow["TotalPurchasesTax"] = totalPurchasesTax;
                dt.Rows.Add(totalRow);

                SuspendLayout();
                dataGridViewBranchSummary.DataSource = null;
                dataGridViewBranchSummary.AutoGenerateColumns = true;
                dataGridViewBranchSummary.DataSource = dt;

                // Column formatting
                TryFormatColumn("TotalSales", "C2", DataGridViewContentAlignment.MiddleRight);
                TryFormatColumn("TotalSalesTax", "C2", DataGridViewContentAlignment.MiddleRight);
                TryFormatColumn("TotalPurchases", "C2", DataGridViewContentAlignment.MiddleRight);
                TryFormatColumn("TotalPurchasesTax", "C2", DataGridViewContentAlignment.MiddleRight);

                StyleGrandTotalRow();

                // KPI values (uses the original label names for compatibility)
                lbl_total_sales.Text = totalSales.ToString("C2");
                lbl_total_purchases.Text = totalPurchases.ToString("C2");
                lbl_sales_tax.Text = totalSalesTax.ToString("C2");
                lbl_purchase_tax.Text = totalPurchasesTax.ToString("C2");
                lbl_net_income.Text = netIncome.ToString("C2");

                lblStatus.Text = $"Branches: {dt.Rows.Count - 1}  |  Total Sales: {totalSales:C2}  |  Net Income: {netIncome:C2}";
                ResumeLayout(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error loading branch summary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Load failed";
            }
            finally
            {
                btnRefresh.Enabled = true;
                UseWaitCursor = false;
            }
        }

        private static decimal SumColumn(DataTable dt, string name)
        {
            if (!dt.Columns.Contains(name)) return 0m;
            return dt.AsEnumerable().Select(r => r.Field<decimal?>(name) ?? 0m).Sum();
        }

        private void SetKpiValues(decimal sales, decimal purchases, decimal salesTax, decimal purchasesTax)
        {
            lbl_total_sales.Text = sales.ToString("C2");
            lbl_total_purchases.Text = purchases.ToString("C2");
            lbl_sales_tax.Text = salesTax.ToString("C2");
            lbl_purchase_tax.Text = purchasesTax.ToString("C2");
            lbl_net_income.Text = (sales - purchases).ToString("C2");
        }

        private void TryFormatColumn(string name, string format, DataGridViewContentAlignment align)
        {
            if (!dataGridViewBranchSummary.Columns.Contains(name)) return;
            var col = dataGridViewBranchSummary.Columns[name];
            col.DefaultCellStyle.Format = format;
            col.DefaultCellStyle.Alignment = align;
            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void StyleGrandTotalRow()
        {
            if (dataGridViewBranchSummary.Rows.Count == 0) return;

            DataGridViewRow totalRow = null;
            if (dataGridViewBranchSummary.Columns.Contains("BranchName"))
            {
                totalRow = dataGridViewBranchSummary.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(r => string.Equals(Convert.ToString(r.Cells["BranchName"].Value), "Grand Total", StringComparison.OrdinalIgnoreCase));
            }
            totalRow = totalRow ?? dataGridViewBranchSummary.Rows[dataGridViewBranchSummary.Rows.Count - 1];

            var style = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(240, 240, 240),
                Font = new Font(dataGridViewBranchSummary.Font, FontStyle.Bold)
            };
            totalRow.DefaultCellStyle = style;
        }

        private void ApplyGridTheme()
        {
            var grid = dataGridViewBranchSummary;
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = Color.White;
            grid.EnableHeadersVisualStyles = false;

            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font(grid.Font, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
            grid.RowTemplate.Height = 28;

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.RowHeadersVisible = false;
        }

        private static void EnableDoubleBuffer(DataGridView grid)
        {
            try
            {
                // Reduce flicker in WinForms grid
                typeof(DataGridView).InvokeMember("DoubleBuffered",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                    null, grid, new object[] { true });
            }
            catch { /* ignore if not supported */ }
        }
    }
}
