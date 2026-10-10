namespace pos.Reports.Sales
{
    partial class frm_SalesInvoiceReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_SalesInvoiceReport));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmb_payment_method = new System.Windows.Forms.ComboBox();
            this.cmb_users = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.chk_ShowZatcaInvoice = new System.Windows.Forms.CheckBox();
            this.txtCustomerSearch = new System.Windows.Forms.TextBox();
            this.CmbCondition = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmb_employees = new System.Windows.Forms.ComboBox();
            this.btn_print = new System.Windows.Forms.Button();
            this.btn_export = new System.Windows.Forms.Button();
            this.btn_search = new System.Windows.Forms.Button();
            this.cmb_sale_account = new System.Windows.Forms.ComboBox();
            this.cmb_sale_type = new System.Windows.Forms.ComboBox();
            this.txt_to_date = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_from_date = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_sales_invoice_report = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customer_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.payment_method = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_items = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discount_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_with_vat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.profit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_sales_invoice_report)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.cmb_payment_method);
            this.panel1.Controls.Add(this.cmb_users);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.chk_ShowZatcaInvoice);
            this.panel1.Controls.Add(this.txtCustomerSearch);
            this.panel1.Controls.Add(this.CmbCondition);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.cmb_employees);
            this.panel1.Controls.Add(this.btn_print);
            this.panel1.Controls.Add(this.btn_export);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.cmb_sale_account);
            this.panel1.Controls.Add(this.cmb_sale_type);
            this.panel1.Controls.Add(this.txt_to_date);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txt_from_date);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Name = "panel1";
            // 
            // cmb_payment_method
            // 
            resources.ApplyResources(this.cmb_payment_method, "cmb_payment_method");
            this.cmb_payment_method.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_payment_method.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_payment_method.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_payment_method.FormattingEnabled = true;
            this.cmb_payment_method.Name = "cmb_payment_method";
            // 
            // cmb_users
            // 
            resources.ApplyResources(this.cmb_users, "cmb_users");
            this.cmb_users.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_users.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_users.FormattingEnabled = true;
            this.cmb_users.Name = "cmb_users";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // txt_search
            // 
            resources.ApplyResources(this.txt_search, "txt_search");
            this.txt_search.Name = "txt_search";
            // 
            // chk_ShowZatcaInvoice
            // 
            resources.ApplyResources(this.chk_ShowZatcaInvoice, "chk_ShowZatcaInvoice");
            this.chk_ShowZatcaInvoice.Name = "chk_ShowZatcaInvoice";
            this.chk_ShowZatcaInvoice.UseVisualStyleBackColor = true;
            // 
            // txtCustomerSearch
            // 
            resources.ApplyResources(this.txtCustomerSearch, "txtCustomerSearch");
            this.txtCustomerSearch.Name = "txtCustomerSearch";
            this.txtCustomerSearch.TextChanged += new System.EventHandler(this.txtCustomerSearch_TextChanged_1);
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
            // cmb_sale_account
            // 
            resources.ApplyResources(this.cmb_sale_account, "cmb_sale_account");
            this.cmb_sale_account.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_sale_account.FormattingEnabled = true;
            this.cmb_sale_account.Items.AddRange(new object[] {
            resources.GetString("cmb_sale_account.Items"),
            resources.GetString("cmb_sale_account.Items1"),
            resources.GetString("cmb_sale_account.Items2")});
            this.cmb_sale_account.Name = "cmb_sale_account";
            // 
            // cmb_sale_type
            // 
            resources.ApplyResources(this.cmb_sale_type, "cmb_sale_type");
            this.cmb_sale_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_sale_type.FormattingEnabled = true;
            this.cmb_sale_type.Items.AddRange(new object[] {
            resources.GetString("cmb_sale_type.Items"),
            resources.GetString("cmb_sale_type.Items1"),
            resources.GetString("cmb_sale_type.Items2")});
            this.cmb_sale_type.Name = "cmb_sale_type";
            // 
            // txt_to_date
            // 
            resources.ApplyResources(this.txt_to_date, "txt_to_date");
            this.txt_to_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_to_date.Name = "txt_to_date";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
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
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.grid_sales_invoice_report);
            this.panel2.Name = "panel2";
            // 
            // grid_sales_invoice_report
            // 
            resources.ApplyResources(this.grid_sales_invoice_report, "grid_sales_invoice_report");
            this.grid_sales_invoice_report.AllowUserToAddRows = false;
            this.grid_sales_invoice_report.AllowUserToDeleteRows = false;
            this.grid_sales_invoice_report.AllowUserToOrderColumns = true;
            this.grid_sales_invoice_report.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_sales_invoice_report.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_sales_invoice_report.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.sale_date,
            this.invoice_no,
            this.customer_name,
            this.sale_type,
            this.payment_method,
            this.total_items,
            this.cost_total,
            this.subtotal,
            this.discount_value,
            this.vat,
            this.total,
            this.total_with_vat,
            this.profit});
            this.grid_sales_invoice_report.Name = "grid_sales_invoice_report";
            this.grid_sales_invoice_report.ReadOnly = true;
            this.grid_sales_invoice_report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_sales_invoice_report.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_sales_invoice_report_CellDoubleClick);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.FillWeight = 30F;
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // sale_date
            // 
            this.sale_date.DataPropertyName = "sale_date";
            this.sale_date.FillWeight = 70F;
            resources.ApplyResources(this.sale_date, "sale_date");
            this.sale_date.Name = "sale_date";
            this.sale_date.ReadOnly = true;
            // 
            // invoice_no
            // 
            this.invoice_no.DataPropertyName = "invoice_no";
            this.invoice_no.FillWeight = 90F;
            resources.ApplyResources(this.invoice_no, "invoice_no");
            this.invoice_no.Name = "invoice_no";
            this.invoice_no.ReadOnly = true;
            // 
            // customer_name
            // 
            this.customer_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.customer_name.DataPropertyName = "customer_name";
            this.customer_name.FillWeight = 150F;
            resources.ApplyResources(this.customer_name, "customer_name");
            this.customer_name.Name = "customer_name";
            this.customer_name.ReadOnly = true;
            // 
            // sale_type
            // 
            this.sale_type.DataPropertyName = "sale_type";
            this.sale_type.FillWeight = 50F;
            resources.ApplyResources(this.sale_type, "sale_type");
            this.sale_type.Name = "sale_type";
            this.sale_type.ReadOnly = true;
            // 
            // payment_method
            // 
            this.payment_method.DataPropertyName = "payment_method";
            this.payment_method.FillWeight = 50F;
            resources.ApplyResources(this.payment_method, "payment_method");
            this.payment_method.Name = "payment_method";
            this.payment_method.ReadOnly = true;
            // 
            // total_items
            // 
            this.total_items.DataPropertyName = "total_items";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.total_items.DefaultCellStyle = dataGridViewCellStyle1;
            this.total_items.FillWeight = 50F;
            resources.ApplyResources(this.total_items, "total_items");
            this.total_items.Name = "total_items";
            this.total_items.ReadOnly = true;
            // 
            // cost_total
            // 
            this.cost_total.DataPropertyName = "cost_total";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.cost_total.DefaultCellStyle = dataGridViewCellStyle2;
            this.cost_total.FillWeight = 70F;
            resources.ApplyResources(this.cost_total, "cost_total");
            this.cost_total.Name = "cost_total";
            this.cost_total.ReadOnly = true;
            // 
            // subtotal
            // 
            this.subtotal.DataPropertyName = "subtotal";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            this.subtotal.DefaultCellStyle = dataGridViewCellStyle3;
            this.subtotal.FillWeight = 80F;
            resources.ApplyResources(this.subtotal, "subtotal");
            this.subtotal.Name = "subtotal";
            this.subtotal.ReadOnly = true;
            // 
            // discount_value
            // 
            this.discount_value.DataPropertyName = "discount_value";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            this.discount_value.DefaultCellStyle = dataGridViewCellStyle4;
            this.discount_value.FillWeight = 70F;
            resources.ApplyResources(this.discount_value, "discount_value");
            this.discount_value.Name = "discount_value";
            this.discount_value.ReadOnly = true;
            // 
            // vat
            // 
            this.vat.DataPropertyName = "vat";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            this.vat.DefaultCellStyle = dataGridViewCellStyle5;
            this.vat.FillWeight = 60F;
            resources.ApplyResources(this.vat, "vat");
            this.vat.Name = "vat";
            this.vat.ReadOnly = true;
            // 
            // total
            // 
            this.total.DataPropertyName = "total";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            this.total.DefaultCellStyle = dataGridViewCellStyle6;
            this.total.FillWeight = 80F;
            resources.ApplyResources(this.total, "total");
            this.total.Name = "total";
            this.total.ReadOnly = true;
            // 
            // total_with_vat
            // 
            this.total_with_vat.DataPropertyName = "total_with_vat";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            this.total_with_vat.DefaultCellStyle = dataGridViewCellStyle7;
            this.total_with_vat.FillWeight = 80F;
            resources.ApplyResources(this.total_with_vat, "total_with_vat");
            this.total_with_vat.Name = "total_with_vat";
            this.total_with_vat.ReadOnly = true;
            // 
            // profit
            // 
            this.profit.DataPropertyName = "profit";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N2";
            this.profit.DefaultCellStyle = dataGridViewCellStyle8;
            this.profit.FillWeight = 70F;
            resources.ApplyResources(this.profit, "profit");
            this.profit.Name = "profit";
            this.profit.ReadOnly = true;
            // 
            // frm_SalesInvoiceReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_SalesInvoiceReport";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_SalesInvoiceReport_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_SalesInvoiceReport_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_sales_invoice_report)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.CheckBox chk_ShowZatcaInvoice;
        private System.Windows.Forms.TextBox txtCustomerSearch;
        private System.Windows.Forms.ComboBox CmbCondition;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmb_employees;
        private System.Windows.Forms.Button btn_print;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.ComboBox cmb_sale_account;
        private System.Windows.Forms.ComboBox cmb_sale_type;
        private System.Windows.Forms.DateTimePicker txt_to_date;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker txt_from_date;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView grid_sales_invoice_report;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmb_users;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn customer_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn payment_method;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_items;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_total;
        private System.Windows.Forms.DataGridViewTextBoxColumn subtotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn discount_value;
        private System.Windows.Forms.DataGridViewTextBoxColumn vat;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_with_vat;
        private System.Windows.Forms.DataGridViewTextBoxColumn profit;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmb_payment_method;
    }
}
