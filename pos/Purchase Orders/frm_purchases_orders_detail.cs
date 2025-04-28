using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using POS.BLL;

namespace pos
{
    public partial class frm_purchases_orders_detail : Form
    {

        public frm_purchases_orders_detail()
        {
            InitializeComponent();
        }


        public void frm_purchases_orders_detail_Load(object sender, EventArgs e)
        {
            //load_purchases_orders_detail_grid(sale_id);
        }

        public void load_purchases_orders_detail_grid(int sale_id)
        {
            try
            {
                grid_purchases_orders_detail.DataSource = null;

                //bind data in data grid view  
                Purchases_orderBLL objPurchases_orderBLL = new Purchases_orderBLL();
                grid_purchases_orders_detail.AutoGenerateColumns = false;

                //String keyword = "id,name,date_created";
                // String table = "pos_purchases_orders_detail";
                var dt = objPurchases_orderBLL.GetAllPurchases_orderItems(sale_id); // Assuming GetAllPurchasesOrders returns a DataTable

                if (dt != null && dt.Rows.Count > 0)
                {
                    // Add grand total row to the DataTable itself
                    AddGrandTotalRowToDataTable(dt);
                    grid_purchases_orders_detail.DataSource = dt;
                    MakeLastRowBold();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
        private void AddGrandTotalRowToDataTable(DataTable dt)
        {
            decimal totalQty = 0;
            decimal totalCostPrice = 0;
            decimal totalTax = 0;
            decimal grandTotal = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalQty += Convert.ToDecimal(row["quantity"] ?? 0);
                totalCostPrice += Convert.ToDecimal(row["cost_price"] ?? 0);
                totalTax += Convert.ToDecimal(row["tax"] ?? 0);

                decimal lineTotal = (Convert.ToDecimal(row["cost_price"] ?? 0) +
                                    Convert.ToDecimal(row["tax"] ?? 0)) *
                                    Convert.ToDecimal(row["quantity"] ?? 0); 
                grandTotal += lineTotal;
            }

            // Create a new row
            DataRow totalRow = dt.NewRow();
            totalRow["product_name"] = "Grand Total:";
            totalRow["quantity"] = totalQty;
            totalRow["cost_price"] = totalCostPrice;
            totalRow["tax"] = totalTax;
            totalRow["total"] = grandTotal;

            dt.Rows.Add(totalRow);
        }
        private void MakeLastRowBold()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_purchases_orders_detail.Rows[grid_purchases_orders_detail.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_purchases_orders_detail.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }
        }


        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                txt_close.PerformClick();
            }
        }

        private void frm_purchases_orders_detail_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void txt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
