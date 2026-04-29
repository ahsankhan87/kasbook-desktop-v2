using System;
using System.Data;
using System.Data.SqlClient;
using POS.Core;

namespace POS.DLL
{
    public class VatDashboardDLL
    {
        public DataTable GetCompanySummary(DateTime from, DateTime to)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
                DECLARE @from DATETIME = @pFrom;
                DECLARE @to   DATETIME = @pTo;

                ;WITH SalesAgg AS (
                    SELECT
                        Docs = COUNT(1),
                        NetAmount = ISNULL(SUM(ABS(ISNULL(total_amount,0) - ISNULL(discount_value,0))),0),
                        VatAmount = ISNULL(SUM(ABS(ISNULL(total_tax,0))),0)
                    FROM pos_sales
                    WHERE sale_date >= @from AND sale_date < DATEADD(day,1,@to)
                      AND account <> 'Return'
                      AND ISNULL(invoice_no,'') NOT LIKE 'ZS%'
                ),
                SalesReturnAgg AS (
                    SELECT
                        Docs = COUNT(1),
                        NetAmount = ISNULL(SUM(ABS(ISNULL(total_amount,0) - ISNULL(discount_value,0))),0),
                        VatAmount = ISNULL(SUM(ABS(ISNULL(total_tax,0))),0)
                    FROM pos_sales
                    WHERE sale_date >= @from AND sale_date < DATEADD(day,1,@to)
                      AND account = 'Return'
                      AND ISNULL(invoice_no,'') NOT LIKE 'ZS%'
                ),
                PurchAgg AS (
                    SELECT
                        Docs = COUNT(1),
                        NetAmount = ISNULL(SUM(ABS(ISNULL(total_amount,0) - ISNULL(discount_value,0))),0),
                        VatAmount = ISNULL(SUM(ABS(ISNULL(total_tax,0))),0)
                    FROM pos_purchases
                    WHERE purchase_date >= @from AND purchase_date < DATEADD(day,1,@to)
                      AND account <> 'Return'
                      AND ISNULL(invoice_no,'') NOT LIKE 'ZS%'
                ),
                PurchReturnAgg AS (
                    SELECT
                        Docs = COUNT(1),
                        NetAmount = ISNULL(SUM(ABS(ISNULL(total_amount,0) - ISNULL(discount_value,0))),0),
                        VatAmount = ISNULL(SUM(ABS(ISNULL(total_tax,0))),0)
                    FROM pos_purchases
                    WHERE purchase_date >= @from AND purchase_date < DATEADD(day,1,@to)
                      AND account = 'Return'
                      AND ISNULL(invoice_no,'') NOT LIKE 'ZS%'
                )
                SELECT * FROM (
                    SELECT
                        Terms = 'Sales',
                        Docs = (SELECT Docs FROM SalesAgg),
                        Flag = 'Sales',
                        NetAmount = (SELECT NetAmount FROM SalesAgg),
                        VatAmount = (SELECT VatAmount FROM SalesAgg),
                        SortOrder = 1
                    UNION ALL
                    SELECT
                        Terms = 'Sales Return',
                        Docs = (SELECT Docs FROM SalesReturnAgg),
                        Flag = 'Return',
                        NetAmount = (SELECT NetAmount FROM SalesReturnAgg) * -1,
                        VatAmount = (SELECT VatAmount FROM SalesReturnAgg) * -1,
                        SortOrder = 2
                    UNION ALL
                    SELECT
                        Terms = 'Purchases',
                        Docs = (SELECT Docs FROM PurchAgg),
                        Flag = 'Purchase',
                        NetAmount = (SELECT NetAmount FROM PurchAgg) * -1,
                        VatAmount = (SELECT VatAmount FROM PurchAgg) * -1,
                        SortOrder = 3
                    UNION ALL
                    SELECT
                        Terms = 'Purchase Return',
                        Docs = (SELECT Docs FROM PurchReturnAgg),
                        Flag = 'Return',
                        NetAmount = (SELECT NetAmount FROM PurchReturnAgg),
                        VatAmount = (SELECT VatAmount FROM PurchReturnAgg),
                        SortOrder = 4
                ) x
                ORDER BY SortOrder;";

                cmd.Parameters.AddWithValue("@pFrom", from.Date);
                cmd.Parameters.AddWithValue("@pTo", to.Date);

