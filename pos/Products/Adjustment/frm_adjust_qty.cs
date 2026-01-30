using System;
using System.Globalization;
using System.Windows.Forms;
using pos.UI;

namespace pos.Products.Adjustment
{
    public partial class frm_adjust_qty : Form
    {
        public decimal EnteredQty { get; private set; }

        public frm_adjust_qty(decimal defaultQty = 0m)
        {
            InitializeComponent();
            txtQty.Text = defaultQty.ToString("N2");
            txtQty.Focus();
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
    }
}