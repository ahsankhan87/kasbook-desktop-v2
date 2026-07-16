using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    /// <summary>
    /// Represents a budget header
    /// </summary>
    public class BudgetHeaderModal
    {
        public int budget_id { get; set; }
        public int financial_year_id { get; set; }
        public string budget_version { get; set; }
        public int? cc_id { get; set; }
        public string budget_name { get; set; }
        public string status { get; set; } // 'Draft', 'Approved', 'Active'
        public int? approved_by { get; set; }
        public DateTime? approved_at { get; set; }
        public string notes { get; set; }
        public int created_by { get; set; }
        public DateTime created_at { get; set; }
    }

    /// <summary>
    /// Represents a budget line with monthly allocations
    /// </summary>
    public class BudgetLineModal
    {
        public int line_id { get; set; }
        public int budget_id { get; set; }
        public int account_id { get; set; }
        public int acc_id { get { return account_id; } set { account_id = value; } }
        public decimal jan { get; set; }
        public decimal feb { get; set; }
        public decimal mar { get; set; }
        public decimal apr { get; set; }
        public decimal may { get; set; }
        public decimal jun { get; set; }
        public decimal jul { get; set; }
        public decimal aug { get; set; }
        public decimal sep { get; set; }
        public decimal oct { get; set; }
        public decimal nov { get; set; }
        public decimal dec { get; set; }
        public decimal annual_total { get; set; }
    }

    /// <summary>
    /// Represents a variance note for explaining budget differences
    /// </summary>
    public class BudgetVarianceNoteModal
    {
        public int note_id { get; set; }
        public int budget_id { get; set; }
        public int account_id { get; set; }
        public int acc_id { get { return account_id; } set { account_id = value; } }
        public int period_month { get; set; }
        public int period_year { get; set; }
        public string variance_note { get; set; }
        public int added_by { get; set; }
        public DateTime added_at { get; set; }
    }

    /// <summary>
    /// Represents budget vs actual comparison result
    /// </summary>
    public class BudgetVsActualModal
    {
        public string acc_code { get; set; }
        public string acc_name { get; set; }
        public string account_type { get; set; }
        public decimal annual_budget { get; set; }
        public decimal ytd_budget { get; set; }
        public decimal ytd_actual { get; set; }
        public decimal ytd_variance { get; set; }
        public decimal ytd_variance_pct { get; set; }
        public decimal monthly_budget { get; set; }
        public decimal monthly_actual { get; set; }
        public decimal monthly_variance { get; set; }
        public decimal full_year_forecast { get; set; }
    }

    /// <summary>
    /// Represents monthly budget detail for a specific account
    /// </summary>
    public class BudgetMonthlyDetailModal
    {
        public int MonthNo { get; set; }
        public string MonthName { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal Variance { get; set; }
        public decimal CumulativeBudget { get; set; }
        public decimal CumulativeActual { get; set; }
    }

    /// <summary>
    /// Represents budget summary KPIs
    /// </summary>
    public class BudgetSummaryKPIsModal
    {
        public decimal TotalIncomeBudget { get; set; }
        public decimal TotalIncomeActual { get; set; }
        public decimal TotalExpenseBudget { get; set; }
        public decimal TotalExpenseActual { get; set; }
        public decimal NetProfitBudget { get; set; }
        public decimal NetProfitActual { get; set; }
        public decimal OverallAchievementPct { get; set; }
    }

    /// <summary>
    /// Represents monthly percentage for seasonal budget spreading
    /// </summary>
    public class MonthlyPercentageModal
    {
        public int MonthNo { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Result of budget exceeded check
    /// </summary>
    public class BudgetExceededCheckModal
    {
        public bool IsExceeded { get; set; }
        public decimal CurrentBudget { get; set; }
        public decimal CurrentActual { get; set; }
        public decimal NewAmount { get; set; }
        public decimal RemainingBudget { get; set; }
        public decimal ExcessAmount { get; set; }
        public string Message { get; set; }
    }
}
