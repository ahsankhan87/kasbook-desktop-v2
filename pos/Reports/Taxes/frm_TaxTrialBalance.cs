using pos.UI;
using pos.UI.Busy;
using POS.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace pos.Reports.Taxes
{
    public partial class frm_TaxTrialBalance : Form
    {
        private TaxReportingBLL _taxReportingBll = new TaxReportingBLL();
        private int _selectedFinancialYearId = 0;

        public frm_TaxTrialBalance()
        {
            InitializeComponent();
        }

        private void frm_TaxTrialBalance_Load(object sender, EventArgs e)
        {
            // Apply theme
            UI.AppTheme.Apply(this);

            // Load financial years
            LoadFinancialYears();

            // Set default account type filter
            cmbAccountType.SelectedIndex = 0;  // "All"

            // Load data
            RefreshReport();
        }

        private void LoadFinancialYears()
        {
            try
            {
                using (BusyScope.Show(this, "Loading financial years..."))
                {
                    var financialYearsBll = new FiscalYearBLL();
                    DataTable dtYears = financialYearsBll.GetAll();

                    cmbFinancialYear.Items.Clear();

                    if (dtYears != null && dtYears.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtYears.Rows)
                        {
                            int id = Convert.ToInt32(row["id"]);
                            string name = Convert.ToString(row["name"]);
                            cmbFinancialYear.Items.Add(new { Text = name, Value = id });
                        }

                        if (cmbFinancialYear.Items.Count > 0)
                        {
                            cmbFinancialYear.SelectedIndex = 0;
                            _selectedFinancialYearId = ((dynamic)cmbFinancialYear.SelectedItem).Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading financial years: {ex.Message}",
                    $"خطأ في تحميل السنوات المالية: {ex.Message}",
                    "Error",
                    "خطأ");
            }
        }

        private void cmbFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFinancialYear.SelectedItem != null)
            {
                _selectedFinancialYearId = ((dynamic)cmbFinancialYear.SelectedItem).Value;
                RefreshReport();
            }
        }

        private void cmbAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshReport();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshReport();
        }

        private void RefreshReport()
        {
            if (_selectedFinancialYearId == 0)
                return;

            try
            {
                using (BusyScope.Show(this, "Loading trial balance..."))
                {
                    string accountType = cmbAccountType.SelectedItem?.ToString() ?? "All";

                    DataTable dtTrialBalance;

                    if (accountType == "All")
                    {
                        dtTrialBalance = _taxReportingBll.GetIncomeTaxTrialBalanceDataTable(_selectedFinancialYearId);
                    }
                    else if (accountType == "Income")
                    {
                        var incomeAccounts = _taxReportingBll.GetIncomeAccounts(_selectedFinancialYearId);
                        dtTrialBalance = ConvertModalListToDataTable(incomeAccounts);
                    }
                    else  // Expense
                    {
                        var expenseAccounts = _taxReportingBll.GetExpenseAccounts(_selectedFinancialYearId);
                        dtTrialBalance = ConvertModalListToDataTable(expenseAccounts);
                    }

                    dgvTrialBalance.DataSource = dtTrialBalance;

                    // Format grid
                    FormatTrialBalanceGrid();

                    // Update summary
                    UpdateSummaryLabels();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading trial balance: {ex.Message}",
                    $"خطأ في تحميل ميزان المراجعة: {ex.Message}",
                    "Error",
                    "خطأ");
            }
        }

        private DataTable ConvertModalListToDataTable(List<POS.Core.TaxTrialBalanceModal> accounts)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Code", typeof(string));
            dt.Columns.Add("Account Name", typeof(string));
            dt.Columns.Add("Account Group", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Debit", typeof(decimal));
            dt.Columns.Add("Credit", typeof(decimal));
            dt.Columns.Add("Balance", typeof(decimal));

            foreach (var account in accounts)
            {
                dt.Rows.Add(
                    account.AccountCode,
                    account.AccountName,
                    account.AccountGroupName,
                    account.AccountType,
                    account.DebitAmount,
                    account.CreditAmount,
                    account.Balance
                );
            }

            return dt;
        }

        private void FormatTrialBalanceGrid()
        {
            if (dgvTrialBalance.DataSource == null)
                return;

            dgvTrialBalance.ReadOnly = true;

            foreach (DataGridViewColumn col in dgvTrialBalance.Columns)
            {
                if (col.Name == "Debit" || col.Name == "Credit" || col.Name == "Balance")
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = "N2";
                }

                // Remove ID columns if present
                if (col.Name == "AccountId")
                {
                    col.Visible = false;
                }
            }

            // Alternate row colors for better readability
            dgvTrialBalance.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
        }

        private void UpdateSummaryLabels()
        {
            try
            {
                var (totalIncome, totalExpense) = _taxReportingBll.GetIncomeTaxTotals(_selectedFinancialYearId);
                decimal netIncome = _taxReportingBll.GetNetIncome(_selectedFinancialYearId);

                lblTotalIncome.Text = $"Total Income: SAR {totalIncome:N2}";
                lblTotalExpense.Text = $"Total Expense: SAR {totalExpense:N2}";

                // Color code net income
                Color netIncomeColor = netIncome >= 0 ? Color.DarkGreen : Color.DarkRed;
                lblNetIncome.ForeColor = netIncomeColor;
                lblNetIncome.Text = $"Net Income: SAR {netIncome:N2}";
            }
            catch
            {
                lblTotalIncome.Text = "Total Income: Error";
                lblTotalExpense.Text = "Total Expense: Error";
                lblNetIncome.Text = "Net Income: Error";
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    FileName = $"TaxTrialBalance_{DateTime.Now:yyyyMMdd}.xlsx",
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    DefaultExt = "xlsx"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (BusyScope.Show(this, "Exporting to Excel..."))
                    {
                        DataTable dtExport = _taxReportingBll.PrepareExportData(_selectedFinancialYearId);

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
                    "Print functionality would be implemented using PrintDocument or Crystal Reports.",
                    "يتم تنفيذ وظيفة الطباعة باستخدام PrintDocument أو Crystal Reports.",
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
