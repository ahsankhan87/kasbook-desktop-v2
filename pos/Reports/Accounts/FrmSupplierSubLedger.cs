using System;
using System.Data;
using System.Windows.Forms;
using POS.BLL;
using pos.Reports.Financial;
using pos.UI;
using pos.UI.Busy;

namespace pos.Reports.Accounts
{
    /// <summary>
    /// Supplier Sub-Ledger (Accounts Payable) form
    /// Shows all bills and payments for a supplier with aging analysis
    /// </summary>
    public partial class FrmSupplierSubLedger : FrmSubLedgerBase
    {
        private readonly SupplierBLL _supplierBll = new SupplierBLL();

        public FrmSupplierSubLedger()
        {
            InitializeComponent();
            lblTitle.Text = UiMessages.T("Supplier Sub-Ledger (AP)", "دفتر الموردين الفرعي (الذمم الدائنة)");
            lblEntity.Text = UiMessages.T("Supplier:", "المورد:");
            btnReceivePayment.Text = UiMessages.T("Make Payment", "تسجيل الدفع");
        }

        protected override void LoadEntitySelector()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading suppliers...", "جاري تحميل الموردين...")))
                {
                    DataTable dt = _supplierBll.GetAll();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow emptyRow = dt.NewRow();
                        emptyRow["id"] = 0;
                        emptyRow["first_name"] = UiMessages.T("-- Select Supplier --", "-- اختر مورد --");
                        dt.Rows.InsertAt(emptyRow, 0);

                        cmbEntity.DisplayMember = "first_name";
                        cmbEntity.ValueMember = "id";
                        cmbEntity.DataSource = dt;
                        cmbEntity.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(UiMessages.T("Error loading suppliers", "خطأ في تحميل الموردين"), ex.Message);
            }
        }

        protected override void InitializeGrid()
        {
            dgvLedger.Columns.Clear();
            dgvLedger.AllowUserToAddRows = false;
            dgvLedger.AllowUserToDeleteRows = false;
            dgvLedger.ReadOnly = false;  // Enable editing
            dgvLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;

            // Clear default columns and add AP-specific columns
            AddGridColumn(UiMessages.T("Date", "التاريخ"), "transaction_date", System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter, 80);
            AddGridColumn(UiMessages.T("Type", "النوع"), "transaction_type", System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft, 100);
            AddGridColumn(UiMessages.T("Bill", "الفاتورة"), "invoice_no", System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft, 80);
            AddGridColumn(UiMessages.T("Reference", "المرجع"), "reference_no", System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft, 100);
            AddGridColumn(UiMessages.T("Amount", "المبلغ"), "amount", System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, 80);
            AddGridColumn(UiMessages.T("Running Balance", "الرصيد المتراكم"), "running_balance", System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, 100);
            AddGridColumn(UiMessages.T("Due Date", "تاريخ الاستحقاق"), "due_date", System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter, 80);
            AddGridColumn(UiMessages.T("Days Overdue", "أيام التأخير"), "days_overdue", System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter, 80);
            AddGridColumn(UiMessages.T("Status", "الحالة"), "invoice_status", System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter, 80);
        }

        protected override void DisplayEntityInfo()
        {
            if (_selectedEntityId <= 0) return;

            try
            {
                DataTable dtSupplier = _supplierBll.SearchRecordBySupplierID(_selectedEntityId);
                if (dtSupplier != null && dtSupplier.Rows.Count > 0)
                {
                    DataRow row = dtSupplier.Rows[0];
                    string name = row["first_name"]?.ToString() ?? "";
                    string phone = row["contact_no"]?.ToString() ?? "";
                    string email = row["email"]?.ToString() ?? "";
                    string address = row["address"]?.ToString() ?? "";

                    lblEntityInfo.Text = $"{name} | Phone: {phone} | Email: {email} | Address: {address}";
                }
            }
            catch (Exception ex)
            {
                lblEntityInfo.Text = $"Error loading supplier info: {ex.Message}";
            }
        }

        protected override DataTable GetLedgerData()
        {
            try
            {
                return _accountsBll.GetSupplierSubLedger(
                    _selectedEntityId,
                    dtpFromDate.Value,
                    dtpToDate.Value
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading supplier sub-ledger: {ex.Message}");
            }
        }

        protected override DataTable GetAgingData()
        {
            try
            {
                return _accountsBll.GetSupplierSubLedgerAging(
                    _selectedEntityId,
                    dtpFromDate.Value,
                    dtpToDate.Value
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading aging data: {ex.Message}");
            }
        }

        protected override string GetPrintTitle()
        {
            string supplierName = UiMessages.T("Unknown", "غير معروف");
            if (cmbEntity.SelectedItem is DataRowView drv)
            {
                supplierName = drv["first_name"]?.ToString() ?? UiMessages.T("Unknown", "غير معروف");
            }
            return $"{UiMessages.T("Supplier Sub-Ledger (AP)", "دفتر الموردين الفرعي (الذمم الدائنة)")} - {supplierName}";
        }

        protected override string GetPrintSubtitle()
        {
            return $"{UiMessages.T("Period", "الفترة")}: {dtpFromDate.Value:MMM dd, yyyy} {UiMessages.T("to", "إلى")} {dtpToDate.Value:MMM dd, yyyy}";
        }

        protected override void OnReceivePaymentClick()
        {
            if (_selectedEntityId <= 0)
            {
                UiMessages.ShowWarning(UiMessages.T("Select Supplier", "اختر مورد"), UiMessages.T("Please select a supplier first", "يرجى اختيار مورد أولاً"));
                return;
            }

            try
            {
                // Get supplier name
                string supplierName = "";
                if (cmbEntity.SelectedItem is DataRowView drv)
                {
                    supplierName = drv["first_name"]?.ToString() ?? "";
                }

                // Open supplier payment form
                frm_supplier_payment paymentForm = new frm_supplier_payment(null, _selectedEntityId, supplierName);
                paymentForm.ShowDialog();

                // Reload ledger after payment
                LoadLedger();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(UiMessages.T("Error", "خطأ"), ex.Message);
            }
        }
    }
}
