namespace pos
{
    partial class frm_suppliers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_suppliers));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.tableKpis = new System.Windows.Forms.TableLayoutPanel();
            this.cardTotalSuppliers = new System.Windows.Forms.Panel();
            this.lblTotalSuppliersValue = new System.Windows.Forms.Label();
            this.lblTotalSuppliersTitle = new System.Windows.Forms.Label();
            this.cardTotalPayables = new System.Windows.Forms.Panel();
            this.lblTotalPayablesValue = new System.Windows.Forms.Label();
            this.lblTotalPayablesTitle = new System.Windows.Forms.Label();
            this.cardOverduePayables = new System.Windows.Forms.Panel();
            this.lblOverduePayablesValue = new System.Windows.Forms.Label();
            this.lblOverduePayablesTitle = new System.Windows.Forms.Label();
            this.cardThisMonth = new System.Windows.Forms.Panel();
            this.lblThisMonthValue = new System.Windows.Forms.Label();
            this.lblThisMonthTitle = new System.Windows.Forms.Label();
            this.lblThisMonthTrend = new System.Windows.Forms.Label();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.lblFilterStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblFilterCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblFilterSearch = new System.Windows.Forms.Label();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.grid_suppliers = new System.Windows.Forms.DataGridView();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalPurchases = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPayable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreditDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastBill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDaysSince = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreditLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalPaid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsOverdue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.btnMakePayment = new System.Windows.Forms.Button();
            this.btnNewBill = new System.Windows.Forms.Button();
            this.btnViewProfile = new System.Windows.Forms.Button();
            this.lstBills = new System.Windows.Forms.ListBox();
            this.lblBills = new System.Windows.Forms.Label();
            this.lblCreditLimitValue = new System.Windows.Forms.Label();
            this.lblCreditLimitTitle = new System.Windows.Forms.Label();
            this.lblCreditDaysValue = new System.Windows.Forms.Label();
            this.lblCreditDaysTitle = new System.Windows.Forms.Label();
            this.lblPayableValue = new System.Windows.Forms.Label();
            this.lblPayableTitle = new System.Windows.Forms.Label();
            this.lblPaidValue = new System.Windows.Forms.Label();
            this.lblPaidTitle = new System.Windows.Forms.Label();
            this.lblPurchasedValue = new System.Windows.Forms.Label();
            this.lblPurchasedTitle = new System.Windows.Forms.Label();
            this.lblCategoryValue = new System.Windows.Forms.Label();
            this.lblAddressValue = new System.Windows.Forms.Label();
            this.lblPhoneValue = new System.Windows.Forms.Label();
            this.panelAvatar = new System.Windows.Forms.Panel();
            this.lblSupplierHeader = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.lblBottomSummary = new System.Windows.Forms.Label();
            this.detailSlideTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlTop.SuspendLayout();
            this.tableKpis.SuspendLayout();
            this.cardTotalSuppliers.SuspendLayout();
            this.cardTotalPayables.SuspendLayout();
            this.cardOverduePayables.SuspendLayout();
            this.cardThisMonth.SuspendLayout();
            this.pnlFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_suppliers)).BeginInit();
            this.pnlDetails.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.tableKpis);
            this.pnlTop.Controls.Add(this.pnlFilters);
            this.pnlTop.Controls.Add(this.lblTitle);
            resources.ApplyResources(this.pnlTop, "pnlTop");
            this.pnlTop.Name = "pnlTop";
            // 
            // tableKpis
            // 
            resources.ApplyResources(this.tableKpis, "tableKpis");
            this.tableKpis.Controls.Add(this.cardTotalSuppliers, 0, 0);
            this.tableKpis.Controls.Add(this.cardTotalPayables, 1, 0);
            this.tableKpis.Controls.Add(this.cardOverduePayables, 2, 0);
            this.tableKpis.Controls.Add(this.cardThisMonth, 3, 0);
            this.tableKpis.Name = "tableKpis";
            // 
            // cardTotalSuppliers
            // 
            this.cardTotalSuppliers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTotalSuppliers.Controls.Add(this.lblTotalSuppliersValue);
            this.cardTotalSuppliers.Controls.Add(this.lblTotalSuppliersTitle);
            resources.ApplyResources(this.cardTotalSuppliers, "cardTotalSuppliers");
            this.cardTotalSuppliers.Name = "cardTotalSuppliers";
            // 
            // lblTotalSuppliersValue
            // 
            resources.ApplyResources(this.lblTotalSuppliersValue, "lblTotalSuppliersValue");
            this.lblTotalSuppliersValue.Name = "lblTotalSuppliersValue";
            // 
            // lblTotalSuppliersTitle
            // 
            resources.ApplyResources(this.lblTotalSuppliersTitle, "lblTotalSuppliersTitle");
            this.lblTotalSuppliersTitle.Name = "lblTotalSuppliersTitle";
            // 
            // cardTotalPayables
            // 
            this.cardTotalPayables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardTotalPayables.Controls.Add(this.lblTotalPayablesValue);
            this.cardTotalPayables.Controls.Add(this.lblTotalPayablesTitle);
            resources.ApplyResources(this.cardTotalPayables, "cardTotalPayables");
            this.cardTotalPayables.Name = "cardTotalPayables";
            // 
            // lblTotalPayablesValue
            // 
            resources.ApplyResources(this.lblTotalPayablesValue, "lblTotalPayablesValue");
            this.lblTotalPayablesValue.Name = "lblTotalPayablesValue";
            // 
            // lblTotalPayablesTitle
            // 
            resources.ApplyResources(this.lblTotalPayablesTitle, "lblTotalPayablesTitle");
            this.lblTotalPayablesTitle.Name = "lblTotalPayablesTitle";
            // 
            // cardOverduePayables
            // 
            this.cardOverduePayables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardOverduePayables.Controls.Add(this.lblOverduePayablesValue);
            this.cardOverduePayables.Controls.Add(this.lblOverduePayablesTitle);
            resources.ApplyResources(this.cardOverduePayables, "cardOverduePayables");
            this.cardOverduePayables.Name = "cardOverduePayables";
            // 
            // lblOverduePayablesValue
            // 
            resources.ApplyResources(this.lblOverduePayablesValue, "lblOverduePayablesValue");
            this.lblOverduePayablesValue.ForeColor = System.Drawing.Color.Firebrick;
            this.lblOverduePayablesValue.Name = "lblOverduePayablesValue";
            // 
            // lblOverduePayablesTitle
            // 
            resources.ApplyResources(this.lblOverduePayablesTitle, "lblOverduePayablesTitle");
            this.lblOverduePayablesTitle.Name = "lblOverduePayablesTitle";
            // 
            // cardThisMonth
            // 
            this.cardThisMonth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardThisMonth.Controls.Add(this.lblThisMonthValue);
            this.cardThisMonth.Controls.Add(this.lblThisMonthTitle);
            this.cardThisMonth.Controls.Add(this.lblThisMonthTrend);
            resources.ApplyResources(this.cardThisMonth, "cardThisMonth");
            this.cardThisMonth.Name = "cardThisMonth";
            // 
            // lblThisMonthValue
            // 
            resources.ApplyResources(this.lblThisMonthValue, "lblThisMonthValue");
            this.lblThisMonthValue.Name = "lblThisMonthValue";
            // 
            // lblThisMonthTitle
            // 
            resources.ApplyResources(this.lblThisMonthTitle, "lblThisMonthTitle");
            this.lblThisMonthTitle.Name = "lblThisMonthTitle";
            // 
            // lblThisMonthTrend
            // 
            resources.ApplyResources(this.lblThisMonthTrend, "lblThisMonthTrend");
            this.lblThisMonthTrend.Name = "lblThisMonthTrend";
            // 
            // pnlFilters
            // 
            this.pnlFilters.Controls.Add(this.lblFilterStatus);
            this.pnlFilters.Controls.Add(this.cmbStatus);
            this.pnlFilters.Controls.Add(this.lblFilterCategory);
            this.pnlFilters.Controls.Add(this.cmbCategory);
            this.pnlFilters.Controls.Add(this.lblFilterSearch);
            this.pnlFilters.Controls.Add(this.txt_search);
            resources.ApplyResources(this.pnlFilters, "pnlFilters");
            this.pnlFilters.Name = "pnlFilters";
            // 
            // lblFilterStatus
            // 
            resources.ApplyResources(this.lblFilterStatus, "lblFilterStatus");
            this.lblFilterStatus.Name = "lblFilterStatus";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            resources.ApplyResources(this.cmbStatus, "cmbStatus");
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_SelectedIndexChanged);
            // 
            // lblFilterCategory
            // 
            resources.ApplyResources(this.lblFilterCategory, "lblFilterCategory");
            this.lblFilterCategory.Name = "lblFilterCategory";
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCategory, "cmbCategory");
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
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
            // splitMain
            // 
            resources.ApplyResources(this.splitMain, "splitMain");
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.grid_suppliers);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.pnlDetails);
            // 
            // grid_suppliers
            // 
            this.grid_suppliers.AllowUserToAddRows = false;
            this.grid_suppliers.AllowUserToDeleteRows = false;
            this.grid_suppliers.AllowUserToOrderColumns = true;
            this.grid_suppliers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_suppliers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_suppliers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo,
            this.colSupplierCode,
            this.colSupplierName,
            this.colCategory,
            this.colPhone,
            this.colTotalPurchases,
            this.colPayable,
            this.colCreditDays,
            this.colLastBill,
            this.colDaysSince,
            this.colStatus,
            this.colId,
            this.colAddress,
            this.colCreditLimit,
            this.colTotalPaid,
            this.colIsOverdue});
            resources.ApplyResources(this.grid_suppliers, "grid_suppliers");
            this.grid_suppliers.MultiSelect = false;
            this.grid_suppliers.Name = "grid_suppliers";
            this.grid_suppliers.ReadOnly = true;
            this.grid_suppliers.RowHeadersVisible = false;
            this.grid_suppliers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_suppliers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_suppliers_CellDoubleClick);
            this.grid_suppliers.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid_suppliers_CellFormatting);
            this.grid_suppliers.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grid_suppliers_CellPainting);
            this.grid_suppliers.SelectionChanged += new System.EventHandler(this.grid_suppliers_SelectionChanged);
            // 
            // colNo
            // 
            this.colNo.DataPropertyName = "row_no";
            this.colNo.FillWeight = 40F;
            resources.ApplyResources(this.colNo, "colNo");
            this.colNo.Name = "colNo";
            this.colNo.ReadOnly = true;
            // 
            // colSupplierCode
            // 
            this.colSupplierCode.DataPropertyName = "supplier_code";
            this.colSupplierCode.FillWeight = 80F;
            resources.ApplyResources(this.colSupplierCode, "colSupplierCode");
            this.colSupplierCode.Name = "colSupplierCode";
            this.colSupplierCode.ReadOnly = true;
            // 
            // colSupplierName
            // 
            this.colSupplierName.DataPropertyName = "supplier_name";
            this.colSupplierName.FillWeight = 150F;
            resources.ApplyResources(this.colSupplierName, "colSupplierName");
            this.colSupplierName.Name = "colSupplierName";
            this.colSupplierName.ReadOnly = true;
            // 
            // colCategory
            // 
            this.colCategory.DataPropertyName = "category";
            this.colCategory.FillWeight = 90F;
            resources.ApplyResources(this.colCategory, "colCategory");
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            // 
            // colPhone
            // 
            this.colPhone.DataPropertyName = "contact_no";
            this.colPhone.FillWeight = 90F;
            resources.ApplyResources(this.colPhone, "colPhone");
            this.colPhone.Name = "colPhone";
            this.colPhone.ReadOnly = true;
            // 
            // colTotalPurchases
            // 
            this.colTotalPurchases.DataPropertyName = "total_purchases";
            this.colTotalPurchases.FillWeight = 95F;
            resources.ApplyResources(this.colTotalPurchases, "colTotalPurchases");
            this.colTotalPurchases.Name = "colTotalPurchases";
            this.colTotalPurchases.ReadOnly = true;
            // 
            // colPayable
            // 
            this.colPayable.DataPropertyName = "payable_balance";
            this.colPayable.FillWeight = 95F;
            resources.ApplyResources(this.colPayable, "colPayable");
            this.colPayable.Name = "colPayable";
            this.colPayable.ReadOnly = true;
            // 
            // colCreditDays
            // 
            this.colCreditDays.DataPropertyName = "credit_days";
            this.colCreditDays.FillWeight = 70F;
            resources.ApplyResources(this.colCreditDays, "colCreditDays");
            this.colCreditDays.Name = "colCreditDays";
            this.colCreditDays.ReadOnly = true;
            // 
            // colLastBill
            // 
            this.colLastBill.DataPropertyName = "last_bill_date";
            this.colLastBill.FillWeight = 90F;
            resources.ApplyResources(this.colLastBill, "colLastBill");
            this.colLastBill.Name = "colLastBill";
            this.colLastBill.ReadOnly = true;
            // 
            // colDaysSince
            // 
            this.colDaysSince.DataPropertyName = "days_since_last_purchase";
            this.colDaysSince.FillWeight = 90F;
            resources.ApplyResources(this.colDaysSince, "colDaysSince");
            this.colDaysSince.Name = "colDaysSince";
            this.colDaysSince.ReadOnly = true;
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
            // colAddress
            // 
            this.colAddress.DataPropertyName = "address";
            resources.ApplyResources(this.colAddress, "colAddress");
            this.colAddress.Name = "colAddress";
            this.colAddress.ReadOnly = true;
            // 
            // colCreditLimit
            // 
            this.colCreditLimit.DataPropertyName = "credit_limit";
            resources.ApplyResources(this.colCreditLimit, "colCreditLimit");
            this.colCreditLimit.Name = "colCreditLimit";
            this.colCreditLimit.ReadOnly = true;
            // 
            // colTotalPaid
            // 
            this.colTotalPaid.DataPropertyName = "total_paid";
            resources.ApplyResources(this.colTotalPaid, "colTotalPaid");
            this.colTotalPaid.Name = "colTotalPaid";
            this.colTotalPaid.ReadOnly = true;
            // 
            // colIsOverdue
            // 
            this.colIsOverdue.DataPropertyName = "is_overdue";
            resources.ApplyResources(this.colIsOverdue, "colIsOverdue");
            this.colIsOverdue.Name = "colIsOverdue";
            this.colIsOverdue.ReadOnly = true;
            // 
            // pnlDetails
            // 
            this.pnlDetails.BackColor = System.Drawing.Color.White;
            this.pnlDetails.Controls.Add(this.btnMakePayment);
            this.pnlDetails.Controls.Add(this.btnNewBill);
            this.pnlDetails.Controls.Add(this.btnViewProfile);
            this.pnlDetails.Controls.Add(this.lstBills);
            this.pnlDetails.Controls.Add(this.lblBills);
            this.pnlDetails.Controls.Add(this.lblCreditLimitValue);
            this.pnlDetails.Controls.Add(this.lblCreditLimitTitle);
            this.pnlDetails.Controls.Add(this.lblCreditDaysValue);
            this.pnlDetails.Controls.Add(this.lblCreditDaysTitle);
            this.pnlDetails.Controls.Add(this.lblPayableValue);
            this.pnlDetails.Controls.Add(this.lblPayableTitle);
            this.pnlDetails.Controls.Add(this.lblPaidValue);
            this.pnlDetails.Controls.Add(this.lblPaidTitle);
            this.pnlDetails.Controls.Add(this.lblPurchasedValue);
            this.pnlDetails.Controls.Add(this.lblPurchasedTitle);
            this.pnlDetails.Controls.Add(this.lblCategoryValue);
            this.pnlDetails.Controls.Add(this.lblAddressValue);
            this.pnlDetails.Controls.Add(this.lblPhoneValue);
            this.pnlDetails.Controls.Add(this.panelAvatar);
            this.pnlDetails.Controls.Add(this.lblSupplierHeader);
            resources.ApplyResources(this.pnlDetails, "pnlDetails");
            this.pnlDetails.Name = "pnlDetails";
            // 
            // btnMakePayment
            // 
            resources.ApplyResources(this.btnMakePayment, "btnMakePayment");
            this.btnMakePayment.Name = "btnMakePayment";
            this.btnMakePayment.UseVisualStyleBackColor = true;
            this.btnMakePayment.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnNewBill
            // 
            resources.ApplyResources(this.btnNewBill, "btnNewBill");
            this.btnNewBill.Name = "btnNewBill";
            this.btnNewBill.UseVisualStyleBackColor = true;
            this.btnNewBill.Click += new System.EventHandler(this.btnNewBill_Click);
            // 
            // btnViewProfile
            // 
            resources.ApplyResources(this.btnViewProfile, "btnViewProfile");
            this.btnViewProfile.Name = "btnViewProfile";
            this.btnViewProfile.UseVisualStyleBackColor = true;
            this.btnViewProfile.Click += new System.EventHandler(this.btnViewProfile_Click);
            // 
            // lstBills
            // 
            this.lstBills.FormattingEnabled = true;
            resources.ApplyResources(this.lstBills, "lstBills");
            this.lstBills.Name = "lstBills";
            // 
            // lblBills
            // 
            resources.ApplyResources(this.lblBills, "lblBills");
            this.lblBills.Name = "lblBills";
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
            // lblCreditDaysValue
            // 
            resources.ApplyResources(this.lblCreditDaysValue, "lblCreditDaysValue");
            this.lblCreditDaysValue.Name = "lblCreditDaysValue";
            // 
            // lblCreditDaysTitle
            // 
            resources.ApplyResources(this.lblCreditDaysTitle, "lblCreditDaysTitle");
            this.lblCreditDaysTitle.Name = "lblCreditDaysTitle";
            // 
            // lblPayableValue
            // 
            resources.ApplyResources(this.lblPayableValue, "lblPayableValue");
            this.lblPayableValue.Name = "lblPayableValue";
            // 
            // lblPayableTitle
            // 
            resources.ApplyResources(this.lblPayableTitle, "lblPayableTitle");
            this.lblPayableTitle.Name = "lblPayableTitle";
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
            // lblPurchasedValue
            // 
            resources.ApplyResources(this.lblPurchasedValue, "lblPurchasedValue");
            this.lblPurchasedValue.Name = "lblPurchasedValue";
            // 
            // lblPurchasedTitle
            // 
            resources.ApplyResources(this.lblPurchasedTitle, "lblPurchasedTitle");
            this.lblPurchasedTitle.Name = "lblPurchasedTitle";
            // 
            // lblCategoryValue
            // 
            this.lblCategoryValue.AutoEllipsis = true;
            resources.ApplyResources(this.lblCategoryValue, "lblCategoryValue");
            this.lblCategoryValue.Name = "lblCategoryValue";
            // 
            // lblAddressValue
            // 
            this.lblAddressValue.AutoEllipsis = true;
            resources.ApplyResources(this.lblAddressValue, "lblAddressValue");
            this.lblAddressValue.Name = "lblAddressValue";
            // 
            // lblPhoneValue
            // 
            this.lblPhoneValue.AutoEllipsis = true;
            resources.ApplyResources(this.lblPhoneValue, "lblPhoneValue");
            this.lblPhoneValue.Name = "lblPhoneValue";
            // 
            // panelAvatar
            // 
            resources.ApplyResources(this.panelAvatar, "panelAvatar");
            this.panelAvatar.Name = "panelAvatar";
            this.panelAvatar.Paint += new System.Windows.Forms.PaintEventHandler(this.panelAvatar_Paint);
            // 
            // lblSupplierHeader
            // 
            this.lblSupplierHeader.AutoEllipsis = true;
            resources.ApplyResources(this.lblSupplierHeader, "lblSupplierHeader");
            this.lblSupplierHeader.Name = "lblSupplierHeader";
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.White;
            this.pnlBottom.Controls.Add(this.lblBottomSummary);
            resources.ApplyResources(this.pnlBottom, "pnlBottom");
            this.pnlBottom.Name = "pnlBottom";
            // 
            // lblBottomSummary
            // 
            resources.ApplyResources(this.lblBottomSummary, "lblBottomSummary");
            this.lblBottomSummary.Name = "lblBottomSummary";
            // 
            // detailSlideTimer
            // 
            this.detailSlideTimer.Interval = 15;
            // 
            // frm_suppliers
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Name = "frm_suppliers";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_suppliers_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.tableKpis.ResumeLayout(false);
            this.cardTotalSuppliers.ResumeLayout(false);
            this.cardTotalSuppliers.PerformLayout();
            this.cardTotalPayables.ResumeLayout(false);
            this.cardTotalPayables.PerformLayout();
            this.cardOverduePayables.ResumeLayout(false);
            this.cardOverduePayables.PerformLayout();
            this.cardThisMonth.ResumeLayout(false);
            this.cardThisMonth.PerformLayout();
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_suppliers)).EndInit();
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel tableKpis;
        private System.Windows.Forms.Panel cardTotalSuppliers;
        private System.Windows.Forms.Label lblTotalSuppliersValue;
        private System.Windows.Forms.Label lblTotalSuppliersTitle;
        private System.Windows.Forms.Panel cardTotalPayables;
        private System.Windows.Forms.Label lblTotalPayablesValue;
        private System.Windows.Forms.Label lblTotalPayablesTitle;
        private System.Windows.Forms.Panel cardOverduePayables;
        private System.Windows.Forms.Label lblOverduePayablesValue;
        private System.Windows.Forms.Label lblOverduePayablesTitle;
        private System.Windows.Forms.Panel cardThisMonth;
        private System.Windows.Forms.Label lblThisMonthValue;
        private System.Windows.Forms.Label lblThisMonthTitle;
        private System.Windows.Forms.Label lblThisMonthTrend;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label lblFilterStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblFilterCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblFilterSearch;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.DataGridView grid_suppliers;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Panel panelAvatar;
        private System.Windows.Forms.Label lblSupplierHeader;
        private System.Windows.Forms.Label lblPhoneValue;
        private System.Windows.Forms.Label lblAddressValue;
        private System.Windows.Forms.Label lblCategoryValue;
        private System.Windows.Forms.Label lblPurchasedTitle;
        private System.Windows.Forms.Label lblPurchasedValue;
        private System.Windows.Forms.Label lblPaidTitle;
        private System.Windows.Forms.Label lblPaidValue;
        private System.Windows.Forms.Label lblPayableTitle;
        private System.Windows.Forms.Label lblPayableValue;
        private System.Windows.Forms.Label lblCreditDaysTitle;
        private System.Windows.Forms.Label lblCreditDaysValue;
        private System.Windows.Forms.Label lblCreditLimitTitle;
        private System.Windows.Forms.Label lblCreditLimitValue;
        private System.Windows.Forms.Label lblBills;
        private System.Windows.Forms.ListBox lstBills;
        private System.Windows.Forms.Button btnViewProfile;
        private System.Windows.Forms.Button btnNewBill;
        private System.Windows.Forms.Button btnMakePayment;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblBottomSummary;
        private System.Windows.Forms.Timer detailSlideTimer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalPurchases;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPayable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreditDays;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastBill;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDaysSince;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreditLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsOverdue;
    }
}

