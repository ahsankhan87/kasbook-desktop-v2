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
    public partial class frm_accounts : Form
    {

        public frm_accounts()
        {
            InitializeComponent();
        }


        public void frm_accounts_Load(object sender, EventArgs e)
        {
            load_accounts_grid();
        }

        public void load_accounts_grid()
        {
            try
            {
                grid_accounts.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_accounts.AutoGenerateColumns = false;

                String keyword = "AC.id,AC.group_id,G.name AS group_name,G.name_2 AS group_name_2,AC.code,AC.name,AC.name_2 AS name_2,AC.description,AC.op_dr_balance,AC.op_cr_balance,AC.date_created";
                String table = "acc_accounts AC LEFT JOIN acc_groups G ON AC.group_id=G.id WHERE AC.branch_id = "+UsersModal.logged_in_branch_id+"";
                grid_accounts.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addAccount frm_addAccount_obj = new frm_addAccount(this);
            frm_addAccount.instance.tb_lbl_is_edit.Text = "false";

            frm_addAccount_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string id = grid_accounts.CurrentRow.Cells["id"].Value.ToString();
            string name = grid_accounts.CurrentRow.Cells["name"].Value.ToString();
            string name_2 = grid_accounts.CurrentRow.Cells["name_2"].Value.ToString();
            string code = grid_accounts.CurrentRow.Cells["code"].Value.ToString();
            string group_id = grid_accounts.CurrentRow.Cells["group_id"].Value.ToString();
            string op_dr_balance = grid_accounts.CurrentRow.Cells["op_dr_balance"].Value.ToString();
            string op_cr_balance = grid_accounts.CurrentRow.Cells["op_cr_balance"].Value.ToString();
            
            frm_addAccount frm_addAccount_obj = new frm_addAccount(this);
            frm_addAccount.instance.tb_lbl_is_edit.Text = "true";

            frm_addAccount.instance.tb_id.Text = id;
            frm_addAccount.instance.tb_name.Text = name;
            frm_addAccount.instance.tb_name_2.Text = name_2;
            frm_addAccount.instance.tb_code.Text = code;
            frm_addAccount.instance.tb_op_dr_balance.Text = op_dr_balance;
            frm_addAccount.instance.tb_op_cr_balance.Text = op_cr_balance;
            frm_addAccount.instance.tb_group_id.SelectedValue = group_id;
            frm_addAccount.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_accounts.CurrentRow.Cells[0].Value.ToString();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {

                AccountsBLL objBLL = new AccountsBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_accounts_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_accounts_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_accounts.DataSource = null;

                    //bind data in data grid view  
                    AccountsBLL objBLL = new AccountsBLL();
                    //grid_accounts.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_accounts.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_accounts_KeyDown(object sender, KeyEventArgs e)
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
