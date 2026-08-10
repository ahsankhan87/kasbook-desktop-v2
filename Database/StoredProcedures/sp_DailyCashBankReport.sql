-- Stored Procedure: sp_DailyCashBankReport
-- Purpose: Daily Cash and Bank Opening/Closing Report with separate Cash and Bank columns
-- Returns: (1) Consolidated daily totals, (2) By-account daily balances
-- Supports: Single day or date range, branch filtering

IF OBJECT_ID(N'dbo.sp_DailyCashBankReport', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_DailyCashBankReport;
GO

CREATE PROCEDURE dbo.sp_DailyCashBankReport
	@FromDate DATE = NULL,
	@ToDate DATE = NULL,
	@BranchId INT = NULL,
	@AccountId INT = NULL  -- Optional filter for specific cash/bank account
AS
BEGIN
	SET NOCOUNT ON;

	-- Default to today if dates not specified
	IF @ToDate IS NULL
		SET @ToDate = CAST(GETDATE() AS DATE);

	IF @FromDate IS NULL
		SET @FromDate = @ToDate;

	-- Ensure FromDate <= ToDate
	IF @FromDate > @ToDate
	BEGIN
		DECLARE @temp DATE = @FromDate;
		SET @FromDate = @ToDate;
		SET @ToDate = @temp;
	END

	-- Calculate opening balances for Cash and Bank as of day before FromDate
	DECLARE @CashOpeningBalance DECIMAL(18, 2) = 0;
	DECLARE @BankOpeningBalance DECIMAL(18, 2) = 0;

	-- Cash opening balance
	SELECT @CashOpeningBalance = ISNULL(SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)), 0)
	FROM acc_entries E
	INNER JOIN acc_accounts A ON A.id = E.account_id
	WHERE E.entry_date < @FromDate
	  AND A.is_cash = 1
	  AND (E.branch_id = @BranchId OR @BranchId IS NULL)
	  AND (@AccountId IS NULL OR A.id = @AccountId);

	-- Bank opening balance
	SELECT @BankOpeningBalance = ISNULL(SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)), 0)
	FROM acc_entries E
	INNER JOIN acc_accounts A ON A.id = E.account_id
	WHERE E.entry_date < @FromDate
	  AND A.is_bank = 1
	  AND (E.branch_id = @BranchId OR @BranchId IS NULL)
	  AND (@AccountId IS NULL OR A.id = @AccountId);

	-- Result Set 1: Consolidated Daily Totals (one row per day with Cash and Bank columns)
	;WITH DailyMovements AS (
		SELECT
			CAST(E.entry_date AS DATE) AS transaction_day,
			-- Cash columns
			SUM(CASE WHEN A.is_cash = 1 AND E.debit > 0 THEN E.debit ELSE 0 END) AS cash_receipts,
			SUM(CASE WHEN A.is_cash = 1 AND E.credit > 0 THEN E.credit ELSE 0 END) AS cash_payments,
			SUM(CASE WHEN A.is_cash = 1 THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0) ELSE 0 END) AS cash_net_movement,
			-- Bank columns
			SUM(CASE WHEN A.is_bank = 1 AND E.debit > 0 THEN E.debit ELSE 0 END) AS bank_receipts,
			SUM(CASE WHEN A.is_bank = 1 AND E.credit > 0 THEN E.credit ELSE 0 END) AS bank_payments,
			SUM(CASE WHEN A.is_bank = 1 THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0) ELSE 0 END) AS bank_net_movement
		FROM acc_entries E
		INNER JOIN acc_accounts A ON A.id = E.account_id
		WHERE E.entry_date BETWEEN @FromDate AND @ToDate
		  AND (A.is_cash = 1 OR A.is_bank = 1)
		  AND (E.branch_id = @BranchId OR @BranchId IS NULL)
		  AND (@AccountId IS NULL OR A.id = @AccountId)
		GROUP BY CAST(E.entry_date AS DATE)
	),
	RunningBalances AS (
		SELECT
			transaction_day,
			cash_receipts,
			cash_payments,
			cash_net_movement,
			bank_receipts,
			bank_payments,
			bank_net_movement,
			-- Running cash balance
			@CashOpeningBalance + SUM(cash_net_movement) OVER (ORDER BY transaction_day ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS cash_closing_balance,
			-- Running bank balance
			@BankOpeningBalance + SUM(bank_net_movement) OVER (ORDER BY transaction_day ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS bank_closing_balance,
			-- Opening balances for each day
			CASE 
				WHEN ROW_NUMBER() OVER (ORDER BY transaction_day) = 1 
				THEN @CashOpeningBalance
				ELSE @CashOpeningBalance + SUM(cash_net_movement) OVER (ORDER BY transaction_day ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING)
			END AS cash_opening_balance,
			CASE 
				WHEN ROW_NUMBER() OVER (ORDER BY transaction_day) = 1 
				THEN @BankOpeningBalance
				ELSE @BankOpeningBalance + SUM(bank_net_movement) OVER (ORDER BY transaction_day ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING)
			END AS bank_opening_balance
		FROM DailyMovements
	)
	SELECT
		transaction_day,
		-- Cash columns
		cash_opening_balance,
		cash_receipts,
		cash_payments,
		cash_closing_balance,
		-- Bank columns
		bank_opening_balance,
		bank_receipts,
		bank_payments,
		bank_closing_balance,
		-- Combined totals
		cash_opening_balance + bank_opening_balance AS total_opening_balance,
		cash_receipts + bank_receipts AS total_receipts,
		cash_payments + bank_payments AS total_payments,
		cash_closing_balance + bank_closing_balance AS total_closing_balance
	FROM RunningBalances
	ORDER BY transaction_day;

	-- Result Set 2: By-Account Daily Balances (detailed view per account per day)
	;WITH AccountOpenings AS (
		SELECT
			A.id AS account_id,
			A.code AS account_code,
			A.name AS account_name,
			CASE WHEN A.is_cash = 1 THEN 'Cash' WHEN A.is_bank = 1 THEN 'Bank' ELSE 'Other' END AS account_type,
			ISNULL(SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)), 0) AS opening_balance
		FROM acc_accounts A
		LEFT JOIN acc_entries E ON E.account_id = A.id AND E.entry_date < @FromDate
			AND (E.branch_id = @BranchId OR @BranchId IS NULL)
		WHERE (A.is_cash = 1 OR A.is_bank = 1)
		  AND (ISNULL(A.is_active, 1) = 1)
		  AND (@AccountId IS NULL OR A.id = @AccountId)
		GROUP BY A.id, A.code, A.name, A.is_cash, A.is_bank
	),
	DailyAccountMovements AS (
		SELECT
			CAST(E.entry_date AS DATE) AS transaction_day,
			A.id AS account_id,
			A.code AS account_code,
			A.name AS account_name,
			CASE WHEN A.is_cash = 1 THEN 'Cash' WHEN A.is_bank = 1 THEN 'Bank' ELSE 'Other' END AS account_type,
			SUM(CASE WHEN E.debit > 0 THEN E.debit ELSE 0 END) AS receipts,
			SUM(CASE WHEN E.credit > 0 THEN E.credit ELSE 0 END) AS payments,
			SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)) AS net_movement
		FROM acc_entries E
		INNER JOIN acc_accounts A ON A.id = E.account_id
		WHERE E.entry_date BETWEEN @FromDate AND @ToDate
		  AND (A.is_cash = 1 OR A.is_bank = 1)
		  AND (E.branch_id = @BranchId OR @BranchId IS NULL)
		  AND (@AccountId IS NULL OR A.id = @AccountId)
		GROUP BY CAST(E.entry_date AS DATE), A.id, A.code, A.name, A.is_cash, A.is_bank
	)
	SELECT
		D.transaction_day,
		D.account_id,
		D.account_code,
		D.account_name,
		D.account_type,
		ISNULL(O.opening_balance, 0) + 
			ISNULL((
				SELECT SUM(net_movement) 
				FROM DailyAccountMovements D2 
				WHERE D2.account_id = D.account_id 
				  AND D2.transaction_day < D.transaction_day
			), 0) AS opening_balance,
		D.receipts,
		D.payments,
		ISNULL(O.opening_balance, 0) + 
			ISNULL((
				SELECT SUM(net_movement) 
				FROM DailyAccountMovements D2 
				WHERE D2.account_id = D.account_id 
				  AND D2.transaction_day <= D.transaction_day
			), 0) AS closing_balance
	FROM DailyAccountMovements D
	LEFT JOIN AccountOpenings O ON O.account_id = D.account_id
	ORDER BY D.transaction_day, D.account_type, D.account_code;

	-- Result Set 3: Summary Statistics
	SELECT
		@FromDate AS from_date,
		@ToDate AS to_date,
		@CashOpeningBalance AS cash_opening_balance,
		@BankOpeningBalance AS bank_opening_balance,
		@CashOpeningBalance + @BankOpeningBalance AS total_opening_balance,
		(
			SELECT ISNULL(SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)), 0)
			FROM acc_entries E
			INNER JOIN acc_accounts A ON A.id = E.account_id
			WHERE E.entry_date <= @ToDate
			  AND A.is_cash = 1
			  AND (E.branch_id = @BranchId OR @BranchId IS NULL)
			  AND (@AccountId IS NULL OR A.id = @AccountId)
		) AS cash_closing_balance,
		(
			SELECT ISNULL(SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)), 0)
			FROM acc_entries E
			INNER JOIN acc_accounts A ON A.id = E.account_id
			WHERE E.entry_date <= @ToDate
			  AND A.is_bank = 1
			  AND (E.branch_id = @BranchId OR @BranchId IS NULL)
			  AND (@AccountId IS NULL OR A.id = @AccountId)
		) AS bank_closing_balance,
		(
			SELECT ISNULL(SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)), 0)
			FROM acc_entries E
			INNER JOIN acc_accounts A ON A.id = E.account_id
			WHERE E.entry_date <= @ToDate
			  AND (A.is_cash = 1 OR A.is_bank = 1)
			  AND (E.branch_id = @BranchId OR @BranchId IS NULL)
			  AND (@AccountId IS NULL OR A.id = @AccountId)
		) AS total_closing_balance;
END
GO
