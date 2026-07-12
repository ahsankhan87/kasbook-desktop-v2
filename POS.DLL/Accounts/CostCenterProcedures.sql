IF OBJECT_ID(N'dbo.acc_cost_centers', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.acc_cost_centers
	(
		cc_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_acc_cost_centers PRIMARY KEY,
		cc_code VARCHAR(20) NOT NULL,
		cc_name NVARCHAR(100) NOT NULL,
		cc_type VARCHAR(20) NOT NULL,
		parent_cc_id INT NULL,
		manager_id INT NULL,
		monthly_budget DECIMAL(18,2) NULL,
		start_date DATE NOT NULL,
		end_date DATE NULL,
		is_active BIT NOT NULL CONSTRAINT DF_acc_cost_centers_is_active DEFAULT(1),
		description NVARCHAR(300) NULL,
		created_at DATETIME NOT NULL CONSTRAINT DF_acc_cost_centers_created_at DEFAULT(GETDATE())
	);
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM sys.indexes
	WHERE object_id = OBJECT_ID(N'dbo.acc_cost_centers')
	  AND name = N'UX_acc_cost_centers_cc_code'
)
BEGIN
	CREATE UNIQUE INDEX UX_acc_cost_centers_cc_code
	ON dbo.acc_cost_centers(cc_code);
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM sys.foreign_keys
	WHERE name = N'FK_acc_cost_centers_parent'
)
BEGIN
	ALTER TABLE dbo.acc_cost_centers
	ADD CONSTRAINT FK_acc_cost_centers_parent
		FOREIGN KEY (parent_cc_id) REFERENCES dbo.acc_cost_centers(cc_id);
END
GO

IF OBJECT_ID(N'dbo.users', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_cost_centers_manager'
   )
BEGIN
	ALTER TABLE dbo.acc_cost_centers
	ADD CONSTRAINT FK_acc_cost_centers_manager
		FOREIGN KEY (manager_id) REFERENCES dbo.users(id);
END
GO

IF OBJECT_ID(N'dbo.acc_cost_center_budgets', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.acc_cost_center_budgets
	(
		budget_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_acc_cost_center_budgets PRIMARY KEY,
		cc_id INT NOT NULL,
		financial_year_id INT NOT NULL,
		account_id INT NOT NULL,
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
		created_by INT NULL,
		created_at DATETIME NOT NULL CONSTRAINT DF_acc_cc_budget_created_at DEFAULT(GETDATE())
	);
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM sys.indexes
	WHERE object_id = OBJECT_ID(N'dbo.acc_cost_center_budgets')
	  AND name = N'UX_acc_cost_center_budgets_cc_year_account'
)
BEGIN
	CREATE UNIQUE INDEX UX_acc_cost_center_budgets_cc_year_account
	ON dbo.acc_cost_center_budgets(cc_id, financial_year_id, account_id);
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM sys.foreign_keys
	WHERE name = N'FK_acc_cost_center_budgets_cc'
)
BEGIN
	ALTER TABLE dbo.acc_cost_center_budgets
	ADD CONSTRAINT FK_acc_cost_center_budgets_cc
		FOREIGN KEY (cc_id) REFERENCES dbo.acc_cost_centers(cc_id);
END
GO

IF OBJECT_ID(N'dbo.acc_fiscal_years', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_cost_center_budgets_year'
   )
BEGIN
	ALTER TABLE dbo.acc_cost_center_budgets
	ADD CONSTRAINT FK_acc_cost_center_budgets_year
		FOREIGN KEY (financial_year_id) REFERENCES dbo.acc_fiscal_years(id);
END
GO

IF OBJECT_ID(N'dbo.acc_accounts', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_cost_center_budgets_account'
   )
BEGIN
	ALTER TABLE dbo.acc_cost_center_budgets
	ADD CONSTRAINT FK_acc_cost_center_budgets_account
		FOREIGN KEY (account_id) REFERENCES dbo.acc_accounts(id);
END
GO

IF OBJECT_ID(N'dbo.users', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_cost_center_budgets_created_by'
   )
BEGIN
	ALTER TABLE dbo.acc_cost_center_budgets
	ADD CONSTRAINT FK_acc_cost_center_budgets_created_by
		FOREIGN KEY (created_by) REFERENCES dbo.users(id);
END
GO

