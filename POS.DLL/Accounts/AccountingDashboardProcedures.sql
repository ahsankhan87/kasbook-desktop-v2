IF TYPE_ID(N'dbo.AccountBalanceSummaryRequestType') IS NULL
BEGIN
	CREATE TYPE dbo.AccountBalanceSummaryRequestType AS TABLE
	(
		RequestKey NVARCHAR(80) NOT NULL,
		AccountClass NVARCHAR(20) NOT NULL,
		AmountMode NVARCHAR(30) NOT NULL,
		FromDate DATE NULL,
		ToDateExclusive DATE NOT NULL,
		UseInvoiceGrouping BIT NOT NULL
	);
END
GO

IF OBJECT_ID(N'dbo.sp_AccountBalanceSummary', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_AccountBalanceSummary;
GO

CREATE PROCEDURE dbo.sp_AccountBalanceSummary
	@BranchId INT,
	@Requests dbo.AccountBalanceSummaryRequestType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	;WITH AccountClassify AS
	(
		SELECT
			A.id AS account_id,
			CASE
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%income%'
				  OR LOWER(ISNULL(T.name, '')) LIKE '%revenue%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%income%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%revenue%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%revenue%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%income%'
					THEN 'Revenue'
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%expense%'
					THEN 'Expense'
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%receivable%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%receivable%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%receivable%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%debtor%'
					THEN 'Receivable'
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%payable%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%payable%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%payable%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%creditor%'
					THEN 'Payable'
				WHEN LOWER(ISNULL(A.name, '')) LIKE '%cash%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%bank%'
					THEN 'CashBank'
				ELSE 'Other'
			END AS account_class
		FROM acc_accounts A WITH (NOLOCK)
		LEFT JOIN acc_groups G WITH (NOLOCK) ON G.id = A.group_id
		LEFT JOIN acc_account_type T WITH (NOLOCK) ON T.id = G.account_type_id
	),
	RequestRows AS
	(
		SELECT RequestKey, AccountClass, AmountMode, FromDate, ToDateExclusive, UseInvoiceGrouping
		FROM @Requests
	),
	BaseEntries AS
	(
		SELECT
			R.RequestKey,
			E.invoice_no,
			E.debit,
			E.credit
		FROM RequestRows R
		INNER JOIN acc_entries E WITH (NOLOCK)
			ON E.branch_id = @BranchId
		   AND E.entry_date < R.ToDateExclusive
		   AND (R.FromDate IS NULL OR E.entry_date >= R.FromDate)
		INNER JOIN AccountClassify C
			ON C.account_id = E.account_id
		   AND C.account_class = R.AccountClass
	),
	NonGrouped AS
	(
		SELECT
			R.RequestKey,
			SUM(
				CASE R.AmountMode
					WHEN 'DebitMinusCredit' THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0)
					WHEN 'CreditMinusDebit' THEN ISNULL(E.credit, 0) - ISNULL(E.debit, 0)
					WHEN 'DebitOnly' THEN ISNULL(E.debit, 0)
					WHEN 'CreditOnly' THEN ISNULL(E.credit, 0)
					ELSE ISNULL(E.debit, 0) - ISNULL(E.credit, 0)
				END
			) AS Amount
		FROM RequestRows R
		INNER JOIN BaseEntries E ON E.RequestKey = R.RequestKey
		WHERE ISNULL(R.UseInvoiceGrouping, 0) = 0
		GROUP BY R.RequestKey
	),
	GroupedInvoices AS
	(
		SELECT
			R.RequestKey,
			E.invoice_no,
			SUM(
				CASE R.AmountMode
					WHEN 'DebitMinusCredit' THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0)
					WHEN 'CreditMinusDebit' THEN ISNULL(E.credit, 0) - ISNULL(E.debit, 0)
					WHEN 'DebitOnly' THEN ISNULL(E.debit, 0)
					WHEN 'CreditOnly' THEN ISNULL(E.credit, 0)
					ELSE ISNULL(E.debit, 0) - ISNULL(E.credit, 0)
				END
			) AS InvoiceAmount
		FROM RequestRows R
		INNER JOIN BaseEntries E ON E.RequestKey = R.RequestKey
		WHERE ISNULL(R.UseInvoiceGrouping, 0) = 1
		GROUP BY R.RequestKey, E.invoice_no, R.AmountMode
	)
	SELECT RequestKey, CAST(ISNULL(Amount, 0) AS DECIMAL(19,4)) AS Amount
	FROM NonGrouped
	UNION ALL
	SELECT RequestKey, CAST(ISNULL(SUM(CASE WHEN InvoiceAmount > 0 THEN InvoiceAmount ELSE 0 END), 0) AS DECIMAL(19,4)) AS Amount
	FROM GroupedInvoices
	GROUP BY RequestKey;
