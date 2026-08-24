namespace pos.Accounting.CostCenter
{
    partial class frm_departmental_pl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_departmental_pl));
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFilter = new System.Windows.Forms.GroupBox();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblCostCenters = new System.Windows.Forms.Label();
            this.chkListCostCenters = new System.Windows.Forms.CheckedListBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnClearSelection = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.pnlReport = new System.Windows.Forms.Panel();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.pnlFilter.SuspendLayout();
            this.pnlReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // pnlFilter
            // 
            resources.ApplyResources(this.pnlFilter, "pnlFilter");
            this.pnlFilter.Controls.Add(this.lblFromDate);
            this.pnlFilter.Controls.Add(this.dtpFromDate);
            this.pnlFilter.Controls.Add(this.lblToDate);
            this.pnlFilter.Controls.Add(this.dtpToDate);
            this.pnlFilter.Controls.Add(this.lblCostCenters);
            this.pnlFilter.Controls.Add(this.chkListCostCenters);
            this.pnlFilter.Controls.Add(this.btnSelectAll);
            this.pnlFilter.Controls.Add(this.btnClearSelection);
            this.pnlFilter.Controls.Add(this.btnRefresh);
            this.pnlFilter.Controls.Add(this.btnCompare);
            this.pnlFilter.Controls.Add(this.btnExportExcel);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.TabStop = false;
            // 
            // lblFromDate
            // 
            resources.ApplyResources(this.lblFromDate, "lblFromDate");
            this.lblFromDate.Name = "lblFromDate";
            // 
            // dtpFromDate
            // 
            resources.ApplyResources(this.dtpFromDate, "dtpFromDate");
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Name = "dtpFromDate";
            // 
            // lblToDate
            // 
            resources.ApplyResources(this.lblToDate, "lblToDate");
            this.lblToDate.Name = "lblToDate";
            // 
            // dtpToDate
            // 
            resources.ApplyResources(this.dtpToDate, "dtpToDate");
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Name = "dtpToDate";
            // 
            // lblCostCenters
            // 
            resources.ApplyResources(this.lblCostCenters, "lblCostCenters");
            this.lblCostCenters.Name = "lblCostCenters";
            // 
            // chkListCostCenters
            // 
            resources.ApplyResources(this.chkListCostCenters, "chkListCostCenters");
            this.chkListCostCenters.CheckOnClick = true;
            this.chkListCostCenters.Name = "chkListCostCenters";
            // 
            // btnSelectAll
            // 
            resources.ApplyResources(this.btnSelectAll, "btnSelectAll");
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            // 
            // btnClearSelection
            // 
            resources.ApplyResources(this.btnClearSelection, "btnClearSelection");
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // btnCompare
            // 
            resources.ApplyResources(this.btnCompare, "btnCompare");
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.UseVisualStyleBackColor = true;
            // 
            // btnExportExcel
            // 
            resources.ApplyResources(this.btnExportExcel, "btnExportExcel");
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            // 
            // pnlReport
            // 
            resources.ApplyResources(this.pnlReport, "pnlReport");
            this.pnlReport.Controls.Add(this.dgvReport);
            this.pnlReport.Controls.Add(this.lblRecordCount);
            this.pnlReport.Name = "pnlReport";
            // 
            // dgvReport
            // 
            resources.ApplyResources(this.dgvReport, "dgvReport");
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvReport.BackgroundColor = System.Drawing.Color.White;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            // 
            // lblRecordCount
            // 
            resources.ApplyResources(this.lblRecordCount, "lblRecordCount");
            this.lblRecordCount.Name = "lblRecordCount";
            // 
            // frm_departmental_pl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlReport);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.lblTitle);
            this.Name = "frm_departmental_pl";
            this.Load += new System.EventHandler(this.FrmDepartmentalPL_Load);
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.pnlReport.ResumeLayout(false);
            this.pnlReport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox pnlFilter;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label lblCostCenters;
        private System.Windows.Forms.CheckedListBox chkListCostCenters;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnClearSelection;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Panel pnlReport;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Label lblRecordCount;
    }
}
