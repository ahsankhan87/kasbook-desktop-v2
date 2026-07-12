using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using POS.BLL;
using POS.Core;
using pos.UI;
using pos.UI.Busy;

namespace pos.Accounting.CostCenter
{
    /// <summary>
    /// Departmental P&L Report Form - Shows income/expense breakdown by cost center
    /// </summary>
    public partial class frm_departmental_pl : Form
    {
        private CostCenterBLL bll = new CostCenterBLL();
        private List<int> selectedCostCenterIds = new List<int>();

        public frm_departmental_pl()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Apply theme
            AppTheme.Apply(this);

            // Setup event handlers
            btnRefresh.Click += BtnRefresh_Click;
            btnExportExcel.Click += BtnExportExcel_Click;
            btnCompare.Click += BtnCompare_Click;
            btnSelectAll.Click += BtnSelectAll_Click;
            btnClearSelection.Click += BtnClearSelection_Click;

            // Setup date pickers
            dtpFromDate.Value = new DateTime(DateTime.Today.Year, 1, 1);
            dtpToDate.Value = DateTime.Today;

            // Setup cost center checklist
            LoadCostCenters();
        }

        private void LoadCostCenters()
        {
            try
            {
                DataTable dt = bll.GetCostCenterDropdown();
                chkListCostCenters.Items.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    string displayText = row["display_text"]?.ToString() ?? "";
                    int id = (int)row["id"];
                    chkListCostCenters.Items.Add(new CostCenterItem { Id = id, Text = displayText });
                }

                // Select all by default
                for (int i = 0; i < chkListCostCenters.Items.Count; i++)
                    chkListCostCenters.SetItemChecked(i, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cost centers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListCostCenters.Items.Count; i++)
                chkListCostCenters.SetItemChecked(i, true);
        }

        private void BtnClearSelection_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < chkListCostCenters.Items.Count; i++)
                chkListCostCenters.SetItemChecked(i, false);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtpFromDate.Value > dtpToDate.Value)
                {
                    MessageBox.Show("From Date must be before To Date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get selected cost center IDs
                selectedCostCenterIds.Clear();
                foreach (var item in chkListCostCenters.CheckedItems)
                {
                    if (item is CostCenterItem ccItem)
                        selectedCostCenterIds.Add(ccItem.Id);
                }

                using (BusyScope.Show(this, "Loading departmental P&L..."))
                {
                    DataTable dt = bll.GetDepartmentalPL(
                        dtpFromDate.Value.Date,
                        dtpToDate.Value.Date,
                        selectedCostCenterIds.Count > 0 ? selectedCostCenterIds : null
                    );

                    dgvReport.DataSource = dt;
                    dgvReport.AutoResizeColumns();

                    // Format numeric columns
                    foreach (DataGridViewColumn col in dgvReport.Columns)
                    {
                        if (col.Name.Contains("Budget") || col.Name.Contains("Total") || col.Name.Contains("Amount") ||
                            col.Name.Contains("Income") || col.Name.Contains("Expense") || col.Name.Contains("Profit"))
                        {
                            col.DefaultCellStyle.Format = "N2";
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }

                    // Alternate row colors
                    foreach (DataGridViewRow row in dgvReport.Rows)
                    {
                        if (row.Index % 2 == 0)
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
                        else
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    }

                    lblRecordCount.Text = $"Records: {dgvReport.Rows.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCompare_Click(object sender, EventArgs e)
        {
            // Year-over-year or same period last year comparison
            try
            {
                DateTime previousYearFrom = dtpFromDate.Value.AddYears(-1);
                DateTime previousYearTo = dtpToDate.Value.AddYears(-1);

                MessageBox.Show($"Comparing:\nCurrent: {dtpFromDate.Value:MM/dd/yyyy} to {dtpToDate.Value:MM/dd/yyyy}\n" +
                    $"Previous Year: {previousYearFrom:MM/dd/yyyy} to {previousYearTo:MM/dd/yyyy}",
                    "Comparison Period", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // TODO: Implement side-by-side comparison view
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            if (dgvReport.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "Excel Files (*.xlsx)|*.xlsx",
                    FileName = $"Departmental_PL_{DateTime.Today:yyyyMMdd}.xlsx"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (BusyScope.Show(this, "Exporting to CSV..."))
                    {
                        ExportToCsv(sfd.FileName.Replace(".xlsx", ".csv"));
                        MessageBox.Show("Exported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Export error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCsv(string filePath)
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath))
            {
                // Write headers
                var headers = new List<string>();
                foreach (DataGridViewColumn col in dgvReport.Columns)
                    headers.Add($"\"{col.HeaderText}\"");
                writer.WriteLine(string.Join(",", headers));

                // Write data
                foreach (DataGridViewRow row in dgvReport.Rows)
                {
                    var values = new List<string>();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string value = cell.Value?.ToString() ?? "";
                        values.Add($"\"{value}\"");
                    }
                    writer.WriteLine(string.Join(",", values));
                }
            }
        }

        private class CostCenterItem
        {
            public int Id { get; set; }
            public string Text { get; set; }

            public override string ToString() => Text;
        }

        private void FrmDepartmentalPL_Load(object sender, EventArgs e)
        {
            BtnRefresh_Click(null, null);
        }
    }
}
