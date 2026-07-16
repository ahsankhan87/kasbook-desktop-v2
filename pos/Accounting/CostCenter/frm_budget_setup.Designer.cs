namespace pos.Accounting.CostCenter
{
    partial class frm_budget_setup
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
            this.grpHeader = new System.Windows.Forms.GroupBox();
            this.btnApprove = new System.Windows.Forms.Button();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatusHeader = new System.Windows.Forms.Label();
            this.cmbCostCenter = new System.Windows.Forms.ComboBox();
            this.lblCostCenter = new System.Windows.Forms.Label();
            this.cmbVersion = new System.Windows.Forms.ComboBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.chkShowComparison = new System.Windows.Forms.CheckBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnImportExcel = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnCopyGrowth = new System.Windows.Forms.Button();
            this.btnCopyLastYear = new System.Windows.Forms.Button();
            this.btnSeasonality = new System.Windows.Forms.Button();
            this.btnSpreadEven = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvBudgets = new System.Windows.Forms.DataGridView();
            this.pnlTotals = new System.Windows.Forms.Panel();
            this.lblNetProfitValue = new System.Windows.Forms.Label();
            this.lblNetProfit = new System.Windows.Forms.Label();
            this.lblExpenseValue = new System.Windows.Forms.Label();
            this.lblExpense = new System.Windows.Forms.Label();
            this.lblIncomeValue = new System.Windows.Forms.Label();
            this.lblIncome = new System.Windows.Forms.Label();
            this.pnlComparison = new System.Windows.Forms.Panel();
            this.dgvComparison = new System.Windows.Forms.DataGridView();
            this.lblComparisonTitle = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.grpHeader.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBudgets)).BeginInit();
            this.pnlTotals.SuspendLayout();
            this.pnlComparison.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComparison)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(290, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Budget Entry && Planning Center";
            // 
            // grpHeader
            // 
            this.grpHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpHeader.Controls.Add(this.btnApprove);
            this.grpHeader.Controls.Add(this.txtNotes);
            this.grpHeader.Controls.Add(this.lblNotes);
            this.grpHeader.Controls.Add(this.cmbStatus);
            this.grpHeader.Controls.Add(this.lblStatusHeader);
            this.grpHeader.Controls.Add(this.cmbCostCenter);
            this.grpHeader.Controls.Add(this.lblCostCenter);
            this.grpHeader.Controls.Add(this.cmbVersion);
            this.grpHeader.Controls.Add(this.lblVersion);
            this.grpHeader.Controls.Add(this.cmbYear);
            this.grpHeader.Controls.Add(this.lblYear);
            this.grpHeader.Location = new System.Drawing.Point(12, 42);
            this.grpHeader.Name = "grpHeader";
            this.grpHeader.Size = new System.Drawing.Size(1288, 108);
            this.grpHeader.TabIndex = 1;
            this.grpHeader.TabStop = false;
            this.grpHeader.Text = "Budget Header";
            // 
            // btnApprove
            // 
            this.btnApprove.Location = new System.Drawing.Point(1145, 24);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(128, 28);
            this.btnApprove.TabIndex = 10;
            this.btnApprove.Tag = "finance.edit";
            this.btnApprove.Text = "Approve Budget";
            this.btnApprove.UseVisualStyleBackColor = true;
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(127, 62);
            this.txtNotes.MaxLength = 500;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(1146, 22);
            this.txtNotes.TabIndex = 9;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(17, 66);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(104, 15);
            this.lblNotes.TabIndex = 8;
            this.lblNotes.Text = "Notes/Description";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(971, 26);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(155, 23);
            this.cmbStatus.TabIndex = 7;
            // 
            // lblStatusHeader
            // 
            this.lblStatusHeader.AutoSize = true;
            this.lblStatusHeader.Location = new System.Drawing.Point(922, 30);
            this.lblStatusHeader.Name = "lblStatusHeader";
            this.lblStatusHeader.Size = new System.Drawing.Size(39, 15);
            this.lblStatusHeader.TabIndex = 6;
            this.lblStatusHeader.Text = "Status";
            // 
            // cmbCostCenter
            // 
            this.cmbCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCostCenter.FormattingEnabled = true;
            this.cmbCostCenter.Location = new System.Drawing.Point(647, 26);
            this.cmbCostCenter.Name = "cmbCostCenter";
            this.cmbCostCenter.Size = new System.Drawing.Size(260, 23);
            this.cmbCostCenter.TabIndex = 5;
            // 
            // lblCostCenter
            // 
            this.lblCostCenter.AutoSize = true;
            this.lblCostCenter.Location = new System.Drawing.Point(571, 30);
            this.lblCostCenter.Name = "lblCostCenter";
            this.lblCostCenter.Size = new System.Drawing.Size(67, 15);
            this.lblCostCenter.TabIndex = 4;
            this.lblCostCenter.Text = "Cost Center";
            // 
            // cmbVersion
            // 
            this.cmbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVersion.FormattingEnabled = true;
            this.cmbVersion.Location = new System.Drawing.Point(357, 26);
            this.cmbVersion.Name = "cmbVersion";
            this.cmbVersion.Size = new System.Drawing.Size(196, 23);
            this.cmbVersion.TabIndex = 3;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(261, 30);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(88, 15);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "Budget Version";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(127, 26);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(126, 23);
            this.cmbYear.TabIndex = 1;
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(17, 30);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(77, 15);
            this.lblYear.TabIndex = 0;
            this.lblYear.Text = "Financial Year";
            // 
            // pnlActions
            // 
            this.pnlActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlActions.Controls.Add(this.chkShowComparison);
            this.pnlActions.Controls.Add(this.btnPrint);
            this.pnlActions.Controls.Add(this.btnImportExcel);
            this.pnlActions.Controls.Add(this.btnExportExcel);
            this.pnlActions.Controls.Add(this.btnCopyGrowth);
            this.pnlActions.Controls.Add(this.btnCopyLastYear);
            this.pnlActions.Controls.Add(this.btnSeasonality);
            this.pnlActions.Controls.Add(this.btnSpreadEven);
            this.pnlActions.Controls.Add(this.btnSave);
            this.pnlActions.Controls.Add(this.btnLoad);
            this.pnlActions.Controls.Add(this.btnNew);
            this.pnlActions.Location = new System.Drawing.Point(12, 156);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(1288, 42);
            this.pnlActions.TabIndex = 2;
            // 
            // chkShowComparison
            // 
            this.chkShowComparison.AutoSize = true;
            this.chkShowComparison.Location = new System.Drawing.Point(1090, 11);
            this.chkShowComparison.Name = "chkShowComparison";
            this.chkShowComparison.Size = new System.Drawing.Size(165, 19);
            this.chkShowComparison.TabIndex = 10;
            this.chkShowComparison.Text = "Show Budget Comparison";
            this.chkShowComparison.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(972, 6);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(102, 30);
            this.btnPrint.TabIndex = 9;
            this.btnPrint.Text = "Print Budget";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnImportExcel
            // 
            this.btnImportExcel.Location = new System.Drawing.Point(860, 6);
            this.btnImportExcel.Name = "btnImportExcel";
            this.btnImportExcel.Size = new System.Drawing.Size(108, 30);
            this.btnImportExcel.TabIndex = 8;
            this.btnImportExcel.Text = "Import from Excel";
            this.btnImportExcel.UseVisualStyleBackColor = true;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(745, 6);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(111, 30);
            this.btnExportExcel.TabIndex = 7;
            this.btnExportExcel.Text = "Export to Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            // 
            // btnCopyGrowth
            // 
            this.btnCopyGrowth.Location = new System.Drawing.Point(588, 6);
            this.btnCopyGrowth.Name = "btnCopyGrowth";
            this.btnCopyGrowth.Size = new System.Drawing.Size(153, 30);
            this.btnCopyGrowth.TabIndex = 6;
            this.btnCopyGrowth.Text = "Copy Last Year + X%";
            this.btnCopyGrowth.UseVisualStyleBackColor = true;
            // 
            // btnCopyLastYear
            // 
            this.btnCopyLastYear.Location = new System.Drawing.Point(471, 6);
            this.btnCopyLastYear.Name = "btnCopyLastYear";
            this.btnCopyLastYear.Size = new System.Drawing.Size(113, 30);
            this.btnCopyLastYear.TabIndex = 5;
            this.btnCopyLastYear.Text = "Copy Last Year";
            this.btnCopyLastYear.UseVisualStyleBackColor = true;
            // 
            // btnSeasonality
            // 
            this.btnSeasonality.Location = new System.Drawing.Point(348, 6);
            this.btnSeasonality.Name = "btnSeasonality";
            this.btnSeasonality.Size = new System.Drawing.Size(119, 30);
            this.btnSeasonality.TabIndex = 4;
            this.btnSeasonality.Text = "Spread Seasonality";
            this.btnSeasonality.UseVisualStyleBackColor = true;
            // 
            // btnSpreadEven
            // 
            this.btnSpreadEven.Location = new System.Drawing.Point(226, 6);
            this.btnSpreadEven.Name = "btnSpreadEven";
            this.btnSpreadEven.Size = new System.Drawing.Size(118, 30);
            this.btnSpreadEven.TabIndex = 3;
            this.btnSpreadEven.Text = "Spread Evenly";
            this.btnSpreadEven.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(153, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(69, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(79, 6);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(70, 30);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(6, 6);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(69, 30);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            // 
            // pnlGrid
            // 
            this.pnlGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrid.Controls.Add(this.dgvBudgets);
            this.pnlGrid.Location = new System.Drawing.Point(12, 204);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(1288, 340);
            this.pnlGrid.TabIndex = 3;
            // 
            // dgvBudgets
            // 
            this.dgvBudgets.AllowUserToAddRows = false;
            this.dgvBudgets.AllowUserToDeleteRows = false;
            this.dgvBudgets.BackgroundColor = System.Drawing.Color.White;
            this.dgvBudgets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBudgets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBudgets.Location = new System.Drawing.Point(0, 0);
            this.dgvBudgets.Name = "dgvBudgets";
            this.dgvBudgets.RowHeadersVisible = false;
            this.dgvBudgets.Size = new System.Drawing.Size(1286, 338);
            this.dgvBudgets.TabIndex = 0;
            // 
            // pnlTotals
            // 
            this.pnlTotals.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTotals.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTotals.Controls.Add(this.lblNetProfitValue);
            this.pnlTotals.Controls.Add(this.lblNetProfit);
            this.pnlTotals.Controls.Add(this.lblExpenseValue);
            this.pnlTotals.Controls.Add(this.lblExpense);
            this.pnlTotals.Controls.Add(this.lblIncomeValue);
            this.pnlTotals.Controls.Add(this.lblIncome);
            this.pnlTotals.Location = new System.Drawing.Point(12, 550);
            this.pnlTotals.Name = "pnlTotals";
            this.pnlTotals.Size = new System.Drawing.Size(1288, 38);
            this.pnlTotals.TabIndex = 4;
            // 
            // lblNetProfitValue
            // 
            this.lblNetProfitValue.AutoSize = true;
            this.lblNetProfitValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNetProfitValue.Location = new System.Drawing.Point(1118, 10);
            this.lblNetProfitValue.Name = "lblNetProfitValue";
            this.lblNetProfitValue.Size = new System.Drawing.Size(32, 15);
            this.lblNetProfitValue.TabIndex = 5;
            this.lblNetProfitValue.Text = "0.00";
            // 
            // lblNetProfit
            // 
            this.lblNetProfit.AutoSize = true;
            this.lblNetProfit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNetProfit.Location = new System.Drawing.Point(1012, 10);
            this.lblNetProfit.Name = "lblNetProfit";
            this.lblNetProfit.Size = new System.Drawing.Size(102, 15);
            this.lblNetProfit.TabIndex = 4;
            this.lblNetProfit.Text = "Net Profit Budget:";
            // 
            // lblExpenseValue
            // 
            this.lblExpenseValue.AutoSize = true;
            this.lblExpenseValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblExpenseValue.Location = new System.Drawing.Point(698, 10);
            this.lblExpenseValue.Name = "lblExpenseValue";
            this.lblExpenseValue.Size = new System.Drawing.Size(32, 15);
            this.lblExpenseValue.TabIndex = 3;
            this.lblExpenseValue.Text = "0.00";
            // 
            // lblExpense
            // 
            this.lblExpense.AutoSize = true;
            this.lblExpense.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblExpense.Location = new System.Drawing.Point(553, 10);
            this.lblExpense.Name = "lblExpense";
            this.lblExpense.Size = new System.Drawing.Size(141, 15);
            this.lblExpense.TabIndex = 2;
            this.lblExpense.Text = "Total Expense Budget:";
            // 
            // lblIncomeValue
            // 
            this.lblIncomeValue.AutoSize = true;
            this.lblIncomeValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblIncomeValue.Location = new System.Drawing.Point(205, 10);
            this.lblIncomeValue.Name = "lblIncomeValue";
            this.lblIncomeValue.Size = new System.Drawing.Size(32, 15);
            this.lblIncomeValue.TabIndex = 1;
            this.lblIncomeValue.Text = "0.00";
            // 
            // lblIncome
            // 
            this.lblIncome.AutoSize = true;
            this.lblIncome.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblIncome.Location = new System.Drawing.Point(20, 10);
            this.lblIncome.Name = "lblIncome";
            this.lblIncome.Size = new System.Drawing.Size(181, 15);
            this.lblIncome.TabIndex = 0;
            this.lblIncome.Text = "Total Income Budget (Annual):";
            // 
            // pnlComparison
            // 
            this.pnlComparison.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlComparison.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlComparison.Controls.Add(this.dgvComparison);
            this.pnlComparison.Controls.Add(this.lblComparisonTitle);
            this.pnlComparison.Location = new System.Drawing.Point(12, 594);
            this.pnlComparison.Name = "pnlComparison";
            this.pnlComparison.Size = new System.Drawing.Size(1288, 161);
            this.pnlComparison.TabIndex = 5;
            // 
            // dgvComparison
            // 
            this.dgvComparison.AllowUserToAddRows = false;
            this.dgvComparison.AllowUserToDeleteRows = false;
            this.dgvComparison.BackgroundColor = System.Drawing.Color.White;
            this.dgvComparison.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComparison.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvComparison.Location = new System.Drawing.Point(0, 28);
            this.dgvComparison.Name = "dgvComparison";
            this.dgvComparison.ReadOnly = true;
            this.dgvComparison.RowHeadersVisible = false;
            this.dgvComparison.Size = new System.Drawing.Size(1286, 131);
            this.dgvComparison.TabIndex = 1;
            // 
            // lblComparisonTitle
            // 
            this.lblComparisonTitle.AutoSize = true;
            this.lblComparisonTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblComparisonTitle.Location = new System.Drawing.Point(10, 7);
            this.lblComparisonTitle.Name = "lblComparisonTitle";
            this.lblComparisonTitle.Size = new System.Drawing.Size(269, 15);
            this.lblComparisonTitle.TabIndex = 0;
            this.lblComparisonTitle.Text = "Budget vs Actual (Budget | Actual | Variance | %)";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(14, 762);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 15);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Ready";
            // 
            // frm_budget_setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 786);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlComparison);
            this.Controls.Add(this.pnlTotals);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.grpHeader);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1180, 760);
            this.Name = "frm_budget_setup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Budget Entry && Planning";
            this.Load += new System.EventHandler(this.FrmBudgetSetup_Load);
            this.grpHeader.ResumeLayout(false);
            this.grpHeader.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBudgets)).EndInit();
            this.pnlTotals.ResumeLayout(false);
            this.pnlTotals.PerformLayout();
            this.pnlComparison.ResumeLayout(false);
            this.pnlComparison.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComparison)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpHeader;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.ComboBox cmbVersion;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.ComboBox cmbCostCenter;
        private System.Windows.Forms.Label lblCostCenter;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblStatusHeader;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.Button btnApprove;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSpreadEven;
        private System.Windows.Forms.Button btnSeasonality;
        private System.Windows.Forms.Button btnCopyLastYear;
        private System.Windows.Forms.Button btnCopyGrowth;
        private System.Windows.Forms.Button btnExportExcel;
        private System.Windows.Forms.Button btnImportExcel;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.CheckBox chkShowComparison;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvBudgets;
        private System.Windows.Forms.Panel pnlTotals;
        private System.Windows.Forms.Label lblIncome;
        private System.Windows.Forms.Label lblIncomeValue;
        private System.Windows.Forms.Label lblExpense;
        private System.Windows.Forms.Label lblExpenseValue;
        private System.Windows.Forms.Label lblNetProfit;
        private System.Windows.Forms.Label lblNetProfitValue;
        private System.Windows.Forms.Panel pnlComparison;
        private System.Windows.Forms.Label lblComparisonTitle;
        private System.Windows.Forms.DataGridView dgvComparison;
        private System.Windows.Forms.Label lblStatus;
    }
}
