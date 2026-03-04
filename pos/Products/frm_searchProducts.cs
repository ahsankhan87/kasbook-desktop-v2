using POS.BLL;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pos.UI.Busy;
using pos.UI;

namespace pos
{
    public partial class frm_searchProducts : Form
    {
        private frm_sales mainForm;
        private frm_assign_products assign_product_frm;
        private frm_alt_products frm_alt_products;
        private frm_products_labels frm_pro_labels;
        private frm_product_full_detail frm_product_detail;
        private frm_product_adjustment frm_pro_adjmt;

        string _product_code = "";
        int _grid_row_index = 0;
        string _category_id = "";
        string _brand_id = "";
        bool _isGrid = false;
        bool _source_product = false;

        public bool _returnStatus = false;

        private readonly Timer _debounceTimer = new Timer();
        private const int DebounceMs = 250;
        private string _pendingSearchText = string.Empty;
        private Dictionary<string, DataTable> _searchCache = new Dictionary<string, DataTable>(StringComparer.OrdinalIgnoreCase);
        private int _pageIndex = 0;
        private const int _pageSize = 100;
        private int _totalCount = 0;
        private int _totalPages = 0;

        public frm_searchProducts(frm_sales mainForm, frm_assign_products assign_product_frm, frm_alt_products frm_alt_products,
            string product_code, string category_id, string brand_id, int rowIndex = 0, bool isGrid = false, bool source_product = false,
            frm_products_labels frm_pro_labels = null, frm_product_full_detail frm_pro_detail = null, frm_product_adjustment frm_pro_adjmt = null)
        {
            this.mainForm = mainForm;
            this.assign_product_frm = assign_product_frm;
            this.frm_alt_products = frm_alt_products;
            this.frm_pro_labels = frm_pro_labels;
            this.frm_product_detail = frm_pro_detail;
            this.frm_pro_adjmt = frm_pro_adjmt;

            _product_code = product_code;
            _grid_row_index = rowIndex;
            _isGrid = isGrid;
            _source_product = source_product;
            _category_id = category_id;
            _brand_id = brand_id;

            InitializeComponent();

            _debounceTimer.Interval = DebounceMs;
            _debounceTimer.Tick += DebounceTimer_Tick;
        }

        public frm_searchProducts()
        {
            InitializeComponent();

            _debounceTimer.Interval = DebounceMs;
            _debounceTimer.Tick += DebounceTimer_Tick;
        }

        private async void frm_searchProducts_Load(object sender, EventArgs e)
        {
            txt_search.Text = _product_code;
            ConfigureGridLayout();

            using (BusyScope.Show(this, UiMessages.T("Loading products...", "جاري تحميل الأصناف...")))
            {
                await LoadProductsGridAsync();
            }

            grid_search_products.Focus();
        }

        private Task<DataTable> SearchProductsAsync(string condition)
        {
            // Run DB query off UI thread so BusyForm can repaint/animate.
            return Task.Run(() => ProductBLL.SearchProductByBrandAndCategory(condition, _category_id, _brand_id));
        }

        private Task<Tuple<DataTable, int>> SearchProductsPagedAsync(string condition, int pageIndex, int pageSize)
        {
            return Task.Run(() =>
            {
                int total;
                var bll = new ProductBLL();
                var dt = bll.SearchProductsPagedWithCount(condition, _category_id, _brand_id, "", pageIndex, pageSize, out total);
                return Tuple.Create(dt, total);
            });
        }

        private Task<DataTable> SearchProductsAsync(string condition, bool by_code, bool by_name)
        {
            return Task.Run(() =>
            {
                var objBLL = new ProductBLL();
                return objBLL.SearchRecord(condition, by_code, by_name);
            });
        }

        private async Task LoadProductsGridAsync()
        {
            grid_search_products.DataSource = null;
            grid_search_products.AutoGenerateColumns = false;

            string condition = (txt_search.Text ?? string.Empty).Trim();
            if (condition == string.Empty)
            {
                _totalCount = 0;
                _totalPages = 0;
                return;
            }

            var result = await SearchProductsPagedAsync(condition, _pageIndex, _pageSize);
            var dt = result.Item1;
            _totalCount = result.Item2;
            _totalPages = (_totalCount + _pageSize - 1) / _pageSize;

            if (_pageIndex >= _totalPages && _totalPages > 0)
            {
                _pageIndex = _totalPages - 1;
                result = await SearchProductsPagedAsync(condition, _pageIndex, _pageSize);
                dt = result.Item1;
                _totalCount = result.Item2;
                _totalPages = (_totalCount + _pageSize - 1) / _pageSize;
            }

            grid_search_products.DataSource = dt;
            UpdateTotalProductsLabel();
        }

