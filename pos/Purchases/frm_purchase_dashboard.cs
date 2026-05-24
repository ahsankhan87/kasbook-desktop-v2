using POS.BLL;
using pos.UI;
using pos.UI.Busy;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using WinFormsChart = System.Windows.Forms.DataVisualization.Charting;

namespace pos
{
    public partial class frm_purchase_dashboard : Form
    {
        private readonly Color _primaryGreen = Color.FromArgb(46, 125, 50);
        private readonly Color _darkGreen = Color.FromArgb(27, 94, 32);
        private readonly Color _pageBg = Color.FromArgb(243, 244, 246);

        private DateTime _fromDate;
        private DateTime _toDate;
        private DateTime _prevFromDate;
        private DateTime _prevToDate;

        public frm_purchase_dashboard()
        {
            InitializeComponent();
        }

        private void frm_purchase_dashboard_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleDashboard();
            ConfigureCharts();
            ConfigureGrids();
            LoadPeriodOptions();
            cmbPeriod.SelectedIndex = 2;
            LoadDashboard();
        }

        private void StyleDashboard()
        {
            BackColor = _pageBg;
            panelRoot.BackColor = _pageBg;

            StyleKpiCard(cardTotalPurchases);
            StyleKpiCard(cardTotalBills);
            StyleKpiCard(cardAmountPaid);
            StyleKpiCard(cardPayable);
            StyleKpiCard(cardAvgPurchase);

            StyleContainerCard(cardMonthlyBar);
            StyleContainerCard(cardSupplierDonut);
            StyleContainerCard(cardTrend);
            StyleContainerCard(cardTopSuppliers);
            StyleContainerCard(cardPendingBills);
            StyleContainerCard(panelFilters);

            lblTitle.ForeColor = AppTheme.TextPrimary;
        }

