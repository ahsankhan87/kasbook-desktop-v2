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
    public class UsersDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private UsersModal info = new UsersModal();

        public int Login(UsersModal obj)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_UsersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@username", obj.username);
                        cmd.Parameters.AddWithValue("@password", obj.password);
                        cmd.Parameters.AddWithValue("@OperationType", "4");
                        
                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    
                }
                catch
                {

                    throw;
                }
            }
            return (int)result;
        }
        
        public bool IsUsernameExist(string username)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT username FROM pos_users WHERE username = @username", cn);
                        cmd.Parameters.AddWithValue("@username", username);

                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                    }

                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }
                    return false;

                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable GetUser(int user_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_UsersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.Parameters.AddWithValue("@id", user_id);

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

        public DataTable GetUserModules(int user_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Users_modules", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OperationType", "4");
                        cmd.Parameters.AddWithValue("@user_id", user_id);

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

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_UsersCrud", cn);
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

        public DataTable SearchRecordByUsersID(int Users_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT id,name,FROM pos_users WHERE id = @id", cn);
                        cmd.Parameters.AddWithValue("@id", Users_id);
                        
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

        public DataTable GetUserRights(int Users_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT * FROM pos_users_rights WHERE user_id = @id", cn);
                        cmd.Parameters.AddWithValue("@id", Users_id);

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

        public DataTable SearchRecord(String condition)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT id,name,username, date_created FROM pos_users WHERE name LIKE @name", cn);
                        //cmd.Parameters.AddWithValue("@id", condition);
                        cmd.Parameters.AddWithValue("@name", string.Format("%{0}%", condition));
                        
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

        public int Insert(UsersModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_UsersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", obj.branch_id);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@companyID", obj.companyID);
                        cmd.Parameters.AddWithValue("@language", obj.language);
                        cmd.Parameters.AddWithValue("@username", obj.username);
                        cmd.Parameters.AddWithValue("@password", obj.password);
                        cmd.Parameters.AddWithValue("@user_role", obj.user_role);
                        cmd.Parameters.AddWithValue("@user_level", obj.user_level);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@commission_percent", obj.user_commission_percent);
                        cmd.Parameters.AddWithValue("@OperationType", "1");
                        
                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    Log.LogAction("Add User", $"User ID: {result}, User Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    
                }
                catch
                {

                    throw;
                }
            }
            return (int)result;
        }

        public int InsertUserModules(UsersModal obj)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Users_modules", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@user_id", obj.user_id);
                        cmd.Parameters.AddWithValue("@module_name", obj.module_name);
                        cmd.Parameters.AddWithValue("@module_id", obj.module_id);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "1");

                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());

                    return (int)result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int Update(UsersModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_UsersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        cmd.Parameters.AddWithValue("@branch_id", obj.branch_id);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@language", obj.language);
                        cmd.Parameters.AddWithValue("@user_role", obj.user_role);
                        cmd.Parameters.AddWithValue("@user_level", obj.user_level);
                        cmd.Parameters.AddWithValue("@username", obj.username);
                        cmd.Parameters.AddWithValue("@commission_percent", obj.user_commission_percent);
                        cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "2");
                        
                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    Log.LogAction("Update User", $"User ID: {obj.id}, User Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)result;
                }
                catch
                {

                    throw;
                }
                
            }
        }

        public int UpdateUserRights(UsersModal obj)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Users_modules", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@user_id", obj.id);
                        cmd.Parameters.AddWithValue("@branch_id", obj.branch_id);
                        //cmd.Parameters.AddWithValue("@allow_cash_sales", obj.allow_cash_sales);
                        cmd.Parameters.AddWithValue("@allow_credit_sales", obj.allow_credit_sales);
                        //cmd.Parameters.AddWithValue("@allow_cash_purchase", obj.allow_cash_purchase);
                        cmd.Parameters.AddWithValue("@allow_credit_purchase", obj.allow_credit_purchase);
                        cmd.Parameters.AddWithValue("@credit_sales_amount", obj.credit_sales_amount_limit);
                        cmd.Parameters.AddWithValue("@cash_sales_amount", obj.cash_sales_amount_limit);
                        cmd.Parameters.AddWithValue("@credit_purchase_amount", obj.credit_purchase_amount_limit);
                        cmd.Parameters.AddWithValue("@cash_purchase_amount", obj.cash_purchase_amount_limit);
                        cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "5");

                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    Log.LogAction("Update User Rights", $"User ID: {obj.id}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)result;
                }
                catch
                {

                    throw;
                }

            }
        }

        public int UpdatePassword(UsersModal obj)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_UsersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        cmd.Parameters.AddWithValue("@password", obj.password);
                        cmd.Parameters.AddWithValue("@OperationType", "6");

                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    Log.LogAction("Update User Password", $"User ID: {obj.id}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)result;
                }
                catch
                {

                    throw;
                }

            }
        }

        public int Delete(int UsersId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_UsersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", UsersId); 
                        cmd.Parameters.AddWithValue("@OperationType", "3");

                    }

                    int result = cmd.ExecuteNonQuery();
                    Log.LogAction("Delete User", $"User ID: {UsersId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int DeleteUserModules(int UsersId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Users_modules", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@user_id", UsersId);
                        cmd.Parameters.AddWithValue("@OperationType", "3");

                    }

                    int result = cmd.ExecuteNonQuery();
                    Log.LogAction("Delete User Modules", $"User ID: {UsersId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int DeleteUserRights(int UsersId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Users_modules", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@user_id", UsersId);
                        cmd.Parameters.AddWithValue("@OperationType", "6");

                    }

                    int result = cmd.ExecuteNonQuery();
                    Log.LogAction("Delete User Rights", $"User ID: {UsersId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }


        public Int64 GetUserCommissionBalance(int user_id)
        {
            Int64 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand();
                        string query = "SELECT SUM(credit-debit) AS balance FROM pos_user_commission WHERE user_id = @id";
                        cmd.Parameters.AddWithValue("@id", user_id);

                        cmd.CommandText = query;
                        cmd.Connection = cn;
                    }
                    var output = cmd.ExecuteScalar();

                    if (!(output is DBNull))
                    {
                        result = Convert.ToInt32(output);
                    }

                    return (int)result;
                }
                catch
                {
                    throw;
                }
            }
        }


        public int InsertUserCommission(JournalsModal obj)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_JournalsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@account_id", obj.account_id);
                        cmd.Parameters.AddWithValue("@entry_date", obj.entry_date);
                        cmd.Parameters.AddWithValue("@debit", obj.debit);
                        cmd.Parameters.AddWithValue("@credit", obj.credit);
                        cmd.Parameters.AddWithValue("@description", obj.description);
                        cmd.Parameters.AddWithValue("@user_id", obj.user_id);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@entry_id", obj.entry_id);
                        cmd.Parameters.AddWithValue("@OperationType", "7");

                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    return (int)result;
                }
                catch
                {

                    throw;
                }
            }
        }

    }
}
