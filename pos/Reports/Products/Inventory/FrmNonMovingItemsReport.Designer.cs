namespace pos.Reports.Products.Inventory
{
    partial class FrmNonMovingItemsReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNonMovingItemsReport));
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.lblDaysThreshold = new System.Windows.Forms.Label();
            this.nudDaysThreshold = new System.Windows.Forms.NumericUpDown();
            this.lblMinQty = new System.Windows.Forms.Label();
            this.nudMinQty = new System.Windows.Forms.NumericUpDown();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblBrand = new System.Windows.Forms.Label();
            this.cmbBrand = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.pnlSummary = new System.Windows.Forms.Panel();
            this.lblTotalItems = new System.Windows.Forms.Label();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblDormantDays = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDaysThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.pnlSummary.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFilters
            // 
            this.pnlFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.pnlFilters.Controls.Add(this.lblDaysThreshold);
            this.pnlFilters.Controls.Add(this.nudDaysThreshold);
            this.pnlFilters.Controls.Add(this.lblMinQty);
            this.pnlFilters.Controls.Add(this.nudMinQty);
            this.pnlFilters.Controls.Add(this.lblCategory);
            this.pnlFilters.Controls.Add(this.cmbCategory);
            this.pnlFilters.Controls.Add(this.lblBrand);
            this.pnlFilters.Controls.Add(this.cmbBrand);
            this.pnlFilters.Controls.Add(this.lblLocation);
            this.pnlFilters.Controls.Add(this.cmbLocation);
            this.pnlFilters.Controls.Add(this.btnLoad);
            this.pnlFilters.Controls.Add(this.btnExport);
            this.pnlFilters.Controls.Add(this.btnPrint);
            this.pnlFilters.Controls.Add(this.btnClear);
            resources.ApplyResources(this.pnlFilters, "pnlFilters");
            this.pnlFilters.Name = "pnlFilters";
            // 
            // lblDaysThreshold
            // 
            resources.ApplyResources(this.lblDaysThreshold, "lblDaysThreshold");
            this.lblDaysThreshold.Name = "lblDaysThreshold";
            // 
            // nudDaysThreshold
            // 
            resources.ApplyResources(this.nudDaysThreshold, "nudDaysThreshold");
            this.nudDaysThreshold.Maximum = new decimal(new int[] {
            1095,
            0,
            0,
            0});
            this.nudDaysThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDaysThreshold.Name = "nudDaysThreshold";
            this.nudDaysThreshold.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // lblMinQty
            // 
            resources.ApplyResources(this.lblMinQty, "lblMinQty");
            this.lblMinQty.Name = "lblMinQty";
            // 
            // nudMinQty
            // 
            resources.ApplyResources(this.nudMinQty, "nudMinQty");
            this.nudMinQty.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudMinQty.Name = "nudMinQty";
            this.nudMinQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblCategory
            // 
            resources.ApplyResources(this.lblCategory, "lblCategory");
            this.lblCategory.Name = "lblCategory";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCategory, "cmbCategory");
            this.cmbCategory.Name = "cmbCategory";
            // 
            // lblBrand
            // 
            resources.ApplyResources(this.lblBrand, "lblBrand");
            this.lblBrand.Name = "lblBrand";
            // 
            // cmbBrand
            // 
            this.cmbBrand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBrand.FormattingEnabled = true;
            resources.ApplyResources(this.cmbBrand, "cmbBrand");
            this.cmbBrand.Name = "cmbBrand";
            // 
            // lblLocation
            // 
            resources.ApplyResources(this.lblLocation, "lblLocation");
            this.lblLocation.Name = "lblLocation";
            // 
            // cmbLocation
            // 
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLocation, "cmbLocation");
            this.cmbLocation.Name = "cmbLocation";
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.btnLoad.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnLoad, "btnLoad");
            this.btnLoad.ForeColor = System.Drawing.Color.White;
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(136)))));
            this.btnExport.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(136)))));
            this.btnPrint.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.btnClear.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(49)))), ((int)(((byte)(48)))));
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvReport.BackgroundColor = System.Drawing.Color.White;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvReport, "dgvReport");
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // pnlSummary
            // 
            this.pnlSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(249)))), ((int)(((byte)(248)))));
            this.pnlSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSummary.Controls.Add(this.lblTotalItems);
            this.pnlSummary.Controls.Add(this.lblTotalValue);
            this.pnlSummary.Controls.Add(this.lblDormantDays);
            resources.ApplyResources(this.pnlSummary, "pnlSummary");
            this.pnlSummary.Name = "pnlSummary";
            // 
            // lblTotalItems
            // 
            resources.ApplyResources(this.lblTotalItems, "lblTotalItems");
            this.lblTotalItems.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(49)))), ((int)(((byte)(48)))));
            this.lblTotalItems.Name = "lblTotalItems";
            // 
            // lblTotalValue
            // 
            resources.ApplyResources(this.lblTotalValue, "lblTotalValue");
            this.lblTotalValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(49)))), ((int)(((byte)(48)))));
            this.lblTotalValue.Name = "lblTotalValue";
            // 
            // lblDormantDays
            // 
            resources.ApplyResources(this.lblDormantDays, "lblDormantDays");
            this.lblDormantDays.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(49)))), ((int)(((byte)(48)))));
            this.lblDormantDays.Name = "lblDormantDays";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            resources.ApplyResources(this.lblStatus, "lblStatus");
            // 
            // FrmNonMovingItemsReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.pnlSummary);
            this.Controls.Add(this.statusStrip1);
            this.Name = "FrmNonMovingItemsReport";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmNonMovingItemsReport_Load);
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDaysThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.pnlSummary.ResumeLayout(false);
            this.pnlSummary.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label lblDaysThreshold;
        private System.Windows.Forms.NumericUpDown nudDaysThreshold;
        private System.Windows.Forms.Label lblMinQty;
        private System.Windows.Forms.NumericUpDown nudMinQty;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblBrand;
        private System.Windows.Forms.ComboBox cmbBrand;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Label lblTotalItems;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label lblDormantDays;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}
