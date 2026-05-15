-- Migration: Move discount scheme FK from pos_discount_schemes to pos_products
-- Run this script once against your SQL Server database.

-- 1. Add discount_scheme_id column to pos_products (nullable FK)
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('pos_products') AND name = 'discount_scheme_id'
)
BEGIN
    ALTER TABLE pos_products
        ADD discount_scheme_id INT NULL;
END
GO

-- 2. Add FK constraint (optional but recommended)
IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = 'FK_pos_products_discount_scheme'
)
BEGIN
    ALTER TABLE pos_products
        ADD CONSTRAINT FK_pos_products_discount_scheme
        FOREIGN KEY (discount_scheme_id) REFERENCES pos_discount_schemes(id)
        ON DELETE SET NULL;
END
GO

-- 3. Migrate existing product-level links from pos_discount_schemes to pos_products
--    (only where product_id was set on the scheme)
UPDATE P
SET    P.discount_scheme_id = DS.id
FROM   pos_products P
INNER JOIN pos_discount_schemes DS ON DS.product_id = P.id
WHERE  DS.product_id IS NOT NULL;
GO

-- 4. Drop the old FK columns from pos_discount_schemes
--    (do this after verifying the migration above is correct)
-- ALTER TABLE pos_discount_schemes DROP COLUMN product_id;
-- ALTER TABLE pos_discount_schemes DROP COLUMN brand_id;
-- ALTER TABLE pos_discount_schemes DROP COLUMN category_id;
-- Note: uncomment the lines above only after confirming data is correct.
GO
