# Stock Suppression Feature - Implementation Guide

## Overview
This document explains the stock suppression feature in the POS/ERP system, which handles item-code supersession while preserving historical integrity.

## Business Model

### Best Practice: Preserve Old Item + Link New Item

When an item code changes or is superseded, the system uses a **forward/backward link** approach:

```
OLD ITEM (immutable)                     NEW ITEM (future)
├─ Original item code (unchanged)       ├─ New item code
├─ Historical transactions intact       ├─ Ready for new transactions
├─ All past records preserved           ├─ Inherits stock from old item
└─ superseded_to_item_code = NEW ──────→ superseded_from_item_code = OLD
```

### Key Principles

1. **Historical Integrity**: Old item remains unchanged with all historical data
2. **Stock Transfer**: Inventory quantity transferred from old to new item
3. **Forward Traceability**: Can follow old → new item through `superseded_to_item_code`
4. **Backward Traceability**: Can trace new → old item through `superseded_from_item_code`
5. **Chain Supersession**: Supports multiple levels (OLD → INTERMEDIATE → NEW)

---

## Database Schema

### pos_products Table Changes

Two new columns were added to support supersession:

```sql
ALTER TABLE pos_products ADD (
    superseded_from_item_code NVARCHAR(100) NULL,  -- Links to source item
    superseded_to_item_code NVARCHAR(100) NULL     -- Links to replacement item
);
```

### Data Flow Example

**Before Supersession:**
```
Item ABC (in stock: 100 units)
├─ superseded_from_item_code: NULL
└─ superseded_to_item_code: NULL
```

**After Superseding ABC → XYZ:**
```
OLD - Item ABC (in stock: 0 units - preserved)
├─ superseded_from_item_code: NULL
└─ superseded_to_item_code: XYZ ─────────┐
                                         │
NEW - Item XYZ (in stock: 100 units)     │
├─ superseded_from_item_code: ABC ←─────┘
└─ superseded_to_item_code: NULL
```

---

## UI/Form Workflow

### File: `frm_stock_suppression.cs`

**Step 1: Select Old Part**
- User clicks "Search" button in "Old part number" group
- Dialog opens to search and select the item to be superseded
- System automatically checks if already superseded
- Displays current supersession status if exists

**Step 2: Review Supersession Status**
- Label shows: "Not superseded" OR "Already superseded to: [code] ([name])"
- If already superseded, user can confirm chain supersession

**Step 3: Configure Options**
- **Transfer Stock**: Move inventory from old to new item (checked by default)
- **Zero Demand**: Clear demand quantities on old item (checked by default)
- **Transfer Part Description**: Copy description if new item empty (unchecked)
- **Reset Re-order Level**: Clear reorder threshold on old item
- **Select Branch/Company**: Choose which branches to apply supersession

**Step 4: Select New Part**
- User clicks "Search" button in "New part number" group
- Selects the replacement item code

**Step 5: Execute Supersession**
- Click "Supersede" button
- System performs multi-step transaction:
  1. Updates old item's `superseded_to_item_code`
  2. Updates new item's `superseded_from_item_code`
  3. Transfers stock (if selected)
  4. Clears demand/reorder (if selected)
  5. Logs action for audit trail

---

## Implementation Details

### 1. Form Component (`frm_stock_suppression.cs`)

**Key Method: ExecuteSupersede()**
```csharp
private void ExecuteSupersede()
{
    // Validation
    // - Check old and new parts selected
    // - Verify not same item
    // - Ensure branch selected

    // Check for existing supersession chain
    // - If already superseded, offer chain option

    // Call BLL
    int result = _productBLL.ExecuteStockSuppression(...)

    // On success: refresh display, clear new selection
}
```

**Key Method: LoadAlreadySupersededTo()**
```csharp
private void LoadAlreadySupersededTo(string oldItemNumber)
{
    // Query: SELECT superseded_to_item_code FROM pos_products WHERE item_number = @old
    // Display current supersession status
    // If already linked, show the target item details
}
```

### 2. Business Layer (`ProductBLL.cs`)

**Wrapper Method: ExecuteStockSuppression()**
```csharp
public int ExecuteStockSuppression(
    string oldItemNumber,
    string newItemNumber,
    List<int> branchIds,
    bool transferStock,
    bool zeroDemandOldPart,
    bool transferPartDescription,
    bool resetReorderLevel)
{
    return productDLL.ExecuteStockSuppression(...);
}
```

### 3. Data Layer (`ProductsDLL.cs`)

**Implementation Method: ExecuteStockSuppression()**

Uses **SQL Transaction** for atomicity:

```sql
BEGIN TRANSACTION

-- 1. Fetch old item details
SELECT id, description, superseded_to_item_code FROM pos_products 
WHERE item_number = @old_item_number

-- 2. Fetch new item details  
SELECT id, description FROM pos_products 
WHERE item_number = @new_item_number

-- 3. UPDATE OLD ITEM: Mark as superseded
UPDATE pos_products 
SET superseded_to_item_code = @new_item_number,
    date_updated = GETDATE()
WHERE id = @old_product_id

-- 4. UPDATE NEW ITEM: Set backward link
UPDATE pos_products 
SET superseded_from_item_code = @old_item_number,
    date_updated = GETDATE()
WHERE id = @new_product_id

-- 5. TRANSFER STOCK (per branch)
For each branch:
  a) Get old item stock quantity
  b) Add to new item stock (or create if not exists)
  c) Zero out old item stock

-- 6. LOG ACTION for audit trail
INSERT INTO audit_logs ...

COMMIT TRANSACTION
```

