namespace pos
{
    partial class frm_financial_periods
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
            this.components = new System.ComponentModel.Container();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblYearCaption = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.toolActions = new System.Windows.Forms.ToolStrip();
            this.btnOpenNewPeriod = new System.Windows.Forms.ToolStripButton();
            this.btnSoftClose = new System.Windows.Forms.ToolStripButton();
            this.btnHardLock = new System.Windows.Forms.ToolStripButton();
            this.btnReopen = new System.Windows.Forms.ToolStripButton();
            this.btnViewTransactions = new System.Windows.Forms.ToolStripButton();
            this.btnYearEndClose = new System.Windows.Forms.ToolStripButton();
            this.panelBody = new System.Windows.Forms.Panel();
            this.gridPeriods = new System.Windows.Forms.DataGridView();
            this.colPeriodId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYearId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFinancialYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeriodName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatusBadge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClosedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransactionsCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCanReopen = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.contextPeriodActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxOpenNewPeriod = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxSoftClose = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxHardLock = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxReopen = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxViewTransactions = new System.Windows.Forms.ToolStripMenuItem();
            this.panelHeader.SuspendLayout();
            this.toolActions.SuspendLayout();
            this.panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPeriods)).BeginInit();
            this.contextPeriodActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.lblYearCaption);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1284, 63);
            this.panelHeader.TabIndex = 0;
            // 
            // lblYearCaption
            // 
            this.lblYearCaption.AutoSize = true;
            this.lblYearCaption.Location = new System.Drawing.Point(16, 37);
            this.lblYearCaption.Name = "lblYearCaption";
            this.lblYearCaption.Size = new System.Drawing.Size(106, 17);
            this.lblYearCaption.TabIndex = 1;
            this.lblYearCaption.Text = "Active Year: N/A";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblTitle.Location = new System.Drawing.Point(14, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(239, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Financial Period Management";
            // 
            // toolActions
            // 
            this.toolActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenNewPeriod,
            this.btnSoftClose,
            this.btnHardLock,
            this.btnReopen,
            this.btnViewTransactions,
            this.btnYearEndClose});
            this.toolActions.Location = new System.Drawing.Point(0, 63);
            this.toolActions.Name = "toolActions";
            this.toolActions.Size = new System.Drawing.Size(1284, 25);
            this.toolActions.TabIndex = 1;
            this.toolActions.Text = "toolStrip1";
            // 
            // btnOpenNewPeriod
            // 
            this.btnOpenNewPeriod.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnOpenNewPeriod.Name = "btnOpenNewPeriod";
            this.btnOpenNewPeriod.Size = new System.Drawing.Size(106, 22);
            this.btnOpenNewPeriod.Tag = "finance.edit";
            this.btnOpenNewPeriod.Text = "Open New Period";
            this.btnOpenNewPeriod.Click += new System.EventHandler(this.btnOpenNewPeriod_Click);
            // 
            // btnSoftClose
            // 
            this.btnSoftClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSoftClose.Name = "btnSoftClose";
            this.btnSoftClose.Size = new System.Drawing.Size(97, 22);
            this.btnSoftClose.Tag = "finance.edit";
            this.btnSoftClose.Text = "Soft Close Period";
            this.btnSoftClose.Click += new System.EventHandler(this.btnSoftClose_Click);
            // 
            // btnHardLock
            // 
            this.btnHardLock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnHardLock.Name = "btnHardLock";
            this.btnHardLock.Size = new System.Drawing.Size(95, 22);
            this.btnHardLock.Tag = "finance.edit";
            this.btnHardLock.Text = "Hard Lock Period";
            this.btnHardLock.Click += new System.EventHandler(this.btnHardLock_Click);
            // 
            // btnReopen
            // 
            this.btnReopen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnReopen.Name = "btnReopen";
            this.btnReopen.Size = new System.Drawing.Size(84, 22);
            this.btnReopen.Tag = "finance.edit";
            this.btnReopen.Text = "Reopen Period";
            this.btnReopen.Click += new System.EventHandler(this.btnReopen_Click);
            // 
            // btnViewTransactions
            // 
            this.btnViewTransactions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnViewTransactions.Name = "btnViewTransactions";
            this.btnViewTransactions.Size = new System.Drawing.Size(135, 22);
            this.btnViewTransactions.Tag = "finance.view";
            this.btnViewTransactions.Text = "View Period Transactions";
            this.btnViewTransactions.Click += new System.EventHandler(this.btnViewTransactions_Click);
            // 
            // btnYearEndClose
            // 
            this.btnYearEndClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnYearEndClose.Name = "btnYearEndClose";
            this.btnYearEndClose.Size = new System.Drawing.Size(132, 22);
            this.btnYearEndClose.Tag = "finance.edit";
            this.btnYearEndClose.Text = "Year-End Closing...";
            this.btnYearEndClose.Click += new System.EventHandler(this.btnYearEndClose_Click);
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.gridPeriods);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 88);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(8);
            this.panelBody.Size = new System.Drawing.Size(1284, 573);
            this.panelBody.TabIndex = 2;
            // 
            // gridPeriods
            // 
            this.gridPeriods.AllowUserToAddRows = false;
            this.gridPeriods.AllowUserToDeleteRows = false;
            this.gridPeriods.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridPeriods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPeriods.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPeriodId,
            this.colYearId,
            this.colFinancialYear,
            this.colPeriodName,
            this.colStartDate,
            this.colEndDate,
            this.colStatusBadge,
            this.colClosedBy,
            this.colClosedAt,
            this.colTransactionsCount,
            this.colCanReopen});
            this.gridPeriods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPeriods.Location = new System.Drawing.Point(8, 8);
            this.gridPeriods.MultiSelect = false;
            this.gridPeriods.Name = "gridPeriods";
            this.gridPeriods.ReadOnly = true;
            this.gridPeriods.RowHeadersVisible = false;
            this.gridPeriods.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPeriods.Size = new System.Drawing.Size(1268, 557);
            this.gridPeriods.TabIndex = 0;
            this.gridPeriods.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gridPeriods_CellPainting);
            this.gridPeriods.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridPeriods_MouseDown);
            // 
            // colPeriodId
            // 
            this.colPeriodId.DataPropertyName = "period_id";
            this.colPeriodId.HeaderText = "Period ID";
            this.colPeriodId.Name = "colPeriodId";
            this.colPeriodId.ReadOnly = true;
            this.colPeriodId.Visible = false;
            // 
            // colYearId
            // 
            this.colYearId.DataPropertyName = "year_id";
            this.colYearId.HeaderText = "Year ID";
            this.colYearId.Name = "colYearId";
            this.colYearId.ReadOnly = true;
            this.colYearId.Visible = false;
            // 
            // colFinancialYear
            // 
            this.colFinancialYear.DataPropertyName = "financial_year";
            this.colFinancialYear.HeaderText = "Financial Year";
            this.colFinancialYear.Name = "colFinancialYear";
            this.colFinancialYear.ReadOnly = true;
            // 
            // colPeriodName
            // 
            this.colPeriodName.DataPropertyName = "period_name";
            this.colPeriodName.HeaderText = "Period Name";
            this.colPeriodName.Name = "colPeriodName";
            this.colPeriodName.ReadOnly = true;
            // 
            // colStartDate
            // 
            this.colStartDate.DataPropertyName = "start_date";
            this.colStartDate.HeaderText = "Start Date";
            this.colStartDate.Name = "colStartDate";
            this.colStartDate.ReadOnly = true;
            // 
            // colEndDate
            // 
            this.colEndDate.DataPropertyName = "end_date";
            this.colEndDate.HeaderText = "End Date";
            this.colEndDate.Name = "colEndDate";
            this.colEndDate.ReadOnly = true;
            // 
            // colStatusBadge
            // 
            this.colStatusBadge.DataPropertyName = "status";
            this.colStatusBadge.HeaderText = "Status";
            this.colStatusBadge.Name = "colStatusBadge";
            this.colStatusBadge.ReadOnly = true;
            // 
            // colClosedBy
            // 
            this.colClosedBy.DataPropertyName = "closed_by";
            this.colClosedBy.HeaderText = "Closed By";
            this.colClosedBy.Name = "colClosedBy";
            this.colClosedBy.ReadOnly = true;
            // 
            // colClosedAt
            // 
            this.colClosedAt.DataPropertyName = "closed_at";
            this.colClosedAt.HeaderText = "Closed At";
            this.colClosedAt.Name = "colClosedAt";
            this.colClosedAt.ReadOnly = true;
            // 
            // colTransactionsCount
            // 
            this.colTransactionsCount.DataPropertyName = "transactions_count";
            this.colTransactionsCount.HeaderText = "Transactions Count";
            this.colTransactionsCount.Name = "colTransactionsCount";
            this.colTransactionsCount.ReadOnly = true;
            // 
            // colCanReopen
            // 
            this.colCanReopen.DataPropertyName = "can_reopen";
            this.colCanReopen.HeaderText = "Can Reopen";
            this.colCanReopen.Name = "colCanReopen";
            this.colCanReopen.ReadOnly = true;
            // 
            // contextPeriodActions
            // 
            this.contextPeriodActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxOpenNewPeriod,
            this.ctxSoftClose,
            this.ctxHardLock,
            this.ctxReopen,
            this.ctxViewTransactions});
            this.contextPeriodActions.Name = "contextPeriodActions";
            this.contextPeriodActions.Size = new System.Drawing.Size(199, 114);
            // 
            // ctxOpenNewPeriod
            // 
            this.ctxOpenNewPeriod.Name = "ctxOpenNewPeriod";
            this.ctxOpenNewPeriod.Size = new System.Drawing.Size(198, 22);
            this.ctxOpenNewPeriod.Text = "Open New Period";
            this.ctxOpenNewPeriod.Click += new System.EventHandler(this.btnOpenNewPeriod_Click);
            // 
            // ctxSoftClose
            // 
            this.ctxSoftClose.Name = "ctxSoftClose";
            this.ctxSoftClose.Size = new System.Drawing.Size(198, 22);
            this.ctxSoftClose.Text = "Soft Close Period";
            this.ctxSoftClose.Click += new System.EventHandler(this.btnSoftClose_Click);
            // 
            // ctxHardLock
            // 
            this.ctxHardLock.Name = "ctxHardLock";
            this.ctxHardLock.Size = new System.Drawing.Size(198, 22);
            this.ctxHardLock.Text = "Hard Lock Period";
            this.ctxHardLock.Click += new System.EventHandler(this.btnHardLock_Click);
            // 
            // ctxReopen
            // 
            this.ctxReopen.Name = "ctxReopen";
            this.ctxReopen.Size = new System.Drawing.Size(198, 22);
            this.ctxReopen.Text = "Reopen Period";
            this.ctxReopen.Click += new System.EventHandler(this.btnReopen_Click);
            // 
            // ctxViewTransactions
            // 
            this.ctxViewTransactions.Name = "ctxViewTransactions";
            this.ctxViewTransactions.Size = new System.Drawing.Size(198, 22);
            this.ctxViewTransactions.Text = "View Period Transactions";
            this.ctxViewTransactions.Click += new System.EventHandler(this.btnViewTransactions_Click);
            // 
            // frm_financial_periods
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 661);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.toolActions);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "frm_financial_periods";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Financial Period Management";
            this.Load += new System.EventHandler(this.frm_financial_periods_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.toolActions.ResumeLayout(false);
            this.toolActions.PerformLayout();
            this.panelBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPeriods)).EndInit();
            this.contextPeriodActions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ToolStrip toolActions;
        private System.Windows.Forms.ToolStripButton btnOpenNewPeriod;
        private System.Windows.Forms.ToolStripButton btnSoftClose;
        private System.Windows.Forms.ToolStripButton btnHardLock;
        private System.Windows.Forms.ToolStripButton btnReopen;
        private System.Windows.Forms.ToolStripButton btnViewTransactions;
        private System.Windows.Forms.ToolStripButton btnYearEndClose;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.DataGridView gridPeriods;
        private System.Windows.Forms.ContextMenuStrip contextPeriodActions;
        private System.Windows.Forms.ToolStripMenuItem ctxOpenNewPeriod;
        private System.Windows.Forms.ToolStripMenuItem ctxSoftClose;
        private System.Windows.Forms.ToolStripMenuItem ctxHardLock;
        private System.Windows.Forms.ToolStripMenuItem ctxReopen;
        private System.Windows.Forms.ToolStripMenuItem ctxViewTransactions;
        private System.Windows.Forms.Label lblYearCaption;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeriodId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYearId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFinancialYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeriodName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatusBadge;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClosedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransactionsCount;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCanReopen;
    }
}
