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
            Text = "VAT Dashboard";
            dtFrom.Format = DateTimePickerFormat.Short;
            dtTo.Format = DateTimePickerFormat.Short;
            dtFrom.ShowUpDown = false;
            dtTo.ShowUpDown = false;
            dtFrom.Value = DateTime.Today.AddDays(-30);
            dtTo.Value = DateTime.Today;

            SetupGrids();
            LoadBranches();
            RefreshDashboard();
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
                AppendGrandTotal(company, termsColumnName: "Terms", docsColumnName: "Docs");
                gridCompany.DataSource = company;

                DataTable branches = _bll.GetBranchMovement(from, to);
                AppendGrandTotal(branches, termsColumnName: "Terms", docsColumnName: "Doc");
                gridBranches.DataSource = branches;

                DataTable branchSummary = _bll.GetBranchSummary(from, to);
                AppendGrandTotal(branchSummary, termsColumnName: "Terms", docsColumnName: "Docs");
                gridBranch.DataSource = branchSummary;

                // KPI cards are based on company totals
                decimal vatCollected = GetVat(company, "Sales") + GetVat(company, "Purchase Return");
                decimal vatPaid = -(GetVat(company, "Purchases") + GetVat(company, "Sales Return"));
                decimal netVat = vatCollected - vatPaid;

                lblVatCollected.Text = vatCollected.ToString("N2");
                lblVatPaid.Text = vatPaid.ToString("N2");
                lblVatNet.Text = netVat.ToString("N2");
                lblVatNet.ForeColor = netVat >= 0 ? Color.DarkGreen : Color.Maroon;

                ApplyMoneyFormatting(gridCompany);
                ApplyMoneyFormatting(gridBranches);
                ApplyMoneyFormatting(gridBranch);

                HighlightGrandTotalRow(gridCompany);
                HighlightGrandTotalRow(gridBranches);
                HighlightGrandTotalRow(gridBranch);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "ÎØÃ", "Error", "ÎØÃ");
            }
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
                HighlightGrandTotalRow(gridBranch);
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

            decimal net = 0m;
            decimal vat = 0m;
            long docs = 0;

            foreach (DataRow r in dt.Rows)
            {
                if (r["NetAmount"] != DBNull.Value) net += Convert.ToDecimal(r["NetAmount"]);
                if (r["VatAmount"] != DBNull.Value) vat += Convert.ToDecimal(r["VatAmount"]);
                if (!string.IsNullOrWhiteSpace(docsColumnName) && dt.Columns.Contains(docsColumnName) && r[docsColumnName] != DBNull.Value)
                    docs += Convert.ToInt64(r[docsColumnName]);
            }

            var totalRow = dt.NewRow();
            totalRow[termsColumnName] = "Grand Total";
            if (!string.IsNullOrWhiteSpace(docsColumnName) && dt.Columns.Contains(docsColumnName))
                totalRow[docsColumnName] = docs;
            totalRow["NetAmount"] = net;
            totalRow["VatAmount"] = vat;
            dt.Rows.Add(totalRow);
        }

        private static void HighlightGrandTotalRow(DataGridView grid)
        {
            if (grid == null) return;
            if (grid.Rows.Count == 0) return;

            var row = grid.Rows[grid.Rows.Count - 1];
            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 240);
            row.DefaultCellStyle.Font = new Font(grid.Font, FontStyle.Bold);
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
                    col.DefaultCellStyle.Format = "N2";
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
