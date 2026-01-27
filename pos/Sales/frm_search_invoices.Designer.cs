namespace pos
{
    partial class frm_search_invoices
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_search_invoices));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_from_date = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_condition = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_to_date = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Listbox_method = new System.Windows.Forms.ListBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_search = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_sales_report = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.copyContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customer_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_sales_report)).BeginInit();
            this.copyContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btn_close);
            this.panel1.Controls.Add(this.btn_ok);
            this.panel1.Controls.Add(this.btn_search);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_from_date);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txt_condition);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txt_to_date);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // txt_from_date
            // 
            this.txt_from_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.txt_from_date, "txt_from_date");
            this.txt_from_date.Name = "txt_from_date";
            this.txt_from_date.ValueChanged += new System.EventHandler(this.txt_from_date_ValueChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txt_condition
            // 
            resources.ApplyResources(this.txt_condition, "txt_condition");
            this.txt_condition.Name = "txt_condition";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txt_to_date
            // 
            this.txt_to_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.txt_to_date, "txt_to_date");
            this.txt_to_date.Name = "txt_to_date";
            this.txt_to_date.ValueChanged += new System.EventHandler(this.txt_to_date_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Listbox_method);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // Listbox_method
            // 
            this.Listbox_method.FormattingEnabled = true;
            resources.ApplyResources(this.Listbox_method, "Listbox_method");
            this.Listbox_method.Items.AddRange(new object[] {
            resources.GetString("Listbox_method.Items"),
            resources.GetString("Listbox_method.Items1"),
            resources.GetString("Listbox_method.Items2"),
            resources.GetString("Listbox_method.Items3")});
            this.Listbox_method.Name = "Listbox_method";
            // 
            // btn_close
            // 
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btn_close, "btn_close");
            this.btn_close.Name = "btn_close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_ok
            // 
            resources.ApplyResources(this.btn_ok, "btn_ok");
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_search
            // 
            resources.ApplyResources(this.btn_search, "btn_search");
            this.btn_search.Name = "btn_search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
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
            this.total_amount});
            this.grid_sales_report.Name = "grid_sales_report";
            this.grid_sales_report.ReadOnly = true;
            this.grid_sales_report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_sales_report.DoubleClick += new System.EventHandler(this.grid_sales_report_DoubleClick);
            this.grid_sales_report.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_sales_report_KeyDown);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.FillWeight = 200F;
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // sale_date
            // 
            this.sale_date.DataPropertyName = "sale_date";
            this.sale_date.FillWeight = 54.6067F;
            resources.ApplyResources(this.sale_date, "sale_date");
            this.sale_date.Name = "sale_date";
            this.sale_date.ReadOnly = true;
            // 
            // invoice_no
            // 
            this.invoice_no.ContextMenuStrip = this.copyContextMenuStrip;
            this.invoice_no.DataPropertyName = "invoice_no";
            this.invoice_no.FillWeight = 55.53223F;
            resources.ApplyResources(this.invoice_no, "invoice_no");
            this.invoice_no.Name = "invoice_no";
            this.invoice_no.ReadOnly = true;
            // 
            // copyContextMenuStrip
            // 
            this.copyContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.copyContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.copyContextMenuStrip.Name = "copyContextMenuStrip";
            resources.ApplyResources(this.copyContextMenuStrip, "copyContextMenuStrip");
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // customer_name
            // 
            this.customer_name.DataPropertyName = "customer_name";
            this.customer_name.FillWeight = 54.6067F;
            resources.ApplyResources(this.customer_name, "customer_name");
            this.customer_name.Name = "customer_name";
            this.customer_name.ReadOnly = true;
            // 
            // total_amount
            // 
            this.total_amount.DataPropertyName = "total";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.total_amount.DefaultCellStyle = dataGridViewCellStyle1;
            this.total_amount.FillWeight = 30F;
            resources.ApplyResources(this.total_amount, "total_amount");
            this.total_amount.Name = "total_amount";
            this.total_amount.ReadOnly = true;
            // 
            // frm_search_invoices
            // 
            this.AcceptButton = this.btn_search;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_close;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_search_invoices";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.search_invoices_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_search_invoices_KeyDown);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_sales_report)).EndInit();
            this.copyContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView grid_sales_report;
        private System.Windows.Forms.DateTimePicker txt_to_date;
        private System.Windows.Forms.DateTimePicker txt_from_date;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txt_condition;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox Listbox_method;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.ContextMenuStrip copyContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn customer_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_amount;
    }
}