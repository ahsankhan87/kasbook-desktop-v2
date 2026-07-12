# Cost Center Module — SQL Schema Reference

## Tables Created

### 1. acc_cost_centers
**Primary master table for all cost centers (departments, profit centers, service units)**

```sql
CREATE TABLE dbo.acc_cost_centers
(
	cc_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_acc_cost_centers PRIMARY KEY,
	cc_code VARCHAR(20) NOT NULL,                          -- Unique code (e.g., "DEPT-SALES")
	cc_name NVARCHAR(100) NOT NULL,                        -- Display name
	cc_type VARCHAR(20) NOT NULL,                          -- Type: "Department", "Profit Center", "Service Unit"
	parent_cc_id INT NULL,                                 -- Hierarchical parent (self-referential FK)
	manager_id INT NULL,                                   -- Manager user ID (FK to users)
	monthly_budget DECIMAL(18,2) NULL,                     -- Deprecated (use per-account budgets)
	start_date DATE NOT NULL,                              -- Effective date
	end_date DATE NULL,                                    -- Discontinuation date (NULL = active)
	is_active BIT NOT NULL CONSTRAINT DF_acc_cost_centers_is_active DEFAULT(1),
	description NVARCHAR(300) NULL,
	created_at DATETIME NOT NULL CONSTRAINT DF_acc_cost_centers_created_at DEFAULT(GETDATE())
);

-- Unique Index on cc_code
CREATE UNIQUE INDEX UX_acc_cost_centers_cc_code ON dbo.acc_cost_centers(cc_code);

-- Self-referential FK for hierarchy
ALTER TABLE dbo.acc_cost_centers
ADD CONSTRAINT FK_acc_cost_centers_parent
	FOREIGN KEY (parent_cc_id) REFERENCES dbo.acc_cost_centers(cc_id);

-- FK to users table (if exists)
ALTER TABLE dbo.acc_cost_centers
ADD CONSTRAINT FK_acc_cost_centers_manager
	FOREIGN KEY (manager_id) REFERENCES dbo.users(id);
```

---

### 2. acc_cost_center_budgets
**Monthly budgets for GL accounts within cost centers (per fiscal year)**

```sql
CREATE TABLE dbo.acc_cost_center_budgets
(
	budget_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_acc_cost_center_budgets PRIMARY KEY,
	cc_id INT NOT NULL,                                    -- Cost center ID
	financial_year_id INT NOT NULL,                        -- Fiscal year ID
	account_id INT NOT NULL,                               -- GL account ID (Income or Expense)
	-- Monthly budget amounts (12 columns)
	jan_budget DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_cc_budget_jan DEFAULT(0),
	feb_budget DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_cc_budget_feb DEFAULT(0),
	mar_budget DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_cc_budget_mar DEFAULT(0),
	apr_budget DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_cc_budget_apr DEFAULT(0),
	may_budget DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_cc_budget_may DEFAULT(0),
	jun_budget DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_cc_budget_jun DEFAULT(0),
	jul_budget DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_cc_budget_jul DEFAULT(0),
	aug_budget DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_cc_budget_aug DEFAULT(0),
	sep_budget DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_cc_budget_sep DEFAULT(0),
	oct_budget DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_cc_budget_oct DEFAULT(0),
	nov_budget DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_cc_budget_nov DEFAULT(0),
	dec_budget DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_cc_budget_dec DEFAULT(0),
	created_by INT NULL,                                   -- User who created
	created_at DATETIME NOT NULL CONSTRAINT DF_acc_cc_budget_created_at DEFAULT(GETDATE())
);

-- Unique index: one budget per account per cost center per year
CREATE UNIQUE INDEX UX_acc_cost_center_budgets_cc_year_account
ON dbo.acc_cost_center_budgets(cc_id, financial_year_id, account_id);

-- Foreign keys
ALTER TABLE dbo.acc_cost_center_budgets
ADD CONSTRAINT FK_acc_cost_center_budgets_cc
	FOREIGN KEY (cc_id) REFERENCES dbo.acc_cost_centers(cc_id);

ALTER TABLE dbo.acc_cost_center_budgets
ADD CONSTRAINT FK_acc_cost_center_budgets_year
	FOREIGN KEY (financial_year_id) REFERENCES dbo.acc_fiscal_years(id);

ALTER TABLE dbo.acc_cost_center_budgets
ADD CONSTRAINT FK_acc_cost_center_budgets_account
	FOREIGN KEY (account_id) REFERENCES dbo.acc_accounts(id);

ALTER TABLE dbo.acc_cost_center_budgets
ADD CONSTRAINT FK_acc_cost_center_budgets_created_by
	FOREIGN KEY (created_by) REFERENCES dbo.users(id);
```

---

### 3. acc_cost_center_allocations
**Rules for automatic expense allocation across cost centers**

