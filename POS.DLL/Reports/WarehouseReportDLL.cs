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
    public class WarehouseReportDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
       // private WarehouseModal info = new WarehouseModal();

        public DataTable WarehouseReport(string[] category_code, string[] brand_code, string[] location_code, int unit_id = 0, string item_type = "", bool qty_onhand = false)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand();
                        int i = 0;
                        string brand = "brand";
                        string location = "loc";
                        string category = "cat";

                        String query = "SELECT P.code,P.name,"+
                            " COALESCE((select TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=P.item_number and s.branch_id=@branch_id),0) as qty," + //branch wise qty
                            " P.avg_cost as cost_price,P.unit_price,P.brand_code,P.item_type,P.location_code," +
                            " C.name AS category_name," +
                            " U.name AS unit," +
                            " B.name AS brand," +
                            " (COALESCE((select TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=P.item_number and s.branch_id=@branch_id),0)*avg_cost) AS total_cost" +
                            " FROM pos_products P" +
                            " LEFT JOIN pos_units U ON U.id=P.unit_id" +
                            " LEFT JOIN pos_categories C ON C.code=P.category_code" +
                            " LEFT JOIN pos_brands B ON B.code=P.brand_code" +
                            //" LEFT JOIN pos_product_stocks PS ON PS.item_id=P.id" +
                            " WHERE ("; // 4 empty spaces is needed, it further remove in below code

                        if (brand_code.Length > 0)
                        {
                            //query += " (";

                            foreach (string word in brand_code)
                            {
                                if(word != "0")
                                {
                                    brand += i++;
                                    query += string.Format(" P.brand_code = @{0} OR ", brand);
                                    cmd.Parameters.AddWithValue(String.Format("@{0}", brand), string.Format("{0}", word));
                                }
                                
                            }
                            //query = query.Substring(0, query.Length - 4);// remove last 4 letter i.e. AND from query
                           // query += ")";

                        }
                        if (location_code.Length > 0)
                        {
                            //query += " (";

                            foreach (string word in location_code)
                            {
                                if (word != "0")
                                {
                                    location += i++;
                                    query += string.Format(" P.location_code = @{0} OR ", location);
                                    cmd.Parameters.AddWithValue(String.Format("@{0}", location), string.Format("{0}", word));
                                }
                            }
                            //query = query.Substring(0, query.Length - 4);// remove last 4 letter i.e. AND from query
                            //query += ")";

                        }

                        if (category_code.Length > 0)
                        {
                            //query += " (";

                            foreach (string word in brand_code)
                            {
                                if (word != "0")
                                {
                                    category += i++;
                                    query += string.Format(" P.category_code = @{0} OR ", category);
                                    cmd.Parameters.AddWithValue(String.Format("@{0}", category), string.Format("{0}", word));
                                }
                            }
                            //query = query.Substring(0, query.Length - 4);// remove last 4 letter i.e. AND from query
                            //query += ")";

                        }
                        
                        if (category_code.Length <= 0 && location_code.Length <= 0 && brand_code.Length <= 0)
                        {
                            query = query.Substring(0, query.Length - 8);
                            query += " WHERE 1=1";
                        }
                        else
                        {
                            query = query.Substring(0, query.Length - 4);
                            query += ")";

                        }
                        
                        if (item_type != "All")
                        {
                            query += " AND P.item_type = @item_type";
                            cmd.Parameters.AddWithValue("@item_type", item_type);
                        }
                        
                        if (unit_id != 0)
                        {
                            query += " AND P.unit_id = @unit_id";
                            cmd.Parameters.AddWithValue("@unit_id", unit_id);
                        }
                        if (qty_onhand)
                        {
                            query += " AND COALESCE((select TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=P.item_number and s.branch_id=@branch_id),0) > 0";
                        }


                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        //cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.CommandText = query;
                        cmd.Connection = cn;
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


        public DataTable WarehouseReport_total_amount(string[] category_code, string[] brand_code, string[] location_code, int unit_id = 0, string item_type = "", bool qty_onhand = false)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand();
                        int i = 0;
                        string brand = "brand";
                        string location = "loc";
                        string category = "cat";

                        String query = "SELECT COALESCE((select TOP 1 s.qty from pos_product_stocks s where s.item_number = P.item_number and s.branch_id = @branch_id),0) as qty," + //branch wise qty" +
                            " SUM(P.avg_cost) AS cost_price,SUM(P.unit_price) as unit_price," +
                            " SUM(COALESCE((select s.qty from pos_product_stocks s where s.item_number = P.item_number and s.branch_id = @branch_id),0)*P.avg_cost) as total_cost" +
                            " FROM pos_products P" +
                            " WHERE ("; // 4 empty spaces is needed, it further remove in below code

                        if (brand_code.Length > 0)
                        {
                            //query += " (";

                            foreach (string word in brand_code)
                            {
                                if (word != "0")
                                {
                                    brand += i++;
                                    query += string.Format(" P.brand_code = @{0} OR ", brand);
                                    cmd.Parameters.AddWithValue(String.Format("@{0}", brand), string.Format("{0}", word));
                                }

                            }
                            //query = query.Substring(0, query.Length - 4);// remove last 4 letter i.e. AND from query
                            // query += ")";

                        }
                        if (location_code.Length > 0)
                        {
                            //query += " (";

                            foreach (string word in location_code)
                            {
                                if (word != "0")
                                {
                                    location += i++;
                                    query += string.Format(" P.location_code = @{0} OR ", location);
                                    cmd.Parameters.AddWithValue(String.Format("@{0}", location), string.Format("{0}", word));
                                }
                            }
                            //query = query.Substring(0, query.Length - 4);// remove last 4 letter i.e. AND from query
                            //query += ")";

                        }

                        if (category_code.Length > 0)
                        {
                            //query += " (";

                            foreach (string word in brand_code)
                            {
                                if (word != "0")
                                {
                                    category += i++;
                                    query += string.Format(" P.category_code = @{0} OR ", category);
                                    cmd.Parameters.AddWithValue(String.Format("@{0}", category), string.Format("{0}", word));
                                }
                            }
                            //query = query.Substring(0, query.Length - 4);// remove last 4 letter i.e. AND from query
                            //query += ")";

                        }

                        if (category_code.Length <= 0 && location_code.Length <= 0 && brand_code.Length <= 0)
                        {
                            query = query.Substring(0, query.Length - 8);
                            query += " WHERE 1=1";
                        }
                        else
                        {
                            query = query.Substring(0, query.Length - 4);
                            query += ")";

                        }
                        

                        if (item_type != "All")
                        {
                            query += " AND P.item_type = @item_type";
                            cmd.Parameters.AddWithValue("@item_type", item_type);
                        }
                       
                        if (unit_id != 0)
                        {
                            query += " AND P.unit_id = @unit_id";
                            cmd.Parameters.AddWithValue("@unit_id", unit_id);
                        }
                        if (qty_onhand)
                        {
                            query += " AND COALESCE((select TOP 1 s.qty from pos_product_stocks s where s.item_number = P.item_number and s.branch_id = @branch_id),0) 0";
                        }


                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        //cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.CommandText = query;
                        cmd.Connection = cn;
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
        public DataSet InventoryReport(int branchId, int userId, string category = null, string brand = null, string location = null, int OperationType=1)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    // Create a new dataset
                    DataSet ds = new DataSet();

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand();
                        // SQL connection and command

                        using (SqlCommand cmd = new SqlCommand("sp_InventoryReport", cn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@BranchID", branchId);
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.Parameters.AddWithValue("@Category", (object)category ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Brand", (object)brand ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@Location", (object)location ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@IsQOH", 1);

                            cmd.Parameters.AddWithValue("@OperationType", OperationType);
                                //--operation types   
                                //-- 1) Get Filtered Stock Report  
                                //-- 2) Get Low Stock Report  
                            

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(ds, "StockReport");
                        }
                    }

                    return ds;
                }
                catch
                {

                    throw;
                }
            }

        }
    }
}
