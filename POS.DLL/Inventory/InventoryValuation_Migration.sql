-- =============================================================================
-- INVENTORY VALUATION - INITIAL DATA MIGRATION
-- Project : Kasbook Desktop v3
-- Module  : Inventory Valuation & Balance Sheet Integration
-- Created : 2025
-- =============================================================================
-- Run ONCE on the live database after InventoryValuation_Schema.sql.
-- Calculates initial avg_cost, last_cost for existing products from
-- purchase history stored in pos_purchase_detail / pos_products.
--
-- IMPORTANT: Run during a maintenance window.  Back up the database first.
-- =============================================================================

SET NOCOUNT ON;
GO

PRINT 'Starting inventory valuation migration...';
GO

-- ---------------------------------------------------------------------------
-- STEP 1: Populate avg_cost from existing purchase history (Weighted Average)
--         Uses pos_purchase_detail as the purchase line source.
--         Falls back to existing cost_price if no purchase history exists.
-- ---------------------------------------------------------------------------
;WITH PurchaseHistory AS (
	SELECT
		pd.item_number,
		-- WAC = total cost / total qty across all purchases
		CASE
			WHEN SUM(pd.qty) > 0
			THEN SUM(pd.qty * pd.unit_cost) / SUM(pd.qty)
			ELSE 0
		END AS calculated_avg_cost,
		-- Last cost = cost from the most recent purchase line
		MAX(CASE WHEN pd.rn = 1 THEN pd.unit_cost ELSE NULL END) AS last_purchase_cost
	FROM (
		SELECT
			pd_inner.item_number,
			pd_inner.qty,
			pd_inner.unit_cost,
			ROW_NUMBER() OVER (
				PARTITION BY pd_inner.item_number
				ORDER BY ph_inner.date_created DESC, pd_inner.id DESC
			) AS rn
		FROM pos_purchase_detail pd_inner
		INNER JOIN pos_purchase ph_inner ON ph_inner.id = pd_inner.purchase_id
		WHERE
			pd_inner.qty       > 0
			AND pd_inner.unit_cost > 0
			AND ph_inner.deleted   = 0
	) pd
	GROUP BY pd.item_number
)
UPDATE p
SET
	p.avg_cost  = CASE
					WHEN ph.calculated_avg_cost > 0 THEN ROUND(ph.calculated_avg_cost, 4)
					WHEN p.cost_price            > 0 THEN p.cost_price
					ELSE 0
				  END,
	p.last_cost = CASE
					WHEN ph.last_purchase_cost > 0 THEN ROUND(ph.last_purchase_cost, 4)
					WHEN p.cost_price          > 0 THEN p.cost_price
					ELSE 0
				  END
FROM pos_products p
LEFT JOIN PurchaseHistory ph ON ph.item_number = p.item_number
WHERE p.deleted = 0;

PRINT CONCAT('Step 1 complete. Products updated: ', @@ROWCOUNT);
GO

-- ---------------------------------------------------------------------------
-- STEP 2: Seed standard_cost = avg_cost for products where standard_cost = 0
--         (standard cost is set manually; this just gives a sensible starting point)
-- ---------------------------------------------------------------------------
UPDATE pos_products
SET    standard_cost = avg_cost
WHERE  standard_cost = 0
  AND  avg_cost      > 0
  AND  deleted       = 0;

PRINT CONCAT('Step 2 complete. standard_cost seeded for ', @@ROWCOUNT, ' products.');
GO

-- ---------------------------------------------------------------------------
-- STEP 3: Seed FIFO cost layers from existing purchase history.
--         Each purchase line becomes one layer.
--         remaining_qty is reduced by sales that occurred after the purchase.
--         NOTE: This is a best-effort seed.  If older sales data is incomplete,
--         remaining_qty may be inaccurate; adjust manually as needed.
-- ---------------------------------------------------------------------------

-- 3a. Insert one layer per purchase detail line (original_qty = qty received)
INSERT INTO inv_cost_layers
	(product_id, branch_id, purchase_date, purchase_ref_id, original_qty, remaining_qty, unit_cost, created_at)
SELECT
	p.id                                        AS product_id,
	ISNULL(ph.branch_id, 1)                     AS branch_id,
	CAST(ph.date_created AS DATE)               AS purchase_date,
	ph.id                                       AS purchase_ref_id,
	pd.qty                                      AS original_qty,
	pd.qty                                      AS remaining_qty,   -- will be adjusted in 3b
	CASE WHEN pd.unit_cost > 0 THEN pd.unit_cost ELSE p.avg_cost END AS unit_cost,
	ph.date_created                             AS created_at
