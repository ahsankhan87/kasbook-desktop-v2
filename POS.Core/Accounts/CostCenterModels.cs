using System;
using System.Collections.Generic;

namespace POS.Core
{
    /// <summary>
    /// Represents a Cost Center (department, profit center, or service unit).
    /// Used for departmental P&L, budget allocation, and expense tracking.
    /// </summary>
    public class CostCenterModel
    {
        /// <summary>
        /// Primary key. Auto-generated on creation.
        /// </summary>
        public int CcId { get; set; }

        /// <summary>
        /// Unique code identifier for the cost center (e.g., "CC-001", "DEPT-SALES").
        /// Required and must be unique within the system.
        /// </summary>
        public string CcCode { get; set; }

        /// <summary>
        /// Human-readable name (e.g., "Sales Department", "Marketing").
        /// Required.
        /// </summary>
        public string CcName { get; set; }

        /// <summary>
        /// Classification type (e.g., "Department", "Profit Center", "Service Unit").
        /// Used for grouping and reporting.
        /// </summary>
        public string CcType { get; set; }

        /// <summary>
        /// Parent cost center ID for hierarchical organization.
        /// Null if this is a root cost center. Validated to prevent circular references.
        /// </summary>
        public int? ParentCcId { get; set; }

        /// <summary>
        /// User ID of the cost center manager.
        /// Optional; used for approval workflows and notifications.
        /// </summary>
        public int? ManagerId { get; set; }

        /// <summary>
        /// Monthly budget amount allocated to this cost center.
        /// Deprecated in favor of acc_cost_center_budgets table for per-account tracking.
        /// </summary>
        public decimal? MonthlyBudget { get; set; }

        /// <summary>
        /// Effective start date for this cost center.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date if the cost center is discontinued.
        /// Null if still active.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Flag indicating if the cost center is active.
        /// Inactive cost centers are excluded from dropdowns and reporting.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Optional description or notes about the cost center.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// UTC timestamp when the cost center was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Hierarchical path representation (e.g., "CC-001 > CC-002").
        /// Populated by reporting queries; not stored in DB.
        /// </summary>
        public string PathCode { get; set; }

        /// <summary>
        /// Depth in the hierarchy (0 = root).
        /// </summary>
        public int LevelNo { get; set; }
    }

    /// <summary>
    /// Represents a monthly budget for a specific account within a cost center for a fiscal year.
    /// One row per account per cost center per year, with 12 monthly amounts.
    /// </summary>
    public class AccountBudget
    {
        /// <summary>
        /// Primary key. Auto-generated.
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        /// Cost center ID. Required.
        /// </summary>
        public int CcId { get; set; }

        /// <summary>
        /// Fiscal year ID. Required.
        /// </summary>
        public int FinancialYearId { get; set; }

        /// <summary>
        /// GL Account ID. Required. Must be an Income or Expense account.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// January budget amount. Non-negative.
        /// </summary>
        public decimal JanBudget { get; set; }

        /// <summary>
        /// February budget amount. Non-negative.
        /// </summary>
        public decimal FebBudget { get; set; }

        /// <summary>
        /// March budget amount. Non-negative.
        /// </summary>
        public decimal MarBudget { get; set; }

        /// <summary>
        /// April budget amount. Non-negative.
        /// </summary>
        public decimal AprBudget { get; set; }

        /// <summary>
        /// May budget amount. Non-negative.
        /// </summary>
        public decimal MayBudget { get; set; }

        /// <summary>
        /// June budget amount. Non-negative.
        /// </summary>
        public decimal JunBudget { get; set; }

        /// <summary>
        /// July budget amount. Non-negative.
        /// </summary>
        public decimal JulBudget { get; set; }

        /// <summary>
        /// August budget amount. Non-negative.
        /// </summary>
        public decimal AugBudget { get; set; }

        /// <summary>
        /// September budget amount. Non-negative.
        /// </summary>
        public decimal SepBudget { get; set; }

        /// <summary>
        /// October budget amount. Non-negative.
        /// </summary>
        public decimal OctBudget { get; set; }

        /// <summary>
        /// November budget amount. Non-negative.
        /// </summary>
        public decimal NovBudget { get; set; }

        /// <summary>
        /// December budget amount. Non-negative.
        /// </summary>
        public decimal DecBudget { get; set; }

        /// <summary>
        /// User ID who created this budget.
        /// </summary>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// UTC timestamp when this budget was created or last modified.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Retrieves the budget amount for a specific month (1-12).
        /// </summary>
        public decimal GetMonthBudget(int month)
        {
            switch (month)
            {
                case 1: return JanBudget;
                case 2: return FebBudget;
                case 3: return MarBudget;
                case 4: return AprBudget;
                case 5: return MayBudget;
                case 6: return JunBudget;
                case 7: return JulBudget;
                case 8: return AugBudget;
                case 9: return SepBudget;
                case 10: return OctBudget;
                case 11: return NovBudget;
                case 12: return DecBudget;
                default: return 0;
            }
        }

        /// <summary>
        /// Sets the budget amount for a specific month (1-12).
        /// </summary>
        public void SetMonthBudget(int month, decimal amount)
        {
            switch (month)
            {
                case 1: JanBudget = amount; break;
                case 2: FebBudget = amount; break;
                case 3: MarBudget = amount; break;
                case 4: AprBudget = amount; break;
                case 5: MayBudget = amount; break;
                case 6: JunBudget = amount; break;
                case 7: JulBudget = amount; break;
                case 8: AugBudget = amount; break;
                case 9: SepBudget = amount; break;
                case 10: OctBudget = amount; break;
                case 11: NovBudget = amount; break;
                case 12: DecBudget = amount; break;
            }
        }

