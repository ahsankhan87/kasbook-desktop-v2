using POS.BLL;
using POS.Core;
using pos.UI;
using pos.UI.Busy;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_addCurrency : Form
    {
        public static frm_addCurrency instance;
        public TextBox tb_id;
        public TextBox tb_code;
        public TextBox tb_name;
        public TextBox tb_symbol;
        public TextBox tb_exchange_rate;
        public CheckBox tb_is_active;
        public Label tb_lbl_is_edit;
        private frm_currencies mainForm;

        public frm_addCurrency(frm_currencies mainForm) : this()
        {
            this.mainForm = mainForm;
        }

        public frm_addCurrency()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_code = txt_code;
            tb_name = txt_name;
            tb_symbol = txt_symbol;
            tb_exchange_rate = txt_exchange_rate;
            tb_is_active = chk_is_active;
            tb_lbl_is_edit = lbl_edit_status;
        }

        private void frm_addCurrency_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update Currency";
            }
            else
            {
                btn_save.Text = "Save";
                chk_is_active.Checked = true;
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                bool isEdit = (lbl_edit_status.Text == "true");

                if (string.IsNullOrWhiteSpace(txt_code.Text))
                {
                    UiMessages.ShowInfo("Code is required.", "الكود مطلوب.", "Validation", "التحقق");
                    txt_code.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_name.Text))
                {
                    UiMessages.ShowInfo("Name is required.", "الاسم مطلوب.", "Validation", "التحقق");
                    txt_name.Focus();
                    return;
                }

                decimal exchangeRate;
                if (!decimal.TryParse(txt_exchange_rate.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out exchangeRate) &&
                    !decimal.TryParse(txt_exchange_rate.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out exchangeRate))
                {
                    UiMessages.ShowInfo("Exchange rate is not valid.", "سعر الصرف غير صالح.", "Validation", "التحقق");
                    txt_exchange_rate.Focus();
                    return;
                }

                if (exchangeRate <= 0)
                {
                    UiMessages.ShowInfo("Exchange rate must be greater than zero.", "يجب أن يكون سعر الصرف أكبر من صفر.", "Validation", "التحقق");
                    txt_exchange_rate.Focus();
                    return;
                }

                if (UiMessages.ConfirmYesNo(isEdit ? "Update this currency?" : "Save this currency?", isEdit ? "هل تريد تحديث هذه العملة؟" : "هل تريد حفظ هذه العملة؟", "Confirm", "تأكيد") != DialogResult.Yes) return;

                using (BusyScope.Show(this, UiMessages.T(isEdit ? "Updating..." : "Saving...", isEdit ? "جاري التحديث..." : "جاري الحفظ...")))
                {
                    CurrencyModal info = new CurrencyModal
                    {
                        code = txt_code.Text.Trim(),
                        name = txt_name.Text.Trim(),
                        symbol = txt_symbol.Text.Trim(),
                        exchange_rate = exchangeRate,
                        is_active = chk_is_active.Checked
                    };

                    CurrencyBLL objBLL = new CurrencyBLL();
                    int result;

                    if (isEdit)
                    {
                        int id;
                        if (!int.TryParse(txt_id.Text, out id) || id <= 0)
                        {
                            UiMessages.ShowError("Invalid currency id.", "معرّف العملة غير صالح.", "Error", "خطأ");
                            return;
                        }

                        info.id = id;
                        result = objBLL.Update(info);
                    }
                    else
                    {
                        result = objBLL.Insert(info);
                    }

                    if (result > 0)
                        UiMessages.ShowInfo(isEdit ? "Record updated successfully." : "Record created successfully.", isEdit ? "تم تحديث السجل بنجاح." : "تم إنشاء السجل بنجاح.", "Success", "نجاح");
                    else
                    {
                        UiMessages.ShowError("Record not saved.", "لم يتم حفظ السجل.", "Error", "خطأ");
                        return;
                    }
                }

                if (mainForm != null) mainForm.load_currencies_grid();
                Close();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            if (UiMessages.ConfirmYesNo("Close without saving?", "هل تريد الإغلاق بدون حفظ؟", "Confirm", "تأكيد") == DialogResult.Yes)
            {
                Dispose();
                Close();
            }
        }

        private void frm_addCurrency_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) SendKeys.Send("{TAB}");
        }
    }
}
