using POS.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_searchSaleProducts : Form
    {
        private frm_sales mainForm;

        string _product_code = "";
        string _category_code = "";
        string _brand_code = "";
        string _group_code = "";
        bool _isGrid = false;
        public bool _returnStatus = false;

        public frm_searchSaleProducts(frm_sales mainForm, string product_code, string category_code, string brand_code, bool isGrid = false, string group_code = "")
        {
            this.mainForm = mainForm;

            _product_code = product_code;

            _isGrid = isGrid;
            _category_code = category_code;
            _brand_code = brand_code;
            _group_code = group_code;

            InitializeComponent();
        }

        public frm_searchSaleProducts()
        {
            InitializeComponent();

        }

        private void frm_searchSaleProducts_Load(object sender, EventArgs e)
        {
            txt_search.Text = _product_code;
            load_Products_grid();
            grid_search_products.Focus();
        }

        private void grid_search_products_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok.PerformClick();
            }
        }

        public void load_Products_grid()
        {
            try
            {
                grid_search_products.DataSource = null;

                // set it to false if not needed for fast load
                grid_search_products.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                // or even better, use .DisableResizing. Most time consuming enum is DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
                grid_search_products.RowHeadersVisible = false;

                //bind data in data grid view  
                //ProductBLL objBLL = new ProductBLL();
                grid_search_products.AutoGenerateColumns = false;

                String condition = txt_search.Text.Trim();

                if (condition != "")
                {
                    //Call search first time
                    grid_search_products.DataSource = ProductBLL.SearchProductByBrandAndCategory(condition, _category_code, _brand_code, _group_code);
                    //grid_search_products.DataSource = ProductBLL.GetAllProducts(condition);

                    if (grid_search_products.Rows.Count <= 0)
                    {
                        DialogResult result = MessageBox.Show("Product not found, want to create new product?", "Sale Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                        if (result == DialogResult.Yes)
                        {
                            //var keyword = "";
                            //int numericValue;
                            // bool isNumber = int.TryParse(condition, out numericValue);
                            //if (isNumber)
                            // {
                            //    keyword = numericValue.ToString();
                            //}

                            frm_product_full_detail frm_products = new frm_product_full_detail(null, null, "", this, null, condition);
                            frm_products.ShowDialog();

                            //Call serach again after new product added
                            grid_search_products.DataSource = ProductBLL.SearchProductByBrandAndCategory(condition, _category_code, _brand_code, _group_code);
                            
                            ///
                        }
                        else
                        {
                            //this.Visible = false;
                            this.Close();
                        }
                    }
                    else
                    {
                        string productID = (grid_search_products.CurrentRow.Cells["id"].Value != null ? grid_search_products.CurrentRow.Cells["id"].Value.ToString() : "");
                        string product_code = (grid_search_products.CurrentRow.Cells["code"].Value != null ? grid_search_products.CurrentRow.Cells["code"].Value.ToString() : "");
                        int alternate_no = (grid_search_products.CurrentRow.Cells["alternate_no"].Value != null ? Convert.ToInt32(grid_search_products.CurrentRow.Cells["alternate_no"].Value) : 0);
                        load_alternate_product(alternate_no);//load alternate products
                        load_other_stock(productID,product_code);

                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (grid_search_products.SelectedCells.Count > 0)
            {
                string product_id = grid_search_products.CurrentRow.Cells["id"].Value.ToString();
                string code = grid_search_products.CurrentRow.Cells["code"].Value.ToString();

                if (_isGrid)
                {

                    mainForm.Load_products_to_grid(product_id);
                    _returnStatus = true;

                }
                else
                {
                    mainForm.load_products(code);
                }

                this.Visible = false;
            }
            else
            {
                MessageBox.Show("Please select record", "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                string product_code = grid_search_products.CurrentRow.Cells["code"].Value.ToString();
                frm_productsMovements frm_prod_move_obj = new frm_productsMovements(product_code);

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
        }

        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_search.Text != "")
                {
                    //grid_search_products.DataSource = null;
                    bool by_code = rb_by_code.Checked;
                    bool by_name = rb_by_name.Checked;


                    //bind data in data grid view  
                    ProductBLL objBLL = new ProductBLL();
                    grid_search_products.AutoGenerateColumns = false;

                    String condition = txt_search.Text.Trim();
                    grid_search_products.DataSource = objBLL.SearchRecord(condition, by_code, by_name);


                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void grid_group_products_DoubleClick(object sender, EventArgs e)
        {
            group_grid_select();//alternate / group products insert to grid
        }

        private void grid_group_products_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                group_grid_select();//alternate / group products insert to grid
            }
        }

        private void group_grid_select()//alternate / group products insert to grid
        {
            if (grid_group_products.SelectedCells.Count > 0)
            {
                string g_product_id = grid_group_products.CurrentRow.Cells["g_id"].Value.ToString();

                if (_isGrid)
                {

                    mainForm.Load_products_to_grid(g_product_id);
                    _returnStatus = true;

                }
                else
                {

                    mainForm.load_products(g_product_id);


                }


                this.Visible = false;
            }
            else
            {
                MessageBox.Show("Please select record", "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void grid_search_products_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (grid_search_products.Focused && grid_search_products.RowCount > 0)
                {
                    string productID = grid_search_products.CurrentRow.Cells["id"].Value.ToString();
                    string product_code = grid_search_products.CurrentRow.Cells["code"].Value.ToString();
                    int alternate_no = (grid_search_products.CurrentRow.Cells["alternate_no"].Value != null ? Convert.ToInt32(grid_search_products.CurrentRow.Cells["alternate_no"].Value) : 0);
                    load_alternate_product(alternate_no);
                    load_other_stock(productID, product_code);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void load_alternate_product(int alternate_no)
        {
            try
            {
                grid_group_products.Refresh();
                //grid_group_products.Rows.Clear();

                //bind data in data grid view  
                ProductBLL objBLL = new ProductBLL();
                grid_group_products.AutoGenerateColumns = false;

                // set it to false if not needed for fast load
                grid_group_products.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                // or even better, use .DisableResizing. Most time consuming enum is DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
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

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void grid_search_products_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            System.Collections.IList list = grid_search_products.Rows;
            for (int i = 0; i < list.Count; i++)
            {
                DataGridViewRow gvr = (DataGridViewRow)list[i];
                if (Convert.ToDouble(gvr.Cells["qty"].Value.ToString()) <= 0 || gvr.Cells["qty"].Value.ToString() == string.Empty)
                {
                    gvr.DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    gvr.DefaultCellStyle.ForeColor = Color.Black;
                }
            }

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
        public void load_other_stock(string productID, string productCode)
        {
            if(grid_search_products.RowCount > 0)
            {
                ProductBLL objBLL = new ProductBLL();
                grid_other_stock.DataSource = null;
                grid_other_stock.Rows.Clear();

                DataTable dt = objBLL.Get_otherStock(productID, productCode);
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
