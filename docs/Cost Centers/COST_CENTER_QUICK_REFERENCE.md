# Cost Center Module - Quick Reference

## Forms at a Glance

| Form | Purpose | Key Controls | Launch |
|------|---------|--------------|--------|
| `frm_cost_center_setup` | CRUD cost centers | Grid + entry form | Menu → Accounting → Cost Centers → Setup |
| `frm_cost_center_tree` | Hierarchy browser | TreeView + context menu | Menu → Accounting → Cost Centers → Hierarchy |
| `frm_departmental_pl` | P&L by department | Date filter + cost center checklist + report grid | Menu → Accounting → Reports → Departmental P&L |
| `frm_budget_setup` | Monthly budgets | Cost center + year + editable grid | Menu → Accounting → Cost Centers → Budget |
| `frm_allocation_rules` | Allocation rules | Rule grid + auto-allocate runner | Menu → Accounting → Cost Centers → Allocation Rules |

---

## BLL Methods (API Facade)

```csharp
// CRUD
int SaveCostCenter(CostCenterModel model, int userId);
CostCenterModel GetCostCenterById(int ccId);
DataTable GetCostCenterDropdown(string ccType = null);
DataTable GetCostCenterTree(bool includeBalances = true, DateTime? fromDate = null, DateTime? toDate = null);

// Budgets
void SetBudget(int ccId, int yearId, List<AccountBudget> budgets, int userId);
List<BudgetAlertModel> GetBudgetAlert(int ccId, DateTime currentDate);
BudgetCheckResult CheckBudgetBeforePosting(int ccId, int accountId, decimal amount, DateTime date);

// Reporting
DataTable GetDepartmentalPL(DateTime fromDate, DateTime toDate, List<int> ccIds = null);
DataTable GetBudgetVsActual(int ccId, int yearId, int? month = null);
DataTable GetCostCenterSummary(int ccId, DateTime? fromDate = null, DateTime? toDate = null);

// Allocation
AllocationResult RunExpenseAllocation(DateTime period, int userId, int? allocationRuleId = null);
```

---

## Journal Entry Integration Example

```csharp
// Populate cost center dropdown
var bll = new CostCenterBLL();
DataTable dt = bll.GetCostCenterDropdown();
cmbCostCenter.DataSource = dt;
cmbCostCenter.DisplayMember = "display_text";
cmbCostCenter.ValueMember = "id";

// Before posting, check budget
if (cmbCostCenter.SelectedValue != null && int.TryParse(cmbCostCenter.SelectedValue.ToString(), out int ccId))
{
	var result = bll.CheckBudgetBeforePosting(ccId, accountId, debitAmount, entryDate);
	if (result.IsOverBudget)
	{
		MessageBox.Show($"Budget alert: {result.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		// Decide: block or warn and continue
	}
}
```

---

## Database Dependencies

**Must pre-exist** (created via `CostCenterProcedures.sql`):
- `dbo.acc_cost_centers` table
- `dbo.acc_cost_center_budgets` table
- `dbo.acc_cost_center_allocations` table
- `dbo.acc_entries.cost_center_id` column
- `dbo.CostCenterIdListType` (table-valued parameter)
- `sp_GetCostCenterTree` procedure
- `sp_DepartmentalPL` procedure
- `sp_CostCenterBudgetVsActual` procedure
- `sp_AutoAllocateExpenses` procedure
- `sp_CostCenterSummary` procedure

**Used by**:
- `CostCenterDLL.cs` methods call these procedures
- `CostCenterBLL.cs` wraps and validates
- WinForms UI binds to results

---

## Common Tasks

### Create Cost Center
```csharp
var bll = new CostCenterBLL();
var model = new CostCenterModel
{
	CcCode = "SALES-01",
	CcName = "Sales Department",
	CcType = "Department",
	ParentCcId = 1, // Set to null for root
	ManagerId = 42,
	MonthlyBudget = 50000m,
	StartDate = DateTime.Today,
	IsActive = true
};
int ccId = bll.SaveCostCenter(model, UsersModal.logged_in_userid);
```

