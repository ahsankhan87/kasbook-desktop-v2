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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_expense_tracker));
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
            resources.ApplyResources(this.panelFilters, "panelFilters");
            this.panelFilters.Name = "panelFilters";
            // 
            // btnFilter
            // 
            resources.ApplyResources(this.btnFilter, "btnFilter");
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // txtSearch
            // 
            resources.ApplyResources(this.txtSearch, "txtSearch");
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            resources.ApplyResources(this.lblSearch, "lblSearch");
            this.lblSearch.Name = "lblSearch";
            // 
            // cmbPaymentMode
            // 
            this.cmbPaymentMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMode.FormattingEnabled = true;
            resources.ApplyResources(this.cmbPaymentMode, "cmbPaymentMode");
            this.cmbPaymentMode.Name = "cmbPaymentMode";
            // 
            // lblPaymentMode
            // 
            resources.ApplyResources(this.lblPaymentMode, "lblPaymentMode");
            this.lblPaymentMode.Name = "lblPaymentMode";
            // 
            // cmbExpenseAccount
            // 
            this.cmbExpenseAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExpenseAccount.FormattingEnabled = true;
            resources.ApplyResources(this.cmbExpenseAccount, "cmbExpenseAccount");
            this.cmbExpenseAccount.Name = "cmbExpenseAccount";
            // 
            // lblExpenseAccount
            // 
            resources.ApplyResources(this.lblExpenseAccount, "lblExpenseAccount");
            this.lblExpenseAccount.Name = "lblExpenseAccount";
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpTo, "dtpTo");
            this.dtpTo.Name = "dtpTo";
            // 
            // lblTo
            // 
            resources.ApplyResources(this.lblTo, "lblTo");
            this.lblTo.Name = "lblTo";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpFrom, "dtpFrom");
            this.dtpFrom.Name = "dtpFrom";
            // 
            // lblFrom
            // 
            resources.ApplyResources(this.lblFrom, "lblFrom");
            this.lblFrom.Name = "lblFrom";
            // 
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // panelBody
            // 
            this.panelBody.Controls.Add(this.gridExpenses);
            resources.ApplyResources(this.panelBody, "panelBody");
            this.panelBody.Name = "panelBody";
            // 
            // gridExpenses
            // 
            this.gridExpenses.AllowUserToAddRows = false;
            this.gridExpenses.AllowUserToDeleteRows = false;
            this.gridExpenses.BackgroundColor = System.Drawing.Color.White;
            this.gridExpenses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridExpenses.ContextMenuStrip = this.ctxGrid;
            resources.ApplyResources(this.gridExpenses, "gridExpenses");
            this.gridExpenses.MultiSelect = false;
            this.gridExpenses.Name = "gridExpenses";
            this.gridExpenses.ReadOnly = true;
            this.gridExpenses.RowHeadersVisible = false;
            this.gridExpenses.RowTemplate.Height = 28;
            this.gridExpenses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
            resources.ApplyResources(this.ctxGrid, "ctxGrid");
            // 
            // mnuViewDetails
            // 
            this.mnuViewDetails.Name = "mnuViewDetails";
            resources.ApplyResources(this.mnuViewDetails, "mnuViewDetails");
            this.mnuViewDetails.Click += new System.EventHandler(this.mnuViewDetails_Click);
            // 
            // mnuEdit
            // 
            this.mnuEdit.Name = "mnuEdit";
            resources.ApplyResources(this.mnuEdit, "mnuEdit");
            this.mnuEdit.Click += new System.EventHandler(this.mnuEdit_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            resources.ApplyResources(this.mnuDelete, "mnuDelete");
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // mnuPrintVoucher
            // 
            this.mnuPrintVoucher.Name = "mnuPrintVoucher";
            resources.ApplyResources(this.mnuPrintVoucher, "mnuPrintVoucher");
            this.mnuPrintVoucher.Click += new System.EventHandler(this.mnuPrintVoucher_Click);
            // 
            // mnuViewJournal
            // 
            this.mnuViewJournal.Name = "mnuViewJournal";
            resources.ApplyResources(this.mnuViewJournal, "mnuViewJournal");
            this.mnuViewJournal.Click += new System.EventHandler(this.mnuViewJournal_Click);
            // 
            // panelSummary
            // 
            this.panelSummary.Controls.Add(this.cardNetTotal);
            this.panelSummary.Controls.Add(this.cardTotalTax);
            this.panelSummary.Controls.Add(this.cardTotalExpenses);
            resources.ApplyResources(this.panelSummary, "panelSummary");
            this.panelSummary.Name = "panelSummary";
            // 
            // cardNetTotal
            // 
            this.cardNetTotal.Controls.Add(this.lblNetTotalValue);
            this.cardNetTotal.Controls.Add(this.lblNetTotalTitle);
            resources.ApplyResources(this.cardNetTotal, "cardNetTotal");
            this.cardNetTotal.Name = "cardNetTotal";
            // 
            // lblNetTotalValue
            // 
            resources.ApplyResources(this.lblNetTotalValue, "lblNetTotalValue");
            this.lblNetTotalValue.Name = "lblNetTotalValue";
            // 
            // lblNetTotalTitle
            // 
            resources.ApplyResources(this.lblNetTotalTitle, "lblNetTotalTitle");
            this.lblNetTotalTitle.Name = "lblNetTotalTitle";
            // 
            // cardTotalTax
            // 
            this.cardTotalTax.Controls.Add(this.lblTotalTaxValue);
            this.cardTotalTax.Controls.Add(this.lblTotalTaxTitle);
            resources.ApplyResources(this.cardTotalTax, "cardTotalTax");
            this.cardTotalTax.Name = "cardTotalTax";
            // 
            // lblTotalTaxValue
            // 
            resources.ApplyResources(this.lblTotalTaxValue, "lblTotalTaxValue");
            this.lblTotalTaxValue.Name = "lblTotalTaxValue";
            // 
            // lblTotalTaxTitle
            // 
            resources.ApplyResources(this.lblTotalTaxTitle, "lblTotalTaxTitle");
            this.lblTotalTaxTitle.Name = "lblTotalTaxTitle";
            // 
            // cardTotalExpenses
            // 
            this.cardTotalExpenses.Controls.Add(this.lblTotalExpensesValue);
            this.cardTotalExpenses.Controls.Add(this.lblTotalExpensesTitle);
            resources.ApplyResources(this.cardTotalExpenses, "cardTotalExpenses");
            this.cardTotalExpenses.Name = "cardTotalExpenses";
            // 
            // lblTotalExpensesValue
            // 
            resources.ApplyResources(this.lblTotalExpensesValue, "lblTotalExpensesValue");
            this.lblTotalExpensesValue.Name = "lblTotalExpensesValue";
            // 
            // lblTotalExpensesTitle
            // 
            resources.ApplyResources(this.lblTotalExpensesTitle, "lblTotalExpensesTitle");
            this.lblTotalExpensesTitle.Name = "lblTotalExpensesTitle";
            // 
            // frm_expense_tracker
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.panelFilters);
            this.Name = "frm_expense_tracker";
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
