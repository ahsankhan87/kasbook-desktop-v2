namespace pos.Expenses
{
    partial class frm_expense_tracker
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
            this.components = new System.ComponentModel.Container();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.btnFilter = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.cmbPaymentMode = new System.Windows.Forms.ComboBox();
            this.lblPaymentMode = new System.Windows.Forms.Label();
            this.cmbExpenseAccount = new System.Windows.Forms.ComboBox();
            this.lblExpenseAccount = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.panelBody = new System.Windows.Forms.Panel();
            this.gridExpenses = new System.Windows.Forms.DataGridView();
            this.ctxGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuViewDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuPrintVoucher = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuViewJournal = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.cardNetTotal = new System.Windows.Forms.Panel();
            this.lblNetTotalValue = new System.Windows.Forms.Label();
            this.lblNetTotalTitle = new System.Windows.Forms.Label();
            this.cardTotalTax = new System.Windows.Forms.Panel();
            this.lblTotalTaxValue = new System.Windows.Forms.Label();
            this.lblTotalTaxTitle = new System.Windows.Forms.Label();
            this.cardTotalExpenses = new System.Windows.Forms.Panel();
            this.lblTotalExpensesValue = new System.Windows.Forms.Label();
            this.lblTotalExpensesTitle = new System.Windows.Forms.Label();
            this.panelFilters.SuspendLayout();
            this.panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridExpenses)).BeginInit();
            this.ctxGrid.SuspendLayout();
            this.panelSummary.SuspendLayout();
            this.cardNetTotal.SuspendLayout();
            this.cardTotalTax.SuspendLayout();
            this.cardTotalExpenses.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFilters
            // 
            this.panelFilters.Controls.Add(this.btnFilter);
            this.panelFilters.Controls.Add(this.txtSearch);
            this.panelFilters.Controls.Add(this.lblSearch);
            this.panelFilters.Controls.Add(this.cmbPaymentMode);
            this.panelFilters.Controls.Add(this.lblPaymentMode);
            this.panelFilters.Controls.Add(this.cmbExpenseAccount);
            this.panelFilters.Controls.Add(this.lblExpenseAccount);
            this.panelFilters.Controls.Add(this.dtpTo);
            this.panelFilters.Controls.Add(this.lblTo);
            this.panelFilters.Controls.Add(this.dtpFrom);
            this.panelFilters.Controls.Add(this.lblFrom);
            this.panelFilters.Controls.Add(this.btnPrint);
            this.panelFilters.Controls.Add(this.btnExport);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(0, 0);
            this.panelFilters.Margin = new System.Windows.Forms.Padding(2);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Padding = new System.Windows.Forms.Padding(9, 8, 9, 6);
            this.panelFilters.Size = new System.Drawing.Size(996, 70);
            this.panelFilters.TabIndex = 0;
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(915, 10);
            this.btnFilter.Margin = new System.Windows.Forms.Padding(2);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(68, 24);
            this.btnFilter.TabIndex = 10;
            this.btnFilter.Text = "Apply";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(758, 10);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(153, 24);
            this.txtSearch.TabIndex = 9;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.Location = new System.Drawing.Point(685, 10);
            this.lblSearch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(56, 24);
            this.lblSearch.TabIndex = 8;
            this.lblSearch.Text = "Search";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbPaymentMode
            // 
            this.cmbPaymentMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMode.FormattingEnabled = true;
            this.cmbPaymentMode.Location = new System.Drawing.Point(559, 11);
            this.cmbPaymentMode.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPaymentMode.Name = "cmbPaymentMode";
            this.cmbPaymentMode.Size = new System.Drawing.Size(120, 24);
            this.cmbPaymentMode.TabIndex = 7;
            // 
            // lblPaymentMode
            // 
            this.lblPaymentMode.Location = new System.Drawing.Point(491, 10);
            this.lblPaymentMode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPaymentMode.Name = "lblPaymentMode";
            this.lblPaymentMode.Size = new System.Drawing.Size(64, 24);
            this.lblPaymentMode.TabIndex = 6;
            this.lblPaymentMode.Text = "Payment Mode";
            this.lblPaymentMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbExpenseAccount
            // 
            this.cmbExpenseAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExpenseAccount.FormattingEnabled = true;
            this.cmbExpenseAccount.Location = new System.Drawing.Point(343, 11);
            this.cmbExpenseAccount.Margin = new System.Windows.Forms.Padding(2);
            this.cmbExpenseAccount.Name = "cmbExpenseAccount";
            this.cmbExpenseAccount.Size = new System.Drawing.Size(144, 24);
            this.cmbExpenseAccount.TabIndex = 5;
            // 
            // lblExpenseAccount
            // 
            this.lblExpenseAccount.Location = new System.Drawing.Point(275, 10);
            this.lblExpenseAccount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblExpenseAccount.Name = "lblExpenseAccount";
            this.lblExpenseAccount.Size = new System.Drawing.Size(63, 24);
            this.lblExpenseAccount.TabIndex = 4;
            this.lblExpenseAccount.Text = "Expense Account";
            this.lblExpenseAccount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(180, 8);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(2);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(91, 24);
            this.dtpTo.TabIndex = 3;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(145, 10);
            this.lblTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(31, 24);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "To";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(51, 8);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(2);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(90, 24);
            this.dtpFrom.TabIndex = 1;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(2, 10);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(45, 24);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "From";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(817, 38);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(2);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(68, 24);
            this.btnPrint.TabIndex = 12;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(891, 38);
            this.btnExport.Margin = new System.Windows.Forms.Padding(2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(93, 24);
            this.btnExport.TabIndex = 11;
            this.btnExport.Text = "Export Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.gridExpenses);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 70);
            this.panelBody.Margin = new System.Windows.Forms.Padding(2);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(9, 0, 9, 0);
            this.panelBody.Size = new System.Drawing.Size(996, 421);
            this.panelBody.TabIndex = 1;
            // 
            // gridExpenses
            // 
            this.gridExpenses.AllowUserToAddRows = false;
            this.gridExpenses.AllowUserToDeleteRows = false;
            this.gridExpenses.BackgroundColor = System.Drawing.Color.White;
            this.gridExpenses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridExpenses.ContextMenuStrip = this.ctxGrid;
            this.gridExpenses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridExpenses.Location = new System.Drawing.Point(9, 0);
            this.gridExpenses.Margin = new System.Windows.Forms.Padding(2);
            this.gridExpenses.MultiSelect = false;
            this.gridExpenses.Name = "gridExpenses";
            this.gridExpenses.ReadOnly = true;
            this.gridExpenses.RowHeadersVisible = false;
            this.gridExpenses.RowHeadersWidth = 51;
            this.gridExpenses.RowTemplate.Height = 28;
            this.gridExpenses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridExpenses.Size = new System.Drawing.Size(978, 421);
            this.gridExpenses.TabIndex = 0;
            this.gridExpenses.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridExpenses_CellDoubleClick);
            this.gridExpenses.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridExpenses_MouseDown);
            // 
            // ctxGrid
            // 
            this.ctxGrid.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ctxGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewDetails,
            this.mnuEdit,
            this.mnuDelete,
            this.toolStripSeparator1,
            this.mnuPrintVoucher,
            this.mnuViewJournal});
            this.ctxGrid.Name = "ctxGrid";
            this.ctxGrid.Size = new System.Drawing.Size(199, 130);
            // 
            // mnuViewDetails
            // 
            this.mnuViewDetails.Name = "mnuViewDetails";
            this.mnuViewDetails.Size = new System.Drawing.Size(198, 24);
            this.mnuViewDetails.Text = "View Details";
            this.mnuViewDetails.Click += new System.EventHandler(this.mnuViewDetails_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(198, 24);
            this.mnuEdit.Text = "Edit";
            this.mnuEdit.Click += new System.EventHandler(this.mnuEdit_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(198, 24);
            this.mnuDelete.Text = "Delete";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(195, 6);
            // 
            // mnuPrintVoucher
            // 
            this.mnuPrintVoucher.Name = "mnuPrintVoucher";
            this.mnuPrintVoucher.Size = new System.Drawing.Size(198, 24);
            this.mnuPrintVoucher.Text = "Print Voucher";
            this.mnuPrintVoucher.Click += new System.EventHandler(this.mnuPrintVoucher_Click);
            // 
            // mnuViewJournal
            // 
            this.mnuViewJournal.Name = "mnuViewJournal";
            this.mnuViewJournal.Size = new System.Drawing.Size(198, 24);
            this.mnuViewJournal.Text = "View Journal Entry";
            this.mnuViewJournal.Click += new System.EventHandler(this.mnuViewJournal_Click);
            // 
            // panelSummary
            // 
            this.panelSummary.Controls.Add(this.cardNetTotal);
            this.panelSummary.Controls.Add(this.cardTotalTax);
            this.panelSummary.Controls.Add(this.cardTotalExpenses);
            this.panelSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSummary.Location = new System.Drawing.Point(0, 491);
            this.panelSummary.Margin = new System.Windows.Forms.Padding(2);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Padding = new System.Windows.Forms.Padding(9, 6, 9, 8);
            this.panelSummary.Size = new System.Drawing.Size(996, 86);
            this.panelSummary.TabIndex = 2;
            // 
            // cardNetTotal
            // 
            this.cardNetTotal.Controls.Add(this.lblNetTotalValue);
            this.cardNetTotal.Controls.Add(this.lblNetTotalTitle);
            this.cardNetTotal.Location = new System.Drawing.Point(670, 10);
            this.cardNetTotal.Margin = new System.Windows.Forms.Padding(2);
            this.cardNetTotal.Name = "cardNetTotal";
            this.cardNetTotal.Size = new System.Drawing.Size(313, 67);
            this.cardNetTotal.TabIndex = 2;
            // 
            // lblNetTotalValue
            // 
            this.lblNetTotalValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNetTotalValue.Location = new System.Drawing.Point(0, 24);
            this.lblNetTotalValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNetTotalValue.Name = "lblNetTotalValue";
            this.lblNetTotalValue.Size = new System.Drawing.Size(313, 43);
            this.lblNetTotalValue.TabIndex = 1;
            this.lblNetTotalValue.Text = "0.00";
            this.lblNetTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNetTotalTitle
            // 
            this.lblNetTotalTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNetTotalTitle.Location = new System.Drawing.Point(0, 0);
            this.lblNetTotalTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNetTotalTitle.Name = "lblNetTotalTitle";
            this.lblNetTotalTitle.Size = new System.Drawing.Size(313, 24);
            this.lblNetTotalTitle.TabIndex = 0;
            this.lblNetTotalTitle.Text = "Net Total";
            this.lblNetTotalTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardTotalTax
            // 
            this.cardTotalTax.Controls.Add(this.lblTotalTaxValue);
            this.cardTotalTax.Controls.Add(this.lblTotalTaxTitle);
            this.cardTotalTax.Location = new System.Drawing.Point(340, 10);
            this.cardTotalTax.Margin = new System.Windows.Forms.Padding(2);
            this.cardTotalTax.Name = "cardTotalTax";
            this.cardTotalTax.Size = new System.Drawing.Size(313, 67);
            this.cardTotalTax.TabIndex = 1;
            // 
            // lblTotalTaxValue
            // 
            this.lblTotalTaxValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalTaxValue.Location = new System.Drawing.Point(0, 24);
            this.lblTotalTaxValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalTaxValue.Name = "lblTotalTaxValue";
            this.lblTotalTaxValue.Size = new System.Drawing.Size(313, 43);
            this.lblTotalTaxValue.TabIndex = 1;
            this.lblTotalTaxValue.Text = "0.00";
            this.lblTotalTaxValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotalTaxTitle
            // 
            this.lblTotalTaxTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalTaxTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTotalTaxTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalTaxTitle.Name = "lblTotalTaxTitle";
            this.lblTotalTaxTitle.Size = new System.Drawing.Size(313, 24);
            this.lblTotalTaxTitle.TabIndex = 0;
            this.lblTotalTaxTitle.Text = "Total Tax";
            this.lblTotalTaxTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cardTotalExpenses
            // 
            this.cardTotalExpenses.Controls.Add(this.lblTotalExpensesValue);
            this.cardTotalExpenses.Controls.Add(this.lblTotalExpensesTitle);
            this.cardTotalExpenses.Location = new System.Drawing.Point(9, 10);
            this.cardTotalExpenses.Margin = new System.Windows.Forms.Padding(2);
            this.cardTotalExpenses.Name = "cardTotalExpenses";
            this.cardTotalExpenses.Size = new System.Drawing.Size(313, 67);
            this.cardTotalExpenses.TabIndex = 0;
            // 
            // lblTotalExpensesValue
            // 
            this.lblTotalExpensesValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalExpensesValue.Location = new System.Drawing.Point(0, 24);
            this.lblTotalExpensesValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalExpensesValue.Name = "lblTotalExpensesValue";
            this.lblTotalExpensesValue.Size = new System.Drawing.Size(313, 43);
            this.lblTotalExpensesValue.TabIndex = 1;
            this.lblTotalExpensesValue.Text = "0.00";
            this.lblTotalExpensesValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotalExpensesTitle
            // 
            this.lblTotalExpensesTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalExpensesTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTotalExpensesTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalExpensesTitle.Name = "lblTotalExpensesTitle";
            this.lblTotalExpensesTitle.Size = new System.Drawing.Size(313, 24);
            this.lblTotalExpensesTitle.TabIndex = 0;
            this.lblTotalExpensesTitle.Text = "Total Expenses";
            this.lblTotalExpensesTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frm_expense_tracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 577);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.panelFilters);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frm_expense_tracker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Expense Tracker";
            this.Load += new System.EventHandler(this.frm_expense_tracker_Load);
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.panelBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridExpenses)).EndInit();
            this.ctxGrid.ResumeLayout(false);
            this.panelSummary.ResumeLayout(false);
            this.cardNetTotal.ResumeLayout(false);
            this.cardTotalTax.ResumeLayout(false);
            this.cardTotalExpenses.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.ComboBox cmbExpenseAccount;
        private System.Windows.Forms.Label lblExpenseAccount;
        private System.Windows.Forms.ComboBox cmbPaymentMode;
        private System.Windows.Forms.Label lblPaymentMode;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.DataGridView gridExpenses;
        private System.Windows.Forms.Panel panelSummary;
        private System.Windows.Forms.Panel cardTotalExpenses;
        private System.Windows.Forms.Label lblTotalExpensesValue;
        private System.Windows.Forms.Label lblTotalExpensesTitle;
        private System.Windows.Forms.Panel cardTotalTax;
        private System.Windows.Forms.Label lblTotalTaxValue;
        private System.Windows.Forms.Label lblTotalTaxTitle;
        private System.Windows.Forms.Panel cardNetTotal;
        private System.Windows.Forms.Label lblNetTotalValue;
        private System.Windows.Forms.Label lblNetTotalTitle;
        private System.Windows.Forms.ContextMenuStrip ctxGrid;
        private System.Windows.Forms.ToolStripMenuItem mnuViewDetails;
        private System.Windows.Forms.ToolStripMenuItem mnuEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuPrintVoucher;
        private System.Windows.Forms.ToolStripMenuItem mnuViewJournal;
    }
}
