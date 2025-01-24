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
    public partial class frm_groups : Form
    {

        public frm_groups()
        {
            InitializeComponent();
        }


        public void frm_groups_Load(object sender, EventArgs e)
        {
            load_groups_grid();
        }

        public void load_groups_grid()
        {
            try
            {
                grid_groups.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_groups.AutoGenerateColumns = false;

                String keyword = "*";
                String table = "acc_groups";
                grid_groups.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addGroup frm_addGroup_obj = new frm_addGroup(this);
            frm_addGroup.instance.tb_lbl_is_edit.Text = "false";

            frm_addGroup_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string id = grid_groups.CurrentRow.Cells["id"].Value.ToString();
            string title = grid_groups.CurrentRow.Cells["name"].Value.ToString();
            string name_2 = grid_groups.CurrentRow.Cells["name_2"].Value.ToString();
            string parent_id = grid_groups.CurrentRow.Cells["parent_id"].Value.ToString();
            string account_type_id = grid_groups.CurrentRow.Cells["account_type_id"].Value.ToString();
            
            frm_addGroup frm_addGroup_obj = new frm_addGroup(this);
            frm_addGroup.instance.tb_lbl_is_edit.Text = "true";

            frm_addGroup.instance.tb_id.Text = id;
            frm_addGroup.instance.tb_name.Text = title;
            frm_addGroup.instance.tb_name_2.Text = name_2;
            frm_addGroup.instance.tb_account_type.SelectedValue = account_type_id;
            frm_addGroup.instance.tb_parent_id.SelectedValue = parent_id;
            
            frm_addGroup.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_groups.CurrentRow.Cells[0].Value.ToString();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {

                GroupsBLL objBLL = new GroupsBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_groups_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_groups_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_groups.DataSource = null;

                    //bind data in data grid view  
                    GroupsBLL objBLL = new GroupsBLL();
                    //grid_groups.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_groups.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_groups_KeyDown(object sender, KeyEventArgs e)
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
