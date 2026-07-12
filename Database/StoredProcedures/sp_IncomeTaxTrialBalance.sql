
-- =============================================
-- Author: Nozum ERP Tax Module
-- Create date: 2025
-- Description: Fetch Trial Balance filtered for Income and Expense accounts only (for tax purposes)
-- =============================================
CREATE PROCEDURE sp_IncomeTaxTrialBalance
	@FinancialYearId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		a.id AS AccountId,
		a.code AS AccountCode,
		a.name AS AccountName,
		ag.name AS AccountGroupName,
		at.name AS AccountType,  -- 'Income', 'Expense', etc.
		ISNULL(SUM(ae.debit), 0) AS DebitAmount,
		ISNULL(SUM(ae.credit),0) AS CreditAmount,
		(ISNULL(SUM(ae.debit), 0) - ISNULL(SUM(ae.credit), 0)) AS Balance
	FROM 
		acc_accounts a
		LEFT JOIN acc_groups ag ON a.group_id = ag.id
		LEFT JOIN acc_account_type at ON ag.account_type_id = at.id
		LEFT JOIN acc_entries_header aeh ON aeh.period_id = @FinancialYearId
		LEFT JOIN acc_entries ae ON ae.invoice_no = aeh.InvoiceNo AND ae.account_id = a.id
	WHERE 
		at.name IN ('Revenue', 'Expense')  -- Filter for income and expense accounts only
		AND aeh.status IN (1, 2)  -- Active or Posted
	GROUP BY 
		a.id, a.code, a.name, ag.name, at.name
	HAVING 
		ISNULL(SUM(ae.debit), 0) 
		+ ISNULL(SUM(ae.credit), 0) > 0
	ORDER BY 
		at.name, a.code;
END;
