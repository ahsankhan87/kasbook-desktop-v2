# Budget Module Documentation

## Overview
The Budget Module provides comprehensive budgeting capabilities for the accounting system, including budget creation, approval workflow, variance analysis, and real-time budget monitoring.

## Database Schema

### Tables Created

#### 1. acc_budget_headers
Main budget header table storing budget metadata.

**Columns:**
- `budget_id` (INT, PK, IDENTITY): Unique budget identifier
- `financial_year_id` (INT, FK): Links to fiscal_years table
- `budget_version` (VARCHAR(20)): Version identifier (e.g., "V1", "V2", "Revised")
- `cc_id` (INT, FK, NULL): Optional cost center assignment
- `budget_name` (NVARCHAR(100)): Descriptive budget name
- `status` (VARCHAR(20)): Budget status - 'Draft', 'Approved', or 'Active'
- `approved_by` (INT, FK, NULL): User who approved the budget
- `approved_at` (DATETIME, NULL): Approval timestamp
- `notes` (NVARCHAR(500), NULL): Additional notes
- `created_by` (INT, FK): User who created the budget
- `created_at` (DATETIME): Creation timestamp

**Constraints:**
- CHECK constraint on status (Draft, Approved, Active)
- Unique index on (financial_year_id, budget_version, cc_id)

#### 2. acc_budget_lines
Monthly budget allocations per account.

**Columns:**
- `line_id` (INT, PK, IDENTITY): Unique line identifier
- `budget_id` (INT, FK): Links to acc_budget_headers
- `acc_id` (INT, FK): Links to acc_accounts
- `jan` through `dec` (DECIMAL(18,2)): Monthly budget amounts
- `annual_total` (DECIMAL(18,2), COMPUTED): Auto-calculated sum of all months

**Constraints:**
- Cascade delete when budget header is deleted
- Unique index on (budget_id, acc_id)

#### 3. acc_budget_variance_notes
Explanatory notes for budget variances.

**Columns:**
- `note_id` (INT, PK, IDENTITY): Unique note identifier
- `budget_id` (INT, FK): Links to acc_budget_headers
- `acc_id` (INT, FK): Links to acc_accounts
- `period_month` (INT): Month number (1-12)
- `period_year` (INT): Year
- `variance_note` (NVARCHAR(500)): Explanation text
- `added_by` (INT, FK): User who added the note
- `added_at` (DATETIME): Note timestamp

## Stored Procedures

### 1. sp_BudgetVsActual
Compares budget vs actual with comprehensive variance analysis.

**Parameters:**
- `@BudgetId` (INT): Budget to analyze
- `@FromDate` (DATE): Start date for actuals
- `@ToDate` (DATE): End date for actuals
- `@CCId` (INT, NULL): Optional cost center filter

**Returns:**
- Account code and name
- Account type
- Annual budget amount
- Year-to-date budget (prorated to current month)
- Year-to-date actual
- YTD variance (amount and percentage)
- Current month budget and actual
- Monthly variance
- Full year forecast (based on YTD run rate)

### 2. sp_BudgetMonthlyDetail
Returns 12-month breakdown for a specific account.

**Parameters:**
- `@BudgetId` (INT): Budget identifier
- `@AccId` (INT): Account identifier

**Returns:**
- Month number and name
- Budget amount
- Actual amount
- Variance
- Cumulative budget
- Cumulative actual

### 3. sp_CopyBudgetFromActuals
Copies prior year actuals as new budget baseline.

**Parameters:**
- `@SourceYearId` (INT): Fiscal year to copy from
- `@TargetBudgetId` (INT): Budget to populate
- `@GrowthPct` (DECIMAL(5,2)): Optional inflation/growth percentage

**Process:**
1. Retrieves actuals from source fiscal year
2. Applies growth percentage (if provided)
3. Deletes existing lines in target budget
4. Inserts new budget lines with calculated amounts

### 4. sp_BudgetSeasonalSpread
Distributes annual budget across months using percentages.

