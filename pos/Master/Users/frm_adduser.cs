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
    public partial class frm_adduser : Form
    {
        public frm_users mainForm;
        
        public int _user_id;
        string _status;

        public frm_adduser(frm_users mainForm, int user_id, string status)
        {
            this.mainForm = mainForm;
            _user_id = user_id;
            _status = status;
            InitializeComponent();
        }

        public frm_adduser()
        {
            InitializeComponent();
            

        }

        public void frm_adduser_Load(object sender, EventArgs e)
        {
            Load_all_menu();
           
            get_branches_dropdownlist();
            get_language_dropdownlist();
            get_userrole_dropdownlist();

            load_user_detail(_user_id);
            load_user_rights(_user_id);
            load_user_commission_grid(_user_id);
            check_all_modules(_user_id);
            
            //lbl_cpwd.Visible = false;
            //lbl_pwd.Visible = false;
            //txt_confirm_pwd.Visible = false;
           // txt_password.Visible = false;
            
        }

        private void Load_all_menu()
        {
            ModulesBLL moduleDLL = new ModulesBLL();
            ///Master Menu
            DataTable dt = moduleDLL.GetParentModules(1); //0 for all parent modules

            checkedListBox_Master.DataSource = dt;
            checkedListBox_Master.DisplayMember = "name";
            checkedListBox_Master.ValueMember = "id";
            ////
            ////POS Menu
            DataTable dt_pos = moduleDLL.GetParentModules(2); //0 for all parent modules

            checkedListBox_POS.DataSource = dt_pos;
            checkedListBox_POS.DisplayMember = "name";
            checkedListBox_POS.ValueMember = "id";
            ///
            ////Accounts Menu
            DataTable dt_accounts = moduleDLL.GetParentModules(3); //0 for all parent modules

            checkedListBox_Accounts.DataSource = dt_accounts;
            checkedListBox_Accounts.DisplayMember = "name";
            checkedListBox_Accounts.ValueMember = "id";
            ///
            ////reports Menu
            DataTable dt_reports = moduleDLL.GetParentModules(4); //0 for all parent modules

            checkedListBox_Reports.DataSource = dt_reports;
            checkedListBox_Reports.DisplayMember = "name";
            checkedListBox_Reports.ValueMember = "id";
            ///
            ////services Menu
            DataTable dt_services = moduleDLL.GetParentModules(5); //0 for all parent modules

            checkedListBox_Services.DataSource = dt_services;
            checkedListBox_Services.DisplayMember = "name";
            checkedListBox_Services.ValueMember = "id";
            ///
            ////HR Menu
            DataTable dt_HR = moduleDLL.GetParentModules(6); //0 for all parent modules

            checkedListBox_HR.DataSource = dt_HR;
            checkedListBox_HR.DisplayMember = "name";
            checkedListBox_HR.ValueMember = "id";
            ///

        }
        public void check_all_modules(int user_id)
        {  
            //////
            ModulesBLL usermoduleDLL = new ModulesBLL();

            DataTable dt_parent_menus = usermoduleDLL.GetParentModules(0); //GET ALL PARENT MENU FIRST
            
            foreach(DataRow dr in dt_parent_menus.Rows)
            {
                DataTable dt_1 = usermoduleDLL.GetUsersModules_ByParent((int)dr["id"], user_id); //get all modules according to parent id

                if(dr["name"].ToString() == "Master")
                {
                    for (int i = 0; i < dt_1.Rows.Count; i++)
                    {
                        checkedListBox_Master.SetItemChecked((int)dt_1.Rows[i]["checked_index"], true);
                    }
                }
                if (dr["name"].ToString() == "POS")
                {
                    for (int i = 0; i < dt_1.Rows.Count; i++)
                    {
                        checkedListBox_POS.SetItemChecked((int)dt_1.Rows[i]["checked_index"], true);
                    }
                }
                if (dr["name"].ToString() == "Accounts")
                {
                    for (int i = 0; i < dt_1.Rows.Count; i++)
                    {
                        checkedListBox_Accounts.SetItemChecked((int)dt_1.Rows[i]["checked_index"], true);
                    }
                }
                if (dr["name"].ToString() == "Reports")
                {
                    for (int i = 0; i < dt_1.Rows.Count; i++)
                    {
                        checkedListBox_Reports.SetItemChecked((int)dt_1.Rows[i]["checked_index"], true);
                    }
                }
                if (dr["name"].ToString() == "Services")
                {
                    for (int i = 0; i < dt_1.Rows.Count; i++)
                    {
                        checkedListBox_Services.SetItemChecked((int)dt_1.Rows[i]["checked_index"], true);
                    }
                }
                if (dr["name"].ToString() == "Human Resource")
                {
                    for (int i = 0; i < dt_1.Rows.Count; i++)
                    {
                        checkedListBox_HR.SetItemChecked((int)dt_1.Rows[i]["checked_index"], true);
                    }
                }
            }
            
            //////////
        }

        public void load_user_detail(int user_id)
        {
            UsersBLL generalBLL_obj = new UsersBLL();
            
            DataTable users = generalBLL_obj.GetUser(user_id);

            foreach (DataRow dr in users.Rows)
            {
                txt_id.Text = dr["id"].ToString();
                txt_name.Text = dr["name"].ToString();
                txt_username.Text = dr["username"].ToString();
                txt_commission_percent.Text = dr["commission_percent"].ToString();

                cmb_user_role.SelectedValue = dr["user_level"];
                cmb_branches.SelectedValue = dr["branch_id"].ToString();
                cmb_lang.SelectedValue = dr["language"].ToString();
                
            }
            lbl_name.Visible = true;
            lbl_name.Text = txt_name.Text;
        }
        public void load_user_rights(int user_id)
        {
            UsersBLL userBLL_obj = new UsersBLL();
            DataTable users = userBLL_obj.GetUserRights(user_id);

            foreach (DataRow dr in users.Rows)
            {
                ///USER RIGHTS
                
                txt_cash_sales_amt.Text = dr["cash_sales_amount"].ToString();
                txt_credit_sales_amt.Text = dr["credit_sales_amount"].ToString();
                txt_cash_purchase_amt.Text = dr["cash_purchase_amount"].ToString();
                txt_credit_purchase_amt.Text = dr["credit_purchase_amount"].ToString();
                //chk_allow_cash_sales.Checked = (bool)dr["allow_cash_sales"];
                chk_allow_credit_sales.Checked = (bool)dr["allow_credit_sales"];
                //chk_allow_cash_purchase.Checked = (bool)dr["allow_cash_purchase"];
                chk_allow_credit_purchase.Checked = (bool)dr["allow_credit_purchase"];

            }
        }

        public void get_branches_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "pos_branches";
            DataTable branches = generalBLL_obj.GetRecord(keyword,table);

            DataRow emptyRow = branches.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            branches.Rows.InsertAt(emptyRow, 0);

            cmb_branches.DisplayMember = "name";
            cmb_branches.ValueMember = "id";
            cmb_branches.DataSource = branches;

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            UsersBLL objBLL = new UsersBLL();
            if (objBLL.IsUsernameExist(txt_username.Text))
            {
                MessageBox.Show("Username already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
    
                if (txt_name.Text != string.Empty && txt_username.Text != "" && txt_password.Text != "")
                {
                    UsersModal info = new UsersModal();
                    info.companyID = UsersModal.loggedIncompanyID;
                    info.name = txt_name.Text;
                    info.username = txt_username.Text;
                    info.password = txt_password.Text;
                    info.branch_id = Convert.ToInt32(cmb_branches.SelectedValue);
                    info.language = cmb_lang.SelectedValue.ToString();
                    info.user_role = cmb_user_role.Text;
                    info.user_level = Convert.ToInt32(cmb_user_role.SelectedValue);
                    info.user_commission_percent = (string.IsNullOrEmpty(txt_commission_percent.Text) ? 0 : int.Parse(txt_commission_percent.Text));

                    
                    int result = objBLL.Insert(info); // result is user_id
                        
                    /////////INSERTING USER MODUELS
                    foreach (object itemChecked in checkedListBox_Master.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string moduleName = castedItem["name"].ToString();
                        int moduleID = (int)castedItem["id"];

                        info.user_id = result;
                        info.module_name = moduleName;
                        info.module_id = moduleID;
                        objBLL.InsertUserModules(info);
                    }
                    foreach (object itemChecked in checkedListBox_POS.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string moduleName = castedItem["name"].ToString();
                        int moduleID = (int)castedItem["id"];

                        info.user_id = result;
                        info.module_name = moduleName;
                        info.module_id = moduleID;
                        objBLL.InsertUserModules(info);
                    }
                    foreach (object itemChecked in checkedListBox_Accounts.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string moduleName = castedItem["name"].ToString();
                        int moduleID = (int)castedItem["id"];

                        info.user_id = result;
                        info.module_name = moduleName;
                        info.module_id = moduleID;

                        objBLL.InsertUserModules(info);
                    }
                    foreach (object itemChecked in checkedListBox_Reports.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string moduleName = castedItem["name"].ToString();
                        int moduleID = (int)castedItem["id"];
                        info.user_id = result;
                        info.module_name = moduleName;
                        info.module_id = moduleID;
                        objBLL.InsertUserModules(info);
                    }
                    foreach (object itemChecked in checkedListBox_Reports.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string moduleName = castedItem["name"].ToString();
                        int moduleID = (int)castedItem["id"];
                        info.user_id = result;
                        info.module_name = moduleName;
                        info.module_id = moduleID;
                        objBLL.InsertUserModules(info);
                    }
                    foreach (object itemChecked in checkedListBox_HR.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string moduleName = castedItem["name"].ToString();
                        int moduleID = (int)castedItem["id"];
                        info.user_id = result;
                        info.module_name = moduleName;
                        info.module_id = moduleID;
                        objBLL.InsertUserModules(info);
                    }
                    ///////////


                    if (result > 0)
                    {
                        MessageBox.Show("Record created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear_all();
                    }
                    else
                    {
                        MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    

                }
                else
                {
                    MessageBox.Show("Please enter value in name, username and password field", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
            
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //this.Dispose(); 
            this.Close();
        }

        private void frm_adduser_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
            if (e.KeyData == Keys.F3)
            {
                btn_save.PerformClick();
            }
            if (e.KeyData == Keys.F4)
            {
                btn_update.PerformClick();
            }
            if (e.KeyData == Keys.F5)
            {
                btn_refresh.PerformClick();
            }
            if (e.KeyData == Keys.Delete)
            {
                btn_delete.PerformClick();
            }
        }

        private void get_language_dropdownlist()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            DataRow _row_1 = dt.NewRow();
            _row_1["id"] = "en-US";
            _row_1["name"] = "English";
            dt.Rows.Add(_row_1);
            
            DataRow _row = dt.NewRow();
            _row["id"] = "ar-SA";
            _row["name"] = "Arabic";
            dt.Rows.Add(_row);

            
            cmb_lang.DisplayMember = "name";
            cmb_lang.ValueMember = "id";
            cmb_lang.DataSource = dt;

        }

        private void get_userrole_dropdownlist()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            DataRow _row_1 = dt.NewRow();
            _row_1["id"] = "1";
            _row_1["name"] = "Administrator";
            dt.Rows.Add(_row_1);

            DataRow _row = dt.NewRow();
            _row["id"] = "2";
            _row["name"] = "User";
            dt.Rows.Add(_row);


            cmb_user_role.DisplayMember = "name";
            cmb_user_role.ValueMember = "id";
            cmb_user_role.DataSource = dt;

        }

        private void chk_master_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox_Master.Items.Count; i++)
            {
                checkedListBox_Master.SetItemChecked(i, chk_master.Checked);
            }
        }

        private void chk_pos_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox_POS.Items.Count; i++)
            {
                checkedListBox_POS.SetItemChecked(i, chk_pos.Checked);
            }
        }

        private void chk_accounts_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox_Accounts.Items.Count; i++)
            {
                checkedListBox_Accounts.SetItemChecked(i, chk_accounts.Checked);
            }
        }

        private void chk_reports_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox_Reports.Items.Count; i++)
            {
                checkedListBox_Reports.SetItemChecked(i, chk_reports.Checked);
            }
        }

        private void chk_services_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox_Services.Items.Count; i++)
            {
                checkedListBox_Services.SetItemChecked(i, chk_services.Checked);
            }
        }

        private void chk_hr_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox_HR.Items.Count; i++)
            {
                checkedListBox_HR.SetItemChecked(i, chk_hr.Checked);
            }
        }

        private void txt_cash_sales_amt_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txt_commission_percent_KeyPress(object sender, KeyPressEventArgs e)
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


        public void load_user_commission_grid(int user_id)
        {
            try
            {
                grid_commission.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_commission.AutoGenerateColumns = false;

                String keyword = "id,entry_date,invoice_no,debit,credit,(debit-credit) AS balance,description";
                String table = "pos_user_commission WHERE user_id = " + user_id + "";

                DataTable dt = new DataTable();
                dt = objBLL.GetRecord(keyword, table);

                double _dr_total = 0;
                double _cr_total = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    _dr_total += Convert.ToDouble(dr["debit"].ToString());
                    _cr_total += Convert.ToDouble(dr["credit"].ToString());

                }

                DataRow newRow = dt.NewRow();
                newRow[2] = "Total";
                newRow[3] = _dr_total;
                newRow[4] = _cr_total;
                newRow[5] = (_dr_total - _cr_total);
                dt.Rows.InsertAt(newRow, dt.Rows.Count);

                grid_commission.DataSource = dt;
                ViewTotalInLastRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void ViewTotalInLastRow()
        {
            grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["invoice_no"].Style.BackColor = Color.LightGray;
            grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["entry_date"].Style.BackColor = Color.LightGray;
            //grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["account_name"].Style.BackColor = Color.LightGray;
            grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["debit"].Style.BackColor = Color.LightGray;
            grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["credit"].Style.BackColor = Color.LightGray;
            grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["balance"].Style.BackColor = Color.LightGray;
            grid_commission.Rows[grid_commission.Rows.Count - 1].Cells["description"].Style.BackColor = Color.LightGray;


        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            string user_id = txt_id.Text;

            if (user_id != "")
            {
                load_user_commission_grid(int.Parse(user_id));
                ViewTotalInLastRow();
            }
            
        }

        private void btn_payment_Click(object sender, EventArgs e)
        {
            string user_id = txt_id.Text;

            if (user_id != "")
            {
                frm_user_commission_payment obj = new frm_user_commission_payment(this, int.Parse(user_id));
                obj.ShowDialog();
                ViewTotalInLastRow();
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string search = txt_search.Text;
            if (search != "")
            {
                frm_search_users search_product_obj = new frm_search_users(this, search);
                search_product_obj.ShowDialog();
            }
        }

        private void btn_pwd_change_Click(object sender, EventArgs e)
        {
            string id = txt_id.Text;
            string username = txt_username.Text;
            
            if (id != "")
            {
                frm_pwd_change frm = new frm_pwd_change(this, int.Parse(id), username);
                frm.ShowDialog();
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (txt_name.Text != string.Empty)
            {
                UsersModal info = new UsersModal();
                info.name = txt_name.Text;
                info.username = txt_username.Text;
                info.password = txt_password.Text;
                info.branch_id = Convert.ToInt32(cmb_branches.SelectedValue);
                info.language = cmb_lang.SelectedValue.ToString();
                info.user_role = cmb_user_role.Text;
                info.user_level = Convert.ToInt32(cmb_user_role.SelectedValue);
                info.user_commission_percent = int.Parse(txt_commission_percent.Text);

                UsersBLL objBLL = new UsersBLL();

                
                    info.id = int.Parse(txt_id.Text);

                    int result = objBLL.Update(info);

                    /////////DELETE USER MODULES FIRST AND THEN INSERT AGAINS USER MODULES
                    objBLL.DeleteUserModules(int.Parse(txt_id.Text));
                    /////
                    foreach (object itemChecked in checkedListBox_Master.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string moduleName = castedItem["name"].ToString();
                        int moduleID = (int)castedItem["id"];

                        info.user_id = result;
                        info.module_name = moduleName;
                        info.module_id = moduleID;
                        objBLL.InsertUserModules(info);
                    }
                    foreach (object itemChecked in checkedListBox_POS.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string moduleName = castedItem["name"].ToString();
                        int moduleID = (int)castedItem["id"];

                        info.user_id = result;
                        info.module_name = moduleName;
                        info.module_id = moduleID;
                        objBLL.InsertUserModules(info);
                    }
                    foreach (object itemChecked in checkedListBox_Accounts.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string moduleName = castedItem["name"].ToString();
                        int moduleID = (int)castedItem["id"];

                        info.user_id = result;
                        info.module_name = moduleName;
                        info.module_id = moduleID;

                        objBLL.InsertUserModules(info);
                    }
                    foreach (object itemChecked in checkedListBox_Reports.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string moduleName = castedItem["name"].ToString();
                        int moduleID = (int)castedItem["id"];
                        info.user_id = result;
                        info.module_name = moduleName;
                        info.module_id = moduleID;
                        objBLL.InsertUserModules(info);
                    }
                    foreach (object itemChecked in checkedListBox_Reports.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string moduleName = castedItem["name"].ToString();
                        int moduleID = (int)castedItem["id"];
                        info.user_id = result;
                        info.module_name = moduleName;
                        info.module_id = moduleID;
                        objBLL.InsertUserModules(info);
                    }
                    foreach (object itemChecked in checkedListBox_HR.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string moduleName = castedItem["name"].ToString();
                        int moduleID = (int)castedItem["id"];
                        info.user_id = result;
                        info.module_name = moduleName;
                        info.module_id = moduleID;
                        objBLL.InsertUserModules(info);
                    }
                    ///////////

                    ///USER RIGHTS
                    ////////////DELETE USER Rights FIRST AND THEN INSERT AGAINS USER rights
                    objBLL.DeleteUserRights(int.Parse(txt_id.Text));
                    /////
                    info.cash_sales_amount_limit = (txt_cash_sales_amt.Text == string.Empty ? 0 : Convert.ToDouble(txt_cash_sales_amt.Text));
                    info.credit_sales_amount_limit = (txt_credit_sales_amt.Text == string.Empty ? 0 : Convert.ToDouble(txt_credit_sales_amt.Text));
                    info.cash_purchase_amount_limit = (txt_cash_purchase_amt.Text == string.Empty ? 0 : Convert.ToDouble(txt_cash_purchase_amt.Text));
                    info.credit_purchase_amount_limit = (txt_credit_purchase_amt.Text == string.Empty ? 0 : Convert.ToDouble(txt_credit_purchase_amt.Text));
                    //info.allow_cash_sales = chk_allow_cash_sales.Checked;
                    info.allow_credit_sales = chk_allow_credit_sales.Checked;
                    //info.allow_cash_purchase = chk_allow_cash_purchase.Checked;
                    info.allow_credit_purchase = chk_allow_credit_purchase.Checked;
                    objBLL.UpdateUserRights(info);
                    ///

                    ///////////

                    if (result > 0)
                    {
                        MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear_all();
                    }
                    else
                    {
                        MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                
                
            }
            else
            {
                MessageBox.Show("Please enter value in field", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }

        private void clear_all()
        {
            txt_id.Text = "";
            txt_name.Text = "";
            txt_username.Text = "";
            txt_commission_percent.Text = "";

            txt_password.Text = "";
            txt_confirm_pwd.Text = "";

            cmb_user_role.SelectedValue = "";
            cmb_branches.SelectedIndex = 0;
            cmb_lang.SelectedIndex = 0;
            cmb_user_role.SelectedIndex = 0;
                
            txt_cash_sales_amt.Text = "";
            txt_credit_sales_amt.Text = "";
            txt_cash_purchase_amt.Text = "";
            txt_credit_purchase_amt.Text = "";
            //chk_allow_cash_sales.Checked = (bool)dr["allow_cash_sales"];
            chk_allow_credit_sales.Checked = false;
            //chk_allow_cash_purchase.Checked = (bool)dr["allow_cash_purchase"];
            chk_allow_credit_purchase.Checked = false;

            grid_commission.DataSource = null;

            for (int i = 0; i < checkedListBox_Master.Items.Count; i++)
            {
                checkedListBox_Master.SetItemChecked(i, false);
            }
       
            for (int i = 0; i < checkedListBox_POS.Items.Count; i++)
            {
                checkedListBox_POS.SetItemChecked(i, false);
            }
       
            for (int i = 0; i < checkedListBox_Accounts.Items.Count; i++)
            {
                checkedListBox_Accounts.SetItemChecked(i, false);
            }
        
            for (int i = 0; i < checkedListBox_Reports.Items.Count; i++)
            {
                checkedListBox_Reports.SetItemChecked(i, false);
            }
        
            for (int i = 0; i < checkedListBox_Services.Items.Count; i++)
            {
                checkedListBox_Services.SetItemChecked(i, false);
            }
        
            for (int i = 0; i < checkedListBox_HR.Items.Count; i++)
            {
                checkedListBox_HR.SetItemChecked(i, false);
            }
        }

        private void btn_blank_Click(object sender, EventArgs e)
        {
            clear_all();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string user_id = txt_id.Text;

            if (user_id != "")
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {

                    UsersBLL objBLL = new UsersBLL();
                    objBLL.Delete(int.Parse(user_id));

                    MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear_all();
                }
                else
                {
                    MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
        }

        private void btn_main_refresh_Click(object sender, EventArgs e)
        {
            string user_id = txt_id.Text;

            if(user_id != "")
            {
                load_user_detail(int.Parse(user_id));
                load_user_rights(int.Parse(user_id));
                load_user_commission_grid(int.Parse(user_id));
                check_all_modules(int.Parse(user_id));
            }
            
        }
    }
}
