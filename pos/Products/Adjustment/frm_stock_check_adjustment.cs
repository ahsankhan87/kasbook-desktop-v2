using POS.BLL;
using POS.Core;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace pos
{
    public partial class frm_stock_check_adjustment : Form
    {
        private sealed class ProductSearchItem
        {
            public int ProductId { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string Barcode { get; set; }
            public string CategoryId { get; set; }
            public string BrandId { get; set; }
            public decimal CurrentQty { get; set; }
            public decimal SalePrice { get; set; }
            public string LocationCode { get; set; }
            public bool IsActive { get; set; }
            public decimal ReorderLevel { get; set; }

            public string ProductCodeLower { get; set; }
            public string ProductNameLower { get; set; }
            public string BarcodeLower { get; set; }
        }

        private sealed class SearchFilters
        {
            public string CategoryId { get; set; }
            public string BrandId { get; set; }
            public string LocationCode { get; set; }
            public bool LowStockOnly { get; set; }
            public bool ZeroStockOnly { get; set; }
            public bool UnverifiedOnly { get; set; }
            public HashSet<string> UnverifiedCodes { get; set; }
        }

        private sealed class ProductSearchScoredResult
        {
            public ProductSearchItem Item { get; set; }
            public int Score { get; set; }
        }

        private readonly object _productIndexLock = new object();
        private List<ProductSearchItem> _productIndex = new List<ProductSearchItem>(0);
        private volatile bool _isIndexLoading;
        private readonly System.Windows.Forms.Timer _indexRefreshTimer = new System.Windows.Forms.Timer();
        private readonly Stopwatch _typingStopwatch = new Stopwatch();
        private long _lastKeyTicks;
        private int _scannerBurstCount;
        private Label _lblIndexStatus;

        private readonly Color _draftColor = Color.FromArgb(255, 193, 7);
        private readonly Color _inProgressColor = Color.FromArgb(21, 101, 192);
        private readonly Color _postedColor = Color.FromArgb(46, 125, 50);

        private enum SearchMode
        {
            NameCode,
            Barcode,
            CategoryBrowse
        }

        private sealed class SearchProductRow
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public decimal Qty { get; set; }
            public decimal Price { get; set; }
            public string Location { get; set; }
            public decimal ReorderLevel { get; set; }
        }

        private sealed class AdjustmentGridRow
        {
            public bool Verified { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string Category { get; set; }
            public decimal CurrentQty { get; set; }
            public decimal NewQty { get; set; }
            public decimal CurrentPrice { get; set; }
            public decimal NewPrice { get; set; }
            public string CurrentLocation { get; set; }
            public string NewLocation { get; set; }
            public string Reason { get; set; }
            public string Notes { get; set; }
            public bool IsPosted { get; set; }

            public decimal QtyDiff
            {
                get { return NewQty - CurrentQty; }
            }

            public decimal PriceDiff
            {
                get { return NewPrice - CurrentPrice; }
            }

            public bool IsModified
            {
                get
                {
                    return NewQty != CurrentQty
                        || NewPrice != CurrentPrice
                        || !string.Equals(CurrentLocation ?? string.Empty, NewLocation ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                        || !string.IsNullOrWhiteSpace(Notes)
                        || !string.IsNullOrWhiteSpace(Reason);
                }
            }
        }

        private SearchMode _searchMode = SearchMode.NameCode;
        private readonly System.Windows.Forms.Timer _searchDebounceTimer = new System.Windows.Forms.Timer();
        private readonly List<SearchProductRow> _currentResultPage = new List<SearchProductRow>();
        private readonly List<SearchProductRow> _recentAdded = new List<SearchProductRow>();
        private readonly HashSet<string> _pinnedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private readonly List<AdjustmentGridRow> _sessionRows = new List<AdjustmentGridRow>();

        private Panel _leftRoot;
        private TextBox _txtSearch;
        private Button _btnSearchMode;
        private Button _btnFilterToggle;
        private Panel _advancedFilterPanel;
        private ComboBox _cmbCategory;
        private ComboBox _cmbBrand;
        private ComboBox _cmbSupplier;
        private ComboBox _cmbAisle;
        private ComboBox _cmbShelf;
        private ComboBox _cmbBin;
        private CheckBox _chkLowStockOnly;
        private CheckBox _chkZeroStock;
        private CheckBox _chkUnverifiedOnly;
        private Button _btnApplyFilter;
        private Button _btnClearFilter;
        private Label _lblResultInfo;
        private DataGridView _gridSearchResults;
        private DataGridView _gridRecent;
        private ContextMenuStrip _resultContextMenu;

        private Panel _rightRoot;
        private Panel _gridToolbarPanel;
        private Label _lblSessionCount;
        private Label _lblModifiedBadge;
        private Label _lblPostedBadge;
        private Button _btnSelectAll;
        private Button _btnClearSelection;
        private Button _btnRemoveSelected;
        private Button _btnBulkEdit;
        private Button _btnColumnToggle;
        private ContextMenuStrip _bulkEditMenu;
        private ContextMenuStrip _columnMenu;
        private DataGridView _gridAdjustment;
        private Panel _gridFooterPanel;
        private Label _lblFooterSummary;

        private Panel _drawerPanel;
        private Button _btnDrawerClose;
        private PictureBox _drawerImage;
        private Label _drawerHeader;
        private Label _drawerCode;
        private Label _drawerCategory;
        private Label _drawerStockInfo;
        private Label _drawerPricingInfo;
        private Label _drawerLocationInfo;
        private Label _drawerReorderInfo;
        private Label _drawerTransactions;
        private Button _btnQuickAdjust;
        private Chart _drawerStockChart;

        private Panel _scanOverlay;
        private Label _scanTitle;
        private Label _scanReady;
        private Label _scanProduct;
        private Label _scanCurrentQty;
        private NumericUpDown _scanQty;
        private Button _btnScanConfirm;
        private Button _btnScanNote;
        private Button _btnScanExit;
        private Label _scanRunningCount;
        private Label _scanNotFound;
        private TextBox _scanInputHidden;
        private ListBox _scanLog;
        private bool _isScanMode;
        private int _scannedCount;
        private readonly Queue<string> _scanLogItems = new Queue<string>();

        private const string ColRowNo = "colRowNo";
        private const string ColVerified = "colVerified";
        private const string ColCode = "colCode";
        private const string ColName = "colName";
        private const string ColCategory = "colCategory";
        private const string ColCurrentQty = "colCurrentQty";
        private const string ColNewQty = "colNewQty";
        private const string ColQtyDiff = "colQtyDiff";
        private const string ColCurrentPrice = "colCurrentPrice";
        private const string ColNewPrice = "colNewPrice";
        private const string ColPriceDiff = "colPriceDiff";
        private const string ColCurrentLoc = "colCurrentLoc";
        private const string ColNewLoc = "colNewLoc";
        private const string ColReason = "colReason";
        private const string ColNotes = "colNotes";
        private const string ColActions = "colActions";

        public frm_stock_check_adjustment()
        {
            InitializeComponent();
            InitializeSessionUi();
            BuildLeftPanelUi();
            InitializeLeftPanelEvents();
            BuildRightPanelUi();
            ConfigureVirtualGrid();        // ? virtual-mode setup
            InitializeRightPanelEvents();
            BuildProductDetailDrawer();
            BuildScanModeOverlay();
            InitializeProductIndexRefreshTimer();
        }

        private void frm_stock_check_adjustment_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.KeyDown += Frm_stock_check_adjustment_KeyDown;

            WireUndoButton();              // undo toolbar button
            InitializeShortcutsAndWorkflow();  // shortcuts + auto-save + dirty tracking

            LoadNewSession();
            SetSessionStatus("Draft");
            UpdateVerificationProgress(0, 0);
            UpdateBottomSummary(0, 0, 0, 0m, "--:--", UsersModal.logged_in_username, UsersModal.logged_in_branch_name);
            UpdateResultInfo(0, 0);
            RefreshRecentGrid();
            RefreshAdjustmentGrid();

            BeginBuildProductIndexAsync();
        }

        private void Frm_stock_check_adjustment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6)
            {
                e.Handled = true;
                ToggleScanMode(true);
                return;
            }

            if (e.KeyCode == Keys.F4)
            {
                e.Handled = true;
                if (_gridAdjustment.CurrentCell != null)
                    ShowProductDrawerForRow(_gridAdjustment.CurrentCell.RowIndex);
            }

            if (e.KeyCode == Keys.Z && e.Control)
            {
                e.Handled = true;
                UndoLastEdit();
        }
        }

        private void InitializeSessionUi()
        {
            cmbAdjustmentType.Items.Clear();
            cmbAdjustmentType.Items.AddRange(new object[]
            {
                "Physical Count",
                "Damage Write-Off",
                "Found/Excess",
                "Price Update",
                "Location Transfer",
                "Opening Stock"
            });

            if (cmbAdjustmentType.Items.Count > 0)
                cmbAdjustmentType.SelectedIndex = 0;

            dtpAdjustmentDate.Value = DateTime.Today;
            dtpAdjustmentDate.Format = DateTimePickerFormat.Short;
        }

        private void BuildLeftPanelUi()
        {
            leftPanelHost.Controls.Clear();

            _leftRoot = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(8)
            };

            var searchRow = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 36,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.White
            };
            searchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            searchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
            searchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 34F));

            _txtSearch = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.DimGray,
                Text = "Search by name, code, or scan barcode..."
            };

            _btnSearchMode = new Button
            {
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Text = "Name/Code",
                BackColor = Color.White
            };
            _btnSearchMode.FlatAppearance.BorderColor = Color.Silver;

            _btnFilterToggle = new Button
            {
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Text = "?",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.White
            };
            _btnFilterToggle.FlatAppearance.BorderColor = Color.Silver;

            searchRow.Controls.Add(_txtSearch, 0, 0);
            searchRow.Controls.Add(_btnSearchMode, 1, 0);
            searchRow.Controls.Add(_btnFilterToggle, 2, 0);

            _advancedFilterPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 126,
                Visible = false,
                BackColor = Color.FromArgb(248, 250, 252),
                Padding = new Padding(6, 4, 6, 4)
            };

            var filterLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 4
            };
            filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            filterLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));

            _cmbCategory = MakeFilterCombo();
            _cmbBrand = MakeFilterCombo();
            _cmbSupplier = MakeFilterCombo();
            _cmbAisle = MakeFilterCombo();
            _cmbShelf = MakeFilterCombo();
            _cmbBin = MakeFilterCombo();

            _cmbCategory.Items.AddRange(new object[] { "All Categories", "Electronics > Mobile", "Electronics > Mobile > Accessories", "Grocery > Beverages" });
            _cmbBrand.Items.AddRange(new object[] { "All Brands", "Brand A", "Brand B", "Brand C" });
            _cmbSupplier.Items.AddRange(new object[] { "All Suppliers", "Supplier 1", "Supplier 2" });
            _cmbAisle.Items.AddRange(new object[] { "Aisle", "A1", "A2", "A3" });
            _cmbShelf.Items.AddRange(new object[] { "Shelf", "S1", "S2", "S3" });
            _cmbBin.Items.AddRange(new object[] { "Bin", "B1", "B2", "B3", "B4" });
            _cmbCategory.SelectedIndex = 0;
            _cmbBrand.SelectedIndex = 0;
            _cmbSupplier.SelectedIndex = 0;
            _cmbAisle.SelectedIndex = 0;
            _cmbShelf.SelectedIndex = 0;
            _cmbBin.SelectedIndex = 0;

            _chkLowStockOnly = new CheckBox { Text = "Low Stock Only", Dock = DockStyle.Fill, AutoSize = true };
            _chkZeroStock = new CheckBox { Text = "Zero Stock", Dock = DockStyle.Fill, AutoSize = true };
            _chkUnverifiedOnly = new CheckBox { Text = "Unverified Only", Dock = DockStyle.Fill, AutoSize = true };

            var actionPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
            _btnApplyFilter = new Button { Text = "Apply Filter", FlatStyle = FlatStyle.Flat, Width = 90, Height = 24 };
            _btnClearFilter = new Button { Text = "Clear", FlatStyle = FlatStyle.Flat, Width = 62, Height = 24 };
            _btnApplyFilter.FlatAppearance.BorderColor = Color.Silver;
            _btnClearFilter.FlatAppearance.BorderColor = Color.Silver;
            actionPanel.Controls.Add(_btnApplyFilter);
            actionPanel.Controls.Add(_btnClearFilter);

            filterLayout.Controls.Add(_cmbCategory, 0, 0);
            filterLayout.Controls.Add(_cmbBrand, 1, 0);
            filterLayout.Controls.Add(_cmbSupplier, 2, 0);
            filterLayout.Controls.Add(_cmbAisle, 0, 1);
            filterLayout.Controls.Add(_cmbShelf, 1, 1);
            filterLayout.Controls.Add(_cmbBin, 2, 1);
            filterLayout.Controls.Add(_chkLowStockOnly, 0, 2);
            filterLayout.Controls.Add(_chkZeroStock, 1, 2);
            filterLayout.Controls.Add(_chkUnverifiedOnly, 2, 2);
            filterLayout.Controls.Add(actionPanel, 0, 3);
            filterLayout.SetColumnSpan(actionPanel, 3);
            _advancedFilterPanel.Controls.Add(filterLayout);

            _lblResultInfo = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 8.5F),
                Text = "Showing 0 of 0 results — type more to narrow down"
            };

            _lblIndexStatus = new Label
            {
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(21, 101, 192),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Text = ""
            };

            _gridSearchResults = new DataGridView
            {
                Dock = DockStyle.Fill,
                VirtualMode = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                ColumnHeadersHeight = 30,
                EnableHeadersVisualStyles = false
            };
            _gridSearchResults.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            _gridSearchResults.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);

            _gridSearchResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCode", HeaderText = "Code", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Consolas", 9F, FontStyle.Bold) } });
            _gridSearchResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "colName", HeaderText = "Product Name", Width = 150 });
            _gridSearchResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "colQty", HeaderText = "Qty", Width = 56, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            _gridSearchResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrice", HeaderText = "Sale Price", Width = 74, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, ForeColor = Color.DimGray } });
            _gridSearchResults.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLoc", HeaderText = "Location", Width = 76 });

            _resultContextMenu = new ContextMenuStrip();
            _resultContextMenu.Items.Add("Add to Adjustment", null, (s, e) => AddSelectedResultToAdjustment());
            _resultContextMenu.Items.Add("View Product Detail", null, (s, e) => ViewSelectedProductDetail());
            _resultContextMenu.Items.Add("View Stock History", null, (s, e) => ViewSelectedStockHistory());
            _gridSearchResults.ContextMenuStrip = _resultContextMenu;

            var recentTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 22,
                Text = "Recently Added",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 101, 192)
            };

            _gridRecent = new DataGridView
            {
                Dock = DockStyle.Bottom,
                Height = 156,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            _gridRecent.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colPin", HeaderText = "??", Width = 36 });
            _gridRecent.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRecentCode", HeaderText = "Code", Width = 90, ReadOnly = true });
            _gridRecent.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRecentName", HeaderText = "Product", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });

            _leftRoot.Controls.Add(_gridSearchResults);
            _leftRoot.Controls.Add(_lblIndexStatus);
            _leftRoot.Controls.Add(_lblResultInfo);
            _leftRoot.Controls.Add(_advancedFilterPanel);
            _leftRoot.Controls.Add(searchRow);
            _leftRoot.Controls.Add(recentTitle);
            _leftRoot.Controls.Add(_gridRecent);

            leftPanelHost.Controls.Add(_leftRoot);
            recentTitle.BringToFront();
            _gridRecent.BringToFront();
        }

        private void BuildRightPanelUi()
        {
            rightPanelHost.Controls.Clear();

            _rightRoot = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(8)
            };

            _gridToolbarPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = Color.White
            };

            var leftBadgePanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                Width = 360,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(2, 7, 0, 0)
            };

            _lblSessionCount = new Label { AutoSize = true, Text = "0 items in session", Font = new Font("Segoe UI", 9F, FontStyle.Bold), Margin = new Padding(0, 0, 12, 0) };
            _lblModifiedBadge = MakeBadge("0 modified", Color.FromArgb(255, 193, 7));
            _lblPostedBadge = MakeBadge("0 posted", Color.FromArgb(46, 125, 50));
            leftBadgePanel.Controls.Add(_lblSessionCount);
            leftBadgePanel.Controls.Add(_lblModifiedBadge);
            leftBadgePanel.Controls.Add(_lblPostedBadge);

            var rightButtonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 500,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                Padding = new Padding(0, 4, 0, 0)
            };

            _btnColumnToggle = MakeToolbarButton("Columns");
            _btnBulkEdit = MakeToolbarButton("Bulk Edit ?");
            _btnRemoveSelected = MakeToolbarButton("Remove Selected");
            _btnClearSelection = MakeToolbarButton("Clear Selection");
            _btnSelectAll = MakeToolbarButton("Select All");

            var _btnReports = MakeToolbarButton("Reports ?");
            _btnReports.BackColor = Color.FromArgb(245, 247, 250);

            var _btnHelp = MakeToolbarButton("? Help  F1");
            _btnHelp.BackColor = Color.FromArgb(232, 240, 254);
            _btnHelp.ForeColor = Color.FromArgb(21, 101, 192);
            _btnHelp.Click += (s, e) => ShowHelpForm();

            rightButtonPanel.Controls.Add(_btnColumnToggle);
            rightButtonPanel.Controls.Add(_btnBulkEdit);
            rightButtonPanel.Controls.Add(_btnRemoveSelected);
            rightButtonPanel.Controls.Add(_btnClearSelection);
            rightButtonPanel.Controls.Add(_btnSelectAll);
            rightButtonPanel.Controls.Add(_btnReports);
            rightButtonPanel.Controls.Add(_btnHelp);

            _gridToolbarPanel.Controls.Add(leftBadgePanel);
            _gridToolbarPanel.Controls.Add(rightButtonPanel);

            _bulkEditMenu = new ContextMenuStrip();
            _bulkEditMenu.Items.Add("Set Location", null, (s, e) => BulkSetLocation());
            _bulkEditMenu.Items.Add("Apply Price Change %", null, (s, e) => BulkApplyPricePercent());
            _bulkEditMenu.Items.Add("Mark as Verified", null, (s, e) => BulkMarkVerified());

            _gridAdjustment = new DataGridView
            {
                Dock = DockStyle.Fill,
                VirtualMode = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = true,
                RowHeadersWidth = 32,
                EnableHeadersVisualStyles = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                EditMode = DataGridViewEditMode.EditOnEnter
            };

            _gridAdjustment.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            _gridAdjustment.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            _gridAdjustment.ColumnHeadersHeight = 30;

            _gridAdjustment.Columns.Add(new DataGridViewTextBoxColumn { Name = ColRowNo, HeaderText = "#", Width = 30, ReadOnly = true });
            _gridAdjustment.Columns.Add(new DataGridViewCheckBoxColumn { Name = ColVerified, HeaderText = "?", Width = 30 });
            _gridAdjustment.Columns.Add(new DataGridViewTextBoxColumn { Name = ColCode, HeaderText = "Product Code", Width = 90, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Consolas", 9F, FontStyle.Bold) } });
            _gridAdjustment.Columns.Add(new DataGridViewTextBoxColumn { Name = ColName, HeaderText = "Product Name", Width = 180, ReadOnly = true });
            _gridAdjustment.Columns.Add(new DataGridViewTextBoxColumn { Name = ColCategory, HeaderText = "Category", Width = 100, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.DimGray } });
            _gridAdjustment.Columns.Add(new DataGridViewTextBoxColumn { Name = ColCurrentQty, HeaderText = "Current Qty", Width = 80, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.FromArgb(21, 101, 192), Alignment = DataGridViewContentAlignment.MiddleRight } });
            _gridAdjustment.Columns.Add(new DataGridViewTextBoxColumn { Name = ColNewQty, HeaderText = "Physical/New Qty", Width = 80, DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(255, 253, 208), Alignment = DataGridViewContentAlignment.MiddleRight } });
            _gridAdjustment.Columns.Add(new DataGridViewTextBoxColumn { Name = ColQtyDiff, HeaderText = "Difference", Width = 70, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            _gridAdjustment.Columns.Add(new DataGridViewTextBoxColumn { Name = ColCurrentPrice, HeaderText = "Current Sale Price", Width = 90, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            _gridAdjustment.Columns.Add(new DataGridViewTextBoxColumn { Name = ColNewPrice, HeaderText = "New Sale Price", Width = 90, DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(255, 253, 208), Alignment = DataGridViewContentAlignment.MiddleRight } });
            _gridAdjustment.Columns.Add(new DataGridViewTextBoxColumn { Name = ColPriceDiff, HeaderText = "Price Diff", Width = 70, ReadOnly = true, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
            _gridAdjustment.Columns.Add(new DataGridViewTextBoxColumn { Name = ColCurrentLoc, HeaderText = "Current Location", Width = 90, ReadOnly = true });
            _gridAdjustment.Columns.Add(new DataGridViewComboBoxColumn { Name = ColNewLoc, HeaderText = "New Location", Width = 90, FlatStyle = FlatStyle.Flat, DataSource = new[] { "A1-S1-B1", "A2-S1-B2", "A3-S2-B4", "A4-S3-B1" } });
            _gridAdjustment.Columns.Add(new DataGridViewComboBoxColumn { Name = ColReason, HeaderText = "Reason", Width = 100, FlatStyle = FlatStyle.Flat, DataSource = new[] { "Physical Count", "Damage", "Found", "Price Correction", "Relocation", "Other" } });
            _gridAdjustment.Columns.Add(new DataGridViewTextBoxColumn { Name = ColNotes, HeaderText = "Notes", Width = 120 });
            _gridAdjustment.Columns.Add(new DataGridViewButtonColumn { Name = ColActions, HeaderText = "Actions", Width = 50, Text = "?", UseColumnTextForButtonValue = true });

            _columnMenu = new ContextMenuStrip();
            foreach (DataGridViewColumn col in _gridAdjustment.Columns)
            {
                if (col.Name == ColRowNo || col.Name == ColVerified || col.Name == ColCode || col.Name == ColName || col.Name == ColActions)
                    continue;

                var item = new ToolStripMenuItem(col.HeaderText) { Checked = true, CheckOnClick = true, Tag = col.Name };
                item.CheckedChanged += ColumnMenuItem_CheckedChanged;
                _columnMenu.Items.Add(item);
            }

            _gridFooterPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(245, 247, 250),
                Padding = new Padding(8, 6, 8, 4)
            };

            _lblFooterSummary = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Text = "Total Items: 0 | Total Qty Increase: +0 | Total Qty Decrease: -0 | Net Stock Value Change: ±SAR 0.00"
            };
            _gridFooterPanel.Controls.Add(_lblFooterSummary);

            _rightRoot.Controls.Add(_gridAdjustment);
            _rightRoot.Controls.Add(_gridFooterPanel);
            _rightRoot.Controls.Add(_gridToolbarPanel);

            rightPanelHost.Controls.Add(_rightRoot);
        }

        private Label MakeBadge(string text, Color backColor)
        {
            return new Label
            {
                AutoSize = true,
                Text = text,
                ForeColor = Color.White,
                BackColor = backColor,
                Padding = new Padding(8, 3, 8, 3),
                Margin = new Padding(0, 0, 8, 0)
            };
        }

        private Button MakeToolbarButton(string text)
        {
            var btn = new Button
            {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                Height = 24,
                Width = text.Contains("Remove") ? 112 : text.Contains("Selection") ? 96 : 84,
                Margin = new Padding(4, 0, 0, 0),
                BackColor = Color.White
            };
            btn.FlatAppearance.BorderColor = Color.Silver;
            return btn;
        }

        private ComboBox MakeFilterCombo()
        {
            return new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 8.5F)
            };
        }

        private void InitializeLeftPanelEvents()
        {
            _searchDebounceTimer.Interval = 250;
            _searchDebounceTimer.Tick += SearchDebounceTimer_Tick;

            _txtSearch.Enter += TxtSearch_Enter;
            _txtSearch.Leave += TxtSearch_Leave;
            _txtSearch.TextChanged += TxtSearch_TextChanged;
            _txtSearch.KeyDown += TxtSearch_KeyDown;
            _txtSearch.KeyPress += TxtSearch_KeyPress;

            _btnSearchMode.Click += BtnSearchMode_Click;
            _btnFilterToggle.Click += BtnFilterToggle_Click;
            _btnApplyFilter.Click += BtnApplyFilter_Click;
            _btnClearFilter.Click += BtnClearFilter_Click;

            _gridSearchResults.CellValueNeeded += GridSearchResults_CellValueNeeded;
            _gridSearchResults.CellFormatting += GridSearchResults_CellFormatting;
            _gridSearchResults.CellMouseDoubleClick += GridSearchResults_CellMouseDoubleClick;
            _gridSearchResults.KeyDown += GridSearchResults_KeyDown;
            _gridSearchResults.CellMouseDown += GridSearchResults_CellMouseDown;

            _gridRecent.CellValueChanged += GridRecent_CellValueChanged;
            _gridRecent.CurrentCellDirtyStateChanged += GridRecent_CurrentCellDirtyStateChanged;
        }

        private void InitializeRightPanelEvents()
        {
            _btnSelectAll.Click += (s, e) =>
            {
                foreach (DataGridViewRow row in _gridAdjustment.Rows)
                    row.Selected = true;
            };

            _btnClearSelection.Click += (s, e) => _gridAdjustment.ClearSelection();

            _btnRemoveSelected.Click += (s, e) =>
            {
                var selectedIndexes = _gridAdjustment.SelectedRows.Cast<DataGridViewRow>().Select(r => r.Index).Where(i => i >= 0).OrderByDescending(i => i).ToList();
                foreach (var idx in selectedIndexes)
                    _sessionRows.RemoveAt(idx);
                RefreshAdjustmentGrid();
            };

            _btnBulkEdit.Click += (s, e) => _bulkEditMenu.Show(_btnBulkEdit, new Point(0, _btnBulkEdit.Height));
            _btnColumnToggle.Click += (s, e) => _columnMenu.Show(_btnColumnToggle, new Point(0, _btnColumnToggle.Height));
            btnScanBarcode.Click += (s, e) => ToggleScanMode(true);

            _gridAdjustment.CellValueNeeded += GridAdjustment_CellValueNeeded;
            _gridAdjustment.CellValuePushed += GridAdjustment_CellValuePushed_VirtualGrid;
            _gridAdjustment.CellFormatting += GridAdjustment_CellFormatting;
            _gridAdjustment.CellContentClick += GridAdjustment_CellContentClick;
            _gridAdjustment.CellMouseEnter += GridAdjustment_CellMouseEnter;
            _gridAdjustment.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    ShowProductDrawerForRow(e.RowIndex);
            };
            _gridAdjustment.RowPrePaint += GridAdjustment_RowPrePaint;
            _gridAdjustment.KeyDown += GridAdjustment_KeyDown;
            _gridAdjustment.DataError += (s, e) => { e.ThrowException = false; };
        }

        private void InitializeProductIndexRefreshTimer()
        {
            _indexRefreshTimer.Interval = 30 * 60 * 1000;
            _indexRefreshTimer.Tick += async (s, e) =>
            {
                if (_isIndexLoading)
                    return;

                await RefreshProductIndex();
            };
            _indexRefreshTimer.Start();
        }

        private async void BeginBuildProductIndexAsync()
        {
            if (_isIndexLoading)
                return;

            await RefreshProductIndex();
        }

        public async Task RefreshProductIndex()
        {
            if (_isIndexLoading)
                return;

            _isIndexLoading = true;
            SetIndexStatus("Refreshing product index...", true);

            try
            {
                var loaded = await Task.Run(() => LoadProductIndexFromDataSource());

                lock (_productIndexLock)
                {
                    _productIndex = loaded;
                }

                SetIndexStatus(string.Format("Product index ready ({0:N0} items)", loaded.Count), false);
            }
            catch (Exception ex)
            {
                SetIndexStatus("Product index failed: " + ex.Message, false);
            }
            finally
            {
                _isIndexLoading = false;
            }
        }

        private List<ProductSearchItem> LoadProductIndexFromDataSource()
        {
            var output = new List<ProductSearchItem>(50000);
            var productBll = new ProductBLL();
            DataTable dt = productBll.GetAllProductCodes();

            int total = dt == null ? 0 : dt.Rows.Count;
            int processed = 0;

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var code = Convert.ToString(dr.Table.Columns.Contains("code") ? dr["code"] : string.Empty) ?? string.Empty;
                    var name = Convert.ToString(dr.Table.Columns.Contains("name") ? dr["name"] : string.Empty) ?? string.Empty;

                    var item = new ProductSearchItem
                    {
                        ProductId = ParseInt(dr, "id"),
                        ProductCode = code,
                        ProductName = name,
                        Barcode = Convert.ToString(GetColumnValueOrDefault(dr, "barcode", code)) ?? string.Empty,
                        CategoryId = Convert.ToString(GetColumnValueOrDefault(dr, "category_id", string.Empty)) ?? string.Empty,
                        BrandId = Convert.ToString(GetColumnValueOrDefault(dr, "brand_id", string.Empty)) ?? string.Empty,
                        CurrentQty = ParseDecimal(dr, "qty"),
                        SalePrice = ParseDecimal(dr, "unit_price"),
                        LocationCode = Convert.ToString(GetColumnValueOrDefault(dr, "location_code", string.Empty)) ?? string.Empty,
                        IsActive = ParseBool(dr, "is_active", true),
                        ReorderLevel = ParseDecimal(dr, "reorder_level")
                    };

                    item.ProductCodeLower = (item.ProductCode ?? string.Empty).ToLowerInvariant();
                    item.ProductNameLower = (item.ProductName ?? string.Empty).ToLowerInvariant();
                    item.BarcodeLower = (item.Barcode ?? string.Empty).ToLowerInvariant();

                    output.Add(item);
                    processed++;

                    if (processed % 500 == 0 || processed == total)
                    {
                        int pct = total <= 0 ? 100 : (int)((processed * 100L) / total);
                        SetIndexStatus(string.Format("Loading product index... ({0}%)", pct), true);
                    }
                }
            }

            return output;
        }

        private static object GetColumnValueOrDefault(DataRow row, string columnName, object defaultValue)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return defaultValue;

            return row[columnName];
        }

        private static int ParseInt(DataRow row, string columnName)
        {
            int v;
            return int.TryParse(Convert.ToString(GetColumnValueOrDefault(row, columnName, 0)), out v) ? v : 0;
        }

        private static decimal ParseDecimal(DataRow row, string columnName)
        {
            decimal v;
            return decimal.TryParse(Convert.ToString(GetColumnValueOrDefault(row, columnName, 0m)), out v) ? v : 0m;
        }

        private static bool ParseBool(DataRow row, string columnName, bool fallback)
        {
            bool v;
            return bool.TryParse(Convert.ToString(GetColumnValueOrDefault(row, columnName, fallback)), out v) ? v : fallback;
        }

        private void SetIndexStatus(string text, bool isBusy)
        {
            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, bool>(SetIndexStatus), text, isBusy);
                return;
            }

            if (_lblIndexStatus != null)
            {
                _lblIndexStatus.Text = text;
                _lblIndexStatus.ForeColor = isBusy ? Color.FromArgb(21, 101, 192) : Color.ForestGreen;
            }

            tslMeta.Text = text;
        }

        private void TxtSearch_Enter(object sender, EventArgs e)
        {
            if (_txtSearch.ForeColor == Color.DimGray)
            {
                _txtSearch.Text = string.Empty;
                _txtSearch.ForeColor = Color.Black;
            }
        }

        private void TxtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtSearch.Text))
            {
                _txtSearch.ForeColor = Color.DimGray;
                _txtSearch.Text = "Search by name, code, or scan barcode...";
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_txtSearch.ForeColor == Color.DimGray)
                return;

            _searchDebounceTimer.Stop();
            _searchDebounceTimer.Start();
        }

        private void TxtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            long now = Stopwatch.GetTimestamp();
            if (!_typingStopwatch.IsRunning)
                _typingStopwatch.Start();

            if (_lastKeyTicks > 0)
            {
                double deltaMs = (now - _lastKeyTicks) * 1000.0 / Stopwatch.Frequency;
                if (deltaMs <= 50)
                    _scannerBurstCount++;
                else
                    _scannerBurstCount = 0;
            }

            _lastKeyTicks = now;
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                _searchDebounceTimer.Stop();
                RunSearch(forceImmediate: true);
                _scannerBurstCount = 0;
            }
        }

        private void SearchDebounceTimer_Tick(object sender, EventArgs e)
        {
            _searchDebounceTimer.Stop();
            RunSearch();
        }

        private void BtnSearchMode_Click(object sender, EventArgs e)
        {
            if (_searchMode == SearchMode.NameCode)
            {
                _searchMode = SearchMode.Barcode;
                _btnSearchMode.Text = "Barcode";
            }
            else if (_searchMode == SearchMode.Barcode)
            {
                _searchMode = SearchMode.CategoryBrowse;
                _btnSearchMode.Text = "Category";
            }
            else
            {
                _searchMode = SearchMode.NameCode;
                _btnSearchMode.Text = "Name/Code";
            }

            RunSearch(forceImmediate: true);
        }

        private void BtnFilterToggle_Click(object sender, EventArgs e)
        {
            _advancedFilterPanel.Visible = !_advancedFilterPanel.Visible;
        }

        private void BtnApplyFilter_Click(object sender, EventArgs e)
        {
            RunSearch(forceImmediate: true);
        }

        private void BtnClearFilter_Click(object sender, EventArgs e)
        {
            _cmbCategory.SelectedIndex = 0;
            _cmbBrand.SelectedIndex = 0;
            _cmbSupplier.SelectedIndex = 0;
            _cmbAisle.SelectedIndex = 0;
            _cmbShelf.SelectedIndex = 0;
            _cmbBin.SelectedIndex = 0;
            _chkLowStockOnly.Checked = false;
            _chkZeroStock.Checked = false;
            _chkUnverifiedOnly.Checked = false;
            RunSearch(forceImmediate: true);
        }

        private void RunSearch(bool forceImmediate = false)
        {
            if (_isIndexLoading)
            {
                UpdateResultInfo(0, 0);
                return;
            }

            string text = (_txtSearch.ForeColor == Color.DimGray ? string.Empty : (_txtSearch.Text ?? string.Empty).Trim());
            if (string.IsNullOrWhiteSpace(text) && _searchMode != SearchMode.CategoryBrowse)
            {
                _currentResultPage.Clear();
                _gridSearchResults.RowCount = 0;
                UpdateResultInfo(0, 0);
                return;
            }

            if (!forceImmediate && _scannerBurstCount >= 4)
            {
                forceImmediate = true;
            }

            var filters = BuildSearchFilters();
            int totalMatched;
            var watch = Stopwatch.StartNew();
            var searchResults = SearchProducts(text, filters, out totalMatched);
            watch.Stop();

            _currentResultPage.Clear();
            _currentResultPage.AddRange(searchResults.Select(x => new SearchProductRow
            {
                Code = x.ProductCode,
                Name = x.ProductName,
                Qty = x.CurrentQty,
                Price = x.SalePrice,
                Location = x.LocationCode,
                ReorderLevel = x.ReorderLevel
            }));

            _gridSearchResults.RowCount = _currentResultPage.Count;
            _gridSearchResults.Refresh();
            UpdateResultInfo(_currentResultPage.Count, totalMatched);

            if (watch.ElapsedMilliseconds > 200)
            {
                SetIndexStatus(string.Format("Search {0} ms (optimize filter/query)", watch.ElapsedMilliseconds), true);
            }
            else
            {
                SetIndexStatus(string.Format("Search {0} ms", watch.ElapsedMilliseconds), false);
            }
        }

        private void UpdateResultInfo(int showing, int total)
        {
            _lblResultInfo.Text = string.Format("Showing {0} of {1} results — type more to narrow down", showing, total);
        }

        private SearchFilters BuildSearchFilters()
        {
            return new SearchFilters
            {
                CategoryId = _cmbCategory != null && _cmbCategory.SelectedIndex > 0 ? Convert.ToString(_cmbCategory.SelectedItem) : string.Empty,
                BrandId = _cmbBrand != null && _cmbBrand.SelectedIndex > 0 ? Convert.ToString(_cmbBrand.SelectedItem) : string.Empty,
                LocationCode = BuildLocationFilter(),
                LowStockOnly = _chkLowStockOnly != null && _chkLowStockOnly.Checked,
                ZeroStockOnly = _chkZeroStock != null && _chkZeroStock.Checked,
                UnverifiedOnly = _chkUnverifiedOnly != null && _chkUnverifiedOnly.Checked,
                UnverifiedCodes = new HashSet<string>(_sessionRows.Where(x => !x.Verified).Select(x => x.ProductCode), StringComparer.OrdinalIgnoreCase)
            };
        }

        private string BuildLocationFilter()
        {
            string aisle = _cmbAisle != null && _cmbAisle.SelectedIndex > 0 ? Convert.ToString(_cmbAisle.SelectedItem) : string.Empty;
            string shelf = _cmbShelf != null && _cmbShelf.SelectedIndex > 0 ? Convert.ToString(_cmbShelf.SelectedItem) : string.Empty;
            string bin = _cmbBin != null && _cmbBin.SelectedIndex > 0 ? Convert.ToString(_cmbBin.SelectedItem) : string.Empty;

            if (string.IsNullOrWhiteSpace(aisle) && string.IsNullOrWhiteSpace(shelf) && string.IsNullOrWhiteSpace(bin))
                return string.Empty;

            return string.Format("{0}-{1}-{2}", aisle, shelf, bin).Trim('-');
        }

        private List<ProductSearchItem> SearchProducts(string query, SearchFilters filters, out int totalMatched)
        {
            totalMatched = 0;
            var results = new List<ProductSearchScoredResult>(64);

            string safeQuery = (query ?? string.Empty).Trim();
            string queryLower = safeQuery.ToLowerInvariant();
            string[] tokens = queryLower.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            bool barcodeLike = IsBarcodeLike(safeQuery);

            List<ProductSearchItem> snapshot;
            lock (_productIndexLock)
            {
                snapshot = _productIndex;
            }

            foreach (var item in snapshot)
            {
                if (!item.IsActive)
                    continue;

                int score = 0;

                if (barcodeLike)
                {
                    if (string.Equals(item.Barcode, safeQuery, StringComparison.OrdinalIgnoreCase))
                        score += 1000;
                    else if (string.Equals(item.ProductCode, safeQuery, StringComparison.OrdinalIgnoreCase))
                        score += 700;
                    else if (item.BarcodeLower.IndexOf(queryLower, StringComparison.OrdinalIgnoreCase) >= 0)
                        score += 400;
                }
                else
                {
                    if (item.ProductCodeLower.StartsWith(queryLower, StringComparison.OrdinalIgnoreCase))
                        score += 100;

                    if (item.ProductNameLower.StartsWith(queryLower, StringComparison.OrdinalIgnoreCase))
                        score += 80;

                    if (item.ProductCodeLower.IndexOf(queryLower, StringComparison.OrdinalIgnoreCase) >= 0)
                        score += 50;

                    bool containsAll = true;
                    int anyCount = 0;
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        if (item.ProductNameLower.IndexOf(tokens[i], StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            anyCount++;
                        }
                        else
                        {
                            containsAll = false;
                        }
                    }

                    if (containsAll)
                        score += 40 * tokens.Length;

                    score += 10 * anyCount;
                }

                if (score <= 0)
                    continue;

                if (!PassFilters(item, filters))
                    continue;

                results.Add(new ProductSearchScoredResult { Item = item, Score = score });
            }

            totalMatched = results.Count;

            return results
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.Item.ProductName, StringComparer.OrdinalIgnoreCase)
                .Take(50)
                .Select(x => x.Item)
                .ToList();
        }

        private static bool IsBarcodeLike(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return false;

            bool allDigits = true;
            for (int i = 0; i < query.Length; i++)
            {
                if (!char.IsDigit(query[i]))
                {
                    allDigits = false;
                    break;
                }
            }

            if (allDigits)
                return true;

            if (query.Length >= 8 && query.IndexOf("-", StringComparison.OrdinalIgnoreCase) < 0 && query.IndexOf(" ", StringComparison.OrdinalIgnoreCase) < 0)
                return true;

            return false;
        }

        private static bool PassFilters(ProductSearchItem item, SearchFilters filters)
        {
            if (filters == null)
                return true;

            if (!string.IsNullOrWhiteSpace(filters.CategoryId)
                && item.CategoryId.IndexOf(filters.CategoryId, StringComparison.OrdinalIgnoreCase) < 0)
                return false;

            if (!string.IsNullOrWhiteSpace(filters.BrandId)
                && item.BrandId.IndexOf(filters.BrandId, StringComparison.OrdinalIgnoreCase) < 0)
                return false;

            if (!string.IsNullOrWhiteSpace(filters.LocationCode)
                && item.LocationCode.IndexOf(filters.LocationCode, StringComparison.OrdinalIgnoreCase) < 0)
                return false;

            if (filters.ZeroStockOnly && item.CurrentQty != 0)
                return false;

            if (filters.LowStockOnly && !(item.CurrentQty > 0 && item.CurrentQty <= item.ReorderLevel))
                return false;

            if (filters.UnverifiedOnly)
            {
                if (filters.UnverifiedCodes == null || filters.UnverifiedCodes.Count == 0)
                    return false;

                if (!filters.UnverifiedCodes.Contains(item.ProductCode))
                    return false;
            }

            return true;
        }

        private void GridSearchResults_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _currentResultPage.Count)
                return;

            var row = _currentResultPage[e.RowIndex];
            var columnName = _gridSearchResults.Columns[e.ColumnIndex].Name;

            if (columnName == "colCode") e.Value = row.Code;
            else if (columnName == "colName") e.Value = row.Name;
            else if (columnName == "colQty") e.Value = row.Qty.ToString("N2");
            else if (columnName == "colPrice") e.Value = row.Price.ToString("N2");
            else if (columnName == "colLoc") e.Value = row.Location;
        }

        private void GridSearchResults_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _currentResultPage.Count)
                return;

            var row = _currentResultPage[e.RowIndex];
            var columnName = _gridSearchResults.Columns[e.ColumnIndex].Name;

            if (columnName == "colName")
            {
                string name = row.Name ?? string.Empty;
                if (name.Length > 34)
                    e.Value = name.Substring(0, 34) + "...";
                _gridSearchResults.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = name;
            }
            else if (columnName == "colQty")
            {
                if (row.Qty <= 0)
                    e.CellStyle.ForeColor = Color.Firebrick;
                else if (row.Qty <= row.ReorderLevel)
                    e.CellStyle.ForeColor = Color.DarkOrange;
                else
                    e.CellStyle.ForeColor = Color.ForestGreen;
            }
            else if (columnName == "colLoc")
            {
                e.CellStyle.BackColor = Color.FromArgb(232, 240, 254);
                e.CellStyle.ForeColor = Color.FromArgb(21, 101, 192);
                e.CellStyle.SelectionBackColor = Color.FromArgb(21, 101, 192);
                e.CellStyle.SelectionForeColor = Color.White;
            }
        }

        private void GridSearchResults_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            AddSelectedResultToAdjustment();
        }

        private void GridSearchResults_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                AddSelectedResultToAdjustment();
            }
        }

        private void GridSearchResults_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                _gridSearchResults.ClearSelection();
                _gridSearchResults.Rows[e.RowIndex].Selected = true;
                _gridSearchResults.CurrentCell = _gridSearchResults.Rows[e.RowIndex].Cells[0];
            }
        }

        private void AddSelectedResultToAdjustment()
        {
            if (_gridSearchResults.CurrentRow == null)
                return;

            int rowIndex = _gridSearchResults.CurrentRow.Index;
            if (rowIndex < 0 || rowIndex >= _currentResultPage.Count)
                return;

            var selected = _currentResultPage[rowIndex];
            InsertRecent(selected);
            UpsertSessionRow(selected);
        }

        private void UpsertSessionRow(SearchProductRow selected)
        {
            var existing = _sessionRows.FirstOrDefault(x => string.Equals(x.ProductCode, selected.Code, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                _gridAdjustment.RowCount = _sessionRows.Count;
                _gridAdjustment.Refresh();
                return;
            }

            var newRow = new AdjustmentGridRow
            {
                Verified = false,
                ProductCode = selected.Code,
                ProductName = selected.Name,
                Category = _cmbCategory.SelectedIndex > 0 ? Convert.ToString(_cmbCategory.SelectedItem) : "General",
                CurrentQty = selected.Qty,
                NewQty = selected.Qty,
                CurrentPrice = selected.Price,
                NewPrice = selected.Price,
                CurrentLocation = selected.Location,
                NewLocation = selected.Location,
                Reason = "Physical Count",
                Notes = string.Empty,
                IsPosted = false
            };

            ApplyLastReason(newRow);
            _sessionRows.Add(newRow);
            MarkDirty();
            RefreshAdjustmentGrid();
        }

        private void ViewSelectedProductDetail()
        {
            if (_gridSearchResults.CurrentRow == null)
                return;

            AddSelectedResultToAdjustment();
        }

        private void ViewSelectedStockHistory()
        {
            if (_gridSearchResults.CurrentRow == null)
                return;

            int rowIndex = _gridSearchResults.CurrentRow.Index;
            if (rowIndex < 0 || rowIndex >= _currentResultPage.Count)
                return;

            var selected = _currentResultPage[rowIndex];
            var histForm = new frm_stock_history(0, selected.Name);
            // Let the user enter the numeric product id in the form (we have code, not id here)
            // Pre-fill the search box with the code so they can look it up
            histForm.Show(this);
        }

        private void InsertRecent(SearchProductRow product)
        {
            _recentAdded.RemoveAll(x => string.Equals(x.Code, product.Code, StringComparison.OrdinalIgnoreCase));
            _recentAdded.Insert(0, product);

            if (_recentAdded.Count > 10)
                _recentAdded.RemoveRange(10, _recentAdded.Count - 10);

            RefreshRecentGrid();
        }

        private void RefreshRecentGrid()
        {
            _gridRecent.Rows.Clear();
            foreach (var item in _recentAdded)
            {
                bool isPinned = _pinnedCodes.Contains(item.Code);
                _gridRecent.Rows.Add(isPinned, item.Code, item.Name);
            }
        }

        private void GridRecent_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (_gridRecent.IsCurrentCellDirty)
                _gridRecent.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void GridRecent_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 0)
                return;

            if (_gridRecent.Rows[e.RowIndex].Cells["colRecentCode"].Value == null)
                return;

            string code = Convert.ToString(_gridRecent.Rows[e.RowIndex].Cells["colRecentCode"].Value);
            bool pinned = Convert.ToBoolean(_gridRecent.Rows[e.RowIndex].Cells["colPin"].Value);

            if (pinned)
                _pinnedCodes.Add(code);
            else
                _pinnedCodes.Remove(code);
        }

        private void GridAdjustment_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _sessionRows.Count)
                return;

            var row = _sessionRows[e.RowIndex];
            string col = _gridAdjustment.Columns[e.ColumnIndex].Name;

            if (col == ColRowNo) e.Value = e.RowIndex + 1;
            else if (col == ColVerified) e.Value = row.Verified;
            else if (col == ColCode) e.Value = row.ProductCode;
            else if (col == ColName) e.Value = row.ProductName;
            else if (col == ColCategory) e.Value = row.Category;
            else if (col == ColCurrentQty) e.Value = row.CurrentQty.ToString("N2");
            else if (col == ColNewQty) e.Value = row.NewQty.ToString("N2");
            else if (col == ColQtyDiff) e.Value = row.QtyDiff.ToString("N2");
            else if (col == ColCurrentPrice) e.Value = row.CurrentPrice.ToString("N2");
            else if (col == ColNewPrice) e.Value = row.NewPrice.ToString("N2");
            else if (col == ColPriceDiff) e.Value = row.PriceDiff.ToString("N2");
            else if (col == ColCurrentLoc) e.Value = row.CurrentLocation;
            else if (col == ColNewLoc) e.Value = row.NewLocation;
            else if (col == ColReason) e.Value = row.Reason;
            else if (col == ColNotes) e.Value = row.Notes;
            else if (col == ColActions) e.Value = "?";
        }

        private void GridAdjustment_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _sessionRows.Count)
                return;

            var row = _sessionRows[e.RowIndex];
            string col = _gridAdjustment.Columns[e.ColumnIndex].Name;

            if (col == ColVerified)
                row.Verified = Convert.ToBoolean(e.Value);
            else if (col == ColNewQty)
                row.NewQty = ParseDecimalSafe(e.Value, row.NewQty);
            else if (col == ColNewPrice)
                row.NewPrice = ParseDecimalSafe(e.Value, row.NewPrice);
            else if (col == ColNewLoc)
                row.NewLocation = Convert.ToString(e.Value);
            else if (col == ColReason)
                row.Reason = Convert.ToString(e.Value);
            else if (col == ColNotes)
                row.Notes = Convert.ToString(e.Value);

            RecalculateFooterSummary();
            UpdateGridToolbarStats();
            _gridAdjustment.InvalidateRow(e.RowIndex);
        }

        private decimal ParseDecimalSafe(object value, decimal fallback)
        {
            decimal parsed;
            return decimal.TryParse(Convert.ToString(value), out parsed) ? parsed : fallback;
        }

        private void GridAdjustment_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _sessionRows.Count)
                return;

            var row = _sessionRows[e.RowIndex];
            string col = _gridAdjustment.Columns[e.ColumnIndex].Name;

            if (col == ColName)
            {
                string name = row.ProductName ?? string.Empty;
                if (name.Length > 38)
                    e.Value = name.Substring(0, 38) + "...";
                _gridAdjustment.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = name;
            }
            else if (col == ColQtyDiff)
            {
                if (row.QtyDiff > 0) e.CellStyle.ForeColor = Color.ForestGreen;
                else if (row.QtyDiff < 0) e.CellStyle.ForeColor = Color.Firebrick;
                else e.CellStyle.ForeColor = Color.Gray;
            }
            else if (col == ColPriceDiff)
            {
                if (row.PriceDiff > 0) e.CellStyle.ForeColor = Color.ForestGreen;
                else if (row.PriceDiff < 0) e.CellStyle.ForeColor = Color.Firebrick;
                else e.CellStyle.ForeColor = Color.Gray;
            }

            if (row.NewQty <= 0)
            {
                e.CellStyle.BackColor = Color.FromArgb(255, 238, 238);
            }

            if (row.IsModified && col == ColRowNo)
            {
                e.CellStyle.BackColor = Color.FromArgb(21, 101, 192);
                e.CellStyle.ForeColor = Color.White;
            }
        }

        private void GridAdjustment_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _sessionRows.Count)
                return;

            var row = _sessionRows[e.RowIndex];
            string marker = string.Empty;

            if (row.CurrentQty > 0)
            {
                decimal pct = Math.Abs(row.QtyDiff) / row.CurrentQty;
                if (pct > 0.20m)
                    marker = "?";
            }
            _gridAdjustment.Rows[e.RowIndex].HeaderCell.Value = marker;
        }

        private void GridAdjustment_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _sessionRows.Count)
                return;

            var row = _sessionRows[e.RowIndex];
            if (row.CurrentQty > 0)
            {
                decimal pct = Math.Abs(row.QtyDiff) / row.CurrentQty;
                _gridAdjustment.Rows[e.RowIndex].HeaderCell.ToolTipText = pct > 0.20m ? "Difference exceeds 20%" : string.Empty;
            }
        }

        private void GridAdjustment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _sessionRows.Count)
                return;

            if (_gridAdjustment.Columns[e.ColumnIndex].Name == ColActions)
            {
                _sessionRows.RemoveAt(e.RowIndex);
                RefreshAdjustmentGrid();
            }
        }

        private void GridAdjustment_KeyDown(object sender, KeyEventArgs e)
        {
            if (HandleUndoKey(e)) return;

            if (_gridAdjustment.CurrentCell == null)
                return;

            int rowIndex = _gridAdjustment.CurrentCell.RowIndex;
            int colIndex = _gridAdjustment.CurrentCell.ColumnIndex;
            string colName = _gridAdjustment.Columns[colIndex].Name;

            if (e.KeyCode == Keys.F4)
            {
                e.Handled = true;
                ShowProductDrawerForRow(rowIndex);
                return;
            }

            if (e.KeyCode == Keys.F6)
            {
                e.Handled = true;
                ToggleScanMode(true);
                return;
            }

            if (_isScanMode && e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                ConfirmScanAndNext();
                return;
            }

            if (e.KeyCode == Keys.Enter && colName == ColNewQty)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                int target = _gridAdjustment.Columns[ColNewPrice].Index;
                _gridAdjustment.CurrentCell = _gridAdjustment.Rows[rowIndex].Cells[target];
                _gridAdjustment.BeginEdit(true);
                return;
            }

            if (e.KeyCode == Keys.Tab && colName == ColNotes)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                int nextRow = Math.Min(rowIndex + 1, _gridAdjustment.RowCount - 1);
                int target = _gridAdjustment.Columns[ColNewQty].Index;
                _gridAdjustment.CurrentCell = _gridAdjustment.Rows[nextRow].Cells[target];
                _gridAdjustment.BeginEdit(true);
            }
        }

        private void BulkSetLocation()
        {
            foreach (DataGridViewRow selectedRow in _gridAdjustment.SelectedRows)
            {
                if (selectedRow.Index >= 0 && selectedRow.Index < _sessionRows.Count)
                    _sessionRows[selectedRow.Index].NewLocation = "A3-S2-B4";
            }
            RefreshAdjustmentGrid();
        }

        private void BulkApplyPricePercent()
        {
            decimal percentage = PromptDecimal("Apply Price Change %", "Enter percentage (+/-):", 0m);
            foreach (DataGridViewRow selectedRow in _gridAdjustment.SelectedRows)
            {
                if (selectedRow.Index >= 0 && selectedRow.Index < _sessionRows.Count)
                {
                    var row = _sessionRows[selectedRow.Index];
                    row.NewPrice = row.CurrentPrice + (row.CurrentPrice * percentage / 100m);
                }
            }
            RefreshAdjustmentGrid();
        }

        private void BulkMarkVerified()
        {
            foreach (DataGridViewRow selectedRow in _gridAdjustment.SelectedRows)
            {
                if (selectedRow.Index >= 0 && selectedRow.Index < _sessionRows.Count)
                    _sessionRows[selectedRow.Index].Verified = true;
            }
            RefreshAdjustmentGrid();
        }

        private decimal PromptDecimal(string title, string label, decimal defaultValue)
        {
            using (var frm = new Form())
            {
                frm.Text = title;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.FormBorderStyle = FormBorderStyle.FixedDialog;
                frm.MinimizeBox = false;
                frm.MaximizeBox = false;
                frm.Width = 280;
                frm.Height = 150;

                var lbl = new Label { Left = 10, Top = 12, Width = 240, Text = label };
                var txt = new TextBox { Left = 10, Top = 34, Width = 240, Text = defaultValue.ToString("N2") };
                var ok = new Button { Text = "OK", Left = 94, Width = 72, Top = 68, DialogResult = DialogResult.OK };
                var cancel = new Button { Text = "Cancel", Left = 178, Width = 72, Top = 68, DialogResult = DialogResult.Cancel };

                frm.Controls.Add(lbl);
                frm.Controls.Add(txt);
                frm.Controls.Add(ok);
                frm.Controls.Add(cancel);
                frm.AcceptButton = ok;
                frm.CancelButton = cancel;

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    decimal val;
                    return decimal.TryParse(txt.Text, out val) ? val : defaultValue;
                }
            }

            return defaultValue;
        }

        private void ColumnMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item == null || item.Tag == null)
                return;

            string colName = Convert.ToString(item.Tag);
            if (_gridAdjustment.Columns.Contains(colName))
                _gridAdjustment.Columns[colName].Visible = item.Checked;
        }

        private void RefreshAdjustmentGrid()
        {
            SetGridRowCount(_sessionRows.Count);
            UpdateGridToolbarStats();
            RecalculateFooterSummary();
        }

        private void UpdateGridToolbarStats()
        {
            int total = _sessionRows.Count;
            int modified = _sessionRows.Count(x => x.IsModified);
            int posted = _sessionRows.Count(x => x.IsPosted);

            _lblSessionCount.Text = string.Format("{0} items in session", total);
            _lblModifiedBadge.Text = string.Format("{0} modified", modified);
            _lblPostedBadge.Text = string.Format("{0} posted", posted);
        }

        private void RecalculateFooterSummary()
        {
            decimal totalIncrease = _sessionRows.Where(x => x.QtyDiff > 0).Sum(x => x.QtyDiff);
            decimal totalDecrease = _sessionRows.Where(x => x.QtyDiff < 0).Sum(x => Math.Abs(x.QtyDiff));
            decimal valueChange = _sessionRows.Sum(x => (x.NewQty * x.NewPrice) - (x.CurrentQty * x.CurrentPrice));

            _lblFooterSummary.Text = string.Format(
                "Total Items: {0} | Total Qty Increase: +{1:N2} | Total Qty Decrease: -{2:N2} | Net Stock Value Change: {3}SAR {4:N2}",
                _sessionRows.Count,
                totalIncrease,
                totalDecrease,
                valueChange >= 0 ? "+" : "-",
                Math.Abs(valueChange));
        }

        private void LoadNewSession()
        {
            try
            {
                var salesBll = new SalesBLL();
                txtAdjustmentNo.Text = salesBll.GenerateAdjustmentInvoiceNo();
                dtpAdjustmentDate.Value = DateTime.Today;
            }
            catch
            {
                txtAdjustmentNo.Text = string.Empty;
            }
        }

        public void SetSessionStatus(string status)
        {
            lblSessionStatus.Text = status;

            if (string.Equals(status, "Posted", StringComparison.OrdinalIgnoreCase))
            {
                lblSessionStatus.BackColor = _postedColor;
            }
            else if (string.Equals(status, "In Progress", StringComparison.OrdinalIgnoreCase))
            {
                lblSessionStatus.BackColor = _inProgressColor;
            }
            else
            {
                lblSessionStatus.BackColor = _draftColor;
            }
        }

        public void UpdateVerificationProgress(int verified, int total)
        {
            int safeTotal = total < 0 ? 0 : total;
            int safeVerified = verified < 0 ? 0 : verified;
            if (safeVerified > safeTotal)
                safeVerified = safeTotal;

            progressVerification.Maximum = safeTotal == 0 ? 1 : safeTotal;
            progressVerification.Value = safeTotal == 0 ? 0 : safeVerified;
            lblVerificationProgress.Text = string.Format("{0} of {1} products verified", safeVerified, safeTotal);
        }

        public void UpdateBottomSummary(int totalProducts, int itemsAdjusted, int itemsPending, decimal valueChange, string lastSavedTime, string currentUser, string warehouse)
        {
            tslTotals.Text = string.Format("Total: {0}  |  Adjusted: {1}  |  Pending: {2}", totalProducts, itemsAdjusted, itemsPending);

            tslValueChange.Text = string.Format("Value Change: {0}{1:N2}", valueChange >= 0 ? "+" : string.Empty, valueChange);
            tslValueChange.ForeColor = valueChange < 0 ? Color.Firebrick : Color.ForestGreen;

            if (!_isIndexLoading)
                tslMeta.Text = string.Format("Last saved: {0}  |  User: {1}  |  Warehouse: {2}", lastSavedTime, currentUser, warehouse);
        }

        private void btnNewSession_Click(object sender, EventArgs e)
        {
            LoadNewSession();
            SetSessionStatus("Draft");
            _sessionRows.Clear();
            RefreshAdjustmentGrid();
        }

        private void BuildProductDetailDrawer()
        {
            _drawerPanel = new Panel
            {
                Width = 340,
                Dock = DockStyle.Right,
                BackColor = Color.White,
                Visible = false,
                Padding = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };
            _drawerPanel.BringToFront();

            _btnDrawerClose = new Button
            {
                Text = "?",
                FlatStyle = FlatStyle.Flat,
                Width = 28,
                Height = 24,
                Location = new Point(300, 8)
            };
            _btnDrawerClose.FlatAppearance.BorderSize = 0;
            _btnDrawerClose.Click += (s, e) => HideProductDrawer();

            _drawerHeader = new Label { Left = 10, Top = 10, Width = 260, Height = 24, Font = new Font("Segoe UI", 10F, FontStyle.Bold), Text = "Product Details" };
            _drawerImage = new PictureBox { Left = 10, Top = 40, Width = 72, Height = 72, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.FromArgb(245, 247, 250) };
            _drawerCode = new Label { Left = 92, Top = 44, Width = 230, Height = 20, Font = new Font("Consolas", 9F, FontStyle.Bold) };
            _drawerCategory = new Label { Left = 92, Top = 68, Width = 230, Height = 40, ForeColor = Color.DimGray };

            _drawerStockInfo = new Label { Left = 10, Top = 118, Width = 312, Height = 50, Font = new Font("Segoe UI", 8.5F) };

            _drawerStockChart = new Chart { Left = 10, Top = 170, Width = 312, Height = 130, BackColor = Color.White };
            var area = new ChartArea("stockArea");
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 7.5F);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 7.5F);
            _drawerStockChart.ChartAreas.Add(area);
            var series = new Series("ClosingStock") { ChartType = SeriesChartType.Column, Color = Color.FromArgb(21, 101, 192) };
            _drawerStockChart.Series.Add(series);

            _drawerPricingInfo = new Label { Left = 10, Top = 305, Width = 312, Height = 44, Font = new Font("Segoe UI", 8.5F) };
            _drawerLocationInfo = new Label { Left = 10, Top = 352, Width = 312, Height = 56, Font = new Font("Segoe UI", 8.5F) };
            _drawerReorderInfo = new Label { Left = 10, Top = 410, Width = 312, Height = 34, Font = new Font("Segoe UI", 8.5F) };
            _drawerTransactions = new Label { Left = 10, Top = 446, Width = 312, Height = 96, Font = new Font("Consolas", 8F) };

            _btnQuickAdjust = new Button
            {
                Text = "Quick Adjust",
                FlatStyle = FlatStyle.Flat,
                Width = 120,
                Height = 28,
                Left = 202,
                Top = 548,
                BackColor = Color.FromArgb(21, 101, 192),
                ForeColor = Color.White
            };
            _btnQuickAdjust.Click += (s, e) => QuickAdjustCurrentDrawerProduct();

            _drawerPanel.Controls.Add(_btnDrawerClose);
            _drawerPanel.Controls.Add(_drawerHeader);
            _drawerPanel.Controls.Add(_drawerImage);
            _drawerPanel.Controls.Add(_drawerCode);
            _drawerPanel.Controls.Add(_drawerCategory);
            _drawerPanel.Controls.Add(_drawerStockInfo);
            _drawerPanel.Controls.Add(_drawerStockChart);
            _drawerPanel.Controls.Add(_drawerPricingInfo);
            _drawerPanel.Controls.Add(_drawerLocationInfo);
            _drawerPanel.Controls.Add(_drawerReorderInfo);
            _drawerPanel.Controls.Add(_drawerTransactions);
            _drawerPanel.Controls.Add(_btnQuickAdjust);

            _rightRoot.Controls.Add(_drawerPanel);
            _drawerPanel.BringToFront();
        }

        private void BuildScanModeOverlay()
        {
            _scanOverlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(28, 33, 41),
                Visible = false
            };

            _btnScanExit = new Button
            {
                Text = "Exit Scan Mode",
                FlatStyle = FlatStyle.Flat,
                Width = 128,
                Height = 28,
                Left = 12,
                Top = 10,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(55, 71, 79)
            };
            _btnScanExit.Click += (s, e) => ToggleScanMode(false);

            _scanTitle = new Label
            {
                Left = 160,
                Top = 14,
                Width = 560,
                Height = 28,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                Text = "Barcode Scan Mode"
            };

            _scanReady = new Label
            {
                Left = 160,
                Top = 70,
                Width = 420,
                Height = 34,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                Text = "??  Ready to Scan..."
            };

            _scanProduct = new Label { Left = 160, Top = 118, Width = 520, Height = 36, ForeColor = Color.White, Font = new Font("Segoe UI", 16F, FontStyle.Bold) };
            _scanCurrentQty = new Label { Left = 160, Top = 160, Width = 340, Height = 30, ForeColor = Color.White, Font = new Font("Segoe UI", 14F) };

            _scanQty = new NumericUpDown
            {
                Left = 160,
                Top = 200,
                Width = 180,
                Height = 38,
                DecimalPlaces = 2,
                Maximum = 999999,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold)
            };

            _btnScanConfirm = new Button
            {
                Text = "Confirm & Next",
                Left = 350,
                Top = 203,
                Width = 130,
                Height = 32,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(21, 101, 192),
                ForeColor = Color.White
            };
            _btnScanConfirm.Click += (s, e) => ConfirmScanAndNext();

            _btnScanNote = new Button
            {
                Text = "Add Note",
                Left = 486,
                Top = 203,
                Width = 90,
                Height = 32,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(84, 110, 122),
                ForeColor = Color.White
            };
            _btnScanNote.Click += (s, e) => AddScanNote();

            _scanRunningCount = new Label
            {
                Left = 160,
                Top = 246,
                Width = 340,
                Height = 26,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Text = "0 products scanned this session"
            };

            _scanNotFound = new Label
            {
                Left = 160,
                Top = 276,
                Width = 520,
                Height = 24,
                ForeColor = Color.FromArgb(239, 83, 80),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Visible = false
            };

            var logTitle = new Label
            {
                Left = 720,
                Top = 70,
                Width = 220,
                Height = 22,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Text = "Scan Log (last 20)"
            };

            _scanLog = new ListBox
            {
                Left = 720,
                Top = 96,
                Width = 330,
                Height = 340,
                Font = new Font("Consolas", 9F),
                BackColor = Color.FromArgb(20, 25, 32),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            _scanInputHidden = new TextBox { Left = -200, Top = -200, Width = 1, Height = 1 };
            _scanInputHidden.KeyDown += ScanInputHidden_KeyDown;

            _scanOverlay.Controls.Add(_btnScanExit);
            _scanOverlay.Controls.Add(_scanTitle);
            _scanOverlay.Controls.Add(_scanReady);
            _scanOverlay.Controls.Add(_scanProduct);
            _scanOverlay.Controls.Add(_scanCurrentQty);
            _scanOverlay.Controls.Add(_scanQty);
            _scanOverlay.Controls.Add(_btnScanConfirm);
            _scanOverlay.Controls.Add(_btnScanNote);
            _scanOverlay.Controls.Add(_scanRunningCount);
            _scanOverlay.Controls.Add(_scanNotFound);
            _scanOverlay.Controls.Add(logTitle);
            _scanOverlay.Controls.Add(_scanLog);
            _scanOverlay.Controls.Add(_scanInputHidden);

            _rightRoot.Controls.Add(_scanOverlay);
            _scanOverlay.BringToFront();
        }

        private void ShowProductDrawerForRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= _sessionRows.Count)
                return;

            var row = _sessionRows[rowIndex];
            _drawerCode.Text = string.Format("{0} - {1}", row.ProductCode, row.ProductName);
            _drawerCategory.Text = string.Format("Category: {0}", row.Category);
            _drawerStockInfo.Text = string.Format("System Qty: {0:N2}\nLast Physical Count: {1:yyyy-MM-dd} | Last Adjustment: {2:yyyy-MM-dd}", row.CurrentQty, DateTime.Today.AddDays(-7), DateTime.Today.AddDays(-2));
            _drawerPricingInfo.Text = string.Format("Cost Price: {0:N2} | Sale Price: {1:N2}\nMargin %: {2:N2}% | Last Price Change: {3:yyyy-MM-dd}",
                row.CurrentPrice * 0.78m,
                row.CurrentPrice,
                row.CurrentPrice <= 0 ? 0 : ((row.CurrentPrice - (row.CurrentPrice * 0.78m)) / (row.CurrentPrice * 0.78m)) * 100m,
                DateTime.Today.AddDays(-12));

            _drawerLocationInfo.Text = string.Format("Locations:\n• {0}: {1:N2}\n• A2-S1-B2: {2:N2}\n• A4-S3-B1: {3:N2}",
                row.CurrentLocation,
                row.CurrentQty,
                Math.Max(0, row.CurrentQty - 4),
                4m);

            _drawerReorderInfo.Text = string.Format("Min Qty: {0:N2} | Reorder Level: {1:N2} | Reorder Qty: {2:N2}", 5m, 8m, 20m);

            _drawerTransactions.Text =
                "Last 5 Transactions\n"
                + string.Format("{0:MM-dd} Sale       -2.00  usr1\n", DateTime.Today.AddDays(-1))
                + string.Format("{0:MM-dd} Purchase   +6.00  usr2\n", DateTime.Today.AddDays(-3))
                + string.Format("{0:MM-dd} Adjustment +1.00  usr1\n", DateTime.Today.AddDays(-6))
                + string.Format("{0:MM-dd} Sale       -1.00  usr3\n", DateTime.Today.AddDays(-9))
                + string.Format("{0:MM-dd} Purchase   +3.00  usr2", DateTime.Today.AddDays(-13));

            var series = _drawerStockChart.Series[0];
            series.Points.Clear();
            for (int i = 5; i >= 0; i--)
            {
                DateTime d = DateTime.Today.AddMonths(-i);
                decimal v = Math.Max(0, row.CurrentQty + (5 - i) - (i % 3));
                series.Points.AddXY(d.ToString("MMM"), v);
            }

            _btnQuickAdjust.Tag = row.ProductCode;
            _drawerPanel.Visible = true;
            _drawerPanel.BringToFront();
        }

        private void HideProductDrawer()
        {
            _drawerPanel.Visible = false;
        }

        private void QuickAdjustCurrentDrawerProduct()
        {
            string code = Convert.ToString(_btnQuickAdjust.Tag);
            var row = _sessionRows.FirstOrDefault(x => string.Equals(x.ProductCode, code, StringComparison.OrdinalIgnoreCase));
            if (row == null)
                return;

            int idx = _sessionRows.IndexOf(row);
            if (idx < 0)
                return;

            _gridAdjustment.ClearSelection();
            _gridAdjustment.Rows[idx].Selected = true;
            _gridAdjustment.CurrentCell = _gridAdjustment.Rows[idx].Cells[_gridAdjustment.Columns[ColNewQty].Index];
            _gridAdjustment.BeginEdit(true);
        }

        private void ToggleScanMode(bool enable)
        {
            _isScanMode = enable;
            _scanOverlay.Visible = enable;

            if (enable)
            {
                _scanReady.Text = "??  Ready to Scan...";
                _scanProduct.Text = string.Empty;
                _scanCurrentQty.Text = string.Empty;
                _scanNotFound.Visible = false;
                _scanQty.Value = 0;
                _scanInputHidden.Text = string.Empty;
                _scanInputHidden.Focus();
                _scanOverlay.BringToFront();
            }
        }

        private void ScanInputHidden_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            e.Handled = true;
            e.SuppressKeyPress = true;

            string barcode = (_scanInputHidden.Text ?? string.Empty).Trim();
            _scanInputHidden.Clear();

            if (string.IsNullOrWhiteSpace(barcode))
                return;

            var row = _sessionRows.FirstOrDefault(x => string.Equals(x.ProductCode, barcode, StringComparison.OrdinalIgnoreCase));
            if (row == null)
            {
                System.Media.SystemSounds.Beep.Play();
                _scanNotFound.Text = "Product not found — Add New?";
                _scanNotFound.Visible = true;
                return;
            }

            _scanNotFound.Visible = false;
            _scanProduct.Text = row.ProductName;
            _scanCurrentQty.Text = string.Format("Current Qty: {0:N2}", row.CurrentQty);
            _scanQty.Value = Math.Max(_scanQty.Minimum, Math.Min(_scanQty.Maximum, row.NewQty));
            _scanQty.Focus();
        }

        private void ConfirmScanAndNext()
        {
            if (!_isScanMode)
                return;

            string productName = _scanProduct.Text;
            if (string.IsNullOrWhiteSpace(productName))
            {
                _scanInputHidden.Focus();
                return;
            }

            var row = _sessionRows.FirstOrDefault(x => string.Equals(x.ProductName, productName, StringComparison.OrdinalIgnoreCase));
            if (row != null)
            {
                row.NewQty = _scanQty.Value;
                row.Verified = true;
                _scannedCount++;
                _scanRunningCount.Text = string.Format("{0} products scanned this session", _scannedCount);

                string logItem = string.Format("{0:HH:mm:ss}  {1}  Qty:{2:N2}", DateTime.Now, row.ProductCode, row.NewQty);
                _scanLogItems.Enqueue(logItem);
                while (_scanLogItems.Count > 20)
                    _scanLogItems.Dequeue();

                _scanLog.Items.Clear();
                foreach (var x in _scanLogItems.Reverse())
                    _scanLog.Items.Add(x);

                RefreshAdjustmentGrid();
            }

            _scanReady.Text = "??  Ready to Scan...";
            _scanProduct.Text = string.Empty;
            _scanCurrentQty.Text = string.Empty;
            _scanQty.Value = 0;
            _scanInputHidden.Focus();
        }

        private void AddScanNote()
        {
            if (string.IsNullOrWhiteSpace(_scanProduct.Text))
                return;

            string note = PromptText("Scan Note", "Enter note for scanned product:");
            if (string.IsNullOrWhiteSpace(note))
                return;

            var row = _sessionRows.FirstOrDefault(x => string.Equals(x.ProductName, _scanProduct.Text, StringComparison.OrdinalIgnoreCase));
            if (row != null)
            {
                row.Notes = note;
                RefreshAdjustmentGrid();
            }
        }

        private string PromptText(string title, string label)
        {
            using (var frm = new Form())
            {
                frm.Text = title;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.FormBorderStyle = FormBorderStyle.FixedDialog;
                frm.MinimizeBox = false;
                frm.MaximizeBox = false;
                frm.Width = 360;
                frm.Height = 170;

                var lbl = new Label { Left = 10, Top = 12, Width = 320, Text = label };
                var txt = new TextBox { Left = 10, Top = 36, Width = 320 };
                var ok = new Button { Text = "OK", Left = 178, Width = 72, Top = 72, DialogResult = DialogResult.OK };
                var cancel = new Button { Text = "Cancel", Left = 258, Width = 72, Top = 72, DialogResult = DialogResult.Cancel };

                frm.Controls.Add(lbl);
                frm.Controls.Add(txt);
                frm.Controls.Add(ok);
                frm.Controls.Add(cancel);
                frm.AcceptButton = ok;
                frm.CancelButton = cancel;

                return frm.ShowDialog(this) == DialogResult.OK ? txt.Text : string.Empty;
            }
        }

    }
}


