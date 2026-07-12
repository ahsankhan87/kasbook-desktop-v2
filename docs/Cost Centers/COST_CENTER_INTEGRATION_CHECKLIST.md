# Cost Center Module - Integration Checklist

## Phase 1: Build Verification ✅ COMPLETE

- [x] Backend models compile (CostCenterModels.cs)
- [x] Data access layer compiles (CostCenterDLL.cs)
- [x] Business logic layer compiles (CostCenterBLL.cs)
- [x] All WinForms UI files created and compile
- [x] Solution builds without errors

**Build Status**: ✅ Clean build successful

---

## Phase 2: Project File Registration

### Action: Update `pos.csproj`

**If using Visual Studio UI**:
1. Right-click Project → Add → Existing Item
2. Navigate to `pos\Accounting\CostCenter\` folder
3. Select all `.cs` and `.Designer.cs` files
4. Click "Add"

**If manually editing `.csproj`** (add to `<ItemGroup>`):
```xml
<!-- Cost Center Forms -->
<Compile Include="Accounting\CostCenter\frm_cost_center_setup.cs" />
<Compile Include="Accounting\CostCenter\frm_cost_center_setup.Designer.cs">
  <DependentUpon>frm_cost_center_setup.cs</DependentUpon>
</Compile>
<Compile Include="Accounting\CostCenter\frm_cost_center_tree.cs" />
<Compile Include="Accounting\CostCenter\frm_cost_center_tree.Designer.cs">
  <DependentUpon>frm_cost_center_tree.cs</DependentUpon>
</Compile>
<Compile Include="Accounting\CostCenter\frm_departmental_pl.cs" />
<Compile Include="Accounting\CostCenter\frm_departmental_pl.Designer.cs">
  <DependentUpon>frm_departmental_pl.cs</DependentUpon>
</Compile>
<Compile Include="Accounting\CostCenter\frm_budget_setup.cs" />
<Compile Include="Accounting\CostCenter\frm_budget_setup.Designer.cs">
  <DependentUpon>frm_budget_setup.cs</DependentUpon>
</Compile>
<Compile Include="Accounting\CostCenter\frm_allocation_rules.cs" />
<Compile Include="Accounting\CostCenter\frm_allocation_rules.Designer.cs">
  <DependentUpon>frm_allocation_rules.cs</DependentUpon>
</Compile>
```

**After editing:**
- [ ] Save `.csproj` file
- [ ] Reload project in Visual Studio (right-click → Reload Project)
- [ ] Rebuild solution
- [ ] Verify build succeeds

---

## Phase 3: Database Deployment

### Action: Run SQL Script

**Prerequisites**:
- SQL Server connection with sa or db_owner role
- Target database: (verify from web.config or App.config)

**Steps**:
1. Open SQL Server Management Studio (SSMS)
2. Connect to server and select database
3. Open script: `POS.DLL\Accounts\CostCenterProcedures.sql`
4. Review script (check table/procedure names)
5. Execute script

**Verify**:
- [ ] Script executes without errors
- [ ] Tables created:
  - [ ] `acc_cost_centers`
  - [ ] `acc_cost_center_budgets`
  - [ ] `acc_cost_center_allocations`
- [ ] Procedures created:
  - [ ] `sp_GetCostCenterTree`
  - [ ] `sp_DepartmentalPL`
  - [ ] `sp_CostCenterBudgetVsActual`
  - [ ] `sp_AutoAllocateExpenses`
  - [ ] `sp_CostCenterSummary`
- [ ] Table-valued type created: `CostCenterIdListType`

---

## Phase 4: Menu Integration

### Action: Add Menu Items to Main Form

**Where**: `pos\frm_main.cs` (main application form)

**Add to Menu Structure**:
```
Accounting (or Finances)
  ├─ Cost Centers
  │  ├─ Setup → frm_cost_center_setup
  │  ├─ Hierarchy → frm_cost_center_tree
  │  ├─ Budget → frm_budget_setup
  │  └─ Allocation Rules → frm_allocation_rules
  └─ Reports
	 └─ Departmental P&L → frm_departmental_pl
