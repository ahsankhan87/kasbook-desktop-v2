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
    public partial class frm_addBranch : Form
    {
        public static frm_addBranch instance;
        public TextBox tb_id;
        public TextBox tb_name;
        public Label tb_lbl_is_edit;
        private frm_branches mainForm;
        
        public frm_addBranch(frm_branches mainForm): this()
        {
            this.mainForm = mainForm;
        }

        public frm_addBranch()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_name = txt_name;
            
            tb_lbl_is_edit = lbl_edit_status;

        }

        public void frm_addBranch_Load(object sender, EventArgs e)
        {
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update Branches";

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
                if (string.IsNullOrWhiteSpace(txt_name.Text))
                {
                    UiMessages.ShowInfo(
                        "Branch name is required.",
                        "اسم الفرع مطلوب.",
                        "Validation",
                        "التحقق"
                    );
                    txt_name.Focus();
                    return;
                }

                bool isEdit = (lbl_edit_status.Text == "true");

                var confirm = UiMessages.ConfirmYesNo(
                    isEdit ? "Update this branch?" : "Save this branch?",
                    isEdit ? "هل تريد تحديث هذا الفرع؟" : "هل تريد حفظ هذا الفرع؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T(isEdit ? "Updating..." : "Saving...", isEdit ? "جاري التحديث..." : "جاري الحفظ...")))
                {
                    BranchesModal info = new BranchesModal();
                    info.name = txt_name.Text.Trim();
                    info.description = txt_description.Text;

                    BranchesBLL objBLL = new BranchesBLL();

                    int result;
                    if (isEdit)
                    {
                        int id;
                        if (!int.TryParse(txt_id.Text, out id) || id <= 0)
                        {
                            UiMessages.ShowError(
                                "Invalid branch id.",
                                "معرّف الفرع غير صالح.",
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
                            isEdit ? "Branch has been updated successfully." : "Branch has been created successfully.",
                            isEdit ? "تم تحديث الفرع بنجاح." : "تم إنشاء الفرع بنجاح.",
                            "Success",
                            "نجاح"
                        );
                    }
                    else
                    {
                        UiMessages.ShowError(
                            isEdit ? "Branch could not be updated. Please try again." : "Branch could not be saved. Please try again.",
                            isEdit ? "تعذر تحديث الفرع. يرجى المحاولة مرة أخرى." : "تعذر حفظ الفرع. يرجى المحاولة مرة أخرى.",
                            "Error",
                            "خطأ"
                        );
                        return;
                    }
                }

                if (mainForm != null)
                {
                    mainForm.load_branches_grid();
                }

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

        private void frm_addBranch_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        
    }
}
