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
    public partial class frm_categories : Form
    {

        public frm_categories()
        {
            InitializeComponent();
        }


        public void frm_categories_Load(object sender, EventArgs e)
        {
            load_categories_grid();
        }

        public void load_categories_grid()
        {
            try
            {
                grid_categories.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_categories.AutoGenerateColumns = false;

                String keyword = "id,code,name,date_created";
                String table = "pos_categories";
                grid_categories.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addCategory frm_addCategory_obj = new frm_addCategory(this);
            frm_addCategory.instance.tb_lbl_is_edit.Text = "false";

            frm_addCategory_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string id = grid_categories.CurrentRow.Cells["id"].Value.ToString();
            string code = grid_categories.CurrentRow.Cells["code"].Value.ToString();
            string name = grid_categories.CurrentRow.Cells["name"].Value.ToString();
            
            frm_addCategory frm_addCategory_obj = new frm_addCategory(this);
            frm_addCategory.instance.tb_lbl_is_edit.Text = "true";

            frm_addCategory.instance.tb_id.Text = id;
            frm_addCategory.instance.tb_code.Text = code;
            frm_addCategory.instance.tb_name.Text = name;
            
            frm_addCategory.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_categories.CurrentRow.Cells[0].Value.ToString();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {

                CategoriesBLL objBLL = new CategoriesBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_categories_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_categories_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_categories.DataSource = null;

                    //bind data in data grid view  
                    CategoriesBLL objBLL = new CategoriesBLL();
                    //grid_categories.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_categories.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_categories_KeyDown(object sender, KeyEventArgs e)
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
