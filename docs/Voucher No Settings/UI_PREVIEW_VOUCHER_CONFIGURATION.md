# User Interface Preview - Voucher Configuration

## Accounting Settings Form - Voucher Configuration Tab

### Complete View
```
╔════════════════════════════════════════════════════════════════════════════════╗
║ Accounting Settings                                                            │
├────────────────────────────────────────────────────────────────────────────────┤
│ │ Company │ Defaults │ Accounts │ Posting Rules │ Reports │ Vouchers │ Tax   │
├─┴────────────────────────────────────────────────────────────────────────────────┤
│                                                                                    │
│ Voucher Configuration                                                            │
│ Configure your voucher numbering scheme for all transaction types                │
│                                                                                    │
│ ┌─────────────────────────────────────────────────────────────────────────────┐ │
│ │ Type    │ Prefix │ Branch │ Number   │ Date      │ Reset    │ Start │Preview│ │
│ │         │        │ ID     │ Format   │ Format    │          │       │       │ │
│ ├─────────┼────────┼────────┼──────────┼───────────┼──────────┼───────┼───────┤ │
│ │ JV      │ JV     │ 1      │ YYYY-NNN │ YYYYMMDD  │ Annually │ 1     │ JV1-  │ │
│ │ (RO)    │        │ (RO)   │ N        │           │          │       │ 20260 │ │
│ │         │        │        │          │           │          │       │ 713-2 │ │
│ │         │        │        │          │           │          │       │ 026-0 │ │
│ │         │        │        │          │           │          │       │ 001   │ │
│ ├─────────┼────────┼────────┼──────────┼───────────┼──────────┼───────┼───────┤ │
│ │ RECEIPT │ RV     │ 1      │ YYYY-NNN │           │ Annually │ 1     │ RV1-  │ │
│ │ (RO)    │        │ (RO)   │ N        │ (blank)   │          │       │ 2026- │ │
│ │         │        │        │          │           │          │       │ 0001  │ │
│ ├─────────┼────────┼────────┼──────────┼───────────┼──────────┼───────┼───────┤ │
│ │ PAYMENT │ PV     │ 1      │ YYYY-NNN │           │ Annually │ 1     │ PV1-  │ │
│ │ (RO)    │        │ (RO)   │ N        │ (blank)   │          │       │ 2026- │ │
│ │         │        │        │          │           │          │       │ 0001  │ │
│ ├─────────┼────────┼────────┼──────────┼───────────┼──────────┼───────┼───────┤ │
│ │ IBT     │ IBT    │ 1      │ YYYY-NNN │           │ Annually │ 1     │ IBT1- │ │
│ │ (RO)    │        │ (RO)   │ N        │ (blank)   │          │       │ 2026- │ │
│ │         │        │        │          │           │          │       │ 0001  │ │
│ ├─────────┼────────┼────────┼──────────┼───────────┼──────────┼───────┼───────┤ │
│ │ ADJ     │ ADJ    │ 1      │ YYYY-NNN │ YYYYMMDD  │ Annually │ 1     │ ADJ1- │ │
│ │ (RO)    │        │ (RO)   │ N        │           │          │       │ 20260 │ │
│ │         │        │        │          │           │          │       │ 713-2 │ │
│ │         │        │        │          │           │          │       │ 026-0 │ │
│ │         │        │        │          │           │          │       │ 001   │ │
│ └─────────┴────────┴────────┴──────────┴───────────┴──────────┴───────┴───────┘ │
│                                                                                    │
│ [ Save Settings ] [ Reset Defaults ]                                            │
│                                                                                    │
└────────────────────────────────────────────────────────────────────────────────────┘
```

---

## Column Descriptions

### 1. Type (Read-Only)
```
Shows voucher type: JV, RECEIPT, PAYMENT, IBT, ADJ
Cannot be modified
Used for identifying which transaction type
```

### 2. Prefix
```
Custom code for voucher identification
Examples:
  - JV (Journal Voucher)
  - RV (Receipt Voucher)
  - PV (Payment Voucher)
  - S (Sales)
  - CR (Cash Receipt)

Editable text field
```

