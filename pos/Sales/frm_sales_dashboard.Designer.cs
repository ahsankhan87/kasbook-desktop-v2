namespace pos.Sales
{
    partial class frm_sales_dashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_sales_dashboard));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.splitBottom = new System.Windows.Forms.SplitContainer();
            this.panelTopCustomers = new System.Windows.Forms.Panel();
            this.gridTopCustomers = new System.Windows.Forms.DataGridView();
            this.colCustomerRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerShare = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerOutstanding = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTopCustomers = new System.Windows.Forms.Label();
            this.panelRecentInvoices = new System.Windows.Forms.Panel();
            this.gridRecentInvoices = new System.Windows.Forms.DataGridView();
            this.colInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRecentInvoices = new System.Windows.Forms.Label();
            this.splitCharts = new System.Windows.Forms.SplitContainer();
            this.panelMonthlyChart = new System.Windows.Forms.Panel();
            this.chartMonthlySales = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblMonthlyChart = new System.Windows.Forms.Label();
            this.panelCategoryChart = new System.Windows.Forms.Panel();
            this.chartCategory = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblCategoryChart = new System.Windows.Forms.Label();
            this.panelRoot = new System.Windows.Forms.Panel();
            this.panelPeriod = new System.Windows.Forms.Panel();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.flowPeriod = new System.Windows.Forms.FlowLayoutPanel();
            this.rbToday = new System.Windows.Forms.RadioButton();
            this.rbWeek = new System.Windows.Forms.RadioButton();
            this.rbMonth = new System.Windows.Forms.RadioButton();
            this.rbQuarter = new System.Windows.Forms.RadioButton();
            this.rbYear = new System.Windows.Forms.RadioButton();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            this.lblPeriodTitle = new System.Windows.Forms.Label();
            this.panelKpis = new System.Windows.Forms.TableLayoutPanel();
            this.cardTotalSales = new System.Windows.Forms.Panel();
            this.lblTotalSalesTrend = new System.Windows.Forms.Label();
            this.lblTotalSalesValue = new System.Windows.Forms.Label();
            this.lblTotalSalesTitle = new System.Windows.Forms.Label();
            this.cardInvoices = new System.Windows.Forms.Panel();
            this.lblInvoicesValue = new System.Windows.Forms.Label();
            this.lblInvoicesTitle = new System.Windows.Forms.Label();
            this.cardPaid = new System.Windows.Forms.Panel();
            this.lblPaidValue = new System.Windows.Forms.Label();
            this.lblPaidTitle = new System.Windows.Forms.Label();
            this.cardOutstanding = new System.Windows.Forms.Panel();
            this.lblOutstandingSub = new System.Windows.Forms.Label();
            this.lblOutstandingValue = new System.Windows.Forms.Label();
            this.lblOutstandingTitle = new System.Windows.Forms.Label();
            this.cardAvgInvoice = new System.Windows.Forms.Panel();
            this.lblAvgInvoiceValue = new System.Windows.Forms.Label();
            this.lblAvgInvoiceTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tslLastRefresh = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslRefresh = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).BeginInit();
            this.splitBottom.Panel1.SuspendLayout();
            this.splitBottom.Panel2.SuspendLayout();
            this.splitBottom.SuspendLayout();
            this.panelTopCustomers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTopCustomers)).BeginInit();
            this.panelRecentInvoices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRecentInvoices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitCharts)).BeginInit();
            this.splitCharts.Panel1.SuspendLayout();
            this.splitCharts.Panel2.SuspendLayout();
            this.splitCharts.SuspendLayout();
            this.panelMonthlyChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthlySales)).BeginInit();
            this.panelCategoryChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartCategory)).BeginInit();
            this.panelRoot.SuspendLayout();
            this.panelPeriod.SuspendLayout();
            this.flowPeriod.SuspendLayout();
            this.panelKpis.SuspendLayout();
            this.cardTotalSales.SuspendLayout();
            this.cardInvoices.SuspendLayout();
            this.cardPaid.SuspendLayout();
            this.cardOutstanding.SuspendLayout();
            this.cardAvgInvoice.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitBottom
            // 
            resources.ApplyResources(this.splitBottom, "splitBottom");
            this.splitBottom.Name = "splitBottom";
            // 
            // splitBottom.Panel1
            // 
            resources.ApplyResources(this.splitBottom.Panel1, "splitBottom.Panel1");
            this.splitBottom.Panel1.Controls.Add(this.panelTopCustomers);
            // 
            // splitBottom.Panel2
            // 
            resources.ApplyResources(this.splitBottom.Panel2, "splitBottom.Panel2");
            this.splitBottom.Panel2.Controls.Add(this.panelRecentInvoices);
            // 
            // panelTopCustomers
            // 
            resources.ApplyResources(this.panelTopCustomers, "panelTopCustomers");
            this.panelTopCustomers.BackColor = System.Drawing.Color.White;
            this.panelTopCustomers.Controls.Add(this.gridTopCustomers);
            this.panelTopCustomers.Controls.Add(this.lblTopCustomers);
            this.panelTopCustomers.Name = "panelTopCustomers";
            // 
            // gridTopCustomers
            // 
            resources.ApplyResources(this.gridTopCustomers, "gridTopCustomers");
            this.gridTopCustomers.AllowUserToAddRows = false;
            this.gridTopCustomers.AllowUserToDeleteRows = false;
            this.gridTopCustomers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridTopCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTopCustomers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCustomerRank,
            this.colCustomerName,
            this.colCustomerTotal,
            this.colCustomerShare,
            this.colCustomerOutstanding});
            this.gridTopCustomers.Name = "gridTopCustomers";
            this.gridTopCustomers.ReadOnly = true;
            // 
            // colCustomerRank
            // 
            this.colCustomerRank.DataPropertyName = "rank_no";
            this.colCustomerRank.FillWeight = 45F;
            resources.ApplyResources(this.colCustomerRank, "colCustomerRank");
            this.colCustomerRank.Name = "colCustomerRank";
            this.colCustomerRank.ReadOnly = true;
            // 
            // colCustomerName
            // 
            this.colCustomerName.DataPropertyName = "customer_name";
            this.colCustomerName.FillWeight = 165F;
            resources.ApplyResources(this.colCustomerName, "colCustomerName");
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.ReadOnly = true;
            // 
            // colCustomerTotal
            // 
            this.colCustomerTotal.DataPropertyName = "total_sales";
            this.colCustomerTotal.FillWeight = 95F;
            resources.ApplyResources(this.colCustomerTotal, "colCustomerTotal");
            this.colCustomerTotal.Name = "colCustomerTotal";
            this.colCustomerTotal.ReadOnly = true;
            // 
            // colCustomerShare
            // 
            this.colCustomerShare.DataPropertyName = "share_percent";
            this.colCustomerShare.FillWeight = 75F;
            resources.ApplyResources(this.colCustomerShare, "colCustomerShare");
            this.colCustomerShare.Name = "colCustomerShare";
            this.colCustomerShare.ReadOnly = true;
            // 
            // colCustomerOutstanding
            // 
            this.colCustomerOutstanding.DataPropertyName = "outstanding";
            this.colCustomerOutstanding.FillWeight = 95F;
            resources.ApplyResources(this.colCustomerOutstanding, "colCustomerOutstanding");
            this.colCustomerOutstanding.Name = "colCustomerOutstanding";
            this.colCustomerOutstanding.ReadOnly = true;
            // 
            // lblTopCustomers
            // 
            resources.ApplyResources(this.lblTopCustomers, "lblTopCustomers");
            this.lblTopCustomers.Name = "lblTopCustomers";
            // 
            // panelRecentInvoices
            // 
            resources.ApplyResources(this.panelRecentInvoices, "panelRecentInvoices");
            this.panelRecentInvoices.BackColor = System.Drawing.Color.White;
            this.panelRecentInvoices.Controls.Add(this.gridRecentInvoices);
            this.panelRecentInvoices.Controls.Add(this.lblRecentInvoices);
            this.panelRecentInvoices.Name = "panelRecentInvoices";
            // 
            // gridRecentInvoices
            // 
            resources.ApplyResources(this.gridRecentInvoices, "gridRecentInvoices");
            this.gridRecentInvoices.AllowUserToAddRows = false;
            this.gridRecentInvoices.AllowUserToDeleteRows = false;
            this.gridRecentInvoices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridRecentInvoices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRecentInvoices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInvoiceNo,
            this.colInvoiceCustomer,
            this.colInvoiceAmount,
            this.colInvoiceStatus});
            this.gridRecentInvoices.Name = "gridRecentInvoices";
            this.gridRecentInvoices.ReadOnly = true;
            // 
            // colInvoiceNo
            // 
            this.colInvoiceNo.DataPropertyName = "invoice_no";
            this.colInvoiceNo.FillWeight = 95F;
            resources.ApplyResources(this.colInvoiceNo, "colInvoiceNo");
            this.colInvoiceNo.Name = "colInvoiceNo";
            this.colInvoiceNo.ReadOnly = true;
            // 
            // colInvoiceCustomer
            // 
            this.colInvoiceCustomer.DataPropertyName = "customer_name";
            this.colInvoiceCustomer.FillWeight = 155F;
            resources.ApplyResources(this.colInvoiceCustomer, "colInvoiceCustomer");
            this.colInvoiceCustomer.Name = "colInvoiceCustomer";
            this.colInvoiceCustomer.ReadOnly = true;
            // 
            // colInvoiceAmount
            // 
            this.colInvoiceAmount.DataPropertyName = "amount";
            this.colInvoiceAmount.FillWeight = 90F;
            resources.ApplyResources(this.colInvoiceAmount, "colInvoiceAmount");
            this.colInvoiceAmount.Name = "colInvoiceAmount";
            this.colInvoiceAmount.ReadOnly = true;
            // 
            // colInvoiceStatus
            // 
            this.colInvoiceStatus.DataPropertyName = "status";
            this.colInvoiceStatus.FillWeight = 70F;
            resources.ApplyResources(this.colInvoiceStatus, "colInvoiceStatus");
            this.colInvoiceStatus.Name = "colInvoiceStatus";
            this.colInvoiceStatus.ReadOnly = true;
            // 
            // lblRecentInvoices
            // 
            resources.ApplyResources(this.lblRecentInvoices, "lblRecentInvoices");
            this.lblRecentInvoices.Name = "lblRecentInvoices";
            // 
            // splitCharts
            // 
            resources.ApplyResources(this.splitCharts, "splitCharts");
            this.splitCharts.Name = "splitCharts";
            // 
            // splitCharts.Panel1
            // 
            resources.ApplyResources(this.splitCharts.Panel1, "splitCharts.Panel1");
            this.splitCharts.Panel1.Controls.Add(this.panelMonthlyChart);
            // 
            // splitCharts.Panel2
            // 
            resources.ApplyResources(this.splitCharts.Panel2, "splitCharts.Panel2");
            this.splitCharts.Panel2.Controls.Add(this.panelCategoryChart);
            // 
            // panelMonthlyChart
            // 
            resources.ApplyResources(this.panelMonthlyChart, "panelMonthlyChart");
            this.panelMonthlyChart.BackColor = System.Drawing.Color.White;
            this.panelMonthlyChart.Controls.Add(this.chartMonthlySales);
            this.panelMonthlyChart.Controls.Add(this.lblMonthlyChart);
            this.panelMonthlyChart.Name = "panelMonthlyChart";
            // 
            // chartMonthlySales
            // 
            resources.ApplyResources(this.chartMonthlySales, "chartMonthlySales");
            chartArea5.Name = "ChartArea1";
            this.chartMonthlySales.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chartMonthlySales.Legends.Add(legend5);
            this.chartMonthlySales.Name = "chartMonthlySales";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Monthly";
            this.chartMonthlySales.Series.Add(series5);
            // 
            // lblMonthlyChart
            // 
            resources.ApplyResources(this.lblMonthlyChart, "lblMonthlyChart");
            this.lblMonthlyChart.Name = "lblMonthlyChart";
            // 
            // panelCategoryChart
            // 
            resources.ApplyResources(this.panelCategoryChart, "panelCategoryChart");
            this.panelCategoryChart.BackColor = System.Drawing.Color.White;
            this.panelCategoryChart.Controls.Add(this.chartCategory);
            this.panelCategoryChart.Controls.Add(this.lblCategoryChart);
            this.panelCategoryChart.Name = "panelCategoryChart";
            // 
            // chartCategory
            // 
            resources.ApplyResources(this.chartCategory, "chartCategory");
            chartArea6.Name = "ChartArea1";
            this.chartCategory.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.chartCategory.Legends.Add(legend6);
            this.chartCategory.Name = "chartCategory";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Category";
            this.chartCategory.Series.Add(series6);
            // 
            // lblCategoryChart
            // 
            resources.ApplyResources(this.lblCategoryChart, "lblCategoryChart");
            this.lblCategoryChart.Name = "lblCategoryChart";
            // 
            // panelRoot
            // 
            resources.ApplyResources(this.panelRoot, "panelRoot");
            this.panelRoot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelRoot.Controls.Add(this.splitBottom);
            this.panelRoot.Controls.Add(this.splitCharts);
            this.panelRoot.Controls.Add(this.panelPeriod);
            this.panelRoot.Controls.Add(this.panelKpis);
            this.panelRoot.Controls.Add(this.lblTitle);
            this.panelRoot.Controls.Add(this.statusStrip);
            this.panelRoot.Name = "panelRoot";
            // 
            // panelPeriod
            // 
            resources.ApplyResources(this.panelPeriod, "panelPeriod");
            this.panelPeriod.BackColor = System.Drawing.Color.White;
            this.panelPeriod.Controls.Add(this.dtpTo);
            this.panelPeriod.Controls.Add(this.lblTo);
            this.panelPeriod.Controls.Add(this.dtpFrom);
            this.panelPeriod.Controls.Add(this.lblFrom);
            this.panelPeriod.Controls.Add(this.flowPeriod);
            this.panelPeriod.Controls.Add(this.lblPeriodTitle);
            this.panelPeriod.Name = "panelPeriod";
            // 
            // dtpTo
            // 
            resources.ApplyResources(this.dtpTo, "dtpTo");
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ValueChanged += new System.EventHandler(this.CustomDateChanged);
            // 
            // lblTo
            // 
            resources.ApplyResources(this.lblTo, "lblTo");
            this.lblTo.Name = "lblTo";
            // 
            // dtpFrom
            // 
            resources.ApplyResources(this.dtpFrom, "dtpFrom");
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ValueChanged += new System.EventHandler(this.CustomDateChanged);
            // 
            // lblFrom
            // 
            resources.ApplyResources(this.lblFrom, "lblFrom");
            this.lblFrom.Name = "lblFrom";
            // 
            // flowPeriod
            // 
            resources.ApplyResources(this.flowPeriod, "flowPeriod");
            this.flowPeriod.Controls.Add(this.rbToday);
            this.flowPeriod.Controls.Add(this.rbWeek);
            this.flowPeriod.Controls.Add(this.rbMonth);
            this.flowPeriod.Controls.Add(this.rbQuarter);
            this.flowPeriod.Controls.Add(this.rbYear);
            this.flowPeriod.Controls.Add(this.rbCustom);
            this.flowPeriod.Name = "flowPeriod";
            // 
            // rbToday
            // 
            resources.ApplyResources(this.rbToday, "rbToday");
            this.rbToday.Name = "rbToday";
            this.rbToday.TabStop = true;
            this.rbToday.UseVisualStyleBackColor = true;
            this.rbToday.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // rbWeek
            // 
            resources.ApplyResources(this.rbWeek, "rbWeek");
            this.rbWeek.Name = "rbWeek";
            this.rbWeek.TabStop = true;
            this.rbWeek.UseVisualStyleBackColor = true;
            this.rbWeek.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // rbMonth
            // 
            resources.ApplyResources(this.rbMonth, "rbMonth");
            this.rbMonth.Name = "rbMonth";
            this.rbMonth.TabStop = true;
            this.rbMonth.UseVisualStyleBackColor = true;
            this.rbMonth.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // rbQuarter
            // 
            resources.ApplyResources(this.rbQuarter, "rbQuarter");
            this.rbQuarter.Name = "rbQuarter";
            this.rbQuarter.TabStop = true;
            this.rbQuarter.UseVisualStyleBackColor = true;
            this.rbQuarter.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // rbYear
            // 
            resources.ApplyResources(this.rbYear, "rbYear");
            this.rbYear.Name = "rbYear";
            this.rbYear.TabStop = true;
            this.rbYear.UseVisualStyleBackColor = true;
            this.rbYear.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // rbCustom
            // 
            resources.ApplyResources(this.rbCustom, "rbCustom");
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.TabStop = true;
            this.rbCustom.UseVisualStyleBackColor = true;
            this.rbCustom.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // lblPeriodTitle
            // 
            resources.ApplyResources(this.lblPeriodTitle, "lblPeriodTitle");
            this.lblPeriodTitle.Name = "lblPeriodTitle";
            // 
            // panelKpis
            // 
            resources.ApplyResources(this.panelKpis, "panelKpis");
            this.panelKpis.Controls.Add(this.cardTotalSales, 0, 0);
            this.panelKpis.Controls.Add(this.cardInvoices, 1, 0);
            this.panelKpis.Controls.Add(this.cardPaid, 2, 0);
            this.panelKpis.Controls.Add(this.cardOutstanding, 3, 0);
            this.panelKpis.Controls.Add(this.cardAvgInvoice, 4, 0);
            this.panelKpis.Name = "panelKpis";
            // 
            // cardTotalSales
            // 
            resources.ApplyResources(this.cardTotalSales, "cardTotalSales");
            this.cardTotalSales.BackColor = System.Drawing.Color.White;
            this.cardTotalSales.Controls.Add(this.lblTotalSalesTrend);
            this.cardTotalSales.Controls.Add(this.lblTotalSalesValue);
            this.cardTotalSales.Controls.Add(this.lblTotalSalesTitle);
            this.cardTotalSales.Name = "cardTotalSales";
            // 
            // lblTotalSalesTrend
            // 
            resources.ApplyResources(this.lblTotalSalesTrend, "lblTotalSalesTrend");
            this.lblTotalSalesTrend.Name = "lblTotalSalesTrend";
            // 
            // lblTotalSalesValue
            // 
            resources.ApplyResources(this.lblTotalSalesValue, "lblTotalSalesValue");
            this.lblTotalSalesValue.Name = "lblTotalSalesValue";
            // 
            // lblTotalSalesTitle
            // 
            resources.ApplyResources(this.lblTotalSalesTitle, "lblTotalSalesTitle");
            this.lblTotalSalesTitle.Name = "lblTotalSalesTitle";
            // 
            // cardInvoices
            // 
            resources.ApplyResources(this.cardInvoices, "cardInvoices");
            this.cardInvoices.BackColor = System.Drawing.Color.White;
            this.cardInvoices.Controls.Add(this.lblInvoicesValue);
            this.cardInvoices.Controls.Add(this.lblInvoicesTitle);
            this.cardInvoices.Name = "cardInvoices";
            // 
            // lblInvoicesValue
            // 
            resources.ApplyResources(this.lblInvoicesValue, "lblInvoicesValue");
            this.lblInvoicesValue.Name = "lblInvoicesValue";
            // 
            // lblInvoicesTitle
            // 
            resources.ApplyResources(this.lblInvoicesTitle, "lblInvoicesTitle");
            this.lblInvoicesTitle.Name = "lblInvoicesTitle";
            // 
            // cardPaid
            // 
            resources.ApplyResources(this.cardPaid, "cardPaid");
            this.cardPaid.BackColor = System.Drawing.Color.White;
            this.cardPaid.Controls.Add(this.lblPaidValue);
            this.cardPaid.Controls.Add(this.lblPaidTitle);
            this.cardPaid.Name = "cardPaid";
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
            // cardOutstanding
            // 
            resources.ApplyResources(this.cardOutstanding, "cardOutstanding");
            this.cardOutstanding.BackColor = System.Drawing.Color.White;
            this.cardOutstanding.Controls.Add(this.lblOutstandingSub);
            this.cardOutstanding.Controls.Add(this.lblOutstandingValue);
            this.cardOutstanding.Controls.Add(this.lblOutstandingTitle);
            this.cardOutstanding.Name = "cardOutstanding";
            // 
            // lblOutstandingSub
            // 
            resources.ApplyResources(this.lblOutstandingSub, "lblOutstandingSub");
            this.lblOutstandingSub.Name = "lblOutstandingSub";
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
            // cardAvgInvoice
            // 
            resources.ApplyResources(this.cardAvgInvoice, "cardAvgInvoice");
            this.cardAvgInvoice.BackColor = System.Drawing.Color.White;
            this.cardAvgInvoice.Controls.Add(this.lblAvgInvoiceValue);
            this.cardAvgInvoice.Controls.Add(this.lblAvgInvoiceTitle);
            this.cardAvgInvoice.Name = "cardAvgInvoice";
            // 
            // lblAvgInvoiceValue
            // 
            resources.ApplyResources(this.lblAvgInvoiceValue, "lblAvgInvoiceValue");
            this.lblAvgInvoiceValue.Name = "lblAvgInvoiceValue";
            // 
            // lblAvgInvoiceTitle
            // 
            resources.ApplyResources(this.lblAvgInvoiceTitle, "lblAvgInvoiceTitle");
            this.lblAvgInvoiceTitle.Name = "lblAvgInvoiceTitle";
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // statusStrip
            // 
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslLastRefresh,
            this.tslRefresh});
            this.statusStrip.Name = "statusStrip";
            // 
            // tslLastRefresh
            // 
            resources.ApplyResources(this.tslLastRefresh, "tslLastRefresh");
            this.tslLastRefresh.Name = "tslLastRefresh";
            // 
            // tslRefresh
            // 
            resources.ApplyResources(this.tslRefresh, "tslRefresh");
            this.tslRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tslRefresh.IsLink = true;
            this.tslRefresh.Name = "tslRefresh";
            this.tslRefresh.Click += new System.EventHandler(this.tslRefresh_Click);
            // 
            // frm_sales_dashboard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelRoot);
            this.Name = "frm_sales_dashboard";
            this.Load += new System.EventHandler(this.frm_sales_dashboard_Load);
            this.splitBottom.Panel1.ResumeLayout(false);
            this.splitBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).EndInit();
            this.splitBottom.ResumeLayout(false);
            this.panelTopCustomers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTopCustomers)).EndInit();
            this.panelRecentInvoices.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRecentInvoices)).EndInit();
            this.splitCharts.Panel1.ResumeLayout(false);
            this.splitCharts.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCharts)).EndInit();
            this.splitCharts.ResumeLayout(false);
            this.panelMonthlyChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthlySales)).EndInit();
            this.panelCategoryChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartCategory)).EndInit();
            this.panelRoot.ResumeLayout(false);
            this.panelRoot.PerformLayout();
            this.panelPeriod.ResumeLayout(false);
            this.panelPeriod.PerformLayout();
            this.flowPeriod.ResumeLayout(false);
            this.flowPeriod.PerformLayout();
            this.panelKpis.ResumeLayout(false);
            this.cardTotalSales.ResumeLayout(false);
            this.cardTotalSales.PerformLayout();
            this.cardInvoices.ResumeLayout(false);
            this.cardInvoices.PerformLayout();
            this.cardPaid.ResumeLayout(false);
            this.cardPaid.PerformLayout();
            this.cardOutstanding.ResumeLayout(false);
            this.cardOutstanding.PerformLayout();
            this.cardAvgInvoice.ResumeLayout(false);
            this.cardAvgInvoice.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelRoot;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel panelKpis;
        private System.Windows.Forms.Panel cardTotalSales;
        private System.Windows.Forms.Label lblTotalSalesTrend;
        private System.Windows.Forms.Label lblTotalSalesValue;
        private System.Windows.Forms.Label lblTotalSalesTitle;
        private System.Windows.Forms.Panel cardInvoices;
        private System.Windows.Forms.Label lblInvoicesValue;
        private System.Windows.Forms.Label lblInvoicesTitle;
        private System.Windows.Forms.Panel cardPaid;
        private System.Windows.Forms.Label lblPaidValue;
        private System.Windows.Forms.Label lblPaidTitle;
        private System.Windows.Forms.Panel cardOutstanding;
        private System.Windows.Forms.Label lblOutstandingSub;
        private System.Windows.Forms.Label lblOutstandingValue;
        private System.Windows.Forms.Label lblOutstandingTitle;
        private System.Windows.Forms.Panel cardAvgInvoice;
        private System.Windows.Forms.Label lblAvgInvoiceValue;
        private System.Windows.Forms.Label lblAvgInvoiceTitle;
        private System.Windows.Forms.Panel panelPeriod;
        private System.Windows.Forms.Label lblPeriodTitle;
        private System.Windows.Forms.FlowLayoutPanel flowPeriod;
        private System.Windows.Forms.RadioButton rbToday;
        private System.Windows.Forms.RadioButton rbWeek;
        private System.Windows.Forms.RadioButton rbMonth;
        private System.Windows.Forms.RadioButton rbQuarter;
        private System.Windows.Forms.RadioButton rbYear;
        private System.Windows.Forms.RadioButton rbCustom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.SplitContainer splitCharts;
        private System.Windows.Forms.Panel panelMonthlyChart;
        private System.Windows.Forms.Label lblMonthlyChart;
        private System.Windows.Forms.Panel panelCategoryChart;
        private System.Windows.Forms.Label lblCategoryChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMonthlySales;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCategory;
        private System.Windows.Forms.SplitContainer splitBottom;
        private System.Windows.Forms.Panel panelTopCustomers;
        private System.Windows.Forms.Label lblTopCustomers;
        private System.Windows.Forms.Panel panelRecentInvoices;
        private System.Windows.Forms.Label lblRecentInvoices;
        private System.Windows.Forms.DataGridView gridTopCustomers;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerShare;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerOutstanding;
        private System.Windows.Forms.DataGridView gridRecentInvoices;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceStatus;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel tslLastRefresh;
        private System.Windows.Forms.ToolStripStatusLabel tslRefresh;
    }
}
