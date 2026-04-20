using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace pos.Security.Authorization
{
    public sealed class SqlRoleRepository : IRoleRepository
    {
        private static void EnsureSecurityTables(SqlConnection cn, SqlTransaction tx = null)
        {
            using (var cmd = new SqlCommand(@"
IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Roles
    (
        role_name NVARCHAR(50) NOT NULL PRIMARY KEY
    );
END;

IF OBJECT_ID(N'dbo.Permissions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Permissions
    (
        permission_name NVARCHAR(200) NOT NULL PRIMARY KEY
    );
END;

IF OBJECT_ID(N'dbo.RolePermissions', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RolePermissions
    (
        role_name NVARCHAR(50) NOT NULL,
        permission_name NVARCHAR(200) NOT NULL,
        CONSTRAINT PK_RolePermissions PRIMARY KEY (role_name, permission_name)
    );
END;

IF OBJECT_ID(N'dbo.UserClaims', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UserClaims
    (
        user_id INT NOT NULL,
        permission_name NVARCHAR(200) NOT NULL,
        CONSTRAINT PK_UserClaims PRIMARY KEY (user_id, permission_name)
    );
END;
", cn, tx))
            {
                cmd.ExecuteNonQuery();
            }
        }
        
        public IEnumerable<RoleDefinition> LoadAllRoles()
        {
            var results = new Dictionary<SystemRole, RoleDefinition>();

            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
                    SELECT rp.role_name, rp.permission_name
                    FROM dbo.RolePermissions rp WITH (NOLOCK);
                    ", cn))
            {
                cn.Open();
                EnsureSecurityTables(cn);
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        var roleName = rd.GetString(0);
                        var perm = rd.GetString(1);

                        if (!Enum.TryParse<SystemRole>(roleName, true, out var role))
                            continue;

                        if (!results.TryGetValue(role, out var def))
                        {
                            def = new RoleDefinition { Role = role };
                            results[role] = def;
                        }
                        def.GrantedPermissions.Add(perm);
                    }
                }
            }

            return results.Values;
        }

        public void SaveRolePermissions(SystemRole role, IEnumerable<string> permissions)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    cmd.Transaction = tx;
                    EnsureSecurityTables(cn, tx);

                    // Ensure role and permissions exist
                    cmd.CommandText = @"
                    MERGE dbo.Roles AS t
                    USING (SELECT @role_name AS role_name) AS s
                    ON t.role_name = s.role_name
                    WHEN NOT MATCHED THEN INSERT (role_name) VALUES (s.role_name);

                    DELETE FROM dbo.RolePermissions WHERE role_name = @role_name;";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@role_name", role.ToString());
                    cmd.ExecuteNonQuery();

                    if (permissions != null)
                    {
                        foreach (var p in permissions)
                        {
                            // Ensure permission exists
                            cmd.CommandText = @"
                                MERGE dbo.Permissions AS t
                                USING (SELECT @p AS permission_name) AS s
                                ON t.permission_name = s.permission_name
                                WHEN NOT MATCHED THEN INSERT (permission_name) VALUES (s.permission_name);";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p", p);
                            cmd.ExecuteNonQuery();

                            // Insert role-permission
                            cmd.CommandText = @"
                                INSERT INTO dbo.RolePermissions(role_name, permission_name)
                                VALUES(@role_name, @p);";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@role_name", role.ToString());
                            cmd.Parameters.AddWithValue("@p", p);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    tx.Commit();
                }
            }
        }

        public IEnumerable<string> LoadUserClaims(int userId)
        {
            var claims = new List<string>();
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
                SELECT permission_name
                FROM dbo.UserClaims WITH (NOLOCK)
                WHERE user_id = @uid;", cn))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cn.Open();
                EnsureSecurityTables(cn);
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                        claims.Add(rd.GetString(0));
                }
            }
            return claims;
        }

        public void SaveUserClaims(int userId, IEnumerable<string> claims)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = cn;
                cn.Open();
                using (var tx = cn.BeginTransaction())
                {
                    cmd.Transaction = tx;
                    EnsureSecurityTables(cn, tx);

                    cmd.CommandText = "DELETE FROM dbo.UserClaims WHERE user_id = @uid;";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.ExecuteNonQuery();

                    if (claims != null)
                    {
                        foreach (var c in claims)
                        {
                            // Ensure permission exists
                            cmd.CommandText = @"
                                MERGE dbo.Permissions AS t
                                USING (SELECT @p AS permission_name) AS s
                                ON t.permission_name = s.permission_name
                                WHEN NOT MATCHED THEN INSERT (permission_name) VALUES (s.permission_name);";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p", c);
                            cmd.ExecuteNonQuery();

                            // Insert user-claim
                            cmd.CommandText = @"
                                INSERT INTO dbo.UserClaims(user_id, permission_name)
                                VALUES(@uid, @p);";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@uid", userId);
                            cmd.Parameters.AddWithValue("@p", c);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    tx.Commit();
                }
            }
        }
    }
}