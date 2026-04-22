namespace pos
{
    partial class frm_sales_excel_import
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_sales_excel_import));
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDetails = new System.Windows.Forms.Label();
            this.groupBoxSteps = new System.Windows.Forms.GroupBox();
            this.lblSteps = new System.Windows.Forms.Label();
            this.btnImportExcel = new System.Windows.Forms.Button();
            this.btnDownloadTemplate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBoxSteps.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // lblDetails
            // 
            resources.ApplyResources(this.lblDetails, "lblDetails");
            this.lblDetails.Name = "lblDetails";
            // 
            // groupBoxSteps
            // 
            resources.ApplyResources(this.groupBoxSteps, "groupBoxSteps");
            this.groupBoxSteps.Controls.Add(this.lblSteps);
            this.groupBoxSteps.Name = "groupBoxSteps";
            this.groupBoxSteps.TabStop = false;
            // 
            // lblSteps
            // 
            resources.ApplyResources(this.lblSteps, "lblSteps");
            this.lblSteps.Name = "lblSteps";
            // 
            // btnImportExcel
            // 
            resources.ApplyResources(this.btnImportExcel, "btnImportExcel");
            this.btnImportExcel.Name = "btnImportExcel";
            this.btnImportExcel.UseVisualStyleBackColor = true;
            this.btnImportExcel.Click += new System.EventHandler(this.btnImportExcel_Click);
            // 
            // btnDownloadTemplate
            // 
            resources.ApplyResources(this.btnDownloadTemplate, "btnDownloadTemplate");
            this.btnDownloadTemplate.Name = "btnDownloadTemplate";
            this.btnDownloadTemplate.UseVisualStyleBackColor = true;
            this.btnDownloadTemplate.Click += new System.EventHandler(this.btnDownloadTemplate_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frm_sales_excel_import
            // 
            this.AcceptButton = this.btnImportExcel;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDownloadTemplate);
            this.Controls.Add(this.btnImportExcel);
            this.Controls.Add(this.groupBoxSteps);
            this.Controls.Add(this.lblDetails);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_sales_excel_import";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_sales_excel_import_Load);
            this.groupBoxSteps.ResumeLayout(false);
            this.groupBoxSteps.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDetails;
        private System.Windows.Forms.GroupBox groupBoxSteps;
        private System.Windows.Forms.Label lblSteps;
        private System.Windows.Forms.Button btnImportExcel;
        private System.Windows.Forms.Button btnDownloadTemplate;
        private System.Windows.Forms.Button btnClose;
    }
}
