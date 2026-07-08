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

        public Tuple<DataTable, int, decimal> GetAccountLedger(int accId, DateTime from, DateTime to, int page, int pageSize)
        {
            try
            {
                int safePage = page <= 0 ? 1 : page;
                int safePageSize = pageSize <= 0 ? 50 : pageSize;

                decimal openingBalance = GetAccountBalanceBeforeDate(accId, from);
                int totalCount = GetLedgerCount(accId, from, to);
                DataTable entries = GetLedgerPage(accId, from, to, safePage, safePageSize);

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
SELECT A.acc_id,
       A.acc_code,
       A.acc_name,
       A.parent_group_id,
       A.account_type,
       A.normal_balance,
       ISNULL(A.opening_balance, 0) AS opening_balance,
       SUM(ISNULL(E.debit, 0)) AS total_debit,
       SUM(ISNULL(E.credit, 0)) AS total_credit,
       CASE WHEN A.normal_balance = 'Cr' THEN
                -(ISNULL(A.opening_balance, 0))
            ELSE
                ISNULL(A.opening_balance, 0)
       END + SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)) AS balance
FROM acc_accounts A
LEFT JOIN acc_entries E
       ON E.acc_id = A.acc_id
      AND E.entry_date <= @as_of_date
GROUP BY A.acc_id, A.acc_code, A.acc_name, A.parent_group_id, A.account_type, A.normal_balance, A.opening_balance
ORDER BY A.acc_code, A.acc_name;",
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
SELECT ISNULL(CASE WHEN A.normal_balance = 'Cr' THEN -ISNULL(A.opening_balance, 0) ELSE ISNULL(A.opening_balance, 0) END, 0)
     + ISNULL((SELECT SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0))
               FROM acc_entries E
               WHERE E.acc_id = A.acc_id
                 AND E.entry_date <= @as_of_date), 0)
FROM acc_accounts A
WHERE A.acc_id = @acc_id;",
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
                sql.AppendLine("SELECT A.acc_id,");
                sql.AppendLine("       ISNULL(CASE WHEN A.normal_balance = 'Cr' THEN -ISNULL(A.opening_balance, 0) ELSE ISNULL(A.opening_balance, 0) END, 0)");
                sql.AppendLine("     + ISNULL(SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)), 0) AS balance");
                sql.AppendLine("FROM acc_accounts A");
                sql.AppendLine("LEFT JOIN acc_entries E ON E.acc_id = A.acc_id AND E.entry_date <= @as_of_date");
                sql.AppendLine("WHERE A.acc_id IN (");
                for (int i = 0; i < distinctIds.Count; i++)
                {
                    if (i > 0)
                    {
                        sql.Append(",");
                    }
                    sql.Append("@id").Append(i);
                }
                sql.AppendLine(")");
                sql.AppendLine("GROUP BY A.acc_id, A.normal_balance, A.opening_balance;");

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
SELECT ISNULL(CASE WHEN A.normal_balance = 'Cr' THEN -ISNULL(A.opening_balance, 0) ELSE ISNULL(A.opening_balance, 0) END, 0)
     + ISNULL((SELECT SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0))
               FROM acc_entries E
               WHERE E.acc_id = A.acc_id
                 AND E.entry_date < @before_date), 0)
FROM acc_accounts A
WHERE A.acc_id = @acc_id;",
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@acc_id", accId);
                    cmd.Parameters.AddWithValue("@before_date", beforeDate.Date);
                });
        }

        private int GetLedgerCount(int accId, DateTime from, DateTime to)
        {
            return ExecuteScalar<int>(@"
SELECT COUNT(1)
FROM acc_entries
WHERE acc_id = @acc_id
  AND entry_date BETWEEN @from_date AND @to_date;",
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@acc_id", accId);
                    cmd.Parameters.AddWithValue("@from_date", from.Date);
                    cmd.Parameters.AddWithValue("@to_date", to.Date);
                });
        }

        private DataTable GetLedgerPage(int accId, DateTime from, DateTime to, int page, int pageSize)
        {
            return ExecuteDataTable(@"
WITH Ledger AS
(
    SELECT E.entry_id,
           E.voucher_id,
           E.voucher_no,
           E.entry_date,
           E.acc_id,
           A.acc_code,
           A.acc_name,
           E.debit,
           E.credit,
           E.narration,
           E.cost_center_id,
           E.ref_module,
           E.ref_id,
           E.period_id,
           E.created_by,
           E.created_at,
           COUNT(1) OVER() AS total_count,
           SUM(ISNULL(E.debit,0) - ISNULL(E.credit,0)) OVER (ORDER BY E.entry_date, E.entry_id ROWS UNBOUNDED PRECEDING) AS running_balance
    FROM acc_entries E
    INNER JOIN acc_accounts A ON A.acc_id = E.acc_id
    WHERE E.acc_id = @acc_id
      AND E.entry_date BETWEEN @from_date AND @to_date
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
                    cmd.Parameters.AddWithValue("@offset", (page - 1) * pageSize);
                    cmd.Parameters.AddWithValue("@fetch", pageSize);
                });
        }
    }
}
