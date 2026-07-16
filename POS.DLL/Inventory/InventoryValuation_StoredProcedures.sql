-- =============================================================================
-- INVENTORY VALUATION STORED PROCEDURES
-- Project : Kasbook Desktop v3
-- Module  : Inventory Valuation & Balance Sheet Integration
-- Created : 2025
-- =============================================================================
-- Run after InventoryValuation_Schema.sql
-- =============================================================================

SET NOCOUNT ON;
GO

-- ===========================================================================
-- 1. sp_CalculateInventoryValueBulk
--    Returns per-product valuation using WAC or FIFO (set-based, no cursors).
--    Supports 50 k+ product catalogues efficiently.
-- ===========================================================================

IF OBJECT_ID('sp_CalculateInventoryValueBulk', 'P') IS NOT NULL
	DROP PROCEDURE sp_CalculateInventoryValueBulk;
GO

CREATE PROCEDURE sp_CalculateInventoryValueBulk
	@AsOfDate   DATE        = NULL,        -- NULL = today
	@Method     VARCHAR(10) = 'WAC',       -- 'WAC' | 'FIFO' | 'STANDARD'
	@BranchId   INT         = 0            -- 0 = all branches consolidated
AS
BEGIN
	SET NOCOUNT ON;

	IF @AsOfDate IS NULL
		SET @AsOfDate = CAST(GETDATE() AS DATE);

	-- -------------------------------------------------------------------------
	-- Branch stock CTE: current qty per product per branch (or consolidated)
	-- -------------------------------------------------------------------------
	;WITH BranchStock AS (
		SELECT
			ps.item_number,
			SUM(ps.qty) AS current_qty
		FROM pos_product_stocks ps
		WHERE
			(@BranchId = 0 OR ps.branch_id = @BranchId)
		GROUP BY ps.item_number
	),

	-- -------------------------------------------------------------------------
	-- Product base info
	-- -------------------------------------------------------------------------
	ProductBase AS (
		SELECT
			p.id            AS product_id,
			p.item_number,
			p.code,
			p.name,
			p.category_code,
			p.brand_code,
			p.valuation_method,
			p.avg_cost,
			p.standard_cost,
			p.last_cost,
			ISNULL(bs.current_qty, 0) AS current_qty
		FROM pos_products p
		LEFT JOIN BranchStock bs ON bs.item_number = p.item_number
		WHERE p.deleted = 0
	),

	-- -------------------------------------------------------------------------
	-- FIFO open layers aggregated per product
	-- -------------------------------------------------------------------------
	FifoCost AS (
		SELECT
			cl.product_id,
			SUM(cl.remaining_qty * cl.unit_cost)    AS fifo_total_value,
			SUM(cl.remaining_qty)                    AS fifo_total_qty,
			CASE
				WHEN SUM(cl.remaining_qty) > 0
				THEN SUM(cl.remaining_qty * cl.unit_cost) / SUM(cl.remaining_qty)
				ELSE 0
			END AS fifo_unit_cost
		FROM inv_cost_layers cl
		WHERE
			cl.remaining_qty > 0
			AND cl.purchase_date <= @AsOfDate
			AND (@BranchId = 0 OR cl.branch_id = @BranchId)
		GROUP BY cl.product_id
	)

	-- -------------------------------------------------------------------------
	-- Final result set
	-- -------------------------------------------------------------------------
	SELECT
		pb.product_id,
		pb.item_number,
		pb.code,
		pb.name,
		pb.category_code,
		pb.brand_code,
		pb.current_qty,

		-- Effective method: product-level override takes priority, else use parameter
		CASE
			WHEN pb.valuation_method = 'PRODUCT' THEN pb.valuation_method
			ELSE @Method
		END AS effective_method,

		-- Unit cost by method
		CASE @Method
			WHEN 'WAC'      THEN pb.avg_cost
			WHEN 'FIFO'     THEN ISNULL(fc.fifo_unit_cost, pb.avg_cost)
			WHEN 'STANDARD' THEN pb.standard_cost
			ELSE pb.avg_cost
		END AS unit_cost,

		-- Total value
		pb.current_qty *
		CASE @Method
			WHEN 'WAC'      THEN pb.avg_cost
			WHEN 'FIFO'     THEN ISNULL(fc.fifo_unit_cost, pb.avg_cost)
			WHEN 'STANDARD' THEN pb.standard_cost
			ELSE pb.avg_cost
		END AS total_value,

		-- Supplementary columns
		pb.avg_cost,
		pb.standard_cost,
		pb.last_cost,
		ISNULL(fc.fifo_unit_cost, 0) AS fifo_unit_cost,
		ISNULL(fc.fifo_total_qty, 0) AS fifo_layer_qty

	FROM ProductBase pb
	LEFT JOIN FifoCost fc ON fc.product_id = pb.product_id
	ORDER BY pb.category_code, pb.name;
