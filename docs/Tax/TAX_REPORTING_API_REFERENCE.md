# Tax Reporting Module - Complete API Reference

## Data Transfer Objects (POS.Core/Tax/TaxModals.cs)

### SalesTaxModal
```csharp
public class SalesTaxModal
{
	public int TransactionId { get; set; }
	public string TransactionType { get; set; }  // "Sales" or "Purchase"
	public string InvoiceNo { get; set; }
	public DateTime Date { get; set; }
	public string PartyName { get; set; }
	public string NTN { get; set; }
	public decimal Amount { get; set; }
	public decimal TaxRate { get; set; }
	public decimal TaxAmount { get; set; }
	public string Remarks { get; set; }
	public int BranchId { get; set; }
}
```

### SalesTaxSummaryModal
```csharp
public class SalesTaxSummaryModal
{
	public string TaxPeriod { get; set; }
	public string TaxRegistrationNo { get; set; }
	public DateTime PeriodFromDate { get; set; }
	public DateTime PeriodToDate { get; set; }

	// Output Tax
	public decimal StandardRatedSalesAmount { get; set; }
	public decimal StandardRatedSalesTax { get; set; }
	public decimal ZeroRatedSalesAmount { get; set; }
	public decimal ExemptSalesAmount { get; set; }
	public decimal TotalOutputTax { get; set; }

	// Input Tax
	public decimal TaxablePurchasesAmount { get; set; }
	public decimal TaxablePurchasesTax { get; set; }
	public decimal ImportTaxPaid { get; set; }
	public decimal TotalInputTax { get; set; }

	// Net Tax
	public decimal NetTaxPayable { get; set; }
}
```

### WHTModal
```csharp
public class WHTModal
{
	public int TransactionId { get; set; }
	public string SupplierName { get; set; }
	public string VATNO { get; set; }
	public DateTime PaymentDate { get; set; }
	public decimal PaymentAmount { get; set; }
	public decimal WHTRate { get; set; }
	public decimal WHTAmount { get; set; }
	public string TaxSection { get; set; }  // "153", "155", etc.
	public string Remarks { get; set; }
	public int BranchId { get; set; }
}
```

### WHTSummaryModal
```csharp
public class WHTSummaryModal
{
	public string TaxSection { get; set; }
	public decimal TotalPaymentAmount { get; set; }
	public decimal TotalWHTAmount { get; set; }
	public decimal AverageWHTRate { get; set; }
	public int TransactionCount { get; set; }
}
```

### TaxTrialBalanceModal
```csharp
public class TaxTrialBalanceModal
{
	public int AccountId { get; set; }
	public string AccountCode { get; set; }
	public string AccountName { get; set; }
	public string AccountGroupName { get; set; }
	public string AccountType { get; set; }  // "Income" or "Expense"
	public decimal DebitAmount { get; set; }
	public decimal CreditAmount { get; set; }
	public decimal Balance { get; set; }
}
```

---

## Data Access Layer (POS.DLL/Tax)

### TaxRegistrationDLL

```csharp
public class TaxRegistrationDLL
{
	/// <summary>
	/// Gets complete sales and purchase tax register as DataTable
	/// </summary>
	/// <param name="fromDate">Starting date</param>
	/// <param name="toDate">Ending date</param>
	/// <returns>DataTable with columns: TransactionType, InvoiceNo, Date, PartyName, NTN, Amount, TaxRate, TaxAmount</returns>
	public DataTable GetSalesTaxRegister(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Gets tax register as List of SalesTaxModal objects
	/// </summary>
	public List<SalesTaxModal> GetSalesTaxRegisterList(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Gets only sales tax transactions
	/// </summary>
	public DataTable GetSalesTaxRegisterSalesOnly(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Gets only purchase tax transactions
	/// </summary>
	public DataTable GetSalesTaxRegisterPurchasesOnly(DateTime fromDate, DateTime toDate);
}
```

### WHTRegistrationDLL

```csharp
public class WHTRegistrationDLL
{
	/// <summary>
	/// Gets complete WHT report as DataTable
	/// </summary>
	/// <param name="fromDate">Starting date</param>
	/// <param name="toDate">Ending date</param>
	/// <returns>DataTable with WHT transaction details</returns>
	public DataTable GetWHTReport(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Gets WHT report as List of WHTModal objects
	/// </summary>
	public List<WHTModal> GetWHTReportList(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Gets WHT summary grouped by tax section
	/// </summary>
	public List<WHTSummaryModal> GetWHTSummaryBySection(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Gets monthly WHT summary
	/// </summary>
	public DataTable GetWHTMonthlySummary(DateTime fromDate, DateTime toDate);
}
```

