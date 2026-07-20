-- ============================================================
-- Fixed Assets Depreciation Support
-- Adds the depreciation run table and procedures required by the
-- C# depreciation engine.
-- ============================================================

SET NOCOUNT ON;
GO

-- ============================================================
-- Ensure fa_assets has the fields required by the depreciation engine.
-- The script is safe to rerun.
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'fa_assets' AND COLUMN_NAME = 'residual_value')
	ALTER TABLE dbo.fa_assets ADD residual_value DECIMAL(18,2) NOT NULL CONSTRAINT DF_fa_assets_residual_value DEFAULT (0);
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'fa_assets' AND COLUMN_NAME = 'dep_rate')
	ALTER TABLE dbo.fa_assets ADD dep_rate DECIMAL(9,4) NOT NULL CONSTRAINT DF_fa_assets_dep_rate DEFAULT (0);
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'fa_assets' AND COLUMN_NAME = 'dep_account_id')
	ALTER TABLE dbo.fa_assets ADD dep_account_id INT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'fa_assets' AND COLUMN_NAME = 'accum_dep_account_id')
	ALTER TABLE dbo.fa_assets ADD accum_dep_account_id INT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'fa_assets' AND COLUMN_NAME = 'accumulated_depreciation')
	ALTER TABLE dbo.fa_assets ADD accumulated_depreciation DECIMAL(18,2) NOT NULL CONSTRAINT DF_fa_assets_accumulated_depreciation DEFAULT (0);
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'fa_assets' AND COLUMN_NAME = 'current_wdv')
	ALTER TABLE dbo.fa_assets ADD current_wdv DECIMAL(18,2) NOT NULL CONSTRAINT DF_fa_assets_current_wdv DEFAULT (0);
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'fa_assets' AND COLUMN_NAME = 'status')
	ALTER TABLE dbo.fa_assets ADD status VARCHAR(30) NOT NULL CONSTRAINT DF_fa_assets_status DEFAULT ('Active');
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'fa_assets' AND COLUMN_NAME = 'disposal_date')
	ALTER TABLE dbo.fa_assets ADD disposal_date DATE NULL;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'fa_assets' AND COLUMN_NAME = 'disposal_proceeds')
	ALTER TABLE dbo.fa_assets ADD disposal_proceeds DECIMAL(18,2) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'fa_assets' AND COLUMN_NAME = 'last_dep_date')
	ALTER TABLE dbo.fa_assets ADD last_dep_date DATE NULL;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'fa_assets' AND COLUMN_NAME = 'created_by')
	ALTER TABLE dbo.fa_assets ADD created_by INT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'fa_assets' AND COLUMN_NAME = 'created_at')
	ALTER TABLE dbo.fa_assets ADD created_at DATETIME NOT NULL CONSTRAINT DF_fa_assets_created_at_legacy DEFAULT (GETDATE());
GO

-- ============================================================
-- fa_depreciation_runs
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'fa_depreciation_runs')
BEGIN
	CREATE TABLE dbo.fa_depreciation_runs (
		run_id              INT             NOT NULL IDENTITY(1,1),
		asset_id            INT             NOT NULL,
		period_date         DATE            NOT NULL,
		opening_wdv         DECIMAL(18,2)   NOT NULL,
		dep_amount          DECIMAL(18,2)   NOT NULL,
		closing_wdv         DECIMAL(18,2)   NOT NULL,
		voucher_id          INT             NULL,
		run_by              INT             NOT NULL,
		run_at              DATETIME        NOT NULL CONSTRAINT DF_fa_depreciation_runs_run_at DEFAULT (GETDATE()),

		CONSTRAINT PK_fa_depreciation_runs PRIMARY KEY (run_id),
		CONSTRAINT FK_fa_depreciation_runs_asset FOREIGN KEY (asset_id) REFERENCES dbo.fa_assets (asset_id),
		CONSTRAINT CK_fa_depreciation_runs_amount CHECK (dep_amount >= 0 AND opening_wdv >= 0 AND closing_wdv >= 0)
	);

	CREATE UNIQUE INDEX UX_fa_depreciation_runs_asset_period
		ON dbo.fa_depreciation_runs (asset_id, period_date);

	CREATE INDEX IX_fa_depreciation_runs_period_date
		ON dbo.fa_depreciation_runs (period_date);
END
GO

