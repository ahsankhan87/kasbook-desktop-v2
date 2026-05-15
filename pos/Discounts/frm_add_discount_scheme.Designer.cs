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
            this.cmb_apply_on = new System.Windows.Forms.ComboBox();
            this.cmb_product = new System.Windows.Forms.ComboBox();
            this.cmb_brand = new System.Windows.Forms.ComboBox();
            this.cmb_category = new System.Windows.Forms.ComboBox();
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
            this.lbl_apply_on = new System.Windows.Forms.Label();
            this.lbl_product = new System.Windows.Forms.Label();
            this.lbl_brand = new System.Windows.Forms.Label();
            this.lbl_category = new System.Windows.Forms.Label();
            this.lbl_calc_type = new System.Windows.Forms.Label();
            this.lbl_value = new System.Windows.Forms.Label();
            this.lbl_start_date = new System.Windows.Forms.Label();
            this.lbl_end_date = new System.Windows.Forms.Label();

            this.panel_header.SuspendLayout();
            this.SuspendLayout();

            this.panel_header.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.panel_header.Controls.Add(this.lbl_header_title);
            this.panel_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_header.Height = 45;

            this.lbl_header_title.AutoSize = true;
            this.lbl_header_title.ForeColor = System.Drawing.Color.White;
            this.lbl_header_title.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lbl_header_title.Location = new System.Drawing.Point(12, 10);
            this.lbl_header_title.Text = "Discount Scheme";

            this.txt_id.Visible = false;

            int lx = 20, fx = 160, fy = 60, gap = 36;

            this.lbl_name.Text = "Name:";
            this.lbl_name.Location = new System.Drawing.Point(lx, fy);
            this.lbl_name.AutoSize = true;
            this.txt_name.Location = new System.Drawing.Point(fx, fy - 3);
            this.txt_name.Size = new System.Drawing.Size(260, 23);

            fy += gap;
            this.lbl_name_ar.Text = "Name (AR):";
            this.lbl_name_ar.Location = new System.Drawing.Point(lx, fy);
            this.lbl_name_ar.AutoSize = true;
            this.txt_name_ar.Location = new System.Drawing.Point(fx, fy - 3);
            this.txt_name_ar.Size = new System.Drawing.Size(260, 23);

            fy += gap;
            this.lbl_apply_on.Text = "Apply On:";
            this.lbl_apply_on.Location = new System.Drawing.Point(lx, fy);
            this.lbl_apply_on.AutoSize = true;
            this.cmb_apply_on.Location = new System.Drawing.Point(fx, fy - 3);
            this.cmb_apply_on.Size = new System.Drawing.Size(180, 23);
            this.cmb_apply_on.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_apply_on.SelectedIndexChanged += new System.EventHandler(this.cmb_apply_on_SelectedIndexChanged);

            fy += gap;
            this.lbl_product.Text = "Product:";
            this.lbl_product.Location = new System.Drawing.Point(lx, fy);
            this.lbl_product.AutoSize = true;
            this.cmb_product.Location = new System.Drawing.Point(fx, fy - 3);
            this.cmb_product.Size = new System.Drawing.Size(260, 23);
            this.cmb_product.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lbl_brand.Text = "Brand:";
            this.lbl_brand.Location = new System.Drawing.Point(lx, fy);
            this.lbl_brand.AutoSize = true;
            this.cmb_brand.Location = new System.Drawing.Point(fx, fy - 3);
            this.cmb_brand.Size = new System.Drawing.Size(260, 23);
            this.cmb_brand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lbl_category.Text = "Category:";
            this.lbl_category.Location = new System.Drawing.Point(lx, fy);
            this.lbl_category.AutoSize = true;
            this.cmb_category.Location = new System.Drawing.Point(fx, fy - 3);
            this.cmb_category.Size = new System.Drawing.Size(260, 23);
            this.cmb_category.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            fy += gap;
            this.lbl_calc_type.Text = "Calc Type:";
            this.lbl_calc_type.Location = new System.Drawing.Point(lx, fy);
            this.lbl_calc_type.AutoSize = true;
            this.cmb_calc_type.Location = new System.Drawing.Point(fx, fy - 3);
            this.cmb_calc_type.Size = new System.Drawing.Size(140, 23);
            this.cmb_calc_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            fy += gap;
            this.lbl_value.Text = "Discount Value:";
            this.lbl_value.Location = new System.Drawing.Point(lx, fy);
            this.lbl_value.AutoSize = true;
            this.txt_value.Location = new System.Drawing.Point(fx, fy - 3);
            this.txt_value.Size = new System.Drawing.Size(120, 23);
            this.txt_value.Text = "0";

            fy += gap;
            this.lbl_start_date.Text = "Start Date:";
            this.lbl_start_date.Location = new System.Drawing.Point(lx, fy);
            this.lbl_start_date.AutoSize = true;
            this.dtp_start.Location = new System.Drawing.Point(fx, fy - 3);
            this.dtp_start.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.chk_no_start.Text = "No start limit";
            this.chk_no_start.Location = new System.Drawing.Point(fx + 170, fy - 2);
            this.chk_no_start.AutoSize = true;
            this.chk_no_start.CheckedChanged += new System.EventHandler(this.chk_no_start_CheckedChanged);

            fy += gap;
            this.lbl_end_date.Text = "End Date:";
            this.lbl_end_date.Location = new System.Drawing.Point(lx, fy);
            this.lbl_end_date.AutoSize = true;
            this.dtp_end.Location = new System.Drawing.Point(fx, fy - 3);
            this.dtp_end.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.chk_no_end.Text = "No end limit";
            this.chk_no_end.Location = new System.Drawing.Point(fx + 170, fy - 2);
            this.chk_no_end.AutoSize = true;
            this.chk_no_end.CheckedChanged += new System.EventHandler(this.chk_no_end_CheckedChanged);

            fy += gap;
            this.chk_is_active.Text = "Active";
            this.chk_is_active.Location = new System.Drawing.Point(fx, fy);
            this.chk_is_active.AutoSize = true;
            this.chk_is_active.Checked = true;

            fy += gap + 8;
            this.btn_save.Text = "Save";
            this.btn_save.Location = new System.Drawing.Point(fx, fy);
            this.btn_save.Size = new System.Drawing.Size(90, 30);
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);

            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.Location = new System.Drawing.Point(fx + 100, fy);
            this.btn_cancel.Size = new System.Drawing.Size(90, 30);
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, fy + 60);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Discount Scheme";
            this.Load += new System.EventHandler(this.frm_add_discount_scheme_Load);

            this.Controls.Add(this.panel_header);
            this.Controls.Add(this.txt_id);
            this.Controls.Add(this.lbl_name);
            this.Controls.Add(this.txt_name);
            this.Controls.Add(this.lbl_name_ar);
            this.Controls.Add(this.txt_name_ar);
            this.Controls.Add(this.lbl_apply_on);
            this.Controls.Add(this.cmb_apply_on);
            this.Controls.Add(this.lbl_product);
            this.Controls.Add(this.cmb_product);
            this.Controls.Add(this.lbl_brand);
            this.Controls.Add(this.cmb_brand);
            this.Controls.Add(this.lbl_category);
            this.Controls.Add(this.cmb_category);
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
        private System.Windows.Forms.ComboBox cmb_apply_on;
        private System.Windows.Forms.ComboBox cmb_product;
        private System.Windows.Forms.ComboBox cmb_brand;
        private System.Windows.Forms.ComboBox cmb_category;
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
        private System.Windows.Forms.Label lbl_apply_on;
        private System.Windows.Forms.Label lbl_product;
        private System.Windows.Forms.Label lbl_brand;
        private System.Windows.Forms.Label lbl_category;
        private System.Windows.Forms.Label lbl_calc_type;
        private System.Windows.Forms.Label lbl_value;
        private System.Windows.Forms.Label lbl_start_date;
        private System.Windows.Forms.Label lbl_end_date;
    }
}
