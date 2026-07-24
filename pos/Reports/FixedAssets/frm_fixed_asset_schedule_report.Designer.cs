namespace pos.Reports.FixedAssets
{
    partial class frm_fixed_asset_schedule_report
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.dtAsOfDate = new System.Windows.Forms.DateTimePicker();
            this.lblAsOf = new System.Windows.Forms.Label();
            this.lblAsOfDate = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCompany = new System.Windows.Forms.Label();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnExportPdf);
            this.pnlTop.Controls.Add(this.btnPrintPreview);
            this.pnlTop.Controls.Add(this.btnExportExcel);
            this.pnlTop.Controls.Add(this.btnLoad);
            this.pnlTop.Controls.Add(this.dtAsOfDate);
            this.pnlTop.Controls.Add(this.lblAsOf);
            this.pnlTop.Controls.Add(this.lblAsOfDate);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.lblCompany);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1280, 110);
            this.pnlTop.TabIndex = 0;
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.Location = new System.Drawing.Point(610, 72);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(120, 28);
            this.btnExportPdf.TabIndex = 8;
            this.btnExportPdf.Text = "Export PDF";
            this.btnExportPdf.UseVisualStyleBackColor = true;
            this.btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Location = new System.Drawing.Point(484, 72);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(120, 28);
            this.btnPrintPreview.TabIndex = 7;
            this.btnPrintPreview.Text = "Print Preview";
            this.btnPrintPreview.UseVisualStyleBackColor = true;
            this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(358, 72);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(120, 28);
            this.btnExportExcel.TabIndex = 6;
            this.btnExportExcel.Text = "Export Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(232, 72);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(120, 28);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // dtAsOfDate
            // 
            this.dtAsOfDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtAsOfDate.Location = new System.Drawing.Point(86, 74);
            this.dtAsOfDate.Name = "dtAsOfDate";
            this.dtAsOfDate.Size = new System.Drawing.Size(140, 24);
            this.dtAsOfDate.TabIndex = 4;
            // 
            // lblAsOf
            // 
            this.lblAsOf.AutoSize = true;
            this.lblAsOf.Location = new System.Drawing.Point(12, 78);
            this.lblAsOf.Name = "lblAsOf";
            this.lblAsOf.Size = new System.Drawing.Size(68, 17);
            this.lblAsOf.TabIndex = 3;
            this.lblAsOf.Text = "As of Date";
            // 
            // lblAsOfDate
            // 
            this.lblAsOfDate.AutoSize = true;
            this.lblAsOfDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAsOfDate.Location = new System.Drawing.Point(12, 50);
            this.lblAsOfDate.Name = "lblAsOfDate";
            this.lblAsOfDate.Size = new System.Drawing.Size(108, 15);
            this.lblAsOfDate.TabIndex = 2;
            this.lblAsOfDate.Text = "As of Date: yyyy-mm";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(11, 26);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(154, 20);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Fixed Asset Schedule";
            // 
            // lblCompany
            // 
            this.lblCompany.AutoSize = true;
            this.lblCompany.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCompany.Location = new System.Drawing.Point(11, 6);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(76, 20);
            this.lblCompany.TabIndex = 0;
            this.lblCompany.Text = "Company";
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReport.Location = new System.Drawing.Point(0, 110);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new System.Drawing.Size(1280, 610);
            this.dgvReport.TabIndex = 1;
            // 
            // frm_fixed_asset_schedule_report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.Name = "frm_fixed_asset_schedule_report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fixed Asset Schedule Report";
            this.Load += new System.EventHandler(this.frm_fixed_asset_schedule_report_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Button btnPrintPreview;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DateTimePicker dtAsOfDate;
        private System.Windows.Forms.Label lblAsOf;
        private System.Windows.Forms.Label lblAsOfDate;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCompany;
        private System.Windows.Forms.DataGridView dgvReport;
    }
}
