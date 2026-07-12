# Cost Center Module — .csproj Registration

Since your .csproj files are currently locked/in use, **manually add the following `<Compile>` entries** to register the new classes.

## For POS.Core\POS.Core.csproj

Add this line inside the `<ItemGroup>` that contains the other `<Compile>` elements:

```xml
<Compile Include="Accounts\CostCenterModels.cs" />
```

**Location context:** Look for the ItemGroup with other Accounts model files like `AccountsModal.cs`, `JournalsModal.cs`, `ExpenseModal.cs`, etc. Add the line in that group.

---

## For POS.DLL\POS.DLL.csproj

Add this line inside the `<ItemGroup>` that contains other `<Compile>` elements:

```xml
<Compile Include="Accounts\CostCenterDLL.cs" />
```

**Location context:** Look for the ItemGroup with other Accounts DLL files like `AccountsDLL.cs`, `JournalsDLL.cs`, `AccountingDAL.cs`, etc. Add the line in that group.

---

## For POS.BLL\POS.BLL.csproj

Add this line inside the `<ItemGroup>` that contains other `<Compile>` elements:

```xml
<Compile Include="Accounts\CostCenterBLL.cs" />
```

**Location context:** Look for the ItemGroup with other Accounts BLL files like `AccountsBLL.cs`, `JournalsBLL.cs`, `FinancialPeriodBLL.cs`, etc. Add the line in that group.

---

## For POS.DLL\POS.DLL.csproj (SQL Script Registration)

Add this line inside the `<ItemGroup>` that contains SQL files:

```xml
<Content Include="Accounts\CostCenterProcedures.sql" />
```

**Location context:** Look for the ItemGroup with other SQL files like `AccountingDashboardProcedures.sql`, `BankReconciliationProcedures.sql`, `FinancialPeriodProcedures.sql`. Add the line in that group.

---

## After Adding Entries

1. **Close the solution** in Visual Studio (or reload it).
2. **Unload and reload** the affected projects if IDE doesn't recognize changes immediately.
3. **Build** the solution to verify compilation:
   - Clean → Build → Rebuild

---

## Files Created

| File | Project | Purpose |
|------|---------|---------|
| `POS.Core/Accounts/CostCenterModels.cs` | POS.Core | Model classes: CostCenterModel, AccountBudget, AllocationResult, BudgetAlertModel, BudgetCheckResult |
| `POS.DLL/Accounts/CostCenterDLL.cs` | POS.DLL | Data access layer with CRUD, budget queries, and allocation orchestration |
| `POS.BLL/Accounts/CostCenterBLL.cs` | POS.BLL | Business logic layer with validation and workflow coordination |
| `POS.DLL/Accounts/CostCenterProcedures.sql` | POS.DLL | Database tables, indexes, TVP, and stored procedures (already created) |

---

## Integration Points

### From JournalsBLL (Post Journal Entry)

Before posting a journal entry with a cost center tag, call:

```csharp
var bll = new CostCenterBLL();

if (voucherLine.CostCenterId.HasValue && voucherLine.CostCenterId > 0)
{
	var budgetCheck = bll.CheckBudgetBeforePosting(
		voucherLine.CostCenterId.Value,
		voucherLine.AccountId,
		Math.Abs(voucherLine.Amount),
		voucherLine.EntryDate
	);

	if (budgetCheck.IsOverBudget && !userConfirmedOverBudget)
	{
		// Show warning/dialog to user
		throw new OperationCanceledException($"Budget exceeded: {budgetCheck.Message}");
	}
}
```

### From Journal Entry Form (UI Warning Panel)

When the user selects a cost center in a JV form, call:

```csharp
var bll = new CostCenterBLL();
var alerts = bll.GetBudgetAlert(selectedCcId, DateTime.Today);

if (alerts.Count > 0)
{
	// Populate warning panel with alerts
	// Show account names, budget, actual, overspend
	warningPanel.Visible = true;
	warningPanel.DataSource = alerts;
}
```

### Monthly Maintenance: Run Allocation

Call from a scheduled task or admin form:

```csharp
var bll = new CostCenterBLL();
var result = bll.RunExpenseAllocation(
	DateTime.Now,  // Period
	UsersModal.logged_in_userid
	// allocationRuleId: null (all rules)
);

MessageBox.Show(result.Message);
if (result.Success)
	MessageBox.Show($"Voucher: {result.VoucherNo}, Total: {result.TotalAllocated:N2}");
```

### Reporting: Departmental P&L

```csharp
var bll = new CostCenterBLL();
var reportData = bll.GetDepartmentalPL(
	DateTime.Parse("2025-01-01"),
	DateTime.Parse("2025-01-31"),
	null  // All cost centers
);

// Bind to grid or report
dataGridView.DataSource = reportData;
```

---

## Notes

- **Transaction Handling:** SetBudget and RunExpenseAllocation use SQL transactions to ensure atomicity.
- **Circular Reference Validation:** Stored as a recursive query in the DLL's HasCircularReference method.
- **Residual Rounding:** The SQL procedure `sp_AutoAllocateExpenses` uses ROUND(..., 2) and the last item absorbs remainder.
- **No Headcount Table Yet:** HEADCOUNT allocation method is planned but not yet implemented (uses FIXED_PCT as fallback).
- **Cost Center Dropdown:** Used in JV entry forms; includes both code and name for clarity.
- **Budget Alerts:** Real-time check; can be called on each form load to show warnings.

