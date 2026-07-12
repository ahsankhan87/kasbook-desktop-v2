IF OBJECT_ID('dbo.sp_CheckPeriodClose', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_CheckPeriodClose;
GO

CREATE PROCEDURE dbo.sp_CheckPeriodClose
	@period_id INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @start_date DATE;
	DECLARE @end_date DATE;

	SELECT
		@start_date = CAST(start_date AS DATE),
		@end_date = CAST(end_date AS DATE)
	FROM acc_financial_periods
	WHERE period_id = @period_id;

	IF @start_date IS NULL OR @end_date IS NULL
	BEGIN
		SELECT
			CAST('period' AS NVARCHAR(60)) AS item_key,
			CAST('Financial period not found.' AS NVARCHAR(200)) AS item_name,
			CAST(0 AS BIT) AS is_passed,
			CAST(1 AS INT) AS pending_count,
			CAST('Master > Fiscal Years' AS NVARCHAR(120)) AS fix_module;
		RETURN;
	END

	;WITH VoucherBase AS
	(
		SELECT
			h.InvoiceNo AS voucher_no,
			h.EntryDate AS entry_date,
			ISNULL(h.VoucherType, '') AS voucher_type,
			ISNULL(h.status, 'Posted') AS voucher_status
		FROM acc_entries_header h
		WHERE h.EntryDate >= @start_date
		  AND h.EntryDate < DATEADD(DAY, 1, @end_date)
	),
	VoucherBalance AS
	(
		SELECT
			vb.voucher_no,
			SUM(ISNULL(e.debit, 0) - ISNULL(e.credit, 0)) AS balance_delta
		FROM VoucherBase vb
		LEFT JOIN acc_entries e ON vb.voucher_no = e.invoice_no
		GROUP BY vb.voucher_no
	)
	SELECT
		chk.item_key,
		chk.item_name,
		CAST(CASE WHEN chk.pending_count = 0 THEN 1 ELSE 0 END AS BIT) AS is_passed,
		chk.pending_count,
		chk.fix_module
	FROM
	(
		SELECT
			'sales_posted' AS item_key,
			'All sales invoices posted for the period' AS item_name,
			SUM(CASE WHEN voucher_type LIKE 'Sales%' AND voucher_status IN ('Draft', 'Unposted') THEN 1 ELSE 0 END) AS pending_count,
			'Sales Invoices' AS fix_module
		FROM VoucherBase

		UNION ALL

		SELECT
			'purchase_posted',
			'All purchase bills entered',
			SUM(CASE WHEN voucher_type LIKE 'Purchase%' AND voucher_status IN ('Draft', 'Unposted') THEN 1 ELSE 0 END),
			'Purchases'
		FROM VoucherBase

		UNION ALL

		SELECT
			'bank_reconciliation',
			'Bank reconciliation completed',
			SUM(CASE WHEN voucher_type LIKE 'Bank Reconciliation%' AND voucher_status IN ('Draft', 'Unposted') THEN 1 ELSE 0 END),
			'Bank Reconciliation'
		FROM VoucherBase

		UNION ALL

		SELECT
			'unposted_jv',
			'No unposted journal vouchers',
			SUM(CASE WHEN voucher_type LIKE '%Journal%' AND voucher_status IN ('Draft', 'Unposted') THEN 1 ELSE 0 END),
			'Journal Vouchers'
		FROM VoucherBase

		UNION ALL

		SELECT
			'ar_ap_allocations',
			'AR/AP allocations up to date',
			SUM(CASE WHEN ABS(ISNULL(balance_delta, 0)) > 0.009 THEN 1 ELSE 0 END),
			'AR/AP Allocations'
		FROM VoucherBalance
	) chk;
END
GO

IF OBJECT_ID('dbo.sp_ClosePeriod', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_ClosePeriod;
GO

CREATE PROCEDURE dbo.sp_ClosePeriod
	@period_id INT,
	@close_type NVARCHAR(20),
	@closed_by INT,
	@auto_post_depreciation BIT = 0,
	@reverse_prior_accruals BIT = 0,
	@confirmation_text NVARCHAR(250) = NULL,
	@is_success BIT OUTPUT,
	@result_message NVARCHAR(500) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @current_status NVARCHAR(30);
	DECLARE @target_status NVARCHAR(30);

	SET @is_success = 0;
	SET @result_message = N'';

	SELECT @current_status = status
	FROM acc_financial_periods
	WHERE period_id = @period_id;

	IF @current_status IS NULL
	BEGIN
		SET @result_message = N'Financial period was not found.';
		RETURN;
	END

	IF @close_type NOT IN ('Soft', 'Hard')
	BEGIN
		SET @result_message = N'Invalid close type. Use Soft or Hard.';
		RETURN;
	END

	IF @close_type = 'Soft' AND @current_status = 'Hard-Locked'
	BEGIN
		SET @result_message = N'Hard-locked period cannot be soft-closed.';
		RETURN;
	END

	IF @close_type = 'Soft'
	BEGIN
		DECLARE @pending_count INT;
		DECLARE @checklist TABLE
		(
			item_key NVARCHAR(60),
			item_name NVARCHAR(200),
			is_passed BIT,
			pending_count INT,
			fix_module NVARCHAR(120)
		);

		INSERT INTO @checklist(item_key, item_name, is_passed, pending_count, fix_module)
		EXEC dbo.sp_CheckPeriodClose @period_id;

		SELECT @pending_count = SUM(CASE WHEN is_passed = 1 THEN 0 ELSE pending_count END)
		FROM @checklist;

		IF ISNULL(@pending_count, 0) > 0
		BEGIN
			SET @result_message = N'Checklist has pending issues. Resolve before soft close.';
			RETURN;
		END

		SET @target_status = 'Soft-Closed';
	END
	ELSE
	BEGIN
		SET @target_status = 'Hard-Locked';
	END

	IF @close_type = 'Hard' AND @current_status = 'Hard-Locked'
	BEGIN
		SET @result_message = N'Period is already hard-locked.';
		RETURN;
	END

	IF @close_type = 'Soft' AND @current_status = 'Soft-Closed'
	BEGIN
		SET @result_message = N'Period is already soft-closed.';
		RETURN;
	END

	BEGIN TRANSACTION;

	UPDATE acc_financial_periods
	SET status = @target_status,
		closed_by = CAST(@closed_by AS NVARCHAR(50)),
		closed_at = GETDATE()
	WHERE period_id = @period_id;

	IF @auto_post_depreciation = 1 AND OBJECT_ID('dbo.sp_PostDepreciationForPeriod', 'P') IS NOT NULL
	BEGIN
		EXEC dbo.sp_PostDepreciationForPeriod @period_id;
	END

	IF @reverse_prior_accruals = 1 AND OBJECT_ID('dbo.sp_ReverseAccrualsForPeriod', 'P') IS NOT NULL
	BEGIN
		EXEC dbo.sp_ReverseAccrualsForPeriod @period_id;
	END

	COMMIT TRANSACTION;

	SET @is_success = 1;
	SET @result_message = CASE WHEN @close_type = 'Soft'
							   THEN N'Period soft-closed successfully.'
							   ELSE N'Period hard-locked successfully.' END;
END
GO

IF OBJECT_ID('dbo.sp_OpenNextFinancialPeriod', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_OpenNextFinancialPeriod;
GO

CREATE PROCEDURE dbo.sp_OpenNextFinancialPeriod
	@year_id INT,
	@new_period_id INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @next_start DATE;
	DECLARE @next_end DATE;
	DECLARE @period_name NVARCHAR(60);

	SET @new_period_id = 0;

	SELECT TOP 1 @next_start = DATEADD(DAY, 1, end_date)
	FROM acc_financial_periods
	WHERE year_id = @year_id
	ORDER BY end_date DESC;

	IF @next_start IS NULL
	BEGIN
		SELECT @next_start = CAST(from_date AS DATE)
		FROM acc_fiscal_years
		WHERE id = @year_id;
	END

	IF @next_start IS NULL
		RETURN;

	SET @next_end = EOMONTH(@next_start);
	SET @period_name = DATENAME(MONTH, @next_start) + ' ' + CAST(YEAR(@next_start) AS NVARCHAR(4));

	IF EXISTS (SELECT 1 FROM acc_financial_periods WHERE year_id = @year_id AND start_date = @next_start)
		RETURN;

	INSERT INTO acc_financial_periods(year_id, period_name, start_date, end_date, status, closed_by, closed_at)
	VALUES(@year_id, @period_name, @next_start, @next_end, 'Open', NULL, NULL);

	SET @new_period_id = CAST(SCOPE_IDENTITY() AS INT);
END
GO

IF OBJECT_ID('dbo.acc_year_end_close_runs', 'U') IS NULL
BEGIN
	CREATE TABLE dbo.acc_year_end_close_runs
	(
		run_id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		year_id INT NOT NULL,
		new_year_id INT NOT NULL,
		close_date DATE NOT NULL,
		closed_by INT NOT NULL,
		branch_id INT NULL,
		status NVARCHAR(30) NOT NULL,
		net_profit_loss DECIMAL(18,2) NOT NULL CONSTRAINT DF_acc_year_end_close_runs_net DEFAULT(0),
		closing_voucher_no NVARCHAR(100) NULL,
		opening_voucher_no NVARCHAR(100) NULL,
		pre_close_report_json NVARCHAR(MAX) NULL,
		pre_snapshot_json NVARCHAR(MAX) NULL,
		post_snapshot_json NVARCHAR(MAX) NULL,
		started_at DATETIME NOT NULL CONSTRAINT DF_acc_year_end_close_runs_started_at DEFAULT(GETDATE()),
		completed_at DATETIME NULL,
		rolled_back_at DATETIME NULL,
		rolled_back_by INT NULL,
		rollback_reason NVARCHAR(500) NULL
	);
END
GO

IF OBJECT_ID('dbo.sp_YearEndPreCloseValidation', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_YearEndPreCloseValidation;
GO

CREATE PROCEDURE dbo.sp_YearEndPreCloseValidation
	@year_id INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @year_start DATE;
	DECLARE @year_end DATE;

	SELECT
		@year_start = CAST(from_date AS DATE),
		@year_end = CAST(to_date AS DATE)
	FROM acc_fiscal_years
	WHERE id = @year_id;

	DECLARE @result TABLE
	(
		check_key NVARCHAR(80),
		check_name NVARCHAR(250),
		is_passed BIT,
		failed_count INT,
		details NVARCHAR(500)
	);

	IF @year_start IS NULL OR @year_end IS NULL
	BEGIN
		INSERT INTO @result(check_key, check_name, is_passed, failed_count, details)
		VALUES('year_exists', 'Financial year exists', 0, 1, 'Closing year was not found.');

		SELECT * FROM @result ORDER BY check_key;
		RETURN;
	END

	DECLARE @period_count INT = ISNULL((SELECT COUNT(1) FROM acc_financial_periods WHERE year_id = @year_id), 0);
	DECLARE @open_or_not_soft INT = ISNULL((SELECT COUNT(1) FROM acc_financial_periods WHERE year_id = @year_id AND ISNULL(status, 'Open') <> 'Soft-Closed'), 0);
	DECLARE @draft_count INT = ISNULL((
		SELECT COUNT(1)
		FROM acc_entries_header h
		WHERE h.EntryDate >= @year_start
		  AND h.EntryDate < DATEADD(DAY, 1, @year_end)
		  AND ISNULL(h.status, 'Posted') IN ('Draft', 'Unposted')
	), 0);
	DECLARE @out_of_balance_count INT = ISNULL((
		SELECT COUNT(1)
		FROM
		(
			SELECT e.invoice_no,
				   SUM(ISNULL(e.debit, 0) - ISNULL(e.credit, 0)) AS delta
			FROM acc_entries e
			INNER JOIN acc_entries_header h ON h.InvoiceNo = e.invoice_no
			WHERE h.EntryDate >= @year_start
			  AND h.EntryDate < DATEADD(DAY, 1, @year_end)
			GROUP BY e.invoice_no
		) b
		WHERE ABS(ISNULL(b.delta, 0)) > 0.009
	), 0);
	DECLARE @income_summary_count INT = ISNULL((
		SELECT COUNT(1)
		FROM acc_accounts a
		WHERE ISNULL(a.is_active, 1) = 1
		  AND (
				UPPER(ISNULL(a.name, '')) LIKE '%INCOME SUMMARY%'
				OR UPPER(ISNULL(a.code, '')) IN ('INCOME-SUMMARY', 'INCOME_SUMMARY', 'INCOME SUMMARY')
			  )
	), 0);
	DECLARE @retained_earnings_count INT = ISNULL((
		SELECT COUNT(1)
		FROM acc_accounts a
		WHERE ISNULL(a.is_active, 1) = 1
		  AND (
				UPPER(ISNULL(a.name, '')) LIKE '%RETAINED EARNING%'
				OR UPPER(ISNULL(a.code, '')) IN ('RETAINED-EARNINGS', 'RETAINED_EARNINGS', 'RETAINED EARNINGS')
			  )
	), 0);

	INSERT INTO @result(check_key, check_name, is_passed, failed_count, details)
	VALUES
	('period_count', 'Exactly 12 periods exist for closing year', CASE WHEN @period_count = 12 THEN 1 ELSE 0 END, CASE WHEN @period_count = 12 THEN 0 ELSE ABS(12 - @period_count) END, CONCAT('Found periods: ', @period_count)),
	('period_soft_closed', 'All periods are Soft-Closed', CASE WHEN @open_or_not_soft = 0 THEN 1 ELSE 0 END, @open_or_not_soft, 'Close all open or non-soft-closed periods before year-end close.'),
	('draft_vouchers', 'No draft or unposted vouchers in closing year', CASE WHEN @draft_count = 0 THEN 1 ELSE 0 END, @draft_count, 'Post or delete all draft/unposted vouchers.'),
	('voucher_balance', 'All vouchers are balanced', CASE WHEN @out_of_balance_count = 0 THEN 1 ELSE 0 END, @out_of_balance_count, 'Fix out-of-balance vouchers first.'),
	('income_summary_account', 'Income Summary account exists', CASE WHEN @income_summary_count > 0 THEN 1 ELSE 0 END, CASE WHEN @income_summary_count > 0 THEN 0 ELSE 1 END, 'Create an active account named Income Summary.'),
	('retained_earnings_account', 'Retained Earnings account exists', CASE WHEN @retained_earnings_count > 0 THEN 1 ELSE 0 END, CASE WHEN @retained_earnings_count > 0 THEN 0 ELSE 1 END, 'Create an active account named Retained Earnings.');

	SELECT *
	FROM @result
	ORDER BY check_key;
END
GO

IF OBJECT_ID('dbo.sp_YearEndClose', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_YearEndClose;
GO

CREATE PROCEDURE dbo.sp_YearEndClose
	@year_id INT,
	@user_id INT,
	@branch_id INT = NULL,
	@income_summary_account_id INT = NULL,
	@retained_earnings_account_id INT = NULL,
	@is_success BIT OUTPUT,
	@result_message NVARCHAR(500) OUTPUT,
	@run_id INT OUTPUT,
	@net_profit_loss DECIMAL(18,2) OUTPUT,
	@closing_voucher_no NVARCHAR(100) OUTPUT,
	@opening_voucher_no NVARCHAR(100) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @year_start DATE;
	DECLARE @year_end DATE;
	DECLARE @new_year_id INT;
	DECLARE @new_year_start DATE;
	DECLARE @new_year_end DATE;
	DECLARE @first_new_period_id INT;
	DECLARE @pre_snapshot_json NVARCHAR(MAX);
	DECLARE @post_snapshot_json NVARCHAR(MAX);
	DECLARE @pre_close_report_json NVARCHAR(MAX);
	DECLARE @period_soft_close_issues INT;
	DECLARE @period_count INT;
	DECLARE @check_failed_count INT;
	DECLARE @branch_id_effective INT;
	DECLARE @income_close_total DECIMAL(18,2) = 0;
	DECLARE @expense_close_total DECIMAL(18,2) = 0;
	DECLARE @close_date DATE;
	DECLARE @closing_header_id INT = NULL;
	DECLARE @opening_header_id INT = NULL;
	DECLARE @nowSuffix NVARCHAR(14) = REPLACE(CONVERT(NVARCHAR(19), GETDATE(), 120), '-', '');
	SET @nowSuffix = REPLACE(@nowSuffix, ':', '');
	SET @nowSuffix = REPLACE(@nowSuffix, ' ', '');

	SET @is_success = 0;
	SET @result_message = N'';
	SET @run_id = 0;
	SET @net_profit_loss = 0;
	SET @closing_voucher_no = NULL;
	SET @opening_voucher_no = NULL;

	SELECT
		@year_start = CAST(from_date AS DATE),
		@year_end = CAST(to_date AS DATE)
	FROM acc_fiscal_years
	WHERE id = @year_id;

	IF @year_start IS NULL OR @year_end IS NULL
	BEGIN
		SET @result_message = N'Closing year does not exist.';
		RETURN;
	END

	SELECT TOP 1
		@new_year_id = id,
		@new_year_start = CAST(from_date AS DATE),
		@new_year_end = CAST(to_date AS DATE)
	FROM acc_fiscal_years
	WHERE CAST(from_date AS DATE) = DATEADD(DAY, 1, @year_end)
	ORDER BY id;

	IF @new_year_id IS NULL
	BEGIN
		SELECT TOP 1
			@new_year_id = id,
			@new_year_start = CAST(from_date AS DATE),
			@new_year_end = CAST(to_date AS DATE)
		FROM acc_fiscal_years
		WHERE CAST(from_date AS DATE) > @year_end
		ORDER BY from_date;
	END

	IF @new_year_id IS NULL
	BEGIN
		SET @result_message = N'No next financial year found. Create next year before closing.';
		RETURN;
	END

	SELECT @period_count = COUNT(1),
		   @period_soft_close_issues = SUM(CASE WHEN ISNULL(status, 'Open') <> 'Soft-Closed' THEN 1 ELSE 0 END)
	FROM acc_financial_periods
	WHERE year_id = @year_id;

	IF ISNULL(@period_count, 0) <> 12
	BEGIN
		SET @result_message = N'Year-end close requires exactly 12 periods in the closing year.';
		RETURN;
	END

	IF ISNULL(@period_soft_close_issues, 0) > 0
	BEGIN
		SET @result_message = N'All periods must be Soft-Closed before year-end close.';
		RETURN;
	END

	IF EXISTS
	(
		SELECT 1
		FROM dbo.acc_year_end_close_runs r
		WHERE r.year_id = @year_id
		  AND r.status = 'Completed'
		  AND r.rolled_back_at IS NULL
	)
	BEGIN
		SET @result_message = N'Year-end close already completed for this year. Roll back first if needed.';
		RETURN;
	END

	SET @branch_id_effective = @branch_id;
	IF @branch_id_effective IS NULL
	BEGIN
		SELECT TOP 1 @branch_id_effective = ISNULL(u.branch_id, 1)
		FROM pos_users u
		WHERE u.id = @user_id;
	END
	IF @branch_id_effective IS NULL SET @branch_id_effective = 1;

	IF @income_summary_account_id IS NULL
	BEGIN
		SELECT TOP 1 @income_summary_account_id = a.id
		FROM acc_accounts a
		WHERE ISNULL(a.is_active, 1) = 1
		  AND (UPPER(ISNULL(a.name, '')) LIKE '%INCOME SUMMARY%' OR UPPER(ISNULL(a.code, '')) IN ('INCOME-SUMMARY', 'INCOME_SUMMARY', 'INCOME SUMMARY'))
		ORDER BY a.id;
	END

	IF @retained_earnings_account_id IS NULL
	BEGIN
		SELECT TOP 1 @retained_earnings_account_id = a.id
		FROM acc_accounts a
		WHERE ISNULL(a.is_active, 1) = 1
		  AND (UPPER(ISNULL(a.name, '')) LIKE '%RETAINED EARNING%' OR UPPER(ISNULL(a.code, '')) IN ('RETAINED-EARNINGS', 'RETAINED_EARNINGS', 'RETAINED EARNINGS'))
		ORDER BY a.id;
	END

	IF @income_summary_account_id IS NULL OR @retained_earnings_account_id IS NULL
	BEGIN
		SET @result_message = N'Income Summary or Retained Earnings account is missing.';
		RETURN;
	END

	DECLARE @validation TABLE
	(
		check_key NVARCHAR(80),
		check_name NVARCHAR(250),
		is_passed BIT,
		failed_count INT,
		details NVARCHAR(500)
	);

	INSERT INTO @validation(check_key, check_name, is_passed, failed_count, details)
	EXEC dbo.sp_YearEndPreCloseValidation @year_id = @year_id;

	SELECT @check_failed_count = COUNT(1)
	FROM @validation
	WHERE ISNULL(is_passed, 0) = 0;

	SET @pre_close_report_json = (
		SELECT check_key,
			   check_name,
			   is_passed,
			   failed_count,
			   details
		FROM @validation
		FOR JSON PATH
	);

	IF ISNULL(@check_failed_count, 0) > 0
	BEGIN
		SET @result_message = N'Pre-close validation failed. Review validation report.';
		SELECT check_key, check_name, is_passed, failed_count, details FROM @validation ORDER BY check_key;
		RETURN;
	END

	RAISERROR('Step 1/7 - Validation passed.', 10, 1) WITH NOWAIT;

	SET @close_date = @year_end;

	SET @pre_snapshot_json =
	(
		SELECT
			a.id AS account_id,
			a.code AS account_code,
			a.name AS account_name,
			ISNULL(at.name, '') AS account_type,
			CAST(
				ISNULL(a.op_dr_balance, 0) - ISNULL(a.op_cr_balance, 0)
				+ ISNULL((
					SELECT SUM(ISNULL(e.debit, 0) - ISNULL(e.credit, 0))
					FROM acc_entries e
					WHERE e.account_id = a.id
					  AND e.entry_date <= @close_date
				), 0)
			AS DECIMAL(18,2)) AS closing_balance
		FROM acc_accounts a
		LEFT JOIN acc_groups g ON g.id = a.group_id
		LEFT JOIN acc_account_type at ON at.id = g.account_type_id
		WHERE ISNULL(a.is_active, 1) = 1
		ORDER BY a.code, a.name
		FOR JSON PATH
	);

	BEGIN TRANSACTION;
	BEGIN TRY
		INSERT INTO dbo.acc_year_end_close_runs
		(
			year_id,
			new_year_id,
			close_date,
			closed_by,
			branch_id,
			status,
			pre_close_report_json,
			pre_snapshot_json,
			started_at
		)
		VALUES
		(
			@year_id,
			@new_year_id,
			@close_date,
			@user_id,
			@branch_id_effective,
			'InProgress',
			@pre_close_report_json,
			@pre_snapshot_json,
			GETDATE()
		);

		SET @run_id = CAST(SCOPE_IDENTITY() AS INT);

		RAISERROR('Step 2/7 - Calculating net profit/loss.', 10, 1) WITH NOWAIT;

		DECLARE @year_balances TABLE
		(
			account_id INT PRIMARY KEY,
			account_type NVARCHAR(50),
			delta DECIMAL(18,2),
			income_amount DECIMAL(18,2),
			expense_amount DECIMAL(18,2)
		);

		INSERT INTO @year_balances(account_id, account_type, delta, income_amount, expense_amount)
		SELECT
			a.id,
			UPPER(ISNULL(at.name, '')),
			CAST(ISNULL(SUM(ISNULL(e.debit, 0) - ISNULL(e.credit, 0)), 0) AS DECIMAL(18,2)) AS delta,
			CAST(CASE WHEN UPPER(ISNULL(at.name, '')) IN ('INCOME', 'REVENUE')
				THEN ISNULL(SUM(ISNULL(e.credit, 0) - ISNULL(e.debit, 0)), 0)
				ELSE 0 END AS DECIMAL(18,2)) AS income_amount,
			CAST(CASE WHEN UPPER(ISNULL(at.name, '')) IN ('EXPENSE', 'EXPENSES', 'COST', 'COGS')
				THEN ISNULL(SUM(ISNULL(e.debit, 0) - ISNULL(e.credit, 0)), 0)
				ELSE 0 END AS DECIMAL(18,2)) AS expense_amount
		FROM acc_accounts a
		LEFT JOIN acc_groups g ON g.id = a.group_id
		LEFT JOIN acc_account_type at ON at.id = g.account_type_id
		LEFT JOIN acc_entries e
			ON e.account_id = a.id
		   AND e.entry_date >= @year_start
		   AND e.entry_date < DATEADD(DAY, 1, @year_end)
		WHERE ISNULL(a.is_active, 1) = 1
		GROUP BY a.id, at.name;

		SELECT @income_close_total = ISNULL(SUM(CASE WHEN income_amount > 0 THEN income_amount ELSE 0 END), 0),
			   @expense_close_total = ISNULL(SUM(CASE WHEN expense_amount > 0 THEN expense_amount ELSE 0 END), 0)
		FROM @year_balances;

		SET @net_profit_loss = CAST(@income_close_total - @expense_close_total AS DECIMAL(18,2));

		RAISERROR('Step 3/7 - Posting closing entries.', 10, 1) WITH NOWAIT;

		DECLARE @closing_lines TABLE
		(
			line_no INT IDENTITY(1,1) PRIMARY KEY,
			account_id INT,
			debit DECIMAL(18,2),
			credit DECIMAL(18,2),
			description NVARCHAR(400)
		);

		INSERT INTO @closing_lines(account_id, debit, credit, description)
		SELECT y.account_id,
			   y.income_amount,
			   0,
			   N'Year-End Close Income Account'
		FROM @year_balances y
		WHERE y.income_amount > 0;

		IF @income_close_total > 0
		BEGIN
			INSERT INTO @closing_lines(account_id, debit, credit, description)
			VALUES(@income_summary_account_id, 0, @income_close_total, N'Year-End Close Income to Income Summary');
		END

		INSERT INTO @closing_lines(account_id, debit, credit, description)
		SELECT y.account_id,
			   0,
			   y.expense_amount,
			   N'Year-End Close Expense Account'
		FROM @year_balances y
		WHERE y.expense_amount > 0;

		IF @expense_close_total > 0
		BEGIN
			INSERT INTO @closing_lines(account_id, debit, credit, description)
			VALUES(@income_summary_account_id, @expense_close_total, 0, N'Year-End Close Expense to Income Summary');
		END

		IF @net_profit_loss > 0
		BEGIN
			INSERT INTO @closing_lines(account_id, debit, credit, description)
			VALUES(@income_summary_account_id, @net_profit_loss, 0, N'Close Income Summary (Profit)');

			INSERT INTO @closing_lines(account_id, debit, credit, description)
			VALUES(@retained_earnings_account_id, 0, @net_profit_loss, N'Transfer Profit to Retained Earnings');
		END
		ELSE IF @net_profit_loss < 0
		BEGIN
			INSERT INTO @closing_lines(account_id, debit, credit, description)
			VALUES(@income_summary_account_id, 0, ABS(@net_profit_loss), N'Close Income Summary (Loss)');

			INSERT INTO @closing_lines(account_id, debit, credit, description)
			VALUES(@retained_earnings_account_id, ABS(@net_profit_loss), 0, N'Transfer Loss to Retained Earnings');
		END

		IF EXISTS
		(
			SELECT 1
			FROM @closing_lines
			GROUP BY account_id
			HAVING ABS(SUM(ISNULL(debit, 0)) - SUM(ISNULL(credit, 0))) < 0.009
		)
		BEGIN
			DELETE l
			FROM @closing_lines l
			WHERE l.account_id IN
			(
				SELECT account_id
				FROM @closing_lines
				GROUP BY account_id
				HAVING ABS(SUM(ISNULL(debit, 0)) - SUM(ISNULL(credit, 0))) < 0.009
			);
		END

		IF EXISTS(SELECT 1 FROM @closing_lines)
		BEGIN
			DECLARE @closing_total_debit DECIMAL(18,2) = ISNULL((SELECT SUM(ISNULL(debit, 0)) FROM @closing_lines), 0);
			DECLARE @closing_total_credit DECIMAL(18,2) = ISNULL((SELECT SUM(ISNULL(credit, 0)) FROM @closing_lines), 0);

			IF ABS(@closing_total_debit - @closing_total_credit) > 0.009
			BEGIN
				RAISERROR('Closing entry is out of balance.', 16, 1);
			END

			SET @closing_voucher_no = CONCAT('YEC-', @year_id, '-', @nowSuffix, '-', RIGHT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), '-', ''), 6));

			INSERT INTO acc_entries_header
			(
				InvoiceNo,
				EntryDate,
				VoucherType,
				ReferenceNo,
				Narration,
				total_debit,
				total_credit,
				status,
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
				@closing_voucher_no,
				@close_date,
				'Closing Entry',
				CONCAT('YE-', @year_id),
				CONCAT('Year-end closing voucher for year ', @year_id),
				@closing_total_debit,
				@closing_total_credit,
				'Posted',
				@user_id,
				GETDATE(),
				1,
				GETDATE(),
				GETDATE(),
				@user_id,
				@branch_id_effective
			);

			SET @closing_header_id = CAST(SCOPE_IDENTITY() AS INT);

			INSERT INTO acc_entries
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
				entry_id,
				payment_ref_invoice_no
			)
			SELECT
				@closing_voucher_no,
				l.account_id,
				@close_date,
				l.debit,
				l.credit,
				l.description,
				@user_id,
				@branch_id_effective,
				GETDATE(),
				@closing_header_id,
				CONCAT('YE-', @year_id)
			FROM @closing_lines l;
		END

		RAISERROR('Step 4/7 - Verifying income and expense balances are zero.', 10, 1) WITH NOWAIT;

		IF EXISTS
		(
			SELECT 1
			FROM
			(
				SELECT a.id,
					   SUM(ISNULL(e.debit, 0) - ISNULL(e.credit, 0)) AS delta
				FROM acc_accounts a
				LEFT JOIN acc_groups g ON g.id = a.group_id
				LEFT JOIN acc_account_type at ON at.id = g.account_type_id
				LEFT JOIN acc_entries e
					ON e.account_id = a.id
				   AND e.entry_date >= @year_start
				   AND e.entry_date < DATEADD(DAY, 1, @year_end)
				WHERE UPPER(ISNULL(at.name, '')) IN ('INCOME', 'REVENUE', 'EXPENSE', 'EXPENSES', 'COST', 'COGS')
				GROUP BY a.id
			) z
			WHERE ABS(ISNULL(z.delta, 0)) > 0.009
		)
		BEGIN
			RAISERROR('Post-close verification failed. Income/Expense account(s) are not zero.', 16, 1);
		END

		RAISERROR('Step 5/7 - Posting opening entries for new year.', 10, 1) WITH NOWAIT;

		DECLARE @opening_lines TABLE
		(
			line_no INT IDENTITY(1,1) PRIMARY KEY,
			account_id INT,
			debit DECIMAL(18,2),
			credit DECIMAL(18,2),
			description NVARCHAR(400)
		);

		INSERT INTO @opening_lines(account_id, debit, credit, description)
		SELECT
			a.id,
			CASE
				WHEN UPPER(ISNULL(at.name, '')) IN ('ASSET', 'ASSETS') AND bal.closing_balance >= 0 THEN bal.closing_balance
				WHEN UPPER(ISNULL(at.name, '')) IN ('LIABILITY', 'LIABILITIES', 'EQUITY') AND bal.closing_balance < 0 THEN ABS(bal.closing_balance)
				ELSE 0
			END AS debit,
			CASE
				WHEN UPPER(ISNULL(at.name, '')) IN ('ASSET', 'ASSETS') AND bal.closing_balance < 0 THEN ABS(bal.closing_balance)
				WHEN UPPER(ISNULL(at.name, '')) IN ('LIABILITY', 'LIABILITIES', 'EQUITY') AND bal.closing_balance >= 0 THEN bal.closing_balance
				ELSE 0
			END AS credit,
			N'Opening Balance Carry Forward'
		FROM acc_accounts a
		LEFT JOIN acc_groups g ON g.id = a.group_id
		LEFT JOIN acc_account_type at ON at.id = g.account_type_id
		CROSS APPLY
		(
			SELECT CAST(
				ISNULL(a.op_dr_balance, 0) - ISNULL(a.op_cr_balance, 0)
				+ ISNULL((
					SELECT SUM(ISNULL(e.debit, 0) - ISNULL(e.credit, 0))
					FROM acc_entries e
					WHERE e.account_id = a.id
					  AND e.entry_date <= @close_date
				), 0)
			AS DECIMAL(18,2)) AS closing_balance
		) bal
		WHERE ISNULL(a.is_active, 1) = 1
		  AND UPPER(ISNULL(at.name, '')) IN ('ASSET', 'ASSETS', 'LIABILITY', 'LIABILITIES', 'EQUITY')
		  AND ABS(ISNULL(bal.closing_balance, 0)) > 0.009;

		IF EXISTS(SELECT 1 FROM @opening_lines)
		BEGIN
			DECLARE @opening_total_debit DECIMAL(18,2) = ISNULL((SELECT SUM(ISNULL(debit, 0)) FROM @opening_lines), 0);
			DECLARE @opening_total_credit DECIMAL(18,2) = ISNULL((SELECT SUM(ISNULL(credit, 0)) FROM @opening_lines), 0);

			IF ABS(@opening_total_debit - @opening_total_credit) > 0.009
			BEGIN
				RAISERROR('Opening entry is out of balance.', 16, 1);
			END

			SET @opening_voucher_no = CONCAT('YEO-', @new_year_id, '-', @nowSuffix, '-', RIGHT(REPLACE(CONVERT(NVARCHAR(36), NEWID()), '-', ''), 6));

			INSERT INTO acc_entries_header
			(
				InvoiceNo,
				EntryDate,
				VoucherType,
				ReferenceNo,
				Narration,
				total_debit,
				total_credit,
				status,
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
				@opening_voucher_no,
				@new_year_start,
				'Opening Entry',
				CONCAT('OPEN-', @new_year_id),
				CONCAT('Opening balances seeded from year ', @year_id),
				@opening_total_debit,
				@opening_total_credit,
				'Posted',
				@user_id,
				GETDATE(),
				1,
				GETDATE(),
				GETDATE(),
				@user_id,
				@branch_id_effective
			);

			SET @opening_header_id = CAST(SCOPE_IDENTITY() AS INT);

			INSERT INTO acc_entries
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
				entry_id,
				payment_ref_invoice_no
			)
			SELECT
				@opening_voucher_no,
				l.account_id,
				@new_year_start,
				l.debit,
				l.credit,
				l.description,
				@user_id,
				@branch_id_effective,
				GETDATE(),
				@opening_header_id,
				CONCAT('OPEN-', @new_year_id)
			FROM @opening_lines l;
		END

		RAISERROR('Step 6/7 - Hard locking old year periods.', 10, 1) WITH NOWAIT;

		UPDATE acc_financial_periods
		SET status = 'Hard-Locked',
			closed_by = CAST(@user_id AS NVARCHAR(50)),
			closed_at = GETDATE()
		WHERE year_id = @year_id;

		RAISERROR('Step 7/7 - Opening period 1 of new financial year.', 10, 1) WITH NOWAIT;

		SELECT TOP 1 @first_new_period_id = period_id
		FROM acc_financial_periods
		WHERE year_id = @new_year_id
		ORDER BY start_date;

		IF @first_new_period_id IS NULL
		BEGIN
			INSERT INTO acc_financial_periods(year_id, period_name, start_date, end_date, status, closed_by, closed_at)
			VALUES
			(
				@new_year_id,
				DATENAME(MONTH, @new_year_start) + ' ' + CAST(YEAR(@new_year_start) AS NVARCHAR(4)),
				@new_year_start,
				EOMONTH(@new_year_start),
				'Open',
				NULL,
				NULL
			);
		END
		ELSE
		BEGIN
			UPDATE acc_financial_periods
			SET status = 'Open',
				closed_by = NULL,
				closed_at = NULL
			WHERE period_id = @first_new_period_id;
		END

		SET @post_snapshot_json =
		(
			SELECT
				a.id AS account_id,
				a.code AS account_code,
				a.name AS account_name,
				ISNULL(at.name, '') AS account_type,
				CAST(
					ISNULL(a.op_dr_balance, 0) - ISNULL(a.op_cr_balance, 0)
					+ ISNULL((
						SELECT SUM(ISNULL(e.debit, 0) - ISNULL(e.credit, 0))
						FROM acc_entries e
						WHERE e.account_id = a.id
						  AND e.entry_date <= @close_date
					), 0)
				AS DECIMAL(18,2)) AS closing_balance
			FROM acc_accounts a
			LEFT JOIN acc_groups g ON g.id = a.group_id
			LEFT JOIN acc_account_type at ON at.id = g.account_type_id
			WHERE ISNULL(a.is_active, 1) = 1
			ORDER BY a.code, a.name
			FOR JSON PATH
		);

		UPDATE dbo.acc_year_end_close_runs
		SET status = 'Completed',
			net_profit_loss = @net_profit_loss,
			closing_voucher_no = @closing_voucher_no,
			opening_voucher_no = @opening_voucher_no,
			post_snapshot_json = @post_snapshot_json,
			completed_at = GETDATE()
		WHERE run_id = @run_id;

		COMMIT TRANSACTION;

		SET @is_success = 1;
		SET @result_message = N'Year-end close completed successfully.';

		SELECT check_key, check_name, is_passed, failed_count, details FROM @validation ORDER BY check_key;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;

		IF @run_id > 0
		BEGIN
			UPDATE dbo.acc_year_end_close_runs
			SET status = 'Failed',
				completed_at = GETDATE(),
				rollback_reason = LEFT(ERROR_MESSAGE(), 500)
			WHERE run_id = @run_id;
		END

		SET @is_success = 0;
		SET @result_message = CONCAT('Year-end close failed: ', ERROR_MESSAGE());
		THROW;
	END CATCH
