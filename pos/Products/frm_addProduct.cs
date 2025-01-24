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

namespace pos
{
    public partial class frm_addProduct : Form
    {
        private frm_products mainForm;
        ProductBLL objBLL = new ProductBLL();
        private frm_searchSaleProducts searchsalesForm;
        private frm_sales salesForm;
        
        public int _product_id;
        public string _keyword;
        string _status;
        public string picture_name = "";

        public frm_addProduct(frm_products mainForm, int product_id, string status, frm_searchSaleProducts searchsalesForm = null, string keyword = "", frm_sales salesForm=null)
        {
            this.mainForm = mainForm;
            _product_id = product_id;
            _status = status;
            _keyword = keyword;
            this.searchsalesForm = searchsalesForm;
            this.salesForm = salesForm;

            InitializeComponent();
        }

        public frm_addProduct()
        {
            InitializeComponent();
            
        }
        
        public void frm_addProduct_Load(object sender, EventArgs e)
        {
            txt_barcode.Focus();
            cmb_item_type.SelectedIndex = 0;
            cmb_tax.SelectedIndex = 1;

            get_taxes_dropdownlist();
            get_brands_dropdownlist();
            get_origin_dropdownlist();
            get_product_group_dropdownlist();
            get_locations_dropdownlist();
            get_units_dropdownlist();
            get_categories_dropdownlist();

            txt_item_number.Text = _keyword;

            if(_product_id != 0)
            {
                load_product_detail(_product_id);
            }
            
            if (_status == "true")
            {
                btn_save.Text = "Update";
                lbl_header_title.Text = "Update Product";
                
                if (UsersModal.logged_in_user_level != 1)
                {
                    txt_item_number.Enabled = false;
                }
                
            }
            else
            {
                btn_save.Text = "Save";
            }
        }


