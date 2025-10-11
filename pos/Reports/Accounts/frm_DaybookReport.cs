using System;
using System.Data;
using POS.BLL;
using pos.Reports.Common;

namespace pos.Reports.Accounts
{
    public class frm_DaybookReport : BaseReportForm
    {
        public frm_DaybookReport() { Text = "Cashbook / Daybook"; }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var bll = new AccountsBLL();
            // Re-use GroupReport or AccountReport depending on your schema; this is a placeholder until specific BLL exists
            var dt = bll.GroupReport(from, to, 0);
            return dt;
        }
    }
}
