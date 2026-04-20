using POS.BLL;
using pos.UI;
using pos.Reports;
using pos.Reports.Common;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using pos.UI.Busy;

namespace pos.Reports.Taxes
{
    public partial class frm_VatDashboard : Form
    {
        private readonly VatDashboardBLL _bll = new VatDashboardBLL();
        private bool _suppressModeEvents;

        public frm_VatDashboard()
        {
            InitializeComponent();
        }

        private void frm_VatDashboard_Load(object sender, EventArgs e)
        {
            //AppTheme.Apply(this);

            Text = "VAT Dashboard";
            dtFrom.Format = DateTimePickerFormat.Short;
            dtTo.Format = DateTimePickerFormat.Short;
            dtFrom.ShowUpDown = false;
            dtTo.ShowUpDown = false;
            dtFrom.Value = DateTime.Today.AddDays(-30);
            dtTo.Value = DateTime.Today;

            SetupGrids();
            LoadBranches();

            // Run after the form has finished its initial layout/binding so grid styling
            // and summary row formatting appear correctly on first display.
            BeginInvoke((MethodInvoker)delegate
            {
                RefreshDashboard();
            });
        }

        private void LoadBranches()
        {
            try
            {
                var bll = new POS.BLL.BranchesBLL();
                var dt = bll.GetAll();
                cmbBranch.DisplayMember = "name";
                cmbBranch.ValueMember = "id";
                cmbBranch.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    cmbBranch.SelectedValue = POS.Core.UsersModal.logged_in_branch_id;
                }
            }
            catch
            {
                // ignore branch load failures
            }
        }

        private void SetupGrids()
        {
            gridCompany.AutoGenerateColumns = true;
            gridCompany.ReadOnly = true;
            gridCompany.AllowUserToAddRows = false;
            gridCompany.AllowUserToDeleteRows = false;
            gridCompany.RowHeadersVisible = false;
            gridCompany.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridCompany.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridCompany.CellClick -= gridCompany_CellClick;
            gridCompany.CellClick += gridCompany_CellClick;

            gridBranches.AutoGenerateColumns = true;
            gridBranches.ReadOnly = true;
            gridBranches.AllowUserToAddRows = false;
            gridBranches.AllowUserToDeleteRows = false;
            gridBranches.RowHeadersVisible = false;
            gridBranches.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridBranches.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gridBranch.AutoGenerateColumns = true;
            gridBranch.ReadOnly = true;
            gridBranch.AllowUserToAddRows = false;
            gridBranch.AllowUserToDeleteRows = false;
            gridBranch.RowHeadersVisible = false;
            gridBranch.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridBranch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            RefreshDashboard();
        }

