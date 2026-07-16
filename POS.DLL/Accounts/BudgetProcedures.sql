-- =============================================
-- Budget Module - Database Schema and Stored Procedures
-- =============================================

-- =============================================
-- TABLE: acc_budget_headers
-- =============================================
IF OBJECT_ID(N'dbo.acc_budget_headers', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.acc_budget_headers
	(
		budget_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_acc_budget_headers PRIMARY KEY,
		financial_year_id INT NOT NULL,
		budget_version VARCHAR(20) NOT NULL,
		cc_id INT NULL,
		budget_name NVARCHAR(100) NOT NULL,
		status VARCHAR(20) NOT NULL CONSTRAINT DF_acc_budget_headers_status DEFAULT('Draft'),
		approved_by INT NULL,
		approved_at DATETIME NULL,
		notes NVARCHAR(500) NULL,
		created_by INT NOT NULL,
		created_at DATETIME NOT NULL CONSTRAINT DF_acc_budget_headers_created_at DEFAULT(GETDATE()),
		CONSTRAINT CHK_acc_budget_headers_status CHECK (status IN ('Draft', 'Approved', 'Active'))
	);
END
GO

-- Add foreign key to acc_fiscal_years if not exists
IF OBJECT_ID(N'dbo.acc_fiscal_years', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_budget_headers_fiscal_year'
   )
BEGIN
	ALTER TABLE dbo.acc_budget_headers
	ADD CONSTRAINT FK_acc_budget_headers_fiscal_year
		FOREIGN KEY (financial_year_id) REFERENCES dbo.acc_fiscal_years(id);
END
GO

-- Add foreign key to cost centers if exists
IF OBJECT_ID(N'dbo.acc_cost_centers', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_budget_headers_cost_center'
   )
BEGIN
	ALTER TABLE dbo.acc_budget_headers
	ADD CONSTRAINT FK_acc_budget_headers_cost_center
		FOREIGN KEY (cc_id) REFERENCES dbo.acc_cost_centers(cc_id);
END
GO

-- Add foreign key to pos_users for approved_by
IF OBJECT_ID(N'dbo.pos_users', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_budget_headers_approved_by'
   )
BEGIN
	ALTER TABLE dbo.acc_budget_headers
	ADD CONSTRAINT FK_acc_budget_headers_approved_by
		FOREIGN KEY (approved_by) REFERENCES dbo.pos_users(id);
END
GO

-- Add foreign key to pos_users for created_by
IF OBJECT_ID(N'dbo.pos_users', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_budget_headers_created_by'
   )
BEGIN
	ALTER TABLE dbo.acc_budget_headers
	ADD CONSTRAINT FK_acc_budget_headers_created_by
		FOREIGN KEY (created_by) REFERENCES dbo.pos_users(id);
END
GO

-- Create unique index on financial_year_id + budget_version + cc_id
IF NOT EXISTS
(
	SELECT 1
	FROM sys.indexes
	WHERE object_id = OBJECT_ID(N'dbo.acc_budget_headers')
	  AND name = N'UX_acc_budget_headers_year_version_cc'
)
BEGIN
	CREATE UNIQUE INDEX UX_acc_budget_headers_year_version_cc
	ON dbo.acc_budget_headers(financial_year_id, budget_version, cc_id);
END
GO

-- =============================================
-- TABLE: acc_budget_lines
-- =============================================
IF OBJECT_ID(N'dbo.acc_budget_lines', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.acc_budget_lines
	(
		line_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_acc_budget_lines PRIMARY KEY,
		budget_id INT NOT NULL,
		account_id INT NOT NULL,
		jan DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_budget_lines_jan DEFAULT(0),
		feb DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_budget_lines_feb DEFAULT(0),
		mar DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_budget_lines_mar DEFAULT(0),
		apr DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_budget_lines_apr DEFAULT(0),
		may DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_budget_lines_may DEFAULT(0),
		jun DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_budget_lines_jun DEFAULT(0),
		jul DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_budget_lines_jul DEFAULT(0),
		aug DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_budget_lines_aug DEFAULT(0),
		sep DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_budget_lines_sep DEFAULT(0),
		oct DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_budget_lines_oct DEFAULT(0),
		nov DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_budget_lines_nov DEFAULT(0),
		dec DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_budget_lines_dec DEFAULT(0),
		annual_total AS (jan + feb + mar + apr + may + jun + jul + aug + sep + oct + nov + dec) PERSISTED
	);
END
GO

-- Add foreign key to budget headers
IF NOT EXISTS
(
	SELECT 1
	FROM sys.foreign_keys
	WHERE name = N'FK_acc_budget_lines_budget_header'
)
BEGIN
	ALTER TABLE dbo.acc_budget_lines
	ADD CONSTRAINT FK_acc_budget_lines_budget_header
		FOREIGN KEY (budget_id) REFERENCES dbo.acc_budget_headers(budget_id) ON DELETE CASCADE;
END
GO

-- Add foreign key to accounts
IF OBJECT_ID(N'dbo.acc_accounts', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_budget_lines_account'
   )
BEGIN
	ALTER TABLE dbo.acc_budget_lines
	ADD CONSTRAINT FK_acc_budget_lines_account
		FOREIGN KEY (account_id) REFERENCES dbo.acc_accounts(id);
END
GO

-- Create unique index on budget_id + account_id
IF NOT EXISTS
(
	SELECT 1
	FROM sys.indexes
	WHERE object_id = OBJECT_ID(N'dbo.acc_budget_lines')
	  AND name = N'UX_acc_budget_lines_budget_account'
)
BEGIN
	CREATE UNIQUE INDEX UX_acc_budget_lines_budget_account
	ON dbo.acc_budget_lines(budget_id, account_id);
END
GO

-- =============================================
-- TABLE: acc_budget_variance_notes
-- =============================================
IF OBJECT_ID(N'dbo.acc_budget_variance_notes', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.acc_budget_variance_notes
	(
		note_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_acc_budget_variance_notes PRIMARY KEY,
		budget_id INT NOT NULL,
		account_id INT NOT NULL,
		period_month INT NOT NULL,
		period_year INT NOT NULL,
		variance_note NVARCHAR(500) NULL,
		added_by INT NOT NULL,
		added_at DATETIME NOT NULL CONSTRAINT DF_acc_budget_variance_notes_added_at DEFAULT(GETDATE())
	);
END
GO

-- Add foreign key to budget headers
IF NOT EXISTS
(
	SELECT 1
	FROM sys.foreign_keys
	WHERE name = N'FK_acc_budget_variance_notes_budget'
)
BEGIN
	ALTER TABLE dbo.acc_budget_variance_notes
	ADD CONSTRAINT FK_acc_budget_variance_notes_budget
		FOREIGN KEY (budget_id) REFERENCES dbo.acc_budget_headers(budget_id) ON DELETE CASCADE;
END
GO

-- Add foreign key to accounts
IF OBJECT_ID(N'dbo.acc_accounts', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_budget_variance_notes_account'
   )
BEGIN
	ALTER TABLE dbo.acc_budget_variance_notes
	ADD CONSTRAINT FK_acc_budget_variance_notes_account
		FOREIGN KEY (account_id) REFERENCES dbo.acc_accounts(id);
END
GO

-- Add foreign key to pos_users
IF OBJECT_ID(N'dbo.pos_users', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_budget_variance_notes_user'
   )
BEGIN
	ALTER TABLE dbo.acc_budget_variance_notes
	ADD CONSTRAINT FK_acc_budget_variance_notes_user
		FOREIGN KEY (added_by) REFERENCES dbo.pos_users(id);
END
GO

-- Create index on budget_id + account_id + period
IF NOT EXISTS
(
	SELECT 1
	FROM sys.indexes
	WHERE object_id = OBJECT_ID(N'dbo.acc_budget_variance_notes')
	  AND name = N'IX_acc_budget_variance_notes_budget_account_period'
)
BEGIN
	CREATE INDEX IX_acc_budget_variance_notes_budget_account_period
	ON dbo.acc_budget_variance_notes(budget_id, account_id, period_year, period_month);
END
GO

-- =============================================
-- STORED PROCEDURE: sp_BudgetVsActual
-- Compare budget vs actual with variance analysis
-- =============================================
IF OBJECT_ID(N'dbo.sp_BudgetVsActual', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_BudgetVsActual;
GO

CREATE PROCEDURE dbo.sp_BudgetVsActual
	@BudgetId INT,
	@FromDate DATE,
	@ToDate DATE,
	@CCId INT = NULL,
	@PeriodMode VARCHAR(10) = 'YTD',
	@AccountTypeFilter VARCHAR(20) = 'All'
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CurrentMonth INT = MONTH(@ToDate);
	DECLARE @CurrentYear INT = YEAR(@ToDate);
	DECLARE @FiscalYearStart DATE;
	DECLARE @FiscalYearEnd DATE;
	DECLARE @FiscalStartMonth INT;
	DECLARE @MonthsElapsed INT;

	-- Get fiscal year dates from budget header
	SELECT 
		@FiscalYearStart = fy.from_date,
		@FiscalYearEnd = fy.to_date
	FROM acc_budget_headers bh
	INNER JOIN acc_fiscal_years fy ON bh.financial_year_id = fy.id
	WHERE bh.budget_id = @BudgetId;

	IF @FiscalYearStart IS NULL OR @FiscalYearEnd IS NULL
	BEGIN
		RAISERROR('Invalid budget/fiscal year mapping.', 16, 1);
		RETURN;
	END

	SET @FiscalStartMonth = MONTH(@FiscalYearStart);
	SET @MonthsElapsed = DATEDIFF(MONTH, DATEFROMPARTS(YEAR(@FiscalYearStart), MONTH(@FiscalYearStart), 1), DATEFROMPARTS(YEAR(@ToDate), MONTH(@ToDate), 1)) + 1;
	IF @MonthsElapsed < 1 SET @MonthsElapsed = 1;
	IF @MonthsElapsed > 12 SET @MonthsElapsed = 12;

	SET @PeriodMode = UPPER(ISNULL(@PeriodMode, 'YTD'));
	IF @PeriodMode NOT IN ('MONTH', 'QUARTER', 'YTD')
		SET @PeriodMode = 'YTD';

	SET @AccountTypeFilter = UPPER(ISNULL(@AccountTypeFilter, 'ALL'));

	WITH BudgetData AS
	(
		SELECT 
			bl.account_id,
			bl.jan, bl.feb, bl.mar, bl.apr, bl.may, bl.jun,
			bl.jul, bl.aug, bl.sep, bl.oct, bl.nov, bl.dec,
			bl.annual_total,
			(SELECT SUM(v.BudgetAmount)
			 FROM (VALUES
				(1, bl.jan), (2, bl.feb), (3, bl.mar), (4, bl.apr),
				(5, bl.may), (6, bl.jun), (7, bl.jul), (8, bl.aug),
				(9, bl.sep), (10, bl.oct), (11, bl.nov), (12, bl.dec)
			 ) v(MonthNo, BudgetAmount)
			 WHERE v.MonthNo <= @MonthsElapsed
			) AS ytd_budget,
			(SELECT SUM(v.BudgetAmount)
			 FROM (VALUES
				(1, bl.jan), (2, bl.feb), (3, bl.mar), (4, bl.apr),
				(5, bl.may), (6, bl.jun), (7, bl.jul), (8, bl.aug),
				(9, bl.sep), (10, bl.oct), (11, bl.nov), (12, bl.dec)
			 ) v(MonthNo, BudgetAmount)
			 WHERE v.MonthNo > @MonthsElapsed
			) AS remaining_budget,
			(SELECT v.BudgetAmount
			 FROM (VALUES
				(1, bl.jan), (2, bl.feb), (3, bl.mar), (4, bl.apr),
				(5, bl.may), (6, bl.jun), (7, bl.jul), (8, bl.aug),
				(9, bl.sep), (10, bl.oct), (11, bl.nov), (12, bl.dec)
			 ) v(MonthNo, BudgetAmount)
			 WHERE v.MonthNo = @MonthsElapsed
			) AS monthly_budget,
			(SELECT SUM(v.BudgetAmount)
			 FROM (VALUES
				(1, bl.jan), (2, bl.feb), (3, bl.mar), (4, bl.apr),
				(5, bl.may), (6, bl.jun), (7, bl.jul), (8, bl.aug),
				(9, bl.sep), (10, bl.oct), (11, bl.nov), (12, bl.dec)
			 ) v(MonthNo, BudgetAmount)
			 WHERE v.MonthNo BETWEEN (((@MonthsElapsed - 1) / 3) * 3 + 1) AND (((@MonthsElapsed - 1) / 3) * 3 + 3)
			) AS quarter_budget,
			(SELECT SUM(v.BudgetAmount)
			 FROM (VALUES
				(1, bl.jan), (2, bl.feb), (3, bl.mar), (4, bl.apr),
				(5, bl.may), (6, bl.jun), (7, bl.jul), (8, bl.aug),
				(9, bl.sep), (10, bl.oct), (11, bl.nov), (12, bl.dec)
			 ) v(MonthNo, BudgetAmount)
			 WHERE v.MonthNo BETWEEN 1 AND @MonthsElapsed
			) / CAST(@MonthsElapsed AS DECIMAL(18,4)) AS prorated_monthly_budget
		FROM acc_budget_lines bl
		WHERE bl.budget_id = @BudgetId
	),
	ActualData AS
	(
		SELECT 
			ae.account_id,
			SUM(CASE WHEN ae.entry_date BETWEEN @FiscalYearStart AND @ToDate THEN ae.debit - ae.credit ELSE 0 END) AS ytd_actual,
			SUM(CASE WHEN YEAR(ae.entry_date) = YEAR(@ToDate) AND MONTH(ae.entry_date) = MONTH(@ToDate) THEN ae.debit - ae.credit ELSE 0 END) AS monthly_actual,
			SUM(CASE WHEN ae.entry_date >= DATEADD(MONTH, ((@MonthsElapsed - 1) / 3) * 3, @FiscalYearStart)
					  AND ae.entry_date < DATEADD(MONTH, ((@MonthsElapsed - 1) / 3) * 3 + 3, @FiscalYearStart)
				 THEN ae.debit - ae.credit ELSE 0 END) AS quarter_actual
		FROM acc_entries ae
		INNER JOIN acc_entries_header aeh ON ae.invoice_no = aeh.InvoiceNo AND ae.branch_id = aeh.branch_id
		WHERE aeh.status = 'Posted'
		  AND ae.entry_date BETWEEN @FiscalYearStart AND @FiscalYearEnd
		  AND (@CCId IS NULL OR ae.cost_center_id = @CCId)
		GROUP BY ae.account_id
	)
	SELECT 
		a.id AS acc_id,
		a.code AS acc_code,
		a.name AS acc_name,
		ISNULL(t.name, '') AS account_type,
		bd.annual_total AS annual_budget,
		bd.ytd_budget,
		ISNULL(ad.ytd_actual, 0) AS ytd_actual,
		ISNULL(ad.ytd_actual, 0) - bd.ytd_budget AS ytd_variance,
		CASE WHEN bd.ytd_budget = 0 THEN 0 ELSE ((ISNULL(ad.ytd_actual, 0) - bd.ytd_budget) / NULLIF(bd.ytd_budget, 0)) * 100 END AS ytd_variance_pct,
		bd.monthly_budget,
		ISNULL(ad.monthly_actual, 0) AS monthly_actual,
		ISNULL(ad.monthly_actual, 0) - bd.monthly_budget AS monthly_variance,
		bd.quarter_budget,
		ISNULL(ad.quarter_actual, 0) AS quarter_actual,
		ISNULL(ad.quarter_actual, 0) - bd.quarter_budget AS quarter_variance,
		CASE WHEN bd.quarter_budget = 0 THEN 0 ELSE ((ISNULL(ad.quarter_actual, 0) - bd.quarter_budget) / NULLIF(bd.quarter_budget, 0)) * 100 END AS quarter_variance_pct,
		bd.remaining_budget,
		(ISNULL(ad.ytd_actual, 0) + bd.remaining_budget) AS full_year_forecast,
		CASE WHEN bd.annual_total = 0 THEN 0 ELSE ((ISNULL(ad.ytd_actual, 0) + bd.remaining_budget) / NULLIF(bd.annual_total, 0)) * 100 END AS forecast_achievement_pct,
		CASE 
			WHEN @PeriodMode = 'MONTH' THEN bd.monthly_budget
			WHEN @PeriodMode = 'QUARTER' THEN bd.quarter_budget
			ELSE bd.ytd_budget
		END AS period_budget,
		CASE 
			WHEN @PeriodMode = 'MONTH' THEN ISNULL(ad.monthly_actual, 0)
			WHEN @PeriodMode = 'QUARTER' THEN ISNULL(ad.quarter_actual, 0)
			ELSE ISNULL(ad.ytd_actual, 0)
		END AS period_actual,
		CASE 
			WHEN @PeriodMode = 'MONTH' THEN ISNULL(ad.monthly_actual, 0) - bd.monthly_budget
			WHEN @PeriodMode = 'QUARTER' THEN ISNULL(ad.quarter_actual, 0) - bd.quarter_budget
			ELSE ISNULL(ad.ytd_actual, 0) - bd.ytd_budget
		END AS period_variance,
		CASE 
			WHEN @PeriodMode = 'MONTH' AND bd.monthly_budget <> 0 THEN ((ISNULL(ad.monthly_actual, 0) - bd.monthly_budget) / NULLIF(bd.monthly_budget, 0)) * 100
			WHEN @PeriodMode = 'QUARTER' AND bd.quarter_budget <> 0 THEN ((ISNULL(ad.quarter_actual, 0) - bd.quarter_budget) / NULLIF(bd.quarter_budget, 0)) * 100
			WHEN @PeriodMode = 'YTD' AND bd.ytd_budget <> 0 THEN ((ISNULL(ad.ytd_actual, 0) - bd.ytd_budget) / NULLIF(bd.ytd_budget, 0)) * 100
			ELSE 0
		END AS period_variance_pct,
		CASE 
			WHEN ISNULL(t.name, '') IN ('Income', 'Revenue', 'Other Income') AND (ISNULL(ad.ytd_actual, 0) - bd.ytd_budget) >= 0 THEN 1
			WHEN ISNULL(t.name, '') IN ('Expense', 'COGS', 'Cost of Goods Sold') AND (ISNULL(ad.ytd_actual, 0) - bd.ytd_budget) <= 0 THEN 1
			WHEN ISNULL(t.name, '') NOT IN ('Income', 'Revenue', 'Other Income', 'Expense', 'COGS', 'Cost of Goods Sold') AND (ISNULL(ad.ytd_actual, 0) - bd.ytd_budget) <= 0 THEN 1
			ELSE 0
		END AS is_favorable,
		DATEPART(MONTH, @ToDate) AS period_month,
		DATEPART(YEAR, @ToDate) AS period_year
	FROM BudgetData bd
	INNER JOIN acc_accounts a ON bd.account_id = a.id
	INNER JOIN acc_groups g ON a.group_id = g.id
	LEFT JOIN acc_account_type t ON g.account_type_id = t.id
	LEFT JOIN ActualData ad ON bd.account_id = ad.account_id
	WHERE 
		@AccountTypeFilter = 'ALL'
		OR (@AccountTypeFilter = 'INCOME' AND ISNULL(t.name, '') IN ('Income', 'Revenue', 'Other Income'))
		OR (@AccountTypeFilter = 'EXPENSE' AND ISNULL(t.name, '') IN ('Expense', 'COGS', 'Cost of Goods Sold'))
	ORDER BY t.name, a.code;
END
GO

-- =============================================
-- STORED PROCEDURE: sp_BudgetMonthlyDetail
-- Returns monthly breakdown for a specific account
-- =============================================
IF OBJECT_ID(N'dbo.sp_BudgetMonthlyDetail', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_BudgetMonthlyDetail;
GO

CREATE PROCEDURE dbo.sp_BudgetMonthlyDetail
	@BudgetId INT,
	@AccId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FiscalYearStart DATE;
	DECLARE @FiscalYearId INT;

	-- Get fiscal year info
	SELECT 
		@FiscalYearStart = fy.from_date,
		@FiscalYearId = fy.id
	FROM acc_budget_headers bh
	INNER JOIN acc_fiscal_years fy ON bh.financial_year_id = fy.id
	WHERE bh.budget_id = @BudgetId;

	-- Get budget amounts
	DECLARE @Jan DECIMAL(18,2), @Feb DECIMAL(18,2), @Mar DECIMAL(18,2),
			@Apr DECIMAL(18,2), @May DECIMAL(18,2), @Jun DECIMAL(18,2),
			@Jul DECIMAL(18,2), @Aug DECIMAL(18,2), @Sep DECIMAL(18,2),
			@Oct DECIMAL(18,2), @Nov DECIMAL(18,2), @Dec DECIMAL(18,2);

	SELECT 
		@Jan = jan, @Feb = feb, @Mar = mar,
		@Apr = apr, @May = may, @Jun = jun,
		@Jul = jul, @Aug = aug, @Sep = sep,
		@Oct = oct, @Nov = nov, @Dec = dec
	FROM acc_budget_lines
	WHERE budget_id = @BudgetId AND account_id = @AccId;

	-- Get actual amounts by month
	WITH MonthlyActuals AS
	(
		SELECT 
			MONTH(ae.entry_date) AS MonthNo,
			SUM(ae.debit - ae.credit) AS ActualAmount
		FROM acc_entries ae
		INNER JOIN acc_entries_header aeh ON ae.invoice_no = aeh.InvoiceNo AND ae.branch_id = aeh.branch_id
		WHERE ae.account_id = @AccId
		  AND aeh.status = 'Posted'
		  AND ae.entry_date >= @FiscalYearStart
		  AND DATEDIFF(MONTH, @FiscalYearStart, ae.entry_date) < 12
		GROUP BY MONTH(ae.entry_date)
	)
	SELECT 
		m.MonthNo,
		DATENAME(MONTH, DATEADD(MONTH, m.MonthNo - 1, 0)) AS MonthName,
		m.BudgetAmount,
		ISNULL(ma.ActualAmount, 0) AS ActualAmount,
		ISNULL(ma.ActualAmount, 0) - m.BudgetAmount AS Variance,
		SUM(m.BudgetAmount) OVER (ORDER BY m.MonthNo) AS CumulativeBudget,
		SUM(ISNULL(ma.ActualAmount, 0)) OVER (ORDER BY m.MonthNo) AS CumulativeActual
	FROM
	(
		SELECT 1 AS MonthNo, @Jan AS BudgetAmount UNION ALL
		SELECT 2, @Feb UNION ALL
		SELECT 3, @Mar UNION ALL
		SELECT 4, @Apr UNION ALL
		SELECT 5, @May UNION ALL
		SELECT 6, @Jun UNION ALL
		SELECT 7, @Jul UNION ALL
		SELECT 8, @Aug UNION ALL
		SELECT 9, @Sep UNION ALL
		SELECT 10, @Oct UNION ALL
		SELECT 11, @Nov UNION ALL
		SELECT 12, @Dec
	) m
	LEFT JOIN MonthlyActuals ma ON m.MonthNo = ma.MonthNo
	ORDER BY m.MonthNo;
END
GO

-- =============================================
-- STORED PROCEDURE: sp_CopyBudgetFromActuals
-- Copies last year's actuals as new budget baseline
-- =============================================
IF OBJECT_ID(N'dbo.sp_CopyBudgetFromActuals', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_CopyBudgetFromActuals;
GO

CREATE PROCEDURE dbo.sp_CopyBudgetFromActuals
	@SourceYearId INT,
	@TargetBudgetId INT,
	@GrowthPct DECIMAL(5,2) = 0
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRY
		BEGIN TRANSACTION;

		DECLARE @SourceYearStart DATE;
		DECLARE @SourceYearEnd DATE;

		-- Get source year dates
		SELECT 
			@SourceYearStart = from_date,
			@SourceYearEnd = to_date
		FROM acc_fiscal_years
		WHERE id = @SourceYearId;

		-- Delete existing budget lines for target budget
		DELETE FROM acc_budget_lines
		WHERE budget_id = @TargetBudgetId;

		-- Insert budget lines based on actuals with growth factor
		DECLARE @GrowthMultiplier DECIMAL(10,4) = 1 + (@GrowthPct / 100.0);

		INSERT INTO acc_budget_lines
		(
			budget_id,
			account_id,
			jan, feb, mar, apr, may, jun,
			jul, aug, sep, oct, nov, dec
		)
		SELECT 
			@TargetBudgetId,
			ae.account_id,
			SUM(CASE WHEN MONTH(ae.entry_date) = 1 THEN (ae.debit - ae.credit) * @GrowthMultiplier ELSE 0 END) AS jan,
			SUM(CASE WHEN MONTH(ae.entry_date) = 2 THEN (ae.debit - ae.credit) * @GrowthMultiplier ELSE 0 END) AS feb,
			SUM(CASE WHEN MONTH(ae.entry_date) = 3 THEN (ae.debit - ae.credit) * @GrowthMultiplier ELSE 0 END) AS mar,
			SUM(CASE WHEN MONTH(ae.entry_date) = 4 THEN (ae.debit - ae.credit) * @GrowthMultiplier ELSE 0 END) AS apr,
			SUM(CASE WHEN MONTH(ae.entry_date) = 5 THEN (ae.debit - ae.credit) * @GrowthMultiplier ELSE 0 END) AS may,
			SUM(CASE WHEN MONTH(ae.entry_date) = 6 THEN (ae.debit - ae.credit) * @GrowthMultiplier ELSE 0 END) AS jun,
			SUM(CASE WHEN MONTH(ae.entry_date) = 7 THEN (ae.debit - ae.credit) * @GrowthMultiplier ELSE 0 END) AS jul,
			SUM(CASE WHEN MONTH(ae.entry_date) = 8 THEN (ae.debit - ae.credit) * @GrowthMultiplier ELSE 0 END) AS aug,
			SUM(CASE WHEN MONTH(ae.entry_date) = 9 THEN (ae.debit - ae.credit) * @GrowthMultiplier ELSE 0 END) AS sep,
			SUM(CASE WHEN MONTH(ae.entry_date) = 10 THEN (ae.debit - ae.credit) * @GrowthMultiplier ELSE 0 END) AS oct,
			SUM(CASE WHEN MONTH(ae.entry_date) = 11 THEN (ae.debit - ae.credit) * @GrowthMultiplier ELSE 0 END) AS nov,
			SUM(CASE WHEN MONTH(ae.entry_date) = 12 THEN (ae.debit - ae.credit) * @GrowthMultiplier ELSE 0 END) AS dec
		FROM acc_entries ae
		INNER JOIN acc_entries_header aeh ON ae.invoice_no = aeh.InvoiceNo AND ae.branch_id = aeh.branch_id
		WHERE aeh.status = 'Posted'
		  AND ae.entry_date BETWEEN @SourceYearStart AND @SourceYearEnd
		GROUP BY ae.account_id
		HAVING SUM(ABS(ae.debit - ae.credit)) > 0;

		COMMIT TRANSACTION;

		SELECT 
			@@ROWCOUNT AS rows_copied,
			'Budget copied successfully from actuals' AS message;

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
		DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
		DECLARE @ErrorState INT = ERROR_STATE();

		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO

-- =============================================
-- TABLE-VALUED TYPE: MonthlyPercentagesType
-- Used for seasonal spread procedure
-- =============================================
IF NOT EXISTS (SELECT 1 FROM sys.types WHERE name = 'MonthlyPercentagesType' AND is_table_type = 1)
BEGIN
	CREATE TYPE dbo.MonthlyPercentagesType AS TABLE
	(
		MonthNo INT NOT NULL,
		Percentage DECIMAL(5,2) NOT NULL
	);
END
GO

-- =============================================
-- STORED PROCEDURE: sp_BudgetSeasonalSpread
-- Distributes annual budget across months using percentages
-- =============================================
IF OBJECT_ID(N'dbo.sp_BudgetSeasonalSpread', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_BudgetSeasonalSpread;
GO

CREATE PROCEDURE dbo.sp_BudgetSeasonalSpread
	@BudgetId INT,
	@AccId INT,
	@AnnualAmount DECIMAL(18,2),
	@Percentages MonthlyPercentagesType READONLY
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	BEGIN TRY
		-- Validate that percentages sum to 100
		DECLARE @TotalPct DECIMAL(10,2);
		SELECT @TotalPct = SUM(Percentage) FROM @Percentages;

		IF ABS(@TotalPct - 100.0) > 0.01
		BEGIN
			DECLARE @TotalPctText VARCHAR(32) = CONVERT(VARCHAR(32), @TotalPct);
			RAISERROR('Seasonal percentages must sum to 100%%. Current sum: %s', 16, 1, @TotalPctText);
			RETURN;
		END

		-- Calculate monthly amounts
		DECLARE @Jan DECIMAL(18,2) = @AnnualAmount * (SELECT Percentage FROM @Percentages WHERE MonthNo = 1) / 100.0;
		DECLARE @Feb DECIMAL(18,2) = @AnnualAmount * (SELECT Percentage FROM @Percentages WHERE MonthNo = 2) / 100.0;
		DECLARE @Mar DECIMAL(18,2) = @AnnualAmount * (SELECT Percentage FROM @Percentages WHERE MonthNo = 3) / 100.0;
		DECLARE @Apr DECIMAL(18,2) = @AnnualAmount * (SELECT Percentage FROM @Percentages WHERE MonthNo = 4) / 100.0;
		DECLARE @May DECIMAL(18,2) = @AnnualAmount * (SELECT Percentage FROM @Percentages WHERE MonthNo = 5) / 100.0;
		DECLARE @Jun DECIMAL(18,2) = @AnnualAmount * (SELECT Percentage FROM @Percentages WHERE MonthNo = 6) / 100.0;
		DECLARE @Jul DECIMAL(18,2) = @AnnualAmount * (SELECT Percentage FROM @Percentages WHERE MonthNo = 7) / 100.0;
		DECLARE @Aug DECIMAL(18,2) = @AnnualAmount * (SELECT Percentage FROM @Percentages WHERE MonthNo = 8) / 100.0;
		DECLARE @Sep DECIMAL(18,2) = @AnnualAmount * (SELECT Percentage FROM @Percentages WHERE MonthNo = 9) / 100.0;
		DECLARE @Oct DECIMAL(18,2) = @AnnualAmount * (SELECT Percentage FROM @Percentages WHERE MonthNo = 10) / 100.0;
		DECLARE @Nov DECIMAL(18,2) = @AnnualAmount * (SELECT Percentage FROM @Percentages WHERE MonthNo = 11) / 100.0;
		DECLARE @Dec DECIMAL(18,2) = @AnnualAmount * (SELECT Percentage FROM @Percentages WHERE MonthNo = 12) / 100.0;

		-- Update or insert budget line
		IF EXISTS (SELECT 1 FROM acc_budget_lines WHERE budget_id = @BudgetId AND account_id = @AccId)
		BEGIN
			UPDATE acc_budget_lines
			SET jan = @Jan, feb = @Feb, mar = @Mar,
				apr = @Apr, may = @May, jun = @Jun,
				jul = @Jul, aug = @Aug, sep = @Sep,
				oct = @Oct, nov = @Nov, dec = @Dec
			WHERE budget_id = @BudgetId AND account_id = @AccId;
		END
		ELSE
		BEGIN
			INSERT INTO acc_budget_lines
			(budget_id, account_id, jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec)
			VALUES
			(@BudgetId, @AccId, @Jan, @Feb, @Mar, @Apr, @May, @Jun, @Jul, @Aug, @Sep, @Oct, @Nov, @Dec);
		END

		SELECT 
			'Budget seasonal spread applied successfully' AS message,
			@AnnualAmount AS annual_amount,
			@Jan + @Feb + @Mar + @Apr + @May + @Jun + @Jul + @Aug + @Sep + @Oct + @Nov + @Dec AS calculated_total;

	END TRY
	BEGIN CATCH
		DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
		DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
		DECLARE @ErrorState INT = ERROR_STATE();

		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO

-- =============================================
-- STORED PROCEDURE: sp_BudgetSummaryKPIs
-- Returns high-level budget performance KPIs
-- =============================================
IF OBJECT_ID(N'dbo.sp_BudgetSummaryKPIs', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_BudgetSummaryKPIs;
GO

CREATE PROCEDURE dbo.sp_BudgetSummaryKPIs
	@BudgetId INT,
	@AsOfDate DATE
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FiscalYearStart DATE;
	DECLARE @CurrentMonth INT = MONTH(@AsOfDate);

	-- Get fiscal year start
	SELECT @FiscalYearStart = fy.from_date
	FROM acc_budget_headers bh
	INNER JOIN acc_fiscal_years fy ON bh.financial_year_id = fy.id
	WHERE bh.budget_id = @BudgetId;

	-- Calculate KPIs
	WITH BudgetTotals AS
	(
		SELECT 
			ISNULL(t.name, '') AS account_type,
			-- YTD Budget
			SUM(CASE @CurrentMonth
				WHEN 1 THEN bl.jan
				WHEN 2 THEN bl.jan + bl.feb
				WHEN 3 THEN bl.jan + bl.feb + bl.mar
				WHEN 4 THEN bl.jan + bl.feb + bl.mar + bl.apr
				WHEN 5 THEN bl.jan + bl.feb + bl.mar + bl.apr + bl.may
				WHEN 6 THEN bl.jan + bl.feb + bl.mar + bl.apr + bl.may + bl.jun
				WHEN 7 THEN bl.jan + bl.feb + bl.mar + bl.apr + bl.may + bl.jun + bl.jul
				WHEN 8 THEN bl.jan + bl.feb + bl.mar + bl.apr + bl.may + bl.jun + bl.jul + bl.aug
				WHEN 9 THEN bl.jan + bl.feb + bl.mar + bl.apr + bl.may + bl.jun + bl.jul + bl.aug + bl.sep
				WHEN 10 THEN bl.jan + bl.feb + bl.mar + bl.apr + bl.may + bl.jun + bl.jul + bl.aug + bl.sep + bl.oct
				WHEN 11 THEN bl.jan + bl.feb + bl.mar + bl.apr + bl.may + bl.jun + bl.jul + bl.aug + bl.sep + bl.oct + bl.nov
				ELSE bl.annual_total
			END) AS ytd_budget,
			-- Annual Budget
			SUM(bl.annual_total) AS annual_budget
		FROM acc_budget_lines bl
		INNER JOIN acc_accounts a ON bl.account_id = a.id
		INNER JOIN acc_groups g ON a.group_id = g.id
		LEFT JOIN acc_account_type t ON g.account_type_id = t.id
		WHERE bl.budget_id = @BudgetId
		GROUP BY ISNULL(t.name, '')
	),
	ActualTotals AS
	(
		SELECT 
			ISNULL(t.name, '') AS account_type,
			SUM(CASE WHEN ae.entry_date BETWEEN @FiscalYearStart AND @AsOfDate THEN ae.debit - ae.credit ELSE 0 END) AS ytd_actual
		FROM acc_entries ae
		INNER JOIN acc_entries_header aeh ON ae.invoice_no = aeh.InvoiceNo AND ae.branch_id = aeh.branch_id
		INNER JOIN acc_accounts a ON ae.account_id = a.id
		INNER JOIN acc_groups g ON a.group_id = g.id
		LEFT JOIN acc_account_type t ON g.account_type_id = t.id
		WHERE aeh.status = 'Posted'
		GROUP BY ISNULL(t.name, '')
	)
	SELECT 
		ISNULL(SUM(CASE WHEN bt.account_type IN ('Income', 'Revenue') THEN bt.ytd_budget ELSE 0 END), 0) AS TotalIncomeBudget,
		ISNULL(SUM(CASE WHEN at.account_type IN ('Income', 'Revenue') THEN at.ytd_actual ELSE 0 END), 0) AS TotalIncomeActual,
		ISNULL(SUM(CASE WHEN bt.account_type = 'Expense' THEN bt.ytd_budget ELSE 0 END), 0) AS TotalExpenseBudget,
		ISNULL(SUM(CASE WHEN at.account_type = 'Expense' THEN at.ytd_actual ELSE 0 END), 0) AS TotalExpenseActual,
		-- Net Profit = Income - Expense
		ISNULL(SUM(CASE WHEN bt.account_type IN ('Income', 'Revenue') THEN bt.ytd_budget ELSE 0 END), 0) -
		ISNULL(SUM(CASE WHEN bt.account_type = 'Expense' THEN bt.ytd_budget ELSE 0 END), 0) AS NetProfitBudget,
		ISNULL(SUM(CASE WHEN at.account_type IN ('Income', 'Revenue') THEN at.ytd_actual ELSE 0 END), 0) -
		ISNULL(SUM(CASE WHEN at.account_type = 'Expense' THEN at.ytd_actual ELSE 0 END), 0) AS NetProfitActual,
		-- Overall Achievement %
		CASE 
			WHEN SUM(bt.ytd_budget) = 0 THEN 0
			ELSE (SUM(ISNULL(at.ytd_actual, 0)) / NULLIF(SUM(bt.ytd_budget), 0)) * 100
		END AS OverallAchievementPct
	FROM BudgetTotals bt
	LEFT JOIN ActualTotals at ON bt.account_type = at.account_type;
END
GO

PRINT 'Budget module schema and stored procedures created successfully.';
GO
