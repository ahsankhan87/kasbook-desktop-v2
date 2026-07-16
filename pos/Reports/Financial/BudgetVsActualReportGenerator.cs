using System;
using System.Data;
using System.Globalization;
using System.Linq;

namespace pos.Reports.Financial
{
    internal sealed class BudgetVsActualReportSummary
    {
        public decimal TotalIncomeBudget { get; set; }
        public decimal TotalIncomeActual { get; set; }
        public decimal TotalExpenseBudget { get; set; }
        public decimal TotalExpenseActual { get; set; }
        public decimal NetProfitBudget { get; set; }
        public decimal NetProfitActual { get; set; }
        public decimal OverallAchievementPct { get; set; }
        public string HealthStatus { get; set; }
    }

    internal sealed class BudgetVsActualReportGenerator
    {
        public DataTable BuildReportTable(DataTable source, decimal varianceThresholdPct)
        {
            DataTable report = new DataTable();
            report.Columns.Add("acc_id", typeof(int));
            report.Columns.Add("acc_code", typeof(string));
            report.Columns.Add("acc_name", typeof(string));
            report.Columns.Add("account_type", typeof(string));
            report.Columns.Add("annual_budget", typeof(decimal));
            report.Columns.Add("ytd_budget", typeof(decimal));
            report.Columns.Add("ytd_actual", typeof(decimal));
            report.Columns.Add("ytd_variance", typeof(decimal));
            report.Columns.Add("ytd_variance_pct", typeof(decimal));
            report.Columns.Add("monthly_budget", typeof(decimal));
            report.Columns.Add("monthly_actual", typeof(decimal));
            report.Columns.Add("monthly_variance", typeof(decimal));
            report.Columns.Add("full_year_forecast", typeof(decimal));
            report.Columns.Add("period_month", typeof(int));
            report.Columns.Add("period_year", typeof(int));
            report.Columns.Add("is_favorable", typeof(bool));
            report.Columns.Add("variance_bar_pct", typeof(decimal));
            report.Columns.Add("is_high_variance", typeof(bool));
            report.Columns.Add("variance_icon", typeof(string));

            if (source == null)
                return report;

            foreach (DataRow src in source.Rows)
            {
                decimal ytdVariancePct = ReadDecimal(src, "ytd_variance_pct");
                decimal varianceBarPct = Math.Min(100m, Math.Abs(ytdVariancePct));
                bool highVariance = Math.Abs(ytdVariancePct) >= varianceThresholdPct;

                DataRow row = report.NewRow();
                row["acc_id"] = ReadInt(src, "acc_id");
                row["acc_code"] = ReadString(src, "acc_code");
                row["acc_name"] = ReadString(src, "acc_name");
                row["account_type"] = ReadString(src, "account_type");
                row["annual_budget"] = ReadDecimal(src, "annual_budget");
                row["ytd_budget"] = ReadDecimal(src, "ytd_budget");
                row["ytd_actual"] = ReadDecimal(src, "ytd_actual");
                row["ytd_variance"] = ReadDecimal(src, "ytd_variance");
                row["ytd_variance_pct"] = ytdVariancePct;
                row["monthly_budget"] = ReadDecimal(src, "monthly_budget");
                row["monthly_actual"] = ReadDecimal(src, "monthly_actual");
                row["monthly_variance"] = ReadDecimal(src, "monthly_variance");
                row["full_year_forecast"] = ReadDecimal(src, "full_year_forecast");
                row["period_month"] = ReadInt(src, "period_month");
                row["period_year"] = ReadInt(src, "period_year");
                row["is_favorable"] = ReadInt(src, "is_favorable") == 1;
                row["variance_bar_pct"] = varianceBarPct;
                row["is_high_variance"] = highVariance;
                row["variance_icon"] = highVariance ? "📝" : string.Empty;
                report.Rows.Add(row);
            }

            return report;
        }

        public BudgetVsActualReportSummary BuildSummary(DataTable source)
        {
            var summary = new BudgetVsActualReportSummary
            {
                HealthStatus = "Amber"
            };

            if (source == null || source.Rows.Count == 0)
                return summary;

            decimal incomeBudget = 0m;
            decimal incomeActual = 0m;
            decimal expenseBudget = 0m;
            decimal expenseActual = 0m;

            foreach (DataRow row in source.Rows)
            {
                string accountType = ReadString(row, "account_type").ToLowerInvariant();
                decimal budget = ReadDecimal(row, "ytd_budget");
                decimal actual = ReadDecimal(row, "ytd_actual");

                if (accountType.Contains("income") || accountType.Contains("revenue"))
                {
                    incomeBudget += budget;
                    incomeActual += actual;
                }
                else
                {
                    expenseBudget += budget;
                    expenseActual += actual;
                }
            }

            summary.TotalIncomeBudget = incomeBudget;
            summary.TotalIncomeActual = incomeActual;
            summary.TotalExpenseBudget = expenseBudget;
            summary.TotalExpenseActual = expenseActual;
            summary.NetProfitBudget = incomeBudget - expenseBudget;
            summary.NetProfitActual = incomeActual - expenseActual;

            decimal totalBudget = incomeBudget + expenseBudget;
            decimal totalActual = incomeActual + expenseActual;
            summary.OverallAchievementPct = totalBudget == 0m ? 0m : (totalActual / totalBudget) * 100m;

            if (summary.OverallAchievementPct >= 98m)
                summary.HealthStatus = "Green";
            else if (summary.OverallAchievementPct < 90m)
                summary.HealthStatus = "Red";

            return summary;
        }

        public DataTable BuildExportTable(DataTable source)
        {
            if (source == null)
                return new DataTable();

            DataTable export = source.Copy();
            if (!export.Columns.Contains("variance_highlight"))
            {
                export.Columns.Add("variance_highlight", typeof(string));
            }

            foreach (DataRow row in export.Rows)
            {
                decimal variancePct = ReadDecimal(row, "ytd_variance_pct");
                row["variance_highlight"] = Math.Abs(variancePct) >= 15m
                    ? string.Format(CultureInfo.InvariantCulture, "High variance ({0:N2}%)", variancePct)
                    : string.Empty;
            }

            return export;
        }

        private static decimal ReadDecimal(DataRow row, string column)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(column) || row[column] == DBNull.Value)
                return 0m;

            decimal value;
            return decimal.TryParse(Convert.ToString(row[column], CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out value)
                ? value
                : 0m;
        }

        private static int ReadInt(DataRow row, string column)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(column) || row[column] == DBNull.Value)
                return 0;

            int value;
            return int.TryParse(Convert.ToString(row[column], CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out value)
                ? value
                : 0;
        }

        private static string ReadString(DataRow row, string column)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(column) || row[column] == DBNull.Value)
                return string.Empty;

            return Convert.ToString(row[column]);
        }
    }
}
