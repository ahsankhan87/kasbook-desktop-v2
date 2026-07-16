-- ============================================================
-- INVENTORY COSTING ENGINE  –  DDL + Stored Procedures
-- Target: SQL Server 2014+  |  Project: KasBook Desktop v3
-- ============================================================

-- ─────────────────────────────────────────────────────────────
-- 1.  inv_cost_layers  (FIFO purchase lots)
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'inv_cost_layers')
BEGIN
	CREATE TABLE dbo.inv_cost_layers (
		layer_id        INT             NOT NULL IDENTITY(1,1) CONSTRAINT PK_inv_cost_layers PRIMARY KEY,
		product_id      INT             NOT NULL,   -- pos_products.id
		item_number     NVARCHAR(50)    NOT NULL,
		branch_id       INT             NOT NULL DEFAULT 1,
		purchase_ref_id INT             NULL,       -- pos_purchases.id (NULL = migration seed)
		purchase_date   DATE            NOT NULL,
		original_qty    DECIMAL(18,6)   NOT NULL,
		remaining_qty   DECIMAL(18,6)   NOT NULL,
		unit_cost       DECIMAL(18,6)   NOT NULL,   -- base-currency cost per unit
		currency_id     INT             NULL DEFAULT 0,
		exchange_rate   DECIMAL(18,6)   NOT NULL DEFAULT 1,
		created_at      DATETIME        NOT NULL DEFAULT GETDATE(),
		CONSTRAINT CHK_icl_qty  CHECK (remaining_qty >= 0),
		CONSTRAINT CHK_icl_cost CHECK (unit_cost     >= 0)
	);

	-- Covering index used by FIFO consumption (oldest-first)
	CREATE INDEX IX_inv_cost_layers_prod_branch
		ON dbo.inv_cost_layers (product_id, branch_id, purchase_date, layer_id)
		WHERE remaining_qty > 0;

	-- Index for product look-up
	CREATE INDEX IX_inv_cost_layers_itemno
		ON dbo.inv_cost_layers (item_number, branch_id)
		WHERE remaining_qty > 0;
END
GO

-- ─────────────────────────────────────────────────────────────
-- 2.  inv_cogs_log  (audit trail of COGS postings)
-- ─────────────────────────────────────────────────────────────
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'inv_cogs_log')
BEGIN
	CREATE TABLE dbo.inv_cogs_log (
		cogs_log_id     INT             NOT NULL IDENTITY(1,1) CONSTRAINT PK_inv_cogs_log PRIMARY KEY,
		sale_invoice_no NVARCHAR(50)    NOT NULL,
		sale_id         INT             NULL,
		product_id      INT             NOT NULL,
		item_number     NVARCHAR(50)    NOT NULL,
		branch_id       INT             NOT NULL,
		qty_sold        DECIMAL(18,6)   NOT NULL,
		unit_cost       DECIMAL(18,6)   NOT NULL,
		cogs_amount     DECIMAL(18,6)   NOT NULL,
		costing_method  NVARCHAR(10)    NOT NULL DEFAULT 'WAC',  -- WAC | FIFO
		journal_ref     NVARCHAR(100)   NULL,
		posted_at       DATETIME        NOT NULL DEFAULT GETDATE(),
		posted_by       INT             NULL
	);

	CREATE INDEX IX_inv_cogs_log_invoice
		ON dbo.inv_cogs_log (sale_invoice_no, branch_id);
END
GO

