-- ============================================================
-- Fixed Assets Module — Schema + Reporting Stored Procedures
-- Target: SQL Server 2014+
-- Scope: asset master, categories, locations, depreciation history, disposal history
-- ============================================================

SET NOCOUNT ON;
GO

-- ============================================================
-- 1. fa_categories
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'fa_categories')
BEGIN
	CREATE TABLE dbo.fa_categories (
		category_id                 INT             NOT NULL IDENTITY(1,1),
		category_code               VARCHAR(20)     NOT NULL,
		category_name               NVARCHAR(150)   NOT NULL,
		depreciation_method         VARCHAR(30)     NOT NULL CONSTRAINT DF_fa_categories_depreciation_method DEFAULT ('STRAIGHT_LINE'),
		useful_life_months          INT             NOT NULL CONSTRAINT DF_fa_categories_useful_life_months DEFAULT (60),
		annual_depreciation_rate    DECIMAL(9,4)    NULL,
		is_active                   BIT             NOT NULL CONSTRAINT DF_fa_categories_is_active DEFAULT (1),
		created_at                  DATETIME        NOT NULL CONSTRAINT DF_fa_categories_created_at DEFAULT (GETDATE()),

		CONSTRAINT PK_fa_categories PRIMARY KEY (category_id),
		CONSTRAINT UQ_fa_categories_code UNIQUE (category_code),
		CONSTRAINT CK_fa_categories_useful_life CHECK (useful_life_months > 0),
		CONSTRAINT CK_fa_categories_rate CHECK (annual_depreciation_rate IS NULL OR annual_depreciation_rate >= 0)
	);
END
GO

-- ============================================================
-- 2. fa_locations
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'fa_locations')
BEGIN
	CREATE TABLE dbo.fa_locations (
		location_id                 INT             NOT NULL IDENTITY(1,1),
		location_code               VARCHAR(20)     NOT NULL,
		location_name               NVARCHAR(150)   NOT NULL,
		location_type               VARCHAR(30)     NOT NULL CONSTRAINT DF_fa_locations_location_type DEFAULT ('LOCATION'),
		parent_location_id          INT             NULL,
		is_active                   BIT             NOT NULL CONSTRAINT DF_fa_locations_is_active DEFAULT (1),
		created_at                  DATETIME        NOT NULL CONSTRAINT DF_fa_locations_created_at DEFAULT (GETDATE()),

		CONSTRAINT PK_fa_locations PRIMARY KEY (location_id),
		CONSTRAINT UQ_fa_locations_code UNIQUE (location_code),
		CONSTRAINT CK_fa_locations_type CHECK (location_type IN ('DEPARTMENT', 'LOCATION', 'SITE', 'BRANCH')),
		CONSTRAINT FK_fa_locations_parent FOREIGN KEY (parent_location_id) REFERENCES dbo.fa_locations (location_id)
	);

	CREATE INDEX IX_fa_locations_parent_location_id ON dbo.fa_locations (parent_location_id);
END
GO

