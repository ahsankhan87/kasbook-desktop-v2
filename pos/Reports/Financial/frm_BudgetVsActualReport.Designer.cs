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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_BudgetVsActualReport));
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
            resources.ApplyResources(this.panelTop, "panelTop");
            this.panelTop.Name = "panelTop";
            // 
            // tblFilters
            // 
            resources.ApplyResources(this.tblFilters, "tblFilters");
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
            this.tblFilters.Name = "tblFilters";
            // 
            // lblFiscalYear
            // 
            resources.ApplyResources(this.lblFiscalYear, "lblFiscalYear");
            this.lblFiscalYear.Name = "lblFiscalYear";
            // 
            // cmbFiscalYear
            // 
            resources.ApplyResources(this.cmbFiscalYear, "cmbFiscalYear");
            this.cmbFiscalYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiscalYear.FormattingEnabled = true;
            this.cmbFiscalYear.Name = "cmbFiscalYear";
            // 
            // lblBudgetVersion
            // 
            resources.ApplyResources(this.lblBudgetVersion, "lblBudgetVersion");
            this.lblBudgetVersion.Name = "lblBudgetVersion";
            // 
            // cmbBudgetVersion
            // 
            resources.ApplyResources(this.cmbBudgetVersion, "cmbBudgetVersion");
            this.cmbBudgetVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBudgetVersion.FormattingEnabled = true;
            this.cmbBudgetVersion.Name = "cmbBudgetVersion";
            // 
            // lblPeriod
            // 
            resources.ApplyResources(this.lblPeriod, "lblPeriod");
            this.lblPeriod.Name = "lblPeriod";
            // 
            // cmbPeriod
            // 
            resources.ApplyResources(this.cmbPeriod, "cmbPeriod");
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.FormattingEnabled = true;
            this.cmbPeriod.Name = "cmbPeriod";
            // 
            // lblCostCenter
            // 
            resources.ApplyResources(this.lblCostCenter, "lblCostCenter");
            this.lblCostCenter.Name = "lblCostCenter";
            // 
            // cmbCostCenter
            // 
            resources.ApplyResources(this.cmbCostCenter, "cmbCostCenter");
            this.cmbCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCostCenter.FormattingEnabled = true;
            this.cmbCostCenter.Name = "cmbCostCenter";
            // 
            // lblAccountType
            // 
            resources.ApplyResources(this.lblAccountType, "lblAccountType");
            this.lblAccountType.Name = "lblAccountType";
            // 
            // cmbAccountType
            // 
            resources.ApplyResources(this.cmbAccountType, "cmbAccountType");
            this.cmbAccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccountType.FormattingEnabled = true;
            this.cmbAccountType.Name = "cmbAccountType";
            // 
            // lblVarianceThreshold
            // 
            resources.ApplyResources(this.lblVarianceThreshold, "lblVarianceThreshold");
            this.lblVarianceThreshold.Name = "lblVarianceThreshold";
            // 
            // nudVarianceThreshold
            // 
            this.nudVarianceThreshold.DecimalPlaces = 2;
            resources.ApplyResources(this.nudVarianceThreshold, "nudVarianceThreshold");
            this.nudVarianceThreshold.Name = "nudVarianceThreshold";
            this.nudVarianceThreshold.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // btnGenerate
            // 
            resources.ApplyResources(this.btnGenerate, "btnGenerate");
            this.btnGenerate.Name = "btnGenerate";
            this.tblFilters.SetRowSpan(this.btnGenerate, 2);
            this.btnGenerate.UseVisualStyleBackColor = true;
            // 
            // panelSummary
            // 
            this.panelSummary.Controls.Add(this.flowSummary);
            resources.ApplyResources(this.panelSummary, "panelSummary");
            this.panelSummary.Name = "panelSummary";
            // 
            // flowSummary
            // 
            this.flowSummary.Controls.Add(this.cardIncome);
            this.flowSummary.Controls.Add(this.cardExpense);
            this.flowSummary.Controls.Add(this.cardNet);
            this.flowSummary.Controls.Add(this.cardAchievement);
            this.flowSummary.Controls.Add(this.lblHealth);
            resources.ApplyResources(this.flowSummary, "flowSummary");
            this.flowSummary.Name = "flowSummary";
            // 
            // cardIncome
            // 
            this.cardIncome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardIncome.Controls.Add(this.lblIncomeValue);
            this.cardIncome.Controls.Add(this.lblIncomeTitle);
            resources.ApplyResources(this.cardIncome, "cardIncome");
            this.cardIncome.Name = "cardIncome";
            // 
            // lblIncomeValue
            // 
            resources.ApplyResources(this.lblIncomeValue, "lblIncomeValue");
            this.lblIncomeValue.Name = "lblIncomeValue";
            // 
            // lblIncomeTitle
            // 
            resources.ApplyResources(this.lblIncomeTitle, "lblIncomeTitle");
            this.lblIncomeTitle.Name = "lblIncomeTitle";
            // 
            // cardExpense
            // 
            this.cardExpense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardExpense.Controls.Add(this.lblExpenseValue);
            this.cardExpense.Controls.Add(this.lblExpenseTitle);
            resources.ApplyResources(this.cardExpense, "cardExpense");
            this.cardExpense.Name = "cardExpense";
            // 
            // lblExpenseValue
            // 
            resources.ApplyResources(this.lblExpenseValue, "lblExpenseValue");
            this.lblExpenseValue.Name = "lblExpenseValue";
            // 
            // lblExpenseTitle
            // 
            resources.ApplyResources(this.lblExpenseTitle, "lblExpenseTitle");
            this.lblExpenseTitle.Name = "lblExpenseTitle";
            // 
            // cardNet
            // 
            this.cardNet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardNet.Controls.Add(this.lblNetValue);
            this.cardNet.Controls.Add(this.lblNetTitle);
            resources.ApplyResources(this.cardNet, "cardNet");
            this.cardNet.Name = "cardNet";
            // 
            // lblNetValue
            // 
            resources.ApplyResources(this.lblNetValue, "lblNetValue");
            this.lblNetValue.Name = "lblNetValue";
            // 
            // lblNetTitle
            // 
            resources.ApplyResources(this.lblNetTitle, "lblNetTitle");
            this.lblNetTitle.Name = "lblNetTitle";
            // 
            // cardAchievement
            // 
            this.cardAchievement.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardAchievement.Controls.Add(this.lblAchievementValue);
            this.cardAchievement.Controls.Add(this.lblAchievementTitle);
            resources.ApplyResources(this.cardAchievement, "cardAchievement");
            this.cardAchievement.Name = "cardAchievement";
            // 
            // lblAchievementValue
            // 
            resources.ApplyResources(this.lblAchievementValue, "lblAchievementValue");
            this.lblAchievementValue.Name = "lblAchievementValue";
            // 
            // lblAchievementTitle
            // 
            resources.ApplyResources(this.lblAchievementTitle, "lblAchievementTitle");
            this.lblAchievementTitle.Name = "lblAchievementTitle";
            // 
            // lblHealth
            // 
            resources.ApplyResources(this.lblHealth, "lblHealth");
            this.lblHealth.Name = "lblHealth";
            // 
            // panelGrid
            // 
            this.panelGrid.Controls.Add(this.dgvReport);
            resources.ApplyResources(this.panelGrid, "panelGrid");
            this.panelGrid.Name = "panelGrid";
            // 
            // dgvReport
            // 
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvReport, "dgvReport");
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.RowTemplate.Height = 24;
            // 
            // panelActions
            // 
            this.panelActions.Controls.Add(this.btnExportExcel);
            this.panelActions.Controls.Add(this.btnPrint);
            resources.ApplyResources(this.panelActions, "panelActions");
            this.panelActions.Name = "panelActions";
            // 
            // btnExportExcel
            // 
            resources.ApplyResources(this.btnExportExcel, "btnExportExcel");
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // frm_BudgetVsActualReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelGrid);
            this.Controls.Add(this.panelActions);
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.panelTop);
            this.Name = "frm_BudgetVsActualReport";
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
