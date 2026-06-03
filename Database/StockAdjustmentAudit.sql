-- ============================================================
-- Stock Adjustment Audit Table and Reporting Procedures
-- ============================================================

-- 1. Audit table
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'stk_adj_audit')
BEGIN
    CREATE TABLE stk_adj_audit (
        audit_id    INT             NOT NULL IDENTITY(1,1) PRIMARY KEY,
        adj_id      INT             NOT NULL,
        product_id  INT             NOT NULL,
        change_type NVARCHAR(20)    NOT NULL,   -- 'QTY_CHANGE','PRICE_CHANGE','LOCATION_CHANGE'
        old_value   NVARCHAR(200)   NOT NULL DEFAULT '',
        new_value   NVARCHAR(200)   NOT NULL DEFAULT '',
        changed_by  INT             NOT NULL,
        changed_at  DATETIME        NOT NULL DEFAULT GETDATE(),
        reason      NVARCHAR(200)   NULL,
        adj_no      NVARCHAR(50)    NOT NULL DEFAULT ''
    );

    CREATE INDEX IX_stk_adj_audit_product ON stk_adj_audit (product_id, changed_at);
    CREATE INDEX IX_stk_adj_audit_adj     ON stk_adj_audit (adj_id);
END
GO

-- ============================================================
-- sp_GetAdjustmentHistory
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
    FROM  stk_adj_audit a
    JOIN  users         u ON u.id = a.changed_by
    LEFT  JOIN stock_adjustment_sessions s ON s.adj_id = a.adj_id
    WHERE a.product_id = @ProductId
      AND a.changed_at >= @FromDate
      AND a.changed_at <  DATEADD(DAY, 1, @ToDate)
    ORDER BY a.changed_at DESC;
END
GO

-- ============================================================
-- sp_GetAdjustmentSessions
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
        CONVERT(VARCHAR(10), s.adj_date, 120)       AS adj_date,
        s.adj_type,
        s.status,
        COUNT(DISTINCT l.product_id)                 AS product_count,
        ISNULL(SUM(CASE WHEN a.change_type = 'QTY_CHANGE'
                         AND TRY_CAST(a.new_value AS DECIMAL(18,4)) > TRY_CAST(a.old_value AS DECIMAL(18,4))
                        THEN 1 ELSE 0 END), 0)       AS qty_increases,
        ISNULL(SUM(CASE WHEN a.change_type = 'QTY_CHANGE'
                         AND TRY_CAST(a.new_value AS DECIMAL(18,4)) < TRY_CAST(a.old_value AS DECIMAL(18,4))
                        THEN 1 ELSE 0 END), 0)       AS qty_decreases,
        ISNULL(SUM(CASE WHEN a.change_type = 'PRICE_CHANGE'    THEN 1 ELSE 0 END), 0) AS price_changes,
        ISNULL(SUM(CASE WHEN a.change_type = 'LOCATION_CHANGE' THEN 1 ELSE 0 END), 0) AS location_changes,
        ISNULL(uc.full_name, uc.username)            AS created_by,
        ISNULL(up.full_name, up.username)            AS posted_by,
        s.notes
    FROM  stock_adjustment_sessions s
    JOIN  users uc ON uc.id = s.created_by
    LEFT  JOIN users up ON up.id = s.posted_by
    LEFT  JOIN stock_adjustment_lines l  ON l.adj_id = s.adj_id
    LEFT  JOIN stk_adj_audit           a ON a.adj_id = s.adj_id
    WHERE s.adj_date >= @FromDate
      AND s.adj_date <  DATEADD(DAY, 1, @ToDate)
      AND (@Status IS NULL OR s.status = @Status)
    GROUP BY
        s.adj_id, s.adj_no, s.adj_date, s.adj_type, s.status, s.notes,
        uc.full_name, uc.username, up.full_name, up.username
    ORDER BY s.adj_date DESC, s.adj_id DESC;
END
GO

-- ============================================================
-- sp_StockVarianceReport
-- Products with most/largest adjustments (theft/damage patterns)
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
    FROM  stk_adj_audit a
    JOIN  products p  ON p.id   = a.product_id
    LEFT  JOIN categories c ON c.id = p.category_id
    WHERE a.change_type = 'QTY_CHANGE'
      AND a.changed_at >= @FromDate
      AND a.changed_at <  DATEADD(DAY, 1, @ToDate)
    GROUP BY p.id, p.code, p.name, p.unit_price, c.name
    ORDER BY total_adjustments DESC, ABS(ISNULL(SUM(
        (TRY_CAST(a.new_value AS DECIMAL(18,4)) -
         TRY_CAST(a.old_value AS DECIMAL(18,4))) * ISNULL(p.unit_price, 0)), 0)) DESC;
END
GO

-- ============================================================
-- sp_PriceChangeReport
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
        p.code                          AS product_code,
        p.name                          AS product_name,
        ISNULL(c.name, '')              AS category_name,
        a.adj_no,
        CONVERT(VARCHAR(10), a.changed_at, 120)     AS change_date,
        TRY_CAST(a.old_value AS DECIMAL(18,4))      AS old_price,
        TRY_CAST(a.new_value AS DECIMAL(18,4))      AS new_price,
        CASE WHEN TRY_CAST(a.old_value AS DECIMAL(18,4)) > 0
             THEN ROUND(
                    (TRY_CAST(a.new_value AS DECIMAL(18,4)) - TRY_CAST(a.old_value AS DECIMAL(18,4)))
                    / TRY_CAST(a.old_value AS DECIMAL(18,4)) * 100, 2)
             ELSE 0
        END                             AS pct_change,
        ISNULL(u.full_name, u.username) AS approved_by,
        a.reason
    FROM  stk_adj_audit a
    JOIN  products p  ON p.id = a.product_id
    JOIN  users    u  ON u.id = a.changed_by
    LEFT  JOIN categories c ON c.id = p.category_id
    WHERE a.change_type = 'PRICE_CHANGE'
      AND a.changed_at >= @FromDate
      AND a.changed_at <  DATEADD(DAY, 1, @ToDate)
    ORDER BY a.changed_at DESC, p.name;
END
GO
