-- =============================================================================
-- INVENTORY VALUATION SCHEMA
-- Project : Kasbook Desktop v3
-- Module  : Inventory Valuation & Balance Sheet Integration
-- Created : 2025
-- =============================================================================
-- Run this script once against the target database.
-- Safe to re-run: all objects are created only if they do not already exist.
-- =============================================================================

SET NOCOUNT ON;
GO

-- ---------------------------------------------------------------------------
-- 1. ALTER pos_products: add costing columns (idempotent)
-- ---------------------------------------------------------------------------

IF NOT EXISTS (
	SELECT 1 FROM sys.columns
	WHERE object_id = OBJECT_ID('pos_products') AND name = 'avg_cost'
)
BEGIN
	ALTER TABLE pos_products
		ADD avg_cost DECIMAL(18,4) NOT NULL DEFAULT 0;
	PRINT 'Added: pos_products.avg_cost';
END
GO

IF NOT EXISTS (
	SELECT 1 FROM sys.columns
	WHERE object_id = OBJECT_ID('pos_products') AND name = 'last_cost'
)
BEGIN
	ALTER TABLE pos_products
		ADD last_cost DECIMAL(18,4) NOT NULL DEFAULT 0;
	PRINT 'Added: pos_products.last_cost';
END
GO

IF NOT EXISTS (
	SELECT 1 FROM sys.columns
	WHERE object_id = OBJECT_ID('pos_products') AND name = 'standard_cost'
)
BEGIN
	ALTER TABLE pos_products
		ADD standard_cost DECIMAL(18,4) NOT NULL DEFAULT 0;
	PRINT 'Added: pos_products.standard_cost';
END
GO

IF NOT EXISTS (
	SELECT 1 FROM sys.columns
	WHERE object_id = OBJECT_ID('pos_products') AND name = 'valuation_method'
)
BEGIN
	ALTER TABLE pos_products
		ADD valuation_method VARCHAR(10) NOT NULL DEFAULT 'WAC';
	PRINT 'Added: pos_products.valuation_method';
END
GO

-- ---------------------------------------------------------------------------
-- 2. FIFO Cost Layers: inv_cost_layers
-- ---------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'inv_cost_layers')
BEGIN
	CREATE TABLE inv_cost_layers (
		layer_id        BIGINT          IDENTITY(1,1) NOT NULL,
		product_id      INT             NOT NULL,           -- FK -> pos_products.id
		branch_id       INT             NOT NULL DEFAULT 1,
		purchase_date   DATE            NOT NULL,
		purchase_ref_id INT             NULL,               -- FK -> purchase header id
		original_qty    DECIMAL(18,3)   NOT NULL DEFAULT 0,
		remaining_qty   DECIMAL(18,3)   NOT NULL DEFAULT 0,
		unit_cost       DECIMAL(18,4)   NOT NULL DEFAULT 0,
		created_at      DATETIME        NOT NULL DEFAULT GETDATE(),
		CONSTRAINT PK_inv_cost_layers PRIMARY KEY CLUSTERED (layer_id)
	);

	-- Filtered index used by FIFO consumption queries (only open layers)
	CREATE NONCLUSTERED INDEX IX_inv_cost_layers_open
		ON inv_cost_layers (product_id, branch_id, purchase_date, layer_id)
		INCLUDE (remaining_qty, unit_cost)
		WHERE remaining_qty > 0;

	PRINT 'Created table: inv_cost_layers';
END
GO

-- ---------------------------------------------------------------------------
-- 3. Valuation Snapshots Header: inv_valuation_snapshots
-- ---------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'inv_valuation_snapshots')
BEGIN
	CREATE TABLE inv_valuation_snapshots (
		snapshot_id             INT             IDENTITY(1,1) NOT NULL,
		snapshot_date           DATE            NOT NULL,
		branch_id               INT             NOT NULL DEFAULT 0,   -- 0 = all branches
		total_inventory_value   DECIMAL(18,2)   NOT NULL DEFAULT 0,
		total_sku_count         INT             NOT NULL DEFAULT 0,
		total_qty               DECIMAL(18,3)   NOT NULL DEFAULT 0,
		method                  VARCHAR(10)     NOT NULL DEFAULT 'WAC',
		generated_by            INT             NULL,                  -- FK -> user id
		generated_at            DATETIME        NOT NULL DEFAULT GETDATE(),
		notes                   NVARCHAR(500)   NULL,
		CONSTRAINT PK_inv_valuation_snapshots PRIMARY KEY CLUSTERED (snapshot_id)
	);

	CREATE UNIQUE NONCLUSTERED INDEX UX_inv_valuation_snapshots_date_branch
		ON inv_valuation_snapshots (snapshot_date, branch_id, method);

	PRINT 'Created table: inv_valuation_snapshots';
END
GO

-- ---------------------------------------------------------------------------
-- 4. Valuation Snapshot Lines: inv_valuation_snapshot_lines
-- ---------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'inv_valuation_snapshot_lines')
BEGIN
	CREATE TABLE inv_valuation_snapshot_lines (
		line_id         BIGINT          IDENTITY(1,1) NOT NULL,
		snapshot_id     INT             NOT NULL,
		product_id      INT             NOT NULL,
		item_number     NVARCHAR(50)    NOT NULL DEFAULT '',
		branch_id       INT             NOT NULL DEFAULT 0,
		qty             DECIMAL(18,3)   NOT NULL DEFAULT 0,
		unit_cost       DECIMAL(18,4)   NOT NULL DEFAULT 0,
		total_value     DECIMAL(18,2)   NOT NULL DEFAULT 0,
		CONSTRAINT PK_inv_valuation_snapshot_lines PRIMARY KEY CLUSTERED (line_id),
		CONSTRAINT FK_inv_vslines_snapshot
			FOREIGN KEY (snapshot_id) REFERENCES inv_valuation_snapshots (snapshot_id)
			ON DELETE CASCADE
	);

	CREATE NONCLUSTERED INDEX IX_inv_vslines_snapshot
		ON inv_valuation_snapshot_lines (snapshot_id);

	CREATE NONCLUSTERED INDEX IX_inv_vslines_product
		ON inv_valuation_snapshot_lines (product_id, snapshot_id);

	PRINT 'Created table: inv_valuation_snapshot_lines';
END
GO

PRINT '=== Schema creation complete ===';
GO
