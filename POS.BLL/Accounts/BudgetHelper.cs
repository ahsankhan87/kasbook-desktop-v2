using System;
using System.Collections.Generic;
using System.Data;
using POS.Core;

namespace POS.BLL
{
    /// <summary>
    /// Helper class providing convenient budget operations and utilities
    /// </summary>
    public static class BudgetHelper
    {
        /// <summary>
        /// Gets the current month number (1-12)
        /// </summary>
        public static int GetCurrentMonth()
        {
            return DateTime.Now.Month;
        }

        /// <summary>
        /// Gets the current fiscal year ID based on system date
        /// </summary>
        public static int? GetCurrentFiscalYearId()
        {
            try
            {
                var fiscalYearBll = new FiscalYearBLL();
                DataTable dt = fiscalYearBll.GetAll();
                DataRow[] activeYear = dt.Select("status = 'Active'");

                if (activeYear.Length > 0)
                {
                    return Convert.ToInt32(activeYear[0]["id"]);
                }

                // If no active year, find the year containing today's date
                DateTime today = DateTime.Today;
                foreach (DataRow row in dt.Rows)
                {
                    DateTime startDate = Convert.ToDateTime(row["start_date"]);
                    DateTime endDate = Convert.ToDateTime(row["end_date"]);

                    if (today >= startDate && today <= endDate)
                    {
                        return Convert.ToInt32(row["id"]);
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Checks if a budget exists for the current fiscal year
        /// </summary>
        public static bool HasActiveBudget()
        {
            try
            {
                var budgetBll = new BudgetBLL();
                int? budgetId = budgetBll.GetActiveBudgetForPeriod(DateTime.Today);
                return budgetId.HasValue;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the budget status color code for UI display (hex format)
        /// </summary>
        public static string GetStatusColorHex(string status)
        {
            switch (status?.ToUpperInvariant())
            {
                case "DRAFT":
                    return "#808080"; // Gray
                case "APPROVED":
                    return "#0000FF"; // Blue
                case "ACTIVE":
                    return "#008000"; // Green
                default:
                    return "#000000"; // Black
            }
        }

        /// <summary>
        /// Formats a budget status for display with icon
        /// </summary>
        public static string GetStatusDisplay(string status)
        {
            switch (status?.ToUpperInvariant())
            {
                case "DRAFT":
                    return "📝 Draft";
                case "APPROVED":
                    return "✓ Approved";
                case "ACTIVE":
                    return "⭐ Active";
                default:
                    return status;
            }
        }

        /// <summary>
        /// Validates budget line data before save
        /// </summary>
        public static List<string> ValidateBudgetLines(List<BudgetLineModal> lines)
        {
            var errors = new List<string>();

            if (lines == null || lines.Count == 0)
            {
                errors.Add("Budget must have at least one account line");
                return errors;
            }

            // Check for duplicate accounts
            var accountIds = new HashSet<int>();
            foreach (var line in lines)
            {
                if (!accountIds.Add(line.account_id))
                {
                    errors.Add($"Duplicate account ID found: {line.account_id}");
                }

                // Check for negative amounts (optional - depends on business rules)
                decimal[] months = new[] 
                { 
                    line.jan, line.feb, line.mar, line.apr, 
                    line.may, line.jun, line.jul, line.aug, 
                    line.sep, line.oct, line.nov, line.dec 
                };

                foreach (var amount in months)
                {
                    if (amount < 0)
                    {
                        errors.Add($"Negative budget amount found for account {line.account_id}. Consider using positive amounts for both income and expenses.");
                        break;
                    }
                }
            }

            return errors;
        }

        /// <summary>
        /// Creates monthly percentages for equal distribution (8.33% each month)
        /// </summary>
        public static List<MonthlyPercentageModal> CreateEqualSpread()
        {
            var percentages = new List<MonthlyPercentageModal>();
            for (int i = 1; i <= 11; i++)
            {
                percentages.Add(new MonthlyPercentageModal { MonthNo = i, Percentage = 8.33m });
            }
            percentages.Add(new MonthlyPercentageModal { MonthNo = 12, Percentage = 8.37m }); // Adjust last month for 100% total
            return percentages;
        }

        /// <summary>
        /// Creates monthly percentages for quarterly distribution
        /// Q1: 20%, Q2: 25%, Q3: 30%, Q4: 25%
        /// </summary>
        public static List<MonthlyPercentageModal> CreateQuarterlySpread()
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

        /// <summary>
        /// Creates monthly percentages for retail seasonal pattern
        /// Higher in Q4 (holiday season), moderate in Q2, lower in Q1/Q3
        /// </summary>
        public static List<MonthlyPercentageModal> CreateRetailSeasonalSpread()
        {
            return new List<MonthlyPercentageModal>
            {
                new MonthlyPercentageModal { MonthNo = 1, Percentage = 6.00m },  // Jan - post-holiday
                new MonthlyPercentageModal { MonthNo = 2, Percentage = 5.00m },  // Feb - low
                new MonthlyPercentageModal { MonthNo = 3, Percentage = 7.00m },  // Mar - spring
                new MonthlyPercentageModal { MonthNo = 4, Percentage = 8.00m },  // Apr - moderate
                new MonthlyPercentageModal { MonthNo = 5, Percentage = 8.00m },  // May - moderate
                new MonthlyPercentageModal { MonthNo = 6, Percentage = 7.00m },  // Jun - moderate
                new MonthlyPercentageModal { MonthNo = 7, Percentage = 7.00m },  // Jul - moderate
                new MonthlyPercentageModal { MonthNo = 8, Percentage = 8.00m },  // Aug - back to school
                new MonthlyPercentageModal { MonthNo = 9, Percentage = 7.00m },  // Sep - moderate
                new MonthlyPercentageModal { MonthNo = 10, Percentage = 9.00m }, // Oct - ramp up
                new MonthlyPercentageModal { MonthNo = 11, Percentage = 13.00m },// Nov - holiday start
                new MonthlyPercentageModal { MonthNo = 12, Percentage = 15.00m } // Dec - peak holiday
            };
        }

        /// <summary>
        /// Gets the month name from month number
        /// </summary>
        public static string GetMonthName(int monthNo)
        {
            if (monthNo < 1 || monthNo > 12)
                return "Invalid";

            return new DateTime(2000, monthNo, 1).ToString("MMMM");
        }

        /// <summary>
        /// Extracts budget amount for a specific month
        /// </summary>
        public static decimal GetMonthAmount(BudgetLineModal line, int monthNo)
        {
            switch (monthNo)
            {
                case 1: return line.jan;
                case 2: return line.feb;
                case 3: return line.mar;
                case 4: return line.apr;
                case 5: return line.may;
                case 6: return line.jun;
                case 7: return line.jul;
                case 8: return line.aug;
                case 9: return line.sep;
                case 10: return line.oct;
                case 11: return line.nov;
                case 12: return line.dec;
                default: return 0m;
            }
        }

        /// <summary>
        /// Sets budget amount for a specific month
        /// </summary>
        public static void SetMonthAmount(BudgetLineModal line, int monthNo, decimal amount)
        {
            switch (monthNo)
            {
                case 1: line.jan = amount; break;
                case 2: line.feb = amount; break;
                case 3: line.mar = amount; break;
                case 4: line.apr = amount; break;
                case 5: line.may = amount; break;
                case 6: line.jun = amount; break;
                case 7: line.jul = amount; break;
                case 8: line.aug = amount; break;
                case 9: line.sep = amount; break;
                case 10: line.oct = amount; break;
                case 11: line.nov = amount; break;
                case 12: line.dec = amount; break;
            }
        }

        /// <summary>
        /// Distributes an annual amount equally across 12 months
        /// </summary>
        public static BudgetLineModal DistributeAnnualEqually(int accId, decimal annualAmount)
        {
            decimal monthlyAmount = Math.Round(annualAmount / 12, 2);
            decimal lastMonthAmount = annualAmount - (monthlyAmount * 11); // Adjust for rounding

            return new BudgetLineModal
            {
                account_id = accId,
                jan = monthlyAmount,
                feb = monthlyAmount,
                mar = monthlyAmount,
                apr = monthlyAmount,
                may = monthlyAmount,
                jun = monthlyAmount,
                jul = monthlyAmount,
                aug = monthlyAmount,
                sep = monthlyAmount,
                oct = monthlyAmount,
                nov = monthlyAmount,
                dec = lastMonthAmount
            };
        }

        /// <summary>
        /// Calculates variance percentage
        /// </summary>
        public static decimal CalculateVariancePct(decimal actual, decimal budget)
        {
            if (budget == 0)
                return 0m;

            return ((actual - budget) / budget) * 100m;
        }

        /// <summary>
        /// Gets variance status (favorable/unfavorable) for display
        /// </summary>
        public static string GetVarianceStatus(decimal variance, string accountType)
        {
            if (variance == 0)
                return "On Target";

            // For income/revenue accounts, positive variance is favorable
            if (accountType == "Income" || accountType == "Revenue")
            {
                return variance > 0 ? "Favorable" : "Unfavorable";
            }

            // For expense accounts, negative variance is favorable (spending less)
            if (accountType == "Expense")
            {
                return variance < 0 ? "Favorable" : "Unfavorable";
            }

            return variance > 0 ? "Over Budget" : "Under Budget";
        }

        /// <summary>
        /// Converts DataTable to BudgetLineModal list
        /// </summary>
        public static List<BudgetLineModal> DataTableToBudgetLines(DataTable dt)
        {
            var lines = new List<BudgetLineModal>();

            foreach (DataRow row in dt.Rows)
            {
                lines.Add(new BudgetLineModal
                {
                    line_id = Convert.ToInt32(row["line_id"]),
                    budget_id = Convert.ToInt32(row["budget_id"]),
                    account_id = row.Table.Columns.Contains("account_id")
                        ? Convert.ToInt32(row["account_id"])
                        : Convert.ToInt32(row["acc_id"]),
                    jan = Convert.ToDecimal(row["jan"]),
                    feb = Convert.ToDecimal(row["feb"]),
                    mar = Convert.ToDecimal(row["mar"]),
                    apr = Convert.ToDecimal(row["apr"]),
                    may = Convert.ToDecimal(row["may"]),
                    jun = Convert.ToDecimal(row["jun"]),
                    jul = Convert.ToDecimal(row["jul"]),
                    aug = Convert.ToDecimal(row["aug"]),
                    sep = Convert.ToDecimal(row["sep"]),
                    oct = Convert.ToDecimal(row["oct"]),
                    nov = Convert.ToDecimal(row["nov"]),
                    dec = Convert.ToDecimal(row["dec"]),
                    annual_total = Convert.ToDecimal(row["annual_total"])
                });
            }

            return lines;
        }

        /// <summary>
        /// Checks if user can approve budgets (CFO, Manager, Admin)
        /// </summary>
        public static bool CanApproveBudgets()
        {
            var role = (UsersModal.logged_in_user_role ?? string.Empty).Trim().ToLowerInvariant();
            return role == "administrator" || 
                   role == "admin" || 
                   role == "owner" || 
                   role == "cfo" || 
                   role == "manager";
        }

        /// <summary>
        /// Gets a formatted budget display name
        /// </summary>
        public static string FormatBudgetDisplayName(string budgetName, string fiscalYear, string version, string status)
        {
            return $"{budgetName} ({fiscalYear} {version}) - {status}";
        }
    }
}
