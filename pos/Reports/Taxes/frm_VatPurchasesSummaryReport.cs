using System;
using System.Data;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;

namespace pos.Reports.Taxes
{
    public class frm_VatPurchasesSummaryReport : BaseReportForm
    {
        public frm_VatPurchasesSummaryReport() { Text = "VAT/GST Purchases Summary"; }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var bll = new PurchasesReportBLL();
            int branch_id = branchId ?? UsersModal.logged_in_branch_id;
            var dt = bll.PurchaseReport(from, to, 0, 0, "All", 0, branch_id);
            return dt;
        }
    }
}
