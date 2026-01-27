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
    public partial class frm_addTax : Form
    {
        public static frm_addTax instance;
        public TextBox tb_id;
        public TextBox tb_title;
        public TextBox tb_rate;
        public Label tb_lbl_is_edit;
        private frm_taxes mainForm;
        
        public frm_addTax(frm_taxes mainForm): this()
        {
            this.mainForm = mainForm;
        }

        public frm_addTax()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_title = txt_title;
            tb_rate = txt_rate;
            
            tb_lbl_is_edit = lbl_edit_status;

        }
        
        public void frm_addTax_Load(object sender, EventArgs e)
        {
           
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update Tax";
                
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

                if (string.IsNullOrWhiteSpace(txt_title.Text))
                {
                    UiMessages.ShowInfo(
                        "Title is required.",
                        "العنوان مطلوب.",
                        "Validation",
                        "التحقق"
                    );
                    txt_title.Focus();
                    return;
                }

                double rate;
                if (string.IsNullOrWhiteSpace(txt_rate.Text) || !double.TryParse(txt_rate.Text.Trim(), out rate) || rate < 0)
                {
                    UiMessages.ShowInfo(
                        "Please enter a valid tax rate.",
                        "يرجى إدخال نسبة ضريبة صحيحة.",
                        "Validation",
                        "التحقق"
                    );
                    txt_rate.Focus();
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    isEdit ? "Update this tax?" : "Save this tax?",
                    isEdit ? "هل تريد تحديث هذه الضريبة؟" : "هل تريد حفظ هذه الضريبة؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T(isEdit ? "Updating..." : "Saving...", isEdit ? "جاري التحديث..." : "جاري الحفظ...")))
                {
                    TaxModal info = new TaxModal();
                    info.title = txt_title.Text.Trim();
                    info.rate = rate;

                    TaxBLL objBLL = new TaxBLL();
                    int result;

                    if (isEdit)
                    {
                        int id;
                        if (!int.TryParse(txt_id.Text, out id) || id <= 0)
                        {
                            UiMessages.ShowError(
                                "Invalid tax id.",
                                "معرّف الضريبة غير صالح.",
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
                    mainForm.load_Taxes_grid();

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

        private void frm_addTax_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        
    }
}
