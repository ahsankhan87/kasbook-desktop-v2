namespace pos
{
    partial class frm_year_end_close_wizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_year_end_close_wizard));
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.grpValidation = new System.Windows.Forms.GroupBox();
            this.gridValidation = new System.Windows.Forms.DataGridView();
            this.colCheckKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckPassed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCheckStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckFailedCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCheckDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelValidationActions = new System.Windows.Forms.Panel();
            this.btnRefreshValidation = new System.Windows.Forms.Button();
            this.grpProgress = new System.Windows.Forms.GroupBox();
            this.lstProgress = new System.Windows.Forms.ListBox();
            this.grpExecution = new System.Windows.Forms.GroupBox();
            this.lblResultValue = new System.Windows.Forms.Label();
            this.lblResultCaption = new System.Windows.Forms.Label();
            this.btnRollback = new System.Windows.Forms.Button();
            this.btnExecuteClose = new System.Windows.Forms.Button();
            this.txtConfirmClose = new System.Windows.Forms.TextBox();
            this.lblConfirmHint = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblYearValue = new System.Windows.Forms.Label();
            this.lblYearCaption = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.grpValidation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridValidation)).BeginInit();
            this.panelValidationActions.SuspendLayout();
            this.grpProgress.SuspendLayout();
            this.grpExecution.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            resources.ApplyResources(this.splitMain, "splitMain");
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            resources.ApplyResources(this.splitMain.Panel1, "splitMain.Panel1");
            this.splitMain.Panel1.Controls.Add(this.grpValidation);
            // 
            // splitMain.Panel2
            // 
            resources.ApplyResources(this.splitMain.Panel2, "splitMain.Panel2");
            this.splitMain.Panel2.Controls.Add(this.grpProgress);
            this.splitMain.Panel2.Controls.Add(this.grpExecution);
            // 
            // grpValidation
            // 
            resources.ApplyResources(this.grpValidation, "grpValidation");
            this.grpValidation.Controls.Add(this.gridValidation);
            this.grpValidation.Controls.Add(this.panelValidationActions);
            this.grpValidation.Name = "grpValidation";
            this.grpValidation.TabStop = false;
            // 
            // gridValidation
            // 
            resources.ApplyResources(this.gridValidation, "gridValidation");
            this.gridValidation.AllowUserToAddRows = false;
            this.gridValidation.AllowUserToDeleteRows = false;
            this.gridValidation.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridValidation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridValidation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheckKey,
            this.colCheckName,
            this.colCheckPassed,
            this.colCheckStatus,
            this.colCheckFailedCount,
            this.colCheckDetails});
            this.gridValidation.Name = "gridValidation";
            this.gridValidation.ReadOnly = true;
            this.gridValidation.RowHeadersVisible = false;
            this.gridValidation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // colCheckKey
            // 
            this.colCheckKey.DataPropertyName = "check_key";
            resources.ApplyResources(this.colCheckKey, "colCheckKey");
            this.colCheckKey.Name = "colCheckKey";
            this.colCheckKey.ReadOnly = true;
            // 
            // colCheckName
            // 
            this.colCheckName.DataPropertyName = "check_name";
            resources.ApplyResources(this.colCheckName, "colCheckName");
            this.colCheckName.Name = "colCheckName";
            this.colCheckName.ReadOnly = true;
            // 
            // colCheckPassed
            // 
            this.colCheckPassed.DataPropertyName = "is_passed";
            resources.ApplyResources(this.colCheckPassed, "colCheckPassed");
            this.colCheckPassed.Name = "colCheckPassed";
            this.colCheckPassed.ReadOnly = true;
            // 
            // colCheckStatus
            // 
            resources.ApplyResources(this.colCheckStatus, "colCheckStatus");
            this.colCheckStatus.Name = "colCheckStatus";
            this.colCheckStatus.ReadOnly = true;
            // 
            // colCheckFailedCount
            // 
            this.colCheckFailedCount.DataPropertyName = "failed_count";
            resources.ApplyResources(this.colCheckFailedCount, "colCheckFailedCount");
            this.colCheckFailedCount.Name = "colCheckFailedCount";
            this.colCheckFailedCount.ReadOnly = true;
            // 
            // colCheckDetails
            // 
            this.colCheckDetails.DataPropertyName = "details";
            resources.ApplyResources(this.colCheckDetails, "colCheckDetails");
            this.colCheckDetails.Name = "colCheckDetails";
            this.colCheckDetails.ReadOnly = true;
            // 
            // panelValidationActions
            // 
            resources.ApplyResources(this.panelValidationActions, "panelValidationActions");
            this.panelValidationActions.Controls.Add(this.btnRefreshValidation);
            this.panelValidationActions.Name = "panelValidationActions";
            // 
            // btnRefreshValidation
            // 
            resources.ApplyResources(this.btnRefreshValidation, "btnRefreshValidation");
            this.btnRefreshValidation.Name = "btnRefreshValidation";
            this.btnRefreshValidation.UseVisualStyleBackColor = true;
            this.btnRefreshValidation.Click += new System.EventHandler(this.btnRefreshValidation_Click);
            // 
            // grpProgress
            // 
            resources.ApplyResources(this.grpProgress, "grpProgress");
            this.grpProgress.Controls.Add(this.lstProgress);
            this.grpProgress.Name = "grpProgress";
            this.grpProgress.TabStop = false;
            // 
            // lstProgress
            // 
            resources.ApplyResources(this.lstProgress, "lstProgress");
            this.lstProgress.FormattingEnabled = true;
            this.lstProgress.Name = "lstProgress";
            // 
            // grpExecution
            // 
            resources.ApplyResources(this.grpExecution, "grpExecution");
            this.grpExecution.Controls.Add(this.lblResultValue);
            this.grpExecution.Controls.Add(this.lblResultCaption);
            this.grpExecution.Controls.Add(this.btnRollback);
            this.grpExecution.Controls.Add(this.btnExecuteClose);
            this.grpExecution.Controls.Add(this.txtConfirmClose);
            this.grpExecution.Controls.Add(this.lblConfirmHint);
            this.grpExecution.Name = "grpExecution";
            this.grpExecution.TabStop = false;
            // 
            // lblResultValue
            // 
            resources.ApplyResources(this.lblResultValue, "lblResultValue");
            this.lblResultValue.Name = "lblResultValue";
            // 
            // lblResultCaption
            // 
            resources.ApplyResources(this.lblResultCaption, "lblResultCaption");
            this.lblResultCaption.Name = "lblResultCaption";
            // 
            // btnRollback
            // 
            resources.ApplyResources(this.btnRollback, "btnRollback");
            this.btnRollback.Name = "btnRollback";
            this.btnRollback.UseVisualStyleBackColor = true;
            this.btnRollback.Click += new System.EventHandler(this.btnRollback_Click);
            // 
            // btnExecuteClose
            // 
            resources.ApplyResources(this.btnExecuteClose, "btnExecuteClose");
            this.btnExecuteClose.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnExecuteClose.ForeColor = System.Drawing.Color.White;
            this.btnExecuteClose.Name = "btnExecuteClose";
            this.btnExecuteClose.UseVisualStyleBackColor = false;
            this.btnExecuteClose.Click += new System.EventHandler(this.btnExecuteClose_Click);
            // 
            // txtConfirmClose
            // 
            resources.ApplyResources(this.txtConfirmClose, "txtConfirmClose");
            this.txtConfirmClose.Name = "txtConfirmClose";
            // 
            // lblConfirmHint
            // 
            resources.ApplyResources(this.lblConfirmHint, "lblConfirmHint");
            this.lblConfirmHint.Name = "lblConfirmHint";
            // 
            // panelHeader
            // 
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.Controls.Add(this.lblYearValue);
            this.panelHeader.Controls.Add(this.lblYearCaption);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Name = "panelHeader";
            // 
            // lblYearValue
            // 
            resources.ApplyResources(this.lblYearValue, "lblYearValue");
            this.lblYearValue.Name = "lblYearValue";
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
            // panelMain
            // 
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Controls.Add(this.splitMain);
            this.panelMain.Name = "panelMain";
            // 
            // panelFooter
            // 
            resources.ApplyResources(this.panelFooter, "panelFooter");
            this.panelFooter.Controls.Add(this.btnClose);
            this.panelFooter.Name = "panelFooter";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frm_year_end_close_wizard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.Name = "frm_year_end_close_wizard";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_year_end_close_wizard_FormClosed);
            this.Load += new System.EventHandler(this.frm_year_end_close_wizard_Load);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.grpValidation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridValidation)).EndInit();
            this.panelValidationActions.ResumeLayout(false);
            this.grpProgress.ResumeLayout(false);
            this.grpExecution.ResumeLayout(false);
            this.grpExecution.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblYearCaption;
        private System.Windows.Forms.Label lblYearValue;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.GroupBox grpValidation;
        private System.Windows.Forms.DataGridView gridValidation;
        private System.Windows.Forms.Panel panelValidationActions;
        private System.Windows.Forms.Button btnRefreshValidation;
        private System.Windows.Forms.GroupBox grpExecution;
        private System.Windows.Forms.Label lblConfirmHint;
        private System.Windows.Forms.TextBox txtConfirmClose;
        private System.Windows.Forms.Button btnExecuteClose;
        private System.Windows.Forms.Button btnRollback;
        private System.Windows.Forms.Label lblResultValue;
        private System.Windows.Forms.Label lblResultCaption;
        private System.Windows.Forms.GroupBox grpProgress;
        private System.Windows.Forms.ListBox lstProgress;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCheckPassed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckFailedCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCheckDetails;
    }
}
