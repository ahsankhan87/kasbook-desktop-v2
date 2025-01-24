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
    public partial class frm_product_groups : Form
    {

        public frm_product_groups()
        {
            InitializeComponent();
        }


        public void frm_product_groups_Load(object sender, EventArgs e)
        {
            load_ProductGroups_grid();
        }

        public void load_ProductGroups_grid()
        {
            try
            {
                grid_product_groups.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_product_groups.AutoGenerateColumns = false;

                String keyword = "id,code,name,date_created";
                String table = "pos_product_groups";
                grid_product_groups.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addProductGroup frm_addProductGroup_obj = new frm_addProductGroup(this);
            frm_addProductGroup.instance.tb_lbl_is_edit.Text = "false";

            frm_addProductGroup_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string id = grid_product_groups.CurrentRow.Cells["id"].Value.ToString();
            string code = grid_product_groups.CurrentRow.Cells["code"].Value.ToString();
            string name = grid_product_groups.CurrentRow.Cells["name"].Value.ToString();
            
            frm_addProductGroup frm_addProductGroup_obj = new frm_addProductGroup(this);
            frm_addProductGroup.instance.tb_lbl_is_edit.Text = "true";

            frm_addProductGroup.instance.tb_id.Text = id;
            frm_addProductGroup.instance.tb_code.Text = code;
            frm_addProductGroup.instance.tb_name.Text = name;
            
            frm_addProductGroup.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_product_groups.CurrentRow.Cells[0].Value.ToString();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {

                ProductGroupsBLL objBLL = new ProductGroupsBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_ProductGroups_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_ProductGroups_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_product_groups.DataSource = null;

                    //bind data in data grid view  
                    ProductGroupsBLL objBLL = new ProductGroupsBLL();
                    //grid_product_groups.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_product_groups.DataSource = objBLL.SearchRecord(condition);

                    //txt_search.Text = "";
                
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

        private void frm_product_groups_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.N)
            {
                btn_new.PerformClick();

            }

            if (e.Control == true && e.KeyCode == Keys.U)
            {
                btn_update.PerformClick();
            }

            if (e.Control == true && e.KeyCode == Keys.D)
            {
                btn_delete.PerformClick();

            }
        }

        private void btn_assign_product_Click(object sender, EventArgs e)
        {
            frm_assign_products frm = new frm_assign_products();
            frm.ShowDialog();
        }
    }
}
