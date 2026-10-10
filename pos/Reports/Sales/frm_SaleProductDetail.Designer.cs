namespace pos.Reports.Sales
{
    partial class frm_SaleProductDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_SaleProductDetail));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_export = new System.Windows.Forms.Button();
            this.btn_print = new System.Windows.Forms.Button();
            this.lbl_invoice_no = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_products = new System.Windows.Forms.DataGridView();
            this.item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantity_sold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discount_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.profit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_products)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.btn_export);
            this.panel1.Controls.Add(this.btn_print);
            this.panel1.Controls.Add(this.lbl_invoice_no);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Name = "panel1";
            // 
            // btn_export
            // 
            resources.ApplyResources(this.btn_export, "btn_export");
            this.btn_export.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(124)))), ((int)(((byte)(16)))));
            this.btn_export.ForeColor = System.Drawing.Color.White;
            this.btn_export.Name = "btn_export";
            this.btn_export.UseVisualStyleBackColor = false;
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // btn_print
            // 
            resources.ApplyResources(this.btn_print, "btn_print");
            this.btn_print.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btn_print.ForeColor = System.Drawing.Color.White;
            this.btn_print.Name = "btn_print";
            this.btn_print.UseVisualStyleBackColor = false;
            this.btn_print.Click += new System.EventHandler(this.btn_print_Click);
            // 
            // lbl_invoice_no
            // 
            resources.ApplyResources(this.lbl_invoice_no, "lbl_invoice_no");
            this.lbl_invoice_no.Name = "lbl_invoice_no";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.grid_products);
            this.panel2.Name = "panel2";
            // 
            // grid_products
            // 
            resources.ApplyResources(this.grid_products, "grid_products");
            this.grid_products.AllowUserToAddRows = false;
            this.grid_products.AllowUserToDeleteRows = false;
            this.grid_products.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_products.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_products.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.item_code,
            this.product_name,
            this.quantity_sold,
            this.unit_price,
            this.discount_value,
            this.vat,
            this.total,
            this.cost_price,
            this.profit});
            this.grid_products.Name = "grid_products";
            this.grid_products.ReadOnly = true;
            this.grid_products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // item_code
            // 
            this.item_code.DataPropertyName = "item_code";
            this.item_code.FillWeight = 60F;
            resources.ApplyResources(this.item_code, "item_code");
            this.item_code.Name = "item_code";
            this.item_code.ReadOnly = true;
            // 
            // product_name
            // 
            this.product_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.product_name.DataPropertyName = "product_name";
            this.product_name.FillWeight = 150F;
            resources.ApplyResources(this.product_name, "product_name");
            this.product_name.Name = "product_name";
            this.product_name.ReadOnly = true;
            // 
            // quantity_sold
            // 
            this.quantity_sold.DataPropertyName = "quantity_sold";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.Format = "N2";
            this.quantity_sold.DefaultCellStyle = dataGridViewCellStyle7;
            this.quantity_sold.FillWeight = 50F;
            resources.ApplyResources(this.quantity_sold, "quantity_sold");
            this.quantity_sold.Name = "quantity_sold";
            this.quantity_sold.ReadOnly = true;
            // 
            // unit_price
            // 
            this.unit_price.DataPropertyName = "unit_price";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N2";
            this.unit_price.DefaultCellStyle = dataGridViewCellStyle8;
            this.unit_price.FillWeight = 60F;
            resources.ApplyResources(this.unit_price, "unit_price");
            this.unit_price.Name = "unit_price";
            this.unit_price.ReadOnly = true;
            // 
            // discount_value
            // 
            this.discount_value.DataPropertyName = "discount_value";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N2";
            this.discount_value.DefaultCellStyle = dataGridViewCellStyle9;
            this.discount_value.FillWeight = 55F;
            resources.ApplyResources(this.discount_value, "discount_value");
            this.discount_value.Name = "discount_value";
            this.discount_value.ReadOnly = true;
            // 
            // vat
            // 
            this.vat.DataPropertyName = "vat";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N2";
            this.vat.DefaultCellStyle = dataGridViewCellStyle10;
            this.vat.FillWeight = 50F;
            resources.ApplyResources(this.vat, "vat");
            this.vat.Name = "vat";
            this.vat.ReadOnly = true;
            // 
            // total
            // 
            this.total.DataPropertyName = "total";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N2";
            this.total.DefaultCellStyle = dataGridViewCellStyle11;
            this.total.FillWeight = 65F;
            resources.ApplyResources(this.total, "total");
            this.total.Name = "total";
            this.total.ReadOnly = true;
            // 
            // cost_price
            // 
            this.cost_price.DataPropertyName = "cost_price";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle12.Format = "N2";
            this.cost_price.DefaultCellStyle = dataGridViewCellStyle12;
            this.cost_price.FillWeight = 55F;
            resources.ApplyResources(this.cost_price, "cost_price");
            this.cost_price.Name = "cost_price";
            this.cost_price.ReadOnly = true;
            // 
            // profit
            // 
            this.profit.DataPropertyName = "profit";
            this.profit.FillWeight = 55F;
            resources.ApplyResources(this.profit, "profit");
            this.profit.Name = "profit";
            this.profit.ReadOnly = true;
            // 
            // frm_SaleProductDetail
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_SaleProductDetail";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_SaleProductDetail_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_SaleProductDetail_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_products)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_print;
        private System.Windows.Forms.Button btn_export;
        private System.Windows.Forms.Label lbl_invoice_no;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView grid_products;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantity_sold;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn discount_value;
        private System.Windows.Forms.DataGridViewTextBoxColumn vat;
        private System.Windows.Forms.DataGridViewTextBoxColumn total;
        private System.Windows.Forms.DataGridViewTextBoxColumn cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn profit;
    }
}
