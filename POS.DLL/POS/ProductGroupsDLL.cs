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
    public class ProductGroupsDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private ProductGroupsModal info = new ProductGroupsModal();

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_product_groupsCrud", cn);
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

        public DataTable SearchRecordByProductGroupsID(int ProductGroups_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT id,code,name FROM pos_product_groups WHERE id = @id", cn);
                        cmd.Parameters.AddWithValue("@id", ProductGroups_id);
                        
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

        public int GetMaxAlternateNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT MAX(alt_no) FROM pos_products", cn);

                        int maxId = Convert.ToInt32(cmd.ExecuteScalar());

                        if (maxId == 0)
                        {
                            return maxId = 1;
                        }
                        else
                        {
                            int intval = maxId;
                            intval++;
                            maxId = intval;
                            return maxId++;
                        }

                    }
                    return 0;
                }
                catch
                {

                    throw;
                }
            }

        }

        public DataTable SearchRecordByName(String group_name)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT id,code,name, date_created FROM pos_product_groups WHERE name LIKE @name", cn);
                        
                        cmd.Parameters.AddWithValue("@name", string.Format("%{0}%", group_name));

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

        public DataTable SearchRecordByGroup(String group_code)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        string query = "SELECT P.id,P.name,P.code" +
                            " FROM pos_products P" +
                            " LEFT JOIN pos_product_groups C ON C.code=P.group_code" +
                            " WHERE P.group_code = @group_code ";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@group_code ", group_code);

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

        public DataTable SearchAlternateProducts(int alt_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        string query = "SELECT P.id,P.name,P.code,alt_no As alternate_no" +
                            " FROM pos_products P" +
                            
                            " WHERE P.alt_no = @alt_no ";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@alt_no ", alt_no);

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

        public int Insert(ProductGroupsModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_product_groupsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@code", obj.code);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
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
                    Log.LogAction("Add Product Group", $"Product Group Code: {obj.code},  Product Group Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public string InsertProductAlternate(ProductGroupsModal obj)
        {
            string result = "";
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_product_groupsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@code", obj.code);
                        cmd.Parameters.AddWithValue("@alt_no", obj.alt_no);
                        cmd.Parameters.AddWithValue("@product_id", obj.product_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "6");

                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result =cmd.ExecuteScalar().ToString();
                    Log.LogAction("Add Product Alternate", $"Product Alternate Code: {obj.code},  Product Alternate No.: {obj.alt_no}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                }
                catch
                {

                    throw;
                }
            }
            return result;
        }
        public string InsertProductGroupDetail(ProductGroupsModal obj)
        {
            
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    var result = "";
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_product_groupsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@id", obj.id);
                        cmd.Parameters.AddWithValue("@group_code", obj.group_code);
                        cmd.Parameters.AddWithValue("@product_id", obj.product_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "5");

                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToString(cmd.ExecuteScalar());
                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int Update(ProductGroupsModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_product_groupsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        cmd.Parameters.AddWithValue("@code", obj.code);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@user_id", obj.user_id);
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
                    Log.LogAction("Update Product Group", $"Product Group Code: {obj.code},  Product Group Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)result;
                }
                catch
                {

                    throw;
                }
                
            }
        }

        public int Delete(int ProductGroupsId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_product_groupsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", ProductGroupsId); 
                        cmd.Parameters.AddWithValue("@OperationType", "3");

                    }

                    int result = cmd.ExecuteNonQuery();
                    Log.LogAction("Delete Product Group", $"Product Group ID: {ProductGroupsId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int DeleteAltNo(int ProductId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_product_groupsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@product_id", ProductId);
                        cmd.Parameters.AddWithValue("@OperationType", "7");

                    }

                    int result = cmd.ExecuteNonQuery();
                    Log.LogAction("Delete Product Alternate", $"Product Alternate ID: {ProductId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

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
