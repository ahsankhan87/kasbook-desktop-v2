using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using POS.Core;
using POS.DLL;

namespace POS.BLL
{
    /// <summary>
    /// Business Logic Layer for Branch module.
    /// Orchestrates Branch CRUD, budget management, and expense allocation workflows.
    /// </summary>
    public class CostCenterBLL
    {
        private readonly CostCenterDLL _dll;
        private readonly BudgetDLL _budgetDll;

        public CostCenterBLL()
        {
            _dll = new CostCenterDLL();
            _budgetDll = new BudgetDLL();
        }

        #region Branch Operations

        /// <summary>
        /// Saves a Branch (insert or update).
        /// Validates: code uniqueness, parent exists, no circular hierarchy.
        /// </summary>
        /// <param name="model">Branch model with all required fields.</param>
        /// <param name="userId">User ID for audit logging.</param>
        /// <returns>Branch ID (new or existing).</returns>
        /// <exception cref="ArgumentNullException">If model is null.</exception>
        /// <exception cref="ArgumentException">If required fields are missing or invalid.</exception>
        /// <exception cref="InvalidOperationException">If validation fails (duplicate code, bad parent, circular ref).</exception>
        public int SaveBranch(CostCenterModel model, int userId)
        {
            try
            {
                if (model == null)
                    throw new ArgumentNullException(nameof(model), "Branch model is required.");

                if (string.IsNullOrWhiteSpace(model.BranchCode))
                    throw new ArgumentException("Branch code is required.", nameof(model.BranchCode));

                if (string.IsNullOrWhiteSpace(model.BranchName))
                    throw new ArgumentException("Branch name is required.", nameof(model.BranchName));

                if (model.StartDate == DateTime.MinValue)
                    model.StartDate = DateTime.Now.Date;

                return _dll.SaveBranch(model);
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in SaveBranch", ex.Message, userId, UsersModal.logged_in_branch_id);
                throw;
            }
        }

        /// <summary>
        /// Gets a flat list of active Branches formatted for dropdown display.
        /// Excludes inactive Branches.
        /// </summary>
        /// <param name="branchType">Optional filter by Branch type (e.g., "Department", "Profit Center").</param>
        /// <returns>DataTable with id, display_text, branch_code, branch_name, branch_type columns.</returns>
        public DataTable GetBranchDropdown(string branchType = null)
        {
            try
            {
                return _dll.GetBranchDropdown(branchType);
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in GetBranchDropdown", ex.Message, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return new DataTable();
            }
        }

        /// <summary>
        /// Gets a single Branch by ID with all details.
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <returns>BranchModel or null if not found.</returns>
        public CostCenterModel GetBranchById(int branchId)
        {
            try
            {
                return _dll.GetBranchById(branchId);
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in GetBranchById", ex.Message, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return null;
            }
        }

        /// <summary>
        /// Gets the Branch hierarchy tree with optional rollup of income/expense balances.
        /// </summary>
        /// <param name="includeBalances">If true, includes total_income, total_expense, net_profit columns.</param>
        /// <param name="fromDate">Period start date for balance calculations (null = all time).</param>
        /// <param name="toDate">Period end date for balance calculations (null = all time).</param>
        /// <returns>DataTable with hierarchical Branch data.</returns>
        public DataTable GetBranchTree(bool includeBalances = true, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return _dll.GetBranchTree(includeBalances, fromDate, toDate);
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in GetBranchTree", ex.Message, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return new DataTable();
            }
        }

        #endregion

        #region Budget Operations

