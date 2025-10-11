using System;
using System.Data;
using System.Windows.Forms;
using POS.BLL;
using pos.Reports.Common;

namespace pos.Reports.Accounts
{
    public class frm_CustomerLedgerReport : BaseReportForm
    {
        private readonly ComboBox _customerCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220 };

        public frm_CustomerLedgerReport()
        {
            Text = "Customer Ledger";
            Filters.Controls.Add(new Label { Text = "Customer:", AutoSize = true });
            Filters.Controls.Add(_customerCombo);
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            var bll = new CustomerBLL();
            var dt = bll.GetAll();
            var blank = dt.NewRow();
            blank[0] = 0; blank[1] = "All Customers";
            dt.Rows.InsertAt(blank, 0);
            _customerCombo.DisplayMember = "name";
            _customerCombo.ValueMember = "id";
            _customerCombo.DataSource = dt;
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var accounts = new AccountsBLL();
            int customerId = 0;
            if (_customerCombo.SelectedValue is int) customerId = (int)_customerCombo.SelectedValue;
            var dt = accounts.AccountReport(from, to, customerId);
            return dt;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frm_CustomerLedgerReport
            // 
            this.ClientSize = new System.Drawing.Size(986, 492);
            this.Name = "frm_CustomerLedgerReport";
            this.Load += new System.EventHandler(this.frm_CustomerLedgerReport_Load);
            this.ResumeLayout(false);

        }

        private void frm_CustomerLedgerReport_Load(object sender, EventArgs e)
        {

        }
    }
}
