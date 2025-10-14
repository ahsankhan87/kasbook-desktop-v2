using System;
using System.Data;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;

namespace pos.Reports.Inventory
{
    public class frm_LowStockReport : BaseReportForm
    {
        public frm_LowStockReport()
        {
            Text = "Inventory - Low Stock";
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var warehouse = new WarehouseReportBLL();
            int branch_id = branchId ?? UsersModal.logged_in_branch_id;
            // OperationType 2 was used for LowStock in existing code
            var ds = warehouse.InventoryReport(branch_id, UsersModal.logged_in_userid, null, null, null, 2);
            var dt = ds != null && ds.Tables.Count > 0 ? ds.Tables["StockReport"] : new DataTable();
            return dt;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frm_LowStockReport
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "frm_LowStockReport";
            this.Load += new System.EventHandler(this.frm_LowStockReport_Load);
            this.ResumeLayout(false);

        }

        private void frm_LowStockReport_Load(object sender, EventArgs e)
        {

        }
    }
}
