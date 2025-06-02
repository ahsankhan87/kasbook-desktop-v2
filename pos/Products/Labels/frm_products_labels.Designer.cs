namespace pos
{
    partial class frm_products_labels
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_products_labels));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_purchase_inv_no = new System.Windows.Forms.TextBox();
            this.txt_product_code = new System.Windows.Forms.TextBox();
            this.btn_invoice_search = new System.Windows.Forms.Button();
            this.btn_search_products = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grid_product_groups = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.location_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_product_groups)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.txt_purchase_inv_no);
            this.panel1.Controls.Add(this.txt_product_code);
            this.panel1.Controls.Add(this.btn_invoice_search);
            this.panel1.Controls.Add(this.btn_search_products);
            this.panel1.Controls.Add(this.btn_cancel);
            this.panel1.Controls.Add(this.btn_save);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.grid_product_groups);
            this.panel1.Name = "panel1";
            // 
            // txt_purchase_inv_no
            // 
            resources.ApplyResources(this.txt_purchase_inv_no, "txt_purchase_inv_no");
            this.txt_purchase_inv_no.Name = "txt_purchase_inv_no";
            this.txt_purchase_inv_no.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_purchase_inv_no_KeyDown);
            // 
            // txt_product_code
            // 
            resources.ApplyResources(this.txt_product_code, "txt_product_code");
            this.txt_product_code.Name = "txt_product_code";
            this.txt_product_code.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_product_code_KeyDown);
            // 
            // btn_invoice_search
            // 
            resources.ApplyResources(this.btn_invoice_search, "btn_invoice_search");
            this.btn_invoice_search.Name = "btn_invoice_search";
            this.btn_invoice_search.UseVisualStyleBackColor = true;
            this.btn_invoice_search.Click += new System.EventHandler(this.btn_invoice_search_Click);
            // 
            // btn_search_products
            // 
            resources.ApplyResources(this.btn_search_products, "btn_search_products");
            this.btn_search_products.Name = "btn_search_products";
            this.btn_search_products.UseVisualStyleBackColor = true;
            this.btn_search_products.Click += new System.EventHandler(this.btn_search_products_Click);
            // 
            // btn_cancel
            // 
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_save
            // 
            resources.ApplyResources(this.btn_save, "btn_save");
            this.btn_save.Name = "btn_save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // grid_product_groups
            // 
            resources.ApplyResources(this.grid_product_groups, "grid_product_groups");
            this.grid_product_groups.AllowUserToAddRows = false;
            this.grid_product_groups.AllowUserToDeleteRows = false;
            this.grid_product_groups.AllowUserToOrderColumns = true;
            this.grid_product_groups.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_product_groups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_product_groups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.code,
            this.category,
            this.name,
            this.qty,
            this.unit_price,
            this.location_code,
            this.label_qty,
            this.barcode});
            this.grid_product_groups.Name = "grid_product_groups";
            this.grid_product_groups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grid_product_groups.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_product_groups_CellContentClick);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            // 
            // code
            // 
            this.code.DataPropertyName = "code";
            resources.ApplyResources(this.code, "code");
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // category
            // 
            this.category.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.category.DataPropertyName = "category";
            resources.ApplyResources(this.category, "category");
            this.category.Name = "category";
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // qty
            // 
            this.qty.DataPropertyName = "qty";
            resources.ApplyResources(this.qty, "qty");
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            // 
            // unit_price
            // 
            this.unit_price.DataPropertyName = "unit_price";
            resources.ApplyResources(this.unit_price, "unit_price");
            this.unit_price.Name = "unit_price";
            this.unit_price.ReadOnly = true;
            // 
            // location_code
            // 
            this.location_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.location_code.DataPropertyName = "location_code";
            resources.ApplyResources(this.location_code, "location_code");
            this.location_code.Name = "location_code";
            // 
            // label_qty
            // 
            this.label_qty.DataPropertyName = "label_qty";
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label_qty.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.label_qty, "label_qty");
            this.label_qty.Name = "label_qty";
            // 
            // barcode
            // 
            this.barcode.DataPropertyName = "barcode";
            resources.ApplyResources(this.barcode, "barcode");
            this.barcode.Name = "barcode";
            // 
            // frm_products_labels
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_products_labels";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_products_labels_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_products_labels_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_product_groups)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView grid_product_groups;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.TextBox txt_product_code;
        private System.Windows.Forms.Button btn_search_products;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_purchase_inv_no;
        private System.Windows.Forms.Button btn_invoice_search;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn location_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn label_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn barcode;
    }
}

