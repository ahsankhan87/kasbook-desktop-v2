namespace pos.Discounts
{
    partial class frm_discount_schemes
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_discount_schemes));
            this.panel_header = new System.Windows.Forms.Panel();
            this.lbl_title = new System.Windows.Forms.Label();
            this.panel_body = new System.Windows.Forms.Panel();
            this.grid_schemes = new System.Windows.Forms.DataGridView();
            this.panel_toolbar = new System.Windows.Forms.Panel();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.btn_apply_to_products = new System.Windows.Forms.Button();
            this.btn_new = new System.Windows.Forms.Button();
            this.btn_update = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.btn_toggle = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.col_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_calc_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_is_active = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.col_start_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_end_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel_header.SuspendLayout();
            this.panel_body.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_schemes)).BeginInit();
            this.panel_toolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_header
            // 
            resources.ApplyResources(this.panel_header, "panel_header");
            this.panel_header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel_header.Controls.Add(this.lbl_title);
            this.panel_header.Name = "panel_header";
            // 
            // lbl_title
            // 
            resources.ApplyResources(this.lbl_title, "lbl_title");
            this.lbl_title.ForeColor = System.Drawing.Color.White;
            this.lbl_title.Name = "lbl_title";
            // 
            // panel_body
            // 
            resources.ApplyResources(this.panel_body, "panel_body");
            this.panel_body.BackColor = System.Drawing.Color.White;
            this.panel_body.Controls.Add(this.grid_schemes);
            this.panel_body.Controls.Add(this.panel_toolbar);
            this.panel_body.Name = "panel_body";
            // 
            // grid_schemes
            // 
            resources.ApplyResources(this.grid_schemes, "grid_schemes");
            this.grid_schemes.AllowUserToAddRows = false;
            this.grid_schemes.AllowUserToDeleteRows = false;
            this.grid_schemes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_schemes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_schemes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_id,
            this.col_name,
            this.col_calc_type,
            this.col_value,
            this.col_is_active,
            this.col_start_date,
            this.col_end_date});
            this.grid_schemes.Name = "grid_schemes";
            this.grid_schemes.ReadOnly = true;
            this.grid_schemes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // panel_toolbar
            // 
            resources.ApplyResources(this.panel_toolbar, "panel_toolbar");
            this.panel_toolbar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel_toolbar.Controls.Add(this.txt_search);
            this.panel_toolbar.Controls.Add(this.btn_search);
            this.panel_toolbar.Controls.Add(this.btn_apply_to_products);
            this.panel_toolbar.Controls.Add(this.btn_new);
            this.panel_toolbar.Controls.Add(this.btn_update);
            this.panel_toolbar.Controls.Add(this.btn_delete);
            this.panel_toolbar.Controls.Add(this.btn_toggle);
            this.panel_toolbar.Controls.Add(this.btn_refresh);
            this.panel_toolbar.Name = "panel_toolbar";
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
            // btn_apply_to_products
            // 
            resources.ApplyResources(this.btn_apply_to_products, "btn_apply_to_products");
            this.btn_apply_to_products.Name = "btn_apply_to_products";
            this.btn_apply_to_products.UseVisualStyleBackColor = true;
            this.btn_apply_to_products.Click += new System.EventHandler(this.btn_apply_to_products_Click);
            // 
            // btn_new
            // 
            resources.ApplyResources(this.btn_new, "btn_new");
            this.btn_new.Name = "btn_new";
            this.btn_new.UseVisualStyleBackColor = true;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // btn_update
            // 
            resources.ApplyResources(this.btn_update, "btn_update");
            this.btn_update.Name = "btn_update";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // btn_delete
            // 
            resources.ApplyResources(this.btn_delete, "btn_delete");
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_toggle
            // 
            resources.ApplyResources(this.btn_toggle, "btn_toggle");
            this.btn_toggle.Name = "btn_toggle";
            this.btn_toggle.UseVisualStyleBackColor = true;
            this.btn_toggle.Click += new System.EventHandler(this.btn_toggle_Click);
            // 
            // btn_refresh
            // 
            resources.ApplyResources(this.btn_refresh, "btn_refresh");
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // col_id
            // 
            this.col_id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.col_id.DataPropertyName = "id";
            resources.ApplyResources(this.col_id, "col_id");
            this.col_id.Name = "col_id";
            this.col_id.ReadOnly = true;
            // 
            // col_name
            // 
            this.col_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_name.DataPropertyName = "name";
            resources.ApplyResources(this.col_name, "col_name");
            this.col_name.Name = "col_name";
            this.col_name.ReadOnly = true;
            // 
            // col_calc_type
            // 
            this.col_calc_type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_calc_type.DataPropertyName = "calc_type";
            resources.ApplyResources(this.col_calc_type, "col_calc_type");
            this.col_calc_type.Name = "col_calc_type";
            this.col_calc_type.ReadOnly = true;
            // 
            // col_value
            // 
            this.col_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_value.DataPropertyName = "value";
            resources.ApplyResources(this.col_value, "col_value");
            this.col_value.Name = "col_value";
            this.col_value.ReadOnly = true;
            // 
            // col_is_active
            // 
            this.col_is_active.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_is_active.DataPropertyName = "is_active";
            resources.ApplyResources(this.col_is_active, "col_is_active");
            this.col_is_active.Name = "col_is_active";
            this.col_is_active.ReadOnly = true;
            // 
            // col_start_date
            // 
            this.col_start_date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_start_date.DataPropertyName = "start_date";
            resources.ApplyResources(this.col_start_date, "col_start_date");
            this.col_start_date.Name = "col_start_date";
            this.col_start_date.ReadOnly = true;
            // 
            // col_end_date
            // 
            this.col_end_date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_end_date.DataPropertyName = "end_date";
            resources.ApplyResources(this.col_end_date, "col_end_date");
            this.col_end_date.Name = "col_end_date";
            this.col_end_date.ReadOnly = true;
            // 
            // frm_discount_schemes
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_body);
            this.Controls.Add(this.panel_header);
            this.Name = "frm_discount_schemes";
            this.Load += new System.EventHandler(this.frm_discount_schemes_Load);
            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            this.panel_body.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_schemes)).EndInit();
            this.panel_toolbar.ResumeLayout(false);
            this.panel_toolbar.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panel_header;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Panel panel_body;
        private System.Windows.Forms.Panel panel_toolbar;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btn_toggle;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.Button btn_apply_to_products;
        private System.Windows.Forms.DataGridView grid_schemes;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_calc_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_value;
        private System.Windows.Forms.DataGridViewCheckBoxColumn col_is_active;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_start_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_end_date;
    }
}