        public void load_product_detail(int product_id)
        {
            ProductBLL objBLL = new ProductBLL();
            DataTable dt = objBLL.GetAllByProductId(product_id);
            foreach (DataRow myProductView in dt.Rows)
            {
                txt_id.Text = myProductView["id"].ToString();
                txt_name.Text = myProductView["name"].ToString();
                txt_name_ar.Text = myProductView["name_ar"].ToString();
                txt_barcode.Text = myProductView["barcode"].ToString();
                txt_code.Text = myProductView["code"].ToString();
                txt_item_number.Text = myProductView["item_number"].ToString();
                cmb_item_type.Text = myProductView["item_type"].ToString();
                txt_cost_price.Text = myProductView["cost_price"].ToString();
                txt_unit_price.Text = myProductView["unit_price"].ToString();
                txt_unit_price_2.Text = myProductView["unit_price_2"].ToString();
                txt_description.Text = myProductView["description"].ToString();

                cmb_brands.SelectedValue = myProductView["brand_code"].ToString();
                cmb_units.SelectedValue = myProductView["unit_id"].ToString();
                cmb_locations.SelectedValue = myProductView["location_code"].ToString();
                cmb_categories.SelectedValue= myProductView["category_code"].ToString();
                cmb_tax.SelectedValue = myProductView["tax_id"].ToString();
                //cmb_origin.SelectedValue = myProductView["origin"].ToString();

                txt_demand_qty.Text = myProductView["demand_qty"].ToString();
                txt_pur_dmnd_qty.Text =myProductView["purchase_demand_qty"].ToString();
                txt_sale_dmnd_qty.Text = myProductView["sale_demand_qty"].ToString();
                txt_restock_level.Text = myProductView["re_stock_level"].ToString();

                if (myProductView["picture"].ToString() != "")
                {
                    byte[] myImage = new byte[0];
                    myImage = (byte[])myProductView["Picture"];
                    MemoryStream stream = new MemoryStream(myImage);
                    if (stream.Length > 0)
                    {
                        pictureBox1.Image = Image.FromStream(stream);

                    }

                }
                
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (objBLL.IsProductExist(txt_code.Text,txt_category_code.Text) && _status == "false")
            {
                MessageBox.Show("Product already exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            FileStream fs;
            BinaryReader br; 

                if (txt_code.Text != string.Empty && txt_name.Text != string.Empty)
                {
                    ProductModal info = new ProductModal();

                    //string FileName = openFileDialog1.FileName; 
                    if(picture_name != "")
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
                    info.item_number = txt_item_number.Text;
                    //info.origin = (cmb_origin.SelectedValue != null ? cmb_origin.SelectedValue.ToString() : "" );
                    info.group_code = (cmb_groups.SelectedValue != null ? cmb_groups.SelectedValue.ToString() : "");

                    info.name = txt_name.Text;
                    info.name_ar = txt_name_ar.Text;
                    info.cost_price = (String.IsNullOrEmpty(txt_cost_price.Text)) ?  0 : double.Parse(txt_cost_price.Text);
                    info.unit_price = (String.IsNullOrEmpty(txt_unit_price.Text)) ? 0 : double.Parse(txt_unit_price.Text);
                    info.unit_price_2 = (String.IsNullOrEmpty(txt_unit_price_2.Text)) ? 0 : double.Parse(txt_unit_price_2.Text);
                    info.item_type = cmb_item_type.Text;
                    info.description = txt_description.Text;
                    info.tax_id = (cmb_tax.SelectedValue == null ? 0 : Convert.ToInt32(cmb_tax.SelectedValue.ToString()));
                    info.unit_id = Convert.ToInt32(cmb_units.SelectedValue.ToString());
                    info.location_code = (cmb_locations.SelectedValue != null ? cmb_locations.SelectedValue.ToString() : "");
                    info.category_code = (cmb_categories.SelectedValue != null ? cmb_categories.SelectedValue.ToString() : "");
                    info.brand_code = (cmb_brands.SelectedValue != null ? cmb_brands.SelectedValue.ToString() : "");
                    info.demand_qty = (txt_demand_qty.Text != "" ? decimal.Parse(txt_demand_qty.Text) : 0);
                    info.purchase_demand_qty = (txt_pur_dmnd_qty.Text != "" ? decimal.Parse(txt_pur_dmnd_qty.Text) : 0);
                    info.sale_demand_qty = (txt_sale_dmnd_qty.Text != "" ? decimal.Parse(txt_sale_dmnd_qty.Text) : 0);
                    info.re_stock_level = (txt_restock_level.Text != "" ? decimal.Parse(txt_restock_level.Text) : 0);
                    
                    if (_status == "true")
                    {
                        info.id = int.Parse(txt_id.Text);
                    
                        int result = objBLL.Update(info);
                        if (result > 0)
                        {
                            MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        int result = objBLL.Insert(info);
                        if (result > 0)
                        {
                            MessageBox.Show("Record created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    
                    if(mainForm != null)
                    {
                        mainForm.load_Products_grid();

                    }
                    else if (searchsalesForm != null)
                    {
                        searchsalesForm.BringToFront();
                        searchsalesForm.Activate();
                    }
                    else if (salesForm != null)
                    {
                        //salesForm.load_products(_product_id.ToString());

                    }
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Please enter value in field", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
            
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Are you sure you want to save", "Save Record", buttons, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    btn_save.PerformClick();
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void get_taxes_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,title";
            string table = "pos_taxes";

            DataTable taxes = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = taxes.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "Please Select";              // Set Column Value
            //taxes.Rows.InsertAt(emptyRow, 0);

            cmb_tax.DisplayMember = "title";
            cmb_tax.ValueMember = "id";
            cmb_tax.DataSource = taxes;

        }

        public void get_brands_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_brands";

            DataTable brands = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = brands.NewRow();
            emptyRow[0] = "";              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            brands.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = brands.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[1] = "ADD NEW";              // Set Column Value
            brands.Rows.InsertAt(emptyRow1, 1);

            cmb_brands.DisplayMember = "name";
            cmb_brands.ValueMember = "code";
            cmb_brands.DataSource = brands;

        }

        private void cmb_brands_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_brands.SelectedValue != null && cmb_brands.SelectedValue.ToString() == "-1")
            {
                frm_addBrand brandfrm = new frm_addBrand(null);
                brandfrm.ShowDialog();

                get_brands_dropdownlist();


            }
            txt_brand_code.Text = (cmb_brands.SelectedValue != null ? cmb_brands.SelectedValue.ToString() : "");
            generate_item_code();

            if (txt_brand_code.Text != "")
            {
                GeneralBLL objBLL = new GeneralBLL();
                String keyword = "*";
                String table = "pos_brands WHERE code = '" + txt_brand_code.Text + "'";
                DataTable dt = objBLL.GetRecord(keyword, table);
                foreach (DataRow dr in dt.Rows)
                {
                    cmb_categories.SelectedValue = dr["category_code"].ToString();
                    cmb_groups.SelectedValue = dr["group_code"].ToString();
                }
            }
            else
            {
                cmb_categories.SelectedValue = "";
            }


        }

        public void get_origin_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_country_origin";

            DataTable origin = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = origin.NewRow();
            emptyRow[0] = "";              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            origin.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = origin.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[1] = "ADD NEW";              // Set Column Value
            origin.Rows.InsertAt(emptyRow1, 1);

            //cmb_origin.DisplayMember = "name";
            //cmb_origin.ValueMember = "code";
            //cmb_origin.DataSource = origin;

        }

        private void cmb_origin_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmb_origin.SelectedValue != null && cmb_origin.SelectedValue.ToString() == "-1")
            //{
            //    frm_addOrigin originfrm = new frm_addOrigin(null);
            //    originfrm.ShowDialog();

            //    get_origin_dropdownlist();
            //}
            //txt_origin_code.Text = (cmb_origin.SelectedValue != null ? cmb_origin.SelectedValue.ToString() : "");
            //generate_item_code();
        }

        public void get_product_group_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_product_groups";

            DataTable groups = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = groups.NewRow();
            emptyRow[0] = "";              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            groups.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = groups.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[1] = "ADD NEW";              // Set Column Value
            groups.Rows.InsertAt(emptyRow1, 1);

            cmb_groups.DisplayMember = "name";
            cmb_groups.ValueMember = "code";
            cmb_groups.DataSource = groups;

        }
        public void get_locations_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_locations";

            DataTable locations = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = locations.NewRow();
            emptyRow[0] = "";              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            locations.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = locations.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[1] = "ADD NEW";              // Set Column Value
            locations.Rows.InsertAt(emptyRow1, 1);

            cmb_locations.DisplayMember = "name";
            cmb_locations.ValueMember = "code";
            cmb_locations.DataSource = locations;

        }

