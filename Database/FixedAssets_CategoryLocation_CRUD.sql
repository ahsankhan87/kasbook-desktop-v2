-- ============================================================
-- Fixed Assets CRUD Procedures
-- These procedures follow the standard CRUD pattern with OperationType
-- Created: 2026-07-18
-- Author: Ahsan Khan
-- ============================================================

SET NOCOUNT ON;
GO

-- ============================================================
-- sp_FixedAssetCategoriesCrud
-- Handles all CRUD operations for fa_categories
-- OperationTypes: 1=Insert, 2=Update, 3=Delete, 4=Select One, 5=Select All
-- ============================================================

IF OBJECT_ID('dbo.sp_FixedAssetCategoriesCrud', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_FixedAssetCategoriesCrud;
GO

CREATE PROCEDURE dbo.sp_FixedAssetCategoriesCrud
	@category_id                INT = 0 OUTPUT,
	@category_code              VARCHAR(20) = '',
	@category_name              NVARCHAR(150) = '',
	@depreciation_method        VARCHAR(30) = 'STRAIGHT_LINE',
	@useful_life_months         INT = 60,
	@annual_depreciation_rate   DECIMAL(9,4) = NULL,
	@is_active                  BIT = 1,
	@OperationType              INT
AS
BEGIN
	SET NOCOUNT ON;

	-- 1) Insert
	IF @OperationType = 1
	BEGIN
		IF EXISTS (SELECT 1 FROM dbo.fa_categories WHERE category_code = @category_code)
		BEGIN
			RAISERROR('Category code already exists.', 16, 1);
			RETURN -1;
		END

		INSERT INTO dbo.fa_categories (category_code, category_name, depreciation_method, useful_life_months, annual_depreciation_rate, is_active, created_at)
		VALUES (@category_code, @category_name, @depreciation_method, @useful_life_months, @annual_depreciation_rate, @is_active, GETDATE());

		SELECT @category_id = SCOPE_IDENTITY();
		RETURN @category_id;
	END

	-- 2) Update
	ELSE IF @OperationType = 2
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.fa_categories WHERE category_id = @category_id)
		BEGIN
			RAISERROR('Category not found.', 16, 1);
			RETURN -1;
		END

		UPDATE dbo.fa_categories
		SET category_name = @category_name,
			depreciation_method = @depreciation_method,
			useful_life_months = @useful_life_months,
			annual_depreciation_rate = @annual_depreciation_rate,
			is_active = @is_active
		WHERE category_id = @category_id;

		SELECT @category_id;
		RETURN 0;
	END

	-- 3) Delete (Soft delete if in use, hard delete if not)
	ELSE IF @OperationType = 3
	BEGIN
		IF EXISTS (SELECT 1 FROM dbo.fa_assets WHERE category_id = @category_id)
		BEGIN
			-- Soft delete by marking inactive
			UPDATE dbo.fa_categories SET is_active = 0 WHERE category_id = @category_id;
			RETURN 1; -- Soft deleted
		END

		-- Hard delete if not in use
		DELETE FROM dbo.fa_categories WHERE category_id = @category_id;
		RETURN 0; -- Hard deleted
	END

	-- 4) Select Particular Record
	ELSE IF @OperationType = 4
	BEGIN
		SELECT
			category_id,
			category_code,
			category_name,
			depreciation_method,
			useful_life_months,
			annual_depreciation_rate,
			is_active,
			created_at
		FROM dbo.fa_categories
		WHERE category_id = @category_id;
	END

	-- 5) Select All
	ELSE
	BEGIN
		SELECT
			category_id,
			category_code,
			category_name,
			depreciation_method,
			useful_life_months,
			annual_depreciation_rate,
			is_active,
			created_at
		FROM dbo.fa_categories
		WHERE @is_active = 0 OR is_active = 1
		ORDER BY category_name;
	END
END;
GO

-- ============================================================
-- sp_FixedAssetLocationsCrud
-- Handles all CRUD operations for fa_locations
-- OperationTypes: 1=Insert, 2=Update, 3=Delete, 4=Select One, 5=Select All
-- ============================================================

