-- ============================================================
-- Stock Adjustment Session Lifecycle Stored Procedures
-- Aligned to the canonical schema in StockAdjustmentSchema.sql
-- Table names: stk_adj_sessions, pos_stk_adj_lines, pos_stk_adj_audit
-- Run after StockAdjustmentSchema.sql
-- ============================================================

SET NOCOUNT ON;
GO

-- ============================================================
-- sp_StockAdjustment_CreateSession
-- Creates a new Draft session, generates adj_no via sp_GenerateAdjNo
-- Returns: adj_id, adj_no
-- ============================================================
IF OBJECT_ID('sp_StockAdjustment_CreateSession', 'P') IS NOT NULL
    DROP PROCEDURE sp_StockAdjustment_CreateSession;
GO

CREATE PROCEDURE sp_StockAdjustment_CreateSession
    @adj_date       DATE,
    @adj_type       VARCHAR(30),
    @warehouse_id   INT,
    @notes          NVARCHAR(500),
    @created_by     INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @adj_no VARCHAR(20);
    EXEC sp_GenerateAdjNo @AdjType = @adj_type, @AdjNo = @adj_no OUTPUT;

    INSERT INTO pos_stk_adj_sessions
        (adj_no, adj_date, adj_type, warehouse_id, status, notes, created_by, created_at)
    VALUES
        (@adj_no, ISNULL(@adj_date, CAST(GETDATE() AS DATE)),
         @adj_type, @warehouse_id, 'Draft', @notes, @created_by, GETDATE());

    DECLARE @adj_id INT = SCOPE_IDENTITY();

    SELECT @adj_id AS adj_id, @adj_no AS adj_no;
END;
GO

-- ============================================================
-- sp_StockAdjustment_UpsertLine
-- Insert or update a single adjustment line within a Draft session.
-- qty_difference and price_difference are computed/persisted columns — not supplied.
-- ============================================================
IF OBJECT_ID('sp_StockAdjustment_UpsertLine', 'P') IS NOT NULL
    DROP PROCEDURE sp_StockAdjustment_UpsertLine;
GO

CREATE PROCEDURE sp_StockAdjustment_UpsertLine
    @adj_id             INT,
    @product_id         INT,
    @system_qty         DECIMAL(18,3),
    @physical_qty       DECIMAL(18,3),
    @current_sale_price DECIMAL(18,2),
    @new_sale_price     DECIMAL(18,2),
    @current_location   NVARCHAR(30),
    @new_location       NVARCHAR(30),
    @reason             VARCHAR(30),
    @notes              NVARCHAR(300),
    @is_verified        BIT
AS
BEGIN
    SET NOCOUNT ON;

    -- Guard: only allow edits on Draft sessions
    IF NOT EXISTS (SELECT 1 FROM pos_stk_adj_sessions WHERE adj_id = @adj_id AND status = 'Draft')
    BEGIN
        RAISERROR('Only Draft sessions can be edited.', 16, 1);
        RETURN;
    END;

    IF EXISTS (SELECT 1 FROM pos_stk_adj_lines WHERE adj_id = @adj_id AND product_id = @product_id)
    BEGIN
        UPDATE pos_stk_adj_lines SET
            system_qty          = @system_qty,
            physical_qty        = @physical_qty,
            current_sale_price  = @current_sale_price,
            new_sale_price      = @new_sale_price,
            current_location    = @current_location,
            new_location        = @new_location,
            reason              = @reason,
            notes               = @notes,
            is_verified         = @is_verified
        WHERE adj_id = @adj_id AND product_id = @product_id;
    END
    ELSE
    BEGIN
        INSERT INTO pos_stk_adj_lines
            (adj_id, product_id, system_qty, physical_qty,
             current_sale_price, new_sale_price,
             current_location, new_location,
             reason, notes, is_verified)
        VALUES
            (@adj_id, @product_id, @system_qty, @physical_qty,
             @current_sale_price, @new_sale_price,
             @current_location, @new_location,
             @reason, @notes, @is_verified);
    END;
END;
GO

-- ============================================================
-- sp_StockAdjustment_DeleteLine
-- Remove a single line from a Draft session.
-- ============================================================
IF OBJECT_ID('sp_StockAdjustment_DeleteLine', 'P') IS NOT NULL
    DROP PROCEDURE sp_StockAdjustment_DeleteLine;
GO

