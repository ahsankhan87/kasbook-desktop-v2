# Budget Module - Quick Reference Guide

## 🚀 Quick Start

### 1. Database Setup (5 minutes)
```sql
-- Execute this file in SQL Server Management Studio:
USE [your_database_name]
GO
-- Run: POS.DLL\Accounts\BudgetProcedures.sql
```

### 2. Verify Installation
```sql
-- Check tables
SELECT * FROM acc_budget_headers;
SELECT * FROM acc_budget_lines;
SELECT * FROM acc_budget_variance_notes;

-- Check stored procedures
EXEC sp_BudgetVsActual @BudgetId = 1, @FromDate = '2024-01-01', @ToDate = '2024-12-31', @CCId = NULL;
```

### 3. First Budget (C# Code)
```csharp
using POS.BLL;
using POS.Core;

// Create budget
var budgetBll = new BudgetBLL();

var header = new BudgetHeaderModal
{
	financial_year_id = BudgetHelper.GetCurrentFiscalYearId() ?? 1,
	budget_version = "V1",
	budget_name = "2024 Operating Budget",
	status = "Draft",
	created_by = UsersModal.logged_in_userid
};

int budgetId = budgetBll.SaveBudgetHeader(header);

// Copy from last year with 3% growth
budgetBll.CopyFromLastYear(sourceYearId: 4, targetBudgetId: budgetId, growthPct: 3.0m);

// Approve and activate
if (BudgetHelper.CanApproveBudgets())
{
	budgetBll.ApproveBudget(budgetId, UsersModal.logged_in_userid);
	budgetBll.ActivateBudget(budgetId, UsersModal.logged_in_userid);
}
```

## 📊 Common Operations

### Check Budget Before Journal Entry
```csharp
var budgetBll = new BudgetBLL();
var check = budgetBll.CheckBudgetExceeded(
	accId: 200,
	newAmount: 5000m,
	date: DateTime.Today
);

if (check.IsExceeded)
{
	MessageBox.Show(
		check.Message + "\n\nProceed anyway?",
		"Budget Warning",
		MessageBoxButtons.YesNo,
		MessageBoxIcon.Warning
	);
}
```

### Get Variance Report
```csharp
var budgetBll = new BudgetBLL();
int? budgetId = budgetBll.GetActiveBudgetForPeriod(DateTime.Today);

if (budgetId.HasValue)
{
	DataTable variance = budgetBll.GetBudgetVsActual(
		budgetId.Value,
		new DateTime(2024, 1, 1),
		DateTime.Today
	);

	gridVariance.DataSource = variance;
}
```

### Dashboard KPIs
```csharp
var budgetBll = new BudgetBLL();
int? budgetId = budgetBll.GetActiveBudgetForPeriod(DateTime.Today);

if (budgetId.HasValue)
{
	var kpis = budgetBll.GetBudgetSummaryKPIs(budgetId.Value, DateTime.Today);

	lblIncomeBudget.Text = kpis.TotalIncomeBudget.ToString("N2");
	lblIncomeActual.Text = kpis.TotalIncomeActual.ToString("N2");
	lblExpenseBudget.Text = kpis.TotalExpenseBudget.ToString("N2");
	lblExpenseActual.Text = kpis.TotalExpenseActual.ToString("N2");
	lblNetProfitBudget.Text = kpis.NetProfitBudget.ToString("N2");
	lblNetProfitActual.Text = kpis.NetProfitActual.ToString("N2");
	lblAchievement.Text = kpis.OverallAchievementPct.ToString("N1") + "%";
}
```

### Seasonal Distribution
```csharp
var budgetBll = new BudgetBLL();

// Use predefined patterns
var equalSpread = BudgetHelper.CreateEqualSpread(); // 8.33% each month
var quarterlySpread = BudgetHelper.CreateQuarterlySpread(); // Q-based
var retailSpread = BudgetHelper.CreateRetailSeasonalSpread(); // Holiday-heavy

// Apply to account
budgetBll.ApplySeasonalSpread(
	budgetId: 1,
	accId: 300,
	annualAmount: 120000m,
	percentages: retailSpread
);
```

