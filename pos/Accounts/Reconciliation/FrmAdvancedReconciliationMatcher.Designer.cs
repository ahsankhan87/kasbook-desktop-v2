namespace pos.Accounts.Reconciliation
{
    partial class FrmAdvancedReconciliationMatcher
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.dgvJournalUnmatched = new System.Windows.Forms.DataGridView();
            this.colJournalInvoice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJournalDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJournalAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelJournalHeader = new System.Windows.Forms.Panel();
            this.lblUnmatchedJournal = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.dgvMatchResults = new System.Windows.Forms.DataGridView();
            this.colResultJournal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResultJournalAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResultType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResultMatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResultMatchAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResultScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblMatchResults = new System.Windows.Forms.Label();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.tabMatches = new System.Windows.Forms.TabControl();
            this.tabSales = new System.Windows.Forms.TabPage();
            this.dgvSalesMatches = new System.Windows.Forms.DataGridView();
            this.colSalesInvoice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPurchase = new System.Windows.Forms.TabPage();
            this.dgvPurchaseMatches = new System.Windows.Forms.DataGridView();
            this.colPurchaseInvoice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseSupplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblPotentialMatches = new System.Windows.Forms.Label();
            this.panelSettings = new System.Windows.Forms.Panel();
            this.chkAutoTolerance = new System.Windows.Forms.CheckBox();
            this.lblTolerance = new System.Windows.Forms.Label();
            this.numTolerance = new System.Windows.Forms.NumericUpDown();
            this.lblCurrency = new System.Windows.Forms.Label();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnAutoMatch = new System.Windows.Forms.Button();
            this.btnManualMatch = new System.Windows.Forms.Button();
            this.btnApplyMatches = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournalUnmatched)).BeginInit();
            this.panelJournalHeader.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatchResults)).BeginInit();
            this.panelMiddle.SuspendLayout();
            this.tabMatches.SuspendLayout();
            this.tabSales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSalesMatches)).BeginInit();
            this.tabPurchase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchaseMatches)).BeginInit();
            this.panelSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerance)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();

            // splitContainer
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Panel1.Controls.Add(this.panelLeft);
            this.splitContainer.Panel2.Controls.Add(this.panelRight);
            this.splitContainer.Size = new System.Drawing.Size(1400, 900);
            this.splitContainer.SplitterDistance = 400;
            this.splitContainer.TabIndex = 0;

            // panelLeft
            this.panelLeft.Controls.Add(this.dgvJournalUnmatched);
            this.panelLeft.Controls.Add(this.panelJournalHeader);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(400, 900);
            this.panelLeft.TabIndex = 0;

            // dgvJournalUnmatched
            this.dgvJournalUnmatched.AllowUserToAddRows = false;
            this.dgvJournalUnmatched.ColumnHeadersHeight = 29;
            this.dgvJournalUnmatched.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colJournalInvoice,
            this.colJournalDate,
            this.colJournalAmount});
            this.dgvJournalUnmatched.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvJournalUnmatched.Location = new System.Drawing.Point(0, 30);
            this.dgvJournalUnmatched.Name = "dgvJournalUnmatched";
            this.dgvJournalUnmatched.RowHeadersWidth = 51;
            this.dgvJournalUnmatched.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvJournalUnmatched.Size = new System.Drawing.Size(400, 870);
            this.dgvJournalUnmatched.TabIndex = 0;

            // colJournalInvoice
            this.colJournalInvoice.DataPropertyName = "invoice_no";
            this.colJournalInvoice.HeaderText = "Invoice No";
            this.colJournalInvoice.MinimumWidth = 6;
            this.colJournalInvoice.Name = "colJournalInvoice";
            this.colJournalInvoice.Width = 120;

            // colJournalDate
            this.colJournalDate.DataPropertyName = "entry_date";
            this.colJournalDate.HeaderText = "Date";
            this.colJournalDate.MinimumWidth = 6;
            this.colJournalDate.Name = "colJournalDate";
            this.colJournalDate.Width = 100;

            // colJournalAmount
            this.colJournalAmount.DataPropertyName = "debit";
            this.colJournalAmount.HeaderText = "Amount";
            this.colJournalAmount.MinimumWidth = 6;
            this.colJournalAmount.Name = "colJournalAmount";
            this.colJournalAmount.Width = 100;

            // panelJournalHeader
            this.panelJournalHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panelJournalHeader.Controls.Add(this.lblUnmatchedJournal);
            this.panelJournalHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelJournalHeader.Location = new System.Drawing.Point(0, 0);
            this.panelJournalHeader.Name = "panelJournalHeader";
            this.panelJournalHeader.Size = new System.Drawing.Size(400, 30);
            this.panelJournalHeader.TabIndex = 1;

            // lblUnmatchedJournal
            this.lblUnmatchedJournal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUnmatchedJournal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUnmatchedJournal.ForeColor = System.Drawing.Color.White;
            this.lblUnmatchedJournal.Location = new System.Drawing.Point(0, 0);
            this.lblUnmatchedJournal.Name = "lblUnmatchedJournal";
            this.lblUnmatchedJournal.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblUnmatchedJournal.Size = new System.Drawing.Size(400, 30);
            this.lblUnmatchedJournal.TabIndex = 0;
            this.lblUnmatchedJournal.Text = "Unmatched Journal Entries";
            this.lblUnmatchedJournal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // panelRight
            this.panelRight.Controls.Add(this.panelBottom);
            this.panelRight.Controls.Add(this.panelMiddle);
            this.panelRight.Controls.Add(this.panelSettings);
            this.panelRight.Controls.Add(this.panelButtons);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(0, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(996, 900);
            this.panelRight.TabIndex = 1;

            // panelBottom
            this.panelBottom.Controls.Add(this.dgvMatchResults);
            this.panelBottom.Controls.Add(this.lblMatchResults);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 600);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(996, 250);
            this.panelBottom.TabIndex = 3;

            // dgvMatchResults
            this.dgvMatchResults.AllowUserToAddRows = false;
            this.dgvMatchResults.ColumnHeadersHeight = 29;
            this.dgvMatchResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colResultJournal,
            this.colResultJournalAmount,
            this.colResultType,
            this.colResultMatch,
            this.colResultMatchAmount,
            this.colResultScore});
            this.dgvMatchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMatchResults.Location = new System.Drawing.Point(0, 25);
            this.dgvMatchResults.Name = "dgvMatchResults";
            this.dgvMatchResults.RowHeadersWidth = 51;
            this.dgvMatchResults.Size = new System.Drawing.Size(996, 225);
            this.dgvMatchResults.TabIndex = 0;

            // colResultJournal
            this.colResultJournal.HeaderText = "Journal Invoice";
            this.colResultJournal.MinimumWidth = 6;
            this.colResultJournal.Name = "colResultJournal";
            this.colResultJournal.Width = 100;

            // colResultJournalAmount
            this.colResultJournalAmount.HeaderText = "Journal Amount";
            this.colResultJournalAmount.MinimumWidth = 6;
            this.colResultJournalAmount.Name = "colResultJournalAmount";
            this.colResultJournalAmount.Width = 100;

            // colResultType
            this.colResultType.HeaderText = "Type";
            this.colResultType.MinimumWidth = 6;
            this.colResultType.Name = "colResultType";
            this.colResultType.Width = 70;

            // colResultMatch
            this.colResultMatch.HeaderText = "Match Invoice";
            this.colResultMatch.MinimumWidth = 6;
            this.colResultMatch.Name = "colResultMatch";
            this.colResultMatch.Width = 100;

            // colResultMatchAmount
            this.colResultMatchAmount.HeaderText = "Match Amount";
            this.colResultMatchAmount.MinimumWidth = 6;
            this.colResultMatchAmount.Name = "colResultMatchAmount";
            this.colResultMatchAmount.Width = 100;

            // colResultScore
            this.colResultScore.HeaderText = "Match Score %";
            this.colResultScore.MinimumWidth = 6;
            this.colResultScore.Name = "colResultScore";
            this.colResultScore.Width = 80;

            // lblMatchResults
            this.lblMatchResults.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.lblMatchResults.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMatchResults.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMatchResults.ForeColor = System.Drawing.Color.White;
            this.lblMatchResults.Location = new System.Drawing.Point(0, 0);
            this.lblMatchResults.Name = "lblMatchResults";
            this.lblMatchResults.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblMatchResults.Size = new System.Drawing.Size(996, 25);
            this.lblMatchResults.TabIndex = 1;
            this.lblMatchResults.Text = "Reconciliation Matches";
            this.lblMatchResults.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // panelMiddle
            this.panelMiddle.Controls.Add(this.tabMatches);
            this.panelMiddle.Controls.Add(this.lblPotentialMatches);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(0, 55);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(996, 545);
            this.panelMiddle.TabIndex = 2;

            // tabMatches
            this.tabMatches.Controls.Add(this.tabSales);
            this.tabMatches.Controls.Add(this.tabPurchase);
            this.tabMatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMatches.Location = new System.Drawing.Point(0, 25);
            this.tabMatches.Name = "tabMatches";
            this.tabMatches.SelectedIndex = 0;
            this.tabMatches.Size = new System.Drawing.Size(996, 520);
            this.tabMatches.TabIndex = 1;

            // tabSales
            this.tabSales.Controls.Add(this.dgvSalesMatches);
            this.tabSales.Location = new System.Drawing.Point(4, 25);
            this.tabSales.Name = "tabSales";
            this.tabSales.Padding = new System.Windows.Forms.Padding(3);
            this.tabSales.Size = new System.Drawing.Size(988, 491);
            this.tabSales.TabIndex = 0;
            this.tabSales.Text = "Sales Matches";
            this.tabSales.UseVisualStyleBackColor = true;

            // dgvSalesMatches
            this.dgvSalesMatches.AllowUserToAddRows = false;
            this.dgvSalesMatches.ColumnHeadersHeight = 29;
            this.dgvSalesMatches.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSalesInvoice,
            this.colSalesDate,
            this.colSalesCustomer,
            this.colSalesAmount});
            this.dgvSalesMatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSalesMatches.Location = new System.Drawing.Point(3, 3);
            this.dgvSalesMatches.Name = "dgvSalesMatches";
            this.dgvSalesMatches.RowHeadersWidth = 51;
            this.dgvSalesMatches.Size = new System.Drawing.Size(982, 485);
            this.dgvSalesMatches.TabIndex = 0;

            // colSalesInvoice
            this.colSalesInvoice.DataPropertyName = "invoice_no";
            this.colSalesInvoice.HeaderText = "Invoice No";
            this.colSalesInvoice.MinimumWidth = 6;
            this.colSalesInvoice.Name = "colSalesInvoice";
            this.colSalesInvoice.Width = 100;

            // colSalesDate
            this.colSalesDate.DataPropertyName = "invoice_date";
            this.colSalesDate.HeaderText = "Date";
            this.colSalesDate.MinimumWidth = 6;
            this.colSalesDate.Name = "colSalesDate";
            this.colSalesDate.Width = 90;

            // colSalesCustomer
            this.colSalesCustomer.DataPropertyName = "customer_name";
            this.colSalesCustomer.HeaderText = "Customer";
            this.colSalesCustomer.MinimumWidth = 6;
            this.colSalesCustomer.Name = "colSalesCustomer";
            this.colSalesCustomer.Width = 150;

            // colSalesAmount
            this.colSalesAmount.DataPropertyName = "total_amount";
            this.colSalesAmount.HeaderText = "Amount";
            this.colSalesAmount.MinimumWidth = 6;
            this.colSalesAmount.Name = "colSalesAmount";
            this.colSalesAmount.Width = 100;

            // tabPurchase
            this.tabPurchase.Controls.Add(this.dgvPurchaseMatches);
            this.tabPurchase.Location = new System.Drawing.Point(4, 25);
            this.tabPurchase.Name = "tabPurchase";
            this.tabPurchase.Padding = new System.Windows.Forms.Padding(3);
            this.tabPurchase.Size = new System.Drawing.Size(988, 491);
            this.tabPurchase.TabIndex = 1;
            this.tabPurchase.Text = "Purchase Matches";
            this.tabPurchase.UseVisualStyleBackColor = true;

            // dgvPurchaseMatches
            this.dgvPurchaseMatches.AllowUserToAddRows = false;
            this.dgvPurchaseMatches.ColumnHeadersHeight = 29;
            this.dgvPurchaseMatches.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPurchaseInvoice,
            this.colPurchaseDate,
            this.colPurchaseSupplier,
            this.colPurchaseAmount});
            this.dgvPurchaseMatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPurchaseMatches.Location = new System.Drawing.Point(3, 3);
            this.dgvPurchaseMatches.Name = "dgvPurchaseMatches";
            this.dgvPurchaseMatches.RowHeadersWidth = 51;
            this.dgvPurchaseMatches.Size = new System.Drawing.Size(982, 485);
            this.dgvPurchaseMatches.TabIndex = 0;

            // colPurchaseInvoice
            this.colPurchaseInvoice.DataPropertyName = "invoice_no";
            this.colPurchaseInvoice.HeaderText = "Invoice No";
            this.colPurchaseInvoice.MinimumWidth = 6;
            this.colPurchaseInvoice.Name = "colPurchaseInvoice";
            this.colPurchaseInvoice.Width = 100;

            // colPurchaseDate
            this.colPurchaseDate.DataPropertyName = "invoice_date";
            this.colPurchaseDate.HeaderText = "Date";
            this.colPurchaseDate.MinimumWidth = 6;
            this.colPurchaseDate.Name = "colPurchaseDate";
            this.colPurchaseDate.Width = 90;

            // colPurchaseSupplier
            this.colPurchaseSupplier.DataPropertyName = "supplier_name";
            this.colPurchaseSupplier.HeaderText = "Supplier";
            this.colPurchaseSupplier.MinimumWidth = 6;
            this.colPurchaseSupplier.Name = "colPurchaseSupplier";
            this.colPurchaseSupplier.Width = 150;

            // colPurchaseAmount
            this.colPurchaseAmount.DataPropertyName = "total_amount";
            this.colPurchaseAmount.HeaderText = "Amount";
            this.colPurchaseAmount.MinimumWidth = 6;
            this.colPurchaseAmount.Name = "colPurchaseAmount";
            this.colPurchaseAmount.Width = 100;

            // lblPotentialMatches
            this.lblPotentialMatches.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.lblPotentialMatches.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPotentialMatches.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPotentialMatches.ForeColor = System.Drawing.Color.White;
            this.lblPotentialMatches.Location = new System.Drawing.Point(0, 0);
            this.lblPotentialMatches.Name = "lblPotentialMatches";
            this.lblPotentialMatches.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblPotentialMatches.Size = new System.Drawing.Size(996, 25);
            this.lblPotentialMatches.TabIndex = 0;
            this.lblPotentialMatches.Text = "Potential Matches";
            this.lblPotentialMatches.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // panelSettings
            this.panelSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelSettings.Controls.Add(this.chkAutoTolerance);
            this.panelSettings.Controls.Add(this.numTolerance);
            this.panelSettings.Controls.Add(this.lblTolerance);
            this.panelSettings.Controls.Add(this.lblCurrency);
            this.panelSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSettings.Location = new System.Drawing.Point(0, 0);
            this.panelSettings.Name = "panelSettings";
            this.panelSettings.Padding = new System.Windows.Forms.Padding(10);
            this.panelSettings.Size = new System.Drawing.Size(996, 55);
            this.panelSettings.TabIndex = 1;

            // chkAutoTolerance
            this.chkAutoTolerance.AutoSize = true;
            this.chkAutoTolerance.Checked = true;
            this.chkAutoTolerance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoTolerance.Location = new System.Drawing.Point(13, 17);
            this.chkAutoTolerance.Name = "chkAutoTolerance";
            this.chkAutoTolerance.Size = new System.Drawing.Size(101, 17);
            this.chkAutoTolerance.TabIndex = 0;
            this.chkAutoTolerance.Text = "Match Tolerance";
            this.chkAutoTolerance.UseVisualStyleBackColor = true;

            // lblTolerance
            this.lblTolerance.AutoSize = true;
            this.lblTolerance.Location = new System.Drawing.Point(120, 20);
            this.lblTolerance.Name = "lblTolerance";
            this.lblTolerance.Size = new System.Drawing.Size(31, 13);
            this.lblTolerance.TabIndex = 1;
            this.lblTolerance.Text = "Amt:";

            // numTolerance
            this.numTolerance.DecimalPlaces = 2;
            this.numTolerance.Location = new System.Drawing.Point(160, 17);
            this.numTolerance.Name = "numTolerance";
            this.numTolerance.Size = new System.Drawing.Size(80, 20);
            this.numTolerance.TabIndex = 2;
            this.numTolerance.Value = new decimal(new int[] { 1, 0, 0, 0 });

            // lblCurrency
            this.lblCurrency.AutoSize = true;
            this.lblCurrency.Location = new System.Drawing.Point(250, 20);
            this.lblCurrency.Name = "lblCurrency";
            this.lblCurrency.Size = new System.Drawing.Size(24, 13);
            this.lblCurrency.TabIndex = 3;
            this.lblCurrency.Text = "SAR";

            // panelButtons
            this.panelButtons.BackColor = System.Drawing.Color.White;
            this.panelButtons.Controls.Add(this.btnAutoMatch);
            this.panelButtons.Controls.Add(this.btnManualMatch);
            this.panelButtons.Controls.Add(this.btnApplyMatches);
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 850);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new System.Windows.Forms.Padding(10);
            this.panelButtons.Size = new System.Drawing.Size(996, 50);
            this.panelButtons.TabIndex = 0;

            // btnAutoMatch
            this.btnAutoMatch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnAutoMatch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAutoMatch.ForeColor = System.Drawing.Color.White;
            this.btnAutoMatch.Location = new System.Drawing.Point(10, 10);
            this.btnAutoMatch.Name = "btnAutoMatch";
            this.btnAutoMatch.Size = new System.Drawing.Size(120, 30);
            this.btnAutoMatch.TabIndex = 0;
            this.btnAutoMatch.Text = "Auto Match";
            this.btnAutoMatch.UseVisualStyleBackColor = false;

            // btnManualMatch
            this.btnManualMatch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnManualMatch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnManualMatch.ForeColor = System.Drawing.Color.White;
            this.btnManualMatch.Location = new System.Drawing.Point(140, 10);
            this.btnManualMatch.Name = "btnManualMatch";
            this.btnManualMatch.Size = new System.Drawing.Size(120, 30);
            this.btnManualMatch.TabIndex = 1;
            this.btnManualMatch.Text = "Add Manual Match";
            this.btnManualMatch.UseVisualStyleBackColor = false;

            // btnApplyMatches
            this.btnApplyMatches.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnApplyMatches.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnApplyMatches.ForeColor = System.Drawing.Color.White;
            this.btnApplyMatches.Location = new System.Drawing.Point(270, 10);
            this.btnApplyMatches.Name = "btnApplyMatches";
            this.btnApplyMatches.Size = new System.Drawing.Size(120, 30);
            this.btnApplyMatches.TabIndex = 2;
            this.btnApplyMatches.Text = "Apply Matches";
            this.btnApplyMatches.UseVisualStyleBackColor = false;

            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(400, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;

            // FrmAdvancedReconciliationMatcher
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 900);
            this.Controls.Add(this.splitContainer);
            this.Name = "FrmAdvancedReconciliationMatcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Advanced Reconciliation Matcher";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournalUnmatched)).EndInit();
            this.panelJournalHeader.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatchResults)).EndInit();
            this.panelMiddle.ResumeLayout(false);
            this.tabMatches.ResumeLayout(false);
            this.tabSales.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSalesMatches)).EndInit();
            this.tabPurchase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchaseMatches)).EndInit();
            this.panelSettings.ResumeLayout(false);
            this.panelSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTolerance)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.DataGridView dgvJournalUnmatched;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJournalInvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJournalDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJournalAmount;
        private System.Windows.Forms.Panel panelJournalHeader;
        private System.Windows.Forms.Label lblUnmatchedJournal;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.DataGridView dgvMatchResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultJournal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultJournalAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultMatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultMatchAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResultScore;
        private System.Windows.Forms.Label lblMatchResults;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.TabControl tabMatches;
        private System.Windows.Forms.TabPage tabSales;
        private System.Windows.Forms.DataGridView dgvSalesMatches;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesInvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesAmount;
        private System.Windows.Forms.TabPage tabPurchase;
        private System.Windows.Forms.DataGridView dgvPurchaseMatches;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseInvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseAmount;
        private System.Windows.Forms.Label lblPotentialMatches;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.CheckBox chkAutoTolerance;
        private System.Windows.Forms.Label lblTolerance;
        private System.Windows.Forms.NumericUpDown numTolerance;
        private System.Windows.Forms.Label lblCurrency;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnAutoMatch;
        private System.Windows.Forms.Button btnManualMatch;
        private System.Windows.Forms.Button btnApplyMatches;
        private System.Windows.Forms.Button btnCancel;
    }
}
