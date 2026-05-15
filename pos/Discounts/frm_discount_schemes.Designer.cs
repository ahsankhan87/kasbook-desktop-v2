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
            this.lbl_title    = new System.Windows.Forms.Label();
            this.panel_body   = new System.Windows.Forms.Panel();
            this.txt_search   = new System.Windows.Forms.TextBox();
            this.btn_search   = new System.Windows.Forms.Button();
            this.btn_refresh  = new System.Windows.Forms.Button();
            this.btn_toggle   = new System.Windows.Forms.Button();
            this.btn_delete   = new System.Windows.Forms.Button();
            this.btn_update   = new System.Windows.Forms.Button();
            this.btn_new      = new System.Windows.Forms.Button();
            this.grid_schemes = new System.Windows.Forms.DataGridView();
            this.col_id            = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_name          = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_target         = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_calc_type     = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_value         = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_is_active     = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.col_start_date    = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col_end_date      = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.panel_header.SuspendLayout();
            this.panel_body.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_schemes)).BeginInit();
            this.SuspendLayout();

            // panel_header
            this.panel_header.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.panel_header.Controls.Add(this.lbl_title);
            this.panel_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_header.Height = 45;
            this.panel_header.Name = "panel_header";

            // lbl_title
            this.lbl_title.AutoSize = true;
            this.lbl_title.ForeColor = System.Drawing.Color.White;
            this.lbl_title.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lbl_title.Location = new System.Drawing.Point(12, 10);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Text = "Discount Schemes";

            // panel_body
            this.panel_body.BackColor = System.Drawing.Color.White;
            this.panel_body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_body.Controls.Add(this.grid_schemes);
            this.panel_body.Controls.Add(this.txt_search);
            this.panel_body.Controls.Add(this.btn_search);
            this.panel_body.Controls.Add(this.btn_refresh);
            this.panel_body.Controls.Add(this.btn_toggle);
            this.panel_body.Controls.Add(this.btn_delete);
            this.panel_body.Controls.Add(this.btn_update);
            this.panel_body.Controls.Add(this.btn_new);
            this.panel_body.Name = "panel_body";

            // txt_search
            this.txt_search.Location  = new System.Drawing.Point(12, 12);
            this.txt_search.Size      = new System.Drawing.Size(220, 23);
            this.txt_search.Name      = "txt_search";
            //this.txt_search.PlaceholderText = "Search...";
            this.txt_search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_search_KeyPress);

            // btn_search
            this.btn_search.Text     = "Search";
            this.btn_search.Location = new System.Drawing.Point(240, 10);
            this.btn_search.Size     = new System.Drawing.Size(75, 26);
            this.btn_search.Name     = "btn_search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click   += new System.EventHandler(this.btn_search_Click);

            // btn_new
            this.btn_new.Text     = "New";
            this.btn_new.Location = new System.Drawing.Point(700, 10);
            this.btn_new.Size     = new System.Drawing.Size(80, 26);
            this.btn_new.Name     = "btn_new";
            this.btn_new.UseVisualStyleBackColor = true;
            this.btn_new.Click   += new System.EventHandler(this.btn_new_Click);

            // btn_update
            this.btn_update.Text     = "Edit";
            this.btn_update.Location = new System.Drawing.Point(785, 10);
            this.btn_update.Size     = new System.Drawing.Size(80, 26);
            this.btn_update.Name     = "btn_update";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click   += new System.EventHandler(this.btn_update_Click);

            // btn_delete
            this.btn_delete.Text     = "Delete";
            this.btn_delete.Location = new System.Drawing.Point(870, 10);
            this.btn_delete.Size     = new System.Drawing.Size(80, 26);
            this.btn_delete.Name     = "btn_delete";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click   += new System.EventHandler(this.btn_delete_Click);

            // btn_toggle
            this.btn_toggle.Text     = "Toggle Active";
            this.btn_toggle.Location = new System.Drawing.Point(955, 10);
            this.btn_toggle.Size     = new System.Drawing.Size(100, 26);
            this.btn_toggle.Name     = "btn_toggle";
            this.btn_toggle.UseVisualStyleBackColor = true;
            this.btn_toggle.Click   += new System.EventHandler(this.btn_toggle_Click);

            // btn_refresh
            this.btn_refresh.Text     = "Refresh";
            this.btn_refresh.Location = new System.Drawing.Point(1060, 10);
            this.btn_refresh.Size     = new System.Drawing.Size(80, 26);
            this.btn_refresh.Name     = "btn_refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click   += new System.EventHandler(this.btn_refresh_Click);

            // grid_schemes
            this.grid_schemes.AllowUserToAddRows    = false;
            this.grid_schemes.AllowUserToDeleteRows = false;
            this.grid_schemes.AutoSizeColumnsMode   = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_schemes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_schemes.Location  = new System.Drawing.Point(0, 46);
            this.grid_schemes.Dock      = System.Windows.Forms.DockStyle.Bottom;
            this.grid_schemes.Height    = 460;
            this.grid_schemes.Name      = "grid_schemes";
            this.grid_schemes.ReadOnly  = true;
            this.grid_schemes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_schemes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.col_id, this.col_name, this.col_target,
                this.col_calc_type, this.col_value, this.col_is_active,
                this.col_start_date, this.col_end_date });

            // columns
            this.col_id.DataPropertyName = "id";            this.col_id.HeaderText = "ID";            this.col_id.Name = "col_id";            this.col_id.Width = 50;
            this.col_name.DataPropertyName = "name";        this.col_name.HeaderText = "Name";         this.col_name.Name = "col_name";
            this.col_target.DataPropertyName = "target_name"; this.col_target.HeaderText = "Linked To"; this.col_target.Name = "col_target";
            this.col_calc_type.DataPropertyName = "calc_type";         this.col_calc_type.HeaderText = "Calc";  this.col_calc_type.Name = "col_calc_type"; this.col_calc_type.Width = 80;
            this.col_value.DataPropertyName = "value";                 this.col_value.HeaderText = "Value";    this.col_value.Name = "col_value"; this.col_value.Width = 80;
            this.col_is_active.DataPropertyName = "is_active";         this.col_is_active.HeaderText = "Active"; this.col_is_active.Name = "col_is_active"; this.col_is_active.Width = 60;
            this.col_start_date.DataPropertyName = "start_date";       this.col_start_date.HeaderText = "Start Date"; this.col_start_date.Name = "col_start_date"; this.col_start_date.Width = 100;
            this.col_end_date.DataPropertyName = "end_date";           this.col_end_date.HeaderText = "End Date";   this.col_end_date.Name = "col_end_date"; this.col_end_date.Width = 100;

            // form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 550);
            this.Controls.Add(this.panel_body);
            this.Controls.Add(this.panel_header);
            this.Name = "frm_discount_schemes";
            this.Text = "Discount Schemes";
            this.Load += new System.EventHandler(this.frm_discount_schemes_Load);

            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            this.panel_body.ResumeLayout(false);
            this.panel_body.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_schemes)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panel_header;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Panel panel_body;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn col_target;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_calc_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_value;
        private System.Windows.Forms.DataGridViewCheckBoxColumn col_is_active;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_start_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn col_end_date;
    }
}
