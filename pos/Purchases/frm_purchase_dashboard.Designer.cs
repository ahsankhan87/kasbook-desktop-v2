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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_purchase_dashboard));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.splitBottom = new System.Windows.Forms.SplitContainer();
            this.cardTopSuppliers = new System.Windows.Forms.Panel();
            this.gridTopSuppliers = new System.Windows.Forms.DataGridView();
            this.lblTopSuppliers = new System.Windows.Forms.Label();
            this.cardPendingBills = new System.Windows.Forms.Panel();
            this.gridPendingBills = new System.Windows.Forms.DataGridView();
            this.lblPendingBills = new System.Windows.Forms.Label();
            this.splitCharts = new System.Windows.Forms.SplitContainer();
            this.cardMonthlyBar = new System.Windows.Forms.Panel();
            this.chartMonthlyPurchases = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblMonthlyBar = new System.Windows.Forms.Label();
            this.cardSupplierDonut = new System.Windows.Forms.Panel();
            this.chartSupplierDonut = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblSupplierDonut = new System.Windows.Forms.Label();
            this.panelRoot = new System.Windows.Forms.Panel();
            this.cardTrend = new System.Windows.Forms.Panel();
            this.chartTrend = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblTrend = new System.Windows.Forms.Label();
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
            this.colRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierShare = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSupplierPayable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBillNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBillSupplier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBillDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDueDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBillAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDaysOverdue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).BeginInit();
            this.splitBottom.Panel1.SuspendLayout();
            this.splitBottom.Panel2.SuspendLayout();
            this.splitBottom.SuspendLayout();
            this.cardTopSuppliers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTopSuppliers)).BeginInit();
            this.cardPendingBills.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPendingBills)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitCharts)).BeginInit();
            this.splitCharts.Panel1.SuspendLayout();
            this.splitCharts.Panel2.SuspendLayout();
            this.splitCharts.SuspendLayout();
            this.cardMonthlyBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthlyPurchases)).BeginInit();
            this.cardSupplierDonut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSupplierDonut)).BeginInit();
            this.panelRoot.SuspendLayout();
            this.cardTrend.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend)).BeginInit();
            this.panelFilters.SuspendLayout();
            this.panelKpis.SuspendLayout();
            this.cardTotalPurchases.SuspendLayout();
            this.cardTotalBills.SuspendLayout();
            this.cardAmountPaid.SuspendLayout();
            this.cardPayable.SuspendLayout();
            this.cardAvgPurchase.SuspendLayout();
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
            this.splitBottom.Panel1.Controls.Add(this.cardTopSuppliers);
            // 
            // splitBottom.Panel2
            // 
            resources.ApplyResources(this.splitBottom.Panel2, "splitBottom.Panel2");
            this.splitBottom.Panel2.Controls.Add(this.cardPendingBills);
            // 
            // cardTopSuppliers
            // 
            resources.ApplyResources(this.cardTopSuppliers, "cardTopSuppliers");
            this.cardTopSuppliers.BackColor = System.Drawing.Color.White;
            this.cardTopSuppliers.Controls.Add(this.gridTopSuppliers);
            this.cardTopSuppliers.Controls.Add(this.lblTopSuppliers);
            this.cardTopSuppliers.Name = "cardTopSuppliers";
            // 
            // gridTopSuppliers
            // 
            resources.ApplyResources(this.gridTopSuppliers, "gridTopSuppliers");
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
            this.gridTopSuppliers.MultiSelect = false;
            this.gridTopSuppliers.Name = "gridTopSuppliers";
            this.gridTopSuppliers.ReadOnly = true;
            this.gridTopSuppliers.RowHeadersVisible = false;
            this.gridTopSuppliers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // lblTopSuppliers
            // 
            resources.ApplyResources(this.lblTopSuppliers, "lblTopSuppliers");
            this.lblTopSuppliers.Name = "lblTopSuppliers";
            // 
            // cardPendingBills
            // 
            resources.ApplyResources(this.cardPendingBills, "cardPendingBills");
            this.cardPendingBills.BackColor = System.Drawing.Color.White;
            this.cardPendingBills.Controls.Add(this.gridPendingBills);
            this.cardPendingBills.Controls.Add(this.lblPendingBills);
            this.cardPendingBills.Name = "cardPendingBills";
            // 
            // gridPendingBills
            // 
            resources.ApplyResources(this.gridPendingBills, "gridPendingBills");
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
            this.gridPendingBills.MultiSelect = false;
            this.gridPendingBills.Name = "gridPendingBills";
            this.gridPendingBills.ReadOnly = true;
            this.gridPendingBills.RowHeadersVisible = false;
            this.gridPendingBills.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPendingBills.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.gridPendingBills_RowPrePaint);
            // 
            // lblPendingBills
            // 
            resources.ApplyResources(this.lblPendingBills, "lblPendingBills");
            this.lblPendingBills.Name = "lblPendingBills";
            // 
            // splitCharts
            // 
            resources.ApplyResources(this.splitCharts, "splitCharts");
            this.splitCharts.Name = "splitCharts";
            // 
            // splitCharts.Panel1
            // 
            resources.ApplyResources(this.splitCharts.Panel1, "splitCharts.Panel1");
            this.splitCharts.Panel1.Controls.Add(this.cardMonthlyBar);
            // 
            // splitCharts.Panel2
            // 
            resources.ApplyResources(this.splitCharts.Panel2, "splitCharts.Panel2");
            this.splitCharts.Panel2.Controls.Add(this.cardSupplierDonut);
            // 
            // cardMonthlyBar
            // 
            resources.ApplyResources(this.cardMonthlyBar, "cardMonthlyBar");
            this.cardMonthlyBar.BackColor = System.Drawing.Color.White;
            this.cardMonthlyBar.Controls.Add(this.chartMonthlyPurchases);
            this.cardMonthlyBar.Controls.Add(this.lblMonthlyBar);
            this.cardMonthlyBar.Name = "cardMonthlyBar";
            // 
            // chartMonthlyPurchases
            // 
            resources.ApplyResources(this.chartMonthlyPurchases, "chartMonthlyPurchases");
            chartArea1.Name = "ChartArea1";
            this.chartMonthlyPurchases.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chartMonthlyPurchases.Legends.Add(legend1);
            this.chartMonthlyPurchases.Name = "chartMonthlyPurchases";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartMonthlyPurchases.Series.Add(series1);
            // 
            // lblMonthlyBar
            // 
            resources.ApplyResources(this.lblMonthlyBar, "lblMonthlyBar");
            this.lblMonthlyBar.Name = "lblMonthlyBar";
            // 
            // cardSupplierDonut
            // 
            resources.ApplyResources(this.cardSupplierDonut, "cardSupplierDonut");
            this.cardSupplierDonut.BackColor = System.Drawing.Color.White;
            this.cardSupplierDonut.Controls.Add(this.chartSupplierDonut);
            this.cardSupplierDonut.Controls.Add(this.lblSupplierDonut);
            this.cardSupplierDonut.Name = "cardSupplierDonut";
            // 
            // chartSupplierDonut
            // 
            resources.ApplyResources(this.chartSupplierDonut, "chartSupplierDonut");
            chartArea2.Name = "ChartArea1";
            this.chartSupplierDonut.ChartAreas.Add(chartArea2);
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Name = "Legend1";
            this.chartSupplierDonut.Legends.Add(legend2);
            this.chartSupplierDonut.Name = "chartSupplierDonut";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartSupplierDonut.Series.Add(series2);
            // 
            // lblSupplierDonut
            // 
            resources.ApplyResources(this.lblSupplierDonut, "lblSupplierDonut");
            this.lblSupplierDonut.Name = "lblSupplierDonut";
            // 
            // panelRoot
            // 
            resources.ApplyResources(this.panelRoot, "panelRoot");
            this.panelRoot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.panelRoot.Controls.Add(this.splitBottom);
            this.panelRoot.Controls.Add(this.cardTrend);
            this.panelRoot.Controls.Add(this.splitCharts);
            this.panelRoot.Controls.Add(this.panelFilters);
            this.panelRoot.Controls.Add(this.panelKpis);
            this.panelRoot.Controls.Add(this.lblTitle);
            this.panelRoot.Name = "panelRoot";
            // 
            // cardTrend
            // 
            resources.ApplyResources(this.cardTrend, "cardTrend");
            this.cardTrend.BackColor = System.Drawing.Color.White;
            this.cardTrend.Controls.Add(this.chartTrend);
            this.cardTrend.Controls.Add(this.lblTrend);
            this.cardTrend.Name = "cardTrend";
            // 
            // chartTrend
            // 
            resources.ApplyResources(this.chartTrend, "chartTrend");
            chartArea3.Name = "ChartArea1";
            this.chartTrend.ChartAreas.Add(chartArea3);
            legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend3.Name = "Legend1";
            this.chartTrend.Legends.Add(legend3);
            this.chartTrend.Name = "chartTrend";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartTrend.Series.Add(series3);
            // 
            // lblTrend
            // 
            resources.ApplyResources(this.lblTrend, "lblTrend");
            this.lblTrend.Name = "lblTrend";
            // 
            // panelFilters
            // 
            resources.ApplyResources(this.panelFilters, "panelFilters");
            this.panelFilters.BackColor = System.Drawing.Color.White;
            this.panelFilters.Controls.Add(this.dtpTo);
            this.panelFilters.Controls.Add(this.lblTo);
            this.panelFilters.Controls.Add(this.dtpFrom);
            this.panelFilters.Controls.Add(this.lblFrom);
            this.panelFilters.Controls.Add(this.cmbPeriod);
            this.panelFilters.Controls.Add(this.lblPeriod);
            this.panelFilters.Name = "panelFilters";
            // 
            // dtpTo
            // 
            resources.ApplyResources(this.dtpTo, "dtpTo");
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.ValueChanged += new System.EventHandler(this.dtpFromTo_ValueChanged);
            // 
            // lblTo
            // 
            resources.ApplyResources(this.lblTo, "lblTo");
            this.lblTo.Name = "lblTo";
            // 
            // dtpFrom
            // 
            resources.ApplyResources(this.dtpFrom, "dtpFrom");
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFromTo_ValueChanged);
            // 
            // lblFrom
            // 
            resources.ApplyResources(this.lblFrom, "lblFrom");
            this.lblFrom.Name = "lblFrom";
            // 
            // cmbPeriod
            // 
            resources.ApplyResources(this.cmbPeriod, "cmbPeriod");
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.FormattingEnabled = true;
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.SelectedIndexChanged += new System.EventHandler(this.cmbPeriod_SelectedIndexChanged);
            // 
            // lblPeriod
            // 
            resources.ApplyResources(this.lblPeriod, "lblPeriod");
            this.lblPeriod.Name = "lblPeriod";
            // 
            // panelKpis
            // 
            resources.ApplyResources(this.panelKpis, "panelKpis");
            this.panelKpis.Controls.Add(this.cardTotalPurchases, 0, 0);
            this.panelKpis.Controls.Add(this.cardTotalBills, 1, 0);
            this.panelKpis.Controls.Add(this.cardAmountPaid, 2, 0);
            this.panelKpis.Controls.Add(this.cardPayable, 3, 0);
            this.panelKpis.Controls.Add(this.cardAvgPurchase, 4, 0);
            this.panelKpis.Name = "panelKpis";
            // 
            // cardTotalPurchases
            // 
            resources.ApplyResources(this.cardTotalPurchases, "cardTotalPurchases");
            this.cardTotalPurchases.BackColor = System.Drawing.Color.White;
            this.cardTotalPurchases.Controls.Add(this.lblTotalPurchasesTrend);
            this.cardTotalPurchases.Controls.Add(this.lblTotalPurchasesValue);
            this.cardTotalPurchases.Controls.Add(this.lblTotalPurchasesTitle);
            this.cardTotalPurchases.Name = "cardTotalPurchases";
            // 
            // lblTotalPurchasesTrend
            // 
            resources.ApplyResources(this.lblTotalPurchasesTrend, "lblTotalPurchasesTrend");
            this.lblTotalPurchasesTrend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.lblTotalPurchasesTrend.Name = "lblTotalPurchasesTrend";
            // 
            // lblTotalPurchasesValue
            // 
            resources.ApplyResources(this.lblTotalPurchasesValue, "lblTotalPurchasesValue");
            this.lblTotalPurchasesValue.Name = "lblTotalPurchasesValue";
            // 
            // lblTotalPurchasesTitle
            // 
            resources.ApplyResources(this.lblTotalPurchasesTitle, "lblTotalPurchasesTitle");
            this.lblTotalPurchasesTitle.Name = "lblTotalPurchasesTitle";
            // 
            // cardTotalBills
            // 
            resources.ApplyResources(this.cardTotalBills, "cardTotalBills");
            this.cardTotalBills.BackColor = System.Drawing.Color.White;
            this.cardTotalBills.Controls.Add(this.lblTotalBillsValue);
            this.cardTotalBills.Controls.Add(this.lblTotalBillsTitle);
            this.cardTotalBills.Name = "cardTotalBills";
            // 
            // lblTotalBillsValue
            // 
            resources.ApplyResources(this.lblTotalBillsValue, "lblTotalBillsValue");
            this.lblTotalBillsValue.Name = "lblTotalBillsValue";
            // 
            // lblTotalBillsTitle
            // 
            resources.ApplyResources(this.lblTotalBillsTitle, "lblTotalBillsTitle");
            this.lblTotalBillsTitle.Name = "lblTotalBillsTitle";
            // 
            // cardAmountPaid
            // 
            resources.ApplyResources(this.cardAmountPaid, "cardAmountPaid");
            this.cardAmountPaid.BackColor = System.Drawing.Color.White;
            this.cardAmountPaid.Controls.Add(this.lblAmountPaidValue);
            this.cardAmountPaid.Controls.Add(this.lblAmountPaidTitle);
            this.cardAmountPaid.Name = "cardAmountPaid";
            // 
            // lblAmountPaidValue
            // 
            resources.ApplyResources(this.lblAmountPaidValue, "lblAmountPaidValue");
            this.lblAmountPaidValue.Name = "lblAmountPaidValue";
            // 
            // lblAmountPaidTitle
            // 
            resources.ApplyResources(this.lblAmountPaidTitle, "lblAmountPaidTitle");
            this.lblAmountPaidTitle.Name = "lblAmountPaidTitle";
            // 
            // cardPayable
            // 
            resources.ApplyResources(this.cardPayable, "cardPayable");
            this.cardPayable.BackColor = System.Drawing.Color.White;
            this.cardPayable.Controls.Add(this.lblPayableSub);
            this.cardPayable.Controls.Add(this.lblPayableValue);
            this.cardPayable.Controls.Add(this.lblPayableTitle);
            this.cardPayable.Name = "cardPayable";
            // 
            // lblPayableSub
            // 
            resources.ApplyResources(this.lblPayableSub, "lblPayableSub");
            this.lblPayableSub.ForeColor = System.Drawing.Color.Firebrick;
            this.lblPayableSub.Name = "lblPayableSub";
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
            // cardAvgPurchase
            // 
            resources.ApplyResources(this.cardAvgPurchase, "cardAvgPurchase");
            this.cardAvgPurchase.BackColor = System.Drawing.Color.White;
            this.cardAvgPurchase.Controls.Add(this.lblAvgPurchaseValue);
            this.cardAvgPurchase.Controls.Add(this.lblAvgPurchaseTitle);
            this.cardAvgPurchase.Name = "cardAvgPurchase";
            // 
            // lblAvgPurchaseValue
            // 
            resources.ApplyResources(this.lblAvgPurchaseValue, "lblAvgPurchaseValue");
            this.lblAvgPurchaseValue.Name = "lblAvgPurchaseValue";
            // 
            // lblAvgPurchaseTitle
            // 
            resources.ApplyResources(this.lblAvgPurchaseTitle, "lblAvgPurchaseTitle");
            this.lblAvgPurchaseTitle.Name = "lblAvgPurchaseTitle";
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // colRank
            // 
            this.colRank.DataPropertyName = "rank_no";
            this.colRank.FillWeight = 45F;
            resources.ApplyResources(this.colRank, "colRank");
            this.colRank.Name = "colRank";
            this.colRank.ReadOnly = true;
            // 
            // colSupplierName
            // 
            this.colSupplierName.DataPropertyName = "supplier_name";
            this.colSupplierName.FillWeight = 170F;
            resources.ApplyResources(this.colSupplierName, "colSupplierName");
            this.colSupplierName.Name = "colSupplierName";
            this.colSupplierName.ReadOnly = true;
            // 
            // colSupplierTotal
            // 
            this.colSupplierTotal.DataPropertyName = "total_purchases";
            this.colSupplierTotal.FillWeight = 95F;
            resources.ApplyResources(this.colSupplierTotal, "colSupplierTotal");
            this.colSupplierTotal.Name = "colSupplierTotal";
            this.colSupplierTotal.ReadOnly = true;
            // 
            // colSupplierShare
            // 
            this.colSupplierShare.DataPropertyName = "share_percent";
            this.colSupplierShare.FillWeight = 75F;
            resources.ApplyResources(this.colSupplierShare, "colSupplierShare");
            this.colSupplierShare.Name = "colSupplierShare";
            this.colSupplierShare.ReadOnly = true;
            // 
            // colSupplierPayable
            // 
            this.colSupplierPayable.DataPropertyName = "payable_amount";
            this.colSupplierPayable.FillWeight = 95F;
            resources.ApplyResources(this.colSupplierPayable, "colSupplierPayable");
            this.colSupplierPayable.Name = "colSupplierPayable";
            this.colSupplierPayable.ReadOnly = true;
            // 
            // colBillNo
            // 
            this.colBillNo.DataPropertyName = "bill_no";
            this.colBillNo.FillWeight = 82F;
            resources.ApplyResources(this.colBillNo, "colBillNo");
            this.colBillNo.Name = "colBillNo";
            this.colBillNo.ReadOnly = true;
            // 
            // colBillSupplier
            // 
            this.colBillSupplier.DataPropertyName = "supplier_name";
            this.colBillSupplier.FillWeight = 130F;
            resources.ApplyResources(this.colBillSupplier, "colBillSupplier");
            this.colBillSupplier.Name = "colBillSupplier";
            this.colBillSupplier.ReadOnly = true;
            // 
            // colBillDate
            // 
            this.colBillDate.DataPropertyName = "bill_date";
            this.colBillDate.FillWeight = 85F;
            resources.ApplyResources(this.colBillDate, "colBillDate");
            this.colBillDate.Name = "colBillDate";
            this.colBillDate.ReadOnly = true;
            // 
            // colDueDate
            // 
            this.colDueDate.DataPropertyName = "due_date";
            this.colDueDate.FillWeight = 85F;
            resources.ApplyResources(this.colDueDate, "colDueDate");
            this.colDueDate.Name = "colDueDate";
            this.colDueDate.ReadOnly = true;
            // 
            // colBillAmount
            // 
            this.colBillAmount.DataPropertyName = "amount";
            this.colBillAmount.FillWeight = 88F;
            resources.ApplyResources(this.colBillAmount, "colBillAmount");
            this.colBillAmount.Name = "colBillAmount";
            this.colBillAmount.ReadOnly = true;
            // 
            // colDaysOverdue
            // 
            this.colDaysOverdue.DataPropertyName = "days_overdue";
            this.colDaysOverdue.FillWeight = 70F;
            resources.ApplyResources(this.colDaysOverdue, "colDaysOverdue");
            this.colDaysOverdue.Name = "colDaysOverdue";
            this.colDaysOverdue.ReadOnly = true;
            // 
            // frm_purchase_dashboard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelRoot);
            this.Name = "frm_purchase_dashboard";
            this.Load += new System.EventHandler(this.frm_purchase_dashboard_Load);
            this.splitBottom.Panel1.ResumeLayout(false);
            this.splitBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBottom)).EndInit();
            this.splitBottom.ResumeLayout(false);
            this.cardTopSuppliers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTopSuppliers)).EndInit();
            this.cardPendingBills.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridPendingBills)).EndInit();
            this.splitCharts.Panel1.ResumeLayout(false);
            this.splitCharts.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCharts)).EndInit();
            this.splitCharts.ResumeLayout(false);
            this.cardMonthlyBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthlyPurchases)).EndInit();
            this.cardSupplierDonut.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartSupplierDonut)).EndInit();
            this.panelRoot.ResumeLayout(false);
            this.cardTrend.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTrend)).EndInit();
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