        /// <summary>
        /// Saves monthly budgets for a Branch and fiscal year.
        /// Replaces any existing budgets for that year.
        /// Validates: Branch exists, year exists, all amounts non-negative.
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <param name="yearId">Fiscal year ID.</param>
        /// <param name="budgets">List of AccountBudget objects with monthly amounts per account.</param>
        /// <param name="userId">User ID for audit logging.</param>
        /// <exception cref="ArgumentException">If validation fails.</exception>
        public void SetBudget(int branchId, int yearId, List<AccountBudget> budgets, int userId)
        {
            try
            {
                if (branchId <= 0)
                    throw new ArgumentException("Invalid Branch ID.", nameof(branchId));

                if (yearId <= 0)
                    throw new ArgumentException("Invalid fiscal year ID.", nameof(yearId));

                if (budgets == null || budgets.Count == 0)
                    throw new ArgumentException("At least one budget entry is required.", nameof(budgets));

                _budgetDll.SaveCostCenterBudgets(branchId, yearId, budgets, userId);
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in SetBudget", ex.Message, userId, UsersModal.logged_in_branch_id);
                throw;
            }
        }

        /// <summary>
        /// Gets budget alerts for a Branch in the current month.
        /// Returns list of accounts that have exceeded their monthly budget.
        /// Used to populate a warning panel in the journal entry form.
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <param name="currentDate">Reference date (typically today); month and year extracted from this.</param>
        /// <returns>List of BudgetAlertModel for over-budget accounts. Empty if none or no budget defined.</returns>
        public List<BudgetAlertModel> GetBudgetAlert(int branchId, DateTime currentDate)
        {
            try
            {
                return _budgetDll.GetCostCenterBudgetAlerts(branchId, currentDate);
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in GetBudgetAlert", ex.Message, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return new List<BudgetAlertModel>();
            }
        }

        /// <summary>
        /// Checks if posting an amount to an account in a Branch would exceed the monthly budget.
        /// Called from JournalsBLL before posting a journal entry to enforce budget limits.
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <param name="accId">GL Account ID.</param>
        /// <param name="amount">Amount to be posted (debit or credit absolute value).</param>
        /// <param name="date">Entry date (month/year used to determine budget period).</param>
        /// <returns>BudgetCheckResult with IsOverBudget flag and remaining budget.</returns>
        public BudgetCheckResult CheckBudgetBeforePosting(int branchId, int accId, decimal amount, DateTime date)
        {
            try
            {
                if (branchId <= 0)
                    return new BudgetCheckResult
                    {
                        IsOverBudget = false,
                        Message = "No Branch specified.",
                        SeverityLevel = null
                    };

                if (accId <= 0 || amount < 0)
                    return new BudgetCheckResult
                    {
                        IsOverBudget = false,
                        Message = "Invalid account or amount.",
                        SeverityLevel = null
                    };

                return _budgetDll.CheckBranchBudgetBeforePosting(branchId, accId, amount, date);
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in CheckBudgetBeforePosting", ex.Message, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return new BudgetCheckResult
                {
                    IsOverBudget = false,
                    Message = $"Budget check failed: {ex.Message}",
                    SeverityLevel = "Critical"
                };
            }
        }

        #endregion

        #region Allocation Operations

