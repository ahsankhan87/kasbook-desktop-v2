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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_budget_setup));
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
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // grpHeader
            // 
            resources.ApplyResources(this.grpHeader, "grpHeader");
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
            this.grpHeader.Name = "grpHeader";
            this.grpHeader.TabStop = false;
            // 
            // btnApprove
            // 
            resources.ApplyResources(this.btnApprove, "btnApprove");
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Tag = "finance.edit";
            this.btnApprove.UseVisualStyleBackColor = true;
            // 
            // txtNotes
            // 
            resources.ApplyResources(this.txtNotes, "txtNotes");
            this.txtNotes.Name = "txtNotes";
            // 
            // lblNotes
            // 
            resources.ApplyResources(this.lblNotes, "lblNotes");
            this.lblNotes.Name = "lblNotes";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            resources.ApplyResources(this.cmbStatus, "cmbStatus");
            this.cmbStatus.Name = "cmbStatus";
            // 
            // lblStatusHeader
            // 
            resources.ApplyResources(this.lblStatusHeader, "lblStatusHeader");
            this.lblStatusHeader.Name = "lblStatusHeader";
            // 
            // cmbCostCenter
            // 
            this.cmbCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCostCenter.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCostCenter, "cmbCostCenter");
            this.cmbCostCenter.Name = "cmbCostCenter";
            // 
            // lblCostCenter
            // 
            resources.ApplyResources(this.lblCostCenter, "lblCostCenter");
            this.lblCostCenter.Name = "lblCostCenter";
            // 
            // cmbVersion
            // 
            this.cmbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVersion.FormattingEnabled = true;
            resources.ApplyResources(this.cmbVersion, "cmbVersion");
            this.cmbVersion.Name = "cmbVersion";
            // 
            // lblVersion
            // 
            resources.ApplyResources(this.lblVersion, "lblVersion");
            this.lblVersion.Name = "lblVersion";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            resources.ApplyResources(this.cmbYear, "cmbYear");
            this.cmbYear.Name = "cmbYear";
            // 
            // lblYear
            // 
            resources.ApplyResources(this.lblYear, "lblYear");
            this.lblYear.Name = "lblYear";
            // 
            // pnlActions
            // 
            resources.ApplyResources(this.pnlActions, "pnlActions");
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
            this.pnlActions.Name = "pnlActions";
            // 
            // chkShowComparison
            // 
            resources.ApplyResources(this.chkShowComparison, "chkShowComparison");
            this.chkShowComparison.Name = "chkShowComparison";
            this.chkShowComparison.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnImportExcel
            // 
            resources.ApplyResources(this.btnImportExcel, "btnImportExcel");
            this.btnImportExcel.Name = "btnImportExcel";
            this.btnImportExcel.UseVisualStyleBackColor = true;
            // 
            // btnExportExcel
            // 
            resources.ApplyResources(this.btnExportExcel, "btnExportExcel");
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            // 
            // btnCopyGrowth
            // 
            resources.ApplyResources(this.btnCopyGrowth, "btnCopyGrowth");
            this.btnCopyGrowth.Name = "btnCopyGrowth";
            this.btnCopyGrowth.UseVisualStyleBackColor = true;
            // 
            // btnCopyLastYear
            // 
            resources.ApplyResources(this.btnCopyLastYear, "btnCopyLastYear");
            this.btnCopyLastYear.Name = "btnCopyLastYear";
            this.btnCopyLastYear.UseVisualStyleBackColor = true;
            // 
            // btnSeasonality
            // 
            resources.ApplyResources(this.btnSeasonality, "btnSeasonality");
            this.btnSeasonality.Name = "btnSeasonality";
            this.btnSeasonality.UseVisualStyleBackColor = true;
            // 
            // btnSpreadEven
            // 
            resources.ApplyResources(this.btnSpreadEven, "btnSpreadEven");
            this.btnSpreadEven.Name = "btnSpreadEven";
            this.btnSpreadEven.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            resources.ApplyResources(this.btnLoad, "btnLoad");
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // btnNew
            // 
            resources.ApplyResources(this.btnNew, "btnNew");
            this.btnNew.Name = "btnNew";
            this.btnNew.UseVisualStyleBackColor = true;
            // 
            // pnlGrid
            // 
            resources.ApplyResources(this.pnlGrid, "pnlGrid");
            this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrid.Controls.Add(this.dgvBudgets);
            this.pnlGrid.Name = "pnlGrid";
            // 
            // dgvBudgets
            // 
            this.dgvBudgets.AllowUserToAddRows = false;
            this.dgvBudgets.AllowUserToDeleteRows = false;
            this.dgvBudgets.BackgroundColor = System.Drawing.Color.White;
            this.dgvBudgets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvBudgets, "dgvBudgets");
            this.dgvBudgets.Name = "dgvBudgets";
            this.dgvBudgets.RowHeadersVisible = false;
            // 
            // pnlTotals
            // 
            resources.ApplyResources(this.pnlTotals, "pnlTotals");
            this.pnlTotals.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTotals.Controls.Add(this.lblNetProfitValue);
            this.pnlTotals.Controls.Add(this.lblNetProfit);
            this.pnlTotals.Controls.Add(this.lblExpenseValue);
            this.pnlTotals.Controls.Add(this.lblExpense);
            this.pnlTotals.Controls.Add(this.lblIncomeValue);
            this.pnlTotals.Controls.Add(this.lblIncome);
            this.pnlTotals.Name = "pnlTotals";
            // 
            // lblNetProfitValue
            // 
            resources.ApplyResources(this.lblNetProfitValue, "lblNetProfitValue");
            this.lblNetProfitValue.Name = "lblNetProfitValue";
            // 
            // lblNetProfit
            // 
            resources.ApplyResources(this.lblNetProfit, "lblNetProfit");
            this.lblNetProfit.Name = "lblNetProfit";
            // 
            // lblExpenseValue
            // 
            resources.ApplyResources(this.lblExpenseValue, "lblExpenseValue");
            this.lblExpenseValue.Name = "lblExpenseValue";
            // 
            // lblExpense
            // 
            resources.ApplyResources(this.lblExpense, "lblExpense");
            this.lblExpense.Name = "lblExpense";
            // 
            // lblIncomeValue
            // 
            resources.ApplyResources(this.lblIncomeValue, "lblIncomeValue");
            this.lblIncomeValue.Name = "lblIncomeValue";
            // 
            // lblIncome
            // 
            resources.ApplyResources(this.lblIncome, "lblIncome");
            this.lblIncome.Name = "lblIncome";
            // 
            // pnlComparison
            // 
            resources.ApplyResources(this.pnlComparison, "pnlComparison");
            this.pnlComparison.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlComparison.Controls.Add(this.dgvComparison);
            this.pnlComparison.Controls.Add(this.lblComparisonTitle);
            this.pnlComparison.Name = "pnlComparison";
            // 
            // dgvComparison
            // 
            this.dgvComparison.AllowUserToAddRows = false;
            this.dgvComparison.AllowUserToDeleteRows = false;
            this.dgvComparison.BackgroundColor = System.Drawing.Color.White;
            this.dgvComparison.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvComparison, "dgvComparison");
            this.dgvComparison.Name = "dgvComparison";
            this.dgvComparison.ReadOnly = true;
            this.dgvComparison.RowHeadersVisible = false;
            // 
            // lblComparisonTitle
            // 
            resources.ApplyResources(this.lblComparisonTitle, "lblComparisonTitle");
            this.lblComparisonTitle.Name = "lblComparisonTitle";
            // 
            // lblStatus
            // 
            resources.ApplyResources(this.lblStatus, "lblStatus");
            this.lblStatus.Name = "lblStatus";
            // 
            // frm_budget_setup
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlComparison);
            this.Controls.Add(this.pnlTotals);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.grpHeader);
            this.Controls.Add(this.lblTitle);
            this.Name = "frm_budget_setup";
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