## 🔐 Security & Roles

### Role Check
```csharp
if (BudgetHelper.CanApproveBudgets())
{
	btnApprove.Enabled = true;
	btnActivate.Enabled = true;
}
else
{
	btnApprove.Enabled = false;
	btnActivate.Enabled = false;
}
```

### Authorized Roles
- Administrator
- Admin
- Owner
- CFO
- Manager

## 📋 Status Workflow

```
Draft → Approved → Active
  ↓        ↓         ↓
Edit    Locked    One per FY
Delete  Can Activate  In Use
```

**Rules:**
- Only Draft budgets can be edited/deleted
- Approval requires authorized role
- Only one Active budget per fiscal year + cost center
- Activation auto-deactivates other Active budgets

## 🛠️ Helper Functions

### Validation
```csharp
var lines = GetBudgetLinesFromGrid();
var errors = BudgetHelper.ValidateBudgetLines(lines);

if (errors.Count > 0)
{
	MessageBox.Show(string.Join("\n", errors), "Validation Errors");
	return;
}
```

### Month Operations
```csharp
// Get month name
string monthName = BudgetHelper.GetMonthName(3); // "March"

// Get amount for specific month
decimal marchAmount = BudgetHelper.GetMonthAmount(budgetLine, 3);

// Set amount for specific month
BudgetHelper.SetMonthAmount(budgetLine, 3, 15000m);
```

### Distribution
```csharp
// Distribute annual amount equally
var budgetLine = BudgetHelper.DistributeAnnualEqually(
	accId: 200,
	annualAmount: 120000m
);
// Each month gets 10,000 (last month adjusted for rounding)
```

### Variance Status
```csharp
string status = BudgetHelper.GetVarianceStatus(
	variance: 5000m,
	accountType: "Expense"
);
// Returns: "Unfavorable" (expense is over budget)

decimal variancePct = BudgetHelper.CalculateVariancePct(
	actual: 105000m,
	budget: 100000m
);
// Returns: 5.0 (5% over budget)
```

## 📝 Monthly Detail Report
```csharp
var budgetBll = new BudgetBLL();
DataTable monthlyDetail = budgetBll.GetBudgetMonthlyDetail(
	budgetId: 1,
	accId: 200
);

// Columns: MonthNo, MonthName, BudgetAmount, ActualAmount, 
//          Variance, CumulativeBudget, CumulativeActual

gridMonthly.DataSource = monthlyDetail;
```

## 💾 Bulk Save Budget Lines
```csharp
var budgetBll = new BudgetBLL();
var lines = new List<BudgetLineModal>();

// Add budget lines from grid or other source
foreach (DataGridViewRow row in gridBudget.Rows)
{
	if (row.IsNewRow) continue;

	lines.Add(new BudgetLineModal
	{
		acc_id = Convert.ToInt32(row.Cells["acc_id"].Value),
		jan = Convert.ToDecimal(row.Cells["jan"].Value),
		feb = Convert.ToDecimal(row.Cells["feb"].Value),
		// ... other months
	});
}

// Validate before save
var errors = BudgetHelper.ValidateBudgetLines(lines);
if (errors.Count > 0)
{
	MessageBox.Show(string.Join("\n", errors));
	return;
}

// Save (uses SqlBulkCopy for performance)
budgetBll.SaveBudgetLines(budgetId, lines);
```

## 🔍 Common Queries

### Get All Budgets
```csharp
var budgetBll = new BudgetBLL();
DataTable allBudgets = budgetBll.GetAllBudgetHeaders();
gridBudgets.DataSource = allBudgets;
```

### Filter by Fiscal Year
```csharp
DataTable yearBudgets = budgetBll.GetBudgetsByFiscalYear(fiscalYearId: 5);
```

### Filter by Status
```csharp
DataTable draftBudgets = budgetBll.GetBudgetsByStatus("Draft");
DataTable approvedBudgets = budgetBll.GetBudgetsByStatus("Approved");
DataTable activeBudgets = budgetBll.GetBudgetsByStatus("Active");
```

