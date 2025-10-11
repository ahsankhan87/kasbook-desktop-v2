using System;
using System.Data;
using POS.BLL;
using pos.Reports.Common;

namespace pos.Reports.Accounts
{
    public class frm_AccountPayableAging : BaseReportForm
    {
        public frm_AccountPayableAging() { Text = "Accounts Payable - Aging"; }

        protected override DataTable GetData(System.DateTime from, System.DateTime to, int? branchId)
        {
            // TODO: implement via AccountsBLL when available
            return new DataTable();
        }
    }
}
