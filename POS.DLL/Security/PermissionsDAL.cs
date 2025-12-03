using POS.DLL;
using System;
using System.Data;
using System.Data.SqlClient;

namespace pos.DAL
{
    public class PermissionsDAL
    {
        public DataTable GetAll()
        {
            using (var con = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand("SELECT id, permission_name FROM Permissions ORDER BY permission_name", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                con.Open();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable Search(string keyword)
        {
            using (var con = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand("SELECT id, permission_name FROM Permissions WHERE permission_name LIKE @kw ORDER BY permission_name", con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@kw", "%" + (keyword ?? "").Trim() + "%");
                var dt = new DataTable();
                con.Open();
                da.Fill(dt);
                return dt;
            }
        }

        public int Insert(string permissionName)
        {
            using (var con = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand("INSERT INTO Permissions(permission_name) VALUES(@name)", con))
            {
                cmd.Parameters.AddWithValue("@name", permissionName);
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Update(int id, string permissionName)
        {
            using (var con = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand("UPDATE Permissions SET permission_name = @name WHERE id = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", permissionName);
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public int Delete(int id)
        {
            using (var con = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand("DELETE FROM Permissions WHERE id = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}