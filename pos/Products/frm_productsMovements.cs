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

        string _product_code;
        
        public frm_productsMovements(string product_code)
        {
            InitializeComponent();
            _product_code = product_code;
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

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_search_products.AutoGenerateColumns = false;

                String keyword = "TOP 1000 I.id,P.name AS product_name,I.item_code,I.qty,I.unit_price,I.cost_price,I.invoice_no,I.description,trans_date,C.first_name AS customer, S.first_name AS supplier";
                String table = "pos_inventory I LEFT JOIN pos_products P ON P.code=I.item_code LEFT JOIN pos_customers C ON C.id=I.customer_id LEFT JOIN pos_suppliers S ON S.id=I.supplier_id "+
                    " WHERE I.item_code = '" + _product_code + "' AND I.branch_id = "+ UsersModal.logged_in_branch_id + " ORDER BY I.id DESC";
                //grid_search_products.DataSource = objBLL.GetRecord(keyword, table);

                DataTable product_dt = objBLL.GetRecord(keyword, table);
                if (product_dt.Rows.Count > 0)
                {
                    int RowIndex = 0;
                    foreach (DataRow myProductView in product_dt.Rows)
                    {
                        int id = Convert.ToInt32(myProductView["id"]);
                        string invoice_no = myProductView["invoice_no"].ToString();
                        string name = myProductView["product_name"].ToString();
                        string qty = myProductView["qty"].ToString();
                        double cost_price = Convert.ToDouble(myProductView["cost_price"]);
                        double unit_price = Convert.ToDouble(myProductView["unit_price"]);
                        string description = myProductView["description"].ToString();
                        string supplier = myProductView["supplier"].ToString();
                        string customer = myProductView["customer"].ToString();
                        string date = myProductView["trans_date"].ToString();
                       
                        string[] row0 = { id.ToString(), invoice_no, name, qty,cost_price.ToString(), unit_price.ToString(),
                                          description, supplier, customer,date};

                        grid_search_products.Rows.Add(row0);

                        if (description == "Sale")
                        {
                            grid_search_products.Rows[RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                        }
                        if(description == "Purchase")
                        {
                            grid_search_products.Rows[RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;

                        }
                        if (description == "Adjustment")
                        {
                            grid_search_products.Rows[RowIndex].DefaultCellStyle.BackColor = Color.Yellow;

                        }

                        
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
