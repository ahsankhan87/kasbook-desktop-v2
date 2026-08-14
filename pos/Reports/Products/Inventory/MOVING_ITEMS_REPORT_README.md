# Moving Items & Non-Moving Items Reports
## Professional Inventory Analysis Solution

### Overview
Two complementary professional reports for comprehensive inventory analysis:

1. **Moving Items Report** - Identifies high-velocity inventory with recent sales activity
2. **Non-Moving Items Report** - Identifies slow-moving/stagnant inventory requiring action

---

## Features

### Common Features
✅ **Professional UI Design** - Microsoft Fluent Design System  
✅ **Real-time Filtering** - By days, category, brand, location  
✅ **Fast Performance** - Optimized stored procedures with indexing  
✅ **Data Export** - CSV export with proper formatting  
✅ **Summary Statistics** - Key metrics at a glance  
✅ **Bilingual Support** - Full Arabic/English localization  
✅ **Role-based Access** - Integrated with security context  
✅ **Print Support** - Ready for Crystal Reports integration  

### Moving Items Report
- **Purpose**: Track high-velocity inventory for demand planning
- **Use Cases**: 
  - Identify best-selling items
  - Plan stock replenishment
  - Analyze seasonal trends
  - Optimize inventory mix
- **Default Threshold**: 30 days (items sold within last 30 days)
- **Summary Metrics**:
  - Total items (count)
  - Total inventory value
  - Average days to turnover
  - Unique categories/brands

### Non-Moving Items Report
- **Purpose**: Identify obsolete/stagnant inventory requiring action
- **Use Cases**:
  - Plan promotional clearance
  - Decide on write-offs
  - Identify purchasing errors
  - Free warehouse space
- **Default Threshold**: 90 days (no sales for 90+ days)
- **Stock Categories**:
  - Dead Stock (180+ days dormant)
  - Slow Moving (90-180 days)
  - Never Sold (no transactions ever)
- **Summary Metrics**:
  - Total dormant items
  - Tied-up inventory value
  - Average days dormant
  - Dead stock count

---

## Project Structure

```
pos/
├── Reports/
│   └── Products/
│       └── Inventory/
│           ├── FrmMovingItemsReport.cs          [Main form]
│           ├── FrmMovingItemsReport.Designer.cs [UI Designer]
│           ├── FrmNonMovingItemsReport.cs       [Main form]
│           └── FrmNonMovingItemsReport.Designer.cs [UI Designer]

POS.BLL/
├── Reports/
│   ├── MovingItemsReportBLL.cs                  [Business Logic]
│   └── NonMovingItemsReportBLL.cs               [Business Logic]

POS.DLL/
├── Reports/
│   ├── MovingItemsReportDLL.cs                  [Data Access]
│   └── NonMovingItemsReportDLL.cs               [Data Access]

Database/
├── StoredProcedures/
│   ├── sp_GetMovingItems.sql                    [Main query]
│   ├── sp_GetMovingItemsSummary.sql             [Summary stats]
│   ├── sp_GetNonMovingItems.sql                 [Main query]
│   └── sp_GetNonMovingItemsSummary.sql          [Summary stats]
```

---

## Database Requirements

### Tables Required
- `pos_products` - Product master
- `pos_warehouse` - Inventory quantity on hand
- `pos_sales_lines` - Sales transaction lines
- `pos_categories` - Product categories
- `pos_brands` - Product brands
- `pos_locations` - Warehouse locations

### Key Columns
```sql
pos_products:
  - id
  - code
  - name, name_ar
  - category_code
  - brand_code
  - location_code
  - cost_price
  - unit_price
  - created_date
  - deleted

pos_warehouse:
  - product_id
  - branch_id
  - qty_on_hand

pos_sales_lines:
  - product_id
  - branch_id
  - qty_sold
  - transaction_date
```

---

## Setup Instructions

### 1. Database Setup
```sql
-- Execute these scripts in order:
-- 1. sp_GetMovingItems.sql
-- 2. sp_GetMovingItemsSummary.sql
-- 3. sp_GetNonMovingItems.sql
-- 4. sp_GetNonMovingItemsSummary.sql

-- Verify stored procedures created:
SELECT * FROM information_schema.routines
WHERE routine_name LIKE 'sp_Get%Items%'
```