END
GO

PRINT 'Created: sp_CalculateInventoryValueBulk';
GO

-- ===========================================================================
-- 2. sp_GetInventoryValueForBalanceSheet
--    Returns a SINGLE total value for the given date (used by BS report).
--    Current date  -> live calculation from avg_cost × stock qty.
--    Historical    -> reads from inv_valuation_snapshots (must snapshot period end).
-- ===========================================================================

IF OBJECT_ID('sp_GetInventoryValueForBalanceSheet', 'P') IS NOT NULL
	DROP PROCEDURE sp_GetInventoryValueForBalanceSheet;
GO

CREATE PROCEDURE sp_GetInventoryValueForBalanceSheet
	@AsOfDate   DATE        = NULL,
	@BranchId   INT         = 0,
	@Method     VARCHAR(10) = 'WAC'
AS
BEGIN
	SET NOCOUNT ON;

	IF @AsOfDate IS NULL
		SET @AsOfDate = CAST(GETDATE() AS DATE);

	DECLARE @today DATE = CAST(GETDATE() AS DATE);

	-- If the requested date is today (or in the future), compute live
	IF @AsOfDate >= @today
	BEGIN
		SELECT
			@AsOfDate                               AS as_of_date,
			'LIVE'                                  AS source,
			@Method                                 AS method,
			SUM(
				ISNULL(ps.branch_qty, 0) *
				CASE @Method
					WHEN 'WAC'      THEN p.avg_cost
					WHEN 'STANDARD' THEN p.standard_cost
					ELSE p.avg_cost
				END
			)                                       AS inventory_value,
			COUNT(DISTINCT p.id)                    AS sku_count,
			SUM(ISNULL(ps.branch_qty, 0))           AS total_qty
		FROM pos_products p
		LEFT JOIN (
			SELECT item_number, SUM(qty) AS branch_qty
			FROM pos_product_stocks
			WHERE @BranchId = 0 OR branch_id = @BranchId
			GROUP BY item_number
		) ps ON ps.item_number = p.item_number
		WHERE p.deleted = 0;
	END
	ELSE
	BEGIN
		-- Attempt to read from snapshot
		IF EXISTS (
			SELECT 1 FROM inv_valuation_snapshots
			WHERE snapshot_date = @AsOfDate
			  AND (@BranchId = 0 OR branch_id = @BranchId)
			  AND method = @Method
		)
		BEGIN
			SELECT TOP 1
				snapshot_date               AS as_of_date,
				'SNAPSHOT'                  AS source,
				method,
				total_inventory_value       AS inventory_value,
				total_sku_count             AS sku_count,
				total_qty
			FROM inv_valuation_snapshots
			WHERE snapshot_date = @AsOfDate
			  AND (@BranchId = 0 OR branch_id = @BranchId)
			  AND method = @Method
			ORDER BY generated_at DESC;
		END
		ELSE
		BEGIN
			-- No snapshot: warn and return NULL so caller can fallback gracefully
			SELECT
				@AsOfDate   AS as_of_date,
				'NO_SNAPSHOT' AS source,
				@Method     AS method,
				NULL        AS inventory_value,
				NULL        AS sku_count,
				NULL        AS total_qty;
		END
	END
END
GO

PRINT 'Created: sp_GetInventoryValueForBalanceSheet';
GO

-- ===========================================================================
-- 3. sp_TakeInventorySnapshot
--    Captures current inventory into snapshot tables.
--    Call during period closing (month-end / year-end).
--    Re-running for the same date+branch+method replaces previous snapshot.
-- ===========================================================================

