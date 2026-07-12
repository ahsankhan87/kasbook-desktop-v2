-- View: vw_supplier_subledger_entries
-- Purpose: Combines purchase invoices and supplier payments for sub-ledger reporting
-- Note: payment_terms_id is a foreign key to pos_payment_terms table, days stored in code field
IF OBJECT_ID(N'dbo.vw_supplier_subledger_entries', N'V') IS NOT NULL
	DROP VIEW dbo.vw_supplier_subledger_entries;
GO

CREATE VIEW dbo.vw_supplier_subledger_entries
AS
-- Bills (Credit - we owe supplier)
SELECT
	pm.supplier_id,
	pm.purchase_date AS transaction_date,
	'Purchase Bill' AS transaction_type,
	pm.invoice_no AS reference_no,
	pm.invoice_no,
	'Bill for ' + CAST(pm.total_amount AS NVARCHAR(20)) AS description,
	1 AS is_credit,  -- Credit increases payable
	pm.total_amount AS amount,
	DATEADD(DAY, ISNULL(CASE WHEN ISNUMERIC(pt.code) = 1 THEN CAST(pt.code AS INT) ELSE 30 END, 30), pm.purchase_date) AS due_date,
	CASE 
		WHEN pm.total_amount - ISNULL(paid.paid_amount, 0) > 0 THEN 'Outstanding'
		ELSE 'Paid'
	END AS status,
	pm.branch_id,
	pm.id AS entry_id
FROM pos_purchases pm
LEFT JOIN pos_payment_terms pt ON pm.payment_terms_id = pt.id
LEFT JOIN (
	SELECT invoice_no, SUM(debit) AS paid_amount
	FROM pos_suppliers_payments
	GROUP BY invoice_no
) paid ON pm.invoice_no = paid.invoice_no

UNION ALL

-- Payments (Debit - reduces payable)
SELECT
	sp.supplier_id,
	sp.entry_date AS transaction_date,
	'Payment Made' AS transaction_type,
	ISNULL(sp.invoice_no, 'PAY-' + CAST(sp.id AS NVARCHAR(20))) AS reference_no,
	ISNULL(pm.invoice_no, '') AS invoice_no,
	'Payment made for bill' AS description,
	0 AS is_credit,  -- Debit decreases payable
	sp.debit AS amount,
	sp.entry_date AS due_date,
	'Realized' AS status,
	sp.branch_id,
	10000000 + sp.id AS entry_id  -- Offset entry_id to ensure uniqueness
FROM pos_suppliers_payments sp
LEFT JOIN pos_purchases pm ON sp.invoice_no = pm.invoice_no;

GO
