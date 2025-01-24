using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace pos
{
    public partial class frm_sales_v1 : Form
    {
        public string lang = (UsersModal.logged_in_lang.Length > 0 ? UsersModal.logged_in_lang : "en-US");
        public int cash_account_id = 0;
        public int sales_account_id = 0;
        public int receivable_account_id = 0;
        public int tax_account_id = 0;
        public int sales_discount_acc_id = 0;
        public int inventory_acc_id = 0;
        public int purchases_acc_id = 0;

        public double employee_commission_percent = 0;
        public double user_commission_percent = 0;

        public double total_amount = 0;
        public double total_cost_amount = 0;
        public double total_tax = 0;
        public double total_discount = 0;
        public double total_sub_total = 0;
        string invoice_status = "";
        string product_code = "";
        
        public double cash_sales_amount_limit = 0;
        //public double cash_purchase_amount_limit= 0;
        public bool allow_credit_sales = false;
        //public bool allow_credit_purchase= false;

        private DataGridView brandsDataGridView = new DataGridView();
        private DataGridView salesDataGridView = new DataGridView();
        private DataGridView categoriesDataGridView = new DataGridView();
        private DataGridView groupsDataGridView = new DataGridView();

        DataTable allProduct_dt = new DataTable();
        ProductBLL productsBLL_obj = new ProductBLL();

        public frm_sales_v1()
        {
            InitializeComponent();
            
        }

        private void TextBoxOnClick(object sender, EventArgs eventArgs)
        {
            var txt_brands = (TextBox)sender;
            txt_brands.SelectAll();
            txt_brands.Focus();

            var txt_categories = (TextBox)sender;
            txt_categories.SelectAll();
            txt_categories.Focus();

            var txt_groups = (TextBox)sender;
            txt_groups.SelectAll();
            txt_groups.Focus();
        }

        private void frm_sales_v1_Load(object sender, EventArgs e)
        {
            grid_sales.Rows.Add();
            this.ActiveControl = grid_sales;
            grid_sales.CurrentCell = grid_sales.Rows[0].Cells["code"];
            //grid_sales.Focus();
            //grid_sales.BeginEdit(true);

            //btn_movements.Enabled = false;
            //autoCompleteProductCode();
            load_user_rights(UsersModal.logged_in_userid);
            Get_AccountID_From_Company();
            get_customers_dropdownlist();
            get_employees_dropdownlist();
            get_saletype_dropdownlist();
            if (lang == "en-US")
            {
                cmb_sale_type.SelectedValue = "Cash";
            }
            else if (lang == "ar-SA")
            {
                cmb_sale_type.SelectedIndex = 0;
            }
            
            //get_groups_dropdownlist();
            autoCompleteBrands();
            //Get_user_total_commission();

            //disable sorting in grid
            foreach (DataGridViewColumn column in grid_sales.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchSaleProducts frm_search_product_obj = new frm_searchSaleProducts();
            frm_search_product_obj.Show();

        }

        private void SetupSalesDataGridView()
        {
            var current_lang_code = System.Globalization.CultureInfo.CurrentCulture;
            salesDataGridView.ColumnCount = 7;
            salesDataGridView.Name = "salesDataGridView";
            if (lang == "en-US")
            {
                salesDataGridView.Location = new Point(300, 80);
                salesDataGridView.Size = new Size(700, 400);
            }
            else if (lang == "ar-SA")
            {
                salesDataGridView.Location = new Point(260, 205);
                salesDataGridView.Size = new Size(700, 500);
            }

            salesDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            salesDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            salesDataGridView.Columns[0].Name = "ID";
            salesDataGridView.Columns[1].Name = "Code";
            salesDataGridView.Columns[2].Name = "Name";
            salesDataGridView.Columns[3].Name = "Qty";
            salesDataGridView.Columns[4].Name = "Price";
            salesDataGridView.Columns[5].Name = "Category";
            salesDataGridView.Columns[6].Name = "Description";
            salesDataGridView.Columns[0].ReadOnly = true;
            salesDataGridView.Columns[1].ReadOnly = true;
            salesDataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            salesDataGridView.MultiSelect = false;
            salesDataGridView.AllowUserToAddRows = false;
            salesDataGridView.AllowUserToDeleteRows = false;

            salesDataGridView.RowHeadersVisible = false;
            //salesDataGridView.ColumnHeadersVisible = false;
            salesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            salesDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            salesDataGridView.AutoResizeColumns();

            salesDataGridView.CellClick += new DataGridViewCellEventHandler(salesDataGridView_CellClick);
            this.salesDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(salesDataGridView_KeyDown);

            this.Controls.Add(salesDataGridView);
            salesDataGridView.BringToFront();

        }

        //private void txt_brands_Leave(object sender, EventArgs e)
        //{
        //    if (!salesDataGridView.Focused)
        //    {
        //        this.Controls.Remove(salesDataGridView);
        //    }
        //}

        Form frm_searchSaleProducts_obj;
        private void grid_sales_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                //int iColumn = grid_sales.CurrentCell.ColumnIndex;
                //int iRow = grid_sales.CurrentCell.RowIndex; 
                string columnName = grid_sales.Columns[e.ColumnIndex].Name;
                //if (columnName == "code")
                //{
                //    product_code = (grid_sales.CurrentRow.Cells["code"].Value != null ? grid_sales.CurrentRow.Cells["code"].Value.ToString() : "");
                //    var brand_code = txt_brand_code.Text; // (cmb_brands.SelectedValue != null ? cmb_brands.SelectedValue.ToString() : "");
                //    var category_code = txt_category_code.Text; // (cmb_categories.SelectedValue != null ? cmb_categories.SelectedValue.ToString() : "");
                //    var group_code = txt_group_code.Text; // (cmb_groups.SelectedValue != null ? cmb_groups.SelectedValue.ToString() : "");

                //            SetupSalesDataGridView();

                //            ProductBLL objBLL = new ProductBLL();
                //            string brand_name = txt_brands.Text;

                //            DataTable dt = objBLL.SearchProductByBrandAndCategory(product_code, category_code, brand_code, group_code);

                //            if (dt.Rows.Count > 0)
                //            {
                //                salesDataGridView.Rows.Clear();
                //                foreach (DataRow dr in dt.Rows)
                //                {
                //                    string id = dr["id"].ToString();
                //                    string code = dr["code"].ToString();
                //                    string name = dr["name"].ToString();
                //                    string qty = dr["qty"].ToString();
                //                    string unit_price = dr["unit_price"].ToString();
                //                    string category = dr["category"].ToString();
                //                    string description = dr["description"].ToString();

                //                    string[] row0 = { id, code, name, qty, unit_price, category, description };

                //                    salesDataGridView.Rows.Add(row0);
                //                }
                //                //salesDataGridView.CurrentCell = salesDataGridView.Rows[0].Cells[0];
                //                salesDataGridView.ClearSelection();
                //                salesDataGridView.CurrentCell = null;
                //            }

                //}

                double tax_rate = (grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value == null || grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                double sub_total = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value) * Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["qty"].Value);
                double tax = (sub_total * tax_rate / 100);
                
                grid_sales.Rows[e.RowIndex].Cells["tax"].Value = tax;
                grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total + tax;
                grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = sub_total;

                if(columnName == "packing")
                {
                    grid_sales.Rows[e.RowIndex].Cells["qty"].Value = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["packing"].Value) * Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["packet_qty"].Value);
                }

                if (columnName == "code")//if discount is changed
                {
                    
                    if(salesDataGridView.Rows.Count <= 0)
                    {
                        this.Controls.Remove(salesDataGridView);
                    }
                    else
                    {
                        //Load_products_to_grid(product_id);
                        salesDataGridView.Focus();
                    }
                    
                }

                if (columnName == "discount")//if discount is changed
                {
                    double total_value = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value) * Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["qty"].Value);
                    double discount_percent = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value) / total_value * 100;

                    double tax_1 = ((total_value - Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value)) * tax_rate / 100);

                    double sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value);
                    //total_discount += Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value);
                    //txt_total_discount.Text = total_amount.ToString();

                    grid_sales.Rows[e.RowIndex].Cells["discount_percent"].Value = discount_percent.ToString("0.00");
                    grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total_1.ToString("0.00");
                    grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = (sub_total_1 - tax_1).ToString("0.00");
                    grid_sales.Rows[e.RowIndex].Cells["tax"].Value = (tax_1).ToString("0.00");


                }
                if (columnName == "discount_percent")//if discount is changed
                {
                    double total_value = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value) * Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["qty"].Value);
                    double discount_value = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount_percent"].Value) * total_value / 100;

                    //double tax_rate = (grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                    double tax_1 = ((total_value - discount_value) * tax_rate / 100);
                    //grid_sales.Rows[e.RowIndex].Cells["tax"].Value = (tax * Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["qty"].Value)).ToString("0.00");

                    double sub_total_1 = (tax_1 + total_value - discount_value);
                    //total_discount += Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value);
                    //txt_total_discount.Text = total_amount.ToString();

                    grid_sales.Rows[e.RowIndex].Cells["discount"].Value = discount_value.ToString("0.00");
                    grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total_1.ToString("0.00");
                    grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                    grid_sales.Rows[e.RowIndex].Cells["tax"].Value = (tax_1).ToString("0.00");
                }

                get_total_tax();
                get_total_discount();
                get_sub_total_amount();
                get_total_cost_amount();
                get_total_amount();
                get_total_qty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }

        }

        void frm_searchSaleProducts_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            //frm_searchSaleProducts_obj = null;
        }

        void salesDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                string product_code = salesDataGridView.CurrentRow.Cells["code"].Value.ToString();
                //txt_brands.Text = salesDataGridView.CurrentRow.Cells[1].Value.ToString();
                if(product_code != null)
                {
                    if(!string.IsNullOrEmpty(product_code))
                    {
                        Load_products_to_grid(product_code);
                    }
          
                }
                
                this.Controls.Remove(salesDataGridView);
                grid_sales.Focus();
            }
        }

        private void salesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string product_code = salesDataGridView.CurrentRow.Cells["code"].Value.ToString();
            //txt_brands.Text = salesDataGridView.CurrentRow.Cells[1].Value.ToString();
            if (product_code != null)
            {
                if(!string.IsNullOrEmpty(product_code))
                {
                    Load_products_to_grid(product_code);
                }
            }
            this.Controls.Remove(salesDataGridView);
            grid_sales.Focus();

        }

        public void Load_products_to_grid(string product_code)
        {
            
            DataTable product_dt = new DataTable();
        
            product_dt = productsBLL_obj.SearchRecordByProductCode(product_code);

            int RowIndex = grid_sales.CurrentCell.RowIndex;

            if (product_dt.Rows.Count > 0)
            {
                //for (int i = 0; i < grid_sales.RowCount; i++)
                //{
                //    var item_id = (grid_sales.Rows[i].Cells["id"].Value != null ? grid_sales.Rows[i].Cells["id"].Value : "");
                //    if (item_id.ToString() == product_id)
                //    {
                //        MessageBox.Show("Product already added", "Already exist", MessageBoxButtons.OK, MessageBoxIcon.Question);
                //        grid_sales.CurrentCell = grid_sales.Rows[RowIndex].Cells["code"]; //make qty cell active
                //        //grid_sales.CurrentCell.Selected = true;
                //        grid_sales.BeginEdit(true);
                //        return;
                //    }
                //    else
                //    {
                //        grid_sales.CurrentCell = grid_sales.Rows[RowIndex].Cells["qty"]; //make qty cell active
                //        grid_sales.CurrentCell.Selected = true;
                            
                //    }
                //}

                foreach (DataRow myProductView in product_dt.Rows)
                {

                    double qty = Convert.ToDouble(myProductView["sale_demand_qty"].ToString() == string.Empty || (decimal)myProductView["sale_demand_qty"] == 0 ? "1" : myProductView["sale_demand_qty"].ToString());
                    double total = qty * double.Parse(myProductView["unit_price"].ToString());
                    double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                    double tax = (total * tax_rate / 100);
                    double sub_total = tax + total;
                    double sub_total_without_vat = total;

                    grid_sales.Rows[RowIndex].Cells["id"].Value = myProductView["id"].ToString();
                    grid_sales.Rows[RowIndex].Cells["code"].Value = myProductView["code"].ToString();
                    grid_sales.Rows[RowIndex].Cells["name"].Value = myProductView["name"].ToString();
                    grid_sales.Rows[RowIndex].Cells["packing"].Value = "0";
                    grid_sales.Rows[RowIndex].Cells["qty"].Value = qty;
                    grid_sales.Rows[RowIndex].Cells["cost_price"].Value = myProductView["avg_cost"].ToString();
                    grid_sales.Rows[RowIndex].Cells["unit_price"].Value = myProductView["unit_price"].ToString();
                    grid_sales.Rows[RowIndex].Cells["discount"].Value = 0.00;
                    grid_sales.Rows[RowIndex].Cells["discount_percent"].Value = 0.00;
                    grid_sales.Rows[RowIndex].Cells["tax"].Value = tax;
                    grid_sales.Rows[RowIndex].Cells["sub_total"].Value = sub_total;
                    grid_sales.Rows[RowIndex].Cells["total_without_vat"].Value = sub_total_without_vat;
                    grid_sales.Rows[RowIndex].Cells["location_code"].Value = myProductView["location_code"].ToString();
                    grid_sales.Rows[RowIndex].Cells["unit"].Value = myProductView["unit"].ToString();
                    grid_sales.Rows[RowIndex].Cells["category"].Value = myProductView["category"].ToString();
                    grid_sales.Rows[RowIndex].Cells["category_code"].Value = myProductView["category_code"].ToString();
                    grid_sales.Rows[RowIndex].Cells["btn_delete"].Value = "Del";

                    grid_sales.Rows[RowIndex].Cells["tax_id"].Value = myProductView["tax_id"].ToString();
                    grid_sales.Rows[RowIndex].Cells["tax_rate"].Value = myProductView["tax_rate"].ToString();
                    grid_sales.Rows[RowIndex].Cells["item_type"].Value = myProductView["item_type"].ToString();

                    grid_sales.Rows[RowIndex].Cells["shop_qty"].Value = myProductView["qty"].ToString();
                    grid_sales.Rows[RowIndex].Cells["packet_qty"].Value = myProductView["packet_qty"].ToString();
                    
                    /////
                    //fill_locations_grid_combo(RowIndex, "", myProductView["id"].ToString());
                    ////

                    if (Convert.ToDouble(myProductView["qty"]) <= 0 || myProductView["qty"].ToString() == string.Empty)
                    {
                        grid_sales.Rows[RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    
                }
                get_total_tax();
                get_total_discount();
                get_sub_total_amount();
                get_total_cost_amount();
                get_total_amount();
                get_total_qty();
            }

        }

        public void fill_locations_grid_combo(int RowIndex, string SelectedValue = "DEF",string product_id = "")
        {
            DataTable dt = new DataTable();
            var locationComboCell = new DataGridViewComboBoxCell();
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "loc_code as location_code";
            string table = "pos_product_stocks WHERE item_id=" + product_id + " AND  qty > 0 GROUP BY loc_code";

            dt = generalBLL_obj.GetRecord(keyword, table);

            //WHEN NO LOCATION ASSIGNED TO PRODUCT THEN ALL LOCATIONS SHALL BE LOADED
            if(dt.Rows.Count <= 0)
            {
                string keyword1 = "L.code as location_code,L.name";
                string table1 = "pos_locations L";

                dt = generalBLL_obj.GetRecord(keyword1, table1);

            }
            ///////////

            locationComboCell.DataSource = dt;
            locationComboCell.DisplayMember = "location_code";
            locationComboCell.ValueMember = "location_code";

            grid_sales.Rows[RowIndex].Cells["location_code"] = locationComboCell;
            //grid_sales.Rows[RowIndex].Cells["location_code"].Value = SelectedValue;
            grid_sales.Rows[RowIndex].Cells["location_code"].Value = dt.Rows[0]["location_code"].ToString(); // GET FIRST COLUMN OF DT TO SHOW FIRST VALUE AS SELECTED
            
                        
        }

        private void txt_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txt_barcode.Text != string.Empty && e.KeyCode == Keys.Enter)
            {
                load_products("", "", txt_barcode.Text.Trim());
                txt_barcode.Text = "";

            }
            txt_barcode.Focus();
        }

        public void load_products(string product_code = "", string product_name = "", string barcode = "")
        {

            DataTable product_dt = new DataTable();

            if (product_code != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByProductCode(product_code);
            }

            if (product_name != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByProductName(product_name);
            }

            if (barcode != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByBarcode(barcode);
            }

            if (product_dt.Rows.Count > 0)
            {
                
                foreach (DataRow myProductView in product_dt.Rows)
                {
                    double qty = Convert.ToDouble(myProductView["sale_demand_qty"].ToString() == string.Empty || (decimal)myProductView["sale_demand_qty"] == 0 ? "1" : myProductView["sale_demand_qty"].ToString());
                    double total = qty * double.Parse(myProductView["unit_price"].ToString());
                    double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                    double tax = (total * tax_rate / 100);
                    double sub_total = tax + total;
                    double sub_total_without_vat = total;

                    int id = Convert.ToInt32(myProductView["id"]);
                    string code = myProductView["code"].ToString();
                    string name = myProductView["name"].ToString();
                    double cost_price = Convert.ToDouble(myProductView["avg_cost"]);
                    double unit_price = Convert.ToDouble(myProductView["unit_price"]);
                    double discount = 0.00;
                    double discount_percent = 0.00;
                    string location_code = ""; // myProductView["location_code"].ToString();
                    string unit = myProductView["unit"].ToString();
                    string category = myProductView["category"].ToString();
                    string btn_delete = "Del";

                    string shop_qty = myProductView["qty"].ToString();
                    string tax_id = myProductView["tax_id"].ToString();
                    string item_type = myProductView["item_type"].ToString();
                    string category_code = myProductView["category_code"].ToString();

                    //double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : Convert.ToDouble(myProductView["tax_rate"]));
                    //double tax = (Convert.ToDouble(qty) * unit_price * tax_rate / 100);

                    double current_sub_total = Convert.ToDouble(qty) * unit_price + tax;

                    string[] row0 = { id.ToString(), code, name, qty.ToString(), unit_price.ToString(), discount.ToString(), discount_percent.ToString(),
                                            tax.ToString(), current_sub_total.ToString(),location_code,unit,category,sub_total_without_vat.ToString(),
                                            btn_delete, shop_qty,tax_id.ToString(), tax_rate.ToString(), cost_price.ToString(),
                                            item_type,category_code};

                    int RowIndex = grid_sales.Rows.Add(row0);

                    if (Convert.ToDouble(myProductView["qty"]) <= 0 || myProductView["qty"].ToString() == string.Empty)
                    {
                        grid_sales.CurrentRow.DefaultCellStyle.ForeColor = Color.Red;
                    }

                    //GET / SET Location Dropdown list
                    /////
                    fill_locations_grid_combo(RowIndex,"",myProductView["id"].ToString());
                    //////////

                    get_total_tax();
                    get_total_qty();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_cost_amount();
                    get_total_amount();

                }

                txt_barcode.Focus();

            }
            else
            {
                MessageBox.Show("Record not found", "Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void get_sub_total_amount()
        {
            total_sub_total = 0;

            for (int i = 0; i <= grid_sales.Rows.Count-1; i++ )
            {
                total_sub_total += Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value);
            }

            txt_sub_total.Text = (total_sub_total).ToString("0.00");
            //txt_sub_total_2.Text = (total_sub_total - total_discount).ToString("0.00");
        }
        
        private void get_total_cost_amount()
        {
            total_cost_amount = 0;

            for (int i = 0; i <= grid_sales.Rows.Count-1; i++)
            {
                total_cost_amount += Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["cost_price"].Value);
            }
            
        }

        private void get_total_amount()
        {
            total_amount = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_amount += Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value);
            }
            double net = (total_amount + total_tax-total_discount);
            txt_total_amount.Text = net.ToString("0.00");
            
           
            
        }

        private void get_total_tax()
        {
            total_tax = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_tax += Convert.ToDouble(grid_sales.Rows[i].Cells["tax"].Value);
            }

            txt_total_tax.Text = total_tax.ToString("0.00");
        }

        private void get_total_discount()
        {
            total_discount = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_discount += Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);
            }

            txt_total_discount.Text = total_discount.ToString("0.00");

           
        }

        private void get_total_qty()
        {
            double total_qty = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_qty += Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value);
            }

            txt_total_qty.Text = (total_qty).ToString();
        }
        
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                string sale_type = (string.IsNullOrEmpty(cmb_sale_type.SelectedValue.ToString()) ? "Cash" : cmb_sale_type.SelectedValue.ToString());
                
                DialogResult result = MessageBox.Show("Are you sure you want to sale", "Sale Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {

                    if (grid_sales.Rows.Count > 0)
                    {
                        List<SalesModalHeader> sales_model_header = new List<SalesModalHeader> { };
                        List<SalesModal> sales_model_detail = new List<SalesModal> { };
                        
                        string invoice_no = "";
                        double total_tax_var = 0;
                        Int32 tax_id = 0;
                        double tax_rate = 0;
                        string estimate_invoice_no = "";
                        bool estimate_status = false;
                        Int32 sale_id = 0;
                        
                        SalesModal salesModal_obj = new SalesModal();
                        SalesBLL salesObj = new SalesBLL();

                        if (chkbox_is_taxable.Checked)
                        {
                            total_tax_var = total_tax;
                        }
                        else
                        {
                            total_tax_var = 0;
                        }
                        
                        //if (invoice_status == "Update" && txt_invoice_no.Text.Substring(0, 1).ToUpper() == "S") //Update sales delete all record first and insert new sales
                        //{
                        //    salesObj.DeleteSales(txt_invoice_no.Text); //DELETE ALL TRANSACTIONS
                        //    invoice_no = txt_invoice_no.Text;
                        //}
                        //else
                        //{
                            if (sale_type == "Quotation")
                            {
                                invoice_no = salesObj.GetMaxEstimateInvoiceNo();
                            }
                            else
                            {
                                invoice_no = salesObj.GetMaxSaleInvoiceNo();
                                
                            }
                        
                        //}

                        if (txt_invoice_no.Text != "" && txt_invoice_no.Text.Substring(0, 1).ToUpper() == "E") //if estimates
                        {
                            estimate_invoice_no = txt_invoice_no.Text;
                            estimate_status = true;
                        }
                        
                        
                        DateTime sale_date = txt_sale_date.Value.Date;
                        int customer_id = (cmb_customers.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_customers.SelectedValue.ToString()));
                        int employee_id = (cmb_employees.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_employees.SelectedValue.ToString()));
                        int payment_terms_id = 0;// (cmb_payment_terms.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_payment_terms.SelectedValue.ToString()));
                        int payment_method_id = 0;// (cmb_payment_method.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_payment_method.SelectedValue.ToString()));

                        //salesModal_obj.customer_id = customer_id;
                        //salesModal_obj.employee_id = employee_id;
                        //salesModal_obj.invoice_no = invoice_no;
                        //salesModal_obj.total_amount = total_amount;
                        //salesModal_obj.total_discount = total_discount;
                        //salesModal_obj.sale_type = cmb_sale_type.SelectedValue.ToString();
                        //salesModal_obj.sale_date = sale_date;
                        //salesModal_obj.description = txt_description.Text;
                        //salesModal_obj.payment_terms_id = payment_terms_id;
                        //salesModal_obj.payment_method_id = payment_method_id;
                        //salesModal_obj.account = "Sale";
                        //salesModal_obj.is_return= false;
                        
                        
                        //set the date from datetimepicker and set time to te current time
                        DateTime now = DateTime.Now;
                        txt_sale_date.Value = new DateTime(txt_sale_date.Value.Year, txt_sale_date.Value.Month, txt_sale_date.Value.Day, now.Hour, now.Minute, now.Second);
                        /////////////////////

                        salesModal_obj.sale_time = txt_sale_date.Value;

                        /////Added sales header into the List
                        sales_model_header.Add(new SalesModalHeader
                        {
                            customer_id = customer_id,
                            employee_id = employee_id,
                            invoice_no = invoice_no,
                            total_amount = total_amount,
                            total_tax = total_tax_var,
                            total_discount = total_discount,
                           // total_discount_percent = (string.IsNullOrEmpty(txt_total_disc_percent.Text) ? 0 : Convert.ToDouble(txt_total_disc_percent.Text)),
                            sale_type = sale_type,
                            sale_date = sale_date,
                            sale_time = txt_sale_date.Value,
                            description = txt_description.Text,
                            payment_terms_id = payment_terms_id,
                            payment_method_id = payment_method_id,
                            account = "Sale",
                            is_return = false,
                            estimate_invoice_no = estimate_invoice_no,
                            estimate_status = estimate_status,

                            total_cost_amount = total_cost_amount,
                            cash_account_id = cash_account_id,
                            receivable_account_id = receivable_account_id,
                            tax_account_id = tax_account_id,
                            sales_discount_acc_id = sales_discount_acc_id,
                            inventory_acc_id = inventory_acc_id,
                            purchases_acc_id = purchases_acc_id,
                            sales_account_id = sales_account_id,
                        });
                        //////
                        
                        //if invoice type is sale then insert sales otherwise insert estimates/quotation
                        //Int32 sale_id = (cmb_sale_type.SelectedValue.ToString() == "Quotation" ? salesObj.InsertEstimates(salesModal_obj) : salesObj.InsertSales(salesModal_obj));

                        for (int i = 0; i < grid_sales.Rows.Count; i++)
                        {
                            if (grid_sales.Rows[i].Cells["code"].Value != null)
                            {
                                if (chkbox_is_taxable.Checked)
                                {
                                    tax_rate = (grid_sales.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString()));
                                    tax_id = Convert.ToInt32(grid_sales.Rows[i].Cells["tax_id"].Value.ToString());
                                    
                                }
                                else
                                {
                                    tax_id = 0;
                                    tax_rate = 0;
                                }

                                ///// Added sales detail in to List
                                sales_model_detail.Add(new SalesModal
                                {
                                    invoice_no = invoice_no,
                                    code = grid_sales.Rows[i].Cells["code"].Value.ToString(),
                                    name = grid_sales.Rows[i].Cells["name"].Value.ToString(),
                                    quantity_sold = (string.IsNullOrEmpty(grid_sales.Rows[i].Cells["qty"].Value.ToString()) ? 0 : double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString())),
                                    packet_qty = double.Parse(grid_sales.Rows[i].Cells["packet_qty"].Value.ToString()),
                                    unit_price = (string.IsNullOrEmpty(grid_sales.Rows[i].Cells["unit_price"].Value.ToString()) ? 0 : Math.Round(double.Parse(grid_sales.Rows[i].Cells["unit_price"].Value.ToString()), 4)),
                                    discount =  (string.IsNullOrEmpty(grid_sales.Rows[i].Cells["discount"].Value.ToString()) ? 0 : Math.Round(double.Parse(grid_sales.Rows[i].Cells["discount"].Value.ToString()), 4)),
                                    //discount_percent = double.Parse(grid_sales.Rows[i].Cells["discount_percent"].Value.ToString()),
                                    cost_price = (string.IsNullOrEmpty(grid_sales.Rows[i].Cells["cost_price"].Value.ToString()) ? 0 : Math.Round(Convert.ToDouble(grid_sales.Rows[i].Cells["cost_price"].Value.ToString()), 4)),// its avg cost actually ,
                                    item_type = grid_sales.Rows[i].Cells["item_type"].Value.ToString(),
                                    location_code = (grid_sales.Rows[i].Cells["location_code"].Value == null ? "" : grid_sales.Rows[i].Cells["location_code"].Value.ToString()),
                                    tax_id = tax_id,
                                    tax_rate = tax_rate,
                                    sale_date = sale_date,
                                });
                                //////////////

                            }
                            
                        }

                        //if invoice type is sale then insert sales otherwise insert estimates/quotation
                        if (sale_type == "Quotation")
                        {
                            sale_id = salesObj.InsertEstimates(sales_model_header, sales_model_detail); // for quotation / estimates
                        }
                        else
                        {
                            sale_id = salesObj.InsertSales(sales_model_header, sales_model_detail);// for sales items
                        }


                        if (sale_type != "Quotation" && sale_type != "Gift")//for sales 
                        {

                            //Employee commission entry
                            if (employee_commission_percent > 0)
                            {
                                var emp_commission_amount = employee_commission_percent * total_amount / 100;
                                Insert_emp_commission(invoice_no, 0, 0, emp_commission_amount, sale_date, txt_description.Text, int.Parse(cmb_employees.SelectedValue.ToString()));
                            }
                            /////

                            //User commission entry
                            if (user_commission_percent > 0)
                            {
                                var user_commission_amount = user_commission_percent * total_amount / 100;
                                Insert_user_commission(invoice_no, 0, 0, user_commission_amount, sale_date, txt_description.Text);
                            }
                            /////

                            

                        }
                        
                        if (sale_id > 0)
                        {
                            MessageBox.Show(invoice_no+" transaction created successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            
                            // PRINT INVOICE
                            if(chk_print_invoice.Checked)
                            {
                                if (sale_type == "Cash" || sale_type == "Credit")//for sales 
                                {
                                    using (frm_sales_invoice obj = new frm_sales_invoice(load_sales_receipt(salesModal_obj.invoice_no), true))
                                    {
                                        obj.load_print();
                                    }
                                }
                                else
                                {
                                    using (frm_sales_invoice obj = new frm_sales_invoice(load_estiamte_receipt(salesModal_obj.invoice_no), true))
                                    {
                                        obj.load_print();
                                    }
                                }
                            }

                            // CLEAR ALL FORM TEXTBOXES, GRID AND EVERYTING
                            clear_form();

                        }
                        else
                        {
                            MessageBox.Show("Record not saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please add products", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private int Insert_Journal_entry(string invoice_no, int account_id, double debit, double credit, DateTime date, 
            string description,int customer_id, int supplier_id,int entry_id, int employee_id=0)
        {
            int journal_id =0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            JournalsBLL JournalsObj = new JournalsBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = debit;
            JournalsModal_obj.credit = credit;
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.description = description;
            JournalsModal_obj.customer_id = customer_id;
            JournalsModal_obj.supplier_id = supplier_id;
            JournalsModal_obj.entry_id = entry_id;
            JournalsModal_obj.employee_id = employee_id;

            journal_id = JournalsObj.Insert(JournalsModal_obj);
            return journal_id;
        }

        private int Insert_emp_commission(string invoice_no, int account_id, double debit, double credit, DateTime date,
            string description, int employee_id = 0)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            EmployeeBLL emp_Obj = new EmployeeBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = debit;
            JournalsModal_obj.credit = credit;
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.description = description;
            JournalsModal_obj.employee_id = employee_id;

            journal_id = emp_Obj.InsertEmpCommission(JournalsModal_obj);
            return journal_id;
        }

        private int Insert_user_commission(string invoice_no, int account_id, double debit, double credit, DateTime date,
            string description)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            UsersBLL emp_Obj = new UsersBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = debit;
            JournalsModal_obj.credit = credit;
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.description = description;
            
            journal_id = emp_Obj.InsertUserCommission(JournalsModal_obj);
            return journal_id;
        }
        
        private void Get_AccountID_From_Company()
        {
            GeneralBLL objBLL = new GeneralBLL();

            String keyword = "TOP 1 *";
            String table = "pos_companies";
            DataTable companies_dt = objBLL.GetRecord(keyword, table);
            foreach (DataRow dr in companies_dt.Rows)
            {
                cash_account_id = (int)dr["cash_acc_id"];
                sales_account_id = (int)dr["sales_acc_id"];
                receivable_account_id = (int)dr["receivable_acc_id"];
                tax_account_id = (int)dr["tax_acc_id"];
                sales_discount_acc_id = (int)dr["sales_discount_acc_id"];
                inventory_acc_id = (int)dr["inventory_acc_id"];
                purchases_acc_id = (int)dr["purchases_acc_id"];
            }
        }

        public DataTable load_sales_receipt(string invoice_no)
        {
            //bind data in data grid view  
            SalesBLL objSalesBLL = new SalesBLL();
            DataTable dt = objSalesBLL.SaleReceipt(invoice_no);
            return dt;

        }

        public DataTable load_estiamte_receipt(string invoice_no)
        {
            //bind data in data grid view  
            SalesBLL objSalesBLL = new SalesBLL();
            DataTable dt = objSalesBLL.EstimateReceipt(invoice_no);
            return dt;

        }
        
        private void frm_sales_v1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                //if (e.KeyData == Keys.Enter)
                //{
                //    SendKeys.Send("{TAB}");
                //}
                if (e.KeyCode == Keys.F1)
                {
                    btn_new.PerformClick();
                }
                if (e.KeyCode == Keys.F3)
                {
                    btn_save.PerformClick();
                }
                if (e.Control && e.KeyCode == Keys.H)
                {
                    btn_movements.PerformClick();
                }
                
                if (e.KeyCode == Keys.F5)
                {
                    btn_round_prices.PerformClick();
                }
                if (e.KeyCode == Keys.F6)
                {
                    chk_print_invoice.Checked = !chk_print_invoice.Checked;
                }
                if (e.KeyCode == Keys.Escape)
                {
                    this.Controls.Remove(salesDataGridView);
                }
                if (e.KeyData == Keys.Down)
                {
                    brandsDataGridView.Focus();
                    salesDataGridView.Focus();
                    categoriesDataGridView.Focus();
                    groupsDataGridView.Focus();
                }

                if (e.Control && e.KeyCode == Keys.O)
                {
                    btn_search_invioces.PerformClick();
                }
                if(e.Control && e.KeyCode == Keys.Tab)
                {
                    int form_count = Application.OpenForms.Count;

                    MessageBox.Show(form_count.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void grid_sales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_sales.Columns[e.ColumnIndex].Name;
                if (name == "btn_delete")
                { 
                    grid_sales.Rows.RemoveAt(e.RowIndex);

                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_cost_amount();
                    get_total_amount();
                    get_total_qty();
                
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            
        }

        public void autoCompleteProductCode()
        {
            try
            {
            //    txt_product_code.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //    txt_product_code.AutoCompleteSource = AutoCompleteSource.CustomSource;
            //    AutoCompleteStringCollection Products_coll = new AutoCompleteStringCollection();

            //    ProductBLL productsBLL_obj = new ProductBLL();
            //    DataTable dt = productsBLL_obj.GetAllProductCodes();
                
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            Products_coll.Add(dr["code"].ToString() +" - "+ dr["name"].ToString());

            //        }

            //    }

            //    txt_product_code.AutoCompleteCustomSource = Products_coll;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        private void chkbox_is_taxable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_is_taxable.Checked == false)
            {
                txt_total_tax.Text = "0.00";
                total_amount -= total_tax;
                txt_total_amount.Text = total_amount.ToString();
            }
            else
            {
                txt_total_tax.Text = total_tax.ToString();
                total_amount += total_tax;
                txt_total_amount.Text = total_amount.ToString();
            }
        }

        private void btn_movements_Click(object sender, EventArgs e)
        {
            string global_product_code = "";

            if(grid_sales.Rows.Count > 0)
                {
                global_product_code = grid_sales.CurrentRow.Cells["code"].Value.ToString();
                }

            frm_productsMovements frm_prod_move_obj = new frm_productsMovements(global_product_code);

            frm_prod_move_obj.ShowDialog();

        }

        private void clear_form()
        {
            Get_user_total_commission();

            txt_description.Text = "";
            cmb_customers.SelectedValue = 0;
            //cmb_categories.SelectedValue = 0;
            cmb_employees.SelectedValue = 0;
            cmb_sale_type.SelectedValue = "Cash";
            //cmb_brands.SelectedValue = 0;
            cmb_customers.Refresh();

            invoice_status = "";
            btn_save.Text = "Save (F3)";
            txt_invoice_no.Text = "";
            
            total_amount = 0;
            total_discount = 0;
            total_tax = 0;
            total_sub_total = 0;
            total_cost_amount = 0;

            txt_sale_date.Refresh();
            txt_sale_date.Value = DateTime.Now;
            
            txt_sub_total.Text = "0.00";
            txt_total_amount.Text = "0.00";
            txt_total_tax.Text = "0.00";
            txt_total_discount.Text = "0.00";
            txt_total_amount.Text = "";
            
            txt_order_qty.Text = "";
            txt_shop_qty.Text = "";
            txt_company_qty.Text = "";
            txt_total_cost.Text = "";
            txt_cost_price.Text = "";

            // grid_sales.DataSource = null;
            grid_sales.Rows.Clear();
            grid_sales.Refresh();
            grid_sales.Rows.Add();

            this.ActiveControl = grid_sales;
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want new sale transaction", "New Transaction", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                clear_form();
                //txt_product_code.Focus();
            }
        }

        private void txt_customer_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        public void get_customers_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,first_name";
            string table = "pos_customers";

            DataTable customers = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = customers.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "";              // Set Column Value
            customers.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = customers.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[1] = "ADD NEW";              // Set Column Value
            customers.Rows.InsertAt(emptyRow1, 1);

            cmb_customers.DisplayMember = "first_name";
            cmb_customers.ValueMember = "id";
            cmb_customers.DataSource = customers;

           
        }

        public void get_employees_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,first_name";
            string table = "pos_employees";

            DataTable employees = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = employees.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "";              // Set Column Value
            employees.Rows.InsertAt(emptyRow, 0);
            
            
            cmb_employees.DisplayMember = "first_name";
            cmb_employees.ValueMember = "id";
            cmb_employees.DataSource = employees;


        }

        private void cmb_customers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_customers.SelectedValue.ToString() == "-1")
            {
                frm_addCustomer custfrm = new frm_addCustomer();
                custfrm.ShowDialog();

                get_customers_dropdownlist();
            }
            
        }

        private void btn_round_prices_Click(object sender, EventArgs e)
        {
            double subtotal = Convert.ToDouble(txt_sub_total.Text);
            double total_amount = Convert.ToDouble(txt_total_amount.Text);
            //frm_round_prices obj = new frm_round_prices(this, subtotal, total_amount,total_tax);
            //obj.ShowDialog();
        }

        public void round_total_amount(double round_total_amount)
        {
            //txt_total_amount.Text = round_total_amount.ToString();
            int total_rows = grid_sales.Rows.Count;
            double rounded_amount = round_total_amount / total_rows;

            //if(rounded_amount > 0)
            //{
                for (int i = 0; i <= total_rows-1; i++)
                {
                    double tax_rate = (grid_sales.Rows[i].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString()));
                    
                    grid_sales.Rows[i].Cells["discount"].Value = rounded_amount + Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);
                    //grid_sales.Rows[i].Cells["sub_total"].Value = Convert.ToDouble(grid_sales.Rows[i].Cells["sub_total"].Value) - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);

                    double total_value = Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value);
                    
                    double tax_1 = ((total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value)) * tax_rate / 100);

                    double sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);
                    
                    grid_sales.Rows[i].Cells["sub_total"].Value = sub_total_1.ToString("0.00");
                    //grid_sales.Rows[i].Cells["total_without_vat"].Value = (sub_total_1 - tax_1).ToString("0.00");
                    grid_sales.Rows[i].Cells["tax"].Value = (tax_1).ToString("0.00");
                }

                get_total_tax();
                get_total_discount();
                get_sub_total_amount();
                get_total_cost_amount();
                get_total_amount();
                
            //}

        }

        public void autoCompleteBrands()
        {
            try
            {
                //txt_brands.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                //txt_brands.AutoCompleteSource = AutoCompleteSource.CustomSource;
                //AutoCompleteStringCollection Products_coll = new AutoCompleteStringCollection();

                //BrandsBLL brandsBLL_obj = new BrandsBLL();
                //DataTable dt = brandsBLL_obj.GetAll();

                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        Products_coll.Add(dr["name"].ToString());

                //    }

                //}

                //txt_brands.AutoCompleteCustomSource = Products_coll;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        public void get_brands_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_brands";

            DataTable brands = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = brands.NewRow();
            emptyRow[0] = "";              // Set Column Value
            emptyRow[1] = "";              // Set Column Value
            brands.Rows.InsertAt(emptyRow, 0);
            
            //cmb_brands.DisplayMember = "name";
            //cmb_brands.ValueMember = "code";
            //cmb_brands.DataSource = brands;
            
        }

        public void get_categories_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_categories";

            DataTable categories = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = categories.NewRow();
            emptyRow[0] = "";              // Set Column Value
            emptyRow[1] = "";              // Set Column Value
           
            // Set Column Value
            categories.Rows.InsertAt(emptyRow, 0);

            //cmb_categories.DataSource = categories;
            //cmb_categories.DisplayMember = "name";
            //cmb_categories.ValueMember = "code";
            
        }

        private void get_saletype_dropdownlist()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            DataRow _row_1 = dt.NewRow();
            _row_1["id"] = "Cash";
            if (lang == "en-US")
            {
                _row_1["name"] = "Cash";
            }
            else if (lang == "ar-SA")
            {
                _row_1["name"] = "نقدي";
            }

            dt.Rows.Add(_row_1);

            if (allow_credit_sales)//user right check
            {
                DataRow _row = dt.NewRow();
                _row["id"] = "Credit";
                if (lang == "en-US")
                {
                    _row["name"] = "Credit";
                }
                else if (lang == "ar-SA")
                {
                    _row["name"] = "اجل";
                }

                dt.Rows.Add(_row);

            }

            DataRow _row_2 = dt.NewRow();
            _row_2["id"] = "Quotation";
            if (lang == "en-US")
            {
                _row_2["name"] = "Quotation";
            }
            else if (lang == "ar-SA")
            {
                _row_2["name"] = "اقتباس";
            }

            dt.Rows.Add(_row_2);

            DataRow _row_3 = dt.NewRow();
            _row_3["id"] = "Gift";
            if (lang == "en-US")
            {
                _row_3["name"] = "Gift";
            }
            else if (lang == "ar-SA")
            {
                _row_3["name"] = "هدية";
            }
            dt.Rows.Add(_row_3);

            cmb_sale_type.DisplayMember = "name";
            cmb_sale_type.ValueMember = "id";
            cmb_sale_type.DataSource = dt;

        }


        public void get_groups_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_product_groups";

            DataTable groups = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = groups.NewRow();
            emptyRow[0] = "";              // Set Column Value
            emptyRow[1] = "";              // Set Column Value
            // Set Column Value
            groups.Rows.InsertAt(emptyRow, 0);

            //cmb_groups.DataSource = groups;
            //cmb_groups.DisplayMember = "name";
            //cmb_groups.ValueMember = "code";

        }

        public void load_user_rights(int user_id)
        {
            UsersBLL userBLL_obj = new UsersBLL();
            DataTable users = userBLL_obj.GetUserRights(user_id);

            foreach (DataRow dr in users.Rows)
            {
                ///USER RIGHTS

                cash_sales_amount_limit = Convert.ToDouble(dr["cash_sales_amount"]);
                //txt_credit_sales_amt= (double) dr["credit_sales_amount"].ToString();
                //cash_purchase_amount_limit = Convert.ToDouble(dr["cash_purchase_amount"]);
                //txt_credit_purchase_amt.Text = (double) dr["credit_purchase_amount"].ToString();
                //allow_cash_sales.Checked = (bool)dr["allow_cash_sales"];
                allow_credit_sales = (bool)dr["allow_credit_sales"];
                //chk_allow_cash_purchase.Checked = (bool)dr["allow_cash_purchase"];
                //allow_credit_purchase= (bool)dr["allow_credit_purchase"];

            }
        }

        private void grid_sales_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("Are you sure you want delete", "Delete", buttons, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        grid_sales.Rows.RemoveAt(grid_sales.CurrentRow.Index);

                        get_total_tax();
                        get_total_discount();
                        get_sub_total_amount();
                        get_total_cost_amount();
                        get_total_amount();
                        get_total_qty();
                    }
                }
  
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    e.SuppressKeyPress = true; 
                    int iColumn = grid_sales.CurrentCell.ColumnIndex;
                    int iRow = grid_sales.CurrentCell.RowIndex;

                    if (iColumn <= 8)
                    {
                        if (grid_sales.Rows[iRow].Cells["code"].Value != null && grid_sales.Rows[iRow].Cells["unit_price"].Value != null && grid_sales.Rows[iRow].Cells["qty"].Value != null)
                        {
                            grid_sales.CurrentCell = grid_sales.Rows[iRow].Cells[iColumn + 1];
                            grid_sales.Focus();
                            grid_sales.CurrentCell.Selected = true;
                            //grid_sales.BeginEdit(true);
                        }
                    }
                    else if (iColumn > 8)
                    {
                        if (grid_sales.Rows[iRow].Cells["code"].Value != null && grid_sales.Rows[iRow].Cells["unit_price"].Value != null && grid_sales.Rows[iRow].Cells["qty"].Value != null)
                        {
                            grid_sales.Rows.Add();  //adds new row on last cell of row
                            this.ActiveControl = grid_sales;
                            grid_sales.CurrentCell = grid_sales.Rows[iRow + 1].Cells["code"];
                            //grid_sales.Rows[iRow + 1].Cells["code"].Value = product_code;
                            grid_sales.CurrentCell.Selected = true;
                            grid_sales.BeginEdit(true);
                        }
                        
                    }
                } 

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

        }

        private void txt_invoice_no_KeyDown(object sender, KeyEventArgs e)
        {
            try
            { 
                if (txt_invoice_no.Text != "" && e.KeyCode == Keys.Enter)
                {
                    string invoice_no = txt_invoice_no.Text;
                    string invoice_chr = invoice_no.Substring(0, 1);
                    SalesBLL salesObj = new SalesBLL();
                    DataTable estimates_dt = new DataTable();

                    if(invoice_chr.ToUpper() == "S")
                    {
                        estimates_dt = salesObj.GetSaleAndSalesItems(invoice_no);
                        Load_products_to_grid_by_invoiceno(estimates_dt, invoice_no);
                    }
                    else
                    {
                        estimates_dt = salesObj.GetEstimatesAndEstimatesItems(invoice_no);
                        Load_products_to_grid_by_invoiceno(estimates_dt, invoice_no);
                    }
                    txt_invoice_no.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
           
        }

        public void Load_products_to_grid_by_invoiceno(DataTable _dt, string invoice_no)
        {
            try
            {
                grid_sales.Rows.Clear();
                grid_sales.Refresh();
                txt_invoice_no.Text = invoice_no;

                string invoice_chr = invoice_no.Substring(0, 1);
                    
                if (invoice_chr.ToUpper() == "S")// for invoice edit
                {
                    invoice_status = "Update";
                    btn_save.Text = "Update (F3)";

                }
                else // for estimates 
                {
                    invoice_status = "Estimate";
                    btn_save.Text = "Sale (F3)";
                    //cmb_invoice_type.Enabled = false;

                }

                if (_dt.Rows.Count > 0)
                {

                    foreach (DataRow myProductView in _dt.Rows)
                    {
                        cmb_customers.SelectedValue = myProductView["customer_id"];
                        cmb_employees.SelectedValue = myProductView["employee_id"];
                        cmb_sale_type.SelectedValue = myProductView["sale_type"];
                        txt_sale_date.Value = Convert.ToDateTime(myProductView["sale_date"].ToString());
                        txt_description.Text = myProductView["description"].ToString();


                        double packet_qty = Convert.ToDouble(myProductView["packet_qty"].ToString());
                        double qty = Convert.ToDouble(myProductView["quantity_sold"].ToString());
                        double total = qty * double.Parse(myProductView["unit_price"].ToString());
                        double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                        double tax = (total * tax_rate / 100);
                        double sub_total = tax + total;
                        double sub_total_without_vat = total;


                        int id = Convert.ToInt32(myProductView["id"]);
                        string code = myProductView["code"].ToString();
                        string name = myProductView["name"].ToString();
                        double cost_price = Convert.ToDouble(myProductView["cost_price"]);
                        double unit_price = Convert.ToDouble(myProductView["unit_price"]);
                        double discount = Convert.ToDouble(myProductView["discount_value"]); ;

                        double total_value = Convert.ToDouble(myProductView["unit_price"]) * Convert.ToDouble(myProductView["quantity_sold"].ToString());
                        double discount_percent = Convert.ToDouble(myProductView["discount_value"]) / total_value * 100;

                        string location_code =  myProductView["loc_code"].ToString();
                        string unit = myProductView["unit"].ToString();
                        string category = myProductView["category"].ToString();
                        string item_type = myProductView["item_type"].ToString();
                        string btn_delete = "Del";
                        string tax_id = myProductView["tax_id"].ToString();

                        string shop_qty = ""; // myProductView["qty"].ToString();
                        string category_code = myProductView["category_code"].ToString();

                        double current_sub_total = Convert.ToDouble(qty) * unit_price + tax;


                        string[] row0 = { id.ToString(), code, name, packet_qty.ToString(),qty.ToString(), unit_price.ToString(), discount.ToString(), discount_percent.ToString(),
                                            sub_total_without_vat.ToString(),tax.ToString(), current_sub_total.ToString(),location_code,unit,category,
                                            btn_delete, shop_qty,tax_id.ToString(), tax_rate.ToString(), cost_price.ToString(),
                                            item_type,category_code};
                        int rowIndex = grid_sales.Rows.Add(row0);

                        ////////
                        //fill_locations_grid_combo(rowIndex, "", id.ToString());
                        //////
                    
                    }
                    //grid_sales.Rows.Add();

                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_cost_amount();
                    get_total_amount();
                    get_total_qty();
                    grid_sales.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void link_load_estimates_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //frm_search_estimates obj_estimate = new frm_search_estimates(this);
            //obj_estimate.ShowDialog();
        }

        private void addToPurchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(grid_sales.CurrentRow.Cells["code"].Value.ToString());
                if(grid_sales.RowCount > 0)
                {
                    if (grid_sales.CurrentRow.Cells["code"].Value != null)
                    {
                        int id = int.Parse(grid_sales.CurrentRow.Cells["id"].Value.ToString());
                        string code = grid_sales.CurrentRow.Cells["code"].Value.ToString();
                        string name = grid_sales.CurrentRow.Cells["name"].Value.ToString();
                        string category_code = grid_sales.CurrentRow.Cells["category_code"].Value.ToString();
                        double cost_price = double.Parse(grid_sales.CurrentRow.Cells["cost_price"].Value.ToString());
                        double unit_price = double.Parse(grid_sales.CurrentRow.Cells["unit_price"].Value.ToString());


                        //frm_add_porder porder_obj = new frm_add_porder(this, id, code, name, category_code, cost_price, unit_price);
                        //porder_obj.ShowDialog();
                    }

                }
                                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void grid_sales_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            
           e.Control.KeyPress -= new KeyPressEventHandler(tb_KeyPress);
           // e.Control.KeyPress -= new KeyPressEventHandler(code_KeyPress);

            e.Control.KeyUp -= new KeyEventHandler(code_txtbox_KeyUp);
            
            if (grid_sales.CurrentCell.ColumnIndex == 3 || grid_sales.CurrentCell.ColumnIndex == 4 || grid_sales.CurrentCell.ColumnIndex == 5
                || grid_sales.CurrentCell.ColumnIndex == 6 || grid_sales.CurrentCell.ColumnIndex == 7) //qty, unit price and discount Column will accept only numeric
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
                }
            }

            if (grid_sales.CurrentCell.ColumnIndex == 1)//code cell key press
            {
                TextBox code_txtbox = e.Control as TextBox;
                if (code_txtbox != null)
                {
                    code_txtbox.KeyUp += code_txtbox_KeyUp; 
                }
            }

        }

        void code_txtbox_KeyUp(object sender, KeyEventArgs e)
        {
            //when type in code cell the search window will appear and search from database
            try
            {
                //if (e.KeyCode == Keys.Enter)
                //{
                //    e.Handled = true;
                //    salesDataGridView.Focus();
                //   // salesDataGridView.CurrentCell = salesDataGridView.Rows[0].Cells[0];
                //}

                    int iColumn = grid_sales.CurrentCell.ColumnIndex;
                    int iRow = grid_sales.CurrentCell.RowIndex;
                    string columnName = grid_sales.Columns[iColumn].Name;

                    string product_code = (grid_sales.CurrentRow.Cells["code"].EditedFormattedValue.ToString() != "" ? grid_sales.CurrentRow.Cells["code"].EditedFormattedValue.ToString() : "");

                    if (product_code != "") // && product_code.Length >= 3
                    {
                        // string product_code = grid_sales.Rows[iRow].Cells[1].Value.ToString();
                        //var brand_code = "";// txt_brand_code.Text; // (cmb_brands.SelectedValue != null ? cmb_brands.SelectedValue.ToString() : "");
                       // var category_code = "";//txt_category_code.Text; // (cmb_categories.SelectedValue != null ? cmb_categories.SelectedValue.ToString() : "");
                       // var group_code = "";//txt_group_code.Text; // (cmb_groups.SelectedValue != null ? cmb_groups.SelectedValue.ToString() : "");

                        SetupSalesDataGridView();

                        //ProductBLL objBLL = new ProductBLL();
                        //string brand_name = "";//txt_brands.Text;
                        
                        DataView dv = allProduct_dt.DefaultView;
                        dv.RowFilter = string.Format("name LIKE '%{0}%' OR code LIKE '%{0}%'", product_code);
                        DataTable dt = dv.ToTable();

                        //DataTable dt = ProductBLL.SearchProductByBrandAndCategory(product_code, category_code, brand_code, group_code);

                        if (dt.Rows.Count > 0)
                        {
                            salesDataGridView.Rows.Clear();
                            foreach (DataRow dr in dt.Rows)
                            {
                                string id = dr["id"].ToString();
                                string code = dr["code"].ToString();
                                string name = dr["name"].ToString();
                                string qty = dr["qty"].ToString();
                                string unit_price = dr["unit_price"].ToString();
                                string category = dr["category_code"].ToString();
                                string description = dr["description"].ToString();

                                string[] row0 = { id, code, name, qty, unit_price, category, description };

                                salesDataGridView.Rows.Add(row0);
                            }
                            //salesDataGridView.CurrentCell = salesDataGridView.Rows[0].Cells[0];
                            //salesDataGridView.ClearSelection();
                            //salesDataGridView.CurrentCell = salesDataGridView.Rows[0].Cells[0];
                        }
                    }
                    else
                    {
                        salesDataGridView.Rows.Clear();
                    }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
           
                
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            btn_movements.PerformClick();
        }

        private void btn_search_invioces_Click(object sender, EventArgs e)
        {
            frm_search_invoices frm = new frm_search_invoices(null,this);
            frm.ShowDialog();
            
        }

        private void chk_show_total_cost_CheckedChanged(object sender, EventArgs e)
        {
            if(chk_show_total_cost.Checked)
            {
                txt_cost_price_with_vat.Visible = true;
                lbl_cost_vat.Visible = true;
                lbl_cost_price.Visible = true;
                lbl_total_cost.Visible = true;
                txt_total_cost.Visible = true;
                txt_cost_price.Visible = true;
            }else{
                txt_total_cost.Text = "0";
                lbl_cost_vat.Visible = false;
                txt_cost_price_with_vat.Visible = false;
                txt_total_cost.Visible = false;
                lbl_cost_price.Visible = false;
                lbl_total_cost.Visible = false;
                txt_cost_price.Visible = false;
            }
        }

        private void productDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(grid_sales.RowCount > 0)
                {
                    if (grid_sales.CurrentRow.Cells["code"].Value != null)
                    {
                        string product_id = grid_sales.CurrentRow.Cells["code"].Value.ToString();
                        
                        //frm_product_full_detail obj = new frm_product_full_detail(this, product_id);
                       // obj.ShowDialog();
                    }
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void grid_sales_SelectionChanged(object sender, EventArgs e)
        {
            if (grid_sales.Focused)
            {
                // your code
            
            if (grid_sales.Rows.Count > 0 && grid_sales.CurrentRow.Cells["shop_qty"].Value != null)
            {
                string  product_code = grid_sales.CurrentRow.Cells["code"].Value.ToString();
                double tax_rate = (grid_sales.CurrentRow.Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.CurrentRow.Cells["tax_rate"].Value.ToString()));
                if(product_code != null)
                {
                    string shop_qty = "";
                    string avg_cost = "";

                    DataTable dt = productsBLL_obj.GetAllByProductCode(product_code);
                    foreach (DataRow myProductView in dt.Rows)
                    {
                        shop_qty = myProductView["qty"].ToString();
                        avg_cost = myProductView["avg_cost"].ToString();
                        
                        if (myProductView["picture"].ToString() != "" && myProductView["picture"].ToString() != "0x")
                        {
                            byte[] myImage = new byte[0];
                            myImage = (byte[])myProductView["picture"];
                            MemoryStream stream = new MemoryStream(myImage);
                            
                            if (stream.Length > 0)
                            {
                                product_pic.Image = Image.FromStream(stream);

                            }
                            else
                            {
                                product_pic.Image = null;
                            }

                        }
                        
                    }

                    txt_shop_qty.Text = shop_qty;
                    txt_company_qty.Text = shop_qty;
                    txt_cost_price.Text = avg_cost;
                    txt_total_cost.Text = total_cost_amount.ToString();
                    txt_cost_price_with_vat.Text = (total_cost_amount+(total_cost_amount * tax_rate / 100)).ToString();

                    Purchases_orderBLL poBLL = new Purchases_orderBLL();
                    txt_order_qty.Text = poBLL.GetPOrder_qty(product_code).ToString();
                }
             }
            }
            
        }

        private void grid_sales_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //for Serial No. in grid
            using (SolidBrush b = new SolidBrush(grid_sales.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            } 
            //grid_sales.Rows[e.RowIndex].Cells["sno"].Value = (e.RowIndex + 1).ToString();
        }

        private void updateProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(grid_sales.CurrentRow.Cells["code"].Value.ToString());
                if (grid_sales.CurrentRow.Cells["code"].Value != null)
                {
                    string product_id = grid_sales.CurrentRow.Cells["id"].Value.ToString();
                    //frm_addProduct frm_addProduct_obj = new frm_addProduct(null, int.Parse(product_id), "true",null,"",this);
                    //frm_addProduct_obj.WindowState = FormWindowState.Maximized;
                    //frm_addProduct_obj.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cmb_employees_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_employees.SelectedValue.ToString() != null && cmb_employees.SelectedValue.ToString() != "0")
            {
                int employee_id = Convert.ToInt32(cmb_employees.SelectedValue.ToString());

                EmployeeBLL obj = new EmployeeBLL();
                DataTable employees = obj.SearchRecordByID(employee_id);

                foreach (DataRow dr in employees.Rows)
                {
                    //txt_customer_vat.Text = dr["commission_percent"].ToString();
                    employee_commission_percent = double.Parse(dr["commission_percent"].ToString());
                }

                ///customer commission balance
                //long emp_total_balance = obj.GetEmpCommissionBalance(employee_id);
                //txt_cust_balance.Text = emp_total_balance.ToString();
                ///
                
            }
        }

        private void Get_user_total_commission()
        {
            UsersBLL obj = new UsersBLL();
            DataTable users = obj.GetUser(UsersModal.logged_in_userid);

            foreach (DataRow dr in users.Rows)
            {
                user_commission_percent = double.Parse(dr["commission_percent"].ToString());
            }

            ///customer commission balance
            long user_commission_total_balance = obj.GetUserCommissionBalance(UsersModal.logged_in_userid);
            //txt_user_commission_balance.Text = user_commission_total_balance.ToString();
            ///
        }

        private void addNewRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid_sales.Rows.Add();
        }
        
        
        private void link_load_estimates_Click(object sender, EventArgs e)
        {
            //frm_search_estimates obj_estimate = new frm_search_estimates(this);
            //obj_estimate.ShowDialog();
        }

        private void txt_amount_received_KeyUp(object sender, KeyEventArgs e)
        {
            double received_amount = (txt_amount_received.Text == "" ? 0 : double.Parse(txt_amount_received.Text));
            txt_change_amount.Text = (total_amount+total_tax - received_amount).ToString();
        }

        private void grid_sales_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string columnName = grid_sales.Columns[e.ColumnIndex].Name; 
            if(columnName == "code")
            {
                if(allProduct_dt.Rows.Count <= 0)
                {
                    allProduct_dt = ProductBLL.GetAll();
                }
            }
        }
    }
}
