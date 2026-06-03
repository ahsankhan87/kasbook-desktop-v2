-- ============================================================
-- Stock Check & Adjustment Module — Complete Table Schemas
-- Run order: 1) stk_adj_sessions  2) stk_adj_lines
--            3) stk_adj_audit     4) mst_product_locations
--            5) sp_GenerateAdjNo
-- ============================================================

SET NOCOUNT ON;
GO

-- ============================================================
-- 1. stk_adj_sessions — Adjustment session header
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'stk_adj_sessions')
BEGIN
    CREATE TABLE stk_adj_sessions (
        adj_id              INT             NOT NULL IDENTITY(1,1),
        adj_no              VARCHAR(20)     NOT NULL,
        adj_date            DATE            NOT NULL CONSTRAINT DF_stk_adj_sessions_adj_date     DEFAULT CAST(GETDATE() AS DATE),
        adj_type            VARCHAR(30)     NOT NULL,
        warehouse_id        INT             NOT NULL,
        status              VARCHAR(10)     NOT NULL CONSTRAINT DF_stk_adj_sessions_status        DEFAULT 'Draft',

        -- Rolling counters (updated by sp_PostStockAdjustment)
        total_lines         INT             NOT NULL CONSTRAINT DF_stk_adj_sessions_total_lines   DEFAULT 0,
        qty_increases       INT             NOT NULL CONSTRAINT DF_stk_adj_sessions_qty_inc       DEFAULT 0,
        qty_decreases       INT             NOT NULL CONSTRAINT DF_stk_adj_sessions_qty_dec       DEFAULT 0,
        price_changes       INT             NOT NULL CONSTRAINT DF_stk_adj_sessions_price_chg     DEFAULT 0,
        location_changes    INT             NOT NULL CONSTRAINT DF_stk_adj_sessions_loc_chg       DEFAULT 0,

        notes               NVARCHAR(500)   NULL,

        -- Lifecycle timestamps & actors
        created_by          INT             NOT NULL,
        created_at          DATETIME        NOT NULL CONSTRAINT DF_stk_adj_sessions_created_at    DEFAULT GETDATE(),
        modified_at         DATETIME        NULL,
        posted_by           INT             NULL,
        posted_at           DATETIME        NULL,
        reversed_by         INT             NULL,
        reversed_at         DATETIME        NULL,
        reversal_reason     NVARCHAR(500)   NULL,

        -- Constraints
        CONSTRAINT PK_stk_adj_sessions        PRIMARY KEY (adj_id),
        CONSTRAINT UQ_stk_adj_sessions_adj_no UNIQUE      (adj_no),
        CONSTRAINT CK_stk_adj_sessions_status CHECK       (status IN ('Draft','Posted','Reversed')),
        CONSTRAINT CK_stk_adj_sessions_type   CHECK       (adj_type IN (
            'Physical Count','Damage Write-Off','Found/Excess',
            'Price Update','Location Transfer','Opening Stock')),
        CONSTRAINT CK_stk_adj_sessions_counters CHECK (
            total_lines    >= 0 AND qty_increases >= 0 AND qty_decreases >= 0
            AND price_changes >= 0 AND location_changes >= 0),

        CONSTRAINT FK_stk_adj_sessions_warehouse
            FOREIGN KEY (warehouse_id) REFERENCES warehouses (id),
        CONSTRAINT FK_stk_adj_sessions_created_by
            FOREIGN KEY (created_by)  REFERENCES users (id),
        CONSTRAINT FK_stk_adj_sessions_posted_by
            FOREIGN KEY (posted_by)   REFERENCES users (id),
        CONSTRAINT FK_stk_adj_sessions_reversed_by
            FOREIGN KEY (reversed_by) REFERENCES users (id)
    );

    -- Indexes
    CREATE INDEX IX_stk_adj_sessions_adj_date
        ON stk_adj_sessions (adj_date DESC);

    CREATE INDEX IX_stk_adj_sessions_status
        ON stk_adj_sessions (status, adj_date DESC);

    CREATE INDEX IX_stk_adj_sessions_warehouse
        ON stk_adj_sessions (warehouse_id, adj_date DESC);

    CREATE INDEX IX_stk_adj_sessions_created_by
        ON stk_adj_sessions (created_by);

    PRINT 'Created table: stk_adj_sessions';
END
ELSE
    PRINT 'Table already exists: stk_adj_sessions';
GO

