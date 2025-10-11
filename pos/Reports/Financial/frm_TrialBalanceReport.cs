using System;
using System.Data;
using POS.BLL;
using pos.Reports.Common;

namespace pos.Reports.Financial
{
    public class frm_TrialBalanceReport : BaseReportForm
    {
        public frm_TrialBalanceReport() { Text = "Trial Balance"; }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var bll = new AccountsBLL();
            return bll.TrialBalanceReport(from, to);
        }
    }
}
