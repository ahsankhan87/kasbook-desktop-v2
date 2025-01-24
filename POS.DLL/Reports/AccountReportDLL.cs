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
    public static class AccountReportDLL
    {
        public static DataTable TrialBalanceReport(int branchId, DateTime StartDate, DateTime EndDate, int OperationType=1)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    // Create a new dataset
                    DataTable dataTable = new DataTable();

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        // SQL connection and command

                        using (SqlCommand cmd = new SqlCommand("sp_AccountReports", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@BranchID", branchId);
                            cmd.Parameters.AddWithValue("@StartDate", StartDate);
                            cmd.Parameters.AddWithValue("@EndDate", EndDate);
                           
                            cmd.Parameters.AddWithValue("@OperationType", OperationType);
                            //--operation types   
                            //-- 1) Trial Balance Report  

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            
                            adapter.Fill(dataTable);
                        }
                    }

                    return dataTable;
                }
                catch
                {
                    throw;
                }
            }

        }

        public static DataTable ProfitAndLossReport(int branchId, DateTime StartDate, DateTime EndDate, int OperationType = 2)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    // Create a new dataset
                    DataTable dataTable = new DataTable();

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        // SQL connection and command

                        using (SqlCommand cmd = new SqlCommand("sp_AccountReports", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@BranchID", branchId);
                            cmd.Parameters.AddWithValue("@StartDate", StartDate);
                            cmd.Parameters.AddWithValue("@EndDate", EndDate);

                            cmd.Parameters.AddWithValue("@OperationType", OperationType);
                            //--operation types   
                            //-- 1) Trial Balance Report  
                            //-- 2) Profit and Loss Report  

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                            adapter.Fill(dataTable);
                        }
                    }

                    return dataTable;
                }
                catch
                {
                    throw;
                }
            }

        }
        public static DataTable BalanceSheetReport(int branchId, DateTime StartDate, DateTime EndDate, int OperationType = 3)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    // Create a new dataset
                    DataTable dataTable = new DataTable();

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        // SQL connection and command

                        using (SqlCommand cmd = new SqlCommand("sp_AccountReports", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@BranchID", branchId);
                            cmd.Parameters.AddWithValue("@StartDate", StartDate);
                            cmd.Parameters.AddWithValue("@EndDate", EndDate);

                            cmd.Parameters.AddWithValue("@OperationType", OperationType);
                            //--operation types   
                            //-- 1) Trial Balance Report  
                            //-- 2) Profit and Loss Report  
                            //-- 2) Balance Report  

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                            adapter.Fill(dataTable);
                        }
                    }

                    return dataTable;
                }
                catch
                {
                    throw;
                }
            }

        }
    }
}
