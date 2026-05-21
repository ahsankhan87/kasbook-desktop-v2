# Stock Suppression - Technical Reference

## Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│ PRESENTATION LAYER                                          │
│ frm_stock_suppression.cs                                    │
│  ├─ SelectPart(bool isOldPart)                             │
│  ├─ LoadAlreadySupersededTo(string oldItemNumber)          │
│  └─ ExecuteSupersede()                                     │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ BUSINESS LOGIC LAYER                                        │
│ ProductBLL.cs                                               │
│  └─ ExecuteStockSuppression(...) → calls DLL               │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ DATA ACCESS LAYER                                           │
│ ProductsDLL.cs                                              │
│  └─ ExecuteStockSuppression(...) → SQL Transaction         │
└─────────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────────┐
│ DATABASE                                                    │
│ pos_products table (with supersession columns)             │
│  ├─ superseded_from_item_code                              │
│  └─ superseded_to_item_code                                │
└─────────────────────────────────────────────────────────────┘
```

---

## Method Signatures

### Form Level: `frm_stock_suppression.cs`

```csharp
private void frm_stock_suppression_Load(object sender, EventArgs e)
// Initializes form, applies theme, sets defaults

private void SelectPart(bool isOldPart)
// Opens dialog to select old or new item
// Parameters:
//   isOldPart: true = select old part, false = select new part
// Returns: void (sets txtOldPartCode or txtNewPartCode)

private void LoadAlreadySupersededTo(string oldItemNumber)
// Queries database to check if old item already superseded
// Displays: "Not superseded" or "Already superseded to: XYZ"
// Parameters:
//   oldItemNumber: Item code to check
// Returns: void (updates lblAlreadySupersededTo.Text)

private void ExecuteSupersede()
// Main execution method
// Validates inputs, calls BLL, handles errors
// Returns: void (updates UI with result)
```

### BLL Level: `ProductBLL.cs`

```csharp
public int ExecuteStockSuppression(
    string oldItemNumber,           // Item code being superseded
    string newItemNumber,           // Replacement item code
    List<int> branchIds,           // Branches to apply to
    bool transferStock,            // Transfer inventory
    bool zeroDemandOldPart,        // Clear demand on old item
    bool transferPartDescription,  // Copy description if empty
    bool resetReorderLevel)        // Clear reorder level
// 
// Returns: int (number of rows affected)
// Throws: Exception on error (caught and displayed in form)
```

### DLL Level: `ProductsDLL.cs`

```csharp
public int ExecuteStockSuppression(
    string oldItemNumber,           // Item code being superseded
    string newItemNumber,           // Replacement item code
    List<int> branchIds,           // Branches to apply to
    bool transferStock,            // Transfer inventory
    bool zeroDemandOldPart,        // Clear demand on old item
    bool transferPartDescription,  // Copy description if empty
    bool resetReorderLevel)        // Clear reorder level
