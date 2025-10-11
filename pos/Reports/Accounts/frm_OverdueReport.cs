using System;
using System.Data;
using pos.Reports.Common;

namespace pos.Reports.Accounts
{
    public class frm_OverdueReport : BaseReportForm
    {
        public frm_OverdueReport() { Text = "Overdue Customers"; }

        protected override DataTable GetData(System.DateTime from, System.DateTime to, int? branchId)
        {
            var dt = new DataTable();
            dt.Columns.Add("Customer");
            dt.Columns.Add("OverdueAmount", typeof(decimal));
            dt.Columns.Add("MaxDaysOverdue", typeof(int));
            dt.Columns.Add("Phone");
            dt.Columns.Add("Email");
            // TODO: Fill from BLL/DLL when available
            return dt;
        }
    }
}
