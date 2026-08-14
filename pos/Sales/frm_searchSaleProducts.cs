using pos.UI;
using pos.UI.Busy;
using pos.Security.Authorization;
using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AppPermissions = pos.Security.Authorization.Permissions;

namespace pos
{
    public partial class frm_searchSaleProducts : Form
    {
        public string lang = (UsersModal.logged_in_lang.Length > 0 ? UsersModal.logged_in_lang : "en-US");
        private frm_sales mainForm;

        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        string _product_code = "";
        string _category_code = "";
        string _brand_code = "";
        string _group_code = "";
        bool _isGrid = false;

        public bool _returnStatus = false;

        int _pageIndex = 0;
        int _pageSize = 100; // adjust as needed
        Timer _debounceTimer;
        private ProductBLL _productBll = new ProductBLL();
        private DataTable _virtualProductPage;
        int _totalCount = 0;
        int _totalPages = 0;
        Label _lblPages; // runtime created label

        private readonly string[] _productGridColumns =
        {
            "id",
            "code",
            "name",
            "qty",
            "unit_price",
            "location_code",
            "category",
            "description",
            "group_code",
            "alternate_no",
            "item_number"
        };

        public frm_searchSaleProducts(frm_sales mainForm, string product_code, string category_code, string brand_code, bool isGrid = false, string group_code = "")
        {
            this.mainForm = mainForm;

            _product_code = product_code;

            _isGrid = isGrid;
            _category_code = category_code;
            _brand_code = brand_code;
            _group_code = group_code;

            InitializeComponent();

            grid_search_products.VirtualMode = true;
            grid_search_products.CellValueNeeded += grid_search_products_CellValueNeeded;

            _debounceTimer = new Timer();
            _debounceTimer.Interval = 300; // 300ms debounce
            _debounceTimer.Tick += (s, e) =>
            {
                _debounceTimer.Stop();
                _pageIndex = 0; // reset page when typing
                PerformPagedSearch();
            };
        }

        public frm_searchSaleProducts()
        {
            InitializeComponent();

            grid_search_products.VirtualMode = true;
            grid_search_products.CellValueNeeded += grid_search_products_CellValueNeeded;

            _debounceTimer = new Timer();
            _debounceTimer.Interval = 300;
            _debounceTimer.Tick += (s, e) =>
            {
                _debounceTimer.Stop();
                _pageIndex = 0;
                PerformPagedSearch();
            };
        }

        private void frm_searchSaleProducts_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            txt_search.Text = _product_code;
            PerformPagedSearch();
            grid_search_products.Focus();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(panelTop, null, panel1, grid_search_products);
            AppTheme.ApplyListFormStyleLightHeader(null, null, panel3, grid_group_products);
            AppTheme.ApplyListFormStyleLightHeader(null, null, panel4, grid_other_stock);
        }

