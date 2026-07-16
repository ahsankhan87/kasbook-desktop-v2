# Accounting Import Module - Installation Guide

## Overview
This module provides functionality to import accounting data from Excel files into the system.

## Database Installation

### Step 1: Create Tables
Execute the schema script to create the required tables:
```sql
-- File: POS.DLL\Accounts\ImportModule_Schema.sql
```

This creates:
- `acc_import_sessions` - Tracks all import sessions
- `acc_import_vouchers` - Links vouchers to import sessions for rollback
- `acc_import_templates` - Stores custom template definitions

### Step 2: Create Stored Procedures
Execute the procedures script:
```sql
-- File: POS.DLL\Accounts\ImportModule_Procedures.sql
```

This creates:
- `sp_RollbackImport` - Rollback completed import
- `sp_ImportDataQualityCheck` - Validate imported data
- `sp_GetImportHistory` - Get import history
- `sp_ValidateOpeningBalanceImport` - Pre-import validation for opening balances

Also creates the table type:
- `ImportBalanceRow` - TVP for opening balance validation

## C# Components

### ImportTemplateGenerator Class
Location: `POS.DLL\Accounts\ImportTemplateGenerator.cs`

Usage:
```csharp
using POS.DLL.Accounts;

// Generate template
string filePath = ImportTemplateGenerator.CreateImportTemplate("OPENING_BALANCE");

// Open the file
System.Diagnostics.Process.Start(filePath);
```

Supported Import Types:
- `COA` - Chart of Accounts
- `OPENING_BALANCE` - Opening Balances
- `CUSTOMER_BALANCES` - Customer Outstanding Balances
- `SUPPLIER_BALANCES` - Supplier Outstanding Balances
- `JOURNAL_HISTORY` - Historical Journal Vouchers

## Table Structures

### acc_import_sessions
Tracks each import operation with status and statistics.

| Column | Type | Description |
|--------|------|-------------|
| session_id | INT IDENTITY | Primary key |
| import_type | VARCHAR(30) | Type of import (COA, OPENING_BALANCE, etc.) |
| file_name | NVARCHAR(200) | Original file name |
| total_rows | INT | Total rows in file |
| imported_rows | INT | Successfully imported rows |
| skipped_rows | INT | Skipped rows |
| error_rows | INT | Rows with errors |
| status | VARCHAR(15) | InProgress, Completed, PartialSuccess, Failed, RolledBack |
| error_log | NVARCHAR(MAX) | JSON array of errors |
| rollback_available_until | DATETIME | Rollback expiry date |
| imported_by | INT | User ID (FK to pos_users) |
| imported_at | DATETIME | Import timestamp |

### acc_import_vouchers
Links vouchers to import sessions for rollback capability.

| Column | Type | Description |
|--------|------|-------------|
| link_id | INT IDENTITY | Primary key |
| session_id | INT | FK to acc_import_sessions |
| voucher_id | INT | FK to acc_entries_header |

### acc_import_templates
Stores reusable import template definitions.

| Column | Type | Description |
|--------|------|-------------|
| template_id | INT IDENTITY | Primary key |
| template_name | NVARCHAR(100) | Unique template name |
| import_type | VARCHAR(30) | Type of import |
| column_definitions | NVARCHAR(MAX) | JSON column definitions |
| sample_data | NVARCHAR(MAX) | JSON sample data |
| created_at | DATETIME | Creation timestamp |

## Stored Procedure Usage

### sp_RollbackImport
Rollback a completed import session.

```sql
EXEC sp_RollbackImport 
	@SessionId = 1, 
	@UserId = 1
```

Returns:
- Result: SUCCESS/ERROR
- VouchersDeleted: Count of deleted vouchers
- EntriesDeleted: Count of deleted entries
- Message: Status message

### sp_ImportDataQualityCheck
Validate imported data quality.

```sql
EXEC sp_ImportDataQualityCheck @SessionId = 1
```

