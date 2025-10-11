using System;
using System.Data;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;

namespace pos.Reports.Sales
{
    public class frm_SalesByCategoryReport : BaseReportForm
    {
        public frm_SalesByCategoryReport()
        {
            Text = "Sales by Category";
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var bll = new SalesReportBLL();
            int customer_id = 0; // All
            string product_code = string.Empty;
            string sale_type = "All";
            int employee_id = 0;
            string sale_account = "All";
            int branch_id = branchId ?? UsersModal.logged_in_branch_id;

            string f = from.ToString("yyyy-MM-dd");
            string t = to.ToString("yyyy-MM-dd");
            var dt = bll.categoryWiseSaleReport(f, t, customer_id, product_code, sale_type, employee_id, sale_account, branch_id);
            return dt;
        }
    }
}
