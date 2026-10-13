# Estimate Conversion Status Visibility Feature

## Overview
This feature makes it clear to users which estimates have been converted to regular sales invoices. When an estimate is loaded into the sales form and converted to a sales invoice, the system now:
1. Updates the estimate status to mark it as "Converted"
2. Stores the linked sales invoice number
3. Displays visual indicators in the UI
4. Logs the conversion for audit purposes

## Database Schema Changes Required

The `pos_estimates` table needs the following columns (if not already present):
```sql
-- Add these columns to pos_estimates table if they don't exist
ALTER TABLE pos_estimates ADD linked_sales_invoice_no NVARCHAR(50) NULL;
ALTER TABLE pos_estimates ADD converted_date DATETIME NULL;
ALTER TABLE pos_estimates ADD converted_by INT NULL;
```

## Implementation Details

### 1. Backend Changes

#### EstimatesDLL.cs (`POS.DLL\POS\EstimatesDLL.cs`)
- **New Method**: `UpdateEstimateStatusToConverted(string estimateInvoiceNo, string linkedSalesInvoiceNo)`
  - Updates `pos_estimates.status = 1` when an estimate is converted
  - Records the linked sales invoice number
  - Records conversion timestamp and user ID
  - Used internally by the conversion process

#### EstimatesBLL.cs (`POS.BLL\POS\EstimatesBLL.cs`)
- **New Method**: `UpdateEstimateStatusToConverted(string estimateInvoiceNo, string linkedSalesInvoiceNo)`
  - BLL wrapper for the DLL method
  - Follows existing layered architecture pattern

#### SalesBLL.cs (`POS.BLL\POS\SalesBLL.cs`)
- **New Method**: `UpdateEstimateStatusToConverted(string estimateInvoiceNo, string linkedSalesInvoiceNo)`
  - High-level wrapper for sales-related conversions
  - Called from sales save operations

#### SalesDLL.cs (`POS.DLL\POS\SalesDLL.cs`)
- **Modified Method**: `InsertSales(List<SalesModalHeader> sales, List<SalesModal> sales_detail)`
  - After successful transaction commit, checks if `sale_header.estimate_invoice_no` is not null
  - Calls `EstimatesDLL.UpdateEstimateStatusToConverted()` to mark the estimate as converted
  - **Audit Logging**:
	- Logs "Convert Estimate" action: `Estimate: {estimateNo} converted to Sale: {saleNo}`
	- Logs "Convert Estimate Error" if conversion fails (non-blocking)
  - Ensures conversion happens AFTER the sale is successfully saved

### 2. UI Changes

#### frm_all_estimates.cs (`pos\Estimates\frm_all_estimates.cs`)
- **New Method**: `ApplyEstimateStatusFormatting()`
  - Reads the `status` column from the grid DataSource
  - Converts numeric status (0/1) to user-friendly text ("Active" / "Converted")
  - **Visual Indicator**: Rows with converted estimates are highlighted with light green background
  - Applied after data binding in:
	- `load_all_estimates_grid()`
	- `btn_search_Click()` (search results)

**User Experience**:
- Users can immediately see which estimates are active (can be converted) vs already converted
- Converted estimates have distinct visual styling (green background)
- Status text changes from numeric to descriptive

#### frm_search_estimates.cs (`pos\Estimates\frm_search_estimates.cs`)
- **Existing Behavior Preserved**: Already uses `GetAllActiveEstimates()`
  - Filters the search grid to show only `status = 0` estimates
  - Prevents users from accidentally re-converting an already converted estimate
  - Provides clean UX: users only see estimates available for conversion

#### frm_estimates_detail.cs (`pos\Estimates\frm_estimates_detail.cs`)
- **New Method**: `DisplayEstimateConversionStatus()`
  - Queries `pos_estimates` table for the specific estimate
  - Checks `status`, `linked_sales_invoice_no`, and `converted_date` columns
  - **If Converted**: Shows an info dialog with:
	- Original message: "This estimate was converted to Sales Invoice: {invoiceNo} on {date}"
	- Arabic translation provided
  - **If Not Converted**: No message (silent, allows normal detail view)
  - Called early in form load, before grid population
  - Uses bilingual message via `UiMessages.ShowInfo()` (follows team conventions)

