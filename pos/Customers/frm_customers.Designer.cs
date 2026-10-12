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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_customers));
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
            this.lblNewCustomersTrend = new System.Windows.Forms.Label();
            this.lblNewCustomersValue = new System.Windows.Forms.Label();
            this.lblNewCustomersTitle = new System.Windows.Forms.Label();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.lblFilterStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblFilterArea = new System.Windows.Forms.Label();
            this.cmbArea = new System.Windows.Forms.ComboBox();
            this.lblFilterSearch = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.detailSlideTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_customers)).BeginInit();
            this.pnlDetails.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.tableKpis.SuspendLayout();
            this.cardTotalCustomers.SuspendLayout();
            this.cardReceivables.SuspendLayout();
            this.cardOverdue.SuspendLayout();
            this.cardNewCustomers.SuspendLayout();
            this.pnlFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            resources.ApplyResources(this.splitMain, "splitMain");
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            resources.ApplyResources(this.splitMain.Panel1, "splitMain.Panel1");
            this.splitMain.Panel1.Controls.Add(this.grid_customers);
            // 
            // splitMain.Panel2
            // 
            resources.ApplyResources(this.splitMain.Panel2, "splitMain.Panel2");
            this.splitMain.Panel2.Controls.Add(this.pnlDetails);
            // 
            // grid_customers
            // 
            resources.ApplyResources(this.grid_customers, "grid_customers");
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
            this.grid_customers.MultiSelect = false;
            this.grid_customers.Name = "grid_customers";
            this.grid_customers.ReadOnly = true;
            this.grid_customers.RowHeadersVisible = false;
            this.grid_customers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_customers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_customers_CellDoubleClick);
            this.grid_customers.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid_customers_CellFormatting);
            this.grid_customers.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grid_customers_CellPainting);
            this.grid_customers.SelectionChanged += new System.EventHandler(this.grid_customers_SelectionChanged);
            // 
            // colNo
            // 
            this.colNo.DataPropertyName = "row_no";
            this.colNo.FillWeight = 40F;
            resources.ApplyResources(this.colNo, "colNo");
            this.colNo.Name = "colNo";
            this.colNo.ReadOnly = true;
            // 
            // colCustomerCode
            // 
            this.colCustomerCode.DataPropertyName = "customer_code";
            this.colCustomerCode.FillWeight = 80F;
            resources.ApplyResources(this.colCustomerCode, "colCustomerCode");
            this.colCustomerCode.Name = "colCustomerCode";
            this.colCustomerCode.ReadOnly = true;
            // 
            // colCustomerName
            // 
            this.colCustomerName.DataPropertyName = "customer_name";
            this.colCustomerName.FillWeight = 150F;
            resources.ApplyResources(this.colCustomerName, "colCustomerName");
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.ReadOnly = true;
            // 
            // colArea
            // 
            this.colArea.DataPropertyName = "area";
            this.colArea.FillWeight = 90F;
            resources.ApplyResources(this.colArea, "colArea");
            this.colArea.Name = "colArea";
            this.colArea.ReadOnly = true;
            // 
            // colPhone
            // 
            this.colPhone.DataPropertyName = "contact_no";
            this.colPhone.FillWeight = 90F;
            resources.ApplyResources(this.colPhone, "colPhone");
            this.colPhone.Name = "colPhone";
            this.colPhone.ReadOnly = true;
            // 
            // colTotalSales
            // 
            this.colTotalSales.DataPropertyName = "total_sales";
            this.colTotalSales.FillWeight = 95F;
            resources.ApplyResources(this.colTotalSales, "colTotalSales");
            this.colTotalSales.Name = "colTotalSales";
            this.colTotalSales.ReadOnly = true;
            // 
            // colOutstanding
            // 
            this.colOutstanding.DataPropertyName = "outstanding_balance";
            this.colOutstanding.FillWeight = 95F;
            resources.ApplyResources(this.colOutstanding, "colOutstanding");
            this.colOutstanding.Name = "colOutstanding";
            this.colOutstanding.ReadOnly = true;
            // 
            // colCreditLimit
            // 
            this.colCreditLimit.DataPropertyName = "credit_limit";
            this.colCreditLimit.FillWeight = 90F;
            resources.ApplyResources(this.colCreditLimit, "colCreditLimit");
            this.colCreditLimit.Name = "colCreditLimit";
            this.colCreditLimit.ReadOnly = true;
            // 
            // colCreditUsed
            // 
            this.colCreditUsed.DataPropertyName = "credit_used_percent";
            this.colCreditUsed.FillWeight = 95F;
            resources.ApplyResources(this.colCreditUsed, "colCreditUsed");
            this.colCreditUsed.Name = "colCreditUsed";
            this.colCreditUsed.ReadOnly = true;
            // 
            // colLastTransaction
            // 
            this.colLastTransaction.DataPropertyName = "last_transaction_date";
            this.colLastTransaction.FillWeight = 90F;
            resources.ApplyResources(this.colLastTransaction, "colLastTransaction");
            this.colLastTransaction.Name = "colLastTransaction";
            this.colLastTransaction.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "status_text";
            this.colStatus.FillWeight = 80F;
            resources.ApplyResources(this.colStatus, "colStatus");
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // colId
            // 
            this.colId.DataPropertyName = "id";
            resources.ApplyResources(this.colId, "colId");
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            // 
            // colFirstName
            // 
            this.colFirstName.DataPropertyName = "first_name";
            resources.ApplyResources(this.colFirstName, "colFirstName");
            this.colFirstName.Name = "colFirstName";
            this.colFirstName.ReadOnly = true;
            // 
            // colLastName
            // 
            this.colLastName.DataPropertyName = "last_name";
            resources.ApplyResources(this.colLastName, "colLastName");
            this.colLastName.Name = "colLastName";
            this.colLastName.ReadOnly = true;
            // 
            // colAddress
            // 
            this.colAddress.DataPropertyName = "address";
            resources.ApplyResources(this.colAddress, "colAddress");
            this.colAddress.Name = "colAddress";
            this.colAddress.ReadOnly = true;
            // 
            // colOverdue30
            // 
            this.colOverdue30.DataPropertyName = "overdue_over_30";
            resources.ApplyResources(this.colOverdue30, "colOverdue30");
            this.colOverdue30.Name = "colOverdue30";
            this.colOverdue30.ReadOnly = true;
            // 
            // colOverdue1530
            // 
            this.colOverdue1530.DataPropertyName = "overdue_15_30";
            resources.ApplyResources(this.colOverdue1530, "colOverdue1530");
            this.colOverdue1530.Name = "colOverdue1530";
            this.colOverdue1530.ReadOnly = true;
            // 
            // colPaidAmount
            // 
            this.colPaidAmount.DataPropertyName = "paid_amount";
            resources.ApplyResources(this.colPaidAmount, "colPaidAmount");
            this.colPaidAmount.Name = "colPaidAmount";
            this.colPaidAmount.ReadOnly = true;
            // 
            // colCreditAvailable
            // 
            this.colCreditAvailable.DataPropertyName = "credit_available";
            resources.ApplyResources(this.colCreditAvailable, "colCreditAvailable");
            this.colCreditAvailable.Name = "colCreditAvailable";
            this.colCreditAvailable.ReadOnly = true;
            // 
            // pnlDetails
            // 
            resources.ApplyResources(this.pnlDetails, "pnlDetails");
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
            this.pnlDetails.Name = "pnlDetails";
            // 
            // btnReceivePayment
            // 
            resources.ApplyResources(this.btnReceivePayment, "btnReceivePayment");
            this.btnReceivePayment.Name = "btnReceivePayment";
            this.btnReceivePayment.UseVisualStyleBackColor = true;
            this.btnReceivePayment.Click += new System.EventHandler(this.btnReceivePayment_Click);
            // 
            // btnNewInvoice
            // 
            resources.ApplyResources(this.btnNewInvoice, "btnNewInvoice");
            this.btnNewInvoice.Name = "btnNewInvoice";
            this.btnNewInvoice.UseVisualStyleBackColor = true;
            this.btnNewInvoice.Click += new System.EventHandler(this.btnNewInvoice_Click);
            // 
            // btnViewProfile
            // 
            resources.ApplyResources(this.btnViewProfile, "btnViewProfile");
            this.btnViewProfile.Name = "btnViewProfile";
            this.btnViewProfile.UseVisualStyleBackColor = true;
            this.btnViewProfile.Click += new System.EventHandler(this.btnViewProfile_Click);
            // 
            // lstTransactions
            // 
            resources.ApplyResources(this.lstTransactions, "lstTransactions");
            this.lstTransactions.FormattingEnabled = true;
            this.lstTransactions.Name = "lstTransactions";
            // 
            // lblTransactions
            // 
            resources.ApplyResources(this.lblTransactions, "lblTransactions");
            this.lblTransactions.Name = "lblTransactions";
            // 
            // lblCreditAvailableValue
            // 
            resources.ApplyResources(this.lblCreditAvailableValue, "lblCreditAvailableValue");
            this.lblCreditAvailableValue.Name = "lblCreditAvailableValue";
            // 
            // lblCreditAvailableTitle
            // 
            resources.ApplyResources(this.lblCreditAvailableTitle, "lblCreditAvailableTitle");
            this.lblCreditAvailableTitle.Name = "lblCreditAvailableTitle";
            // 
            // lblCreditUsedValue
            // 
            resources.ApplyResources(this.lblCreditUsedValue, "lblCreditUsedValue");
            this.lblCreditUsedValue.Name = "lblCreditUsedValue";
            // 
            // lblCreditUsedTitle
            // 
            resources.ApplyResources(this.lblCreditUsedTitle, "lblCreditUsedTitle");
            this.lblCreditUsedTitle.Name = "lblCreditUsedTitle";
            // 
            // lblCreditLimitValue
            // 
            resources.ApplyResources(this.lblCreditLimitValue, "lblCreditLimitValue");
            this.lblCreditLimitValue.Name = "lblCreditLimitValue";
            // 
            // lblCreditLimitTitle
            // 
            resources.ApplyResources(this.lblCreditLimitTitle, "lblCreditLimitTitle");
            this.lblCreditLimitTitle.Name = "lblCreditLimitTitle";
            // 
            // lblOutstandingValue
            // 
            resources.ApplyResources(this.lblOutstandingValue, "lblOutstandingValue");
            this.lblOutstandingValue.Name = "lblOutstandingValue";
            // 
            // lblOutstandingTitle
            // 
            resources.ApplyResources(this.lblOutstandingTitle, "lblOutstandingTitle");
            this.lblOutstandingTitle.Name = "lblOutstandingTitle";
            // 
            // lblPaidValue
            // 
            resources.ApplyResources(this.lblPaidValue, "lblPaidValue");
            this.lblPaidValue.Name = "lblPaidValue";
            // 
            // lblPaidTitle
            // 
            resources.ApplyResources(this.lblPaidTitle, "lblPaidTitle");
            this.lblPaidTitle.Name = "lblPaidTitle";
            // 
            // lblTotalSalesDetailValue
            // 
            resources.ApplyResources(this.lblTotalSalesDetailValue, "lblTotalSalesDetailValue");
            this.lblTotalSalesDetailValue.Name = "lblTotalSalesDetailValue";
            // 
            // lblTotalSalesDetailTitle
            // 
            resources.ApplyResources(this.lblTotalSalesDetailTitle, "lblTotalSalesDetailTitle");
            this.lblTotalSalesDetailTitle.Name = "lblTotalSalesDetailTitle";
            // 
            // lblAreaValue
            // 
            resources.ApplyResources(this.lblAreaValue, "lblAreaValue");
            this.lblAreaValue.Name = "lblAreaValue";
            // 
            // lblAddressValue
            // 
            resources.ApplyResources(this.lblAddressValue, "lblAddressValue");
            this.lblAddressValue.Name = "lblAddressValue";
            // 
            // lblPhoneValue
            // 
            resources.ApplyResources(this.lblPhoneValue, "lblPhoneValue");
            this.lblPhoneValue.Name = "lblPhoneValue";
            // 
            // panelAvatar
            // 
            resources.ApplyResources(this.panelAvatar, "panelAvatar");
            this.panelAvatar.Name = "panelAvatar";
            this.panelAvatar.Paint += new System.Windows.Forms.PaintEventHandler(this.panelAvatar_Paint);
            // 
            // lblCustomerHeader
            // 
            resources.ApplyResources(this.lblCustomerHeader, "lblCustomerHeader");
            this.lblCustomerHeader.Name = "lblCustomerHeader";
            // 
            // pnlTop
            // 
            resources.ApplyResources(this.pnlTop, "pnlTop");
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.tableKpis);
            this.pnlTop.Controls.Add(this.pnlFilters);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Name = "pnlTop";
            // 
            // tableKpis
            // 
            resources.ApplyResources(this.tableKpis, "tableKpis");
            this.tableKpis.Controls.Add(this.cardTotalCustomers, 0, 0);
            this.tableKpis.Controls.Add(this.cardReceivables, 1, 0);
            this.tableKpis.Controls.Add(this.cardOverdue, 2, 0);
            this.tableKpis.Controls.Add(this.cardNewCustomers, 3, 0);
            this.tableKpis.Name = "tableKpis";
            // 
            // cardTotalCustomers
            // 
            resources.ApplyResources(this.cardTotalCustomers, "cardTotalCustomers");
            this.cardTotalCustomers.BackColor = System.Drawing.Color.White;
            this.cardTotalCustomers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTotalCustomers.Controls.Add(this.lblTotalCustomersValue);
            this.cardTotalCustomers.Controls.Add(this.lblTotalCustomersTitle);
            this.cardTotalCustomers.Name = "cardTotalCustomers";
            // 
            // lblTotalCustomersValue
            // 
            resources.ApplyResources(this.lblTotalCustomersValue, "lblTotalCustomersValue");
            this.lblTotalCustomersValue.Name = "lblTotalCustomersValue";
            // 
            // lblTotalCustomersTitle
            // 
            resources.ApplyResources(this.lblTotalCustomersTitle, "lblTotalCustomersTitle");
            this.lblTotalCustomersTitle.Name = "lblTotalCustomersTitle";
            // 
            // cardReceivables
            // 
            resources.ApplyResources(this.cardReceivables, "cardReceivables");
            this.cardReceivables.BackColor = System.Drawing.Color.White;
            this.cardReceivables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardReceivables.Controls.Add(this.lblReceivablesValue);
            this.cardReceivables.Controls.Add(this.lblReceivablesTitle);
            this.cardReceivables.Name = "cardReceivables";
            // 
            // lblReceivablesValue
            // 
            resources.ApplyResources(this.lblReceivablesValue, "lblReceivablesValue");
            this.lblReceivablesValue.Name = "lblReceivablesValue";
            // 
            // lblReceivablesTitle
            // 
            resources.ApplyResources(this.lblReceivablesTitle, "lblReceivablesTitle");
            this.lblReceivablesTitle.Name = "lblReceivablesTitle";
            // 
            // cardOverdue
            // 
            resources.ApplyResources(this.cardOverdue, "cardOverdue");
            this.cardOverdue.BackColor = System.Drawing.Color.White;
            this.cardOverdue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardOverdue.Controls.Add(this.lblOverdueValue);
            this.cardOverdue.Controls.Add(this.lblOverdueTitle);
            this.cardOverdue.Name = "cardOverdue";
            // 
            // lblOverdueValue
            // 
            resources.ApplyResources(this.lblOverdueValue, "lblOverdueValue");
            this.lblOverdueValue.ForeColor = System.Drawing.Color.Firebrick;
            this.lblOverdueValue.Name = "lblOverdueValue";
            // 
            // lblOverdueTitle
            // 
            resources.ApplyResources(this.lblOverdueTitle, "lblOverdueTitle");
            this.lblOverdueTitle.Name = "lblOverdueTitle";
            // 
            // cardNewCustomers
            // 
            resources.ApplyResources(this.cardNewCustomers, "cardNewCustomers");
            this.cardNewCustomers.BackColor = System.Drawing.Color.White;
            this.cardNewCustomers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardNewCustomers.Controls.Add(this.lblNewCustomersTrend);
            this.cardNewCustomers.Controls.Add(this.lblNewCustomersValue);
            this.cardNewCustomers.Controls.Add(this.lblNewCustomersTitle);
            this.cardNewCustomers.Name = "cardNewCustomers";
            // 
            // lblNewCustomersTrend
            // 
            resources.ApplyResources(this.lblNewCustomersTrend, "lblNewCustomersTrend");
            this.lblNewCustomersTrend.Name = "lblNewCustomersTrend";
            // 
            // lblNewCustomersValue
            // 
            resources.ApplyResources(this.lblNewCustomersValue, "lblNewCustomersValue");
            this.lblNewCustomersValue.Name = "lblNewCustomersValue";
            // 
            // lblNewCustomersTitle
            // 
            resources.ApplyResources(this.lblNewCustomersTitle, "lblNewCustomersTitle");
            this.lblNewCustomersTitle.Name = "lblNewCustomersTitle";
            // 
            // pnlFilters
            // 
            resources.ApplyResources(this.pnlFilters, "pnlFilters");
            this.pnlFilters.Controls.Add(this.lblFilterStatus);
            this.pnlFilters.Controls.Add(this.cmbStatus);
            this.pnlFilters.Controls.Add(this.lblFilterArea);
            this.pnlFilters.Controls.Add(this.cmbArea);
            this.pnlFilters.Controls.Add(this.lblFilterSearch);
            this.pnlFilters.Controls.Add(this.txt_search);
            this.pnlFilters.Name = "pnlFilters";
            // 
            // lblFilterStatus
            // 
            resources.ApplyResources(this.lblFilterStatus, "lblFilterStatus");
            this.lblFilterStatus.Name = "lblFilterStatus";
            // 
            // cmbStatus
            // 
            resources.ApplyResources(this.cmbStatus, "cmbStatus");
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_SelectedIndexChanged);
            // 
            // lblFilterArea
            // 
            resources.ApplyResources(this.lblFilterArea, "lblFilterArea");
            this.lblFilterArea.Name = "lblFilterArea";
            // 
            // cmbArea
            // 
            resources.ApplyResources(this.cmbArea, "cmbArea");
            this.cmbArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbArea.FormattingEnabled = true;
            this.cmbArea.Name = "cmbArea";
            this.cmbArea.SelectedIndexChanged += new System.EventHandler(this.cmbArea_SelectedIndexChanged);
            // 
            // lblFilterSearch
            // 
            resources.ApplyResources(this.lblFilterSearch, "lblFilterSearch");
            this.lblFilterSearch.Name = "lblFilterSearch";
            // 
            // txt_search
            // 
            resources.ApplyResources(this.txt_search, "txt_search");
            this.txt_search.Name = "txt_search";
            this.txt_search.TextChanged += new System.EventHandler(this.txt_search_TextChanged);
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // detailSlideTimer
            // 
            this.detailSlideTimer.Interval = 15;
            this.detailSlideTimer.Tick += new System.EventHandler(this.detailSlideTimer_Tick);
            // 
            // frm_customers
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlTop);
            this.KeyPreview = true;
            this.Name = "frm_customers";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_customers_Load);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_customers)).EndInit();
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
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

