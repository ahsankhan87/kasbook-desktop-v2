namespace pos.Discounts
{
    partial class frm_add_discount_scheme
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
            this.lbl_header_title = new System.Windows.Forms.Label();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.txt_name_ar = new System.Windows.Forms.TextBox();
            this.txt_value = new System.Windows.Forms.TextBox();
            this.cmb_calc_type = new System.Windows.Forms.ComboBox();
            this.chk_is_active = new System.Windows.Forms.CheckBox();
            this.chk_no_start = new System.Windows.Forms.CheckBox();
            this.chk_no_end = new System.Windows.Forms.CheckBox();
            this.dtp_start = new System.Windows.Forms.DateTimePicker();
            this.dtp_end = new System.Windows.Forms.DateTimePicker();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.lbl_name = new System.Windows.Forms.Label();
            this.lbl_name_ar = new System.Windows.Forms.Label();
            this.lbl_calc_type = new System.Windows.Forms.Label();
            this.lbl_value = new System.Windows.Forms.Label();
            this.lbl_start_date = new System.Windows.Forms.Label();
            this.lbl_end_date = new System.Windows.Forms.Label();
            this.panel_header.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_header
            // 
            this.panel_header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel_header.Controls.Add(this.lbl_header_title);
            this.panel_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_header.Location = new System.Drawing.Point(0, 0);
            this.panel_header.Name = "panel_header";
            this.panel_header.Size = new System.Drawing.Size(500, 48);
            this.panel_header.TabIndex = 0;
            // 
            // lbl_header_title
            // 
            this.lbl_header_title.AutoSize = true;
            this.lbl_header_title.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lbl_header_title.ForeColor = System.Drawing.Color.White;
            this.lbl_header_title.Location = new System.Drawing.Point(12, 11);
            this.lbl_header_title.Name = "lbl_header_title";
            this.lbl_header_title.Size = new System.Drawing.Size(170, 28);
            this.lbl_header_title.TabIndex = 0;
            this.lbl_header_title.Text = "Discount Scheme";
            // 
            // txt_id
            // 
            this.txt_id.Location = new System.Drawing.Point(0, 0);
            this.txt_id.Name = "txt_id";
            this.txt_id.Size = new System.Drawing.Size(0, 24);
            this.txt_id.TabIndex = 1;
            this.txt_id.Visible = false;
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(160, 61);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(260, 24);
            this.txt_name.TabIndex = 3;
            // 
            // txt_name_ar
            // 
            this.txt_name_ar.Location = new System.Drawing.Point(160, 99);
            this.txt_name_ar.Name = "txt_name_ar";
            this.txt_name_ar.Size = new System.Drawing.Size(260, 24);
            this.txt_name_ar.TabIndex = 5;
            // 
            // txt_value
            // 
            this.txt_value.Location = new System.Drawing.Point(160, 176);
            this.txt_value.Name = "txt_value";
            this.txt_value.Size = new System.Drawing.Size(120, 24);
            this.txt_value.TabIndex = 9;
            this.txt_value.Text = "0";
            // 
            // cmb_calc_type
            // 
            this.cmb_calc_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_calc_type.Location = new System.Drawing.Point(160, 138);
            this.cmb_calc_type.Name = "cmb_calc_type";
            this.cmb_calc_type.Size = new System.Drawing.Size(140, 24);
            this.cmb_calc_type.TabIndex = 7;
            // 
            // chk_is_active
            // 
            this.chk_is_active.AutoSize = true;
            this.chk_is_active.Checked = true;
            this.chk_is_active.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_is_active.Location = new System.Drawing.Point(160, 294);
            this.chk_is_active.Name = "chk_is_active";
            this.chk_is_active.Size = new System.Drawing.Size(67, 21);
            this.chk_is_active.TabIndex = 16;
            this.chk_is_active.Text = "Active";
            // 
            // chk_no_start
            // 
            this.chk_no_start.AutoSize = true;
            this.chk_no_start.Location = new System.Drawing.Point(366, 214);
            this.chk_no_start.Name = "chk_no_start";
            this.chk_no_start.Size = new System.Drawing.Size(106, 21);
            this.chk_no_start.TabIndex = 12;
            this.chk_no_start.Text = "No start limit";
            this.chk_no_start.CheckedChanged += new System.EventHandler(this.chk_no_start_CheckedChanged);
            // 
            // chk_no_end
            // 
            this.chk_no_end.AutoSize = true;
            this.chk_no_end.Location = new System.Drawing.Point(366, 253);
            this.chk_no_end.Name = "chk_no_end";
            this.chk_no_end.Size = new System.Drawing.Size(101, 21);
            this.chk_no_end.TabIndex = 15;
            this.chk_no_end.Text = "No end limit";
            this.chk_no_end.CheckedChanged += new System.EventHandler(this.chk_no_end_CheckedChanged);
            // 
            // dtp_start
            // 
            this.dtp_start.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_start.Location = new System.Drawing.Point(160, 214);
            this.dtp_start.Name = "dtp_start";
            this.dtp_start.Size = new System.Drawing.Size(200, 24);
            this.dtp_start.TabIndex = 11;
            // 
            // dtp_end
            // 
            this.dtp_end.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_end.Location = new System.Drawing.Point(160, 253);
            this.dtp_end.Name = "dtp_end";
            this.dtp_end.Size = new System.Drawing.Size(200, 24);
            this.dtp_end.TabIndex = 14;
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(160, 341);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(90, 32);
            this.btn_save.TabIndex = 17;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(260, 341);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(90, 32);
            this.btn_cancel.TabIndex = 18;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.Location = new System.Drawing.Point(20, 64);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(48, 17);
            this.lbl_name.TabIndex = 2;
            this.lbl_name.Text = "Name:";
            // 
            // lbl_name_ar
            // 
            this.lbl_name_ar.AutoSize = true;
            this.lbl_name_ar.Location = new System.Drawing.Point(20, 102);
            this.lbl_name_ar.Name = "lbl_name_ar";
            this.lbl_name_ar.Size = new System.Drawing.Size(79, 17);
            this.lbl_name_ar.TabIndex = 4;
            this.lbl_name_ar.Text = "Name (AR):";
            // 
            // lbl_calc_type
            // 
            this.lbl_calc_type.AutoSize = true;
            this.lbl_calc_type.Location = new System.Drawing.Point(20, 141);
            this.lbl_calc_type.Name = "lbl_calc_type";
            this.lbl_calc_type.Size = new System.Drawing.Size(73, 17);
            this.lbl_calc_type.TabIndex = 6;
            this.lbl_calc_type.Text = "Calc Type:";
            // 
            // lbl_value
            // 
            this.lbl_value.AutoSize = true;
            this.lbl_value.Location = new System.Drawing.Point(20, 179);
            this.lbl_value.Name = "lbl_value";
            this.lbl_value.Size = new System.Drawing.Size(103, 17);
            this.lbl_value.TabIndex = 8;
            this.lbl_value.Text = "Discount Value:";
            // 
            // lbl_start_date
            // 
            this.lbl_start_date.AutoSize = true;
            this.lbl_start_date.Location = new System.Drawing.Point(20, 218);
            this.lbl_start_date.Name = "lbl_start_date";
            this.lbl_start_date.Size = new System.Drawing.Size(76, 17);
            this.lbl_start_date.TabIndex = 10;
            this.lbl_start_date.Text = "Start Date:";
            // 
            // lbl_end_date
            // 
            this.lbl_end_date.AutoSize = true;
            this.lbl_end_date.Location = new System.Drawing.Point(20, 256);
            this.lbl_end_date.Name = "lbl_end_date";
            this.lbl_end_date.Size = new System.Drawing.Size(70, 17);
            this.lbl_end_date.TabIndex = 13;
            this.lbl_end_date.Text = "End Date:";
            // 
            // frm_add_discount_scheme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(500, 405);
            this.Controls.Add(this.panel_header);
            this.Controls.Add(this.txt_id);
            this.Controls.Add(this.lbl_name);
            this.Controls.Add(this.txt_name);
            this.Controls.Add(this.lbl_name_ar);
            this.Controls.Add(this.txt_name_ar);
            this.Controls.Add(this.lbl_calc_type);
            this.Controls.Add(this.cmb_calc_type);
            this.Controls.Add(this.lbl_value);
            this.Controls.Add(this.txt_value);
            this.Controls.Add(this.lbl_start_date);
            this.Controls.Add(this.dtp_start);
            this.Controls.Add(this.chk_no_start);
            this.Controls.Add(this.lbl_end_date);
            this.Controls.Add(this.dtp_end);
            this.Controls.Add(this.chk_no_end);
            this.Controls.Add(this.chk_is_active);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_add_discount_scheme";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Discount Scheme";
            this.Load += new System.EventHandler(this.frm_add_discount_scheme_Load);
            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel panel_header;
        private System.Windows.Forms.Label lbl_header_title;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.TextBox txt_name_ar;
        private System.Windows.Forms.TextBox txt_value;
        private System.Windows.Forms.ComboBox cmb_calc_type;
        private System.Windows.Forms.CheckBox chk_is_active;
        private System.Windows.Forms.CheckBox chk_no_start;
        private System.Windows.Forms.CheckBox chk_no_end;
        private System.Windows.Forms.DateTimePicker dtp_start;
        private System.Windows.Forms.DateTimePicker dtp_end;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.Label lbl_name_ar;
        private System.Windows.Forms.Label lbl_calc_type;
        private System.Windows.Forms.Label lbl_value;
        private System.Windows.Forms.Label lbl_start_date;
        private System.Windows.Forms.Label lbl_end_date;
    }
}
