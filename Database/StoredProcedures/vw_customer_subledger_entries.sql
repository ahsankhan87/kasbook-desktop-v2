
-- View: vw_customer_subledger_entries
-- Purpose: Combines sales invoices and customer payments for sub-ledger reporting
-- Note: payment_terms_id is a foreign key to pos_payment_terms table, days stored in code field
IF OBJECT_ID(N'dbo.vw_customer_subledger_entries', N'V') IS NOT NULL
	DROP VIEW dbo.vw_customer_subledger_entries;
GO

CREATE VIEW dbo.vw_customer_subledger_entries
AS
-- Invoices (Debit - customer owes us)
SELECT
	sm.customer_id,
	sm.sale_date AS transaction_date,
	'Sales Invoice' AS transaction_type,
	sm.invoice_no AS reference_no,
	sm.invoice_no,
	'Invoice for ' + CAST(sm.total_amount AS NVARCHAR(20)) AS description,
	1 AS is_debit,  -- Debit increases receivable
	sm.total_amount AS amount,
	DATEADD(DAY, ISNULL(CASE WHEN ISNUMERIC(pt.code) = 1 THEN CAST(pt.code AS INT) ELSE 30 END, 30), sm.sale_date) AS due_date,
	CASE 
		WHEN sm.total_amount - ISNULL(paid.paid_amount, 0) > 0 THEN 'Outstanding'
		ELSE 'Paid'
	END AS status,
	sm.branch_id,
	sm.id AS entry_id
FROM pos_sales sm
LEFT JOIN pos_payment_terms pt ON sm.payment_terms_id = pt.id
LEFT JOIN (
	SELECT invoice_no, SUM(credit) AS paid_amount
	FROM pos_customers_payments
	GROUP BY invoice_no
) paid ON sm.invoice_no = paid.invoice_no


UNION ALL

-- Payments (Credit - reduces customer receivable)
SELECT
	cp.customer_id,
	cp.entry_date AS transaction_date,
	'Payment Received' AS transaction_type,
	ISNULL(cp.invoice_no, 'PAY-' + CAST(cp.id AS NVARCHAR(20))) AS reference_no,
	ISNULL(sm.invoice_no, '') AS invoice_no,
	'Payment received against invoice' AS description,
	0 AS is_debit,  -- Credit decreases receivable
	cp.credit AS amount,
	cp.entry_date AS due_date,
	'Realized' AS status,
	cp.branch_id,
	10000000 + cp.id AS entry_id  -- Offset entry_id to ensure uniqueness
FROM pos_customers_payments cp
LEFT JOIN pos_sales sm ON cp.invoice_no = sm.invoice_no

GO
