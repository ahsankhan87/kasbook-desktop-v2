-- ============================================================
-- Discount Module Migration - Phase 1: Product & Brand Level
-- Run this script on your SQL Server database once.
-- ============================================================

-- 1. Create pos_discount_schemes table
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'pos_discount_schemes')
BEGIN
    CREATE TABLE pos_discount_schemes (
        id            INT IDENTITY(1,1) PRIMARY KEY,
        name          NVARCHAR(100) NOT NULL,
        name_ar       NVARCHAR(100) NULL,
        calc_type     VARCHAR(10)   NOT NULL,    -- 'PERCENT' | 'AMOUNT'
        value         DECIMAL(18,4) NOT NULL DEFAULT 0,
        is_active     BIT           NOT NULL DEFAULT 1,
        start_date    DATE          NULL,
        end_date      DATE          NULL,
        branch_id     INT           NOT NULL DEFAULT 0,
        company_id    INT           NOT NULL DEFAULT 0,
        created_by    INT           NULL,
        created_at    DATETIME      NOT NULL DEFAULT GETDATE(),
        updated_at    DATETIME      NOT NULL DEFAULT GETDATE()
    );
    PRINT 'pos_discount_schemes table created.';
END
ELSE
    PRINT 'pos_discount_schemes table already exists.';
GO