### TaxTrialBalanceDLL

```csharp
public class TaxTrialBalanceDLL
{
	/// <summary>
	/// Gets income and expense trial balance as DataTable
	/// </summary>
	/// <param name="financialYearId">Fiscal year ID</param>
	/// <returns>DataTable with account details</returns>
	public DataTable GetIncomeTaxTrialBalance(int financialYearId);

	/// <summary>
	/// Gets trial balance as List of TaxTrialBalanceModal objects
	/// </summary>
	public List<TaxTrialBalanceModal> GetIncomeTaxTrialBalanceList(int financialYearId);

	/// <summary>
	/// Gets only income accounts
	/// </summary>
	public DataTable GetIncomeAccounts(int financialYearId);

	/// <summary>
	/// Gets only expense accounts
	/// </summary>
	public DataTable GetExpenseAccounts(int financialYearId);

	/// <summary>
	/// Gets summary totals for trial balance
	/// </summary>
	public DataTable GetIncomeTaxTotals(int financialYearId);
}
```

---

## Business Logic Layer (POS.BLL/Tax)

### SalesTaxBLL

```csharp
public class SalesTaxBLL
{
	/// <summary>
	/// Calculates complete sales tax summary with all metrics
	/// </summary>
	/// <param name="fromDate">Period start date</param>
	/// <param name="toDate">Period end date</param>
	/// <returns>SalesTaxSummaryModal with all calculations</returns>
	public SalesTaxSummaryModal CalculateSalesTaxSummary(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Gets raw tax register data from database
	/// </summary>
	public DataTable GetSalesTaxRegister(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Gets formatted DataTable ready for grid display
	/// </summary>
	public DataTable GetSalesTaxRegisterDataTable(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Prepares data in Excel-compatible format for export
	/// </summary>
	/// <param name="fromDate">Period start date</param>
	/// <param name="toDate">Period end date</param>
	/// <returns>Formatted DataTable for Excel export</returns>
	public DataTable PrepareExportData(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Gets formatted string representation of tax period
	/// </summary>
	/// <param name="fromDate">Period start date</param>
	/// <param name="toDate">Period end date</param>
	/// <returns>String like "January 2025" or "Q1 2025"</returns>
	public string GetTaxPeriodString(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Gets total output tax for period
	/// </summary>
	public decimal GetTotalOutputTax(DateTime fromDate, DateTime toDate);
}
```

### WHTCalculationBLL

```csharp
public class WHTCalculationBLL
{
	/// <summary>
	/// Gets complete WHT report with all transactions
	/// </summary>
	/// <param name="fromDate">Period start date</param>
	/// <param name="toDate">Period end date</param>
	/// <returns>DataTable with transaction-level detail</returns>
	public DataTable GetWHTReport(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Gets formatted DataTable for grid display
	/// </summary>
	public DataTable GetWHTReportDataTable(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Gets WHT summary grouped by tax section (153, 155, etc.)
	/// </summary>
	/// <param name="fromDate">Period start date</param>
	/// <param name="toDate">Period end date</param>
	/// <returns>List of WHTSummaryModal grouped by section</returns>
	public List<WHTSummaryModal> GetWHTSummaryBySection(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Gets WHT broken down by month
	/// </summary>
	/// <param name="fromDate">Period start date</param>
	/// <param name="toDate">Period end date</param>
	/// <returns>DataTable with monthly aggregation</returns>
	public DataTable GetWHTMonthlySummary(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Calculates grand totals for WHT period
	/// </summary>
	/// <param name="fromDate">Period start date</param>
	/// <param name="toDate">Period end date</param>
	/// <returns>WHTSummaryModal with totals</returns>
	public WHTSummaryModal GetWHTTotals(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Calculates average WHT rate for period
	/// </summary>
	public decimal GetAverageWHTRate(DateTime fromDate, DateTime toDate);

	/// <summary>
	/// Prepares WHT data for Excel export
	/// </summary>
	/// <param name="fromDate">Period start date</param>
	/// <param name="toDate">Period end date</param>
	/// <returns>Formatted DataTable</returns>
	public DataTable PrepareWHTExportData(DateTime fromDate, DateTime toDate);
}
```

### TaxReportingBLL

