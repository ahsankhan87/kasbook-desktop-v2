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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cmbPeriod = new System.Windows.Forms.ComboBox();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelMiddle = new System.Windows.Forms.Panel();
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
            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMiddle)).BeginInit();
            this.splitMiddle.Panel1.SuspendLayout();
            this.splitMiddle.Panel2.SuspendLayout();
            this.splitMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUnreconciledBanks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttention)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRecentJournals)).BeginInit();
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
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.btnRefresh);
            this.panelHeader.Controls.Add(this.cmbPeriod);
            this.panelHeader.Controls.Add(this.lblPeriod);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(16, 12, 16, 8);
            this.panelHeader.Size = new System.Drawing.Size(1484, 56);
            this.panelHeader.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(320, 14);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(96, 30);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cmbPeriod
            // 
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.FormattingEnabled = true;
            this.cmbPeriod.Location = new System.Drawing.Point(106, 15);
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.Size = new System.Drawing.Size(200, 33);
            this.cmbPeriod.TabIndex = 1;
            this.cmbPeriod.SelectedIndexChanged += new System.EventHandler(this.cmbPeriod_SelectedIndexChanged);
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point(20, 19);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(70, 25);
            this.lblPeriod.TabIndex = 0;
            this.lblPeriod.Text = "Period:";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelMiddle);
            this.panelMain.Controls.Add(this.panelCharts);
            this.panelMain.Controls.Add(this.panelKpis);
            this.panelMain.Controls.Add(this.panelBottomActions);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 56);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(12);
            this.panelMain.Size = new System.Drawing.Size(1484, 705);
            this.panelMain.TabIndex = 1;
            // 
            // panelMiddle
            // 
            this.panelMiddle.Controls.Add(this.splitMiddle);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(12, 446);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelMiddle.Size = new System.Drawing.Size(1460, 195);
            this.panelMiddle.TabIndex = 2;
            // 
            // splitMiddle
            // 
            this.splitMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMiddle.Location = new System.Drawing.Point(0, 8);
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
            this.splitMiddle.Size = new System.Drawing.Size(1460, 187);
            this.splitMiddle.SplitterDistance = 721;
            this.splitMiddle.TabIndex = 0;
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
            this.gridUnreconciledBanks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridUnreconciledBanks.Location = new System.Drawing.Point(0, 194);
            this.gridUnreconciledBanks.Name = "gridUnreconciledBanks";
            this.gridUnreconciledBanks.RowHeadersVisible = false;
            this.gridUnreconciledBanks.RowHeadersWidth = 51;
            this.gridUnreconciledBanks.RowTemplate.Height = 28;
            this.gridUnreconciledBanks.Size = new System.Drawing.Size(721, 0);
            this.gridUnreconciledBanks.TabIndex = 3;
            this.gridUnreconciledBanks.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridUnreconciledBanks_CellContentClick);
            // 
            // colBankName
            // 
            this.colBankName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colBankName.HeaderText = "Bank Account";
            this.colBankName.MinimumWidth = 6;
            this.colBankName.Name = "colBankName";
            this.colBankName.ReadOnly = true;
            // 
            // colBankReconcile
            // 
            this.colBankReconcile.HeaderText = "Action";
            this.colBankReconcile.MinimumWidth = 6;
            this.colBankReconcile.Name = "colBankReconcile";
            this.colBankReconcile.Text = "Reconcile";
            this.colBankReconcile.UseColumnTextForButtonValue = true;
            this.colBankReconcile.Width = 125;
            // 
            // lblUnreconciledTitle
            // 
            this.lblUnreconciledTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUnreconciledTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblUnreconciledTitle.Location = new System.Drawing.Point(0, 170);
            this.lblUnreconciledTitle.Name = "lblUnreconciledTitle";
            this.lblUnreconciledTitle.Size = new System.Drawing.Size(721, 24);
            this.lblUnreconciledTitle.TabIndex = 2;
            this.lblUnreconciledTitle.Text = "Bank accounts not reconciled this month";
            this.lblUnreconciledTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.gridAttention.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridAttention.Location = new System.Drawing.Point(0, 28);
            this.gridAttention.Name = "gridAttention";
            this.gridAttention.RowHeadersVisible = false;
            this.gridAttention.RowHeadersWidth = 51;
            this.gridAttention.RowTemplate.Height = 30;
            this.gridAttention.Size = new System.Drawing.Size(721, 142);
            this.gridAttention.TabIndex = 1;
            this.gridAttention.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridAttention_CellContentClick);
            // 
            // colAttentionItem
            // 
            this.colAttentionItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAttentionItem.HeaderText = "Item";
            this.colAttentionItem.MinimumWidth = 6;
            this.colAttentionItem.Name = "colAttentionItem";
            this.colAttentionItem.ReadOnly = true;
            // 
            // colAttentionCount
            // 
            this.colAttentionCount.HeaderText = "Count";
            this.colAttentionCount.MinimumWidth = 6;
            this.colAttentionCount.Name = "colAttentionCount";
            this.colAttentionCount.ReadOnly = true;
            this.colAttentionCount.Width = 125;
            // 
            // colAttentionAmount
            // 
            this.colAttentionAmount.HeaderText = "Amount";
            this.colAttentionAmount.MinimumWidth = 6;
            this.colAttentionAmount.Name = "colAttentionAmount";
            this.colAttentionAmount.ReadOnly = true;
            this.colAttentionAmount.Width = 125;
            // 
            // colAttentionAction
            // 
            this.colAttentionAction.HeaderText = "Action";
            this.colAttentionAction.MinimumWidth = 6;
            this.colAttentionAction.Name = "colAttentionAction";
            this.colAttentionAction.Text = "Open";
            this.colAttentionAction.UseColumnTextForButtonValue = true;
            this.colAttentionAction.Width = 125;
            // 
            // lblAttentionTitle
            // 
            this.lblAttentionTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAttentionTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblAttentionTitle.Location = new System.Drawing.Point(0, 0);
            this.lblAttentionTitle.Name = "lblAttentionTitle";
            this.lblAttentionTitle.Size = new System.Drawing.Size(721, 28);
            this.lblAttentionTitle.TabIndex = 0;
            this.lblAttentionTitle.Text = "Accounts Requiring Attention";
            this.lblAttentionTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gridRecentJournals
            // 
            this.gridRecentJournals.AllowUserToAddRows = false;
            this.gridRecentJournals.AllowUserToDeleteRows = false;
            this.gridRecentJournals.AllowUserToResizeRows = false;
            this.gridRecentJournals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRecentJournals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRecentJournals.Location = new System.Drawing.Point(0, 28);
            this.gridRecentJournals.Name = "gridRecentJournals";
            this.gridRecentJournals.RowHeadersVisible = false;
            this.gridRecentJournals.RowHeadersWidth = 51;
            this.gridRecentJournals.RowTemplate.Height = 30;
            this.gridRecentJournals.Size = new System.Drawing.Size(735, 159);
            this.gridRecentJournals.TabIndex = 1;
            this.gridRecentJournals.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridRecentJournals_CellFormatting);
            // 
            // lblJournalTitle
            // 
            this.lblJournalTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblJournalTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblJournalTitle.Location = new System.Drawing.Point(0, 0);
            this.lblJournalTitle.Name = "lblJournalTitle";
            this.lblJournalTitle.Size = new System.Drawing.Size(735, 28);
            this.lblJournalTitle.TabIndex = 0;
            this.lblJournalTitle.Text = "Recent Journal Activity";
            this.lblJournalTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelCharts
            // 
            this.panelCharts.Controls.Add(this.tableCharts);
            this.panelCharts.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCharts.Location = new System.Drawing.Point(12, 232);
            this.panelCharts.Name = "panelCharts";
            this.panelCharts.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelCharts.Size = new System.Drawing.Size(1460, 214);
            this.panelCharts.TabIndex = 1;
            // 
            // tableCharts
            // 
            this.tableCharts.ColumnCount = 3;
            this.tableCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableCharts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableCharts.Controls.Add(this.panelChartPnl, 0, 0);
            this.tableCharts.Controls.Add(this.panelChartCash, 1, 0);
            this.tableCharts.Controls.Add(this.panelChartExpense, 2, 0);
            this.tableCharts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableCharts.Location = new System.Drawing.Point(0, 8);
            this.tableCharts.Name = "tableCharts";
            this.tableCharts.RowCount = 1;
            this.tableCharts.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableCharts.Size = new System.Drawing.Size(1460, 206);
            this.tableCharts.TabIndex = 0;
            // 
            // panelChartPnl
            // 
            this.panelChartPnl.Controls.Add(this.chartPnl);
            this.panelChartPnl.Controls.Add(this.lblChartPnl);
            this.panelChartPnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChartPnl.Location = new System.Drawing.Point(3, 3);
            this.panelChartPnl.Name = "panelChartPnl";
            this.panelChartPnl.Padding = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.panelChartPnl.Size = new System.Drawing.Size(480, 200);
            this.panelChartPnl.TabIndex = 0;
            // 
            // chartPnl
            // 
            chartArea1.Name = "ChartArea1";
            this.chartPnl.ChartAreas.Add(chartArea1);
            this.chartPnl.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartPnl.Legends.Add(legend1);
            this.chartPnl.Location = new System.Drawing.Point(0, 24);
            this.chartPnl.Name = "chartPnl";
            this.chartPnl.Size = new System.Drawing.Size(474, 176);
            this.chartPnl.TabIndex = 1;
            this.chartPnl.Text = "chart1";
            // 
            // lblChartPnl
            // 
            this.lblChartPnl.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChartPnl.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblChartPnl.Location = new System.Drawing.Point(0, 0);
            this.lblChartPnl.Name = "lblChartPnl";
            this.lblChartPnl.Size = new System.Drawing.Size(474, 24);
            this.lblChartPnl.TabIndex = 0;
            this.lblChartPnl.Text = "Monthly P&&L Comparison (Last 6 Months)";
            this.lblChartPnl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelChartCash
            // 
            this.panelChartCash.Controls.Add(this.chartCashFlow);
            this.panelChartCash.Controls.Add(this.lblChartCash);
            this.panelChartCash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChartCash.Location = new System.Drawing.Point(489, 3);
            this.panelChartCash.Name = "panelChartCash";
            this.panelChartCash.Padding = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.panelChartCash.Size = new System.Drawing.Size(480, 200);
            this.panelChartCash.TabIndex = 1;
            // 
            // chartCashFlow
            // 
            chartArea2.Name = "ChartArea1";
            this.chartCashFlow.ChartAreas.Add(chartArea2);
            this.chartCashFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chartCashFlow.Legends.Add(legend2);
            this.chartCashFlow.Location = new System.Drawing.Point(0, 24);
            this.chartCashFlow.Name = "chartCashFlow";
            this.chartCashFlow.Size = new System.Drawing.Size(474, 176);
            this.chartCashFlow.TabIndex = 1;
            this.chartCashFlow.Text = "chart2";
            // 
            // lblChartCash
            // 
            this.lblChartCash.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChartCash.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblChartCash.Location = new System.Drawing.Point(0, 0);
            this.lblChartCash.Name = "lblChartCash";
            this.lblChartCash.Size = new System.Drawing.Size(474, 24);
            this.lblChartCash.TabIndex = 0;
            this.lblChartCash.Text = "Cash && Bank Trend (Last 12 Months)";
            this.lblChartCash.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelChartExpense
            // 
            this.panelChartExpense.Controls.Add(this.chartExpense);
            this.panelChartExpense.Controls.Add(this.lblChartExpense);
            this.panelChartExpense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChartExpense.Location = new System.Drawing.Point(975, 3);
            this.panelChartExpense.Name = "panelChartExpense";
            this.panelChartExpense.Size = new System.Drawing.Size(482, 200);
            this.panelChartExpense.TabIndex = 2;
            // 
            // chartExpense
            // 
            chartArea3.Name = "ChartArea1";
            this.chartExpense.ChartAreas.Add(chartArea3);
            this.chartExpense.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.Name = "Legend1";
            this.chartExpense.Legends.Add(legend3);
            this.chartExpense.Location = new System.Drawing.Point(0, 24);
            this.chartExpense.Name = "chartExpense";
            this.chartExpense.Size = new System.Drawing.Size(482, 176);
            this.chartExpense.TabIndex = 1;
            this.chartExpense.Text = "chart3";
            // 
            // lblChartExpense
            // 
            this.lblChartExpense.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChartExpense.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblChartExpense.Location = new System.Drawing.Point(0, 0);
            this.lblChartExpense.Name = "lblChartExpense";
            this.lblChartExpense.Size = new System.Drawing.Size(482, 24);
            this.lblChartExpense.TabIndex = 0;
            this.lblChartExpense.Text = "Expense Breakdown (Top 5)";
            this.lblChartExpense.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelKpis
            // 
            this.panelKpis.Controls.Add(this.tableKpis);
            this.panelKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelKpis.Location = new System.Drawing.Point(12, 12);
            this.panelKpis.Name = "panelKpis";
            this.panelKpis.Size = new System.Drawing.Size(1460, 220);
            this.panelKpis.TabIndex = 0;
            // 
            // tableKpis
            // 
            this.tableKpis.ColumnCount = 3;
            this.tableKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableKpis.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableKpis.Controls.Add(this.cardCash, 0, 0);
            this.tableKpis.Controls.Add(this.cardReceivable, 1, 0);
            this.tableKpis.Controls.Add(this.cardPayable, 2, 0);
            this.tableKpis.Controls.Add(this.cardRevenue, 0, 1);
            this.tableKpis.Controls.Add(this.cardExpenses, 1, 1);
            this.tableKpis.Controls.Add(this.cardNetProfit, 2, 1);
            this.tableKpis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableKpis.Location = new System.Drawing.Point(0, 0);
            this.tableKpis.Name = "tableKpis";
            this.tableKpis.RowCount = 2;
            this.tableKpis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableKpis.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableKpis.Size = new System.Drawing.Size(1460, 220);
            this.tableKpis.TabIndex = 0;
            // 
            // cardCash
            // 
            this.cardCash.Controls.Add(this.lblCashValue);
            this.cardCash.Controls.Add(this.lblCashTitle);
            this.cardCash.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardCash.Location = new System.Drawing.Point(3, 3);
            this.cardCash.Name = "cardCash";
            this.cardCash.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.cardCash.Size = new System.Drawing.Size(480, 104);
            this.cardCash.TabIndex = 0;
            this.cardCash.Click += new System.EventHandler(this.cardCash_Click);
            // 
            // lblCashValue
            // 
            this.lblCashValue.AutoSize = true;
            this.lblCashValue.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblCashValue.Location = new System.Drawing.Point(17, 46);
            this.lblCashValue.Name = "lblCashValue";
            this.lblCashValue.Size = new System.Drawing.Size(69, 37);
            this.lblCashValue.TabIndex = 1;
            this.lblCashValue.Text = "0.00";
            // 
            // lblCashTitle
            // 
            this.lblCashTitle.AutoSize = true;
            this.lblCashTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblCashTitle.Location = new System.Drawing.Point(17, 15);
            this.lblCashTitle.Name = "lblCashTitle";
            this.lblCashTitle.Size = new System.Drawing.Size(171, 23);
            this.lblCashTitle.TabIndex = 0;
            this.lblCashTitle.Text = "Cash && Bank Balance";
            // 
            // cardReceivable
            // 
            this.cardReceivable.Controls.Add(this.lblReceivableValue);
            this.cardReceivable.Controls.Add(this.lblReceivableTitle);
            this.cardReceivable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardReceivable.Location = new System.Drawing.Point(489, 3);
            this.cardReceivable.Name = "cardReceivable";
            this.cardReceivable.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.cardReceivable.Size = new System.Drawing.Size(480, 104);
            this.cardReceivable.TabIndex = 1;
            this.cardReceivable.Click += new System.EventHandler(this.cardReceivable_Click);
            // 
            // lblReceivableValue
            // 
            this.lblReceivableValue.AutoSize = true;
            this.lblReceivableValue.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblReceivableValue.Location = new System.Drawing.Point(17, 46);
            this.lblReceivableValue.Name = "lblReceivableValue";
            this.lblReceivableValue.Size = new System.Drawing.Size(69, 37);
            this.lblReceivableValue.TabIndex = 1;
            this.lblReceivableValue.Text = "0.00";
            // 
            // lblReceivableTitle
            // 
            this.lblReceivableTitle.AutoSize = true;
            this.lblReceivableTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblReceivableTitle.Location = new System.Drawing.Point(17, 15);
            this.lblReceivableTitle.Name = "lblReceivableTitle";
            this.lblReceivableTitle.Size = new System.Drawing.Size(140, 23);
            this.lblReceivableTitle.TabIndex = 0;
            this.lblReceivableTitle.Text = "Total Receivables";
            // 
            // cardPayable
            // 
            this.cardPayable.Controls.Add(this.lblPayableValue);
            this.cardPayable.Controls.Add(this.lblPayableTitle);
            this.cardPayable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardPayable.Location = new System.Drawing.Point(975, 3);
            this.cardPayable.Name = "cardPayable";
            this.cardPayable.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.cardPayable.Size = new System.Drawing.Size(482, 104);
            this.cardPayable.TabIndex = 2;
            this.cardPayable.Click += new System.EventHandler(this.cardPayable_Click);
            // 
            // lblPayableValue
            // 
            this.lblPayableValue.AutoSize = true;
            this.lblPayableValue.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblPayableValue.Location = new System.Drawing.Point(17, 46);
            this.lblPayableValue.Name = "lblPayableValue";
            this.lblPayableValue.Size = new System.Drawing.Size(69, 37);
            this.lblPayableValue.TabIndex = 1;
            this.lblPayableValue.Text = "0.00";
            // 
            // lblPayableTitle
            // 
            this.lblPayableTitle.AutoSize = true;
            this.lblPayableTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblPayableTitle.Location = new System.Drawing.Point(17, 15);
            this.lblPayableTitle.Name = "lblPayableTitle";
            this.lblPayableTitle.Size = new System.Drawing.Size(118, 23);
            this.lblPayableTitle.TabIndex = 0;
            this.lblPayableTitle.Text = "Total Payables";
            // 
            // cardRevenue
            // 
            this.cardRevenue.Controls.Add(this.lblRevenueValue);
            this.cardRevenue.Controls.Add(this.lblRevenueTitle);
            this.cardRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardRevenue.Location = new System.Drawing.Point(3, 113);
            this.cardRevenue.Name = "cardRevenue";
            this.cardRevenue.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.cardRevenue.Size = new System.Drawing.Size(480, 104);
            this.cardRevenue.TabIndex = 3;
            this.cardRevenue.Click += new System.EventHandler(this.cardRevenue_Click);
            // 
            // lblRevenueValue
            // 
            this.lblRevenueValue.AutoSize = true;
            this.lblRevenueValue.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblRevenueValue.Location = new System.Drawing.Point(17, 46);
            this.lblRevenueValue.Name = "lblRevenueValue";
            this.lblRevenueValue.Size = new System.Drawing.Size(69, 37);
            this.lblRevenueValue.TabIndex = 1;
            this.lblRevenueValue.Text = "0.00";
            // 
            // lblRevenueTitle
            // 
            this.lblRevenueTitle.AutoSize = true;
            this.lblRevenueTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblRevenueTitle.Location = new System.Drawing.Point(17, 15);
            this.lblRevenueTitle.Name = "lblRevenueTitle";
            this.lblRevenueTitle.Size = new System.Drawing.Size(169, 23);
            this.lblRevenueTitle.TabIndex = 0;
            this.lblRevenueTitle.Text = "Revenue This Month";
            // 
            // cardExpenses
            // 
            this.cardExpenses.Controls.Add(this.lblExpensesValue);
            this.cardExpenses.Controls.Add(this.lblExpensesTitle);
            this.cardExpenses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardExpenses.Location = new System.Drawing.Point(489, 113);
            this.cardExpenses.Name = "cardExpenses";
            this.cardExpenses.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.cardExpenses.Size = new System.Drawing.Size(480, 104);
            this.cardExpenses.TabIndex = 4;
            this.cardExpenses.Click += new System.EventHandler(this.cardRevenue_Click);
            // 
            // lblExpensesValue
            // 
            this.lblExpensesValue.AutoSize = true;
            this.lblExpensesValue.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblExpensesValue.Location = new System.Drawing.Point(17, 46);
            this.lblExpensesValue.Name = "lblExpensesValue";
            this.lblExpensesValue.Size = new System.Drawing.Size(69, 37);
            this.lblExpensesValue.TabIndex = 1;
            this.lblExpensesValue.Text = "0.00";
            // 
            // lblExpensesTitle
            // 
            this.lblExpensesTitle.AutoSize = true;
            this.lblExpensesTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblExpensesTitle.Location = new System.Drawing.Point(17, 15);
            this.lblExpensesTitle.Name = "lblExpensesTitle";
            this.lblExpensesTitle.Size = new System.Drawing.Size(172, 23);
            this.lblExpensesTitle.TabIndex = 0;
            this.lblExpensesTitle.Text = "Expenses This Month";
            // 
            // cardNetProfit
            // 
            this.cardNetProfit.Controls.Add(this.lblNetProfitValue);
            this.cardNetProfit.Controls.Add(this.lblNetProfitTitle);
            this.cardNetProfit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cardNetProfit.Location = new System.Drawing.Point(975, 113);
            this.cardNetProfit.Name = "cardNetProfit";
            this.cardNetProfit.Padding = new System.Windows.Forms.Padding(14, 10, 14, 10);
            this.cardNetProfit.Size = new System.Drawing.Size(482, 104);
            this.cardNetProfit.TabIndex = 5;
            // 
            // lblNetProfitValue
            // 
            this.lblNetProfitValue.AutoSize = true;
            this.lblNetProfitValue.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblNetProfitValue.Location = new System.Drawing.Point(17, 46);
            this.lblNetProfitValue.Name = "lblNetProfitValue";
            this.lblNetProfitValue.Size = new System.Drawing.Size(69, 37);
            this.lblNetProfitValue.TabIndex = 1;
            this.lblNetProfitValue.Text = "0.00";
            // 
            // lblNetProfitTitle
            // 
            this.lblNetProfitTitle.AutoSize = true;
            this.lblNetProfitTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblNetProfitTitle.Location = new System.Drawing.Point(17, 15);
            this.lblNetProfitTitle.Name = "lblNetProfitTitle";
            this.lblNetProfitTitle.Size = new System.Drawing.Size(177, 23);
            this.lblNetProfitTitle.TabIndex = 0;
            this.lblNetProfitTitle.Text = "Net Profit This Month";
            // 
            // panelBottomActions
            // 
            this.panelBottomActions.Controls.Add(this.flowQuickActions);
            this.panelBottomActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottomActions.Location = new System.Drawing.Point(12, 641);
            this.panelBottomActions.Name = "panelBottomActions";
            this.panelBottomActions.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.panelBottomActions.Size = new System.Drawing.Size(1460, 52);
            this.panelBottomActions.TabIndex = 3;
            // 
            // flowQuickActions
            // 
            this.flowQuickActions.Controls.Add(this.btnNewJv);
            this.flowQuickActions.Controls.Add(this.btnReceivePayment);
            this.flowQuickActions.Controls.Add(this.btnMakePayment);
            this.flowQuickActions.Controls.Add(this.btnBankRec);
            this.flowQuickActions.Controls.Add(this.btnRunPL);
            this.flowQuickActions.Controls.Add(this.btnRunBalanceSheet);
            this.flowQuickActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowQuickActions.Location = new System.Drawing.Point(0, 8);
            this.flowQuickActions.Name = "flowQuickActions";
            this.flowQuickActions.Size = new System.Drawing.Size(1460, 44);
            this.flowQuickActions.TabIndex = 0;
            // 
            // btnNewJv
            // 
            this.btnNewJv.Location = new System.Drawing.Point(3, 3);
            this.btnNewJv.Name = "btnNewJv";
            this.btnNewJv.Size = new System.Drawing.Size(140, 34);
            this.btnNewJv.TabIndex = 0;
            this.btnNewJv.Text = "New JV";
            this.btnNewJv.UseVisualStyleBackColor = true;
            this.btnNewJv.Click += new System.EventHandler(this.btnNewJv_Click);
            // 
            // btnReceivePayment
            // 
            this.btnReceivePayment.Location = new System.Drawing.Point(149, 3);
            this.btnReceivePayment.Name = "btnReceivePayment";
            this.btnReceivePayment.Size = new System.Drawing.Size(140, 34);
            this.btnReceivePayment.TabIndex = 1;
            this.btnReceivePayment.Text = "Receive Payment";
            this.btnReceivePayment.UseVisualStyleBackColor = true;
            this.btnReceivePayment.Click += new System.EventHandler(this.btnReceivePayment_Click);
            // 
            // btnMakePayment
            // 
            this.btnMakePayment.Location = new System.Drawing.Point(295, 3);
            this.btnMakePayment.Name = "btnMakePayment";
            this.btnMakePayment.Size = new System.Drawing.Size(140, 34);
            this.btnMakePayment.TabIndex = 2;
            this.btnMakePayment.Text = "Make Payment";
            this.btnMakePayment.UseVisualStyleBackColor = true;
            this.btnMakePayment.Click += new System.EventHandler(this.btnMakePayment_Click);
            // 
            // btnBankRec
            // 
            this.btnBankRec.Location = new System.Drawing.Point(441, 3);
            this.btnBankRec.Name = "btnBankRec";
            this.btnBankRec.Size = new System.Drawing.Size(140, 34);
            this.btnBankRec.TabIndex = 3;
            this.btnBankRec.Text = "Bank Rec";
            this.btnBankRec.UseVisualStyleBackColor = true;
            this.btnBankRec.Click += new System.EventHandler(this.btnBankRec_Click);
            // 
            // btnRunPL
            // 
            this.btnRunPL.Location = new System.Drawing.Point(587, 3);
            this.btnRunPL.Name = "btnRunPL";
            this.btnRunPL.Size = new System.Drawing.Size(140, 34);
            this.btnRunPL.TabIndex = 4;
            this.btnRunPL.Text = "Run P&&L";
            this.btnRunPL.UseVisualStyleBackColor = true;
            this.btnRunPL.Click += new System.EventHandler(this.btnRunPL_Click);
            // 
            // btnRunBalanceSheet
            // 
            this.btnRunBalanceSheet.Location = new System.Drawing.Point(733, 3);
            this.btnRunBalanceSheet.Name = "btnRunBalanceSheet";
            this.btnRunBalanceSheet.Size = new System.Drawing.Size(160, 34);
            this.btnRunBalanceSheet.TabIndex = 5;
            this.btnRunBalanceSheet.Text = "Run Balance Sheet";
            this.btnRunBalanceSheet.UseVisualStyleBackColor = true;
            this.btnRunBalanceSheet.Click += new System.EventHandler(this.btnRunBalanceSheet_Click);
            // 
            // frm_accounting_dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 761);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.Name = "frm_accounting_dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Accounting Dashboard";
            this.Load += new System.EventHandler(this.frm_accounting_dashboard_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMiddle.ResumeLayout(false);
            this.splitMiddle.Panel1.ResumeLayout(false);
            this.splitMiddle.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMiddle)).EndInit();
            this.splitMiddle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridUnreconciledBanks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttention)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridRecentJournals)).EndInit();
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
