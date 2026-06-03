using POS.Core;
using System;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    public class CurrencyDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("sp_currenciesCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OperationType", "5");
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

        public DataTable SearchRecordByCurrencyID(int currencyId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("SELECT id,code,name,symbol,exchange_rate,is_active,date_created FROM pos_currencies WHERE id = @id", cn);
                        cmd.Parameters.AddWithValue("@id", currencyId);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }

                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable SearchRecord(string condition)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("SELECT id,code,name,symbol,exchange_rate,is_active,date_created FROM pos_currencies WHERE code LIKE @keyword OR name LIKE @keyword OR symbol LIKE @keyword ORDER BY id DESC", cn);
                        cmd.Parameters.AddWithValue("@keyword", string.Format("%{0}%", condition));
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                    }

                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int Insert(CurrencyModal obj)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("sp_currenciesCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@code", obj.code);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@symbol", obj.symbol ?? string.Empty);
                        cmd.Parameters.AddWithValue("@exchange_rate", obj.exchange_rate);
                        cmd.Parameters.AddWithValue("@is_active", obj.is_active);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "1");
                    }

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    Log.LogAction("Add Currency", $"Currency Code: {obj.code}, Currency Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int Update(CurrencyModal obj)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("sp_currenciesCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        cmd.Parameters.AddWithValue("@code", obj.code);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@symbol", obj.symbol ?? string.Empty);
                        cmd.Parameters.AddWithValue("@exchange_rate", obj.exchange_rate);
                        cmd.Parameters.AddWithValue("@is_active", obj.is_active);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "2");
                    }

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    Log.LogAction("Update Currency", $"Currency Code: {obj.code}, Currency Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int Delete(int currencyId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("sp_currenciesCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", currencyId);
                        cmd.Parameters.AddWithValue("@OperationType", "3");
                    }

                    int result = cmd.ExecuteNonQuery();
                    Log.LogAction("Delete Currency", $"Currency ID: {currencyId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