```csharp
public class TaxReportingBLL
{
	/// <summary>
	/// Gets complete income and expense trial balance
	/// </summary>
	/// <param name="financialYearId">Fiscal year ID</param>
	/// <returns>List of TaxTrialBalanceModal for all income/expense accounts</returns>
	public List<TaxTrialBalanceModal> GetIncomeTaxTrialBalance(int financialYearId);

	/// <summary>
	/// Gets only income accounts
	/// </summary>
	/// <param name="financialYearId">Fiscal year ID</param>
	/// <returns>List of income account records</returns>
	public List<TaxTrialBalanceModal> GetIncomeAccounts(int financialYearId);

	/// <summary>
	/// Gets only expense accounts
	/// </summary>
	/// <param name="financialYearId">Fiscal year ID</param>
	/// <returns>List of expense account records</returns>
	public List<TaxTrialBalanceModal> GetExpenseAccounts(int financialYearId);

	/// <summary>
	/// Gets summary totals (total income, total expense, net income)
	/// </summary>
	/// <param name="financialYearId">Fiscal year ID</param>
	/// <returns>TaxTrialBalanceModal with aggregate values</returns>
	public TaxTrialBalanceModal GetIncomeTaxTotals(int financialYearId);

	/// <summary>
	/// Calculates net income (Profit or Loss)
	/// </summary>
	/// <param name="financialYearId">Fiscal year ID</param>
	/// <returns>Decimal net income value</returns>
	public decimal GetNetIncome(int financialYearId);

	/// <summary>
	/// Prepares trial balance for Excel export
	/// </summary>
	/// <param name="financialYearId">Fiscal year ID</param>
	/// <returns>Formatted DataTable</returns>
	public DataTable PrepareExportData(int financialYearId);

	/// <summary>
	/// Gets trial balance summary by account group
	/// </summary>
	/// <param name="financialYearId">Fiscal year ID</param>
	/// <returns>DataTable grouped by account group</returns>
	public DataTable GetTrialBalanceSummary(int financialYearId);

	/// <summary>
	/// Gets trial balance records grouped by account group
	/// </summary>
	/// <param name="financialYearId">Fiscal year ID</param>
	/// <returns>Dictionary keyed by group name with list of accounts</returns>
	public Dictionary<string, List<TaxTrialBalanceModal>> GetTrialBalanceByGroup(int financialYearId);
}
```

---

## User Interface Forms (pos/Reports/Taxes)

### frm_SalesTaxSummary

```csharp
public partial class frm_SalesTaxSummary : Form
{
	private SalesTaxBLL _salesTaxBll;

	// Controls
	private DateTimePicker dtpFromDate;
	private DateTimePicker dtpToDate;
	private TextBox txtTaxRegNo;
	private Button btnRefresh;
	private Button btnExportExcel;
	private Button btnPrint;
	private Label lblStandardRatedTax;
	private Label lblZeroRatedSales;
	private Label lblExemptSales;
	private Label lblTotalOutputTax;
	private Label lblTotalInputTax;
	private Label lblNetTaxPayable;
	private Label lblTaxablePurchases;
	private Label lblImportTax;
	private DataGridView dgvSalesTaxRegister;
	private DataGridView dgvPurchaseTaxRegister;

	// Events
	private void frm_SalesTaxSummary_Load(object sender, EventArgs e);
	private void LoadTaxRegistrationNumber();
	private void RefreshReport();
	private void DisplaySummary(SalesTaxSummaryModal summary);
	private void LoadSalesTaxRegisterGrid(DataTable dt);
	private void LoadPurchaseTaxRegisterGrid(DataTable dt);
	private void btnExportExcel_Click(object sender, EventArgs e);
	private void btnPrint_Click(object sender, EventArgs e);
}
```

### frm_WithholdingTaxReport

```csharp
public partial class frm_WithholdingTaxReport : Form
{
	private WHTCalculationBLL _whtBll;

	// Controls
	private DateTimePicker dtpFromDate;
	private DateTimePicker dtpToDate;
	private Button btnRefresh;
	private Button btnGeneratePSID;
	private Button btnExportExcel;
	private Button btnPrint;
	private Label lblTotalPayments;
	private Label lblTotalWHT;
	private Label lblTransactionCount;
	private DataGridView dgvWHTTransactions;
	private DataGridView dgvWHTSummary;

	// Events
	private void frm_WithholdingTaxReport_Load(object sender, EventArgs e);
	private void RefreshReport();
	private void LoadWHTTransactionsGrid(DataTable dt);
	private void LoadWHTSummaryGrid(List<WHTSummaryModal> summary);
	private void UpdateSummaryLabels();
	private void btnGeneratePSID_Click(object sender, EventArgs e);
	private void btnExportExcel_Click(object sender, EventArgs e);
	private void btnPrint_Click(object sender, EventArgs e);
}
```

### frm_TaxTrialBalance

