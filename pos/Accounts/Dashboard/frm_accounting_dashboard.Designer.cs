namespace pos
{
    partial class frm_accounting_dashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_accounting_dashboard));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.splitMiddle = new System.Windows.Forms.SplitContainer();
            this.gridUnreconciledBanks = new System.Windows.Forms.DataGridView();
            this.colBankName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBankReconcile = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lblUnreconciledTitle = new System.Windows.Forms.Label();
            this.gridAttention = new System.Windows.Forms.DataGridView();
            this.colAttentionItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttentionCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttentionAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAttentionAction = new System.Windows.Forms.DataGridViewButtonColumn();
            this.lblAttentionTitle = new System.Windows.Forms.Label();
            this.gridRecentJournals = new System.Windows.Forms.DataGridView();
            this.lblJournalTitle = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cmbPeriod = new System.Windows.Forms.ComboBox();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.panelCharts = new System.Windows.Forms.Panel();
            this.tableCharts = new System.Windows.Forms.TableLayoutPanel();
            this.panelChartPnl = new System.Windows.Forms.Panel();
            this.chartPnl = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblChartPnl = new System.Windows.Forms.Label();
            this.panelChartCash = new System.Windows.Forms.Panel();
            this.chartCashFlow = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblChartCash = new System.Windows.Forms.Label();
            this.panelChartExpense = new System.Windows.Forms.Panel();
            this.chartExpense = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblChartExpense = new System.Windows.Forms.Label();
            this.panelKpis = new System.Windows.Forms.Panel();
            this.tableKpis = new System.Windows.Forms.TableLayoutPanel();
            this.cardCash = new System.Windows.Forms.Panel();
            this.lblCashValue = new System.Windows.Forms.Label();
            this.lblCashTitle = new System.Windows.Forms.Label();
            this.cardReceivable = new System.Windows.Forms.Panel();
            this.lblReceivableValue = new System.Windows.Forms.Label();
            this.lblReceivableTitle = new System.Windows.Forms.Label();
            this.cardPayable = new System.Windows.Forms.Panel();
            this.lblPayableValue = new System.Windows.Forms.Label();
            this.lblPayableTitle = new System.Windows.Forms.Label();
            this.cardRevenue = new System.Windows.Forms.Panel();
            this.lblRevenueValue = new System.Windows.Forms.Label();
            this.lblRevenueTitle = new System.Windows.Forms.Label();
            this.cardExpenses = new System.Windows.Forms.Panel();
            this.lblExpensesValue = new System.Windows.Forms.Label();
            this.lblExpensesTitle = new System.Windows.Forms.Label();
            this.cardNetProfit = new System.Windows.Forms.Panel();
            this.lblNetProfitValue = new System.Windows.Forms.Label();
            this.lblNetProfitTitle = new System.Windows.Forms.Label();
            this.panelBottomActions = new System.Windows.Forms.Panel();
            this.flowQuickActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNewJv = new System.Windows.Forms.Button();
            this.btnReceivePayment = new System.Windows.Forms.Button();
            this.btnMakePayment = new System.Windows.Forms.Button();
            this.btnBankRec = new System.Windows.Forms.Button();
            this.btnRunPL = new System.Windows.Forms.Button();
            this.btnRunBalanceSheet = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitMiddle)).BeginInit();
            this.splitMiddle.Panel1.SuspendLayout();
            this.splitMiddle.Panel2.SuspendLayout();
            this.splitMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUnreconciledBanks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttention)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRecentJournals)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelMiddle.SuspendLayout();
            this.panelCharts.SuspendLayout();
            this.tableCharts.SuspendLayout();
            this.panelChartPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartPnl)).BeginInit();
            this.panelChartCash.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartCashFlow)).BeginInit();
            this.panelChartExpense.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartExpense)).BeginInit();
            this.panelKpis.SuspendLayout();
            this.tableKpis.SuspendLayout();
            this.cardCash.SuspendLayout();
            this.cardReceivable.SuspendLayout();
            this.cardPayable.SuspendLayout();
            this.cardRevenue.SuspendLayout();
            this.cardExpenses.SuspendLayout();
            this.cardNetProfit.SuspendLayout();
            this.panelBottomActions.SuspendLayout();
            this.flowQuickActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMiddle
            // 
            resources.ApplyResources(this.splitMiddle, "splitMiddle");
            this.splitMiddle.Name = "splitMiddle";
            // 
            // splitMiddle.Panel1
            // 
            this.splitMiddle.Panel1.Controls.Add(this.gridUnreconciledBanks);
            this.splitMiddle.Panel1.Controls.Add(this.lblUnreconciledTitle);
            this.splitMiddle.Panel1.Controls.Add(this.gridAttention);
            this.splitMiddle.Panel1.Controls.Add(this.lblAttentionTitle);
            // 
            // splitMiddle.Panel2
            // 
            this.splitMiddle.Panel2.Controls.Add(this.gridRecentJournals);
            this.splitMiddle.Panel2.Controls.Add(this.lblJournalTitle);
            // 
            // gridUnreconciledBanks
            // 
            this.gridUnreconciledBanks.AllowUserToAddRows = false;
            this.gridUnreconciledBanks.AllowUserToDeleteRows = false;
            this.gridUnreconciledBanks.AllowUserToResizeRows = false;
            this.gridUnreconciledBanks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridUnreconciledBanks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBankName,
            this.colBankReconcile});
            resources.ApplyResources(this.gridUnreconciledBanks, "gridUnreconciledBanks");
            this.gridUnreconciledBanks.Name = "gridUnreconciledBanks";
            this.gridUnreconciledBanks.RowHeadersVisible = false;
            this.gridUnreconciledBanks.RowTemplate.Height = 28;
            this.gridUnreconciledBanks.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridUnreconciledBanks_CellContentClick);
            // 
            // colBankName
            // 
            this.colBankName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.colBankName, "colBankName");
            this.colBankName.Name = "colBankName";
            this.colBankName.ReadOnly = true;
            // 
            // colBankReconcile
            // 
            resources.ApplyResources(this.colBankReconcile, "colBankReconcile");
            this.colBankReconcile.Name = "colBankReconcile";
            this.colBankReconcile.Text = "Reconcile";
            this.colBankReconcile.UseColumnTextForButtonValue = true;
            // 
            // lblUnreconciledTitle
            // 
            resources.ApplyResources(this.lblUnreconciledTitle, "lblUnreconciledTitle");
            this.lblUnreconciledTitle.Name = "lblUnreconciledTitle";
            // 
            // gridAttention
            // 
            this.gridAttention.AllowUserToAddRows = false;
            this.gridAttention.AllowUserToDeleteRows = false;
            this.gridAttention.AllowUserToResizeRows = false;
            this.gridAttention.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAttention.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAttentionItem,
            this.colAttentionCount,
            this.colAttentionAmount,
            this.colAttentionAction});
            resources.ApplyResources(this.gridAttention, "gridAttention");
            this.gridAttention.Name = "gridAttention";
            this.gridAttention.RowHeadersVisible = false;
            this.gridAttention.RowTemplate.Height = 30;
            this.gridAttention.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridAttention_CellContentClick);
            // 
            // colAttentionItem
            // 
            this.colAttentionItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.colAttentionItem, "colAttentionItem");
            this.colAttentionItem.Name = "colAttentionItem";
            this.colAttentionItem.ReadOnly = true;
            // 
            // colAttentionCount
            // 
            resources.ApplyResources(this.colAttentionCount, "colAttentionCount");
            this.colAttentionCount.Name = "colAttentionCount";
            this.colAttentionCount.ReadOnly = true;
            // 
            // colAttentionAmount
            // 
            resources.ApplyResources(this.colAttentionAmount, "colAttentionAmount");
            this.colAttentionAmount.Name = "colAttentionAmount";
            this.colAttentionAmount.ReadOnly = true;
            // 
            // colAttentionAction
            // 
            resources.ApplyResources(this.colAttentionAction, "colAttentionAction");
            this.colAttentionAction.Name = "colAttentionAction";
            this.colAttentionAction.Text = "Open";
            this.colAttentionAction.UseColumnTextForButtonValue = true;
            // 
            // lblAttentionTitle
            // 
            resources.ApplyResources(this.lblAttentionTitle, "lblAttentionTitle");
            this.lblAttentionTitle.Name = "lblAttentionTitle";
            // 
            // gridRecentJournals
            // 
            this.gridRecentJournals.AllowUserToAddRows = false;
            this.gridRecentJournals.AllowUserToDeleteRows = false;
            this.gridRecentJournals.AllowUserToResizeRows = false;
            this.gridRecentJournals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.gridRecentJournals, "gridRecentJournals");
            this.gridRecentJournals.Name = "gridRecentJournals";
            this.gridRecentJournals.RowHeadersVisible = false;
            this.gridRecentJournals.RowTemplate.Height = 30;
            this.gridRecentJournals.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridRecentJournals_CellFormatting);
            // 
            // lblJournalTitle
            // 
            resources.ApplyResources(this.lblJournalTitle, "lblJournalTitle");
            this.lblJournalTitle.Name = "lblJournalTitle";
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.btnRefresh);
            this.panelHeader.Controls.Add(this.cmbPeriod);
            this.panelHeader.Controls.Add(this.lblPeriod);
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.Name = "panelHeader";
            // 
            // btnRefresh
            // 
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cmbPeriod
            // 
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.FormattingEnabled = true;
            resources.ApplyResources(this.cmbPeriod, "cmbPeriod");
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.SelectedIndexChanged += new System.EventHandler(this.cmbPeriod_SelectedIndexChanged);
            // 
            // lblPeriod
            // 
            resources.ApplyResources(this.lblPeriod, "lblPeriod");
            this.lblPeriod.Name = "lblPeriod";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelMiddle);
            this.panelMain.Controls.Add(this.panelCharts);
            this.panelMain.Controls.Add(this.panelKpis);
            this.panelMain.Controls.Add(this.panelBottomActions);
            resources.ApplyResources(this.panelMain, "panelMain");
            this.panelMain.Name = "panelMain";
            // 
            // panelMiddle
            // 
            this.panelMiddle.Controls.Add(this.splitMiddle);
            resources.ApplyResources(this.panelMiddle, "panelMiddle");
            this.panelMiddle.Name = "panelMiddle";
            // 
            // panelCharts
            // 
            this.panelCharts.Controls.Add(this.tableCharts);
            resources.ApplyResources(this.panelCharts, "panelCharts");
            this.panelCharts.Name = "panelCharts";
            // 
            // tableCharts
            // 
            resources.ApplyResources(this.tableCharts, "tableCharts");
            this.tableCharts.Controls.Add(this.panelChartPnl, 0, 0);
            this.tableCharts.Controls.Add(this.panelChartCash, 1, 0);
            this.tableCharts.Controls.Add(this.panelChartExpense, 2, 0);
            this.tableCharts.Name = "tableCharts";
            // 
            // panelChartPnl
            // 
            this.panelChartPnl.Controls.Add(this.chartPnl);
            this.panelChartPnl.Controls.Add(this.lblChartPnl);
            resources.ApplyResources(this.panelChartPnl, "panelChartPnl");
            this.panelChartPnl.Name = "panelChartPnl";
            // 
            // chartPnl
            // 
            chartArea1.Name = "ChartArea1";
            this.chartPnl.ChartAreas.Add(chartArea1);
            resources.ApplyResources(this.chartPnl, "chartPnl");
            legend1.Name = "Legend1";
            this.chartPnl.Legends.Add(legend1);
            this.chartPnl.Name = "chartPnl";
            // 
            // lblChartPnl
            // 
            resources.ApplyResources(this.lblChartPnl, "lblChartPnl");
            this.lblChartPnl.Name = "lblChartPnl";
            // 
            // panelChartCash
            // 
            this.panelChartCash.Controls.Add(this.chartCashFlow);
            this.panelChartCash.Controls.Add(this.lblChartCash);
            resources.ApplyResources(this.panelChartCash, "panelChartCash");
            this.panelChartCash.Name = "panelChartCash";
            // 
            // chartCashFlow
            // 
            chartArea2.Name = "ChartArea1";
            this.chartCashFlow.ChartAreas.Add(chartArea2);
            resources.ApplyResources(this.chartCashFlow, "chartCashFlow");
            legend2.Name = "Legend1";
            this.chartCashFlow.Legends.Add(legend2);
            this.chartCashFlow.Name = "chartCashFlow";
            // 
            // lblChartCash
            // 
            resources.ApplyResources(this.lblChartCash, "lblChartCash");
            this.lblChartCash.Name = "lblChartCash";
            // 
            // panelChartExpense
            // 
            this.panelChartExpense.Controls.Add(this.chartExpense);
            this.panelChartExpense.Controls.Add(this.lblChartExpense);
            resources.ApplyResources(this.panelChartExpense, "panelChartExpense");
            this.panelChartExpense.Name = "panelChartExpense";
            // 
            // chartExpense
            // 
            chartArea3.Name = "ChartArea1";
            this.chartExpense.ChartAreas.Add(chartArea3);
            resources.ApplyResources(this.chartExpense, "chartExpense");
            legend3.Name = "Legend1";
            this.chartExpense.Legends.Add(legend3);
            this.chartExpense.Name = "chartExpense";
            // 
            // lblChartExpense
            // 
            resources.ApplyResources(this.lblChartExpense, "lblChartExpense");
            this.lblChartExpense.Name = "lblChartExpense";
            // 
            // panelKpis
            // 
            this.panelKpis.Controls.Add(this.tableKpis);
            resources.ApplyResources(this.panelKpis, "panelKpis");
            this.panelKpis.Name = "panelKpis";
            // 
            // tableKpis
            // 
            resources.ApplyResources(this.tableKpis, "tableKpis");
            this.tableKpis.Controls.Add(this.cardCash, 0, 0);
            this.tableKpis.Controls.Add(this.cardReceivable, 1, 0);
            this.tableKpis.Controls.Add(this.cardPayable, 2, 0);
            this.tableKpis.Controls.Add(this.cardRevenue, 0, 1);
            this.tableKpis.Controls.Add(this.cardExpenses, 1, 1);
            this.tableKpis.Controls.Add(this.cardNetProfit, 2, 1);
            this.tableKpis.Name = "tableKpis";
            // 
            // cardCash
            // 
            this.cardCash.Controls.Add(this.lblCashValue);
            this.cardCash.Controls.Add(this.lblCashTitle);
            resources.ApplyResources(this.cardCash, "cardCash");
            this.cardCash.Name = "cardCash";
            this.cardCash.Click += new System.EventHandler(this.cardCash_Click);
            // 
            // lblCashValue
            // 
            resources.ApplyResources(this.lblCashValue, "lblCashValue");
            this.lblCashValue.Name = "lblCashValue";
            // 
            // lblCashTitle
            // 
            resources.ApplyResources(this.lblCashTitle, "lblCashTitle");
            this.lblCashTitle.Name = "lblCashTitle";
            // 
            // cardReceivable
            // 
            this.cardReceivable.Controls.Add(this.lblReceivableValue);
            this.cardReceivable.Controls.Add(this.lblReceivableTitle);
            resources.ApplyResources(this.cardReceivable, "cardReceivable");
            this.cardReceivable.Name = "cardReceivable";
            this.cardReceivable.Click += new System.EventHandler(this.cardReceivable_Click);
            // 
            // lblReceivableValue
            // 
            resources.ApplyResources(this.lblReceivableValue, "lblReceivableValue");
            this.lblReceivableValue.Name = "lblReceivableValue";
            // 
            // lblReceivableTitle
            // 
            resources.ApplyResources(this.lblReceivableTitle, "lblReceivableTitle");
            this.lblReceivableTitle.Name = "lblReceivableTitle";
            // 
            // cardPayable
            // 
            this.cardPayable.Controls.Add(this.lblPayableValue);
            this.cardPayable.Controls.Add(this.lblPayableTitle);
            resources.ApplyResources(this.cardPayable, "cardPayable");
            this.cardPayable.Name = "cardPayable";
            this.cardPayable.Click += new System.EventHandler(this.cardPayable_Click);
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
            // cardRevenue
            // 
            this.cardRevenue.Controls.Add(this.lblRevenueValue);
            this.cardRevenue.Controls.Add(this.lblRevenueTitle);
            resources.ApplyResources(this.cardRevenue, "cardRevenue");
            this.cardRevenue.Name = "cardRevenue";
            this.cardRevenue.Click += new System.EventHandler(this.cardRevenue_Click);
            // 
            // lblRevenueValue
            // 
            resources.ApplyResources(this.lblRevenueValue, "lblRevenueValue");
            this.lblRevenueValue.Name = "lblRevenueValue";
            // 
            // lblRevenueTitle
            // 
            resources.ApplyResources(this.lblRevenueTitle, "lblRevenueTitle");
            this.lblRevenueTitle.Name = "lblRevenueTitle";
            // 
            // cardExpenses
            // 
            this.cardExpenses.Controls.Add(this.lblExpensesValue);
            this.cardExpenses.Controls.Add(this.lblExpensesTitle);
            resources.ApplyResources(this.cardExpenses, "cardExpenses");
            this.cardExpenses.Name = "cardExpenses";
            this.cardExpenses.Click += new System.EventHandler(this.cardRevenue_Click);
            // 
            // lblExpensesValue
            // 
            resources.ApplyResources(this.lblExpensesValue, "lblExpensesValue");
            this.lblExpensesValue.Name = "lblExpensesValue";
            // 
            // lblExpensesTitle
            // 
            resources.ApplyResources(this.lblExpensesTitle, "lblExpensesTitle");
            this.lblExpensesTitle.Name = "lblExpensesTitle";
            // 
            // cardNetProfit
            // 
            this.cardNetProfit.Controls.Add(this.lblNetProfitValue);
            this.cardNetProfit.Controls.Add(this.lblNetProfitTitle);
            resources.ApplyResources(this.cardNetProfit, "cardNetProfit");
            this.cardNetProfit.Name = "cardNetProfit";
            // 
            // lblNetProfitValue
            // 
            resources.ApplyResources(this.lblNetProfitValue, "lblNetProfitValue");
            this.lblNetProfitValue.Name = "lblNetProfitValue";
            // 
            // lblNetProfitTitle
            // 
            resources.ApplyResources(this.lblNetProfitTitle, "lblNetProfitTitle");
            this.lblNetProfitTitle.Name = "lblNetProfitTitle";
            // 
            // panelBottomActions
            // 
            this.panelBottomActions.Controls.Add(this.flowQuickActions);
            resources.ApplyResources(this.panelBottomActions, "panelBottomActions");
            this.panelBottomActions.Name = "panelBottomActions";
            // 
            // flowQuickActions
            // 
            this.flowQuickActions.Controls.Add(this.btnNewJv);
            this.flowQuickActions.Controls.Add(this.btnReceivePayment);
            this.flowQuickActions.Controls.Add(this.btnMakePayment);
            this.flowQuickActions.Controls.Add(this.btnBankRec);
            this.flowQuickActions.Controls.Add(this.btnRunPL);
            this.flowQuickActions.Controls.Add(this.btnRunBalanceSheet);
            resources.ApplyResources(this.flowQuickActions, "flowQuickActions");
            this.flowQuickActions.Name = "flowQuickActions";
            // 
            // btnNewJv
            // 
            resources.ApplyResources(this.btnNewJv, "btnNewJv");
            this.btnNewJv.Name = "btnNewJv";
            this.btnNewJv.UseVisualStyleBackColor = true;
            this.btnNewJv.Click += new System.EventHandler(this.btnNewJv_Click);
            // 
            // btnReceivePayment
            // 
            resources.ApplyResources(this.btnReceivePayment, "btnReceivePayment");
            this.btnReceivePayment.Name = "btnReceivePayment";
            this.btnReceivePayment.UseVisualStyleBackColor = true;
            this.btnReceivePayment.Click += new System.EventHandler(this.btnReceivePayment_Click);
            // 
            // btnMakePayment
            // 
            resources.ApplyResources(this.btnMakePayment, "btnMakePayment");
            this.btnMakePayment.Name = "btnMakePayment";
            this.btnMakePayment.UseVisualStyleBackColor = true;
            this.btnMakePayment.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnBankRec
            // 
            resources.ApplyResources(this.btnBankRec, "btnBankRec");
            this.btnBankRec.Name = "btnBankRec";
            this.btnBankRec.UseVisualStyleBackColor = true;
            this.btnBankRec.Click += new System.EventHandler(this.btnBankRec_Click);
            // 
            // btnRunPL
            // 
            resources.ApplyResources(this.btnRunPL, "btnRunPL");
            this.btnRunPL.Name = "btnRunPL";
            this.btnRunPL.UseVisualStyleBackColor = true;
            this.btnRunPL.Click += new System.EventHandler(this.btnRunPL_Click);
            // 
            // btnRunBalanceSheet
            // 
            resources.ApplyResources(this.btnRunBalanceSheet, "btnRunBalanceSheet");
            this.btnRunBalanceSheet.Name = "btnRunBalanceSheet";
            this.btnRunBalanceSheet.UseVisualStyleBackColor = true;
            this.btnRunBalanceSheet.Click += new System.EventHandler(this.btnRunBalanceSheet_Click);
            // 
            // frm_accounting_dashboard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelHeader);
            this.Name = "frm_accounting_dashboard";
            this.Load += new System.EventHandler(this.frm_accounting_dashboard_Load);
            this.splitMiddle.Panel1.ResumeLayout(false);
            this.splitMiddle.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMiddle)).EndInit();
            this.splitMiddle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridUnreconciledBanks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttention)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRecentJournals)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMiddle.ResumeLayout(false);
            this.panelCharts.ResumeLayout(false);
            this.tableCharts.ResumeLayout(false);
            this.panelChartPnl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartPnl)).EndInit();
            this.panelChartCash.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartCashFlow)).EndInit();
            this.panelChartExpense.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartExpense)).EndInit();
            this.panelKpis.ResumeLayout(false);
            this.tableKpis.ResumeLayout(false);
            this.cardCash.ResumeLayout(false);
            this.cardCash.PerformLayout();
            this.cardReceivable.ResumeLayout(false);
            this.cardReceivable.PerformLayout();
            this.cardPayable.ResumeLayout(false);
            this.cardPayable.PerformLayout();
            this.cardRevenue.ResumeLayout(false);
            this.cardRevenue.PerformLayout();
            this.cardExpenses.ResumeLayout(false);
            this.cardExpenses.PerformLayout();
            this.cardNetProfit.ResumeLayout(false);
            this.cardNetProfit.PerformLayout();
            this.panelBottomActions.ResumeLayout(false);
            this.flowQuickActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cmbPeriod;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelKpis;
        private System.Windows.Forms.TableLayoutPanel tableKpis;
        private System.Windows.Forms.Panel cardCash;
        private System.Windows.Forms.Label lblCashValue;
        private System.Windows.Forms.Label lblCashTitle;
        private System.Windows.Forms.Panel cardReceivable;
        private System.Windows.Forms.Label lblReceivableValue;
        private System.Windows.Forms.Label lblReceivableTitle;
        private System.Windows.Forms.Panel cardPayable;
        private System.Windows.Forms.Label lblPayableValue;
        private System.Windows.Forms.Label lblPayableTitle;
        private System.Windows.Forms.Panel cardRevenue;
        private System.Windows.Forms.Label lblRevenueValue;
        private System.Windows.Forms.Label lblRevenueTitle;
        private System.Windows.Forms.Panel cardExpenses;
        private System.Windows.Forms.Label lblExpensesValue;
        private System.Windows.Forms.Label lblExpensesTitle;
        private System.Windows.Forms.Panel cardNetProfit;
        private System.Windows.Forms.Label lblNetProfitValue;
        private System.Windows.Forms.Label lblNetProfitTitle;
        private System.Windows.Forms.Panel panelCharts;
        private System.Windows.Forms.TableLayoutPanel tableCharts;
        private System.Windows.Forms.Panel panelChartPnl;
        private System.Windows.Forms.Label lblChartPnl;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPnl;
        private System.Windows.Forms.Panel panelChartCash;
        private System.Windows.Forms.Label lblChartCash;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCashFlow;
        private System.Windows.Forms.Panel panelChartExpense;
        private System.Windows.Forms.Label lblChartExpense;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartExpense;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.SplitContainer splitMiddle;
        private System.Windows.Forms.Label lblAttentionTitle;
        private System.Windows.Forms.DataGridView gridAttention;
        private System.Windows.Forms.Label lblUnreconciledTitle;
        private System.Windows.Forms.DataGridView gridUnreconciledBanks;
        private System.Windows.Forms.Label lblJournalTitle;
        private System.Windows.Forms.DataGridView gridRecentJournals;
        private System.Windows.Forms.Panel panelBottomActions;
        private System.Windows.Forms.FlowLayoutPanel flowQuickActions;
        private System.Windows.Forms.Button btnNewJv;
        private System.Windows.Forms.Button btnReceivePayment;
        private System.Windows.Forms.Button btnMakePayment;
        private System.Windows.Forms.Button btnBankRec;
        private System.Windows.Forms.Button btnRunPL;
        private System.Windows.Forms.Button btnRunBalanceSheet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttentionItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttentionCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAttentionAmount;
        private System.Windows.Forms.DataGridViewButtonColumn colAttentionAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBankName;
        private System.Windows.Forms.DataGridViewButtonColumn colBankReconcile;
    }
}