END
GO

IF OBJECT_ID(N'dbo.sp_AccountingDashboardKPIs', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_AccountingDashboardKPIs;
GO

CREATE PROCEDURE dbo.sp_AccountingDashboardKPIs
	@BranchId INT,
	@FromDate DATE,
	@ToDate DATE
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ToExclusive DATE = DATEADD(DAY, 1, @ToDate);
	DECLARE @PeriodDays INT = DATEDIFF(DAY, @FromDate, @ToDate) + 1;
	DECLARE @PrevFromDate DATE = DATEADD(DAY, -@PeriodDays, @FromDate);
	DECLARE @PrevToExclusive DATE = @FromDate;

	DECLARE @Requests dbo.AccountBalanceSummaryRequestType;

	INSERT INTO @Requests (RequestKey, AccountClass, AmountMode, FromDate, ToDateExclusive, UseInvoiceGrouping)
	VALUES
		('CashBankBalance', 'CashBank', 'DebitMinusCredit', NULL, @ToExclusive, 0),
		('TotalReceivables', 'Receivable', 'DebitMinusCredit', NULL, @ToExclusive, 1),
		('TotalPayables', 'Payable', 'CreditMinusDebit', NULL, @ToExclusive, 1),
		('RevenueThisPeriod', 'Revenue', 'CreditOnly', @FromDate, @ToExclusive, 0),
		('ExpensesThisPeriod', 'Expense', 'DebitOnly', @FromDate, @ToExclusive, 0),
		('PrevRevenue', 'Revenue', 'CreditOnly', @PrevFromDate, @PrevToExclusive, 0),
		('PrevExpenses', 'Expense', 'DebitOnly', @PrevFromDate, @PrevToExclusive, 0);

	DECLARE @Balances TABLE
	(
		RequestKey NVARCHAR(80) NOT NULL,
		Amount DECIMAL(19,4) NOT NULL
	);

	INSERT INTO @Balances (RequestKey, Amount)
	EXEC dbo.sp_AccountBalanceSummary
		@BranchId = @BranchId,
		@Requests = @Requests;

	DECLARE @CashBankBalance DECIMAL(19,4) = 0;
	DECLARE @TotalReceivables DECIMAL(19,4) = 0;
	DECLARE @TotalPayables DECIMAL(19,4) = 0;
	DECLARE @RevenueThisPeriod DECIMAL(19,4) = 0;
	DECLARE @ExpensesThisPeriod DECIMAL(19,4) = 0;
	DECLARE @PrevRevenue DECIMAL(19,4) = 0;
	DECLARE @PrevExpenses DECIMAL(19,4) = 0;

	SELECT
		@CashBankBalance = SUM(CASE WHEN RequestKey = 'CashBankBalance' THEN Amount ELSE 0 END),
		@TotalReceivables = SUM(CASE WHEN RequestKey = 'TotalReceivables' THEN Amount ELSE 0 END),
		@TotalPayables = SUM(CASE WHEN RequestKey = 'TotalPayables' THEN Amount ELSE 0 END),
		@RevenueThisPeriod = SUM(CASE WHEN RequestKey = 'RevenueThisPeriod' THEN Amount ELSE 0 END),
		@ExpensesThisPeriod = SUM(CASE WHEN RequestKey = 'ExpensesThisPeriod' THEN Amount ELSE 0 END),
		@PrevRevenue = SUM(CASE WHEN RequestKey = 'PrevRevenue' THEN Amount ELSE 0 END),
		@PrevExpenses = SUM(CASE WHEN RequestKey = 'PrevExpenses' THEN Amount ELSE 0 END)
	FROM @Balances;

	DECLARE @NetProfit DECIMAL(19,4) = @RevenueThisPeriod - @ExpensesThisPeriod;
	DECLARE @PrevPeriodNetProfit DECIMAL(19,4) = @PrevRevenue - @PrevExpenses;
	DECLARE @ProfitChangePercent DECIMAL(19,4) = CASE WHEN NULLIF(ABS(@PrevPeriodNetProfit), 0) IS NULL THEN 0 ELSE ((@NetProfit - @PrevPeriodNetProfit) / ABS(@PrevPeriodNetProfit)) * 100 END;

	SELECT
		CAST(ISNULL(@CashBankBalance, 0) AS DECIMAL(19,4)) AS CashBankBalance,
		CAST(ISNULL(@TotalReceivables, 0) AS DECIMAL(19,4)) AS TotalReceivables,
		CAST(ISNULL(@TotalPayables, 0) AS DECIMAL(19,4)) AS TotalPayables,
		CAST(ISNULL(@RevenueThisPeriod, 0) AS DECIMAL(19,4)) AS RevenueThisPeriod,
		CAST(ISNULL(@ExpensesThisPeriod, 0) AS DECIMAL(19,4)) AS ExpensesThisPeriod,
		CAST(ISNULL(@NetProfit, 0) AS DECIMAL(19,4)) AS NetProfit,
		CAST(ISNULL(@PrevPeriodNetProfit, 0) AS DECIMAL(19,4)) AS PrevPeriodNetProfit,
		CAST(ISNULL(@ProfitChangePercent, 0) AS DECIMAL(19,4)) AS ProfitChangePercent;
END
GO

IF OBJECT_ID(N'dbo.sp_MonthlyPLComparison', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_MonthlyPLComparison;
GO

CREATE PROCEDURE dbo.sp_MonthlyPLComparison
	@BranchId INT,
	@Months INT = 6,
	@AsOfDate DATE = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF @Months IS NULL OR @Months < 1
	BEGIN
		SET @Months = 6;
	END

	DECLARE @EndDate DATE = ISNULL(@AsOfDate, CAST(GETDATE() AS DATE));
	DECLARE @StartMonth DATE = DATEADD(MONTH, 1 - @Months, DATEFROMPARTS(YEAR(@EndDate), MONTH(@EndDate), 1));
	DECLARE @EndExclusive DATE = DATEADD(MONTH, 1, DATEFROMPARTS(YEAR(@EndDate), MONTH(@EndDate), 1));

	;WITH AccountClassify AS
	(
		SELECT
			A.id AS account_id,
			CASE
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%income%'
				  OR LOWER(ISNULL(T.name, '')) LIKE '%revenue%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%income%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%revenue%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%revenue%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%income%'
					THEN 'Revenue'
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%expense%'
					THEN 'Expense'
				ELSE 'Other'
			END AS account_class
		FROM acc_accounts A WITH (NOLOCK)
		LEFT JOIN acc_groups G WITH (NOLOCK) ON G.id = A.group_id
		LEFT JOIN acc_account_type T WITH (NOLOCK) ON T.id = G.account_type_id
	),
	Months AS
	(
		SELECT CAST(@StartMonth AS DATE) AS MonthStart, 1 AS Step
		UNION ALL
		SELECT DATEADD(MONTH, 1, MonthStart), Step + 1
		FROM Months
		WHERE Step < @Months
	),
	MonthlyTotals AS
	(
		SELECT
			DATEFROMPARTS(YEAR(E.entry_date), MONTH(E.entry_date), 1) AS MonthStart,
			SUM(CASE WHEN C.account_class = 'Revenue' THEN ISNULL(E.credit, 0) ELSE 0 END) AS TotalRevenue,
			SUM(CASE WHEN C.account_class = 'Expense' THEN ISNULL(E.debit, 0) ELSE 0 END) AS TotalExpenses
		FROM acc_entries E WITH (NOLOCK)
		INNER JOIN AccountClassify C ON C.account_id = E.account_id
		WHERE E.branch_id = @BranchId
		  AND E.entry_date >= @StartMonth
		  AND E.entry_date < @EndExclusive
		GROUP BY DATEFROMPARTS(YEAR(E.entry_date), MONTH(E.entry_date), 1)
	)
	SELECT
		DATENAME(MONTH, M.MonthStart) AS MonthName,
		CAST(YEAR(M.MonthStart) AS NVARCHAR(4)) AS MonthYear,
		CAST(ISNULL(T.TotalRevenue, 0) AS DECIMAL(19,4)) AS TotalRevenue,
		CAST(ISNULL(T.TotalExpenses, 0) AS DECIMAL(19,4)) AS TotalExpenses,
		CAST(ISNULL(T.TotalRevenue, 0) - ISNULL(T.TotalExpenses, 0) AS DECIMAL(19,4)) AS NetProfit
	FROM Months M
	LEFT JOIN MonthlyTotals T ON T.MonthStart = M.MonthStart
	ORDER BY M.MonthStart
	OPTION (MAXRECURSION 0);
END
GO

IF OBJECT_ID(N'dbo.sp_CashFlowTrend', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_CashFlowTrend;
GO

CREATE PROCEDURE dbo.sp_CashFlowTrend
	@BranchId INT,
	@Months INT = 12,
	@AsOfDate DATE = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF @Months IS NULL OR @Months < 1
	BEGIN
		SET @Months = 12;
	END

	DECLARE @EndDate DATE = ISNULL(@AsOfDate, CAST(GETDATE() AS DATE));
	DECLARE @StartDate DATE = DATEADD(MONTH, -@Months, DATEFROMPARTS(YEAR(@EndDate), MONTH(@EndDate), 1));

	;WITH AccountClassify AS
	(
		SELECT
			A.id AS account_id,
			CASE
				WHEN LOWER(ISNULL(A.name, '')) LIKE '%cash%' THEN 'Cash'
				WHEN LOWER(ISNULL(A.name, '')) LIKE '%bank%' THEN 'Bank'
				ELSE 'Other'
			END AS cash_kind
		FROM acc_accounts A WITH (NOLOCK)
	),
	OpeningBalances AS
	(
		SELECT
			SUM(CASE WHEN C.cash_kind = 'Cash' THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0) ELSE 0 END) AS CashOpening,
			SUM(CASE WHEN C.cash_kind = 'Bank' THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0) ELSE 0 END) AS BankOpening
		FROM acc_entries E WITH (NOLOCK)
		INNER JOIN AccountClassify C ON C.account_id = E.account_id
		WHERE E.branch_id = @BranchId
		  AND E.entry_date < @StartDate
		  AND C.cash_kind IN ('Cash', 'Bank')
	),
	DailyMovements AS
	(
		SELECT
			CAST(E.entry_date AS DATE) AS MovementDate,
			SUM(CASE WHEN C.cash_kind = 'Cash' THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0) ELSE 0 END) AS CashMovement,
			SUM(CASE WHEN C.cash_kind = 'Bank' THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0) ELSE 0 END) AS BankMovement
		FROM acc_entries E WITH (NOLOCK)
		INNER JOIN AccountClassify C ON C.account_id = E.account_id
		WHERE E.branch_id = @BranchId
		  AND E.entry_date >= @StartDate
		  AND E.entry_date < DATEADD(DAY, 1, @EndDate)
		  AND C.cash_kind IN ('Cash', 'Bank')
		GROUP BY CAST(E.entry_date AS DATE)
	),
	WeekCalendar AS
	(
		SELECT @StartDate AS WeekStart
		UNION ALL
		SELECT DATEADD(DAY, 7, WeekStart)
		FROM WeekCalendar
		WHERE DATEADD(DAY, 7, WeekStart) <= @EndDate
	)
	SELECT
		WeekEnd = CASE WHEN DATEADD(DAY, 6, W.WeekStart) > @EndDate THEN @EndDate ELSE DATEADD(DAY, 6, W.WeekStart) END,
		CashBalance = CAST(ISNULL(O.CashOpening, 0) + ISNULL((SELECT SUM(D.CashMovement) FROM DailyMovements D WHERE D.MovementDate <= CASE WHEN DATEADD(DAY, 6, W.WeekStart) > @EndDate THEN @EndDate ELSE DATEADD(DAY, 6, W.WeekStart) END), 0) AS DECIMAL(19,4)),
		BankBalance = CAST(ISNULL(O.BankOpening, 0) + ISNULL((SELECT SUM(D.BankMovement) FROM DailyMovements D WHERE D.MovementDate <= CASE WHEN DATEADD(DAY, 6, W.WeekStart) > @EndDate THEN @EndDate ELSE DATEADD(DAY, 6, W.WeekStart) END), 0) AS DECIMAL(19,4)),
		TotalCash = CAST((ISNULL(O.CashOpening, 0) + ISNULL((SELECT SUM(D.CashMovement) FROM DailyMovements D WHERE D.MovementDate <= CASE WHEN DATEADD(DAY, 6, W.WeekStart) > @EndDate THEN @EndDate ELSE DATEADD(DAY, 6, W.WeekStart) END), 0)) + (ISNULL(O.BankOpening, 0) + ISNULL((SELECT SUM(D.BankMovement) FROM DailyMovements D WHERE D.MovementDate <= CASE WHEN DATEADD(DAY, 6, W.WeekStart) > @EndDate THEN @EndDate ELSE DATEADD(DAY, 6, W.WeekStart) END), 0)) AS DECIMAL(19,4))
	FROM WeekCalendar W
	CROSS JOIN OpeningBalances O
	ORDER BY WeekEnd
	OPTION (MAXRECURSION 0);
