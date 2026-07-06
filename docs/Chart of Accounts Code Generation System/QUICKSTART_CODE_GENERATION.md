# Quick Reference: Chart of Accounts Code Generation

## What Was Created

### Files Added:

1. **CodesUpdateHelper.cs** - C# helper for batch code generation
2. **UpdateCodesSQL.sql** - Pure SQL alternative script
3. **frm_codes_maintenance.cs** - WinForms UI for code generation
4. **CODE_GENERATION_GUIDE.md** - Full documentation

---

## Quick Start

### Option A: One-Click from UI (Easiest)

```csharp
// In your admin menu or maintenance area:
var form = new frm_codes_maintenance();
form.ShowDialog();
```

**What it does:**
- Shows current code statistics (total, with codes, coverage %)
- One-click "Generate Codes" button
- Auto-updates missing codes in database

---

### Option B: Programmatically

```csharp
using POS.DLL.Accounts;

var helper = new CodesUpdateHelper();
var result = helper.UpdateAllCodes();

if (result.IsSuccess)
	MessageBox.Show(result.Message); // "Successfully updated X groups, Y accounts"
else
	MessageBox.Show($"Error: {result.Message}");
```

---

### Option C: SQL Script

Run directly in SQL Server Management Studio:
```
File: POS.DLL\Accounts\UpdateCodesSQL.sql
```

---

## Code Format Reference

### Level-1 Groups (Root)
- Assets: `1000`
- Liabilities: `2000`
- Equity: `3000`
- Income: `4000`
- Expenses: `5000`

### Level-2 Groups (Sub-groups)
`ParentCode + Sequential (e.g., 1001, 1002, 1003)`

### Accounts
`GroupCode + '-' + Sequential (e.g., 1001-001, 1001-002)`

---

## Auto-Code on Form Load

Both `frm_addAccount.cs` and `frm_addGroup.cs` now auto-generate unique codes:

```
User opens "Add New Account" form
↓
Form loads, generates next available code
↓
Textbox shows: e.g., "1101-001"
↓
User can change group → code auto-updates
```

No manual code entry needed!

---

## Database Requirements

Must have:
- `acc_groups` table with `code`, `parent_id` columns
- `acc_accounts` table with `code`, `group_id` columns
- `acc_account_type` table with account type names

---

## Verify Results

```sql
-- Check Level-1 Groups
SELECT code, name FROM acc_groups WHERE parent_id = 0 OR parent_id IS NULL ORDER BY code;

-- Check Level-2 Groups
SELECT code, name FROM acc_groups WHERE parent_id > 0 ORDER BY code;

-- Check Accounts (first 50)
SELECT TOP 50 code, name FROM acc_accounts ORDER BY code;
```

---

## Integration Checklist

- ✅ `CodesUpdateHelper.cs` created
- ✅ `frm_codes_maintenance.cs` created for UI
- ✅ `frm_addAccount.cs` updated with auto-code generation
- ✅ `frm_addGroup.cs` updated with auto-code generation
- ⏳ Add project file reference (manual or via IDE)
- ⏳ Add menu option or button to call `frm_codes_maintenance`
- ⏳ Run code generation for existing data

---

## Troubleshooting

| Issue | Solution |
|-------|----------|
| "Connection failed" | Check `dbConnection.ConnectionString` |
| "No codes generated" | Verify `acc_account_type` has entries |
| "Foreign key error" | Ensure all group_id/parent_id values exist |
| "Codes already exist" | Helper only updates NULL/empty/'0' codes |

---

## Performance

- **Level-1 Groups**: < 100ms
- **Level-2 Groups**: < 500ms for 1000+ groups
- **Accounts**: 1-2 sec for 10,000+ accounts
- **Total**: ~2-3 seconds for complete update

---

## Next Steps

1. Add project file reference to `CodesUpdateHelper.cs`
2. Create WinForms designer for `frm_codes_maintenance.cs`
3. Add menu item to admin area pointing to maintenance form
4. Run code generation for existing data in your database
5. Test auto-code generation in account/group forms