        /// <summary>
        /// Returns the annual budget total across all 12 months.
        /// </summary>
        public decimal GetAnnualBudget()
        {
            return JanBudget + FebBudget + MarBudget + AprBudget + MayBudget + JunBudget
                 + JulBudget + AugBudget + SepBudget + OctBudget + NovBudget + DecBudget;
        }
    }

    /// <summary>
    /// Result of an automatic expense allocation run for a period.
    /// Contains allocation summary and per-department breakdowns.
    /// </summary>
    public class AllocationResult
    {
        /// <summary>
        /// Period start date (1st of month).
        /// </summary>
        public DateTime PeriodStart { get; set; }

        /// <summary>
        /// Period end date (exclusive, first of next month).
        /// </summary>
        public DateTime PeriodEnd { get; set; }

        /// <summary>
        /// Allocation voucher number generated (e.g., "CCA-202501-A1B2C3D4").
        /// </summary>
        public string VoucherNo { get; set; }

        /// <summary>
        /// Entry header ID of the allocation voucher.
        /// </summary>
        public int EntryHeaderId { get; set; }

        /// <summary>
        /// Total amount allocated across all cost centers.
        /// Validates that this matches sum of departmental allocations.
        /// </summary>
        public decimal TotalAllocated { get; set; }

        /// <summary>
        /// Success flag. False if validation failed or no allocation rules found.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Message describing the result (success message or error reason).
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Per-department allocation details.
        /// Each entry shows source account, target cost center, and allocated amount.
        /// </summary>
        public List<AllocationResultRow> Allocations { get; set; } = new List<AllocationResultRow>();
    }

    /// <summary>
    /// Single row of an allocation result, showing one department's allocation.
    /// </summary>
    public class AllocationResultRow
    {
        /// <summary>
        /// Allocation rule ID.
        /// </summary>
        public int AllocationRuleId { get; set; }

        /// <summary>
        /// Allocation rule name (e.g., "Rent Allocation to Departments").
        /// </summary>
        public string AllocationName { get; set; }

        /// <summary>
        /// Source GL account ID (expense to be allocated).
        /// </summary>
        public int SourceAccountId { get; set; }

        /// <summary>
        /// Target cost center ID.
        /// </summary>
        public int CostCenterId { get; set; }

        /// <summary>
        /// Allocation method used (FIXED_PCT, HEADCOUNT, REVENUE).
        /// </summary>
        public string AllocationMethod { get; set; }

        /// <summary>
        /// Configured allocation percentage for FIXED_PCT method.
        /// </summary>
        public decimal AllocationPercent { get; set; }

        /// <summary>
        /// Total unallocated source amount before distribution.
        /// </summary>
        public decimal SourceAmount { get; set; }

        /// <summary>
        /// Amount allocated to this cost center.
        /// </summary>
        public decimal AllocatedAmount { get; set; }
    }

    /// <summary>
    /// Budget alert indicating that an account has exceeded its monthly budget.
    /// Displayed as a warning in the journal entry form when posting to an over-budget cost center.
    /// </summary>
    public class BudgetAlertModel
    {
        /// <summary>
        /// Cost center ID.
        /// </summary>
        public int CcId { get; set; }

        /// <summary>
        /// Cost center code.
        /// </summary>
        public string CcCode { get; set; }

        /// <summary>
        /// GL Account ID.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// GL Account code (e.g., "5001").
        /// </summary>
        public string AccountCode { get; set; }

        /// <summary>
        /// GL Account name (e.g., "Salaries Expense").
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// Current month (1-12).
        /// </summary>
        public int CurrentMonth { get; set; }

        /// <summary>
        /// Budgeted amount for this account in the current month.
        /// </summary>
        public decimal BudgetAmount { get; set; }

        /// <summary>
        /// Actual amount posted to this account for the current month (to date).
        /// </summary>
        public decimal ActualAmount { get; set; }

        /// <summary>
        /// Amount by which actual exceeds budget. Always >= 0.
        /// </summary>
        public decimal OverspendAmount { get; set; }

        /// <summary>
        /// Overspend as a percentage of budget. E.g., 125 means 25% over budget.
        /// </summary>
        public decimal OverspendPercent { get; set; }

        /// <summary>
        /// Severity level: "Info", "Warning", "Critical".
        /// Critical = overspend > 20%; Warning = overspend > 5%.
        /// </summary>
        public string SeverityLevel { get; set; }
    }

    /// <summary>
    /// Result of checking budget availability before posting a journal entry.
    /// Called from JournalsBLL.PostJournalVoucher.
    /// </summary>
    public class BudgetCheckResult
    {
        /// <summary>
        /// True if the entry would exceed the account's budget for the month.
        /// </summary>
        public bool IsOverBudget { get; set; }

        /// <summary>
        /// Remaining budget available (can be negative if already over budget).
        /// </summary>
        public decimal RemainingBudget { get; set; }

        /// <summary>
        /// Monthly budget amount for this account.
        /// </summary>
        public decimal MonthlyBudget { get; set; }

        /// <summary>
        /// Current month's actual amount (before this entry).
        /// </summary>
        public decimal CurrentActual { get; set; }

        /// <summary>
        /// Description of the condition for display in the UI.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Severity: "Info", "Warning", "Critical", or null if no budget is defined.
        /// </summary>
        public string SeverityLevel { get; set; }
    }
}