CREATE PROCEDURE sp_StockAdjustment_DeleteLine
    @adj_id     INT,
    @product_id INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM pos_stk_adj_sessions WHERE adj_id = @adj_id AND status = 'Draft')
    BEGIN
        RAISERROR('Only Draft sessions can be edited.', 16, 1);
        RETURN;
    END;

    DELETE FROM pos_stk_adj_lines WHERE adj_id = @adj_id AND product_id = @product_id;
END;
GO

-- ============================================================
-- sp_StockAdjustment_GetSessionSummary
-- Returns header + aggregate counts for a single session.
-- ============================================================
IF OBJECT_ID('sp_StockAdjustment_GetSessionSummary', 'P') IS NOT NULL
    DROP PROCEDURE sp_StockAdjustment_GetSessionSummary;
GO

CREATE PROCEDURE sp_StockAdjustment_GetSessionSummary
    @adj_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        COUNT(l.line_id)                                                AS total_lines,
        ISNULL(SUM(CASE WHEN l.qty_difference > 0 THEN l.qty_difference  ELSE 0 END), 0) AS qty_increase,
        ISNULL(ABS(SUM(CASE WHEN l.qty_difference < 0 THEN l.qty_difference ELSE 0 END)), 0) AS qty_decrease,
        ISNULL(SUM(CASE WHEN l.price_difference <> 0 THEN 1 ELSE 0 END), 0) AS price_change_products,
        ISNULL(SUM(CASE WHEN l.new_location IS NOT NULL
                         AND l.new_location <> ISNULL(l.current_location,'') THEN 1 ELSE 0 END), 0) AS relocated_products,
        ISNULL(SUM(
            l.qty_difference * ISNULL(l.new_sale_price, l.current_sale_price)
        ), 0)                                                           AS total_stock_value_impact
    FROM  pos_stk_adj_sessions s
    LEFT  JOIN pos_stk_adj_lines l ON l.adj_id = s.adj_id
    WHERE s.adj_id = @adj_id;
END;
GO

-- ============================================================
-- sp_PostStockAdjustment
-- Validates the session, applies stock and price changes,
-- writes the full audit trail, then marks status = 'Posted'.
-- Entire operation is a single serializable transaction.
-- ============================================================
IF OBJECT_ID('sp_PostStockAdjustment', 'P') IS NOT NULL
    DROP PROCEDURE sp_PostStockAdjustment;
GO