-- ============================================================
-- 3. fa_assets
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'fa_assets')
BEGIN
	CREATE TABLE dbo.fa_assets (
		asset_id                    INT             NOT NULL IDENTITY(1,1),
		asset_code                  VARCHAR(50)     NOT NULL,
		asset_name                  NVARCHAR(200)   NOT NULL,
		category_id                 INT             NOT NULL,
		location_id                 INT             NULL,
		serial_number               NVARCHAR(100)   NULL,
		purchase_date               DATE            NOT NULL,
		cost                        DECIMAL(18,2)   NOT NULL CONSTRAINT DF_fa_assets_cost DEFAULT (0),
		dep_method                  VARCHAR(30)     NOT NULL CONSTRAINT DF_fa_assets_dep_method DEFAULT ('STRAIGHT_LINE'),
		useful_life_months          INT             NOT NULL CONSTRAINT DF_fa_assets_useful_life_months DEFAULT (60),
		salvage_value               DECIMAL(18,2)   NOT NULL CONSTRAINT DF_fa_assets_salvage_value DEFAULT (0),
		replacement_cost            DECIMAL(18,2)   NULL,
		is_active                   BIT             NOT NULL CONSTRAINT DF_fa_assets_is_active DEFAULT (1),
		is_disposed                 BIT             NOT NULL CONSTRAINT DF_fa_assets_is_disposed DEFAULT (0),
		disposed_on                 DATE            NULL,
		disposal_method             VARCHAR(50)     NULL,
		disposal_proceeds           DECIMAL(18,2)   NULL,
		notes                       NVARCHAR(500)   NULL,
		created_at                  DATETIME        NOT NULL CONSTRAINT DF_fa_assets_created_at DEFAULT (GETDATE()),
		updated_at                  DATETIME        NULL,

		CONSTRAINT PK_fa_assets PRIMARY KEY (asset_id),
		CONSTRAINT UQ_fa_assets_code UNIQUE (asset_code),
		CONSTRAINT FK_fa_assets_category FOREIGN KEY (category_id) REFERENCES dbo.fa_categories (category_id),
		CONSTRAINT FK_fa_assets_location FOREIGN KEY (location_id) REFERENCES dbo.fa_locations (location_id),
		CONSTRAINT CK_fa_assets_cost CHECK (cost >= 0),
		CONSTRAINT CK_fa_assets_salvage CHECK (salvage_value >= 0),
		CONSTRAINT CK_fa_assets_life CHECK (useful_life_months > 0),
		CONSTRAINT CK_fa_assets_disposal CHECK (disposed_on IS NULL OR disposed_on >= purchase_date)
	);

	CREATE INDEX IX_fa_assets_category_id ON dbo.fa_assets (category_id);
	CREATE INDEX IX_fa_assets_location_id ON dbo.fa_assets (location_id);
	CREATE INDEX IX_fa_assets_disposed_on ON dbo.fa_assets (disposed_on);
END
GO

-- ============================================================
-- 4. fa_asset_depreciation
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'fa_asset_depreciation')
BEGIN
	CREATE TABLE dbo.fa_asset_depreciation (
		depreciation_id             INT             NOT NULL IDENTITY(1,1),
		asset_id                    INT             NOT NULL,
		run_date                    DATE            NOT NULL,
		period_start                DATE            NULL,
		period_end                  DATE            NULL,
		depreciation_amount         DECIMAL(18,2)   NOT NULL CONSTRAINT DF_fa_asset_depreciation_amount DEFAULT (0),
		opening_accum_dep           DECIMAL(18,2)   NULL,
		closing_accum_dep           DECIMAL(18,2)   NULL,
		created_at                  DATETIME        NOT NULL CONSTRAINT DF_fa_asset_depreciation_created_at DEFAULT (GETDATE()),

		CONSTRAINT PK_fa_asset_depreciation PRIMARY KEY (depreciation_id),
		CONSTRAINT FK_fa_asset_depreciation_asset FOREIGN KEY (asset_id) REFERENCES dbo.fa_assets (asset_id),
		CONSTRAINT CK_fa_asset_depreciation_amount CHECK (depreciation_amount >= 0)
	);

	CREATE INDEX IX_fa_asset_depreciation_asset_run ON dbo.fa_asset_depreciation (asset_id, run_date);
END
GO

-- ============================================================
-- 5. fa_asset_disposals
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'fa_asset_disposals')
BEGIN
	CREATE TABLE dbo.fa_asset_disposals (
		disposal_id                 INT             NOT NULL IDENTITY(1,1),
		asset_id                    INT             NOT NULL,
		disposal_date               DATE            NOT NULL,
		disposal_method             VARCHAR(50)     NOT NULL,
		disposal_proceeds           DECIMAL(18,2)   NOT NULL CONSTRAINT DF_fa_asset_disposals_proceeds DEFAULT (0),
		disposal_cost               DECIMAL(18,2)   NOT NULL CONSTRAINT DF_fa_asset_disposals_cost DEFAULT (0),
		accum_dep_at_disposal       DECIMAL(18,2)   NOT NULL CONSTRAINT DF_fa_asset_disposals_accum DEFAULT (0),
		wdv_at_disposal             DECIMAL(18,2)   NOT NULL CONSTRAINT DF_fa_asset_disposals_wdv DEFAULT (0),
		notes                       NVARCHAR(500)   NULL,
		created_at                  DATETIME        NOT NULL CONSTRAINT DF_fa_asset_disposals_created_at DEFAULT (GETDATE()),

		CONSTRAINT PK_fa_asset_disposals PRIMARY KEY (disposal_id),
		CONSTRAINT FK_fa_asset_disposals_asset FOREIGN KEY (asset_id) REFERENCES dbo.fa_assets (asset_id),
		CONSTRAINT CK_fa_asset_disposals_values CHECK (disposal_proceeds >= 0 AND disposal_cost >= 0 AND accum_dep_at_disposal >= 0 AND wdv_at_disposal >= 0)
	);

	CREATE INDEX IX_fa_asset_disposals_asset_date ON dbo.fa_asset_disposals (asset_id, disposal_date);
