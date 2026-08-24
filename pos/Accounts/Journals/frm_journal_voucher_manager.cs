using DGVPrinterHelper;
using pos.Reports.Common;
using pos.UI;
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
            // Set RTL mode based on user language
            bool isArabic = string.Equals(UsersModal.logged_in_lang, "ar-SA", StringComparison.OrdinalIgnoreCase);
            this.RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No;
            this.RightToLeftLayout = isArabic;

            BuildUi();
        }

        /// <summary>
        /// Translates text based on current user language (Arabic/English).
        /// </summary>
        private string T(string englishText, string arabicText)
        {
            return string.Equals(UsersModal.logged_in_lang, "ar-SA", StringComparison.OrdinalIgnoreCase) 
                ? arabicText 
                : englishText;
        }

        private void BuildUi()
        {
            Text = T("Journal Voucher List & Posting Manager", "قائمة كشوفات اليومية وإدارة الترحيل");
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 8.75F, FontStyle.Regular, GraphicsUnit.Point);
            bool isArabic = string.Equals(UsersModal.logged_in_lang, "ar-SA", StringComparison.OrdinalIgnoreCase);

            var filters = new Panel { Dock = DockStyle.Top, Height = 54, BackColor = Color.FromArgb(245, 247, 250), Padding = new Padding(10, 8, 10, 6) };
            var filterFlow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, AutoScroll = true };
            filters.Controls.Add(filterFlow);

            filterFlow.Controls.Add(MakeLabel(T("Date From", "من التاريخ")));
            _dtpFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddMonths(-1), Width = 102 };
            filterFlow.Controls.Add(_dtpFrom);
            filterFlow.Controls.Add(MakeLabel(T("Date To", "إلى التاريخ")));
            _dtpTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today, Width = 102 };
            filterFlow.Controls.Add(_dtpTo);
            filterFlow.Controls.Add(MakeLabel(T("Voucher Type", "نوع الكشف")));
            _cmbVoucherType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150 };
            _cmbVoucherType.Items.Add(new ComboBoxItem("All", T("All", "الكل")));
            _cmbVoucherType.Items.Add(new ComboBoxItem("General Journal", T("General Journal", "قيد عام")));
            _cmbVoucherType.Items.Add(new ComboBoxItem("Opening Entry", T("Opening Entry", "قيد افتتاحي")));
            _cmbVoucherType.Items.Add(new ComboBoxItem("Adjusting Entry", T("Adjusting Entry", "قيد تسوية")));
            _cmbVoucherType.Items.Add(new ComboBoxItem("Closing Entry", T("Closing Entry", "قيد إقفال")));
            _cmbVoucherType.Items.Add(new ComboBoxItem("Reversal Entry", T("Reversal Entry", "قيد معاكس")));
            _cmbVoucherType.SelectedIndex = 0;
            filterFlow.Controls.Add(_cmbVoucherType);
            filterFlow.Controls.Add(MakeLabel(T("Status", "الحالة")));
            _cmbStatus = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 105 };
            _cmbStatus.Items.Add(new ComboBoxItem("All", T("All", "الكل")));
            _cmbStatus.Items.Add(new ComboBoxItem("Draft", T("Draft", "مسودة")));
            _cmbStatus.Items.Add(new ComboBoxItem("Posted", T("Posted", "مرحل")));
            _cmbStatus.Items.Add(new ComboBoxItem("Reversed", T("Reversed", "معاكس")));
            _cmbStatus.SelectedIndex = 0;
            filterFlow.Controls.Add(_cmbStatus);
            filterFlow.Controls.Add(MakeLabel(T("Search", "بحث")));
            _txtSearch = new TextBox { Width = 230 };
            filterFlow.Controls.Add(_txtSearch);
            _btnSearch = MakeButton(T("Search", "بحث"), Color.FromArgb(21, 101, 192));
            _btnClear = MakeButton(T("Clear", "مسح"), Color.FromArgb(96, 125, 139));
            _btnRefresh = MakeButton(T("Refresh", "تحديث"), Color.FromArgb(46, 125, 50));
            filterFlow.Controls.Add(_btnSearch);
            filterFlow.Controls.Add(_btnClear);
            filterFlow.Controls.Add(_btnRefresh);

            var batch = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White, Padding = new Padding(10, 5, 10, 5) };

            // Use the same RTL detection for batch panel
            var batchFlow = new FlowLayoutPanel 
            { 
                Dock = DockStyle.Fill, 
                FlowDirection = isArabic ? FlowDirection.RightToLeft : FlowDirection.LeftToRight, 
                WrapContents = true,
                AutoScroll = true,
                RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No
            };
            batch.Controls.Add(batchFlow);
            _btnPostSelected = MakeButton(T("Post Selected", "ترحيل المختار"), Color.FromArgb(46, 125, 50));
            _btnReverseSelected = MakeButton(T("Reverse Selected", "معاكسة المختار"), Color.FromArgb(192, 57, 43));
            _btnDeleteSelected = MakeButton(T("Delete Selected", "حذف المختار"), Color.FromArgb(231, 76, 60));
            _btnExport = MakeButton(T("Export to Excel", "تصدير إلى إكسل"), Color.FromArgb(84, 110, 122));
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
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "VoucherNo", HeaderText = T("Voucher No", "رقم الكشف"), Width = 130, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "VoucherDate", HeaderText = T("Date", "التاريخ"), Width = 90, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "VoucherType", HeaderText = T("Type", "النوع"), Width = 140, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Narration", HeaderText = T("Narration", "الوصف"), Width = 230, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "LinesCount", HeaderText = T("Lines", "البنود"), Width = 60, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalDebit", HeaderText = T("Total Debit", "إجمالي المدين"), Width = 110, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalCredit", HeaderText = T("Total Credit", "إجمالي الدائن"), Width = 110, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = T("Status", "الحالة"), Width = 90, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "CreatedBy", HeaderText = T("Created By", "أنشأه"), Width = 130, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "PostedBy", HeaderText = T("Posted By", "رحله"), Width = 130, ReadOnly = true });
            _grid.Columns.Add(new DataGridViewButtonColumn { Name = "Actions", HeaderText = T("Actions", "إجراءات"), Width = 85, Text = T("Open", "فتح"), UseColumnTextForButtonValue = true });

            var previewPanel = new Panel { Dock = DockStyle.Bottom, Height = 230, BackColor = Color.FromArgb(250, 251, 253), Padding = new Padding(10) };
            var previewLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            previewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            previewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230F));

            var previewLeft = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 0, 10, 0) };
            _lblPreviewTitle = new Label { Dock = DockStyle.Top, Height = 20, Text = T("Detail Preview", "معاينة التفاصيل"), Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
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
            _previewGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "AccountCode", HeaderText = T("Account Code", "كود الحساب"), ReadOnly = true, Width = 90 });
            _previewGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "AccountName", HeaderText = T("Account Name", "اسم الحساب"), ReadOnly = true, Width = 150 });
            _previewGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Description", HeaderText = T("Description", "الوصف"), ReadOnly = true });
            _previewGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Debit", HeaderText = T("Debit", "مدين"), ReadOnly = true, Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            _previewGrid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Credit", HeaderText = T("Credit", "دائن"), ReadOnly = true, Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N2" } });
            previewLeft.Controls.Add(_previewGrid);
            previewLeft.Controls.Add(_lblPreviewTitle);

            var previewRight = new Panel { Dock = DockStyle.Fill, BackColor = Color.WhiteSmoke, Padding = new Padding(12) };
            _lblPreviewDr = new Label { AutoSize = true, Location = new Point(12, 18), Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.MidnightBlue, Text = T("Debit: 0.00", "مدين: 0.00") };
            _lblPreviewCr = new Label { AutoSize = true, Location = new Point(12, 48), Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.MidnightBlue, Text = T("Credit: 0.00", "دائن: 0.00") };
            _lblPreviewBalance = new Label { AutoSize = true, Location = new Point(12, 78), Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.DarkGreen, Text = T("Balanced ✓", "متوازن ✓") };
            _btnEdit = MakeButton(T("Edit", "تعديل"), Color.FromArgb(21, 101, 192));
            _btnPost = MakeButton(T("Post", "ترحيل"), Color.FromArgb(46, 125, 50));
            _btnPrint = MakeButton(T("Print", "طباعة"), Color.FromArgb(84, 110, 122));
            _btnReverse = MakeButton(T("Reverse", "معاكسة"), Color.FromArgb(192, 57, 43));
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
            _lblViewCount = new ToolStripStatusLabel(T("Total vouchers in view: 0", "إجمالي الكشوفات المعروضة: 0"));
            _lblPostedCount = new ToolStripStatusLabel(T("Posted: 0", "المرحل: 0"));
            _lblDraftCount = new ToolStripStatusLabel(T("Draft: 0", "المسودة: 0"));
            _lblDebitSum = new ToolStripStatusLabel(T("Filtered debit sum: 0.00", "مجموع المدين المصفى: 0.00"));
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
                // Extract English values from ComboBoxItem for BLL queries
                string voucherType = "All";
                string status = "All";

                if (_cmbVoucherType.SelectedItem is ComboBoxItem voucherItem)
                    voucherType = voucherItem.Value;

                if (_cmbStatus.SelectedItem is ComboBoxItem statusItem)
                    status = statusItem.Value;

                string search = _txtSearch.Text.Trim();
                _voucherTable = _bll.GetVoucherHeaders(_dtpFrom.Value, _dtpTo.Value, voucherType, status, search);
                RenderGrid();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, T("Load Error", "خطأ التحميل"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                _lblViewCount.Text = T("Total vouchers in view: 0", "إجمالي الكشوفات المعروضة: 0");
                _lblPostedCount.Text = T("Posted: 0", "المرحل: 0");
                _lblDraftCount.Text = T("Draft: 0", "المسودة: 0");
                _lblDebitSum.Text = T("Filtered debit sum: 0.00", "مجموع المدين المصفى: 0.00");
                return;
            }

            int total = _voucherTable.Rows.Count;
            int posted = _voucherTable.AsEnumerable().Count(r => string.Equals(Convert.ToString(r["Status"]), "Posted", StringComparison.OrdinalIgnoreCase));
            int draft = _voucherTable.AsEnumerable().Count(r => string.Equals(Convert.ToString(r["Status"]), "Draft", StringComparison.OrdinalIgnoreCase));
            decimal debitSum = _voucherTable.AsEnumerable().Sum(r => r["TotalDebit"] == DBNull.Value ? 0m : Convert.ToDecimal(r["TotalDebit"]));

            _lblViewCount.Text = string.Format(T("Total vouchers in view: {0}", "إجمالي الكشوفات المعروضة: {0}"), total);
            _lblPostedCount.Text = string.Format(T("Posted: {0}", "المرحل: {0}"), posted);
            _lblDraftCount.Text = string.Format(T("Draft: {0}", "المسودة: {0}"), draft);
            _lblDebitSum.Text = string.Format(T("Filtered debit sum: {0:N2}", "مجموع المدين المصفى: {0:N2}"), debitSum);
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
            _lblPreviewTitle.Text = string.Format(T("Detail Preview - {0}", "معاينة التفاصيل - {0}"), invoiceNo);
            _previewTable = _bll.GetVoucherLines(invoiceNo);
            _previewGrid.Rows.Clear();

            decimal dr = 0m;
            decimal cr = 0m;

            // Detect if Arabic mode for loading account names
            bool isArabic = string.Equals(UsersModal.logged_in_lang, "ar-SA", StringComparison.OrdinalIgnoreCase);

            if (_previewTable != null)
            {
                foreach (DataRow line in _previewTable.Rows)
                {
                    decimal debit = line["Debit"] == DBNull.Value ? 0m : Convert.ToDecimal(line["Debit"]);
                    decimal credit = line["Credit"] == DBNull.Value ? 0m : Convert.ToDecimal(line["Credit"]);
                    dr += debit;
                    cr += credit;

                    // Load account name based on language mode
                    string accountName = Convert.ToString(line["AccountName"]);
                    if (isArabic && line.Table.Columns.Contains("AccountName_2"))
                    {
                        string accountName2 = Convert.ToString(line["AccountName_2"]);
                        if (!string.IsNullOrWhiteSpace(accountName2))
                            accountName = accountName2;
                    }

                    _previewGrid.Rows.Add(
                        Convert.ToString(line["AccountCode"]), 
                        accountName, 
                        Convert.ToString(line["Description"]), 
                        debit, 
                        credit
                    );
                }
            }

            _lblPreviewDr.Text = string.Format(T("Debit: {0:N2}", "مدين: {0:N2}"), dr);
            _lblPreviewCr.Text = string.Format(T("Credit: {0:N2}", "دائن: {0:N2}"), cr);
            bool balanced = Math.Abs(dr - cr) < 0.005m;
            _lblPreviewBalance.Text = balanced ? T("Balanced ✓", "متوازن ✓") : T("Not Balanced", "غير متوازن");
            _lblPreviewBalance.ForeColor = balanced ? Color.DarkGreen : Color.DarkRed;

            SetQuickButtons(headerRow);
        }

        private void ClearPreview()
        {
            _lblPreviewTitle.Text = T("Detail Preview", "معاينة التفاصيل");
            _previewGrid.Rows.Clear();
            _lblPreviewDr.Text = T("Debit: 0.00", "مدين: 0.00");
            _lblPreviewCr.Text = T("Credit: 0.00", "دائن: 0.00");
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
                MessageBox.Show(this, T("Select one or more Draft vouchers.", "اختر واحد أو أكثر من الكشوفات المسودة."), T("Post Selected", "ترحيل المختار"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this, string.Format(T("Post {0} vouchers?", "هل تريد ترحيل {0} كشف؟"), drafts.Count), T("Confirm Post", "تأكيد الترحيل"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            List<int> ids = drafts.Select(r => Convert.ToInt32(r["id"])).ToList();
            BatchPostResult result = _bll.BatchPostVouchers(ids, UsersModal.logged_in_userid);
            string message = string.Format(T("Posted: {0}\r\nFailed: {1}", "مرحل: {0}\r\nفشل: {1}"), result.SuccessCount, result.FailureCount);
            if (result.FailedVouchers.Count > 0)
            {
                message += "\r\n\r\n" + string.Join("\r\n", result.FailedVouchers.Select(x => string.Format("{0}: {1}", x.VoucherNo, x.Message)));
            }

            MessageBox.Show(this, message, T("Batch Post", "ترحيل جماعي"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }

        private void DeleteSelected()
        {
            List<DataRow> drafts = GetCheckedHeaderRows("Draft");
            if (drafts.Count == 0)
            {
                MessageBox.Show(this, T("Select one or more Draft vouchers.", "اختر واحد أو أكثر من الكشوفات المسودة."), T("Delete Selected", "حذف المختار"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(this, string.Format(T("Delete {0} draft vouchers?", "هل تريد حذف {0} من الكشوفات المسودة؟"), drafts.Count), T("Confirm Delete", "تأكيد الحذف"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
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
                MessageBox.Show(this, T("Select one or more Posted vouchers.", "اختر واحد أو أكثر من الكشوفات المرحلة."), T("Reverse Selected", "معاكسة المختار"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRow sample = posted[0];
            DataTable lines = _bll.GetVoucherLines(Convert.ToString(sample["VoucherNo"]));
            using (var dlg = new frm_journal_reversal(sample, lines))
            {
                dlg.Text = posted.Count > 1 
                    ? string.Format(T("Create Reversal Entry ({0} vouchers selected)", "إنشاء قيد معاكس ({0} كشف مختار)"), posted.Count) 
                    : T("Create Reversal Entry", "إنشاء قيد معاكس");
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                if (MessageBox.Show(this, string.Format(T("Create reversal entries for {0} vouchers?", "هل تريد إنشاء قيود معاكسة لـ {0} كشف؟"), posted.Count), T("Confirm Reversal", "تأكيد المعاكسة"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                foreach (DataRow row in posted)
                {
                    PostResult result = _bll.ReverseJournalVoucher(Convert.ToInt32(row["id"]), dlg.ReversalDate, dlg.Reason, UsersModal.logged_in_userid);
                    if (!result.Success && result.Messages.Count > 0)
                    {
                        MessageBox.Show(this, string.Join("\r\n", result.Messages.Select(x => x.Message)), T("Reversal Failed", "فشلت المعاكسة"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frm_journal_voucher_manager
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "frm_journal_voucher_manager";
            this.Load += new System.EventHandler(this.frm_journal_voucher_manager_Load);
            this.ResumeLayout(false);

        }

        private void frm_journal_voucher_manager_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            //LoadData();
        }
    }

    /// <summary>
    /// Helper class to store both display text and underlying value for ComboBox items.
    /// Enables multilingual display while preserving English values for database queries.
    /// </summary>
    internal class ComboBoxItem
    {
        public string Value { get; set; }
        public string Display { get; set; }

        public ComboBoxItem(string value, string display)
        {
            Value = value;
            Display = display;
        }

        public override string ToString()
        {
            return Display;
        }
    }
}
