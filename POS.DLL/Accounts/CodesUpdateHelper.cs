using System;
using System.Data;
using System.Data.SqlClient;
using POS.Core;

namespace POS.DLL.Accounts
{
    /// <summary>
    /// Helper class to batch-update codes for existing groups and accounts in the database.
    /// Generates unique hierarchical codes following the standard COA numbering scheme.
    /// </summary>
    public class CodesUpdateHelper
    {
        private readonly string _connectionString = dbConnection.ConnectionString;

        /// <summary>
        /// Updates codes for all groups and accounts that have null, empty, or '0' codes.
        /// Executes in transaction to ensure consistency.
        /// </summary>
        public CodesUpdateResult UpdateAllCodes()
        {
            var result = new CodesUpdateResult();
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                try
                {
                    cn.Open();
                    SqlTransaction tx = cn.BeginTransaction();

                    try
                    {
                        result.Level1GroupsUpdated = UpdateLevel1GroupCodes(cn, tx);
                        result.Level2GroupsUpdated = UpdateLevel2GroupCodes(cn, tx);
                        result.AccountsUpdated = UpdateAccountCodes(cn, tx);

                        tx.Commit();
                        result.IsSuccess = true;
                        result.Message = $"Successfully updated {result.Level1GroupsUpdated} Level-1 groups, " +
                                       $"{result.Level2GroupsUpdated} Level-2 groups, and " +
                                       $"{result.AccountsUpdated} accounts.";
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.Message = $"Error updating codes: {ex.Message}";
                }
            }
            return result;
        }

        /// <summary>
        /// Updates codes for Level-1 groups (root groups) based on account type.
        /// Assets=1000, Liabilities=2000, Equity=3000, Income=4000, Expenses=5000
        /// </summary>
        private int UpdateLevel1GroupCodes(SqlConnection cn, SqlTransaction tx)
        {
            string sql = @"
                DECLARE @AssetTypeId INT = (SELECT TOP 1 id FROM acc_account_type WHERE LOWER(name) LIKE '%asset%');
                DECLARE @LiabilityTypeId INT = (SELECT TOP 1 id FROM acc_account_type WHERE LOWER(name) LIKE '%liabil%');
                DECLARE @EquityTypeId INT = (SELECT TOP 1 id FROM acc_account_type WHERE LOWER(name) LIKE '%equity%');
                DECLARE @IncomeTypeId INT = (SELECT TOP 1 id FROM acc_account_type WHERE LOWER(name) LIKE '%income%' OR LOWER(name) LIKE '%revenue%');
                DECLARE @ExpenseTypeId INT = (SELECT TOP 1 id FROM acc_account_type WHERE LOWER(name) LIKE '%expense%');

                UPDATE acc_groups
                SET code = CASE 
                    WHEN account_type_id = @AssetTypeId THEN '1000'
                    WHEN account_type_id = @LiabilityTypeId THEN '2000'
                    WHEN account_type_id = @EquityTypeId THEN '3000'
                    WHEN account_type_id = @IncomeTypeId THEN '4000'
                    WHEN account_type_id = @ExpenseTypeId THEN '5000'
                    ELSE '9000'
                END
                WHERE (parent_id = 0 OR parent_id IS NULL)
                  AND (code IS NULL OR code = '' OR code = '0');

                SELECT @@ROWCOUNT;
            ";

            using (SqlCommand cmd = new SqlCommand(sql, cn, tx))
            {
                cmd.CommandTimeout = 120;
                int rowsAffected = (int)cmd.ExecuteScalar();
                return rowsAffected;
            }
        }

        /// <summary>
        /// Updates codes for Level-2 groups (sub-groups).
        /// Codes are generated as ParentCode + 2-digit sequential number (e.g., 1100, 1101, 1102)
        /// </summary>
        private int UpdateLevel2GroupCodes(SqlConnection cn, SqlTransaction tx)
        {
            string sql = @"
                WITH L2Groups AS (
                    SELECT 
                        g.id,
                        g.parent_id,
                        ROW_NUMBER() OVER (PARTITION BY g.parent_id ORDER BY g.id) AS RowNum,
                        p.code AS ParentCode
                    FROM acc_groups g
                    INNER JOIN acc_groups p ON g.parent_id = p.id
                    WHERE g.parent_id > 0
                      AND (g.code IS NULL OR g.code = '' OR g.code = '0')
                )
                UPDATE acc_groups
                SET code = ParentCode + SUBSTRING(RIGHT('00' + CAST(RowNum AS VARCHAR), 2), 1, 2)
                FROM L2Groups l2
                WHERE acc_groups.id = l2.id;

                SELECT @@ROWCOUNT;
            ";

            using (SqlCommand cmd = new SqlCommand(sql, cn, tx))
            {
                cmd.CommandTimeout = 120;
                int rowsAffected = (int)cmd.ExecuteScalar();
                return rowsAffected;
            }
        }

