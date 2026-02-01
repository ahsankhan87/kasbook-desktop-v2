using POS.Core;
using POS.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace POS.DLL
{
    public class ProductDLL
    {
        private InventoryDAL inventoryDAL;

        public ProductDLL()
        {
            inventoryDAL = new InventoryDAL();
        }
        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ProductsCrud", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OperationType", "");
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

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
                    throw new Exception("Error fetching all Products: " + ex.Message);
                }
                return dt;
            }

        }
        public String GetMaxProductNumber()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    // item_number is NVARCHAR; find the largest numeric value safely.
                    // Non-numeric values are ignored.
                    using (SqlCommand cmd = new SqlCommand(@"
                        SELECT TOP 1 item_number
                        FROM pos_products
                        WHERE TRY_CONVERT(BIGINT, item_number) IS NOT NULL
                        ORDER BY TRY_CONVERT(BIGINT, item_number) DESC, id DESC", cn))
                    {
                        var maxVal = Convert.ToString(cmd.ExecuteScalar());

                        if (string.IsNullOrWhiteSpace(maxVal))
                            return "1";

                        long current;
                        if (!long.TryParse(maxVal, out current))
                            return "1";

                        return (current + 1).ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error getting max item_number: " + ex.Message, ex);
                }
            }
        }

        public DataTable GetProductsSummary(DateTime StartDate, DateTime EndDate, bool is_zero, string group_code, string brand_code, string category_code)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ProductsCrud", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OperationType", "10");
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@fromDate", StartDate);
                cmd.Parameters.AddWithValue("@toDate", EndDate);
                cmd.Parameters.AddWithValue("@is_zero", is_zero);

                // Convert brand code list to comma-separated string
                //string brandCodesCsv = (brand_code != null && brand_code != "")
                //    ? string.Join(",", brand_code)
                //    : null;

                cmd.Parameters.AddWithValue("@brand_codes", string.IsNullOrEmpty(brand_code) ? (object)DBNull.Value : brand_code);
                //cmd.Parameters.AddWithValue("@brand_code", string.IsNullOrEmpty(brand_code) ? (object)DBNull.Value : brand_code);
                cmd.Parameters.AddWithValue("@category_code", string.IsNullOrEmpty(category_code) ? (object)DBNull.Value : category_code);
                cmd.Parameters.AddWithValue("@group_code", string.IsNullOrEmpty(group_code) ? (object)DBNull.Value : group_code);

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
                    throw new Exception("Error fetching all Products: " + ex.Message);
                }
                return dt;
            }

        }
        public List<ProductModal> GetAllProducts(string keyword)
        {
            List<ProductModal> products = new List<ProductModal>();
            using (SqlConnection connection = new SqlConnection(dbConnection.ConnectionString))
            {
                string query = "SELECT p.id,p.item_number, p.code,p.name, p.name_ar, p.category_code, p.item_type, p.brand_code, p.status, p.barcode, p.avg_cost, p.cost_price, p.unit_price, p.unit_price_2, p.tax_id,P.location_code," +
                            " p.unit_id, p.re_stock_level, p.description, p.deleted, p.date_created, p.date_updated, p.user_id, p.demand_qty, p.purchase_demand_qty, p.sale_demand_qty, p.origin, p.group_code, p.alt_no, p.picture, p.packet_qty," +
                            " COALESCE((select  TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=p.item_number and s.branch_id=" + UsersModal.logged_in_branch_id + "),0) as qty," + //branch wise qty
                            " ISNULL(C.name,'') AS category, ISNULL(C.id,'') AS category_id,P.part_number" +
                            " FROM pos_products AS P" +
                            " LEFT JOIN pos_categories C ON C.code=P.category_code" +
                            //" WHERE (contains(P.name,@keyword_name) OR contains(P.item_number,@item_number) OR contains(P.item_number_2,@item_number_2) OR contains(P.description,@keyword_desc) OR contains(P.code,@keyword_code)) AND P.branch_id = @branch_id";
                            " WHERE p.deleted=0 AND contains(P.*,'" + keyword + "')" +
                            " ORDER BY qty desc";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            products.Add(new ProductModal
                            {
                                id = reader.GetInt32(0),
                                item_number = reader.GetString(1),

                                code = reader.GetString(2),
                                name = reader.GetString(3),
                                name_ar = reader.GetString(4),
                                category_code = reader.GetString(5),
                                item_type = reader.GetString(6),
                                brand_code = reader.GetString(7),
                                status = reader.GetBoolean(8),
                                barcode = reader.GetString(9),
                                avg_cost = (double)reader.GetDecimal(10),
                                cost_price = (double)reader.GetDecimal(11),
                                unit_price = (double)reader.GetDecimal(12),
                                unit_price_2 = (double)reader.GetDecimal(13),
                                tax_id = reader.GetInt32(14),
                                location_code = reader.GetString(15),
                                unit_id = reader.GetInt32(16),
                                re_stock_level = reader.GetDecimal(17),
                                description = reader.GetString(18),
                                //date_created = reader.GetString(20),
                                //date_updated = reader.GetString(21),
                                demand_qty = reader.GetDecimal(23),
                                purchase_demand_qty = reader.GetDecimal(24),
                                sale_demand_qty = reader.GetDecimal(25),
                                origin = reader.GetString(26),
                                group_code = reader.GetString(27),
                                alt_no = reader.GetInt32(28),
                                //picture = reader.GetByte(29),
                                packet_qty = reader.GetDecimal(30),
                                qty = (double)reader.GetDecimal(31),
                                category = (reader.GetString(32) == string.Empty ? "" : reader.GetString(32)),
                                category_id = reader.GetInt32(33),
                                part_number = reader.GetString(34),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching all Products: " + ex.Message);
                }
            }

            return products;
        }

        public DataTable GetAllByProductId(int Product_id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = "SELECT p.id,p.item_number,P.part_number, p.item_number_2, p.code,p.name, p.name_ar, p.category_code, p.item_type, p.brand_code, p.status, p.barcode, p.avg_cost, p.cost_price, p.unit_price, p.unit_price_2, p.tax_id," +
                    " p.unit_id, p.re_stock_level, p.description, p.deleted, p.date_created, p.date_updated, p.user_id, p.demand_qty, p.purchase_demand_qty, p.sale_demand_qty, p.origin, p.group_code, p.alt_no," +
                    " p.picture, p.packet_qty, p.expiry_date,p.location_code,p.supplier_id," +
                    " COALESCE((select  TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=p.item_number and s.branch_id=@branch_id),0) as qty" + //branch wise qty
                    " FROM pos_products p" +
                    " WHERE p.deleted=0 AND p.id = @Product_id ";
                // "AND P.branch_id = @branch_id";

                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@Product_id", Product_id);

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
                    throw new Exception("Error in GetAllByProductId: " + ex.Message);
                }
            }
            return dt;

        }

        public DataTable GetAllByProductCode(string Product_code)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            string query = "SELECT p.id,p.item_number,p.part_number, p.item_number_2,p.code,p.name, p.name_ar, p.category_code, p.item_type, p.brand_code, p.status, p.barcode, p.avg_cost, p.cost_price, p.unit_price, p.unit_price_2, p.tax_id," +
                                " p.unit_id, p.re_stock_level, p.description, p.deleted, p.date_created, p.date_updated, p.user_id, p.demand_qty, p.purchase_demand_qty, p.sale_demand_qty, p.origin, p.group_code, p.alt_no, " +
                                "p.picture, p.packet_qty, p.expiry_date,p.location_code,p.supplier_id," +
                                " COALESCE((select TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=p.item_number and s.branch_id=@branch_id),0) as qty," + //branch wise qty
                                " COALESCE((select TOP 1 SUM(s.qty) as qty from pos_product_stocks s where s.item_number=p.item_number),0) as company_qty, " + //branch wise qty
                                " ps.reorder_level " +
                                " FROM pos_products p " +
                                " LEFT JOIN pos_product_stocks ps ON p.id = ps.item_id" +
                                " WHERE p.deleted=0 AND p.code = @code ";
                            // " AND ps.branch_id = @branch_id";

                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@code", Product_code);
                            cmd.CommandText = query;
                            cmd.Connection = cn;

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }

                        }

                        return dt;
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception as necessary
                        throw new Exception("Error: " + ex.Message);
                    }
                }
            }

        }
        public DataTable GetAllByProductByItemNumber(string item_number)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            string query = "SELECT p.id,p.item_number,p.part_number, p.item_number_2,p.part_number, p.code,p.name, p.name_ar, p.category_code, p.item_type, p.brand_code, p.status, p.barcode, p.avg_cost, p.cost_price, p.unit_price, p.unit_price_2, p.tax_id," +
                                " p.unit_id, p.re_stock_level, p.description, p.deleted, p.date_created, p.date_updated, p.user_id, p.demand_qty, p.purchase_demand_qty, p.sale_demand_qty, p.origin, p.group_code, p.alt_no, " +
                                "p.picture, p.packet_qty, p.expiry_date,p.location_code,p.supplier_id," +
                                " COALESCE((select TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=p.item_number and s.branch_id=@branch_id),0) as qty," + //branch wise qty
                                " COALESCE((select TOP 1 SUM(s.qty) as qty from pos_product_stocks s where s.item_number=p.item_number),0) as company_qty, " + //branch wise qty
                                " ps.reorder_level " +
                                " FROM pos_products p " +
                                " LEFT JOIN pos_product_stocks ps ON p.id = ps.item_id" +
                                " WHERE p.deleted=0 AND p.item_number = @item_number ";
                            //" AND ps.branch_id = @branch_id";

                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@item_number", item_number);
                            cmd.CommandText = query;
                            cmd.Connection = cn;

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }

                        }

                        return dt;
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception as necessary
                        throw new Exception("Error: " + ex.Message);
                    }
                }
            }

        }
        public DataTable GetProductsByAlternateNo(int alt_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable(); // Instantiate DataTable

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            string query = "SELECT P.id,P.item_number,P.code,P.name,P.avg_cost,P.unit_price,P.cost_price,P.category_code, " +
                                " COALESCE((select  TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=P.item_number and s.branch_id=@branch_id),0) as qty," + //branch wise qty
                                " C.name AS category, C.id AS category_id" +
                                " FROM pos_products P" +
                                " LEFT JOIN pos_categories C ON C.code=P.category_code" +
                                " WHERE P.deleted=0 AND P.alt_no = @alt_no";
                            //" AND P.branch_id = @branch_id";

                            //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@alt_no", alt_no);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.CommandText = query;
                            cmd.Connection = cn;
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }
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

        }
        public DataTable SearchRecord(String condition, bool by_code = false, bool by_name = false)
        {
            DataTable dt = new DataTable(); // Instantiate DataTable
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            string[] words = condition.Split(' ');
                            string keyword = "";

                            foreach (string word in words)
                            {
                                keyword += "\"" + word + "*\" AND ";

                            }
                            keyword = keyword.Substring(0, keyword.Length - 4);// remove last 3 letter i.e. AND from query

                            string query = "SELECT p.id,p.item_number,p.part_number, p.code,p.name, p.name_ar, p.category_code, p.item_type, p.brand_code, p.status, p.barcode, p.avg_cost, p.cost_price, p.unit_price, p.unit_price_2, p.tax_id," +
                                " p.unit_id, p.re_stock_level, p.description, p.deleted, p.date_created, p.date_updated, p.user_id, p.demand_qty, p.purchase_demand_qty, p.sale_demand_qty, p.origin, p.group_code, p.alt_no, p.picture, p.packet_qty," +
                                " COALESCE((select  TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=p.item_number and s.branch_id=@branch_id),0) as qty" + //branch wise qty
                                " FROM pos_products AS p";

                            if (by_code)
                            {
                                query += " WHERE p.deleted=0 AND contains(p.part_number,@code)";
                                cmd.Parameters.AddWithValue("@code", keyword);
                                // cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                                //cmd.Parameters.AddWithValue("@item_number", keyword);
                            }
                            else if (by_name)
                            {
                                query += " WHERE p.deleted=0 AND contains(p.name,@keyword_name) OR contains(p.description,@keyword_desc)";
                                cmd.Parameters.AddWithValue("@keyword_name", keyword);
                                cmd.Parameters.AddWithValue("@keyword_desc", keyword);
                                //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            }
                            else
                            {
                                query += " WHERE p.deleted=0 AND contains(p.name,@keyword_name) OR contains(p.description,@keyword_desc)";
                                cmd.Parameters.AddWithValue("@keyword_name", keyword);
                                cmd.Parameters.AddWithValue("@keyword_desc", keyword);
                                //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            }
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            cmd.CommandText = query;
                            cmd.Connection = cn;
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }

                        }


                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return dt;
        }

        public DataTable SearchProductByLocation(String condition, bool by_code = false, bool by_name = false)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable(); // Instantiate DataTable

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            string query = "SELECT p.id,p.item_number,part_number, p.item_number_2, p.code,p.name, p.name_ar, p.category_code, p.item_type, p.brand_code, p.status, p.barcode, p.avg_cost, p.cost_price, p.unit_price, p.unit_price_2, p.tax_id," +
                                " p.unit_id, p.re_stock_level, p.description, p.deleted, p.date_created, p.date_updated, p.user_id, p.demand_qty, p.purchase_demand_qty, p.sale_demand_qty, p.origin, p.group_code, p.alt_no, p.picture, p.packet_qty," +
                                " COALESCE((select  TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=p.item_number and s.branch_id=@branch_id),0) as qty" + //branch wise qty
                                " FROM pos_products as p";
                            //" FROM pos_products_location_view";

                            if (by_code)
                            {
                                query += " WHERE p.deleted=0 AND p.code LIKE @code OR replace(p.code,'-','') LIKE @code";
                                cmd.Parameters.AddWithValue("@code", string.Format("%{0}%", condition));
                                //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            }
                            else if (by_name)
                            {
                                query += " WHERE p.deleted=0 AND p.name LIKE @name";
                                cmd.Parameters.AddWithValue("@name", string.Format("%{0}%", condition));
                                //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            }
                            else
                            {
                                query += " WHERE deleted=0 AND p.name LIKE @name";
                                cmd.Parameters.AddWithValue("@name", string.Format("%{0}%", condition));
                                //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            }
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            cmd.CommandText = query;
                            cmd.Connection = cn;
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }

                        }

                        return dt;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        //public DataTable SearchProductByBrandAndCategory_2(String condition, string category_id, string brand_id,string group_code="")
        //{
        //    using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
        //    {
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            try
        //            {
        //                DataTable dt = new DataTable(); // Instantiate DataTable

        //                if (cn.State == ConnectionState.Closed)
        //                {
        //                    cn.Open();

        //                    //WHEN the keyword has + sign then it will separate left and right into two keyword 
        //                    int condition_index_len = (condition.IndexOf("+") <= 0 ? 0 : condition.IndexOf("+"));
        //                    string[] words = condition.Substring(condition.IndexOf("+") + 1).Split(' ');
        //                    string[] words_2 = condition.Substring(0, condition_index_len).Split(' ');


        //                    string keyword = "";
        //                    string keyword_2 = "";


        //                    foreach (string word in words)
        //                    {
        //                        keyword += "\"" + word + "*\" AND ";
        //                    }

        //                    if (words_2[0].Length > 0)
        //                    {
        //                        foreach (string word_2 in words_2)
        //                        {
        //                            keyword_2 += "\"" + word_2 + "*\" AND ";
        //                            //keyword_2 += "AND contains(P.*, '\"" + word_2 + "*\"') AND ";
        //                        }

        //                        keyword_2 = keyword_2.Substring(0, keyword_2.Length - 4);// remove last 3 letter i.e. AND from query

        //                    }

        //                    keyword = keyword.Substring(0, keyword.Length - 4);// remove last 3 letter i.e. AND from query

        //                    string query = "SELECT p.id,p.item_number, p.code,p.name, p.name_ar, p.category_code, p.item_type, p.brand_code, p.status, p.barcode, p.avg_cost, p.cost_price, p.unit_price, p.unit_price_2, p.tax_id,P.location_code," +
        //                        " p.unit_id, p.re_stock_level, p.description, p.deleted, p.date_created, p.date_updated, p.user_id, p.demand_qty, p.purchase_demand_qty, p.sale_demand_qty, p.origin, p.group_code, p.alt_no, p.picture, p.packet_qty," +
        //                        " COALESCE((select TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_code=p.code and s.branch_id=@branch_id),0) as qty," + //branch wise qty
        //                        " C.name AS category, C.id AS category_id" +
        //                        " FROM pos_products AS P" +
        //                        " LEFT JOIN pos_categories C ON C.code=P.category_code" +
        //                        //" WHERE (contains(P.name,@keyword_name) OR contains(P.item_number,@item_number) OR contains(P.item_number_2,@item_number_2) OR contains(P.description,@keyword_desc) OR contains(P.code,@keyword_code)) AND P.branch_id = @branch_id";
        //                        " WHERE contains(P.*,'" + keyword + "')" +
        //                        " OR P.code LIKE '%" + words[0] + "%'" +
        //                        " " + (words_2[0].Length > 0 ? " OR contains(P.*,'" + keyword_2 + "')" : "") + "";
        //                    //" AND P.branch_id = @branch_id";

        //                    //cmd.Parameters.AddWithValue("@item_number_2", keyword);
        //                    //cmd.Parameters.AddWithValue("@item_number", keyword);
        //                    //cmd.Parameters.AddWithValue("@keyword_name", keyword);
        //                    //cmd.Parameters.AddWithValue("@keyword_desc", keyword);
        //                    //cmd.Parameters.AddWithValue("@keyword_code", keyword);
        //                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

        //                    //cmd.Parameters.AddWithValue("@OperationType", "9");

        //                    if (category_id != "")
        //                    {
        //                        query += " AND P.category_code = @category_code";
        //                        cmd.Parameters.AddWithValue("@category_code", category_id);
        //                    }
        //                    if (brand_id != "")
        //                    {
        //                        query += " AND P.brand_code = @brand_code";
        //                        cmd.Parameters.AddWithValue("@brand_code", brand_id);
        //                    }
        //                    if (group_code != "")
        //                    {
        //                        query += " AND P.group_code = @group_code";
        //                        cmd.Parameters.AddWithValue("@group_code", group_code);
        //                    }
        //                    query += " ORDER BY qty desc";

        //                    //cmd.Parameters.AddWithValue("@id", condition);
        //                    //cmd.Parameters.AddWithValue("@code", string.Format("%{0}%", condition));
        //                    //cmd.Parameters.AddWithValue("@name", string.Format("%{0}%", condition));

        //                    cmd.CommandText = query;
        //                    cmd.Connection = cn;
        //                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        //                    {
        //                        da.Fill(dt);
        //                    }

        //                }

        //                return dt;
        //            }
        //            catch
        //            {
        //                throw;
        //            }
        //        }
        //    }
        //}
        public DataTable SearchProductByBrandAndCategory_3(string condition, string category_id, string brand_id, string group_code = "")
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable();

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            // Base query - optimized with NOLOCK hints
                            string query = @"SELECT p.id, p.item_number,p.part_number, p.code, p.name, p.name_ar, p.category_code, 
                                    p.item_type, p.brand_code, p.status, p.barcode, p.avg_cost, p.cost_price, 
                                    p.unit_price, p.unit_price_2, p.tax_id, P.location_code, p.unit_id, 
                                    p.re_stock_level, p.description, p.deleted, p.date_created, p.date_updated, 
                                    p.user_id, p.demand_qty, p.purchase_demand_qty, p.sale_demand_qty, p.origin, 
                                    p.group_code, p.alt_no, p.picture, p.packet_qty,
                                    COALESCE((SELECT TOP 1 COALESCE(s.qty,0) 
                                    FROM pos_product_stocks s WITH (NOLOCK)
                                    WHERE s.item_number=p.item_number AND s.branch_id=@branch_id),0) as qty,
                                    C.name AS category, C.id AS category_id
                                    FROM pos_products AS P WITH (NOLOCK)
                                    LEFT JOIN pos_categories C WITH (NOLOCK) ON C.code=P.category_code
                                    WHERE p.deleted=0 ";

                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            // Handle search condition
                            if (!string.IsNullOrEmpty(condition))
                            {
                                // Use FREETEXT for more flexible searching
                                query += " AND FREETEXT((P.name, P.code, P.part_number, P.description), @searchTerm)";
                                cmd.Parameters.AddWithValue("@searchTerm", condition);

                                //Fallback for short search terms(FREETEXT ignores words < 3 chars)
                                if (condition.Trim().Length > 3)
                                {
                                    query += " OR P.code LIKE @fallbackSearch";
                                    cmd.Parameters.AddWithValue("@fallbackSearch", "%" + condition + "%");
                                }
                            }

                            // Add optional filters
                            if (!string.IsNullOrEmpty(category_id))
                            {
                                query += " AND P.category_code = @category_code";
                                cmd.Parameters.AddWithValue("@category_code", category_id);
                            }
                            if (!string.IsNullOrEmpty(brand_id))
                            {
                                query += " AND P.brand_code = @brand_code";
                                cmd.Parameters.AddWithValue("@brand_code", brand_id);
                            }
                            if (!string.IsNullOrEmpty(group_code))
                            {
                                query += " AND P.group_code = @group_code";
                                cmd.Parameters.AddWithValue("@group_code", group_code);
                            }

                            // Add TOP to limit results and improve performance
                            query = "SELECT TOP 500 * FROM (" + query + ") AS Results ORDER BY qty DESC";

                            cmd.CommandText = query;
                            cmd.CommandTimeout = 30; // Set reasonable timeout
                            cmd.Connection = cn;

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }
                        }

                        return dt;
                    }
                    catch (Exception ex)
                    {
                        // Enhanced error logging
                        throw new ApplicationException($"Search failed for term: {condition}. See inner exception.", ex);
                    }
                }
            }
        }
        public DataTable SearchProductByBrandAndCategory_2(string condition, string category_code, string brand_code, string group_code = "")
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable();

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            // Base query
                            string query = @"SELECT p.id, p.item_number,p.part_number, p.code, p.name, p.name_ar, p.category_code, 
                                    p.item_type, p.brand_code, p.status, p.barcode, p.avg_cost, p.cost_price, 
                                    p.unit_price, p.unit_price_2, p.tax_id, P.location_code, p.unit_id, 
                                    p.re_stock_level, p.description, p.deleted, p.date_created, p.date_updated, 
                                    p.user_id, p.demand_qty, p.purchase_demand_qty, p.sale_demand_qty, p.origin, 
                                    p.group_code, p.alt_no, p.picture, p.packet_qty,
                                    COALESCE((SELECT TOP 1 COALESCE(s.qty,0) 
                                    FROM pos_product_stocks s WITH (NOLOCK)
                                    WHERE s.item_number=p.item_number AND s.branch_id=@branch_id),0) as qty,
                                    C.name AS category, C.id AS category_id
                                    FROM pos_products AS P WITH (NOLOCK)
                                    LEFT JOIN pos_categories C WITH (NOLOCK) ON C.code=P.category_code
                                    WHERE p.deleted=0 ";

                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            // Handle search condition
                            if (!string.IsNullOrEmpty(condition))
                            {
                                // First try with CONTAINS (more efficient if full-text index exists)
                                string containsClause = BuildContainsClause(condition);

                                // Combine both approaches with OR
                                query += " AND (" + containsClause;

                                //string containsClause1 = OptimizeSearchTerm(condition);

                                // Fallback for short words (full-text ignores words < 3 chars)
                                if (condition.Trim().Length > 3)
                                {
                                    string likeClause = BuildLikeClause(condition,
                                    new[] { "P.code" }); //{ "P.name", "P.code", "P.item_number", "P.description" }

                                    query += " OR " + likeClause + ")";
                                    // Add LIKE pattern parameter
                                    cmd.Parameters.AddWithValue("@likePattern", "%" + condition + "%");

                                    //query += " OR P.code LIKE @exactCode";
                                    //cmd.Parameters.AddWithValue("@exactCode", "%" + condition + "%");
                                }
                                else
                                {
                                    query += ")";
                                }
                            }

                            // Add optional filters
                            if (!string.IsNullOrEmpty(category_code))
                            {
                                query += " AND P.category_code = @category_code";
                                cmd.Parameters.AddWithValue("@category_code", category_code);
                            }
                            if (!string.IsNullOrEmpty(brand_code))
                            {
                                var brandCodes = brand_code.Split(',').Select((code, index) =>
                                {
                                    string param = $"@brand_code_{index}";
                                    cmd.Parameters.AddWithValue(param, code);
                                    return param;
                                });

                                string brandInClause = string.Join(",", brandCodes);
                                query += $" AND P.brand_code IN ({brandInClause})";

                                //query += " AND P.brand_code = @brand_code";
                                //cmd.Parameters.AddWithValue("@brand_code", brand_code);
                            }
                            if (!string.IsNullOrEmpty(group_code))
                            {
                                query += " AND P.group_code = @group_code";
                                cmd.Parameters.AddWithValue("@group_code", group_code);
                            }

                            //query += " ORDER BY qty DESC";
                            // Add TOP to limit results and improve performance
                            query = "SELECT TOP 200 * FROM (" + query + ") AS Results ORDER BY name ASC";

                            cmd.CommandText = query;
                            cmd.Connection = cn;

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }
                        }

                        return dt;
                    }
                    catch (Exception ex)
                    {
                        // Log error and return empty table or rethrow
                        // Consider logging the actual query for debugging
                        throw new ApplicationException("Search failed. See inner exception for details. " + ex, ex);
                    }
                }
            }
        }

        private string BuildContainsClause(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return "1=1";

            // Handle both space-separated words and + separated terms
            string[] parts = searchTerm.Split('+');
            List<string> allConditions = new List<string>();

            foreach (string part in parts)
            {
                string[] words = part.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length == 0) continue;

                // Create CONTAINS condition for each part (split by +)
                var containsTerms = words.Select(word => $"\"{word}*\"");
                allConditions.Add($"CONTAINS((P.name, P.code, P.part_number, P.description,P.barcode), '{string.Join(" AND ", containsTerms)}')");
            }

            return allConditions.Count > 0 ? string.Join(" AND ", allConditions) : "1=1";
        }

        private string BuildLikeClause(string searchTerm, string[] columns)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return "1=1";

            // Remove dashes from searchTerm before assigning it to the parameter (do this in your parameter assignment code)
            return "(" + string.Join(" OR ", columns.Select(c => $"REPLACE({c}, '-', '') LIKE @likePattern")) + ")";
        }

        private string OptimizeSearchTerm(string searchTerm)
        {
            // Convert to lowercase and remove special characters
            searchTerm = searchTerm.ToLower().Replace("'", "").Replace("\"", "").Replace("*", "");

            // Handle different search patterns
            if (searchTerm.Contains("+"))
            {
                var parts = searchTerm.Split('+');
                return string.Join(" AND ", parts.Select(p => $"\"{p.Trim()}*\""));
            }
            else if (searchTerm.Contains(" "))
            {
                var words = searchTerm.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                return string.Join(" AND ", words.Select(w => $"\"{w}*\""));
            }
            else
            {
                return $"\"{searchTerm}*\"";
            }
        }

        public DataTable SearchProductByBrandAndCategory(String condition, string category_id, string brand_id, string group_code = "")
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable(); // Instantiate DataTable

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            //WHEN the keyword has + sign then it will separate left and right into two keyword 
                            string query = @"
                                    SELECT p.id, p.item_number,p.part_number, p.code, p.name, p.name_ar, p.category_code, p.item_type, 
                                           p.brand_code, p.status, p.barcode, p.avg_cost, p.cost_price, p.unit_price, 
                                           p.unit_price_2, p.tax_id, p.location_code, p.unit_id, p.re_stock_level, 
                                           p.description, p.deleted, p.date_created, p.date_updated, p.user_id, 
                                           p.demand_qty, p.purchase_demand_qty, p.sale_demand_qty, p.origin, 
                                           p.group_code, p.alt_no, p.picture, p.packet_qty, 
                                           COALESCE((SELECT TOP 1 COALESCE(s.qty, 0) 
                                                     FROM pos_product_stocks s 
                                                     WHERE s.item_number = p.item_number AND s.branch_id = @branch_id), 0) AS qty, 
                                           C.name AS category, C.id AS category_id
                                    FROM pos_products AS P
                                    LEFT JOIN pos_categories C ON C.code = P.category_code
                                    WHERE p.deleted=0 AND 
                                ";

                            // Conditions for search types (part number suffix, name, combined search)
                            if (condition.Contains("+"))
                            {
                                int condition_index_len = condition.IndexOf("+");
                                string partNumberCondition = condition.Substring(0, condition_index_len).Trim();
                                string partNameCondition = condition.Substring(condition_index_len + 1).Trim();

                                query += " (CONTAINS(P.part_number, @partNumberQuery) AND CONTAINS(P.name, @partNameQuery)) ";

                                cmd.Parameters.AddWithValue("@partNumberQuery", "\"" + partNumberCondition + "*\"");
                                cmd.Parameters.AddWithValue("@partNameQuery", "\"" + partNameCondition + "*\"");
                            }
                            else if (condition.Any(char.IsDigit))
                            {
                                query += " CONTAINS(P.part_number, @partNumberQuery)";
                                //query += " OR P.code LIKE '%' + @partCode + '%' ";
                                cmd.Parameters.AddWithValue("@partNumberQuery", "\"" + condition + "*\"");
                                //cmd.Parameters.AddWithValue("@partNumber", "\"*" + condition + "\"");
                                //cmd.Parameters.AddWithValue("@partCode", condition);
                            }
                            else
                            {
                                // Part name search only

                                query += " P.name LIKE @partName + '%' OR  P.code LIKE '%' + @partCode + '%' ";
                                cmd.Parameters.AddWithValue("@partName", condition);
                                cmd.Parameters.AddWithValue("@partCode", condition);
                                //query += " CONTAINS(P.name, @partNameQuery) ";
                                //cmd.Parameters.AddWithValue("@partNameQuery", "\"" + condition + "*\"");
                            }

                            // Filters for category, brand, and group codes
                            if (!string.IsNullOrEmpty(category_id))
                            {
                                query += " AND P.category_code = @category_code";
                                cmd.Parameters.AddWithValue("@category_code", category_id);
                            }
                            if (!string.IsNullOrEmpty(brand_id))
                            {
                                query += " AND P.brand_code = @brand_code";
                                cmd.Parameters.AddWithValue("@brand_code", brand_id);
                            }
                            if (!string.IsNullOrEmpty(group_code))
                            {
                                query += " AND P.group_code = @group_code";
                                cmd.Parameters.AddWithValue("@group_code", group_code);
                            }
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            query += " ORDER BY qty DESC";
                            cmd.CommandText = query;
                            cmd.Connection = cn;

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }

                        }

                        return dt;
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception as necessary
                        throw new Exception("Error: " + ex.Message);
                    }
                }
            }
        }

        public DataTable SearchProductByBrandAndCategory_1(String condition, string category_id, string brand_id, string group_code = "")
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable(); // Instantiate DataTable

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            string[] words = condition.Split(' ');
                            string name = "name";
                            string description = "description";
                            int i = 0;

                            string query = "SELECT P.id,P.code,P.part_number,P.name,P.name_ar,P.brand_code,P.item_type,P.barcode,P.avg_cost,P.location_code," +
                                " P.unit_price,P.cost_price,P.description,P.group_code,alt_no," +
                                " C.name AS category, C.id AS category_id," +
                                " COALESCE((select  TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=p.item_number and s.branch_id=@branch_id),0) as qty" + //branch wise qty
                                " FROM pos_products P" +
                                " LEFT JOIN pos_categories C ON C.code=P.category_code" +
                                //" LEFT JOIN pos_product_stocks PS ON PS.item_id=P.id" +
                                " WHERE p.deleted=0 AND ((replace(P.code,'-','') LIKE @code OR P.code LIKE @code OR ";

                            foreach (string word in words)
                            {
                                name += i++;
                                query += string.Format(" P.name LIKE @{0} AND ", name);
                                cmd.Parameters.AddWithValue(String.Format("@{0}", name), string.Format("%{0}%", word));
                            }

                            query = query.Substring(0, query.Length - 4);// remove last 4 letter i.e. AND from query
                            query += ")";

                            query += " OR (";
                            foreach (string word in words)
                            {
                                description += i++;
                                query += string.Format(" P.description LIKE @{0} AND ", description);
                                cmd.Parameters.AddWithValue(String.Format("@{0}", description), string.Format("%{0}%", word));
                            }
                            query = query.Substring(0, query.Length - 4);// remove last 4 letter i.e. AND from query
                            query += " ) )";

                            if (category_id != "")
                            {
                                query += " AND P.category_code = @category_code";
                                cmd.Parameters.AddWithValue("@category_code", category_id);
                            }
                            if (brand_id != "")
                            {
                                query += " AND P.brand_code = @brand_code";
                                cmd.Parameters.AddWithValue("@brand_code", brand_id);
                            }
                            if (group_code != "")
                            {
                                query += " AND P.group_code = @group_code";
                                cmd.Parameters.AddWithValue("@group_code", group_code);
                            }
                            query += " ORDER BY P.id desc";

                            //cmd.Parameters.AddWithValue("@id", condition);
                            cmd.Parameters.AddWithValue("@code", string.Format("%{0}%", condition));
                            //cmd.Parameters.AddWithValue("@name", string.Format("%{0}%", condition));
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            cmd.CommandText = query;
                            cmd.Connection = cn;
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }
                        }

                        return dt;
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception as necessary
                        throw new Exception("Error: " + ex.Message);
                    }
                }
            }
        }

        //
        public DataTable SearchProductByLocations(string condition, string category_id, string brand_id, string group_code, string fromLocation, string toLocation, bool qty_onhand)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable();

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            // Base query
                            string query = @"SELECT p.id, p.item_number,p.part_number, p.code, p.name, p.name_ar, p.category_code, 
                                    p.item_type, p.brand_code, p.status, p.barcode, p.avg_cost, p.cost_price, 
                                    p.unit_price, p.unit_price_2, p.tax_id, P.location_code, p.unit_id, 
                                    p.re_stock_level, p.description, p.deleted, p.date_created, p.date_updated, 
                                    p.user_id, p.demand_qty, p.purchase_demand_qty, p.sale_demand_qty, p.origin, 
                                    p.group_code, p.alt_no, p.picture, p.packet_qty,
                                    COALESCE((SELECT TOP 1 COALESCE(s.qty,0) 
                                    FROM pos_product_stocks s WITH (NOLOCK)
                                    WHERE s.item_number=p.item_number AND s.branch_id=@branch_id),0) as qty,
                                    C.name AS category, C.id AS category_id
                                    FROM pos_products AS P WITH (NOLOCK)
                                    LEFT JOIN pos_categories C WITH (NOLOCK) ON C.code=P.category_code
                                    WHERE p.deleted=0 ";


                            if (fromLocation == "All" && toLocation == "All")
                            {
                                query += ""; //search all locations
                            }
                            else if (!string.IsNullOrEmpty(fromLocation) && fromLocation != "all")
                            {
                                query += " AND p.location_code >= @fromLocation AND p.location_code <= @toLocation";

                            }
                            if (qty_onhand)
                            {
                                query += " AND COALESCE((select TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=P.item_number and s.branch_id=@branch_id),0) > 0";
                            }

                            cmd.Parameters.AddWithValue("@fromLocation", fromLocation);
                            cmd.Parameters.AddWithValue("@toLocation", toLocation);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            // Handle search condition
                            if (!string.IsNullOrEmpty(condition))
                            {
                                // First try with CONTAINS (more efficient if full-text index exists)
                                string containsClause = BuildContainsClause(condition);

                                // Combine both approaches with OR
                                query += " AND (" + containsClause + ")";

                                //string containsClause1 = OptimizeSearchTerm(condition);

                                // Fallback for short words (full-text ignores words < 3 chars)
                                if (condition.Length > 3)
                                {
                                    string likeClause = BuildLikeClause(condition,
                                    new[] { "P.code" }); //{ "P.name", "P.code", "P.item_number", "P.description" }

                                    query += " OR " + likeClause;
                                    // Add LIKE pattern parameter
                                    cmd.Parameters.AddWithValue("@likePattern", "%" + condition + "%");

                                    //query += " OR P.code LIKE @exactCode";
                                    //cmd.Parameters.AddWithValue("@exactCode", "%" + condition + "%");
                                }
                            }

                            // Add optional filters
                            if (!string.IsNullOrEmpty(category_id))
                            {
                                query += " AND P.category_code = @category_code";
                                cmd.Parameters.AddWithValue("@category_code", category_id);
                            }
                            if (!string.IsNullOrEmpty(brand_id))
                            {
                                query += " AND P.brand_code = @brand_code";
                                cmd.Parameters.AddWithValue("@brand_code", brand_id);
                            }
                            if (!string.IsNullOrEmpty(group_code))
                            {
                                query += " AND P.group_code = @group_code";
                                cmd.Parameters.AddWithValue("@group_code", group_code);
                            }

                            //query += " ORDER BY qty DESC";
                            query = "SELECT TOP 1000 * FROM (" + query + ") AS Results ORDER BY name ASC";

                            cmd.CommandText = query;
                            cmd.Connection = cn;

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }
                        }

                        return dt;
                    }
                    catch (Exception ex)
                    {
                        // Log error and return empty table or rethrow
                        // Consider logging the actual query for debugging
                        throw new ApplicationException("Search failed. See inner exception for details. " + ex, ex);
                    }
                }
            }
        }

        public DataTable GetAllProductCodes()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    DataTable dt = new DataTable(); // Instantiate DataTable

                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            cmd.CommandText = @"SELECT code, name FROM pos_products WHERE deleted != '1' ORDER BY name";
                            cmd.Connection = cn;
                            //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }
                        }

                        return dt;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }


        public bool IsProductExist(string productCode, string category_code)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable(); // Instantiate DataTable

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            cmd.CommandText = @"SELECT code FROM pos_products WHERE code = @productCode AND category_code=@category_code AND deleted=0";
                            cmd.Connection = cn;
                            cmd.Parameters.AddWithValue("@productCode", productCode);
                            cmd.Parameters.AddWithValue("@category_code", category_code);
                            //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }

                        }

                        if (dt.Rows.Count > 0)
                        {
                            return true;
                        }
                        return false;

                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception as necessary
                        throw new Exception("Error: " + ex.Message);
                    }
                }
            }
        }
        public bool CheckDuplicateBarcode(string barcode)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable(); // Instantiate DataTable

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = @"SELECT barcode FROM pos_products WHERE barcode = @barcode";
                            cmd.Parameters.AddWithValue("@barcode", barcode);
                            //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }

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
        }

        public DataTable GetAllProductNamesOnly()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable(); // Instantiate DataTable

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            cmd.Connection = cn;
                            cmd.CommandText = @"SELECT name FROM pos_products WHERE deleted != '1'";
                            //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }
                        }

                        return dt;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public DataTable SearchRecordByProductCode_1(string item_number)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable(); // Instantiate DataTable

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = @"SELECT P.id,P.item_number,P.part_number,P.name,P.cost_price,P.unit_price,P.item_type,P.code,P.tax_id,P.category_code," +
                                " COALESCE((select  TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=P.item_number and s.branch_id=@branch_id),0) as qty," + //branch wise qty
                                " T.title AS tax_title,T.rate AS tax_rate," +
                                " U.name AS unit," +
                                " C.name AS category" +
                                " FROM pos_products P" +
                                " LEFT JOIN pos_taxes T ON T.id=P.tax_id" +
                                " LEFT JOIN pos_units U ON U.id=P.unit_id" +
                                " LEFT JOIN pos_categories C ON C.code=P.category_code" +
                                " WHERE P.deleted=0 AND P.item_number = @item_number AND P.item_type = 'Purchased'";

                            cmd.Parameters.AddWithValue("@item_number", item_number);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }

                        }

                        return dt;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public DataTable SearchRecordByProductID(string product_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable(); // Instantiate DataTable

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = @"SELECT P.id,P.code,P.part_number,P.item_number,P.name,P.name_ar,P.brand_code,P.item_type,P.barcode,P.avg_cost,P.tax_id,P.location_code," +
                                " P.unit_price,P.cost_price,P.description,P.group_code,alt_no,demand_qty,purchase_demand_qty,sale_demand_qty," +
                                " COALESCE((select  TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=P.item_number and s.branch_id=@branch_id),0) as qty," + //branch wise qty
                                " P.category_code,P.picture,P.packet_qty," +
                                " T.title AS tax_title,T.rate AS tax_rate," +
                                " U.name AS unit," +
                                " C.name AS category, C.id AS category_id" +
                                " FROM pos_products P" +
                                " LEFT JOIN pos_taxes T ON T.id=P.tax_id" +
                                " LEFT JOIN pos_units U ON U.id=P.unit_id" +
                                " LEFT JOIN pos_categories C ON C.code=P.category_code" +
                                " WHERE p.deleted=0 AND P.id = @id";

                            cmd.Parameters.AddWithValue("@id", product_id);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }
                        }

                        return dt;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }
        public DataTable SearchRecordByProductCode(string product_code)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = @"SELECT P.id,P.code,P.part_number,P.item_number,P.name,P.name_ar,P.brand_code,P.item_type,P.barcode,P.avg_cost,P.tax_id,P.location_code," +
                                " P.unit_price,P.cost_price,P.description,P.group_code,alt_no,demand_qty,purchase_demand_qty,sale_demand_qty," +
                                " COALESCE((select  TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=P.item_number and s.branch_id=@branch_id),0) as qty," + //branch wise qty
                                " P.category_code,P.picture,P.location_code,P.packet_qty," +
                                " T.title AS tax_title,T.rate AS tax_rate," +
                                " U.name AS unit," +
                                " C.name AS category, C.id AS category_id" +
                                " FROM pos_products P" +
                                " LEFT JOIN pos_taxes T ON T.id=P.tax_id" +
                                " LEFT JOIN pos_units U ON U.id=P.unit_id" +
                                " LEFT JOIN pos_categories C ON C.code=P.category_code" +
                                " WHERE p.deleted=0 AND P.code = @code";

                            cmd.Parameters.AddWithValue("@code", product_code);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }
                        }

                        return dt;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public DataTable SearchRecordByBarcode(string barcode)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable(); // Instantiate DataTable

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = @"SELECT P.id,P.code,P.part_number,P.item_number,P.name,P.name_ar,P.brand_code,P.item_type,P.barcode,P.avg_cost,P.tax_id,P.location_code," +
                                " P.unit_price,P.cost_price,P.description,P.group_code,alt_no,demand_qty,purchase_demand_qty,sale_demand_qty," +
                                " COALESCE((select  TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=P.item_number and s.branch_id=@branch_id),0) as qty," + //branch wise qty
                                " P.category_code," +
                               " T.title AS tax_title,T.rate AS tax_rate," +
                               " U.name AS unit," +
                               " C.name AS category, C.id AS category_id" +
                               " FROM pos_products P" +
                               " LEFT JOIN pos_taxes T ON T.id=P.tax_id" +
                               " LEFT JOIN pos_units U ON U.id=P.unit_id" +
                               " LEFT JOIN pos_categories C ON C.code=P.category_code" +
                               " WHERE p.deleted=0 AND P.barcode = @barcode";

                            cmd.Parameters.AddWithValue("@barcode", string.Format("{0}", barcode));
                            //cmd.Parameters.AddWithValue("@name", product_name);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }
                        }

                        return dt;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public DataTable SearchRecordByProductName(string product_name)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable(); // Instantiate DataTable

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = @"SELECT P.id,P.name,P.part_number,P.item_number, P.cost_price,P.unit_price,P.item_type,P.code,P.tax_id,P.category_code," +
                                 " COALESCE((select  TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=P.item_number and s.branch_id=@branch_id),0) as qty," + //branch wise qty
                                " T.title AS tax_title,T.rate AS tax_rate" +
                                " FROM pos_products P" +
                                " LEFT JOIN pos_taxes T ON T.id=P.tax_id" +
                                " WHERE p.deleted=0 AND P.name like @product_name AND P.item_type = 'Purchased'";

                            cmd.Parameters.AddWithValue("@name", string.Format("{0}", product_name));
                            //cmd.Parameters.AddWithValue("@name", product_name);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);


                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }

                        }

                        return dt;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public DataTable SearchRecordByProductNumber(string item_number)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable(); // Instantiate DataTable

                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = @"SELECT P.id,P.item_number,P.part_number,P.code,P.name,P.name_ar,P.brand_code,P.item_type,P.barcode,P.avg_cost,P.tax_id,P.location_code," +
                                " P.unit_price,P.cost_price,P.description,P.group_code,alt_no,demand_qty,purchase_demand_qty,sale_demand_qty," +
                                " COALESCE((select  TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=P.item_number and s.branch_id=@branch_id),0) as qty," + //branch wise qty
                                " P.category_code,P.picture,P.packet_qty,P.item_number_2,P.unit_price_2,P.expiry_date,P.unit_id,P.supplier_id," +
                                " T.title AS tax_title,T.rate AS tax_rate," +
                                " U.name AS unit," +
                                " C.name AS category, C.id AS category_id" +
                                " FROM pos_products P" +
                                " LEFT JOIN pos_taxes T ON T.id=P.tax_id" +
                                " LEFT JOIN pos_units U ON U.id=P.unit_id" +
                                " LEFT JOIN pos_categories C ON C.code=P.category_code" +
                                " WHERE p.deleted=0 AND P.item_number = @item_number";

                            cmd.Parameters.AddWithValue("@item_number", item_number);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);


                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }

                        }

                        return dt;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }
        public DataTable SearchProductBySupplier(string supplierId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = @"SELECT P.id,P.code,P.part_number,P.item_number,P.name,P.name_ar,P.brand_code,P.item_type,P.barcode,P.avg_cost,P.tax_id,P.location_code," +
                                " P.unit_price,P.cost_price,P.description,P.group_code,alt_no,demand_qty,purchase_demand_qty,sale_demand_qty," +
                                " COALESCE((select  TOP 1 COALESCE(s.qty,0) as qty from pos_product_stocks s where s.item_number=P.item_number and s.branch_id=@branch_id),0) as qty," + //branch wise qty
                                " P.category_code,P.picture,P.location_code,P.packet_qty," +
                                " T.title AS tax_title,T.rate AS tax_rate," +
                                " U.name AS unit," +
                                " C.name AS category, C.id AS category_id" +
                                " FROM pos_products P" +
                                " LEFT JOIN pos_taxes T ON T.id=P.tax_id" +
                                " LEFT JOIN pos_units U ON U.id=P.unit_id" +
                                " LEFT JOIN pos_categories C ON C.code=P.category_code" +
                                " WHERE P.supplier_id = @supplierId AND P.deleted = 0";

                            cmd.Parameters.AddWithValue("@supplierId", supplierId);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }
                        }

                        return dt;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public DataTable Get_otherStock(string productID, string item_number)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                DataTable dt = new DataTable();
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    cmd.Connection = cn;
                    cmd.CommandText = "SELECT S.qty,S.item_code, B.name AS branch_name FROM pos_product_stocks S" +
                                      " LEFT JOIN pos_branches B ON S.branch_id=B.id" +
                                      " WHERE (S.item_number = @item_number) AND S.branch_id <> @branch_id";
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@productID", productID);
                    cmd.Parameters.AddWithValue("@item_number", item_number);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error fetching other stock: " + ex.Message, ex);
                }
                return dt;
            }
        }

        public int Insert(ProductModal obj)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ProductsCrud", cn))
                {
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@barcode", obj.barcode);
                            cmd.Parameters.AddWithValue("@part_number", obj.part_number);
                            cmd.Parameters.AddWithValue("@item_number", obj.item_number);
                            cmd.Parameters.AddWithValue("@item_number_2", obj.alt_item_number);
                            cmd.Parameters.AddWithValue("@origin", obj.origin);
                            cmd.Parameters.AddWithValue("@group_code", obj.group_code);
                            cmd.Parameters.AddWithValue("@code", obj.code);
                            cmd.Parameters.AddWithValue("@name", obj.name);
                            cmd.Parameters.AddWithValue("@name_ar", obj.name_ar);
                            cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                            cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                            cmd.Parameters.AddWithValue("@unit_price_2", obj.unit_price_2);
                            cmd.Parameters.AddWithValue("@avg_cost", obj.cost_price);
                            cmd.Parameters.AddWithValue("@item_type", obj.item_type);
                            cmd.Parameters.AddWithValue("@tax_id", obj.tax_id);
                            cmd.Parameters.AddWithValue("@status", 1);
                            cmd.Parameters.AddWithValue("@description", obj.description);
                            cmd.Parameters.AddWithValue("@unit_id", obj.unit_id);
                            cmd.Parameters.AddWithValue("@category_code", obj.category_code);
                            cmd.Parameters.AddWithValue("@location_code", obj.location_code);
                            cmd.Parameters.AddWithValue("@re_stock_level", obj.re_stock_level);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@brand_code", obj.brand_code);

                            cmd.Parameters.AddWithValue("@demand_qty", obj.demand_qty);
                            cmd.Parameters.AddWithValue("@purchase_demand_qty", obj.purchase_demand_qty);
                            cmd.Parameters.AddWithValue("@sale_demand_qty", obj.sale_demand_qty);
                            cmd.Parameters.AddWithValue("@picture", obj.picture);
                            cmd.Parameters.AddWithValue("@expiry_date", obj.expiry_date);
                            cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                            cmd.Parameters.AddWithValue("@packet_qty", obj.packet_qty);

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
                        Log.LogAction("Add Product", $"Product Code: {obj.code}, Product Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return (int)result;
        }

        public int Update(ProductModal obj)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ProductsCrud", cn))
                {
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id", obj.id);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@part_number", obj.part_number);
                            cmd.Parameters.AddWithValue("@item_number_2", obj.alt_item_number);
                            cmd.Parameters.AddWithValue("@origin", obj.origin);
                            cmd.Parameters.AddWithValue("@barcode", obj.barcode);
                            cmd.Parameters.AddWithValue("@group_code", obj.group_code);
                            cmd.Parameters.AddWithValue("@code", obj.code);
                            cmd.Parameters.AddWithValue("@name", obj.name);
                            cmd.Parameters.AddWithValue("@name_ar", obj.name_ar);
                            cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                            cmd.Parameters.AddWithValue("@unit_price_2", obj.unit_price_2);
                            cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                            cmd.Parameters.AddWithValue("@tax_id", obj.tax_id);
                            cmd.Parameters.AddWithValue("@avg_cost", obj.cost_price);
                            cmd.Parameters.AddWithValue("@item_type", obj.item_type);
                            cmd.Parameters.AddWithValue("@status", 1);
                            cmd.Parameters.AddWithValue("@description", obj.description);
                            cmd.Parameters.AddWithValue("@unit_id", obj.unit_id);
                            cmd.Parameters.AddWithValue("@category_code", obj.category_code);
                            cmd.Parameters.AddWithValue("@location_code", obj.location_code);
                            cmd.Parameters.AddWithValue("@re_stock_level", obj.re_stock_level);
                            cmd.Parameters.AddWithValue("@brand_code", obj.brand_code);
                            //cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                            cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);

                            cmd.Parameters.AddWithValue("@demand_qty", obj.demand_qty);
                            cmd.Parameters.AddWithValue("@purchase_demand_qty", obj.purchase_demand_qty);
                            cmd.Parameters.AddWithValue("@sale_demand_qty", obj.sale_demand_qty);
                            cmd.Parameters.AddWithValue("@picture", obj.picture);
                            cmd.Parameters.AddWithValue("@expiry_date", obj.expiry_date);
                            cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                            cmd.Parameters.AddWithValue("@packet_qty", obj.packet_qty);

                            cmd.Parameters.AddWithValue("@OperationType", "2");

                            //--operation types   
                            //-- 1) Insert  
                            //-- 2) Update  
                            //-- 3) Delete  
                            //-- 4) Select Perticular Record  
                            //-- 5) Selec All 
                        }

                        result = Convert.ToInt32(cmd.ExecuteScalar());
                        Log.LogAction("Update Product", $"Product Code: {obj.code}, Product Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return (int)result;
        }

        public int UpdateReorder_level(ProductModal obj)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ProductsCrud", cn))
                {
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id", obj.id);
                            cmd.Parameters.AddWithValue("@location_code", obj.location_code);
                            cmd.Parameters.AddWithValue("@re_stock_level", obj.re_stock_level);

                            cmd.Parameters.AddWithValue("@OperationType", "8");

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
            }
            return (int)result;
        }
        public int BulkUpdate(ProductModal obj)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ProductsCrud", cn))
                {
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id", obj.id);
                            //cmd.Parameters.AddWithValue("@branch_id", 0);
                            //cmd.Parameters.AddWithValue("@qty", obj.qty);
                            //cmd.Parameters.AddWithValue("@adjustment_qty", obj.adjustment_qty);
                            cmd.Parameters.AddWithValue("@code", obj.code);
                            cmd.Parameters.AddWithValue("@item_number", obj.item_number);
                            cmd.Parameters.AddWithValue("@name", obj.name);
                            cmd.Parameters.AddWithValue("@name_ar", obj.name_ar);
                            //cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                            //cmd.Parameters.AddWithValue("@unit_price_2", obj.unit_price_2);
                            cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                            //cmd.Parameters.AddWithValue("@tax_id", obj.tax_id);
                            // cmd.Parameters.AddWithValue("@avg_cost", obj.cost_price);
                            //cmd.Parameters.AddWithValue("@item_type", obj.item_type);
                            //cmd.Parameters.AddWithValue("@status", 1);
                            cmd.Parameters.AddWithValue("@description", obj.description);
                            //cmd.Parameters.AddWithValue("@unit_id", obj.unit_id);
                            //cmd.Parameters.AddWithValue("@category_id", obj.category_id);
                            cmd.Parameters.AddWithValue("@location_code", obj.location_code);
                            //cmd.Parameters.AddWithValue("@brand_id", obj.brand_id);
                            //cmd.Parameters.AddWithValue("@date_created", DateTime.Now);

                            cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                            cmd.Parameters.AddWithValue("@OperationType", "5"); //-- 5) Bulk update

                            //--operation types   
                            //-- 1) Insert  
                            //-- 2) Update  
                            //-- 3) Delete  
                            //-- 4) Select Perticular Record  
                            //-- 5) Bulk update
                        }

                        result = Convert.ToInt32(cmd.ExecuteScalar());
                        Log.LogAction("Bulk Update Product", $"Product Code: {obj.code}, Product Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                        return (int)result;
                    }
                    catch
                    {

                        throw;
                    }
                }
            }
        }

        public string UpdateQtyAdjustment(ProductModal obj)
        {
            string result = "";
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_ProductsCrud", cn))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@item_number", obj.item_number);
                        cmd.Parameters.AddWithValue("@code", obj.code);
                        cmd.Parameters.AddWithValue("@qty", obj.qty);
                        cmd.Parameters.AddWithValue("@adjustment_qty", obj.adjustment_qty);
                        cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                        cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@location_code", obj.location_code);

                        cmd.Parameters.AddWithValue("@OperationType", "7"); //-- 6) qty adjustment update

                    }

                    result = cmd.ExecuteScalar().ToString();
                    Log.LogAction("Update Product Adjustment", $"Product Code: {obj.code}, InvoiceNo: {obj.invoice_no}, Adjustment Qty= {obj.adjustment_qty}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                }
                catch
                {

                    throw;
                }
            }
            return result;
        }

        public String GetMaxAdjustmentInvoiceNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            cmd.Connection = cn;
                            cmd.CommandText = @"SELECT MAX(invoice_no) FROM pos_product_adjustment";

                            string maxId = Convert.ToString(cmd.ExecuteScalar());

                            if (maxId == "")
                            {
                                return maxId = "AD-000001";
                            }
                            else
                            {
                                int intval = int.Parse(maxId.Substring(3, 6));
                                intval++;
                                maxId = String.Format("AD-{0:000000}", intval);
                                return maxId;
                            }

                        }
                        return "";
                    }
                    catch
                    {

                        throw;
                    }
                }
            }

        }

        // Get product adjustment by invoice no
        public DataTable GetProductAdjustmentsByInvoiceNo(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();
                            cmd.Connection = cn;
                            cmd.CommandText = @"SELECT p.name,p.name_ar,p.category_code,p.location_code,p.qty,p.item_type,
                            pa.*,pa.qty as adjustment_qty FROM pos_product_adjustment pa
                            LEFT JOIN pos_products p ON p.item_number=pa.item_number
                            WHERE pa.invoice_no = @invoice_no AND pa.branch_id = @branch_id";
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt);
                            }
                        }
                        return dt;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public int Delete(int ProductId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ProductsCrud", cn))
                {
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id", ProductId);
                            cmd.Parameters.AddWithValue("@OperationType", "3");

                        }

                        int result = cmd.ExecuteNonQuery();
                        Log.LogAction("Delete Product", $"Product ID: {ProductId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                        return result;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }


        public String GetMaxLocationTransferInvoiceNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            cmd.Connection = cn;
                            cmd.CommandText = @"SELECT MAX(invoice_no) FROM pos_product_loc_transfer";

                            string maxId = Convert.ToString(cmd.ExecuteScalar());

                            if (maxId == "")
                            {
                                return maxId = "LT-000001";
                            }
                            else
                            {
                                int intval = int.Parse(maxId.Substring(3, 6));
                                intval++;
                                maxId = String.Format("LT-{0:000000}", intval);
                                return maxId;
                            }
                        }
                        return "";
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public int UpdateProductLocationTransfer(ProductModal obj)
        {
            Int32 result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ProductsCrud", cn))
                {
                    try
                    {
                        if (cn.State == ConnectionState.Closed)
                        {
                            cn.Open();

                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@item_number", obj.item_number);
                            cmd.Parameters.AddWithValue("@code", obj.code);
                            cmd.Parameters.AddWithValue("@id", obj.id);
                            cmd.Parameters.AddWithValue("@qty", obj.qty);
                            cmd.Parameters.AddWithValue("@from_location_code", obj.from_location_code);
                            cmd.Parameters.AddWithValue("@location_code", obj.location_code);
                            cmd.Parameters.AddWithValue("@description", obj.description);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                            cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);

                            cmd.Parameters.AddWithValue("@OperationType", "6"); //-- 6) qty Location Transer of Products update

                        }

                        result = Convert.ToInt32(cmd.ExecuteScalar());
                        Log.LogAction("Update Product Location", $"Product Code: {obj.code},From Loc: {obj.name}, To Loc= {obj.location_code} Qty= {obj.qty}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return (int)result;
        }

        public DataTable SearchProductsPaged(string condition, string category_code, string brand_code_csv, string group_code, int pageIndex, int pageSize)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                DataTable dt = new DataTable();
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    // Base select (only required columns)
                    string select = @"SELECT p.id, p.item_number, p.part_number, p.code, p.name, p.name_ar, p.category_code, 
                                         p.item_type, p.brand_code, p.avg_cost, p.cost_price, p.unit_price, p.unit_price_2,
                                         p.tax_id, p.location_code, p.unit_id, p.re_stock_level, p.description, p.group_code,
                                         p.alt_no, p.packet_qty, ISNULL(bs.branch_qty,0) AS qty,
                                         C.name AS category, C.id AS category_id";

                    // Derived table for branch stock (pre-aggregated for speed)
                    string joinStock = @"LEFT JOIN (
                                            SELECT item_number, SUM(qty) AS branch_qty
                                            FROM pos_product_stocks WITH (NOLOCK)
                                            WHERE branch_id=@branch_id
                                            GROUP BY item_number
                                        ) bs ON bs.item_number = p.item_number";
                    string from = @"FROM pos_products p WITH (NOLOCK)
                                   LEFT JOIN pos_categories C WITH (NOLOCK) ON C.code = p.category_code
                                   " + joinStock;

                    StringBuilder where = new StringBuilder();
                    where.Append("WHERE p.deleted=0");

                    // Filters
                    if (!string.IsNullOrEmpty(category_code))
                    {
                        where.Append(" AND p.category_code = @category_code");
                        cmd.Parameters.AddWithValue("@category_code", category_code);
                    }
                    if (!string.IsNullOrEmpty(group_code))
                    {
                        where.Append(" AND p.group_code = @group_code");
                        cmd.Parameters.AddWithValue("@group_code", group_code);
                    }
                    if (!string.IsNullOrEmpty(brand_code_csv))
                    {
                        var brands = brand_code_csv.Split(',').Select(b => b.Trim()).Where(b => b.Length > 0).ToList();
                        if (brands.Count > 0)
                        {
                            if (brands.Count == 1)
                            {
                                where.Append(" AND p.brand_code = @brand_code");
                                cmd.Parameters.AddWithValue("@brand_code", brands[0]);
                            }
                            else
                            {
                                var inParams = new List<string>();
                                for (int i = 0; i < brands.Count; i++)
                                {
                                    string pname = "@brand" + i;
                                    inParams.Add(pname);
                                    cmd.Parameters.AddWithValue(pname, brands[i]);
                                }
                                where.Append(" AND p.brand_code IN (" + string.Join(",", inParams) + ")");
                            }
                        }
                    }

                    // Search condition (debounced externally). Prefer LIKE with leading pattern only for index usage.
                    if (!string.IsNullOrWhiteSpace(condition))
                    {
                        var words = condition.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                              .Where(w => w.Length > 0).ToList();
                        if (words.Count == 1 && words[0].Length < 3)
                        {
                            where.Append(" AND (p.code LIKE @term OR p.part_number LIKE @term OR p.name LIKE @term)");
                            cmd.Parameters.AddWithValue("@term", words[0] + "%");
                        }
                        else if (words.Count == 1)
                        {
                            where.Append(" AND (p.name LIKE @termLong OR p.code LIKE @termLike OR p.part_number LIKE @termLike OR p.description LIKE @termDesc)");
                            cmd.Parameters.AddWithValue("@termLong", words[0] + "%");
                            cmd.Parameters.AddWithValue("@termLike", "%" + words[0] + "%");
                            cmd.Parameters.AddWithValue("@termDesc", "%" + words[0] + "%");
                        }
                        else
                        {
                            // multi-word: each word must appear in ANY of the searchable columns; build AND groups
                            // ( (col LIKE %w1%) AND (col LIKE %w2%) ... ) across combined columns
                            int idx = 0;
                            where.Append(" AND (");
                            foreach (var w in words)
                            {
                                string pName = "@w" + idx++;
                                // Short tokens (<3) use prefix match to allow partial typing
                                if (w.Length < 3)
                                {
                                    cmd.Parameters.AddWithValue(pName, w + "%");
                                    where.Append(" (p.name LIKE " + pName + " OR p.code LIKE " + pName + " OR p.part_number LIKE " + pName + " OR p.description LIKE " + pName + ") AND");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue(pName, "%" + w + "%");
                                    where.Append(" (p.name LIKE " + pName + " OR p.code LIKE " + pName + " OR p.part_number LIKE " + pName + " OR p.description LIKE " + pName + ") AND");
                                }
                            }
                            where.Length -= 4; // remove trailing AND
                            where.Append(")");
                        }
                    }

                    string orderPaging = " ORDER BY p.name ASC, p.id ASC OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
                    cmd.Parameters.AddWithValue("@offset", pageIndex * pageSize);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    cmd.CommandText = select + " " + from + " " + where.ToString() + orderPaging;
                    cmd.Connection = cn;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Paged search failed: " + ex.Message, ex);
                }
                return dt;
            }
        }

        public DataTable SearchProductsPagedWithCount(string condition, string category_code, string brand_code_csv, string group_code, int pageIndex, int pageSize, out int totalCount)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                DataTable dt = new DataTable();
                totalCount = 0;
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    StringBuilder where = new StringBuilder("WHERE p.deleted=0");

                    if (!string.IsNullOrEmpty(category_code))
                    {
                        where.Append(" AND p.category_code = @category_code");
                        cmd.Parameters.AddWithValue("@category_code", category_code);
                    }
                    if (!string.IsNullOrEmpty(group_code))
                    {
                        where.Append(" AND p.group_code = @group_code");
                        cmd.Parameters.AddWithValue("@group_code", group_code);
                    }
                    if (!string.IsNullOrEmpty(brand_code_csv))
                    {
                        var brands = brand_code_csv.Split(',').Select(b => b.Trim()).Where(b => b.Length > 0).ToList();
                        if (brands.Count > 0)
                        {
                            if (brands.Count == 1)
                            {
                                where.Append(" AND p.brand_code = @brand_code");
                                cmd.Parameters.AddWithValue("@brand_code", brands[0]);
                            }
                            else
                            {
                                var inParams = new List<string>();
                                for (int i = 0; i < brands.Count; i++)
                                {
                                    string pname = "@brand" + i;
                                    inParams.Add(pname);
                                    cmd.Parameters.AddWithValue(pname, brands[i]);
                                }
                                where.Append(" AND p.brand_code IN (" + string.Join(",", inParams) + ")");
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(condition))
                    {
                        var words = condition.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                              .Where(w => w.Length > 0).ToList();
                        if (words.Count > 1)
                        {
                            int idx = 0;
                            where.Append(" AND (");
                            foreach (var w in words)
                            {
                                string pName = "@w" + idx++;
                                if (w.Length < 3)
                                {
                                    cmd.Parameters.AddWithValue(pName, w + "%");
                                    where.Append(" (p.name LIKE " + pName + " OR p.code LIKE " + pName + " OR p.part_number LIKE " + pName + " OR p.description LIKE " + pName + ") AND");
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue(pName, "%" + w + "%");
                                    where.Append(" (p.name LIKE " + pName + " OR p.code LIKE " + pName + " OR p.part_number LIKE " + pName + " OR p.description LIKE " + pName + ") AND");
                                }
                            }
                            where.Length -= 4; // remove trailing AND
                            where.Append(")");
                        }
                        else
                        {
                            string single = words[0];
                            if (single.Length < 3)
                            {
                                where.Append(" AND (p.code LIKE @term OR p.part_number LIKE @term OR p.name LIKE @term)");
                                cmd.Parameters.AddWithValue("@term", single + "%");
                            }
                            else
                            {
                                where.Append(" AND (p.name LIKE @termLong OR p.code LIKE @termLike OR p.part_number LIKE @termLike OR p.description LIKE @termDesc)");
                                cmd.Parameters.AddWithValue("@termLong", single + "%");
                                cmd.Parameters.AddWithValue("@termLike", "%" + single + "%");
                                cmd.Parameters.AddWithValue("@termDesc", "%" + single + "%");
                                //cmd.Parameters.AddWithValue("@barcodeExact", single);
                            }
                        }
                    }

                    using (SqlCommand countCmd = new SqlCommand("SELECT COUNT(1) FROM pos_products p " + where.ToString(), cn))
                    {
                        foreach (SqlParameter p in cmd.Parameters)
                            countCmd.Parameters.Add(new SqlParameter(p.ParameterName, p.Value));
                        totalCount = Convert.ToInt32(countCmd.ExecuteScalar());
                    }

                    string select = @"SELECT p.id, p.item_number, p.part_number, p.code, p.name, p.name_ar, p.category_code, 
                                         p.item_type, p.brand_code, p.avg_cost, p.cost_price, p.unit_price, p.unit_price_2,
                                         p.tax_id, p.location_code, p.unit_id, p.re_stock_level, p.description, p.group_code,
                                         p.alt_no, p.packet_qty, ISNULL(bs.branch_qty,0) AS qty,
                                         C.name AS category, C.id AS category_id";

                    string joinStock = @"LEFT JOIN (
                                            SELECT item_number, SUM(qty) AS branch_qty
                                            FROM pos_product_stocks WITH (NOLOCK)
                                            WHERE branch_id=@branch_id
                                            GROUP BY item_number
                                        ) bs ON bs.item_number = p.item_number";
                    string from = @"FROM pos_products p WITH (NOLOCK)
                                   LEFT JOIN pos_categories C WITH (NOLOCK) ON C.code = p.category_code
                                   " + joinStock;

                    string orderPaging = " ORDER BY p.name ASC, p.id ASC OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";
                    cmd.Parameters.AddWithValue("@offset", pageIndex * pageSize);
                    cmd.Parameters.AddWithValue("@pageSize", pageSize);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    cmd.CommandText = select + " " + from + " " + where.ToString() + orderPaging;
                    cmd.Connection = cn;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Paged search with count failed: " + ex.Message, ex);
                }
                return dt;
            }
        }

        /// <summary>
        /// New methods for POS product operations
        /// </summary>

        private dbConnection dbHelper = new dbConnection();

        public DataTable SearchProducts(string searchTerm, int branchId)
        {
            string query = @"
                SELECT 
                    code,item_number, name, name_ar, barcode, unit_price, 
                    qty, unit_price_2, cost_price, avg_cost,unit_id,tax_id,packet_qty, brand_code,
                    category_code, item_type
                FROM pos_products 
                WHERE (branch_id = @BranchId OR branch_id IS NULL) 
                AND (deleted = 0 OR deleted IS NULL)
                AND (name LIKE @SearchTerm OR code LIKE @SearchTerm 
                    OR barcode LIKE @SearchTerm OR name_ar LIKE @SearchTerm)";

            SqlParameter[] parameters = {
                new SqlParameter("@BranchId", branchId),
                new SqlParameter("@SearchTerm", $"%{searchTerm}%")
            };

            return dbHelper.ExecuteQuery(query, parameters);
        }

        public DataTable GetProductByBarcode(string barcode, int branchId)
        {
            string query = @"
                SELECT 
                    code,item_number, name, name_ar, barcode, unit_price, 
                    qty, unit_price_2, cost_price,  avg_cost,packet_qty, brand_code,
                    category_code, item_type, tax_id, unit_id
                FROM pos_products 
                WHERE (branch_id = @BranchId OR branch_id IS NULL) 
                AND barcode = @Barcode
                AND (deleted = 0 OR deleted IS NULL)";

            SqlParameter[] parameters = {
                new SqlParameter("@BranchId", branchId),
                new SqlParameter("@Barcode", barcode)
            };

            return dbHelper.ExecuteQuery(query, parameters);
        }

        public bool UpdateProductQuantity(string ItemNumber, decimal quantity, int branchId)
        {
            string query = @"
                UPDATE pos_product_stocks
                SET qty = qty - @Quantity,
                    date_updated = GETDATE()
                WHERE item_number = @ItemNumber
                AND (branch_id = @BranchId OR branch_id IS NULL)";

            SqlParameter[] parameters = {
                new SqlParameter("@Quantity", quantity),
                new SqlParameter("@ItemNumber", ItemNumber),
                new SqlParameter("@BranchId", branchId)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public DataTable GetProductStock(string itemNumber, int branchId, string locationCode = null)
        {
            string query = @"
                SELECT 
                    ps.item_code, ps.qty, ps.loc_code, ps.reorder_level,
                    p.name, p.unit_price, p.cost_price, p.barcode,
                    p.category_code, p.brand_code, p.unit_id, p.tax_id
                FROM pos_product_stocks ps
                INNER JOIN pos_products p ON ps.item_number = p.item_number
                WHERE ps.item_number = @ItemNumber
                AND ps.branch_id = @BranchId
                AND (ps.loc_code = @LocationCode OR @LocationCode IS NULL)
                AND (p.deleted = 0 OR p.deleted IS NULL)";

            SqlParameter[] parameters = {
                new SqlParameter("@ItemNumber", itemNumber),
                new SqlParameter("@BranchId", branchId),
                new SqlParameter("@LocationCode", (object)locationCode ?? DBNull.Value)
            };

            return dbHelper.ExecuteQuery(query, parameters);
        }

        public bool UpdateProductStock(string ItemNumber, int branchId, decimal quantityChange,
                                     string locationCode, int userId, string transactionType)
        {
            using (SqlConnection conn = dbHelper.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Update or Insert into pos_product_stocks
                        string stockQuery = @"
                            IF EXISTS (SELECT 1 FROM pos_product_stocks 
                                      WHERE item_number = @ItemNumber 
                                      AND branch_id = @BranchId 
                                      )
                            BEGIN
                                UPDATE pos_product_stocks 
                                SET qty = qty + @QuantityChange,
                                    date_updated = GETDATE(),
                                    user_id = @UserId
                                WHERE item_number = @ItemNumber 
                                AND branch_id = @BranchId 
                                
                            END
                            ELSE
                            BEGIN
                                INSERT INTO pos_product_stocks (
                                    branch_id, user_id, loc_code, item_number, 
                                    qty, date_created, date_updated
                                ) VALUES (
                                    @BranchId, @UserId, @LocationCode, @ItemNumber,
                                    @QuantityChange, GETDATE(), GETDATE()
                                )
                            END";

                        SqlParameter[] stockParams = {
                            new SqlParameter("@ItemNumber", ItemNumber),
                            new SqlParameter("@BranchId", branchId),
                            new SqlParameter("@LocationCode", locationCode),
                            new SqlParameter("@QuantityChange", quantityChange),
                            new SqlParameter("@UserId", userId)
                        };

                        using (SqlCommand cmd = new SqlCommand(stockQuery, conn, transaction))
                        {
                            cmd.Parameters.AddRange(stockParams);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public decimal GetCurrentStock(string ItemNumber, int branchId, string locationCode = null)
        {
            string query = @"
                SELECT ISNULL(SUM(qty), 0) as current_stock
                FROM pos_product_stocks 
                WHERE item_number = @ItemNumber
                AND branch_id = @BranchId
                ";

            SqlParameter[] parameters = {
                new SqlParameter("@ItemNumber", ItemNumber),
                new SqlParameter("@BranchId", branchId),
                new SqlParameter("@LocationCode", (object)locationCode ?? DBNull.Value)
            };

            object result = dbHelper.ExecuteScalar(query, parameters);
            return Convert.ToDecimal(result);
        }

        public bool ValidateStockAvailability(string ItemNumber, int branchId, decimal requestedQuantity, string locationCode = null)
        {
            decimal currentStock = GetCurrentStock(ItemNumber, branchId, locationCode);
            return currentStock >= requestedQuantity;
        }


        public bool ProcessSaleTransaction(string productCode, int branchId, decimal quantity,
                                         decimal unitPrice, decimal costPrice, int userId,
                                         string invoiceNo, string itemNumber, int? customerId = null,
                                         string locationCode = "MAIN")
        {
            //using (var transaction = new System.Transactions.TransactionScope())

            try
            {
                // Update stock (negative quantity for sale)
                bool stockUpdated = UpdateProductStock(
                    itemNumber, branchId, -quantity, locationCode, userId, "SALE");

                if (!stockUpdated)
                    return false;

                // Record inventory transaction
                bool inventoryRecorded = inventoryDAL.RecordInventoryTransaction(
                    productCode, -quantity, costPrice, unitPrice, branchId, userId,
                    $"Sale - Invoice: {invoiceNo}", invoiceNo, "SALE", itemNumber,
                    customerId, null, locationCode);

                if (!inventoryRecorded)
                    return false;

                //transaction.Complete();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing sale transaction: {ex.Message}", ex);
            }

        }
    }
}
