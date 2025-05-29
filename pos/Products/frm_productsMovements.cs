using POS.BLL;
using POS.Core;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_productsMovements : Form
    {

        string _item_number;
        
        public frm_productsMovements(string item_number)
        {
            InitializeComponent();
            _item_number = item_number;
        }
        
        private void frm_productsMovements_Load(object sender, EventArgs e)
        {
            load_Products_grid();
           
        }

        public void load_Products_grid()
        {
            try
            {
                load_product_movements();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                //load_product_movements();
                   
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 
        }
        private void load_product_movements()
        {
            try
            {
                grid_search_products.Rows.Clear();

                GeneralBLL objBLL = new GeneralBLL();
                grid_search_products.AutoGenerateColumns = false;

                string keyword = "I.id,P.name AS product_name,I.item_code,I.item_number,I.qty,I.unit_price,I.cost_price,I.invoice_no,I.description,trans_date,C.first_name AS customer,S.first_name AS supplier";
                string table = "pos_inventory I " +
                               "LEFT JOIN pos_products P ON P.code = I.item_code " +
                               "LEFT JOIN pos_customers C ON C.id = I.customer_id " +
                               "LEFT JOIN pos_suppliers S ON S.id = I.supplier_id " +
                               "WHERE I.item_number = '" + _item_number + "' AND I.branch_id = " + UsersModal.logged_in_branch_id + " " +
                               "ORDER BY I.id ASC";

                DataTable product_dt = objBLL.GetRecord(keyword, table);

                if (product_dt.Rows.Count > 0)
                {
                    // ✅ Add balance_qty column manually to avoid error
                    if (!product_dt.Columns.Contains("balance_qty"))
                        product_dt.Columns.Add("balance_qty", typeof(double));

                    // Calculate running balance
                    double balance_qty = 0;
                    foreach (DataRow row in product_dt.Rows)
                    {
                        balance_qty += Convert.ToDouble(row["qty"]);
                        row["balance_qty"] = balance_qty;
                    }

                    // Display in DESC order
                    int RowIndex = 0;
                    foreach (DataRow row in product_dt.Select("", "id DESC"))
                    {
                        int id = Convert.ToInt32(row["id"]);
                        string invoice_no = row["invoice_no"].ToString();
                        string name = row["product_name"].ToString();
                        string qty = row["qty"].ToString();
                        string balance = row["balance_qty"].ToString();
                        double cost_price = Convert.ToDouble(row["cost_price"]);
                        double unit_price = Convert.ToDouble(row["unit_price"]);
                        string description = row["description"].ToString();
                        string supplier = row["supplier"].ToString();
                        string customer = row["customer"].ToString();
                        string date = row["trans_date"].ToString();

                        string[] row0 = { id.ToString(), invoice_no, name, qty, balance, cost_price.ToString(), unit_price.ToString(),
                                  description, supplier, customer, date };

                        grid_search_products.Rows.Add(row0);

                        if (description == "Sale")
                            grid_search_products.Rows[RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                        else if (description == "Purchase")
                            grid_search_products.Rows[RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                        else if (description == "Adjustment")
                            grid_search_products.Rows[RowIndex].DefaultCellStyle.BackColor = Color.Yellow;

                        RowIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }



        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(grid_search_products.CurrentRow.Cells["invoice_no"].Value.ToString());
        }



    }
}
