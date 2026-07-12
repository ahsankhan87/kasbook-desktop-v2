# Sub-Ledger Database Schema Updates - Corrected Field Mappings

## Summary of Changes

All views and stored procedures have been updated with corrected table and field names based on your actual database schema.

### ⚠️ IMPORTANT: Payment Terms Structure
**payment_terms_id IS a foreign key to the pos_payment_terms table.**  
The `code` field in `pos_payment_terms` contains the **number of days** (e.g., "30", "60", "90").  
The calculation uses: `DATEADD(DAY, CASE WHEN ISNUMERIC(pt.code) = 1 THEN CAST(pt.code AS INT) ELSE 30 END, date_column)`  
This structure has been corrected in both views to properly join to `pos_payment_terms`.

---

## Corrected Table and Field Mappings

### Customer Subledger (Sales & Payments)

| Concept | Table | Field | Type | Notes |
|---------|-------|-------|------|-------|
| Sales Table | pos_sales | - | - | ✓ Main source |
| Sales Date | pos_sales | sale_date | DATE | ✓ Transaction date |
| Sales Amount | pos_sales | total_amount | DECIMAL | ✓ Invoice amount |
| Invoice Number | pos_sales | invoice_no | VARCHAR | ✓ Reference |
| Customer ID | pos_sales | customer_id | INT | ✓ FK to customer |
| Payment Terms FK | pos_sales | payment_terms_id | INT | ✓ **FK to pos_payment_terms.id** |
| Payment Terms Days | pos_payment_terms | code | VARCHAR | ✓ **Stores days value** |
| Payment Table | pos_customers_payments | - | - | ✓ Payment records |
| Payment Date | pos_customers_payments | entry_date | DATE | ✓ Payment date |
| Payment Amount | pos_customers_payments | credit | DECIMAL | ✓ Amount received |
| Invoice Reference | pos_customers_payments | invoice_no | VARCHAR | ✓ Join field |

### Supplier Subledger (Purchases & Payments)

| Concept | Table | Field | Type | Notes |
|---------|-------|-------|------|-------|
| Purchase Table | pos_purchase | - | - | ✓ Main source |
| Purchase Date | pos_purchase | purchase_date | DATE | ✓ Transaction date |
| Purchase Amount | pos_purchase | total_amount | DECIMAL | ✓ Bill amount |
| Invoice Number | pos_purchase | invoice_no | VARCHAR | ✓ Reference |
| Supplier ID | pos_purchase | supplier_id | INT | ✓ FK to supplier |
| Payment Terms FK | pos_purchase | payment_terms_id | INT | ✓ **FK to pos_payment_terms.id** |
| Payment Terms Days | pos_payment_terms | code | VARCHAR | ✓ **Stores days value** |
| Payment Table | pos_suppliers_payments | - | - | ✓ Payment records |
| Payment Date | pos_suppliers_payments | entry_date | DATE | ✓ Payment date |
| Payment Amount | pos_suppliers_payments | debit | DECIMAL | ✓ Amount paid |
| Invoice Reference | pos_suppliers_payments | invoice_no | VARCHAR | ✓ Join field |

### Cash Book (All Cash Transactions)

| Concept | Old Name | New Name | Table | Field |
|---------|----------|----------|-------|-------|
| Payment Table | pos_customer_payments | **pos_customers_payments** | pos_customers_payments | ✓ |
| Receipt Amount | payment_amount | **credit** | pos_customers_payments | ✓ |
| Payment Table | pos_supplier_payments | **pos_suppliers_payments** | pos_suppliers_payments | ✓ |
| Payment Amount | payment_amount | **debit** | pos_suppliers_payments | ✓ |
| Bank Deposits | pos_bank_deposits | **pos_bank_deposits** | pos_bank_deposits | ✓ |
| Bank Withdrawals | pos_bank_withdrawals | **pos_bank_withdrawals** | pos_bank_withdrawals | ✓ |
| Journal Vouchers | acc_journal_vouchers | **acc_journal_vouchers** | acc_journal_vouchers | ✓ |

---

## Updated Views

### 1. vw_customer_subledger_entries
**Location:** `Database\StoredProcedures\vw_customer_subledger_entries.sql`

