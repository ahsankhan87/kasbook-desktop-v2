namespace pos
{
    partial class frm_financial_management_help
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.tabsHelp = new System.Windows.Forms.TabControl();
            this.tabEnglish = new System.Windows.Forms.TabPage();
            this.txtHelpEnglish = new System.Windows.Forms.RichTextBox();
            this.tabArabic = new System.Windows.Forms.TabPage();
            this.txtHelpArabic = new System.Windows.Forms.RichTextBox();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabsHelp.SuspendLayout();
            this.tabEnglish.SuspendLayout();
            this.tabArabic.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(12, 12, 12, 6);
            this.lblTitle.Size = new System.Drawing.Size(1044, 44);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Financial Management Help";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubtitle.Location = new System.Drawing.Point(0, 44);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Padding = new System.Windows.Forms.Padding(12, 0, 12, 8);
            this.lblSubtitle.Size = new System.Drawing.Size(1044, 28);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Purpose, workflow, controls, security, checklist meaning, and best practices.";
            // 
            // tabsHelp
            // 
            this.tabsHelp.Controls.Add(this.tabEnglish);
            this.tabsHelp.Controls.Add(this.tabArabic);
            this.tabsHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabsHelp.Location = new System.Drawing.Point(0, 72);
            this.tabsHelp.Name = "tabsHelp";
            this.tabsHelp.SelectedIndex = 0;
            this.tabsHelp.Size = new System.Drawing.Size(1044, 539);
            this.tabsHelp.TabIndex = 2;
            // 
            // tabEnglish
            // 
            this.tabEnglish.Controls.Add(this.txtHelpEnglish);
            this.tabEnglish.Location = new System.Drawing.Point(4, 24);
            this.tabEnglish.Name = "tabEnglish";
            this.tabEnglish.Padding = new System.Windows.Forms.Padding(8);
            this.tabEnglish.Size = new System.Drawing.Size(1036, 511);
            this.tabEnglish.TabIndex = 0;
            this.tabEnglish.Text = "English";
            this.tabEnglish.UseVisualStyleBackColor = true;
            // 
            // txtHelpEnglish
            // 
            this.txtHelpEnglish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHelpEnglish.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtHelpEnglish.Location = new System.Drawing.Point(8, 8);
            this.txtHelpEnglish.Name = "txtHelpEnglish";
            this.txtHelpEnglish.ReadOnly = true;
            this.txtHelpEnglish.Size = new System.Drawing.Size(1020, 495);
            this.txtHelpEnglish.TabIndex = 0;
            this.txtHelpEnglish.Text = "";
            // 
            // tabArabic
            // 
            this.tabArabic.Controls.Add(this.txtHelpArabic);
            this.tabArabic.Location = new System.Drawing.Point(4, 24);
            this.tabArabic.Name = "tabArabic";
            this.tabArabic.Padding = new System.Windows.Forms.Padding(8);
            this.tabArabic.Size = new System.Drawing.Size(1036, 511);
            this.tabArabic.TabIndex = 1;
            this.tabArabic.Text = "العربية";
            this.tabArabic.UseVisualStyleBackColor = true;
            // 
            // txtHelpArabic
            // 
            this.txtHelpArabic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHelpArabic.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtHelpArabic.Location = new System.Drawing.Point(8, 8);
            this.txtHelpArabic.Name = "txtHelpArabic";
            this.txtHelpArabic.ReadOnly = true;
            this.txtHelpArabic.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtHelpArabic.Size = new System.Drawing.Size(1020, 495);
            this.txtHelpArabic.TabIndex = 0;
            this.txtHelpArabic.Text = "";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.btnClose);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 611);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1044, 50);
            this.panelFooter.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(945, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frm_financial_management_help
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 661);
            this.Controls.Add(this.tabsHelp);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.KeyPreview = true;
            this.Name = "frm_financial_management_help";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Financial Management Help";
            this.Load += new System.EventHandler(this.frm_financial_management_help_Load);
            this.tabsHelp.ResumeLayout(false);
            this.tabEnglish.ResumeLayout(false);
            this.tabArabic.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.TabControl tabsHelp;
        private System.Windows.Forms.TabPage tabEnglish;
        private System.Windows.Forms.RichTextBox txtHelpEnglish;
        private System.Windows.Forms.TabPage tabArabic;
        private System.Windows.Forms.RichTextBox txtHelpArabic;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Button btnClose;
    }
}