END
GO

-- ============================================================
-- Helper notes:
--   The reporting procedures below derive fiscal-year windows from the
--   company setup month when available. If no setup row exists, July is used.
-- ============================================================

-- ============================================================
-- sp_FixedAssetSchedule
-- Primary fixed assets report — one row per asset
-- ============================================================
IF OBJECT_ID('dbo.sp_FixedAssetSchedule', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_FixedAssetSchedule;
GO

CREATE PROCEDURE dbo.sp_FixedAssetSchedule
	@AsOfDate   DATE,
	@CategoryId  INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @EffDate DATE = ISNULL(@AsOfDate, CAST(GETDATE() AS DATE));
	DECLARE @FyStartMonth INT = ISNULL((SELECT TOP 1 financial_year_start_month FROM dbo.pos_companies WHERE financial_year_start_month BETWEEN 1 AND 12), 7);
	DECLARE @FiscalStartDate DATE = DATEFROMPARTS(
		CASE WHEN MONTH(@EffDate) < @FyStartMonth THEN YEAR(@EffDate) - 1 ELSE YEAR(@EffDate) END,
		@FyStartMonth,
		1
	);

	WITH Dep AS (
		SELECT
			d.asset_id,
			SUM(CASE WHEN d.run_date < @FiscalStartDate THEN d.depreciation_amount ELSE 0 END) AS opening_accum_dep,
			SUM(CASE WHEN d.run_date >= @FiscalStartDate AND d.run_date <= @EffDate THEN d.depreciation_amount ELSE 0 END) AS depreciation_this_year,
			SUM(CASE WHEN d.run_date <= @EffDate THEN d.depreciation_amount ELSE 0 END) AS total_dep_to_date
		FROM dbo.fa_asset_depreciation d
		GROUP BY d.asset_id
	),
	Disp AS (
		SELECT
			d.asset_id,
			MAX(CASE WHEN d.disposal_date <= @EffDate THEN d.disposal_date END) AS disposal_date,
			MAX(CASE WHEN d.disposal_date <= @EffDate THEN d.disposal_method END) AS disposal_method,
			MAX(CASE WHEN d.disposal_date <= @EffDate THEN d.disposal_proceeds END) AS disposal_proceeds,
			MAX(CASE WHEN d.disposal_date <= @EffDate THEN d.disposal_cost END) AS disposal_cost,
			MAX(CASE WHEN d.disposal_date <= @EffDate THEN d.accum_dep_at_disposal END) AS accum_dep_at_disposal,
			MAX(CASE WHEN d.disposal_date <= @EffDate THEN d.wdv_at_disposal END) AS wdv_at_disposal
		FROM dbo.fa_asset_disposals d
		GROUP BY d.asset_id
	)
	SELECT
		a.asset_code AS AssetCode,
		a.asset_name AS AssetName,
		c.category_name AS Category,
		a.purchase_date AS PurchaseDate,
		a.cost AS Cost,
		ISNULL(dep.opening_accum_dep, 0) AS OpeningAccumDep,
		CASE
			WHEN a.purchase_date >= @FiscalStartDate AND a.purchase_date <= @EffDate THEN a.cost
			ELSE 0
		END AS AdditionsThisYear,
		CASE
			WHEN disp.disposal_date >= @FiscalStartDate AND disp.disposal_date <= @EffDate THEN ISNULL(disp.disposal_cost, a.cost)
			ELSE 0
		END AS DisposalsCost,
		ISNULL(dep.depreciation_this_year, 0) AS DepreciationThisYear,
		CASE
			WHEN disp.disposal_date IS NOT NULL AND disp.disposal_date <= @EffDate THEN ISNULL(disp.accum_dep_at_disposal, 0)
			ELSE CASE WHEN ISNULL(dep.total_dep_to_date, 0) > (a.cost - a.salvage_value) THEN (a.cost - a.salvage_value) ELSE ISNULL(dep.total_dep_to_date, 0) END
		END AS ClosingAccumDep,
		CASE
			WHEN disp.disposal_date IS NOT NULL AND disp.disposal_date <= @EffDate THEN 0
			ELSE CASE WHEN (a.cost - CASE WHEN ISNULL(dep.total_dep_to_date, 0) > (a.cost - a.salvage_value) THEN (a.cost - a.salvage_value) ELSE ISNULL(dep.total_dep_to_date, 0) END) < 0 THEN 0 ELSE (a.cost - CASE WHEN ISNULL(dep.total_dep_to_date, 0) > (a.cost - a.salvage_value) THEN (a.cost - a.salvage_value) ELSE ISNULL(dep.total_dep_to_date, 0) END) END
		END AS ClosingWDV,
		a.dep_method AS DepMethod,
		a.useful_life_months AS UsefulLife,
		CASE
			WHEN disp.disposal_date IS NOT NULL AND disp.disposal_date <= @EffDate THEN 0
			ELSE CASE
				WHEN DATEDIFF(MONTH, @EffDate, DATEADD(MONTH, a.useful_life_months, a.purchase_date)) < 0 THEN 0
				ELSE DATEDIFF(MONTH, @EffDate, DATEADD(MONTH, a.useful_life_months, a.purchase_date))
			END
		END AS RemainingLife
	FROM dbo.fa_assets a
	INNER JOIN dbo.fa_categories c ON c.category_id = a.category_id
	LEFT JOIN Dep dep ON dep.asset_id = a.asset_id
	LEFT JOIN Disp disp ON disp.asset_id = a.asset_id
	WHERE (@CategoryId IS NULL OR a.category_id = @CategoryId)
	ORDER BY c.category_name, a.asset_code;
END;
GO

-- ============================================================
-- sp_DepreciationSummaryByCategory
-- Grouped summary by fa_categories
-- ============================================================
IF OBJECT_ID('dbo.sp_DepreciationSummaryByCategory', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_DepreciationSummaryByCategory;
GO

CREATE PROCEDURE dbo.sp_DepreciationSummaryByCategory
	@FromDate   DATE,
	@ToDate     DATE
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @StartDate DATE = ISNULL(@FromDate, CAST(GETDATE() AS DATE));
	DECLARE @EndDate DATE = ISNULL(@ToDate, CAST(GETDATE() AS DATE));

	WITH DepPeriod AS (
		SELECT
			a.category_id,
			SUM(d.depreciation_amount) AS total_dep_run
		FROM dbo.fa_assets a
		INNER JOIN dbo.fa_asset_depreciation d ON d.asset_id = a.asset_id
		WHERE d.run_date BETWEEN @StartDate AND @EndDate
		GROUP BY a.category_id
	),
	DepToDate AS (
		SELECT
			a.category_id,
			a.asset_id,
			SUM(CASE WHEN d.run_date <= @EndDate THEN d.depreciation_amount ELSE 0 END) AS dep_to_date
		FROM dbo.fa_assets a
		LEFT JOIN dbo.fa_asset_depreciation d ON d.asset_id = a.asset_id
		GROUP BY a.category_id, a.asset_id
	),
	DispToDate AS (
		SELECT
			d.asset_id,
			MAX(CASE WHEN d.disposal_date <= @EndDate THEN d.wdv_at_disposal END) AS wdv_at_disposal,
			MAX(CASE WHEN d.disposal_date <= @EndDate THEN d.disposal_date END) AS disposal_date
		FROM dbo.fa_asset_disposals d
		GROUP BY d.asset_id
	)
	SELECT
		c.category_name AS CategoryName,
		COUNT(a.asset_id) AS AssetCount,
		SUM(a.cost) AS TotalCost,
		ISNULL(dp.total_dep_run, 0) AS TotalDepreciationRun,
		SUM(CASE
			WHEN disp.disposal_date IS NOT NULL THEN ISNULL(dtd.dep_to_date, 0)
			ELSE CASE WHEN ISNULL(dtd.dep_to_date, 0) > (a.cost - a.salvage_value) THEN (a.cost - a.salvage_value) ELSE ISNULL(dtd.dep_to_date, 0) END
		END) AS TotalAccumDep,
		SUM(CASE
			WHEN disp.disposal_date IS NOT NULL THEN 0
			ELSE CASE WHEN (a.cost - CASE WHEN ISNULL(dtd.dep_to_date, 0) > (a.cost - a.salvage_value) THEN (a.cost - a.salvage_value) ELSE ISNULL(dtd.dep_to_date, 0) END) < 0 THEN 0 ELSE (a.cost - CASE WHEN ISNULL(dtd.dep_to_date, 0) > (a.cost - a.salvage_value) THEN (a.cost - a.salvage_value) ELSE ISNULL(dtd.dep_to_date, 0) END) END
		END) AS TotalWDV,
		AVG(CASE WHEN a.useful_life_months > 0 THEN (1200.0 / a.useful_life_months) ELSE 0 END) AS AvgDepRate
	FROM dbo.fa_assets a
	INNER JOIN dbo.fa_categories c ON c.category_id = a.category_id
	LEFT JOIN DepPeriod dp ON dp.category_id = a.category_id
	LEFT JOIN DepToDate dtd ON dtd.asset_id = a.asset_id
	LEFT JOIN DispToDate disp ON disp.asset_id = a.asset_id
	GROUP BY c.category_name, dp.total_dep_run
	ORDER BY c.category_name;
END;
GO

-- ============================================================
-- sp_AssetDisposalReport
-- Disposed assets between the requested dates
-- ============================================================
IF OBJECT_ID('dbo.sp_AssetDisposalReport', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_AssetDisposalReport;
GO

CREATE PROCEDURE dbo.sp_AssetDisposalReport
	@FromDate   DATE,
	@ToDate     DATE
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @StartDate DATE = ISNULL(@FromDate, CAST(GETDATE() AS DATE));
	DECLARE @EndDate DATE = ISNULL(@ToDate, CAST(GETDATE() AS DATE));

	SELECT
		a.asset_code AS AssetCode,
		a.asset_name AS AssetName,
		c.category_name AS Category,
		a.purchase_date AS PurchaseDate,
		a.cost AS Cost,
		d.accum_dep_at_disposal AS AccumDepAtDisposal,
		d.wdv_at_disposal AS WDVAtDisposal,
		d.disposal_date AS DisposalDate,
		d.disposal_method AS DisposalMethod,
		d.disposal_proceeds AS DisposalProceeds,
		(d.disposal_proceeds - d.wdv_at_disposal) AS GainLoss,
		CASE WHEN (d.disposal_proceeds - d.wdv_at_disposal) >= 0 THEN 'Gain' ELSE 'Loss' END AS GainLossType
	FROM dbo.fa_asset_disposals d
	INNER JOIN dbo.fa_assets a ON a.asset_id = d.asset_id
	INNER JOIN dbo.fa_categories c ON c.category_id = a.category_id
	WHERE d.disposal_date BETWEEN @StartDate AND @EndDate
	ORDER BY c.category_name, a.asset_code, d.disposal_date;
END;
GO

-- ============================================================
-- sp_AssetsDueForReplacement
-- Assets whose useful life ends within @Months
-- ============================================================
IF OBJECT_ID('dbo.sp_AssetsDueForReplacement', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_AssetsDueForReplacement;
GO

CREATE PROCEDURE dbo.sp_AssetsDueForReplacement
	@Months INT = 12
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @EffDate DATE = CAST(GETDATE() AS DATE);
	DECLARE @CutoffDate DATE = DATEADD(MONTH, @Months, @EffDate);

	WITH Dep AS (
		SELECT
			d.asset_id,
			SUM(CASE WHEN d.run_date <= @EffDate THEN d.depreciation_amount ELSE 0 END) AS total_dep_to_date
		FROM dbo.fa_asset_depreciation d
		GROUP BY d.asset_id
	),
	Disp AS (
		SELECT
			d.asset_id,
			MAX(CASE WHEN d.disposal_date <= @EffDate THEN d.disposal_date END) AS disposal_date
		FROM dbo.fa_asset_disposals d
		GROUP BY d.asset_id
	)
	SELECT
		a.asset_code AS AssetCode,
		a.asset_name AS AssetName,
		c.category_name AS Category,
		a.purchase_date AS PurchaseDate,
		DATEADD(MONTH, a.useful_life_months, a.purchase_date) AS UsefulLifeEnd,
		CASE
			WHEN DATEDIFF(MONTH, @EffDate, DATEADD(MONTH, a.useful_life_months, a.purchase_date)) < 0 THEN 0
			ELSE DATEDIFF(MONTH, @EffDate, DATEADD(MONTH, a.useful_life_months, a.purchase_date))
		END AS RemainingMonths,
		CASE WHEN ISNULL(dep.total_dep_to_date, 0) >= (a.cost - a.salvage_value) THEN a.salvage_value ELSE (a.cost - ISNULL(dep.total_dep_to_date, 0)) END AS CurrentWDV,
		a.replacement_cost AS ReplacementCost
	FROM dbo.fa_assets a
	INNER JOIN dbo.fa_categories c ON c.category_id = a.category_id
	LEFT JOIN Dep dep ON dep.asset_id = a.asset_id
	LEFT JOIN Disp disp ON disp.asset_id = a.asset_id
	WHERE a.is_disposed = 0
	  AND DATEADD(MONTH, a.useful_life_months, a.purchase_date) <= @CutoffDate
	ORDER BY c.category_name, a.asset_code;
END;
GO

-- ============================================================
-- sp_AssetLocationSummary
-- Asset count and total WDV grouped by location / department
-- ============================================================
IF OBJECT_ID('dbo.sp_AssetLocationSummary', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_AssetLocationSummary;
GO

CREATE PROCEDURE dbo.sp_AssetLocationSummary
AS
BEGIN
	SET NOCOUNT ON;

	WITH Dep AS (
		SELECT
			d.asset_id,
			SUM(d.depreciation_amount) AS total_dep_to_date
		FROM dbo.fa_asset_depreciation d
		GROUP BY d.asset_id
	),
	Disp AS (
		SELECT
			d.asset_id,
			MAX(CASE WHEN d.disposal_date IS NOT NULL THEN d.disposal_date END) AS disposal_date,
			MAX(CASE WHEN d.disposal_date IS NOT NULL THEN d.wdv_at_disposal END) AS wdv_at_disposal
		FROM dbo.fa_asset_disposals d
		GROUP BY d.asset_id
	)
	SELECT
		ISNULL(parent.location_name, ISNULL(loc.location_name, 'Unassigned')) AS DepartmentName,
		CASE WHEN parent.location_name IS NOT NULL THEN loc.location_name ELSE NULL END AS LocationName,
		COUNT(a.asset_id) AS AssetCount,
		SUM(CASE
			WHEN disp.disposal_date IS NOT NULL THEN 0
			WHEN ISNULL(dep.total_dep_to_date, 0) >= (a.cost - a.salvage_value) THEN a.salvage_value
			ELSE (a.cost - ISNULL(dep.total_dep_to_date, 0))
		END) AS TotalWDV
	FROM dbo.fa_assets a
	LEFT JOIN dbo.fa_locations loc ON loc.location_id = a.location_id
	LEFT JOIN dbo.fa_locations parent ON parent.location_id = loc.parent_location_id
	LEFT JOIN Dep dep ON dep.asset_id = a.asset_id
	LEFT JOIN Disp disp ON disp.asset_id = a.asset_id
	WHERE a.is_active = 1
	  AND a.is_disposed = 0
	GROUP BY ISNULL(parent.location_name, ISNULL(loc.location_name, 'Unassigned')), CASE WHEN parent.location_name IS NOT NULL THEN loc.location_name ELSE NULL END
	ORDER BY DepartmentName, LocationName;
END;
GO
