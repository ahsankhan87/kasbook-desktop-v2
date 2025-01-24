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
using POS.Core;

namespace pos
{
    public partial class frm_alt_products : Form
    {
        string global_product_code = "";
        int global_alt_id = 0;

        public frm_alt_products()
        {
            InitializeComponent();
        }

        public void frm_alt_products_Load(object sender, EventArgs e)
        {
            txt_source_code.Focus();
        }

        private void txt_source_code_KeyDown(object sender, KeyEventArgs e)
        {
            bool source_product = true;
            if (txt_source_code.Text != "" && e.KeyData == Keys.Enter)
            {
                frm_searchProducts search_product_obj = new frm_searchProducts(null, null, this, txt_source_code.Text, "", "", 0, false, source_product);
                search_product_obj.ShowDialog();

            }
        }

        public void fill_product_txtbox(string product_id, string code, string name,int alt_no)
        {
            txt_source_code.Text = name;
            global_product_code = code;
            global_alt_id = alt_no;
            txt_item_code.Text = code;

            if (txt_item_code.Text != "" && global_alt_id != 0)
            {

                Load_alternateProductsToGrid(global_alt_id);

            }
        }


        public void Load_alternateProductsToGrid(int alt_no)
        {
            grid_product_groups.Rows.Clear();
            grid_product_groups.Refresh();

            ProductGroupsBLL objBLL = new ProductGroupsBLL();
            DataTable product_dt = new DataTable();
            product_dt = objBLL.SearchAlternateProducts(alt_no);
            
            if (product_dt.Rows.Count > 0)
            {
                foreach (DataRow myProductView in product_dt.Rows)
                {

                    int id = Convert.ToInt32(myProductView["id"]);
                    string code = myProductView["code"].ToString();
                    string name = myProductView["name"].ToString();
                    string alternate_no = myProductView["alternate_no"].ToString();

                    string[] row0 = { id.ToString(), code, name, alternate_no };

                    grid_product_groups.Rows.Add(row0);

                }


            }
        }

        
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if(txt_item_code.Text != "")
                {
                    string result = "";
                    ProductGroupsModal info = new ProductGroupsModal();
                    ProductGroupsBLL objBLL = new ProductGroupsBLL();

                    for (int i = 0; i < grid_product_groups.Rows.Count; i++)
                    {
                        if (grid_product_groups.Rows[i].Cells["code"].Value != null)
                        {
                            info.alt_no = int.Parse(grid_product_groups.Rows[i].Cells["alternate_no"].Value.ToString());
                            info.product_id = grid_product_groups.Rows[i].Cells["code"].Value.ToString();
                            info.code = global_product_code;
                            result = objBLL.InsertProductAlternate(info); // 

                        }

                    }
                    if(result.ToString().Length > 0)
                    {
                        MessageBox.Show("Record Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        grid_product_groups.DataSource = null;
                        grid_product_groups.Rows.Clear();
                        txt_product_code.Text = "";
                        txt_source_code.Text ="";
                        txt_item_code.Text = "";
                        txt_source_code.Focus();
                    }
                    else
                    {
                        MessageBox.Show("Record not saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select group", "Group", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_product_code_KeyDown(object sender, KeyEventArgs e)
        {
            if(txt_item_code.Text != "" && e.KeyData == Keys.Enter)
            {
                frm_searchProducts search_product_obj = new frm_searchProducts(null, null,this, txt_product_code.Text, "", "", 0, false);
                search_product_obj.ShowDialog();

            }
        }

        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchProducts search_product_obj = new frm_searchProducts(null,null, this, txt_product_code.Text, "", "", 0, false);
            search_product_obj.ShowDialog();
            
            txt_source_code.Focus();

        }
        public void load_products(string product_id = "")
        {

            ProductBLL productsBLL_obj = new ProductBLL();
            DataTable product_dt = new DataTable();

            if (product_id != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByProductCode(product_id);
            }

            if (product_dt.Rows.Count > 0)
            {
                foreach (DataRow myProductView in product_dt.Rows)
                {

                    int id = Convert.ToInt32(myProductView["id"]);
                    string code = myProductView["code"].ToString();
                    string name = myProductView["name"].ToString();
                    int alt_no = global_alt_id; // (myProductView["alt_no"] != null ? Convert.ToInt32(myProductView["alt_no"]) : 0);

                    if (alt_no == 0)
                    {
                        ProductGroupsBLL objBLL = new ProductGroupsBLL();
                        int maxAltNo = objBLL.GetMaxAlternateNo();
                    
                        alt_no = maxAltNo;
                    }

                    string[] row0 = { id.ToString(), code, name, alt_no.ToString() };

                    grid_product_groups.Rows.Add(row0);

                }
                txt_product_code.Text = "";

            }
            else
            {
                MessageBox.Show("Record not found", "Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void frm_alt_products_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                btn_save.PerformClick();
            }
        }

        private void grid_product_groups_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int product_id = int.Parse(grid_product_groups.CurrentRow.Cells["id"].Value.ToString());
                            
            string name = grid_product_groups.Columns[e.ColumnIndex].Name;
            if (name == "btn_delete")
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    ProductGroupsBLL objBLL = new ProductGroupsBLL();
                    objBLL.DeleteAltNo(product_id);

                    MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Load_alternateProductsToGrid(global_alt_id);
                }
                else
                {
                    MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

                //load_Products_grid();

            }
        }

        private void txt_source_code_KeyUp(object sender, KeyEventArgs e)
        {
            if (txt_source_code.Text == "")
            {
                txt_item_code.Text = "";
            }

            
        }


    }
}
