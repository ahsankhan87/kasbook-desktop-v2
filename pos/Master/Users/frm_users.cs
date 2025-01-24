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
    public partial class frm_users : Form
    {

        public frm_users()
        {
            InitializeComponent();
        }


        public void frm_users_Load(object sender, EventArgs e)
        {
            load_users_grid();
        }

        public void load_users_grid()
        {
            try
            {
                grid_users.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_users.AutoGenerateColumns = false;

                String keyword = "*";
                String table = "pos_users";
                grid_users.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_adduser frm_adduser_obj = new frm_adduser(this, 0, "false");
           
            frm_adduser_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string id = grid_users.CurrentRow.Cells["id"].Value.ToString();
            
            frm_adduser frm_adduser_obj = new frm_adduser(this,int.Parse(id),"true");
            frm_adduser_obj.ShowDialog();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_users.CurrentRow.Cells["id"].Value.ToString();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {

                UsersBLL objBLL = new UsersBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_users_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_users_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_users.DataSource = null;

                    //bind data in data grid view  
                    UsersBLL objBLL = new UsersBLL();
                    //grid_users.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_users.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_users_KeyDown(object sender, KeyEventArgs e)
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

        private void btn_pwd_change_Click(object sender, EventArgs e)
        {
            //string id = grid_users.CurrentRow.Cells["id"].Value.ToString();
            //string username = grid_users.CurrentRow.Cells["username"].Value.ToString();
            
            //frm_pwd_change frm = new frm_pwd_change(this,int.Parse(id),username);
            //frm.ShowDialog();
        }
    }
}
