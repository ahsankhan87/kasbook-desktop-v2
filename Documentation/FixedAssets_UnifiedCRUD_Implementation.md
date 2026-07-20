# Fixed Assets Module - Unified CRUD Pattern Implementation

## Overview
The Fixed Assets module now follows the **standard CRUD pattern** used throughout the KasBook application (similar to `sp_banksCrud`), consolidating multiple stored procedures into single unified procedures with `OperationType` parameters.

---

## Database Layer - SQL Stored Procedures

### Three Unified CRUD Procedures

#### 1. **sp_FixedAssetCategoriesCrud**
```sql
OperationTypes:
  1 = Insert    → INSERT INTO fa_categories
  2 = Update    → UPDATE fa_categories WHERE category_id = @category_id
  3 = Delete    → SOFT DELETE (mark inactive if in use) / HARD DELETE
  4 = Select One → SELECT * FROM fa_categories WHERE category_id = @category_id
  5 = Select All → SELECT * FROM fa_categories (filtered by @is_active)
```

**Parameters:**
- `@category_id` - Primary key (OUTPUT for Insert)
- `@category_code` - Code (varchar 20)
- `@category_name` - Name (nvarchar 150)
- `@depreciation_method` - STRAIGHT_LINE, REDUCING_BALANCE, etc.
- `@useful_life_months` - Default 60
- `@annual_depreciation_rate` - Optional decimal
- `@is_active` - Soft delete flag
- `@OperationType` - 1, 2, 3, 4, or 5

---

#### 2. **sp_FixedAssetLocationsCrud**
```sql
OperationTypes:
  1 = Insert    → INSERT INTO fa_locations
  2 = Update    → UPDATE fa_locations WHERE location_id = @location_id
  3 = Delete    → SOFT DELETE (mark inactive if in use) / HARD DELETE
  4 = Select One → SELECT * FROM fa_locations WHERE location_id = @location_id
  5 = Select All → SELECT * FROM fa_locations (filtered by @is_active)
```

**Parameters:**
- `@location_id` - Primary key (OUTPUT for Insert)
- `@location_code` - Code (varchar 20)
- `@location_name` - Name (nvarchar 150)
- `@location_type` - LOCATION, DEPARTMENT, SITE, BRANCH
- `@parent_location_id` - Optional parent for hierarchies
- `@is_active` - Soft delete flag
- `@OperationType` - 1, 2, 3, 4, or 5

---

#### 3. **sp_FixedAssetsCrud**
```sql
OperationTypes:
  1 = Insert    → INSERT INTO fa_assets (with cost immutable after creation)
  2 = Update    → UPDATE fa_assets (only asset_name, location_id, notes, is_active)
  3 = Delete    → SOFT DELETE (mark inactive)
  4 = Select One → SELECT * FROM fa_assets WHERE asset_id = @asset_id
  5 = Select All → SELECT * FROM fa_assets (filtered by @is_active)
```

**Parameters:**
- `@asset_id` - Primary key (OUTPUT for Insert)
- `@asset_code` - Code (varchar 50, unique)
- `@asset_name` - Name (nvarchar 200)
- `@category_id` - Foreign key to fa_categories
- `@location_id` - Foreign key to fa_locations (nullable)
- `@serial_number` - Optional
- `@purchase_date` - Required for Insert
- `@cost` - IMMUTABLE after creation (not updatable)
- `@dep_method` - STRAIGHT_LINE, REDUCING_BALANCE, UNITS_OF_PRODUCTION
- `@useful_life_months` - In months
- `@salvage_value` - Expected residual value
- `@replacement_cost` - Optional
- `@notes` - Additional notes
- `@is_active` - Soft delete flag
- `@created_by` - User ID who created
- `@OperationType` - 1, 2, 3, 4, or 5

---

## C# Data Layer (POS.DLL)

### FixedAssetDLL Class

All methods call the unified CRUD procedures with the appropriate `@OperationType`:

#### Category Methods
```csharp
// OperationType 5: Select All
public DataTable GetCategories(bool activeOnly = true)

// OperationType 1: Insert
public int InsertCategory(string categoryCode, string categoryName, ...)

// OperationType 2: Update
public int UpdateCategory(int categoryId, string categoryName, ...)

// OperationType 3: Delete
public int DeleteCategory(int categoryId)
```

#### Location Methods
```csharp
// OperationType 5: Select All
public DataTable GetLocations(bool activeOnly = true)

// OperationType 1: Insert
public int InsertLocation(string locationCode, string locationName, ...)

// OperationType 2: Update
public int UpdateLocation(int locationId, string locationName, ...)

// OperationType 3: Delete
public int DeleteLocation(int locationId)
```

#### Asset Methods
```csharp
// OperationType 1: Insert
public int InsertAsset(string assetCode, string assetName, ...)

// OperationType 2: Update (only non-cost fields)
public int UpdateAssetDetails(int assetId, string assetName, ...)

// OperationType 3: Delete
public int DeleteAsset(int assetId)
```

---

## Usage Examples

### C# - Insert Category
```csharp
FixedAssetDLL dll = new FixedAssetDLL();

// Calls sp_FixedAssetCategoriesCrud with @OperationType = 1
int newCategoryId = dll.InsertCategory(
	categoryCode: "BLDG",
	categoryName: "Building",
	deprecationMethod: "STRAIGHT_LINE",
	usefulLifeMonths: 40,
	annualDepreciationRate: 2.5m
);
```

### C# - Get All Locations
```csharp
FixedAssetDLL dll = new FixedAssetDLL();

// Calls sp_FixedAssetLocationsCrud with @OperationType = 5
DataTable locations = dll.GetLocations(activeOnly: true);

foreach (DataRow row in locations.Rows)
{
	int locId = (int)row["location_id"];
	string locName = row["location_name"].ToString();
	// Use data...
}
```

