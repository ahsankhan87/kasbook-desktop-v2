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
    public partial class frm_pwd_change : Form
    {
        public frm_adduser mainForm;
        
        public int _user_id;
        string _username;

        public frm_pwd_change(frm_adduser mainForm, int user_id, string username)
        {
            this.mainForm = mainForm;
            _user_id = user_id;
            _username = username;
            
            InitializeComponent();
        }

        public frm_pwd_change()
        {
            InitializeComponent();
            

        }

        public void frm_pwd_change_Load(object sender, EventArgs e)
        {
            //get_branches_dropdownlist();
            //load_user_detail(_user_id);
            txt_username.Text = _username;
        }


        public void load_user_detail(int user_id)
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "*";
            string table = "pos_users WHERE id = '" + user_id + "'";

            DataTable users = generalBLL_obj.GetRecord(keyword, table);

            foreach (DataRow dr in users.Rows)
            {
                //txt_id.Text = dr["id"].ToString();
                //txt_name.Text = dr["name"].ToString();
                //txt_username.Text = dr["username"].ToString();
                
                //cmb_branches.SelectedValue = dr["branch_id"].ToString();
                
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {


            if (txt_password.Text == txt_confirm_pwd.Text)
                {
                    UsersModal info = new UsersModal();
                    //info.username = txt_username.Text;
                    info.password = txt_password.Text;
                   
                    UsersBLL objBLL = new UsersBLL();


                    info.id = _user_id;
                    
                        int result = objBLL.UpdatePassword(info);
                        if (result > 0)
                        {
                            MessageBox.Show("Password updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    
                    
                    //frm_users obj_frm_cust = new frm_users();
                    //obj_frm_cust.Close();
                    //obj_frm_cust.ShowDialog();
                    //mainForm.load_user_detail();
                    //obj_frm_cust.load_userss_grid();
                    //obj_frm_cust.frm_users_Load(sender,e);

                    this.Close();

                }
                else
                {
                    MessageBox.Show("Password does not match", "Password Change", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
            
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
            this.Close();
        }

        private void frm_pwd_change_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        
    }
}
