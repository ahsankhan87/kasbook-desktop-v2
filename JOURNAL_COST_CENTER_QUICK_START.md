# Cost Center in Journal Entry - Quick Start

## What Changed?

The **Cost Center** column in the journal entry grid now loads **real cost centers from the database** instead of demo values.

---

## For End Users

### Entering Journal Entry with Cost Center

1. **Open Journal Entry form** (`Accounting → Journals → New Entry`)

2. **In the Cost Center column**:
   - Click the dropdown
   - Select a cost center (e.g., "CC-001 — Sales")
   - Or leave blank for "Unallocated"

3. **Save the entry**
   - Cost center is stored with the line item
   - Appears in departmental P&L reports

### Editing Existing Entries

1. **Open existing draft voucher**
2. **Cost centers are auto-restored**
   - Previously assigned cost center shows in dropdown
3. **Edit if needed** or leave as-is

### Viewing by Department

1. **Go to Reports → Departmental P&L**
2. **Select period and cost centers**
3. **Entries are shown by column (by cost center)**

---

## For Developers

### Architecture

```
frm_journal_entries.cs
	↓ (on form load)
ConfigureCostCenters()
	↓
CostCenterBLL.GetCostCenterDropdown()
	↓
CostCenterDLL.GetCostCenterDropdown()
	↓
SQL: SELECT from acc_cost_centers WHERE is_active = 1
	↓
BindingSource → DataGridViewComboBoxColumn
```

### Code Usage

**Loading cost centers on form init:**
```csharp
private void ConfigureCostCenters()
{
	CostCenterBLL ccBll = new CostCenterBLL();
	_costCentersTable = ccBll.GetCostCenterDropdown();
	_costCenterBindingSource.DataSource = _costCentersTable;
	cost_center.DataSource = _costCenterBindingSource;
	cost_center.DisplayMember = "display_text";
	cost_center.ValueMember = "id";
}
```

**Restoring when editing:**
```csharp
int costCenterId = line["cost_center_id"] != DBNull.Value 
	? Convert.ToInt32(line["cost_center_id"]) 
	: 0;
gridRow.Cells["cost_center"].Value = costCenterId > 0 
	? (object)costCenterId 
	: (object)DBNull.Value;
```

**Getting selected value when saving:**
```csharp
object cellValue = grid_journal.Rows[i].Cells["cost_center"].Value;
int costCenterId = cellValue != null && cellValue != DBNull.Value 
	? Convert.ToInt32(cellValue) 
	: 0;
```

---

## Database Integration

### Table: acc_entries
- **Column**: `cost_center_id` (INT NULL)
- **Reference**: `acc_cost_centers(cc_id)`
- **Index**: `IX_acc_entries_cost_center_id`
- **Nullable**: Yes (allows unallocated entries)

### Query (GetVoucherLines)
```sql
SELECT 
	E.id, E.account_id, E.description,
	E.debit, E.credit,
	ISNULL(E.cost_center_id, 0) AS cost_center_id  -- ← NEW
FROM acc_entries E
WHERE E.invoice_no = @invoice_no
```

---

## Features

✅ **Real Data** - Loads from database, not hardcoded  
✅ **Active Only** - Shows only active cost centers  
✅ **Unallocated** - Empty option for entries without cost center  
✅ **Edit Support** - Restores cost center when editing draft  
✅ **Reporting** - Data flows to departmental P&L  
✅ **Error Handling** - Graceful fallback if load fails  

---

## Testing Scenarios

### Scenario 1: New Entry with Cost Center
```
1. Open Journal Entry
2. Add line: Account=5100, Debit=1000, Cost Center=CC-001 (Sales)
3. Add line: Account=1100, Credit=1000
4. Save
→ Cost center assigned to first line, visible in P&L
```

### Scenario 2: Unallocated Entry
```
1. Open Journal Entry
2. Add line: Account=5100, Debit=500, Cost Center=[blank]
3. Add line: Account=1100, Credit=500
4. Save
→ Entry appears in "Unallocated" column in P&L
```

### Scenario 3: Edit Existing Entry
```
1. Search for draft voucher JV-001
2. Load for editing
3. Cost center is pre-selected (if was set)
4. Edit and save
→ Cost center preserved or updated
```

### Scenario 4: Departmental P&L View
```
1. Go to Reports → Departmental P&L
2. Period: Jun 2024
3. Cost Centers: All
4. See columns: CC-001 Sales | CC-002 Admin | Unallocated | Total
5. Journal entries appear in correct column
```

---

## Common Questions

**Q: What if I don't assign a cost center?**  
A: Entry goes to "Unallocated" column in reports. Cost center remains optional for backward compatibility.

**Q: Can I change cost center on posted entries?**  
A: No, only draft vouchers are editable. Post → Approve → Locked.

**Q: How do unallocated entries show in reports?**  
A: Separate "Unallocated" column in Departmental P&L. Helps identify cost center gaps.

**Q: Does cost center affect posting?**  
A: No, posting logic unchanged. Cost center is metadata, not part of double-entry balancing.

**Q: Can I allocate entries retroactively?**  
A: Use Allocation Rules form to auto-allocate by rules. Manual reassignment requires reversing entry.

---

## Related Documentation

- **Module Setup**: See `COST_CENTER_MODULE_SUMMARY.md`
- **Full Integration**: See `COST_CENTER_INTEGRATION_CHECKLIST.md`
- **API Reference**: See `COST_CENTER_QUICK_REFERENCE.md`
- **Reports**: See `frm_departmental_pl.cs` (Departmental P&L form)
- **Budgets**: See `frm_budget_setup.cs` (Budget control)

---

## Support

For issues:
1. Verify cost centers exist in **Cost Center Setup** form
2. Check cost centers are **marked as Active**
3. Ensure **Cost Center module database script** has been run
4. See error message if cost center dropdown fails to load

---

**Status**: ✅ Integration Complete & Working

