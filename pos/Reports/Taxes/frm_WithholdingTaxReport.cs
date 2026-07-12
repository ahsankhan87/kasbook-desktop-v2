using pos.UI;
using pos.UI.Busy;
using POS.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace pos.Reports.Taxes
{
    public partial class frm_WithholdingTaxReport : Form
    {
        private WHTCalculationBLL _whtBll = new WHTCalculationBLL();

        public frm_WithholdingTaxReport()
        {
            InitializeComponent();
        }

        private void frm_WithholdingTaxReport_Load(object sender, EventArgs e)
        {
            // Apply theme
            UI.AppTheme.Apply(this);

            // Set default dates (current month)
            dtpToDate.Value = DateTime.Now;
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Refresh data
            RefreshReport();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshReport();
        }

        private void RefreshReport()
        {
            try
            {
                using (BusyScope.Show(this, "Loading WHT report..."))
                {
                    DateTime fromDate = dtpFromDate.Value;
                    DateTime toDate = dtpToDate.Value.AddDays(1).AddSeconds(-1);

                    // Load transactions
                    LoadWHTTransactionsGrid(fromDate, toDate);

                    // Load summary
                    LoadWHTSummaryGrid(fromDate, toDate);

                    // Update summary labels
                    UpdateSummaryLabels(fromDate, toDate);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading WHT report: {ex.Message}",
                    $"خطأ في تحميل تقرير الضريبة المحتفظ بها: {ex.Message}",
                    "Error",
                    "خطأ");
            }
        }

        private void LoadWHTTransactionsGrid(DateTime fromDate, DateTime toDate)
        {
            try
            {
                DataTable dt = _whtBll.GetWHTReportDataTable(fromDate, toDate);
                dgvWHTTransactions.DataSource = dt;

                // Format columns
                FormatWHTGrid(dgvWHTTransactions);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading WHT transactions: {ex.Message}",
                    $"خطأ في تحميل معاملات WHT: {ex.Message}",
                    "Error",
                    "خطأ");
            }
        }

        private void LoadWHTSummaryGrid(DateTime fromDate, DateTime toDate)
        {
            try
            {
                DataTable dtSummary = new DataTable();
                dtSummary.Columns.Add("Tax Section", typeof(string));
                dtSummary.Columns.Add("Total Payments", typeof(decimal));
                dtSummary.Columns.Add("Total WHT", typeof(decimal));
                dtSummary.Columns.Add("Avg Rate %", typeof(decimal));
                dtSummary.Columns.Add("Count", typeof(int));

                var summaryBySection = _whtBll.GetWHTSummaryBySection(fromDate, toDate);

                foreach (var section in summaryBySection)
                {
                    dtSummary.Rows.Add(
                        section.TaxSection,
                        section.TotalPaymentAmount,
                        section.TotalWHTAmount,
                        section.AverageWHTRate,
                        section.TransactionCount
                    );
                }

                dgvWHTSummary.DataSource = dtSummary;

                // Format columns
                FormatWHTGrid(dgvWHTSummary);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading WHT summary: {ex.Message}",
                    $"خطأ في تحميل ملخص WHT: {ex.Message}",
                    "Error",
                    "خطأ");
            }
        }

        private void FormatWHTGrid(DataGridView grid)
        {
            if (grid.DataSource == null)
                return;

            grid.ReadOnly = true;

            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (col.Name.Contains("Total") || col.Name.Contains("Payments") || col.Name.Contains("Rate"))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = "N2";
                }

                if (col.Name.Contains("Date"))
                {
                    col.DefaultCellStyle.Format = "dd-MMM-yyyy";
                }

                if (col.Name.Contains("Count"))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        private void UpdateSummaryLabels(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var (totalPayments, totalWHT, transactionCount) = _whtBll.GetWHTTotals(fromDate, toDate);
                decimal avgRate = _whtBll.GetAverageWHTRate(fromDate, toDate);

                lblTotalPayments.Text = $"Total Payments: SAR {totalPayments:N2}";
                lblTotalWHT.Text = $"Total WHT: SAR {totalWHT:N2} | Avg Rate: {avgRate:F2}%";
                lblTransactionCount.Text = $"Transactions: {transactionCount}";
            }
            catch
            {
                lblTotalPayments.Text = "Total Payments: Error";
                lblTotalWHT.Text = "Total WHT: Error";
                lblTransactionCount.Text = "Transactions: Error";
            }
        }

        private void btnGeneratePSID_Click(object sender, EventArgs e)
        {
            try
            {
                UiMessages.ShowInfo(
                    "Generate PSID functionality would integrate with FBR Pakistan's online system. This is a placeholder for PSID generation.",
                    "ستتم وظيفة إنشاء PSID هنا. هذا عنصر نائب لتوليد PSID.",
                    "Generate PSID",
                    "إنشاء PSID");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error: {ex.Message}",
                    $"خطأ: {ex.Message}",
                    "Error",
                    "خطأ");
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    FileName = $"WHTReport_{DateTime.Now:yyyyMMdd}.xlsx",
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    DefaultExt = "xlsx"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (BusyScope.Show(this, "Exporting to Excel..."))
                    {
                        DateTime fromDate = dtpFromDate.Value;
                        DateTime toDate = dtpToDate.Value.AddDays(1).AddSeconds(-1);

                        // Export logic would go here
                        DataTable dtWHT = _whtBll.PrepareWHTExportData(fromDate, toDate);

                        UiMessages.ShowInfo(
                            "Excel export functionality would be implemented here with EPPlus or similar library.",
                            "وظيفة تصدير Excel يتم تنفيذها هنا باستخدام مكتبة مماثلة.",
                            "Export",
                            "تصدير");
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error exporting to Excel: {ex.Message}",
                    $"خطأ في التصدير إلى Excel: {ex.Message}",
                    "Error",
                    "خطأ");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                UiMessages.ShowInfo(
                    "Print functionality would be implemented using PrintDocument or similar.",
                    "يتم تنفيذ وظيفة الطباعة باستخدام PrintDocument أو مماثل.",
                    "Print",
                    "طباعة");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error printing: {ex.Message}",
                    $"خطأ في الطباعة: {ex.Message}",
                    "Error",
                    "خطأ");
            }
        }
    }
}
