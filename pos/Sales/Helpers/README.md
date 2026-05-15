# Sales Helper Classes - Refactoring Documentation

## Overview

This folder contains helper classes extracted from `frm_sales.cs` to improve code maintainability, testability, and organization. The original `frm_sales.cs` file remains unchanged to ensure compatibility with existing customer installations.

## Helper Classes

### 1. SalesStylingHelper.cs

**Purpose:** Centralizes all UI styling logic for the sales form.

**Key Methods:**
- `StyleSalesForm()` - Applies professional theme styling to the entire sales form
- `StyleTotalField()` - Styles summary total fields in the footer
- `StyleSecondaryField()` - Styles secondary footer fields (received/change)
- `StyleCostField()` - Styles cost-info read-only fields
- `StyleDropdownGrid()` - Styles popup DataGridView dropdowns
- `StyleFooterCard()` - Styles footer TableLayoutPanel cards
- `StyleFooterLabel()` - Styles footer labels
- `ApplySalesLabelForeColor()` - Recursively applies foreground colors to labels

**Usage Example:**
```csharp
// Instead of calling StyleSalesForm() with all inline code
SalesStylingHelper.StyleSalesForm(
    this, 
    panel_header, panel_footer, panel_grid,
    lbl_title, SalesToolStrip, grid_sales,
    groupBox2, groupBox5, groupBox6,
    txt_total_amount, txt_sub_total, txt_sub_total_2,
    // ... other controls
);
```

### 2. SalesCalculationHelper.cs

**Purpose:** Contains all calculation logic for sales transactions.

**Key Methods:**
- `GetSubTotalAmount()` - Calculates sub-total (qty × unit_price)
- `GetTotalCostAmount()` - Calculates total cost and cost with VAT
- `GetTotalAmount()` - Calculates total amount
- `CalculateNetAmount()` - Calculates net amount after tax and discount
- `GetTotalTax()` - Sums tax values from all rows
- `GetTotalDiscount()` - Sums discount values from all rows
- `GetTotalQty()` - Sums quantity values from all rows
- `ValidateCreditLimit()` - Validates customer credit limit

**Usage Example:**
```csharp
// Calculate totals
double subTotal = SalesCalculationHelper.GetSubTotalAmount(grid_sales);
var costData = SalesCalculationHelper.GetTotalCostAmount(grid_sales);
double totalCost = costData.Item1;
double totalCostWithVat = costData.Item2;

// Validate credit limit
var validation = SalesCalculationHelper.ValidateCreditLimit(
    sale_type, customer_credit_limit, customerBalance, netAmount);
if (!validation.Item1)
{
    // Credit limit exceeded by validation.Item2
}
```

### 3. SalesGridHelper.cs

**Purpose:** Handles all DataGridView operations for the sales grid.

**Key Methods:**
- `LoadProductToGrid()` - Loads or updates a product in the grid
- `ConfigureNumericColumns()` - Configures numeric column formatting
- `UpdateSerialNumbers()` - Updates row serial numbers
- `RemoveEmptyTrailingRows()` - Removes empty rows from grid end
- `EnsureEmptyRowForEntry()` - Ensures an empty row exists for data entry
- `ClearGrid()` - Clears all rows from the grid
- `GetCurrentProductId()` - Gets product ID from selected row
- `ValidateRow()` - Validates required cells in a row

**Usage Example:**
```csharp
// Load product into grid
int rowIndex = SalesGridHelper.LoadProductToGrid(grid_sales, productRow);

// Configure grid columns
SalesGridHelper.ConfigureNumericColumns(grid_sales);

// Ensure empty row for new entry
SalesGridHelper.EnsureEmptyRowForEntry(grid_sales);
```

### 4. SalesDropdownHelper.cs

**Purpose:** Manages dropdown list population and company settings retrieval.

**Key Methods:**
- `PopulateCustomersDropdown()` - Populates customer combo box
- `PopulateEmployeesDropdown()` - Populates employee combo box
- `PopulatePaymentTermsDropdown()` - Populates payment terms combo box
- `PopulatePaymentMethodsDropdown()` - Populates payment methods combo box
- `PopulateSaleTypeDropdown()` - Populates sale type dropdown
- `PopulateInvoiceSubtypeDropdown()` - Populates invoice subtype dropdown (ZATCA)
- `GetCompanyAccountIds()` - Retrieves company account configuration
- `FillLocationsGridCombo()` - Fills location dropdown in grid row

**Usage Example:**
```csharp
// Populate dropdowns on form load
SalesDropdownHelper.PopulateCustomersDropdown(cmb_customers);
SalesDropdownHelper.PopulateEmployeesDropdown(cmb_employees);
SalesDropdownHelper.PopulatePaymentTermsDropdown(cmb_payment_terms);
SalesDropdownHelper.PopulatePaymentMethodsDropdown(cmb_payment_method);
SalesDropdownHelper.PopulateSaleTypeDropdown(cmb_sale_type);
SalesDropdownHelper.PopulateInvoiceSubtypeDropdown(cmb_invoice_subtype_code, UsersModal.useZatcaEInvoice);

// Get company accounts
CompanyAccounts accounts = SalesDropdownHelper.GetCompanyAccountIds();
cash_account_id = accounts.CashAccountId;
sales_account_id = accounts.SalesAccountId;
// etc.
```

### 5. CompanyAccounts.cs

**Purpose:** Data class to hold company account configuration.

**Properties:**
- `CashAccountId`
- `SalesAccountId`
- `ReceivableAccountId`
- `TaxAccountId`
- `SalesDiscountAccId`
- `InventoryAccId`
- `PurchasesAccId`
- `CashSalesAmountLimit`
- `AllowCreditSales`

## Benefits of This Refactoring

1. **Separation of Concerns:** Each helper class has a single responsibility
2. **Testability:** Helper methods can be unit tested independently
3. **Maintainability:** Changes to styling, calculations, or grid operations are localized
4. **Reusability:** Helper classes can be used by other forms (e.g., frm_sales_return, frm_pos_sale)
5. **Readability:** Code is organized by functionality rather than being monolithic

## Important Notes

- **Original Code Preserved:** The `frm_sales.cs` file remains unchanged to maintain compatibility with existing customer installations
- **No Logic Changes:** All helper methods replicate the exact logic from the original file
- **Namespace:** All helpers are in the `pos.Sales.Helpers` namespace
- **Static Classes:** All helper classes are static for easy access without instantiation

## Future Improvements

When ready to update customer installations, consider:
1. Replacing inline code in `frm_sales.cs` with calls to these helpers
2. Adding unit tests for each helper method
3. Creating similar helpers for other sales-related forms
4. Extracting validation logic into a separate `SalesValidationHelper`
5. Creating interfaces for better testability (e.g., `ISalesCalculator`)
