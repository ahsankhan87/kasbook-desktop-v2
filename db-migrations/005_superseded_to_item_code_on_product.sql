-- Migration:
-- Run this script once against your SQL Server database.

-- 1. Add superseded_to_item_code and superseded_from_item_code column to pos_products (nullable FK)
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('pos_products') AND name = 'superseded_to_item_code'
)
BEGIN
    ALTER TABLE pos_products
        ADD superseded_to_item_code NVARCHAR(50) NULL,
            superseded_from_item_code NVARCHAR(50) NULL;
END
GO

