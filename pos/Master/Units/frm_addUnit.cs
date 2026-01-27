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
    public partial class frm_addUnit : Form
    {
        public static frm_addUnit instance;
        public TextBox tb_id;
        public TextBox tb_name;
        public Label tb_lbl_is_edit;
        private frm_units mainForm;
        
        public frm_addUnit(frm_units mainForm): this()
        {
            this.mainForm = mainForm;
        }

        public frm_addUnit()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_name = txt_name;
            
            tb_lbl_is_edit = lbl_edit_status;

        }

        public void frm_addUnit_Load(object sender, EventArgs e)
        {
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update units";
                
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

                if (string.IsNullOrWhiteSpace(txt_name.Text))
                {
                    UiMessages.ShowInfo(
                        "Unit name is required.",
                        "اسم الوحدة مطلوب.",
                        "Validation",
                        "التحقق"
                    );
                    txt_name.Focus();
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    isEdit ? "Update this unit?" : "Save this unit?",
                    isEdit ? "هل تريد تحديث هذه الوحدة؟" : "هل تريد حفظ هذه الوحدة؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T(isEdit ? "Updating..." : "Saving...", isEdit ? "جاري التحديث..." : "جاري الحفظ...")))
                {
                    UnitsModal info = new UnitsModal();
                    info.name = txt_name.Text.Trim();

                    UnitsBLL objBLL = new UnitsBLL();
                    int result;

                    if (isEdit)
                    {
                        int id;
                        if (!int.TryParse(txt_id.Text, out id) || id <= 0)
                        {
                            UiMessages.ShowError(
                                "Invalid unit id.",
                                "معرّف الوحدة غير صالح.",
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
                    mainForm.load_units_grid();

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

        private void frm_addUnit_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        
    }
}