**Parameters:**
- `@BudgetId` (INT): Budget identifier
- `@AccId` (INT): Account identifier
- `@AnnualAmount` (DECIMAL(18,2)): Total annual budget
- `@Percentages` (MonthlyPercentagesType TVP): Table-valued parameter with monthly percentages

**Validation:**
- Ensures percentages sum to 100%
- Calculates monthly amounts based on annual total

### 5. sp_BudgetSummaryKPIs
Returns high-level budget performance metrics.

**Parameters:**
- `@BudgetId` (INT): Budget identifier
- `@AsOfDate` (DATE): Date for calculations

**Returns:**
- Total income budget and actual
- Total expense budget and actual
- Net profit budget and actual
- Overall achievement percentage

## C# Classes

### POS.Core.BudgetModal.cs
Contains modal classes for budget data:
- `BudgetHeaderModal`
- `BudgetLineModal`
- `BudgetVarianceNoteModal`
- `BudgetVsActualModal`
- `BudgetMonthlyDetailModal`
- `BudgetSummaryKPIsModal`
- `MonthlyPercentageModal`
- `BudgetExceededCheckModal`

### POS.DLL.BudgetDLL.cs
Data access layer with methods:
- CRUD operations for headers and lines
- Bulk save using SqlBulkCopy
- Budget approval and activation
- Stored procedure execution
- Variance note management

### POS.BLL.BudgetBLL.cs
Business logic layer with methods:
- `SaveBudgetHeader()` - Create or update budget
- `SaveBudgetLines()` - Bulk save budget lines
- `ApproveBudget()` - Approve budget (with role check)
- `ActivateBudget()` - Set as active budget
- `CopyFromLastYear()` - Copy from actuals
- `CheckBudgetExceeded()` - Real-time budget check for journal entries
- `GetBudgetVsActual()` - Variance analysis
- `GetBudgetMonthlyDetail()` - Monthly breakdown
- `ApplySeasonalSpread()` - Seasonal distribution
- `GetBudgetSummaryKPIs()` - Performance metrics
- Helper methods for percentages and authorization

## Usage Examples

### Creating a New Budget

```csharp
var budgetBll = new BudgetBLL();

// Create header
var header = new BudgetHeaderModal
{
	financial_year_id = 5,
	budget_version = "V1",
	budget_name = "FY 2024 Operating Budget",
	status = "Draft",
	created_by = UsersModal.logged_in_userid
};

int budgetId = budgetBll.SaveBudgetHeader(header);

// Create budget lines
var lines = new List<BudgetLineModal>
{
	new BudgetLineModal 
	{ 
		acc_id = 100, 
		jan = 10000, feb = 10000, mar = 10000,
		apr = 10000, may = 10000, jun = 10000,
		jul = 10000, aug = 10000, sep = 10000,
		oct = 10000, nov = 10000, dec = 10000
	}
};

budgetBll.SaveBudgetLines(budgetId, lines);
```

### Copying from Last Year with Growth

```csharp
// Copy last year's actuals with 5% growth
budgetBll.CopyFromLastYear(
	sourceYearId: 4,
	targetBudgetId: budgetId,
	growthPct: 5.0m
);
```

### Checking Budget Before Journal Entry

```csharp
var check = budgetBll.CheckBudgetExceeded(
	accId: 200,
	newAmount: 5000m,
	date: DateTime.Today
);

if (check.IsExceeded)
{
	MessageBox.Show(check.Message, "Budget Warning");
}
```

### Approving and Activating Budget

```csharp
// Approve (requires CFO/Manager role)
budgetBll.ApproveBudget(budgetId, UsersModal.logged_in_userid);

// Activate (sets as active, deactivates others)
budgetBll.ActivateBudget(budgetId, UsersModal.logged_in_userid);
```

### Budget Variance Analysis

```csharp
DataTable variance = budgetBll.GetBudgetVsActual(
	budgetId: budgetId,
	fromDate: new DateTime(2024, 1, 1),
	toDate: DateTime.Today,
	ccId: null
);

// Bind to grid for display
gridBudgetVariance.DataSource = variance;
```

### Seasonal Budget Spread

