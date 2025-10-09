using System;
using System.Data;
using System.Data.SqlClient;
using POS.Core;

namespace POS.DLL
{
    public class CustomerDLL
    {
        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_CustomersCrud", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OperationType", SqlDbType.VarChar).Value = "5";
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

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
                    // Log exception details here
                    throw new Exception("Error fetching all customers: " + ex.Message);
                }
                return dt;
            }
        }

        public DataTable SearchRecordByCustomerID(int customerId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_CustomersCrud", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@OperationType", SqlDbType.VarChar).Value = "4";
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = customerId;
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

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
                    // Log exception details here
                    throw new Exception("Error searching record by customer ID: " + ex.Message);
                }
                return dt;
            }
        }
        public DataTable SearchRecord(string condition)
        {
            DataTable dt = new DataTable(); // Instantiate DataTable

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = @"SELECT * 
                                FROM pos_customers 
                                WHERE branch_id = @branch_id 
                                AND (first_name LIKE @condition OR last_name LIKE @condition 
                                OR vat_no LIKE @condition OR address LIKE @condition OR contact_no LIKE @condition)";

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

        public decimal GetCustomerAccountBalance(int customerId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"
            SELECT ISNULL(SUM(CAST(debit AS decimal(18,4)) - CAST(credit AS decimal(18,4))), 0) AS balance 
            FROM pos_customers_payments 
            WHERE customer_id = @id AND branch_id = @branch_id", cn))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = customerId;
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

                try
                {
                    cn.Open();
                    object scalar = cmd.ExecuteScalar();

                    decimal balance = 0m;
                    if (scalar != null && scalar != DBNull.Value)
                    {
                        balance = Convert.ToDecimal(scalar);
                    }

                    return Math.Round(balance, 2);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching customer account balance: " + ex.Message, ex);
                }
            }
        }


        public int Insert(CustomerModal obj)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_CustomersCrud", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //SetCommonParameters(cmd, obj);
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

                cmd.Parameters.Add("first_name", SqlDbType.NVarChar).Value = obj.first_name;
                cmd.Parameters.Add("last_name", SqlDbType.NVarChar).Value = obj.last_name;
                cmd.Parameters.Add("email", SqlDbType.NVarChar).Value = obj.email;
                cmd.Parameters.Add("address", SqlDbType.NVarChar).Value = obj.address;
                cmd.Parameters.Add("status", SqlDbType.Int).Value = 1; // Assuming status is always active
                cmd.Parameters.Add("contact_no", SqlDbType.NVarChar).Value = obj.contact_no;
                cmd.Parameters.Add("vat_no", SqlDbType.NVarChar).Value = obj.vat_no;
                cmd.Parameters.Add("credit_limit", SqlDbType.Decimal).Value = obj.credit_limit;
                cmd.Parameters.Add("vin_no", SqlDbType.NVarChar).Value = obj.vin_no;
                cmd.Parameters.Add("car_name", SqlDbType.NVarChar).Value = obj.car_name;

                cmd.Parameters.Add("@OperationType", SqlDbType.VarChar).Value = "1";
                cmd.Parameters.Add("@date_created", SqlDbType.DateTime).Value = obj.date_created;
                cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UsersModal.logged_in_userid;

                cmd.Parameters.Add("@StreetName", SqlDbType.NVarChar).Value = obj.StreetName;
                cmd.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = obj.PostalCode;
                cmd.Parameters.Add("@BuildingNumber", SqlDbType.NVarChar).Value = obj.BuildingNumber;
                cmd.Parameters.Add("@CitySubdivisionName", SqlDbType.NVarChar).Value = obj.CitySubdivisionName;
                cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = obj.CityName;
                cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = obj.CountryName;
                cmd.Parameters.Add("@RegistrationName", SqlDbType.NVarChar).Value = obj.registrationName;

                try
                {
                    cn.Open();
                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    LogAction("Add Customer", result, obj);
                    return result;
                }
                catch (Exception ex)
                {
                    // Log exception details here
                    throw new Exception("Error inserting customer: " + ex.Message);
                }
            }
        }

        public int Update(CustomerModal obj)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_CustomersCrud", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = obj.id;
                cmd.Parameters.Add("first_name", SqlDbType.NVarChar).Value = obj.first_name;
                cmd.Parameters.Add("last_name", SqlDbType.NVarChar).Value = obj.last_name;
                cmd.Parameters.Add("email", SqlDbType.NVarChar).Value = obj.email;
                cmd.Parameters.Add("address", SqlDbType.NVarChar).Value = obj.address;
                cmd.Parameters.Add("status", SqlDbType.Int).Value = 1; // Assuming status is always active
                cmd.Parameters.Add("contact_no", SqlDbType.NVarChar).Value = obj.contact_no;
                cmd.Parameters.Add("vat_no", SqlDbType.NVarChar).Value = obj.vat_no;
                cmd.Parameters.Add("credit_limit", SqlDbType.Decimal).Value = obj.credit_limit;
                cmd.Parameters.Add("vin_no", SqlDbType.NVarChar).Value = obj.vin_no;
                cmd.Parameters.Add("car_name", SqlDbType.NVarChar).Value = obj.car_name;

                cmd.Parameters.Add("@OperationType", SqlDbType.VarChar).Value = "2";
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                cmd.Parameters.Add("@date_updated", SqlDbType.DateTime).Value = obj.date_updated;
                cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UsersModal.logged_in_userid;

                cmd.Parameters.Add("@StreetName", SqlDbType.NVarChar).Value = obj.StreetName;
                cmd.Parameters.Add("@PostalCode", SqlDbType.NVarChar).Value = obj.PostalCode;
                cmd.Parameters.Add("@BuildingNumber", SqlDbType.NVarChar).Value = obj.BuildingNumber;
                cmd.Parameters.Add("@CitySubdivisionName", SqlDbType.NVarChar).Value = obj.CitySubdivisionName;
                cmd.Parameters.Add("@CityName", SqlDbType.NVarChar).Value = obj.CityName;
                cmd.Parameters.Add("@CountryName", SqlDbType.NVarChar).Value = obj.CountryName;
                cmd.Parameters.Add("@RegistrationName", SqlDbType.NVarChar).Value = obj.registrationName;


                //SetCommonParameters(cmd, obj);


                try
                {
                    cn.Open();
                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    LogAction("Update Customer", obj.id, obj);
                    return result;
                }
                catch (Exception ex)
                {
                    // Log exception details here
                    throw new Exception("Error updating customer: " + ex.Message);
                }
            }
        }

        public int Delete(int customerId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_CustomersCrud", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = customerId;
                cmd.Parameters.Add("@OperationType", SqlDbType.VarChar).Value = "3";
                cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

                try
                {
                    cn.Open();
                    int result = cmd.ExecuteNonQuery();
                    Log.LogAction("Delete Customer", $"Customer ID: {customerId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                    return result;
                }
                catch (Exception ex)
                {
                    // Log exception details here
                    throw new Exception("Error deleting customer: " + ex.Message);
                }
            }
        }

        private void SetCommonParameters(SqlCommand cmd, CustomerModal obj)
        {
            cmd.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
            cmd.Parameters.Add("@first_name", SqlDbType.NVarChar).Value = obj.first_name;
            cmd.Parameters.Add("@last_name", SqlDbType.NVarChar).Value = obj.last_name;
            cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = obj.email;
            cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = obj.address;
            cmd.Parameters.Add("@status", SqlDbType.Int).Value = 1;
            cmd.Parameters.Add("@contact_no", SqlDbType.NVarChar).Value = obj.contact_no;
            cmd.Parameters.Add("@vat_no", SqlDbType.NVarChar).Value = obj.vat_no;
            cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UsersModal.logged_in_userid;
            cmd.Parameters.Add("@credit_limit", SqlDbType.Decimal).Value = obj.credit_limit;
            cmd.Parameters.Add("@vin_no", SqlDbType.NVarChar).Value = obj.vin_no;
            cmd.Parameters.Add("@car_name", SqlDbType.NVarChar).Value = obj.car_name;
            cmd.Parameters.Add("@date_created", SqlDbType.DateTime).Value = DateTime.Now;
        }

        private void LogAction(string action, int customerId, CustomerModal obj)
        {
            Log.LogAction(action, $"Customer ID: {customerId}, Customer Name: {obj.first_name} {obj.last_name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
        }
    }
}