```

**Code Pattern**:
```csharp
// In frm_main menu click handler
private void mnuCostCenterSetup_Click(object sender, EventArgs e)
{
	var frm = new pos.Accounting.CostCenter.frm_cost_center_setup();
	frm.ShowDialog(this);
}

private void mnuCostCenterTree_Click(object sender, EventArgs e)
{
	var frm = new pos.Accounting.CostCenter.frm_cost_center_tree();
	frm.ShowDialog(this);
}

private void mnuDepartmentalPL_Click(object sender, EventArgs e)
{
	var frm = new pos.Accounting.CostCenter.frm_departmental_pl();
	frm.ShowDialog(this);
}

private void mnuBudgetSetup_Click(object sender, EventArgs e)
{
	var frm = new pos.Accounting.CostCenter.frm_budget_setup();
	frm.ShowDialog(this);
}

private void mnuAllocationRules_Click(object sender, EventArgs e)
{
	var frm = new pos.Accounting.CostCenter.frm_allocation_rules();
	frm.ShowDialog(this);
}
```

**Checklist**:
- [ ] Menu items added and named appropriately
- [ ] Click handlers implemented
- [ ] Forms instantiated with correct namespace
- [ ] Forms shown as modal dialogs (`ShowDialog`)

---

## Phase 5: Journal Entry Integration

### Action: Add Cost Center Column to Journal Entry Form

**Where**: `pos\Accounting\Journal\frm_journal_entry.cs` (or similar)

**In Form Load**:
```csharp
private void InitializeCostCenterDropdown()
{
	try
	{
		var bll = new POS.BLL.CostCenterBLL();
		DataTable dt = bll.GetCostCenterDropdown();
		cmbCostCenter.DataSource = dt;
		cmbCostCenter.DisplayMember = "display_text";
		cmbCostCenter.ValueMember = "id";
		cmbCostCenter.SelectedIndex = -1; // No default
	}
	catch (Exception ex)
	{
		MessageBox.Show($"Error loading cost centers: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
	}
}
```

**Before Posting Entry** (in Save method):
```csharp
private void PostJournalEntry()
{
	// ... existing validation ...

	// Check cost center budget if assigned
	if (cmbCostCenter.SelectedValue != null && int.TryParse(cmbCostCenter.SelectedValue.ToString(), out int ccId))
	{
		var bll = new POS.BLL.CostCenterBLL();
		var budgetCheck = bll.CheckBudgetBeforePosting(ccId, accountId, debitAmount, DateTime.Today);

		if (budgetCheck.IsOverBudget)
		{
			DialogResult result = MessageBox.Show(
				$"Over budget: {budgetCheck.Message}\n\nContinue posting anyway?",
				"Budget Alert",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning);

			if (result != DialogResult.Yes)
				return;
		}
	}

	// ... post entry ...
}
```

**Checklist**:
- [ ] Cost center dropdown displays active cost centers
- [ ] Budget check is called before posting
- [ ] User sees warning if over budget
- [ ] Entry posts with cost_center_id assigned

---

## Phase 6: Security & Permissions (Optional)

### If Using Role-Based Security

**Add Permission Tags** (in form code):
```csharp
private void frm_cost_center_setup_Load(object sender, EventArgs e)
{
	// Apply permissions based on user role
	this.Tag = "CostCenter_Setup|CostCenter_Edit|CostCenter_View";
	FormSecurityExtensions.ApplyPermissions(this);
}
```

**Create Security Roles** (in database or config):
- `CostCenter_View` — View cost centers and reports
- `CostCenter_Setup` — Create/edit cost centers
- `CostCenter_Budget` — Set budgets
- `CostCenter_Allocation` — Define and run allocation rules

**Assign to Users**:
- [ ] Finance Manager: All cost center permissions
- [ ] Department Manager: View + Budget for own department
- [ ] Accountant: View + Setup + Allocation

---

## Phase 7: Testing

### Smoke Test
- [ ] Open Cost Center Setup form → Grid appears, no errors
- [ ] Open Cost Center Tree form → Tree builds, no errors
- [ ] Open Departmental P&L form → Grid appears, no errors
- [ ] Open Budget Setup form → Dropdowns populate, no errors
- [ ] Open Allocation Rules form → Grid appears, no errors

### Functional Test

**Cost Center CRUD**:
- [ ] Create new cost center (root level)
- [ ] Create child cost center under parent
- [ ] Verify circular hierarchy is rejected
- [ ] Edit existing cost center
- [ ] Deactivate cost center
- [ ] Verify deactivated center not in active dropdowns

**Budget Management**:
- [ ] Load budget grid for cost center + fiscal year
- [ ] Enter monthly budgets
- [ ] Save and reload to verify persistence
- [ ] Use "Fill Year Template" to replicate amounts
- [ ] Verify total auto-calculation

**Departmental P&L**:
- [ ] Select date range and cost centers
- [ ] Click Refresh → Report populates
- [ ] Verify column totals match company P&L
- [ ] Export to CSV and verify format
- [ ] Test year-over-year comparison (if implemented)

**Allocation Rules**:
- [ ] Create allocation rule (expense account → cost center, % method)
- [ ] Save and verify in grid
- [ ] Edit rule and change percentage
- [ ] Run auto-allocation for a month
- [ ] Verify journal entries created with cost center assignment
- [ ] Check if allocation total matches source expense

**Journal Integration**:
- [ ] Open journal entry form
- [ ] Verify cost center dropdown populated
- [ ] Select cost center and post entry
- [ ] Check if entry assigned to cost center in ledger

### Regression Test
- [ ] Existing journal posting (non-cost-center) works as before
- [ ] Departmental P&L column "Unallocated" shows entries without cost center
- [ ] Main P&L report unchanged (cost center module is additive)

---

## Phase 8: Documentation & Training

- [ ] Share `COST_CENTER_MODULE_SUMMARY.md` with team
- [ ] Share `COST_CENTER_QUICK_REFERENCE.md` with developers
- [ ] Train finance staff on cost center hierarchy creation
- [ ] Train accountants on budget entry and allocation rules
- [ ] Document cost center naming convention (e.g., "DEPT-SALES-01")
- [ ] Document budget approval workflow

---

## Phase 9: Deployment Preparation

### Pre-Production
- [ ] Run full test suite
- [ ] Backup production database
- [ ] Deploy SQL script to test environment
- [ ] Deploy .csproj changes to test build
- [ ] Deploy application DLL to test
- [ ] Run smoke tests in test environment
- [ ] Gather user feedback

### Production
- [ ] Schedule deployment window (low-traffic period)
- [ ] Notify users of planned downtime
- [ ] Run SQL script on production database
- [ ] Deploy updated application to production
- [ ] Run smoke tests
- [ ] Monitor for errors in logs
- [ ] Announce feature availability to users

---

## Rollback Plan

If issues arise:

1. **SQL Rollback** (if needed):
   ```sql
   DROP TABLE [dbo].[acc_cost_center_allocations];
   DROP TABLE [dbo].[acc_cost_center_budgets];
   DROP TABLE [dbo].[acc_cost_centers];
   DROP PROCEDURE [dbo].[sp_GetCostCenterTree];
   DROP PROCEDURE [dbo].[sp_DepartmentalPL];
   -- ... drop other procedures and types
   ```

2. **Code Rollback**:
   - Revert `.csproj` to remove Cost Center forms
   - Redeploy previous application version
   - Remove menu items

3. **Data Preservation**:
   - All entries in `acc_entries` table retain any previously-assigned `cost_center_id` values
   - No data loss if module is disabled

---

## Sign-Off

| Role | Name | Date | Status |
|------|------|------|--------|
| Developer | | | |
| QA Tester | | | |
| Finance Manager | | | |
| System Admin | | | |

---

**Module Status**: Ready for Integration  
**Last Updated**: [YYYY-MM-DD]  
**Support Contact**: [Dev Team Lead]
