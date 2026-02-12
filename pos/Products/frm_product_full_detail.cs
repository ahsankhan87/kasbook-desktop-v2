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
using POS.BLL;
using POS.Core;
using System.IO;
using System.Net;
using System.Web;
using pos.UI;

namespace pos
{
    public partial class frm_product_full_detail : Form
    {
        public string lang = (UsersModal.logged_in_lang.Length > 0 ? UsersModal.logged_in_lang : "en-US");
        ProductBLL objBLL = new ProductBLL();
        private readonly frm_sales salesForm;
        private readonly frm_purchases PurchaseForm;
        private readonly frm_searchSaleProducts searchsalesForm;
        private readonly frm_searchPurchaseProducts searchPurchaseForm;

        private DataGridView brandsDataGridView = new DataGridView();
        private DataGridView categoriesDataGridView = new DataGridView();
        private DataGridView groupsDataGridView = new DataGridView();

        public string _item_number = "";
        public string _keyword;
        public string _status;
        public string picture_name = "";
        private bool _loadMovementHistory;
            
        public frm_product_full_detail(frm_sales salesForm = null,frm_purchases PurchaseForm = null,string item_number = "",
            frm_searchSaleProducts searchsalesForm = null, frm_searchPurchaseProducts searchPurchaseForm = null, string keyword = "",
            bool loadMovementHistory = false)
        {
            this.salesForm = salesForm;
            this.PurchaseForm = PurchaseForm;

            _item_number = item_number;
            _keyword = keyword;
            this.searchsalesForm = searchsalesForm;
            _loadMovementHistory = loadMovementHistory;
            InitializeComponent();
        }

        public frm_product_full_detail()
        {
            InitializeComponent();
            
        }
        
        public void frm_product_full_detail_Load(object sender, EventArgs e)
        {
            txt_part_number.Focus();
            this.ActiveControl = txt_part_number;
            cmb_item_type.SelectedIndex = 0;
            cmb_tax.SelectedIndex = 1;
            
            get_taxes_dropdownlist();
            get_suppliers_dropdownlist();
            //get_brands_dropdownlist();
            //get_origin_dropdownlist();
            //get_product_group_dropdownlist();
            //get_locations_dropdownlist();
            get_units_dropdownlist();
            //get_categories_dropdownlist();

            if (_keyword != "")
            {
                txt_part_number.Text = _keyword;
                txt_part_number.Focus();
            }

            // Fix broken if statement in frm_product_full_detail_Load
            if (_item_number != "")
            {
                load_product_detail(_item_number);
            }
            //when this is true it will focus on history tab
            if (_loadMovementHistory)
            {
                Products_tab.SelectedTab = tabPage3; // Switch to history tab
                
            }
            ApplyGregorianCalendarForDatePickersIfArabic();

            // Restrict numeric fields
            if (txt_cost_price != null) txt_cost_price.KeyPress += NumericTextBox_KeyPress;
            if (txt_unit_price != null) txt_unit_price.KeyPress += NumericTextBox_KeyPress;
            if (txt_unit_price_2 != null) txt_unit_price_2.KeyPress += NumericTextBox_KeyPress;
            if (txt_packet_qty != null) txt_packet_qty.KeyPress += NumericTextBox_KeyPress;
            if (txt_demand_qty != null) txt_demand_qty.KeyPress += NumericTextBox_KeyPress;
            if (txt_pur_dmnd_qty != null) txt_pur_dmnd_qty.KeyPress += NumericTextBox_KeyPress;
            if (txt_sale_dmnd_qty != null) txt_sale_dmnd_qty.KeyPress += NumericTextBox_KeyPress;
            if (txt_restock_level != null) txt_restock_level.KeyPress += NumericTextBox_KeyPress;
        }

