namespace pos
{
    partial class frm_all_estimates
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_all_estimates));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSendWhatsApp = new System.Windows.Forms.Button();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.btn_print_invoice = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.grid_all_estimates = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_taxes_title = new System.Windows.Forms.Label();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invoice_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customer_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sale_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discount_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total_tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.detail = new System.Windows.Forms.DataGridViewImageColumn();
            this.btn_delete = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_all_estimates)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnSendWhatsApp);
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.btn_print_invoice);
            this.panel1.Controls.Add(this.btn_refresh);
            this.panel1.Controls.Add(this.grid_all_estimates);
            this.panel1.Name = "panel1";
            // 
            // btnSendWhatsApp
            // 
            resources.ApplyResources(this.btnSendWhatsApp, "btnSendWhatsApp");
            this.btnSendWhatsApp.BackColor = System.Drawing.Color.Green;
            this.btnSendWhatsApp.ForeColor = System.Drawing.SystemColors.Control;
            this.btnSendWhatsApp.Name = "btnSendWhatsApp";
            this.btnSendWhatsApp.UseVisualStyleBackColor = false;
            this.btnSendWhatsApp.Click += new System.EventHandler(this.btnSendWhatsApp_Click);
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
            // grid_all_estimates
            // 
            resources.ApplyResources(this.grid_all_estimates, "grid_all_estimates");
            this.grid_all_estimates.AllowUserToAddRows = false;
            this.grid_all_estimates.AllowUserToDeleteRows = false;
            this.grid_all_estimates.AllowUserToOrderColumns = true;
            this.grid_all_estimates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_all_estimates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_all_estimates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.invoice_no,
            this.customer_name,
            this.sale_date,
            this.sale_type,
            this.discount_value,
            this.total_tax,
            this.total,
            this.status,
            this.detail,
            this.btn_delete});
            this.grid_all_estimates.Name = "grid_all_estimates";
            this.grid_all_estimates.ReadOnly = true;
            this.grid_all_estimates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_all_estimates.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_all_estimates_CellContentClick);
            this.grid_all_estimates.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_all_estimates_CellDoubleClick);
            this.grid_all_estimates.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_all_estimates_KeyDown);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel2.Controls.Add(this.lbl_taxes_title);
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
            this.invoice_no.DataPropertyName = "invoice_no";
            resources.ApplyResources(this.invoice_no, "invoice_no");
            this.invoice_no.Name = "invoice_no";
            this.invoice_no.ReadOnly = true;
            // 
            // customer_name
            // 
            this.customer_name.DataPropertyName = "customer";
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
            // discount_value
            // 
            this.discount_value.DataPropertyName = "discount_value";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            this.discount_value.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.discount_value, "discount_value");
            this.discount_value.Name = "discount_value";
            this.discount_value.ReadOnly = true;
            // 
            // total_tax
            // 
            this.total_tax.DataPropertyName = "total_tax";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N2";
            this.total_tax.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.total_tax, "total_tax");
            this.total_tax.Name = "total_tax";
            this.total_tax.ReadOnly = true;
            // 
            // total
            // 
            this.total.DataPropertyName = "total";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N2";
            this.total.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.total, "total");
            this.total.Name = "total";
            this.total.ReadOnly = true;
            // 
            // status
            // 
            this.status.DataPropertyName = "status";
            resources.ApplyResources(this.status, "status");
            this.status.Name = "status";
            this.status.ReadOnly = true;
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
            // frm_all_estimates
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.KeyPreview = true;
            this.Name = "frm_all_estimates";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_all_estimates_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_all_estimates_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_all_estimates)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView grid_all_estimates;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbl_taxes_title;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Button btn_print_invoice;
        private System.Windows.Forms.Button btnSendWhatsApp;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn invoice_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn customer_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn sale_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn discount_value;
        private System.Windows.Forms.DataGridViewTextBoxColumn total_tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewImageColumn detail;
        private System.Windows.Forms.DataGridViewImageColumn btn_delete;
    }
}

