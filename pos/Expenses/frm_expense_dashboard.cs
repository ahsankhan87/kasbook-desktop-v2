using pos.UI;
using POS.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinFormsChart = System.Windows.Forms.DataVisualization.Charting;
using pos.UI.Busy;

namespace pos.Expenses
{
    public partial class frm_expense_dashboard : Form
    {
        public frm_expense_dashboard()
        {
            InitializeComponent();
        }

        private void frm_expense_dashboard_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleDashboard();
            LoadPeriodOptions();
            ConfigureCharts();
            ConfigureGrids();
            cmbPeriod.SelectedIndex = 1;
            LoadDashboard();
        }

        private void StyleDashboard()
        {
            BackColor = Color.FromArgb(245, 247, 250);
            panelFilters.BackColor = BackColor;
            panelKpis.BackColor = BackColor;
            panelContent.BackColor = BackColor;

            StyleCard(cardToday, Color.FromArgb(0, 120, 212));
            StyleCard(cardMonth, Color.FromArgb(0, 153, 188));
            StyleCard(cardYear, Color.FromArgb(255, 185, 0));
            StyleCard(cardPending, Color.FromArgb(255, 99, 71));
            StyleCard(cardMonthlyChart, AppTheme.Border);
            StyleCard(cardPieChart, AppTheme.Border);
            StyleCard(cardRecent, AppTheme.Border);
            StyleCard(cardTopAccounts, AppTheme.Border);
        }

        private void StyleCard(Panel panel, Color accent)
        {
            panel.BackColor = Color.White;
            panel.Paint += (s, e) =>
            {
                using (var shadow = new SolidBrush(Color.FromArgb(18, 0, 0, 0)))
                {
                    e.Graphics.FillRectangle(shadow, 4, 4, panel.Width - 6, panel.Height - 6);
                }

                using (var bg = new SolidBrush(Color.White))
                {
                    e.Graphics.FillRectangle(bg, 0, 0, panel.Width - 6, panel.Height - 6);
                }

                using (var pen = new Pen(Color.FromArgb(230, 233, 238), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 7, panel.Height - 7);
                }

                using (var accentBrush = new SolidBrush(accent))
                {
                    e.Graphics.FillRectangle(accentBrush, 0, 0, 6, panel.Height - 6);
                }
            };
        }

        private void LoadPeriodOptions()
        {
            cmbPeriod.Items.Clear();
            cmbPeriod.Items.Add("This Week");
            cmbPeriod.Items.Add("This Month");
            cmbPeriod.Items.Add("This Year");
            cmbPeriod.Items.Add("Custom Range");
        }

        private void ConfigureCharts()
        {
            ConfigureChart(chartMonthly, WinFormsChart.SeriesChartType.Column);
            ConfigureChart(chartBreakdown, WinFormsChart.SeriesChartType.Pie);
        }