-- ============================================================
-- 2. stk_adj_lines — Individual product adjustment lines
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'stk_adj_lines')
BEGIN
    CREATE TABLE stk_adj_lines (
        line_id                 INT             NOT NULL IDENTITY(1,1),
        adj_id                  INT             NOT NULL,
        product_id              INT             NOT NULL,

        -- Stock quantities
        system_qty              DECIMAL(18,3)   NOT NULL CONSTRAINT DF_stk_adj_lines_system_qty  DEFAULT 0,
        physical_qty            DECIMAL(18,3)   NOT NULL CONSTRAINT DF_stk_adj_lines_phys_qty    DEFAULT 0,
        qty_difference          AS (physical_qty - system_qty) PERSISTED,

        -- Pricing
        current_sale_price      DECIMAL(18,2)   NOT NULL CONSTRAINT DF_stk_adj_lines_cur_price   DEFAULT 0,
        new_sale_price          DECIMAL(18,2)   NOT NULL CONSTRAINT DF_stk_adj_lines_new_price   DEFAULT 0,
        price_difference        AS (new_sale_price - current_sale_price) PERSISTED,

        -- Location
        current_location        NVARCHAR(30)    NULL,
        new_location            NVARCHAR(30)    NULL,

        -- Metadata
        reason                  VARCHAR(30)     NULL,
        notes                   NVARCHAR(300)   NULL,
        is_verified             BIT             NOT NULL CONSTRAINT DF_stk_adj_lines_verified     DEFAULT 0,

        -- Constraints
        CONSTRAINT PK_stk_adj_lines               PRIMARY KEY (line_id),
        CONSTRAINT UQ_stk_adj_lines_adj_product   UNIQUE      (adj_id, product_id),
        CONSTRAINT CK_stk_adj_lines_system_qty    CHECK       (system_qty   >= 0),
        CONSTRAINT CK_stk_adj_lines_physical_qty  CHECK       (physical_qty >= 0),
        CONSTRAINT CK_stk_adj_lines_cur_price     CHECK       (current_sale_price >= 0),
        CONSTRAINT CK_stk_adj_lines_new_price     CHECK       (new_sale_price     >= 0),
        CONSTRAINT CK_stk_adj_lines_reason        CHECK       (reason IS NULL OR reason IN (
            'Physical Count','Damage','Found','Price Correction','Relocation','Opening Stock','Other')),

        CONSTRAINT FK_stk_adj_lines_adj
            FOREIGN KEY (adj_id)     REFERENCES stk_adj_sessions (adj_id),
        CONSTRAINT FK_stk_adj_lines_product
            FOREIGN KEY (product_id) REFERENCES mst_products (id)
    );

    -- Indexes
    CREATE INDEX IX_stk_adj_lines_adj_id
        ON stk_adj_lines (adj_id);

    CREATE INDEX IX_stk_adj_lines_product_id
        ON stk_adj_lines (product_id);

    CREATE INDEX IX_stk_adj_lines_verified
        ON stk_adj_lines (adj_id, is_verified);

    CREATE INDEX IX_stk_adj_lines_new_location
        ON stk_adj_lines (new_location) WHERE new_location IS NOT NULL;

    PRINT 'Created table: stk_adj_lines';
END
ELSE
    PRINT 'Table already exists: stk_adj_lines';
GO

-- ============================================================
-- 3. stk_adj_audit — Immutable append-only audit log
--    Never UPDATE or DELETE rows from this table.
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'stk_adj_audit')
BEGIN
    CREATE TABLE stk_adj_audit (
        audit_id    BIGINT          NOT NULL IDENTITY(1,1),
        adj_id      INT             NOT NULL,
        product_id  INT             NOT NULL,
        change_type VARCHAR(20)     NOT NULL,
        old_value   NVARCHAR(100)   NOT NULL CONSTRAINT DF_stk_adj_audit_old_value DEFAULT '',
        new_value   NVARCHAR(100)   NOT NULL CONSTRAINT DF_stk_adj_audit_new_value DEFAULT '',
        changed_by  INT             NOT NULL,
        changed_at  DATETIME        NOT NULL CONSTRAINT DF_stk_adj_audit_changed_at DEFAULT GETDATE(),
        adj_no      VARCHAR(20)     NOT NULL CONSTRAINT DF_stk_adj_audit_adj_no    DEFAULT '',
        reason      NVARCHAR(300)   NULL,

        -- Constraints
        CONSTRAINT PK_stk_adj_audit            PRIMARY KEY (audit_id),
        CONSTRAINT CK_stk_adj_audit_change_type CHECK (change_type IN (
            'QTY_CHANGE','PRICE_CHANGE','LOCATION_CHANGE')),

        CONSTRAINT FK_stk_adj_audit_adj
            FOREIGN KEY (adj_id)     REFERENCES stk_adj_sessions (adj_id),
        CONSTRAINT FK_stk_adj_audit_product
            FOREIGN KEY (product_id) REFERENCES mst_products (id),
        CONSTRAINT FK_stk_adj_audit_changed_by
            FOREIGN KEY (changed_by) REFERENCES users (id)
    );

    -- Deny UPDATE/DELETE via DDL trigger to enforce immutability
    PRINT 'Created table: stk_adj_audit';
