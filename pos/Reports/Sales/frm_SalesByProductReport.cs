using System;
using System.Data;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;

namespace pos.Reports.Sales
{
    public class frm_SalesByProductReport : BaseReportForm
    {
        public frm_SalesByProductReport()
        {
            Text = "Sales by Product";
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var bll = new SalesReportBLL();
            int customer_id = 0;
            string product_code = string.Empty; // All
            string sale_type = "All";
            int employee_id = 0;
            string sale_account = "All";
            int branch_id = branchId ?? UsersModal.logged_in_branch_id;

            string f = from.ToString("yyyy-MM-dd");
            string t = to.ToString("yyyy-MM-dd");
            var dt = bll.ProductWiseSaleReport(f, t, customer_id, product_code, sale_type, employee_id, sale_account, branch_id);
            return dt;
        }
    }
}
