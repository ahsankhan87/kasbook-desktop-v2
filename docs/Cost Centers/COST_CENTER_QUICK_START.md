# Cost Center Module — Quick Start Guide

## 🚀 5-Minute Setup

### Phase 1: Register Code Files (2 min)

1. **Open Visual Studio** → Right-click solution → "Edit Solution File" or open in file explorer
2. **Edit three `.csproj` files** manually:

   **POS.Core\POS.Core.csproj** — Find the `<ItemGroup>` with `<Compile>` elements, add:
   ```xml
   <Compile Include="Accounts\CostCenterModels.cs" />
   ```

   **POS.DLL\POS.DLL.csproj** — Find the `<ItemGroup>` with `<Compile>` and `<Content>` elements, add:
   ```xml
   <Compile Include="Accounts\CostCenterDLL.cs" />
   <Content Include="Accounts\CostCenterProcedures.sql" />
   ```

   **POS.BLL\POS.BLL.csproj** — Find the `<ItemGroup>` with `<Compile>` elements, add:
   ```xml
   <Compile Include="Accounts\CostCenterBLL.cs" />
   ```

3. **Save** and reload the solution in Visual Studio

### Phase 2: Deploy Database (2 min)

1. **Open SQL Server Management Studio** (or equivalent)
2. **Connect** to your target database
3. **Open and run:** `POS.DLL\Accounts\CostCenterProcedures.sql`
4. **Verify success:** No errors in the output

### Phase 3: Build & Test (1 min)

1. **In Visual Studio:** `Build → Clean Solution` → `Build → Rebuild Solution`
2. **Check output:** "Build successful" with no errors
3. **Done!** ✅

---

## 🎯 First Use: Create a Cost Center

### Via Code (Console/Unit Test)

```csharp
using POS.BLL;
using POS.Core;

// Create a cost center
var bll = new CostCenterBLL();
var model = new CostCenterModel
{
	CcCode = "DEPT-SALES",
	CcName = "Sales Department",
	CcType = "Department",
	IsActive = true,
	StartDate = DateTime.Now.Date
};

int ccId = bll.SaveCostCenter(model, userId: 1);
Console.WriteLine($"Created cost center: {ccId}");

// Get it back
var retrieved = bll.GetCostCenterById(ccId);
Console.WriteLine($"Retrieved: {retrieved.CcName}");

// Get dropdown for forms
var dropdown = bll.GetCostCenterDropdown();
foreach (DataRow row in dropdown.Rows)
{
	Console.WriteLine($"{row["display_text"]}");
}
```

---

## 📊 Second Use: Set Monthly Budgets

```csharp
using POS.BLL;
using POS.Core;

var bll = new CostCenterBLL();

// Create budget entries for a cost center
var budgets = new List<AccountBudget>
{
	new AccountBudget
	{
		CcId = 1,                          // Cost center ID
		FinancialYearId = 2025,            // Year
		AccountId = 5001,                  // Expense account
		JanBudget = 50000,
		FebBudget = 50000,
		MarBudget = 50000,
		AprBudget = 50000,
		MayBudget = 50000,
		JunBudget = 50000,
		JulBudget = 50000,
		AugBudget = 50000,
		SepBudget = 50000,
		OctBudget = 50000,
		NovBudget = 50000,
		DecBudget = 50000
	}
};

bll.SetBudget(ccId: 1, yearId: 2025, budgets: budgets, userId: 1);
Console.WriteLine("Budget saved.");
```

---

## ⚠️ Third Use: Check Budget Before Posting

```csharp
using POS.BLL;
using POS.Core;

var bll = new CostCenterBLL();

// Check if posting $5000 to account 5001 in cost center 1 would exceed budget
var check = bll.CheckBudgetBeforePosting(
	ccId: 1,
	accId: 5001,
	amount: 5000,
	date: DateTime.Today
);

if (check.IsOverBudget)
{
	Console.WriteLine($"❌ Over budget! {check.Message}");
	Console.WriteLine($"   Remaining: {check.RemainingBudget:N2}");
}
else
{
	Console.WriteLine($"✓ Within budget. Remaining: {check.RemainingBudget:N2}");
}
```

