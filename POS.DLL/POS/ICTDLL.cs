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
    public class ICTDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        
        public DataTable GetAll_ict_transfer_transactions()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT ict.*,B.name as source_branch, B_1.name as receiving_branch," +
                            " IIF(ict.status > 0,1,0) as transfer_status" +
                            " FROM pos_inter_company_transfer ict" +
                            " LEFT JOIN pos_branches B ON B.id=ict.source_branch_id" +
                            " LEFT JOIN pos_branches B_1 ON B_1.id=ict.destination_branch_id" +
                            " where (ict.destination_branch_id = @branch_id OR ict.source_branch_id = @branch_id)";


                        cmd = new SqlCommand(query, cn);
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
        public DataTable GetAll_ict_releases()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT ict.*,B.name as source_branch, B_1.name as receiving_branch" +
                            " FROM pos_inter_company_transfer ict" +
                            " LEFT JOIN pos_branches B ON B.id=ict.source_branch_id" +
                            " LEFT JOIN pos_branches B_1 ON B_1.id=ict.destination_branch_id" +
                            " where ict.destination_branch_id = @branch_id AND ict.released_qty < ict.requested_qty AND ict.status=0";


                        cmd = new SqlCommand(query, cn);
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
        public DataTable GetAll_ict_requests()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT ict.*,B.name as source_branch, B_1.name as receiving_branch," +
                            " IIF(ict.released_qty>0,1,0) as release_status, ict.released_qty as qty_transferred" +
                            " FROM pos_inter_company_transfer ict" +
                            " LEFT JOIN pos_branches B ON B.id=ict.source_branch_id" +
                            " LEFT JOIN pos_branches B_1 ON B_1.id=ict.destination_branch_id" +
                            " where ict.source_branch_id = @branch_id AND ict.status=0";


                        cmd = new SqlCommand(query, cn);
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

        public int save_ict_release_qty(List<ICTModal> ict_detail)
        {
            Int32 newSaleID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction;

                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();

                    try
                    {
                        foreach (ICTModal detail in ict_detail)
                        {
                            cmd = new SqlCommand("sp_ict_transfer", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@id", detail.id);
                            cmd.Parameters.AddWithValue("@source_branch_id", detail.source_branch_id);
                            cmd.Parameters.AddWithValue("@destination_branch_id", detail.destination_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@item_code", detail.item_code);
                            cmd.Parameters.AddWithValue("@item_number", detail.item_number);
                            cmd.Parameters.AddWithValue("@quantity_released", detail.quantity);
                            cmd.Parameters.AddWithValue("@release_date", detail.release_date);
                            //cmd.Parameters.AddWithValue("@status", detail.status);

                            cmd.Parameters.AddWithValue("@OperationType", "3");

                            newSaleID = Convert.ToInt32(cmd.ExecuteScalar());

                            Log.LogAction("ICT Release", $"Source Branch Id: {detail.source_branch_id}, Destination Branch Id: {detail.destination_branch_id}, Item Code={detail.item_code}, Quantity Released={detail.quantity}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }


                return newSaleID;

            }
        }

        public int save_ict_transfer_qty(List<ICTModal> ict_detail)
        {
            Int32 newSaleID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction;

                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();

                    try
                    {
                        foreach (ICTModal detail in ict_detail)
                        {
                            cmd = new SqlCommand("sp_ict_transfer", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@id", detail.id);
                            cmd.Parameters.AddWithValue("@source_branch_id", detail.source_branch_id);
                            cmd.Parameters.AddWithValue("@destination_branch_id", detail.destination_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@item_code", detail.item_code);
                            cmd.Parameters.AddWithValue("@item_number", detail.item_number);
                            cmd.Parameters.AddWithValue("@quantity_transferred", detail.quantity);
                            cmd.Parameters.AddWithValue("@transfer_date", detail.transfer_date);
                            cmd.Parameters.AddWithValue("@status", detail.status);

                            cmd.Parameters.AddWithValue("@OperationType", "2");

                            newSaleID = Convert.ToInt32(cmd.ExecuteScalar());

                            Log.LogAction("ICT Transfer", $"Source Branch Id: {detail.source_branch_id}, Destination Branch Id: {detail.destination_branch_id}, Item Code={detail.item_code}, Quantity Transferred={detail.quantity}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }


                return newSaleID;

            }
        }
    }
}