IF OBJECT_ID('sp_TakeInventorySnapshot', 'P') IS NOT NULL
	DROP PROCEDURE sp_TakeInventorySnapshot;
GO

CREATE PROCEDURE sp_TakeInventorySnapshot
	@SnapshotDate   DATE        = NULL,
	@BranchId       INT         = 0,
	@Method         VARCHAR(10) = 'WAC',
	@UserId         INT         = NULL,
	@Notes          NVARCHAR(500) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	IF @SnapshotDate IS NULL
		SET @SnapshotDate = CAST(GETDATE() AS DATE);

	BEGIN TRANSACTION;
	BEGIN TRY

		-- Remove existing snapshot for same date/branch/method (idempotent re-run)
		DELETE s
		FROM inv_valuation_snapshots s
		WHERE s.snapshot_date = @SnapshotDate
		  AND s.branch_id     = @BranchId
		  AND s.method        = @Method;

		-- Insert new snapshot header (placeholder totals, updated after lines)
		DECLARE @SnapshotId INT;

		INSERT INTO inv_valuation_snapshots
			(snapshot_date, branch_id, total_inventory_value, total_sku_count,
			 total_qty, method, generated_by, generated_at, notes)
		VALUES
			(@SnapshotDate, @BranchId, 0, 0, 0, @Method, @UserId, GETDATE(), @Notes);

		SET @SnapshotId = SCOPE_IDENTITY();

		-- Build stock per product for the snapshot
		;WITH BranchStock AS (
			SELECT
				item_number,
				SUM(qty) AS current_qty
			FROM pos_product_stocks
			WHERE @BranchId = 0 OR branch_id = @BranchId
			GROUP BY item_number
		),
		FifoCost AS (
			SELECT
				cl.product_id,
				CASE
					WHEN SUM(cl.remaining_qty) > 0
					THEN SUM(cl.remaining_qty * cl.unit_cost) / SUM(cl.remaining_qty)
					ELSE 0
				END AS fifo_unit_cost
			FROM inv_cost_layers cl
			WHERE cl.remaining_qty > 0
			  AND cl.purchase_date <= @SnapshotDate
			  AND (@BranchId = 0 OR cl.branch_id = @BranchId)
			GROUP BY cl.product_id
		),
		ProductValuation AS (
			SELECT
				p.id        AS product_id,
				p.item_number,
				ISNULL(bs.current_qty, 0) AS qty,
				CASE @Method
					WHEN 'WAC'      THEN p.avg_cost
					WHEN 'FIFO'     THEN ISNULL(fc.fifo_unit_cost, p.avg_cost)
					WHEN 'STANDARD' THEN p.standard_cost
					ELSE p.avg_cost
				END AS unit_cost
			FROM pos_products p
			LEFT JOIN BranchStock bs ON bs.item_number = p.item_number
			LEFT JOIN FifoCost    fc ON fc.product_id  = p.id
			WHERE p.deleted = 0
		)
		INSERT INTO inv_valuation_snapshot_lines
			(snapshot_id, product_id, item_number, branch_id, qty, unit_cost, total_value)
		SELECT
			@SnapshotId,
			pv.product_id,
			pv.item_number,
			@BranchId,
			pv.qty,
			pv.unit_cost,
			pv.qty * pv.unit_cost AS total_value
		FROM ProductValuation pv
		WHERE pv.qty <> 0 OR pv.unit_cost <> 0;  -- skip zero-zero rows

		-- Update header aggregates from lines
		UPDATE inv_valuation_snapshots
		SET
			total_inventory_value = (SELECT SUM(total_value) FROM inv_valuation_snapshot_lines WHERE snapshot_id = @SnapshotId),
			total_sku_count       = (SELECT COUNT(1)         FROM inv_valuation_snapshot_lines WHERE snapshot_id = @SnapshotId),
			total_qty             = (SELECT SUM(qty)         FROM inv_valuation_snapshot_lines WHERE snapshot_id = @SnapshotId)
		WHERE snapshot_id = @SnapshotId;

		COMMIT TRANSACTION;

		-- Return the created snapshot summary
		SELECT
			snapshot_id,
			snapshot_date,
			branch_id,
			total_inventory_value,
			total_sku_count,
			total_qty,
			method,
			generated_at
		FROM inv_valuation_snapshots
		WHERE snapshot_id = @SnapshotId;

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
		DECLARE @msg NVARCHAR(2048) = ERROR_MESSAGE();
		RAISERROR(@msg, 16, 1);
	END CATCH
