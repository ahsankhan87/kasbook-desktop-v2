namespace pos
{
    partial class frm_period_close_wizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_period_close_wizard));
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabSteps = new System.Windows.Forms.TabControl();
            this.tabChecklist = new System.Windows.Forms.TabPage();
            this.gridChecklist = new System.Windows.Forms.DataGridView();
            this.colChecklistKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChecklistItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChecklistPassed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colChecklistStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChecklistPending = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFixModule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFixIssues = new System.Windows.Forms.DataGridViewLinkColumn();
            this.tabSummary = new System.Windows.Forms.TabPage();
            this.tableSummary = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotalTransactions = new System.Windows.Forms.Label();
            this.lblTotalTransactionsValue = new System.Windows.Forms.Label();
            this.lblTotalJournals = new System.Windows.Forms.Label();
            this.lblTotalJournalsValue = new System.Windows.Forms.Label();
            this.lblDebits = new System.Windows.Forms.Label();
            this.lblDebitsValue = new System.Windows.Forms.Label();
            this.lblCredits = new System.Windows.Forms.Label();
            this.lblCreditsValue = new System.Windows.Forms.Label();
            this.lblNetProfit = new System.Windows.Forms.Label();
            this.lblNetProfitValue = new System.Windows.Forms.Label();
            this.lblOutOfBalance = new System.Windows.Forms.Label();
            this.lblOutOfBalanceValue = new System.Windows.Forms.Label();
            this.tabAdjustments = new System.Windows.Forms.TabPage();
            this.chkReverseAccruals = new System.Windows.Forms.CheckBox();
            this.chkAutoDepreciation = new System.Windows.Forms.CheckBox();
            this.tabConfirm = new System.Windows.Forms.TabPage();
            this.btnCompleteClosing = new System.Windows.Forms.Button();
            this.txtPinPassword = new System.Windows.Forms.TextBox();
            this.lblPin = new System.Windows.Forms.Label();
            this.chkConfirmReviewed = new System.Windows.Forms.CheckBox();
            this.lblConfirmText = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.tabSteps.SuspendLayout();
            this.tabChecklist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridChecklist)).BeginInit();
            this.tabSummary.SuspendLayout();
            this.tableSummary.SuspendLayout();
            this.tabAdjustments.SuspendLayout();
            this.tabConfirm.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // tabSteps
            // 
            this.tabSteps.Controls.Add(this.tabChecklist);
            this.tabSteps.Controls.Add(this.tabSummary);
            this.tabSteps.Controls.Add(this.tabAdjustments);
            this.tabSteps.Controls.Add(this.tabConfirm);
            resources.ApplyResources(this.tabSteps, "tabSteps");
            this.tabSteps.Name = "tabSteps";
            this.tabSteps.SelectedIndex = 0;
            this.tabSteps.SelectedIndexChanged += new System.EventHandler(this.tabSteps_SelectedIndexChanged);
            // 
            // tabChecklist
            // 
            this.tabChecklist.Controls.Add(this.gridChecklist);
            resources.ApplyResources(this.tabChecklist, "tabChecklist");
            this.tabChecklist.Name = "tabChecklist";
            this.tabChecklist.UseVisualStyleBackColor = true;
            // 
            // gridChecklist
            // 
            this.gridChecklist.AllowUserToAddRows = false;
            this.gridChecklist.AllowUserToDeleteRows = false;
            this.gridChecklist.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridChecklist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridChecklist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colChecklistKey,
            this.colChecklistItem,
            this.colChecklistPassed,
            this.colChecklistStatus,
            this.colChecklistPending,
            this.colFixModule,
            this.colFixIssues});
            resources.ApplyResources(this.gridChecklist, "gridChecklist");
            this.gridChecklist.Name = "gridChecklist";
            this.gridChecklist.ReadOnly = true;
            this.gridChecklist.RowHeadersVisible = false;
            this.gridChecklist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridChecklist.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridChecklist_CellContentClick);
            this.gridChecklist.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridChecklist_CellFormatting);
            // 
            // colChecklistKey
            // 
            this.colChecklistKey.DataPropertyName = "item_key";
            resources.ApplyResources(this.colChecklistKey, "colChecklistKey");
            this.colChecklistKey.Name = "colChecklistKey";
            this.colChecklistKey.ReadOnly = true;
            // 
            // colChecklistItem
            // 
            this.colChecklistItem.DataPropertyName = "item_name";
            resources.ApplyResources(this.colChecklistItem, "colChecklistItem");
            this.colChecklistItem.Name = "colChecklistItem";
            this.colChecklistItem.ReadOnly = true;
            // 
            // colChecklistPassed
            // 
            this.colChecklistPassed.DataPropertyName = "is_passed";
            resources.ApplyResources(this.colChecklistPassed, "colChecklistPassed");
            this.colChecklistPassed.Name = "colChecklistPassed";
            this.colChecklistPassed.ReadOnly = true;
            // 
            // colChecklistStatus
            // 
            resources.ApplyResources(this.colChecklistStatus, "colChecklistStatus");
            this.colChecklistStatus.Name = "colChecklistStatus";
            this.colChecklistStatus.ReadOnly = true;
            // 
            // colChecklistPending
            // 
            this.colChecklistPending.DataPropertyName = "pending_count";
            resources.ApplyResources(this.colChecklistPending, "colChecklistPending");
            this.colChecklistPending.Name = "colChecklistPending";
            this.colChecklistPending.ReadOnly = true;
            // 
            // colFixModule
            // 
            this.colFixModule.DataPropertyName = "fix_module";
            resources.ApplyResources(this.colFixModule, "colFixModule");
            this.colFixModule.Name = "colFixModule";
            this.colFixModule.ReadOnly = true;
            // 
            // colFixIssues
            // 
            resources.ApplyResources(this.colFixIssues, "colFixIssues");
            this.colFixIssues.Name = "colFixIssues";
            this.colFixIssues.ReadOnly = true;
            this.colFixIssues.Text = "Fix Issues";
            this.colFixIssues.UseColumnTextForLinkValue = true;
            // 
            // tabSummary
            // 
            this.tabSummary.Controls.Add(this.tableSummary);
            resources.ApplyResources(this.tabSummary, "tabSummary");
            this.tabSummary.Name = "tabSummary";
            this.tabSummary.UseVisualStyleBackColor = true;
            // 
            // tableSummary
            // 
            resources.ApplyResources(this.tableSummary, "tableSummary");
            this.tableSummary.Controls.Add(this.lblTotalTransactions, 0, 0);
            this.tableSummary.Controls.Add(this.lblTotalTransactionsValue, 1, 0);
            this.tableSummary.Controls.Add(this.lblTotalJournals, 0, 1);
            this.tableSummary.Controls.Add(this.lblTotalJournalsValue, 1, 1);
            this.tableSummary.Controls.Add(this.lblDebits, 0, 2);
            this.tableSummary.Controls.Add(this.lblDebitsValue, 1, 2);
            this.tableSummary.Controls.Add(this.lblCredits, 0, 3);
            this.tableSummary.Controls.Add(this.lblCreditsValue, 1, 3);
            this.tableSummary.Controls.Add(this.lblNetProfit, 0, 4);
            this.tableSummary.Controls.Add(this.lblNetProfitValue, 1, 4);
            this.tableSummary.Controls.Add(this.lblOutOfBalance, 0, 5);
            this.tableSummary.Controls.Add(this.lblOutOfBalanceValue, 1, 5);
            this.tableSummary.Name = "tableSummary";
            // 
            // lblTotalTransactions
            // 
            resources.ApplyResources(this.lblTotalTransactions, "lblTotalTransactions");
            this.lblTotalTransactions.Name = "lblTotalTransactions";
            // 
            // lblTotalTransactionsValue
            // 
            resources.ApplyResources(this.lblTotalTransactionsValue, "lblTotalTransactionsValue");
            this.lblTotalTransactionsValue.Name = "lblTotalTransactionsValue";
            // 
            // lblTotalJournals
            // 
            resources.ApplyResources(this.lblTotalJournals, "lblTotalJournals");
            this.lblTotalJournals.Name = "lblTotalJournals";
            // 
            // lblTotalJournalsValue
            // 
            resources.ApplyResources(this.lblTotalJournalsValue, "lblTotalJournalsValue");
            this.lblTotalJournalsValue.Name = "lblTotalJournalsValue";
            // 
            // lblDebits
            // 
            resources.ApplyResources(this.lblDebits, "lblDebits");
            this.lblDebits.Name = "lblDebits";
            // 
            // lblDebitsValue
            // 
            resources.ApplyResources(this.lblDebitsValue, "lblDebitsValue");
            this.lblDebitsValue.Name = "lblDebitsValue";
            // 
            // lblCredits
            // 
            resources.ApplyResources(this.lblCredits, "lblCredits");
            this.lblCredits.Name = "lblCredits";
            // 
            // lblCreditsValue
            // 
            resources.ApplyResources(this.lblCreditsValue, "lblCreditsValue");
            this.lblCreditsValue.Name = "lblCreditsValue";
            // 
            // lblNetProfit
            // 
            resources.ApplyResources(this.lblNetProfit, "lblNetProfit");
            this.lblNetProfit.Name = "lblNetProfit";
            // 
            // lblNetProfitValue
            // 
            resources.ApplyResources(this.lblNetProfitValue, "lblNetProfitValue");
            this.lblNetProfitValue.Name = "lblNetProfitValue";
            // 
            // lblOutOfBalance
            // 
            resources.ApplyResources(this.lblOutOfBalance, "lblOutOfBalance");
            this.lblOutOfBalance.Name = "lblOutOfBalance";
            // 
            // lblOutOfBalanceValue
            // 
            resources.ApplyResources(this.lblOutOfBalanceValue, "lblOutOfBalanceValue");
            this.lblOutOfBalanceValue.Name = "lblOutOfBalanceValue";
            // 
            // tabAdjustments
            // 
            this.tabAdjustments.Controls.Add(this.chkReverseAccruals);
            this.tabAdjustments.Controls.Add(this.chkAutoDepreciation);
            resources.ApplyResources(this.tabAdjustments, "tabAdjustments");
            this.tabAdjustments.Name = "tabAdjustments";
            this.tabAdjustments.UseVisualStyleBackColor = true;
            // 
            // chkReverseAccruals
            // 
            resources.ApplyResources(this.chkReverseAccruals, "chkReverseAccruals");
            this.chkReverseAccruals.Name = "chkReverseAccruals";
            this.chkReverseAccruals.UseVisualStyleBackColor = true;
            // 
            // chkAutoDepreciation
            // 
            resources.ApplyResources(this.chkAutoDepreciation, "chkAutoDepreciation");
            this.chkAutoDepreciation.Name = "chkAutoDepreciation";
            this.chkAutoDepreciation.UseVisualStyleBackColor = true;
            // 
            // tabConfirm
            // 
            this.tabConfirm.Controls.Add(this.btnCompleteClosing);
            this.tabConfirm.Controls.Add(this.txtPinPassword);
            this.tabConfirm.Controls.Add(this.lblPin);
            this.tabConfirm.Controls.Add(this.chkConfirmReviewed);
            this.tabConfirm.Controls.Add(this.lblConfirmText);
            resources.ApplyResources(this.tabConfirm, "tabConfirm");
            this.tabConfirm.Name = "tabConfirm";
            this.tabConfirm.UseVisualStyleBackColor = true;
            // 
            // btnCompleteClosing
            // 
            this.btnCompleteClosing.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnCompleteClosing.ForeColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnCompleteClosing, "btnCompleteClosing");
            this.btnCompleteClosing.Name = "btnCompleteClosing";
            this.btnCompleteClosing.UseVisualStyleBackColor = false;
            this.btnCompleteClosing.Click += new System.EventHandler(this.btnCompleteClosing_Click);
            // 
            // txtPinPassword
            // 
            resources.ApplyResources(this.txtPinPassword, "txtPinPassword");
            this.txtPinPassword.Name = "txtPinPassword";
            this.txtPinPassword.UseSystemPasswordChar = true;
            // 
            // lblPin
            // 
            resources.ApplyResources(this.lblPin, "lblPin");
            this.lblPin.Name = "lblPin";
            // 
            // chkConfirmReviewed
            // 
            resources.ApplyResources(this.chkConfirmReviewed, "chkConfirmReviewed");
            this.chkConfirmReviewed.Name = "chkConfirmReviewed";
            this.chkConfirmReviewed.UseVisualStyleBackColor = true;
            // 
            // lblConfirmText
            // 
            resources.ApplyResources(this.lblConfirmText, "lblConfirmText");
            this.lblConfirmText.Name = "lblConfirmText";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.btnCancel);
            this.panelFooter.Controls.Add(this.btnNext);
            this.panelFooter.Controls.Add(this.btnBack);
            this.panelFooter.Controls.Add(this.btnHelp);
            resources.ApplyResources(this.panelFooter, "panelFooter");
            this.panelFooter.Name = "panelFooter";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            resources.ApplyResources(this.btnNext, "btnNext");
            this.btnNext.Name = "btnNext";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            resources.ApplyResources(this.btnBack, "btnBack");
            this.btnBack.Name = "btnBack";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnHelp
            // 
            resources.ApplyResources(this.btnHelp, "btnHelp");
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // frm_period_close_wizard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabSteps);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.lblTitle);
            this.KeyPreview = true;
            this.Name = "frm_period_close_wizard";
            this.Load += new System.EventHandler(this.frm_period_close_wizard_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_period_close_wizard_KeyDown);
            this.tabSteps.ResumeLayout(false);
            this.tabChecklist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridChecklist)).EndInit();
            this.tabSummary.ResumeLayout(false);
            this.tableSummary.ResumeLayout(false);
            this.tabAdjustments.ResumeLayout(false);
            this.tabAdjustments.PerformLayout();
            this.tabConfirm.ResumeLayout(false);
            this.tabConfirm.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TabControl tabSteps;
        private System.Windows.Forms.TabPage tabChecklist;
        private System.Windows.Forms.TabPage tabSummary;
        private System.Windows.Forms.TabPage tabAdjustments;
        private System.Windows.Forms.TabPage tabConfirm;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.DataGridView gridChecklist;
        private System.Windows.Forms.TableLayoutPanel tableSummary;
        private System.Windows.Forms.Label lblTotalTransactions;
        private System.Windows.Forms.Label lblTotalTransactionsValue;
        private System.Windows.Forms.Label lblTotalJournals;
        private System.Windows.Forms.Label lblTotalJournalsValue;
        private System.Windows.Forms.Label lblDebits;
        private System.Windows.Forms.Label lblDebitsValue;
        private System.Windows.Forms.Label lblCredits;
        private System.Windows.Forms.Label lblCreditsValue;
        private System.Windows.Forms.Label lblNetProfit;
        private System.Windows.Forms.Label lblNetProfitValue;
        private System.Windows.Forms.Label lblOutOfBalance;
        private System.Windows.Forms.Label lblOutOfBalanceValue;
        private System.Windows.Forms.CheckBox chkReverseAccruals;
        private System.Windows.Forms.CheckBox chkAutoDepreciation;
        private System.Windows.Forms.Button btnCompleteClosing;
        private System.Windows.Forms.TextBox txtPinPassword;
        private System.Windows.Forms.Label lblPin;
        private System.Windows.Forms.CheckBox chkConfirmReviewed;
        private System.Windows.Forms.Label lblConfirmText;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChecklistKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChecklistItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colChecklistPassed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChecklistStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChecklistPending;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFixModule;
        private System.Windows.Forms.DataGridViewLinkColumn colFixIssues;
    }
}
