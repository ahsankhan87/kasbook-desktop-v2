using pos.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            var company = POS.Core.UsersModal.logged_in_company_name;
            var branch = POS.Core.UsersModal.logged_in_branch_name;
            var fy = POS.Core.UsersModal.fiscal_year;
            lblSubtitle.Text = $"Company: {company} • Branch: {branch} • Fiscal Year: {fy} • {DateTime.Now:dddd, MMM dd, yyyy}";
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

        Form ZatcaInvoices;
        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (ZatcaInvoices == null)
            {
                ZatcaInvoices = new pos.Sales.frm_zatca_invoices()
                {
                    MdiParent = frm_main.ActiveForm,
                };
                ZatcaInvoices.FormClosed += new FormClosedEventHandler(ZatcaInvoices_FormClosed);
                ZatcaInvoices.Show();
            }
            else
            {
                ZatcaInvoices.Activate();
            }
        }

        private void ZatcaInvoices_FormClosed(object sender, FormClosedEventArgs e)
        {
            ZatcaInvoices = null;
        }

        Form frm_sales_obj;
        private void btnNewSale_Click(object sender, EventArgs e)
        {
            frm_sales_obj = new frm_sales();
            frm_sales_obj.MdiParent = frm_main.ActiveForm;

            //frm_sales_obj.Dock = DockStyle.Fill;
            //frm_sales_obj.FormClosed += new FormClosedEventHandler(frm_sales_obj_FormClosed);
            frm_sales_obj.WindowState = FormWindowState.Maximized;
            frm_sales_obj.Show();
        }

        Form frm_products;
        private void btnProducts_Click(object sender, EventArgs e)
        {
            if (frm_products == null)
            {
                frm_products = new frm_product_full_detail();
                frm_products.MdiParent = frm_main.ActiveForm;
                //frm_cust.Dock = DockStyle.Fill;
                frm_products.FormClosed += new FormClosedEventHandler(Frm_products_FormClosed);
                frm_products.Show();
            }
            else
            {
                frm_products.Activate();
            }
        }

        private void Frm_products_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_products = null;
        }

        Form frmCustomers;
        private void btnCustomers_Click(object sender, EventArgs e)
        {
            if (frmCustomers == null)
            {
                frmCustomers = new frm_customers();
                frmCustomers.MdiParent = frm_main.ActiveForm;
                //frm_cust.Dock = DockStyle.Fill;
                frmCustomers.FormClosed += new FormClosedEventHandler(FrmCustomers_FormClosed);
                frmCustomers.Show();
            }
            else
            {
                frmCustomers.Activate();
            }

        }
        private void FrmCustomers_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmCustomers = null;
        }

        Form frmSuppliers;
        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            if (frmSuppliers == null)
            {
                frmSuppliers = new frm_suppliers();
                frmSuppliers.MdiParent = frm_main.ActiveForm;
                //frm_cust.Dock = DockStyle.Fill;
                frmSuppliers.FormClosed += new FormClosedEventHandler(FrmSuppliers_FormClosed);
                frmSuppliers.Show();
            }
            else
            {
                frmSuppliers.Activate();
            }
        }
        private void FrmSuppliers_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmSuppliers = null;
        }

        Form frmSalesReport;
        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            if (frmSalesReport == null)
            {
                frmSalesReport = new frm_SalesReport();
                frmSalesReport.MdiParent = frm_main.ActiveForm;
                //frm_cust.Dock = DockStyle.Fill;
                frmSalesReport.FormClosed += new FormClosedEventHandler(FrmSalesReport_FormClosed);
                frmSalesReport.Show();
            }
            else
            {
                frmSalesReport.Activate();
            }
        }
        private void FrmSalesReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmSalesReport = null;
        }

        Form frmPurchasesReport;
        private void btnPurchasesReport_Click(object sender, EventArgs e)
        {
            if (frmPurchasesReport == null)
            {
                frmPurchasesReport = new frm_PurchasesReport();
                frmPurchasesReport.MdiParent = frm_main.ActiveForm;
                //frm_cust.Dock = DockStyle.Fill;
                frmPurchasesReport.FormClosed += new FormClosedEventHandler(FrmPurchasesReport_FormClosed);
                frmPurchasesReport.Show();
            }
            else
            {
                frmPurchasesReport.Activate();
            }
        }
        private void FrmPurchasesReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmPurchasesReport = null;
        }

        Form LowStockReport;
        private void lblLowStockValue_Click(object sender, EventArgs e)
        {
            if (LowStockReport == null)
            {
                LowStockReport = new pos.Reports.Products.Inventory.FrmLowStockReport
                {
                    MdiParent = frm_main.ActiveForm
                };
                LowStockReport.FormClosed += new FormClosedEventHandler(LowStockReport_FormClosed);
                LowStockReport.Show();
            }
            else
            {
                LowStockReport.Activate();
            }
        }
        private void LowStockReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            LowStockReport = null;
        }
    }
}