using System;
using System.Data;
using POS.BLL;
using pos.Reports.Common;

namespace pos.Reports.Accounts
{
    public class frm_AccountReceivableAging : BaseReportForm
    {
        public frm_AccountReceivableAging() { Text = "Accounts Receivable - Aging"; }

        protected override DataTable GetData(System.DateTime from, System.DateTime to, int? branchId)
        {
            // TODO: replace with actual AR aging once available in BLL/DLL
            // Using AccountsBLL as placeholder, returning an empty table to avoid compile errors
            return new DataTable();
        }
    }
}
