using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace POS.DLL
{
    public class LedgerDAL : AccountingDalBase
    {
        public LedgerDAL()
        {
        }

        public LedgerDAL(string connectionString)
            : base(connectionString)
        {
        }

        public DataTable GetLedgerAccounts()
        {
            try
            {
                return ExecuteDataTable(@"
                SELECT A.id AS acc_id,
                       A.code AS acc_code,
                       A.name AS acc_name,
                       '' AS account_type,
                       CASE WHEN ISNULL(A.op_cr_balance, 0) > ISNULL(A.op_dr_balance, 0) THEN 'Cr' ELSE 'Dr' END AS normal_balance,
                       ISNULL(A.op_dr_balance, 0) - ISNULL(A.op_cr_balance, 0)
                       + ISNULL((SELECT SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0))
                                 FROM acc_entries E
                                 WHERE E.account_id = A.id), 0) AS current_balance
                FROM acc_accounts A
                WHERE ISNULL(A.is_active, 1) = 1
                ORDER BY A.code, A.name;");
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load ledger accounts.", ex);
            }
        }

        public DataTable GetAccountLedgerSummary(int accId, DateTime from, DateTime to)
        {
            try
            {
                return ExecuteDataTable(@"
SELECT TOP 1
       A.id AS acc_id,
       A.code AS acc_code,
       A.name AS acc_name,
       '' AS account_type,
       CASE WHEN ISNULL(A.op_cr_balance, 0) > ISNULL(A.op_dr_balance, 0) THEN 'Cr' ELSE 'Dr' END AS normal_balance,
       ISNULL(A.op_dr_balance, 0) - ISNULL(A.op_cr_balance, 0)
       + ISNULL((SELECT SUM(ISNULL(E0.debit, 0) - ISNULL(E0.credit, 0))
                 FROM acc_entries E0
                 WHERE E0.account_id = A.id), 0) AS current_balance,
       ISNULL((SELECT SUM(ISNULL(E1.debit, 0))
               FROM acc_entries E1
               WHERE E1.account_id = A.id
                 AND E1.entry_date BETWEEN @from_date AND @to_date), 0) AS period_debit,
       ISNULL((SELECT SUM(ISNULL(E2.credit, 0))
               FROM acc_entries E2
               WHERE E2.account_id = A.id
                 AND E2.entry_date BETWEEN @from_date AND @to_date), 0) AS period_credit,
       ISNULL((SELECT COUNT(1)
               FROM acc_entries E3
               WHERE E3.account_id = A.id
                 AND E3.entry_date BETWEEN @from_date AND @to_date), 0) AS period_count,
       (SELECT MAX(E4.entry_date)
        FROM acc_entries E4
        WHERE E4.account_id = A.id) AS last_txn_date
FROM acc_accounts A
WHERE A.id = @acc_id;",
                    cmd =>
                    {
                        cmd.Parameters.AddWithValue("@acc_id", accId);
                        cmd.Parameters.AddWithValue("@from_date", from.Date);
                        cmd.Parameters.AddWithValue("@to_date", to.Date);
                    });
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load ledger summary.", ex);
            }
        }

        public Tuple<DataTable, int, decimal> GetAccountLedger(int accId, DateTime from, DateTime to, int page, int pageSize)
        {
            return GetAccountLedger(accId, from, to, page, pageSize, "All");
        }

        public Tuple<DataTable, int, decimal> GetAccountLedger(int accId, DateTime from, DateTime to, int page, int pageSize, string showFilter)
        {
            try
            {
                int safePage = page <= 0 ? 1 : page;
                int safePageSize = pageSize <= 0 ? 50 : pageSize;
                string filter = string.IsNullOrWhiteSpace(showFilter) ? "All" : showFilter.Trim();

                decimal openingBalance = GetAccountBalanceBeforeDate(accId, from);
                int totalCount = GetLedgerCount(accId, from, to, filter);
                DataTable entries = GetLedgerPage(accId, from, to, safePage, safePageSize, filter, openingBalance);

                return Tuple.Create(entries, totalCount, openingBalance);
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load account ledger.", ex);
            }
        }

        public DataTable GetTrialBalance(DateTime asOfDate)
        {
            try
            {
                return ExecuteDataTable(@"
SELECT A.id AS acc_id,
       A.code AS acc_code,
       A.name AS acc_name,
       A.group_id AS parent_group_id,
       '' AS account_type,
       CASE WHEN ISNULL(A.op_cr_balance, 0) > ISNULL(A.op_dr_balance, 0) THEN 'Cr' ELSE 'Dr' END AS normal_balance,
       ISNULL(A.op_dr_balance, 0) - ISNULL(A.op_cr_balance, 0) AS opening_balance,
       SUM(ISNULL(E.debit, 0)) AS total_debit,
       SUM(ISNULL(E.credit, 0)) AS total_credit,
       ISNULL(A.op_dr_balance, 0) - ISNULL(A.op_cr_balance, 0)
       + SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)) AS balance
FROM acc_accounts A
LEFT JOIN acc_entries E
       ON E.account_id = A.id
      AND E.entry_date <= @as_of_date
GROUP BY A.id, A.code, A.name, A.group_id, A.op_dr_balance, A.op_cr_balance
ORDER BY A.code, A.name;",
                    cmd => cmd.Parameters.AddWithValue("@as_of_date", asOfDate.Date));
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load trial balance.", ex);
            }
        }

        public decimal GetAccountBalance(int accId, DateTime? asOfDate)
        {
            try
            {
                DateTime date = (asOfDate ?? DateTime.Today).Date;
                return ExecuteScalar<decimal>(@"
SELECT ISNULL(A.op_dr_balance, 0) - ISNULL(A.op_cr_balance, 0)
     + ISNULL((SELECT SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0))
               FROM acc_entries E
               WHERE E.account_id = A.id
                 AND E.entry_date <= @as_of_date), 0)
FROM acc_accounts A
WHERE A.id = @acc_id;",
                    cmd =>
                    {
                        cmd.Parameters.AddWithValue("@acc_id", accId);
                        cmd.Parameters.AddWithValue("@as_of_date", date);
                    });
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load account balance.", ex);
            }
        }

        public Dictionary<int, decimal> GetMultipleBalances(int[] accIds, DateTime asOfDate)
        {
            try
            {
                Dictionary<int, decimal> balances = new Dictionary<int, decimal>();
                if (accIds == null || accIds.Length == 0)
                {
                    return balances;
                }

                List<int> distinctIds = new List<int>(new HashSet<int>(accIds));
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT A.id AS acc_id,");
                sql.AppendLine("       ISNULL(A.op_dr_balance, 0) - ISNULL(A.op_cr_balance, 0)");
                sql.AppendLine("     + ISNULL(SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)), 0) AS balance");
                sql.AppendLine("FROM acc_accounts A");
                sql.AppendLine("LEFT JOIN acc_entries E ON E.account_id = A.id AND E.entry_date <= @as_of_date");
                sql.AppendLine("WHERE A.id IN (");
                for (int i = 0; i < distinctIds.Count; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(",");
                    }
                    sql.Append("@id").Append(i);
                }
                sql.AppendLine(")");
                sql.AppendLine("GROUP BY A.id, A.op_dr_balance, A.op_cr_balance;");

                using (SqlConnection cn = CreateConnection())
                using (SqlCommand cmd = new SqlCommand(sql.ToString(), cn))
                {
                    for (int i = 0; i < distinctIds.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("@id" + i, distinctIds[i]);
                    }
                    cmd.Parameters.AddWithValue("@as_of_date", asOfDate.Date);

                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int accId = Convert.ToInt32(reader["acc_id"]);
                            decimal balance = reader["balance"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["balance"]);
                            balances[accId] = balance;
                        }
                    }
                }

                for (int i = 0; i < distinctIds.Count; i++)
                {
                    if (!balances.ContainsKey(distinctIds[i]))
                    {
                        balances[distinctIds[i]] = 0m;
                    }
                }

                return balances;
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load multiple account balances.", ex);
            }
        }

        private decimal GetAccountBalanceBeforeDate(int accId, DateTime beforeDate)
        {
            return ExecuteScalar<decimal>(@"
SELECT ISNULL(A.op_dr_balance, 0) - ISNULL(A.op_cr_balance, 0)
     + ISNULL((SELECT SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0))
               FROM acc_entries E
               WHERE E.account_id = A.id
                 AND E.entry_date < @before_date), 0)