END
ELSE
    PRINT 'Table already exists: stk_adj_audit';
GO

-- Immutability trigger — prevents UPDATE and DELETE on the audit log
IF OBJECT_ID('TR_stk_adj_audit_immutable', 'TR') IS NOT NULL
    DROP TRIGGER TR_stk_adj_audit_immutable;
GO

CREATE TRIGGER TR_stk_adj_audit_immutable
ON stk_adj_audit
INSTEAD OF UPDATE, DELETE
AS
BEGIN
    RAISERROR('Audit log rows are immutable and cannot be modified or deleted.', 16, 1);
    ROLLBACK TRANSACTION;
END;
GO

-- Indexes on stk_adj_audit
CREATE INDEX IX_stk_adj_audit_product_at
    ON stk_adj_audit (product_id, changed_at DESC);
GO

CREATE INDEX IX_stk_adj_audit_adj_id
    ON stk_adj_audit (adj_id);
GO

CREATE INDEX IX_stk_adj_audit_change_type
    ON stk_adj_audit (change_type, changed_at DESC);
GO

CREATE INDEX IX_stk_adj_audit_changed_by
    ON stk_adj_audit (changed_by, changed_at DESC);
GO

CREATE INDEX IX_stk_adj_audit_adj_no
    ON stk_adj_audit (adj_no);
GO

-- ============================================================
-- 4. mst_product_locations — Multi-location stock per bin
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'mst_product_locations')
BEGIN
    CREATE TABLE mst_product_locations (
        loc_id              INT             NOT NULL IDENTITY(1,1),
        product_id          INT             NOT NULL,
        warehouse_id        INT             NOT NULL,

        -- Location hierarchy
        aisle_code          VARCHAR(10)     NOT NULL,
        shelf_code          VARCHAR(10)     NOT NULL,
        bin_code            VARCHAR(10)     NOT NULL,

        -- Computed full location code: e.g. A1-S2-B3
        location_code       AS (aisle_code + '-' + shelf_code + '-' + bin_code) PERSISTED,

        qty_at_location     DECIMAL(18,3)   NOT NULL CONSTRAINT DF_mst_product_locs_qty DEFAULT 0,

        is_primary          BIT             NOT NULL CONSTRAINT DF_mst_product_locs_primary DEFAULT 0,
        last_counted_at     DATETIME        NULL,
        last_counted_by     INT             NULL,

        -- Constraints
        CONSTRAINT PK_mst_product_locations       PRIMARY KEY (loc_id),
        CONSTRAINT UQ_mst_product_locations_bin   UNIQUE      (product_id, warehouse_id, aisle_code, shelf_code, bin_code),
        CONSTRAINT CK_mst_product_locs_qty        CHECK       (qty_at_location >= 0),

        CONSTRAINT FK_mst_product_locs_product
            FOREIGN KEY (product_id)  REFERENCES mst_products (id),
        CONSTRAINT FK_mst_product_locs_warehouse
            FOREIGN KEY (warehouse_id) REFERENCES warehouses (id),
        CONSTRAINT FK_mst_product_locs_counted_by
            FOREIGN KEY (last_counted_by) REFERENCES users (id)
    );

    -- Indexes
    CREATE INDEX IX_mst_product_locs_product
        ON mst_product_locations (product_id);

    CREATE INDEX IX_mst_product_locs_warehouse
        ON mst_product_locations (warehouse_id);

    CREATE INDEX IX_mst_product_locs_location_code
        ON mst_product_locations (location_code);

    CREATE INDEX IX_mst_product_locs_primary
        ON mst_product_locations (product_id, is_primary) WHERE is_primary = 1;

    PRINT 'Created table: mst_product_locations';
END
ELSE
    PRINT 'Table already exists: mst_product_locations';
GO

-- ============================================================
-- 5. sp_GenerateAdjNo
--    Returns the next adjustment number: ADJ-YYYY-NNNN
--    Thread-safe via UPDLOCK/HOLDLOCK to prevent duplicates
--    under concurrent sessions.
-- ============================================================
IF OBJECT_ID('sp_GenerateAdjNo', 'P') IS NOT NULL
    DROP PROCEDURE sp_GenerateAdjNo;
GO

