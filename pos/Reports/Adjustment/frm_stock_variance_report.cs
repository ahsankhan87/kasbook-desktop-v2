using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace pos.Reports.Adjustment
{
    /// <summary>
    /// Report 2 — Stock Variance Report
    /// Products where physical count differed from system by more than a configurable threshold.
    /// </summary>
    public partial class frm_stock_variance_report : Form
    {
        private readonly StockAdjustmentBLL _bll = new StockAdjustmentBLL();

        private DateTimePicker _dtpFrom;
        private DateTimePicker _dtpTo;
        private NumericUpDown _nudThreshold;
        private Button _btnLoad;
        private Button _btnPreview;
        private Button _btnPrint;
        private Button _btnExport;
        private DataGridView _grid;
        private Label _lblSummary;

        private List<StockVarianceRow> _allRows = new List<StockVarianceRow>();
        private List<StockVarianceRow> _filtered = new List<StockVarianceRow>();

        public frm_stock_variance_report()
        {
            BuildUi();
        }

        private void BuildUi()
        {
            Text = "Stock Variance Report";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;

            var top = new Panel { Dock = DockStyle.Top, Height = 46, BackColor = Color.FromArgb(245, 247, 250), Padding = new Padding(8, 8, 8, 0) };
            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };

            flow.Controls.Add(MakeLbl("From:"));
            _dtpFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddMonths(-1), Width = 100, Margin = new Padding(0, 3, 8, 0) };
            flow.Controls.Add(_dtpFrom);

            flow.Controls.Add(MakeLbl("To:"));
            _dtpTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today, Width = 100, Margin = new Padding(0, 3, 8, 0) };
            flow.Controls.Add(_dtpTo);

            flow.Controls.Add(MakeLbl("Min Adjustments:"));
            _nudThreshold = new NumericUpDown { Minimum = 0, Maximum = 9999, Value = 0, Width = 70, Margin = new Padding(0, 3, 8, 0), DecimalPlaces = 0 };
            flow.Controls.Add(_nudThreshold);

            _btnLoad = MakeBtn("Load", Color.FromArgb(21, 101, 192));
            _btnPreview = MakeBtn("Preview", Color.FromArgb(46, 125, 50));
            _btnPrint = MakeBtn("Print", Color.FromArgb(46, 125, 50));
            _btnExport = MakeBtn("Export CSV", Color.FromArgb(84, 110, 122));

            flow.Controls.Add(_btnLoad);
            flow.Controls.Add(_btnPreview);
            flow.Controls.Add(_btnPrint);
            flow.Controls.Add(_btnExport);
            top.Controls.Add(flow);

            _lblSummary = new Label { Dock = DockStyle.Bottom, Height = 22, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.DimGray, Padding = new Padding(8, 0, 0, 0) };

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

            AddCol("Product Code", 110);
            AddCol("Product Name", 200);
            AddCol("Category", 120);
            AddCol("Total Adjustments", 110, DataGridViewContentAlignment.MiddleRight);
            AddCol("Total Qty Adjusted", 120, DataGridViewContentAlignment.MiddleRight);
            AddCol("Total Value Impact", 120, DataGridViewContentAlignment.MiddleRight);
            AddCol("Last Adjustment", 110);

            _grid.CellFormatting += Grid_CellFormatting;

            Controls.Add(_grid);
            Controls.Add(_lblSummary);
            Controls.Add(top);

            _btnLoad.Click += (s, e) => LoadData();
            _btnPreview.Click += (s, e) => PrintReport(preview: true);
            _btnPrint.Click += (s, e) => PrintReport(preview: false);
            _btnExport.Click += (s, e) => ExportCsv();
        }

        private void Grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _filtered.Count)
                return;

            var row = _filtered[e.RowIndex];
            // Highlight high-volume adjusters in red
            if (row.TotalAdjustments >= 10)
                _grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 238, 238);
            else if (row.TotalAdjustments >= 5)
                _grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 253, 208);
        }

        private void LoadData()
        {
            try
            {
                _allRows = _bll.GetStockVarianceReport(_dtpFrom.Value, _dtpTo.Value);
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter()
        {
            int minAdj = (int)_nudThreshold.Value;
            _filtered = _allRows.Where(x => x.TotalAdjustments >= minAdj).ToList();

            _grid.Rows.Clear();
            foreach (var r in _filtered)
            {
                _grid.Rows.Add(
                    r.ProductCode,
                    r.ProductName,
                    r.CategoryName,
                    r.TotalAdjustments.ToString("N0"),
                    r.TotalQtyAdjusted.ToString("N2"),
                    r.TotalValueImpact.ToString("N2"),
                    r.LastAdjustmentDate.ToString("yyyy-MM-dd"));
            }

            _lblSummary.Text = string.Format(
                "{0} products shown  |  Total adjustments: {1}  |  Total value impact: {2:N2}",
                _filtered.Count,
                _filtered.Sum(x => x.TotalAdjustments),
                _filtered.Sum(x => x.TotalValueImpact));
        }

        private void PrintReport(bool preview)
        {
            if (_filtered == null || _filtered.Count == 0)
            {
                MessageBox.Show(this, "Load data first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string title = string.Format("Stock Variance Report  {0} – {1}", _dtpFrom.Value.ToString("yyyy-MM-dd"), _dtpTo.Value.ToString("yyyy-MM-dd"));
            var headers = new[] { "Code", "Product Name", "Category", "Adjustments", "Qty Adjusted", "Value Impact", "Last Adj" };
            var widths = new[] { 90, 180, 110, 80, 90, 90, 88 };

            var rows = new List<string[]>();
            foreach (var r in _filtered)
            {
                rows.Add(new[] { r.ProductCode, r.ProductName, r.CategoryName, r.TotalAdjustments.ToString("N0"), r.TotalQtyAdjusted.ToString("N2"), r.TotalValueImpact.ToString("N2"), r.LastAdjustmentDate.ToString("yyyy-MM-dd") });
            }

            string footer = string.Format("{0} products | Printed by {1}", _filtered.Count, UsersModal.logged_in_username);
            AdjustmentReportPrintHelper.PrintOrPreview(title, headers, widths, rows, footer, preview, this);
        }

        private void ExportCsv()
        {
            if (_filtered == null || _filtered.Count == 0)
            {
                MessageBox.Show(this, "Load data first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var headers = new[] { "Product Code", "Product Name", "Category", "Total Adjustments", "Total Qty Adjusted", "Total Value Impact", "Last Adjustment Date" };
            var rows = new List<string[]>();
            foreach (var r in _filtered)
                rows.Add(new[] { r.ProductCode, r.ProductName, r.CategoryName, r.TotalAdjustments.ToString(), r.TotalQtyAdjusted.ToString("N2"), r.TotalValueImpact.ToString("N2"), r.LastAdjustmentDate.ToString("yyyy-MM-dd") });

            AdjustmentReportPrintHelper.ExportToCsv("Stock_Variance", headers, rows, this);
        }

        private void AddCol(string header, int width, DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleLeft)
        {
            _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = header, Width = width, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = align } });
        }

        private static Label MakeLbl(string text) => new Label { Text = text, AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0, 5, 4, 0) };

        private static Button MakeBtn(string text, Color back)
        {
            return new Button { Text = text, FlatStyle = FlatStyle.Flat, BackColor = back, ForeColor = Color.White, Height = 26, AutoSize = true, Margin = new Padding(0, 2, 6, 0) };
        }
    }
}
