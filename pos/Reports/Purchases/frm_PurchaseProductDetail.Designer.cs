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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_print = new System.Windows.Forms.Button();
            this.btn_export = new System.Windows.Forms.Button();
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
            this.panel1.Controls.Add(this.btn_export);
            this.panel1.Controls.Add(this.btn_print);
            this.panel1.Controls.Add(this.lbl_invoice_no);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(900, 50);
            this.panel1.TabIndex = 0;
            // 
            // lbl_invoice_no
            // 
            this.lbl_invoice_no.AutoSize = true;
            this.lbl_invoice_no.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_invoice_no.Location = new System.Drawing.Point(125, 15);
            this.lbl_invoice_no.Name = "lbl_invoice_no";
            this.lbl_invoice_no.Size = new System.Drawing.Size(19, 21);
            this.lbl_invoice_no.TabIndex = 1;
            this.lbl_invoice_no.Text = "0";
            // 
            // btn_print
            // 
            this.btn_print.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_print.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btn_print.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_print.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_print.ForeColor = System.Drawing.Color.White;
            this.btn_print.Location = new System.Drawing.Point(680, 10);
            this.btn_print.Name = "btn_print";
            this.btn_print.Size = new System.Drawing.Size(100, 32);
            this.btn_print.TabIndex = 2;
            this.btn_print.Text = "Print (F4)";
            this.btn_print.UseVisualStyleBackColor = false;
            this.btn_print.Click += new System.EventHandler(this.btn_print_Click);
            // 
            // btn_export
            // 
            this.btn_export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_export.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(124)))), ((int)(((byte)(16)))));
            this.btn_export.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_export.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_export.ForeColor = System.Drawing.Color.White;
            this.btn_export.Location = new System.Drawing.Point(788, 10);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(100, 32);
            this.btn_export.TabIndex = 3;
            this.btn_export.Text = "Export";
            this.btn_export.UseVisualStyleBackColor = false;
            this.btn_export.Click += new System.EventHandler(this.btn_export_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Invoice No. :";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid_products);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(900, 450);
            this.panel2.TabIndex = 1;
            // 
            // grid_products
            // 
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
            this.grid_products.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_products.Location = new System.Drawing.Point(0, 0);
            this.grid_products.Name = "grid_products";
            this.grid_products.ReadOnly = true;
            this.grid_products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_products.Size = new System.Drawing.Size(900, 450);
            this.grid_products.TabIndex = 0;
            // 
            // col_item_code
            // 
            this.col_item_code.DataPropertyName = "item_code";
            this.col_item_code.FillWeight = 60F;
            this.col_item_code.HeaderText = "Code";
            this.col_item_code.Name = "col_item_code";
            this.col_item_code.ReadOnly = true;
            // 
            // col_product_name
            // 
            this.col_product_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_product_name.DataPropertyName = "product_name";
            this.col_product_name.FillWeight = 150F;
            this.col_product_name.HeaderText = "Product";
            this.col_product_name.Name = "col_product_name";
            this.col_product_name.ReadOnly = true;
            // 
            // col_quantity
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Format = "N2";
            this.col_quantity.DataPropertyName = "quantity";
            this.col_quantity.DefaultCellStyle = dataGridViewCellStyle1;
            this.col_quantity.FillWeight = 50F;
            this.col_quantity.HeaderText = "Qty";
            this.col_quantity.Name = "col_quantity";
            this.col_quantity.ReadOnly = true;
            // 
            // col_cost_price
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            this.col_cost_price.DataPropertyName = "cost_price";
            this.col_cost_price.DefaultCellStyle = dataGridViewCellStyle2;
            this.col_cost_price.FillWeight = 70F;
            this.col_cost_price.HeaderText = "Cost Price";
            this.col_cost_price.Name = "col_cost_price";
            this.col_cost_price.ReadOnly = true;
            // 
            // col_discount_value
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            this.col_discount_value.DataPropertyName = "discount_value";
            this.col_discount_value.DefaultCellStyle = dataGridViewCellStyle3;
            this.col_discount_value.FillWeight = 55F;
            this.col_discount_value.HeaderText = "Discount";
            this.col_discount_value.Name = "col_discount_value";
            this.col_discount_value.ReadOnly = true;
            // 
            // col_vat
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            this.col_vat.DataPropertyName = "vat";
            this.col_vat.DefaultCellStyle = dataGridViewCellStyle4;
            this.col_vat.FillWeight = 50F;
            this.col_vat.HeaderText = "VAT";
            this.col_vat.Name = "col_vat";
            this.col_vat.ReadOnly = true;
            // 
            // col_total
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            this.col_total.DataPropertyName = "total";
            this.col_total.DefaultCellStyle = dataGridViewCellStyle5;
            this.col_total.FillWeight = 65F;
            this.col_total.HeaderText = "Total";
            this.col_total.Name = "col_total";
            this.col_total.ReadOnly = true;
            // 
            // col_total_with_vat
            // 
            this.col_total_with_vat.DataPropertyName = "total_with_vat";
            this.col_total_with_vat.FillWeight = 70F;
            this.col_total_with_vat.HeaderText = "Total (VAT)";
            this.col_total_with_vat.Name = "col_total_with_vat";
            this.col_total_with_vat.ReadOnly = true;
            // 
            // frm_PurchaseProductDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 500);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_PurchaseProductDetail";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Purchase Product Details";
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
