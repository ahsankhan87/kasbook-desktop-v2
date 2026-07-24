using System;
using System.Data;
using System.Windows.Forms;
using POS.Core;
using pos.UI;

namespace pos.Reports.FixedAssets
{
    public partial class frm_depreciation_projection_report : Form
    {
        private readonly DepreciationProjectionReportGenerator _generator;
        private DataTable _reportData;

        public frm_depreciation_projection_report()
        {
            InitializeComponent();
            _generator = new DepreciationProjectionReportGenerator();
            _reportData = new DataTable();
        }

        private void frm_depreciation_projection_report_Load(object sender, EventArgs e)
        {
            try
            {
                AppTheme.Apply(this);
                lblCompany.Text = UsersModal.logged_in_company_name;
                dtFutureDate.Value = DateTime.Today.AddMonths(12);
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
            FixedAssetReportPrintExportHelper.ExportExcel(_reportData, "depreciation_projection", this);
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            FixedAssetReportPrintExportHelper.ShowGridPrintPreview(
                dgvReport,
                "Depreciation Projection",
                "Projected as of: " + dtFutureDate.Value.ToString("yyyy-MM-dd"),
                true);
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            FixedAssetReportPrintExportHelper.ExportPdfFromDataTable(
                this,
                _reportData,
                lblCompany.Text,
                "Depreciation Projection",
                "Projected as of: " + dtFutureDate.Value.ToString("yyyy-MM-dd"),
                true,
                "DepreciationProjection");
        }

        private void LoadReport()
        {
            _reportData = _generator.Build(dtFutureDate.Value.Date);
            dgvReport.DataSource = _reportData;
            FormatGrid();
        }

        private void FormatGrid()
        {
            if (dgvReport.Columns.Count == 0)
            {
                return;
            }

            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvReport.RowHeadersVisible = false;

            string[] moneyColumns = { "Current WDV", "Projected Depreciation", "Projected WDV" };
            foreach (string col in moneyColumns)
            {
                if (dgvReport.Columns.Contains(col))
                {
                    dgvReport.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvReport.Columns[col].DefaultCellStyle.Format = "N2";
                }
            }
        }
    }
}
