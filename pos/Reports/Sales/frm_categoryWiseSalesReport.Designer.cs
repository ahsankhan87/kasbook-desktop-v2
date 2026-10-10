namespace pos
{
    partial class frm_categoryWiseSalesReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_categoryWiseSalesReport));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.cmb_employees = new System.Windows.Forms.ComboBox();
            this.btn_print = new System.Windows.Forms.Button();
            this.btn_search = new System.Windows.Forms.Button();
            this.cmb_sale_account = new System.Windows.Forms.ComboBox();
            this.cmb_sale_type = new System.Windows.Forms.ComboBox();
            this.txt_to_date = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_from_date = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.cmb_customers = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_sales_report = new System.Windows.Forms.DataGridView();
            this.category_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_sales_report)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.cmb_employees);
            this.panel1.Controls.Add(this.btn_print);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.cmb_sale_account);
            this.panel1.Controls.Add(this.cmb_sale_type);
            this.panel1.Controls.Add(this.txt_to_date);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txt_from_date);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmb_customers);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Name = "panel1";
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
            // cmb_customers
            // 
            resources.ApplyResources(this.cmb_customers, "cmb_customers");
            this.cmb_customers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_customers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_customers.FormattingEnabled = true;
            this.cmb_customers.Name = "cmb_customers";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
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
            this.panel2.Controls.Add(this.grid_sales_report);
            this.panel2.Name = "panel2";
            // 
            // grid_sales_report
            // 
            resources.ApplyResources(this.grid_sales_report, "grid_sales_report");
            this.grid_sales_report.AllowUserToAddRows = false;
            this.grid_sales_report.AllowUserToDeleteRows = false;
            this.grid_sales_report.AllowUserToOrderColumns = true;
            this.grid_sales_report.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_sales_report.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_sales_report.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.category_name,
            this.qty,
            this.total_amount});
            this.grid_sales_report.Name = "grid_sales_report";
            this.grid_sales_report.ReadOnly = true;
            this.grid_sales_report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // category_name
            // 
            this.category_name.DataPropertyName = "category_name";
            resources.ApplyResources(this.category_name, "category_name");
            this.category_name.Name = "category_name";
            this.category_name.ReadOnly = true;
            // 
            // qty
            // 
            this.qty.DataPropertyName = "qty";
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.qty.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.qty, "qty");
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            // 
            // total_amount
            // 
            this.total_amount.DataPropertyName = "total_amount";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.total_amount.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.total_amount, "total_amount");
            this.total_amount.Name = "total_amount";
            this.total_amount.ReadOnly = true;
            // 
            // frm_categoryWiseSalesReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_categoryWiseSalesReport";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.categoryWiseSalesReport_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_categoryWiseSalesReport_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_sales_report)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView grid_sales_report;
        private System.Windows.Forms.DateTimePicker txt_to_date;
        private System.Windows.Forms.DateTimePicker txt_from_date;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmb_customers;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmb_sale_type;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.ComboBox cmb_sale_account;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmb_employees;
        private System.Windows.Forms.Button btn_print;
        private System.Windows.Forms.DataGridViewTextBoxColumn category_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_amount;
    }
}