-- View: vw_cashbook_entries
-- Purpose: All cash account transactions for cash book reporting
-- Updated to use is_cash and is_bank bit fields from acc_accounts table
IF OBJECT_ID(N'dbo.vw_cashbook_entries', N'V') IS NOT NULL
	DROP VIEW dbo.vw_cashbook_entries;
GO

CREATE VIEW dbo.vw_cashbook_entries
AS
-- Customer Payments (Receipts)
SELECT
	aa.id AS cash_account_id,
	cp.entry_date AS transaction_date,
	'Customer Payment' AS transaction_type,
	ISNULL(cp.invoice_no, 'CP-' + CAST(cp.id AS NVARCHAR(20))) AS reference_no,
	'Payment from customer' AS description,
	1 AS is_receipt,
	cp.credit AS amount,
	aa.id AS cash_account_id_actual,
	aa.name AS cash_account_name,
	cp.branch_id,
	cp.id AS entry_id
FROM pos_customers_payments cp
LEFT JOIN acc_accounts aa ON aa.is_cash = 1 AND aa.branch_id = cp.branch_id
WHERE aa.is_cash = 1

UNION ALL

-- Supplier Payments (Payments)
SELECT
	aa.id AS cash_account_id,
	sp.entry_date AS transaction_date,
	'Supplier Payment' AS transaction_type,
	ISNULL(sp.invoice_no, 'SP-' + CAST(sp.id AS NVARCHAR(20))) AS reference_no,
	'Payment to supplier' AS description,
	0 AS is_receipt,
	sp.debit AS amount,
	aa.id AS cash_account_id_actual,
	aa.name AS cash_account_name,
	sp.branch_id,
	10000000 + sp.id AS entry_id
FROM pos_suppliers_payments sp
LEFT JOIN acc_accounts aa ON aa.is_cash = 1 AND aa.branch_id = sp.branch_id
WHERE aa.is_cash = 1

UNION ALL

-- Bank Deposits (Receipts)
SELECT
	ba.id AS cash_account_id,
	bd.entry_date AS transaction_date,
	'Bank Deposit' AS transaction_type,
	bd.invoice_no,
	'Deposit to bank account' AS description,
	1 AS is_receipt,
	bd.debit,
	ba.id AS cash_account_id_actual,
	ba.name AS cash_account_name,
	bd.branch_id,
	20000000 + bd.id AS entry_id
FROM pos_banks_payments bd
LEFT JOIN acc_accounts ba ON bd.account_id = ba.id AND ba.is_bank = 1
WHERE ba.is_bank = 1

UNION ALL

-- Bank Withdrawals (Payments)
SELECT
	ba.id AS cash_account_id,
	bw.entry_date AS transaction_date,
	'Bank Withdrawal' AS transaction_type,
	bw.invoice_no,
	'Withdrawal from bank account' AS description,
	0 AS is_receipt,
	bw.credit,
	ba.id AS cash_account_id_actual,
	ba.name AS cash_account_name,
	bw.branch_id,
	30000000 + bw.id AS entry_id
FROM pos_banks_payments bw
LEFT JOIN acc_accounts ba ON bw.account_id = ba.id AND ba.is_bank = 1
WHERE ba.is_bank = 1

UNION ALL

-- Journal Voucher Entries (General Ledger cash transactions)
SELECT
	aa.id AS cash_account_id,
	jv.EntryDate AS transaction_date,
	'Journal Entry' AS transaction_type,
	jv.InvoiceNo AS reference_no,
	'Journal voucher entry' AS description,
	CASE WHEN jvd.debit > 0 THEN 1 ELSE 0 END AS is_receipt,
	CASE WHEN jvd.debit > 0 THEN jvd.debit ELSE jvd.credit END AS amount,
	aa.id AS cash_account_id_actual,
	aa.name AS cash_account_name,
	jv.branch_id,
	40000000 + jvd.id AS entry_id
FROM acc_entries_header jv
INNER JOIN acc_entries jvd ON jv.InvoiceNo = jvd.invoice_no
INNER JOIN acc_accounts aa ON jvd.account_id = aa.id
WHERE jv.status = 'Posted' 
  AND (aa.is_cash = 1 OR aa.is_bank = 1);

GO
