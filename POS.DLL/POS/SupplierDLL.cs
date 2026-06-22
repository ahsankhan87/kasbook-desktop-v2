using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using POS.Core;

namespace POS.DLL
{
    public class SupplierDLL
    {
        private const string OpeningBalanceMarker = "[Opening Balance]";

        private SqlCommand cmd;
        private SqlDataAdapter da;

        public string NormalizeSupplierCodeInput(string input)
        {
            var normalized = (input ?? string.Empty).Trim();

            if (normalized.Length == 0) return normalized;

            // Only normalize when the text is intended to be a supplier code.
            // Examples:
            //  - "S00001"  => "S-00001"
            //  - "S-00001" => "S-00001"
            // Do NOT normalize names like "Sam" or "saleem".
            bool isCodeNoDash =
                normalized.Length > 1 &&
                (normalized[0] == 'S' || normalized[0] == 's') &&
                normalized.Substring(1).All(char.IsDigit) &&
                normalized.Substring(1).Length >= 5;

            bool isCodeWithDash =
                normalized.Length > 2 &&
                (normalized[0] == 'S' || normalized[0] == 's') &&
                normalized[1] != '-' &&
                normalized.Substring(2).All(char.IsDigit) &&
                normalized.Substring(2).Length >= 5;

            if (isCodeNoDash)
            {
                normalized = "S" + NormalizeNumericPart(normalized.Substring(1));
            }
            else if (isCodeWithDash)
            {
                normalized = "S" + NormalizeNumericPart(normalized.Substring(2));
            }

            return normalized;
        }

        private static string NormalizeNumericPart(string digits)
        {
            if (string.IsNullOrWhiteSpace(digits)) return digits;

            var trimmed = digits.TrimStart('0');
            if (trimmed.Length == 0) trimmed = "0";

            int n;
            if (!int.TryParse(trimmed, out n)) return digits;
            return n.ToString("D5");
        }

