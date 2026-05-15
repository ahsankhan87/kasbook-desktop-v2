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
            this.panel_header = new System.Windows.Forms.Panel();
            this.lbl_title = new System.Windows.Forms.Label();
            this.panel_body = new System.Windows.Forms.Panel();
            this.grid_schemes = new System.Windows.Forms.DataGridView();
            this.panel_toolbar = new System.Windows.Forms.Panel();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
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
            this.panel_header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel_header.Controls.Add(this.lbl_title);
            this.panel_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_header.Location = new System.Drawing.Point(0, 0);
            this.panel_header.Name = "panel_header";
            this.panel_header.Size = new System.Drawing.Size(1100, 48);
            this.panel_header.TabIndex = 1;
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lbl_title.ForeColor = System.Drawing.Color.White;
            this.lbl_title.Location = new System.Drawing.Point(12, 11);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(179, 28);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "Discount Schemes";
            // 
            // panel_body
            // 
            this.panel_body.BackColor = System.Drawing.Color.White;
            this.panel_body.Controls.Add(this.grid_schemes);
            this.panel_body.Controls.Add(this.panel_toolbar);
            this.panel_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_body.Location = new System.Drawing.Point(0, 48);
            this.panel_body.Name = "panel_body";
            this.panel_body.Size = new System.Drawing.Size(1100, 571);
            this.panel_body.TabIndex = 0;
            // 
            // grid_schemes
            // 
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
            this.grid_schemes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_schemes.Location = new System.Drawing.Point(0, 49);
            this.grid_schemes.Name = "grid_schemes";
            this.grid_schemes.ReadOnly = true;
            this.grid_schemes.RowHeadersWidth = 51;
            this.grid_schemes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_schemes.Size = new System.Drawing.Size(1100, 522);
            this.grid_schemes.TabIndex = 0;
            // 
            // panel_toolbar
            // 
            this.panel_toolbar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel_toolbar.Controls.Add(this.txt_search);
            this.panel_toolbar.Controls.Add(this.btn_search);
            this.panel_toolbar.Controls.Add(this.btn_new);
            this.panel_toolbar.Controls.Add(this.btn_update);
            this.panel_toolbar.Controls.Add(this.btn_delete);
            this.panel_toolbar.Controls.Add(this.btn_toggle);
            this.panel_toolbar.Controls.Add(this.btn_refresh);
            this.panel_toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_toolbar.Location = new System.Drawing.Point(0, 0);
            this.panel_toolbar.Name = "panel_toolbar";
            this.panel_toolbar.Size = new System.Drawing.Size(1100, 49);
            this.panel_toolbar.TabIndex = 1;
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(10, 12);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(200, 24);
            this.txt_search.TabIndex = 0;
            this.txt_search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_search_KeyPress);
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(216, 11);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(70, 28);
            this.btn_search.TabIndex = 1;
            this.btn_search.Text = "Search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_new
            // 
            this.btn_new.Location = new System.Drawing.Point(580, 11);
            this.btn_new.Name = "btn_new";
            this.btn_new.Size = new System.Drawing.Size(75, 28);
            this.btn_new.TabIndex = 2;
            this.btn_new.Text = "New";
            this.btn_new.UseVisualStyleBackColor = true;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // btn_update
            // 
            this.btn_update.Location = new System.Drawing.Point(660, 11);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(75, 28);
            this.btn_update.TabIndex = 3;
            this.btn_update.Text = "Edit";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Location = new System.Drawing.Point(740, 11);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(75, 28);
            this.btn_delete.TabIndex = 4;
            this.btn_delete.Text = "Delete";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_toggle
            // 
            this.btn_toggle.Location = new System.Drawing.Point(820, 11);
            this.btn_toggle.Name = "btn_toggle";
            this.btn_toggle.Size = new System.Drawing.Size(100, 28);
            this.btn_toggle.TabIndex = 5;
            this.btn_toggle.Text = "Toggle Active";
            this.btn_toggle.UseVisualStyleBackColor = true;
            this.btn_toggle.Click += new System.EventHandler(this.btn_toggle_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(925, 11);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(75, 28);
            this.btn_refresh.TabIndex = 6;
            this.btn_refresh.Text = "Refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // col_id
            // 
            this.col_id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.col_id.DataPropertyName = "id";
            this.col_id.HeaderText = "ID";
            this.col_id.MinimumWidth = 6;
            this.col_id.Name = "col_id";
            this.col_id.ReadOnly = true;
            this.col_id.Width = 50;
            // 
            // col_name
            // 
            this.col_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col_name.DataPropertyName = "name";
            this.col_name.HeaderText = "Name";
            this.col_name.MinimumWidth = 6;
            this.col_name.Name = "col_name";
            this.col_name.ReadOnly = true;
            // 
            // col_calc_type
            // 
            this.col_calc_type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_calc_type.DataPropertyName = "calc_type";
            this.col_calc_type.HeaderText = "Calc Type";
            this.col_calc_type.MinimumWidth = 6;
            this.col_calc_type.Name = "col_calc_type";
            this.col_calc_type.ReadOnly = true;
            this.col_calc_type.Width = 97;
            // 
            // col_value
            // 
            this.col_value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_value.DataPropertyName = "value";
            this.col_value.HeaderText = "Value";
            this.col_value.MinimumWidth = 6;
            this.col_value.Name = "col_value";
            this.col_value.ReadOnly = true;
            this.col_value.Width = 69;
            // 
            // col_is_active
            // 
            this.col_is_active.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_is_active.DataPropertyName = "is_active";
            this.col_is_active.HeaderText = "Active";
            this.col_is_active.MinimumWidth = 6;
            this.col_is_active.Name = "col_is_active";
            this.col_is_active.ReadOnly = true;
            this.col_is_active.Width = 51;
            // 
            // col_start_date
            // 
            this.col_start_date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_start_date.DataPropertyName = "start_date";
            this.col_start_date.HeaderText = "Start Date";
            this.col_start_date.MinimumWidth = 6;
            this.col_start_date.Name = "col_start_date";
            this.col_start_date.ReadOnly = true;
            // 
            // col_end_date
            // 
            this.col_end_date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.col_end_date.DataPropertyName = "end_date";
            this.col_end_date.HeaderText = "End Date";
            this.col_end_date.MinimumWidth = 6;
            this.col_end_date.Name = "col_end_date";
            this.col_end_date.ReadOnly = true;
            this.col_end_date.Width = 94;
            // 
            // frm_discount_schemes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 619);
            this.Controls.Add(this.panel_body);
            this.Controls.Add(this.panel_header);
            this.Name = "frm_discount_schemes";
            this.Text = "Discount Schemes";
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
