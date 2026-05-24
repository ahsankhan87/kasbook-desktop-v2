using POS.DLL;
using System;
using System.Data;

namespace POS.BLL
{
    public class SalesDashboardBLL
    {
        private readonly SalesDashboardDLL _dll = new SalesDashboardDLL();

        public DataTable GetSalesReps() => _dll.GetSalesReps();

        public DataTable GetKpis(DateTime fromDate, DateTime toDate, DateTime prevFromDate, DateTime prevToDate, int? employeeId = null)
            => _dll.GetKpis(fromDate, toDate, prevFromDate, prevToDate, employeeId);

        public DataTable GetMonthlySales(int year, int? employeeId = null)
            => _dll.GetMonthlySales(year, employeeId);

        public DataTable GetCategorySplit(DateTime fromDate, DateTime toDate, int top = 5, int? employeeId = null)
            => _dll.GetCategorySplit(fromDate, toDate, top, employeeId);

        public DataTable GetTopCustomers(DateTime fromDate, DateTime toDate, int top = 10, int? employeeId = null)
            => _dll.GetTopCustomers(fromDate, toDate, top, employeeId);

        public DataTable GetRecentInvoices(DateTime fromDate, DateTime toDate, int top = 8, int? employeeId = null, int? monthNo = null, string categoryName = null, int? customerId = null)
            => _dll.GetRecentInvoices(fromDate, toDate, top, employeeId, monthNo, categoryName, customerId);

        public DataTable GetCategoryTopInvoices(DateTime fromDate, DateTime toDate, string categoryName, int top = 10, int? employeeId = null)
            => _dll.GetRecentInvoices(fromDate, toDate, top, employeeId, null, categoryName, null);

        public DataTable GetCustomerSummary(int customerId, DateTime fromDate, DateTime toDate, int? employeeId = null)
            => _dll.GetCustomerSummary(customerId, fromDate, toDate, employeeId);

        public DataTable GetCustomerMonthlyTrend(int customerId, int year, int? employeeId = null)
            => _dll.GetCustomerMonthlyTrend(customerId, year, employeeId);

        public DataTable GetReceivables(DateTime fromDate, DateTime toDate, bool overdueOnly = false, int? employeeId = null)
            => _dll.GetReceivables(fromDate, toDate, overdueOnly, employeeId);
    }
}
