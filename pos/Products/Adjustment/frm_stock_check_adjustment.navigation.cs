using POS.BLL;
using POS.Core;
using pos.Reports.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_stock_check_adjustment
    {
        private sealed class CategoryNavNode
        {
            public int Id { get; set; }
            public int ParentId { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public int BranchCount { get; set; }
            public bool HasChildren { get; set; }
        }

        private sealed class LocationNavNode
        {
            public string Warehouse { get; set; }
            public string Aisle { get; set; }
            public string Shelf { get; set; }
            public string Bin { get; set; }
            public string Code { get; set; }
            public int ProductCount { get; set; }
        }

        // SQL helper queries requested
        private const string SqlGetCategoryTreeWithCounts = @"
;WITH CategoryTree AS (
    SELECT c.id AS category_id, ISNULL(c.parent_id, 0) AS parent_id, c.code AS category_code, c.name AS category_name
    FROM pos_categories c
    WHERE ISNULL(c.deleted, 0) = 0
), CategoryClosure AS (
    SELECT ct.category_id AS root_category_id, ct.category_id
    FROM CategoryTree ct
    UNION ALL
    SELECT cc.root_category_id, ct.category_id
    FROM CategoryClosure cc
    INNER JOIN CategoryTree ct ON ct.parent_id = cc.category_id
), CategoryCounts AS (
    SELECT cc.root_category_id AS category_id, COUNT_BIG(p.id) AS product_count
    FROM CategoryClosure cc
    LEFT JOIN pos_products p ON (p.category_id = cc.category_id OR CAST(p.category_code AS nvarchar(50)) = CAST(cc.category_id AS nvarchar(50))) AND ISNULL(p.deleted, 0) = 0
    GROUP BY cc.root_category_id
)
SELECT ct.category_id, ct.parent_id, ct.category_code, ct.category_name,
       ISNULL(cc.product_count, 0) AS product_count,
       CASE WHEN EXISTS (SELECT 1 FROM CategoryTree ch WHERE ch.parent_id = ct.category_id) THEN 1 ELSE 0 END AS has_children
FROM CategoryTree ct
LEFT JOIN CategoryCounts cc ON cc.category_id = ct.category_id
ORDER BY ct.category_name;
";

        private const string SqlGetAllWarehouseLocations = @"
SELECT
    ISNULL(NULLIF(l.warehouse_code, ''), 'Main Store') AS warehouse_code,
    PARSENAME(REPLACE(l.code, '-', '.'), 3) AS aisle_code,
    PARSENAME(REPLACE(l.code, '-', '.'), 2) AS shelf_code,
    PARSENAME(REPLACE(l.code, '-', '.'), 1) AS bin_code,
    l.code AS location_code,
    COUNT(p.id) AS product_count
FROM pos_locations l
LEFT JOIN pos_products p ON p.location_code = l.code AND ISNULL(p.deleted, 0) = 0
WHERE ISNULL(l.deleted, 0) = 0
GROUP BY l.warehouse_code, l.code
ORDER BY warehouse_code, aisle_code, shelf_code, bin_code;
";

        private const string SqlGetProductsByCategoryIncludingChildren = @"
DECLARE @CategoryId INT = @category_id;
;WITH CategoryTree AS (
    SELECT c.id
    FROM pos_categories c
    WHERE c.id = @CategoryId AND ISNULL(c.deleted, 0) = 0
    UNION ALL
    SELECT c.id
    FROM pos_categories c
    INNER JOIN CategoryTree t ON t.id = c.parent_id
    WHERE ISNULL(c.deleted, 0) = 0
)
SELECT
    p.id AS product_id,
    p.code AS product_code,
    p.name AS product_name,
    p.barcode,
    ISNULL(CAST(p.category_id AS nvarchar(50)), CAST(p.category_code AS nvarchar(50))) AS category_id,
    ISNULL(CAST(p.brand_id AS nvarchar(50)), CAST(p.brand_code AS nvarchar(50))) AS brand_id,
    p.location_code,
    p.unit_price AS sale_price,
    ISNULL(p.is_active, 1) AS is_active
FROM pos_products p
INNER JOIN CategoryTree ct ON (p.category_id = ct.id OR CAST(p.category_code AS nvarchar(50)) = CAST(ct.id AS nvarchar(50)))
WHERE ISNULL(p.deleted, 0) = 0;
";

        private const string LazyCategoryNodeTag = "__lazy__";

        private readonly Dictionary<int, CategoryNavNode> _categoryMap = new Dictionary<int, CategoryNavNode>();
        private readonly Dictionary<int, List<CategoryNavNode>> _categoryChildrenMap = new Dictionary<int, List<CategoryNavNode>>();
        private readonly List<LocationNavNode> _locationNavData = new List<LocationNavNode>();
        private HashSet<string> _selectedCategoryBranchIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private Panel _categoryTreePanel;
        private TreeView _categoryTree;
        private TextBox _txtCategoryFilter;
        private Label _lblCategoryTreeInfo;
        private ContextMenuStrip _gridLocationMenu;
        private bool _navInitialized;
        private bool _bulkOpsInitialized;

        private enum BulkPriceChangeType
        {
            IncreaseFixed,
            IncreasePercent,
            DecreaseFixed,
            DecreasePercent,
            SetSpecific
        }

        private enum BulkPriceApplyScope
        {
            SelectedRows,
            AllRowsInSession,
            AllRowsInCategory
        }

        private sealed class BulkPriceRule
        {
            public BulkPriceChangeType ChangeType { get; set; }
            public decimal Value { get; set; }
            public int? RoundToNearest { get; set; }
            public BulkPriceApplyScope ApplyScope { get; set; }
            public string Category { get; set; }
        }

        private sealed class StockImportValidRow
        {
            public int ProductId { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string Category { get; set; }
            public decimal SystemQty { get; set; }
            public decimal PhysicalQty { get; set; }
            public decimal? NewSalePrice { get; set; }
            public string NewLocation { get; set; }
            public string Notes { get; set; }
            public decimal CurrentSalePrice { get; set; }
            public string CurrentLocation { get; set; }
        }

        private sealed class StockImportErrorRow
        {
            public int RowNo { get; set; }
            public string ProductCode { get; set; }
            public string ErrorMessage { get; set; }
        }

        private sealed class StockImportResult
        {
            public List<StockImportValidRow> ValidRows { get; set; }
            public List<StockImportErrorRow> ErrorRows { get; set; }
            public int SkippedRows { get; set; }
            public DataTable PreviewTable { get; set; }

            public StockImportResult()
            {
                ValidRows = new List<StockImportValidRow>();
                ErrorRows = new List<StockImportErrorRow>();
                PreviewTable = new DataTable();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (_navInitialized)
                return;

            _navInitialized = true;
            BuildCategoryBrowseTreePanel();
            HookNavigationEvents();
            LoadCategoryTree();
            LoadLocationHierarchy();
            RefreshLocationColumnDataSource();
            ToggleCategoryBrowseVisibility();
        }

        private void HookNavigationEvents()
        {
            _btnSearchMode.Click += (s, e) => BeginInvoke(new Action(ToggleCategoryBrowseVisibility));

            _txtCategoryFilter.Enter += (s, e) =>
            {
                if (_txtCategoryFilter.ForeColor == Color.DimGray)
                {
                    _txtCategoryFilter.Text = string.Empty;
                    _txtCategoryFilter.ForeColor = Color.Black;
                }
            };
            _txtCategoryFilter.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(_txtCategoryFilter.Text))
                {
                    _txtCategoryFilter.ForeColor = Color.DimGray;
                    _txtCategoryFilter.Text = "Filter categories...";
                }
            };
            _txtCategoryFilter.TextChanged += (s, e) =>
            {
                if (_txtCategoryFilter.ForeColor != Color.DimGray)
                    FilterCategoryTree(_txtCategoryFilter.Text.Trim());
            };

            _categoryTree.BeforeExpand += CategoryTree_BeforeExpand;
            _categoryTree.AfterSelect += CategoryTree_AfterSelect;

            _gridAdjustment.CellBeginEdit += GridAdjustment_CellBeginEdit_LocationPicker;
            _gridAdjustment.CellMouseDown += GridAdjustment_CellMouseDown_SelectForContext;

            _gridLocationMenu = new ContextMenuStrip();
            _gridLocationMenu.Items.Add("Apply location to selected rows", null, (s, e) => ApplyLocationToSelectedRows());
            _gridAdjustment.ContextMenuStrip = _gridLocationMenu;

            EnsureBulkOperationsInitialized();
        }

        private void BuildCategoryBrowseTreePanel()
        {
            _categoryTreePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Visible = false
            };

            _txtCategoryFilter = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI", 9F),
                Text = "Filter categories...",
                ForeColor = Color.DimGray
            };

            _lblCategoryTreeInfo = new Label
            {
                Dock = DockStyle.Top,
                Height = 20,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.DimGray,
                Text = "Category tree ready"
            };

            _categoryTree = new TreeView
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                HideSelection = false,
                FullRowSelect = true
            };

            _categoryTreePanel.Controls.Add(_categoryTree);
            _categoryTreePanel.Controls.Add(_lblCategoryTreeInfo);
            _categoryTreePanel.Controls.Add(_txtCategoryFilter);

            _leftRoot.Controls.Add(_categoryTreePanel);
            _categoryTreePanel.BringToFront();
        }

        private void ToggleCategoryBrowseVisibility()
        {
            bool showTree = _searchMode == SearchMode.CategoryBrowse;
            _categoryTreePanel.Visible = showTree;
            _gridSearchResults.Visible = !showTree;

            if (showTree)
                _lblCategoryTreeInfo.Text = string.Format("{0} categories", _categoryMap.Count);
        }

        private void LoadCategoryTree()
        {
            var started = Environment.TickCount;
            _categoryMap.Clear();
            _categoryChildrenMap.Clear();

            DataTable categoryTable = new CategoriesBLL().GetAll();
            if (categoryTable == null || categoryTable.Rows.Count == 0)
                return;

            var productCategoryCounts = BuildProductCategoryCounts();

            foreach (DataRow row in categoryTable.Rows)
            {
                int id = ToInt(row, "id");
                if (id <= 0)
                    continue;

                int parentId = ToInt(row, "parent_id");
                string name = ToString(row, "name");
                string code = ToString(row, "code");

                var node = new CategoryNavNode
                {
                    Id = id,
                    ParentId = parentId,
                    Name = name,
                    Code = code,
                    BranchCount = 0,
                    HasChildren = false
                };

                _categoryMap[id] = node;
                if (!_categoryChildrenMap.ContainsKey(parentId))
                    _categoryChildrenMap[parentId] = new List<CategoryNavNode>();
                _categoryChildrenMap[parentId].Add(node);
            }

            foreach (var kv in _categoryChildrenMap)
            {
                foreach (var child in kv.Value)
                    child.HasChildren = _categoryChildrenMap.ContainsKey(child.Id);
            }

            foreach (var kv in _categoryMap)
                kv.Value.BranchCount = ComputeBranchProductCount(kv.Key, productCategoryCounts);

            BindCategoryRoots();

            int elapsed = Environment.TickCount - started;
            _lblCategoryTreeInfo.Text = string.Format("Loaded {0} categories in {1} ms", _categoryMap.Count, elapsed);
        }

        private Dictionary<string, int> BuildProductCategoryCounts()
        {
            var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            List<ProductSearchItem> snapshot;
            lock (_productIndexLock)
            {
                snapshot = _productIndex;
            }

            foreach (var item in snapshot)
            {
                string key = item.CategoryId ?? string.Empty;
                if (key.Length == 0)
                    continue;
                int count;
                map.TryGetValue(key, out count);
                map[key] = count + 1;
            }

            return map;
        }

        private int ComputeBranchProductCount(int categoryId, Dictionary<string, int> directCounts)
        {
            int total = 0;
            int direct;
            if (directCounts.TryGetValue(categoryId.ToString(), out direct))
                total += direct;

            List<CategoryNavNode> children;
            if (_categoryChildrenMap.TryGetValue(categoryId, out children))
            {
                for (int i = 0; i < children.Count; i++)
                    total += ComputeBranchProductCount(children[i].Id, directCounts);
            }

            return total;
        }

        private void BindCategoryRoots()
        {
            _categoryTree.BeginUpdate();
            _categoryTree.Nodes.Clear();

            List<CategoryNavNode> roots;
            if (!_categoryChildrenMap.TryGetValue(0, out roots))
                roots = _categoryMap.Values.Where(x => x.ParentId <= 0).ToList();

            for (int i = 0; i < roots.Count; i++)
                _categoryTree.Nodes.Add(CreateCategoryTreeNode(roots[i], addLazy: true));

            _categoryTree.EndUpdate();
        }

        private TreeNode CreateCategoryTreeNode(CategoryNavNode node, bool addLazy)
        {
            var treeNode = new TreeNode(string.Format("{0} ({1:N0})", node.Name, node.BranchCount)) { Tag = node.Id };
            if (addLazy && node.HasChildren)
                treeNode.Nodes.Add(new TreeNode(LazyCategoryNodeTag));
            return treeNode;
        }

        private void CategoryTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node == null || e.Node.Nodes.Count == 0 || e.Node.Nodes[0].Text != LazyCategoryNodeTag)
                return;

            e.Node.Nodes.Clear();
            int categoryId = Convert.ToInt32(e.Node.Tag);
            List<CategoryNavNode> children;
            if (_categoryChildrenMap.TryGetValue(categoryId, out children))
            {
                for (int i = 0; i < children.Count; i++)
                    e.Node.Nodes.Add(CreateCategoryTreeNode(children[i], addLazy: true));
            }
        }

        private void CategoryTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null || e.Node.Tag == null)
                return;

            int categoryId = Convert.ToInt32(e.Node.Tag);
            var branchIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            CollectCategoryBranch(categoryId, branchIds);
            _selectedCategoryBranchIds = branchIds;
            LoadProductsByCategoryBranch(branchIds);
        }

        private void CollectCategoryBranch(int categoryId, HashSet<string> collector)
        {
            collector.Add(categoryId.ToString());
            List<CategoryNavNode> children;
            if (_categoryChildrenMap.TryGetValue(categoryId, out children))
            {
                for (int i = 0; i < children.Count; i++)
                    CollectCategoryBranch(children[i].Id, collector);
            }
        }

        private void FilterCategoryTree(string term)
        {
            term = (term ?? string.Empty).Trim();
            if (term.Length == 0)
            {
                BindCategoryRoots();
                return;
            }

            var matched = _categoryMap.Values.Where(x => x.Name.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0).OrderBy(x => x.Name).ToList();

            _categoryTree.BeginUpdate();
            _categoryTree.Nodes.Clear();
            for (int i = 0; i < matched.Count; i++)
                _categoryTree.Nodes.Add(CreateCategoryTreeNode(matched[i], addLazy: false));
            _categoryTree.EndUpdate();

            _lblCategoryTreeInfo.Text = string.Format("{0} categories matched", matched.Count);
        }

        private void LoadProductsByCategoryBranch(HashSet<string> categoryIds)
        {
            List<ProductSearchItem> snapshot;
            lock (_productIndexLock)
            {
                snapshot = _productIndex;
            }

            var selectedProducts = snapshot
                .Where(x => categoryIds.Contains(x.CategoryId ?? string.Empty))
                .Take(50)
                .ToList();

            _currentResultPage.Clear();
            for (int i = 0; i < selectedProducts.Count; i++)
            {
                var p = selectedProducts[i];
                _currentResultPage.Add(new SearchProductRow
                {
                    Code = p.ProductCode,
                    Name = p.ProductName,
                    Qty = p.CurrentQty,
                    Price = p.SalePrice,
                    Location = p.LocationCode,
                    ReorderLevel = p.ReorderLevel
                });
            }

            _gridSearchResults.RowCount = _currentResultPage.Count;
            _gridSearchResults.Refresh();
            _lblResultInfo.Text = string.Format("Showing {0} products from selected category branch", _currentResultPage.Count);
        }

        private void LoadLocationHierarchy()
        {
            _locationNavData.Clear();
            DataTable locationTable = new LocationsBLL().GetAll();
            if (locationTable == null)
                return;

            var locationCount = BuildLocationProductCounts();

            foreach (DataRow row in locationTable.Rows)
            {
                string code = ToString(row, "code").ToUpperInvariant();
                if (string.IsNullOrWhiteSpace(code))
                    continue;

                string warehouse = ToString(row, "warehouse_code");
                if (string.IsNullOrWhiteSpace(warehouse))
                    warehouse = "Main Store";

                string aisle;
                string shelf;
                string bin;
                ParseLocationCode(code, out aisle, out shelf, out bin);

                int count;
                locationCount.TryGetValue(code, out count);

                _locationNavData.Add(new LocationNavNode
                {
                    Warehouse = warehouse,
                    Aisle = aisle,
                    Shelf = shelf,
                    Bin = bin,
                    Code = code,
                    ProductCount = count
                });
            }
        }

        private Dictionary<string, int> BuildLocationProductCounts()
        {
            var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            List<ProductSearchItem> snapshot;
            lock (_productIndexLock)
            {
                snapshot = _productIndex;
            }

            for (int i = 0; i < snapshot.Count; i++)
            {
                string code = (snapshot[i].LocationCode ?? string.Empty).ToUpperInvariant();
                if (code.Length == 0)
                    continue;
                int n;
                map.TryGetValue(code, out n);
                map[code] = n + 1;
            }

            return map;
        }

        private void RefreshLocationColumnDataSource()
        {
            var col = _gridAdjustment.Columns[ColNewLoc] as DataGridViewComboBoxColumn;
            if (col == null)
                return;

            col.DataSource = _locationNavData.Select(x => x.Code).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x).ToList();
        }

        private void GridAdjustment_CellBeginEdit_LocationPicker(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (_gridAdjustment.Columns[e.ColumnIndex].Name != ColNewLoc)
                return;

            e.Cancel = true;
            string current = e.RowIndex < _sessionRows.Count ? (_sessionRows[e.RowIndex].NewLocation ?? string.Empty) : string.Empty;

            string selected;
            if (ShowLocationPicker(current, out selected))
            {
                if (e.RowIndex < _sessionRows.Count)
                {
                    _sessionRows[e.RowIndex].NewLocation = selected;
                    _gridAdjustment.InvalidateRow(e.RowIndex);
                    RecalculateFooterSummary();
                }
            }
        }

        private void GridAdjustment_CellMouseDown_SelectForContext(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right || e.RowIndex < 0)
                return;

            _gridAdjustment.ClearSelection();
            _gridAdjustment.Rows[e.RowIndex].Selected = true;
            _gridAdjustment.CurrentCell = _gridAdjustment.Rows[e.RowIndex].Cells[Math.Max(0, e.ColumnIndex)];
        }

        private bool ShowLocationPicker(string initialCode, out string selectedCode)
        {
            selectedCode = initialCode ?? string.Empty;

            using (var frm = new Form())
            {
                frm.Text = "Select New Location";
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.FormBorderStyle = FormBorderStyle.FixedDialog;
                frm.MinimizeBox = false;
                frm.MaximizeBox = false;
                frm.Width = 500;
                frm.Height = 250;

                var cmbWh = new ComboBox { Left = 12, Top = 24, Width = 110, DropDownStyle = ComboBoxStyle.DropDownList };
                var cmbA = new ComboBox { Left = 130, Top = 24, Width = 90, DropDownStyle = ComboBoxStyle.DropDownList };
                var cmbS = new ComboBox { Left = 228, Top = 24, Width = 90, DropDownStyle = ComboBoxStyle.DropDownList };
                var cmbB = new ComboBox { Left = 326, Top = 24, Width = 90, DropDownStyle = ComboBoxStyle.DropDownList };
                var txtCode = new TextBox { Left = 12, Top = 66, Width = 260, Text = initialCode ?? string.Empty };
                var lblHint = new Label { Left = 12, Top = 98, Width = 460, Height = 36, ForeColor = Color.DimGray };
                var btnNew = new Button { Left = 278, Top = 64, Width = 138, Height = 26, Text = "New Location" };
                var btnOk = new Button { Text = "OK", Left = 318, Width = 72, Top = 152, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Cancel", Left = 398, Width = 72, Top = 152, DialogResult = DialogResult.Cancel };

                frm.Controls.Add(new Label { Left = 12, Top = 8, Width = 300, Text = "Warehouse > Aisle > Shelf > Bin" });
                frm.Controls.Add(cmbWh);
                frm.Controls.Add(cmbA);
                frm.Controls.Add(cmbS);
                frm.Controls.Add(cmbB);
                frm.Controls.Add(new Label { Left = 12, Top = 50, Width = 200, Text = "Location code" });
                frm.Controls.Add(txtCode);
                frm.Controls.Add(btnNew);
                frm.Controls.Add(lblHint);
                frm.Controls.Add(btnOk);
                frm.Controls.Add(btnCancel);
                frm.AcceptButton = btnOk;
                frm.CancelButton = btnCancel;

                Action bindWarehouses = () =>
                {
                    cmbWh.DataSource = _locationNavData.Select(x => x.Warehouse).Distinct().OrderBy(x => x).ToList();
                };
                Action bindAisles = () =>
                {
                    string wh = Convert.ToString(cmbWh.SelectedItem);
                    cmbA.DataSource = _locationNavData.Where(x => x.Warehouse == wh).Select(x => x.Aisle).Distinct().OrderBy(x => x).ToList();
                };
                Action bindShelves = () =>
                {
                    string wh = Convert.ToString(cmbWh.SelectedItem);
                    string a = Convert.ToString(cmbA.SelectedItem);
                    cmbS.DataSource = _locationNavData.Where(x => x.Warehouse == wh && x.Aisle == a).Select(x => x.Shelf).Distinct().OrderBy(x => x).ToList();
                };
                Action bindBins = () =>
                {
                    string wh = Convert.ToString(cmbWh.SelectedItem);
                    string a = Convert.ToString(cmbA.SelectedItem);
                    string s = Convert.ToString(cmbS.SelectedItem);
                    cmbB.DataSource = _locationNavData.Where(x => x.Warehouse == wh && x.Aisle == a && x.Shelf == s).Select(x => x.Bin).Distinct().OrderBy(x => x).ToList();
                };
                Action updateCodeAndHint = () =>
                {
                    string code = BuildLocationCode(Convert.ToString(cmbA.SelectedItem), Convert.ToString(cmbS.SelectedItem), Convert.ToString(cmbB.SelectedItem));
                    if (!string.IsNullOrWhiteSpace(code))
                        txtCode.Text = code;

                    string wh = Convert.ToString(cmbWh.SelectedItem);
                    string a = Convert.ToString(cmbA.SelectedItem);
                    string s = Convert.ToString(cmbS.SelectedItem);
                    string b = Convert.ToString(cmbB.SelectedItem);

                    int whCount = _locationNavData.Where(x => x.Warehouse == wh).Sum(x => x.ProductCount);
                    int aCount = _locationNavData.Where(x => x.Warehouse == wh && x.Aisle == a).Sum(x => x.ProductCount);
                    int sCount = _locationNavData.Where(x => x.Warehouse == wh && x.Aisle == a && x.Shelf == s).Sum(x => x.ProductCount);
                    int bCount = _locationNavData.Where(x => x.Warehouse == wh && x.Aisle == a && x.Shelf == s && x.Bin == b).Sum(x => x.ProductCount);
                    lblHint.Text = string.Format("Warehouse: {0:N0} | Aisle: {1:N0} | Shelf: {2:N0} | Bin: {3:N0} products", whCount, aCount, sCount, bCount);
                };

                cmbWh.SelectedIndexChanged += (s, e) => { bindAisles(); bindShelves(); bindBins(); updateCodeAndHint(); };
                cmbA.SelectedIndexChanged += (s, e) => { bindShelves(); bindBins(); updateCodeAndHint(); };
                cmbS.SelectedIndexChanged += (s, e) => { bindBins(); updateCodeAndHint(); };
                cmbB.SelectedIndexChanged += (s, e) => updateCodeAndHint();

                txtCode.TextChanged += (s, e) =>
                {
                    string a;
                    string sh;
                    string b;
                    if (TryParseLocationCode(txtCode.Text, out a, out sh, out b))
                    {
                        if (cmbA.Items.Contains(a)) cmbA.SelectedItem = a;
                        if (cmbS.Items.Contains(sh)) cmbS.SelectedItem = sh;
                        if (cmbB.Items.Contains(b)) cmbB.SelectedItem = b;
                    }
                };

                btnNew.Click += (s, e) =>
                {
                    string newCode = (txtCode.Text ?? string.Empty).Trim().ToUpperInvariant();
                    if (newCode.Length == 0)
                        return;

                    bool exists = _locationNavData.Any(x => string.Equals(x.Code, newCode, StringComparison.OrdinalIgnoreCase));
                    if (!exists)
                    {
                        var bll = new LocationsBLL();
                        bll.Insert(new LocationsModal { code = newCode, name = newCode });
                        LoadLocationHierarchy();
                        RefreshLocationColumnDataSource();
                    }
                };

                bindWarehouses();
                bindAisles();
                bindShelves();
                bindBins();
                updateCodeAndHint();

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    selectedCode = (txtCode.Text ?? string.Empty).Trim().ToUpperInvariant();
                    return selectedCode.Length > 0;
                }
            }

            return false;
        }

        private void ApplyLocationToSelectedRows()
        {
            if (_gridAdjustment.CurrentRow == null)
                return;

            int sourceIndex = _gridAdjustment.CurrentRow.Index;
            if (sourceIndex < 0 || sourceIndex >= _sessionRows.Count)
                return;

            string location = _sessionRows[sourceIndex].NewLocation ?? string.Empty;
            if (location.Length == 0)
                return;

            foreach (DataGridViewRow row in _gridAdjustment.SelectedRows)
            {
                if (row.Index >= 0 && row.Index < _sessionRows.Count)
                    _sessionRows[row.Index].NewLocation = location;
            }

            _gridAdjustment.Refresh();
        }

        private void EnsureBulkOperationsInitialized()
        {
            if (_bulkOpsInitialized || _bulkEditMenu == null)
                return;

            _bulkEditMenu.Items.Clear();
            _bulkEditMenu.Items.Add("Set Location", null, (s, e) => BulkSetLocation());
            _bulkEditMenu.Items.Add("Bulk Price Update...", null, (s, e) => ShowBulkPriceUpdateFlow());
            _bulkEditMenu.Items.Add("Mark as Verified", null, (s, e) => BulkMarkVerified());

            _bulkOpsInitialized = true;
        }

        private void ShowBulkPriceUpdateFlow()
        {
            EnsureBulkOperationsInitialized();

            if (_sessionRows == null || _sessionRows.Count == 0)
            {
                MessageBox.Show(this, "No products in session to update.", "Bulk Price Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BulkPriceRule rule;
            if (!ShowBulkPriceUpdateDialog(out rule) || rule == null)
                return;

            var targets = GetBulkPriceTargets(rule).ToList();
            if (targets.Count == 0)
            {
                MessageBox.Show(this, "No rows matched the selected scope.", "Bulk Price Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal oldTotal = 0m;
            decimal newTotal = 0m;

            for (int i = 0; i < targets.Count; i++)
            {
                var row = _sessionRows[targets[i]];
                oldTotal += row.NewPrice;

                decimal updatedPrice = CalculateBulkPrice(row.NewPrice, rule);
                row.NewPrice = updatedPrice;
                if (string.IsNullOrWhiteSpace(row.Reason))
                    row.Reason = "Price Correction";

                newTotal += row.NewPrice;
            }

            RefreshAdjustmentGrid();

            MessageBox.Show(
                this,
                string.Format("Updated {0} product prices.\nTotal before: {1:N2}\nTotal after: {2:N2}", targets.Count, oldTotal, newTotal),
                "Bulk Price Update",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private bool ShowBulkPriceUpdateDialog(out BulkPriceRule rule)
        {
            rule = null;

            using (var frm = new Form())
            {
                frm.Text = "Bulk Price Update";
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.FormBorderStyle = FormBorderStyle.FixedDialog;
                frm.MinimizeBox = false;
                frm.MaximizeBox = false;
                frm.Width = 430;
                frm.Height = 290;

                var lblType = new Label { Left = 12, Top = 14, Width = 150, Text = "Change type" };
                var cmbType = new ComboBox { Left = 12, Top = 34, Width = 190, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbType.Items.AddRange(new object[]
                {
                    "Increase by fixed amount",
                    "Increase by percent",
                    "Decrease by fixed amount",
                    "Decrease by percent",
                    "Set specific price"
                });
                cmbType.SelectedIndex = 1;

                var lblValue = new Label { Left = 220, Top = 14, Width = 180, Text = "Value" };
                var txtValue = new TextBox { Left = 220, Top = 34, Width = 180, Text = "0" };

                var lblRound = new Label { Left = 12, Top = 72, Width = 190, Text = "Round to nearest" };
                var cmbRound = new ComboBox { Left = 12, Top = 92, Width = 190, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbRound.Items.AddRange(new object[]
                {
                    "No rounding",
                    "0.05",
                    "0.10",
                    "0.25",
                    "0.50",
                    "1.00"
                });
                cmbRound.SelectedIndex = 0;

                var lblScope = new Label { Left = 220, Top = 72, Width = 180, Text = "Apply scope" };
                var cmbScope = new ComboBox { Left = 220, Top = 92, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbScope.Items.AddRange(new object[]
                {
                    "Selected rows",
                    "All rows in session",
                    "All rows in category"
                });
                cmbScope.SelectedIndex = 0;

                var lblCategory = new Label { Left = 12, Top = 132, Width = 190, Text = "Category (for category scope)" };
                var cmbCategory = new ComboBox { Left = 12, Top = 152, Width = 388, DropDownStyle = ComboBoxStyle.DropDownList, Enabled = false };
                var categories = _sessionRows.Select(x => x.Category ?? string.Empty).Where(x => x.Length > 0).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x).ToList();
                cmbCategory.Items.AddRange(categories.Cast<object>().ToArray());
                if (cmbCategory.Items.Count > 0)
                    cmbCategory.SelectedIndex = 0;

                cmbScope.SelectedIndexChanged += (s, e) =>
                {
                    cmbCategory.Enabled = cmbScope.SelectedIndex == 2;
                };

                var btnOk = new Button { Text = "Apply", Left = 246, Width = 72, Top = 198, DialogResult = DialogResult.OK };
                var btnCancel = new Button { Text = "Cancel", Left = 328, Width = 72, Top = 198, DialogResult = DialogResult.Cancel };

                frm.Controls.Add(lblType);
                frm.Controls.Add(cmbType);
                frm.Controls.Add(lblValue);
                frm.Controls.Add(txtValue);
                frm.Controls.Add(lblRound);
                frm.Controls.Add(cmbRound);
                frm.Controls.Add(lblScope);
                frm.Controls.Add(cmbScope);
                frm.Controls.Add(lblCategory);
                frm.Controls.Add(cmbCategory);
                frm.Controls.Add(btnOk);
                frm.Controls.Add(btnCancel);
                frm.AcceptButton = btnOk;
                frm.CancelButton = btnCancel;

                if (frm.ShowDialog(this) != DialogResult.OK)
                    return false;

                decimal value;
                if (!decimal.TryParse((txtValue.Text ?? string.Empty).Trim(), out value))
                {
                    MessageBox.Show(this, "Invalid value.", "Bulk Price Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string selectedCategory = cmbCategory.Enabled ? Convert.ToString(cmbCategory.SelectedItem) : string.Empty;

                rule = new BulkPriceRule
                {
                    ChangeType = (BulkPriceChangeType)cmbType.SelectedIndex,
                    Value = value,
                    RoundToNearest = GetRoundSetting(cmbRound.SelectedIndex),
                    ApplyScope = (BulkPriceApplyScope)cmbScope.SelectedIndex,
                    Category = selectedCategory ?? string.Empty
                };

                if (rule.ApplyScope == BulkPriceApplyScope.AllRowsInCategory && string.IsNullOrWhiteSpace(rule.Category))
                {
                    MessageBox.Show(this, "Please select a category.", "Bulk Price Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                return true;
            }
        }

        private IEnumerable<int> GetBulkPriceTargets(BulkPriceRule rule)
        {
            if (rule == null)
                return Enumerable.Empty<int>();

            if (rule.ApplyScope == BulkPriceApplyScope.AllRowsInSession)
                return Enumerable.Range(0, _sessionRows.Count);

            if (rule.ApplyScope == BulkPriceApplyScope.AllRowsInCategory)
            {
                return _sessionRows
                    .Select((x, i) => new { Row = x, Index = i })
                    .Where(x => string.Equals(x.Row.Category ?? string.Empty, rule.Category ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Index);
            }

            return _gridAdjustment.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(x => x.Index)
                .Where(i => i >= 0 && i < _sessionRows.Count)
                .Distinct()
                .OrderBy(i => i);
        }

        private decimal CalculateBulkPrice(decimal currentPrice, BulkPriceRule rule)
        {
            decimal result = currentPrice;

            switch (rule.ChangeType)
            {
                case BulkPriceChangeType.IncreaseFixed:
                    result = currentPrice + rule.Value;
                    break;
                case BulkPriceChangeType.IncreasePercent:
                    result = currentPrice + (currentPrice * rule.Value / 100m);
                    break;
                case BulkPriceChangeType.DecreaseFixed:
                    result = currentPrice - rule.Value;
                    break;
                case BulkPriceChangeType.DecreasePercent:
                    result = currentPrice - (currentPrice * rule.Value / 100m);
                    break;
                case BulkPriceChangeType.SetSpecific:
                    result = rule.Value;
                    break;
            }

            result = Math.Max(0m, result);

            if (rule.RoundToNearest.HasValue)
            {
                decimal step = rule.RoundToNearest.Value / 100m;
                if (step > 0m)
                    result = Math.Round(result / step, 0, MidpointRounding.AwayFromZero) * step;
            }

            return Math.Max(0m, result);
        }

        private static int? GetRoundSetting(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 1: return 5;
                case 2: return 10;
                case 3: return 25;
                case 4: return 50;
                case 5: return 100;
                default: return null;
            }
        }

        private static string BuildLocationCode(string aisle, string shelf, string bin)
        {
            aisle = (aisle ?? string.Empty).Trim().ToUpperInvariant();
            shelf = (shelf ?? string.Empty).Trim().ToUpperInvariant();
            bin = (bin ?? string.Empty).Trim().ToUpperInvariant();
            if (aisle.Length == 0 || shelf.Length == 0 || bin.Length == 0)
                return string.Empty;
            return string.Format("{0}-{1}-{2}", aisle, shelf, bin);
        }

        private static bool TryParseLocationCode(string code, out string aisle, out string shelf, out string bin)
        {
            aisle = string.Empty;
            shelf = string.Empty;
            bin = string.Empty;

            if (string.IsNullOrWhiteSpace(code))
                return false;

            string[] parts = code.Trim().ToUpperInvariant().Split('-');
            if (parts.Length != 3)
                return false;

            aisle = parts[0];
            shelf = parts[1];
            bin = parts[2];
            return true;
        }

        private static void ParseLocationCode(string code, out string aisle, out string shelf, out string bin)
        {
            if (!TryParseLocationCode(code, out aisle, out shelf, out bin))
            {
                aisle = string.Empty;
                shelf = string.Empty;
                bin = string.Empty;
            }
        }

        private static int ToInt(DataRow row, string column)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(column))
                return 0;
            int n;
            return int.TryParse(Convert.ToString(row[column]), out n) ? n : 0;
        }

        private static string ToString(DataRow row, string column)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(column) || row[column] == DBNull.Value)
                return string.Empty;
            return Convert.ToString(row[column]);
        }
    }
}
