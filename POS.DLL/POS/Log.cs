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
    public static class Log
    {
        private static SqlCommand cmd;
        private static SqlDataAdapter da;

        public static int LogAction(string action, string details, int user, int branchId)
        {
            Int32 result = 0;
            try
            {
                string pcName = Environment.MachineName;
                string additionalInfo = $"OS: {Environment.OSVersion}";
                
                using (SqlConnection connection = new SqlConnection(dbConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("sp_LogAction", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@Action", action);
                    command.Parameters.AddWithValue("@Details", details);
                    command.Parameters.AddWithValue("@UserId", user);
                    command.Parameters.AddWithValue("@BranchId", branchId);
                    command.Parameters.AddWithValue("@PcName", pcName);
                    command.Parameters.AddWithValue("@AdditionalInfo", additionalInfo);
                    command.Parameters.AddWithValue("@OperationType", "1");
                    connection.Open();
                    //command.ExecuteNonQuery();
                    result = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch
            {
                throw;
            }
            return (int)result;
        }

        public static DataTable SearchRecordByDate(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        string query = "SELECT * FROM Logs WHERE BranchId = @branch_id AND CAST(Timestamp AS DATE) BETWEEN @fromDate AND @toDate ORDER BY id desc";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@fromDate", fromDate);
                        cmd.Parameters.AddWithValue("@toDate", toDate);

                        

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
        public static DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_LogAction", cn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.Parameters.AddWithValue("@BranchId", UsersModal.logged_in_branch_id);

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

        public static DataTable SearchRecordByLogID(int log_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        //cmd = new SqlCommand("SELECT id,first_name,last_name,email,vat_no FROM pos_Banks WHERE id = @id", cn);
                        cmd = new SqlCommand("sp_LogAction", cn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.AddWithValue("@OperationType", "4");
                        cmd.Parameters.AddWithValue("@Id", log_id);
                        cmd.Parameters.AddWithValue("@BranchId", UsersModal.logged_in_branch_id);

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

    }
}
