﻿namespace pos
{
    partial class frm_searchSaleProducts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_searchSaleProducts));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid_search_products = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.location_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alternate_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rb_by_name = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.rb_by_code = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.grid_group_products = new System.Windows.Forms.DataGridView();
            this.g_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.g_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.g_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.g_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.g_unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.g_location_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.g_category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alt_item_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_other_stock = new System.Windows.Forms.DataGridView();
            this.branch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.branch_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_products)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_group_products)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_other_stock)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid_search_products
            // 
            this.grid_search_products.AllowUserToAddRows = false;
            this.grid_search_products.AllowUserToDeleteRows = false;
            this.grid_search_products.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.grid_search_products, "grid_search_products");
            this.grid_search_products.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_search_products.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.code,
            this.name,
            this.qty,
            this.unit_price,
            this.location_code,
            this.category,
            this.description,
            this.group_code,
            this.alternate_no,
            this.item_number});
            this.grid_search_products.Name = "grid_search_products";
            this.grid_search_products.ReadOnly = true;
            this.grid_search_products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_search_products.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid_search_products_CellFormatting);
            this.grid_search_products.SelectionChanged += new System.EventHandler(this.grid_search_products_SelectionChanged);
            this.grid_search_products.DoubleClick += new System.EventHandler(this.grid_search_products_DoubleClick);
            this.grid_search_products.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_search_products_KeyDown);
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // code
            // 
            this.code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.code.DataPropertyName = "code";
            resources.ApplyResources(this.code, "code");
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.DataPropertyName = "name";
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qty.DataPropertyName = "qty";
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.qty.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.qty, "qty");
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            // 
            // unit_price
            // 
            this.unit_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.unit_price.DataPropertyName = "unit_price";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.unit_price.DefaultCellStyle = dataGridViewCellStyle2;
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
            this.location_code.ReadOnly = true;
            // 
            // category
            // 
            this.category.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.category.DataPropertyName = "category";
            resources.ApplyResources(this.category, "category");
            this.category.Name = "category";
            this.category.ReadOnly = true;
            // 
            // description
            // 
            this.description.DataPropertyName = "description";
            resources.ApplyResources(this.description, "description");
            this.description.Name = "description";
            this.description.ReadOnly = true;
            // 
            // group_code
            // 
            this.group_code.DataPropertyName = "group_code";
            resources.ApplyResources(this.group_code, "group_code");
            this.group_code.Name = "group_code";
            this.group_code.ReadOnly = true;
            // 
            // alternate_no
            // 
            this.alternate_no.DataPropertyName = "alt_no";
            resources.ApplyResources(this.alternate_no, "alternate_no");
            this.alternate_no.Name = "alternate_no";
            this.alternate_no.ReadOnly = true;
            // 
            // item_number
            // 
            this.item_number.DataPropertyName = "item_number";
            resources.ApplyResources(this.item_number, "item_number");
            this.item_number.Name = "item_number";
            this.item_number.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rb_by_name);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.rb_by_code);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_ok);
            this.panel1.Controls.Add(this.btn_cancel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // rb_by_name
            // 
            resources.ApplyResources(this.rb_by_name, "rb_by_name");
            this.rb_by_name.Checked = true;
            this.rb_by_name.Name = "rb_by_name";
            this.rb_by_name.TabStop = true;
            this.rb_by_name.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // rb_by_code
            // 
            resources.ApplyResources(this.rb_by_code, "rb_by_code");
            this.rb_by_code.Name = "rb_by_code";
            this.rb_by_code.UseVisualStyleBackColor = true;
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
            this.txt_search.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_search_KeyUp);
            // 
            // btn_ok
            // 
            this.btn_ok.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btn_ok, "btn_ok");
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // grid_group_products
            // 
            this.grid_group_products.AllowUserToAddRows = false;
            this.grid_group_products.AllowUserToDeleteRows = false;
            this.grid_group_products.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.grid_group_products, "grid_group_products");
            this.grid_group_products.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_group_products.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.g_id,
            this.g_code,
            this.g_name,
            this.g_qty,
            this.g_unit_price,
            this.g_location_code,
            this.g_category,
            this.alt_item_number});
            this.grid_group_products.Name = "grid_group_products";
            this.grid_group_products.ReadOnly = true;
            this.grid_group_products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_group_products.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid_group_products_CellFormatting);
            this.grid_group_products.DoubleClick += new System.EventHandler(this.grid_group_products_DoubleClick);
            this.grid_group_products.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_group_products_KeyDown);
            // 
            // g_id
            // 
            this.g_id.DataPropertyName = "id";
            resources.ApplyResources(this.g_id, "g_id");
            this.g_id.Name = "g_id";
            this.g_id.ReadOnly = true;
            // 
            // g_code
            // 
            this.g_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.g_code.DataPropertyName = "code";
            resources.ApplyResources(this.g_code, "g_code");
            this.g_code.Name = "g_code";
            this.g_code.ReadOnly = true;
            // 
            // g_name
            // 
            this.g_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.g_name.DataPropertyName = "name";
            resources.ApplyResources(this.g_name, "g_name");
            this.g_name.Name = "g_name";
            this.g_name.ReadOnly = true;
            // 
            // g_qty
            // 
            this.g_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.g_qty.DataPropertyName = "qty";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.g_qty.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.g_qty, "g_qty");
            this.g_qty.Name = "g_qty";
            this.g_qty.ReadOnly = true;
            // 
            // g_unit_price
            // 
            this.g_unit_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.g_unit_price.DataPropertyName = "unit_price";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.g_unit_price.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.g_unit_price, "g_unit_price");
            this.g_unit_price.Name = "g_unit_price";
            this.g_unit_price.ReadOnly = true;
            // 
            // g_location_code
            // 
            this.g_location_code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.g_location_code.DataPropertyName = "location_code";
            resources.ApplyResources(this.g_location_code, "g_location_code");
            this.g_location_code.Name = "g_location_code";
            this.g_location_code.ReadOnly = true;
            // 
            // g_category
            // 
            this.g_category.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.g_category.DataPropertyName = "category";
            resources.ApplyResources(this.g_category, "g_category");
            this.g_category.Name = "g_category";
            this.g_category.ReadOnly = true;
            // 
            // alt_item_number
            // 
            this.alt_item_number.DataPropertyName = "item_number";
            resources.ApplyResources(this.alt_item_number, "alt_item_number");
            this.alt_item_number.Name = "alt_item_number";
            this.alt_item_number.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid_other_stock);
            this.panel2.Controls.Add(this.grid_search_products);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // grid_other_stock
            // 
            this.grid_other_stock.AllowUserToAddRows = false;
            this.grid_other_stock.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.grid_other_stock, "grid_other_stock");
            this.grid_other_stock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_other_stock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.branch,
            this.branch_qty});
            this.grid_other_stock.Name = "grid_other_stock";
            this.grid_other_stock.ReadOnly = true;
            // 
            // branch
            // 
            this.branch.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.branch, "branch");
            this.branch.Name = "branch";
            this.branch.ReadOnly = true;
            // 
            // branch_qty
            // 
            this.branch_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.branch_qty.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.branch_qty, "branch_qty");
            this.branch_qty.Name = "branch_qty";
            this.branch_qty.ReadOnly = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.grid_group_products);
            this.panel3.Controls.Add(this.label2);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // frm_searchSaleProducts
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_searchSaleProducts";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_searchSaleProducts_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_searchSaleProducts_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_products)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_group_products)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_other_stock)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grid_search_products;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.RadioButton rb_by_name;
        private System.Windows.Forms.RadioButton rb_by_code;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView grid_group_products;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView grid_other_stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn branch;
        private System.Windows.Forms.DataGridViewTextBoxColumn branch_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn location_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
        private System.Windows.Forms.DataGridViewTextBoxColumn group_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn alternate_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_location_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_category;
        private System.Windows.Forms.DataGridViewTextBoxColumn alt_item_number;
    }
}