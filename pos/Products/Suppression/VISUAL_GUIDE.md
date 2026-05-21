# Stock Suppression - Visual Architecture Guide

## System Flow Diagram

```
┌───────────────────────────────────────────────────────────────┐
│                     END USER                                  │
│                                                               │
│  1. Opens Stock Suppression Form                             │
│     (Main Menu → Products → Stock Suppression)               │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌───────────────────────────────────────────────────────────────┐
│                  PRESENTATION LAYER                           │
│            frm_stock_suppression.cs                           │
│                                                               │
│  2. User selects OLD part (search dialog)                    │
│     └─ LoadAlreadySupersededTo() checks for existing links  │
│                                                               │
│  3. User configures options                                  │
│     ├─ Transfer Stock ✓                                     │
│     ├─ Zero Demand ✓                                        │
│     ├─ Transfer Description                                 │
│     └─ Reset Reorder ✓                                      │
│                                                               │
│  4. User selects NEW part (search dialog)                    │
│                                                               │
│  5. User clicks "Supersede" button                           │
│     └─ ExecuteSupersede() validates & calls BLL             │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌───────────────────────────────────────────────────────────────┐
│                  BUSINESS LOGIC LAYER                         │
│            ProductBLL.cs                                      │
│                                                               │
│  6. ExecuteStockSuppression(                                 │
│       oldItemNumber,                                         │
│       newItemNumber,                                         │
│       branchIds,                                             │
│       transferStock,                                         │
│       zeroDemand,                                            │
│       transferDescription,                                   │
│       resetReorder)                                          │
│     └─ Delegates to ProductDLL                              │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌───────────────────────────────────────────────────────────────┐
│                  DATA ACCESS LAYER                            │
│            ProductsDLL.cs                                     │
│                                                               │
│  7. ExecuteStockSuppression() Implementation:                │
│                                                               │
│     ┌─ BEGIN TRANSACTION ────────────────────────┐           │
│     │                                            │           │
│     │  a) Fetch old product ID & details        │           │
│     │  b) Fetch new product ID & details        │           │
│     │  c) Validate both products exist          │           │
│     │  d) UPDATE old: set superseded_to         │           │
│     │  e) UPDATE new: set superseded_from       │           │
│     │  f) Transfer description (if option)      │           │
│     │  g) Zero demand (if option)               │           │
│     │  h) Reset reorder (if option)             │           │
│     │  i) Transfer stock per branch (if option) │           │
│     │  j) Log action to audit                   │           │
│     │                                            │           │
│     └─ COMMIT (or ROLLBACK on error) ──────────┘           │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌───────────────────────────────────────────────────────────────┐
│                  DATABASE LAYER                               │
│            SQL Server - pos_products table                    │
│                                                               │
│  8. Data Modified:                                            │
│                                                               │
│     OLD ITEM (ABC):                                           │
│     ├─ item_number: ABC (unchanged)                         │
│     ├─ name: Original Part Name (unchanged)                 │
│     ├─ superseded_to_item_code: "XYZ" ◄─── UPDATED        │
│     ├─ qty: 0 (transferred)                                 │
│     ├─ demand_qty: 0 (cleared)                              │
│     └─ date_updated: [CURRENT]                              │
│                                                               │
│     NEW ITEM (XYZ):                                           │
│     ├─ item_number: XYZ (unchanged)                         │
│     ├─ name: Original Part Name (copied if empty)           │
│     ├─ superseded_from_item_code: "ABC" ◄─── UPDATED      │
│     ├─ qty: 100 (transferred)                               │
│     └─ date_updated: [CURRENT]                              │
│                                                               │
│     pos_product_stocks (per branch):                         │
│     ├─ ABC@Branch1: qty=0 (was 100)                         │
│     └─ XYZ@Branch1: qty=100 (was 0)                         │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌───────────────────────────────────────────────────────────────┐
│                  AUDIT LAYER                                  │
│            pos_logs table                                     │
│                                                               │
│  9. Action Logged:                                            │
│     ├─ action: "Stock Suppression"                           │
│     ├─ description: "Old=ABC, New=XYZ, ..."                 │
│     ├─ user_id: [CURRENT_USER]                              │
│     ├─ branch_id: [CURRENT_BRANCH]                          │
│     └─ date_time: [CURRENT_DATETIME]                        │
└────────────────────────┬────────────────────────────────────┘
                         │
                         ▼
┌───────────────────────────────────────────────────────────────┐
│                  RETURN TO USER                               │
│                                                               │
│  10. Success Message:                                         │
│      "Stock suppression completed successfully."             │
│      "• Old item preserved with history"                     │
│      "• New item ready for future transactions"              │
│      "• Supersession link established"                       │
└───────────────────────────────────────────────────────────────┘
```

---

## Data Transformation Diagram

