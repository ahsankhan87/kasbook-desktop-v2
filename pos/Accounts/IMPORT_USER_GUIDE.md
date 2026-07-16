# Opening Balance & Historical Data Import Module - User Guide

## Overview
The Import Data module (`frm_import_data`) is a professional, safety-first migration tool for one-time or rare imports from old accounting systems. It supports Chart of Accounts, Opening Balances, Customer/Supplier Balances, and Historical Journal Entries.

## Features
- ✅ **Excel-based Import** - Simple Excel templates with instructions
- ✅ **Preview & Validation** - See errors before importing
- ✅ **Dry Run Mode** - Test validation without committing data
- ✅ **Double-Entry Validation** - Ensures Dr = Cr for all journal postings
- ✅ **24-Hour Rollback** - Full undo capability for safety
- ✅ **Import History** - Track all imports with session management
- ✅ **Admin-Only Access** - Secured import functionality

---

## Database Setup

### 1. Install Import Module Schema

Run the following SQL scripts in order:

```sql
-- 1. Create import tables and session tracking
USE YourDatabase;
GO

EXEC sp_executesql N'
-- Import Sessions Table
CREATE TABLE acc_import_sessions (
	session_id INT IDENTITY(1,1) PRIMARY KEY,
	import_type VARCHAR(50) NOT NULL,
	file_name NVARCHAR(255),
	total_rows INT,
	imported_rows INT DEFAULT 0,
	skipped_rows INT DEFAULT 0,
	error_rows INT DEFAULT 0,
	status VARCHAR(20) DEFAULT ''InProgress'',
	error_log NVARCHAR(MAX),
	rollback_available_until DATETIME NULL,
	imported_by INT NOT NULL,
	imported_at DATETIME DEFAULT GETDATE()
);

-- Import Vouchers Link Table
CREATE TABLE acc_import_vouchers (
	id INT IDENTITY(1,1) PRIMARY KEY,
	session_id INT NOT NULL,
	voucher_id INT NOT NULL,
	FOREIGN KEY (session_id) REFERENCES acc_import_sessions(session_id)
);
';
```

### 2. Create Stored Procedures

Run `POS.DLL\Accounts\ImportModule_Procedures.sql` or create manually:

```sql
-- Create Table-Valued Parameter Type
CREATE TYPE dbo.ImportBalanceRow AS TABLE (
	acc_code VARCHAR(50),
	dr_amount DECIMAL(18,2),
	cr_amount DECIMAL(18,2)
);
GO

-- sp_ValidateOpeningBalanceImport
-- sp_RollbackImport
-- sp_GetImportHistory
-- sp_ImportDataQualityCheck
```

See `POS.DLL\Accounts\ImportModule_Procedures.sql` for full procedure definitions.

---

## Usage Guide

### Access the Import Form

```csharp
// From main menu or admin panel
var importForm = new frm_import_data();
importForm.ShowDialog();
```

### Tab 1: Chart of Accounts Import

**Purpose:** Import new accounts into the Chart of Accounts

**Steps:**
1. Click **⬇ Download Template**
2. Open Excel file and fill in:
   - Account Code (required, unique)
   - Account Name (required)
   - Account Type (Asset, Liability, Equity, Revenue, Expense)
   - Parent Code (optional, must exist)
   - Normal Balance (Dr or Cr)
   - Opening Balance (optional)
   - Is Bank Account (Yes/No)
   - Bank Name & Account No (if bank account)

3. Click **📁 Upload File** to preview
4. Review validation:
   - ✅ Valid rows (green/white)
   - ❌ Invalid rows (red background)

5. Click **Import** to add accounts

**Validation Rules:**
- Account Code must be unique
- Parent Code must exist (in system or earlier in file)
- Account Type must be valid
- Bank Name required if Is Bank Account = Yes

---

### Tab 2: Opening Balances

**Purpose:** Post opening balances for existing accounts (creates journal voucher)

**Steps:**
1. Set **Opening Balance Date** (e.g., start of fiscal year)
2. Click **⬇ Download Template** (lists all existing accounts)
3. Fill in Debit/Credit amounts for each account
4. Click **📁 Upload File** to preview

**Critical Validation:**
```
Total Debit = Total Credit (must balance!)
```

5. Review the balance summary:
   - Dr: 150,000.00 | Cr: 150,000.00 | Diff: 0.00 ✓ BALANCED