        public void load_product_detail(string item_number)
        {
            DataTable dt = objBLL.GetAllByProductByItemNumber(item_number);
            foreach (DataRow myProductView in dt.Rows)
            {
                txt_id.Text = myProductView["id"].ToString();
                txt_name.Text = myProductView["name"].ToString();
                txt_name_ar.Text = myProductView["name_ar"].ToString();
                txt_barcode.Text = myProductView["barcode"].ToString();
                txt_code.Text = myProductView["code"].ToString();
                txt_part_number.Text = myProductView["part_number"].ToString();
                txtItemNumber.Text = myProductView["item_number"].ToString();
                //txt_item_number.ReadOnly = true;
                txt_alt_item_number.Text = myProductView["item_number_2"].ToString();
                cmb_item_type.Text = myProductView["item_type"].ToString();
                txt_cost_price.Text = Math.Round(Convert.ToDecimal(myProductView["avg_cost"]), 4).ToString();
                txt_unit_price.Text = Math.Round(Convert.ToDecimal( myProductView["unit_price"]), 4).ToString();
                txt_unit_price_2.Text = Math.Round(Convert.ToDecimal(myProductView["unit_price_2"]), 4).ToString();
                txt_description.Text = myProductView["description"].ToString();
                //txt_expiry_date.Text = myProductView["expiry_date"].ToString();

                //txt_brand_code.Text = myProductView["brand_code"].ToString();
                //txt_category_code.Text = myProductView["category_code"].ToString();
                //txt_group_code.Text = myProductView["group_code"].ToString();
                txt_packet_qty.Text = myProductView["packet_qty"].ToString();
                txt_def_location.Text = myProductView["location_code"].ToString();
                
                ///
                fetch_brands_by_code(myProductView["brand_code"].ToString());
                fetch_categories_by_code(myProductView["category_code"].ToString());
                fetch_groups_by_code(myProductView["group_code"].ToString());
                ///

                cmb_units.SelectedValue = (String.IsNullOrEmpty(myProductView["unit_id"].ToString()) ? 0 : myProductView["unit_id"]); 
                //cmb_locations.SelectedValue = myProductView["location_code"].ToString();
                cmb_tax.SelectedValue = (String.IsNullOrEmpty(myProductView["tax_id"].ToString()) ? 0 : myProductView["tax_id"]); 
                cmb_supplier.SelectedValue = (String.IsNullOrEmpty(myProductView["supplier_id"].ToString()) ? 0 : myProductView["supplier_id"]); 
                //cmb_origin.SelectedValue = myProductView["origin"].ToString();

                txt_demand_qty.Text = myProductView["demand_qty"].ToString();
                txt_pur_dmnd_qty.Text =myProductView["purchase_demand_qty"].ToString();
                txt_sale_dmnd_qty.Text = myProductView["sale_demand_qty"].ToString();
                txt_restock_level.Text = myProductView["reorder_level"].ToString();

                if (myProductView["picture"].ToString() != "")
                {
                    byte[] myImage = new byte[0];
                    myImage = (byte[])myProductView["Picture"];
                    MemoryStream stream = new MemoryStream(myImage);
                    if (stream.Length > 0)
                    {
                        pictureBox1.Image = Image.FromStream(stream);

                    }
                    else
                    {
                        pictureBox1.Image = null;
                    }

                }

                // set expiry date safely if column exists
                if (dt.Columns.Contains("expiry_date") && myProductView["expiry_date"] != DBNull.Value)
                {
                    try
                    {
                        txt_expiry_date.Value = ClampToSafePickerDate(Convert.ToDateTime(myProductView["expiry_date"]));
                    }
                    catch
                    {
                        txt_expiry_date.Value = DateTime.Today;
                    }
                }

                Load_product_movements_with_balance_qty(item_number);
                //load_product_location_qty(item_number);
            }
            lbl_product_name.Visible = true;
            lbl_product_name.Text = txt_code.Text+' '+txt_name.Text;
            btn_other_stock.Enabled = true;
        }
        
        private void fetch_brands_by_code(string brand_code)
        {
            GeneralBLL brandBLL = new GeneralBLL();
            String keyword = "*";
            String table = "pos_brands WHERE code = '" + brand_code + "'";
            DataTable dt_1 = brandBLL.GetRecord(keyword, table);
            foreach (DataRow dr in dt_1.Rows)
            {
                txt_brand_code.Text = dr["code"].ToString();
                txt_brands.Text = dr["name"].ToString();
                    
            }
        }

        private void fetch_categories_by_code(string category_code)
        {
            GeneralBLL BLL = new GeneralBLL();
            String keyword = "*";
            String table = "pos_categories WHERE code = '" + category_code + "'";
            DataTable dt_1 = BLL.GetRecord(keyword, table);
            if(dt_1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_1.Rows)
                {
                    txt_category_code.Text = dr["code"].ToString();
                    txt_categories.Text = dr["name"].ToString();

                }
            }
            else
            {
                txt_category_code.Text = "";
                txt_categories.Text = "";
            }
        }

