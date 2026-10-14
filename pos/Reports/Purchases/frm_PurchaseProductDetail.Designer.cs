namespace pos.Reports.Purchases
{
    partial class frm_PurchaseProductDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_PurchaseProductDetail));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_export = new System.Windows.Forms.Button();
            this.btn_print = new System.Windows.Forms.Button();
            this.lbl_invoice_no = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_products = new System.Windows.Forms.DataGridView();
            this.col_item_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_product_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_discount_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_vat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_total_with_vat = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.col_item_code,
            this.col_product_name,
            this.col_quantity,
            this.col_cost_price,
            this.col_discount_value,
            this.col_vat,
            this.col_total,
            this.col_total_with_vat});
            this.grid_products.Name = "grid_products";
            this.grid_products.ReadOnly = true;
            this.grid_products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // col_item_code
            // 
            this.col_item_code.DataPropertyName = "item_code";
            this.col_item_code.FillWeight = 60F;
            resources.ApplyResources(this.col_item_code, "col_item_code");
            this.col_item_code.Name = "col_item_code";
            this.col_item_code.ReadOnly = true;
            // 
            // col_product_name
            // 
            this.col_product_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_product_name.DataPropertyName = "product_name";
            this.col_product_name.FillWeight = 150F;
            resources.ApplyResources(this.col_product_name, "col_product_name");
            this.col_product_name.Name = "col_product_name";
            this.col_product_name.ReadOnly = true;
            // 
            // col_quantity
            // 
            this.col_quantity.DataPropertyName = "quantity";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Format = "N2";
            this.col_quantity.DefaultCellStyle = dataGridViewCellStyle6;
            this.col_quantity.FillWeight = 50F;
            resources.ApplyResources(this.col_quantity, "col_quantity");
            this.col_quantity.Name = "col_quantity";
            this.col_quantity.ReadOnly = true;
            // 
            // col_cost_price
            // 
            this.col_cost_price.DataPropertyName = "cost_price";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            this.col_cost_price.DefaultCellStyle = dataGridViewCellStyle7;
            this.col_cost_price.FillWeight = 70F;
            resources.ApplyResources(this.col_cost_price, "col_cost_price");
            this.col_cost_price.Name = "col_cost_price";
            this.col_cost_price.ReadOnly = true;
            // 
            // col_discount_value
            // 
            this.col_discount_value.DataPropertyName = "discount_value";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N2";
            this.col_discount_value.DefaultCellStyle = dataGridViewCellStyle8;
            this.col_discount_value.FillWeight = 55F;
            resources.ApplyResources(this.col_discount_value, "col_discount_value");
            this.col_discount_value.Name = "col_discount_value";
            this.col_discount_value.ReadOnly = true;
            // 
            // col_vat
            // 
            this.col_vat.DataPropertyName = "vat";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N2";
            this.col_vat.DefaultCellStyle = dataGridViewCellStyle9;
            this.col_vat.FillWeight = 50F;
            resources.ApplyResources(this.col_vat, "col_vat");
            this.col_vat.Name = "col_vat";
            this.col_vat.ReadOnly = true;
            // 
            // col_total
            // 
            this.col_total.DataPropertyName = "total";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N2";
            this.col_total.DefaultCellStyle = dataGridViewCellStyle10;
            this.col_total.FillWeight = 65F;
            resources.ApplyResources(this.col_total, "col_total");
            this.col_total.Name = "col_total";
            this.col_total.ReadOnly = true;
            // 
            // col_total_with_vat
            // 
            this.col_total_with_vat.DataPropertyName = "total_with_vat";
            this.col_total_with_vat.FillWeight = 70F;
            resources.ApplyResources(this.col_total_with_vat, "col_total_with_vat");
            this.col_total_with_vat.Name = "col_total_with_vat";
            this.col_total_with_vat.ReadOnly = true;
            // 
            // frm_PurchaseProductDetail
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_PurchaseProductDetail";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_PurchaseProductDetail_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_PurchaseProductDetail_KeyDown);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn col_item_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_product_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_cost_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_discount_value;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_vat;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_total;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_total_with_vat;
    }
}
