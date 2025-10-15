using System;
using System.Collections.Generic;
using System.Data;
using POS.Core;
using POS.DLL.Dashboard;
using POS.BLL;

namespace POS.BLL.Dashboard
{
    public class DashboardMetrics
    {
        public decimal TodayAmount { get; set; }
        public decimal MonthlyAmount { get; set; }
        public int LowStockCount { get; set; }
        public List<RecentLogItem> RecentLogs { get; set; } = new List<RecentLogItem>();
    }

    public class RecentLogItem
    {
        public string Area { get; set; }        // maps from Action
        public string Description { get; set; } // maps from Details
        public DateTime Timestamp { get; set; }
    }

    public class DashboardBLL
    {
        public DashboardMetrics GetDashboardMetrics(int recentLogsToTake = 10)
        {
            var branchId = UsersModal.logged_in_branch_id;
            var userId = UsersModal.logged_in_userid;

            var today = DateTime.Today;
            var monthStart = new DateTime(today.Year, today.Month, 1);
            var nextMonthStart = monthStart.AddMonths(1);

            // Sales amounts
            var dll = new DashboardDLL();
            var sales = dll.GetSalesAmounts(branchId, today, monthStart, nextMonthStart);

            // Low stock via existing warehouse report (OperationType=2)
            var warehouse = new WarehouseReportBLL();
            var ds = warehouse.InventoryReport(branchId, userId, null, null, null, 2);
            var lowStockCount = (ds != null && ds.Tables.Count > 0 && ds.Tables["StockReport"] != null)
                ? ds.Tables["StockReport"].Rows.Count
                : 0;

            // Recent activity logs
            var recentLogs = new List<RecentLogItem>();
            var logsDt = dll.GetRecentLogs(branchId, recentLogsToTake);
            foreach (DataRow r in logsDt.Rows)
            {
                recentLogs.Add(new RecentLogItem
                {
                    Area = r["Action"]?.ToString(),
                    Description = r["Details"]?.ToString(),
                    Timestamp = r["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(r["Timestamp"])
                });
            }

            return new DashboardMetrics
            {
                TodayAmount = sales.TodayAmount,
                MonthlyAmount = sales.MonthlyAmount,
                LowStockCount = lowStockCount,
                RecentLogs = recentLogs
            };
        }
    }
}