        public string GetNextSupplierCode()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    string nextCode = "S00001"; // Default code if no suppliers exist
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("SELECT MAX(supplier_code) FROM pos_suppliers WHERE branch_id = @branch_id", cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        var result = cmd.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            string maxCode = result.ToString();
                            // Support both legacy (S-00001) and new (S00001)
                            string numeric = maxCode.StartsWith("S-") ? maxCode.Substring(2) : (maxCode.StartsWith("S") ? maxCode.Substring(1) : maxCode);
                            if (int.TryParse(numeric, out int numericPart))
                                nextCode = $"S{(numericPart + 1).ToString("D5")}";
                        }
                    }
                    return nextCode;
                }
                catch
                {
                    throw;
                }
            }
        }
        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_SuppliersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }

        }

        public DataTable SearchRecordBySupplierID(int Supplier_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("sp_SuppliersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd = new SqlCommand("SELECT id,first_name,last_name,email,vat_no FROM pos_suppliers WHERE id = @id", cn);
                        cmd.Parameters.AddWithValue("@OperationType", "4");
                        cmd.Parameters.AddWithValue("@id", Supplier_id);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        return dt;

                    }

                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable GetSupplierAccountBalance(int Supplier_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand();
                        string query = "SELECT SUM(debit-credit) AS balance FROM pos_suppliers_payments WHERE branch_id=@branch_id AND supplier_id = @id";
                        cmd.Parameters.AddWithValue("@id", Supplier_id);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        cmd.CommandText = query;
                        cmd.Connection = cn;
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        return dt;

                    }

                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        public decimal GetSupplierOpeningBalance(int supplierId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmdLocal = new SqlCommand(@"
                    SELECT TOP 1 CAST(ISNULL(credit, 0) - ISNULL(debit, 0) AS decimal(18,2))
                    FROM pos_suppliers_payments
                    WHERE branch_id = @branch_id
                      AND supplier_id = @supplier_id
                      AND (
                            invoice_no = @opening_invoice_no
                            OR CHARINDEX(@opening_marker, ISNULL(description, '')) = 1
                          )
                    ORDER BY id DESC", cn))
            {
                cmdLocal.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmdLocal.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplierId;
                cmdLocal.Parameters.Add("@opening_invoice_no", SqlDbType.NVarChar, 50).Value = BuildOpeningBalanceInvoiceNo(supplierId);
                cmdLocal.Parameters.Add("@opening_marker", SqlDbType.NVarChar).Value = OpeningBalanceMarker;

                cn.Open();
                object result = cmdLocal.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0m : Math.Round(Convert.ToDecimal(result), 2);
            }
        }

        private static int GetFallbackPayableAccountId(SqlConnection cn)
        {
            using (SqlCommand cmdLocal = new SqlCommand("SELECT TOP 1 payable_acc_id FROM pos_companies", cn))
            {
                object result = cmdLocal.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
            }
        }

        private static string BuildOpeningBalanceInvoiceNo(int supplierId)
        {
            return "SUP-OB-" + supplierId;
        }

        private static string BuildOpeningBalanceDescription()
        {
            return OpeningBalanceMarker + " Supplier opening balance";
        }

        private static void UpsertSupplierOpeningBalance(SqlConnection cn, int supplierId, int accountId, decimal openingBalance)
        {
            const string findQuery = @"
                    SELECT TOP 1 id
                    FROM pos_suppliers_payments
                    WHERE branch_id = @branch_id
                      AND supplier_id = @supplier_id
                      AND (
                            invoice_no = @opening_invoice_no
                            OR CHARINDEX(@opening_marker, ISNULL(description, '')) = 1
                          )
                    ORDER BY id DESC";

            int existingId = 0;
            using (SqlCommand findCmd = new SqlCommand(findQuery, cn))
            {
                findCmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                findCmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplierId;
                findCmd.Parameters.Add("@opening_invoice_no", SqlDbType.NVarChar, 50).Value = BuildOpeningBalanceInvoiceNo(supplierId);
                findCmd.Parameters.Add("@opening_marker", SqlDbType.NVarChar).Value = OpeningBalanceMarker;

                object existing = findCmd.ExecuteScalar();
                existingId = existing == null || existing == DBNull.Value ? 0 : Convert.ToInt32(existing);
            }

            decimal roundedOpening = Math.Round(openingBalance, 2);
            if (roundedOpening <= 0)
            {
                if (existingId > 0)
                {
                    using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM pos_suppliers_payments WHERE id = @id AND branch_id = @branch_id", cn))
                    {
                        deleteCmd.Parameters.Add("@id", SqlDbType.Int).Value = existingId;
                        deleteCmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                        deleteCmd.ExecuteNonQuery();
                    }
                }

                return;
            }

            int effectiveAccountId = accountId > 0 ? accountId : GetFallbackPayableAccountId(cn);
            string invoiceNo = BuildOpeningBalanceInvoiceNo(supplierId);
            string description = BuildOpeningBalanceDescription();

            if (existingId > 0)
            {
                using (SqlCommand updateCmd = new SqlCommand(@"
                            UPDATE pos_suppliers_payments
                            SET invoice_no = @invoice_no,
                                debit = 0,
                                credit = @credit,
                                description = @description,
                                entry_date = @entry_date,
                                account_id = @account_id,
                                supplier_id = @supplier_id,
                                user_id = @user_id,
                                date_created = @date_created,
                                entry_id = 0,
                                payment_ref_invoice_no = ''
                            WHERE id = @id
                              AND branch_id = @branch_id", cn))
                {
                    updateCmd.Parameters.Add("@id", SqlDbType.Int).Value = existingId;
                    updateCmd.Parameters.Add("@invoice_no", SqlDbType.NVarChar, 50).Value = invoiceNo;
                    updateCmd.Parameters.Add("@credit", SqlDbType.Decimal).Value = roundedOpening;
                    updateCmd.Parameters["@credit"].Precision = 18;
                    updateCmd.Parameters["@credit"].Scale = 2;
                    updateCmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = description;
                    updateCmd.Parameters.Add("@entry_date", SqlDbType.DateTime).Value = DateTime.Now;
                    updateCmd.Parameters.Add("@account_id", SqlDbType.Int).Value = effectiveAccountId;
                    updateCmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplierId;
                    updateCmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UsersModal.logged_in_userid;
                    updateCmd.Parameters.Add("@date_created", SqlDbType.DateTime).Value = DateTime.Now;
                    updateCmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                    updateCmd.ExecuteNonQuery();
                }

                return;
            }

            using (SqlCommand insertCmd = new SqlCommand(@"
                    INSERT INTO pos_suppliers_payments
                    (
                        invoice_no,
                        debit,
                        credit,
                        description,
                        entry_date,
                        account_id,
                        supplier_id,
                        user_id,
                        branch_id,
                        date_created,
                        entry_id,
                        payment_ref_invoice_no
                    )
                    VALUES
                    (
                        @invoice_no,
                        0,
                        @credit,
                        @description,
                        @entry_date,
                        @account_id,
                        @supplier_id,
                        @user_id,
                        @branch_id,
                        @date_created,
                        0,
                        ''
                    )", cn))
            {
                insertCmd.Parameters.Add("@invoice_no", SqlDbType.NVarChar, 50).Value = invoiceNo;
                insertCmd.Parameters.Add("@credit", SqlDbType.Decimal).Value = roundedOpening;
                insertCmd.Parameters["@credit"].Precision = 18;
                insertCmd.Parameters["@credit"].Scale = 2;
                insertCmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = description;
                insertCmd.Parameters.Add("@entry_date", SqlDbType.DateTime).Value = DateTime.Now;
                insertCmd.Parameters.Add("@account_id", SqlDbType.Int).Value = effectiveAccountId;
                insertCmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplierId;
                insertCmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UsersModal.logged_in_userid;
                insertCmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                insertCmd.Parameters.Add("@date_created", SqlDbType.DateTime).Value = DateTime.Now;
                insertCmd.ExecuteNonQuery();
            }
        }

        public DataTable SearchRecord(String condition)
        {
            DataTable dt = new DataTable(); // Instantiate DataTable

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = @"SELECT *
                                FROM pos_suppliers 
                                WHERE branch_id = @branch_id 
                                AND (first_name LIKE @condition OR last_name LIKE @condition 
                                OR supplier_code LIKE @condition
                                OR contact_no LIKE @condition
                                OR vat_no LIKE @condition OR address LIKE @condition)";

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

        public DataTable GetPendingSupplierInvoices(int supplierId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"
            SELECT
                p.invoice_no,
                p.purchase_date,
                CAST(ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0) AS decimal(18,4)) AS invoice_amount,
                CAST(ISNULL(SUM(sp.debit), 0) AS decimal(18,4)) AS paid_amount,
                CAST((ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0)) - ISNULL(SUM(sp.debit), 0) AS decimal(18,4)) AS balance_amount
            FROM pos_purchases p
            LEFT JOIN pos_suppliers_payments sp
                ON sp.branch_id = p.branch_id
                AND sp.supplier_id = p.supplier_id
                AND sp.invoice_no = p.invoice_no
            WHERE p.branch_id = @branch_id
              AND p.supplier_id = @supplier_id
              AND ISNULL(p.purchase_type, '') = 'Credit'
            GROUP BY p.invoice_no, p.purchase_date, p.total_amount, p.total_tax, p.discount_value
            HAVING ((ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0)) - ISNULL(SUM(sp.debit), 0)) > 0.004
            ORDER BY p.purchase_date DESC, p.invoice_no DESC", cn))
            {
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmd.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplierId;

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
                        DateTime purchaseDate;
                        DateTime.TryParse(Convert.ToString(row["purchase_date"]), out purchaseDate);

                        decimal balanceAmount = 0m;
                        if (row["balance_amount"] != DBNull.Value)
                            balanceAmount = Convert.ToDecimal(row["balance_amount"]);

                        row["display_text"] = string.Format(
                            "{0} | {1:yyyy-MM-dd} | Balance: {2:N2}",
                            Convert.ToString(row["invoice_no"]),
                            purchaseDate == DateTime.MinValue ? DateTime.Today : purchaseDate,
                            balanceAmount);
                    }

                    return dt;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching pending supplier invoices: " + ex.Message, ex);
                }
            }
        }

        public DataTable GetSupplierDashboardKPIs()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmdLocal = new SqlCommand(@"
;WITH PurchaseSummary AS
(
    SELECT
        p.supplier_id,
        CAST(p.purchase_date AS date) AS purchase_date,
        CAST(ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0) AS decimal(18,4)) AS invoice_amount,
        DATEADD(DAY,
            CASE WHEN ISNUMERIC(p.payment_terms_id) = 1 THEN CAST(p.payment_terms_id AS int) ELSE 30 END,
            CAST(p.purchase_date AS date)) AS due_date,
        ISNULL(p.purchase_type, '') AS purchase_type
    FROM pos_purchases p
    WHERE p.branch_id = @branch_id
),
SupplierLedgerBal AS
(
    -- Net balance per supplier = what supplier is still owed (credit entries = liability raised,
    -- debit entries = payments made + discounts given + other reductions)
    SELECT
        sp.supplier_id,
        CAST(ISNULL(SUM(ISNULL(sp.credit, 0)) - SUM(ISNULL(sp.debit, 0)), 0) AS decimal(18,4)) AS net_payable
    FROM pos_suppliers_payments sp
    WHERE sp.branch_id = @branch_id
    GROUP BY sp.supplier_id
),
OverdueSuppliers AS
(
    -- A supplier is overdue if they have any past-due credit invoice AND still carry a payable balance
    SELECT DISTINCT ps.supplier_id
    FROM PurchaseSummary ps
    INNER JOIN SupplierLedgerBal lb ON lb.supplier_id = ps.supplier_id
    WHERE ps.purchase_type = 'Credit'
      AND ps.due_date < CAST(GETDATE() AS date)
      AND lb.net_payable > 0.004
),
MonthlyPurchases AS
(
    SELECT
        SUM(CASE WHEN YEAR(ps.purchase_date) = YEAR(GETDATE()) AND MONTH(ps.purchase_date) = MONTH(GETDATE()) THEN ps.invoice_amount ELSE 0 END) AS this_month,
        SUM(CASE WHEN YEAR(ps.purchase_date) = YEAR(DATEADD(MONTH,-1,GETDATE())) AND MONTH(ps.purchase_date) = MONTH(DATEADD(MONTH,-1,GETDATE())) THEN ps.invoice_amount ELSE 0 END) AS prev_month
    FROM PurchaseSummary ps
)
SELECT
    CAST((SELECT COUNT(1) FROM pos_suppliers s WHERE s.branch_id = @branch_id) AS int) AS TotalSuppliers,
    CAST(ISNULL((SELECT SUM(CASE WHEN lb.net_payable > 0 THEN lb.net_payable ELSE 0 END) FROM SupplierLedgerBal lb), 0) AS decimal(18,2)) AS TotalPayables,
    CAST(ISNULL((SELECT SUM(lb.net_payable) FROM SupplierLedgerBal lb INNER JOIN OverdueSuppliers os ON os.supplier_id = lb.supplier_id WHERE lb.net_payable > 0), 0) AS decimal(18,2)) AS OverduePayables,
    CAST(ISNULL((SELECT this_month FROM MonthlyPurchases), 0) AS decimal(18,2)) AS TotalPurchasesThisMonth,
    CAST(ISNULL((SELECT prev_month FROM MonthlyPurchases), 0) AS decimal(18,2)) AS TotalPurchasesPrevMonth", cn))
            {
                cmdLocal.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

                DataTable dt = new DataTable();
                cn.Open();
                using (SqlDataAdapter daLocal = new SqlDataAdapter(cmdLocal))
                {
                    daLocal.Fill(dt);
                }
                return dt;
            }
        }

        public DataTable GetSupplierSummaryDashboard(string searchText = null, string category = null, string statusFilter = "All")
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmdLocal = new SqlCommand(@"
;WITH Purchases AS
(
    SELECT
        p.supplier_id,
        p.invoice_no,
        CAST(p.purchase_date AS date) AS purchase_date,
        CAST(ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0) AS decimal(18,4)) AS invoice_amount,
        DATEADD(DAY,
            CASE WHEN ISNUMERIC(p.payment_terms_id) = 1 THEN CAST(p.payment_terms_id AS int) ELSE 30 END,
            CAST(p.purchase_date AS date)) AS due_date,
        ISNULL(p.purchase_type, '') AS purchase_type
    FROM pos_purchases p
    WHERE p.branch_id = @branch_id
),
PurchaseAgg AS
(
    SELECT
        pu.supplier_id,
        CAST(ISNULL(SUM(pu.invoice_amount), 0) AS decimal(18,2)) AS total_purchases,
        MAX(pu.purchase_date) AS last_bill_date
    FROM Purchases pu
    GROUP BY pu.supplier_id
),
SupplierLedgerBal AS
(
    -- Net payable = SUM(credit) - SUM(debit) from the supplier sub-ledger.
    -- Credit entries: raised when a credit purchase is posted (liability).
    -- Debit entries: reduced when payment is made OR purchase discount is applied.
    SELECT
        sp.supplier_id,
        CAST(ISNULL(SUM(ISNULL(sp.credit, 0)), 0) AS decimal(18,4)) AS total_credit,
        CAST(ISNULL(SUM(ISNULL(sp.debit, 0)), 0) AS decimal(18,4)) AS total_debit,
        CAST(ISNULL(SUM(ISNULL(sp.credit, 0)) - SUM(ISNULL(sp.debit, 0)), 0) AS decimal(18,4)) AS net_payable
    FROM pos_suppliers_payments sp
    WHERE sp.branch_id = @branch_id
    GROUP BY sp.supplier_id
),
SupplierAgg AS
(
    SELECT
        ISNULL(pa.supplier_id, lb.supplier_id) AS supplier_id,
        ISNULL(pa.total_purchases, 0) AS total_purchases,
        -- total_paid shown in detail panel: sum of actual payment debits
        ISNULL(lb.total_debit, 0) AS total_paid,
        -- payable balance: net from ledger, floor at 0
        CAST(CASE WHEN ISNULL(lb.net_payable, 0) > 0 THEN lb.net_payable ELSE 0 END AS decimal(18,2)) AS payable_balance,
        ISNULL(pa.last_bill_date, NULL) AS last_bill_date,
        -- overdue: any past-due credit invoice exists AND supplier still has a net payable
        CAST(CASE
            WHEN ISNULL(lb.net_payable, 0) > 0.004
             AND EXISTS (
                SELECT 1 FROM Purchases pu2
                WHERE pu2.supplier_id = ISNULL(pa.supplier_id, lb.supplier_id)
                  AND pu2.purchase_type = 'Credit'
                  AND pu2.due_date < CAST(GETDATE() AS date)
             )
            THEN 1 ELSE 0 END AS int) AS is_overdue
    FROM PurchaseAgg pa
    FULL OUTER JOIN SupplierLedgerBal lb ON lb.supplier_id = pa.supplier_id
)
SELECT
    s.id AS supplier_id,
    ISNULL(s.supplier_code, '') AS supplier_code,
    LTRIM(RTRIM(ISNULL(s.first_name, '') + ' ' + ISNULL(s.last_name, ''))) AS supplier_name,
    CASE
        WHEN CHARINDEX('|', ISNULL(s.address, '')) > 0 THEN LTRIM(RTRIM(LEFT(s.address, CHARINDEX('|', s.address) - 1)))
        WHEN CHARINDEX(',', ISNULL(s.address, '')) > 0 THEN LTRIM(RTRIM(LEFT(s.address, CHARINDEX(',', s.address) - 1)))
        ELSE 'General'
    END AS category,
    ISNULL(s.contact_no, '') AS contact_no,
    ISNULL(s.address, '') AS address,
    ISNULL(sa.total_purchases, 0) AS total_purchases,
    ISNULL(sa.total_paid, 0) AS total_paid,
    ISNULL(sa.payable_balance, 0) AS payable_balance,
    CAST(30 AS int) AS credit_days,
    CAST(0 AS decimal(18,2)) AS credit_limit,
    sa.last_bill_date,
    CASE WHEN sa.last_bill_date IS NULL THEN 0 ELSE DATEDIFF(DAY, sa.last_bill_date, CAST(GETDATE() AS date)) END AS days_since_last_purchase,
    CASE WHEN ISNULL(s.status, 1) = 1 THEN 1 ELSE 0 END AS is_active,
    ISNULL(sa.is_overdue, 0) AS is_overdue
FROM pos_suppliers s
LEFT JOIN SupplierAgg sa ON sa.supplier_id = s.id
WHERE s.branch_id = @branch_id
  AND (
        @SearchText IS NULL
        OR LTRIM(RTRIM(ISNULL(s.first_name, '') + ' ' + ISNULL(s.last_name, ''))) LIKE '%' + @SearchText + '%'
        OR ISNULL(s.supplier_code, '') LIKE '%' + @SearchText + '%'
        OR ISNULL(s.contact_no, '') LIKE '%' + @SearchText + '%'
        OR ISNULL(s.vat_no, '') LIKE '%' + @SearchText + '%'
        OR ISNULL(s.address, '') LIKE '%' + @SearchText + '%'
      )
  AND (
        @Category IS NULL
        OR (
            CASE
                WHEN CHARINDEX('|', ISNULL(s.address, '')) > 0 THEN LTRIM(RTRIM(LEFT(s.address, CHARINDEX('|', s.address) - 1)))
                WHEN CHARINDEX(',', ISNULL(s.address, '')) > 0 THEN LTRIM(RTRIM(LEFT(s.address, CHARINDEX(',', s.address) - 1)))
                ELSE 'General'
            END
        ) = @Category
      )
ORDER BY LTRIM(RTRIM(ISNULL(s.first_name, '') + ' ' + ISNULL(s.last_name, '')))", cn))
            {
                string status = string.IsNullOrWhiteSpace(statusFilter) ? "All" : statusFilter.Trim();
                cmdLocal.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmdLocal.Parameters.Add("@SearchText", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(searchText) ? (object)DBNull.Value : searchText.Trim();
                cmdLocal.Parameters.Add("@Category", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(category) ? (object)DBNull.Value : category.Trim();

                DataTable dt = new DataTable();
                cn.Open();
                using (SqlDataAdapter daLocal = new SqlDataAdapter(cmdLocal))
                {
                    daLocal.Fill(dt);
                }

                if (dt.Columns.Contains("supplier_id") && !dt.Columns.Contains("id")) dt.Columns.Add("id", typeof(int));
                if (!dt.Columns.Contains("supplier_name")) dt.Columns.Add("supplier_name", typeof(string));
                if (!dt.Columns.Contains("category")) dt.Columns.Add("category", typeof(string));
                if (!dt.Columns.Contains("contact_no")) dt.Columns.Add("contact_no", typeof(string));
                if (!dt.Columns.Contains("address")) dt.Columns.Add("address", typeof(string));
                if (!dt.Columns.Contains("total_purchases")) dt.Columns.Add("total_purchases", typeof(decimal));
                if (!dt.Columns.Contains("payable_balance")) dt.Columns.Add("payable_balance", typeof(decimal));
                if (!dt.Columns.Contains("credit_days")) dt.Columns.Add("credit_days", typeof(int));
                if (!dt.Columns.Contains("credit_limit")) dt.Columns.Add("credit_limit", typeof(decimal));
                if (!dt.Columns.Contains("last_bill_date")) dt.Columns.Add("last_bill_date", typeof(DateTime));
                if (!dt.Columns.Contains("days_since_last_purchase")) dt.Columns.Add("days_since_last_purchase", typeof(int));
                if (!dt.Columns.Contains("is_active")) dt.Columns.Add("is_active", typeof(int));
                if (!dt.Columns.Contains("is_overdue")) dt.Columns.Add("is_overdue", typeof(int));
                if (!dt.Columns.Contains("total_paid")) dt.Columns.Add("total_paid", typeof(decimal));
                if (!dt.Columns.Contains("status_text")) dt.Columns.Add("status_text", typeof(string));
                if (!dt.Columns.Contains("row_no")) dt.Columns.Add("row_no", typeof(int));

                int i = 1;
                foreach (DataRow r in dt.Rows)
                {
                    if (dt.Columns.Contains("id") && dt.Columns.Contains("supplier_id"))
                        r["id"] = r["supplier_id"] == DBNull.Value ? 0 : Convert.ToInt32(r["supplier_id"]);

                    if (dt.Columns.Contains("supplier_name") && string.IsNullOrWhiteSpace(Convert.ToString(r["supplier_name"])) && dt.Columns.Contains("first_name"))
                    {
                        string fullName = (Convert.ToString(r["first_name"]) + " " + Convert.ToString(dt.Columns.Contains("last_name") ? r["last_name"] : string.Empty)).Trim();
                        r["supplier_name"] = fullName;
                    }

                    if (dt.Columns.Contains("status_text"))
                    {
                        bool isActive = !dt.Columns.Contains("is_active") || r["is_active"] == DBNull.Value || Convert.ToInt32(r["is_active"]) == 1;
                        bool isOverdue = dt.Columns.Contains("is_overdue") && r["is_overdue"] != DBNull.Value && Convert.ToInt32(r["is_overdue"]) == 1;
                        r["status_text"] = !isActive ? "Inactive" : (isOverdue ? "Overdue" : "Active");
                    }

                    if (dt.Columns.Contains("row_no")) r["row_no"] = i++;
                }

                if (!string.Equals(status, "All", StringComparison.OrdinalIgnoreCase) && dt.Columns.Contains("status_text"))
                {
                    DataView view = dt.DefaultView;
                    view.RowFilter = "status_text = '" + status.Replace("'", "''") + "'";
                    return view.ToTable();
                }

                return dt;
            }
        }

        public DataTable GetSupplierRecentBills(int supplierId, int top = 5)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmdLocal = new SqlCommand(@"
SELECT TOP (@top)
    p.invoice_no,
    CAST(p.purchase_date AS date) AS purchase_date,
    CAST(ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0) AS decimal(18,2)) AS amount,
    CAST(ISNULL(SUM(sp.debit), 0) AS decimal(18,2)) AS paid,
    CAST((ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0)) - ISNULL(SUM(sp.debit), 0) AS decimal(18,2)) AS balance
FROM pos_purchases p
LEFT JOIN pos_suppliers_payments sp
    ON sp.branch_id = p.branch_id
   AND sp.supplier_id = p.supplier_id
   AND sp.invoice_no = p.invoice_no
WHERE p.branch_id = @branch_id
  AND p.supplier_id = @supplier_id
GROUP BY p.invoice_no, p.purchase_date, p.total_amount, p.total_tax, p.discount_value
ORDER BY p.purchase_date DESC, p.invoice_no DESC", cn))
            {
                cmdLocal.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmdLocal.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplierId;
                cmdLocal.Parameters.Add("@top", SqlDbType.Int).Value = top <= 0 ? 5 : top;

                DataTable dt = new DataTable();
                cn.Open();
                using (SqlDataAdapter daLocal = new SqlDataAdapter(cmdLocal))
                {
                    daLocal.Fill(dt);
                }
                return dt;
            }
        }

        public DataTable GetSupplierProfileOverview(int supplierId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmdLocal = new SqlCommand(@"
SELECT
    s.id,
    ISNULL(s.supplier_code, '') AS supplier_code,
    ISNULL(s.first_name, '') AS first_name,
    ISNULL(s.last_name, '') AS last_name,
    ISNULL(s.email, '') AS email,
    ISNULL(s.contact_no, '') AS contact_no,
    ISNULL(s.address, '') AS address,
    ISNULL(s.status, 1) AS status,
    CAST(30 AS int) AS credit_days,
    CAST(0 AS decimal(18,2)) AS credit_limit,
    -- Lifetime purchases: all invoices (informational total)
    CAST(ISNULL((
        SELECT SUM(ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0))
        FROM pos_purchases p
        WHERE p.branch_id = s.branch_id AND p.supplier_id = s.id
    ), 0) AS decimal(18,2)) AS lifetime_purchases,
    -- Total paid (debit side of sub-ledger: payments + discounts given)
    CAST(ISNULL((
        SELECT SUM(ISNULL(sp.debit, 0))
        FROM pos_suppliers_payments sp
        WHERE sp.branch_id = s.branch_id AND sp.supplier_id = s.id
    ), 0) AS decimal(18,2)) AS total_paid,
    -- Current payable: net from sub-ledger = SUM(credit) - SUM(debit), floor at 0
    CAST(CASE
        WHEN ISNULL((SELECT SUM(ISNULL(sp.credit,0)) - SUM(ISNULL(sp.debit,0))
                     FROM pos_suppliers_payments sp
                     WHERE sp.branch_id = s.branch_id AND sp.supplier_id = s.id), 0) > 0
        THEN (SELECT SUM(ISNULL(sp.credit,0)) - SUM(ISNULL(sp.debit,0))
              FROM pos_suppliers_payments sp
              WHERE sp.branch_id = s.branch_id AND sp.supplier_id = s.id)
        ELSE 0
    END AS decimal(18,2)) AS current_payable,
    -- Available credit: positive when overpaid (debit > credit)
    CAST(CASE
        WHEN ISNULL((SELECT SUM(ISNULL(sp.debit,0)) - SUM(ISNULL(sp.credit,0))
                     FROM pos_suppliers_payments sp
                     WHERE sp.branch_id = s.branch_id AND sp.supplier_id = s.id), 0) > 0
        THEN (SELECT SUM(ISNULL(sp.debit,0)) - SUM(ISNULL(sp.credit,0))
              FROM pos_suppliers_payments sp
              WHERE sp.branch_id = s.branch_id AND sp.supplier_id = s.id)
        ELSE 0
    END AS decimal(18,2)) AS available_credit
FROM pos_suppliers s
WHERE s.branch_id = @branch_id AND s.id = @supplier_id", cn))
            {
                cmdLocal.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmdLocal.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplierId;

                DataTable dt = new DataTable();
                cn.Open();
                using (SqlDataAdapter daLocal = new SqlDataAdapter(cmdLocal))
                {
                    daLocal.Fill(dt);
                }
                return dt;
            }
        }

        public DataTable GetSupplierMonthlyPurchaseHistory(int supplierId, int months = 12)
        {
            int safeMonths = months <= 0 ? 12 : months;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmdLocal = new SqlCommand(@"
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
        DATEADD(MONTH, DATEDIFF(MONTH, 0, p.purchase_date), 0) AS MonthStart,
        SUM(CAST(ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0) AS decimal(18,4))) AS total_amount
    FROM pos_purchases p
    WHERE p.branch_id = @branch_id
      AND p.supplier_id = @supplier_id
    GROUP BY DATEADD(MONTH, DATEDIFF(MONTH, 0, p.purchase_date), 0)
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
                cmdLocal.Parameters.Add("@months", SqlDbType.Int).Value = safeMonths;
                cmdLocal.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmdLocal.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplierId;

                DataTable dt = new DataTable();
                cn.Open();
                using (SqlDataAdapter daLocal = new SqlDataAdapter(cmdLocal))
                {
                    daLocal.Fill(dt);
                }
                return dt;
            }
        }

        public DataTable GetSupplierTopItems(int supplierId, int top = 5)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmdLocal = new SqlCommand(@"
SELECT TOP (@top)
    ISNULL(pi.item_number, '') AS item_no,
    ISNULL(pr.name, ISNULL(pi.item_number, '')) AS item_name,
    CAST(SUM(ISNULL(pi.quantity, 0)) AS decimal(18,2)) AS qty,
    CAST(SUM((ISNULL(pi.cost_price, 0) * ISNULL(pi.quantity, 0)) - ABS(ISNULL(pi.discount_value, 0))) AS decimal(18,2)) AS total_value
FROM pos_purchases_items pi
INNER JOIN pos_purchases ph ON ph.branch_id = pi.branch_id AND ph.invoice_no = pi.invoice_no
LEFT JOIN pos_products pr ON pr.item_number = pi.item_number
WHERE pi.branch_id = @branch_id
  AND ph.supplier_id = @supplier_id
GROUP BY pi.item_number, pr.name
ORDER BY total_value DESC", cn))
            {
                cmdLocal.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmdLocal.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplierId;
                cmdLocal.Parameters.Add("@top", SqlDbType.Int).Value = top <= 0 ? 5 : top;

                DataTable dt = new DataTable();
                cn.Open();
                using (SqlDataAdapter daLocal = new SqlDataAdapter(cmdLocal))
                {
                    daLocal.Fill(dt);
                }
                return dt;
            }
        }

        public DataTable GetSupplierLedger(int supplierId, DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmdLocal = new SqlCommand(@"
SELECT
    x.entry_date,
    x.trans_type,
    x.reference_no,
    x.debit,
    x.credit,
    x.running_balance
FROM
(
    SELECT
        CAST(p.purchase_date AS datetime) AS entry_date,
        CAST('Bill' AS varchar(20)) AS trans_type,
        ISNULL(p.invoice_no, '') AS reference_no,
        CAST(0 AS decimal(18,2)) AS debit,
        CAST(ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0) AS decimal(18,2)) AS credit,
        CAST(0 AS decimal(18,2)) AS running_balance,
        1 AS sort_no,
        p.id AS sort_id
    FROM pos_purchases p
    WHERE p.branch_id = @branch_id
      AND p.supplier_id = @supplier_id
      AND CAST(p.purchase_date AS date) BETWEEN @from_date AND @to_date

    UNION ALL

    SELECT
        CAST(sp.entry_date AS datetime) AS entry_date,
        CASE WHEN ISNULL(sp.debit, 0) > 0 THEN 'Payment' ELSE 'Debit Note' END AS trans_type,
        ISNULL(sp.invoice_no, '') AS reference_no,
        CAST(ISNULL(sp.debit, 0) AS decimal(18,2)) AS debit,
        CAST(0 AS decimal(18,2)) AS credit,
        CAST(0 AS decimal(18,2)) AS running_balance,
        2 AS sort_no,
        sp.id AS sort_id
    FROM pos_suppliers_payments sp
    WHERE sp.branch_id = @branch_id
      AND sp.supplier_id = @supplier_id
      AND CAST(sp.entry_date AS date) BETWEEN @from_date AND @to_date
) x
ORDER BY x.entry_date, x.sort_no, x.sort_id", cn))
            {
                cmdLocal.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmdLocal.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplierId;
                cmdLocal.Parameters.Add("@from_date", SqlDbType.Date).Value = fromDate.Date;
                cmdLocal.Parameters.Add("@to_date", SqlDbType.Date).Value = toDate.Date;

                DataTable dt = new DataTable();
                cn.Open();
                using (SqlDataAdapter daLocal = new SqlDataAdapter(cmdLocal))
                {
                    daLocal.Fill(dt);
                }

                decimal running = 0m;
                foreach (DataRow row in dt.Rows)
                {
                    running += (row["credit"] == DBNull.Value ? 0m : Convert.ToDecimal(row["credit"]))
                               - (row["debit"] == DBNull.Value ? 0m : Convert.ToDecimal(row["debit"]));
                    row["running_balance"] = Math.Round(running, 2);
                }

                return dt;
            }
        }

        public DataTable GetSupplierOutstandingBills(int supplierId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmdLocal = new SqlCommand(@"
SELECT
    p.invoice_no,
    p.purchase_date,
    DATEADD(DAY,
        CASE
            WHEN ISNUMERIC(p.payment_terms_id) = 1 THEN CAST(p.payment_terms_id AS int)
            ELSE 30
        END,
        p.purchase_date) AS due_date,
    CAST(ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0) AS decimal(18,2)) AS amount,
    CAST(ISNULL(SUM(sp.debit), 0) AS decimal(18,2)) AS paid,
    CAST((ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0)) - ISNULL(SUM(sp.debit), 0) AS decimal(18,2)) AS balance,
    CASE
        WHEN DATEDIFF(DAY, DATEADD(DAY,
            CASE WHEN ISNUMERIC(p.payment_terms_id) = 1 THEN CAST(p.payment_terms_id AS int) ELSE 30 END,
            p.purchase_date), GETDATE()) < 0 THEN 0
        ELSE DATEDIFF(DAY, DATEADD(DAY,
            CASE WHEN ISNUMERIC(p.payment_terms_id) = 1 THEN CAST(p.payment_terms_id AS int) ELSE 30 END,
            p.purchase_date), GETDATE())
    END AS days_overdue
