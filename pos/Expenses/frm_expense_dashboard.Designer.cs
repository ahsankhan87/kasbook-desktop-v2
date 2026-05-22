namespace pos.Expenses
{
    partial class frm_expense_dashboard
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
            this.panelFilters = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.cmbPeriod = new System.Windows.Forms.ComboBox();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.panelKpis = new System.Windows.Forms.Panel();
            this.cardPending = new System.Windows.Forms.Panel();
            this.lblPendingTrend = new System.Windows.Forms.Label();
            this.lblPendingAmount = new System.Windows.Forms.Label();
            this.lblPendingTitle = new System.Windows.Forms.Label();
            this.picPending = new System.Windows.Forms.PictureBox();
            this.cardYear = new System.Windows.Forms.Panel();
            this.lblYearTrend = new System.Windows.Forms.Label();
            this.lblYearAmount = new System.Windows.Forms.Label();
            this.lblYearTitle = new System.Windows.Forms.Label();
            this.picYear = new System.Windows.Forms.PictureBox();
            this.cardMonth = new System.Windows.Forms.Panel();
            this.lblMonthTrend = new System.Windows.Forms.Label();
            this.lblMonthAmount = new System.Windows.Forms.Label();
            this.lblMonthTitle = new System.Windows.Forms.Label();
            this.picMonth = new System.Windows.Forms.PictureBox();
            this.cardToday = new System.Windows.Forms.Panel();
            this.lblTodayTrend = new System.Windows.Forms.Label();
            this.lblTodayAmount = new System.Windows.Forms.Label();
            this.lblTodayTitle = new System.Windows.Forms.Label();
            this.picToday = new System.Windows.Forms.PictureBox();
            this.panelContent = new System.Windows.Forms.Panel();
            this.cardTopAccounts = new System.Windows.Forms.Panel();
            this.gridTopAccounts = new System.Windows.Forms.DataGridView();
            this.lblTopAccounts = new System.Windows.Forms.Label();
            this.cardRecent = new System.Windows.Forms.Panel();
            this.gridRecentTransactions = new System.Windows.Forms.DataGridView();
            this.lblRecent = new System.Windows.Forms.Label();
            this.cardPieChart = new System.Windows.Forms.Panel();
            this.chartBreakdown = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblBreakdown = new System.Windows.Forms.Label();
            this.cardMonthlyChart = new System.Windows.Forms.Panel();
            this.chartMonthly = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblMonthlyChart = new System.Windows.Forms.Label();
            this.panelFilters.SuspendLayout();
            this.panelKpis.SuspendLayout();
            this.cardPending.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPending)).BeginInit();
            this.cardYear.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picYear)).BeginInit();
            this.cardMonth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMonth)).BeginInit();
            this.cardToday.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picToday)).BeginInit();
            this.panelContent.SuspendLayout();
            this.cardTopAccounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTopAccounts)).BeginInit();
            this.cardRecent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRecentTransactions)).BeginInit();
            this.cardPieChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartBreakdown)).BeginInit();
            this.cardMonthlyChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthly)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFilters
            // 
            this.panelFilters.Controls.Add(this.btnRefresh);
            this.panelFilters.Controls.Add(this.dtpTo);
            this.panelFilters.Controls.Add(this.lblTo);
            this.panelFilters.Controls.Add(this.dtpFrom);
            this.panelFilters.Controls.Add(this.lblFrom);
            this.panelFilters.Controls.Add(this.cmbPeriod);
            this.panelFilters.Controls.Add(this.lblPeriod);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(0, 0);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Padding = new System.Windows.Forms.Padding(18, 14, 18, 8);
            this.panelFilters.Size = new System.Drawing.Size(1380, 70);
            this.panelFilters.TabIndex = 0;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(608, 18);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(98, 32);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(487, 21);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(106, 26);
            this.dtpTo.TabIndex = 5;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(452, 18);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(29, 30);
            this.lblTo.TabIndex = 4;
            this.lblTo.Text = "To";
            this.lblTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(340, 21);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(106, 26);
            this.dtpFrom.TabIndex = 3;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(287, 18);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(47, 30);
            this.lblFrom.TabIndex = 2;
            this.lblFrom.Text = "From";
            this.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbPeriod
            // 
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.FormattingEnabled = true;
            this.cmbPeriod.Location = new System.Drawing.Point(90, 20);
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.Size = new System.Drawing.Size(191, 28);
            this.cmbPeriod.TabIndex = 1;
            this.cmbPeriod.SelectedIndexChanged += new System.EventHandler(this.cmbPeriod_SelectedIndexChanged);
            // 
            // lblPeriod
            // 
            this.lblPeriod.Location = new System.Drawing.Point(19, 18);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(65, 30);
            this.lblPeriod.TabIndex = 0;
            this.lblPeriod.Text = "Period";
            this.lblPeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelKpis
            // 
            this.panelKpis.Controls.Add(this.cardPending);
            this.panelKpis.Controls.Add(this.cardYear);
            this.panelKpis.Controls.Add(this.cardMonth);
            this.panelKpis.Controls.Add(this.cardToday);
            this.panelKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelKpis.Location = new System.Drawing.Point(0, 70);
            this.panelKpis.Name = "panelKpis";
            this.panelKpis.Padding = new System.Windows.Forms.Padding(18, 10, 18, 8);
            this.panelKpis.Size = new System.Drawing.Size(1380, 160);
            this.panelKpis.TabIndex = 1;
            // 
            // cardPending
            // 
            this.cardPending.Controls.Add(this.lblPendingTrend);
            this.cardPending.Controls.Add(this.lblPendingAmount);
            this.cardPending.Controls.Add(this.lblPendingTitle);
            this.cardPending.Controls.Add(this.picPending);
            this.cardPending.Location = new System.Drawing.Point(1050, 14);
            this.cardPending.Name = "cardPending";
            this.cardPending.Size = new System.Drawing.Size(300, 126);
            this.cardPending.TabIndex = 3;
            // 
            // lblPendingTrend
            // 
            this.lblPendingTrend.AutoSize = true;
            this.lblPendingTrend.Location = new System.Drawing.Point(78, 91);
            this.lblPendingTrend.Name = "lblPendingTrend";
            this.lblPendingTrend.Size = new System.Drawing.Size(49, 20);
            this.lblPendingTrend.TabIndex = 3;
            this.lblPendingTrend.Text = "Trend";
            // 
            // lblPendingAmount
            // 
            this.lblPendingAmount.AutoSize = true;
            this.lblPendingAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblPendingAmount.Location = new System.Drawing.Point(76, 47);
            this.lblPendingAmount.Name = "lblPendingAmount";
            this.lblPendingAmount.Size = new System.Drawing.Size(80, 32);
            this.lblPendingAmount.TabIndex = 2;
            this.lblPendingAmount.Text = "0.00";
            // 
            // lblPendingTitle
            // 
            this.lblPendingTitle.AutoSize = true;
            this.lblPendingTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblPendingTitle.Location = new System.Drawing.Point(78, 21);
            this.lblPendingTitle.Name = "lblPendingTitle";
            this.lblPendingTitle.Size = new System.Drawing.Size(126, 19);
            this.lblPendingTitle.TabIndex = 1;
            this.lblPendingTitle.Text = "Pending/Unposted";
            // 
            // picPending
            // 
            this.picPending.BackColor = System.Drawing.Color.Tomato;
            this.picPending.Location = new System.Drawing.Point(18, 24);
            this.picPending.Name = "picPending";
            this.picPending.Size = new System.Drawing.Size(42, 42);
            this.picPending.TabIndex = 0;
            this.picPending.TabStop = false;
            // 
            // cardYear
            // 
            this.cardYear.Controls.Add(this.lblYearTrend);
            this.cardYear.Controls.Add(this.lblYearAmount);
            this.cardYear.Controls.Add(this.lblYearTitle);
            this.cardYear.Controls.Add(this.picYear);
            this.cardYear.Location = new System.Drawing.Point(704, 14);
            this.cardYear.Name = "cardYear";
            this.cardYear.Size = new System.Drawing.Size(300, 126);
            this.cardYear.TabIndex = 2;
            // 
            // lblYearTrend
            // 
            this.lblYearTrend.AutoSize = true;
            this.lblYearTrend.Location = new System.Drawing.Point(78, 91);
            this.lblYearTrend.Name = "lblYearTrend";
            this.lblYearTrend.Size = new System.Drawing.Size(49, 20);
            this.lblYearTrend.TabIndex = 3;
            this.lblYearTrend.Text = "Trend";
            // 
            // lblYearAmount
            // 
            this.lblYearAmount.AutoSize = true;
            this.lblYearAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblYearAmount.Location = new System.Drawing.Point(76, 47);
            this.lblYearAmount.Name = "lblYearAmount";
            this.lblYearAmount.Size = new System.Drawing.Size(80, 32);
            this.lblYearAmount.TabIndex = 2;
            this.lblYearAmount.Text = "0.00";
            // 
            // lblYearTitle
            // 
            this.lblYearTitle.AutoSize = true;
            this.lblYearTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblYearTitle.Location = new System.Drawing.Point(78, 21);
            this.lblYearTitle.Name = "lblYearTitle";
            this.lblYearTitle.Size = new System.Drawing.Size(69, 19);
            this.lblYearTitle.TabIndex = 1;
            this.lblYearTitle.Text = "This Year";
            // 
            // picYear
            // 
            this.picYear.BackColor = System.Drawing.Color.Gold;
            this.picYear.Location = new System.Drawing.Point(18, 24);
            this.picYear.Name = "picYear";
            this.picYear.Size = new System.Drawing.Size(42, 42);
            this.picYear.TabIndex = 0;
            this.picYear.TabStop = false;
            // 
            // cardMonth
            // 
            this.cardMonth.Controls.Add(this.lblMonthTrend);
            this.cardMonth.Controls.Add(this.lblMonthAmount);
            this.cardMonth.Controls.Add(this.lblMonthTitle);
            this.cardMonth.Controls.Add(this.picMonth);
            this.cardMonth.Location = new System.Drawing.Point(362, 14);
            this.cardMonth.Name = "cardMonth";
            this.cardMonth.Size = new System.Drawing.Size(300, 126);
            this.cardMonth.TabIndex = 1;
            // 
            // lblMonthTrend
            // 
            this.lblMonthTrend.AutoSize = true;
            this.lblMonthTrend.Location = new System.Drawing.Point(78, 91);
            this.lblMonthTrend.Name = "lblMonthTrend";
            this.lblMonthTrend.Size = new System.Drawing.Size(49, 20);
            this.lblMonthTrend.TabIndex = 3;
            this.lblMonthTrend.Text = "Trend";
            // 
            // lblMonthAmount
            // 
            this.lblMonthAmount.AutoSize = true;
            this.lblMonthAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblMonthAmount.Location = new System.Drawing.Point(76, 47);
            this.lblMonthAmount.Name = "lblMonthAmount";
            this.lblMonthAmount.Size = new System.Drawing.Size(80, 32);
            this.lblMonthAmount.TabIndex = 2;
            this.lblMonthAmount.Text = "0.00";
            // 
            // lblMonthTitle
            // 
            this.lblMonthTitle.AutoSize = true;
            this.lblMonthTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblMonthTitle.Location = new System.Drawing.Point(78, 21);
            this.lblMonthTitle.Name = "lblMonthTitle";
            this.lblMonthTitle.Size = new System.Drawing.Size(82, 19);
            this.lblMonthTitle.TabIndex = 1;
            this.lblMonthTitle.Text = "This Month";
            // 
            // picMonth
            // 
            this.picMonth.BackColor = System.Drawing.Color.Teal;
            this.picMonth.Location = new System.Drawing.Point(18, 24);
            this.picMonth.Name = "picMonth";
            this.picMonth.Size = new System.Drawing.Size(42, 42);
            this.picMonth.TabIndex = 0;
            this.picMonth.TabStop = false;
            // 
            // cardToday
            // 
            this.cardToday.Controls.Add(this.lblTodayTrend);
            this.cardToday.Controls.Add(this.lblTodayAmount);
            this.cardToday.Controls.Add(this.lblTodayTitle);
            this.cardToday.Controls.Add(this.picToday);
            this.cardToday.Location = new System.Drawing.Point(18, 14);
            this.cardToday.Name = "cardToday";
            this.cardToday.Size = new System.Drawing.Size(300, 126);
            this.cardToday.TabIndex = 0;
            // 
            // lblTodayTrend
            // 
            this.lblTodayTrend.AutoSize = true;
            this.lblTodayTrend.Location = new System.Drawing.Point(78, 91);
            this.lblTodayTrend.Name = "lblTodayTrend";
            this.lblTodayTrend.Size = new System.Drawing.Size(49, 20);
            this.lblTodayTrend.TabIndex = 3;
            this.lblTodayTrend.Text = "Trend";
            // 
            // lblTodayAmount
            // 
            this.lblTodayAmount.AutoSize = true;
            this.lblTodayAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblTodayAmount.Location = new System.Drawing.Point(76, 47);
            this.lblTodayAmount.Name = "lblTodayAmount";
            this.lblTodayAmount.Size = new System.Drawing.Size(80, 32);
            this.lblTodayAmount.TabIndex = 2;
            this.lblTodayAmount.Text = "0.00";
            // 
            // lblTodayTitle
            // 
            this.lblTodayTitle.AutoSize = true;
            this.lblTodayTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblTodayTitle.Location = new System.Drawing.Point(78, 21);
            this.lblTodayTitle.Name = "lblTodayTitle";
            this.lblTodayTitle.Size = new System.Drawing.Size(118, 19);
            this.lblTodayTitle.TabIndex = 1;
            this.lblTodayTitle.Text = "Today's Expenses";
            // 
            // picToday
            // 
            this.picToday.BackColor = System.Drawing.Color.DodgerBlue;
            this.picToday.Location = new System.Drawing.Point(18, 24);
            this.picToday.Name = "picToday";
            this.picToday.Size = new System.Drawing.Size(42, 42);
            this.picToday.TabIndex = 0;
            this.picToday.TabStop = false;
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.cardTopAccounts);
            this.panelContent.Controls.Add(this.cardRecent);
            this.panelContent.Controls.Add(this.cardPieChart);
            this.panelContent.Controls.Add(this.cardMonthlyChart);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 230);
            this.panelContent.Name = "panelContent";
            this.panelContent.Padding = new System.Windows.Forms.Padding(18, 8, 18, 18);
            this.panelContent.Size = new System.Drawing.Size(1380, 590);
            this.panelContent.TabIndex = 2;
            // 
            // cardTopAccounts
            // 
            this.cardTopAccounts.Controls.Add(this.gridTopAccounts);
            this.cardTopAccounts.Controls.Add(this.lblTopAccounts);
            this.cardTopAccounts.Location = new System.Drawing.Point(700, 304);
            this.cardTopAccounts.Name = "cardTopAccounts";
            this.cardTopAccounts.Size = new System.Drawing.Size(650, 260);
            this.cardTopAccounts.TabIndex = 3;
            // 
            // gridTopAccounts
            // 
            this.gridTopAccounts.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridTopAccounts.Location = new System.Drawing.Point(0, 40);
            this.gridTopAccounts.Name = "gridTopAccounts";
            this.gridTopAccounts.Size = new System.Drawing.Size(650, 220);
            this.gridTopAccounts.TabIndex = 1;
            // 
            // lblTopAccounts
            // 
            this.lblTopAccounts.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTopAccounts.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblTopAccounts.Location = new System.Drawing.Point(0, 0);
            this.lblTopAccounts.Name = "lblTopAccounts";
            this.lblTopAccounts.Padding = new System.Windows.Forms.Padding(14, 10, 0, 0);
            this.lblTopAccounts.Size = new System.Drawing.Size(650, 40);
            this.lblTopAccounts.TabIndex = 0;
            this.lblTopAccounts.Text = "Top Expense Accounts";
            // 
            // cardRecent
            // 
            this.cardRecent.Controls.Add(this.gridRecentTransactions);
            this.cardRecent.Controls.Add(this.lblRecent);
            this.cardRecent.Location = new System.Drawing.Point(18, 304);
            this.cardRecent.Name = "cardRecent";
            this.cardRecent.Size = new System.Drawing.Size(650, 260);
            this.cardRecent.TabIndex = 2;
            // 
            // gridRecentTransactions
            // 
            this.gridRecentTransactions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridRecentTransactions.Location = new System.Drawing.Point(0, 40);
            this.gridRecentTransactions.Name = "gridRecentTransactions";
            this.gridRecentTransactions.Size = new System.Drawing.Size(650, 220);
            this.gridRecentTransactions.TabIndex = 1;
            // 
            // lblRecent
            // 
            this.lblRecent.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRecent.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblRecent.Location = new System.Drawing.Point(0, 0);
            this.lblRecent.Name = "lblRecent";
            this.lblRecent.Padding = new System.Windows.Forms.Padding(14, 10, 0, 0);
            this.lblRecent.Size = new System.Drawing.Size(650, 40);
            this.lblRecent.TabIndex = 0;
            this.lblRecent.Text = "Recent Transactions";
            // 
            // cardPieChart
            // 
            this.cardPieChart.Controls.Add(this.chartBreakdown);
            this.cardPieChart.Controls.Add(this.lblBreakdown);
            this.cardPieChart.Location = new System.Drawing.Point(700, 14);
            this.cardPieChart.Name = "cardPieChart";
            this.cardPieChart.Size = new System.Drawing.Size(650, 270);
            this.cardPieChart.TabIndex = 1;
            // 
            // chartBreakdown
            // 
            this.chartBreakdown.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chartBreakdown.Location = new System.Drawing.Point(0, 40);
            this.chartBreakdown.Name = "chartBreakdown";
            this.chartBreakdown.Size = new System.Drawing.Size(650, 230);
            this.chartBreakdown.TabIndex = 1;
            // 
            // lblBreakdown
            // 
            this.lblBreakdown.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBreakdown.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblBreakdown.Location = new System.Drawing.Point(0, 0);
            this.lblBreakdown.Name = "lblBreakdown";
            this.lblBreakdown.Padding = new System.Windows.Forms.Padding(14, 10, 0, 0);
            this.lblBreakdown.Size = new System.Drawing.Size(650, 40);
            this.lblBreakdown.TabIndex = 0;
            this.lblBreakdown.Text = "Expense Breakdown by Account";
            // 
            // cardMonthlyChart
            // 
            this.cardMonthlyChart.Controls.Add(this.chartMonthly);
            this.cardMonthlyChart.Controls.Add(this.lblMonthlyChart);
            this.cardMonthlyChart.Location = new System.Drawing.Point(18, 14);
            this.cardMonthlyChart.Name = "cardMonthlyChart";
            this.cardMonthlyChart.Size = new System.Drawing.Size(650, 270);
            this.cardMonthlyChart.TabIndex = 0;
            // 
            // chartMonthly
            // 
            this.chartMonthly.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chartMonthly.Location = new System.Drawing.Point(0, 40);
            this.chartMonthly.Name = "chartMonthly";
            this.chartMonthly.Size = new System.Drawing.Size(650, 230);
            this.chartMonthly.TabIndex = 1;
            // 
            // lblMonthlyChart
            // 
            this.lblMonthlyChart.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMonthlyChart.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblMonthlyChart.Location = new System.Drawing.Point(0, 0);
            this.lblMonthlyChart.Name = "lblMonthlyChart";
            this.lblMonthlyChart.Padding = new System.Windows.Forms.Padding(14, 10, 0, 0);
            this.lblMonthlyChart.Size = new System.Drawing.Size(650, 40);
            this.lblMonthlyChart.TabIndex = 0;
            this.lblMonthlyChart.Text = "Monthly Expense Comparison";
            // 
            // frm_expense_dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1380, 820);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelKpis);
            this.Controls.Add(this.panelFilters);
            this.Name = "frm_expense_dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Expense Dashboard";
            this.Load += new System.EventHandler(this.frm_expense_dashboard_Load);
            this.panelFilters.ResumeLayout(false);
            this.panelKpis.ResumeLayout(false);
            this.cardPending.ResumeLayout(false);
            this.cardPending.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPending)).EndInit();
            this.cardYear.ResumeLayout(false);
            this.cardYear.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picYear)).EndInit();
            this.cardMonth.ResumeLayout(false);
            this.cardMonth.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMonth)).EndInit();
            this.cardToday.ResumeLayout(false);
            this.cardToday.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picToday)).EndInit();
            this.panelContent.ResumeLayout(false);
            this.cardTopAccounts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTopAccounts)).EndInit();
            this.cardRecent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRecentTransactions)).EndInit();
            this.cardPieChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartBreakdown)).EndInit();
            this.cardMonthlyChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMonthly)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.ComboBox cmbPeriod;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.Panel panelKpis;
        private System.Windows.Forms.Panel cardPending;
        private System.Windows.Forms.Label lblPendingTrend;
        private System.Windows.Forms.Label lblPendingAmount;
        private System.Windows.Forms.Label lblPendingTitle;
        private System.Windows.Forms.PictureBox picPending;
        private System.Windows.Forms.Panel cardYear;
        private System.Windows.Forms.Label lblYearTrend;
        private System.Windows.Forms.Label lblYearAmount;
        private System.Windows.Forms.Label lblYearTitle;
        private System.Windows.Forms.PictureBox picYear;
        private System.Windows.Forms.Panel cardMonth;
        private System.Windows.Forms.Label lblMonthTrend;
        private System.Windows.Forms.Label lblMonthAmount;
        private System.Windows.Forms.Label lblMonthTitle;
        private System.Windows.Forms.PictureBox picMonth;
        private System.Windows.Forms.Panel cardToday;
        private System.Windows.Forms.Label lblTodayTrend;
        private System.Windows.Forms.Label lblTodayAmount;
        private System.Windows.Forms.Label lblTodayTitle;
        private System.Windows.Forms.PictureBox picToday;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel cardTopAccounts;
        private System.Windows.Forms.DataGridView gridTopAccounts;
        private System.Windows.Forms.Label lblTopAccounts;
        private System.Windows.Forms.Panel cardRecent;
        private System.Windows.Forms.DataGridView gridRecentTransactions;
        private System.Windows.Forms.Label lblRecent;
        private System.Windows.Forms.Panel cardPieChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartBreakdown;
        private System.Windows.Forms.Label lblBreakdown;
        private System.Windows.Forms.Panel cardMonthlyChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMonthly;
        private System.Windows.Forms.Label lblMonthlyChart;
    }
}