        private void fetch_groups_by_code(string group_code)
        {
            GeneralBLL BLL = new GeneralBLL();
            String keyword = "*";
            String table = "pos_product_groups WHERE code = '" + group_code + "'";
            DataTable dt_1 = BLL.GetRecord(keyword, table);
            if(dt_1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_1.Rows)
                {
                    txt_group_code.Text = dr["code"].ToString();
                    txt_groups.Text = dr["name"].ToString();

                }
            }
            else
            {
                txt_group_code.Text = "";
                txt_groups.Text = "";
            }
            
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (objBLL.IsProductExist(txt_code.Text.Trim(), txt_category_code.Text.Trim()))
                {
                    UiMessages.ShowWarning(
                        "A product with the same code already exists.",
                        "يوجد منتج بنفس الكود بالفعل.",
                        "Duplicate",
                        "مكرر"
                    );
                    return;
                }
                if (objBLL.CheckDuplicateBarcode(txt_barcode.Text) && !string.IsNullOrEmpty(txt_barcode.Text))
                {
                    UiMessages.ShowWarning(
                        "A product with the same barcode already exists.",
                        "يوجد منتج بنفس الباركود بالفعل.",
                        "Duplicate",
                        "مكرر"
                    );
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_code.Text) || string.IsNullOrWhiteSpace(txt_name.Text) || string.IsNullOrWhiteSpace(txt_part_number.Text))
                {
                    UiMessages.ShowInfo(
                        "Code, name, and part number are required.",
                        "الكود والاسم ورقم القطعة حقول مطلوبة.",
                        "Validation",
                        "التحقق"
                    );
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    "Save this product?",
                    "هل تريد حفظ هذا المنتج؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );
                if (confirm != DialogResult.Yes) return;

                FileStream fs;
                BinaryReader br;

                ProductModal info = new ProductModal();

                if (picture_name != "")
                {
                    byte[] ImageData;
                    fs = new FileStream(picture_name, FileMode.Open, FileAccess.Read);
                    br = new BinaryReader(fs);
                    ImageData = br.ReadBytes((int)fs.Length);
                    br.Close();
                    fs.Close();

                    info.picture = ImageData;
                }

                //Unique item number / id 
                string maxItemNumber = objBLL.GetMaxProductNumber();

                info.barcode = txt_barcode.Text;
                info.code = txt_code.Text;
                info.part_number = txt_part_number.Text;
                info.item_number = maxItemNumber;
                info.alt_item_number = txt_alt_item_number.Text;
                info.group_code = txt_group_code.Text;
                info.category_code = txt_category_code.Text;
                info.brand_code = txt_brand_code.Text;

                info.name = txt_name.Text;
                info.name_ar = txt_name_ar.Text;
                info.cost_price = (String.IsNullOrEmpty(txt_cost_price.Text)) ? 0 : double.Parse(txt_cost_price.Text);
                info.unit_price = (String.IsNullOrEmpty(txt_unit_price.Text)) ? 0 : double.Parse(txt_unit_price.Text);
                info.unit_price_2 = (String.IsNullOrEmpty(txt_unit_price_2.Text)) ? 0 : double.Parse(txt_unit_price_2.Text);
                info.item_type = cmb_item_type.Text;
                info.description = txt_description.Text;
                info.tax_id = (cmb_tax.SelectedValue == null ? 0 : Convert.ToInt32(cmb_tax.SelectedValue.ToString()));
                info.supplier_id = (cmb_supplier.SelectedValue == null ? 0 : Convert.ToInt32(cmb_supplier.SelectedValue.ToString()));
                info.unit_id = (cmb_units.SelectedValue == null ? 0 : Convert.ToInt32(cmb_units.SelectedValue.ToString()));
                info.location_code = txt_def_location.Text;
                info.demand_qty = (txt_demand_qty.Text != "" ? decimal.Parse(txt_demand_qty.Text) : 0);
                info.purchase_demand_qty = (txt_pur_dmnd_qty.Text != "" ? decimal.Parse(txt_pur_dmnd_qty.Text) : 0);
                info.sale_demand_qty = (txt_sale_dmnd_qty.Text != "" ? decimal.Parse(txt_sale_dmnd_qty.Text) : 0);
                info.re_stock_level = (txt_restock_level.Text != "" ? decimal.Parse(txt_restock_level.Text) : 0);
                info.expiry_date = ClampToSafePickerDate(txt_expiry_date.Value.Date);
                info.packet_qty = (String.IsNullOrEmpty(txt_packet_qty.Text)) ? 0 : decimal.Parse(txt_packet_qty.Text);

                int result = objBLL.Insert(info);
                if (result > 0)
                {
                    UiMessages.ShowInfo(
                        "Product has been created successfully.",
                        "تم إنشاء المنتج بنجاح.",
                        "Success",
                        "نجاح"
                    );
                    clear_all();
                    txt_part_number.Focus();
                }
                else
                {
                    UiMessages.ShowError(
                        "Product could not be saved. Please try again.",
                        "تعذر حفظ المنتج. يرجى المحاولة مرة أخرى.",
                        "Error",
                        "خطأ"
                    );
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }
       
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                var confirm = UiMessages.ConfirmYesNo(
                    "Save this product?",
                    "هل تريد حفظ هذا المنتج؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm == DialogResult.Yes)
                    btn_save.PerformClick();

                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void get_taxes_dropdownlist()
        {
            TaxBLL taxBLL = new TaxBLL();
            
            DataTable taxes = taxBLL.GetAll(); 
            DataRow emptyRow = taxes.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "Please Select";              // Set Column Value
            //taxes.Rows.InsertAt(emptyRow, 0);

            cmb_tax.DisplayMember = "title";
            cmb_tax.ValueMember = "id";
            cmb_tax.DataSource = taxes;

        }


        public void get_suppliers_dropdownlist()
        {
            SupplierBLL supplierBLL = new SupplierBLL();
            
            DataTable suppliers = supplierBLL.GetAll(); // generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = suppliers.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[2] = "Please Select";              // Set Column Value
            suppliers.Rows.InsertAt(emptyRow, 0);

            cmb_supplier.DisplayMember = "first_name";
            cmb_supplier.ValueMember = "id";
            cmb_supplier.DataSource = suppliers;

        }

        public void get_units_dropdownlist()
        {
            UnitsBLL unitsBLL = new UnitsBLL();

            DataTable units = unitsBLL.GetAll();
            DataRow emptyRow = units.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[3] = "Please Select";              // Set Column Value
            units.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = units.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[3] = "ADD NEW";              // Set Column Value
            units.Rows.InsertAt(emptyRow1, 1);

            cmb_units.DisplayMember = "name";
            cmb_units.ValueMember = "id";
            cmb_units.DataSource = units;

        }

        public DataTable get_GL_accounts_dt()
        {
            AccountsBLL accountsBLL = new AccountsBLL();
            
            DataTable dt = accountsBLL.GetAll();
            DataRow emptyRow = dt.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Select Account";              // Set Column Value
            dt.Rows.InsertAt(emptyRow, 0);
            return dt;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //this.Dispose(); 
            this.Close();
        }

        private void frm_product_full_detail_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
            if(e.KeyData == Keys.Down)
            {
                brandsDataGridView.Focus();
                categoriesDataGridView.Focus();
                groupsDataGridView.Focus();
            }
            if (e.KeyData == Keys.F3)
            {
                btn_save.PerformClick();
            }
            if (e.KeyData == Keys.F4)
            {
                btn_update.PerformClick();
            }
            if (e.KeyData == Keys.F5)
            {
                btn_refresh.PerformClick();
            }
            if (e.KeyData == Keys.F6)
            {
                btn_delete.PerformClick();
            }
            if (e.KeyData == Keys.F9)
            {
                txt_product_code.Focus();
            }
        }

        private void txt_cost_price_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txt_unit_price_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cmb_units_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_units.SelectedValue != null && cmb_units.SelectedValue.ToString() == "-1")
            {
                frm_addUnit frm = new frm_addUnit(null);
                frm.ShowDialog();

                get_units_dropdownlist();
            }
        }

        private void txt_code_KeyDown(object sender, KeyEventArgs e)
        {
            if (objBLL.IsProductExist(txt_code.Text, txt_category_code.Text) && e.KeyData == Keys.Enter)
            {
                UiMessages.ShowWarning(
                    "This item code is already in use.",
                    "كود الصنف مستخدم بالفعل.",
                    "Duplicate",
                    "مكرر"
                );
                btn_save.Enabled = false;
                lbl_errors.Visible = true;
                lbl_errors.Text = "*Item code already exist";
            }
            else
            {
                btn_save.Enabled = true;
                lbl_errors.Text = "";
                lbl_errors.Visible = false;

            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {

                if (String.IsNullOrEmpty(txt_id.Text))
                {
                    UiMessages.ShowInfo(
                        "Please select a product record to update.",
                        "يرجى اختيار سجل منتج للتحديث.",
                        "Not Found",
                        "غير موجود"
                    );
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_code.Text) || string.IsNullOrWhiteSpace(txt_name.Text) || string.IsNullOrWhiteSpace(txt_part_number.Text))
                {
                    UiMessages.ShowInfo(
                        "Code, name, and part number are required.",
                        "الكود والاسم ورقم القطعة حقول مطلوبة.",
                        "Validation",
                        "التحقق"
                    );
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    "Update this product?",
                    "هل تريد تحديث هذا المنتج؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );
                if (confirm != DialogResult.Yes) return;

                FileStream fs;
                BinaryReader br;

                ProductModal info = new ProductModal();

                if (picture_name != "")
                {
                    byte[] ImageData;
                    fs = new FileStream(picture_name, FileMode.Open, FileAccess.Read);
                    br = new BinaryReader(fs);
                    ImageData = br.ReadBytes((int)fs.Length);
                    br.Close();
                    fs.Close();

                    info.picture = ImageData;
                }

                info.barcode = txt_barcode.Text;
                info.code = txt_code.Text;
                info.part_number = txt_part_number.Text;
                info.alt_item_number = txt_alt_item_number.Text;
                info.group_code = txt_group_code.Text;
                info.category_code = txt_category_code.Text;
                info.brand_code = txt_brand_code.Text;

                info.name = txt_name.Text;
                info.name_ar = txt_name_ar.Text;
                info.cost_price = (String.IsNullOrEmpty(txt_cost_price.Text)) ? 0 : double.Parse(txt_cost_price.Text);
                info.unit_price = (String.IsNullOrEmpty(txt_unit_price.Text)) ? 0 : double.Parse(txt_unit_price.Text);
                info.unit_price_2 = (String.IsNullOrEmpty(txt_unit_price_2.Text)) ? 0 : double.Parse(txt_unit_price_2.Text);
                info.item_type = cmb_item_type.Text;
                info.description = txt_description.Text;
                info.tax_id = (cmb_tax.SelectedValue == null ? 0 : Convert.ToInt32(cmb_tax.SelectedValue.ToString()));
                info.unit_id = Convert.ToInt32(cmb_units.SelectedValue.ToString());
                info.supplier_id = (cmb_supplier.SelectedValue == null ? 0 : Convert.ToInt32(cmb_supplier.SelectedValue.ToString()));
                info.location_code = txt_def_location.Text;
                info.demand_qty = (txt_demand_qty.Text != "" ? decimal.Parse(txt_demand_qty.Text) : 0);
                info.purchase_demand_qty = (txt_pur_dmnd_qty.Text != "" ? decimal.Parse(txt_pur_dmnd_qty.Text) : 0);
                info.sale_demand_qty = (txt_sale_dmnd_qty.Text != "" ? decimal.Parse(txt_sale_dmnd_qty.Text) : 0);
                info.re_stock_level = (txt_restock_level.Text != "" ? decimal.Parse(txt_restock_level.Text) : 0);
                info.expiry_date = ClampToSafePickerDate(txt_expiry_date.Value.Date);
                info.packet_qty = (String.IsNullOrEmpty(txt_packet_qty.Text)) ? 0 : decimal.Parse(txt_packet_qty.Text);


                info.id = int.Parse(txt_id.Text);
                int result = objBLL.Update(info);

                if (result > 0)
                {
                    UiMessages.ShowInfo(
                        "Product has been updated successfully.",
                        "تم تحديث المنتج بنجاح.",
                        "Success",
                        "نجاح"
                    );
                    clear_all();
                    txt_part_number.Focus();
                }
                else
                {
                    UiMessages.ShowError(
                        "Product could not be updated. Please try again.",
                        "تعذر تحديث المنتج. يرجى المحاولة مرة أخرى.",
                        "Error",
                        "خطأ"
                    );
                }
            }
            catch (Exception ex)
            {

                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_blank_Click(object sender, EventArgs e)
        {
            clear_all();
            txt_part_number.Focus();
        }
        private void clear_all()
        {
            txt_barcode.Text = "";
            txt_code.Text = "";
            txt_part_number.Text = "";
            txt_part_number.ReadOnly = false;
            txt_alt_item_number.Text = "";
            txt_packet_qty.Text = "";

            lbl_product_name.Text = "";
            grid_movements.Rows.Clear();
            //grid_location_qty.Rows.Clear();

            txt_name.Text = ""; ;
            txt_name_ar.Text = ""; ;
            txt_cost_price.Text = "";
            txt_unit_price.Text = "";
            txt_unit_price_2.Text = "";
            cmb_item_type.Text = "";
            txt_description.Text = "";
            cmb_units.SelectedIndex = 0;
            cmb_supplier.SelectedIndex = -1;
            //cmb_locations.SelectedValue = "";
            txt_def_location.Text = "";
            
            txt_brand_code.Text= "";
            txt_brands.Text = "";
            txt_category_code.Text = "";
            txt_categories.Text = "";
            txt_groups.Text = "";
            txt_group_code.Text = "";
            
            txt_demand_qty.Text = "";
            txt_pur_dmnd_qty.Text = "";
            txt_sale_dmnd_qty.Text = "";
            txt_restock_level.Text = "";
            txt_id.Text = "";
            cmb_tax.SelectedIndex = 0;
            pictureBox1.Image = null;

            
        }

        private void load_product_movements(string item_number)
        {
            try
            {
                grid_movements.Rows.Clear();

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_movements.AutoGenerateColumns = false;

                String keyword = "TOP 1000 I.id,I.item_code,I.item_number,I.qty,I.unit_price,I.cost_price,I.loc_code,I.invoice_no,I.description,trans_date,C.first_name AS customer, S.first_name AS supplier";
                String table = "pos_inventory I LEFT JOIN pos_customers C ON C.id=I.customer_id LEFT JOIN pos_suppliers S ON S.id=I.supplier_id"+
                " WHERE I.item_number = '" + item_number + "' AND I.branch_id = " + UsersModal.logged_in_branch_id + " ORDER BY I.id DESC";
                //grid_movements.DataSource = objBLL.GetRecord(keyword, table);

                DataTable product_dt = objBLL.GetRecord(keyword, table);
                if (product_dt.Rows.Count > 0)
                {
                    int RowIndex = 0;
                    foreach (DataRow myProductView in product_dt.Rows)
                    {
                        int id = Convert.ToInt32(myProductView["id"]);
                        string invoice_no = myProductView["invoice_no"].ToString();
                        //string name = myProductView["product_name"].ToString();
                        string qty = myProductView["qty"].ToString();
                        double cost_price = Convert.ToDouble(myProductView["cost_price"]);
                        double unit_price = Convert.ToDouble(myProductView["unit_price"]);
                        string loc_code = myProductView["loc_code"].ToString();
                        string description = myProductView["description"].ToString();
                        string supplier = myProductView["supplier"].ToString();
                        string customer = myProductView["customer"].ToString();
                        string date = myProductView["trans_date"].ToString();

                        string[] row0 = { id.ToString(), invoice_no, qty,cost_price.ToString(), unit_price.ToString(),
                                          loc_code,description, supplier, customer,date};

                        grid_movements.Rows.Add(row0);

                        if (description == "Sale")
                        {
                            grid_movements.Rows[RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                        }
                        if (description == "Purchase")
                        {
                            grid_movements.Rows[RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;

                        }
                        if (description == "Adjustment")
                        {
                            grid_movements.Rows[RowIndex].DefaultCellStyle.BackColor = Color.Yellow;

                        }


                        RowIndex++;
                    }
                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
                throw;
            }
        }

        private void Load_product_movements_with_balance_qty(string item_number)
        {
            try
            {
                grid_movements.Rows.Clear();

                GeneralBLL objBLL = new GeneralBLL();
                grid_movements.AutoGenerateColumns = false;

                string keyword = "I.id,P.name AS product_name,I.item_code,I.item_number,I.qty,I.loc_code,I.unit_price," +
                    "I.cost_price,I.invoice_no,I.description,trans_date,C.first_name AS customer," +
                    "CONCAT(S.first_name,' ',S.last_name) AS supplier";
                string table = "pos_inventory I " +
                               "LEFT JOIN pos_products P ON P.item_number = I.item_number " +
                               "LEFT JOIN pos_customers C ON C.id = I.customer_id " +
                               "LEFT JOIN pos_suppliers S ON S.id = I.supplier_id " +
                               "WHERE I.item_number = '" + item_number + "' AND I.branch_id = " + UsersModal.logged_in_branch_id + " " +
                               "ORDER BY I.id ASC";

                DataTable product_dt = objBLL.GetRecord(keyword, table);

                if (product_dt.Rows.Count > 0)
                {
                    // ✅ Add balance_qty column manually to avoid error
                    if (!product_dt.Columns.Contains("balance_qty"))
                        product_dt.Columns.Add("balance_qty", typeof(double));

                    // Calculate running balance
                    double balance_qty = 0;
                    foreach (DataRow row in product_dt.Rows)
                    {
                        balance_qty += Convert.ToDouble(row["qty"]);
                        row["balance_qty"] = balance_qty;
                    }

                    // Display in DESC order
                    int RowIndex = 0;
                    foreach (DataRow row in product_dt.Select("", "id DESC"))
                    {
                        int id = Convert.ToInt32(row["id"]);
                        string invoice_no = row["invoice_no"].ToString();
                        //string name = row["product_name"].ToString();
                        string qty = row["qty"].ToString();
                        string balance = row["balance_qty"].ToString();
                        double cost_price = Convert.ToDouble(row["cost_price"]);
                        double unit_price = Convert.ToDouble(row["unit_price"]);
                        string location = row["loc_code"].ToString();
                        string description = row["description"].ToString();
                        string supplier = row["supplier"].ToString();
                        string customer = row["customer"].ToString();
                        string date = row["trans_date"].ToString();

                        string[] row0 = { id.ToString(), invoice_no, qty, balance, cost_price.ToString(), unit_price.ToString(),
                                  location,description, supplier, customer, date };

                        grid_movements.Rows.Add(row0);

                        if (description == "Sale")
                            grid_movements.Rows[RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                        else if (description == "Purchase")
                            grid_movements.Rows[RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                        else if (description == "Adjustment")
                            grid_movements.Rows[RowIndex].DefaultCellStyle.BackColor = Color.Yellow;

                        RowIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
                throw;
            }
        }

        private void load_product_location_qty(string product_code)
        {
            //try
            //{
            //    grid_location_qty.Rows.Clear();

            //    //bind data in data grid view  
            //    GeneralBLL objBLL = new GeneralBLL();
            //    grid_location_qty.AutoGenerateColumns = false;

            //    //String keyword = "L.name AS location_name, PS.loc_code AS location_code, SUM(PS.qty) AS location_qty";
            //    //String table = "pos_product_stocks PS LEFT JOIN pos_locations L ON PS.loc_code=L.code WHERE PS.item_id = " + product_id + " GROUP BY L.name,PS.loc_code";
            //    //grid_movements.DataSource = objBLL.GetRecord(keyword, table);

            //    String keyword = "loc_code AS location_name, loc_code AS location_code, qty AS location_qty";
            //    String table = "pos_products_location_view WHERE id = " + product_id + "";
                
            //    DataTable product_dt = objBLL.GetRecord(keyword, table);
            //    if (product_dt.Rows.Count > 0)
            //    {
                   
            //        foreach (DataRow myProductView in product_dt.Rows)
            //        {
            //            string location_name = myProductView["location_name"].ToString();
            //            string location_code = myProductView["location_code"].ToString();
            //            string location_qty = myProductView["location_qty"].ToString();

            //            string[] row0 = { location_code,location_name, location_qty};

            //            grid_location_qty.Rows.Add(row0);

            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    throw;
            //}
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = txt_id.Text;

            if (!string.IsNullOrWhiteSpace(id))
            {
                var confirm = UiMessages.ConfirmYesNo(
                    "Delete this product? This action cannot be undone.",
                    "هل تريد حذف هذا المنتج؟ لا يمكن التراجع عن هذا الإجراء.",
                    captionEn: "Confirm Delete",
                    captionAr: "تأكيد الحذف"
                );

                if (confirm == DialogResult.Yes)
                {
                    ProductBLL objBLL = new ProductBLL();
                    objBLL.Delete(int.Parse(id));

                    UiMessages.ShowInfo(
                        "Product has been deleted successfully.",
                        "تم حذف المنتج بنجاح.",
                        "Deleted",
                        "تم الحذف"
                    );
                    clear_all();
                    txt_part_number.Focus();
                }
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a product record first.",
                    "يرجى اختيار سجل منتج أولاً.",
                    "Product",
                    "المنتج"
                );
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            string product_code = txt_id.Text;
            string product_number = txtItemNumber.Text;

            if (!string.IsNullOrEmpty(product_number))
            {
                load_product_detail(product_number);
                //Load_product_movements_with_balance_qty(product_number);
                load_product_location_qty(product_number);
            }
            txt_part_number.Focus();
        }

        private void SetupBrandDataGridView()
        {
            var current_lang_code = System.Globalization.CultureInfo.CurrentCulture;
            brandsDataGridView.ColumnCount = 2;
            int xLocation = tabPage1.Location.X + txt_brands.Location.X;
            int yLocation = tabPage1.Location.Y + txt_brands.Location.Y;

            brandsDataGridView.Name = "brandsDataGridView";
            if (lang == "en-US")
            {
                brandsDataGridView.Location = new Point(xLocation, yLocation);
                brandsDataGridView.Size = new Size(300, 250);
            }
            else if (lang == "ar-SA")
            {
                brandsDataGridView.Location = new Point(xLocation, yLocation);
                brandsDataGridView.Size = new Size(200, 250);
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

            this.tabPage1.Controls.Add(brandsDataGridView);
            brandsDataGridView.BringToFront();
            
            

        }

        void brandsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
                txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.tabPage1.Controls.Remove(brandsDataGridView);
                txt_brands.Focus();

                e.Handled = true;
            }
        }

        private void brandsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.tabPage1.Controls.Remove(brandsDataGridView);
            txt_brands.Focus();
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
                    this.tabPage1.Controls.Remove(brandsDataGridView);
                }


            }
            catch (Exception ex)
            {

                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void txt_brands_Leave(object sender, EventArgs e)
        {
           if(!brandsDataGridView.Focused)
           {
               this.tabPage1.Controls.Remove(brandsDataGridView);
           }
            
        }

        private void txt_brands_TextChanged(object sender, EventArgs e)
        {
            generate_item_code();

            if (txt_brand_code.Text != "")
            {
                BrandsBLL brandsBLL = new BrandsBLL();
                DataTable dt = brandsBLL.SearchRecordByBrandsCode(txt_brand_code.Text);
                foreach (DataRow dr in dt.Rows)
                {
                    fetch_categories_by_code(dr["category_code"].ToString());
                    fetch_groups_by_code(dr["group_code"].ToString());
                }
            }
            else
            {
                txt_category_code.Text = "";
                txt_group_code.Text = "";
            }
        }

        private void SetupCategoriesDataGridView()
        {
            var current_lang_code = System.Globalization.CultureInfo.CurrentCulture;
            categoriesDataGridView.ColumnCount = 2;
            categoriesDataGridView.Name = "categoriesDataGridView";
            int xLocation = tabPage1.Location.X + txt_categories.Location.X;
            int yLocation = tabPage1.Location.Y + txt_categories.Location.Y;

            if (lang == "en-US")
            {
                categoriesDataGridView.Location = new Point(xLocation, yLocation);
                categoriesDataGridView.Size = new Size(300, 250);
            }
            else if (lang == "ar-SA")
            {
                categoriesDataGridView.Location = new Point(xLocation, yLocation);
                categoriesDataGridView.Size = new Size(200, 250);
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
            //categoriesDataGridView.ColumnHeadersVisible = false;
            categoriesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            categoriesDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            categoriesDataGridView.AutoResizeColumns();

            this.categoriesDataGridView.CellClick += new DataGridViewCellEventHandler(categoriesDataGridView_CellClick);
            this.categoriesDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(categoriesDataGridView_KeyDown);

            this.tabPage1.Controls.Add(categoriesDataGridView);
            categoriesDataGridView.BringToFront();

        }

        void categoriesDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_category_code.Text = categoriesDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_categories.Text = categoriesDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.tabPage1.Controls.Remove(categoriesDataGridView);
                txt_categories.Focus();
                e.Handled = true;
            }
        }

        private void categoriesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_category_code.Text = categoriesDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_categories.Text = categoriesDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.tabPage1.Controls.Remove(categoriesDataGridView);
            txt_categories.Focus();

        }

        private void txt_categories_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_categories.Text != "")
                {
                    SetupCategoriesDataGridView();

                    CategoriesBLL categoryBLL_obj = new CategoriesBLL();
                    string category_name = txt_categories.Text;

                    DataTable dt = categoryBLL_obj.SearchRecord(category_name);

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
                    this.tabPage1.Controls.Remove(categoriesDataGridView);
                }


            }
            catch (Exception ex)
            {

                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void txt_categories_Leave(object sender, EventArgs e)
        {
            if (!categoriesDataGridView.Focused)
            {
                this.tabPage1.Controls.Remove(categoriesDataGridView);
            }

        }

        private void SetupGroupsDataGridView()
        {
            var current_lang_code = System.Globalization.CultureInfo.CurrentCulture;
            groupsDataGridView.ColumnCount = 2;
            int xLocation = tabPage1.Location.X + txt_groups.Location.X;
            int yLocation = tabPage1.Location.Y + txt_groups.Location.Y;
            
            groupsDataGridView.Name = "groupsDataGridView";
            if (lang == "en-US")
            {
                groupsDataGridView.Location = new Point(xLocation, yLocation);
                groupsDataGridView.Size = new Size(300, 250);
            }
            else if (lang == "ar-SA")
            {
                groupsDataGridView.Location = new Point(xLocation, yLocation);
                groupsDataGridView.Size = new Size(200, 250);
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
            //groupsDataGridView.ColumnHeadersVisible = false;
            groupsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            groupsDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            groupsDataGridView.AutoResizeColumns();

            this.groupsDataGridView.CellClick += new DataGridViewCellEventHandler(groupsDataGridView_CellClick);
            this.groupsDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(groupsDataGridView_KeyDown);

            this.tabPage1.Controls.Add(groupsDataGridView);
            groupsDataGridView.BringToFront();

        }

        void groupsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_group_code.Text = groupsDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_groups.Text = groupsDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.tabPage1.Controls.Remove(groupsDataGridView);
                txt_groups.Focus();
                e.Handled = true;
            }
        }

        private void groupsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_group_code.Text = groupsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_groups.Text = groupsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.tabPage1.Controls.Remove(groupsDataGridView);
            txt_groups.Focus();

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
                    this.tabPage1.Controls.Remove(groupsDataGridView);
                }


            }
            catch (Exception ex)
            {

                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void txt_groups_Leave(object sender, EventArgs e)
        {
            if (!groupsDataGridView.Focused)
            {
                this.tabPage1.Controls.Remove(groupsDataGridView);
            }

        }

        private void btn_other_stock_Click(object sender, EventArgs e)
        {
            string product_id="";
            string product_code="";
            string item_number="";
            string product_name = "";
            if(!string.IsNullOrEmpty(txt_id.Text))
            {
                product_id = txt_id.Text;
                item_number = txtItemNumber.Text;
                product_name = txt_name.Text;
            }
            frm_other_stocks frm_other_stock_obj = new frm_other_stocks(product_id, item_number, product_name);
            frm_other_stock_obj.ShowDialog();
        }

        private void btn_translate_Click(object sender, EventArgs e)
        {
            txt_name_ar.Text = Translate(txt_name.Text);
        }
        public String Translate(String word)
        {
            var result = "";
            try
            {
                var toLanguage = "ar";//English
                var fromLanguage = "en";//Deutsch
                var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(word)}";
                var webClient = new WebClient
                {
                    Encoding = System.Text.Encoding.UTF8
                };
                result = webClient.DownloadString(url);
                
                    result = result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
                
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
            return result;
        }

        private void txt_part_number_KeyUp(object sender, KeyEventArgs e)
        {
            generate_item_code();
        }

        private void ApplyGregorianCalendarForDatePickersIfArabic()
        {
            // When running in ar-SA, Windows may use UmAlQuraCalendar (Hijri) for DateTimePicker.
            // That calendar has a limited supported range and can throw when Value is outside it.
            // We keep Arabic UI but force Gregorian display for this picker.
            if (string.Equals(lang, "ar-SA", StringComparison.OrdinalIgnoreCase))
            {
                txt_expiry_date.Format = DateTimePickerFormat.Custom;
                txt_expiry_date.CustomFormat = "yyyy-MM-dd";
            }
            else
            {
                // restore default short format for other cultures
                txt_expiry_date.Format = DateTimePickerFormat.Short;
                txt_expiry_date.CustomFormat = null;
            }
        }

        private static DateTime ClampToSafePickerDate(DateTime dt)
        {
            // Keep within DateTimePicker supported range
            if (dt < DateTimePicker.MinimumDateTime) return DateTimePicker.MinimumDateTime;
            if (dt > DateTimePicker.MaximumDateTime) return DateTimePicker.MaximumDateTime;
            return dt;
        }

        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow digits, control keys (Backspace), and one decimal point.
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }

            // Only allow one decimal point, and not as first character
            var tb = sender as TextBox;
            if (tb != null && e.KeyChar == '.')
            {
                if (tb.Text.Contains(".") || tb.SelectionStart == 0 && tb.TextLength == 0)
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        // Add missing helper used by txt_part_number_KeyUp / txt_brands_TextChanged
        private void generate_item_code()
        {
            string brand_code = txt_brand_code.Text;
            string part_number = txt_part_number.Text;

            txt_code.Text = brand_code + part_number;
        }

        private void txt_item_number_KeyUp(object sender, KeyEventArgs e)
        {
            generate_item_code();
        }

        private void txt_product_code_KeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_product_code.Text) && e.KeyData == Keys.Enter)
            {
                frm_searchProducts search_product_obj = new frm_searchProducts(null, null, null, txt_product_code.Text, "", "", 0, false, false, null, this);
                search_product_obj.ShowDialog();
            }
        }

        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchProducts search_product_obj = new frm_searchProducts(null, null, null, txt_product_code.Text, "", "", 0, false, false, null, this);
            search_product_obj.ShowDialog();
        }

        private void btn_upload_picture_Click(object sender, EventArgs e)
        {
            OpenFileDialog opnfd = new OpenFileDialog();
            opnfd.Filter = "Image Files (*.jpg;*.jpeg;.*.gif;)|*.jpg;*.jpeg;.*.gif";
            if (opnfd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(opnfd.FileName);
                picture_name = opnfd.FileName;
            }
        }
    }
}
