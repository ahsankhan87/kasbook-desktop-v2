using System;
using System.Data;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;

namespace pos.Reports.Inventory
{
    public class frm_InventoryQohReport : BaseReportForm
    {
        public frm_InventoryQohReport()
        {
            Text = "Inventory - Quantity On Hand";
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var warehouse = new WarehouseReportBLL();
            int branch_id = branchId ?? UsersModal.logged_in_branch_id;
            // OperationType 1 can be QOH summary in your existing BLL; using 1 by default
            var ds = warehouse.InventoryReport(branch_id, UsersModal.logged_in_userid, null, null, null, 1);
            var dt = ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
            return dt;
        }
    }
}
