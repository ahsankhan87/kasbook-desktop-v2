using System;
using System.Data;
using System.Data.SqlClient;
using POS.Core;

namespace POS.DLL
{
    public class CustomerDLL
    {
        private const string OpeningBalanceMarker = "[Opening Balance]";

        public string NormalizeCustomerCodeInput(string input)
        {
            var normalized = (input ?? string.Empty).Trim();

            if (normalized.Length == 0) return normalized;

            // Only normalize when the text is intended to be a customer code.
            // Examples:
            //  - "C00012"  => "C-00012"
            //  - "C-00012" => "C-00012"
            // Do NOT normalize names like "car" or "customer".
            bool isCodeNoDash =
                normalized.Length > 1 &&
                (normalized[0] == 'C' || normalized[0] == 'c') &&
                IsAllDigits(normalized.Substring(1));

            bool isCodeWithDash =
                normalized.Length > 2 &&
                (normalized[0] == 'C' || normalized[0] == 'c') &&
                normalized[1] == '-' &&
                IsAllDigits(normalized.Substring(2));

            if (isCodeNoDash)
            {
                normalized = "C" + normalized.Substring(1);
            }
            else if (isCodeWithDash)
            {
                normalized = "C" + normalized.Substring(2);
            }

            return normalized;
        }

