using System;
using System.Data;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;

namespace pos.Reports.Sales
{
    public class frm_PromotionsDiscountsReport : BaseReportForm
    {
        public frm_PromotionsDiscountsReport()
        {
            Text = "Promotions / Discounts";
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var bll = new SalesReportBLL();
            int branch_id = branchId ?? UsersModal.logged_in_branch_id;
            // If discount info is in SaleReport, we can aggregate here; otherwise, add a dedicated BLL
            var dt = bll.SaleReport(from, to, 0, string.Empty, "All", 0, "All", branch_id);
            return dt;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frm_PromotionsDiscountsReport
            // 
            this.ClientSize = new System.Drawing.Size(1050, 496);
            this.Name = "frm_PromotionsDiscountsReport";
            this.ResumeLayout(false);

        }
    }
}
