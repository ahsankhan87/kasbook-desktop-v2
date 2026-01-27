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
    public partial class frm_addBrand : Form
    {
        public static frm_addBrand instance;
        public TextBox tb_id;
        public TextBox tb_name;
        public TextBox tb_code;
        public ComboBox tb_category;
        public ComboBox tb_groups;
        public Label tb_lbl_is_edit;
        private frm_brands mainForm;

        public frm_addBrand(frm_brands mainForm)
            : this()
        {
            this.mainForm = mainForm;
            
        }

        public frm_addBrand()
        {
            InitializeComponent();
            instance = this;
            get_category_dropdownlist();
            get_groups_dropdownlist();
            
            tb_id = txt_id;
            tb_name = txt_name;
            tb_code = txt_code;
            tb_category = cmb_category;
            tb_groups = cmb_groups;
            
            tb_lbl_is_edit = lbl_edit_status;

        }

        public void frm_addBrand_Load(object sender, EventArgs e)
        {
            
            if (lbl_edit_status.Text == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update Brands";
                
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

                if (string.IsNullOrWhiteSpace(txt_code.Text) || string.IsNullOrWhiteSpace(txt_name.Text))
                {
                    UiMessages.ShowInfo(
                        "Brand code and name are required.",
                        "كود واسم العلامة التجارية حقول مطلوبة.",
                        "Validation",
                        "التحقق"
                    );
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    isEdit ? "Update this brand?" : "Save this brand?",
                    isEdit ? "هل تريد تحديث هذه العلامة التجارية؟" : "هل تريد حفظ هذه العلامة التجارية؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T(isEdit ? "Updating..." : "Saving...", isEdit ? "جاري التحديث..." : "جاري الحفظ...")))
                {
                    BrandsModal info = new BrandsModal();
                    info.code = txt_code.Text.Trim();
                    info.name = txt_name.Text.Trim();
                    info.category_code = cmb_category.SelectedValue == null ? "" : cmb_category.SelectedValue.ToString();
                    info.group_code = cmb_groups.SelectedValue == null ? "" : cmb_groups.SelectedValue.ToString();

                    BrandsBLL objBLL = new BrandsBLL();
                    int result;

                    if (isEdit)
                    {
                        int id;
                        if (!int.TryParse(txt_id.Text, out id) || id <= 0)
                        {
                            UiMessages.ShowError(
                                "Invalid brand id.",
                                "معرّف العلامة التجارية غير صالح.",
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
                    mainForm.load_Brands_grid();

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

        private void frm_addBrand_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }


        public void get_category_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_categories";

            DataTable category_code = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = category_code.NewRow();
            emptyRow[0] = "";              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            category_code.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = category_code.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[1] = "ADD NEW";              // Set Column Value
            category_code.Rows.InsertAt(emptyRow1, 1);

            cmb_category.DisplayMember = "name";
            cmb_category.ValueMember = "code";
            cmb_category.DataSource = category_code;

        }

        private void cmb_origin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_category.SelectedValue != null && cmb_category.SelectedValue.ToString() == "-1")
            {
                frm_addOrigin originfrm = new frm_addOrigin(null);
                originfrm.ShowDialog();

                get_category_dropdownlist();
            }
            txt_category_code.Text = (cmb_category.SelectedValue != null ? cmb_category.SelectedValue.ToString() : "");
           
        }

        public void get_groups_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_product_groups";

            DataTable dt = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = dt.NewRow();
            emptyRow[0] = "";              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            dt.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = dt.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[1] = "ADD NEW";              // Set Column Value
            dt.Rows.InsertAt(emptyRow1, 1);

            cmb_groups.DisplayMember = "name";
            cmb_groups.ValueMember = "code";
            cmb_groups.DataSource = dt;

        }

        private void cmb_groups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_groups.SelectedValue != null && cmb_groups.SelectedValue.ToString() == "-1")
            {
                frm_product_groups frm = new frm_product_groups();
                frm.ShowDialog();

                get_groups_dropdownlist();
            }
            txt_group_code.Text = (cmb_groups.SelectedValue != null ? cmb_groups.SelectedValue.ToString() : "");
           
        }

    }
}