FROM acc_accounts A
WHERE A.id = @acc_id;",
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@acc_id", accId);
                    cmd.Parameters.AddWithValue("@before_date", beforeDate.Date);
                });
        }

        private int GetLedgerCount(int accId, DateTime from, DateTime to, string showFilter)
        {
            return ExecuteScalar<int>(@"
SELECT COUNT(1)
FROM acc_entries E
LEFT JOIN acc_entries_header H ON H.InvoiceNo = E.invoice_no
WHERE E.account_id = @acc_id
  AND E.entry_date BETWEEN @from_date AND @to_date
  AND (
        @show_filter = 'All'
        OR (@show_filter = 'Debits' AND ISNULL(E.debit, 0) > 0)
        OR (@show_filter = 'Credits' AND ISNULL(E.credit, 0) > 0)
        OR (@show_filter = 'Unposted' AND ISNULL(H.status, 'Draft') <> 'Posted')
      );",
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@acc_id", accId);
                    cmd.Parameters.AddWithValue("@from_date", from.Date);
                    cmd.Parameters.AddWithValue("@to_date", to.Date);
                    cmd.Parameters.AddWithValue("@show_filter", showFilter);
                });
        }

        private DataTable GetLedgerPage(int accId, DateTime from, DateTime to, int page, int pageSize, string showFilter, decimal openingBalance)
        {
            return ExecuteDataTable(@"
WITH Ledger AS
(
    SELECT E.id AS entry_id,
           E.id AS voucher_id,
           E.invoice_no AS voucher_no,
           E.entry_date,
           E.account_id AS acc_id,
           A.code AS acc_code,
           A.name AS acc_name,
           ISNULL(E.debit, 0) AS debit,
           ISNULL(E.credit, 0) AS credit,
           ISNULL(E.description, '') AS narration,
           E.cost_center_id,
           E.ref_module,
           E.ref_id,
           E.period_id,
           E.user_id AS created_by,
           E.date_created AS created_at,
           ISNULL(H.VoucherType, '') AS voucher_type,
           ISNULL(H.status, 'Posted') AS status,
           @opening_balance + SUM(ISNULL(E.debit,0) - ISNULL(E.credit,0)) OVER (ORDER BY E.entry_date, E.id ROWS UNBOUNDED PRECEDING) AS running_balance
    FROM acc_entries E
    INNER JOIN acc_accounts A ON A.id = E.account_id
    LEFT JOIN acc_entries_header H ON H.InvoiceNo = E.invoice_no
    WHERE E.account_id = @acc_id
      AND E.entry_date BETWEEN @from_date AND @to_date
      AND (
            @show_filter = 'All'
            OR (@show_filter = 'Debits' AND ISNULL(E.debit, 0) > 0)
            OR (@show_filter = 'Credits' AND ISNULL(E.credit, 0) > 0)
            OR (@show_filter = 'Unposted' AND ISNULL(H.status, 'Draft') <> 'Posted')
          )
)
SELECT *
FROM Ledger
ORDER BY entry_date, entry_id
OFFSET @offset ROWS FETCH NEXT @fetch ROWS ONLY;",
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@acc_id", accId);
                    cmd.Parameters.AddWithValue("@from_date", from.Date);
                    cmd.Parameters.AddWithValue("@to_date", to.Date);
                    cmd.Parameters.AddWithValue("@show_filter", showFilter);
                    cmd.Parameters.AddWithValue("@opening_balance", openingBalance);
                    cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
                    cmd.Parameters.AddWithValue("@fetch", pageSize);
                });
        }
    }
}