        private static bool IsAllDigits(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsDigit(s[i])) return false;
            }
            return true;
        }

        public string GetNextCustomerCode()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT ISNULL(MAX(TRY_CAST(
                    CASE 
                        WHEN customer_code LIKE 'C-%' THEN SUBSTRING(customer_code, 3, 50)
                        WHEN customer_code LIKE 'C%' THEN SUBSTRING(customer_code, 2, 50)
                        ELSE customer_code
                    END AS int)), 0)
                FROM pos_customers
                WHERE branch_id = @branch_id", cn))
            {
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cn.Open();
                int max = Convert.ToInt32(cmd.ExecuteScalar());
                int nextNum = max + 1;
                return "C" + nextNum.ToString("D5");
            }
        }
        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_CustomersCrud", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OperationType", SqlDbType.VarChar).Value = "5";
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    // Log exception details here
                    throw new Exception("Error fetching all customers: " + ex.Message);
                }
                return dt;
            }
        }

        public DataTable SearchRecordByCustomerID(int customerId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_CustomersCrud", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OperationType", SqlDbType.VarChar).Value = "4";
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = customerId;
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    // Log exception details here
                    throw new Exception("Error searching record by customer ID: " + ex.Message);
                }
                return dt;
            }
        }
        public DataTable SearchRecord(string condition)
        {
            DataTable dt = new DataTable(); // Instantiate DataTable

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = @"SELECT * 
                                FROM pos_customers 
                                WHERE branch_id = @branch_id 
                                AND (first_name LIKE @condition OR last_name LIKE @condition 
                                OR customer_code LIKE @condition
                                OR vat_no LIKE @condition OR address LIKE @condition OR contact_no LIKE @condition)";

                    // Add parameters with precise type and value to prevent SQL injection
                    cmd.Parameters.Add("@condition", SqlDbType.NVarChar).Value = $"%{condition}%";
                    cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception as necessary
                        throw new Exception("Error: " + ex.Message);
                    }
                }
            }

            return dt;
        }

        public decimal GetCustomerAccountBalance(int customerId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"
            SELECT ISNULL(SUM(CAST(debit AS decimal(18,4)) - CAST(credit AS decimal(18,4))), 0) AS balance 
            FROM pos_customers_payments 
            WHERE customer_id = @id AND branch_id = @branch_id", cn))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = customerId;
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

                try
                {
                    cn.Open();
                    object scalar = cmd.ExecuteScalar();

                    decimal balance = 0m;
                    if (scalar != null && scalar != DBNull.Value)
                    {
                        balance = Convert.ToDecimal(scalar);
                    }

                    return Math.Round(balance, 2);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching customer account balance: " + ex.Message, ex);
                }
            }
        }

        public decimal GetCustomerOpeningBalance(int customerId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmdLocal = new SqlCommand(@"
            SELECT TOP 1 CAST(ISNULL(debit, 0) - ISNULL(credit, 0) AS decimal(18,2))
            FROM pos_customers_payments
            WHERE branch_id = @branch_id
              AND customer_id = @customer_id
              AND (
                    invoice_no = @opening_invoice_no
                    OR CHARINDEX(@opening_marker, ISNULL(description, '')) = 1
                  )
            ORDER BY id DESC", cn))
            {
                cmdLocal.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmdLocal.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;
                cmdLocal.Parameters.Add("@opening_invoice_no", SqlDbType.NVarChar, 50).Value = BuildOpeningBalanceInvoiceNo(customerId);
                cmdLocal.Parameters.Add("@opening_marker", SqlDbType.NVarChar).Value = OpeningBalanceMarker;

                cn.Open();
                object result = cmdLocal.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0m : Math.Round(Convert.ToDecimal(result), 2);
            }
        }

        private static int GetFallbackReceivableAccountId(SqlConnection cn)
        {
            using (SqlCommand cmdLocal = new SqlCommand("SELECT TOP 1 receivable_acc_id FROM pos_companies", cn))
            {
                object result = cmdLocal.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        private static string BuildOpeningBalanceInvoiceNo(int customerId)
        {
            return "CUS-OB-" + customerId;
        }

        private static string BuildOpeningBalanceDescription()
        {
            return OpeningBalanceMarker + " Customer opening balance";
        }

        private static void UpsertCustomerOpeningBalance(SqlConnection cn, int customerId, int accountId, decimal openingBalance)
        {
            const string findQuery = @"
            SELECT TOP 1 id
            FROM pos_customers_payments
            WHERE branch_id = @branch_id
              AND customer_id = @customer_id
              AND (
                    invoice_no = @opening_invoice_no
                    OR CHARINDEX(@opening_marker, ISNULL(description, '')) = 1
                  )
            ORDER BY id DESC";

            int existingId = 0;
            using (SqlCommand findCmd = new SqlCommand(findQuery, cn))
            {
                findCmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                findCmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;
                findCmd.Parameters.Add("@opening_invoice_no", SqlDbType.NVarChar, 50).Value = BuildOpeningBalanceInvoiceNo(customerId);
                findCmd.Parameters.Add("@opening_marker", SqlDbType.NVarChar).Value = OpeningBalanceMarker;

                object existing = findCmd.ExecuteScalar();
                existingId = existing == null || existing == DBNull.Value ? 0 : Convert.ToInt32(existing);
            }

            decimal roundedOpening = Math.Round(openingBalance, 2);
            if (roundedOpening <= 0)
            {
                if (existingId > 0)
                {
                    using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM pos_customers_payments WHERE id = @id AND branch_id = @branch_id", cn))
                    {
                        deleteCmd.Parameters.Add("@id", SqlDbType.Int).Value = existingId;
                        deleteCmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                        deleteCmd.ExecuteNonQuery();
                    }
                }

                return;
            }

            int effectiveAccountId = accountId > 0 ? accountId : GetFallbackReceivableAccountId(cn);
            string invoiceNo = BuildOpeningBalanceInvoiceNo(customerId);
            string description = BuildOpeningBalanceDescription();

            if (existingId > 0)
            {
                using (SqlCommand updateCmd = new SqlCommand(@"
                UPDATE pos_customers_payments
                SET invoice_no = @invoice_no,
                    debit = @debit,
                    credit = 0,
                    description = @description,
                    entry_date = @entry_date,
                    account_id = @account_id,
                    customer_id = @customer_id,
                    user_id = @user_id,
                    date_created = @date_created,
                    entry_id = 0,
                    payment_ref_invoice_no = ''
                WHERE id = @id
                  AND branch_id = @branch_id", cn))
                {
                    updateCmd.Parameters.Add("@id", SqlDbType.Int).Value = existingId;
                    updateCmd.Parameters.Add("@invoice_no", SqlDbType.NVarChar, 50).Value = invoiceNo;
                    updateCmd.Parameters.Add("@debit", SqlDbType.Decimal).Value = roundedOpening;
                    updateCmd.Parameters["@debit"].Precision = 18;
                    updateCmd.Parameters["@debit"].Scale = 2;
                    updateCmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = description;
                    updateCmd.Parameters.Add("@entry_date", SqlDbType.DateTime).Value = DateTime.Now;
                    updateCmd.Parameters.Add("@account_id", SqlDbType.Int).Value = effectiveAccountId;
                    updateCmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;
                    updateCmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UsersModal.logged_in_userid;
                    updateCmd.Parameters.Add("@date_created", SqlDbType.DateTime).Value = DateTime.Now;
                    updateCmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                    updateCmd.ExecuteNonQuery();
                }

                return;
            }

            using (SqlCommand insertCmd = new SqlCommand(@"
            INSERT INTO pos_customers_payments
            (
                invoice_no,
                debit,
                credit,
                description,
                entry_date,
                account_id,
                customer_id,
                user_id,
                branch_id,
                date_created,
                entry_id,
                payment_ref_invoice_no
            )
            VALUES
            (
                @invoice_no,
                @debit,
                0,
                @description,
                @entry_date,
                @account_id,
                @customer_id,
                @user_id,
                @branch_id,
                @date_created,
                0,
                ''
            )", cn))
            {
                insertCmd.Parameters.Add("@invoice_no", SqlDbType.NVarChar, 50).Value = invoiceNo;
                insertCmd.Parameters.Add("@debit", SqlDbType.Decimal).Value = roundedOpening;
                insertCmd.Parameters["@debit"].Precision = 18;
                insertCmd.Parameters["@debit"].Scale = 2;
                insertCmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = description;
                insertCmd.Parameters.Add("@entry_date", SqlDbType.DateTime).Value = DateTime.Now;
                insertCmd.Parameters.Add("@account_id", SqlDbType.Int).Value = effectiveAccountId;
                insertCmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;
                insertCmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UsersModal.logged_in_userid;
                insertCmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                insertCmd.Parameters.Add("@date_created", SqlDbType.DateTime).Value = DateTime.Now;
                insertCmd.ExecuteNonQuery();
            }
        }

        public DataTable GetPendingCustomerInvoices(int customerId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT
                    s.invoice_no,
                    s.sale_date,
                    CAST(ISNULL(s.total_amount, 0) + ISNULL(s.total_tax, 0) - ISNULL(s.discount_value, 0) AS decimal(18,4)) AS invoice_amount,
                    CAST(ISNULL(SUM(cp.credit), 0) AS decimal(18,4)) AS paid_amount,
                    CAST((ISNULL(s.total_amount, 0) + ISNULL(s.total_tax, 0) - ISNULL(s.discount_value, 0)) - ISNULL(SUM(cp.credit), 0) AS decimal(18,4)) AS balance_amount
                FROM pos_sales s
                LEFT JOIN pos_customers_payments cp
                    ON cp.branch_id = s.branch_id
                    AND cp.customer_id = s.customer_id
                    AND cp.invoice_no = s.invoice_no
                WHERE s.branch_id = @branch_id
                  AND s.customer_id = @customer_id
                  AND ISNULL(s.sale_type, '') = 'Credit'
                  AND ISNULL(s.account, '') <> 'Return'
                GROUP BY s.invoice_no, s.sale_date, s.total_amount, s.total_tax, s.discount_value
                HAVING ((ISNULL(s.total_amount, 0) + ISNULL(s.total_tax, 0) - ISNULL(s.discount_value, 0)) - ISNULL(SUM(cp.credit), 0)) > 0.004
                ORDER BY s.sale_date DESC, s.invoice_no DESC", cn))
            {
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;

                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    if (!dt.Columns.Contains("display_text"))
                        dt.Columns.Add("display_text", typeof(string));

                    foreach (DataRow row in dt.Rows)
                    {
                        DateTime saleDate;
                        DateTime.TryParse(Convert.ToString(row["sale_date"]), out saleDate);

                        decimal balanceAmount = 0m;
                        if (row["balance_amount"] != DBNull.Value)
                            balanceAmount = Convert.ToDecimal(row["balance_amount"]);

                        row["display_text"] = string.Format(
                            "{0} | {1:yyyy-MM-dd} | Balance: {2:N2}",
                            Convert.ToString(row["invoice_no"]),
                            saleDate == DateTime.MinValue ? DateTime.Today : saleDate,
                            balanceAmount);
                    }

                    return dt;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching pending customer invoices: " + ex.Message, ex);
                }
            }
        }


        public DataTable GetCustomerSummaryDashboard()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"
WITH Cust AS
(
    SELECT
        c.id,
        ISNULL(c.customer_code, '') AS customer_code,
        ISNULL(c.first_name, '') AS first_name,
        ISNULL(c.last_name, '') AS last_name,
        ISNULL(c.address, '') AS address,
        ISNULL(c.contact_no, '') AS contact_no,
        ISNULL(c.credit_limit, 0) AS credit_limit,
        ISNULL(c.status, 1) AS status,
        ISNULL(c.date_created, GETDATE()) AS date_created
    FROM pos_customers c
    WHERE c.branch_id = @branch_id
),
SalesAgg AS
(
    SELECT
        s.customer_id,
        SUM(CAST(ISNULL(s.total_amount, 0) + ISNULL(s.total_tax, 0) - ISNULL(s.discount_value, 0) AS decimal(18,4))) AS total_sales,
        MAX(s.sale_date) AS last_transaction_date
    FROM pos_sales s
    WHERE s.branch_id = @branch_id
      AND ISNULL(s.account, '') <> 'Return'
      AND ISNULL(s.sale_type, '') <> 'Quotation'
      AND ISNULL(s.sale_type, '') <> 'Gift'
    GROUP BY s.customer_id
),
OutstandingAgg AS
(
    SELECT
        p.customer_id,
        SUM(CAST(ISNULL(p.debit, 0) - ISNULL(p.credit, 0) AS decimal(18,4))) AS outstanding_balance,
        SUM(CASE WHEN DATEDIFF(DAY, ISNULL(p.entry_date, GETDATE()), GETDATE()) > 30
                 THEN CAST(ISNULL(p.debit, 0) - ISNULL(p.credit, 0) AS decimal(18,4))
                 ELSE 0 END) AS overdue_over_30,
        SUM(CASE WHEN DATEDIFF(DAY, ISNULL(p.entry_date, GETDATE()), GETDATE()) BETWEEN 15 AND 30
                 THEN CAST(ISNULL(p.debit, 0) - ISNULL(p.credit, 0) AS decimal(18,4))
                 ELSE 0 END) AS overdue_15_30
    FROM pos_customers_payments p
    WHERE p.branch_id = @branch_id
    GROUP BY p.customer_id
)
SELECT
    c.id,
    c.customer_code,
    c.first_name,
    c.last_name,
    c.address,
    c.contact_no,
    c.credit_limit,
    c.status,
    c.date_created,
    CAST(ISNULL(s.total_sales, 0) AS decimal(18,2)) AS total_sales,
    CAST(ISNULL(o.outstanding_balance, 0) AS decimal(18,2)) AS outstanding_balance,
    CAST(ISNULL(o.overdue_over_30, 0) AS decimal(18,2)) AS overdue_over_30,
    CAST(ISNULL(o.overdue_15_30, 0) AS decimal(18,2)) AS overdue_15_30,
    s.last_transaction_date,
    CAST(CASE
        WHEN ISNULL(c.credit_limit, 0) <= 0 THEN 0
        ELSE (ISNULL(o.outstanding_balance, 0) * 100.0) / NULLIF(c.credit_limit, 0)
    END AS decimal(18,2)) AS credit_used_percent,
    CAST(ISNULL(c.credit_limit, 0) - ISNULL(o.outstanding_balance, 0) AS decimal(18,2)) AS credit_available,
    CAST(ISNULL(s.total_sales, 0) - ISNULL(o.outstanding_balance, 0) AS decimal(18,2)) AS paid_amount,
    CASE
        WHEN ISNULL(c.status, 1) = 0 THEN 'Inactive'
        WHEN ISNULL(o.overdue_over_30, 0) > 0 THEN 'Overdue'
        ELSE 'Active'
    END AS status_text
FROM Cust c
LEFT JOIN SalesAgg s ON s.customer_id = c.id
LEFT JOIN OutstandingAgg o ON o.customer_id = c.id
ORDER BY c.first_name, c.last_name", cn))
        {
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            DataTable dt = new DataTable();
            cn.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
            return dt;
        }
    }

    public DataTable GetCustomerRecentTransactions(int customerId, int top = 5)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
SELECT TOP (@top)
    ISNULL(invoice_no, '') AS invoice_no,
    entry_date,
    CAST(ISNULL(debit, 0) AS decimal(18,2)) AS debit,
    CAST(ISNULL(credit, 0) AS decimal(18,2)) AS credit,
    CAST(ISNULL(debit, 0) - ISNULL(credit, 0) AS decimal(18,2)) AS balance,
    ISNULL(description, '') AS description
FROM pos_customers_payments
WHERE branch_id = @branch_id
  AND customer_id = @customer_id
ORDER BY ISNULL(entry_date, GETDATE()) DESC, id DESC", cn))
        {
            cmd.Parameters.Add("@top", SqlDbType.Int).Value = top <= 0 ? 5 : top;
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;

            DataTable dt = new DataTable();
            cn.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
            return dt;
        }
    }

    public DataTable GetCustomerProfileOverview(int customerId)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
WITH SalesAgg AS
(
    SELECT
        s.customer_id,
        SUM(CAST(ISNULL(s.total_amount, 0) + ISNULL(s.total_tax, 0) - ISNULL(s.discount_value, 0) AS decimal(18,4))) AS lifetime_sales
    FROM pos_sales s
    WHERE s.branch_id = @branch_id
      AND s.customer_id = @customer_id
      AND ISNULL(s.account, '') <> 'Return'
      AND ISNULL(s.sale_type, '') <> 'Quotation'
    GROUP BY s.customer_id
),
PayAgg AS
(
    SELECT
        p.customer_id,
        SUM(CAST(ISNULL(p.credit, 0) AS decimal(18,4))) AS total_paid,
        SUM(CAST(ISNULL(p.debit, 0) - ISNULL(p.credit, 0) AS decimal(18,4))) AS outstanding_balance
    FROM pos_customers_payments p
    WHERE p.branch_id = @branch_id
      AND p.customer_id = @customer_id
    GROUP BY p.customer_id
)
SELECT
    c.id,
    ISNULL(c.customer_code, '') AS customer_code,
    ISNULL(c.first_name, '') AS first_name,
    ISNULL(c.last_name, '') AS last_name,
    ISNULL(c.email, '') AS email,
    ISNULL(c.contact_no, '') AS contact_no,
    ISNULL(c.address, '') AS address,
    ISNULL(c.credit_limit, 0) AS credit_limit,
    ISNULL(c.status, 1) AS status,
    CAST(ISNULL(s.lifetime_sales, 0) AS decimal(18,2)) AS lifetime_sales,
    CAST(ISNULL(p.total_paid, 0) AS decimal(18,2)) AS total_paid,
    CAST(ISNULL(p.outstanding_balance, 0) AS decimal(18,2)) AS current_outstanding,
    CAST(ISNULL(c.credit_limit, 0) - ISNULL(p.outstanding_balance, 0) AS decimal(18,2)) AS available_credit
FROM pos_customers c
LEFT JOIN SalesAgg s ON s.customer_id = c.id
LEFT JOIN PayAgg p ON p.customer_id = c.id
WHERE c.branch_id = @branch_id
  AND c.id = @customer_id", cn))
        {
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;

            DataTable dt = new DataTable();
            cn.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
            return dt;
        }
    }

    public DataTable GetCustomerMonthlyPurchaseHistory(int customerId, int months = 12)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
;WITH M AS
(
    SELECT 0 AS n, DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0) AS MonthStart
    UNION ALL
    SELECT n + 1, DATEADD(MONTH, -1, MonthStart)
    FROM M
    WHERE n + 1 < @months
),
S AS
(
    SELECT
        DATEADD(MONTH, DATEDIFF(MONTH, 0, s.sale_date), 0) AS MonthStart,
        SUM(CAST(ISNULL(s.total_amount, 0) + ISNULL(s.total_tax, 0) - ISNULL(s.discount_value, 0) AS decimal(18,4))) AS total_amount
    FROM pos_sales s
    WHERE s.branch_id = @branch_id
      AND s.customer_id = @customer_id
      AND ISNULL(s.account, '') <> 'Return'
      AND ISNULL(s.sale_type, '') <> 'Quotation'
    GROUP BY DATEADD(MONTH, DATEDIFF(MONTH, 0, s.sale_date), 0)
)
SELECT
    DATENAME(MONTH, m.MonthStart) + ' ' + CONVERT(varchar(4), YEAR(m.MonthStart)) AS month_label,
    CAST(ISNULL(s.total_amount, 0) AS decimal(18,2)) AS amount,
    m.MonthStart
FROM M m
LEFT JOIN S s ON s.MonthStart = m.MonthStart
ORDER BY m.MonthStart
OPTION (MAXRECURSION 100)", cn))
        {
            cmd.Parameters.Add("@months", SqlDbType.Int).Value = months <= 0 ? 12 : months;
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;

            DataTable dt = new DataTable();
            cn.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
            return dt;
        }
    }

    public DataTable GetCustomerLedger(int customerId, DateTime fromDate, DateTime toDate)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
SELECT
    p.id,
    p.entry_date,
    CASE
        WHEN ISNULL(p.debit, 0) > 0 THEN 'Invoice'
        WHEN ISNULL(p.credit, 0) > 0 THEN 'Payment'
        ELSE 'Adjustment'
    END AS trans_type,
    ISNULL(p.invoice_no, '') AS reference_no,
    CAST(ISNULL(p.debit, 0) AS decimal(18,2)) AS debit,
    CAST(ISNULL(p.credit, 0) AS decimal(18,2)) AS credit,
    CAST(0 AS decimal(18,2)) AS running_balance,
    ISNULL(p.description, '') AS description
FROM pos_customers_payments p
WHERE p.branch_id = @branch_id
  AND p.customer_id = @customer_id
  AND CAST(p.entry_date AS date) BETWEEN @from_date AND @to_date
ORDER BY p.entry_date, p.id", cn))
        {
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;
            cmd.Parameters.Add("@from_date", SqlDbType.Date).Value = fromDate.Date;
            cmd.Parameters.Add("@to_date", SqlDbType.Date).Value = toDate.Date;

            DataTable dt = new DataTable();
            cn.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }

            decimal running = 0m;
            foreach (DataRow row in dt.Rows)
            {
                running += (row["debit"] == DBNull.Value ? 0m : Convert.ToDecimal(row["debit"]))
                           - (row["credit"] == DBNull.Value ? 0m : Convert.ToDecimal(row["credit"]));
                row["running_balance"] = Math.Round(running, 2);
            }

            return dt;
        }
    }

    public DataTable GetCustomerOutstandingInvoices(int customerId)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
