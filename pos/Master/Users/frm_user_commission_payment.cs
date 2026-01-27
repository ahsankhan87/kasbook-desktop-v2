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
    public partial class frm_user_commission_payment : Form
    {
        private frm_adduser mainForm;
        public int _user_id;
        public string _invoice_no;

        public int cash_account_id = 0;
        public int sales_account_id = 0;
        public int receivable_account_id = 0;
        public int sales_discount_acc_id = 0;
        //public int item_variance_acc_id = 0;
        public int commission_acc_id = 0;

        private readonly Timer _amountDebounce = new Timer();
        private const int AmountDebounceMs = 250;

        public frm_user_commission_payment(frm_adduser mainForm, int user_id)
       {
            this.mainForm = mainForm;
            _user_id = user_id;
            
            InitializeComponent();
        }

        public frm_user_commission_payment()
        {
            InitializeComponent();
           
        }
        
        public void frm_user_commission_payment_Load(object sender, EventArgs e)
        {
            _amountDebounce.Interval = AmountDebounceMs;
            _amountDebounce.Tick += AmountDebounce_Tick;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading...", "جاري التحميل...")))
                {
                    Get_AccountID_From_Company();
                    GetMAXInvoiceNo();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void AmountDebounce_Tick(object sender, EventArgs e)
        {
            _amountDebounce.Stop();

            if (string.IsNullOrWhiteSpace(txt_total_amount.Text))
                return;

            double amount;
            if (!double.TryParse(txt_total_amount.Text.Trim(), out amount) || amount <= 0)
            {
                UiMessages.ShowInfo(
                    "Please enter a valid amount.",
                    "يرجى إدخال مبلغ صحيح.",
                    "Validation",
                    "التحقق"
                );
            }
        }
        
        public void GetMAXInvoiceNo()
        {
            JournalsBLL JournalsBLL_obj = new JournalsBLL();
            _invoice_no = JournalsBLL_obj.GetMaxInvoiceNo();
        }

        private void txt_total_amount_TextChanged(object sender, EventArgs e)
        {
            _amountDebounce.Stop();
            _amountDebounce.Start();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (_user_id <= 0)
                {
                    UiMessages.ShowInfo(
                        "Invalid user.",
                        "مستخدم غير صالح.",
                        "Validation",
                        "التحقق"
                    );
                    return;
                }

                if (commission_acc_id <= 0 || cash_account_id <= 0)
                {
                    UiMessages.ShowError(
                        "Commission/Cash accounts are not configured.",
                        "حسابات العمولة/النقد غير مُعدة.",
                        "Error",
                        "خطأ"
                    );
                    return;
                }

                double amount;
                if (string.IsNullOrWhiteSpace(txt_total_amount.Text) || !double.TryParse(txt_total_amount.Text.Trim(), out amount) || amount <= 0)
                {
                    UiMessages.ShowInfo(
                        "Please enter a valid amount.",
                        "يرجى إدخال مبلغ صحيح.",
                        "Validation",
                        "التحقق"
                    );
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_description.Text))
                {
                    UiMessages.ShowInfo(
                        "Description is required.",
                        "الوصف مطلوب.",
                        "Validation",
                        "التحقق"
                    );
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    "Post this commission payment?",
                    "هل تريد ترحيل دفعة العمولة؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T("Posting payment...", "جاري ترحيل الدفعة...")))
                {
                    if (string.IsNullOrWhiteSpace(_invoice_no))
                        GetMAXInvoiceNo();

                    int entry_id = Insert_user_commission(_invoice_no, 0, amount, 0, txt_payment_date.Value.Date, txt_description.Text, _user_id);

                    // Commision JOURNAL ENTRY (debit)
                    Insert_Journal_entry(_invoice_no, commission_acc_id, amount, 0, txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0);

                    // CASH JOURNAL ENTRY (credit)
                    Insert_Journal_entry(_invoice_no, cash_account_id, 0, amount, txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0);

                    if (entry_id > 0)
                    {
                        UiMessages.ShowInfo(
                            "Commission payment posted successfully.",
                            "تم ترحيل دفعة العمولة بنجاح.",
                            "Success",
                            "نجاح"
                        );
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "Payment not saved.",
                            "لم يتم حفظ الدفعة.",
                            "Error",
                            "خطأ"
                        );
                        return;
                    }
                }

                if (mainForm != null)
                    mainForm.load_user_commission_grid(_user_id);

                this.Close();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private int Insert_user_commission(string invoice_no, int account_id, double debit, double credit, DateTime date,string description,int user_id)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            UsersBLL emp_Obj = new UsersBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = debit;
            JournalsModal_obj.credit = credit;
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.description = description;
            JournalsModal_obj.user_id = user_id;

            journal_id = emp_Obj.InsertUserCommission(JournalsModal_obj);
            return journal_id;
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
                //item_variance_acc_id = (int)dr["item_variance_acc_id"];
                commission_acc_id = (int)dr["commission_acc_id"];
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

        private void frm_user_commission_payment_KeyDown(object sender, KeyEventArgs e)
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
