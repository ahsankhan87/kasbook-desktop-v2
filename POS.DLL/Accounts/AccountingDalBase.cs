using System;
using System.Data;
using System.Data.SqlClient;
using POS.Core;

namespace POS.DLL
{
    public abstract class AccountingDalBase
    {
        protected readonly string ConnectionString;

        protected AccountingDalBase()
            : this(dbConnection.ConnectionString)
        {
        }

        protected AccountingDalBase(string connectionString)
        {
            ConnectionString = string.IsNullOrWhiteSpace(connectionString) ? dbConnection.ConnectionString : connectionString;
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        protected static void AddParameter(SqlCommand cmd, string name, object value)
        {
            cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
        }

        protected static object DbValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value.Trim();
        }

        protected static object DbValue(DateTime? value)
        {
            return value.HasValue ? (object)value.Value : DBNull.Value;
        }

        protected static object DbValue(int? value)
        {
            return value.HasValue ? (object)value.Value : DBNull.Value;
        }

        protected static object DbValue(decimal? value)
        {
            return value.HasValue ? (object)value.Value : DBNull.Value;
        }

        protected static object DbValue(bool value)
        {
            return value ? 1 : 0;
        }

        protected DataTable ExecuteDataTable(string sql, Action<SqlCommand> configure = null)
        {
            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                configure?.Invoke(cmd);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        protected T ExecuteScalar<T>(string sql, Action<SqlCommand> configure = null)
        {
            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                configure?.Invoke(cmd);
                object value = cmd.ExecuteScalar();
                if (value == null || value == DBNull.Value)
                {
                    return default(T);
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
        }

        protected int ExecuteNonQuery(string sql, Action<SqlCommand> configure = null)
        {
            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                configure?.Invoke(cmd);
                return cmd.ExecuteNonQuery();
            }
        }

        protected static string NormalizeText(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
