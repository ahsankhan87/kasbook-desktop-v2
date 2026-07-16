# Import Module Stored Procedures - Schema Alignment Changes

## Overview
This document outlines the changes made to align the import module stored procedures with the actual database schema used in the codebase (as defined in `JournalsDLL.cs` and `AccountsDLL.cs`).

## Key Schema Corrections

### 1. **Account Table Structure**
**Previous Assumption:**
- Fields: `account_code`, `account_name`, `is_active`, `account_type`

**Actual Schema (from AccountsDLL.cs):**
- Table: `acc_accounts`
- Fields: `id`, `code`, `name`, `name_2`, `group_id`, `op_dr_balance`, `op_cr_balance`, `description`, `branch_id`, `user_id`, `date_created`, `date_updated`
- **No direct `account_type` field** - type is derived through `acc_groups` relationship

### 2. **Account Type Hierarchy**
**Correct Relationship Chain:**
```
acc_accounts.group_id → acc_groups.id → acc_groups.account_type_id → acc_account_type.id
```

**Example Query Pattern:**
```sql
SELECT a.* 
FROM acc_accounts a
INNER JOIN acc_groups g ON g.id = a.group_id
INNER JOIN acc_account_type at ON at.id = g.account_type_id
WHERE at.name = 'Asset'
```

### 3. **Voucher/Entry Linking**
**Previous Assumption:**
- `acc_entries.entry_id` links to `acc_entries_header.id`

**Actual Schema (from JournalsDLL.cs):**
- `acc_entries.invoice_no` links to `acc_entries_header.InvoiceNo` (string-based link)
- `acc_entries.entry_id` is actually the header ID reference but join must include `branch_id`

**Correct Join Pattern:**
```sql
FROM acc_entries ae
INNER JOIN acc_entries_header aeh 
	ON aeh.InvoiceNo = ae.invoice_no 
	AND aeh.branch_id = ae.branch_id
```

### 4. **User Table Fields**
**Previous Assumption:**
- `pos_users.full_name`

**Actual Schema:**
- Table: `pos_users`
- Fields: `id`, `username`, `name` (not `full_name`)
- Display pattern: `ISNULL(u.name, u.username)`

### 5. **Schema Prefix Consistency**
**Changed:**
- Removed unnecessary `dbo.` prefix throughout for consistency with existing codebase patterns

---

## Stored Procedure Changes

### **sp_RollbackImport**

#### Changes Made:
1. **Fixed entry deletion join:**
   ```sql
   -- OLD (INCORRECT):
   DELETE ae
   FROM dbo.acc_entries ae
   INNER JOIN dbo.acc_import_vouchers aiv ON aiv.voucher_id = ae.entry_id

   -- NEW (CORRECT):
   DELETE ae
   FROM acc_entries ae
   INNER JOIN acc_entries_header aeh 
	   ON aeh.InvoiceNo = ae.invoice_no AND aeh.branch_id = ae.branch_id
   INNER JOIN acc_import_vouchers aiv ON aiv.voucher_id = aeh.id
   ```

2. **Removed `dbo.` prefix** from all table references

3. **Updated audit trail table check** to not use `dbo.` prefix

---

### **sp_ImportDataQualityCheck**

#### Changes Made:
1. **Fixed account field references:**
   ```sql
   -- OLD:
   'Account Code: ' + ISNULL(a.account_code, '') + ', Name: ' + ISNULL(a.account_name, '')

   -- NEW:
   'Account Code: ' + ISNULL(a.code, '') + ', Name: ' + ISNULL(a.name, '')
   ```

2. **Fixed account type filtering (Check 1):**
   ```sql
   -- OLD (INCORRECT - no such fields):
   FROM dbo.acc_accounts a
   WHERE a.is_active = 1
	 AND a.account_type IN ('Asset', 'Liability', 'Equity')

   -- NEW (CORRECT - using proper joins):
   FROM acc_accounts a
   INNER JOIN acc_groups g ON g.id = a.group_id
   INNER JOIN acc_account_type at ON at.id = g.account_type_id
   WHERE at.name IN ('Asset', 'Liability', 'Equity')
   ```

