using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using POS.Core;
using POS.DLL;

namespace POS.BLL
{
    public class BudgetBLL
    {
        private readonly BudgetDLL _budgetDll = new BudgetDLL();
        private readonly GeneralBLL _generalBll = new GeneralBLL();

        /// <summary>
        /// Gets all budget headers
        /// </summary>
        public DataTable GetAllBudgetHeaders()
        {
            try
            {
                return _budgetDll.GetAllBudgetHeaders();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a specific budget header by ID
        /// </summary>
        public DataTable GetBudgetHeaderById(int budgetId)
        {
            try
            {
                return _budgetDll.GetBudgetHeaderById(budgetId);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets budget lines for a specific budget
        /// </summary>
        public DataTable GetBudgetLines(int budgetId)
        {
            try
            {
                return _budgetDll.GetBudgetLines(budgetId);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Saves a budget header (insert or update)
        /// </summary>
        public int SaveBudgetHeader(BudgetHeaderModal modal)
        {
            try
            {
                if (modal.budget_id > 0)
                {
                    _budgetDll.UpdateBudgetHeader(modal);
                    return modal.budget_id;
                }
                else
                {
                    modal.created_at = DateTime.Now;
                    return _budgetDll.InsertBudgetHeader(modal);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Saves budget lines using SqlBulkCopy for performance
        /// </summary>
        public void SaveBudgetLines(int budgetId, List<BudgetLineModal> lines)
        {
            try
            {
                // Create DataTable for bulk copy
                DataTable dt = new DataTable();
                dt.Columns.Add("budget_id", typeof(int));
                dt.Columns.Add("account_id", typeof(int));
                dt.Columns.Add("jan", typeof(decimal));
                dt.Columns.Add("feb", typeof(decimal));
                dt.Columns.Add("mar", typeof(decimal));
                dt.Columns.Add("apr", typeof(decimal));
                dt.Columns.Add("may", typeof(decimal));
                dt.Columns.Add("jun", typeof(decimal));
                dt.Columns.Add("jul", typeof(decimal));
                dt.Columns.Add("aug", typeof(decimal));
                dt.Columns.Add("sep", typeof(decimal));
                dt.Columns.Add("oct", typeof(decimal));
                dt.Columns.Add("nov", typeof(decimal));
                dt.Columns.Add("dec", typeof(decimal));

                foreach (var line in lines)
                {
                    dt.Rows.Add(
                        budgetId,
                        line.account_id,
                        line.jan,
                        line.feb,
                        line.mar,
                        line.apr,
                        line.may,
                        line.jun,
                        line.jul,
                        line.aug,
                        line.sep,
                        line.oct,
                        line.nov,
                        line.dec
                    );
                }

                _budgetDll.SaveBudgetLinesBulk(budgetId, dt);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes a budget
        /// </summary>
        public void DeleteBudget(int budgetId)
        {
            try
            {
                // Check if budget can be deleted (only Draft budgets)
                DataTable budgetDt = _budgetDll.GetBudgetHeaderById(budgetId);
                if (budgetDt.Rows.Count == 0)
                    throw new Exception("Budget not found");

                string status = Convert.ToString(budgetDt.Rows[0]["status"]);
                if (status != "Draft")
                    throw new Exception("Only Draft budgets can be deleted. Current status: " + status);

                _budgetDll.DeleteBudgetHeader(budgetId);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Approves a budget (requires CFO/Manager role)
        /// </summary>
        public void ApproveBudget(int budgetId, int userId)
        {
            try
            {
                // Role check
                if (!IsAuthorizedForBudgetApproval(userId))
                    throw new UnauthorizedAccessException("Only CFO, Manager, or Administrator can approve budgets");

                // Check current status
                DataTable budgetDt = _budgetDll.GetBudgetHeaderById(budgetId);
                if (budgetDt.Rows.Count == 0)
                    throw new Exception("Budget not found");

                string status = Convert.ToString(budgetDt.Rows[0]["status"]);
                if (status == "Approved" || status == "Active")
                    throw new Exception("Budget is already approved");

                _budgetDll.ApproveBudget(budgetId, userId);

                // Log action
                Log.LogAction("Approve Budget", $"Budget ID {budgetId} approved", userId, UsersModal.logged_in_branch_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Activates a budget (sets it as the active budget for the fiscal year)
        /// </summary>
        public void ActivateBudget(int budgetId, int userId)
        {
            try
            {
                // Role check
                if (!IsAuthorizedForBudgetApproval(userId))
                    throw new UnauthorizedAccessException("Only CFO, Manager, or Administrator can activate budgets");

                // Check current status
                DataTable budgetDt = _budgetDll.GetBudgetHeaderById(budgetId);
                if (budgetDt.Rows.Count == 0)
                    throw new Exception("Budget not found");

                string status = Convert.ToString(budgetDt.Rows[0]["status"]);
                if (status == "Draft")
                    throw new Exception("Budget must be approved before activation");

                _budgetDll.ActivateBudget(budgetId);

                // Log action
                Log.LogAction("Activate Budget", $"Budget ID {budgetId} activated", userId, UsersModal.logged_in_branch_id);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Copies budget from last year's actuals with optional growth percentage
        /// </summary>
        public DataTable CopyFromLastYear(int sourceYearId, int targetBudgetId, decimal growthPct)
        {
            try
            {
                return _budgetDll.CopyBudgetFromActuals(sourceYearId, targetBudgetId, growthPct);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the active budget for a specific date
        /// </summary>
        public int? GetActiveBudgetForPeriod(DateTime date, int? ccId = null)
        {
            try
            {
                DataTable dt = _budgetDll.GetActiveBudgetForPeriod(date, ccId);
                if (dt.Rows.Count == 0)
                    return null;

                return Convert.ToInt32(dt.Rows[0]["budget_id"]);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if a journal entry will exceed budget and returns warning info
        /// </summary>
        public BudgetExceededCheckModal CheckBudgetExceeded(int accId, decimal newAmount, DateTime date, int? ccId = null)
        {
            var result = new BudgetExceededCheckModal
            {
                IsExceeded = false,
                CurrentBudget = 0,
                CurrentActual = 0,
                NewAmount = newAmount,
                RemainingBudget = 0,
                ExcessAmount = 0,
                Message = string.Empty
            };

            try
            {
                // Get active budget for the period
                int? budgetId = GetActiveBudgetForPeriod(date, ccId);
                if (!budgetId.HasValue)
                {
                    result.Message = "No active budget found for this period";
                    return result;
                }

                // Get budget vs actual for this account
                DataTable budgetVsActual = _budgetDll.GetBudgetVsActual(budgetId.Value, date, date, ccId);
                DataRow[] accountRows = budgetVsActual.Select($"acc_code = (SELECT code FROM acc_accounts WHERE id = {accId})");

                if (accountRows.Length == 0)
                {
                    result.Message = "Account not found in budget";
                    return result;
                }

                DataRow row = accountRows[0];
                decimal monthlyBudget = Convert.ToDecimal(row["monthly_budget"]);
                decimal monthlyActual = Convert.ToDecimal(row["monthly_actual"]);

                result.CurrentBudget = monthlyBudget;
                result.CurrentActual = monthlyActual;
                result.RemainingBudget = monthlyBudget - monthlyActual;

                decimal projectedTotal = monthlyActual + newAmount;

                if (projectedTotal > monthlyBudget)
                {
                    result.IsExceeded = true;
                    result.ExcessAmount = projectedTotal - monthlyBudget;
                    result.Message = $"Warning: This entry will exceed budget by {result.ExcessAmount:N2}. Budget: {monthlyBudget:N2}, Current: {monthlyActual:N2}, New Total: {projectedTotal:N2}";
                }
                else
                {
                    result.Message = $"Within budget. Remaining: {result.RemainingBudget:N2}";
                }

                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets budget vs actual comparison
        /// </summary>
        public DataTable GetBudgetVsActual(int budgetId, DateTime fromDate, DateTime toDate, int? ccId = null)
        {
            try
            {
                return _budgetDll.GetBudgetVsActual(budgetId, fromDate, toDate, ccId);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets budget vs actual comparison with period and account-type filters
        /// </summary>
        public DataTable GetBudgetVsActual(int budgetId, DateTime fromDate, DateTime toDate, int? ccId, string periodMode, string accountTypeFilter)
        {
            try
            {
                return _budgetDll.GetBudgetVsActual(budgetId, fromDate, toDate, ccId, periodMode, accountTypeFilter);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets monthly budget detail for a specific account
        /// </summary>
        public DataTable GetBudgetMonthlyDetail(int budgetId, int accId)
        {
            try
            {
                return _budgetDll.GetBudgetMonthlyDetail(budgetId, accId);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Applies seasonal spread to a budget line
        /// </summary>
        public DataTable ApplySeasonalSpread(int budgetId, int accId, decimal annualAmount, List<MonthlyPercentageModal> percentages)
        {
            try
            {
                // Validate percentages sum to 100
                decimal totalPct = percentages.Sum(p => p.Percentage);
                if (Math.Abs(totalPct - 100m) > 0.01m)
                    throw new Exception($"Seasonal percentages must sum to 100%. Current sum: {totalPct}");

                return _budgetDll.ApplySeasonalSpread(budgetId, accId, annualAmount, percentages);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets budget summary KPIs
        /// </summary>
        public BudgetSummaryKPIsModal GetBudgetSummaryKPIs(int budgetId, DateTime asOfDate)
        {
            try
            {
                DataTable dt = _budgetDll.GetBudgetSummaryKPIs(budgetId, asOfDate);
                if (dt.Rows.Count == 0)
                    return new BudgetSummaryKPIsModal();

                DataRow row = dt.Rows[0];
                return new BudgetSummaryKPIsModal
                {
                    TotalIncomeBudget = Convert.ToDecimal(row["TotalIncomeBudget"]),
                    TotalIncomeActual = Convert.ToDecimal(row["TotalIncomeActual"]),
                    TotalExpenseBudget = Convert.ToDecimal(row["TotalExpenseBudget"]),
                    TotalExpenseActual = Convert.ToDecimal(row["TotalExpenseActual"]),
                    NetProfitBudget = Convert.ToDecimal(row["NetProfitBudget"]),
                    NetProfitActual = Convert.ToDecimal(row["NetProfitActual"]),
                    OverallAchievementPct = Convert.ToDecimal(row["OverallAchievementPct"])
                };
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets variance notes
        /// </summary>
        public DataTable GetVarianceNotes(int budgetId, int? accId = null)
        {
            try
            {
                return _budgetDll.GetVarianceNotes(budgetId, accId);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adds a variance note
        /// </summary>
        public void AddVarianceNote(int budgetId, int accId, int periodMonth, int periodYear, string note, int userId)
        {
            try
            {
                var modal = new BudgetVarianceNoteModal
                {
                    budget_id = budgetId,
                    account_id = accId,
                    period_month = periodMonth,
                    period_year = periodYear,
                    variance_note = note,
                    added_by = userId,
                    added_at = DateTime.Now
                };

                _budgetDll.InsertVarianceNote(modal);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if user is authorized for budget approval
        /// </summary>
        private bool IsAuthorizedForBudgetApproval(int userId)
        {
            try
            {
                // Get user role from UsersModal or database
                var role = (UsersModal.logged_in_user_role ?? string.Empty).Trim().ToLowerInvariant();
                return role == "administrator" || 
                       role == "admin" || 
                       role == "owner" || 
                       role == "cfo" || 
                       role == "manager";
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets budgets by fiscal year
        /// </summary>
        public DataTable GetBudgetsByFiscalYear(int fiscalYearId)
        {
            try
            {
                DataTable allBudgets = GetAllBudgetHeaders();
                DataView dv = allBudgets.DefaultView;
                dv.RowFilter = $"financial_year_id = {fiscalYearId}";
                return dv.ToTable();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets budgets by status
        /// </summary>
        public DataTable GetBudgetsByStatus(string status)
        {
            try
            {
                DataTable allBudgets = GetAllBudgetHeaders();
                DataView dv = allBudgets.DefaultView;
                dv.RowFilter = $"status = '{status.Replace("'", "''")}'";
                return dv.ToTable();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a default equal-spread monthly percentage list (8.33% per month)
        /// </summary>
        public List<MonthlyPercentageModal> GetEqualSpreadPercentages()
        {
            var percentages = new List<MonthlyPercentageModal>();
            for (int i = 1; i <= 12; i++)
            {
                percentages.Add(new MonthlyPercentageModal
                {
                    MonthNo = i,
                    Percentage = 8.33m
                });
            }
            // Adjust last month to ensure total is exactly 100%
            percentages[11].Percentage = 8.37m;
            return percentages;
        }

        /// <summary>
        /// Creates a quarterly-based spread (Q1: 20%, Q2: 25%, Q3: 30%, Q4: 25%)
        /// </summary>
        public List<MonthlyPercentageModal> GetQuarterlySpreadPercentages()
        {
            return new List<MonthlyPercentageModal>
            {
                new MonthlyPercentageModal { MonthNo = 1, Percentage = 6.67m },
                new MonthlyPercentageModal { MonthNo = 2, Percentage = 6.67m },
                new MonthlyPercentageModal { MonthNo = 3, Percentage = 6.66m },
                new MonthlyPercentageModal { MonthNo = 4, Percentage = 8.33m },
                new MonthlyPercentageModal { MonthNo = 5, Percentage = 8.33m },
                new MonthlyPercentageModal { MonthNo = 6, Percentage = 8.34m },
                new MonthlyPercentageModal { MonthNo = 7, Percentage = 10.00m },
                new MonthlyPercentageModal { MonthNo = 8, Percentage = 10.00m },
                new MonthlyPercentageModal { MonthNo = 9, Percentage = 10.00m },
                new MonthlyPercentageModal { MonthNo = 10, Percentage = 8.33m },
                new MonthlyPercentageModal { MonthNo = 11, Percentage = 8.33m },
                new MonthlyPercentageModal { MonthNo = 12, Percentage = 8.34m }
            };
        }
    }
}
