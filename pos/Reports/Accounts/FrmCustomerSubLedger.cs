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
    /// Customer Sub-Ledger (Accounts Receivable) form
    /// Shows all invoices and payments for a customer with aging analysis
    /// </summary>
    public partial class FrmCustomerSubLedger : FrmSubLedgerBase
    {
        private readonly CustomerBLL _customerBll = new CustomerBLL();

        public FrmCustomerSubLedger()
        {
            InitializeComponent();
            lblTitle.Text = UiMessages.T("Customer Sub-Ledger (AR)", "دفتر العملاء الفرعي (الذمم المدينة)");
            lblEntity.Text = UiMessages.T("Customer:", "العميل:");
            btnReceivePayment.Text = UiMessages.T("Receive Payment", "استلام الدفع");
        }

        protected override void LoadEntitySelector()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading customers...", "جاري تحميل العملاء...")))
                {
                    DataTable dt = _customerBll.GetAll();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow emptyRow = dt.NewRow();
                        emptyRow["id"] = 0;
                        emptyRow["first_name"] = UiMessages.T("-- Select Customer --", "-- اختر عميل --");
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
                UiMessages.ShowError(UiMessages.T("Error loading customers", "خطأ في تحميل العملاء"), ex.Message);
            }
        }

        protected override void InitializeGrid()
        {
            dgvLedger.Columns.Clear();
            dgvLedger.AllowUserToAddRows = false;
            dgvLedger.AllowUserToDeleteRows = false;
            dgvLedger.ReadOnly = false;  // Enable editing
            dgvLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;

            // Clear default columns and add AR-specific columns
            AddGridColumn(UiMessages.T("Date", "التاريخ"), "transaction_date", System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter, 80);
            AddGridColumn(UiMessages.T("Type", "النوع"), "transaction_type", System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft, 100);
            AddGridColumn(UiMessages.T("Invoice", "الفاتورة"), "invoice_no", System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft, 80);
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
                DataTable dtCustomer = _customerBll.SearchRecordByCustomerID(_selectedEntityId);
                if (dtCustomer != null && dtCustomer.Rows.Count > 0)
                {
                    DataRow row = dtCustomer.Rows[0];
                    string name = row["first_name"]?.ToString() ?? "";
                    string phone = row["contact_no"]?.ToString() ?? "";
                    string email = row["email"]?.ToString() ?? "";
                    string address = row["address"]?.ToString() ?? "";

                    lblEntityInfo.Text = $"{name} | Phone: {phone} | Email: {email} | Address: {address}";
                }
            }
            catch (Exception ex)
            {
                lblEntityInfo.Text = $"Error loading customer info: {ex.Message}";
            }
        }

        protected override DataTable GetLedgerData()
        {
            try
            {
                return _accountsBll.GetCustomerSubLedger(
                    _selectedEntityId,
                    dtpFromDate.Value,
                    dtpToDate.Value
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading customer sub-ledger: {ex.Message}");
            }
        }

        protected override DataTable GetAgingData()
        {
            try
            {
                return _accountsBll.GetCustomerSubLedgerAging(
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
            string customerName = UiMessages.T("Unknown", "غير معروف");
            if (cmbEntity.SelectedItem is DataRowView drv)
            {
                customerName = drv["first_name"]?.ToString() ?? UiMessages.T("Unknown", "غير معروف");
            }
            return $"{UiMessages.T("Customer Sub-Ledger (AR)", "دفتر العملاء الفرعي (الذمم المدينة)")} - {customerName}";
        }

        protected override string GetPrintSubtitle()
        {
            return $"{UiMessages.T("Period", "الفترة")}: {dtpFromDate.Value:MMM dd, yyyy} {UiMessages.T("to", "إلى")} {dtpToDate.Value:MMM dd, yyyy}";
        }

        protected override void OnReceivePaymentClick()
        {
            if (_selectedEntityId <= 0)
            {
                UiMessages.ShowWarning(UiMessages.T("Select Customer", "اختر عميل"), UiMessages.T("Please select a customer first", "يرجى اختيار عميل أولاً"));
                return;
            }

            try
            {
                // Get customer name
                string customerName = "";
                if (cmbEntity.SelectedItem is DataRowView drv)
                {
                    customerName = drv["first_name"]?.ToString() ?? "";
                }

                // Open payment form
                frm_customer_payment paymentForm = new frm_customer_payment(null, _selectedEntityId, customerName);
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
