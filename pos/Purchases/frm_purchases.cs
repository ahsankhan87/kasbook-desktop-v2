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

namespace pos
{
    public partial class frm_purchases : Form
    {
        public string lang = (UsersModal.logged_in_lang.Length > 0 ? UsersModal.logged_in_lang : "en-US");
        public int cash_account_id = 0;
        //public int sales_account_id = 0;
        public int payable_account_id = 0;
        public int tax_account_id = 0;
        public int purchases_discount_acc_id = 0;
        public int inventory_acc_id = 0;
        public int purchases_acc_id = 0;

        public static frm_purchases instance;
        public TextBox tb_product_id;
        public TextBox tb_code;
        public TextBox tb_product_name;
        public TextBox tb_qty;
        public TextBox tb_cost_price;

        public decimal total_amount = 0;
        public decimal total_tax = 0;
        public decimal total_discount = 0;
        public decimal total_sub_total = 0;

        public int global_product_id = 0;
        public int global_tax_id = 0;
        public decimal global_tax_rate = 0;
        public decimal global_unit_price = 0;
        public string global_location_code = "";
        public string global_unit = "";
        public string global_item_category = "";

        string invoice_status = "";
        string po_invoice_no = "";

        public double cash_purchase_amount_limit = 0;
        public bool allow_credit_purchase = false;

        private DataGridView brandsDataGridView = new DataGridView();
        private DataGridView categoriesDataGridView = new DataGridView();
        private DataGridView groupsDataGridView = new DataGridView();

        public DataTable products_dt = new DataTable();

        //private frm_searchProducts productsMainForm;

        //public frm_purchases(frm_searchProducts productsMainForm) : this()
        //{
        //    this.productsMainForm = productsMainForm;
        //}

        public frm_purchases()
        {
            InitializeComponent();
            //autoCompleteProductCode();

        }

        private void frm_purchases_Load(object sender, EventArgs e)
        {
            grid_purchases.Rows.Add();
            this.ActiveControl = grid_purchases;
            grid_purchases.CurrentCell = grid_purchases.Rows[0].Cells["code"];

            Get_AccountID_From_Company();
            load_user_rights(UsersModal.logged_in_userid);
            //txt_product_name.Text = tb_product_name.Text;
            get_purchasetype_dropdownlist();
            if (lang == "en-US")
            {
                cmb_purchase_type.SelectedValue = "Cash";
            }
            else if (lang == "ar-SA")
            {
                cmb_purchase_type.SelectedIndex = 0;
            }

            //btn_movements.Enabled = false;
            get_suppliers_dropdownlist();
            get_employees_dropdownlist();
            
            //GetMAXInvoiceNo();
            //disable sorting in grid
            foreach (DataGridViewColumn column in grid_purchases.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchProducts frm_search_product_obj = new frm_searchProducts();
            frm_search_product_obj.Show();

        }

        public string GetMAXInvoiceNo()
        {
            PurchasesBLL PurchasesBLL_obj = new PurchasesBLL();
            return PurchasesBLL_obj.GetMaxInvoiceNo();
        }

        public string GetMAXInvoiceNo_HOLD()
        {
            PurchasesBLL PurchasesBLL_obj = new PurchasesBLL();
            return PurchasesBLL_obj.GetMaxInvoiceNo_HOLD();
        }

        Form purchaseSearchObj;
        private void grid_purchases_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //int iColumn = grid_purchases.CurrentCell.ColumnIndex;
                //int iRow = grid_purchases.CurrentCell.RowIndex;
                string columnName = grid_purchases.Columns[e.ColumnIndex].Name;
                if (columnName == "code")
                {
                    string product_code = (grid_purchases.CurrentRow.Cells["code"].Value != null ? grid_purchases.CurrentRow.Cells["code"].Value.ToString() : "");
                    bool isGrid = true;
                    var brand_code = txt_brand_code.Text; // (cmb_brands.SelectedValue != null ? cmb_brands.SelectedValue.ToString() : "");
                    var category_code = txt_category_code.Text; // (cmb_categories.SelectedValue != null ? cmb_categories.SelectedValue.ToString() : "");
                    var group_code = txt_group_code.Text; // (cmb_groups.SelectedValue != null ? cmb_groups.SelectedValue.ToString() : "");

                    ////////////////////////
                    if (purchaseSearchObj == null || product_code != "")
                    {
                        purchaseSearchObj = new frm_searchPurchaseProducts(this, null, product_code, category_code, brand_code, e.RowIndex, isGrid, group_code);
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

                }

                if (e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6)
                {
                    var cell = grid_purchases.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                    {
                        cell.Value = 0; // Set default value to 0 if null or empty
                    }
                }

                if (columnName == "Qty") // if qty is changed
                {
                    double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                    double tax = (Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * tax_rate / 100);

                    grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = (tax * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value));
                    double sub_total = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                    grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total;
                }
                if (columnName == "avg_cost")//if avg_cost is changed
                {
                    if (rd_btn_without_vat.Checked && rd_btn_by_unitprice.Checked)
                    {
                        double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                        double tax = (Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * tax_rate / 100);
                        grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = (tax * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value));

                        double sub_total = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                        grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total;
                    }

                    if (rd_btn_without_vat.Checked && rd_btn_bytotal_price.Checked)
                    {
                        double total_avg_cost_1 = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value);
                        double qty_1 = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) * 1;
                        double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                        string pre_tax = "1." + tax_rate.ToString();
                        double single_avg_cost = (total_avg_cost_1 / qty_1);
                        double tax = (single_avg_cost * tax_rate / 100);
                        double new_tax_value = (tax * qty_1);

                        grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = single_avg_cost;

                        grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = new_tax_value;

                        double sub_total = ((single_avg_cost * qty_1) + new_tax_value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value); 
                        
                        grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total;
                    }

