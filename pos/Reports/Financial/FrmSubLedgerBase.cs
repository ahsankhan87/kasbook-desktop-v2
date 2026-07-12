using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using POS.BLL;
using pos.UI;
using pos.UI.Busy;
using DGVPrinterHelper;

namespace pos.Reports.Financial
{
    /// <summary>
    /// Base class for sub-ledger forms (Customer, Supplier, Cash Book)
    /// Provides common layout, data loading, and reporting functionality
    /// </summary>
    public partial class FrmSubLedgerBase : Form
    {
        protected readonly AccountsBLL _accountsBll = new AccountsBLL();
        protected DataTable _ledgerData;
        protected DataTable _agingData;
        protected int _selectedEntityId = 0;

        public FrmSubLedgerBase()
        {
            InitializeComponent();
            WireEvents();
        }

        private void WireEvents()
        {
            Load += (s, e) => OnFormLoad();
            btnLoad.Click += (s, e) => LoadLedger();
            btnPrint.Click += (s, e) => PrintLedger();
            btnExport.Click += (s, e) => ExportLedger();
            btnReceivePayment.Click += (s, e) => OnReceivePaymentClick();
            cmbEntity.SelectedIndexChanged += (s, e) => OnEntityChanged();
        }

        protected virtual void OnFormLoad()
        {
            try
            {
                using (BusyScope.Show(this, "Initializing..."))
                {
                    AppTheme.Apply(this);
                    InitializeGrid();
                    LoadEntitySelector();
                    SetDateDefaults();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Error loading form", ex.Message);
            }
        }

        /// <summary>
        /// Override to load entity-specific dropdown (customers, suppliers, or accounts)
        /// </summary>
        protected virtual void LoadEntitySelector()
        {
            // To be overridden by derived classes
        }

        /// <summary>
        /// Override to display entity information card
        /// </summary>
        protected virtual void DisplayEntityInfo()
        {
            lblEntityInfo.Text = "Select an entity to display information";
        }

        /// <summary>
        /// Setup date pickers to default range
        /// </summary>
        protected virtual void SetDateDefaults()
        {
            dtpToDate.Value = DateTime.Today;
            dtpFromDate.Value = DateTime.Today.AddMonths(-1);
        }

        /// <summary>
        /// Initialize the ledger grid columns
        /// </summary>
        protected virtual void InitializeGrid()
        {
            dgvLedger.AllowUserToAddRows = false;
            dgvLedger.AllowUserToDeleteRows = false;
            dgvLedger.ReadOnly = true;
            dgvLedger.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLedger.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;

            AddGridColumn("Date", "transaction_date", DataGridViewContentAlignment.MiddleCenter, 80);
            AddGridColumn("Type", "transaction_type", DataGridViewContentAlignment.MiddleLeft, 100);
            AddGridColumn("Reference", "reference_no", DataGridViewContentAlignment.MiddleLeft, 100);
            AddGridColumn("Description", "description", DataGridViewContentAlignment.MiddleLeft, 150);
            AddGridColumn("Amount", "amount", DataGridViewContentAlignment.MiddleRight, 80);
            AddGridColumn("Balance", "running_balance", DataGridViewContentAlignment.MiddleRight, 100);
            AddGridColumn("Status", "invoice_status", DataGridViewContentAlignment.MiddleCenter, 80);
        }

        /// <summary>
        /// Add a column to the grid
        /// </summary>
        protected void AddGridColumn(string headerText, string dataPropertyName, DataGridViewContentAlignment alignment, int width)
        {
            DataGridViewColumn col = new DataGridViewTextBoxColumn
            {
                HeaderText = headerText,
                DataPropertyName = dataPropertyName,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = alignment },
                Width = width
            };
            dgvLedger.Columns.Add(col);
        }

        /// <summary>
        /// Called when entity selection changes
        /// </summary>
        protected virtual void OnEntityChanged()
        {
            if (cmbEntity.SelectedValue != null && int.TryParse(cmbEntity.SelectedValue.ToString(), out int entityId))
            {
                _selectedEntityId = entityId;
                DisplayEntityInfo();
            }
        }

