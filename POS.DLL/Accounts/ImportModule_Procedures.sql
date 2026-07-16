-- =============================================
-- Accounting Import Module - Stored Procedures
-- =============================================

PRINT '========================================';
PRINT 'Accounting Import Module Stored Procedures Installation Starting...';
PRINT '========================================';
PRINT '';

-- =============================================
-- TABLE TYPE: ImportBalanceRow
-- For sp_ValidateOpeningBalanceImport
-- =============================================
IF NOT EXISTS (SELECT 1 FROM sys.types WHERE name = 'ImportBalanceRow' AND is_table_type = 1)
BEGIN
	PRINT 'Creating table type: ImportBalanceRow';

	CREATE TYPE dbo.ImportBalanceRow AS TABLE
	(
		acc_code NVARCHAR(50) NOT NULL,
		dr_amount DECIMAL(18,2) NOT NULL DEFAULT(0),
		cr_amount DECIMAL(18,2) NOT NULL DEFAULT(0)
	);

	PRINT 'Table type created successfully.';
END
ELSE
BEGIN
	PRINT 'Table type ImportBalanceRow already exists.';
END
GO

-- =============================================
-- STORED PROCEDURE: sp_RollbackImport
-- Rolls back a completed import session
-- =============================================
IF OBJECT_ID(N'dbo.sp_RollbackImport', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_RollbackImport;
GO

PRINT 'Creating stored procedure: sp_RollbackImport';
GO

CREATE PROCEDURE dbo.sp_RollbackImport
	@SessionId INT,
	@UserId INT
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @Status VARCHAR(15);
	DECLARE @RollbackAvailableUntil DATETIME;
	DECLARE @ImportType VARCHAR(30);
	DECLARE @FileName NVARCHAR(200);
	DECLARE @VoucherCount INT = 0;
	DECLARE @EntriesDeleted INT = 0;
	DECLARE @VouchersDeleted INT = 0;

	-- Check if session exists
	IF NOT EXISTS (SELECT 1 FROM dbo.acc_import_sessions WHERE session_id = @SessionId)
	BEGIN
		RAISERROR('Import session not found.', 16, 1);
		RETURN;
	END

	-- Get session details
	SELECT 
		@Status = status,
		@RollbackAvailableUntil = rollback_available_until,
		@ImportType = import_type,
		@FileName = file_name
	FROM dbo.acc_import_sessions
	WHERE session_id = @SessionId;

	-- Validate session status
	IF @Status <> 'Completed'
	BEGIN
		RAISERROR('Only completed imports can be rolled back. Current status: %s', 16, 1, @Status);
		RETURN;
	END

	-- Check rollback availability
	IF @RollbackAvailableUntil IS NOT NULL AND GETDATE() > @RollbackAvailableUntil
	BEGIN
		RAISERROR('Rollback period has expired. Rollback was available until %s', 16, 1, @RollbackAvailableUntil);
		RETURN;
	END

	BEGIN TRY
		BEGIN TRANSACTION;

		-- Get voucher count
		SELECT @VoucherCount = COUNT(*)
		FROM dbo.acc_import_vouchers
		WHERE session_id = @SessionId;

		-- Delete all acc_entries linked to vouchers in this session
		DELETE ae
		FROM dbo.acc_entries ae
		INNER JOIN dbo.acc_import_vouchers aiv ON aiv.voucher_id = ae.entry_id
		WHERE aiv.session_id = @SessionId;

		SET @EntriesDeleted = @@ROWCOUNT;

		-- Delete all acc_entries_header (vouchers) in this session
		DELETE aeh
		FROM dbo.acc_entries_header aeh
		INNER JOIN dbo.acc_import_vouchers aiv ON aiv.voucher_id = aeh.id
		WHERE aiv.session_id = @SessionId;

		SET @VouchersDeleted = @@ROWCOUNT;

		-- Delete session's acc_import_vouchers links
		DELETE FROM dbo.acc_import_vouchers
		WHERE session_id = @SessionId;

		-- Update session status
		UPDATE dbo.acc_import_sessions
		SET status = 'RolledBack'
		WHERE session_id = @SessionId;

		-- Log to audit trail if table exists
		IF OBJECT_ID(N'dbo.acc_audit_trail', N'U') IS NOT NULL
		BEGIN
			DECLARE @AuditMessage NVARCHAR(MAX);
			SET @AuditMessage = 'Import rollback: Type=' + @ImportType + 
								', File=' + @FileName + 
								', Vouchers deleted=' + CAST(@VouchersDeleted AS NVARCHAR(10)) + 
								', Entries deleted=' + CAST(@EntriesDeleted AS NVARCHAR(10));

			INSERT INTO dbo.acc_audit_trail (action, description, user_id, action_date)
			VALUES ('IMPORT_ROLLBACK', @AuditMessage, @UserId, GETDATE());
		END

		COMMIT TRANSACTION;

		-- Return summary
		SELECT 
			'SUCCESS' AS Result,
			@VouchersDeleted AS VouchersDeleted,
			@EntriesDeleted AS EntriesDeleted,
			'Import session rolled back successfully' AS Message;

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

PRINT 'Stored procedure sp_RollbackImport created successfully.';
GO

-- =============================================
-- STORED PROCEDURE: sp_ImportDataQualityCheck
-- Post-import validation
-- =============================================
IF OBJECT_ID(N'dbo.sp_ImportDataQualityCheck', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_ImportDataQualityCheck;
GO

PRINT 'Creating stored procedure: sp_ImportDataQualityCheck';
GO

CREATE PROCEDURE dbo.sp_ImportDataQualityCheck
	@SessionId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ImportType VARCHAR(30);
	DECLARE @Status VARCHAR(15);

	-- Get session details
	SELECT 
		@ImportType = import_type,
		@Status = status
	FROM dbo.acc_import_sessions
	WHERE session_id = @SessionId;

	-- Check if session exists
	IF @ImportType IS NULL
	BEGIN
		SELECT 'ERROR' AS CheckType, 'Import session not found' AS Issue, NULL AS Details;
		RETURN;
	END

	-- Create temporary results table
	CREATE TABLE #QualityIssues
	(
		CheckType VARCHAR(50),
		Issue NVARCHAR(500),
		Details NVARCHAR(MAX)
	);

	-- Check 1: Accounts with no transactions (for OPENING_BALANCE import)
	IF @ImportType = 'OPENING_BALANCE'
	BEGIN
		INSERT INTO #QualityIssues (CheckType, Issue, Details)
		SELECT 
			'NO_TRANSACTIONS' AS CheckType,
			'Account has no transactions after opening balance import' AS Issue,
			'Account Code: ' + ISNULL(a.account_code, '') + ', Name: ' + ISNULL(a.account_name, '') AS Details
		FROM dbo.acc_accounts a
		WHERE a.is_active = 1
		  AND a.account_type IN ('Asset', 'Liability', 'Equity')
		  AND NOT EXISTS (
			  SELECT 1 
			  FROM dbo.acc_entries ae
			  INNER JOIN dbo.acc_import_vouchers aiv ON aiv.voucher_id = ae.entry_id
			  WHERE aiv.session_id = @SessionId
				AND ae.account_code = a.account_code
		  );
	END

	-- Check 2: Accounts with unexpectedly large balances
	INSERT INTO #QualityIssues (CheckType, Issue, Details)
	SELECT TOP 10
		'LARGE_BALANCE' AS CheckType,
		'Account has unusually large balance after import' AS Issue,
		'Account Code: ' + ae.account_code + 
		', Total Debit: ' + CAST(SUM(ae.debit) AS NVARCHAR(50)) + 
		', Total Credit: ' + CAST(SUM(ae.credit) AS NVARCHAR(50)) +
		', Net Balance: ' + CAST(SUM(ae.debit - ae.credit) AS NVARCHAR(50)) AS Details
	FROM dbo.acc_entries ae
	INNER JOIN dbo.acc_import_vouchers aiv ON aiv.voucher_id = ae.entry_id
	WHERE aiv.session_id = @SessionId
	GROUP BY ae.account_code
	HAVING ABS(SUM(ae.debit - ae.credit)) > 1000000 -- Threshold: 1 million
	ORDER BY ABS(SUM(ae.debit - ae.credit)) DESC;

	-- Check 3: Vouchers that make opening trial balance unbalanced
	IF @ImportType = 'OPENING_BALANCE'
	BEGIN
		WITH VoucherBalance AS
		(
			SELECT 
				aeh.id AS voucher_id,
				aeh.InvoiceNo AS voucher_no,
				SUM(ae.debit) AS total_debit,
				SUM(ae.credit) AS total_credit,
				ABS(SUM(ae.debit) - SUM(ae.credit)) AS difference
			FROM dbo.acc_entries_header aeh
			INNER JOIN dbo.acc_entries ae ON ae.entry_id = aeh.id
			INNER JOIN dbo.acc_import_vouchers aiv ON aiv.voucher_id = aeh.id
			WHERE aiv.session_id = @SessionId
			GROUP BY aeh.id, aeh.InvoiceNo
			HAVING ABS(SUM(ae.debit) - SUM(ae.credit)) > 0.01 -- Allow small rounding difference
		)
		INSERT INTO #QualityIssues (CheckType, Issue, Details)
		SELECT 
			'UNBALANCED_VOUCHER' AS CheckType,
			'Voucher is not balanced (Debit <> Credit)' AS Issue,
			'Voucher No: ' + voucher_no + 
			', Debit: ' + CAST(total_debit AS NVARCHAR(50)) + 
			', Credit: ' + CAST(total_credit AS NVARCHAR(50)) + 
			', Difference: ' + CAST(difference AS NVARCHAR(50)) AS Details
		FROM VoucherBalance;
	END

	-- Check 4: Duplicate account entries in same voucher
	INSERT INTO #QualityIssues (CheckType, Issue, Details)
	SELECT 
		'DUPLICATE_ACCOUNT' AS CheckType,
		'Multiple entries for same account in one voucher' AS Issue,
		'Voucher No: ' + aeh.InvoiceNo + 
		', Account: ' + ae.account_code + 
		', Count: ' + CAST(COUNT(*) AS NVARCHAR(10)) AS Details
	FROM dbo.acc_entries ae
	INNER JOIN dbo.acc_entries_header aeh ON aeh.id = ae.entry_id
	INNER JOIN dbo.acc_import_vouchers aiv ON aiv.voucher_id = aeh.id
	WHERE aiv.session_id = @SessionId
	GROUP BY aeh.id, aeh.InvoiceNo, ae.account_code
	HAVING COUNT(*) > 1;

	-- Check 5: Verify overall trial balance
	IF @ImportType = 'OPENING_BALANCE'
	BEGIN
		DECLARE @TotalDebit DECIMAL(18,2);
		DECLARE @TotalCredit DECIMAL(18,2);
		DECLARE @Difference DECIMAL(18,2);

		SELECT 
			@TotalDebit = SUM(ae.debit),
			@TotalCredit = SUM(ae.credit),
			@Difference = ABS(SUM(ae.debit) - SUM(ae.credit))
		FROM dbo.acc_entries ae
		INNER JOIN dbo.acc_import_vouchers aiv ON aiv.voucher_id = ae.entry_id
		WHERE aiv.session_id = @SessionId;

		IF @Difference > 0.01 -- Allow small rounding difference
		BEGIN
			INSERT INTO #QualityIssues (CheckType, Issue, Details)
			VALUES (
				'TRIAL_BALANCE_MISMATCH',
				'Overall trial balance is not balanced',
				'Total Debit: ' + CAST(@TotalDebit AS NVARCHAR(50)) + 
				', Total Credit: ' + CAST(@TotalCredit AS NVARCHAR(50)) + 
				', Difference: ' + CAST(@Difference AS NVARCHAR(50))
			);
		END
	END

	-- Return results
	IF EXISTS (SELECT 1 FROM #QualityIssues)
	BEGIN
		SELECT CheckType, Issue, Details
		FROM #QualityIssues
		ORDER BY 
			CASE CheckType
				WHEN 'TRIAL_BALANCE_MISMATCH' THEN 1
				WHEN 'UNBALANCED_VOUCHER' THEN 2
				WHEN 'LARGE_BALANCE' THEN 3
				WHEN 'DUPLICATE_ACCOUNT' THEN 4
				WHEN 'NO_TRANSACTIONS' THEN 5
				ELSE 99
			END;
	END
	ELSE
	BEGIN
		SELECT 
			'SUCCESS' AS CheckType,
			'All data quality checks passed' AS Issue,
			NULL AS Details;
	END

	DROP TABLE #QualityIssues;
END
GO

PRINT 'Stored procedure sp_ImportDataQualityCheck created successfully.';
GO

-- =============================================
-- STORED PROCEDURE: sp_GetImportHistory
-- Returns list of import sessions
-- =============================================
IF OBJECT_ID(N'dbo.sp_GetImportHistory', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_GetImportHistory;
GO

PRINT 'Creating stored procedure: sp_GetImportHistory';
GO

CREATE PROCEDURE dbo.sp_GetImportHistory
	@ImportType VARCHAR(30) = NULL,
	@FromDate DATETIME = NULL,
	@ToDate DATETIME = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- Default date range if not provided
	IF @FromDate IS NULL
		SET @FromDate = DATEADD(MONTH, -3, GETDATE()); -- Last 3 months

	IF @ToDate IS NULL
		SET @ToDate = GETDATE();

	SELECT 
		s.session_id,
		s.import_type,
		s.file_name,
		s.total_rows,
		s.imported_rows,
		s.skipped_rows,
		s.error_rows,
		s.status,
		s.imported_at,
		s.rollback_available_until,
		ISNULL(u.full_name, u.username) AS imported_by_name,
		CASE 
			WHEN s.status = 'Completed' AND s.rollback_available_until IS NOT NULL 
				 AND GETDATE() <= s.rollback_available_until 
			THEN 1
			ELSE 0
		END AS can_rollback,
		(SELECT COUNT(*) FROM dbo.acc_import_vouchers WHERE session_id = s.session_id) AS voucher_count
	FROM dbo.acc_import_sessions s
	LEFT JOIN dbo.pos_users u ON u.id = s.imported_by
	WHERE 
		(@ImportType IS NULL OR s.import_type = @ImportType)
		AND s.imported_at >= @FromDate
		AND s.imported_at < DATEADD(DAY, 1, @ToDate)
	ORDER BY s.imported_at DESC;
END
GO

PRINT 'Stored procedure sp_GetImportHistory created successfully.';
GO

-- =============================================
-- STORED PROCEDURE: sp_ValidateOpeningBalanceImport
-- Validates opening balance data before import
-- =============================================
IF OBJECT_ID(N'dbo.sp_ValidateOpeningBalanceImport', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_ValidateOpeningBalanceImport;
GO

PRINT 'Creating stored procedure: sp_ValidateOpeningBalanceImport';
GO

CREATE PROCEDURE dbo.sp_ValidateOpeningBalanceImport
	@BalanceRows dbo.ImportBalanceRow READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TotalDr DECIMAL(18,2) = 0;
	DECLARE @TotalCr DECIMAL(18,2) = 0;
	DECLARE @Difference DECIMAL(18,2) = 0;
	DECLARE @ValidationStatus VARCHAR(20) = 'PASSED';
	DECLARE @ErrorList NVARCHAR(MAX) = '';

	-- Calculate totals
	SELECT 
		@TotalDr = SUM(dr_amount),
		@TotalCr = SUM(cr_amount)
	FROM @BalanceRows;

	SET @Difference = @TotalDr - @TotalCr;

	-- Create temp table for errors
	CREATE TABLE #ValidationErrors
	(
		ErrorType VARCHAR(50),
		ErrorMessage NVARCHAR(500),
		AccountCode NVARCHAR(50)
	);

	-- Check 1: Validate all accounts exist
	INSERT INTO #ValidationErrors (ErrorType, ErrorMessage, AccountCode)
	SELECT 
		'ACCOUNT_NOT_FOUND' AS ErrorType,
		'Account code not found in Chart of Accounts' AS ErrorMessage,
		br.acc_code
	FROM @BalanceRows br
	WHERE NOT EXISTS (
		SELECT 1 FROM dbo.acc_accounts a 
		WHERE a.account_code = br.acc_code
	);

	-- Check 2: Validate no account appears twice
	INSERT INTO #ValidationErrors (ErrorType, ErrorMessage, AccountCode)
	SELECT 
		'DUPLICATE_ACCOUNT' AS ErrorType,
		'Account appears multiple times in import data (Count: ' + CAST(COUNT(*) AS NVARCHAR(10)) + ')' AS ErrorMessage,
		acc_code
	FROM @BalanceRows
	GROUP BY acc_code
	HAVING COUNT(*) > 1;

	-- Check 3: Validate amounts are not negative
	INSERT INTO #ValidationErrors (ErrorType, ErrorMessage, AccountCode)
	SELECT 
		'NEGATIVE_AMOUNT' AS ErrorType,
		'Negative amounts not allowed' AS ErrorMessage,
		acc_code
	FROM @BalanceRows
	WHERE dr_amount < 0 OR cr_amount < 0;

	-- Check 4: Validate account has only debit OR credit (not both)
	INSERT INTO #ValidationErrors (ErrorType, ErrorMessage, AccountCode)
	SELECT 
		'BOTH_DR_CR' AS ErrorType,
		'Account has both debit and credit amounts' AS ErrorMessage,
		acc_code
	FROM @BalanceRows
	WHERE dr_amount > 0 AND cr_amount > 0;

	-- Check 5: Validate balance totals match (Dr = Cr)
	IF ABS(@Difference) > 0.01 -- Allow small rounding difference
	BEGIN
		SET @ValidationStatus = 'FAILED';
		INSERT INTO #ValidationErrors (ErrorType, ErrorMessage, AccountCode)
		VALUES (
			'BALANCE_MISMATCH',
			'Total Debit does not equal Total Credit. Difference: ' + CAST(@Difference AS NVARCHAR(50)),
			NULL
		);
	END

	-- If any errors, mark as FAILED
	IF EXISTS (SELECT 1 FROM #ValidationErrors)
	BEGIN
		SET @ValidationStatus = 'FAILED';

		-- Build error list as JSON array
		SELECT @ErrorList = '[' + STRING_AGG(
			'{"ErrorType":"' + ErrorType + 
			'","Message":"' + ErrorMessage + 
			'","Account":"' + ISNULL(AccountCode, '') + '"}', 
			','
		) + ']'
		FROM #ValidationErrors;
	END

	-- Return validation summary
	SELECT 
		@ValidationStatus AS validation_status,
		@TotalDr AS total_dr,
		@TotalCr AS total_cr,
		@Difference AS difference,
		@ErrorList AS error_list;

	-- Return detailed errors
	IF EXISTS (SELECT 1 FROM #ValidationErrors)
	BEGIN
		SELECT 
			ErrorType,
			ErrorMessage,
			AccountCode
		FROM #ValidationErrors
		ORDER BY 
			CASE ErrorType
				WHEN 'BALANCE_MISMATCH' THEN 1
				WHEN 'ACCOUNT_NOT_FOUND' THEN 2
				WHEN 'DUPLICATE_ACCOUNT' THEN 3
				WHEN 'BOTH_DR_CR' THEN 4
				WHEN 'NEGATIVE_AMOUNT' THEN 5
				ELSE 99
			END,
			AccountCode;
	END

	DROP TABLE #ValidationErrors;
END
GO

PRINT 'Stored procedure sp_ValidateOpeningBalanceImport created successfully.';
GO

PRINT '';
PRINT '========================================';
PRINT 'Stored Procedures Installation Completed Successfully';
PRINT '========================================';
PRINT '';
PRINT 'Available Procedures:';
PRINT '- sp_RollbackImport: Rollback completed import session';
PRINT '- sp_ImportDataQualityCheck: Post-import data validation';
PRINT '- sp_GetImportHistory: Get list of import sessions';
PRINT '- sp_ValidateOpeningBalanceImport: Pre-import validation for opening balances';
PRINT '';
GO