//
// IMPLEMENTATION:
// 1. BEGIN TRANSACTION
// 2. Fetch old product ID and details
// 3. Fetch new product ID and details
// 4. UPDATE pos_products SET superseded_to_item_code
// 5. UPDATE pos_products SET superseded_from_item_code
// 6. IF transferPartDescription: Copy description
// 7. IF zeroDemandOldPart: Zero demand fields
// 8. IF resetReorderLevel: Zero reorder level
// 9. IF transferStock: For each branch:
//    - Get old stock qty
//    - Add to new item (or create if not exists)
//    - Zero old item stock
// 10. Log action to audit trail
// 11. COMMIT/ROLLBACK
//
// Returns: int (1 if success, 0 if failure)
// Throws: Exception on SQL error (automatic rollback)
```

---

## SQL Transactions

### Main Transaction Structure

```sql
BEGIN TRANSACTION

  -- 1. Fetch old product
  SELECT @oldProductId = id, @oldDescription = description,
         @existingSupersedeTo = superseded_to_item_code
  FROM pos_products
  WHERE item_number = @oldItemNumber AND deleted = 0

  -- 2. Fetch new product
  SELECT @newProductId = id, @newDescription = description
  FROM pos_products
  WHERE item_number = @newItemNumber AND deleted = 0

  -- 3. Validate both products exist
  IF @oldProductId <= 0 OR @newProductId <= 0
    ROLLBACK TRANSACTION
    RETURN 0

  -- 4. Update old item (mark as superseded)
  UPDATE pos_products
  SET superseded_to_item_code = @newItemNumber,
      date_updated = GETDATE(),
      user_id = @userId
  WHERE id = @oldProductId

  -- 5. Update new item (set source link)
  UPDATE pos_products
  SET superseded_from_item_code = CASE 
        WHEN superseded_from_item_code IS NULL OR 
             TRIM(superseded_from_item_code) = ''
        THEN @oldItemNumber
        ELSE superseded_from_item_code
      END,
      date_updated = GETDATE(),
      user_id = @userId
  WHERE id = @newProductId

  -- 6. (Optional) Transfer description
  IF @transferPartDescription = 1 AND 
     @oldDescription IS NOT NULL AND 
     TRIM(@oldDescription) != '' AND
     (@newDescription IS NULL OR TRIM(@newDescription) = '')
  BEGIN
    UPDATE pos_products
    SET description = @oldDescription,
        date_updated = GETDATE(),
        user_id = @userId
    WHERE id = @newProductId
  END

  -- 7. (Optional) Zero demand
  IF @zeroDemandOldPart = 1
  BEGIN
    UPDATE pos_products
    SET demand_qty = 0,
        purchase_demand_qty = 0,
        sale_demand_qty = 0,
        date_updated = GETDATE(),
        user_id = @userId
    WHERE id = @oldProductId
  END

  -- 8. (Optional) Reset reorder
  IF @resetReorderLevel = 1
  BEGIN
    UPDATE pos_products
    SET re_stock_level = 0,
        date_updated = GETDATE(),
        user_id = @userId
    WHERE id = @newProductId
  END

  -- 9. (Optional) Transfer stock per branch
  IF @transferStock = 1
  BEGIN
    DECLARE @branchId INT, @qty DECIMAL
    DECLARE branchCursor CURSOR FOR
      SELECT @branch FROM @branchIds

    OPEN branchCursor
    FETCH NEXT FROM branchCursor INTO @branchId

    WHILE @@FETCH_STATUS = 0
    BEGIN
      -- Get old item qty
      SELECT @qty = ISNULL(SUM(qty), 0)
      FROM pos_product_stocks
      WHERE item_number = @oldItemNumber AND branch_id = @branchId

      IF @qty > 0
      BEGIN
        -- Update or insert new item stock
        IF EXISTS(SELECT 1 FROM pos_product_stocks 
                  WHERE item_number = @newItemNumber AND branch_id = @branchId)
        BEGIN
          UPDATE pos_product_stocks
          SET qty = ISNULL(qty, 0) + @qty,
              date_updated = GETDATE(),
              user_id = @userId
          WHERE item_number = @newItemNumber AND branch_id = @branchId
        END
        ELSE
        BEGIN
          INSERT INTO pos_product_stocks
          (item_code, item_number, branch_id, qty, date_created, date_updated, user_id)
          VALUES (@newItemNumber, @newItemNumber, @branchId, @qty, GETDATE(), GETDATE(), @userId)
        END

        -- Zero old item stock
        UPDATE pos_product_stocks
        SET qty = 0,
            date_updated = GETDATE(),
            user_id = @userId
        WHERE item_number = @oldItemNumber AND branch_id = @branchId
      END

      FETCH NEXT FROM branchCursor INTO @branchId
    END

    CLOSE branchCursor
    DEALLOCATE branchCursor
  END

  -- 10. Log action
  INSERT INTO pos_logs (action, description, user_id, branch_id, date_time)
  VALUES ('Stock Suppression',
          'Old=' + @oldItemNumber + ', New=' + @newItemNumber,
          @userId,
          @currentBranchId,
          GETDATE())

COMMIT TRANSACTION
```

---

## Error Handling

### Form Level (UI Validation)

```csharp
try
{
    // Validation checks
    if (string.IsNullOrWhiteSpace(oldPart))
        throw new Exception("Please select old part");

    if (string.IsNullOrWhiteSpace(newPart))
        throw new Exception("Please select new part");

    if (string.Equals(oldPart, newPart, StringComparison.OrdinalIgnoreCase))
        throw new Exception("Old and new part cannot be the same");

    if (_selectedBranchIds.Count == 0)
        throw new Exception("Please select at least one branch");

    // Call BLL
    int result = _productBLL.ExecuteStockSuppression(...);

    if (result > 0)
    {
        // Success
        UiMessages.ShowInfo("Stock suppression completed successfully", ...);
    }
    else
    {
        // No changes
        UiMessages.ShowWarning("No changes were saved", ...);
    }
}
catch (Exception ex)
{
    // Error
    UiMessages.ShowError(ex.Message, ...);
    Log.LogAction("Stock Suppression Error", ex.Message, ...);
}
```

### DLL Level (Data Validation)

```csharp
// Validate input
if (string.IsNullOrWhiteSpace(oldItemNumber) || 
    string.IsNullOrWhiteSpace(newItemNumber))
    return 0;

if (string.Equals(oldItemNumber.Trim(), newItemNumber.Trim(), 
    StringComparison.OrdinalIgnoreCase))
    return 0;

// Validate branch list
var validBranches = (branchIds ?? new List<int>())
    .Where(x => x > 0)
    .Distinct()
    .ToList();

// Transaction error handling
try
{
    // ... SQL operations ...
}
catch
{
    tx.Rollback();  // Automatic rollback on error
    throw;          // Re-throw for UI to handle
}
finally
{
    // Cleanup (automatic with using statements)
}
```

---

## Performance Considerations

| Operation | Complexity | Impact |
|-----------|-----------|--------|
| Fetch old product | O(1) | Indexed on item_number |
| Fetch new product | O(1) | Indexed on item_number |
| Update supersession links | O(2) | Two simple UPDATE statements |
| Transfer stock (n branches) | O(n) | n SELECT + n UPDATE pairs |
| Log action | O(1) | Simple INSERT |

**Total Complexity**: O(n) where n = number of branches

**Typical Execution Time**: < 1 second (per branch)

---

## Indexes Recommended

```sql
-- Improve lookup performance
CREATE INDEX idx_pos_products_item_number 
  ON pos_products(item_number)
  WHERE deleted = 0;