### 3. Branch ID (Auto-Populated, Read-Only)
```
Automatically filled from logged-in user's branch
Example: 1, 2, 3, etc.

Red-Only - Cannot be edited
Auto-updated when user logs in from different branch
Ensures multi-branch safety
```

### 4. Number Format
```
Dropdown with options:
  ✓ YYYY-NNNN  (Year + 4-digit counter, e.g., 2026-0001)
  ✓ YY-NNNN    (2-digit year + counter, e.g., 26-0001)
  ✓ NNNN       (Counter only, e.g., 0001)

Default: YYYY-NNNN
```

### 5. Date Format (NEW!)
```
Free-text field for date pattern
Use placeholders:
  YYYY = 4-digit year (2026)
  YY   = 2-digit year (26)
  MM   = 2-digit month (07)
  DD   = 2-digit day (13)

Examples:
  ✓ YYYYMMDD     → 20260713
  ✓ YYYY-MM-DD   → 2026-07-13
  ✓ DD/MM/YYYY   → 13/07/2026
  ✓ (blank)      → No date (omit from number)

Optional field
```

### 6. Reset
```
Dropdown with reset frequency options:
  ✓ Daily            (Restarts counter each day)
  ✓ Annually         (Restarts each year)
  ✓ Never            (Counter never resets)
  ✓ Per Financial Yr (Restarts on FY date)

Default: Annually
```

### 7. Starting Number
```
Initial counter value
Examples:
  1, 100, 1000, etc.

Numeric field
Default: 1
```

### 8. Preview (Read-Only)
```
Shows real example of generated voucher number
Updates live as you type
Format: [Prefix][BranchId]-[Date]-[Counter]

Examples:
  S1-20260713-2026-0001
  RV1-2026-0001
  PV1-2026-07-13-0100

Read-Only - Shows example with today's date
```

---

## Editing Workflow

### Step 1: Open Settings
```
Settings → Accounting Settings
(Admin/CFO access only)
```

### Step 2: Select Voucher Configuration Tab
```
Click on "Vouchers" or "Voucher Configuration" tab
```

### Step 3: Edit Row
```
For Sales (JV row):
  1. Click on Prefix cell → Change to "S"
  2. Branch ID → Auto-filled (e.g., "1")
  3. Number Format → Select "YYYY-NNNN"
  4. Date Format → Type "YYYYMMDD"
  5. Reset → Select "Annually"
  6. Starting Number → Enter "1"
```

### Step 4: View Preview
```
Preview column shows: S1-20260713-2026-0001
This updates as you edit each field
```

### Step 5: Save
```
Click "Save Settings" button
Settings persisted to database
```

---

## Interactive Examples

### Example 1: User Edits Date Format Field

**Before:**
```
Date Format: YYYYMMDD
Preview:     JV1-20260713-2026-0001
```

**User changes Date Format to:**
```
YYYY-MM-DD
```

**Immediate update (live):**
```
Preview:     JV1-2026-07-13-2026-0001
```

### Example 2: User Changes Number Format

**Before:**
```
Number Format: YYYY-NNNN
Preview:       S1-20260713-2026-0001
```

**User changes to:**
```
NNNN
```

**Preview updates to:**
```
S1-20260713-0001
```

### Example 3: User Clears Date Format

**Before:**
```
Date Format: YYYYMMDD
Preview:     RV1-20260713-2026-0001
```

**User clears field:**
```
Date Format: (blank)
```

**Preview updates to:**
```
RV1-2026-0001
```

---

## Keyboard Navigation

```
Tab              → Move to next field
Shift+Tab        → Move to previous field
Enter            → Confirm edit (move next row)
Escape           → Cancel edit
Ctrl+S           → Save (if button is focused)
Arrow Keys       → Navigate grid cells
```

---

## Visual Indicators

### Read-Only Fields
```
- Grayed out text
- Cannot click to edit
- Shows tooltip on hover: "This field is read-only"
- Example: Branch ID field
```

### Dropdown Fields
```
- Has dropdown arrow
- Shows list when clicked
- Example: Number Format, Reset fields
```

