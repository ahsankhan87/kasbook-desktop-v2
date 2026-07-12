using pos.UI;
using pos.UI.Busy;
using POS.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WinFormsChart = System.Windows.Forms.DataVisualization.Charting;

namespace pos
{
    public partial class frm_accounting_dashboard : Form
    {
        private readonly AccountingDashboardBLL _bll = new AccountingDashboardBLL();

        public frm_accounting_dashboard()
        {
            InitializeComponent();
        }

        private void frm_accounting_dashboard_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            ApplyStyles();
            LoadPeriods();
            ConfigureCharts();
            ConfigureGrids();
            cmbPeriod.SelectedIndex = 0;
            LoadDashboard();
        }

        private void LoadPeriods()
        {
            cmbPeriod.Items.Clear();
            cmbPeriod.Items.Add("This Month");
            cmbPeriod.Items.Add("Last Month");
            cmbPeriod.Items.Add("This Quarter");
            cmbPeriod.Items.Add("This Year");
        }

        private void ApplyStyles()
        {
            BackColor = Color.FromArgb(244, 247, 252);
            panelHeader.BackColor = BackColor;
            panelMain.BackColor = BackColor;

            StyleKpiCard(cardCash, Color.FromArgb(0, 120, 212));
            StyleKpiCard(cardReceivable, Color.FromArgb(0, 153, 188));
            StyleKpiCard(cardPayable, Color.FromArgb(246, 133, 90));
            StyleKpiCard(cardRevenue, Color.FromArgb(45, 146, 77));
            StyleKpiCard(cardExpenses, Color.FromArgb(220, 92, 73));
            StyleKpiCard(cardNetProfit, Color.FromArgb(102, 89, 216));

            btnRefresh.BackColor = AppTheme.Primary;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;

            StyleQuickActionButton(btnNewJv);
            StyleQuickActionButton(btnReceivePayment);
            StyleQuickActionButton(btnMakePayment);
            StyleQuickActionButton(btnBankRec);
            StyleQuickActionButton(btnRunPL);
            StyleQuickActionButton(btnRunBalanceSheet);
        }

        private static void StyleQuickActionButton(Button button)
        {
            button.BackColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = Color.FromArgb(210, 216, 227);
            button.FlatAppearance.BorderSize = 1;
            button.ForeColor = AppTheme.TextPrimary;
            button.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
        }

