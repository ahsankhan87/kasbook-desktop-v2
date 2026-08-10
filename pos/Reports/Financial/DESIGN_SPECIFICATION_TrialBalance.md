# Trial Balance Report - Design Specification

## Visual Layout

```
┌─────────────────────────────────────────────────────────────────────────┐
│ Trial Balance Report                                                    │
│ (Dark Blue Header - RGB: 41, 128, 185)                                 │
│ 70px Height                                                              │
└─────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────┐
│ Filter Panel (Light Gray Background - RGB: 236, 240, 241)              │
│ 140px Height                                                             │
│                                                                          │
│ Date Range: [This Month ▼]      [Custom Dates Hidden]                  │
│ [Load Report]  [Print] [Export]                                        │
│                                                                          │
└─────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────┐
│ Account Name              │ Total Debit │ Total Credit │ Closing Balance│
│ (Column Header - White Text on Blue, Bold, 45px height)               │
├─────────────────────────────────────────────────────────────────────────┤
│ Cash at Bank              │    50,000.00│    40,000.00 │     10,000.00  │
├─────────────────────────────────────────────────────────────────────────┤
│ Sales Account             │             │   100,000.00 │   -100,000.00  │
├─────────────────────────────────────────────────────────────────────────┤
│ Expenses                  │    30,000.00│             │     30,000.00   │
├─────────────────────────────────────────────────────────────────────────┤
│ ... more rows (alternating background colors) ...                       │
├─────────────────────────────────────────────────────────────────────────┤
│ ═══ TOTALS ═══             │   250,000.00│   250,000.00 │          0.00  │
│ (Blue Background, White Bold Text)                                      │
└─────────────────────────────────────────────────────────────────────────┘
```

## Color Palette

### Primary Colors
| Usage | RGB Values | Hex | Purpose |
|-------|-----------|-----|---------|
| Header Background | 41, 128, 185 | #2980B9 | Professional, trustworthy |
| Primary Button | 41, 128, 185 | #2980B9 | Main action (Load) |
| Success Button | 39, 174, 96 | #27AE60 | Print action |
| Warning Button | 243, 156, 18 | #F39C12 | Export action |

### Secondary Colors
| Usage | RGB Values | Hex | Purpose |
|-------|-----------|-----|---------|
| Filter Panel | 236, 240, 241 | #ECF0F1 | Light background |
| Grid Header Text | 255, 255, 255 | #FFFFFF | Contrast with header |
| Grid Body Text | 52, 73, 94 | #34495E | Dark text, readable |
| Alternating Row | 245, 248, 250 | #F5F8FA | Light blue-gray |
| Selection Highlight | 155, 195, 228 | #9BC3E4 | Medium blue |
| Totals Row | 41, 128, 185 | #2980B9 | Same as header |

## Typography

### Font Family
**Segoe UI** (system font, universally available)

