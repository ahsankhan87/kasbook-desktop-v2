using System;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    public class PeriodDAL : AccountingDalBase
    {
        public PeriodDAL()
        {
        }

        public PeriodDAL(string connectionString)
            : base(connectionString)
        {
        }

        public DataTable GetAllPeriods(int yearId)
        {
            try
            {
                return ExecuteDataTable(@"
SELECT period_id, year_id, period_name, start_date, end_date, status, closed_by, closed_at, transaction_count
FROM acc_financial_periods
WHERE year_id = @year_id
ORDER BY start_date;",
                    cmd => cmd.Parameters.AddWithValue("@year_id", yearId));
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load financial periods.", ex);
            }
        }

        public bool IsPeriodLocked(DateTime date)
        {
            try
            {
                string status = ExecuteScalar<string>(@"
SELECT TOP 1 status
FROM acc_financial_periods
WHERE @dt BETWEEN start_date AND end_date
ORDER BY start_date DESC;",
                    cmd => cmd.Parameters.AddWithValue("@dt", date.Date));

                if (string.IsNullOrWhiteSpace(status))
                {
                    return true;
                }

                return !string.Equals(status, "Open", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to check period lock state.", ex);
            }
        }

        public int GetCurrentPeriodId(DateTime date)
        {
            try
            {
                return ExecuteScalar<int>(@"
SELECT TOP 1 period_id
FROM acc_financial_periods
WHERE @dt BETWEEN start_date AND end_date
ORDER BY start_date DESC;",
                    cmd => cmd.Parameters.AddWithValue("@dt", date.Date));
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to resolve current period.", ex);
            }
        }

        public int UpdatePeriodStatus(int periodId, string status, int userId)
        {
            try
            {
                using (SqlConnection cn = CreateConnection())
                using (SqlCommand cmd = new SqlCommand(@"
UPDATE acc_financial_periods
SET status = @status,
    closed_by = CASE WHEN @status = 'Open' THEN NULL ELSE @user_id END,
    closed_at = CASE WHEN @status = 'Open' THEN NULL ELSE @closed_at END
WHERE period_id = @period_id;", cn))
                {
                    cmd.Parameters.AddWithValue("@period_id", periodId);
                    cmd.Parameters.AddWithValue("@status", string.IsNullOrWhiteSpace(status) ? "Open" : status.Trim());
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    cmd.Parameters.AddWithValue("@closed_at", DateTime.Now);
                    cn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to update financial period status.", ex);
            }
        }
    }
}
