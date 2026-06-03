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
        public bool IsProductDeleted { get; private set; }

        public int _productID { get; private set; }
        public string _productCode { get; private set; }

        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        public frm_adjust_qty(decimal defaultQty = 0m,decimal price = 0m, string locationCode = null, int productID = 0, string productCode = null)
        {
            InitializeComponent();
            txtQty.Text = defaultQty.ToString("N2");
            txtQty.Focus();
            _productID = productID;
            _productCode = productCode;
            txt_location.Text = locationCode;
            txt_sale_price.Text = price.ToString();
            lbl_productCode.Text = productCode;
            
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
            // convert text_location to capital letters to maintain consistency in location codes
            locationCode = txt_location.Text?.ToUpper(); // pass through location code for use in calling code if needed
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
            // Permission check
            if (!_auth.HasPermission(_currentUser, Security.Authorization.Permissions.Products_Delete))
            {
                UiMessages.ShowWarning(
                    "You do not have permission to delete products.",
                    "·Ì” ·œÌﬂ ’·«ÕÌ… ·Õ–› «·„‰ Ã« .",
                    "Permission Denied",
                    " „ —›÷ «·’·«ÕÌ…"
                );
                return;
            }

            // delete product permanently from the system
            if (_productID > 0)
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
                    IsProductDeleted = true;

                    UiMessages.ShowInfo(
                        "Product has been deleted successfully.",
                        " „ Õ–› «·„‰ Ã »‰Ã«Õ.",
                        "Deleted",
                        " „ «·Õ–›"
                    );

                    DialogResult = DialogResult.Cancel;
                    this.Close();
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
            
        }
        
    }
}