END
GO

IF OBJECT_ID('dbo.sp_RollbackYearEndClose', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_RollbackYearEndClose;
GO

CREATE PROCEDURE dbo.sp_RollbackYearEndClose
	@year_id INT,
	@user_id INT,
	@reason NVARCHAR(500) = NULL,
	@is_success BIT OUTPUT,
	@result_message NVARCHAR(500) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @run_id INT;
	DECLARE @new_year_id INT;
	DECLARE @closing_voucher_no NVARCHAR(100);
	DECLARE @opening_voucher_no NVARCHAR(100);
	DECLARE @completed_at DATETIME;

	SET @is_success = 0;
	SET @result_message = N'';

	SELECT TOP 1
		@run_id = run_id,
		@new_year_id = new_year_id,
		@closing_voucher_no = closing_voucher_no,
		@opening_voucher_no = opening_voucher_no,
		@completed_at = completed_at
	FROM dbo.acc_year_end_close_runs
	WHERE year_id = @year_id
	  AND status = 'Completed'
	  AND rolled_back_at IS NULL
	ORDER BY run_id DESC;

	IF @run_id IS NULL
	BEGIN
		SET @result_message = N'No completed year-end close was found for rollback.';
		RETURN;
	END

	IF DATEDIFF(DAY, @completed_at, GETDATE()) > 7
	BEGIN
		SET @result_message = N'Rollback window expired. Rollback is allowed only within 7 days.';
		RETURN;
	END

	BEGIN TRANSACTION;
	BEGIN TRY
		IF ISNULL(@closing_voucher_no, '') <> ''
		BEGIN
			DELETE FROM acc_entries WHERE invoice_no = @closing_voucher_no;
			DELETE FROM acc_entries_header WHERE InvoiceNo = @closing_voucher_no AND VoucherType = 'Closing Entry';
		END

		IF ISNULL(@opening_voucher_no, '') <> ''
		BEGIN
			DELETE FROM acc_entries WHERE invoice_no = @opening_voucher_no;
			DELETE FROM acc_entries_header WHERE InvoiceNo = @opening_voucher_no AND VoucherType = 'Opening Entry';
		END

		UPDATE acc_financial_periods
		SET status = 'Soft-Closed',
			closed_by = CAST(@user_id AS NVARCHAR(50)),
			closed_at = GETDATE()
		WHERE year_id = @year_id;

		UPDATE acc_financial_periods
		SET status = 'Open',
			closed_by = NULL,
			closed_at = NULL
		WHERE period_id =
		(
			SELECT TOP 1 period_id
			FROM acc_financial_periods
			WHERE year_id = @new_year_id
			ORDER BY start_date
		);

		UPDATE dbo.acc_year_end_close_runs
		SET status = 'RolledBack',
			rolled_back_at = GETDATE(),
			rolled_back_by = @user_id,
			rollback_reason = LEFT(ISNULL(@reason, 'Rollback requested by admin.'), 500)
		WHERE run_id = @run_id;

		COMMIT TRANSACTION;
		SET @is_success = 1;
		SET @result_message = N'Year-end close rollback completed successfully.';
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;

		SET @is_success = 0;
		SET @result_message = CONCAT('Rollback failed: ', ERROR_MESSAGE());
		THROW;
	END CATCH
END
GO
