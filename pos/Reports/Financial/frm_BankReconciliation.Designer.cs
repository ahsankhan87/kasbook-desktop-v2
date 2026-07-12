namespace pos.Reports.Financial
{
    partial class frm_BankReconciliation
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.txtStatementBalance = new System.Windows.Forms.TextBox();
            this.lblStatementBalance = new System.Windows.Forms.Label();
            this.dtpStatementDate = new System.Windows.Forms.DateTimePicker();
            this.lblStatementDate = new System.Windows.Forms.Label();
            this.cmbBankAccount = new System.Windows.Forms.ComboBox();
            this.lblBankAccount = new System.Windows.Forms.Label();
            this.mainSplit = new System.Windows.Forms.SplitContainer();
            this.dgvSystemTransactions = new System.Windows.Forms.DataGridView();
            this.colCleared = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colEntryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEntryId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rightSplit = new System.Windows.Forms.SplitContainer();
            this.pnlSummary = new System.Windows.Forms.Panel();
            this.lblDifference = new System.Windows.Forms.Label();
            this.lblDifferenceValue = new System.Windows.Forms.Label();
            this.lblBookBalance = new System.Windows.Forms.Label();
            this.lblBookBalanceValue = new System.Windows.Forms.Label();
            this.lblAdjustedBalance = new System.Windows.Forms.Label();
            this.lblAdjustedBalanceValue = new System.Windows.Forms.Label();
            this.lblOutstandingCheques = new System.Windows.Forms.Label();
            this.lblOutstandingChequesValue = new System.Windows.Forms.Label();
            this.lblOutstandingDeposits = new System.Windows.Forms.Label();
            this.lblOutstandingDepositsValue = new System.Windows.Forms.Label();
            this.lblStatement = new System.Windows.Forms.Label();
            this.lblStatementValue = new System.Windows.Forms.Label();
            this.dgvUncleared = new System.Windows.Forms.DataGridView();
            this.colUnclearedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnclearedInvoice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnclearedDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnclearedDebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnclearedCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnclearedAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).BeginInit();
            this.mainSplit.Panel1.SuspendLayout();
            this.mainSplit.Panel2.SuspendLayout();
            this.mainSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSystemTransactions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightSplit)).BeginInit();
            this.rightSplit.Panel1.SuspendLayout();
            this.rightSplit.Panel2.SuspendLayout();
            this.rightSplit.SuspendLayout();
            this.pnlSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUncleared)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnPrint);
            this.pnlTop.Controls.Add(this.btnSave);
            this.pnlTop.Controls.Add(this.btnLoad);
            this.pnlTop.Controls.Add(this.txtStatementBalance);
            this.pnlTop.Controls.Add(this.lblStatementBalance);
            this.pnlTop.Controls.Add(this.dtpStatementDate);
            this.pnlTop.Controls.Add(this.lblStatementDate);
            this.pnlTop.Controls.Add(this.cmbBankAccount);
            this.pnlTop.Controls.Add(this.lblBankAccount);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1240, 76);
            this.pnlTop.TabIndex = 0;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(1132, 24);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(96, 28);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "Print Bank Rec";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1024, 24);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save Reconciliation";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(926, 24);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(90, 28);
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // txtStatementBalance
            // 
            this.txtStatementBalance.Location = new System.Drawing.Point(744, 28);
            this.txtStatementBalance.Name = "txtStatementBalance";
            this.txtStatementBalance.Size = new System.Drawing.Size(168, 20);
            this.txtStatementBalance.TabIndex = 5;
            // 
            // lblStatementBalance
            // 
            this.lblStatementBalance.AutoSize = true;
            this.lblStatementBalance.Location = new System.Drawing.Point(607, 31);
            this.lblStatementBalance.Name = "lblStatementBalance";
            this.lblStatementBalance.Size = new System.Drawing.Size(131, 13);
            this.lblStatementBalance.TabIndex = 4;
            this.lblStatementBalance.Text = "Bank Statement Balance";
            // 
            // dtpStatementDate
            // 
            this.dtpStatementDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStatementDate.Location = new System.Drawing.Point(485, 28);
            this.dtpStatementDate.Name = "dtpStatementDate";
            this.dtpStatementDate.Size = new System.Drawing.Size(108, 20);
            this.dtpStatementDate.TabIndex = 3;
            // 
            // lblStatementDate
            // 
            this.lblStatementDate.AutoSize = true;
            this.lblStatementDate.Location = new System.Drawing.Point(400, 31);
            this.lblStatementDate.Name = "lblStatementDate";
            this.lblStatementDate.Size = new System.Drawing.Size(79, 13);
            this.lblStatementDate.TabIndex = 2;
            this.lblStatementDate.Text = "Statement Date";
            // 
            // cmbBankAccount
            // 
            this.cmbBankAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBankAccount.FormattingEnabled = true;
            this.cmbBankAccount.Location = new System.Drawing.Point(95, 28);
            this.cmbBankAccount.Name = "cmbBankAccount";
            this.cmbBankAccount.Size = new System.Drawing.Size(287, 21);
            this.cmbBankAccount.TabIndex = 1;
            // 
            // lblBankAccount
            // 
            this.lblBankAccount.AutoSize = true;
            this.lblBankAccount.Location = new System.Drawing.Point(15, 31);
            this.lblBankAccount.Name = "lblBankAccount";
            this.lblBankAccount.Size = new System.Drawing.Size(74, 13);
            this.lblBankAccount.TabIndex = 0;
            this.lblBankAccount.Text = "Bank Account";
            // 
            // mainSplit
            // 
            this.mainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplit.Location = new System.Drawing.Point(0, 76);
            this.mainSplit.Name = "mainSplit";
            // 
            // mainSplit.Panel1
            // 
            this.mainSplit.Panel1.Controls.Add(this.dgvSystemTransactions);
            // 
            // mainSplit.Panel2
            // 
            this.mainSplit.Panel2.Controls.Add(this.rightSplit);
            this.mainSplit.Size = new System.Drawing.Size(1240, 586);
            this.mainSplit.SplitterDistance = 620;
            this.mainSplit.TabIndex = 1;
            // 
            // dgvSystemTransactions
            // 
            this.dgvSystemTransactions.AllowUserToAddRows = false;
            this.dgvSystemTransactions.AllowUserToDeleteRows = false;
            this.dgvSystemTransactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSystemTransactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSystemTransactions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCleared,
            this.colEntryDate,
            this.colInvoice,
            this.colDescription,
            this.colDebit,
            this.colCredit,
            this.colAmount,
            this.colEntryId});
            this.dgvSystemTransactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSystemTransactions.Location = new System.Drawing.Point(0, 0);
            this.dgvSystemTransactions.Name = "dgvSystemTransactions";
            this.dgvSystemTransactions.RowHeadersVisible = false;
            this.dgvSystemTransactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSystemTransactions.Size = new System.Drawing.Size(620, 586);
            this.dgvSystemTransactions.TabIndex = 0;
            // 
            // colCleared
            // 
            this.colCleared.DataPropertyName = "is_cleared";
            this.colCleared.FillWeight = 45F;
            this.colCleared.HeaderText = "Cleared";
            this.colCleared.Name = "colCleared";
            // 
            // colEntryDate
            // 
            this.colEntryDate.DataPropertyName = "entry_date";
            this.colEntryDate.FillWeight = 75F;
            this.colEntryDate.HeaderText = "Date";
            this.colEntryDate.Name = "colEntryDate";
            this.colEntryDate.ReadOnly = true;
            // 
            // colInvoice
            // 
            this.colInvoice.DataPropertyName = "invoice_no";
            this.colInvoice.FillWeight = 80F;
            this.colInvoice.HeaderText = "Invoice";
            this.colInvoice.Name = "colInvoice";
            this.colInvoice.ReadOnly = true;
            // 
            // colDescription
            // 
            this.colDescription.DataPropertyName = "description";
            this.colDescription.FillWeight = 160F;
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            // 
            // colDebit
            // 
            this.colDebit.DataPropertyName = "debit";
            this.colDebit.FillWeight = 70F;
            this.colDebit.HeaderText = "Debit";
            this.colDebit.Name = "colDebit";
            this.colDebit.ReadOnly = true;
            // 
            // colCredit
            // 
            this.colCredit.DataPropertyName = "credit";
            this.colCredit.FillWeight = 70F;
            this.colCredit.HeaderText = "Credit";
            this.colCredit.Name = "colCredit";
            this.colCredit.ReadOnly = true;
            // 
            // colAmount
            // 
            this.colAmount.DataPropertyName = "amount";
            this.colAmount.FillWeight = 70F;
            this.colAmount.HeaderText = "Amount";
            this.colAmount.Name = "colAmount";
            this.colAmount.ReadOnly = true;
            // 
            // colEntryId
            // 
            this.colEntryId.DataPropertyName = "entry_id";
            this.colEntryId.HeaderText = "EntryId";
            this.colEntryId.Name = "colEntryId";
            this.colEntryId.ReadOnly = true;
            this.colEntryId.Visible = false;
            // 
            // rightSplit
            // 
            this.rightSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightSplit.Location = new System.Drawing.Point(0, 0);
            this.rightSplit.Name = "rightSplit";
            this.rightSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // rightSplit.Panel1
            // 
            this.rightSplit.Panel1.Controls.Add(this.pnlSummary);
            // 
            // rightSplit.Panel2
            // 
            this.rightSplit.Panel2.Controls.Add(this.dgvUncleared);
            this.rightSplit.Size = new System.Drawing.Size(616, 586);
            this.rightSplit.SplitterDistance = 260;
            this.rightSplit.TabIndex = 0;
            // 
            // pnlSummary
            // 
            this.pnlSummary.Controls.Add(this.lblDifference);
            this.pnlSummary.Controls.Add(this.lblDifferenceValue);
            this.pnlSummary.Controls.Add(this.lblBookBalance);
            this.pnlSummary.Controls.Add(this.lblBookBalanceValue);
            this.pnlSummary.Controls.Add(this.lblAdjustedBalance);
            this.pnlSummary.Controls.Add(this.lblAdjustedBalanceValue);
            this.pnlSummary.Controls.Add(this.lblOutstandingCheques);
            this.pnlSummary.Controls.Add(this.lblOutstandingChequesValue);
            this.pnlSummary.Controls.Add(this.lblOutstandingDeposits);
            this.pnlSummary.Controls.Add(this.lblOutstandingDepositsValue);
            this.pnlSummary.Controls.Add(this.lblStatement);
            this.pnlSummary.Controls.Add(this.lblStatementValue);
            this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSummary.Location = new System.Drawing.Point(0, 0);
            this.pnlSummary.Name = "pnlSummary";
            this.pnlSummary.Size = new System.Drawing.Size(616, 260);
            this.pnlSummary.TabIndex = 0;
            // 
            // lblDifference
            // 
            this.lblDifference.AutoSize = true;
            this.lblDifference.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDifference.Location = new System.Drawing.Point(16, 212);
            this.lblDifference.Name = "lblDifference";
            this.lblDifference.Size = new System.Drawing.Size(69, 15);
            this.lblDifference.TabIndex = 11;
            this.lblDifference.Text = "DIFFERENCE";
            // 
            // lblDifferenceValue
            // 
            this.lblDifferenceValue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDifferenceValue.Location = new System.Drawing.Point(335, 208);
            this.lblDifferenceValue.Name = "lblDifferenceValue";
            this.lblDifferenceValue.Size = new System.Drawing.Size(266, 23);
            this.lblDifferenceValue.TabIndex = 10;
            this.lblDifferenceValue.Text = "0.00";
            this.lblDifferenceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBookBalance
            // 
            this.lblBookBalance.AutoSize = true;
            this.lblBookBalance.Location = new System.Drawing.Point(16, 172);
            this.lblBookBalance.Name = "lblBookBalance";
            this.lblBookBalance.Size = new System.Drawing.Size(106, 13);
            this.lblBookBalance.TabIndex = 9;
            this.lblBookBalance.Text = "System Book Balance";
            // 
            // lblBookBalanceValue
            // 
            this.lblBookBalanceValue.Location = new System.Drawing.Point(335, 168);
            this.lblBookBalanceValue.Name = "lblBookBalanceValue";
            this.lblBookBalanceValue.Size = new System.Drawing.Size(266, 23);
            this.lblBookBalanceValue.TabIndex = 8;
            this.lblBookBalanceValue.Text = "0.00";
            this.lblBookBalanceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAdjustedBalance
            // 
            this.lblAdjustedBalance.AutoSize = true;
            this.lblAdjustedBalance.Location = new System.Drawing.Point(16, 132);
            this.lblAdjustedBalance.Name = "lblAdjustedBalance";
            this.lblAdjustedBalance.Size = new System.Drawing.Size(112, 13);
            this.lblAdjustedBalance.TabIndex = 7;
            this.lblAdjustedBalance.Text = "Adjusted Bank Balance";
            // 
            // lblAdjustedBalanceValue
            // 
            this.lblAdjustedBalanceValue.Location = new System.Drawing.Point(335, 128);
            this.lblAdjustedBalanceValue.Name = "lblAdjustedBalanceValue";
            this.lblAdjustedBalanceValue.Size = new System.Drawing.Size(266, 23);
            this.lblAdjustedBalanceValue.TabIndex = 6;
            this.lblAdjustedBalanceValue.Text = "0.00";
            this.lblAdjustedBalanceValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOutstandingCheques
            // 
            this.lblOutstandingCheques.AutoSize = true;
            this.lblOutstandingCheques.Location = new System.Drawing.Point(16, 92);
            this.lblOutstandingCheques.Name = "lblOutstandingCheques";
            this.lblOutstandingCheques.Size = new System.Drawing.Size(120, 13);
            this.lblOutstandingCheques.TabIndex = 5;
            this.lblOutstandingCheques.Text = "Outstanding Cheques (-)";
            // 
            // lblOutstandingChequesValue
            // 
            this.lblOutstandingChequesValue.Location = new System.Drawing.Point(335, 88);
            this.lblOutstandingChequesValue.Name = "lblOutstandingChequesValue";
            this.lblOutstandingChequesValue.Size = new System.Drawing.Size(266, 23);
            this.lblOutstandingChequesValue.TabIndex = 4;
            this.lblOutstandingChequesValue.Text = "0.00";
            this.lblOutstandingChequesValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOutstandingDeposits
            // 
            this.lblOutstandingDeposits.AutoSize = true;
            this.lblOutstandingDeposits.Location = new System.Drawing.Point(16, 52);
            this.lblOutstandingDeposits.Name = "lblOutstandingDeposits";
            this.lblOutstandingDeposits.Size = new System.Drawing.Size(117, 13);
            this.lblOutstandingDeposits.TabIndex = 3;
            this.lblOutstandingDeposits.Text = "Outstanding Deposits (+)";
            // 
            // lblOutstandingDepositsValue
            // 
            this.lblOutstandingDepositsValue.Location = new System.Drawing.Point(335, 48);
            this.lblOutstandingDepositsValue.Name = "lblOutstandingDepositsValue";
            this.lblOutstandingDepositsValue.Size = new System.Drawing.Size(266, 23);
            this.lblOutstandingDepositsValue.TabIndex = 2;
            this.lblOutstandingDepositsValue.Text = "0.00";
            this.lblOutstandingDepositsValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStatement
            // 
            this.lblStatement.AutoSize = true;
            this.lblStatement.Location = new System.Drawing.Point(16, 12);
            this.lblStatement.Name = "lblStatement";
            this.lblStatement.Size = new System.Drawing.Size(121, 13);
            this.lblStatement.TabIndex = 1;
            this.lblStatement.Text = "Bank Statement Balance";
            // 
            // lblStatementValue
            // 
            this.lblStatementValue.Location = new System.Drawing.Point(335, 8);
            this.lblStatementValue.Name = "lblStatementValue";
            this.lblStatementValue.Size = new System.Drawing.Size(266, 23);
            this.lblStatementValue.TabIndex = 0;
            this.lblStatementValue.Text = "0.00";
            this.lblStatementValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvUncleared
            // 
            this.dgvUncleared.AllowUserToAddRows = false;
            this.dgvUncleared.AllowUserToDeleteRows = false;
            this.dgvUncleared.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUncleared.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUncleared.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUnclearedDate,
            this.colUnclearedInvoice,
            this.colUnclearedDescription,
            this.colUnclearedDebit,
            this.colUnclearedCredit,
            this.colUnclearedAmount});
            this.dgvUncleared.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUncleared.Location = new System.Drawing.Point(0, 0);
            this.dgvUncleared.Name = "dgvUncleared";
            this.dgvUncleared.ReadOnly = true;
            this.dgvUncleared.RowHeadersVisible = false;
            this.dgvUncleared.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUncleared.Size = new System.Drawing.Size(616, 322);
            this.dgvUncleared.TabIndex = 0;
            // 
            // colUnclearedDate
            // 
            this.colUnclearedDate.DataPropertyName = "entry_date";
            this.colUnclearedDate.FillWeight = 70F;
            this.colUnclearedDate.HeaderText = "Date";
            this.colUnclearedDate.Name = "colUnclearedDate";
            this.colUnclearedDate.ReadOnly = true;
            // 
            // colUnclearedInvoice
            // 
            this.colUnclearedInvoice.DataPropertyName = "invoice_no";
            this.colUnclearedInvoice.FillWeight = 80F;
            this.colUnclearedInvoice.HeaderText = "Invoice";
            this.colUnclearedInvoice.Name = "colUnclearedInvoice";
            this.colUnclearedInvoice.ReadOnly = true;
            // 
            // colUnclearedDescription
            // 
            this.colUnclearedDescription.DataPropertyName = "description";
            this.colUnclearedDescription.FillWeight = 150F;
            this.colUnclearedDescription.HeaderText = "Description";
            this.colUnclearedDescription.Name = "colUnclearedDescription";
            this.colUnclearedDescription.ReadOnly = true;
            // 
            // colUnclearedDebit
            // 
            this.colUnclearedDebit.DataPropertyName = "debit";
            this.colUnclearedDebit.FillWeight = 65F;
            this.colUnclearedDebit.HeaderText = "Debit";
            this.colUnclearedDebit.Name = "colUnclearedDebit";
            this.colUnclearedDebit.ReadOnly = true;
            // 
            // colUnclearedCredit
            // 
            this.colUnclearedCredit.DataPropertyName = "credit";
            this.colUnclearedCredit.FillWeight = 65F;
            this.colUnclearedCredit.HeaderText = "Credit";
            this.colUnclearedCredit.Name = "colUnclearedCredit";
            this.colUnclearedCredit.ReadOnly = true;
            // 
            // colUnclearedAmount
            // 
            this.colUnclearedAmount.DataPropertyName = "amount";
            this.colUnclearedAmount.FillWeight = 70F;
            this.colUnclearedAmount.HeaderText = "Amount";
            this.colUnclearedAmount.Name = "colUnclearedAmount";
            this.colUnclearedAmount.ReadOnly = true;
            // 
            // frm_BankReconciliation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 662);
            this.Controls.Add(this.mainSplit);
            this.Controls.Add(this.pnlTop);
            this.Name = "frm_BankReconciliation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bank Reconciliation";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.mainSplit.Panel1.ResumeLayout(false);
            this.mainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).EndInit();
            this.mainSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSystemTransactions)).EndInit();
            this.rightSplit.Panel1.ResumeLayout(false);
            this.rightSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rightSplit)).EndInit();
            this.rightSplit.ResumeLayout(false);
            this.pnlSummary.ResumeLayout(false);
            this.pnlSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUncleared)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.ComboBox cmbBankAccount;
        private System.Windows.Forms.Label lblBankAccount;
        private System.Windows.Forms.DateTimePicker dtpStatementDate;
        private System.Windows.Forms.Label lblStatementDate;
        private System.Windows.Forms.TextBox txtStatementBalance;
        private System.Windows.Forms.Label lblStatementBalance;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.SplitContainer mainSplit;
        private System.Windows.Forms.DataGridView dgvSystemTransactions;
        private System.Windows.Forms.SplitContainer rightSplit;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.DataGridView dgvUncleared;
        private System.Windows.Forms.Label lblStatement;
        private System.Windows.Forms.Label lblStatementValue;
        private System.Windows.Forms.Label lblOutstandingDeposits;
        private System.Windows.Forms.Label lblOutstandingDepositsValue;
        private System.Windows.Forms.Label lblOutstandingCheques;
        private System.Windows.Forms.Label lblOutstandingChequesValue;
        private System.Windows.Forms.Label lblAdjustedBalance;
        private System.Windows.Forms.Label lblAdjustedBalanceValue;
        private System.Windows.Forms.Label lblBookBalance;
        private System.Windows.Forms.Label lblBookBalanceValue;
        private System.Windows.Forms.Label lblDifference;
        private System.Windows.Forms.Label lblDifferenceValue;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCleared;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEntryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEntryId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnclearedDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnclearedInvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnclearedDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnclearedDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnclearedCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnclearedAmount;
    }
}
