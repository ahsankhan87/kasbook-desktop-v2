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
using pos.UI;
using pos.UI.Busy;

namespace pos.Master.Banks
{
    public partial class frm_deposit_to_bank : Form
    {
        private frm_banks mainForm;
        public int _bank_id;
        public int _bank_account_code;
        public string _invoice_no;
        public string _bankName;

        private readonly Timer _amountDebounce = new Timer();
        private const int AmountDebounceMs = 250;

        public frm_deposit_to_bank(frm_banks mainForm, int bank_id, int bank_account_code,string bankName="")
        {
            this.mainForm = mainForm;
            _bank_id = bank_id;
            _bank_account_code = bank_account_code;
            _bankName = bankName;

            InitializeComponent();
        }
        
        public frm_deposit_to_bank()
        {
            InitializeComponent();
        }

        private void frm_payment_to_bank_Load(object sender, EventArgs e)
        {
            //lbl_header_title.Text = "Deposit amount to bank (" + _bankName + ")";

            _amountDebounce.Interval = AmountDebounceMs;
            _amountDebounce.Tick += AmountDebounce_Tick;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading...", "جاري التحميل...")))
                {
                    get_accounts_dropdownlist();
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

            // lightweight validation feedback; don't block user aggressively
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
                txt_total_amount.SelectAll();
                txt_total_amount.Focus();
            }
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

            cmb_cash_account_code.SelectedValue = "3";

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (_bank_id == 0)
                {
                    UiMessages.ShowInfo(
                        "Bank record is not selected.",
                        "لم يتم اختيار البنك.",
                        "Bank",
                        "البنك"
                    );
                    return;
                }

                if (_bank_account_code == 0)
                {
                    UiMessages.ShowError(
                        "Bank GL account is not configured.",
                        "حساب الأستاذ للبنك غير مُعد.",
                        "Error",
                        "خطأ"
                    );
                    return;
                }

                int cash_account_id = 0;
                if (cmb_cash_account_code.SelectedValue != null)
                    int.TryParse(cmb_cash_account_code.SelectedValue.ToString(), out cash_account_id);

                if (cash_account_id == 0)
                {
                    UiMessages.ShowInfo(
                        "Please select the GL account to transfer from (cash/bank).",
                        "يرجى اختيار حساب الأستاذ للتحويل منه (نقدي/بنك).",
                        "Validation",
                        "التحقق"
                    );
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_total_amount.Text))
                {
                    UiMessages.ShowInfo(
                        "Amount is required.",
                        "المبلغ مطلوب.",
                        "Validation",
                        "التحقق"
                    );
                    return;
                }

                double amount;
                if (!double.TryParse(txt_total_amount.Text.Trim(), out amount) || amount <= 0)
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
                    "Post this bank deposit?",
                    "هل تريد ترحيل إيداع البنك؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T("Posting deposit...", "جاري ترحيل الإيداع...")))
                {
                    // GET MAX INVOICE NO
                    _invoice_no = GetMAXInvoiceNo();

                    // BANK JOURNAL ENTRY (DEBIT)
                    Insert_Journal_entry(_invoice_no, _bank_account_code, amount, 0, txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0);

                    // CASH JOURNAL ENTRY (CREDIT)
                    int entry_id = Insert_Journal_entry(_invoice_no, cash_account_id, 0, amount, txt_payment_date.Value.Date, txt_description.Text, 0, 0, 0);

                    // ADD ENTRY INTO BANK PAYMENT (DEBIT)
                    Insert_Journal_entry(_invoice_no, _bank_account_code, amount, 0, txt_payment_date.Value.Date, txt_description.Text, _bank_id, 0, entry_id);

                    if (entry_id > 0)
                    {
                        UiMessages.ShowInfo(
                            "Deposit has been posted successfully.",
                            "تم ترحيل الإيداع بنجاح.",
                            "Success",
                            "نجاح"
                        );
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "Deposit could not be posted. Please try again.",
                            "تعذر ترحيل الإيداع. يرجى المحاولة مرة أخرى.",
                            "Error",
                            "خطأ"
                        );
                    }

                    if (mainForm != null)
                        mainForm.load_banks_transactions_grid(_bank_id);

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
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

        private void txt_total_amount_TextChanged(object sender, EventArgs e)
        {
            _amountDebounce.Stop();
            _amountDebounce.Start();
        }

        private void frm_deposit_to_bank_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }
}
