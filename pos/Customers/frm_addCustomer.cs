using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using pos.Security.Authorization;
using POS.BLL;
using POS.Core;
using pos.UI;

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
            // Apply professional theme
            AppTheme.Apply(this);
            StyleCustomerForm();

            txt_search.Focus();
            this.ActiveControl = txt_search;
            get_accounts_dropdownlist();
            GetCustomerCode();

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

        private void StyleCustomerForm()
        {
            panel1.BackColor = AppTheme.PrimaryDark;
            panel1.ForeColor = AppTheme.TextOnPrimary;
            panel1.Padding = new Padding(8, 4, 8, 4);
            panel2.BackColor = SystemColors.Control;
            panel2.AutoScroll = true;
            panel2.AutoScrollMinSize = new Size(tableLayoutPanel1.PreferredSize.Width + 40, tableLayoutPanel1.PreferredSize.Height + 40);

            panel3.BackColor = SystemColors.Control;
            panelTransactionTop.BackColor = SystemColors.Control;
            panelTransactionBottom.BackColor = SystemColors.Control;

            foreach (Label lbl in panel1.Controls.OfType<Label>())
                lbl.ForeColor = AppTheme.TextOnPrimary;

            tabControl1.Font = AppTheme.FontTab;
            Detail.BackColor = SystemColors.Control;
            Transactions.BackColor = SystemColors.Control;

            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.Dock = DockStyle.Top;

            groupBox2.Font = AppTheme.FontGroupBox;
            groupBox3.Font = AppTheme.FontGroupBox;

            foreach (Button btn in new[] { btn_save, btn_update, btn_delete, btn_blank, btn_cancel, btn_refresh, btn_search, btn_payment, btn_trans_refresh, Btn_ledger_report, Btn_printCustomerReceipt })
            {
                if (btn == null) continue;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular);
                btn.Height = 34;
            }

            StyleLedgerGrid(grid_customer_transactions);
        }

        private static void StyleLedgerGrid(DataGridView grid)
        {
            if (grid == null) return;

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, grid, new object[] { true });

            grid.BackgroundColor = SystemColors.AppWorkspace;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = SystemColors.ControlLight;
            grid.RowHeadersVisible = false;
            grid.EnableHeadersVisualStyles = false;

            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 34;
            grid.RowTemplate.Height = 30;

            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Control,
                ForeColor = SystemColors.ControlText,
                Font = AppTheme.FontGridHeader,
                SelectionBackColor = SystemColors.Control,
                SelectionForeColor = SystemColors.ControlText,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(6, 4, 6, 4)
            };

            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Window,
                ForeColor = AppTheme.TextPrimary,
                Font = AppTheme.FontGrid,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText,
                Padding = new Padding(6, 2, 6, 2)
            };

            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = AppTheme.GridAltRow,
                ForeColor = AppTheme.TextPrimary,
                Font = AppTheme.FontGrid,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText
            };
        }

       
        
        private void GetCustomerCode()
        {
            // Customer code should not be edited for existing customers
            //txt_customer_code.ReadOnly = !string.IsNullOrWhiteSpace(txt_id.Text);

            // Auto-generate customer code for new customers

            try
            {
                CustomerBLL bll = new CustomerBLL();
                txt_customer_code.Text = bll.GetNextCustomerCode();
            }
            catch
            {
                // ignore auto-code errors; user can still save and DLL will generate if empty
            }
            
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
                txt_cr_number.Text = myProductView["cr_number"].ToString();
                if (dt.Columns.Contains("customer_code"))
                    txt_customer_code.Text = myProductView["customer_code"].ToString();
            }
            lbl_customer_name.Visible = true;
            lbl_customer_name.Text = txt_first_name.Text + ' ' + txt_last_name.Text;

            //txt_customer_code.ReadOnly = true;
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
                    UiMessages.ShowWarning(
                        "You do not have permission to create customers.",
                        "ليس لديك صلاحية لإنشاء العملاء.",
                        "Permission Denied",
                        "تم رفض الصلاحية"
                    );
                    return;
                }

                // Validate required fields
                if (txt_first_name.Text == string.Empty)
                {
                    UiMessages.ShowInfo(
                        "First name is required.",
                        "الاسم الأول مطلوب.",
                        "Validation",
                        "تحقق"
                    );
                    txt_first_name.Focus();
                    return;
                }
                if (txt_registrationName.Text == string.Empty)
                {
                    UiMessages.ShowInfo(
                        "Registration name is required.",
                        "اسم التسجيل مطلوب.",
                        "Validation",
                        "تحقق"
                    );
                    txt_registrationName.Focus();
                    return;
                }
                var customerBLL = new CustomerBLL();
                if (customerBLL.IsCustomerCodeExists(txt_customer_code.Text.Trim()))
                {
                    UiMessages.ShowWarning(
                        "Customer code already exists.",
                        "رمز العميل موجود بالفعل.",
                        "Validation",
                        "تحقق"
                    );
                    txt_customer_code.Focus();
                    return;
                }
                var confirm = UiMessages.ConfirmYesNo(
                    "Save this customer?",
                    "هل تريد حفظ هذا العميل؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );
                if (confirm != DialogResult.Yes)
                    return;

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
                        GLAccountID = Convert.ToInt32(cmb_GL_account_code.SelectedValue),
                        CRNumber = txt_cr_number.Text.Trim(),
                        customer_code = txt_customer_code.Text.Trim(),
                    };

                    CustomerBLL objBLL = new CustomerBLL();
                    int result = objBLL.Insert(info);
                    if (result > 0)
                    {
                        UiMessages.ShowInfo(
                            "Customer has been created successfully.",
                            "تم إنشاء العميل بنجاح.",
                            "Success",
                            "نجاح"
                        );
                        clear_all();
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "Customer could not be saved. Please try again.",
                            "تعذر حفظ العميل. يرجى المحاولة مرة أخرى.",
                            "Error",
                            "خطأ"
                        );
                    }

                }
                else
                {
                    UiMessages.ShowInfo(
                        "Please enter value in field",
                        "يرجى إدخال قيمة في الحقل",
                        "Invalid Data",
                        "بيانات غير صالحة"
                    );
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
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
            if(e.KeyData == Keys.F9)
            {
                txt_search.Focus();
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
            txt_countryName.Text = "SA";
            txt_registrationName.Text = "";

            cmb_GL_account_code.SelectedValue = "5"; // 5 is the default Ac receiavable Account id in acc_accounts table
            txt_cr_number.Text = "";
            GetCustomerCode();
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
                    UiMessages.ShowWarning(
                        "You do not have permission to update customers.",
                        "ليس لديك صلاحية لتحديث العملاء.",
                        "Permission Denied",
                        "تم رفض الصلاحية"
                    );
                    return;
                }

                if (String.IsNullOrEmpty(txt_id.Text))
                {
                    UiMessages.ShowError(
                        "Please select a customer record to update.",
                        "يرجى اختيار سجل عميل للتحديث.",
                        "Not Found",
                        "غير موجود"
                    );
                    return;
                }

                var customerBLL = new CustomerBLL();
                if(customerBLL.IsCustomerCodeExists(txt_customer_code.Text.Trim(), int.Parse(txt_id.Text)))
                {
                    UiMessages.ShowWarning(
                        "Customer code already exists.",
                        "رمز العميل موجود بالفعل.",
                        "Validation",
                        "تحقق"
                    );
                    txt_customer_code.Focus();
                    return;
                }

                if (txt_first_name.Text != string.Empty && txt_registrationName.Text != string.Empty && txt_vatno.Text != string.Empty)
                {
                    var confirmUpdate = UiMessages.ConfirmYesNo(
                        "Update this customer?",
                        "هل تريد تحديث هذا العميل؟",
                        captionEn: "Confirm",
                        captionAr: "تأكيد"
                    );
                    if (confirmUpdate != DialogResult.Yes)
                        return;

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
                        GLAccountID = Convert.ToInt32(cmb_GL_account_code.SelectedValue),
                        CRNumber = txt_cr_number.Text.Trim(),
                        customer_code = txt_customer_code.Text.Trim(),
                    };

                    CustomerBLL objBLL = new CustomerBLL();
                    info.id = int.Parse(txt_id.Text);

                    int result = objBLL.Update(info);
                    if (result > 0)
                    {
                        UiMessages.ShowInfo(
                            "Customer has been updated successfully.",
                            "تم تحديث العميل بنجاح.",
                            "Success",
                            "نجاح"
                        );
                        clear_all();
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "Customer could not be updated. Please try again.",
                            "تعذر تحديث العميل. يرجى المحاولة مرة أخرى.",
                            "Error",
                            "خطأ"
                        );
                    }

                }
                else
                {
                    UiMessages.ShowInfo(
                        "Customer name, registration no. and vat no. are requried",
                        "يرجى إدخال الاسم الأول",
                        "Required Field",
                        "حقل مطلوب"
                    );
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
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
                    UiMessages.ShowWarning(
                        "You do not have permission to delete customers.",
                        "ليس لديك صلاحية لحذف العملاء.",
                        "Permission Denied",
                        "تم رفض الصلاحية"
                    );
                    return;
                }

                string id = txt_id.Text;
                if (string.IsNullOrWhiteSpace(id))
                {
                    UiMessages.ShowInfo(
                        "Please select a customer record to delete.",
                        "يرجى اختيار سجل عميل للحذف.",
                        "Delete",
                        "حذف"
                    );
                    return;
                }

                var confirmDelete = UiMessages.ConfirmYesNo(
                    "Delete this customer? This action cannot be undone.",
                    "هل تريد حذف هذا العميل؟ لا يمكن التراجع عن هذا الإجراء.",
                    captionEn: "Confirm Delete",
                    captionAr: "تأكيد الحذف"
                );

                if (confirmDelete != DialogResult.Yes)
                    return;

                CustomerBLL objBLL = new CustomerBLL();
                objBLL.Delete(int.Parse(id));

                UiMessages.ShowInfo(
                    "Customer has been deleted successfully.",
                    "تم حذف العميل بنجاح.",
                    "Deleted",
                    "تم الحذف"
                );
                clear_all();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"An error occurred: {ex.Message}",
                    $"حدث خطأ: {ex.Message}",
                    "Error",
                    "خطأ"
                );
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            string customer_id = txt_id.Text;

            if (!string.IsNullOrWhiteSpace(customer_id))
            {
                using (pos.UI.Busy.BusyScope.Show(this, UiMessages.T("Loading customer details...", "جاري تحميل بيانات العميل...")))
                {
                    load_customer_detail(int.Parse(customer_id));
                }
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a customer first.",
                    "يرجى اختيار عميل أولاً.",
                    "Customer",
                    "العميل"
                );
            }
        }

        private void btn_trans_refresh_Click(object sender, EventArgs e)
        {
            string customer_id = txt_id.Text;
            if (customer_id != "")
            {
                // Permission check
                if (!_auth.HasPermission(_currentUser, Permissions.Customers_LedgerView))
                {
                    UiMessages.ShowWarning(
                        "You do not have permission to view the customer ledger.",
                        "ليس لديك صلاحية لعرض كشف حساب العميل.",
                        "Permission Denied",
                        "تم رفض الصلاحية"
                    );
                    return;
                }

                using (pos.UI.Busy.BusyScope.Show(this, UiMessages.T("Loading customer transactions...", "جاري تحميل حركات العميل...")))
                {
                    load_customer_transactions_grid(int.Parse(customer_id));
                }
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
                    UiMessages.ShowWarning(
                        "You do not have permission to record payments for this customer.",
                        "ليس لديك صلاحية لتسجيل دفعات لهذا العميل.",
                        "Permission Denied",
                        "تم رفض الصلاحية"
                    );
                    return;
                }
                frm_customer_payment obj = new frm_customer_payment(this, int.Parse(customer_id));
                obj.ShowDialog();

            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a customer first.",
                    "يرجى اختيار عميل أولاً.",
                    "Customer",
                    "العميل"
                );
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
                style.BackColor = Color.FromArgb(245, 246, 248);
                style.ForeColor = Color.FromArgb(0, 0, 0);

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
                UiMessages.ShowError(
                    "Failed to load customer transactions. " + ex.Message,
                    "تعذر تحميل حركات العميل. " + ex.Message,
                    "Error",
                    "خطأ"
                );
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
                    UiMessages.ShowWarning(
                        "You do not have permission to print the customer ledger.",
                        "ليس لديك صلاحية لطباعة كشف حساب العميل.",
                        "Permission Denied",
                        "تم رفض الصلاحية"
                    );
                    return;
                }
                if (String.IsNullOrEmpty(txt_id.Text))
                {
                    UiMessages.ShowInfo(
                        "Please select a customer to view the ledger report.",
                        "يرجى اختيار عميل لعرض تقرير كشف الحساب.",
                        "Customer",
                        "العميل"
                    );
                    return;
                }
                string customer_id = txt_id.Text;
                pos.Customers.Customer_Ledger_Report.FrmCustomerLedgerReport obj = new Customers.Customer_Ledger_Report.FrmCustomerLedgerReport(customer_id);
                obj.ShowDialog();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void Btn_printCustomerReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_customer_transactions.CurrentRow == null)
                {
                    UiMessages.ShowInfo(
                        "Please select a transaction to print the receipt.",
                        "يرجى اختيار حركة لطباعة الإيصال.",
                        "Receipt",
                        "الإيصال"
                    );
                    return;
                }

                string id = grid_customer_transactions.CurrentRow.Cells["id"].Value.ToString();

                if (String.IsNullOrEmpty(id))
                {
                    UiMessages.ShowInfo(
                        "Please select a transaction to print the receipt.",
                        "يرجى اختيار حركة لطباعة الإيصال.",
                        "Receipt",
                        "الإيصال"
                    );
                    return;
                }
                pos.Customers.Customer_Ledger_Report.Frm_customerPaymentReceipt frm_CustomerPaymentReceipt = new Customers.Customer_Ledger_Report.Frm_customerPaymentReceipt(id);
                frm_CustomerPaymentReceipt.ShowDialog();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }
    }
}
