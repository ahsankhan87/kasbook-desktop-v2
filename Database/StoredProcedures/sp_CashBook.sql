IF OBJECT_ID(N'dbo.sp_CashBook', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_CashBook;
GO

CREATE PROCEDURE dbo.sp_CashBook
	@CashAccountId INT = NULL,
	@FromDate DATE = NULL,
	@ToDate DATE = NULL,
	@BranchId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- Default to today if @ToDate is not specified
	IF @ToDate IS NULL
		SET @ToDate = CAST(GETDATE() AS DATE);

	-- Default to 1 year ago if @FromDate is not specified
	IF @FromDate IS NULL
		SET @FromDate = DATEADD(YEAR, -1, @ToDate);

	-- Calculate opening balance (sum of all cash transactions before @FromDate)
	DECLARE @OpeningBalance DECIMAL(18, 2) = 0;
	SELECT @OpeningBalance = ISNULL(SUM(CASE WHEN is_receipt = 1 THEN amount ELSE -amount END), 0)
	FROM vw_cashbook_entries
	WHERE transaction_date < @FromDate
	  AND (cash_account_id_actual = @CashAccountId OR @CashAccountId IS NULL)
	  AND (branch_id = @BranchId OR @BranchId IS NULL);

	-- Main result set with running balance
	SELECT
		ROW_NUMBER() OVER (PARTITION BY transaction_date ORDER BY entry_id) AS daily_sequence,
		transaction_date,
		CAST(transaction_date AS DATE) AS transaction_day,
		transaction_type,
		reference_no,
		description,
		is_receipt,
		amount,
		CASE 
			WHEN ROW_NUMBER() OVER (ORDER BY transaction_date, entry_id) = 1 
			THEN @OpeningBalance + (CASE WHEN is_receipt = 1 THEN amount ELSE -amount END)
			ELSE @OpeningBalance + SUM(CASE WHEN is_receipt = 1 THEN amount ELSE -amount END) 
				 OVER (ORDER BY transaction_date, entry_id ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)
		END AS running_balance,
		cash_account_id_actual,
		cash_account_name,
		branch_id
	FROM vw_cashbook_entries
	WHERE transaction_date BETWEEN @FromDate AND @ToDate
	  AND (cash_account_id_actual = @CashAccountId OR @CashAccountId IS NULL)
	  AND (branch_id = @BranchId OR @BranchId IS NULL)
	ORDER BY transaction_date, entry_id;

	-- Daily totals with opening/closing balances
	;WITH daily_summary AS (
		SELECT
			CAST(transaction_date AS DATE) AS transaction_day,
			SUM(CASE WHEN is_receipt = 1 THEN amount ELSE 0 END) AS receipts_total,
			SUM(CASE WHEN is_receipt = 0 THEN amount ELSE 0 END) AS payments_total,
			SUM(CASE WHEN is_receipt = 1 THEN amount ELSE -amount END) AS daily_balance
		FROM vw_cashbook_entries
		WHERE transaction_date BETWEEN @FromDate AND @ToDate
		  AND (cash_account_id_actual = @CashAccountId OR @CashAccountId IS NULL)
		  AND (branch_id = @BranchId OR @BranchId IS NULL)
		GROUP BY CAST(transaction_date AS DATE)
	)
	SELECT
		transaction_day,
		receipts_total,
		payments_total,
		daily_balance,
		ISNULL(LAG(running_sum) OVER (ORDER BY transaction_day), @OpeningBalance) AS opening_balance_for_day,
		@OpeningBalance + running_sum AS closing_balance_for_day
	FROM (
		SELECT
			transaction_day,
			receipts_total,
			payments_total,
			daily_balance,
			SUM(daily_balance) OVER (ORDER BY transaction_day ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS running_sum
		FROM daily_summary
	) AS balanced_summary
	ORDER BY transaction_day;

	-- Summary statistics
	SELECT
		COUNT(*) AS total_entries,
		SUM(CASE WHEN is_receipt = 1 THEN amount ELSE 0 END) AS total_receipts,
		SUM(CASE WHEN is_receipt = 0 THEN amount ELSE 0 END) AS total_payments,
		@OpeningBalance + SUM(CASE WHEN is_receipt = 1 THEN amount ELSE -amount END) AS closing_balance
	FROM vw_cashbook_entries
	WHERE transaction_date BETWEEN @FromDate AND @ToDate
	  AND (cash_account_id_actual = @CashAccountId OR @CashAccountId IS NULL)
	  AND (branch_id = @BranchId OR @BranchId IS NULL);
END
GO