CREATE PROCEDURE sp_PostStockAdjustment
    @adj_id     INT,
    @user_id    INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    -- ---- Guard checks ----------------------------------------
    DECLARE @status  VARCHAR(10);
    DECLARE @adj_no  VARCHAR(20);
    DECLARE @adj_type VARCHAR(30);

    SELECT @status = status, @adj_no = adj_no, @adj_type = adj_type
    FROM pos_stk_adj_sessions WITH (UPDLOCK)
    WHERE adj_id = @adj_id;

    IF @status IS NULL
    BEGIN
        ROLLBACK; RAISERROR('Adjustment session not found.', 16, 1); RETURN;
    END;
    IF @status <> 'Draft'
    BEGIN
        ROLLBACK; RAISERROR('Only Draft sessions can be posted.', 16, 1); RETURN;
    END;
    IF NOT EXISTS (SELECT 1 FROM pos_stk_adj_lines WHERE adj_id = @adj_id)
    BEGIN
        ROLLBACK; RAISERROR('Session has no lines to post.', 16, 1); RETURN;
    END;

    -- ---- Apply stock qty changes --------------------------------
    -- This assumes stock lives in a table mst_product_stock with columns
    -- (product_id, warehouse_id, qty_on_hand). Adjust the UPDATE target
    -- to match your actual stock-balance table.
    UPDATE ps SET
        ps.qty_on_hand = ps.qty_on_hand + l.qty_difference
    FROM mst_product_stock ps
    JOIN pos_stk_adj_lines l ON l.product_id = ps.product_id
    JOIN pos_stk_adj_sessions s ON s.adj_id = l.adj_id
    WHERE l.adj_id = @adj_id
      AND l.qty_difference <> 0
      AND ps.warehouse_id = s.warehouse_id;

    -- ---- Apply price changes -------------------------------------
    UPDATE p SET
        p.unit_price = l.new_sale_price
    FROM mst_products p
    JOIN pos_stk_adj_lines l ON l.product_id = p.id
    WHERE l.adj_id = @adj_id
      AND l.price_difference <> 0;

    -- ---- Apply location changes ----------------------------------
    UPDATE loc SET
        loc.aisle_code = SUBSTRING(l.new_location, 1, CHARINDEX('-', l.new_location + '--') - 1),
        loc.shelf_code = SUBSTRING(l.new_location,
                            CHARINDEX('-', l.new_location) + 1,
                            CHARINDEX('-', l.new_location, CHARINDEX('-', l.new_location) + 1)
                            - CHARINDEX('-', l.new_location) - 1),
        loc.bin_code   = REVERSE(SUBSTRING(REVERSE(l.new_location), 1,
                            CHARINDEX('-', REVERSE(l.new_location)) - 1))
    FROM mst_product_locations loc
    JOIN pos_stk_adj_lines l ON l.product_id = loc.product_id
    JOIN pos_stk_adj_sessions s ON s.adj_id = l.adj_id
    WHERE l.adj_id = @adj_id
      AND l.new_location IS NOT NULL
      AND l.new_location <> ISNULL(l.current_location, '')
      AND loc.warehouse_id = s.warehouse_id
      AND loc.is_primary = 1;

    -- ---- Write audit rows (one row per change type per line) ----
    INSERT INTO pos_stk_adj_audit
        (adj_id, product_id, change_type, old_value, new_value, changed_by, adj_no, reason)
    SELECT
        l.adj_id,
        l.product_id,
        'QTY_CHANGE',
        CAST(l.system_qty AS NVARCHAR(100)),
        CAST(l.physical_qty AS NVARCHAR(100)),
        @user_id,
        @adj_no,
        l.reason
    FROM pos_stk_adj_lines l
    WHERE l.adj_id = @adj_id AND l.qty_difference <> 0;

    INSERT INTO pos_stk_adj_audit
        (adj_id, product_id, change_type, old_value, new_value, changed_by, adj_no, reason)
    SELECT
        l.adj_id,
        l.product_id,
        'PRICE_CHANGE',
        CAST(l.current_sale_price AS NVARCHAR(100)),
        CAST(l.new_sale_price AS NVARCHAR(100)),
        @user_id,
        @adj_no,
        l.reason
    FROM pos_stk_adj_lines l
    WHERE l.adj_id = @adj_id AND l.price_difference <> 0;

    INSERT INTO pos_stk_adj_audit
        (adj_id, product_id, change_type, old_value, new_value, changed_by, adj_no, reason)
    SELECT
        l.adj_id,
        l.product_id,
        'LOCATION_CHANGE',
        ISNULL(l.current_location, ''),
        ISNULL(l.new_location, ''),
        @user_id,
        @adj_no,
        l.reason
    FROM pos_stk_adj_lines l
    WHERE l.adj_id = @adj_id
      AND l.new_location IS NOT NULL
      AND l.new_location <> ISNULL(l.current_location, '');

    -- ---- Update session counters & mark Posted ------------------
    UPDATE pos_stk_adj_sessions SET
        status              = 'Posted',
        posted_by           = @user_id,
        posted_at           = GETDATE(),
        modified_at         = GETDATE(),
        total_lines         = (SELECT COUNT(*)   FROM pos_stk_adj_lines WHERE adj_id = @adj_id),
        qty_increases       = (SELECT COUNT(*)   FROM pos_stk_adj_lines WHERE adj_id = @adj_id AND qty_difference  > 0),
        qty_decreases       = (SELECT COUNT(*)   FROM pos_stk_adj_lines WHERE adj_id = @adj_id AND qty_difference  < 0),
        price_changes       = (SELECT COUNT(*)   FROM pos_stk_adj_lines WHERE adj_id = @adj_id AND price_difference <> 0),
        location_changes    = (SELECT COUNT(*)   FROM pos_stk_adj_lines WHERE adj_id = @adj_id
                                    AND new_location IS NOT NULL
                                    AND new_location <> ISNULL(current_location,''))
    WHERE adj_id = @adj_id;

    COMMIT TRANSACTION;
END;
GO

-- ============================================================
-- sp_StockAdjustment_Reverse
-- Rolls back all stock/price/location changes made during Post,
-- writes reversal audit rows, and marks status = 'Reversed'.
-- ============================================================
IF OBJECT_ID('sp_StockAdjustment_Reverse', 'P') IS NOT NULL
    DROP PROCEDURE sp_StockAdjustment_Reverse;
GO

