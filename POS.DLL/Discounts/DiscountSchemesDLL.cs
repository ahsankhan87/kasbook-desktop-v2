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
                SELECT *
                FROM pos_discount_schemes
                WHERE branch_id = @branch_id
                ORDER BY id DESC";

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

        public DataTable GetAllActive(int branchId)
        {
            string query = @"
                SELECT id, name, name_ar, calc_type, value
                FROM pos_discount_schemes
                WHERE branch_id = @branch_id
                  AND is_active = 1
                  AND (start_date IS NULL OR start_date <= CAST(GETDATE() AS DATE))
                  AND (end_date   IS NULL OR end_date   >= CAST(GETDATE() AS DATE))
                ORDER BY name";

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
            string query = @"SELECT * FROM pos_discount_schemes WHERE id = @id";

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

        public DataTable GetActiveById(int schemeId)
        {
            string query = @"
                SELECT *
                FROM pos_discount_schemes
                WHERE id = @id
                  AND is_active = 1
                  AND (start_date IS NULL OR start_date <= CAST(GETDATE() AS DATE))
                  AND (end_date   IS NULL OR end_date   >= CAST(GETDATE() AS DATE))";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@id", schemeId);
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
                (name,name_ar,calc_type,value,is_active,start_date,end_date,branch_id,company_id,created_by,created_at,updated_at)
                VALUES
                (@name,@name_ar,@calc_type,@value,@is_active,@start_date,@end_date,@branch_id,@company_id,@created_by,GETDATE(),GETDATE());
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.AddWithValue("@name", info.name);
                cmd.Parameters.AddWithValue("@name_ar", (object)info.name_ar ?? DBNull.Value);
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
    }
}
