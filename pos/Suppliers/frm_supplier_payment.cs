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
    public partial class frm_supplier_payment : Form
    {
        private frm_addSupplier mainForm;
        public int _supplier_id;
        public string _supplier_name;
        public string _invoice_no;

        public int cash_account_id = 0;
        //public int sales_account_id = 0;
        public int payable_account_id = 0;
        public int tax_account_id = 0;
        public int purchases_discount_acc_id = 0;
        public int inventory_acc_id = 0;
        public int purchases_acc_id = 0;

        public frm_supplier_payment(frm_addSupplier mainForm, int supplier_id, string supplier_name="")
       {
            this.mainForm = mainForm;
            _supplier_id = supplier_id;
            _supplier_name = supplier_name;

            InitializeComponent();
        }

        public frm_supplier_payment()
        {
            InitializeComponent();
           
        }
        
        public void frm_supplier_payment_Load(object sender, EventArgs e)
        {
            Get_AccountID_From_Company();
            GetMAXInvoiceNo();
            get_accounts_dropdownlist();
            lblSupplierName.Text = _supplier_name;
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
            if (_invoice_no != string.Empty && _supplier_id != 0)
            {
                int GL_account_id = (cmb_GL_account_code.SelectedValue == null ? 0 : int.Parse(cmb_GL_account_code.SelectedValue.ToString()));
                int Ref_account_id = (cmb_ref_account_code.SelectedValue == null ? 0 : int.Parse(cmb_ref_account_code.SelectedValue.ToString()));

                ///ACCOUNT Payable JOURNAL ENTRY (debit)
                int entry_id = Insert_Journal_entry(_invoice_no, payable_account_id, Convert.ToDouble(txt_total_amount.Text), 0, txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0,0);
  
                ///CASH/BANK JOURNAL ENTRY (credit)
                Insert_Journal_entry(_invoice_no, GL_account_id, 0, Convert.ToDouble(txt_total_amount.Text), txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0,0);
                
                var description = "Payment to supplier: (" + _supplier_id + ") " + _supplier_name + " - " + txt_description.Text;
                        
                
                // Insert entry into reference account if selected
                if (Ref_account_id != 0)
                {
                    // check if reference account is receivable or payable or bank
                    BankBLL bankBLL = new BankBLL();
                    if (bankBLL.IsBankGlAccount(GL_account_id))
                    {
                        description += (" - Ref Bank Account: " + cmb_ref_account_code.SelectedText);

                        // Insert into reference account (credit) in bank payment
                        Insert_Journal_entry(_invoice_no, GL_account_id,  0, Convert.ToDouble(txt_total_amount.Text), txt_payment_date.Value.Date, description, 0, 0, entry_id, Ref_account_id);
                    }
                    else
                    {
                        // Default handling for other account types
                    }

                }

                ///ADD ENTRY INTO supplier PAYMENT(DEBIT)
                Insert_Journal_entry(_invoice_no, GL_account_id, Convert.ToDouble(txt_total_amount.Text), 0, txt_payment_date.Value.Date, description, 0, _supplier_id, entry_id, 0);

                if (txt_discount.Text != string.Empty)
                {
                    ///receivable JOURNAL ENTRY (debit)
                    Insert_Journal_entry(_invoice_no, payable_account_id, Convert.ToDouble(txt_discount.Text), 0, txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0,0);
                    
                    /// SALES DISCOUNT JOURNAL ENTRY (credit)
                    int entry_idd = Insert_Journal_entry(_invoice_no, purchases_discount_acc_id, 0, Convert.ToDouble(txt_discount.Text), txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0,0);
                    
                    ///ADD ENTRY INTO supplier PAYMENT(debit)
                    Insert_Journal_entry(_invoice_no, purchases_discount_acc_id, Convert.ToDouble(txt_discount.Text), 0, txt_payment_date.Value.Date, txt_description.Text, 0, _supplier_id, entry_idd,0);

                }

               if (entry_id > 0)
                {
                    MessageBox.Show("Record created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


               mainForm.load_transactions_grid(_supplier_id);
                    
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
            JournalsModal_obj.entry_id = entry_id;
            JournalsModal_obj.bank_id = bank_id;

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
                payable_account_id = (int)dr["payable_acc_id"];
                tax_account_id = (int)dr["tax_acc_id"];
                purchases_discount_acc_id = (int)dr["purchases_discount_acc_id"];
                inventory_acc_id = (int)dr["inventory_acc_id"];
                purchases_acc_id = (int)dr["purchases_acc_id"];
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
            this.Close();
        }

        private void frm_supplier_payment_KeyDown(object sender, KeyEventArgs e)
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
        private void BindCombo(ComboBox combo, DataTable dt, string valueMember, string displayMember, string type = "Account")
        {
            if (combo == null) return;

            combo.DataSource = null;

            //DataRow emptyRow = dt.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "Please Select " + type;              // Set Column Value
            //dt.Rows.InsertAt(emptyRow, 0);

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

        private void cmb_GL_account_code_SelectedIndexChanged(object sender, EventArgs e)
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

            //if (glAccountId == receivable_account_id)
            //{
            //    LoadCustomersIntoRefCombo();
            //    return;
            //}

            //if (glAccountId == payable_account_id)
            //{
            //    LoadSuppliersIntoRefCombo();
            //    return;
            //}
            BankBLL bankBLL = new BankBLL();
            if (bankBLL.IsBankGlAccount(glAccountId))
            {
                LoadBankAccountsIntoRefCombo(glAccountId);
                return;
            }

            ClearRefCombo();
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
                BindCombo(cmb_ref_account_code, dv2.ToTable(), "id", "name", "Bank");
            }
            catch
            {
                ClearRefCombo();
            }
        }
    }
}
