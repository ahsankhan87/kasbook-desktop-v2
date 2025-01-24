namespace pos
{
    partial class frm_addProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_addProduct));
            this.txt_barcode = new System.Windows.Forms.TextBox();
            this.lbl_barcode = new System.Windows.Forms.Label();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_header_title = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmb_item_type = new System.Windows.Forms.ComboBox();
            this.txt_code = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmb_locations = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmb_categories = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmb_brands = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cmb_units = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmb_groups = new System.Windows.Forms.ComboBox();
            this.lbl_errors = new System.Windows.Forms.Label();
            this.txt_pur_dmnd_qty = new System.Windows.Forms.TextBox();
            this.txt_restock_level = new System.Windows.Forms.TextBox();
            this.txt_category_code = new System.Windows.Forms.TextBox();
            this.txt_demand_qty = new System.Windows.Forms.TextBox();
            this.txt_sale_dmnd_qty = new System.Windows.Forms.TextBox();
            this.txt_description = new System.Windows.Forms.TextBox();
            this.cmb_tax = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_cost_price = new System.Windows.Forms.TextBox();
            this.btn_save = new System.Windows.Forms.Button();
            this.txt_unit_price_2 = new System.Windows.Forms.TextBox();
            this.txt_unit_price = new System.Windows.Forms.TextBox();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.txt_item_number = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txt_brand_code = new System.Windows.Forms.TextBox();
            this.txt_name_ar = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.grid_products = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.copy = new System.Windows.Forms.DataGridViewImageColumn();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_upload_picture = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_products)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_barcode
            // 
            resources.ApplyResources(this.txt_barcode, "txt_barcode");
            this.txt_barcode.Name = "txt_barcode";
            // 
            // lbl_barcode
            // 
            resources.ApplyResources(this.lbl_barcode, "lbl_barcode");
            this.lbl_barcode.Name = "lbl_barcode";
            // 
            // txt_name
            // 
            resources.ApplyResources(this.txt_name, "txt_name");
            this.txt_name.Name = "txt_name";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lbl_header_title
            // 
            resources.ApplyResources(this.lbl_header_title, "lbl_header_title");
            this.lbl_header_title.ForeColor = System.Drawing.Color.White;
            this.lbl_header_title.Name = "lbl_header_title";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel1.Controls.Add(this.lbl_header_title);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.ForeColor = System.Drawing.Color.Coral;
            this.panel1.Name = "panel1";
            // 
            // txt_id
            // 
            resources.ApplyResources(this.txt_id, "txt_id");
            this.txt_id.Name = "txt_id";
            this.txt_id.ReadOnly = true;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // cmb_item_type
            // 
            this.cmb_item_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmb_item_type, "cmb_item_type");
            this.cmb_item_type.FormattingEnabled = true;
            this.cmb_item_type.Items.AddRange(new object[] {
            resources.GetString("cmb_item_type.Items"),
            resources.GetString("cmb_item_type.Items1")});
            this.cmb_item_type.Name = "cmb_item_type";
            // 
            // txt_code
            // 
            resources.ApplyResources(this.txt_code, "txt_code");
            this.txt_code.Name = "txt_code";
            this.txt_code.ReadOnly = true;
            this.txt_code.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_code_KeyDown);
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
            // cmb_locations
            // 
            this.cmb_locations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_locations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cmb_locations, "cmb_locations");
            this.cmb_locations.FormattingEnabled = true;
            this.cmb_locations.Items.AddRange(new object[] {
            resources.GetString("cmb_locations.Items"),
            resources.GetString("cmb_locations.Items1")});
            this.cmb_locations.Name = "cmb_locations";
            this.cmb_locations.SelectedIndexChanged += new System.EventHandler(this.cmb_locations_SelectedIndexChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // cmb_categories
            // 
            this.cmb_categories.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_categories.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cmb_categories, "cmb_categories");
            this.cmb_categories.FormattingEnabled = true;
            this.cmb_categories.Items.AddRange(new object[] {
            resources.GetString("cmb_categories.Items"),
            resources.GetString("cmb_categories.Items1")});
            this.cmb_categories.Name = "cmb_categories";
            this.cmb_categories.SelectedIndexChanged += new System.EventHandler(this.cmb_categories_SelectedIndexChanged);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // cmb_brands
            // 
            this.cmb_brands.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_brands.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cmb_brands, "cmb_brands");
            this.cmb_brands.FormattingEnabled = true;
            this.cmb_brands.Name = "cmb_brands";
            this.cmb_brands.SelectedIndexChanged += new System.EventHandler(this.cmb_brands_SelectedIndexChanged);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // cmb_units
            // 
            this.cmb_units.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_units.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cmb_units, "cmb_units");
            this.cmb_units.FormattingEnabled = true;
            this.cmb_units.Items.AddRange(new object[] {
            resources.GetString("cmb_units.Items"),
            resources.GetString("cmb_units.Items1")});
            this.cmb_units.Name = "cmb_units";
            this.cmb_units.SelectedIndexChanged += new System.EventHandler(this.cmb_units_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.cmb_groups);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.lbl_errors);
            this.groupBox1.Controls.Add(this.txt_pur_dmnd_qty);
            this.groupBox1.Controls.Add(this.txt_restock_level);
            this.groupBox1.Controls.Add(this.txt_category_code);
            this.groupBox1.Controls.Add(this.cmb_categories);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txt_demand_qty);
            this.groupBox1.Controls.Add(this.txt_sale_dmnd_qty);
            this.groupBox1.Controls.Add(this.txt_description);
            this.groupBox1.Controls.Add(this.cmb_tax);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txt_cost_price);
            this.groupBox1.Controls.Add(this.btn_save);
            this.groupBox1.Controls.Add(this.txt_unit_price_2);
            this.groupBox1.Controls.Add(this.txt_unit_price);
            this.groupBox1.Controls.Add(this.btn_cancel);
            this.groupBox1.Controls.Add(this.txt_item_number);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.cmb_brands);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.txt_brand_code);
            this.groupBox1.Controls.Add(this.txt_barcode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txt_code);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txt_id);
            this.groupBox1.Controls.Add(this.cmb_locations);
            this.groupBox1.Controls.Add(this.txt_name_ar);
            this.groupBox1.Controls.Add(this.txt_name);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmb_units);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmb_item_type);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lbl_barcode);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // cmb_groups
            // 
            this.cmb_groups.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_groups.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            resources.ApplyResources(this.cmb_groups, "cmb_groups");
            this.cmb_groups.FormattingEnabled = true;
            this.cmb_groups.Items.AddRange(new object[] {
            resources.GetString("cmb_groups.Items"),
            resources.GetString("cmb_groups.Items1")});
            this.cmb_groups.Name = "cmb_groups";
            // 
            // lbl_errors
            // 
            this.lbl_errors.ForeColor = System.Drawing.Color.Red;
            resources.ApplyResources(this.lbl_errors, "lbl_errors");
            this.lbl_errors.Name = "lbl_errors";
            // 
            // txt_pur_dmnd_qty
            // 
            resources.ApplyResources(this.txt_pur_dmnd_qty, "txt_pur_dmnd_qty");
            this.txt_pur_dmnd_qty.Name = "txt_pur_dmnd_qty";
            // 
            // txt_restock_level
            // 
            resources.ApplyResources(this.txt_restock_level, "txt_restock_level");
            this.txt_restock_level.Name = "txt_restock_level";
            // 
            // txt_category_code
            // 
            resources.ApplyResources(this.txt_category_code, "txt_category_code");
            this.txt_category_code.Name = "txt_category_code";
            this.txt_category_code.ReadOnly = true;
            // 
            // txt_demand_qty
            // 
            resources.ApplyResources(this.txt_demand_qty, "txt_demand_qty");
            this.txt_demand_qty.Name = "txt_demand_qty";
            // 
            // txt_sale_dmnd_qty
            // 
            resources.ApplyResources(this.txt_sale_dmnd_qty, "txt_sale_dmnd_qty");
            this.txt_sale_dmnd_qty.Name = "txt_sale_dmnd_qty";
            // 
            // txt_description
            // 
            resources.ApplyResources(this.txt_description, "txt_description");
            this.txt_description.Name = "txt_description";
            // 
            // cmb_tax
            // 
            this.cmb_tax.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_tax.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_tax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmb_tax, "cmb_tax");
            this.cmb_tax.FormattingEnabled = true;
            this.cmb_tax.Items.AddRange(new object[] {
            resources.GetString("cmb_tax.Items"),
            resources.GetString("cmb_tax.Items1")});
            this.cmb_tax.Name = "cmb_tax";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txt_cost_price
            // 
            resources.ApplyResources(this.txt_cost_price, "txt_cost_price");
            this.txt_cost_price.Name = "txt_cost_price";
            // 
            // btn_save
            // 
            resources.ApplyResources(this.btn_save, "btn_save");
            this.btn_save.Name = "btn_save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // txt_unit_price_2
            // 
            resources.ApplyResources(this.txt_unit_price_2, "txt_unit_price_2");
            this.txt_unit_price_2.Name = "txt_unit_price_2";
            // 
            // txt_unit_price
            // 
            resources.ApplyResources(this.txt_unit_price, "txt_unit_price");
            this.txt_unit_price.Name = "txt_unit_price";
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            // 
            // txt_item_number
            // 
            resources.ApplyResources(this.txt_item_number, "txt_item_number");
            this.txt_item_number.Name = "txt_item_number";
            this.txt_item_number.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_item_number_KeyUp);
            this.txt_item_number.Leave += new System.EventHandler(this.txt_item_number_Leave);
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // txt_brand_code
            // 
            resources.ApplyResources(this.txt_brand_code, "txt_brand_code");
            this.txt_brand_code.Name = "txt_brand_code";
            this.txt_brand_code.ReadOnly = true;
            // 
            // txt_name_ar
            // 
            resources.ApplyResources(this.txt_name_ar, "txt_name_ar");
            this.txt_name_ar.Name = "txt_name_ar";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // grid_products
            // 
            this.grid_products.AllowUserToAddRows = false;
            this.grid_products.AllowUserToDeleteRows = false;
            this.grid_products.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.grid_products, "grid_products");
            this.grid_products.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_products.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.code,
            this.name,
            this.cost_price,
            this.unit_price,
            this.copy});
            this.grid_products.Name = "grid_products";
            this.grid_products.ReadOnly = true;
            this.grid_products.RowTemplate.Height = 28;
            this.grid_products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_products.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_products_CellContentClick);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // code
            // 
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
            this.name.ReadOnly = true;
            // 
            // cost_price
            // 
            this.cost_price.DataPropertyName = "cost_price";
            resources.ApplyResources(this.cost_price, "cost_price");
            this.cost_price.Name = "cost_price";
            this.cost_price.ReadOnly = true;
            // 
            // unit_price
            // 
            this.unit_price.DataPropertyName = "unit_price";
            resources.ApplyResources(this.unit_price, "unit_price");
            this.unit_price.Name = "unit_price";
            this.unit_price.ReadOnly = true;
            // 
            // copy
            // 
            this.copy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.copy, "copy");
            this.copy.Name = "copy";
            this.copy.ReadOnly = true;
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // btn_upload_picture
            // 
            resources.ApplyResources(this.btn_upload_picture, "btn_upload_picture");
            this.btn_upload_picture.Name = "btn_upload_picture";
            this.btn_upload_picture.UseVisualStyleBackColor = true;
            this.btn_upload_picture.Click += new System.EventHandler(this.btn_upload_picture_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.btn_upload_picture);
            this.panel2.Controls.Add(this.grid_products);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Name = "panel2";
            // 
            // frm_addProduct
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.Name = "frm_addProduct";
            this.Load += new System.EventHandler(this.frm_addProduct_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_addProduct_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_products)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_barcode;
        private System.Windows.Forms.Label lbl_barcode;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_header_title;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmb_item_type;
        private System.Windows.Forms.TextBox txt_code;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmb_locations;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmb_categories;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmb_brands;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cmb_units;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_category_code;
        private System.Windows.Forms.TextBox txt_name_ar;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txt_brand_code;
        private System.Windows.Forms.ComboBox cmb_groups;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txt_item_number;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lbl_errors;
        private System.Windows.Forms.TextBox txt_description;
        private System.Windows.Forms.ComboBox cmb_tax;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_cost_price;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.TextBox txt_unit_price_2;
        private System.Windows.Forms.TextBox txt_unit_price;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.TextBox txt_pur_dmnd_qty;
        private System.Windows.Forms.TextBox txt_restock_level;
        private System.Windows.Forms.TextBox txt_demand_qty;
        private System.Windows.Forms.TextBox txt_sale_dmnd_qty;
        private System.Windows.Forms.DataGridView grid_products;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewImageColumn copy;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_upload_picture;
        private System.Windows.Forms.Panel panel2;
    }
}