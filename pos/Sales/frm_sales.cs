using com.sun.org.apache.bcel.@internal.generic;
using pos.Master.Companies.zatca;
using pos.Sales;
using POS.BLL;
using POS.Core;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Zatca.EInvoice.SDK;
using Zatca.EInvoice.SDK.Contracts.Models;


namespace pos
{
    public partial class frm_sales : Form
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
        public double total_cost_amount_e_vat = 0;
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
        private DataGridView categoriesDataGridView = new DataGridView();
        private DataGridView groupsDataGridView = new DataGridView();

        ProductBLL productsBLL_obj = new ProductBLL();

        public frm_sales()
        {
            InitializeComponent();
            txt_groups.Click += TextBoxOnClick;
            txt_categories.Click += TextBoxOnClick;
            txt_brands.Click += TextBoxOnClick;
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

        private void frm_sales_Load(object sender, EventArgs e)
        {
            this.Text = "Sale Invoice ";
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
            get_payment_terms_dropdownlist();
            get_payment_method_dropdownlist();
            get_saletype_dropdownlist();
            get_invoice_subtype_dropdownlist();

            if (lang == "en-US")
            {
                cmb_sale_type.SelectedValue = "Cash";
                cmb_invoice_subtype_code.SelectedValue = "02"; // 02 = Simplified invoice
            }
            else if (lang == "ar-SA")
            {
                cmb_sale_type.SelectedIndex = 0;
                cmb_invoice_subtype_code.SelectedIndex = 0;
            }

            Get_user_total_commission();

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

        Form frm_searchSaleProducts_obj;

        private void grid_sales_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (e.RowIndex < 0) return;

                int iColumn = grid_sales.CurrentCell.ColumnIndex;
                int iRow = grid_sales.CurrentCell.RowIndex;
                string columnName = grid_sales.Columns[e.ColumnIndex].Name;
                if (columnName == "code")
                {
                    
                    product_code = (grid_sales.CurrentRow.Cells["code"].Value != null ? grid_sales.CurrentRow.Cells["code"].Value.ToString() : "");

                    bool isGrid = true;
                    var brand_code = txt_brand_code.Text; // (cmb_brands.SelectedValue != null ? cmb_brands.SelectedValue.ToString() : "");
                    var category_code = txt_category_code.Text; // (cmb_categories.SelectedValue != null ? cmb_categories.SelectedValue.ToString() : "");
                    var group_code = txt_group_code.Text; // (cmb_groups.SelectedValue != null ? cmb_groups.SelectedValue.ToString() : "");

                    ////////////////////////
                    if (frm_searchSaleProducts_obj == null || product_code != "")
                    {
                        frm_searchSaleProducts_obj = new frm_searchSaleProducts(this, product_code, category_code, brand_code, isGrid, group_code);
                        frm_searchSaleProducts_obj.FormClosed += new FormClosedEventHandler(frm_searchSaleProducts_obj_FormClosed);

                        //frm_cust.Dock = DockStyle.Fill;
                        frm_searchSaleProducts_obj.ShowDialog();
                    }
                    else
                    {
                        //frm_searchSaleProducts_obj.ShowDialog();
                        //frm_searchSaleProducts_obj.BringToFront();
                        frm_searchSaleProducts_obj.Visible = true;
                    }
                    ////////////

                   
                }

                // Handle the end of editing for numeric columns (3, 4, 5, 6, 7)
                if (e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6 || e.ColumnIndex == 7)
                {
                    var cell = grid_sales.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    // If the cell value is null or empty, set it to 0
                    if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                    {
                        cell.Value = 0;
                    }
                }
                
                // Safely get values from cells
                double GetCellDouble(string colName)
                {
                    var val = grid_sales.Rows[e.RowIndex].Cells[colName].Value;
                    return val == null || val.ToString() == "" ? 0 : Convert.ToDouble(val);
                }

                double unitPrice = GetCellDouble("unit_price");
                double qty = GetCellDouble("qty");
                double discount = GetCellDouble("discount");
                double taxRate = GetCellDouble("tax_rate");

                double totalValue = unitPrice * qty;
                double subTotal = totalValue - discount;
                double tax = (subTotal * taxRate) / 100;

                // ----------------------------- QTY Changed -----------------------------
                if (columnName == "Qty")
                {
                    grid_sales.Rows[e.RowIndex].Cells["tax"].Value = tax;
                    grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = subTotal + tax;
                    grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = subTotal;
                }

                // --------------------------- UNIT PRICE Changed ------------------------
                if (columnName == "unit_price")
                {
                    double grossTotal = unitPrice * qty;
                    double discountValue = GetCellDouble("discount");
                    double netTotal = grossTotal - discountValue;
                    double tax_1 = (netTotal * taxRate) / 100;

                    if (rd_btn_without_vat.Checked && rd_btn_by_unitprice.Checked)
                    {
                        grid_sales.Rows[e.RowIndex].Cells["tax"].Value = tax_1;
                        grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = netTotal + tax_1;
                        grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = netTotal;
                    }

                    if (rd_btn_without_vat.Checked && rd_btn_bytotal_price.Checked)
                    {
                        double singleUnitPrice = unitPrice / qty;
                        double taxPerUnit = (singleUnitPrice * taxRate) / 100;
                        double totalTax = taxPerUnit * qty;

                        double newTotal = (singleUnitPrice * qty) - discountValue;
                        double discountPercent = grossTotal == 0 ? 0 : (discountValue / grossTotal) * 100;

                        grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value = singleUnitPrice;
                        grid_sales.Rows[e.RowIndex].Cells["tax"].Value = (newTotal * taxRate) / 100;
                        grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = newTotal + tax_1;
                        grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = newTotal;
                        grid_sales.Rows[e.RowIndex].Cells["discount_percent"].Value = discountPercent;
                    }

                    if (rd_btn_with_vat.Checked && rd_btn_by_unitprice.Checked)
                    {
                        double preTax = 1 + (taxRate / 100);
                        double netUnitPrice = unitPrice / preTax;
                        double grossTotalBeforeDiscount = netUnitPrice * qty;
                        double netTotal_1 = grossTotalBeforeDiscount - discountValue;
                        double taxAmount = (netTotal_1 * taxRate) / 100;

                        double discountPercent = grossTotalBeforeDiscount == 0 ? 0 : (discountValue / grossTotalBeforeDiscount) * 100;

                        grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value = netUnitPrice;
                        grid_sales.Rows[e.RowIndex].Cells["tax"].Value = taxAmount;
                        grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = netTotal_1 + taxAmount;
                        grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = netTotal_1;
                        grid_sales.Rows[e.RowIndex].Cells["discount_percent"].Value = discountPercent;
                    }

                    if (rd_btn_with_vat.Checked && rd_btn_bytotal_price.Checked)
                    {
                        double preTax = 1 + (taxRate / 100);
                        double netTotalWithoutTax = unitPrice / preTax;
                        double netUnitPrice = netTotalWithoutTax / qty;
                        double grossBeforeDiscount = netUnitPrice * qty;
                        double newNetTotal = grossBeforeDiscount - discountValue;
                        double taxAmount = (newNetTotal * taxRate) / 100;

                        double discountPercent = grossBeforeDiscount == 0 ? 0 : (discountValue / grossBeforeDiscount) * 100;

                        grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value = netUnitPrice;
                        grid_sales.Rows[e.RowIndex].Cells["tax"].Value = taxAmount;
                        grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = newNetTotal + taxAmount;
                        grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = newNetTotal;
                        grid_sales.Rows[e.RowIndex].Cells["discount_percent"].Value = discountPercent;
                    }
                }

                // --------------------------- DISCOUNT Changed --------------------------
                if (columnName == "discount")
                {
                    double discountPercent = totalValue == 0 ? 0 : (discount / totalValue) * 100;
                    tax = ((totalValue - discount) * taxRate) / 100;
                    double finalSubtotal = totalValue - discount + tax;

                    grid_sales.Rows[e.RowIndex].Cells["discount_percent"].Value = discountPercent;
                    grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = finalSubtotal;
                    grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = finalSubtotal - tax;
                    grid_sales.Rows[e.RowIndex].Cells["tax"].Value = tax;
                }

                // -------------------- DISCOUNT PERCENT Changed -------------------------
                if (columnName == "discount_percent")
                {
                    double discountPercent = GetCellDouble("discount_percent");
                    double discountValue = (discountPercent * totalValue) / 100;
                    tax = ((totalValue - discountValue) * taxRate) / 100;
                    double finalSubtotal = totalValue - discountValue + tax;

                    grid_sales.Rows[e.RowIndex].Cells["discount"].Value = discountValue;
                    grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = finalSubtotal;
                    grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = finalSubtotal - tax;
                    grid_sales.Rows[e.RowIndex].Cells["tax"].Value = tax;
                }

                //double tax_rate = (grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value == null || grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                //double sub_total = (Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value) * Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["qty"].Value)) - Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value);
                //double tax = (sub_total * tax_rate / 100);

                //if (columnName == "Qty") // if qty is changed
                //{
                //    grid_sales.Rows[e.RowIndex].Cells["tax"].Value = tax;
                //    grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total + tax;
                //    grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = sub_total;
                //}

                //if (columnName == "unit_price")//if avg_cost is changed
                //{
                //    if (rd_btn_without_vat.Checked && rd_btn_by_unitprice.Checked)
                //    {
                //        grid_sales.Rows[e.RowIndex].Cells["tax"].Value = tax;
                //        grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total + tax;
                //        grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = sub_total;
                //    }

                //    if (rd_btn_without_vat.Checked && rd_btn_bytotal_price.Checked)
                //    {
                //        double total_unitPrice_1 = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value);
                //        double qty_1 = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["qty"].Value) * 1;
                //        //double tax_rate = (grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                //        string pre_tax = "1." + tax_rate.ToString();
                //        double single_unitPrice = (total_unitPrice_1 / qty_1);
                //        tax = (single_unitPrice * tax_rate / 100);
                //        double new_tax_value = (tax * qty_1);

                //        grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value = single_unitPrice;

                //        grid_sales.Rows[e.RowIndex].Cells["tax"].Value = new_tax_value;

                //        sub_total = (single_unitPrice * qty_1) - Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value);

                //        grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total + new_tax_value;
                //        grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = sub_total;
                //    }

                //    if (rd_btn_with_vat.Checked && rd_btn_by_unitprice.Checked)
                //    {
                //        double total_unitPrice = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value);
                //        double qty = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["qty"].Value) * 1;
                //        //double tax_rate = (grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                //        string pre_tax = "1." + tax_rate.ToString();
                //        double single_unitPrice = (total_unitPrice / double.Parse(pre_tax));
                //        tax = (single_unitPrice * tax_rate / 100);
                //        double new_tax_value = (tax * qty);
                //        grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value = single_unitPrice;

                //        grid_sales.Rows[e.RowIndex].Cells["tax"].Value = new_tax_value;

                //        sub_total = (single_unitPrice * qty) - Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value);

                //        //grid_sales.Rows[e.RowIndex].Cells["avg_cost"].Value = (Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["avg_cost"].Value) - tax);
                //        grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total + new_tax_value;
                //        grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = sub_total;

                //    }

                //    if (rd_btn_with_vat.Checked && rd_btn_bytotal_price.Checked)
                //    {
                //        double total_unitPrice_1 = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value);
                //        double qty_1 = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["qty"].Value) * 1;
                //        //double tax_rate = (grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                //        string pre_tax = "1." + tax_rate.ToString();
                //        double single_unitPrice = ((total_unitPrice_1 / double.Parse(pre_tax)) / qty_1);
                //        tax = (single_unitPrice * tax_rate / 100);
                //        double new_tax_value = (tax * qty_1);

                //        grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value = single_unitPrice;

                //        grid_sales.Rows[e.RowIndex].Cells["tax"].Value = new_tax_value;

                //        sub_total = (single_unitPrice * qty_1) - Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value);
                //        grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total + new_tax_value;
                //        grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = sub_total;

                //    }
                //}

                //if (columnName == "discount")//if discount is changed
                //{
                //    double total_value = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value) * Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["qty"].Value);
                //    double discount_percent = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value) / total_value * 100;

                //    double tax_1 = ((total_value - Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value)) * tax_rate / 100);

                //    double sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value);
                //    //total_discount += Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value);
                //    //txt_total_discount.Text = total_amount.ToString();

                //    grid_sales.Rows[e.RowIndex].Cells["discount_percent"].Value = discount_percent;
                //    grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total_1;
                //    grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                //    grid_sales.Rows[e.RowIndex].Cells["tax"].Value = (tax_1);


                //}
                //if (columnName == "discount_percent")//if discount is changed
                //{
                //    double total_value = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value) * Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["qty"].Value);
                //    double discount_value = Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount_percent"].Value) * total_value / 100;

                //    //double tax_rate = (grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_sales.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                //    double tax_1 = ((total_value - discount_value) * tax_rate / 100);
                //    //grid_sales.Rows[e.RowIndex].Cells["tax"].Value = (tax * Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["qty"].Value));

                //    double sub_total_1 = (tax_1 + total_value - discount_value);
                //    //total_discount += Convert.ToDouble(grid_sales.Rows[e.RowIndex].Cells["discount"].Value);
                //    //txt_total_discount.Text = total_amount.ToString();

                //    grid_sales.Rows[e.RowIndex].Cells["discount"].Value = discount_value;
                //    grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total_1;
                //    grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                //    grid_sales.Rows[e.RowIndex].Cells["tax"].Value = (tax_1);
                //}

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

        public void Load_products_to_grid(string item_number)
        {

            try
            {
                DataTable product_dt = new DataTable();

                product_dt = productsBLL_obj.SearchRecordByProductNumber(item_number);

                int RowIndex = grid_sales.CurrentCell.RowIndex;

                if (product_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < grid_sales.RowCount; i++)
                    {
                        var grid_item_number = (grid_sales.Rows[i].Cells["item_number"].Value != null ? grid_sales.Rows[i].Cells["item_number"].Value : "");
                        if (grid_item_number.ToString() == item_number)
                        {
                            MessageBox.Show("Product already added", "Already exist", MessageBoxButtons.OK, MessageBoxIcon.Question);
                            grid_sales.CurrentCell = grid_sales.Rows[RowIndex].Cells["code"]; //make qty cell active
                                                                                              //grid_sales.CurrentCell.Selected = true;
                            grid_sales.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            grid_sales.CurrentCell = grid_sales.Rows[RowIndex].Cells["qty"]; //make qty cell active
                            grid_sales.CurrentCell.Selected = true;

                        }
                    }

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
                        grid_sales.Rows[RowIndex].Cells["qty"].Value = qty;
                        grid_sales.Rows[RowIndex].Cells["cost_price"].Value = Math.Round(Decimal.Parse(myProductView["avg_cost"].ToString()), 2);
                        grid_sales.Rows[RowIndex].Cells["unit_price"].Value = Math.Round(Decimal.Parse(myProductView["unit_price"].ToString()), 2);
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
                        grid_sales.Rows[RowIndex].Cells["item_number"].Value = myProductView["item_number"].ToString();

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }

        public void fill_locations_grid_combo(int RowIndex, string SelectedValue = "DEF", string product_id = "")
        {
            DataTable dt = new DataTable();
            var locationComboCell = new DataGridViewComboBoxCell();
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "loc_code as location_code";
            string table = "pos_product_stocks WHERE item_id=" + product_id + " AND  qty > 0 GROUP BY loc_code";

            dt = generalBLL_obj.GetRecord(keyword, table);

            //WHEN NO LOCATION ASSIGNED TO PRODUCT THEN ALL LOCATIONS SHALL BE LOADED
            if (dt.Rows.Count <= 0)
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

        public void load_products(string item_number = "", string product_name = "", string barcode = "")
        {

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
                    string location_code = myProductView["location_code"].ToString();
                    string unit = myProductView["unit"].ToString();
                    string category = myProductView["category"].ToString();
                    string btn_delete = "Del";

                    string shop_qty = myProductView["qty"].ToString();
                    string tax_id = myProductView["tax_id"].ToString();
                    string item_type = myProductView["item_type"].ToString();
                    string category_code = myProductView["category_code"].ToString();
                    string grid_item_number = myProductView["item_number"].ToString();

                    //double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : Convert.ToDouble(myProductView["tax_rate"]));
                    //double tax = (Convert.ToDouble(qty) * unit_price * tax_rate / 100);

                    //double current_sub_total = Convert.ToDouble(qty) * unit_price + tax;

                    string[] row0 = { id.ToString(), code, name, qty.ToString(), unit_price.ToString(), discount.ToString(), discount_percent.ToString(),
                                            sub_total_without_vat.ToString(),tax.ToString(), sub_total.ToString(),location_code,unit,category,
                                            btn_delete, shop_qty,tax_id.ToString(), tax_rate.ToString(), cost_price.ToString(),
                                            item_type,category_code,grid_item_number};

                    //Remove the first empty row
                    if (grid_sales.RowCount > 0 && grid_sales.Rows[0].Cells["id"].Value == null)
                    {
                        grid_sales.Rows.RemoveAt(0);
                    }
                    //
                    int RowIndex = grid_sales.Rows.Add(row0);

                    if (Convert.ToDouble(myProductView["qty"]) <= 0 || myProductView["qty"].ToString() == string.Empty)
                    {
                        grid_sales.CurrentRow.DefaultCellStyle.ForeColor = Color.Red;
                    }

                    //GET / SET Location Dropdown list
                    /////
                    //fill_locations_grid_combo(RowIndex,"",myProductView["id"].ToString());
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

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_sub_total += Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value);
            }

            txt_sub_total.Text = Math.Round(total_sub_total, 2).ToString();
            txt_sub_total_2.Text = Math.Round((total_sub_total - total_discount), 2).ToString();
        }

        private void get_total_cost_amount()
        {
            total_cost_amount = 0;
            total_cost_amount_e_vat = 0;
            double total_cost = 0;
            double tax_rate = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                //if tax is not assigned with product then assign zero
                if (grid_sales.Rows[i].Cells["tax_rate"].Value == null || grid_sales.Rows[i].Cells["tax_rate"].Value == DBNull.Value || String.IsNullOrWhiteSpace(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString()))
                {
                    tax_rate = 0;
                }
                else
                {
                    tax_rate = double.Parse(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString());
                }
                ////

                total_cost = Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["cost_price"].Value);

                total_cost_amount += total_cost;
                total_cost_amount_e_vat += (total_cost + (total_cost * tax_rate / 100));
            }

        }