IF OBJECT_ID(N'dbo.acc_cost_center_allocations', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.acc_cost_center_allocations
	(
		alloc_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_acc_cost_center_allocations PRIMARY KEY,
		alloc_name NVARCHAR(120) NOT NULL,
		source_acc_id INT NOT NULL,
		cc_id INT NOT NULL,
		allocation_percent DECIMAL(5,2) NOT NULL,
		allocation_method VARCHAR(20) NOT NULL,
		is_active BIT NOT NULL CONSTRAINT DF_acc_cost_center_allocations_is_active DEFAULT(1),
		created_at DATETIME NOT NULL CONSTRAINT DF_acc_cost_center_allocations_created_at DEFAULT(GETDATE()),
		CONSTRAINT CK_acc_cost_center_allocations_method CHECK (allocation_method IN ('FIXED_PCT', 'HEADCOUNT', 'REVENUE')),
		CONSTRAINT CK_acc_cost_center_allocations_pct CHECK (allocation_percent >= 0 AND allocation_percent <= 100)
	);
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM sys.foreign_keys
	WHERE name = N'FK_acc_cost_center_allocations_source_acc'
)
BEGIN
	ALTER TABLE dbo.acc_cost_center_allocations
	ADD CONSTRAINT FK_acc_cost_center_allocations_source_acc
		FOREIGN KEY (source_acc_id) REFERENCES dbo.acc_accounts(id);
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM sys.foreign_keys
	WHERE name = N'FK_acc_cost_center_allocations_cc'
)
BEGIN
	ALTER TABLE dbo.acc_cost_center_allocations
	ADD CONSTRAINT FK_acc_cost_center_allocations_cc
		FOREIGN KEY (cc_id) REFERENCES dbo.acc_cost_centers(cc_id);
END
GO

IF COL_LENGTH('dbo.acc_entries', 'cost_center_id') IS NULL
BEGIN
	ALTER TABLE dbo.acc_entries
	ADD cost_center_id INT NULL;
END
GO

IF OBJECT_ID(N'dbo.acc_cost_centers', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_entries_cost_center'
   )
BEGIN
	ALTER TABLE dbo.acc_entries
	ADD CONSTRAINT FK_acc_entries_cost_center
		FOREIGN KEY (cost_center_id) REFERENCES dbo.acc_cost_centers(cc_id);
END
GO

IF NOT EXISTS
(
	SELECT 1
	FROM sys.indexes
	WHERE object_id = OBJECT_ID(N'dbo.acc_entries')
	  AND name = N'IX_acc_entries_cost_center_id'
)
BEGIN
	CREATE NONCLUSTERED INDEX IX_acc_entries_cost_center_id
	ON dbo.acc_entries(cost_center_id)
	INCLUDE (entry_date, account_id, debit, credit, branch_id);
END
GO

IF TYPE_ID(N'dbo.CostCenterIdListType') IS NULL
BEGIN
	CREATE TYPE dbo.CostCenterIdListType AS TABLE
	(
		cc_id INT NOT NULL PRIMARY KEY
	);
END
GO

IF OBJECT_ID(N'dbo.sp_GetCostCenterTree', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_GetCostCenterTree;
GO

CREATE PROCEDURE dbo.sp_GetCostCenterTree
	@IncludeBalances BIT = 1,
	@FromDate DATE = NULL,
	@ToDate DATE = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ToExclusive DATE = CASE WHEN @ToDate IS NULL THEN NULL ELSE DATEADD(DAY, 1, @ToDate) END;

	;WITH Tree AS
	(
		SELECT
			c.cc_id,
			c.cc_code,
			c.cc_name,
			c.cc_type,
			c.parent_cc_id,
			CAST(c.cc_code AS NVARCHAR(400)) AS path_code,
			0 AS level_no
		FROM dbo.acc_cost_centers c
		WHERE c.parent_cc_id IS NULL

		UNION ALL

		SELECT
			c.cc_id,
			c.cc_code,
			c.cc_name,
			c.cc_type,
			c.parent_cc_id,
			CAST(t.path_code + N' > ' + c.cc_code AS NVARCHAR(400)) AS path_code,
			t.level_no + 1 AS level_no
		FROM dbo.acc_cost_centers c
		INNER JOIN Tree t ON t.cc_id = c.parent_cc_id
	),
	Descendants AS
	(
		SELECT c.cc_id AS ancestor_cc_id, c.cc_id AS descendant_cc_id
		FROM dbo.acc_cost_centers c

		UNION ALL

		SELECT d.ancestor_cc_id, c.cc_id AS descendant_cc_id
		FROM Descendants d
		INNER JOIN dbo.acc_cost_centers c ON c.parent_cc_id = d.descendant_cc_id
	),
	AccountClassify AS
	(
		SELECT
			A.id AS account_id,
			CASE
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%income%'
				  OR LOWER(ISNULL(T.name, '')) LIKE '%revenue%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%income%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%revenue%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%income%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%revenue%'
					THEN 'Income'
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(T.name, '')) LIKE '%cogs%'
				  OR LOWER(ISNULL(T.name, '')) LIKE '%cost%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%cogs%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%cost%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%cogs%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%cost%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%purchase%'
					THEN 'Expense'
				ELSE 'Other'
			END AS account_class
		FROM dbo.acc_accounts A
		LEFT JOIN dbo.acc_groups G ON G.id = A.group_id
		LEFT JOIN dbo.acc_account_type T ON T.id = G.account_type_id
	),
	DirectBalances AS
	(
		SELECT
			E.cost_center_id AS cc_id,
			SUM(CASE WHEN C.account_class = 'Income' THEN ISNULL(E.credit, 0) - ISNULL(E.debit, 0) ELSE 0 END) AS income_amount,
			SUM(CASE WHEN C.account_class = 'Expense' THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0) ELSE 0 END) AS expense_amount
		FROM dbo.acc_entries E
		INNER JOIN AccountClassify C ON C.account_id = E.account_id
		WHERE E.cost_center_id IS NOT NULL
		  AND (@FromDate IS NULL OR E.entry_date >= @FromDate)
		  AND (@ToExclusive IS NULL OR E.entry_date < @ToExclusive)
		GROUP BY E.cost_center_id
	),
	RolledBalances AS
	(
		SELECT
			d.ancestor_cc_id AS cc_id,
			SUM(ISNULL(b.income_amount, 0)) AS total_income,
			SUM(ISNULL(b.expense_amount, 0)) AS total_expense
		FROM Descendants d
		LEFT JOIN DirectBalances b ON b.cc_id = d.descendant_cc_id
		GROUP BY d.ancestor_cc_id
	)
	SELECT
		t.cc_id,
		t.cc_code,
		t.cc_name,
		t.cc_type,
		t.parent_cc_id,
		t.level_no,
		t.path_code,
		CAST(CASE WHEN @IncludeBalances = 1 THEN ISNULL(r.total_income, 0) ELSE 0 END AS DECIMAL(18,2)) AS total_income,
		CAST(CASE WHEN @IncludeBalances = 1 THEN ISNULL(r.total_expense, 0) ELSE 0 END AS DECIMAL(18,2)) AS total_expense,
		CAST(CASE WHEN @IncludeBalances = 1 THEN ISNULL(r.total_income, 0) - ISNULL(r.total_expense, 0) ELSE 0 END AS DECIMAL(18,2)) AS net_profit
	FROM Tree t
	LEFT JOIN RolledBalances r ON r.cc_id = t.cc_id
	ORDER BY t.path_code
	OPTION (MAXRECURSION 32767);
