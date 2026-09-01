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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_expense_dashboard));
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
            resources.ApplyResources(this.panelFilters, "panelFilters");
            this.panelFilters.Controls.Add(this.btnRefresh);
            this.panelFilters.Controls.Add(this.dtpTo);
            this.panelFilters.Controls.Add(this.lblTo);
            this.panelFilters.Controls.Add(this.dtpFrom);
            this.panelFilters.Controls.Add(this.lblFrom);
            this.panelFilters.Controls.Add(this.cmbPeriod);
            this.panelFilters.Controls.Add(this.lblPeriod);
            this.panelFilters.Name = "panelFilters";
            // 
            // btnRefresh
            // 
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dtpTo
            // 
            resources.ApplyResources(this.dtpTo, "dtpTo");
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Name = "dtpTo";
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
            this.panelKpis.Controls.Add(this.cardPending);
            this.panelKpis.Controls.Add(this.cardYear);
            this.panelKpis.Controls.Add(this.cardMonth);
            this.panelKpis.Controls.Add(this.cardToday);
            this.panelKpis.Name = "panelKpis";
            // 
            // cardPending
            // 
            resources.ApplyResources(this.cardPending, "cardPending");
            this.cardPending.Controls.Add(this.lblPendingTrend);
            this.cardPending.Controls.Add(this.lblPendingAmount);
            this.cardPending.Controls.Add(this.lblPendingTitle);
            this.cardPending.Controls.Add(this.picPending);
            this.cardPending.Name = "cardPending";
            // 
            // lblPendingTrend
            // 
            resources.ApplyResources(this.lblPendingTrend, "lblPendingTrend");
            this.lblPendingTrend.Name = "lblPendingTrend";
            // 
            // lblPendingAmount
            // 
            resources.ApplyResources(this.lblPendingAmount, "lblPendingAmount");
            this.lblPendingAmount.Name = "lblPendingAmount";
            // 
            // lblPendingTitle
            // 
            resources.ApplyResources(this.lblPendingTitle, "lblPendingTitle");
            this.lblPendingTitle.Name = "lblPendingTitle";
            // 
            // picPending
            // 
            resources.ApplyResources(this.picPending, "picPending");
            this.picPending.BackColor = System.Drawing.Color.Tomato;
            this.picPending.Name = "picPending";
            this.picPending.TabStop = false;
            // 
            // cardYear
            // 
            resources.ApplyResources(this.cardYear, "cardYear");
            this.cardYear.Controls.Add(this.lblYearTrend);
            this.cardYear.Controls.Add(this.lblYearAmount);
            this.cardYear.Controls.Add(this.lblYearTitle);
            this.cardYear.Controls.Add(this.picYear);
            this.cardYear.Name = "cardYear";
            // 
            // lblYearTrend
            // 
            resources.ApplyResources(this.lblYearTrend, "lblYearTrend");
            this.lblYearTrend.Name = "lblYearTrend";
            // 
            // lblYearAmount
            // 
            resources.ApplyResources(this.lblYearAmount, "lblYearAmount");
            this.lblYearAmount.Name = "lblYearAmount";
            // 
            // lblYearTitle
            // 
            resources.ApplyResources(this.lblYearTitle, "lblYearTitle");
            this.lblYearTitle.Name = "lblYearTitle";
            // 
            // picYear
            // 
            resources.ApplyResources(this.picYear, "picYear");
            this.picYear.BackColor = System.Drawing.Color.Gold;
            this.picYear.Name = "picYear";
            this.picYear.TabStop = false;
            // 
            // cardMonth
            // 
            resources.ApplyResources(this.cardMonth, "cardMonth");
            this.cardMonth.Controls.Add(this.lblMonthTrend);
            this.cardMonth.Controls.Add(this.lblMonthAmount);
            this.cardMonth.Controls.Add(this.lblMonthTitle);
            this.cardMonth.Controls.Add(this.picMonth);
            this.cardMonth.Name = "cardMonth";
            // 
            // lblMonthTrend
            // 
            resources.ApplyResources(this.lblMonthTrend, "lblMonthTrend");
            this.lblMonthTrend.Name = "lblMonthTrend";
            // 
            // lblMonthAmount
            // 
            resources.ApplyResources(this.lblMonthAmount, "lblMonthAmount");
            this.lblMonthAmount.Name = "lblMonthAmount";
            // 
            // lblMonthTitle
            // 
            resources.ApplyResources(this.lblMonthTitle, "lblMonthTitle");
            this.lblMonthTitle.Name = "lblMonthTitle";
            // 
            // picMonth
            // 
            resources.ApplyResources(this.picMonth, "picMonth");
            this.picMonth.BackColor = System.Drawing.Color.Teal;
            this.picMonth.Name = "picMonth";
            this.picMonth.TabStop = false;
            // 
            // cardToday
            // 
            resources.ApplyResources(this.cardToday, "cardToday");
            this.cardToday.Controls.Add(this.lblTodayTrend);
            this.cardToday.Controls.Add(this.lblTodayAmount);
            this.cardToday.Controls.Add(this.lblTodayTitle);
            this.cardToday.Controls.Add(this.picToday);
            this.cardToday.Name = "cardToday";
            // 
            // lblTodayTrend
            // 
            resources.ApplyResources(this.lblTodayTrend, "lblTodayTrend");
            this.lblTodayTrend.Name = "lblTodayTrend";
            // 
            // lblTodayAmount
            // 
            resources.ApplyResources(this.lblTodayAmount, "lblTodayAmount");
            this.lblTodayAmount.Name = "lblTodayAmount";
            // 
            // lblTodayTitle
            // 
            resources.ApplyResources(this.lblTodayTitle, "lblTodayTitle");
            this.lblTodayTitle.Name = "lblTodayTitle";
            // 
            // picToday
            // 
            resources.ApplyResources(this.picToday, "picToday");
            this.picToday.BackColor = System.Drawing.Color.DodgerBlue;
            this.picToday.Name = "picToday";
            this.picToday.TabStop = false;
            // 
            // panelContent
            // 
            resources.ApplyResources(this.panelContent, "panelContent");
            this.panelContent.Controls.Add(this.cardTopAccounts);
            this.panelContent.Controls.Add(this.cardRecent);
            this.panelContent.Controls.Add(this.cardPieChart);
            this.panelContent.Controls.Add(this.cardMonthlyChart);
            this.panelContent.Name = "panelContent";
            // 
            // cardTopAccounts
            // 
            resources.ApplyResources(this.cardTopAccounts, "cardTopAccounts");
            this.cardTopAccounts.Controls.Add(this.gridTopAccounts);
            this.cardTopAccounts.Controls.Add(this.lblTopAccounts);
            this.cardTopAccounts.Name = "cardTopAccounts";
            // 
            // gridTopAccounts
            // 
            resources.ApplyResources(this.gridTopAccounts, "gridTopAccounts");
            this.gridTopAccounts.Name = "gridTopAccounts";
            // 
            // lblTopAccounts
            // 
            resources.ApplyResources(this.lblTopAccounts, "lblTopAccounts");
            this.lblTopAccounts.Name = "lblTopAccounts";
            // 
            // cardRecent
            // 
            resources.ApplyResources(this.cardRecent, "cardRecent");
            this.cardRecent.Controls.Add(this.gridRecentTransactions);
            this.cardRecent.Controls.Add(this.lblRecent);
            this.cardRecent.Name = "cardRecent";
            // 
            // gridRecentTransactions
            // 
            resources.ApplyResources(this.gridRecentTransactions, "gridRecentTransactions");
            this.gridRecentTransactions.Name = "gridRecentTransactions";
            // 
            // lblRecent
            // 
            resources.ApplyResources(this.lblRecent, "lblRecent");
            this.lblRecent.Name = "lblRecent";
            // 
            // cardPieChart
            // 
            resources.ApplyResources(this.cardPieChart, "cardPieChart");
            this.cardPieChart.Controls.Add(this.chartBreakdown);
            this.cardPieChart.Controls.Add(this.lblBreakdown);
            this.cardPieChart.Name = "cardPieChart";
            // 
            // chartBreakdown
            // 
            resources.ApplyResources(this.chartBreakdown, "chartBreakdown");
            this.chartBreakdown.Name = "chartBreakdown";
            // 
            // lblBreakdown
            // 
            resources.ApplyResources(this.lblBreakdown, "lblBreakdown");
            this.lblBreakdown.Name = "lblBreakdown";
            // 
            // cardMonthlyChart
            // 
            resources.ApplyResources(this.cardMonthlyChart, "cardMonthlyChart");
            this.cardMonthlyChart.Controls.Add(this.chartMonthly);
            this.cardMonthlyChart.Controls.Add(this.lblMonthlyChart);
            this.cardMonthlyChart.Name = "cardMonthlyChart";
            // 
            // chartMonthly
            // 
            resources.ApplyResources(this.chartMonthly, "chartMonthly");
            this.chartMonthly.Name = "chartMonthly";
            // 
            // lblMonthlyChart
            // 
            resources.ApplyResources(this.lblMonthlyChart, "lblMonthlyChart");
            this.lblMonthlyChart.Name = "lblMonthlyChart";
            // 
            // frm_expense_dashboard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelKpis);
            this.Controls.Add(this.panelFilters);
            this.Name = "frm_expense_dashboard";
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
