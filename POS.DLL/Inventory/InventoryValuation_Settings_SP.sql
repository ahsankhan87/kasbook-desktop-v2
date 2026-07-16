-- =============================================================================
-- INVENTORY VALUATION — SETTINGS TABLE + HELPER STORED PROCEDURES
-- Project : Kasbook Desktop v3
-- Run after InventoryValuation_Schema.sql
-- =============================================================================

SET NOCOUNT ON;
GO

-- ---------------------------------------------------------------------------
-- Settings table: one row per branch (branch_id = 0 = company-wide default)
-- ---------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'pos_inv_valuation_settings')
BEGIN
	CREATE TABLE pos_inv_valuation_settings (
		id                      INT             IDENTITY(1,1) NOT NULL,
		branch_id               INT             NOT NULL DEFAULT 0,
		valuation_method        VARCHAR(10)     NOT NULL DEFAULT 'WAC',  -- WAC | FIFO | STANDARD
		cost_components         VARCHAR(50)     NOT NULL DEFAULT 'PURCHASE_ONLY',
																		  -- PURCHASE_ONLY | WITH_LANDED
		include_filter          VARCHAR(20)     NOT NULL DEFAULT 'ACTIVE_ONLY',
																		  -- ALL | ACTIVE_ONLY | EXCLUDE_ZERO
		cogs_account_id         INT             NULL,
		inventory_account_id    INT             NULL,
		post_per_product        BIT             NOT NULL DEFAULT 0,       -- 0=summary JV, 1=per-product JV
		updated_by              INT             NULL,
		updated_at              DATETIME        NOT NULL DEFAULT GETDATE(),
		CONSTRAINT PK_pos_inv_valuation_settings PRIMARY KEY CLUSTERED (id)
	);

	CREATE UNIQUE NONCLUSTERED INDEX UX_pos_inv_val_settings_branch
		ON pos_inv_valuation_settings (branch_id);

	-- Insert company-wide default row
	INSERT INTO pos_inv_valuation_settings (branch_id) VALUES (0);

	PRINT 'Created table: pos_inv_valuation_settings';
END
GO

-- ---------------------------------------------------------------------------
-- sp_GetInventoryValuationSettings
-- ---------------------------------------------------------------------------

IF OBJECT_ID('sp_GetInventoryValuationSettings', 'P') IS NOT NULL
	DROP PROCEDURE sp_GetInventoryValuationSettings;
GO

CREATE PROCEDURE sp_GetInventoryValuationSettings
	@BranchId INT = 0
AS
BEGIN
	SET NOCOUNT ON;

	-- Return branch-specific row; fall back to company default (branch_id=0)
	SELECT TOP 1
		s.id,
		s.branch_id,
		s.valuation_method,
		s.cost_components,
		s.include_filter,
		s.cogs_account_id,
		s.inventory_account_id,
		s.post_per_product,
		s.updated_at,
		cogs_a.name    AS cogs_account_name,
		cogs_a.code    AS cogs_account_code,
		inv_a.name     AS inventory_account_name,
		inv_a.code     AS inventory_account_code
	FROM pos_inv_valuation_settings s
	LEFT JOIN acc_accounts cogs_a ON cogs_a.id = s.cogs_account_id
	LEFT JOIN acc_accounts inv_a  ON inv_a.id  = s.inventory_account_id
	WHERE s.branch_id = @BranchId OR s.branch_id = 0
	ORDER BY
		CASE WHEN s.branch_id = @BranchId THEN 0 ELSE 1 END;
END
GO

PRINT 'Created: sp_GetInventoryValuationSettings';
GO

-- ---------------------------------------------------------------------------
-- sp_SaveInventoryValuationSettings
-- ---------------------------------------------------------------------------

IF OBJECT_ID('sp_SaveInventoryValuationSettings', 'P') IS NOT NULL
	DROP PROCEDURE sp_SaveInventoryValuationSettings;
GO

CREATE PROCEDURE sp_SaveInventoryValuationSettings
	@BranchId               INT,
	@ValuationMethod        VARCHAR(10),
	@CostComponents         VARCHAR(50),
	@IncludeFilter          VARCHAR(20),
	@CogsAccountId          INT,
	@InventoryAccountId     INT,
	@PostPerProduct         BIT,
	@UserId                 INT
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	IF EXISTS (SELECT 1 FROM pos_inv_valuation_settings WHERE branch_id = @BranchId)
	BEGIN
		UPDATE pos_inv_valuation_settings
		SET
			valuation_method     = @ValuationMethod,
			cost_components      = @CostComponents,
			include_filter       = @IncludeFilter,
			cogs_account_id      = @CogsAccountId,
			inventory_account_id = @InventoryAccountId,
			post_per_product     = @PostPerProduct,
			updated_by           = @UserId,
			updated_at           = GETDATE()
		WHERE branch_id = @BranchId;
	END
	ELSE
	BEGIN
		INSERT INTO pos_inv_valuation_settings
			(branch_id, valuation_method, cost_components, include_filter,
			 cogs_account_id, inventory_account_id, post_per_product, updated_by, updated_at)
		VALUES
			(@BranchId, @ValuationMethod, @CostComponents, @IncludeFilter,
			 @CogsAccountId, @InventoryAccountId, @PostPerProduct, @UserId, GETDATE());
	END

	SELECT @@ROWCOUNT AS rows_affected;
END
GO

PRINT 'Created: sp_SaveInventoryValuationSettings';
GO

-- ---------------------------------------------------------------------------
-- sp_GetAccountsByType  (helper: load account dropdowns by type keyword)
-- asset_type   = 1  (Current Assets)
-- expense_type = 5  (Expenses)
-- ---------------------------------------------------------------------------

IF OBJECT_ID('sp_GetAccountsByType', 'P') IS NOT NULL
	DROP PROCEDURE sp_GetAccountsByType;
GO

CREATE PROCEDURE sp_GetAccountsByType
	@AccountTypeId INT = 0    -- 0 = all
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		a.id,
		a.code,
		a.name,
		g.name AS group_name
	FROM acc_accounts a
	LEFT JOIN acc_groups g ON g.id = a.group_id
	WHERE
		a.is_active = 1
		AND (@AccountTypeId = 0 OR g.account_type_id = @AccountTypeId)
	ORDER BY a.code;
END
GO

PRINT 'Created: sp_GetAccountsByType';
GO

-- ---------------------------------------------------------------------------
-- sp_GetNextCOGSVoucherNo  — returns next JV number for COGS posting
-- ---------------------------------------------------------------------------

IF OBJECT_ID('sp_GetNextCOGSVoucherNo', 'P') IS NOT NULL
	DROP PROCEDURE sp_GetNextCOGSVoucherNo;
GO

CREATE PROCEDURE sp_GetNextCOGSVoucherNo
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @maxNo NVARCHAR(20);

	SELECT @maxNo = MAX(InvoiceNo)
	FROM acc_entries_header
	WHERE InvoiceNo LIKE 'COGS-%';

	IF @maxNo IS NULL
		SELECT 'COGS-000001' AS next_voucher_no;
	ELSE
	BEGIN
		DECLARE @num INT = TRY_CAST(SUBSTRING(@maxNo, 6, 10) AS INT);
		SELECT 'COGS-' + RIGHT('000000' + CAST(ISNULL(@num, 0) + 1 AS VARCHAR), 6) AS next_voucher_no;
	END
END
GO

PRINT 'Created: sp_GetNextCOGSVoucherNo';
GO

PRINT '=== Settings SPs complete ===';
GO