END
GO

IF OBJECT_ID(N'dbo.sp_DepartmentalPL', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_DepartmentalPL;
GO

CREATE PROCEDURE dbo.sp_DepartmentalPL
	@FromDate DATE,
	@ToDate DATE,
	@CCIds dbo.CostCenterIdListType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ToExclusive DATE = DATEADD(DAY, 1, @ToDate);

	DECLARE @SelectedCC TABLE
	(
		cc_id INT NOT NULL PRIMARY KEY,
		cc_code VARCHAR(20) NOT NULL
	);

	IF EXISTS (SELECT 1 FROM @CCIds)
	BEGIN
		INSERT INTO @SelectedCC(cc_id, cc_code)
		SELECT c.cc_id, c.cc_code
		FROM dbo.acc_cost_centers c
		INNER JOIN @CCIds i ON i.cc_id = c.cc_id;
	END
	ELSE
	BEGIN
		INSERT INTO @SelectedCC(cc_id, cc_code)
		SELECT c.cc_id, c.cc_code
		FROM dbo.acc_cost_centers c;
	END

	DECLARE @Columns NVARCHAR(MAX) = N'';
	DECLARE @ColumnsWithAlias NVARCHAR(MAX) = N'';
	DECLARE @GrandExpr NVARCHAR(MAX) = N'';

	SELECT @Columns = STUFF(
		(
			SELECT N',' + QUOTENAME(cc_code)
			FROM @SelectedCC
			ORDER BY cc_code
			FOR XML PATH(''), TYPE
		).value('.', 'NVARCHAR(MAX)'),
		1,
		1,
		N''
	);

	SELECT @ColumnsWithAlias = STUFF(
		(
			SELECT N',ISNULL(p.' + QUOTENAME(cc_code) + N',0) AS ' + QUOTENAME(cc_code)
			FROM @SelectedCC
			ORDER BY cc_code
			FOR XML PATH(''), TYPE
		).value('.', 'NVARCHAR(MAX)'),
		1,
		1,
		N''
	);

	SELECT @GrandExpr = STUFF(
		(
			SELECT N' + ISNULL(p.' + QUOTENAME(cc_code) + N',0)'
			FROM @SelectedCC
			ORDER BY cc_code
			FOR XML PATH(''), TYPE
		).value('.', 'NVARCHAR(MAX)'),
		1,
		3,
		N''
	);

	IF ISNULL(@GrandExpr, N'') = N''
		SET @GrandExpr = N'0';

	DECLARE @Sql NVARCHAR(MAX) = N'
;WITH AccountClassify AS
(
	SELECT
		A.id AS account_id,
		ISNULL(A.code, '''') AS account_code,
		ISNULL(A.name, '''') AS account_name,
		CASE
			WHEN LOWER(ISNULL(T.name, '''')) LIKE ''%income%''
			  OR LOWER(ISNULL(T.name, '''')) LIKE ''%revenue%''
			  OR LOWER(ISNULL(G.name, '''')) LIKE ''%income%''
			  OR LOWER(ISNULL(G.name, '''')) LIKE ''%revenue%''
			  OR LOWER(ISNULL(A.name, '''')) LIKE ''%income%''
			  OR LOWER(ISNULL(A.name, '''')) LIKE ''%revenue%''
				THEN ''Income''
			WHEN LOWER(ISNULL(T.name, '''')) LIKE ''%expense%''
			  OR LOWER(ISNULL(T.name, '''')) LIKE ''%cogs%''
			  OR LOWER(ISNULL(T.name, '''')) LIKE ''%cost%''
			  OR LOWER(ISNULL(G.name, '''')) LIKE ''%expense%''
			  OR LOWER(ISNULL(G.name, '''')) LIKE ''%cogs%''
			  OR LOWER(ISNULL(G.name, '''')) LIKE ''%cost%''
			  OR LOWER(ISNULL(A.name, '''')) LIKE ''%expense%''
			  OR LOWER(ISNULL(A.name, '''')) LIKE ''%cogs%''
			  OR LOWER(ISNULL(A.name, '''')) LIKE ''%cost%''
			  OR LOWER(ISNULL(A.name, '''')) LIKE ''%purchase%''
				THEN ''Expense''
			ELSE ''Other''
		END AS section
	FROM dbo.acc_accounts A
	LEFT JOIN dbo.acc_groups G ON G.id = A.group_id
	LEFT JOIN dbo.acc_account_type T ON T.id = G.account_type_id
),
BaseBuckets AS
(
	SELECT
		E.account_id,
		CASE
			WHEN E.cost_center_id IS NULL THEN ''Unallocated''
			ELSE S.cc_code
		END AS bucket_name,
		SUM(CASE
				WHEN C.section = ''Income'' THEN ISNULL(E.credit, 0) - ISNULL(E.debit, 0)
				WHEN C.section = ''Expense'' THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0)
				ELSE 0
			END) AS amount
	FROM dbo.acc_entries E
	INNER JOIN AccountClassify C ON C.account_id = E.account_id
	LEFT JOIN #SelectedCC S ON S.cc_id = E.cost_center_id
	WHERE E.entry_date >= @FromDate
	  AND E.entry_date < @ToExclusive
	  AND (
			E.cost_center_id IS NULL
			OR EXISTS (SELECT 1 FROM #SelectedCC SC WHERE SC.cc_id = E.cost_center_id)
		  )
	  AND C.section IN (''Income'', ''Expense'')
	GROUP BY E.account_id,
			 CASE WHEN E.cost_center_id IS NULL THEN ''Unallocated'' ELSE S.cc_code END
),
SingleEntity AS
(
	SELECT
		E.account_id,
		SUM(CASE
				WHEN C.section = ''Income'' THEN ISNULL(E.credit, 0) - ISNULL(E.debit, 0)
				WHEN C.section = ''Expense'' THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0)
				ELSE 0
			END) AS total_amount
	FROM dbo.acc_entries E
	INNER JOIN AccountClassify C ON C.account_id = E.account_id
	WHERE E.entry_date >= @FromDate
	  AND E.entry_date < @ToExclusive
	  AND (
			E.cost_center_id IS NULL
			OR EXISTS (SELECT 1 FROM #SelectedCC SC WHERE SC.cc_id = E.cost_center_id)
		  )
	  AND C.section IN (''Income'', ''Expense'')
	GROUP BY E.account_id
),
Pivoted AS
(
	SELECT *
	FROM BaseBuckets
	PIVOT (SUM(amount) FOR bucket_name IN (' + CASE WHEN ISNULL(@Columns, N'') = N'' THEN N'' ELSE @Columns + N',' END + N'[Unallocated])) p
)
SELECT
	a.account_id,
	a.account_code,
	a.account_name,
	a.section,
	' + CASE WHEN ISNULL(@ColumnsWithAlias, N'') = N'' THEN N'' ELSE @ColumnsWithAlias + N',' END + N'
	CAST(ISNULL(p.[Unallocated], 0) AS DECIMAL(18,2)) AS [Unallocated],
	CAST((' + @GrandExpr + N') + ISNULL(p.[Unallocated], 0) AS DECIMAL(18,2)) AS GrandTotal,
	CAST(ISNULL(se.total_amount, 0) AS DECIMAL(18,2)) AS SingleEntityTotal,
	CAST(((' + @GrandExpr + N') + ISNULL(p.[Unallocated], 0)) - ISNULL(se.total_amount, 0) AS DECIMAL(18,2)) AS ValidationDelta
FROM Pivoted p
INNER JOIN AccountClassify a ON a.account_id = p.account_id
LEFT JOIN SingleEntity se ON se.account_id = p.account_id
ORDER BY
	CASE WHEN a.section = ''Income'' THEN 1 ELSE 2 END,
	a.account_code,
	a.account_name;

SELECT
	CAST(SUM(GrandTotal) AS DECIMAL(18,2)) AS GrandTotalPL,
	CAST(SUM(SingleEntityTotal) AS DECIMAL(18,2)) AS SingleEntityPL,
	CAST(SUM(GrandTotal - SingleEntityTotal) AS DECIMAL(18,2)) AS ValidationDelta
FROM
(
	SELECT
		((' + @GrandExpr + N') + ISNULL(p.[Unallocated], 0)) AS GrandTotal,
		ISNULL(se.total_amount, 0) AS SingleEntityTotal
	FROM Pivoted p
	LEFT JOIN SingleEntity se ON se.account_id = p.account_id
) v;';

	CREATE TABLE #SelectedCC
	(
		cc_id INT NOT NULL PRIMARY KEY,
		cc_code VARCHAR(20) NOT NULL
	);

	INSERT INTO #SelectedCC(cc_id, cc_code)
	SELECT cc_id, cc_code
	FROM @SelectedCC;

	EXEC sp_executesql
		@Sql,
		N'@FromDate DATE, @ToExclusive DATE',
		@FromDate = @FromDate,
		@ToExclusive = @ToExclusive;
END
GO

IF OBJECT_ID(N'dbo.sp_CostCenterBudgetVsActual', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_CostCenterBudgetVsActual;
GO

CREATE PROCEDURE dbo.sp_CostCenterBudgetVsActual
	@CCId INT,
	@FinancialYearId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @YearStart DATE;
	DECLARE @YearEnd DATE;

	SELECT
		@YearStart = CAST(from_date AS DATE),
		@YearEnd = CAST(to_date AS DATE)
	FROM dbo.acc_fiscal_years
	WHERE id = @FinancialYearId;

	IF @YearStart IS NULL OR @YearEnd IS NULL
	BEGIN
		RAISERROR(''Invalid FinancialYearId.'', 16, 1);
		RETURN;
	END

	;WITH AccountClassify AS
	(
		SELECT
			A.id AS account_id,
			ISNULL(A.code, '') AS account_code,
			ISNULL(A.name, '') AS account_name,
			CASE
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%income%'
				  OR LOWER(ISNULL(T.name, '')) LIKE '%revenue%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%income%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%revenue%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%income%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%revenue%'
					THEN 'Income'
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(T.name, '')) LIKE '%cogs%'
				  OR LOWER(ISNULL(T.name, '')) LIKE '%cost%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%cogs%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%cost%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%cogs%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%cost%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%purchase%'
					THEN 'Expense'
				ELSE 'Other'
			END AS section
		FROM dbo.acc_accounts A
		LEFT JOIN dbo.acc_groups G ON G.id = A.group_id
		LEFT JOIN dbo.acc_account_type T ON T.id = G.account_type_id
	),
	BudgetUnpivot AS
	(
		SELECT
			b.account_id,
			v.month_no,
			DATEFROMPARTS(YEAR(@YearStart), v.month_no, 1) AS month_start,
			v.month_name,
			v.budget_amount
		FROM dbo.acc_cost_center_budgets b
		CROSS APPLY
		(
			VALUES
				(1, N'January', b.jan_budget),
				(2, N'February', b.feb_budget),
				(3, N'March', b.mar_budget),
				(4, N'April', b.apr_budget),
				(5, N'May', b.may_budget),
				(6, N'June', b.jun_budget),
				(7, N'July', b.jul_budget),
				(8, N'August', b.aug_budget),
				(9, N'September', b.sep_budget),
				(10, N'October', b.oct_budget),
				(11, N'November', b.nov_budget),
				(12, N'December', b.dec_budget)
		) v(month_no, month_name, budget_amount)
		WHERE b.cc_id = @CCId
		  AND b.financial_year_id = @FinancialYearId
	),
	ActualByMonth AS
	(
		SELECT
			E.account_id,
			MONTH(E.entry_date) AS month_no,
			SUM(CASE
					WHEN C.section = 'Income' THEN ISNULL(E.credit, 0) - ISNULL(E.debit, 0)
					WHEN C.section = 'Expense' THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0)
					ELSE 0
				END) AS actual_amount
		FROM dbo.acc_entries E
		INNER JOIN AccountClassify C ON C.account_id = E.account_id
		WHERE E.cost_center_id = @CCId
		  AND E.entry_date >= @YearStart
		  AND E.entry_date < DATEADD(DAY, 1, @YearEnd)
		  AND C.section IN ('Income', 'Expense')
		GROUP BY E.account_id, MONTH(E.entry_date)
	),
	BaseRows AS
	(
		SELECT
			ISNULL(b.account_id, a.account_id) AS account_id,
			ISNULL(b.month_no, a.month_no) AS month_no,
			ISNULL(b.month_name, DATENAME(MONTH, DATEFROMPARTS(YEAR(@YearStart), a.month_no, 1))) AS month_name,
			ISNULL(b.budget_amount, 0) AS budget_amount,
			ISNULL(a.actual_amount, 0) AS actual_amount
		FROM BudgetUnpivot b
		FULL OUTER JOIN ActualByMonth a
			ON a.account_id = b.account_id
		   AND a.month_no = b.month_no
	)
	SELECT
		CAST(br.account_id AS INT) AS account_id,
		ac.account_code,
		ac.account_name,
		ac.section,
		br.month_no,
		br.month_name AS MonthName,
		CAST(br.budget_amount AS DECIMAL(18,2)) AS BudgetAmount,
		CAST(br.actual_amount AS DECIMAL(18,2)) AS ActualAmount,
		CAST(br.actual_amount - br.budget_amount AS DECIMAL(18,2)) AS Variance,
		CAST(CASE WHEN NULLIF(br.budget_amount, 0) IS NULL THEN 0 ELSE ((br.actual_amount - br.budget_amount) / NULLIF(br.budget_amount, 0)) * 100 END AS DECIMAL(18,2)) AS VariancePct,
		CAST(0 AS BIT) AS IsSubtotal
	FROM BaseRows br
	INNER JOIN AccountClassify ac ON ac.account_id = br.account_id

	UNION ALL

	SELECT
		NULL AS account_id,
		NULL AS account_code,
		N'Monthly Subtotal' AS account_name,
		N'Subtotal' AS section,
		br.month_no,
		br.month_name AS MonthName,
		CAST(SUM(br.budget_amount) AS DECIMAL(18,2)) AS BudgetAmount,
		CAST(SUM(br.actual_amount) AS DECIMAL(18,2)) AS ActualAmount,
		CAST(SUM(br.actual_amount - br.budget_amount) AS DECIMAL(18,2)) AS Variance,
		CAST(CASE WHEN NULLIF(SUM(br.budget_amount), 0) IS NULL THEN 0 ELSE (SUM(br.actual_amount - br.budget_amount) / NULLIF(SUM(br.budget_amount), 0)) * 100 END AS DECIMAL(18,2)) AS VariancePct,
		CAST(1 AS BIT) AS IsSubtotal
	FROM BaseRows br
	GROUP BY br.month_no, br.month_name
	ORDER BY month_no, IsSubtotal, account_code;
END
GO

IF OBJECT_ID(N'dbo.sp_AutoAllocateExpenses', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_AutoAllocateExpenses;
GO

CREATE PROCEDURE dbo.sp_AutoAllocateExpenses
	@Period DATE,
	@AllocationRuleId INT = NULL,
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @PeriodStart DATE = DATEFROMPARTS(YEAR(@Period), MONTH(@Period), 1);
	DECLARE @PeriodEndExclusive DATE = DATEADD(MONTH, 1, @PeriodStart);

	DECLARE @BranchId INT;
	SELECT @BranchId = branch_id FROM dbo.users WHERE id = @UserId;

	IF @BranchId IS NULL
		SET @BranchId = 1;

	DECLARE @RuleSet TABLE
	(
		alloc_id INT NOT NULL PRIMARY KEY,
		alloc_name NVARCHAR(120) NOT NULL,
		source_acc_id INT NOT NULL,
		cc_id INT NOT NULL,
		allocation_percent DECIMAL(5,2) NOT NULL,
		allocation_method VARCHAR(20) NOT NULL,
		normalized_percent DECIMAL(18,8) NOT NULL
	);

	;WITH BaseRules AS
	(
		SELECT
			a.alloc_id,
			a.alloc_name,
			a.source_acc_id,
			a.cc_id,
			a.allocation_percent,
			a.allocation_method,
			CASE
				WHEN a.allocation_method = 'FIXED_PCT' THEN NULL
				ELSE 100.0 / NULLIF(COUNT(1) OVER (PARTITION BY a.source_acc_id, a.allocation_method), 0)
			END AS non_fixed_percent,
			SUM(CASE WHEN a.allocation_method = 'FIXED_PCT' THEN a.allocation_percent ELSE 0 END) OVER (PARTITION BY a.source_acc_id, a.allocation_method) AS fixed_total
		FROM dbo.acc_cost_center_allocations a
		WHERE a.is_active = 1
		  AND (@AllocationRuleId IS NULL OR a.alloc_id = @AllocationRuleId)
	)
	INSERT INTO @RuleSet(alloc_id, alloc_name, source_acc_id, cc_id, allocation_percent, allocation_method, normalized_percent)
	SELECT
		alloc_id,
		alloc_name,
		source_acc_id,
		cc_id,
		allocation_percent,
		allocation_method,
		CASE
			WHEN allocation_method = 'FIXED_PCT' THEN allocation_percent / NULLIF(fixed_total, 0)
			ELSE ISNULL(non_fixed_percent, 0) / 100.0
		END AS normalized_percent
	FROM BaseRules;

	IF NOT EXISTS (SELECT 1 FROM @RuleSet)
	BEGIN
		RAISERROR('No active allocation rule found for the input criteria.', 16, 1);
		RETURN;
	END

	DECLARE @ExpenseBase TABLE
	(
		source_acc_id INT NOT NULL PRIMARY KEY,
		base_amount DECIMAL(18,2) NOT NULL
	);

	INSERT INTO @ExpenseBase(source_acc_id, base_amount)
	SELECT
		r.source_acc_id,
		SUM(ISNULL(e.debit, 0) - ISNULL(e.credit, 0)) AS base_amount
	FROM @RuleSet r
	INNER JOIN dbo.acc_entries e
		ON e.account_id = r.source_acc_id
	   AND e.entry_date >= @PeriodStart
	   AND e.entry_date < @PeriodEndExclusive
	GROUP BY r.source_acc_id;

	DECLARE @AllocResult TABLE
	(
		alloc_id INT NOT NULL,
		alloc_name NVARCHAR(120) NOT NULL,
		source_acc_id INT NOT NULL,
		cc_id INT NOT NULL,
		allocation_method VARCHAR(20) NOT NULL,
		allocation_percent DECIMAL(5,2) NOT NULL,
		source_amount DECIMAL(18,2) NOT NULL,
		allocated_amount DECIMAL(18,2) NOT NULL
	);

	INSERT INTO @AllocResult(alloc_id, alloc_name, source_acc_id, cc_id, allocation_method, allocation_percent, source_amount, allocated_amount)
	SELECT
		r.alloc_id,
		r.alloc_name,
		r.source_acc_id,
		r.cc_id,
		r.allocation_method,
		r.allocation_percent,
		ISNULL(b.base_amount, 0) AS source_amount,
		CAST(ROUND(ISNULL(b.base_amount, 0) * r.normalized_percent, 2) AS DECIMAL(18,2)) AS allocated_amount
	FROM @RuleSet r
	LEFT JOIN @ExpenseBase b ON b.source_acc_id = r.source_acc_id;

	DECLARE @TotalAlloc DECIMAL(18,2) = ISNULL((SELECT SUM(allocated_amount) FROM @AllocResult), 0);

	IF @TotalAlloc = 0
	BEGIN
		SELECT
			CAST(@PeriodStart AS DATE) AS PeriodStart,
			CAST(@PeriodEndExclusive AS DATE) AS PeriodEndExclusive,
			CAST('No allocation posted because source amount is zero.' AS NVARCHAR(200)) AS Message;

		SELECT
			alloc_id,
			alloc_name,
			source_acc_id,
			cc_id,
			allocation_method,
			allocation_percent,
			source_amount,
			allocated_amount,
			CAST(NULL AS NVARCHAR(100)) AS voucher_no
		FROM @AllocResult
		ORDER BY source_acc_id, alloc_id;

		RETURN;
	END

	DECLARE @VoucherNo NVARCHAR(100) = CONCAT('CCA-', FORMAT(@PeriodStart, 'yyyyMM'), '-', RIGHT(REPLACE(CONVERT(VARCHAR(36), NEWID()), '-', ''), 8));
	DECLARE @HeaderId INT;

	BEGIN TRANSACTION;

	INSERT INTO dbo.acc_entries_header
	(
		InvoiceNo,
		EntryDate,
		VoucherType,
		ReferenceNo,
		Narration,
		Attachment,
		total_debit,
		total_credit,
		status,
		reversal_of,
		posted_by,
		posted_at,
		is_auto_posted,
		date_created,
		date_updated,
		user_id,
		branch_id
	)
	VALUES
	(
		@VoucherNo,
		@PeriodStart,
		N'Auto Cost Center Allocation',
		CAST(ISNULL(@AllocationRuleId, 0) AS NVARCHAR(40)),
		N'Automatic allocation for period ' + CONVERT(NVARCHAR(7), @PeriodStart, 120),
		NULL,
		@TotalAlloc,
		@TotalAlloc,
		N'Posted',
		NULL,
		@UserId,
		GETDATE(),
		1,
		GETDATE(),
		GETDATE(),
		@UserId,
		@BranchId
	);

	SET @HeaderId = CAST(SCOPE_IDENTITY() AS INT);

	INSERT INTO dbo.acc_entries
	(
		invoice_no,
		account_id,
		entry_date,
		debit,
		credit,
		description,
		user_id,
		branch_id,
		date_created,
		customer_id,
		supplier_id,
		bank_id,
		entry_id,
		payment_ref_invoice_no,
		cost_center_id
	)
	SELECT
		@VoucherNo,
		r.source_acc_id,
		@PeriodStart,
		r.allocated_amount,
		0,
		CONCAT('Allocation in: ', r.alloc_name),
		@UserId,
		@BranchId,
		GETDATE(),
		NULL,
		NULL,
		NULL,
		@HeaderId,
		NULL,
		r.cc_id
	FROM @AllocResult r
	WHERE r.allocated_amount <> 0;

	INSERT INTO dbo.acc_entries
	(
		invoice_no,
		account_id,
		entry_date,
		debit,
		credit,
		description,
		user_id,
		branch_id,
		date_created,
		customer_id,
		supplier_id,
		bank_id,
		entry_id,
		payment_ref_invoice_no,
		cost_center_id
	)
	SELECT
		@VoucherNo,
		r.source_acc_id,
		@PeriodStart,
		0,
		r.allocated_amount,
		CONCAT('Allocation out: ', r.alloc_name),
		@UserId,
		@BranchId,
		GETDATE(),
		NULL,
		NULL,
		NULL,
		@HeaderId,
		NULL,
		NULL
	FROM @AllocResult r
	WHERE r.allocated_amount <> 0;

	COMMIT TRANSACTION;

	SELECT
		CAST(@PeriodStart AS DATE) AS PeriodStart,
		CAST(@PeriodEndExclusive AS DATE) AS PeriodEndExclusive,
		@VoucherNo AS VoucherNo,
		@HeaderId AS EntryHeaderId,
		CAST(@TotalAlloc AS DECIMAL(18,2)) AS TotalAllocated;

	SELECT
		alloc_id,
		alloc_name,
		source_acc_id,
		cc_id,
		allocation_method,
		allocation_percent,
		CAST(source_amount AS DECIMAL(18,2)) AS source_amount,
		CAST(allocated_amount AS DECIMAL(18,2)) AS allocated_amount,
		@VoucherNo AS voucher_no
	FROM @AllocResult
	ORDER BY source_acc_id, alloc_id;
END
GO

IF OBJECT_ID(N'dbo.sp_CostCenterSummary', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_CostCenterSummary;
GO

CREATE PROCEDURE dbo.sp_CostCenterSummary
	@FromDate DATE,
	@ToDate DATE
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ToExclusive DATE = DATEADD(DAY, 1, @ToDate);

	;WITH AccountClassify AS
	(
		SELECT
			A.id AS account_id,
			CASE
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%income%'
				  OR LOWER(ISNULL(T.name, '')) LIKE '%revenue%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%income%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%revenue%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%income%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%revenue%'
					THEN 'Income'
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(T.name, '')) LIKE '%cogs%'
				  OR LOWER(ISNULL(T.name, '')) LIKE '%cost%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%cogs%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%cost%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%cogs%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%cost%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%purchase%'
					THEN 'Expense'
				ELSE 'Other'
			END AS section
		FROM dbo.acc_accounts A
		LEFT JOIN dbo.acc_groups G ON G.id = A.group_id
		LEFT JOIN dbo.acc_account_type T ON T.id = G.account_type_id
	),
	ActualTotals AS
	(
		SELECT
			E.cost_center_id,
			SUM(CASE WHEN C.section = 'Income' THEN ISNULL(E.credit, 0) - ISNULL(E.debit, 0) ELSE 0 END) AS total_income,
			SUM(CASE WHEN C.section = 'Expense' THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0) ELSE 0 END) AS total_expense
		FROM dbo.acc_entries E
		INNER JOIN AccountClassify C ON C.account_id = E.account_id
		WHERE E.cost_center_id IS NOT NULL
		  AND E.entry_date >= @FromDate
		  AND E.entry_date < @ToExclusive
		GROUP BY E.cost_center_id
	),
	BudgetRows AS
	(
		SELECT
			b.cc_id,
			DATEFROMPARTS(YEAR(f.from_date), v.month_no, 1) AS month_start,
			v.budget_amount
		FROM dbo.acc_cost_center_budgets b
		INNER JOIN dbo.acc_fiscal_years f ON f.id = b.financial_year_id
		CROSS APPLY
		(
			VALUES
				(1, b.jan_budget),
				(2, b.feb_budget),
				(3, b.mar_budget),
				(4, b.apr_budget),
				(5, b.may_budget),
				(6, b.jun_budget),
				(7, b.jul_budget),
				(8, b.aug_budget),
				(9, b.sep_budget),
				(10, b.oct_budget),
				(11, b.nov_budget),
				(12, b.dec_budget)
		) v(month_no, budget_amount)
	),
	BudgetTotals AS
	(
		SELECT
			br.cc_id,
			SUM(br.budget_amount) AS total_budget
		FROM BudgetRows br
		WHERE br.month_start >= DATEFROMPARTS(YEAR(@FromDate), MONTH(@FromDate), 1)
		  AND br.month_start <= DATEFROMPARTS(YEAR(@ToDate), MONTH(@ToDate), 1)
		GROUP BY br.cc_id
	)
	SELECT
		c.cc_code AS CCCode,
		c.cc_name AS CCName,
		c.cc_type AS [Type],
		CAST(ISNULL(a.total_income, 0) AS DECIMAL(18,2)) AS TotalIncome,
		CAST(ISNULL(a.total_expense, 0) AS DECIMAL(18,2)) AS TotalExpense,
		CAST(ISNULL(a.total_income, 0) - ISNULL(a.total_expense, 0) AS DECIMAL(18,2)) AS NetProfit,
		CAST(ISNULL(b.total_budget, 0) - ISNULL(a.total_expense, 0) AS DECIMAL(18,2)) AS BudgetVariance
	FROM dbo.acc_cost_centers c
	LEFT JOIN ActualTotals a ON a.cost_center_id = c.cc_id
	LEFT JOIN BudgetTotals b ON b.cc_id = c.cc_id
	ORDER BY c.cc_code, c.cc_name;
END
GO