SELECT
    s.invoice_no,
    s.sale_date,
    DATEADD(DAY,
        CASE
            WHEN ISNUMERIC(s.payment_terms_id) = 1 THEN CAST(s.payment_terms_id AS int)
            ELSE 30
        END,
        s.sale_date) AS due_date,
    CAST(ISNULL(s.total_amount, 0) + ISNULL(s.total_tax, 0) - ISNULL(s.discount_value, 0) AS decimal(18,2)) AS amount,
    CAST(ISNULL(SUM(cp.credit), 0) AS decimal(18,2)) AS paid,
    CAST((ISNULL(s.total_amount, 0) + ISNULL(s.total_tax, 0) - ISNULL(s.discount_value, 0)) - ISNULL(SUM(cp.credit), 0) AS decimal(18,2)) AS balance,
    CASE
        WHEN DATEDIFF(DAY, DATEADD(DAY,
            CASE WHEN ISNUMERIC(s.payment_terms_id) = 1 THEN CAST(s.payment_terms_id AS int) ELSE 30 END,
            s.sale_date), GETDATE()) < 0 THEN 0
        ELSE DATEDIFF(DAY, DATEADD(DAY,
            CASE WHEN ISNUMERIC(s.payment_terms_id) = 1 THEN CAST(s.payment_terms_id AS int) ELSE 30 END,
            s.sale_date), GETDATE())
    END AS days_overdue
