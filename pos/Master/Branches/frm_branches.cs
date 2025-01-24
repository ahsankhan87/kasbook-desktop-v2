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
    public partial class frm_branches : Form
    {

        public frm_branches()
        {
            InitializeComponent();
        }


        public void frm_branches_Load(object sender, EventArgs e)
        {
            load_branches_grid();
        }

        public void load_branches_grid()
        {
            try
            {
                grid_branches.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_branches.AutoGenerateColumns = false;

                String keyword = "id,name,description, date_created";
                String table = "pos_branches";
                grid_branches.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addBranch frm_addBranch_obj = new frm_addBranch(this);
            frm_addBranch.instance.tb_lbl_is_edit.Text = "false";

            frm_addBranch_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string id = grid_branches.CurrentRow.Cells[0].Value.ToString();
            string title = grid_branches.CurrentRow.Cells[1].Value.ToString();
            string rate = grid_branches.CurrentRow.Cells[2].Value.ToString();
            
            frm_addBranch frm_addBranch_obj = new frm_addBranch(this);
            frm_addBranch.instance.tb_lbl_is_edit.Text = "true";

            frm_addBranch.instance.tb_id.Text = id;
            frm_addBranch.instance.tb_name.Text = title;
            
            frm_addBranch.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_branches.CurrentRow.Cells[0].Value.ToString();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {

                BranchesBLL objBLL = new BranchesBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_branches_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_branches_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_branches.DataSource = null;

                    //bind data in data grid view  
                    BranchesBLL objBLL = new BranchesBLL();
                    //grid_branches.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_branches.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_branches_KeyDown(object sender, KeyEventArgs e)
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
