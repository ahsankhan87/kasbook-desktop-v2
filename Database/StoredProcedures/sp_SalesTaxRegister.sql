
-- =============================================
-- Author: Nozum ERP Tax Module
-- Create date: 2025
-- Description: Fetch Sales and Purchase Tax Register for a given date range and tax type
-- =============================================
CREATE PROCEDURE sp_SalesTaxRegister
	@FromDate DATE,
	@ToDate DATE,
	@TaxType NVARCHAR(50) = 'ALL'  -- 'ALL', 'SALES', 'PURCHASES'
AS
BEGIN
	SET NOCOUNT ON;

	-- Sales Tax Register
	IF @TaxType IN ('ALL', 'SALES')
	BEGIN
		SELECT 
			'SALES' AS TransactionType,
			sh.id AS TransactionId,
			sh.invoice_no AS InvoiceNo,
			sh.sale_date AS [Date],
			ISNULL(c.first_name, sh.customer_name) AS PartyName,
			ISNULL(c.vat_no, '') AS NTN,
			sh.total_amount AS Amount,
			ISNULL(stl.tax_rate, 15) AS TaxRate,
			ISNULL(sh.total_tax, 0) AS TaxAmount,
			sh.description AS Remarks,
			sh.branch_id
		FROM 
			pos_sales sh
			LEFT JOIN pos_customers c ON sh.customer_id = c.id
			LEFT JOIN pos_sales_items stl ON sh.id = stl.sale_id
		WHERE 
			sh.sale_date >= @FromDate 
			AND sh.sale_date <= @ToDate
			--AND sh.status IN (1, 2)  -- Active or Posted
		ORDER BY sh.sale_date, sh.invoice_no;
	END

	-- Purchase Tax Register
	IF @TaxType IN ('ALL', 'PURCHASES')
	BEGIN
		SELECT 
			'PURCHASES' AS TransactionType,
			ph.id AS TransactionId,
			ph.invoice_no AS InvoiceNo,
			ph.purchase_date AS [Date],
			ISNULL(s.first_name, '') AS PartyName,
			ISNULL(s.vat_no, '') AS NTN,
			ph.total_amount AS Amount,
			ISNULL(ptl.tax_rate, 15) AS TaxRate,
			ISNULL(ph.total_tax, 0) AS TaxAmount,
			ph.description AS Remarks,
			ph.branch_id
		FROM 
			pos_purchases ph
			LEFT JOIN pos_suppliers s ON ph.supplier_id = s.id
			LEFT JOIN pos_purchases_items ptl ON ph.id = ptl.purchase_id
		WHERE 
			ph.purchase_date >= @FromDate 
			AND ph.purchase_date <= @ToDate
			--AND ph.status IN (1, 2)  -- Active or Posted
		ORDER BY ph.purchase_date, ph.invoice_no;
	END
END;
