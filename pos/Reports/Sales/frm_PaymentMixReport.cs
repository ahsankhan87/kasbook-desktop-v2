using System;
using System.Data;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;

namespace pos.Reports.Sales
{
    public class frm_PaymentMixReport : BaseReportForm
    {
        public frm_PaymentMixReport()
        {
            Text = "Payment Mix";
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            // If you have a dedicated BLL for payment mix, use it. Otherwise, use sales report and aggregate client-side.
            var bll = new SalesReportBLL();
            int customer_id = 0;
            string product_code = string.Empty;
            string sale_type = "All";
            int employee_id = 0;
            string sale_account = "All";
            int branch_id = branchId ?? UsersModal.logged_in_branch_id;

            // Re-use sales details and then group by payment method if the dataset provides it
            var dtRaw = bll.SaleReport(from, to, customer_id, product_code, sale_type, employee_id, sale_account, branch_id);
            var dt = dtRaw.Clone();
            // TODO: replace with proper grouping by payment method when available in DLL/BLL
            return dtRaw; // for now show raw sales; later add grouping
        }
    }
}
