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

namespace pos
{
    public partial class frm_searchPurchaseProducts : Form
    {
        private frm_purchases mainForm;
        private frm_purchases_order purchase_orderForm;
        
        string _product_code = "";
        int _grid_row_index = 0;
        string _category_id = "";
        string _brand_id = "";
        string _group_code = "";
        bool _isGrid = false;
        public bool _returnStatus = false;

        public frm_searchPurchaseProducts(frm_purchases mainForm,frm_purchases_order purchase_orderForm, string product_code, string category_id, string brand_id, int rowIndex = 0, bool isGrid = false,string group_code="")
        {
            this.mainForm = mainForm;
            this.purchase_orderForm = purchase_orderForm;

            _product_code = product_code;
            _grid_row_index = rowIndex;
            _isGrid = isGrid;
            _category_id = category_id;
            _brand_id = brand_id;
            _group_code = group_code;
            InitializeComponent();
        }

        public frm_searchPurchaseProducts()
        {
            InitializeComponent();
            
        }

        private void frm_searchPurchaseProducts_Load(object sender, EventArgs e)
        {
            txt_search.Text = _product_code;
            load_Products_grid();
            grid_search_products.Focus();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (grid_search_products.SelectedCells.Count > 0)
            {
                string product_id = grid_search_products.CurrentRow.Cells["id"].Value.ToString();
                string code = grid_search_products.CurrentRow.Cells["code"].Value.ToString();
                string item_number = grid_search_products.CurrentRow.Cells["item_number"].Value.ToString();


                if (_isGrid)
                {
                    if(purchase_orderForm == null)
                    {
                        mainForm.Load_products_to_grid(item_number, _grid_row_index);
                    }
                    else
                    {
                        purchase_orderForm.Load_products_to_grid(item_number, _grid_row_index);
                        _returnStatus = true;
                    }
                    
                }
                else
                {
                    if (purchase_orderForm == null)
                    {
                        mainForm.load_products(item_number);
                    }
                    else
                    {
                        purchase_orderForm.load_products(item_number);
                    }
                   
                }
                
                //this.Visible = false;
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("Please select record", "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
                grid_search_products.DataSource = ProductBLL.SearchProductByBrandAndCategory(condition, _category_id, _brand_id, _group_code);
                load_alternate_product();

                if (grid_search_products.Rows.Count <= 0)
                {
                    DialogResult result = MessageBox.Show("Product not found, want to create new product?", "Purchase Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Yes)
                    {
                        //var keyword = "";
                        //int numericValue;
                        //bool isNumber = int.TryParse(condition, out numericValue);
                        //if (isNumber)
                        //{
                        //    keyword = numericValue.ToString();
                        //}

                        frm_product_full_detail frm_products = new frm_product_full_detail(null, null, "", null, this, condition);
                        frm_products.ShowDialog();

                        //Call serach again after new product added
                        grid_search_products.DataSource = ProductBLL.SearchProductByBrandAndCategory(condition, _category_id, _brand_id, _group_code);
                        ///
                    }
                    else
                    {
                        //this.Visible = false;
                        this.Close();
                    }
                }
                    
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 

        }

      
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //this.Close();
            this.Visible = false;
        }

        private void grid_search_products_DoubleClick(object sender, EventArgs e)
        {
            btn_ok.PerformClick();
        }

        private void frm_searchPurchaseProducts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
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


        public void product_movement_check()
        {
            if(grid_search_products.RowCount > 0)
            {
                string product_code = grid_search_products.CurrentRow.Cells["code"].Value.ToString();
                frm_productsMovements frm_prod_move_obj = new frm_productsMovements(product_code);

                frm_prod_move_obj.ShowDialog();
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
            group_grid_insert();//alternate / group products insert to grid
        }

        private void grid_group_products_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                group_grid_insert();//alternate / group products insert to grid
            }
        }
        private void group_grid_insert()//alternate / group products insert to grid
        {
            if (grid_group_products.SelectedCells.Count > 0)
            {
                string g_product_id = grid_group_products.CurrentRow.Cells["g_id"].Value.ToString();

                if (_isGrid)
                {
                    if (purchase_orderForm == null)
                    {
                        mainForm.Load_products_to_grid(g_product_id, _grid_row_index);
                    }
                    else
                    {
                        purchase_orderForm.Load_products_to_grid(g_product_id, _grid_row_index);
                        _returnStatus = true;
                    }

                }
                else
                {
                    if (purchase_orderForm == null)
                    {
                        mainForm.load_products(g_product_id);
                    }
                    else
                    {
                        purchase_orderForm.load_products(g_product_id);
                    }

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
                if (grid_search_products.Focused)
                {
                    load_alternate_product();

                } 
                
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

       private void load_alternate_product()
       {
           try
           {
                grid_group_products.Refresh();
                //grid_group_products.Rows.Clear();
                // set it to false if not needed for fast load
                grid_group_products.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                // or even better, use .DisableResizing. Most time consuming enum is DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
                grid_group_products.RowHeadersVisible = false;


                //bind data in data grid view  
                ProductBLL objBLL = new ProductBLL();
                grid_group_products.AutoGenerateColumns = false;
                
               if (grid_search_products.Rows.Count > 0)
                {
                    int alternate_no = Convert.ToInt32(grid_search_products.CurrentRow.Cells["alternate_no"].Value);

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
    }
}
