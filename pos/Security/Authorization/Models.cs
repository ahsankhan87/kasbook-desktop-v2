using System;
using System.Collections.Generic;

namespace pos.Security.Authorization
{
    // System roles; align these with your DB/user profile
    public enum SystemRole
    {
        Owner,
        Administrator,
        Manager,
        User,
        Cashier,
        SalesRep,
        Accountant,
        Viewer
    }

    // Canonical permission keys (flat list – easy to store and query)
    public static class Permissions
    {
        // Products module permissions (normalized to lowercase)
        public const string Products_Create = "products.create";
        public const string Products_Delete = "products.delete";
        public const string Products_View = "products.view";
        public const string Products_Edit = "products.edit";
        public const string Products_Import = "products.import";
        public const string Products_Export = "products.export";
        public const string Products_BulkPriceUpdate = "products.bulkpriceupdate";
        public const string Products_warehousesManage = "products.warehousesmanage";
        public const string Inventory_View = "inventory.view";
        public const string Inventory_Edit = "inventory.edit";

        // Sales module permissions (normalized to lowercase)
        public const string Sales_Create = "sales.create";
        public const string Sales_Edit = "sales.edit";
        public const string Sales_Delete = "sales.delete";
        public const string Sales_View = "sales.view";
        public const string Sales_Print = "sales.print";
        public const string Sales_Return = "sales.return";
        public const string Sales_DebitNote = "sales.debitnote";
        public const string Sales_ictTransactions_View = "sales.icttransactions.view";
        public const string Sales_ictTransactions_Request = "sales.icttransactions.request";
        public const string Sales_ictTransactions_Release = "sales.icttransactions.release";

        // Quotes module permissions (normalized to lowercase)
        public const string Quotes_Create = "quotes.create";
        public const string Quotes_Edit = "quotes.edit";
        public const string Quotes_Delete = "quotes.delete";
        public const string Quotes_View = "quotes.view";
        public const string Quotes_Print = "quotes.print";

        // Invoices module permissions (normalized to lowercase)
        public const string Invoices_Create = "invoices.create";
        public const string Invoices_Edit = "invoices.edit";
        public const string Invoices_Delete = "invoices.delete";
        public const string Invoices_View = "invoices.view";
        public const string Invoices_Print = "invoices.print";
        public const string Invoices_SendEmail = "invoices.sendemail";

        // Credit Notes module permissions (normalized to lowercase)
        public const string CreditNotes_Create = "creditnotes.create";
        public const string CreditNotes_Edit = "creditnotes.edit";
        public const string CreditNotes_Delete = "creditnotes.delete";
        public const string CreditNotes_View = "creditnotes.view";

        // Debit Notes module permissions (normalized to lowercase)
        public const string DebitNotes_Create = "debitnotes.create";
        public const string DebitNotes_Edit = "debitnotes.edit";
        public const string DebitNotes_Delete = "debitnotes.delete";
        public const string DebitNotes_View = "debitnotes.view";
        public const string DebitNotes_Print = "debitnotes.print";

        // Purchases module permissions (normalized to lowercase)
        public const string Purchases_Create = "purchases.create";
        public const string Purchases_Edit = "purchases.edit";
        public const string Purchases_Delete = "purchases.delete";
        public const string Purchases_View = "purchases.view";
        public const string Purchases_Print = "purchases.print";
        public const string Purchases_Return = "purchases.return";

        // Suppliers module permissions (normalized to lowercase)
        public const string Suppliers_Create = "suppliers.create";
        public const string Suppliers_Delete = "suppliers.delete";
        public const string Suppliers_View = "suppliers.view";
        public const string Suppliers_Edit = "suppliers.edit";

        // Purchase Orders module permissions (normalized to lowercase)
        public const string PurchaseOrders_Create = "purchaseorders.create";
        public const string PurchaseOrders_Edit = "purchaseorders.edit";
        public const string PurchaseOrders_Delete = "purchaseorders.delete";
        public const string PurchaseOrders_View = "purchaseorders.view";
        public const string PurchaseOrders_Print = "purchaseorders.print";

        // Stock Adjustments module permissions (normalized to lowercase)
        public const string StockAdjustments_Create = "stockadjustments.create";
        public const string StockAdjustments_Edit = "stockadjustments.edit";
        public const string StockAdjustments_Delete = "stockadjustments.delete";
        public const string StockAdjustments_View = "stockadjustments.view";
        public const string StockAdjustments_Print = "stockadjustments.print";

        // Transfers module permissions (normalized to lowercase)
        public const string Transfers_Create = "transfers.create";
        public const string Transfers_Edit = "transfers.edit";
        public const string Transfers_Delete = "transfers.delete";
        public const string Transfers_View = "transfers.view";
        public const string Transfers_Print = "transfers.print";

