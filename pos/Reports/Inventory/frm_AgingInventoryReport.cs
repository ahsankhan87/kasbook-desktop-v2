using System;
using System.Data;
using pos.Reports.Common;

namespace pos.Reports.Inventory
{
    public class frm_AgingInventoryReport : BaseReportForm
    {
        public frm_AgingInventoryReport()
        {
            Text = "Aging Inventory";
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            // Placeholder; requires inventory receipt date and current QOH to bucket
            var dt = new DataTable();
            dt.Columns.Add("Item");
            dt.Columns.Add("0-30", typeof(decimal));
            dt.Columns.Add("31-60", typeof(decimal));
            dt.Columns.Add("61-90", typeof(decimal));
            dt.Columns.Add("90+", typeof(decimal));
            return dt;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frm_AgingInventoryReport
            // 
            this.ClientSize = new System.Drawing.Size(993, 399);
            this.Name = "frm_AgingInventoryReport";
            this.ResumeLayout(false);

        }
    }
}