        /// <summary>
        /// Load ledger data - override in derived classes
        /// </summary>
        protected virtual void LoadLedger()
        {
            if (_selectedEntityId == 0)
            {
                UiMessages.ShowWarning("Please select an entity", "Please select an entity to view ledger");
                return;
            }

            try
            {
                using (BusyScope.Show(this, "Loading ledger data..."))
                {
                    _ledgerData = GetLedgerData();
                    _agingData = GetAgingData();

                    if (_ledgerData != null && _ledgerData.Rows.Count > 0)
                    {
                        dgvLedger.DataSource = _ledgerData;
                        UpdateAgingPanel();
                    }
                    else
                    {
                        UiMessages.ShowInfo("No data", "No transactions found for the selected period");
                        dgvLedger.DataSource = null;
                        ResetAgingPanel();
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Error loading ledger", ex.Message);
            }
        }

        /// <summary>
        /// Get ledger data from database - override in derived classes
        /// </summary>
        protected virtual DataTable GetLedgerData()
        {
            return new DataTable();
        }

        /// <summary>
        /// Get aging data from database - override in derived classes
        /// </summary>
        protected virtual DataTable GetAgingData()
        {
            return new DataTable();
        }

        /// <summary>
        /// Update aging analysis panel
        /// </summary>
        protected virtual void UpdateAgingPanel()
        {
            if (_agingData == null || _agingData.Rows.Count == 0)
            {
                ResetAgingPanel();
                return;
            }

            DataRow row = _agingData.Rows[0];
            decimal bucket0_30 = row["bucket_0_30"] != DBNull.Value ? Convert.ToDecimal(row["bucket_0_30"]) : 0;
            decimal bucket31_60 = row["bucket_31_60"] != DBNull.Value ? Convert.ToDecimal(row["bucket_31_60"]) : 0;
            decimal bucket61_90 = row["bucket_61_90"] != DBNull.Value ? Convert.ToDecimal(row["bucket_61_90"]) : 0;
            decimal bucket90Plus = row["bucket_90_plus"] != DBNull.Value ? Convert.ToDecimal(row["bucket_90_plus"]) : 0;

            lbl0_30.Text = $"0-30: {bucket0_30:N2}";
            lbl31_60.Text = $"31-60: {bucket31_60:N2}";
            lbl61_90.Text = $"61-90: {bucket61_90:N2}";
            lbl90Plus.Text = $"90+: {bucket90Plus:N2}";
        }

        /// <summary>
        /// Reset aging analysis panel
        /// </summary>
        protected virtual void ResetAgingPanel()
        {
            lbl0_30.Text = "0-30: 0";
            lbl31_60.Text = "31-60: 0";
            lbl61_90.Text = "61-90: 0";
            lbl90Plus.Text = "90+: 0";
        }

        /// <summary>
        /// Print ledger
        /// </summary>
        protected virtual void PrintLedger()
        {
            if (dgvLedger.Rows.Count == 0)
            {
                UiMessages.ShowWarning("No data to print", "Please load data first");
                return;
            }

            try
            {
                DGVPrinter printer = new DGVPrinter();
                printer.Title = GetPrintTitle();
                printer.SubTitle = GetPrintSubtitle();
                printer.PageNumbers = true;
                printer.PrintDataGridView(dgvLedger);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Print error", ex.Message);
            }
        }

        /// <summary>
        /// Get print title - override in derived classes
        /// </summary>
        protected virtual string GetPrintTitle()
        {
            return "Sub Ledger Report";
        }

        /// <summary>
        /// Get print subtitle - override in derived classes
        /// </summary>
        protected virtual string GetPrintSubtitle()
        {
            return $"From {dtpFromDate.Value:yyyy-MM-dd} To {dtpToDate.Value:yyyy-MM-dd}";
        }

        /// <summary>
        /// Export ledger to Excel
        /// </summary>
        protected virtual void ExportLedger()
        {
            if (dgvLedger.Rows.Count == 0)
            {
                UiMessages.ShowWarning("No data to export", "Please load data first");
                return;
            }

            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx|CSV files (*.csv)|*.csv",
                    FileName = $"SubLedger_{DateTime.Now:yyyyMMdd_HHmmss}"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (sfd.FileName.EndsWith(".xlsx"))
                    {
                        ExportToExcel(sfd.FileName);
                    }
                    else if (sfd.FileName.EndsWith(".csv"))
                    {
                        ExportToCsv(sfd.FileName);
                    }
                    UiMessages.ShowInfo("Success", "Data exported successfully");
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Export error", ex.Message);
            }
        }

        /// <summary>
        /// Export to Excel - override for specific implementation
        /// </summary>
        protected virtual void ExportToExcel(string filePath)
        {
            // Simple export implementation - can be enhanced
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath))
            {
                // Write headers
                for (int i = 0; i < dgvLedger.Columns.Count; i++)
                {
                    writer.Write(dgvLedger.Columns[i].HeaderText);
                    if (i < dgvLedger.Columns.Count - 1) writer.Write(",");
                }
                writer.WriteLine();

                // Write data
                foreach (DataGridViewRow row in dgvLedger.Rows)
                {
                    for (int i = 0; i < dgvLedger.Columns.Count; i++)
                    {
                        writer.Write(row.Cells[i].Value);
                        if (i < dgvLedger.Columns.Count - 1) writer.Write(",");
                    }
                    writer.WriteLine();
                }
            }
        }

        /// <summary>
        /// Export to CSV
        /// </summary>
        protected virtual void ExportToCsv(string filePath)
        {
            ExportToExcel(filePath);  // Same implementation for now
        }

        /// <summary>
        /// Handle payment button click - override in derived classes
        /// </summary>
        protected virtual void OnReceivePaymentClick()
        {
            UiMessages.ShowInfo("Not implemented", "Payment functionality to be implemented in derived class");
        }
    }
}
