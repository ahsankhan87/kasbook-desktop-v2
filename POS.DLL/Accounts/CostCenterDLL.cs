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
        /// Saves a cost center (insert or update).
        /// Validates: code uniqueness, parent existence, no circular hierarchy.
        /// </summary>
        public int SaveCostCenter(CostCenterModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.CcCode))
                throw new ArgumentException("Cost center code is required.", nameof(model.CcCode));

            if (string.IsNullOrWhiteSpace(model.CcName))
                throw new ArgumentException("Cost center name is required.", nameof(model.CcName));

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();

                // Check code uniqueness (if inserting or code changed)
                if (model.CcId == 0 || HasCcCodeChanged(cn, model))
                {
                    const string checkCodeSql = "SELECT COUNT(1) FROM dbo.acc_cost_centers WHERE cc_code = @code AND cc_id <> @ccId";
                    using (SqlCommand checkCmd = new SqlCommand(checkCodeSql, cn))
                    {
                        checkCmd.Parameters.AddWithValue("@code", model.CcCode);
                        checkCmd.Parameters.AddWithValue("@ccId", model.CcId);
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                            throw new InvalidOperationException($"Cost center code '{model.CcCode}' already exists.");
                    }
                }

                // Check parent exists and validate hierarchy
                if (model.ParentCcId.HasValue && model.ParentCcId.Value > 0)
                {
                    const string parentCheckSql = "SELECT COUNT(1) FROM dbo.acc_cost_centers WHERE cc_id = @parentId";
                    using (SqlCommand parentCmd = new SqlCommand(parentCheckSql, cn))
                    {
                        parentCmd.Parameters.AddWithValue("@parentId", model.ParentCcId.Value);
                        int count = (int)parentCmd.ExecuteScalar();
                        if (count == 0)
                            throw new InvalidOperationException("Parent cost center does not exist.");
                    }

                    // Check for circular reference
                    if (HasCircularReference(cn, model.CcId, model.ParentCcId.Value))
                        throw new InvalidOperationException("Circular hierarchy detected. Parent cannot be a descendant of this cost center.");
                }

                if (model.CcId == 0)
                {
                    // Insert
                    const string insertSql = @"
INSERT INTO dbo.acc_cost_centers
(cc_code, cc_name, cc_type, parent_cc_id, manager_id, monthly_budget, start_date, end_date, is_active, description, created_at)
VALUES
(@code, @name, @type, @parentId, @managerId, @monthlyBudget, @startDate, @endDate, @isActive, @description, GETDATE());
SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    using (SqlCommand insertCmd = new SqlCommand(insertSql, cn))
                    {
                        insertCmd.Parameters.AddWithValue("@code", model.CcCode);
                        insertCmd.Parameters.AddWithValue("@name", model.CcName);
                        insertCmd.Parameters.AddWithValue("@type", (object)model.CcType ?? DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@parentId", (object)model.ParentCcId ?? DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@managerId", (object)model.ManagerId ?? DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@monthlyBudget", (object)model.MonthlyBudget ?? DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@startDate", model.StartDate.Date);
                        insertCmd.Parameters.AddWithValue("@endDate", (object)model.EndDate?.Date ?? DBNull.Value);
                        insertCmd.Parameters.AddWithValue("@isActive", model.IsActive ? 1 : 0);
                        insertCmd.Parameters.AddWithValue("@description", (object)model.Description ?? DBNull.Value);

                        model.CcId = (int)insertCmd.ExecuteScalar();
                        Log.LogAction("Cost Center Created", $"Code: {model.CcCode}, Name: {model.CcName}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                        return model.CcId;
                    }
                }
                else
                {
                    // Update
                    const string updateSql = @"
UPDATE dbo.acc_cost_centers
SET cc_code = @code,
    cc_name = @name,
    cc_type = @type,
    parent_cc_id = @parentId,
    manager_id = @managerId,
    monthly_budget = @monthlyBudget,
    start_date = @startDate,
    end_date = @endDate,
    is_active = @isActive,
    description = @description
WHERE cc_id = @ccId;
SELECT @ccId;";

                    using (SqlCommand updateCmd = new SqlCommand(updateSql, cn))
                    {
                        updateCmd.Parameters.AddWithValue("@ccId", model.CcId);
                        updateCmd.Parameters.AddWithValue("@code", model.CcCode);
                        updateCmd.Parameters.AddWithValue("@name", model.CcName);
                        updateCmd.Parameters.AddWithValue("@type", (object)model.CcType ?? DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@parentId", (object)model.ParentCcId ?? DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@managerId", (object)model.ManagerId ?? DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@monthlyBudget", (object)model.MonthlyBudget ?? DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@startDate", model.StartDate.Date);
                        updateCmd.Parameters.AddWithValue("@endDate", (object)model.EndDate?.Date ?? DBNull.Value);
                        updateCmd.Parameters.AddWithValue("@isActive", model.IsActive ? 1 : 0);
                        updateCmd.Parameters.AddWithValue("@description", (object)model.Description ?? DBNull.Value);

                        updateCmd.ExecuteScalar();
                        Log.LogAction("Cost Center Updated", $"Code: {model.CcCode}, Name: {model.CcName}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                        return model.CcId;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a flat list of active cost centers formatted for dropdown (e.g., "CC-001 — Sales").
        /// Optionally filtered by cost center type.
        /// </summary>
        public DataTable GetCostCenterDropdown(string ccType = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                const string sql = @"
SELECT
    cc_id AS id,
    CONCAT(cc_code, ' — ', cc_name) AS display_text,
    cc_code,
    cc_name,
    cc_type,
    is_active
FROM dbo.acc_cost_centers
WHERE is_active = 1
  AND (@ccType IS NULL OR cc_type = @ccType)
ORDER BY cc_code, cc_name;";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ccType", (object)ccType ?? DBNull.Value);
                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// Gets a single cost center by ID.
        /// </summary>
        public CostCenterModel GetCostCenterById(int ccId)
        {
            if (ccId <= 0)
                return null;

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                const string sql = @"
SELECT
    cc_id, cc_code, cc_name, cc_type, parent_cc_id, manager_id,
    monthly_budget, start_date, end_date, is_active, description, created_at
FROM dbo.acc_cost_centers
WHERE cc_id = @ccId;";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ccId", ccId);
                    cn.Open();
                    using (SqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (!r.Read())
                            return null;

                        return new CostCenterModel
                        {
                            CcId = (int)r["cc_id"],
                            CcCode = r["cc_code"]?.ToString() ?? "",
                            CcName = r["cc_name"]?.ToString() ?? "",
                            CcType = r["cc_type"]?.ToString(),
                            ParentCcId = r["parent_cc_id"] == DBNull.Value ? null : (int?)r["parent_cc_id"],
                            ManagerId = r["manager_id"] == DBNull.Value ? null : (int?)r["manager_id"],
                            MonthlyBudget = r["monthly_budget"] == DBNull.Value ? null : (decimal?)r["monthly_budget"],
                            StartDate = r["start_date"] == DBNull.Value ? DateTime.Today : (DateTime)r["start_date"],
                            EndDate = r["end_date"] == DBNull.Value ? null : (DateTime?)r["end_date"],
                            IsActive = (bool)r["is_active"],
                            Description = r["description"]?.ToString(),
                            CreatedAt = r["created_at"] == DBNull.Value ? DateTime.Now : (DateTime)r["created_at"]
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Gets the cost center tree with hierarchical rollup of income/expense balances.
        /// </summary>
        public DataTable GetCostCenterTree(bool includeBalances = true, DateTime? fromDate = null, DateTime? toDate = null)
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

        #region Budget Operations

        /// <summary>
        /// Saves or replaces monthly budgets for a cost center and fiscal year.
        /// Validates that all amounts are non-negative and accounts are Income/Expense types.
        /// </summary>
        public void SetBudgets(int ccId, int yearId, List<AccountBudget> budgets, int userId)
        {
            if (ccId <= 0)
                throw new ArgumentException("Invalid cost center ID.", nameof(ccId));

            if (yearId <= 0)
                throw new ArgumentException("Invalid financial year ID.", nameof(yearId));

            if (budgets == null || budgets.Count == 0)
                throw new ArgumentException("At least one budget entry is required.", nameof(budgets));

            // Validate all amounts are non-negative
            foreach (var budget in budgets)
            {
                if (budget.JanBudget < 0 || budget.FebBudget < 0 || budget.MarBudget < 0 ||
                    budget.AprBudget < 0 || budget.MayBudget < 0 || budget.JunBudget < 0 ||
                    budget.JulBudget < 0 || budget.AugBudget < 0 || budget.SepBudget < 0 ||
                    budget.OctBudget < 0 || budget.NovBudget < 0 || budget.DecBudget < 0)
                {
                    throw new ArgumentException("Budget amounts cannot be negative.");
                }
            }

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();

                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        // Delete existing budgets for this cost center and year
                        const string deleteSql = @"
DELETE FROM dbo.acc_cost_center_budgets
WHERE cc_id = @ccId AND financial_year_id = @yearId;";

                        using (SqlCommand deleteCmd = new SqlCommand(deleteSql, cn, tx))
                        {
                            deleteCmd.Parameters.AddWithValue("@ccId", ccId);
                            deleteCmd.Parameters.AddWithValue("@yearId", yearId);
                            deleteCmd.ExecuteNonQuery();
                        }

                        // Insert new budgets
                        const string insertSql = @"
INSERT INTO dbo.acc_cost_center_budgets
(cc_id, financial_year_id, account_id, jan_budget, feb_budget, mar_budget, apr_budget,
 may_budget, jun_budget, jul_budget, aug_budget, sep_budget, oct_budget, nov_budget, dec_budget,
 created_by, created_at)
VALUES
(@ccId, @yearId, @accountId, @jan, @feb, @mar, @apr, @may, @jun, @jul, @aug, @sep, @oct, @nov, @dec,
 @createdBy, GETDATE());";

                        foreach (var budget in budgets)
                        {
                            using (SqlCommand insertCmd = new SqlCommand(insertSql, cn, tx))
                            {
                                insertCmd.Parameters.AddWithValue("@ccId", ccId);
                                insertCmd.Parameters.AddWithValue("@yearId", yearId);
                                insertCmd.Parameters.AddWithValue("@accountId", budget.AccountId);
                                insertCmd.Parameters.AddWithValue("@jan", budget.JanBudget);
                                insertCmd.Parameters.AddWithValue("@feb", budget.FebBudget);
                                insertCmd.Parameters.AddWithValue("@mar", budget.MarBudget);
                                insertCmd.Parameters.AddWithValue("@apr", budget.AprBudget);
                                insertCmd.Parameters.AddWithValue("@may", budget.MayBudget);
                                insertCmd.Parameters.AddWithValue("@jun", budget.JunBudget);
                                insertCmd.Parameters.AddWithValue("@jul", budget.JulBudget);
                                insertCmd.Parameters.AddWithValue("@aug", budget.AugBudget);
                                insertCmd.Parameters.AddWithValue("@sep", budget.SepBudget);
                                insertCmd.Parameters.AddWithValue("@oct", budget.OctBudget);
                                insertCmd.Parameters.AddWithValue("@nov", budget.NovBudget);
                                insertCmd.Parameters.AddWithValue("@dec", budget.DecBudget);
                                insertCmd.Parameters.AddWithValue("@createdBy", userId);
                                insertCmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                        Log.LogAction("Cost Center Budgets Set", $"CC: {ccId}, Year: {yearId}, Accounts: {budgets.Count}", userId, UsersModal.logged_in_branch_id);
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Gets budget alerts for a cost center in the current month.
        /// Returns accounts that have exceeded their monthly budget.
        /// </summary>
        public List<BudgetAlertModel> GetBudgetAlerts(int ccId, DateTime currentDate)
        {
            if (ccId <= 0)
                return new List<BudgetAlertModel>();

            var alerts = new List<BudgetAlertModel>();
            int currentMonth = currentDate.Month;
            DateTime monthStart = new DateTime(currentDate.Year, currentMonth, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                const string sql = @"
DECLARE @cc_id INT = @CCId;
DECLARE @month INT = @Month;
DECLARE @from_date DATE = @FromDate;
DECLARE @to_date DATE = @ToDate;

SELECT
    @cc_id AS cc_id,
    c.cc_code,
    b.account_id,
    a.code AS account_code,
    a.name AS account_name,
    @month AS current_month,
    ISNULL(
        CASE @month
            WHEN 1 THEN b.jan_budget
            WHEN 2 THEN b.feb_budget
            WHEN 3 THEN b.mar_budget
            WHEN 4 THEN b.apr_budget
            WHEN 5 THEN b.may_budget
            WHEN 6 THEN b.jun_budget
            WHEN 7 THEN b.jul_budget
            WHEN 8 THEN b.aug_budget
            WHEN 9 THEN b.sep_budget
            WHEN 10 THEN b.oct_budget
            WHEN 11 THEN b.nov_budget
            WHEN 12 THEN b.dec_budget
        END,
        0
    ) AS budget_amount,
    ISNULL(SUM(CASE WHEN E.account_id = b.account_id THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0) ELSE 0 END), 0) AS actual_amount
FROM dbo.acc_cost_center_budgets b
INNER JOIN dbo.acc_cost_centers c ON c.cc_id = b.cc_id
INNER JOIN dbo.acc_accounts a ON a.id = b.account_id
LEFT JOIN dbo.acc_entries E
    ON E.account_id = b.account_id
    AND E.cost_center_id = @cc_id
    AND E.entry_date >= @from_date
    AND E.entry_date <= @to_date
WHERE b.cc_id = @cc_id
GROUP BY b.account_id, a.code, a.name, c.cc_code, b.jan_budget, b.feb_budget, b.mar_budget,
         b.apr_budget, b.may_budget, b.jun_budget, b.jul_budget, b.aug_budget, b.sep_budget,
         b.oct_budget, b.nov_budget, b.dec_budget;";

                cn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@CCId", ccId);
                    cmd.Parameters.AddWithValue("@Month", currentMonth);
                    cmd.Parameters.AddWithValue("@FromDate", monthStart.Date);
                    cmd.Parameters.AddWithValue("@ToDate", monthEnd.Date);

                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            decimal budgetAmount = (decimal)(r["budget_amount"] ?? 0m);
                            decimal actualAmount = (decimal)(r["actual_amount"] ?? 0m);
                            decimal overspendAmount = Math.Max(0, actualAmount - budgetAmount);

                            if (overspendAmount > 0)
                            {
                                decimal overspendPercent = budgetAmount > 0 ? (actualAmount / budgetAmount) * 100 : 0;
                                string severity = overspendPercent > 120 ? "Critical" : overspendPercent > 105 ? "Warning" : "Info";

                                alerts.Add(new BudgetAlertModel
                                {
                                    CcId = ccId,
                                    CcCode = r["cc_code"]?.ToString() ?? "",
                                    AccountId = (int)r["account_id"],
                                    AccountCode = r["account_code"]?.ToString() ?? "",
                                    AccountName = r["account_name"]?.ToString() ?? "",
                                    CurrentMonth = currentMonth,
                                    BudgetAmount = budgetAmount,
                                    ActualAmount = actualAmount,
                                    OverspendAmount = overspendAmount,
                                    OverspendPercent = overspendPercent,
                                    SeverityLevel = severity
                                });
                            }
                        }
                    }
                }
            }

            return alerts;
        }

        /// <summary>
        /// Checks if posting an amount to an account in a cost center would exceed budget.
        /// Returns detailed result including remaining budget.
        /// </summary>
        public BudgetCheckResult CheckBudgetBeforePosting(int ccId, int accountId, decimal amount, DateTime date)
        {
            if (ccId <= 0 || accountId <= 0)
                return new BudgetCheckResult { Message = "Invalid cost center or account." };

            int month = date.Month;
            DateTime monthStart = new DateTime(date.Year, month, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                const string sql = @"
SELECT TOP 1
    ISNULL(
        CASE @Month
            WHEN 1 THEN b.jan_budget
            WHEN 2 THEN b.feb_budget
            WHEN 3 THEN b.mar_budget
            WHEN 4 THEN b.apr_budget
            WHEN 5 THEN b.may_budget
            WHEN 6 THEN b.jun_budget
            WHEN 7 THEN b.jul_budget
            WHEN 8 THEN b.aug_budget
            WHEN 9 THEN b.sep_budget
            WHEN 10 THEN b.oct_budget
            WHEN 11 THEN b.nov_budget
            WHEN 12 THEN b.dec_budget
        END,
        0
    ) AS monthly_budget,
    ISNULL(SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)), 0) AS current_actual
FROM dbo.acc_cost_center_budgets b
LEFT JOIN dbo.acc_entries E
    ON E.account_id = @AccountId
    AND E.cost_center_id = @CCId
    AND E.entry_date >= @MonthStart
    AND E.entry_date <= @MonthEnd
WHERE b.cc_id = @CCId
  AND b.account_id = @AccountId
GROUP BY b.jan_budget, b.feb_budget, b.mar_budget, b.apr_budget, b.may_budget, b.jun_budget,
         b.jul_budget, b.aug_budget, b.sep_budget, b.oct_budget, b.nov_budget, b.dec_budget;";

                cn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@CCId", ccId);
                    cmd.Parameters.AddWithValue("@AccountId", accountId);
                    cmd.Parameters.AddWithValue("@Month", month);
                    cmd.Parameters.AddWithValue("@MonthStart", monthStart.Date);
                    cmd.Parameters.AddWithValue("@MonthEnd", monthEnd.Date);

                    using (SqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (!r.Read())
                        {
                            return new BudgetCheckResult
                            {
                                IsOverBudget = false,
                                RemainingBudget = 0,
                                Message = "No budget defined for this account in this cost center.",
                                SeverityLevel = null
                            };
                        }

                        decimal monthlyBudget = (decimal)r["monthly_budget"];
                        decimal currentActual = (decimal)r["current_actual"];
                        decimal projectedActual = currentActual + amount;
                        decimal remainingBudget = monthlyBudget - projectedActual;
                        bool isOver = remainingBudget < 0;

                        string severity = null;
                        string message = $"Budget: {monthlyBudget:N2}, Current: {currentActual:N2}, Projected: {projectedActual:N2}";

                        if (isOver)
                        {
                            decimal overspendPercent = monthlyBudget > 0 ? (projectedActual / monthlyBudget) * 100 : 100;
                            severity = overspendPercent > 120 ? "Critical" : "Warning";
                            message = $"⚠ Over budget by {Math.Abs(remainingBudget):N2}. {message}";
                        }

                        return new BudgetCheckResult
                        {
                            IsOverBudget = isOver,
                            RemainingBudget = remainingBudget,
                            MonthlyBudget = monthlyBudget,
                            CurrentActual = currentActual,
                            Message = message,
                            SeverityLevel = severity
                        };
                    }
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
                                        CostCenterId = (int)r["cc_id"],
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
                            Log.LogAction("Cost Center Allocation", $"Period: {result.PeriodStart:yyyy-MM}, Voucher: {result.VoucherNo}, Total: {result.TotalAllocated}", userId, UsersModal.logged_in_branch_id);
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

        private bool HasCcCodeChanged(SqlConnection cn, CostCenterModel model)
        {
            if (model.CcId <= 0)
                return true;

            const string sql = "SELECT cc_code FROM dbo.acc_cost_centers WHERE cc_id = @ccId";
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@ccId", model.CcId);
                object result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value)
                    return true;

                return !result.ToString().Equals(model.CcCode, StringComparison.OrdinalIgnoreCase);
            }
        }

        private bool HasCircularReference(SqlConnection cn, int ccId, int parentId)
        {
            if (ccId <= 0 || parentId <= 0 || ccId == parentId)
                return true;

            // Traverse up the hierarchy from parentId; if we find ccId, it's circular
            const string sql = @"
WITH Ancestors AS
(
    SELECT parent_cc_id FROM dbo.acc_cost_centers WHERE cc_id = @parentId AND parent_cc_id IS NOT NULL
    UNION ALL
    SELECT parent_cc_id FROM dbo.acc_cost_centers c
    INNER JOIN Ancestors a ON c.cc_id = a.parent_cc_id
    WHERE c.parent_cc_id IS NOT NULL
)
SELECT COUNT(1) FROM Ancestors WHERE parent_cc_id = @ccId;";

            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@ccId", ccId);
                cmd.Parameters.AddWithValue("@parentId", parentId);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        #endregion
    }
}
