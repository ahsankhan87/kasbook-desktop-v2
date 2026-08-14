-- SQL Query to diagnose the issue
-- Run these queries to understand the data structure

-- 1. Check how many bank accounts exist and their GL Account IDs
SELECT 
	id,
	name,
	GLAccountID,
	branch_id
FROM pos_banks
ORDER BY id;

-- 2. Check the acc_entries structure - see what account_ids are being used
SELECT 
	DISTINCT E.account_id,
	COUNT(*) as entry_count,
	A.name as account_name
FROM acc_entries E
LEFT JOIN acc_accounts A ON A.id = E.account_id
GROUP BY E.account_id, A.name
ORDER BY E.account_id;

-- 3. Manually test the stored procedure with the first bank account
-- (Replace @BankAccountId with the actual GLAccountID from pos_banks)
DECLARE @TestBankAccountId INT = (SELECT TOP 1 GLAccountID FROM pos_banks ORDER BY id);
DECLARE @TestBranchId INT = 1;  -- Change if needed
DECLARE @TestStatementDate DATE = CAST(GETDATE() AS DATE);

PRINT 'Testing with BankAccountId=' + CAST(@TestBankAccountId AS VARCHAR);
PRINT 'Branch=' + CAST(@TestBranchId AS VARCHAR);
PRINT 'StatementDate=' + CAST(@TestStatementDate AS VARCHAR);

EXEC sp_BankReconciliation 
	@OperationType = 1,
	@BranchId = @TestBranchId,
	@BankAccountId = @TestBankAccountId,
	@StatementDate = @TestStatementDate;
