namespace pos
{
    partial class frm_alt_products
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_alt_products));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_source_code = new System.Windows.Forms.TextBox();
            this.txt_product_code = new System.Windows.Forms.TextBox();
            this.btn_search_products = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txt_item_code = new System.Windows.Forms.TextBox();
            this.grid_product_groups = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alternate_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.item_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_delete = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_product_groups)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.txt_source_code);
            this.panel1.Controls.Add(this.txt_product_code);
            this.panel1.Controls.Add(this.btn_search_products);
            this.panel1.Controls.Add(this.btn_cancel);
            this.panel1.Controls.Add(this.btn_save);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.txt_item_code);
            this.panel1.Controls.Add(this.grid_product_groups);
            this.panel1.Name = "panel1";
            // 
            // txt_source_code
            // 
            resources.ApplyResources(this.txt_source_code, "txt_source_code");
            this.txt_source_code.Name = "txt_source_code";
            this.txt_source_code.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_source_code_KeyDown);
            this.txt_source_code.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_source_code_KeyUp);
            // 
            // txt_product_code
            // 
            resources.ApplyResources(this.txt_product_code, "txt_product_code");
            this.txt_product_code.Name = "txt_product_code";
            this.txt_product_code.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_product_code_KeyDown);
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // txt_item_code
            // 
            resources.ApplyResources(this.txt_item_code, "txt_item_code");
            this.txt_item_code.Name = "txt_item_code";
            this.txt_item_code.ReadOnly = true;
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
            this.name,
            this.alternate_no,
            this.item_number,
            this.btn_delete});
            this.grid_product_groups.Name = "grid_product_groups";
            this.grid_product_groups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            // 
            // alternate_no
            // 
            this.alternate_no.DataPropertyName = "alternate_no";
            resources.ApplyResources(this.alternate_no, "alternate_no");
            this.alternate_no.Name = "alternate_no";
            // 
            // item_number
            // 
            this.item_number.DataPropertyName = "item_number";
            resources.ApplyResources(this.item_number, "item_number");
            this.item_number.Name = "item_number";
            // 
            // btn_delete
            // 
            this.btn_delete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.btn_delete, "btn_delete");
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btn_delete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // frm_alt_products
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "frm_alt_products";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_alt_products_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_alt_products_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_product_groups)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView grid_product_groups;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txt_item_code;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.TextBox txt_product_code;
        private System.Windows.Forms.Button btn_search_products;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_source_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn alternate_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn item_number;
        private System.Windows.Forms.DataGridViewImageColumn btn_delete;
    }
}

