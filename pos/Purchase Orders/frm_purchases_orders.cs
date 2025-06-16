using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace pos
{
    

    public partial class frm_purchases_order : Form
    {
        public string lang = (UsersModal.logged_in_lang.Length > 0 ? UsersModal.logged_in_lang : "en-US");
        public int cash_account_id = 0;
        //public int sales_account_id = 0;
        public int payable_account_id = 0;
        public int tax_account_id = 0;
        public int purchases_order_discount_acc_id = 0;
        public int inventory_acc_id = 0;
        public int purchases_order_acc_id = 0;
        
        public static frm_purchases_order instance;
        public TextBox tb_product_id;
        public TextBox tb_code;
        public TextBox tb_product_name;
        public TextBox tb_qty;
        public TextBox tb_cost_price;

        public double total_amount = 0;
        public double total_tax = 0;
        public double total_discount = 0;
        public double total_sub_total = 0;

        public int global_product_id = 0;
        public int global_tax_id = 0;
        public double global_tax_rate = 0;
        public double global_unit_price = 0;
        public string global_location_code = "";
        public string global_unit = "";
        public string global_item_category = "";

        public double cash_purchase_amount_limit = 0;
        public bool allow_credit_purchase = false;

        private DataGridView brandsDataGridView = new DataGridView();
        private DataGridView categoriesDataGridView = new DataGridView();
        private DataGridView groupsDataGridView = new DataGridView();

        string invoice_status = "";
        string po_invoice_no = "";
        string product_code = "";
        //private frm_searchProducts productsMainForm;

        //public frm_purchases_order(frm_searchProducts productsMainForm) : this()
        //{
        //    this.productsMainForm = productsMainForm;
        //}

        public frm_purchases_order()
        {
            InitializeComponent();
            //autoCompleteProductCode();
            
            //instance = this;
            //global_product_id = txt_product_code;
            //tb_code = txt_product_code;
            //tb_product_name = txt_product_name;
            ////tb_description = txt_description;
            //tb_qty = txt_qty;
            //tb_cost_price = txt_cost_price;
            //tb_cost_price = txt_cost_price;
            
        }

        private void frm_purchases_order_Load(object sender, EventArgs e)
        {
            grid_purchases_order.Rows.Add();
            this.ActiveControl = grid_purchases_order;
            grid_purchases_order.CurrentCell = grid_purchases_order.Rows[0].Cells[1];
            
            Get_AccountID_From_Company();
            load_user_rights(UsersModal.logged_in_userid);
            //txt_product_name.Text = tb_product_name.Text;
            
            Cmb_zero_qty.SelectedIndex = 0;
            //btn_movements.Enabled = false;
            get_suppliers_dropdownlist();
            get_employees_dropdownlist();
            
            GetMAXInvoiceNo();

            //disable sorting in grid
            foreach (DataGridViewColumn column in grid_purchases_order.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            LoadBrandList();
        }
        
        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchProducts frm_search_product_obj = new frm_searchProducts();
            frm_search_product_obj.Show();

        }

        public string GetMAXInvoiceNo()
        {
            Purchases_orderBLL Purchases_orderBLL_obj = new Purchases_orderBLL();
            return Purchases_orderBLL_obj.GetMaxInvoiceNo();
        }
        Form purchaseSearchObj;
        private void grid_purchases_order_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                string columnName = grid_purchases_order.Columns[e.ColumnIndex].Name;
                if (columnName == "code")
                {
                    // string product_code = grid_purchases_order[e.ColumnIndex, e.RowIndex].Value.ToString();

                    product_code = (grid_purchases_order.CurrentRow.Cells["code"].Value != null ? grid_purchases_order.CurrentRow.Cells["code"].Value.ToString() : "");    
                    bool isGrid = true;
                    //var brand_code = txt_brand_code.Text; // (cmb_brands.SelectedValue != null ? cmb_brands.SelectedValue.ToString() : "");
                    var category_code = txt_category_code.Text; // (cmb_categories.SelectedValue != null ? cmb_categories.SelectedValue.ToString() : "");
                    var group_code = txt_group_code.Text; // (cmb_groups.SelectedValue != null ? cmb_groups.SelectedValue.ToString() : "");
                    
                    // Get selected brand codes
                    var selectedBrands = GetSelectedBrandCodes();
                    string brand_code = string.Join(",", selectedBrands); // comma-separated

                                                                        ////////////////////////
                    if (purchaseSearchObj == null || product_code != "")
                    {
                        purchaseSearchObj = new frm_searchPurchaseProducts(null, this, product_code, category_code, brand_code, e.RowIndex, isGrid, group_code);
                        purchaseSearchObj.FormClosed += new FormClosedEventHandler(purchaseSearchObj_FormClosed);
                        //frm_cust.Dock = DockStyle.Fill;
                        purchaseSearchObj.ShowDialog();
                    }
                    else
                    {
                        //frm_searchSaleProducts_obj.ShowDialog();
                        //frm_searchSaleProducts_obj.BringToFront();
                        purchaseSearchObj.Visible = true;
                    }
                    ////////////
                        
                    //show product purchase history form
                    //btn_product_purchase_history.PerformClick();

                    
                }

                if (columnName == "Qty") // if qty is changed
                {
                    double tax_rate = (grid_purchases_order.Rows[e.RowIndex].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_purchases_order.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                    double tax = (Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["avg_cost"].Value) * tax_rate / 100);
                     
                    grid_purchases_order.Rows[e.RowIndex].Cells["tax"].Value = (tax * Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["qty"].Value));
                    double sub_total = Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["discount"].Value);
                    grid_purchases_order.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total;
                }
                //if (columnName == "avg_cost")//if avg_cost is changed
                //{
                //    double tax_rate = (grid_purchases_order.Rows[e.RowIndex].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_purchases_order.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                //    double tax = (Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["avg_cost"].Value) * tax_rate / 100);
                //    grid_purchases_order.Rows[e.RowIndex].Cells["tax"].Value = (tax * Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["qty"].Value));
                    
                //    double sub_total = Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["discount"].Value);
                //    grid_purchases_order.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total;
                //}

                //if (columnName == "discount")//if discount is changed
                //{
                //    double sub_total = Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["discount"].Value);
                //    //total_discount += Convert.ToDouble(grid_purchases_order.Rows[e.RowIndex].Cells["discount"].Value);
                //    //txt_total_discount.Text = total_amount.ToString();

                //    grid_purchases_order.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total;
                //}

                get_total_tax();
                get_total_discount();
                get_sub_total_amount();
                get_total_amount();
                get_total_qty();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }

        }

        void purchaseSearchObj_FormClosed(object sender, FormClosedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void Load_products_to_grid(string item_number, int RowIndex1)
        {
            ProductBLL productsBLL_obj = new ProductBLL();
            DataTable dt = productsBLL_obj.SearchRecordByProductNumber(item_number);
            //grid_purchases_order.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            int RowIndex = grid_purchases_order.CurrentCell.RowIndex;

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < grid_purchases_order.RowCount; i++)
                {
                    var grid_item_number = (grid_purchases_order.Rows[i].Cells["item_number"].Value != null ? grid_purchases_order.Rows[i].Cells["item_number"].Value : "");
                    if (grid_item_number.ToString() == item_number)
                    {
                        MessageBox.Show("Product already added", "Already exist", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        //DataView dvProducts = products_dt.DefaultView;
                        //dvProducts.RowFilter = ""
                        grid_purchases_order.Focus();
                        grid_purchases_order.CurrentCell = grid_purchases_order.Rows[RowIndex].Cells["code"]; //make qty cell active
                        grid_purchases_order.CurrentCell.Selected = true;
                        grid_purchases_order.BeginEdit(true);
                        return;
                    }
                    else
                    {
                        grid_purchases_order.CurrentCell = grid_purchases_order.Rows[RowIndex].Cells["qty"]; //make qty cell active
                        grid_purchases_order.CurrentCell.Selected = true;

                    }
                }

                foreach (DataRow myProductView in dt.Rows)
                {
                    double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                    double tax = (double.Parse(myProductView["avg_cost"].ToString()) * tax_rate / 100);
                    double sub_total = tax + double.Parse(myProductView["avg_cost"].ToString());

                    grid_purchases_order.Rows[RowIndex].Cells["id"].Value = myProductView["id"].ToString();
                    grid_purchases_order.Rows[RowIndex].Cells["item_number"].Value = myProductView["item_number"].ToString();
                    grid_purchases_order.Rows[RowIndex].Cells["code"].Value = myProductView["code"].ToString();
                    grid_purchases_order.Rows[RowIndex].Cells["name"].Value = myProductView["name"].ToString();
                    grid_purchases_order.Rows[RowIndex].Cells["qty"].Value = (myProductView["demand_qty"].ToString() == string.Empty || (decimal)myProductView["demand_qty"] == 0 ? "1" : myProductView["demand_qty"].ToString());
                    grid_purchases_order.Rows[RowIndex].Cells["avg_cost"].Value = Math.Round(double.Parse(myProductView["avg_cost"].ToString()),3);
                    grid_purchases_order.Rows[RowIndex].Cells["unit_price"].Value = Math.Round(double.Parse(myProductView["unit_price"].ToString()),3);
                    grid_purchases_order.Rows[RowIndex].Cells["discount"].Value = 0.00;
                    grid_purchases_order.Rows[RowIndex].Cells["tax"].Value = Math.Round(tax,3);
                    grid_purchases_order.Rows[RowIndex].Cells["sub_total"].Value = Math.Round(sub_total,3);
                    grid_purchases_order.Rows[RowIndex].Cells["location_code"].Value = ""; // myProductView["location_code"].ToString();
                    grid_purchases_order.Rows[RowIndex].Cells["unit"].Value = myProductView["unit"].ToString();
                    grid_purchases_order.Rows[RowIndex].Cells["category"].Value = myProductView["category"].ToString();
                    grid_purchases_order.Rows[RowIndex].Cells["btn_delete"].Value = "Del";
                    grid_purchases_order.Rows[RowIndex].Cells["tax_id"].Value = myProductView["tax_id"].ToString();
                    grid_purchases_order.Rows[RowIndex].Cells["tax_rate"].Value = myProductView["tax_rate"].ToString();
                    grid_purchases_order.Rows[RowIndex].Cells["available_qty"].Value = myProductView["qty"].ToString();
                    grid_purchases_order.Rows[RowIndex].Cells["qty_sold"].Value = 0;

                }

                get_total_qty();

                //load_product_purchase_history(product_id);
            }
        }

        private void txt_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txt_barcode.Text != string.Empty && e.KeyCode == Keys.Enter)
            {
                load_products("", "", txt_barcode.Text.Trim());
                txt_barcode.Text = "";

            }
        }
        public void load_products(string item_number = "", string product_name = "", string barcode = "")
        {

            ProductBLL productsBLL_obj = new ProductBLL();
            //DataTable product_dt = productsBLL_obj.SearchRecordByProductCode(txt_product_code.Text);
            DataTable product_dt = new DataTable();

            if (item_number != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByProductNumber(item_number);
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
                    double qty = (myProductView["purchase_demand_qty"].ToString() == "0" || (decimal)myProductView["purchase_demand_qty"] == 0 ? 1 : Convert.ToDouble(myProductView["purchase_demand_qty"].ToString()));
                    double total = qty * double.Parse(myProductView["cost_price"].ToString());
                    double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                    double tax = (total * tax_rate / 100);
                    double current_sub_total = tax + total;

                    int id = Convert.ToInt32(myProductView["id"]);
                    string code = myProductView["code"].ToString();
                    string name = myProductView["name"].ToString();
                    string available_qty = "1";
                    string qty_sold = "0";
                    double avg_cost = Convert.ToDouble(myProductView["avg_cost"]);
                    double unit_price = Convert.ToDouble(myProductView["unit_price"]);
                    double discount = 0.00;// Convert.ToDouble(myProductView["discount_value"]); ;
                    string location_code = ""; // myProductView["location_code"].ToString();
                    string unit = myProductView["unit"].ToString();
                    string category = myProductView["category"].ToString();
                    string item_type = myProductView["item_type"].ToString();
                    string btn_delete = "Del";
                    string tax_id = myProductView["tax_id"].ToString();
                    string item_number1 = myProductView["item_number"].ToString();

                    string[] row0 = { id.ToString(), code, name,  avg_cost.ToString(), tax.ToString(), qty_sold,
                                            available_qty,qty.ToString(),category,unit_price.ToString(), discount.ToString(),
                            current_sub_total.ToString(),location_code,unit,btn_delete,
                            tax_id.ToString(), tax_rate.ToString(),item_number1 };

                    //Remove the first empty row
                    if (grid_purchases_order.RowCount > 0 && grid_purchases_order.Rows[0].Cells["id"].Value == null)
                    {
                        grid_purchases_order.Rows.RemoveAt(0);
                    }
                    //
                    int RowIndex = grid_purchases_order.Rows.Add(row0);

                    //int id = Convert.ToInt32(myProductView["id"]);
                    //string code = myProductView["code"].ToString();
                    //string name = myProductView["name"].ToString();
                    //string qty = (myProductView["purchase_demand_qty"].ToString() == string.Empty || (decimal)myProductView["purchase_demand_qty"] == 0 ? "1" : myProductView["purchase_demand_qty"].ToString());
                    //double avg_cost = Convert.ToDouble(myProductView["avg_cost"]);
                    //double unit_price = Convert.ToDouble(myProductView["unit_price"]);
                    //double discount = 0.00;
                    //string location_code = "";// myProductView["location_code"].ToString();
                    //string unit = myProductView["unit"].ToString();
                    //string category = myProductView["category"].ToString();
                    //string btn_delete = "Del";
                    //string tax_id = myProductView["tax_id"].ToString();
                    //double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : Convert.ToDouble(myProductView["tax_rate"]));

                    //double tax = (Convert.ToDouble(qty) * avg_cost * tax_rate / 100);

                    //double current_sub_total = Convert.ToDouble(qty) * avg_cost + tax;
                    //string item_number1 = myProductView["item_number"].ToString();

                    //string[] row0 = { id.ToString(), code, name, 
                    //                        qty, avg_cost.ToString(),unit_price.ToString(), discount.ToString(), 
                    //                        tax.ToString(), current_sub_total.ToString(),location_code,unit,category, 
                    //                        btn_delete, tax_id.ToString(), tax_rate.ToString(), unit_price.ToString(),item_number1 };
                    //int RowIndex = grid_purchases_order.Rows.Add(row0);


                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_amount();
                    get_total_qty();

                    global_product_id = 0;
                    global_unit_price = 0;
                    global_tax_id = 0;
                    global_tax_rate = 0;
                }

                txt_barcode.Focus();

            }
            else
            {
                
                MessageBox.Show("Record not found", "Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_barcode.Focus();
            }

        }

        private void get_sub_total_amount()
        {
            total_sub_total = 0;

            for (int i = 0; i <= grid_purchases_order.Rows.Count - 1; i++)
            {
                total_sub_total += Convert.ToDouble(grid_purchases_order.Rows[i].Cells["qty"].Value) * Convert.ToDouble(grid_purchases_order.Rows[i].Cells["avg_cost"].Value) - Convert.ToDouble(grid_purchases_order.Rows[i].Cells["discount"].Value);
            }

            txt_sub_total.Text = (total_sub_total).ToString();
        }
        
        private void get_total_amount()
        {
            total_amount = 0;

            for (int i = 0; i <= grid_purchases_order.Rows.Count - 1; i++)
            {
                total_amount += Convert.ToDouble(grid_purchases_order.Rows[i].Cells["qty"].Value) * Convert.ToDouble(grid_purchases_order.Rows[i].Cells["avg_cost"].Value);
            }
            double net = (total_amount + total_tax - total_discount);
            txt_total_amount.Text = net.ToString();
        }

        private void get_total_tax()
        {
            total_tax = 0;

            for (int i = 0; i <= grid_purchases_order.Rows.Count - 1; i++)
            {
                total_tax += (grid_purchases_order.Rows[i].Cells["tax"].Value == "" ? 0 : Convert.ToDouble(grid_purchases_order.Rows[i].Cells["tax"].Value));
            }

            txt_total_tax.Text = total_tax.ToString();
        }
        private void get_total_discount()
        {
            total_discount = 0;

            for (int i = 0; i <= grid_purchases_order.Rows.Count - 1; i++)
            {
                total_discount += Convert.ToDouble(grid_purchases_order.Rows[i].Cells["discount"].Value);
            }

            txt_total_discount.Text = total_discount.ToString();
        }

        private void get_total_qty()
        {
            double total_qty = 0;

            for (int i = 0; i <= grid_purchases_order.Rows.Count - 1; i++)
            {
                total_qty += Convert.ToDouble(grid_purchases_order.Rows[i].Cells["qty"].Value);
            }

            txt_total_qty.Text = (total_qty).ToString();

        }
        
        private int Insert_Journal_entry(string invoice_no, int account_id, double debit, double credit, DateTime date,
            string description, int customer_id, int supplier_id, int entry_id)
        {
            int journal_id = 0;
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

            journal_id = JournalsObj.Insert(JournalsModal_obj);
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
                payable_account_id = (int)dr["payable_acc_id"];
                tax_account_id = (int)dr["tax_acc_id"];
                purchases_order_discount_acc_id = (int)dr["purchases_discount_acc_id"];
                inventory_acc_id = (int)dr["inventory_acc_id"];
                purchases_order_acc_id = (int)dr["purchases_acc_id"];
            }
        }

        private void frm_purchases_order_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                if (e.KeyData == Keys.Enter)
                {
                    //SendKeys.Send("{TAB}");
                }
                if (e.KeyCode == Keys.F1)
                {
                    NewToolStripButton.PerformClick();
                }
                if (e.KeyCode == Keys.F3)
                {
                    SaveToolStripButton.PerformClick();
                }
                if (e.KeyCode == Keys.F4)
                {
                    SearchToolStripButton.PerformClick();
                }
                if (e.Control && e.KeyCode == Keys.H)
                {
                    HistoryToolStripButton.PerformClick();
                }
                if (e.KeyCode == Keys.F5)
                {
                    PurchaseHistoryToolStripButton.PerformClick();
                } 
                if (e.KeyCode == Keys.Escape)
                {
                    
                }

                if (e.Control && e.KeyCode == Keys.O)
                {
                    SearchToolStripButton.PerformClick();
                }

                if (e.KeyData == Keys.Down)
                {
                    brandsDataGridView.Focus();
                    categoriesDataGridView.Focus();
                    groupsDataGridView.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void grid_purchases_order_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_purchases_order.Columns[e.ColumnIndex].Name;
                if (name == "btn_delete")
                {
                    grid_purchases_order.Rows.RemoveAt(e.RowIndex);

                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_amount();
                    get_total_qty();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    
            }
            

        }

        private void clear_form()
        {
            //txt_supplier_id.Text = "";
            //txt_supplier_name.Text = "";
            //txt_supplier_vat.Text = "";
            invoice_status = "";
            SearchBySupplierCheckbox.Checked = false;
            cmb_suppliers.SelectedValue = 0;
            cmb_suppliers.Refresh();
            //cmb_categories.SelectedValue = 0;
            cmb_employees.SelectedValue = 0;
            
            //cmb_brands.SelectedValue = 0;

            txt_brand_code.Text = "";
            txt_brands.Text = "";
            txt_category_code.Text = "";
            txt_categories.Text = "";
            txt_groups.Text = "";
            txt_group_code.Text = "";
            txt_invoice_no.Text = "";
            txt_description.Text = "";
            //txt_discount.Text = "";
            //txt_vat.Text = "";

            global_product_id = 0;
            //global_avg_cost = 0;
            global_tax_id = 0;
            global_tax_rate = 0;

            total_amount = 0;
            total_discount = 0;
            total_tax = 0;
            total_sub_total = 0;
           

            //txt_product_code.Text = "";
            //txt_product_name.Text = "";
            //txt_qty.Text = "";
            //txt_avg_cost.Text = "";
            //txt_discount.Text = "";
            txt_purchase_date.Refresh();
            txt_purchase_date.Value = DateTime.Now;
            
            txt_sub_total.Text = "0.00";
            txt_total_amount.Text = "0.00";
            txt_total_tax.Text = "0.00";
            txt_total_discount.Text = "0.00";
            txt_total_qty.Text = "";

            grid_purchases_order.DataSource = null;
            grid_purchases_order.Rows.Clear();
            grid_purchases_order.Refresh();
            grid_purchases_order.Rows.Add();
            grid_purchases_order.CurrentCell = grid_purchases_order.Rows[0].Cells[1];

            grid_product_history.DataSource = null;
            grid_product_history.Refresh();

        }

        public void get_suppliers_dropdownlist()
        {
            SupplierBLL supplierBLL = new SupplierBLL();

            DataTable suppliers = supplierBLL.GetAll();
            DataRow emptyRow = suppliers.NewRow();
            emptyRow[0] = "0";              // Set Column Value
            emptyRow[2] = "Select Supplier";              // Set Column Value
            suppliers.Rows.InsertAt(emptyRow, 0);

            //DataRow emptyRow1 = suppliers.NewRow();
            //emptyRow1[0] = "-1";              // Set Column Value
            //emptyRow1[2] = "ADD NEW";              // Set Column Value
            //suppliers.Rows.InsertAt(emptyRow1, 1);
            
            cmb_suppliers.DisplayMember = "first_name";
            cmb_suppliers.ValueMember = "id";
            cmb_suppliers.DataSource = suppliers;

        }
        private void cmb_suppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(SearchBySupplierCheckbox.Checked && cmb_suppliers.Focused)
                {
                    grid_purchases_order.Rows.Clear();
                    grid_purchases_order.Refresh();

                    if (!string.IsNullOrEmpty(cmb_suppliers.SelectedValue.ToString()) && cmb_suppliers.SelectedValue.ToString() != "0")
                    {
                        ProductBLL productsBLL_obj = new ProductBLL();
                        DataTable product_dt = productsBLL_obj.SearchProductBySupplier(cmb_suppliers.SelectedValue.ToString());

                        if (product_dt.Rows.Count > 0)
                        {
                            
                            foreach (DataRow myProductView in product_dt.Rows)
                            {
                                double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                                double required_qty = (myProductView["purchase_demand_qty"].ToString() == string.Empty || (decimal)myProductView["purchase_demand_qty"] == 0 ? 1 : Convert.ToDouble(myProductView["purchase_demand_qty"].ToString()));
                                double tax = (double.Parse(myProductView["avg_cost"].ToString()) * tax_rate / 100)* required_qty;
                                double sub_total = tax + double.Parse(myProductView["avg_cost"].ToString());

                                double qty = Convert.ToDouble(myProductView["qty"].ToString());
                                double total = qty * double.Parse(myProductView["avg_cost"].ToString());
                                double current_sub_total = tax + total;

                                int id = Convert.ToInt32(myProductView["id"]);
                                string code = myProductView["code"].ToString();
                                string name = myProductView["name"].ToString();
                                double cost_price = Convert.ToDouble(myProductView["avg_cost"]);
                                double qty_sold = 0;
                                string available_qty = qty.ToString();
                                string category = myProductView["category"].ToString();
                                double unit_price = Convert.ToDouble(myProductView["unit_price"]);
                                double discount = 0; //Convert.ToDouble(myProductView["discount_value"]);
                                string btn_delete = "Del";
                                string location_code = myProductView["location_code"].ToString();
                                string unit = myProductView["unit"].ToString();
                                string tax_id = myProductView["tax_id"].ToString();
                                string item_number = myProductView["item_number"].ToString();


                                string[] row0 = { id.ToString(), code, name,  cost_price.ToString(), tax.ToString(), qty_sold.ToString(),
                                            available_qty,required_qty.ToString(),category,unit_price.ToString(), discount.ToString(),
                                sub_total.ToString(),location_code,unit,btn_delete,tax_id, tax_rate.ToString(),item_number    };

                                int RowIndex = grid_purchases_order.Rows.Add(row0);


                                get_total_tax();
                                get_total_discount();
                                get_sub_total_amount();
                                get_total_amount();
                                get_total_qty();

                                global_product_id = 0;
                                global_unit_price = 0;
                                global_tax_id = 0;
                                global_tax_rate = 0;
                            }
                        }
                        cmb_suppliers.Focus();

                    }
                }
                

                if (cmb_suppliers.SelectedValue.ToString() == "-1")
                {
                    frm_addSupplier custfrm = new frm_addSupplier(null);
                    custfrm.ShowDialog();

                    get_suppliers_dropdownlist();
                }
                    
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void get_employees_dropdownlist()
        {
            EmployeeBLL employeeBLL = new EmployeeBLL();
            DataTable employees = employeeBLL.GetAll();

            DataRow emptyRow = employees.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[2] = "Select Employee";              // Set Column Value
            employees.Rows.InsertAt(emptyRow, 0);

            cmb_employees.DisplayMember = "first_name";
            cmb_employees.ValueMember = "id";
            cmb_employees.DataSource = employees;


        }


        
        public void load_user_rights(int user_id)
        {
            UsersBLL userBLL_obj = new UsersBLL();
            DataTable users = userBLL_obj.GetUserRights(user_id);

            foreach (DataRow dr in users.Rows)
            {
                ///USER RIGHTS

                //cash_sales_amount_limit = Convert.ToDouble(dr["cash_sales_amount"]);
                //txt_credit_sales_amt= (double) dr["credit_sales_amount"].ToString();
                cash_purchase_amount_limit = Convert.ToDouble(dr["cash_purchase_amount"]);
                //txt_credit_purchase_amt.Text = (double) dr["credit_purchase_amount"].ToString();
                //chk_allow_cash_sales.Checked = (bool)dr["allow_cash_sales"];
                //allow_credit_sales = (bool)dr["allow_credit_sales"];
                //allow_cash_purchase = (bool)dr["allow_cash_purchase"];
                allow_credit_purchase = (bool)dr["allow_credit_purchase"];

            }
        }

       
        private void grid_purchases_order_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("Are you sure you want delete", "Delete", buttons, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        grid_purchases_order.Rows.RemoveAt(grid_purchases_order.CurrentRow.Index);

                        get_total_tax();
                        get_total_discount();
                        get_sub_total_amount();
                        get_total_amount();
                        get_total_qty();
                    }
                }


                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    e.SuppressKeyPress = true;
                    int iColumn = grid_purchases_order.CurrentCell.ColumnIndex;
                    int iRow = grid_purchases_order.CurrentCell.RowIndex;
                    
                    if (iColumn <= 6)
                    {
                        if (grid_purchases_order.Rows[iRow].Cells["code"].Value != null && grid_purchases_order.Rows[iRow].Cells["avg_cost"].Value != null && grid_purchases_order.Rows[iRow].Cells["qty"].Value != null)
                        {
                            grid_purchases_order.CurrentCell = grid_purchases_order.Rows[iRow].Cells[iColumn + 1];
                            grid_purchases_order.Focus();
                            grid_purchases_order.CurrentCell.Selected = true;
                            
                        }
                    }
                    else if (iColumn > 6)
                    {
                        if (grid_purchases_order.Rows[iRow].Cells["code"].Value != null && grid_purchases_order.Rows[iRow].Cells["avg_cost"].Value != null && grid_purchases_order.Rows[iRow].Cells["qty"].Value != null)
                        {
                            grid_purchases_order.Rows.Add();  //adds new row on last cell of row
                            this.ActiveControl = grid_purchases_order;
                            grid_purchases_order.CurrentCell = grid_purchases_order.Rows[iRow + 1].Cells[1];
                            grid_purchases_order.CurrentCell.Selected = true;
                            grid_purchases_order.BeginEdit(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }

        private void grid_purchases_order_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(tb_KeyPress);

            if (grid_purchases_order.CurrentCell.ColumnIndex == 7) //required qty Column will accept only numeric
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
                }
            }
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btn_product_purchase_history_Click(object sender, EventArgs e)
        {
            
        }

        private void grid_purchases_order_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //grid_purchases_order.Rows[e.RowIndex].Cells["sno"].Value = (e.RowIndex + 1).ToString();
            using (SolidBrush b = new SolidBrush(grid_purchases_order.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void SetupBrandDataGridView()
        {
            var current_lang_code = System.Globalization.CultureInfo.CurrentCulture;

            brandsDataGridView.ColumnCount = 2;
            int xLocation = groupBox_products.Location.X + txt_brands.Location.X;
            int yLocation = groupBox_products.Location.Y + txt_brands.Location.Y + 22;

            brandsDataGridView.Name = "brandsDataGridView";
            if (lang == "en-US")
            {
                brandsDataGridView.Location = new Point(xLocation, yLocation);
                brandsDataGridView.Size = new Size(250, 250);
            }
            else if (lang == "ar-SA")
            {
                brandsDataGridView.Location = new Point(xLocation, yLocation);
                brandsDataGridView.Size = new Size(250, 250);
            }

            brandsDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            brandsDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            brandsDataGridView.Columns[0].Name = "Code";
            brandsDataGridView.Columns[1].Name = "Name";
            brandsDataGridView.Columns[0].ReadOnly = true;
            brandsDataGridView.Columns[1].ReadOnly = true;
            brandsDataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            brandsDataGridView.MultiSelect = false;
            brandsDataGridView.AllowUserToAddRows = false;
            brandsDataGridView.AllowUserToDeleteRows = false;

            brandsDataGridView.RowHeadersVisible = false;
            //brandsDataGridView.ColumnHeadersVisible = false;
            brandsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            brandsDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            brandsDataGridView.AutoResizeColumns();

            brandsDataGridView.CellClick += new DataGridViewCellEventHandler(brandsDataGridView_CellClick);
            this.brandsDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(brandsDataGridView_KeyDown);

            this.Controls.Add(brandsDataGridView);
            brandsDataGridView.BringToFront();

        }

        void brandsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.Controls.Remove(brandsDataGridView);
                grid_purchases_order.Focus();
            }
        }

        private void brandsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(brandsDataGridView);
            grid_purchases_order.Focus();

        }

        private void txt_brands_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_brands.Text != "")
                {
                    SetupBrandDataGridView();

                    BrandsBLL brandsBLL_obj = new BrandsBLL();
                    string brand_name = txt_brands.Text;

                    DataTable dt = brandsBLL_obj.SearchRecord(brand_name);

                    if (dt.Rows.Count > 0)
                    {
                        brandsDataGridView.Rows.Clear();
                        foreach (DataRow dr in dt.Rows)
                        {
                            string code = dr["code"].ToString();
                            string name = dr["name"].ToString();

                            string[] row0 = { code, name };

                            brandsDataGridView.Rows.Add(row0);
                        }
                        //brandsDataGridView.CurrentCell = brandsDataGridView.Rows[0].Cells[0];
                        brandsDataGridView.ClearSelection();
                        brandsDataGridView.CurrentCell = null;
                    }

                }
                else
                {
                    txt_brand_code.Text = "";
                    this.Controls.Remove(brandsDataGridView);
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_brands_Leave(object sender, EventArgs e)
        {
            if (!brandsDataGridView.Focused)
            {
                this.Controls.Remove(brandsDataGridView);
            }
        }

        private void SetupCategoriesDataGridView()
        {
            var current_lang_code = System.Globalization.CultureInfo.CurrentCulture;
            categoriesDataGridView.ColumnCount = 2;
            int xLocation = groupBox_products.Location.X + txt_categories.Location.X + 5;
            int yLocation = groupBox_products.Location.Y + txt_categories.Location.Y + 22;

            categoriesDataGridView.Name = "categoriesDataGridView";
            if (lang == "en-US")
            {
                categoriesDataGridView.Location = new Point(xLocation, yLocation);
                categoriesDataGridView.Size = new Size(250, 250);
            }
            else if (lang == "ar-SA")
            {
                categoriesDataGridView.Location = new Point(xLocation, yLocation);
                categoriesDataGridView.Size = new Size(250, 250);
            }

            categoriesDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            categoriesDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            categoriesDataGridView.Columns[0].Name = "Code";
            categoriesDataGridView.Columns[1].Name = "Name";
            categoriesDataGridView.Columns[0].ReadOnly = true;
            categoriesDataGridView.Columns[1].ReadOnly = true;
            categoriesDataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            categoriesDataGridView.MultiSelect = false;
            categoriesDataGridView.AllowUserToAddRows = false;
            categoriesDataGridView.AllowUserToDeleteRows = false;

            categoriesDataGridView.RowHeadersVisible = false;
            //brandsDataGridView.ColumnHeadersVisible = false;
            categoriesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            categoriesDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            categoriesDataGridView.AutoResizeColumns();

            this.categoriesDataGridView.CellClick += new DataGridViewCellEventHandler(categoriesDataGridView_CellClick);
            this.categoriesDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(categoriesDataGridView_KeyDown);

            this.Controls.Add(categoriesDataGridView);
            categoriesDataGridView.BringToFront();

        }

        void categoriesDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                txt_category_code.Text = categoriesDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_categories.Text = categoriesDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.Controls.Remove(categoriesDataGridView);
                grid_purchases_order.Focus();

            }
        }

        private void categoriesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_category_code.Text = categoriesDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_categories.Text = categoriesDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(categoriesDataGridView);
            grid_purchases_order.Focus();

        }

        private void txt_categories_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_categories.Text != "")
                {
                    SetupCategoriesDataGridView();

                    CategoriesBLL brandsBLL_obj = new CategoriesBLL();
                    string category_name = txt_categories.Text;

                    DataTable dt = brandsBLL_obj.SearchRecord(category_name);

                    if (dt.Rows.Count > 0)
                    {
                        categoriesDataGridView.Rows.Clear();
                        foreach (DataRow dr in dt.Rows)
                        {
                            string code = dr["code"].ToString();
                            string name = dr["name"].ToString();

                            string[] row0 = { code, name };

                            categoriesDataGridView.Rows.Add(row0);
                        }
                        //categoriesDataGridView.CurrentCell = categoriesDataGridView.Rows[0].Cells[0];
                        categoriesDataGridView.ClearSelection();
                        categoriesDataGridView.CurrentCell = null;
                    }

                }
                else
                {
                    txt_category_code.Text = "";
                    this.Controls.Remove(categoriesDataGridView);
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_categories_Leave(object sender, EventArgs e)
        {
            if (!categoriesDataGridView.Focused)
            {
                this.Controls.Remove(categoriesDataGridView);
            }

        }

        

        private void SetupGroupsDataGridView()
        {
            var current_lang_code = System.Globalization.CultureInfo.CurrentCulture;
            groupsDataGridView.ColumnCount = 2;
            groupsDataGridView.Name = "groupsDataGridView";
            int xLocation = groupBox_products.Location.X + txt_groups.Location.X;
            int yLocation = groupBox_products.Location.Y + txt_groups.Location.Y + 22;

            groupsDataGridView.Name = "groupsDataGridView";
            if (lang == "en-US")
            {
                groupsDataGridView.Location = new Point(xLocation, yLocation);
                groupsDataGridView.Size = new Size(250, 250);
            }
            else if (lang == "ar-SA")
            {
                groupsDataGridView.Location = new Point(xLocation, yLocation);
                groupsDataGridView.Size = new Size(250, 250);
            }

            groupsDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            groupsDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            groupsDataGridView.Columns[0].Name = "Code";
            groupsDataGridView.Columns[1].Name = "Name";
            groupsDataGridView.Columns[0].ReadOnly = true;
            groupsDataGridView.Columns[1].ReadOnly = true;
            groupsDataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            groupsDataGridView.MultiSelect = false;
            groupsDataGridView.AllowUserToAddRows = false;
            groupsDataGridView.AllowUserToDeleteRows = false;

            groupsDataGridView.RowHeadersVisible = false;
            //brandsDataGridView.ColumnHeadersVisible = false;
            groupsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            groupsDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            groupsDataGridView.AutoResizeColumns();

            this.groupsDataGridView.CellClick += new DataGridViewCellEventHandler(groupsDataGridView_CellClick);
            this.groupsDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(groupsDataGridView_KeyDown);

            this.Controls.Add(groupsDataGridView);
            groupsDataGridView.BringToFront();

        }

        void groupsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
                txt_group_code.Text = groupsDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_groups.Text = groupsDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.Controls.Remove(groupsDataGridView);
                grid_purchases_order.Focus();
                e.Handled = true;
            }
        }

        private void groupsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_group_code.Text = groupsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_groups.Text = groupsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(groupsDataGridView);
            grid_purchases_order.Focus();

        }

        private void txt_groups_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_groups.Text != "")
                {
                    SetupGroupsDataGridView();

                    ProductGroupsBLL pg_BLL_obj = new ProductGroupsBLL();
                    string grp_name = txt_groups.Text;

                    DataTable dt = pg_BLL_obj.SearchRecordByName(grp_name);

                    if (dt.Rows.Count > 0)
                    {
                        groupsDataGridView.Rows.Clear();
                        foreach (DataRow dr in dt.Rows)
                        {
                            string code = dr["code"].ToString();
                            string name = dr["name"].ToString();

                            string[] row0 = { code, name };

                            groupsDataGridView.Rows.Add(row0);
                        }
                        //groupsDataGridView.CurrentCell = groupsDataGridView.Rows[0].Cells[0];
                        groupsDataGridView.ClearSelection();
                        groupsDataGridView.CurrentCell = null;
                    }

                }
                else
                {
                    txt_group_code.Text = "";
                    this.Controls.Remove(groupsDataGridView);
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_groups_Leave(object sender, EventArgs e)
        {
            if (!groupsDataGridView.Focused)
            {
                this.Controls.Remove(groupsDataGridView);
            }

        }

        private void btn_search_purchase_orders_Click(object sender, EventArgs e)
        {
            
        }

         
        public void Load_products_to_grid_by_invoiceno(DataTable _dt, string invoice_no)
        {
            try
            {
                grid_purchases_order.Rows.Clear();
                grid_purchases_order.Refresh();
                txt_invoice_no.Text = invoice_no;
                SearchBySupplierCheckbox.Checked = false;

                invoice_status = "Update";
                
                //btn_save.Text = "Update (F3)";
                po_invoice_no = invoice_no;

                if (_dt.Rows.Count > 0)
                {

                    foreach (DataRow myProductView in _dt.Rows)
                    {
                        cmb_suppliers.SelectedValue = myProductView["supplier_id"];
                        cmb_employees.SelectedValue = myProductView["employee_id"];
                        //cmb_purchase_type.SelectedValue = myProductView["purchase_type"];
                        txt_purchase_date.Value = Convert.ToDateTime(myProductView["purchase_date"].ToString());
                        txt_description.Text = myProductView["description"].ToString();

                        double qty = Convert.ToDouble(myProductView["quantity"].ToString());
                        double total = qty * double.Parse(myProductView["cost_price"].ToString());
                        double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                        double tax = (total * tax_rate / 100);
                        double current_sub_total = tax + total;

                        int id = Convert.ToInt32(myProductView["id"]);
                        string code = myProductView["item_code"].ToString();
                        string name = myProductView["name"].ToString();
                        string available_qty = "1";
                        string qty_sold = "0";
                        double cost_price = Convert.ToDouble(myProductView["cost_price"]);
                        double unit_price = Convert.ToDouble(myProductView["unit_price"]);
                        double discount = Convert.ToDouble(myProductView["discount_value"]); ;
                        string location_code = ""; // myProductView["location_code"].ToString();
                        string unit = myProductView["unit"].ToString();
                        string category = myProductView["category"].ToString();
                        string item_type = myProductView["item_type"].ToString();
                        string btn_delete = "Del";
                        string tax_id = myProductView["tax_id"].ToString();
                        string item_number = myProductView["item_number"].ToString();

                        string[] row0 = { id.ToString(), code, name,  cost_price.ToString(), tax.ToString(), qty_sold,
                                            available_qty,qty.ToString(),category,unit_price.ToString(), discount.ToString(),
                            current_sub_total.ToString(),location_code,unit,btn_delete,
                            tax_id.ToString(), tax_rate.ToString(),item_number };
                        int RowIndex = grid_purchases_order.Rows.Add(row0);

                        //GET / SET Location Dropdown list
                        //fill_locations_grid_combo(RowIndex);
                        //////
                    }
                    //grid_purchases_order.Rows.Add();
                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_amount();
                    get_total_qty();
                    grid_purchases_order.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void load_product_purchase_history(string product_code)
        {
            try
            {
                
                grid_product_history.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_product_history.AutoGenerateColumns = false;

                String keyword = "TOP 100 I.id,P.name AS product_name,I.item_code,I.qty,I.unit_price,I.cost_price,I.invoice_no,I.description,trans_date, S.first_name AS supplier";
                String table = "pos_inventory I LEFT JOIN pos_products P ON P.code=I.item_code LEFT JOIN pos_suppliers S ON S.id=I.supplier_id WHERE I.item_code = '" + product_code + "' AND I.description = 'Purchase' AND I.branch_id=" + UsersModal.logged_in_branch_id + " ORDER BY I.id DESC";
                grid_product_history.DataSource = objBLL.GetRecord(keyword, table);

                    
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void grid_purchases_order_SelectionChanged(object sender, EventArgs e)
        {

            if (grid_purchases_order.Rows.Count > 0 && grid_purchases_order.Focused)
            {
                string product_code = (grid_purchases_order.CurrentRow.Cells["code"].Value != null ? grid_purchases_order.CurrentRow.Cells["code"].Value.ToString() : "");
                if (product_code != "")
                {
                    if (grid_product_history.Rows.Count > 0) // if history grid is empty then load product history 
                    {
                        //if product history grid has already same product the don not load but 
                        //if history gird has different product the load
                        //it will improve performance
                        if (product_code != grid_product_history.CurrentRow.Cells["item_code"].Value.ToString())
                        {
                            load_product_purchase_history(product_code);
                        }
                    }
                    else
                    {
                        load_product_purchase_history(product_code);
                    }
                }

            }
        }

        private void NewToolStripButton_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want new sale transaction", "New Transaction", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                clear_form();
            }
            this.ActiveControl = grid_purchases_order;
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to order " + invoice_status, "Purchase Order Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (grid_purchases_order.Rows.Count > 0)
                    {
                        List<Purchases_orderModal> Purchases_orderModal_obj = new List<Purchases_orderModal> { };
                        List<PurchaseOrderDetailModal> po_model_detail = new List<PurchaseOrderDetailModal> { };

                        Purchases_orderBLL purchases_orderObj = new Purchases_orderBLL();
                        DateTime purchase_date = txt_purchase_date.Value.Date;
                        DateTime delivery_date = txt_delivery_date.Value.Date;

                        int supplier_id = (cmb_suppliers.SelectedValue == null ? 0 : int.Parse(cmb_suppliers.SelectedValue.ToString()));
                        int employee_id = (cmb_employees.SelectedValue == null ? 0 : int.Parse(cmb_employees.SelectedValue.ToString()));

                        string invoice_no = "";

                        if (invoice_status == "Update") //Update sales delete all record first and insert new sales
                        {
                            purchases_orderObj.DeletePurchasesOrder(txt_invoice_no.Text); //DELETE ALL TRANSACTIONS
                            invoice_no = txt_invoice_no.Text;
                        }
                        else
                        {
                            invoice_no = GetMAXInvoiceNo();
                        }

                        //set the date from datetimepicker and set time to te current time
                        DateTime now = DateTime.Now;
                        txt_purchase_date.Value = new DateTime(txt_purchase_date.Value.Year, txt_purchase_date.Value.Month, txt_purchase_date.Value.Day, now.Hour, now.Minute, now.Second);
                        /////////////////////
                        ///
                        
                        /////Add sales header into the List
                        Purchases_orderModal_obj.Add(new Purchases_orderModal
                        {
                            employee_id = employee_id,
                            supplier_id = supplier_id,
                            supplier_invoice_no = "",
                            invoice_no = invoice_no,
                            total_amount = total_amount,
                            purchase_type = "Purchase Order", //cmb_purchase_type.SelectedValue.ToString(),
                            total_discount = total_discount,
                            total_tax = total_tax,
                            description = txt_description.Text,
                            purchase_date = purchase_date.ToString("yyyy-MM-dd"),
                            account = "Purchase Order",
                            delivery_date = delivery_date.ToString("yyyy-MM-dd"),
                            purchase_time = txt_purchase_date.Value,
                        });


                        
                        int sno = 0;
                        for (int i = 0; i < grid_purchases_order.Rows.Count; i++)
                        {
                            if (grid_purchases_order.Rows[i].Cells["id"].Value != null && grid_purchases_order.Rows[i].Cells["qty"].Value != null)
                            {
                                if (double.Parse(grid_purchases_order.Rows[i].Cells["qty"].Value.ToString()) > 0)
                                {
                                    double tax_rate = (grid_purchases_order.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases_order.Rows[i].Cells["tax_rate"].Value.ToString()));

                                    po_model_detail.Add(new PurchaseOrderDetailModal
                                    {
                                        serialNo = sno++,
                                        invoice_no = invoice_no,
                                        supplier_id = supplier_id,
                                        purchase_date = purchase_date.ToString("yyyy-MM-dd"),
                                        item_number = grid_purchases_order.Rows[i].Cells["item_number"].Value.ToString(),
                                        code = grid_purchases_order.Rows[i].Cells["code"].Value.ToString(),
                                        //name = grid_purchases_order.Rows[i].Cells["name"].Value.ToString(),
                                        quantity = double.Parse(grid_purchases_order.Rows[i].Cells["qty"].Value.ToString()),
                                        cost_price = Convert.ToDouble(grid_purchases_order.Rows[i].Cells["avg_cost"].Value.ToString()),
                                        unit_price = double.Parse(grid_purchases_order.Rows[i].Cells["unit_price"].Value.ToString()),
                                        discount = double.Parse(grid_purchases_order.Rows[i].Cells["discount"].Value.ToString()),
                                        tax_id = Convert.ToInt32(grid_purchases_order.Rows[i].Cells["tax_id"].Value.ToString()),
                                        tax_rate = tax_rate,
                                    });

                                }
                            }

                        }
                        Int32 purchase_id = purchases_orderObj.InsertPurchaseOrderBLL(Purchases_orderModal_obj, po_model_detail);

                        if (purchase_id > 0)
                        {
                            MessageBox.Show(invoice_no+" Purchase Order created", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            clear_form();
                            GetMAXInvoiceNo();

                            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                            DialogResult result1 = MessageBox.Show("Print purchase order report", "Print Order Report", buttons, MessageBoxIcon.Warning);

                            if (result1 == DialogResult.Yes)
                            {
                                if (!string.IsNullOrEmpty(invoice_no))
                                {
                                    using (frm_purchase_order_report obj = new frm_purchase_order_report(invoice_no, false))
                                    {
                                        obj.ShowDialog();
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Purchase Order not created", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void SearchToolStripButton_Click(object sender, EventArgs e)
        {
            frm_search_porder obj = new frm_search_porder(null, this);
            obj.ShowDialog();
        }

        private void HistoryToolStripButton_Click(object sender, EventArgs e)
        {
            string product_code = "";

            if (grid_purchases_order.Rows.Count > 0 && grid_purchases_order.CurrentRow.Cells["code"].Value != null)
            {
                product_code = grid_purchases_order.CurrentRow.Cells["code"].Value.ToString();

                frm_productsMovements frm_prod_move_obj = new frm_productsMovements(product_code);
                frm_prod_move_obj.load_Products_grid();
                frm_prod_move_obj.ShowDialog();
            }

        }

        private void PurchaseHistoryToolStripButton_Click(object sender, EventArgs e)
        {
            //if (global_product_id == 0)
            //{
            //    if (grid_purchases_order.Rows.Count > 0)
            //    {
            //        global_product_id = Convert.ToInt32(grid_purchases_order.CurrentRow.Cells["id"].Value);
            //    }

            //}

            string product_code = "";

            if (grid_purchases_order.Rows.Count > 0 && grid_purchases_order.CurrentRow.Cells["code"].Value != null)
            {
                product_code = grid_purchases_order.CurrentRow.Cells["code"].Value.ToString();

                frm_purchase_product_history frm_prod_move_obj = new frm_purchase_product_history(product_code);

                frm_prod_move_obj.ShowDialog();

            }

            //global_product_id = 0;
        }

        private void Btn_load_products_Click(object sender, EventArgs e)
        {
            ProductBLL productsBLL_obj = new ProductBLL();
            bool is_zero = (Cmb_zero_qty.SelectedIndex == 1 ? true : false);
            
            var selectedBrands = GetSelectedBrandCodes();
            string brand_code = string.Join(",", selectedBrands); // comma-separated
            DataTable _dt = productsBLL_obj.GetProductsSummary(Start_date.Value.Date,End_date.Value.Date,is_zero,txt_group_code.Text.Trim(), brand_code, txt_category_code.Text.Trim());
            //grid_purchases_order.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            try
            {
                grid_purchases_order.Rows.Clear();
                grid_purchases_order.Refresh();

                if (_dt.Rows.Count > 0)
                {

                    foreach (DataRow myProductView in _dt.Rows)
                    {
                        double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                        double required_qty = 0;
                        double tax = (double.Parse(myProductView["avg_cost"].ToString()) * tax_rate / 100) * required_qty;
                        double sub_total = tax + double.Parse(myProductView["avg_cost"].ToString());

                        double qty = Convert.ToDouble(myProductView["qty_on_hand"].ToString());
                        double total = qty * double.Parse(myProductView["avg_cost"].ToString());
                        double current_sub_total = tax + total;

                        int id = Convert.ToInt32(myProductView["id"]);
                        string code = myProductView["code"].ToString();
                        string name = myProductView["name"].ToString();
                        double cost_price = Convert.ToDouble(myProductView["avg_cost"]);
                        double qty_sold = Math.Round(double.Parse(myProductView["qty_sold"].ToString()),3);
                        string available_qty = myProductView["qty_on_hand"].ToString();
                        string category = myProductView["category"].ToString();
                        double unit_price = Convert.ToDouble(myProductView["unit_price"]);
                        double discount = 0; //Convert.ToDouble(myProductView["discount_value"]);
                        string btn_delete = "Del";
                        string location_code = ""; // myProductView["location_code"].ToString();
                        string unit = ""; //myProductView["unit"].ToString();
                        string tax_id = myProductView["tax_id"].ToString();
                        string item_number = myProductView["item_number"].ToString();


                        string[] row0 = { id.ToString(), code, name,  cost_price.ToString(), tax.ToString(), qty_sold.ToString(),
                                            available_qty,required_qty.ToString(),category,unit_price.ToString(), discount.ToString(),
                            sub_total.ToString(),location_code,unit,btn_delete,tax_id,
                            tax_rate.ToString(),item_number    };

                        int RowIndex = grid_purchases_order.Rows.Add(row0);

                        //GET / SET Location Dropdown list
                        //fill_locations_grid_combo(RowIndex);
                        //////
                    }
                    //grid_purchases_order.Rows.Add();
                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_amount();
                    get_total_qty();
                    grid_purchases_order.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadBrandList(string search = "")
        {
            BrandsBLL brandsBLL = new BrandsBLL();

            // Store checked codes before reload
            HashSet<string> checkedCodes = new HashSet<string>();
            foreach (var item in chkListBrands.CheckedItems)
            {
                if (item is ListItem li)
                {
                    checkedCodes.Add(li.Code);
                }
            }

            DataTable dt = brandsBLL.SearchRecord(search);
            chkListBrands.Items.Clear();

            foreach (DataRow row in dt.Rows)
            {
                string name = row["name"].ToString();
                string code = row["code"].ToString();

                ListItem item = new ListItem(name, code);
                int index = chkListBrands.Items.Add(item);

                // Restore checkmark if previously selected
                if (checkedCodes.Contains(code))
                {
                    chkListBrands.SetItemChecked(index, true);
                }
            }
        }
        private List<string> GetSelectedBrandCodes()
        {
            List<string> selectedCodes = new List<string>();
            foreach (var item in chkListBrands.CheckedItems)
            {
                if (item is ListItem brand)
                {
                    selectedCodes.Add(brand.Code);
                }
            }
            return selectedCodes;
        }

        private void txtSearchBrand_KeyUp(object sender, KeyEventArgs e)
        {
            debounceTimer.Stop();  // Stop the timer once triggered

            string searchText = txtSearchBrand.Text.Trim();
            LoadBrandList(searchText);
        }

        private void debounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();   // Restart the timer
            debounceTimer.Start();
        }
    }
}
