using pos.Security.Authorization;
using pos.UI;
using POS.BLL;
using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace pos.Products.Adjustment
{
    public partial class frm_adjust_qty : Form
    {
        public decimal EnteredQty { get; private set; }
        public decimal Price { get; set; }
        public string locationCode { get; set; }

        public int _productID { get; private set; }
        public string _productCode { get; private set; }

        public frm_adjust_qty(decimal defaultQty = 0m,decimal price = 0m, string locationCode = null, int productID = 0, string productCode = null)
        {
            InitializeComponent();
            txtQty.Text = defaultQty.ToString("N2");
            txtQty.Focus();
            _productID = productID;
            _productCode = productCode;
            //txt_location.Text = locationCode;
            cmb_locations.SelectedValue = locationCode;
            txt_sale_price.Text = price.ToString();
            lbl_productCode.Text = productCode;
            ApplyPermissionTags();
        }
        private void ApplyPermissionTags()
        {
            this.Tag = Permissions.Inventory_View;

            if (btn_deleteProduct != null) btn_deleteProduct.Tag = Permissions.Products_Delete;
        }

        private void ApplyPermissionsOnLoad()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            if (AppSecurityContext.User == null || AppSecurityContext.Auth == null)
                return;

            AppSecurityContext.RefreshUserClaims();
            this.ApplyPermissions(AppSecurityContext.Auth, AppSecurityContext.User);
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            var raw = (txtQty.Text ?? string.Empty).Trim();
            if (raw.Length == 0)
            {
                UiMessages.ShowWarning(
                    "Please enter a quantity.",
                    "Ì—ÃÏ ≈œŒ«· «·ﬂ„Ì….",
                    captionEn: "Adjustment",
                    captionAr: " ”ÊÌ…");
                DialogResult = DialogResult.None;
                txtQty.Focus();
                txtQty.SelectAll();
                return;
            }

            // Parse using current culture (matches numeric input in UI)
            decimal val;
            if (!decimal.TryParse(raw, NumberStyles.Number, CultureInfo.CurrentCulture, out val))
            {
                UiMessages.ShowWarning(
                    "Please enter a valid quantity (numbers only).",
                    "Ì—ÃÏ ≈œŒ«· ﬂ„Ì… ’ÕÌÕ… (√—ﬁ«„ ›ﬁÿ).",
                    captionEn: "Adjustment",
                    captionAr: " ”ÊÌ…");
                DialogResult = DialogResult.None;
                txtQty.Focus();
                txtQty.SelectAll();
                return;
            }

            if (val < 0)
            {
                UiMessages.ShowWarning(
                    "Quantity cannot be negative.",
                    "·« Ì„ﬂ‰ √‰  ﬂÊ‰ «·ﬂ„Ì… ”«·»….",
                    captionEn: "Adjustment",
                    captionAr: " ”ÊÌ…");
                DialogResult = DialogResult.None;
                txtQty.Focus();
                txtQty.SelectAll();
                return;
            }

            EnteredQty = Math.Round(val, 2);
            locationCode = cmb_locations.SelectedValue?.ToString(); // pass through location code for use in calling code if needed
            Price = decimal.TryParse(txt_sale_price.Text, out var price) ? price : 0m; // pass through price for use in calling code if needed
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, control keys, and one decimal separator for current culture
            var sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            char decimalChar = string.IsNullOrEmpty(sep) ? '.' : sep[0];

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != decimalChar)
            {
                e.Handled = true;
                return;
            }

            // Prevent multiple separators and prevent starting with separator
            if (e.KeyChar == decimalChar && (txtQty.Text.Contains(sep) || txtQty.Text.Length == 0))
            {
                e.Handled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_deleteProduct_Click(object sender, EventArgs e)
        {
            // delete product permanently from the system
            if (!string.IsNullOrWhiteSpace(_productID.ToString()))
            {
                var confirm = UiMessages.ConfirmYesNo(
                    "Delete this product? This action cannot be undone.",
                    "Â·  —Ìœ Õ–› Â–« «·„‰ Ãø ·« Ì„ﬂ‰ «· —«Ã⁄ ⁄‰ Â–« «·≈Ã—«¡.",
                    captionEn: "Confirm Delete",
                    captionAr: " √ﬂÌœ «·Õ–›"
                );

                if (confirm == DialogResult.Yes)
                {
                    ProductBLL objBLL = new ProductBLL();
                    objBLL.Delete(_productID);

                    UiMessages.ShowInfo(
                        "Product has been deleted successfully.",
                        " „ Õ–› «·„‰ Ã »‰Ã«Õ.",
                        "Deleted",
                        " „ «·Õ–›"
                    );
                    return;
                }
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a product record first.",
                    "Ì—ÃÏ «Œ Ì«— ”Ã· „‰ Ã √Ê·«.",
                    "Product",
                    "«·„‰ Ã"
                );
            }
        }

        private void frm_adjust_qty_KeyDown(object sender, KeyEventArgs e)
        {
            // When press enter button it should consider TAB
            if(e.KeyValue == (char)Keys.Enter)
            {
                e.Handled = true;
                SendKeys.Send("{TAB}");
            }

            if(e.KeyValue == (char)Keys.Delete)
            {

                btn_deleteProduct.PerformClick();
            }
        }

        private void frm_adjust_qty_Load(object sender, EventArgs e)
        {
            ApplyPermissionsOnLoad();
            get_locations_dropdownlist();
        }
        public void get_locations_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_locations";

            DataTable locations = generalBLL_obj.GetRecord(keyword, table);
            //DataRow emptyRow = locations.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "All";              // Set Column Value
            //locations.Rows.InsertAt(emptyRow, 0);


            cmb_locations.DisplayMember = "name";
            cmb_locations.ValueMember = "code";
            cmb_locations.DataSource = locations;

        }
    }
}