CREATE PROCEDURE sp_GenerateAdjNo
    @AdjType    VARCHAR(30),
    @AdjNo      VARCHAR(20) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Year       CHAR(4)     = CAST(YEAR(GETDATE()) AS CHAR(4));
    DECLARE @Prefix     VARCHAR(8);
    DECLARE @NextSeq    INT;
    DECLARE @Pattern    VARCHAR(25);

    -- Derive a 3-letter prefix from the adjustment type
    SET @Prefix = CASE @AdjType
        WHEN 'Physical Count'    THEN 'PHY'
        WHEN 'Damage Write-Off'  THEN 'DAM'
        WHEN 'Found/Excess'      THEN 'FND'
        WHEN 'Price Update'      THEN 'PRC'
        WHEN 'Location Transfer' THEN 'LOC'
        WHEN 'Opening Stock'     THEN 'OPN'
        ELSE                          'ADJ'
    END;

    SET @Pattern = @Prefix + '-' + @Year + '-%';

    BEGIN TRANSACTION;

        -- Lock the most recent matching row to prevent race conditions
        SELECT @NextSeq = ISNULL(MAX(
            TRY_CAST(
                SUBSTRING(adj_no,
                    LEN(@Prefix) + LEN(@Year) + 3,   -- skip PREFIX-YYYY-
                    4)
            AS INT)), 0) + 1
        FROM stk_adj_sessions WITH (UPDLOCK, HOLDLOCK)
        WHERE adj_no LIKE @Pattern;

    COMMIT TRANSACTION;

    SET @AdjNo = @Prefix + '-' + @Year + '-' + RIGHT('0000' + CAST(@NextSeq AS VARCHAR(10)), 4);
END;
GO

-- ============================================================
-- 6. Useful views
-- ============================================================

-- v_stk_adj_session_summary — joins sessions with line aggregates
IF OBJECT_ID('v_stk_adj_session_summary', 'V') IS NOT NULL
    DROP VIEW v_stk_adj_session_summary;
GO

CREATE VIEW v_stk_adj_session_summary AS
SELECT
    s.adj_id,
    s.adj_no,
    s.adj_date,
    s.adj_type,
    s.status,
    s.warehouse_id,
    w.name                              AS warehouse_name,
    s.notes,
    COUNT(l.line_id)                    AS line_count,
    SUM(CASE WHEN l.qty_difference > 0 THEN 1 ELSE 0 END)  AS qty_increase_lines,
    SUM(CASE WHEN l.qty_difference < 0 THEN 1 ELSE 0 END)  AS qty_decrease_lines,
    SUM(CASE WHEN l.price_difference <> 0 THEN 1 ELSE 0 END) AS price_change_lines,
    SUM(CASE WHEN l.new_location IS NOT NULL
              AND l.new_location <> ISNULL(l.current_location,'') THEN 1 ELSE 0 END) AS location_change_lines,
    SUM(CASE WHEN l.is_verified = 1 THEN 1 ELSE 0 END)     AS verified_lines,
    ISNULL(uc.full_name, uc.username)   AS created_by,
    s.created_at,
    ISNULL(up.full_name, up.username)   AS posted_by,
    s.posted_at
FROM  stk_adj_sessions s
JOIN  warehouses w  ON w.id  = s.warehouse_id
JOIN  users      uc ON uc.id = s.created_by
LEFT  JOIN users up ON up.id = s.posted_by
LEFT  JOIN stk_adj_lines l ON l.adj_id = s.adj_id
GROUP BY
    s.adj_id, s.adj_no, s.adj_date, s.adj_type, s.status,
    s.warehouse_id, w.name, s.notes, s.created_at, s.posted_at,
    uc.full_name, uc.username, up.full_name, up.username;
GO

-- v_stk_adj_audit_detail — human-readable audit feed
IF OBJECT_ID('v_stk_adj_audit_detail', 'V') IS NOT NULL
    DROP VIEW v_stk_adj_audit_detail;
GO

CREATE VIEW v_stk_adj_audit_detail AS
SELECT
    a.audit_id,
    a.adj_no,
    a.changed_at,
    CONVERT(VARCHAR(10), a.changed_at, 120)   AS change_date,
    CONVERT(VARCHAR(8),  a.changed_at, 108)   AS change_time,
    CASE a.change_type
        WHEN 'QTY_CHANGE'      THEN 'Qty Change'
        WHEN 'PRICE_CHANGE'    THEN 'Price Change'
        WHEN 'LOCATION_CHANGE' THEN 'Location Change'
        ELSE a.change_type
    END                                        AS change_label,
    a.change_type,
    p.code                                     AS product_code,
    p.name                                     AS product_name,
    a.old_value,
    a.new_value,
    ISNULL(u.full_name, u.username)            AS changed_by,
    a.reason,
    s.adj_type,
    s.status                                   AS session_status
FROM  stk_adj_audit     a
JOIN  mst_products      p ON p.id  = a.product_id
JOIN  users             u ON u.id  = a.changed_by
JOIN  stk_adj_sessions  s ON s.adj_id = a.adj_id;
GO
