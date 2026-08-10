# Professional Trial Balance Report

## Overview
A modern, user-friendly Trial Balance Report designed with professional UI/UX standards. This report displays a complete trial balance of all accounts with opening balances, total debits, credits, and closing balances. The totals row automatically highlights to provide clear visibility of the balance verification.

## Features

### 🎨 Professional Design
- Clean, modern interface with dark blue header (RGB: 41, 128, 185)
- Light gray filter panel for intuitive control organization
- Alternating row colors for improved readability
- Professional typography using Segoe UI font family
- Proper spacing and alignment for all elements

### 📊 Report Data
- **Account Name**: Full name of the general ledger account
- **Total Debit**: Sum of all debit entries for the period
- **Total Credit**: Sum of all credit entries for the period
- **Closing Balance**: Net balance (Debit - Credit)
- **Automatic Totals Row**: Highlighted summary showing column totals

### 🔍 Flexible Date Selection
- **Quick Presets**: 
  - Today
  - This Week / Last Week
  - This Month / Last Month
  - Last 3 Months / Last 6 Months
  - This Year / Year to Date (YTD)
- **Custom Dates**: Choose any date range with validation
- Date pickers show/hide automatically based on selection

### 📤 Export & Print Options
- **Print Report**: High-quality printing with DGVPrinter integration
  - Title and subtitle headers
  - Page numbers
  - Professional footer with timestamp
  - Proper column width and row height
- **Export to CSV**: Save data for further analysis in Excel or other tools
  - Handles special characters and formatting
  - Preserves all numeric formatting

### 🔐 Security & Logging
- User action logging for audit trail
- Branch-specific filtering via user session
- Permission-based menu access

## Usage Guide

### 1. Loading the Report
1. Navigate to **Finance → Reports → Trial Balance**
2. The report opens in a new dialog window

### 2. Setting Date Range
**Option A - Use Preset:**
- Select a date range from the dropdown (e.g., "This Month")
- Click "Load Report"

**Option B - Custom Date Range:**
- Select "Custom" from the dropdown
- Date pickers appear automatically
- Choose From Date and To Date
- Click "Load Report"

### 3. Understanding the Report

```
Account Name                    | Total Debit | Total Credit | Closing Balance
───────────────────────────────────────────────────────────────────────────
Cash at Bank                    |   50,000.00 |    40,000.00 |    10,000.00
Sales Account                   |            |   100,000.00 |  -100,000.00
Expenses                        |    30,000.00|            |    30,000.00
...
═══ TOTALS ═══                  |  250,000.00|   250,000.00|           0.00
```

**Key Points:**
- Debit balances typically show positive in Closing Balance
- Credit balances typically show negative in Closing Balance
- Total Debit and Total Credit should be equal (debit = credit rule)
- Closing Balance for all accounts should sum to zero (if balanced)

### 4. Exporting Data
1. Click "Export" button
2. Choose CSV or Excel format
3. Specify file location and name
4. Data is saved with proper formatting

### 5. Printing
1. Click "Print" button
2. Print preview window opens
3. Review page layout
4. Click Print to send to printer

## Professional UI/UX Features

### Color Scheme
- **Header**: Blue (RGB: 41, 128, 185) - Professional and trustworthy
- **Buttons**:
  - Load: Blue (action button)
  - Print: Green (RGB: 39, 174, 96) - Success/completed action
  - Export: Orange (RGB: 243, 156, 18) - Export/download action
- **Data Grid**:
  - Header: White on blue background
  - Rows: Alternating white and light blue
  - Selected: Medium blue highlighting
  - Totals: Bold white on blue background
- **Text**: Dark gray (RGB: 52, 73, 94) - Easy on the eyes

### Typography
- **Title**: 18pt Segoe UI Bold - Clear hierarchy
- **Headers**: 11pt Segoe UI Bold - Professional appearance
- **Body Text**: 10pt Segoe UI Regular - Readable and clean
- **Data Values**: 10pt with proper alignment (right-aligned for numbers)

