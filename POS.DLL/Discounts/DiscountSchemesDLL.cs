using POS.Core;
using System;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    public class DiscountSchemesDLL
    {
        public DataTable GetAll(int branchId)
        {
            string query = @"
                SELECT DS.*,
                       CASE
                            WHEN DS.product_id IS NOT NULL THEN ISNULL(P.name, '')
                            WHEN DS.brand_id IS NOT NULL THEN ISNULL(B.name, '')
                            WHEN DS.category_id IS NOT NULL THEN ISNULL(C.name, '')
                            ELSE ''
                       END AS target_name,
                       P.name  AS product_name,
                       B.name  AS brand_name,
                       C.name  AS category_name
                FROM pos_discount_schemes DS
                LEFT JOIN pos_products   P ON P.id = DS.product_id
                LEFT JOIN pos_brands     B ON B.id = DS.brand_id
                LEFT JOIN pos_categories C ON C.id = DS.category_id
                WHERE DS.branch_id = @branch_id
                ORDER BY DS.id DESC";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", branchId);
                DataTable dt = new DataTable();
                cn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetById(int id)
        {
            string query = @"
                SELECT *
                FROM pos_discount_schemes
                WHERE id = @id";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                DataTable dt = new DataTable();
                cn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dt);
                return dt;
            }
        }

        public int Insert(DiscountSchemeModal info)
        {
            string query = @"
                INSERT INTO pos_discount_schemes
                (name,name_ar,product_id,brand_id,category_id,calc_type,value,is_active,start_date,end_date,branch_id,company_id,created_by,created_at,updated_at)
                VALUES
                (@name,@name_ar,@product_id,@brand_id,@category_id,@calc_type,@value,@is_active,@start_date,@end_date,@branch_id,@company_id,@created_by,GETDATE(),GETDATE());
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@name", info.name);
                cmd.Parameters.AddWithValue("@name_ar", (object)info.name_ar ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@product_id", (object)info.product_id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@brand_id", (object)info.brand_id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@category_id", (object)info.category_id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@calc_type", info.calc_type);
                cmd.Parameters.AddWithValue("@value", info.value);
                cmd.Parameters.AddWithValue("@is_active", info.is_active);
                cmd.Parameters.AddWithValue("@start_date", (object)info.start_date ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@end_date", (object)info.end_date ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@branch_id", info.branch_id);
                cmd.Parameters.AddWithValue("@company_id", info.company_id);
                cmd.Parameters.AddWithValue("@created_by", info.created_by);

                cn.Open();
                var result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public int Update(DiscountSchemeModal info)
        {
            string query = @"
                UPDATE pos_discount_schemes SET
                    name=@name,
                    name_ar=@name_ar,
                    product_id=@product_id,
                    brand_id=@brand_id,
                    category_id=@category_id,
                    calc_type=@calc_type,
                    value=@value,
                    is_active=@is_active,
                    start_date=@start_date,
                    end_date=@end_date,
                    updated_at=GETDATE()
                WHERE id=@id;
                SELECT @id;";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@id", info.id);
                cmd.Parameters.AddWithValue("@name", info.name);
                cmd.Parameters.AddWithValue("@name_ar", (object)info.name_ar ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@product_id", (object)info.product_id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@brand_id", (object)info.brand_id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@category_id", (object)info.category_id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@calc_type", info.calc_type);
                cmd.Parameters.AddWithValue("@value", info.value);
                cmd.Parameters.AddWithValue("@is_active", info.is_active);
                cmd.Parameters.AddWithValue("@start_date", (object)info.start_date ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@end_date", (object)info.end_date ?? DBNull.Value);

                cn.Open();
                cmd.ExecuteScalar();
                return info.id;
            }
        }

        public int Delete(int id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("DELETE FROM pos_discount_schemes WHERE id=@id; SELECT @id;", cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                cmd.ExecuteScalar();
                return id;
            }
        }

        public int ToggleActive(int id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"
                UPDATE pos_discount_schemes
                SET is_active = CASE WHEN is_active = 1 THEN 0 ELSE 1 END,
                    updated_at = GETDATE()
                WHERE id = @id;
                SELECT @id;", cn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cn.Open();
                cmd.ExecuteScalar();
                return id;
            }
        }

        public DataTable GetActiveForItem(int productId, int? brandId, int? categoryId, int branchId)
        {
            string query = @"
                SELECT *
                FROM pos_discount_schemes
                WHERE is_active = 1
                  AND branch_id = @branch_id
                  AND (start_date IS NULL OR start_date <= CAST(GETDATE() AS DATE))
                  AND (end_date   IS NULL OR end_date   >= CAST(GETDATE() AS DATE))
                  AND (
                        (product_id  IS NOT NULL AND product_id  = @product_id)
                     OR (brand_id    IS NOT NULL AND brand_id    = @brand_id)
                     OR (category_id IS NOT NULL AND category_id = @category_id)
                  )
                ORDER BY
                    CASE WHEN product_id  IS NOT NULL THEN 1
                         WHEN brand_id    IS NOT NULL THEN 2
                         WHEN category_id IS NOT NULL THEN 3
                         ELSE 4 END";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", branchId);
                cmd.Parameters.AddWithValue("@product_id", productId);
                cmd.Parameters.AddWithValue("@brand_id", (object)brandId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@category_id", (object)categoryId ?? DBNull.Value);
                DataTable dt = new DataTable();
                cn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    da.Fill(dt);
                return dt;
            }
        }
    }
}
