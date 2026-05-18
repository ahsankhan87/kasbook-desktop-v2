using POS.BLL;
using POS.Core;
using pos.UI;
using pos.UI.Busy;
using System;
using System.Data;
using System.Windows.Forms;

namespace pos.Discounts
{
    public partial class frm_apply_discount_scheme_bulk : Form
    {
        private readonly frm_discount_schemes _parent;

        public frm_apply_discount_scheme_bulk(frm_discount_schemes parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void frm_apply_discount_scheme_bulk_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            LoadSchemes();
            LoadBrands();
            LoadCategories();
        }

        private void LoadSchemes()
        {
            var dt = new DiscountSchemesBLL().GetAllActive(UsersModal.logged_in_branch_id);
            var empty = dt.NewRow();
            empty["id"] = 0;
            empty["name"] = "Please Select";
            dt.Rows.InsertAt(empty, 0);

            cmb_scheme.DisplayMember = "name";
            cmb_scheme.ValueMember = "id";
            cmb_scheme.DataSource = dt;
        }

        private void LoadBrands()
        {
            var dt = new GeneralBLL().GetRecord("code,name", "pos_brands");
            var empty = dt.NewRow();
            empty["code"] = "";
            empty["name"] = "All Brands";
            dt.Rows.InsertAt(empty, 0);

            cmb_brand.DisplayMember = "name";
            cmb_brand.ValueMember = "code";
            cmb_brand.DataSource = dt;
        }

        private void LoadCategories()
        {
            var dt = new GeneralBLL().GetRecord("code,name", "pos_categories");
            var empty = dt.NewRow();
            empty["code"] = "";
            empty["name"] = "All Categories";
            dt.Rows.InsertAt(empty, 0);

            cmb_category.DisplayMember = "name";
            cmb_category.ValueMember = "code";
            cmb_category.DataSource = dt;
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmb_scheme.SelectedValue == null || Convert.ToInt32(cmb_scheme.SelectedValue) <= 0)
                {
                    UiMessages.ShowWarning("Please select a discount scheme.", "يرجى اختيار خطة خصم.");
                    return;
                }

                string brandCode = cmb_brand.SelectedValue == null ? "" : cmb_brand.SelectedValue.ToString();
                string categoryCode = cmb_category.SelectedValue == null ? "" : cmb_category.SelectedValue.ToString();

                if (string.IsNullOrWhiteSpace(brandCode) && string.IsNullOrWhiteSpace(categoryCode))
                {
                    UiMessages.ShowWarning(
                        "Select at least a brand or a category filter.",
                        "يرجى اختيار علامة تجارية أو قسم على الأقل.");
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    "Apply selected discount scheme to matching products?",
                    "هل تريد تطبيق خطة الخصم المحددة على المنتجات المطابقة؟",
                    "Confirm",
                    "تأكيد");

                if (confirm != DialogResult.Yes)
                    return;

                int schemeId = Convert.ToInt32(cmb_scheme.SelectedValue);
                int affected;

                using (BusyScope.Show(this, UiMessages.T("Applying...", "جاري التطبيق...")))
                {
                    affected = new DiscountSchemesBLL().ApplySchemeToProducts(schemeId, brandCode, categoryCode);
                }

                UiMessages.ShowInfo(
                    string.Format("Done. {0} product(s) updated.", affected),
                    string.Format("تم بنجاح. تم تحديث {0} منتج/منتجات.", affected),
                    "Discount Schemes",
                    "خطط الخصم");

                _parent?.RefreshGrid();
                Close();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
