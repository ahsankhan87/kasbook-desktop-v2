using System;
using System.Data;
using pos.Reports.Common;

namespace pos.Reports.Accounts
{
    public class frm_APAgingReport : BaseReportForm
    {
        public frm_APAgingReport() { Text = "AP Aging"; }

        protected override DataTable GetData(System.DateTime from, System.DateTime to, int? branchId)
        {
            // Placeholder buckets; wire to BLL when available
            var dt = new DataTable();
            dt.Columns.Add("Supplier");
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
            // frm_APAgingReport
            // 
            this.ClientSize = new System.Drawing.Size(1068, 551);
            this.Name = "frm_APAgingReport";
            this.ResumeLayout(false);

        }
    }
}