END
GO

IF OBJECT_ID(N'dbo.sp_ExpenseBreakdownDonut', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_ExpenseBreakdownDonut;
GO

CREATE PROCEDURE dbo.sp_ExpenseBreakdownDonut
	@BranchId INT,
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
			ISNULL(NULLIF(LTRIM(RTRIM(G.name)), ''), ISNULL(NULLIF(LTRIM(RTRIM(A.name)), ''), 'Other Expense')) AS SubGroupName,
			CASE
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%expense%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%expense%'
					THEN 'Expense'
				ELSE 'Other'
			END AS account_class
		FROM acc_accounts A WITH (NOLOCK)
		LEFT JOIN acc_groups G WITH (NOLOCK) ON G.id = A.group_id
		LEFT JOIN acc_account_type T WITH (NOLOCK) ON T.id = G.account_type_id
	),
	ExpenseTotals AS
	(
		SELECT
			C.SubGroupName,
			SUM(ISNULL(E.debit, 0)) AS Amount
		FROM acc_entries E WITH (NOLOCK)
		INNER JOIN AccountClassify C ON C.account_id = E.account_id
		WHERE E.branch_id = @BranchId
		  AND E.entry_date >= @FromDate
		  AND E.entry_date < @ToExclusive
		  AND C.account_class = 'Expense'
		GROUP BY C.SubGroupName
	),
	Ranked AS
	(
		SELECT
			SubGroupName,
			Amount,
			ROW_NUMBER() OVER (ORDER BY Amount DESC, SubGroupName) AS rn
		FROM ExpenseTotals
	),
	TopRows AS
	(
		SELECT TOP (5)
			SubGroupName AS Category,
			CAST(Amount AS DECIMAL(19,4)) AS Amount,
			rn
		FROM Ranked
		WHERE rn <= 5
		ORDER BY Amount DESC, SubGroupName
	)
	SELECT Category, Amount
	FROM
	(
		SELECT Category, Amount, rn FROM TopRows
		UNION ALL
		SELECT
			'Others' AS Category,
			CAST(ISNULL((SELECT SUM(Amount) FROM Ranked WHERE rn > 5), 0) AS DECIMAL(19,4)) AS Amount,
			999 AS rn
	) X
	ORDER BY rn;
