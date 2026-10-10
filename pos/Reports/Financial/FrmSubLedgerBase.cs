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
            lblEntityInfo.Text = UiMessages.T("Select an entity to display information", "اختر كيان لعرض المعلومات");
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

            AddGridColumn(UiMessages.T("Date", "التاريخ"), "transaction_date", DataGridViewContentAlignment.MiddleCenter, 80);
            AddGridColumn(UiMessages.T("Type", "النوع"), "transaction_type", DataGridViewContentAlignment.MiddleLeft, 100);
            AddGridColumn(UiMessages.T("Reference", "المرجع"), "reference_no", DataGridViewContentAlignment.MiddleLeft, 100);
            AddGridColumn(UiMessages.T("Description", "الوصف"), "description", DataGridViewContentAlignment.MiddleLeft, 150);
            AddGridColumn(UiMessages.T("Amount", "المبلغ"), "amount", DataGridViewContentAlignment.MiddleRight, 80);
            AddGridColumn(UiMessages.T("Balance", "الرصيد"), "running_balance", DataGridViewContentAlignment.MiddleRight, 100);
            AddGridColumn(UiMessages.T("Status", "الحالة"), "invoice_status", DataGridViewContentAlignment.MiddleCenter, 80);
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
                UiMessages.ShowWarning(
                    UiMessages.T("Please select an entity", "يرجى اختيار كيان"), 
                    UiMessages.T("Please select an entity to view ledger", "يرجى اختيار كيان لعرض دفتر الأستاذ"));
                return;
            }

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading ledger data...", "جاري تحميل البيانات...")))
                {
                    _ledgerData = GetLedgerData();
                    _agingData = GetAgingData();

                    if (_ledgerData != null && _ledgerData.Rows.Count > 0)
                    {
                        // Map entity name to Arabic if in ar-SA mode
                        ApplyEntityNameMapping(_ledgerData);

                        dgvLedger.DataSource = _ledgerData;
                        UpdateAgingPanel();
                    }
                    else
                    {
                        UiMessages.ShowInfo(
                            UiMessages.T("No data", "لا توجد بيانات"), 
                            UiMessages.T("No transactions found for the selected period", "لم يتم العثور على معاملات للفترة المختارة"));
                        dgvLedger.DataSource = null;
                        ResetAgingPanel();
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    UiMessages.T("Error loading ledger", "خطأ في تحميل دفتر الأستاذ"), 
                    ex.Message);
            }
        }

        /// <summary>
        /// Apply entity name mapping (name_2) for ar-SA layout - override in derived classes if needed
        /// </summary>
        protected virtual void ApplyEntityNameMapping(DataTable ledgerData)
        {
            // Default implementation: only applies if data has entity name columns
            // Derived classes can override to implement specific mapping logic
            if (POS.Core.UsersModal.logged_in_lang != "ar-SA" || ledgerData == null)
            {
                return;
            }

            // Check if entity name columns exist (to be populated by derived classes)
            if (!ledgerData.Columns.Contains("entity_name") && 
                !ledgerData.Columns.Contains("entity_name_ar"))
            {
                return;
            }

            // Map entity names to Arabic if in ar-SA mode
            foreach (DataRow row in ledgerData.Rows)
            {
                // Try to use entity_name_ar column if it exists
                if (ledgerData.Columns.Contains("entity_name_ar"))
                {
                    object nameArObj = row["entity_name_ar"];
                    string nameAr = nameArObj == null || nameArObj == DBNull.Value ? string.Empty : Convert.ToString(nameArObj).Trim();
                    if (!string.IsNullOrWhiteSpace(nameAr) && ledgerData.Columns.Contains("entity_name"))
                    {
                        row["entity_name"] = nameAr;
                    }
                }
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
            if (_agingData?.Rows.Count > 0)
            {
                DataRow row = _agingData.Rows[0];

                // Validate that expected aging columns exist before accessing them
                if (!_agingData.Columns.Contains("bucket_0_30") || 
                    !_agingData.Columns.Contains("bucket_31_60") || 
                    !_agingData.Columns.Contains("bucket_61_90") || 
                    !_agingData.Columns.Contains("bucket_90_plus"))
                {
                    // Aging data schema mismatch - columns missing from result set
                    ResetAgingPanel();
                    return;
                }

                decimal bucket0_30 = row["bucket_0_30"] != DBNull.Value ? Convert.ToDecimal(row["bucket_0_30"]) : 0;
                decimal bucket31_60 = row["bucket_31_60"] != DBNull.Value ? Convert.ToDecimal(row["bucket_31_60"]) : 0;
                decimal bucket61_90 = row["bucket_61_90"] != DBNull.Value ? Convert.ToDecimal(row["bucket_61_90"]) : 0;
                decimal bucket90Plus = row["bucket_90_plus"] != DBNull.Value ? Convert.ToDecimal(row["bucket_90_plus"]) : 0;

                lbl0_30.Text = UiMessages.T($"0-30: {bucket0_30:N2}", $"0-30: {bucket0_30:N2}");
                lbl31_60.Text = UiMessages.T($"31-60: {bucket31_60:N2}", $"31-60: {bucket31_60:N2}");
                lbl61_90.Text = UiMessages.T($"61-90: {bucket61_90:N2}", $"61-90: {bucket61_90:N2}");
                lbl90Plus.Text = UiMessages.T($"90+: {bucket90Plus:N2}", $"90+: {bucket90Plus:N2}");
            }
            else
            {
                ResetAgingPanel();
            }
        }

        /// <summary>
        /// Reset aging analysis panel
        /// </summary>
        protected virtual void ResetAgingPanel()
        {
            lbl0_30.Text = UiMessages.T("0-30: 0", "0-30: 0");
            lbl31_60.Text = UiMessages.T("31-60: 0", "31-60: 0");
            lbl61_90.Text = UiMessages.T("61-90: 0", "61-90: 0");
            lbl90Plus.Text = UiMessages.T("90+: 0", "90+: 0");
        }

        /// <summary>
        /// Print ledger
        /// </summary>
        protected virtual void PrintLedger()
        {
            if (dgvLedger.Rows.Count == 0)
            {
                UiMessages.ShowWarning(
                    UiMessages.T("No data to print", "لا توجد بيانات للطباعة"), 
                    UiMessages.T("Please load data first", "يرجى تحميل البيانات أولاً"));
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
                UiMessages.ShowError(
                    UiMessages.T("Print error", "خطأ في الطباعة"), 
                    ex.Message);
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
                UiMessages.ShowWarning(
                    UiMessages.T("No data to export", "لا توجد بيانات للتصدير"), 
                    UiMessages.T("Please load data first", "يرجى تحميل البيانات أولاً"));
                return;
            }

            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = UiMessages.T("Excel files (*.xlsx)|*.xlsx|CSV files (*.csv)|*.csv", "ملفات Excel (*.xlsx)|*.xlsx|ملفات CSV (*.csv)|*.csv"),
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
                    UiMessages.ShowInfo(
                        UiMessages.T("Success", "نجح"), 
                        UiMessages.T("Data exported successfully", "تم تصدير البيانات بنجاح"));
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    UiMessages.T("Export error", "خطأ في التصدير"), 
                    ex.Message);
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
            UiMessages.ShowInfo(
                UiMessages.T("Not implemented", "غير مُنفّذ"), 
                UiMessages.T("Payment functionality to be implemented in derived class", "يجب تنفيذ وظيفة الدفع في الفئة المشتقة"));
        }
    }
}
