namespace pos
{
    partial class frm_customers
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.tableKpis = new System.Windows.Forms.TableLayoutPanel();
            this.cardTotalCustomers = new System.Windows.Forms.Panel();
            this.lblTotalCustomersValue = new System.Windows.Forms.Label();
            this.lblTotalCustomersTitle = new System.Windows.Forms.Label();
            this.cardReceivables = new System.Windows.Forms.Panel();
            this.lblReceivablesValue = new System.Windows.Forms.Label();
            this.lblReceivablesTitle = new System.Windows.Forms.Label();
            this.cardOverdue = new System.Windows.Forms.Panel();
            this.lblOverdueValue = new System.Windows.Forms.Label();
            this.lblOverdueTitle = new System.Windows.Forms.Label();
            this.cardNewCustomers = new System.Windows.Forms.Panel();
            this.lblNewCustomersValue = new System.Windows.Forms.Label();
            this.lblNewCustomersTitle = new System.Windows.Forms.Label();
            this.lblNewCustomersTrend = new System.Windows.Forms.Label();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.lblFilterStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblFilterArea = new System.Windows.Forms.Label();
            this.cmbArea = new System.Windows.Forms.ComboBox();
            this.lblFilterSearch = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.grid_customers = new System.Windows.Forms.DataGridView();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalSales = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutstanding = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreditLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreditUsed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastTransaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOverdue30 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOverdue1530 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaidAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreditAvailable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.btnReceivePayment = new System.Windows.Forms.Button();
            this.btnNewInvoice = new System.Windows.Forms.Button();
            this.btnViewProfile = new System.Windows.Forms.Button();
            this.lstTransactions = new System.Windows.Forms.ListBox();
            this.lblTransactions = new System.Windows.Forms.Label();
            this.lblCreditAvailableValue = new System.Windows.Forms.Label();
            this.lblCreditAvailableTitle = new System.Windows.Forms.Label();
            this.lblCreditUsedValue = new System.Windows.Forms.Label();
            this.lblCreditUsedTitle = new System.Windows.Forms.Label();
            this.lblCreditLimitValue = new System.Windows.Forms.Label();
            this.lblCreditLimitTitle = new System.Windows.Forms.Label();
            this.lblOutstandingValue = new System.Windows.Forms.Label();
            this.lblOutstandingTitle = new System.Windows.Forms.Label();
            this.lblPaidValue = new System.Windows.Forms.Label();
            this.lblPaidTitle = new System.Windows.Forms.Label();
            this.lblTotalSalesDetailValue = new System.Windows.Forms.Label();
            this.lblTotalSalesDetailTitle = new System.Windows.Forms.Label();
            this.lblAreaValue = new System.Windows.Forms.Label();
            this.lblAddressValue = new System.Windows.Forms.Label();
            this.lblPhoneValue = new System.Windows.Forms.Label();
            this.panelAvatar = new System.Windows.Forms.Panel();
            this.lblCustomerHeader = new System.Windows.Forms.Label();
            this.detailSlideTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlTop.SuspendLayout();
            this.tableKpis.SuspendLayout();
            this.cardTotalCustomers.SuspendLayout();
            this.cardReceivables.SuspendLayout();
            this.cardOverdue.SuspendLayout();
            this.cardNewCustomers.SuspendLayout();
            this.pnlFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_customers)).BeginInit();
            this.pnlDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.tableKpis);
            this.pnlTop.Controls.Add(this.pnlFilters);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(16, 12, 16, 10);
            this.pnlTop.Size = new System.Drawing.Size(1384, 210);
            this.pnlTop.TabIndex = 0;
            // 
            // tableKpis
            // 
            this.tableKpis.ColumnCount = 4;
            this.tableKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableKpis.Controls.Add(this.cardTotalCustomers, 0, 0);
            this.tableKpis.Controls.Add(this.cardReceivables, 1, 0);
            this.tableKpis.Controls.Add(this.cardOverdue, 2, 0);
            this.tableKpis.Controls.Add(this.cardNewCustomers, 3, 0);
            this.tableKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableKpis.Location = new System.Drawing.Point(16, 43);
            this.tableKpis.Name = "tableKpis";
            this.tableKpis.RowCount = 1;
            this.tableKpis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableKpis.Size = new System.Drawing.Size(1352, 92);
            this.tableKpis.TabIndex = 1;
            // 
            // cardTotalCustomers
            // 
            this.cardTotalCustomers.BackColor = System.Drawing.Color.White;
            this.cardTotalCustomers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTotalCustomers.Controls.Add(this.lblTotalCustomersValue);
            this.cardTotalCustomers.Controls.Add(this.lblTotalCustomersTitle);
            this.cardTotalCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTotalCustomers.Location = new System.Drawing.Point(3, 3);
            this.cardTotalCustomers.Name = "cardTotalCustomers";
            this.cardTotalCustomers.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.cardTotalCustomers.Size = new System.Drawing.Size(332, 86);
            this.cardTotalCustomers.TabIndex = 0;
            // 
            // lblTotalCustomersValue
            // 
            this.lblTotalCustomersValue.AutoSize = true;
            this.lblTotalCustomersValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTotalCustomersValue.Location = new System.Drawing.Point(14, 33);
            this.lblTotalCustomersValue.Name = "lblTotalCustomersValue";
            this.lblTotalCustomersValue.Size = new System.Drawing.Size(42, 41);
            this.lblTotalCustomersValue.TabIndex = 1;
            this.lblTotalCustomersValue.Text = "0";
            // 
            // lblTotalCustomersTitle
            // 
            this.lblTotalCustomersTitle.AutoSize = true;
            this.lblTotalCustomersTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotalCustomersTitle.Location = new System.Drawing.Point(16, 10);
            this.lblTotalCustomersTitle.Name = "lblTotalCustomersTitle";
            this.lblTotalCustomersTitle.Size = new System.Drawing.Size(104, 20);
            this.lblTotalCustomersTitle.TabIndex = 0;
            this.lblTotalCustomersTitle.Text = "Total Customers";
            // 
            // cardReceivables
            // 
            this.cardReceivables.BackColor = System.Drawing.Color.White;
            this.cardReceivables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardReceivables.Controls.Add(this.lblReceivablesValue);
            this.cardReceivables.Controls.Add(this.lblReceivablesTitle);
            this.cardReceivables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardReceivables.Location = new System.Drawing.Point(341, 3);
            this.cardReceivables.Name = "cardReceivables";
            this.cardReceivables.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.cardReceivables.Size = new System.Drawing.Size(332, 86);
            this.cardReceivables.TabIndex = 1;
            // 
            // lblReceivablesValue
            // 
            this.lblReceivablesValue.AutoSize = true;
            this.lblReceivablesValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblReceivablesValue.Location = new System.Drawing.Point(14, 33);
            this.lblReceivablesValue.Name = "lblReceivablesValue";
            this.lblReceivablesValue.Size = new System.Drawing.Size(81, 41);
            this.lblReceivablesValue.TabIndex = 1;
            this.lblReceivablesValue.Text = "0.00";
            // 
            // lblReceivablesTitle
            // 
            this.lblReceivablesTitle.AutoSize = true;
            this.lblReceivablesTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblReceivablesTitle.Location = new System.Drawing.Point(16, 10);
            this.lblReceivablesTitle.Name = "lblReceivablesTitle";
            this.lblReceivablesTitle.Size = new System.Drawing.Size(114, 20);
            this.lblReceivablesTitle.TabIndex = 0;
            this.lblReceivablesTitle.Text = "Total Receivables";
            // 
            // cardOverdue
            // 
            this.cardOverdue.BackColor = System.Drawing.Color.White;
            this.cardOverdue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardOverdue.Controls.Add(this.lblOverdueValue);
            this.cardOverdue.Controls.Add(this.lblOverdueTitle);
            this.cardOverdue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardOverdue.Location = new System.Drawing.Point(679, 3);
            this.cardOverdue.Name = "cardOverdue";
            this.cardOverdue.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.cardOverdue.Size = new System.Drawing.Size(332, 86);
            this.cardOverdue.TabIndex = 2;
            // 
            // lblOverdueValue
            // 
            this.lblOverdueValue.AutoSize = true;
            this.lblOverdueValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblOverdueValue.ForeColor = System.Drawing.Color.Firebrick;
            this.lblOverdueValue.Location = new System.Drawing.Point(14, 33);
            this.lblOverdueValue.Name = "lblOverdueValue";
            this.lblOverdueValue.Size = new System.Drawing.Size(81, 41);
            this.lblOverdueValue.TabIndex = 1;
            this.lblOverdueValue.Text = "0.00";
            // 
            // lblOverdueTitle
            // 
            this.lblOverdueTitle.AutoSize = true;
            this.lblOverdueTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOverdueTitle.Location = new System.Drawing.Point(16, 10);
            this.lblOverdueTitle.Name = "lblOverdueTitle";
            this.lblOverdueTitle.Size = new System.Drawing.Size(124, 20);
            this.lblOverdueTitle.TabIndex = 0;
            this.lblOverdueTitle.Text = "Overdue (>30 days)";
            // 
            // cardNewCustomers
            // 
            this.cardNewCustomers.BackColor = System.Drawing.Color.White;
            this.cardNewCustomers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardNewCustomers.Controls.Add(this.lblNewCustomersTrend);
            this.cardNewCustomers.Controls.Add(this.lblNewCustomersValue);
            this.cardNewCustomers.Controls.Add(this.lblNewCustomersTitle);
            this.cardNewCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardNewCustomers.Location = new System.Drawing.Point(1017, 3);
            this.cardNewCustomers.Name = "cardNewCustomers";
            this.cardNewCustomers.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.cardNewCustomers.Size = new System.Drawing.Size(332, 86);
            this.cardNewCustomers.TabIndex = 3;
            // 
            // lblNewCustomersValue
            // 
            this.lblNewCustomersValue.AutoSize = true;
            this.lblNewCustomersValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblNewCustomersValue.Location = new System.Drawing.Point(14, 33);
            this.lblNewCustomersValue.Name = "lblNewCustomersValue";
            this.lblNewCustomersValue.Size = new System.Drawing.Size(42, 41);
            this.lblNewCustomersValue.TabIndex = 1;
            this.lblNewCustomersValue.Text = "0";
            // 
            // lblNewCustomersTitle
            // 
            this.lblNewCustomersTitle.AutoSize = true;
            this.lblNewCustomersTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNewCustomersTitle.Location = new System.Drawing.Point(16, 10);
            this.lblNewCustomersTitle.Name = "lblNewCustomersTitle";
            this.lblNewCustomersTitle.Size = new System.Drawing.Size(169, 20);
            this.lblNewCustomersTitle.TabIndex = 0;
            this.lblNewCustomersTitle.Text = "New Customers This Month";
            // 
            // lblNewCustomersTrend
            // 
            this.lblNewCustomersTrend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNewCustomersTrend.AutoSize = true;
            this.lblNewCustomersTrend.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNewCustomersTrend.Location = new System.Drawing.Point(244, 11);
            this.lblNewCustomersTrend.Name = "lblNewCustomersTrend";
            this.lblNewCustomersTrend.Size = new System.Drawing.Size(73, 20);
            this.lblNewCustomersTrend.TabIndex = 2;
            this.lblNewCustomersTrend.Text = "▲ 0 vs PM";
            // 
            // pnlFilters
            // 
            this.pnlFilters.Controls.Add(this.lblFilterStatus);
            this.pnlFilters.Controls.Add(this.cmbStatus);
            this.pnlFilters.Controls.Add(this.lblFilterArea);
            this.pnlFilters.Controls.Add(this.cmbArea);
            this.pnlFilters.Controls.Add(this.lblFilterSearch);
            this.pnlFilters.Controls.Add(this.txt_search);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFilters.Location = new System.Drawing.Point(16, 142);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(1352, 58);
            this.pnlFilters.TabIndex = 2;
            // 
            // lblFilterStatus
            // 
            this.lblFilterStatus.AutoSize = true;
            this.lblFilterStatus.Location = new System.Drawing.Point(938, 20);
            this.lblFilterStatus.Name = "lblFilterStatus";
            this.lblFilterStatus.Size = new System.Drawing.Size(46, 16);
            this.lblFilterStatus.TabIndex = 5;
            this.lblFilterStatus.Text = "Status";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(993, 16);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(180, 24);
            this.cmbStatus.TabIndex = 4;
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_SelectedIndexChanged);
            // 
            // lblFilterArea
            // 
            this.lblFilterArea.AutoSize = true;
            this.lblFilterArea.Location = new System.Drawing.Point(672, 20);
            this.lblFilterArea.Name = "lblFilterArea";
            this.lblFilterArea.Size = new System.Drawing.Size(66, 16);
            this.lblFilterArea.TabIndex = 3;
            this.lblFilterArea.Text = "Area/Zone";
            // 
            // cmbArea
            // 
            this.cmbArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbArea.FormattingEnabled = true;
            this.cmbArea.Location = new System.Drawing.Point(747, 16);
            this.cmbArea.Name = "cmbArea";
            this.cmbArea.Size = new System.Drawing.Size(180, 24);
            this.cmbArea.TabIndex = 2;
            this.cmbArea.SelectedIndexChanged += new System.EventHandler(this.cmbArea_SelectedIndexChanged);
            // 
            // lblFilterSearch
            // 
            this.lblFilterSearch.AutoSize = true;
            this.lblFilterSearch.Location = new System.Drawing.Point(14, 20);
            this.lblFilterSearch.Name = "lblFilterSearch";
            this.lblFilterSearch.Size = new System.Drawing.Size(46, 16);
            this.lblFilterSearch.TabIndex = 1;
            this.lblFilterSearch.Text = "Search";
            // 
            // txt_search
            // 
            this.txt_search.Location = new System.Drawing.Point(68, 16);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(595, 22);
            this.txt_search.TabIndex = 0;
            this.txt_search.TextChanged += new System.EventHandler(this.txt_search_TextChanged);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(16, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(280, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Customer Summary Dashboard";
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitMain.Location = new System.Drawing.Point(0, 210);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.grid_customers);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.pnlDetails);
            this.splitMain.Size = new System.Drawing.Size(1384, 551);
            this.splitMain.SplitterDistance = 1100;
            this.splitMain.TabIndex = 1;
            // 
            // grid_customers
            // 
            this.grid_customers.AllowUserToAddRows = false;
            this.grid_customers.AllowUserToDeleteRows = false;
            this.grid_customers.AllowUserToOrderColumns = true;
            this.grid_customers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_customers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_customers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo,
            this.colCustomerCode,
            this.colCustomerName,
            this.colArea,
            this.colPhone,
            this.colTotalSales,
            this.colOutstanding,
            this.colCreditLimit,
            this.colCreditUsed,
            this.colLastTransaction,
            this.colStatus,
            this.colId,
            this.colFirstName,
            this.colLastName,
            this.colAddress,
            this.colOverdue30,
            this.colOverdue1530,
            this.colPaidAmount,
            this.colCreditAvailable});
            this.grid_customers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_customers.Location = new System.Drawing.Point(0, 0);
            this.grid_customers.MultiSelect = false;
            this.grid_customers.Name = "grid_customers";
            this.grid_customers.ReadOnly = true;
            this.grid_customers.RowHeadersVisible = false;
            this.grid_customers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_customers.Size = new System.Drawing.Size(1100, 551);
            this.grid_customers.TabIndex = 0;
            this.grid_customers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_customers_CellDoubleClick);
            this.grid_customers.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid_customers_CellFormatting);
            this.grid_customers.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grid_customers_CellPainting);
            this.grid_customers.SelectionChanged += new System.EventHandler(this.grid_customers_SelectionChanged);
            // 
            // colNo
            // 
            this.colNo.DataPropertyName = "row_no";
            this.colNo.FillWeight = 40F;
            this.colNo.HeaderText = "#";
            this.colNo.Name = "colNo";
            this.colNo.ReadOnly = true;
            // 
            // colCustomerCode
            // 
            this.colCustomerCode.DataPropertyName = "customer_code";
            this.colCustomerCode.FillWeight = 80F;
            this.colCustomerCode.HeaderText = "Customer Code";
            this.colCustomerCode.Name = "colCustomerCode";
            this.colCustomerCode.ReadOnly = true;
            // 
            // colCustomerName
            // 
            this.colCustomerName.DataPropertyName = "customer_name";
            this.colCustomerName.FillWeight = 150F;
            this.colCustomerName.HeaderText = "Customer Name";
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.ReadOnly = true;
            // 
            // colArea
            // 
            this.colArea.DataPropertyName = "area";
            this.colArea.FillWeight = 90F;
            this.colArea.HeaderText = "Area";
            this.colArea.Name = "colArea";
            this.colArea.ReadOnly = true;
            // 
            // colPhone
            // 
            this.colPhone.DataPropertyName = "contact_no";
            this.colPhone.FillWeight = 90F;
            this.colPhone.HeaderText = "Phone";
            this.colPhone.Name = "colPhone";
            this.colPhone.ReadOnly = true;
            // 
            // colTotalSales
            // 
            this.colTotalSales.DataPropertyName = "total_sales";
            this.colTotalSales.FillWeight = 95F;
            this.colTotalSales.HeaderText = "Total Sales";
            this.colTotalSales.Name = "colTotalSales";
            this.colTotalSales.ReadOnly = true;
            // 
            // colOutstanding
            // 
            this.colOutstanding.DataPropertyName = "outstanding_balance";
            this.colOutstanding.FillWeight = 95F;
            this.colOutstanding.HeaderText = "Outstanding Balance";
            this.colOutstanding.Name = "colOutstanding";
            this.colOutstanding.ReadOnly = true;
            // 
            // colCreditLimit
            // 
            this.colCreditLimit.DataPropertyName = "credit_limit";
            this.colCreditLimit.FillWeight = 90F;
            this.colCreditLimit.HeaderText = "Credit Limit";
            this.colCreditLimit.Name = "colCreditLimit";
            this.colCreditLimit.ReadOnly = true;
            // 
            // colCreditUsed
            // 
            this.colCreditUsed.DataPropertyName = "credit_used_percent";
            this.colCreditUsed.FillWeight = 95F;
            this.colCreditUsed.HeaderText = "Credit Used %";
            this.colCreditUsed.Name = "colCreditUsed";
            this.colCreditUsed.ReadOnly = true;
            // 
            // colLastTransaction
            // 
            this.colLastTransaction.DataPropertyName = "last_transaction_date";
            this.colLastTransaction.FillWeight = 90F;
            this.colLastTransaction.HeaderText = "Last Transaction Date";
            this.colLastTransaction.Name = "colLastTransaction";
            this.colLastTransaction.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "status_text";
            this.colStatus.FillWeight = 80F;
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // colId
            // 
            this.colId.DataPropertyName = "id";
            this.colId.HeaderText = "id";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Visible = false;
            // 
            // colFirstName
            // 
            this.colFirstName.DataPropertyName = "first_name";
            this.colFirstName.HeaderText = "first_name";
            this.colFirstName.Name = "colFirstName";
            this.colFirstName.ReadOnly = true;
            this.colFirstName.Visible = false;
            // 
            // colLastName
            // 
            this.colLastName.DataPropertyName = "last_name";
            this.colLastName.HeaderText = "last_name";
            this.colLastName.Name = "colLastName";
            this.colLastName.ReadOnly = true;
            this.colLastName.Visible = false;
            // 
            // colAddress
            // 
            this.colAddress.DataPropertyName = "address";
            this.colAddress.HeaderText = "address";
            this.colAddress.Name = "colAddress";
            this.colAddress.ReadOnly = true;
            this.colAddress.Visible = false;
            // 
            // colOverdue30
            // 
            this.colOverdue30.DataPropertyName = "overdue_over_30";
            this.colOverdue30.HeaderText = "overdue_over_30";
            this.colOverdue30.Name = "colOverdue30";
            this.colOverdue30.ReadOnly = true;
            this.colOverdue30.Visible = false;
            // 
            // colOverdue1530
            // 
            this.colOverdue1530.DataPropertyName = "overdue_15_30";
            this.colOverdue1530.HeaderText = "overdue_15_30";
            this.colOverdue1530.Name = "colOverdue1530";
            this.colOverdue1530.ReadOnly = true;
            this.colOverdue1530.Visible = false;
            // 
            // colPaidAmount
            // 
            this.colPaidAmount.DataPropertyName = "paid_amount";
            this.colPaidAmount.HeaderText = "paid_amount";
            this.colPaidAmount.Name = "colPaidAmount";
            this.colPaidAmount.ReadOnly = true;
            this.colPaidAmount.Visible = false;
            // 
            // colCreditAvailable
            // 
            this.colCreditAvailable.DataPropertyName = "credit_available";
            this.colCreditAvailable.HeaderText = "credit_available";
            this.colCreditAvailable.Name = "colCreditAvailable";
            this.colCreditAvailable.ReadOnly = true;
            this.colCreditAvailable.Visible = false;
            // 
            // pnlDetails
            // 
            this.pnlDetails.BackColor = System.Drawing.Color.White;
            this.pnlDetails.Controls.Add(this.btnReceivePayment);
            this.pnlDetails.Controls.Add(this.btnNewInvoice);
            this.pnlDetails.Controls.Add(this.btnViewProfile);
            this.pnlDetails.Controls.Add(this.lstTransactions);
            this.pnlDetails.Controls.Add(this.lblTransactions);
            this.pnlDetails.Controls.Add(this.lblCreditAvailableValue);
            this.pnlDetails.Controls.Add(this.lblCreditAvailableTitle);
            this.pnlDetails.Controls.Add(this.lblCreditUsedValue);
            this.pnlDetails.Controls.Add(this.lblCreditUsedTitle);
            this.pnlDetails.Controls.Add(this.lblCreditLimitValue);
            this.pnlDetails.Controls.Add(this.lblCreditLimitTitle);
            this.pnlDetails.Controls.Add(this.lblOutstandingValue);
            this.pnlDetails.Controls.Add(this.lblOutstandingTitle);
            this.pnlDetails.Controls.Add(this.lblPaidValue);
            this.pnlDetails.Controls.Add(this.lblPaidTitle);
            this.pnlDetails.Controls.Add(this.lblTotalSalesDetailValue);
            this.pnlDetails.Controls.Add(this.lblTotalSalesDetailTitle);
            this.pnlDetails.Controls.Add(this.lblAreaValue);
            this.pnlDetails.Controls.Add(this.lblAddressValue);
            this.pnlDetails.Controls.Add(this.lblPhoneValue);
            this.pnlDetails.Controls.Add(this.panelAvatar);
            this.pnlDetails.Controls.Add(this.lblCustomerHeader);
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Padding = new System.Windows.Forms.Padding(12);
            this.pnlDetails.Size = new System.Drawing.Size(280, 551);
            this.pnlDetails.TabIndex = 0;
            // 
            // btnReceivePayment
            // 
            this.btnReceivePayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReceivePayment.Location = new System.Drawing.Point(15, 512);
            this.btnReceivePayment.Name = "btnReceivePayment";
            this.btnReceivePayment.Size = new System.Drawing.Size(250, 28);
            this.btnReceivePayment.TabIndex = 21;
            this.btnReceivePayment.Text = "Receive Payment";
            this.btnReceivePayment.UseVisualStyleBackColor = true;
            this.btnReceivePayment.Click += new System.EventHandler(this.btnReceivePayment_Click);
            // 
            // btnNewInvoice
            // 
            this.btnNewInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewInvoice.Location = new System.Drawing.Point(15, 478);
            this.btnNewInvoice.Name = "btnNewInvoice";
            this.btnNewInvoice.Size = new System.Drawing.Size(250, 28);
            this.btnNewInvoice.TabIndex = 20;
            this.btnNewInvoice.Text = "New Invoice";
            this.btnNewInvoice.UseVisualStyleBackColor = true;
            this.btnNewInvoice.Click += new System.EventHandler(this.btnNewInvoice_Click);
            // 
            // btnViewProfile
            // 
            this.btnViewProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnViewProfile.Location = new System.Drawing.Point(15, 444);
            this.btnViewProfile.Name = "btnViewProfile";
            this.btnViewProfile.Size = new System.Drawing.Size(250, 28);
            this.btnViewProfile.TabIndex = 19;
            this.btnViewProfile.Text = "View Full Profile";
            this.btnViewProfile.UseVisualStyleBackColor = true;
            this.btnViewProfile.Click += new System.EventHandler(this.btnViewProfile_Click);
            // 
            // lstTransactions
            // 
            this.lstTransactions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstTransactions.FormattingEnabled = true;
            this.lstTransactions.ItemHeight = 16;
            this.lstTransactions.Location = new System.Drawing.Point(15, 307);
            this.lstTransactions.Name = "lstTransactions";
            this.lstTransactions.Size = new System.Drawing.Size(250, 116);
            this.lstTransactions.TabIndex = 18;
            // 
            // lblTransactions
            // 
            this.lblTransactions.AutoSize = true;
            this.lblTransactions.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblTransactions.Location = new System.Drawing.Point(15, 284);
            this.lblTransactions.Name = "lblTransactions";
            this.lblTransactions.Size = new System.Drawing.Size(138, 20);
            this.lblTransactions.TabIndex = 17;
            this.lblTransactions.Text = "Last 5 transactions";
            // 
            // lblCreditAvailableValue
            // 
            this.lblCreditAvailableValue.AutoSize = true;
            this.lblCreditAvailableValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCreditAvailableValue.Location = new System.Drawing.Point(135, 249);
            this.lblCreditAvailableValue.Name = "lblCreditAvailableValue";
            this.lblCreditAvailableValue.Size = new System.Drawing.Size(36, 20);
            this.lblCreditAvailableValue.TabIndex = 16;
            this.lblCreditAvailableValue.Text = "0.00";
            // 
            // lblCreditAvailableTitle
            // 
            this.lblCreditAvailableTitle.AutoSize = true;
            this.lblCreditAvailableTitle.Location = new System.Drawing.Point(15, 249);
            this.lblCreditAvailableTitle.Name = "lblCreditAvailableTitle";
            this.lblCreditAvailableTitle.Size = new System.Drawing.Size(56, 16);
            this.lblCreditAvailableTitle.TabIndex = 15;
            this.lblCreditAvailableTitle.Text = "Available";
            // 
            // lblCreditUsedValue
            // 
            this.lblCreditUsedValue.AutoSize = true;
            this.lblCreditUsedValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCreditUsedValue.Location = new System.Drawing.Point(135, 228);
            this.lblCreditUsedValue.Name = "lblCreditUsedValue";
            this.lblCreditUsedValue.Size = new System.Drawing.Size(36, 20);
            this.lblCreditUsedValue.TabIndex = 14;
            this.lblCreditUsedValue.Text = "0.00";
            // 
            // lblCreditUsedTitle
            // 
            this.lblCreditUsedTitle.AutoSize = true;
            this.lblCreditUsedTitle.Location = new System.Drawing.Point(15, 228);
            this.lblCreditUsedTitle.Name = "lblCreditUsedTitle";
            this.lblCreditUsedTitle.Size = new System.Drawing.Size(35, 16);
            this.lblCreditUsedTitle.TabIndex = 13;
            this.lblCreditUsedTitle.Text = "Used";
            // 
            // lblCreditLimitValue
            // 
            this.lblCreditLimitValue.AutoSize = true;
            this.lblCreditLimitValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCreditLimitValue.Location = new System.Drawing.Point(135, 207);
            this.lblCreditLimitValue.Name = "lblCreditLimitValue";
            this.lblCreditLimitValue.Size = new System.Drawing.Size(36, 20);
            this.lblCreditLimitValue.TabIndex = 12;
            this.lblCreditLimitValue.Text = "0.00";
            // 
            // lblCreditLimitTitle
            // 
            this.lblCreditLimitTitle.AutoSize = true;
            this.lblCreditLimitTitle.Location = new System.Drawing.Point(15, 207);
            this.lblCreditLimitTitle.Name = "lblCreditLimitTitle";
            this.lblCreditLimitTitle.Size = new System.Drawing.Size(32, 16);
            this.lblCreditLimitTitle.TabIndex = 11;
            this.lblCreditLimitTitle.Text = "Limit";
            // 
            // lblOutstandingValue
            // 
            this.lblOutstandingValue.AutoSize = true;
            this.lblOutstandingValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblOutstandingValue.Location = new System.Drawing.Point(135, 176);
            this.lblOutstandingValue.Name = "lblOutstandingValue";
            this.lblOutstandingValue.Size = new System.Drawing.Size(36, 20);
            this.lblOutstandingValue.TabIndex = 10;
            this.lblOutstandingValue.Text = "0.00";
            // 
            // lblOutstandingTitle
            // 
            this.lblOutstandingTitle.AutoSize = true;
            this.lblOutstandingTitle.Location = new System.Drawing.Point(15, 176);
            this.lblOutstandingTitle.Name = "lblOutstandingTitle";
            this.lblOutstandingTitle.Size = new System.Drawing.Size(74, 16);
            this.lblOutstandingTitle.TabIndex = 9;
            this.lblOutstandingTitle.Text = "Outstanding";
            // 
            // lblPaidValue
            // 
            this.lblPaidValue.AutoSize = true;
            this.lblPaidValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPaidValue.Location = new System.Drawing.Point(135, 155);
            this.lblPaidValue.Name = "lblPaidValue";
            this.lblPaidValue.Size = new System.Drawing.Size(36, 20);
            this.lblPaidValue.TabIndex = 8;
            this.lblPaidValue.Text = "0.00";
            // 
            // lblPaidTitle
            // 
            this.lblPaidTitle.AutoSize = true;
            this.lblPaidTitle.Location = new System.Drawing.Point(15, 155);
            this.lblPaidTitle.Name = "lblPaidTitle";
            this.lblPaidTitle.Size = new System.Drawing.Size(32, 16);
            this.lblPaidTitle.TabIndex = 7;
            this.lblPaidTitle.Text = "Paid";
            // 
            // lblTotalSalesDetailValue
            // 
            this.lblTotalSalesDetailValue.AutoSize = true;
            this.lblTotalSalesDetailValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalSalesDetailValue.Location = new System.Drawing.Point(135, 134);
            this.lblTotalSalesDetailValue.Name = "lblTotalSalesDetailValue";
            this.lblTotalSalesDetailValue.Size = new System.Drawing.Size(36, 20);
            this.lblTotalSalesDetailValue.TabIndex = 6;
            this.lblTotalSalesDetailValue.Text = "0.00";
            // 
            // lblTotalSalesDetailTitle
            // 
            this.lblTotalSalesDetailTitle.AutoSize = true;
            this.lblTotalSalesDetailTitle.Location = new System.Drawing.Point(15, 134);
            this.lblTotalSalesDetailTitle.Name = "lblTotalSalesDetailTitle";
            this.lblTotalSalesDetailTitle.Size = new System.Drawing.Size(68, 16);
            this.lblTotalSalesDetailTitle.TabIndex = 5;
            this.lblTotalSalesDetailTitle.Text = "Total Sales";
            // 
            // lblAreaValue
            // 
            this.lblAreaValue.AutoSize = true;
            this.lblAreaValue.Location = new System.Drawing.Point(84, 110);
            this.lblAreaValue.Name = "lblAreaValue";
            this.lblAreaValue.Size = new System.Drawing.Size(34, 16);
            this.lblAreaValue.TabIndex = 4;
            this.lblAreaValue.Text = "Area";
            // 
            // lblAddressValue
            // 
            this.lblAddressValue.AutoSize = true;
            this.lblAddressValue.Location = new System.Drawing.Point(84, 94);
            this.lblAddressValue.Name = "lblAddressValue";
            this.lblAddressValue.Size = new System.Drawing.Size(52, 16);
            this.lblAddressValue.TabIndex = 3;
            this.lblAddressValue.Text = "Address";
            // 
            // lblPhoneValue
            // 
            this.lblPhoneValue.AutoSize = true;
            this.lblPhoneValue.Location = new System.Drawing.Point(84, 78);
            this.lblPhoneValue.Name = "lblPhoneValue";
            this.lblPhoneValue.Size = new System.Drawing.Size(44, 16);
            this.lblPhoneValue.TabIndex = 2;
            this.lblPhoneValue.Text = "Phone";
            // 
            // panelAvatar
            // 
            this.panelAvatar.Location = new System.Drawing.Point(15, 76);
            this.panelAvatar.Name = "panelAvatar";
            this.panelAvatar.Size = new System.Drawing.Size(58, 50);
            this.panelAvatar.TabIndex = 1;
            this.panelAvatar.Paint += new System.Windows.Forms.PaintEventHandler(this.panelAvatar_Paint);
            // 
            // lblCustomerHeader
            // 
            this.lblCustomerHeader.AutoSize = true;
            this.lblCustomerHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblCustomerHeader.Location = new System.Drawing.Point(14, 18);
            this.lblCustomerHeader.Name = "lblCustomerHeader";
            this.lblCustomerHeader.Size = new System.Drawing.Size(128, 23);
            this.lblCustomerHeader.TabIndex = 0;
            this.lblCustomerHeader.Text = "Select customer";
            // 
            // detailSlideTimer
            // 
            this.detailSlideTimer.Interval = 15;
            this.detailSlideTimer.Tick += new System.EventHandler(this.detailSlideTimer_Tick);
            // 
            // frm_customers
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1384, 761);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlTop);
            this.KeyPreview = true;
            this.Name = "frm_customers";
            this.ShowIcon = false;
            this.Text = "Customers";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_customers_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.tableKpis.ResumeLayout(false);
            this.cardTotalCustomers.ResumeLayout(false);
            this.cardTotalCustomers.PerformLayout();
            this.cardReceivables.ResumeLayout(false);
            this.cardReceivables.PerformLayout();
            this.cardOverdue.ResumeLayout(false);
            this.cardOverdue.PerformLayout();
            this.cardNewCustomers.ResumeLayout(false);
            this.cardNewCustomers.PerformLayout();
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_customers)).EndInit();
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.TableLayoutPanel tableKpis;
        private System.Windows.Forms.Panel cardTotalCustomers;
        private System.Windows.Forms.Label lblTotalCustomersValue;
        private System.Windows.Forms.Label lblTotalCustomersTitle;
        private System.Windows.Forms.Panel cardReceivables;
        private System.Windows.Forms.Label lblReceivablesValue;
        private System.Windows.Forms.Label lblReceivablesTitle;
        private System.Windows.Forms.Panel cardOverdue;
        private System.Windows.Forms.Label lblOverdueValue;
        private System.Windows.Forms.Label lblOverdueTitle;
        private System.Windows.Forms.Panel cardNewCustomers;
        private System.Windows.Forms.Label lblNewCustomersValue;
        private System.Windows.Forms.Label lblNewCustomersTitle;
        private System.Windows.Forms.Label lblNewCustomersTrend;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label lblFilterStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblFilterArea;
        private System.Windows.Forms.ComboBox cmbArea;
        private System.Windows.Forms.Label lblFilterSearch;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.DataGridView grid_customers;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Label lblCustomerHeader;
        private System.Windows.Forms.Panel panelAvatar;
        private System.Windows.Forms.Label lblPhoneValue;
        private System.Windows.Forms.Label lblAddressValue;
        private System.Windows.Forms.Label lblAreaValue;
        private System.Windows.Forms.Label lblTotalSalesDetailTitle;
        private System.Windows.Forms.Label lblTotalSalesDetailValue;
        private System.Windows.Forms.Label lblPaidTitle;
        private System.Windows.Forms.Label lblPaidValue;
        private System.Windows.Forms.Label lblOutstandingTitle;
        private System.Windows.Forms.Label lblOutstandingValue;
        private System.Windows.Forms.Label lblCreditLimitTitle;
        private System.Windows.Forms.Label lblCreditLimitValue;
        private System.Windows.Forms.Label lblCreditUsedTitle;
        private System.Windows.Forms.Label lblCreditUsedValue;
        private System.Windows.Forms.Label lblCreditAvailableTitle;
        private System.Windows.Forms.Label lblCreditAvailableValue;
        private System.Windows.Forms.Label lblTransactions;
        private System.Windows.Forms.ListBox lstTransactions;
        private System.Windows.Forms.Button btnViewProfile;
        private System.Windows.Forms.Button btnNewInvoice;
        private System.Windows.Forms.Button btnReceivePayment;
        private System.Windows.Forms.Timer detailSlideTimer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalSales;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutstanding;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreditLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreditUsed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastTransaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOverdue30;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOverdue1530;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaidAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreditAvailable;
    }
}

