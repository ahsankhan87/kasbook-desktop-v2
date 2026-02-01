/*
Fix for product insertion in `sp_ProductsCrud` to correctly insert into `pos_product_stocks`.

Notes:
- Uses a captured `@new_item_id` from `SCOPE_IDENTITY()` immediately after inserting into `pos_products`.
- Avoids calling `SCOPE_IDENTITY()` multiple times (each subsequent insert changes it).
- Inserts a stock row for the creating branch if it does not already exist.
- Ensures stock row contains `item_number`, `item_code`, and `item_id`.
- Keeps qty initialized to 0.

Apply manually in SQL Server (or integrate into your DB migration process).
*/

-- œ«Œ· ⁄„·Ì… «·≈œŒ«· (OperationType = 1)

BEGIN
    INSERT INTO pos_products (
        branch_id, item_number, code, name, name_ar, category_code, item_type, brand_code, barcode,
        status, qty, avg_cost, cost_price, unit_price, unit_price_2, tax_id, unit_id, location_code,
        description, deleted, date_created, date_updated, user_id, demand_qty, purchase_demand_qty,
        sale_demand_qty, origin, group_code, alt_no, picture, expiry_date, supplier_id, packet_qty,
        item_number_2, part_number
    )
    VALUES (
        LTRIM(RTRIM(@branch_id)),
        LTRIM(RTRIM(@item_number)),
        LTRIM(RTRIM(@code)),
        LTRIM(RTRIM(@name)),
        LTRIM(RTRIM(@name_ar)),
        LTRIM(RTRIM(@category_code)),
        LTRIM(RTRIM(@item_type)),
        LTRIM(RTRIM(@brand_code)),
        LTRIM(RTRIM(@barcode)),
        LTRIM(RTRIM(@status)),
        LTRIM(RTRIM(@qty)),
        LTRIM(RTRIM(@cost_price)),
        LTRIM(RTRIM(@cost_price)),
        LTRIM(RTRIM(@unit_price)),
        LTRIM(RTRIM(@unit_price_2)),
        LTRIM(RTRIM(@tax_id)),
        LTRIM(RTRIM(@unit_id)),
        LTRIM(RTRIM(@location_code)),
        LTRIM(RTRIM(@description)),
        LTRIM(RTRIM(@deleted)),
        @date_created,
        @date_updated,
        @user_id,
        LTRIM(RTRIM(@demand_qty)),
        LTRIM(RTRIM(@purchase_demand_qty)),
        LTRIM(RTRIM(@sale_demand_qty)),
        LTRIM(RTRIM(@origin)),
        LTRIM(RTRIM(@group_code)),
        LTRIM(RTRIM(@alt_no)),
        @picture,
        @expiry_date,
        @supplier_id,
        LTRIM(RTRIM(@packet_qty)),
        LTRIM(RTRIM(@item_number_2)),
        LTRIM(RTRIM(@part_number))
    );

    DECLARE @new_item_id INT = CAST(SCOPE_IDENTITY() AS INT);

    -- Ensure location exists
    INSERT INTO pos_locations (branch_id, user_id, code, name, date_created)
    SELECT @branch_id, @user_id, LTRIM(RTRIM(@location_code)), LTRIM(RTRIM(@location_code)), GETDATE()
    WHERE NOT EXISTS (
        SELECT 1 FROM pos_locations WHERE code = LTRIM(RTRIM(@location_code))
    );

    -- Insert stock row for current branch (safe insert)
    IF NOT EXISTS (
        SELECT 1
        FROM pos_product_stocks
        WHERE branch_id = @branch_id
          AND item_number = LTRIM(RTRIM(@item_number))
    )
    BEGIN
        INSERT INTO pos_product_stocks (
            branch_id, user_id, item_id, item_code, qty, reorder_level, date_created, item_number
        )
        VALUES (
            LTRIM(RTRIM(@branch_id)),
            LTRIM(RTRIM(@user_id)),
            @new_item_id,
            LTRIM(RTRIM(@code)),
            0,
            LTRIM(RTRIM(@re_stock_level)),
            GETDATE(),
            LTRIM(RTRIM(@item_number))
        );
    END

    -- Insert products qty to all branches, initially zero qty
    EXEC sp_ProductsStocks 1, @branch_id, @user_id, @code, 0, 0, @new_item_id, @item_number;

    -- Return the new product id
    SELECT @new_item_id;
END
