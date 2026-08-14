-- ============================================================================
-- ADD BANK_ID COLUMN TO ACC_ENTRIES - SOLUTION TO BANK FILTERING ISSUE
-- ============================================================================
-- Problem: All banks point to same GLAccountID in Chart of Accounts,
--          so entries cannot be distinguished by account_id alone.
--
-- Solution: Add bank_id column to acc_entries to track which bank account
--           each entry belongs to.
-- ============================================================================

-- STEP 1: Add bank_id column to acc_entries (if not already exists)
IF NOT EXISTS (
	SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE TABLE_NAME = 'acc_entries' AND COLUMN_NAME = 'bank_id'
)
BEGIN
	ALTER TABLE dbo.acc_entries 
	ADD bank_id INT NULL;

	PRINT 'Column bank_id added to acc_entries';
END
ELSE
BEGIN
	PRINT 'Column bank_id already exists on acc_entries';
END
GO

-- STEP 2: Create index on bank_id for faster queries
IF NOT EXISTS (
	SELECT 1 FROM sys.indexes 
	WHERE name = 'IX_acc_entries_bank_id' AND object_id = OBJECT_ID('dbo.acc_entries')
)
BEGIN
	CREATE INDEX IX_acc_entries_bank_id ON dbo.acc_entries(bank_id) WHERE bank_id IS NOT NULL;
	PRINT 'Index IX_acc_entries_bank_id created';
END
ELSE
BEGIN
	PRINT 'Index IX_acc_entries_bank_id already exists';
END
GO

-- STEP 3: Update existing entries to populate bank_id
-- This assumes you can identify which bank based on the entry content
-- Adjust the logic based on your business rules
/*
UPDATE E
SET E.bank_id = B.id
FROM dbo.acc_entries E
INNER JOIN dbo.pos_banks B ON B.GLAccountID = E.account_id
WHERE E.bank_id IS NULL AND E.account_id = B.GLAccountID;

PRINT 'Existing entries updated with bank_id';
*/

-- STEP 4: Update stored procedure sp_BankReconciliation to use bank_id
-- Replace the existing procedure with this updated version