---

## Tracing Supersessions

### Forward Trace (Old → New)
```sql
-- Find what an old item was superseded to
SELECT item_number, superseded_to_item_code 
FROM pos_products 
WHERE item_number = 'ABC'
-- Result: ABC → XYZ

-- Then trace further if XYZ also superseded
SELECT item_number, superseded_to_item_code 
FROM pos_products 
WHERE item_number = 'XYZ'
-- If result empty, XYZ is current
```

### Backward Trace (New → Old)
```sql
-- Find what superseded a current item
SELECT item_number, superseded_from_item_code 
FROM pos_products 
WHERE item_number = 'XYZ'
-- Result: XYZ ← ABC
```

### Full Chain
```sql
-- Get full supersession chain
WITH SupersessionChain AS (
    SELECT item_number, superseded_from_item_code, 0 AS level
    FROM pos_products 
    WHERE item_number = 'XYZ'

    UNION ALL

    SELECT p.item_number, p.superseded_from_item_code, c.level + 1
    FROM pos_products p
    INNER JOIN SupersessionChain c ON p.item_number = c.superseded_from_item_code
    WHERE c.level < 10  -- Prevent infinite loops
)
SELECT * FROM SupersessionChain
-- Shows: XYZ ← ABC ← OLD_ABC ← OLDER_ABC ...
```

---

## Error Handling

### Validations in Form
1. Old part must be selected
2. New part must be selected
3. Old and new parts cannot be identical
4. At least one branch must be selected

### Validations in Data Layer
1. Both old and new products must exist in database
2. Both products must not be deleted
3. Stock transfer only if quantities > 0
4. Transaction rolled back on any error

### Error Messages
- **"No changes were saved"**: Product IDs not found or validation failed
- **Exception details**: Logged with user ID and branch for audit

---

## Reporting & Queries

### Find All Superseded Items
```sql
SELECT item_number, name, superseded_to_item_code 
FROM pos_products 
WHERE superseded_to_item_code IS NOT NULL 
  AND TRIM(superseded_to_item_code) != ''
```

### Find Orphaned Supersessions
```sql
-- Find links where target doesn't exist
SELECT p.item_number, p.superseded_to_item_code 
FROM pos_products p
LEFT JOIN pos_products target ON target.item_number = p.superseded_to_item_code
WHERE p.superseded_to_item_code IS NOT NULL 
  AND TRIM(p.superseded_to_item_code) != ''
  AND target.id IS NULL
```

### Stock History Audit
```sql
SELECT 
    p.item_number,
    p.name,
    p.superseded_to_item_code,
    p.superseded_from_item_code,
    ps.qty,
    ps.date_updated,
    u.username
FROM pos_products p
LEFT JOIN pos_product_stocks ps ON ps.item_number = p.item_number
LEFT JOIN users u ON u.id = p.user_id
WHERE p.superseded_to_item_code IS NOT NULL
ORDER BY p.date_updated DESC
```

---

## Audit & Logging

### Action Logged As
```
Action Name: "Stock Suppression"
Details: "Old Item=ABC, New Item=XYZ, TransferStock=true, 
         ZeroDemand=true, Branches=1,2,3"
User ID: [Current User]
Branch ID: [Current Branch]
Timestamp: [Current DateTime]
```

### Access Log Table
```
pos_logs / audit_logs
├─ action: "Stock Suppression"
├─ description: Details (as above)
├─ user_id: Who performed it
├─ branch_id: Which branch
└─ date_time: When it happened
```

---

## Troubleshooting

### Issue: "Supersession link not created"
**Cause**: Product not found or deleted flag set  
**Solution**: Verify both items exist and are not marked as deleted

### Issue: "Stock not transferred"
**Cause**: transferStock option unchecked OR no stock in old item  
**Solution**: Check the checkbox and verify old item has stock

### Issue: "Chain supersession detected"
**Cause**: Old item already has `superseded_to_item_code`  
**Solution**: User can confirm to create chain (old → intermediate → new)

### Issue: Transaction rolled back
**Cause**: SQL error during update  
**Solution**: Check logs for specific SQL error; verify database consistency

---

## Security Considerations

1. **Permission Required**: `Permissions.Inventory_Edit`
2. **User Tracking**: All changes logged with user ID
3. **Audit Trail**: Immutable history preserved (old item never updated except supersession link)
4. **Transaction Safety**: SQL transaction ensures all-or-nothing atomicity

---

## Performance Notes

- Stock suppression operations are typically one-time per item
- Queries on supersession fields should use indexes if volume large
- Transaction locks are brief (seconds)
- No impact on regular sales/purchase operations

---

## Future Enhancements

1. **Automated Cleanup**: Option to archive old items after supersession
2. **Bulk Supersession**: CSV import for multiple item supersessions
3. **Supersession Report**: Detailed report of all supersessions with dates
4. **Reverse Supersession**: Undo supersession if discovered in error
5. **Item Merge**: Consolidate multiple similar items into one

---

## References

- **Form**: `pos/Products/Suppression/frm_stock_suppression.cs`
- **BLL**: `POS.BLL/ProductBLL.cs` → `ExecuteStockSuppression()`
- **DLL**: `POS.DLL/ProductsDLL.cs` → `ExecuteStockSuppression()`
- **Main Menu**: `pos/Main.cs` → `stockSuppressionToolStripMenuItem_Click()`
- **Permission**: `Permissions.Inventory_Edit`
