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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_financial_periods));
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
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
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
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.Name = "panelHeader";
            // 
            // lblYearCaption
            // 
            resources.ApplyResources(this.lblYearCaption, "lblYearCaption");
            this.lblYearCaption.Name = "lblYearCaption";
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // toolActions
            // 
            this.toolActions.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenNewPeriod,
            this.btnSoftClose,
            this.btnHardLock,
            this.btnReopen,
            this.btnViewTransactions,
            this.btnYearEndClose,
            this.btnHelp});
            resources.ApplyResources(this.toolActions, "toolActions");
            this.toolActions.Name = "toolActions";
            // 
            // btnOpenNewPeriod
            // 
            this.btnOpenNewPeriod.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnOpenNewPeriod.Name = "btnOpenNewPeriod";
            resources.ApplyResources(this.btnOpenNewPeriod, "btnOpenNewPeriod");
            this.btnOpenNewPeriod.Tag = "finance.edit";
            this.btnOpenNewPeriod.Click += new System.EventHandler(this.btnOpenNewPeriod_Click);
            // 
            // btnSoftClose
            // 
            this.btnSoftClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSoftClose.Name = "btnSoftClose";
            resources.ApplyResources(this.btnSoftClose, "btnSoftClose");
            this.btnSoftClose.Tag = "finance.edit";
            this.btnSoftClose.Click += new System.EventHandler(this.btnSoftClose_Click);
            // 
            // btnHardLock
            // 
            this.btnHardLock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnHardLock.Name = "btnHardLock";
            resources.ApplyResources(this.btnHardLock, "btnHardLock");
            this.btnHardLock.Tag = "finance.edit";
            this.btnHardLock.Click += new System.EventHandler(this.btnHardLock_Click);
            // 
            // btnReopen
            // 
            this.btnReopen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnReopen.Name = "btnReopen";
            resources.ApplyResources(this.btnReopen, "btnReopen");
            this.btnReopen.Tag = "finance.edit";
            this.btnReopen.Click += new System.EventHandler(this.btnReopen_Click);
            // 
            // btnViewTransactions
            // 
            this.btnViewTransactions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnViewTransactions.Name = "btnViewTransactions";
            resources.ApplyResources(this.btnViewTransactions, "btnViewTransactions");
            this.btnViewTransactions.Tag = "finance.view";
            this.btnViewTransactions.Click += new System.EventHandler(this.btnViewTransactions_Click);
            // 
            // btnYearEndClose
            // 
            this.btnYearEndClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnYearEndClose.Name = "btnYearEndClose";
            resources.ApplyResources(this.btnYearEndClose, "btnYearEndClose");
            this.btnYearEndClose.Tag = "finance.edit";
            this.btnYearEndClose.Click += new System.EventHandler(this.btnYearEndClose_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnHelp.Name = "btnHelp";
            resources.ApplyResources(this.btnHelp, "btnHelp");
            this.btnHelp.Tag = "finance.view";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.gridPeriods);
            resources.ApplyResources(this.panelBody, "panelBody");
            this.panelBody.Name = "panelBody";
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
            resources.ApplyResources(this.gridPeriods, "gridPeriods");
            this.gridPeriods.MultiSelect = false;
            this.gridPeriods.Name = "gridPeriods";
            this.gridPeriods.ReadOnly = true;
            this.gridPeriods.RowHeadersVisible = false;
            this.gridPeriods.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPeriods.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gridPeriods_CellPainting);
            this.gridPeriods.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridPeriods_MouseDown);
            // 
            // colPeriodId
            // 
            this.colPeriodId.DataPropertyName = "period_id";
            resources.ApplyResources(this.colPeriodId, "colPeriodId");
            this.colPeriodId.Name = "colPeriodId";
            this.colPeriodId.ReadOnly = true;
            // 
            // colYearId
            // 
            this.colYearId.DataPropertyName = "year_id";
            resources.ApplyResources(this.colYearId, "colYearId");
            this.colYearId.Name = "colYearId";
            this.colYearId.ReadOnly = true;
            // 
            // colFinancialYear
            // 
            this.colFinancialYear.DataPropertyName = "financial_year";
            resources.ApplyResources(this.colFinancialYear, "colFinancialYear");
            this.colFinancialYear.Name = "colFinancialYear";
            this.colFinancialYear.ReadOnly = true;
            // 
            // colPeriodName
            // 
            this.colPeriodName.DataPropertyName = "period_name";
            resources.ApplyResources(this.colPeriodName, "colPeriodName");
            this.colPeriodName.Name = "colPeriodName";
            this.colPeriodName.ReadOnly = true;
            // 
            // colStartDate
            // 
            this.colStartDate.DataPropertyName = "start_date";
            resources.ApplyResources(this.colStartDate, "colStartDate");
            this.colStartDate.Name = "colStartDate";
            this.colStartDate.ReadOnly = true;
            // 
            // colEndDate
            // 
            this.colEndDate.DataPropertyName = "end_date";
            resources.ApplyResources(this.colEndDate, "colEndDate");
            this.colEndDate.Name = "colEndDate";
            this.colEndDate.ReadOnly = true;
            // 
            // colStatusBadge
            // 
            this.colStatusBadge.DataPropertyName = "status";
            resources.ApplyResources(this.colStatusBadge, "colStatusBadge");
            this.colStatusBadge.Name = "colStatusBadge";
            this.colStatusBadge.ReadOnly = true;
            // 
            // colClosedBy
            // 
            this.colClosedBy.DataPropertyName = "closed_by";
            resources.ApplyResources(this.colClosedBy, "colClosedBy");
            this.colClosedBy.Name = "colClosedBy";
            this.colClosedBy.ReadOnly = true;
            // 
            // colClosedAt
            // 
            this.colClosedAt.DataPropertyName = "closed_at";
            resources.ApplyResources(this.colClosedAt, "colClosedAt");
            this.colClosedAt.Name = "colClosedAt";
            this.colClosedAt.ReadOnly = true;
            // 
            // colTransactionsCount
            // 
            this.colTransactionsCount.DataPropertyName = "transactions_count";
            resources.ApplyResources(this.colTransactionsCount, "colTransactionsCount");
            this.colTransactionsCount.Name = "colTransactionsCount";
            this.colTransactionsCount.ReadOnly = true;
            // 
            // colCanReopen
            // 
            this.colCanReopen.DataPropertyName = "can_reopen";
            resources.ApplyResources(this.colCanReopen, "colCanReopen");
            this.colCanReopen.Name = "colCanReopen";
            this.colCanReopen.ReadOnly = true;
            // 
            // contextPeriodActions
            // 
            this.contextPeriodActions.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextPeriodActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxOpenNewPeriod,
            this.ctxSoftClose,
            this.ctxHardLock,
            this.ctxReopen,
            this.ctxViewTransactions});
            this.contextPeriodActions.Name = "contextPeriodActions";
            resources.ApplyResources(this.contextPeriodActions, "contextPeriodActions");
            // 
            // ctxOpenNewPeriod
            // 
            this.ctxOpenNewPeriod.Name = "ctxOpenNewPeriod";
            resources.ApplyResources(this.ctxOpenNewPeriod, "ctxOpenNewPeriod");
            this.ctxOpenNewPeriod.Click += new System.EventHandler(this.btnOpenNewPeriod_Click);
            // 
            // ctxSoftClose
            // 
            this.ctxSoftClose.Name = "ctxSoftClose";
            resources.ApplyResources(this.ctxSoftClose, "ctxSoftClose");
            this.ctxSoftClose.Click += new System.EventHandler(this.btnSoftClose_Click);
            // 
            // ctxHardLock
            // 
            this.ctxHardLock.Name = "ctxHardLock";
            resources.ApplyResources(this.ctxHardLock, "ctxHardLock");
            this.ctxHardLock.Click += new System.EventHandler(this.btnHardLock_Click);
            // 
            // ctxReopen
            // 
            this.ctxReopen.Name = "ctxReopen";
            resources.ApplyResources(this.ctxReopen, "ctxReopen");
            this.ctxReopen.Click += new System.EventHandler(this.btnReopen_Click);
            // 
            // ctxViewTransactions
            // 
            this.ctxViewTransactions.Name = "ctxViewTransactions";
            resources.ApplyResources(this.ctxViewTransactions, "ctxViewTransactions");
            this.ctxViewTransactions.Click += new System.EventHandler(this.btnViewTransactions_Click);
            // 
            // frm_financial_periods
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.toolActions);
            this.Controls.Add(this.panelHeader);
            this.KeyPreview = true;
            this.Name = "frm_financial_periods";
            this.Load += new System.EventHandler(this.frm_financial_periods_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_financial_periods_KeyDown);
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
        private System.Windows.Forms.ToolStripButton btnHelp;
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
