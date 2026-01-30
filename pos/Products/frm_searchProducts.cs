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
                return;

            // Prefer the existing behavior used in load_Products_grid (brand/category)
            var dt = await SearchProductsAsync(condition);
            grid_search_products.DataSource = dt;
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
                return;
            }

            bool by_code = rb_by_code.Checked;
            bool by_name = rb_by_name.Checked;

            // cache key includes mode
            string cacheKey = string.Format("{0}|c:{1}|n:{2}|cat:{3}|brand:{4}", condition, by_code, by_name, _category_id, _brand_id);

            if (_searchCache.TryGetValue(cacheKey, out var cached))
            {
                grid_search_products.DataSource = cached;
                return;
            }

            using (BusyScope.Show(this, UiMessages.T("Searching products...", "جاري البحث عن الأصناف...")))
            {
                try
                {
                    var dt = await SearchProductsAsync(condition, by_code, by_name);
                    _searchCache[cacheKey] = dt;
                    grid_search_products.DataSource = dt;
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
                frm_productsMovements frm_prod_move_obj = new frm_productsMovements(item_number);

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
        }

    }
}