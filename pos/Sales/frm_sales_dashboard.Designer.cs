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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panelRoot = new System.Windows.Forms.Panel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tslLastRefresh = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslRefresh = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.panelRoot.SuspendLayout();
            this.statusStrip.SuspendLayout();
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
            this.panelPeriod.SuspendLayout();
            this.flowPeriod.SuspendLayout();
            this.panelKpis.SuspendLayout();
            this.cardTotalSales.SuspendLayout();
            this.cardInvoices.SuspendLayout();
            this.cardPaid.SuspendLayout();
            this.cardOutstanding.SuspendLayout();
            this.cardAvgInvoice.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelRoot.Controls.Add(this.splitBottom);
            this.panelRoot.Controls.Add(this.splitCharts);
            this.panelRoot.Controls.Add(this.panelPeriod);
            this.panelRoot.Controls.Add(this.panelKpis);
            this.panelRoot.Controls.Add(this.lblTitle);
            this.panelRoot.Controls.Add(this.statusStrip);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(12);
            this.panelRoot.Size = new System.Drawing.Size(1250, 790);
            this.panelRoot.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslLastRefresh,
            this.tslRefresh});
            this.statusStrip.Location = new System.Drawing.Point(12, 756);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1226, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // tslLastRefresh
            // 
            this.tslLastRefresh.Name = "tslLastRefresh";
            this.tslLastRefresh.Size = new System.Drawing.Size(91, 17);
            this.tslLastRefresh.Text = "Last refresh: --";
            // 
            // tslRefresh
            // 
            this.tslRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tslRefresh.IsLink = true;
            this.tslRefresh.Name = "tslRefresh";
            this.tslRefresh.Size = new System.Drawing.Size(47, 17);
            this.tslRefresh.Text = "Refresh";
            this.tslRefresh.Click += new System.EventHandler(this.tslRefresh_Click);
            // 
            // splitBottom
            // 
            this.splitBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBottom.Location = new System.Drawing.Point(12, 542);
            this.splitBottom.Name = "splitBottom";
            // 
            // splitBottom.Panel1
            // 
            this.splitBottom.Panel1.Controls.Add(this.panelTopCustomers);
            // 
            // splitBottom.Panel2
            // 
            this.splitBottom.Panel2.Controls.Add(this.panelRecentInvoices);
            this.splitBottom.Size = new System.Drawing.Size(1226, 214);
            this.splitBottom.SplitterDistance = 720;
            this.splitBottom.TabIndex = 4;
            // 
            // panelTopCustomers
            // 
            this.panelTopCustomers.BackColor = System.Drawing.Color.White;
            this.panelTopCustomers.Controls.Add(this.gridTopCustomers);
            this.panelTopCustomers.Controls.Add(this.lblTopCustomers);
            this.panelTopCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTopCustomers.Location = new System.Drawing.Point(0, 0);
            this.panelTopCustomers.Name = "panelTopCustomers";
            this.panelTopCustomers.Padding = new System.Windows.Forms.Padding(10);
            this.panelTopCustomers.Size = new System.Drawing.Size(720, 214);
            this.panelTopCustomers.TabIndex = 0;
            // 
            // gridTopCustomers
            // 
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
            this.gridTopCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTopCustomers.Location = new System.Drawing.Point(10, 36);
            this.gridTopCustomers.Name = "gridTopCustomers";
            this.gridTopCustomers.ReadOnly = true;
            this.gridTopCustomers.Size = new System.Drawing.Size(700, 168);
            this.gridTopCustomers.TabIndex = 1;
            // 
            // colCustomerRank
            // 
            this.colCustomerRank.DataPropertyName = "rank_no";
            this.colCustomerRank.FillWeight = 45F;
            this.colCustomerRank.HeaderText = "Rank";
            this.colCustomerRank.Name = "colCustomerRank";
            this.colCustomerRank.ReadOnly = true;
            // 
            // colCustomerName
            // 
            this.colCustomerName.DataPropertyName = "customer_name";
            this.colCustomerName.FillWeight = 165F;
            this.colCustomerName.HeaderText = "Customer Name";
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.ReadOnly = true;
            // 
            // colCustomerTotal
            // 
            this.colCustomerTotal.DataPropertyName = "total_sales";
            this.colCustomerTotal.FillWeight = 95F;
            this.colCustomerTotal.HeaderText = "Total Sales";
            this.colCustomerTotal.Name = "colCustomerTotal";
            this.colCustomerTotal.ReadOnly = true;
            // 
            // colCustomerShare
            // 
            this.colCustomerShare.DataPropertyName = "share_percent";
            this.colCustomerShare.FillWeight = 75F;
            this.colCustomerShare.HeaderText = "% of Total";
            this.colCustomerShare.Name = "colCustomerShare";
            this.colCustomerShare.ReadOnly = true;
            // 
            // colCustomerOutstanding
            // 
            this.colCustomerOutstanding.DataPropertyName = "outstanding";
            this.colCustomerOutstanding.FillWeight = 95F;
            this.colCustomerOutstanding.HeaderText = "Outstanding";
            this.colCustomerOutstanding.Name = "colCustomerOutstanding";
            this.colCustomerOutstanding.ReadOnly = true;
            // 
            // lblTopCustomers
            // 
            this.lblTopCustomers.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTopCustomers.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblTopCustomers.Location = new System.Drawing.Point(10, 10);
            this.lblTopCustomers.Name = "lblTopCustomers";
            this.lblTopCustomers.Size = new System.Drawing.Size(700, 26);
            this.lblTopCustomers.TabIndex = 0;
            this.lblTopCustomers.Text = "Top 10 Customers";
            // 
            // panelRecentInvoices
            // 
            this.panelRecentInvoices.BackColor = System.Drawing.Color.White;
            this.panelRecentInvoices.Controls.Add(this.gridRecentInvoices);
            this.panelRecentInvoices.Controls.Add(this.lblRecentInvoices);
            this.panelRecentInvoices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRecentInvoices.Location = new System.Drawing.Point(0, 0);
            this.panelRecentInvoices.Name = "panelRecentInvoices";
            this.panelRecentInvoices.Padding = new System.Windows.Forms.Padding(10);
            this.panelRecentInvoices.Size = new System.Drawing.Size(502, 214);
            this.panelRecentInvoices.TabIndex = 0;
            // 
            // gridRecentInvoices
            // 
            this.gridRecentInvoices.AllowUserToAddRows = false;
            this.gridRecentInvoices.AllowUserToDeleteRows = false;
            this.gridRecentInvoices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridRecentInvoices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRecentInvoices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInvoiceNo,
            this.colInvoiceCustomer,
            this.colInvoiceAmount,
            this.colInvoiceStatus});
            this.gridRecentInvoices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRecentInvoices.Location = new System.Drawing.Point(10, 36);
            this.gridRecentInvoices.Name = "gridRecentInvoices";
            this.gridRecentInvoices.ReadOnly = true;
            this.gridRecentInvoices.Size = new System.Drawing.Size(482, 168);
            this.gridRecentInvoices.TabIndex = 1;
            // 
            // colInvoiceNo
            // 
            this.colInvoiceNo.DataPropertyName = "invoice_no";
            this.colInvoiceNo.FillWeight = 95F;
            this.colInvoiceNo.HeaderText = "Invoice No";
            this.colInvoiceNo.Name = "colInvoiceNo";
            this.colInvoiceNo.ReadOnly = true;
            // 
            // colInvoiceCustomer
            // 
            this.colInvoiceCustomer.DataPropertyName = "customer_name";
            this.colInvoiceCustomer.FillWeight = 155F;
            this.colInvoiceCustomer.HeaderText = "Customer";
            this.colInvoiceCustomer.Name = "colInvoiceCustomer";
            this.colInvoiceCustomer.ReadOnly = true;
            // 
            // colInvoiceAmount
            // 
            this.colInvoiceAmount.DataPropertyName = "amount";
            this.colInvoiceAmount.FillWeight = 90F;
            this.colInvoiceAmount.HeaderText = "Amount";
            this.colInvoiceAmount.Name = "colInvoiceAmount";
            this.colInvoiceAmount.ReadOnly = true;
            // 
            // colInvoiceStatus
            // 
            this.colInvoiceStatus.DataPropertyName = "status";
            this.colInvoiceStatus.FillWeight = 70F;
            this.colInvoiceStatus.HeaderText = "Status";
            this.colInvoiceStatus.Name = "colInvoiceStatus";
            this.colInvoiceStatus.ReadOnly = true;
            // 
            // lblRecentInvoices
            // 
            this.lblRecentInvoices.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRecentInvoices.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblRecentInvoices.Location = new System.Drawing.Point(10, 10);
            this.lblRecentInvoices.Name = "lblRecentInvoices";
            this.lblRecentInvoices.Size = new System.Drawing.Size(482, 26);
            this.lblRecentInvoices.TabIndex = 0;
            this.lblRecentInvoices.Text = "Recent Invoices";
            // 
            // splitCharts
            // 
            this.splitCharts.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitCharts.Location = new System.Drawing.Point(12, 334);
            this.splitCharts.Name = "splitCharts";
            // 
            // splitCharts.Panel1
            // 
            this.splitCharts.Panel1.Controls.Add(this.panelMonthlyChart);
            // 
            // splitCharts.Panel2
            // 
            this.splitCharts.Panel2.Controls.Add(this.panelCategoryChart);
            this.splitCharts.Size = new System.Drawing.Size(1226, 208);
            this.splitCharts.SplitterDistance = 740;
            this.splitCharts.TabIndex = 3;
            // 
            // panelMonthlyChart
            // 
            this.panelMonthlyChart.BackColor = System.Drawing.Color.White;
            this.panelMonthlyChart.Controls.Add(this.chartMonthlySales);
            this.panelMonthlyChart.Controls.Add(this.lblMonthlyChart);
            this.panelMonthlyChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMonthlyChart.Location = new System.Drawing.Point(0, 0);
            this.panelMonthlyChart.Name = "panelMonthlyChart";
            this.panelMonthlyChart.Padding = new System.Windows.Forms.Padding(10);
            this.panelMonthlyChart.Size = new System.Drawing.Size(740, 208);
            this.panelMonthlyChart.TabIndex = 0;
            // 
            // chartMonthlySales
            // 
            chartArea1.Name = "ChartArea1";
            this.chartMonthlySales.ChartAreas.Add(chartArea1);
            this.chartMonthlySales.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartMonthlySales.Legends.Add(legend1);
            this.chartMonthlySales.Location = new System.Drawing.Point(10, 36);
            this.chartMonthlySales.Name = "chartMonthlySales";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Monthly";
            this.chartMonthlySales.Series.Add(series1);
            this.chartMonthlySales.Size = new System.Drawing.Size(720, 162);
            this.chartMonthlySales.TabIndex = 1;
            this.chartMonthlySales.Text = "chart1";
            // 
            // lblMonthlyChart
            // 
            this.lblMonthlyChart.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMonthlyChart.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblMonthlyChart.Location = new System.Drawing.Point(10, 10);
            this.lblMonthlyChart.Name = "lblMonthlyChart";
            this.lblMonthlyChart.Size = new System.Drawing.Size(720, 26);
            this.lblMonthlyChart.TabIndex = 0;
            this.lblMonthlyChart.Text = "Monthly Sales (Current Year)";
            // 
            // panelCategoryChart
            // 
            this.panelCategoryChart.BackColor = System.Drawing.Color.White;
            this.panelCategoryChart.Controls.Add(this.chartCategory);
            this.panelCategoryChart.Controls.Add(this.lblCategoryChart);
            this.panelCategoryChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCategoryChart.Location = new System.Drawing.Point(0, 0);
            this.panelCategoryChart.Name = "panelCategoryChart";
            this.panelCategoryChart.Padding = new System.Windows.Forms.Padding(10);
            this.panelCategoryChart.Size = new System.Drawing.Size(482, 208);
            this.panelCategoryChart.TabIndex = 0;
            // 
            // chartCategory
            // 
            chartArea2.Name = "ChartArea1";
            this.chartCategory.ChartAreas.Add(chartArea2);
            this.chartCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chartCategory.Legends.Add(legend2);
            this.chartCategory.Location = new System.Drawing.Point(10, 36);
            this.chartCategory.Name = "chartCategory";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Category";
            this.chartCategory.Series.Add(series2);
            this.chartCategory.Size = new System.Drawing.Size(462, 162);
            this.chartCategory.TabIndex = 1;
            this.chartCategory.Text = "chart2";
            // 
            // lblCategoryChart
            // 
            this.lblCategoryChart.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCategoryChart.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblCategoryChart.Location = new System.Drawing.Point(10, 10);
            this.lblCategoryChart.Name = "lblCategoryChart";
            this.lblCategoryChart.Size = new System.Drawing.Size(462, 26);
            this.lblCategoryChart.TabIndex = 0;
            this.lblCategoryChart.Text = "Sales by Category";
            // 
            // panelPeriod
            // 
            this.panelPeriod.BackColor = System.Drawing.Color.White;
            this.panelPeriod.Controls.Add(this.dtpTo);
            this.panelPeriod.Controls.Add(this.lblTo);
            this.panelPeriod.Controls.Add(this.dtpFrom);
            this.panelPeriod.Controls.Add(this.lblFrom);
            this.panelPeriod.Controls.Add(this.flowPeriod);
            this.panelPeriod.Controls.Add(this.lblPeriodTitle);
            this.panelPeriod.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPeriod.Location = new System.Drawing.Point(12, 259);
            this.panelPeriod.Name = "panelPeriod";
            this.panelPeriod.Padding = new System.Windows.Forms.Padding(10);
            this.panelPeriod.Size = new System.Drawing.Size(1226, 75);
            this.panelPeriod.TabIndex = 2;
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(967, 40);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(120, 20);
            this.dtpTo.TabIndex = 5;
            this.dtpTo.Visible = false;
            this.dtpTo.ValueChanged += new System.EventHandler(this.CustomDateChanged);
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(945, 44);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(20, 13);
            this.lblTo.TabIndex = 4;
            this.lblTo.Text = "To";
            this.lblTo.Visible = false;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(805, 40);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(120, 20);
            this.dtpFrom.TabIndex = 3;
            this.dtpFrom.Visible = false;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.CustomDateChanged);
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(769, 44);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(30, 13);
            this.lblFrom.TabIndex = 2;
            this.lblFrom.Text = "From";
            this.lblFrom.Visible = false;
            // 
            // flowPeriod
            // 
            this.flowPeriod.Controls.Add(this.rbToday);
            this.flowPeriod.Controls.Add(this.rbWeek);
            this.flowPeriod.Controls.Add(this.rbMonth);
            this.flowPeriod.Controls.Add(this.rbQuarter);
            this.flowPeriod.Controls.Add(this.rbYear);
            this.flowPeriod.Controls.Add(this.rbCustom);
            this.flowPeriod.Location = new System.Drawing.Point(13, 35);
            this.flowPeriod.Name = "flowPeriod";
            this.flowPeriod.Size = new System.Drawing.Size(734, 27);
            this.flowPeriod.TabIndex = 1;
            // 
            // rbToday
            // 
            this.rbToday.AutoSize = true;
            this.rbToday.Location = new System.Drawing.Point(3, 3);
            this.rbToday.Name = "rbToday";
            this.rbToday.Size = new System.Drawing.Size(55, 17);
            this.rbToday.TabIndex = 0;
            this.rbToday.TabStop = true;
            this.rbToday.Text = "Today";
            this.rbToday.UseVisualStyleBackColor = true;
            this.rbToday.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // rbWeek
            // 
            this.rbWeek.AutoSize = true;
            this.rbWeek.Location = new System.Drawing.Point(64, 3);
            this.rbWeek.Name = "rbWeek";
            this.rbWeek.Size = new System.Drawing.Size(78, 17);
            this.rbWeek.TabIndex = 1;
            this.rbWeek.TabStop = true;
            this.rbWeek.Text = "This Week";
            this.rbWeek.UseVisualStyleBackColor = true;
            this.rbWeek.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // rbMonth
            // 
            this.rbMonth.AutoSize = true;
            this.rbMonth.Location = new System.Drawing.Point(148, 3);
            this.rbMonth.Name = "rbMonth";
            this.rbMonth.Size = new System.Drawing.Size(80, 17);
            this.rbMonth.TabIndex = 2;
            this.rbMonth.TabStop = true;
            this.rbMonth.Text = "This Month";
            this.rbMonth.UseVisualStyleBackColor = true;
            this.rbMonth.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // rbQuarter
            // 
            this.rbQuarter.AutoSize = true;
            this.rbQuarter.Location = new System.Drawing.Point(234, 3);
            this.rbQuarter.Name = "rbQuarter";
            this.rbQuarter.Size = new System.Drawing.Size(85, 17);
            this.rbQuarter.TabIndex = 3;
            this.rbQuarter.TabStop = true;
            this.rbQuarter.Text = "This Quarter";
            this.rbQuarter.UseVisualStyleBackColor = true;
            this.rbQuarter.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // rbYear
            // 
            this.rbYear.AutoSize = true;
            this.rbYear.Location = new System.Drawing.Point(325, 3);
            this.rbYear.Name = "rbYear";
            this.rbYear.Size = new System.Drawing.Size(72, 17);
            this.rbYear.TabIndex = 4;
            this.rbYear.TabStop = true;
            this.rbYear.Text = "This Year";
            this.rbYear.UseVisualStyleBackColor = true;
            this.rbYear.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // rbCustom
            // 
            this.rbCustom.AutoSize = true;
            this.rbCustom.Location = new System.Drawing.Point(403, 3);
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.Size = new System.Drawing.Size(97, 17);
            this.rbCustom.TabIndex = 5;
            this.rbCustom.TabStop = true;
            this.rbCustom.Text = "Custom Range";
            this.rbCustom.UseVisualStyleBackColor = true;
            this.rbCustom.CheckedChanged += new System.EventHandler(this.PeriodChanged);
            // 
            // lblPeriodTitle
            // 
            this.lblPeriodTitle.AutoSize = true;
            this.lblPeriodTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblPeriodTitle.Location = new System.Drawing.Point(10, 10);
            this.lblPeriodTitle.Name = "lblPeriodTitle";
            this.lblPeriodTitle.Size = new System.Drawing.Size(104, 19);
            this.lblPeriodTitle.TabIndex = 0;
            this.lblPeriodTitle.Text = "Select Period";
            // 
            // panelKpis
            // 
            this.panelKpis.ColumnCount = 5;
            this.panelKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelKpis.Controls.Add(this.cardTotalSales, 0, 0);
            this.panelKpis.Controls.Add(this.cardInvoices, 1, 0);
            this.panelKpis.Controls.Add(this.cardPaid, 2, 0);
            this.panelKpis.Controls.Add(this.cardOutstanding, 3, 0);
            this.panelKpis.Controls.Add(this.cardAvgInvoice, 4, 0);
            this.panelKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelKpis.Location = new System.Drawing.Point(12, 57);
            this.panelKpis.Name = "panelKpis";
            this.panelKpis.RowCount = 1;
            this.panelKpis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelKpis.Size = new System.Drawing.Size(1226, 202);
            this.panelKpis.TabIndex = 1;
            // 
            // cardTotalSales
            // 
            this.cardTotalSales.BackColor = System.Drawing.Color.White;
            this.cardTotalSales.Controls.Add(this.lblTotalSalesTrend);
            this.cardTotalSales.Controls.Add(this.lblTotalSalesValue);
            this.cardTotalSales.Controls.Add(this.lblTotalSalesTitle);
            this.cardTotalSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTotalSales.Location = new System.Drawing.Point(3, 3);
            this.cardTotalSales.Name = "cardTotalSales";
            this.cardTotalSales.Padding = new System.Windows.Forms.Padding(10);
            this.cardTotalSales.Size = new System.Drawing.Size(239, 196);
            this.cardTotalSales.TabIndex = 0;
            // 
            // lblTotalSalesTrend
            // 
            this.lblTotalSalesTrend.AutoSize = true;
            this.lblTotalSalesTrend.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotalSalesTrend.Location = new System.Drawing.Point(13, 98);
            this.lblTotalSalesTrend.Name = "lblTotalSalesTrend";
            this.lblTotalSalesTrend.Size = new System.Drawing.Size(121, 15);
            this.lblTotalSalesTrend.TabIndex = 2;
            this.lblTotalSalesTrend.Text = "▲ +0.00% vs last month";
            // 
            // lblTotalSalesValue
            // 
            this.lblTotalSalesValue.AutoSize = true;
            this.lblTotalSalesValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            this.lblTotalSalesValue.Location = new System.Drawing.Point(10, 56);
            this.lblTotalSalesValue.Name = "lblTotalSalesValue";
            this.lblTotalSalesValue.Size = new System.Drawing.Size(74, 32);
            this.lblTotalSalesValue.TabIndex = 1;
            this.lblTotalSalesValue.Text = "0.00";
            // 
            // lblTotalSalesTitle
            // 
            this.lblTotalSalesTitle.AutoSize = true;
            this.lblTotalSalesTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblTotalSalesTitle.Location = new System.Drawing.Point(10, 16);
            this.lblTotalSalesTitle.Name = "lblTotalSalesTitle";
            this.lblTotalSalesTitle.Size = new System.Drawing.Size(159, 17);
            this.lblTotalSalesTitle.TabIndex = 0;
            this.lblTotalSalesTitle.Text = "Total Sales (This Month)";
            // 
            // cardInvoices
            // 
            this.cardInvoices.BackColor = System.Drawing.Color.White;
            this.cardInvoices.Controls.Add(this.lblInvoicesValue);
            this.cardInvoices.Controls.Add(this.lblInvoicesTitle);
            this.cardInvoices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardInvoices.Location = new System.Drawing.Point(248, 3);
            this.cardInvoices.Name = "cardInvoices";
            this.cardInvoices.Padding = new System.Windows.Forms.Padding(10);
            this.cardInvoices.Size = new System.Drawing.Size(239, 196);
            this.cardInvoices.TabIndex = 1;
            // 
            // lblInvoicesValue
            // 
            this.lblInvoicesValue.AutoSize = true;
            this.lblInvoicesValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            this.lblInvoicesValue.Location = new System.Drawing.Point(10, 56);
            this.lblInvoicesValue.Name = "lblInvoicesValue";
            this.lblInvoicesValue.Size = new System.Drawing.Size(28, 32);
            this.lblInvoicesValue.TabIndex = 1;
            this.lblInvoicesValue.Text = "0";
            // 
            // lblInvoicesTitle
            // 
            this.lblInvoicesTitle.AutoSize = true;
            this.lblInvoicesTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblInvoicesTitle.Location = new System.Drawing.Point(10, 16);
            this.lblInvoicesTitle.Name = "lblInvoicesTitle";
            this.lblInvoicesTitle.Size = new System.Drawing.Size(88, 17);
            this.lblInvoicesTitle.TabIndex = 0;
            this.lblInvoicesTitle.Text = "Total Invoices";
            // 
            // cardPaid
            // 
            this.cardPaid.BackColor = System.Drawing.Color.White;
            this.cardPaid.Controls.Add(this.lblPaidValue);
            this.cardPaid.Controls.Add(this.lblPaidTitle);
            this.cardPaid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardPaid.Location = new System.Drawing.Point(493, 3);
            this.cardPaid.Name = "cardPaid";
            this.cardPaid.Padding = new System.Windows.Forms.Padding(10);
            this.cardPaid.Size = new System.Drawing.Size(239, 196);
            this.cardPaid.TabIndex = 2;
            // 
            // lblPaidValue
            // 
            this.lblPaidValue.AutoSize = true;
            this.lblPaidValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            this.lblPaidValue.Location = new System.Drawing.Point(10, 56);
            this.lblPaidValue.Name = "lblPaidValue";
            this.lblPaidValue.Size = new System.Drawing.Size(74, 32);
            this.lblPaidValue.TabIndex = 1;
            this.lblPaidValue.Text = "0.00";
            // 
            // lblPaidTitle
            // 
            this.lblPaidTitle.AutoSize = true;
            this.lblPaidTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblPaidTitle.Location = new System.Drawing.Point(10, 16);
            this.lblPaidTitle.Name = "lblPaidTitle";
            this.lblPaidTitle.Size = new System.Drawing.Size(82, 17);
            this.lblPaidTitle.TabIndex = 0;
            this.lblPaidTitle.Text = "Paid Amount";
            // 
            // cardOutstanding
            // 
            this.cardOutstanding.BackColor = System.Drawing.Color.White;
            this.cardOutstanding.Controls.Add(this.lblOutstandingSub);
            this.cardOutstanding.Controls.Add(this.lblOutstandingValue);
            this.cardOutstanding.Controls.Add(this.lblOutstandingTitle);
            this.cardOutstanding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardOutstanding.Location = new System.Drawing.Point(738, 3);
            this.cardOutstanding.Name = "cardOutstanding";
            this.cardOutstanding.Padding = new System.Windows.Forms.Padding(10);
            this.cardOutstanding.Size = new System.Drawing.Size(239, 196);
            this.cardOutstanding.TabIndex = 3;
            // 
            // lblOutstandingSub
            // 
            this.lblOutstandingSub.AutoSize = true;
            this.lblOutstandingSub.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOutstandingSub.Location = new System.Drawing.Point(13, 98);
            this.lblOutstandingSub.Name = "lblOutstandingSub";
            this.lblOutstandingSub.Size = new System.Drawing.Size(84, 15);
            this.lblOutstandingSub.TabIndex = 2;
            this.lblOutstandingSub.Text = "Over 30d: 0.00";
            // 
            // lblOutstandingValue
            // 
            this.lblOutstandingValue.AutoSize = true;
            this.lblOutstandingValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            this.lblOutstandingValue.Location = new System.Drawing.Point(10, 56);
            this.lblOutstandingValue.Name = "lblOutstandingValue";
            this.lblOutstandingValue.Size = new System.Drawing.Size(74, 32);
            this.lblOutstandingValue.TabIndex = 1;
            this.lblOutstandingValue.Text = "0.00";
            // 
            // lblOutstandingTitle
            // 
            this.lblOutstandingTitle.AutoSize = true;
            this.lblOutstandingTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblOutstandingTitle.Location = new System.Drawing.Point(10, 16);
            this.lblOutstandingTitle.Name = "lblOutstandingTitle";
            this.lblOutstandingTitle.Size = new System.Drawing.Size(157, 17);
            this.lblOutstandingTitle.TabIndex = 0;
            this.lblOutstandingTitle.Text = "Outstanding / Receivable";
            // 
            // cardAvgInvoice
            // 
            this.cardAvgInvoice.BackColor = System.Drawing.Color.White;
            this.cardAvgInvoice.Controls.Add(this.lblAvgInvoiceValue);
            this.cardAvgInvoice.Controls.Add(this.lblAvgInvoiceTitle);
            this.cardAvgInvoice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardAvgInvoice.Location = new System.Drawing.Point(983, 3);
            this.cardAvgInvoice.Name = "cardAvgInvoice";
            this.cardAvgInvoice.Padding = new System.Windows.Forms.Padding(10);
            this.cardAvgInvoice.Size = new System.Drawing.Size(240, 196);
            this.cardAvgInvoice.TabIndex = 4;
            // 
            // lblAvgInvoiceValue
            // 
            this.lblAvgInvoiceValue.AutoSize = true;
            this.lblAvgInvoiceValue.Font = new System.Drawing.Font("Segoe UI Semibold", 18F);
            this.lblAvgInvoiceValue.Location = new System.Drawing.Point(10, 56);
            this.lblAvgInvoiceValue.Name = "lblAvgInvoiceValue";
            this.lblAvgInvoiceValue.Size = new System.Drawing.Size(74, 32);
            this.lblAvgInvoiceValue.TabIndex = 1;
            this.lblAvgInvoiceValue.Text = "0.00";
            // 
            // lblAvgInvoiceTitle
            // 
            this.lblAvgInvoiceTitle.AutoSize = true;
            this.lblAvgInvoiceTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblAvgInvoiceTitle.Location = new System.Drawing.Point(10, 16);
            this.lblAvgInvoiceTitle.Name = "lblAvgInvoiceTitle";
            this.lblAvgInvoiceTitle.Size = new System.Drawing.Size(132, 17);
            this.lblAvgInvoiceTitle.TabIndex = 0;
            this.lblAvgInvoiceTitle.Text = "Average Invoice Value";
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 15F);
            this.lblTitle.Location = new System.Drawing.Point(12, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1226, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Sales Dashboard";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frm_sales_dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 790);
            this.Controls.Add(this.panelRoot);
            this.Name = "frm_sales_dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sales Dashboard";
            this.Load += new System.EventHandler(this.frm_sales_dashboard_Load);
            this.panelRoot.ResumeLayout(false);
            this.panelRoot.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
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