        private void grid_search_products_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok.PerformClick();
            }
        }

        private void PerformPagedSearch()
        {
            using (BusyScope.Show(this, UiMessages.T("Searching...", "جارٍ البحث...")))
            {
                try
                {
                    string condition = txt_search.Text.Trim();
                    if (string.IsNullOrWhiteSpace(condition))
                    {
                        _virtualProductPage = null;
                        grid_search_products.RowCount = 0;
                        _totalCount = 0;
                        _totalPages = 0;
                        UpdatePagingLabel();
                        return;
                    }

                    var sw = Stopwatch.StartNew();

                    var dt = _productBll.SearchProductsPagedWithCount(
                        condition,
                        _category_code,
                        _brand_code,
                        _group_code,
                        _pageIndex,
                        _pageSize,
                        out _totalCount);

                    sw.Stop();
                    _totalPages = (_totalCount + _pageSize - 1) / _pageSize;

                    // clamp page index if past last page (can occur after data changes)
                    if (_pageIndex >= _totalPages && _totalPages > 0)
                    {
                        _pageIndex = _totalPages - 1;
                        dt = _productBll.SearchProductsPagedWithCount(
                            condition,
                            _category_code,
                            _brand_code,
                            _group_code,
                            _pageIndex,
                            _pageSize,
                            out _totalCount);
                    }

                    _virtualProductPage = dt;
                    grid_search_products.RowCount = dt.Rows.Count;
                    grid_search_products.Invalidate();
                    UpdatePagingLabel(sw.ElapsedMilliseconds, dt.Rows.Count);
                    this.Text = $"Products (Page {_pageIndex + 1}/{_totalPages})";

                    if (dt.Rows.Count == 0)
                    {
                        var result = UiMessages.ConfirmYesNo(
                            "Product not found, want to create new product?",
                            "لم يتم العثور على المنتج، هل تريد إنشاء منتج جديد؟",
                            captionEn: "Sale Transaction",
                            captionAr: "معاملة بيع");

                        if (result == DialogResult.Yes)
                        {
                            frm_product_full_detail frm_products = new frm_product_full_detail(null, null, "", this, null, condition);
                            frm_products.ShowDialog();
                            dt = _productBll.SearchProductsPagedWithCount(condition, _category_code, _brand_code, _group_code, _pageIndex, _pageSize, out _totalCount);
                            _totalPages = (_totalCount + _pageSize - 1) / _pageSize;
                            _virtualProductPage = dt;
                            grid_search_products.RowCount = dt.Rows.Count;
                            grid_search_products.Invalidate();
                            UpdatePagingLabel(sw.ElapsedMilliseconds, dt.Rows.Count);
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        var selectedRow = GetCurrentVirtualRow();
                        if (selectedRow != null)
                        {
                            string productID = selectedRow["id"]?.ToString();
                            string item_number = selectedRow["item_number"]?.ToString();

                            int alternate_no = 0;
                            if (selectedRow["alternate_no"] != DBNull.Value && selectedRow["alternate_no"] != null)
                                int.TryParse(selectedRow["alternate_no"].ToString(), out alternate_no);

                            load_alternate_product(alternate_no);

                            if (!string.IsNullOrEmpty(productID) && !string.IsNullOrEmpty(item_number))
                                load_other_stock(productID, item_number);
                        }
                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
                }
            }
        }

        private void UpdatePagingLabel(long elapsedMs = 0, int currentCount = 0)
        {
            if (lbl_pages1 == null) return;
            string timePart = elapsedMs > 0 ? $" | {elapsedMs} ms" : string.Empty;
            lbl_pages1.Text = $"Page {_pageIndex + 1} of {_totalPages} | Rows {currentCount}/{_totalCount}{timePart}";
        }

        public void load_Products_grid()
        {
            PerformPagedSearch();
        }

        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                _debounceTimer.Stop();
                _debounceTimer.Start();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (grid_search_products.SelectedCells.Count > 0)
            {
                var selectedRow = GetCurrentVirtualRow();
                string item_number = selectedRow != null && selectedRow["item_number"] != DBNull.Value && selectedRow["item_number"] != null
                    ? selectedRow["item_number"].ToString()
                    : "";

                if (_isGrid)
                {
                    mainForm.Load_products_to_grid(item_number);
                    _returnStatus = true;
                }
                else
                {
                    mainForm.load_products(item_number);
                }

                this.Visible = false;
            }
            else
            {
                UiMessages.ShowWarning("Please select record", "يرجى اختيار سجل", "Products", "المنتجات");
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void grid_search_products_DoubleClick(object sender, EventArgs e)
        {
            btn_ok.PerformClick();
        }

        public void product_movement_check()
        {
            if (grid_search_products.RowCount > 0)
            {
                var selectedRow = GetCurrentVirtualRow();
                if (selectedRow == null)
                    return;

                string item_number = selectedRow["item_number"] != DBNull.Value && selectedRow["item_number"] != null
                    ? selectedRow["item_number"].ToString()
                    : string.Empty;
                string code = selectedRow["code"] != DBNull.Value && selectedRow["code"] != null
                    ? selectedRow["code"].ToString()
                    : string.Empty;
                string product_name = selectedRow["name"] != DBNull.Value && selectedRow["name"] != null
                    ? selectedRow["name"].ToString()
                    : string.Empty;
                string display_name = !string.IsNullOrEmpty(code) ? $"{code} - {product_name}" : product_name;

                if (string.IsNullOrEmpty(item_number))
                { return; }

                frm_productsMovements frm_prod_move_obj = new frm_productsMovements(item_number, display_name);
                frm_prod_move_obj.ShowDialog();
            }
        }

        private void frm_searchSaleProducts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.H || e.KeyCode == Keys.F6)
            {
                product_movement_check();
            }
            if (e.KeyCode == Keys.Add || e.KeyCode == Keys.Oemplus)
            {
                grid_group_products.Focus();
            }
            if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
            {
                grid_search_products.Focus();
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                if (_pageIndex + 1 < _totalPages) { _pageIndex++; PerformPagedSearch(); }
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                if (_pageIndex > 0) { _pageIndex--; PerformPagedSearch(); }
            }
            else if (e.KeyCode == Keys.F5)
            {
                PerformPagedSearch();
            }
        }

        private void grid_group_products_DoubleClick(object sender, EventArgs e)
        {
            group_grid_select();
        }

        private void grid_group_products_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                group_grid_select();
            }
        }

        private void group_grid_select()
        {
            if (grid_group_products.SelectedCells.Count > 0)
            {
                string alt_item_number = grid_group_products.CurrentRow.Cells["alt_item_number"].Value.ToString();

                if (_isGrid)
                {
                    mainForm.Load_products_to_grid(alt_item_number);
                    _returnStatus = true;
                }
                else
                {
                    mainForm.load_products(alt_item_number);
                }

                this.Visible = false;
            }
            else
            {
                UiMessages.ShowWarning("Please select record", "يرجى اختيار سجل", "Products", "المنتجات");
            }
        }

        /*
        private bool CanSelectMainProduct()
        {
            if (_auth.HasPermission(_currentUser, AppPermissions.Sales_allowZeroQtySale))
                return true;

            if (grid_search_products.CurrentRow == null)
                return false;

            double qty;
            var qtyValue = Convert.ToString(grid_search_products.CurrentRow.Cells["qty"].Value);
            if (string.IsNullOrWhiteSpace(qtyValue) || !double.TryParse(qtyValue, out qty) || qty <= 0)
            {
                UiMessages.ShowWarning(
                    "Cannot add this product because available quantity is zero.",
                    "لا يمكن إضافة هذا الصنف لأن الكمية المتاحة صفر.",
                    "Out of Stock",
                    "نفاد الكمية");
                return false;
            }

            return true;
        }

        private bool CanSelectAlternateProduct()
        {
            if (_auth.HasPermission(_currentUser, AppPermissions.Sales_allowZeroQtySale))
                return true;

            if (grid_group_products.CurrentRow == null)
                return false;

            double qty;
            var qtyValue = Convert.ToString(grid_group_products.CurrentRow.Cells["g_qty"].Value);
            if (string.IsNullOrWhiteSpace(qtyValue) || !double.TryParse(qtyValue, out qty) || qty <= 0)
            {
                UiMessages.ShowWarning(
                    "Cannot add this product because available quantity is zero.",
                    "لا يمكن إضافة هذا الصنف لأن الكمية المتاحة صفر.",
                    "Out of Stock",
                    "نفاد الكمية");
                return false;
            }

            return true;
        }
        */

        private void grid_search_products_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (grid_search_products.Focused && grid_search_products.RowCount > 0)
                {
                    var selectedRow = GetCurrentVirtualRow();
                    if (selectedRow == null)
                        return;

                    string productID = selectedRow.Table.Columns.Contains("id") && selectedRow["id"] != DBNull.Value && selectedRow["id"] != null ? selectedRow["id"].ToString() : string.Empty;
                    int alternate_no = 0;
                    if (selectedRow.Table.Columns.Contains("alternate_no") && selectedRow["alternate_no"] != DBNull.Value && selectedRow["alternate_no"] != null)
                        int.TryParse(selectedRow["alternate_no"].ToString(), out alternate_no);
                    string item_number = selectedRow.Table.Columns.Contains("item_number") && selectedRow["item_number"] != DBNull.Value && selectedRow["item_number"] != null ? selectedRow["item_number"].ToString() : string.Empty;
                    load_alternate_product(alternate_no);
                    load_other_stock(productID, item_number);
                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void load_alternate_product(int alternate_no)
        {
            try
            {
                grid_group_products.Refresh();

                ProductBLL objBLL = new ProductBLL();
                grid_group_products.AutoGenerateColumns = false;

                grid_group_products.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                grid_group_products.RowHeadersVisible = false;

                if (grid_search_products.Rows.Count > 0)
                {
                    if (alternate_no != 0)
                    {
                        grid_group_products.DataSource = objBLL.GetProductsByAlternateNo(alternate_no);
                    }
                    else
                    {
                        grid_group_products.DataSource = null;
                    }
                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void grid_search_products_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                var row = grid_search_products.Rows[e.RowIndex];
                var qtyCell = row.Cells["qty"]?.Value;
                double qty;
                bool isZeroOrEmpty = qtyCell == null || string.IsNullOrWhiteSpace(qtyCell.ToString()) || !double.TryParse(qtyCell.ToString(), out qty) || qty <= 0;
                row.DefaultCellStyle.ForeColor = isZeroOrEmpty ? Color.Red : Color.Black;
            }
            catch { }
        }

        private void grid_search_products_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (_virtualProductPage == null || e.RowIndex < 0 || e.RowIndex >= _virtualProductPage.Rows.Count)
            {
                e.Value = null;
                return;
            }

            DataRow row = _virtualProductPage.Rows[e.RowIndex];
            string columnName = grid_search_products.Columns[e.ColumnIndex].Name;

            if (row.Table.Columns.Contains(columnName))
            {
                e.Value = row[columnName];
            }
            else
            {
                e.Value = null;
            }
        }

        private DataRow GetCurrentVirtualRow()
        {
            if (_virtualProductPage == null || grid_search_products.CurrentCell == null)
                return null;

            int rowIndex = grid_search_products.CurrentCell.RowIndex;
            if (rowIndex < 0 || rowIndex >= _virtualProductPage.Rows.Count)
                return null;

            return _virtualProductPage.Rows[rowIndex];
        }

        private void grid_group_products_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow gvr in grid_group_products.Rows)
            {
                if (Convert.ToDouble(gvr.Cells["g_qty"].Value.ToString()) <= 0 || gvr.Cells["g_qty"].Value.ToString() == string.Empty)
                {
                    gvr.DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    gvr.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }

        public void load_other_stock(string productID, string ProductNumber)
        {
            if (grid_search_products.RowCount > 0)
            {
                ProductBLL objBLL = new ProductBLL();
                DataTable dt = objBLL.Get_otherStock(productID, ProductNumber);

                grid_other_stock.AutoGenerateColumns = false;
                grid_other_stock.DataSource = dt;
            }
        }
    }
}