### User Experience
- **Auto-hiding controls**: Date pickers hide/show based on selection mode
- **Clear visual feedback**: Buttons change color on hover (flat style)
- **Professional spacing**: Consistent padding and margins (15px)
- **Responsive grid**: Columns auto-size to content width
- **Summary highlighting**: Totals row is visually distinct with bold and colored background

## Technical Architecture

### Code Organization
- **Form Class**: `FrmTrialBalanceReport.cs`
  - Contains all UI logic and event handlers
  - Uses MVVM-light patterns for data binding
  - Implements proper error handling with user messages

- **Designer File**: `FrmTrialBalanceReport.Designer.cs`
  - Auto-generated (DO NOT EDIT manually)
  - Contains all control definitions
  - Maintains UI layout consistency

### Data Flow
```
User Interface (FrmTrialBalanceReport)
	↓
Business Logic (AccountsBLL.TrialBalanceReport)
	↓
Data Access (AccountsDLL)
	↓
SQL Database (sp_TrialBalance stored procedure)
	↓
Result → DataTable → Totals Addition → Grid Binding
```

### Key Methods

**LoadReport()**
- Validates date range
- Calls BLL to fetch trial balance data
- Adds automatic totals row
- Binds data to grid
- Logs user action

**AddTotalsRow()**
- Calculates sum of Debit column
- Calculates sum of Credit column
- Calculates sum of Balance column
- Appends formatted totals row

**BindGrid()**
- Clears existing grid data
- Binds DataTable to grid
- Configures column headers and formatting
- Applies number formatting (N2 for currency)
- Hides technical columns (IDs)

**FormatTotalsRow()**
- Applies special styling to totals row
- Sets background color to blue
- Sets font to bold
- Sets text color to white

## Performance Considerations

- **Large date ranges**: May require more processing time. Consider using smaller ranges for better performance.
- **Database indexing**: Ensure proper indexes on `entry_date` and `account_id` columns.
- **Memory usage**: CSV export processes data in memory; very large datasets may require optimization.

## Date Range Examples

| Selection | Result |
|-----------|--------|
| Today | Current date only |
| This Week | Monday to Sunday of current week |
| This Month | 1st to last day of current month |
| Year to Date | January 1st to today |
| Last 6 Months | 6 months backward from today |
| Custom | User-specified date range |

## Troubleshooting

### Issue: "No data found for the selected period"
**Cause**: No transactions in the database for the selected date range
**Solution**: 
- Verify transactions exist in the database
- Try a wider date range
- Check if accounts are properly configured

### Issue: Totals row shows incorrect values
**Cause**: Data integrity issue or calculation error
**Solution**:
- Refresh the report
- Check source data for corrupted entries
- Contact system administrator

### Issue: Column headers are cut off
**Cause**: Window is too small or display scaling issue
**Solution**:
- Maximize the window
- Adjust monitor display scaling
- Resize columns manually by dragging column borders

### Issue: Print output looks different
**Cause**: Printer driver or font availability
**Solution**:
- Ensure Segoe UI font is installed
- Check printer driver is up to date
- Use "Print Preview" before printing

## Keyboard Shortcuts
- **Ctrl+P**: Print (when report is loaded)
- **Ctrl+E**: Export (when report is loaded)
- **Tab**: Navigate between controls
- **Enter**: Load Report (from filter panel)

## System Requirements
- .NET Framework 4.8+
- Windows Forms support
- DGVPrinter library for printing functionality
- SQL Server database with accounting data

## Menu Location
**Finance → Reports → Trial Balance**

## Permissions
- Requires "Finance_Report" permission (if permission system is enabled)
- User can only view data for their assigned branch

## Audit Logging
All user actions are logged:
- View Report: Logged with date range
- Print Report: Logged with timestamp
- Export Report: Logged with filename

## Future Enhancements
- Export to PDF format
- Email report directly
- Schedule automated reports
- Account filtering (by type, group, etc.)
- Comparative analysis (period-over-period)
- Account details drill-down on double-click
- Custom report templates

## Support
For issues or feature requests, contact the system administrator or development team.

---

**Version**: 1.0  
**Last Updated**: 2024  
**Author**: Development Team  
**Status**: Production Ready ✓
