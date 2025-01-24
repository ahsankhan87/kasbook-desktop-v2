namespace pos
{
    partial class frm_purchases_v1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_purchases_v1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel_header = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_new = new System.Windows.Forms.Button();
            this.btn_movements = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txt_barcode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_search_purchase_invoices = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label7 = new System.Windows.Forms.Label();
            this.cmb_employees = new System.Windows.Forms.ComboBox();
            this.txt_purchase_date = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.cmb_purchase_type = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_invoice_no = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_supplier_invoice = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmb_suppliers = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txt_description = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.contextMenuStrip_purchases = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productMovementF4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.grid_purchases = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.packing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avg_cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sub_total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.location_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tax_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tax_rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.packet_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationsdtDataTableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel_grid = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rd_btn_bytotal_price = new System.Windows.Forms.RadioButton();
            this.rd_btn_by_unitprice = new System.Windows.Forms.RadioButton();
            this.rd_btn_with_vat = new System.Windows.Forms.RadioButton();
            this.rd_btn_without_vat = new System.Windows.Forms.RadioButton();
            this.txt_total_amount = new System.Windows.Forms.TextBox();
            this.txt_total_tax = new System.Windows.Forms.TextBox();
            this.txt_total_discount = new System.Windows.Forms.TextBox();
            this.txt_sub_total = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel_header.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip_purchases.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_purchases)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationsdtDataTableBindingSource)).BeginInit();
            this.panel_grid.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_header
            // 
            this.panel_header.Controls.Add(this.groupBox5);
            this.panel_header.Controls.Add(this.groupBox4);
            this.panel_header.Controls.Add(this.groupBox1);
            this.panel_header.Controls.Add(this.txt_description);
            this.panel_header.Controls.Add(this.label17);
            resources.ApplyResources(this.panel_header, "panel_header");
            this.panel_header.Name = "panel_header";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.btn_save);
            this.groupBox5.Controls.Add(this.btn_new);
            this.groupBox5.Controls.Add(this.btn_movements);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Name = "label4";
            // 
            // btn_save
            // 
            resources.ApplyResources(this.btn_save, "btn_save");
            this.btn_save.Name = "btn_save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_new
            // 
            resources.ApplyResources(this.btn_new, "btn_new");
            this.btn_new.Name = "btn_new";
            this.btn_new.UseVisualStyleBackColor = true;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // btn_movements
            // 
            resources.ApplyResources(this.btn_movements, "btn_movements");
            this.btn_movements.Name = "btn_movements";
            this.btn_movements.UseVisualStyleBackColor = true;
            this.btn_movements.Click += new System.EventHandler(this.btn_movements_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txt_barcode);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.btn_search_purchase_invoices);
            this.groupBox4.Controls.Add(this.linkLabel1);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.cmb_employees);
            this.groupBox4.Controls.Add(this.txt_purchase_date);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.cmb_purchase_type);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.txt_invoice_no);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // txt_barcode
            // 
            resources.ApplyResources(this.txt_barcode, "txt_barcode");
            this.txt_barcode.Name = "txt_barcode";
            this.txt_barcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_barcode_KeyDown);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // btn_search_purchase_invoices
            // 
            resources.ApplyResources(this.btn_search_purchase_invoices, "btn_search_purchase_invoices");
            this.btn_search_purchase_invoices.Name = "btn_search_purchase_invoices";
            this.btn_search_purchase_invoices.UseVisualStyleBackColor = true;
            this.btn_search_purchase_invoices.Click += new System.EventHandler(this.btn_search_purchase_invoices_Click);
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmb_employees
            // 
            this.cmb_employees.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_employees.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_employees.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_employees, "cmb_employees");
            this.cmb_employees.Name = "cmb_employees";
            // 
            // txt_purchase_date
            // 
            resources.ApplyResources(this.txt_purchase_date, "txt_purchase_date");
            this.txt_purchase_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_purchase_date.Name = "txt_purchase_date";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmb_purchase_type
            // 
            this.cmb_purchase_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_purchase_type.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_purchase_type, "cmb_purchase_type");
            this.cmb_purchase_type.Name = "cmb_purchase_type";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // txt_invoice_no
            // 
            resources.ApplyResources(this.txt_invoice_no, "txt_invoice_no");
            this.txt_invoice_no.Name = "txt_invoice_no";
            this.txt_invoice_no.ReadOnly = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.txt_supplier_invoice);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmb_suppliers);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // txt_supplier_invoice
            // 
            resources.ApplyResources(this.txt_supplier_invoice, "txt_supplier_invoice");
            this.txt_supplier_invoice.Name = "txt_supplier_invoice";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cmb_suppliers
            // 
            this.cmb_suppliers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_suppliers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_suppliers.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_suppliers, "cmb_suppliers");
            this.cmb_suppliers.Name = "cmb_suppliers";
            this.cmb_suppliers.SelectedIndexChanged += new System.EventHandler(this.cmb_suppliers_SelectedIndexChanged);
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // txt_description
            // 
            resources.ApplyResources(this.txt_description, "txt_description");
            this.txt_description.Name = "txt_description";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // contextMenuStrip_purchases
            // 
            this.contextMenuStrip_purchases.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip_purchases.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewRowToolStripMenuItem,
            this.productMovementF4ToolStripMenuItem});
            this.contextMenuStrip_purchases.Name = "contextMenuStrip_purchases";
            resources.ApplyResources(this.contextMenuStrip_purchases, "contextMenuStrip_purchases");
            // 
            // addNewRowToolStripMenuItem
            // 
            this.addNewRowToolStripMenuItem.Name = "addNewRowToolStripMenuItem";
            resources.ApplyResources(this.addNewRowToolStripMenuItem, "addNewRowToolStripMenuItem");
            this.addNewRowToolStripMenuItem.Click += new System.EventHandler(this.addNewRowToolStripMenuItem_Click);
            // 
            // productMovementF4ToolStripMenuItem
            // 
            this.productMovementF4ToolStripMenuItem.Name = "productMovementF4ToolStripMenuItem";
            resources.ApplyResources(this.productMovementF4ToolStripMenuItem, "productMovementF4ToolStripMenuItem");
            this.productMovementF4ToolStripMenuItem.Click += new System.EventHandler(this.productMovementF4ToolStripMenuItem_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.dataGridViewImageColumn1, "dataGridViewImageColumn1");
            this.dataGridViewImageColumn1.Image = global::pos.Properties.Resources.delete;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // grid_purchases
            // 
            this.grid_purchases.AllowUserToAddRows = false;
            resources.ApplyResources(this.grid_purchases, "grid_purchases");
            this.grid_purchases.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_purchases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_purchases.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.code,
            this.name,
            this.packing,
            this.Qty,
            this.avg_cost,
            this.unit_price,
            this.discount,
            this.tax,
            this.sub_total,
            this.location_code,
            this.unit,
            this.category,
            this.btn_delete,
            this.tax_id,
            this.tax_rate,
            this.packet_qty,
            this.Column1});
            this.grid_purchases.ContextMenuStrip = this.contextMenuStrip_purchases;
            this.grid_purchases.Name = "grid_purchases";
            this.grid_purchases.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grid_purchases.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_purchases_CellContentClick);
            this.grid_purchases.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_purchases_CellEndEdit);
            this.grid_purchases.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grid_purchases_EditingControlShowing);
            this.grid_purchases.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.grid_purchases_RowPostPaint);
            this.grid_purchases.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_purchases_KeyDown);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            // 
            // code
            // 
            this.code.DataPropertyName = "code";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.code.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.code, "code");
            this.code.Name = "code";
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // packing
            // 
            this.packing.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.packing, "packing");
            this.packing.Name = "packing";
            // 
            // Qty
            // 
            this.Qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Qty.DataPropertyName = "qty";
            resources.ApplyResources(this.Qty, "Qty");
            this.Qty.Name = "Qty";
            // 
            // avg_cost
            // 
            this.avg_cost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.avg_cost.DataPropertyName = "avg_cost";
            resources.ApplyResources(this.avg_cost, "avg_cost");
            this.avg_cost.Name = "avg_cost";
            // 
            // unit_price
            // 
            this.unit_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.unit_price.DataPropertyName = "unit_price";
            resources.ApplyResources(this.unit_price, "unit_price");
            this.unit_price.Name = "unit_price";
            // 
            // discount
            // 
            this.discount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.discount, "discount");
            this.discount.Name = "discount";
            // 
            // tax
            // 
            this.tax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.tax.DataPropertyName = "tax";
            resources.ApplyResources(this.tax, "tax");
            this.tax.Name = "tax";
            this.tax.ReadOnly = true;
            // 
            // sub_total
            // 
            this.sub_total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.sub_total, "sub_total");
            this.sub_total.Name = "sub_total";
            this.sub_total.ReadOnly = true;
            // 
            // location_code
            // 
            this.location_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.location_code.DataPropertyName = "location_code";
            resources.ApplyResources(this.location_code, "location_code");
            this.location_code.Name = "location_code";
            this.location_code.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.location_code.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // unit
            // 
            this.unit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.unit.DataPropertyName = "unit";
            resources.ApplyResources(this.unit, "unit");
            this.unit.Name = "unit";
            // 
            // category
            // 
            this.category.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.category.DataPropertyName = "category";
            resources.ApplyResources(this.category, "category");
            this.category.Name = "category";
            // 
            // btn_delete
            // 
            this.btn_delete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.btn_delete, "btn_delete");
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btn_delete.Text = "Delete";
            // 
            // tax_id
            // 
            this.tax_id.DataPropertyName = "tax_id";
            resources.ApplyResources(this.tax_id, "tax_id");
            this.tax_id.Name = "tax_id";
            // 
            // tax_rate
            // 
            this.tax_rate.DataPropertyName = "tax_rate";
            resources.ApplyResources(this.tax_rate, "tax_rate");
            this.tax_rate.Name = "tax_rate";
            // 
            // packet_qty
            // 
            resources.ApplyResources(this.packet_qty, "packet_qty");
            this.packet_qty.Name = "packet_qty";
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            // 
            // panel_grid
            // 
            this.panel_grid.Controls.Add(this.grid_purchases);
            resources.ApplyResources(this.panel_grid, "panel_grid");
            this.panel_grid.Name = "panel_grid";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.txt_total_amount);
            this.panel1.Controls.Add(this.txt_total_tax);
            this.panel1.Controls.Add(this.txt_total_discount);
            this.panel1.Controls.Add(this.txt_sub_total);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label9);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.rd_btn_with_vat);
            this.groupBox2.Controls.Add(this.rd_btn_without_vat);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.rd_btn_bytotal_price);
            this.groupBox3.Controls.Add(this.rd_btn_by_unitprice);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // rd_btn_bytotal_price
            // 
            resources.ApplyResources(this.rd_btn_bytotal_price, "rd_btn_bytotal_price");
            this.rd_btn_bytotal_price.Name = "rd_btn_bytotal_price";
            this.rd_btn_bytotal_price.UseVisualStyleBackColor = true;
            // 
            // rd_btn_by_unitprice
            // 
            resources.ApplyResources(this.rd_btn_by_unitprice, "rd_btn_by_unitprice");
            this.rd_btn_by_unitprice.Checked = true;
            this.rd_btn_by_unitprice.Name = "rd_btn_by_unitprice";
            this.rd_btn_by_unitprice.TabStop = true;
            this.rd_btn_by_unitprice.UseVisualStyleBackColor = true;
            // 
            // rd_btn_with_vat
            // 
            resources.ApplyResources(this.rd_btn_with_vat, "rd_btn_with_vat");
            this.rd_btn_with_vat.Name = "rd_btn_with_vat";
            this.rd_btn_with_vat.UseVisualStyleBackColor = true;
            // 
            // rd_btn_without_vat
            // 
            resources.ApplyResources(this.rd_btn_without_vat, "rd_btn_without_vat");
            this.rd_btn_without_vat.Checked = true;
            this.rd_btn_without_vat.Name = "rd_btn_without_vat";
            this.rd_btn_without_vat.TabStop = true;
            this.rd_btn_without_vat.UseVisualStyleBackColor = true;
            // 
            // txt_total_amount
            // 
            resources.ApplyResources(this.txt_total_amount, "txt_total_amount");
            this.txt_total_amount.Name = "txt_total_amount";
            this.txt_total_amount.ReadOnly = true;
            // 
            // txt_total_tax
            // 
            resources.ApplyResources(this.txt_total_tax, "txt_total_tax");
            this.txt_total_tax.Name = "txt_total_tax";
            this.txt_total_tax.ReadOnly = true;
            // 
            // txt_total_discount
            // 
            resources.ApplyResources(this.txt_total_discount, "txt_total_discount");
            this.txt_total_discount.Name = "txt_total_discount";
            this.txt_total_discount.ReadOnly = true;
            // 
            // txt_sub_total
            // 
            resources.ApplyResources(this.txt_sub_total, "txt_sub_total");
            this.txt_sub_total.Name = "txt_sub_total";
            this.txt_sub_total.ReadOnly = true;
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // frm_purchases_v1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_grid);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel_header);
            this.KeyPreview = true;
            this.Name = "frm_purchases_v1";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_purchases_v1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_purchases_v1_KeyDown);
            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip_purchases.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_purchases)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.locationsdtDataTableBindingSource)).EndInit();
            this.panel_grid.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_header;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox cmb_suppliers;
        private System.Windows.Forms.TextBox txt_supplier_invoice;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_barcode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_purchases;
        private System.Windows.Forms.ToolStripMenuItem productMovementF4ToolStripMenuItem;
        private System.Windows.Forms.DataGridView grid_purchases;
        private System.Windows.Forms.Panel panel_grid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_total_amount;
        private System.Windows.Forms.TextBox txt_total_tax;
        private System.Windows.Forms.TextBox txt_total_discount;
        private System.Windows.Forms.TextBox txt_sub_total;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btn_movements;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txt_description;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rd_btn_with_vat;
        private System.Windows.Forms.RadioButton rd_btn_without_vat;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rd_btn_bytotal_price;
        private System.Windows.Forms.RadioButton rd_btn_by_unitprice;
        private System.Windows.Forms.ToolStripMenuItem addNewRowToolStripMenuItem;
        private System.Windows.Forms.BindingSource locationsdtDataTableBindingSource;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_search_purchase_invoices;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmb_employees;
        private System.Windows.Forms.DateTimePicker txt_purchase_date;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmb_purchase_type;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_invoice_no;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn packing;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn avg_cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn discount;
        private System.Windows.Forms.DataGridViewTextBoxColumn tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn sub_total;
        private System.Windows.Forms.DataGridViewTextBoxColumn location_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewButtonColumn btn_delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn tax_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn tax_rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn packet_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}