using System;
using System.Data;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;

namespace pos.Reports.Purchases
{
    public class frm_PurchaseRegisterReport : BaseReportForm
    {
        public frm_PurchaseRegisterReport()
        {
            Text = "Purchase Register";
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var bll = new PurchasesReportBLL();
            int supplier_id = 0; // All
            int product_id = 0; // All
            string purchase_type = "All"; // All types
            int employee_id = 0; // All
            int branch_id = branchId ?? UsersModal.logged_in_branch_id;

            var dt = bll.PurchaseReport(from.Date, to.Date, supplier_id, product_id, purchase_type, employee_id, branch_id);
            return dt;
        }
    }
}
