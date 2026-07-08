using DGVPrinterHelper;
using pos.Reports.Common;
using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace pos
{
    public class frm_journal_voucher_manager : Form
    {
        private readonly JournalsBLL _bll = new JournalsBLL();

        private DateTimePicker _dtpFrom;
        private DateTimePicker _dtpTo;
        private ComboBox _cmbVoucherType;
        private ComboBox _cmbStatus;
        private TextBox _txtSearch;
        private Button _btnSearch;
        private Button _btnClear;
        private Button _btnRefresh;
        private Button _btnPostSelected;
        private Button _btnReverseSelected;
        private Button _btnDeleteSelected;
        private Button _btnExport;
        private DataGridView _grid;
        private DataGridView _previewGrid;
        private Label _lblPreviewTitle;
        private Label _lblPreviewDr;
        private Label _lblPreviewCr;
        private Label _lblPreviewBalance;
        private Button _btnEdit;
        private Button _btnPost;
        private Button _btnPrint;
        private Button _btnReverse;
        private StatusStrip _statusStrip;
        private ToolStripStatusLabel _lblViewCount;
        private ToolStripStatusLabel _lblPostedCount;
        private ToolStripStatusLabel _lblDraftCount;
        private ToolStripStatusLabel _lblDebitSum;

        private DataTable _voucherTable;
        private DataTable _previewTable;

        public frm_journal_voucher_manager()
        {
            BuildUi();
        }

        private void BuildUi()
        {
            Text = "Journal Voucher List & Posting Manager";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 8.75F, FontStyle.Regular, GraphicsUnit.Point);

            var filters = new Panel { Dock = DockStyle.Top, Height = 54, BackColor = Color.FromArgb(245, 247, 250), Padding = new Padding(10, 8, 10, 6) };
            var filterFlow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, AutoScroll = true };
            filters.Controls.Add(filterFlow);

            filterFlow.Controls.Add(MakeLabel("Date From"));
            _dtpFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddMonths(-1), Width = 102 };
            filterFlow.Controls.Add(_dtpFrom);
            filterFlow.Controls.Add(MakeLabel("Date To"));
            _dtpTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today, Width = 102 };
            filterFlow.Controls.Add(_dtpTo);
            filterFlow.Controls.Add(MakeLabel("Voucher Type"));
            _cmbVoucherType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150 };
            _cmbVoucherType.Items.AddRange(new object[] { "All", "General Journal", "Opening Entry", "Adjusting Entry", "Closing Entry", "Reversal Entry" });
            _cmbVoucherType.SelectedIndex = 0;
            filterFlow.Controls.Add(_cmbVoucherType);
            filterFlow.Controls.Add(MakeLabel("Status"));
            _cmbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 105 };
            _cmbStatus.Items.AddRange(new object[] { "All", "Draft", "Posted", "Reversed" });
            _cmbStatus.SelectedIndex = 0;
            filterFlow.Controls.Add(_cmbStatus);
            filterFlow.Controls.Add(MakeLabel("Search"));
            _txtSearch = new TextBox { Width = 230 };
            filterFlow.Controls.Add(_txtSearch);
            _btnSearch = MakeButton("Search", Color.FromArgb(21, 101, 192));
            _btnClear = MakeButton("Clear", Color.FromArgb(96, 125, 139));
            _btnRefresh = MakeButton("Refresh", Color.FromArgb(46, 125, 50));
            filterFlow.Controls.Add(_btnSearch);
            filterFlow.Controls.Add(_btnClear);
            filterFlow.Controls.Add(_btnRefresh);

            var batch = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White, Padding = new Padding(10, 5, 10, 5) };
            var batchFlow = new FlowLayoutPanel { Dock = DockStyle.Left, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };
            batch.Controls.Add(batchFlow);
            _btnPostSelected = MakeButton("Post Selected", Color.FromArgb(46, 125, 50));
            _btnReverseSelected = MakeButton("Reverse Selected", Color.FromArgb(192, 57, 43));
            _btnDeleteSelected = MakeButton("Delete Selected", Color.FromArgb(231, 76, 60));
            _btnExport = MakeButton("Export to Excel", Color.FromArgb(84, 110, 122));
            batchFlow.Controls.Add(_btnPostSelected);
            batchFlow.Controls.Add(_btnReverseSelected);
            batchFlow.Controls.Add(_btnDeleteSelected);
            batchFlow.Controls.Add(_btnExport);

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = true
            };
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            _grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _grid.ColumnHeadersHeight = 32;
            _grid.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colSelect", HeaderText = string.Empty, Width = 30 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "VoucherNo", HeaderText = "Voucher No", Width = 130, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "VoucherDate", HeaderText = "Date", Width = 90, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "VoucherType", HeaderText = "Type", Width = 140, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Narration", HeaderText = "Narration", Width = 230, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "LinesCount", HeaderText = "Lines", Width = 60, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalDebit", HeaderText = "Total Debit", Width = 110, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalCredit", HeaderText = "Total Credit", Width = 110, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", Width = 90, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedBy", HeaderText = "Created By", Width = 130, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "PostedBy", HeaderText = "Posted By", Width = 130, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewButtonColumn { Name = "Actions", HeaderText = "Actions", Width = 85, Text = "Open", UseColumnTextForButtonValue = true });

            var previewPanel = new Panel { Dock = DockStyle.Bottom, Height = 230, BackColor = Color.FromArgb(250, 251, 253), Padding = new Padding(10) };
            var previewLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            previewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            previewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230F));

            var previewLeft = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 0, 10, 0) };
            _lblPreviewTitle = new Label { Dock = DockStyle.Top, Height = 20, Text = "Detail Preview", Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
            _previewGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                EnableHeadersVisualStyles = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            _previewGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            _previewGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _previewGrid.ColumnHeadersHeight = 28;
            _previewGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "AccountCode", HeaderText = "Account Code", ReadOnly = true, Width = 90 });
            _previewGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "AccountName", HeaderText = "Account Name", ReadOnly = true, Width = 150 });
            _previewGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Description", HeaderText = "Description", ReadOnly = true });
            _previewGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Debit", HeaderText = "Debit", ReadOnly = true, Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            _previewGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Credit", HeaderText = "Credit", ReadOnly = true, Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            previewLeft.Controls.Add(_previewGrid);
            previewLeft.Controls.Add(_lblPreviewTitle);

            var previewRight = new Panel { Dock = DockStyle.Fill, BackColor = Color.WhiteSmoke, Padding = new Padding(12) };
            _lblPreviewDr = new Label { AutoSize = true, Location = new Point(12, 18), Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.MidnightBlue, Text = "Debit: 0.00" };
            _lblPreviewCr = new Label { AutoSize = true, Location = new Point(12, 48), Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.MidnightBlue, Text = "Credit: 0.00" };
            _lblPreviewBalance = new Label { AutoSize = true, Location = new Point(12, 78), Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.DarkGreen, Text = "Balanced ✓" };
            _btnEdit = MakeButton("Edit", Color.FromArgb(21, 101, 192));
            _btnPost = MakeButton("Post", Color.FromArgb(46, 125, 50));
            _btnPrint = MakeButton("Print", Color.FromArgb(84, 110, 122));
            _btnReverse = MakeButton("Reverse", Color.FromArgb(192, 57, 43));
            _btnEdit.SetBounds(12, 116, 86, 30);
            _btnPost.SetBounds(102, 116, 86, 30);
            _btnPrint.SetBounds(12, 152, 86, 30);
            _btnReverse.SetBounds(102, 152, 86, 30);
            previewRight.Controls.Add(_lblPreviewDr);
            previewRight.Controls.Add(_lblPreviewCr);
            previewRight.Controls.Add(_lblPreviewBalance);
            previewRight.Controls.Add(_btnEdit);
            previewRight.Controls.Add(_btnPost);
            previewRight.Controls.Add(_btnPrint);
            previewRight.Controls.Add(_btnReverse);

            previewLayout.Controls.Add(previewLeft, 0, 0);
            previewLayout.Controls.Add(previewRight, 1, 0);
            previewPanel.Controls.Add(previewLayout);

            _statusStrip = new StatusStrip { SizingGrip = false };
            _lblViewCount = new ToolStripStatusLabel("Total vouchers in view: 0");
            _lblPostedCount = new ToolStripStatusLabel("Posted: 0");
            _lblDraftCount = new ToolStripStatusLabel("Draft: 0");
            _lblDebitSum = new ToolStripStatusLabel("Filtered debit sum: 0.00");
            _statusStrip.Items.AddRange(new ToolStripItem[] { _lblViewCount, _lblPostedCount, _lblDraftCount, _lblDebitSum });

            Controls.Add(_grid);
            Controls.Add(previewPanel);
            Controls.Add(_statusStrip);
            Controls.Add(batch);
            Controls.Add(filters);

            _btnSearch.Click += (s, e) => LoadData();
            _btnRefresh.Click += (s, e) => LoadData();
            _btnClear.Click += (s, e) => ClearFilters();
            _btnPostSelected.Click += (s, e) => PostSelected();
            _btnReverseSelected.Click += (s, e) => ReverseSelected();
            _btnDeleteSelected.Click += (s, e) => DeleteSelected();
            _btnExport.Click += (s, e) => ExportToExcel();
            _btnEdit.Click += (s, e) => EditSelected();
            _btnPost.Click += (s, e) => PostSelected();
            _btnPrint.Click += (s, e) => PrintSelectedPreview();
            _btnReverse.Click += (s, e) => ReverseSelected();
            _grid.SelectionChanged += (s, e) => UpdatePreviewFromSelection();
            _grid.CellContentClick += Grid_CellContentClick;
            _grid.CellDoubleClick += Grid_CellDoubleClick;
            _grid.DataBindingComplete += (s, e) => UpdatePreviewFromSelection();
            Load += (s, e) => LoadData();
        }

        private Label MakeLabel(string text)
        {
            return new Label { AutoSize = true, Text = text, Margin = new Padding(0, 7, 4, 0) };
        }

        private Button MakeButton(string text, Color back)
        {
            return new Button
            {
                Text = text,
                BackColor = back,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Height = 30,
                AutoSize = true,
                Margin = new Padding(0, 2, 6, 0)
            };
        }

        private void ClearFilters()
        {
            _dtpFrom.Value = DateTime.Today.AddMonths(-1);
            _dtpTo.Value = DateTime.Today;
            _cmbVoucherType.SelectedIndex = 0;
            _cmbStatus.SelectedIndex = 0;
            _txtSearch.Clear();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string voucherType = Convert.ToString(_cmbVoucherType.SelectedItem);
                string status = Convert.ToString(_cmbStatus.SelectedItem);
                string search = _txtSearch.Text.Trim();
                _voucherTable = _bll.GetVoucherHeaders(_dtpFrom.Value, _dtpTo.Value, voucherType, status, search);
                RenderGrid();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenderGrid()
        {
            _grid.Rows.Clear();
            if (_voucherTable == null)
            {
                return;
            }

            foreach (DataRow row in _voucherTable.Rows)
            {
                int idx = _grid.Rows.Add();
                DataGridViewRow gridRow = _grid.Rows[idx];
                gridRow.Tag = row;
                gridRow.Cells["VoucherNo"].Value = Convert.ToString(row["VoucherNo"]);
                gridRow.Cells["VoucherDate"].Value = row["VoucherDate"] == DBNull.Value ? string.Empty : Convert.ToDateTime(row["VoucherDate"]).ToShortDateString();
                gridRow.Cells["VoucherType"].Value = Convert.ToString(row["VoucherType"]);
                gridRow.Cells["Narration"].Value = Truncate(Convert.ToString(row["Narration"]), 60);
                gridRow.Cells["LinesCount"].Value = Convert.ToInt32(row["LinesCount"]);
                gridRow.Cells["TotalDebit"].Value = row["TotalDebit"];
                gridRow.Cells["TotalCredit"].Value = row["TotalCredit"];
                gridRow.Cells["Status"].Value = Convert.ToString(row["Status"]);
                gridRow.Cells["CreatedBy"].Value = Convert.ToString(row["CreatedBy"]);
                gridRow.Cells["PostedBy"].Value = Convert.ToString(row["PostedBy"]);
                ApplyRowStyle(gridRow);
            }

            if (_grid.Rows.Count > 0)
            {
                _grid.Rows[0].Selected = true;
            }
            UpdatePreviewFromSelection();
        }

        private void ApplyRowStyle(DataGridViewRow row)
        {
            string status = Convert.ToString(row.Cells["Status"].Value);
            if (string.Equals(status, "Posted", StringComparison.OrdinalIgnoreCase))
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
            }
            else if (string.Equals(status, "Reversed", StringComparison.OrdinalIgnoreCase))
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void UpdateStatusBar()
        {
            if (_voucherTable == null)
            {
                _lblViewCount.Text = "Total vouchers in view: 0";
                _lblPostedCount.Text = "Posted: 0";
                _lblDraftCount.Text = "Draft: 0";
                _lblDebitSum.Text = "Filtered debit sum: 0.00";
                return;
            }

            int total = _voucherTable.Rows.Count;
            int posted = _voucherTable.AsEnumerable().Count(r => string.Equals(Convert.ToString(r["Status"]), "Posted", StringComparison.OrdinalIgnoreCase));
            int draft = _voucherTable.AsEnumerable().Count(r => string.Equals(Convert.ToString(r["Status"]), "Draft", StringComparison.OrdinalIgnoreCase));
            decimal debitSum = _voucherTable.AsEnumerable().Sum(r => r["TotalDebit"] == DBNull.Value ? 0m : Convert.ToDecimal(r["TotalDebit"]));

            _lblViewCount.Text = string.Format("Total vouchers in view: {0}", total);
            _lblPostedCount.Text = string.Format("Posted: {0}", posted);
            _lblDraftCount.Text = string.Format("Draft: {0}", draft);
            _lblDebitSum.Text = string.Format("Filtered debit sum: {0:N2}", debitSum);
        }

        private void UpdatePreviewFromSelection()
        {
            DataGridViewRow selected = GetSingleFocusedRow();
            if (selected == null)
            {
                ClearPreview();
                return;
            }

            DataRow headerRow = selected.Tag as DataRow;
            if (headerRow == null)
            {
                ClearPreview();
                return;
            }

            string invoiceNo = Convert.ToString(headerRow["VoucherNo"]);
            _lblPreviewTitle.Text = string.Format("Detail Preview - {0}", invoiceNo);
            _previewTable = _bll.GetVoucherLines(invoiceNo);
            _previewGrid.Rows.Clear();

            decimal dr = 0m;
            decimal cr = 0m;
            if (_previewTable != null)
            {
                foreach (DataRow line in _previewTable.Rows)
                {
                    decimal debit = line["Debit"] == DBNull.Value ? 0m : Convert.ToDecimal(line["Debit"]);
                    decimal credit = line["Credit"] == DBNull.Value ? 0m : Convert.ToDecimal(line["Credit"]);
                    dr += debit;
                    cr += credit;
                    _previewGrid.Rows.Add(Convert.ToString(line["AccountCode"]), Convert.ToString(line["AccountName"]), Convert.ToString(line["Description"]), debit, credit);
                }
            }

            _lblPreviewDr.Text = string.Format("Debit: {0:N2}", dr);
            _lblPreviewCr.Text = string.Format("Credit: {0:N2}", cr);
            bool balanced = Math.Abs(dr - cr) < 0.005m;
            _lblPreviewBalance.Text = balanced ? "Balanced ✓" : "Not Balanced";
            _lblPreviewBalance.ForeColor = balanced ? Color.DarkGreen : Color.DarkRed;

            SetQuickButtons(headerRow);
        }

        private void ClearPreview()
        {
            _lblPreviewTitle.Text = "Detail Preview";
            _previewGrid.Rows.Clear();
            _lblPreviewDr.Text = "Debit: 0.00";
            _lblPreviewCr.Text = "Credit: 0.00";
            _lblPreviewBalance.Text = "Balanced ✓";
            _lblPreviewBalance.ForeColor = Color.DarkGreen;
            _btnEdit.Enabled = false;
            _btnPost.Enabled = false;
            _btnReverse.Enabled = false;
        }

        private void SetQuickButtons(DataRow headerRow)
        {
            string status = Convert.ToString(headerRow["Status"]);
            _btnEdit.Enabled = string.Equals(status, "Draft", StringComparison.OrdinalIgnoreCase);
            _btnPost.Enabled = string.Equals(status, "Draft", StringComparison.OrdinalIgnoreCase);
            _btnReverse.Enabled = string.Equals(status, "Posted", StringComparison.OrdinalIgnoreCase);
        }

        private DataGridViewRow GetSingleFocusedRow()
        {
            if (_grid.SelectedRows.Count > 0)
            {
                return _grid.SelectedRows[0];
            }

            return _grid.Rows.Count > 0 ? _grid.Rows[0] : null;
        }

        private List<DataGridViewRow> GetCheckedRows()
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in _grid.Rows)
            {
                object checkedValue = row.Cells["colSelect"].Value;
                if (checkedValue is bool && (bool)checkedValue)
                {
                    rows.Add(row);
                }
            }
            return rows;
        }

        private List<DataRow> GetCheckedHeaderRows(string requiredStatus = null)
        {
            List<DataRow> rows = new List<DataRow>();
            foreach (DataGridViewRow gridRow in GetCheckedRows())
            {
                DataRow dataRow = gridRow.Tag as DataRow;
                if (dataRow == null)
                {
                    continue;
                }

                string status = Convert.ToString(dataRow["Status"]);
                if (!string.IsNullOrWhiteSpace(requiredStatus) && !string.Equals(status, requiredStatus, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                rows.Add(dataRow);
            }
            return rows;
        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (_grid.Columns[e.ColumnIndex].Name == "Actions")
            {
                OpenSelectedVoucher(_grid.Rows[e.RowIndex]);
                return;
            }
        }

        private void Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            OpenSelectedVoucher(_grid.Rows[e.RowIndex]);
        }

        private void OpenSelectedVoucher(DataGridViewRow row)
        {
            DataRow headerRow = row.Tag as DataRow;
            if (headerRow == null)
            {
                return;
            }

            string status = Convert.ToString(headerRow["Status"]);
            string invoiceNo = Convert.ToString(headerRow["VoucherNo"]);

            if (string.Equals(status, "Draft", StringComparison.OrdinalIgnoreCase))
            {
                var editor = new frm_journal_entries();
                if (editor.LoadVoucherForEdit(invoiceNo))
                {
                    editor.ShowDialog(this);
                    LoadData();
                }
                return;
            }

            UpdatePreviewFromSelection();
        }

        private void EditSelected()
        {
            DataGridViewRow row = GetSingleFocusedRow();
            if (row == null)
            {
                return;
            }

            OpenSelectedVoucher(row);
        }

        private void PostSelected()
        {
            List<DataRow> drafts = GetCheckedHeaderRows("Draft");
            if (drafts.Count == 0)
            {
                MessageBox.Show(this, "Select one or more Draft vouchers.", "Post Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this, string.Format("Post {0} vouchers?", drafts.Count), "Confirm Post", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            List<int> ids = drafts.Select(r => Convert.ToInt32(r["id"])).ToList();
            BatchPostResult result = _bll.BatchPostVouchers(ids, UsersModal.logged_in_userid);
            string message = string.Format("Posted: {0}\r\nFailed: {1}", result.SuccessCount, result.FailureCount);
            if (result.FailedVouchers.Count > 0)
            {
                message += "\r\n\r\n" + string.Join("\r\n", result.FailedVouchers.Select(x => string.Format("{0}: {1}", x.VoucherNo, x.Message)));
            }

            MessageBox.Show(this, message, "Batch Post", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }

        private void DeleteSelected()
        {
            List<DataRow> drafts = GetCheckedHeaderRows("Draft");
            if (drafts.Count == 0)
            {
                MessageBox.Show(this, "Select one or more Draft vouchers.", "Delete Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this, string.Format("Delete {0} draft vouchers?", drafts.Count), "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            List<int> ids = drafts.Select(r => Convert.ToInt32(r["id"])).ToList();
            _bll.DeleteDraftVouchers(ids);
            LoadData();
        }

        private void ReverseSelected()
        {
            List<DataRow> posted = GetCheckedHeaderRows("Posted");
            if (posted.Count == 0)
            {
                MessageBox.Show(this, "Select one or more Posted vouchers.", "Reverse Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRow sample = posted[0];
            DataTable lines = _bll.GetVoucherLines(Convert.ToString(sample["VoucherNo"]));
            using (var dlg = new frm_journal_reversal(sample, lines))
            {
                dlg.Text = posted.Count > 1 ? string.Format("Create Reversal Entry ({0} vouchers selected)", posted.Count) : "Create Reversal Entry";
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                if (MessageBox.Show(this, string.Format("Create reversal entries for {0} vouchers?", posted.Count), "Confirm Reversal", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                foreach (DataRow row in posted)
                {
                    PostResult result = _bll.ReverseJournalVoucher(Convert.ToInt32(row["id"]), dlg.ReversalDate, dlg.Reason, UsersModal.logged_in_userid);
                    if (!result.Success && result.Messages.Count > 0)
                    {
                        MessageBox.Show(this, string.Join("\r\n", result.Messages.Select(x => x.Message)), "Reversal Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            LoadData();
        }

        private void PrintSelectedPreview()
        {
            DataGridViewRow row = GetSingleFocusedRow();
            if (row == null)
            {
                return;
            }

            string invoiceNo = Convert.ToString(((DataRow)row.Tag)["VoucherNo"]);
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "Journal Voucher";
            printer.SubTitle = string.Format("Voucher No: {0}", invoiceNo);
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "kasbook app";
            printer.FooterSpacing = 15;
            printer.PrintPreviewDataGridView(_previewGrid);
        }

        private void ExportToExcel()
        {
            ExcelExportHelper.ExportDataTableToExcel(_voucherTable, "Journal_Vouchers", this);
        }

        private static string Truncate(string text, int length)
        {
            if (string.IsNullOrWhiteSpace(text) || text.Length <= length)
            {
                return text;
            }

            return text.Substring(0, length - 1) + "…";
        }
    }
}
