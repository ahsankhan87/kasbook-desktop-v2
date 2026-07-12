IF OBJECT_ID(N'dbo.sp_CustomerSubLedger', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_CustomerSubLedger;
GO

CREATE PROCEDURE dbo.sp_CustomerSubLedger
	@CustomerId INT,
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

	-- Calculate opening balance (sum of all transactions before @FromDate)
	DECLARE @OpeningBalance DECIMAL(18, 2) = 0;
	SELECT @OpeningBalance = ISNULL(SUM(CASE WHEN is_debit = 1 THEN -amount ELSE amount END), 0)
	FROM vw_customer_subledger_entries
	WHERE customer_id = @CustomerId 
	  AND transaction_date < @FromDate
	  AND (branch_id = @BranchId OR @BranchId IS NULL);

	-- Main result set with running balance
	SELECT
		ROW_NUMBER() OVER (ORDER BY transaction_date, entry_id) AS sequence,
		transaction_date,
		transaction_type,
		reference_no,
		invoice_no,
		description,
		is_debit,
		amount,
		CASE 
			WHEN ROW_NUMBER() OVER (ORDER BY transaction_date, entry_id) = 1 
			THEN @OpeningBalance + (CASE WHEN is_debit = 1 THEN -amount ELSE amount END)
			ELSE @OpeningBalance + SUM(CASE WHEN is_debit = 1 THEN -amount ELSE amount END) 
				 OVER (ORDER BY transaction_date, entry_id ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)
		END AS running_balance,
		ISNULL(due_date, transaction_date) AS due_date,
		CASE 
			WHEN ISNULL(due_date, transaction_date) < CAST(GETDATE() AS DATE)
			THEN DATEDIFF(DAY, ISNULL(due_date, transaction_date), CAST(GETDATE() AS DATE))
			ELSE 0
		END AS days_overdue,
		status AS invoice_status,
		branch_id
	FROM vw_customer_subledger_entries
	WHERE customer_id = @CustomerId 
	  AND transaction_date BETWEEN @FromDate AND @ToDate
	  AND (branch_id = @BranchId OR @BranchId IS NULL)
	ORDER BY transaction_date, entry_id;

	-- Aging summary
	SELECT
		SUM(CASE WHEN DATEDIFF(DAY, due_date, CAST(GETDATE() AS DATE)) <= 30 THEN running_balance ELSE 0 END) AS bucket_0_30,
		SUM(CASE WHEN DATEDIFF(DAY, due_date, CAST(GETDATE() AS DATE)) > 30 AND DATEDIFF(DAY, due_date, CAST(GETDATE() AS DATE)) <= 60 THEN running_balance ELSE 0 END) AS bucket_31_60,
		SUM(CASE WHEN DATEDIFF(DAY, due_date, CAST(GETDATE() AS DATE)) > 60 AND DATEDIFF(DAY, due_date, CAST(GETDATE() AS DATE)) <= 90 THEN running_balance ELSE 0 END) AS bucket_61_90,
		SUM(CASE WHEN DATEDIFF(DAY, due_date, CAST(GETDATE() AS DATE)) > 90 THEN running_balance ELSE 0 END) AS bucket_90_plus
	FROM (
		SELECT
			ISNULL(due_date, transaction_date) AS due_date,
			CASE 
				WHEN ROW_NUMBER() OVER (ORDER BY transaction_date, entry_id) = 1 
				THEN @OpeningBalance + (CASE WHEN is_debit = 1 THEN -amount ELSE amount END)
				ELSE @OpeningBalance + SUM(CASE WHEN is_debit = 1 THEN -amount ELSE amount END) 
					 OVER (ORDER BY transaction_date, entry_id ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)
			END AS running_balance
		FROM vw_customer_subledger_entries
		WHERE customer_id = @CustomerId 
		  AND transaction_date BETWEEN @FromDate AND @ToDate
		  AND (branch_id = @BranchId OR @BranchId IS NULL)
	) aging_data;
END
GO
