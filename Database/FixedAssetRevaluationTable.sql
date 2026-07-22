-- ============================================================
-- Fixed Assets: Revaluation Table
-- Run this script in SQL Server (once)
-- ============================================================

SET NOCOUNT ON;
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'fa_asset_revaluations')
BEGIN
	CREATE TABLE dbo.fa_asset_revaluations
	(
		revaluation_id       INT             NOT NULL IDENTITY(1,1),
		asset_id             INT             NOT NULL,
		revaluation_date     DATE            NOT NULL,

		old_cost             DECIMAL(18,2)   NOT NULL CONSTRAINT DF_fa_asset_reval_old_cost DEFAULT (0),
		new_cost             DECIMAL(18,2)   NOT NULL CONSTRAINT DF_fa_asset_reval_new_cost DEFAULT (0),

		old_accum_dep        DECIMAL(18,2)   NOT NULL CONSTRAINT DF_fa_asset_reval_old_accum DEFAULT (0),
		new_accum_dep        DECIMAL(18,2)   NOT NULL CONSTRAINT DF_fa_asset_reval_new_accum DEFAULT (0),

		old_wdv              DECIMAL(18,2)   NOT NULL CONSTRAINT DF_fa_asset_reval_old_wdv DEFAULT (0),
		new_wdv              DECIMAL(18,2)   NOT NULL CONSTRAINT DF_fa_asset_reval_new_wdv DEFAULT (0),

		notes                NVARCHAR(500)   NULL,
		created_at           DATETIME        NOT NULL CONSTRAINT DF_fa_asset_reval_created_at DEFAULT (GETDATE()),

		CONSTRAINT PK_fa_asset_revaluations PRIMARY KEY (revaluation_id),
		CONSTRAINT FK_fa_asset_revaluations_asset FOREIGN KEY (asset_id) REFERENCES dbo.fa_assets(asset_id),

		CONSTRAINT CK_fa_asset_reval_costs CHECK (old_cost >= 0 AND new_cost >= 0),
		CONSTRAINT CK_fa_asset_reval_accum CHECK (old_accum_dep >= 0 AND new_accum_dep >= 0),
		CONSTRAINT CK_fa_asset_reval_wdv CHECK (old_wdv >= 0 AND new_wdv >= 0),
		CONSTRAINT CK_fa_asset_reval_change CHECK (old_cost <> new_cost)
	);

	-- Enforce one-time revaluation per asset (matches app validation)
	CREATE UNIQUE INDEX UX_fa_asset_revaluations_asset_id
		ON dbo.fa_asset_revaluations(asset_id);

	CREATE INDEX IX_fa_asset_revaluations_asset_date
		ON dbo.fa_asset_revaluations(asset_id, revaluation_date);
END
GO