-- 2. Create stored procedure sp_DiscountSchemesCrud
IF OBJECT_ID('dbo.sp_DiscountSchemesCrud', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_DiscountSchemesCrud;
GO
CREATE PROCEDURE dbo.sp_DiscountSchemesCrud
    @OperationType  VARCHAR(5)    = NULL,
    @id             INT           = NULL,
    @name           NVARCHAR(100) = NULL,
    @name_ar        NVARCHAR(100) = NULL,
    @discount_type  VARCHAR(20)   = NULL,
    @scope_value    NVARCHAR(100) = NULL,
    @calc_type      VARCHAR(10)   = NULL,
    @value          DECIMAL(18,4) = NULL,
    @is_active      BIT           = NULL,
    @start_date     DATE          = NULL,
    @end_date       DATE          = NULL,
    @branch_id      INT           = NULL,
    @company_id     INT           = NULL,
    @created_by     INT           = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- INSERT
    IF @OperationType = '1'
    BEGIN
        INSERT INTO pos_discount_schemes
            (name, name_ar, calc_type, value,
             is_active, start_date, end_date, branch_id, company_id, created_by, created_at, updated_at)
        VALUES
            (@name, @name_ar,  @calc_type, @value,
             ISNULL(@is_active,1), @start_date, @end_date, @branch_id, @company_id, @created_by, GETDATE(), GETDATE());
        SELECT SCOPE_IDENTITY() AS id;
        RETURN;
    END

    -- UPDATE
    IF @OperationType = '2'
    BEGIN
        UPDATE pos_discount_schemes SET
            name          = @name,
            name_ar       = @name_ar,
            calc_type     = @calc_type,
            value         = @value,
            is_active     = ISNULL(@is_active, is_active),
            start_date    = @start_date,
            end_date      = @end_date,
            updated_at    = GETDATE()
        WHERE id = @id;
        SELECT @id AS id;
        RETURN;
    END

    -- DELETE
    IF @OperationType = '3'
    BEGIN
        DELETE FROM pos_discount_schemes WHERE id = @id;
        SELECT @id AS id;
        RETURN;
    END

    -- GET BY ID
    IF @OperationType = '4'
    BEGIN
        SELECT * FROM pos_discount_schemes WHERE id = @id;
        RETURN;
    END

    -- GET ALL (branch-filtered)
    IF @OperationType = '5'
    BEGIN
        SELECT * FROM pos_discount_schemes
        WHERE branch_id = @branch_id
        ORDER BY calc_type, name;
        RETURN;
    END

    -- TOGGLE ACTIVE
    IF @OperationType = '6'
    BEGIN
        UPDATE pos_discount_schemes
        SET is_active = CASE WHEN is_active = 1 THEN 0 ELSE 1 END,
            updated_at = GETDATE()
        WHERE id = @id;
        SELECT @id AS id;
        RETURN;
    END
END
GO
PRINT 'sp_DiscountSchemesCrud created.';
GO

-- ============================================================
-- Discount Module Migration - FK-linked Product/Brand/Category discounts
-- ============================================================

-- Ensure table exists with FK-linked schema
IF OBJECT_ID('dbo.pos_discount_schemes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.pos_discount_schemes
    (
        id          INT IDENTITY(1,1) PRIMARY KEY,
        name        NVARCHAR(100) NOT NULL,
        name_ar     NVARCHAR(100) NULL,
        product_id  INT NULL,
        brand_id    INT NULL,
        category_id INT NULL,
        calc_type   VARCHAR(10) NOT NULL,      -- PERCENT | AMOUNT
        value       DECIMAL(18,4) NOT NULL DEFAULT(0),
        is_active   BIT NOT NULL DEFAULT(1),
        start_date  DATE NULL,
        end_date    DATE NULL,
        branch_id   INT NOT NULL DEFAULT(0),
        company_id  INT NOT NULL DEFAULT(0),
        created_by  INT NULL,
        created_at  DATETIME NOT NULL DEFAULT(GETDATE()),
        updated_at  DATETIME NOT NULL DEFAULT(GETDATE())
    );
END
GO

-- Add new FK columns for existing installations
IF COL_LENGTH('dbo.pos_discount_schemes','product_id') IS NULL
    ALTER TABLE dbo.pos_discount_schemes ADD product_id INT NULL;
IF COL_LENGTH('dbo.pos_discount_schemes','brand_id') IS NULL
    ALTER TABLE dbo.pos_discount_schemes ADD brand_id INT NULL;
IF COL_LENGTH('dbo.pos_discount_schemes','category_id') IS NULL
    ALTER TABLE dbo.pos_discount_schemes ADD category_id INT NULL;
GO

-- Remove legacy columns if present (after migration to FK model)
IF COL_LENGTH('dbo.pos_discount_schemes','discount_type') IS NOT NULL
BEGIN
    ALTER TABLE dbo.pos_discount_schemes DROP COLUMN discount_type;
END
IF COL_LENGTH('dbo.pos_discount_schemes','scope_value') IS NOT NULL
BEGIN
    ALTER TABLE dbo.pos_discount_schemes DROP COLUMN scope_value;
END
GO

-- Foreign keys
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_pos_discount_schemes_products')
BEGIN
    ALTER TABLE dbo.pos_discount_schemes WITH CHECK
    ADD CONSTRAINT FK_pos_discount_schemes_products FOREIGN KEY(product_id) REFERENCES dbo.pos_products(id);
END

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_pos_discount_schemes_brands')
BEGIN
    ALTER TABLE dbo.pos_discount_schemes WITH CHECK
    ADD CONSTRAINT FK_pos_discount_schemes_brands FOREIGN KEY(brand_id) REFERENCES dbo.pos_brands(id);
END

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_pos_discount_schemes_categories')
BEGIN
    ALTER TABLE dbo.pos_discount_schemes WITH CHECK
    ADD CONSTRAINT FK_pos_discount_schemes_categories FOREIGN KEY(category_id) REFERENCES dbo.pos_categories(id);
END
GO

-- Prevent invalid rows where no target or multiple targets are set
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_pos_discount_schemes_single_target')
BEGIN
    ALTER TABLE dbo.pos_discount_schemes ADD CONSTRAINT CK_pos_discount_schemes_single_target
    CHECK (
        (CASE WHEN product_id IS NULL THEN 0 ELSE 1 END) +
        (CASE WHEN brand_id IS NULL THEN 0 ELSE 1 END) +
        (CASE WHEN category_id IS NULL THEN 0 ELSE 1 END) = 1
    );
END
GO

PRINT 'Discount FK migration completed.';
GO