        // Bank module permissions (normalized to lowercase)
        public const string Bank_Create = "bank.create";
        public const string Bank_Edit = "bank.edit";
        public const string Bank_Delete = "bank.delete";
        public const string Bank_View = "bank.view";
        public const string Bank_Print = "bank.print";
        public const string Bank_LedgerPayment = "bank.ledgerpayment";
        public const string Bank_LedgerView = "bank.ledgerview";
        public const string Bank_LedgerPrint = "bank.ledgerprint";

        // Expenses module permissions (normalized to lowercase)
        public const string Expenses_Create = "expenses.create";
        public const string Expenses_Edit = "expenses.edit";
        public const string Expenses_Delete = "expenses.delete";
        public const string Expenses_View = "expenses.view";
        public const string Expenses_Print = "expenses.print";

        // Reports module permissions (normalized to lowercase)
        public const string Reports_SalesView = "reports.sales.view";
        public const string Reports_PurchasesView = "reports.purchases.view";
        public const string Reports_InventoryView = "reports.inventory.view";
        public const string Reports_FinanceView = "reports.finance.view";
        public const string Reports_AccountsView = "reports.accounts.view";
        public const string Reports_ProfitLossView = "reports.profitloss.view";


        // ZATCA (FATOORA) specific permissions
        public const string Sales_Zatca_Enable = "sales.zatca.enable";
        public const string Sales_Zatca_Configure = "sales.zatca.configure";
        public const string Sales_Zatca_Generate = "sales.zatca.generate";
        public const string Sales_Zatca_Transmit = "sales.zatca.transmit";
        public const string Sales_Zatca_Sign = "sales.zatca.sign";
        public const string Sales_Zatca_Report = "sales.zatca.report";
        public const string Sales_Zatca_Clear = "sales.zatca.clear";
        public const string Sales_Zatca_Qr_Show = "sales.zatca.qr.show";
        public const string Sales_Zatca_DownloadUBL = "sales.zatca.ubl.download";

        // Customers module permissions (normalized to lowercase)
        public const string Customers_Create = "customers.create";
        public const string Customers_Delete = "customers.delete";
        public const string Customers_View = "customers.view";
        public const string Customers_Edit = "customers.edit";
        public const string Customers_Import = "customers.import";
        public const string Customers_Export = "customers.export";
        public const string Customers_LedgerPayment = "customers.ledgerpayment";
        public const string Customers_LedgerView = "customers.ledgerview";
        public const string Customers_LedgerPrint = "customers.ledgerprint";

        // Finance module permissions (normalized to lowercase)
        public const string Finance_View = "finance.view";
        public const string Finance_Report = "finance.report";
        public const string Finance_Edit = "finance.edit";
        public const string Finance_Create = "finance.create";

       
        // Security: Permissions management (normalized to lowercase)
        public const string Security_Permissions_View   = "security.permissions.view";
        public const string Security_Permissions_Create = "security.permissions.create";
        public const string Security_Permissions_Edit   = "security.permissions.edit";
        public const string Security_Permissions_Delete = "security.permissions.delete";

        // Security: User management (normalized to lowercase)
        public const string Security_Users_View   = "security.users.view";
        public const string Security_Users_Create = "security.users.create";
        public const string Security_Users_Edit   = "security.users.edit";
        public const string Security_Users_Delete = "security.users.delete";

        // Profile management (normalized to lowercase)
        public const string Profile_View   = "profile.view";
        public const string Profile_Edit   = "profile.edit";

        // Branches management (normalized to lowercase)
        public const string Branches_View   = "branches.view";
        public const string Branches_Create = "branches.create";
        public const string Branches_Edit   = "branches.edit";
        public const string Branches_Delete = "branches.delete";

        // Financial years management (normalized to lowercase)
        public const string FinancialYears_View   = "financialyears.view";
        public const string FinancialYears_Create = "financialyears.create";
        public const string FinancialYears_Edit   = "financialyears.edit";
        public const string FinancialYears_Delete = "financialyears.delete";

        // Accounts 
        public const string Account_View = "account.view";
        public const string Account_Create = "account.create";
        public const string Account_Edit = "account.edit";
        public const string Account_Delete = "account.delete";

        // Groups 
        public const string Group_View = "group.view";
        public const string Group_Create = "group.create";
        public const string Group_Edit = "group.edit";
        public const string Group_Delete = "group.delete";

        // Journals
        public const string Journal_View = "journal.view";
        public const string Journal_Create = "journal.create";
        public const string Journal_Edit = "journal.edit";
        public const string Journal_Delete = "journal.delete";


    }

    public sealed class UserIdentity
    {
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public string Username { get; set; }
        public SystemRole Role { get; set; }
        public HashSet<string> Claims { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // per-user overrides
    }

    public sealed class RoleDefinition
    {
        public SystemRole Role { get; set; }
        public HashSet<string> GrantedPermissions { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }
}