using POS.Core;
using System;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    public class SalesDashboardDLL
    {
        public DataTable GetSalesReps()
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
                SELECT 0 AS id, 'All Sales Reps' AS employee_name
                UNION ALL
                SELECT e.id, CONCAT(ISNULL(e.first_name, ''), ' ', ISNULL(e.last_name, '')) AS employee_name
                FROM pos_employees e
                WHERE e.branch_id = @branch_id
                ORDER BY id;", cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cn.Open();
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetKpis(DateTime fromDate, DateTime toDate, DateTime prevFromDate, DateTime prevToDate, int? employeeId)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            ;WITH CurrentSales AS (
                SELECT s.invoice_no,
                       ISNULL(s.sale_type, '') AS sale_type,
                       CAST(ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0) AS decimal(18,2)) AS net_amount,
                       s.sale_date
                FROM pos_sales s
                WHERE s.branch_id = @branch_id
                  AND s.sale_date BETWEEN @fromDate AND @toDate
                  AND ISNULL(s.account, '') <> 'Return'
                  AND (@employee_id IS NULL OR s.employee_id = @employee_id)
            ),
            PrevSales AS (
                SELECT CAST(ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0) AS decimal(18,2)) AS net_amount
                FROM pos_sales s
                WHERE s.branch_id = @branch_id
                  AND s.sale_date BETWEEN @prevFromDate AND @prevToDate
                  AND ISNULL(s.account, '') <> 'Return'
                  AND (@employee_id IS NULL OR s.employee_id = @employee_id)
            ),
            PaidByInvoiceTotal AS (
                SELECT cp.invoice_no, SUM(ISNULL(cp.credit, 0)) AS paid_amount
                FROM pos_customers_payments cp
                WHERE cp.branch_id = @branch_id
                  AND cp.entry_date <= @toDate
                GROUP BY cp.invoice_no
            ),
            PaidByInvoicePeriod AS (
                SELECT cp.invoice_no, SUM(ISNULL(cp.credit, 0)) AS paid_amount
                FROM pos_customers_payments cp
                WHERE cp.branch_id = @branch_id
                  AND cp.entry_date BETWEEN @fromDate AND @toDate
                GROUP BY cp.invoice_no
            )
            SELECT
                ISNULL((SELECT SUM(net_amount) FROM CurrentSales), 0) AS total_sales,
                ISNULL((SELECT SUM(net_amount) FROM PrevSales), 0) AS total_sales_prev,
                ISNULL((SELECT COUNT(1) FROM CurrentSales), 0) AS total_invoices,
                ISNULL((SELECT SUM(CASE
                                   WHEN LOWER(c.sale_type) = 'cash' THEN c.net_amount
                                   WHEN LOWER(c.sale_type) = 'credit' THEN ISNULL(pp.paid_amount, 0)
                                   ELSE 0
                                 END)
                        FROM CurrentSales c
                        LEFT JOIN PaidByInvoicePeriod pp ON pp.invoice_no = c.invoice_no), 0) AS paid_amount,
                ISNULL((SELECT SUM(CASE
                                   WHEN LOWER(c.sale_type) = 'credit'
                                        AND c.net_amount - ISNULL(pt.paid_amount, 0) > 0
                                   THEN c.net_amount - ISNULL(pt.paid_amount, 0)
                                   ELSE 0
                                 END)
                        FROM CurrentSales c
                        LEFT JOIN PaidByInvoiceTotal pt ON pt.invoice_no = c.invoice_no), 0) AS receivable_outstanding,
                ISNULL((SELECT SUM(CASE
                                   WHEN LOWER(c.sale_type) = 'credit'
                                        AND DATEDIFF(DAY, c.sale_date, GETDATE()) > 30
                                        AND c.net_amount - ISNULL(pt.paid_amount, 0) > 0
                                   THEN c.net_amount - ISNULL(pt.paid_amount, 0)
                                   ELSE 0
                                 END)
                        FROM CurrentSales c
                        LEFT JOIN PaidByInvoiceTotal pt ON pt.invoice_no = c.invoice_no), 0) AS receivable_over_30,
                CASE WHEN ISNULL((SELECT COUNT(1) FROM CurrentSales), 0) = 0 THEN 0
                     ELSE ISNULL((SELECT SUM(net_amount) FROM CurrentSales), 0) / NULLIF((SELECT COUNT(1) FROM CurrentSales), 0)
                END AS avg_invoice_value;", cn))
            {
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@prevFromDate", prevFromDate);
                cmd.Parameters.AddWithValue("@prevToDate", prevToDate);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@employee_id", (object)employeeId ?? DBNull.Value);
                cn.Open();
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetMonthlySales(int year, int? employeeId)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            ;WITH M AS
            (
                SELECT 1 AS month_no
                UNION ALL
                SELECT month_no + 1 FROM M WHERE month_no < 12
            )
            SELECT m.month_no,
                   LEFT(DATENAME(MONTH, DATEFROMPARTS(@year, m.month_no, 1)), 3) AS month_label,
                   ISNULL(SUM(ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0)), 0) AS amount
            FROM M m
            LEFT JOIN pos_sales s ON MONTH(s.sale_date) = m.month_no
                                 AND YEAR(s.sale_date) = @year
                                 AND s.branch_id = @branch_id
                                 AND ISNULL(s.account, '') <> 'Return'
                                 AND (@employee_id IS NULL OR s.employee_id = @employee_id)
            GROUP BY m.month_no
            ORDER BY m.month_no
            OPTION (MAXRECURSION 12);", cn))
            {
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@employee_id", (object)employeeId ?? DBNull.Value);
                cn.Open();
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetCategorySplit(DateTime fromDate, DateTime toDate, int top, int? employeeId)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            ;WITH CategoryTotals AS
            (
                SELECT ISNULL(NULLIF(c.name, ''), 'Uncategorized') AS category_name,
                       SUM(ISNULL(si.unit_price, 0) * ISNULL(si.quantity_sold, 0) - ISNULL(si.discount_value, 0)) AS total_amount
                FROM pos_sales_items si
                INNER JOIN pos_sales s ON s.id = si.sale_id AND s.branch_id = si.branch_id
                LEFT JOIN pos_products p ON p.item_number = si.item_number
                LEFT JOIN pos_categories c ON c.code = p.category_code
                WHERE s.sale_date BETWEEN @fromDate AND @toDate
                  AND s.branch_id = @branch_id
                  AND ISNULL(s.account, '') <> 'Return'
                  AND (@employee_id IS NULL OR s.employee_id = @employee_id)
                GROUP BY ISNULL(NULLIF(c.name, ''), 'Uncategorized')
            )
            SELECT TOP (@top)
                ROW_NUMBER() OVER (ORDER BY total_amount DESC) AS rn,
                category_name,
                total_amount,
                CASE WHEN t.grand_total = 0 THEN 0 ELSE (total_amount * 100.0 / t.grand_total) END AS share_percent
            FROM CategoryTotals
            CROSS JOIN (SELECT ISNULL(SUM(total_amount),0) AS grand_total FROM CategoryTotals) t
            ORDER BY total_amount DESC;", cn))
            {
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@top", top);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@employee_id", (object)employeeId ?? DBNull.Value);
                cn.Open();
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetTopCustomers(DateTime fromDate, DateTime toDate, int top, int? employeeId)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            ;WITH SalesByCustomer AS
            (
                SELECT s.customer_id,
                       CONCAT(ISNULL(c.first_name, 'Walk-in'), ' ', ISNULL(c.last_name, 'Customer')) AS customer_name,
                       SUM(ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0)) AS total_sales,
                       SUM(CASE WHEN LOWER(s.sale_type) = 'credit'
                                     AND (ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0)) - ISNULL(p.paid_amount,0) > 0
                                THEN (ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0)) - ISNULL(p.paid_amount,0)
                                ELSE 0 END) AS outstanding
                FROM pos_sales s
                LEFT JOIN pos_customers c ON c.id = s.customer_id
                LEFT JOIN (
                    SELECT invoice_no, SUM(ISNULL(credit, 0)) AS paid_amount
                    FROM pos_customers_payments
                    WHERE branch_id = @branch_id
                    GROUP BY invoice_no
                ) p ON p.invoice_no = s.invoice_no
                WHERE s.sale_date BETWEEN @fromDate AND @toDate
                  AND s.branch_id = @branch_id
                  AND ISNULL(s.account, '') <> 'Return'
                  AND (@employee_id IS NULL OR s.employee_id = @employee_id)
                GROUP BY s.customer_id, CONCAT(ISNULL(c.first_name, 'Walk-in'), ' ', ISNULL(c.last_name, 'Customer'))
            )
            SELECT TOP (@top)
                ROW_NUMBER() OVER (ORDER BY total_sales DESC) AS rank_no,
                customer_id,
                customer_name,
                total_sales,
                CASE WHEN t.grand_total = 0 THEN 0 ELSE (total_sales * 100.0 / t.grand_total) END AS share_percent,
                outstanding
            FROM SalesByCustomer
            CROSS JOIN (SELECT ISNULL(SUM(total_sales),0) AS grand_total FROM SalesByCustomer) t
            ORDER BY total_sales DESC;", cn))
            {
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@top", top);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@employee_id", (object)employeeId ?? DBNull.Value);
                cn.Open();
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetRecentInvoices(DateTime fromDate, DateTime toDate, int top, int? employeeId, int? monthNo, string categoryName, int? customerId)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            SELECT TOP (@top)
                s.invoice_no,
                CONCAT(ISNULL(c.first_name, 'Walk-in'), ' ', ISNULL(c.last_name, 'Customer')) AS customer_name,
                CAST(ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0) AS decimal(18,2)) AS amount,
                CASE
                    WHEN LOWER(s.sale_type) = 'cash' THEN 'Paid'
                    WHEN LOWER(s.sale_type) = 'credit' AND ISNULL(p.paid_amount, 0) >= CAST(ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0) AS decimal(18,2)) THEN 'Paid'
                    WHEN LOWER(s.sale_type) = 'credit' AND ISNULL(p.paid_amount, 0) > 0 THEN 'Partial'
                    WHEN LOWER(s.sale_type) = 'credit' THEN 'Unpaid'
                    ELSE 'Paid'
                END AS status,
                s.sale_date
            FROM pos_sales s
            LEFT JOIN pos_customers c ON c.id = s.customer_id
            LEFT JOIN (
                SELECT invoice_no, SUM(ISNULL(credit, 0)) AS paid_amount
                FROM pos_customers_payments
                WHERE branch_id = @branch_id
                GROUP BY invoice_no
            ) p ON p.invoice_no = s.invoice_no
            WHERE s.sale_date BETWEEN @fromDate AND @toDate
              AND s.branch_id = @branch_id
              AND ISNULL(s.account, '') <> 'Return'
              AND (@employee_id IS NULL OR s.employee_id = @employee_id)
              AND (@month_no IS NULL OR MONTH(s.sale_date) = @month_no)
              AND (@customer_id IS NULL OR s.customer_id = @customer_id)
              AND (
                   @category_name IS NULL
                   OR EXISTS (
                       SELECT 1
                       FROM pos_sales_items si
                       LEFT JOIN pos_products p2 ON p2.item_number = si.item_number
                       LEFT JOIN pos_categories c2 ON c2.code = p2.category_code
                       WHERE si.sale_id = s.id
                         AND si.branch_id = s.branch_id
                         AND ISNULL(NULLIF(c2.name, ''), 'Uncategorized') = @category_name
                   )
              )
            ORDER BY s.sale_date DESC, s.id DESC;", cn))
            {
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@top", top);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@employee_id", (object)employeeId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@month_no", (object)monthNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@category_name", (object)categoryName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@customer_id", (object)customerId ?? DBNull.Value);
                cn.Open();
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetCustomerSummary(int customerId, DateTime fromDate, DateTime toDate, int? employeeId)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            ;WITH S AS
            (
                SELECT s.invoice_no,
                       ISNULL(s.sale_type, '') AS sale_type,
                       CAST(ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0) AS decimal(18,2)) AS net_amount
                FROM pos_sales s
                WHERE s.branch_id = @branch_id
                  AND s.customer_id = @customer_id
                  AND s.sale_date BETWEEN @fromDate AND @toDate
                  AND ISNULL(s.account, '') <> 'Return'
                  AND (@employee_id IS NULL OR s.employee_id = @employee_id)
            )
            SELECT CONCAT(ISNULL(c.first_name, 'Walk-in'), ' ', ISNULL(c.last_name, 'Customer')) AS customer_name,
                   ISNULL((SELECT SUM(net_amount) FROM S), 0) AS total_sales,
                   ISNULL((SELECT SUM(CASE
                                       WHEN LOWER(s.sale_type) = 'credit' AND s.net_amount - ISNULL(p.paid_amount,0) > 0
                                       THEN s.net_amount - ISNULL(p.paid_amount,0)
                                       ELSE 0
                                     END)
                           FROM S s
                           LEFT JOIN (SELECT invoice_no, SUM(ISNULL(credit,0)) AS paid_amount
                                      FROM pos_customers_payments
                                      WHERE branch_id = @branch_id
                                      GROUP BY invoice_no) p ON p.invoice_no = s.invoice_no), 0) AS outstanding
            FROM pos_customers c
            WHERE c.id = @customer_id;", cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@customer_id", customerId);
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@employee_id", (object)employeeId ?? DBNull.Value);
                cn.Open();
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetReceivables(DateTime fromDate, DateTime toDate, bool overdueOnly, int? employeeId)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            SELECT s.invoice_no,
                   s.sale_date,
                   CONCAT(ISNULL(c.first_name, 'Walk-in'), ' ', ISNULL(c.last_name, 'Customer')) AS customer_name,
                   CAST(ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0) AS decimal(18,2)) AS invoice_amount,
                   ISNULL(p.paid_amount,0) AS paid_amount,
                   CAST((ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0)) - ISNULL(p.paid_amount,0) AS decimal(18,2)) AS pending_amount,
                   DATEDIFF(DAY, s.sale_date, GETDATE()) AS days_outstanding
            FROM pos_sales s
            LEFT JOIN pos_customers c ON c.id = s.customer_id
            LEFT JOIN (
                SELECT invoice_no, SUM(ISNULL(credit,0)) AS paid_amount
                FROM pos_customers_payments
                WHERE branch_id = @branch_id
                GROUP BY invoice_no
            ) p ON p.invoice_no = s.invoice_no
            WHERE s.sale_date BETWEEN @fromDate AND @toDate
              AND s.branch_id = @branch_id
              AND ISNULL(s.account, '') <> 'Return'
              AND LOWER(ISNULL(s.sale_type, '')) = 'credit'
              AND (@employee_id IS NULL OR s.employee_id = @employee_id)
              AND ((ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0)) - ISNULL(p.paid_amount,0)) > 0
              AND (@overdue_only = 0 OR DATEDIFF(DAY, s.sale_date, GETDATE()) > 30)
            ORDER BY pending_amount DESC;", cn))
            {
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@overdue_only", overdueOnly ? 1 : 0);
                cmd.Parameters.AddWithValue("@employee_id", (object)employeeId ?? DBNull.Value);
                cn.Open();
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetCustomerMonthlyTrend(int customerId, int year, int? employeeId)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            ;WITH M AS
            (
                SELECT 1 AS month_no
                UNION ALL
                SELECT month_no + 1 FROM M WHERE month_no < 12
            )
            SELECT m.month_no,
                   LEFT(DATENAME(MONTH, DATEFROMPARTS(@year, m.month_no, 1)), 3) AS month_label,
                   ISNULL(SUM(ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0)), 0) AS amount
            FROM M m
            LEFT JOIN pos_sales s ON MONTH(s.sale_date) = m.month_no
                                 AND YEAR(s.sale_date) = @year
                                 AND s.branch_id = @branch_id
                                 AND s.customer_id = @customer_id
                                 AND ISNULL(s.account, '') <> 'Return'
                                 AND (@employee_id IS NULL OR s.employee_id = @employee_id)
            GROUP BY m.month_no
            ORDER BY m.month_no
            OPTION (MAXRECURSION 12);", cn))
            {
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@customer_id", customerId);
                cmd.Parameters.AddWithValue("@employee_id", (object)employeeId ?? DBNull.Value);
                cn.Open();
                var dt = new DataTable();
                using (var da = new SqlDataAdapter(cmd)) da.Fill(dt);
                return dt;
            }
        }
    }
}