```sql
CREATE TABLE dbo.acc_cost_center_allocations
(
	alloc_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_acc_cost_center_allocations PRIMARY KEY,
	alloc_name NVARCHAR(120) NOT NULL,                     -- Rule name (e.g., "Rent Allocation to Departments")
	source_acc_id INT NOT NULL,                            -- Expense account to allocate (e.g., Rent)
	cc_id INT NOT NULL,                                    -- Target cost center
	allocation_percent DECIMAL(5,2) NOT NULL,             -- Percentage for FIXED_PCT method (0-100)
	allocation_method VARCHAR(20) NOT NULL,               -- FIXED_PCT, HEADCOUNT, REVENUE
	is_active BIT NOT NULL CONSTRAINT DF_acc_cost_center_allocations_is_active DEFAULT(1),
	created_at DATETIME NOT NULL CONSTRAINT DF_acc_cost_center_allocations_created_at DEFAULT(GETDATE()),
	-- Constraints
	CONSTRAINT CK_acc_cost_center_allocations_method CHECK (allocation_method IN ('FIXED_PCT', 'HEADCOUNT', 'REVENUE')),
	CONSTRAINT CK_acc_cost_center_allocations_pct CHECK (allocation_percent >= 0 AND allocation_percent <= 100)
);

-- Foreign keys
ALTER TABLE dbo.acc_cost_center_allocations
ADD CONSTRAINT FK_acc_cost_center_allocations_source_acc
	FOREIGN KEY (source_acc_id) REFERENCES dbo.acc_accounts(id);

ALTER TABLE dbo.acc_cost_center_allocations
ADD CONSTRAINT FK_acc_cost_center_allocations_cc
	FOREIGN KEY (cc_id) REFERENCES dbo.acc_cost_centers(cc_id);
```

---

### 4. acc_entries (Modified)
**Existing table – NEW COLUMN ADDED**

```sql
-- Add cost center tracking to existing entries
ALTER TABLE dbo.acc_entries
ADD cost_center_id INT NULL;

-- Create index for fast lookups by cost center
CREATE NONCLUSTERED INDEX IX_acc_entries_cost_center_id
ON dbo.acc_entries(cost_center_id)
INCLUDE (entry_date, account_id, debit, credit, branch_id);

-- Add foreign key
ALTER TABLE dbo.acc_entries
ADD CONSTRAINT FK_acc_entries_cost_center
	FOREIGN KEY (cost_center_id) REFERENCES dbo.acc_cost_centers(cc_id);
```

---

## Types Created

### CostCenterIdListType
**Table-Valued Parameter for bulk operations (used in sp_DepartmentalPL)**

```sql
CREATE TYPE dbo.CostCenterIdListType AS TABLE
(
	cc_id INT NOT NULL PRIMARY KEY
);
```

**Usage Example:**
```csharp
DataTable ccIds = new DataTable();
ccIds.Columns.Add("cc_id", typeof(int));
ccIds.Rows.Add(1);
ccIds.Rows.Add(2);
ccIds.Rows.Add(3);

var tvpParam = cmd.Parameters.AddWithValue("@CCIds", ccIds);
tvpParam.SqlDbType = SqlDbType.Structured;
tvpParam.TypeName = "dbo.CostCenterIdListType";
```

---

## Stored Procedures

### 1. sp_GetCostCenterTree
**Returns hierarchical cost centers with optional rollup of income/expense balances**

```sql
EXEC sp_GetCostCenterTree 
	@IncludeBalances = 1,           -- Include income/expense rollups
	@FromDate = '2025-01-01',       -- Period start (NULL = all time)
	@ToDate = '2025-01-31';         -- Period end (NULL = all time)
```

**Output Columns:**
- `cc_id, cc_code, cc_name, cc_type, parent_cc_id`
- `level_no` – Depth in hierarchy (0 = root)
- `path_code` – Hierarchical path (e.g., "CC-001 > CC-002 > CC-003")
- `total_income, total_expense, net_profit` – Rolled-up balances (0 if @IncludeBalances = 0)

---

### 2. sp_DepartmentalPL
**Departmental Profit & Loss pivot report**

```sql
DECLARE @CCIds AS dbo.CostCenterIdListType;
INSERT INTO @CCIds VALUES (1), (2), (3);

EXEC sp_DepartmentalPL 
	@FromDate = '2025-01-01',
	@ToDate = '2025-01-31',
	@CCIds = @CCIds;             -- NULL = all cost centers
```

**Output:**
- Rows: GL accounts (Income/Expense)
- Columns: Each cost center + "Unallocated"
- Values: Amount for each account/cost center combination
- Summary row with totals

---

### 3. sp_CostCenterBudgetVsActual
**Monthly budget vs. actual for a cost center and fiscal year**

```sql
EXEC sp_CostCenterBudgetVsActual 
	@CCId = 1,                  -- Cost center ID
	@FinancialYearId = 2025;    -- Fiscal year ID
```

**Output Columns:**
- `account_id, account_code, account_name`
- `month_no, MonthName`
- `BudgetAmount, ActualAmount, Variance, VariancePct`
- `IsSubtotal` – Flag for monthly subtotal rows

---

### 4. sp_AutoAllocateExpenses
**Automatic expense allocation engine**