CREATE PROCEDURE sp_StockAdjustment_Reverse
    @adj_id     INT,
    @reason     NVARCHAR(500),
    @user_id    INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    DECLARE @status  VARCHAR(10);
    DECLARE @adj_no  VARCHAR(20);

    SELECT @status = status, @adj_no = adj_no
    FROM pos_stk_adj_sessions WITH (UPDLOCK)
    WHERE adj_id = @adj_id;

    IF @status IS NULL
    BEGIN
        ROLLBACK; RAISERROR('Adjustment session not found.', 16, 1); RETURN;
    END;
    IF @status <> 'Posted'
    BEGIN
        ROLLBACK; RAISERROR('Only Posted sessions can be reversed.', 16, 1); RETURN;
    END;

    -- Reverse stock qty changes
    UPDATE ps SET
        ps.qty_on_hand = ps.qty_on_hand - l.qty_difference
    FROM mst_product_stock ps
    JOIN pos_stk_adj_lines l ON l.product_id = ps.product_id
    JOIN stk_adj_sessions s ON s.adj_id = l.adj_id
    WHERE l.adj_id = @adj_id
      AND l.qty_difference <> 0
      AND ps.warehouse_id = s.warehouse_id;

    -- Reverse price changes
    UPDATE p SET
        p.unit_price = l.current_sale_price
    FROM mst_products p
    JOIN pos_stk_adj_lines l ON l.product_id = p.id
    WHERE l.adj_id = @adj_id
      AND l.price_difference <> 0;

    -- Write reversal audit rows (change_type prefixed REV_)
    INSERT INTO pos_stk_adj_audit
        (adj_id, product_id, change_type, old_value, new_value, changed_by, adj_no, reason)
    SELECT
        l.adj_id, l.product_id, 'QTY_CHANGE',
        CAST(l.physical_qty AS NVARCHAR(100)),
        CAST(l.system_qty   AS NVARCHAR(100)),
        @user_id, @adj_no, 'REVERSAL: ' + ISNULL(@reason,'')
    FROM pos_stk_adj_lines l
    WHERE l.adj_id = @adj_id AND l.qty_difference <> 0;

    INSERT INTO pos_stk_adj_audit
        (adj_id, product_id, change_type, old_value, new_value, changed_by, adj_no, reason)
    SELECT
        l.adj_id, l.product_id, 'PRICE_CHANGE',
        CAST(l.new_sale_price     AS NVARCHAR(100)),
        CAST(l.current_sale_price AS NVARCHAR(100)),
        @user_id, @adj_no, 'REVERSAL: ' + ISNULL(@reason,'')
    FROM pos_stk_adj_lines l
    WHERE l.adj_id = @adj_id AND l.price_difference <> 0;

    -- Mark session as Reversed
    UPDATE stk_adj_sessions SET
        status          = 'Reversed',
        reversed_by     = @user_id,
        reversed_at     = GETDATE(),
        modified_at     = GETDATE(),
        reversal_reason = @reason
    WHERE adj_id = @adj_id;

    COMMIT TRANSACTION;
END;
GO