**User Experience**:
- When user double-clicks a converted estimate in the list, they see a notification
- Shows which sales invoice was created from the estimate
- Shows conversion timestamp
- Supports both English and Arabic

## Data Flow: Estimate Conversion

```
User in frm_sales
  ↓
Click "Load Estimate" button
  ↓
frm_search_estimates opens (shows only unconverted estimates)
  ↓
User selects estimate → salesForm.Load_products_to_grid_by_invoiceno()
  ↓
User populates sales details and clicks Save
  ↓
frm_sales.Save_Sales() → SalesObj.InsertSales()
  ↓
SalesDLL.InsertSales() 
  ├─ Insert sale header to pos_sales ✓
  ├─ Insert sale items to pos_sales_items ✓
  ├─ Commit transaction ✓
  └─ FOR EACH sale_header:
	   ├─ Log "Add Sales" audit
	   ├─ IF estimate_invoice_no is not null:
	   │   ├─ EstimatesDLL.UpdateEstimateStatusToConverted()
	   │   │   └─ UPDATE pos_estimates SET status=1, linked_sales_invoice_no, converted_date, converted_by
	   │   └─ Log "Convert Estimate" audit ✓
	   └─ Continue to auto-post if enabled
  ↓
User refreshes estimates list
  ↓
frm_all_estimates grid now shows converted estimate with:
  - Green background
  - "Converted" status text
  - NOT in frm_search_estimates anymore
```

## Backward Compatibility

- **Graceful Degradation**: If the new columns don't exist in database:
  - `UpdateEstimateStatusToConverted()` will fail with SQL error (non-blocking)
  - Conversion logging will log the error but not break the sale save
  - UI formatting methods use try-catch to silently handle missing columns
  - System continues to function with older database schema

- **Migration Path**:
  - Run SQL ALTER statements above to add missing columns
  - No code changes needed after that
  - Existing estimates remain with status = 0

## Permissions & Authorization

- No new permissions required
- Existing sales entry permissions control access
- Estimate conversion is automatic, non-blocking

## Audit Trail

All conversions are logged via `Log.LogAction()`:
- Action: "Convert Estimate"
- Details: "Estimate: {estimateNo} converted to Sale: {saleNo}"
- User ID: Current logged-in user
- Branch ID: Current branch
- Timestamp: Automatic

Errors are also logged:
- Action: "Convert Estimate Error"
- Details: Full error message
- Allows investigation of any failed conversions

## Testing Checklist

- [ ] Create an estimate in the system
- [ ] Verify estimate appears in frm_all_estimates with "Active" status and white background
- [ ] Load the estimate into sales form via frm_search_estimates
- [ ] Create a sales invoice from the loaded estimate
- [ ] Refresh frm_all_estimates grid
- [ ] Verify the estimate now shows "Converted" status with green background
- [ ] Verify the estimate no longer appears in frm_search_estimates
- [ ] Double-click the converted estimate in frm_all_estimates
- [ ] Verify info dialog shows linked sales invoice number and conversion date
- [ ] Check audit logs to confirm "Convert Estimate" action is logged
- [ ] Test with multiple estimates being converted
- [ ] Verify search functionality still works on converted estimates
- [ ] Verify Arabic language support works correctly

## Files Modified

1. `POS.DLL\POS\EstimatesDLL.cs` - Added UpdateEstimateStatusToConverted method
2. `POS.BLL\POS\EstimatesBLL.cs` - Added UpdateEstimateStatusToConverted wrapper
3. `POS.BLL\POS\SalesBLL.cs` - Added UpdateEstimateStatusToConverted wrapper
4. `POS.DLL\POS\SalesDLL.cs` - Modified InsertSales to call conversion and logging
5. `pos\Estimates\frm_all_estimates.cs` - Added status formatting and visual indicators
6. `pos\Estimates\frm_estimates_detail.cs` - Added conversion status display notification

## No Breaking Changes

- All changes are additive
- Existing functionality preserved
- Error handling ensures robustness
- Graceful degradation if schema columns missing
- Fully backward compatible with existing sales flow
