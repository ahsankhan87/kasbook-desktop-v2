using System;
using System.Data;
using System.Data.SqlClient;
using POS.Core;

namespace POS.DLL.Dashboard
{
    public class DashboardSalesAmounts
    {
        public decimal TodayAmount { get; set; }
        public decimal MonthlyAmount { get; set; }
    }

    public class DashboardDLL
    {
        public DashboardSalesAmounts GetSalesAmounts(int branchId, DateTime today, DateTime monthStart, DateTime nextMonthStart)
        {
            var result = new DashboardSalesAmounts();

            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                // Net amount = (total_amount + total_tax - discount_value), returns subtracted (account = 'Return')
                cmd.CommandText = @"
                    SELECT
                        TodayAmount = SUM(
                            CASE 
                                WHEN CAST(S.sale_date AS date) = @TodayDate THEN
                                    (CASE WHEN ISNULL(S.account,'') = 'Return' THEN -1 ELSE 1 END) *
                                    (COALESCE(S.total_amount,0) + COALESCE(S.total_tax,0) - COALESCE(S.discount_value,0))
                                ELSE 0
                            END
                        ),
                        MonthlyAmount = SUM(
                            CASE 
                                WHEN S.sale_date >= @MonthStart AND S.sale_date < @NextMonthStart THEN
                                    (CASE WHEN ISNULL(S.account,'') = 'Return' THEN -1 ELSE 1 END) *
                                    (COALESCE(S.total_amount,0) + COALESCE(S.total_tax,0) - COALESCE(S.discount_value,0))
                                ELSE 0
                            END
                        )
                    FROM pos_sales S
                    WHERE S.branch_id = @BranchId;";

                cmd.Parameters.AddWithValue("@BranchId", branchId);
                cmd.Parameters.AddWithValue("@TodayDate", today.Date);
                cmd.Parameters.AddWithValue("@MonthStart", monthStart);
                cmd.Parameters.AddWithValue("@NextMonthStart", nextMonthStart);

                if (cn.State != ConnectionState.Open) cn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        result.TodayAmount = rdr.IsDBNull(0) ? 0m : Convert.ToDecimal(rdr[0]);
                        result.MonthlyAmount = rdr.IsDBNull(1) ? 0m : Convert.ToDecimal(rdr[1]);
                    }
                }
            }

            return result;
        }

        public DataTable GetRecentLogs(int branchId, int topN)
        {
            var dt = new DataTable();
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand())
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"
                    SELECT TOP (@TopN) [Action], [Details], [Timestamp]
                    FROM Logs
                    WHERE BranchId = @BranchId
                    ORDER BY id DESC;";
                cmd.Parameters.AddWithValue("@TopN", topN);
                cmd.Parameters.AddWithValue("@BranchId", branchId);

                da.Fill(dt);
            }
            return dt;
        }
    }
}