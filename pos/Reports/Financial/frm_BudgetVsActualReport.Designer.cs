namespace pos.Reports.Financial
{
    partial class frm_BudgetVsActualReport
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
            this.tblFilters = new System.Windows.Forms.TableLayoutPanel();
            this.lblFiscalYear = new System.Windows.Forms.Label();
            this.cmbFiscalYear = new System.Windows.Forms.ComboBox();
            this.lblBudgetVersion = new System.Windows.Forms.Label();
            this.cmbBudgetVersion = new System.Windows.Forms.ComboBox();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.cmbPeriod = new System.Windows.Forms.ComboBox();
            this.lblCostCenter = new System.Windows.Forms.Label();
            this.cmbCostCenter = new System.Windows.Forms.ComboBox();
            this.lblAccountType = new System.Windows.Forms.Label();
            this.cmbAccountType = new System.Windows.Forms.ComboBox();
            this.lblVarianceThreshold = new System.Windows.Forms.Label();
            this.nudVarianceThreshold = new System.Windows.Forms.NumericUpDown();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.flowSummary = new System.Windows.Forms.FlowLayoutPanel();
            this.cardIncome = new System.Windows.Forms.Panel();
            this.lblIncomeValue = new System.Windows.Forms.Label();
            this.lblIncomeTitle = new System.Windows.Forms.Label();
            this.cardExpense = new System.Windows.Forms.Panel();
            this.lblExpenseValue = new System.Windows.Forms.Label();
            this.lblExpenseTitle = new System.Windows.Forms.Label();
            this.cardNet = new System.Windows.Forms.Panel();
            this.lblNetValue = new System.Windows.Forms.Label();
            this.lblNetTitle = new System.Windows.Forms.Label();
            this.cardAchievement = new System.Windows.Forms.Panel();
            this.lblAchievementValue = new System.Windows.Forms.Label();
            this.lblAchievementTitle = new System.Windows.Forms.Label();
            this.lblHealth = new System.Windows.Forms.Label();
            this.panelGrid = new System.Windows.Forms.Panel();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.panelActions = new System.Windows.Forms.Panel();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.tblFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVarianceThreshold)).BeginInit();
            this.panelSummary.SuspendLayout();
            this.flowSummary.SuspendLayout();
            this.cardIncome.SuspendLayout();
            this.cardExpense.SuspendLayout();
            this.cardNet.SuspendLayout();
            this.cardAchievement.SuspendLayout();
            this.panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.panelActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.tblFilters);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.panelTop.Size = new System.Drawing.Size(1295, 94);
            this.panelTop.TabIndex = 0;
            // 
            // tblFilters
            // 
            this.tblFilters.ColumnCount = 7;
            this.tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 131F));
            this.tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 158F));
            this.tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 131F));
            this.tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 158F));
            this.tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 131F));
            this.tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 158F));
            this.tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblFilters.Controls.Add(this.lblFiscalYear, 0, 0);
            this.tblFilters.Controls.Add(this.cmbFiscalYear, 1, 0);
            this.tblFilters.Controls.Add(this.lblBudgetVersion, 2, 0);
            this.tblFilters.Controls.Add(this.cmbBudgetVersion, 3, 0);
            this.tblFilters.Controls.Add(this.lblPeriod, 4, 0);
            this.tblFilters.Controls.Add(this.cmbPeriod, 5, 0);
            this.tblFilters.Controls.Add(this.lblCostCenter, 0, 1);
            this.tblFilters.Controls.Add(this.cmbCostCenter, 1, 1);
            this.tblFilters.Controls.Add(this.lblAccountType, 2, 1);
            this.tblFilters.Controls.Add(this.cmbAccountType, 3, 1);
            this.tblFilters.Controls.Add(this.lblVarianceThreshold, 4, 1);
            this.tblFilters.Controls.Add(this.nudVarianceThreshold, 5, 1);
            this.tblFilters.Controls.Add(this.btnGenerate, 6, 0);
            this.tblFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblFilters.Location = new System.Drawing.Point(7, 8);
            this.tblFilters.Name = "tblFilters";
            this.tblFilters.RowCount = 2;
            this.tblFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblFilters.Size = new System.Drawing.Size(1281, 78);
            this.tblFilters.TabIndex = 0;
            // 
            // lblFiscalYear
            // 
            this.lblFiscalYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFiscalYear.Location = new System.Drawing.Point(3, 0);
            this.lblFiscalYear.Name = "lblFiscalYear";
            this.lblFiscalYear.Size = new System.Drawing.Size(125, 39);
            this.lblFiscalYear.TabIndex = 0;
            this.lblFiscalYear.Text = "Financial Year";
            this.lblFiscalYear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbFiscalYear
            // 
            this.cmbFiscalYear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFiscalYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiscalYear.FormattingEnabled = true;
            this.cmbFiscalYear.Location = new System.Drawing.Point(134, 6);
            this.cmbFiscalYear.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.cmbFiscalYear.Name = "cmbFiscalYear";
            this.cmbFiscalYear.Size = new System.Drawing.Size(152, 24);
            this.cmbFiscalYear.TabIndex = 1;
            // 
            // lblBudgetVersion
            // 
            this.lblBudgetVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBudgetVersion.Location = new System.Drawing.Point(292, 0);
            this.lblBudgetVersion.Name = "lblBudgetVersion";
            this.lblBudgetVersion.Size = new System.Drawing.Size(125, 39);
            this.lblBudgetVersion.TabIndex = 2;
            this.lblBudgetVersion.Text = "Budget Version";
            this.lblBudgetVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbBudgetVersion
            // 
            this.cmbBudgetVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbBudgetVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBudgetVersion.FormattingEnabled = true;
            this.cmbBudgetVersion.Location = new System.Drawing.Point(423, 6);
            this.cmbBudgetVersion.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.cmbBudgetVersion.Name = "cmbBudgetVersion";
            this.cmbBudgetVersion.Size = new System.Drawing.Size(152, 24);
            this.cmbBudgetVersion.TabIndex = 3;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPeriod.Location = new System.Drawing.Point(581, 0);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(125, 39);
            this.lblPeriod.TabIndex = 4;
            this.lblPeriod.Text = "Period";
            this.lblPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPeriod
            // 
            this.cmbPeriod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.FormattingEnabled = true;
            this.cmbPeriod.Location = new System.Drawing.Point(712, 6);
            this.cmbPeriod.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.Size = new System.Drawing.Size(152, 24);
            this.cmbPeriod.TabIndex = 5;
            // 
            // lblCostCenter
            // 
            this.lblCostCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCostCenter.Location = new System.Drawing.Point(3, 39);
            this.lblCostCenter.Name = "lblCostCenter";
            this.lblCostCenter.Size = new System.Drawing.Size(125, 39);
            this.lblCostCenter.TabIndex = 6;
            this.lblCostCenter.Text = "Cost Center";
            this.lblCostCenter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbCostCenter
            // 
            this.cmbCostCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCostCenter.FormattingEnabled = true;
            this.cmbCostCenter.Location = new System.Drawing.Point(134, 45);
            this.cmbCostCenter.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.cmbCostCenter.Name = "cmbCostCenter";
            this.cmbCostCenter.Size = new System.Drawing.Size(152, 24);
            this.cmbCostCenter.TabIndex = 7;
            // 
            // lblAccountType
            // 
            this.lblAccountType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAccountType.Location = new System.Drawing.Point(292, 39);
            this.lblAccountType.Name = "lblAccountType";
            this.lblAccountType.Size = new System.Drawing.Size(125, 39);
            this.lblAccountType.TabIndex = 8;
            this.lblAccountType.Text = "Account Type";
            this.lblAccountType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbAccountType
            // 
            this.cmbAccountType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccountType.FormattingEnabled = true;
            this.cmbAccountType.Location = new System.Drawing.Point(423, 45);
            this.cmbAccountType.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.cmbAccountType.Name = "cmbAccountType";
            this.cmbAccountType.Size = new System.Drawing.Size(152, 24);
            this.cmbAccountType.TabIndex = 9;
            // 
            // lblVarianceThreshold
            // 
            this.lblVarianceThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVarianceThreshold.Location = new System.Drawing.Point(581, 39);
            this.lblVarianceThreshold.Name = "lblVarianceThreshold";
            this.lblVarianceThreshold.Size = new System.Drawing.Size(125, 39);
            this.lblVarianceThreshold.TabIndex = 10;
            this.lblVarianceThreshold.Text = "Threshold %";
            this.lblVarianceThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudVarianceThreshold
            // 
            this.nudVarianceThreshold.DecimalPlaces = 2;
            this.nudVarianceThreshold.Dock = System.Windows.Forms.DockStyle.Left;
            this.nudVarianceThreshold.Location = new System.Drawing.Point(712, 45);
            this.nudVarianceThreshold.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.nudVarianceThreshold.Name = "nudVarianceThreshold";
            this.nudVarianceThreshold.Size = new System.Drawing.Size(105, 24);
            this.nudVarianceThreshold.TabIndex = 11;
            this.nudVarianceThreshold.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(1184, 6);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.btnGenerate.Name = "btnGenerate";
            this.tblFilters.SetRowSpan(this.btnGenerate, 2);
            this.btnGenerate.Size = new System.Drawing.Size(94, 60);
            this.btnGenerate.TabIndex = 12;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.UseVisualStyleBackColor = true;
            // 
            // panelSummary
            // 
            this.panelSummary.Controls.Add(this.flowSummary);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSummary.Location = new System.Drawing.Point(0, 94);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(7, 0, 7, 8);
            this.panelSummary.Size = new System.Drawing.Size(1295, 90);
            this.panelSummary.TabIndex = 1;
            // 
            // flowSummary
            // 
            this.flowSummary.Controls.Add(this.cardIncome);
            this.flowSummary.Controls.Add(this.cardExpense);
            this.flowSummary.Controls.Add(this.cardNet);
            this.flowSummary.Controls.Add(this.cardAchievement);
            this.flowSummary.Controls.Add(this.lblHealth);
            this.flowSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowSummary.Location = new System.Drawing.Point(7, 0);
            this.flowSummary.Name = "flowSummary";
            this.flowSummary.Size = new System.Drawing.Size(1281, 82);
            this.flowSummary.TabIndex = 0;
            // 
            // cardIncome
            // 
            this.cardIncome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardIncome.Controls.Add(this.lblIncomeValue);
            this.cardIncome.Controls.Add(this.lblIncomeTitle);
            this.cardIncome.Location = new System.Drawing.Point(3, 3);
            this.cardIncome.Name = "cardIncome";
            this.cardIncome.Size = new System.Drawing.Size(228, 72);
            this.cardIncome.TabIndex = 0;
            // 
            // lblIncomeValue
            // 
            this.lblIncomeValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIncomeValue.Location = new System.Drawing.Point(0, 26);
            this.lblIncomeValue.Name = "lblIncomeValue";
            this.lblIncomeValue.Size = new System.Drawing.Size(226, 44);
            this.lblIncomeValue.TabIndex = 1;
            this.lblIncomeValue.Text = "B: 0.00 | A: 0.00";
            this.lblIncomeValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblIncomeTitle
            // 
            this.lblIncomeTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblIncomeTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblIncomeTitle.Location = new System.Drawing.Point(0, 0);
            this.lblIncomeTitle.Name = "lblIncomeTitle";
            this.lblIncomeTitle.Size = new System.Drawing.Size(226, 26);
            this.lblIncomeTitle.TabIndex = 0;
            this.lblIncomeTitle.Text = "Total Income";
            this.lblIncomeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardExpense
            // 
            this.cardExpense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardExpense.Controls.Add(this.lblExpenseValue);
            this.cardExpense.Controls.Add(this.lblExpenseTitle);
            this.cardExpense.Location = new System.Drawing.Point(237, 3);
            this.cardExpense.Name = "cardExpense";
            this.cardExpense.Size = new System.Drawing.Size(228, 72);
            this.cardExpense.TabIndex = 1;
            // 
            // lblExpenseValue
            // 
            this.lblExpenseValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExpenseValue.Location = new System.Drawing.Point(0, 26);
            this.lblExpenseValue.Name = "lblExpenseValue";
            this.lblExpenseValue.Size = new System.Drawing.Size(226, 44);
            this.lblExpenseValue.TabIndex = 1;
            this.lblExpenseValue.Text = "B: 0.00 | A: 0.00";
            this.lblExpenseValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExpenseTitle
            // 
            this.lblExpenseTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblExpenseTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblExpenseTitle.Location = new System.Drawing.Point(0, 0);
            this.lblExpenseTitle.Name = "lblExpenseTitle";
            this.lblExpenseTitle.Size = new System.Drawing.Size(226, 26);
            this.lblExpenseTitle.TabIndex = 0;
            this.lblExpenseTitle.Text = "Total Expenses";
            this.lblExpenseTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardNet
            // 
            this.cardNet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardNet.Controls.Add(this.lblNetValue);
            this.cardNet.Controls.Add(this.lblNetTitle);
            this.cardNet.Location = new System.Drawing.Point(471, 3);
            this.cardNet.Name = "cardNet";
            this.cardNet.Size = new System.Drawing.Size(228, 72);
            this.cardNet.TabIndex = 2;
            // 
            // lblNetValue
            // 
            this.lblNetValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetValue.Location = new System.Drawing.Point(0, 26);
            this.lblNetValue.Name = "lblNetValue";
            this.lblNetValue.Size = new System.Drawing.Size(226, 44);
            this.lblNetValue.TabIndex = 1;
            this.lblNetValue.Text = "B: 0.00 | A: 0.00";
            this.lblNetValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNetTitle
            // 
            this.lblNetTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNetTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblNetTitle.Location = new System.Drawing.Point(0, 0);
            this.lblNetTitle.Name = "lblNetTitle";
            this.lblNetTitle.Size = new System.Drawing.Size(226, 26);
            this.lblNetTitle.TabIndex = 0;
            this.lblNetTitle.Text = "Net Profit";
            this.lblNetTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardAchievement
            // 
            this.cardAchievement.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardAchievement.Controls.Add(this.lblAchievementValue);
            this.cardAchievement.Controls.Add(this.lblAchievementTitle);
            this.cardAchievement.Location = new System.Drawing.Point(705, 3);
            this.cardAchievement.Name = "cardAchievement";
            this.cardAchievement.Size = new System.Drawing.Size(193, 72);
            this.cardAchievement.TabIndex = 3;
            // 
            // lblAchievementValue
            // 
            this.lblAchievementValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAchievementValue.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblAchievementValue.Location = new System.Drawing.Point(0, 26);
            this.lblAchievementValue.Name = "lblAchievementValue";
            this.lblAchievementValue.Size = new System.Drawing.Size(191, 44);
            this.lblAchievementValue.TabIndex = 1;
            this.lblAchievementValue.Text = "0.00%";
            this.lblAchievementValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAchievementTitle
            // 
            this.lblAchievementTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAchievementTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblAchievementTitle.Location = new System.Drawing.Point(0, 0);
            this.lblAchievementTitle.Name = "lblAchievementTitle";
            this.lblAchievementTitle.Size = new System.Drawing.Size(191, 26);
            this.lblAchievementTitle.TabIndex = 0;
            this.lblAchievementTitle.Text = "Budget Achievement %";
            this.lblAchievementTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHealth
            // 
            this.lblHealth.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblHealth.Location = new System.Drawing.Point(904, 0);
            this.lblHealth.Name = "lblHealth";
            this.lblHealth.Size = new System.Drawing.Size(158, 78);
            this.lblHealth.TabIndex = 4;
            this.lblHealth.Text = "● Amber";
            this.lblHealth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelGrid
            // 
            this.panelGrid.Controls.Add(this.dgvReport);
            this.panelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrid.Location = new System.Drawing.Point(0, 184);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Padding = new System.Windows.Forms.Padding(7, 0, 7, 8);
            this.panelGrid.Size = new System.Drawing.Size(1295, 539);
            this.panelGrid.TabIndex = 2;
            // 
            // dgvReport
            // 
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReport.Location = new System.Drawing.Point(7, 0);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.RowHeadersWidth = 51;
            this.dgvReport.RowTemplate.Height = 24;
            this.dgvReport.Size = new System.Drawing.Size(1281, 531);
            this.dgvReport.TabIndex = 0;
            // 
            // panelActions
            // 
            this.panelActions.Controls.Add(this.btnExportExcel);
            this.panelActions.Controls.Add(this.btnPrint);
            this.panelActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelActions.Location = new System.Drawing.Point(0, 723);
            this.panelActions.Name = "panelActions";
            this.panelActions.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.panelActions.Size = new System.Drawing.Size(1295, 48);
            this.panelActions.TabIndex = 3;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportExcel.Location = new System.Drawing.Point(1190, 8);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(94, 32);
            this.btnExportExcel.TabIndex = 1;
            this.btnExportExcel.Text = "Export Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(1090, 8);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(94, 32);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "Print Report";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // frm_BudgetVsActualReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1295, 771);
            this.Controls.Add(this.panelGrid);
            this.Controls.Add(this.panelActions);
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.panelTop);
            this.Name = "frm_BudgetVsActualReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Budget vs Actual Report";
            this.panelTop.ResumeLayout(false);
            this.tblFilters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudVarianceThreshold)).EndInit();
            this.panelSummary.ResumeLayout(false);
            this.flowSummary.ResumeLayout(false);
            this.cardIncome.ResumeLayout(false);
            this.cardExpense.ResumeLayout(false);
            this.cardNet.ResumeLayout(false);
            this.cardAchievement.ResumeLayout(false);
            this.panelGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.panelActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TableLayoutPanel tblFilters;
        private System.Windows.Forms.Label lblFiscalYear;
        private System.Windows.Forms.ComboBox cmbFiscalYear;
        private System.Windows.Forms.Label lblBudgetVersion;
        private System.Windows.Forms.ComboBox cmbBudgetVersion;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.ComboBox cmbPeriod;
        private System.Windows.Forms.Label lblCostCenter;
        private System.Windows.Forms.ComboBox cmbCostCenter;
        private System.Windows.Forms.Label lblAccountType;
        private System.Windows.Forms.ComboBox cmbAccountType;
        private System.Windows.Forms.Label lblVarianceThreshold;
        private System.Windows.Forms.NumericUpDown nudVarianceThreshold;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.FlowLayoutPanel flowSummary;
        private System.Windows.Forms.Panel cardIncome;
        private System.Windows.Forms.Label lblIncomeTitle;
        private System.Windows.Forms.Label lblIncomeValue;
        private System.Windows.Forms.Panel cardExpense;
        private System.Windows.Forms.Label lblExpenseTitle;
        private System.Windows.Forms.Label lblExpenseValue;
        private System.Windows.Forms.Panel cardNet;
        private System.Windows.Forms.Label lblNetTitle;
        private System.Windows.Forms.Label lblNetValue;
        private System.Windows.Forms.Panel cardAchievement;
        private System.Windows.Forms.Label lblAchievementTitle;
        private System.Windows.Forms.Label lblAchievementValue;
        private System.Windows.Forms.Label lblHealth;
        private System.Windows.Forms.Panel panelGrid;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Panel panelActions;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExportExcel;
    }
}
