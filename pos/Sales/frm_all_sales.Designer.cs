namespace pos
{
    partial class frm_all_sales
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_all_sales));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnUBLInvoice = new System.Windows.Forms.Button();
            this.BtnCustomerNameChange = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.btn_print_invoice = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.grid_all_sales = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_taxes_title = new System.Windows.Forms.Label();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customer_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discount_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.detail = new System.Windows.Forms.DataGridViewImageColumn();
            this.btn_delete = new System.Windows.Forms.DataGridViewImageColumn();
            this.sale_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zetca_qrcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_all_sales)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.BtnUBLInvoice);
            this.panel1.Controls.Add(this.BtnCustomerNameChange);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.btn_print_invoice);
            this.panel1.Controls.Add(this.btn_refresh);
            this.panel1.Controls.Add(this.grid_all_sales);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnUBLInvoice
            // 
            resources.ApplyResources(this.BtnUBLInvoice, "BtnUBLInvoice");
            this.BtnUBLInvoice.Name = "BtnUBLInvoice";
            this.BtnUBLInvoice.UseVisualStyleBackColor = true;
            this.BtnUBLInvoice.Click += new System.EventHandler(this.BtnUBLInvoice_Click);
            // 
            // BtnCustomerNameChange
            // 
            resources.ApplyResources(this.BtnCustomerNameChange, "BtnCustomerNameChange");
            this.BtnCustomerNameChange.Name = "BtnCustomerNameChange";
            this.BtnCustomerNameChange.UseVisualStyleBackColor = true;
            this.BtnCustomerNameChange.Click += new System.EventHandler(this.BtnCustomerNameChange_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txt_search
            // 
            resources.ApplyResources(this.txt_search, "txt_search");
            this.txt_search.Name = "txt_search";
            this.txt_search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_search_KeyPress);
            // 
            // btn_search
            // 
            resources.ApplyResources(this.btn_search, "btn_search");
            this.btn_search.Name = "btn_search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_print_invoice
            // 
            resources.ApplyResources(this.btn_print_invoice, "btn_print_invoice");
            this.btn_print_invoice.Name = "btn_print_invoice";
            this.btn_print_invoice.UseVisualStyleBackColor = true;
            this.btn_print_invoice.Click += new System.EventHandler(this.btn_print_invoice_Click);
            // 
            // btn_refresh
            // 
            resources.ApplyResources(this.btn_refresh, "btn_refresh");
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // grid_all_sales
            // 
            this.grid_all_sales.AllowUserToAddRows = false;
            this.grid_all_sales.AllowUserToDeleteRows = false;
            this.grid_all_sales.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.grid_all_sales, "grid_all_sales");
            this.grid_all_sales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_all_sales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_all_sales.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.invoice_no,
            this.customer_name,
            this.sale_date,
            this.sale_type,
            this.account,
            this.discount_value,
            this.total_tax,
            this.total,
            this.detail,
            this.btn_delete,
            this.sale_time,
            this.Zetca_qrcode});
            this.grid_all_sales.Name = "grid_all_sales";
            this.grid_all_sales.ReadOnly = true;
            this.grid_all_sales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_all_sales.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_all_sales_CellContentClick);
            this.grid_all_sales.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_all_sales_CellDoubleClick);
            this.grid_all_sales.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_all_sales_KeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel2.Controls.Add(this.lbl_taxes_title);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // lbl_taxes_title
            // 
            resources.ApplyResources(this.lbl_taxes_title, "lbl_taxes_title");
            this.lbl_taxes_title.ForeColor = System.Drawing.Color.White;
            this.lbl_taxes_title.Name = "lbl_taxes_title";
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // invoice_no
            // 
            this.invoice_no.ContextMenuStrip = this.contextMenuStrip1;
            this.invoice_no.DataPropertyName = "invoice_no";
            resources.ApplyResources(this.invoice_no, "invoice_no");
            this.invoice_no.Name = "invoice_no";
            this.invoice_no.ReadOnly = true;
            // 
            // customer_name
            // 
            this.customer_name.DataPropertyName = "customer_name";
            resources.ApplyResources(this.customer_name, "customer_name");
            this.customer_name.Name = "customer_name";
            this.customer_name.ReadOnly = true;
            // 
            // sale_date
            // 
            this.sale_date.DataPropertyName = "sale_date";
            resources.ApplyResources(this.sale_date, "sale_date");
            this.sale_date.Name = "sale_date";
            this.sale_date.ReadOnly = true;
            // 
            // sale_type
            // 
            this.sale_type.DataPropertyName = "sale_type";
            resources.ApplyResources(this.sale_type, "sale_type");
            this.sale_type.Name = "sale_type";
            this.sale_type.ReadOnly = true;
            // 
            // account
            // 
            this.account.DataPropertyName = "account";
            resources.ApplyResources(this.account, "account");
            this.account.Name = "account";
            this.account.ReadOnly = true;
            // 
            // discount_value
            // 
            this.discount_value.DataPropertyName = "discount_value";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            this.discount_value.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.discount_value, "discount_value");
            this.discount_value.Name = "discount_value";
            this.discount_value.ReadOnly = true;
            // 
            // total_tax
            // 
            this.total_tax.DataPropertyName = "total_tax";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            this.total_tax.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.total_tax, "total_tax");
            this.total_tax.Name = "total_tax";
            this.total_tax.ReadOnly = true;
            // 
            // total
            // 
            this.total.DataPropertyName = "total";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            this.total.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.total, "total");
            this.total.Name = "total";
            this.total.ReadOnly = true;
            // 
            // detail
            // 
            this.detail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.detail, "detail");
            this.detail.Image = global::pos.Properties.Resources.Detail_16;
            this.detail.Name = "detail";
            this.detail.ReadOnly = true;
            // 
            // btn_delete
            // 
            this.btn_delete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.btn_delete, "btn_delete");
            this.btn_delete.Image = global::pos.Properties.Resources.Trash_16;
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.ReadOnly = true;
            // 
            // sale_time
            // 
            this.sale_time.DataPropertyName = "sale_time";
            resources.ApplyResources(this.sale_time, "sale_time");
            this.sale_time.Name = "sale_time";
            this.sale_time.ReadOnly = true;
            // 
            // Zetca_qrcode
            // 
            this.Zetca_qrcode.DataPropertyName = "Zetca_qrcode";
            resources.ApplyResources(this.Zetca_qrcode, "Zetca_qrcode");
            this.Zetca_qrcode.Name = "Zetca_qrcode";
            this.Zetca_qrcode.ReadOnly = true;
            // 
            // frm_all_sales
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.KeyPreview = true;
            this.Name = "frm_all_sales";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_all_sales_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_all_sales_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_all_sales)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView grid_all_sales;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbl_taxes_title;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Button btn_print_invoice;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnCustomerNameChange;
        private System.Windows.Forms.Button BtnUBLInvoice;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn customer_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn account;
        private System.Windows.Forms.DataGridViewTextBoxColumn discount_value;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewImageColumn detail;
        private System.Windows.Forms.DataGridViewImageColumn btn_delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zetca_qrcode;
    }
}

