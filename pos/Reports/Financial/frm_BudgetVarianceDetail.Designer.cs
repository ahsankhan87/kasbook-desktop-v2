namespace pos.Reports.Financial
{
    partial class frm_BudgetVarianceDetail
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelActions = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnViewLedger = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.chartMonthly = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvMonthly = new System.Windows.Forms.DataGridView();
            this.panelTop.SuspendLayout();
            this.panelActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthly)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonthly)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(10, 10, 10, 4);
            this.panelTop.Size = new System.Drawing.Size(1084, 50);
            this.panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.lblTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1064, 36);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Variance Detail";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelActions
            // 
            this.panelActions.Controls.Add(this.btnClose);
            this.panelActions.Controls.Add(this.btnViewLedger);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelActions.Location = new System.Drawing.Point(0, 639);
            this.panelActions.Name = "panelActions";
            this.panelActions.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.panelActions.Size = new System.Drawing.Size(1084, 52);
            this.panelActions.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(972, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 32);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnViewLedger
            // 
            this.btnViewLedger.Location = new System.Drawing.Point(12, 10);
            this.btnViewLedger.Name = "btnViewLedger";
            this.btnViewLedger.Size = new System.Drawing.Size(130, 32);
            this.btnViewLedger.TabIndex = 0;
            this.btnViewLedger.Text = "View Ledger";
            this.btnViewLedger.UseVisualStyleBackColor = true;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 50);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.chartMonthly);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.dgvMonthly);
            this.splitMain.Size = new System.Drawing.Size(1084, 589);
            this.splitMain.SplitterDistance = 286;
            this.splitMain.TabIndex = 1;
            // 
            // chartMonthly
            // 
            this.chartMonthly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartMonthly.Location = new System.Drawing.Point(0, 0);
            this.chartMonthly.Name = "chartMonthly";
            this.chartMonthly.Size = new System.Drawing.Size(1084, 286);
            this.chartMonthly.TabIndex = 0;
            this.chartMonthly.Text = "chart1";
            // 
            // dgvMonthly
            // 
            this.dgvMonthly.AllowUserToAddRows = false;
            this.dgvMonthly.AllowUserToDeleteRows = false;
            this.dgvMonthly.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvMonthly.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMonthly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMonthly.Location = new System.Drawing.Point(0, 0);
            this.dgvMonthly.MultiSelect = false;
            this.dgvMonthly.Name = "dgvMonthly";
            this.dgvMonthly.ReadOnly = true;
            this.dgvMonthly.RowHeadersVisible = false;
            this.dgvMonthly.RowHeadersWidth = 51;
            this.dgvMonthly.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMonthly.Size = new System.Drawing.Size(1084, 299);
            this.dgvMonthly.TabIndex = 0;
            // 
            // frm_BudgetVarianceDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 691);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.panelActions);
            this.Controls.Add(this.panelTop);
            this.Name = "frm_BudgetVarianceDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Budget Variance Detail";
            this.panelTop.ResumeLayout(false);
            this.panelActions.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthly)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonthly)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelActions;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnViewLedger;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMonthly;
        private System.Windows.Forms.DataGridView dgvMonthly;
    }
}