6. Click **Post** to create Opening Balance journal voucher

**Notes:**
- Each account can have EITHER Debit OR Credit (not both)
- The form will show: `X imported, Y skipped` (with error log if any)
- A single journal voucher is created (type: "Opening Balance")

---

### Tab 3: Customer/Supplier Balances

**Purpose:** Import outstanding invoices from old system

**Steps:**
1. Select **Customers** or **Suppliers** radio button
2. Click **⬇ Download Template**
3. Fill in Excel:
   - Party Code (must exist in system)
   - Invoice No
   - Invoice Date
   - Due Date
   - Amount
   - Outstanding Amount

4. Upload and import

**Result:**
- Creates "Opening Balance" type invoices
- Establishes proper AR/AP balances

---

### Tab 4: Historical Journal Entries

**Purpose:** Import last year's journals for comparison/reporting

**Steps:**
1. Click **⬇ Download Template**
2. Fill in Excel:
   - Voucher Date
   - Voucher No
   - Account Code (must exist)
   - Debit Amount
   - Credit Amount
   - Narration

3. Upload to preview

**Validation:**
- Each voucher (group of entries with same Voucher No) must balance
- All account codes must exist
- Dates within allowed range

**Summary Display:**
```
Vouchers: 50 | Entries: 250 | Balanced: 50 ✓
```

4. Click **Import** to create historical vouchers (type: "Historical Journal")

**Performance:**
- Imports in batches of 1,000 entries using SqlBulkCopy

---

### Tab 5: Import History & Rollback

**Purpose:** View all imports and rollback if needed

**Features:**

1. **Import History Grid:**
   - Type, File Name, Date, Status
   - Imported/Skipped/Error counts
   - Voucher count
   - Rollback availability

2. **Status Color Coding:**
   - 🟢 Green = Completed
   - 🔴 Red = Failed
   - 🟠 Orange = Rolled Back

3. **Rollback:**
   - Select a completed import (within 24 hours)
   - Click **Rollback Selected**
   - Confirm deletion

**Rollback Behavior:**
- Deletes ALL vouchers created by that import session
- Deletes ALL journal entries
- Updates session status to "RolledBack"
- Cannot rollback after 24-hour window expires

---

## Safety Features

### 1. Dry Run Mode
Check the **🛡 Dry Run Mode** checkbox at bottom of form:
- Validates everything
- Shows preview of what WOULD be imported
- **Does NOT commit any data**
- Use to test templates before actual import

### 2. Transaction Wrapping
All imports use SQL transactions:
- If ANY error occurs, entire import is rolled back
- Database remains consistent

### 3. Session Tracking
Every import creates a session record:
- Tracks who imported, when, and what file
- Links to all created vouchers
- Enables 24-hour rollback

### 4. Validation Layers
**Row-Level:**
- Required fields
- Data types
- Code existence
- Business rules

**Batch-Level:**
- Double-entry balance (Dr = Cr)
- Voucher balance
- Trial balance

### 5. Admin-Only Access
Only users with role = "Admin" or "Administrator" can access import form.

---

## Common Workflows

### Workflow 1: New Company Setup (Fresh Install)

```
1. Import Chart of Accounts
   └─> Creates account structure

2. Import Opening Balances
   └─> Sets initial balances (must balance!)

3. Import Customer Balances
   └─> Outstanding AR

4. Import Supplier Balances
   └─> Outstanding AP

5. Verify via Reports:
   - Trial Balance
   - AR/AP Aging
```

### Workflow 2: Migration from Old System

```
1. Export data from old system to Excel templates

2. Enable Dry Run Mode
   └─> Test all imports without committing

3. Fix validation errors in Excel

4. Disable Dry Run Mode

5. Import in order:
   a. Chart of Accounts
   b. Opening Balances
   c. Party Balances
   d. Historical Journals

6. Run sp_ImportDataQualityCheck
   └─> Validates data integrity

7. If issues found, rollback and retry
```

---

## Error Handling

### Upload Errors
- **"Failed to read file"**
  - Check Excel format (.xls or .xlsx)
  - Ensure file is not open in Excel
  - Verify column headers match template

### Validation Errors
- Invalid rows highlighted in **red**
- Error message shown in "Validation Error" column
- Fix in Excel and re-upload

### Import Errors
- Shown in result dialog
- Logged to session error_log
- View in Import History tab