**Changes:**
- Table: `pos_sales_master` → `pos_sales`
- Payment table: `pos_customer_payments` → `pos_customers_payments`
- Field: `payment_date` → `entry_date`
- Field: `payment_amount` → `credit`
- Join condition: Uses `invoice_no` matching

**Result Columns:**
```
customer_id, transaction_date, transaction_type, reference_no, invoice_no,
description, is_debit (1=invoice, 0=payment), amount, due_date, status,
branch_id, entry_id
```

### 2. vw_supplier_subledger_entries
**Location:** `Database\StoredProcedures\vw_supplier_subledger_entries.sql`

**Changes:**
- Table: `pos_purchase_master` → `pos_purchase`
- Payment table: `pos_supplier_payments` → `pos_suppliers_payments`
- Field: `payment_date` → `entry_date`
- Field: `payment_amount` → `debit` (for payments)
- Join condition: Uses `invoice_no` matching

**Result Columns:**
```
supplier_id, transaction_date, transaction_type, reference_no, invoice_no,
description, is_credit (1=bill, 0=payment), amount, due_date, status,
branch_id, entry_id
```

### 3. vw_cashbook_entries
**Location:** `Database\StoredProcedures\vw_cashbook_entries.sql`

**Changes:**
- Customer payments: Uses `pos_customers_payments.credit` (receipts)
- Supplier payments: Uses `pos_suppliers_payments.debit` (payments)
- Field: `payment_date` → `entry_date` (for both)
- Removed soft-delete checks (based on corrected schema)

**Result Columns:**
```
transaction_date, transaction_type, reference_no, description,
is_receipt (1=receipt, 0=payment), amount, cash_account_id_actual,
cash_account_name, branch_id, entry_id
```

---

## Updated Stored Procedures

All three stored procedures remain functionally the same but use the corrected view names and field names:

### 1. sp_CustomerSubLedger
**File:** `Database\StoredProcedures\sp_CustomerSubLedger.sql`
- Uses: `vw_customer_subledger_entries`
- Returns: Ledger entries + Aging summary

### 2. sp_SupplierSubLedger
**File:** `Database\StoredProcedures\sp_SupplierSubLedger.sql`
- Uses: `vw_supplier_subledger_entries`
- Returns: Ledger entries + Aging summary

### 3. sp_CashBook
**File:** `Database\StoredProcedures\sp_CashBook.sql`
- Uses: `vw_cashbook_entries`
- Returns: Ledger entries + Daily totals + Summary statistics

---

## Key Field Mapping Details

### Payment Terms Structure (IMPORTANT - CORRECTED)

The `payment_terms_id` field in `pos_sales` and `pos_purchase` **IS a foreign key** to `pos_payment_terms.id`.  
The payment terms table's `code` field stores the **number of days as a string** (e.g., "30", "60", "90").

**Database Relationship:**
```
pos_sales.payment_terms_id → pos_payment_terms.id
pos_payment_terms.code → Contains days value (stored as VARCHAR)
```

**Formula for Due Date Calculation:**
```sql
DATEADD(DAY, 
	ISNULL(CASE 
		WHEN ISNUMERIC(pt.code) = 1 
		THEN CAST(pt.code AS INT) 
		ELSE 30  -- Default to 30 days if null/invalid
	END, 30),
	transaction_date
) AS due_date
```

**Join Syntax:**
```sql
LEFT JOIN pos_payment_terms pt ON pos_sales.payment_terms_id = pt.id
DATEADD(DAY, CASE WHEN ISNUMERIC(pt.code) = 1 THEN CAST(pt.code AS INT) ELSE 30 END, sale_date)
```

**Examples:**
- If `pos_payment_terms.code = "30"`: Due date is 30 days after transaction
- If `pos_payment_terms.code = "60"`: Due date is 60 days after transaction
- If `payment_terms_id` is NULL or invalid: Defaults to 30 days

### Payment Amount Fields

**Customer Payments:**
- Source: `pos_customers_payments.credit` (this is the payment received)
- Used in: `vw_customer_subledger_entries` with `is_debit = 0`
- Meaning: Reduces outstanding amount

