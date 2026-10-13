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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_add_discount_scheme));
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
            resources.ApplyResources(this.panel_header, "panel_header");
            this.panel_header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel_header.Controls.Add(this.lbl_header_title);
            this.panel_header.Name = "panel_header";
            // 
            // lbl_header_title
            // 
            resources.ApplyResources(this.lbl_header_title, "lbl_header_title");
            this.lbl_header_title.ForeColor = System.Drawing.Color.White;
            this.lbl_header_title.Name = "lbl_header_title";
            // 
            // txt_id
            // 
            resources.ApplyResources(this.txt_id, "txt_id");
            this.txt_id.Name = "txt_id";
            // 
            // txt_name
            // 
            resources.ApplyResources(this.txt_name, "txt_name");
            this.txt_name.Name = "txt_name";
            // 
            // txt_name_ar
            // 
            resources.ApplyResources(this.txt_name_ar, "txt_name_ar");
            this.txt_name_ar.Name = "txt_name_ar";
            // 
            // txt_value
            // 
            resources.ApplyResources(this.txt_value, "txt_value");
            this.txt_value.Name = "txt_value";
            // 
            // cmb_calc_type
            // 
            resources.ApplyResources(this.cmb_calc_type, "cmb_calc_type");
            this.cmb_calc_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_calc_type.Name = "cmb_calc_type";
            // 
            // chk_is_active
            // 
            resources.ApplyResources(this.chk_is_active, "chk_is_active");
            this.chk_is_active.Checked = true;
            this.chk_is_active.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_is_active.Name = "chk_is_active";
            // 
            // chk_no_start
            // 
            resources.ApplyResources(this.chk_no_start, "chk_no_start");
            this.chk_no_start.Name = "chk_no_start";
            this.chk_no_start.CheckedChanged += new System.EventHandler(this.chk_no_start_CheckedChanged);
            // 
            // chk_no_end
            // 
            resources.ApplyResources(this.chk_no_end, "chk_no_end");
            this.chk_no_end.Name = "chk_no_end";
            this.chk_no_end.CheckedChanged += new System.EventHandler(this.chk_no_end_CheckedChanged);
            // 
            // dtp_start
            // 
            resources.ApplyResources(this.dtp_start, "dtp_start");
            this.dtp_start.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_start.Name = "dtp_start";
            // 
            // dtp_end
            // 
            resources.ApplyResources(this.dtp_end, "dtp_end");
            this.dtp_end.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_end.Name = "dtp_end";
            // 
            // btn_save
            // 
            resources.ApplyResources(this.btn_save, "btn_save");
            this.btn_save.Name = "btn_save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_cancel
            // 
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // lbl_name
            // 
            resources.ApplyResources(this.lbl_name, "lbl_name");
            this.lbl_name.Name = "lbl_name";
            // 
            // lbl_name_ar
            // 
            resources.ApplyResources(this.lbl_name_ar, "lbl_name_ar");
            this.lbl_name_ar.Name = "lbl_name_ar";
            // 
            // lbl_calc_type
            // 
            resources.ApplyResources(this.lbl_calc_type, "lbl_calc_type");
            this.lbl_calc_type.Name = "lbl_calc_type";
            // 
            // lbl_value
            // 
            resources.ApplyResources(this.lbl_value, "lbl_value");
            this.lbl_value.Name = "lbl_value";
            // 
            // lbl_start_date
            // 
            resources.ApplyResources(this.lbl_start_date, "lbl_start_date");
            this.lbl_start_date.Name = "lbl_start_date";
            // 
            // lbl_end_date
            // 
            resources.ApplyResources(this.lbl_end_date, "lbl_end_date");
            this.lbl_end_date.Name = "lbl_end_date";
            // 
            // frm_add_discount_scheme
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
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
