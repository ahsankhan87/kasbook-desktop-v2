namespace pos.Reports.Purchases
{
    partial class frm_PurchaseInvoiceReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.txtSupplierSearch = new System.Windows.Forms.TextBox();
            this.CmbCondition = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmb_employees = new System.Windows.Forms.ComboBox();
            this.btn_print = new System.Windows.Forms.Button();
            this.btn_export = new System.Windows.Forms.Button();
            this.btn_search = new System.Windows.Forms.Button();
            this.cmb_purchase_type = new System.Windows.Forms.ComboBox();
            this.txt_to_date = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_from_date = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_purchase_invoice_report = new System.Windows.Forms.DataGridView();
            this.col_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_purchase_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_supplier_invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_supplier_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_purchase_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_total_items = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_subtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_discount_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_vat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_total_with_vat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_purchase_invoice_report)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.txtSupplierSearch);
            this.panel1.Controls.Add(this.CmbCondition);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.cmb_employees);
            this.panel1.Controls.Add(this.btn_print);
            this.panel1.Controls.Add(this.btn_export);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.cmb_purchase_type);
            this.panel1.Controls.Add(this.txt_to_date);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txt_from_date);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1517, 120);
            this.panel1.TabIndex = 0;
            // 
            // txt_search
            // 
            this.txt_search.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_search.Location = new System.Drawing.Point(761, 60);
            this.txt_search.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(209, 29);
            this.txt_search.TabIndex = 50;
            // 
            // txtSupplierSearch
            // 
            this.txtSupplierSearch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSupplierSearch.Location = new System.Drawing.Point(455, 12);
            this.txtSupplierSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSupplierSearch.Name = "txtSupplierSearch";
            this.txtSupplierSearch.Size = new System.Drawing.Size(209, 29);
            this.txtSupplierSearch.TabIndex = 48;
            // 
            // CmbCondition
            // 
            this.CmbCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbCondition.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbCondition.FormattingEnabled = true;
            this.CmbCondition.Location = new System.Drawing.Point(135, 10);
            this.CmbCondition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CmbCondition.Name = "CmbCondition";
            this.CmbCondition.Size = new System.Drawing.Size(209, 29);
            this.CmbCondition.TabIndex = 47;
            this.CmbCondition.SelectedIndexChanged += new System.EventHandler(this.CmbCondition_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(675, 15);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 20);
            this.label7.TabIndex = 46;
            this.label7.Text = "Employee:";
            // 
            // cmb_employees
            // 
            this.cmb_employees.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_employees.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_employees.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_employees.FormattingEnabled = true;
            this.cmb_employees.Location = new System.Drawing.Point(761, 12);
            this.cmb_employees.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_employees.Name = "cmb_employees";
            this.cmb_employees.Size = new System.Drawing.Size(209, 29);
            this.cmb_employees.TabIndex = 45;
            // 
            // btn_print
            // 
            this.btn_print.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_print.Location = new System.Drawing.Point(1266, 62);
            this.btn_print.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_print.Name = "btn_print";
            this.btn_print.Size = new System.Drawing.Size(146, 34);
            this.btn_print.TabIndex = 44;
            this.btn_print.Text = "Print";
            this.btn_print.UseVisualStyleBackColor = true;
            this.btn_print.Click += new System.EventHandler(this.btn_print_Click);
            // 
            // btn_export
            // 
            this.btn_export.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_export.Location = new System.Drawing.Point(1266, 17);
            this.btn_export.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(146, 34);
            this.btn_export.TabIndex = 43;
            this.btn_export.Text = "Export";
            this.btn_export.UseVisualStyleBackColor = true;
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // btn_search
            // 
            this.btn_search.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_search.Location = new System.Drawing.Point(1266, 77);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(146, 34);
            this.btn_search.TabIndex = 42;
            this.btn_search.Text = "Search (F3)";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // cmb_purchase_type
            // 
            this.cmb_purchase_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_purchase_type.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_purchase_type.FormattingEnabled = true;
            this.cmb_purchase_type.Items.AddRange(new object[] {
            "All",
            "Cash",
            "Credit"});
            this.cmb_purchase_type.Location = new System.Drawing.Point(455, 60);
            this.cmb_purchase_type.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_purchase_type.Name = "cmb_purchase_type";
            this.cmb_purchase_type.Size = new System.Drawing.Size(209, 29);
            this.cmb_purchase_type.TabIndex = 40;
            // 
            // txt_to_date
            // 
            this.txt_to_date.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_to_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_to_date.Location = new System.Drawing.Point(135, 77);
            this.txt_to_date.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_to_date.Name = "txt_to_date";
            this.txt_to_date.Size = new System.Drawing.Size(209, 29);
            this.txt_to_date.TabIndex = 39;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(370, 65);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 20);
            this.label6.TabIndex = 38;
            this.label6.Text = "Purch. Type:";
            // 
            // txt_from_date
            // 
            this.txt_from_date.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_from_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_from_date.Location = new System.Drawing.Point(135, 42);
            this.txt_from_date.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_from_date.Name = "txt_from_date";
            this.txt_from_date.Size = new System.Drawing.Size(209, 29);
            this.txt_from_date.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 80);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 20);
            this.label2.TabIndex = 36;
            this.label2.Text = "To Date:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(370, 17);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 20);
            this.label5.TabIndex = 35;
            this.label5.Text = "Supplier:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(13, 12);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 20);
            this.label8.TabIndex = 34;
            this.label8.Text = "Quick Condition:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(675, 64);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 20);
            this.label3.TabIndex = 33;
            this.label3.Text = "Search:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 20);
            this.label1.TabIndex = 32;
            this.label1.Text = "From Date:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(13, 12);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(118, 20);
            this.label9.TabIndex = 51;
            this.label9.Text = "Quick Condition:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid_purchase_invoice_report);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 120);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1517, 680);
            this.panel2.TabIndex = 1;
            // 
            // grid_purchase_invoice_report
            // 
            this.grid_purchase_invoice_report.AllowUserToAddRows = false;
            this.grid_purchase_invoice_report.AllowUserToDeleteRows = false;
            this.grid_purchase_invoice_report.AllowUserToOrderColumns = true;
            this.grid_purchase_invoice_report.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_purchase_invoice_report.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_purchase_invoice_report.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_id,
            this.col_purchase_date,
            this.col_invoice_no,
            this.col_supplier_invoice_no,
            this.col_supplier_name,
            this.col_purchase_type,
            this.col_total_items,
            this.col_subtotal,
            this.col_discount_value,
            this.col_vat,
            this.col_total,
            this.col_total_with_vat});
            this.grid_purchase_invoice_report.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_purchase_invoice_report.Location = new System.Drawing.Point(0, 0);
            this.grid_purchase_invoice_report.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grid_purchase_invoice_report.Name = "grid_purchase_invoice_report";
            this.grid_purchase_invoice_report.ReadOnly = true;
            this.grid_purchase_invoice_report.RowHeadersWidth = 51;
            this.grid_purchase_invoice_report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_purchase_invoice_report.Size = new System.Drawing.Size(1517, 680);
            this.grid_purchase_invoice_report.TabIndex = 0;
            this.grid_purchase_invoice_report.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_purchase_invoice_report_CellDoubleClick);
            // 
            // col_id
            // 
            this.col_id.DataPropertyName = "id";
            this.col_id.FillWeight = 30F;
            this.col_id.HeaderText = "ID";
            this.col_id.MinimumWidth = 6;
            this.col_id.Name = "col_id";
            this.col_id.ReadOnly = true;
            // 
            // col_purchase_date
            // 
            this.col_purchase_date.DataPropertyName = "purchase_date";
            this.col_purchase_date.FillWeight = 70F;
            this.col_purchase_date.HeaderText = "Date";
            this.col_purchase_date.MinimumWidth = 6;
            this.col_purchase_date.Name = "col_purchase_date";
            this.col_purchase_date.ReadOnly = true;
            // 
            // col_invoice_no
            // 
            this.col_invoice_no.DataPropertyName = "invoice_no";
            this.col_invoice_no.FillWeight = 80F;
            this.col_invoice_no.HeaderText = "Invoice #";
            this.col_invoice_no.MinimumWidth = 6;
            this.col_invoice_no.Name = "col_invoice_no";
            this.col_invoice_no.ReadOnly = true;
            // 
            // col_supplier_invoice_no
            // 
            this.col_supplier_invoice_no.DataPropertyName = "supplier_invoice_no";
            this.col_supplier_invoice_no.FillWeight = 80F;
            this.col_supplier_invoice_no.HeaderText = "Supplier Inv #";
            this.col_supplier_invoice_no.MinimumWidth = 6;
            this.col_supplier_invoice_no.Name = "col_supplier_invoice_no";
            this.col_supplier_invoice_no.ReadOnly = true;
            // 
            // col_supplier_name
            // 
            this.col_supplier_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_supplier_name.DataPropertyName = "supplier_name";
            this.col_supplier_name.FillWeight = 150F;
            this.col_supplier_name.HeaderText = "Supplier";
            this.col_supplier_name.MinimumWidth = 6;
            this.col_supplier_name.Name = "col_supplier_name";
            this.col_supplier_name.ReadOnly = true;
            // 
            // col_purchase_type
            // 
            this.col_purchase_type.DataPropertyName = "purchase_type";
            this.col_purchase_type.FillWeight = 60F;
            this.col_purchase_type.HeaderText = "Type";
            this.col_purchase_type.MinimumWidth = 6;
            this.col_purchase_type.Name = "col_purchase_type";
            this.col_purchase_type.ReadOnly = true;
            // 
            // col_total_items
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.col_total_items.DataPropertyName = "total_items";
            this.col_total_items.DefaultCellStyle = dataGridViewCellStyle1;
            this.col_total_items.FillWeight = 50F;
            this.col_total_items.HeaderText = "Items";
            this.col_total_items.MinimumWidth = 6;
            this.col_total_items.Name = "col_total_items";
            this.col_total_items.ReadOnly = true;
            // 
            // col_subtotal
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            this.col_subtotal.DataPropertyName = "subtotal";
            this.col_subtotal.DefaultCellStyle = dataGridViewCellStyle2;
            this.col_subtotal.FillWeight = 80F;
            this.col_subtotal.HeaderText = "Subtotal";
            this.col_subtotal.MinimumWidth = 6;
            this.col_subtotal.Name = "col_subtotal";
            this.col_subtotal.ReadOnly = true;
            // 
            // col_discount_value
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            this.col_discount_value.DataPropertyName = "discount_value";
            this.col_discount_value.DefaultCellStyle = dataGridViewCellStyle3;
            this.col_discount_value.FillWeight = 70F;
            this.col_discount_value.HeaderText = "Discount";
            this.col_discount_value.MinimumWidth = 6;
            this.col_discount_value.Name = "col_discount_value";
            this.col_discount_value.ReadOnly = true;
            // 
            // col_vat
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            this.col_vat.DataPropertyName = "vat";
            this.col_vat.DefaultCellStyle = dataGridViewCellStyle4;
            this.col_vat.FillWeight = 60F;
            this.col_vat.HeaderText = "VAT";
            this.col_vat.MinimumWidth = 6;
            this.col_vat.Name = "col_vat";
            this.col_vat.ReadOnly = true;
            // 
            // col_total
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            this.col_total.DataPropertyName = "total";
            this.col_total.DefaultCellStyle = dataGridViewCellStyle5;
            this.col_total.FillWeight = 80F;
            this.col_total.HeaderText = "Total";
            this.col_total.MinimumWidth = 6;
            this.col_total.Name = "col_total";
            this.col_total.ReadOnly = true;
            // 
            // col_total_with_vat
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            this.col_total_with_vat.DataPropertyName = "total_with_vat";
            this.col_total_with_vat.DefaultCellStyle = dataGridViewCellStyle6;
            this.col_total_with_vat.FillWeight = 80F;
            this.col_total_with_vat.HeaderText = "Total (VAT)";
            this.col_total_with_vat.MinimumWidth = 6;
            this.col_total_with_vat.Name = "col_total_with_vat";
            this.col_total_with_vat.ReadOnly = true;
            // 
            // frm_PurchaseInvoiceReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1517, 800);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frm_PurchaseInvoiceReport";
            this.ShowIcon = false;
            this.Text = "Purchase Invoice Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_PurchaseInvoiceReport_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_PurchaseInvoiceReport_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_purchase_invoice_report)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.TextBox txtSupplierSearch;
        private System.Windows.Forms.ComboBox CmbCondition;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmb_employees;
        private System.Windows.Forms.Button btn_print;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.ComboBox cmb_purchase_type;
        private System.Windows.Forms.DateTimePicker txt_to_date;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker txt_from_date;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView grid_purchase_invoice_report;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_purchase_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_supplier_invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_supplier_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_purchase_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_total_items;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_subtotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_discount_value;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_vat;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_total;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_total_with_vat;
    }
}
