using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace pos.Security.Authorization
{
    public sealed class SqlRoleRepository : IRoleRepository
    {
        
        public IEnumerable<RoleDefinition> LoadAllRoles()
        {
            var results = new Dictionary<SystemRole, RoleDefinition>();

            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
                    SELECT rp.role_name, rp.permission_name
                    FROM RolePermissions rp WITH (NOLOCK);
                    ", cn))
            {
                cn.Open();
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

                    // Ensure role and permissions exist
                    cmd.CommandText = @"
                    MERGE Roles AS t
                    USING (SELECT @role_name AS role_name) AS s
                    ON t.role_name = s.role_name
                    WHEN NOT MATCHED THEN INSERT (role_name) VALUES (s.role_name);

                    DELETE FROM RolePermissions WHERE role_name = @role_name;";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@role_name", role.ToString());
                    cmd.ExecuteNonQuery();

                    if (permissions != null)
                    {
                        foreach (var p in permissions)
                        {
                            // Ensure permission exists
                            cmd.CommandText = @"
                                MERGE Permissions AS t
                                USING (SELECT @p AS permission_name) AS s
                                ON t.permission_name = s.permission_name
                                WHEN NOT MATCHED THEN INSERT (permission_name) VALUES (s.permission_name);";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p", p);
                            cmd.ExecuteNonQuery();

                            // Insert role-permission
                            cmd.CommandText = @"
                                INSERT INTO RolePermissions(role_name, permission_name)
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
                FROM UserClaims WITH (NOLOCK)
                WHERE user_id = @uid;", cn))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cn.Open();
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

                    cmd.CommandText = "DELETE FROM UserClaims WHERE user_id = @uid;";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.ExecuteNonQuery();

                    if (claims != null)
                    {
                        foreach (var c in claims)
                        {
                            // Ensure permission exists
                            cmd.CommandText = @"
                                MERGE Permissions AS t
                                USING (SELECT @p AS permission_name) AS s
                                ON t.permission_name = s.permission_name
                                WHEN NOT MATCHED THEN INSERT (permission_name) VALUES (s.permission_name);";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p", c);
                            cmd.ExecuteNonQuery();

                            // Insert user-claim
                            cmd.CommandText = @"
                                INSERT INTO UserClaims(user_id, permission_name)
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