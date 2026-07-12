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

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Departmental P&L Report";

            // pnlFilter
            this.pnlFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.pnlFilter.Location = new System.Drawing.Point(12, 45);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Padding = new System.Windows.Forms.Padding(10);
            this.pnlFilter.Size = new System.Drawing.Size(250, 500);
            this.pnlFilter.TabIndex = 1;
            this.pnlFilter.TabStop = false;
            this.pnlFilter.Text = "Filters";

            // lblFromDate
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Location = new System.Drawing.Point(10, 25);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(62, 13);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "From Date:";

            // dtpFromDate
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(10, 45);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(220, 22);
            this.dtpFromDate.TabIndex = 1;

            // lblToDate
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(10, 75);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(51, 13);
            this.lblToDate.TabIndex = 2;
            this.lblToDate.Text = "To Date:";

            // dtpToDate
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(10, 95);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(220, 22);
            this.dtpToDate.TabIndex = 3;

            // lblCostCenters
            this.lblCostCenters.AutoSize = true;
            this.lblCostCenters.Location = new System.Drawing.Point(10, 125);
            this.lblCostCenters.Name = "lblCostCenters";
            this.lblCostCenters.Size = new System.Drawing.Size(87, 13);
            this.lblCostCenters.TabIndex = 4;
            this.lblCostCenters.Text = "Cost Centers:";

            // chkListCostCenters
            this.chkListCostCenters.CheckOnClick = true;
            this.chkListCostCenters.Location = new System.Drawing.Point(10, 145);
            this.chkListCostCenters.Name = "chkListCostCenters";
            this.chkListCostCenters.Size = new System.Drawing.Size(220, 150);
            this.chkListCostCenters.TabIndex = 5;

            // btnSelectAll
            this.btnSelectAll.Location = new System.Drawing.Point(10, 305);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(105, 25);
            this.btnSelectAll.TabIndex = 6;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;

            // btnClearSelection
            this.btnClearSelection.Location = new System.Drawing.Point(120, 305);
            this.btnClearSelection.Name = "btnClearSelection";
            this.btnClearSelection.Size = new System.Drawing.Size(110, 25);
            this.btnClearSelection.TabIndex = 7;
            this.btnClearSelection.Text = "Clear Selection";
            this.btnClearSelection.UseVisualStyleBackColor = true;

            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(10, 340);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(220, 28);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "Refresh Report";
            this.btnRefresh.UseVisualStyleBackColor = true;

            // btnCompare
            this.btnCompare.Location = new System.Drawing.Point(10, 375);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(220, 28);
            this.btnCompare.TabIndex = 9;
            this.btnCompare.Text = "Compare Year";
            this.btnCompare.UseVisualStyleBackColor = true;

            // btnExportExcel
            this.btnExportExcel.Location = new System.Drawing.Point(10, 410);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(220, 28);
            this.btnExportExcel.TabIndex = 10;
            this.btnExportExcel.Text = "Export to Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;

            // pnlReport
            this.pnlReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlReport.Controls.Add(this.dgvReport);
            this.pnlReport.Controls.Add(this.lblRecordCount);
            this.pnlReport.Location = new System.Drawing.Point(270, 45);
            this.pnlReport.Name = "pnlReport";
            this.pnlReport.Size = new System.Drawing.Size(682, 500);
            this.pnlReport.TabIndex = 2;

            // dgvReport
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvReport.BackgroundColor = System.Drawing.Color.White;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Location = new System.Drawing.Point(5, 5);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.Size = new System.Drawing.Size(672, 468);
            this.dgvReport.TabIndex = 0;

            // lblRecordCount
            this.lblRecordCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecordCount.AutoSize = true;
            this.lblRecordCount.Location = new System.Drawing.Point(5, 480);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(63, 13);
            this.lblRecordCount.TabIndex = 1;
            this.lblRecordCount.Text = "Records: 0";

            // frm_departmental_pl
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 560);
            this.Controls.Add(this.pnlReport);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "frm_departmental_pl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Departmental P&L Report";
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
