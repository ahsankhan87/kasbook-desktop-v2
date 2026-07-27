using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using POS.Core;

namespace POS.DLL
{
    /// <summary>
    /// Data Access Layer for Cost Center module.
    /// Handles CRUD operations and complex queries for cost centers, budgets, and allocations.
    /// </summary>
    public class CostCenterDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;

        #region Cost Center Operations

        /// <summary>
        /// Saves a branch (insert or update).
        /// Validates: code uniqueness, parent existence, no circular hierarchy.
        /// </summary>
        public int SaveBranch(CostCenterModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.BranchCode))
                throw new ArgumentException("Branch code is required.", nameof(model.BranchCode));

            if (string.IsNullOrWhiteSpace(model.BranchName))
                throw new ArgumentException("Branch name is required.", nameof(model.BranchName));

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();

                // Check code uniqueness (if inserting or code changed)
                if (model.BranchId == 0 || HasBranchCodeChanged(cn, model))
                {
                    const string checkCodeSql = "SELECT COUNT(1) FROM dbo.pos_branches WHERE branch_code = @code AND id <> @branchId";
                    using (SqlCommand checkCmd = new SqlCommand(checkCodeSql, cn))
                    {
                        checkCmd.Parameters.AddWithValue("@code", model.BranchCode);
                        checkCmd.Parameters.AddWithValue("@branchId", model.BranchId);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                            throw new InvalidOperationException($"Branch code '{model.BranchCode}' already exists.");
                    }
                }

                // Check parent exists and validate hierarchy
                if (model.ParentBranchId.HasValue && model.ParentBranchId.Value > 0)
                {
                    const string parentCheckSql = "SELECT COUNT(1) FROM dbo.pos_branches WHERE id = @parentId";
                    using (SqlCommand parentCmd = new SqlCommand(parentCheckSql, cn))
                    {
                        parentCmd.Parameters.AddWithValue("@parentId", model.ParentBranchId.Value);
                        int count = (int)parentCmd.ExecuteScalar();
                        if (count == 0)
                            throw new InvalidOperationException("Parent branch does not exist.");
                    }

                    // Check for circular reference
                    if (HasCircularReference(cn, model.BranchId, model.ParentBranchId.Value))
                        throw new InvalidOperationException("Circular hierarchy detected. Parent cannot be a descendant of this branch.");
                }

                if (model.BranchId == 0)
                {
                    // Insert
                    const string insertSql = @"
                        INSERT INTO dbo.pos_branches
                        (branch_code, name, branch_type, parent_id, manager_id, monthly_budget, start_date, end_date, is_active, description, date_created)
                        VALUES
                        (@code, @name, @type, @parentId, @managerId, @monthlyBudget, @startDate, @endDate, @isActive, @description, GETDATE());
                        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    using (SqlCommand insertCmd = new SqlCommand(insertSql, cn))
                    {
                        insertCmd.Parameters.AddWithValue("@code", model.BranchCode);
                        insertCmd.Parameters.AddWithValue("@name", model.BranchName);
                        insertCmd.Parameters.AddWithValue("@type", (object)model.BranchType ?? DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@parentId", (object)model.ParentBranchId ?? DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@managerId", (object)model.ManagerId ?? DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@monthlyBudget", (object)model.MonthlyBudget ?? DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@startDate", model.StartDate.Date);
                        insertCmd.Parameters.AddWithValue("@endDate", (object)model.EndDate?.Date ?? DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@isActive", model.IsActive ? 1 : 0);
                        insertCmd.Parameters.AddWithValue("@description", (object)model.Description ?? DBNull.Value);

                        model.BranchId = (int)insertCmd.ExecuteScalar();
                        Log.LogAction("Branch Created", $"Code: {model.BranchCode}, Name: {model.BranchName}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                        return model.BranchId;
                    }
                }
                else
                {
                    // Update
                    const string updateSql = @"
                        UPDATE dbo.pos_branches
                        SET branch_code = @code,
                            name = @name,
                            branch_type = @type,
                            parent_id = @parentId,
                            manager_id = @managerId,
                            monthly_budget = @monthlyBudget,
                            start_date = @startDate,
                            end_date = @endDate,
                            is_active = @isActive,
                            description = @description,
                            date_updated = GETDATE()
                        WHERE id = @branchId;
                        SELECT @branchId;";

                    using (SqlCommand updateCmd = new SqlCommand(updateSql, cn))
                    {
                        updateCmd.Parameters.AddWithValue("@branchId", model.BranchId);
                        updateCmd.Parameters.AddWithValue("@code", model.BranchCode);
                        updateCmd.Parameters.AddWithValue("@name", model.BranchName);
                        updateCmd.Parameters.AddWithValue("@type", (object)model.BranchType ?? DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@parentId", (object)model.ParentBranchId ?? DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@managerId", (object)model.ManagerId ?? DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@monthlyBudget", (object)model.MonthlyBudget ?? DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@startDate", model.StartDate.Date);
                        updateCmd.Parameters.AddWithValue("@endDate", (object)model.EndDate?.Date ?? DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@isActive", model.IsActive ? 1 : 0);
                        updateCmd.Parameters.AddWithValue("@description", (object)model.Description ?? DBNull.Value);

                        updateCmd.ExecuteScalar();
                        Log.LogAction("Branch Updated", $"Code: {model.BranchCode}, Name: {model.BranchName}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                        return model.BranchId;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a flat list of active branches formatted for dropdown (e.g., "BR-001 — Sales").
        /// Optionally filtered by branch type.
        /// </summary>
        public DataTable GetBranchDropdown(string branchType = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                const string sql = @"
SELECT
    id AS id,
    CONCAT(branch_code, ' — ', name) AS display_text,
    branch_code,
    name,
    branch_type,
    is_active
FROM dbo.pos_branches
WHERE is_active = 1
  AND (@branchType IS NULL OR branch_type = @branchType)
ORDER BY branch_code, name;";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@branchType", (object)branchType ?? DBNull.Value);
                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// Gets a single branch by ID.
        /// </summary>
        public CostCenterModel GetBranchById(int branchId)
        {
            if (branchId <= 0)
                return null;

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                const string sql = @"
SELECT
    id, branch_code, name, branch_type, parent_id, manager_id,
    monthly_budget, start_date, end_date, is_active, description, date_created
FROM dbo.pos_branches
WHERE id = @branchId;";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@branchId", branchId);
                    cn.Open();
                    using (SqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (!r.Read())
                            return null;

                        return new CostCenterModel
                        {
                            BranchId = (int)r["id"],
                            BranchCode = r["branch_code"]?.ToString() ?? "",
                            BranchName = r["name"]?.ToString() ?? "",
                            BranchType = r["branch_type"]?.ToString(),
                            ParentBranchId = r["parent_id"] == DBNull.Value ? null : (int?)r["parent_id"],
                            ManagerId = r["manager_id"] == DBNull.Value ? null : (int?)r["manager_id"],
                            MonthlyBudget = r["monthly_budget"] == DBNull.Value ? null : (decimal?)r["monthly_budget"],
                            StartDate = r["start_date"] == DBNull.Value ? DateTime.Today : (DateTime)r["start_date"],
                            EndDate = r["end_date"] == DBNull.Value ? null : (DateTime?)r["end_date"],
                            IsActive = (bool)r["is_active"],
                            Description = r["description"]?.ToString(),
                            CreatedAt = r["date_created"] == DBNull.Value ? DateTime.Now : (DateTime)r["date_created"]
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Gets the branch tree with hierarchical rollup of income/expense balances.
        /// </summary>
        public DataTable GetBranchTree(bool includeBalances = true, DateTime? fromDate = null, DateTime? toDate = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetCostCenterTree", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IncludeBalances", includeBalances ? 1 : 0);
                    cmd.Parameters.AddWithValue("@FromDate", (object)fromDate?.Date ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ToDate", (object)toDate?.Date ?? DBNull.Value);

                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        #endregion


        #region Allocation Operations

        /// <summary>
        /// Runs automatic expense allocation using the sp_AutoAllocateExpenses stored procedure.
        /// </summary>
        public AllocationResult RunExpenseAllocation(DateTime period, int userId, int? allocationRuleId = null)
        {
            var result = new AllocationResult
            {
                PeriodStart = new DateTime(period.Year, period.Month, 1),
                Success = false,
                Allocations = new List<AllocationResultRow>()
            };

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_AutoAllocateExpenses", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300; // 5 minutes for complex allocation
                    cmd.Parameters.AddWithValue("@Period", result.PeriodStart.Date);
                    cmd.Parameters.AddWithValue("@AllocationRuleId", (object)allocationRuleId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    try
                    {
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            // First result set: summary
                            if (r.Read())
                            {
                                result.PeriodStart = ((DateTime)r["PeriodStart"]).Date;
                                result.PeriodEnd = ((DateTime)r["PeriodEndExclusive"]).Date;
                                result.VoucherNo = r["VoucherNo"]?.ToString() ?? "";
                                result.EntryHeaderId = (int?)r["EntryHeaderId"] ?? 0;
                                result.TotalAllocated = (decimal?)r["TotalAllocated"] ?? 0m;
                                result.Message = r["Message"]?.ToString() ?? "Allocation completed.";
                                result.Success = !string.IsNullOrEmpty(result.VoucherNo);
                            }

                            // Second result set: per-department details
                            if (r.NextResult())
                            {
                                while (r.Read())
                                {
                                    result.Allocations.Add(new AllocationResultRow
                                    {
                                        AllocationRuleId = (int)r["alloc_id"],
                                        AllocationName = r["alloc_name"]?.ToString() ?? "",
                                        SourceAccountId = (int)r["source_acc_id"],
                                        BranchId = (int)r["id"],
                                        AllocationMethod = r["allocation_method"]?.ToString() ?? "",
                                        AllocationPercent = (decimal?)r["allocation_percent"] ?? 0m,
                                        SourceAmount = (decimal?)r["source_amount"] ?? 0m,
                                        AllocatedAmount = (decimal?)r["allocated_amount"] ?? 0m
                                    });
                                }
                            }
                        }

                        if (result.Success)
                        {
                            Log.LogAction("Branch Allocation", $"Period: {result.PeriodStart:yyyy-MM}, Voucher: {result.VoucherNo}, Total: {result.TotalAllocated}", userId, UsersModal.logged_in_branch_id);
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.Message = $"Allocation failed: {ex.Message}";
                    }
                }
            }

            return result;
        }

        #endregion

        #region Helper Methods

        private bool HasBranchCodeChanged(SqlConnection cn, CostCenterModel model)
        {
            if (model.BranchId <= 0)
                return true;

            const string sql = "SELECT branch_code FROM dbo.pos_branches WHERE id = @branchId";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@branchId", model.BranchId);
                object result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return true;

                return !result.ToString().Equals(model.BranchCode, StringComparison.OrdinalIgnoreCase);
            }
        }

        private bool HasCircularReference(SqlConnection cn, int branchId, int parentId)
        {
            if (branchId <= 0 || parentId <= 0 || branchId == parentId)
                return true;

            // Traverse up the hierarchy from parentId; if we find branchId, it's circular
            const string sql = @"
                WITH Ancestors AS
                (

                    SELECT parent_id FROM dbo.pos_branches WHERE id = @parentId AND parent_id IS NOT NULL
                    UNION ALL
                    SELECT c.parent_id FROM dbo.pos_branches c
                    INNER JOIN Ancestors a ON c.id = a.parent_id
                    WHERE c.parent_id IS NOT NULL
                )
                SELECT COUNT(1) FROM Ancestors WHERE parent_id = @branchId;";

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@branchId", branchId);
                cmd.Parameters.AddWithValue("@parentId", parentId);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        #endregion
    }
}