        public void get_units_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "pos_units";

            DataTable units = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = units.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            units.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = units.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[1] = "ADD NEW";              // Set Column Value
            units.Rows.InsertAt(emptyRow1, 1);

            cmb_units.DisplayMember = "name";
            cmb_units.ValueMember = "id";
            cmb_units.DataSource = units;

        }

        public void get_categories_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_categories";

            DataTable categories = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = categories.NewRow();
            emptyRow[0] = "";              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            // Set Column Value
            categories.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = categories.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[1] = "ADD NEW";              // Set Column Value
            categories.Rows.InsertAt(emptyRow1, 1);

            cmb_categories.DisplayMember = "name";
            cmb_categories.ValueMember = "code";
            cmb_categories.DataSource = categories;

        }

        public DataTable get_GL_accounts_dt()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_accounts";

            DataTable dt = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = dt.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Select Account";              // Set Column Value
            dt.Rows.InsertAt(emptyRow, 0);
            return dt;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Dispose(); 
            this.Close();
        }

        private void frm_addProduct_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
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

        private void cmb_categories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_categories.SelectedValue != null && cmb_categories.SelectedValue.ToString() == "-1")
            {
                frm_addCategory frm = new frm_addCategory(null);
                frm.ShowDialog();

                get_categories_dropdownlist();
            }
            txt_category_code.Text = (cmb_categories.SelectedValue != null ? cmb_categories.SelectedValue.ToString() : "");
        
            
        }

        private void cmb_locations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_locations.SelectedValue.ToString() == "-1")
            {
                frm_addLocation frm = new frm_addLocation(null);
                frm.ShowDialog();

                get_locations_dropdownlist();
            }
        }

        private void cmb_units_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_units.SelectedValue.ToString() == "-1")
            {
                frm_addUnit frm = new frm_addUnit(null);
                frm.ShowDialog();

                get_units_dropdownlist();
            }
        }

        private void txt_code_KeyDown(object sender, KeyEventArgs e)
        {
            if (objBLL.IsProductExist(txt_code.Text,txt_category_code.Text) && e.KeyData == Keys.Enter)
            {
                MessageBox.Show("Item code already exist","Product",MessageBoxButtons.OK,MessageBoxIcon.Error);
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

        private void txt_item_number_KeyUp(object sender, KeyEventArgs e)
        {
            generate_item_code();
        }

        private void generate_item_code()
        {
            string brand_code = txt_brand_code.Text;
            string item_number = txt_item_number.Text;

            txt_code.Text = brand_code +item_number;
        }

        private void txt_item_number_Leave(object sender, EventArgs e)
        {
            if (txt_item_number.Text != "")
            {
                grid_products.AutoGenerateColumns = false;
                grid_products.DataSource = objBLL.SearchRecordByProductNumber(txt_item_number.Text);
            }
        }

        private void grid_products_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string name = grid_products.Columns[e.ColumnIndex].Name;
            if (name == "copy")
            {
                txt_name.Text = grid_products.CurrentRow.Cells["name"].Value.ToString();
                
            }
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
