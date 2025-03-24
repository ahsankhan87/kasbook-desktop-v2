namespace pos
{
    partial class frm_bulk_edit_product
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_bulk_edit_product));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.grid_search_products = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name_ar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.location_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_search_purchase_invoice = new System.Windows.Forms.Button();
            this.txt_purchase_inv_no = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_total_rows = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmb_edit_pro_loc = new System.Windows.Forms.ComboBox();
            this.txt_group_code = new System.Windows.Forms.TextBox();
            this.txt_category_code = new System.Windows.Forms.TextBox();
            this.txt_groups = new System.Windows.Forms.TextBox();
            this.txt_categories = new System.Windows.Forms.TextBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_update = new System.Windows.Forms.Button();
            this.btn_products_print = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.edit_desc = new System.Windows.Forms.TabPage();
            this.loc_transfer = new System.Windows.Forms.TabPage();
            this.grid_loc_transfer = new System.Windows.Forms.DataGridView();
            this.product_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_name_ar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.transfer_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmb_from_locations = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_to_locations = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txt_date = new System.Windows.Forms.DateTimePicker();
            this.txt_ref_no = new System.Windows.Forms.TextBox();
            this.rb_by_name_trans = new System.Windows.Forms.RadioButton();
            this.rb_by_code_trans = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_transfer_search = new System.Windows.Forms.TextBox();
            this.btn_transfer = new System.Windows.Forms.Button();
            this.btn_transfer_search = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_products)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.edit_desc.SuspendLayout();
            this.loc_transfer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_loc_transfer)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_search
            // 
            resources.ApplyResources(this.txt_search, "txt_search");
            this.txt_search.Name = "txt_search";
            // 
            // btn_search
            // 
            resources.ApplyResources(this.btn_search, "btn_search");
            this.btn_search.Name = "btn_search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // grid_search_products
            // 
            resources.ApplyResources(this.grid_search_products, "grid_search_products");
            this.grid_search_products.AllowUserToAddRows = false;
            this.grid_search_products.AllowUserToDeleteRows = false;
            this.grid_search_products.AllowUserToOrderColumns = true;
            this.grid_search_products.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_search_products.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.code,
            this.name,
            this.name_ar,
            this.qty,
            this.cost_price,
            this.unit_price,
            this.description,
            this.location_code,
            this.category,
            this.item_type});
            this.grid_search_products.Name = "grid_search_products";
            this.grid_search_products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            // 
            // code
            // 
            this.code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.code.DataPropertyName = "code";
            resources.ApplyResources(this.code, "code");
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.DataPropertyName = "name";
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            // 
            // name_ar
            // 
            this.name_ar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.name_ar.DataPropertyName = "name_ar";
            resources.ApplyResources(this.name_ar, "name_ar");
            this.name_ar.Name = "name_ar";
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qty.DataPropertyName = "qty";
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.qty.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.qty, "qty");
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            // 
            // cost_price
            // 
            this.cost_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cost_price.DataPropertyName = "avg_cost";
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.cost_price.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.cost_price, "cost_price");
            this.cost_price.Name = "cost_price";
            this.cost_price.ReadOnly = true;
            // 
            // unit_price
            // 
            this.unit_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.unit_price.DataPropertyName = "unit_price";
            resources.ApplyResources(this.unit_price, "unit_price");
            this.unit_price.Name = "unit_price";
            // 
            // description
            // 
            this.description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.description.DataPropertyName = "description";
            resources.ApplyResources(this.description, "description");
            this.description.Name = "description";
            // 
            // location_code
            // 
            this.location_code.DataPropertyName = "location_code";
            resources.ApplyResources(this.location_code, "location_code");
            this.location_code.Name = "location_code";
            // 
            // category
            // 
            this.category.DataPropertyName = "category";
            resources.ApplyResources(this.category, "category");
            this.category.Name = "category";
            // 
            // item_type
            // 
            this.item_type.DataPropertyName = "item_type";
            resources.ApplyResources(this.item_type, "item_type");
            this.item_type.Name = "item_type";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.btn_search_purchase_invoice);
            this.panel1.Controls.Add(this.txt_purchase_inv_no);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lbl_total_rows);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.cmb_edit_pro_loc);
            this.panel1.Controls.Add(this.txt_group_code);
            this.panel1.Controls.Add(this.txt_category_code);
            this.panel1.Controls.Add(this.txt_groups);
            this.panel1.Controls.Add(this.txt_categories);
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_close);
            this.panel1.Controls.Add(this.btn_update);
            this.panel1.Controls.Add(this.btn_products_print);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Name = "panel1";
            // 
            // btn_search_purchase_invoice
            // 
            resources.ApplyResources(this.btn_search_purchase_invoice, "btn_search_purchase_invoice");
            this.btn_search_purchase_invoice.Name = "btn_search_purchase_invoice";
            this.btn_search_purchase_invoice.UseVisualStyleBackColor = true;
            this.btn_search_purchase_invoice.Click += new System.EventHandler(this.btn_search_purchase_invoice_Click);
            // 
            // txt_purchase_inv_no
            // 
            resources.ApplyResources(this.txt_purchase_inv_no, "txt_purchase_inv_no");
            this.txt_purchase_inv_no.Name = "txt_purchase_inv_no";
            this.txt_purchase_inv_no.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_purchase_inv_no_KeyDown);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lbl_total_rows
            // 
            resources.ApplyResources(this.lbl_total_rows, "lbl_total_rows");
            this.lbl_total_rows.Name = "lbl_total_rows";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cmb_edit_pro_loc
            // 
            resources.ApplyResources(this.cmb_edit_pro_loc, "cmb_edit_pro_loc");
            this.cmb_edit_pro_loc.FormattingEnabled = true;
            this.cmb_edit_pro_loc.Name = "cmb_edit_pro_loc";
            this.cmb_edit_pro_loc.SelectedIndexChanged += new System.EventHandler(this.cmb_edit_pro_loc_SelectedIndexChanged);
            // 
            // txt_group_code
            // 
            resources.ApplyResources(this.txt_group_code, "txt_group_code");
            this.txt_group_code.Name = "txt_group_code";
            // 
            // txt_category_code
            // 
            resources.ApplyResources(this.txt_category_code, "txt_category_code");
            this.txt_category_code.Name = "txt_category_code";
            // 
            // txt_groups
            // 
            resources.ApplyResources(this.txt_groups, "txt_groups");
            this.txt_groups.Name = "txt_groups";
            this.txt_groups.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_groups_KeyUp);
            this.txt_groups.Leave += new System.EventHandler(this.txt_groups_Leave);
            // 
            // txt_categories
            // 
            resources.ApplyResources(this.txt_categories, "txt_categories");
            this.txt_categories.Name = "txt_categories";
            this.txt_categories.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_categories_KeyUp);
            this.txt_categories.Leave += new System.EventHandler(this.txt_categories_Leave);
            // 
            // btn_close
            // 
            resources.ApplyResources(this.btn_close, "btn_close");
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.Name = "btn_close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_update
            // 
            resources.ApplyResources(this.btn_update, "btn_update");
            this.btn_update.Name = "btn_update";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // btn_products_print
            // 
            resources.ApplyResources(this.btn_products_print, "btn_products_print");
            this.btn_products_print.Name = "btn_products_print";
            this.btn_products_print.UseVisualStyleBackColor = true;
            this.btn_products_print.Click += new System.EventHandler(this.btn_products_print_Click);
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.edit_desc);
            this.tabControl1.Controls.Add(this.loc_transfer);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // edit_desc
            // 
            resources.ApplyResources(this.edit_desc, "edit_desc");
            this.edit_desc.Controls.Add(this.grid_search_products);
            this.edit_desc.Controls.Add(this.panel1);
            this.edit_desc.Name = "edit_desc";
            this.edit_desc.UseVisualStyleBackColor = true;
            // 
            // loc_transfer
            // 
            resources.ApplyResources(this.loc_transfer, "loc_transfer");
            this.loc_transfer.Controls.Add(this.grid_loc_transfer);
            this.loc_transfer.Controls.Add(this.panel2);
            this.loc_transfer.Name = "loc_transfer";
            this.loc_transfer.UseVisualStyleBackColor = true;
            // 
            // grid_loc_transfer
            // 
            resources.ApplyResources(this.grid_loc_transfer, "grid_loc_transfer");
            this.grid_loc_transfer.AllowUserToAddRows = false;
            this.grid_loc_transfer.AllowUserToDeleteRows = false;
            this.grid_loc_transfer.AllowUserToOrderColumns = true;
            this.grid_loc_transfer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_loc_transfer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product_id,
            this.product_code,
            this.product_name,
            this.product_name_ar,
            this.product_qty,
            this.transfer_qty});
            this.grid_loc_transfer.Name = "grid_loc_transfer";
            this.grid_loc_transfer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // product_id
            // 
            this.product_id.DataPropertyName = "id";
            resources.ApplyResources(this.product_id, "product_id");
            this.product_id.Name = "product_id";
            // 
            // product_code
            // 
            this.product_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.product_code.DataPropertyName = "code";
            resources.ApplyResources(this.product_code, "product_code");
            this.product_code.Name = "product_code";
            this.product_code.ReadOnly = true;
            // 
            // product_name
            // 
            this.product_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.product_name.DataPropertyName = "name";
            resources.ApplyResources(this.product_name, "product_name");
            this.product_name.Name = "product_name";
            this.product_name.ReadOnly = true;
            // 
            // product_name_ar
            // 
            this.product_name_ar.DataPropertyName = "name_ar";
            resources.ApplyResources(this.product_name_ar, "product_name_ar");
            this.product_name_ar.Name = "product_name_ar";
            // 
            // product_qty
            // 
            this.product_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.product_qty.DataPropertyName = "qty";
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            this.product_qty.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.product_qty, "product_qty");
            this.product_qty.Name = "product_qty";
            // 
            // transfer_qty
            // 
            this.transfer_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.transfer_qty.DataPropertyName = "transfer_qty";
            resources.ApplyResources(this.transfer_qty, "transfer_qty");
            this.transfer_qty.Name = "transfer_qty";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.cmb_from_locations);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.cmb_to_locations);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.txt_date);
            this.panel2.Controls.Add(this.txt_ref_no);
            this.panel2.Controls.Add(this.rb_by_name_trans);
            this.panel2.Controls.Add(this.rb_by_code_trans);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txt_transfer_search);
            this.panel2.Controls.Add(this.btn_transfer);
            this.panel2.Controls.Add(this.btn_transfer_search);
            this.panel2.Name = "panel2";
            // 
            // cmb_from_locations
            // 
            resources.ApplyResources(this.cmb_from_locations, "cmb_from_locations");
            this.cmb_from_locations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_from_locations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_from_locations.FormattingEnabled = true;
            this.cmb_from_locations.Name = "cmb_from_locations";
            this.cmb_from_locations.SelectedIndexChanged += new System.EventHandler(this.cmb_from_locations_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmb_to_locations
            // 
            resources.ApplyResources(this.cmb_to_locations, "cmb_to_locations");
            this.cmb_to_locations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_to_locations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_to_locations.FormattingEnabled = true;
            this.cmb_to_locations.Name = "cmb_to_locations";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // txt_date
            // 
            resources.ApplyResources(this.txt_date, "txt_date");
            this.txt_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_date.Name = "txt_date";
            // 
            // txt_ref_no
            // 
            resources.ApplyResources(this.txt_ref_no, "txt_ref_no");
            this.txt_ref_no.Name = "txt_ref_no";
            this.txt_ref_no.ReadOnly = true;
            // 
            // rb_by_name_trans
            // 
            resources.ApplyResources(this.rb_by_name_trans, "rb_by_name_trans");
            this.rb_by_name_trans.Name = "rb_by_name_trans";
            this.rb_by_name_trans.UseVisualStyleBackColor = true;
            // 
            // rb_by_code_trans
            // 
            resources.ApplyResources(this.rb_by_code_trans, "rb_by_code_trans");
            this.rb_by_code_trans.Checked = true;
            this.rb_by_code_trans.Name = "rb_by_code_trans";
            this.rb_by_code_trans.TabStop = true;
            this.rb_by_code_trans.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txt_transfer_search
            // 
            resources.ApplyResources(this.txt_transfer_search, "txt_transfer_search");
            this.txt_transfer_search.Name = "txt_transfer_search";
            // 
            // btn_transfer
            // 
            resources.ApplyResources(this.btn_transfer, "btn_transfer");
            this.btn_transfer.Name = "btn_transfer";
            this.btn_transfer.UseVisualStyleBackColor = true;
            this.btn_transfer.Click += new System.EventHandler(this.btn_transfer_Click);
            // 
            // btn_transfer_search
            // 
            resources.ApplyResources(this.btn_transfer_search, "btn_transfer_search");
            this.btn_transfer_search.Name = "btn_transfer_search";
            this.btn_transfer_search.UseVisualStyleBackColor = true;
            this.btn_transfer_search.Click += new System.EventHandler(this.btn_transfer_search_Click);
            // 
            // frm_bulk_edit_product
            // 
            this.AcceptButton = this.btn_search;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_close;
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Name = "frm_bulk_edit_product";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_bulk_edit_product_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_bulk_edit_product_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_products)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.edit_desc.ResumeLayout(false);
            this.loc_transfer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_loc_transfer)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.DataGridView grid_search_products;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage edit_desc;
        private System.Windows.Forms.TabPage loc_transfer;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cmb_from_locations;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb_to_locations;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker txt_date;
        private System.Windows.Forms.TextBox txt_ref_no;
        private System.Windows.Forms.RadioButton rb_by_name_trans;
        private System.Windows.Forms.RadioButton rb_by_code_trans;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_transfer_search;
        private System.Windows.Forms.Button btn_transfer;
        private System.Windows.Forms.Button btn_transfer_search;
        private System.Windows.Forms.DataGridView grid_loc_transfer;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_name_ar;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn transfer_qty;
        private System.Windows.Forms.TextBox txt_category_code;
        private System.Windows.Forms.TextBox txt_groups;
        private System.Windows.Forms.TextBox txt_group_code;
        private System.Windows.Forms.ComboBox cmb_edit_pro_loc;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_categories;
        private System.Windows.Forms.Label lbl_total_rows;
        private System.Windows.Forms.Button btn_products_print;
        private System.Windows.Forms.TextBox txt_purchase_inv_no;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_search_purchase_invoice;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn name_ar;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn location_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_type;
    }
}