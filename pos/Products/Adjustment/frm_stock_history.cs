using POS.BLL;
using POS.Core;
using pos.Reports.Adjustment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace pos
{
    /// <summary>
    /// Standalone small form: chronological timeline of all changes (qty / price / location)
    /// for a selected product. Opened from the stock-check adjustment form or product detail.
    /// </summary>
    public partial class frm_stock_history : Form
    {
        private readonly StockAdjustmentBLL _bll = new StockAdjustmentBLL();

        // Accept optional product id + name for pre-loading
        private int _productId;
        private string _productName;

        // Controls
        private TextBox _txtProductSearch;
        private Button _btnSearch;
        private Label _lblProductName;
        private DateTimePicker _dtpFrom;
        private DateTimePicker _dtpTo;
        private CheckBox _chkQty;
        private CheckBox _chkPrice;
        private CheckBox _chkLocation;
        private Button _btnLoad;
        private Button _btnPrint;
        private Button _btnExport;
        private DataGridView _grid;
        private Chart _chart;
        private Label _lblSummary;

        private List<AdjAuditRow> _rows = new List<AdjAuditRow>();
        private List<AdjAuditRow> _filtered = new List<AdjAuditRow>();

        public frm_stock_history(int productId = 0, string productName = null)
        {
            InitializeComponent();
            _productId = productId;
            _productName = productName ?? string.Empty;
            BuildUi();

            if (_productId > 0)
            {
                _txtProductSearch.Text = productId.ToString();
                _lblProductName.Text = _productName;
                LoadData();
            }
        }

        private void BuildUi()
        {
            Text = "Product Stock History";
            Width = 1100;
            Height = 680;
            MinimumSize = new Size(900, 560);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;

            // ---- Top filter bar ----
            var top = new Panel { Dock = DockStyle.Top, Height = 48, BackColor = Color.FromArgb(245, 247, 250), Padding = new Padding(8, 8, 8, 0) };
            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };

            flow.Controls.Add(MakeLbl("Product ID:"));
            _txtProductSearch = new TextBox { Width = 80, Margin = new Padding(0, 4, 4, 0) };
            flow.Controls.Add(_txtProductSearch);

            _btnSearch = new Button { Text = "Find", FlatStyle = FlatStyle.Flat, Width = 48, Height = 26, Margin = new Padding(0, 3, 10, 0) };
            _btnSearch.Click += BtnSearch_Click;
            flow.Controls.Add(_btnSearch);

            _lblProductName = new Label { AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.FromArgb(21, 101, 192), TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0, 5, 16, 0) };
            flow.Controls.Add(_lblProductName);

            flow.Controls.Add(MakeLbl("From:"));
            _dtpFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddMonths(-6), Width = 100, Margin = new Padding(0, 3, 6, 0) };
            flow.Controls.Add(_dtpFrom);

            flow.Controls.Add(MakeLbl("To:"));
            _dtpTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today, Width = 100, Margin = new Padding(0, 3, 10, 0) };
            flow.Controls.Add(_dtpTo);

            _chkQty = new CheckBox { Text = "Qty", Checked = true, AutoSize = true, Margin = new Padding(0, 6, 6, 0) };
            _chkPrice = new CheckBox { Text = "Price", Checked = true, AutoSize = true, Margin = new Padding(0, 6, 6, 0) };
            _chkLocation = new CheckBox { Text = "Location", Checked = true, AutoSize = true, Margin = new Padding(0, 6, 10, 0) };
            flow.Controls.Add(_chkQty);
            flow.Controls.Add(_chkPrice);
            flow.Controls.Add(_chkLocation);

            _btnLoad = MakeBtn("Load", Color.FromArgb(21, 101, 192));
            _btnPrint = MakeBtn("Print", Color.FromArgb(46, 125, 50));
            _btnExport = MakeBtn("Export CSV", Color.FromArgb(84, 110, 122));
            flow.Controls.Add(_btnLoad);
            flow.Controls.Add(_btnPrint);
            flow.Controls.Add(_btnExport);
            top.Controls.Add(flow);

            // ---- Summary strip ----
            _lblSummary = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 22,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.DimGray,
                Padding = new Padding(8, 0, 0, 0)
            };

            // ---- Split: grid on left, chart on right ----
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 680,
                Panel1MinSize = 400,
                Panel2MinSize = 260
            };

            // Grid
            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                EnableHeadersVisualStyles = false
            };
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            _grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _grid.ColumnHeadersHeight = 30;

            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Date/Time", Width = 130, DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Consolas", 8.5F) } });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Change Type", Width = 110 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Old Value", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "New Value", Width = 100, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, ForeColor = Color.FromArgb(21, 101, 192) } });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Adj No", Width = 100 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Adj Type", Width = 110 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Changed By", Width = 110 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Reason", Width = 140 });

            _grid.CellFormatting += Grid_CellFormatting;
            split.Panel1.Controls.Add(_grid);

            // Chart
            _chart = new Chart { Dock = DockStyle.Fill, BackColor = Color.White };
            var chartArea = new ChartArea("history")
            {
                BackColor = Color.White
            };
            chartArea.AxisX.MajorGrid.LineColor = Color.Gainsboro;
            chartArea.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 7.5F);
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 7.5F);
            _chart.ChartAreas.Add(chartArea);

            var seriesQty = new Series("Qty") { ChartType = SeriesChartType.Line, Color = Color.FromArgb(21, 101, 192), BorderWidth = 2, IsValueShownAsLabel = false };
            var seriesPrice = new Series("Price") { ChartType = SeriesChartType.Line, Color = Color.FromArgb(46, 125, 50), BorderWidth = 2, IsValueShownAsLabel = false };
            _chart.Series.Add(seriesQty);
            _chart.Series.Add(seriesPrice);

            var legend = new Legend("main") { Font = new Font("Segoe UI", 8F) };
            _chart.Legends.Add(legend);
            split.Panel2.Controls.Add(_chart);

            Controls.Add(split);
            Controls.Add(_lblSummary);
            Controls.Add(top);

            _btnLoad.Click += (s, e) => LoadData();
            _btnPrint.Click += (s, e) => PrintHistory();
            _btnExport.Click += (s, e) => ExportCsv();
            _chkQty.CheckedChanged += (s, e) => ApplyFilter();
            _chkPrice.CheckedChanged += (s, e) => ApplyFilter();
            _chkLocation.CheckedChanged += (s, e) => ApplyFilter();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(_txtProductSearch.Text.Trim(), out id) || id <= 0)
            {
                MessageBox.Show(this, "Enter a valid product ID.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _productId = id;
            _lblProductName.Text = string.Format("Product #{0}", id);
            LoadData();
        }

        private void LoadData()
        {
            if (_productId <= 0)
            {
                MessageBox.Show(this, "Enter a product ID first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _rows = _bll.GetAdjustmentHistory(_productId, _dtpFrom.Value, _dtpTo.Value);
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter()
        {
            var types = new List<string>();
            if (_chkQty.Checked) types.Add("QTY_CHANGE");
            if (_chkPrice.Checked) types.Add("PRICE_CHANGE");
            if (_chkLocation.Checked) types.Add("LOCATION_CHANGE");

            _filtered = _rows.Where(r => types.Contains(r.ChangeType)).OrderByDescending(r => r.ChangedAt).ToList();

            _grid.Rows.Clear();
            foreach (var r in _filtered)
            {
                _grid.Rows.Add(
                    r.ChangedAt.ToString("yyyy-MM-dd HH:mm"),
                    FormatChangeType(r.ChangeType),
                    r.OldValue,
                    r.NewValue,
                    r.AdjNo,
                    r.AdjType,
                    r.ChangedBy,
                    r.Reason);
            }

            UpdateChart();

            int qtyCount = _filtered.Count(r => r.ChangeType == "QTY_CHANGE");
            int priceCount = _filtered.Count(r => r.ChangeType == "PRICE_CHANGE");
            int locCount = _filtered.Count(r => r.ChangeType == "LOCATION_CHANGE");
            _lblSummary.Text = string.Format("{0} total events  |  Qty: {1}  |  Price: {2}  |  Location: {3}", _filtered.Count, qtyCount, priceCount, locCount);
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _filtered.Count)
                return;

            var row = _filtered[e.RowIndex];
            Color bg;
            switch (row.ChangeType)
            {
                case "QTY_CHANGE":
                    bg = Color.FromArgb(232, 240, 254);
                    break;
                case "PRICE_CHANGE":
                    bg = Color.FromArgb(232, 245, 233);
                    break;
                case "LOCATION_CHANGE":
                    bg = Color.FromArgb(255, 253, 208);
                    break;
                default:
                    return;
            }
            _grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = bg;
        }

        private void UpdateChart()
        {
            _chart.Series["Qty"].Points.Clear();
            _chart.Series["Price"].Points.Clear();

            var qtyRows = _filtered.Where(r => r.ChangeType == "QTY_CHANGE").OrderBy(r => r.ChangedAt).ToList();
            var priceRows = _filtered.Where(r => r.ChangeType == "PRICE_CHANGE").OrderBy(r => r.ChangedAt).ToList();

            foreach (var r in qtyRows)
            {
                decimal v;
                if (decimal.TryParse(r.NewValue, out v))
                    _chart.Series["Qty"].Points.AddXY(r.ChangedAt.ToString("MM/dd"), (double)v);
            }

            foreach (var r in priceRows)
            {
                decimal v;
                if (decimal.TryParse(r.NewValue, out v))
                    _chart.Series["Price"].Points.AddXY(r.ChangedAt.ToString("MM/dd"), (double)v);
            }
        }

        private void PrintHistory()
        {
            if (_filtered == null || _filtered.Count == 0)
            {
                MessageBox.Show(this, "Load data first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string title = string.Format("Stock History — {0}  ({1} – {2})", _lblProductName.Text, _dtpFrom.Value.ToString("yyyy-MM-dd"), _dtpTo.Value.ToString("yyyy-MM-dd"));
            var headers = new[] { "Date/Time", "Change Type", "Old Value", "New Value", "Adj No", "Adj Type", "Changed By", "Reason" };
            var widths = new[] { 110, 100, 80, 80, 90, 100, 100, 140 };
            var rows = new List<string[]>();
            foreach (var r in _filtered)
                rows.Add(new[] { r.ChangedAt.ToString("yyyy-MM-dd HH:mm"), FormatChangeType(r.ChangeType), r.OldValue, r.NewValue, r.AdjNo, r.AdjType, r.ChangedBy, r.Reason });

            AdjustmentReportPrintHelper.PrintOrPreview(title, headers, widths, rows, "Product History | Printed by " + UsersModal.logged_in_username, true, this);
        }

        private void ExportCsv()
        {
            if (_filtered == null || _filtered.Count == 0)
            {
                MessageBox.Show(this, "Load data first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var headers = new[] { "Date/Time", "Change Type", "Old Value", "New Value", "Adj No", "Adj Type", "Changed By", "Reason" };
            var rows = new List<string[]>();
            foreach (var r in _filtered)
                rows.Add(new[] { r.ChangedAt.ToString("yyyy-MM-dd HH:mm"), r.ChangeType, r.OldValue, r.NewValue, r.AdjNo, r.AdjType, r.ChangedBy, r.Reason });

            AdjustmentReportPrintHelper.ExportToCsv("Stock_History", headers, rows, this);
        }

        private static string FormatChangeType(string ct)
        {
            switch (ct)
            {
                case "QTY_CHANGE": return "Qty Change";
                case "PRICE_CHANGE": return "Price Change";
                case "LOCATION_CHANGE": return "Location Change";
                default: return ct;
            }
        }

        private static Label MakeLbl(string text) => new Label { Text = text, AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0, 5, 4, 0) };

        private static Button MakeBtn(string text, Color back)
            => new Button { Text = text, FlatStyle = FlatStyle.Flat, BackColor = back, ForeColor = Color.White, Height = 26, AutoSize = true, Margin = new Padding(0, 2, 6, 0) };
    }
}
