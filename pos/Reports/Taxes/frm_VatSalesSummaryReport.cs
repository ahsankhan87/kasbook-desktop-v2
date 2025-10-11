using System;
using System.Data;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;

namespace pos.Reports.Taxes
{
    public class frm_VatSalesSummaryReport : BaseReportForm
    {
        public frm_VatSalesSummaryReport() { Text = "VAT/GST Sales Summary"; }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var bll = new SalesReportBLL();
            int branch_id = branchId ?? UsersModal.logged_in_branch_id;
            var dt = bll.SaleReport(from, to, 0, string.Empty, "All", 0, "All", branch_id);
            // TODO: group by tax rate; for now show raw with tax column
            return dt;
        }
    }
}
