namespace pos
{
    partial class frm_searchPurchaseProducts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_searchPurchaseProducts));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid_search_products = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avg_cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.location_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alternate_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.rb_by_name = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.rb_by_code = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid_group_products = new System.Windows.Forms.DataGridView();
            this.g_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.g_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.g_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.g_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.g_avg_cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.g_location_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.g_category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alt_item_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbl_pages1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_products)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_group_products)).BeginInit();
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
            this.avg_cost,
            this.location_code,
            this.category,
            this.group_code,
            this.alternate_no,
            this.item_number});
            this.grid_search_products.Name = "grid_search_products";
            this.grid_search_products.ReadOnly = true;
            this.grid_search_products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
            dataGridViewCellStyle25.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.code.DefaultCellStyle = dataGridViewCellStyle25;
            resources.ApplyResources(this.code, "code");
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.DataPropertyName = "name";
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.DefaultCellStyle = dataGridViewCellStyle26;
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qty.DataPropertyName = "qty";
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle27.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle27.Format = "N2";
            dataGridViewCellStyle27.NullValue = null;
            this.qty.DefaultCellStyle = dataGridViewCellStyle27;
            resources.ApplyResources(this.qty, "qty");
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            // 
            // avg_cost
            // 
            this.avg_cost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.avg_cost.DataPropertyName = "avg_cost";
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle28.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle28.Format = "N2";
            dataGridViewCellStyle28.NullValue = null;
            this.avg_cost.DefaultCellStyle = dataGridViewCellStyle28;
            resources.ApplyResources(this.avg_cost, "avg_cost");
            this.avg_cost.Name = "avg_cost";
            this.avg_cost.ReadOnly = true;
            // 
            // location_code
            // 
            this.location_code.DataPropertyName = "location_code";
            dataGridViewCellStyle29.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.location_code.DefaultCellStyle = dataGridViewCellStyle29;
            resources.ApplyResources(this.location_code, "location_code");
            this.location_code.Name = "location_code";
            this.location_code.ReadOnly = true;
            // 
            // category
            // 
            this.category.DataPropertyName = "category";
            dataGridViewCellStyle30.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.category.DefaultCellStyle = dataGridViewCellStyle30;
            resources.ApplyResources(this.category, "category");
            this.category.Name = "category";
            this.category.ReadOnly = true;
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
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lbl_pages1);
            this.panel1.Controls.Add(this.btn_ok);
            this.panel1.Controls.Add(this.btn_cancel);
            this.panel1.Controls.Add(this.rb_by_name);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.rb_by_code);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txt_search);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
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
            // rb_by_name
            // 
            resources.ApplyResources(this.rb_by_name, "rb_by_name");
            this.rb_by_name.Name = "rb_by_name";
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
            this.rb_by_code.Checked = true;
            this.rb_by_code.Name = "rb_by_code";
            this.rb_by_code.TabStop = true;
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
            // panel2
            // 
            this.panel2.Controls.Add(this.grid_group_products);
            this.panel2.Controls.Add(this.label2);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
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
            this.g_avg_cost,
            this.g_location_code,
            this.g_category,
            this.alt_item_number});
            this.grid_group_products.Name = "grid_group_products";
            this.grid_group_products.ReadOnly = true;
            this.grid_group_products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
            dataGridViewCellStyle31.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.g_code.DefaultCellStyle = dataGridViewCellStyle31;
            resources.ApplyResources(this.g_code, "g_code");
            this.g_code.Name = "g_code";
            this.g_code.ReadOnly = true;
            // 
            // g_name
            // 
            this.g_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.g_name.DataPropertyName = "name";
            dataGridViewCellStyle32.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.g_name.DefaultCellStyle = dataGridViewCellStyle32;
            resources.ApplyResources(this.g_name, "g_name");
            this.g_name.Name = "g_name";
            this.g_name.ReadOnly = true;
            // 
            // g_qty
            // 
            this.g_qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.g_qty.DataPropertyName = "qty";
            dataGridViewCellStyle33.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.g_qty.DefaultCellStyle = dataGridViewCellStyle33;
            resources.ApplyResources(this.g_qty, "g_qty");
            this.g_qty.Name = "g_qty";
            this.g_qty.ReadOnly = true;
            // 
            // g_avg_cost
            // 
            this.g_avg_cost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.g_avg_cost.DataPropertyName = "avg_cost";
            dataGridViewCellStyle34.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.g_avg_cost.DefaultCellStyle = dataGridViewCellStyle34;
            resources.ApplyResources(this.g_avg_cost, "g_avg_cost");
            this.g_avg_cost.Name = "g_avg_cost";
            this.g_avg_cost.ReadOnly = true;
            // 
            // g_location_code
            // 
            this.g_location_code.DataPropertyName = "location_code";
            dataGridViewCellStyle35.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.g_location_code.DefaultCellStyle = dataGridViewCellStyle35;
            resources.ApplyResources(this.g_location_code, "g_location_code");
            this.g_location_code.Name = "g_location_code";
            this.g_location_code.ReadOnly = true;
            // 
            // g_category
            // 
            this.g_category.DataPropertyName = "category";
            dataGridViewCellStyle36.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.g_category.DefaultCellStyle = dataGridViewCellStyle36;
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
            // panel3
            // 
            this.panel3.Controls.Add(this.grid_search_products);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // lbl_pages1
            // 
            resources.ApplyResources(this.lbl_pages1, "lbl_pages1");
            this.lbl_pages1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lbl_pages1.Name = "lbl_pages1";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label5.Name = "label5";
            // 
            // frm_searchPurchaseProducts
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_searchPurchaseProducts";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_searchPurchaseProducts_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_searchPurchaseProducts_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grid_search_products)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_group_products)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grid_search_products;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.RadioButton rb_by_name;
        private System.Windows.Forms.RadioButton rb_by_code;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView grid_group_products;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn avg_cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn location_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewTextBoxColumn group_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn alternate_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_avg_cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_location_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_category;
        private System.Windows.Forms.DataGridViewTextBoxColumn alt_item_number;
        private System.Windows.Forms.Label lbl_pages1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
    }
}