        private void gridCompany_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || gridCompany.CurrentRow == null)
                return;

            string term = Convert.ToString(gridCompany.CurrentRow.Cells["Terms"].Value);
            if (string.IsNullOrWhiteSpace(term))
                return;

            if (!CanOpenInvoiceDetails(term))
                return;

            DateTime from = dtFrom.Value.Date;
            DateTime to = dtTo.Value.Date;
            if (from > to)
            {
                var tmp = from;
                from = to;
                to = tmp;
            }

            using (var frm = new frm_VatInvoiceDetails(from, to, term))
            {
                frm.ShowDialog(this);
            }
        }

        private static bool CanOpenInvoiceDetails(string term)
        {
            return string.Equals(term, "Sales", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(term, "Sales Return", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(term, "Purchases", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(term, "Purchase Return", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(term, "Total Sales", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(term, "Total Purchases", StringComparison.OrdinalIgnoreCase);
        }

        private void ModeChanged(object sender, EventArgs e)
        {
            if (_suppressModeEvents) return;
            // Mode affects export/print only (not the on-screen grids)
            cmbBranch.Visible = rbBranch.Checked;
        }

        private void cmbBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressModeEvents) return;
            // branch selection affects export/print only
        }

        private void RefreshDashboard()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading VAT dashboard...", "ÌÇÑí ÊÍãíá áæÍÉ ÖÑíÈÉ ÇáÞíãÉ ÇáãÖÇÝÉ...")))
                {
                var from = dtFrom.Value.Date;
                var to = dtTo.Value.Date;

                if (from > to)
                {
                    var tmp = from;
                    from = to;
                    to = tmp;
                }

                // keep UI consistent
                dtFrom.Value = from;
                dtTo.Value = to;

                DataTable company = _bll.GetCompanySummary(from, to);
                AppendCompanyTotalsAndGrandTotal(company);
                gridCompany.DataSource = company;

                DataTable branches = _bll.GetBranchMovement(from, to);
                AppendGrandTotal(branches, termsColumnName: "Terms", docsColumnName: "Doc");
                gridBranches.DataSource = branches;

                DataTable branchSummary = _bll.GetBranchSummary(from, to);
                AppendGrandTotal(branchSummary, termsColumnName: "Terms", docsColumnName: "Docs");
                gridBranch.DataSource = branchSummary;

                // KPI cards are based on company totals (ZATCA formula)
                // Net Payable = Sales VAT - Sales Return VAT - Purchase VAT + Purchase Return VAT
                // Source rows may be signed in the grid, so use absolute values per term first.
                decimal salesVat = Math.Abs(GetVat(company, "Sales"));
                decimal salesReturnVat = Math.Abs(GetVat(company, "Sales Return"));
                decimal purchaseVat = Math.Abs(GetVat(company, "Purchases"));
                decimal purchaseReturnVat = Math.Abs(GetVat(company, "Purchase Return"));

                decimal vatCollected = salesVat - salesReturnVat;
                decimal vatPaid = purchaseVat - purchaseReturnVat;
                decimal netVat = vatCollected - vatPaid;

                lblVatCollected.Text = vatCollected.ToString("N2");
                lblVatPaid.Text = vatPaid.ToString("N2");
                lblVatNet.Text = netVat.ToString("N2");
                lblVatNet.ForeColor = netVat >= 0 ? Color.DarkGreen : Color.Maroon;

                ApplyMoneyFormatting(gridCompany);
                ApplyMoneyFormatting(gridBranches);
                ApplyMoneyFormatting(gridBranch);

                HighlightSummaryRows(gridCompany);
                HighlightSummaryRows(gridBranches);
                HighlightSummaryRows(gridBranch);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "ÎØÃ", "Error", "ÎØÃ");
            }
        }

        private static void AppendCompanyTotalsAndGrandTotal(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return;
            if (!dt.Columns.Contains("Terms") || !dt.Columns.Contains("NetAmount") || !dt.Columns.Contains("VatAmount")) return;

            // Remove summary rows if already present
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                var terms = Convert.ToString(dt.Rows[i]["Terms"]);
                if (string.Equals(terms, "Total Sales", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(terms, "Total Purchases", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(terms, "Grand Total", StringComparison.OrdinalIgnoreCase))
                {
                    dt.Rows.RemoveAt(i);
                }
            }

            decimal salesNet = Math.Abs(GetAmount(dt, "Terms", "Sales", "NetAmount"));
            decimal salesReturnNet = Math.Abs(GetAmount(dt, "Terms", "Sales Return", "NetAmount"));
            decimal purchaseNet = Math.Abs(GetAmount(dt, "Terms", "Purchases", "NetAmount"));
            decimal purchaseReturnNet = Math.Abs(GetAmount(dt, "Terms", "Purchase Return", "NetAmount"));

            decimal salesVat = Math.Abs(GetAmount(dt, "Terms", "Sales", "VatAmount"));
            decimal salesReturnVat = Math.Abs(GetAmount(dt, "Terms", "Sales Return", "VatAmount"));
            decimal purchaseVat = Math.Abs(GetAmount(dt, "Terms", "Purchases", "VatAmount"));
            decimal purchaseReturnVat = Math.Abs(GetAmount(dt, "Terms", "Purchase Return", "VatAmount"));

            decimal totalSalesNet = salesNet - salesReturnNet;
            decimal totalSalesVat = salesVat - salesReturnVat;

            decimal totalPurchasesNet = purchaseNet - purchaseReturnNet;
            decimal totalPurchasesVat = purchaseVat - purchaseReturnVat;

            decimal grandNet = totalSalesNet - totalPurchasesNet;
            decimal grandVat = totalSalesVat - totalPurchasesVat;

            long salesDocs = GetDocs(dt, "Terms", "Sales", "Docs") + GetDocs(dt, "Terms", "Sales Return", "Docs");
            long purchaseDocs = GetDocs(dt, "Terms", "Purchases", "Docs") + GetDocs(dt, "Terms", "Purchase Return", "Docs");

            var totalSalesRow = dt.NewRow();
            totalSalesRow["Terms"] = "Total Sales";
            if (dt.Columns.Contains("Docs")) totalSalesRow["Docs"] = salesDocs;
            totalSalesRow["NetAmount"] = totalSalesNet;
            totalSalesRow["VatAmount"] = totalSalesVat;
            int salesReturnIndex = FindRowIndex(dt, "Terms", "Sales Return");
            if (salesReturnIndex >= 0)
                dt.Rows.InsertAt(totalSalesRow, salesReturnIndex + 1);
            else
                dt.Rows.Add(totalSalesRow);

            // visual spacer
            dt.Rows.InsertAt(dt.NewRow(), Math.Min(dt.Rows.Count, (salesReturnIndex >= 0 ? salesReturnIndex + 2 : dt.Rows.Count)));

            var totalPurchasesRow = dt.NewRow();
            totalPurchasesRow["Terms"] = "Total Purchases";
            if (dt.Columns.Contains("Docs")) totalPurchasesRow["Docs"] = purchaseDocs;
            totalPurchasesRow["NetAmount"] = totalPurchasesNet;
            totalPurchasesRow["VatAmount"] = totalPurchasesVat;
            int purchaseReturnIndex = FindRowIndex(dt, "Terms", "Purchase Return");
            if (purchaseReturnIndex >= 0)
                dt.Rows.InsertAt(totalPurchasesRow, purchaseReturnIndex + 1);
            else
                dt.Rows.Add(totalPurchasesRow);

            // visual spacer before grand total
            dt.Rows.Add(dt.NewRow());

            var grandRow = dt.NewRow();
            grandRow["Terms"] = "Grand Total";
            if (dt.Columns.Contains("Docs")) grandRow["Docs"] = salesDocs + purchaseDocs;
            grandRow["NetAmount"] = grandNet;
            grandRow["VatAmount"] = grandVat;
            dt.Rows.Add(grandRow);
        }

        private static int FindRowIndex(DataTable dt, string columnName, string value)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (string.Equals(Convert.ToString(dt.Rows[i][columnName]), value, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }

        private static decimal GetColumnTotal(DataTable dt, string columnName)
        {
            if (dt == null || dt.Rows.Count == 0) return 0m;
            if (!dt.Columns.Contains(columnName)) return 0m;

            decimal total = 0m;
            foreach (DataRow r in dt.Rows)
            {
                if (r[columnName] != DBNull.Value)
                    total += Convert.ToDecimal(r[columnName]);
            }
            return total;
        }

        private DataGridView GetActiveGrid()
        {
            // Mode is used only for Export/Print
            if (rbCompany.Checked) return gridCompany;
            if (rbBranches.Checked) return gridBranches;

            // By Branch
            if (rbBranch.Checked)
            {
                // Use selected branch export/print
                int branchId = POS.Core.UsersModal.logged_in_branch_id;
                if (cmbBranch.SelectedValue != null)
                {
                    int parsed;
                    if (int.TryParse(Convert.ToString(cmbBranch.SelectedValue), out parsed))
                        branchId = parsed;
                }

                var from = dtFrom.Value.Date;
                var to = dtTo.Value.Date;
                if (from > to)
                {
                    var tmp = from;
                    from = to;
                    to = tmp;
                }

                var dt = new POS.DLL.VatDashboardDLL().GetBranchSummary(from, to, branchId);
                AppendGrandTotal(dt, termsColumnName: "Terms", docsColumnName: "Docs");
                gridBranch.DataSource = dt;
                HighlightSummaryRows(gridBranch);
                ApplyMoneyFormatting(gridBranch);
                return gridBranch;
            }

            return gridCompany;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var grid = GetActiveGrid();
            var dt = grid?.DataSource as DataTable;
            ExcelExportHelper.ExportDataTableToExcel(dt, "vat_dashboard", this, includeLastRow: true);
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            var p = new DGVPrinterHelper.DGVPrinter();
            p.Title = "VAT Dashboard";
            p.SubTitle = string.Format("From {0}  To {1}", dtFrom.Value.ToShortDateString(), dtTo.Value.ToShortDateString());
            p.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            p.PageNumbers = true;
            p.PorportionalColumns = true;
            p.HeaderCellAlignment = StringAlignment.Near;
            p.PrintPreviewDataGridView(GetActiveGrid());
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            var p = new DGVPrinterHelper.DGVPrinter();
            p.Title = "VAT Dashboard";
            p.SubTitle = string.Format("From {0}  To {1}", dtFrom.Value.ToShortDateString(), dtTo.Value.ToShortDateString());
            p.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            p.PageNumbers = true;
            p.PorportionalColumns = true;
            p.HeaderCellAlignment = StringAlignment.Near;
            p.PrintDataGridView(GetActiveGrid());
        }

        private static void AppendGrandTotal(DataTable dt, string termsColumnName, string docsColumnName)
        {
            if (dt == null) return;
            if (dt.Rows.Count == 0) return;
            if (!dt.Columns.Contains(termsColumnName)) return;

            if (!dt.Columns.Contains("NetAmount") || !dt.Columns.Contains("VatAmount")) return;

            // Avoid adding more than once
            var last = dt.Rows[dt.Rows.Count - 1];
            if (string.Equals(Convert.ToString(last[termsColumnName]), "Grand Total", StringComparison.OrdinalIgnoreCase))
                return;

            long docs = 0;

            foreach (DataRow r in dt.Rows)
            {
                if (!string.IsNullOrWhiteSpace(docsColumnName) && dt.Columns.Contains(docsColumnName) && r[docsColumnName] != DBNull.Value)
                    docs += Convert.ToInt64(r[docsColumnName]);
            }

            decimal net;
            decimal vat;

            // Terms grid (Sales / Sales Return / Purchases / Purchase Return)
            // Use explicit ZATCA formula to avoid sign inconsistencies.
            bool hasTaxTerms = HasTerm(dt, termsColumnName, "Sales")
                               || HasTerm(dt, termsColumnName, "Sales Return")
                               || HasTerm(dt, termsColumnName, "Purchases")
                               || HasTerm(dt, termsColumnName, "Purchase Return");

            if (hasTaxTerms)
            {
                decimal salesNet = Math.Abs(GetAmount(dt, termsColumnName, "Sales", "NetAmount"));
                decimal salesReturnNet = Math.Abs(GetAmount(dt, termsColumnName, "Sales Return", "NetAmount"));
                decimal purchaseNet = Math.Abs(GetAmount(dt, termsColumnName, "Purchases", "NetAmount"));
                decimal purchaseReturnNet = Math.Abs(GetAmount(dt, termsColumnName, "Purchase Return", "NetAmount"));

                decimal salesVat = Math.Abs(GetAmount(dt, termsColumnName, "Sales", "VatAmount"));
                decimal salesReturnVat = Math.Abs(GetAmount(dt, termsColumnName, "Sales Return", "VatAmount"));
                decimal purchaseVat = Math.Abs(GetAmount(dt, termsColumnName, "Purchases", "VatAmount"));
                decimal purchaseReturnVat = Math.Abs(GetAmount(dt, termsColumnName, "Purchase Return", "VatAmount"));

                // Net = Sales - Sales Return - Purchases + Purchase Return
                net = salesNet - salesReturnNet - purchaseNet + purchaseReturnNet;
                // VAT = Sales VAT - Sales Return VAT - Purchase VAT + Purchase Return VAT
                vat = salesVat - salesReturnVat - purchaseVat + purchaseReturnVat;
            }
            else
            {
                // Non-terms grid (e.g., branch movement already carries net payable per branch)
                net = 0m;
                vat = 0m;
                foreach (DataRow r in dt.Rows)
                {
                    if (r["NetAmount"] != DBNull.Value) net += Convert.ToDecimal(r["NetAmount"]);
                    if (r["VatAmount"] != DBNull.Value) vat += Convert.ToDecimal(r["VatAmount"]);
                }
            }

            var totalRow = dt.NewRow();
            totalRow[termsColumnName] = "Grand Total";
            if (!string.IsNullOrWhiteSpace(docsColumnName) && dt.Columns.Contains(docsColumnName))
                totalRow[docsColumnName] = docs;
            totalRow["NetAmount"] = net;
            totalRow["VatAmount"] = vat;
            dt.Rows.Add(totalRow);
        }

        private static bool HasTerm(DataTable dt, string termsColumnName, string term)
        {
            foreach (DataRow r in dt.Rows)
            {
                if (string.Equals(Convert.ToString(r[termsColumnName]), term, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private static decimal GetAmount(DataTable dt, string termsColumnName, string term, string amountColumn)
        {
            decimal amount = 0m;
            foreach (DataRow r in dt.Rows)
            {
                if (!string.Equals(Convert.ToString(r[termsColumnName]), term, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (r[amountColumn] == null || r[amountColumn] == DBNull.Value)
                    continue;

                amount += Convert.ToDecimal(r[amountColumn]);
            }
            return amount;
        }

        private static long GetDocs(DataTable dt, string termsColumnName, string term, string docsColumn)
        {
            long docs = 0;
            if (!dt.Columns.Contains(docsColumn)) return 0;

            foreach (DataRow r in dt.Rows)
            {
                if (!string.Equals(Convert.ToString(r[termsColumnName]), term, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (r[docsColumn] == null || r[docsColumn] == DBNull.Value)
                    continue;

                docs += Convert.ToInt64(r[docsColumn]);
            }

            return docs;
        }

        private static void HighlightSummaryRows(DataGridView grid)
        {
            if (grid == null) return;
            if (grid.Rows.Count == 0) return;

            foreach (DataGridViewRow row in grid.Rows)
            {
                string term = string.Empty;

                if (row.DataGridView.Columns.Contains("Terms"))
                    term = Convert.ToString(row.Cells["Terms"].Value);

                if (string.IsNullOrWhiteSpace(term))
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    continue;
                }

                if (string.Equals(term, "Grand Total", StringComparison.OrdinalIgnoreCase))
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 240);
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    row.DefaultCellStyle.Font = new Font(grid.Font, FontStyle.Bold);
                }
                else if (string.Equals(term, "Total Sales", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(term, "Total Purchases", StringComparison.OrdinalIgnoreCase))
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(240, 247, 255);
                    row.DefaultCellStyle.ForeColor = Color.DarkBlue;
                    row.DefaultCellStyle.Font = new Font(grid.Font, FontStyle.Bold);
                }
            }
        }

        private static void ApplyMoneyFormatting(DataGridView grid)
        {
            if (grid.DataSource == null) return;

            foreach (DataGridViewColumn col in grid.Columns)
            {
                if (string.Equals(col.Name, "NetAmount", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(col.Name, "VatAmount", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(col.Name, "total", StringComparison.OrdinalIgnoreCase))
                {
                    col.DefaultCellStyle.Format = "#,##0.00;-#,##0.00;0.00";

                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        private static decimal GetVat(DataTable dt, string terms)
        {
            if (dt == null) return 0m;
            foreach (DataRow r in dt.Rows)
            {
                if (string.Equals(Convert.ToString(r["Terms"]), terms, StringComparison.OrdinalIgnoreCase))
                {
                    if (r["VatAmount"] == null || r["VatAmount"] == DBNull.Value) return 0m;
                    return Convert.ToDecimal(r["VatAmount"]);
                }
            }
            return 0m;
        }
    }
}