### Font Sizes & Weights
| Element | Size | Weight | Color |
|---------|------|--------|-------|
| Form Title | 18pt | Bold | White (#FFFFFF) |
| Column Headers | 11pt | Bold | White (#FFFFFF) |
| Label Text | 10pt | Bold | Dark gray (#34495E) |
| Data Values | 10pt | Regular | Dark gray (#34495E) |
| Totals Row | 11pt | Bold | White (#FFFFFF) |

## Component Specifications

### Header Panel (pnlTop)
- **Height**: 70px
- **Background**: RGB(41, 128, 185)
- **Padding**: 20px left margin
- **Content**: "Trial Balance Report" title
- **Font**: 18pt Segoe UI Bold, white

### Filter Panel (pnlFilters)
- **Height**: 140px
- **Background**: RGB(236, 240, 241)
- **Padding**: 15px all sides
- **Elements**:
  - Date Range Label & ComboBox (200px wide)
  - Date Picker Panel (conditionally visible)
  - Load Button (110px × 36px)
  - Print Button (110px × 36px)
  - Export Button (110px × 36px)

### Date Range ComboBox
- **Width**: 200px
- **Height**: 25px
- **Style**: DropDownList (read-only)
- **Font**: 10pt Segoe UI
- **Items**:
  - Custom
  - Today
  - This Week / Last Week
  - This Month / Last Month
  - Last 3 Months / Last 6 Months
  - This Year
  - Year to Date (YTD)

### Date Picker Controls (pnlDatePickers)
- **Visibility**: Hidden by default, shown when "Custom" selected
- **Layout**: Two DateTimePicker controls side-by-side
- **From Date**: Position (0, 20)
- **To Date**: Position (170, 20)
- **Spacing**: 20px between controls

### Buttons
All buttons use **FlatStyle** for modern appearance

| Button | Width | Height | Color | Font |
|--------|-------|--------|-------|------|
| Load | 110px | 36px | Blue | 10pt Bold |
| Print | 110px | 36px | Green | 10pt Bold |
| Export | 110px | 36px | Orange | 10pt Bold |

### DataGridView (dgvReport)
- **Width**: Fill available space
- **Height**: Remaining window space
- **Background**: White
- **Border**: None (clean look)
- **Cell Border**: Single horizontal lines only
- **Selection Mode**: Full row select
- **Row Height**: 28px
- **Header Height**: 45px

#### Column Specifications

| Column | Header Text | Width | Alignment | Format |
|--------|-------------|-------|-----------|--------|
| AccountName | Account Name | 300px | Left | Text |
| TotalDebit | Total Debit | 150px | Right | N2 (currency) |
| TotalCredit | Total Credit | 150px | Right | N2 (currency) |
| ClosingBalance | Closing Balance | 150px | Right | N2 (currency) |
| AccountID | (Hidden) | - | - | - |

#### Row Styling
- **Normal Rows**:
  - Background: Alternating white and light blue (RGB: 245, 248, 250)
  - Text: Dark gray (RGB: 52, 73, 94)
  - Font: 10pt Segoe UI
  - Height: 28px

- **Header Row**:
  - Background: Blue (RGB: 41, 128, 185)
  - Text: White, Bold
  - Font: 11pt Segoe UI Bold
  - Height: 45px
  - Alignment: Center

- **Totals Row**:
  - Background: Blue (RGB: 41, 128, 185)
  - Text: White, Bold
  - Font: 11pt Segoe UI Bold
  - Marker Text: "═══ TOTALS ═══"

## Spacing & Layout

### Margins & Padding
| Element | Margin | Padding |
|---------|--------|---------|
| Form | 0 | 0 |
| Header Panel | 0 | 0 |
| Filter Panel | 0 | 15px |
| Button Group | - | 15px spacing |
| Grid | 0 | 0 |

### Button Positioning
- **Load Button**: X: 780px, Y: 52px
- **Print Button**: X: 910px, Y: 52px (130px spacing)
- **Export Button**: X: 1040px, Y: 52px (130px spacing)
- Form Width: 1200px

## Default Form Dimensions
- **Width**: 1200px (HD-friendly)
- **Height**: 600px (minimum)
- **Position**: CenterScreen
- **State**: Normal (maximizable, resizable)

## Interactive Behaviors

### Date Range ComboBox
- **Default Selection**: "This Month" (index 4)
- **On Selection Change**:
  - If "Custom": Show date pickers, keep existing values
  - If Preset: Hide date pickers, auto-calculate dates, auto-load report

### Date Pickers
- **From Date**:
  - Default: Today - 1 month
  - Triggers "To Date Min Date" update
  - Min Date: 01/01/2000

- **To Date**:
  - Default: Today
  - Min Date: Same as From Date
  - Max Date: Today

### Load Report Button
- **OnClick**: 
  1. Validate date range (from ≤ to)
  2. Show busy indicator
  3. Fetch data from database
  4. Add totals row
  5. Bind to grid
  6. Format cells
  7. Hide busy indicator
  8. Log user action

### Print Button
- **OnClick** (if data loaded):
  1. Open print preview with DGVPrinter
  2. Show: Title, date range, page numbers, footer
  3. Fit columns to width
  4. Log print action

### Export Button
- **OnClick** (if data loaded):
  1. Open SaveFileDialog
  2. Default name: TrialBalance_YYYYMMDD_HHMMSS
  3. Export to CSV (Excel compatible)
  4. Show success message
  5. Log export action

## Accessibility

### Keyboard Navigation
- **Tab Order**:
  1. Date Range ComboBox
  2. From Date Picker (if visible)
  3. To Date Picker (if visible)
  4. Load Button
  5. Print Button
  6. Export Button
  7. Data Grid

- **Keyboard Shortcuts**:
  - Ctrl+P: Print
  - Enter: Load Report
  - Escape: Close form

### Screen Reader Support
- All labels have associated controls via "for" relationship
- Column headers have descriptive text
- Buttons have clear text labels

### Color Contrast
- Header: White (255) on Blue (41, 128, 185) ✓ WCAG AA
- Data: Dark Gray (52, 73, 94) on White (255) ✓ WCAG AA
- Totals: White (255) on Blue (41, 128, 185) ✓ WCAG AA

## Responsive Behavior

### On Form Resize
- **Width Decrease** (< 1000px):
  - Button spacing reduces proportionally
  - Columns auto-adjust to fit

- **Height Decrease** (< 400px):
  - Minimum grid rows shown
  - Vertical scrollbar appears

### DPI Scaling
- Form respects system DPI settings
- Text remains readable at 125%, 150% scaling
- All components use relative positioning

## Error & Validation States

### Invalid Date Range
- **Condition**: From Date > To Date
- **Visual**: Warning message box appears
- **Action**: Report does not load
- **Message**: "From date cannot be after To date"

### No Data Available
- **Condition**: No transactions in database for period
- **Visual**: Info message box appears
- **Grid**: Remains empty
- **Message**: "No trial balance data found for the selected period"

### Load in Progress
- **Visual**: Busy overlay with spinner
- **Text**: "Loading Trial Balance Report..."
- **Interaction**: Form is disabled until complete

### Load Error
- **Condition**: Database error or network issue
- **Visual**: Error message box appears
- **Details**: Full error message provided
- **Action**: User can retry with different parameters

## Animation & Transitions

- **Date Pickers Visibility**:
  - Smooth show/hide (if implemented)
  - Or: Immediate toggle

- **Row Selection**:
  - Highlight changes on mouse hover (standard Windows)

- **Button Hover**:
  - Color intensity increases (FlatStyle default)
  - Cursor changes to pointer

## Print Template

### Page Layout
```
┌────────────────────────────────────────┐
│  Trial Balance Report                  │
│  From 01/01/2024 To 12/31/2024         │
│                                        │
│ Account Name    | Debit  | Credit | Balance
├────────────────────────────────────────┤
│ [Data Rows]                            │
├────────────────────────────────────────┤
│ ═══ TOTALS ═══   | [Total Fields]      │
│                                        │
│                                        │
│ Generated by Kasbook - 01/15/2024 10:30
└────────────────────────────────────────┘
				  Page 1 of X
```

### Print Formatting
- **Margins**: Standard (1 inch all sides)
- **Page Breaks**: Automatic at bottom
- **Fonts**: 10pt Segoe UI (same as screen)
- **Footer**: Timestamp and application name
- **Header**: Title and date range on each page

---

## Implementation Notes

1. **Use ThemeHelper.Apply()** for consistent styling across the application
2. **Use UiMessages** for all user notifications
3. **Use BusyScope** for loading indicators
4. **Use DGVPrinter** for printing functionality
5. **Log all user actions** via POS.DLL.Log
6. **Respect user's branch** from UsersModal.logged_in_branch_id
7. **Handle null/DBNull values** in data binding
8. **Test with large datasets** (100+ accounts)
9. **Verify print output** on common printers
10. **Monitor performance** with date ranges > 1 year

---

**Status**: Design Complete ✓  
**Version**: 1.0  
**Approval**: Ready for Implementation
