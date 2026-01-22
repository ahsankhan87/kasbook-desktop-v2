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
    public class SupplierDLL
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

                        cmd = new SqlCommand("sp_SuppliersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OperationType", "5");
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

        public DataTable SearchRecordBySupplierID(int Supplier_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("sp_SuppliersCrud", cn); 
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd = new SqlCommand("SELECT id,first_name,last_name,email,vat_no FROM pos_suppliers WHERE id = @id", cn);
                        cmd.Parameters.AddWithValue("@OperationType", "4");
                        cmd.Parameters.AddWithValue("@id", Supplier_id);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

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

        public DataTable GetSupplierAccountBalance(int Supplier_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand();
                        string query = "SELECT SUM(debit-credit) AS balance FROM pos_suppliers_payments WHERE branch_id=@branch_id AND supplier_id = @id";
                        cmd.Parameters.AddWithValue("@id", Supplier_id);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        cmd.CommandText = query;
                        cmd.Connection = cn;
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
            DataTable dt = new DataTable(); // Instantiate DataTable

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = @"SELECT id, first_name, last_name, email, vat_no, address, date_created, branch_id 
                                FROM pos_suppliers 
                                WHERE branch_id = @branch_id 
                                AND (first_name LIKE @condition OR last_name LIKE @condition 
                                OR vat_no LIKE @condition OR address LIKE @condition)";

                    // Add parameters with precise type and value to prevent SQL injection
                    cmd.Parameters.Add("@condition", SqlDbType.NVarChar).Value = $"%{condition}%";
                    cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception as necessary
                        throw new Exception("Error: " + ex.Message);
                    }
                }
            }

            return dt;
        }

        public int Insert(SupplierModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_SuppliersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@first_name", obj.first_name);
                        cmd.Parameters.AddWithValue("@last_name", obj.last_name);
                        cmd.Parameters.AddWithValue("@email", obj.email);
                        cmd.Parameters.AddWithValue("@address", obj.address);
                        cmd.Parameters.AddWithValue("@status", 1);
                        cmd.Parameters.AddWithValue("@vat_status", obj.vat_with_status);
                        cmd.Parameters.AddWithValue("@contact_no", obj.contact_no);
                        cmd.Parameters.AddWithValue("@vat_no", obj.vat_no);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "1");
                        
                        cmd.Parameters.Add("@StreetName", SqlDbType.NVarChar).Value = obj.StreetName;
                        cmd.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = obj.PostalCode;
                        cmd.Parameters.Add("@BuildingNumber", SqlDbType.NVarChar).Value = obj.BuildingNumber;
                        cmd.Parameters.Add("@CitySubdivisionName", SqlDbType.NVarChar).Value = obj.CitySubdivisionName;
                        cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = obj.CityName;
                        cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = obj.CountryName;
                        cmd.Parameters.Add("@GLAccountID", SqlDbType.Int).Value = obj.GLAccountID;

                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    Log.LogAction("Add Supplier", $"Supplier ID: {result}, Supplier Name: {obj.first_name + ' ' + obj.last_name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int Update(SupplierModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_SuppliersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@branch_id", 0);
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        cmd.Parameters.AddWithValue("@first_name", obj.first_name);
                        cmd.Parameters.AddWithValue("@last_name", obj.last_name);
                        cmd.Parameters.AddWithValue("@email", obj.email);
                        cmd.Parameters.AddWithValue("@address", obj.address);
                        cmd.Parameters.AddWithValue("@status", 1);
                        cmd.Parameters.AddWithValue("@vat_status", obj.vat_with_status);
                        cmd.Parameters.AddWithValue("@contact_no", obj.contact_no);
                        cmd.Parameters.AddWithValue("@vat_no", obj.vat_no);
                        cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "2");
                        
                        cmd.Parameters.Add("@StreetName", SqlDbType.NVarChar).Value = obj.StreetName;
                        cmd.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = obj.PostalCode;
                        cmd.Parameters.Add("@BuildingNumber", SqlDbType.NVarChar).Value = obj.BuildingNumber;
                        cmd.Parameters.Add("@CitySubdivisionName", SqlDbType.NVarChar).Value = obj.CitySubdivisionName;
                        cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = obj.CityName;
                        cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = obj.CountryName;
                        cmd.Parameters.Add("@GLAccountID", SqlDbType.Int).Value = obj.GLAccountID;

                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    Log.LogAction("Update Supplier", $"Supplier ID: {obj.id}, Supplier Name: {obj.first_name + ' ' + obj.last_name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)result;
                }
                catch
                {

                    throw;
                }
                
            }
        }

        public int Delete(int SupplierId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    int result=0;
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                    }
                    using (SqlCommand cmd = new SqlCommand("sp_SuppliersCrud", cn))
                    {
                        //cmd = new SqlCommand("sp_SuppliersCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", SupplierId);
                        cmd.Parameters.AddWithValue("@OperationType", "3");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        result = cmd.ExecuteNonQuery();
                    }
                    
                    Log.LogAction("Delete Supplier", $"Customer ID: {SupplierId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

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