END
GO

IF OBJECT_ID(N'dbo.sp_AccountingAttentionItems', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_AccountingAttentionItems;
GO

CREATE PROCEDURE dbo.sp_AccountingAttentionItems
	@BranchId INT,
	@AsOfDate DATE = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SET @AsOfDate = ISNULL(@AsOfDate, CAST(GETDATE() AS DATE));
	DECLARE @MonthStart DATE = DATEFROMPARTS(YEAR(@AsOfDate), MONTH(@AsOfDate), 1);

	;WITH AccountClassify AS
	(
		SELECT
			A.id AS account_id,
			CASE
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%receivable%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%receivable%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%receivable%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%debtor%'
					THEN 'Receivable'
				WHEN LOWER(ISNULL(T.name, '')) LIKE '%payable%'
				  OR LOWER(ISNULL(G.name, '')) LIKE '%payable%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%payable%'
				  OR LOWER(ISNULL(A.name, '')) LIKE '%creditor%'
					THEN 'Payable'
				WHEN LOWER(ISNULL(A.name, '')) LIKE '%bank%' THEN 'Bank'
				ELSE 'Other'
			END AS account_class
		FROM acc_accounts A WITH (NOLOCK)
		LEFT JOIN acc_groups G WITH (NOLOCK) ON G.id = A.group_id
		LEFT JOIN acc_account_type T WITH (NOLOCK) ON T.id = G.account_type_id
	)
	SELECT
		ItemKey = 'OverdueReceivables',
		ItemTitle = 'Overdue Receivables (>30 days)',
		ItemCount = CAST(ISNULL(COUNT(1), 0) AS INT),
		ItemAmount = CAST(ISNULL(SUM(B.balance), 0) AS DECIMAL(19,4)),
		ActionText = 'View'
	FROM
	(
		SELECT
			E.invoice_no,
			MIN(CAST(E.entry_date AS DATE)) AS first_date,
			SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)) AS balance
		FROM acc_entries E WITH (NOLOCK)
		INNER JOIN AccountClassify C ON C.account_id = E.account_id
		WHERE E.branch_id = @BranchId
		  AND E.entry_date <= @AsOfDate
		  AND C.account_class = 'Receivable'
		GROUP BY E.invoice_no
		HAVING SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)) > 0
		   AND DATEDIFF(DAY, MIN(CAST(E.entry_date AS DATE)), @AsOfDate) > 30
	) B;

	SELECT
		ItemKey = 'OverduePayables',
		ItemTitle = 'Overdue Payables (>30 days)',
		ItemCount = CAST(ISNULL(COUNT(1), 0) AS INT),
		ItemAmount = CAST(ISNULL(SUM(B.balance), 0) AS DECIMAL(19,4)),
		ActionText = 'View'
	FROM
	(
		SELECT
			E.invoice_no,
			MIN(CAST(E.entry_date AS DATE)) AS first_date,
			SUM(ISNULL(E.credit, 0) - ISNULL(E.debit, 0)) AS balance
		FROM acc_entries E WITH (NOLOCK)
		INNER JOIN AccountClassify C ON C.account_id = E.account_id
		WHERE E.branch_id = @BranchId
		  AND E.entry_date <= @AsOfDate
		  AND C.account_class = 'Payable'
		GROUP BY E.invoice_no
		HAVING SUM(ISNULL(E.credit, 0) - ISNULL(E.debit, 0)) > 0
		   AND DATEDIFF(DAY, MIN(CAST(E.entry_date AS DATE)), @AsOfDate) > 30
	) B;

	SELECT
		ItemKey = 'UnpostedJv',
		ItemTitle = 'Unposted Journal Vouchers',
		ItemCount = CAST(COUNT(1) AS INT),
		ItemAmount = CAST(0 AS DECIMAL(19,4)),
		ActionText = 'Review'
	FROM acc_entries_header H WITH (NOLOCK)
	WHERE H.branch_id = @BranchId
	  AND ISNULL(H.status, 'Draft') IN ('Draft', 'Unposted');

	SELECT
		ItemKey = 'BankReconciliation',
		ItemTitle = 'Bank accounts not reconciled this month',
		ItemCount = CAST(COUNT(1) AS INT),
		ItemAmount = CAST(0 AS DECIMAL(19,4)),
		ActionText = 'Reconcile'
	FROM
	(
		SELECT A.id
		FROM acc_accounts A WITH (NOLOCK)
		LEFT JOIN acc_bank_reconciliation_header H WITH (NOLOCK)
			ON H.bank_account_id = A.id
		   AND H.branch_id = @BranchId
		   AND H.statement_date >= @MonthStart
		   AND H.statement_date <= @AsOfDate
		WHERE LOWER(ISNULL(A.name, '')) LIKE '%bank%'
		GROUP BY A.id
		HAVING COUNT(H.id) = 0
	) X;

	SELECT
		ItemKey = 'OpenPeriods',
		ItemTitle = 'Open periods older than 45 days',
		ItemCount = CAST(COUNT(1) AS INT),
		ItemAmount = CAST(0 AS DECIMAL(19,4)),
		ActionText = 'Close Period'
	FROM acc_financial_periods P WITH (NOLOCK)
	WHERE P.status = 'Open'
	  AND P.end_date < DATEADD(DAY, -45, @AsOfDate);
