namespace pos
{
    partial class frm_all_purchases_orders
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_all_purchases_orders));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_print_invoice = new System.Windows.Forms.Button();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.grid_all_purchases_orders = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_taxes_title = new System.Windows.Forms.Label();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.delivery_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.purchase_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discount_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.detail = new System.Windows.Forms.DataGridViewImageColumn();
            this.delete = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_all_purchases_orders)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btn_print_invoice);
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.btn_refresh);
            this.panel1.Controls.Add(this.grid_all_purchases_orders);
            this.panel1.Name = "panel1";
            // 
            // btn_print_invoice
            // 
            resources.ApplyResources(this.btn_print_invoice, "btn_print_invoice");
            this.btn_print_invoice.Name = "btn_print_invoice";
            this.btn_print_invoice.UseVisualStyleBackColor = true;
            this.btn_print_invoice.Click += new System.EventHandler(this.btn_print_invoice_Click);
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
            // btn_refresh
            // 
            resources.ApplyResources(this.btn_refresh, "btn_refresh");
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // grid_all_purchases_orders
            // 
            this.grid_all_purchases_orders.AllowUserToAddRows = false;
            this.grid_all_purchases_orders.AllowUserToDeleteRows = false;
            this.grid_all_purchases_orders.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.grid_all_purchases_orders, "grid_all_purchases_orders");
            this.grid_all_purchases_orders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_all_purchases_orders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_all_purchases_orders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.invoice_no,
            this.supplier_name,
            this.purchase_date,
            this.delivery_date,
            this.purchase_type,
            this.discount_value,
            this.total_tax,
            this.total_amount,
            this.detail,
            this.delete});
            this.grid_all_purchases_orders.Name = "grid_all_purchases_orders";
            this.grid_all_purchases_orders.ReadOnly = true;
            this.grid_all_purchases_orders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_all_purchases_orders.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_all_purchases_orders_CellContentClick);
            this.grid_all_purchases_orders.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_all_purchases_orders_CellDoubleClick);
            this.grid_all_purchases_orders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_all_purchases_orders_KeyDown);
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
            this.id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // invoice_no
            // 
            this.invoice_no.DataPropertyName = "invoice_no";
            resources.ApplyResources(this.invoice_no, "invoice_no");
            this.invoice_no.Name = "invoice_no";
            this.invoice_no.ReadOnly = true;
            // 
            // supplier_name
            // 
            this.supplier_name.DataPropertyName = "supplier_name";
            resources.ApplyResources(this.supplier_name, "supplier_name");
            this.supplier_name.Name = "supplier_name";
            this.supplier_name.ReadOnly = true;
            // 
            // purchase_date
            // 
            this.purchase_date.DataPropertyName = "purchase_date";
            resources.ApplyResources(this.purchase_date, "purchase_date");
            this.purchase_date.Name = "purchase_date";
            this.purchase_date.ReadOnly = true;
            // 
            // delivery_date
            // 
            this.delivery_date.DataPropertyName = "delivery_date";
            resources.ApplyResources(this.delivery_date, "delivery_date");
            this.delivery_date.Name = "delivery_date";
            this.delivery_date.ReadOnly = true;
            // 
            // purchase_type
            // 
            this.purchase_type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.purchase_type.DataPropertyName = "purchase_type";
            resources.ApplyResources(this.purchase_type, "purchase_type");
            this.purchase_type.Name = "purchase_type";
            this.purchase_type.ReadOnly = true;
            // 
            // discount_value
            // 
            this.discount_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.discount_value.DataPropertyName = "discount_value";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.discount_value.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.discount_value, "discount_value");
            this.discount_value.Name = "discount_value";
            this.discount_value.ReadOnly = true;
            // 
            // total_tax
            // 
            this.total_tax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.total_tax.DataPropertyName = "total_tax";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.total_tax.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.total_tax, "total_tax");
            this.total_tax.Name = "total_tax";
            this.total_tax.ReadOnly = true;
            // 
            // total_amount
            // 
            this.total_amount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.total_amount.DataPropertyName = "total_amount";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.total_amount.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.total_amount, "total_amount");
            this.total_amount.Name = "total_amount";
            this.total_amount.ReadOnly = true;
            // 
            // detail
            // 
            this.detail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.detail, "detail");
            this.detail.Image = global::pos.Properties.Resources.Detail_16;
            this.detail.Name = "detail";
            this.detail.ReadOnly = true;
            // 
            // delete
            // 
            this.delete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.delete, "delete");
            this.delete.Image = global::pos.Properties.Resources.Trash_16;
            this.delete.Name = "delete";
            this.delete.ReadOnly = true;
            // 
            // frm_all_purchases_orders
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.KeyPreview = true;
            this.Name = "frm_all_purchases_orders";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_all_purchases_orders_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_all_purchases_orders_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_all_purchases_orders)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView grid_all_purchases_orders;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbl_taxes_title;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Button btn_print_invoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn delivery_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchase_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn discount_value;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_amount;
        private System.Windows.Forms.DataGridViewImageColumn detail;
        private System.Windows.Forms.DataGridViewImageColumn delete;
    }
}

