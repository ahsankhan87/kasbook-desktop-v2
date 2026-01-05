using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace pos.Sales
{
    public partial class frm_pos_sale : Form
    {
        private readonly SalesBLL _salesBll = new SalesBLL();
        private readonly ProductBLL _productBll = new ProductBLL();

        private readonly System.Windows.Forms.Timer _searchDebounce = new System.Windows.Forms.Timer();

        public frm_pos_sale()
        {
            InitializeComponent();

            // touch-friendly defaults
            KeyPreview = true;
            _searchDebounce.Interval = 250;
            _searchDebounce.Tick += (s, e) =>
            {
                _searchDebounce.Stop();
                LoadProductsTiles(txtSearch.Text);
            };

            ApplyLanguage(UsersModal.logged_in_lang);
        }

        private void frm_pos_sale_Load(object sender, EventArgs e)
        {
            lblCashier.Text = UsersModal.logged_in_userid.ToString();
            lblBranch.Text = UsersModal.logged_in_branch_id.ToString();
            UpdateTotals();

            txtBarcode.Focus();
            timerClock.Start();

            // initial tiles
            LoadProductsTiles(string.Empty);
        }

        private void ApplyLanguage(string lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
                lang = "en-US";

            UsersModal.logged_in_lang = lang;

            var culture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            bool isArabic = lang.Equals("ar-SA", StringComparison.OrdinalIgnoreCase);

            // RTL support (keeps UI usable)
            RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No;
            RightToLeftLayout = isArabic;

            btnLang.Text = isArabic ? "EN" : "AR";
            lblHeader.Text = isArabic ? "نقطة البيع" : "Point of Sale";

            btnPayCash.Text = isArabic ? "نقداً" : "Cash";
            btnPayCard.Text = isArabic ? "بطاقة" : "Card";
            btnPayMixed.Text = isArabic ? "مختلط" : "Mixed";
            btnHold.Text = isArabic ? "تعليق" : "Hold";
            btnRecall.Text = isArabic ? "استرجاع" : "Recall";
            btnNewSale.Text = isArabic ? "جديد" : "New";
            btnRemoveLine.Text = isArabic ? "حذف" : "Remove";
            btnQtyPlus.Text = isArabic ? "+ كمية" : "+ Qty";
            btnQtyMinus.Text = isArabic ? "- كمية" : "- Qty";
            btnDiscount.Text = isArabic ? "خصم" : "Discount";
            btnSearchFocus.Text = isArabic ? "بحث" : "Search";
            btnBarcodeFocus.Text = isArabic ? "باركود" : "Barcode";

            lblSubtotalCaption.Text = isArabic ? "الإجمالي" : "Subtotal";
            lblTaxCaption.Text = isArabic ? "الضريبة" : "Tax";
            lblDiscountCaption.Text = isArabic ? "الخصم" : "Discount";
            lblGrandCaption.Text = isArabic ? "المجموع" : "Total";
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            lblClock.Text = DateTime.Now.ToString("dd MMM yyyy  HH:mm");
        }

        private void btnLang_Click(object sender, EventArgs e)
        {
            ApplyLanguage(UsersModal.logged_in_lang == "ar-SA" ? "en-US" : "ar-SA");
        }

        private void btnBarcodeFocus_Click(object sender, EventArgs e)
        {
            txtBarcode.Focus();
            txtBarcode.SelectAll();
        }

        private void btnSearchFocus_Click(object sender, EventArgs e)
        {
            txtSearch.Focus();
            txtSearch.SelectAll();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _searchDebounce.Stop();
            _searchDebounce.Start();
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            e.Handled = true;
            e.SuppressKeyPress = true;

            var barcode = (txtBarcode.Text ?? string.Empty).Trim();
            if (barcode.Length == 0) return;

            AddProductByBarcode(barcode);
            txtBarcode.Clear();
        }

        private void AddProductByBarcode(string barcode)
        {
            // Your ProductDLL has GetProductByBarcode(barcode, branchId) in DLL
            // ProductBLL likely wraps it similarly; if not, add in BLL later.
            DataTable dt = new POS.DLL.ProductDLL().GetProductByBarcode(barcode, UsersModal.logged_in_branch_id);

            if (dt == null || dt.Rows.Count == 0)
            {
                System.Media.SystemSounds.Beep.Play();
                return;
            }

            var row = dt.Rows[0];

            string code = Convert.ToString(row["code"]);
            string name = UsersModal.logged_in_lang == "ar-SA"
                ? Convert.ToString(row["name_ar"])
                : Convert.ToString(row["name"]);
            string itemNumber = Convert.ToString(row["item_number"]);

            decimal unitPrice = Convert.ToDecimal(row["unit_price"]);
            decimal taxRate = row.Table.Columns.Contains("tax_rate") ? Convert.ToDecimal(row["tax_rate"] ?? 0) : 0m;

            AddOrIncrementCartLine(code, itemNumber, name, 1m, unitPrice, taxRate);
        }

        private void LoadProductsTiles(string keyword)
        {
            // Keep it cheap: show favorites later; for now just search
            // Uses your existing full-text product search in ProductDLL
            var dt = new POS.DLL.ProductDLL().SearchProductByBrandAndCategory_2(keyword ?? "", "", "", "");

            flpTiles.SuspendLayout();
            flpTiles.Controls.Clear();

            int max = Math.Min(dt.Rows.Count, 60); // avoid heavy UI load
            for (int i = 0; i < max; i++)
            {
                var r = dt.Rows[i];

                string name = UsersModal.logged_in_lang == "ar-SA"
                    ? Convert.ToString(r["name_ar"])
                    : Convert.ToString(r["name"]);

                string code = Convert.ToString(r["code"]);
                string itemNumber = Convert.ToString(r["item_number"]);

                decimal unitPrice = Convert.ToDecimal(r["unit_price"]);
                decimal taxRate = r.Table.Columns.Contains("tax_rate") && r["tax_rate"] != DBNull.Value
                    ? Convert.ToDecimal(r["tax_rate"])
                    : 0m;

                var btn = new Button
                {
                    Width = 170,
                    Height = 95,
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    Text = name + Environment.NewLine + unitPrice.ToString("N2"),
                    Tag = new Tuple<string, string, string, decimal, decimal>(code, itemNumber, name, unitPrice, taxRate)
                };
                btn.FlatAppearance.BorderColor = Color.Gainsboro;

                btn.Click += (s, e) =>
                {
                    var t = (Tuple<string, string, string, decimal, decimal>)((Button)s).Tag;
                    AddOrIncrementCartLine(t.Item1, t.Item2, t.Item3, 1m, t.Item4, t.Item5);
                };

                flpTiles.Controls.Add(btn);
            }

            flpTiles.ResumeLayout();
        }

        private void AddOrIncrementCartLine(string code, string itemNumber, string name, decimal qty, decimal unitPrice, decimal taxRate)
        {
            // Grid columns: Code, ItemNumber(hidden), Name, Qty, Price, TaxRate(hidden), LineTotal
            foreach (DataGridViewRow r in gridCart.Rows)
            {
                if (r.IsNewRow) continue;
                if (Convert.ToString(r.Cells["colItemNumber"].Value) == itemNumber)
                {
                    var currentQty = Convert.ToDecimal(r.Cells["colQty"].Value);
                    r.Cells["colQty"].Value = currentQty + qty;
                    RecalcRow(r);
                    UpdateTotals();
                    return;
                }
            }

            int idx = gridCart.Rows.Add();
            var row = gridCart.Rows[idx];
            row.Cells["colCode"].Value = code;
            row.Cells["colItemNumber"].Value = itemNumber;
            row.Cells["colName"].Value = name;
            row.Cells["colQty"].Value = qty;
            row.Cells["colPrice"].Value = unitPrice;
            row.Cells["colTaxRate"].Value = taxRate;
            RecalcRow(row);
            UpdateTotals();
        }

        private void RecalcRow(DataGridViewRow row)
        {
            decimal qty = Convert.ToDecimal(row.Cells["colQty"].Value);
            decimal price = Convert.ToDecimal(row.Cells["colPrice"].Value);
            decimal taxRate = Convert.ToDecimal(row.Cells["colTaxRate"].Value);

            decimal lineSub = qty * price;
            decimal lineTax = (lineSub * taxRate) / 100m;
            row.Cells["colTotal"].Value = (lineSub + lineTax).ToString("N2");
        }

        private void UpdateTotals()
        {
            decimal subtotal = 0m;
            decimal tax = 0m;

            foreach (DataGridViewRow r in gridCart.Rows)
            {
                if (r.IsNewRow) continue;

                decimal qty = Convert.ToDecimal(r.Cells["colQty"].Value);
                decimal price = Convert.ToDecimal(r.Cells["colPrice"].Value);
                decimal taxRate = Convert.ToDecimal(r.Cells["colTaxRate"].Value);

                decimal lineSub = qty * price;
                subtotal += lineSub;
                tax += (lineSub * taxRate) / 100m;
            }

            decimal discount = 0m; // invoice-level discount can be added later
            decimal grand = subtotal + tax - discount;

            lblSubtotal.Text = subtotal.ToString("N2");
            lblTax.Text = tax.ToString("N2");
            lblDiscount.Text = discount.ToString("N2");
            lblGrandTotal.Text = grand.ToString("N2");
        }

        private void btnNewSale_Click(object sender, EventArgs e)
        {
            gridCart.Rows.Clear();
            UpdateTotals();
            txtBarcode.Focus();
        }

        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (gridCart.CurrentRow == null || gridCart.CurrentRow.IsNewRow) return;
            gridCart.Rows.Remove(gridCart.CurrentRow);
            UpdateTotals();
        }

        private void btnQtyPlus_Click(object sender, EventArgs e)
        {
            if (gridCart.CurrentRow == null || gridCart.CurrentRow.IsNewRow) return;
            gridCart.CurrentRow.Cells["colQty"].Value = Convert.ToDecimal(gridCart.CurrentRow.Cells["colQty"].Value) + 1m;
            RecalcRow(gridCart.CurrentRow);
            UpdateTotals();
        }

        private void btnQtyMinus_Click(object sender, EventArgs e)
        {
            if (gridCart.CurrentRow == null || gridCart.CurrentRow.IsNewRow) return;
            var q = Convert.ToDecimal(gridCart.CurrentRow.Cells["colQty"].Value) - 1m;
            if (q <= 0m)
            {
                gridCart.Rows.Remove(gridCart.CurrentRow);
            }
            else
            {
                gridCart.CurrentRow.Cells["colQty"].Value = q;
                RecalcRow(gridCart.CurrentRow);
            }
            UpdateTotals();
        }

        private void btnPayCash_Click(object sender, EventArgs e)
        {
            // TODO: open payment dialog, then call SalesBLL.ProcessSale(...) or SalesDLL.CreateSale(...)
            MessageBox.Show("Cash payment flow not wired yet.");
        }

        private void btnPayCard_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Card payment flow not wired yet.");
        }

        private void btnPayMixed_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mixed payment flow not wired yet.");
        }
    }
}