END
GO

PRINT 'Created: sp_TakeInventorySnapshot';
GO

-- ===========================================================================
-- 4. sp_InventoryCOGSReport
--    Period COGS & inventory movement reconciliation report.
--    Logic:
--      Opening Stock Value  (from snapshot at @FromDate - 1 day, else live)
--    + Purchases Value      (from pos_purchase_detail / inv_cost_layers in period)
--    - COGS                 (from pos_sales_detail in period, valued at avg_cost at time of sale)
--    = Closing Stock Value
--    Validation: Opening + Purchases - COGS should equal Closing (shown as variance).
-- ===========================================================================

IF OBJECT_ID('sp_InventoryCOGSReport', 'P') IS NOT NULL
	DROP PROCEDURE sp_InventoryCOGSReport;
GO

CREATE PROCEDURE sp_InventoryCOGSReport
	@FromDate   DATE,
	@ToDate     DATE,
	@BranchId   INT         = 0,
	@Method     VARCHAR(10) = 'WAC'
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @OpeningDate DATE = DATEADD(DAY, -1, @FromDate);

	-- -------------------------------------------------------------------------
	-- Opening stock value: try snapshot first, else live WAC
	-- -------------------------------------------------------------------------
	;WITH OpeningValues AS (
		SELECT
			sl.product_id,
			sl.item_number,
			sl.qty        AS opening_qty,
			sl.unit_cost  AS opening_unit_cost,
			sl.total_value AS opening_value
		FROM inv_valuation_snapshot_lines sl
		INNER JOIN inv_valuation_snapshots s
			ON s.snapshot_id = sl.snapshot_id
		WHERE s.snapshot_date = @OpeningDate
		  AND (s.branch_id = @BranchId OR @BranchId = 0)
		  AND s.method = @Method
	),

	-- -------------------------------------------------------------------------
	-- Purchases during period (from FIFO layers, which are inserted on receipt)
	-- -------------------------------------------------------------------------
	PurchaseValues AS (
		SELECT
			cl.product_id,
			SUM(cl.original_qty)                    AS purchased_qty,
			SUM(cl.original_qty * cl.unit_cost)     AS purchased_value
		FROM inv_cost_layers cl
		WHERE cl.purchase_date BETWEEN @FromDate AND @ToDate
		  AND (@BranchId = 0 OR cl.branch_id = @BranchId)
		GROUP BY cl.product_id
	),

	-- -------------------------------------------------------------------------
	-- COGS during period (from pos_sales_detail joined with avg_cost at product)
	-- Uses cost_price stored on sale line as the COGS value (as recorded at time of sale)
	-- -------------------------------------------------------------------------
	COGSValues AS (
		SELECT
			p.id        AS product_id,
			SUM(sd.qty) AS sold_qty,
			-- Use cost_price on sale line if available, else current avg_cost
			SUM(sd.qty * ISNULL(NULLIF(sd.cost_price, 0), p.avg_cost)) AS cogs_value
		FROM pos_sales_detail sd
		INNER JOIN pos_sales sh ON sh.id = sd.sales_id
		INNER JOIN pos_products p ON p.item_number = sd.item_number
		WHERE
			CAST(sh.date_created AS DATE) BETWEEN @FromDate AND @ToDate
			AND sh.deleted = 0
			AND (@BranchId = 0 OR sh.branch_id = @BranchId)
		GROUP BY p.id
	),

	-- -------------------------------------------------------------------------
	-- Closing stock value (live from pos_product_stocks × avg_cost)
	-- -------------------------------------------------------------------------
	ClosingValues AS (
		SELECT
			p.id        AS product_id,
			ISNULL(SUM(ps.qty), 0) AS closing_qty,
			CASE @Method
				WHEN 'WAC'      THEN p.avg_cost
				WHEN 'STANDARD' THEN p.standard_cost
				ELSE p.avg_cost
			END AS closing_unit_cost,
			ISNULL(SUM(ps.qty), 0) *
			CASE @Method
				WHEN 'WAC'      THEN p.avg_cost
				WHEN 'STANDARD' THEN p.standard_cost
				ELSE p.avg_cost
			END AS closing_value
		FROM pos_products p
		LEFT JOIN pos_product_stocks ps
			ON ps.item_number = p.item_number
			AND (@BranchId = 0 OR ps.branch_id = @BranchId)
		WHERE p.deleted = 0
		GROUP BY p.id, p.avg_cost, p.standard_cost
	)

	SELECT
		p.id                        AS product_id,
		p.item_number,
		p.code,
		p.name,
		p.category_code,

		-- Opening
		ISNULL(ov.opening_qty,    0) AS opening_qty,
		ISNULL(ov.opening_value,  0) AS opening_stock_value,

		-- Purchases
		ISNULL(pv.purchased_qty,  0) AS purchased_qty,
		ISNULL(pv.purchased_value,0) AS purchases_value,

		-- COGS
		ISNULL(cv.sold_qty,       0) AS sold_qty,
		ISNULL(cv.cogs_value,     0) AS cogs_value,

		-- Closing
		clv.closing_qty,
		clv.closing_unit_cost,
		clv.closing_value,

		-- Reconciliation: expected closing = Opening + Purchases - COGS
		(ISNULL(ov.opening_value, 0)
			+ ISNULL(pv.purchased_value, 0)
			- ISNULL(cv.cogs_value, 0))   AS expected_closing_value,

		-- Variance (ideally 0; non-zero indicates adjustments/write-offs/errors)
		clv.closing_value
			- (ISNULL(ov.opening_value, 0)
			   + ISNULL(pv.purchased_value, 0)
			   - ISNULL(cv.cogs_value, 0)) AS reconciliation_variance

	FROM pos_products p
	LEFT JOIN OpeningValues  ov  ON ov.product_id  = p.id
	LEFT JOIN PurchaseValues pv  ON pv.product_id  = p.id
	LEFT JOIN COGSValues     cv  ON cv.product_id  = p.id
	LEFT JOIN ClosingValues  clv ON clv.product_id = p.id
	WHERE
		-- Only show products with activity in the period or non-zero closing stock
		(ISNULL(ov.opening_value,  0) <> 0
		 OR ISNULL(pv.purchased_value, 0) <> 0
		 OR ISNULL(cv.cogs_value,   0) <> 0
		 OR clv.closing_value          <> 0)
	ORDER BY p.category_code, p.name;
