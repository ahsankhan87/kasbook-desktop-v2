namespace pos.Discounts
{
    partial class frm_apply_discount_scheme_bulk
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
            this.lbl_scheme = new System.Windows.Forms.Label();
            this.cmb_scheme = new System.Windows.Forms.ComboBox();
            this.lbl_brand = new System.Windows.Forms.Label();
            this.cmb_brand = new System.Windows.Forms.ComboBox();
            this.lbl_category = new System.Windows.Forms.Label();
            this.cmb_category = new System.Windows.Forms.ComboBox();
            this.btn_apply = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.panel_header.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_header
            // 
            this.panel_header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panel_header.Controls.Add(this.lbl_title);
            this.panel_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_header.Location = new System.Drawing.Point(0, 0);
            this.panel_header.Name = "panel_header";
            this.panel_header.Size = new System.Drawing.Size(560, 48);
            this.panel_header.TabIndex = 0;
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lbl_title.ForeColor = System.Drawing.Color.White;
            this.lbl_title.Location = new System.Drawing.Point(12, 11);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(266, 28);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "Apply Scheme To Products";
            // 
            // lbl_scheme
            // 
            this.lbl_scheme.AutoSize = true;
            this.lbl_scheme.Location = new System.Drawing.Point(26, 74);
            this.lbl_scheme.Name = "lbl_scheme";
            this.lbl_scheme.Size = new System.Drawing.Size(112, 17);
            this.lbl_scheme.TabIndex = 1;
            this.lbl_scheme.Text = "Discount Scheme";
            // 
            // cmb_scheme
            // 
            this.cmb_scheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_scheme.FormattingEnabled = true;
            this.cmb_scheme.Location = new System.Drawing.Point(170, 70);
            this.cmb_scheme.Name = "cmb_scheme";
            this.cmb_scheme.Size = new System.Drawing.Size(350, 24);
            this.cmb_scheme.TabIndex = 2;
            // 
            // lbl_brand
            // 
            this.lbl_brand.AutoSize = true;
            this.lbl_brand.Location = new System.Drawing.Point(26, 114);
            this.lbl_brand.Name = "lbl_brand";
            this.lbl_brand.Size = new System.Drawing.Size(45, 17);
            this.lbl_brand.TabIndex = 3;
            this.lbl_brand.Text = "Brand";
            // 
            // cmb_brand
            // 
            this.cmb_brand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_brand.FormattingEnabled = true;
            this.cmb_brand.Location = new System.Drawing.Point(170, 110);
            this.cmb_brand.Name = "cmb_brand";
            this.cmb_brand.Size = new System.Drawing.Size(350, 24);
            this.cmb_brand.TabIndex = 4;
            // 
            // lbl_category
            // 
            this.lbl_category.AutoSize = true;
            this.lbl_category.Location = new System.Drawing.Point(26, 154);
            this.lbl_category.Name = "lbl_category";
            this.lbl_category.Size = new System.Drawing.Size(62, 17);
            this.lbl_category.TabIndex = 5;
            this.lbl_category.Text = "Category";
            // 
            // cmb_category
            // 
            this.cmb_category.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_category.FormattingEnabled = true;
            this.cmb_category.Location = new System.Drawing.Point(170, 150);
            this.cmb_category.Name = "cmb_category";
            this.cmb_category.Size = new System.Drawing.Size(350, 24);
            this.cmb_category.TabIndex = 6;
            // 
            // btn_apply
            // 
            this.btn_apply.Location = new System.Drawing.Point(170, 203);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(100, 32);
            this.btn_apply.TabIndex = 7;
            this.btn_apply.Text = "Apply";
            this.btn_apply.UseVisualStyleBackColor = true;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(276, 203);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(100, 32);
            this.btn_cancel.TabIndex = 8;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // frm_apply_discount_scheme_bulk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 260);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_apply);
            this.Controls.Add(this.cmb_category);
            this.Controls.Add(this.lbl_category);
            this.Controls.Add(this.cmb_brand);
            this.Controls.Add(this.lbl_brand);
            this.Controls.Add(this.cmb_scheme);
            this.Controls.Add(this.lbl_scheme);
            this.Controls.Add(this.panel_header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_apply_discount_scheme_bulk";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Apply Discount Scheme";
            this.Load += new System.EventHandler(this.frm_apply_discount_scheme_bulk_Load);
            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel panel_header;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label lbl_scheme;
        private System.Windows.Forms.ComboBox cmb_scheme;
        private System.Windows.Forms.Label lbl_brand;
        private System.Windows.Forms.ComboBox cmb_brand;
        private System.Windows.Forms.Label lbl_category;
        private System.Windows.Forms.ComboBox cmb_category;
        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.Button btn_cancel;
    }
}
