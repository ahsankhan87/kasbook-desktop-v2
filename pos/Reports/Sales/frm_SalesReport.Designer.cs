namespace pos
{
    partial class frm_SalesReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_SalesReport));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtCustomerSearch = new System.Windows.Forms.TextBox();
            this.CmbCondition = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmb_employees = new System.Windows.Forms.ComboBox();
            this.txt_product_name = new System.Windows.Forms.TextBox();
            this.txt_product_code = new System.Windows.Forms.TextBox();
            this.btn_print = new System.Windows.Forms.Button();
            this.btn_export = new System.Windows.Forms.Button();
            this.btn_search = new System.Windows.Forms.Button();
            this.cmb_sale_account = new System.Windows.Forms.ComboBox();
            this.cmb_sale_type = new System.Windows.Forms.ComboBox();
            this.txt_to_date = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_from_date = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_sales_report = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customer_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loc_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantity_sold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discount_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_with_vat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.profit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.profit_percent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_sales_report)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtCustomerSearch);
            this.panel1.Controls.Add(this.CmbCondition);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.cmb_employees);
            this.panel1.Controls.Add(this.txt_product_name);
            this.panel1.Controls.Add(this.txt_product_code);
            this.panel1.Controls.Add(this.btn_print);
            this.panel1.Controls.Add(this.btn_export);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.cmb_sale_account);
            this.panel1.Controls.Add(this.cmb_sale_type);
            this.panel1.Controls.Add(this.txt_to_date);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txt_from_date);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // txtCustomerSearch
            // 
            resources.ApplyResources(this.txtCustomerSearch, "txtCustomerSearch");
            this.txtCustomerSearch.Name = "txtCustomerSearch";
            // 
            // CmbCondition
            // 
            this.CmbCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbCondition.FormattingEnabled = true;
            resources.ApplyResources(this.CmbCondition, "CmbCondition");
            this.CmbCondition.Name = "CmbCondition";
            this.CmbCondition.SelectedIndexChanged += new System.EventHandler(this.CmbCondition_SelectedIndexChanged);
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
            // txt_product_name
            // 
            resources.ApplyResources(this.txt_product_name, "txt_product_name");
            this.txt_product_name.Name = "txt_product_name";
            this.txt_product_name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_product_code_KeyDown);
            // 
            // txt_product_code
            // 
            resources.ApplyResources(this.txt_product_code, "txt_product_code");
            this.txt_product_code.Name = "txt_product_code";
            this.txt_product_code.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_product_code_KeyDown);
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
            this.cmb_sale_account.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_sale_account.FormattingEnabled = true;
            this.cmb_sale_account.Items.AddRange(new object[] {
            resources.GetString("cmb_sale_account.Items"),
            resources.GetString("cmb_sale_account.Items1"),
            resources.GetString("cmb_sale_account.Items2")});
            resources.ApplyResources(this.cmb_sale_account, "cmb_sale_account");
            this.cmb_sale_account.Name = "cmb_sale_account";
            // 
            // cmb_sale_type
            // 
            this.cmb_sale_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_sale_type.FormattingEnabled = true;
            this.cmb_sale_type.Items.AddRange(new object[] {
            resources.GetString("cmb_sale_type.Items"),
            resources.GetString("cmb_sale_type.Items1"),
            resources.GetString("cmb_sale_type.Items2")});
            resources.ApplyResources(this.cmb_sale_type, "cmb_sale_type");
            this.cmb_sale_type.Name = "cmb_sale_type";
            // 
            // txt_to_date
            // 
            this.txt_to_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.txt_to_date, "txt_to_date");
            this.txt_to_date.Name = "txt_to_date";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // txt_from_date
            // 
            this.txt_from_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.txt_from_date, "txt_from_date");
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
            this.panel2.Controls.Add(this.grid_sales_report);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // grid_sales_report
            // 
            this.grid_sales_report.AllowUserToAddRows = false;
            this.grid_sales_report.AllowUserToDeleteRows = false;
            this.grid_sales_report.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.grid_sales_report, "grid_sales_report");
            this.grid_sales_report.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_sales_report.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_sales_report.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.sale_date,
            this.invoice_no,
            this.customer_name,
            this.product_code,
            this.product_name,
            this.loc_code,
            this.quantity_sold,
            this.cost_price,
            this.unit_price,
            this.discount_value,
            this.total,
            this.vat,
            this.total_with_vat,
            this.profit,
            this.profit_percent,
            this.cost_total});
            this.grid_sales_report.Name = "grid_sales_report";
            this.grid_sales_report.ReadOnly = true;
            this.grid_sales_report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
            // product_code
            // 
            this.product_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.product_code.DataPropertyName = "item_code";
            this.product_code.FillWeight = 90F;
            resources.ApplyResources(this.product_code, "product_code");
            this.product_code.Name = "product_code";
            this.product_code.ReadOnly = true;
            // 
            // product_name
            // 
            this.product_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.product_name.DataPropertyName = "product_name";
            this.product_name.FillWeight = 200F;
            resources.ApplyResources(this.product_name, "product_name");
            this.product_name.Name = "product_name";
            this.product_name.ReadOnly = true;
            // 
            // loc_code
            // 
            this.loc_code.DataPropertyName = "loc_code";
            this.loc_code.FillWeight = 50F;
            resources.ApplyResources(this.loc_code, "loc_code");
            this.loc_code.Name = "loc_code";
            this.loc_code.ReadOnly = true;
            // 
            // quantity_sold
            // 
            this.quantity_sold.DataPropertyName = "quantity_sold";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Format = "N2";
            dataGridViewCellStyle10.NullValue = null;
            this.quantity_sold.DefaultCellStyle = dataGridViewCellStyle10;
            this.quantity_sold.FillWeight = 70F;
            resources.ApplyResources(this.quantity_sold, "quantity_sold");
            this.quantity_sold.Name = "quantity_sold";
            this.quantity_sold.ReadOnly = true;
            // 
            // cost_price
            // 
            this.cost_price.DataPropertyName = "cost_price";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N2";
            dataGridViewCellStyle11.NullValue = null;
            this.cost_price.DefaultCellStyle = dataGridViewCellStyle11;
            this.cost_price.FillWeight = 70F;
            resources.ApplyResources(this.cost_price, "cost_price");
            this.cost_price.Name = "cost_price";
            this.cost_price.ReadOnly = true;
            // 
            // unit_price
            // 
            this.unit_price.DataPropertyName = "unit_price";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Format = "N2";
            this.unit_price.DefaultCellStyle = dataGridViewCellStyle12;
            this.unit_price.FillWeight = 70F;
            resources.ApplyResources(this.unit_price, "unit_price");
            this.unit_price.Name = "unit_price";
            this.unit_price.ReadOnly = true;
            // 
            // discount_value
            // 
            this.discount_value.DataPropertyName = "discount_value";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.Format = "N2";
            this.discount_value.DefaultCellStyle = dataGridViewCellStyle13;
            this.discount_value.FillWeight = 70F;
            resources.ApplyResources(this.discount_value, "discount_value");
            this.discount_value.Name = "discount_value";
            this.discount_value.ReadOnly = true;
            // 
            // total
            // 
            this.total.DataPropertyName = "total";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.Format = "N2";
            dataGridViewCellStyle14.NullValue = null;
            this.total.DefaultCellStyle = dataGridViewCellStyle14;
            this.total.FillWeight = 80F;
            resources.ApplyResources(this.total, "total");
            this.total.Name = "total";
            this.total.ReadOnly = true;
            // 
            // vat
            // 
            this.vat.DataPropertyName = "vat";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.Format = "N2";
            dataGridViewCellStyle15.NullValue = null;
            this.vat.DefaultCellStyle = dataGridViewCellStyle15;
            this.vat.FillWeight = 60F;
            resources.ApplyResources(this.vat, "vat");
            this.vat.Name = "vat";
            this.vat.ReadOnly = true;
            // 
            // total_with_vat
            // 
            this.total_with_vat.DataPropertyName = "total_with_vat";
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Format = "N2";
            dataGridViewCellStyle16.NullValue = null;
            this.total_with_vat.DefaultCellStyle = dataGridViewCellStyle16;
            this.total_with_vat.FillWeight = 80F;
            resources.ApplyResources(this.total_with_vat, "total_with_vat");
            this.total_with_vat.Name = "total_with_vat";
            this.total_with_vat.ReadOnly = true;
            // 
            // profit
            // 
            this.profit.DataPropertyName = "profit";
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle17.Format = "N2";
            dataGridViewCellStyle17.NullValue = null;
            this.profit.DefaultCellStyle = dataGridViewCellStyle17;
            this.profit.FillWeight = 70F;
            resources.ApplyResources(this.profit, "profit");
            this.profit.Name = "profit";
            this.profit.ReadOnly = true;
            // 
            // profit_percent
            // 
            this.profit_percent.DataPropertyName = "profit_percent";
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle18.Format = "N2";
            dataGridViewCellStyle18.NullValue = null;
            this.profit_percent.DefaultCellStyle = dataGridViewCellStyle18;
            this.profit_percent.FillWeight = 60F;
            resources.ApplyResources(this.profit_percent, "profit_percent");
            this.profit_percent.Name = "profit_percent";
            this.profit_percent.ReadOnly = true;
            // 
            // cost_total
            // 
            this.cost_total.FillWeight = 70F;
            resources.ApplyResources(this.cost_total, "cost_total");
            this.cost_total.Name = "cost_total";
            this.cost_total.ReadOnly = true;
            // 
            // frm_SalesReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_SalesReport";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SalesReport_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_SalesReport_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_sales_report)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView grid_sales_report;
        private System.Windows.Forms.DateTimePicker txt_to_date;
        private System.Windows.Forms.DateTimePicker txt_from_date;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmb_sale_type;
        private System.Windows.Forms.TextBox txt_product_code;
        private System.Windows.Forms.TextBox txt_product_name;
        private System.Windows.Forms.ComboBox cmb_sale_account;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmb_employees;
        private System.Windows.Forms.Button btn_print;
        private System.Windows.Forms.ComboBox CmbCondition;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn customer_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn loc_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantity_sold;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn discount_value;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn vat;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_with_vat;
        private System.Windows.Forms.DataGridViewTextBoxColumn profit;
        private System.Windows.Forms.DataGridViewTextBoxColumn profit_percent;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_total;
        private System.Windows.Forms.TextBox txtCustomerSearch;
    }
}