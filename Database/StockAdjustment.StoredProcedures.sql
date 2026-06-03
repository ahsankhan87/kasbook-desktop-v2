CREATE OR ALTER PROCEDURE dbo.sp_StockAdjustment_CreateSession
    @adj_date DATE,
    @adj_type NVARCHAR(100),
    @warehouse_id INT,
    @notes NVARCHAR(1000) = NULL,
    @created_by INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @adj_id INT;
    DECLARE @adj_no NVARCHAR(40);
    DECLARE @ymd CHAR(8) = CONVERT(CHAR(8), GETDATE(), 112);
    DECLARE @nextNo INT;

    SELECT @nextNo = ISNULL(MAX(CAST(RIGHT(adj_no, 5) AS INT)), 0) + 1
    FROM dbo.stk_adj_headers
    WHERE adj_no LIKE CONCAT('SA-', @ymd, '-%');

    SET @adj_no = CONCAT('SA-', @ymd, '-', RIGHT('00000' + CAST(@nextNo AS VARCHAR(5)), 5));

    INSERT INTO dbo.stk_adj_headers
    (
        adj_no, adj_date, adj_type, warehouse_id, status, notes,
        created_by, created_at, modified_by, modified_at
    )
    VALUES
    (
        @adj_no, @adj_date, @adj_type, @warehouse_id, 'Draft', @notes,
        @created_by, GETDATE(), @created_by, GETDATE()
    );

    SET @adj_id = SCOPE_IDENTITY();
    SELECT @adj_id AS adj_id, @adj_no AS adj_no;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_StockAdjustment_UpsertLine
    @adj_id INT,
    @product_id INT,
    @system_qty DECIMAL(18,4),
    @physical_qty DECIMAL(18,4),
    @qty_difference DECIMAL(18,4),
    @current_sale_price DECIMAL(18,4),
    @new_sale_price DECIMAL(18,4),
    @current_location NVARCHAR(100),
    @new_location NVARCHAR(100),
    @reason NVARCHAR(200) = NULL,
    @notes NVARCHAR(1000) = NULL,
    @is_verified BIT,
    @modified_by INT
