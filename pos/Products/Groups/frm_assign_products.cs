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
    public partial class frm_assign_products : Form
    {

        public frm_assign_products()
        {
            InitializeComponent();
        }


        public void frm_assign_products_Load(object sender, EventArgs e)
        {
            get_groups_dropdownlist();
        }

        public void load_ProductGroups_grid(string product_id, int RowIndex)
        {
            try
            {
                ProductBLL productsBLL_obj = new ProductBLL();
                DataTable product_dt = new DataTable();

                product_dt = productsBLL_obj.SearchRecordByProductID(product_id);
                if (product_dt.Rows.Count > 0)
                {
                    foreach (DataRow myProductView in product_dt.Rows)
                    {
                        
                        grid_product_groups.Rows[RowIndex].Cells["id"].Value = myProductView["id"].ToString();
                        grid_product_groups.Rows[RowIndex].Cells["code"].Value = myProductView["code"].ToString();
                        grid_product_groups.Rows[RowIndex].Cells["name"].Value = myProductView["name"].ToString();
                        //total_amount += sub_total; 
                        //total_tax += tax;

                    }


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }


        public void get_groups_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_product_groups";

            DataTable groups = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = groups.NewRow();
            emptyRow[0] = "";              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            groups.Rows.InsertAt(emptyRow, 0);

            //DataRow emptyRow1 = groups.NewRow();
            //emptyRow1[0] = "-1";              // Set Column Value
            //emptyRow1[1] = "ADD NEW";              // Set Column Value
            //groups.Rows.InsertAt(emptyRow1, 1);

            cmb_product_groups.DisplayMember = "name";
            cmb_product_groups.ValueMember = "code";
            cmb_product_groups.DataSource = groups;

        }

        private void cmb_product_groups_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_group_code.Text = cmb_product_groups.SelectedValue.ToString();
            
            if(txt_group_code.Text != "")
            {
                Load_grid(txt_group_code.Text);
            }
            
        }

        public void Load_grid(string group_code)
        {
            grid_product_groups.Rows.Clear();
            grid_product_groups.Refresh();

            ProductGroupsBLL objBLL = new ProductGroupsBLL();
            DataTable product_dt = new DataTable();
            product_dt = objBLL.SearchRecord(group_code);
            
            if (product_dt.Rows.Count > 0)
            {
                foreach (DataRow myProductView in product_dt.Rows)
                {

                    int id = Convert.ToInt32(myProductView["id"]);
                    string code = myProductView["code"].ToString();
                    string name = myProductView["name"].ToString();

                    string[] row0 = { id.ToString(), code, name };

                    grid_product_groups.Rows.Add(row0);

                }


            }
        }

        private void grid_product_groups_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if(txt_group_code.Text != "")
                {
                    var result = "";
                    ProductGroupsModal info = new ProductGroupsModal();
                    ProductGroupsBLL objBLL = new ProductGroupsBLL();

                    for (int i = 0; i < grid_product_groups.Rows.Count; i++)
                    {
                        if (grid_product_groups.Rows[i].Cells["code"].Value != null)
                        {
                            info.group_code = txt_group_code.Text;
                            info.product_id = grid_product_groups.Rows[i].Cells["code"].Value.ToString();

                            result = objBLL.InsertProductGroupDetail(info); // for sales items

                        }

                    }
                    if(result.ToString().Length > 0)
                    {
                        MessageBox.Show("Record Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            if(txt_group_code.Text != "" && e.KeyData == Keys.Enter)
            {
                frm_searchProducts search_product_obj = new frm_searchProducts(null, this,null, txt_product_code.Text, "", "", 0, false);
                search_product_obj.ShowDialog();

            }
        }

        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchProducts search_product_obj = new frm_searchProducts(null, this,null, txt_product_code.Text, "", "", 0, false);
            search_product_obj.ShowDialog();

        }
        public void load_products(string product_id = "")
        {

            ProductBLL productsBLL_obj = new ProductBLL();
            DataTable product_dt = new DataTable();

            if (product_id != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByProductID(product_id);
            }

            if (product_dt.Rows.Count > 0)
            {
                foreach (DataRow myProductView in product_dt.Rows)
                {

                    int id = Convert.ToInt32(myProductView["id"]);
                    string code = myProductView["code"].ToString();
                    string name = myProductView["name"].ToString();
                    
                    string[] row0 = { id.ToString(), code, name};

                    grid_product_groups.Rows.Add(row0);

                }


            }
            else
            {
                MessageBox.Show("Record not found", "Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

    }
}