```sql
EXEC sp_AutoAllocateExpenses 
	@Period = '2025-01-15',         -- Any date in the period (1st of month used)
	@AllocationRuleId = NULL,        -- NULL = all active rules
	@UserId = 5;                     -- User ID for audit
```

**Logic:**
1. Sum unallocated entries (NULL `cost_center_id`) per source account
2. For each allocation rule, calculate department share:
   - **FIXED_PCT:** Multiply by configured %
   - **HEADCOUNT:** Divide by headcount ratio
   - **REVENUE:** Divide by revenue proportion
3. Post balanced voucher with allocation entries
4. Validate: sum of allocations = total (residual method for rounding)

**Output Result Set 1 (Summary):**
- `PeriodStart, PeriodEndExclusive, VoucherNo, EntryHeaderId, TotalAllocated`
- `Message` – Success message or error reason

**Output Result Set 2 (Details):**
- `alloc_id, alloc_name, source_acc_id, cc_id`
- `allocation_method, allocation_percent`
- `source_amount, allocated_amount, voucher_no`

---

### 5. sp_CostCenterSummary
**Summary of all cost centers with income, expense, net profit, and budget variance**

```sql
EXEC sp_CostCenterSummary 
	@FromDate = '2025-01-01',
	@ToDate = '2025-12-31';
```

**Output Columns:**
- `CCCode, CCName, [Type]`
- `TotalIncome, TotalExpense, NetProfit`
- `BudgetVariance` – Budget balance vs. actual expense

---

## Query Examples

### Get All Active Cost Centers (Flat)
```sql
SELECT cc_id, cc_code, cc_name, cc_type, is_active
FROM dbo.acc_cost_centers
WHERE is_active = 1
ORDER BY cc_code;
```

### Get Cost Center Hierarchy
```sql
EXEC sp_GetCostCenterTree @IncludeBalances = 0;
```

### Get Over-Budget Accounts (Current Month)
```sql
DECLARE @cc_id INT = 1;
DECLARE @month INT = MONTH(GETDATE());
DECLARE @year INT = YEAR(GETDATE());
DECLARE @month_start DATE = DATEFROMPARTS(@year, @month, 1);
DECLARE @month_end DATE = EOMONTH(@month_start);

SELECT
	b.account_id,
	a.code,
	a.name,
	CASE @month
		WHEN 1 THEN b.jan_budget
		WHEN 2 THEN b.feb_budget
		-- ... (12 cases)
		WHEN 12 THEN b.dec_budget
	END AS budget_amount,
	ISNULL(SUM(E.debit - E.credit), 0) AS actual_amount
FROM dbo.acc_cost_center_budgets b
INNER JOIN dbo.acc_accounts a ON a.id = b.account_id
LEFT JOIN dbo.acc_entries E 
	ON E.account_id = b.account_id 
	AND E.cost_center_id = @cc_id
	AND E.entry_date >= @month_start 
	AND E.entry_date <= @month_end
WHERE b.cc_id = @cc_id
GROUP BY b.account_id, a.code, a.name, b.jan_budget, b.feb_budget, ...
HAVING ISNULL(SUM(E.debit - E.credit), 0) > 
	CASE @month
		WHEN 1 THEN b.jan_budget
		-- ...
	END;
```

### Departmental P&L Pivot
```sql
DECLARE @CCIds AS dbo.CostCenterIdListType;
INSERT INTO @CCIds VALUES (1), (2), (3);

EXEC sp_DepartmentalPL 
	@FromDate = '2025-01-01',
	@ToDate = '2025-12-31',
	@CCIds = @CCIds;
```

---

## Performance Notes

1. **Recursive CTE Limits:** `sp_GetCostCenterTree` uses MAXRECURSION 32767 to handle deep hierarchies.
2. **Dynamic SQL:** `sp_DepartmentalPL` builds PIVOT dynamically; best performance with <50 cost centers.
3. **Index Strategy:**
   - `acc_entries(cost_center_id)` with INCLUDE of common columns for fast dept rolls
   - `acc_cost_center_budgets(cc_id, financial_year_id, account_id)` unique index prevents duplicates
4. **Allocation Performance:** `sp_AutoAllocateExpenses` may take 1-5 minutes for large datasets; set `CommandTimeout = 300` in C#.

---

## Data Integrity Checks

**Before running sp_AutoAllocateExpenses:**
```sql
-- Verify allocation rules exist and are active
SELECT COUNT(*) as active_rules
FROM dbo.acc_cost_center_allocations
WHERE is_active = 1;

-- Check for unallocated entries
SELECT COUNT(*) as unallocated_entries
FROM dbo.acc_entries
WHERE cost_center_id IS NULL
  AND entry_date >= '2025-01-01'
  AND entry_date < '2025-02-01';
```

**After allocation to verify balance:**
```sql
-- Debit and credit should match
SELECT 
	SUM(debit) as total_debit,
	SUM(credit) as total_credit
FROM dbo.acc_entries_header
WHERE InvoiceNo LIKE 'CCA-%'
  AND status = 'Posted';
```

---

**All procedures are safe for production with transaction support and error handling.**