AS
BEGIN
    SET NOCOUNT ON;

    MERGE dbo.stk_adj_lines AS T
    USING (SELECT @adj_id AS adj_id, @product_id AS product_id) AS S
       ON T.adj_id = S.adj_id AND T.product_id = S.product_id
    WHEN MATCHED THEN
        UPDATE SET
            system_qty = @system_qty,
            physical_qty = @physical_qty,
            qty_difference = @qty_difference,
            current_sale_price = @current_sale_price,
            new_sale_price = @new_sale_price,
            current_location = @current_location,
            new_location = @new_location,
            reason = @reason,
            notes = @notes,
            is_verified = @is_verified,
            modified_by = @modified_by,
            modified_at = GETDATE()
    WHEN NOT MATCHED THEN
        INSERT
        (
            adj_id, product_id, system_qty, physical_qty, qty_difference,
            current_sale_price, new_sale_price,
            current_location, new_location,
            reason, notes, is_verified,
            created_by, created_at, modified_by, modified_at
        )
        VALUES
        (
            @adj_id, @product_id, @system_qty, @physical_qty, @qty_difference,
            @current_sale_price, @new_sale_price,
            @current_location, @new_location,
            @reason, @notes, @is_verified,
            @modified_by, GETDATE(), @modified_by, GETDATE()
        );
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_PostStockAdjustment
    @adj_id INT,
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRAN;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.stk_adj_headers h WITH (UPDLOCK, HOLDLOCK)
            WHERE h.adj_id = @adj_id
              AND h.status = 'Draft'
        )
            THROW 51001, 'Session not found or not in Draft status.', 1;

        IF EXISTS (
            SELECT 1
            FROM dbo.stk_adj_lines l
            WHERE l.adj_id = @adj_id
              AND (l.product_id IS NULL OR l.system_qty IS NULL OR l.physical_qty IS NULL)
        )
            THROW 51002, 'Required fields missing in one or more lines.', 1;

        UPDATE p
        SET p.current_qty = l.physical_qty
        FROM dbo.mst_products p
        INNER JOIN dbo.stk_adj_lines l ON l.product_id = p.product_id
        WHERE l.adj_id = @adj_id
          AND ISNULL(l.qty_difference, 0) <> 0;

        UPDATE p
        SET p.sale_price = l.new_sale_price,
            p.last_price_change = GETDATE()
        FROM dbo.mst_products p
        INNER JOIN dbo.stk_adj_lines l ON l.product_id = p.product_id
        WHERE l.adj_id = @adj_id
          AND ISNULL(l.new_sale_price, 0) <> ISNULL(l.current_sale_price, 0);

        UPDATE pl
        SET pl.location_code = l.new_location,
            pl.updated_at = GETDATE(),
            pl.updated_by = @user_id
        FROM dbo.product_locations pl
        INNER JOIN dbo.stk_adj_lines l ON l.product_id = pl.product_id
        WHERE l.adj_id = @adj_id
          AND ISNULL(l.new_location, '') <> ISNULL(l.current_location, '');

        INSERT INTO dbo.stk_adj_audit
        (
            adj_id, line_id, product_id,
            old_qty, new_qty,
            old_sale_price, new_sale_price,
            old_location, new_location,
            change_type, changed_by, changed_at, notes
        )
        SELECT
            l.adj_id,
            l.line_id,
            l.product_id,
            l.system_qty,
            l.physical_qty,
            l.current_sale_price,
            l.new_sale_price,
            l.current_location,
            l.new_location,
            CASE
                WHEN ISNULL(l.qty_difference, 0) <> 0 THEN 'Qty'
                WHEN ISNULL(l.new_sale_price, 0) <> ISNULL(l.current_sale_price, 0) THEN 'Price'
                WHEN ISNULL(l.new_location, '') <> ISNULL(l.current_location, '') THEN 'Location'
                ELSE 'NoChange'
            END,
            @user_id,
            GETDATE(),
            l.notes
        FROM dbo.stk_adj_lines l
        WHERE l.adj_id = @adj_id
          AND (
                ISNULL(l.qty_difference, 0) <> 0
             OR ISNULL(l.new_sale_price, 0) <> ISNULL(l.current_sale_price, 0)
             OR ISNULL(l.new_location, '') <> ISNULL(l.current_location, '')
          );

        UPDATE dbo.stk_adj_headers
        SET status = 'Posted',
            posted_by = @user_id,
            posted_at = GETDATE(),
            modified_at = GETDATE(),
            modified_by = @user_id
        WHERE adj_id = @adj_id;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;

        DECLARE @Err NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @Severity INT = ERROR_SEVERITY();
        DECLARE @State INT = ERROR_STATE();
        RAISERROR(@Err, @Severity, @State);
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_StockAdjustment_Reverse
    @adj_id INT,
    @reason NVARCHAR(1000),
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRAN;

        DECLARE @posted_at DATETIME;
        DECLARE @can_reverse_after_24h BIT = 0;

        SELECT
            @posted_at = posted_at,
            @can_reverse_after_24h = ISNULL(can_reverse_after_24h, 0)
        FROM dbo.stk_adj_headers WITH (UPDLOCK, HOLDLOCK)
        WHERE adj_id = @adj_id
          AND status = 'Posted';

        IF @posted_at IS NULL
            THROW 52001, 'Only posted adjustments can be reversed.', 1;

        IF DATEDIFF(HOUR, @posted_at, GETDATE()) > 24 AND ISNULL(@can_reverse_after_24h, 0) = 0
            THROW 52002, 'Reverse allowed within 24 hours only unless special permission is granted.', 1;

        UPDATE p
        SET p.current_qty = l.system_qty,
            p.sale_price = l.current_sale_price,
            p.last_price_change = GETDATE()
        FROM dbo.mst_products p
        INNER JOIN dbo.stk_adj_lines l ON l.product_id = p.product_id
        WHERE l.adj_id = @adj_id;

        UPDATE pl
        SET pl.location_code = l.current_location,
            pl.updated_at = GETDATE(),
            pl.updated_by = @user_id
        FROM dbo.product_locations pl
        INNER JOIN dbo.stk_adj_lines l ON l.product_id = pl.product_id
        WHERE l.adj_id = @adj_id;

        INSERT INTO dbo.stk_adj_audit
        (
            adj_id, line_id, product_id,
            old_qty, new_qty,
            old_sale_price, new_sale_price,
            old_location, new_location,
            change_type, changed_by, changed_at, notes
        )
        SELECT
            l.adj_id,
            l.line_id,
            l.product_id,
            l.physical_qty,
            l.system_qty,
            l.new_sale_price,
            l.current_sale_price,
            l.new_location,
            l.current_location,
            'Reverse',
            @user_id,
            GETDATE(),
            @reason
        FROM dbo.stk_adj_lines l
        WHERE l.adj_id = @adj_id;

        UPDATE dbo.stk_adj_headers
        SET status = 'Reversed',
            reverse_reason = @reason,
            reversed_by = @user_id,
            reversed_at = GETDATE(),
            modified_by = @user_id,
            modified_at = GETDATE()
        WHERE adj_id = @adj_id;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;

        DECLARE @Err NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @Severity INT = ERROR_SEVERITY();
        DECLARE @State INT = ERROR_STATE();
        RAISERROR(@Err, @Severity, @State);
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_StockAdjustment_GetSessionSummary
    @adj_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        COUNT(1) AS total_lines,
        ISNULL(SUM(CASE WHEN qty_difference > 0 THEN qty_difference ELSE 0 END), 0) AS qty_increase,
        ISNULL(SUM(CASE WHEN qty_difference < 0 THEN ABS(qty_difference) ELSE 0 END), 0) AS qty_decrease,
        SUM(CASE WHEN ISNULL(new_sale_price,0) <> ISNULL(current_sale_price,0) THEN 1 ELSE 0 END) AS price_change_products,
        SUM(CASE WHEN ISNULL(new_location,'') <> ISNULL(current_location,'') THEN 1 ELSE 0 END) AS relocated_products,
        ISNULL(SUM((physical_qty * new_sale_price) - (system_qty * current_sale_price)), 0) AS total_stock_value_impact
    FROM dbo.stk_adj_lines
    WHERE adj_id = @adj_id;
END;
GO
