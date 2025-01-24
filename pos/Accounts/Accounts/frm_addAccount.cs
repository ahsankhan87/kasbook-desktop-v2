using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POS.BLL;
using POS.Core;

namespace pos
{
    public partial class frm_addAccount : Form
    {
        public static frm_addAccount instance;
        public TextBox tb_id;
        public TextBox tb_name;
        public TextBox tb_name_2;
        public TextBox tb_code;
        public ComboBox tb_group_id;
        public TextBox tb_op_dr_balance;
        public TextBox tb_op_cr_balance;
        public Label tb_lbl_is_edit;
        private frm_accounts mainForm;
        
        public frm_addAccount(frm_accounts mainForm): this()
        {
            this.mainForm = mainForm;
        }

        public frm_addAccount()
        {
            InitializeComponent();
            instance = this;
            
            get_groups_dropdownlist();
            
            tb_id = txt_id;
            tb_name = txt_name;
            tb_name_2 = txt_name_2;
            tb_code = txt_account_code;
            tb_group_id = cmb_group_id;
            tb_op_dr_balance = txt_op_dr_balance;
            tb_op_cr_balance = txt_op_cr_balance;

            tb_lbl_is_edit = lbl_edit_status;

        }

        public void frm_addAccount_Load(object sender, EventArgs e)
        {
            
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update Accountes";
                
            }
            else
            {
                btn_save.Text = "Save";
            }
        }

        public void get_groups_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_groups";

            DataTable taxes = generalBLL_obj.GetRecord(keyword, table);
            //DataRow emptyRow = taxes.NewRow();
            //emptyRow[0] = Convert.ToInt32("0");              // Set Column Value
            //taxes.Rows.InsertAt(emptyRow, 0);

            cmb_group_id.DataSource = taxes;

            cmb_group_id.DisplayMember = "name";
            cmb_group_id.ValueMember = "id";
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            
            if (txt_name.Text != string.Empty)
            {
                AccountsModal info = new AccountsModal();
                info.name = txt_name.Text;
                info.name_2 = txt_name_2.Text;
                info.description = txt_description.Text;
                info.code = txt_account_code.Text;
                info.op_dr_balance = (String.IsNullOrEmpty(txt_op_dr_balance.Text)) ? 0 : Convert.ToDouble(txt_op_dr_balance.Text);
                info.op_cr_balance = (String.IsNullOrEmpty(txt_op_cr_balance.Text)) ? 0 : Convert.ToDouble(txt_op_cr_balance.Text);
                info.group_id = Convert.ToInt32(cmb_group_id.SelectedValue.ToString()); 
                    
                AccountsBLL objBLL = new AccountsBLL();
                    
                if (lbl_edit_status.Text == "true")
                {
                    info.id = int.Parse(txt_id.Text);
                    
                    int result = objBLL.Update(info);
                    if (result > 0)
                    {
                        MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    int result = objBLL.Insert(info);
                    if (result > 0)
                    {
                        MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                frm_accounts obj_frm_cust = new frm_accounts();
                //obj_frm_cust.Close();
                //obj_frm_cust.ShowDialog();
                mainForm.load_accounts_grid();
                //obj_frm_cust.load_Accountess_grid();
                //obj_frm_cust.frm_Accounts_Load(sender,e);

                this.Close();

            }
            else
            {
                MessageBox.Show("Please enter value in field", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
            
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
            this.Close();
        }

        private void frm_addAccount_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }


        private void txt_op_dr_balance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txt_op_cr_balance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        
    }
}