                var dt = new DataTable();
                cn.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;
            }
        }

        public DataTable GetBranchMovement(DateTime from, DateTime to)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
                DECLARE @from DATETIME = @pFrom;
                DECLARE @to   DATETIME = @pTo;

                ;WITH SalesAgg AS (
                    SELECT
                        branch_id,
                        Docs = COUNT(1),
                        NetAmount = ISNULL(SUM(ABS(ISNULL(total_amount,0) - ISNULL(discount_value,0))),0),
                        VatAmount = ISNULL(SUM(ABS(ISNULL(total_tax,0))),0)
                    FROM pos_sales
                    WHERE sale_date >= @from AND sale_date < DATEADD(day,1,@to)
                      AND account <> 'Return'
                      AND ISNULL(invoice_no,'') NOT LIKE 'ZS%'
                    GROUP BY branch_id
                ),
                SalesReturnAgg AS (
                    SELECT
                        branch_id,
                        Docs = COUNT(1),
                        NetAmount = ISNULL(SUM(ABS(ISNULL(total_amount,0) - ISNULL(discount_value,0))),0),
                        VatAmount = ISNULL(SUM(ABS(ISNULL(total_tax,0))),0)
                    FROM pos_sales
                    WHERE sale_date >= @from AND sale_date < DATEADD(day,1,@to)
                      AND account = 'Return'
                      AND ISNULL(invoice_no,'') NOT LIKE 'ZS%'
                    GROUP BY branch_id
                ),
                PurchAgg AS (
                    SELECT
                        branch_id,
                        Docs = COUNT(1),
                        NetAmount = ISNULL(SUM(ABS(ISNULL(total_amount,0) - ISNULL(discount_value,0))),0),
                        VatAmount = ISNULL(SUM(ABS(ISNULL(total_tax,0))),0)
                    FROM pos_purchases
                    WHERE purchase_date >= @from AND purchase_date < DATEADD(day,1,@to)
                      AND account <> 'Return'
                      AND ISNULL(invoice_no,'') NOT LIKE 'ZS%'
                    GROUP BY branch_id
                ),
                PurchReturnAgg AS (
                    SELECT
                        branch_id,
                        Docs = COUNT(1),
                        NetAmount = ISNULL(SUM(ABS(ISNULL(total_amount,0) - ISNULL(discount_value,0))),0),
                        VatAmount = ISNULL(SUM(ABS(ISNULL(total_tax,0))),0)
                    FROM pos_purchases
                    WHERE purchase_date >= @from AND purchase_date < DATEADD(day,1,@to)
                      AND account = 'Return'
                      AND ISNULL(invoice_no,'') NOT LIKE 'ZS%'
                    GROUP BY branch_id
                ),
                Branches AS (
                    SELECT id AS branch_id, name
                    FROM pos_branches
                )
                SELECT
                    Terms = b.name,
                    Doc = ISNULL(sa.Docs,0) + ISNULL(sr.Docs,0) + ISNULL(pa.Docs,0) + ISNULL(pr.Docs,0),
                    [Brn-ID] = RIGHT('00' + CAST(b.branch_id AS varchar(10)), 2),

                    -- Net payable base (sales base - purchases base) with returns applied
                    NetAmount = (ISNULL(sa.NetAmount,0) - ISNULL(sr.NetAmount,0)) - (ISNULL(pa.NetAmount,0) - ISNULL(pr.NetAmount,0)),

                    -- VAT payable (VAT on sales - VAT on purchases) with returns applied
                    VatAmount = (ISNULL(sa.VatAmount,0) - ISNULL(sr.VatAmount,0)) - (ISNULL(pa.VatAmount,0) - ISNULL(pr.VatAmount,0))
                FROM Branches b
                LEFT JOIN SalesAgg sa ON sa.branch_id = b.branch_id
                LEFT JOIN SalesReturnAgg sr ON sr.branch_id = b.branch_id
                LEFT JOIN PurchAgg pa ON pa.branch_id = b.branch_id
                LEFT JOIN PurchReturnAgg pr ON pr.branch_id = b.branch_id
                WHERE (ISNULL(sa.Docs,0) + ISNULL(sr.Docs,0) + ISNULL(pa.Docs,0) + ISNULL(pr.Docs,0)) > 0
                ORDER BY b.branch_id;";

                cmd.Parameters.AddWithValue("@pFrom", from.Date);
                cmd.Parameters.AddWithValue("@pTo", to.Date);

                var dt = new DataTable();
                cn.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;
            }
        }

        public DataTable GetBranchSummary(DateTime from, DateTime to, int branchId)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = @"
                DECLARE @from DATETIME = @pFrom;
                DECLARE @to   DATETIME = @pTo;

                ;WITH SalesAgg AS (
                    SELECT Docs = COUNT(1), NetAmount = ISNULL(SUM(ABS(ISNULL(total_amount,0) - ISNULL(discount_value,0))),0), VatAmount = ISNULL(SUM(ABS(ISNULL(total_tax,0))),0)
                    FROM pos_sales
                    WHERE branch_id = @branch_id AND sale_date >= @from AND sale_date < DATEADD(day,1,@to) AND account <> 'Return'
                      AND ISNULL(invoice_no,'') NOT LIKE 'ZS%'
                ),
                SalesReturnAgg AS (
                    SELECT Docs = COUNT(1), NetAmount = ISNULL(SUM(ABS(ISNULL(total_amount,0) - ISNULL(discount_value,0))),0), VatAmount = ISNULL(SUM(ABS(ISNULL(total_tax,0))),0)
                    FROM pos_sales
                    WHERE branch_id = @branch_id AND sale_date >= @from AND sale_date < DATEADD(day,1,@to) AND account = 'Return'
                      AND ISNULL(invoice_no,'') NOT LIKE 'ZS%'
                ),
                PurchAgg AS (
                    SELECT Docs = COUNT(1), NetAmount = ISNULL(SUM(ABS(ISNULL(total_amount,0) - ISNULL(discount_value,0))),0), VatAmount = ISNULL(SUM(ABS(ISNULL(total_tax,0))),0)
                    FROM pos_purchases
                    WHERE branch_id = @branch_id AND purchase_date >= @from AND purchase_date < DATEADD(day,1,@to) AND account <> 'Return'
                      AND ISNULL(invoice_no,'') NOT LIKE 'ZS%'
                ),
                PurchReturnAgg AS (
                    SELECT Docs = COUNT(1), NetAmount = ISNULL(SUM(ABS(ISNULL(total_amount,0) - ISNULL(discount_value,0))),0), VatAmount = ISNULL(SUM(ABS(ISNULL(total_tax,0))),0)
                    FROM pos_purchases
                    WHERE branch_id = @branch_id AND purchase_date >= @from AND purchase_date < DATEADD(day,1,@to) AND account = 'Return'
                      AND ISNULL(invoice_no,'') NOT LIKE 'ZS%'
                )
                SELECT * FROM (
                    SELECT Terms = 'Sales', Docs = (SELECT Docs FROM SalesAgg), Flag = 'Sales', NetAmount = (SELECT NetAmount FROM SalesAgg), VatAmount = (SELECT VatAmount FROM SalesAgg), SortOrder = 1
                    UNION ALL
                    SELECT Terms = 'Sales Return', Docs = (SELECT Docs FROM SalesReturnAgg), Flag = 'Return', NetAmount = (SELECT NetAmount FROM SalesReturnAgg) * -1, VatAmount = (SELECT VatAmount FROM SalesReturnAgg) * -1, SortOrder = 2
                    UNION ALL
                    SELECT Terms = 'Purchases', Docs = (SELECT Docs FROM PurchAgg), Flag = 'Purchase', NetAmount = (SELECT NetAmount FROM PurchAgg) * -1, VatAmount = (SELECT VatAmount FROM PurchAgg) * -1, SortOrder = 3
                    UNION ALL
                    SELECT Terms = 'Purchase Return', Docs = (SELECT Docs FROM PurchReturnAgg), Flag = 'Return', NetAmount = (SELECT NetAmount FROM PurchReturnAgg), VatAmount = (SELECT VatAmount FROM PurchReturnAgg), SortOrder = 4
                ) x
                ORDER BY SortOrder;";

                cmd.Parameters.AddWithValue("@branch_id", branchId);
                cmd.Parameters.AddWithValue("@pFrom", from.Date);
                cmd.Parameters.AddWithValue("@pTo", to.Date);

                var dt = new DataTable();
                cn.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;
            }
        }

        public DataTable GetInvoiceDetails(DateTime from, DateTime to, string term)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = cn.CreateCommand())
            {
                string normalizedTerm = (term ?? string.Empty).Trim();
                if (normalizedTerm.Length == 0)
                    return new DataTable();

                string salesSelect = @"
                    SELECT
                        DocDate = s.sale_date,
                        InvoiceNo = s.invoice_no,
                        Party = LTRIM(RTRIM(ISNULL(c.first_name, '') + ' ' + ISNULL(c.last_name, ''))),
                        VATNo = ISNULL(c.vat_no, ''),
                        DocType = CASE WHEN s.account = 'Return' THEN 'Sales Return' ELSE 'Sales' END,
                        SaleType = s.sale_type,
                        NetAmount = {0}ABS(ISNULL(s.total_amount,0) - ISNULL(s.discount_value,0)),
                        VatAmount = {0}ABS(ISNULL(s.total_tax,0)),
                        TotalAmount = {0}(ABS(ISNULL(s.total_amount,0) - ISNULL(s.discount_value,0)) + ABS(ISNULL(s.total_tax,0))),
                        Description = ISNULL(s.description, '')
                    FROM pos_sales s
                    LEFT JOIN pos_customers c ON c.id = s.customer_id
                    WHERE s.sale_date >= @pFrom AND s.sale_date < DATEADD(day,1,@pTo)
                      AND ISNULL(s.invoice_no,'') NOT LIKE 'ZS%'
                      AND {1}";

                string purchasesSelect = @"
                    SELECT
                        DocDate = p.purchase_date,
                        InvoiceNo = p.invoice_no,
                        SupplierInvoice = p.supplier_invoice_no,
                        Party = LTRIM(RTRIM(ISNULL(s.first_name, '') + ' ' + ISNULL(s.last_name, ''))),
                        VATNo = ISNULL(s.vat_no, ''),
                        DocType = CASE WHEN p.account = 'Return' THEN 'Purchase Return' ELSE 'Purchases' END,
                        PurType = p.purchase_type,
                        NetAmount = {0}ABS(ISNULL(p.total_amount,0) - ISNULL(p.discount_value,0)),
                        VatAmount = {0}ABS(ISNULL(p.total_tax,0)),
                        TotalAmount = {0}(ABS(ISNULL(p.total_amount,0) - ISNULL(p.discount_value,0)) + ABS(ISNULL(p.total_tax,0))),
                        Description = ISNULL(p.description, '')
                    FROM pos_purchases p
                    LEFT JOIN pos_suppliers s ON s.id = p.supplier_id
                    WHERE p.purchase_date >= @pFrom AND p.purchase_date < DATEADD(day,1,@pTo)
                      AND ISNULL(p.invoice_no,'') NOT LIKE 'ZS%'
                      AND {1}";

                string sql;
                switch (normalizedTerm.ToLowerInvariant())
                {
                    case "sales":
                        sql = string.Format(salesSelect, string.Empty, "s.account <> 'Return'");
                        break;
                    case "sales return":
                        sql = string.Format(salesSelect, "-", "s.account = 'Return'");
                        break;
                    case "purchases":
                        sql = string.Format(purchasesSelect, "-", "p.account <> 'Return'");
                        break;
                    case "purchase return":
                        sql = string.Format(purchasesSelect, string.Empty, "p.account = 'Return'");
                        break;
                    case "total sales":
                        sql = string.Format(salesSelect, string.Empty, "s.account <> 'Return'")
                            + " UNION ALL "
                            + string.Format(salesSelect, "-", "s.account = 'Return'");
                        break;
                    case "total purchases":
                        sql = string.Format(purchasesSelect, "-", "p.account <> 'Return'")
                            + " UNION ALL "
                            + string.Format(purchasesSelect, string.Empty, "p.account = 'Return'");
                        break;
                    default:
                        return new DataTable();
                }

                cmd.CommandText = sql + " ORDER BY DocDate ASC, InvoiceNo ASC";
                cmd.Parameters.AddWithValue("@pFrom", from.Date);
                cmd.Parameters.AddWithValue("@pTo", to.Date);

                var dt = new DataTable();
                cn.Open();
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;
            }
        }
    }
}
