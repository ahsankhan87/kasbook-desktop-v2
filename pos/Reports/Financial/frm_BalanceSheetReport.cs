using System;
using System.Data;
using pos.Reports.Common;

namespace pos.Reports.Financial
{
    public class frm_BalanceSheetReport : BaseReportForm
    {
        public frm_BalanceSheetReport() { Text = "Balance Sheet"; }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            // TODO: implement via AccountsBLL when available
            return new DataTable();
        }
    }
}
