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
    public partial class frm_emp_commission_payment : Form
    {
        private frm_addEmployee mainForm;
        public int _emp_id;
        public string _invoice_no;

        public int cash_account_id = 0;
        public int sales_account_id = 0;
        public int receivable_account_id = 0;
        public int sales_discount_acc_id = 0;
        //public int item_variance_acc_id = 0;
        public int commission_acc_id = 0;

        public frm_emp_commission_payment(frm_addEmployee mainForm, int emp_id)
       {
            this.mainForm = mainForm;
            _emp_id = emp_id;
            
            InitializeComponent();
        }

        public frm_emp_commission_payment()
        {
            InitializeComponent();
           
        }
        
        public void frm_emp_commission_payment_Load(object sender, EventArgs e)
        {
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


        public void GetMAXInvoiceNo()
        {
            JournalsBLL JournalsBLL_obj = new JournalsBLL();
            _invoice_no = JournalsBLL_obj.GetMaxInvoiceNo();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (_emp_id <= 0)
                {
                    UiMessages.ShowInfo("Invalid employee.", "موظف غير صالح.", "Validation", "التحقق");
                    return;
                }

                if (commission_acc_id <= 0 || cash_account_id <= 0)
                {
                    UiMessages.ShowError("Commission/Cash accounts are not configured.", "حسابات العمولة/النقد غير مُعدة.", "Error", "خطأ");
                    return;
                }

                double amount;
                if (string.IsNullOrWhiteSpace(txt_total_amount.Text) || !double.TryParse(txt_total_amount.Text.Trim(), out amount) || amount <= 0)
                {
                    UiMessages.ShowInfo("Please enter a valid amount.", "يرجى إدخال مبلغ صحيح.", "Validation", "التحقق");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_description.Text))
                {
                    UiMessages.ShowInfo("Description is required.", "الوصف مطلوب.", "Validation", "التحقق");
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo("Post this commission payment?", "هل تريد ترحيل دفعة العمولة؟", captionEn: "Confirm", captionAr: "تأكيد");
                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T("Posting payment...", "جاري ترحيل الدفعة...")))
                {
                    if (string.IsNullOrWhiteSpace(_invoice_no))
                        GetMAXInvoiceNo();

                    int entry_id = Insert_emp_commission(_invoice_no, 0, amount, 0, txt_payment_date.Value.Date, txt_description.Text, _emp_id);
                    if (entry_id <= 0)
                    {
                        UiMessages.ShowError("Payment not saved.", "لم يتم حفظ الدفعة.", "Error", "خطأ");
                        return;
                    }

                    List<JVLineModel> lines = new List<JVLineModel>
                    {
                        new JVLineModel
                        {
                            AccountId = commission_acc_id,
                            Debit = Convert.ToDecimal(amount),
                            Credit = 0m,
                            Narration = txt_description.Text.Trim(),
                            ModuleName = "COMMISSION"
                        },
                        new JVLineModel
                        {
                            AccountId = cash_account_id,
                            Debit = 0m,
                            Credit = Convert.ToDecimal(amount),
                            Narration = txt_description.Text.Trim(),
                            ModuleName = "COMMISSION"
                        }
                    };

                    AutoJVModel model = new AutoJVModel
                    {
                        ModuleName = "PAYMENT",
                        RefModule = "pos_employees_commission",
                        RefId = _emp_id,
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

                    UiMessages.ShowInfo("Commission payment posted successfully.", "تم ترحيل دفعة العمولة بنجاح.", "Success", "نجاح");
                }

                if (mainForm != null)
                    mainForm.load_employee_commission_grid(_emp_id);

                this.Close();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private int Insert_emp_commission(string invoice_no, int account_id, double debit, double credit, DateTime date, string description, int employee_id)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            EmployeeBLL emp_Obj = new EmployeeBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = debit;
            JournalsModal_obj.credit = credit;
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.employee_id = employee_id;
            JournalsModal_obj.description = description;

            journal_id = emp_Obj.InsertEmpCommission(JournalsModal_obj);
            return journal_id;
        }
        
        private void Get_AccountID_From_Company()
        {
            cash_account_id = ResolveDefaultAccountId(SettingKeys.DefaultCashAccount);
            commission_acc_id = ResolveDefaultAccountId("ACC_DEFAULT_COMMISSION_ACCOUNT");
            if (commission_acc_id <= 0)
                commission_acc_id = ResolveDefaultAccountId("ACC_DEFAULT_SALARY_EXPENSE_ACCOUNT");
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

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
            this.Close();
        }

        private void frm_emp_commission_payment_KeyDown(object sender, KeyEventArgs e)
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
