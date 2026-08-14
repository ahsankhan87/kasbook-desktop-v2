-- ============================================================================
-- File: sp_GetMovingItems.sql
-- Purpose: Retrieve moving items (high-velocity inventory with recent sales)
-- Database Tables:
--   - pos_products: Product master (code, name, category_code, brand_code, avg_cost, unit_price)
--   - pos_product_stocks: Branch-wise inventory (product_id, branch_id, qty_on_hand)
--   - pos_sales_items: Sales transactions (product_id, branch_id, qty_sold, created_date)
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_GetMovingItems]
	@BranchId INT,
	@DaysThreshold INT = 30,
	@MinQtyOnHand DECIMAL(18,4) = 0,
	@CategoryCode NVARCHAR(50) = NULL,
	@BrandCode NVARCHAR(50) = NULL,
	@LocationCode NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	-- CTE to calculate aggregates for each product
	WITH ProductAggregates AS (
		SELECT
			p.id,
			p.code,
			p.name,
			p.name_ar,
			p.category_code,
			p.brand_code,
			p.location_code,
			p.date_created,
			p.avg_cost,
			p.unit_price,
			COALESCE(ps.qty, 0) AS QtyOnHand,
			COALESCE(MAX(s.sale_date), p.date_created) AS LastSaleDate,
			DATEDIFF(DAY, COALESCE(MAX(s.sale_date), p.date_created), CAST(GETDATE() AS DATE)) AS DaysSinceLastSale,
			COUNT(DISTINCT si.id) AS TotalTransactions,
			COALESCE(SUM(si.quantity_sold), 0) AS QtyMovedLastPeriod,
			COALESCE(ps.qty, 0) * COALESCE(p.avg_cost, 0) AS TotalValue
		FROM
			pos_products p
			LEFT JOIN pos_product_stocks ps ON p.item_number = ps.item_number AND ps.branch_id = @BranchId
			LEFT JOIN pos_sales_items si ON p.item_number = si.item_number AND si.branch_id = @BranchId
			LEFT JOIN pos_sales s ON si.sale_id = s.id AND s.branch_id = @BranchId
		WHERE
			p.deleted = 0
			AND (@CategoryCode IS NULL OR p.category_code = @CategoryCode)
			AND (@BrandCode IS NULL OR p.brand_code = @BrandCode)
			AND (@LocationCode IS NULL OR p.location_code = @LocationCode)
		GROUP BY
			p.id, p.code, p.name, p.name_ar, p.category_code, p.brand_code, p.location_code,
			p.date_created, p.avg_cost, p.unit_price, ps.qty
	)
	SELECT
		id,
		code AS ItemCode,
		name AS ItemName,
		ISNULL(name_ar, name) AS ItemNameAR,
		category_code AS Category,
		brand_code AS Brand,
		location_code AS Location,
		QtyOnHand,
		avg_cost AS CostPrice,
		unit_price AS SalePrice,
		TotalValue,
		LastSaleDate,
		DaysSinceLastSale,
		TotalTransactions,
		QtyMovedLastPeriod,
		CASE
			WHEN DaysSinceLastSale <= @DaysThreshold
			THEN 'Fast Moving'
			ELSE 'Slow Moving'
		END AS MovingStatus
	FROM
		ProductAggregates
	WHERE
		DaysSinceLastSale <= @DaysThreshold
		AND QtyOnHand >= @MinQtyOnHand
	ORDER BY
		DaysSinceLastSale ASC,
		QtyMovedLastPeriod DESC,
		code ASC;

END
GO

-- ============================================================================
-- File: sp_GetMovingItemsSummary.sql
-- Purpose: Retrieve summary statistics for moving items
-- ============================================================================

CREATE PROCEDURE [dbo].[sp_GetMovingItemsSummary]
	@BranchId INT,
	@DaysThreshold INT = 30
AS
BEGIN
	SET NOCOUNT ON;

	-- CTE to calculate aggregates for each product
	WITH ProductAggregates AS (
		SELECT
			p.id,
			p.avg_cost,
			p.unit_price,
			p.category_code,
			p.brand_code,
			COALESCE(ps.qty, 0) AS QtyOnHand,
			COALESCE(MAX(s.sale_date), p.date_created) AS LastSaleDate,
			DATEDIFF(DAY, COALESCE(MAX(s.sale_date), p.date_created), CAST(GETDATE() AS DATE)) AS DaysSinceLastSale,
			COUNT(DISTINCT si.id) AS TotalTransactions,
			COALESCE(SUM(si.quantity_sold), 0) AS QtyMovedLastPeriod,
			COALESCE(ps.qty, 0) * COALESCE(p.avg_cost, 0) AS TotalValue
		FROM
			pos_products p
			LEFT JOIN pos_product_stocks ps ON p.item_number = ps.item_number AND ps.branch_id = @BranchId
			LEFT JOIN pos_sales_items si ON p.item_number = si.item_number AND si.branch_id = @BranchId
			LEFT JOIN pos_sales s ON si.sale_id = s.id AND s.branch_id = @BranchId
		WHERE
			p.deleted = 0
		GROUP BY
			p.id, p.avg_cost, p.unit_price, p.category_code, p.brand_code,
			p.date_created, ps.qty
	)
	SELECT
		COUNT(DISTINCT id) AS TotalMovingItems,
		SUM(QtyOnHand) AS TotalQtyOnHand,
		SUM(TotalValue) AS TotalInventoryValue,
		AVG(DaysSinceLastSale) AS AvgDaysSinceLastSale,
		MIN(DaysSinceLastSale) AS MinDaysSinceLastSale,
		MAX(DaysSinceLastSale) AS MaxDaysSinceLastSale,
		AVG(unit_price) AS AvgSellingPrice,
		COUNT(DISTINCT category_code) AS UniqueCategories,
		COUNT(DISTINCT brand_code) AS UniqueBrands
	FROM
		ProductAggregates
	WHERE
		DaysSinceLastSale <= @DaysThreshold;

END
GO