3. **Fixed entry-to-voucher join (Check 1):**
   ```sql
   -- OLD:
   FROM dbo.acc_entries ae
   INNER JOIN dbo.acc_import_vouchers aiv ON aiv.voucher_id = ae.entry_id
   WHERE ae.account_code = a.account_code

   -- NEW:
   FROM acc_entries ae
   INNER JOIN acc_entries_header aeh 
	   ON aeh.InvoiceNo = ae.invoice_no AND aeh.branch_id = ae.branch_id
   INNER JOIN acc_import_vouchers aiv ON aiv.voucher_id = aeh.id
   WHERE ae.account_id = a.id
   ```

4. **Fixed large balance check (Check 2):**
   ```sql
   -- OLD:
   GROUP BY ae.account_code

   -- NEW:
   INNER JOIN acc_accounts a ON a.id = ae.account_id
   GROUP BY a.code, a.name
   ```

5. **Fixed voucher balance check (Check 3):**
   ```sql
   -- OLD:
   FROM dbo.acc_entries_header aeh
   INNER JOIN dbo.acc_entries ae ON ae.entry_id = aeh.id

   -- NEW:
   FROM acc_entries_header aeh
   INNER JOIN acc_entries ae 
	   ON ae.invoice_no = aeh.InvoiceNo AND ae.branch_id = aeh.branch_id
   ```

6. **Fixed duplicate account check (Check 4):**
   ```sql
   -- OLD:
   GROUP BY aeh.id, aeh.InvoiceNo, ae.account_code

   -- NEW:
   INNER JOIN acc_accounts a ON a.id = ae.account_id
   GROUP BY aeh.id, aeh.InvoiceNo, a.code, a.name, ae.account_id
   ```

7. **Fixed trial balance check (Check 5):**
   ```sql
   -- OLD:
   FROM dbo.acc_entries ae
   INNER JOIN dbo.acc_import_vouchers aiv ON aiv.voucher_id = ae.entry_id

   -- NEW:
   FROM acc_entries ae
   INNER JOIN acc_entries_header aeh 
	   ON aeh.InvoiceNo = ae.invoice_no AND aeh.branch_id = ae.branch_id
   INNER JOIN acc_import_vouchers aiv ON aiv.voucher_id = aeh.id
   ```

---

### **sp_GetImportHistory**

#### Changes Made:
1. **Fixed user display name field:**
   ```sql
   -- OLD:
   ISNULL(u.full_name, u.username) AS imported_by_name

   -- NEW:
   ISNULL(u.name, u.username) AS imported_by_name
   ```

2. **Removed `dbo.` prefix** from all table references

---

### **sp_ValidateOpeningBalanceImport**

#### Changes Made:
1. **Fixed account code field in validation:**
   ```sql
   -- OLD:
   WHERE a.account_code = br.acc_code

   -- NEW:
   WHERE a.code = br.acc_code
   ```

2. **Fixed STRING_AGG for SQL Server 2016 compatibility:**
   ```sql
   -- OLD (SQL Server 2017+ only):
   SELECT @ErrorList = '[' + STRING_AGG(...) + ']'

   -- NEW (SQL Server 2012+ compatible):
   SELECT @ErrorJson = '[' + STUFF((
	   SELECT ',' + '{"ErrorType":"' + ErrorType + ...
	   FROM #ValidationErrors
	   FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') + ']'
   ```

3. **Removed `dbo.` prefix** from all table references

---

## Testing Checklist

After applying these changes, verify:

- [ ] All stored procedures compile without errors
- [ ] `sp_RollbackImport` correctly deletes entries using invoice_no link
- [ ] `sp_ImportDataQualityCheck` correctly identifies accounts by type through groups
- [ ] `sp_GetImportHistory` displays correct user names
- [ ] `sp_ValidateOpeningBalanceImport` correctly validates account codes
- [ ] Import/rollback workflow completes successfully in the WinForms UI
- [ ] Data quality checks run without errors after import
- [ ] Opening balance validation catches mismatched debits/credits

---

## Related Files Modified
- `POS.DLL/Accounts/ImportModule_Procedures.sql` (this file)

## Reference Files Used for Schema
- `POS.DLL/Accounts/JournalsDLL.cs` - Voucher/entry table structure
- `POS.DLL/Accounts/AccountsDLL.cs` - Account/group/type hierarchy

---

## Notes
- All changes maintain backward compatibility with existing import functionality
- SQL Server 2012+ compatibility maintained (no reliance on 2017+ features)
- Schema aligns with existing accounting module patterns
- Branch filtering logic preserved throughout all procedures
