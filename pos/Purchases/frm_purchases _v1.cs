using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_purchases_v1 : Form
    {
        public string lang = (UsersModal.logged_in_lang.Length > 0 ? UsersModal.logged_in_lang : "en-US");
        public int cash_account_id = 0;
        //public int sales_account_id = 0;
        public int payable_account_id = 0;
        public int tax_account_id = 0;
        public int purchases_discount_acc_id = 0;
        public int inventory_acc_id = 0;
        public int purchases_acc_id = 0;

        public static frm_purchases_v1 instance;
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
        public double global_tax_rate = 0;
        public double global_unit_price = 0;
        public string global_location_code = "";
        public string global_unit = "";
        public string global_item_category = "";

        string invoice_status = "";
        string po_invoice_no = "";

        public double cash_purchase_amount_limit = 0;
        public bool allow_credit_purchase = false;

        private DataGridView salesDataGridView = new DataGridView();
        
        public frm_purchases_v1()
        {
            InitializeComponent();
        
        }

        private void frm_purchases_v1_Load(object sender, EventArgs e)
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
            //get_brands_dropdownlist();
            //get_categories_dropdownlist();
            //get_groups_dropdownlist();

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

        Form purchaseSearchObj;
        private void grid_purchases_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //int iColumn = grid_purchases.CurrentCell.ColumnIndex;
                //int iRow = grid_purchases.CurrentCell.RowIndex;
                string columnName = grid_purchases.Columns[e.ColumnIndex].Name;
                //if (columnName == "code")
                //{
                //    string product_code = (grid_purchases.CurrentRow.Cells["code"].Value != null ? grid_purchases.CurrentRow.Cells["code"].Value.ToString() : "");
                //    bool isGrid = true;
                //    var brand_code = txt_brand_code.Text; // (cmb_brands.SelectedValue != null ? cmb_brands.SelectedValue.ToString() : "");
                //    var category_code = txt_category_code.Text; // (cmb_categories.SelectedValue != null ? cmb_categories.SelectedValue.ToString() : "");
                //    var group_code = txt_group_code.Text; // (cmb_groups.SelectedValue != null ? cmb_groups.SelectedValue.ToString() : "");

                //    ////////////////////////
                //    if (purchaseSearchObj == null || product_code != "")
                //    {
                //        purchaseSearchObj = new frm_searchPurchaseProducts(this, null, product_code, category_code, brand_code, e.RowIndex, isGrid, group_code);
                //        purchaseSearchObj.FormClosed += new FormClosedEventHandler(purchaseSearchObj_FormClosed);
                //        //frm_cust.Dock = DockStyle.Fill;
                //        purchaseSearchObj.ShowDialog();
                //    }
                //    else
                //    {
                //        //frm_searchSaleProducts_obj.ShowDialog();
                //        //frm_searchSaleProducts_obj.BringToFront();
                //        purchaseSearchObj.Visible = true;
                //    }
                //    ////////////

                //}
                if (columnName == "code")//if discount is changed
                {

                    if (salesDataGridView.Rows.Count <= 0)
                    {
                        this.Controls.Remove(salesDataGridView);
                    }
                    else
                    {
                        salesDataGridView.Focus();
                    }

                }

                if (columnName == "packing")
                {
                    grid_purchases.Rows[e.RowIndex].Cells["qty"].Value = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["packing"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["packet_qty"].Value);
                }

                if (columnName == "Qty") // if qty is changed
                {
                    double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                    double tax = (Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * tax_rate / 100);

                    grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = (tax * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value)).ToString("0.00");
                    double sub_total = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                    grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total.ToString("0.00");
                }
                if (columnName == "avg_cost")//if avg_cost is changed
                {
                    if (rd_btn_without_vat.Checked && rd_btn_by_unitprice.Checked)
                    {
                        double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                        double tax = (Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * tax_rate / 100);
                        grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = (tax * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value)).ToString("0.00");

                        double sub_total = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                        grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total.ToString("0.00");
                    }

                    if (rd_btn_without_vat.Checked && rd_btn_bytotal_price.Checked)
                    {
                        double total_avg_cost_1 = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value);
                        double qty_1 = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) * 1;
                        double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                        string pre_tax = "1." + tax_rate.ToString();
                        double single_avg_cost = (total_avg_cost_1 / qty_1);
                        double tax = (single_avg_cost * tax_rate / 100);

                        grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = single_avg_cost.ToString("0.00");

                        grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = (tax * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value)).ToString("0.00");

                        double sub_total = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                        grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total.ToString("0.00");
                    }

                    if (rd_btn_with_vat.Checked && rd_btn_by_unitprice.Checked)
                    {
                        double total_avg_cost = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value);
                        double qty = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) * 1;
                        double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                        string pre_tax = "1." + tax_rate.ToString();
                        double single_avg_cost = (total_avg_cost / double.Parse(pre_tax));
                        double tax = (single_avg_cost * tax_rate / 100);

                        grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = (tax * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value)).ToString("0.00");

                        double sub_total = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);

                        grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = (Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) - tax).ToString("0.00");
                        grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total.ToString("0.00");
                    }

                    if (rd_btn_with_vat.Checked && rd_btn_bytotal_price.Checked)
                    {
                        double total_avg_cost_1 = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value);
                        double qty_1 = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) * 1;
                        double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                        string pre_tax = "1." + tax_rate.ToString();
                        double single_avg_cost = ((total_avg_cost_1 / double.Parse(pre_tax)) / qty_1);
                        double tax = (single_avg_cost * tax_rate / 100);

                        grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = single_avg_cost.ToString("0.00");

                        grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = (tax * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value)).ToString("0.00");

                        double sub_total = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                        grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total.ToString("0.00");
                    }
                }

                if (columnName == "discount")//if discount is changed
                {
                    //double sub_total = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);

                    double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                    double total_value = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value);
                    double tax_1 = ((total_value - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value)) * tax_rate / 100);
                    double sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                    //total_discount += Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                    //txt_total_discount.Text = total_amount.ToString();
                    grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = (tax_1).ToString("0.00");
                    grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total_1.ToString("0.00");
                }

                get_total_tax();
                get_total_discount();
                get_sub_total_amount();
                get_total_amount();

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
        public void Load_products_to_grid(string product_id)
        {
            ProductBLL productsBLL_obj = new ProductBLL();
            DataTable dt = productsBLL_obj.SearchRecordByProductID(product_id);
            //grid_purchases.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            int RowIndex = grid_purchases.CurrentCell.RowIndex;

            if (dt.Rows.Count > 0)
            {
                //for (int i = 0; i < grid_purchases.RowCount; i++)
                //{
                //    var item_id = (grid_purchases.Rows[i].Cells["id"].Value != null ? grid_purchases.Rows[i].Cells["id"].Value : "");
                //    if (item_id.ToString() == product_id)
                //    {
                //        MessageBox.Show("Product already added", "Already exist", MessageBoxButtons.OK, MessageBoxIcon.Question);
                //        grid_purchases.Focus();
                //        grid_purchases.CurrentCell = grid_purchases.Rows[RowIndex].Cells["code"]; //make qty cell active
                //        grid_purchases.CurrentCell.Selected = true;
                //        grid_purchases.BeginEdit(true);
                //        return;
                //    }
                //    else
                //    {
                //        grid_purchases.CurrentCell = grid_purchases.Rows[RowIndex].Cells["qty"]; //make qty cell active
                //        grid_purchases.CurrentCell.Selected = true;

                //    }
                //}

                foreach (DataRow myProductView in dt.Rows)
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
                    grid_purchases.Rows[RowIndex].Cells["packing"].Value = "0"; 
                    grid_purchases.Rows[RowIndex].Cells["qty"].Value = qty;
                    grid_purchases.Rows[RowIndex].Cells["avg_cost"].Value = avg_cost;
                    grid_purchases.Rows[RowIndex].Cells["unit_price"].Value = unit_price;
                    grid_purchases.Rows[RowIndex].Cells["discount"].Value = 0.00;
                    grid_purchases.Rows[RowIndex].Cells["tax"].Value = tax;
                    grid_purchases.Rows[RowIndex].Cells["sub_total"].Value = sub_total;
                    grid_purchases.Rows[RowIndex].Cells["location_code"].Value = (String.IsNullOrEmpty(myProductView["location_code"].ToString()) ? "DEF" : myProductView["location_code"].ToString());
                    grid_purchases.Rows[RowIndex].Cells["unit"].Value = myProductView["unit"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["category"].Value = myProductView["category"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["btn_delete"].Value = "Del";
                    grid_purchases.Rows[RowIndex].Cells["tax_id"].Value = myProductView["tax_id"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["tax_rate"].Value = myProductView["tax_rate"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["packet_qty"].Value = myProductView["packet_qty"].ToString();
                    /////
                    //fill_locations_grid_combo(RowIndex);
                    ////
                }

            }
        }

        private void fill_locations_grid_combo(int RowIndex)
        {
            var locationComboCell = new DataGridViewComboBoxCell();
            DataTable dt = new DataTable();

            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code as location_code,name";
            string table = "pos_locations";

            dt = generalBLL_obj.GetRecord(keyword, table);

            locationComboCell.DataSource = dt;
            locationComboCell.DisplayMember = "location_code";
            locationComboCell.ValueMember = "location_code";

            grid_purchases.Rows[RowIndex].Cells["location_code"] = locationComboCell;
            grid_purchases.Rows[RowIndex].Cells["location_code"].Value = dt.Rows[0]["location_code"].ToString(); // GET FIRST COLUMN OF DT TO SHOW FIRST VALUE AS SELECTED

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
        void salesDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                string product_id = salesDataGridView.CurrentRow.Cells[0].Value.ToString();
                //txt_brands.Text = salesDataGridView.CurrentRow.Cells[1].Value.ToString();
                if (product_id != null)
                {
                    Load_products_to_grid(product_id);

                }

                this.Controls.Remove(salesDataGridView);
                grid_purchases.Focus();
            }
        }

        private void salesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string product_id = salesDataGridView.CurrentRow.Cells[0].Value.ToString();
            //txt_brands.Text = salesDataGridView.CurrentRow.Cells[1].Value.ToString();
            if (product_id != null)
            {
                Load_products_to_grid(product_id);

            }
            this.Controls.Remove(salesDataGridView);
            grid_purchases.Focus();

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                string purchase_type = (string.IsNullOrEmpty(cmb_purchase_type.SelectedValue.ToString()) ? "Cash" : cmb_purchase_type.SelectedValue.ToString());
                int supplier_id = (cmb_suppliers.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_suppliers.SelectedValue.ToString()));

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

                DialogResult result = MessageBox.Show("Are you sure you want to purchase", "Purchase Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (grid_purchases.Rows.Count > 0)
                    {
                        List<PurchaseModalHeader> purchase_model_header = new List<PurchaseModalHeader> { };
                        List<PurchasesModal> purchase_model_detail = new List<PurchasesModal> { };
                        
                       // PurchasesModal PurchasesModal_obj = new PurchasesModal();
                        PurchasesBLL purchasesObj = new PurchasesBLL();
                        DateTime purchase_date = txt_purchase_date.Value.Date;
                       
                        int employee_id = (cmb_employees.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_employees.SelectedValue.ToString()));
                        string po_invoice_no_1 = "";
                        bool po_status = false;
                        string invoice_no = "";
                        string location_code = "";


                        if (invoice_status == "Update" && txt_invoice_no.Text.Substring(0, 1).ToUpper() == "P") //Update sales delete all record first and insert new sales
                        {
                            purchasesObj.DeletePurchases(txt_invoice_no.Text); //DELETE ALL TRANSACTIONS
                            invoice_no = txt_invoice_no.Text;
                        }
                        else if (invoice_status == "PO") //if purchase order
                        {
                            po_invoice_no_1 = po_invoice_no;
                            po_status = true;
                            invoice_no = GetMAXInvoiceNo();
                        }
                        else
                        {
                            invoice_no = GetMAXInvoiceNo();
                        }


                        //set the date from datetimepicker and set time to te current time
                        DateTime now = DateTime.Now;
                        txt_purchase_date.Value = new DateTime(txt_purchase_date.Value.Year, txt_purchase_date.Value.Month, txt_purchase_date.Value.Day, now.Hour, now.Minute, now.Second);
                        /////////////////////
                       
                        /////Added sales header into the List
                        purchase_model_header.Add(new PurchaseModalHeader
                        {
                            supplier_id = supplier_id,
                            employee_id = employee_id,
                            invoice_no = invoice_no,
                            supplier_invoice_no = txt_supplier_invoice.Text,
                            total_amount = total_amount,
                            total_tax = Math.Round(total_tax, 6),
                            total_discount = total_discount,
                            //total_discount_percent = (string.IsNullOrEmpty(txt_total_disc_percent.Text) ? 0 : Convert.ToDouble(txt_total_disc_percent.Text)),
                            purchase_type = purchase_type,
                            purchase_date = purchase_date,
                            purchase_time = txt_purchase_date.Value,
                            description = txt_description.Text,
                            //shipping_cost = (string.IsNullOrEmpty(txt_shipping_cost.Text) ? 0 : Convert.ToDecimal(txt_shipping_cost.Text)),
                            account = "Purchase",
                            //po_invoice_no = po_invoice_no_1,
                            //po_status = po_status

                            cash_account_id = cash_account_id,
                            payable_account_id = payable_account_id,
                            tax_account_id = tax_account_id,
                            purchases_discount_acc_id = purchases_discount_acc_id,
                            inventory_acc_id = inventory_acc_id,
                            purchases_acc_id = purchases_acc_id,
                        });
                        //////

                        //Int32 purchase_id = 0; // purchasesObj.Insertpurchases(PurchasesModal_obj);

                        for (int i = 0; i < grid_purchases.Rows.Count; i++)
                        {
                            if (grid_purchases.Rows[i].Cells["id"].Value != null)
                            {
                               
                                ///// Added sales detail in to List
                                decimal tax_rate = (grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : decimal.Parse(grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString()));

                                purchase_model_detail.Add(new PurchasesModal
                                {
                                    invoice_no = invoice_no,
                                    item_id = int.Parse(grid_purchases.Rows[i].Cells["id"].Value.ToString()),
                                    //name = grid_purchases.Rows[i].Cells["name"].Value.ToString(),
                                    packet_qty = decimal.Parse(grid_purchases.Rows[i].Cells["packet_qty"].Value.ToString()),
                                    quantity = (string.IsNullOrEmpty(grid_purchases.Rows[i].Cells["qty"].Value.ToString()) ? 0 : decimal.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString())),
                                    cost_price = (string.IsNullOrEmpty(grid_purchases.Rows[i].Cells["avg_cost"].Value.ToString()) ? 0 : Math.Round(Convert.ToDecimal(grid_purchases.Rows[i].Cells["avg_cost"].Value.ToString()), 4)),// its avg cost actually ,
                                    unit_price = (string.IsNullOrEmpty(grid_purchases.Rows[i].Cells["unit_price"].Value.ToString()) ? 0 : Math.Round(decimal.Parse(grid_purchases.Rows[i].Cells["unit_price"].Value.ToString()), 4)),
                                    discount = (string.IsNullOrEmpty(grid_purchases.Rows[i].Cells["discount"].Value.ToString()) ? 0 : Math.Round(decimal.Parse(grid_purchases.Rows[i].Cells["discount"].Value.ToString()), 4)),
                                    tax_id = Convert.ToInt32(grid_purchases.Rows[i].Cells["tax_id"].Value.ToString()),
                                    tax_rate = tax_rate,
                                    purchase_date = purchase_date,
                                    location_code = location_code
                                });
                                //////////////
                            }

                        }

                        Int32 purchase_id = purchasesObj.Insertpurchases(purchase_model_header, purchase_model_detail);

                        if (purchase_id > 0)
                        {
                            MessageBox.Show(invoice_no + " transaction created successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            clear_form();
                            GetMAXInvoiceNo();

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

            txt_sub_total.Text = (total_sub_total).ToString("0.00");
        }

        private void get_total_amount()
        {
            total_amount = 0;

            for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
            {
                total_amount += Convert.ToDecimal(grid_purchases.Rows[i].Cells["qty"].Value) * Convert.ToDecimal(grid_purchases.Rows[i].Cells["avg_cost"].Value);
            }
            decimal net = (total_amount + total_tax - total_discount);
            txt_total_amount.Text = net.ToString("0.00");
        }

        private void get_total_tax()
        {
            total_tax = 0;

            for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
            {
                total_tax += Convert.ToDecimal(grid_purchases.Rows[i].Cells["tax"].Value);
            }

            txt_total_tax.Text = total_tax.ToString("0.00");
        }
        private void get_total_discount()
        {
            total_discount = 0;

            for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
            {
                total_discount += Convert.ToDecimal(grid_purchases.Rows[i].Cells["discount"].Value);
            }

            txt_total_discount.Text = total_discount.ToString("0.00");
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
                    string location_code = "";// myProductView["location_code"].ToString();
                    string unit = myProductView["unit"].ToString();
                    string category = myProductView["category"].ToString();
                    string btn_delete = "Del";
                    string tax_id = myProductView["tax_id"].ToString();


                    string[] row0 = { id.ToString(), code, name, 
                                            qty.ToString(), avg_cost.ToString(),unit_price.ToString(), discount.ToString(), 
                                            tax.ToString(), current_sub_total.ToString(),location_code,unit,category, 
                                            btn_delete, tax_id.ToString(), tax_rate.ToString(), unit_price.ToString() };
                    int RowIndex = grid_purchases.Rows.Add(row0);

                    //GET / SET Location Dropdown list
                    /////
                    fill_locations_grid_combo(RowIndex);
                    //////////

                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_amount();

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


        private void frm_purchases_v1_KeyDown(object sender, KeyEventArgs e)
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
                    btn_new.PerformClick();
                }
                if (e.KeyData == Keys.F3)
                {
                    btn_save.PerformClick();
                }
                if (e.Control && e.KeyCode == Keys.H)
                {
                    btn_movements.PerformClick();
                }

                if (e.Control && e.KeyCode == Keys.L)
                {
                    //frm_search_porder obj = new frm_search_porder(this);
                    //obj.ShowDialog();
                }
                
                if (e.Control && e.KeyCode == Keys.O)
                {
                    btn_search_purchase_invoices.PerformClick();
                }

                if (e.KeyCode == Keys.Escape)
                {
                    this.Controls.Remove(salesDataGridView);
                }
                if (e.KeyData == Keys.Down)
                {
                    salesDataGridView.Focus();
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
            
            txt_sub_total.Text = "0.00";
            txt_total_amount.Text = "0.00";
            txt_total_tax.Text = "0.00";
            txt_total_discount.Text = "0.00";

            rd_btn_by_unitprice.Checked = true;
            rd_btn_without_vat.Checked = true;

            grid_purchases.DataSource = null;
            grid_purchases.Rows.Clear();
            grid_purchases.Refresh();
            grid_purchases.Rows.Add();
            //this.ActiveControl = grid_purchases;

            
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want new sale transaction", "New Transaction", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                clear_form();
            }
            this.ActiveControl = grid_purchases;
        }

        public void get_suppliers_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,first_name";
            string table = "pos_suppliers";

            DataTable suppliers = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = suppliers.NewRow();
            emptyRow[0] = "0";              // Set Column Value
            emptyRow[1] = "Select Supplier";              // Set Column Value
            suppliers.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = suppliers.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[1] = "ADD NEW";              // Set Column Value
            suppliers.Rows.InsertAt(emptyRow1, 1);

            cmb_suppliers.DisplayMember = "first_name";
            cmb_suppliers.ValueMember = "id";
            cmb_suppliers.DataSource = suppliers;

        }
        private void cmb_suppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cmb_purchase_type.SelectedValue = "Credit";

                if (cmb_suppliers.SelectedValue.ToString() != null)
                {
                    int supplier_id = Convert.ToInt32(cmb_suppliers.SelectedValue.ToString());

                    SupplierBLL BLL_obj = new SupplierBLL();
                    DataTable suppliers = BLL_obj.SearchRecordBySupplierID(supplier_id);


                    foreach (DataRow dr in suppliers.Rows)
                    {
                        
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
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,first_name";
            string table = "pos_employees";

            DataTable employees = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = employees.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Select Employee";              // Set Column Value
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


            cmb_purchase_type.DataSource = dt;

            cmb_purchase_type.DisplayMember = "name";
            cmb_purchase_type.ValueMember = "id";

        }
        public void get_brands_dropdownlist()
        {
            //GeneralBLL generalBLL_obj = new GeneralBLL();
            //string keyword = "code,name";
            //string table = "pos_brands";

            //DataTable brands = generalBLL_obj.GetRecord(keyword, table);
            //DataRow emptyRow = brands.NewRow();
            //emptyRow[0] = "";              // Set Column Value
            //emptyRow[1] = "";              // Set Column Value
            //brands.Rows.InsertAt(emptyRow, 0);

            //cmb_brands.DataSource = brands;
            //cmb_brands.DisplayMember = "name";
            //cmb_brands.ValueMember = "code";

        }

        public void get_categories_dropdownlist()
        {
            //GeneralBLL generalBLL_obj = new GeneralBLL();
            //string keyword = "code,name";
            //string table = "pos_categories";

            //DataTable categories = generalBLL_obj.GetRecord(keyword, table);
            //DataRow emptyRow = categories.NewRow();
            //emptyRow[0] = "";              // Set Column Value
            //emptyRow[1] = "";              // Set Column Value
            //// Set Column Value
            //categories.Rows.InsertAt(emptyRow, 0);

            //cmb_categories.DataSource = categories;
            //cmb_categories.DisplayMember = "name";
            //cmb_categories.ValueMember = "code";

        }

        public void get_groups_dropdownlist()
        {
            //GeneralBLL generalBLL_obj = new GeneralBLL();
            //string keyword = "code,name";
            //string table = "pos_product_groups";

            //DataTable groups = generalBLL_obj.GetRecord(keyword, table);
            //DataRow emptyRow = groups.NewRow();
            //emptyRow[0] = "";              // Set Column Value
            //emptyRow[1] = "";              // Set Column Value
            //// Set Column Value
            //groups.Rows.InsertAt(emptyRow, 0);

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
                else
                {
                    invoice_status = "Update";
                }

                btn_save.Text = "Update (F3)";
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

                        double packet_qty = Convert.ToDouble(myProductView["packet_qty"].ToString()); 
                        double qty = Convert.ToDouble(myProductView["quantity"].ToString());
                        double total = qty * double.Parse(myProductView["cost_price"].ToString());
                        double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                        double tax = (total * tax_rate / 100);
                        double current_sub_total = tax + total;

                        int id = Convert.ToInt32(myProductView["item_id"]);
                        string code = myProductView["code"].ToString();
                        string name = myProductView["name"].ToString();
                        //string qty = myProductView["quantity"].ToString();
                        double cost_price = Convert.ToDouble(myProductView["cost_price"]);
                        double unit_price = Convert.ToDouble(myProductView["unit_price"]);
                        double discount = Convert.ToDouble(myProductView["discount_value"]);
                        string location_code = myProductView["location_code"].ToString();
                        string unit = myProductView["unit"].ToString();
                        string category = myProductView["category"].ToString();
                        string item_type = myProductView["item_type"].ToString();
                        string btn_delete = "Del";
                        string tax_id = myProductView["tax_id"].ToString();


                        string[] row0 = { id.ToString(), code, name, packet_qty.ToString(),
                                            qty.ToString(), cost_price.ToString(),unit_price.ToString(), discount.ToString(), 
                                            tax.ToString(), current_sub_total.ToString(),location_code,unit,category, 
                                            btn_delete, tax_id.ToString(), tax_rate.ToString(), unit_price.ToString() };
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
                    grid_purchases.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //frm_search_porder obj = new frm_search_porder(this);
            //obj.ShowDialog();
        }

        private void grid_purchases_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(tb_KeyPress);
            e.Control.KeyUp -= new KeyEventHandler(code_txtbox_KeyUp);

            if (grid_purchases.CurrentCell.ColumnIndex == 3 || grid_purchases.CurrentCell.ColumnIndex == 4 || grid_purchases.CurrentCell.ColumnIndex == 5 || grid_purchases.CurrentCell.ColumnIndex == 6) //qty, unit price and discount Column will accept only numeric
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
                }
            }


            if (grid_purchases.CurrentCell.ColumnIndex == 1)//code cell key press
            {
                TextBox code_txtbox = e.Control as TextBox;
                if (code_txtbox != null)
                {
                    code_txtbox.KeyUp += code_txtbox_KeyUp;
                }
            }

        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
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

                int iColumn = grid_purchases.CurrentCell.ColumnIndex;
                int iRow = grid_purchases.CurrentCell.RowIndex;
                string columnName = grid_purchases.Columns[iColumn].Name;

                string product_code = (grid_purchases.CurrentRow.Cells["code"].EditedFormattedValue.ToString() != "" ? grid_purchases.CurrentRow.Cells["code"].EditedFormattedValue.ToString() : "");

                if (product_code != "") // && product_code.Length >= 3
                {
                    // string product_code = grid_purchases.Rows[iRow].Cells[1].Value.ToString();
                    var brand_code = "";// txt_brand_code.Text; // (cmb_brands.SelectedValue != null ? cmb_brands.SelectedValue.ToString() : "");
                    var category_code = "";//txt_category_code.Text; // (cmb_categories.SelectedValue != null ? cmb_categories.SelectedValue.ToString() : "");
                    var group_code = "";//txt_group_code.Text; // (cmb_groups.SelectedValue != null ? cmb_groups.SelectedValue.ToString() : "");

                    SetupSalesDataGridView();

                    //ProductBLL objBLL = new ProductBLL();
                    string brand_name = "";//txt_brands.Text;

                    DataTable dt = ProductBLL.SearchProductByBrandAndCategory(product_code, category_code, brand_code, group_code);

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
                            string category = dr["category"].ToString();
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

        private void btn_search_purchase_invoices_Click(object sender, EventArgs e)
        {
            //frm_search_p_invoices frm = new frm_search_p_invoices(this);
            //frm.ShowDialog();
        }

        private void btn_movements_Click(object sender, EventArgs e)
        {

            string product_code = "";

            if (grid_purchases.Rows.Count > 0)
            {
                product_code = grid_purchases.CurrentRow.Cells["code"].Value.ToString();
            }

            frm_productsMovements frm_prod_move_obj = new frm_productsMovements(product_code);
            frm_prod_move_obj.ShowDialog();
        }

        private void productMovementF4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_movements.PerformClick();
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
       

        
    }
}