```
BEFORE SUPERSESSION:
═════════════════════

pos_products (ABC):
┌──────────────────────────────────────────────┐
│ id: 1                                        │
│ item_number: ABC                             │
│ name: Widget v1                              │
│ superseded_from_item_code: NULL              │
│ superseded_to_item_code: NULL ◄─ EMPTY      │
│ qty_on_hand: 100                             │
│ demand_qty: 50                               │
└──────────────────────────────────────────────┘

pos_products (XYZ):
┌──────────────────────────────────────────────┐
│ id: 2                                        │
│ item_number: XYZ                             │
│ name: Widget v2                              │
│ superseded_from_item_code: NULL              │
│ superseded_to_item_code: NULL ◄─ EMPTY      │
│ qty_on_hand: 0                               │
│ demand_qty: 0                                │
└──────────────────────────────────────────────┘

pos_product_stocks (Branch 1):
┌──────────────────────────────────────────────┐
│ item_number: ABC, qty: 100                   │
│ item_number: XYZ, qty: 0                     │
└──────────────────────────────────────────────┘


AFTER SUPERSESSION:
═══════════════════

pos_products (ABC):
┌──────────────────────────────────────────────┐
│ id: 1                                        │
│ item_number: ABC                             │
│ name: Widget v1                              │
│ superseded_from_item_code: NULL              │
│ superseded_to_item_code: "XYZ" ◄─ LINKED    │ ───┐
│ qty_on_hand: 0 ◄─ CLEARED                   │    │
│ demand_qty: 0 ◄─ CLEARED                    │    │
└──────────────────────────────────────────────┘    │
                                                    │ LINK
pos_products (XYZ):                                 │
┌──────────────────────────────────────────────┐    │
│ id: 2                                        │    │
│ item_number: XYZ                             │ ◄──┘
│ name: Widget v2                              │
│ superseded_from_item_code: "ABC" ◄─ LINKED  │
│ superseded_to_item_code: NULL                │
│ qty_on_hand: 100 ◄─ TRANSFERRED              │
│ demand_qty: 0                                │
└──────────────────────────────────────────────┘

pos_product_stocks (Branch 1):
┌──────────────────────────────────────────────┐
│ item_number: ABC, qty: 0 ◄─ ZEROED           │
│ item_number: XYZ, qty: 100 ◄─ TRANSFERRED   │
└──────────────────────────────────────────────┘
```

---

## Traceability Diagram

```
FORWARD TRACE (Old → New):
═════════════════════════════

Query: SELECT superseded_to_item_code FROM pos_products WHERE item_number='ABC'
Result: "XYZ"

ABC ──superseded_to_item_code='XYZ'──> XYZ
     (Answer: ABC was superseded to XYZ)


BACKWARD TRACE (New → Old):
═════════════════════════════

Query: SELECT superseded_from_item_code FROM pos_products WHERE item_number='XYZ'
Result: "ABC"

XYZ <──superseded_from_item_code='ABC'── ABC
    (Answer: XYZ replaced ABC)


CHAIN TRACE (Multi-level):
═════════════════════════════

Query: Recursive CTE to follow chain
Result:

ABC ──superseded_to_item_code='XYZ'──> XYZ ──superseded_to_item_code='UVW'──> UVW
                                                                           (current)

History: ABC → XYZ → UVW
```

---

## Stock Movement Diagram

```
BEFORE TRANSFER:
═══════════════

pos_product_stocks:
┌────────────────────────────────────────┐
│ Item: ABC, Branch: 1, Qty: 100         │
│ Item: XYZ, Branch: 1, Qty: 0           │
│                                        │
│ Item: ABC, Branch: 2, Qty: 50          │
│ Item: XYZ, Branch: 2, Qty: 0           │
└────────────────────────────────────────┘


DURING TRANSFER:
════════════════

For each branch:
  1. Get ABC qty (100 @ Branch 1, 50 @ Branch 2)
  2. Add to XYZ qty (or create if not exists)
  3. Zero ABC qty


AFTER TRANSFER:
═══════════════

pos_product_stocks:
┌────────────────────────────────────────┐
│ Item: ABC, Branch: 1, Qty: 0           │ ◄─ Cleared
│ Item: XYZ, Branch: 1, Qty: 100         │ ◄─ Increased by 100
│                                        │
│ Item: ABC, Branch: 2, Qty: 0           │ ◄─ Cleared
│ Item: XYZ, Branch: 2, Qty: 50          │ ◄─ Increased by 50
└────────────────────────────────────────┘

Total Stock Movement:
  ABC: 150 → 0 (100+50 transferred out)
  XYZ: 0 → 150 (100+50 transferred in)
```

---

## Transaction Safety Diagram

```
NORMAL EXECUTION:
═════════════════

├─ BEGIN TRANSACTION
│  ├─ Fetch old product
│  ├─ Fetch new product
│  ├─ Validate both exist
│  ├─ [Operations succeed]
│  ├─ Update links
│  ├─ Transfer stock
│  ├─ Log action
│  └─ COMMIT ✓
└─ Result: All changes saved


ERROR EXECUTION:
════════════════

├─ BEGIN TRANSACTION
│  ├─ Fetch old product
│  ├─ Fetch new product
│  ├─ Validate both exist
│  ├─ [Error occurs here]
│  └─ ROLLBACK ✓
└─ Result: No changes saved (all-or-nothing)


Atomicity Guarantee:
═══════════════════

EITHER all changes are saved
OR    no changes are saved

Never partial changes
Never inconsistent state
Never corrupted data
```

