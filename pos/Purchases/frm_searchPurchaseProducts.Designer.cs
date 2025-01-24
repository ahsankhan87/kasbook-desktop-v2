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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid_search_products = new System.Windows.Forms.DataGridView();
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
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.avg_cost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.location_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.group_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alternate_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.alternate_no});
            this.grid_search_products.Name = "grid_search_products";
            this.grid_search_products.ReadOnly = true;
            this.grid_search_products.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_search_products.SelectionChanged += new System.EventHandler(this.grid_search_products_SelectionChanged);
            this.grid_search_products.DoubleClick += new System.EventHandler(this.grid_search_products_DoubleClick);
            this.grid_search_products.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_search_products_KeyDown);
            // 
            // panel1
            // 
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
            this.g_category});
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
            resources.ApplyResources(this.g_qty, "g_qty");
            this.g_qty.Name = "g_qty";
            this.g_qty.ReadOnly = true;
            // 
            // g_avg_cost
            // 
            this.g_avg_cost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.g_avg_cost.DataPropertyName = "avg_cost";
            resources.ApplyResources(this.g_avg_cost, "g_avg_cost");
            this.g_avg_cost.Name = "g_avg_cost";
            this.g_avg_cost.ReadOnly = true;
            // 
            // g_location_code
            // 
            this.g_location_code.DataPropertyName = "location_code";
            resources.ApplyResources(this.g_location_code, "g_location_code");
            this.g_location_code.Name = "g_location_code";
            this.g_location_code.ReadOnly = true;
            // 
            // g_category
            // 
            this.g_category.DataPropertyName = "category";
            resources.ApplyResources(this.g_category, "g_category");
            this.g_category.Name = "g_category";
            this.g_category.ReadOnly = true;
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.qty.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.qty, "qty");
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            // 
            // avg_cost
            // 
            this.avg_cost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.avg_cost.DataPropertyName = "avg_cost";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.avg_cost.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.avg_cost, "avg_cost");
            this.avg_cost.Name = "avg_cost";
            this.avg_cost.ReadOnly = true;
            // 
            // location_code
            // 
            this.location_code.DataPropertyName = "location_code";
            resources.ApplyResources(this.location_code, "location_code");
            this.location_code.Name = "location_code";
            this.location_code.ReadOnly = true;
            // 
            // category
            // 
            this.category.DataPropertyName = "category";
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
        private System.Windows.Forms.DataGridViewTextBoxColumn g_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_avg_cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_location_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn g_category;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn avg_cost;
        private System.Windows.Forms.DataGridViewTextBoxColumn location_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewTextBoxColumn group_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn alternate_no;
    }
}