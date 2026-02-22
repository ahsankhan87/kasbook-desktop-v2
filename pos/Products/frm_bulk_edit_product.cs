using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

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

            var txt_groups = (TextBox)sender;
            txt_groups.SelectAll();
            txt_groups.Focus();
        }

        private void frm_bulk_edit_product_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            using (BusyScope.Show(this, UiMessages.T("Loading...", "جاري التحميل...")))
            {
                autoCompleteInvoice();
                //load_Products_grid();
                if (tabControl1.SelectedTab == tabControl1.TabPages["edit_desc"])//your specific tabname
                {
                    get_product_locations_dropdownlist();
                    get_product_to_locations_dropdownlist();
                }
            }
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(null, null, panel1, grid_search_products, id);
            AppTheme.ApplyListFormStyleLightHeader(null, null, null, grid_loc_transfer);
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
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
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

                cmb_from_pro_loc.DisplayMember = "name";
                cmb_from_pro_loc.ValueMember = "code";
                cmb_from_pro_loc.DataSource = locations;

            }
        }
        public void get_product_to_locations_dropdownlist()
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

                //To Location Drop down list
                cmb_to_pro_loc.DisplayMember = "name";
                cmb_to_pro_loc.ValueMember = "code";
                cmb_to_pro_loc.DataSource = locations;
            }
        }
        private void GetMAXInvoiceNo()
        {
            ProductBLL objBLL = new ProductBLL();
            txt_ref_no.Text = objBLL.GetMaxLocationTransferInvoiceNo();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Updating products...", "جاري تحديث الأصناف...")))
            {
                try
                {
                    if (grid_search_products.Rows.Count <= 0)
                    {
                        UiMessages.ShowWarning(
                            "No products found.",
                            "لا توجد أصناف.",
                            captionEn: "Products",
                            captionAr: "الأصناف");
                        return;
                    }

                    DialogResult result = UiMessages.ConfirmYesNo(
                        "Are you sure you want to update the selected products?",
                        "هل أنت متأكد أنك تريد تحديث الأصناف المحددة؟",
                        captionEn: "Confirm",
                        captionAr: "تأكيد");

                    if (result != DialogResult.Yes)
                        return;

                    int updated = 0;
                    ProductModal info = new ProductModal();
                    ProductBLL productBLLObj = new ProductBLL();

                    for (int i = 0; i < grid_search_products.Rows.Count; i++)
                    {
                        if (grid_search_products.Rows[i].Cells["code"].Value != null)
                        {
                            info.id = int.Parse(grid_search_products.Rows[i].Cells["id"].Value.ToString());
                            info.item_number = (string.IsNullOrEmpty(grid_search_products.Rows[i].Cells["item_number"].Value.ToString()) ? "" : grid_search_products.Rows[i].Cells["item_number"].Value.ToString());
                            info.code = (string.IsNullOrEmpty(grid_search_products.Rows[i].Cells["code"].Value.ToString()) ? "" : grid_search_products.Rows[i].Cells["code"].Value.ToString());
                            info.name = (string.IsNullOrEmpty(grid_search_products.Rows[i].Cells["name"].Value.ToString()) ? "" : grid_search_products.Rows[i].Cells["name"].Value.ToString());
                            info.name_ar = (string.IsNullOrEmpty(grid_search_products.Rows[i].Cells["name_ar"].Value.ToString()) ? "" : grid_search_products.Rows[i].Cells["name_ar"].Value.ToString());
                            info.cost_price = (string.IsNullOrEmpty(grid_search_products.Rows[i].Cells["cost_price"].Value.ToString()) ? 0 : double.Parse(grid_search_products.Rows[i].Cells["cost_price"].Value.ToString()));
                            info.unit_price = (string.IsNullOrEmpty(grid_search_products.Rows[i].Cells["unit_price"].Value.ToString()) ? 0 : double.Parse(grid_search_products.Rows[i].Cells["unit_price"].Value.ToString()));
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

                            var qresult = Convert.ToInt32(productBLLObj.BulkUpdate(info));
                            if (qresult > 0)
                                updated++;
                        }

                    }

                    if (updated > 0)
                    {
                        UiMessages.ShowInfo(
                            $"Products updated successfully. Rows: {updated}",
                            $"تم تحديث الأصناف بنجاح. عدد الصفوف: {updated}",
                            captionEn: "Success",
                            captionAr: "نجاح");

                        grid_search_products.DataSource = null;
                        grid_search_products.Refresh();
                        txt_search.Focus();
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "No changes were saved.",
                            "لم يتم حفظ أي تغييرات.",
                            captionEn: "Error",
                            captionAr: "خطأ");
                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            String condition = (txt_search.Text ?? string.Empty).Trim();
            using (BusyScope.Show(this, UiMessages.T("Searching products...", "جاري البحث عن الأصناف...")))
            {
                try
                {
                    var brand_code = "";
                    var category_code = txt_category_code.Text;
                    var group_code = txt_group_code.Text;
                    bool qty_onhand = chk_qty_on_hand.Checked;

                    grid_search_products.DataSource = null;
                    grid_search_products.AutoGenerateColumns = false;

                    products_dt = ProductBLL.SearchProductByLocations(condition, category_code, brand_code, group_code, cmb_from_pro_loc.Text, cmb_to_pro_loc.Text, qty_onhand);
                    grid_search_products.DataSource = products_dt;

                    txt_search.Text = "";
                    lbl_total_rows.Text = UiMessages.T("Total Rows: ", "إجمالي الصفوف: ") + grid_search_products.RowCount;

                    if (grid_search_products.RowCount <= 0)
                    {
                        UiMessages.ShowInfo(
                            "No products found.",
                            "لم يتم العثور على أصناف.",
                            captionEn: "Search",
                            captionAr: "بحث");
                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
            }
        }

        public void load_Products_grid()
        {
            try
            {
                // intentionally left blank
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
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
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
            }
        }

        private void cmb_from_locations_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmb_from_locations.SelectedValue != null)
                {
                    using (BusyScope.Show(this, UiMessages.T("Loading products...", "جاري تحميل الأصناف...")))
                    {
                        grid_loc_transfer.DataSource = null;
                        GeneralBLL objBLL = new GeneralBLL();
                        grid_loc_transfer.AutoGenerateColumns = false;

                        String keyword = "P.id, P.code,P.name,P.qty,P.qty as transfer_qty, P.item_number";
                        String table = "pos_products_location_view P";
                        if (cmb_from_locations.SelectedValue.ToString() != "0")
                        {
                            table += " WHERE P.deleted=0 AND P.loc_code = '" + cmb_from_locations.SelectedValue.ToString().Replace("'", "''") + "'";
                        }
                        table += " Group by P.id, P.code,P.name,P.qty, P.item_number HAVING P.qty > 0";
                        grid_loc_transfer.DataSource = objBLL.GetRecord(keyword, table);
                    }
                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                throw;
            }

        }

        private void btn_transfer_search_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
            {
                try
                {
                    if (txt_transfer_search.Text != string.Empty)
                    {
                        bool by_code = rb_by_code_trans.Checked;
                        bool by_name = rb_by_name_trans.Checked;

                        String condition = txt_transfer_search.Text.Trim();

                        GeneralBLL objBLL = new GeneralBLL();
                        grid_loc_transfer.AutoGenerateColumns = false;

                        String keyword = "P.id, P.code,P.name,P.item_number,P.qty,P.qty as transfer_qty";
                        String table = "pos_products_location_view P";

                        if (by_code)
                        {
                            table += " WHERE P.deleted=0 AND (P.code LIKE '%" + condition.Replace("'", "''") + "%' OR replace(code,'-',' ') LIKE '%" + condition.Replace("'", "''") + "%')";

                        }
                        else if (by_name)
                        {
                            table += " WHERE P.deleted=0 AND P.name LIKE '%" + condition.Replace("'", "''") + "%'";

                        }
                        if (cmb_from_locations.SelectedValue.ToString() != "0")
                        {
                            table += " AND P.loc_code = '" + cmb_from_locations.SelectedValue.ToString().Replace("'", "''") + "'";
                        }

                        grid_loc_transfer.DataSource = objBLL.GetRecord(keyword, table);
                    }
                    else
                    {
                        UiMessages.ShowWarning(
                            "Please enter search text.",
                            "يرجى إدخال نص البحث.",
                            captionEn: "Search",
                            captionAr: "بحث");
                    }

                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
            }
        }

        private void btn_transfer_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Processing transfer...", "جاري تنفيذ النقل...")))
            {
                try
                {
                    if (cmb_from_locations.SelectedValue.ToString() == cmb_to_locations.SelectedValue.ToString())
                    {
                        UiMessages.ShowWarning(
                            "From and To Location must not be the same.",
                            "لا يمكن أن يكون موقع النقل من وإلى نفس الموقع.",
                            captionEn: "Inventory Location Transfer",
                            captionAr: "نقل موقع المخزون");
                        return;
                    }

                    DialogResult result = UiMessages.ConfirmYesNo(
                        "Are you sure you want to transfer?",
                        "هل أنت متأكد أنك تريد النقل؟",
                        captionEn: "Inventory Location Transfer",
                        captionAr: "نقل موقع المخزون");

                    if (result != DialogResult.Yes)
                        return;

                    if (grid_loc_transfer.Rows.Count <= 0)
                    {
                        UiMessages.ShowWarning(
                            "No products to transfer.",
                            "لا توجد أصناف للنقل.",
                            captionEn: "Inventory Location Transfer",
                            captionAr: "نقل موقع المخزون");
                        return;
                    }

                    int processed = 0;
                    ProductModal info = new ProductModal();
                    ProductBLL productBLLObj = new ProductBLL();

                    for (int i = 0; i < grid_loc_transfer.Rows.Count; i++)
                    {
                        if (grid_loc_transfer.Rows[i].Cells["product_id"].Value != null && grid_loc_transfer.Rows[i].Cells["transfer_qty"].Value != "")
                        {
                            if (double.Parse(grid_loc_transfer.Rows[i].Cells["transfer_qty"].Value.ToString()) > 0)
                            {

                                info.invoice_no = txt_ref_no.Text;
                                info.id = int.Parse(grid_loc_transfer.Rows[i].Cells["product_id"].Value.ToString());
                                info.item_number = grid_loc_transfer.Rows[i].Cells["loc_transfer_item_number"].Value.ToString();
                                info.code = grid_loc_transfer.Rows[i].Cells["product_code"].Value.ToString();
                                info.qty = (grid_loc_transfer.Rows[i].Cells["transfer_qty"].Value.ToString() != "" ? double.Parse(grid_loc_transfer.Rows[i].Cells["transfer_qty"].Value.ToString()) : 0);
                                info.location_code = cmb_to_locations.SelectedValue.ToString();
                                info.from_location_code = cmb_from_locations.SelectedValue.ToString();
                                info.description = "Product Location Transfer";

                                var qresult = Convert.ToInt32(productBLLObj.UpdateProductLocationTransfer(info));
                                if (qresult > 0)
                                    processed++;

                            }
                        }

                    }

                    if (processed > 0)
                    {
                        UiMessages.ShowInfo(
                            $"Transfer processed successfully. Items: {processed}",
                            $"تم تنفيذ النقل بنجاح. عدد الأصناف: {processed}",
                            captionEn: "Success",
                            captionAr: "نجاح");
                        GetMAXInvoiceNo();
                        grid_loc_transfer.DataSource = null;
                        grid_loc_transfer.Refresh();
                        txt_transfer_search.Text = "";
                        txt_transfer_search.Focus();
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "No transfer was saved.",
                            "لم يتم حفظ أي نقل.",
                            captionEn: "Error",
                            captionAr: "خطأ");
                    }

                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
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
            groupsDataGridView.ColumnCount = 2;
            int xLocation = txt_groups.Location.X;
            int yLocation = txt_groups.Location.Y + 45;

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
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
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
            categoriesDataGridView.ColumnCount = 2;
            int xLocation = txt_categories.Location.X;
            int yLocation = txt_categories.Location.Y + 45;

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
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
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
                using (BusyScope.Show(this, UiMessages.T("Loading locations...", "جاري تحميل المواقع...")))
                {
                    GetMAXInvoiceNo();
                    get_from_locations_dropdownlist();
                    get_to_locations_dropdownlist();
                }
            }
        }

        private void cmb_edit_pro_loc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab == tabControl1.TabPages["edit_desc"])//your specific tabname
                {
                    // reserved for future
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                throw;
            }

        }

        private void btn_products_print_Click(object sender, EventArgs e)
        {
            if (grid_search_products.Rows.Count > 0)
            {
                if (invoiceSearch_dt.Rows.Count > 0)
                {
                    pos.Reports.Products.frm_products_report frm_product_obj = new Reports.Products.frm_products_report(invoiceSearch_dt, false, cmb_from_pro_loc.SelectedValue.ToString());
                    frm_product_obj.Show();

                }
                else if (products_dt.Rows.Count > 0)
                {
                    pos.Reports.Products.frm_products_report frm_product_obj = new Reports.Products.frm_products_report(products_dt, false, cmb_from_pro_loc.SelectedValue.ToString());
                    frm_product_obj.Show();
                }
            }
            else
            {
                UiMessages.ShowWarning(
                    "No rows to print.",
                    "لا توجد صفوف للطباعة.",
                    captionEn: "Print",
                    captionAr: "طباعة");
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
                        string id = myProductView["id"].ToString();
                        string code = myProductView["code"].ToString();
                        string name = myProductView["name"].ToString();
                        string name_ar = "";
                        string qty = Math.Round(Convert.ToDecimal(myProductView["quantity"]), 2).ToString();
                        string cost_price = Math.Round(Convert.ToDecimal(myProductView["cost_price"]), 2).ToString();
                        string unit_price = Math.Round(Convert.ToDecimal(myProductView["unit_price"]), 2).ToString();
                        string description = myProductView["description"].ToString();
                        string location_code = myProductView["location_code"].ToString();
                        string category = myProductView["category"].ToString();
                        string item_type = "";
                        string item_number = myProductView["item_number"].ToString();


                        string[] row0 = { id, code, name, name_ar, qty, cost_price, unit_price, description, location_code, category, item_type, item_number };

                        grid_search_products.Rows.Add(row0);

                    }

                }
                else
                {
                    UiMessages.ShowInfo(
                        "No products found for this invoice.",
                        "لا توجد أصناف لهذه الفاتورة.",
                        captionEn: "Invoice",
                        captionAr: "فاتورة");
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
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
            using (BusyScope.Show(this, UiMessages.T("Loading invoice...", "جاري تحميل الفاتورة...")))
            {
                try
                {
                    string invoice_no = (txt_purchase_inv_no.Text ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(invoice_no))
                    {
                        UiMessages.ShowWarning(
                            "Please enter an invoice number.",
                            "يرجى إدخال رقم الفاتورة.",
                            captionEn: "Invoice",
                            captionAr: "فاتورة");
                        return;
                    }

                    PurchasesBLL purchasesObj = new PurchasesBLL();

                    invoiceSearch_dt = purchasesObj.GetAllPurchaseByInvoice(invoice_no);
                    Load_products_to_grid_by_invoiceno(invoiceSearch_dt);
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
            }
        }


    }

}
