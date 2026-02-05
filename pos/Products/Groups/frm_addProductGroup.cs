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

namespace pos
{
    public partial class frm_addProductGroup : Form
    {
        public static frm_addProductGroup instance;
        public TextBox tb_id;
        public TextBox tb_name;
        public TextBox tb_code;
        public Label tb_lbl_is_edit;
        private frm_product_groups mainForm;

        public frm_addProductGroup(frm_product_groups mainForm)
            : this()
        {
            this.mainForm = mainForm;
            
        }

        public frm_addProductGroup()
        {
            InitializeComponent();
            instance = this;
            tb_id = txt_id;
            tb_name = txt_name;
            tb_code = txt_code;
            
            tb_lbl_is_edit = lbl_edit_status;

        }

        public void frm_addProductGroup_Load(object sender, EventArgs e)
        {
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = UiMessages.T("Update", "تحديث");
                lbl_header_title.Text = UiMessages.T("Update Product Group", "تحديث مجموعة المنتجات");
            }
            else
            {
                btn_save.Text = UiMessages.T("Save", "حفظ");
                lbl_header_title.Text = UiMessages.T("Add Product Group", "إضافة مجموعة منتجات");
            }
        }
        
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                string code = (txt_code.Text ?? string.Empty).Trim();
                string name = (txt_name.Text ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
                {
                    UiMessages.ShowWarning(
                        "Please enter both group code and name.",
                        "يرجى إدخال كود واسم المجموعة.",
                        captionEn: "Validation",
                        captionAr: "التحقق");
                    txt_code.Focus();
                    return;
                }

                bool isEdit = lbl_edit_status.Text == "true";

                var confirm = UiMessages.ConfirmYesNo(
                    isEdit ? "Update this product group?" : "Create this product group?",
                    isEdit ? "هل تريد تحديث مجموعة المنتجات؟" : "هل تريد إنشاء مجموعة المنتجات؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد");

                if (confirm != DialogResult.Yes)
                    return;

                ProductGroupsModal info = new ProductGroupsModal();
                info.code = code;
                info.name = name;

                ProductGroupsBLL objBLL = new ProductGroupsBLL();

                int result;
                if (isEdit)
                {
                    int id;
                    int.TryParse(txt_id.Text, out id);
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
                        isEdit ? "Product group updated successfully." : "Product group created successfully.",
                        isEdit ? "تم تحديث مجموعة المنتجات بنجاح." : "تم إنشاء مجموعة المنتجات بنجاح.",
                        captionEn: "Success",
                        captionAr: "نجاح");

                    if (mainForm != null)
                        mainForm.load_ProductGroups_grid();

                    Close();
                }
                else
                {
                    UiMessages.ShowError(
                        "Nothing was saved. Please try again.",
                        "لم يتم حفظ أي بيانات. يرجى المحاولة مرة أخرى.",
                        captionEn: "Error",
                        captionAr: "خطأ");
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frm_addProductGroup_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        
    }
}
