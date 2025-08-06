using POS.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        private void frm_branchWiseSummary_Load(object sender, EventArgs e)
        {
            LoadBranchSummary();
        }
        private void LoadBranchSummary()
        {
            SalesReportBLL usersBLL_obj = new SalesReportBLL();

            DataTable dt = usersBLL_obj.GetBranchSummary();

            // Calculate totals
            decimal totalSales = 0;
            decimal totalPurchases = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalSales += Convert.ToDecimal(row["TotalSales"]);
                totalPurchases += Convert.ToDecimal(row["TotalPurchases"]);
                lbl_total_sales.Text = Convert.ToDecimal(row["totalSales"]).ToString();
                lbl_total_purchases.Text = Convert.ToDecimal(row["TotalPurchases"]).ToString();
            }

            // Add a new row for totals
            DataRow totalRow = dt.NewRow();
            totalRow["BranchName"] = "Grand Total";
            totalRow["TotalSales"] = totalSales;
            totalRow["TotalPurchases"] = totalPurchases;
            dt.Rows.Add(totalRow);

           

            dataGridViewBranchSummary.DataSource = dt;

            CustomizeDataGridView();

          
        }
        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = dataGridViewBranchSummary.Rows[dataGridViewBranchSummary.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(dataGridViewBranchSummary.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }
    }
    
}