---

## Data Quality Checks

After import, run quality check procedure:

```sql
EXEC sp_ImportDataQualityCheck;
```

**Checks:**
1. No-Transaction Accounts (orphaned)
2. Large Balances (> 1M warning)
3. Unbalanced Vouchers
4. Duplicate Entries
5. Trial Balance Mismatch

---

## Troubleshooting

### Problem: "Access Denied"
**Solution:** Only Admin users can import. Check `UsersModal.logged_in_user_role`.

### Problem: "Total Debit ≠ Total Credit"
**Solution:** Review all entries. Each account must have EITHER Dr OR Cr (not both).

### Problem: "Account Code does not exist"
**Solution:** Import Chart of Accounts first, or fix codes in Excel.

### Problem: "Voucher not balanced"
**Solution:** For each unique Voucher No, sum(Debit) must equal sum(Credit).

### Problem: "Rollback Not Available"
**Solution:**
- Check status is "Completed"
- Check rollback_available_until date (must be within 24 hours)
- Already rolled back imports cannot be rolled back again

---

## Technical Notes

### Excel Reading
- Uses OleDb provider (`Microsoft.ACE.OLEDB.12.0`)
- Supports both .xls and .xlsx
- Reads first worksheet

### Database Connection
- Uses existing `dbConnection.ConnectionString`
- All operations use `SqlConnection` / `SqlCommand`
- Bulk inserts use `SqlBulkCopy` for performance

### Performance
- Chart of Accounts: ~100 rows/sec
- Opening Balance: Single transaction (fast)
- Historical Journals: 1,000 entries/batch

### Logging
- Session tracking in `acc_import_sessions`
- Voucher links in `acc_import_vouchers`
- Error details in `error_log` column (first 100 errors)

---

## Template Locations

Templates are generated dynamically and saved to:
```
%TEMP%\ImportTemplate_{Type}_{Timestamp}.xls
```

Templates include:
- Column headers
- Instructions row (row 2)
- Sample data row

---

## API Reference

### ImportBLL Methods

```csharp
// Chart of Accounts
public ImportResultModal ImportChartOfAccounts(
	List<ChartOfAccountsImportRow> rows, 
	string fileName, 
	ImportConfigModal config)

// Opening Balance
public ImportResultModal PostOpeningBalance(
	List<OpeningBalanceImportRow> rows, 
	DateTime voucherDate, 
	string fileName, 
	ImportConfigModal config)

// Historical Journals
public ImportResultModal ImportJournalEntries(
	List<JournalEntryImportRow> rows, 
	string fileName, 
	ImportConfigModal config)

// Rollback
public ImportResultModal RollbackImport(int sessionId)
```

### ImportConfigModal

```csharp
public class ImportConfigModal
{
	public bool DryRunMode { get; set; }           // Default: false
	public bool SkipValidationErrors { get; set; } // Default: false
	public int BatchSize { get; set; }             // Default: 1000
	public int RollbackHours { get; set; }         // Default: 24
}
```

---

## Security Considerations

1. **Admin-Only:** Form checks `UsersModal.logged_in_user_role` on load
2. **Audit Trail:** All imports logged with user ID and timestamp
3. **Rollback Window:** 24-hour limit prevents accidental deletion of old data
4. **Transaction Safety:** All-or-nothing commit prevents partial imports
5. **Validation:** Prevents invalid data from entering system

---

## Support & Maintenance

### Regular Maintenance
- Review `acc_import_sessions` table monthly
- Archive old sessions (status = "Completed" or "RolledBack")
- Monitor error logs for recurring issues

### Troubleshooting Queries

```sql
-- Recent imports
SELECT TOP 10 * FROM acc_import_sessions 
ORDER BY imported_at DESC;

-- Failed imports
SELECT * FROM acc_import_sessions 
WHERE status = 'Failed';

-- Imports pending rollback expiry
SELECT * FROM acc_import_sessions 
WHERE status = 'Completed' 
  AND rollback_available_until > GETDATE();
```

---

## Version History

- **v3.0.0** - Initial release
  - Chart of Accounts import
  - Opening Balance posting
  - Party balance import (Customer/Supplier)
  - Historical journal import
  - 24-hour rollback
  - Dry run mode
  - Import session tracking

---

**Important:** Always backup your database before performing large imports!

For questions or issues, contact your system administrator.
