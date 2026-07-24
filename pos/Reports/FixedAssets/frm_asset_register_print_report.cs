using System;
using System.Data;
using System.Windows.Forms;
using POS.Core;
using pos.UI;

namespace pos.Reports.FixedAssets
{
    public partial class frm_asset_register_print_report : Form
    {
        private readonly AssetRegisterReportGenerator _generator;
        private DataTable _reportData;

        public frm_asset_register_print_report()
        {
            InitializeComponent();
            _generator = new AssetRegisterReportGenerator();
            _reportData = new DataTable();
        }

        private void frm_asset_register_print_report_Load(object sender, EventArgs e)
        {
            try
            {
                AppTheme.Apply(this);
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
            FixedAssetReportPrintExportHelper.ExportExcel(_reportData, "fixed_asset_register", this);
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            FixedAssetReportPrintExportHelper.ShowGridPrintPreview(
                dgvReport,
                "Asset Register Print",
                "One row per asset for physical verification",
                true);
        }

        private void btnExportPdf_Click(object sender, EventArgs e)
        {
            FixedAssetReportPrintExportHelper.ExportPdfFromDataTable(
                this,
                _reportData,
                lblCompany.Text,
                "Asset Register Print",
                "Physical Verification Register",
                true,
                "AssetRegisterPrint");
        }

        private void LoadReport()
        {
            _reportData = _generator.Build();
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

            string[] moneyColumns =
            {
                "Cost", "Residual Value", "Dep. Rate %", "Accum. Depreciation", "Current WDV"
            };

            foreach (string col in moneyColumns)
            {
                if (dgvReport.Columns.Contains(col))
                {
                    dgvReport.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvReport.Columns[col].DefaultCellStyle.Format = "N2";
                }
            }

            if (dgvReport.Columns.Contains("Verified By"))
            {
                dgvReport.Columns["Verified By"].Width = 120;
            }

            if (dgvReport.Columns.Contains("Verification Notes"))
            {
                dgvReport.Columns["Verification Notes"].Width = 180;
            }
        }
    }
}
