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
    public class TaxDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private TaxModal info = new TaxModal();

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_TaxesCrud", cn);
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

        public DataTable SearchRecordByTaxID(int Tax_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT id,title,rate,FROM pos_taxes WHERE id = @id", cn);
                        cmd.Parameters.AddWithValue("@id", Tax_id);
                        
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

                        cmd = new SqlCommand("SELECT * FROM pos_taxes WHERE title LIKE @title", cn);
                        //cmd.Parameters.AddWithValue("@id", condition);
                        cmd.Parameters.AddWithValue("@title", string.Format("%{0}%", condition));
                        
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

        public int Insert(TaxModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_TaxesCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@title", obj.title);
                        cmd.Parameters.AddWithValue("@rate", obj.rate);
                        cmd.Parameters.AddWithValue("@status", 1);
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
                    Log.LogAction("Add Tax", $"Tax ID: {result}, Tax Name: {obj.title}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int Update(TaxModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_TaxesCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        //cmd.Parameters.AddWithValue("@branch_id", 0);
                        cmd.Parameters.AddWithValue("@title", obj.title);
                        cmd.Parameters.AddWithValue("@rate", obj.rate);
                        cmd.Parameters.AddWithValue("@status", 1);
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
                    Log.LogAction("Update Tax", $"Tax ID: {obj.id}, Tax Name: {obj.title}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                     
                    return (int)result;
                }
                catch
                {

                    throw;
                }
                
            }
        }

        public int Delete(int TaxId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_TaxesCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", TaxId); 
                        cmd.Parameters.AddWithValue("@OperationType", "3");

                    }

                    int result = cmd.ExecuteNonQuery();
                    Log.LogAction("Delete Tax/VAT", $"VAT ID: {TaxId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

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