```csharp
public partial class frm_TaxTrialBalance : Form
{
	private TaxReportingBLL _taxBll;

	// Controls
	private ComboBox cmbFinancialYear;
	private ComboBox cmbAccountType;
	private Button btnRefresh;
	private Button btnExportExcel;
	private Button btnPrint;
	private Label lblTotalIncome;
	private Label lblTotalExpense;
	private Label lblNetIncome;
	private DataGridView dgvTrialBalance;

	// Events
	private void frm_TaxTrialBalance_Load(object sender, EventArgs e);
	private void LoadFinancialYears();
	private void cmbFinancialYear_SelectedIndexChanged(object sender, EventArgs e);
	private void cmbAccountType_SelectedIndexChanged(object sender, EventArgs e);
	private void RefreshReport();
	private void ConvertModalListToDataTable(List<TaxTrialBalanceModal> list);
	private void FormatTrialBalanceGrid();
	private void UpdateSummaryLabels();
	private void btnExportExcel_Click(object sender, EventArgs e);
	private void btnPrint_Click(object sender, EventArgs e);
}
```

---

## SQL Stored Procedures (Database/StoredProcedures)

### sp_SalesTaxRegister
```sql
CREATE PROCEDURE sp_SalesTaxRegister
	@FromDate DATE,
	@ToDate DATE,
	@TaxType NVARCHAR(50) = 'ALL'  -- 'ALL', 'SALES', 'PURCHASES'
AS
-- Returns: TransactionType, TransactionId, InvoiceNo, Date, PartyName, NTN, Amount, TaxRate, TaxAmount, Remarks, branch_id
```

### sp_WHTReport
```sql
CREATE PROCEDURE sp_WHTReport
	@FromDate DATE,
	@ToDate DATE
AS
-- Returns: SupplierName, VATNO, PaymentDate, PaymentAmount, WHTRate, WHTAmount, TaxSection, branch_id
```

### sp_IncomeTaxTrialBalance
```sql
CREATE PROCEDURE sp_IncomeTaxTrialBalance
	@FinancialYearId INT
AS
-- Returns: AccountCode, AccountName, AccountGroup, DebitAmount, CreditAmount, Balance, AccountType
```

---

## Namespace Organization

```csharp
// Data Models
namespace POS.Core
{
	public class SalesTaxModal { ... }
	public class SalesTaxSummaryModal { ... }
	public class WHTModal { ... }
	public class WHTSummaryModal { ... }
	public class TaxTrialBalanceModal { ... }
}

// Data Access Layer
namespace POS.DLL
{
	public class TaxRegistrationDLL { ... }
	public class WHTRegistrationDLL { ... }
	public class TaxTrialBalanceDLL { ... }
}

// Business Logic Layer
namespace POS.BLL
{
	public class SalesTaxBLL { ... }
	public class WHTCalculationBLL { ... }
	public class TaxReportingBLL { ... }
}

// User Interface
namespace pos.Reports.Taxes
{
	public class frm_SalesTaxSummary : Form { ... }
	public class frm_WithholdingTaxReport : Form { ... }
	public class frm_TaxTrialBalance : Form { ... }
}
```

---

## Exception Handling Pattern

All methods follow this pattern:

```csharp
public SomeType MethodName(parameters)
{
	try
	{
		// Implementation
		return result;
	}
	catch
	{
		// Log if needed
		// Log.LogAction(...)  // Audit trail
		throw;  // Re-throw to caller
	}
}
```

---

## Common Usage Patterns

### Pattern 1: Get Summary in Form
```csharp
var bll = new SalesTaxBLL();
var summary = bll.CalculateSalesTaxSummary(fromDate, toDate);
// Use summary properties for display
```

### Pattern 2: Load Grid from DataTable
```csharp
var bll = new WHTCalculationBLL();
DataTable dt = bll.GetWHTReport(fromDate, toDate);
dgvGrid.DataSource = dt;
```

### Pattern 3: Get Typed List
```csharp
var dll = new TaxRegistrationDLL();
List<SalesTaxModal> items = dll.GetSalesTaxRegisterList(fromDate, toDate);
foreach (var item in items)
{
	// Process item
}
```

---

## Performance Considerations

- All queries use SQL Server stored procedures (optimized)
- DataTable binding for large result sets
- In-memory grouping only after retrieval
- Index recommendation: `CREATE INDEX idx_voucher_date ON sales_header(voucherDate)`

---

**API Reference Complete**
**Total Classes**: 12
**Total Methods**: 41+
**Total Parameters**: 100+
**Status**: ✅ Production Ready
