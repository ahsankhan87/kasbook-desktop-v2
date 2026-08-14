# Implementation Checklist for Moving & Non-Moving Items Reports

## Quick Start Checklist

### Phase 1: Database Setup (30 minutes)
- [ ] Copy SQL stored procedure files to Database/StoredProcedures/
  - `sp_GetMovingItems.sql`
  - `sp_GetNonMovingItems.sql`
- [ ] Execute stored procedures in SQL Server Management Studio
- [ ] Verify procedures created: 
  ```sql
  SELECT * FROM sys.objects WHERE name LIKE 'sp_Get%Items%'
  ```
- [ ] Create recommended indexes:
  ```sql
  CREATE INDEX idx_sales_product_branch_date 
  ON pos_sales_lines(product_id, branch_id, transaction_date);
  ```

### Phase 2: Solution Build (15 minutes)
- [ ] Add MovingItemsReportBLL.cs to POS.BLL\Reports\
- [ ] Add NonMovingItemsReportBLL.cs to POS.BLL\Reports\
- [ ] Add MovingItemsReportDLL.cs to POS.DLL\Reports\
- [ ] Add NonMovingItemsReportDLL.cs to POS.DLL\Reports\
- [ ] Build Solution → Verify no compilation errors
  ```powershell
  msbuild pos.sln /t:Build /p:Configuration=Release
  ```

### Phase 3: UI Forms (15 minutes)
- [ ] Add FrmMovingItemsReport.cs to pos\Reports\Products\Inventory\
- [ ] Add FrmMovingItemsReport.Designer.cs to pos\Reports\Products\Inventory\
- [ ] Add FrmNonMovingItemsReport.cs to pos\Reports\Products\Inventory\
- [ ] Add FrmNonMovingItemsReport.Designer.cs to pos\Reports\Products\Inventory\
- [ ] Build Solution → Forms should appear in designer toolbox

### Phase 4: Menu Integration (20 minutes)
- [ ] Locate frm_main.cs (Main application form)
- [ ] Add menu items for reports:
  ```csharp
  // Under Inventory or Reports menu
  var movingItemsMenuItem = new ToolStripMenuItem("Moving Items Report");
  movingItemsMenuItem.Click += (s, e) => {
	  var frm = new FrmMovingItemsReport();
	  frm.ShowDialog(this);
  };

  var nonMovingItemsMenuItem = new ToolStripMenuItem("Non-Moving Items Report");
  nonMovingItemsMenuItem.Click += (s, e) => {
	  var frm = new FrmNonMovingItemsReport();
	  frm.ShowDialog(this);
  };

  inventoryMenu.DropDownItems.Add(movingItemsMenuItem);
  inventoryMenu.DropDownItems.Add(nonMovingItemsMenuItem);
  ```
- [ ] Test menu items → Forms should open and respond

### Phase 5: Testing (30 minutes)

#### Unit Testing
- [ ] Test Moving Items Report with:
  - Different DaysThreshold values (7, 30, 60, 90)
  - Category filters
  - Brand filters
  - Location filters
- [ ] Test Non-Moving Items Report with:
  - Different DaysThreshold values (30, 90, 180)
  - Min Qty filter
  - All filter combinations
- [ ] Verify summary statistics calculations

#### Integration Testing
- [ ] Verify data accuracy:
  ```sql
  -- Sample query to validate
  EXEC sp_GetMovingItems @BranchId=1, @DaysThreshold=30
  ```
- [ ] Check grid formatting:
  - Currency columns show "N2" format
  - Date columns show "yyyy-MM-dd"
  - Quantity columns are right-aligned
- [ ] Test export functionality:
  - Export to CSV
  - Verify headers and data
  - Check special characters handling
- [ ] Test security:
  - Logged-in user's branch data only
  - No data leakage across branches

#### Performance Testing
- [ ] Load report with 1000+ items → Should load in <5 seconds
- [ ] Monitor memory usage during export
- [ ] Check database query execution plan for optimization

