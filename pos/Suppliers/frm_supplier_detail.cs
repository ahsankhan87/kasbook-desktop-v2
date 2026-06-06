using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using POS.BLL;
using pos.Reports.Common;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_supplier_detail : Form
    {
        public int _supplier_id;
        public string _supplier_name;

        private readonly Color _teal = Color.FromArgb(0, 105, 92);
        private readonly Color _tealLight = Color.FromArgb(178, 223, 219);
        private string _avatarInitials = "?";
        private SupplierBLL bll = new SupplierBLL(); // Moved here for broader access

        public frm_supplier_detail(int supplier_id, string supplier_name)
        {
            _supplier_id = supplier_id;
            _supplier_name = supplier_name;
            InitializeComponent();
        }

        public frm_supplier_detail()
        {
            InitializeComponent();
        }

        public void frm_supplier_detail_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();

            lbl_title.Text = "Supplier Profile";
            dtLedgerTo.Value = DateTime.Today;
            dtLedgerFrom.Value = DateTime.Today.AddMonths(-3);

            LoadAllTabs();
        }

        private void StyleForm()
        {
            panelTop.BackColor = Color.White;
            tabProfile.SizeMode = TabSizeMode.Fixed;

            foreach (var panel in new[] { pnlKpi1, pnlKpi2, pnlKpi3, pnlKpi4 })
            {
                panel.Paint += KpiPaint;
            }

            var grids = new[] { gridTopItemsSmall, gridLedger, gridOutstanding };
            foreach (var grid in grids)
            {
                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 245, 243);
                grid.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
                grid.ColumnHeadersDefaultCellStyle.Font = AppTheme.FontGridHeader;
                grid.RowTemplate.Height = 30;
                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
                grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 242, 241);
                grid.DefaultCellStyle.SelectionForeColor = AppTheme.TextPrimary;
            }

            chartMonthlyPurchases.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chartMonthlyPurchases.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartMonthlyPurchases.ChartAreas[0].AxisX.Interval = 1;
            chartMonthlyPurchases.ChartAreas[0].AxisX.LabelStyle.Angle = -35;

            chartTopProducts.Series["Top"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
            chartTopProducts.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;

            chartMonthlyTrend24.Series["Trend"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chartMonthlyTrend24.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chartMonthlyTrend24.ChartAreas[0].AxisX.LabelStyle.Angle = -35;

            chartPriceHistory.Series["Price"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chartPriceHistory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chartPriceHistory.ChartAreas[0].AxisX.LabelStyle.Angle = -35;

            gridLedger.RowPrePaint += gridLedger_RowPrePaint;
        }

        private void KpiPaint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            if (p == null) return;
            using (var pen = new Pen(Color.FromArgb(225, 225, 225)))
                e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
            using (var b = new SolidBrush(_teal))
                e.Graphics.FillRectangle(b, 0, 0, 4, p.Height);
        }

        private void LoadAllTabs()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading supplier profile...", "جاري تحميل ملف المورد...")))
                {
                    LoadOverviewTab();
                    LoadLedgerTab();
                    LoadOutstandingTab();
                    LoadAnalyticsTab();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Supplier Profile", "ملف المورد");
            }
        }

        private void LoadOverviewTab()
        {
            DataTable dt = bll.GetSupplierProfileOverview(_supplier_id);
            if (dt == null || dt.Rows.Count == 0) return;

            DataRow r = dt.Rows[0];
            string fullName = (Convert.ToString(r["first_name"]) + " " + Convert.ToString(r["last_name"])).Trim();
            if (string.IsNullOrWhiteSpace(fullName))
                fullName = _supplier_name;

            _avatarInitials = GetInitials(fullName);
            pnlAvatar.Invalidate();

            lblHeaderName.Text = fullName;
            lblHeaderCode.Text = "Code: " + Convert.ToString(r["supplier_code"]);
            lblHeaderCategory.Text = "Category: " + ExtractCategory(Convert.ToString(r["address"]));
            lblHeaderPhone.Text = "Phone: " + Convert.ToString(r["contact_no"]);
            lblHeaderEmail.Text = "Email: " + (string.IsNullOrWhiteSpace(Convert.ToString(r["email"])) ? "-" : Convert.ToString(r["email"]));
            lblHeaderCreditDays.Text = "Credit Terms: " + ToInt(r["credit_days"]).ToString("N0") + " days";

            lblLifetimePurchasesValue.Text = ToDecimal(r["lifetime_purchases"]).ToString("N2");
            lblTotalPaidValue.Text = ToDecimal(r["total_paid"]).ToString("N2");
            lblCurrentPayableValue.Text = ToDecimal(r["current_payable"]).ToString("N2");
            lblAvailableCreditValue.Text = ToDecimal(r["available_credit"]).ToString("N2");

            DataTable months = bll.GetSupplierMonthlyPurchaseHistory(_supplier_id, 12);
            chartMonthlyPurchases.DataSource = months;
            chartMonthlyPurchases.Series["Monthly"].XValueMember = "month_label";
            chartMonthlyPurchases.Series["Monthly"].YValueMembers = "amount";
            chartMonthlyPurchases.DataBind();

            DataTable top5 = bll.GetSupplierTopItems(_supplier_id, 5);
            gridTopItemsSmall.DataSource = top5;
            if (gridTopItemsSmall.Columns.Contains("colTopItemQty")) gridTopItemsSmall.Columns["colTopItemQty"].DefaultCellStyle.Format = "N2";
            if (gridTopItemsSmall.Columns.Contains("colTopItemValue")) gridTopItemsSmall.Columns["colTopItemValue"].DefaultCellStyle.Format = "N2";
        }

        private void LoadLedgerTab()
        {
            DateTime from = dtLedgerFrom.Value.Date;
            DateTime to = dtLedgerTo.Value.Date;
            if (to < from)
            {
                var t = from;
                from = to;
                to = t;
            }

            DataTable dt = bll.GetSupplierLedger(_supplier_id, from, to);

            decimal dr = 0m;
            decimal cr = 0m;
            foreach (DataRow row in dt.Rows)
            {
                dr += ToDecimal(row["debit"]);
                cr += ToDecimal(row["credit"]);
            }

            DataRow total = dt.NewRow();
            total["reference_no"] = "TOTAL";
            total["debit"] = Math.Round(dr, 2);
            total["credit"] = Math.Round(cr, 2);
            total["running_balance"] = Math.Round(cr - dr, 2);
            dt.Rows.Add(total);

            gridLedger.DataSource = dt;
            lblLedgerTotals.Text = "Totals: DR " + dr.ToString("N2") + " | CR " + cr.ToString("N2") + " | " + (cr - dr).ToString("N2");

            foreach (DataGridViewColumn col in gridLedger.Columns)
            {
                if (col.Name == "colLedgerDebit" || col.Name == "colLedgerCredit" || col.Name == "colLedgerBalance")
                    col.DefaultCellStyle.Format = "N2";
            }
        }

        private void LoadOutstandingTab()
        {
            DataTable dt = bll.GetSupplierOutstandingBills(_supplier_id);
            gridOutstanding.DataSource = dt;

            foreach (DataGridViewColumn col in gridOutstanding.Columns)
            {
                if (col.Name == "colOutAmount" || col.Name == "colOutPaid" || col.Name == "colOutBalance")
                    col.DefaultCellStyle.Format = "N2";
                if (col.Name == "colOutBillDate" || col.Name == "colOutDueDate")
                    col.DefaultCellStyle.Format = "yyyy-MM-dd";
            }

            DataTable aging = bll.GetSupplierPayableAgingSummary(_supplier_id);
            if (aging != null && aging.Rows.Count > 0)
            {
                DataRow a = aging.Rows[0];
                lblAgeCurrentValue.Text = ToDecimal(a["bucket_current"]).ToString("N2");
                lblAge1_30Value.Text = ToDecimal(a["bucket_1_30"]).ToString("N2");
                lblAge31_60Value.Text = ToDecimal(a["bucket_31_60"]).ToString("N2");
                lblAge61_90Value.Text = ToDecimal(a["bucket_61_90"]).ToString("N2");
                lblAge90PlusValue.Text = ToDecimal(a["bucket_90_plus"]).ToString("N2");
            }
        }

        private void LoadAnalyticsTab()
        {
            DataTable top10 = bll.GetSupplierTopProductsByValue(_supplier_id, 10);
            chartTopProducts.DataSource = top10;
            chartTopProducts.Series["Top"].XValueMember = "item_name";
            chartTopProducts.Series["Top"].YValueMembers = "total_value";
            chartTopProducts.DataBind();

            DataTable trend24 = bll.GetSupplierMonthlySpendTrend(_supplier_id, 24);
            chartMonthlyTrend24.DataSource = trend24;
            chartMonthlyTrend24.Series["Trend"].XValueMember = "month_label";
            chartMonthlyTrend24.Series["Trend"].YValueMembers = "amount";
            chartMonthlyTrend24.DataBind();

            DataTable products = bll.GetSupplierProductsForPriceHistory(_supplier_id);
            cmbPriceProduct.DataSource = null;
            if (products != null)
            {
                cmbPriceProduct.DisplayMember = "item_name";
                cmbPriceProduct.ValueMember = "item_no";
                cmbPriceProduct.DataSource = products;
            }

            if (cmbPriceProduct.Items.Count > 0)
                LoadPriceHistoryForSelected();
            else
                chartPriceHistory.DataSource = null;
        }

        private void LoadPriceHistoryForSelected()
        {
            if (cmbPriceProduct.SelectedValue == null) return;
            string itemNo = Convert.ToString(cmbPriceProduct.SelectedValue);
            if (string.IsNullOrWhiteSpace(itemNo)) return;

            DataTable history = bll.GetSupplierProductPriceHistory(_supplier_id, itemNo);
            chartPriceHistory.DataSource = history;
            chartPriceHistory.Series["Price"].XValueMember = "purchase_date";
            chartPriceHistory.Series["Price"].YValueMembers = "unit_cost";
            chartPriceHistory.DataBind();
        }

        private void gridLedger_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (gridLedger.Rows[e.RowIndex].IsNewRow) return;

            string typeVal = Convert.ToString(gridLedger.Rows[e.RowIndex].Cells["colLedgerType"].Value);
            string refVal = Convert.ToString(gridLedger.Rows[e.RowIndex].Cells["colLedgerRef"].Value);

            if (string.Equals(refVal, "TOTAL", StringComparison.OrdinalIgnoreCase))
            {
                gridLedger.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                gridLedger.Rows[e.RowIndex].DefaultCellStyle.Font = new Font(gridLedger.Font, FontStyle.Bold);
                return;
            }

            if (string.Equals(typeVal, "Bill", StringComparison.OrdinalIgnoreCase))
                gridLedger.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
            else if (string.Equals(typeVal, "Payment", StringComparison.OrdinalIgnoreCase))
                gridLedger.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
            else
                gridLedger.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 249, 196);
        }

        private void btnLoadLedger_Click(object sender, EventArgs e)
        {
            LoadLedgerTab();
        }

        private void btnPrintStatement_Click(object sender, EventArgs e)
        {
            DataTable table = gridLedger.DataSource as DataTable;
            if (table == null || table.Rows.Count == 0)
            {
                UiMessages.ShowWarning("No ledger data to print.", "لا توجد بيانات كشف للطباعة.");
                return;
            }

            var dict = new Dictionary<string, DataTable> { { "Supplier Ledger", table.Copy() } };
            using (var viewer = new DataGridReportViewerForm("Supplier Ledger - " + _supplier_name, dict))
            {
                viewer.ShowDialog(this);
            }
        }

        private void btnMakePayment_Click(object sender, EventArgs e)
        {
            using (var frm = new frm_supplier_payment(null, _supplier_id, _supplier_name))
            {
                frm.ShowDialog(this);
            }
            LoadAllTabs();
        }

        private void cmbPriceProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPriceHistoryForSelected();
        }

        private void tabProfile_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage page = tabProfile.TabPages[e.Index];
            Rectangle rect = e.Bounds;
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            using (Brush b = new SolidBrush(selected ? _teal : Color.White))
                e.Graphics.FillRectangle(b, rect);

            using (Pen p = new Pen(Color.FromArgb(220, 220, 220)))
                e.Graphics.DrawRectangle(p, rect);

            TextRenderer.DrawText(
                e.Graphics,
                page.Text,
                new Font("Segoe UI Semibold", 9F),
                rect,
                selected ? Color.White : Color.FromArgb(50, 49, 48),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void pnlAvatar_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = pnlAvatar.ClientRectangle;
            r.Inflate(-8, -8);

            using (var fill = new SolidBrush(_tealLight))
                e.Graphics.FillEllipse(fill, r);
            using (var pen = new Pen(_teal, 2f))
                e.Graphics.DrawEllipse(pen, r);

            TextRenderer.DrawText(
                e.Graphics,
                _avatarInitials,
                new Font("Segoe UI Semibold", 20F, FontStyle.Bold),
                r,
                _teal,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "?";
            string[] parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0].Substring(0, 1).ToUpperInvariant();
            return (parts[0].Substring(0, 1) + parts[parts.Length - 1].Substring(0, 1)).ToUpperInvariant();
        }

        private string ExtractCategory(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) return "General";
            string[] parts = address.Split(new[] { '|', ',' }, StringSplitOptions.RemoveEmptyEntries);
            string first = parts.Length == 0 ? string.Empty : parts[0].Trim();
            return string.IsNullOrWhiteSpace(first) ? "General" : first;
        }

        private decimal ToDecimal(object value)
        {
            decimal d;
            decimal.TryParse(Convert.ToString(value), out d);
            return d;
        }

        private int ToInt(object value)
        {
            int i;
            int.TryParse(Convert.ToString(value), out i);
            return i;
        }

        private void Btn_ledger_report_Click(object sender, EventArgs e)
        {
            string supplier_id = Convert.ToString(_supplier_id);
            if (!string.IsNullOrWhiteSpace(supplier_id))
            {
                using (BusyScope.Show(this, UiMessages.T("Opening ledger report...", "جاري فتح تقرير كشف الحساب...")))
                {
                    pos.Suppliers.Supplier_Ledger_Report.FrmSupplierLedgerReport obj = new Suppliers.Supplier_Ledger_Report.FrmSupplierLedgerReport(supplier_id);
                    obj.ShowDialog();
                }
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a supplier to view the ledger report.",
                    "يرجى اختيار مورد لعرض تقرير كشف الحساب.",
                    "Supplier",
                    "المورد"
                );
            }
        }

        
    }
}
