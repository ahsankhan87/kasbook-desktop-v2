using System;
using System.Data;
using POS.BLL;
using pos.Reports.Common;

namespace pos.Reports.Inventory
{
    public class frm_InventoryValuationReport : BaseReportForm
    {
        public frm_InventoryValuationReport()
        {
            Text = "Inventory Valuation (As-of)";
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            // As-of valuation typically requires snapshot or ledger calculation; placeholder pulls current stock
            var warehouse = new WarehouseReportBLL();
            var ds = warehouse.InventoryReport(POS.Core.UsersModal.logged_in_branch_id, POS.Core.UsersModal.logged_in_userid, null, null, null, 1);
            return ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
        }
    }
}
