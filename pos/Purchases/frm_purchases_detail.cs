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
    public partial class frm_purchases_detail : Form
    {

        public frm_purchases_detail()
        {
            InitializeComponent();
        }


        public void frm_purchases_detail_Load(object sender, EventArgs e)
        {
            //load_purchases_detail_grid(sale_id);
        }

        public void load_purchases_detail_grid(string invoice_no)
        {
            try
            {
                double _total_qty = 0;
                double _total_cost = 0;
                double _total_vat = 0;
                double _total_discount = 0;
                double _grand_total = 0;

                grid_purchases_detail.DataSource = null;

                //bind data in data grid view  
                PurchasesBLL objpurchasesBLL = new PurchasesBLL();
                grid_purchases_detail.AutoGenerateColumns = false;

                //String keyword = "id,name,date_created";
               // String table = "pos_purchases_detail";
                DataTable dt = objpurchasesBLL.GetAllPurchasesItems(invoice_no);
                
                foreach (DataRow dr in dt.Rows)
                {
                    
                    string[] row00 = {
                        dr["id"].ToString(),
                        dr["invoice_no"].ToString(),
                        dr["item_code"].ToString(),
                        dr["product_name"].ToString(),
                        dr["loc_code"].ToString(),
                        Math.Round(Convert.ToDouble(dr["quantity"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["cost_price"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["discount_value"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["vat"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["net_total"]),2).ToString()
                    };

                    _total_qty += Convert.ToDouble(dr["quantity"].ToString());
                    _total_cost += Convert.ToDouble(dr["cost_price"].ToString());
                    _total_discount += Convert.ToDouble(dr["discount_value"].ToString());
                    _total_vat += Convert.ToDouble(dr["vat"].ToString());
                    _grand_total += Convert.ToDouble(dr["net_total"].ToString());
                    
                    grid_purchases_detail.Rows.Add(row00);

                }
                string[] row12 = { "","","","","Total", _total_qty.ToString(), _total_cost.ToString(), _total_discount.ToString(), _total_vat.ToString(), _grand_total.ToString() };
                grid_purchases_detail.Rows.Add(row12);
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
            DataGridViewRow lastRow = grid_purchases_detail.Rows[grid_purchases_detail.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_purchases_detail.Font, FontStyle.Bold);

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

        private void frm_purchases_detail_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void txt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
