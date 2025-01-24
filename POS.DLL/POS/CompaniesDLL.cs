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
    public class CompaniesDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private CompaniesModal info = new CompaniesModal();

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_CompaniesCrud", cn);
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

        public DataTable SearchRecordByCompaniesID(int Companies_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT * FROM pos_companies WHERE id = @id", cn);
                        cmd.Parameters.AddWithValue("@id", Companies_id);
                        
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

        public DataTable GetCompany()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT TOP 1 * FROM pos_companies", cn);
                        
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

                        cmd = new SqlCommand("SELECT * FROM pos_companies WHERE name LIKE @name", cn);
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

        public int Insert(CompaniesModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_CompaniesCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", 0);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@address", obj.address);
                        cmd.Parameters.AddWithValue("@vat_no", obj.vat_no);
                        cmd.Parameters.AddWithValue("@email", obj.email);
                        cmd.Parameters.AddWithValue("@contact_no", obj.contact_no);
                        cmd.Parameters.AddWithValue("@image", obj.image);
                        //cmd.Parameters.AddWithValue("@currency_id", obj.name);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "1");

                        cmd.Parameters.AddWithValue("@cash_acc_id", obj.cash_acc_id);
                        cmd.Parameters.AddWithValue("@sales_acc_id", obj.sales_acc_id);
                        cmd.Parameters.AddWithValue("@inventory_acc_id", obj.inventory_acc_id);
                        cmd.Parameters.AddWithValue("@sales_return_acc_id", obj.sales_return_acc_id);
                        cmd.Parameters.AddWithValue("@sales_discount_acc_id", obj.sales_discount_acc_id);
                        cmd.Parameters.AddWithValue("@tax_acc_id", obj.tax_acc_id);
                        cmd.Parameters.AddWithValue("@receivable_acc_id", obj.receivable_acc_id);
                        cmd.Parameters.AddWithValue("@payable_acc_id", obj.payable_acc_id);
                        cmd.Parameters.AddWithValue("@purchases_acc_id", obj.purchases_acc_id);
                        cmd.Parameters.AddWithValue("@purchases_return_acc_id", obj.purchases_return_acc_id);
                        cmd.Parameters.AddWithValue("@purchases_discount_acc_id", obj.purchases_discount_acc_id);
                        cmd.Parameters.AddWithValue("@item_variance_acc_id", obj.item_variance_acc_id);
                        cmd.Parameters.AddWithValue("@commission_acc_id", obj.commission_acc_id);
                        
                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    Log.LogAction("Add Company", $"Company id: {result}, Company Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int Update(CompaniesModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_CompaniesCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        cmd.Parameters.AddWithValue("@branch_id", 0);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@address", obj.address);
                        cmd.Parameters.AddWithValue("@vat_no", obj.vat_no);
                        cmd.Parameters.AddWithValue("@email", obj.email);
                        cmd.Parameters.AddWithValue("@contact_no", obj.contact_no);
                        cmd.Parameters.AddWithValue("@image", obj.image);
                        //cmd.Parameters.AddWithValue("@currency_id", obj.name);
                        cmd.Parameters.AddWithValue("@user_id", obj.user_id);
                        cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "2");

                        cmd.Parameters.AddWithValue("@cash_acc_id", obj.cash_acc_id);
                        cmd.Parameters.AddWithValue("@sales_acc_id", obj.sales_acc_id);
                        cmd.Parameters.AddWithValue("@inventory_acc_id", obj.inventory_acc_id);
                        cmd.Parameters.AddWithValue("@sales_return_acc_id", obj.sales_return_acc_id);
                        cmd.Parameters.AddWithValue("@sales_discount_acc_id", obj.sales_discount_acc_id);
                        cmd.Parameters.AddWithValue("@tax_acc_id", obj.tax_acc_id);
                        cmd.Parameters.AddWithValue("@receivable_acc_id", obj.receivable_acc_id);
                        cmd.Parameters.AddWithValue("@payable_acc_id", obj.payable_acc_id);
                        cmd.Parameters.AddWithValue("@purchases_acc_id", obj.purchases_acc_id);
                        cmd.Parameters.AddWithValue("@purchases_return_acc_id", obj.purchases_return_acc_id);
                        cmd.Parameters.AddWithValue("@purchases_discount_acc_id", obj.purchases_discount_acc_id);
                        cmd.Parameters.AddWithValue("@item_variance_acc_id", obj.item_variance_acc_id);
                        cmd.Parameters.AddWithValue("@commission_acc_id", obj.commission_acc_id);
                        cmd.Parameters.AddWithValue("@bank_acc_id", 0);
                        
                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());

                    Log.LogAction("Update Company", $"Company Code: {obj.id}, Company Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)result;
                }
                catch
                {

                    throw;
                }
                
            }
        }
        public int Register(CompaniesModal obj)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_CompaniesCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@address", obj.address);
                        cmd.Parameters.AddWithValue("@vat_no", obj.vat_no);
                        cmd.Parameters.AddWithValue("@email", obj.email);
                        cmd.Parameters.AddWithValue("@contact_no", obj.contact_no);
                        cmd.Parameters.AddWithValue("@subscriptionKey", obj.subscriptionKey);
                        cmd.Parameters.AddWithValue("@systemID", obj.systemID);
                        //cmd.Parameters.AddWithValue("@image", obj.image);
                        //cmd.Parameters.AddWithValue("@currency_id", obj.name);
                        //cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
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
                    Log.LogAction("Register Company", $"Company Code: {result}, Company Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)result;
                }
                catch
                {

                    throw;
                }
            }
        }
        public int Delete(int CompaniesId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_CompaniesCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", CompaniesId); 
                        cmd.Parameters.AddWithValue("@OperationType", "3");

                    }

                    int result = cmd.ExecuteNonQuery();
                    Log.LogAction("Delete Company", $"Company ID: {CompaniesId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int UpdateSubscriptionKeyToDatabase(int companyId, string key)
        {
            int affected = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (key != string.Empty)
                    {
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();
                            SqlCommand cmd = cn.CreateCommand();

                            string query = "UPDATE pos_companies SET subscriptionKey = @key, locked = @locked WHERE id = @id";
                            cmd.Parameters.AddWithValue("@id", companyId);
                            cmd.Parameters.AddWithValue("@key", key.Trim());
                            cmd.Parameters.AddWithValue("@locked", false);

                            cmd.CommandText = query;
                            affected = cmd.ExecuteNonQuery();

                            Log.LogAction("Renew Subscription Company", $"Company ID: {companyId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                            //if(affected > 0)
                            //{
                            //    UsersDLL user_obj = new UsersDLL();
                            //    user_obj.UpdateAppLocked(userId, 0);
                            //}

                        }
                    }
                    else
                    {
                        return affected;
                    }

                }
                catch 
                {
                    throw;

                }

            }
            return affected;
        }

        public int updateAppLock(int company_id, bool status)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "UPDATE pos_companies SET locked = @status" +
                            " WHERE id = @id";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@id", company_id);
                        cmd.Parameters.AddWithValue("@status", status);


                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());

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
