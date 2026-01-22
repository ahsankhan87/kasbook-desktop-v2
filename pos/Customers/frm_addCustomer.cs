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
using pos.Security.Authorization;
using POS.BLL;
using POS.Core;

namespace pos
{
    public partial class frm_addCustomer : Form
    {
        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        public frm_addCustomer()
        {
            InitializeComponent();
            // Ensure user identity exists; hydrate claims from DB
            if (_currentUser == null)
            {
                var parsedRole = SystemRole.Viewer;
                System.Enum.TryParse(UsersModal.logged_in_user_role, true, out parsedRole);
                AppSecurityContext.SetUser(new UserIdentity
                {
                    UserId = UsersModal.logged_in_userid,
                    BranchId = UsersModal.logged_in_branch_id,
                    Username = UsersModal.logged_in_username,
                    Role = parsedRole
                });
                _currentUser = AppSecurityContext.User;
            }
        }
        
        public void frm_addCustomer_Load(object sender, EventArgs e)
        {
            txt_search.Focus();
            this.ActiveControl = txt_search;
            get_accounts_dropdownlist();
            // Disable/hide actions based on DB-backed permissions
            //btn_save.Enabled = _auth.HasPermission(_currentUser, Permissions.Customers_Edit);
            //btn_update.Enabled = _auth.HasPermission(_currentUser, Permissions.Customers_Edit);
            //btn_delete.Enabled = _auth.HasPermission(_currentUser,Permissions.Customers_Delete);
            //btn_payment.Enabled = _auth.HasPermission(_currentUser, Permissions.Customers_LedgerPayment);
            //Btn_ledger_report.Enabled = _auth.HasPermission(_currentUser, Permissions.Customers_LedgerPrint);
            //Btn_printCustomerReceipt.Enabled = _auth.HasPermission(_currentUser, Permissions.Customers_LedgerPrint);
            //grid_customer_transactions.Enabled = _auth.HasPermission(_currentUser, Permissions.Customers_LedgerView);
            // Add further UI elements here as needed, e.g. delete/report buttons/menus.
        }
        public void get_accounts_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_accounts";

            DataTable accounts = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = accounts.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            accounts.Rows.InsertAt(emptyRow, 0);

            cmb_GL_account_code.DisplayMember = "name";
            cmb_GL_account_code.ValueMember = "id";
            cmb_GL_account_code.DataSource = accounts;

            cmb_GL_account_code.SelectedValue = "5"; // 5 is the default Ac receiavable Account id in acc_accounts table

        }
        public void load_customer_detail(int customer_id)
        {
            CustomerBLL objBLL = new CustomerBLL();
            DataTable dt = objBLL.SearchRecordByCustomerID(customer_id);
            foreach (DataRow myProductView in dt.Rows)
            {
                txt_id.Text = myProductView["id"].ToString();
                txt_first_name.Text = myProductView["first_name"].ToString();
                txt_last_name.Text = myProductView["last_name"].ToString();
                txt_address.Text = myProductView["address"].ToString();
                txt_vatno.Text = myProductView["vat_no"].ToString();
                txt_contact_no.Text = myProductView["contact_no"].ToString();
                txt_email.Text = myProductView["email"].ToString();
                txt_vin_no.Text = myProductView["vin_no"].ToString();
                txt_car_name.Text = myProductView["car_name"].ToString();
                txt_credit_limit.Text = myProductView["credit_limit"].ToString();
                txt_StreetName.Text = myProductView["StreetName"].ToString();
                txt_cityName.Text = myProductView["CityName"].ToString();
                txt_buildingNumber.Text = myProductView["BuildingNumber"].ToString();
                txt_citySubdivisionName.Text = myProductView["CitySubdivisionName"].ToString();
                txt_postalCode.Text = myProductView["PostalCode"].ToString();
                txt_countryName.Text = myProductView["CountryName"].ToString();
                txt_registrationName.Text = myProductView["RegistrationName"].ToString();
                cmb_GL_account_code.SelectedValue = (myProductView["GLAccountID"].ToString() == "" ? 0 : Convert.ToInt32(myProductView["GLAccountID"].ToString()));

            }
            lbl_customer_name.Visible = true;
            lbl_customer_name.Text = txt_first_name.Text + ' ' + txt_last_name.Text;
        }

