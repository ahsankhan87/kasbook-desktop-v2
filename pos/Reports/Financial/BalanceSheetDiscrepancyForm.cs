using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using pos.UI;

namespace pos.Reports.Financial
{
    public partial class BalanceSheetDiscrepancyForm : Form
    {
        private readonly BalanceSheetReportModel _report;

        public BalanceSheetDiscrepancyForm(BalanceSheetReportModel report)
        {
            _report = report;
            InitializeComponent();
            Text = "Balance Sheet Discrepancy";
            Load += BalanceSheetDiscrepancyForm_Load;
        }

        private void BalanceSheetDiscrepancyForm_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            lblSummary.Text = _report.IsBalanced
                ? "No discrepancy detected."
                : string.Format("Difference: PKR {0:N2}", Math.Abs(_report.Difference));

            dgvDiagnostics.DataSource = _report.Diagnostics;
            if (dgvDiagnostics.Columns.Count > 0)
            {
                dgvDiagnostics.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvDiagnostics.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDiagnostics.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvDiagnostics.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
