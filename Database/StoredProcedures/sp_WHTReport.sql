-- =============================================
-- Author: Nozum ERP Tax Module
-- Create date: 2025
-- Description: Fetch Withholding Tax (WHT) Report for a given date range
-- =============================================
CREATE PROCEDURE sp_WHTReport
	@FromDate DATE,
	@ToDate DATE
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		s.id AS SupplierId,
		s.first_name AS SupplierName,
		ISNULL(s.vat_no, '') AS VATNO,
		ph.purchaseDate AS PaymentDate,
		ph.totalAmount AS PaymentAmount,
		ISNULL(wht.whtRate, 0) AS WHTRate,
		ISNULL(wht.whtAmount, 0) AS WHTAmount,
		ISNULL(wht.taxSection, '') AS TaxSection,  -- '153', '155', etc.
		ISNULL(wht.remarks, '') AS Remarks,
		ph.branch_id
	FROM 
		purchases_header ph
		LEFT JOIN supplier s ON ph.party_id = s.id
		LEFT JOIN wht_deductions wht ON ph.id = wht.purchase_id
	WHERE 
		ph.voucherDate >= @FromDate 
		AND ph.voucherDate <= @ToDate
		AND wht.whtAmount > 0
		AND ph.status IN (1, 2)  -- Active or Posted
	ORDER BY 
		ph.voucherDate DESC, 
		s.name;
END;
