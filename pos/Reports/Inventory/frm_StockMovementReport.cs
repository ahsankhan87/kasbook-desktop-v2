using System;
using System.Data;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;

namespace pos.Reports.Inventory
{
    public class frm_StockMovementReport : BaseReportForm
    {
        public frm_StockMovementReport()
        {
            Text = "Stock Movement";
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            // Stock movement can be fetched from inventory ledger (pos_inventory); using GeneralBLL or a dedicated report BLL; placeholder returns empty
            return new DataTable();
        }
    }
}