        public DataTable get_GL_accounts_dt()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_accounts";

            DataTable dt = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = dt.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Select Account";              // Set Column Value
            dt.Rows.InsertAt(emptyRow, 0);
            return dt;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                // Permission check
                if (!_auth.HasPermission(_currentUser, Permissions.Customers_Create))
                {
                    MessageBox.Show("You do not have permission to perform this action.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate required fields
                if (txt_first_name.Text == string.Empty)
                {
                    MessageBox.Show("Please enter first name", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if(txt_registrationName.Text == string.Empty)
                {
                    MessageBox.Show("Please enter registration name", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txt_first_name.Text != string.Empty)
                {
                    CustomerModal info = new CustomerModal
                    {
                        first_name = txt_first_name.Text,
                        last_name = txt_last_name.Text,
                        email = txt_email.Text,
                        vat_no = txt_vatno.Text,
                        address = txt_address.Text,
                        contact_no = txt_contact_no.Text,
                        vin_no = txt_vin_no.Text,
                        car_name = txt_car_name.Text,
                        credit_limit = (txt_credit_limit.Text != "" ? Convert.ToDouble(txt_credit_limit.Text) : 0),
                        StreetName = txt_StreetName.Text.Trim(),
                        CityName = txt_cityName.Text.Trim(),
                        BuildingNumber = txt_buildingNumber.Text.Trim(),
                        CitySubdivisionName = txt_citySubdivisionName.Text.Trim(),
                        PostalCode = txt_postalCode.Text.Trim(),
                        CountryName = txt_countryName.Text.Trim(),
                        registrationName = txt_registrationName.Text.Trim(),
                        date_created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        GLAccountID = Convert.ToInt32(cmb_GL_account_code.SelectedValue)

                    };

                    CustomerBLL objBLL = new CustomerBLL();
                    int result = objBLL.Insert(info);
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
                    MessageBox.Show("Please enter value in field", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //this.Dispose(); 
            this.Close();
        }

        private void frm_addCustomer_KeyDown(object sender, KeyEventArgs e)
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
            
        }

        private void clear_all()
        {
            txt_id.Text = "";
            txt_first_name.Text = "";
            txt_last_name.Text = "";
            txt_address.Text = "";
            txt_vatno.Text = "";
            txt_contact_no.Text = "";
            txt_email.Text = "";
            txt_vin_no.Text = "";
            txt_car_name.Text = "";
            lbl_customer_name.Text = "";
            grid_customer_transactions.DataSource = null;
            txt_credit_limit.Text = "";

            txt_StreetName.Text = "";
            txt_cityName.Text = "";
            txt_buildingNumber.Text = "";
            txt_citySubdivisionName.Text = "";
            txt_postalCode.Text = "";
            txt_countryName.Text = "";
            txt_registrationName.Text = "";
        }

        private void btn_blank_Click(object sender, EventArgs e)
        {
            clear_all();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                // Permission check
                if (!_auth.HasPermission(_currentUser, Permissions.Customers_Edit))
                {
                    MessageBox.Show("You do not have permission to perform this action.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                if (String.IsNullOrEmpty(txt_id.Text))
                {
                    MessageBox.Show("Record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txt_first_name.Text != string.Empty && txt_registrationName.Text != string.Empty && txt_vatno.Text != string.Empty)
                {
                    CustomerModal info = new CustomerModal
                    {
                        first_name = txt_first_name.Text,
                        last_name = txt_last_name.Text,
                        email = txt_email.Text,
                        vat_no = txt_vatno.Text,
                        address = txt_address.Text,
                        contact_no = txt_contact_no.Text,
                        vin_no = txt_vin_no.Text,
                        car_name = txt_car_name.Text,
                        credit_limit = (txt_credit_limit.Text != "" ? Convert.ToDouble(txt_credit_limit.Text) : 0),
                        StreetName = txt_StreetName.Text.Trim(),
                        CityName = txt_cityName.Text.Trim(),
                        BuildingNumber = txt_buildingNumber.Text.Trim(),
                        CitySubdivisionName = txt_citySubdivisionName.Text.Trim(),
                        PostalCode = txt_postalCode.Text.Trim(),
                        CountryName = txt_countryName.Text.Trim(),
                        registrationName = txt_registrationName.Text.Trim(),
                        date_updated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        GLAccountID = Convert.ToInt32(cmb_GL_account_code.SelectedValue)
                    };

                    CustomerBLL objBLL = new CustomerBLL();

                    info.id = int.Parse(txt_id.Text);

                    int result = objBLL.Update(info);
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
                    MessageBox.Show("Please enter first name", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
}

        private void btn_search_Click(object sender, EventArgs e)
        {
            frm_search_customers search_obj = new frm_search_customers(this, txt_search.Text);
            search_obj.ShowDialog();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                // Permission check
                if (!_auth.HasPermission(_currentUser, Permissions.Customers_Delete))
                {
                    MessageBox.Show("You do not have permission to perform this action.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string id = txt_id.Text;

                if (id != "")
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        CustomerBLL objBLL = new CustomerBLL();
                        int resulte = objBLL.Delete(int.Parse(id));

                        MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear_all();
                    }
                    else
                    {
                        MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            string customer_id = txt_id.Text;

            if (customer_id != "")
            {
                load_customer_detail(int.Parse(customer_id));
                
            }

        }

        private void btn_trans_refresh_Click(object sender, EventArgs e)
        {
            string customer_id = txt_id.Text;
            if (customer_id != "")
            {
                //load customer transactions in grid
                // Permission check
                if (!_auth.HasPermission(_currentUser, Permissions.Customers_LedgerView))
                {
                    MessageBox.Show("You do not have permission to perform this action.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                load_customer_transactions_grid(int.Parse(customer_id));
            }
        }

        private void btn_payment_Click(object sender, EventArgs e)
        {
            string customer_id = txt_id.Text;
            if (customer_id != "")
            {
                // Permission check
                if (!_auth.HasPermission(_currentUser, Permissions.Customers_LedgerPayment))
                {
                    MessageBox.Show("You do not have permission to perform this action.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                frm_customer_payment obj = new frm_customer_payment(this, int.Parse(customer_id));
                obj.ShowDialog();
               
            }
        }
        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_customer_transactions.Rows[grid_customer_transactions.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_customer_transactions.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }

        public void load_customer_transactions_grid(int customer_id)
        {
            try
            {
                grid_customer_transactions.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_customer_transactions.AutoGenerateColumns = false;

                String keyword = "id,invoice_no,debit,credit,(debit-credit) AS balance,description,entry_date,account_id,account_name";
                String table = "pos_customers_payments WHERE customer_id = " + customer_id + "";

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
                newRow[8] = "Total";
                newRow[2] = _dr_total;
                newRow[3] = _cr_total;
                newRow[4] = (_dr_total - _cr_total);
                dt.Rows.InsertAt(newRow, dt.Rows.Count);

                grid_customer_transactions.DataSource = dt;
                CustomizeDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void Btn_ledger_report_Click(object sender, EventArgs e)
        {
            try
            {
                // Permission check
                if (!_auth.HasPermission(_currentUser, Permissions.Customers_LedgerPrint))
                {
                    MessageBox.Show("You do not have permission to perform this action.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (String.IsNullOrEmpty(txt_id.Text))
                {
                    MessageBox.Show("Please select customer to view report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string customer_id = txt_id.Text;
                pos.Customers.Customer_Ledger_Report.FrmCustomerLedgerReport obj = new Customers.Customer_Ledger_Report.FrmCustomerLedgerReport(customer_id);
                obj.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_printCustomerReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_customer_transactions.CurrentRow == null)
                {
                    MessageBox.Show("Please select customer to view report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string id = grid_customer_transactions.CurrentRow.Cells["id"].Value.ToString();

                if (String.IsNullOrEmpty(id))
                {
                    MessageBox.Show("Please select customer to view report.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                pos.Customers.Customer_Ledger_Report.Frm_customerPaymentReceipt frm_CustomerPaymentReceipt = new Customers.Customer_Ledger_Report.Frm_customerPaymentReceipt(id);
                frm_CustomerPaymentReceipt.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
