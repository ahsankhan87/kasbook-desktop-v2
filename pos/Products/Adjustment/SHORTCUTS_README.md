# Stock Check & Adjustment — Keyboard Shortcuts & Workflow Guide

## Overview

The Stock Check & Adjustment form is designed for high-speed warehouse operations.
It supports processing hundreds of products per session using keyboard-first navigation,
barcode scanning, bulk operations, and auto-save protection.

---

## Keyboard Shortcuts

### Navigation

| Key | Action |
|-----|--------|
| **F2** or **Ctrl+F** | Focus the Search box (left panel) |
| **F3** | Toggle Advanced Filter panel |
| **F4** | Open Product Detail drawer for the selected row |
| **F5** | Refresh the product index from the database |
| **F6** | Enter Barcode Scan Mode |

### Session Lifecycle

| Key | Action |
|-----|--------|
| **F9** | Save current session as Draft |
| **F10** | Post Adjustment (shows confirmation with item counts) |

### Editing

| Key | Action |
|-----|--------|
| **Ctrl+Z** | Undo last cell edit (up to 20 operations) |
| **Ctrl+Y** | Redo last undone edit |
| **Ctrl+A** | Select all rows in the adjustment grid |
| **Ctrl+D** | Duplicate current row's Location + Reason to all selected rows |
| **Ctrl+L** | Apply current row's New Location to all selected rows |
| **Delete** | Remove selected rows (asks confirmation if more than 5) |

### Export & Print

| Key | Action |
|-----|--------|
| **Ctrl+E** | Export session to Excel |
| **Ctrl+P** | Print adjustment voucher |

### Navigation Inside Grid

| Key | Action |
|-----|--------|
| **Tab** | Move to next editable column (Smart Tab Order — skips read-only columns) |
| **Shift+Tab** | Move to previous editable column |
| **Enter** (in search results) | Add selected product to adjustment grid |
| **Escape** | Cancel current edit / Close drawer / Exit Scan Mode |

### Help

| Key | Action |
|-----|--------|
| **F1** | Open this help reference (Shortcuts & Workflow Guide) |

---

## Smart Tab Order

When navigating the adjustment grid with **Tab**, only editable columns are visited:

```
Physical/New Qty → New Sale Price → New Location → Reason → Notes
                                                              ↓
                                          next row → Physical/New Qty
```

Read-only columns (Product Code, Name, Category, Current Qty, Current Price,
Current Location, Difference columns) are automatically skipped.

---

## Workflow Features

### Remember Last Reason
When you add a new product to the grid, the **Reason** field is automatically
pre-filled with the last reason you used. This saves time when processing batches
of the same adjustment type (e.g., "Physical Count").

### Batch Verify
To mark multiple consecutive rows as Verified without clicking each checkbox:
1. Click the first row you want to verify.
2. **Shift+click** the last row in the range.
3. All rows between them are instantly marked Verified.

### Quick Location Copy
After typing a location in a row's **New Location** cell, a tooltip appears:
> *"Press Ctrl+L to apply this location to all selected rows"*

Select the rows you want to update first, then press **Ctrl+L**.

### Auto-Save Draft
The session is automatically saved as a Draft every **5 minutes** if there are
unsaved changes. The status bar shows:
> *"Auto-saved at HH:MM | User: ... | Warehouse: ..."*

Auto-save runs silently — it never interrupts your editing flow.

### Unsaved Changes Protection
If you close the form while there are unsaved changes, a dialog will appear:
- **Yes** → Save as Draft then close
- **No** → Discard changes and close
- **Cancel** → Stay on the form

---

## Barcode Scan Mode (F6)

Scan Mode takes over the right panel with a full-screen interface:

1. Press **F6** to enter Scan Mode.
2. Scan a barcode — the product is located in the session grid automatically.
3. Enter the physical quantity using the numeric spinner.
4. Press **Enter** or click **Confirm & Next** to save and move to the next scan.
5. Click **Add Note** to attach a note to the scanned product.
6. The **Scan Log** (right side) shows the last 20 scans with timestamps.
7. Press **Escape** or click **Exit Scan Mode** to return to normal view.

---

## Product Detail Drawer (F4)

Press **F4** on any row to open the Product Detail drawer (right side panel):

- System quantity and last physical count date
- Pricing: cost, sale price, margin %
- Location breakdown with per-location quantities
- Min/reorder level information
- Last 5 transactions (sales, purchases, adjustments)
- 6-month stock trend chart
- **Quick Adjust** button — jumps directly to that row's Physical Qty cell

---

## Adjustment Grid Column Reference

| Column | Editable | Description |
|--------|----------|-------------|
| # | No | Row number; blue = modified, green = verified |
| ✓ | Yes | Verified checkbox |
| Product Code | No | Unique product identifier |
| Product Name | No | Product description |
| Category | No | Product category |
| Current Qty | No | System quantity before adjustment |
| Physical/New Qty | **Yes** | Quantity counted or new target quantity |
| Difference | No | Auto-calculated: New Qty − Current Qty |
| Current Sale Price | No | Price before adjustment |
| New Sale Price | **Yes** | Updated sale price |
| Price Diff | No | Auto-calculated: New Price − Current Price |
| Current Location | No | Existing warehouse location code |
| New Location | **Yes** | Target location (dropdown) |
| Reason | **Yes** | Reason code for this adjustment (dropdown) |
| Notes | **Yes** | Free-text notes |
| Actions | — | ✖ button removes the row from session |

---

## Status Bar Reference

| Field | Description |
|-------|-------------|
| Left | Total items / Adjusted / Pending counts |
| Centre | Net stock value change (green = increase, red = decrease) |
| Right | Last save time, current user, warehouse name |

---

## Adjustment Types

| Type | Use Case |
|------|----------|
| Physical Count | Routine stock count reconciliation |
| Damage Write-Off | Damaged or expired goods removal |
| Found/Excess | Stock found above recorded level |
| Price Update | Sale price corrections only |
| Location Transfer | Move stock between warehouse locations |
| Opening Stock | Initial stock entry for new products |

---

## Tips for High-Volume Sessions

1. **Use Barcode Scan Mode** (F6) for the fastest data entry — no mouse needed.
2. **Filter first** (F3) to narrow down to a single aisle or category before adding products.
3. **Shift+click** to batch-verify an entire shelf's worth of products at once.
4. Use **Ctrl+D** after filling in one row's location and reason to copy it to the rest of the batch.
5. **F9** to save a draft mid-session — never lose work on a long count.
6. Use the **Verified** checkbox column to track what has been physically counted vs. still pending.
