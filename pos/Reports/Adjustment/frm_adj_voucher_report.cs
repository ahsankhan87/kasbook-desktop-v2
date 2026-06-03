using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace pos.Reports.Adjustment
{
    /// <summary>
    /// Report 1 — Adjustment Voucher
    /// Shows header info, all adjusted products with before/after values, totals,
    /// and a signatures section (Counted By / Verified By / Approved By).
    /// </summary>
    public partial class frm_adj_voucher_report : Form
    {
        private readonly StockAdjustmentBLL _bll = new StockAdjustmentBLL();

        // Controls
        private DateTimePicker _dtpFrom;
        private DateTimePicker _dtpTo;
        private ComboBox _cmbStatus;
        private Button _btnLoad;
        private Button _btnPreview;
        private Button _btnPrint;
        private Button _btnExport;
        private DataGridView _grid;
        private Label _lblSummary;

        private List<AdjSessionListRow> _sessions = new List<AdjSessionListRow>();

        public frm_adj_voucher_report()
        {
            BuildUi();
        }

        private void BuildUi()
        {
            Text = "Adjustment Voucher Report";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;

            var top = new Panel { Dock = DockStyle.Top, Height = 46, BackColor = Color.FromArgb(245, 247, 250), Padding = new Padding(8, 8, 8, 0) };
            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };

            flow.Controls.Add(new Label { Text = "From:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0, 5, 4, 0) });
            _dtpFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddMonths(-1), Width = 100, Margin = new Padding(0, 3, 8, 0) };
            flow.Controls.Add(_dtpFrom);

            flow.Controls.Add(new Label { Text = "To:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0, 5, 4, 0) });
            _dtpTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today, Width = 100, Margin = new Padding(0, 3, 8, 0) };
            flow.Controls.Add(_dtpTo);

            flow.Controls.Add(new Label { Text = "Status:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Margin = new Padding(0, 5, 4, 0) });
            _cmbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100, Margin = new Padding(0, 3, 8, 0) };
            _cmbStatus.Items.AddRange(new object[] { "All", "Draft", "Posted" });
            _cmbStatus.SelectedIndex = 0;
            flow.Controls.Add(_cmbStatus);

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

            AddCol("Adj No", 100);
            AddCol("Date", 86);
            AddCol("Type", 110);
            AddCol("Status", 70);
            AddCol("Products", 70, DataGridViewContentAlignment.MiddleRight);
            AddCol("Qty+", 56, DataGridViewContentAlignment.MiddleRight);
            AddCol("Qty-", 56, DataGridViewContentAlignment.MiddleRight);
            AddCol("Price Chg", 70, DataGridViewContentAlignment.MiddleRight);
            AddCol("Loc Chg", 60, DataGridViewContentAlignment.MiddleRight);
            AddCol("Created By", 110);
            AddCol("Posted By", 110);
            AddCol("Notes", 200);

            Controls.Add(_grid);
            Controls.Add(_lblSummary);
            Controls.Add(top);

            _btnLoad.Click += (s, e) => LoadData();
            _btnPreview.Click += (s, e) => PrintReport(preview: true);
            _btnPrint.Click += (s, e) => PrintReport(preview: false);
            _btnExport.Click += (s, e) => ExportCsv();
        }

        private void LoadData()
        {
            try
            {
                string status = _cmbStatus.SelectedIndex == 0 ? null : _cmbStatus.SelectedItem.ToString();
                _sessions = _bll.GetAdjustmentSessions(_dtpFrom.Value, _dtpTo.Value, status);

                _grid.Rows.Clear();
                foreach (var s in _sessions)
                {
                    _grid.Rows.Add(
                        s.AdjNo, s.AdjDate, s.AdjType, s.Status,
                        s.ProductCount, s.QtyIncreases, s.QtyDecreases,
                        s.PriceChanges, s.LocationChanges,
                        s.CreatedBy, s.PostedBy, s.Notes);
                }

                _lblSummary.Text = string.Format(
                    "{0} sessions  |  {1} total products  |  {2} qty increases  |  {3} qty decreases  |  {4} price changes",
                    _sessions.Count,
                    _sessions.Sum(x => x.ProductCount),
                    _sessions.Sum(x => x.QtyIncreases),
                    _sessions.Sum(x => x.QtyDecreases),
                    _sessions.Sum(x => x.PriceChanges));

                // Row coloring by status
                for (int i = 0; i < _grid.Rows.Count; i++)
                {
                    string st = Convert.ToString(_grid.Rows[i].Cells[3].Value);
                    if (string.Equals(st, "Posted", StringComparison.OrdinalIgnoreCase))
                        _grid.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
                    else if (string.Equals(st, "Draft", StringComparison.OrdinalIgnoreCase))
                        _grid.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 253, 208);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintReport(bool preview)
        {
            if (_sessions == null || _sessions.Count == 0)
            {
                MessageBox.Show(this, "Load data first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string title = string.Format("Adjustment Voucher Report  {0} – {1}", _dtpFrom.Value.ToString("yyyy-MM-dd"), _dtpTo.Value.ToString("yyyy-MM-dd"));

            var headers = new[] { "Adj No", "Date", "Type", "Status", "Products", "Qty+", "Qty-", "Price Chg", "Loc Chg", "Created By", "Posted By" };
            var widths = new[] { 90, 76, 100, 64, 62, 46, 46, 60, 52, 90, 90 };

            var rows = new List<string[]>();
            foreach (var s in _sessions)
            {
                rows.Add(new[]
                {
                    s.AdjNo, s.AdjDate, s.AdjType, s.Status,
                    s.ProductCount.ToString(),
                    s.QtyIncreases.ToString(),
                    s.QtyDecreases.ToString(),
                    s.PriceChanges.ToString(),
                    s.LocationChanges.ToString(),
                    s.CreatedBy, s.PostedBy
                });
            }

            // Signatures footer appended as data rows for printing
            rows.Add(new string[headers.Length]);
            rows.Add(new[] { "Counted By: ___________________", string.Empty, string.Empty, string.Empty, "Verified By: ___________________", string.Empty, string.Empty, string.Empty, string.Empty, "Approved By: ___________________", string.Empty });

            string footer = string.Format("{0} sessions | Printed by {1}", _sessions.Count, UsersModal.logged_in_username);
            AdjustmentReportPrintHelper.PrintOrPreview(title, headers, widths, rows, footer, preview, this);
        }

        private void ExportCsv()
        {
            if (_sessions == null || _sessions.Count == 0)
            {
                MessageBox.Show(this, "Load data first.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var headers = new[] { "Adj No", "Date", "Type", "Status", "Products", "Qty+", "Qty-", "Price Chg", "Loc Chg", "Created By", "Posted By", "Notes" };
            var rows = new List<string[]>();
            foreach (var s in _sessions)
            {
                rows.Add(new[]
                {
                    s.AdjNo, s.AdjDate, s.AdjType, s.Status,
                    s.ProductCount.ToString(), s.QtyIncreases.ToString(),
                    s.QtyDecreases.ToString(), s.PriceChanges.ToString(),
                    s.LocationChanges.ToString(), s.CreatedBy, s.PostedBy, s.Notes
                });
            }

            AdjustmentReportPrintHelper.ExportToCsv("Adjustment_Voucher", headers, rows, this);
        }

        private void AddCol(string header, int width, DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleLeft)
        {
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = header,
                Width = width,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = align }
            });
        }

        private static Button MakeBtn(string text, Color back)
        {
            return new Button
            {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                BackColor = back,
                ForeColor = Color.White,
                Height = 26,
                AutoSize = true,
                Margin = new Padding(0, 2, 6, 0)
            };
        }
    }
}