        private void ConfigureChart(WinFormsChart.Chart chart, WinFormsChart.SeriesChartType chartType)
        {
            chart.BackColor = Color.White;
            chart.BorderlineColor = Color.White;
            chart.Legends.Clear();
            chart.Series.Clear();
            chart.ChartAreas.Clear();

            var area = new WinFormsChart.ChartArea("Main");
            area.BackColor = Color.White;
            area.AxisX.LineColor = Color.FromArgb(220, 224, 230);
            area.AxisY.LineColor = Color.FromArgb(220, 224, 230);
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(237, 240, 244);
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 8.5F);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 8.5F);
            chart.ChartAreas.Add(area);

            var series = new WinFormsChart.Series("Series");
            series.ChartArea = "Main";
            series.ChartType = chartType;
            series.IsValueShownAsLabel = chartType != WinFormsChart.SeriesChartType.Pie;
            series.Font = new Font("Segoe UI", 8F);
            chart.Series.Add(series);

            var legend = new WinFormsChart.Legend();
            legend.Docking = WinFormsChart.Docking.Bottom;
            legend.Font = new Font("Segoe UI", 8.5F);
            chart.Legends.Add(legend);
        }

        private void ConfigureGrids()
        {
            ConfigureGrid(gridRecentTransactions);
            ConfigureGrid(gridTopAccounts);
        }

        private void ConfigureGrid(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(244, 247, 251);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            grid.DefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(222, 236, 249);
            grid.DefaultCellStyle.SelectionForeColor = AppTheme.TextPrimary;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 252);
            grid.RowTemplate.Height = 30;
        }

        private void cmbPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool custom = string.Equals(Convert.ToString(cmbPeriod.SelectedItem), "Custom Range", StringComparison.OrdinalIgnoreCase);
            dtpFrom.Enabled = custom;
            dtpTo.Enabled = custom;
            if (!custom)
            {
                ApplyPeriodSelection();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            try
            {
                // Apply busyUI if loading takes long time due to large data
                using (BusyScope.Show(this, UiMessages.T("Loading...", "جاري التحميل...")))
                {
                    ApplyPeriodSelection();

                    var bll = new ExpenseBLL();
                    DateTime fromDate = dtpFrom.Value.Date;
                    DateTime toDate = dtpTo.Value.Date;

                    decimal todayTotal = bll.GetExpenseTotal(DateTime.Today, DateTime.Today);
                    decimal yesterdayTotal = bll.GetExpenseTotal(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1));
                    decimal monthTotal = bll.GetExpenseTotal(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Today);
                    DateTime prevMonthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
                    DateTime prevMonthEnd = prevMonthStart.AddMonths(1).AddDays(-1);
                    decimal prevMonthTotal = bll.GetExpenseTotal(prevMonthStart, prevMonthEnd);
                    decimal yearTotal = bll.GetExpenseTotal(new DateTime(DateTime.Today.Year, 1, 1), DateTime.Today);
                    DateTime prevYearStart = new DateTime(DateTime.Today.Year - 1, 1, 1);
                    DateTime prevYearEnd = new DateTime(DateTime.Today.Year - 1, 12, 31);
                    decimal prevYearTotal = bll.GetExpenseTotal(prevYearStart, prevYearEnd);
                    decimal pendingTotal = bll.GetPendingExpenseTotal();
                    int pendingCount = bll.GetPendingExpenseCount();

                    lblTodayAmount.Text = todayTotal.ToString("N2");
                    lblMonthAmount.Text = monthTotal.ToString("N2");
                    lblYearAmount.Text = yearTotal.ToString("N2");
                    lblPendingAmount.Text = pendingTotal.ToString("N2");

                    lblTodayTrend.Text = BuildTrend(todayTotal, yesterdayTotal, "vs yesterday");
                    lblMonthTrend.Text = BuildTrend(monthTotal, prevMonthTotal, "vs last month");
                    lblYearTrend.Text = BuildTrend(yearTotal, prevYearTotal, "vs last year");
                    lblPendingTrend.Text = pendingCount.ToString() + " pending vouchers";

                    BindMonthlyChart(bll.GetExpenseMonthlyComparison(DateTime.Today.Year));
                    BindBreakdownChart(bll.GetExpenseBreakdown(fromDate, toDate));
                    BindRecentTransactions(bll.GetRecentExpenses(10));
                    BindTopAccounts(bll.GetTopExpenseAccounts(fromDate, toDate, 10));
                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private string BuildTrend(decimal current, decimal previous, string suffix)
        {
            if (previous == 0)
            {
                return current == 0 ? "0.0% " + suffix : "+100.0% " + suffix;
            }

            decimal change = ((current - previous) / previous) * 100m;
            return string.Format("{0}{1:N1}% {2}", change >= 0 ? "+" : string.Empty, change, suffix);
        }

        private void ApplyPeriodSelection()
        {
            string period = Convert.ToString(cmbPeriod.SelectedItem);
            if (string.Equals(period, "This Week", StringComparison.OrdinalIgnoreCase))
            {
                int diff = (7 + (DateTime.Today.DayOfWeek - DayOfWeek.Monday)) % 7;
                dtpFrom.Value = DateTime.Today.AddDays(-1 * diff);
                dtpTo.Value = DateTime.Today;
            }
            else if (string.Equals(period, "This Month", StringComparison.OrdinalIgnoreCase))
            {
                dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                dtpTo.Value = DateTime.Today;
            }
            else if (string.Equals(period, "This Year", StringComparison.OrdinalIgnoreCase))
            {
                dtpFrom.Value = new DateTime(DateTime.Today.Year, 1, 1);
                dtpTo.Value = DateTime.Today;
            }
        }

        private void BindMonthlyChart(DataTable data)
        {
            var series = chartMonthly.Series[0];
            series.Points.Clear();
            series.Color = Color.FromArgb(0, 120, 212);
            series.BackSecondaryColor = Color.FromArgb(0, 153, 188);
            series.BackGradientStyle = WinFormsChart.GradientStyle.TopBottom;
            chartMonthly.Legends[0].Enabled = false;

            string[] months = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            for (int i = 1; i <= 12; i++)
            {
                decimal value = 0;
                var row = data.AsEnumerable().FirstOrDefault(x => Convert.ToInt32(x["MonthNo"]) == i);
                if (row != null)
                {
                    value = Convert.ToDecimal(row["TotalAmount"]);
                }

                int pointIndex = series.Points.AddXY(months[i - 1], value);
                series.Points[pointIndex].Color = Color.FromArgb(0, 120, 212);
            }
        }

        private void BindBreakdownChart(DataTable data)
        {
            var series = chartBreakdown.Series[0];
            series.Points.Clear();
            series.IsValueShownAsLabel = true;
            series.Label = "#PERCENT{P0}";
            chartBreakdown.Legends[0].Enabled = true;

            Color[] palette = new[]
            {
                Color.FromArgb(0, 120, 212),
                Color.FromArgb(0, 153, 188),
                Color.FromArgb(255, 185, 0),
                Color.FromArgb(255, 99, 71)
            };

            int colorIndex = 0;
            foreach (DataRow row in data.Rows)
            {
                int pointIndex = series.Points.AddXY(Convert.ToString(row["AccountName"]), Convert.ToDecimal(row["TotalAmount"]));
                series.Points[pointIndex].Color = palette[colorIndex % palette.Length];
                colorIndex++;
            }
        }

        private void BindRecentTransactions(DataTable data)
        {
            gridRecentTransactions.DataSource = data;
            if (gridRecentTransactions.Columns.Count > 0)
            {
                gridRecentTransactions.Columns["TxnDate"].HeaderText = "Date";
                gridRecentTransactions.Columns["AccountName"].HeaderText = "Account";
                gridRecentTransactions.Columns["NetAmount"].HeaderText = "Amount";
                gridRecentTransactions.Columns["Status"].HeaderText = "Status";
                gridRecentTransactions.Columns["TxnDate"].DefaultCellStyle.Format = "dd-MMM-yyyy";
                gridRecentTransactions.Columns["NetAmount"].DefaultCellStyle.Format = "N2";
            }
        }

        private void BindTopAccounts(DataTable data)
        {
            gridTopAccounts.DataSource = data;
            if (gridTopAccounts.Columns.Count > 0)
            {
                gridTopAccounts.Columns["AccountName"].HeaderText = "Account Name";
                gridTopAccounts.Columns["TotalAmount"].HeaderText = "Total Amount";
                gridTopAccounts.Columns["SharePercent"].HeaderText = "% of Total";
                gridTopAccounts.Columns["TotalAmount"].DefaultCellStyle.Format = "N2";
                gridTopAccounts.Columns["SharePercent"].DefaultCellStyle.Format = "N2'%'";
            }
        }
    }
}