CREATE INDEX idx_pos_products_superseded_to 
  ON pos_products(superseded_to_item_code)
  WHERE superseded_to_item_code IS NOT NULL;

CREATE INDEX idx_pos_products_superseded_from 
  ON pos_products(superseded_from_item_code)
  WHERE superseded_from_item_code IS NOT NULL;

CREATE INDEX idx_pos_product_stocks_item_branch 
  ON pos_product_stocks(item_number, branch_id);
```

---

## Testing Scenarios

### Scenario 1: Basic Supersession
```
Given: Old item ABC with 100 units in stock
When: User supersedes ABC → XYZ
Then: 
  ✓ Old ABC.superseded_to_item_code = "XYZ"
  ✓ New XYZ.superseded_from_item_code = "ABC"
  ✓ ABC stock = 0
  ✓ XYZ stock = 100
  ✓ Log entry created
```

### Scenario 2: Chain Supersession
```
Given: A→B→C chain exists, now superseding C→D
When: User confirms chain supersession
Then:
  ✓ C.superseded_to_item_code = "D"
  ✓ D.superseded_from_item_code = "C"
  ✓ A→B→C→D chain complete
```

### Scenario 3: Multiple Branches
```
Given: ABC with 100@Branch1, 50@Branch2
When: User selects both branches
Then:
  ✓ XYZ.stock@Branch1 = 100
  ✓ XYZ.stock@Branch2 = 50
  ✓ ABC.stock@Branch1 = 0
  ✓ ABC.stock@Branch2 = 0
```

### Scenario 4: Partial Transfer
```
Given: User unchecks "Transfer Stock"
When: Supersession executed
Then:
  ✓ Supersession links created
  ✓ Stock NOT transferred
  ✓ Old item stock unchanged
```

---

## Query Examples for Reports

### Items Currently in Supersession

```sql
SELECT 
    p1.item_number AS 'Old Code',
    p1.name AS 'Old Name',
    p1.superseded_to_item_code AS 'New Code',
    p2.name AS 'New Name',
    ISNULL(SUM(ps.qty), 0) AS 'Stock@New Item',
    p1.date_updated AS 'Supersede Date'
FROM pos_products p1
LEFT JOIN pos_products p2 ON p2.item_number = p1.superseded_to_item_code
LEFT JOIN pos_product_stocks ps ON ps.item_number = p2.item_number
WHERE p1.superseded_to_item_code IS NOT NULL
  AND TRIM(p1.superseded_to_item_code) != ''
GROUP BY p1.item_number, p1.name, p1.superseded_to_item_code, 
         p2.name, p1.date_updated
ORDER BY p1.date_updated DESC;
```

### Find Orphaned Links

```sql
-- Supersession links where target doesn't exist
SELECT 
    p.item_number,
    p.superseded_to_item_code AS 'Missing Target',
    p.date_updated
FROM pos_products p
LEFT JOIN pos_products target ON target.item_number = p.superseded_to_item_code
WHERE p.superseded_to_item_code IS NOT NULL
  AND TRIM(p.superseded_to_item_code) != ''
  AND target.id IS NULL
ORDER BY p.date_updated DESC;
```

### Full Audit Trail

```sql
SELECT 
    pl.id,
    pl.action,
    pl.description,
    u.username,
    b.name AS 'Branch',
    pl.date_time
FROM pos_logs pl
LEFT JOIN users u ON u.id = pl.user_id
LEFT JOIN branches b ON b.id = pl.branch_id
WHERE pl.action = 'Stock Suppression'
ORDER BY pl.date_time DESC;
```

---

## Debugging Tips

### Enable SQL Profiler to capture exact commands
```sql
-- Use SQL Server Profiler to trace:
-- - Transaction START
-- - UPDATE statements
-- - Any ROLLBACK
-- - Transaction COMMIT
```

### Check Audit Trail
```sql
SELECT * FROM pos_logs 
WHERE action = 'Stock Suppression'
ORDER BY date_time DESC
LIMIT 5;
```

### Verify Supersession Links
```sql
SELECT 
    item_number,
    superseded_from_item_code,
    superseded_to_item_code
FROM pos_products
WHERE superseded_from_item_code IS NOT NULL
  OR superseded_to_item_code IS NOT NULL;
```

### Monitor Transaction Locks
```sql
SELECT * FROM sys.dm_tran_locks WHERE session_id = @@SPID;
```

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | Current | Initial implementation with superseded_from_item_code and superseded_to_item_code |

---

**Last Updated**: [Current Date]  
**Maintainer**: [Your Name/Team]  
**Status**: Production Ready ✅
