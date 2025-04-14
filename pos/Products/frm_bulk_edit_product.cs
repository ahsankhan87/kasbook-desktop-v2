using POS.BLL;
using POS.Core;
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
    public partial class frm_bulk_edit_product : Form
    {

        private DataGridView brandsDataGridView = new DataGridView();
        private DataGridView categoriesDataGridView = new DataGridView();
        private DataGridView groupsDataGridView = new DataGridView();
        public string lang = (UsersModal.logged_in_lang.Length > 0 ? UsersModal.logged_in_lang : "en-US");

        DataTable products_dt = new DataTable();
        DataTable invoiceSearch_dt = new DataTable();

        public int inventory_acc_id = 0;
        public int item_variance_acc_id = 0;
        
        public frm_bulk_edit_product()
        {
            InitializeComponent();
            //txt_brands.Click += TextBoxOnClick;
            txt_categories.Click += TextBoxOnClick;
            txt_groups.Click += TextBoxOnClick;
        }

        private void TextBoxOnClick(object sender, EventArgs eventArgs)
        {
            var txt_brands = (TextBox)sender;
            txt_brands.SelectAll();
            txt_brands.Focus();

            //var txt_categories = (TextBox)sender;
            //txt_categories.SelectAll();
            //txt_categories.Focus();

            var txt_groups = (TextBox)sender;
            txt_groups.SelectAll();
            txt_groups.Focus();
        }
        
        private void frm_bulk_edit_product_Load(object sender, EventArgs e)
        {
            autoCompleteInvoice();
            //load_Products_grid();
            if (tabControl1.SelectedTab == tabControl1.TabPages["edit_desc"])//your specific tabname
            {
                get_product_locations_dropdownlist();
            }
        }

        public void autoCompleteInvoice()
        {
            try
            {
                txt_purchase_inv_no.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txt_purchase_inv_no.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection coll = new AutoCompleteStringCollection();

                GeneralBLL invoicesBLL_obj = new GeneralBLL();
                string keyword = "TOP 500 invoice_no ";
                string table = "pos_purchases WHERE account <> 'return' ORDER BY id desc";
                DataTable dt = invoicesBLL_obj.GetRecord(keyword, table);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        coll.Add(dr["invoice_no"].ToString());

                    }

                }

                txt_purchase_inv_no.AutoCompleteCustomSource = coll;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        
        public void get_from_locations_dropdownlist()
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["loc_transfer"])//your specific tabname
            {
                GeneralBLL generalBLL_obj = new GeneralBLL();
                string keyword = "code,name";
                string table = "pos_locations";

                DataTable locations = generalBLL_obj.GetRecord(keyword, table);

                DataRow emptyRow = locations.NewRow();
                emptyRow[0] = 0;              // Set Column Value
                emptyRow[1] = "All";              // Set Column Value
                locations.Rows.InsertAt(emptyRow, 0);


                cmb_from_locations.DisplayMember = "name";
                cmb_from_locations.ValueMember = "code";
                cmb_from_locations.DataSource = locations;

                
            }
        }


        public void get_to_locations_dropdownlist()
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["loc_transfer"])//your specific tabname
            {
                GeneralBLL generalBLL_obj = new GeneralBLL();
                string keyword = "code,name";
                string table = "pos_locations";

                DataTable locations = generalBLL_obj.GetRecord(keyword, table);
                //DataRow emptyRow = locations.NewRow();
                //emptyRow[0] = 0;              // Set Column Value
                //emptyRow[1] = "All";              // Set Column Value
                //locations.Rows.InsertAt(emptyRow, 0);

                cmb_to_locations.DisplayMember = "name";
                cmb_to_locations.ValueMember = "code";
                cmb_to_locations.DataSource = locations;
            }
        }

        public void get_product_locations_dropdownlist()
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["edit_desc"])//your specific tabname
            {
                GeneralBLL generalBLL_obj = new GeneralBLL();
                string keyword = "code,name";
                string table = "pos_locations";

                DataTable locations = generalBLL_obj.GetRecord(keyword, table);

                DataRow emptyRow = locations.NewRow();
                emptyRow[0] = "all";              // Set Column Value
                emptyRow[1] = "All";              // Set Column Value
                locations.Rows.InsertAt(emptyRow, 0);

                cmb_edit_pro_loc.DisplayMember = "name";
                cmb_edit_pro_loc.ValueMember = "code";
                cmb_edit_pro_loc.DataSource = locations;
            }
        }

        private void GetMAXInvoiceNo()
        {
            ProductBLL objBLL = new ProductBLL();
            txt_ref_no.Text = objBLL.GetMaxLocationTransferInvoiceNo();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 qresult = 0;
                DialogResult result = MessageBox.Show("Are you sure you want to update", "Sale Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (grid_search_products.Rows.Count > 0)
                    
                    {
                        ProductModal info = new ProductModal();
                        ProductBLL productBLLObj = new ProductBLL();
                        
                        for (int i = 0; i < grid_search_products.Rows.Count; i++)
                        {
                            if (grid_search_products.Rows[i].Cells["code"].Value != null)
                            {
                                info.id = int.Parse(grid_search_products.Rows[i].Cells["id"].Value.ToString());
                                info.code = (string.IsNullOrEmpty(grid_search_products.Rows[i].Cells["code"].Value.ToString()) ? "" : grid_search_products.Rows[i].Cells["code"].Value.ToString()); 
                                info.name = (string.IsNullOrEmpty(grid_search_products.Rows[i].Cells["name"].Value.ToString()) ? "" : grid_search_products.Rows[i].Cells["name"].Value.ToString());
                                info.name_ar = (string.IsNullOrEmpty(grid_search_products.Rows[i].Cells["name_ar"].Value.ToString()) ? "" : grid_search_products.Rows[i].Cells["name_ar"].Value.ToString());
                                info.cost_price = (string.IsNullOrEmpty(grid_search_products.Rows[i].Cells["cost_price"].Value.ToString()) ? 0 : double.Parse(grid_search_products.Rows[i].Cells["cost_price"].Value.ToString()));
                                info.unit_price = (string.IsNullOrEmpty(grid_search_products.Rows[i].Cells["unit_price"].Value.ToString()) ? 0 : double.Parse(grid_search_products.Rows[i].Cells["unit_price"].Value.ToString()));
                                //info.unit_price_2 = (String.IsNullOrEmpty(txt_unit_price_2.Text)) ? 0 : double.Parse(txt_unit_price_2.Text);
                                info.item_type = (string.IsNullOrEmpty(grid_search_products.Rows[i].Cells["item_type"].Value.ToString()) ? "" : grid_search_products.Rows[i].Cells["item_type"].Value.ToString());
                                info.description = (string.IsNullOrEmpty(grid_search_products.Rows[i].Cells["description"].Value.ToString()) ? "" : grid_search_products.Rows[i].Cells["description"].Value.ToString());
                                
                                if (grid_search_products.Rows[i].Cells["location_code"].Value == null || grid_search_products.Rows[i].Cells["location_code"].Value == DBNull.Value || String.IsNullOrEmpty(grid_search_products.Rows[i].Cells["location_code"].Value as String) || String.IsNullOrWhiteSpace(grid_search_products.Rows[i].Cells["location_code"].Value.ToString()))
                                {
                                    info.location_code = "";
                                }
                                else
                                {
                                    info.location_code = grid_search_products.Rows[i].Cells["location_code"].Value.ToString();
                                }

                                qresult = Convert.ToInt32(productBLLObj.BulkUpdate(info));
                            }
                            
                        }
                        
                        if (qresult > 0)
                        {
                            MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            grid_search_products.DataSource = null;
                            grid_search_products.Refresh();
                            txt_search.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("no record found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            String condition = txt_search.Text.Trim();
            try
            {
                if (condition != string.Empty)
                {
                    //bool by_code = rb_by_code.Checked;
                    //bool by_name = rb_by_name.Checked;
                    
                    //ProductBLL objBLL = new ProductBLL();
                    //grid_search_products.AutoGenerateColumns = false;

                    //String condition = txt_search.Text.Trim();
                    //grid_search_products.DataSource = objBLL.SearchRecord(condition, by_code, by_name);

                    var brand_code = "";
                    var category_code = txt_category_code.Text;
                    var group_code = txt_group_code.Text;


                    grid_search_products.DataSource = null;

                    //bind data in data grid view  
                    //ProductBLL objBLL = new ProductBLL();
                    grid_search_products.AutoGenerateColumns = false;
                    
                    //String keyword = "id,code,name,qty,avg_cost,unit_price,loc_code, (1) AS purchase_order_qty";
                    //String table = string.Format("pos_products_location_view where brand_code LIKE '%{0}%' ORDER BY id desc", brand_code);
                    products_dt = ProductBLL.SearchProductByBrandAndCategory(condition, category_code, brand_code, group_code);
                    grid_search_products.DataSource = products_dt;

                    txt_search.Text = "";
                    lbl_total_rows.Text = "Total Rows: " + grid_search_products.RowCount.ToString();
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void load_Products_grid()
        {
            try
            {
                //grid_search_products.DataSource = null;

                ////bind data in data grid view  
                //GeneralBLL objBLL = new GeneralBLL();
                //grid_search_products.AutoGenerateColumns = false;

                //String keyword = "I.id,P.name AS product_name,I.item_id,I.qty,I.unit_price,I.cost_price,I.invoice_no,I.description";
                //String table = "pos_inventory I LEFT JOIN pos_products P ON P.id=I.item_id WHERE P.id = "+ _product_id+" ORDER BY I.id DESC";
                //grid_search_products.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }


        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_bulk_edit_product_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                //if (e.KeyData == Keys.Enter)
                //{
                //    SendKeys.Send("{TAB}");
                //}

                if (e.KeyCode == Keys.F3)
                {
                    btn_update.PerformClick();
                }
                if (e.Control && e.KeyData == Keys.P)
                {
                    btn_products_print.PerformClick();
                }
                if (e.KeyCode == Keys.F4)
                {
                    btn_transfer.PerformClick();
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



        private void cmb_from_locations_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmb_from_locations.SelectedValue != null)
                {
                    grid_loc_transfer.DataSource = null;
                    //bind data in data grid view  
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_loc_transfer.AutoGenerateColumns = false;

                    //String keyword = "P.id,P.code,P.name,(select sum(PS.qty) from  pos_product_stocks PS where PS.id=P.id AND PS.loc_code = '" + cmb_from_locations.SelectedValue.ToString() + "') as qty";
                    String keyword = "P.id, P.code,P.name,P.qty,P.qty as transfer_qty";
                    String table = "pos_products_location_view P";
                    if (cmb_from_locations.SelectedValue.ToString() != "0")
                    {
                        table += " where P.loc_code = '" + cmb_from_locations.SelectedValue.ToString() + "'";
                    }
                    table += " Group by P.id, P.code,P.name,P.qty HAVING P.qty > 0";
                    //String table = "pos_products AS P";
                    grid_loc_transfer.DataSource = objBLL.GetRecord(keyword, table);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_transfer_search_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_transfer_search.Text != string.Empty)
                {
                    bool by_code = rb_by_code_trans.Checked;
                    bool by_name = rb_by_name_trans.Checked;

                    //ProductBLL objBLL = new ProductBLL();
                    //grid_search_products.AutoGenerateColumns = false;

                    String condition = txt_transfer_search.Text.Trim();
                    //grid_search_products.DataSource = objBLL.SearchRecord(condition, by_code, by_name);

                    GeneralBLL objBLL = new GeneralBLL();
                    grid_loc_transfer.AutoGenerateColumns = false;

                    String keyword = "P.id, P.code,P.name,P.qty,P.qty as transfer_qty";
                    String table = "pos_products_location_view P";

                    if (by_code)
                    {
                        table += " WHERE (P.code LIKE '%" + condition + "%' OR replace(code,'-',' ') LIKE '%" + condition + "%')";

                    }
                    else if (by_name)
                    {
                        table += " WHERE P.name LIKE '%" + condition + "%'";

                    }
                    if(cmb_from_locations.SelectedValue.ToString() != "0")
                    {
                        table += " AND P.loc_code = '" + cmb_from_locations.SelectedValue.ToString() + "'";
                    }
                    
                    grid_loc_transfer.DataSource = objBLL.GetRecord(keyword, table);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_transfer_Click(object sender, EventArgs e)
        {

            try
            {
                if (cmb_from_locations.SelectedValue.ToString() == cmb_to_locations.SelectedValue.ToString())
                {
                    MessageBox.Show("From and To Location shall not be same", "Inventory Location Transfer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Int32 qresult = 0;
                DialogResult result = MessageBox.Show("Are you sure you want to transfer", "Inventory Location Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (grid_loc_transfer.Rows.Count > 0)
                    {
                        ProductModal info = new ProductModal();
                        ProductBLL productBLLObj = new ProductBLL();

                        for (int i = 0; i < grid_loc_transfer.Rows.Count; i++)
                        {
                            if (grid_loc_transfer.Rows[i].Cells["product_id"].Value != null && grid_loc_transfer.Rows[i].Cells["transfer_qty"].Value != "")
                            {

                                info.invoice_no = txt_ref_no.Text;
                                info.id = int.Parse(grid_loc_transfer.Rows[i].Cells["product_id"].Value.ToString());
                                info.code = grid_loc_transfer.Rows[i].Cells["product_code"].Value.ToString();
                                info.qty = (grid_loc_transfer.Rows[i].Cells["transfer_qty"].Value.ToString() != "" ? double.Parse(grid_loc_transfer.Rows[i].Cells["transfer_qty"].Value.ToString()) : 0);
                                info.location_code = cmb_to_locations.SelectedValue.ToString();
                                info.from_location_code = cmb_from_locations.SelectedValue.ToString();
                                info.description = "Product Location Transfer";

                                qresult = Convert.ToInt32(productBLLObj.UpdateProductLocationTransfer(info));


                            }

                        }

                        if (qresult > 0)
                        {
                            MessageBox.Show("Transfer processed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            GetMAXInvoiceNo();
                            grid_loc_transfer.DataSource = null;
                            grid_loc_transfer.Refresh();
                            txt_transfer_search.Text = "";
                            txt_transfer_search.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("no record found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frm_low_stock_products_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                brandsDataGridView.Focus();

            }
        }

        private void cmb_locations_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SetupGroupsDataGridView()
        {
            var current_lang_code = System.Globalization.CultureInfo.CurrentCulture;
            groupsDataGridView.ColumnCount = 2;
            groupsDataGridView.Name = "groupsDataGridView";
            if (lang == "en-US")
            {
                groupsDataGridView.Location = new Point(540, 60);
                groupsDataGridView.Size = new Size(250, 250);
            }
            else if (lang == "ar-SA")
            {
                groupsDataGridView.Location = new Point(16, 95);
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
                txt_categories.Focus();

            }
        }

        private void groupsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_group_code.Text = groupsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_groups.Text = groupsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(groupsDataGridView);
            txt_categories.Focus();

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

        private void SetupCategoriesDataGridView()
        {
            var current_lang_code = System.Globalization.CultureInfo.CurrentCulture;
            categoriesDataGridView.ColumnCount = 2;
            categoriesDataGridView.Name = "categoriesDataGridView";
            if (lang == "en-US")
            {
                categoriesDataGridView.Location = new Point(320, 60);
                categoriesDataGridView.Size = new Size(250, 250);
            }
            else if (lang == "ar-SA")
            {
                categoriesDataGridView.Location = new Point(250, 95);
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
                txt_search.Focus();

            }
        }

        private void categoriesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_category_code.Text = categoriesDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_categories.Text = categoriesDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(categoriesDataGridView);
            txt_search.Focus();

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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["loc_transfer"])//your specific tabname
            {
                GetMAXInvoiceNo();
                get_from_locations_dropdownlist();
                get_to_locations_dropdownlist();
            }
        }

        private void cmb_edit_pro_loc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab == tabControl1.TabPages["edit_desc"])//your specific tabname
                {
                    if (cmb_edit_pro_loc.SelectedValue != null || cmb_edit_pro_loc.SelectedValue.ToString() != "all")
                    {
                        grid_search_products.DataSource = null;
                        //bind data in data grid view  
                        GeneralBLL objBLL = new GeneralBLL();
                        grid_search_products.AutoGenerateColumns = false;

                        //String keyword = "P.id,P.code,P.name,(select sum(PS.qty) from  pos_product_stocks PS where PS.id=P.id AND PS.loc_code = '" + cmb_from_locations.SelectedValue.ToString() + "') as qty";
                        String keyword = "P.id, P.code,P.name,P.name_ar,COALESCE((select TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_code=p.code and s.branch_id="+ UsersModal.logged_in_branch_id + "),0) as qty,P.qty as transfer_qty,P.avg_cost,P.cost_price,P.unit_price,P.location_code,P.description,P.item_type ";
                        String table = "pos_products P where P.location_code = '" + cmb_edit_pro_loc.SelectedValue.ToString() + "' ";
                        //String table = "pos_products AS P";
                        products_dt = objBLL.GetRecord(keyword, table);
                        grid_search_products.DataSource = products_dt;
                        lbl_total_rows.Text = "Total Rows: " + grid_search_products.RowCount.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        
        }

        private void btn_products_print_Click(object sender, EventArgs e)
        {
            if(grid_search_products.Rows.Count > 0)
            {
                if(invoiceSearch_dt.Rows.Count > 0)
                {
                    pos.Reports.Products.frm_products_report frm_product_obj = new Reports.Products.frm_products_report(invoiceSearch_dt, false, cmb_edit_pro_loc.SelectedValue.ToString());
                    frm_product_obj.Show();

                }else if(products_dt.Rows.Count > 0)
                {
                    pos.Reports.Products.frm_products_report frm_product_obj = new Reports.Products.frm_products_report(products_dt, false, cmb_edit_pro_loc.SelectedValue.ToString());
                    frm_product_obj.Show();
                }
            }
        }

        private void txt_search_purchase_invoice_Click(object sender, EventArgs e)
        {
            
        }

        public void Load_products_to_grid_by_invoiceno(DataTable dt)
        {
            try
            {
                grid_search_products.DataSource = null;
                grid_search_products.Rows.Clear();
                grid_search_products.Refresh();

                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow myProductView in dt.Rows)
                    {
                        string  id = myProductView["id"].ToString();
                        string code = myProductView["code"].ToString();
                        string name = myProductView["name"].ToString();
                        string name_ar = "";
                        string qty = Math.Round(Convert.ToDecimal(myProductView["quantity"]),2).ToString();
                        string cost_price = Math.Round(Convert.ToDecimal(myProductView["cost_price"]),2).ToString();
                        string unit_price = Math.Round(Convert.ToDecimal(myProductView["unit_price"]), 2).ToString();
                        string description = "";
                        string location_code = myProductView["location_code"].ToString();
                        string category = myProductView["category"].ToString();
                        string item_type = "";

                        string[] row0 = { id, code, name, name_ar, qty, cost_price, unit_price, description, location_code,category, item_type };

                        grid_search_products.Rows.Add(row0);
                        
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txt_purchase_inv_no_KeyDown(object sender, KeyEventArgs e)
        {
            if (txt_purchase_inv_no.Text != "" && e.KeyData == Keys.Enter)
            {

                btn_search_purchase_invoice.PerformClick();
            }
        }

        private void btn_search_purchase_invoice_Click(object sender, EventArgs e)
        {
            string invoice_no = txt_purchase_inv_no.Text;

            PurchasesBLL purchasesObj = new PurchasesBLL();

            invoiceSearch_dt = purchasesObj.GetAllPurchaseByInvoice(invoice_no);
            //grid_search_products.DataSource = invoiceSearch_dt;
            Load_products_to_grid_by_invoiceno(invoiceSearch_dt);
        }

       
    }



    
}