-- ============================================================
-- sp_GetAdjustmentHistory  (updated to use stk_adj_sessions)
-- Full audit log for a product between two dates
-- ============================================================
IF OBJECT_ID('sp_GetAdjustmentHistory', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetAdjustmentHistory;
GO

CREATE PROCEDURE sp_GetAdjustmentHistory
    @ProductId  INT,
    @FromDate   DATETIME,
    @ToDate     DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        a.audit_id,
        a.adj_no,
        CONVERT(VARCHAR(10), a.changed_at, 120) AS change_date,
        CONVERT(VARCHAR(8),  a.changed_at, 108) AS change_time,
        a.changed_at,
        a.change_type,
        a.old_value,
        a.new_value,
        ISNULL(u.full_name, u.username) AS changed_by,
        a.reason,
        s.adj_type
    FROM  pos_stk_adj_audit a
    JOIN  users             u ON u.id  = a.changed_by
    LEFT  JOIN stk_adj_sessions s ON s.adj_id = a.adj_id
    WHERE a.product_id = @ProductId
      AND a.changed_at >= @FromDate
      AND a.changed_at <  DATEADD(DAY, 1, @ToDate)
    ORDER BY a.changed_at DESC;
END;
GO

-- ============================================================
-- sp_GetAdjustmentSessions  (updated to use stk_adj_sessions/lines)
-- Session list with per-session change counts
-- ============================================================
IF OBJECT_ID('sp_GetAdjustmentSessions', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetAdjustmentSessions;
GO

CREATE PROCEDURE sp_GetAdjustmentSessions
    @FromDate   DATETIME,
    @ToDate     DATETIME,
    @Status     NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.adj_id,
        s.adj_no,
        CONVERT(VARCHAR(10), s.adj_date, 120)   AS adj_date,
        s.adj_type,
        s.status,
        s.total_lines                            AS product_count,
        s.qty_increases,
        s.qty_decreases,
        s.price_changes,
        s.location_changes,
        ISNULL(uc.full_name, uc.username)        AS created_by,
        ISNULL(up.full_name, up.username)        AS posted_by,
        s.notes
    FROM  stk_adj_sessions s
    JOIN  users uc ON uc.id = s.created_by
    LEFT  JOIN users up ON up.id = s.posted_by
    WHERE s.adj_date >= @FromDate
      AND s.adj_date <  DATEADD(DAY, 1, @ToDate)
      AND (@Status IS NULL OR s.status = @Status)
    ORDER BY s.adj_date DESC, s.adj_id DESC;
END;
GO

-- ============================================================
-- sp_StockVarianceReport  (updated to use pos_stk_adj_audit + mst_products)
-- Products with most/largest adjustments
-- ============================================================
IF OBJECT_ID('sp_StockVarianceReport', 'P') IS NOT NULL
    DROP PROCEDURE sp_StockVarianceReport;
GO

CREATE PROCEDURE sp_StockVarianceReport
    @FromDate   DATETIME,
    @ToDate     DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.code                  AS product_code,
        p.name                  AS product_name,
        ISNULL(c.name, '')      AS category_name,
        COUNT(a.audit_id)       AS total_adjustments,
        ISNULL(SUM(ABS(
            TRY_CAST(a.new_value AS DECIMAL(18,4)) -
            TRY_CAST(a.old_value AS DECIMAL(18,4))
        )), 0)                  AS total_qty_adjusted,
        ISNULL(SUM(
            (TRY_CAST(a.new_value AS DECIMAL(18,4)) -
             TRY_CAST(a.old_value AS DECIMAL(18,4))) * ISNULL(p.unit_price, 0)
        ), 0)                   AS total_value_impact,
        MAX(a.changed_at)       AS last_adjustment_date
    FROM  pos_stk_adj_audit a
    JOIN  mst_products  p ON p.id = a.product_id
    LEFT  JOIN categories c ON c.id = p.category_id
    WHERE a.change_type = 'QTY_CHANGE'
      AND a.changed_at >= @FromDate
      AND a.changed_at <  DATEADD(DAY, 1, @ToDate)
    GROUP BY p.id, p.code, p.name, p.unit_price, c.name
    ORDER BY total_adjustments DESC;
END;
GO

-- ============================================================
-- sp_PriceChangeReport  (updated to use pos_stk_adj_audit + mst_products)
-- All price changes in a date range
-- ============================================================
IF OBJECT_ID('sp_PriceChangeReport', 'P') IS NOT NULL
    DROP PROCEDURE sp_PriceChangeReport;
GO

CREATE PROCEDURE sp_PriceChangeReport
    @FromDate   DATETIME,
    @ToDate     DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.code                                          AS product_code,
        p.name                                          AS product_name,
        ISNULL(c.name, '')                              AS category_name,
        a.adj_no,
        CONVERT(VARCHAR(10), a.changed_at, 120)         AS change_date,
        TRY_CAST(a.old_value AS DECIMAL(18,4))          AS old_price,
        TRY_CAST(a.new_value AS DECIMAL(18,4))          AS new_price,
        CASE WHEN TRY_CAST(a.old_value AS DECIMAL(18,4)) > 0
             THEN ROUND(
                    (TRY_CAST(a.new_value AS DECIMAL(18,4)) - TRY_CAST(a.old_value AS DECIMAL(18,4)))
                    / TRY_CAST(a.old_value AS DECIMAL(18,4)) * 100, 2)
             ELSE 0
        END                                             AS pct_change,
        ISNULL(u.full_name, u.username)                 AS approved_by,
        a.reason
    FROM  pos_stk_adj_audit a
    JOIN  mst_products p ON p.id = a.product_id
    JOIN  users        u ON u.id = a.changed_by
    LEFT  JOIN categories c ON c.id = p.category_id
    WHERE a.change_type = 'PRICE_CHANGE'
      AND a.changed_at >= @FromDate
      AND a.changed_at <  DATEADD(DAY, 1, @ToDate)
    ORDER BY a.changed_at DESC, p.name;
END;
GO
