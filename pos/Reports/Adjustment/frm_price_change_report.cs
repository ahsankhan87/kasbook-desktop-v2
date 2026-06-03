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
    /// Report 3 — Price Change Report
    /// All products whose price was changed in a date range, with old vs new price, % change, and approver.
    /// </summary>
    public partial class frm_price_change_report : Form
    {
        private readonly StockAdjustmentBLL _bll = new StockAdjustmentBLL();

        private DateTimePicker _dtpFrom;
        private DateTimePicker _dtpTo;
        private Button _btnLoad;
        private Button _btnPreview;
        private Button _btnPrint;
        private Button _btnExport;
        private DataGridView _grid;
        private Label _lblSummary;

        private List<PriceChangeRow> _rows = new List<PriceChangeRow>();

        public frm_price_change_report()
        {
            BuildUi();
        }

        private void BuildUi()
        {
            Text = "Price Change Report";
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
            AddCol("Adj No", 100);
            AddCol("Date", 86);
            AddCol("Old Price", 90, DataGridViewContentAlignment.MiddleRight);
            AddCol("New Price", 90, DataGridViewContentAlignment.MiddleRight);
            AddCol("% Change", 80, DataGridViewContentAlignment.MiddleRight);
            AddCol("Approved By", 120);
            AddCol("Reason", 180);

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
            if (e.RowIndex < 0 || e.RowIndex >= _rows.Count)
                return;

            var row = _rows[e.RowIndex];
            // colour code: price increases = light green, decreases = light red
            if (row.PctChange > 0)
                _grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
            else if (row.PctChange < 0)
                _grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 238, 238);
        }

        private void LoadData()
        {
            try
            {
                _rows = _bll.GetPriceChangeReport(_dtpFrom.Value, _dtpTo.Value);

                _grid.Rows.Clear();
                foreach (var r in _rows)
                {
                    _grid.Rows.Add(
                        r.ProductCode, r.ProductName, r.CategoryName,
                        r.AdjNo, r.ChangeDate,
                        r.OldPrice.ToString("N2"),
                        r.NewPrice.ToString("N2"),
                        r.PctChange.ToString("N2") + "%",
                        r.ApprovedBy, r.Reason);
                }

                _lblSummary.Text = string.Format(
                    "{0} price changes  |  Avg change: {1:N2}%  |  Increases: {2}  |  Decreases: {3}",
                    _rows.Count,
                    _rows.Count > 0 ? _rows.Average(x => x.PctChange) : 0,
                    _rows.Count(x => x.PctChange > 0),
                    _rows.Count(x => x.PctChange < 0));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintReport(bool preview)
        {
            if (_rows == null || _rows.Count == 0)
            {
                MessageBox.Show(this, "Load data first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string title = string.Format("Price Change Report  {0} – {1}", _dtpFrom.Value.ToString("yyyy-MM-dd"), _dtpTo.Value.ToString("yyyy-MM-dd"));
            var headers = new[] { "Code", "Product Name", "Category", "Adj No", "Date", "Old Price", "New Price", "% Chg", "Approved By", "Reason" };
            var widths = new[] { 90, 160, 100, 90, 76, 76, 76, 60, 100, 150 };

            var rows = new List<string[]>();
            foreach (var r in _rows)
            {
                rows.Add(new[] { r.ProductCode, r.ProductName, r.CategoryName, r.AdjNo, r.ChangeDate, r.OldPrice.ToString("N2"), r.NewPrice.ToString("N2"), r.PctChange.ToString("N2") + "%", r.ApprovedBy, r.Reason });
            }

            string footer = string.Format("{0} changes | Printed by {1}", _rows.Count, UsersModal.logged_in_username);
            AdjustmentReportPrintHelper.PrintOrPreview(title, headers, widths, rows, footer, preview, this);
        }

        private void ExportCsv()
        {
            if (_rows == null || _rows.Count == 0)
            {
                MessageBox.Show(this, "Load data first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var headers = new[] { "Product Code", "Product Name", "Category", "Adj No", "Date", "Old Price", "New Price", "% Change", "Approved By", "Reason" };
            var rows = new List<string[]>();
            foreach (var r in _rows)
                rows.Add(new[] { r.ProductCode, r.ProductName, r.CategoryName, r.AdjNo, r.ChangeDate, r.OldPrice.ToString("N2"), r.NewPrice.ToString("N2"), r.PctChange.ToString("N2"), r.ApprovedBy, r.Reason });

            AdjustmentReportPrintHelper.ExportToCsv("Price_Change", headers, rows, this);
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