### Editable Text Fields
```
- White background
- Can type directly
- Cursor appears on click
- Example: Prefix, Date Format, Starting Number
```

### Preview Column
```
- Light gray background
- Read-only (cannot edit)
- Bold text
- Shows example with today's date
```

---

## Error Handling

### Invalid Date Format
```
User types: "DDDDDD" (invalid placeholder)
System: Treats as literal text, no conversion
Result: DDDDDD (appears literally in voucher)
Tip: Must use uppercase YYYY, MM, DD
```

### Invalid Number Format
```
User selects: Non-existent format
System: Falls back to YYYY-NNNN
```

### Empty Required Fields
```
User tries to save with empty Prefix:
System: Validation error (if required by rules)
Tooltip: "Please enter a prefix"
```

---

## Help Text & Tooltips

### When hovering over "Date Format" header:
```
"Enter date pattern using YYYY (year), MM (month), DD (day)
Examples: YYYYMMDD, YYYY-MM-DD, DD/MM/YYYY
Leave blank to omit date from voucher number"
```

### When hovering over "Branch ID" field:
```
"Auto-populated from your login session
Shows which branch this voucher is for
Cannot be edited - change by logging in as different branch"
```

### When hovering over "Preview" column:
```
"Live example of how your vouchers will look
Shows today's date as example
Format: [Prefix][BranchId]-[Date]-[Counter]"
```

---

## Common User Actions

### Action: Change all to include dates
```
1. Click Date Format cell in first row
2. Type: YYYYMMDD
3. Press Enter (moves to next row)
4. Repeat for all rows
5. Click Save Settings
```

### Action: Disable dates for specific type
```
1. Find row (e.g., RECEIPT)
2. Click Date Format cell
3. Clear the field (delete existing text)
4. Preview updates to show no date
5. Click Save Settings
```

### Action: Change Reset frequency
```
1. Click Reset dropdown for desired row
2. Select new option
3. Preview updates (if shows year)
4. Click Save Settings
```

### Action: Revert to defaults
```
1. Click "Reset Defaults" button
2. Confirm: "Are you sure?"
3. All rows restored to default values
4. No save needed (automatic reset)
```

---

## Multi-Branch Behavior

### User logs in as Branch 1:
```
Branch ID Column: 1
Preview: JV1-20260713-2026-0001
```

### Same user logs in as Branch 2:
```
Branch ID Column: 2 (auto-updated)
Preview: JV2-20260713-2026-0001
```

### Note:
```
Each branch can have different date formats
Settings are per-branch
User can switch branches by logging out/in
```

---

## Accessibility Features

✅ Tab navigation works
✅ Keyboard shortcuts available
✅ Clear visual indicators
✅ Descriptive column headers
✅ Helpful error messages
✅ Tooltip explanations
✅ Consistent with application theme

---

## Performance Notes

⚡ Grid loads instantly
⚡ Preview updates in real-time (no delay)
⚡ Large datasets handled smoothly
⚡ Responsive to user input
⚡ Memory efficient

---

## Responsive Design

### Desktop (Standard View)
```
All columns visible
Full width display
Horizontal scrolling if needed
```

### Tablet (Reduced Width)
```
May need horizontal scrolling
Columns remain readable
Touch-friendly controls
```

### Mobile (Not Supported)
```
Settings form designed for desktop
Use desktop view for configuration
Mobile access not recommended
```

---

## Color Scheme

```
Header Background:    Theme accent color
Row Background:       Light theme (alternating white/gray)
Read-Only Fields:     Lighter gray background
Text Color:           Dark/primary color
Preview:              Bold, slightly highlighted
Focus:                Blue highlight on active field
Hover:                Light blue background
```

---

## Summary

The Voucher Configuration tab provides an intuitive interface for setting up advanced voucher numbering with:
- ✅ Automatic branch tracking
- ✅ Flexible date formatting
- ✅ Live preview feedback
- ✅ Simple row-based editing
- ✅ Clear visual indicators
- ✅ Helpful documentation

Users can configure complex voucher schemes with just a few clicks!
