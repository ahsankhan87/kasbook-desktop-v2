using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace pos.Dashboard
{
    public partial class frm_dashboard : Form
    {
        // External actions to wire from main
        public Action OpenNewSale { get; set; }
        public Action OpenProducts { get; set; }
        public Action OpenCustomers { get; set; }
        public Action OpenSuppliers { get; set; }
        public Action OpenSalesReport { get; set; }
        public Action OpenPurchasesReport { get; set; }
        public Action OpenSettings { get; set; }

        public frm_dashboard()
        {
            InitializeComponent();
        }

        private void frm_dashboard_Load(object sender, EventArgs e)
        {
            if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            // English-only UI (no localization, no RTL)
            ApplyEnglishTexts();

            // Actions
            btnNewSale.Click += (s, a) => OpenNewSale?.Invoke();
            btnProducts.Click += (s, a) => OpenProducts?.Invoke();
            btnCustomers.Click += (s, a) => OpenCustomers?.Invoke();
            btnSuppliers.Click += (s, a) => OpenSuppliers?.Invoke();
            btnSalesReport.Click += (s, a) => OpenSalesReport?.Invoke();
            btnPurchasesReport.Click += (s, a) => OpenPurchasesReport?.Invoke();
            btnSettings.Click += (s, a) => OpenSettings?.Invoke();

            // Load metrics from BLL
            LoadDashboardMetrics();
        }

        private void ApplyEnglishTexts()
        {
            this.Text = "Dashboard";
            lblTitle.Text = "Dashboard";

            lblSalesTitle.Text = "Today's Sales";     // amount
            lblRevenueTitle.Text = "Monthly Sales";   // amount
            lblLowStockTitle.Text = "Low Stock Items";

            lblQuickAccess.Text = "Quick Access";
            lblRecent.Text = "Recent Activity";

            btnNewSale.Text = "💳  New Sale";
            btnProducts.Text = "📦  Products";
            btnCustomers.Text = "👤  Customers";
            btnSuppliers.Text = "🏢  Suppliers";
            btnSalesReport.Text = "📊  Sales Report";
            btnPurchasesReport.Text = "🧾  Purchases Report";
            btnSettings.Text = "⚙️  Settings";

            colArea.Text = "Area";
            colInfo.Text = "Info";
            colDateTime.Text = "Date/Time";

            var branch = POS.Core.UsersModal.logged_in_branch_name;
            var fy = POS.Core.UsersModal.fiscal_year;
            lblSubtitle.Text = $"Branch: {branch} • Fiscal Year: {fy} • {DateTime.Now:dddd, MMM dd, yyyy}";
        }

        private void LoadDashboardMetrics()
        {
            try
            {
                var bll = new POS.BLL.Dashboard.DashboardBLL();
                var metrics = bll.GetDashboardMetrics(10);

                SetSalesTodayAmount(metrics.TodayAmount);
                SetMonthlySalesAmount(metrics.MonthlyAmount);
                SetLowStockCount(metrics.LowStockCount);
                SetRecentActivity(metrics.RecentLogs.Select(l => new RecentItem(l.Area, l.Description, l.Timestamp)));
            }
            catch
            {
                // Fallback defaults
                SetSalesTodayAmount(0m);
                SetMonthlySalesAmount(0m);
                SetLowStockCount(0);
                SetRecentActivity(Enumerable.Empty<RecentItem>());
            }
        }

        // Public API to feed data
        public void SetSalesTodayAmount(decimal amount) => lblSalesValue.Text = amount.ToString("N2");
        public void SetMonthlySalesAmount(decimal amount) => lblRevenueValue.Text = amount.ToString("N2");
        public void SetLowStockCount(int count) => lblLowStockValue.Text = count.ToString("N0");

        public void SetRecentActivity(IEnumerable<RecentItem> items)
        {
            listRecent.BeginUpdate();
            try
            {
                listRecent.Items.Clear();
                foreach (var i in items ?? Enumerable.Empty<RecentItem>())
                {
                    var lvi = new ListViewItem(i.Area ?? "-");
                    lvi.SubItems.Add(i.Description ?? "-");
                    lvi.SubItems.Add(i.Timestamp == DateTime.MinValue ? "-" : i.Timestamp.ToString("yyyy-MM-dd HH:mm"));
                    listRecent.Items.Add(lvi);
                }
                for (int i = 0; i < listRecent.Columns.Count; i++) listRecent.Columns[i].Width = -2;
            }
            finally { listRecent.EndUpdate(); }
        }

        public class RecentItem
        {
            public string Area { get; }
            public string Description { get; }
            public DateTime Timestamp { get; }
            public RecentItem(string area, string description, DateTime timestamp)
            { Area = area; Description = description; Timestamp = timestamp; }
        }
    }
}