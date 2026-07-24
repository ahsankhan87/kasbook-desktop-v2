using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using POS.Core;
using pos.UI;

namespace pos.Reports.FixedAssets
{
    public partial class frm_fixed_asset_schedule_report : Form
    {
        private readonly FixedAssetScheduleReportGenerator _generator;
        private DataTable _reportData;

        public frm_fixed_asset_schedule_report()
        {
            InitializeComponent();
            _generator = new FixedAssetScheduleReportGenerator();
            _reportData = new DataTable();
        }

        private void frm_fixed_asset_schedule_report_Load(object sender, EventArgs e)
        {
            try
            {
                AppTheme.Apply(this);
                dtAsOfDate.Value = DateTime.Today;
                lblCompany.Text = UsersModal.logged_in_company_name;
                LoadReport();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            FixedAssetReportPrintExportHelper.ExportExcel(_reportData, "fixed_asset_schedule", this);
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            FixedAssetReportPrintExportHelper.ShowGridPrintPreview(
                dgvReport,
                "Fixed Asset Schedule",
                "As of: " + dtAsOfDate.Value.ToString("yyyy-MM-dd"),
                true);
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            FixedAssetReportPrintExportHelper.ExportPdfFromDataTable(
                this,
                _reportData,
                lblCompany.Text,
                "Fixed Asset Schedule",
                "As of: " + dtAsOfDate.Value.ToString("yyyy-MM-dd"),
                true,
                "FixedAssetSchedule");
        }

        private void LoadReport()
        {
            _reportData = _generator.Build(dtAsOfDate.Value.Date);
            dgvReport.DataSource = _reportData;
            lblAsOfDate.Text = "As of Date: " + dtAsOfDate.Value.ToString("yyyy-MM-dd");
            FormatGrid();
        }

        private void FormatGrid()
        {
            if (dgvReport.Columns.Count == 0)
            {
                return;
            }

            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvReport.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            string[] moneyColumns = { "OpeningCost", "Additions", "Disposals", "Depreciation", "ClosingWDV" };
            foreach (string col in moneyColumns)
            {
                if (dgvReport.Columns.Contains(col))
                {
                    dgvReport.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvReport.Columns[col].DefaultCellStyle.Format = "N2";
                }
            }

            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                string rowType = Convert.ToString(row.Cells["RowType"].Value);
                if (rowType == "CATEGORY_TOTAL")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(235, 245, 255);
                    row.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
                else if (rowType == "GRAND_TOTAL")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(215, 232, 255);
                    row.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }
        }
    }
}
