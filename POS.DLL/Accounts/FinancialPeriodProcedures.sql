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