---

## 💰 Fourth Use: Run Expense Allocation

```csharp
using POS.BLL;
using POS.Core;

var bll = new CostCenterBLL();

// Run allocation for January 2025
var result = bll.RunExpenseAllocation(
	period: new DateTime(2025, 1, 1),
	userId: 1
	// allocationRuleId: null  (all rules)
);

if (result.Success)
{
	Console.WriteLine($"✓ Allocation complete");
	Console.WriteLine($"  Voucher: {result.VoucherNo}");
	Console.WriteLine($"  Total: {result.TotalAllocated:N2}");
	Console.WriteLine($"  Departments: {result.Allocations.Count}");

	foreach (var alloc in result.Allocations)
	{
		Console.WriteLine($"    {alloc.AllocationName}: {alloc.AllocatedAmount:N2}");
	}
}
else
{
	Console.WriteLine($"❌ {result.Message}");
}
```

---

## 📈 Fifth Use: Get Budget Alerts

```csharp
using POS.BLL;
using POS.Core;

var bll = new CostCenterBLL();

// Get list of over-budget accounts for cost center 1, current month
var alerts = bll.GetBudgetAlert(ccId: 1, currentDate: DateTime.Today);

if (alerts.Count == 0)
{
	Console.WriteLine("✓ No budget alerts.");
}
else
{
	foreach (var alert in alerts)
	{
		Console.WriteLine($"⚠ {alert.AccountName}");
		Console.WriteLine($"   Budget: {alert.BudgetAmount:N2}");
		Console.WriteLine($"   Actual: {alert.ActualAmount:N2}");
		Console.WriteLine($"   Overspend: {alert.OverspendAmount:N2} ({alert.OverspendPercent:F0}%)");
		Console.WriteLine($"   Severity: {alert.SeverityLevel}");
	}
}
```

---

## 📋 Integration Checklist

### In JournalsBLL.PostJournalVoucher (Before Posting)

```csharp
public void PostJournalVoucher(JournalVoucherModel voucher, int userId)
{
	// ... existing validation ...

	// NEW: Check budget for each line with cost center
	var ccBll = new CostCenterBLL();
	foreach (var line in voucher.Lines)
	{
		if (line.CostCenterId.HasValue && line.CostCenterId > 0)
		{
			var budgetCheck = ccBll.CheckBudgetBeforePosting(
				line.CostCenterId.Value,
				line.AccountId,
				Math.Abs(line.Amount),
				voucher.EntryDate
			);

			if (budgetCheck.IsOverBudget)
			{
				// Option 1: Block posting
				throw new OperationCanceledException(budgetCheck.Message);

				// Option 2: Warn user (needs UI confirmation)
				// if (!userConfirmed)
				//     throw new OperationCanceledException(budgetCheck.Message);
			}
		}
	}

	// ... existing posting logic ...
}
```

### In frm_journals_entry (Cost Center Selection)

```csharp
private void cmbCostCenter_SelectedIndexChanged(object sender, EventArgs e)
{
	int ccId = (int)cmbCostCenter.SelectedValue;

	// Show budget warnings
	var bll = new CostCenterBLL();
	var alerts = bll.GetBudgetAlert(ccId, DateTime.Today);

	if (alerts.Count > 0)
	{
		pnlBudgetWarning.Visible = true;
		dgvBudgetAlerts.DataSource = alerts;

		// Color code severity
		foreach (DataGridViewRow row in dgvBudgetAlerts.Rows)
		{
			string severity = row.Cells["SeverityLevel"].Value?.ToString() ?? "Info";
			row.DefaultCellStyle.BackColor = severity == "Critical" ? Color.Red
										   : severity == "Warning" ? Color.Yellow
										   : Color.LightGray;
		}
	}
	else
	{
		pnlBudgetWarning.Visible = false;
	}
}
```