FROM pos_purchase_detail pd
INNER JOIN pos_purchase    ph ON ph.id = pd.purchase_id
INNER JOIN pos_products     p  ON p.item_number = pd.item_number
WHERE
	pd.qty       > 0
	AND ph.deleted   = 0
	AND p.deleted    = 0
	AND pd.unit_cost >= 0
	AND NOT EXISTS (                            -- idempotent: skip if already loaded
		SELECT 1 FROM inv_cost_layers cl
		WHERE cl.product_id      = p.id
		  AND cl.purchase_ref_id = ph.id
	);

PRINT CONCAT('Step 3a complete. FIFO layers inserted: ', @@ROWCOUNT);
GO

-- 3b. Reduce remaining_qty in FIFO layers by applying cumulative sales (FIFO order)
--     Uses a cursor-free approach: build a running consumed total per product/branch,
--     then subtract from layers in chronological order.

-- Temporary table: total qty sold per product/branch (all historical sales)
IF OBJECT_ID('tempdb..#SoldQty') IS NOT NULL DROP TABLE #SoldQty;

SELECT
	p.id        AS product_id,
	ISNULL(sh.branch_id, 1) AS branch_id,
	SUM(sd.qty) AS total_sold
INTO #SoldQty
FROM pos_sales_detail sd
INNER JOIN pos_sales    sh ON sh.id = sd.sales_id
INNER JOIN pos_products p  ON p.item_number = sd.item_number
WHERE sh.deleted = 0
  AND sd.qty     > 0
GROUP BY p.id, ISNULL(sh.branch_id, 1);

-- Running layer total per product/branch (oldest-first cumulative qty)
;WITH LayerRunning AS (
	SELECT
		cl.layer_id,
		cl.product_id,
		cl.branch_id,
		cl.original_qty,
		SUM(cl.original_qty) OVER (
			PARTITION BY cl.product_id, cl.branch_id
			ORDER BY cl.purchase_date ASC, cl.layer_id ASC
			ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
		) AS cumulative_qty
	FROM inv_cost_layers cl
),
LayerConsumed AS (
	SELECT
		lr.layer_id,
		lr.product_id,
		lr.branch_id,
		lr.original_qty,
		lr.cumulative_qty,
		ISNULL(sq.total_sold, 0)                          AS total_sold,
		-- How much of this layer was consumed?
		CASE
			WHEN lr.cumulative_qty - lr.original_qty >= ISNULL(sq.total_sold, 0)
				THEN 0                                      -- layer is before the sold amount; fully consumed
			WHEN lr.cumulative_qty <= ISNULL(sq.total_sold, 0)
				THEN 0                                      -- entire layer consumed
			ELSE lr.cumulative_qty - ISNULL(sq.total_sold, 0)  -- partial remaining
		END AS new_remaining_qty
	FROM LayerRunning lr
	LEFT JOIN #SoldQty sq
		ON sq.product_id = lr.product_id
		AND sq.branch_id = lr.branch_id
)
UPDATE cl
SET cl.remaining_qty = CASE
						   WHEN lc.new_remaining_qty < 0 THEN 0
						   ELSE lc.new_remaining_qty
					   END
FROM inv_cost_layers cl
INNER JOIN LayerConsumed lc ON lc.layer_id = cl.layer_id;

PRINT CONCAT('Step 3b complete. FIFO remaining_qty adjusted. Rows: ', @@ROWCOUNT);
GO

DROP TABLE IF EXISTS #SoldQty;
GO

-- ---------------------------------------------------------------------------
-- STEP 4: Validation report — check products where avg_cost = 0 but stock > 0
--         These need manual review before going live.
-- ---------------------------------------------------------------------------
SELECT
	p.id,
	p.item_number,
	p.code,
	p.name,
	p.avg_cost,
	p.cost_price,
	ISNULL(SUM(ps.qty), 0) AS current_qty,
	'avg_cost is zero but stock exists — review manually' AS warning
FROM pos_products p
LEFT JOIN pos_product_stocks ps ON ps.item_number = p.item_number
WHERE p.deleted  = 0
  AND p.avg_cost = 0
  AND ISNULL(ps.qty, 0) > 0
GROUP BY p.id, p.item_number, p.code, p.name, p.avg_cost, p.cost_price
ORDER BY p.name;
GO

PRINT '=== Migration complete. Review warning rows above before going live. ===';
GO
