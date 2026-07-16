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
using pos.Suppliers.Supplier_Ledger_Report;
using pos.Security.Authorization;
using POS.BLL;
using POS.Core;
using POS.DLL;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_addSupplier : Form
    {
        private const string PaymentReferencePrefix = "[Payment Ref: ";

        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        // Account IDs and invoice tracking for opening balance journal entries
        public int cash_account_id = 0;
        public int sales_account_id = 0;
        public int receivable_account_id = 0;
        public int payable_account_id = 0;
        public int sales_discount_acc_id = 0;
        public int opening_balance_equity_acc_id = 0;
        private string _invoice_no = string.Empty;

        public static frm_addSupplier instance;
        public TextBox tb_id;
        public TextBox tb_first_name;
        public TextBox tb_last_name;
        public TextBox tb_address;
        public TextBox tb_vat_no;
        public TextBox tb_contact_no;
        public TextBox tb_email;
        public CheckBox vat_with_status;
        public Label tb_lbl_is_edit;
        private frm_suppliers mainForm;

        public frm_addSupplier(frm_suppliers mainForm) : this()
        {
            this.mainForm = mainForm;
        }

        public frm_addSupplier()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_first_name = txt_first_name;
            tb_last_name = txt_last_name;
            tb_address = txt_address;
            tb_vat_no = txt_vatno;
            tb_contact_no = txt_contact_no;
            tb_email = txt_email;
            tb_lbl_is_edit = lbl_edit_status;
            vat_with_status = chk_vat_status;
            btn_transDelete.Enabled = false;

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

        public void frm_addSupplier_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleSupplierForm();

            txt_search.Focus();
            this.ActiveControl = txt_search;
            Get_AccountID_From_Company();
            GetSupplierCode();
            get_accounts_dropdownlist();
            GetMAXInvoiceNo();
        }

        private void StyleSupplierForm()
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

            lbl_customer_name.ForeColor = AppTheme.TextOnPrimary;
            label21.ForeColor = AppTheme.TextOnPrimary;

            foreach (Label lbl in panel1.Controls.OfType<Label>())
                lbl.ForeColor = AppTheme.TextOnPrimary;

            tabControl1.Font = AppTheme.FontTab;
            tabPage1.BackColor = SystemColors.Control;
            tabPage2.BackColor = SystemColors.Control;

            //tableLayoutPanel1.AutoSize = true;
            //tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //tableLayoutPanel1.Dock = DockStyle.Top;

            groupBox1.Font = AppTheme.FontGroupBox;
            groupBox3.Font = AppTheme.FontGroupBox;
            groupBox2.Font = AppTheme.FontGroupBox;

            StyleLedgerGrid(grid_supplier_transactions);
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
        private void GetSupplierCode()
        {
            try
            {
                SupplierBLL objBLL = new SupplierBLL();
                string newCode = objBLL.getNextSupplierCode();
                txt_supplier_code.Text = newCode;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Failed to generate supplier code. " + ex.Message,
                    "تعذر إنشاء رمز المورد. " + ex.Message,
                    "Error",
                    "خطأ"
                );
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

            cmb_GL_account_code.SelectedValue = "6"; // 6 is the default Ac payable Account id in acc_accounts table

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

        private void Get_AccountID_From_Company()
        {
            GeneralBLL objBLL = new GeneralBLL();
            String keyword = "TOP 1 *";
            String table = "pos_companies";
            DataTable companies_dt = objBLL.GetRecord(keyword, table);
            foreach (DataRow dr in companies_dt.Rows)
            {
                cash_account_id = (int)dr["cash_acc_id"];
                sales_account_id = (int)dr["sales_acc_id"];
                receivable_account_id = (int)dr["receivable_acc_id"];
                sales_discount_acc_id = (int)dr["sales_discount_acc_id"];

                // Load payable account
                try
                {
                    if (companies_dt.Columns.Contains("payable_acc_id") && dr["payable_acc_id"] != DBNull.Value)
                        payable_account_id = Convert.ToInt32(dr["payable_acc_id"]);
                }
                catch
                {
                    payable_account_id = 0;
                }

                // Load opening balance equity account if available
                try
                {
                    if (companies_dt.Columns.Contains("opening_balance_equity_acc_id") && dr["opening_balance_equity_acc_id"] != DBNull.Value)
                        opening_balance_equity_acc_id = Convert.ToInt32(dr["opening_balance_equity_acc_id"]);
                    else
                        opening_balance_equity_acc_id = GetOpeningBalanceEquityAccountId();
                }
                catch
                {
                    opening_balance_equity_acc_id = GetOpeningBalanceEquityAccountId();
                }
            }
        }

        private int GetOpeningBalanceEquityAccountId()
        {
            try
            {
                GeneralBLL objBLL = new GeneralBLL();
                DataTable dt = objBLL.GetRecord("id, name", "acc_accounts");
                foreach (DataRow row in dt.Rows)
                {
                    string accountName = row["name"].ToString().Trim().ToLower();
                    if (accountName.Contains("opening balance") ||
                        accountName.Contains("retained earnings") ||
                        accountName.Contains("equity") ||
                        accountName == "owner's equity" ||
                        accountName == "capital")
                    {
                        return Convert.ToInt32(row["id"]);
                    }
                }
                // If no equity account found, use cash account as fallback
                return cash_account_id;
            }
            catch
            {
                return 0;
            }
        }

        private void GetMAXInvoiceNo()
        {
            JournalsBLL journalsBLL_obj = new JournalsBLL();
            _invoice_no = journalsBLL_obj.GetMaxInvoiceNo();
        }

        private int Insert_Journal_entry(string invoice_no, int account_id, double debit, double credit, DateTime date,
            string description, int customer_id, int supplier_id, int entry_id, int bank_id, string payment_ref_invoice_no = "")
        {
            int journal_id = 0;
            JournalsModal journalsModal_obj = new JournalsModal();
            JournalsBLL journalsObj = new JournalsBLL();

            journalsModal_obj.invoice_no = invoice_no;
            journalsModal_obj.entry_date = date;
            journalsModal_obj.debit = debit;
            journalsModal_obj.credit = credit;
            journalsModal_obj.account_id = account_id;
            journalsModal_obj.description = description;
            journalsModal_obj.customer_id = customer_id;
            journalsModal_obj.supplier_id = supplier_id;
            journalsModal_obj.bank_id = bank_id;
            journalsModal_obj.entry_id = entry_id;
            journalsModal_obj.payment_ref_invoice_no = payment_ref_invoice_no;

            journal_id = journalsObj.Insert(journalsModal_obj);
            return journal_id;
        }


        private void btn_save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_first_name.Text))
            {
                UiMessages.ShowInfo(
                    "Supplier name is required name.",
                    "اسم المورد مطلوب (الاسم الأول واسم العائلة).",
                    "Validation",
                    "التحقق"
                );
                return;
            }
            var supplierBLL = new SupplierBLL();
            if (supplierBLL.IsSupplierCodeExists(txt_supplier_code.Text.Trim()))
            {
                UiMessages.ShowWarning(
                    "Supplier code already exists.",
                    "رمز المورد موجود بالفعل.",
                    "Validation",
                    "تحقق"
                );
                txt_supplier_code.Focus();
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Save this supplier?",
                "هل تريد حفظ هذا المورد؟",
                captionEn: "Confirm",
                captionAr: "تأكيد"
            );
            if (confirm != DialogResult.Yes) return;

            try
            {
                decimal openingBalance = 0m;
                if (!string.IsNullOrWhiteSpace(txt_opening_balance.Text) && !decimal.TryParse(txt_opening_balance.Text, out openingBalance))
                {
                    UiMessages.ShowInfo(
                        "Please enter a valid opening balance amount.",
                        "يرجى إدخال مبلغ رصيد افتتاحي صحيح.",
                        "Validation",
                        "التحقق"
                    );
                    txt_opening_balance.Focus();
                    return;
                }

                SupplierModal info = new SupplierModal();
                info.first_name = txt_first_name.Text;
                info.last_name = txt_last_name.Text;
                info.email = txt_email.Text;
                info.vat_no = txt_vatno.Text;
                info.address = txt_address.Text;
                info.contact_no = txt_contact_no.Text;
                info.vat_with_status = chk_vat_status.Checked;
                info.StreetName = txt_StreetName.Text.Trim();
                info.CityName = txt_cityName.Text.Trim();
                info.BuildingNumber = txt_buildingNumber.Text.Trim();
                info.CitySubdivisionName = txt_citySubdivisionName.Text.Trim();
                info.PostalCode = txt_postalCode.Text.Trim();
                info.CountryName = txt_countryName.Text.Trim();
                info.GLAccountID = int.Parse(cmb_GL_account_code.SelectedValue.ToString());
                info.supplier_code = txt_supplier_code.Text.Trim();
                info.opening_balance = Math.Round(openingBalance, 2);

                SupplierBLL objBLL = new SupplierBLL();
                int result = objBLL.Insert(info);
                if (result > 0)
                {
                    // Post opening balance journal entries if opening balance is not zero
                    if (openingBalance != 0)
                    {
                        try
                        {
                            PostOpeningBalanceJournalEntries(result, openingBalance, info.supplier_code, ((info.first_name ?? "") + " " + (info.last_name ?? "")).Trim());
                        }
                        catch (Exception journalEx)
                        {
                            UiMessages.ShowWarning(
                                "Supplier created but opening balance journal entry failed: " + journalEx.Message,
                                "تم إنشاء المورد ولكن فشل قيد الرصيد الافتتاحي: " + journalEx.Message,
                                "Warning",
                                "تحذير"
                            );
                        }
                    }

                    Log.LogAction(
                        "Create Supplier",
                        "SupplierId=" + result
                        + " | Code=" + info.supplier_code
                        + " | Name=" + ((info.first_name ?? "") + " " + (info.last_name ?? "")).Trim()
                        + " | VAT=" + (info.vat_no ?? "")
                        + " | Contact=" + (info.contact_no ?? "")
                        + " | GLAccountId=" + info.GLAccountID
                        + " | OpeningBalance=" + openingBalance.ToString("N2"),
                        UsersModal.logged_in_userid,
                        UsersModal.logged_in_branch_id);

                    UiMessages.ShowInfo(
                        "Supplier has been created successfully.",
                        "تم إنشاء المورد بنجاح.",
                        "Success",
                        "نجاح"
                    );
                    clear_all();

                    if (mainForm != null)
                        mainForm.load_Suppliers_grid();
                }
                else
                {
                    UiMessages.ShowError(
                        "Supplier could not be saved. Please try again.",
                        "تعذر حفظ المورد. يرجى المحاولة مرة أخرى.",
                        "Error",
                        "خطأ"
                    );
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //this.Dispose(); 
            this.Close();
        }

        private void frm_addSupplier_KeyDown(object sender, KeyEventArgs e)
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
            if (e.KeyData == Keys.F9)
            {
                txt_search.Focus();
            }

        }

        private void btn_blank_Click(object sender, EventArgs e)
        {
            clear_all();
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
            chk_vat_status.Checked = false;
            lbl_customer_name.Text = "";
            grid_supplier_transactions.DataSource = null;

            txt_StreetName.Text = "";
            txt_cityName.Text = "";
            txt_buildingNumber.Text = "";
            txt_citySubdivisionName.Text = "";
            txt_postalCode.Text = "";
            txt_countryName.Text = "SA";

            cmb_GL_account_code.SelectedValue = 6;
            txt_supplier_code.Text = "";
            txt_opening_balance.Text = "0.00";
            btn_transDelete.Enabled = false;
            GetSupplierCode();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txt_id.Text))
            {
                UiMessages.ShowInfo(
                    "Please select a supplier record to update.",
                    "يرجى اختيار سجل مورد للتحديث.",
                    "Not Found",
                    "غير موجود"
                );
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_first_name.Text))
            {
                UiMessages.ShowInfo(
                    "Supplier name is required name.",
                    "اسم المورد مطلوب.",
                    "Validation",
                    "التحقق"
                );
                return;
            }
            var supplierBLL = new SupplierBLL();
            if (supplierBLL.IsSupplierCodeExists(txt_supplier_code.Text.Trim(), int.Parse(txt_id.Text)))
            {
                UiMessages.ShowWarning(
                    "Supplier code already exists.",
                    "رمز المورد موجود بالفعل.",
                    "Validation",
                    "تحقق"
                );
                txt_supplier_code.Focus();
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Update this supplier?",
                "هل تريد تحديث هذا المورد؟",
                captionEn: "Confirm",
                captionAr: "تأكيد"
            );
            if (confirm != DialogResult.Yes) return;

            try
            {
                decimal openingBalance = 0m;
                if (!string.IsNullOrWhiteSpace(txt_opening_balance.Text) && !decimal.TryParse(txt_opening_balance.Text, out openingBalance))
                {
                    UiMessages.ShowInfo(
                        "Please enter a valid opening balance amount.",
                        "يرجى إدخال مبلغ رصيد افتتاحي صحيح.",
                        "Validation",
                        "التحقق"
                    );
                    txt_opening_balance.Focus();
                    return;
                }

                SupplierModal info = new SupplierModal();
                info.first_name = txt_first_name.Text;
                info.last_name = txt_last_name.Text;
                info.email = txt_email.Text;
                info.vat_no = txt_vatno.Text;
                info.address = txt_address.Text;
                info.contact_no = txt_contact_no.Text;
                info.vat_with_status = chk_vat_status.Checked;
                info.StreetName = txt_StreetName.Text.Trim();
                info.CityName = txt_cityName.Text.Trim();
                info.BuildingNumber = txt_buildingNumber.Text.Trim();
                info.CitySubdivisionName = txt_citySubdivisionName.Text.Trim();
                info.PostalCode = txt_postalCode.Text.Trim();
                info.CountryName = txt_countryName.Text.Trim();
                info.GLAccountID = int.Parse(cmb_GL_account_code.SelectedValue.ToString());
                info.id = int.Parse(txt_id.Text);
                info.supplier_code = txt_supplier_code.Text.Trim();
                info.opening_balance = Math.Round(openingBalance, 2);

                SupplierBLL objBLL = new SupplierBLL();

                // Get previous opening balance to check if it changed
                decimal previousOpeningBalance = objBLL.GetSupplierOpeningBalance(info.id);

                int result = objBLL.Update(info);
                if (result > 0)
                {
                    // Handle opening balance change
                    if (openingBalance != previousOpeningBalance)
                    {
                        try
                        {
                            decimal balanceChange = openingBalance - previousOpeningBalance;
                            PostOpeningBalanceAdjustmentJournalEntries(info.id, balanceChange, info.supplier_code, ((info.first_name ?? "") + " " + (info.last_name ?? "")).Trim());
                        }
                        catch (Exception journalEx)
                        {
                            UiMessages.ShowWarning(
                                "Supplier updated but opening balance adjustment journal entry failed: " + journalEx.Message,
                                "تم تحديث المورد ولكن فشل قيد تعديل الرصيد الافتتاحي: " + journalEx.Message,
                                "Warning",
                                "تحذير"
                            );
                        }
                    }

                    Log.LogAction(
                        "Update Supplier",
                        "SupplierId=" + info.id
                        + " | Code=" + info.supplier_code
                        + " | Name=" + ((info.first_name ?? "") + " " + (info.last_name ?? "")).Trim()
                        + " | VAT=" + (info.vat_no ?? "")
                        + " | Contact=" + (info.contact_no ?? "")
                        + " | GLAccountId=" + info.GLAccountID
                        + " | OpeningBalance=" + openingBalance.ToString("N2")
                        + " | PreviousOpeningBalance=" + previousOpeningBalance.ToString("N2"),
                        UsersModal.logged_in_userid,
                        UsersModal.logged_in_branch_id);

                    UiMessages.ShowInfo(
                        "Supplier has been updated successfully.",
                        "تم تحديث المورد بنجاح.",
                        "Success",
                        "نجاح"
                    );
                    clear_all();

                    if (mainForm != null)
                        mainForm.load_Suppliers_grid();
                }
                else
                {
                    UiMessages.ShowError(
                        "Supplier could not be updated. Please try again.",
                        "تعذر تحديث المورد. يرجى المحاولة مرة أخرى.",
                        "Error",
                        "خطأ"
                    );
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = txt_id.Text;

            if (string.IsNullOrWhiteSpace(id))
            {
                UiMessages.ShowInfo(
                    "Please select a supplier record to delete.",
                    "يرجى اختيار سجل مورد للحذف.",
                    "Delete",
                    "حذف"
                );
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this supplier? This action cannot be undone.",
                "هل تريد حذف هذا المورد؟ لا يمكن التراجع عن هذا الإجراء.",
                captionEn: "Confirm Delete",
                captionAr: "تأكيد الحذف"
            );
            if (confirm != DialogResult.Yes) return;

            try
            {
                SupplierBLL objBLL = new SupplierBLL();
                objBLL.Delete(int.Parse(id));

                Log.LogAction(
                    "Delete Supplier",
                    "SupplierId=" + id
                    + " | Code=" + (txt_supplier_code.Text ?? string.Empty).Trim()
                    + " | Name=" + ((txt_first_name.Text ?? "") + " " + (txt_last_name.Text ?? "")).Trim()
                    + " | VAT=" + (txt_vatno.Text ?? string.Empty).Trim(),
                    UsersModal.logged_in_userid,
                    UsersModal.logged_in_branch_id);

                UiMessages.ShowInfo(
                    "Supplier has been deleted successfully.",
                    "تم حذف المورد بنجاح.",
                    "Deleted",
                    "تم الحذف"
                );
                clear_all();

                if (mainForm != null)
                    mainForm.load_Suppliers_grid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Failed to delete supplier. " + ex.Message,
                    "تعذر حذف المورد. " + ex.Message,
                    "Error",
                    "خطأ"
                );
            }
        }

        public void load_transactions_grid(int supplier_id)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading supplier transactions...", "جاري تحميل حركات المورد...")))
                {
                    grid_supplier_transactions.DataSource = null;

                    //bind data in data grid view  
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_supplier_transactions.AutoGenerateColumns = false;

                    String keyword = "id,invoice_no,debit,credit,(credit-debit) AS balance,description,entry_date,account_id,account_name";
                    String table = "pos_suppliers_payments WHERE supplier_id = " + supplier_id + "";

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
                    newRow[4] = (_cr_total - _dr_total);
                    dt.Rows.InsertAt(newRow, dt.Rows.Count);

                    grid_supplier_transactions.DataSource = dt;
                    CustomizeDataGridView();
                    UpdateDeleteTransactionButtonState();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Failed to load supplier transactions. " + ex.Message,
                    "تعذر تحميل حركات المورد. " + ex.Message,
                    "Error",
                    "خطأ"
                );
                throw;
            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            string supplier_id = txt_id.Text;

            if (!string.IsNullOrWhiteSpace(supplier_id))
            {
                using (BusyScope.Show(this, UiMessages.T("Loading supplier details...", "جاري تحميل بيانات المورد...")))
                {
                    load_detail(int.Parse(supplier_id));
                }

            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a supplier first.",
                    "يرجى اختيار مورد أولاً.",
                    "Supplier",
                    "المورد"
                );
            }
        }

        private void btn_trans_refresh_Click(object sender, EventArgs e)
        {
            string supplier_id = txt_id.Text;
            if (!string.IsNullOrWhiteSpace(supplier_id))
            {
                load_transactions_grid(int.Parse(supplier_id));
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a supplier first.",
                    "يرجى اختيار مورد أولاً.",
                    "Supplier",
                    "المورد"
                );
            }
        }

        private void btn_payment_Click(object sender, EventArgs e)
        {
            string supplier_id = txt_id.Text;
            string supplier_name = ((txt_first_name.Text ?? "") + " " + (txt_last_name.Text ?? "")).Trim();
            if (!string.IsNullOrWhiteSpace(supplier_id))
            {
                frm_supplier_payment obj = new frm_supplier_payment(this, int.Parse(supplier_id), supplier_name);
                obj.ShowDialog();
                CustomizeDataGridView();
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a supplier first.",
                    "يرجى اختيار مورد أولاً.",
                    "Supplier",
                    "المورد"
                );
            }
        }

        private void Btn_printPaymentReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_supplier_transactions.SelectedRows.Count == 0)
                {
                    UiMessages.ShowInfo(
                        "Please select a payment record to print.",
                        "يرجى اختيار سجل دفعة للطباعة.",
                        "Receipt",
                        "الإيصال"
                    );
                    return;
                }

                string payment_id = grid_supplier_transactions.SelectedRows[0].Cells["id"].Value.ToString();
                if (string.IsNullOrEmpty(payment_id))
                {
                    UiMessages.ShowError(
                        "The selected payment record is not valid.",
                        "سجل الدفعة المحدد غير صالح.",
                        "Error",
                        "خطأ"
                    );
                    return;
                }

                Frm_SupplierPaymentReceipt reportForm = new Frm_SupplierPaymentReceipt(payment_id);
                reportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_transDelete_Click(object sender, EventArgs e)
        {
            if (!_auth.HasPermission(_currentUser, Permissions.Suppliers_Delete))
            {
                UiMessages.ShowWarning(
                    "You do not have permission to delete supplier payment transactions.",
                    "ليس لديك صلاحية لحذف حركات دفعات الموردين.",
                    "Permission Denied",
                    "تم رفض الصلاحية"
                );
                return;
            }

            if (grid_supplier_transactions.SelectedRows.Count == 0)
            {
                UiMessages.ShowInfo(
                    "Please select a payment transaction to delete.",
                    "يرجى اختيار حركة دفعة للحذف.",
                    "Delete Transaction",
                    "حذف الحركة"
                );
                return;
            }

            int supplierId;
            if (!int.TryParse(txt_id.Text, out supplierId) || supplierId <= 0)
            {
                UiMessages.ShowInfo(
                    "Please select a supplier first.",
                    "يرجى اختيار مورد أولاً.",
                    "Supplier",
                    "المورد"
                );
                return;
            }

            string paymentInvoiceNo = Convert.ToString(grid_supplier_transactions.SelectedRows[0].Cells["invoice_no"].Value);
            string paymentDescription = Convert.ToString(grid_supplier_transactions.SelectedRows[0].Cells["description"].Value);

            if (string.IsNullOrWhiteSpace(paymentInvoiceNo))
            {
                UiMessages.ShowWarning(
                    "Please select a valid payment transaction row.",
                    "يرجى اختيار صف دفعة صالح.",
                    "Delete Transaction",
                    "حذف الحركة"
                );
                return;
            }

            string paymentReferenceInvoiceNo = IsJournalTransactionInvoice(paymentInvoiceNo)
                ? paymentInvoiceNo.Trim()
                : ExtractPaymentReferenceInvoice(paymentDescription);

            if (string.IsNullOrWhiteSpace(paymentReferenceInvoiceNo))
            {
                UiMessages.ShowWarning(
                    "Only payment transactions linked to a journal reference can be deleted.",
                    "يمكن حذف حركات الدفعات المرتبطة بمرجع قيد يومية فقط.",
                    "Delete Transaction",
                    "حذف الحركة"
                );
                UpdateDeleteTransactionButtonState();
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this supplier payment transaction and all linked journal/bank entries?",
                "هل تريد حذف حركة دفعة المورد هذه وجميع قيود اليومية/البنك المرتبطة بها؟",
                captionEn: "Confirm Delete",
                captionAr: "تأكيد الحذف"
            );
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Deleting supplier payment transaction...", "جاري حذف حركة دفعة المورد...")))
                {
                    SupplierBLL objBLL = new SupplierBLL();
                    int result = objBLL.DeletePaymentTransaction(paymentReferenceInvoiceNo.Trim());

                    if (result > 0)
                    {
                        UiMessages.ShowInfo(
                            "Supplier payment transaction has been deleted successfully.",
                            "تم حذف حركة دفعة المورد بنجاح.",
                            "Deleted",
                            "تم الحذف"
                        );

                        Log.LogAction(
                            "Delete Supplier Payment Transaction",
                            "SupplierId=" + supplierId
                            + " | DisplayInvoice=" + paymentInvoiceNo
                            + " | JournalReference=" + paymentReferenceInvoiceNo
                            + " | Description=" + (paymentDescription ?? string.Empty),
                            UsersModal.logged_in_userid,
                            UsersModal.logged_in_branch_id);

                        load_transactions_grid(supplierId);

                        if (mainForm != null)
                            mainForm.load_Suppliers_grid();
                    }
                    else
                    {
                        UiMessages.ShowWarning(
                            "No linked records were found for the selected invoice number.",
                            "لم يتم العثور على سجلات مرتبطة برقم الفاتورة المحدد.",
                            "Delete Transaction",
                            "حذف الحركة"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Failed to delete supplier payment transaction. " + ex.Message,
                    "تعذر حذف حركة دفعة المورد. " + ex.Message,
                    "Error",
                    "خطأ"
                );
            }
        }

        private void grid_supplier_transactions_SelectionChanged(object sender, EventArgs e)
        {
            UpdateDeleteTransactionButtonState();
        }

        private void UpdateDeleteTransactionButtonState()
        {
            btn_transDelete.Enabled = false;

            if (!_auth.HasPermission(_currentUser, Permissions.Suppliers_Delete))
                return;

            if (grid_supplier_transactions == null || grid_supplier_transactions.SelectedRows.Count == 0)
                return;

            var selectedRow = grid_supplier_transactions.SelectedRows[0];
            if (selectedRow == null)
                return;

            string invoiceNo = Convert.ToString(selectedRow.Cells["invoice_no"].Value);
            string description = Convert.ToString(selectedRow.Cells["description"].Value);
            btn_transDelete.Enabled = IsJournalTransactionInvoice(invoiceNo)
                || !string.IsNullOrWhiteSpace(ExtractPaymentReferenceInvoice(description));
        }

        private static bool IsJournalTransactionInvoice(string invoiceNo)
        {
            return !string.IsNullOrWhiteSpace(invoiceNo)
                && invoiceNo.Trim().StartsWith("J", StringComparison.OrdinalIgnoreCase);
        }

        private static string ExtractPaymentReferenceInvoice(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return string.Empty;

            int startIndex = description.IndexOf(PaymentReferencePrefix, StringComparison.OrdinalIgnoreCase);
            if (startIndex < 0)
                return string.Empty;

            startIndex += PaymentReferencePrefix.Length;
            int endIndex = description.IndexOf(']', startIndex);
            if (endIndex <= startIndex)
                return string.Empty;

            return description.Substring(startIndex, endIndex - startIndex).Trim();
        }

        private void CustomizeDataGridView()
        {
            if (grid_supplier_transactions == null || grid_supplier_transactions.Rows.Count == 0)
                return;

            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_supplier_transactions.Rows[grid_supplier_transactions.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_supplier_transactions.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }

        private void Btn_ledger_report_Click(object sender, EventArgs e)
        {
            string supplier_id = txt_id.Text;
            if (!string.IsNullOrWhiteSpace(supplier_id))
            {
                using (BusyScope.Show(this, UiMessages.T("Opening ledger report...", "جاري فتح تقرير كشف الحساب...")))
                {
                    pos.Suppliers.Supplier_Ledger_Report.FrmSupplierLedgerReport obj = new Suppliers.Supplier_Ledger_Report.FrmSupplierLedgerReport(supplier_id);
                    obj.ShowDialog();
                }
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a supplier to view the ledger report.",
                    "يرجى اختيار مورد لعرض تقرير كشف الحساب.",
                    "Supplier",
                    "المورد"
                );
            }
        }

        public void load_detail(int supplier_id)
        {
            SupplierBLL objBLL = new SupplierBLL();
            DataTable dt = objBLL.SearchRecordBySupplierID(supplier_id);
            foreach (DataRow myProductView in dt.Rows)
            {
                txt_id.Text = myProductView["id"].ToString();
                txt_first_name.Text = myProductView["first_name"].ToString();
                txt_last_name.Text = myProductView["last_name"].ToString();
                txt_address.Text = myProductView["address"].ToString();
                txt_vatno.Text = myProductView["vat_no"].ToString();
                txt_contact_no.Text = myProductView["contact_no"].ToString();
                txt_email.Text = myProductView["email"].ToString();
                chk_vat_status.Checked = bool.TryParse(Convert.ToString(myProductView["vat_status"]), out var vat) && vat;
                txt_StreetName.Text = myProductView["StreetName"].ToString();
                txt_cityName.Text = myProductView["CityName"].ToString();
                txt_buildingNumber.Text = myProductView["BuildingNumber"].ToString();
                txt_citySubdivisionName.Text = myProductView["CitySubdivisionName"].ToString();
                txt_postalCode.Text = myProductView["PostalCode"].ToString();
                txt_countryName.Text = myProductView["CountryName"].ToString();
                cmb_GL_account_code.SelectedValue = (myProductView["GLAccountID"].ToString() == "" ? 0 : Convert.ToInt32(myProductView["GLAccountID"].ToString()));
                txt_supplier_code.Text = myProductView["supplier_code"].ToString();
            }

            decimal openingBalance = objBLL.GetSupplierOpeningBalance(supplier_id);
            txt_opening_balance.Text = openingBalance.ToString("N2");

            lbl_customer_name.Visible = true;
            lbl_customer_name.Text = txt_first_name.Text + ' ' + txt_last_name.Text;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            frm_search_suppliers search_obj = new frm_search_suppliers(this, txt_search.Text);
            search_obj.ShowDialog();
        }

        private void PostOpeningBalanceJournalEntries(int supplierId, decimal openingBalance, string supplierCode, string supplierName)
        {
            if (openingBalance == 0 || opening_balance_equity_acc_id == 0)
                return;

            GetMAXInvoiceNo();
            if (string.IsNullOrWhiteSpace(_invoice_no))
                throw new Exception("Failed to generate invoice number for opening balance entry.");

            DateTime entryDate = DateTime.Now.Date;
            bool isArabic = IsArabicEnvironment();
            string description = isArabic
                ? $"رصيد افتتاحي للمورد {supplierName} (كود: {supplierCode})"
                : $"Opening balance for supplier {supplierName} (Code: {supplierCode})";

            if (openingBalance > 0)
            {
                // We owe supplier money
                // Debit: Opening Balance Equity (decrease equity)
                int entry_id = Insert_Journal_entry(_invoice_no, opening_balance_equity_acc_id, (double)openingBalance, 0, entryDate, description, 0, 0, 0, 0);

                // Credit: Accounts Payable (increase liability)
                Insert_Journal_entry(_invoice_no, payable_account_id, 0, (double)openingBalance, entryDate, description, 0, 0, 0, 0);

                // Add entry into supplier ledger (credit supplier account)
                Insert_Journal_entry(_invoice_no, payable_account_id, 0, (double)openingBalance, entryDate, description, 0, supplierId, entry_id, 0, _invoice_no);
            }
            else if (openingBalance < 0)
            {
                // Supplier owes us money (debit balance)
                decimal positiveAmount = Math.Abs(openingBalance);

                // Debit: Accounts Payable (decrease liability)
                int entry_id = Insert_Journal_entry(_invoice_no, payable_account_id, (double)positiveAmount, 0, entryDate, description, 0, 0, 0, 0);

                // Credit: Opening Balance Equity (increase equity)
                Insert_Journal_entry(_invoice_no, opening_balance_equity_acc_id, 0, (double)positiveAmount, entryDate, description, 0, 0, 0, 0);

                // Add entry into supplier ledger (debit supplier account)
                Insert_Journal_entry(_invoice_no, opening_balance_equity_acc_id, (double)positiveAmount, 0, entryDate, description, 0, supplierId, entry_id, 0, _invoice_no);
            }
        }

        private void PostOpeningBalanceAdjustmentJournalEntries(int supplierId, decimal balanceChange, string supplierCode, string supplierName)
        {
            if (balanceChange == 0 || opening_balance_equity_acc_id == 0)
                return;

            GetMAXInvoiceNo();
            if (string.IsNullOrWhiteSpace(_invoice_no))
                throw new Exception("Failed to generate invoice number for opening balance adjustment.");

            DateTime entryDate = DateTime.Now.Date;
            bool isArabic = IsArabicEnvironment();
            string description = isArabic
                ? $"تعديل الرصيد الافتتاحي للمورد {supplierName} (كود: {supplierCode})"
                : $"Opening balance adjustment for supplier {supplierName} (Code: {supplierCode})";

            if (balanceChange > 0)
            {
                // Increase in opening balance (we owe more)
                // Debit: Opening Balance Equity
                int entry_id = Insert_Journal_entry(_invoice_no, opening_balance_equity_acc_id, (double)balanceChange, 0, entryDate, description, 0, 0, 0, 0);

                // Credit: Accounts Payable
                Insert_Journal_entry(_invoice_no, payable_account_id, 0, (double)balanceChange, entryDate, description, 0, 0, 0, 0);

                // Add entry into supplier ledger
                Insert_Journal_entry(_invoice_no, payable_account_id, 0, (double)balanceChange, entryDate, description, 0, supplierId, entry_id, 0, _invoice_no);
            }
            else if (balanceChange < 0)
            {
                // Decrease in opening balance
                decimal positiveAmount = Math.Abs(balanceChange);

                // Debit: Accounts Payable
                int entry_id = Insert_Journal_entry(_invoice_no, payable_account_id, (double)positiveAmount, 0, entryDate, description, 0, 0, 0, 0);

                // Credit: Opening Balance Equity
                Insert_Journal_entry(_invoice_no, opening_balance_equity_acc_id, 0, (double)positiveAmount, entryDate, description, 0, 0, 0, 0);

                // Add entry into supplier ledger
                Insert_Journal_entry(_invoice_no, opening_balance_equity_acc_id, (double)positiveAmount, 0, entryDate, description, 0, supplierId, entry_id, 0, _invoice_no);
            }
        }

        private static bool IsArabicEnvironment()
        {
            string lang = UsersModal.logged_in_lang ?? string.Empty;
            return lang.StartsWith("ar", StringComparison.OrdinalIgnoreCase);
        }
    }
}
