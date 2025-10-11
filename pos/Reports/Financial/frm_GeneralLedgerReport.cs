using System;
using System.Data;
using POS.BLL;
using pos.Reports.Common;

namespace pos.Reports.Financial
{
    public class frm_GeneralLedgerReport : BaseReportForm
    {
        public frm_GeneralLedgerReport() { Text = "General Ledger"; }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var bll = new AccountsBLL();
            // For GL, we might need an account picker; for now, full range via GroupReport (placeholder)
            return bll.GroupReport(from, to, 0);
        }
    }
}