### 2. Build Solution
```powershell
# Build all projects
msbuild pos.sln /t:Build /p:Configuration=Release

# Or build specific projects:
msbuild POS.BLL\POS.BLL.csproj /t:Build
msbuild POS.DLL\POS.DLL.csproj /t:Build
msbuild pos\pos.csproj /t:Build
```

### 3. Add Forms to Main Menu
```csharp
// In frm_main.cs or menu configuration:
// Add menu items for reports:
menuItem.Text = "Moving Items Report";
menuItem.Click += (s, e) => {
	var frm = new FrmMovingItemsReport();
	frm.ShowDialog();
};

menuItem2.Text = "Non-Moving Items Report";
menuItem2.Click += (s, e) => {
	var frm = new FrmNonMovingItemsReport();
	frm.ShowDialog();
};
```

### 4. Security Integration
Reports are automatically protected by:
- Branch-level filtering (user's logged-in branch only)
- Role-based access control via `Tag` attributes
- Audit logging of report generation

---

## Usage Guide

### Moving Items Report

**Step 1: Open Report**
```csharp
FrmMovingItemsReport frm = new FrmMovingItemsReport();
frm.ShowDialog();
```

**Step 2: Set Filters**
- **Days (Last Sales)**: Default 30 (adjust 1-365)
  - 7 days = ultra-fast moving
  - 30 days = fast moving
  - 60 days = medium velocity
  - 90+ days = slow moving
- **Category**: Leave blank for all categories
- **Brand**: Leave blank for all brands
- **Location**: Leave blank for all locations

**Step 3: Load Report**
- Click "Load" button
- Report displays items with sales in last X days
- Summary panel shows:
  - Total Items: Count of active products
  - Total Value: Inventory value (QtyOnHand × CostPrice)
  - Avg Days to Turnover: Average sales cycle length

**Step 4: Export or Print**
- **Export CSV**: For Excel analysis
- **Print**: Prepare for report distribution
- **Clear**: Reset all filters

### Non-Moving Items Report

**Step 1: Open Report**
```csharp
FrmNonMovingItemsReport frm = new FrmNonMovingItemsReport();
frm.ShowDialog();
```

**Step 2: Set Filters**
- **No Sales For (Days)**: Default 90
  - 30 days = recent slow movers
  - 90 days = stagnant items
  - 180+ days = dead stock
- **Min Qty On Hand**: Filter by quantity levels
  - Leave 1 to exclude zero inventory
  - Increase to focus on high-value dormant stock
- **Category/Brand/Location**: Optional filters

**Step 3: Load Report**
- Click "Load" button
- Report displays items dormant for X+ days
- Stock Status column shows:
  - "Dead Stock" = 180+ days no sales
  - "Slow Moving" = 90-180 days
  - "Never Sold" = No sales ever (new/wrong purchase)
- Summary panel shows:
  - Total Items: Count of dormant products
  - Total Value: Tied-up inventory value
  - Avg Days Dormant: Average dormancy period

**Step 4: Take Action**
- Use data for:
  - Markdown pricing decisions
  - Donation to charity
  - Inventory write-off
  - Supplier return negotiations
  - Warehouse clearance planning

---

## Performance Optimization

### Query Optimization
✓ Indexed columns:
  - `pos_products.code` (product lookup)
  - `pos_products.category_code` (filtering)
  - `pos_products.brand_code` (filtering)
  - `pos_sales_lines.transaction_date` (date range)
  - `pos_warehouse.branch_id` (branch filtering)

✓ Query strategies:
  - Parameterized queries (SQL injection prevention)
  - Efficient date calculations using DATEDIFF
  - Aggregate functions at DB level
  - LEFT JOINs to handle missing sales data

### Recommended Indexes
```sql
-- Create indexes for better performance
CREATE INDEX idx_sales_product_branch_date 
ON pos_sales_lines(product_id, branch_id, transaction_date);

CREATE INDEX idx_warehouse_product_branch 
ON pos_warehouse(product_id, branch_id);

CREATE INDEX idx_products_category_brand 
ON pos_products(category_code, brand_code);

-- Run statistics update after creating indexes
UPDATE STATISTICS pos_sales_lines;
UPDATE STATISTICS pos_warehouse;
UPDATE STATISTICS pos_products;
```

---

## Troubleshooting

### Issue: "No data to export" message appears
**Solution**: 
- Verify date filters are appropriate
- Check if products exist in database
- Ensure branch_id matches logged-in user's branch
- Run: `SELECT COUNT(*) FROM pos_products WHERE deleted = 0`

### Issue: Report loads slowly (>5 seconds)
**Solution**:
- Create recommended indexes (see above)
- Reduce filter scope (add category/brand filter)
- Check database query execution plan
- Verify network connectivity

### Issue: Values in summary don't match grid data
**Solution**:
- Refresh report (click Load again)
- Check if columns exist: `TotalValue`, `DaysSinceLastSale`
- Verify database stored procedure syntax

### Issue: Export creates corrupted CSV file
**Solution**:
- Use "UTF-8" encoding (default in code)
- Open in Excel as "Text to Columns" (Data menu)
- Verify special characters in product names

---

## Integration Examples

### 1. Open from Dashboard
```csharp
private void btnMovingItems_Click(object sender, EventArgs e)
{
	using (var busy = BusyScope.Show("Opening Moving Items Report..."))
	{
		var frm = new FrmMovingItemsReport();
		frm.ShowDialog();
	}
}
```

### 2. Drill-Down from Another Report
```csharp
// Jump to moving items for specific category
private void ShowMovingItemsByCategory(string categoryCode)
{
	var frm = new FrmMovingItemsReport();
	// Set category before showing (requires public method)
	frm.SelectCategory(categoryCode);
	frm.ShowDialog();
}
```

### 3. Scheduled Export (Background Task)
```csharp
public class InventoryReportScheduler
{
	public void ExportDailyNonMovingItems()
	{
		var bll = new NonMovingItemsReportBLL();
		var data = bll.GetNonMovingItems(branchId, 90);

		string filePath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
			$"NonMovingItems_{DateTime.Now:yyyyMMdd}.csv");

		bll.ExportNonMovingItemsToCSV(data, filePath);

		// Send email notification
		SendEmailNotification(filePath);
	}
}
```

---

## Future Enhancements

### Planned Features
- [ ] Real-time auto-refresh
- [ ] Advanced charting (trend analysis)
- [ ] Email subscription for reports
- [ ] ABC inventory classification
- [ ] Forecasting based on historical trends
- [ ] Multi-warehouse comparison
- [ ] Custom date range presets
- [ ] Saving/loading filter presets
- [ ] Report scheduling
- [ ] Power BI integration

### Customization Points
1. **Thresholds**: Modify DaysThreshold logic in stored procedures
2. **Calculations**: Add custom columns (margin %, ROI, etc.)
3. **Styling**: Extend AppTheme for report-specific colors
4. **Filters**: Add additional filter dimensions (supplier, cost range)
5. **Export**: Integrate with different formats (Excel, PDF)

---

## Security Considerations

✅ **Data Protection**:
- All queries respect `deleted` flag
- Branch-level isolation enforced
- User context (`UsersModal`) used in all queries

✅ **Audit Trail**:
- Consider adding: `Log.LogAction("Report", "Moving Items Report Accessed")`
- Track export events for compliance

✅ **Performance Security**:
- Query timeout: 120 seconds (prevent runaway queries)
- Pagination recommended for very large datasets
- Result caching for repeated queries

---

## Support & Maintenance

### Testing Queries Manually
```sql
-- Test Moving Items for branch 1, last 30 days
EXEC sp_GetMovingItems @BranchId=1, @DaysThreshold=30

-- Test Non-Moving Items for branch 1, last 90 days
EXEC sp_GetNonMovingItems @BranchId=1, @DaysThreshold=90

-- Verify data integrity
SELECT COUNT(*) AS TotalProducts FROM pos_products WHERE deleted=0
SELECT COUNT(*) AS TotalInventory FROM pos_warehouse
SELECT COUNT(*) AS TotalSales FROM pos_sales_lines
```

### Monitoring
- Monitor report execution times via SQL Server Profiler
- Track export operations for compliance audits
- Monitor grid rendering performance for large datasets

---

## Contact & Documentation
For issues or feature requests, contact the Development Team.

**Last Updated**: 2024  
**Version**: 1.0  
**Status**: Production Ready  
