CREATE OR ALTER PROCEDURE dbo.sp_SupplierDashboardKPIs
    @branch_id INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Today DATE = CAST(GETDATE() AS DATE);
    DECLARE @MonthStart DATE = DATEFROMPARTS(YEAR(@Today), MONTH(@Today), 1);
    DECLARE @NextMonthStart DATE = DATEADD(MONTH, 1, @MonthStart);
    DECLARE @PrevMonthStart DATE = DATEADD(MONTH, -1, @MonthStart);

    ;WITH P AS
    (
        SELECT
            p.supplier_id,
            p.purchase_date,
            p.due_date,
            total_amount = CAST(ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0) AS DECIMAL(18,2)),
            paid_amount  = CAST(ISNULL(SUM(sp.debit), 0) AS DECIMAL(18,2))
        FROM pos_purchases p WITH (NOLOCK)
        LEFT JOIN pos_suppliers_payments sp WITH (NOLOCK)
            ON sp.branch_id = p.branch_id
           AND sp.supplier_id = p.supplier_id
           AND sp.invoice_no = p.invoice_no
        WHERE p.branch_id = @branch_id
          
        GROUP BY p.supplier_id, p.purchase_date, p.due_date, p.total_amount, p.total_tax, p.discount_value
    ),
    Bal AS
    (
        SELECT
            supplier_id,
            purchase_date,
            due_date,
            balance = total_amount - paid_amount
        FROM P
        WHERE (total_amount - paid_amount) > 0.004
    ),
    PurchMonth AS
    (
        SELECT
            ThisMonth = SUM(CASE WHEN p.purchase_date >= @MonthStart AND p.purchase_date < @NextMonthStart THEN p.total_amount ELSE 0 END),
            PrevMonth = SUM(CASE WHEN p.purchase_date >= @PrevMonthStart AND p.purchase_date < @MonthStart THEN p.total_amount ELSE 0 END)
        FROM P p
    )
    SELECT
        TotalSuppliers = (
            SELECT COUNT(1)
            FROM pos_suppliers s WITH (NOLOCK)
            WHERE s.branch_id = @branch_id
              AND ISNULL(s.status, 1) = 1
        ),
        TotalPayables = ISNULL((SELECT SUM(balance) FROM Bal), 0),
        OverduePayables = ISNULL((SELECT SUM(balance) FROM Bal WHERE due_date < @Today), 0),
        TotalPurchasesThisMonth = ISNULL((SELECT ThisMonth FROM PurchMonth), 0),
        TotalPurchasesPrevMonth = ISNULL((SELECT PrevMonth FROM PurchMonth), 0);
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_SupplierSummaryList
    @branch_id INT,
    @SearchText NVARCHAR(200) = NULL,
    @Category NVARCHAR(100) = NULL,
    @StatusFilter VARCHAR(20) = 'All'
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Today DATE = CAST(GETDATE() AS DATE);

    ;WITH S AS
    (
        SELECT
            s.id AS supplier_id,
            ISNULL(s.supplier_code, '') AS supplier_code,
            ISNULL(s.first_name, '') AS first_name,
            ISNULL(s.last_name, '') AS last_name,
            LTRIM(RTRIM(ISNULL(s.first_name, '') + ' ' + ISNULL(s.last_name, ''))) AS supplier_name,
            ISNULL(s.address, '') AS address,
            ISNULL(s.contact_no, '') AS contact_no,
            ISNULL(s.status, 1) AS is_active,
            ISNULL(s.date_created, GETDATE()) AS date_created,
            category = ISNULL(
                        NULLIF(LTRIM(RTRIM(
                            CASE
                                WHEN CHARINDEX('|', ISNULL(s.address,'')) > 0 THEN LEFT(s.address, CHARINDEX('|', s.address)-1)
                                WHEN CHARINDEX(',', ISNULL(s.address,'')) > 0 THEN LEFT(s.address, CHARINDEX(',', s.address)-1)
                                ELSE ''
                            END
                        )), ''),
                        'General')
        FROM pos_suppliers s WITH (NOLOCK)
        WHERE s.branch_id = @branch_id
    ),
    P AS
    (
        SELECT
            p.supplier_id,
            p.purchase_date,
            p.due_date,
            total_amount = CAST(ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0) AS DECIMAL(18,2)),
            paid_amount  = CAST(ISNULL(SUM(sp.debit), 0) AS DECIMAL(18,2))
        FROM pos_purchases p WITH (NOLOCK)
        LEFT JOIN pos_suppliers_payments sp WITH (NOLOCK)
            ON sp.branch_id = p.branch_id
           AND sp.supplier_id = p.supplier_id
           AND sp.invoice_no = p.invoice_no
        WHERE p.branch_id = @branch_id
          
        GROUP BY p.supplier_id, p.purchase_date, p.due_date, p.total_amount, p.total_tax, p.discount_value
    ),
    Agg AS
    (
        SELECT
            supplier_id,
            TotalPurchases = SUM(total_amount),
            PayableBalance = SUM(total_amount - paid_amount),
            LastBillDate = MAX(purchase_date),
            IsOverdue = MAX(CASE WHEN (total_amount - paid_amount) > 0.004 AND due_date < @Today THEN 1 ELSE 0 END)
        FROM P
        GROUP BY supplier_id
    )
    SELECT
        s.supplier_id,
        s.supplier_code,
        s.supplier_name,
        s.category,
        s.contact_no,
        s.address,
        TotalPurchases = CAST(ISNULL(a.TotalPurchases, 0) AS DECIMAL(18,2)),
        PayableBalance = CAST(ISNULL(a.PayableBalance, 0) AS DECIMAL(18,2)),
        CreditDays = ISNULL((
            SELECT TOP 1 DATEDIFF(DAY, p2.purchase_date, p2.due_date)
            FROM pos_purchases p2 WITH (NOLOCK)
            WHERE p2.branch_id = @branch_id
              AND p2.supplier_id = s.supplier_id
              AND p2.due_date IS NOT NULL
            ORDER BY p2.purchase_date DESC, p2.id DESC
        ), 0),
        CreditLimit = CAST(0 AS DECIMAL(18,2)),
        LastBillDate = a.LastBillDate,
        DaysSinceLastPurchase = CASE WHEN a.LastBillDate IS NULL THEN NULL ELSE DATEDIFF(DAY, a.LastBillDate, @Today) END,
        IsActive = s.is_active,
        IsOverdue = ISNULL(a.IsOverdue, 0),
        TotalPaid = CAST(ISNULL(a.TotalPurchases, 0) - ISNULL(a.PayableBalance, 0) AS DECIMAL(18,2))
    FROM S s
    LEFT JOIN Agg a ON a.supplier_id = s.supplier_id
    WHERE
        (
            @SearchText IS NULL OR LTRIM(RTRIM(@SearchText)) = ''
            OR s.supplier_name LIKE '%' + @SearchText + '%'
            OR s.supplier_code LIKE '%' + @SearchText + '%'
        )
        AND
        (
            @Category IS NULL OR LTRIM(RTRIM(@Category)) = ''
            OR s.category = @Category
        )
        AND
        (
            @StatusFilter = 'All'
            OR (@StatusFilter = 'Active' AND s.is_active = 1)
            OR (@StatusFilter = 'Inactive' AND s.is_active = 0)
            OR (@StatusFilter = 'Overdue' AND ISNULL(a.IsOverdue, 0) = 1)
        )
    ORDER BY
        ISNULL(a.IsOverdue, 0) DESC,
        ISNULL(a.PayableBalance, 0) DESC,
        s.supplier_name;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_SupplierRecentBills
    @branch_id INT,
    @SupplierId INT,
    @Top INT = 5
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@Top)
        p.invoice_no,
        p.purchase_date,
        total_amount = CAST(ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0) AS DECIMAL(18,2)),
        paid_amount = CAST(ISNULL(SUM(sp.debit), 0) AS DECIMAL(18,2)),
        balance = CAST((ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0)) - ISNULL(SUM(sp.debit), 0) AS DECIMAL(18,2))
    FROM pos_purchases p WITH (NOLOCK)
    LEFT JOIN pos_suppliers_payments sp WITH (NOLOCK)
        ON sp.branch_id = p.branch_id
       AND sp.supplier_id = p.supplier_id
       AND sp.invoice_no = p.invoice_no
    WHERE p.branch_id = @branch_id
      AND p.supplier_id = @SupplierId
      
    GROUP BY p.invoice_no, p.purchase_date, p.total_amount, p.total_tax, p.discount_value
    ORDER BY p.purchase_date DESC, p.invoice_no DESC;
END
GO