### Get Budget Lines
```csharp
DataTable lines = budgetBll.GetBudgetLines(budgetId: 1);
// Convert to list if needed
List<BudgetLineModal> lineList = BudgetHelper.DataTableToBudgetLines(lines);
```

## 🎨 UI Integration Examples

### ComboBox - Budget Selection
```csharp
var budgetBll = new BudgetBLL();
DataTable budgets = budgetBll.GetAllBudgetHeaders();

cmbBudget.DisplayMember = "budget_name";
cmbBudget.ValueMember = "budget_id";
cmbBudget.DataSource = budgets;
```

### Status Display
```csharp
// In DataGridView CellFormatting event
if (e.ColumnIndex == colStatus.Index)
{
	string status = e.Value?.ToString();
	string display = BudgetHelper.GetStatusDisplay(status);
	e.Value = display;

	// Set color (for WinForms, convert hex to Color)
	string colorHex = BudgetHelper.GetStatusColorHex(status);
	// Use colorHex in UI styling
}
```

### Variance Notes
```csharp
var budgetBll = new BudgetBLL();

// Add note
budgetBll.AddVarianceNote(
	budgetId: 1,
	accId: 200,
	periodMonth: 3,
	periodYear: 2024,
	note: "Higher than expected due to special project",
	userId: UsersModal.logged_in_userid
);

// View notes
DataTable notes = budgetBll.GetVarianceNotes(budgetId: 1, accId: 200);
gridNotes.DataSource = notes;
```

## ⚡ Performance Tips

1. **Bulk Operations**: Use `SaveBudgetLines()` instead of individual inserts
2. **Active Budget Cache**: Cache the active budget ID to avoid repeated lookups
3. **Stored Procedures**: All complex queries use optimized stored procedures
4. **Indexes**: All foreign keys are indexed automatically

## 🐛 Troubleshooting

### Budget Not Found
```csharp
int? budgetId = budgetBll.GetActiveBudgetForPeriod(DateTime.Today);
if (!budgetId.HasValue)
{
	MessageBox.Show("No active budget found for current period");
	return;
}
```

### Approval Denied
```csharp
try
{
	budgetBll.ApproveBudget(budgetId, userId);
}
catch (UnauthorizedAccessException ex)
{
	MessageBox.Show("You do not have permission to approve budgets.\n" + 
					"Required role: CFO, Manager, or Administrator");
}
```

### Duplicate Budget Version
```csharp
// Check if version exists before creating
DataTable existing = budgetBll.GetBudgetsByFiscalYear(fiscalYearId);
var versions = existing.AsEnumerable()
	.Where(r => r.Field<string>("budget_version") == newVersion)
	.ToList();

if (versions.Count > 0)
{
	MessageBox.Show($"Budget version {newVersion} already exists for this fiscal year");
	return;
}
```

## 📚 Files Reference

**Database:**
- `POS.DLL/Accounts/BudgetProcedures.sql` - Schema & procedures

**C# Classes:**
- `POS.Core/Accounts/BudgetModal.cs` - Data models
- `POS.DLL/Accounts/BudgetDLL.cs` - Data access
- `POS.BLL/Accounts/BudgetBLL.cs` - Business logic
- `POS.BLL/Accounts/BudgetHelper.cs` - Utility functions

**Documentation:**
- `POS.DLL/Accounts/BUDGET_MODULE_README.md` - Full guide
- `BUDGET_MODULE_SUMMARY.md` - Implementation summary

## 🎯 Next Steps

1. ✅ Run `BudgetProcedures.sql` on your database
2. ✅ Rebuild solution (already done - build successful)
3. ⬜ Create budget management UI forms
4. ⬜ Integrate budget check into journal voucher form
5. ⬜ Add budget widgets to accounting dashboard
6. ⬜ Test with sample data

---

**Need Help?** Refer to `BUDGET_MODULE_README.md` for detailed documentation and examples.
