-- ============================================================================
-- BANK RECONCILIATION STORED PROCEDURE FIX
-- ============================================================================
-- Issue: When no reconciliation header exists, @LatestRecId is NULL, and the
--        LEFT JOIN condition (I.reconciliation_id = NULL) returns no rows,
--        causing ALL bank entries to be loaded instead of filtering by bank.
--
-- Solution: Add "AND @LatestRecId IS NOT NULL" to the LEFT JOIN condition
--           to ensure the join only happens when @LatestRecId has a value.
-- ============================================================================

IF OBJECT_ID(N'dbo.sp_BankReconciliation', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_BankReconciliation;
GO

CREATE PROCEDURE dbo.sp_BankReconciliation
	@OperationType INT,
	@ReconciliationId INT = NULL,
	@BranchId INT = NULL,
	@BankAccountId INT = NULL,
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

		SELECT
			E.id AS entry_id,
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
		   AND @LatestRecId IS NOT NULL  -- FIX: Only join if @LatestRecId is not NULL
		WHERE E.branch_id = @BranchId
		  AND E.account_id = @BankAccountId
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
		   AND @RecId IS NOT NULL  -- FIX: Only join if @RecId is not NULL
		WHERE E.branch_id = @BranchId
		  AND E.account_id = @BankAccountId
		  AND E.entry_date <= @StatementDate
		  AND ISNULL(I.is_cleared, 0) = 0
		ORDER BY E.entry_date, E.id;

		RETURN;
	END
END
GO

-- Verify the stored procedure was created
PRINT 'Stored procedure sp_BankReconciliation updated successfully!';
