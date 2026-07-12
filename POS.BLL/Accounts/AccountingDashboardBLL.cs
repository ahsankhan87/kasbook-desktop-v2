using POS.DLL;
using System;
using System.Data;

namespace POS.BLL
{
    public class AccountingDashboardData
    {
        public decimal CashBankBalance { get; set; }
        public decimal TotalReceivables { get; set; }
        public decimal TotalPayables { get; set; }
        public decimal Revenue { get; set; }
        public decimal Expenses { get; set; }
        public decimal NetProfit { get; set; }

        public DataTable MonthlyPnl { get; set; }
        public DataTable CashFlowTrend { get; set; }
        public DataTable ExpenseBreakdown { get; set; }
        public DataTable AttentionSummary { get; set; }
        public DataTable UnreconciledBankAccounts { get; set; }
        public DataTable RecentJournalActivity { get; set; }
    }

    public class AccountingDashboardBLL
    {
        private readonly AccountingDashboardDLL _dll = new AccountingDashboardDLL();

        public AccountingDashboardData GetDashboardData(DateTime fromDate, DateTime toDate)
        {
            var result = new AccountingDashboardData();

            DataTable kpis = _dll.GetKpis(fromDate, toDate);
            if (kpis != null && kpis.Rows.Count > 0)
            {
                DataRow row = kpis.Rows[0];
                result.CashBankBalance = ReadDecimal(row, "CashBankBalance");
                result.TotalReceivables = ReadDecimal(row, "TotalReceivables");
                result.TotalPayables = ReadDecimal(row, "TotalPayables");
                result.Revenue = ReadDecimal(row, "RevenuePeriod");
                result.Expenses = ReadDecimal(row, "ExpensesPeriod");
                result.NetProfit = result.Revenue - result.Expenses;
            }

            result.MonthlyPnl = _dll.GetMonthlyPnlComparison(toDate, 6);
            result.CashFlowTrend = _dll.GetCashFlowWeeklyTrend(toDate.AddMonths(-12).Date, toDate.Date);
            result.ExpenseBreakdown = _dll.GetExpenseBreakdown(fromDate, toDate, 5);
            result.AttentionSummary = _dll.GetAttentionSummary(toDate.Date);
            result.UnreconciledBankAccounts = _dll.GetUnreconciledBankAccounts(new DateTime(toDate.Year, toDate.Month, 1), toDate.Date);
            result.RecentJournalActivity = _dll.GetRecentJournalActivity(10);

            return result;
        }

        private static decimal ReadDecimal(DataRow row, string columnName)
        {
            if (row == null || !row.Table.Columns.Contains(columnName))
            {
                return 0m;
            }

            object value = row[columnName];
            return value == null || value == DBNull.Value ? 0m : Convert.ToDecimal(value);
        }
    }
}