        private static void StyleKpiCard(Panel panel, Color accent)
        {
            panel.BackColor = Color.White;
            panel.Cursor = Cursors.Hand;
            panel.Paint += (s, e) =>
            {
                using (var borderPen = new Pen(Color.FromArgb(224, 229, 236), 1))
                {
                    e.Graphics.DrawRectangle(borderPen, 0, 0, panel.Width - 1, panel.Height - 1);
                }

                using (var accentBrush = new SolidBrush(accent))
                {
                    e.Graphics.FillRectangle(accentBrush, 0, 0, 6, panel.Height);
                }
            };

            foreach (Control child in panel.Controls)
            {
                child.ForeColor = AppTheme.TextPrimary;
                child.Cursor = Cursors.Hand;
                child.Click += (sender, args) =>
                {
                    // Invoke the protected OnClick method using reflection
                    typeof(Control).GetMethod("OnClick",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                        ?.Invoke(panel, new object[] { args });
                };
            }
        }

        private void ConfigureCharts()
        {
            ConfigureChart(chartPnl, true);
            ConfigureChart(chartCashFlow, false);
            ConfigureChart(chartExpense, false);
        }

        private static void ConfigureChart(WinFormsChart.Chart chart, bool showYGrid)
        {
            chart.BackColor = Color.White;
            chart.BorderlineColor = Color.White;
            chart.ChartAreas[0].BackColor = Color.White;
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart.ChartAreas[0].AxisY.MajorGrid.Enabled = showYGrid;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(236, 240, 246);
            chart.ChartAreas[0].AxisX.LineColor = Color.FromArgb(214, 220, 229);
            chart.ChartAreas[0].AxisY.LineColor = Color.FromArgb(214, 220, 229);
            chart.Legends[0].Docking = WinFormsChart.Docking.Bottom;
            chart.Legends[0].Font = new Font("Segoe UI", 8.5F);
            chart.Series.Clear();
        }

        private void ConfigureGrids()
        {
            ConfigureGrid(gridAttention);
            ConfigureGrid(gridUnreconciledBanks);
            ConfigureGrid(gridRecentJournals);

            gridAttention.ReadOnly = true;
            gridAttention.Columns["colAttentionAction"].ReadOnly = false;

            gridUnreconciledBanks.ReadOnly = true;
            gridUnreconciledBanks.Columns["colBankReconcile"].ReadOnly = false;

            gridRecentJournals.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridRecentJournals.ReadOnly = true;
        }

        private static void ConfigureGrid(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(244, 247, 252);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            grid.DefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(223, 236, 251);
            grid.DefaultCellStyle.SelectionForeColor = AppTheme.TextPrimary;
            grid.RowHeadersVisible = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.RowTemplate.Height = 30;
        }

        private void LoadDashboard()
        {
            try
            {
                DateTime fromDate;
                DateTime toDate;
                ResolvePeriod(out fromDate, out toDate);

                using (BusyScope.Show(this, UiMessages.T("Loading accounting dashboard...", "جاري تحميل لوحة الحسابات...")))
                {
                    AccountingDashboardData dashboard = _bll.GetDashboardData(fromDate, toDate);

                    BindKpis(dashboard);
                    BindMonthlyPnlChart(dashboard.MonthlyPnl);
                    BindCashFlowChart(dashboard.CashFlowTrend);
                    BindExpenseChart(dashboard.ExpenseBreakdown);
                    BindAttentionGrid(dashboard.AttentionSummary);
                    BindUnreconciledBanks(dashboard.UnreconciledBankAccounts);
                    BindRecentJournals(dashboard.RecentJournalActivity);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void ResolvePeriod(out DateTime fromDate, out DateTime toDate)
        {
            DateTime today = DateTime.Today;
            string period = Convert.ToString(cmbPeriod.SelectedItem);

            if (string.Equals(period, "Last Month", StringComparison.OrdinalIgnoreCase))
            {
                DateTime lastMonth = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
                fromDate = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                toDate = fromDate.AddMonths(1).AddDays(-1);
                return;
            }

            if (string.Equals(period, "This Quarter", StringComparison.OrdinalIgnoreCase))
            {
                int quarter = ((today.Month - 1) / 3) + 1;
                int firstMonth = ((quarter - 1) * 3) + 1;
                fromDate = new DateTime(today.Year, firstMonth, 1);
                toDate = today;
                return;
            }

            if (string.Equals(period, "This Year", StringComparison.OrdinalIgnoreCase))
            {
                fromDate = new DateTime(today.Year, 1, 1);
                toDate = today;
                return;
            }

            fromDate = new DateTime(today.Year, today.Month, 1);
            toDate = today;
        }

        private void BindKpis(AccountingDashboardData dashboard)
        {
            lblCashValue.Text = dashboard.CashBankBalance.ToString("N2");
            lblReceivableValue.Text = dashboard.TotalReceivables.ToString("N2");
            lblPayableValue.Text = dashboard.TotalPayables.ToString("N2");
            lblRevenueValue.Text = dashboard.Revenue.ToString("N2");
            lblExpensesValue.Text = dashboard.Expenses.ToString("N2");
            lblNetProfitValue.Text = dashboard.NetProfit.ToString("N2");
            lblNetProfitValue.ForeColor = dashboard.NetProfit >= 0 ? Color.FromArgb(27, 138, 62) : Color.FromArgb(198, 59, 41);
        }

        private void BindMonthlyPnlChart(DataTable data)
        {
            chartPnl.Series.Clear();

            var revenueSeries = chartPnl.Series.Add("Revenue");
            revenueSeries.ChartType = WinFormsChart.SeriesChartType.Column;
            revenueSeries.Color = Color.FromArgb(0, 120, 212);
            revenueSeries.XValueType = WinFormsChart.ChartValueType.String;

            var expenseSeries = chartPnl.Series.Add("Expenses");
            expenseSeries.ChartType = WinFormsChart.SeriesChartType.Column;
            expenseSeries.Color = Color.FromArgb(255, 127, 80);
            expenseSeries.XValueType = WinFormsChart.ChartValueType.String;

            var netSeries = chartPnl.Series.Add("Net Profit");
            netSeries.ChartType = WinFormsChart.SeriesChartType.Line;
            netSeries.Color = Color.FromArgb(45, 146, 77);
            netSeries.BorderWidth = 2;
            netSeries.XValueType = WinFormsChart.ChartValueType.String;

            if (data == null) return;

            foreach (DataRow row in data.Rows)
            {
                string month = Convert.ToString(row["MonthLabel"]);
                decimal revenue = row["Revenue"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Revenue"]);
                decimal expenses = row["Expenses"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Expenses"]);
                decimal net = row["NetProfit"] == DBNull.Value ? 0m : Convert.ToDecimal(row["NetProfit"]);

                revenueSeries.Points.AddXY(month, revenue);
                expenseSeries.Points.AddXY(month, expenses);
                netSeries.Points.AddXY(month, net);
            }
        }

        private void BindCashFlowChart(DataTable data)
        {
            chartCashFlow.Series.Clear();
            var series = chartCashFlow.Series.Add("Cash & Bank");
            series.ChartType = WinFormsChart.SeriesChartType.Line;
            series.Color = Color.FromArgb(66, 103, 178);
            series.BorderWidth = 2;
            series.XValueType = WinFormsChart.ChartValueType.String;

            if (data == null || data.Rows.Count == 0)
            {
                return;
            }

            int minIndex = -1;
            int maxIndex = -1;
            decimal minValue = decimal.MaxValue;
            decimal maxValue = decimal.MinValue;

            for (int i = 0; i < data.Rows.Count; i++)
            {
                DataRow row = data.Rows[i];
                DateTime pointDate = Convert.ToDateTime(row["PointDate"]);
                decimal balance = row["Balance"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Balance"]);
                int pointIndex = series.Points.AddXY(pointDate.ToString("dd MMM"), balance);

                if (balance < minValue)
                {
                    minValue = balance;
                    minIndex = pointIndex;
                }

                if (balance > maxValue)
                {
                    maxValue = balance;
                    maxIndex = pointIndex;
                }
            }

            if (maxIndex >= 0)
            {
                series.Points[maxIndex].Label = "High";
                series.Points[maxIndex].MarkerStyle = WinFormsChart.MarkerStyle.Circle;
                series.Points[maxIndex].MarkerSize = 7;
            }

            if (minIndex >= 0)
            {
                series.Points[minIndex].Label = "Low";
                series.Points[minIndex].MarkerStyle = WinFormsChart.MarkerStyle.Circle;
                series.Points[minIndex].MarkerSize = 7;
            }
        }

        private void BindExpenseChart(DataTable data)
        {
            chartExpense.Series.Clear();
            var series = chartExpense.Series.Add("Expenses");
            series.ChartType = WinFormsChart.SeriesChartType.Doughnut;
            series.IsValueShownAsLabel = true;
            series.Label = "#PERCENT{P0}";

            if (data == null)
            {
                return;
            }

            foreach (DataRow row in data.Rows)
            {
                string category = Convert.ToString(row["Category"]);
                decimal amount = row["Amount"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Amount"]);
                series.Points.AddXY(category, amount);
            }

            chartExpense.Legends[0].Enabled = true;
        }

        private void BindAttentionGrid(DataTable data)
        {
            gridAttention.Rows.Clear();
            if (data == null)
            {
                return;
            }

            foreach (DataRow row in data.Rows)
            {
                int index = gridAttention.Rows.Add(
                    Convert.ToString(row["ItemTitle"]),
                    Convert.ToInt32(row["ItemCount"]),
                    Convert.ToDecimal(row["ItemAmount"]).ToString("N2"),
                    Convert.ToString(row["ActionText"]));
                gridAttention.Rows[index].Tag = Convert.ToString(row["ItemKey"]);
            }
        }

        private void BindUnreconciledBanks(DataTable data)
        {
            gridUnreconciledBanks.Rows.Clear();
            if (data == null)
            {
                return;
            }

            foreach (DataRow row in data.Rows)
            {
                int index = gridUnreconciledBanks.Rows.Add(Convert.ToString(row["AccountName"]), "Reconcile");
                gridUnreconciledBanks.Rows[index].Tag = row["AccountId"];
            }
        }

        private void BindRecentJournals(DataTable data)
        {
            gridRecentJournals.DataSource = data;
            if (gridRecentJournals.Columns.Contains("ModuleSource"))
            {
                gridRecentJournals.Columns["ModuleSource"].HeaderText = "Module";
            }

            if (gridRecentJournals.Columns.Contains("Date"))
            {
                gridRecentJournals.Columns["Date"].DefaultCellStyle.Format = "dd-MMM-yyyy";
            }

            if (gridRecentJournals.Columns.Contains("Amount"))
            {
                gridRecentJournals.Columns["Amount"].DefaultCellStyle.Format = "N2";
                gridRecentJournals.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void cmbPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void gridRecentJournals_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !gridRecentJournals.Columns.Contains("ModuleSource"))
            {
                return;
            }

            string module = Convert.ToString(gridRecentJournals.Rows[e.RowIndex].Cells["ModuleSource"].Value);
            if (module.IndexOf("sale", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                gridRecentJournals.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(236, 246, 255);
            }
            else if (module.IndexOf("purchase", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                gridRecentJournals.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 245, 235);
            }
            else if (module.IndexOf("bank", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                gridRecentJournals.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(240, 251, 244);
            }
            else
            {
                gridRecentJournals.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void gridAttention_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != gridAttention.Columns["colAttentionAction"].Index)
            {
                return;
            }

            string key = Convert.ToString(gridAttention.Rows[e.RowIndex].Tag);
            if (string.Equals(key, "OverdueReceivables", StringComparison.OrdinalIgnoreCase))
            {
                cardReceivable_Click(sender, EventArgs.Empty);
            }
            else if (string.Equals(key, "OverduePayables", StringComparison.OrdinalIgnoreCase))
            {
                cardPayable_Click(sender, EventArgs.Empty);
            }
            else if (string.Equals(key, "UnpostedJv", StringComparison.OrdinalIgnoreCase))
            {
                var frm = new frm_journal_voucher_manager();
                frm.ShowDialog(this);
            }
            else if (string.Equals(key, "BankReconciliation", StringComparison.OrdinalIgnoreCase))
            {
                btnBankRec_Click(sender, EventArgs.Empty);
            }
            else if (string.Equals(key, "OpenPeriods", StringComparison.OrdinalIgnoreCase))
            {
                var frm = new frm_financial_periods();
                frm.ShowDialog(this);
            }
        }

        private void gridUnreconciledBanks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != gridUnreconciledBanks.Columns["colBankReconcile"].Index)
            {
                return;
            }

            btnBankRec_Click(sender, EventArgs.Empty);
        }

        private void cardCash_Click(object sender, EventArgs e)
        {
            var frm = new Reports.Accounts.FrmCashBook();
            frm.ShowDialog(this);
        }

        private void cardReceivable_Click(object sender, EventArgs e)
        {
            var frm = new Reports.Accounts.frm_ARAgingReport();
            frm.ShowDialog(this);
        }

        private void cardPayable_Click(object sender, EventArgs e)
        {
            var frm = new Reports.Accounts.frm_APAgingReport();
            frm.ShowDialog(this);
        }

        private void cardRevenue_Click(object sender, EventArgs e)
        {
            var frm = new Reports.Financial.frm_ProfitAndLossReport();
            frm.ShowDialog(this);
        }

        private void btnNewJv_Click(object sender, EventArgs e)
        {
            var frm = new frm_journal_entries();
            frm.ShowDialog(this);
        }

        private void btnReceivePayment_Click(object sender, EventArgs e)
        {
            var frm = new frm_customer_payment();
            frm.ShowDialog(this);
        }

        private void btnMakePayment_Click(object sender, EventArgs e)
        {
            var frm = new frm_supplier_payment();
            frm.ShowDialog(this);
        }

        private void btnBankRec_Click(object sender, EventArgs e)
        {
            var frm = new Reports.Financial.frm_BankReconciliation();
            frm.ShowDialog(this);
        }

        private void btnRunPL_Click(object sender, EventArgs e)
        {
            cardRevenue_Click(sender, e);
        }

        private void btnRunBalanceSheet_Click(object sender, EventArgs e)
        {
            var frm = new Reports.Financial.frm_BalanceSheetReport();
            frm.ShowDialog(this);
        }
    }
}