FROM pos_sales s
LEFT JOIN pos_customers_payments cp
    ON cp.branch_id = s.branch_id
   AND cp.customer_id = s.customer_id
   AND cp.invoice_no = s.invoice_no
WHERE s.branch_id = @branch_id
  AND s.customer_id = @customer_id
  AND ISNULL(s.sale_type, '') = 'Credit'
  AND ISNULL(s.account, '') <> 'Return'
GROUP BY s.invoice_no, s.sale_date, s.payment_terms_id, s.total_amount, s.total_tax, s.discount_value
HAVING ((ISNULL(s.total_amount, 0) + ISNULL(s.total_tax, 0) - ISNULL(s.discount_value, 0)) - ISNULL(SUM(cp.credit), 0)) > 0.004
ORDER BY s.sale_date DESC, s.invoice_no DESC", cn))
        {
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;

            DataTable dt = new DataTable();
            cn.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
            return dt;
        }
    }

    public DataTable GetCustomerAgingSummary(int customerId)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
WITH O AS
(
    SELECT
        DATEDIFF(DAY,
            DATEADD(DAY,
                CASE WHEN ISNUMERIC(s.payment_terms_id) = 1 THEN CAST(s.payment_terms_id AS int) ELSE 30 END,
                s.sale_date),
            GETDATE()) AS due_days,
        ((ISNULL(s.total_amount, 0) + ISNULL(s.total_tax, 0) - ISNULL(s.discount_value, 0)) - ISNULL(SUM(cp.credit), 0)) AS balance
    FROM pos_sales s
    LEFT JOIN pos_customers_payments cp
        ON cp.branch_id = s.branch_id
       AND cp.customer_id = s.customer_id
       AND cp.invoice_no = s.invoice_no
    WHERE s.branch_id = @branch_id
      AND s.customer_id = @customer_id
      AND ISNULL(s.sale_type, '') = 'Credit'
      AND ISNULL(s.account, '') <> 'Return'
    GROUP BY s.invoice_no, s.sale_date, s.payment_terms_id, s.total_amount, s.total_tax, s.discount_value
    HAVING ((ISNULL(s.total_amount, 0) + ISNULL(s.total_tax, 0) - ISNULL(s.discount_value, 0)) - ISNULL(SUM(cp.credit), 0)) > 0.004
)
SELECT
    CAST(SUM(CASE WHEN due_days <= 30 THEN balance ELSE 0 END) AS decimal(18,2)) AS bucket_0_30,
    CAST(SUM(CASE WHEN due_days BETWEEN 31 AND 60 THEN balance ELSE 0 END) AS decimal(18,2)) AS bucket_31_60,
    CAST(SUM(CASE WHEN due_days BETWEEN 61 AND 90 THEN balance ELSE 0 END) AS decimal(18,2)) AS bucket_61_90,
    CAST(SUM(CASE WHEN due_days > 90 THEN balance ELSE 0 END) AS decimal(18,2)) AS bucket_90_plus
