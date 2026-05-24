namespace pos
{
    partial class frm_purchase_dashboard
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panelRoot = new System.Windows.Forms.Panel();
            this.splitBottom = new System.Windows.Forms.SplitContainer();
            this.cardTopSuppliers = new System.Windows.Forms.Panel();
            this.gridTopSuppliers = new System.Windows.Forms.DataGridView();
            this.colRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierShare = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierPayable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTopSuppliers = new System.Windows.Forms.Label();
            this.cardPendingBills = new System.Windows.Forms.Panel();
            this.gridPendingBills = new System.Windows.Forms.DataGridView();
            this.colBillNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBillSupplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBillDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBillAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDaysOverdue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblPendingBills = new System.Windows.Forms.Label();
            this.cardTrend = new System.Windows.Forms.Panel();
            this.chartTrend = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblTrend = new System.Windows.Forms.Label();
            this.splitCharts = new System.Windows.Forms.SplitContainer();
            this.cardMonthlyBar = new System.Windows.Forms.Panel();
            this.chartMonthlyPurchases = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblMonthlyBar = new System.Windows.Forms.Label();
            this.cardSupplierDonut = new System.Windows.Forms.Panel();
            this.chartSupplierDonut = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblSupplierDonut = new System.Windows.Forms.Label();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.cmbPeriod = new System.Windows.Forms.ComboBox();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.panelKpis = new System.Windows.Forms.TableLayoutPanel();
            this.cardTotalPurchases = new System.Windows.Forms.Panel();
            this.lblTotalPurchasesTrend = new System.Windows.Forms.Label();
            this.lblTotalPurchasesValue = new System.Windows.Forms.Label();
            this.lblTotalPurchasesTitle = new System.Windows.Forms.Label();
            this.cardTotalBills = new System.Windows.Forms.Panel();
            this.lblTotalBillsValue = new System.Windows.Forms.Label();
            this.lblTotalBillsTitle = new System.Windows.Forms.Label();
            this.cardAmountPaid = new System.Windows.Forms.Panel();
            this.lblAmountPaidValue = new System.Windows.Forms.Label();
            this.lblAmountPaidTitle = new System.Windows.Forms.Label();
            this.cardPayable = new System.Windows.Forms.Panel();
            this.lblPayableSub = new System.Windows.Forms.Label();
            this.lblPayableValue = new System.Windows.Forms.Label();
            this.lblPayableTitle = new System.Windows.Forms.Label();
            this.cardAvgPurchase = new System.Windows.Forms.Panel();
            this.lblAvgPurchaseValue = new System.Windows.Forms.Label();
            this.lblAvgPurchaseTitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelRoot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).BeginInit();
            this.splitBottom.Panel1.SuspendLayout();
            this.splitBottom.Panel2.SuspendLayout();
            this.splitBottom.SuspendLayout();
            this.cardTopSuppliers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTopSuppliers)).BeginInit();
            this.cardPendingBills.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPendingBills)).BeginInit();
            this.cardTrend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitCharts)).BeginInit();
            this.splitCharts.Panel1.SuspendLayout();
            this.splitCharts.Panel2.SuspendLayout();
            this.splitCharts.SuspendLayout();
            this.cardMonthlyBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthlyPurchases)).BeginInit();
            this.cardSupplierDonut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSupplierDonut)).BeginInit();
            this.panelFilters.SuspendLayout();
            this.panelKpis.SuspendLayout();
            this.cardTotalPurchases.SuspendLayout();
            this.cardTotalBills.SuspendLayout();
            this.cardAmountPaid.SuspendLayout();
            this.cardPayable.SuspendLayout();
            this.cardAvgPurchase.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRoot
            // 
            this.panelRoot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.panelRoot.Controls.Add(this.splitBottom);
            this.panelRoot.Controls.Add(this.cardTrend);
            this.panelRoot.Controls.Add(this.splitCharts);
            this.panelRoot.Controls.Add(this.panelFilters);
            this.panelRoot.Controls.Add(this.panelKpis);
            this.panelRoot.Controls.Add(this.lblTitle);
            this.panelRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRoot.Location = new System.Drawing.Point(0, 0);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Padding = new System.Windows.Forms.Padding(12);
            this.panelRoot.Size = new System.Drawing.Size(1220, 760);
            this.panelRoot.TabIndex = 0;
            // 
            // splitBottom
            // 
            this.splitBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBottom.Location = new System.Drawing.Point(12, 549);
            this.splitBottom.Name = "splitBottom";
            // 
            // splitBottom.Panel1
            // 
            this.splitBottom.Panel1.Controls.Add(this.cardTopSuppliers);
            // 
            // splitBottom.Panel2
            // 
            this.splitBottom.Panel2.Controls.Add(this.cardPendingBills);
            this.splitBottom.Size = new System.Drawing.Size(1196, 199);
            this.splitBottom.SplitterDistance = 666;
            this.splitBottom.TabIndex = 5;
            // 
            // cardTopSuppliers
            // 
            this.cardTopSuppliers.BackColor = System.Drawing.Color.White;
            this.cardTopSuppliers.Controls.Add(this.gridTopSuppliers);
            this.cardTopSuppliers.Controls.Add(this.lblTopSuppliers);
            this.cardTopSuppliers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTopSuppliers.Location = new System.Drawing.Point(0, 0);
            this.cardTopSuppliers.Name = "cardTopSuppliers";
            this.cardTopSuppliers.Padding = new System.Windows.Forms.Padding(10);
            this.cardTopSuppliers.Size = new System.Drawing.Size(666, 199);
            this.cardTopSuppliers.TabIndex = 0;
            // 
            // gridTopSuppliers
            // 
            this.gridTopSuppliers.AllowUserToAddRows = false;
            this.gridTopSuppliers.AllowUserToDeleteRows = false;
            this.gridTopSuppliers.AllowUserToResizeRows = false;
            this.gridTopSuppliers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridTopSuppliers.BackgroundColor = System.Drawing.Color.White;
            this.gridTopSuppliers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridTopSuppliers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTopSuppliers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRank,
            this.colSupplierName,
            this.colSupplierTotal,
            this.colSupplierShare,
            this.colSupplierPayable});
            this.gridTopSuppliers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTopSuppliers.Location = new System.Drawing.Point(10, 35);
            this.gridTopSuppliers.MultiSelect = false;
            this.gridTopSuppliers.Name = "gridTopSuppliers";
            this.gridTopSuppliers.ReadOnly = true;
            this.gridTopSuppliers.RowHeadersVisible = false;
            this.gridTopSuppliers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTopSuppliers.Size = new System.Drawing.Size(646, 154);
            this.gridTopSuppliers.TabIndex = 1;
            // 
            // colRank
            // 
            this.colRank.DataPropertyName = "rank_no";
            this.colRank.FillWeight = 45F;
            this.colRank.HeaderText = "Rank";
            this.colRank.Name = "colRank";
            this.colRank.ReadOnly = true;
            // 
            // colSupplierName
            // 
            this.colSupplierName.DataPropertyName = "supplier_name";
            this.colSupplierName.FillWeight = 170F;
            this.colSupplierName.HeaderText = "Supplier Name";
            this.colSupplierName.Name = "colSupplierName";
            this.colSupplierName.ReadOnly = true;
            // 
            // colSupplierTotal
            // 
            this.colSupplierTotal.DataPropertyName = "total_purchases";
            this.colSupplierTotal.FillWeight = 95F;
            this.colSupplierTotal.HeaderText = "Total Purchases";
            this.colSupplierTotal.Name = "colSupplierTotal";
            this.colSupplierTotal.ReadOnly = true;
            // 
            // colSupplierShare
            // 
            this.colSupplierShare.DataPropertyName = "share_percent";
            this.colSupplierShare.FillWeight = 75F;
            this.colSupplierShare.HeaderText = "% of Total";
            this.colSupplierShare.Name = "colSupplierShare";
            this.colSupplierShare.ReadOnly = true;
            // 
            // colSupplierPayable
            // 
            this.colSupplierPayable.DataPropertyName = "payable_amount";
            this.colSupplierPayable.FillWeight = 95F;
            this.colSupplierPayable.HeaderText = "Payable Amount";
            this.colSupplierPayable.Name = "colSupplierPayable";
            this.colSupplierPayable.ReadOnly = true;
            // 
            // lblTopSuppliers
            // 
            this.lblTopSuppliers.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTopSuppliers.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblTopSuppliers.Location = new System.Drawing.Point(10, 10);
            this.lblTopSuppliers.Name = "lblTopSuppliers";
            this.lblTopSuppliers.Size = new System.Drawing.Size(646, 25);
            this.lblTopSuppliers.TabIndex = 0;
            this.lblTopSuppliers.Text = "Top 10 Suppliers";
            // 
            // cardPendingBills
            // 
            this.cardPendingBills.BackColor = System.Drawing.Color.White;
            this.cardPendingBills.Controls.Add(this.gridPendingBills);
            this.cardPendingBills.Controls.Add(this.lblPendingBills);
            this.cardPendingBills.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardPendingBills.Location = new System.Drawing.Point(0, 0);
            this.cardPendingBills.Name = "cardPendingBills";
            this.cardPendingBills.Padding = new System.Windows.Forms.Padding(10);
            this.cardPendingBills.Size = new System.Drawing.Size(526, 199);
            this.cardPendingBills.TabIndex = 0;
            // 
            // gridPendingBills
            // 
            this.gridPendingBills.AllowUserToAddRows = false;
            this.gridPendingBills.AllowUserToDeleteRows = false;
            this.gridPendingBills.AllowUserToResizeRows = false;
            this.gridPendingBills.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridPendingBills.BackgroundColor = System.Drawing.Color.White;
            this.gridPendingBills.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridPendingBills.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPendingBills.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBillNo,
            this.colBillSupplier,
            this.colBillDate,
            this.colDueDate,
            this.colBillAmount,
            this.colDaysOverdue});
            this.gridPendingBills.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPendingBills.Location = new System.Drawing.Point(10, 35);
            this.gridPendingBills.MultiSelect = false;
            this.gridPendingBills.Name = "gridPendingBills";
            this.gridPendingBills.ReadOnly = true;
            this.gridPendingBills.RowHeadersVisible = false;
            this.gridPendingBills.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPendingBills.Size = new System.Drawing.Size(506, 154);
            this.gridPendingBills.TabIndex = 1;
            this.gridPendingBills.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.gridPendingBills_RowPrePaint);
            // 
            // colBillNo
            // 
            this.colBillNo.DataPropertyName = "bill_no";
            this.colBillNo.FillWeight = 82F;
            this.colBillNo.HeaderText = "Bill No";
            this.colBillNo.Name = "colBillNo";
            this.colBillNo.ReadOnly = true;
            // 
            // colBillSupplier
            // 
            this.colBillSupplier.DataPropertyName = "supplier_name";
            this.colBillSupplier.FillWeight = 130F;
            this.colBillSupplier.HeaderText = "Supplier";
            this.colBillSupplier.Name = "colBillSupplier";
            this.colBillSupplier.ReadOnly = true;
            // 
            // colBillDate
            // 
            this.colBillDate.DataPropertyName = "bill_date";
            this.colBillDate.FillWeight = 85F;
            this.colBillDate.HeaderText = "Bill Date";
            this.colBillDate.Name = "colBillDate";
            this.colBillDate.ReadOnly = true;
            // 
            // colDueDate
            // 
            this.colDueDate.DataPropertyName = "due_date";
            this.colDueDate.FillWeight = 85F;
            this.colDueDate.HeaderText = "Due Date";
            this.colDueDate.Name = "colDueDate";
            this.colDueDate.ReadOnly = true;
            // 
            // colBillAmount
            // 
            this.colBillAmount.DataPropertyName = "amount";
            this.colBillAmount.FillWeight = 88F;
            this.colBillAmount.HeaderText = "Amount";
            this.colBillAmount.Name = "colBillAmount";
            this.colBillAmount.ReadOnly = true;
            // 
            // colDaysOverdue
            // 
            this.colDaysOverdue.DataPropertyName = "days_overdue";
            this.colDaysOverdue.FillWeight = 70F;
            this.colDaysOverdue.HeaderText = "Days Overdue";
            this.colDaysOverdue.Name = "colDaysOverdue";
            this.colDaysOverdue.ReadOnly = true;
            // 
            // lblPendingBills
            // 
            this.lblPendingBills.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPendingBills.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblPendingBills.Location = new System.Drawing.Point(10, 10);
            this.lblPendingBills.Name = "lblPendingBills";
            this.lblPendingBills.Size = new System.Drawing.Size(506, 25);
            this.lblPendingBills.TabIndex = 0;
            this.lblPendingBills.Text = "Pending Bills";
            // 
            // cardTrend
            // 
            this.cardTrend.BackColor = System.Drawing.Color.White;
            this.cardTrend.Controls.Add(this.chartTrend);
            this.cardTrend.Controls.Add(this.lblTrend);
            this.cardTrend.Dock = System.Windows.Forms.DockStyle.Top;
            this.cardTrend.Location = new System.Drawing.Point(12, 377);
            this.cardTrend.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.cardTrend.Name = "cardTrend";
            this.cardTrend.Padding = new System.Windows.Forms.Padding(10);
            this.cardTrend.Size = new System.Drawing.Size(1196, 172);
            this.cardTrend.TabIndex = 4;
            // 
            // chartTrend
            // 
            chartArea1.Name = "ChartArea1";
            this.chartTrend.ChartAreas.Add(chartArea1);
            this.chartTrend.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "Legend1";
            this.chartTrend.Legends.Add(legend1);
            this.chartTrend.Location = new System.Drawing.Point(10, 35);
            this.chartTrend.Name = "chartTrend";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartTrend.Series.Add(series1);
            this.chartTrend.Size = new System.Drawing.Size(1176, 127);
            this.chartTrend.TabIndex = 1;
            this.chartTrend.Text = "chart1";
            // 
            // lblTrend
            // 
            this.lblTrend.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTrend.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblTrend.Location = new System.Drawing.Point(10, 10);
            this.lblTrend.Name = "lblTrend";
            this.lblTrend.Size = new System.Drawing.Size(1176, 25);
            this.lblTrend.TabIndex = 0;
            this.lblTrend.Text = "Purchase Trend (This Year vs Last Year)";
            // 
            // splitCharts
            // 
            this.splitCharts.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitCharts.Location = new System.Drawing.Point(12, 215);
            this.splitCharts.Name = "splitCharts";
            // 
            // splitCharts.Panel1
            // 
            this.splitCharts.Panel1.Controls.Add(this.cardMonthlyBar);
            // 
            // splitCharts.Panel2
            // 
            this.splitCharts.Panel2.Controls.Add(this.cardSupplierDonut);
            this.splitCharts.Size = new System.Drawing.Size(1196, 162);
            this.splitCharts.SplitterDistance = 710;
            this.splitCharts.TabIndex = 3;
            // 
            // cardMonthlyBar
            // 
            this.cardMonthlyBar.BackColor = System.Drawing.Color.White;
            this.cardMonthlyBar.Controls.Add(this.chartMonthlyPurchases);
            this.cardMonthlyBar.Controls.Add(this.lblMonthlyBar);
            this.cardMonthlyBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardMonthlyBar.Location = new System.Drawing.Point(0, 0);
            this.cardMonthlyBar.Name = "cardMonthlyBar";
            this.cardMonthlyBar.Padding = new System.Windows.Forms.Padding(10);
            this.cardMonthlyBar.Size = new System.Drawing.Size(710, 162);
            this.cardMonthlyBar.TabIndex = 0;
            // 
            // chartMonthlyPurchases
            // 
            chartArea2.Name = "ChartArea1";
            this.chartMonthlyPurchases.ChartAreas.Add(chartArea2);
            this.chartMonthlyPurchases.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Enabled = false;
            legend2.Name = "Legend1";
            this.chartMonthlyPurchases.Legends.Add(legend2);
            this.chartMonthlyPurchases.Location = new System.Drawing.Point(10, 35);
            this.chartMonthlyPurchases.Name = "chartMonthlyPurchases";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartMonthlyPurchases.Series.Add(series2);
            this.chartMonthlyPurchases.Size = new System.Drawing.Size(690, 117);
            this.chartMonthlyPurchases.TabIndex = 1;
            this.chartMonthlyPurchases.Text = "chart1";
            // 
            // lblMonthlyBar
            // 
            this.lblMonthlyBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMonthlyBar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblMonthlyBar.Location = new System.Drawing.Point(10, 10);
            this.lblMonthlyBar.Name = "lblMonthlyBar";
            this.lblMonthlyBar.Size = new System.Drawing.Size(690, 25);
            this.lblMonthlyBar.TabIndex = 0;
            this.lblMonthlyBar.Text = "Monthly Purchases (12 Months)";
            // 
            // cardSupplierDonut
            // 
            this.cardSupplierDonut.BackColor = System.Drawing.Color.White;
            this.cardSupplierDonut.Controls.Add(this.chartSupplierDonut);
            this.cardSupplierDonut.Controls.Add(this.lblSupplierDonut);
            this.cardSupplierDonut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardSupplierDonut.Location = new System.Drawing.Point(0, 0);
            this.cardSupplierDonut.Name = "cardSupplierDonut";
            this.cardSupplierDonut.Padding = new System.Windows.Forms.Padding(10);
            this.cardSupplierDonut.Size = new System.Drawing.Size(482, 162);
            this.cardSupplierDonut.TabIndex = 0;
            // 
            // chartSupplierDonut
            // 
            chartArea3.Name = "ChartArea1";
            this.chartSupplierDonut.ChartAreas.Add(chartArea3);
            this.chartSupplierDonut.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend3.Name = "Legend1";
            this.chartSupplierDonut.Legends.Add(legend3);
            this.chartSupplierDonut.Location = new System.Drawing.Point(10, 35);
            this.chartSupplierDonut.Name = "chartSupplierDonut";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartSupplierDonut.Series.Add(series3);
            this.chartSupplierDonut.Size = new System.Drawing.Size(462, 117);
            this.chartSupplierDonut.TabIndex = 1;
            this.chartSupplierDonut.Text = "chart1";
            // 
            // lblSupplierDonut
            // 
            this.lblSupplierDonut.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSupplierDonut.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblSupplierDonut.Location = new System.Drawing.Point(10, 10);
            this.lblSupplierDonut.Name = "lblSupplierDonut";
            this.lblSupplierDonut.Size = new System.Drawing.Size(462, 25);
            this.lblSupplierDonut.TabIndex = 0;
            this.lblSupplierDonut.Text = "Purchases by Supplier (Top 5 + Others)";
            // 
            // panelFilters
            // 
            this.panelFilters.BackColor = System.Drawing.Color.White;
            this.panelFilters.Controls.Add(this.dtpTo);
            this.panelFilters.Controls.Add(this.lblTo);
            this.panelFilters.Controls.Add(this.dtpFrom);
            this.panelFilters.Controls.Add(this.lblFrom);
            this.panelFilters.Controls.Add(this.cmbPeriod);
            this.panelFilters.Controls.Add(this.lblPeriod);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(12, 175);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.panelFilters.Size = new System.Drawing.Size(1196, 40);
            this.panelFilters.TabIndex = 2;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "yyyy-MM-dd";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(470, 9);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(130, 23);
            this.dtpTo.TabIndex = 5;
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpFromTo_ValueChanged);
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(438, 13);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(21, 15);
            this.lblTo.TabIndex = 4;
            this.lblTo.Text = "To";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "yyyy-MM-dd";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(290, 9);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(130, 23);
            this.dtpFrom.TabIndex = 3;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFromTo_ValueChanged);
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(248, 13);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(36, 15);
            this.lblFrom.TabIndex = 2;
            this.lblFrom.Text = "From";
            // 
            // cmbPeriod
            // 
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.FormattingEnabled = true;
            this.cmbPeriod.Location = new System.Drawing.Point(63, 9);
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.Size = new System.Drawing.Size(167, 23);
            this.cmbPeriod.TabIndex = 1;
            this.cmbPeriod.SelectedIndexChanged += new System.EventHandler(this.cmbPeriod_SelectedIndexChanged);
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point(12, 13);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(42, 15);
            this.lblPeriod.TabIndex = 0;
            this.lblPeriod.Text = "Period";
            // 
            // panelKpis
            // 
            this.panelKpis.ColumnCount = 5;
            this.panelKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelKpis.Controls.Add(this.cardTotalPurchases, 0, 0);
            this.panelKpis.Controls.Add(this.cardTotalBills, 1, 0);
            this.panelKpis.Controls.Add(this.cardAmountPaid, 2, 0);
            this.panelKpis.Controls.Add(this.cardPayable, 3, 0);
            this.panelKpis.Controls.Add(this.cardAvgPurchase, 4, 0);
            this.panelKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelKpis.Location = new System.Drawing.Point(12, 52);
            this.panelKpis.Name = "panelKpis";
            this.panelKpis.RowCount = 1;
            this.panelKpis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelKpis.Size = new System.Drawing.Size(1196, 123);
            this.panelKpis.TabIndex = 1;
            // 
            // cardTotalPurchases
            // 
            this.cardTotalPurchases.BackColor = System.Drawing.Color.White;
            this.cardTotalPurchases.Controls.Add(this.lblTotalPurchasesTrend);
            this.cardTotalPurchases.Controls.Add(this.lblTotalPurchasesValue);
            this.cardTotalPurchases.Controls.Add(this.lblTotalPurchasesTitle);
            this.cardTotalPurchases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTotalPurchases.Location = new System.Drawing.Point(3, 3);
            this.cardTotalPurchases.Name = "cardTotalPurchases";
            this.cardTotalPurchases.Padding = new System.Windows.Forms.Padding(10);
            this.cardTotalPurchases.Size = new System.Drawing.Size(233, 117);
            this.cardTotalPurchases.TabIndex = 0;
            // 
            // lblTotalPurchasesTrend
            // 
            this.lblTotalPurchasesTrend.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalPurchasesTrend.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblTotalPurchasesTrend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.lblTotalPurchasesTrend.Location = new System.Drawing.Point(10, 75);
            this.lblTotalPurchasesTrend.Name = "lblTotalPurchasesTrend";
            this.lblTotalPurchasesTrend.Size = new System.Drawing.Size(213, 21);
            this.lblTotalPurchasesTrend.TabIndex = 2;
            this.lblTotalPurchasesTrend.Text = "0.00% vs last period";
            // 
            // lblTotalPurchasesValue
            // 
            this.lblTotalPurchasesValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalPurchasesValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblTotalPurchasesValue.Location = new System.Drawing.Point(10, 40);
            this.lblTotalPurchasesValue.Name = "lblTotalPurchasesValue";
            this.lblTotalPurchasesValue.Size = new System.Drawing.Size(213, 35);
            this.lblTotalPurchasesValue.TabIndex = 1;
            this.lblTotalPurchasesValue.Text = "0.00";
            // 
            // lblTotalPurchasesTitle
            // 
            this.lblTotalPurchasesTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalPurchasesTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotalPurchasesTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTotalPurchasesTitle.Name = "lblTotalPurchasesTitle";
            this.lblTotalPurchasesTitle.Size = new System.Drawing.Size(213, 30);
            this.lblTotalPurchasesTitle.TabIndex = 0;
            this.lblTotalPurchasesTitle.Text = "Total Purchases";
            // 
            // cardTotalBills
            // 
            this.cardTotalBills.BackColor = System.Drawing.Color.White;
            this.cardTotalBills.Controls.Add(this.lblTotalBillsValue);
            this.cardTotalBills.Controls.Add(this.lblTotalBillsTitle);
            this.cardTotalBills.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardTotalBills.Location = new System.Drawing.Point(242, 3);
            this.cardTotalBills.Name = "cardTotalBills";
            this.cardTotalBills.Padding = new System.Windows.Forms.Padding(10);
            this.cardTotalBills.Size = new System.Drawing.Size(233, 117);
            this.cardTotalBills.TabIndex = 1;
            // 
            // lblTotalBillsValue
            // 
            this.lblTotalBillsValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalBillsValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblTotalBillsValue.Location = new System.Drawing.Point(10, 40);
            this.lblTotalBillsValue.Name = "lblTotalBillsValue";
            this.lblTotalBillsValue.Size = new System.Drawing.Size(213, 35);
            this.lblTotalBillsValue.TabIndex = 1;
            this.lblTotalBillsValue.Text = "0";
            // 
            // lblTotalBillsTitle
            // 
            this.lblTotalBillsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTotalBillsTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotalBillsTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTotalBillsTitle.Name = "lblTotalBillsTitle";
            this.lblTotalBillsTitle.Size = new System.Drawing.Size(213, 30);
            this.lblTotalBillsTitle.TabIndex = 0;
            this.lblTotalBillsTitle.Text = "Total Bills / GRNs";
            // 
            // cardAmountPaid
            // 
            this.cardAmountPaid.BackColor = System.Drawing.Color.White;
            this.cardAmountPaid.Controls.Add(this.lblAmountPaidValue);
            this.cardAmountPaid.Controls.Add(this.lblAmountPaidTitle);
            this.cardAmountPaid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardAmountPaid.Location = new System.Drawing.Point(481, 3);
            this.cardAmountPaid.Name = "cardAmountPaid";
            this.cardAmountPaid.Padding = new System.Windows.Forms.Padding(10);
            this.cardAmountPaid.Size = new System.Drawing.Size(233, 117);
            this.cardAmountPaid.TabIndex = 2;
            // 
            // lblAmountPaidValue
            // 
            this.lblAmountPaidValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAmountPaidValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblAmountPaidValue.Location = new System.Drawing.Point(10, 40);
            this.lblAmountPaidValue.Name = "lblAmountPaidValue";
            this.lblAmountPaidValue.Size = new System.Drawing.Size(213, 35);
            this.lblAmountPaidValue.TabIndex = 1;
            this.lblAmountPaidValue.Text = "0.00";
            // 
            // lblAmountPaidTitle
            // 
            this.lblAmountPaidTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAmountPaidTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAmountPaidTitle.Location = new System.Drawing.Point(10, 10);
            this.lblAmountPaidTitle.Name = "lblAmountPaidTitle";
            this.lblAmountPaidTitle.Size = new System.Drawing.Size(213, 30);
            this.lblAmountPaidTitle.TabIndex = 0;
            this.lblAmountPaidTitle.Text = "Amount Paid";
            // 
            // cardPayable
            // 
            this.cardPayable.BackColor = System.Drawing.Color.White;
            this.cardPayable.Controls.Add(this.lblPayableSub);
            this.cardPayable.Controls.Add(this.lblPayableValue);
            this.cardPayable.Controls.Add(this.lblPayableTitle);
            this.cardPayable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardPayable.Location = new System.Drawing.Point(720, 3);
            this.cardPayable.Name = "cardPayable";
            this.cardPayable.Padding = new System.Windows.Forms.Padding(10);
            this.cardPayable.Size = new System.Drawing.Size(233, 117);
            this.cardPayable.TabIndex = 3;
            // 
            // lblPayableSub
            // 
            this.lblPayableSub.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPayableSub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblPayableSub.ForeColor = System.Drawing.Color.Firebrick;
            this.lblPayableSub.Location = new System.Drawing.Point(10, 75);
            this.lblPayableSub.Name = "lblPayableSub";
            this.lblPayableSub.Size = new System.Drawing.Size(213, 21);
            this.lblPayableSub.TabIndex = 2;
            this.lblPayableSub.Text = "Overdue: 0.00";
            // 
            // lblPayableValue
            // 
            this.lblPayableValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPayableValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblPayableValue.Location = new System.Drawing.Point(10, 40);
            this.lblPayableValue.Name = "lblPayableValue";
            this.lblPayableValue.Size = new System.Drawing.Size(213, 35);
            this.lblPayableValue.TabIndex = 1;
            this.lblPayableValue.Text = "0.00";
            // 
            // lblPayableTitle
            // 
            this.lblPayableTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPayableTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPayableTitle.Location = new System.Drawing.Point(10, 10);
            this.lblPayableTitle.Name = "lblPayableTitle";
            this.lblPayableTitle.Size = new System.Drawing.Size(213, 30);
            this.lblPayableTitle.TabIndex = 0;
            this.lblPayableTitle.Text = "Payable / Outstanding";
            // 
            // cardAvgPurchase
            // 
            this.cardAvgPurchase.BackColor = System.Drawing.Color.White;
            this.cardAvgPurchase.Controls.Add(this.lblAvgPurchaseValue);
            this.cardAvgPurchase.Controls.Add(this.lblAvgPurchaseTitle);
            this.cardAvgPurchase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardAvgPurchase.Location = new System.Drawing.Point(959, 3);
            this.cardAvgPurchase.Name = "cardAvgPurchase";
            this.cardAvgPurchase.Padding = new System.Windows.Forms.Padding(10);
            this.cardAvgPurchase.Size = new System.Drawing.Size(234, 117);
            this.cardAvgPurchase.TabIndex = 4;
            // 
            // lblAvgPurchaseValue
            // 
            this.lblAvgPurchaseValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAvgPurchaseValue.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblAvgPurchaseValue.Location = new System.Drawing.Point(10, 40);
            this.lblAvgPurchaseValue.Name = "lblAvgPurchaseValue";
            this.lblAvgPurchaseValue.Size = new System.Drawing.Size(214, 35);
            this.lblAvgPurchaseValue.TabIndex = 1;
            this.lblAvgPurchaseValue.Text = "0.00";
            // 
            // lblAvgPurchaseTitle
            // 
            this.lblAvgPurchaseTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAvgPurchaseTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAvgPurchaseTitle.Location = new System.Drawing.Point(10, 10);
            this.lblAvgPurchaseTitle.Name = "lblAvgPurchaseTitle";
            this.lblAvgPurchaseTitle.Size = new System.Drawing.Size(214, 30);
            this.lblAvgPurchaseTitle.TabIndex = 0;
            this.lblAvgPurchaseTitle.Text = "Average Purchase Value";
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblTitle.Location = new System.Drawing.Point(12, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1196, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Purchase Dashboard";
            // 
            // frm_purchase_dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 760);
            this.Controls.Add(this.panelRoot);
            this.MinimumSize = new System.Drawing.Size(920, 640);
            this.Name = "frm_purchase_dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Purchase Dashboard";
            this.Load += new System.EventHandler(this.frm_purchase_dashboard_Load);
            this.panelRoot.ResumeLayout(false);
            this.splitBottom.Panel1.ResumeLayout(false);
            this.splitBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).EndInit();
            this.splitBottom.ResumeLayout(false);
            this.cardTopSuppliers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTopSuppliers)).EndInit();
            this.cardPendingBills.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPendingBills)).EndInit();
            this.cardTrend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend)).EndInit();
            this.splitCharts.Panel1.ResumeLayout(false);
            this.splitCharts.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCharts)).EndInit();
            this.splitCharts.ResumeLayout(false);
            this.cardMonthlyBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthlyPurchases)).EndInit();
            this.cardSupplierDonut.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartSupplierDonut)).EndInit();
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.panelKpis.ResumeLayout(false);
            this.cardTotalPurchases.ResumeLayout(false);
            this.cardTotalBills.ResumeLayout(false);
            this.cardAmountPaid.ResumeLayout(false);
            this.cardPayable.ResumeLayout(false);
            this.cardAvgPurchase.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelRoot;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel panelKpis;
        private System.Windows.Forms.Panel cardTotalPurchases;
        private System.Windows.Forms.Label lblTotalPurchasesTrend;
        private System.Windows.Forms.Label lblTotalPurchasesValue;
        private System.Windows.Forms.Label lblTotalPurchasesTitle;
        private System.Windows.Forms.Panel cardTotalBills;
        private System.Windows.Forms.Label lblTotalBillsValue;
        private System.Windows.Forms.Label lblTotalBillsTitle;
        private System.Windows.Forms.Panel cardAmountPaid;
        private System.Windows.Forms.Label lblAmountPaidValue;
        private System.Windows.Forms.Label lblAmountPaidTitle;
        private System.Windows.Forms.Panel cardPayable;
        private System.Windows.Forms.Label lblPayableSub;
        private System.Windows.Forms.Label lblPayableValue;
        private System.Windows.Forms.Label lblPayableTitle;
        private System.Windows.Forms.Panel cardAvgPurchase;
        private System.Windows.Forms.Label lblAvgPurchaseValue;
        private System.Windows.Forms.Label lblAvgPurchaseTitle;
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.ComboBox cmbPeriod;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.SplitContainer splitCharts;
        private System.Windows.Forms.Panel cardMonthlyBar;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMonthlyPurchases;
        private System.Windows.Forms.Label lblMonthlyBar;
        private System.Windows.Forms.Panel cardSupplierDonut;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSupplierDonut;
        private System.Windows.Forms.Label lblSupplierDonut;
        private System.Windows.Forms.Panel cardTrend;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTrend;
        private System.Windows.Forms.Label lblTrend;
        private System.Windows.Forms.SplitContainer splitBottom;
        private System.Windows.Forms.Panel cardTopSuppliers;
        private System.Windows.Forms.DataGridView gridTopSuppliers;
        private System.Windows.Forms.Label lblTopSuppliers;
        private System.Windows.Forms.Panel cardPendingBills;
        private System.Windows.Forms.DataGridView gridPendingBills;
        private System.Windows.Forms.Label lblPendingBills;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierShare;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSupplierPayable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBillNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBillSupplier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBillDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDueDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBillAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDaysOverdue;
    }
}
