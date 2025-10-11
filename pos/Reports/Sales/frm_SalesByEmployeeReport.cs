using System;
using System.Data;
using System.Windows.Forms;
using POS.BLL;
using POS.Core;
using pos.Reports.Common;

namespace pos.Reports.Sales
{
    public class frm_SalesByEmployeeReport : BaseReportForm
    {
        private readonly ComboBox _employeeCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220 };

        public frm_SalesByEmployeeReport()
        {
            Text = "Sales by Employee/Cashier";
            Filters.Controls.Add(new Label { Text = "Employee:", AutoSize = true });
            Filters.Controls.Add(_employeeCombo);
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            var bll = new EmployeeBLL();
            var dt = bll.GetAll();
            var blank = dt.NewRow();
            blank[0] = 0; blank[1] = "All Employees";
            dt.Rows.InsertAt(blank, 0);
            _employeeCombo.DisplayMember = "first_name";
            _employeeCombo.ValueMember = "id";
            _employeeCombo.DataSource = dt;
        }

        protected override DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            var bll = new SalesReportBLL();
            int employee_id = 0;
            if (_employeeCombo.SelectedValue is int) employee_id = (int)_employeeCombo.SelectedValue;
            int branch_id = branchId ?? UsersModal.logged_in_branch_id;
            var dt = bll.SaleReport(from, to, 0, string.Empty, "All", employee_id, "All", branch_id);
            return dt;
        }
    }
}
