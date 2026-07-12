using POS.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos.Reports.Financial
{
    public partial class frm_CashFlowStatementReport : Form
    {
        private readonly AccountsBLL _accountsBll = new AccountsBLL();

        public frm_CashFlowStatementReport()
        {
            InitializeComponent();
            WireEvents();
        }

        private void WireEvents()
        {
            Load += Frm_CashFlowStatementReport_Load;
            cmbPeriod.SelectedIndexChanged += (s, e) => ApplyPeriodSelection();
            btnGenerate.Click += (s, e) => GenerateReport();
            dgvCashFlow.DataBindingComplete += DgvCashFlow_DataBindingComplete;
        }

        private void Frm_CashFlowStatementReport_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            cmbPeriod.Items.AddRange(new object[] { "This Month", "This Quarter", "This Year", "Custom Range" });
            cmbMethod.Items.AddRange(new object[] { "Indirect", "Direct" });

            cmbPeriod.SelectedIndex = 0;
            cmbMethod.SelectedIndex = 0;

            dgvCashFlow.AutoGenerateColumns = false;
            dgvCashFlow.Columns["colAmount"].DefaultCellStyle.Format = "N2";
            dgvCashFlow.Columns["colAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ApplyPeriodSelection();
            GenerateReport();
        }

        private void ApplyPeriodSelection()
        {
            DateTime today = DateTime.Today;
            DateTime from;
            DateTime to;

            switch (Convert.ToString(cmbPeriod.SelectedItem))
            {
                case "This Quarter":
                    int quarterStartMonth = ((today.Month - 1) / 3) * 3 + 1;
                    from = new DateTime(today.Year, quarterStartMonth, 1);
                    to = from.AddMonths(3).AddDays(-1);
                    dtpFrom.Enabled = false;
                    dtpTo.Enabled = false;
                    dtpFrom.Value = from;
                    dtpTo.Value = to;
                    break;

                case "This Year":
                    from = new DateTime(today.Year, 1, 1);
                    to = new DateTime(today.Year, 12, 31);
                    dtpFrom.Enabled = false;
                    dtpTo.Enabled = false;
                    dtpFrom.Value = from;
                    dtpTo.Value = to;
                    break;

                case "Custom Range":
                    dtpFrom.Enabled = true;
                    dtpTo.Enabled = true;
                    break;

                default:
                    from = new DateTime(today.Year, today.Month, 1);
                    to = from.AddMonths(1).AddDays(-1);
                    dtpFrom.Enabled = false;
                    dtpTo.Enabled = false;
                    dtpFrom.Value = from;
                    dtpTo.Value = to;
                    break;
            }
        }

        private void GenerateReport()
        {
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                UiMessages.ShowError("From date cannot be after To date.", "لا يمكن أن يكون تاريخ البداية بعد تاريخ النهاية.");
                return;
            }

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Generating cash flow statement...", "جاري إعداد قائمة التدفقات النقدية...")))
                {
                    if (string.Equals(Convert.ToString(cmbMethod.SelectedItem), "Direct", StringComparison.OrdinalIgnoreCase))
                    {
                        UiMessages.ShowInfo("Direct method is currently rendered using the same computed source movements.", "يتم عرض الطريقة المباشرة حاليًا بنفس حركات المصدر المحسوبة.");
                    }

                    DataTable source = _accountsBll.GetCashFlowStatement(dtpFrom.Value.Date, dtpTo.Value.Date);
                    Dictionary<string, decimal> map = source.AsEnumerable().ToDictionary(
                        r => Convert.ToString(r["line_key"]),
                        r => r["amount"] == DBNull.Value ? 0m : Convert.ToDecimal(r["amount"]));

                    decimal netProfit = GetAmount(map, "NET_PROFIT");
                    decimal depAmort = GetAmount(map, "DEP_AMORT");
                    decimal receivablesChange = GetAmount(map, "CHG_RECEIVABLES");
                    decimal payablesChange = GetAmount(map, "CHG_PAYABLES");
                    decimal inventoryChange = GetAmount(map, "CHG_INVENTORY");

                    decimal purchaseFixedAssets = GetAmount(map, "PURCHASE_FIXED_ASSETS");
                    decimal saleFixedAssets = GetAmount(map, "SALE_FIXED_ASSETS");

                    decimal loanProceeds = GetAmount(map, "LOAN_PROCEEDS");
                    decimal loanRepayments = GetAmount(map, "LOAN_REPAYMENTS");
                    decimal ownerDrawings = GetAmount(map, "OWNER_DRAWINGS_DIVIDENDS");

                    decimal openingCash = GetAmount(map, "OPENING_CASH");
                    decimal actualClosingCash = GetAmount(map, "CLOSING_CASH");

                    decimal netOperating = netProfit + depAmort - receivablesChange + payablesChange - inventoryChange;
                    decimal netInvesting = -purchaseFixedAssets + saleFixedAssets;
                    decimal netFinancing = loanProceeds - loanRepayments - ownerDrawings;
                    decimal netIncreaseDecrease = netOperating + netInvesting + netFinancing;
                    decimal calculatedClosingCash = openingCash + netIncreaseDecrease;

                    DataTable report = BuildReportTable(
                        netProfit,
                        depAmort,
                        receivablesChange,
                        payablesChange,
                        inventoryChange,
                        netOperating,
                        purchaseFixedAssets,
                        saleFixedAssets,
                        netInvesting,
                        loanProceeds,
                        loanRepayments,
                        ownerDrawings,
                        netFinancing,
                        netIncreaseDecrease,
                        openingCash,
                        calculatedClosingCash,
                        actualClosingCash);

                    dgvCashFlow.DataSource = report;

                    bool match = Math.Abs(calculatedClosingCash - actualClosingCash) < 0.01m;
                    lblStatus.ForeColor = match ? Color.DarkGreen : Color.DarkRed;
                    lblStatus.Text = match
                        ? "Closing Cash Balance matches Balance Sheet cash accounts ✓"
                        : "Closing Cash Balance mismatch with Balance Sheet cash accounts ✗";
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Unable to generate cash flow statement.", ex.Message);
            }
        }

        private static decimal GetAmount(Dictionary<string, decimal> map, string key)
        {
            decimal value;
            return map.TryGetValue(key, out value) ? value : 0m;
        }

        private static DataTable BuildReportTable(
            decimal netProfit,
            decimal depAmort,
            decimal receivablesChange,
            decimal payablesChange,
            decimal inventoryChange,
            decimal netOperating,
            decimal purchaseFixedAssets,
            decimal saleFixedAssets,
            decimal netInvesting,
            decimal loanProceeds,
            decimal loanRepayments,
            decimal ownerDrawings,
            decimal netFinancing,
            decimal netIncreaseDecrease,
            decimal openingCash,
            decimal calculatedClosingCash,
            decimal actualClosingCash)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Particular", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));

            table.Rows.Add("Operating Activities", DBNull.Value);
            table.Rows.Add("  Net Profit for period", netProfit);
            table.Rows.Add("  Add back: Depreciation & Amortization", depAmort);
            table.Rows.Add("  Increase/Decrease in Receivables", -receivablesChange);
            table.Rows.Add("  Increase/Decrease in Payables", payablesChange);
            table.Rows.Add("  Increase/Decrease in Inventory", -inventoryChange);
            table.Rows.Add("= Net Cash from Operating Activities", netOperating);

            table.Rows.Add("Investing Activities", DBNull.Value);
            table.Rows.Add("  Purchase of Fixed Assets", -purchaseFixedAssets);
            table.Rows.Add("  Sale of Fixed Assets", saleFixedAssets);
            table.Rows.Add("= Net Cash from Investing Activities", netInvesting);

            table.Rows.Add("Financing Activities", DBNull.Value);
            table.Rows.Add("  Loan Proceeds", loanProceeds);
            table.Rows.Add("  Loan Repayments", -loanRepayments);
            table.Rows.Add("  Owner Drawings / Dividends", -ownerDrawings);
            table.Rows.Add("= Net Cash from Financing Activities", netFinancing);

            table.Rows.Add("= NET INCREASE / (DECREASE) IN CASH", netIncreaseDecrease);
            table.Rows.Add("+ Opening Cash Balance", openingCash);
            table.Rows.Add("= Closing Cash Balance (Calculated)", calculatedClosingCash);
            table.Rows.Add("= Closing Cash Balance (Balance Sheet)", actualClosingCash);

            return table;
        }

        private void DgvCashFlow_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvCashFlow.Rows)
            {
                string text = Convert.ToString(row.Cells["colParticular"].Value);
                bool isSection = !string.IsNullOrWhiteSpace(text) && !text.StartsWith(" ") && text.IndexOf('=') < 0;
                bool isTotal = !string.IsNullOrWhiteSpace(text) && text.StartsWith("=");

                if (isSection)
                {
                    row.DefaultCellStyle.Font = new Font(dgvCashFlow.Font, FontStyle.Bold);
                    row.DefaultCellStyle.BackColor = Color.Gainsboro;
                }
                else if (isTotal)
                {
                    row.DefaultCellStyle.Font = new Font(dgvCashFlow.Font, FontStyle.Bold);
                    row.DefaultCellStyle.BackColor = text.Contains("NET INCREASE") ? Color.LightGoldenrodYellow : Color.AliceBlue;
                }
            }
        }
    }
}
