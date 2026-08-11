namespace pos.Accounts.Reconciliation
{
    partial class FrmJournalReconciliation
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabJournalEntries = new System.Windows.Forms.TabPage();
            this.panelJournalControls = new System.Windows.Forms.Panel();
            this.lblSearchJournal = new System.Windows.Forms.Label();
            this.txtSearchJournal = new System.Windows.Forms.TextBox();
            this.dgvJournalEntries = new System.Windows.Forms.DataGridView();
            this.colInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEntryDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReconcileDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabSalesEntries = new System.Windows.Forms.TabPage();
            this.panelSalesControls = new System.Windows.Forms.Panel();
            this.lblSearchSales = new System.Windows.Forms.Label();
            this.txtSearchSales = new System.Windows.Forms.TextBox();
            this.dgvSalesEntries = new System.Windows.Forms.DataGridView();
            this.tabPurchaseEntries = new System.Windows.Forms.TabPage();
            this.panelPurchaseControls = new System.Windows.Forms.Panel();
            this.lblSearchPurchase = new System.Windows.Forms.Label();
            this.txtSearchPurchase = new System.Windows.Forms.TextBox();
            this.dgvPurchaseEntries = new System.Windows.Forms.DataGridView();
            this.tabUnreconciled = new System.Windows.Forms.TabPage();
            this.dgvUnreconciled = new System.Windows.Forms.DataGridView();
            this.colUnreconciledInvoice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnreconciledDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnreconciledAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnreconciledDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.lblAccountFilter = new System.Windows.Forms.Label();
            this.cmbAccountFilter = new System.Windows.Forms.ComboBox();
            this.lblStatusFilter = new System.Windows.Forms.Label();
            this.cmbStatusFilter = new System.Windows.Forms.ComboBox();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnReconcile = new System.Windows.Forms.Button();
            this.btnReverseReconciliation = new System.Windows.Forms.Button();
            this.btnAdvancedMatch = new System.Windows.Forms.Button();
            this.btnExportReport = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblSummary = new System.Windows.Forms.Label();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.colPurchaseInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchaseStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalesStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabJournalEntries.SuspendLayout();
            this.panelJournalControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournalEntries)).BeginInit();
            this.tabSalesEntries.SuspendLayout();
            this.panelSalesControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSalesEntries)).BeginInit();
            this.tabPurchaseEntries.SuspendLayout();
            this.panelPurchaseControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchaseEntries)).BeginInit();
            this.tabUnreconciled.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnreconciled)).BeginInit();
            this.panelFilters.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabJournalEntries);
            this.tabControl1.Controls.Add(this.tabSalesEntries);
            this.tabControl1.Controls.Add(this.tabPurchaseEntries);
            this.tabControl1.Controls.Add(this.tabUnreconciled);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 80);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1400, 630);
            this.tabControl1.TabIndex = 0;
            // 
            // tabJournalEntries
            // 
            this.tabJournalEntries.Controls.Add(this.dgvJournalEntries);
            this.tabJournalEntries.Controls.Add(this.panelJournalControls);
            this.tabJournalEntries.Location = new System.Drawing.Point(4, 25);
            this.tabJournalEntries.Name = "tabJournalEntries";
            this.tabJournalEntries.Size = new System.Drawing.Size(1392, 601);
            this.tabJournalEntries.TabIndex = 0;
            this.tabJournalEntries.Text = "Journal Entries";
            // 
            // panelJournalControls
            // 
            this.panelJournalControls.Controls.Add(this.lblSearchJournal);
            this.panelJournalControls.Controls.Add(this.txtSearchJournal);
            this.panelJournalControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelJournalControls.Location = new System.Drawing.Point(0, 0);
            this.panelJournalControls.Name = "panelJournalControls";
            this.panelJournalControls.Size = new System.Drawing.Size(1392, 40);
            this.panelJournalControls.TabIndex = 0;
            // 
            // lblSearchJournal
            // 
            this.lblSearchJournal.Location = new System.Drawing.Point(10, 10);
            this.lblSearchJournal.Name = "lblSearchJournal";
            this.lblSearchJournal.Size = new System.Drawing.Size(60, 25);
            this.lblSearchJournal.TabIndex = 0;
            this.lblSearchJournal.Text = "Search:";
            // 
            // txtSearchJournal
            // 
            this.txtSearchJournal.Location = new System.Drawing.Point(75, 10);
            this.txtSearchJournal.Name = "txtSearchJournal";
            this.txtSearchJournal.Size = new System.Drawing.Size(300, 24);
            this.txtSearchJournal.TabIndex = 1;
            // 
            // dgvJournalEntries
            // 
            this.dgvJournalEntries.ColumnHeadersHeight = 29;
            this.dgvJournalEntries.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInvoiceNo,
            this.colEntryDate,
            this.colDescription,
            this.colDebit,
            this.colCredit,
            this.colStatus,
            this.colReconcileDate});
            this.dgvJournalEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvJournalEntries.Location = new System.Drawing.Point(0, 40);
            this.dgvJournalEntries.Name = "dgvJournalEntries";
            this.dgvJournalEntries.RowHeadersWidth = 51;
            this.dgvJournalEntries.Size = new System.Drawing.Size(1392, 561);
            this.dgvJournalEntries.TabIndex = 1;
            // 
            // colInvoiceNo
            // 
            this.colInvoiceNo.DataPropertyName = "invoice_no";
            this.colInvoiceNo.HeaderText = "Invoice No";
            this.colInvoiceNo.MinimumWidth = 6;
            this.colInvoiceNo.Name = "colInvoiceNo";
            // 
            // colEntryDate
            // 
            this.colEntryDate.DataPropertyName = "entry_date";
            this.colEntryDate.HeaderText = "Entry Date";
            this.colEntryDate.MinimumWidth = 6;
            this.colEntryDate.Name = "colEntryDate";
            // 
            // colDescription
            // 
            this.colDescription.DataPropertyName = "description";
            this.colDescription.HeaderText = "Description";
            this.colDescription.MinimumWidth = 6;
            this.colDescription.Name = "colDescription";
            this.colDescription.Width = 300;
            // 
            // colDebit
            // 
            this.colDebit.DataPropertyName = "debit";
            this.colDebit.HeaderText = "Debit";
            this.colDebit.MinimumWidth = 6;
            this.colDebit.Name = "colDebit";
            // 
            // colCredit
            // 
            this.colCredit.DataPropertyName = "credit";
            this.colCredit.HeaderText = "Credit";
            this.colCredit.MinimumWidth = 6;
            this.colCredit.Name = "colCredit";
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "is_reconciled";
            this.colStatus.HeaderText = "Status";
            this.colStatus.MinimumWidth = 6;
            this.colStatus.Name = "colStatus";
            this.colStatus.Width = 80;
            // 
            // colReconcileDate
            // 
            this.colReconcileDate.DataPropertyName = "reconcile_date";
            this.colReconcileDate.HeaderText = "Reconcile Date";
            this.colReconcileDate.MinimumWidth = 6;
            this.colReconcileDate.Name = "colReconcileDate";
            // 
            // tabSalesEntries
            // 
            this.tabSalesEntries.Controls.Add(this.dgvSalesEntries);
            this.tabSalesEntries.Controls.Add(this.panelSalesControls);
            this.tabSalesEntries.Location = new System.Drawing.Point(4, 25);
            this.tabSalesEntries.Name = "tabSalesEntries";
            this.tabSalesEntries.Size = new System.Drawing.Size(1392, 601);
            this.tabSalesEntries.TabIndex = 1;
            this.tabSalesEntries.Text = "Sales Entries";
            // 
            // panelSalesControls
            // 
            this.panelSalesControls.Controls.Add(this.lblSearchSales);
            this.panelSalesControls.Controls.Add(this.txtSearchSales);
            this.panelSalesControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSalesControls.Location = new System.Drawing.Point(0, 0);
            this.panelSalesControls.Name = "panelSalesControls";
            this.panelSalesControls.Size = new System.Drawing.Size(1392, 40);
            this.panelSalesControls.TabIndex = 0;
            // 
            // lblSearchSales
            // 
            this.lblSearchSales.Location = new System.Drawing.Point(10, 10);
            this.lblSearchSales.Name = "lblSearchSales";
            this.lblSearchSales.Size = new System.Drawing.Size(60, 25);
            this.lblSearchSales.TabIndex = 0;
            this.lblSearchSales.Text = "Search:";
            // 
            // txtSearchSales
            // 
            this.txtSearchSales.Location = new System.Drawing.Point(75, 10);
            this.txtSearchSales.Name = "txtSearchSales";
            this.txtSearchSales.Size = new System.Drawing.Size(300, 24);
            this.txtSearchSales.TabIndex = 1;
            // 
            // dgvSalesEntries
            // 
            this.dgvSalesEntries.ColumnHeadersHeight = 29;
            this.dgvSalesEntries.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSalesInvoiceNo,
            this.colSalesDate,
            this.colCustomer,
            this.colSalesAmount,
            this.colSalesStatus});
            this.dgvSalesEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSalesEntries.Location = new System.Drawing.Point(0, 40);
            this.dgvSalesEntries.Name = "dgvSalesEntries";
            this.dgvSalesEntries.RowHeadersWidth = 51;
            this.dgvSalesEntries.Size = new System.Drawing.Size(1392, 561);
            this.dgvSalesEntries.TabIndex = 1;
            // 
            // tabPurchaseEntries
            // 
            this.tabPurchaseEntries.Controls.Add(this.dgvPurchaseEntries);
            this.tabPurchaseEntries.Controls.Add(this.panelPurchaseControls);
            this.tabPurchaseEntries.Location = new System.Drawing.Point(4, 25);
            this.tabPurchaseEntries.Name = "tabPurchaseEntries";
            this.tabPurchaseEntries.Size = new System.Drawing.Size(1392, 601);
            this.tabPurchaseEntries.TabIndex = 2;
            this.tabPurchaseEntries.Text = "Purchase Entries";
            // 
            // panelPurchaseControls
            // 
            this.panelPurchaseControls.Controls.Add(this.lblSearchPurchase);
            this.panelPurchaseControls.Controls.Add(this.txtSearchPurchase);
            this.panelPurchaseControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPurchaseControls.Location = new System.Drawing.Point(0, 0);
            this.panelPurchaseControls.Name = "panelPurchaseControls";
            this.panelPurchaseControls.Size = new System.Drawing.Size(1392, 40);
            this.panelPurchaseControls.TabIndex = 0;
            // 
            // lblSearchPurchase
            // 
            this.lblSearchPurchase.Location = new System.Drawing.Point(10, 10);
            this.lblSearchPurchase.Name = "lblSearchPurchase";
            this.lblSearchPurchase.Size = new System.Drawing.Size(60, 25);
            this.lblSearchPurchase.TabIndex = 0;
            this.lblSearchPurchase.Text = "Search:";
            // 
            // txtSearchPurchase
            // 
            this.txtSearchPurchase.Location = new System.Drawing.Point(75, 10);
            this.txtSearchPurchase.Name = "txtSearchPurchase";
            this.txtSearchPurchase.Size = new System.Drawing.Size(300, 24);
            this.txtSearchPurchase.TabIndex = 1;
            // 
            // dgvPurchaseEntries
            // 
            this.dgvPurchaseEntries.ColumnHeadersHeight = 29;
            this.dgvPurchaseEntries.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPurchaseInvoiceNo,
            this.colPurchaseDate,
            this.colSupplier,
            this.colPurchaseAmount,
            this.colPurchaseStatus});
            this.dgvPurchaseEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPurchaseEntries.Location = new System.Drawing.Point(0, 40);
            this.dgvPurchaseEntries.Name = "dgvPurchaseEntries";
            this.dgvPurchaseEntries.RowHeadersWidth = 51;
            this.dgvPurchaseEntries.Size = new System.Drawing.Size(1392, 561);
            this.dgvPurchaseEntries.TabIndex = 1;
            // 
            // tabUnreconciled
            // 
            this.tabUnreconciled.Controls.Add(this.dgvUnreconciled);
            this.tabUnreconciled.Location = new System.Drawing.Point(4, 25);
            this.tabUnreconciled.Name = "tabUnreconciled";
            this.tabUnreconciled.Size = new System.Drawing.Size(1392, 601);
            this.tabUnreconciled.TabIndex = 3;
            this.tabUnreconciled.Text = "Unreconciled Entries";
            // 
            // dgvUnreconciled
            // 
            this.dgvUnreconciled.ColumnHeadersHeight = 29;
            this.dgvUnreconciled.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUnreconciledInvoice,
            this.colUnreconciledDate,
            this.colUnreconciledAmount,
            this.colUnreconciledDays});
            this.dgvUnreconciled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUnreconciled.Location = new System.Drawing.Point(0, 0);
            this.dgvUnreconciled.Name = "dgvUnreconciled";
            this.dgvUnreconciled.RowHeadersWidth = 51;
            this.dgvUnreconciled.Size = new System.Drawing.Size(1392, 601);
            this.dgvUnreconciled.TabIndex = 0;
            // 
            // colUnreconciledInvoice
            // 
            this.colUnreconciledInvoice.DataPropertyName = "invoice_no";
            this.colUnreconciledInvoice.HeaderText = "Invoice No";
            this.colUnreconciledInvoice.MinimumWidth = 6;
            this.colUnreconciledInvoice.Name = "colUnreconciledInvoice";
            // 
            // colUnreconciledDate
            // 
            this.colUnreconciledDate.DataPropertyName = "entry_date";
            this.colUnreconciledDate.HeaderText = "Date";
            this.colUnreconciledDate.MinimumWidth = 6;
            this.colUnreconciledDate.Name = "colUnreconciledDate";
            // 
            // colUnreconciledAmount
            // 
            this.colUnreconciledAmount.DataPropertyName = "amount";
            this.colUnreconciledAmount.HeaderText = "Amount";
            this.colUnreconciledAmount.MinimumWidth = 6;
            this.colUnreconciledAmount.Name = "colUnreconciledAmount";
            // 
            // colUnreconciledDays
            // 
            this.colUnreconciledDays.DataPropertyName = "days_pending";
            this.colUnreconciledDays.HeaderText = "Days Pending";
            this.colUnreconciledDays.MinimumWidth = 6;
            this.colUnreconciledDays.Name = "colUnreconciledDays";
            // 
            // panelFilters
            // 
            this.panelFilters.Controls.Add(this.lblAccountFilter);
            this.panelFilters.Controls.Add(this.cmbAccountFilter);
            this.panelFilters.Controls.Add(this.lblStatusFilter);
            this.panelFilters.Controls.Add(this.cmbStatusFilter);
            this.panelFilters.Controls.Add(this.lblFromDate);
            this.panelFilters.Controls.Add(this.dtpFromDate);
            this.panelFilters.Controls.Add(this.lblToDate);
            this.panelFilters.Controls.Add(this.dtpToDate);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(0, 0);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Size = new System.Drawing.Size(1400, 80);
            this.panelFilters.TabIndex = 3;
            // 
            // lblAccountFilter
            // 
            this.lblAccountFilter.Location = new System.Drawing.Point(10, 10);
            this.lblAccountFilter.Name = "lblAccountFilter";
            this.lblAccountFilter.Size = new System.Drawing.Size(70, 25);
            this.lblAccountFilter.TabIndex = 0;
            this.lblAccountFilter.Text = "Account:";
            // 
            // cmbAccountFilter
            // 
            this.cmbAccountFilter.Location = new System.Drawing.Point(85, 10);
            this.cmbAccountFilter.Name = "cmbAccountFilter";
            this.cmbAccountFilter.Size = new System.Drawing.Size(200, 24);
            this.cmbAccountFilter.TabIndex = 1;
            // 
            // lblStatusFilter
            // 
            this.lblStatusFilter.Location = new System.Drawing.Point(300, 10);
            this.lblStatusFilter.Name = "lblStatusFilter";
            this.lblStatusFilter.Size = new System.Drawing.Size(70, 25);
            this.lblStatusFilter.TabIndex = 2;
            this.lblStatusFilter.Text = "Status:";
            // 
            // cmbStatusFilter
            // 
            this.cmbStatusFilter.Location = new System.Drawing.Point(375, 10);
            this.cmbStatusFilter.Name = "cmbStatusFilter";
            this.cmbStatusFilter.Size = new System.Drawing.Size(150, 24);
            this.cmbStatusFilter.TabIndex = 3;
            // 
            // lblFromDate
            // 
            this.lblFromDate.Location = new System.Drawing.Point(550, 10);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(50, 25);
            this.lblFromDate.TabIndex = 4;
            this.lblFromDate.Text = "From:";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Location = new System.Drawing.Point(605, 10);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(150, 24);
            this.dtpFromDate.TabIndex = 5;
            // 
            // lblToDate
            // 
            this.lblToDate.Location = new System.Drawing.Point(770, 10);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(50, 25);
            this.lblToDate.TabIndex = 6;
            this.lblToDate.Text = "To:";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Location = new System.Drawing.Point(825, 10);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(150, 24);
            this.dtpToDate.TabIndex = 7;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.btnLoad);
            this.panelButtons.Controls.Add(this.btnReconcile);
            this.panelButtons.Controls.Add(this.btnReverseReconciliation);
            this.panelButtons.Controls.Add(this.btnAdvancedMatch);
            this.panelButtons.Controls.Add(this.btnExportReport);
            this.panelButtons.Controls.Add(this.btnRefresh);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(0, 750);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1400, 50);
            this.panelButtons.TabIndex = 2;
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnLoad.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLoad.ForeColor = System.Drawing.Color.White;
            this.btnLoad.Location = new System.Drawing.Point(10, 10);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 35);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load Data";
            this.btnLoad.UseVisualStyleBackColor = false;
            // 
            // btnReconcile
            // 
            this.btnReconcile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnReconcile.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReconcile.ForeColor = System.Drawing.Color.White;
            this.btnReconcile.Location = new System.Drawing.Point(120, 10);
            this.btnReconcile.Name = "btnReconcile";
            this.btnReconcile.Size = new System.Drawing.Size(130, 35);
            this.btnReconcile.TabIndex = 1;
            this.btnReconcile.Text = "Mark as Reconciled";
            this.btnReconcile.UseVisualStyleBackColor = false;
            // 
            // btnReverseReconciliation
            // 
            this.btnReverseReconciliation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnReverseReconciliation.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReverseReconciliation.ForeColor = System.Drawing.Color.White;
            this.btnReverseReconciliation.Location = new System.Drawing.Point(260, 10);
            this.btnReverseReconciliation.Name = "btnReverseReconciliation";
            this.btnReverseReconciliation.Size = new System.Drawing.Size(100, 35);
            this.btnReverseReconciliation.TabIndex = 2;
            this.btnReverseReconciliation.Text = "Reverse";
            this.btnReverseReconciliation.UseVisualStyleBackColor = false;
            // 
            // btnAdvancedMatch
            // 
            this.btnAdvancedMatch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnAdvancedMatch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdvancedMatch.ForeColor = System.Drawing.Color.White;
            this.btnAdvancedMatch.Location = new System.Drawing.Point(370, 10);
            this.btnAdvancedMatch.Name = "btnAdvancedMatch";
            this.btnAdvancedMatch.Size = new System.Drawing.Size(130, 35);
            this.btnAdvancedMatch.TabIndex = 3;
            this.btnAdvancedMatch.Text = "Advanced Match";
            this.btnAdvancedMatch.UseVisualStyleBackColor = false;
            // 
            // btnExportReport
            // 
            this.btnExportReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.btnExportReport.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnExportReport.ForeColor = System.Drawing.Color.White;
            this.btnExportReport.Location = new System.Drawing.Point(510, 10);
            this.btnExportReport.Name = "btnExportReport";
            this.btnExportReport.Size = new System.Drawing.Size(120, 35);
            this.btnExportReport.TabIndex = 4;
            this.btnExportReport.Text = "Export Report";
            this.btnExportReport.UseVisualStyleBackColor = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(640, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 35);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            // 
            // lblSummary
            // 
            this.lblSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSummary.Location = new System.Drawing.Point(0, 0);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblSummary.Size = new System.Drawing.Size(1400, 40);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.Text = "Summary: Loading...";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelSummary
            // 
            this.panelSummary.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.panelSummary.Controls.Add(this.lblSummary);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSummary.Location = new System.Drawing.Point(0, 710);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(1400, 40);
            this.panelSummary.TabIndex = 1;
            // 
            // colPurchaseInvoiceNo
            // 
            this.colPurchaseInvoiceNo.DataPropertyName = "invoice_no";
            this.colPurchaseInvoiceNo.HeaderText = "Invoice No";
            this.colPurchaseInvoiceNo.MinimumWidth = 6;
            this.colPurchaseInvoiceNo.Name = "colPurchaseInvoiceNo";
            // 
            // colPurchaseDate
            // 
            this.colPurchaseDate.DataPropertyName = "invoice_date";
            this.colPurchaseDate.HeaderText = "Date";
            this.colPurchaseDate.MinimumWidth = 6;
            this.colPurchaseDate.Name = "colPurchaseDate";
            // 
            // colSupplier
            // 
            this.colSupplier.DataPropertyName = "supplier_name";
            this.colSupplier.HeaderText = "Supplier";
            this.colSupplier.MinimumWidth = 6;
            this.colSupplier.Name = "colSupplier";
            this.colSupplier.Width = 250;
            // 
            // colPurchaseAmount
            // 
            this.colPurchaseAmount.DataPropertyName = "total_amount";
            this.colPurchaseAmount.HeaderText = "Amount";
            this.colPurchaseAmount.MinimumWidth = 6;
            this.colPurchaseAmount.Name = "colPurchaseAmount";
            // 
            // colPurchaseStatus
            // 
            this.colPurchaseStatus.DataPropertyName = "status";
            this.colPurchaseStatus.HeaderText = "Status";
            this.colPurchaseStatus.MinimumWidth = 6;
            this.colPurchaseStatus.Name = "colPurchaseStatus";
            this.colPurchaseStatus.Width = 80;
            // 
            // colSalesInvoiceNo
            // 
            this.colSalesInvoiceNo.DataPropertyName = "invoice_no";
            this.colSalesInvoiceNo.HeaderText = "Invoice No";
            this.colSalesInvoiceNo.MinimumWidth = 6;
            this.colSalesInvoiceNo.Name = "colSalesInvoiceNo";
            // 
            // colSalesDate
            // 
            this.colSalesDate.DataPropertyName = "invoice_date";
            this.colSalesDate.HeaderText = "Date";
            this.colSalesDate.MinimumWidth = 6;
            this.colSalesDate.Name = "colSalesDate";
            // 
            // colCustomer
            // 
            this.colCustomer.DataPropertyName = "customer_name";
            this.colCustomer.HeaderText = "Customer";
            this.colCustomer.MinimumWidth = 6;
            this.colCustomer.Name = "colCustomer";
            this.colCustomer.Width = 250;
            // 
            // colSalesAmount
            // 
            this.colSalesAmount.DataPropertyName = "total_amount";
            this.colSalesAmount.HeaderText = "Amount";
            this.colSalesAmount.MinimumWidth = 6;
            this.colSalesAmount.Name = "colSalesAmount";
            // 
            // colSalesStatus
            // 
            this.colSalesStatus.DataPropertyName = "status";
            this.colSalesStatus.HeaderText = "Status";
            this.colSalesStatus.MinimumWidth = 6;
            this.colSalesStatus.Name = "colSalesStatus";
            this.colSalesStatus.Width = 80;
            // 
            // FrmJournalReconciliation
            // 
            this.ClientSize = new System.Drawing.Size(1400, 800);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelFilters);
            this.Name = "FrmJournalReconciliation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Journal Reconciliation";
            this.tabControl1.ResumeLayout(false);
            this.tabJournalEntries.ResumeLayout(false);
            this.panelJournalControls.ResumeLayout(false);
            this.panelJournalControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournalEntries)).EndInit();
            this.tabSalesEntries.ResumeLayout(false);
            this.panelSalesControls.ResumeLayout(false);
            this.panelSalesControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSalesEntries)).EndInit();
            this.tabPurchaseEntries.ResumeLayout(false);
            this.panelPurchaseControls.ResumeLayout(false);
            this.panelPurchaseControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurchaseEntries)).EndInit();
            this.tabUnreconciled.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUnreconciled)).EndInit();
            this.panelFilters.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabJournalEntries;
        private System.Windows.Forms.TabPage tabSalesEntries;
        private System.Windows.Forms.TabPage tabPurchaseEntries;
        private System.Windows.Forms.TabPage tabUnreconciled;

        private System.Windows.Forms.DataGridView dgvJournalEntries;
        private System.Windows.Forms.DataGridView dgvSalesEntries;
        private System.Windows.Forms.DataGridView dgvPurchaseEntries;
        private System.Windows.Forms.DataGridView dgvUnreconciled;

        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEntryDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReconcileDate;

        private System.Windows.Forms.DataGridViewTextBoxColumn colUnreconciledInvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnreconciledDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnreconciledAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnreconciledDays;

        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Panel panelJournalControls;
        private System.Windows.Forms.Panel panelSalesControls;
        private System.Windows.Forms.Panel panelPurchaseControls;

        private System.Windows.Forms.Label lblAccountFilter;
        private System.Windows.Forms.Label lblStatusFilter;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.Label lblSearchJournal;
        private System.Windows.Forms.Label lblSearchSales;
        private System.Windows.Forms.Label lblSearchPurchase;
        private System.Windows.Forms.Label lblSummary;

        private System.Windows.Forms.ComboBox cmbAccountFilter;
        private System.Windows.Forms.ComboBox cmbStatusFilter;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.TextBox txtSearchJournal;
        private System.Windows.Forms.TextBox txtSearchSales;
        private System.Windows.Forms.TextBox txtSearchPurchase;

        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnReconcile;
        private System.Windows.Forms.Button btnReverseReconciliation;
        private System.Windows.Forms.Button btnAdvancedMatch;
        private System.Windows.Forms.Button btnExportReport;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalesStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchaseStatus;
    }
}
