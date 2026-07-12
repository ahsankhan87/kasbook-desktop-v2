IF OBJECT_ID(N'dbo.acc_bank_reconciliation_header', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.acc_bank_reconciliation_header
	(
		id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		branch_id INT NOT NULL,
		bank_account_id INT NOT NULL,
		statement_date DATE NOT NULL,
		bank_statement_balance DECIMAL(18,2) NOT NULL,
		adjusted_balance DECIMAL(18,2) NOT NULL,
		book_balance DECIMAL(18,2) NOT NULL,
		difference DECIMAL(18,2) NOT NULL,
		reconciled_by INT NULL,
		reconciled_on DATETIME NOT NULL CONSTRAINT DF_acc_bank_reconciliation_header_reconciled_on DEFAULT(GETDATE()),
		date_created DATETIME NOT NULL CONSTRAINT DF_acc_bank_reconciliation_header_date_created DEFAULT(GETDATE())
	);

	CREATE UNIQUE INDEX UX_acc_bank_rec_header_branch_account_date
	ON dbo.acc_bank_reconciliation_header(branch_id, bank_account_id, statement_date);
END
GO

IF OBJECT_ID(N'dbo.acc_bank_reconciliation_items', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.acc_bank_reconciliation_items
	(
		id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		reconciliation_id INT NOT NULL,
		entry_id INT NOT NULL,
		is_cleared BIT NOT NULL,
		updated_by INT NULL,
		updated_on DATETIME NOT NULL CONSTRAINT DF_acc_bank_reconciliation_items_updated_on DEFAULT(GETDATE()),
		CONSTRAINT FK_acc_bank_reconciliation_items_header FOREIGN KEY (reconciliation_id)
			REFERENCES dbo.acc_bank_reconciliation_header(id)
	);

	CREATE UNIQUE INDEX UX_acc_bank_rec_items_recon_entry
	ON dbo.acc_bank_reconciliation_items(reconciliation_id, entry_id);
END
GO

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
		WHERE E.branch_id = @BranchId
		  AND E.account_id = @BankAccountId
		  AND E.entry_date <= @StatementDate
		  AND ISNULL(I.is_cleared, 0) = 0
		ORDER BY E.entry_date, E.id;

		RETURN;
	END
END
GO