---

## User Interface Workflow

```
┌─────────────────────────────────────────┐
│ STOCK SUPPRESSION FORM                  │
├─────────────────────────────────────────┤
│                                         │
│ OLD PART NUMBER:                        │
│ ┌─────────────────────┐ ┌─────────┐    │
│ │ [ABC]               │ │ Search  │    │
│ └─────────────────────┘ └─────────┘    │
│                                         │
│ Description: Widget v1                  │
│ Already superseded to: [Not superseded] │
│                                         │
│ ─────────────────────────────────────── │
│ OPTIONS:                                │
│ ☑ Transfer Stock                        │
│ ☑ Zero Demand Old Part                  │
│ ☐ Transfer Description                  │
│ ☑ Reset Re-order Level                  │
│                                         │
│ Branch: [Branch 1] (1 branch selected)  │
│ ┌─────────────────────────────────────┐ │
│ │ [Select Branches]                   │ │
│ └─────────────────────────────────────┘ │
│                                         │
│ ─────────────────────────────────────── │
│ NEW PART NUMBER:                        │
│ ┌─────────────────────┐ ┌─────────┐    │
│ │ [XYZ]               │ │ Search  │    │
│ └─────────────────────┘ └─────────┘    │
│                                         │
│ ─────────────────────────────────────── │
│ ┌──────────┐ ┌──────────┐ ┌───────┐    │
│ │ Supersed │ │  Cancel  │ │ Help  │    │
│ └──────────┘ └──────────┘ └───────┘    │
│                                         │
└─────────────────────────────────────────┘
```

---

## Permission & Audit

```
PERMISSION CHECK:
═════════════════

User clicks "Stock Suppression" menu item
        ↓
AppSecurityContext checks:
  "Does user have Permissions.Inventory_Edit?"
        ↓
YES → Form opens
NO  → Form blocked


AUDIT LOGGING:
══════════════

After successful supersession:
        ↓
Log entry created:
  ├─ action: "Stock Suppression"
  ├─ description: "Old=ABC, New=XYZ, ..."
  ├─ user_id: 123 (current user)
  ├─ branch_id: 1 (current branch)
  ├─ date_time: 2024-12-20 14:30:45
  └─ status: SUCCESS
```

---

## Error Handling Flow

```
USER ACTION
     │
     ▼
VALIDATION LAYER
     │
     ├─ Old part selected? ─────NO──→ Show warning → STOP
     ├─ New part selected? ─────NO──→ Show warning → STOP
     ├─ Are they different? ────NO──→ Show warning → STOP
     ├─ Branch selected? ───────NO──→ Show warning → STOP
     └─ All checks pass? ──────YES──→ Continue
         │
         ▼
    CALL BLL
         │
         ▼
    DATABASE TRANSACTION
         │
         ├─ Products found? ─────NO──→ Rollback → Error message
         ├─ Query OK? ────────────NO──→ Rollback → Error message
         ├─ Update OK? ───────────NO──→ Rollback → Error message
         └─ All OK? ─────────────YES──→ Commit → Success message
         │
         ▼
    SHOW RESULT TO USER
```

---

## Quick Reference Card

```
╔══════════════════════════════════════════════════════╗
║          STOCK SUPPRESSION - QUICK REFERENCE         ║
╠══════════════════════════════════════════════════════╣
║                                                      ║
║ WHAT IT DOES:                                       ║
║ • Marks item ABC as superseded to XYZ              ║
║ • Transfers stock from ABC to XYZ                   ║
║ • Creates bidirectional links                       ║
║ • Preserves history completely                      ║
║                                                      ║
║ WHEN TO USE:                                        ║
║ • Item code changed                                 ║
║ • New supplier item number                          ║
║ • Product version update                            ║
║ • Item consolidation                                ║
║                                                      ║
║ WHAT NOT TO USE FOR:                                ║
║ • Deleting items (use Product → Delete)            ║
║ • Changing prices (use Product → Update)           ║
║ • Inventory adjustments (use Adjustment form)      ║
║ • Stock transfers (use Location Transfer)          ║
║                                                      ║
║ KEY OPTIONS:                                        ║
║ ✓ Transfer Stock - Move qty to new item            ║
║ ✓ Zero Demand - Clear demand on old item           ║
║ ✓ Transfer Description - Copy if new is empty      ║
║ ✓ Reset Re-order - Clear reorder on old item       ║
║                                                      ║
║ RESULT:                                             ║
║ OLD: qty=0, superseded_to="XYZ"                    ║
║ NEW: qty=transferred, superseded_from="ABC"        ║
║ LINK: ABC ←→ XYZ (bidirectional)                  ║
║                                                      ║
║ SAFE? YES - Transaction protected                   ║
║ REVERSIBLE? Manual reset possible                  ║
║ AUDITED? Full logging included                      ║
║                                                      ║
╚══════════════════════════════════════════════════════╝
```

---

This visual guide helps understand the complete flow from user action to database updates! 📊
