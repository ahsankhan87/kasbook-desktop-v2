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
            lblTitle.Text = "Customer Sub-Ledger (AR)";
            lblEntity.Text = "Customer:";
            btnReceivePayment.Text = "Receive Payment";
        }

        protected override void LoadEntitySelector()
        {
            try
            {
                using (BusyScope.Show(this, "Loading customers..."))
                {
                    DataTable dt = _customerBll.GetAll();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow emptyRow = dt.NewRow();
                        emptyRow["id"] = 0;
                        emptyRow["first_name"] = "-- Select Customer --";
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
                UiMessages.ShowError("Error loading customers", ex.Message);
            }
        }

        protected override void InitializeGrid()
        {
            base.InitializeGrid();

            // Clear default columns and add AR-specific columns
            dgvLedger.Columns.Clear();

            AddGridColumn("Date", "transaction_date", System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter, 80);
            AddGridColumn("Type", "transaction_type", System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft, 100);
            AddGridColumn("Invoice", "invoice_no", System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft, 80);
            AddGridColumn("Reference", "reference_no", System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft, 100);
            AddGridColumn("Amount", "amount", System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, 80);
            AddGridColumn("Running Balance", "running_balance", System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, 100);
            AddGridColumn("Due Date", "due_date", System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter, 80);
            AddGridColumn("Days Overdue", "days_overdue", System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter, 80);
            AddGridColumn("Status", "invoice_status", System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter, 80);
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
            string customerName = "Unknown";
            if (cmbEntity.SelectedItem is DataRowView drv)
            {
                customerName = drv["first_name"]?.ToString() ?? "Unknown";
            }
            return $"Customer Sub-Ledger (AR) - {customerName}";
        }

        protected override string GetPrintSubtitle()
        {
            return $"Period: {dtpFromDate.Value:MMM dd, yyyy} to {dtpToDate.Value:MMM dd, yyyy}";
        }

        protected override void OnReceivePaymentClick()
        {
            if (_selectedEntityId <= 0)
            {
                UiMessages.ShowWarning("Select Customer", "Please select a customer first");
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
                UiMessages.ShowError("Error", ex.Message);
            }
        }
    }
}