IF OBJECT_ID('dbo.sp_FixedAssetLocationsCrud', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_FixedAssetLocationsCrud;
GO

CREATE PROCEDURE dbo.sp_FixedAssetLocationsCrud
	@location_id            INT = 0 OUTPUT,
	@location_code          VARCHAR(20) = '',
	@location_name          NVARCHAR(150) = '',
	@location_type          VARCHAR(30) = 'LOCATION',
	@parent_location_id     INT = NULL,
	@is_active              BIT = 1,
	@OperationType          INT
AS
BEGIN
	SET NOCOUNT ON;

	-- 1) Insert
	IF @OperationType = 1
	BEGIN
		IF EXISTS (SELECT 1 FROM dbo.fa_locations WHERE location_code = @location_code)
		BEGIN
			RAISERROR('Location code already exists.', 16, 1);
			RETURN -1;
		END

		INSERT INTO dbo.fa_locations (location_code, location_name, location_type, parent_location_id, is_active, created_at)
		VALUES (@location_code, @location_name, @location_type, @parent_location_id, @is_active, GETDATE());

		SELECT @location_id = SCOPE_IDENTITY();
		RETURN @location_id;
	END

	-- 2) Update
	ELSE IF @OperationType = 2
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.fa_locations WHERE location_id = @location_id)
		BEGIN
			RAISERROR('Location not found.', 16, 1);
			RETURN -1;
		END

		UPDATE dbo.fa_locations
		SET location_name = @location_name,
			location_type = @location_type,
			parent_location_id = @parent_location_id,
			is_active = @is_active
		WHERE location_id = @location_id;

		SELECT @location_id;
		RETURN 0;
	END

	-- 3) Delete (Soft delete if in use, hard delete if not)
	ELSE IF @OperationType = 3
	BEGIN
		IF EXISTS (SELECT 1 FROM dbo.fa_assets WHERE location_id = @location_id)
		BEGIN
			-- Soft delete by marking inactive
			UPDATE dbo.fa_locations SET is_active = 0 WHERE location_id = @location_id;
			RETURN 1; -- Soft deleted
		END

		-- Hard delete if not in use
		DELETE FROM dbo.fa_locations WHERE location_id = @location_id;
		RETURN 0; -- Hard deleted
	END

	-- 4) Select Particular Record
	ELSE IF @OperationType = 4
	BEGIN
		SELECT
			location_id,
			location_code,
			location_name,
			location_type,
			parent_location_id,
			is_active,
			created_at
		FROM dbo.fa_locations
		WHERE location_id = @location_id;
	END

	-- 5) Select All
	ELSE
	BEGIN
		SELECT
			location_id,
			location_code,
			location_name,
			location_type,
			parent_location_id,
			is_active,
			created_at
		FROM dbo.fa_locations
		WHERE @is_active = 0 OR is_active = 1
		ORDER BY location_name;
	END
END;
GO

-- ============================================================
-- sp_FixedAssetsCrud
-- Handles all CRUD operations for fa_assets
-- OperationTypes: 1=Insert, 2=Update, 3=Delete, 4=Select One, 5=Select All
-- ============================================================

IF OBJECT_ID('dbo.sp_FixedAssetsCrud', 'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_FixedAssetsCrud;
GO

CREATE PROCEDURE dbo.sp_FixedAssetsCrud
	@asset_id               INT = 0 OUTPUT,
	@asset_code             VARCHAR(50) = '',
	@asset_name             NVARCHAR(200) = '',
	@category_id            INT = NULL,
	@location_id            INT = NULL,
	@serial_number          NVARCHAR(100) = NULL,
	@purchase_date          DATE = NULL,
	@cost                   DECIMAL(18,2) = 0,
	@dep_method             VARCHAR(30) = 'STRAIGHT_LINE',
	@useful_life_months     INT = 60,
	@salvage_value          DECIMAL(18,2) = 0,
	@replacement_cost       DECIMAL(18,2) = NULL,
	@notes                  NVARCHAR(500) = NULL,
	@is_active              BIT = 1,
	@created_by             INT = NULL,
	@OperationType          INT
AS
BEGIN
	SET NOCOUNT ON;

	-- 1) Insert
	IF @OperationType = 1
	BEGIN
		IF EXISTS (SELECT 1 FROM dbo.fa_assets WHERE asset_code = @asset_code)
		BEGIN
			RAISERROR('Asset code already exists.', 16, 1);
			RETURN -1;
		END

		INSERT INTO dbo.fa_assets (
			asset_code, asset_name, category_id, location_id, serial_number,
			purchase_date, cost, dep_method, useful_life_months, salvage_value,
			replacement_cost, notes, is_active, created_at, created_by
		)
		VALUES (
			@asset_code, @asset_name, @category_id, @location_id, @serial_number,
			@purchase_date, @cost, @dep_method, @useful_life_months, @salvage_value,
			@replacement_cost, @notes, @is_active, GETDATE(), @created_by
		);

		SELECT @asset_id = SCOPE_IDENTITY();
		RETURN @asset_id;
	END

	-- 2) Update (only non-cost fields)
	ELSE IF @OperationType = 2
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.fa_assets WHERE asset_id = @asset_id)
		BEGIN
			RAISERROR('Asset not found.', 16, 1);
			RETURN -1;
		END

		UPDATE dbo.fa_assets
		SET asset_name = @asset_name,
			location_id = @location_id,
			notes = @notes,
			is_active = @is_active,
			updated_at = GETDATE()
		WHERE asset_id = @asset_id;

		SELECT @asset_id;
		RETURN 0;
	END

	-- 3) Delete
	ELSE IF @OperationType = 3
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM dbo.fa_assets WHERE asset_id = @asset_id)
		BEGIN
			RAISERROR('Asset not found.', 16, 1);
			RETURN -1;
		END

		-- Soft delete by marking inactive
		UPDATE dbo.fa_assets SET is_active = 0 WHERE asset_id = @asset_id;
		RETURN 0;
	END

	-- 4) Select Particular Record
	ELSE IF @OperationType = 4
	BEGIN
		SELECT
			asset_id, asset_code, asset_name, category_id, location_id,
			serial_number, purchase_date, cost, dep_method, useful_life_months,
			salvage_value, replacement_cost, notes, is_active, created_at,
			created_by, updated_at
		FROM dbo.fa_assets
		WHERE asset_id = @asset_id;
	END

	-- 5) Select All
	ELSE
	BEGIN
		SELECT
			asset_id, asset_code, asset_name, category_id, location_id,
			serial_number, purchase_date, cost, dep_method, useful_life_months,
			salvage_value, replacement_cost, notes, is_active, created_at,
			created_by, updated_at
		FROM dbo.fa_assets
		WHERE @is_active = 0 OR is_active = 1
		ORDER BY asset_code;
	END
END;
GO
