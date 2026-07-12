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
    /// Business Logic Layer for Cost Center module.
    /// Orchestrates cost center CRUD, budget management, and expense allocation workflows.
    /// </summary>
    public class CostCenterBLL
    {
        private readonly CostCenterDLL _dll;

        public CostCenterBLL()
        {
            _dll = new CostCenterDLL();
        }

        #region Cost Center Operations

        /// <summary>
        /// Saves a cost center (insert or update).
        /// Validates: code uniqueness, parent exists, no circular hierarchy.
        /// </summary>
        /// <param name="model">Cost center model with all required fields.</param>
        /// <param name="userId">User ID for audit logging.</param>
        /// <returns>Cost center ID (new or existing).</returns>
        /// <exception cref="ArgumentNullException">If model is null.</exception>
        /// <exception cref="ArgumentException">If required fields are missing or invalid.</exception>
        /// <exception cref="InvalidOperationException">If validation fails (duplicate code, bad parent, circular ref).</exception>
        public int SaveCostCenter(CostCenterModel model, int userId)
        {
            try
            {
                if (model == null)
                    throw new ArgumentNullException(nameof(model), "Cost center model is required.");

                if (string.IsNullOrWhiteSpace(model.CcCode))
                    throw new ArgumentException("Cost center code is required.", nameof(model.CcCode));

                if (string.IsNullOrWhiteSpace(model.CcName))
                    throw new ArgumentException("Cost center name is required.", nameof(model.CcName));

                if (model.StartDate == DateTime.MinValue)
                    model.StartDate = DateTime.Now.Date;

                return _dll.SaveCostCenter(model);
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in SaveCostCenter", ex.Message, userId, UsersModal.logged_in_branch_id);
                throw;
            }
        }

        /// <summary>
        /// Gets a flat list of active cost centers formatted for dropdown display.
        /// Excludes inactive cost centers.
        /// </summary>
        /// <param name="ccType">Optional filter by cost center type (e.g., "Department", "Profit Center").</param>
        /// <returns>DataTable with id, display_text, cc_code, cc_name, cc_type columns.</returns>
        public DataTable GetCostCenterDropdown(string ccType = null)
        {
            try
            {
                return _dll.GetCostCenterDropdown(ccType);
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in GetCostCenterDropdown", ex.Message, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return new DataTable();
            }
        }

        /// <summary>
        /// Gets a single cost center by ID with all details.
        /// </summary>
        /// <param name="ccId">Cost center ID.</param>
        /// <returns>CostCenterModel or null if not found.</returns>
        public CostCenterModel GetCostCenterById(int ccId)
        {
            try
            {
                return _dll.GetCostCenterById(ccId);
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in GetCostCenterById", ex.Message, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return null;
            }
        }

        /// <summary>
        /// Gets the cost center hierarchy tree with optional rollup of income/expense balances.
        /// </summary>
        /// <param name="includeBalances">If true, includes total_income, total_expense, net_profit columns.</param>
        /// <param name="fromDate">Period start date for balance calculations (null = all time).</param>
        /// <param name="toDate">Period end date for balance calculations (null = all time).</param>
        /// <returns>DataTable with hierarchical cost center data.</returns>
        public DataTable GetCostCenterTree(bool includeBalances = true, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                return _dll.GetCostCenterTree(includeBalances, fromDate, toDate);
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in GetCostCenterTree", ex.Message, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return new DataTable();
            }
        }

        #endregion

        #region Budget Operations

        /// <summary>
        /// Saves monthly budgets for a cost center and fiscal year.
        /// Replaces any existing budgets for that year.
        /// Validates: cost center exists, year exists, all amounts non-negative.
        /// </summary>
        /// <param name="ccId">Cost center ID.</param>
        /// <param name="yearId">Fiscal year ID.</param>
        /// <param name="budgets">List of AccountBudget objects with monthly amounts per account.</param>
        /// <param name="userId">User ID for audit logging.</param>
        /// <exception cref="ArgumentException">If validation fails.</exception>
        public void SetBudget(int ccId, int yearId, List<AccountBudget> budgets, int userId)
        {
            try
            {
                if (ccId <= 0)
                    throw new ArgumentException("Invalid cost center ID.", nameof(ccId));

                if (yearId <= 0)
                    throw new ArgumentException("Invalid fiscal year ID.", nameof(yearId));

                if (budgets == null || budgets.Count == 0)
                    throw new ArgumentException("At least one budget entry is required.", nameof(budgets));

                _dll.SetBudgets(ccId, yearId, budgets, userId);
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in SetBudget", ex.Message, userId, UsersModal.logged_in_branch_id);
                throw;
            }
        }

        /// <summary>
        /// Gets budget alerts for a cost center in the current month.
        /// Returns list of accounts that have exceeded their monthly budget.
        /// Used to populate a warning panel in the journal entry form.
        /// </summary>
        /// <param name="ccId">Cost center ID.</param>
        /// <param name="currentDate">Reference date (typically today); month and year extracted from this.</param>
        /// <returns>List of BudgetAlertModel for over-budget accounts. Empty if none or no budget defined.</returns>
        public List<BudgetAlertModel> GetBudgetAlert(int ccId, DateTime currentDate)
        {
            try
            {
                return _dll.GetBudgetAlerts(ccId, currentDate);
            }
            catch (Exception ex)
            {
                Log.LogAction("Error in GetBudgetAlert", ex.Message, UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                return new List<BudgetAlertModel>();
            }
        }

        /// <summary>
        /// Checks if posting an amount to an account in a cost center would exceed the monthly budget.
        /// Called from JournalsBLL before posting a journal entry to enforce budget limits.
        /// </summary>
        /// <param name="ccId">Cost center ID.</param>
        /// <param name="accId">GL Account ID.</param>
        /// <param name="amount">Amount to be posted (debit or credit absolute value).</param>
        /// <param name="date">Entry date (month/year used to determine budget period).</param>
        /// <returns>BudgetCheckResult with IsOverBudget flag and remaining budget.</returns>
        public BudgetCheckResult CheckBudgetBeforePosting(int ccId, int accId, decimal amount, DateTime date)
        {
            try
            {
                if (ccId <= 0)
                    return new BudgetCheckResult
                    {
                        IsOverBudget = false,
                        Message = "No cost center specified.",
                        SeverityLevel = null
                    };

                if (accId <= 0 || amount < 0)
                    return new BudgetCheckResult
                    {
                        IsOverBudget = false,
                        Message = "Invalid account or amount.",
                        SeverityLevel = null
                    };

                return _dll.CheckBudgetBeforePosting(ccId, accId, amount, date);
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
        /// All entries are posted in a single balanced voucher tagged with cost center IDs.
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
        /// Gets a departmental P&L pivot report showing amounts by cost center.
        /// One row per GL account with columns for each cost center.
        /// Includes "Unallocated" column for entries with NULL cost_center_id.
        /// </summary>
        /// <param name="fromDate">Period start date.</param>
        /// <param name="toDate">Period end date.</param>
        /// <param name="ccIds">Optional list of cost center IDs to include. If null, all.</param>
        /// <returns>DataTable with account rows and cost center columns.</returns>
        public DataTable GetDepartmentalPL(DateTime fromDate, DateTime toDate, List<int> ccIds = null)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
                {
                    cn.Open();

                    // Create TVP for cost center IDs
                    var ccIdTable = new DataTable();
                    ccIdTable.Columns.Add("cc_id", typeof(int));
                    if (ccIds != null && ccIds.Count > 0)
                    {
                        foreach (int ccId in ccIds)
                        {
                            ccIdTable.Rows.Add(ccId);
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("sp_DepartmentalPL", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                        cmd.Parameters.AddWithValue("@ToDate", toDate.Date);

                        // Add TVP parameter
                        var tvpParam = cmd.Parameters.AddWithValue("@CCIds", ccIdTable);
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
        /// Gets budget vs. actual comparison for a cost center.
        /// One row per account per month showing budget, actual, variance.
        /// </summary>
        /// <param name="ccId">Cost center ID.</param>
        /// <param name="yearId">Fiscal year ID.</param>
        /// <returns>DataTable with monthly budget vs. actual rows.</returns>
        public DataTable GetBudgetVsActual(int ccId, int yearId)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_CostCenterBudgetVsActual", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CCId", ccId);
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
        /// Gets a summary of all cost centers with income, expense, net profit, and budget variance.
        /// </summary>
        /// <param name="fromDate">Period start date.</param>
        /// <param name="toDate">Period end date.</param>
        /// <returns>DataTable with one row per cost center.</returns>
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
