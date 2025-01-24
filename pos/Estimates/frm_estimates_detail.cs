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
    public partial class frm_estimates_detail : Form
    {
        int _sale_id;
        string _invoice_no;

        public frm_estimates_detail(int sale_id,string invoice_no)
        {
            InitializeComponent();
            _sale_id = sale_id;
            _invoice_no = invoice_no;
        }


        public void frm_estimates_detail_Load(object sender, EventArgs e)
        {
            //load_estimates_detail_grid(sale_id);
             try
            {
                double _total_qty = 0;
                double _total_cost = 0;
                double _total_vat = 0;
                double _total_discount = 0;
                double _grand_total = 0;

                grid_estimates_detail.DataSource = null;
                grid_estimates_detail.AutoGenerateColumns = false;
                grid_estimates_detail.DataSource = load_estimates_detail_grid();

                EstimatesBLL objestimatesBLL = new EstimatesBLL();
                DataTable dt = objestimatesBLL.GetAllEstimatesItems(_sale_id);

                foreach (DataRow dr in dt.Rows)
                {

                    string[] row00 = {
                        dr["id"].ToString(),
                        dr["invoice_no"].ToString(),
                        dr["item_code"].ToString(),
                        dr["product_name"].ToString(),
                        dr["loc_code"].ToString(),
                        Math.Round(Convert.ToDouble(dr["quantity_sold"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["unit_price"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["discount_value"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["vat"]),2).ToString(),
                        Math.Round(Convert.ToDouble(dr["net_total"]),2).ToString()

                    };
                    _total_qty += Convert.ToDouble(dr["quantity_sold"].ToString());
                    _total_cost += Convert.ToDouble(dr["unit_price"].ToString());
                    _total_discount += Convert.ToDouble(dr["discount_value"].ToString());
                    _total_vat += Convert.ToDouble(dr["vat"].ToString());
                    _grand_total += Convert.ToDouble(dr["net_total"].ToString());

                    grid_estimates_detail.Rows.Add(row00);

                }
                string[] row12 = { "", "", "", "", "Total", _total_qty.ToString(), _total_cost.ToString(), _total_discount.ToString(), _total_vat.ToString(), _grand_total.ToString() };
                grid_estimates_detail.Rows.Add(row12);

                CustomizeDataGridView();
            }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

             }
        }

        public DataTable load_estimates_detail_grid()
        {
              
            //bind data in data grid view  
            EstimatesBLL objestimatesBLL = new EstimatesBLL();
            DataTable dt = objestimatesBLL.GetAllEstimatesItems(_sale_id);
            return dt;
            
        }

       
        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                txt_close.PerformClick();
            }
        }

        private void frm_estimates_detail_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void txt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_estimates_detail.Rows[grid_estimates_detail.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_estimates_detail.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }

    }
}