        /// <summary>
        /// Runs automatic expense allocation for a period using all active allocation rules.
        /// 
        /// For each active rule:
        /// 1. Sums unallocated entries (NULL cost_center_id) of the source expense account for the period.
        /// 2. Calculates each department's share based on allocation method:
        ///    - FIXED_PCT: multiplies total by configured percentage
        ///    - HEADCOUNT: divides total by headcount ratio (not yet implemented; uses FIXED_PCT)
        ///    - REVENUE: divides by each dept's revenue for the period
        /// 3. Posts allocation journal entries (DR target CC, CR source account).
        /// 4. Validates totals using residual method (last item absorbs rounding differences).
        /// 5. Returns detailed result with per-department amounts.
        /// 
        /// All entries are posted in a single balanced voucher tagged with Branch IDs.
        /// </summary>
        /// <param name="period">Period to allocate (any date in the month; 1st of month used).</param>
        /// <param name="userId">User ID initiating the allocation.</param>
        /// <param name="allocationRuleId">Optional; if specified, only this rule is used. If null, all active rules.</param>
        /// <returns>AllocationResult with success flag, voucher number, and per-department details.</returns>
        public AllocationResult RunExpenseAllocation(DateTime period, int userId, int? allocationRuleId = null)
        {
            try
            {
                if (userId <= 0)
                    throw new ArgumentException("Invalid user ID.", nameof(userId));

                period = new DateTime(period.Year, period.Month, 1); // Normalize to 1st of month

                AllocationResult result = _dll.RunExpenseAllocation(period, userId, allocationRuleId);

                if (result.Success)
                {
                    Log.LogAction(
                        "Expense Allocation Completed",
                        $"Period: {period:yyyy-MM}, Voucher: {result.VoucherNo}, Total: {result.TotalAllocated:N2}, Rules: {result.Allocations.Count}",
                        userId,
                        UsersModal.logged_in_branch_id
                    );
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in RunExpenseAllocation", ex.Message, userId, UsersModal.logged_in_branch_id);
                return new AllocationResult
                {
                    Success = false,
                    Message = $"Allocation failed: {ex.Message}",
                    PeriodStart = new DateTime(period.Year, period.Month, 1)
                };
            }
        }

        #endregion

        #region Reporting

        /// <summary>
        /// Gets a departmental P&L pivot report showing amounts by Branch.
        /// One row per GL account with columns for each Branch.
        /// Includes "Unallocated" column for entries with NULL cost_center_id.
        /// </summary>
        /// <param name="fromDate">Period start date.</param>
        /// <param name="toDate">Period end date.</param>
        /// <param name="ccIds">Optional list of Branch IDs to include. If null, all.</param>
        /// <returns>DataTable with account rows and Branch columns.</returns>
        public DataTable GetDepartmentalPL(DateTime fromDate, DateTime toDate, List<int> ccIds = null)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
                {
                    cn.Open();

                    // Create TVP for Branch IDs
                    var branchIdTable = new DataTable();
                    branchIdTable.Columns.Add("branch_id", typeof(int));
                    if (ccIds != null && ccIds.Count > 0)
                    {
                        foreach (int branchId in ccIds)
                        {
                            branchIdTable.Rows.Add(branchId);
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("sp_DepartmentalPL", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                        cmd.Parameters.AddWithValue("@ToDate", toDate.Date);

                        // Add TVP parameter
                        var tvpParam = cmd.Parameters.AddWithValue("@CCIds", branchIdTable);
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        tvpParam.TypeName = "dbo.CostCenterIdListType";

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable result = new DataTable();
                        da.Fill(result);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in GetDepartmentalPL", ex.Message, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return new DataTable();
            }
        }

        /// <summary>
        /// Gets budget vs. actual comparison for a Branch.
        /// One row per account per month showing budget, actual, variance.
        /// </summary>
        /// <param name="branchId">Branch ID.</param>
        /// <param name="yearId">Fiscal year ID.</param>
        /// <returns>DataTable with monthly budget vs. actual rows.</returns>
        public DataTable GetBudgetVsActual(int branchId, int yearId)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_CostCenterBudgetVsActual", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CCId", branchId);
                        cmd.Parameters.AddWithValue("@FinancialYearId", yearId);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable result = new DataTable();
                        da.Fill(result);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in GetBudgetVsActual", ex.Message, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return new DataTable();
            }
        }

        /// <summary>
        /// Gets a summary of all Branches with income, expense, net profit, and budget variance.
        /// </summary>
        /// <param name="fromDate">Period start date.</param>
        /// <param name="toDate">Period end date.</param>
        /// <returns>DataTable with one row per Branch.</returns>
        public DataTable GetCostCenterSummary(DateTime fromDate, DateTime toDate)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_CostCenterSummary", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                        cmd.Parameters.AddWithValue("@ToDate", toDate.Date);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable result = new DataTable();
                        da.Fill(result);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in GetCostCenterSummary", ex.Message, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return new DataTable();
            }
        }

        #endregion
    }
}
