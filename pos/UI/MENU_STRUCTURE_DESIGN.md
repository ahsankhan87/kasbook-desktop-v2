# Professional ERP Menu Hierarchy Design

## New Menu Structure (Max 3 Levels Deep)

### 1. DASHBOARD
- Dashboard Home
- Quick Stats / Widgets

### 2. SALES
- **New Transaction**
  - New Sale
  - Sales Return
  - Debit Note
- **View Transactions**
  - All Sales
  - Sales Summary
  - All Quotations/Estimates
- **Reports**
  - Daily Sales Report
  - Sales Invoice Report
  - Sales Report
  - Customer-wise Sales
  - Product-wise Sales Summary
  - Category-wise Sales Summary
  - By Payment Method
  - Sales Summary
- **ZATCA (E-Invoice)**
  - Generate ZATCA CSID
  - ZATCA Invoices

### 3. PURCHASES
- **New Transaction**
  - New Purchase
  - Purchase Return
- **View Transactions**
  - All Purchases
  - Purchase Summary
  - Purchase Orders
  - All Purchase Orders
- **Reports**
  - Purchase Report
  - Purchase Invoice Report

### 4. INVENTORY
- **Products/Services**
  - Products/Services (Master)
  - Brands
  - Categories
  - Product Groups
  - Locations
  - Units
  - Labels
  - Alternate Products
- **Warehouse Management**
  - Inventory Report (by Item)
  - Edit Item Data
  - Total Inventory
  - Low Stock Inventory
  - Quantity on Hand
  - Low Stock Report
  - Fast-Moving Items
  - Slow-Moving Items
- **Operations**
  - Product Adjustment
  - Product Location Transfer
  - Stock Check & Adjustment
  - Stock Suppression
- **Valuation**
  - Inventory Valuation
  - Inventory Valuation Settings

### 5. FINANCE
- **Accounting Dashboard**
  - Dashboard
- **Chart of Accounts**
  - Chart of Accounts (Master)
- **Transactions**
  - Journal Entries
  - Journal Voucher List
  - ICT Request
  - ICT Release
- **Reports**
  - General Ledger
  - Sub-Ledgers
	- Customer AR
	- Supplier AP
  - Trial Balance
  - Income Statement
  - Balance Sheet
  - Account Report
  - Group Report
  - Banks Report
  - Account Receivable
  - Account Payable
  - VAT Dashboard
  - Tax Trial Balance
- **Cash Management**
  - Banks (Master)
  - Cash Book
  - Bank Reconciliation
- **Cost Centers**
  - Setup
  - Hierarchy
  - Budget
  - Allocation Rules
  - Departmental P&L
  - Budget vs Actual Report
- **Financial Period Management**
  - Financial Years (Master)

### 6. CRM (Customers & Suppliers)
- **Customers**
  - Customers Summary
  - Create New Customer
- **Suppliers**
  - Suppliers Summary
  - Create New Supplier

### 7. EXPENSES
- Create New Expense
- Expense List
- Expense Dashboard

### 8. MASTERS (Configuration)
- **Company Setup**
  - Profile
  - Country/Origin
  - Branch
- **Transactions Setup**
  - Payment Terms
  - Payment Methods
  - Currencies
- **Taxes & Discounts**
  - Taxes/VAT
  - Discount Schemes
- **Fixed Assets**
  - Fixed Assets (Master)
- **Database**
  - DB Backup

### 9. SETTINGS
- **Configuration**
  - Settings
  - Import Data
- **Language**
  - English
  - Arabic
- **Security**
  - Role Permissions
  - Permissions
  - User Claims
  - Application Logs
  - Users (Master)
- **Session**
  - Logout
  - Exit

### 10. TOOLS
- (Reserved for future utilities)

### 11. HELP
- Help Documentation
- About

---

## Migration Mapping

### Items to Rename (Clarity)
```
newTransactionToolStripMenuItem2         → salesNewTransactionMenuItem
allTransactionToolStripMenuItem1         → salesViewTransactionMenuItem
newTransactionToolStripMenuItem          → purchasesNewTransactionMenuItem
allPurchasesToolStripMenuItem            → purchasesViewTransactionMenuItem
pOSToolStripMenuItem                     → posMenuItem
itemsToolStripMenuItem                   → inventoryMenuItem
```

### Items to Relocate
```
MOVED OUT of "Master" → More specific menus:
- Currency, Payment Terms, Payment Methods → MASTERS > Transactions Setup
- Branch, Country/Origin → MASTERS > Company Setup
- Financial Years → FINANCE > Financial Period Management
- Security items → SETTINGS > Security
- Language → SETTINGS > Language
- DB Backup → MASTERS > Database
- Profile → MASTERS > Company Setup
- Settings → SETTINGS > Configuration

CONSOLIDATED:
- All warehouse reports → INVENTORY > Warehouse Management
- All sales reports → SALES > Reports
- All finance reports → FINANCE > Reports
- All purchase reports → PURCHASES > Reports
```

### Permission Tags (Existing, to be preserved)
```
Sales_Create, Sales_View, Sales_Return, Reports_SalesView
Purchases_Create, Purchases_View, Purchases_Return, Reports_PurchasesView
Suppliers_View, Customers_View, Products_View, Inventory_View, Inventory_Edit
Finance_View, Finance_Report, Journal_View, Reports_InventoryView
Security_Permissions_View, Security_Permissions_Create, Expenses_View
Reports_FinanceView, Reports_AccountsView, Sales_Zatca_View, Sales_Zatca_Configure, Sales_DebitNote
```

---

## RTL/LTR Implementation Strategy
- Detect language at form load: `UsersModal.logged_in_lang`
- If "ar-SA" → Set `menuStrip1.RightToLeft = RightToLeft.Yes`
- If "en-US" → Set `menuStrip1.RightToLeft = RightToLeft.No`
- Apply same logic to status bar and toolbars

---

## Quick-Access Toolbar Icons (Compact, 24x24)
Position: Below Main Menu Bar
1. Dashboard
2. New Sale
3. New Purchase
4. Inventory
5. Finance
6. Customers
7. Suppliers
8. Reports

---

## Status: Design Phase Complete
Ready for Designer.cs Refactoring → Step 3
