using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using POS.BLL;
using pos.Reports.Financial;
using pos.UI;
using pos.UI.Busy;

namespace pos.Reports.Accounts
{
    /// <summary>
    /// Cash Book form
    /// Shows all cash account transactions in two-column format (Receipts | Payments)
    /// Or single-column running balance view
    /// </summary>
    public partial class FrmCashBook : FrmSubLedgerBase
    {
        private readonly GeneralBLL _generalBll = new GeneralBLL();
        private bool _twoColumnMode = true;

        public FrmCashBook()
        {
            InitializeComponent();
            lblTitle.Text = UiMessages.T("Cash Book", "دفتر النقدية");
            lblEntity.Text = UiMessages.T("Cash Account:", "حساب النقدية:");
            btnReceivePayment.Visible = false;
            pnlAging.Visible = false;  // No aging analysis for cash book
        }

        protected override void LoadEntitySelector()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading cash accounts...", "جاري تحميل حسابات النقدية...")))
                {
                    // Load cash accounts from database
                    DataTable dt = _generalBll.GetRecord("id,name", "acc_accounts WHERE name LIKE '%Cash%'");

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow emptyRow = dt.NewRow();
                        emptyRow["id"] = 0;
                        emptyRow["name"] = UiMessages.T("-- All Cash Accounts --", "-- جميع حسابات النقدية --");
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
                UiMessages.ShowError(UiMessages.T("Error loading cash accounts", "خطأ في تحميل حسابات النقدية"), ex.Message);
            }
        }

        protected override void InitializeGrid()
        {
            dgvLedger.Columns.Clear();

            // Single column mode by default
            AddGridColumn(UiMessages.T("Date", "التاريخ"), "transaction_date", System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter, 80);
            AddGridColumn(UiMessages.T("Type", "النوع"), "transaction_type", System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft, 100);
            AddGridColumn(UiMessages.T("Reference", "المرجع"), "reference_no", System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft, 100);
            AddGridColumn(UiMessages.T("Description", "الوصف"), "description", System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft, 150);
            AddGridColumn(UiMessages.T("Receipt", "الإيصال"), "receipt_amount", System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, 80);
            AddGridColumn(UiMessages.T("Payment", "الدفع"), "payment_amount", System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, 80);
            AddGridColumn(UiMessages.T("Balance", "الرصيد"), "running_balance", System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, 100);
        }

        protected override void DisplayEntityInfo()
        {
            if (_selectedEntityId == 0)
            {
                lblEntityInfo.Text = UiMessages.T("Displaying all cash account transactions", "عرض جميع معاملات حساب النقدية");
            }
            else if (_selectedEntityId > 0)
            {
                try
                {
                    DataTable dtAccount = _generalBll.GetRecord("id,code as acc_code, name as acc_name", $"acc_accounts WHERE id = {_selectedEntityId}");
                    if (dtAccount != null && dtAccount.Rows.Count > 0)
                    {
                        DataRow row = dtAccount.Rows[0];
                        string code = row["acc_code"]?.ToString() ?? "";
                        string name = row["acc_name"]?.ToString() ?? "";
                        lblEntityInfo.Text = $"Cash Account: {code} - {name}";
                    }
                }
                catch (Exception ex)
                {
                    lblEntityInfo.Text = $"Error loading account info: {ex.Message}";
                }
            }
        }

        protected override DataTable GetLedgerData()
        {
            try
            {
                DataTable dt = _accountsBll.GetCashBook(
                    _selectedEntityId > 0 ? _selectedEntityId : (int?)null,
                    dtpFromDate.Value,
                    dtpToDate.Value
                );

                if (dt != null)
                {
                    // Add computed columns for two-column view
                    if (!dt.Columns.Contains("receipt_amount"))
                    {
                        dt.Columns.Add("receipt_amount", typeof(decimal));
                    }
                    if (!dt.Columns.Contains("payment_amount"))
                    {
                        dt.Columns.Add("payment_amount", typeof(decimal));
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        int isReceipt = Convert.ToInt32(row["is_receipt"]);
                        decimal amount = Convert.ToDecimal(row["amount"]);

                        if (isReceipt == 1)
                        {
                            row["receipt_amount"] = amount;
                            row["payment_amount"] = 0;
                        }
                        else
                        {
                            row["receipt_amount"] = 0;
                            row["payment_amount"] = amount;
                        }
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading cash book: {ex.Message}");
            }
        }

        protected override DataTable GetAgingData()
        {
            // Cash book doesn't use aging data
            return new DataTable();
        }

        protected override void UpdateAgingPanel()
        {
            // No aging analysis for cash book
        }

        protected override string GetPrintTitle()
        {
            return UiMessages.T("Cash Book Report", "تقرير دفتر النقدية");
        }

        protected override string GetPrintSubtitle()
        {
            string accountInfo = UiMessages.T("All Cash Accounts", "جميع حسابات النقدية");
            if (_selectedEntityId > 0)
            {
                try
                {
                    DataTable dtAccount = _generalBll.GetRecord("name as acc_name", $"acc_accounts WHERE id = {_selectedEntityId}");
                    if (dtAccount != null && dtAccount.Rows.Count > 0)
                    {
                        accountInfo = dtAccount.Rows[0]["acc_name"]?.ToString() ?? "Unknown";
                    }
                }
                catch { }
            }
            return $"{accountInfo} | {UiMessages.T("Period", "الفترة")}: {dtpFromDate.Value:MMM dd, yyyy} {UiMessages.T("to", "إلى")} {dtpToDate.Value:MMM dd, yyyy}";
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
