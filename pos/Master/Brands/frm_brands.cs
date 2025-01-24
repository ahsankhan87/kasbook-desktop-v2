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
    public partial class frm_brands : Form
    {

        public frm_brands()
        {
            InitializeComponent();
        }


        public void frm_brands_Load(object sender, EventArgs e)
        {
            load_Brands_grid();
           
        }

        public void load_Brands_grid()
        {
            try
            {
                grid_brands.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_brands.AutoGenerateColumns = false;

                String keyword = "*";
                String table = "pos_brands";
                grid_brands.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addBrand frm_addBrand_obj = new frm_addBrand(this);
            frm_addBrand.instance.tb_lbl_is_edit.Text = "false";

            frm_addBrand_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string id = grid_brands.CurrentRow.Cells["id"].Value.ToString();
            string code = grid_brands.CurrentRow.Cells["code"].Value.ToString();
            string name = grid_brands.CurrentRow.Cells["name"].Value.ToString();
            string category_code = grid_brands.CurrentRow.Cells["category_code"].Value.ToString();
            string group_code = grid_brands.CurrentRow.Cells["group_code"].Value.ToString();
            
            frm_addBrand frm_addBrand_obj = new frm_addBrand(this);
            frm_addBrand.instance.tb_lbl_is_edit.Text = "true";

            frm_addBrand.instance.tb_id.Text = id;
            frm_addBrand.instance.tb_code.Text = code;
            frm_addBrand.instance.tb_name.Text = name;
            frm_addBrand.instance.tb_category.SelectedValue = category_code;
            frm_addBrand.instance.tb_groups.SelectedValue = group_code;
            
            frm_addBrand.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_brands.CurrentRow.Cells[0].Value.ToString();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {

                BrandsBLL objBLL = new BrandsBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_Brands_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_Brands_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_brands.DataSource = null;

                    //bind data in data grid view  
                    BrandsBLL objBLL = new BrandsBLL();
                    //grid_brands.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_brands.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_brands_KeyDown(object sender, KeyEventArgs e)
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

    }
}
