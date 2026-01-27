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
using pos.UI;
using pos.UI.Busy;

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
            try
            {
                if (_user_id <= 0)
                {
                    UiMessages.ShowError(
                        "Invalid user.",
                        "مستخدم غير صالح.",
                        "Error",
                        "خطأ"
                    );
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_password.Text) || string.IsNullOrWhiteSpace(txt_confirm_pwd.Text))
                {
                    UiMessages.ShowInfo(
                        "Password and confirm password are required.",
                        "كلمة المرور وتأكيد كلمة المرور مطلوبان.",
                        "Validation",
                        "التحقق"
                    );
                    return;
                }

                if (!string.Equals(txt_password.Text, txt_confirm_pwd.Text, StringComparison.Ordinal))
                {
                    UiMessages.ShowInfo(
                        "Password does not match.",
                        "كلمة المرور غير متطابقة.",
                        "Password Change",
                        "تغيير كلمة المرور"
                    );
                    return;
                }

                if (txt_password.Text.Length < 2)
                {
                    UiMessages.ShowInfo(
                        "Password is too short.",
                        "كلمة المرور قصيرة جدًا.",
                        "Validation",
                        "التحقق"
                    );
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    "Change password for this user?",
                    "هل تريد تغيير كلمة المرور لهذا المستخدم؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T("Updating password...", "جاري تحديث كلمة المرور...")))
                {
                    UsersModal info = new UsersModal();
                    info.password = txt_password.Text;
                    info.id = _user_id;

                    UsersBLL objBLL = new UsersBLL();
                    int result = objBLL.UpdatePassword(info);
                    if (result > 0)
                    {
                        UiMessages.ShowInfo(
                            "Password updated successfully.",
                            "تم تحديث كلمة المرور بنجاح.",
                            "Success",
                            "نجاح"
                        );
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "Password was not updated.",
                            "لم يتم تحديث كلمة المرور.",
                            "Error",
                            "خطأ"
                        );
                        return;
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            var confirm = UiMessages.ConfirmYesNo(
                "Close without saving?",
                "هل تريد الإغلاق بدون حفظ؟",
                captionEn: "Confirm",
                captionAr: "تأكيد"
            );

            if (confirm == DialogResult.Yes)
            {
                this.Dispose();
                this.Close();
            }
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