```csharp
// Q4 heavy pattern: 15% per month in Q4, rest spread evenly
var percentages = new List<MonthlyPercentageModal>
{
	new MonthlyPercentageModal { MonthNo = 1, Percentage = 5.00m },
	new MonthlyPercentageModal { MonthNo = 2, Percentage = 5.00m },
	new MonthlyPercentageModal { MonthNo = 3, Percentage = 5.00m },
	new MonthlyPercentageModal { MonthNo = 4, Percentage = 5.00m },
	new MonthlyPercentageModal { MonthNo = 5, Percentage = 5.00m },
	new MonthlyPercentageModal { MonthNo = 6, Percentage = 5.00m },
	new MonthlyPercentageModal { MonthNo = 7, Percentage = 5.00m },
	new MonthlyPercentageModal { MonthNo = 8, Percentage = 5.00m },
	new MonthlyPercentageModal { MonthNo = 9, Percentage = 5.00m },
	new MonthlyPercentageModal { MonthNo = 10, Percentage = 15.00m },
	new MonthlyPercentageModal { MonthNo = 11, Percentage = 15.00m },
	new MonthlyPercentageModal { MonthNo = 12, Percentage = 15.00m }
};

budgetBll.ApplySeasonalSpread(budgetId, accId: 300, annualAmount: 120000m, percentages);
```

## Security

### Role-Based Access
- **Budget Approval**: Restricted to CFO, Manager, Administrator roles
- **Budget Activation**: Restricted to CFO, Manager, Administrator roles
- All actions are logged via `Log.LogAction()`

### Status Workflow
1. **Draft** - Editable, can be deleted
2. **Approved** - Locked for editing, approved by authorized user
3. **Active** - Currently in use for variance tracking, only one active budget per fiscal year/cost center

## Integration Points

### Journal Entry Posting
Call `CheckBudgetExceeded()` before posting to warn users of budget overruns:

```csharp
// In journal voucher form
var budgetCheck = budgetBll.CheckBudgetExceeded(
	accId: selectedAccountId,
	newAmount: entryAmount,
	date: entryDate,
	ccId: selectedCostCenter
);

if (budgetCheck.IsExceeded)
{
	var result = MessageBox.Show(
		budgetCheck.Message + "\n\nDo you want to proceed?",
		"Budget Exceeded",
		MessageBoxButtons.YesNo,
		MessageBoxIcon.Warning
	);

	if (result == DialogResult.No)
		return;
}
```

### Dashboard Integration
Use `GetBudgetSummaryKPIs()` to display budget performance on accounting dashboard.

## Installation

### 1. Run SQL Script
Execute `POS.DLL/Accounts/BudgetProcedures.sql` against your database to create:
- Tables: acc_budget_headers, acc_budget_lines, acc_budget_variance_notes
- Stored procedures: sp_BudgetVsActual, sp_BudgetMonthlyDetail, etc.
- Table type: MonthlyPercentagesType

### 2. Build Solution
The following files are already added:
- `POS.Core/Accounts/BudgetModal.cs`
- `POS.DLL/Accounts/BudgetDLL.cs`
- `POS.BLL/Accounts/BudgetBLL.cs`

Rebuild the solution to compile the new classes.

### 3. Create UI Forms (Future)
Budget UI forms will be created separately to:
- Create and edit budgets
- Import from actuals
- View variance reports
- Manage approval workflow

## Best Practices

1. **Budget Versions**: Use version numbers (V1, V2) for budget revisions
2. **Approval Workflow**: Always approve budgets before activating
3. **Variance Notes**: Document significant variances for audit trail
4. **Cost Centers**: Use cost center-specific budgets for departmental tracking
5. **Growth Factors**: Use conservative growth percentages when copying from actuals
6. **Seasonal Patterns**: Apply seasonal spreads for accounts with predictable patterns
7. **Real-time Checks**: Integrate budget checks into transaction entry forms

## Future Enhancements

- Budget templates (pre-defined account lists)
- Multi-year budget comparison
- Budget rollover automation
- Excel import/export
- Budget amendment workflow
- Graphical variance charts
- Automated alerts for threshold breaches
- Budget allocation by department/project

## Support

For issues or questions, refer to the existing accounting module documentation or contact the development team.
