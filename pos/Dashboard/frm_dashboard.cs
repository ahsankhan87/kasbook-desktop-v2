using com.sun.org.apache.xerces.@internal.impl.dtd.models;
using pos.Sales;
using pos.Security.Authorization;
using pos.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

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

        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        public frm_dashboard()
        {
            InitializeComponent();
        }

        private void frm_dashboard_Load(object sender, EventArgs e)
        {
            if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            // Apply professional theme
            //AppTheme.Apply(this);

            var company = POS.Core.UsersModal.logged_in_company_name;
            var branch = POS.Core.UsersModal.logged_in_branch_name;
            var fy = POS.Core.UsersModal.fiscal_year;

            // Do not override localized resources for Arabic.
            // WinForms will automatically use `frm_dashboard.ar-SA.resx` when CurrentUICulture is ar-SA.
            if (!string.Equals(System.Globalization.CultureInfo.CurrentUICulture.Name, "ar-SA", StringComparison.OrdinalIgnoreCase))
            {
                ApplyEnglishTexts();
                lblSubtitle.Text = $"Company: {company} • Branch: {branch} • Fiscal Year: {fy} • {DateTime.Now:dddd, MMM dd, yyyy}";

            }else
            {
                lblSubtitle.Text = $"الشركة: {company} • الفرع: {branch} • السنة المالية: {fy} • {DateTime.Now:dddd, MMM dd, yyyy}";
                //lblSubtitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            }   
           
            // Actions
            btnNewSale.Click += (s, a) => OpenNewSale?.Invoke();
            btnProducts.Click += (s, a) => OpenProducts?.Invoke();
            btnCustomers.Click += (s, a) => OpenCustomers?.Invoke();
            btnSuppliers.Click += (s, a) => OpenSuppliers?.Invoke();
            btnSalesReport.Click += (s, a) => OpenSalesReport?.Invoke();
            btnPurchasesReport.Click += (s, a) => OpenPurchasesReport?.Invoke();
            btnSettings.Click += (s, a) => OpenSettings?.Invoke();

            // Apply stunning visual theme
            StyleDashboard();

            // Load metrics from BLL
            LoadDashboardMetrics();
        }

        // ── Colour palette ───────────────────────────────────────────────────
        private static readonly Color _bg           = Color.FromArgb(245, 247, 250);  // page background
        private static readonly Color _header1      = Color.FromArgb(15,  76, 129);   // header gradient start
        private static readonly Color _header2      = Color.FromArgb(0,  120, 212);   // header gradient end
        private static readonly Color _cardBg       = Color.White;
        private static readonly Color _cardBorder   = Color.FromArgb(225, 230, 238);
        private static readonly Color _accent1      = Color.FromArgb(46,  204, 113);  // green  – today sales
        private static readonly Color _accent2      = Color.FromArgb(52,  152, 219);  // blue   – monthly
        private static readonly Color _accent3      = Color.FromArgb(231,  76,  60);  // red    – low stock
        private static readonly Color _sectionHdr   = Color.FromArgb(52,  73,  94);   // section label
        private static readonly Color _listHdr      = AppTheme.GridHeader;
        private static readonly Color _listAlt      = AppTheme.GridAltRow;

      

        /// <summary>
        /// Applies the full visual redesign to every control on the dashboard.
        /// Called once from Load, after texts and data are set.
        /// </summary>
        private void StyleDashboard()
        {
            // ── Page background ──────────────────────────────────────────────
            this.BackColor = _bg;

            // ── Header banner ────────────────────────────────────────────────
            headerPanel.BackColor = _header1;   // real gradient via Paint
            headerPanel.Paint    -= HeaderPanel_Paint;
            headerPanel.Paint    += HeaderPanel_Paint;
            headerPanel.Height    = 90;
            headerPanel.Padding   = new System.Windows.Forms.Padding(20, 0, 20, 0);

            bool isRtl = headerPanel.RightToLeft == RightToLeft.Yes;

            // Title: left-aligned for en-US, right-aligned for ar-SA
            lblTitle.Font      = new Font("Segoe UI Semibold", 20F, FontStyle.Regular);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize  = true;
            lblTitle.Anchor    = isRtl
                ? (AnchorStyles.Top | AnchorStyles.Right)
                : (AnchorStyles.Top | AnchorStyles.Left);
            lblTitle.Location  = new Point(
                isRtl ? headerPanel.Width - lblTitle.PreferredWidth - 24 : 24,
                14);

            // Subtitle: same horizontal side as title, below it
            lblSubtitle.Font      = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.FromArgb(210, 230, 255);
            lblSubtitle.AutoSize  = true;
            lblSubtitle.Anchor    = isRtl
                ? (AnchorStyles.Top | AnchorStyles.Right)
                : (AnchorStyles.Top | AnchorStyles.Left);
            lblSubtitle.Location  = new Point(
                isRtl ? headerPanel.Width - lblSubtitle.PreferredWidth - 24 : 24,
                56);

            // Re-anchor subtitle to stretch when resized (en-US only, RTL auto-adapts via Right anchor)
            if (!isRtl)
            {
                lblSubtitle.AutoSize = false;
                lblSubtitle.Anchor   = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                lblSubtitle.Width    = headerPanel.Width - 48;
            }

            // ── Summary cards ────────────────────────────────────────────────
            summaryPanel.BackColor = _bg;
            summaryPanel.Padding   = new System.Windows.Forms.Padding(10, 12, 10, 6);

            StyleSummaryCard(pnlSalesToday,    panelSalesColor,    lblSalesTitle,    lblSalesValue,    _accent1);
            StyleSummaryCard(pnlRevenueToday,  panelRevenueColor,  lblRevenueTitle,  lblRevenueValue,  _accent2);
            StyleSummaryCard(pnlLowStock,      panelLowStockColor, lblLowStockTitle, lblLowStockValue, _accent3);

            // ── Section label – Quick Access ──────────────────────────────────
            lblQuickAccess.Font      = new Font("Segoe UI Semibold", 10F, FontStyle.Regular);
            lblQuickAccess.ForeColor = _sectionHdr;

            // ── Quick-access button strip ─────────────────────────────────────
            quickAccessPanel.BackColor = _bg;
            quickAccessPanel.Padding   = new System.Windows.Forms.Padding(12, 8, 12, 8);
            quickAccessPanel.MinimumSize = new System.Drawing.Size(0, 110);

            //var qaBtns = new[] { btnNewSale, btnNewPurchase, btnProducts, btnCustomers,
            //                     btnSuppliers, btnPurchasesReport, btnSettings, btnSalesReport };
            //for (int i = 0; i < qaBtns.Length; i++)
            //{
            //    if (qaBtns[i] == null) continue;
            //    StyleQuickButton(qaBtns[i], i < _btnColors.Length ? _btnColors[i] : AppTheme.Primary);
            //}

            // ── Section label – Recent Activity ──────────────────────────────
            lblRecent.Font      = new Font("Segoe UI Semibold", 10F, FontStyle.Regular);
            lblRecent.ForeColor = _sectionHdr;

            // ── Recent activity list ──────────────────────────────────────────
            listRecent.BackColor        = _cardBg;
            listRecent.ForeColor        = AppTheme.TextPrimary;
            listRecent.Font             = AppTheme.FontGrid;
            listRecent.BorderStyle      = BorderStyle.FixedSingle;
            listRecent.GridLines        = false;
            listRecent.FullRowSelect    = true;
            listRecent.HeaderStyle      = ColumnHeaderStyle.Nonclickable;

            // Colour alternating rows via OwnerDraw
            listRecent.OwnerDraw      = true;
            listRecent.DrawColumnHeader -= ListRecent_DrawColumnHeader;
            listRecent.DrawItem        -= ListRecent_DrawItem;
            listRecent.DrawSubItem     -= ListRecent_DrawSubItem;
            listRecent.DrawColumnHeader += ListRecent_DrawColumnHeader;
            listRecent.DrawItem        += ListRecent_DrawItem;
            listRecent.DrawSubItem     += ListRecent_DrawSubItem;

            UpdateRecentColumnWidths();
        }

        // ── Paint handlers ───────────────────────────────────────────────────

        private void HeaderPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var p = (Panel)sender;
            using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                p.ClientRectangle, _header1, _header2,
                System.Drawing.Drawing2D.LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(brush, p.ClientRectangle);
            }
            // Thin luminous bottom border
            using (var pen = new Pen(Color.FromArgb(80, 255, 255, 255), 1))
                e.Graphics.DrawLine(pen, 0, p.Height - 1, p.Width, p.Height - 1);
        }

        private void ListRecent_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var bg = new SolidBrush(_listHdr))
                e.Graphics.FillRectangle(bg, e.Bounds);
            using (var pen = new Pen(_cardBorder, 1))
                e.Graphics.DrawRectangle(pen, e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
            TextRenderer.DrawText(e.Graphics, e.Header.Text, AppTheme.FontGridHeader,
                e.Bounds, _sectionHdr, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private void ListRecent_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = false; // handled per sub-item
        }

        private void ListRecent_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            Color bg = e.ItemIndex % 2 == 0 ? _cardBg : _listAlt;
            if ((e.Item.Selected))
                bg = AppTheme.PrimaryLight;

            using (var brush = new SolidBrush(bg))
                e.Graphics.FillRectangle(brush, e.Bounds);

            // Subtle bottom separator
            using (var pen = new Pen(Color.FromArgb(20, 0, 0, 0), 1))
                e.Graphics.DrawLine(pen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

            var textColor = e.ColumnIndex == 0 ? AppTheme.Primary : AppTheme.TextPrimary;
            var textFont = e.ColumnIndex == 0 ? AppTheme.FontSemiBold : AppTheme.FontGrid;
            TextRenderer.DrawText(e.Graphics, e.SubItem.Text,
                textFont,
                System.Drawing.Rectangle.Inflate(e.Bounds, -4, 0),
                textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        // ── Per-control helpers ──────────────────────────────────────────────

        private static void StyleSummaryCard(Panel card, Panel accentBar,
            Label title, Label value, Color accent)
        {
            card.BackColor   = _cardBg;
            card.BorderStyle = BorderStyle.None;

            // Drop-shadow illusion via Paint
            card.Paint -= (s, e) => { };
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                // outer border
                using (var pen = new Pen(_cardBorder, 1))
                    g.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            // Accent left-bar
            accentBar.BackColor = accent;
            accentBar.Width     = 6;
            accentBar.Dock      = DockStyle.Left;

            title.Font      = new Font("Segoe UI", 9F, FontStyle.Regular);
            title.ForeColor = Color.FromArgb(100, 100, 110);

            value.Font      = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            value.ForeColor = Color.FromArgb(30, 30, 40);
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

            
        }

        private void LoadDashboardMetrics()
        {
            // Permission check
            if (!_auth.HasPermission(_currentUser, Permissions.Dashboard_Metrics_View))
            {
                SetSalesTodayAmount(0m);
                SetMonthlySalesAmount(0m);
                SetLowStockCount(0);
                SetRecentActivity(new[]
                {
                    new RecentItem("Security", "Dashboard metrics hidden (no permission).", DateTime.Now)
                });
                return;
            }

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
                UpdateRecentColumnWidths();
            }
            finally { listRecent.EndUpdate(); }
        }

        private void UpdateRecentColumnWidths()
        {
            if (listRecent.Columns.Count == 0) return;

            int padding = 24;
            int totalWidth = Math.Max(0, listRecent.ClientSize.Width - padding);

            int colAreaWidth = Math.Max(90, (int)(totalWidth * 0.2));
            int colDateWidth = Math.Max(140, (int)(totalWidth * 0.25));
            int colInfoWidth = Math.Max(120, totalWidth - colAreaWidth - colDateWidth);

            colArea.Width = colAreaWidth;
            colInfo.Width = colInfoWidth;
            colDateTime.Width = colDateWidth;
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
            // Permission check
            if (!_auth.HasPermission(_currentUser, Permissions.Sales_View))
            {
                MessageBox.Show("You do not have permission to access Settings.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Open ZATCA Invoices settings form
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
            // Permission check
            if (!_auth.HasPermission(_currentUser, Permissions.Sales_Create))
            {
                MessageBox.Show("You do not have permission to create a new sale.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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
            // Permission check
            if (!_auth.HasPermission(_currentUser, Permissions.Products_View))
            {
                MessageBox.Show("You do not have permission to access Products.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
            // Permission check
            if (!_auth.HasPermission(_currentUser, Permissions.Customers_View))
            {
                MessageBox.Show("You do not have permission to access Customers.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (frmCustomers == null)
            {
                frmCustomers = new frm_addCustomer();
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
            // Permission check
            if (!_auth.HasPermission(_currentUser, Permissions.Suppliers_View))
            {
                MessageBox.Show("You do not have permission to access Suppliers.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (frmSuppliers == null)
            {
                frmSuppliers = new frm_addSupplier();
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
            // Permission check
            if (!_auth.HasPermission(_currentUser, Permissions.Reports_SalesView))
            {
                MessageBox.Show("You do not have permission to access Sales Reports.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
            // Permission check
            if (!_auth.HasPermission(_currentUser, Permissions.Reports_PurchasesView))
            {
                MessageBox.Show("You do not have permission to access Purchases Reports.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
            // Permission check
            if (!_auth.HasPermission(_currentUser, Permissions.Reports_InventoryView))
            {
                MessageBox.Show("You do not have permission to access Inventory Reports.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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

        Form frm_purchase;
        private void btnNewPurchase_Click(object sender, EventArgs e)
        {
            // Permission check
            if (!_auth.HasPermission(_currentUser, Permissions.Purchases_Create))
            {
                MessageBox.Show("You do not have permission to create a new purchase.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (frm_purchase == null)
            {
                frm_purchase = new frm_purchases();
                frm_purchase.MdiParent = frm_main.ActiveForm;
                //frm_sales_obj.Dock = DockStyle.Fill;
                frm_purchase.FormClosed += new FormClosedEventHandler(Frm_purchase_FormClosed);
                frm_purchase.WindowState = FormWindowState.Maximized;
                frm_purchase.Show();
            }
            else
            {
                frm_purchase.Activate();
            }
        }

        private void Frm_purchase_FormClosed(object sender, FormClosedEventArgs e)
        {
           frm_purchase = null;
        }
    }
}