FROM O", cn))
        {
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;

            DataTable dt = new DataTable();
            cn.Open();
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
            return dt;
        }
    }

    public DataTable GetCustomerNotes(int customerId)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        {
            EnsureCustomerNotesTable(cn);

            using (SqlCommand cmd = new SqlCommand(@"
            SELECT
                n.id,
                n.note_date,
                n.note_text,
                ISNULL(u.name, ISNULL(CONVERT(varchar(20), n.user_id), '')) AS added_by
            FROM pos_customer_notes n
            LEFT JOIN pos_users u ON u.id = n.user_id
            WHERE n.branch_id = @branch_id
              AND n.customer_id = @customer_id
            ORDER BY n.note_date DESC, n.id DESC", cn))
            {
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;
            }
        }
    }

    public int AddCustomerNote(int customerId, string noteText)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        {
            EnsureCustomerNotesTable(cn);

            using (SqlCommand cmd = new SqlCommand(@"
            INSERT INTO pos_customer_notes(branch_id, customer_id, note_date, note_text, user_id)
            VALUES(@branch_id, @customer_id, GETDATE(), @note_text, @user_id)", cn))
            {
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;
                cmd.Parameters.Add("@note_text", SqlDbType.NVarChar).Value = (object)(noteText ?? string.Empty) ?? DBNull.Value;
                cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UsersModal.logged_in_userid;

                return cmd.ExecuteNonQuery();
            }
        }
    }

    public int UpdateCustomerStatus(int customerId, bool isActive)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(@"
UPDATE pos_customers
SET status = @status
WHERE branch_id = @branch_id
  AND id = @customer_id", cn))
        {
            cmd.Parameters.Add("@status", SqlDbType.Int).Value = isActive ? 1 : 0;
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customerId;

            cn.Open();
            return cmd.ExecuteNonQuery();
        }
    }

    private static void EnsureCustomerNotesTable(SqlConnection cn)
    {
        if (cn.State != ConnectionState.Open)
            cn.Open();

        const string sql = @"
IF OBJECT_ID('dbo.pos_customer_notes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.pos_customer_notes
    (
        id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        branch_id INT NOT NULL,
        customer_id INT NOT NULL,
        note_date DATETIME NOT NULL,
        note_text NVARCHAR(1000) NULL,
        user_id INT NULL
    );
END";

        using (SqlCommand cmd = new SqlCommand(sql, cn))
        {
            cmd.ExecuteNonQuery();
        }
    }

    public int Insert(CustomerModal obj)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        using (SqlCommand cmd = new SqlCommand("sp_CustomersCrud", cn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            //SetCommonParameters(cmd, obj);
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

            cmd.Parameters.Add("first_name", SqlDbType.NVarChar).Value = obj.first_name;
            cmd.Parameters.Add("last_name", SqlDbType.NVarChar).Value = obj.last_name;
            cmd.Parameters.Add("customer_code", SqlDbType.NVarChar).Value = obj.customer_code;
            cmd.Parameters.Add("email", SqlDbType.NVarChar).Value = obj.email;
            cmd.Parameters.Add("address", SqlDbType.NVarChar).Value = obj.address;
            cmd.Parameters.Add("status", SqlDbType.Int).Value = 1; // Assuming status is always active
            cmd.Parameters.Add("contact_no", SqlDbType.NVarChar).Value = obj.contact_no;
            cmd.Parameters.Add("vat_no", SqlDbType.NVarChar).Value = obj.vat_no;
            cmd.Parameters.Add("credit_limit", SqlDbType.Decimal).Value = obj.credit_limit;
            cmd.Parameters.Add("vin_no", SqlDbType.NVarChar).Value = obj.vin_no;
            cmd.Parameters.Add("car_name", SqlDbType.NVarChar).Value = obj.car_name;

            cmd.Parameters.Add("@OperationType", SqlDbType.VarChar).Value = "1";
            cmd.Parameters.Add("@date_created", SqlDbType.DateTime).Value = obj.date_created;
            cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UsersModal.logged_in_userid;

            cmd.Parameters.Add("@StreetName", SqlDbType.NVarChar).Value = obj.StreetName;
            cmd.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = obj.PostalCode;
            cmd.Parameters.Add("@BuildingNumber", SqlDbType.NVarChar).Value = obj.BuildingNumber;
            cmd.Parameters.Add("@CitySubdivisionName", SqlDbType.NVarChar).Value = obj.CitySubdivisionName;
            cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = obj.CityName;
            cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = obj.CountryName;
            cmd.Parameters.Add("@RegistrationName", SqlDbType.NVarChar).Value = obj.registrationName;
            cmd.Parameters.Add("@GLAccountID", SqlDbType.Int).Value = obj.GLAccountID;
            cmd.Parameters.Add("@CRNumber", SqlDbType.NVarChar).Value = obj.CRNumber;

            try
            {
                cn.Open();
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                if (result > 0)
                {
                    UpsertCustomerOpeningBalance(cn, result, obj.GLAccountID, obj.opening_balance);
                }
                LogAction("Add Customer", result, obj);
                return result;
            }
            catch (Exception ex)
            {
                // Log exception details here
                throw new Exception("Error inserting customer: " + ex.Message);
            }
        }
    }

    public int Update(CustomerModal obj)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        using (SqlCommand cmd = new SqlCommand("sp_CustomersCrud", cn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = obj.id;
            cmd.Parameters.Add("first_name", SqlDbType.NVarChar).Value = obj.first_name;
            cmd.Parameters.Add("last_name", SqlDbType.NVarChar).Value = obj.last_name;
            string code = NormalizeCustomerCodeInput(obj.customer_code);
            if (string.IsNullOrWhiteSpace(code) || !(code.StartsWith("C") || code.StartsWith("c")) || code.Length < 2)
            {
                using (SqlCommand getCmd = new SqlCommand(
                    "SELECT customer_code FROM pos_customers WHERE id=@id AND branch_id=@branch_id", cn))
                {
                    getCmd.Parameters.Add("@id", SqlDbType.Int).Value = obj.id;
                    getCmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                    cn.Open();
                    var existing = getCmd.ExecuteScalar();
                    cn.Close();
                    code = existing == null || existing == DBNull.Value ? string.Empty : Convert.ToString(existing);
                }
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                code = GetNextCustomerCode();
            }

            // Prevent duplicates in the same branch (excluding the current customer)
            using (SqlCommand dupCmd = new SqlCommand(
                "SELECT COUNT(1) FROM pos_customers WHERE branch_id=@branch_id AND customer_code=@code AND id<>@id", cn))
            {
                dupCmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                dupCmd.Parameters.Add("@code", SqlDbType.NVarChar, 50).Value = code;
                dupCmd.Parameters.Add("@id", SqlDbType.Int).Value = obj.id;
                cn.Open();
                int exists = Convert.ToInt32(dupCmd.ExecuteScalar());
                cn.Close();
                if (exists > 0)
                    throw new Exception("Customer code already exists: " + code);
            }

            cmd.Parameters.Add("customer_code", SqlDbType.NVarChar).Value = code;
            cmd.Parameters.Add("email", SqlDbType.NVarChar).Value = obj.email;
            cmd.Parameters.Add("address", SqlDbType.NVarChar).Value = obj.address;
            cmd.Parameters.Add("status", SqlDbType.Int).Value = 1; // Assuming status is always active
            cmd.Parameters.Add("contact_no", SqlDbType.NVarChar).Value = obj.contact_no;
            cmd.Parameters.Add("vat_no", SqlDbType.NVarChar).Value = obj.vat_no;
            cmd.Parameters.Add("credit_limit", SqlDbType.Decimal).Value = obj.credit_limit;
            cmd.Parameters.Add("vin_no", SqlDbType.NVarChar).Value = obj.vin_no;
            cmd.Parameters.Add("car_name", SqlDbType.NVarChar).Value = obj.car_name;

            cmd.Parameters.Add("@OperationType", SqlDbType.VarChar).Value = "2";
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            cmd.Parameters.Add("@date_updated", SqlDbType.DateTime).Value = obj.date_updated;
            cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UsersModal.logged_in_userid;

            cmd.Parameters.Add("@StreetName", SqlDbType.NVarChar).Value = obj.StreetName;
            cmd.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = obj.PostalCode;
            cmd.Parameters.Add("@BuildingNumber", SqlDbType.NVarChar).Value = obj.BuildingNumber;
            cmd.Parameters.Add("@CitySubdivisionName", SqlDbType.NVarChar).Value = obj.CitySubdivisionName;
            cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = obj.CityName;
            cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = obj.CountryName;
            cmd.Parameters.Add("@RegistrationName", SqlDbType.NVarChar).Value = obj.registrationName;
            cmd.Parameters.Add("@GLAccountID", SqlDbType.Int).Value = obj.GLAccountID;
            cmd.Parameters.Add("@CRNumber", SqlDbType.NVarChar).Value = obj.CRNumber;


            //SetCommonParameters(cmd, obj);


            try
            {
                cn.Open();
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                if (obj.id > 0)
                {
                    UpsertCustomerOpeningBalance(cn, obj.id, obj.GLAccountID, obj.opening_balance);
                }
                LogAction("Update Customer", obj.id, obj);
                return result;
            }
            catch (Exception ex)
            {
                // Log exception details here
                throw new Exception("Error updating customer: " + ex.Message);
            }
        }
    }

    public int Delete(int customerId)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        using (SqlCommand cmd = new SqlCommand("sp_CustomersCrud", cn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = customerId;
            cmd.Parameters.Add("@OperationType", SqlDbType.VarChar).Value = "3";
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

            try
            {
                cn.Open();
                int result = cmd.ExecuteNonQuery();
                Log.LogAction("Delete Customer", $"Customer ID: {customerId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return result;
            }
            catch (Exception ex)
            {
                // Log exception details here
                throw new Exception("Error deleting customer: " + ex.Message);
            }
        }
    }

    public int DeletePaymentTransaction(string invoiceNo)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        {
            SqlTransaction transaction = null;
            try
            {
                int result = 0;

                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                }

                EnsurePaymentReferenceColumn(cn);

                transaction = cn.BeginTransaction();

                result += DeleteCustomerPaymentTransactionRecords(cn, transaction, invoiceNo);
                result += DeletePaymentTransactionRecords(cn, transaction, "acc_entries", invoiceNo);
                result += DeleteOptionalPaymentTransactionRecords(cn, transaction, invoiceNo, "pos_banks_payments");

                transaction.Commit();

                return result;
            }
            catch
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                throw;
            }
        }
    }

    private static int DeleteCustomerPaymentTransactionRecords(SqlConnection cn, SqlTransaction transaction, string paymentReferenceInvoiceNo)
    {
        const string query = @"DELETE FROM pos_customers_payments
            WHERE branch_id = @branch_id
              AND (
                    payment_ref_invoice_no = @invoice_no
                    OR invoice_no = @invoice_no
                    OR (ISNULL(description, '') <> '' AND CHARINDEX(@payment_ref_token, description) > 0)
                    OR entry_id IN (
                        SELECT id
                        FROM acc_entries
                        WHERE branch_id = @branch_id
                          AND invoice_no = @invoice_no
                    )
                  )";

        using (SqlCommand deleteCommand = new SqlCommand(query, cn, transaction))
        {
            deleteCommand.Parameters.Add("@invoice_no", SqlDbType.NVarChar).Value = paymentReferenceInvoiceNo;
            deleteCommand.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            deleteCommand.Parameters.Add("@payment_ref_token", SqlDbType.NVarChar).Value = "[Payment Ref: " + paymentReferenceInvoiceNo + "]";
            return deleteCommand.ExecuteNonQuery();
        }
    }

    private static int DeletePaymentTransactionRecords(SqlConnection cn, SqlTransaction transaction, string tableName, string invoiceNo)
    {
        using (SqlCommand deleteCommand = new SqlCommand($"DELETE FROM {tableName} WHERE invoice_no = @invoice_no AND branch_id = @branch_id", cn, transaction))
        {
            deleteCommand.Parameters.Add("@invoice_no", SqlDbType.NVarChar).Value = invoiceNo;
            deleteCommand.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            return deleteCommand.ExecuteNonQuery();
        }
    }

    private static int DeleteOptionalPaymentTransactionRecords(SqlConnection cn, SqlTransaction transaction, string invoiceNo, string tableName)
    {
        using (SqlCommand countCommand = new SqlCommand($"SELECT COUNT(1) FROM {tableName} WHERE invoice_no = @invoice_no AND branch_id = @branch_id", cn, transaction))
        {
            countCommand.Parameters.Add("@invoice_no", SqlDbType.NVarChar).Value = invoiceNo;
            countCommand.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

            if (Convert.ToInt32(countCommand.ExecuteScalar()) <= 0)
                return 0;
        }

        return DeletePaymentTransactionRecords(cn, transaction, tableName, invoiceNo);
    }

    private void SetCommonParameters(SqlCommand cmd, CustomerModal obj)
    {
        cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
        cmd.Parameters.Add("@first_name", SqlDbType.NVarChar).Value = obj.first_name;
        cmd.Parameters.Add("@last_name", SqlDbType.NVarChar).Value = obj.last_name;
        cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = obj.email;
        cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = obj.address;
        cmd.Parameters.Add("@status", SqlDbType.Int).Value = 1;
        cmd.Parameters.Add("@contact_no", SqlDbType.NVarChar).Value = obj.contact_no;
        cmd.Parameters.Add("@vat_no", SqlDbType.NVarChar).Value = obj.vat_no;
        cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UsersModal.logged_in_userid;
        cmd.Parameters.Add("@credit_limit", SqlDbType.Decimal).Value = obj.credit_limit;
        cmd.Parameters.Add("@vin_no", SqlDbType.NVarChar).Value = obj.vin_no;
        cmd.Parameters.Add("@car_name", SqlDbType.NVarChar).Value = obj.car_name;
        cmd.Parameters.Add("@date_created", SqlDbType.DateTime).Value = DateTime.Now;
    }

    private void LogAction(string action, int customerId, CustomerModal obj)
    {
        Log.LogAction(action, $"Customer ID: {customerId}, Customer Name: {obj.first_name} {obj.last_name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
    }

    public bool IsCustomerCodeExists(string customerCode, int? excludeCustomerId = null)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(
            "SELECT COUNT(1) FROM pos_customers WHERE branch_id=@branch_id AND customer_code=@code" +
            (excludeCustomerId.HasValue ? " AND id<>@id" : ""), cn))
        {
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            cmd.Parameters.Add("@code", SqlDbType.NVarChar, 50).Value = customerCode;
            if (excludeCustomerId.HasValue)
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = excludeCustomerId.Value;
            }
            cn.Open();
            int exists = Convert.ToInt32(cmd.ExecuteScalar());
            return exists > 0;
        }
    }

    public bool IsCustomerDuplicate(string firstName, string vatNo, string registrationName, int? excludeCustomerId = null)
    {
        using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        using (SqlCommand cmd = new SqlCommand(
            "SELECT COUNT(1) FROM pos_customers " +
            "WHERE branch_id=@branch_id " +
            "AND ISNULL(LTRIM(RTRIM(first_name)),'') = @first_name " +
            "AND ISNULL(LTRIM(RTRIM(vat_no)),'') = @vat_no " +
            "AND ISNULL(LTRIM(RTRIM(RegistrationName)),'') = @registration_name" +
            (excludeCustomerId.HasValue ? " AND id<>@id" : string.Empty), cn))
        {
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            cmd.Parameters.Add("@first_name", SqlDbType.NVarChar, 200).Value = (firstName ?? string.Empty).Trim();
            cmd.Parameters.Add("@vat_no", SqlDbType.NVarChar, 100).Value = (vatNo ?? string.Empty).Trim();
            cmd.Parameters.Add("@registration_name", SqlDbType.NVarChar, 250).Value = (registrationName ?? string.Empty).Trim();

            if (excludeCustomerId.HasValue)
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = excludeCustomerId.Value;

            cn.Open();
            int exists = Convert.ToInt32(cmd.ExecuteScalar());
            return exists > 0;
        }
    }

    private static void EnsurePaymentReferenceColumn(SqlConnection cn)
    {
        const string sql = @"
        IF COL_LENGTH('pos_customers_payments', 'payment_ref_invoice_no') IS NULL
        BEGIN
            ALTER TABLE pos_customers_payments ADD payment_ref_invoice_no NVARCHAR(50) NULL;
        END";

        using (SqlCommand cmd = new SqlCommand(sql, cn))
        {
            cmd.ExecuteNonQuery();
        }
    }
}
}
