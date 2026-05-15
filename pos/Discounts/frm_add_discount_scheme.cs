using POS.BLL;
using POS.Core;
using pos.UI;
using System;
using System.Data;
using System.Windows.Forms;

namespace pos.Discounts
{
    public partial class frm_add_discount_scheme : Form
    {
        private readonly frm_discount_schemes _parent;
        private readonly int _editId;
        private readonly bool _isEdit;

        public frm_add_discount_scheme(frm_discount_schemes parent, int editId = 0)
        {
            InitializeComponent();
            _parent = parent;
            _editId = editId;
            _isEdit = editId > 0;
        }

        private void frm_add_discount_scheme_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            BindLookups();

            cmb_apply_on.Items.Clear();
            cmb_apply_on.Items.Add("Product");
            cmb_apply_on.Items.Add("Brand");
            cmb_apply_on.Items.Add("Category");
            cmb_apply_on.SelectedIndex = 0;

            cmb_calc_type.Items.Clear();
            cmb_calc_type.Items.Add("PERCENT");
            cmb_calc_type.Items.Add("AMOUNT");
            cmb_calc_type.SelectedIndex = 0;

            if (_isEdit)
            {
                lbl_header_title.Text = "Edit Discount Scheme";
                btn_save.Text = "Update";
                LoadForEdit();
            }
            else
            {
                lbl_header_title.Text = "New Discount Scheme";
                btn_save.Text = "Save";
                chk_is_active.Checked = true;
            }

            UpdateApplyOnVisibility();
        }

        private void BindLookups()
        {
            var g = new GeneralBLL();

            var dtProducts = g.GetRecord("id,name", "pos_products WHERE deleted=0 ORDER BY name");
            cmb_product.DisplayMember = "name";
            cmb_product.ValueMember = "id";
            cmb_product.DataSource = dtProducts;

            var dtBrands = g.GetRecord("id,name", "pos_brands ORDER BY name");
            cmb_brand.DisplayMember = "name";
            cmb_brand.ValueMember = "id";
            cmb_brand.DataSource = dtBrands;

            var dtCategories = g.GetRecord("id,name", "pos_categories ORDER BY name");
            cmb_category.DisplayMember = "name";
            cmb_category.ValueMember = "id";
            cmb_category.DataSource = dtCategories;
        }