IF OBJECT_ID(N'dbo.sp_BankReconciliation', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_BankReconciliation;
GO

CREATE PROCEDURE dbo.sp_BankReconciliation
	@OperationType INT,
	@ReconciliationId INT = NULL,
	@BranchId INT = NULL,
	@BankAccountId INT = NULL,      -- Now this is pos_banks.id, not GLAccountID
	@StatementDate DATE = NULL,
	@BankStatementBalance DECIMAL(18,2) = NULL,
	@AdjustedBalance DECIMAL(18,2) = NULL,
	@BookBalance DECIMAL(18,2) = NULL,
	@Difference DECIMAL(18,2) = NULL,
	@EntryId INT = NULL,
	@IsCleared BIT = NULL,
	@UserId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- 1: Load system transactions with cleared status
	IF (@OperationType = 1)
	BEGIN
		DECLARE @LatestRecId INT;

		SELECT TOP 1 @LatestRecId = H.id
		FROM dbo.acc_bank_reconciliation_header H
		WHERE H.branch_id = @BranchId
		  AND H.bank_account_id = @BankAccountId
		  AND H.statement_date = @StatementDate
		ORDER BY H.id DESC;

		-- Filter by bank_id instead of account_id
		SELECT
			E.id AS entry_id,
			E.bank_id,
			E.account_id,
			E.entry_date,
			E.invoice_no,
			ISNULL(E.description, '') AS description,
			ISNULL(E.debit, 0) AS debit,
			ISNULL(E.credit, 0) AS credit,
			ISNULL(E.debit, 0) - ISNULL(E.credit, 0) AS amount,
			CAST(ISNULL(I.is_cleared, 0) AS BIT) AS is_cleared
		FROM acc_entries E
		LEFT JOIN dbo.acc_bank_reconciliation_items I
			ON I.entry_id = E.id
		   AND I.reconciliation_id = @LatestRecId
		   AND @LatestRecId IS NOT NULL
		WHERE E.branch_id = @BranchId
		  AND E.bank_id = @BankAccountId  -- FIX: Filter by bank_id
		  AND E.entry_date <= @StatementDate
		ORDER BY E.entry_date, E.id;

		RETURN;
	END

	-- 2: Upsert reconciliation header and return reconciliation id
	IF (@OperationType = 2)
	BEGIN
		DECLARE @HeaderId INT;

		SELECT @HeaderId = H.id
		FROM dbo.acc_bank_reconciliation_header H
		WHERE H.branch_id = @BranchId
		  AND H.bank_account_id = @BankAccountId
		  AND H.statement_date = @StatementDate;

		IF (@HeaderId IS NULL)
		BEGIN
			INSERT INTO dbo.acc_bank_reconciliation_header
			(
				branch_id,
				bank_account_id,
				statement_date,
				bank_statement_balance,
				adjusted_balance,
				book_balance,
				difference,
				reconciled_by,
				reconciled_on
			)
			VALUES
			(
				@BranchId,
				@BankAccountId,
				@StatementDate,
				ISNULL(@BankStatementBalance, 0),
				ISNULL(@AdjustedBalance, 0),
				ISNULL(@BookBalance, 0),
				ISNULL(@Difference, 0),
				@UserId,
				GETDATE()
			);

			SET @HeaderId = SCOPE_IDENTITY();
		END
		ELSE
		BEGIN
			UPDATE dbo.acc_bank_reconciliation_header
			SET bank_statement_balance = ISNULL(@BankStatementBalance, 0),
				adjusted_balance = ISNULL(@AdjustedBalance, 0),
				book_balance = ISNULL(@BookBalance, 0),
				difference = ISNULL(@Difference, 0),
				reconciled_by = @UserId,
				reconciled_on = GETDATE()
			WHERE id = @HeaderId;

			DELETE FROM dbo.acc_bank_reconciliation_items WHERE reconciliation_id = @HeaderId;
		END

		SELECT @HeaderId AS reconciliation_id;
		RETURN;
	END

	-- 3: Save/update one transaction cleared status for reconciliation
	IF (@OperationType = 3)
	BEGIN
		IF (@ReconciliationId IS NULL OR @EntryId IS NULL)
		BEGIN
			RAISERROR('ReconciliationId and EntryId are required for OperationType 3.', 16, 1);
			RETURN;
		END

		IF EXISTS (SELECT 1 FROM dbo.acc_bank_reconciliation_items WHERE reconciliation_id = @ReconciliationId AND entry_id = @EntryId)
		BEGIN
			UPDATE dbo.acc_bank_reconciliation_items
			SET is_cleared = ISNULL(@IsCleared, 0),
				updated_by = @UserId,
				updated_on = GETDATE()
			WHERE reconciliation_id = @ReconciliationId
			  AND entry_id = @EntryId;
		END
		ELSE
		BEGIN
			INSERT INTO dbo.acc_bank_reconciliation_items (reconciliation_id, entry_id, is_cleared, updated_by)
			VALUES (@ReconciliationId, @EntryId, ISNULL(@IsCleared, 0), @UserId);
		END

		RETURN;
	END

	-- 4: Load uncleared transactions for selected statement date
	IF (@OperationType = 4)
	BEGIN
		DECLARE @RecId INT;

		SELECT TOP 1 @RecId = H.id
		FROM dbo.acc_bank_reconciliation_header H
		WHERE H.branch_id = @BranchId
		  AND H.bank_account_id = @BankAccountId
		  AND H.statement_date = @StatementDate
		ORDER BY H.id DESC;

		SELECT
			E.id AS entry_id,
			E.bank_id,
			E.account_id,
			E.entry_date,
			E.invoice_no,
			ISNULL(E.description, '') AS description,
			ISNULL(E.debit, 0) AS debit,
			ISNULL(E.credit, 0) AS credit,
			ISNULL(E.debit, 0) - ISNULL(E.credit, 0) AS amount
		FROM acc_entries E
		LEFT JOIN dbo.acc_bank_reconciliation_items I
			ON I.entry_id = E.id
		   AND I.reconciliation_id = @RecId
		   AND @RecId IS NOT NULL
		WHERE E.branch_id = @BranchId
		  AND E.bank_id = @BankAccountId  -- FIX: Filter by bank_id
		  AND E.entry_date <= @StatementDate
		  AND ISNULL(I.is_cleared, 0) = 0
		ORDER BY E.entry_date, E.id;

		RETURN;
	END
END
GO

PRINT 'Stored procedure sp_BankReconciliation updated with bank_id filtering!';
