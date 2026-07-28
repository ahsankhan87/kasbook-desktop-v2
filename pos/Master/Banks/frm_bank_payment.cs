using POS.BLL;
using POS.Core;
using pos.UI;
using pos.UI.Busy;
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
        public string _bankName;

        private readonly Timer _amountDebounce = new Timer();
        private const int AmountDebounceMs = 250;

        public frm_bank_payment(frm_banks mainForm, int bank_id,int bank_account_code,string bankName = "")
        {
            this.mainForm = mainForm;
            _bank_id = bank_id;
            _bank_account_code = bank_account_code;
            _bankName = bankName;

            InitializeComponent();
        }
        public frm_bank_payment()
        {
            InitializeComponent();
        }

        private void frm_bank_payment_Load(object sender, EventArgs e)
        {
            // debounce validation for amount
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

            // lightweight validation feedback; do not block
            double amount;
            if (!string.IsNullOrWhiteSpace(txt_total_amount.Text) &&
                (!double.TryParse(txt_total_amount.Text.Trim(), out amount) || amount <= 0))
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

            int defaultCashAccountId = ResolveDefaultAccountId(SettingKeys.DefaultCashAccount);
            int defaultBankAccountId = ResolveDefaultAccountId(SettingKeys.DefaultBankAccount);

            List<int> accountIds = new List<int>();
            if (defaultCashAccountId > 0)
                accountIds.Add(defaultCashAccountId);
            if (defaultBankAccountId > 0 && !accountIds.Contains(defaultBankAccountId))
                accountIds.Add(defaultBankAccountId);
            if (_bank_account_code > 0 && !accountIds.Contains(_bank_account_code))
                accountIds.Add(_bank_account_code);

            DataTable accounts;
            if (accountIds.Count > 0)
            {
                string table = "acc_accounts where id IN (" + string.Join(",", accountIds.Distinct()) + ")";
                accounts = generalBLL_obj.GetRecord(keyword, table);
            }
            else
            {
                accounts = new DataTable();
                accounts.Columns.Add("id", typeof(int));
                accounts.Columns.Add("name", typeof(string));
            }

            DataRow emptyRow = accounts.NewRow();
            emptyRow[0] = 0;
            emptyRow[1] = "Please Select";
            accounts.Rows.InsertAt(emptyRow, 0);

            cmb_cash_account_code.DisplayMember = "name";
            cmb_cash_account_code.ValueMember = "id";
            cmb_cash_account_code.DataSource = accounts;

            cmb_cash_account_code.SelectedValue = defaultCashAccountId > 0 ? defaultCashAccountId : 0;
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
                        "Please select the GL account to transfer to.",
                        "يرجى اختيار حساب الأستاذ للتحويل إليه.",
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
                    "Post this bank payment?",
                    "هل تريد ترحيل دفعة البنك؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T("Posting payment...", "جاري ترحيل الدفعة...")))
                {
                    _invoice_no = GetMAXInvoiceNo();

                    if (string.IsNullOrWhiteSpace(_invoice_no))
                    {
                        UiMessages.ShowError("Voucher number could not be generated.", "تعذر إنشاء رقم القيد.", "Error", "خطأ");
                        return;
                    }

                    if (cash_account_id == _bank_account_code)
                    {
                        UiMessages.ShowInfo("Source and destination account cannot be the same.", "لا يمكن أن يكون حساب المصدر والوجهة نفس الحساب.", "Validation", "التحقق");
                        return;
                    }

                    List<JVLineModel> lines = new List<JVLineModel>
                    {
                        new JVLineModel
                        {
                            AccountId = cash_account_id,
                            Debit = Convert.ToDecimal(amount),
                            Credit = 0m,
                            Narration = txt_description.Text.Trim(),
                            ModuleName = "BANK_PAYMENT"
                        },
                        new JVLineModel
                        {
                            AccountId = _bank_account_code,
                            Debit = 0m,
                            Credit = Convert.ToDecimal(amount),
                            Narration = txt_description.Text.Trim(),
                            ModuleName = "BANK_PAYMENT",
                            BankId = _bank_id
                        }
                    };

                    AutoJVModel model = new AutoJVModel
                    {
                        ModuleName = "PAYMENT",
                        RefModule = "pos_banks_payments",
                        RefId = _bank_id,
                        VoucherDate = txt_payment_date.Value.Date,
                        ReferenceNo = _invoice_no,
                        Narration = txt_description.Text.Trim(),
                        IsAutoPosted = true,
                        BranchId = UsersModal.logged_in_branch_id,
                        Lines = lines
                    };

                    PostResult postResult = new JournalsBLL().PostAutoJournalEntry(model, UsersModal.logged_in_userid);
                    if (postResult == null || !postResult.Success)
                    {
                        string errorMessage = "Payment could not be posted. Please try again.";
                        if (postResult != null && postResult.Messages != null && postResult.Messages.Count > 0)
                        {
                            ValidationError firstError = postResult.Messages.FirstOrDefault(x => x.IsBlocking) ?? postResult.Messages.FirstOrDefault();
                            if (firstError != null && !string.IsNullOrWhiteSpace(firstError.Message))
                                errorMessage = firstError.Message;
                        }

                        UiMessages.ShowError(errorMessage, errorMessage, "Error", "خطأ");
                        return;
                    }

                    UiMessages.ShowInfo(
                        "Payment has been posted successfully.",
                        "تم ترحيل الدفعة بنجاح.",
                        "Success",
                        "نجاح"
                    );

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
        private int ResolveDefaultAccountId(string settingKey)
        {
            if (string.IsNullOrWhiteSpace(settingKey))
                return 0;

            try
            {
                GeneralBLL objBLL = new GeneralBLL();
                string safeKey = settingKey.Replace("'", "''");
                DataTable settingDt = objBLL.GetRecord("TOP 1 setting_value", "pos_settings WHERE setting_key='" + safeKey + "'");
                if (settingDt == null || settingDt.Rows.Count == 0 || settingDt.Rows[0]["setting_value"] == DBNull.Value)
                    return 0;

                string accountCode = Convert.ToString(settingDt.Rows[0]["setting_value"]);
                if (string.IsNullOrWhiteSpace(accountCode))
                    return 0;

                string safeCode = accountCode.Trim().Replace("'", "''");
                DataTable accountDt = objBLL.GetRecord("TOP 1 id", "acc_accounts WHERE LTRIM(RTRIM(code))='" + safeCode + "'");
                if (accountDt == null || accountDt.Rows.Count == 0 || accountDt.Rows[0]["id"] == DBNull.Value)
                    return 0;

                int accountId;
                return int.TryParse(Convert.ToString(accountDt.Rows[0]["id"]), out accountId) ? accountId : 0;
            }
            catch
            {
                return 0;
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

        private void frm_bank_payment_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void txt_total_amount_TextChanged(object sender, EventArgs e)
        {
            _amountDebounce.Stop();
            _amountDebounce.Start();
        }
    }
}
