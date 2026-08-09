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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frm_TrialBalanceReport
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "frm_TrialBalanceReport";
            this.Load += new System.EventHandler(this.frm_TrialBalanceReport_Load);
            this.Controls.SetChildIndex(this.Grid, 0);
            this.ResumeLayout(false);

        }

        private void frm_TrialBalanceReport_Load(object sender, EventArgs e)
        {

        }
    }
}
