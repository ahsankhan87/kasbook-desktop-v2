namespace pos
{
    partial class frm_supplier_detail
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea17 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend17 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series17 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea18 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend18 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series18 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea19 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend19 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series19 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea20 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend20 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series20 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lbl_title = new System.Windows.Forms.Label();
            this.tabProfile = new System.Windows.Forms.TabControl();
            this.tabOverview = new System.Windows.Forms.TabPage();
            this.splitOverview = new System.Windows.Forms.SplitContainer();
            this.gridTopItemsSmall = new System.Windows.Forms.DataGridView();
            this.colTopItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTopItemQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTopItemValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTopItemsSmallTitle = new System.Windows.Forms.Label();
            this.chartMonthlyPurchases = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableKpi = new System.Windows.Forms.TableLayoutPanel();
            this.pnlKpi1 = new System.Windows.Forms.Panel();
            this.lblLifetimePurchasesValue = new System.Windows.Forms.Label();
            this.lblLifetimePurchasesTitle = new System.Windows.Forms.Label();
            this.pnlKpi2 = new System.Windows.Forms.Panel();
            this.lblTotalPaidValue = new System.Windows.Forms.Label();
            this.lblTotalPaidTitle = new System.Windows.Forms.Label();
            this.pnlKpi3 = new System.Windows.Forms.Panel();
            this.lblCurrentPayableValue = new System.Windows.Forms.Label();
            this.lblCurrentPayableTitle = new System.Windows.Forms.Label();
            this.pnlKpi4 = new System.Windows.Forms.Panel();
            this.lblAvailableCreditValue = new System.Windows.Forms.Label();
            this.lblAvailableCreditTitle = new System.Windows.Forms.Label();
            this.pnlOverviewHeader = new System.Windows.Forms.Panel();
            this.lblHeaderCreditDays = new System.Windows.Forms.Label();
            this.lblHeaderEmail = new System.Windows.Forms.Label();
            this.lblHeaderPhone = new System.Windows.Forms.Label();
            this.lblHeaderCategory = new System.Windows.Forms.Label();
            this.lblHeaderCode = new System.Windows.Forms.Label();
            this.lblHeaderName = new System.Windows.Forms.Label();
            this.pnlAvatar = new System.Windows.Forms.Panel();
            this.tabLedger = new System.Windows.Forms.TabPage();
            this.gridLedger = new System.Windows.Forms.DataGridView();
            this.colLedgerDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerRef = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerDebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlLedgerFooter = new System.Windows.Forms.Panel();
            this.lblLedgerTotals = new System.Windows.Forms.Label();
            this.btnPrintStatement = new System.Windows.Forms.Button();
            this.pnlLedgerFilter = new System.Windows.Forms.Panel();
            this.btnLoadLedger = new System.Windows.Forms.Button();
            this.dtLedgerTo = new System.Windows.Forms.DateTimePicker();
            this.lblLedgerTo = new System.Windows.Forms.Label();
            this.dtLedgerFrom = new System.Windows.Forms.DateTimePicker();
            this.lblLedgerFrom = new System.Windows.Forms.Label();
            this.tabOutstanding = new System.Windows.Forms.TabPage();
            this.gridOutstanding = new System.Windows.Forms.DataGridView();
            this.colOutBillNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutBillDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutDueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutPaid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlOutstandingBottom = new System.Windows.Forms.Panel();
            this.btnMakePayment = new System.Windows.Forms.Button();
            this.flowAging = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlAgingCurrent = new System.Windows.Forms.Panel();
            this.lblAgeCurrentValue = new System.Windows.Forms.Label();
            this.lblAgeCurrentTitle = new System.Windows.Forms.Label();
            this.pnlAging1_30 = new System.Windows.Forms.Panel();
            this.lblAge1_30Value = new System.Windows.Forms.Label();
            this.lblAge1_30Title = new System.Windows.Forms.Label();
            this.pnlAging31_60 = new System.Windows.Forms.Panel();
            this.lblAge31_60Value = new System.Windows.Forms.Label();
            this.lblAge31_60Title = new System.Windows.Forms.Label();
            this.pnlAging61_90 = new System.Windows.Forms.Panel();
            this.lblAge61_90Value = new System.Windows.Forms.Label();
            this.lblAge61_90Title = new System.Windows.Forms.Label();
            this.pnlAging90Plus = new System.Windows.Forms.Panel();
            this.lblAge90PlusValue = new System.Windows.Forms.Label();
            this.lblAge90PlusTitle = new System.Windows.Forms.Label();
            this.tabAnalytics = new System.Windows.Forms.TabPage();
            this.tableAnalytics = new System.Windows.Forms.TableLayoutPanel();
            this.pnlPriceHistoryFilter = new System.Windows.Forms.Panel();
            this.lblPriceProduct = new System.Windows.Forms.Label();
            this.cmbPriceProduct = new System.Windows.Forms.ComboBox();
            this.chartPriceHistory = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartMonthlyTrend24 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartTopProducts = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Btn_ledger_report = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            this.tabProfile.SuspendLayout();
            this.tabOverview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitOverview)).BeginInit();
            this.splitOverview.Panel1.SuspendLayout();
            this.splitOverview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTopItemsSmall)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthlyPurchases)).BeginInit();
            this.tableKpi.SuspendLayout();
            this.pnlKpi1.SuspendLayout();
            this.pnlKpi2.SuspendLayout();
            this.pnlKpi3.SuspendLayout();
            this.pnlKpi4.SuspendLayout();
            this.pnlOverviewHeader.SuspendLayout();
            this.tabLedger.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLedger)).BeginInit();
            this.pnlLedgerFooter.SuspendLayout();
            this.pnlLedgerFilter.SuspendLayout();
            this.tabOutstanding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridOutstanding)).BeginInit();
            this.pnlOutstandingBottom.SuspendLayout();
            this.flowAging.SuspendLayout();
            this.pnlAgingCurrent.SuspendLayout();
            this.pnlAging1_30.SuspendLayout();
            this.pnlAging31_60.SuspendLayout();
            this.pnlAging61_90.SuspendLayout();
            this.pnlAging90Plus.SuspendLayout();
            this.tabAnalytics.SuspendLayout();
            this.tableAnalytics.SuspendLayout();
            this.pnlPriceHistoryFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPriceHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthlyTrend24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTopProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.lbl_title);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.panelTop.Size = new System.Drawing.Size(1032, 42);
            this.panelTop.TabIndex = 0;
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_title.Location = new System.Drawing.Point(10, 10);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(182, 28);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "Individual Supplier";
            // 
            // tabProfile
            // 
            this.tabProfile.Controls.Add(this.tabOverview);
            this.tabProfile.Controls.Add(this.tabLedger);
            this.tabProfile.Controls.Add(this.tabOutstanding);
            this.tabProfile.Controls.Add(this.tabAnalytics);
            this.tabProfile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabProfile.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabProfile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabProfile.ItemSize = new System.Drawing.Size(190, 34);
            this.tabProfile.Location = new System.Drawing.Point(0, 42);
            this.tabProfile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabProfile.Multiline = true;
            this.tabProfile.Name = "tabProfile";
            this.tabProfile.SelectedIndex = 0;
            this.tabProfile.Size = new System.Drawing.Size(1032, 474);
            this.tabProfile.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabProfile.TabIndex = 1;
            this.tabProfile.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabProfile_DrawItem);
            // 
            // tabOverview
            // 
            this.tabOverview.BackColor = System.Drawing.Color.White;
            this.tabOverview.Controls.Add(this.splitOverview);
            this.tabOverview.Controls.Add(this.chartMonthlyPurchases);
            this.tabOverview.Controls.Add(this.tableKpi);
            this.tabOverview.Controls.Add(this.pnlOverviewHeader);
            this.tabOverview.Location = new System.Drawing.Point(4, 38);
            this.tabOverview.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabOverview.Name = "tabOverview";
            this.tabOverview.Padding = new System.Windows.Forms.Padding(9, 8, 9, 8);
            this.tabOverview.Size = new System.Drawing.Size(1024, 432);
            this.tabOverview.TabIndex = 0;
            this.tabOverview.Text = "Overview";
            // 
            // splitOverview
            // 
            this.splitOverview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitOverview.Location = new System.Drawing.Point(9, 260);
            this.splitOverview.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitOverview.Name = "splitOverview";
            // 
            // splitOverview.Panel1
            // 
            this.splitOverview.Panel1.Controls.Add(this.gridTopItemsSmall);
            this.splitOverview.Panel1.Controls.Add(this.lblTopItemsSmallTitle);
            this.splitOverview.Size = new System.Drawing.Size(1006, 164);
            this.splitOverview.SplitterDistance = 375;
            this.splitOverview.TabIndex = 3;
            // 
            // gridTopItemsSmall
            // 
            this.gridTopItemsSmall.AllowUserToAddRows = false;
            this.gridTopItemsSmall.AllowUserToDeleteRows = false;
            this.gridTopItemsSmall.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridTopItemsSmall.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTopItemsSmall.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTopItemName,
            this.colTopItemQty,
            this.colTopItemValue});
            this.gridTopItemsSmall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTopItemsSmall.Location = new System.Drawing.Point(0, 19);
            this.gridTopItemsSmall.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridTopItemsSmall.Name = "gridTopItemsSmall";
            this.gridTopItemsSmall.ReadOnly = true;
            this.gridTopItemsSmall.RowHeadersVisible = false;
            this.gridTopItemsSmall.RowHeadersWidth = 51;
            this.gridTopItemsSmall.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTopItemsSmall.Size = new System.Drawing.Size(375, 145);
            this.gridTopItemsSmall.TabIndex = 1;
            // 
            // colTopItemName
            // 
            this.colTopItemName.DataPropertyName = "item_name";
            this.colTopItemName.HeaderText = "Item";
            this.colTopItemName.MinimumWidth = 6;
            this.colTopItemName.Name = "colTopItemName";
            this.colTopItemName.ReadOnly = true;
            // 
            // colTopItemQty
            // 
            this.colTopItemQty.DataPropertyName = "qty";
            this.colTopItemQty.HeaderText = "Qty";
            this.colTopItemQty.MinimumWidth = 6;
            this.colTopItemQty.Name = "colTopItemQty";
            this.colTopItemQty.ReadOnly = true;
            // 
            // colTopItemValue
            // 
            this.colTopItemValue.DataPropertyName = "total_value";
            this.colTopItemValue.HeaderText = "Total Value";
            this.colTopItemValue.MinimumWidth = 6;
            this.colTopItemValue.Name = "colTopItemValue";
            this.colTopItemValue.ReadOnly = true;
            // 
            // lblTopItemsSmallTitle
            // 
            this.lblTopItemsSmallTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTopItemsSmallTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblTopItemsSmallTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTopItemsSmallTitle.Name = "lblTopItemsSmallTitle";
            this.lblTopItemsSmallTitle.Size = new System.Drawing.Size(375, 19);
            this.lblTopItemsSmallTitle.TabIndex = 0;
            this.lblTopItemsSmallTitle.Text = "Top 5 purchased items";
            this.lblTopItemsSmallTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chartMonthlyPurchases
            // 
            chartArea17.Name = "ChartArea1";
            this.chartMonthlyPurchases.ChartAreas.Add(chartArea17);
            this.chartMonthlyPurchases.Dock = System.Windows.Forms.DockStyle.Fill;
            legend17.Enabled = false;
            legend17.Name = "Legend1";
            this.chartMonthlyPurchases.Legends.Add(legend17);
            this.chartMonthlyPurchases.Location = new System.Drawing.Point(9, 260);
            this.chartMonthlyPurchases.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chartMonthlyPurchases.Name = "chartMonthlyPurchases";
            series17.ChartArea = "ChartArea1";
            series17.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(105)))), ((int)(((byte)(92)))));
            series17.Legend = "Legend1";
            series17.Name = "Monthly";
            series17.XValueMember = "month_label";
            series17.YValueMembers = "amount";
            this.chartMonthlyPurchases.Series.Add(series17);
            this.chartMonthlyPurchases.Size = new System.Drawing.Size(1006, 164);
            this.chartMonthlyPurchases.TabIndex = 0;
            // 
            // tableKpi
            // 
            this.tableKpi.ColumnCount = 4;
            this.tableKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableKpi.Controls.Add(this.pnlKpi1, 0, 0);
            this.tableKpi.Controls.Add(this.pnlKpi2, 1, 0);
            this.tableKpi.Controls.Add(this.pnlKpi3, 2, 0);
            this.tableKpi.Controls.Add(this.pnlKpi4, 3, 0);
            this.tableKpi.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableKpi.Location = new System.Drawing.Point(9, 164);
            this.tableKpi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableKpi.Name = "tableKpi";
            this.tableKpi.RowCount = 1;
            this.tableKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableKpi.Size = new System.Drawing.Size(1006, 96);
            this.tableKpi.TabIndex = 2;
            // 
            // pnlKpi1
            // 
            this.pnlKpi1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpi1.Controls.Add(this.lblLifetimePurchasesValue);
            this.pnlKpi1.Controls.Add(this.lblLifetimePurchasesTitle);
            this.pnlKpi1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKpi1.Location = new System.Drawing.Point(3, 2);
            this.pnlKpi1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlKpi1.Name = "pnlKpi1";
            this.pnlKpi1.Size = new System.Drawing.Size(245, 92);
            this.pnlKpi1.TabIndex = 0;
            // 
            // lblLifetimePurchasesValue
            // 
            this.lblLifetimePurchasesValue.AutoSize = true;
            this.lblLifetimePurchasesValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblLifetimePurchasesValue.Location = new System.Drawing.Point(11, 42);
            this.lblLifetimePurchasesValue.Name = "lblLifetimePurchasesValue";
            this.lblLifetimePurchasesValue.Size = new System.Drawing.Size(59, 32);
            this.lblLifetimePurchasesValue.TabIndex = 1;
            this.lblLifetimePurchasesValue.Text = "0.00";
            // 
            // lblLifetimePurchasesTitle
            // 
            this.lblLifetimePurchasesTitle.AutoSize = true;
            this.lblLifetimePurchasesTitle.Location = new System.Drawing.Point(14, 10);
            this.lblLifetimePurchasesTitle.Name = "lblLifetimePurchasesTitle";
            this.lblLifetimePurchasesTitle.Size = new System.Drawing.Size(131, 20);
            this.lblLifetimePurchasesTitle.TabIndex = 0;
            this.lblLifetimePurchasesTitle.Text = "Lifetime Purchases";
            // 
            // pnlKpi2
            // 
            this.pnlKpi2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpi2.Controls.Add(this.lblTotalPaidValue);
            this.pnlKpi2.Controls.Add(this.lblTotalPaidTitle);
            this.pnlKpi2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKpi2.Location = new System.Drawing.Point(254, 2);
            this.pnlKpi2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlKpi2.Name = "pnlKpi2";
            this.pnlKpi2.Size = new System.Drawing.Size(245, 92);
            this.pnlKpi2.TabIndex = 1;
            // 
            // lblTotalPaidValue
            // 
            this.lblTotalPaidValue.AutoSize = true;
            this.lblTotalPaidValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalPaidValue.Location = new System.Drawing.Point(12, 42);
            this.lblTotalPaidValue.Name = "lblTotalPaidValue";
            this.lblTotalPaidValue.Size = new System.Drawing.Size(59, 32);
            this.lblTotalPaidValue.TabIndex = 1;
            this.lblTotalPaidValue.Text = "0.00";
            // 
            // lblTotalPaidTitle
            // 
            this.lblTotalPaidTitle.AutoSize = true;
            this.lblTotalPaidTitle.Location = new System.Drawing.Point(15, 10);
            this.lblTotalPaidTitle.Name = "lblTotalPaidTitle";
            this.lblTotalPaidTitle.Size = new System.Drawing.Size(74, 20);
            this.lblTotalPaidTitle.TabIndex = 0;
            this.lblTotalPaidTitle.Text = "Total Paid";
            // 
            // pnlKpi3
            // 
            this.pnlKpi3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpi3.Controls.Add(this.lblCurrentPayableValue);
            this.pnlKpi3.Controls.Add(this.lblCurrentPayableTitle);
            this.pnlKpi3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKpi3.Location = new System.Drawing.Point(505, 2);
            this.pnlKpi3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlKpi3.Name = "pnlKpi3";
            this.pnlKpi3.Size = new System.Drawing.Size(245, 92);
            this.pnlKpi3.TabIndex = 2;
            // 
            // lblCurrentPayableValue
            // 
            this.lblCurrentPayableValue.AutoSize = true;
            this.lblCurrentPayableValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblCurrentPayableValue.ForeColor = System.Drawing.Color.Firebrick;
            this.lblCurrentPayableValue.Location = new System.Drawing.Point(12, 42);
            this.lblCurrentPayableValue.Name = "lblCurrentPayableValue";
            this.lblCurrentPayableValue.Size = new System.Drawing.Size(59, 32);
            this.lblCurrentPayableValue.TabIndex = 1;
            this.lblCurrentPayableValue.Text = "0.00";
            // 
            // lblCurrentPayableTitle
            // 
            this.lblCurrentPayableTitle.AutoSize = true;
            this.lblCurrentPayableTitle.Location = new System.Drawing.Point(15, 10);
            this.lblCurrentPayableTitle.Name = "lblCurrentPayableTitle";
            this.lblCurrentPayableTitle.Size = new System.Drawing.Size(112, 20);
            this.lblCurrentPayableTitle.TabIndex = 0;
            this.lblCurrentPayableTitle.Text = "Current Payable";
            // 
            // pnlKpi4
            // 
            this.pnlKpi4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlKpi4.Controls.Add(this.lblAvailableCreditValue);
            this.pnlKpi4.Controls.Add(this.lblAvailableCreditTitle);
            this.pnlKpi4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKpi4.Location = new System.Drawing.Point(756, 2);
            this.pnlKpi4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlKpi4.Name = "pnlKpi4";
            this.pnlKpi4.Size = new System.Drawing.Size(247, 92);
            this.pnlKpi4.TabIndex = 3;
            // 
            // lblAvailableCreditValue
            // 
            this.lblAvailableCreditValue.AutoSize = true;
            this.lblAvailableCreditValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblAvailableCreditValue.Location = new System.Drawing.Point(12, 42);
            this.lblAvailableCreditValue.Name = "lblAvailableCreditValue";
            this.lblAvailableCreditValue.Size = new System.Drawing.Size(59, 32);
            this.lblAvailableCreditValue.TabIndex = 1;
            this.lblAvailableCreditValue.Text = "0.00";
            // 
            // lblAvailableCreditTitle
            // 
            this.lblAvailableCreditTitle.AutoSize = true;
            this.lblAvailableCreditTitle.Location = new System.Drawing.Point(15, 10);
            this.lblAvailableCreditTitle.Name = "lblAvailableCreditTitle";
            this.lblAvailableCreditTitle.Size = new System.Drawing.Size(115, 20);
            this.lblAvailableCreditTitle.TabIndex = 0;
            this.lblAvailableCreditTitle.Text = "Available Credit";
            // 
            // pnlOverviewHeader
            // 
            this.pnlOverviewHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOverviewHeader.Controls.Add(this.lblHeaderCreditDays);
            this.pnlOverviewHeader.Controls.Add(this.lblHeaderEmail);
            this.pnlOverviewHeader.Controls.Add(this.lblHeaderPhone);
            this.pnlOverviewHeader.Controls.Add(this.lblHeaderCategory);
            this.pnlOverviewHeader.Controls.Add(this.lblHeaderCode);
            this.pnlOverviewHeader.Controls.Add(this.lblHeaderName);
            this.pnlOverviewHeader.Controls.Add(this.pnlAvatar);
            this.pnlOverviewHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOverviewHeader.Location = new System.Drawing.Point(9, 8);
            this.pnlOverviewHeader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlOverviewHeader.Name = "pnlOverviewHeader";
            this.pnlOverviewHeader.Size = new System.Drawing.Size(1006, 156);
            this.pnlOverviewHeader.TabIndex = 1;
            // 
            // lblHeaderCreditDays
            // 
            this.lblHeaderCreditDays.AutoSize = true;
            this.lblHeaderCreditDays.Location = new System.Drawing.Point(129, 127);
            this.lblHeaderCreditDays.Name = "lblHeaderCreditDays";
            this.lblHeaderCreditDays.Size = new System.Drawing.Size(107, 20);
            this.lblHeaderCreditDays.TabIndex = 6;
            this.lblHeaderCreditDays.Text = "Credit Terms: 0";
            // 
            // lblHeaderEmail
            // 
            this.lblHeaderEmail.AutoSize = true;
            this.lblHeaderEmail.Location = new System.Drawing.Point(129, 106);
            this.lblHeaderEmail.Name = "lblHeaderEmail";
            this.lblHeaderEmail.Size = new System.Drawing.Size(113, 20);
            this.lblHeaderEmail.TabIndex = 5;
            this.lblHeaderEmail.Text = "Email: no-email";
            // 
            // lblHeaderPhone
            // 
            this.lblHeaderPhone.AutoSize = true;
            this.lblHeaderPhone.Location = new System.Drawing.Point(129, 84);
            this.lblHeaderPhone.Name = "lblHeaderPhone";
            this.lblHeaderPhone.Size = new System.Drawing.Size(63, 20);
            this.lblHeaderPhone.TabIndex = 4;
            this.lblHeaderPhone.Text = "Phone: -";
            // 
            // lblHeaderCategory
            // 
            this.lblHeaderCategory.AutoSize = true;
            this.lblHeaderCategory.Location = new System.Drawing.Point(368, 84);
            this.lblHeaderCategory.Name = "lblHeaderCategory";
            this.lblHeaderCategory.Size = new System.Drawing.Size(82, 20);
            this.lblHeaderCategory.TabIndex = 3;
            this.lblHeaderCategory.Text = "Category: -";
            // 
            // lblHeaderCode
            // 
            this.lblHeaderCode.AutoSize = true;
            this.lblHeaderCode.Location = new System.Drawing.Point(368, 53);
            this.lblHeaderCode.Name = "lblHeaderCode";
            this.lblHeaderCode.Size = new System.Drawing.Size(99, 20);
            this.lblHeaderCode.TabIndex = 2;
            this.lblHeaderCode.Text = "Code: S00000";
            // 
            // lblHeaderName
            // 
            this.lblHeaderName.AutoSize = true;
            this.lblHeaderName.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblHeaderName.Location = new System.Drawing.Point(127, 46);
            this.lblHeaderName.Name = "lblHeaderName";
            this.lblHeaderName.Size = new System.Drawing.Size(175, 32);
            this.lblHeaderName.TabIndex = 1;
            this.lblHeaderName.Text = "Supplier Name";
            // 
            // pnlAvatar
            // 
            this.pnlAvatar.Location = new System.Drawing.Point(15, 38);
            this.pnlAvatar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlAvatar.Name = "pnlAvatar";
            this.pnlAvatar.Size = new System.Drawing.Size(96, 91);
            this.pnlAvatar.TabIndex = 0;
            this.pnlAvatar.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlAvatar_Paint);
            // 
            // tabLedger
            // 
            this.tabLedger.BackColor = System.Drawing.Color.White;
            this.tabLedger.Controls.Add(this.gridLedger);
            this.tabLedger.Controls.Add(this.pnlLedgerFooter);
            this.tabLedger.Controls.Add(this.pnlLedgerFilter);
            this.tabLedger.Location = new System.Drawing.Point(4, 38);
            this.tabLedger.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabLedger.Name = "tabLedger";
            this.tabLedger.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tabLedger.Size = new System.Drawing.Size(1024, 432);
            this.tabLedger.TabIndex = 1;
            this.tabLedger.Text = "Account Ledger";
            // 
            // gridLedger
            // 
            this.gridLedger.AllowUserToAddRows = false;
            this.gridLedger.AllowUserToDeleteRows = false;
            this.gridLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLedger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLedgerDate,
            this.colLedgerType,
            this.colLedgerRef,
            this.colLedgerDebit,
            this.colLedgerCredit,
            this.colLedgerBalance});
            this.gridLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLedger.Location = new System.Drawing.Point(7, 44);
            this.gridLedger.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridLedger.Name = "gridLedger";
            this.gridLedger.ReadOnly = true;
            this.gridLedger.RowHeadersVisible = false;
            this.gridLedger.RowHeadersWidth = 51;
            this.gridLedger.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLedger.Size = new System.Drawing.Size(1010, 347);
            this.gridLedger.TabIndex = 1;
            // 
            // colLedgerDate
            // 
            this.colLedgerDate.DataPropertyName = "entry_date";
            this.colLedgerDate.HeaderText = "Date";
            this.colLedgerDate.MinimumWidth = 6;
            this.colLedgerDate.Name = "colLedgerDate";
            this.colLedgerDate.ReadOnly = true;
            // 
            // colLedgerType
            // 
            this.colLedgerType.DataPropertyName = "trans_type";
            this.colLedgerType.HeaderText = "Type";
            this.colLedgerType.MinimumWidth = 6;
            this.colLedgerType.Name = "colLedgerType";
            this.colLedgerType.ReadOnly = true;
            // 
            // colLedgerRef
            // 
            this.colLedgerRef.DataPropertyName = "reference_no";
            this.colLedgerRef.HeaderText = "Reference No";
            this.colLedgerRef.MinimumWidth = 6;
            this.colLedgerRef.Name = "colLedgerRef";
            this.colLedgerRef.ReadOnly = true;
            // 
            // colLedgerDebit
            // 
            this.colLedgerDebit.DataPropertyName = "debit";
            this.colLedgerDebit.HeaderText = "Debit (Payment made)";
            this.colLedgerDebit.MinimumWidth = 6;
            this.colLedgerDebit.Name = "colLedgerDebit";
            this.colLedgerDebit.ReadOnly = true;
            // 
            // colLedgerCredit
            // 
            this.colLedgerCredit.DataPropertyName = "credit";
            this.colLedgerCredit.HeaderText = "Credit (Bill received)";
            this.colLedgerCredit.MinimumWidth = 6;
            this.colLedgerCredit.Name = "colLedgerCredit";
            this.colLedgerCredit.ReadOnly = true;
            // 
            // colLedgerBalance
            // 
            this.colLedgerBalance.DataPropertyName = "running_balance";
            this.colLedgerBalance.HeaderText = "Running Balance";
            this.colLedgerBalance.MinimumWidth = 6;
            this.colLedgerBalance.Name = "colLedgerBalance";
            this.colLedgerBalance.ReadOnly = true;
            // 
            // pnlLedgerFooter
            // 
            this.pnlLedgerFooter.Controls.Add(this.Btn_ledger_report);
            this.pnlLedgerFooter.Controls.Add(this.lblLedgerTotals);
            this.pnlLedgerFooter.Controls.Add(this.btnPrintStatement);
            this.pnlLedgerFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLedgerFooter.Location = new System.Drawing.Point(7, 391);
            this.pnlLedgerFooter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlLedgerFooter.Name = "pnlLedgerFooter";
            this.pnlLedgerFooter.Size = new System.Drawing.Size(1010, 35);
            this.pnlLedgerFooter.TabIndex = 2;
            // 
            // lblLedgerTotals
            // 
            this.lblLedgerTotals.AutoSize = true;
            this.lblLedgerTotals.Location = new System.Drawing.Point(3, 6);
            this.lblLedgerTotals.Name = "lblLedgerTotals";
            this.lblLedgerTotals.Size = new System.Drawing.Size(206, 20);
            this.lblLedgerTotals.TabIndex = 0;
            this.lblLedgerTotals.Text = "Totals: DR 0.00 | CR 0.00 | 0.00";
            // 
            // btnPrintStatement
            // 
            this.btnPrintStatement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintStatement.Location = new System.Drawing.Point(896, 2);
            this.btnPrintStatement.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPrintStatement.Name = "btnPrintStatement";
            this.btnPrintStatement.Size = new System.Drawing.Size(110, 31);
            this.btnPrintStatement.TabIndex = 1;
            this.btnPrintStatement.Text = "Print Statement";
            this.btnPrintStatement.UseVisualStyleBackColor = true;
            this.btnPrintStatement.Click += new System.EventHandler(this.btnPrintStatement_Click);
            // 
            // pnlLedgerFilter
            // 
            this.pnlLedgerFilter.Controls.Add(this.btnLoadLedger);
            this.pnlLedgerFilter.Controls.Add(this.dtLedgerTo);
            this.pnlLedgerFilter.Controls.Add(this.lblLedgerTo);
            this.pnlLedgerFilter.Controls.Add(this.dtLedgerFrom);
            this.pnlLedgerFilter.Controls.Add(this.lblLedgerFrom);
            this.pnlLedgerFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLedgerFilter.Location = new System.Drawing.Point(7, 6);
            this.pnlLedgerFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlLedgerFilter.Name = "pnlLedgerFilter";
            this.pnlLedgerFilter.Size = new System.Drawing.Size(1010, 38);
            this.pnlLedgerFilter.TabIndex = 0;
            // 
            // btnLoadLedger
            // 
            this.btnLoadLedger.Location = new System.Drawing.Point(487, 9);
            this.btnLoadLedger.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLoadLedger.Name = "btnLoadLedger";
            this.btnLoadLedger.Size = new System.Drawing.Size(79, 27);
            this.btnLoadLedger.TabIndex = 4;
            this.btnLoadLedger.Text = "Load";
            this.btnLoadLedger.UseVisualStyleBackColor = true;
            this.btnLoadLedger.Click += new System.EventHandler(this.btnLoadLedger_Click);
            // 
            // dtLedgerTo
            // 
            this.dtLedgerTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtLedgerTo.Location = new System.Drawing.Point(349, 10);
            this.dtLedgerTo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtLedgerTo.Name = "dtLedgerTo";
            this.dtLedgerTo.Size = new System.Drawing.Size(123, 27);
            this.dtLedgerTo.TabIndex = 3;
            // 
            // lblLedgerTo
            // 
            this.lblLedgerTo.AutoSize = true;
            this.lblLedgerTo.Location = new System.Drawing.Point(322, 13);
            this.lblLedgerTo.Name = "lblLedgerTo";
            this.lblLedgerTo.Size = new System.Drawing.Size(25, 20);
            this.lblLedgerTo.TabIndex = 2;
            this.lblLedgerTo.Text = "To";
            // 
            // dtLedgerFrom
            // 
            this.dtLedgerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtLedgerFrom.Location = new System.Drawing.Point(187, 10);
            this.dtLedgerFrom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtLedgerFrom.Name = "dtLedgerFrom";
            this.dtLedgerFrom.Size = new System.Drawing.Size(123, 27);
            this.dtLedgerFrom.TabIndex = 1;
            // 
            // lblLedgerFrom
            // 
            this.lblLedgerFrom.AutoSize = true;
            this.lblLedgerFrom.Location = new System.Drawing.Point(147, 13);
            this.lblLedgerFrom.Name = "lblLedgerFrom";
            this.lblLedgerFrom.Size = new System.Drawing.Size(43, 20);
            this.lblLedgerFrom.TabIndex = 0;
            this.lblLedgerFrom.Text = "From";
            // 
            // tabOutstanding
            // 
            this.tabOutstanding.BackColor = System.Drawing.Color.White;
            this.tabOutstanding.Controls.Add(this.gridOutstanding);
            this.tabOutstanding.Controls.Add(this.pnlOutstandingBottom);
            this.tabOutstanding.Location = new System.Drawing.Point(4, 38);
            this.tabOutstanding.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabOutstanding.Name = "tabOutstanding";
            this.tabOutstanding.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tabOutstanding.Size = new System.Drawing.Size(1024, 432);
            this.tabOutstanding.TabIndex = 2;
            this.tabOutstanding.Text = "Outstanding Bills";
            // 
            // gridOutstanding
            // 
            this.gridOutstanding.AllowUserToAddRows = false;
            this.gridOutstanding.AllowUserToDeleteRows = false;
            this.gridOutstanding.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridOutstanding.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridOutstanding.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOutBillNo,
            this.colOutBillDate,
            this.colOutDueDate,
            this.colOutAmount,
            this.colOutPaid,
            this.colOutBalance,
            this.colOutDays});
            this.gridOutstanding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridOutstanding.Location = new System.Drawing.Point(7, 6);
            this.gridOutstanding.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridOutstanding.Name = "gridOutstanding";
            this.gridOutstanding.ReadOnly = true;
            this.gridOutstanding.RowHeadersVisible = false;
            this.gridOutstanding.RowHeadersWidth = 51;
            this.gridOutstanding.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridOutstanding.Size = new System.Drawing.Size(1010, 366);
            this.gridOutstanding.TabIndex = 0;
            // 
            // colOutBillNo
            // 
            this.colOutBillNo.DataPropertyName = "invoice_no";
            this.colOutBillNo.HeaderText = "Bill No";
            this.colOutBillNo.MinimumWidth = 6;
            this.colOutBillNo.Name = "colOutBillNo";
            this.colOutBillNo.ReadOnly = true;
            // 
            // colOutBillDate
            // 
            this.colOutBillDate.DataPropertyName = "purchase_date";
            this.colOutBillDate.HeaderText = "Bill Date";
            this.colOutBillDate.MinimumWidth = 6;
            this.colOutBillDate.Name = "colOutBillDate";
            this.colOutBillDate.ReadOnly = true;
            // 
            // colOutDueDate
            // 
            this.colOutDueDate.DataPropertyName = "due_date";
            this.colOutDueDate.HeaderText = "Due Date";
            this.colOutDueDate.MinimumWidth = 6;
            this.colOutDueDate.Name = "colOutDueDate";
            this.colOutDueDate.ReadOnly = true;
            // 
            // colOutAmount
            // 
            this.colOutAmount.DataPropertyName = "amount";
            this.colOutAmount.HeaderText = "Amount";
            this.colOutAmount.MinimumWidth = 6;
            this.colOutAmount.Name = "colOutAmount";
            this.colOutAmount.ReadOnly = true;
            // 
            // colOutPaid
            // 
            this.colOutPaid.DataPropertyName = "paid";
            this.colOutPaid.HeaderText = "Paid";
            this.colOutPaid.MinimumWidth = 6;
            this.colOutPaid.Name = "colOutPaid";
            this.colOutPaid.ReadOnly = true;
            // 
            // colOutBalance
            // 
            this.colOutBalance.DataPropertyName = "balance";
            this.colOutBalance.HeaderText = "Balance";
            this.colOutBalance.MinimumWidth = 6;
            this.colOutBalance.Name = "colOutBalance";
            this.colOutBalance.ReadOnly = true;
            // 
            // colOutDays
            // 
            this.colOutDays.DataPropertyName = "days_overdue";
            this.colOutDays.HeaderText = "Days Overdue";
            this.colOutDays.MinimumWidth = 6;
            this.colOutDays.Name = "colOutDays";
            this.colOutDays.ReadOnly = true;
            // 
            // pnlOutstandingBottom
            // 
            this.pnlOutstandingBottom.Controls.Add(this.btnMakePayment);
            this.pnlOutstandingBottom.Controls.Add(this.flowAging);
            this.pnlOutstandingBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlOutstandingBottom.Location = new System.Drawing.Point(7, 372);
            this.pnlOutstandingBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlOutstandingBottom.Name = "pnlOutstandingBottom";
            this.pnlOutstandingBottom.Size = new System.Drawing.Size(1010, 54);
            this.pnlOutstandingBottom.TabIndex = 1;
            // 
            // btnMakePayment
            // 
            this.btnMakePayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMakePayment.Location = new System.Drawing.Point(899, 17);
            this.btnMakePayment.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMakePayment.Name = "btnMakePayment";
            this.btnMakePayment.Size = new System.Drawing.Size(108, 26);
            this.btnMakePayment.TabIndex = 1;
            this.btnMakePayment.Text = "Make Payment";
            this.btnMakePayment.UseVisualStyleBackColor = true;
            this.btnMakePayment.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // flowAging
            // 
            this.flowAging.Controls.Add(this.pnlAgingCurrent);
            this.flowAging.Controls.Add(this.pnlAging1_30);
            this.flowAging.Controls.Add(this.pnlAging31_60);
            this.flowAging.Controls.Add(this.pnlAging61_90);
            this.flowAging.Controls.Add(this.pnlAging90Plus);
            this.flowAging.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowAging.Location = new System.Drawing.Point(0, 0);
            this.flowAging.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowAging.Name = "flowAging";
            this.flowAging.Size = new System.Drawing.Size(881, 54);
            this.flowAging.TabIndex = 0;
            // 
            // pnlAgingCurrent
            // 
            this.pnlAgingCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAgingCurrent.Controls.Add(this.lblAgeCurrentValue);
            this.pnlAgingCurrent.Controls.Add(this.lblAgeCurrentTitle);
            this.pnlAgingCurrent.Location = new System.Drawing.Point(3, 2);
            this.pnlAgingCurrent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlAgingCurrent.Name = "pnlAgingCurrent";
            this.pnlAgingCurrent.Size = new System.Drawing.Size(166, 48);
            this.pnlAgingCurrent.TabIndex = 0;
            // 
            // lblAgeCurrentValue
            // 
            this.lblAgeCurrentValue.AutoSize = true;
            this.lblAgeCurrentValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAgeCurrentValue.Location = new System.Drawing.Point(7, 26);
            this.lblAgeCurrentValue.Name = "lblAgeCurrentValue";
            this.lblAgeCurrentValue.Size = new System.Drawing.Size(40, 20);
            this.lblAgeCurrentValue.TabIndex = 1;
            this.lblAgeCurrentValue.Text = "0.00";
            // 
            // lblAgeCurrentTitle
            // 
            this.lblAgeCurrentTitle.AutoSize = true;
            this.lblAgeCurrentTitle.Location = new System.Drawing.Point(7, 6);
            this.lblAgeCurrentTitle.Name = "lblAgeCurrentTitle";
            this.lblAgeCurrentTitle.Size = new System.Drawing.Size(57, 20);
            this.lblAgeCurrentTitle.TabIndex = 0;
            this.lblAgeCurrentTitle.Text = "Current";
            // 
            // pnlAging1_30
            // 
            this.pnlAging1_30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAging1_30.Controls.Add(this.lblAge1_30Value);
            this.pnlAging1_30.Controls.Add(this.lblAge1_30Title);
            this.pnlAging1_30.Location = new System.Drawing.Point(175, 2);
            this.pnlAging1_30.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlAging1_30.Name = "pnlAging1_30";
            this.pnlAging1_30.Size = new System.Drawing.Size(166, 48);
            this.pnlAging1_30.TabIndex = 1;
            // 
            // lblAge1_30Value
            // 
            this.lblAge1_30Value.AutoSize = true;
            this.lblAge1_30Value.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAge1_30Value.Location = new System.Drawing.Point(7, 26);
            this.lblAge1_30Value.Name = "lblAge1_30Value";
            this.lblAge1_30Value.Size = new System.Drawing.Size(40, 20);
            this.lblAge1_30Value.TabIndex = 1;
            this.lblAge1_30Value.Text = "0.00";
            // 
            // lblAge1_30Title
            // 
            this.lblAge1_30Title.AutoSize = true;
            this.lblAge1_30Title.Location = new System.Drawing.Point(7, 6);
            this.lblAge1_30Title.Name = "lblAge1_30Title";
            this.lblAge1_30Title.Size = new System.Drawing.Size(39, 20);
            this.lblAge1_30Title.TabIndex = 0;
            this.lblAge1_30Title.Text = "1-30";
            // 
            // pnlAging31_60
            // 
            this.pnlAging31_60.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAging31_60.Controls.Add(this.lblAge31_60Value);
            this.pnlAging31_60.Controls.Add(this.lblAge31_60Title);
            this.pnlAging31_60.Location = new System.Drawing.Point(347, 2);
            this.pnlAging31_60.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlAging31_60.Name = "pnlAging31_60";
            this.pnlAging31_60.Size = new System.Drawing.Size(166, 48);
            this.pnlAging31_60.TabIndex = 2;
            // 
            // lblAge31_60Value
            // 
            this.lblAge31_60Value.AutoSize = true;
            this.lblAge31_60Value.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAge31_60Value.Location = new System.Drawing.Point(7, 26);
            this.lblAge31_60Value.Name = "lblAge31_60Value";
            this.lblAge31_60Value.Size = new System.Drawing.Size(40, 20);
            this.lblAge31_60Value.TabIndex = 1;
            this.lblAge31_60Value.Text = "0.00";
            // 
            // lblAge31_60Title
            // 
            this.lblAge31_60Title.AutoSize = true;
            this.lblAge31_60Title.Location = new System.Drawing.Point(7, 6);
            this.lblAge31_60Title.Name = "lblAge31_60Title";
            this.lblAge31_60Title.Size = new System.Drawing.Size(47, 20);
            this.lblAge31_60Title.TabIndex = 0;
            this.lblAge31_60Title.Text = "31-60";
            // 
            // pnlAging61_90
            // 
            this.pnlAging61_90.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAging61_90.Controls.Add(this.lblAge61_90Value);
            this.pnlAging61_90.Controls.Add(this.lblAge61_90Title);
            this.pnlAging61_90.Location = new System.Drawing.Point(519, 2);
            this.pnlAging61_90.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlAging61_90.Name = "pnlAging61_90";
            this.pnlAging61_90.Size = new System.Drawing.Size(166, 48);
            this.pnlAging61_90.TabIndex = 3;
            // 
            // lblAge61_90Value
            // 
            this.lblAge61_90Value.AutoSize = true;
            this.lblAge61_90Value.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAge61_90Value.Location = new System.Drawing.Point(7, 26);
            this.lblAge61_90Value.Name = "lblAge61_90Value";
            this.lblAge61_90Value.Size = new System.Drawing.Size(40, 20);
            this.lblAge61_90Value.TabIndex = 1;
            this.lblAge61_90Value.Text = "0.00";
            // 
            // lblAge61_90Title
            // 
            this.lblAge61_90Title.AutoSize = true;
            this.lblAge61_90Title.Location = new System.Drawing.Point(7, 6);
            this.lblAge61_90Title.Name = "lblAge61_90Title";
            this.lblAge61_90Title.Size = new System.Drawing.Size(47, 20);
            this.lblAge61_90Title.TabIndex = 0;
            this.lblAge61_90Title.Text = "61-90";
            // 
            // pnlAging90Plus
            // 
            this.pnlAging90Plus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAging90Plus.Controls.Add(this.lblAge90PlusValue);
            this.pnlAging90Plus.Controls.Add(this.lblAge90PlusTitle);
            this.pnlAging90Plus.Location = new System.Drawing.Point(691, 2);
            this.pnlAging90Plus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlAging90Plus.Name = "pnlAging90Plus";
            this.pnlAging90Plus.Size = new System.Drawing.Size(166, 48);
            this.pnlAging90Plus.TabIndex = 4;
            // 
            // lblAge90PlusValue
            // 
            this.lblAge90PlusValue.AutoSize = true;
            this.lblAge90PlusValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAge90PlusValue.Location = new System.Drawing.Point(7, 26);
            this.lblAge90PlusValue.Name = "lblAge90PlusValue";
            this.lblAge90PlusValue.Size = new System.Drawing.Size(40, 20);
            this.lblAge90PlusValue.TabIndex = 1;
            this.lblAge90PlusValue.Text = "0.00";
            // 
            // lblAge90PlusTitle
            // 
            this.lblAge90PlusTitle.AutoSize = true;
            this.lblAge90PlusTitle.Location = new System.Drawing.Point(7, 6);
            this.lblAge90PlusTitle.Name = "lblAge90PlusTitle";
            this.lblAge90PlusTitle.Size = new System.Drawing.Size(35, 20);
            this.lblAge90PlusTitle.TabIndex = 0;
            this.lblAge90PlusTitle.Text = "90+";
            // 
            // tabAnalytics
            // 
            this.tabAnalytics.BackColor = System.Drawing.Color.White;
            this.tabAnalytics.Controls.Add(this.tableAnalytics);
            this.tabAnalytics.Location = new System.Drawing.Point(4, 38);
            this.tabAnalytics.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabAnalytics.Name = "tabAnalytics";
            this.tabAnalytics.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.tabAnalytics.Size = new System.Drawing.Size(1024, 432);
            this.tabAnalytics.TabIndex = 3;
            this.tabAnalytics.Text = "Purchase History && Analytics";
            // 
            // tableAnalytics
            // 
            this.tableAnalytics.ColumnCount = 2;
            this.tableAnalytics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableAnalytics.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableAnalytics.Controls.Add(this.pnlPriceHistoryFilter, 0, 2);
            this.tableAnalytics.Controls.Add(this.chartPriceHistory, 1, 2);
            this.tableAnalytics.Controls.Add(this.chartMonthlyTrend24, 1, 0);
            this.tableAnalytics.Controls.Add(this.chartTopProducts, 0, 0);
            this.tableAnalytics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableAnalytics.Location = new System.Drawing.Point(7, 6);
            this.tableAnalytics.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableAnalytics.Name = "tableAnalytics";
            this.tableAnalytics.RowCount = 4;
            this.tableAnalytics.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableAnalytics.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tableAnalytics.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableAnalytics.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableAnalytics.Size = new System.Drawing.Size(1010, 420);
            this.tableAnalytics.TabIndex = 0;
            // 
            // pnlPriceHistoryFilter
            // 
            this.pnlPriceHistoryFilter.Controls.Add(this.lblPriceProduct);
            this.pnlPriceHistoryFilter.Controls.Add(this.cmbPriceProduct);
            this.pnlPriceHistoryFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPriceHistoryFilter.Location = new System.Drawing.Point(3, 202);
            this.pnlPriceHistoryFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlPriceHistoryFilter.Name = "pnlPriceHistoryFilter";
            this.pnlPriceHistoryFilter.Size = new System.Drawing.Size(499, 22);
            this.pnlPriceHistoryFilter.TabIndex = 2;
            // 
            // lblPriceProduct
            // 
            this.lblPriceProduct.AutoSize = true;
            this.lblPriceProduct.Location = new System.Drawing.Point(3, 2);
            this.lblPriceProduct.Name = "lblPriceProduct";
            this.lblPriceProduct.Size = new System.Drawing.Size(60, 20);
            this.lblPriceProduct.TabIndex = 0;
            this.lblPriceProduct.Text = "Product";
            // 
            // cmbPriceProduct
            // 
            this.cmbPriceProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPriceProduct.FormattingEnabled = true;
            this.cmbPriceProduct.Location = new System.Drawing.Point(60, 0);
            this.cmbPriceProduct.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbPriceProduct.Name = "cmbPriceProduct";
            this.cmbPriceProduct.Size = new System.Drawing.Size(368, 28);
            this.cmbPriceProduct.TabIndex = 1;
            this.cmbPriceProduct.SelectedIndexChanged += new System.EventHandler(this.cmbPriceProduct_SelectedIndexChanged);
            // 
            // chartPriceHistory
            // 
            chartArea18.Name = "ChartArea1";
            this.chartPriceHistory.ChartAreas.Add(chartArea18);
            this.chartPriceHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            legend18.Enabled = false;
            legend18.Name = "Legend1";
            this.chartPriceHistory.Legends.Add(legend18);
            this.chartPriceHistory.Location = new System.Drawing.Point(508, 202);
            this.chartPriceHistory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chartPriceHistory.Name = "chartPriceHistory";
            this.tableAnalytics.SetRowSpan(this.chartPriceHistory, 2);
            series18.BorderWidth = 2;
            series18.ChartArea = "ChartArea1";
            series18.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(105)))), ((int)(((byte)(92)))));
            series18.Legend = "Legend1";
            series18.Name = "Price";
            series18.XValueMember = "purchase_date";
            series18.YValueMembers = "unit_cost";
            this.chartPriceHistory.Series.Add(series18);
            this.chartPriceHistory.Size = new System.Drawing.Size(499, 216);
            this.chartPriceHistory.TabIndex = 3;
            // 
            // chartMonthlyTrend24
            // 
            chartArea19.Name = "ChartArea1";
            this.chartMonthlyTrend24.ChartAreas.Add(chartArea19);
            this.chartMonthlyTrend24.Dock = System.Windows.Forms.DockStyle.Fill;
            legend19.Enabled = false;
            legend19.Name = "Legend1";
            this.chartMonthlyTrend24.Legends.Add(legend19);
            this.chartMonthlyTrend24.Location = new System.Drawing.Point(508, 2);
            this.chartMonthlyTrend24.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chartMonthlyTrend24.Name = "chartMonthlyTrend24";
            series19.BorderWidth = 2;
            series19.ChartArea = "ChartArea1";
            series19.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(77)))), ((int)(((byte)(64)))));
            series19.Legend = "Legend1";
            series19.Name = "Trend";
            series19.XValueMember = "month_label";
            series19.YValueMembers = "amount";
            this.chartMonthlyTrend24.Series.Add(series19);
            this.chartMonthlyTrend24.Size = new System.Drawing.Size(499, 190);
            this.chartMonthlyTrend24.TabIndex = 1;
            // 
            // chartTopProducts
            // 
            chartArea20.Name = "ChartArea1";
            this.chartTopProducts.ChartAreas.Add(chartArea20);
            this.chartTopProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            legend20.Enabled = false;
            legend20.Name = "Legend1";
            this.chartTopProducts.Legends.Add(legend20);
            this.chartTopProducts.Location = new System.Drawing.Point(3, 2);
            this.chartTopProducts.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chartTopProducts.Name = "chartTopProducts";
            series20.ChartArea = "ChartArea1";
            series20.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(121)))), ((int)(((byte)(107)))));
            series20.Legend = "Legend1";
            series20.Name = "Top";
            series20.XValueMember = "item_name";
            series20.YValueMembers = "total_value";
            this.chartTopProducts.Series.Add(series20);
            this.chartTopProducts.Size = new System.Drawing.Size(499, 190);
            this.chartTopProducts.TabIndex = 0;
            // 
            // Btn_ledger_report
            // 
            this.Btn_ledger_report.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_ledger_report.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Btn_ledger_report.Location = new System.Drawing.Point(749, 3);
            this.Btn_ledger_report.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Btn_ledger_report.Name = "Btn_ledger_report";
            this.Btn_ledger_report.Size = new System.Drawing.Size(136, 28);
            this.Btn_ledger_report.TabIndex = 11;
            this.Btn_ledger_report.Text = "&Ledger Report ";
            this.Btn_ledger_report.UseVisualStyleBackColor = true;
            this.Btn_ledger_report.Click += new System.EventHandler(this.Btn_ledger_report_Click);
            // 
            // frm_supplier_detail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 516);
            this.Controls.Add(this.tabProfile);
            this.Controls.Add(this.panelTop);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(807, 521);
            this.Name = "frm_supplier_detail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Supplier Profile";
            this.Load += new System.EventHandler(this.frm_supplier_detail_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.tabProfile.ResumeLayout(false);
            this.tabOverview.ResumeLayout(false);
            this.splitOverview.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitOverview)).EndInit();
            this.splitOverview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTopItemsSmall)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthlyPurchases)).EndInit();
            this.tableKpi.ResumeLayout(false);
            this.pnlKpi1.ResumeLayout(false);
            this.pnlKpi1.PerformLayout();
            this.pnlKpi2.ResumeLayout(false);
            this.pnlKpi2.PerformLayout();
            this.pnlKpi3.ResumeLayout(false);
            this.pnlKpi3.PerformLayout();
            this.pnlKpi4.ResumeLayout(false);
            this.pnlKpi4.PerformLayout();
            this.pnlOverviewHeader.ResumeLayout(false);
            this.pnlOverviewHeader.PerformLayout();
            this.tabLedger.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLedger)).EndInit();
            this.pnlLedgerFooter.ResumeLayout(false);
            this.pnlLedgerFooter.PerformLayout();
            this.pnlLedgerFilter.ResumeLayout(false);
            this.pnlLedgerFilter.PerformLayout();
            this.tabOutstanding.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridOutstanding)).EndInit();
            this.pnlOutstandingBottom.ResumeLayout(false);
            this.flowAging.ResumeLayout(false);
            this.pnlAgingCurrent.ResumeLayout(false);
            this.pnlAgingCurrent.PerformLayout();
            this.pnlAging1_30.ResumeLayout(false);
            this.pnlAging1_30.PerformLayout();
            this.pnlAging31_60.ResumeLayout(false);
            this.pnlAging31_60.PerformLayout();
            this.pnlAging61_90.ResumeLayout(false);
            this.pnlAging61_90.PerformLayout();
            this.pnlAging90Plus.ResumeLayout(false);
            this.pnlAging90Plus.PerformLayout();
            this.tabAnalytics.ResumeLayout(false);
            this.tableAnalytics.ResumeLayout(false);
            this.pnlPriceHistoryFilter.ResumeLayout(false);
            this.pnlPriceHistoryFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPriceHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthlyTrend24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartTopProducts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.TabControl tabProfile;
        private System.Windows.Forms.TabPage tabOverview;
        private System.Windows.Forms.TabPage tabLedger;
        private System.Windows.Forms.TabPage tabOutstanding;
        private System.Windows.Forms.TabPage tabAnalytics;
        private System.Windows.Forms.Panel pnlOverviewHeader;
        private System.Windows.Forms.Panel pnlAvatar;
        private System.Windows.Forms.Label lblHeaderName;
        private System.Windows.Forms.Label lblHeaderCode;
        private System.Windows.Forms.Label lblHeaderCategory;
        private System.Windows.Forms.Label lblHeaderPhone;
        private System.Windows.Forms.Label lblHeaderEmail;
        private System.Windows.Forms.Label lblHeaderCreditDays;
        private System.Windows.Forms.TableLayoutPanel tableKpi;
        private System.Windows.Forms.Panel pnlKpi1;
        private System.Windows.Forms.Label lblLifetimePurchasesValue;
        private System.Windows.Forms.Label lblLifetimePurchasesTitle;
        private System.Windows.Forms.Panel pnlKpi2;
        private System.Windows.Forms.Label lblTotalPaidValue;
        private System.Windows.Forms.Label lblTotalPaidTitle;
        private System.Windows.Forms.Panel pnlKpi3;
        private System.Windows.Forms.Label lblCurrentPayableValue;
        private System.Windows.Forms.Label lblCurrentPayableTitle;
        private System.Windows.Forms.Panel pnlKpi4;
        private System.Windows.Forms.Label lblAvailableCreditValue;
        private System.Windows.Forms.Label lblAvailableCreditTitle;
        private System.Windows.Forms.SplitContainer splitOverview;
        private System.Windows.Forms.Label lblTopItemsSmallTitle;
        private System.Windows.Forms.DataGridView gridTopItemsSmall;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTopItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTopItemQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTopItemValue;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMonthlyPurchases;
        private System.Windows.Forms.Panel pnlLedgerFilter;
        private System.Windows.Forms.DateTimePicker dtLedgerTo;
        private System.Windows.Forms.Label lblLedgerTo;
        private System.Windows.Forms.DateTimePicker dtLedgerFrom;
        private System.Windows.Forms.Label lblLedgerFrom;
        private System.Windows.Forms.Button btnLoadLedger;
        private System.Windows.Forms.DataGridView gridLedger;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerRef;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerBalance;
        private System.Windows.Forms.Panel pnlLedgerFooter;
        private System.Windows.Forms.Label lblLedgerTotals;
        private System.Windows.Forms.Button btnPrintStatement;
        private System.Windows.Forms.DataGridView gridOutstanding;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutBillNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutBillDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutDueDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutDays;
        private System.Windows.Forms.Panel pnlOutstandingBottom;
        private System.Windows.Forms.Button btnMakePayment;
        private System.Windows.Forms.FlowLayoutPanel flowAging;
        private System.Windows.Forms.Panel pnlAgingCurrent;
        private System.Windows.Forms.Label lblAgeCurrentValue;
        private System.Windows.Forms.Label lblAgeCurrentTitle;
        private System.Windows.Forms.Panel pnlAging1_30;
        private System.Windows.Forms.Label lblAge1_30Value;
        private System.Windows.Forms.Label lblAge1_30Title;
        private System.Windows.Forms.Panel pnlAging31_60;
        private System.Windows.Forms.Label lblAge31_60Value;
        private System.Windows.Forms.Label lblAge31_60Title;
        private System.Windows.Forms.Panel pnlAging61_90;
        private System.Windows.Forms.Label lblAge61_90Value;
        private System.Windows.Forms.Label lblAge61_90Title;
        private System.Windows.Forms.Panel pnlAging90Plus;
        private System.Windows.Forms.Label lblAge90PlusValue;
        private System.Windows.Forms.Label lblAge90PlusTitle;
        private System.Windows.Forms.TableLayoutPanel tableAnalytics;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTopProducts;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMonthlyTrend24;
        private System.Windows.Forms.Panel pnlPriceHistoryFilter;
        private System.Windows.Forms.Label lblPriceProduct;
        private System.Windows.Forms.ComboBox cmbPriceProduct;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPriceHistory;
        private System.Windows.Forms.Button Btn_ledger_report;
    }
}

