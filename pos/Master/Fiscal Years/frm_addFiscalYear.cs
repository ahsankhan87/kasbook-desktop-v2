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
    public partial class frm_addFiscalYear : Form
    {
        public static frm_addFiscalYear instance;
        public TextBox tb_id;
        public TextBox tb_name;
        public TextBox tb_code;
        public DateTimePicker tb_from_date;
        public DateTimePicker tb_to_date;
        public Label tb_lbl_is_edit;
        private frm_fiscal_years mainForm;
        
        public frm_addFiscalYear(frm_fiscal_years mainForm): this()
        {
            this.mainForm = mainForm;
        }

        public frm_addFiscalYear()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_name = txt_description;
            tb_code = txt_code;
            tb_from_date = txt_from_date;
            tb_to_date = txt_todate;
            
            tb_lbl_is_edit = lbl_edit_status;

        }

        public void frm_addFiscalYear_Load(object sender, EventArgs e)
        {
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update fiscal_years";
                
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

                if (txt_from_date.Value.Date > txt_todate.Value.Date)
                {
                    UiMessages.ShowInfo(
                        "From date must be less than or equal to To date.",
                        "يجب أن يكون تاريخ البداية أقل من أو يساوي تاريخ النهاية.",
                        "Validation",
                        "التحقق"
                    );
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    isEdit ? "Update this fiscal year?" : "Save this fiscal year?",
                    isEdit ? "هل تريد تحديث هذه السنة المالية؟" : "هل تريد حفظ هذه السنة المالية؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T(isEdit ? "Updating..." : "Saving...", isEdit ? "جاري التحديث..." : "جاري الحفظ...")))
                {
                    FiscalYearModal info = new FiscalYearModal();
                    info.code = txt_code.Text.Trim();
                    info.name = txt_description.Text;
                    info.from_date = txt_from_date.Value.Date;
                    info.to_date = txt_todate.Value.Date;

                    FiscalYearBLL objBLL = new FiscalYearBLL();
                    int result;

                    if (isEdit)
                    {
                        int id;
                        if (!int.TryParse(txt_id.Text, out id) || id <= 0)
                        {
                            UiMessages.ShowError(
                                "Invalid fiscal year id.",
                                "معرّف السنة المالية غير صالح.",
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
                    mainForm.load_fiscal_years_grid();

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

        private void frm_addFiscalYear_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        
    }
}