        private void LoadForEdit()
        {
            try
            {
                var bll = new DiscountSchemesBLL();
                DataTable dt = bll.GetById(_editId);
                if (dt.Rows.Count == 0) return;

                DataRow r = dt.Rows[0];
                txt_id.Text = r["id"].ToString();
                txt_name.Text = r["name"].ToString();
                txt_name_ar.Text = r["name_ar"].ToString();
                txt_value.Text = r["value"].ToString();
                chk_is_active.Checked = Convert.ToBoolean(r["is_active"]);

                cmb_calc_type.SelectedItem = r["calc_type"].ToString();

                if (r["start_date"] != DBNull.Value)
                    dtp_start.Value = Convert.ToDateTime(r["start_date"]);
                if (r["end_date"] != DBNull.Value)
                    dtp_end.Value = Convert.ToDateTime(r["end_date"]);

                chk_no_end.Checked = (r["end_date"] == DBNull.Value);
                chk_no_start.Checked = (r["start_date"] == DBNull.Value);

                if (r["product_id"] != DBNull.Value)
                {
                    cmb_apply_on.SelectedItem = "Product";
                    cmb_product.SelectedValue = Convert.ToInt32(r["product_id"]);
                }
                else if (r["brand_id"] != DBNull.Value)
                {
                    cmb_apply_on.SelectedItem = "Brand";
                    cmb_brand.SelectedValue = Convert.ToInt32(r["brand_id"]);
                }
                else if (r["category_id"] != DBNull.Value)
                {
                    cmb_apply_on.SelectedItem = "Category";
                    cmb_category.SelectedValue = Convert.ToInt32(r["category_id"]);
                }

                UpdateApplyOnVisibility();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!Validate_()) return;

            try
            {
                int? productId = null;
                int? brandId = null;
                int? categoryId = null;

                switch (cmb_apply_on.SelectedItem?.ToString())
                {
                    case "Product":
                        productId = Convert.ToInt32(cmb_product.SelectedValue);
                        break;
                    case "Brand":
                        brandId = Convert.ToInt32(cmb_brand.SelectedValue);
                        break;
                    case "Category":
                        categoryId = Convert.ToInt32(cmb_category.SelectedValue);
                        break;
                }

                var info = new DiscountSchemeModal
                {
                    id = _isEdit ? _editId : 0,
                    name = txt_name.Text.Trim(),
                    name_ar = txt_name_ar.Text.Trim(),
                    product_id = productId,
                    brand_id = brandId,
                    category_id = categoryId,
                    calc_type = cmb_calc_type.SelectedItem.ToString(),
                    value = double.Parse(txt_value.Text.Trim()),
                    is_active = chk_is_active.Checked,
                    start_date = chk_no_start.Checked ? (DateTime?)null : dtp_start.Value.Date,
                    end_date = chk_no_end.Checked ? (DateTime?)null : dtp_end.Value.Date,
                    branch_id = UsersModal.logged_in_branch_id,
                    company_id = UsersModal.loggedIncompanyID,
                    created_by = UsersModal.logged_in_userid
                };

                var bll = new DiscountSchemesBLL();
                if (_isEdit)
                    bll.Update(info);
                else
                    bll.Insert(info);

                UiMessages.ShowInfo(
                    _isEdit ? "Scheme updated successfully." : "Scheme saved successfully.",
                    _isEdit ? "تم تحديث الخطة بنجاح." : "تم حفظ الخطة بنجاح.",
                    "Discount Schemes", "خطط الخصم");

                _parent?.RefreshGrid();
                Close();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private bool Validate_()
        {
            if (string.IsNullOrWhiteSpace(txt_name.Text))
            {
                UiMessages.ShowInfo("Name is required.", "الاسم مطلوب.", "Validation", "التحقق");
                txt_name.Focus();
                return false;
            }

            if (cmb_apply_on.SelectedItem == null)
            {
                UiMessages.ShowInfo("Please select discount target.", "يرجى اختيار هدف الخصم.", "Validation", "التحقق");
                return false;
            }

            double val;
            if (!double.TryParse(txt_value.Text.Trim(), out val) || val < 0)
            {
                UiMessages.ShowInfo("Discount value must be a valid positive number.", "قيمة الخصم يجب أن تكون رقماً موجباً.", "Validation", "التحقق");
                txt_value.Focus();
                return false;
            }

            if (cmb_calc_type.SelectedItem?.ToString() == "PERCENT" && val > 100)
            {
                UiMessages.ShowInfo("Percent discount cannot exceed 100%.", "نسبة الخصم لا يمكن أن تتجاوز 100%.", "Validation", "التحقق");
                txt_value.Focus();
                return false;
            }

            return true;
        }

        private void btn_cancel_Click(object sender, EventArgs e) => Close();

        private void cmb_apply_on_SelectedIndexChanged(object sender, EventArgs e) => UpdateApplyOnVisibility();

        private void UpdateApplyOnVisibility()
        {
            string mode = cmb_apply_on.SelectedItem?.ToString() ?? "Product";

            lbl_product.Visible = cmb_product.Visible = (mode == "Product");
            lbl_brand.Visible = cmb_brand.Visible = (mode == "Brand");
            lbl_category.Visible = cmb_category.Visible = (mode == "Category");
        }

        private void chk_no_start_CheckedChanged(object sender, EventArgs e)
            => dtp_start.Enabled = !chk_no_start.Checked;

        private void chk_no_end_CheckedChanged(object sender, EventArgs e)
            => dtp_end.Enabled = !chk_no_end.Checked;
    }
}
