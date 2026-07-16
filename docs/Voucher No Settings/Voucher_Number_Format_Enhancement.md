# Voucher Number Format Enhancement - Implementation Summary

## Overview
Enhanced the accounting settings form to support advanced voucher numbering formats that include:
- Prefix (e.g., "S" for Sales)
- Branch ID (from the logged-in user's branch)
- Date Format (customizable, e.g., YYYYMMDD)
- Number Format (counter with reset options)

## Format Structure
**Pattern:** `[Prefix][BranchId]-[DateFormat]-[NumberFormat]`

**Example:** `S1-20260713-0001`
- `S` = Prefix (configurable per voucher type)
- `1` = Branch ID (auto-populated from user session)
- `20260713` = Date (YYYYMMDD format in this example)
- `0001` = Counter (starting from configured number, resets based on policy)

## Date Format Options
Users can specify date formats using the following placeholders:
- `YYYY` = 4-digit year (e.g., 2026)
- `YY` = 2-digit year (e.g., 26)
- `MM` = 2-digit month (e.g., 07)
- `DD` = 2-digit day (e.g., 13)

**Examples:**
- `YYYYMMDD` → 20260713
- `YYYY-MM-DD` → 2026-07-13
- `YYMMDD` → 260713
- `DD/MM/YYYY` → 13/07/2026
- Leave blank to omit date from voucher number

## Files Modified

### 1. `pos\Accounts\Settings\frm_accounting_settings.cs`
**Changes:**
- Added new columns to grid: `colVoucherBranchId` and `colVoucherDateFormat`
- Updated `LoadVoucherGrid()` to populate Branch ID from current user's session
- Enhanced `RefreshVoucherPreview()` to read date format and branch ID from grid
- Modified `BuildVoucherPreview()` signature to accept branch ID and date format parameters
- Implemented date format parsing logic to support flexible date patterns
- Updated `SaveVoucherSettings()` to persist the new date format settings to the database

**Key Method - BuildVoucherPreview():**
```csharp
private static string BuildVoucherPreview(string prefix, int branchId, string format, 
										 string dateFormat, int number)
{
	// Supports flexible date formatting with YYYY, YY, MM, DD placeholders
	// Combines all components: Prefix + BranchId + DatePart + NumberPart
}
```

### 2. `pos\Accounts\Settings\frm_accounting_settings.Designer.cs`
**Changes:**
- Added `colVoucherBranchId` column declaration (TextBox, read-only)
- Added `colVoucherDateFormat` column declaration (TextBox)
- Updated grid column initialization to include new columns
- Added field declarations for new columns in the designer partial class

**Column Order in Grid:**
1. Voucher Type (read-only)
2. Prefix
3. Branch ID (auto-populated, read-only)
4. Number Format (dropdown: YYYY-NNNN, YY-NNNN, NNNN)
5. Date Format (free text, e.g., YYYYMMDD)
6. Reset (dropdown: Daily, Annually, Never, Per Financial Year)
7. Starting Number
8. Preview (read-only)

## Database Schema Updates
New settings keys stored in `pos_settings` table:
- `ACC_VOUCHER_JV_DATE_FORMAT`
- `ACC_VOUCHER_RECEIPT_DATE_FORMAT`
- `ACC_VOUCHER_PAYMENT_DATE_FORMAT`
- `ACC_VOUCHER_IBT_DATE_FORMAT`
- `ACC_VOUCHER_ADJ_DATE_FORMAT`

## User Experience

### Setting Up Voucher Numbers
1. Open **Accounting Settings** form (Admin/CFO role required)
2. Go to **Voucher Configuration** tab
3. For each voucher type (JV, Receipt, Payment, IBT, Adj):
   - Set the **Prefix** (e.g., "S" for sales)
   - **Branch ID** is auto-populated (read-only)
   - Select **Number Format** (YYYY-NNNN, YY-NNNN, or NNNN)
   - Enter **Date Format** (e.g., YYYYMMDD or leave blank)
   - Select **Reset** frequency
   - Enter **Starting Number**
4. The **Preview** column shows a live example (e.g., S1-20260713-0001)
5. Click **Save Settings** to persist changes

### Example Configurations

**Sales Invoices:**
- Prefix: S
- Date Format: YYYYMMDD
- Number Format: YYYY-NNNN
- Result: S1-20260713-2026-0001

**Journal Vouchers:**
- Prefix: JV
- Date Format: (blank)
- Number Format: YYYY-NNNN
- Result: JV1-2026-0001

**Cash Receipts:**
- Prefix: CR
- Date Format: YYYYMMDD
- Number Format: NNNN
- Result: CR1-20260713-0001

## Settings Service Integration
The `AccountingSettingsService` retrieves and caches these settings:
- `_settings.GetString("ACC_VOUCHER_[TYPE]_DATE_FORMAT", defaultValue)`
- Settings are auto-saved when form Save button is clicked

## Live Preview Updates
The grid's Preview column updates in real-time as users edit:
- Prefix
- Date Format
- Number Format
- Starting Number

This helps users verify their voucher numbering scheme before saving.

## Build Status
✅ Successfully compiled (no warnings or errors)
- `pos\POS.csproj` ✓
- `POS.BLL\POS.BLL.csproj` ✓
- `POS.DLL\POS.DLL.csproj` ✓
- `POS.Core\POS.Core.csproj` ✓

## Next Steps for Testing
1. Open the Accounting Settings form
2. Navigate to "Voucher Configuration" tab
3. Verify:
   - Branch ID column is populated with current branch ID
   - Date Format column is editable
   - Preview updates when you edit Date Format
   - Example: Type "YYYYMMDD" and see preview like "S1-20260713-0001"
4. Save and reload the form to verify persistence

## Backward Compatibility
- Existing voucher configurations remain unchanged
- Date Format field defaults to empty (no date in voucher number)
- Previous voucher format (PREFIX-NNNN or PREFIX-YYYY-NNNN) still works
- New date format feature is optional and additive
