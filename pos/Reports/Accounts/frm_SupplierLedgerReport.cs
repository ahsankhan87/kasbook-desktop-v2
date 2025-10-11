using System;
using System.Data;
using System.Windows.Forms;
using POS.BLL;
using pos.Reports.Common;

namespace pos.Reports.Accounts
{
    public class frm_SupplierLedgerReport : BaseReportForm
    {
        private readonly ComboBox _supplierCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220 };

        public frm_SupplierLedgerReport()
        {
            Text = "Supplier Ledger";
            Filters.Controls.Add(new Label { Text = "Supplier:", AutoSize = true });
            Filters.Controls.Add(_supplierCombo);
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            var bll = new SupplierBLL();
            var dt = bll.GetAll();
            var blank = dt.NewRow();
            blank[0] = 0; blank[1] = "All Suppliers";
            dt.Rows.InsertAt(blank, 0);
            _supplierCombo.DisplayMember = "name";
            _supplierCombo.ValueMember = "id";
            _supplierCombo.DataSource = dt;
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var accounts = new AccountsBLL();
            int supplierId = 0;
            if (_supplierCombo.SelectedValue is int) supplierId = (int)_supplierCombo.SelectedValue;
            var dt = accounts.AccountReport(from, to, supplierId);
            return dt;
        }
    }
}