-- ============================================================
-- sp_FixedAsset_DepreciationRunExists
-- ============================================================
IF OBJECT_ID('dbo.sp_FixedAsset_DepreciationRunExists', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_FixedAsset_DepreciationRunExists;
GO

CREATE PROCEDURE dbo.sp_FixedAsset_DepreciationRunExists
	@AssetId    INT,
	@PeriodDate  DATE
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CanonicalPeriod DATE = DATEFROMPARTS(YEAR(@PeriodDate), MONTH(@PeriodDate), DAY(EOMONTH(@PeriodDate)));

	SELECT CASE WHEN EXISTS (
		SELECT 1
		FROM dbo.fa_depreciation_runs
		WHERE asset_id = @AssetId
		  AND period_date = @CanonicalPeriod
	) THEN 1 ELSE 0 END AS RunExists;
END;
GO

-- ============================================================
-- sp_FixedAsset_GetEligibleAssetsForDepreciation
-- Returns active assets that are due for depreciation.
-- ============================================================
IF OBJECT_ID('dbo.sp_FixedAsset_GetEligibleAssetsForDepreciation', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_FixedAsset_GetEligibleAssetsForDepreciation;
GO

CREATE PROCEDURE dbo.sp_FixedAsset_GetEligibleAssetsForDepreciation
	@PeriodDate DATE
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CanonicalPeriod DATE = DATEFROMPARTS(YEAR(@PeriodDate), MONTH(@PeriodDate), DAY(EOMONTH(@PeriodDate)));

	SELECT
		a.asset_id,
		a.asset_code,
		a.asset_name,
		a.category_id,
		c.category_name,
		a.purchase_date,
		a.cost,
		a.residual_value,
		a.useful_life_months,
		a.dep_method,
		a.dep_rate,
		a.dep_account_id,
		a.accum_dep_account_id,
		a.accumulated_depreciation,
		a.current_wdv,
		a.status,
		a.disposal_date,
		a.disposal_proceeds,
		a.last_dep_date,
		a.created_by,
		a.created_at
	FROM dbo.fa_assets a
	LEFT JOIN dbo.fa_categories c ON c.category_id = a.category_id
	WHERE ISNULL(a.status, 'Active') = 'Active'
	  AND ISNULL(a.disposal_date, '99991231') > @CanonicalPeriod
	  AND a.purchase_date <= @CanonicalPeriod
	  AND ISNULL(a.current_wdv, a.cost - ISNULL(a.accumulated_depreciation, 0)) > ISNULL(a.residual_value, 0)
	ORDER BY ISNULL(c.category_name, ''), a.asset_code;
END;
GO

-- ============================================================
-- sp_FixedAsset_GetMonthlyDepreciationPreview
-- Same rowset as eligible assets, intended for preview UI.
-- ============================================================
IF OBJECT_ID('dbo.sp_FixedAsset_GetMonthlyDepreciationPreview', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_FixedAsset_GetMonthlyDepreciationPreview;
GO

CREATE PROCEDURE dbo.sp_FixedAsset_GetMonthlyDepreciationPreview
	@PeriodDate DATE
AS
BEGIN
	SET NOCOUNT ON;

	EXEC dbo.sp_FixedAsset_GetEligibleAssetsForDepreciation @PeriodDate = @PeriodDate;
END;
GO

-- ============================================================
-- sp_FixedAsset_InsertDepreciationRun
-- Inserts one depreciation run row and returns the new run_id.
-- ============================================================
IF OBJECT_ID('dbo.sp_FixedAsset_InsertDepreciationRun', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_FixedAsset_InsertDepreciationRun;
GO

CREATE PROCEDURE dbo.sp_FixedAsset_InsertDepreciationRun
	@AssetId            INT,
	@PeriodDate         DATE,
	@OpeningWdv         DECIMAL(18,2),
	@DepAmount          DECIMAL(18,2),
	@ClosingWdv         DECIMAL(18,2),
	@VoucherId          INT = NULL,
	@RunBy              INT,
	@RunAt              DATETIME = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @CanonicalPeriod DATE = DATEFROMPARTS(YEAR(@PeriodDate), MONTH(@PeriodDate), DAY(EOMONTH(@PeriodDate)));
	DECLARE @EffectiveRunAt DATETIME = ISNULL(@RunAt, GETDATE());

	INSERT INTO dbo.fa_depreciation_runs
		(asset_id, period_date, opening_wdv, dep_amount, closing_wdv, voucher_id, run_by, run_at)
	VALUES
		(@AssetId, @CanonicalPeriod, @OpeningWdv, @DepAmount, @ClosingWdv, @VoucherId, @RunBy, @EffectiveRunAt);

	SELECT SCOPE_IDENTITY() AS run_id;
END;
GO

-- ============================================================
-- sp_FixedAsset_UpdateAssetDepreciationState
-- Updates accumulated depreciation, WDV, last depreciation date, and status.
-- ============================================================
IF OBJECT_ID('dbo.sp_FixedAsset_UpdateAssetDepreciationState', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_FixedAsset_UpdateAssetDepreciationState;
GO

CREATE PROCEDURE dbo.sp_FixedAsset_UpdateAssetDepreciationState
	@AssetId                INT,
	@AccumulatedDepreciation DECIMAL(18,2),
	@CurrentWdv              DECIMAL(18,2),
	@LastDepDate             DATE,
	@Status                  VARCHAR(30)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE dbo.fa_assets
	SET accumulated_depreciation = @AccumulatedDepreciation,
		current_wdv = @CurrentWdv,
		last_dep_date = @LastDepDate,
		status = CASE
			WHEN @CurrentWdv <= ISNULL(residual_value, 0) THEN 'Fully Depreciated'
			ELSE ISNULL(@Status, status)
		END
	WHERE asset_id = @AssetId;

	SELECT @@ROWCOUNT AS rows_affected;
END;
GO
