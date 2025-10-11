using System;
using System.Data;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;

namespace pos.Reports.Sales
{
    public class frm_ReturnsRefundsReport : BaseReportForm
    {
        public frm_ReturnsRefundsReport()
        {
            Text = "Sales Returns / Refunds";
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            // Use SalesReportBLL.SaleReport with a filter if returns are marked in sale_type; otherwise, query via SalesBLL GetReturnSales and compose
            var bll = new SalesBLL();
            // Placeholder: if you have a returns table, query by date range via DLL. For now, return return sales items for one invoice is supported; we need DLL support for range.
            return new DataTable();
        }
    }
}
