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
    public partial class frm_products : Form
    {
        public frm_products()
        {
            InitializeComponent();
        }

        
        public void frm_products_Load(object sender, EventArgs e)
        {
            load_Products_grid();
        }
        
        public void load_Products_grid()
        {
            try
            {
                grid_products.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_products.AutoGenerateColumns = false;

                String keyword = "TOP 500 id,code,name,item_type,barcode,qty,avg_cost,unit_price,location_code,description,date_created";
                String table = "pos_products ORDER BY id desc";
                grid_products.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addProduct frm_addProduct_obj = new frm_addProduct(this,0, "false");
            frm_addProduct_obj.WindowState = FormWindowState.Maximized;
            frm_addProduct_obj.ShowDialog();
            
        }
        
        private void btn_update_Click(object sender, EventArgs e)
        {
            string product_id = grid_products.CurrentRow.Cells["id"].Value.ToString();
            frm_addProduct frm_addProduct_obj = new frm_addProduct(this, int.Parse(product_id), "true");
            frm_addProduct_obj.WindowState = FormWindowState.Maximized;
            frm_addProduct_obj.ShowDialog();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_products.CurrentRow.Cells[0].Value.ToString();
            
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);  

            if (result == DialogResult.Yes)
            {
                ProductBLL objBLL = new ProductBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_Products_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_Products_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {

                if (txt_search.Text != "")
                {
                    //grid_products.DataSource = null;

                    //bind data in data grid view  
                    ProductBLL objBLL = new ProductBLL();
                    //grid_products.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_products.DataSource = objBLL.SearchRecord(condition);

                    txt_search.Text = "";
                
                }
                    
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 
            
        }

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                btn_search.PerformClick();
            }
        }

        private void frm_products_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.N)
            {
                btn_new.PerformClick();
                
            }

            if (e.Control == true && e.KeyCode == Keys.M)
            {
               btn_movements.PerformClick();

            }
        }

        private void btn_movements_Click(object sender, EventArgs e)
        {
            if (grid_products.RowCount > 0)
            {
                string product_code = grid_products.CurrentRow.Cells["code"].Value.ToString();
                frm_productsMovements frm_prod_move_obj = new frm_productsMovements(product_code);

                frm_prod_move_obj.load_Products_grid();
                frm_prod_move_obj.ShowDialog();
            }
        }

        private void grid_products_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string name = grid_products.Columns[e.ColumnIndex].Name;
            if(name == "edit")
            {
                string product_id = grid_products.CurrentRow.Cells["id"].Value.ToString();
                frm_addProduct frm_addProduct_obj = new frm_addProduct(this, int.Parse(product_id), "true");
                frm_addProduct_obj.WindowState = FormWindowState.Maximized; 
                frm_addProduct_obj.ShowDialog();
            
            }
            else if (name == "delete")
            {
                string id = grid_products.CurrentRow.Cells["id"].Value.ToString();

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    ProductBLL objBLL = new ProductBLL();
                    objBLL.Delete(int.Parse(id));

                    MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    load_Products_grid();
                }
                else
                {
                    MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

                load_Products_grid();
            }
        }

        private void btn_product_detail_Click(object sender, EventArgs e)
        {
            frm_product_full_detail frm_full_detail = new frm_product_full_detail();
            frm_full_detail.Show();
        }  
    }
}