**Supplier Payments:**
- Source: `pos_suppliers_payments.debit` (this is the payment made)
- Used in: `vw_supplier_subledger_entries` with `is_credit = 0`
- Meaning: Reduces payable amount

**Cash Book:**
- Customer Payment Credit → Receipt (+)
- Supplier Payment Debit → Payment (-)

### Transaction Date Fields

| Source Table | Field Name |
|--------------|-----------|
| pos_sales | sale_date |
| pos_purchase | purchase_date |
| pos_customers_payments | entry_date |
| pos_suppliers_payments | entry_date |
| pos_bank_deposits | deposit_date |
| pos_bank_withdrawals | withdrawal_date |

### Join Conditions

**Customer Sub-Ledger:**
```sql
-- Invoice to Payment join (by invoice number)
LEFT JOIN (
	SELECT invoice_no, SUM(credit) AS paid_amount
	FROM pos_customers_payments
	GROUP BY invoice_no
) paid ON sm.invoice_no = paid.invoice_no
```

**Supplier Sub-Ledger:**
```sql
-- Bill to Payment join (by invoice number)
LEFT JOIN (
	SELECT invoice_no, SUM(debit) AS paid_amount
	FROM pos_suppliers_payments
	GROUP BY invoice_no
) paid ON pm.invoice_no = paid.invoice_no
```

---

## Testing Recommendations

After deployment, test these queries to verify data is flowing correctly:

```sql
-- Test Customer Subledger View
SELECT TOP 20 * FROM vw_customer_subledger_entries
WHERE customer_id = 1
ORDER BY transaction_date;

-- Test Supplier Subledger View
SELECT TOP 20 * FROM vw_supplier_subledger_entries
WHERE supplier_id = 1
ORDER BY transaction_date;

-- Test Cash Book View
SELECT TOP 20 * FROM vw_cashbook_entries
ORDER BY transaction_date;

-- Test Customer Subledger Procedure
EXEC sp_CustomerSubLedger @CustomerId=1, @FromDate='2024-01-01', @ToDate='2024-12-31';

-- Test Supplier Subledger Procedure
EXEC sp_SupplierSubLedger @SupplierId=1, @FromDate='2024-01-01', @ToDate='2024-12-31';

-- Test Cash Book Procedure
EXEC sp_CashBook @FromDate='2024-01-01', @ToDate='2024-12-31';
```

---

## Deployment Steps

1. **Drop existing objects** (if they exist):
   ```sql
   DROP PROCEDURE IF EXISTS sp_CashBook;
   DROP PROCEDURE IF EXISTS sp_SupplierSubLedger;
   DROP PROCEDURE IF EXISTS sp_CustomerSubLedger;
   DROP VIEW IF EXISTS vw_cashbook_entries;
   DROP VIEW IF EXISTS vw_supplier_subledger_entries;
   DROP VIEW IF EXISTS vw_customer_subledger_entries;
   ```

2. **Create views** (order matters - views first):
   ```sql
   -- Execute: vw_customer_subledger_entries.sql
   -- Execute: vw_supplier_subledger_entries.sql
   -- Execute: vw_cashbook_entries.sql
   ```

3. **Create procedures** (after views):
   ```sql
   -- Execute: sp_CustomerSubLedger.sql
   -- Execute: sp_SupplierSubLedger.sql
   -- Execute: sp_CashBook.sql
   ```

4. **Verify installation**:
   ```sql
   SELECT * FROM INFORMATION_SCHEMA.VIEWS 
   WHERE TABLE_NAME LIKE 'vw_%';

   SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
   WHERE ROUTINE_NAME LIKE 'sp_%';
   ```

---

## Summary Table

| Item | File | Status |
|------|------|--------|
| Customer View | vw_customer_subledger_entries.sql | ✅ Updated |
| Supplier View | vw_supplier_subledger_entries.sql | ✅ Updated |
| Cash Book View | vw_cashbook_entries.sql | ✅ Updated |
| Customer Procedure | sp_CustomerSubLedger.sql | ✅ Updated |
| Supplier Procedure | sp_SupplierSubLedger.sql | ✅ Updated |
| Cash Book Procedure | sp_CashBook.sql | ✅ Updated |

---

**All database objects are now updated with correct table and field names.**
Ready for deployment to your SQL Server database.
