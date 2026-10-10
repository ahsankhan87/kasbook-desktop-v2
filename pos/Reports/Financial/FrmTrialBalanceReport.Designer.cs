namespace pos.Reports.Financial
{
    partial class FrmTrialBalanceReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTrialBalanceReport));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.pnlDatePickers = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDateRange = new System.Windows.Forms.ComboBox();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            this.pnlFilters.SuspendLayout();
            this.pnlDatePickers.SuspendLayout();
            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            resources.ApplyResources(this.pnlTop, "pnlTop");
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Name = "pnlTop";
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name = "lblTitle";
            // 
            // pnlFilters
            // 
            resources.ApplyResources(this.pnlFilters, "pnlFilters");
            this.pnlFilters.BackColor = System.Drawing.Color.White;
            this.pnlFilters.Controls.Add(this.btnExport);
            this.pnlFilters.Controls.Add(this.btnPrint);
            this.pnlFilters.Controls.Add(this.btnLoad);
            this.pnlFilters.Controls.Add(this.pnlDatePickers);
            this.pnlFilters.Controls.Add(this.label3);
            this.pnlFilters.Controls.Add(this.cmbDateRange);
            this.pnlFilters.Name = "pnlFilters";
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnLoad
            // 
            resources.ApplyResources(this.btnLoad, "btnLoad");
            this.btnLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnLoad.FlatAppearance.BorderSize = 0;
            this.btnLoad.ForeColor = System.Drawing.Color.White;
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.UseVisualStyleBackColor = false;
            // 
            // pnlDatePickers
            // 
            resources.ApplyResources(this.pnlDatePickers, "pnlDatePickers");
            this.pnlDatePickers.Controls.Add(this.label2);
            this.pnlDatePickers.Controls.Add(this.label1);
            this.pnlDatePickers.Controls.Add(this.dtpToDate);
            this.pnlDatePickers.Controls.Add(this.dtpFromDate);
            this.pnlDatePickers.Name = "pnlDatePickers";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label1.Name = "label1";
            // 
            // dtpToDate
            // 
            resources.ApplyResources(this.dtpToDate, "dtpToDate");
            this.dtpToDate.Name = "dtpToDate";
            // 
            // dtpFromDate
            // 
            resources.ApplyResources(this.dtpFromDate, "dtpFromDate");
            this.dtpFromDate.Name = "dtpFromDate";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.label3.Name = "label3";
            // 
            // cmbDateRange
            // 
            resources.ApplyResources(this.cmbDateRange, "cmbDateRange");
            this.cmbDateRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateRange.FormattingEnabled = true;
            this.cmbDateRange.Name = "cmbDateRange";
            // 
            // pnlContent
            // 
            resources.ApplyResources(this.pnlContent, "pnlContent");
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Controls.Add(this.dgvReport);
            this.pnlContent.Name = "pnlContent";
            // 
            // dgvReport
            // 
            resources.ApplyResources(this.dgvReport, "dgvReport");
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.DoubleClick += new System.EventHandler(this.dgvReport_DoubleClick);
            // 
            // FrmTrialBalanceReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(250)))));
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.pnlTop);
            this.Name = "FrmTrialBalanceReport";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.pnlDatePickers.ResumeLayout(false);
            this.pnlDatePickers.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDateRange;
        private System.Windows.Forms.Panel pnlDatePickers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.DataGridView dgvReport;
    }
}
