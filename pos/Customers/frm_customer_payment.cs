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
    public partial class frm_customer_payment : Form
    {
        private frm_addCustomer mainForm;
        public int _customer_id;
        public string _invoice_no;

        public int cash_account_id = 0;
        public int sales_account_id = 0;
        public int receivable_account_id = 0;
        public int sales_discount_acc_id = 0;

        // OPTIONAL: If you have a payable account id in company table, set it here.
        // If not available, keep 0 and supplier reference loading won't trigger.
        public int payable_account_id = 0;

        public frm_customer_payment(frm_addCustomer mainForm, int customer_id)
       {
            this.mainForm = mainForm;
            _customer_id = customer_id;
            
            InitializeComponent();
        }

        public frm_customer_payment()
        {
            InitializeComponent();
           
        }
        
        public void frm_customer_payment_Load(object sender, EventArgs e)
        {
            Get_AccountID_From_Company();
            get_accounts_dropdownlist();
            GetMAXInvoiceNo();
        }

        public void get_accounts_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_accounts where id IN (3,19)"; // Load only 3=Cash Account, 19=Bank Account

            DataTable accounts = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = accounts.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            accounts.Rows.InsertAt(emptyRow, 0);

            cmb_GL_account_code.DisplayMember = "name";
            cmb_GL_account_code.ValueMember = "id";
            cmb_GL_account_code.DataSource = accounts;

            cmb_GL_account_code.SelectedValue = "3"; // 3 is the default Cash Account id in acc_accounts table

        }

        public void GetMAXInvoiceNo()
        {
            JournalsBLL JournalsBLL_obj = new JournalsBLL();
            _invoice_no = JournalsBLL_obj.GetMaxInvoiceNo();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (_invoice_no != string.Empty && _customer_id != 0)
            {
                int GL_account_id = (cmb_GL_account_code.SelectedValue == null ? 0 : int.Parse(cmb_GL_account_code.SelectedValue.ToString()));
                int Ref_account_id = (cmb_ref_account_code.SelectedValue == null ? 0 : int.Parse(cmb_ref_account_code.SelectedValue.ToString()));

                ///CASH/BANK JOURNAL ENTRY (DEBIT)
                Insert_Journal_entry(_invoice_no, GL_account_id, Convert.ToDouble(txt_total_amount.Text), 0, txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0, 0);

                ///ACCOUNT RECEIVABLE JOURNAL ENTRY (CREDIT)
                int entry_id = Insert_Journal_entry(_invoice_no, receivable_account_id, 0, Convert.ToDouble(txt_total_amount.Text), txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0, 0);

                ///ADD ENTRY INTO customer PAYMENT(CREDIT)
                Insert_Journal_entry(_invoice_no, GL_account_id, 0, Convert.ToDouble(txt_total_amount.Text), txt_payment_date.Value.Date, txt_description.Text, _customer_id, 0, entry_id, 0);

                // Insert entry into reference account if selected
                if (Ref_account_id != 0)
                {
                    // check if reference account is receivable or payable or bank
                    BankBLL bankBLL = new BankBLL();
                    if (bankBLL.IsBankGlAccount(GL_account_id))
                    {
                        // Insert into reference account (debit) in bank payment
                        Insert_Journal_entry(_invoice_no, GL_account_id, Convert.ToDouble(txt_total_amount.Text),0, txt_payment_date.Value.Date, txt_description.Text, 0, 0, entry_id, Ref_account_id);
                    }
                    else {
                        // Default handling for other account types
                    }

                }

                ///IF DISCOUNT APPLIED
                if (txt_discount.Text != string.Empty)
                {
                    /// SALES DISCOUNT JOURNAL ENTRY (debit)
                    int entry_idd = Insert_Journal_entry(_invoice_no, sales_discount_acc_id, Convert.ToDouble(txt_discount.Text), 0, txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0, 0);
                    ///receivable JOURNAL ENTRY (credit)
                    Insert_Journal_entry(_invoice_no, receivable_account_id, 0, Convert.ToDouble(txt_discount.Text), txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0, 0);

                    
                    ///ADD ENTRY INTO customer PAYMENT(credit)
                    Insert_Journal_entry(_invoice_no, sales_discount_acc_id, 0, Convert.ToDouble(txt_discount.Text), txt_payment_date.Value.Date, txt_description.Text, _customer_id, 0, entry_idd, 0);

                }

               if (entry_id > 0)
                {
                    MessageBox.Show("Record created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

               mainForm.load_customer_transactions_grid(_customer_id);
                    
                this.Close();

            }
            else
            {
                MessageBox.Show("Please enter value in field", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
            
        }
        
        private int Insert_Journal_entry(string invoice_no, int account_id, double debit, double credit, DateTime date,
           string description, int customer_id, int supplier_id, int entry_id,int bank_id)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            JournalsBLL JournalsObj = new JournalsBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = debit;
            JournalsModal_obj.credit = credit;
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.description = description;
            JournalsModal_obj.customer_id = customer_id;
            JournalsModal_obj.supplier_id = supplier_id;
            JournalsModal_obj.bank_id = bank_id;
            JournalsModal_obj.entry_id = entry_id;

            journal_id = JournalsObj.Insert(JournalsModal_obj);
            return journal_id;
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

                // Defensive: if your `pos_companies` table has payable_acc_id, load it.
                // If not, ignore without failing.
                try
                {
                    if (companies_dt.Columns.Contains("payable_acc_id") && dr["payable_acc_id"] != DBNull.Value)
                        payable_account_id = Convert.ToInt32(dr["payable_acc_id"]);
                }
                catch
                {
                    payable_account_id = 0;
                }
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
            this.Close();
        }

        private void frm_customer_payment_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }


        private void txt_total_amount_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txt_discount_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cmb_cash_account_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Implement "reference account" loading based on selected GL account:
            // - Receivable GL -> Customers list
            // - Payable GL -> Suppliers list (if available)
            // - Bank GL -> Bank accounts for that GL account
            // - Otherwise -> clear ref combo

            int glAccountId = 0;
            try
            {
                glAccountId = (cmb_GL_account_code.SelectedValue == null)
                    ? 0
                    : Convert.ToInt32(cmb_GL_account_code.SelectedValue);
            }
            catch
            {
                glAccountId = 0;
            }

            if (glAccountId <= 0)
            {
                ClearRefCombo();
                return;
            }

            if (glAccountId == receivable_account_id)
            {
                LoadCustomersIntoRefCombo();
                return;
            }

            if (glAccountId == payable_account_id)
            {
                LoadSuppliersIntoRefCombo();
                return;
            }
            BankBLL bankBLL = new BankBLL();
            if (bankBLL.IsBankGlAccount(glAccountId))
            {
                LoadBankAccountsIntoRefCombo(glAccountId);
                return;
            }

            ClearRefCombo();
        }

        private void LoadCustomersIntoRefCombo()
        {
            // Expected table: `pos_customers` with columns: id, first_name, last_name (or name).
            // We build a simple display name using common columns, falling back to "id".
            try
            {
                GeneralBLL bll = new GeneralBLL();

                // Be pragmatic: request a set of columns that commonly exist.
                // If some columns don't exist, DB will error; we catch and fallback.
                DataTable dt;

                try
                {
                    dt = bll.GetRecord("TOP 500 id, first_name, last_name", "pos_customers");
                    if (!dt.Columns.Contains("display_name"))
                        dt.Columns.Add("display_name", typeof(string));

                    foreach (DataRow row in dt.Rows)
                    {
                        var fn = dt.Columns.Contains("first_name") && row["first_name"] != DBNull.Value ? row["first_name"].ToString() : "";
                        var ln = dt.Columns.Contains("last_name") && row["last_name"] != DBNull.Value ? row["last_name"].ToString() : "";
                        var name = (fn + " " + ln).Trim();
                        if (string.IsNullOrWhiteSpace(name))
                            name = "Customer #" + row["id"].ToString();

                        row["display_name"] = name;
                    }

                    BindCombo(cmb_ref_account_code, dt, "id", "display_name","customer");
                    return;
                }
                catch
                {
                    // Fallback: try a generic `name` column
                }

                dt = bll.GetRecord("TOP 500 id, name", "pos_customers");
                BindCombo(cmb_ref_account_code, dt, "id", "name","Customer");
            }
            catch
            {
                ClearRefCombo();
            }
        }

        private void LoadSuppliersIntoRefCombo()
        {
            // Expected table: `pos_suppliers` with columns: id, name (common).
            try
            {
                GeneralBLL bll = new GeneralBLL();
                DataTable dt = bll.GetRecord("TOP 500 id, CONCAT(first_name,' ',last_name) as name", "pos_suppliers");
                BindCombo(cmb_ref_account_code, dt, "id", "name","Supplier");
            }
            catch
            {
                ClearRefCombo();
            }
        }

        private void LoadBankAccountsIntoRefCombo(int glAccountId)
        {
            // Expected table: `pos_bank_accounts` with columns: id, account_title (or name), gl_account_id.
            // The project's `GeneralBLL.GetRecord` API doesn't support WHERE in provided usage,
            // so we request a subset of columns and then filter in-memory.
            try
            {
                GeneralBLL bll = new GeneralBLL();

                DataTable dt;
                try
                {
                    dt = bll.GetRecord("TOP 500 id, name, GLAccountID", "pos_banks");
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = "GLAccountID = " + glAccountId;
                    BindCombo(cmb_ref_account_code, dv.ToTable(), "id", "name");
                    return;
                }
                catch
                {
                    // Fallback to `name`
                }

                dt = bll.GetRecord("TOP 500 id, name, GLAccountID", "pos_banks");
                DataView dv2 = dt.DefaultView;
                dv2.RowFilter = "GLAccountID = " + glAccountId;
                BindCombo(cmb_ref_account_code, dv2.ToTable(), "id", "name","Bank");
            }
            catch
            {
                ClearRefCombo();
            }
        }

        
        private bool IsReceivableAccount(int glAccountId)
        {
            try
            {
                GeneralBLL bll = new GeneralBLL();
                DataTable dt = bll.GetRecord("TOP 500 id, name", "acc_accounts");

                foreach (DataRow row in dt.Rows)
                {
                    if (row["id"] == DBNull.Value) continue;
                    if (Convert.ToInt32(row["id"]) != glAccountId) continue;

                    if (!dt.Columns.Contains("name") || row["name"] == DBNull.Value)
                        return false;

                    var t = row["name"].ToString().Trim();

                    // String-based detection
                    if (t.Equals("Account Receivable", StringComparison.OrdinalIgnoreCase))
                        return true;

                    // Numeric-based detection (if your DB uses codes)
                    int code;
                    if (int.TryParse(t, out code))
                    {
                        // Adjust this if your system has a known Account Receivable code.
                        // Keep conservative: assume code 5 indicates Account Receivable (common pattern in some POS schemas).
                        if (code == 5) return true;
                    }

                    return false;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        private bool IsPayableAccount(int glAccountId)
        {
            try
            {
                GeneralBLL bll = new GeneralBLL();
                DataTable dt = bll.GetRecord("TOP 500 id, name", "acc_accounts");
                foreach (DataRow row in dt.Rows)
                {
                    if (row["id"] == DBNull.Value) continue;
                    if (Convert.ToInt32(row["id"]) != glAccountId) continue;
                    if (!dt.Columns.Contains("name") || row["name"] == DBNull.Value)
                        return false;
                    var t = row["name"].ToString().Trim();
                    // String-based detection
                    if (t.Equals("Account Payable", StringComparison.OrdinalIgnoreCase))
                        return true;
                    // Numeric-based detection (if your DB uses codes)
                    int code;
                    if (int.TryParse(t, out code))
                    {
                        // Adjust this if your system has a known Account Payable code.
                        // Keep conservative: assume code 6 indicates Account Payable (common pattern in some POS schemas).
                        if (code == 6) return true;
                    }
                    return false;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        private void BindCombo(ComboBox combo, DataTable dt, string valueMember, string displayMember, string type = "Account")
        {
            if (combo == null) return;

            combo.DataSource = null;

            DataRow emptyRow = dt.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Please Select "+ type;              // Set Column Value
            dt.Rows.InsertAt(emptyRow, 0);

            combo.DisplayMember = displayMember;
            combo.ValueMember = valueMember;
            combo.DataSource = dt;

            combo.Enabled = (dt != null && dt.Rows.Count > 0);
            if (combo.Enabled)
                combo.SelectedIndex = 0;
        }

        private void ClearRefCombo()
        {
            if (cmb_ref_account_code == null) return;
            cmb_ref_account_code.DataSource = null;
            cmb_ref_account_code.Items.Clear();
            cmb_ref_account_code.Text = string.Empty;
            cmb_ref_account_code.Enabled = false;
        }
    }
}
