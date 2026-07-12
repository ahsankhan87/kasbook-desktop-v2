using pos.Reports.Banks;
using pos.Sales;
using pos.Security.Admin;
using pos.Security.Authorization;
using pos.UI;
using POS.Core;
using POS.DLL;
using POS.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_main : Form
    {
        public string lang = (UsersModal.logged_in_lang.Length > 0 ? UsersModal.logged_in_lang : "en-US");

        private readonly System.Windows.Forms.Timer _inactivityTimer = new System.Windows.Forms.Timer();
        private readonly System.Windows.Forms.Timer _subscriptionMonitorTimer = new System.Windows.Forms.Timer();
        private TimeSpan _autoLogoutThreshold = TimeSpan.FromMinutes(15);
        private readonly TimeSpan _subscriptionRefreshInterval = TimeSpan.FromMinutes(30);
        private bool _isSessionLockInProgress;
        private bool _isSubscriptionExpiryHandlingInProgress;
        private string _currentLoginUsername = string.Empty;
        private string _cachedSystemId = string.Empty;
        private DateTime _cachedSubscriptionExpiry = DateTime.MinValue;
        private DateTime _lastSubscriptionRefreshUtc = DateTime.MinValue;

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        // Dashboard panel
        private Panel dashboardPanel;
        private FlowLayoutPanel quickAccessPanel;
        
        public frm_main()
        {
            Thread.CurrentThread.CurrentUICulture
               = new System.Globalization.CultureInfo(lang);

            InitializeComponent();
            
            // Tag menu and toolbar items with permission keys (DB-backed)
            // Sales
            if (this.GetType() != null)
            {
                LoadAllMenus();
            }

        }

        private void frm_main_Load(object sender, EventArgs e)
        {
            // Apply professional theme
            UI.AppTheme.Apply(this);
            StyleMainForm();

            this.Text = "Nozum ERP V3.0.0 - " + UsersModal.logged_in_branch_name + " (" + UsersModal.logged_in_username + " - " + UsersModal.logged_in_user_role + ")";

            // Re-apply DB-backed permissions to ensure module-based enabling can't elevate privileges
            AppSecurityContext.RefreshUserClaims();
            this.ApplyPermissions(AppSecurityContext.Auth, AppSecurityContext.User);

            mark_checked_lang_menu(); // language menu mark checked

            toolStripStatusLabel_username.Text = UsersModal.logged_in_username + "-" + UsersModal.logged_in_user_role;
            toolStripStatusLabel_branch_name.Text = UsersModal.logged_in_branch_name.ToString();
            toolStripStatusLabel_fiscalyear.Text = UsersModal.fiscal_year.Trim();
            toolStripStatusLabelCompanyName.Text = UsersModal.logged_in_company_name.Trim();

            toolStripButton_dashboard.PerformClick();

            // Load modules based on user permissions
            if (UsersModal.useZatcaEInvoice)
            {
                zatcaInvoicesToolStripMenuItem.Enabled = true;
                generateZATCACSIDToolStripMenuItem.Enabled = true;
            }
             else
            {
                zatcaInvoicesToolStripMenuItem.Enabled = false;
                generateZATCACSIDToolStripMenuItem.Enabled = false;
            }

            //App logging 
            POS.DLL.Log.LogAction("User Login", $"User ID: {UsersModal.logged_in_userid}, Name: {UsersModal.logged_in_username}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

            _currentLoginUsername = ResolveCurrentLoginUsername();
            _cachedSystemId = new HardwareIdentifier().GetUniqueHardwareId();
            RefreshSubscriptionSnapshot(force: true);

            InitializeInactivityLockLogout();
            InitializeRuntimeSubscriptionMonitor();
        }

        private void InitializeInactivityLockLogout()
        {
            int timeoutMinutes = new SettingsBLL().GetAutoLogoutMinutes(60);
            if (timeoutMinutes <= 0)
                timeoutMinutes = 60;

            if (timeoutMinutes < 60)
                timeoutMinutes = 60;

            _autoLogoutThreshold = TimeSpan.FromMinutes(timeoutMinutes);
            _inactivityTimer.Interval = 1000;
            _inactivityTimer.Tick -= InactivityTimer_Tick;
            _inactivityTimer.Tick += InactivityTimer_Tick;
            _inactivityTimer.Start();
        }

        private void InitializeRuntimeSubscriptionMonitor()
        {
            _subscriptionMonitorTimer.Interval = 60 * 1000; // every 1 minute (in-memory checks)
            _subscriptionMonitorTimer.Tick -= SubscriptionMonitorTimer_Tick;
            _subscriptionMonitorTimer.Tick += SubscriptionMonitorTimer_Tick;
            _subscriptionMonitorTimer.Start();
        }

        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            if (_isSessionLockInProgress || !this.Visible)
                return;

            if (GetSystemIdleTime() < _autoLogoutThreshold)
                return;

            _isSessionLockInProgress = true;
            _inactivityTimer.Stop();

            DialogResult lockResult;
            using (var lockForm = new frm_session_lock(UsersModal.logged_in_username, _currentLoginUsername, UsersModal.logged_in_userid))
            {
                lockResult = lockForm.ShowDialog(this);
            }

            if (lockResult == DialogResult.OK)
            {
                _isSessionLockInProgress = false;
                _inactivityTimer.Start();
                return;
            }

            LogoutCurrentUser("Auto Logout", "Session timeout due to inactivity");
        }

        private void SubscriptionMonitorTimer_Tick(object sender, EventArgs e)
        {
            if (_isSessionLockInProgress || _isSubscriptionExpiryHandlingInProgress || !this.Visible)
                return;

            EnsureRuntimeSubscriptionStillValid();
        }

        private bool EnsureRuntimeSubscriptionStillValid()
        {
            if (!IsSubscriptionValidNow())
            {
                HandleRuntimeSubscriptionExpiry();
                return false;
            }

            if (_cachedSubscriptionExpiry != DateTime.MinValue && DateTime.Now > _cachedSubscriptionExpiry)
            {
                HandleRuntimeSubscriptionExpiry();
                return false;
            }

            return true;
        }

        private bool IsSubscriptionValidNow()
        {
            bool needsRefresh = _cachedSubscriptionExpiry == DateTime.MinValue
                                || (DateTime.UtcNow - _lastSubscriptionRefreshUtc) >= _subscriptionRefreshInterval;

            if (needsRefresh)
                return RefreshSubscriptionSnapshot(force: false);

            if (_cachedSubscriptionExpiry == DateTime.MinValue)
                return false;

            if (DateTime.Now <= _cachedSubscriptionExpiry)
                return true;

            return RefreshSubscriptionSnapshot(force: true);
        }

        private bool RefreshSubscriptionSnapshot(bool force)
        {
            if (!force && (DateTime.UtcNow - _lastSubscriptionRefreshUtc) < _subscriptionRefreshInterval)
                return _cachedSubscriptionExpiry != DateTime.MinValue && DateTime.Now <= _cachedSubscriptionExpiry;

            DateTime expiryDate = DateTime.MinValue;

            try
            {
                var companyBll = new CompaniesBLL();
                DataTable companyDt = companyBll.GetCompany();
                if (companyDt == null || companyDt.Rows.Count == 0)
                    return true;

                DataRow dr = companyDt.Rows[0];

                int companyId = Convert.ToInt32(dr["id"]);
                int locked = Convert.ToInt32(dr["locked"]);
                string subscriptionKey = Convert.ToString(dr["subscriptionKey"]);

                if (locked > 0 || string.IsNullOrWhiteSpace(subscriptionKey))
                {
                    _cachedSubscriptionExpiry = DateTime.MinValue;
                    _lastSubscriptionRefreshUtc = DateTime.UtcNow;
                    return false;
                }

                var subscription = new Subscription();
                if (string.IsNullOrWhiteSpace(_cachedSystemId))
                    _cachedSystemId = new HardwareIdentifier().GetUniqueHardwareId();

                bool isValid = subscription.VerifySubscriptionKey(companyId, subscriptionKey, out expiryDate, _cachedSystemId);
                _lastSubscriptionRefreshUtc = DateTime.UtcNow;
                _cachedSubscriptionExpiry = isValid ? expiryDate : DateTime.MinValue;
                return isValid;
            }
            catch
            {
                // Do not force-close on transient check errors.
                return true;
            }
        }

        private void HandleRuntimeSubscriptionExpiry()
        {
            if (_isSubscriptionExpiryHandlingInProgress)
                return;

            _isSubscriptionExpiryHandlingInProgress = true;
            _subscriptionMonitorTimer.Stop();
            _inactivityTimer.Stop();

            UiMessages.ShowWarning(
                "Software subscription has expired. Please renew to continue.",
                "انتهت صلاحيات اشتراك البرنامج. يرجى التجديد للمتابعة.",
                "Software Expiration",
                "انتهاء الاشتراك");

            LogoutCurrentUser("Auto Logout", "Subscription expired while application was running");
        }

        private static TimeSpan GetSystemIdleTime()
        {
            var info = new LASTINPUTINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);

            if (!GetLastInputInfo(ref info))
                return TimeSpan.Zero;

            uint tickCount = (uint)Environment.TickCount;
            uint idleMilliseconds = tickCount - info.dwTime;
            return TimeSpan.FromMilliseconds(idleMilliseconds);
        }

        private void LogoutCurrentUser(string actionName, string actionDescription)
        {
            _inactivityTimer.Stop();
            _subscriptionMonitorTimer.Stop();

            this.Hide();

            try
            {
                POS.DLL.Log.LogAction(actionName,
                    $"User ID: {UsersModal.logged_in_userid}, Name: {UsersModal.logged_in_username}, Detail: {actionDescription}",
                    UsersModal.logged_in_userid,
                    UsersModal.logged_in_branch_id);
            }
            catch
            {
            }

            Login login_obj = new Login();
            login_obj.Show();
        }

        private string ResolveCurrentLoginUsername()
        {
            try
            {
                var bll = new UsersBLL();
                DataTable dt = bll.GetUser(UsersModal.logged_in_userid);
                if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("username"))
                {
                    string username = Convert.ToString(dt.Rows[0]["username"]);
                    if (!string.IsNullOrWhiteSpace(username))
                        return username.Trim();
                }
            }
            catch
            {
            }

            return UsersModal.logged_in_username;
        }

        private void StyleMainForm()
        {
            // Menu strip
            menuStrip1.BackColor = AppTheme.Surface;
            menuStrip1.ForeColor = AppTheme.TextPrimary;
            menuStrip1.Font = AppTheme.FontToolStrip;
            menuStrip1.Renderer = new pos.UI.FluentToolStripRenderer();
            menuStrip1.Padding = new Padding(6, 2, 6, 2);

            // Status strip
            statusStrip1.BackColor = AppTheme.PrimaryDark;
            statusStrip1.ForeColor = AppTheme.TextOnPrimary;
            statusStrip1.Font = AppTheme.FontStatusBar;
            statusStrip1.Renderer = new pos.UI.FluentToolStripRenderer();
            foreach (ToolStripItem item in statusStrip1.Items)
                item.ForeColor = AppTheme.TextOnPrimary;

            // Left navigation (side menu)
            sideMenu.BackColor = SystemColors.Control;
            sideMenu.ForeColor = SystemColors.ControlText;
            sideMenu.GripStyle = ToolStripGripStyle.Hidden;
            sideMenu.RenderMode = ToolStripRenderMode.System;
            sideMenu.AutoSize = true;
            //sideMenu.Padding = new Padding(4, 6, 4, 6);

            foreach (ToolStripItem item in sideMenu.Items)
            {
                if (item is ToolStripButton btn)
                    StyleSideMenuButton(btn);
            }

            // MDI background
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is MdiClient mdi)
                    mdi.BackColor = AppTheme.Background;
            }
        }

        private void StyleSideMenuButton(ToolStripButton btn)
        {
            btn.AutoSize = false;
            if (sideMenu.Width > 0)
                btn.Width = sideMenu.Width - 10;
            //btn.Height = 36;
            //btn.Margin = new Padding(2, 2, 2, 2);
            //btn.Padding = new Padding(6, 0, 4, 0);
            //btn.TextAlign = ContentAlignment.MiddleLeft;
            //btn.ImageAlign = ContentAlignment.MiddleLeft;
            //btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            //btn.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            btn.Font = AppTheme.FontToolStrip;
            btn.ForeColor = SystemColors.ControlText;
            btn.BackColor = SystemColors.Control;
            btn.Checked = false;
            btn.CheckOnClick = false;
        }

        private void LoadAllMenus()
        {
            // Menu items (names must match Designer)
            try
            {
                if (newTransactionToolStripMenuItem2 != null) newTransactionToolStripMenuItem2.Tag = Permissions.Sales_Create;
                if (allTransactionToolStripMenuItem1 != null) allTransactionToolStripMenuItem1.Tag = Permissions.Sales_View;
                if (salesReturnToolStripMenuItem != null) salesReturnToolStripMenuItem.Tag = Permissions.Sales_Return;
                if (salesReportToolStripMenuItem1 != null) salesReportToolStripMenuItem1.Tag = Permissions.Reports_SalesView;
                if (dailySaleReportToolStripMenuItem != null) dailySaleReportToolStripMenuItem.Tag = Permissions.Reports_SalesView;
                if (newTransactionToolStripMenuItem != null) newTransactionToolStripMenuItem.Tag = Permissions.Purchases_Create;
                if (allPurchasesToolStripMenuItem != null) allPurchasesToolStripMenuItem.Tag = Permissions.Purchases_View;
                if (purchaseReturnToolStripMenuItem != null) purchaseReturnToolStripMenuItem.Tag = Permissions.Purchases_Return;
                if (purchaseReportToolStripMenuItem != null) purchaseReportToolStripMenuItem.Tag = Permissions.Reports_PurchasesView;
                if (suppliersToolStripMenuItem != null) suppliersToolStripMenuItem.Tag = Permissions.Suppliers_View;
                if (itemsToolStripMenuItem != null) itemsToolStripMenuItem.Tag = Permissions.Products_View;
                if (productAdjustmentToolStripMenuItem != null) productAdjustmentToolStripMenuItem.Tag = Permissions.Inventory_Edit;
                if (zatcaInvoicesToolStripMenuItem != null) zatcaInvoicesToolStripMenuItem.Tag = Permissions.Sales_Zatca_View;
                if (generateZATCACSIDToolStripMenuItem != null) generateZATCACSIDToolStripMenuItem.Tag = Permissions.Sales_Zatca_Configure;
                if (debitNoteToolStripMenuItem != null) debitNoteToolStripMenuItem.Tag = Permissions.Sales_DebitNote;

                // Inventory / Products
                if (productsServicesToolStripMenuItem != null) productsServicesToolStripMenuItem.Tag = Permissions.Inventory_View;
                if (productAdjustmentToolStripMenuItem != null) productAdjustmentToolStripMenuItem.Tag = Permissions.Inventory_Edit;
                if (labelsToolStripMenuItem != null) labelsToolStripMenuItem.Tag = Permissions.Inventory_View;
                if (quantityOnHandToolStripMenuItem != null) quantityOnHandToolStripMenuItem.Tag = Permissions.Reports_InventoryView;
                if (lowStockReportToolStripMenuItem != null) lowStockReportToolStripMenuItem.Tag = Permissions.Reports_InventoryView;

                // Customers
                if (customersToolStripMenuItem != null) customersToolStripMenuItem.Tag = Permissions.Customers_View;

                // Finance / Reports
                if (journalToolStripMenuItem != null) journalToolStripMenuItem.Tag = Permissions.Journal_View;
                if (journalDaybookToolStripMenuItem != null) journalDaybookToolStripMenuItem.Tag = Permissions.Finance_View;
                if (trialBalanceToolStripMenuItem != null) trialBalanceToolStripMenuItem.Tag = Permissions.Finance_Report;
                if (profitLossToolStripMenuItem != null) profitLossToolStripMenuItem.Tag = Permissions.Finance_Report;
                if (balanceSheetToolStripMenuItem != null) balanceSheetToolStripMenuItem.Tag = Permissions.Finance_Report;
                if (accountReportToolStripMenuItem != null) accountReportToolStripMenuItem.Tag = Permissions.Finance_Report;
                if (groupReportToolStripMenuItem != null) groupReportToolStripMenuItem.Tag = Permissions.Finance_Report;
                if (banksReportToolStripMenuItem != null) banksReportToolStripMenuItem.Tag = Permissions.Finance_Report;
                if (accountPayableToolStripMenuItem != null) accountPayableToolStripMenuItem.Tag = Permissions.Reports_AccountsView;
                if (accountReceivableToolStripMenuItem != null) accountReceivableToolStripMenuItem.Tag = Permissions.Reports_AccountsView;
                if (vATDashboardToolStripMenuItem != null) vATDashboardToolStripMenuItem.Tag = Permissions.Reports_FinanceView;
                if (byPaymentMethodToolStripMenuItem != null) byPaymentMethodToolStripMenuItem.Tag = Permissions.Reports_SalesView;
                if (salesExpensesToolStripMenuItem != null) salesExpensesToolStripMenuItem.Tag = Permissions.Reports_FinanceView;
                if (salesSummaryToolStripMenuItem != null) salesSummaryToolStripMenuItem.Tag = Permissions.Reports_SalesView;
                if (salesSummaryToolStripMenuItem1 != null) salesSummaryToolStripMenuItem1.Tag = Permissions.Reports_SalesView;
                if (purchaseSummaryToolStripMenuItem != null) purchaseSummaryToolStripMenuItem.Tag = Permissions.Reports_PurchasesView;
                if (customersSummaryToolStripMenuItem != null) customersSummaryToolStripMenuItem.Tag = Permissions.Customers_View;
                if (suppliersSummaryToolStripMenuItem != null) suppliersSummaryToolStripMenuItem.Tag = Permissions.Suppliers_View;
                if (expenseDashboardToolStripMenuItem != null) expenseDashboardToolStripMenuItem.Tag = Permissions.Expenses_View;
                if (accountingDashboardToolStripMenuItem != null) accountingDashboardToolStripMenuItem.Tag = Permissions.Finance_View;

                // Security admin
                if (permissionsToolStripMenuItem != null) permissionsToolStripMenuItem.Tag = Permissions.Security_Permissions_View;
                if (rolePermissionsToolStripMenuItem != null) rolePermissionsToolStripMenuItem.Tag = Permissions.Security_Permissions_Create;
                if (userClaimsToolStripMenuItem != null) userClaimsToolStripMenuItem.Tag = Permissions.Security_Permissions_View;
                if (applicationLogsToolStripMenuItem != null) applicationLogsToolStripMenuItem.Tag = Permissions.ApplicationLogs_View;

                // users
                if (usersToolStripMenuItem != null) usersToolStripMenuItem.Tag = Permissions.Security_Users_View;
                if (warehousetoolStripMenuItem != null) warehousetoolStripMenuItem.Tag = Permissions.Inventory_View;
                if (categoriesToolStripMenuItem != null) categoriesToolStripMenuItem.Tag = Permissions.Inventory_View;
                if (unitsToolStripMenuItem != null) unitsToolStripMenuItem.Tag = Permissions.Inventory_View;
                if (locationsToolStripMenuItem != null) locationsToolStripMenuItem.Tag = Permissions.Inventory_View;
                if (brandToolStripMenuItem != null) brandToolStripMenuItem.Tag = Permissions.Inventory_View;
                if (branchToolStripMenuItem != null) branchToolStripMenuItem.Tag = Permissions.Branches_View;
                if (profileToolStripMenuItem != null) profileToolStripMenuItem.Tag = Permissions.Profile_View;
                if (financialYearsToolStripMenuItem != null) financialYearsToolStripMenuItem.Tag = Permissions.FinancialYears_View;

                // Accounts / Banks / Expenses
                if (groupsToolStripMenuItem != null) groupsToolStripMenuItem.Tag = Permissions.Group_View;
                if (accountsToolStripMenuItem1 != null) accountsToolStripMenuItem1.Tag = Permissions.Account_View;
                if (chartOfAccountsToolStripMenuItem != null) chartOfAccountsToolStripMenuItem.Tag = Permissions.Account_View;
                if (banksToolStripMenuItem != null) banksToolStripMenuItem.Tag = Permissions.Bank_View;
                if (expensesToolStripMenuItem != null) expensesToolStripMenuItem.Tag = Permissions.Expenses_View;

                // ICT
                if (iCTToolStripMenuItem != null) iCTToolStripMenuItem.Tag = Permissions.Sales_ictTransactions_View;
                if (iCTRequestToolStripMenuItem != null) iCTRequestToolStripMenuItem.Tag = Permissions.Sales_ictTransactions_Request;
                if (iCTReleaseToolStripMenuItem != null) iCTReleaseToolStripMenuItem.Tag = Permissions.Sales_ictTransactions_Release;

                // Discounts
                if (discountSchemesToolStripMenuItem != null) discountSchemesToolStripMenuItem.Tag = Permissions.Discounts_ManageSchemes;
                if (stockSuppressionToolStripMenuItem != null) stockSuppressionToolStripMenuItem.Tag = Permissions.Inventory_Edit;
            }
            catch { /* ignore if some items are not present in this build */ }

            // ToolStrip buttons
            try
            {
                if (toolStripButton_sales != null) toolStripButton_sales.Tag = Permissions.Sales_Create;
                if (toolStripButton_purchase != null) toolStripButton_purchase.Tag = Permissions.Purchases_Create;
                if (toolStripButton_products != null) toolStripButton_products.Tag = Permissions.Products_View;
                if (toolStripButton_customers != null) toolStripButton_customers.Tag = Permissions.Customers_View;
                if (toolStripButton_suppliers != null) toolStripButton_suppliers.Tag = Permissions.Suppliers_View;
                if (toolStripButtonDailySaleReport != null) toolStripButtonDailySaleReport.Tag = Permissions.Reports_SalesView;
                if (toolStripButtonNewPOS != null) toolStripButtonNewPOS.Tag = Permissions.Sales_Create;
            }
            catch { /* ignore */ }
        }

        private void load_modules()
        {
            UsersDLL usersBLL_obj = new UsersDLL();

            DataTable users_modules = usersBLL_obj.GetUserModules(UsersModal.logged_in_userid);
            string module_name = "";
            foreach (DataRow dr in users_modules.Rows)
            {
                module_name = dr["module_name"].ToString();

                switch (module_name)
                {
                    //Master
                    case "Warehouse":
                        warehousetoolStripMenuItem.Enabled = true;
                        break;
                    case "Brands":
                        brandToolStripMenuItem.Enabled = true;
                        break;
                    case "Branches":
                        branchToolStripMenuItem.Enabled = true;
                        break;
                    case "Categories":
                        categoriesToolStripMenuItem.Enabled = true;
                        break;
                    case "DB Backup":
                        dBBackupToolStripMenuItem.Enabled = true;
                        break;
                    case "Profile":
                        profileToolStripMenuItem.Enabled = true;
                        break;
                    case "Locations":
                        locationsToolStripMenuItem.Enabled = true;
                        break;
                    case "Language":
                        languageToolStripMenuItem.Enabled = true;
                        break;
                    case "Taxes":
                        taxesVATToolStripMenuItem1.Enabled = true;
                        break;
                    case "Users":
                        usersToolStripMenuItem.Enabled = true;
                        break;
                    case "Units":
                        unitsToolStripMenuItem.Enabled = true;
                        break;
                    //////POS
                    case "Sales Transaction":
                        newTransactionToolStripMenuItem2.Enabled = true;
                        break;
                    case "Sales Return":
                        salesReturnToolStripMenuItem.Enabled = true;
                        break;
                    case "All Sales":
                        allTransactionToolStripMenuItem1.Enabled = true;
                        break;
                    case "Purchase Transaction":
                        newTransactionToolStripMenuItem.Enabled = true;
                        break;
                    case "Purchase Return":
                        purchaseReturnToolStripMenuItem.Enabled = true;
                        break;
                    case "All Purchases":
                        allPurchasesToolStripMenuItem.Enabled = true;
                        break;
                    case "Products":
                        productsServicesToolStripMenuItem.Enabled = true;
                        break;
                    ////Accounts
                    case "Customers":
                        customersToolStripMenuItem.Enabled = true;
                        break;
                    case "Suppliers":
                        suppliersToolStripMenuItem.Enabled = true;
                        break;
                    case "Journal":
                        journalToolStripMenuItem.Enabled = true;
                        break;
                    case "Groups":
                        groupsToolStripMenuItem.Enabled = true;
                        break;
                    case "Accounts":
                        accountsToolStripMenuItem1.Enabled = true;
                        break;
                    case "Journal Daybook":
                        journalDaybookToolStripMenuItem.Enabled = true;
                        break;
                    case "Trial Balance":
                        trialBalanceToolStripMenuItem.Enabled = true;
                        break;
                    case "Profit and Loss":
                        profitLossToolStripMenuItem.Enabled = true;
                        break;
                    case "Balance Sheet":
                        balanceSheetToolStripMenuItem.Enabled = true;
                        break;
                    ////Reports
                    case "Sales":
                        salesToolStripMenuItem.Enabled = true;
                        break;
                    case "Purchases":
                        purchasesToolStripMenuItem1.Enabled = true;
                        break;
                    case "Account Report":
                        accountReportToolStripMenuItem.Enabled = true;
                        break;
                    case "Group Report":
                        groupReportToolStripMenuItem.Enabled = true;
                        break;
                    ///
                    //HR
                    case "Employees":
                        employeesToolStripMenuItem.Enabled = true;
                        break;
                }

            }

        }

        void mark_checked_lang_menu()
        {
            var current_lang_code = System.Globalization.CultureInfo.CurrentCulture;

            if (lang == "en-US")
            {
                englishToolStripMenuItem.Checked = true;
            }
            else if (lang == "ar-SA")
            {
                arabicToolStripMenuItem.Checked = true;
            }


        }
        private void Enable_all_menus(bool status = true)
        {
            foreach (ToolStripMenuItem i in menuStrip1.Items)
            {
                for (int x = 0; x <= i.DropDownItems.Count - 1; x++)
                {
                    //i.DropDownItems[x].Visible = true;
                    i.DropDownItems[x].Enabled = status;
                }
            }

        }

        Form frm_customer_obj;
        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        void frm_customer_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_customer_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_supplier_obj;
        private void suppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_supplier_obj == null)
            {
                frm_supplier_obj = new frm_addSupplier();
                frm_supplier_obj.MdiParent = this;
                frm_supplier_obj.FormClosed += new FormClosedEventHandler(frm_supplier_obj_FormClosed);
                //frm_cust.Dock = DockStyle.Fill;
                frm_supplier_obj.Show();
            }
            else
            {
                frm_supplier_obj.Activate();
            }
        }

        void frm_supplier_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_supplier_obj = null;
        }

        Form frm_employee_obj;
        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_employee_obj == null)
            {
                frm_employee_obj = new frm_employees();
                frm_employee_obj.MdiParent = this;
                frm_employee_obj.FormClosed += new FormClosedEventHandler(frm_employee_obj_FormClosed);
                //frm_cust.Dock = DockStyle.Fill;
                frm_employee_obj.Show();
            }
            else { frm_employee_obj.Activate(); }
        }

        void frm_employee_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_employees frm_employee_obj = new frm_employees();
            frm_employee_obj = null;
        }

        Form frm_products_obj;
        private void productsServicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_products_obj == null)
            {
                frm_products_obj = new frm_product_full_detail();
                frm_products_obj.MdiParent = this;
                //frm_cust.Dock = DockStyle.Fill;
                frm_products_obj.FormClosed += new FormClosedEventHandler(frm_products_obj_FormClosed);
                frm_products_obj.Show();
            }
            else
            {
                frm_products_obj.Activate();
            }
        }

        void frm_products_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_products_obj = null;
        }

        Form frm_purchases_obj;
        private void newTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_purchases_obj == null)
            {
                frm_purchases_obj = new frm_purchases();
                frm_purchases_obj.MdiParent = this;
                frm_purchases_obj.FormClosed += new FormClosedEventHandler(frm_purchases_obj_FormClosed);
                frm_purchases_obj.Show();
            }
            else
            {
                frm_purchases_obj.Activate();
            }

        }

        void frm_purchases_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_purchases_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_sales_obj;
        private void newTransactionToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //if (frm_sales_obj == null)
            // {
            frm_sales_obj = new frm_sales();
            frm_sales_obj.MdiParent = this;

            //frm_sales_obj.Dock = DockStyle.Fill;
            //frm_sales_obj.FormClosed += new FormClosedEventHandler(frm_sales_obj_FormClosed);
            frm_sales_obj.WindowState = FormWindowState.Maximized;
            frm_sales_obj.Show();

            // }
            // else { frm_sales_obj.Activate();  }

        }

        void frm_sales_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_sales_obj = null;
            //throw new NotImplementedException();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        Form frm_taxes_obj;
        private void taxesVATToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (frm_taxes_obj == null)
            {
                frm_taxes_obj = new frm_taxes();
                frm_taxes_obj.MdiParent = this;
                //frm_taxes_obj.Dock = DockStyle.Fill;
                frm_taxes_obj.FormClosed += new FormClosedEventHandler(frm_taxes_obj_FormClosed);
                frm_taxes_obj.Show();
                //frm_taxes_obj.WindowState = FormWindowState.Maximized;
            }
            else { frm_taxes_obj.Activate(); }

        }

        void frm_taxes_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_taxes_obj = null;
        }

        Form frm_categories_obj;
        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_categories_obj == null)
            {
                frm_categories_obj = new frm_categories();
                frm_categories_obj.MdiParent = this;
                frm_categories_obj.FormClosed += new FormClosedEventHandler(frm_categories_obj_FormClosed);
                frm_categories_obj.Show();
            }
            else
            {
                frm_categories_obj.Activate();
            }

        }

        void frm_categories_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_categories_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_units_obj;
        private void unitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_units_obj == null)
            {
                frm_units_obj = new frm_units();
                frm_units_obj.MdiParent = this;
                frm_units_obj.FormClosed += new FormClosedEventHandler(frm_units_obj_FormClosed);
                frm_units_obj.Show();

            }
            else { frm_units_obj.Activate(); }

        }

        void frm_units_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_units_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_locations_obj;
        private void locationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_locations_obj == null)
            {
                frm_locations_obj = new frm_locations();
                frm_locations_obj.MdiParent = this;
                frm_locations_obj.FormClosed += new FormClosedEventHandler(frm_locations_obj_FormClosed);
                frm_locations_obj.Show();
            }
            else { frm_locations_obj.Activate(); }

        }

        void frm_locations_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_locations_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_brands_obj;
        private void brandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_brands_obj == null)
            {
                frm_brands_obj = new frm_brands();
                frm_brands_obj.MdiParent = this;
                frm_brands_obj.FormClosed += new FormClosedEventHandler(frm_brands_obj_FormClosed);
                frm_brands_obj.Show();
            }
            else
            {
                frm_brands_obj.Activate();
            }

        }

        void frm_brands_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_brands_obj = null; //throw new NotImplementedException();
        }

        Form frm_discount_schemes_obj;
        private void discountSchemesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_discount_schemes_obj == null)
            {
                frm_discount_schemes_obj = new Discounts.frm_discount_schemes();
                frm_discount_schemes_obj.MdiParent = this;
                frm_discount_schemes_obj.FormClosed += new FormClosedEventHandler(frm_discount_schemes_obj_FormClosed);
                frm_discount_schemes_obj.Show();
            }
            else
            {
                frm_discount_schemes_obj.Activate();
            }
        }

        void frm_discount_schemes_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_discount_schemes_obj = null;
        }

        Form frm_update_com_obj;
        private void profileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_update_com_obj == null)
            {
                frm_update_com_obj = new frm_updateCompany();
                frm_update_com_obj.MdiParent = this;
                frm_update_com_obj.FormClosed += new FormClosedEventHandler(frm_update_com_obj_FormClosed);
                frm_update_com_obj.Show();
            }
            else { frm_update_com_obj.Activate(); }

        }

        void frm_update_com_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_update_com_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_all_sales_obj;
        private void allTransactionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (frm_all_sales_obj == null)
            {
                frm_all_sales_obj = new frm_all_sales();
                frm_all_sales_obj.MdiParent = this;
                frm_all_sales_obj.FormClosed += new FormClosedEventHandler(frm_all_sales_obj_FormClosed);
                frm_all_sales_obj.Show();
            }
            else { frm_all_sales_obj.Activate(); }

        }

        void frm_all_sales_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_all_sales_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_all_purchases_obj;
        private void allPurchasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_all_purchases_obj == null)
            {
                frm_all_purchases_obj = new frm_all_purchases();
                frm_all_purchases_obj.MdiParent = this;
                frm_all_purchases_obj.FormClosed += new FormClosedEventHandler(frm_all_purchases_obj_FormClosed);
                frm_all_purchases_obj.Show();
            }
            else { frm_all_purchases_obj.Activate(); }

        }

        void frm_all_purchases_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_all_purchases_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_sales_report_obj;
        private void salesReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (frm_sales_report_obj == null)
            {
                frm_sales_report_obj = new frm_SalesReport();
                frm_sales_report_obj.MdiParent = this;
                frm_sales_report_obj.FormClosed += new FormClosedEventHandler(frm_sales_report_obj_FormClosed);
                frm_sales_report_obj.Show();
            }
            else { frm_sales_report_obj.Activate(); }

        }

        void frm_sales_report_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_sales_report_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_purchase_report_obj;
        private void purchaseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_purchase_report_obj == null)
            {
                frm_purchase_report_obj = new frm_PurchasesReport();
                frm_purchase_report_obj.MdiParent = this;
                frm_purchase_report_obj.FormClosed += new FormClosedEventHandler(frm_purchase_report_obj_FormClosed);
                frm_purchase_report_obj.Show();
            }
            else { frm_purchase_report_obj.Activate(); }

        }

        void frm_purchase_report_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_purchase_report_obj = null;
            //throw new NotImplementedException();
        }

        Form frm_sales_return_obj;
        private void salesReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_sales_return_obj == null)
            {
                frm_sales_return_obj = new frm_sales_return();
                frm_sales_return_obj.MdiParent = this;
                frm_sales_return_obj.FormClosed += new FormClosedEventHandler(frm_sales_return_obj_FormClosed);
                frm_sales_return_obj.Show();
            }
            else { frm_sales_return_obj.Activate(); }

        }

        void frm_sales_return_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_sales_return_obj = null;
            //throw new NotImplementedException();
        }

        private void frm_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isSessionLockInProgress || _isSubscriptionExpiryHandlingInProgress)
                return;

            // Only prompt on user-initiated close
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            try
            {
                var backupBll = new BackupBLL();
                if (!backupBll.HasBackupInLastDays(7))
                {
                    var backupWarn = UiMessages.ConfirmYesNo(
                        "No database backup was taken in the last 7 days.\n\nDo you still want to exit?",
                        "لم يتم أخذ نسخة احتياطية لقاعدة البيانات خلال آخر 7 أيام.\n\nهل تريد الخروج؟",
                        captionEn: "Backup Reminder",
                        captionAr: "تذكير النسخ الاحتياطي",
                        defaultButton: MessageBoxDefaultButton.Button2);

                    if (backupWarn == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            catch
            {
                // If backup check fails, do not block closing.
            }

            var result = UiMessages.ConfirmYesNo(
                "Are you sure you want to exit the application?",
                "هل أنت متأكد أنك تريد إغلاق التطبيق؟",
                captionEn: "Exit",
                captionAr: "خروج",
                defaultButton: MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                Application.ExitThread();
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            //App logging 
            POS.DLL.Log.LogAction("Application Exit", $"User ID: {UsersModal.logged_in_userid}, Name: {UsersModal.logged_in_username}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

            this.Close();
        }


        Form user_obj;
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (user_obj == null)
            {
                user_obj = new frm_adduser();
                user_obj.MdiParent = this;
                user_obj.FormClosed += new FormClosedEventHandler(user_obj_FormClosed);
                user_obj.Show();
            }
            else
            {
                user_obj.Activate();
            }
        }

        void user_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            user_obj = null;
            //throw new NotImplementedException();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _inactivityTimer.Stop();
            _subscriptionMonitorTimer.Stop();
            LogoutCurrentUser("User Logout", "Manual logout");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _inactivityTimer.Stop();
            _subscriptionMonitorTimer.Stop();
            _inactivityTimer.Dispose();
            _subscriptionMonitorTimer.Dispose();
            base.OnFormClosed(e);
        }

        Form branch_obj;
        private void branchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (branch_obj == null)
            {
                branch_obj = new frm_branches();
                branch_obj.MdiParent = this;
                branch_obj.FormClosed += new FormClosedEventHandler(branch_obj_FormClosed);
                branch_obj.Show();
            }
            else { branch_obj.Activate(); }

        }

        void branch_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            branch_obj = null;
            //throw new NotImplementedException();
        }

        Form group_obj;
        private void groupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (group_obj == null)
            {
                group_obj = new frm_groups();
                group_obj.MdiParent = this;
                group_obj.FormClosed += new FormClosedEventHandler(group_obj_FormClosed);
                group_obj.Show();
            }
            else { group_obj.Activate(); }

        }

        void group_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            group_obj = null;
            //throw new NotImplementedException();
        }

        Form account_obj;
        private void accountsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (account_obj == null)
            {
                account_obj = new frm_accounts();
                account_obj.MdiParent = this;
                account_obj.FormClosed += new FormClosedEventHandler(account_obj_FormClosed);
                account_obj.Show();
            }
            else { account_obj.Activate(); }

        }

        void account_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            account_obj = null;
            //throw new NotImplementedException();
        }

        Form journal_obj;
        private void journalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (journal_obj == null)
            {
                journal_obj = new frm_journal_entries();
                journal_obj.MdiParent = this;
                journal_obj.FormClosed += new FormClosedEventHandler(journal_obj_FormClosed);
                journal_obj.Show();
            }
            else
            {
                journal_obj.Activate();
            }

        }

        void journal_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            journal_obj = null;
            //throw new NotImplementedException();
        }

        Form daybook_obj;
        private void journalDaybookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (daybook_obj == null)
            {
                daybook_obj = new frm_journal_daybook();
                daybook_obj.MdiParent = this;
                daybook_obj.FormClosed += new FormClosedEventHandler(daybook_obj_FormClosed);
                daybook_obj.Show();
            }
            else
            {
                daybook_obj.Activate();
            }

        }

        void daybook_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            daybook_obj = null;
            //throw new NotImplementedException();
        }

        Form account_report_obj;
        private void accountReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (account_report_obj == null)
            {
                account_report_obj = new frm_account_report();
                account_report_obj.MdiParent = this;
                account_report_obj.FormClosed += new FormClosedEventHandler(account_report_obj_FormClosed);
                account_report_obj.Show();
            }
            else
            {
                account_report_obj.Activate();
            }

        }

        void account_report_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            account_report_obj = null;
            //throw new NotImplementedException();
        }

        Form group_report_obj;
        private void groupReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (group_report_obj == null)
            {
                group_report_obj = new frm_group_report();
                group_report_obj.MdiParent = this;
                group_report_obj.FormClosed += new FormClosedEventHandler(group_report_obj_FormClosed);
                group_report_obj.Show();
            }
            else
            {
                group_report_obj.Activate();
            }

        }

        void group_report_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            group_report_obj = null;
            //throw new NotImplementedException();
        }

        Form trial_obj;
        private void trialBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (trial_obj == null)
            {
                trial_obj = new frm_trialbalance_report();
                trial_obj.MdiParent = this;
                trial_obj.FormClosed += new FormClosedEventHandler(trial_obj_FormClosed);
                trial_obj.Show();
            }
            else
            {
                trial_obj.Activate();
            }

        }

        void trial_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            trial_obj = null;
            //throw new NotImplementedException();
        }

        Form customerWiseSalesReport;
        private void customerWiseSalesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (customerWiseSalesReport == null)
            {
                customerWiseSalesReport = new frm_customerWiseSalesReport();
                customerWiseSalesReport.MdiParent = this;
                customerWiseSalesReport.FormClosed += new FormClosedEventHandler(customerWiseSalesReport_FormClosed);
                customerWiseSalesReport.Show();
            }
            else
            {
                customerWiseSalesReport.Activate();
            }
        }

        void customerWiseSalesReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            customerWiseSalesReport = null;
            //throw new NotImplementedException();
        }

        Form frm_productWiseSalesReport;
        private void productWiseSalesSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_productWiseSalesReport == null)
            {
                frm_productWiseSalesReport = new frm_productWiseSalesReport();
                frm_productWiseSalesReport.MdiParent = this;
                frm_productWiseSalesReport.FormClosed += new FormClosedEventHandler(frm_productWiseSalesReport_FormClosed);
                frm_productWiseSalesReport.Show();
            }
            else
            {
                frm_productWiseSalesReport.Activate();
            }
        }

        void frm_productWiseSalesReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_productWiseSalesReport = null;
            //throw new NotImplementedException();
        }

        Form frm_categorySummary;
        private void categoryWiseSalesSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_categorySummary == null)
            {
                frm_categorySummary = new frm_categoryWiseSalesReport();
                frm_categorySummary.MdiParent = this;
                frm_categorySummary.FormClosed += new FormClosedEventHandler(frm_categorySummary_FormClosed);
                frm_categorySummary.Show();
            }
            else
            {
                frm_categorySummary.Activate();
            }
        }

        void frm_categorySummary_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_categorySummary = null;
            //throw new NotImplementedException();
        }

        FrmDbBackup frm_backup;
        private void dBBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_backup == null)
            {
                frm_backup = new FrmDbBackup();
                frm_backup.MdiParent = this;
                frm_backup.FormClosed += new FormClosedEventHandler(frm_backup_FormClosed);
                frm_backup.Show();
            }
            else
            {
                frm_backup.Activate();
            }

        }

        void frm_backup_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_backup = null;
        }

        private void arabicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (Thread.CurrentThread.CurrentUICulture.IetfLanguageTag)
            {
                case "en-US":
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar-SA");
                    break;
            }
            this.Controls.Clear();
            InitializeComponent();
            load_modules();
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {

            switch (Thread.CurrentThread.CurrentUICulture.IetfLanguageTag)
            {
                case "ar-SA":
                    Thread.CurrentThread.CurrentUICulture
                    = new System.Globalization.CultureInfo("en-US"); break;
            }
            this.Controls.Clear();
            InitializeComponent();
            load_modules();
        }

        frm_warehouse_report warehousefrm;
        private void warehouseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (warehousefrm == null)
            {
                warehousefrm = new frm_warehouse_report();
                warehousefrm.MdiParent = this;
                warehousefrm.FormClosed += new FormClosedEventHandler(warehousefrm_FormClosed);
                warehousefrm.Show();
            }
            else
            {
                warehousefrm.Activate();
            }

        }

        void warehousefrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            warehousefrm = null;
            //throw new NotImplementedException();
        }

        frm_bulk_edit_product bulkEditProductsfrm;
        private void editItemDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bulkEditProductsfrm == null)
            {
                bulkEditProductsfrm = new frm_bulk_edit_product();
                bulkEditProductsfrm.MdiParent = this;
                bulkEditProductsfrm.FormClosed += new FormClosedEventHandler(bulkEditProductsfrm_FormClosed);
                bulkEditProductsfrm.Show();
            }
            else
            {
                bulkEditProductsfrm.Activate();
            }
        }

        void bulkEditProductsfrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            bulkEditProductsfrm = null;
            //throw new NotImplementedException();
        }

        frm_purchase_return purchaseReturnFrm;
        private void purchaseReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (purchaseReturnFrm == null)
            {
                purchaseReturnFrm = new frm_purchase_return();
                purchaseReturnFrm.MdiParent = this;
                purchaseReturnFrm.FormClosed += new FormClosedEventHandler(purchaseReturnFrm_FormClosed);
                purchaseReturnFrm.Show();
            }
            else
            {
                purchaseReturnFrm.Activate();
            }
        }

        void purchaseReturnFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            purchaseReturnFrm = null;
            //throw new NotImplementedException();
        }

        frm_low_stock_products lowStockfrm;
        private void lowStockInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lowStockfrm == null)
            {
                lowStockfrm = new frm_low_stock_products();
                lowStockfrm.MdiParent = this;
                lowStockfrm.FormClosed += new FormClosedEventHandler(lowStockfrm_FormClosed);
                lowStockfrm.Show();
            }
            else
            {
                lowStockfrm.Activate();
            }
        }

        void lowStockfrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            lowStockfrm = null;
            //throw new NotImplementedException();
        }

        frm_all_estimates allEstimatesFrm;
        private void allQuotationsEstimatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (allEstimatesFrm == null)
            {
                allEstimatesFrm = new frm_all_estimates();
                allEstimatesFrm.MdiParent = this;
                allEstimatesFrm.FormClosed += new FormClosedEventHandler(allEstimatesFrm_FormClosed);
                allEstimatesFrm.Show();
            }
            else
            {
                allEstimatesFrm.Activate();
            }
        }

        void allEstimatesFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            allEstimatesFrm = null;
            //throw new NotImplementedException();
        }

        frm_purchases_order purchaseorder_frm;
        private void purchaseOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (purchaseorder_frm == null)
            {
                purchaseorder_frm = new frm_purchases_order();
                purchaseorder_frm.MdiParent = this;
                purchaseorder_frm.FormClosed += new FormClosedEventHandler(purchaseorder_frm_FormClosed);
                purchaseorder_frm.Show();
            }
            else
            {
                purchaseorder_frm.Activate();
            }
        }

        void purchaseorder_frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            purchaseorder_frm = null;
            //throw new NotImplementedException();
        }

        frm_all_purchases_orders all_po_frm;
        private void allPurchaseOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (all_po_frm == null)
            {
                all_po_frm = new frm_all_purchases_orders();
                all_po_frm.MdiParent = this;
                all_po_frm.FormClosed += new FormClosedEventHandler(all_po_frm_FormClosed);
                all_po_frm.Show();
            }
            else
            {
                all_po_frm.Activate();
            }
        }

        void all_po_frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            all_po_frm = null;
            //throw new NotImplementedException();
        }

        frm_country_origin coun_origin_frm;
        private void countryOriginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (coun_origin_frm == null)
            {
                coun_origin_frm = new frm_country_origin();
                coun_origin_frm.MdiParent = this;
                coun_origin_frm.FormClosed += new FormClosedEventHandler(coun_origin_frm_FormClosed);
                coun_origin_frm.Show();
            }
            else
            {
                coun_origin_frm.Activate();
            }
        }

        void coun_origin_frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            coun_origin_frm = null;
            //throw new NotImplementedException();
        }

        frm_product_groups product_group_frm;
        private void productGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (product_group_frm == null)
            {
                product_group_frm = new frm_product_groups();
                product_group_frm.MdiParent = this;
                product_group_frm.FormClosed += new FormClosedEventHandler(product_group_frm_FormClosed);
                product_group_frm.Show();
            }
            else
            {
                product_group_frm.Activate();
            }
        }

        void product_group_frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            product_group_frm = null;
            //throw new NotImplementedException();
        }

        frm_alt_products frm_alt_products;
        private void alternateProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_alt_products == null)
            {
                frm_alt_products = new frm_alt_products();
                frm_alt_products.MdiParent = this;
                frm_alt_products.FormClosed += new FormClosedEventHandler(frm_alt_products_FormClosed);
                frm_alt_products.Show();
            }
            else
            {
                frm_alt_products.Activate();
            }
        }

        void frm_alt_products_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_alt_products = null;
        }

        frm_products_labels frm_labels;
        private void labelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_labels == null)
            {
                frm_labels = new frm_products_labels();
                frm_labels.MdiParent = this;
                frm_labels.FormClosed += new FormClosedEventHandler(frm_labels_FormClosed);
                frm_labels.Show();
            }
            else
            {
                frm_labels.Activate();
            }
        }

        void frm_labels_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_labels = null;
        }

        frm_product_adjustment frm_adjust;
        private void productAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_adjust == null)
            {
                frm_adjust = new frm_product_adjustment();
                frm_adjust.MdiParent = this;
                frm_adjust.FormClosed += new FormClosedEventHandler(frm_adjust_FormClosed);
                frm_adjust.Show();
            }
            else
            {
                frm_adjust.Activate();
            }
        }

        void frm_adjust_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_adjust = null;
        }

        private void SalesToolStripButton_Click(object sender, EventArgs e)
        {
            frm_sales_obj = new frm_sales();
            frm_sales_obj.MdiParent = this;

            //frm_sales_obj.Dock = DockStyle.Fill;
            //frm_sales_obj.FormClosed += new FormClosedEventHandler(frm_sales_obj_FormClosed);
            frm_sales_obj.WindowState = FormWindowState.Maximized;
            frm_sales_obj.Show();
        }

        private void PuchaseToolStripButton_Click(object sender, EventArgs e)
        {
            if (frm_purchases_obj == null)
            {
                frm_purchases_obj = new frm_purchases();
                frm_purchases_obj.MdiParent = this;
                frm_purchases_obj.FormClosed += new FormClosedEventHandler(frm_purchases_obj_FormClosed);
                frm_purchases_obj.Show();
            }
            else
            {
                frm_purchases_obj.Activate();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (purchaseorder_frm == null)
            {
                purchaseorder_frm = new frm_purchases_order();
                purchaseorder_frm.MdiParent = this;
                purchaseorder_frm.FormClosed += new FormClosedEventHandler(purchaseorder_frm_FormClosed);
                purchaseorder_frm.Show();
            }
            else
            {
                purchaseorder_frm.Activate();
            }
        }

        private void ProductsToolStripButton_Click(object sender, EventArgs e)
        {
            if (frm_products_obj == null)
            {
                frm_products_obj = new frm_product_full_detail();
                frm_products_obj.MdiParent = this;
                //frm_cust.Dock = DockStyle.Fill;
                frm_products_obj.FormClosed += new FormClosedEventHandler(frm_products_obj_FormClosed);
                frm_products_obj.Show();
            }
            else
            {
                frm_products_obj.Activate();
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (frm_sales_return_obj == null)
            {
                frm_sales_return_obj = new frm_sales_return();
                frm_sales_return_obj.MdiParent = this;
                frm_sales_return_obj.FormClosed += new FormClosedEventHandler(frm_sales_return_obj_FormClosed);
                frm_sales_return_obj.Show();
            }
            else { frm_sales_return_obj.Activate(); }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (frm_labels == null)
            {
                frm_labels = new frm_products_labels();
                frm_labels.MdiParent = this;
                frm_labels.FormClosed += new FormClosedEventHandler(frm_labels_FormClosed);
                frm_labels.Show();
            }
            else
            {
                frm_labels.Activate();
            }
        }

        private void CustomersToolStripButton_Click(object sender, EventArgs e)
        {
            if (frm_customer_obj == null)
            {
                frm_customer_obj = new frm_addCustomer();
                frm_customer_obj.MdiParent = this;
                frm_customer_obj.FormClosed += new FormClosedEventHandler(frm_customer_obj_FormClosed);
                frm_customer_obj.Show();
                //frm_customer_obj.WindowState = FormWindowState.Maximized;
            }
            else
            {
                frm_customer_obj.Activate();
            }
        }

        private void SuppliersToolStripButton_Click(object sender, EventArgs e)
        {
            if (frm_supplier_obj == null)
            {
                frm_supplier_obj = new frm_addSupplier();
                frm_supplier_obj.MdiParent = this;
                frm_supplier_obj.FormClosed += new FormClosedEventHandler(frm_supplier_obj_FormClosed);
                //frm_cust.Dock = DockStyle.Fill;
                frm_supplier_obj.Show();
            }
            else
            {
                frm_supplier_obj.Activate();
            }
        }

        frm_fiscal_years frm_fyear;
        private void financialYearsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_fyear == null)
            {
                frm_fyear = new frm_fiscal_years();
                frm_fyear.MdiParent = this;
                frm_fyear.FormClosed += new FormClosedEventHandler(frm_fyear_FormClosed);
                frm_fyear.Show();
            }
            else
            {
                frm_fyear.Activate();
            }
        }

        void frm_fyear_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_fyear = null;
        }

        frm_profitandloss_report frm_pl;
        private void profitLossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_pl == null)
            {
                frm_pl = new frm_profitandloss_report();
                frm_pl.MdiParent = this;
                frm_pl.FormClosed += new FormClosedEventHandler(frm_pl_FormClosed);
                frm_pl.Show();
            }
            else
            {
                frm_pl.Activate();
            }
        }

        void frm_pl_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_pl = null;
        }

        frm_balancesheet_report frm_blsheet;
        private void balanceSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_blsheet == null)
            {
                frm_blsheet = new frm_balancesheet_report();
                frm_blsheet.MdiParent = this;
                frm_blsheet.FormClosed += new FormClosedEventHandler(frm_blsheet_FormClosed);
                frm_blsheet.Show();
            }
            else
            {
                frm_blsheet.Activate();
            }
        }

        void frm_blsheet_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_blsheet = null;
        }

        frm_daily_salesReport dsr_frm;
        private void dailySaleReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dsr_frm == null)
            {
                dsr_frm = new frm_daily_salesReport();
                dsr_frm.MdiParent = this;
                dsr_frm.FormClosed += new FormClosedEventHandler(dsr_frm_FormClosed);
                dsr_frm.Show();
            }
            else
            {
                dsr_frm.Activate();
            }
        }

        void dsr_frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            dsr_frm = null;
        }

        frm_SalesReport frm_SalesReport;
        private void toolStripButtonDailySaleReport_Click(object sender, EventArgs e)
        {
            if (frm_SalesReport == null)
            {
                frm_SalesReport = new frm_SalesReport();
                frm_SalesReport.MdiParent = this;
                frm_SalesReport.FormClosed += new FormClosedEventHandler(Frm_SalesReport_FormClosed);
                frm_SalesReport.Show();
            }
            else
            {
                frm_SalesReport.Activate();
            }
        }

        private void Frm_SalesReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_SalesReport = null;
        }

        private void frm_main_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyCode == Keys.F2)
                //{
                //    newTransactionToolStripMenuItem2.PerformClick();
                //} 
                //if (e.KeyCode == Keys.F3)
                //{
                //    newTransactionToolStripMenuItem.PerformClick();
                //}
                //if (e.KeyCode == Keys.F4)
                //{
                //    productsServicesToolStripMenuItem.PerformClick();
                //}

            }
            catch (Exception ex)
            {
                UiMessages.ShowWarning(
                    ex.Message,
                    ex.Message,
                    captionEn: "Warning",
                    captionAr: "تنبيه");
            }
        }

        Form frm_location_transfer;
        private void productLocationTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bulkEditProductsfrm == null)
            {
                bulkEditProductsfrm = new frm_bulk_edit_product();
                bulkEditProductsfrm.MdiParent = this;
                bulkEditProductsfrm.FormClosed += new FormClosedEventHandler(bulkEditProductsfrm_FormClosed);
                bulkEditProductsfrm.Show();
            }
            else
            {
                bulkEditProductsfrm.Activate();
            }
        }

        void frm_location_transfer_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_location_transfer = null;
        }

        private void closeAllFormsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Form> openForms = new List<Form>();

            foreach (Form f in Application.OpenForms)
                openForms.Add(f);

            foreach (Form f in openForms)
            {
                if (f.Name != "frm_main")
                    f.Close();
            }
        }

        Form frm_payment_terms;
        private void paymentTermsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_payment_terms == null)
            {
                frm_payment_terms = new frm_payment_terms();
                frm_payment_terms.MdiParent = this;
                frm_payment_terms.FormClosed += new FormClosedEventHandler(frm_payment_terms_FormClosed);
                frm_payment_terms.Show();
            }
            else
            {
                frm_payment_terms.Activate();
            }
        }

        void frm_payment_terms_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_payment_terms = null;
        }

        Form frm_payment_method;
        private void paymentMethodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_payment_method == null)
            {
                frm_payment_method = new frm_payment_method();
                frm_payment_method.MdiParent = this;
                frm_payment_method.FormClosed += new FormClosedEventHandler(frm_payment_method_FormClosed);
                frm_payment_method.Show();
            }
            else
            {
                frm_payment_method.Activate();
            }
        }

        void frm_payment_method_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_payment_method = null;
        }

        Form frm_coa;
        private void chartOfAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_coa == null)
            {
                frm_coa = new frm_coa();
                frm_coa.MdiParent = this;
                frm_coa.FormClosed += new FormClosedEventHandler(frm_coa_FormClosed);
                frm_coa.Show();
            }
            else
            {
                frm_coa.Activate();
            }
        }

        void frm_coa_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_coa = null;
        }

        private void toolStripButton_salesv1_Click(object sender, EventArgs e)
        {
            frm_sales_v1 frmv1 = new frm_sales_v1();
            frmv1.Show();
        }

        private void toolStripButton_purchasesV1_Click(object sender, EventArgs e)
        {
            frm_purchases_v1 frmPurV1 = new frm_purchases_v1();
            frmPurV1.Show();
        }

        Form frm_shortcuts;
        private void shortcutsKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_shortcuts == null)
            {
                frm_shortcuts = new Purchases.frm_shortcuts();
                frm_shortcuts.MdiParent = this;
                frm_shortcuts.FormClosed += new FormClosedEventHandler(Frm_shortcuts_FormClosed);
                frm_shortcuts.Show();
            }
            else
            {
                frm_shortcuts.Activate();
            }
        }

        private void Frm_shortcuts_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_shortcuts = null;
        }

        Form frm_ict;
        private void iCTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_ict == null)
            {
                frm_ict = new Products.ICT.frm_ict();
                frm_ict.MdiParent = this;
                frm_ict.FormClosed += new FormClosedEventHandler(Frm_ict_FormClosed);
                frm_ict.Show();
            }
            else
            {
                frm_ict.Activate();
            }

        }

        private void Frm_ict_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_ict = null;
        }

        Form frm_expenses;
        private void expensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_expenses == null)
            {
                frm_expenses = new pos.Expenses.frm_expenses();
                frm_expenses.MdiParent = this;
                frm_expenses.FormClosed += new FormClosedEventHandler(Frm_expenses_FormClosed);
                frm_expenses.Show();
            }
            else
            {
                frm_expenses.Activate();
            }
        }

        private void Frm_expenses_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_expenses = null;
        }

        Form frm_ict_receive;
        private void iCTRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_expenses == null)
            {
                frm_ict_receive = new pos.Products.ICT.frm_receive_ict();
                frm_ict_receive.MdiParent = this;
                frm_ict_receive.FormClosed += new FormClosedEventHandler(Frm_ict_receive_FormClosed);
                frm_ict_receive.Show();
            }
            else
            {
                frm_ict_receive.Activate();
            }
        }

        private void Frm_ict_receive_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_ict_receive = null;
        }
        Form frm_ict_release;
        private void iCTReleaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_ict_release == null)
            {
                frm_ict_release = new pos.Products.ICT.frm_release_ict();
                frm_ict_release.MdiParent = this;
                frm_ict_release.FormClosed += new FormClosedEventHandler(Frm_ict_release_FormClosed);
                frm_ict_release.Show();
            }
            else
            {
                frm_ict_release.Activate();
            }
        }

        private void Frm_ict_release_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_ict_release = null;
        }

        Form frm_branchSummary;
        private void branchSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_branchSummary == null)
            {
                frm_branchSummary = new pos.Reports.Dashboard.frm_branchWiseSummary();
                frm_branchSummary.MdiParent = this;
                frm_branchSummary.FormClosed += new FormClosedEventHandler(Frm_branchSummary_FormClosed);
                frm_branchSummary.Show();
            }
            else
            {
                frm_branchSummary.Activate();
            }
        }

        private void Frm_branchSummary_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_branchSummary = null;
        }

        Form frm_banks;
        private void banksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_banks == null)
            {
                frm_banks = new pos.Master.Banks.frm_banks();
                frm_banks.MdiParent = this;
                frm_banks.FormClosed += new FormClosedEventHandler(Frm_banks_FormClosed);
                frm_banks.Show();
            }
            else
            {
                frm_banks.Activate();
            }
        }

        private void Frm_banks_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_banks = null;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        Form Logs;
        private void applicationLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Logs == null)
            {
                Logs = new Master.Logs.Logs
                {
                    MdiParent = this
                };
                Logs.FormClosed += new FormClosedEventHandler(Logs_FormClosed);
                Logs.Show();
            }
            else
            {
                Logs.Activate();
            }
        }

        private void Logs_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logs = null;
        }

        Form AccountPayable;
        private void accountPayableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AccountPayable == null)
            {
                AccountPayable = new Accounts.Reports.AP.AccountPayable
                {
                    MdiParent = this
                };
                AccountPayable.FormClosed += new FormClosedEventHandler(AccountPayable_FormClosed);
                AccountPayable.Show();
            }
            else
            {
                AccountPayable.Activate();
            }
        }

        private void AccountPayable_FormClosed(object sender, FormClosedEventArgs e)
        {
            AccountPayable = null;
        }

        Form AccountReceivable;
        private void accountReceivableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AccountReceivable == null)
            {
                AccountReceivable = new Accounts.Reports.AR.AccountReceivable
                {
                    MdiParent = this
                };
                AccountReceivable.FormClosed += new FormClosedEventHandler(AccountReceivable_FormClosed);
                AccountReceivable.Show();
            }
            else
            {
                AccountReceivable.Activate();
            }
        }

        private void AccountReceivable_FormClosed(object sender, FormClosedEventArgs e)
        {
            AccountReceivable = null;
        }

        Form qohReport;
        private void quantityOnHandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (qohReport == null)
            {
                qohReport = new pos.Reports.Products.Inventory.FrmInventoryReport();
                qohReport.MdiParent = this;
                qohReport.FormClosed += new FormClosedEventHandler(QohReport_FormClosed);
                qohReport.Show();
            }
            else
            {
                qohReport.Activate();
            }
        }

        private void QohReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            qohReport = null;
        }

        private void اجماليالمخزونToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        Form LowStockReport;
        private void lowStockReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LowStockReport == null)
            {
                LowStockReport = new pos.Reports.Products.Inventory.FrmLowStockReport
                {
                    MdiParent = this
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

        Form ShowZatcaCSID;
        private void generateZATCACSIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ShowZatcaCSID == null)
            {
                ShowZatcaCSID = new pos.Master.Companies.zatca.ShowZatcaCSID()
                {
                    MdiParent = this
                };
                ShowZatcaCSID.FormClosed += new FormClosedEventHandler(AutoGenerateCSID_FormClosed);
                ShowZatcaCSID.Show();
            }
            else
            {
                ShowZatcaCSID.Activate();
            }
        }

        private void AutoGenerateCSID_FormClosed(object sender, FormClosedEventArgs e)
        {
            ShowZatcaCSID = null;
        }

        Form ZatcaInvoices;
        private void zatcaInvoicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ZatcaInvoices == null)
            {
                ZatcaInvoices = new pos.Sales.frm_zatca_invoices()
                {
                    MdiParent = this
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

        Form DebitNote;
        private void debitNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DebitNote == null)
            {
                DebitNote = new pos.Sales.frm_debitnote()
                {
                    MdiParent = this
                };
                DebitNote.FormClosed += new FormClosedEventHandler(DebitNote_FormClosed);
                DebitNote.Show();
            }
            else
            {
                DebitNote.Activate();
            }
        }

        private void DebitNote_FormClosed(object sender, FormClosedEventArgs e)
        {
            DebitNote = null;
        }


        Form _dashboardForm;
        private void toolStripButton_dashboard_Click(object sender, EventArgs e)
        {
            if (_dashboardForm == null)
            {
                _dashboardForm = new pos.Dashboard.frm_dashboard
                {
                    MdiParent = this,
                    WindowState = FormWindowState.Maximized
                };
                _dashboardForm.FormClosed += new FormClosedEventHandler(_dashboardForm_FormClosed);
                _dashboardForm.Show();
            }
            else
            {
                _dashboardForm.Activate();
            }

        }

 private void _dashboardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _dashboardForm = null;
        }

        Form AllBankReport;
        private void banksReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (AllBankReport == null)
            {
                AllBankReport = new frm_AllBankReport
                {
                    MdiParent = this
                };
                AllBankReport.FormClosed += new FormClosedEventHandler(AllBankReport_FormClosed);
                AllBankReport.Show();
            }
            else
            {
                AllBankReport.Activate();
            }
        }

        private void AllBankReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            AllBankReport = null;
        }

        private void byPaymentMethodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesByPaymentMethodForm salesByPaymentMethodForm = new SalesByPaymentMethodForm
            {
                MdiParent = this
            };
            salesByPaymentMethodForm.Show();

        }

        private void salesExpensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POS.Reports.Finance.frm_salesExpensesReport salesExpensesForm = new POS.Reports.Finance.frm_salesExpensesReport
            {
                MdiParent = this
            };
            salesExpensesForm.Show();
        }

        private void toolStripButtonNewPOS_Click(object sender, EventArgs e)
        {
            frm_pos_sale frmPOS = new frm_pos_sale();
            frmPOS.ShowDialog();
        }

        private void rolePermissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmRolePermissions())
                frm.ShowDialog(this);
        }

        private void userClaimsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmUserClaims())
                frm.ShowDialog(this);
        }

        private void permissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmPermissions())
                frm.ShowDialog(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var about = new pos.About.frm_about())
            {
                about.ShowDialog(this);
            }
        }

        private void toolStripButton_Help_Click(object sender, EventArgs e)
        {
            using (var about = new pos.About.frm_about())
            {
                about.ShowDialog(this);
            }
        }

        private void vATDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form vatDashboard = null;
            foreach (Form f in this.MdiChildren)
            {
                if (f is pos.Reports.Taxes.frm_VatDashboard)
                {
                    vatDashboard = f;
                    break;
                }
            }

            if (vatDashboard == null)
            {
                vatDashboard = new pos.Reports.Taxes.frm_VatDashboard();
                vatDashboard.MdiParent = this;
                vatDashboard.FormClosed += (s, args) => { };
                vatDashboard.Show();
            }
            else
            {
                vatDashboard.Activate();
            }
        }

        private void stockSupressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form stockSupression = null;
            foreach (Form f in this.MdiChildren)
            {
                if (f is pos.Products.Suppression.frm_stock_suppression)
                {
                    stockSupression = f;
                    break;
                }
            }

            if (stockSupression == null)
            {
                stockSupression = new pos.Products.Suppression.frm_stock_suppression();
               // stockSupression.MdiParent = this;
                stockSupression.FormClosed += (s, args) => { };
                stockSupression.ShowDialog();
            }
            else
            {
                stockSupression.Activate();
            }
        }

        private void expenseListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pos.Expenses.frm_expense_tracker expensesTraker = new pos.Expenses.frm_expense_tracker();
            expensesTraker.ShowDialog();
        }

        private void expenseDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var expenseDashboard = new pos.Expenses.frm_expense_dashboard();
            expenseDashboard.ShowDialog(this);
        }

        private void accountingDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var accountingDashboard = new frm_accounting_dashboard();
            accountingDashboard.ShowDialog(this);
        }

        private void customersDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void suppliersSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_suppliersSummary = new pos.frm_suppliers();
            frm_suppliersSummary.ShowDialog(this);
        }

       

        private void salesDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var salesDashboard = new pos.Sales.frm_sales_dashboard();
            salesDashboard.ShowDialog(this);
        }

        private void salesSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            salesDashboardToolStripMenuItem_Click(sender, e);
        }

        private void purchaseSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var purchaseDashboard = new pos.frm_purchase_dashboard();
            purchaseDashboard.ShowDialog(this);
        }

        private void salesSummaryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            salesDashboardToolStripMenuItem_Click(sender, e);
        }

        private void customersSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_customers = new pos.frm_customers();
            frm_customers.ShowDialog(this);
        }

        private void stockCheckAndAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_stockCheckAdjustment = new pos.frm_stock_check_adjustment();
            frm_stockCheckAdjustment.ShowDialog(this);
        }

        private void currenciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_currencies = new pos.frm_currencies();
            frm_currencies.ShowDialog(this);
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_settings = new pos.frm_settings();
            frm_settings.ShowDialog(this);
        }

        private void salesInvoiceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_salesInvoiceReport = new pos.Reports.Sales.frm_SalesInvoiceReport();
            frm_salesInvoiceReport.ShowDialog(this);
        }

        private void purchaseInvoiceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_purchaseInvoiceReport = new pos.Reports.Purchases.frm_PurchaseInvoiceReport();
            frm_purchaseInvoiceReport.ShowDialog(this);
        }

        private void journalVoucherListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_journalVoucherList = new pos.frm_journal_voucher_manager();
            frm_journalVoucherList.ShowDialog(this);
        }

        private void generalLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_generalLedger = new pos.Reports.Financial.frm_GeneralLedgerReport();
            frm_generalLedger.ShowDialog(this);
        }

        private void customerARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_customerAR = new pos.Reports.Accounts.FrmCustomerSubLedger();
            frm_customerAR.ShowDialog(this);
        }

        private void supplierAPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_supplierAP = new pos.Reports.Accounts.FrmSupplierSubLedger();
            frm_supplierAP.ShowDialog(this);
        }

        private void cashBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_cashBook = new pos.Reports.Accounts.FrmCashBook();
            frm_cashBook.ShowDialog(this);
        }

        private void incomeStatementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_incomeStatement = new pos.Reports.Financial.frm_ProfitAndLossReport();
            frm_incomeStatement.ShowDialog(this);
        }

        private void balanceSheetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var frm_balanceSheet = new pos.Reports.Financial.frm_BalanceSheetReport();
            frm_balanceSheet.ShowDialog(this);
        }

        private void financialPeriodManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_financialPeriodManagement = new pos.frm_financial_periods();
            frm_financialPeriodManagement.ShowDialog(this);
        }

        private void cashFlowManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_cashFlowManagement = new pos.Reports.Financial.frm_CashFlowStatementReport();
            frm_cashFlowManagement.ShowDialog(this);
        }

        private void bankReconciliationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm_bankReconciliation = new pos.Reports.Financial.frm_BankReconciliation();
            frm_bankReconciliation.ShowDialog(this);
        }

        // Tax Reporting Forms
        Form frm_salesTaxSummary;
        private void salesTaxSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_salesTaxSummary == null)
            {
                frm_salesTaxSummary = new pos.Reports.Taxes.frm_SalesTaxSummary
                {
                    MdiParent = this,
                    WindowState = FormWindowState.Maximized
                };
                frm_salesTaxSummary.FormClosed += new FormClosedEventHandler(frm_salesTaxSummary_FormClosed);
                frm_salesTaxSummary.Show();
            }
            else
            {
                frm_salesTaxSummary.Activate();
            }
        }

        private void frm_salesTaxSummary_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_salesTaxSummary = null;
        }

        Form frm_whtReport;
        private void withholdinTaxReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_whtReport == null)
            {
                frm_whtReport = new pos.Reports.Taxes.frm_WithholdingTaxReport
                {
                    MdiParent = this,
                    WindowState = FormWindowState.Maximized
                };
                frm_whtReport.FormClosed += new FormClosedEventHandler(frm_whtReport_FormClosed);
                frm_whtReport.Show();
            }
            else
            {
                frm_whtReport.Activate();
            }
        }

        private void frm_whtReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_whtReport = null;
        }

        Form frm_taxTrialBalance;
        private void taxTrialBalanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frm_taxTrialBalance == null)
            {
                frm_taxTrialBalance = new pos.Reports.Taxes.frm_TaxTrialBalance
                {
                    MdiParent = this,
                    WindowState = FormWindowState.Maximized
                };
                frm_taxTrialBalance.FormClosed += new FormClosedEventHandler(frm_taxTrialBalance_FormClosed);
                frm_taxTrialBalance.Show();
            }
            else
            {
                frm_taxTrialBalance.Activate();
            }
        }

        private void frm_taxTrialBalance_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm_taxTrialBalance = null;
        }

        private void setupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new pos.Accounting.CostCenter.frm_cost_center_setup();
            frm.ShowDialog(this);
        }

        private void hierarchyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new pos.Accounting.CostCenter.frm_cost_center_tree();
            frm.ShowDialog(this);
        }

        private void budgetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new pos.Accounting.CostCenter.frm_budget_setup();
            frm.ShowDialog(this);
        }

        private void allocationRulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new pos.Accounting.CostCenter.frm_allocation_rules();
            frm.ShowDialog(this);
        }

        private void departmentalPLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new pos.Accounting.CostCenter.frm_departmental_pl();
            frm.ShowDialog(this);
        }
    }
}