### Set Monthly Budgets
```csharp
var bll = new CostCenterBLL();
var budgets = new List<AccountBudget>
{
	new AccountBudget
	{
		AccountId = 501, // Expense account
		JanBudget = 5000m, FebBudget = 5000m, MarBudget = 5000m,
		AprBudget = 5000m, MayBudget = 5000m, JunBudget = 5000m,
		JulBudget = 5000m, AugBudget = 5000m, SepBudget = 5000m,
		OctBudget = 5000m, NovBudget = 5000m, DecBudget = 5000m
	}
};
bll.SetBudget(1, 1, budgets, UsersModal.logged_in_userid);
```

### Get P&L by Department
```csharp
var bll = new CostCenterBLL();
DataTable dt = bll.GetDepartmentalPL(
	new DateTime(2024, 1, 1),
	new DateTime(2024, 12, 31),
	new List<int> { 1, 2, 3 } // Cost center IDs, or null for all
);
dgvReport.DataSource = dt; // Bind to grid
```

### Run Auto-Allocation
```csharp
var bll = new CostCenterBLL();
var result = bll.RunExpenseAllocation(
	new DateTime(2024, 1, 1),
	UsersModal.logged_in_userid,
	allocationRuleId: null // null = all rules, or specify one
);
if (result.Success)
	MessageBox.Show($"Allocated {result.TotalAllocated:N2}. Voucher: {result.VoucherNo}");
else
	MessageBox.Show($"Error: {result.Message}");
```

---

## Error Handling

All BLL methods throw `InvalidOperationException` for business rule violations:
- Cost center code not unique
- Parent cost center does not exist
- Circular hierarchy detected
- Budget amounts negative
- No budget defined for account

Catch and display to user:
```csharp
try
{
	bll.SaveCostCenter(model, userId);
}
catch (InvalidOperationException ex)
{
	MessageBox.Show($"Validation Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
catch (Exception ex)
{
	MessageBox.Show($"System Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
```

---

## Performance Notes

- **GetCostCenterTree**: Uses recursive CTE; performant for hierarchies up to 10 levels deep
- **GetDepartmentalPL**: Pivots `acc_entries` by cost center; may need index on `(cost_center_id, entry_date, account_id)` for large datasets
- **SetBudget**: Deletes existing then batch-inserts; use transaction management
- **RunExpenseAllocation**: Long-running (300s timeout); show busy indicator (`BusyScope.Show(this, "...")`)

---

## Testing Checklist

- [ ] Create a root cost center (no parent)
- [ ] Create child cost centers (parent = root)
- [ ] Verify circular hierarchy is rejected
- [ ] Load cost center dropdown in journal form
- [ ] Create expense accounts and set budgets
- [ ] Post journal entries with cost center
- [ ] Verify budget alert when over-budget
- [ ] Run departmental P&L report
- [ ] Create allocation rule and run auto-allocation
- [ ] Verify allocation entries appear in general ledger
- [ ] Export departmental P&L to CSV
- [ ] Deactivate a cost center and verify it no longer appears in active dropdowns

---

## Troubleshooting

**Forms won't open**:
- Ensure `.Designer.cs` files paired with code-behind
- Check `.csproj` file includes all form files
- Rebuild solution

**Dropdown empty**:
- Verify cost centers exist in `acc_cost_centers` table
- Check `is_active = 1` filter in query
- Check user permissions (if security role is applied)

**Budget alert not firing**:
- Verify budget exists for account in selected cost center and fiscal year
- Check entry date falls within month of budget
- Verify account type is Expense (income accounts may not need budgets)

**Allocation failed**:
- Verify allocation rules exist and are marked active
- Check period start/end dates are valid
- Verify expense accounts exist and have balances in period
- Check for permission to create journal entries

---

**Last Updated**: [YYYY-MM-DD]  
**Version**: 1.0.0  
**Build Status**: ✅ Successful