Returns warnings for:
- Accounts with no transactions
- Accounts with large balances (>1M threshold)
- Unbalanced vouchers
- Duplicate accounts in vouchers
- Overall trial balance mismatch

### sp_GetImportHistory
Get list of import sessions with filters.

```sql
EXEC sp_GetImportHistory 
	@ImportType = 'OPENING_BALANCE', -- Optional
	@FromDate = '2024-01-01',        -- Optional (default: 3 months ago)
	@ToDate = '2024-12-31'           -- Optional (default: today)
```

Returns:
- session_id, import_type, file_name
- Row counts (total, imported, skipped, errors)
- Status, timestamps
- imported_by_name
- can_rollback (boolean flag)
- voucher_count

### sp_ValidateOpeningBalanceImport
Pre-import validation for opening balances.

```sql
DECLARE @Balances ImportBalanceRow;

INSERT INTO @Balances (acc_code, dr_amount, cr_amount)
VALUES 
	('1110', 50000.00, 0),
	('2100', 0, 35000.00),
	('3000', 0, 15000.00);

EXEC sp_ValidateOpeningBalanceImport @BalanceRows = @Balances;
```

Returns:
- validation_status: PASSED/FAILED
- total_dr, total_cr, difference
- error_list: JSON array of errors

Also returns detailed error rows:
- ErrorType: ACCOUNT_NOT_FOUND, DUPLICATE_ACCOUNT, BALANCE_MISMATCH, etc.
- ErrorMessage: Description
- AccountCode: Related account code

## Data Quality Checks

The module performs comprehensive validation:

1. **Pre-Import Validation** (Opening Balance)
   - All accounts exist
   - No duplicate accounts
   - No negative amounts
   - Each account has debit OR credit (not both)
   - Total Debit = Total Credit

2. **Post-Import Quality Check**
   - Accounts with no transactions
   - Unusually large balances (>1M threshold)
   - Unbalanced vouchers
   - Duplicate account entries in same voucher
   - Overall trial balance verification

## Excel Templates

Templates are generated with:
- **Header row**: Bold, blue background, white text
- **Description row**: Column descriptions (Required, Optional, Format)
- **Sample data**: Gray background, 3 sample rows
- **Instructions sheet**: Step-by-step guide with validation rules

Templates use HTML-based Excel format (.xls):
- No EPPlus dependency required
- Opens natively in Excel
- Full formatting support
- Data validation instructions embedded

## Security & Audit

- All imports tracked in `acc_import_sessions`
- Rollback operations logged to `acc_audit_trail` (if table exists)
- Foreign key to `pos_users` for audit trail
- Rollback time window controlled by `rollback_available_until`

## Error Handling

- All stored procedures use `SET XACT_ABORT ON` for transaction safety
- Comprehensive error messages with context
- Rollback support for recovery
- Error logs stored as JSON in `error_log` column

## Next Steps

After database installation:

1. Build the POS.DLL project to compile `ImportTemplateGenerator.cs`
2. Create UI forms for import operations (not included in this schema)
3. Create BLL layer classes for import orchestration
4. Integrate with existing accounting module

## Testing

To test the installation:

```sql
-- 1. Verify tables exist
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME LIKE 'acc_import%';

-- 2. Verify stored procedures exist
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_NAME LIKE 'sp_%Import%' 
OR ROUTINE_NAME LIKE 'sp_Validate%';

-- 3. Test template generation (C#)
string templatePath = ImportTemplateGenerator.CreateImportTemplate("OPENING_BALANCE");
Console.WriteLine($"Template created: {templatePath}");
```

## Notes

- This module uses `acc_entries_header` and `acc_entries` tables (not `acc_vouchers`)
- Rollback window is configurable per import session
- Templates support bilingual instructions (can be extended)
- All monetary amounts use DECIMAL(18,2)
- Import types are enforced by CHECK constraints
- Session status transitions: InProgress → Completed/Failed → RolledBack (optional)