END
GO

PRINT 'Created: sp_InventoryCOGSReport';
GO

-- ===========================================================================
-- 5. sp_NegativeStockAlert
--    Returns all products where current branch qty < 0.
--    Negative qty signals posting sequence errors (sales before receipts).
-- ===========================================================================

IF OBJECT_ID('sp_NegativeStockAlert', 'P') IS NOT NULL
	DROP PROCEDURE sp_NegativeStockAlert;
GO

CREATE PROCEDURE sp_NegativeStockAlert
	@BranchId   INT = 0       -- 0 = all branches
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		p.id            AS product_id,
		p.item_number,
		p.code,
		p.name,
		p.category_code,
		p.brand_code,
		p.avg_cost,
		ps.branch_id,
		b.name          AS branch_name,
		ps.qty          AS current_qty,
		ps.qty * p.avg_cost AS negative_value_exposure,
		p.last_cost,
		p.valuation_method
	FROM pos_products p
	INNER JOIN pos_product_stocks ps ON ps.item_number = p.item_number
	LEFT  JOIN pos_branches b        ON b.id = ps.branch_id
	WHERE
		ps.qty < 0
		AND p.deleted = 0
		AND (@BranchId = 0 OR ps.branch_id = @BranchId)
	ORDER BY ps.qty ASC;   -- worst (most negative) first
END
GO

PRINT 'Created: sp_NegativeStockAlert';
GO

PRINT '=== All stored procedures created successfully ===';
GO