END
GO

IF OBJECT_ID(N'dbo.sp_RecentJournalActivity', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_RecentJournalActivity;
GO

CREATE PROCEDURE dbo.sp_RecentJournalActivity
	@BranchId INT,
	@TopN INT = 10
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP (@TopN)
		[Date] = CAST(H.EntryDate AS DATE),
		VoucherNo = H.InvoiceNo,
		[Description] = ISNULL(H.Narration, ''),
		[Amount] = CAST(ABS(ISNULL(H.total_debit, 0) - ISNULL(H.total_credit, 0)) AS DECIMAL(19,4)),
		PostedBy = ISNULL(U.name, ISNULL(U.username, 'System')),
		ModuleSource = ISNULL(H.VoucherType, 'General'),
		ModuleBadge = CASE
			WHEN LOWER(ISNULL(H.VoucherType, '')) LIKE 'sale%' THEN 'Sales'
			WHEN LOWER(ISNULL(H.VoucherType, '')) LIKE 'purchase%' THEN 'Purchases'
			WHEN LOWER(ISNULL(H.VoucherType, '')) LIKE 'bank%' THEN 'Bank'
			WHEN LOWER(ISNULL(H.VoucherType, '')) LIKE 'journal%' THEN 'Journal'
			ELSE ISNULL(H.VoucherType, 'General')
		END,
		ModuleBadgeColor = CASE
			WHEN LOWER(ISNULL(H.VoucherType, '')) LIKE 'sale%' THEN 'info'
			WHEN LOWER(ISNULL(H.VoucherType, '')) LIKE 'purchase%' THEN 'warning'
			WHEN LOWER(ISNULL(H.VoucherType, '')) LIKE 'bank%' THEN 'success'
			WHEN LOWER(ISNULL(H.VoucherType, '')) LIKE 'journal%' THEN 'primary'
			ELSE 'secondary'
		END,
		[Status] = ISNULL(H.status, 'Draft')
	FROM acc_entries_header H WITH (NOLOCK)
	LEFT JOIN pos_users U WITH (NOLOCK) ON U.id = ISNULL(H.posted_by, H.user_id)
	WHERE H.branch_id = @BranchId
	ORDER BY H.EntryDate DESC, H.id DESC;
END
GO

IF OBJECT_ID(N'dbo.sp_AccountingHealthScore', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_AccountingHealthScore;
GO

CREATE PROCEDURE dbo.sp_AccountingHealthScore
	@BranchId INT,
	@AsOfDate DATE = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SET @AsOfDate = ISNULL(@AsOfDate, CAST(GETDATE() AS DATE));
	DECLARE @MonthStart DATE = DATEFROMPARTS(YEAR(@AsOfDate), MONTH(@AsOfDate), 1);

	DECLARE @LastBankRec DATE = NULL;
	SELECT @LastBankRec = MAX(H.statement_date)
	FROM acc_bank_reconciliation_header H WITH (NOLOCK)
	WHERE H.branch_id = @BranchId;

	DECLARE @DaysSinceLastBankRec INT = CASE WHEN @LastBankRec IS NULL THEN NULL ELSE DATEDIFF(DAY, @LastBankRec, @AsOfDate) END;

	DECLARE @UnpostedVoucherCount INT = 0;
	SELECT @UnpostedVoucherCount = COUNT(1)
	FROM acc_entries_header H WITH (NOLOCK)
	WHERE H.branch_id = @BranchId
	  AND ISNULL(H.status, 'Draft') IN ('Draft', 'Unposted');

	DECLARE @UnallocatedReceiptCount INT = 0;
	DECLARE @UnallocatedReceiptAmount DECIMAL(19,4) = 0;
	SELECT
		@UnallocatedReceiptCount = COUNT(1),
		@UnallocatedReceiptAmount = ISNULL(SUM(ISNULL(E.credit, 0) - ISNULL(E.debit, 0)), 0)
	FROM acc_entries E WITH (NOLOCK)
	WHERE E.branch_id = @BranchId
	  AND ISNULL(E.customer_id, 0) > 0
	  AND ISNULL(E.payment_ref_invoice_no, '') = ''
	  AND ISNULL(E.credit, 0) > 0;

	DECLARE @OpenPeriodCount INT = 0;
	SELECT @OpenPeriodCount = COUNT(1)
	FROM acc_financial_periods P WITH (NOLOCK)
	WHERE P.status = 'Open'
	  AND P.end_date < DATEADD(DAY, -45, @AsOfDate);

	DECLARE @BankRecScore INT = CASE
		WHEN @DaysSinceLastBankRec IS NULL THEN 0
		WHEN @DaysSinceLastBankRec <= 7 THEN 30
		WHEN @DaysSinceLastBankRec <= 15 THEN 22
		WHEN @DaysSinceLastBankRec <= 30 THEN 14
		WHEN @DaysSinceLastBankRec <= 45 THEN 6
		ELSE 0
	END;

	DECLARE @VoucherScore INT = CASE
		WHEN @UnpostedVoucherCount = 0 THEN 25
		WHEN @UnpostedVoucherCount <= 5 THEN 18
		WHEN @UnpostedVoucherCount <= 10 THEN 10
		ELSE 0
	END;

	DECLARE @ReceiptScore INT = CASE
		WHEN @UnallocatedReceiptCount = 0 THEN 20
		WHEN @UnallocatedReceiptCount <= 5 THEN 14
		WHEN @UnallocatedReceiptCount <= 10 THEN 8
		ELSE 0
	END;

	DECLARE @PeriodScore INT = CASE
		WHEN @OpenPeriodCount = 0 THEN 25
		WHEN @OpenPeriodCount = 1 THEN 16
		WHEN @OpenPeriodCount <= 3 THEN 8
		ELSE 0
	END;

	DECLARE @HealthScore INT = @BankRecScore + @VoucherScore + @ReceiptScore + @PeriodScore;

	SELECT
		HealthScore = @HealthScore,
		BankRecScore = @BankRecScore,
		VoucherScore = @VoucherScore,
		ReceiptScore = @ReceiptScore,
		PeriodScore = @PeriodScore,
		DaysSinceLastBankRec = ISNULL(@DaysSinceLastBankRec, 0),
		UnpostedVoucherCount = @UnpostedVoucherCount,
		UnallocatedReceiptCount = @UnallocatedReceiptCount,
		UnallocatedReceiptAmount = CAST(ISNULL(@UnallocatedReceiptAmount, 0) AS DECIMAL(19,4)),
		OpenPeriodCount = @OpenPeriodCount,
		HealthStatus = CASE
			WHEN @HealthScore >= 85 THEN 'Healthy'
			WHEN @HealthScore >= 70 THEN 'Watch'
			WHEN @HealthScore >= 50 THEN 'Risk'
			ELSE 'Critical'
		END;
END
GO

IF NOT EXISTS (
	SELECT 1
	FROM sys.indexes
	WHERE name = 'IX_acc_entries_dashboard_branch_date_account'
	  AND object_id = OBJECT_ID('dbo.acc_entries')
)
BEGIN
	CREATE NONCLUSTERED INDEX IX_acc_entries_dashboard_branch_date_account
	ON dbo.acc_entries(branch_id, entry_date, account_id)
	INCLUDE (debit, credit, invoice_no, description, customer_id, supplier_id, bank_id, payment_ref_invoice_no);
END
GO

IF NOT EXISTS (
	SELECT 1
	FROM sys.indexes
	WHERE name = 'IX_acc_entries_dashboard_branch_invoice'
	  AND object_id = OBJECT_ID('dbo.acc_entries')
)
BEGIN
	CREATE NONCLUSTERED INDEX IX_acc_entries_dashboard_branch_invoice
	ON dbo.acc_entries(branch_id, invoice_no, entry_date)
	INCLUDE (account_id, debit, credit, customer_id, payment_ref_invoice_no);
END
GO

IF NOT EXISTS (
	SELECT 1
	FROM sys.indexes
	WHERE name = 'IX_acc_entries_dashboard_branch_customer_paymentref'
	  AND object_id = OBJECT_ID('dbo.acc_entries')
)
BEGIN
	CREATE NONCLUSTERED INDEX IX_acc_entries_dashboard_branch_customer_paymentref
	ON dbo.acc_entries(branch_id, customer_id, payment_ref_invoice_no)
	INCLUDE (entry_date, debit, credit, invoice_no);
END
GO

IF NOT EXISTS (
	SELECT 1
	FROM sys.indexes
	WHERE name = 'IX_acc_entries_header_dashboard_branch_entrydate'
	  AND object_id = OBJECT_ID('dbo.acc_entries_header')
)
BEGIN
	CREATE NONCLUSTERED INDEX IX_acc_entries_header_dashboard_branch_entrydate
	ON dbo.acc_entries_header(branch_id, EntryDate DESC, id DESC)
	INCLUDE (InvoiceNo, VoucherType, Narration, total_debit, total_credit, status, posted_by, user_id);
END
GO

IF NOT EXISTS (
	SELECT 1
	FROM sys.indexes
	WHERE name = 'IX_acc_bank_reconciliation_header_dashboard_branch_date'
	  AND object_id = OBJECT_ID('dbo.acc_bank_reconciliation_header')
)
BEGIN
	CREATE NONCLUSTERED INDEX IX_acc_bank_reconciliation_header_dashboard_branch_date
	ON dbo.acc_bank_reconciliation_header(branch_id, statement_date, bank_account_id)
	INCLUDE (reconciled_on, reconciled_by);
END
GO

IF NOT EXISTS (
	SELECT 1
	FROM sys.indexes
	WHERE name = 'IX_acc_financial_periods_dashboard_status_end_date'
	  AND object_id = OBJECT_ID('dbo.acc_financial_periods')
)
BEGIN
	CREATE NONCLUSTERED INDEX IX_acc_financial_periods_dashboard_status_end_date
	ON dbo.acc_financial_periods(status, end_date)
	INCLUDE (period_id, start_date, year_id, period_name);
END
GO