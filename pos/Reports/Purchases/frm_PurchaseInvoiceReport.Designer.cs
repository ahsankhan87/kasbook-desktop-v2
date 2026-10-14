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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_PurchaseInvoiceReport));
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
            this.label9 = new System.Windows.Forms.Label();
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
            resources.ApplyResources(this.panel1, "panel1");
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
            this.panel1.Name = "panel1";
            // 
            // txt_search
            // 
            resources.ApplyResources(this.txt_search, "txt_search");
            this.txt_search.Name = "txt_search";
            // 
            // txtSupplierSearch
            // 
            resources.ApplyResources(this.txtSupplierSearch, "txtSupplierSearch");
            this.txtSupplierSearch.Name = "txtSupplierSearch";
            // 
            // CmbCondition
            // 
            resources.ApplyResources(this.CmbCondition, "CmbCondition");
            this.CmbCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbCondition.FormattingEnabled = true;
            this.CmbCondition.Name = "CmbCondition";
            this.CmbCondition.SelectedIndexChanged += new System.EventHandler(this.CmbCondition_SelectedIndexChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmb_employees
            // 
            resources.ApplyResources(this.cmb_employees, "cmb_employees");
            this.cmb_employees.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_employees.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_employees.FormattingEnabled = true;
            this.cmb_employees.Name = "cmb_employees";
            // 
            // btn_print
            // 
            resources.ApplyResources(this.btn_print, "btn_print");
            this.btn_print.Name = "btn_print";
            this.btn_print.UseVisualStyleBackColor = true;
            this.btn_print.Click += new System.EventHandler(this.btn_print_Click);
            // 
            // btn_export
            // 
            resources.ApplyResources(this.btn_export, "btn_export");
            this.btn_export.Name = "btn_export";
            this.btn_export.UseVisualStyleBackColor = true;
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // btn_search
            // 
            resources.ApplyResources(this.btn_search, "btn_search");
            this.btn_search.Name = "btn_search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // cmb_purchase_type
            // 
            resources.ApplyResources(this.cmb_purchase_type, "cmb_purchase_type");
            this.cmb_purchase_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_purchase_type.FormattingEnabled = true;
            this.cmb_purchase_type.Items.AddRange(new object[] {
            resources.GetString("cmb_purchase_type.Items"),
            resources.GetString("cmb_purchase_type.Items1"),
            resources.GetString("cmb_purchase_type.Items2")});
            this.cmb_purchase_type.Name = "cmb_purchase_type";
            // 
            // txt_to_date
            // 
            resources.ApplyResources(this.txt_to_date, "txt_to_date");
            this.txt_to_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_to_date.Name = "txt_to_date";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // txt_from_date
            // 
            resources.ApplyResources(this.txt_from_date, "txt_from_date");
            this.txt_from_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_from_date.Name = "txt_from_date";
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
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.grid_purchase_invoice_report);
            this.panel2.Name = "panel2";
            // 
            // grid_purchase_invoice_report
            // 
            resources.ApplyResources(this.grid_purchase_invoice_report, "grid_purchase_invoice_report");
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
            this.grid_purchase_invoice_report.Name = "grid_purchase_invoice_report";
            this.grid_purchase_invoice_report.ReadOnly = true;
            this.grid_purchase_invoice_report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_purchase_invoice_report.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_purchase_invoice_report_CellDoubleClick);
            // 
            // col_id
            // 
            this.col_id.DataPropertyName = "id";
            this.col_id.FillWeight = 30F;
            resources.ApplyResources(this.col_id, "col_id");
            this.col_id.Name = "col_id";
            this.col_id.ReadOnly = true;
            // 
            // col_purchase_date
            // 
            this.col_purchase_date.DataPropertyName = "purchase_date";
            this.col_purchase_date.FillWeight = 70F;
            resources.ApplyResources(this.col_purchase_date, "col_purchase_date");
            this.col_purchase_date.Name = "col_purchase_date";
            this.col_purchase_date.ReadOnly = true;
            // 
            // col_invoice_no
            // 
            this.col_invoice_no.DataPropertyName = "invoice_no";
            this.col_invoice_no.FillWeight = 80F;
            resources.ApplyResources(this.col_invoice_no, "col_invoice_no");
            this.col_invoice_no.Name = "col_invoice_no";
            this.col_invoice_no.ReadOnly = true;
            // 
            // col_supplier_invoice_no
            // 
            this.col_supplier_invoice_no.DataPropertyName = "supplier_invoice_no";
            this.col_supplier_invoice_no.FillWeight = 80F;
            resources.ApplyResources(this.col_supplier_invoice_no, "col_supplier_invoice_no");
            this.col_supplier_invoice_no.Name = "col_supplier_invoice_no";
            this.col_supplier_invoice_no.ReadOnly = true;
            // 
            // col_supplier_name
            // 
            this.col_supplier_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_supplier_name.DataPropertyName = "supplier_name";
            this.col_supplier_name.FillWeight = 150F;
            resources.ApplyResources(this.col_supplier_name, "col_supplier_name");
            this.col_supplier_name.Name = "col_supplier_name";
            this.col_supplier_name.ReadOnly = true;
            // 
            // col_purchase_type
            // 
            this.col_purchase_type.DataPropertyName = "purchase_type";
            this.col_purchase_type.FillWeight = 60F;
            resources.ApplyResources(this.col_purchase_type, "col_purchase_type");
            this.col_purchase_type.Name = "col_purchase_type";
            this.col_purchase_type.ReadOnly = true;
            // 
            // col_total_items
            // 
            this.col_total_items.DataPropertyName = "total_items";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.col_total_items.DefaultCellStyle = dataGridViewCellStyle1;
            this.col_total_items.FillWeight = 50F;
            resources.ApplyResources(this.col_total_items, "col_total_items");
            this.col_total_items.Name = "col_total_items";
            this.col_total_items.ReadOnly = true;
            // 
            // col_subtotal
            // 
            this.col_subtotal.DataPropertyName = "subtotal";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            this.col_subtotal.DefaultCellStyle = dataGridViewCellStyle2;
            this.col_subtotal.FillWeight = 80F;
            resources.ApplyResources(this.col_subtotal, "col_subtotal");
            this.col_subtotal.Name = "col_subtotal";
            this.col_subtotal.ReadOnly = true;
            // 
            // col_discount_value
            // 
            this.col_discount_value.DataPropertyName = "discount_value";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            this.col_discount_value.DefaultCellStyle = dataGridViewCellStyle3;
            this.col_discount_value.FillWeight = 70F;
            resources.ApplyResources(this.col_discount_value, "col_discount_value");
            this.col_discount_value.Name = "col_discount_value";
            this.col_discount_value.ReadOnly = true;
            // 
            // col_vat
            // 
            this.col_vat.DataPropertyName = "vat";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            this.col_vat.DefaultCellStyle = dataGridViewCellStyle4;
            this.col_vat.FillWeight = 60F;
            resources.ApplyResources(this.col_vat, "col_vat");
            this.col_vat.Name = "col_vat";
            this.col_vat.ReadOnly = true;
            // 
            // col_total
            // 
            this.col_total.DataPropertyName = "total";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            this.col_total.DefaultCellStyle = dataGridViewCellStyle5;
            this.col_total.FillWeight = 80F;
            resources.ApplyResources(this.col_total, "col_total");
            this.col_total.Name = "col_total";
            this.col_total.ReadOnly = true;
            // 
            // col_total_with_vat
            // 
            this.col_total_with_vat.DataPropertyName = "total_with_vat";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            this.col_total_with_vat.DefaultCellStyle = dataGridViewCellStyle6;
            this.col_total_with_vat.FillWeight = 80F;
            resources.ApplyResources(this.col_total_with_vat, "col_total_with_vat");
            this.col_total_with_vat.Name = "col_total_with_vat";
            this.col_total_with_vat.ReadOnly = true;
            // 
            // frm_PurchaseInvoiceReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_PurchaseInvoiceReport";
            this.ShowIcon = false;
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
