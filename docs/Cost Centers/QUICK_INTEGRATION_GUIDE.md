# Sub-Ledger Forms - Quick Integration Guide

## Step 1: Deploy Database Objects

Run these SQL scripts in SQL Server Management Studio in order:

```sql
-- 1. Create the views (must be done before procedures)
EXEC sp_executesql N'... (copy content of vw_customer_subledger_entries.sql)'
EXEC sp_executesql N'... (copy content of vw_supplier_subledger_entries.sql)'
EXEC sp_executesql N'... (copy content of vw_cashbook_entries.sql)'

-- 2. Create the stored procedures
EXEC sp_executesql N'... (copy content of sp_CustomerSubLedger.sql)'
EXEC sp_executesql N'... (copy content of sp_SupplierSubLedger.sql)'
EXEC sp_executesql N'... (copy content of sp_CashBook.sql)'
```

**Alternative:** Use SQL Server Publish or migration tool if available.

## Step 2: Verify Database Connection

Update the view/procedure definitions if your table names differ:

| Standard Name | Your Table Name | Update In |
|---------------|-----------------|-----------|
| pos_customers | [Your table] | vw_customer_subledger_entries.sql |
| pos_suppliers | [Your table] | vw_supplier_subledger_entries.sql |
| acc_accounts | [Your table] | vw_cashbook_entries.sql, sp_CashBook.sql |
| pos_sales_master | [Your table] | vw_customer_subledger_entries.sql |
| pos_purchase_master | [Your table] | vw_supplier_subledger_entries.sql |

## Step 3: Add Menu Items

In your main menu (e.g., `Main.cs` or `frm_main.cs`):

```csharp
// In the Reports menu or Accounts menu
ToolStripMenuItem menuSubLedgers = new ToolStripMenuItem("Sub-Ledgers");
menuSubLedgers.DropDownItems.Add("Customer (AR)", null, (s, e) => {
	var form = new FrmCustomerSubLedger();
	form.ShowDialog();
});
menuSubLedgers.DropDownItems.Add("Supplier (AP)", null, (s, e) => {
	var form = new FrmSupplierSubLedger();
	form.ShowDialog();
});
menuSubLedgers.DropDownItems.Add("Cash Book", null, (s, e) => {
	var form = new FrmCashBook();
	form.ShowDialog();
});

// Add to appropriate menu
menuReports.DropDownItems.Add(menuSubLedgers);
```

## Step 4: Handle Column Name Mismatches

If you get "column not found" errors, check these common names:

**Customer Table:**
```csharp
// If different, update FrmCustomerSubLedger.cs DisplayEntityInfo()
// Standard assumes: first_name, last_name, contact_no, email, address
```

**Supplier Table:**
```csharp
// If different, update FrmSupplierSubLedger.cs DisplayEntityInfo()
// Standard assumes: name, contact_number, email, address
```

**Account Table:**
```csharp
// If different, update FrmCashBook.cs and sp_CashBook.sql
// Standard assumes: id, acc_name, acc_code
```

## Step 5: Test the Forms

### Test Customer Sub-Ledger:
1. Open form from menu
2. Select a customer with invoices
3. Verify ledger entries display
4. Check aging analysis updates
5. Test Print and Export

### Test Supplier Sub-Ledger:
1. Open form from menu
2. Select a supplier with bills
3. Verify ledger entries display
4. Check aging analysis updates
5. Test Print and Export

### Test Cash Book:
1. Open form from menu
2. Select a cash account (or "All")
3. Verify transaction entries display (should show receipts and payments)
4. Test Print and Export

## Troubleshooting

### Error: "Could not find stored procedure"
- ✓ Verify SQL scripts were executed in database
- ✓ Check connection string is correct
- ✓ Verify procedure name spelling matches

### Error: "Invalid column name"
- ✓ Check your table structure vs. assumed names
- ✓ Update view or form code to match your columns
- ✓ Run `SELECT * FROM your_table` to see actual column names

### Error: "No data appears in grid"
- ✓ Verify you have transactions in the date range
- ✓ Check if soft-delete flags are filtering out data
- ✓ Test stored procedure directly: `EXEC sp_CustomerSubLedger @CustomerId=1`

### Error: "Object reference not set (NullReferenceException)"
- ✓ Check entity selector is populated before loading
- ✓ Verify customer/supplier exists with that ID
- ✓ Check date pickers have valid dates

### Performance Issue: Slow loading with large datasets
- ✓ Add database indexes on transaction date columns
- ✓ Reduce date range for testing
- ✓ Check if views are filtering efficiently

## Customization Examples

### Add a "Days Outstanding" Column

In `FrmCustomerSubLedger.cs` InitializeGrid():
```csharp
protected override void InitializeGrid()
{
	base.InitializeGrid();
	// Add custom column
	AddGridColumn("Days Outstanding", "days_overdue", 
		System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter, 100);
}
```

### Change Aging Bucket Thresholds

In stored procedures, modify these lines:
```sql
-- Current: 0-30, 31-60, 61-90, 90+
-- Change to: 0-60, 61-120, 120+
WHEN DATEDIFF(DAY, due_date, GETDATE()) <= 60 THEN ... -- New 0-60
WHEN DATEDIFF(DAY, due_date, GETDATE()) > 60 AND DATEDIFF(DAY, due_date, GETDATE()) <= 120 THEN ... -- New 61-120
```

### Auto-Load on Form Open

In `FrmCustomerSubLedger.cs` OnFormLoad():
```csharp
protected override void OnFormLoad()
{
	base.OnFormLoad();
	// Auto-select first customer if only one
	if (cmbEntity.Items.Count == 2) // 1 for empty + 1 for customer
	{
		cmbEntity.SelectedIndex = 1;
		LoadLedger();
	}
}
```

### Add Print-to-PDF

Create override in derived form:
```csharp
protected override void PrintLedger()
{
	// Use PrintDocument with PDF printer
	// Or integrate iTextSharp/PdfSharp
}
```

## Performance Optimization Tips

1. **Add Indexes** on SQL Server:
   ```sql
   CREATE INDEX idx_sales_customer_date ON pos_sales_master(customer_id, sale_date);
   CREATE INDEX idx_purchases_supplier_date ON pos_purchase_master(supplier_id, purchase_date);
   ```

2. **Reduce Date Range** in testing
3. **Batch Load** aging data instead of per-row queries
4. **Cache** customer/supplier lists if rarely changing
5. **Pagination** for very large datasets (modify sp to use OFFSET/FETCH)

## Security Considerations

✅ **Already Implemented:**
- Uses parameterized queries (safe from SQL injection)
- Respects deleted_at soft-delete flags
- Uses branch_id for multi-branch isolation

⚠️ **To Add:**
- Verify user has permission to view AR/AP/Cash
- Add [Permission("ARLedger")] attributes to forms
- Audit log who exports what data
- Restrict print/export by role

## Next Steps

1. ✓ Deploy database scripts
2. ✓ Adjust table/column names if needed
3. ✓ Add menu items to main form
4. ✓ Build and test
5. → Document any field mapping changes
6. → Train users on report features
7. → Monitor performance and optimize if needed

---

**Support:** If you encounter issues, check:
- Build output for compilation errors
- Database error logs for procedure failures
- Visual Studio Output window for runtime exceptions
- SQL Profiler to trace procedure execution
