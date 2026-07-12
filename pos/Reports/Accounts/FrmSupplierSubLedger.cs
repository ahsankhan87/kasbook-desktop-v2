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
            lblTitle.Text = "Supplier Sub-Ledger (AP)";
            lblEntity.Text = "Supplier:";
            btnReceivePayment.Text = "Make Payment";
        }

        protected override void LoadEntitySelector()
        {
            try
            {
                using (BusyScope.Show(this, "Loading suppliers..."))
                {
                    DataTable dt = _supplierBll.GetAll();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow emptyRow = dt.NewRow();
                        emptyRow["id"] = 0;
                        emptyRow["name"] = "-- Select Supplier --";
                        dt.Rows.InsertAt(emptyRow, 0);

                        cmbEntity.DisplayMember = "name";
                        cmbEntity.ValueMember = "id";
                        cmbEntity.DataSource = dt;
                        cmbEntity.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Error loading suppliers", ex.Message);
            }
        }

        protected override void InitializeGrid()
        {
            base.InitializeGrid();

            // Clear default columns and add AP-specific columns
            dgvLedger.Columns.Clear();

            AddGridColumn("Date", "transaction_date", System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter, 80);
            AddGridColumn("Type", "transaction_type", System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft, 100);
            AddGridColumn("Bill", "invoice_no", System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft, 80);
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
                DataTable dtSupplier = _supplierBll.SearchRecordBySupplierID(_selectedEntityId);
                if (dtSupplier != null && dtSupplier.Rows.Count > 0)
                {
                    DataRow row = dtSupplier.Rows[0];
                    string name = row["name"]?.ToString() ?? "";
                    string phone = row["contact_number"]?.ToString() ?? "";
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
            string supplierName = "Unknown";
            if (cmbEntity.SelectedItem is DataRowView drv)
            {
                supplierName = drv["name"]?.ToString() ?? "Unknown";
            }
            return $"Supplier Sub-Ledger (AP) - {supplierName}";
        }

        protected override string GetPrintSubtitle()
        {
            return $"Period: {dtpFromDate.Value:MMM dd, yyyy} to {dtpToDate.Value:MMM dd, yyyy}";
        }

        protected override void OnReceivePaymentClick()
        {
            if (_selectedEntityId <= 0)
            {
                UiMessages.ShowWarning("Select Supplier", "Please select a supplier first");
                return;
            }

            try
            {
                // Get supplier name
                string supplierName = "";
                if (cmbEntity.SelectedItem is DataRowView drv)
                {
                    supplierName = drv["name"]?.ToString() ?? "";
                }

                // Open supplier payment form
                frm_supplier_payment paymentForm = new frm_supplier_payment(null, _selectedEntityId, supplierName);
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
