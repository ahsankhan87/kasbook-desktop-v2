using POS.BLL;
using pos.UI;
using pos.UI.Busy;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Windows.Forms;
using WinFormsChart = System.Windows.Forms.DataVisualization.Charting;

namespace pos.Sales
{
    public partial class frm_sales_dashboard : Form
    {
        private readonly Color _pageBg = ColorTranslator.FromHtml("#F5F7FA");
        private readonly Color _primaryBlue = ColorTranslator.FromHtml("#1565C0");
        private readonly Color _border = Color.FromArgb(230, 230, 230);

        private DateTime _fromDate;
        private DateTime _toDate;
        private DateTime _prevFromDate;
        private DateTime _prevToDate;

        private int? _selectedMonth;
        private string _selectedCategory;
        private int? _selectedSalesRepId;

        private DataTable _cachedMonthly;
        private DataTable _cachedRecent;

        private Panel _overlayPanel;
        private Label _lblSalesRep;
        private ComboBox _cmbSalesRep;
        private Button _btnExport;

        private readonly SalesDashboardBLL _dashboardBll = new SalesDashboardBLL();

        public frm_sales_dashboard()
        {
            InitializeComponent();
        }

        private void frm_sales_dashboard_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            BackColor = _pageBg;
            panelRoot.BackColor = _pageBg;

            EnsureTopControls();

            StyleCards();
            ConfigureCharts();
            ConfigureGrids();

            WireDrilldownEvents();
            LoadSalesRepFilter();

            rbMonth.Checked = true;
            LoadDashboard();
        }

        private void EnsureTopControls()
        {
            _lblSalesRep = new Label
            {
                Text = "Sales Rep",
                AutoSize = true,
                Visible = false,
                BackColor = Color.Transparent,
                Location = new Point(735, 12)
            };

            _cmbSalesRep = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 170,
                Visible = false,
                Location = new Point(800, 8)
            };
            _cmbSalesRep.SelectedIndexChanged += SalesRepChanged;

            _btnExport = new Button
            {
                Text = "Export Dashboard",
                Width = 130,
                Height = 28,
                BackColor = _primaryBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(1090, 7)
            };
            _btnExport.FlatAppearance.BorderSize = 0;
            _btnExport.Click += BtnExport_Click;

