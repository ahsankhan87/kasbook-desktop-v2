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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblYearValue = new System.Windows.Forms.Label();
            this.lblYearCaption = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
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
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.grpValidation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridValidation)).BeginInit();
            this.panelValidationActions.SuspendLayout();
            this.grpProgress.SuspendLayout();
            this.grpExecution.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.lblYearValue);
            this.panelHeader.Controls.Add(this.lblYearCaption);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1244, 72);
            this.panelHeader.TabIndex = 0;
            // 
            // lblYearValue
            // 
            this.lblYearValue.AutoSize = true;
            this.lblYearValue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblYearValue.Location = new System.Drawing.Point(101, 43);
            this.lblYearValue.Name = "lblYearValue";
            this.lblYearValue.Size = new System.Drawing.Size(34, 15);
            this.lblYearValue.TabIndex = 2;
            this.lblYearValue.Text = "N/A";
            // 
            // lblYearCaption
            // 
            this.lblYearCaption.AutoSize = true;
            this.lblYearCaption.Location = new System.Drawing.Point(16, 43);
            this.lblYearCaption.Name = "lblYearCaption";
            this.lblYearCaption.Size = new System.Drawing.Size(78, 15);
            this.lblYearCaption.TabIndex = 1;
            this.lblYearCaption.Text = "Closing Year:";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblTitle.Location = new System.Drawing.Point(14, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(244, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Year-End Closing Management";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.splitMain);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 72);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(10);
            this.panelMain.Size = new System.Drawing.Size(1244, 560);
            this.panelMain.TabIndex = 1;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(10, 10);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.grpValidation);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.grpProgress);
            this.splitMain.Panel2.Controls.Add(this.grpExecution);
            this.splitMain.Size = new System.Drawing.Size(1224, 540);
            this.splitMain.SplitterDistance = 268;
            this.splitMain.TabIndex = 0;
            // 
            // grpValidation
            // 
            this.grpValidation.Controls.Add(this.gridValidation);
            this.grpValidation.Controls.Add(this.panelValidationActions);
            this.grpValidation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpValidation.Location = new System.Drawing.Point(0, 0);
            this.grpValidation.Name = "grpValidation";
            this.grpValidation.Padding = new System.Windows.Forms.Padding(10);
            this.grpValidation.Size = new System.Drawing.Size(1224, 268);
            this.grpValidation.TabIndex = 0;
            this.grpValidation.TabStop = false;
            this.grpValidation.Text = "Pre-Close Validation Report";
            // 
            // gridValidation
            // 
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
            this.gridValidation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridValidation.Location = new System.Drawing.Point(10, 25);
            this.gridValidation.Name = "gridValidation";
            this.gridValidation.ReadOnly = true;
            this.gridValidation.RowHeadersVisible = false;
            this.gridValidation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridValidation.Size = new System.Drawing.Size(1204, 198);
            this.gridValidation.TabIndex = 0;
            // 
            // colCheckKey
            // 
            this.colCheckKey.DataPropertyName = "check_key";
            this.colCheckKey.HeaderText = "Key";
            this.colCheckKey.Name = "colCheckKey";
            this.colCheckKey.ReadOnly = true;
            this.colCheckKey.Visible = false;
            // 
            // colCheckName
            // 
            this.colCheckName.DataPropertyName = "check_name";
            this.colCheckName.HeaderText = "Validation Check";
            this.colCheckName.Name = "colCheckName";
            this.colCheckName.ReadOnly = true;
            // 
            // colCheckPassed
            // 
            this.colCheckPassed.DataPropertyName = "is_passed";
            this.colCheckPassed.HeaderText = "Passed";
            this.colCheckPassed.Name = "colCheckPassed";
            this.colCheckPassed.ReadOnly = true;
            this.colCheckPassed.Visible = false;
            // 
            // colCheckStatus
            // 
            this.colCheckStatus.HeaderText = "Status";
            this.colCheckStatus.Name = "colCheckStatus";
            this.colCheckStatus.ReadOnly = true;
            // 
            // colCheckFailedCount
            // 
            this.colCheckFailedCount.DataPropertyName = "failed_count";
            this.colCheckFailedCount.HeaderText = "Failed Count";
            this.colCheckFailedCount.Name = "colCheckFailedCount";
            this.colCheckFailedCount.ReadOnly = true;
            // 
            // colCheckDetails
            // 
            this.colCheckDetails.DataPropertyName = "details";
            this.colCheckDetails.HeaderText = "Details";
            this.colCheckDetails.Name = "colCheckDetails";
            this.colCheckDetails.ReadOnly = true;
            // 
            // panelValidationActions
            // 
            this.panelValidationActions.Controls.Add(this.btnRefreshValidation);
            this.panelValidationActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelValidationActions.Location = new System.Drawing.Point(10, 223);
            this.panelValidationActions.Name = "panelValidationActions";
            this.panelValidationActions.Size = new System.Drawing.Size(1204, 35);
            this.panelValidationActions.TabIndex = 1;
            // 
            // btnRefreshValidation
            // 
            this.btnRefreshValidation.Location = new System.Drawing.Point(0, 5);
            this.btnRefreshValidation.Name = "btnRefreshValidation";
            this.btnRefreshValidation.Size = new System.Drawing.Size(172, 26);
            this.btnRefreshValidation.TabIndex = 0;
            this.btnRefreshValidation.Text = "Refresh Validation Report";
            this.btnRefreshValidation.UseVisualStyleBackColor = true;
            this.btnRefreshValidation.Click += new System.EventHandler(this.btnRefreshValidation_Click);
            // 
            // grpProgress
            // 
            this.grpProgress.Controls.Add(this.lstProgress);
            this.grpProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpProgress.Location = new System.Drawing.Point(0, 131);
            this.grpProgress.Name = "grpProgress";
            this.grpProgress.Padding = new System.Windows.Forms.Padding(10);
            this.grpProgress.Size = new System.Drawing.Size(1224, 137);
            this.grpProgress.TabIndex = 1;
            this.grpProgress.TabStop = false;
            this.grpProgress.Text = "Execution Progress";
            // 
            // lstProgress
            // 
            this.lstProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstProgress.FormattingEnabled = true;
            this.lstProgress.ItemHeight = 15;
            this.lstProgress.Location = new System.Drawing.Point(10, 25);
            this.lstProgress.Name = "lstProgress";
            this.lstProgress.Size = new System.Drawing.Size(1204, 102);
            this.lstProgress.TabIndex = 0;
            // 
            // grpExecution
            // 
            this.grpExecution.Controls.Add(this.lblResultValue);
            this.grpExecution.Controls.Add(this.lblResultCaption);
            this.grpExecution.Controls.Add(this.btnRollback);
            this.grpExecution.Controls.Add(this.btnExecuteClose);
            this.grpExecution.Controls.Add(this.txtConfirmClose);
            this.grpExecution.Controls.Add(this.lblConfirmHint);
            this.grpExecution.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpExecution.Location = new System.Drawing.Point(0, 0);
            this.grpExecution.Name = "grpExecution";
            this.grpExecution.Padding = new System.Windows.Forms.Padding(10);
            this.grpExecution.Size = new System.Drawing.Size(1224, 131);
            this.grpExecution.TabIndex = 0;
            this.grpExecution.TabStop = false;
            this.grpExecution.Text = "Year-End Actions";
            // 
            // lblResultValue
            // 
            this.lblResultValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResultValue.Location = new System.Drawing.Point(540, 25);
            this.lblResultValue.Name = "lblResultValue";
            this.lblResultValue.Size = new System.Drawing.Size(670, 96);
            this.lblResultValue.TabIndex = 5;
            this.lblResultValue.Text = "Not executed yet.";
            // 
            // lblResultCaption
            // 
            this.lblResultCaption.AutoSize = true;
            this.lblResultCaption.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblResultCaption.Location = new System.Drawing.Point(474, 25);
            this.lblResultCaption.Name = "lblResultCaption";
            this.lblResultCaption.Size = new System.Drawing.Size(44, 15);
            this.lblResultCaption.TabIndex = 4;
            this.lblResultCaption.Text = "Result:";
            // 
            // btnRollback
            // 
            this.btnRollback.Location = new System.Drawing.Point(178, 84);
            this.btnRollback.Name = "btnRollback";
            this.btnRollback.Size = new System.Drawing.Size(153, 30);
            this.btnRollback.TabIndex = 3;
            this.btnRollback.Text = "Rollback Year-End Close";
            this.btnRollback.UseVisualStyleBackColor = true;
            this.btnRollback.Click += new System.EventHandler(this.btnRollback_Click);
            // 
            // btnExecuteClose
            // 
            this.btnExecuteClose.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnExecuteClose.ForeColor = System.Drawing.Color.White;
            this.btnExecuteClose.Location = new System.Drawing.Point(13, 84);
            this.btnExecuteClose.Name = "btnExecuteClose";
            this.btnExecuteClose.Size = new System.Drawing.Size(153, 30);
            this.btnExecuteClose.TabIndex = 2;
            this.btnExecuteClose.Text = "Execute Year-End Close";
            this.btnExecuteClose.UseVisualStyleBackColor = false;
            this.btnExecuteClose.Click += new System.EventHandler(this.btnExecuteClose_Click);
            // 
            // txtConfirmClose
            // 
            this.txtConfirmClose.Location = new System.Drawing.Point(13, 47);
            this.txtConfirmClose.Name = "txtConfirmClose";
            this.txtConfirmClose.Size = new System.Drawing.Size(318, 23);
            this.txtConfirmClose.TabIndex = 1;
            // 
            // lblConfirmHint
            // 
            this.lblConfirmHint.AutoSize = true;
            this.lblConfirmHint.Location = new System.Drawing.Point(10, 25);
            this.lblConfirmHint.Name = "lblConfirmHint";
            this.lblConfirmHint.Size = new System.Drawing.Size(304, 15);
            this.lblConfirmHint.TabIndex = 0;
            this.lblConfirmHint.Text = "Type CLOSE YEAR to confirm this irreversible operation.";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.btnClose);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 632);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1244, 44);
            this.panelFooter.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1151, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(81, 28);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frm_year_end_close_wizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1244, 676);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "frm_year_end_close_wizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Year-End Closing";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_year_end_close_wizard_FormClosed);
            this.Load += new System.EventHandler(this.frm_year_end_close_wizard_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelMain.ResumeLayout(false);
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