### C# - Update Asset Details Only
```csharp
FixedAssetDLL dll = new FixedAssetDLL();

// Calls sp_FixedAssetsCrud with @OperationType = 2
// Note: Cost, dep_method, useful_life_months are NOT updated (immutable)
int result = dll.UpdateAssetDetails(
	assetId: 42,
	assetName: "Updated Name",
	locationId: 5,
	notes: "Moved to Warehouse",
	isActive: true
);
```

---

## Pattern Benefits

✅ **Consistent with KasBook Architecture**
- Matches existing patterns (sp_banksCrud, sp_companiesCrud, etc.)
- Familiar to the development team

✅ **Reduced Procedure Clutter**
- 3 unified procedures instead of 13 separate ones
- Easier maintenance and version control
- Less SQL Server database bloat

✅ **Type-Safe Operation**
- `@OperationType` parameter prevents accidental wrong operation
- Compiler enforces parameter types in C#

✅ **Business Logic Protection**
- Costs and depreciation methods immutable after creation
- Soft deletes preserve audit trail and referential integrity
- Automatic `created_at`, `updated_at` timestamps

✅ **Clear Return Values**
- Insert operations return the new ID
- Update/Delete operations return 0 for success, -1 for errors
- Soft delete returns 1 if records were soft-deleted, 0 if hard-deleted

---

## Design Patterns Implemented

### Soft Delete Strategy
```sql
-- For categories/locations WITH assets
UPDATE fa_categories SET is_active = 0 WHERE category_id = @id;
-- Returns 1 (soft deleted)

-- For categories/locations WITHOUT assets (unused)
DELETE FROM fa_categories WHERE category_id = @id;
-- Returns 0 (hard deleted)
```

### Immutable Cost Fields
```csharp
// Insert - all fields allowed
InsertAsset(cost: 50000, dep_method: "STRAIGHT_LINE", ...)

// Update - cost and dep_method NOT in UPDATE statement
UpdateAssetDetails(assetId: 1, assetName: "New Name", ...)
// Only asset_name, location_id, notes, is_active are updatable
```

### Parameter Filtering
```sql
-- In Select All procedures:
WHERE @is_active = 0 OR is_active = 1
-- When @is_active = 0: returns both active AND inactive
-- When @is_active = 1: returns only active records
```

---

## Testing Scenarios

### Scenario 1: Add Category & Use It
```csharp
// 1. Insert category
int catId = dll.InsertCategory("FURN", "Furniture", "STRAIGHT_LINE", 10);

// 2. Use it in asset creation
int assetId = dll.InsertAsset("DESK001", "Office Desk", catId, ...);

// 3. Try to delete category (should soft-delete since it's in use)
dll.DeleteCategory(catId);  // Returns 1 (soft deleted, still appears in assets)
```

### Scenario 2: Update Only Safe Fields
```csharp
// 1. Create asset with cost 100000
int assetId = dll.InsertAsset("BUILD001", "Building", catId, cost: 100000, ...);

// 2. Try to change cost (will be ignored in UPDATE)
dll.UpdateAssetDetails(assetId, assetName: "New Building Name", ...);
// Cost remains 100000 ✓

// 3. Location and notes can be changed
dll.UpdateAssetDetails(assetId, 
	assetName: "Main Building", 
	locationId: 2, 
	notes: "Moved to HQ",
	isActive: true
);  // ✓ Works
```

### Scenario 3: Unused Category Can Be Hard-Deleted
```csharp
// 1. Create category
int catId = dll.InsertCategory("TEST", "Test Category", ...);

// 2. Delete immediately without using (hard delete)
dll.DeleteCategory(catId);  // Returns 0 (hard deleted)

// 3. Verify it's gone (database select finds nothing)
```

---

## Migration Notes

### From Old Procedure Pattern
```csharp
// OLD (separate procedures):
int newId = dll.InsertCategory(code, name);
dll.UpdateCategory(id, name, method);
dll.DeleteCategory(id);

// NEW (unified procedure):
int newId = dll.InsertCategory(code, name);  // Calls sp with @OperationType=1
dll.UpdateCategory(id, name, method);        // Calls sp with @OperationType=2
dll.DeleteCategory(id);                      // Calls sp with @OperationType=3
```

**No changes needed in calling code!** The DLL wrapper methods remain identical from the caller's perspective.

---

## SQL Script Deployment

To deploy the unified procedures:

```sql
-- Run this script on pos_db_v3.0.0:
-- Database\FixedAssets_CategoryLocation_CRUD.sql

-- Creates:
-- 1. sp_FixedAssetCategoriesCrud
-- 2. sp_FixedAssetLocationsCrud
-- 3. sp_FixedAssetsCrud

-- Replaces (drops and recreates):
-- - sp_FixedAsset_GetCategories
-- - sp_FixedAsset_InsertCategory
-- - sp_FixedAsset_UpdateCategory
-- - sp_FixedAsset_DeleteCategory
-- - sp_FixedAsset_GetLocations
-- - sp_FixedAsset_InsertLocation
-- - sp_FixedAsset_UpdateLocation
-- - sp_FixedAsset_DeleteLocation
-- - sp_FixedAsset_InsertAsset
-- - sp_FixedAsset_UpdateAssetDetails
```

---

## Conclusion

The Fixed Assets module now follows **KasBook's established CRUD pattern**, improving code maintainability, consistency, and reducing database procedure complexity by 70% (from 13 procedures to 3). All functionality is preserved while providing a cleaner, more scalable architecture.