            panelPeriod.Controls.Add(_lblSalesRep);
            panelPeriod.Controls.Add(_cmbSalesRep);
            panelPeriod.Controls.Add(_btnExport);
        }

        private void EnsureOverlayPanel()
        {
            _overlayPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Visible = false,
                BackColor = Color.FromArgb(140, 0, 0, 0)
            };
            panelRoot.Controls.Add(_overlayPanel);
            _overlayPanel.BringToFront();
        }

        private void WireDrilldownEvents()
        {
            chartMonthlySales.MouseClick += ChartMonthlySales_MouseClick;
            chartCategory.MouseClick += ChartCategory_MouseClick;
            gridTopCustomers.CellDoubleClick += GridTopCustomers_CellDoubleClick;

            cardOutstanding.Cursor = Cursors.Hand;
            lblOutstandingTitle.Cursor = Cursors.Hand;
            lblOutstandingValue.Cursor = Cursors.Hand;
            lblOutstandingSub.Cursor = Cursors.Hand;

            cardOutstanding.Click += Outstanding_Click;
            lblOutstandingTitle.Click += Outstanding_Click;
            lblOutstandingValue.Click += Outstanding_Click;
            lblOutstandingSub.Click += Outstanding_Click;

            cardTotalSales.Click += KpiCardClicked;
            cardInvoices.Click += KpiCardClicked;
            cardPaid.Click += KpiCardClicked;
            cardAvgInvoice.Click += KpiCardClicked;
        }

        private void StyleCards()
        {
            StyleCard(cardTotalSales);
            StyleCard(cardInvoices);
            StyleCard(cardPaid);
            StyleCard(cardOutstanding);
            StyleCard(cardAvgInvoice);
            StyleContainer(panelMonthlyChart);
            StyleContainer(panelCategoryChart);
            StyleContainer(panelTopCustomers);
            StyleContainer(panelRecentInvoices);
            StyleContainer(panelPeriod);
        }

        private void StyleCard(Panel panel)
        {
            panel.BackColor = Color.White;
            panel.Paint += (s, e) =>
            {
                using (var pen = new Pen(_border))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
                }

                using (var b = new SolidBrush(_primaryBlue))
                {
                    e.Graphics.FillRectangle(b, 0, 0, 4, panel.Height);
                }
            };
        }

        private void StyleContainer(Panel panel)
        {
            panel.BackColor = Color.White;
            panel.Paint += (s, e) =>
            {
                using (var pen = new Pen(_border))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
                }
            };
        }

        private void ConfigureCharts()
        {
            var barArea = chartMonthlySales.ChartAreas[0];
            barArea.BackColor = Color.White;
            barArea.AxisX.Interval = 1;
            barArea.AxisX.LabelStyle.Angle = -30;
            barArea.AxisX.MajorGrid.Enabled = false;
            barArea.AxisY.MajorGrid.LineColor = Color.Gainsboro;

            var barSeries = chartMonthlySales.Series[0];
            barSeries.ChartType = WinFormsChart.SeriesChartType.Column;
            barSeries.IsValueShownAsLabel = false;
            barSeries.Color = _primaryBlue;

            var pieArea = chartCategory.ChartAreas[0];
            pieArea.BackColor = Color.White;

            var pieSeries = chartCategory.Series[0];
            pieSeries.ChartType = WinFormsChart.SeriesChartType.Doughnut;
            pieSeries["DoughnutRadius"] = "60";
            pieSeries.IsValueShownAsLabel = true;
            pieSeries.Label = "#PERCENT{P0}";
            pieSeries.LegendText = "#VALX";
            chartCategory.Legends[0].Docking = WinFormsChart.Docking.Bottom;
        }

        private void ConfigureGrids()
        {
            ConfigureGrid(gridTopCustomers);
            ConfigureGrid(gridRecentInvoices);

            gridTopCustomers.CellFormatting += GridAmountFormatting;
            gridRecentInvoices.CellFormatting += GridAmountFormatting;
            gridRecentInvoices.CellFormatting += GridRecentStatusFormatting;
        }

        private void ConfigureGrid(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 248);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.DefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(227, 242, 253);
            grid.DefaultCellStyle.SelectionForeColor = AppTheme.TextPrimary;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 253);
            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 30;
        }

        private void LoadSalesRepFilter()
        {
            try
            {
                DataTable reps = _dashboardBll.GetSalesReps();
                _cmbSalesRep.DataSource = reps;
                _cmbSalesRep.DisplayMember = "employee_name";
                _cmbSalesRep.ValueMember = "id";

                bool enabled = reps != null && reps.Rows.Count > 1;
                _lblSalesRep.Visible = enabled;
                _cmbSalesRep.Visible = enabled;
                _selectedSalesRepId = null;
            }
            catch
            {
                _lblSalesRep.Visible = false;
                _cmbSalesRep.Visible = false;
            }
        }

        private void SalesRepChanged(object sender, EventArgs e)
        {
            if (_cmbSalesRep == null || _cmbSalesRep.SelectedValue == null)
                return;

            int id;
            if (!int.TryParse(Convert.ToString(_cmbSalesRep.SelectedValue), out id))
                return;

            _selectedSalesRepId = id == 0 ? (int?)null : id;
            _selectedMonth = null;
            _selectedCategory = null;
            LoadDashboard();
        }

        private void PeriodChanged(object sender, EventArgs e)
        {
            bool custom = rbCustom.Checked;
            lblFrom.Visible = custom;
            lblTo.Visible = custom;
            dtpFrom.Visible = custom;
            dtpTo.Visible = custom;

            if (!custom)
            {
                _selectedMonth = null;
                _selectedCategory = null;
                LoadDashboard();
            }
        }

        private void CustomDateChanged(object sender, EventArgs e)
        {
            if (rbCustom.Checked)
            {
                _selectedMonth = null;
                _selectedCategory = null;
                LoadDashboard();
            }
        }

        private void ResolveDates()
        {
            DateTime today = DateTime.Today;

            if (rbToday.Checked)
            {
                _fromDate = today;
                _toDate = today;
                _prevFromDate = today.AddDays(-1);
                _prevToDate = today.AddDays(-1);
            }
            else if (rbWeek.Checked)
            {
                int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                _fromDate = today.AddDays(-diff);
                _toDate = today;
                int len = (_toDate - _fromDate).Days + 1;
                _prevToDate = _fromDate.AddDays(-1);
                _prevFromDate = _prevToDate.AddDays(-(len - 1));
            }
            else if (rbMonth.Checked)
            {
                _fromDate = new DateTime(today.Year, today.Month, 1);
                _toDate = today;
                DateTime prev = _fromDate.AddMonths(-1);
                _prevFromDate = new DateTime(prev.Year, prev.Month, 1);
                _prevToDate = _prevFromDate.AddMonths(1).AddDays(-1);
            }
            else if (rbQuarter.Checked)
            {
                int quarter = ((today.Month - 1) / 3) + 1;
                int startMonth = ((quarter - 1) * 3) + 1;
                _fromDate = new DateTime(today.Year, startMonth, 1);
                _toDate = today;
                _prevFromDate = _fromDate.AddMonths(-3);
                _prevToDate = _fromDate.AddDays(-1);
            }
            else if (rbYear.Checked)
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
                    DateTime t = _fromDate;
                    _fromDate = _toDate;
                    _toDate = t;
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
                using (BusyScope.Show(this, UiMessages.T("Loading sales dashboard...", "جاري تحميل لوحة المبيعات...")))
                {
                    ResolveDates();

                    BindKpis(_dashboardBll.GetKpis(_fromDate, _toDate, _prevFromDate, _prevToDate, _selectedSalesRepId));
                    _cachedMonthly = _dashboardBll.GetMonthlySales(DateTime.Today.Year, _selectedSalesRepId);
                    BindMonthlyChart(_cachedMonthly);
                    BindCategoryChart(_dashboardBll.GetCategorySplit(_fromDate, _toDate, 5, _selectedSalesRepId));
                    BindTopCustomers(_dashboardBll.GetTopCustomers(_fromDate, _toDate, 10, _selectedSalesRepId));

                    _cachedRecent = _dashboardBll.GetRecentInvoices(_fromDate, _toDate, 8, _selectedSalesRepId, _selectedMonth, _selectedCategory, null);
                    BindRecentInvoices(_cachedRecent);

                    tslLastRefresh.Text = "Last refresh: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Sales Dashboard", "لوحة المبيعات");
            }
        }

        private void BindKpis(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                lblTotalSalesValue.Text = 0m.ToString("N2");
                lblTotalSalesTrend.Text = "▲ +0.00% vs last month";
                lblInvoicesValue.Text = "0";
                lblPaidValue.Text = 0m.ToString("N2");
                lblOutstandingValue.Text = 0m.ToString("N2");
                lblOutstandingSub.Text = "Over 30d: 0.00";
                lblAvgInvoiceValue.Text = 0m.ToString("N2");
                return;
            }

            DataRow r = dt.Rows[0];

            decimal totalSales = ToDecimal(r["total_sales"]);
            decimal prevSales = ToDecimal(r["total_sales_prev"]);
            int invoices = ToInt(r["total_invoices"]);
            decimal paid = ToDecimal(r["paid_amount"]);
            decimal receivable = ToDecimal(r["receivable_outstanding"]);
            decimal over30 = ToDecimal(r["receivable_over_30"]);
            decimal avg = ToDecimal(r["avg_invoice_value"]);

            decimal trend = prevSales == 0 ? (totalSales == 0 ? 0 : 100) : ((totalSales - prevSales) / prevSales) * 100m;
            string arrow = trend >= 0 ? "▲" : "▼";

            lblTotalSalesValue.Text = totalSales.ToString("N2");
            lblTotalSalesTrend.Text = string.Format("{0} {1}{2:N2}% vs last month", arrow, trend >= 0 ? "+" : string.Empty, trend);
            lblTotalSalesTrend.ForeColor = trend >= 0 ? Color.SeaGreen : Color.Firebrick;

            lblInvoicesValue.Text = invoices.ToString("N0");
            lblPaidValue.Text = paid.ToString("N2");
            lblOutstandingValue.Text = receivable.ToString("N2");
            lblOutstandingSub.Text = "Over 30d: " + over30.ToString("N2");
            lblOutstandingValue.ForeColor = over30 > 0 ? Color.OrangeRed : AppTheme.TextPrimary;
            lblAvgInvoiceValue.Text = avg.ToString("N2");
        }

        private void BindMonthlyChart(DataTable dt)
        {
            var series = chartMonthlySales.Series[0];
            series.Points.Clear();

            int currentMonth = DateTime.Today.Month;

            foreach (DataRow row in dt.Rows)
            {
                int monthNo = ToInt(row["month_no"]);
                string label = Convert.ToString(row["month_label"]);
                decimal amount = ToDecimal(row["amount"]);

                int idx = series.Points.AddXY(label, amount);
                series.Points[idx].Tag = monthNo;

                if (_selectedMonth.HasValue && _selectedMonth.Value == monthNo)
                    series.Points[idx].Color = Color.FromArgb(13, 71, 161);
                else
                    series.Points[idx].Color = monthNo == currentMonth ? _primaryBlue : Color.FromArgb(100, 181, 246);
            }
        }

        private void BindCategoryChart(DataTable dt)
        {
            var series = chartCategory.Series[0];
            series.Points.Clear();

            Color[] palette =
            {
                Color.FromArgb(21,101,192),
                Color.FromArgb(66,133,244),
                Color.FromArgb(100,181,246),
                Color.FromArgb(144,202,249),
                Color.FromArgb(187,222,251),
                Color.FromArgb(189,189,189)
            };

            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                string category = Convert.ToString(row["category_name"]);
                int p = series.Points.AddXY(category, ToDecimal(row["total_amount"]));
                series.Points[p].Tag = category;
                series.Points[p].Color = string.Equals(_selectedCategory, category, StringComparison.OrdinalIgnoreCase)
                    ? Color.FromArgb(13, 71, 161)
                    : palette[i % palette.Length];
                i++;
            }
        }

        private void BindTopCustomers(DataTable dt)
        {
            gridTopCustomers.DataSource = dt;
        }

        private void BindRecentInvoices(DataTable dt)
        {
            gridRecentInvoices.DataSource = dt;
        }

        private void ChartMonthlySales_MouseClick(object sender, MouseEventArgs e)
        {
            var result = chartMonthlySales.HitTest(e.X, e.Y);
            if (result != null && result.PointIndex >= 0)
            {
                var p = chartMonthlySales.Series[0].Points[result.PointIndex];
                _selectedMonth = ToInt(p.Tag);
                LoadDashboard();
            }
        }

        private void ChartCategory_MouseClick(object sender, MouseEventArgs e)
        {
            var result = chartCategory.HitTest(e.X, e.Y);
            if (result != null && result.PointIndex >= 0)
            {
                var p = chartCategory.Series[0].Points[result.PointIndex];
                _selectedCategory = Convert.ToString(p.Tag);
                ShowCategoryOverlay(_selectedCategory);
                LoadDashboard();
            }
        }

        private void GridTopCustomers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int customerId = 0;
            var row = gridTopCustomers.Rows[e.RowIndex];
            var drv = row.DataBoundItem as DataRowView;
            if (drv != null && drv.Row.Table.Columns.Contains("customer_id"))
            {
                customerId = ToInt(drv.Row["customer_id"]);
            }

            if (customerId <= 0 && gridTopCustomers.DataSource is DataTable dt && dt.Columns.Contains("customer_id"))
            {
                customerId = ToInt(dt.Rows[e.RowIndex]["customer_id"]);
            }

            if (customerId > 0)
            {
                ShowCustomerOverlay(customerId);
            }
        }

        private void Outstanding_Click(object sender, EventArgs e)
        {
            ShowReceivablesOverlay();
        }

        private void KpiCardClicked(object sender, EventArgs e)
        {
            _selectedMonth = null;
            _selectedCategory = null;
            LoadDashboard();
        }

        private void ShowCategoryOverlay(string categoryName)
        {
            DataTable dt = _dashboardBll.GetCategoryTopInvoices(_fromDate, _toDate, categoryName, 10, _selectedSalesRepId);
            decimal totalAmount = 0m;
            foreach (DataRow row in dt.Rows) totalAmount += ToDecimal(row["amount"]);

            Panel body;
            using (Form popup = CreatePopupForm("Category: " + categoryName, out body))
            {
                Label summary = new Label
                {
                    Dock = DockStyle.Top,
                    Height = 28,
                    Padding = new Padding(8, 6, 8, 6),
                    Text = "Invoices: " + dt.Rows.Count.ToString("N0") + "   Total: " + totalAmount.ToString("N2")
                };

                DataGridView grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
                ConfigureGrid(grid);
                grid.DataSource = dt;

                body.Controls.Add(grid);
                body.Controls.Add(summary);

                popup.ShowDialog(this);
            }
        }

        private void ShowCustomerOverlay(int customerId)
        {
            DataTable summaryDt = _dashboardBll.GetCustomerSummary(customerId, _fromDate, _toDate, _selectedSalesRepId);
            DataTable invoices = _dashboardBll.GetRecentInvoices(_fromDate, _toDate, 5, _selectedSalesRepId, null, null, customerId);
            DataTable trend = _dashboardBll.GetCustomerMonthlyTrend(customerId, DateTime.Today.Year, _selectedSalesRepId);

            Panel body;
            using (Form popup = CreatePopupForm("Customer Summary", out body))
            {
                Label info = new Label { Dock = DockStyle.Top, Height = 42, Padding = new Padding(8, 4, 8, 4) };
                if (summaryDt.Rows.Count > 0)
                {
                    DataRow r = summaryDt.Rows[0];
                    info.Text = string.Format("{0}\r\nTotal: {1:N2}   Outstanding: {2:N2}", Convert.ToString(r["customer_name"]), ToDecimal(r["total_sales"]), ToDecimal(r["outstanding"]));
                }

                SplitContainer split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Horizontal, SplitterDistance = 200 };

                DataGridView grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
                ConfigureGrid(grid);
                grid.DataSource = invoices;
                split.Panel1.Controls.Add(grid);

                WinFormsChart.Chart mini = new WinFormsChart.Chart { Dock = DockStyle.Fill };
                mini.ChartAreas.Add(new WinFormsChart.ChartArea("A"));
                mini.Series.Add(new WinFormsChart.Series("S") { ChartType = WinFormsChart.SeriesChartType.Column, Color = _primaryBlue });
                foreach (DataRow row in trend.Rows)
                {
                    mini.Series[0].Points.AddXY(Convert.ToString(row["month_label"]), ToDecimal(row["amount"]));
                }
                split.Panel2.Controls.Add(mini);

                body.Controls.Add(split);
                body.Controls.Add(info);

                popup.ShowDialog(this);
            }
        }

        private void ShowReceivablesOverlay()
        {
            DataTable dt = _dashboardBll.GetReceivables(_fromDate, _toDate, true, _selectedSalesRepId);

            Panel body;
            using (Form popup = CreatePopupForm("Outstanding Receivables (Overdue)", out body))
            {
                DataGridView grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
                ConfigureGrid(grid);
                grid.DataSource = dt;

                body.Controls.Add(grid);
                popup.ShowDialog(this);
            }
        }

        private Form CreatePopupForm(string title, out Panel body)
        {
            Form popup = new Form
            {
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.SizableToolWindow,
                ShowInTaskbar = false,
                MinimizeBox = false,
                MaximizeBox = true,
                Size = new Size(920, 560),
                Text = title,
                BackColor = Color.White
            };

            Panel header = new Panel { Dock = DockStyle.Top, Height = 38, BackColor = _primaryBlue };
            Label lbl = new Label
            {
                Text = title,
                ForeColor = Color.White,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0)
            };
            Button close = new Button
            {
                Text = "X",
                ForeColor = Color.White,
                BackColor = _primaryBlue,
                FlatStyle = FlatStyle.Flat,
                Dock = DockStyle.Right,
                Width = 42
            };
            close.FlatAppearance.BorderSize = 0;
            close.Click += (s, e) => popup.Close();

            header.Controls.Add(close);
            header.Controls.Add(lbl);

            body = new Panel { Dock = DockStyle.Fill, Padding = new Padding(8) };

            popup.Controls.Add(body);
            popup.Controls.Add(header);
            return popup;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            using (PrintDocument pd = new PrintDocument())
            {
                Bitmap bmp = new Bitmap(panelRoot.Width, panelRoot.Height);
                panelRoot.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

                pd.PrintPage += (s, pe) =>
                {
                    Rectangle m = pe.MarginBounds;
                    pe.Graphics.DrawImage(bmp, m);
                    pe.HasMorePages = false;
                };

                using (PrintPreviewDialog pp = new PrintPreviewDialog())
                {
                    pp.Document = pd;
                    pp.Width = 1200;
                    pp.Height = 800;
                    pp.ShowDialog(this);
                }
            }
        }

        private void GridAmountFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid == null || e.Value == null)
                return;

            string name = grid.Columns[e.ColumnIndex].Name;
            if (name == "colCustomerTotal" || name == "colCustomerShare" || name == "colCustomerOutstanding" || name == "colInvoiceAmount")
            {
                decimal value;
                if (decimal.TryParse(Convert.ToString(e.Value), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                {
                    e.Value = name == "colCustomerShare" ? value.ToString("N2") + "%" : value.ToString("N2");
                    e.FormattingApplied = true;
                }
            }
        }

        private void GridRecentStatusFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gridRecentInvoices.Columns[e.ColumnIndex].Name != "colInvoiceStatus" || e.Value == null)
                return;

            string status = Convert.ToString(e.Value);
            if (string.Equals(status, "Paid", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.BackColor = Color.FromArgb(46, 125, 50);
                e.CellStyle.ForeColor = Color.White;
            }
            else if (string.Equals(status, "Partial", StringComparison.OrdinalIgnoreCase))
            {
                e.CellStyle.BackColor = Color.FromArgb(245, 124, 0);
                e.CellStyle.ForeColor = Color.White;
            }
            else
            {
                e.CellStyle.BackColor = Color.FromArgb(198, 40, 40);
                e.CellStyle.ForeColor = Color.White;
            }

            e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
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

        private void tslRefresh_Click(object sender, EventArgs e)
        {
            _selectedMonth = null;
            _selectedCategory = null;
            LoadDashboard();
        }
    }
}
