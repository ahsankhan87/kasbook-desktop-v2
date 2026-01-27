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
    public partial class frm_addPaymentTerm : Form
    {
        public static frm_addPaymentTerm instance;
        public TextBox tb_id;
        public TextBox tb_code;
        public TextBox tb_desc;
        public Label tb_lbl_is_edit;
        private frm_payment_terms mainForm;
        
        public frm_addPaymentTerm(frm_payment_terms mainForm): this()
        {
            this.mainForm = mainForm;
        }

        public frm_addPaymentTerm()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_code = txt_code;
            tb_desc = txt_description;
            
            tb_lbl_is_edit = lbl_edit_status;

        }

        public void frm_addPaymentTerm_Load(object sender, EventArgs e)
        {
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update PaymentTerms";
                
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
                    isEdit ? "Update this payment term?" : "Save this payment term?",
                    isEdit ? "هل تريد تحديث شرط الدفع هذا؟" : "هل تريد حفظ شرط الدفع هذا؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T(isEdit ? "Updating..." : "Saving...", isEdit ? "جاري التحديث..." : "جاري الحفظ...")))
                {
                    PaymentTermsModal info = new PaymentTermsModal();
                    info.code = txt_code.Text.Trim();
                    info.description = txt_description.Text;

                    PaymentTermsBLL objBLL = new PaymentTermsBLL();
                    int result;

                    if (isEdit)
                    {
                        int id;
                        if (!int.TryParse(txt_id.Text, out id) || id <= 0)
                        {
                            UiMessages.ShowError(
                                "Invalid payment term id.",
                                "معرّف شرط الدفع غير صالح.",
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
                    mainForm.load_payment_terms_grid();

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

        private void frm_addPaymentTerm_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        
    }
}
