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
            GetMAXInvoiceNo();
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

                ///CASH JOURNAL ENTRY (DEBIT)
                Insert_Journal_entry(_invoice_no, cash_account_id, Convert.ToDouble(txt_total_amount.Text), 0, txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0);
                
                ///ACCOUNT RECEIVABLE JOURNAL ENTRY (CREDIT)
                int entry_id = Insert_Journal_entry(_invoice_no, receivable_account_id, 0, Convert.ToDouble(txt_total_amount.Text), txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0);
  
                ///ADD ENTRY INTO customer PAYMENT(DEBIT)
                Insert_Journal_entry(_invoice_no, cash_account_id, 0, Convert.ToDouble(txt_total_amount.Text), txt_payment_date.Value.Date, txt_description.Text, _customer_id, 0, entry_id);
                
                if (txt_discount.Text != string.Empty)
                {
                    /// SALES DISCOUNT JOURNAL ENTRY (debit)
                    int entry_idd = Insert_Journal_entry(_invoice_no, sales_discount_acc_id, Convert.ToDouble(txt_discount.Text), 0, txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0);
                    ///receivable JOURNAL ENTRY (credit)
                    Insert_Journal_entry(_invoice_no, receivable_account_id, 0, Convert.ToDouble(txt_discount.Text), txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0);

                    
                    ///ADD ENTRY INTO customer PAYMENT(credit)
                    Insert_Journal_entry(_invoice_no, sales_discount_acc_id, 0, Convert.ToDouble(txt_discount.Text), txt_payment_date.Value.Date, txt_description.Text, _customer_id, 0, entry_idd);

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
           string description, int customer_id, int supplier_id, int entry_id)
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
                //tax_account_id = (int)dr["tax_acc_id"];
                sales_discount_acc_id = (int)dr["sales_discount_acc_id"];
                //inventory_acc_id = (int)dr["inventory_acc_id"];
                //purchases_acc_id = (int)dr["purchases_acc_id"];
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
    }
}
