using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.DLL
{
    /// <summary>
    /// Data Access Layer for Moving Items Report.
    /// Retrieves items with recent sales activity from the database.
    /// </summary>
    public class MovingItemsReportDLL
    {
        private const string ConnectionString = null; // Uses dbConnection.ConnectionString

        public DataTable GetMovingItems(
            int branchId,
            int daysThreshold = 30,
            decimal minQtyOnHand = 0,
            string categoryCode = null,
            string brandCode = null,
            string locationCode = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetMovingItems", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;

                cmd.Parameters.AddWithValue("@BranchId", branchId);
                cmd.Parameters.AddWithValue("@DaysThreshold", daysThreshold);
                cmd.Parameters.AddWithValue("@MinQtyOnHand", minQtyOnHand);
                cmd.Parameters.AddWithValue("@CategoryCode", (object)categoryCode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@BrandCode", (object)brandCode ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LocationCode", (object)locationCode ?? DBNull.Value);

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
                    throw new Exception("Error fetching moving items: " + ex.Message, ex);
                }
                return dt;
            }
        }

        public DataTable GetMovingItemsSummary(int branchId, int daysThreshold = 30)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetMovingItemsSummary", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 60;

                cmd.Parameters.AddWithValue("@BranchId", branchId);
                cmd.Parameters.AddWithValue("@DaysThreshold", daysThreshold);

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
                    throw new Exception("Error fetching moving items summary: " + ex.Message, ex);
                }
                return dt;
            }
        }
    }
}
