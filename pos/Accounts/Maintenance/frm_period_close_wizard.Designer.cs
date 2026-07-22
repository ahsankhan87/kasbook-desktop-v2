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
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.lblTitle.Size = new System.Drawing.Size(984, 48);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Month-End Closing Wizard";
            // 
            // tabSteps
            // 
            this.tabSteps.Controls.Add(this.tabChecklist);
            this.tabSteps.Controls.Add(this.tabSummary);
            this.tabSteps.Controls.Add(this.tabAdjustments);
            this.tabSteps.Controls.Add(this.tabConfirm);
            this.tabSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSteps.Location = new System.Drawing.Point(0, 48);
            this.tabSteps.Name = "tabSteps";
            this.tabSteps.SelectedIndex = 0;
            this.tabSteps.Size = new System.Drawing.Size(984, 485);
            this.tabSteps.TabIndex = 1;
            this.tabSteps.SelectedIndexChanged += new System.EventHandler(this.tabSteps_SelectedIndexChanged);
            // 
            // tabChecklist
            // 
            this.tabChecklist.Controls.Add(this.gridChecklist);
            this.tabChecklist.Location = new System.Drawing.Point(4, 26);
            this.tabChecklist.Name = "tabChecklist";
            this.tabChecklist.Padding = new System.Windows.Forms.Padding(8);
            this.tabChecklist.Size = new System.Drawing.Size(976, 455);
            this.tabChecklist.TabIndex = 0;
            this.tabChecklist.Text = "Step 1 - Pre-Close Checklist";
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
            this.gridChecklist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridChecklist.Location = new System.Drawing.Point(8, 8);
            this.gridChecklist.Name = "gridChecklist";
            this.gridChecklist.ReadOnly = true;
            this.gridChecklist.RowHeadersVisible = false;
            this.gridChecklist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridChecklist.Size = new System.Drawing.Size(960, 439);
            this.gridChecklist.TabIndex = 0;
            this.gridChecklist.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridChecklist_CellContentClick);
            this.gridChecklist.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridChecklist_CellFormatting);
            // 
            // colChecklistKey
            // 
            this.colChecklistKey.DataPropertyName = "item_key";
            this.colChecklistKey.HeaderText = "Key";
            this.colChecklistKey.Name = "colChecklistKey";
            this.colChecklistKey.ReadOnly = true;
            this.colChecklistKey.Visible = false;
            // 
            // colChecklistItem
            // 
            this.colChecklistItem.DataPropertyName = "item_name";
            this.colChecklistItem.HeaderText = "Checklist Item";
            this.colChecklistItem.Name = "colChecklistItem";
            this.colChecklistItem.ReadOnly = true;
            // 
            // colChecklistPassed
            // 
            this.colChecklistPassed.DataPropertyName = "is_passed";
            this.colChecklistPassed.HeaderText = "Passed";
            this.colChecklistPassed.Name = "colChecklistPassed";
            this.colChecklistPassed.ReadOnly = true;
            this.colChecklistPassed.Visible = false;
            // 
            // colChecklistStatus
            // 
            this.colChecklistStatus.HeaderText = "Status";
            this.colChecklistStatus.Name = "colChecklistStatus";
            this.colChecklistStatus.ReadOnly = true;
            // 
            // colChecklistPending
            // 
            this.colChecklistPending.DataPropertyName = "pending_count";
            this.colChecklistPending.HeaderText = "Pending Count";
            this.colChecklistPending.Name = "colChecklistPending";
            this.colChecklistPending.ReadOnly = true;
            // 
            // colFixModule
            // 
            this.colFixModule.DataPropertyName = "fix_module";
            this.colFixModule.HeaderText = "Fix Module";
            this.colFixModule.Name = "colFixModule";
            this.colFixModule.ReadOnly = true;
            // 
            // colFixIssues
            // 
            this.colFixIssues.HeaderText = "Fix Issues";
            this.colFixIssues.Name = "colFixIssues";
            this.colFixIssues.ReadOnly = true;
            this.colFixIssues.Text = "Fix Issues";
            this.colFixIssues.UseColumnTextForLinkValue = true;
            // 
            // tabSummary
            // 
            this.tabSummary.Controls.Add(this.tableSummary);
            this.tabSummary.Location = new System.Drawing.Point(4, 26);
            this.tabSummary.Name = "tabSummary";
            this.tabSummary.Padding = new System.Windows.Forms.Padding(12);
            this.tabSummary.Size = new System.Drawing.Size(976, 455);
            this.tabSummary.TabIndex = 1;
            this.tabSummary.Text = "Step 2 - Period Summary";
            this.tabSummary.UseVisualStyleBackColor = true;
            // 
            // tableSummary
            // 
            this.tableSummary.ColumnCount = 2;
            this.tableSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
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
            this.tableSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableSummary.Location = new System.Drawing.Point(12, 12);
            this.tableSummary.Name = "tableSummary";
            this.tableSummary.RowCount = 6;
            this.tableSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableSummary.Size = new System.Drawing.Size(952, 260);
            this.tableSummary.TabIndex = 0;
            // 
            // lblTotalTransactions
            // 
            this.lblTotalTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalTransactions.Location = new System.Drawing.Point(3, 0);
            this.lblTotalTransactions.Name = "lblTotalTransactions";
            this.lblTotalTransactions.Size = new System.Drawing.Size(565, 42);
            this.lblTotalTransactions.TabIndex = 0;
            this.lblTotalTransactions.Text = "Total Transactions:";
            this.lblTotalTransactions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalTransactionsValue
            // 
            this.lblTotalTransactionsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalTransactionsValue.Location = new System.Drawing.Point(574, 0);
            this.lblTotalTransactionsValue.Name = "lblTotalTransactionsValue";
            this.lblTotalTransactionsValue.Size = new System.Drawing.Size(375, 42);
            this.lblTotalTransactionsValue.TabIndex = 1;
            this.lblTotalTransactionsValue.Text = "0";
            this.lblTotalTransactionsValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotalJournals
            // 
            this.lblTotalJournals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalJournals.Location = new System.Drawing.Point(3, 42);
            this.lblTotalJournals.Name = "lblTotalJournals";
            this.lblTotalJournals.Size = new System.Drawing.Size(565, 42);
            this.lblTotalJournals.TabIndex = 2;
            this.lblTotalJournals.Text = "Total Journals:";
            this.lblTotalJournals.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTotalJournalsValue
            // 
            this.lblTotalJournalsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalJournalsValue.Location = new System.Drawing.Point(574, 42);
            this.lblTotalJournalsValue.Name = "lblTotalJournalsValue";
            this.lblTotalJournalsValue.Size = new System.Drawing.Size(375, 42);
            this.lblTotalJournalsValue.TabIndex = 3;
            this.lblTotalJournalsValue.Text = "0";
            this.lblTotalJournalsValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDebits
            // 
            this.lblDebits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDebits.Location = new System.Drawing.Point(3, 84);
            this.lblDebits.Name = "lblDebits";
            this.lblDebits.Size = new System.Drawing.Size(565, 42);
            this.lblDebits.TabIndex = 4;
            this.lblDebits.Text = "Total Debits:";
            this.lblDebits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDebitsValue
            // 
            this.lblDebitsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDebitsValue.Location = new System.Drawing.Point(574, 84);
            this.lblDebitsValue.Name = "lblDebitsValue";
            this.lblDebitsValue.Size = new System.Drawing.Size(375, 42);
            this.lblDebitsValue.TabIndex = 5;
            this.lblDebitsValue.Text = "0.00";
            this.lblDebitsValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCredits
            // 
            this.lblCredits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCredits.Location = new System.Drawing.Point(3, 126);
            this.lblCredits.Name = "lblCredits";
            this.lblCredits.Size = new System.Drawing.Size(565, 42);
            this.lblCredits.TabIndex = 6;
            this.lblCredits.Text = "Total Credits:";
            this.lblCredits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCreditsValue
            // 
            this.lblCreditsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCreditsValue.Location = new System.Drawing.Point(574, 126);
            this.lblCreditsValue.Name = "lblCreditsValue";
            this.lblCreditsValue.Size = new System.Drawing.Size(375, 42);
            this.lblCreditsValue.TabIndex = 7;
            this.lblCreditsValue.Text = "0.00";
            this.lblCreditsValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNetProfit
            // 
            this.lblNetProfit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetProfit.Location = new System.Drawing.Point(3, 168);
            this.lblNetProfit.Name = "lblNetProfit";
            this.lblNetProfit.Size = new System.Drawing.Size(565, 42);
            this.lblNetProfit.TabIndex = 8;
            this.lblNetProfit.Text = "Net Profit for period:";
            this.lblNetProfit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNetProfitValue
            // 
            this.lblNetProfitValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetProfitValue.Location = new System.Drawing.Point(574, 168);
            this.lblNetProfitValue.Name = "lblNetProfitValue";
            this.lblNetProfitValue.Size = new System.Drawing.Size(375, 42);
            this.lblNetProfitValue.TabIndex = 9;
            this.lblNetProfitValue.Text = "0.00";
            this.lblNetProfitValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOutOfBalance
            // 
            this.lblOutOfBalance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOutOfBalance.Location = new System.Drawing.Point(3, 210);
            this.lblOutOfBalance.Name = "lblOutOfBalance";
            this.lblOutOfBalance.Size = new System.Drawing.Size(565, 50);
            this.lblOutOfBalance.TabIndex = 10;
            this.lblOutOfBalance.Text = "Out-of-balance entries:";
            this.lblOutOfBalance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOutOfBalanceValue
            // 
            this.lblOutOfBalanceValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOutOfBalanceValue.Location = new System.Drawing.Point(574, 210);
            this.lblOutOfBalanceValue.Name = "lblOutOfBalanceValue";
            this.lblOutOfBalanceValue.Size = new System.Drawing.Size(375, 50);
            this.lblOutOfBalanceValue.TabIndex = 11;
            this.lblOutOfBalanceValue.Text = "0";
            this.lblOutOfBalanceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabAdjustments
            // 
            this.tabAdjustments.Controls.Add(this.chkReverseAccruals);
            this.tabAdjustments.Controls.Add(this.chkAutoDepreciation);
            this.tabAdjustments.Location = new System.Drawing.Point(4, 26);
            this.tabAdjustments.Name = "tabAdjustments";
            this.tabAdjustments.Padding = new System.Windows.Forms.Padding(16);
            this.tabAdjustments.Size = new System.Drawing.Size(976, 455);
            this.tabAdjustments.TabIndex = 2;
            this.tabAdjustments.Text = "Step 3 - Depreciation && Accruals";
            this.tabAdjustments.UseVisualStyleBackColor = true;
            // 
            // chkReverseAccruals
            // 
            this.chkReverseAccruals.AutoSize = true;
            this.chkReverseAccruals.Location = new System.Drawing.Point(20, 64);
            this.chkReverseAccruals.Name = "chkReverseAccruals";
            this.chkReverseAccruals.Size = new System.Drawing.Size(281, 21);
            this.chkReverseAccruals.TabIndex = 1;
            this.chkReverseAccruals.Text = "Reverse accrual entries from prior period";
            this.chkReverseAccruals.UseVisualStyleBackColor = true;
            // 
            // chkAutoDepreciation
            // 
            this.chkAutoDepreciation.AutoSize = true;
            this.chkAutoDepreciation.Location = new System.Drawing.Point(20, 28);
            this.chkAutoDepreciation.Name = "chkAutoDepreciation";
            this.chkAutoDepreciation.Size = new System.Drawing.Size(269, 21);
            this.chkAutoDepreciation.TabIndex = 0;
            this.chkAutoDepreciation.Text = "Auto-post depreciation entries for period";
            this.chkAutoDepreciation.UseVisualStyleBackColor = true;
            // 
            // tabConfirm
            // 
            this.tabConfirm.Controls.Add(this.btnCompleteClosing);
            this.tabConfirm.Controls.Add(this.txtPinPassword);
            this.tabConfirm.Controls.Add(this.lblPin);
            this.tabConfirm.Controls.Add(this.chkConfirmReviewed);
            this.tabConfirm.Controls.Add(this.lblConfirmText);
            this.tabConfirm.Location = new System.Drawing.Point(4, 26);
            this.tabConfirm.Name = "tabConfirm";
            this.tabConfirm.Padding = new System.Windows.Forms.Padding(16);
            this.tabConfirm.Size = new System.Drawing.Size(976, 455);
            this.tabConfirm.TabIndex = 3;
            this.tabConfirm.Text = "Step 4 - Confirm Close";
            this.tabConfirm.UseVisualStyleBackColor = true;
            // 
            // btnCompleteClosing
            // 
            this.btnCompleteClosing.BackColor = System.Drawing.Color.MidnightBlue;
            this.btnCompleteClosing.ForeColor = System.Drawing.Color.White;
            this.btnCompleteClosing.Location = new System.Drawing.Point(20, 152);
            this.btnCompleteClosing.Name = "btnCompleteClosing";
            this.btnCompleteClosing.Size = new System.Drawing.Size(172, 36);
            this.btnCompleteClosing.TabIndex = 4;
            this.btnCompleteClosing.Text = "Complete Closing";
            this.btnCompleteClosing.UseVisualStyleBackColor = false;
            this.btnCompleteClosing.Click += new System.EventHandler(this.btnCompleteClosing_Click);
            // 
            // txtPinPassword
            // 
            this.txtPinPassword.Location = new System.Drawing.Point(20, 115);
            this.txtPinPassword.Name = "txtPinPassword";
            this.txtPinPassword.Size = new System.Drawing.Size(273, 25);
            this.txtPinPassword.TabIndex = 3;
            this.txtPinPassword.UseSystemPasswordChar = true;
            // 
            // lblPin
            // 
            this.lblPin.AutoSize = true;
            this.lblPin.Location = new System.Drawing.Point(17, 92);
            this.lblPin.Name = "lblPin";
            this.lblPin.Size = new System.Drawing.Size(191, 17);
            this.lblPin.TabIndex = 2;
            this.lblPin.Text = "Password / PIN confirmation:";
            // 
            // chkConfirmReviewed
            // 
            this.chkConfirmReviewed.AutoSize = true;
            this.chkConfirmReviewed.Location = new System.Drawing.Point(20, 58);
            this.chkConfirmReviewed.Name = "chkConfirmReviewed";
            this.chkConfirmReviewed.Size = new System.Drawing.Size(15, 14);
            this.chkConfirmReviewed.TabIndex = 1;
            this.chkConfirmReviewed.UseVisualStyleBackColor = true;
            // 
            // lblConfirmText
            // 
            this.lblConfirmText.AutoSize = true;
            this.lblConfirmText.Location = new System.Drawing.Point(41, 56);
            this.lblConfirmText.Name = "lblConfirmText";
            this.lblConfirmText.Size = new System.Drawing.Size(322, 17);
            this.lblConfirmText.TabIndex = 0;
            this.lblConfirmText.Text = "I confirm all transactions for [period] have been reviewed";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.btnCancel);
            this.panelFooter.Controls.Add(this.btnNext);
            this.panelFooter.Controls.Add(this.btnBack);
            this.panelFooter.Controls.Add(this.btnHelp);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 533);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(984, 58);
            this.panelFooter.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(894, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(804, 14);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(78, 30);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(714, 14);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(78, 30);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "< Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(624, 14);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(78, 30);
            this.btnHelp.TabIndex = 3;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // frm_period_close_wizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 591);
            this.Controls.Add(this.tabSteps);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.KeyPreview = true;
            this.Name = "frm_period_close_wizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Month-End Closing Wizard";
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