        // Keep old method for callers, but route to async.
        public async void load_Products_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching products...", "جاري البحث عن الأصناف...")))
                {
                    await LoadProductsGridAsync();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (grid_search_products.SelectedCells.Count > 0)
            {
                string product_id = grid_search_products.CurrentRow.Cells["id"].Value.ToString();
                string code = grid_search_products.CurrentRow.Cells["code"].Value.ToString();
                string name = grid_search_products.CurrentRow.Cells["name"].Value.ToString();
                string item_number = grid_search_products.CurrentRow.Cells["item_number"].Value.ToString();
                int alternate_no = Convert.ToInt32(grid_search_products.CurrentRow.Cells["alternate_no"].Value);

                if (_isGrid)
                {
                    if (assign_product_frm == null)
                    {
                        mainForm.Load_products_to_grid(item_number);
                        _returnStatus = true;
                    }
                    else
                    {
                        assign_product_frm.load_products(item_number);
                        _returnStatus = true;
                    }
                }
                else
                {
                    if (mainForm != null)
                    {
                        mainForm.load_products(item_number);

                    }
                    else if (assign_product_frm != null)
                    {
                        assign_product_frm.load_products(item_number);

                    }
                    else if (frm_alt_products != null)
                    {
                        if (_source_product)
                        {
                            frm_alt_products.fill_product_txtbox(product_id, code, name, alternate_no);
                        }
                        else
                        {
                            frm_alt_products.load_products(item_number);
                        }

                    }
                    else if (frm_pro_labels != null)
                    {
                        frm_pro_labels.load_products(product_id);

                    }
                    else if (frm_product_detail != null)
                    {
                        frm_product_detail.load_product_detail(item_number);

                    }
                    else if (frm_pro_adjmt != null)
                    {
                        frm_pro_adjmt.Load_product_to_grid(item_number);

                    }
                }

                this.Close();
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a product.",
                    "يرجى اختيار صنف.",
                    captionEn: "Products",
                    captionAr: "الأصناف");
            }
        }

        private void grid_search_products_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                btn_ok.PerformClick();
            }
        }

        private async void DebounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();

            string condition = (_pendingSearchText ?? string.Empty).Trim();
            if (condition.Length == 0)
            {
                grid_search_products.DataSource = null;
                _totalCount = 0;
                _totalPages = 0;
                UpdateTotalProductsLabel();
                return;
            }

            string cacheKey = string.Format("{0}|p:{1}|ps:{2}|cat:{3}|brand:{4}", condition, _pageIndex, _pageSize, _category_id, _brand_id);

            if (_searchCache.TryGetValue(cacheKey, out var cached))
            {
                grid_search_products.DataSource = cached;
                UpdateTotalProductsLabel();
                return;
            }

            using (BusyScope.Show(this, UiMessages.T("Searching products...", "جاري البحث عن الأصناف...")))
            {
                try
                {
                    var result = await SearchProductsPagedAsync(condition, _pageIndex, _pageSize);
                    var dt = result.Item1;
                    _totalCount = result.Item2;
                    _totalPages = (_totalCount + _pageSize - 1) / _pageSize;
                    _searchCache[cacheKey] = dt;
                    grid_search_products.DataSource = dt;
                    UpdateTotalProductsLabel();
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
            }
        }

        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                _pendingSearchText = txt_search.Text;
                _pageIndex = 0;
                _debounceTimer.Stop();
                _debounceTimer.Start();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
                string product_name = grid_search_products.CurrentRow.Cells["name"].Value.ToString();
                frm_productsMovements frm_prod_move_obj = new frm_productsMovements(item_number,product_name);

                frm_prod_move_obj.ShowDialog();
            }
            else
            {
                UiMessages.ShowWarning(
                    "No product selected.",
                    "لم يتم اختيار صنف.",
                    captionEn: "Products",
                    captionAr: "الأصناف");
            }
        }

        private void frm_searchProducts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.H)
            {
                product_movement_check();
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                if (_pageIndex + 1 < _totalPages)
                {
                    _pageIndex++;
                    load_Products_grid();
                }
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                if (_pageIndex > 0)
                {
                    _pageIndex--;
                    load_Products_grid();
                }
            }
        }

        private void ConfigureGridLayout()
        {
            if (grid_search_products.Columns.Contains("name"))
                grid_search_products.Columns["name"].FillWeight = 220;
        }

        private void UpdateTotalProductsLabel()
        {
            int count = (grid_search_products.DataSource as DataTable)?.Rows.Count ?? grid_search_products.Rows.Count;
            lbl_totalCount.Text = UiMessages.T(
                "Products " + count + "/" + _totalCount + " (Page " + (_pageIndex + 1) + "/" + Math.Max(1, _totalPages) + ")",
                "الأصناف " + count + "/" + _totalCount + " (صفحة " + (_pageIndex + 1) + "/" + Math.Max(1, _totalPages) + ")");
        }

    }
}