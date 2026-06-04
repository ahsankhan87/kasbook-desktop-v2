using POS.BLL;
using POS.Core;
using pos.UI;
using pos.UI.Busy;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_foreign_purchase_setup : Form
    {
        private readonly int _companyCurrencyId;
        private readonly PurchasesBLL _purchasesBll = new PurchasesBLL();

        public int SelectedCurrencyId { get; private set; }
        public string SelectedCurrencyCode { get; private set; }
        public decimal ExchangeRate { get; private set; }
        public string InvoiceNo { get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public string Notes { get; private set; }

        public frm_foreign_purchase_setup(int companyCurrencyId)
        {
            InitializeComponent();
            _companyCurrencyId = companyCurrencyId;
        }

        private void frm_foreign_purchase_setup_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);

            lbl_title.Font = AppTheme.FontHeader;
            lbl_title.ForeColor = Color.Black;
            panel_header.BackColor = SystemColors.Control;

            LoadCurrencies();
            PurchaseDate = DateTime.Today;
            dtp_purchase_date.Value = PurchaseDate;
            txt_notes.Text = string.Empty;

            UpdateInvoicePreview();
            UpdateUiState();
        }

        private void LoadCurrencies()
        {
            var currencyBll = new CurrencyBLL();
            DataTable dt = currencyBll.GetAll();

            if (dt == null)
                dt = new DataTable();

            if (!dt.Columns.Contains("id")) dt.Columns.Add("id", typeof(int));
            if (!dt.Columns.Contains("name")) dt.Columns.Add("name", typeof(string));
            if (!dt.Columns.Contains("code")) dt.Columns.Add("code", typeof(string));
            if (!dt.Columns.Contains("exchange_rate")) dt.Columns.Add("exchange_rate", typeof(decimal));

            cmb_currency.DisplayMember = "name";
            cmb_currency.ValueMember = "id";
            cmb_currency.DataSource = dt;

            int targetCurrencyId = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (row["id"] == DBNull.Value)
                    continue;

                int id = Convert.ToInt32(row["id"]);
                if (_companyCurrencyId > 0 && id == _companyCurrencyId)
                    continue;

                targetCurrencyId = id;
                break;
            }

            if (targetCurrencyId <= 0 && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["id"] != DBNull.Value)
                    {
                        targetCurrencyId = Convert.ToInt32(row["id"]);
                        break;
                    }
                }
            }

            if (targetCurrencyId > 0)
                cmb_currency.SelectedValue = targetCurrencyId;
            else if (cmb_currency.Items.Count > 0)
                cmb_currency.SelectedIndex = 0;

            ApplySelectedCurrencyDefaults();
        }

        private void cmb_currency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_currency.SelectedValue == null || _purchasesBll == null)
                return;

            ApplySelectedCurrencyDefaults();
            UpdateUiState();
            UpdateInvoicePreview();
        }

        private void txt_exchange_rate_TextChanged(object sender, EventArgs e)
        {
            if (!txt_exchange_rate.Focused && !string.IsNullOrWhiteSpace(txt_exchange_rate.Text))
                return;

            UpdateInvoicePreview();
        }

        private void dtp_purchase_date_ValueChanged(object sender, EventArgs e)
        {
            PurchaseDate = dtp_purchase_date.Value.Date;
            UpdateInvoicePreview();
        }

        private void btn_refresh_invoice_Click(object sender, EventArgs e)
        {
            UpdateInvoicePreview();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btn_start_purchase_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            SelectedCurrencyId = Convert.ToInt32(cmb_currency.SelectedValue);
            SelectedCurrencyCode = Convert.ToString(cmb_currency.Text);
            ExchangeRate = GetExchangeRate();
            InvoiceNo = txt_invoice_no.Text.Trim();
            PurchaseDate = dtp_purchase_date.Value.Date;
            Notes = txt_notes.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidateForm()
        {
            if (cmb_currency.SelectedValue == null || Convert.ToInt32(cmb_currency.SelectedValue) <= 0)
            {
                UiMessages.ShowWarning("Select a foreign currency.", "يرجى اختيار عملة أجنبية.", captionEn: "Validation", captionAr: "التحقق");
                cmb_currency.Focus();
                return false;
            }

            if (_companyCurrencyId > 0 && Convert.ToInt32(cmb_currency.SelectedValue) == _companyCurrencyId)
            {
                UiMessages.ShowWarning("Select a currency different from the local currency.", "يرجى اختيار عملة مختلفة عن العملة المحلية.", captionEn: "Validation", captionAr: "التحقق");
                cmb_currency.Focus();
                return false;
            }

            decimal rate;
            if (!decimal.TryParse(txt_exchange_rate.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out rate) &&
                !decimal.TryParse(txt_exchange_rate.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out rate))
            {
                UiMessages.ShowWarning("Enter a valid exchange rate.", "يرجى إدخال سعر صرف صحيح.", captionEn: "Validation", captionAr: "التحقق");
                txt_exchange_rate.Focus();
                txt_exchange_rate.SelectAll();
                return false;
            }

            if (rate <= 0)
            {
                UiMessages.ShowWarning("Exchange rate must be greater than zero.", "يجب أن يكون سعر الصرف أكبر من صفر.", captionEn: "Validation", captionAr: "التحقق");
                txt_exchange_rate.Focus();
                txt_exchange_rate.SelectAll();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txt_invoice_no.Text))
            {
                UiMessages.ShowWarning("Invoice number could not be generated.", "تعذر إنشاء رقم الفاتورة.", captionEn: "Validation", captionAr: "التحقق");
                return false;
            }

            return true;
        }

        private void UpdateUiState()
        {
            if (cmb_currency.SelectedItem is DataRowView drv)
            {
                var currencyCode = Convert.ToString(drv["code"]);
                lbl_currency_hint.Text = string.IsNullOrWhiteSpace(currencyCode)
                    ? "Foreign purchase mode"
                    : "Foreign purchase mode - " + currencyCode;
            }
            else
            {
                lbl_currency_hint.Text = "Foreign purchase mode";
            }

            if (string.IsNullOrWhiteSpace(txt_exchange_rate.Text))
                txt_exchange_rate.Text = "1";
        }

        private void ApplySelectedCurrencyDefaults()
        {
            if (!(cmb_currency.SelectedItem is DataRowView drv))
                return;

            decimal rate = 0;
            if (drv.Row.Table.Columns.Contains("exchange_rate") && drv["exchange_rate"] != DBNull.Value)
                rate = Convert.ToDecimal(drv["exchange_rate"]);

            if (rate > 0)
                txt_exchange_rate.Text = rate.ToString("0.####");

            if (drv.Row.Table.Columns.Contains("code"))
                SelectedCurrencyCode = Convert.ToString(drv["code"]);
        }

        private void UpdateInvoicePreview()
        {
            if (_purchasesBll == null)
                return;

            try
            {
                if (cmb_currency.SelectedValue == null)
                    return;

                int currencyId;
                if (!int.TryParse(Convert.ToString(cmb_currency.SelectedValue), out currencyId) || currencyId <= 0)
                    return;

                decimal rate = GetExchangeRate();
                if (rate > 0)
                    txt_exchange_rate.Text = rate.ToString("0.####");

                txt_invoice_no.Text = _purchasesBll.GenerateForeignPurchaseInvoiceNo(UsersModal.logged_in_branch_id);
                InvoiceNo = txt_invoice_no.Text;
                SelectedCurrencyId = currencyId;
                SelectedCurrencyCode = Convert.ToString(cmb_currency.Text);
                ExchangeRate = rate;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private decimal GetExchangeRate()
        {
            decimal rate;
            if (decimal.TryParse(txt_exchange_rate.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out rate) ||
                decimal.TryParse(txt_exchange_rate.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out rate))
            {
                return rate;
            }

            return 0;
        }
    }
}