        /// <summary>
        /// Updates codes for accounts (Level-3).
        /// Codes are generated as GroupCode + '-' + 3-digit sequential number (e.g., 1100-001, 1100-002)
        /// </summary>
        private int UpdateAccountCodes(SqlConnection cn, SqlTransaction tx)
        {
            string sql = @"
                WITH AccountsWithCodes AS (
                    SELECT 
                        a.id,
                        a.group_id,
                        a.branch_id,
                        ROW_NUMBER() OVER (PARTITION BY a.group_id, a.branch_id ORDER BY a.id) AS RowNum,
                        g.code AS GroupCode
                    FROM acc_accounts a
                    INNER JOIN acc_groups g ON a.group_id = g.id
                    WHERE (a.code IS NULL OR a.code = '' OR a.code = '0')
                )
                UPDATE acc_accounts
                SET code = GroupCode + '-' + RIGHT('000' + CAST(RowNum AS VARCHAR), 3)
                FROM AccountsWithCodes awc
                WHERE acc_accounts.id = awc.id;

                SELECT @@ROWCOUNT;
            ";

            using (SqlCommand cmd = new SqlCommand(sql, cn, tx))
            {
                cmd.CommandTimeout = 120;
                int rowsAffected = (int)cmd.ExecuteScalar();
                return rowsAffected;
            }
        }

        /// <summary>
        /// Retrieves verification statistics showing code assignment coverage.
        /// </summary>
        public CodeAssignmentStats GetCodeAssignmentStats()
        {
            var stats = new CodeAssignmentStats();
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                try
                {
                    cn.Open();

                    string sql = @"
                        SELECT 
                            'Level-1 Groups' AS Category, 
                            COUNT(*) AS TotalRecords, 
                            SUM(CASE WHEN code IS NOT NULL AND code != '' AND code != '0' THEN 1 ELSE 0 END) AS AssignedCodes
                        FROM acc_groups
                        WHERE parent_id = 0 OR parent_id IS NULL
                        UNION ALL
                        SELECT 
                            'Level-2 Groups', 
                            COUNT(*), 
                            SUM(CASE WHEN code IS NOT NULL AND code != '' AND code != '0' THEN 1 ELSE 0 END)
                        FROM acc_groups
                        WHERE parent_id > 0
                        UNION ALL
                        SELECT 
                            'Accounts', 
                            COUNT(*), 
                            SUM(CASE WHEN code IS NOT NULL AND code != '' AND code != '0' THEN 1 ELSE 0 END)
                        FROM acc_accounts;
                    ";

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string category = reader["Category"].ToString();
                                int totalRecords = reader["TotalRecords"] != DBNull.Value ? (int)reader["TotalRecords"] : 0;
                                int assignedCodes = reader["AssignedCodes"] != DBNull.Value ? (int)reader["AssignedCodes"] : 0;

                                if (category == "Level-1 Groups")
                                {
                                    stats.Level1GroupsTotal = totalRecords;
                                    stats.Level1GroupsWithCodes = assignedCodes;
                                }
                                else if (category == "Level-2 Groups")
                                {
                                    stats.Level2GroupsTotal = totalRecords;
                                    stats.Level2GroupsWithCodes = assignedCodes;
                                }
                                else if (category == "Accounts")
                                {
                                    stats.AccountsTotal = totalRecords;
                                    stats.AccountsWithCodes = assignedCodes;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    stats.Error = ex.Message;
                }
            }
            return stats;
        }
    }

    /// <summary>
    /// Result object for code update operations.
    /// </summary>
    public class CodesUpdateResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int Level1GroupsUpdated { get; set; }
        public int Level2GroupsUpdated { get; set; }
        public int AccountsUpdated { get; set; }
    }

    /// <summary>
    /// Statistics object showing code assignment coverage.
    /// </summary>
    public class CodeAssignmentStats
    {
        public int Level1GroupsTotal { get; set; }
        public int Level1GroupsWithCodes { get; set; }
        public int Level2GroupsTotal { get; set; }
        public int Level2GroupsWithCodes { get; set; }
        public int AccountsTotal { get; set; }
        public int AccountsWithCodes { get; set; }
        public string Error { get; set; }

        public int Level1GroupsMissing => Level1GroupsTotal - Level1GroupsWithCodes;
        public int Level2GroupsMissing => Level2GroupsTotal - Level2GroupsWithCodes;
        public int AccountsMissing => AccountsTotal - AccountsWithCodes;

        public double Level1GroupsCoverage => Level1GroupsTotal > 0 ? (Level1GroupsWithCodes * 100.0) / Level1GroupsTotal : 0;
        public double Level2GroupsCoverage => Level2GroupsTotal > 0 ? (Level2GroupsWithCodes * 100.0) / Level2GroupsTotal : 0;
        public double AccountsCoverage => AccountsTotal > 0 ? (AccountsWithCodes * 100.0) / AccountsTotal : 0;
    }
}
