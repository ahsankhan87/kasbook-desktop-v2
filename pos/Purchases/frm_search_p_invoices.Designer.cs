namespace pos
{
    partial class frm_search_p_invoices
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_search_p_invoices));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chk_hold_purchases = new System.Windows.Forms.CheckBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_from_date = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_condition = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_to_date = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Listbox_method = new System.Windows.Forms.ListBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_search = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_sales_report = new System.Windows.Forms.DataGridView();
            this.copy_contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier_invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_sales_report)).BeginInit();
            this.copy_contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.chk_hold_purchases);
            this.panel1.Controls.Add(this.btn_ok);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btn_close);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Name = "panel1";
            // 
            // chk_hold_purchases
            // 
            resources.ApplyResources(this.chk_hold_purchases, "chk_hold_purchases");
            this.chk_hold_purchases.Name = "chk_hold_purchases";
            this.chk_hold_purchases.UseVisualStyleBackColor = true;
            // 
            // btn_ok
            // 
            resources.ApplyResources(this.btn_ok, "btn_ok");
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.txt_from_date);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txt_condition);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txt_to_date);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // txt_from_date
            // 
            resources.ApplyResources(this.txt_from_date, "txt_from_date");
            this.txt_from_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
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
            resources.ApplyResources(this.txt_to_date, "txt_to_date");
            this.txt_to_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txt_to_date.Name = "txt_to_date";
            this.txt_to_date.ValueChanged += new System.EventHandler(this.txt_to_date_ValueChanged);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.Listbox_method);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // Listbox_method
            // 
            resources.ApplyResources(this.Listbox_method, "Listbox_method");
            this.Listbox_method.FormattingEnabled = true;
            this.Listbox_method.Items.AddRange(new object[] {
            resources.GetString("Listbox_method.Items"),
            resources.GetString("Listbox_method.Items1"),
            resources.GetString("Listbox_method.Items2"),
            resources.GetString("Listbox_method.Items3"),
            resources.GetString("Listbox_method.Items4")});
            this.Listbox_method.Name = "Listbox_method";
            // 
            // btn_close
            // 
            resources.ApplyResources(this.btn_close, "btn_close");
            this.btn_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_close.Name = "btn_close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
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
            this.id,
            this.purchase_date,
            this.invoice_no,
            this.supplier_name,
            this.supplier_invoice_no,
            this.total_amount});
            this.grid_sales_report.Name = "grid_sales_report";
            this.grid_sales_report.ReadOnly = true;
            this.grid_sales_report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_sales_report.DoubleClick += new System.EventHandler(this.grid_sales_report_DoubleClick);
            this.grid_sales_report.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_sales_report_KeyDown);
            // 
            // copy_contextMenuStrip
            // 
            resources.ApplyResources(this.copy_contextMenuStrip, "copy_contextMenuStrip");
            this.copy_contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.copy_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.copy_contextMenuStrip.Name = "copy_contextMenuStrip";
            // 
            // copyToolStripMenuItem
            // 
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.FillWeight = 200F;
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // purchase_date
            // 
            this.purchase_date.DataPropertyName = "purchase_date";
            this.purchase_date.FillWeight = 54.6067F;
            resources.ApplyResources(this.purchase_date, "purchase_date");
            this.purchase_date.Name = "purchase_date";
            this.purchase_date.ReadOnly = true;
            // 
            // invoice_no
            // 
            this.invoice_no.ContextMenuStrip = this.copy_contextMenuStrip;
            this.invoice_no.DataPropertyName = "invoice_no";
            this.invoice_no.FillWeight = 55.53223F;
            resources.ApplyResources(this.invoice_no, "invoice_no");
            this.invoice_no.Name = "invoice_no";
            this.invoice_no.ReadOnly = true;
            // 
            // supplier_name
            // 
            this.supplier_name.DataPropertyName = "supplier_name";
            this.supplier_name.FillWeight = 54.6067F;
            resources.ApplyResources(this.supplier_name, "supplier_name");
            this.supplier_name.Name = "supplier_name";
            this.supplier_name.ReadOnly = true;
            // 
            // supplier_invoice_no
            // 
            this.supplier_invoice_no.DataPropertyName = "supplier_invoice_no";
            resources.ApplyResources(this.supplier_invoice_no, "supplier_invoice_no");
            this.supplier_invoice_no.Name = "supplier_invoice_no";
            this.supplier_invoice_no.ReadOnly = true;
            // 
            // total_amount
            // 
            this.total_amount.DataPropertyName = "total";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.total_amount.DefaultCellStyle = dataGridViewCellStyle2;
            this.total_amount.FillWeight = 54.6067F;
            resources.ApplyResources(this.total_amount, "total_amount");
            this.total_amount.Name = "total_amount";
            this.total_amount.ReadOnly = true;
            // 
            // frm_search_p_invoices
            // 
            this.AcceptButton = this.btn_search;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_close;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_search_p_invoices";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.search_p_invoices_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_search_p_invoices_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_sales_report)).EndInit();
            this.copy_contextMenuStrip.ResumeLayout(false);
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
        private System.Windows.Forms.CheckBox chk_hold_purchases;
        private System.Windows.Forms.ContextMenuStrip copy_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_amount;
    }
}