        private void get_total_amount()
        { 
            total_amount = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_amount += Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value);
            }
            double net = (total_amount + total_tax - total_discount);
            txt_total_amount.Text = Math.Round(net, 2).ToString();
            double customerBalance = (txt_cust_balance.Text == string.Empty ? 0 : Convert.ToDouble(txt_cust_balance.Text));
            double customer_credit_limit = (txt_cust_credit_limit.Text == "" ? 0 : Convert.ToDouble(txt_cust_credit_limit.Text));
            double netAmount = (txt_total_amount.Text == string.Empty ? 0 : Convert.ToDouble(txt_total_amount.Text));
            double netCreditLimit = customer_credit_limit - customerBalance;
            double limitExceededBy = netAmount - netCreditLimit;

            ///Checking customer credit limit
            if (txt_cust_credit_limit.Text != "")
            {
                if (cmb_sale_type.SelectedValue.ToString() == "Credit" && netAmount > netCreditLimit)
                {
                    MessageBox.Show("Sales transaction cannot be saved, because customer credit limit has exceeded by " + limitExceededBy, "Credit limit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            ///

        }

        private void get_total_tax()
        {
            total_tax = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_tax += Convert.ToDouble(grid_sales.Rows[i].Cells["tax"].Value);
            }

            txt_total_tax.Text = Math.Round(total_tax, 2).ToString();
        }

        private void get_total_discount()
        {
            total_discount = 0;
            //double flatDiscountValue = Convert.ToDouble(txtTotalFlatDiscountValue.Value);
            //double new_total_discount =Convert.ToDouble(txt_total_disc_percent.Value);
            // double flatDiscountPercent = (total_amount * new_total_discount / 100);

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_discount += Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);
            }
            // total_discount += flatDiscountValue;
            //total_discount += flatDiscountPercent;

            txt_total_discount.Text = Math.Round(total_discount, 2).ToString();

            //double total_disc_percent = (string.IsNullOrEmpty(txt_total_disc_percent.Text) ? 0 : Convert.ToDouble(txt_total_disc_percent.Text));
            //if (total_disc_percent == 0)
            //{
            //    txt_total_discount.Text = Math.Round(total_discount,2).ToString();
            //}


        }

        private void get_total_qty()
        {
            double total_qty = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_qty += Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value);
            }

            txt_total_qty.Text = Math.Round(total_qty, 2).ToString();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {



        }

        private int Insert_Journal_entry(string invoice_no, int account_id, double debit, double credit, DateTime date,
            string description, int customer_id, int supplier_id, int entry_id, int employee_id = 0)
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



        private void frm_sales_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                //if (e.KeyData == Keys.Enter)
                //{
                //    SendKeys.Send("{TAB}");
                //}
                if (e.KeyData == Keys.F1)
                {
                    NewToolStripButton.PerformClick();
                }
                if (e.KeyData == Keys.F3)
                {
                    SaveToolStripButton.PerformClick();
                }
                if (e.Control && e.KeyCode == Keys.H)
                {
                    HistoryToolStripButton.PerformClick();
                }
                if (e.KeyCode == Keys.F4)
                {
                    SearchToolStripButton.PerformClick();
                }
                if (e.KeyCode == Keys.F5)
                {
                    AmountFixToolStripButton.PerformClick();
                }
                if (e.KeyCode == Keys.F6)
                {
                    //chk_print_invoice.Checked = !chk_print_invoice.Checked;
                }
                if (e.KeyCode == Keys.F9)
                {
                    grid_sales.Focus();
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
                if (e.Control && e.KeyCode == Keys.O)
                {
                    SearchToolStripButton.PerformClick();
                }
                if (e.Control && e.KeyCode == Keys.L)
                {
                    frm_search_estimates obj_estimate = new frm_search_estimates(this);
                    obj_estimate.ShowDialog();
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                double netAmount = (total_amount + total_tax-total_discount);
                netAmount -= total_tax;
                txt_total_amount.Text = netAmount.ToString();
            }
            else
            {
                txt_total_tax.Text = total_tax.ToString();
                double netAmount = (total_amount + total_tax - total_discount);
                //netAmount += total_tax;
                txt_total_amount.Text = netAmount.ToString();
            }
        }

        private void btn_movements_Click(object sender, EventArgs e)
        {

        }

        private void clear_form()
        {
            Get_user_total_commission();
            // grid_sales.DataSource = null;
            grid_sales.Rows.Clear();
            grid_sales.Refresh();
            grid_sales.Rows.Add();

            txt_description.Text = "";
            PrinttoolStripButton.Enabled = false;
            SaleReturnToolStripButton.Enabled = false;
            //cmb_brands.SelectedValue = 0;
            //cmb_customers.Refresh();

            txt_group_code.Text = "";
            txt_groups.Text = "";
            txt_category_code.Text = "";
            txt_categories.Text = "";
            txt_brands.Text = "";
            txt_brand_code.Text = "";

            invoice_status = "";
            //btn_save.Text = "Save (F3)";
            txt_invoice_no.Text = "";

            total_amount = 0;
            total_discount = 0;
            total_tax = 0;
            total_sub_total = 0;
            total_cost_amount = 0;
            total_cost_amount_e_vat = 0;

            txt_sale_date.Refresh();
            txt_sale_date.Value = DateTime.Now;

            txt_cust_balance.Text = "";
            txt_cust_credit_limit.Text = "";
            txt_customer_vat.Text = "";

            txt_sub_total.Text = "0.00";
            txt_total_amount.Text = "0.00";
            txt_total_tax.Text = "0.00";
            txt_total_discount.Text = "0.00";
            txt_total_amount.Text = "";

            txt_total_disc_percent.Value = 0;
            txtTotalFlatDiscountValue.Value = 0;

            txt_total_qty.Text = "";
            txt_sub_total_2.Text = "";

            txt_order_qty.Text = "";
            txt_total_cost.Text = "";
            txt_cost_price.Text = "";
            txt_cost_price_with_vat.Text = "";
            txt_single_cost_evat.Text = "";
            txt_single_cost_evat.Text = "";

            txt_shop_qty.Text = "";
            txt_company_qty.Text = "";
            txtPONumber.Text = "";
            txt_user_commission_balance.Text = "";

            cmb_employees.SelectedValue = 0;
            cmb_sale_type.SelectedValue = "Cash";
            cmb_customers.SelectedValue = 0;
            //cmb_categories.SelectedValue = 0;

            this.ActiveControl = grid_sales;

        }

        private void btn_new_Click(object sender, EventArgs e)
        {

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
            CustomerBLL customerBLL = new CustomerBLL();
            DataTable customers = customerBLL.GetAll();

            DataRow emptyRow = customers.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[2] = "";              // Set Column Value
            customers.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = customers.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[2] = "ADD NEW";              // Set Column Value
            customers.Rows.InsertAt(emptyRow1, 1);

            cmb_customers.DisplayMember = "first_name";
            cmb_customers.ValueMember = "id";
            cmb_customers.DataSource = customers;


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

        public void get_payment_terms_dropdownlist()
        {
            PaymentTermsBLL paymentMethodBLL = new PaymentTermsBLL();

            DataTable payment_terms = paymentMethodBLL.GetAll();
            DataRow emptyRow = payment_terms.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[4] = "";              // Set Column Value
            payment_terms.Rows.InsertAt(emptyRow, 0);


            cmb_payment_terms.DisplayMember = "description";
            cmb_payment_terms.ValueMember = "id";
            cmb_payment_terms.DataSource = payment_terms;


        }

        public void get_payment_method_dropdownlist()
        {
            PaymentMethodBLL paymentMethodBLL = new PaymentMethodBLL();

            DataTable payment_method = paymentMethodBLL.GetAll();
            DataRow emptyRow = payment_method.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "";              // Set Column Value
            //payment_method.Rows.InsertAt(emptyRow, 0);


            cmb_payment_method.DisplayMember = "description";
            cmb_payment_method.ValueMember = "id";
            cmb_payment_method.DataSource = payment_method;


        }
        private void cmb_customers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //cmb_sale_type.SelectedValue = (allow_credit_sales ? "Credit" : "Cash"); //if user has no right then select cash instead

                if (cmb_customers.SelectedValue != null && cmb_customers.SelectedValue.ToString() != "0")
                {
                    int customer_id = Convert.ToInt32(cmb_customers.SelectedValue.ToString());

                    CustomerBLL customerBLL_obj = new CustomerBLL();
                    DataTable customers = customerBLL_obj.SearchRecordByCustomerID(customer_id);


                    foreach (DataRow dr in customers.Rows)
                    {
                        txt_customer_vat.Text = dr["vat_no"].ToString();

                        txt_cust_credit_limit.Text = dr["credit_limit"].ToString();
                    }

                    ///customer balance
                    DataTable customer_total_balance = customerBLL_obj.GetCustomerAccountBalance(customer_id);
                    ///
                    foreach (DataRow dr in customer_total_balance.Rows)
                    {
                        txt_cust_balance.Text = dr["balance"].ToString();
                    }
                }


                if (cmb_customers.SelectedValue != null && cmb_customers.SelectedValue.ToString() == "-1")
                {
                    frm_addCustomer custfrm = new frm_addCustomer();
                    custfrm.ShowDialog();


                    get_customers_dropdownlist();
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_round_prices_Click(object sender, EventArgs e)
        {

        }

        public void round_total_amount(double new_amount, double old_total_amount, double sub_total)
        {
            try
            {
                //double total_diff_amount = old_total_amount - new_amount; // calculate difference amount and add to single item unit price
                //double total_item_share = Math.Round(total_diff_amount / sub_total * 100,2);
                //if (total_diff_amount != 0)
                //{
                //    if(radioDiscValue.Checked)
                //    {
                //        txtTotalFlatDiscountValue.Value += Convert.ToDecimal(total_diff_amount);

                //    }else if(radioDiscPercent.Checked)
                //    {
                //        txt_total_disc_percent.Value += Convert.ToDecimal(total_item_share);
                //    }
                //}

                double total_rows = 0;
                //txt_total_amount.Text = round_total_amount.ToString();
                for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
                {
                    if (grid_sales.Rows[i].Cells["qty"].Value != null)
                    {
                        total_rows++;
                    }
                }

                double total_diff_amount = old_total_amount - new_amount; // calculate difference amount and add to single item unit price
                double total_item_share = sub_total * 100 / old_total_amount;
                double total_tax_share = (old_total_amount - sub_total) * 100 / old_total_amount;
                double diff_amount_per_item = total_diff_amount / total_rows;

                double new_amount_total = 0;
                double new_amount_single = 0;
                double new_vat_total = 0;
                double net_total = 0;

                if (total_diff_amount != 0)
                {
                    for (int i = 0; i <= total_rows - 1; i++)
                    {
                        if (grid_sales.Rows[i].Cells["qty"].Value != null)
                        {
                            new_amount_single = (double.Parse(grid_sales.Rows[i].Cells["sub_total"].Value.ToString()) / double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString())) - (diff_amount_per_item / double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString()));
                            new_amount_total = new_amount_single * total_item_share / 100;
                            new_vat_total = new_amount_single * total_tax_share / 100;
                            net_total = (grid_sales.Rows[i].Cells["tax"].Value.ToString() == "0" ? (new_amount_total + new_vat_total) : new_amount_total);
                            grid_sales.Rows[i].Cells["unit_price"].Value = net_total;

                            double tax_rate = (grid_sales.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString()));

                            //grid_sales.Rows[i].Cells["sub_total"].Value = Convert.ToDouble(grid_sales.Rows[i].Cells["sub_total"].Value) - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);

                            double total_value = Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value);

                            double tax_1 = ((total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value)) * tax_rate / 100);

                            double sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);

                            grid_sales.Rows[i].Cells["sub_total"].Value = sub_total_1;
                            //grid_sales.Rows[i].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                            grid_sales.Rows[i].Cells["tax"].Value = (tax_1);
                        }

                    }

                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_cost_amount();
                    get_total_amount();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                _row_2["name"] = "عرض سعر";
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

            DataRow _row_4 = dt.NewRow();
            _row_4["id"] = "ICT";
            if (lang == "en-US")
            {
                _row_4["name"] = "ICT";
            }

            dt.Rows.Add(_row_4);


            //DataRow _row_4 = dt.NewRow();
            //_row_4["id"] = "Return";
            //if (lang == "en-US")
            //{
            //    _row_4["name"] = "Return";
            //}
            //else if (lang == "ar-SA")
            //{
            //    _row_4["name"] = "يعود";
            //}
            //dt.Rows.Add(_row_4);

            cmb_sale_type.DisplayMember = "name";
            cmb_sale_type.ValueMember = "id";
            cmb_sale_type.DataSource = dt;

        }
        private void get_invoice_subtype_dropdownlist()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            DataRow _row_1 = dt.NewRow();
            _row_1["id"] = "02";

            if (lang == "en-US")
            {
                _row_1["name"] = "Simplified";
            }
            else if (lang == "ar-SA")
            {
                _row_1["name"] = "مبسطة";
            }

            dt.Rows.Add(_row_1);

           
            DataRow _row_2 = dt.NewRow();
            _row_2["id"] = "01";

            if (lang == "en-US")
            {
                _row_2["name"] = "Standard";
            }
            else if (lang == "ar-SA")
            {
                _row_2["name"] = "ضريبية";
            }

            dt.Rows.Add(_row_2);

            cmb_invoice_subtype_code.DisplayMember = "name";
            cmb_invoice_subtype_code.ValueMember = "id";
            cmb_invoice_subtype_code.DataSource = dt;

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
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    if (invoice_chr.ToUpper() == "S")
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
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    invoice_status = "Update"; //"Estimate";// 
                    //btn_save.Text = "Update (F3)"; //"Save"; //
                    PrinttoolStripButton.Enabled = true;
                    SaleReturnToolStripButton.Enabled = true;
                }
                else // for estimates 
                {
                    invoice_status = "Estimate";
                    //btn_save.Text = "Sale (F3)";
                    //cmb_invoice_type.Enabled = false;

                }

                if (_dt.Rows.Count > 0)
                {

                    foreach (DataRow myProductView in _dt.Rows)
                    {
                        cmb_customers.SelectedValue = myProductView["customer_id"];
                        cmb_employees.SelectedValue = myProductView["employee_id"];
                        cmb_sale_type.SelectedValue = myProductView["sale_type"];
                        //txt_sale_date.Value = Convert.ToDateTime(myProductView["sale_date"].ToString());
                        txt_description.Text = myProductView["description"].ToString();
                        total_amount = Convert.ToDouble(myProductView["total_amount"].ToString());
                        txt_total_disc_percent.Value = (myProductView["total_disc_percent"] == null || myProductView["total_disc_percent"].ToString() == "" ? 0 : Math.Round(Convert.ToDecimal(myProductView["total_disc_percent"]), 2));
                        txtTotalFlatDiscountValue.Value = (myProductView["flatDiscountValue"] == null || myProductView["flatDiscountValue"].ToString() == "" ? 0 : Math.Round(Convert.ToDecimal(myProductView["flatDiscountValue"]), 2));

                        double qty = Convert.ToDouble(myProductView["quantity_sold"].ToString());
                        double discount = Math.Round(Convert.ToDouble(myProductView["discount_value"]), 3);
                        double total = qty * double.Parse(myProductView["unit_price"].ToString());
                        double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                        double tax = ((total - discount) * tax_rate / 100);
                        double sub_total = tax + total;
                        double sub_total_without_vat = total - discount;

                        int id = Convert.ToInt32(myProductView["id"]);
                        string code = myProductView["code"].ToString();
                        string name = myProductView["name"].ToString();
                        double cost_price = Math.Round(Convert.ToDouble(myProductView["cost_price"]), 2);
                        double unit_price = Math.Round(Convert.ToDouble(myProductView["unit_price"]), 3);

                        double total_value = Convert.ToDouble(myProductView["unit_price"]) * Convert.ToDouble(myProductView["quantity_sold"].ToString());
                        double discount_percent = Convert.ToDouble(myProductView["discount_percent"]); //Convert.ToDouble(myProductView["discount_value"]) / total_value * 100;

                        string location_code = myProductView["loc_code"].ToString();
                        string unit = myProductView["unit"].ToString();
                        string category = myProductView["category"].ToString();
                        string item_type = myProductView["item_type"].ToString();
                        string btn_delete = "Del";
                        string tax_id = myProductView["tax_id"].ToString();

                        string shop_qty = "0"; // myProductView["qty"].ToString();
                        string category_code = myProductView["category_code"].ToString();
                        string item_number = myProductView["item_number"].ToString();

                        double current_sub_total = Convert.ToDouble(qty) * unit_price + tax - discount;


                        string[] row0 = { id.ToString(), code, name, qty.ToString(), unit_price.ToString(), discount.ToString(), discount_percent.ToString(),
                                            sub_total_without_vat.ToString(),tax.ToString(), current_sub_total.ToString(),location_code,unit,category,
                                            btn_delete, shop_qty,tax_id.ToString(), tax_rate.ToString(), cost_price.ToString(),
                                            item_type,category_code,item_number};
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
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void addToPurchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(grid_sales.CurrentRow.Cells["code"].Value.ToString());
                if (grid_sales.RowCount > 0)
                {
                    if (grid_sales.CurrentRow.Cells["code"].Value != null && grid_sales.CurrentRow.Cells["id"].Value != null)
                    {
                        int id = int.Parse(grid_sales.CurrentRow.Cells["id"].Value.ToString());
                        string item_number = grid_sales.CurrentRow.Cells["item_number"].Value.ToString();
                        string name = grid_sales.CurrentRow.Cells["name"].Value.ToString();
                        string category_code = grid_sales.CurrentRow.Cells["category_code"].Value.ToString();
                        double cost_price = double.Parse(grid_sales.CurrentRow.Cells["cost_price"].Value.ToString());
                        double unit_price = double.Parse(grid_sales.CurrentRow.Cells["unit_price"].Value.ToString());


                        frm_add_porder porder_obj = new frm_add_porder(this, id, item_number, name, category_code, cost_price, unit_price);
                        porder_obj.ShowDialog();
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void grid_sales_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            e.Control.KeyPress -= new KeyPressEventHandler(tb_KeyPress);

            if (grid_sales.CurrentCell.ColumnIndex == 3 || grid_sales.CurrentCell.ColumnIndex == 4 || grid_sales.CurrentCell.ColumnIndex == 5
                || grid_sales.CurrentCell.ColumnIndex == 6 || grid_sales.CurrentCell.ColumnIndex == 7) //qty, unit price and discount Column will accept only numeric
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

            // Allow only digits, control keys (Backspace, Delete), and one decimal point
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }

            // Prevent multiple decimal points
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HistoryToolStripButton.PerformClick();
        }

        public class Grid : DataGridView
        {
            protected override void OnHandleCreated(EventArgs e)
            {
                // Touching the TopLeftHeaderCell here prevents
                // System.InvalidOperationException:
                // This operation cannot be performed while
                // an auto-filled column is being resized.

                var topLeftHeaderCell = TopLeftHeaderCell;

                base.OnHandleCreated(e);
            }
        }

        private void btn_search_invioces_Click(object sender, EventArgs e)
        {

        }

        private void chk_show_total_cost_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_show_total_cost.Checked)
            {
                txt_cost_price_with_vat.Visible = true;
                lbl_cost_vat.Visible = true;
                lbl_cost_price.Visible = true;
                lbl_cost_price_evat.Visible = true;
                lbl_total_cost.Visible = true;
                txt_total_cost.Visible = true;
                txt_cost_price.Visible = true;
                txt_single_cost_evat.Visible = true;
            }
            else
            {
                //txt_total_cost.Text = "0";
                lbl_cost_price_evat.Visible = false;
                lbl_cost_vat.Visible = false;
                txt_cost_price_with_vat.Visible = false;
                txt_total_cost.Visible = false;
                lbl_cost_price.Visible = false;
                lbl_total_cost.Visible = false;
                txt_cost_price.Visible = false;
                txt_single_cost_evat.Visible = false;
            }
        }

        private void productDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_sales.RowCount > 0)
                {
                    if (grid_sales.CurrentRow.Cells["item_number"].Value != null)
                    {
                        string item_number = grid_sales.CurrentRow.Cells["item_number"].Value.ToString();

                        frm_product_full_detail obj = new frm_product_full_detail(this, null, item_number);
                        obj.ShowDialog();
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
            if (grid_sales.Rows.Count > 0 && grid_sales.CurrentRow.Cells["shop_qty"].Value != null && grid_sales.CurrentRow.Cells["shop_qty"].Value.ToString() != "")
            {
                string item_number = grid_sales.CurrentRow.Cells["item_number"].Value.ToString();
                double tax_rate = (grid_sales.CurrentRow.Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.CurrentRow.Cells["tax_rate"].Value.ToString()));
                if (item_number != null)
                {
                    string shop_qty = "";
                    string company_qty = "";
                    string avg_cost = "";

                    DataTable dt = productsBLL_obj.GetAllByProductByItemNumber(item_number);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow myProductView in dt.Rows)
                        {
                            shop_qty = myProductView["qty"].ToString();
                            company_qty = myProductView["company_qty"].ToString();
                            avg_cost = Math.Round(double.Parse(myProductView["avg_cost"].ToString()), 4).ToString();
                            //total_cost += Math.Round(Convert.ToDouble(myProductView["avg_cost"].ToString()),4);

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
                        txt_company_qty.Text = company_qty;
                        double tax = (Convert.ToDouble(avg_cost) * tax_rate / 100);
                        txt_cost_price.Text = Math.Round(Convert.ToDouble(avg_cost), 3).ToString();
                        txt_total_cost.Text = Math.Round(total_cost_amount, 3).ToString();
                        txt_cost_price_with_vat.Text = Math.Round(total_cost_amount_e_vat, 3).ToString(); // (total_cost_amount + (total_cost_amount * tax_rate / 100)).ToString();
                        txt_single_cost_evat.Text = (Math.Round((Convert.ToDouble(avg_cost) + tax), 3)).ToString();

                        Purchases_orderBLL poBLL = new Purchases_orderBLL();
                        txt_order_qty.Text = poBLL.GetPOrder_qty(item_number).ToString();
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
                    frm_addProduct frm_addProduct_obj = new frm_addProduct(null, int.Parse(product_id), "true", null, "", this);
                    frm_addProduct_obj.WindowState = FormWindowState.Maximized;
                    frm_addProduct_obj.ShowDialog();
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
            txt_user_commission_balance.Text = user_commission_total_balance.ToString();
            ///
        }

        private void addNewRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid_sales.Rows.Add();
        }

        private void SetupBrandDataGridView()
        {
            var current_lang_code = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

            brandsDataGridView.ColumnCount = 2;
            int xLocation = groupBox_products.Location.X + txt_brands.Location.X;
            int yLocation = groupBox_products.Location.Y + txt_brands.Location.Y + 22;

            brandsDataGridView.Name = "brandsDataGridView";
            if (current_lang_code == "en-US")
            {
                brandsDataGridView.Location = new Point(xLocation, yLocation);
                brandsDataGridView.Size = new Size(250, 250);
            }
            else if (current_lang_code == "ar-SA")
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
                grid_sales.Focus();
            }
        }

        private void brandsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(brandsDataGridView);
            grid_sales.Focus();

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
            var current_lang_code = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;
            categoriesDataGridView.ColumnCount = 2;
            int xLocation = groupBox_products.Location.X + txt_categories.Location.X + 5;
            int yLocation = groupBox_products.Location.Y + txt_categories.Location.Y + 22;

            categoriesDataGridView.Name = "categoriesDataGridView";
            if (current_lang_code == "en-US")
            {
                categoriesDataGridView.Location = new Point(xLocation, yLocation);
                categoriesDataGridView.Size = new Size(250, 250);

            }
            else if (current_lang_code == "ar-SA")
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
                grid_sales.Focus();

            }
        }

        private void categoriesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_category_code.Text = categoriesDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_categories.Text = categoriesDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(categoriesDataGridView);
            grid_sales.Focus();

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
            var current_lang_code = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

            groupsDataGridView.ColumnCount = 2;

            int xLocation = groupBox_products.Location.X + txt_groups.Location.X;
            int yLocation = groupBox_products.Location.Y + txt_groups.Location.Y + 22;

            groupsDataGridView.Name = "groupsDataGridView";
            if (current_lang_code == "en-US")
            {
                groupsDataGridView.Location = new Point(xLocation, yLocation);
                groupsDataGridView.Size = new Size(250, 250);
            }
            else if (current_lang_code == "ar-SA")
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
                grid_sales.Focus();

            }
        }

        private void groupsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_group_code.Text = groupsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_groups.Text = groupsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(groupsDataGridView);
            grid_sales.Focus();

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

        private void link_load_estimates_Click(object sender, EventArgs e)
        {
            frm_search_estimates obj_estimate = new frm_search_estimates(this);
            obj_estimate.ShowDialog();
        }

        private void txt_amount_received_KeyUp(object sender, KeyEventArgs e)
        {
            double received_amount = (txt_amount_received.Text == "" ? 0 : double.Parse(txt_amount_received.Text));
            double total_amount = (txt_total_amount.Text == "" ? 0 : double.Parse(txt_total_amount.Text));
            txt_change_amount.Text = (total_amount - received_amount).ToString();
        }


        private void grid_sales_DataError(object sender, DataGridViewDataErrorEventArgs anError)
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

        private void btn_ict_Click(object sender, EventArgs e)
        {

        }

        private void radioDiscValue_CheckedChanged(object sender, EventArgs e)
        {
            if (radioDiscValue.Checked)
            {
                txtTotalFlatDiscountValue.Enabled = true;
                txt_total_disc_percent.Value = 0;
                total_discount_percent(Convert.ToDouble(txt_total_disc_percent.Value));
                txt_total_disc_percent.Enabled = false;
            }
        }

        private void radioDiscPercent_CheckedChanged(object sender, EventArgs e)
        {
            if (radioDiscPercent.Checked)
            {
                txt_total_disc_percent.Enabled = true;
                txtTotalFlatDiscountValue.Value = 0;
                total_discount_value(Convert.ToDouble(txtTotalFlatDiscountValue.Value));

                txtTotalFlatDiscountValue.Enabled = false;
            }
        }

        private void txtTotalFlatDiscountValue_TextChanged(object sender, EventArgs e)
        {
            get_total_tax();
            get_total_discount();
            get_sub_total_amount();
            get_total_cost_amount();
            get_total_amount();
        }
        private void txt_total_disc_percent_TextChanged(object sender, EventArgs e)
        {
            get_total_tax();
            get_total_discount();
            get_sub_total_amount();
            get_total_cost_amount();
            get_total_amount();
        }

        public void total_discount_value(double total_discount_value)
        {
            int total_rows = grid_sales.Rows.Count;
            int filled_rows = 0;

            for (int i = 0; i <= total_rows - 1; i++)
            {
                int product_id = Convert.ToInt32(grid_sales.Rows[i].Cells["id"].Value);
                if (product_id > 0)
                {
                    filled_rows++;
                }
            }
            //txt_total_amount.Text = round_total_amount.ToString();
            //double total_diff_amount = old_total_amount - new_amount; // calculate difference amount and add to single item unit price
            //double total_item_share = sub_total * 100 / old_total_amount;
            //double total_tax_share = (old_total_amount - sub_total) * 100 / old_total_amount;
            double diff_amount_per_item = total_discount_value / filled_rows;

            //double new_amount_total = 0;
            //double new_amount_single = 0;
            //double new_vat_total = 0;
            //double net_total = 0;

            double discount_percent = 0;
            double tax_1 = 0;
            double total_value = 0;
            double tax_rate = 0;
            double sub_total_1 = 0;

            for (int i = 0; i <= filled_rows - 1; i++)
            {
                int product_id = Convert.ToInt32(grid_sales.Rows[i].Cells["id"].Value);
                if (product_id > 0)
                {
                    grid_sales.Rows[i].Cells["discount"].Value = diff_amount_per_item;

                    discount_percent = diff_amount_per_item / total_amount * 100;
                    grid_sales.Rows[i].Cells["discount_percent"].Value = Math.Round(discount_percent, 4).ToString();

                    //new_amount_single = (double.Parse(grid_sales.Rows[i].Cells["sub_total"].Value.ToString()) / double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString())) - (diff_amount_per_item / double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString()));
                    //new_amount_total = new_amount_single * total_item_share / 100;
                    //new_vat_total = new_amount_single * total_tax_share / 100;
                    //net_total = (grid_sales.Rows[i].Cells["tax"].Value.ToString() == "0" ? (new_amount_total + new_vat_total) : new_amount_total);
                    //grid_sales.Rows[i].Cells["unit_price"].Value = net_total;

                    tax_rate = (grid_sales.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString()));

                    ////grid_sales.Rows[i].Cells["sub_total"].Value = Convert.ToDouble(grid_sales.Rows[i].Cells["sub_total"].Value) - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);

                    total_value = Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value);

                    tax_1 = ((total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value)) * tax_rate / 100);

                    sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);

                    grid_sales.Rows[i].Cells["sub_total"].Value = sub_total_1;
                    ////grid_sales.Rows[i].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                    grid_sales.Rows[i].Cells["tax"].Value = (tax_1);


                }

            }
            get_total_tax();
            get_total_discount();
            get_sub_total_amount();
            get_total_cost_amount();
            get_total_amount();
            get_total_qty();
        }

        private void txtTotalFlatDiscountValue_ValueChanged(object sender, EventArgs e)
        {
            total_discount_value(Convert.ToDouble(txtTotalFlatDiscountValue.Text));

            //get_total_tax();
            //get_total_discount();
            //get_sub_total_amount();
            //get_total_cost_amount();
            //get_total_amount();
        }

        public void total_discount_percent(double total_discount_percent)
        {
            int total_rows = grid_sales.Rows.Count;
            int filled_rows = 0;

            for (int i = 0; i <= total_rows - 1; i++)
            {
                int product_id = Convert.ToInt32(grid_sales.Rows[i].Cells["id"].Value);
                if (product_id > 0)
                {
                    filled_rows++;
                }
            }
            //txt_total_amount.Text = round_total_amount.ToString();
            //double total_diff_amount = old_total_amount - new_amount; // calculate difference amount and add to single item unit price
            //double total_item_share = sub_total * 100 / old_total_amount;
            //double total_tax_share = (old_total_amount - sub_total) * 100 / old_total_amount;
            double diff_percent_per_item = total_discount_percent / filled_rows;

            //double new_amount_total = 0;
            //double new_amount_single = 0;
            //double new_vat_total = 0;
            //double net_total = 0;

            double discount_value = 0;
            double tax_1 = 0;
            double total_value = 0;
            double tax_rate = 0;
            double sub_total_1 = 0;

            for (int i = 0; i <= filled_rows - 1; i++)
            {
                int product_id = Convert.ToInt32(grid_sales.Rows[i].Cells["id"].Value);
                if (product_id > 0)
                {
                    grid_sales.Rows[i].Cells["discount_percent"].Value = diff_percent_per_item;

                    discount_value = total_amount * diff_percent_per_item / 100;
                    grid_sales.Rows[i].Cells["discount"].Value = Math.Round(discount_value, 4).ToString();

                    //new_amount_single = (double.Parse(grid_sales.Rows[i].Cells["sub_total"].Value.ToString()) / double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString())) - (diff_amount_per_item / double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString()));
                    //new_amount_total = new_amount_single * total_item_share / 100;
                    //new_vat_total = new_amount_single * total_tax_share / 100;
                    //net_total = (grid_sales.Rows[i].Cells["tax"].Value.ToString() == "0" ? (new_amount_total + new_vat_total) : new_amount_total);
                    //grid_sales.Rows[i].Cells["unit_price"].Value = net_total;

                    tax_rate = (grid_sales.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString()));

                    ////grid_sales.Rows[i].Cells["sub_total"].Value = Convert.ToDouble(grid_sales.Rows[i].Cells["sub_total"].Value) - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);

                    total_value = Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value);

                    tax_1 = ((total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value)) * tax_rate / 100);

                    sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);

                    grid_sales.Rows[i].Cells["sub_total"].Value = sub_total_1;
                    ////grid_sales.Rows[i].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                    grid_sales.Rows[i].Cells["tax"].Value = (tax_1);


                }

            }
            get_total_tax();
            get_total_discount();
            get_sub_total_amount();
            get_total_cost_amount();
            get_total_amount();
            get_total_qty();

        }

        private void txt_total_disc_percent_ValueChanged(object sender, EventArgs e)
        {
            total_discount_percent(Convert.ToDouble(txt_total_disc_percent.Text));
        }

        private void frm_sales_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (grid_sales.RowCount > 0 && grid_sales.CurrentRow.Cells["code"].Value != null && grid_sales.CurrentRow.Cells["id"].Value != null)
            {
                if (MessageBox.Show("Are you sure you want to close sale?",
                      "Close Sale Transaction",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;

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
                //txt_product_code.Focus();
            }
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_sales.Rows.Count <= 1 && grid_sales.CurrentRow.Cells["code"].Value == null)
                {
                    MessageBox.Show("Please add products ", "Sale Transaction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string bankID = "";
                string bankGLAccountID = "";
                string sale_type;
                string paymentMethodText = cmb_payment_method.Text;

                if (cmb_sale_type.SelectedValue.ToString() == null)
                {
                    MessageBox.Show("Are you sure you want to " + invoice_status, invoice_status + " Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    sale_type = (string.IsNullOrEmpty(cmb_sale_type.SelectedValue.ToString()) ? "Cash" : cmb_sale_type.SelectedValue.ToString());
                }

                int destination_branch_id = 0;
                int source_branch_id = UsersModal.logged_in_branch_id;

                // DialogResult result = MessageBox.Show("Are you sure you want to sale", "Sale Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (sale_type == "Cash" && (paymentMethodText.Contains("Bank") || paymentMethodText.Contains("bank") || paymentMethodText.Contains("banks") || paymentMethodText.Contains("Banks")))
                {
                    Master.Banks.frm_banksPopup bankfrm = new Master.Banks.frm_banksPopup();
                    bankfrm.ShowDialog();
                    string bankIDPlusGLAccountID = bankfrm._bankIDPlusGLAccountID;

                    int condition_index_len = bankIDPlusGLAccountID.IndexOf("+");
                    bankID = bankIDPlusGLAccountID.Substring(0, condition_index_len).Trim();
                    bankGLAccountID = bankIDPlusGLAccountID.Substring(condition_index_len + 1).Trim();

                }

                //if(string.IsNullOrEmpty(bankGLAccountID) || bankGLAccountID == "0")
                //{
                //    MessageBox.Show("The Selected bank does not have GL Account linked. Please link GL Account to your bank and try again", "Bank Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                //    return;
                //}

                Frm_print_options formPrintOption = new Frm_print_options();
                if (formPrintOption.ShowDialog() == DialogResult.OK)
                {
                    if (sale_type == "ICT")
                    {
                        Products.ICT.frm_destination_branch obj_ict = new Products.ICT.frm_destination_branch();
                        obj_ict.ShowDialog();
                        destination_branch_id = obj_ict._branch_id;

                    }

                    ///Checking customer credit limit
                    double customer_credit_limit = (txt_cust_credit_limit.Text == string.Empty ? 0 : Convert.ToDouble(txt_cust_credit_limit.Text));
                    double customerBalance = (txt_cust_balance.Text == string.Empty ? 0 : Convert.ToDouble(txt_cust_balance.Text));
                    double netAmount = (txt_total_amount.Text == string.Empty ? 0 : Convert.ToDouble(txt_total_amount.Text));
                    double netCreditLimit = customer_credit_limit - customerBalance;
                    double limitExceededBy = netAmount - netCreditLimit;
                    
                    if (sale_type == "Credit" && netAmount > netCreditLimit)
                    {
                        MessageBox.Show("Sales transaction cannot be saved, because customer credit limit has exceeded by " + limitExceededBy.ToString(), "Credit limit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ////

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

                        // SalesModal salesModal_obj = new SalesModal();
                        SalesBLL salesObj = new SalesBLL();

                        if (chkbox_is_taxable.Checked)
                        {
                            total_tax_var = Convert.ToDouble(txt_total_tax.Text);
                        }
                        else
                        {
                            total_tax_var = 0;
                        }

                        if (invoice_status == "Update" && txt_invoice_no.Text.Substring(0, 1).ToUpper() == "S") //Update sales delete all record first and insert new sales
                        {
                            MessageBox.Show("Update are not allowed", "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;

                            //int qresult = salesObj.DeleteSales(txt_invoice_no.Text); //DELETE ALL TRANSACTIONS
                            //if (qresult == 0)
                            //{
                            //    MessageBox.Show(invoice_no + "  has issue while updating, please try again", "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //    return;
                            //}
                            //invoice_no = txt_invoice_no.Text;
                        }
                        else
                        {
                            if (sale_type == "Quotation")
                            {
                                invoice_no = salesObj.GetMaxEstimateInvoiceNo();
                            }
                            else
                            {
                                invoice_no = salesObj.GetMaxSaleInvoiceNo();

                            }

                        }

                        if (txt_invoice_no.Text != "" && txt_invoice_no.Text.Substring(0, 1).ToUpper() == "E") //if estimates
                        {
                            estimate_invoice_no = txt_invoice_no.Text;
                            estimate_status = true;

                        }

                        //if purchase return then put minus sign before amount
                        double return_minus_value = (sale_type == "Return" ? -1 : 1);
                        double net_total = Math.Round(return_minus_value * total_amount, 6);
                        double net_total_discount = Math.Round(return_minus_value * total_discount, 6);
                        double net_total_tax = Math.Round(return_minus_value * total_tax, 6);

                        DateTime sale_date = txt_sale_date.Value.Date;
                        int customer_id = (cmb_customers.SelectedValue == null ? 0 : int.Parse(cmb_customers.SelectedValue.ToString()));
                        string customer_name = cmb_customers.Text;
                        string customer_vat = txt_customer_vat.Text;
                        int employee_id = (cmb_employees.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_employees.SelectedValue.ToString()));
                        int payment_terms_id = (cmb_payment_terms.SelectedValue == null ? 0 : int.Parse(cmb_payment_terms.SelectedValue.ToString()));
                        int payment_method_id = (cmb_payment_method.SelectedValue == null ? 0 : int.Parse(cmb_payment_method.SelectedValue.ToString()));
                        string invoice_subtype = (cmb_invoice_subtype_code.SelectedValue == null ? "02" : cmb_invoice_subtype_code.SelectedValue.ToString());
                        string PONumber = txtPONumber.Text;

                        //set the date from datetimepicker and set time to te current time
                        DateTime now = DateTime.Now;
                        txt_sale_date.Value = new DateTime(txt_sale_date.Value.Year, txt_sale_date.Value.Month, txt_sale_date.Value.Day, now.Hour, now.Minute, now.Second);
                        /////////////////////

                        /////Added sales header into the List
                        sales_model_header.Add(new SalesModalHeader
                        {
                            customer_id = customer_id,
                            customer_name = customer_name,
                            customer_vat = customer_vat,
                            employee_id = employee_id,
                            invoice_no = invoice_no,
                            total_amount = net_total,
                            total_tax = total_tax_var,
                            total_discount = net_total_discount,
                            total_discount_percent = Convert.ToDouble(txt_total_disc_percent.Value),
                            flat_discount_value = Convert.ToDouble(txtTotalFlatDiscountValue.Value),
                            sale_type = sale_type,
                            invoice_subtype = invoice_subtype,
                            sale_date = sale_date,
                            sale_time = txt_sale_date.Value,
                            description = txt_description.Text,
                            payment_terms_id = payment_terms_id,
                            payment_method_id = payment_method_id,
                            payment_method_text = paymentMethodText,
                            bankGLAccountID = bankGLAccountID,
                            bank_id = (string.IsNullOrEmpty(bankID) ? 0 : Convert.ToInt32(bankID)),
                            PONumber = PONumber,

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
                        //Int32 sale_id = (sale_type == "Quotation" ? salesObj.InsertEstimates(salesModal_obj) : salesObj.InsertSales(salesModal_obj));
                        int sno = 0;
                        for (int i = 0; i < grid_sales.Rows.Count; i++)
                        {
                            if (grid_sales.Rows[i].Cells["id"].Value != null)
                            {
                                if (chkbox_is_taxable.Checked)
                                {
                                    tax_rate = (grid_sales.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString()));
                                    tax_id = Convert.ToInt32(grid_sales.Rows[i].Cells["tax_id"].Value.ToString());
                                    //tax_rate = tax_rate;
                                }
                                else
                                {
                                    tax_id = 0;
                                    tax_rate = 0;
                                }

                                ///// Added sales detail in to List
                                sales_model_detail.Add(new SalesModal
                                {
                                    serialNo = sno++,
                                    invoice_no = invoice_no,
                                    item_id = Convert.ToInt32(grid_sales.Rows[i].Cells["id"].Value.ToString()),
                                    item_number = grid_sales.Rows[i].Cells["item_number"].Value.ToString(),
                                    code = grid_sales.Rows[i].Cells["code"].Value.ToString(),
                                    name = grid_sales.Rows[i].Cells["name"].Value.ToString(),
                                    quantity_sold = (string.IsNullOrEmpty(grid_sales.Rows[i].Cells["qty"].Value.ToString()) ? 0 : double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString())),
                                    unit_price = (string.IsNullOrEmpty(grid_sales.Rows[i].Cells["unit_price"].Value.ToString()) ? 0 : Math.Round(double.Parse(grid_sales.Rows[i].Cells["unit_price"].Value.ToString()), 4)),
                                    discount = (string.IsNullOrEmpty(grid_sales.Rows[i].Cells["discount"].Value.ToString()) ? 0 : Math.Round(double.Parse(grid_sales.Rows[i].Cells["discount"].Value.ToString()), 4)),
                                    discount_percent = double.Parse(grid_sales.Rows[i].Cells["discount_percent"].Value.ToString()),
                                    cost_price = (string.IsNullOrEmpty(grid_sales.Rows[i].Cells["cost_price"].Value.ToString()) ? 0 : Math.Round(Convert.ToDouble(grid_sales.Rows[i].Cells["cost_price"].Value.ToString()), 4)),// its avg cost actually ,
                                    item_type = grid_sales.Rows[i].Cells["item_type"].Value.ToString(),
                                    location_code = (grid_sales.Rows[i].Cells["location_code"].Value == null ? "" : grid_sales.Rows[i].Cells["location_code"].Value.ToString()),
                                    tax_id = tax_id,
                                    tax_rate = tax_rate,
                                    sale_date = sale_date,
                                    destination_branch_id = destination_branch_id,
                                    source_branch_id = source_branch_id,
                                    customer_id = customer_id,
                                });
                                //////////////

                            }
                        }

                        //if invoice type is sale then insert sales otherwise insert estimates/quotation
                        if (sale_type == "Quotation")
                        {
                            sale_id = salesObj.InsertEstimates(sales_model_header, sales_model_detail); // for quotation / estimates
                        }
                        else if (sale_type == "ICT")
                        {
                            if (destination_branch_id != 0)
                            {
                                int ict_result = salesObj.ict_qty_request(sales_model_header, sales_model_detail);
                                if (ict_result > 0)
                                {
                                    MessageBox.Show("Request for Inter Company Transfer (ICT) sent successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    clear_form();// CLEAR ALL FORM TEXTBOXES, GRID AND EVERYTING
                                    return; // return without printing only save
                                }
                            }

                        }
                        else
                        {
                            sale_id = salesObj.InsertSales(sales_model_header, sales_model_detail);// for sales items
                            if (sale_id > 0)
                            {
                                ZatcaHelper.SignInvoiceToZatca(invoice_no);
                                MessageBox.Show(invoice_no + " " + sale_type + " transaction " + (invoice_status == "Update" ? "updated" : "created") + " successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                        }

                        if (sale_type != "Quotation" && sale_type != "Gift" && sale_type != "ICT")//for sales 
                        {

                            //Employee commission entry
                            if (employee_commission_percent > 0)
                            {
                                var emp_commission_amount = employee_commission_percent * net_total / 100;
                                Insert_emp_commission(invoice_no, 0, 0, emp_commission_amount, sale_date, txt_description.Text, employee_id);
                            }
                            /////

                            //User commission entry
                            if (user_commission_percent > 0)
                            {
                                var user_commission_amount = user_commission_percent * net_total / 100;
                                Insert_user_commission(invoice_no, 0, 0, user_commission_amount, sale_date, txt_description.Text);
                            }
                            /////

                        }

                        if (sale_id > 0)
                        {
                            if (customer_id == 0 && customer_name.Length > 0)
                            {
                                //when new customer added then load again Dropdown list to fetch that record
                                get_customers_dropdownlist();
                            }
                            // PRINT INVOICE
                            string result1 = formPrintOption.PrintOptions;
                            bool isPrintInvoiceCode = false;

                            if (result1 == "0")
                            {
                                isPrintInvoiceCode = true;

                                clear_form();// CLEAR ALL FORM TEXTBOXES, GRID AND EVERYTING
                            }
                            else if (result1 == "1")
                            {
                                isPrintInvoiceCode = false;

                                clear_form();// CLEAR ALL FORM TEXTBOXES, GRID AND EVERYTING
                            }
                            else
                            {

                                clear_form();// CLEAR ALL FORM TEXTBOXES, GRID AND EVERYTING
                                return; // return without printing only save
                            }

                            if (sale_type == "Cash" || sale_type == "Credit")//for sales 
                            {
                                using (frm_sales_invoice obj = new frm_sales_invoice(load_sales_receipt(invoice_no), true, isPrintInvoiceCode))
                                {
                                    obj.load_print(); // send print direct to printer without showing dialog
                                    //obj.ShowDialog();
                                }
                            }
                            else//for estiamte
                            {
                                using (frm_sales_invoice obj = new frm_sales_invoice(load_estiamte_receipt(invoice_no), true, isPrintInvoiceCode))
                                {
                                    //obj.ShowDialog();
                                    obj.load_print(); // send print direct to printer without showing dialog
                                }
                            }

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
                //this will load the product movement 
                
                //string global_product_code = "";
                //if (grid_sales.Rows.Count > 0 && grid_sales.CurrentRow.Cells["code"].Value != null)
                //{
                //    global_product_code = grid_sales.CurrentRow.Cells["code"].Value.ToString();
                //    frm_productsMovements frm_prod_move_obj = new frm_productsMovements(global_product_code);
                //    frm_prod_move_obj.ShowDialog();
                //}

                //this will load the product page
                if (grid_sales.RowCount > 0 && grid_sales.CurrentRow.Cells["item_number"].Value != null)
                {
                    if (grid_sales.CurrentRow.Cells["item_number"].Value != null)
                    {
                        string item_number = grid_sales.CurrentRow.Cells["item_number"].Value.ToString();

                        frm_product_full_detail obj = new frm_product_full_detail(this, null, item_number,null,null,"",true);
                        
                        obj.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AmountFixToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                double subtotal = Convert.ToDouble(txt_sub_total.Text);
                double total_amount = Convert.ToDouble(txt_total_amount.Text);
                frm_round_prices obj = new frm_round_prices(this, subtotal, total_amount);
                obj.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ICTToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                Products.ICT.frm_ict frm_Ict = new Products.ICT.frm_ict();
                frm_Ict.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LosdQuotationToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                frm_search_estimates obj_estimate = new frm_search_estimates(this);
                obj_estimate.ShowDialog();
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
                frm_search_invoices frm = new frm_search_invoices(this);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grid_sales_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Validate only the numeric columns (3, 4, 5, 6, 7)
            if (e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6 || e.ColumnIndex == 7)
            {
                // Check if the value is null or empty
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    e.Cancel = true; // Prevent the cell from losing focus
                    grid_sales.Rows[e.RowIndex].ErrorText = "Value cannot be null or empty";
                }
                // Check if the value is a valid numeric value
                else if (!decimal.TryParse(e.FormattedValue.ToString(), out _))
                {
                    e.Cancel = true; // Prevent the cell from losing focus
                    grid_sales.Rows[e.RowIndex].ErrorText = "Value must be a numeric value";
                }
                else
                {
                    // Clear any previous error messages
                    grid_sales.Rows[e.RowIndex].ErrorText = string.Empty;
                }
            }
        }

        private void PrinttoolStripButton_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(txt_invoice_no.Text))
            {
                using (frm_sales_invoice obj = new frm_sales_invoice(load_sales_receipt(txt_invoice_no.Text), true))
                {
                    //obj.load_print(); // send print direct to printer without showing dialog
                    obj.ShowDialog();
                }
            }
            
        }

        private void SaleReturnToolStripButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_invoice_no.Text))
            {
                using (frm_sales_return obj = new frm_sales_return(txt_invoice_no.Text))
                {
                    obj.ShowDialog();
                    //obj.LoadSalesReturnGrid(); // send print direct to printer without showing dialog
                    obj.Close();
                }
            }
        }

        
        
    }

}
