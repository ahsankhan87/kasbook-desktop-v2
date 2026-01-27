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
    public partial class frm_addPaymentMethod : Form
    {
        public static frm_addPaymentMethod instance;
        public TextBox tb_id;
        public TextBox tb_code;
        public TextBox tb_desc;
        public Label tb_lbl_is_edit;
        private frm_payment_method mainForm;
        
        public frm_addPaymentMethod(frm_payment_method mainForm): this()
        {
            this.mainForm = mainForm;
        }

        public frm_addPaymentMethod()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_code = txt_code;
            tb_desc = txt_description;
            
            tb_lbl_is_edit = lbl_edit_status;

        }

        public void frm_addPaymentMethod_Load(object sender, EventArgs e)
        {
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update PaymentMethods";
                
            }
            else
            {
                btn_save.Text = "Save";
            }
        }
        
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                bool isEdit = (lbl_edit_status.Text == "true");

                if (string.IsNullOrWhiteSpace(txt_code.Text))
                {
                    UiMessages.ShowInfo(
                        "Code is required.",
                        "الكود مطلوب.",
                        "Validation",
                        "التحقق"
                    );
                    txt_code.Focus();
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    isEdit ? "Update this payment method?" : "Save this payment method?",
                    isEdit ? "هل تريد تحديث طريقة الدفع هذه؟" : "هل تريد حفظ طريقة الدفع هذه؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T(isEdit ? "Updating..." : "Saving...", isEdit ? "جاري التحديث..." : "جاري الحفظ...")))
                {
                    PaymentMethodModal info = new PaymentMethodModal();
                    info.code = txt_code.Text.Trim();
                    info.description = txt_description.Text;

                    PaymentMethodBLL objBLL = new PaymentMethodBLL();
                    int result;

                    if (isEdit)
                    {
                        int id;
                        if (!int.TryParse(txt_id.Text, out id) || id <= 0)
                        {
                            UiMessages.ShowError(
                                "Invalid payment method id.",
                                "معرّف طريقة الدفع غير صالح.",
                                "Error",
                                "خطأ"
                            );
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
                    {
                        UiMessages.ShowInfo(
                            isEdit ? "Record updated successfully." : "Record created successfully.",
                            isEdit ? "تم تحديث السجل بنجاح." : "تم إنشاء السجل بنجاح.",
                            "Success",
                            "نجاح"
                        );
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "Record not saved.",
                            "لم يتم حفظ السجل.",
                            "Error",
                            "خطأ"
                        );
                        return;
                    }
                }

                if (mainForm != null)
                    mainForm.load_payment_method_grid();

                this.Close();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
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

        private void frm_addPaymentMethod_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        
    }
}