FROM pos_purchases p
LEFT JOIN pos_suppliers_payments sp
    ON sp.branch_id = p.branch_id
   AND sp.supplier_id = p.supplier_id
   AND sp.invoice_no = p.invoice_no
WHERE p.branch_id = @branch_id
  AND p.supplier_id = @supplier_id
  AND ISNULL(p.purchase_type, '') = 'Credit'
  AND ISNULL(p.account, '') <> 'Return'
GROUP BY p.invoice_no, p.purchase_date, p.payment_terms_id, p.total_amount, p.total_tax, p.discount_value
HAVING ((ISNULL(p.total_amount, 0) + ISNULL(p.total_tax, 0) - ISNULL(p.discount_value, 0)) - ISNULL(SUM(sp.debit), 0)) > 0.004
ORDER BY p.purchase_date DESC, p.invoice_no DESC", cn))
            {
                cmdLocal.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmdLocal.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplierId;

                DataTable dt = new DataTable();
                cn.Open();
                using (SqlDataAdapter daLocal = new SqlDataAdapter(cmdLocal))
                {
                    daLocal.Fill(dt);
                }
                return dt;
            }
        }

        public DataTable GetSupplierPayableAgingSummary(int supplierId)
        {
            DataTable outDt = new DataTable();
            outDt.Columns.Add("bucket_current", typeof(decimal));
            outDt.Columns.Add("bucket_1_30", typeof(decimal));
            outDt.Columns.Add("bucket_31_60", typeof(decimal));
            outDt.Columns.Add("bucket_61_90", typeof(decimal));
            outDt.Columns.Add("bucket_90_plus", typeof(decimal));

            DataTable bills = GetSupplierOutstandingBills(supplierId);
            decimal cur = 0m, b1 = 0m, b31 = 0m, b61 = 0m, b90 = 0m;
            foreach (DataRow r in bills.Rows)
            {
                decimal bal = r["balance"] == DBNull.Value ? 0m : Convert.ToDecimal(r["balance"]);
                int days = r["days_overdue"] == DBNull.Value ? 0 : Convert.ToInt32(r["days_overdue"]);

                if (days <= 0) cur += bal;
                else if (days <= 30) b1 += bal;
                else if (days <= 60) b31 += bal;
                else if (days <= 90) b61 += bal;
                else b90 += bal;
            }

            DataRow row = outDt.NewRow();
            row["bucket_current"] = Math.Round(cur, 2);
            row["bucket_1_30"] = Math.Round(b1, 2);
            row["bucket_31_60"] = Math.Round(b31, 2);
            row["bucket_61_90"] = Math.Round(b61, 2);
            row["bucket_90_plus"] = Math.Round(b90, 2);
            outDt.Rows.Add(row);

            return outDt;
        }

        public DataTable GetSupplierTopProductsByValue(int supplierId, int top = 10)
        {
            return GetSupplierTopItems(supplierId, top <= 0 ? 10 : top);
        }

        public DataTable GetSupplierMonthlySpendTrend(int supplierId, int months = 24)
        {

            return GetSupplierMonthlyPurchaseHistory(supplierId, months <= 0 ? 24 : months);
        }

        public DataTable GetSupplierProductsForPriceHistory(int supplierId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmdLocal = new SqlCommand(@"
SELECT DISTINCT
    ISNULL(pi.item_number, '') AS item_no,
    ISNULL(p.name, ISNULL(pi.item_number, '')) AS item_name
FROM pos_purchases_items pi
INNER JOIN pos_purchases ph ON ph.branch_id = pi.branch_id AND ph.invoice_no = pi.invoice_no
LEFT JOIN pos_products p ON p.item_number = pi.item_number
WHERE pi.branch_id = @branch_id
  AND ph.supplier_id = @supplier_id
ORDER BY item_name", cn))
            {
                cmdLocal.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmdLocal.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplierId;

                DataTable dt = new DataTable();
                cn.Open();
                using (SqlDataAdapter daLocal = new SqlDataAdapter(cmdLocal))
                {
                    daLocal.Fill(dt);
                }
                return dt;
            }
        }

        public DataTable GetSupplierProductPriceHistory(int supplierId, string itemNumber)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmdLocal = new SqlCommand(@"
SELECT
    p.purchase_date,
    CAST(ISNULL(pi.cost_price, 0) AS decimal(18,4)) AS unit_cost,
    ISNULL(p.invoice_no, '') AS invoice_no
FROM pos_purchases_items pi
INNER JOIN pos_purchases p ON p.branch_id = pi.branch_id AND p.invoice_no = pi.invoice_no
WHERE pi.branch_id = @branch_id
  AND p.supplier_id = @supplier_id
  AND pi.item_number = @item_no
ORDER BY p.purchase_date, pi.id", cn))
            {
                cmdLocal.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmdLocal.Parameters.Add("@supplier_id", SqlDbType.Int).Value = supplierId;
                cmdLocal.Parameters.Add("@item_no", SqlDbType.NVarChar, 100).Value = itemNumber ?? string.Empty;

                DataTable dt = new DataTable();
                cn.Open();
                using (SqlDataAdapter daLocal = new SqlDataAdapter(cmdLocal))
                {
                    daLocal.Fill(dt);
                }
                return dt;
            }
        }

        public int Insert(SupplierModal obj)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        string code = string.IsNullOrWhiteSpace(obj.supplier_code)
                            ? GetNextSupplierCode()
                            : NormalizeSupplierCodeInput(obj.supplier_code);

                        if (string.IsNullOrWhiteSpace(code) || !(code.StartsWith("S") || code.StartsWith("s")) || code.Length < 2)
                        {
                            code = GetNextSupplierCode();
                        }

                        // Prevent duplicates in the same branch
                        using (SqlCommand dupCmd = new SqlCommand(
                            "SELECT COUNT(1) FROM pos_suppliers WHERE branch_id=@branch_id AND supplier_code=@code", cn))
                        {
                            dupCmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                            dupCmd.Parameters.Add("@code", SqlDbType.NVarChar, 50).Value = code;
                            int exists = Convert.ToInt32(dupCmd.ExecuteScalar());
                            if (exists > 0)
                                throw new Exception("Supplier code already exists: " + code);
                        }

                        cmd = new SqlCommand("sp_SuppliersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@first_name", obj.first_name);
                        cmd.Parameters.AddWithValue("@last_name", obj.last_name);
                        cmd.Parameters.AddWithValue("@email", obj.email);
                        cmd.Parameters.AddWithValue("@address", obj.address);
                        cmd.Parameters.AddWithValue("@status", 1);
                        cmd.Parameters.AddWithValue("@vat_status", obj.vat_with_status);
                        cmd.Parameters.AddWithValue("@contact_no", obj.contact_no);
                        cmd.Parameters.AddWithValue("@vat_no", obj.vat_no);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "1");

                        cmd.Parameters.Add("@StreetName", SqlDbType.NVarChar).Value = obj.StreetName;
                        cmd.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = obj.PostalCode;
                        cmd.Parameters.Add("@BuildingNumber", SqlDbType.NVarChar).Value = obj.BuildingNumber;
                        cmd.Parameters.Add("@CitySubdivisionName", SqlDbType.NVarChar).Value = obj.CitySubdivisionName;
                        cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = obj.CityName;
                        cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = obj.CountryName;
                        cmd.Parameters.Add("@GLAccountID", SqlDbType.Int).Value = obj.GLAccountID;
                        cmd.Parameters.Add("@supplier_code", SqlDbType.NVarChar).Value = code;


                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());

                    if (result > 0)
                    {
                        UpsertSupplierOpeningBalance(cn, result, obj.GLAccountID, obj.opening_balance);
                    }

                    Log.LogAction("Add Supplier", $"Supplier ID: {result}, Supplier Name: {obj.first_name + ' ' + obj.last_name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int Update(SupplierModal obj)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        // Normalize/validate supplier code; keep existing code if incoming is invalid
                        string incoming = NormalizeSupplierCodeInput(obj.supplier_code);
                        if (string.IsNullOrWhiteSpace(incoming) || !(incoming.StartsWith("S") || incoming.StartsWith("s")) || incoming.Length < 2)
                        {
                            using (SqlCommand getCmd = new SqlCommand(
                                "SELECT supplier_code FROM pos_suppliers WHERE id=@id AND branch_id=@branch_id", cn))
                            {
                                getCmd.Parameters.Add("@id", SqlDbType.Int).Value = obj.id;
                                getCmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                                var existing = getCmd.ExecuteScalar();
                                incoming = existing == null || existing == DBNull.Value ? string.Empty : Convert.ToString(existing);
                            }
                        }

                        if (string.IsNullOrWhiteSpace(incoming))
                        {
                            incoming = GetNextSupplierCode();
                        }

                        cmd = new SqlCommand("sp_SuppliersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@branch_id", 0);
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        cmd.Parameters.AddWithValue("@first_name", obj.first_name);
                        cmd.Parameters.AddWithValue("@last_name", obj.last_name);
                        cmd.Parameters.AddWithValue("@email", obj.email);
                        cmd.Parameters.AddWithValue("@address", obj.address);
                        cmd.Parameters.AddWithValue("@status", 1);
                        cmd.Parameters.AddWithValue("@vat_status", obj.vat_with_status);
                        cmd.Parameters.AddWithValue("@contact_no", obj.contact_no);
                        cmd.Parameters.AddWithValue("@vat_no", obj.vat_no);
                        cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "2");

                        cmd.Parameters.Add("@StreetName", SqlDbType.NVarChar).Value = obj.StreetName;
                        cmd.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = obj.PostalCode;
                        cmd.Parameters.Add("@BuildingNumber", SqlDbType.NVarChar).Value = obj.BuildingNumber;
                        cmd.Parameters.Add("@CitySubdivisionName", SqlDbType.NVarChar).Value = obj.CitySubdivisionName;
                        cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = obj.CityName;
                        cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = obj.CountryName;
                        cmd.Parameters.Add("@GLAccountID", SqlDbType.Int).Value = obj.GLAccountID;
                        cmd.Parameters.Add("@supplier_code", SqlDbType.NVarChar).Value = incoming;

                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());

                    if (obj.id > 0)
                    {
                        UpsertSupplierOpeningBalance(cn, obj.id, obj.GLAccountID, obj.opening_balance);
                    }

                    Log.LogAction("Update Supplier", $"Supplier ID: {obj.id}, Supplier Name: {obj.first_name + ' ' + obj.last_name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)result;
                }
                catch
                {

                    throw;
                }

            }
        }

        public int Delete(int SupplierId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    int result = 0;
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("sp_SuppliersCrud", cn))
                    {
                        //cmd = new SqlCommand("sp_SuppliersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", SupplierId);
                        cmd.Parameters.AddWithValue("@OperationType", "3");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        result = cmd.ExecuteNonQuery();
                    }

                    Log.LogAction("Delete Supplier", $"Customer ID: {SupplierId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return result;
                }
                catch
                {
                    throw;
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

                    transaction = cn.BeginTransaction();

                    //result += DeletePaymentTransactionRecords(cn, transaction, "pos_suppliers_payments", invoiceNo);
                    result += DeleteSupplierPaymentTransactionRecords(cn, transaction, invoiceNo);
                    result += DeletePaymentTransactionRecords(cn, transaction, "acc_entries", invoiceNo);
                    result += DeletePaymentTransactionRecords(cn, transaction, "pos_banks_payments", invoiceNo);

                    transaction.Commit();

                    if (result > 0)
                    {
                        Log.LogAction("Delete Supplier Payment", $"InvoiceNo: {invoiceNo}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                    }

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

        private static int DeleteSupplierPaymentTransactionRecords(SqlConnection cn, SqlTransaction transaction, string paymentReferenceInvoiceNo)
        {
            const string query = @"DELETE FROM pos_suppliers_payments
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

        public bool IsSupplierCodeExists(string supplierCode, int? excludeSupplierId = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT COUNT(1) FROM pos_suppliers WHERE branch_id=@branch_id AND supplier_code=@code" +
                        (excludeSupplierId.HasValue ? " AND id<>@id" : ""), cn))
                    {
                        cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                        cmd.Parameters.Add("@code", SqlDbType.NVarChar, 50).Value = supplierCode;
                        if (excludeSupplierId.HasValue)
                        {
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = excludeSupplierId.Value;
                        }
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
                catch
                {
                    throw;
                }
            }

        }
    } 
}
