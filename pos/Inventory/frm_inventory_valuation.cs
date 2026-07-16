using POS.BLL;
using POS.BLL.Inventory;
using POS.Core;
using POS.Core.Inventory;
using pos.UI;
using pos.UI.Busy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Inventory
{
    public partial class frm_inventory_valuation : Form
    {
        private readonly InventoryValuationBLL _bll = new InventoryValuationBLL();
        private InventoryValuationSettings _settings;

        private List<InventoryValuationLine> _valuationLines;
        private InventoryValuationSummary    _summary;
        private List<COGSLine>               _cogsLines;

        // Status badge colours (Normal/HighCost/ZeroCost/NegativeStock)
        private static readonly Color StatusNormal   = Color.FromArgb(16,  124, 16);
        private static readonly Color StatusHighCost = Color.FromArgb(255, 140, 0);
        private static readonly Color StatusZero     = Color.FromArgb(0,   120, 212);
        private static readonly Color StatusNeg      = Color.FromArgb(209, 52,  56);
        private static readonly Color StatusNegBack  = Color.FromArgb(253, 231, 233);

        // Pie chart palette (top 5)
        private static readonly Color[] PieColours = {
            Color.FromArgb(0,   120, 212),
            Color.FromArgb(16,  124, 16),
            Color.FromArgb(255, 185, 0),
            Color.FromArgb(209, 52,  56),
            Color.FromArgb(0,   120, 136)
        };

        public frm_inventory_valuation()
        {
            InitializeComponent();
        }

        // ===================================================================
        // Form Load
        // ===================================================================

        private void frm_inventory_valuation_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            AppTheme.ApplyListFormStyleLightHeader(panelFilters, null, panelValBody, gridValuation);
            AppTheme.ApplyListFormStyleLightHeader(panelCogsFilters, null, panelCogsBody, gridCogs);

            LoadSettings();
            LoadFilterDropdowns();
        }

        private void LoadSettings()
        {
            try
            {
                _settings = _bll.GetSettings(UsersModal.logged_in_branch_id);
            }
            catch (Exception ex)
            {
                UiMessages.ShowWarning(
                    "Could not load valuation settings. Using defaults. " + ex.Message,
                    "تعذر تحميل إعدادات التقييم. يتم استخدام الإعدادات الافتراضية.");
                _settings = new InventoryValuationSettings { ValuationMethod = "WAC" };
            }
        }

        private void LoadFilterDropdowns()
        {
            try
            {
                // Categories
                var catBll = new CategoriesBLL();
                DataTable cats = catBll.GetAll();
                var allCat = cats.NewRow();
                allCat["code"] = "";
                allCat["name"] = "(All Categories)";
                cats.Rows.InsertAt(allCat, 0);
                cmbCategory.DataSource    = cats;
                cmbCategory.DisplayMember = "name";
                cmbCategory.ValueMember   = "code";

                // Brands
                var brandBll = new BrandsBLL();
                DataTable brands = brandBll.GetAll();
                var allBrand = brands.NewRow();
                allBrand["code"] = "";
                allBrand["name"] = "(All Brands)";
                brands.Rows.InsertAt(allBrand, 0);
                cmbBrand.DataSource    = brands;
                cmbBrand.DisplayMember = "name";
                cmbBrand.ValueMember   = "code";
            }
            catch
            {
                // Dropdowns are optional filters — silently continue
            }
        }

        // ===================================================================
        // Settings
        // ===================================================================

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (var frm = new frm_valuation_settings())
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                    LoadSettings();
            }
        }

        // ===================================================================
        // Valuation Tab — Calculate
        // ===================================================================

        private async void btnCalculate_Click(object sender, EventArgs e)
        {
            btnCalculate.Enabled = false;
            progressBar.Visible  = true;

            try
            {
                DateTime asOf        = dtpAsOfDate.Value.Date;
                string   method      = _settings?.ValuationMethod ?? "WAC";
                string   categoryCode = cmbCategory.SelectedValue?.ToString() ?? "";
                string   brandCode    = cmbBrand.SelectedValue?.ToString() ?? "";
                bool     showZero     = chkShowZero.Checked;
                int      branchId     = UsersModal.logged_in_branch_id;

                var lines = await Task.Run(() =>
                    _bll.CalculateValuation(
                        asOf, method, branchId,
                        categoryCode, brandCode,
                        null, null, showZero));

                var summary = await Task.Run(() => _bll.BuildSummary(lines, branchId));

                _valuationLines = lines;
                _summary        = summary;

                BindValuationGrid(lines);
                UpdateSummaryPanel(summary);
                panelChart.Invalidate();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "Valuation calculation failed: " + ex.Message,
                    "فشل حساب التقييم: " + ex.Message);
            }
            finally
            {
                btnCalculate.Enabled = true;
                progressBar.Visible  = false;
            }
        }

        private void BindValuationGrid(List<InventoryValuationLine> lines)
        {
            gridValuation.Rows.Clear();
            gridValuation.SuspendLayout();
            try
            {
                string numFmt = "N2";
                string qtyFmt = "N3";

                foreach (var l in lines)
                {
                    int idx = gridValuation.Rows.Add(
                        l.Code,
                        l.Name,
                        l.CategoryCode,
                        l.CurrentQty.ToString(qtyFmt),
                        l.UnitCost.ToString(numFmt),
                        l.TotalValue.ToString(numFmt),
                        l.LastPurchaseDate.HasValue ? l.LastPurchaseDate.Value.ToString("dd-MMM-yy") : "",
                        l.LastPurchaseCost.ToString(numFmt),
                        l.ReorderLevel.ToString(qtyFmt),
                        FormatStatus(l.ValuationStatus));

                    // Tag the row with status for CellFormatting
                    gridValuation.Rows[idx].Tag = l.ValuationStatus;
                }
            }
            finally
            {
                gridValuation.ResumeLayout();
            }
        }

        private void UpdateSummaryPanel(InventoryValuationSummary s)
        {
            lblTotalValueVal.Text = s.TotalInventoryValue.ToString("N2");
            lblTotalSkuVal.Text   = s.TotalSKUs.ToString("N0");
            lblTotalQtyVal.Text   = s.TotalQty.ToString("N2");
            lblAvgCostVal.Text    = s.AvgCostPerUnit.ToString("N4");
        }

        // ===================================================================
        // Pie Chart (GDI+) — top 5 categories
        // ===================================================================

        private void panelChart_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (_summary == null || _summary.TopCategoriesByValue == null ||
                _summary.TopCategoriesByValue.Count == 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var items = _summary.TopCategoriesByValue;
            Rectangle pieRect = new Rectangle(8, 8, 160, 160);
            float startAngle  = -90f;
            float total       = items.Sum(x => (float)x.TotalValue);
            if (total <= 0) return;

            for (int i = 0; i < items.Count; i++)
            {
                float sweep = (float)(items[i].TotalValue / (decimal)total * 360m);
                using (SolidBrush br = new SolidBrush(PieColours[i % PieColours.Length]))
                    g.FillPie(br, pieRect, startAngle, sweep);
                startAngle += sweep;
            }

            // Legend
            int legendX = 180;
            int legendY = 8;
            for (int i = 0; i < items.Count; i++)
            {
                using (SolidBrush br = new SolidBrush(PieColours[i % PieColours.Length]))
                    g.FillRectangle(br, legendX, legendY + i * 26, 14, 14);

                string label = $"{items[i].CategoryName} ({items[i].TotalValue:N0})";
                g.DrawString(label, AppTheme.FontSmall, Brushes.Black,
                    legendX + 18, legendY + i * 26 - 1);
            }
        }

        // ===================================================================
        // Valuation Grid — CellFormatting (status badges)
        // ===================================================================

        private void gridValuation_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = gridValuation.Rows[e.RowIndex];
            string status = row.Tag as string ?? "Normal";

            switch (status)
            {
                case "NegativeStock":
                    row.DefaultCellStyle.BackColor = StatusNegBack;
                    if (e.ColumnIndex == colStatus.Index)
                    {
                        e.CellStyle.ForeColor = StatusNeg;
                        e.CellStyle.Font      = AppTheme.FontSemiBold;
                    }
                    break;
                case "ZeroCost":
                    if (e.ColumnIndex == colStatus.Index)
                        e.CellStyle.ForeColor = StatusZero;
                    break;
                case "HighCost":
                    if (e.ColumnIndex == colStatus.Index)
                        e.CellStyle.ForeColor = StatusHighCost;
                    break;
            }
        }

        // ===================================================================
        // Snapshot
        // ===================================================================

        private async void btnSnapshot_Click(object sender, EventArgs e)
        {
            if (UiMessages.ConfirmYesNo(
                "Take a valuation snapshot for today? This is used for historical Balance Sheet reporting.",
                "هل تريد أخذ لقطة تقييم لليوم؟ تستخدم للتقارير التاريخية لقائمة المركز المالي.") != DialogResult.Yes)
                return;

            using (BusyScope.Show(this, "Taking inventory snapshot..."))
            {
                try
                {
                    string method   = _settings?.ValuationMethod ?? "WAC";
                    int branchId    = UsersModal.logged_in_branch_id;
                    DateTime snapDt = DateTime.Today;

                    DataTable result = await Task.Run(() =>
                        _bll.TakeSnapshot(snapDt, branchId, method));

                    if (result != null && result.Rows.Count > 0)
                    {
                        decimal totalVal = result.Rows[0]["total_inventory_value"] == DBNull.Value
                                           ? 0 : Convert.ToDecimal(result.Rows[0]["total_inventory_value"]);
                        int skuCnt       = result.Rows[0]["total_sku_count"] == DBNull.Value
                                           ? 0 : Convert.ToInt32(result.Rows[0]["total_sku_count"]);

                        UiMessages.ShowInfo(
                            $"Snapshot taken successfully.\nTotal Value: {totalVal:N2} | SKUs: {skuCnt:N0}",
                            $"تم أخذ اللقطة بنجاح.\nإجمالي القيمة: {totalVal:N2} | المنتجات: {skuCnt:N0}");
                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError("Snapshot failed: " + ex.Message, "فشلت اللقطة: " + ex.Message);
                }
            }
        }

        // ===================================================================
        // Export (CSV)
        // ===================================================================

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_valuationLines == null || _valuationLines.Count == 0)
            {
                UiMessages.ShowWarning("Calculate valuation first.", "احسب التقييم أولاً.");
                return;
            }

            using (SaveFileDialog dlg = new SaveFileDialog
            {
                Filter   = "CSV Files|*.csv",
                FileName = $"InventoryValuation_{DateTime.Today:yyyyMMdd}.csv"
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Code,Product Name,Category,Qty on Hand,Unit Cost,Total Value,Last Purchase Date,Last Purchase Cost,Reorder Level,Status");
                    foreach (var l in _valuationLines)
                    {
                        sb.AppendLine(string.Join(",",
                            Csv(l.Code), Csv(l.Name), Csv(l.CategoryCode),
                            l.CurrentQty.ToString("N3"), l.UnitCost.ToString("N4"),
                            l.TotalValue.ToString("N2"),
                            l.LastPurchaseDate.HasValue ? l.LastPurchaseDate.Value.ToString("dd-MMM-yy") : "",
                            l.LastPurchaseCost.ToString("N4"),
                            l.ReorderLevel.ToString("N3"),
                            l.ValuationStatus));
                    }
                    System.IO.File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
                    UiMessages.ShowInfo("Exported to " + dlg.FileName, "تم التصدير إلى " + dlg.FileName);
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError("Export failed: " + ex.Message, "فشل التصدير: " + ex.Message);
                }
            }
        }

        // ===================================================================
        // COGS Tab — Calculate
        // ===================================================================

        private async void btnCalcCogs_Click(object sender, EventArgs e)
        {
            if (dtpFromDate.Value.Date > dtpToDate.Value.Date)
            {
                UiMessages.ShowWarning("From date must be before To date.", "يجب أن يكون تاريخ البداية قبل تاريخ النهاية.");
                return;
            }

            btnCalcCogs.Enabled = false;
            btnPostCogs.Enabled = false;

            try
            {
                DateTime from  = dtpFromDate.Value.Date;
                DateTime to    = dtpToDate.Value.Date;
                string method  = _settings?.ValuationMethod ?? "WAC";
                int branchId   = UsersModal.logged_in_branch_id;

                using (BusyScope.Show(this, "Calculating COGS..."))
                {
                    _cogsLines = await Task.Run(() => _bll.GetCOGSLines(from, to, branchId, method));
                }

                BindCogsGrid(_cogsLines);
                UpdateCogsTotals(_cogsLines);

                btnPostCogs.Enabled = _cogsLines != null && _cogsLines.Count > 0
                                      && (_settings?.CogsAccountId ?? 0) > 0
                                      && (_settings?.InventoryAccountId ?? 0) > 0;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("COGS calculation failed: " + ex.Message, "فشل حساب تكلفة البضائع: " + ex.Message);
            }
            finally
            {
                btnCalcCogs.Enabled = true;
            }
        }

        private void BindCogsGrid(List<COGSLine> lines)
        {
            gridCogs.Rows.Clear();
            gridCogs.SuspendLayout();
            try
            {
                foreach (var l in lines)
                {
                    decimal costPerUnit = l.SoldQty > 0 ? l.CogsValue / l.SoldQty : 0;
                    int idx = gridCogs.Rows.Add(
                        l.Code, l.Name, l.CategoryCode,
                        l.SoldQty.ToString("N3"),
                        costPerUnit.ToString("N4"),
                        l.CogsValue.ToString("N2"),
                        l.SalesValue.ToString("N2"),
                        l.GrossMargin.ToString("N2"),
                        l.ReconciliationVariance.ToString("N2"));

                    gridCogs.Rows[idx].Tag = l;
                }
            }
            finally
            {
                gridCogs.ResumeLayout();
            }
        }

        private void UpdateCogsTotals(List<COGSLine> lines)
        {
            decimal totalCogs   = lines.Sum(l => l.CogsValue);
            decimal totalSales  = lines.Sum(l => l.SalesValue);
            decimal grossMargin = totalSales - totalCogs;
            double  marginPct   = totalSales > 0 ? (double)(grossMargin / totalSales) * 100 : 0;

            lblTotalCogsVal.Text    = totalCogs.ToString("N2");
            lblTotalSalesVal.Text   = totalSales.ToString("N2");
            lblGrossMarginVal.Text  = $"{grossMargin:N2}  ({marginPct:N1}%)";
        }

        private void gridCogs_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = gridCogs.Rows[e.RowIndex];
            if (!(row.Tag is COGSLine line)) return;

            // Highlight reconciliation variance
            if (e.ColumnIndex == colVariance.Index && Math.Abs(line.ReconciliationVariance) > 1m)
                e.CellStyle.ForeColor = StatusHighCost;

            // Highlight negative gross margin
            if (e.ColumnIndex == colGrossMargin.Index && line.GrossMargin < 0)
                e.CellStyle.ForeColor = StatusNeg;
        }

        // ===================================================================
        // COGS Tab — Post Journal Entry
        // ===================================================================

        private void btnPostCogs_Click(object sender, EventArgs e)
        {
            if (_cogsLines == null || _cogsLines.Count == 0)
            {
                UiMessages.ShowWarning("Calculate COGS first.", "احسب تكلفة البضائع أولاً.");
                return;
            }

            if (_settings == null || (_settings.CogsAccountId ?? 0) == 0 || (_settings.InventoryAccountId ?? 0) == 0)
            {
                UiMessages.ShowWarning(
                    "COGS and Inventory accounts must be configured in Settings before posting.",
                    "يجب تكوين حسابات تكلفة البضائع والمخزون في الإعدادات قبل الترحيل.");
                return;
            }

            decimal totalCogs = _cogsLines.Sum(l => l.CogsValue);
            if (totalCogs <= 0)
            {
                UiMessages.ShowWarning("Total COGS is zero. Nothing to post.", "إجمالي تكلفة البضائع صفر. لا يوجد شيء للترحيل.");
                return;
            }

            string fromStr = dtpFromDate.Value.ToString("dd-MMM-yy");
            string toStr   = dtpToDate.Value.ToString("dd-MMM-yy");

            if (UiMessages.ConfirmYesNo(
                $"Post COGS journal entry?\n\nPeriod: {fromStr} to {toStr}\nTotal COGS: {totalCogs:N2}\nDR: {_settings.CogsAccountName}\nCR: {_settings.InventoryAccountName}",
                $"هل تريد ترحيل قيد تكلفة البضائع؟\nالفترة: {fromStr} إلى {toStr}\nإجمالي تكلفة البضائع: {totalCogs:N2}") != DialogResult.Yes)
                return;

            using (BusyScope.Show(this, "Posting COGS journal entry..."))
            {
                try
                {
                    var request = new COGSPostingRequest
                    {
                        FromDate            = dtpFromDate.Value.Date,
                        ToDate              = dtpToDate.Value.Date,
                        CogsAccountId       = _settings.CogsAccountId.Value,
                        InventoryAccountId  = _settings.InventoryAccountId.Value,
                        TotalCogs           = totalCogs,
                        PostPerProduct      = _settings.PostPerProduct,
                        Lines               = _cogsLines,
                        Narration           = $"COGS posting for period {fromStr} to {toStr} — {_settings.ValuationMethod}"
                    };

                    int voucherId = _bll.PostCOGSEntry(request);

                    UiMessages.ShowInfo(
                        $"COGS journal entry posted successfully (Voucher ID: {voucherId}).",
                        $"تم ترحيل قيد تكلفة البضائع بنجاح (رقم القيد: {voucherId}).");

                    btnPostCogs.Enabled = false;
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError("Post failed: " + ex.Message, "فشل الترحيل: " + ex.Message);
                }
            }
        }

        // ===================================================================
        // Helpers
        // ===================================================================

        private static string FormatStatus(string status)
        {
            switch (status)
            {
                case "HighCost":     return "⚠ High Cost";
                case "ZeroCost":     return "⚪ Zero Cost";
                case "NegativeStock":return "✘ Negative Stock";
                default:             return "✔ Normal";
            }
        }

        private static string Csv(string v)
        {
            if (v == null) return "";
            return v.Contains(",") ? "\"" + v.Replace("\"", "\"\"") + "\"" : v;
        }
    }
}
