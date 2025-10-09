using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace pos
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        
        private void Frm_Login_Load(object sender, EventArgs e)
        {
            TxtUsername.Focus();
            this.ActiveControl = TxtUsername;
            ActivateTxtUsername();
           
            if (is_company_exist())
            {
                BtnRegister.Visible = false;
            }
            else
            {
                BtnRegister.Visible = true;
            }
        }

        private void ActivateTxtUsername()
        {
            TxtUsername.BackColor = Color.White;
            panel3.BackColor = Color.White;
            TxtPassword.BackColor = SystemColors.Control;
            panel4.BackColor = SystemColors.Control;

        }
        private void ActivateTxtPassword()
        {
            TxtPassword.BackColor = Color.White;
            panel4.BackColor = Color.White;
            TxtUsername.BackColor = SystemColors.Control;
            panel3.BackColor = SystemColors.Control;
        }

        private void TxtUsername_Click(object sender, EventArgs e)
        {
            this.ActivateTxtUsername();
        }

        private void TxtPassword_Click(object sender, EventArgs e)
        {
            ActivateTxtPassword();
        }
        private void Btn_close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TxtUsername_Enter(object sender, EventArgs e)
        {
            this.ActivateTxtUsername();
        }

        private void TxtPassword_Enter(object sender, EventArgs e)
        {
            ActivateTxtPassword();
        }

        private void Bunifu_btn_submit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Login successful");
        }

        private void Frm_Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
               SendKeys.Send("{TAB}");
            }
        }
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                HardwareIdentifier systemID_obj = new HardwareIdentifier();
                string systemID = systemID_obj.GetUniqueHardwareId();

                CompaniesBLL company_obj = new CompaniesBLL();
                DataTable company_dt = company_obj.GetCompany();
                int locked = 1;
                //DateTime expiry_date = new DateTime();

                int company_id = 0;
                string company_name = "";
                string subscriptionKey = "";

                foreach (DataRow dr_company in company_dt.Rows)
                {
                    UsersModal.loggedIncompanyID = Convert.ToInt32(dr_company["id"].ToString());
                    UsersModal.logged_in_company_name = dr_company["name"].ToString();
                    UsersModal.useZatcaEInvoice = (string.IsNullOrEmpty(dr_company["useZatcaEInvoice"].ToString()) ? false : Convert.ToBoolean(dr_company["useZatcaEInvoice"]));
                    company_name = dr_company["name"].ToString();
                    locked = Convert.ToInt32(dr_company["locked"]);
                    //expiry_date = Convert.ToDateTime(dr_company["ex_date"]);
                    company_id = Convert.ToInt32(dr_company["id"]);
                    subscriptionKey = dr_company["subscriptionKey"].ToString();
                }

                if (locked > 0)
                {
                    DialogResult result = MessageBox.Show("Your account has expired. Please contact your software provider for renewal. (khybersoft.com) OR click yes to re-new account", "Software Expiration", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        frmRenewSubscrption frm = new frmRenewSubscrption();
                        frm.ShowDialog();
                    }
                }
                else
                {
                    _ = DateTime.Now;
                    //DateTime current_date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    //TimeSpan t = expiry_date - currentDate;
                    //double no_of_days = t.TotalDays;

                    Subscription subcription_obj = new Subscription();
                    bool verifySubcriptionKey = subcription_obj.VerifySubscriptionKey(company_id, subscriptionKey, out DateTime currentDate, systemID);

                    DateTime today = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    TimeSpan t = currentDate - today;
                    double no_of_days = t.TotalDays;

                    if (no_of_days <= 14)
                    {
                        MessageBox.Show("Your account will expire on " + currentDate.Date.ToShortDateString() + ", number of days left: " + no_of_days + ". Please re-new account.", "Software Expiration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    if (!verifySubcriptionKey)
                    {
                        DialogResult result = MessageBox.Show("Your account has expired. Please contact your software provider for renewal. (khybersoft.com) OR click yes to re-new account", "Software Expiration", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            frmRenewSubscrption frm = new frmRenewSubscrption();
                            frm.ShowDialog();
                        }

                    }

                    //if (no_of_days <= 0)
                    //{
                    //    MessageBox.Show("Your account has expired. Please contact your software provider for renewal. (khybersoft.com)", "Software Expiration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    updateAppLock(company_id,true);
                    //}
                    else
                    {
                        UsersModal userModal_obj = new UsersModal
                        {
                            username = TxtUsername.Text.Trim(),
                            password = TxtPassword.Text.Trim()
                        };

                        UsersBLL user_obj = new UsersBLL();
                        int user_id = user_obj.Login(userModal_obj);

                        if (user_id > 0)
                        {
                            UsersBLL obj_bll = new UsersBLL();
                            DataTable dt = obj_bll.GetUser(user_id);
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    UsersModal.logged_in_userid = (int)dr["id"];
                                    UsersModal.logged_in_username = dr["name"].ToString();

                                    BranchesBLL branchesBLL = new BranchesBLL();
                                    string branchName = branchesBLL.GetBranchNameByID((int)dr["branch_id"]);

                                    UsersModal.logged_in_branch_id = (int)dr["branch_id"];
                                    UsersModal.logged_in_branch_name = branchName;

                                    UsersModal.logged_in_lang = dr["language"].ToString();
                                    UsersModal.logged_in_user_role = dr["user_role"].ToString();
                                    UsersModal.logged_in_user_level = (int)dr["user_level"];


                                }

                                FiscalYearBLL fiscalyear_obj = new FiscalYearBLL();
                                DataTable fiscalyear_dt = fiscalyear_obj.GetActiveFiscalYear();

                                foreach (DataRow dr_fy in fiscalyear_dt.Rows)
                                {
                                    UsersModal.fiscal_year = dr_fy["name"].ToString();
                                    UsersModal.fy_from_date = Convert.ToDateTime(dr_fy["from_date"]);
                                    UsersModal.fy_to_date = Convert.ToDateTime(dr_fy["to_date"]);
                                }
                            }
                            else
                            {
                                UsersModal.logged_in_userid = 0;
                                UsersModal.logged_in_username = "";
                                UsersModal.logged_in_branch_id = 0;
                                UsersModal.logged_in_user_role = "";
                                UsersModal.logged_in_user_level = 0;
                                UsersModal.loggedIncompanyID = 0;
                            }
                            this.Hide();
                            frm_main frm_main_obj = new frm_main();
                            frm_main_obj.Show();

                        }
                        else
                        {
                            MessageBox.Show("Username or password is incorrect!", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
                            TxtUsername.Focus();
                            this.ActiveControl = TxtUsername;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during login: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool is_company_exist()
        {
            GeneralBLL objBLL = new GeneralBLL();
            String keyword = "*";
            String table = "pos_companies";
            DataTable companies_dt = objBLL.GetRecord(keyword, table);
            if (companies_dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            frm_register_company registerfrm = new frm_register_company();
            registerfrm.ShowDialog();
        }

        
    }
}
