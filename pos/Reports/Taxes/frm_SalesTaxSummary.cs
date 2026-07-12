using pos.UI;
using pos.UI.Busy;
using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace pos.Reports.Taxes
{
    public partial class frm_SalesTaxSummary : Form
    {
        private SalesTaxBLL _salesTaxBll = new SalesTaxBLL();

        public frm_SalesTaxSummary()
        {
            InitializeComponent();
        }

        private void frm_SalesTaxSummary_Load(object sender, EventArgs e)
        {
            // Apply theme
            UI.AppTheme.Apply(this);

            // Set default dates (current month)
            dtpToDate.Value = DateTime.Now;
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Load tax registration number from company settings
            LoadTaxRegistrationNumber();

            // Refresh data
            RefreshReport();
        }

        private void LoadTaxRegistrationNumber()
        {
            try
            {
                // Load from company settings - adjust as per your company modal
                var companyBll = new CompaniesBLL();
                DataTable companyDt = companyBll.GetCompany();

                if (companyDt != null && companyDt.Rows.Count > 0)
                {
                    string taxRegNo = Convert.ToString(companyDt.Rows[0]["taxRegistrationNo"] ?? string.Empty);
                    txtTaxRegNo.Text = !string.IsNullOrEmpty(taxRegNo) ? taxRegNo : "Not Set";
                }
            }
            catch
            {
                txtTaxRegNo.Text = "Error Loading";
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshReport();
        }

        private void RefreshReport()
        {
            try
            {
                using (BusyScope.Show(this, "Loading tax report..."))
                {
                    DateTime fromDate = dtpFromDate.Value;
                    DateTime toDate = dtpToDate.Value.AddDays(1).AddSeconds(-1);

                    // Get tax summary
                    SalesTaxSummaryModal summary = _salesTaxBll.CalculateSalesTaxSummary(
                        fromDate, toDate, txtTaxRegNo.Text);

                    // Display summary
                    DisplaySummary(summary);

                    // Load detail grids
                    LoadSalesTaxRegisterGrid(fromDate, toDate);
                    LoadPurchaseTaxRegisterGrid(fromDate, toDate);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading tax report: {ex.Message}",
                    $"خطأ في تحميل تقرير الضريبة: {ex.Message}",
                    "Error",
                    "خطأ");
            }
        }

        private void DisplaySummary(SalesTaxSummaryModal summary)
        {
            // Output Tax
            lblStandardRatedTax.Text = $"SAR {summary.StandardRatedSalesTax:N2}";
            lblZeroRatedSales.Text = $"SAR {summary.ZeroRatedSalesAmount:N2}";
            lblExemptSales.Text = $"SAR {summary.ExemptSalesAmount:N2}";

            // Input Tax
            lblTaxablePurchases.Text = $"SAR {summary.TaxablePurchasesTax:N2}";
            lblImportTax.Text = $"SAR {summary.ImportTaxPaid:N2}";

            // Net Tax
            lblTotalOutputTax.Text = $"SAR {summary.TotalOutputTax:N2}";
            lblTotalInputTax.Text = $"SAR {summary.TotalInputTax:N2}";

            // Net Tax with color indicator
            Color netTaxColor = summary.NetTaxPayable >= 0 ? Color.Red : Color.Green;
            lblNetTaxPayable.ForeColor = netTaxColor;
            lblNetTaxPayable.Text = $"SAR {Math.Abs(summary.NetTaxPayable):N2}";

            if (summary.NetTaxPayable < 0)
            {
                lblNetTaxPayable.Text += " (REFUNDABLE)";
            }
            else
            {
                lblNetTaxPayable.Text += " (PAYABLE)";
            }
        }

        private void LoadSalesTaxRegisterGrid(DateTime fromDate, DateTime toDate)
        {
            try
            {
                DataTable dt = _salesTaxBll.GetSalesTaxRegisterDataTable(fromDate, toDate, "SALES");
                dgvSalesTaxRegister.DataSource = dt;

                // Format columns
                FormatTaxGrid(dgvSalesTaxRegister);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading sales tax register: {ex.Message}",
                    $"خطأ في تحميل سجل ضريبة المبيعات: {ex.Message}",
                    "Error",
                    "خطأ");
            }
        }

        private void LoadPurchaseTaxRegisterGrid(DateTime fromDate, DateTime toDate)
        {
            try
            {
                DataTable dt = _salesTaxBll.GetSalesTaxRegisterDataTable(fromDate, toDate, "PURCHASES");
                dgvPurchaseTaxRegister.DataSource = dt;

                // Format columns
                FormatTaxGrid(dgvPurchaseTaxRegister);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading purchase tax register: {ex.Message}",
                    $"خطأ في تحميل سجل ضريبة المشتريات: {ex.Message}",
                    "Error",
                    "خطأ");
            }
        }

        private void FormatTaxGrid(DataGridView grid)
        {
            if (grid.DataSource == null)
                return;

            // Make columns read-only
            grid.ReadOnly = true;

            // Format numeric columns
            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (col.Name.Contains("Amount") || col.Name.Contains("Rate"))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = "N2";
                }

                if (col.Name.Contains("Date"))
                {
                    col.DefaultCellStyle.Format = "dd-MMM-yyyy";
                }
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    FileName = $"SalesTaxReport_{DateTime.Now:yyyyMMdd}.xlsx",
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
                        // For now, use the prepared data
                        DataTable dtSales = _salesTaxBll.PrepareExportData(fromDate, toDate, "SALES");
                        DataTable dtPurchases = _salesTaxBll.PrepareExportData(fromDate, toDate, "PURCHASES");

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