        private void StyleKpiCard(Panel panel)
        {
            panel.BackColor = Color.White;
            panel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(230, 230, 230)))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
                }

                using (var b = new SolidBrush(_primaryGreen))
                {
                    e.Graphics.FillRectangle(b, 0, 0, 4, panel.Height);
                }
            };
        }

        private void StyleContainerCard(Panel panel)
        {
            panel.BackColor = Color.White;
            panel.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(230, 230, 230)))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
                }
            };
        }

        private void ConfigureCharts()
        {
            ConfigureMonthlyBarChart();
            ConfigureSupplierDonut();
            ConfigureTrendChart();
        }

        private void ConfigureMonthlyBarChart()
        {
            var area = chartMonthlyPurchases.ChartAreas[0];
            area.BackColor = Color.White;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -35;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);

            var series = chartMonthlyPurchases.Series[0];
            series.Name = "Monthly";
            series.ChartType = WinFormsChart.SeriesChartType.Column;
            series.IsValueShownAsLabel = false;
            series.Color = _primaryGreen;
            series.BorderWidth = 1;
        }

        private void ConfigureSupplierDonut()
        {
            var area = chartSupplierDonut.ChartAreas[0];
            area.BackColor = Color.White;

            var series = chartSupplierDonut.Series[0];
            series.Name = "Suppliers";
            series.ChartType = WinFormsChart.SeriesChartType.Doughnut;
            series["DoughnutRadius"] = "55";
            series.Label = "#PERCENT{P0}";
            series.LegendText = "#VALX";
            series.IsValueShownAsLabel = true;
            chartSupplierDonut.Legends[0].Font = new Font("Segoe UI", 8F);
        }

        private void ConfigureTrendChart()
        {
            chartTrend.Series.Clear();

            var area = chartTrend.ChartAreas[0];
            area.BackColor = Color.White;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8F);

            var current = new WinFormsChart.Series("This Year");
            current.ChartType = WinFormsChart.SeriesChartType.Line;
            current.BorderWidth = 3;
            current.Color = _primaryGreen;
            current.MarkerStyle = WinFormsChart.MarkerStyle.Circle;
            current.MarkerSize = 5;

            var last = new WinFormsChart.Series("Last Year");
            last.ChartType = WinFormsChart.SeriesChartType.Line;
            last.BorderWidth = 2;
            last.Color = Color.DimGray;
            last.BorderDashStyle = WinFormsChart.ChartDashStyle.Dash;
            last.MarkerStyle = WinFormsChart.MarkerStyle.None;

            chartTrend.Series.Add(current);
            chartTrend.Series.Add(last);
            chartTrend.Legends[0].Font = new Font("Segoe UI", 8F);
        }

        private void ConfigureGrids()
        {
            ConfigureGrid(gridTopSuppliers);
            ConfigureGrid(gridPendingBills);
        }

        private void ConfigureGrid(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 244, 240);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.75F);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 8.75F);
            grid.DefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(223, 240, 216);
            grid.DefaultCellStyle.SelectionForeColor = AppTheme.TextPrimary;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            grid.RowTemplate.Height = 30;
        }

        private void LoadPeriodOptions()
        {
            cmbPeriod.Items.Clear();
            cmbPeriod.Items.Add("Today");
            cmbPeriod.Items.Add("This Week");
            cmbPeriod.Items.Add("This Month");
            cmbPeriod.Items.Add("This Quarter");
            cmbPeriod.Items.Add("This Year");
            cmbPeriod.Items.Add("Custom Range");
        }

        private void cmbPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool custom = string.Equals(Convert.ToString(cmbPeriod.SelectedItem), "Custom Range", StringComparison.OrdinalIgnoreCase);
            dtpFrom.Enabled = custom;
            dtpTo.Enabled = custom;

            if (!custom)
            {
                LoadDashboard();
            }
        }

        private void dtpFromTo_ValueChanged(object sender, EventArgs e)
        {
            if (string.Equals(Convert.ToString(cmbPeriod.SelectedItem), "Custom Range", StringComparison.OrdinalIgnoreCase))
            {
                LoadDashboard();
            }
        }

        private void ResolveDatesFromPeriod()
        {
            DateTime today = DateTime.Today;
            string period = Convert.ToString(cmbPeriod.SelectedItem);

            if (string.Equals(period, "Today", StringComparison.OrdinalIgnoreCase))
            {
                _fromDate = today;
                _toDate = today;
                _prevFromDate = today.AddDays(-1);
                _prevToDate = today.AddDays(-1);
            }
            else if (string.Equals(period, "This Week", StringComparison.OrdinalIgnoreCase))
            {
                int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                _fromDate = today.AddDays(-diff);
                _toDate = today;
                int len = (_toDate - _fromDate).Days + 1;
                _prevToDate = _fromDate.AddDays(-1);
                _prevFromDate = _prevToDate.AddDays(-(len - 1));
            }
            else if (string.Equals(period, "This Month", StringComparison.OrdinalIgnoreCase))
            {
                _fromDate = new DateTime(today.Year, today.Month, 1);
                _toDate = today;
                DateTime prevMonth = _fromDate.AddMonths(-1);
                _prevFromDate = new DateTime(prevMonth.Year, prevMonth.Month, 1);
                _prevToDate = _prevFromDate.AddMonths(1).AddDays(-1);
            }
            else if (string.Equals(period, "This Quarter", StringComparison.OrdinalIgnoreCase))
            {
                int quarter = ((today.Month - 1) / 3) + 1;
                int startMonth = ((quarter - 1) * 3) + 1;
                _fromDate = new DateTime(today.Year, startMonth, 1);
                _toDate = today;
                _prevFromDate = _fromDate.AddMonths(-3);
                _prevToDate = _fromDate.AddDays(-1);
            }
            else if (string.Equals(period, "This Year", StringComparison.OrdinalIgnoreCase))
            {
                _fromDate = new DateTime(today.Year, 1, 1);
                _toDate = today;
                _prevFromDate = new DateTime(today.Year - 1, 1, 1);
                _prevToDate = new DateTime(today.Year - 1, 12, 31);
            }
            else
            {
                _fromDate = dtpFrom.Value.Date;
                _toDate = dtpTo.Value.Date;
                if (_toDate < _fromDate)
                {
                    DateTime temp = _fromDate;
                    _fromDate = _toDate;
                    _toDate = temp;
                }

                int len = (_toDate - _fromDate).Days + 1;
                _prevToDate = _fromDate.AddDays(-1);
                _prevFromDate = _prevToDate.AddDays(-(len - 1));
            }

            dtpFrom.Value = _fromDate;
            dtpTo.Value = _toDate;
        }

        private void LoadDashboard()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading purchase dashboard...", "جاري تحميل لوحة المشتريات...")))
                {
                    ResolveDatesFromPeriod();

                    PurchasesBLL bll = new PurchasesBLL();

                    DataTable kpi = bll.GetPurchaseDashboardKpis(_fromDate, _toDate, _prevFromDate, _prevToDate);
                    BindKpis(kpi);

                    BindMonthlyBar(bll.GetPurchaseDashboardMonthlyPurchases(12, _toDate));
                    BindSupplierDonut(bll.GetPurchaseDashboardSupplierSplit(_fromDate, _toDate, 5));
                    BindTrend(bll.GetPurchaseDashboardYearlyTrend(_toDate.Year));

                    BindTopSuppliers(bll.GetPurchaseDashboardTopSuppliers(_fromDate, _toDate, 10));
                    BindPendingBills(bll.GetPurchaseDashboardPendingBills(_fromDate, _toDate, 50));
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Purchase Dashboard", "لوحة المشتريات");
            }
        }

        private void BindKpis(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                lblTotalPurchasesValue.Text = 0m.ToString("N2");
                lblTotalPurchasesTrend.Text = "0.00% vs last period";
                lblTotalBillsValue.Text = "0";
                lblAmountPaidValue.Text = 0m.ToString("N2");
                lblPayableValue.Text = 0m.ToString("N2");
                lblPayableSub.Text = "Overdue: 0.00";
                lblAvgPurchaseValue.Text = 0m.ToString("N2");
                return;
            }

            DataRow r = dt.Rows[0];
            decimal totalPurchases = ToDecimal(r["total_purchases"]);
            decimal prev = ToDecimal(r["total_purchases_prev"]);
            int totalBills = ToInt(r["total_bills"]);
            decimal amountPaid = ToDecimal(r["amount_paid"]);
            decimal payable = ToDecimal(r["payable_outstanding"]);
            decimal overdue = ToDecimal(r["overdue_outstanding"]);
            decimal avg = ToDecimal(r["avg_purchase_value"]);

            decimal trend = prev == 0 ? (totalPurchases == 0 ? 0 : 100) : ((totalPurchases - prev) / prev) * 100m;

            lblTotalPurchasesValue.Text = totalPurchases.ToString("N2");
            lblTotalPurchasesTrend.Text = string.Format("{0}{1:N2}% vs last period", trend >= 0 ? "+" : string.Empty, trend);
            lblTotalPurchasesTrend.ForeColor = trend >= 0 ? _primaryGreen : Color.Firebrick;

            lblTotalBillsValue.Text = totalBills.ToString("N0");
            lblAmountPaidValue.Text = amountPaid.ToString("N2");
            lblPayableValue.Text = payable.ToString("N2");
            lblPayableSub.Text = "Overdue: " + overdue.ToString("N2");
            lblPayableValue.ForeColor = overdue > 0 ? Color.Firebrick : AppTheme.TextPrimary;
            lblAvgPurchaseValue.Text = avg.ToString("N2");
        }

        private void BindMonthlyBar(DataTable dt)
        {
            var series = chartMonthlyPurchases.Series[0];
            series.Points.Clear();

            int currentYear = DateTime.Today.Year;
            int currentMonth = DateTime.Today.Month;

            foreach (DataRow row in dt.Rows)
            {
                string label = Convert.ToString(row["month_label"]);
                decimal amount = ToDecimal(row["amount"]);
                int monthNo = ToInt(row["month_no"]);
                int yearNo = ToInt(row["year_no"]);

                int p = series.Points.AddXY(label, amount);
                series.Points[p].Color = (monthNo == currentMonth && yearNo == currentYear) ? _darkGreen : _primaryGreen;
                series.Points[p].ToolTip = label + ": " + amount.ToString("N2", CultureInfo.InvariantCulture);
            }
        }

        private void BindSupplierDonut(DataTable dt)
        {
            var series = chartSupplierDonut.Series[0];
            series.Points.Clear();

            Color[] palette =
            {
                Color.FromArgb(46,125,50),
                Color.FromArgb(56,142,60),
                Color.FromArgb(76,175,80),
                Color.FromArgb(102,187,106),
                Color.FromArgb(129,199,132),
                Color.FromArgb(189,189,189)
            };

            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                string supplier = Convert.ToString(row["supplier_name"]);
                decimal amount = ToDecimal(row["total_amount"]);
                int p = series.Points.AddXY(supplier, amount);
                series.Points[p].Color = palette[i % palette.Length];
                i++;
            }
        }

        private void BindTrend(DataTable dt)
        {
            var current = chartTrend.Series["This Year"];
            var last = chartTrend.Series["Last Year"];
            current.Points.Clear();
            last.Points.Clear();

            foreach (DataRow row in dt.Rows)
            {
                string month = Convert.ToString(row["month_name"]);
                decimal cur = ToDecimal(row["current_year_amount"]);
                decimal prev = ToDecimal(row["last_year_amount"]);

                current.Points.AddXY(month, cur);
                last.Points.AddXY(month, prev);
            }
        }

        private void BindTopSuppliers(DataTable dt)
        {
            gridTopSuppliers.DataSource = dt;
            if (gridTopSuppliers.Columns.Count == 0) return;

            if (gridTopSuppliers.Columns.Contains("colSupplierTotal"))
                gridTopSuppliers.Columns["colSupplierTotal"].DefaultCellStyle.Format = "N2";
            if (gridTopSuppliers.Columns.Contains("colSupplierPayable"))
                gridTopSuppliers.Columns["colSupplierPayable"].DefaultCellStyle.Format = "N2";
            if (gridTopSuppliers.Columns.Contains("colSupplierShare"))
                gridTopSuppliers.Columns["colSupplierShare"].DefaultCellStyle.Format = "N2'%'";
        }

        private void BindPendingBills(DataTable dt)
        {
            gridPendingBills.DataSource = dt;
            if (gridPendingBills.Columns.Count == 0) return;

            if (gridPendingBills.Columns.Contains("colBillDate"))
                gridPendingBills.Columns["colBillDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
            if (gridPendingBills.Columns.Contains("colDueDate"))
                gridPendingBills.Columns["colDueDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
            if (gridPendingBills.Columns.Contains("colBillAmount"))
                gridPendingBills.Columns["colBillAmount"].DefaultCellStyle.Format = "N2";
        }

        private void gridPendingBills_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= gridPendingBills.Rows.Count) return;
            DataGridViewRow row = gridPendingBills.Rows[e.RowIndex];
            if (row.IsNewRow) return;

            int days = ToInt(row.Cells["colDaysOverdue"].Value);
            if (days > 0)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
                row.DefaultCellStyle.ForeColor = Color.Firebrick;
            }
        }

        private decimal ToDecimal(object value)
        {
            decimal d;
            decimal.TryParse(Convert.ToString(value), NumberStyles.Any, CultureInfo.InvariantCulture, out d);
            return d;
        }

        private int ToInt(object value)
        {
            int i;
            int.TryParse(Convert.ToString(value), out i);
            return i;
        }
    }
}