### Form Load: Populate Cost Center Dropdown

```csharp
private void frm_journals_entry_Load(object sender, EventArgs e)
{
	var bll = new CostCenterBLL();
	var dtCC = bll.GetCostCenterDropdown();

	cmbCostCenter.DataSource = dtCC;
	cmbCostCenter.DisplayMember = "display_text";
	cmbCostCenter.ValueMember = "id";
	cmbCostCenter.SelectedIndex = -1;  // No selection initially
}
```

---

## 🔄 Recurring Tasks

### Monthly: Run Expense Allocation

**Schedule in Task Scheduler or call from admin form:**

```csharp
private void btnRunAllocation_Click(object sender, EventArgs e)
{
	var bll = new CostCenterBLL();
	var result = bll.RunExpenseAllocation(DateTime.Now, UsersModal.logged_in_userid);

	UiMessages.Show(result.Message, 
		result.Success ? "Allocation Complete" : "Allocation Failed",
		result.Success ? MessageBoxButtons.OK : MessageBoxButtons.OKCancel,
		result.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error
	);
}
```

### Fiscal Year Start: Set Annual Budgets

Create a form to:
1. Select fiscal year
2. Select cost center
3. Enter 12 monthly amounts per account
4. Click "Save"

```csharp
private void btnSaveBudget_Click(object sender, EventArgs e)
{
	var budgets = new List<AccountBudget>();

	foreach (DataGridViewRow row in dgvBudgets.Rows)
	{
		if (row.IsNewRow) continue;

		budgets.Add(new AccountBudget
		{
			AccountId = (int)row.Cells["AccountId"].Value,
			JanBudget = (decimal)row.Cells["JanBudget"].Value,
			// ... 11 more months
		});
	}

	var bll = new CostCenterBLL();
	bll.SetBudget(selectedCcId, selectedYearId, budgets, UsersModal.logged_in_userid);
	MessageBox.Show("Budget saved.");
}
```

---

## 🧪 Validation Queries

**Verify everything is working:**

```sql
-- 1. Check cost center created
SELECT TOP 5 cc_id, cc_code, cc_name FROM dbo.acc_cost_centers;

-- 2. Check budgets set
SELECT TOP 5 * FROM dbo.acc_cost_center_budgets;

-- 3. Check allocation rules exist
SELECT TOP 5 * FROM dbo.acc_cost_center_allocations;

-- 4. Check cost center entries tagged
SELECT TOP 10 entry_id, account_id, cost_center_id, debit, credit
FROM dbo.acc_entries WHERE cost_center_id IS NOT NULL;

-- 5. Check last allocation voucher
SELECT TOP 1 * FROM dbo.acc_entries_header WHERE InvoiceNo LIKE 'CCA-%' ORDER BY date_created DESC;
```

---

## 🆘 Troubleshooting

| Problem | Solution |
|---------|----------|
| Build fails: "CostCenterModels not found" | Check `.csproj` registration; close/reopen solution |
| SQL error: "acc_cost_centers table not found" | Run `CostCenterProcedures.sql` against target database |
| Allocation returns 0 entries | Verify allocation rules are active (`is_active = 1`) |
| Budget check always returns false | Check budget is set for the cost center and fiscal year |
| Circular reference error on save | Verify parent is not a descendant of the cost center |

---

## 📞 Support Files

- **Schema Reference:** See `COST_CENTER_SQL_SCHEMA_REFERENCE.md`
- **Full Summary:** See `COST_CENTER_MODULE_SUMMARY.md`
- **Registration Details:** See `COST_CENTER_REGISTRATION.md`
- **Source Code:** 
  - `POS.Core/Accounts/CostCenterModels.cs` (Models)
  - `POS.DLL/Accounts/CostCenterDLL.cs` (Data Access)
  - `POS.BLL/Accounts/CostCenterBLL.cs` (Business Logic)
  - `POS.DLL/Accounts/CostCenterProcedures.sql` (Database)

---

**Ready to go! Start with Phase 1 above.** ✅