-- ─────────────────────────────────────────────────────────────
-- SP 1:  sp_CalculateWACBulk
--        Returns WAC for EVERY active product in one query.
--        Used by InventoryCostingEngine.GetWACost() and bulk reporting.
-- ─────────────────────────────────────────────────────────────
IF OBJECT_ID('dbo.sp_CalculateWACBulk', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_CalculateWACBulk;
GO

CREATE PROCEDURE dbo.sp_CalculateWACBulk
	@BranchId   INT  = NULL,    -- NULL = all branches
	@AsOfDate   DATE = NULL     -- NULL = today
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @EffDate DATE = ISNULL(@AsOfDate, CAST(GETDATE() AS DATE));
	DECLARE @BId     INT  = ISNULL(@BranchId, 0);

	/*
	 * WAC formula (per product, per branch):
	 *   totalPurchaseValue = SUM(cost_price * quantity) from pos_purchases_items
	 *                        joined to pos_purchases WHERE purchase_date <= @EffDate
	 *   totalPurchaseQty   = SUM(quantity) from same
	 *   totalSalesQty      = SUM(quantity) from pos_sales_items (via pos_sales) WHERE sale_date <= @EffDate
	 *   currentQty         = totalPurchaseQty - totalSalesQty  (floored at 0)
	 *   WAC                = totalPurchaseValue / totalPurchaseQty  (when > 0, else 0)
	 *
	 * Opening stock value is approximated by current avg_cost * (first purchase qty back-fill).
	 * For simplicity (and speed at 50k products) we compute WAC from purchase history only;
	 * the pos_products.avg_cost column is maintained live after each purchase via
	 * RecalculateWACOnPurchase, so this SP serves as a verification / bulk snapshot tool.
	 */

	WITH Purchases AS (
		SELECT
			pi.item_number,
			ph.branch_id,
			SUM(pi.quantity)              AS total_purchase_qty,
			SUM(pi.quantity * pi.cost_price) AS total_purchase_value
		FROM dbo.pos_purchases_items pi
		INNER JOIN dbo.pos_purchases ph ON ph.id = pi.purchase_id
		WHERE ph.purchase_date <= @EffDate
		  AND ph.account <> 'Return'
		  AND (@BId = 0 OR ph.branch_id = @BId)
		GROUP BY pi.item_number, ph.branch_id
	),
	Sales AS (
		SELECT
			si.item_number,
			sh.branch_id,
			SUM(si.quantity_sold) AS total_sales_qty
		FROM dbo.pos_sales_items si
		INNER JOIN dbo.pos_sales sh ON sh.id = si.sale_id
		WHERE sh.sale_date <= @EffDate
		  AND sh.sale_type <> 'Return'
		  AND (@BId = 0 OR sh.branch_id = @BId)
		GROUP BY si.item_number, sh.branch_id
	),
	Returns AS (
		SELECT
			si.item_number,
			sh.branch_id,
			SUM(ABS(si.quantity)) AS total_return_qty
		FROM dbo.pos_purchases_items si
		INNER JOIN dbo.pos_purchases sh ON sh.id = si.purchase_id
		WHERE sh.purchase_date <= @EffDate
		  AND sh.account = 'Return'
		  AND (@BId = 0 OR sh.branch_id = @BId)
		GROUP BY si.item_number, sh.branch_id
	)
	SELECT
		p.id                          AS product_id,
		p.item_number,
		p.name                        AS product_name,
		p.code                        AS product_code,
		ISNULL(pu.branch_id, @BId)    AS branch_id,
		ISNULL(pu.total_purchase_qty, 0)                        AS total_purchase_qty,
		ISNULL(pu.total_purchase_value, 0)                      AS total_purchase_value,
		ISNULL(s.total_sales_qty, 0)                            AS total_sales_qty,
		ISNULL(r.total_return_qty, 0)                           AS total_return_qty,
		-- current stock = purchased - sold + returns (floored at 0)
		CASE WHEN ISNULL(pu.total_purchase_qty, 0)
				  - ISNULL(s.total_sales_qty,   0)
				  - ISNULL(r.total_return_qty,  0) < 0
			 THEN 0
			 ELSE ISNULL(pu.total_purchase_qty, 0)
				  - ISNULL(s.total_sales_qty,   0)
				  - ISNULL(r.total_return_qty,  0)
		END                                                      AS current_qty,
		-- WAC = total value / total purchased qty
		CASE WHEN ISNULL(pu.total_purchase_qty, 0) > 0
			 THEN ROUND(ISNULL(pu.total_purchase_value, 0)
						/ ISNULL(pu.total_purchase_qty, 1), 6)
			 ELSE ISNULL(p.avg_cost, 0)   -- fall back to stored avg_cost
		END                                                      AS wac_unit_cost,
		-- total inventory value at WAC
		CASE WHEN ISNULL(pu.total_purchase_qty, 0) > 0
			 THEN ROUND(
					(ISNULL(pu.total_purchase_value, 0)
					 / ISNULL(pu.total_purchase_qty, 1))
					* (CASE WHEN ISNULL(pu.total_purchase_qty, 0)
								 - ISNULL(s.total_sales_qty,   0)
								 - ISNULL(r.total_return_qty,  0) < 0
							THEN 0
							ELSE ISNULL(pu.total_purchase_qty, 0)
								 - ISNULL(s.total_sales_qty,   0)
								 - ISNULL(r.total_return_qty,  0)
					   END), 4)
			 ELSE ROUND(ISNULL(p.avg_cost, 0) * ISNULL(p.qty, 0), 4)
		END                                                      AS total_inventory_value,
		p.avg_cost                    AS stored_avg_cost,
		p.qty                         AS stored_qty,
		@EffDate                      AS as_of_date
	FROM dbo.pos_products p
	LEFT JOIN Purchases pu ON pu.item_number = p.item_number
	LEFT JOIN Sales     s  ON s.item_number  = p.item_number
						   AND (s.branch_id = pu.branch_id OR pu.branch_id IS NULL)
	LEFT JOIN Returns   r  ON r.item_number  = p.item_number
						   AND (r.branch_id = pu.branch_id OR pu.branch_id IS NULL)
	WHERE p.status = 1
	  AND p.item_type = 'Product'
	ORDER BY p.item_number;
END
GO

-- ─────────────────────────────────────────────────────────────
-- SP 2:  sp_GetFIFOLayers
--        Returns open FIFO layers for a product (oldest first).
--        Called before each sale to determine cost consumption.
-- ─────────────────────────────────────────────────────────────
IF OBJECT_ID('dbo.sp_GetFIFOLayers', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_GetFIFOLayers;
GO

CREATE PROCEDURE dbo.sp_GetFIFOLayers
	@ProductId  INT,
	@BranchId   INT  = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		layer_id,
		product_id,
		item_number,
		branch_id,
		purchase_ref_id,
		purchase_date,
		original_qty,
		remaining_qty,
		unit_cost,
		currency_id,
		exchange_rate,
		created_at
	FROM dbo.inv_cost_layers
	WHERE product_id  = @ProductId
	  AND remaining_qty > 0
	  AND (@BranchId IS NULL OR branch_id = @BranchId)
	ORDER BY purchase_date ASC, layer_id ASC;
END
GO

-- ─────────────────────────────────────────────────────────────
-- SP 3:  sp_PostCOGSBatch
--        Posts COGS journal entries for every line of one sale invoice.
--        Caller passes the sale invoice_no; the SP reads lines and
--        posts:  DR COGS Account  /  CR Inventory Account.
--        Idempotent: skips lines already logged in inv_cogs_log.
-- ─────────────────────────────────────────────────────────────
IF OBJECT_ID('dbo.sp_PostCOGSBatch', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_PostCOGSBatch;
GO

CREATE PROCEDURE dbo.sp_PostCOGSBatch
	@InvoiceNo          NVARCHAR(50),
	@COGSAccountId      INT,
	@InventoryAccountId INT,
	@UserId             INT  = 1,
	@BranchId           INT  = 1,
	@CostingMethod      NVARCHAR(10) = 'WAC',   -- 'WAC' or 'FIFO'
	@HeaderRef          NVARCHAR(100) = NULL     -- voucher / journal ref
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	-- Validate accounts
	IF @COGSAccountId = 0 OR @InventoryAccountId = 0
	BEGIN
		RAISERROR('COGS account and Inventory account must be set before posting.', 16, 1);
		RETURN;
	END

	-- Build a temp table of lines to post (skip already-logged lines)
	CREATE TABLE #cogs_lines (
		product_id   INT,
		item_number  NVARCHAR(50),
		qty_sold     DECIMAL(18,6),
		unit_cost    DECIMAL(18,6),
		cogs_amount  DECIMAL(18,6)
	);

	IF @CostingMethod = 'FIFO'
	BEGIN
		-- FIFO: use average of remaining layers weighted by qty consumed
		-- (layer consumption is done in C#; by the time SP is called, 
		--  inv_cogs_log already has unit_cost per line written by C# engine)
		INSERT INTO #cogs_lines (product_id, item_number, qty_sold, unit_cost, cogs_amount)
		SELECT
			cl.product_id,
			cl.item_number,
			cl.qty_sold,
			cl.unit_cost,
			cl.cogs_amount
		FROM dbo.inv_cogs_log cl
		WHERE cl.sale_invoice_no = @InvoiceNo
		  AND cl.branch_id       = @BranchId
		  AND NOT EXISTS (
			  SELECT 1 FROM dbo.inv_cogs_log x
			  WHERE x.sale_invoice_no = @InvoiceNo
				AND x.branch_id       = @BranchId
				AND x.journal_ref IS NOT NULL
		  );
	END
	ELSE  -- WAC (default)
	BEGIN
		INSERT INTO #cogs_lines (product_id, item_number, qty_sold, unit_cost, cogs_amount)
		SELECT
			p.id             AS product_id,
			si.item_number,
			ABS(si.quantity_sold) AS qty_sold,
			ISNULL(p.avg_cost, 0)                              AS unit_cost,
			ROUND(ABS(si.quantity_sold) * ISNULL(p.avg_cost, 0), 4) AS cogs_amount
		FROM dbo.pos_sales_items si
		INNER JOIN dbo.pos_sales sh ON sh.id = si.sale_id
		INNER JOIN dbo.pos_products p ON p.item_number = si.item_number
		WHERE sh.invoice_no = @InvoiceNo
		  AND sh.branch_id  = @BranchId
		  AND NOT EXISTS (
			  SELECT 1 FROM dbo.inv_cogs_log lg
			  WHERE lg.sale_invoice_no = @InvoiceNo
				AND lg.branch_id       = @BranchId
				AND lg.journal_ref IS NOT NULL
		  );
	END

	IF NOT EXISTS (SELECT 1 FROM #cogs_lines)
	BEGIN
		DROP TABLE #cogs_lines;
		RETURN;  -- already posted or nothing to post
	END

	DECLARE @TotalCOGS DECIMAL(18,6) = (SELECT SUM(cogs_amount) FROM #cogs_lines);
	DECLARE @SaleDate  DATE = (
		SELECT TOP 1 CAST(sale_date AS DATE)
		FROM dbo.pos_sales
		WHERE invoice_no = @InvoiceNo AND branch_id = @BranchId
	);
	DECLARE @Description NVARCHAR(255) = 'COGS – Sale ' + @InvoiceNo;
	DECLARE @VoucherRef  NVARCHAR(100) = ISNULL(@HeaderRef, 'COGS-' + @InvoiceNo);

	BEGIN TRANSACTION;

	BEGIN TRY
		-- ── COGS header entry ───────────────────────────────────
		DECLARE @HeaderId INT;

		INSERT INTO dbo.acc_entries_header
			(InvoiceNo, EntryDate, Narration, VoucherType,
			 user_id, branch_id, date_created)
		VALUES
			(@VoucherRef, @SaleDate, @Description, 'COGS',
			 @UserId, @BranchId, GETDATE());

		SET @HeaderId = SCOPE_IDENTITY();

		-- ── DR: COGS account ────────────────────────────────────
		EXEC dbo.sp_JournalsCrud
			@invoice_no  = @VoucherRef,
			@account_id  = @COGSAccountId,
			@entry_date  = @SaleDate,
			@debit       = @TotalCOGS,
			@credit      = 0,
			@description = @Description,
			@user_id     = @UserId,
			@branch_id   = @BranchId,
			@date_created = GETDATE,
			@customer_id  = 0,
			@supplier_id  = 0,
			@entry_id     = 0,
			@OperationType = '1';

		-- ── CR: Inventory account ───────────────────────────────
		EXEC dbo.sp_JournalsCrud
			@invoice_no  = @VoucherRef,
			@account_id  = @InventoryAccountId,
			@entry_date  = @SaleDate,
			@debit       = 0,
			@credit      = @TotalCOGS,
			@description = @Description,
			@user_id     = @UserId,
			@branch_id   = @BranchId,
			@date_created = GETDATE,
			@customer_id  = 0,
			@supplier_id  = 0,
			@entry_id     = 0,
			@OperationType = '1';

		-- ── Write / update cogs log with journal ref ────────────
		UPDATE dbo.inv_cogs_log
		SET    journal_ref = @VoucherRef
		WHERE  sale_invoice_no = @InvoiceNo
		  AND  branch_id       = @BranchId;

		-- If WAC (rows weren't pre-inserted), insert them now
		IF @CostingMethod = 'WAC'
		BEGIN
			INSERT INTO dbo.inv_cogs_log
				(sale_invoice_no, sale_id, product_id, item_number, branch_id,
				 qty_sold, unit_cost, cogs_amount, costing_method, journal_ref,
				 posted_at, posted_by)
			SELECT
				@InvoiceNo,
				sh.id,
				cl.product_id,
				cl.item_number,
				@BranchId,
				cl.qty_sold,
				cl.unit_cost,
				cl.cogs_amount,
				'WAC',
				@VoucherRef,
				GETDATE(),
				@UserId
			FROM #cogs_lines cl
			INNER JOIN dbo.pos_sales sh ON sh.invoice_no = @InvoiceNo
										AND sh.branch_id  = @BranchId
			WHERE NOT EXISTS (
				SELECT 1 FROM dbo.inv_cogs_log lg
				WHERE lg.sale_invoice_no = @InvoiceNo
				  AND lg.item_number     = cl.item_number
				  AND lg.branch_id       = @BranchId
			);
		END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
		DROP TABLE IF EXISTS #cogs_lines;
		THROW;
	END CATCH

	DROP TABLE IF EXISTS #cogs_lines;

	-- Return summary
	SELECT @VoucherRef AS journal_ref, @TotalCOGS AS total_cogs, @InvoiceNo AS invoice_no;
END
GO

-- ─────────────────────────────────────────────────────────────
-- SP 4:  sp_ReconcileInventoryValue
--        Compares qty × avg_cost per product against the
--        running balance of the Inventory GL account.
--        Returns variance rows for reconciliation report.
-- ─────────────────────────────────────────────────────────────
IF OBJECT_ID('dbo.sp_ReconcileInventoryValue', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_ReconcileInventoryValue;
GO

CREATE PROCEDURE dbo.sp_ReconcileInventoryValue
	@AsOfDate           DATE = NULL,
	@BranchId           INT  = NULL,
	@InventoryAccountId INT  = 0
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @EffDate DATE = ISNULL(@AsOfDate, CAST(GETDATE() AS DATE));
	DECLARE @BId     INT  = ISNULL(@BranchId, 0);

	-- ── Stock value per product (qty × avg_cost) ───────────────
	WITH StockValue AS (
		SELECT
			p.id              AS product_id,
			p.item_number,
			p.name            AS product_name,
			p.code            AS product_code,
			p.category_code,
			p.qty             AS qty_on_hand,
			p.avg_cost        AS unit_cost,
			ROUND(p.qty * p.avg_cost, 4) AS stock_value
		FROM dbo.pos_products p
		WHERE p.status    = 1
		  AND p.item_type = 'Product'
	),
	-- ── GL balance of Inventory account up to @EffDate ─────────
	GLBalance AS (
		SELECT
			ISNULL(SUM(ae.debit), 0) - ISNULL(SUM(ae.credit), 0) AS gl_inventory_balance
		FROM dbo.acc_entries ae
		WHERE ae.account_id  = @InventoryAccountId
		  AND ae.entry_date  <= @EffDate
		  AND (@BId = 0 OR ae.branch_id = @BId)
	)
	SELECT
		sv.product_id,
		sv.item_number,
		sv.product_name,
		sv.product_code,
		sv.category_code,
		sv.qty_on_hand,
		sv.unit_cost,
		sv.stock_value,
		gb.gl_inventory_balance,
		SUM(sv.stock_value) OVER ()                  AS total_stock_value,
		gb.gl_inventory_balance
		  - SUM(sv.stock_value) OVER ()              AS variance,
		@EffDate                                     AS as_of_date
	FROM StockValue sv
	CROSS JOIN GLBalance gb
	ORDER BY sv.product_name;
END
GO

-- ─────────────────────────────────────────────────────────────
-- Helper: sp_UpdateProductWAC
--   IMPORTANT: sp_Purchase_items is the AUTHORITATIVE owner of
--   avg_cost during live purchase posting. This SP is for forced
--   manual / batch WAC recalculation only (e.g. inventory adjustments,
--   admin corrections). Do NOT call this inside Insertpurchases.
--
--   Uses branch-specific qty from pos_product_stocks (same
--   source as sp_Purchase_items) so results are consistent.
-- ─────────────────────────────────────────────────────────────
IF OBJECT_ID('dbo.sp_UpdateProductWAC', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_UpdateProductWAC;
GO

CREATE PROCEDURE dbo.sp_UpdateProductWAC
	@ItemNumber     NVARCHAR(50),
	@BranchId       INT,
	@PurchaseQty    DECIMAL(18,6),
	@PurchaseCost   DECIMAL(18,6)   -- per-unit base-currency cost
AS
BEGIN
	SET NOCOUNT ON;

	-- Read branch-specific qty (same source as sp_Purchase_items)
	DECLARE @old_qty      DECIMAL(18,6) = COALESCE(
		(SELECT TOP 1 ISNULL(qty, 0) FROM dbo.pos_product_stocks
		 WHERE item_number = @ItemNumber AND branch_id = @BranchId), 0);

	DECLARE @old_avg_cost DECIMAL(18,6) = ISNULL(
		(SELECT avg_cost FROM dbo.pos_products WHERE item_number = @ItemNumber), 0);

	DECLARE @new_avg_cost DECIMAL(18,6);
	DECLARE @total_qty    DECIMAL(18,6) = @old_qty + @PurchaseQty;

	IF @old_avg_cost != 0
		SET @new_avg_cost = CASE WHEN @total_qty > 0
			THEN ROUND(((@old_qty * @old_avg_cost) + (@PurchaseQty * @PurchaseCost)) / @total_qty, 6)
			ELSE @PurchaseCost END;
	ELSE
		SET @new_avg_cost = @PurchaseCost;

	UPDATE dbo.pos_products
	SET avg_cost = @new_avg_cost
	WHERE item_number = @ItemNumber;

	SELECT @new_avg_cost AS new_wac, @total_qty AS total_qty;
END
GO

-- ─────────────────────────────────────────────────────────────
-- Helper: sp_InsertFIFOLayer
--   Inserts a new cost layer on purchase posting (C# calls this
--   inside the same transaction as the purchase insert).
-- ─────────────────────────────────────────────────────────────
IF OBJECT_ID('dbo.sp_InsertFIFOLayer', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_InsertFIFOLayer;
GO

CREATE PROCEDURE dbo.sp_InsertFIFOLayer
	@ProductId      INT,
	@ItemNumber     NVARCHAR(50),
	@BranchId       INT,
	@PurchaseRefId  INT,
	@PurchaseDate   DATE,
	@Qty            DECIMAL(18,6),
	@UnitCost       DECIMAL(18,6),
	@CurrencyId     INT  = 0,
	@ExchangeRate   DECIMAL(18,6) = 1
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.inv_cost_layers
		(product_id, item_number, branch_id, purchase_ref_id, purchase_date,
		 original_qty, remaining_qty, unit_cost, currency_id, exchange_rate, created_at)
	VALUES
		(@ProductId, @ItemNumber, @BranchId, @PurchaseRefId, @PurchaseDate,
		 @Qty, @Qty, @UnitCost, @CurrencyId, @ExchangeRate, GETDATE());

	SELECT SCOPE_IDENTITY() AS layer_id;
END
GO

-- ─────────────────────────────────────────────────────────────
-- Helper: sp_ConsumeFIFOLayer
--   Reduces remaining_qty of a specific layer during FIFO sale.
-- ─────────────────────────────────────────────────────────────
IF OBJECT_ID('dbo.sp_ConsumeFIFOLayer', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_ConsumeFIFOLayer;
GO

CREATE PROCEDURE dbo.sp_ConsumeFIFOLayer
	@LayerId    INT,
	@ConsumeQty DECIMAL(18,6)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.inv_cost_layers
	SET    remaining_qty = remaining_qty - @ConsumeQty
	WHERE  layer_id = @LayerId
	  AND  remaining_qty >= @ConsumeQty;

	IF @@ROWCOUNT = 0
		RAISERROR('Layer %d has insufficient remaining_qty for FIFO consumption.', 16, 1, @LayerId);
END
GO

