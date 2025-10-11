using System;
using System.Data;
using pos.Reports.Common;

namespace pos.Reports.Accounts
{
    public class frm_DuePaymentsReport : BaseReportForm
    {
        public frm_DuePaymentsReport() { Text = "Due Payments Schedule"; }

        protected override DataTable GetData(System.DateTime from, System.DateTime to, int? branchId)
        {
            var dt = new DataTable();
            dt.Columns.Add("Supplier");
            dt.Columns.Add("BillNo");
            dt.Columns.Add("BillDate", typeof(System.DateTime));
            dt.Columns.Add("DueDate", typeof(System.DateTime));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("DaysToDue", typeof(int));
            // TODO: Fill from BLL when available
            return dt;
        }
    }
}
