# Chart of Accounts Code Generation - Documentation

## Overview

This documentation covers the automatic code generation system for Chart of Accounts groups and accounts in the KasBook POS application.

## Files Created

### 1. **CodesUpdateHelper.cs** (`POS.DLL\Accounts\CodesUpdateHelper.cs`)
   - **Purpose**: C# helper class for batch-updating codes in existing groups and accounts
   - **Key Classes**:
	 - `CodesUpdateHelper`: Main utility class
	 - `CodesUpdateResult`: Result object for update operations
	 - `CodeAssignmentStats`: Statistics on code coverage
   - **Key Methods**:
	 - `UpdateAllCodes()`: Runs all code updates in a transaction
	 - `GetCodeAssignmentStats()`: Retrieves current code coverage statistics

### 2. **UpdateCodesSQL.sql** (`POS.DLL\Accounts\UpdateCodesSQL.sql`)
   - **Purpose**: Pure SQL script for generating codes (alternative to C# helper)
   - **Can be executed directly in SQL Server Management Studio**

### 3. **frm_codes_maintenance.cs** (`pos\Accounts\Maintenance\frm_codes_maintenance.cs`)
   - **Purpose**: WinForms UI for running code generation from the application
   - **Features**:
	 - Display current statistics (total, with codes, missing, coverage %)
	 - One-click code generation button
	 - Refresh statistics button
	 - Status messages and error handling

---

## Code Numbering Scheme

### Level-1 Groups (Root Groups - by Account Type)
Assigned based on account type:
- **Assets**: `1000`
- **Liabilities**: `2000`
- **Equity**: `3000`
- **Income/Revenue**: `4000`
- **Expenses**: `5000`

### Level-2 Groups (Sub-Groups)
Generated incrementally within parent's range:
```
Parent Code (e.g., 1000) + 2-digit sequence (00, 01, 02, ...)
Examples: 1000, 1001, 1002, ..., 1099
```

### Level-3 Accounts
Generated incrementally within parent group's range, per branch:
```
Group Code (e.g., 1000) + '-' + 3-digit sequence (001, 002, 003, ...)
Examples: 1000-001, 1000-002, 1000-003, ...
```

---

## Usage Methods

### Method 1: Using C# Helper (Programmatically)

```csharp
using POS.DLL.Accounts;

// Create instance
var helper = new CodesUpdateHelper();

// Get current statistics
var stats = helper.GetCodeAssignmentStats();
Console.WriteLine($"Level-1 Groups: {stats.Level1GroupsTotal}, with codes: {stats.Level1GroupsWithCodes}");
Console.WriteLine($"Coverage: {stats.Level1GroupsCoverage:F2}%");

// Update all codes
var result = helper.UpdateAllCodes();
if (result.IsSuccess)
{
	Console.WriteLine(result.Message);
	// Output: "Successfully updated X Level-1 groups, Y Level-2 groups, and Z accounts."
}
else
{
	Console.WriteLine($"Error: {result.Message}");
}
```

### Method 2: Using WinForms UI

```csharp
// Call from admin menu or maintenance section
var form = new frm_codes_maintenance();
form.ShowDialog();
```

The form displays:
- Total records per category (Level-1 Groups, Level-2 Groups, Accounts)
- Records with codes assigned
- Records missing codes
- Coverage percentage

Clicking "Generate Codes" will:
1. Ask for confirmation
2. Execute the update in a database transaction
3. Refresh statistics
4. Show success/error message

### Method 3: Using SQL Script

Execute directly in SQL Server Management Studio:

```sql
-- Run the script file: POS.DLL\Accounts\UpdateCodesSQL.sql
-- Or copy-paste the SQL content into SSMS

-- The script will:
-- 1. Update Level-1 groups based on account type
-- 2. Update Level-2 groups with sequential codes
-- 3. Update accounts with sequential codes per group and branch
-- 4. Display verification results
-- 5. Show summary statistics
```

---

## Integration with Existing Forms

### In `frm_addAccount.cs`

The form now auto-generates codes when adding new accounts:

```csharp
public void frm_addAccount_Load(object sender, EventArgs e)
{
	if (lbl_edit_status.Text == "true")
	{
		// Edit mode - code is not changed
		btn_save.Text = "Update";
	}
	else
	{
		// Add mode - auto-generate code
		btn_save.Text = "Save";
		GenerateAccountCode();
		cmb_group_id.SelectedValueChanged += (s, ev) => GenerateAccountCode();
	}
}

private void GenerateAccountCode()
{
	try
	{
		int groupId = Convert.ToInt32(cmb_group_id.SelectedValue);
		var coaBll = new ChartOfAccountsBLL();
		string generatedCode = coaBll.GenerateAccountCode(groupId);
		txt_account_code.Text = generatedCode;
	}
	catch { /* Handle error */ }
}
```

### In `frm_addGroup.cs`

Similar implementation for auto-generating group codes:

```csharp
private void GenerateGroupCode()
{
	try
	{
		int parentId = Convert.ToInt32(cmb_parent_id.SelectedValue);
		var coaBll = new ChartOfAccountsBLL();
		string generatedCode = coaBll.GenerateAccountCode(parentId);
		txt_group_code.Text = generatedCode;
	}
	catch { /* Handle error */ }
}
```

---

## Database Schema Assumptions

The helper assumes the following tables exist:

### `acc_groups`
- `id` (INT) - Primary Key
- `code` (NVARCHAR(50)) - Account code (nullable initially)
- `name` (NVARCHAR(255))
- `parent_id` (INT) - Foreign key to parent group (0 or NULL for root)
- `account_type_id` (INT) - Foreign key to account type

### `acc_accounts`
- `id` (INT) - Primary Key
- `code` (NVARCHAR(50)) - Account code (nullable initially)
- `name` (NVARCHAR(255))
- `group_id` (INT) - Foreign key to group
- `branch_id` (INT) - Branch identifier

### `acc_account_type`
- `id` (INT) - Primary Key
- `name` (NVARCHAR(100))

---

## Error Handling

All methods include try-catch blocks and return error information:

```csharp
public CodesUpdateResult UpdateAllCodes()
{
	var result = new CodesUpdateResult();
	// ... execution code ...
	if (exception)
	{
		result.IsSuccess = false;
		result.Message = $"Error updating codes: {ex.Message}";
	}
	return result;
}
```

**Common Errors:**
- Database connection failure → Check connection string in `dbConnection`
- Missing account types → Verify `acc_account_type` table has entries
- NULL/invalid foreign keys → Ensure referential integrity

---

## Performance Considerations

- **Batch Update**: Uses single SQL UPDATE with CTE for efficiency
- **Transaction**: All updates wrapped in SqlTransaction for consistency
- **Timeout**: Command timeout set to 120 seconds (adjustable)
- **Index**: Consider adding indexes on `parent_id` and `group_id` for large datasets

### Estimated Performance:
- Level-1 Groups: < 100ms for 100+ groups
- Level-2 Groups: < 500ms for 1000+ groups
- Accounts: 1-2 seconds for 10,000+ accounts

---

## Verification & Testing

### Query to verify results:

```sql
-- Verify Level-1 Groups
SELECT code, name, account_type_id
FROM acc_groups
WHERE parent_id = 0 OR parent_id IS NULL
ORDER BY code;

-- Verify Level-2 Groups
SELECT g.code, g.name, p.code AS parent_code
FROM acc_groups g
LEFT JOIN acc_groups p ON g.parent_id = p.id
WHERE g.parent_id > 0
ORDER BY g.code;

-- Verify Accounts (sample)
SELECT TOP 50 a.code, a.name, g.code AS group_code
FROM acc_accounts a
INNER JOIN acc_groups g ON a.group_id = g.id
ORDER BY a.code;
```

### Coverage Report:

```csharp
var stats = helper.GetCodeAssignmentStats();

// All should be 100.0 after successful update
Console.WriteLine($"Level-1 Coverage: {stats.Level1GroupsCoverage:F2}%");
Console.WriteLine($"Level-2 Coverage: {stats.Level2GroupsCoverage:F2}%");
Console.WriteLine($"Accounts Coverage: {stats.AccountsCoverage:F2}%");
```

---

## Migration Steps (For Existing Database)

1. **Backup database** before running updates
2. **Check current statistics**:
   ```csharp
   var stats = helper.GetCodeAssignmentStats();
   // Note: Level1GroupsMissing, Level2GroupsMissing, AccountsMissing
   ```
3. **Run code generation**:
   ```csharp
   var result = helper.UpdateAllCodes();
   ```
4. **Verify results**:
   ```csharp
   var newStats = helper.GetCodeAssignmentStats();
   // Verify all missing counts are now 0
   ```
5. **Update any dependent systems** if needed

---

## Rollback / Revert Codes

If you need to revert (rarely needed):

```sql
-- Clear all generated codes
UPDATE acc_groups SET code = NULL WHERE parent_id = 0 OR parent_id IS NULL;
UPDATE acc_groups SET code = NULL WHERE parent_id > 0;
UPDATE acc_accounts SET code = NULL;

-- Then re-run the update process
```

---

## Related Classes

- **ChartOfAccountsBLL** (`POS.BLL\Accounts\ChartOfAccountsBLL.cs`)
  - `GenerateAccountCode(int parentGroupId)`: Generates next available code
  - `IsCodeUnique(...)`: Validates code uniqueness

- **AccGroupModel** (`POS.Core\Accounts\AccGroupModel.cs`)
- **AccAccountModel** (`POS.Core\Accounts\AccAccountModel.cs`)

---

## Support & Troubleshooting

### Issue: "Cannot connect to database"
**Solution**: Verify connection string in `dbConnection.ConnectionString`

### Issue: "Account types not found"
**Solution**: Ensure `acc_account_type` table is populated with: Asset, Liability, Equity, Income, Expense

### Issue: "Foreign key violations"
**Solution**: 
1. Run orphan cleanup first
2. Verify all group_id values in acc_accounts exist in acc_groups
3. Verify all parent_id values in acc_groups exist (or are NULL/0)

### Issue: "Codes already exist"
**Solution**: The helper only updates records where code IS NULL or '' or '0'
If codes already exist, they are left unchanged.

---

## Version History

- **v1.0** (Initial Release): Basic code generation for Level-1, Level-2 groups and accounts
- Future: Multi-digit codes, custom ranges, code prefix support

---

## License & Ownership

Part of KasBook POS System
All updates are logged in audit tables if available.

