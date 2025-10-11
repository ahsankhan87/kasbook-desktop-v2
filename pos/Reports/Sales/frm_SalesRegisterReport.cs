using System;
using System.Data;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;

namespace pos.Reports.Sales
{
    public class frm_SalesRegisterReport : BaseReportForm
    {
        public frm_SalesRegisterReport()
        {
            Text = "Sales Register";
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            // Use existing SalesReportBLL to fetch data matching current filters
            var bll = new SalesReportBLL();
            int customer_id = 0; // All
            string product_code = string.Empty; // All
            string sale_type = "All"; // All types
            int employee_id = 0; // All
            string sale_account = "All"; // All
            int branch_id = branchId ?? UsersModal.logged_in_branch_id;

            var dt = bll.SaleReport(from.Date, to.Date, customer_id, product_code, sale_type, employee_id, sale_account, branch_id);
            return dt;
        }
    }
}
