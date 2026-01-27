using pos.UI;
using pos.UI.Busy;
using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_searchSaleProducts : Form
    {
        public string lang = (UsersModal.logged_in_lang.Length > 0 ? UsersModal.logged_in_lang : "en-US");
        private frm_sales mainForm;

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
        int _totalCount = 0;
        int _totalPages = 0;
        Label _lblPages; // runtime created label

        public frm_searchSaleProducts(frm_sales mainForm, string product_code, string category_code, string brand_code, bool isGrid = false, string group_code = "")
        {
            this.mainForm = mainForm;

            _product_code = product_code;

            _isGrid = isGrid;
            _category_code = category_code;
            _brand_code = brand_code;
            _group_code = group_code;

            InitializeComponent();

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
            txt_search.Text = _product_code;
            PerformPagedSearch();
            grid_search_products.Focus();
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
                        grid_search_products.DataSource = null;
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

                    grid_search_products.AutoGenerateColumns = false;
                    grid_search_products.DataSource = dt;
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
                            grid_search_products.DataSource = dt;
                            UpdatePagingLabel(sw.ElapsedMilliseconds, dt.Rows.Count);
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        if (grid_search_products.CurrentRow != null)
                        {
                            string productID = grid_search_products.CurrentRow.Cells["id"].Value?.ToString();
                            string item_number = grid_search_products.CurrentRow.Cells["item_number"].Value?.ToString();

                            int alternate_no = 0;
                            if (grid_search_products.CurrentRow.Cells["alternate_no"] != null)
                                int.TryParse(grid_search_products.CurrentRow.Cells["alternate_no"].Value?.ToString(), out alternate_no);

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
                string item_number = (grid_search_products.CurrentRow.Cells["item_number"].Value != null ? grid_search_products.CurrentRow.Cells["item_number"].Value.ToString() : "");

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
                string item_number = grid_search_products.CurrentRow.Cells["item_number"].Value.ToString();
                frm_productsMovements frm_prod_move_obj = new frm_productsMovements(item_number);

                frm_prod_move_obj.ShowDialog();
            }
        }

        private void frm_searchSaleProducts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.H)
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

        private void grid_search_products_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (grid_search_products.Focused && grid_search_products.RowCount > 0)
                {
                    string productID = grid_search_products.CurrentRow.Cells["id"].Value.ToString();
                    int alternate_no = (grid_search_products.CurrentRow.Cells["alternate_no"].Value != null ? Convert.ToInt32(grid_search_products.CurrentRow.Cells["alternate_no"].Value) : 0);
                    string item_number = (grid_search_products.CurrentRow.Cells["item_number"].Value != null ? grid_search_products.CurrentRow.Cells["item_number"].Value.ToString() : "");
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
                grid_other_stock.DataSource = null;
                grid_other_stock.Rows.Clear();

                DataTable dt = objBLL.Get_otherStock(productID, ProductNumber);
                foreach (DataRow myProductView in dt.Rows)
                {
                    string compnay_name = myProductView["branch_name"].ToString();
                    string qty = myProductView["qty"].ToString();

                    string[] row0 = { compnay_name, qty };

                    grid_other_stock.Rows.Add(row0);
                }
            }
        }
    }
}
