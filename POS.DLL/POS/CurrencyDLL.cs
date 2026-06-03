using POS.Core;
using System;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    public class CurrencyDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;

        private const string EnsureTableSql = @"
IF OBJECT_ID('dbo.pos_currencies', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.pos_currencies
    (
        id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        branch_id INT NULL,
        code NVARCHAR(20) NOT NULL,
        name NVARCHAR(100) NOT NULL,
        symbol NVARCHAR(20) NULL,
        exchange_rate DECIMAL(18,6) NOT NULL CONSTRAINT DF_pos_currencies_exchange_rate DEFAULT(1),
        is_active BIT NOT NULL CONSTRAINT DF_pos_currencies_is_active DEFAULT(1),
        user_id INT NULL,
        date_created DATETIME NOT NULL CONSTRAINT DF_pos_currencies_date_created DEFAULT(GETDATE()),
        date_updated DATETIME NULL
    );

    CREATE UNIQUE INDEX UX_pos_currencies_code ON dbo.pos_currencies(code);
END";

        private static void EnsureSchema(SqlConnection cn)
        {
            using (var schemaCmd = new SqlCommand(EnsureTableSql, cn))
            {
                schemaCmd.ExecuteNonQuery();
            }
        }

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                DataTable dt = new DataTable();
                cn.Open();
                EnsureSchema(cn);

                cmd = new SqlCommand("SELECT id, code, name, symbol, exchange_rate, is_active, date_created FROM dbo.pos_currencies ORDER BY id DESC", cn);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable SearchRecordByCurrencyID(int currencyId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                DataTable dt = new DataTable();
                cn.Open();
                EnsureSchema(cn);

                cmd = new SqlCommand("SELECT id, code, name, symbol, exchange_rate, is_active, date_created FROM dbo.pos_currencies WHERE id = @id", cn);
                cmd.Parameters.AddWithValue("@id", currencyId);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable SearchRecord(string condition)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                DataTable dt = new DataTable();
                cn.Open();
                EnsureSchema(cn);

                cmd = new SqlCommand("SELECT id, code, name, symbol, exchange_rate, is_active, date_created FROM dbo.pos_currencies WHERE code LIKE @keyword OR name LIKE @keyword OR symbol LIKE @keyword ORDER BY id DESC", cn);
                cmd.Parameters.AddWithValue("@keyword", "%" + (condition ?? string.Empty) + "%");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
        }

        public int Insert(CurrencyModal obj)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                EnsureSchema(cn);

                cmd = new SqlCommand(@"
INSERT INTO dbo.pos_currencies
(
    branch_id,
    code,
    name,
    symbol,
    exchange_rate,
    is_active,
    user_id,
    date_created
)
VALUES
(
    @branch_id,
    @code,
    @name,
    @symbol,
    @exchange_rate,
    @is_active,
    @user_id,
    @date_created
);
SELECT CAST(SCOPE_IDENTITY() AS INT);", cn);

                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@code", obj.code);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@symbol", (object)(obj.symbol ?? string.Empty));
                cmd.Parameters.AddWithValue("@exchange_rate", obj.exchange_rate);
                cmd.Parameters.AddWithValue("@is_active", obj.is_active);
                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);

                int result = Convert.ToInt32(cmd.ExecuteScalar());
                Log.LogAction("Add Currency", $"Currency Code: {obj.code}, Currency Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return result;
            }
        }

        public int Update(CurrencyModal obj)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                EnsureSchema(cn);

                cmd = new SqlCommand(@"
UPDATE dbo.pos_currencies
SET
    code = @code,
    name = @name,
    symbol = @symbol,
    exchange_rate = @exchange_rate,
    is_active = @is_active,
    user_id = @user_id,
    date_updated = @date_updated
WHERE id = @id;
SELECT @@ROWCOUNT;", cn);

                cmd.Parameters.AddWithValue("@id", obj.id);
                cmd.Parameters.AddWithValue("@code", obj.code);
                cmd.Parameters.AddWithValue("@name", obj.name);
                cmd.Parameters.AddWithValue("@symbol", (object)(obj.symbol ?? string.Empty));
                cmd.Parameters.AddWithValue("@exchange_rate", obj.exchange_rate);
                cmd.Parameters.AddWithValue("@is_active", obj.is_active);
                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);

                int result = Convert.ToInt32(cmd.ExecuteScalar());
                Log.LogAction("Update Currency", $"Currency Code: {obj.code}, Currency Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return result;
            }
        }

        public int Delete(int currencyId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                EnsureSchema(cn);

                cmd = new SqlCommand("DELETE FROM dbo.pos_currencies WHERE id = @id; SELECT @@ROWCOUNT;", cn);
                cmd.Parameters.AddWithValue("@id", currencyId);

                int result = Convert.ToInt32(cmd.ExecuteScalar());
                Log.LogAction("Delete Currency", $"Currency ID: {currencyId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return result;
            }
        }
    }
}
