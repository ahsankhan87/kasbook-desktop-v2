using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Master.Banks
{
    public partial class frm_bank_payment : Form
    {
        private frm_banks mainForm;
        public int _bank_id;
        public int _bank_account_code;
        public string _invoice_no;

        public frm_bank_payment(frm_banks mainForm, int bank_id,int bank_account_code)
        {
            this.mainForm = mainForm;
            _bank_id = bank_id;
            _bank_account_code = bank_account_code;

            InitializeComponent();
        }
        public frm_bank_payment()
        {
            InitializeComponent();
        }

        private void frm_bank_payment_Load(object sender, EventArgs e)
        {
            get_accounts_dropdownlist();
        }

        public string GetMAXInvoiceNo()
        {
            JournalsBLL JournalsBLL_obj = new JournalsBLL();
            return JournalsBLL_obj.GetMaxInvoiceNo();
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

            cmb_cash_account_code.DisplayMember = "name";
            cmb_cash_account_code.ValueMember = "id";
            cmb_cash_account_code.DataSource = accounts;

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (_invoice_no != string.Empty && _bank_id != 0)
            {
                _invoice_no = GetMAXInvoiceNo();
                int cash_account_id = (cmb_cash_account_code.SelectedValue == null ? 0 : int.Parse(cmb_cash_account_code.SelectedValue.ToString()));

                if (cash_account_id == 0)
                {
                    MessageBox.Show("Please select transfer to GL Account.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                ///CASH JOURNAL ENTRY (DEBIT)
                Insert_Journal_entry(_invoice_no, cash_account_id, Convert.ToDouble(txt_total_amount.Text), 0, txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0);

                ///BANK JOURNAL ENTRY (CREDIT)
                int entry_id = Insert_Journal_entry(_invoice_no, _bank_account_code, 0, Convert.ToDouble(txt_total_amount.Text), txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0);

                ///ADD ENTRY INTO bank PAYMENT(CREDIT)
                Insert_Journal_entry(_invoice_no, cash_account_id, 0, Convert.ToDouble(txt_total_amount.Text), txt_payment_date.Value.Date, txt_description.Text, _bank_id, 0, entry_id);

               
                if (entry_id > 0)
                {
                    MessageBox.Show("Entry saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Entry not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


                mainForm.load_banks_transactions_grid(_bank_id);

                this.Close();

            }
            else
            {
                MessageBox.Show("Please enter value in field", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }
        private int Insert_Journal_entry(string invoice_no, int account_id, double debit, double credit, DateTime date,
          string description, int bank_id, int supplier_id, int entry_id)
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
            JournalsModal_obj.bank_id = bank_id;
            JournalsModal_obj.supplier_id = supplier_id;
            JournalsModal_obj.entry_id = entry_id;

            journal_id = JournalsObj.Insert(JournalsModal_obj);
            return journal_id;
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

        private void frm_bank_payment_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

    }
}
