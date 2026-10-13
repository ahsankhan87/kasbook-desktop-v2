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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_apply_discount_scheme_bulk));
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
            // lbl_scheme
            // 
            resources.ApplyResources(this.lbl_scheme, "lbl_scheme");
            this.lbl_scheme.Name = "lbl_scheme";
            // 
            // cmb_scheme
            // 
            resources.ApplyResources(this.cmb_scheme, "cmb_scheme");
            this.cmb_scheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_scheme.FormattingEnabled = true;
            this.cmb_scheme.Name = "cmb_scheme";
            // 
            // lbl_brand
            // 
            resources.ApplyResources(this.lbl_brand, "lbl_brand");
            this.lbl_brand.Name = "lbl_brand";
            // 
            // cmb_brand
            // 
            resources.ApplyResources(this.cmb_brand, "cmb_brand");
            this.cmb_brand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_brand.FormattingEnabled = true;
            this.cmb_brand.Name = "cmb_brand";
            // 
            // lbl_category
            // 
            resources.ApplyResources(this.lbl_category, "lbl_category");
            this.lbl_category.Name = "lbl_category";
            // 
            // cmb_category
            // 
            resources.ApplyResources(this.cmb_category, "cmb_category");
            this.cmb_category.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_category.FormattingEnabled = true;
            this.cmb_category.Name = "cmb_category";
            // 
            // btn_apply
            // 
            resources.ApplyResources(this.btn_apply, "btn_apply");
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.UseVisualStyleBackColor = true;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // btn_cancel
            // 
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // frm_apply_discount_scheme_bulk
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