                    if (rd_btn_with_vat.Checked && rd_btn_by_unitprice.Checked)
                    {
                        double total_avg_cost = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value);
                        double qty = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) * 1;
                        double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                        string pre_tax = "1." + tax_rate.ToString();
                        double single_avg_cost = (total_avg_cost / double.Parse(pre_tax));
                        double tax = (single_avg_cost * tax_rate / 100);
                        double new_tax_value = (tax * qty);
                        grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = single_avg_cost;

                        grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = new_tax_value;

                        double sub_total = ((single_avg_cost * qty) + new_tax_value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);

                        //grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = (Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) - tax);
                        grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total;
                    }

                    if (rd_btn_with_vat.Checked && rd_btn_bytotal_price.Checked)
                    {
                        double total_avg_cost_1 = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value);
                        double qty_1 = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) * 1;
                        double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                        string pre_tax = "1." + tax_rate.ToString();
                        double single_avg_cost = ((total_avg_cost_1 / double.Parse(pre_tax)) / qty_1);
                        double tax = (single_avg_cost * tax_rate / 100);
                        double new_tax_value = (tax * qty_1);

                        grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = single_avg_cost;

                        grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = new_tax_value;

                        double sub_total = ((single_avg_cost * qty_1) + new_tax_value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                        grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total;
                    }
                }

                if (columnName == "discount")//if discount is changed
                {
                    //double sub_total = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);

                    double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                    double total_value = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value);
                    double tax_1 = ((total_value - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value)) * tax_rate / 100);
                    double sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                    //total_discount += Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                    //txt_total_discount.Text = total_amount.ToString();
                    grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = (tax_1);
                    grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total_1;
                }

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

        public void Load_products_to_grid(string product_id, int RowIndex1)
        {
            ProductBLL productsBLL_obj = new ProductBLL();
            products_dt = productsBLL_obj.SearchRecordByProductID(product_id);
            //grid_purchases.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            int RowIndex = grid_purchases.CurrentCell.RowIndex;

            if (products_dt.Rows.Count > 0)
            {
                
                for (int i = 0; i < grid_purchases.RowCount; i++)
                {
                    var item_code = (grid_purchases.Rows[i].Cells["id"].Value != null ? grid_purchases.Rows[i].Cells["id"].Value : "");
                    if (item_code.ToString() == product_id)
                    {
                        MessageBox.Show("Product already added", "Already exist", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        //DataView dvProducts = products_dt.DefaultView;
                        //dvProducts.RowFilter = ""
                        grid_purchases.Focus();
                        grid_purchases.CurrentCell = grid_purchases.Rows[RowIndex].Cells["code"]; //make qty cell active
                        grid_purchases.CurrentCell.Selected = true;
                        grid_purchases.BeginEdit(true);
                        return;
                    }
                    else
                    {
                        grid_purchases.CurrentCell = grid_purchases.Rows[RowIndex].Cells["qty"]; //make qty cell active
                        grid_purchases.CurrentCell.Selected = true;

                    }
                }
               
                foreach (DataRow myProductView in products_dt.Rows)
                {
                    double avg_cost = (myProductView["avg_cost"].ToString() != "" ? double.Parse(myProductView["avg_cost"].ToString()) : 0);
                    double unit_price = (myProductView["unit_price"].ToString() != "" ? double.Parse(myProductView["unit_price"].ToString()) : 0);
                    double qty = Convert.ToDouble(myProductView["purchase_demand_qty"].ToString() == string.Empty || (decimal)myProductView["purchase_demand_qty"] == 0 ? "1" : myProductView["purchase_demand_qty"].ToString());
                    double total = qty * avg_cost;
                    double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                    double tax = (total * tax_rate / 100);
                    double sub_total = tax + total;


                    grid_purchases.Rows[RowIndex].Cells["id"].Value = myProductView["id"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["code"].Value = myProductView["code"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["name"].Value = myProductView["name"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["qty"].Value = qty;
                    grid_purchases.Rows[RowIndex].Cells["avg_cost"].Value = avg_cost;
                    grid_purchases.Rows[RowIndex].Cells["unit_price"].Value = unit_price;
                    grid_purchases.Rows[RowIndex].Cells["discount"].Value = 0.00;
                    grid_purchases.Rows[RowIndex].Cells["tax"].Value = tax;
                    grid_purchases.Rows[RowIndex].Cells["sub_total"].Value = sub_total;
                    grid_purchases.Rows[RowIndex].Cells["location_code"].Value = myProductView["location_code"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["unit"].Value = myProductView["unit"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["category"].Value = myProductView["category"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["btn_delete"].Value = "Del";
                    grid_purchases.Rows[RowIndex].Cells["tax_id"].Value = myProductView["tax_id"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["tax_rate"].Value = myProductView["tax_rate"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["shop_qty"].Value = myProductView["qty"].ToString();
                    /////
                    //fill_locations_grid_combo(RowIndex);
                    ////
                }
                get_total_tax();
                get_total_discount();
                get_sub_total_amount();
                get_total_amount();
                get_total_qty();
            }
        }

        private void fill_locations_grid_combo(int RowIndex)
        {
            //var locationComboCell = new DataGridViewComboBoxCell();
            //DataTable dt = new DataTable();

            //GeneralBLL generalBLL_obj = new GeneralBLL();
            //string keyword = "code as location_code,name";
            //string table = "pos_locations";

            //dt = generalBLL_obj.GetRecord(keyword, table);

            //locationComboCell.DataSource = dt;
            //locationComboCell.DisplayMember = "location_code";
            //locationComboCell.ValueMember = "location_code";

            //grid_purchases.Rows[RowIndex].Cells["location_code"] = locationComboCell;
            //grid_purchases.Rows[RowIndex].Cells["location_code"].Value = dt.Rows[0]["location_code"].ToString(); // GET FIRST COLUMN OF DT TO SHOW FIRST VALUE AS SELECTED

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
           
        }
        
        public bool checkNullOrEmpty(string keyword)
        {
            if (keyword == null || keyword == DBNull.Value.ToString() || String.IsNullOrEmpty(keyword)  || String.IsNullOrWhiteSpace(keyword.ToString()))
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        private void hold_purchases()
        {
            try
            {
               
                DialogResult result = MessageBox.Show("Are you sure you want to hold purchase", "Hold Purchase Transaction " + invoice_status, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (grid_purchases.Rows.Count > 0)
                    {
                        PurchasesModal PurchasesModal_obj = new PurchasesModal();
                        PurchasesBLL purchasesObj = new PurchasesBLL();
                        DateTime purchase_date = txt_purchase_date.Value.Date;
                        int employee_id = (cmb_employees.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_employees.SelectedValue.ToString()));
                        int supplier_id = (cmb_suppliers.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_suppliers.SelectedValue.ToString()));
                
                        string invoice_no = "";

                        
                        invoice_no = GetMAXInvoiceNo_HOLD();
                        
                        PurchasesModal_obj.employee_id = employee_id;
                        PurchasesModal_obj.supplier_id = supplier_id;
                        PurchasesModal_obj.supplier_invoice_no = txt_supplier_invoice.Text;
                        PurchasesModal_obj.invoice_no = invoice_no;
                        PurchasesModal_obj.total_amount = Math.Round(total_amount, 4);
                        PurchasesModal_obj.purchase_type = cmb_purchase_type.SelectedValue.ToString();
                        PurchasesModal_obj.total_discount = Math.Round(total_discount, 4);
                        PurchasesModal_obj.total_tax = Math.Round(total_tax, 4);
                        PurchasesModal_obj.description = txt_description.Text;
                        PurchasesModal_obj.purchase_date = purchase_date;
                        PurchasesModal_obj.account = "Purchase";

                        //set the date from datetimepicker and set time to te current time
                        DateTime now = DateTime.Now;
                        txt_purchase_date.Value = new DateTime(txt_purchase_date.Value.Year, txt_purchase_date.Value.Month, txt_purchase_date.Value.Day, now.Hour, now.Minute, now.Second);
                        /////////////////////
                        PurchasesModal_obj.purchase_time = txt_purchase_date.Value;

                        Int32 purchase_id = purchasesObj.Insert_hold_purchases(PurchasesModal_obj);
                        int sno = 1;
                        for (int i = 0; i < grid_purchases.Rows.Count; i++)
                        {
                            if (grid_purchases.Rows[i].Cells["id"].Value != null)
                            {
                                PurchasesModal_obj.serialNo = sno++;
                                PurchasesModal_obj.purchase_id = purchase_id;
                                PurchasesModal_obj.code = grid_purchases.Rows[i].Cells["code"].Value.ToString();
                                //PurchasesModal_obj.name = grid_purchases.Rows[i].Cells["name"].Value.ToString();
                                PurchasesModal_obj.quantity = decimal.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString());
                                PurchasesModal_obj.cost_price = Math.Round(Convert.ToDecimal(grid_purchases.Rows[i].Cells["avg_cost"].Value.ToString()), 4);
                                PurchasesModal_obj.unit_price =Math.Round(decimal.Parse(grid_purchases.Rows[i].Cells["unit_price"].Value.ToString()), 4);
                                PurchasesModal_obj.discount = Math.Round(decimal.Parse(grid_purchases.Rows[i].Cells["discount"].Value.ToString()), 4);
                                PurchasesModal_obj.tax_id = Convert.ToInt32(grid_purchases.Rows[i].Cells["tax_id"].Value.ToString());
                                decimal tax_rate = (grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : decimal.Parse(grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString()));
                                PurchasesModal_obj.tax_rate = (chkbox_is_taxable.Checked ? Math.Round(tax_rate, 4) : 0);
                                if (grid_purchases.Rows[i].Cells["location_code"].Value == null || grid_purchases.Rows[i].Cells["location_code"].Value == DBNull.Value || String.IsNullOrEmpty(grid_purchases.Rows[i].Cells["location_code"].Value as String) || String.IsNullOrWhiteSpace(grid_purchases.Rows[i].Cells["location_code"].Value.ToString()))
                                {
                                    PurchasesModal_obj.location_code = "";
                                }
                                else
                                {
                                    PurchasesModal_obj.location_code = grid_purchases.Rows[i].Cells["location_code"].Value.ToString();
                                }

                                purchasesObj.Insert_hold_purchasesItems(PurchasesModal_obj);
                            }

                        }
                        if (purchase_id > 0)
                        {
                            MessageBox.Show(invoice_no + " hold purchases transaction successfully", "Success "+invoice_status, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            clear_form();
                            //GetMAXInvoiceNo_HOLD();

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

        private void get_sub_total_amount()
        {
            total_sub_total = 0;

            for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
            {
                total_sub_total += Convert.ToDecimal(grid_purchases.Rows[i].Cells["qty"].Value) * Convert.ToDecimal(grid_purchases.Rows[i].Cells["avg_cost"].Value);
            }

            txt_sub_total.Text = Math.Round(total_sub_total,2).ToString();
        }

        private void get_total_amount()
        {
            total_amount = 0;

            for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
            {
                total_amount += Convert.ToDecimal(grid_purchases.Rows[i].Cells["qty"].Value) * Convert.ToDecimal(grid_purchases.Rows[i].Cells["avg_cost"].Value);
            }
            decimal net = (total_amount + total_tax - total_discount);
            txt_total_amount.Text = Math.Round(net,2).ToString();
        }

        private void get_total_tax()
        {
            total_tax = 0;

            for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
            {
                total_tax += Convert.ToDecimal(grid_purchases.Rows[i].Cells["tax"].Value);
            }

            txt_total_tax.Text =Math.Round(total_tax,2).ToString();
        }
        private void get_total_discount()
        {
            total_discount = 0;

            for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
            {
                total_discount += Convert.ToDecimal(grid_purchases.Rows[i].Cells["discount"].Value);
            }

            txt_total_discount.Text = Math.Round(total_discount,2).ToString();
        }

        private void get_total_qty()
        {
            double total_qty = 0;

            for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
            {
                total_qty += Convert.ToDouble(grid_purchases.Rows[i].Cells["qty"].Value);
            }

            txt_total_qty.Text = (total_qty).ToString();
            
        }
        
        private int Insert_Journal_entry(string invoice_no, int account_id, decimal debit, decimal credit, DateTime date,
            string description, int customer_id, int supplier_id, int entry_id)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            JournalsBLL JournalsObj = new JournalsBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = Convert.ToDouble(debit);
            JournalsModal_obj.credit = Convert.ToDouble(credit);
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
                purchases_discount_acc_id = (int)dr["purchases_discount_acc_id"];
                inventory_acc_id = (int)dr["inventory_acc_id"];
                purchases_acc_id = (int)dr["purchases_acc_id"];
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
        public void load_products(string product_id = "", string product_name = "", string barcode = "")
        {

            ProductBLL productsBLL_obj = new ProductBLL();
            //DataTable product_dt = productsBLL_obj.SearchRecordByProductCode(txt_product_code.Text);
            DataTable product_dt = new DataTable();

            if (product_id != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByProductID(product_id);
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
                    double qty = Convert.ToDouble(myProductView["purchase_demand_qty"].ToString() == string.Empty || (decimal)myProductView["purchase_demand_qty"] == 0 ? "1" : myProductView["purchase_demand_qty"].ToString());
                    double total = qty * double.Parse(myProductView["avg_cost"].ToString());
                    double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                    double tax = (total * tax_rate / 100);
                    double current_sub_total = tax + total;

                    int id = Convert.ToInt32(myProductView["id"]);
                    string code = myProductView["code"].ToString();
                    string name = myProductView["name"].ToString();
                    //string qty= (myProductView["purchase_demand_qty"].ToString() == string.Empty || (decimal)myProductView["purchase_demand_qty"] == 0 ? "1" : myProductView["purchase_demand_qty"].ToString());
                    double avg_cost = Convert.ToDouble(myProductView["avg_cost"]);
                    double unit_price = Convert.ToDouble(myProductView["unit_price"]);
                    double discount = 0.00;
                    string location_code = myProductView["location_code"].ToString();
                    string unit = myProductView["unit"].ToString();
                    string category = myProductView["category"].ToString();
                    string btn_delete = "Del";
                    string tax_id = myProductView["tax_id"].ToString();
                    string shop_qty = myProductView["qty"].ToString();

                    string[] row0 = { id.ToString(), code, name, 
                                            qty.ToString(), avg_cost.ToString(),unit_price.ToString(), discount.ToString(), 
                                            tax.ToString(), current_sub_total.ToString(),location_code,unit,category, 
                                            btn_delete, tax_id.ToString(), tax_rate.ToString(), unit_price.ToString(),shop_qty };
                    //Remove the first empty row
                    if (grid_purchases.RowCount > 0 && grid_purchases.Rows[0].Cells["id"].Value == null)
                    {
                        grid_purchases.Rows.RemoveAt(0);
                    }
                    //
                    int RowIndex = grid_purchases.Rows.Add(row0);

                    //GET / SET Location Dropdown list
                    /////
                    //fill_locations_grid_combo(RowIndex);
                    //////////

                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_amount();
                    get_total_qty();
                }

                txt_barcode.Focus();

            }
            else
            {

                MessageBox.Show("Record not found", "Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //txt_product_code.Focus();
                txt_barcode.Focus();
            }


        }


        private void frm_purchases_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                if (e.KeyData == Keys.Enter)
                {
                    //SendKeys.Send("{TAB}");
                }
                if (e.KeyData == Keys.F1)
                {
                    NewToolStripButton.PerformClick();
                }
                if (e.KeyData == Keys.F3)
                {
                    SaveToolStripButton.PerformClick();
                }
                if (e.KeyData == Keys.F4)
                {
                    SearchToolStripButton.PerformClick();
                }
                if (e.Control && e.KeyCode == Keys.H)
                {
                    HistoryToolStripButton.PerformClick();
                }
                if (e.KeyCode == Keys.F9)
                {
                    grid_purchases.Focus();
                }
                if (e.Control && e.KeyCode == Keys.L)
                {
                    frm_search_porder obj = new frm_search_porder(this);
                    obj.ShowDialog();
                }
                
                if (e.Control && e.KeyCode == Keys.O)
                {
                    SearchToolStripButton.PerformClick();
                }

                if (e.KeyCode == Keys.Escape)
                {

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

        private void grid_purchases_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_purchases.Columns[e.ColumnIndex].Name;
                if (name == "btn_delete")
                {
                    grid_purchases.Rows.RemoveAt(e.RowIndex);

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
            //cmb_suppliers.SelectedValue = 0;
            cmb_suppliers.Refresh();
            //cmb_categories.SelectedValue = 0;
            cmb_employees.SelectedValue = 0;
            cmb_purchase_type.SelectedValue = "Cash";
            //cmb_brands.SelectedValue = 0;
            invoice_status = "";
            //btn_save.Text = "Create (F3)";

            txt_brand_code.Text = "";
            txt_brands.Text = "";
            txt_category_code.Text = "";
            txt_categories.Text = "";
            txt_groups.Text = "";
            txt_group_code.Text = "";

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

            txt_invoice_no.Text = "";
            txt_purchase_date.Refresh();
            txt_purchase_date.Value = DateTime.Now;
            txt_supplier_invoice.Text = "";
            txt_supplier_vat.Text = "";
            txt_cust_balance.Text = "";

            txt_sub_total.Text = "0.00";
            txt_total_amount.Text = "0.00";
            txt_total_tax.Text = "0.00";
            txt_total_discount.Text = "0.00";
            txt_total_qty.Text = "";
            txt_shop_qty.Text = "";
            
            rd_btn_by_unitprice.Checked = true;
            rd_btn_without_vat.Checked = true;

            grid_purchases.DataSource = null;
            grid_purchases.Rows.Clear();
            grid_purchases.Refresh();
            grid_purchases.Rows.Add();
            //this.ActiveControl = grid_purchases;

            grid_product_history.DataSource = null;
            grid_product_history.Rows.Clear();
            //grid_product_history.Refresh();

        }

        public void get_suppliers_dropdownlist()
        {
            SupplierBLL supplierBLL = new SupplierBLL();

            DataTable suppliers = supplierBLL.GetAll();
            DataRow emptyRow = suppliers.NewRow();
            emptyRow[0] = "0";              // Set Column Value
            emptyRow[2] = "Select Supplier";              // Set Column Value
            suppliers.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = suppliers.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[2] = "ADD NEW";              // Set Column Value
            suppliers.Rows.InsertAt(emptyRow1, 1);

            cmb_suppliers.DisplayMember = "first_name";
            cmb_suppliers.ValueMember = "id";
            cmb_suppliers.DataSource = suppliers;

        }
        private void cmb_suppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmb_purchase_type.SelectedValue = (allow_credit_purchase ? "Credit" : "Cash"); //if user has no right then select cash instead

                if (cmb_suppliers.SelectedValue.ToString() != null && cmb_suppliers.SelectedValue.ToString() != "0")
                {
                    int supplier_id = Convert.ToInt32(cmb_suppliers.SelectedValue.ToString());

                    SupplierBLL BLL_obj = new SupplierBLL();
                    DataTable suppliers = BLL_obj.SearchRecordBySupplierID(supplier_id);


                    foreach (DataRow dr in suppliers.Rows)
                    {
                        txt_supplier_vat.Text = dr["vat_no"].ToString();
                        bool vat_with_status = Boolean.Parse(dr["vat_status"].ToString());
                        if (vat_with_status)
                        {
                            rd_btn_with_vat.Checked = true;
                        }
                        else
                        {
                            rd_btn_without_vat.Checked = true;
                        }

                    }

                    ///customer balance
                    DataTable supplier_total_balance = BLL_obj.GetSupplierAccountBalance(supplier_id);
                    ///
                    foreach (DataRow dr in supplier_total_balance.Rows)
                    {
                        txt_cust_balance.Text = dr["balance"].ToString();
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


        private void get_purchasetype_dropdownlist()
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

            if (allow_credit_purchase)
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
            _row_2["id"] = "Hold";
            if (lang == "en-US")
            {
                _row_2["name"] = "Hold";
            }
            else if (lang == "ar-SA")
            {
                _row_2["name"] = "يحفظ";
            }
            dt.Rows.Add(_row_2);


            //DataRow _row_3 = dt.NewRow();
            //_row_3["id"] = "Return";
            //if (lang == "en-US")
            //{
            //    _row_3["name"] = "Return";
            //}
            //else if (lang == "ar-SA")
            //{
            //    _row_3["name"] = "يعود";
            //}
            //dt.Rows.Add(_row_3);

            cmb_purchase_type.DataSource = dt;

            cmb_purchase_type.DisplayMember = "name";
            cmb_purchase_type.ValueMember = "id";

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


        private void grid_purchases_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("Are you sure you want delete", "Delete", buttons, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        grid_purchases.Rows.RemoveAt(grid_purchases.CurrentRow.Index);

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
                    // e.Handled = true; 
                    int iColumn = grid_purchases.CurrentCell.ColumnIndex;
                    int iRow = grid_purchases.CurrentCell.RowIndex;

                    if (iColumn <= 8)
                    {
                        if (grid_purchases.Rows[iRow].Cells["code"].Value != null && grid_purchases.Rows[iRow].Cells["avg_cost"].Value != null && grid_purchases.Rows[iRow].Cells["qty"].Value != null)
                        {
                            grid_purchases.CurrentCell = grid_purchases.Rows[iRow].Cells[iColumn + 1];
                            grid_purchases.Focus();
                            grid_purchases.CurrentCell.Selected = true;
                            //grid_purchases.BeginEdit(true);
                        }

                    }
                    else if (iColumn > 8)
                    {
                        if (grid_purchases.Rows[iRow].Cells["code"].Value != null && grid_purchases.Rows[iRow].Cells["avg_cost"].Value != null && grid_purchases.Rows[iRow].Cells["qty"].Value != null)
                        {
                            grid_purchases.Rows.Add();  //adds new row on last cell of row
                            this.ActiveControl = grid_purchases;
                            grid_purchases.CurrentCell = grid_purchases.Rows[iRow + 1].Cells["code"];
                            grid_purchases.CurrentCell.Selected = true;
                            grid_purchases.BeginEdit(true);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        public void Load_products_to_grid_by_invoiceno(DataTable _dt, string invoice_no)
        {
            try
            {
                grid_purchases.Rows.Clear();
                grid_purchases.Refresh();
                txt_invoice_no.Text = invoice_no;

                string invoice_chr = invoice_no.Substring(0, 2);

                //string invoice_chr = invoice_no.Substring(0, 1);

                if (invoice_chr.ToUpper() == "PO")// for invoice edit
                {
                    invoice_status = "PO";
                }
                else if (invoice_no.Substring(0, 1).ToUpper() == "H")// for HOld purchases invoice 
                {
                    invoice_status = "PO";
                }
                else
                {
                    invoice_status = "PO"; // "Update";
                }

                //btn_save.Text = "Save (F3)";
                po_invoice_no = invoice_no;

                if (_dt.Rows.Count > 0)
                {

                    foreach (DataRow myProductView in _dt.Rows)
                    {
                        txt_supplier_invoice.Text = myProductView["supplier_invoice_no"].ToString();
                        cmb_suppliers.SelectedValue = myProductView["supplier_id"];
                        cmb_employees.SelectedValue = myProductView["employee_id"];
                        //cmb_purchase_type.SelectedValue = myProductView["purchase_type"];
                        //txt_purchase_date.Value = Convert.ToDateTime(myProductView["purchase_date"].ToString());
                        txt_description.Text = myProductView["description"].ToString();
                        txt_shipping_cost.Text = (string.IsNullOrEmpty(myProductView["shipping_cost"].ToString() as String) ? "" : myProductView["shipping_cost"].ToString());

                        decimal qty = Math.Round(Convert.ToDecimal(myProductView["quantity"].ToString()),2);
                        decimal discount = Math.Round(Convert.ToDecimal(myProductView["discount_value"]), 4);
                        decimal total = qty * decimal.Parse(myProductView["cost_price"].ToString());
                        decimal tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : decimal.Parse(myProductView["tax_rate"].ToString()));
                        decimal tax = Math.Round(((total- discount) * tax_rate / 100),4);
                        //decimal tax = Math.Round(Convert.ToDecimal(myProductView["vat"].ToString()), 2);
                        decimal current_sub_total = Math.Round((tax + total-discount),2);

                        int id = Convert.ToInt32(myProductView["id"]);
                        string code = myProductView["code"].ToString();
                        string name = myProductView["name"].ToString();
                        //string qty = myProductView["quantity"].ToString();
                        decimal cost_price = Math.Round(Convert.ToDecimal(myProductView["cost_price"]),4);
                        decimal unit_price = Math.Round( Convert.ToDecimal(myProductView["unit_price"]),4);
                        string location_code = myProductView["location_code"].ToString();
                        string unit = myProductView["unit"].ToString();
                        string category = myProductView["category"].ToString();
                        string item_type = myProductView["item_type"].ToString();
                        string btn_delete = "Del";
                        string tax_id = myProductView["tax_id"].ToString();
                        decimal shop_qty = 0;

                        string[] row0 = { id.ToString(), code, name, 
                                            qty.ToString(), cost_price.ToString(),unit_price.ToString(), discount.ToString(), 
                                            tax.ToString(), current_sub_total.ToString(),location_code,unit,category, 
                                            btn_delete, tax_id.ToString(), tax_rate.ToString(), shop_qty.ToString() };
                        int RowIndex = grid_purchases.Rows.Add(row0);

                        //GET / SET Location Dropdown list
                        //fill_locations_grid_combo(RowIndex);
                        //////
                    }
                    //grid_purchases.Rows.Add();
                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_amount();
                    get_total_qty();
                    grid_purchases.Focus();

                    this.ActiveControl = grid_purchases;
                    grid_purchases.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void grid_purchases_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(tb_KeyPress);

            if (grid_purchases.CurrentCell.ColumnIndex == 3 || grid_purchases.CurrentCell.ColumnIndex == 4 || grid_purchases.CurrentCell.ColumnIndex == 5 || grid_purchases.CurrentCell.ColumnIndex == 6) //qty, unit price and discount Column will accept only numeric
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
            TextBox tb = sender as TextBox;

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }
           
            // Allow only one decimal point
            if (e.KeyChar == '.' && tb.Text.Contains("."))
            {
                e.Handled = true;
                return;
            }

            // Prevent entering "." as the first character
            if (e.KeyChar == '.' && tb.Text.Length == 0)
            {
                e.Handled = true;
                return;
            }
        }


        private void productMovementF4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryToolStripButton.PerformClick();
        }

        private void grid_purchases_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //for Serial No. in grid
            using (SolidBrush b = new SolidBrush(grid_purchases.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
            //grid_purchases.Rows[e.RowIndex].Cells["sno"].Value = (e.RowIndex + 1).ToString();
        }

        private void addNewRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid_purchases.Rows.Add();
        }
        private void SetupBrandDataGridView()
        {
            var lang = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

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
                grid_purchases.Focus();
            }
        }

        private void brandsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(brandsDataGridView);
            grid_purchases.Focus();

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
            var lang = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;
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
                grid_purchases.Focus();

            }
        }

        private void categoriesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_category_code.Text = categoriesDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_categories.Text = categoriesDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(categoriesDataGridView);
            grid_purchases.Focus();

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
            var lang = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;
            groupsDataGridView.ColumnCount = 2;
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
                e.Handled = true;
                txt_group_code.Text = groupsDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_groups.Text = groupsDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.Controls.Remove(groupsDataGridView);
                grid_purchases.Focus();

            }
        }

        private void groupsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_group_code.Text = groupsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_groups.Text = groupsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(groupsDataGridView);
            grid_purchases.Focus();

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

        private void grid_purchases_SelectionChanged(object sender, EventArgs e)
        {
            if (grid_purchases.Rows.Count > 0 && grid_purchases.Focused)
            {
                string product_code = (grid_purchases.CurrentRow.Cells["code"].Value != null ? grid_purchases.CurrentRow.Cells["code"].Value.ToString() : "");
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
                            txt_shop_qty.Text = (grid_purchases.CurrentRow.Cells["shop_qty"].Value != null ? grid_purchases.CurrentRow.Cells["shop_qty"].Value.ToString() : "");

                        }
                    }
                    else
                    {
                        load_product_purchase_history(product_code);
                        txt_shop_qty.Text = (grid_purchases.CurrentRow.Cells["shop_qty"].Value != null ? grid_purchases.CurrentRow.Cells["shop_qty"].Value.ToString() : "");

                    }
                }

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

                String keyword = "TOP 100 I.id,P.name AS product_name,I.item_code,I.qty,I.unit_price,I.cost_price,I.invoice_no,I.description,I.trans_date, S.first_name AS supplier";
                String table = "pos_inventory I LEFT JOIN pos_products P ON P.code=I.item_code LEFT JOIN pos_suppliers S ON S.id=I.supplier_id WHERE I.item_code = '" + product_code + "' AND I.description = 'Purchase' ORDER BY I.id DESC";
                grid_product_history.DataSource = objBLL.GetRecord(keyword, table);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void txt_total_disc_value_TextChanged(object sender, EventArgs e)
        {            
            decimal new_total_discount = (string.IsNullOrEmpty(txt_total_disc_value.Text) ? 0 : Convert.ToDecimal(txt_total_disc_value.Text));

            total_discount_value(new_total_discount);
        }


        public void total_discount_value(decimal total_discount_value)
        {
            int total_rows = grid_purchases.Rows.Count;
            int filled_rows = 0;
            
            for (int i = 0; i <= total_rows - 1; i++)
            {
                int product_id = Convert.ToInt16(grid_purchases.Rows[i].Cells["id"].Value);
                if (product_id > 0)
                {
                    filled_rows++;
                }
            }
            //txt_total_amount.Text = round_total_amount.ToString();
            //double total_diff_amount = old_total_amount - new_amount; // calculate difference amount and add to single item unit price
            //double total_item_share = sub_total * 100 / old_total_amount;
            //double total_tax_share = (old_total_amount - sub_total) * 100 / old_total_amount;
            decimal diff_amount_per_item = (filled_rows > 0 ? (total_discount_value / filled_rows) : 0);

            //double new_amount_total = 0;
            //double new_amount_single = 0;
            //double new_vat_total = 0;
            //double net_total = 0;

            double tax_1 = 0;
            double total_value = 0;
            double tax_rate = 0;
            double sub_total_1 = 0;


                for (int i = 0; i <= filled_rows - 1; i++)
                {
                    int product_id = Convert.ToInt16(grid_purchases.Rows[i].Cells["id"].Value);
                    if(product_id > 0)
                    {
                        grid_purchases.Rows[i].Cells["discount"].Value = Math.Round(diff_amount_per_item,3);

                        //new_amount_single = (double.Parse(grid_purchases.Rows[i].Cells["sub_total"].Value.ToString()) / double.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString())) - (diff_amount_per_item / double.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString()));
                        //new_amount_total = new_amount_single * total_item_share / 100;
                        //new_vat_total = new_amount_single * total_tax_share / 100;
                        //net_total = (grid_purchases.Rows[i].Cells["tax"].Value.ToString() == "0" ? (new_amount_total + new_vat_total) : new_amount_total);
                        //grid_purchases.Rows[i].Cells["unit_price"].Value = net_total;

                        tax_rate = (grid_purchases.Rows[i].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString()));

                        ////grid_purchases.Rows[i].Cells["sub_total"].Value = Convert.ToDouble(grid_purchases.Rows[i].Cells["sub_total"].Value) - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value);

                        total_value = Convert.ToDouble(grid_purchases.Rows[i].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[i].Cells["qty"].Value);

                        tax_1 = ((total_value - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value)) * tax_rate / 100);

                        sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value);

                        grid_purchases.Rows[i].Cells["sub_total"].Value = sub_total_1;
                        ////grid_purchases.Rows[i].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                        grid_purchases.Rows[i].Cells["tax"].Value = (tax_1);

                        get_total_tax();
                        get_total_discount();
                        get_sub_total_amount();
                        get_total_amount();
                        get_total_qty(); 
                    }
                    
            }

        }

        private void productDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_purchases.RowCount > 0)
                {
                    if (grid_purchases.CurrentRow.Cells["code"].Value != null)
                    {
                        string product_code = grid_purchases.CurrentRow.Cells["code"].Value.ToString();

                        frm_product_full_detail obj = new frm_product_full_detail(null,this, product_code);
                        obj.ShowDialog();
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void chkbox_is_taxable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_is_taxable.Checked == false)
            {
                int total_rows = grid_purchases.Rows.Count;
                double tax_1 = 0;
                double total_value = 0;
                double tax_rate = 0;
                double sub_total_1 = 0;

                for (int i = 0; i <= total_rows - 1; i++)
                {
                    string product_code = grid_purchases.Rows[i].Cells["code"].Value.ToString();
                    if (product_code.Length > 0)
                    {

                        //new_amount_single = (double.Parse(grid_purchases.Rows[i].Cells["sub_total"].Value.ToString()) / double.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString())) - (diff_amount_per_item / double.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString()));
                        //new_amount_total = new_amount_single * total_item_share / 100;
                        //new_vat_total = new_amount_single * total_tax_share / 100;
                        //net_total = (grid_purchases.Rows[i].Cells["tax"].Value.ToString() == "0" ? (new_amount_total + new_vat_total) : new_amount_total);
                        //grid_purchases.Rows[i].Cells["unit_price"].Value = net_total;

                        tax_rate = 0; // (grid_purchases.Rows[i].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString()));

                        ////grid_purchases.Rows[i].Cells["sub_total"].Value = Convert.ToDouble(grid_purchases.Rows[i].Cells["sub_total"].Value) - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value);

                        total_value = Convert.ToDouble(grid_purchases.Rows[i].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[i].Cells["qty"].Value);

                        tax_1 = ((total_value - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value)) * tax_rate / 100);

                        sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value);
                        grid_purchases.Rows[i].Cells["tax"].Value = tax_1;

                        grid_purchases.Rows[i].Cells["sub_total"].Value = sub_total_1;
                        ////grid_purchases.Rows[i].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                        
                       
                    }
                }
            }
            else
            {
                int total_rows = grid_purchases.Rows.Count;
                double tax_1 = 0;
                double total_value = 0;
                double tax_rate = 0;
                double sub_total_1 = 0;

                for (int i = 0; i <= total_rows - 1; i++)
                {
                    string product_code = grid_purchases.Rows[i].Cells["code"].Value.ToString();
                    if (product_code.Length > 0)
                    {

                        //new_amount_single = (double.Parse(grid_purchases.Rows[i].Cells["sub_total"].Value.ToString()) / double.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString())) - (diff_amount_per_item / double.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString()));
                        //new_amount_total = new_amount_single * total_item_share / 100;
                        //new_vat_total = new_amount_single * total_tax_share / 100;
                        //net_total = (grid_purchases.Rows[i].Cells["tax"].Value.ToString() == "0" ? (new_amount_total + new_vat_total) : new_amount_total);
                        //grid_purchases.Rows[i].Cells["unit_price"].Value = net_total;

                        tax_rate = (grid_purchases.Rows[i].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString()));

                        ////grid_purchases.Rows[i].Cells["sub_total"].Value = Convert.ToDouble(grid_purchases.Rows[i].Cells["sub_total"].Value) - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value);

                        total_value = Convert.ToDouble(grid_purchases.Rows[i].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[i].Cells["qty"].Value);

                        tax_1 = ((total_value - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value)) * tax_rate / 100);

                        sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value);
                        grid_purchases.Rows[i].Cells["tax"].Value = tax_1;
                        grid_purchases.Rows[i].Cells["sub_total"].Value = sub_total_1;
                        ////grid_purchases.Rows[i].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                        //grid_purchases.Rows[i].Cells["tax"].Value = 0;

                        
                    }
                }
            }
            get_total_tax();
            get_total_discount();
            get_sub_total_amount();
            get_total_amount();
            get_total_qty();
        }

        private void grid_purchases_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            MessageBox.Show("Error happened " + anError.Context.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
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
            this.ActiveControl = grid_purchases;
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                string purchase_type = (string.IsNullOrEmpty(cmb_purchase_type.SelectedValue.ToString()) ? "Cash" : cmb_purchase_type.SelectedValue.ToString());
                int supplier_id = (cmb_suppliers.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_suppliers.SelectedValue.ToString()));

                if (purchase_type == "Hold")
                {
                    hold_purchases();
                    return;
                }


                if (supplier_id <= 0 || txt_supplier_invoice.Text.Length == 0)
                {
                    MessageBox.Show("Supplier and Supplier Invoice No. are required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cmb_suppliers.Focus();
                    return;
                }

                //if (txt_supplier_invoice.Text.Length == 0)
                //{
                //    MessageBox.Show("Supplier and Supplier Invoice No. are required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                DialogResult result = MessageBox.Show("Are you sure you want to purchase " + purchase_type, "Purchase Transaction " + invoice_status, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (grid_purchases.Rows.Count > 0)
                    {
                        List<PurchaseModalHeader> purchase_model_header = new List<PurchaseModalHeader> { };
                        List<PurchasesModal> purchase_model_detail = new List<PurchasesModal> { };

                        //PurchasesModal PurchasesModal_obj = new PurchasesModal();
                        PurchasesBLL purchasesObj = new PurchasesBLL();
                        DateTime purchase_date = txt_purchase_date.Value.Date;

                        int employee_id = (cmb_employees.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_employees.SelectedValue.ToString()));
                        string po_invoice_no_1 = "";
                        bool po_status = false;
                        string invoice_no = "";
                        string location_code = "";

                        //if (invoice_status == "Update" && txt_invoice_no.Text.Substring(0, 1).ToUpper() == "P") //Update sales delete all record first and insert new sales
                        //{
                        //    int qresult = purchasesObj.DeletePurchases(txt_invoice_no.Text); //DELETE ALL TRANSACTIONS
                        //    if (qresult <= 0)
                        //    {
                        //        MessageBox.Show(invoice_no + " has issue while updating, please try again", "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //        return;
                        //    }
                        //    invoice_no = txt_invoice_no.Text;
                        //}
                        //else
                        if (invoice_status == "PO") //if purchase order
                        {
                            po_invoice_no_1 = po_invoice_no;
                            po_status = true;
                            invoice_no = GetMAXInvoiceNo();
                        }
                        else
                        {
                            invoice_no = GetMAXInvoiceNo();
                        }

                        //if purchase return then put minus sign before amount
                        decimal return_minus_value = (purchase_type == "Return" ? -1 : 1);
                        decimal net_total = Math.Round(return_minus_value * total_amount, 6);
                        decimal net_total_discount = Math.Round(return_minus_value * total_discount, 6);
                        decimal net_total_tax = Math.Round(return_minus_value * total_tax, 6);


                        //set the date from datetimepicker and set time to te current time
                        DateTime now = DateTime.Now;
                        txt_purchase_date.Value = new DateTime(txt_purchase_date.Value.Year, txt_purchase_date.Value.Month, txt_purchase_date.Value.Day, now.Hour, now.Minute, now.Second);
                        /////////////////////

                        /////Add sales header into the List
                        purchase_model_header.Add(new PurchaseModalHeader
                        {
                            supplier_id = supplier_id,
                            employee_id = employee_id,
                            invoice_no = invoice_no,
                            supplier_invoice_no = txt_supplier_invoice.Text,
                            total_amount = net_total,
                            total_tax = Math.Round(total_tax, 6),
                            total_discount = total_discount,
                            //total_discount_percent = (string.IsNullOrEmpty(txt_total_disc_percent.Text) ? 0 : Convert.ToDouble(txt_total_disc_percent.Text)),
                            purchase_type = purchase_type,
                            purchase_date = purchase_date,
                            purchase_time = txt_purchase_date.Value,
                            description = txt_description.Text,
                            shipping_cost = (string.IsNullOrEmpty(txt_shipping_cost.Text) ? 0 : Convert.ToDecimal(txt_shipping_cost.Text)),
                            account = "Purchase",
                            po_invoice_no = po_invoice_no_1,
                            po_status = po_status,

                            cash_account_id = cash_account_id,
                            payable_account_id = payable_account_id,
                            tax_account_id = tax_account_id,
                            purchases_discount_acc_id = purchases_discount_acc_id,
                            inventory_acc_id = inventory_acc_id,
                            purchases_acc_id = purchases_acc_id,

                        });
                        //////

                        int sno = 1;
                        for (int i = 0; i < grid_purchases.Rows.Count; i++)
                        {
                            if (grid_purchases.Rows[i].Cells["id"].Value != null && grid_purchases.Rows[i].Cells["code"].Value != null)
                            {

                                if (grid_purchases.Rows[i].Cells["location_code"].Value == null || grid_purchases.Rows[i].Cells["location_code"].Value == DBNull.Value || String.IsNullOrEmpty(grid_purchases.Rows[i].Cells["location_code"].Value as String) || String.IsNullOrWhiteSpace(grid_purchases.Rows[i].Cells["location_code"].Value.ToString()))
                                {
                                    location_code = "";
                                }
                                else
                                {
                                    location_code = grid_purchases.Rows[i].Cells["location_code"].Value.ToString();
                                }

                                ///// Added sales detail in to List
                                decimal tax_rate = (grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : decimal.Parse(grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString()));
                                
                                purchase_model_detail.Add(new PurchasesModal
                                {
                                    serialNo = sno++,
                                    invoice_no = invoice_no,
                                    item_id = Convert.ToInt32(grid_purchases.Rows[i].Cells["id"].Value.ToString()),
                                    code = grid_purchases.Rows[i].Cells["code"].Value.ToString(),
                                    //name = grid_purchases.Rows[i].Cells["name"].Value.ToString(),
                                    quantity = return_minus_value * (string.IsNullOrEmpty(grid_purchases.Rows[i].Cells["qty"].Value.ToString()) ? 0 : decimal.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString())),
                                    cost_price = return_minus_value * (string.IsNullOrEmpty(grid_purchases.Rows[i].Cells["avg_cost"].Value.ToString()) ? 0 : Math.Round(Convert.ToDecimal(grid_purchases.Rows[i].Cells["avg_cost"].Value.ToString()), 4)),// its avg cost actually ,
                                    unit_price = return_minus_value * (string.IsNullOrEmpty(grid_purchases.Rows[i].Cells["unit_price"].Value.ToString()) ? 0 : Math.Round(decimal.Parse(grid_purchases.Rows[i].Cells["unit_price"].Value.ToString()), 4)),
                                    discount = return_minus_value * (string.IsNullOrEmpty(grid_purchases.Rows[i].Cells["discount"].Value.ToString()) ? 0 : Math.Round(decimal.Parse(grid_purchases.Rows[i].Cells["discount"].Value.ToString()), 4)),
                                    tax_id = Convert.ToInt32(grid_purchases.Rows[i].Cells["tax_id"].Value.ToString()),
                                    tax_rate = tax_rate,
                                    purchase_date = purchase_date,
                                    location_code = location_code,
                                    supplier_id = supplier_id,

                                });
                                //////////////
                            }

                        }

                        var purchase_id = purchasesObj.Insertpurchases(purchase_model_header, purchase_model_detail);


                        /////INVENTORY JOURNAL ENTRY (DEBIT)
                        //Insert_Journal_entry(invoice_no, inventory_acc_id, net_total, 0, purchase_date, txt_description.Text, 0, 0, 0);

                        //if (purchase_type == "Cash")
                        //{
                        //    ///CASH JOURNAL ENTRY (CREDIT)
                        //    Insert_Journal_entry(invoice_no, cash_account_id, 0, net_total, purchase_date, txt_description.Text, 0, 0, 0);

                        //}
                        //else
                        //{
                        //    ///ACCOUNT PAYABLE JOURNAL ENTRY (CREDIT)
                        //    int entry_id = Insert_Journal_entry(invoice_no, payable_account_id, 0, net_total, purchase_date, txt_description.Text, 0, 0, 0);

                        //    if (supplier_id != 0)
                        //    {
                        //        ///ADD ENTRY INTO supplier PAYMENT(Credit)
                        //        Insert_Journal_entry(invoice_no, inventory_acc_id, 0, net_total, purchase_date, txt_description.Text, 0, supplier_id, entry_id);

                        //    }

                        //}


                        //if (net_total_discount > 0)
                        //{
                        //    /// CASH JOURNAL ENTRY (DEBIT)
                        //    Insert_Journal_entry(invoice_no, cash_account_id, net_total_discount, 0, purchase_date, txt_description.Text, 0, 0, 0);
                        //    ///PURCHASE DISCOUNT JOURNAL ENTRY (CREDIT)
                        //    int entry_id = Insert_Journal_entry(invoice_no, purchases_discount_acc_id, 0, net_total_discount, purchase_date, txt_description.Text, 0, 0, 0);

                        //    if (purchase_type == "Credit" && supplier_id != 0)
                        //    {
                        //        ///ADD ENTRY INTO CUSTOMER PAYMENT(DEBIT)
                        //        Insert_Journal_entry(invoice_no, purchases_discount_acc_id, 0, net_total_discount, purchase_date, txt_description.Text, 0, supplier_id, entry_id);

                        //    }
                        //}

                        //if (net_total_tax > 0)
                        //{
                        //    ///SALES TAX JOURNAL ENTRY (DEBIT)
                        //    Insert_Journal_entry(invoice_no, tax_account_id, net_total_tax, 0, purchase_date, txt_description.Text, 0, 0, 0);

                        //    if (purchase_type == "Cash")
                        //    {
                        //        ///CASH JOURNAL ENTRY (CREDIT)
                        //        Insert_Journal_entry(invoice_no, cash_account_id, 0, net_total_tax, purchase_date, txt_description.Text, 0, 0, 0);

                        //    }
                        //    else
                        //    {
                        //        ///ACCOUNT PAYABLE JOURNAL ENTRY (CREDIT)
                        //        int entry_id = Insert_Journal_entry(invoice_no, payable_account_id, 0, net_total_tax, purchase_date, txt_description.Text, 0, 0, 0);

                        //        if (supplier_id != 0)
                        //        {
                        //            ///ADD ENTRY INTO supplier PAYMENT(Credit)
                        //            Insert_Journal_entry(invoice_no, tax_account_id, 0, net_total_tax, purchase_date, txt_description.Text, 0, supplier_id, entry_id);

                        //        }

                        //    }

                        //}

                        if (purchase_id > 0)
                        {
                            MessageBox.Show(invoice_no + " transaction created successfully", "Success " + invoice_status, MessageBoxButtons.OK, MessageBoxIcon.Information);

                            clear_form();
                            GetMAXInvoiceNo();
                            grid_purchases.Focus();
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

        private void HistoryToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                string product_code = "";

                if (grid_purchases.Rows.Count > 0)
                {
                    product_code = grid_purchases.CurrentRow.Cells["code"].Value.ToString();
                }

                frm_productsMovements frm_prod_move_obj = new frm_productsMovements(product_code);
                frm_prod_move_obj.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                frm_search_p_invoices frm = new frm_search_p_invoices(this);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPOToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                frm_search_porder obj = new frm_search_porder(this);
                obj.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grid_purchases_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6)
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    e.Cancel = true;
                    grid_purchases.Rows[e.RowIndex].ErrorText = "Value cannot be null or empty";
                }
                else if (!decimal.TryParse(e.FormattedValue.ToString(), out _))
                {
                    e.Cancel = true;
                    grid_purchases.Rows[e.RowIndex].ErrorText = "Value must be a numeric value";
                }
                else
                {
                    grid_purchases.Rows[e.RowIndex].ErrorText = string.Empty;
                }
            }
        }
    }
}
