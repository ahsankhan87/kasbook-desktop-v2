namespace pos
{
    partial class frm_customerWiseSalesReport
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_print = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cmb_employees = new System.Windows.Forms.ComboBox();
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
            this.customer_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_sales_report)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_print);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.cmb_employees);
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
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(883, 110);
            this.panel1.TabIndex = 0;
            // 
            // btn_print
            // 
            this.btn_print.Location = new System.Drawing.Point(766, 66);
            this.btn_print.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_print.Name = "btn_print";
            this.btn_print.Size = new System.Drawing.Size(100, 32);
            this.btn_print.TabIndex = 19;
            this.btn_print.Text = "Print";
            this.btn_print.UseVisualStyleBackColor = true;
            this.btn_print.Click += new System.EventHandler(this.btn_print_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(370, 18);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 16);
            this.label7.TabIndex = 18;
            this.label7.Text = "Employees:";
            // 
            // cmb_employees
            // 
            this.cmb_employees.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_employees.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_employees.FormattingEnabled = true;
            this.cmb_employees.Location = new System.Drawing.Point(473, 14);
            this.cmb_employees.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_employees.Name = "cmb_employees";
            this.cmb_employees.Size = new System.Drawing.Size(258, 24);
            this.cmb_employees.TabIndex = 7;
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(766, 14);
            this.btn_search.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(100, 50);
            this.btn_search.TabIndex = 8;
            this.btn_search.Text = "Search (F3)";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // cmb_sale_account
            // 
            this.cmb_sale_account.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_sale_account.FormattingEnabled = true;
            this.cmb_sale_account.Items.AddRange(new object[] {
            "All",
            "Sale",
            "Return"});
            this.cmb_sale_account.Location = new System.Drawing.Point(473, 70);
            this.cmb_sale_account.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_sale_account.Name = "cmb_sale_account";
            this.cmb_sale_account.Size = new System.Drawing.Size(258, 24);
            this.cmb_sale_account.TabIndex = 6;
            // 
            // cmb_sale_type
            // 
            this.cmb_sale_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_sale_type.FormattingEnabled = true;
            this.cmb_sale_type.Items.AddRange(new object[] {
            "All",
            "Cash",
            "Credit"});
            this.cmb_sale_type.Location = new System.Drawing.Point(91, 70);
            this.cmb_sale_type.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_sale_type.Name = "cmb_sale_type";
            this.cmb_sale_type.Size = new System.Drawing.Size(265, 24);
            this.cmb_sale_type.TabIndex = 3;
            // 
            // txt_to_date
            // 
            this.txt_to_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_to_date.Location = new System.Drawing.Point(91, 42);
            this.txt_to_date.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_to_date.Name = "txt_to_date";
            this.txt_to_date.Size = new System.Drawing.Size(265, 22);
            this.txt_to_date.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(370, 74);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Account";
            // 
            // txt_from_date
            // 
            this.txt_from_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_from_date.Location = new System.Drawing.Point(91, 14);
            this.txt_from_date.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_from_date.Name = "txt_from_date";
            this.txt_from_date.Size = new System.Drawing.Size(265, 22);
            this.txt_from_date.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 74);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Sale Type";
            // 
            // cmb_customers
            // 
            this.cmb_customers.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmb_customers.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmb_customers.FormattingEnabled = true;
            this.cmb_customers.Location = new System.Drawing.Point(473, 42);
            this.cmb_customers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmb_customers.Name = "cmb_customers";
            this.cmb_customers.Size = new System.Drawing.Size(258, 24);
            this.cmb_customers.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 46);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 5;
            this.label5.Text = "To Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "From Date";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(370, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Customers";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid_sales_report);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 110);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(883, 548);
            this.panel2.TabIndex = 9;
            // 
            // grid_sales_report
            // 
            this.grid_sales_report.AllowUserToAddRows = false;
            this.grid_sales_report.AllowUserToDeleteRows = false;
            this.grid_sales_report.AllowUserToOrderColumns = true;
            this.grid_sales_report.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid_sales_report.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_sales_report.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_sales_report.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.customer_name,
            this.total_amount});
            this.grid_sales_report.Location = new System.Drawing.Point(4, 4);
            this.grid_sales_report.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grid_sales_report.Name = "grid_sales_report";
            this.grid_sales_report.ReadOnly = true;
            this.grid_sales_report.RowHeadersWidth = 51;
            this.grid_sales_report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_sales_report.Size = new System.Drawing.Size(874, 544);
            this.grid_sales_report.TabIndex = 9;
            // 
            // customer_name
            // 
            this.customer_name.DataPropertyName = "customer_name";
            dataGridViewCellStyle1.NullValue = null;
            this.customer_name.DefaultCellStyle = dataGridViewCellStyle1;
            this.customer_name.HeaderText = "Customer";
            this.customer_name.MinimumWidth = 6;
            this.customer_name.Name = "customer_name";
            this.customer_name.ReadOnly = true;
            // 
            // total_amount
            // 
            this.total_amount.DataPropertyName = "total_amount";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.total_amount.DefaultCellStyle = dataGridViewCellStyle2;
            this.total_amount.HeaderText = "Total Amount";
            this.total_amount.MinimumWidth = 6;
            this.total_amount.Name = "total_amount";
            this.total_amount.ReadOnly = true;
            // 
            // frm_customerWiseSalesReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 658);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frm_customerWiseSalesReport";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Customer Wise Sales Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.customerWiseSalesReport_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_customerWiseSalesReport_KeyDown);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn customer_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_amount;
    }
}