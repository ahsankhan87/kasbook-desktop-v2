using System;
using System.Data;
using pos.Reports.Common;

namespace pos.Reports.Financial
{
    public class frm_ProfitAndLossReport : BaseReportForm
    {
        public frm_ProfitAndLossReport() { Text = "Profit & Loss"; }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            // TODO: implement via AccountsBLL when available (e.g., GroupReport for P&L groups)
            return new DataTable();
        }
    }
}