### Phase 6: Security Review (15 minutes)
- [ ] Verify branch-level isolation in queries
- [ ] Confirm deleted flag filtering in WHERE clause
- [ ] Check parameter validation (SQL injection prevention)
- [ ] Review stored procedure permissions
- [ ] Add role-based access control via Tag attribute if needed:
  ```csharp
  btnLoad.Tag = "InventoryReportView";
  ```

### Phase 7: Documentation (10 minutes)
- [ ] Review MOVING_ITEMS_REPORT_README.md
- [ ] Update application help system with report documentation
- [ ] Create quick reference guide for users
- [ ] Add keyboard shortcuts if needed

## File Checklist

### Core Files Created ✓
```
POS.BLL/
├── Reports/
│   ├── MovingItemsReportBLL.cs ✓
│   └── NonMovingItemsReportBLL.cs ✓

POS.DLL/
├── Reports/
│   ├── MovingItemsReportDLL.cs ✓
│   └── NonMovingItemsReportDLL.cs ✓

pos/Reports/Products/Inventory/
├── FrmMovingItemsReport.cs ✓
├── FrmMovingItemsReport.Designer.cs ✓
├── FrmNonMovingItemsReport.cs ✓
├── FrmNonMovingItemsReport.Designer.cs ✓
└── MOVING_ITEMS_REPORT_README.md ✓

Database/StoredProcedures/
├── sp_GetMovingItems.sql ✓
├── sp_GetMovingItemsSummary.sql ✓
├── sp_GetNonMovingItems.sql ✓
└── sp_GetNonMovingItemsSummary.sql ✓
```

## Common Issues & Solutions

### Issue 1: "Error fetching moving items: ..."
**Cause**: Stored procedure doesn't exist or syntax error  
**Solution**:
```sql
-- Verify procedure exists
SELECT * FROM sys.objects WHERE name = 'sp_GetMovingItems'
-- Re-execute SQL script if missing
```

### Issue 2: "No data to export" after loading report
**Cause**: Filter criteria too restrictive  
**Solution**:
- Click "Clear" button to reset filters
- Use "-- All --" for category/brand/location
- Reduce DaysThreshold value

### Issue 3: Grid columns not formatted correctly
**Cause**: Column names in DataTable don't match format check code  
**Solution**:
```csharp
// Verify column names returned by stored procedure
DataTable dt = bll.GetMovingItems(1);
foreach (DataColumn col in dt.Columns)
	Debug.WriteLine(col.ColumnName);
// Adjust FormatGridColumns() method if needed
```

### Issue 4: Export file is empty or corrupted
**Cause**: UTF-8 encoding issue or special characters  
**Solution**:
- Use UTF-8 BOM encoding
- Test with simple ASCII product names first
- Try opening CSV with different encodings

## Performance Benchmarks (Expected)

| Operation | Time | Condition |
|-----------|------|-----------|
| Load Moving Items | <2s | 100-500 items |
| Load Non-Moving Items | <3s | 50-200 items |
| Export to CSV | <1s | <1000 rows |
| Print setup | <2s | Report preparation |
| Filter application | <1s | Dynamic filtering |

## Deployment Checklist

- [ ] All files compiled without errors
- [ ] Solution builds successfully in Release mode
- [ ] Stored procedures created in production database
- [ ] Menu items added to application
- [ ] Reports accessible from main form
- [ ] Sample data loaded for testing
- [ ] User training materials prepared
- [ ] Backup of database taken before deployment
- [ ] Application released to QA environment
- [ ] UAT sign-off obtained
- [ ] Deployed to production

## Post-Deployment Tasks

- [ ] Monitor report usage patterns
- [ ] Check performance metrics (load times)
- [ ] Gather user feedback
- [ ] Plan Phase 2 enhancements:
  - Charting/visualizations
  - Email reporting
  - Power BI integration
  - ABC classification

## Support Contacts

**Development Lead**: [Your Name]  
**Database Administrator**: [DBA Name]  
**QA Lead**: [QA Name]  

---

**Document Status**: Ready for Implementation  
**Last Updated**: 2024  
**Reviewed By**: Senior Developer  
