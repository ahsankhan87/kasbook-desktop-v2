using System;
using System.Data;
using System.Data.SqlClient;
using POS.Core;

namespace POS.DLL
{
    public class BackupDLL
    {
        public bool HasBackupForDate(DateTime date)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT COUNT(1)
                FROM pos_BackupDetails
                WHERE CAST(CreationDate AS date) = @date", cn))
            {
                cmd.Parameters.Add("@date", SqlDbType.Date).Value = date.Date;
                cn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public bool HasBackupSince(DateTime fromDate)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"
                SELECT COUNT(1)
                FROM pos_BackupDetails
                WHERE CreationDate >= @fromDate", cn))
            {
                cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                cn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }
    }
}
