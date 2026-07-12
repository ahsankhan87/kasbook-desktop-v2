using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    public class AccountingDashboardDLL
    {
        private const string AccountClassifyCte = @"
;WITH AccountClassify AS
(
    SELECT
        A.id AS account_id,
        A.name AS account_name,
        CASE
            WHEN LOWER(ISNULL(T.name, '')) LIKE '%income%'
              OR LOWER(ISNULL(T.name, '')) LIKE '%revenue%'
              OR LOWER(ISNULL(G.name, '')) LIKE '%income%'
              OR LOWER(ISNULL(G.name, '')) LIKE '%revenue%'
              OR LOWER(ISNULL(A.name, '')) LIKE '%revenue%'
              OR LOWER(ISNULL(A.name, '')) LIKE '%income%'
                THEN 'Revenue'
            WHEN LOWER(ISNULL(T.name, '')) LIKE '%expense%'
              OR LOWER(ISNULL(G.name, '')) LIKE '%expense%'
              OR LOWER(ISNULL(A.name, '')) LIKE '%expense%'
                THEN 'Expense'
            WHEN LOWER(ISNULL(T.name, '')) LIKE '%receivable%'
              OR LOWER(ISNULL(G.name, '')) LIKE '%receivable%'
              OR LOWER(ISNULL(A.name, '')) LIKE '%receivable%'
              OR LOWER(ISNULL(A.name, '')) LIKE '%debtor%'
                THEN 'Receivable'
            WHEN LOWER(ISNULL(T.name, '')) LIKE '%payable%'
              OR LOWER(ISNULL(G.name, '')) LIKE '%payable%'
              OR LOWER(ISNULL(A.name, '')) LIKE '%payable%'
              OR LOWER(ISNULL(A.name, '')) LIKE '%creditor%'
                THEN 'Payable'
            WHEN LOWER(ISNULL(A.name, '')) LIKE '%cash%'
              OR LOWER(ISNULL(A.name, '')) LIKE '%bank%'
                THEN 'CashBank'
            ELSE 'Other'
        END AS account_class
    FROM acc_accounts A
    LEFT JOIN acc_groups G ON G.id = A.group_id
    LEFT JOIN acc_account_type T ON T.id = G.account_type_id
)
";

        private static DateTime StartOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public DataTable GetKpis(DateTime fromDate, DateTime toDate)
        {
            DateTime toExclusive = toDate.Date.AddDays(1);

            string sql = AccountClassifyCte + @"
SELECT
    CashBankBalance = ISNULL((
        SELECT SUM(ISNULL(E.debit,0) - ISNULL(E.credit,0))
        FROM acc_entries E
        INNER JOIN AccountClassify C ON C.account_id = E.account_id
        WHERE E.branch_id = @branch_id
          AND E.entry_date < @to_exclusive
          AND C.account_class = 'CashBank'
    ), 0),
    TotalReceivables = ISNULL((
        SELECT SUM(CASE WHEN B.balance > 0 THEN B.balance ELSE 0 END)
        FROM (
            SELECT E.invoice_no, SUM(ISNULL(E.debit,0) - ISNULL(E.credit,0)) AS balance
            FROM acc_entries E
            INNER JOIN AccountClassify C ON C.account_id = E.account_id
            WHERE E.branch_id = @branch_id
              AND E.entry_date < @to_exclusive
              AND C.account_class = 'Receivable'
            GROUP BY E.invoice_no
        ) B
    ), 0),
    TotalPayables = ISNULL((
        SELECT SUM(CASE WHEN B.balance > 0 THEN B.balance ELSE 0 END)
        FROM (
            SELECT E.invoice_no, SUM(ISNULL(E.credit,0) - ISNULL(E.debit,0)) AS balance
            FROM acc_entries E
            INNER JOIN AccountClassify C ON C.account_id = E.account_id
            WHERE E.branch_id = @branch_id
              AND E.entry_date < @to_exclusive
              AND C.account_class = 'Payable'
            GROUP BY E.invoice_no
        ) B
    ), 0),
    RevenuePeriod = ISNULL((
        SELECT SUM(ISNULL(E.credit,0))
        FROM acc_entries E
        INNER JOIN AccountClassify C ON C.account_id = E.account_id
        WHERE E.branch_id = @branch_id
          AND E.entry_date >= @from_date
          AND E.entry_date < @to_exclusive
          AND C.account_class = 'Revenue'
    ), 0),
    ExpensesPeriod = ISNULL((
        SELECT SUM(ISNULL(E.debit,0))
        FROM acc_entries E
        INNER JOIN AccountClassify C ON C.account_id = E.account_id
        WHERE E.branch_id = @branch_id
          AND E.entry_date >= @from_date
          AND E.entry_date < @to_exclusive
          AND C.account_class = 'Expense'
    ), 0);";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@from_date", fromDate.Date);
                cmd.Parameters.AddWithValue("@to_exclusive", toExclusive);

                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetMonthlyPnlComparison(DateTime asOfDate, int months)
        {
            DateTime startMonth = StartOfMonth(asOfDate).AddMonths(-1 * (months - 1));

            string sql = AccountClassifyCte + @"
SELECT
    [Year] = YEAR(E.entry_date),
    [Month] = MONTH(E.entry_date),
    Revenue = SUM(CASE WHEN C.account_class = 'Revenue' THEN ISNULL(E.credit,0) ELSE 0 END),
    Expenses = SUM(CASE WHEN C.account_class = 'Expense' THEN ISNULL(E.debit,0) ELSE 0 END)
FROM acc_entries E
INNER JOIN AccountClassify C ON C.account_id = E.account_id
WHERE E.branch_id = @branch_id
  AND E.entry_date >= @start_month
  AND E.entry_date < @to_exclusive
GROUP BY YEAR(E.entry_date), MONTH(E.entry_date)
ORDER BY [Year], [Month];";

            DataTable raw;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@start_month", startMonth);
                cmd.Parameters.AddWithValue("@to_exclusive", StartOfMonth(asOfDate).AddMonths(1));

                raw = new DataTable();
                da.Fill(raw);
            }

            DataTable result = new DataTable();
            result.Columns.Add("MonthLabel", typeof(string));
            result.Columns.Add("Revenue", typeof(decimal));
            result.Columns.Add("Expenses", typeof(decimal));
            result.Columns.Add("NetProfit", typeof(decimal));

            for (int i = 0; i < months; i++)
            {
                DateTime month = startMonth.AddMonths(i);
                decimal revenue = 0m;
                decimal expenses = 0m;

                foreach (DataRow row in raw.Rows)
                {
                    if (Convert.ToInt32(row["Year"]) == month.Year && Convert.ToInt32(row["Month"]) == month.Month)
                    {
                        revenue = row["Revenue"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Revenue"]);
                        expenses = row["Expenses"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Expenses"]);
                        break;
                    }
                }

                DataRow target = result.NewRow();
                target["MonthLabel"] = month.ToString("MMM yy");
                target["Revenue"] = revenue;
                target["Expenses"] = expenses;
                target["NetProfit"] = revenue - expenses;
                result.Rows.Add(target);
            }

            return result;
        }

        public DataTable GetCashFlowWeeklyTrend(DateTime fromDate, DateTime toDate)
        {
            string openingSql = AccountClassifyCte + @"
SELECT ISNULL(SUM(ISNULL(E.debit,0) - ISNULL(E.credit,0)),0)
FROM acc_entries E
INNER JOIN AccountClassify C ON C.account_id = E.account_id
WHERE E.branch_id = @branch_id
  AND C.account_class = 'CashBank'
  AND E.entry_date < @from_date;";

            decimal openingBalance = 0m;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(openingSql, cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@from_date", fromDate.Date);
                cn.Open();
                object scalar = cmd.ExecuteScalar();
                openingBalance = scalar == null || scalar == DBNull.Value ? 0m : Convert.ToDecimal(scalar);
            }

            string movementSql = AccountClassifyCte + @"
SELECT
    MovementDate = CAST(E.entry_date AS DATE),
    Movement = SUM(ISNULL(E.debit,0) - ISNULL(E.credit,0))
FROM acc_entries E
INNER JOIN AccountClassify C ON C.account_id = E.account_id
WHERE E.branch_id = @branch_id
  AND C.account_class = 'CashBank'
  AND E.entry_date >= @from_date
  AND E.entry_date < @to_exclusive
GROUP BY CAST(E.entry_date AS DATE)
ORDER BY MovementDate;";

            var dailyMovements = new Dictionary<DateTime, decimal>();
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(movementSql, cn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@from_date", fromDate.Date);
                cmd.Parameters.AddWithValue("@to_exclusive", toDate.Date.AddDays(1));

                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    DateTime date = Convert.ToDateTime(row["MovementDate"]).Date;
                    decimal movement = row["Movement"] == DBNull.Value ? 0m : Convert.ToDecimal(row["Movement"]);
                    dailyMovements[date] = movement;
                }
            }

            DataTable result = new DataTable();
            result.Columns.Add("PointDate", typeof(DateTime));
            result.Columns.Add("Balance", typeof(decimal));

            decimal running = openingBalance;
            DateTime cursor = fromDate.Date;
            DateTime weekAnchor = fromDate.Date;

            while (cursor <= toDate.Date)
            {
                decimal movement;
                if (dailyMovements.TryGetValue(cursor, out movement))
                {
                    running += movement;
                }

                if ((cursor - weekAnchor).TotalDays >= 6 || cursor == toDate.Date)
                {
                    DataRow row = result.NewRow();
                    row["PointDate"] = cursor;
                    row["Balance"] = running;
                    result.Rows.Add(row);
                    weekAnchor = cursor.AddDays(1);
                }

                cursor = cursor.AddDays(1);
            }

            return result;
        }

        public DataTable GetExpenseBreakdown(DateTime fromDate, DateTime toDate, int topN)
        {
            string sql = AccountClassifyCte + @"
SELECT TOP (@top_n)
    Category = ISNULL(A.name, 'Other Expense'),
    Amount = SUM(ISNULL(E.debit,0))
FROM acc_entries E
INNER JOIN AccountClassify C ON C.account_id = E.account_id
LEFT JOIN acc_accounts A ON A.id = E.account_id
WHERE E.branch_id = @branch_id
  AND E.entry_date >= @from_date
  AND E.entry_date < @to_exclusive
  AND C.account_class = 'Expense'
GROUP BY A.name
ORDER BY Amount DESC;";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@from_date", fromDate.Date);
                cmd.Parameters.AddWithValue("@to_exclusive", toDate.Date.AddDays(1));
                cmd.Parameters.AddWithValue("@top_n", topN);

                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetAttentionSummary(DateTime asOfDate)
        {
            DataTable result = new DataTable();
            result.Columns.Add("ItemKey", typeof(string));
            result.Columns.Add("ItemTitle", typeof(string));
            result.Columns.Add("ItemCount", typeof(int));
            result.Columns.Add("ItemAmount", typeof(decimal));
            result.Columns.Add("ActionText", typeof(string));

            DateTime monthStart = StartOfMonth(asOfDate);

            var overdueReceivable = GetOverdueReceivables(asOfDate);
            AddAttentionRow(result, "OverdueReceivables", "Overdue Receivables (>30 days)", overdueReceivable.Item1, overdueReceivable.Item2, "View");

            var overduePayable = GetOverduePayables(asOfDate);
            AddAttentionRow(result, "OverduePayables", "Overdue Payables (>30 days)", overduePayable.Item1, overduePayable.Item2, "View");

            int drafts = GetDraftVoucherCount();
            AddAttentionRow(result, "UnpostedJv", "Unposted Journal Vouchers", drafts, 0m, "Review");

            int unreconciledBanks = GetUnreconciledBankAccountsCount(monthStart, asOfDate);
            AddAttentionRow(result, "BankReconciliation", "Bank accounts not reconciled this month", unreconciledBanks, 0m, "Reconcile");

            int staleOpenPeriods = GetStaleOpenPeriodsCount(asOfDate);
            AddAttentionRow(result, "OpenPeriods", "Open periods older than 45 days", staleOpenPeriods, 0m, "Close Period");

            return result;
        }

        public DataTable GetUnreconciledBankAccounts(DateTime monthStart, DateTime asOfDate)
        {
            string sql = @"
SELECT A.id AS AccountId, A.name AS AccountName
FROM acc_accounts A
LEFT JOIN acc_bank_reconciliation_header H
    ON H.bank_account_id = A.id
   AND H.branch_id = @branch_id
   AND H.statement_date >= @month_start
   AND H.statement_date <= @as_of_date
WHERE (LOWER(ISNULL(A.name, '')) LIKE '%bank%')
  AND H.id IS NULL
ORDER BY A.name;";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@month_start", monthStart.Date);
                cmd.Parameters.AddWithValue("@as_of_date", asOfDate.Date);

                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetRecentJournalActivity(int topN)
        {
            string sql = @"
SELECT TOP (@top_n)
    [Date] = CAST(H.EntryDate AS DATE),
    VoucherNo = H.InvoiceNo,
    [Description] = ISNULL(H.Narration, ''),
    [Amount] = ABS(ISNULL(H.total_debit, 0) - ISNULL(H.total_credit, 0)),
    PostedBy = ISNULL(U.name, ISNULL(U.username, 'System')),
    ModuleSource = ISNULL(H.VoucherType, 'General'),
    [Status] = ISNULL(H.status, 'Draft')
FROM acc_entries_header H
LEFT JOIN pos_users U ON U.id = ISNULL(H.posted_by, H.user_id)
WHERE H.branch_id = @branch_id
ORDER BY H.EntryDate DESC, H.id DESC;";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@top_n", topN);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        private Tuple<int, decimal> GetOverdueReceivables(DateTime asOfDate)
        {
            string sql = AccountClassifyCte + @"
SELECT
    ItemCount = ISNULL(COUNT(1), 0),
    ItemAmount = ISNULL(SUM(B.balance), 0)
FROM
(
    SELECT
        E.invoice_no,
        MIN(CAST(E.entry_date AS DATE)) AS first_date,
        SUM(ISNULL(E.debit,0) - ISNULL(E.credit,0)) AS balance
    FROM acc_entries E
    INNER JOIN AccountClassify C ON C.account_id = E.account_id
    WHERE E.branch_id = @branch_id
      AND E.entry_date <= @as_of_date
      AND C.account_class = 'Receivable'
    GROUP BY E.invoice_no
    HAVING SUM(ISNULL(E.debit,0) - ISNULL(E.credit,0)) > 0
       AND DATEDIFF(DAY, MIN(CAST(E.entry_date AS DATE)), @as_of_date) > 30
) B;";

            return ExecuteCountAmount(sql, asOfDate);
        }

        private Tuple<int, decimal> GetOverduePayables(DateTime asOfDate)
        {
            string sql = AccountClassifyCte + @"
SELECT
    ItemCount = ISNULL(COUNT(1), 0),
    ItemAmount = ISNULL(SUM(B.balance), 0)
FROM
(
    SELECT
        E.invoice_no,
        MIN(CAST(E.entry_date AS DATE)) AS first_date,
        SUM(ISNULL(E.credit,0) - ISNULL(E.debit,0)) AS balance
    FROM acc_entries E
    INNER JOIN AccountClassify C ON C.account_id = E.account_id
    WHERE E.branch_id = @branch_id
      AND E.entry_date <= @as_of_date
      AND C.account_class = 'Payable'
    GROUP BY E.invoice_no
    HAVING SUM(ISNULL(E.credit,0) - ISNULL(E.debit,0)) > 0
       AND DATEDIFF(DAY, MIN(CAST(E.entry_date AS DATE)), @as_of_date) > 30
) B;";

            return ExecuteCountAmount(sql, asOfDate);
        }

        private int GetDraftVoucherCount()
        {
            const string sql = @"
SELECT COUNT(1)
FROM acc_entries_header
WHERE branch_id = @branch_id
  AND ISNULL(status, 'Draft') = 'Draft';";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cn.Open();
                object scalar = cmd.ExecuteScalar();
                return scalar == null || scalar == DBNull.Value ? 0 : Convert.ToInt32(scalar);
            }
        }

        private int GetUnreconciledBankAccountsCount(DateTime monthStart, DateTime asOfDate)
        {
            string sql = @"
SELECT COUNT(1)
FROM
(
    SELECT A.id
    FROM acc_accounts A
    LEFT JOIN acc_bank_reconciliation_header H
        ON H.bank_account_id = A.id
       AND H.branch_id = @branch_id
       AND H.statement_date >= @month_start
       AND H.statement_date <= @as_of_date
    WHERE LOWER(ISNULL(A.name, '')) LIKE '%bank%'
    GROUP BY A.id
    HAVING COUNT(H.id) = 0
) X;";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@month_start", monthStart.Date);
                cmd.Parameters.AddWithValue("@as_of_date", asOfDate.Date);
                cn.Open();
                object scalar = cmd.ExecuteScalar();
                return scalar == null || scalar == DBNull.Value ? 0 : Convert.ToInt32(scalar);
            }
        }

        private int GetStaleOpenPeriodsCount(DateTime asOfDate)
        {
            string sql = @"
SELECT COUNT(1)
FROM acc_financial_periods
WHERE status = 'Open'
  AND end_date < DATEADD(DAY, -45, @as_of_date);";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@as_of_date", asOfDate.Date);
                cn.Open();
                object scalar = cmd.ExecuteScalar();
                return scalar == null || scalar == DBNull.Value ? 0 : Convert.ToInt32(scalar);
            }
        }

        private Tuple<int, decimal> ExecuteCountAmount(string sql, DateTime asOfDate)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@as_of_date", asOfDate.Date);
                cn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int count = reader.IsDBNull(0) ? 0 : Convert.ToInt32(reader[0]);
                        decimal amount = reader.IsDBNull(1) ? 0m : Convert.ToDecimal(reader[1]);
                        return Tuple.Create(count, amount);
                    }
                }
            }

            return Tuple.Create(0, 0m);
        }

        private static void AddAttentionRow(DataTable table, string key, string title, int count, decimal amount, string actionText)
        {
            DataRow row = table.NewRow();
            row["ItemKey"] = key;
            row["ItemTitle"] = title;
            row["ItemCount"] = count;
            row["ItemAmount"] = amount;
            row["ActionText"] = actionText;
            table.Rows.Add(row);
        }
    }
}
