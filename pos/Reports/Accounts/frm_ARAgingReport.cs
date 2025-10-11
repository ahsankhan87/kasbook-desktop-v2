using System;
using System.Data;
using System.Linq;
using pos.Reports.Common;

namespace pos.Reports.Accounts
{
    public class frm_ARAgingReport : BaseReportForm
    {
        public frm_ARAgingReport() { Text = "AR Aging"; }

        protected override DataTable GetData(System.DateTime from, System.DateTime to, int? branchId)
        {
            // Placeholder: build aging buckets from a ledger DataTable if available
            var dt = new DataTable();
            dt.Columns.Add("Customer");
            dt.Columns.Add("0-30", typeof(decimal));
            dt.Columns.Add("31-60", typeof(decimal));
            dt.Columns.Add("61-90", typeof(decimal));
            dt.Columns.Add("90+", typeof(decimal));
            // TODO: replace with real BLL result
            return dt;
        }
    }
}
