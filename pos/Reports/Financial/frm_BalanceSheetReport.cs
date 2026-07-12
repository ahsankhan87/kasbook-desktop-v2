using POS.BLL;
using System;
using System.Drawing;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos.Reports.Financial
{
    public partial class frm_BalanceSheetReport : Form
    {
        private readonly AccountsBLL _accountsBll = new AccountsBLL();
        private bool _isInitializing;

        public frm_BalanceSheetReport()
        {
            InitializeComponent();
            WireEvents();
            Text = "Balance Sheet";
        }

        private void WireEvents()
        {
            Load += Frm_BalanceSheetReport_Load;
            btnGenerate.Click += (s, e) => GenerateReport();
            btnFindDiscrepancy.Click += BtnFindDiscrepancy_Click;
            chkShowComparison.CheckedChanged += (s, e) =>
            {
                UpdateComparisonState();
                if (!_isInitializing)
                {
                    GenerateReport();
                }
            };
            dtpAsOfDate.ValueChanged += (s, e) =>
            {
                if (!_isInitializing)
                {
                    GenerateReport();
                }
            };
            dtpComparisonDate.ValueChanged += (s, e) =>
            {
                if (!_isInitializing && chkShowComparison.Checked)
                {
                    GenerateReport();
                }
            };
            cmbDetailLevel.SelectedIndexChanged += (s, e) =>
            {
                if (!_isInitializing)
                {
                    GenerateReport();
                }
            };
            dgvAssets.DataBindingComplete += Grid_DataBindingComplete;
            dgvLiabilitiesEquity.DataBindingComplete += Grid_DataBindingComplete;
        }

        private void Frm_BalanceSheetReport_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            _isInitializing = true;

            cmbDetailLevel.Items.Clear();
            cmbDetailLevel.Items.AddRange(new object[] { "Summarized", "Detailed" });
            cmbDetailLevel.SelectedIndex = 0;

            dtpAsOfDate.Value = DateTime.Today;
            dtpComparisonDate.Value = DateTime.Today.AddMonths(-1);
            chkShowComparison.Checked = false;

            ConfigureGrid(dgvAssets);
            ConfigureGrid(dgvLiabilitiesEquity);
            UpdateComparisonState();

            _isInitializing = false;
            GenerateReport();
        }

        private static void ConfigureGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = false;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            grid.DefaultCellStyle.SelectionBackColor = Color.AliceBlue;
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            grid.RowTemplate.Height = 24;
        }

        private void Grid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null)
            {
                return;
            }

            string amountColumn = grid.Columns.Contains("colAssetsAmount") ? "colAssetsAmount" : "colLiabAmount";
            string comparisonColumn = grid.Columns.Contains("colAssetsComparison") ? "colAssetsComparison" : "colLiabComparison";

            foreach (DataGridViewRow row in grid.Rows)
            {
                object amount = row.Cells[amountColumn].Value;
                if (amount == null || amount == DBNull.Value)
                {
                    row.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }
            }

            grid.Columns[amountColumn].DefaultCellStyle.Format = "N2";
            grid.Columns[amountColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            if (!string.IsNullOrEmpty(comparisonColumn))
            {
                grid.Columns[comparisonColumn].DefaultCellStyle.Format = "N2";
                grid.Columns[comparisonColumn].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void UpdateComparisonState()
        {
            dtpComparisonDate.Enabled = chkShowComparison.Checked;
        }

        private void GenerateReport()
        {
            if (_isInitializing)
            {
                return;
            }

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Generating balance sheet...", "Ã«—Ì ≈‰‘«¡ «·„Ì“«‰Ì… «·⁄„Ê„Ì…...")))
                {
                    BalanceSheetReportModel report = BalanceSheetGenerator.Build(
                        _accountsBll,
                        dtpAsOfDate.Value.Date,
                        chkShowComparison.Checked ? (DateTime?)dtpComparisonDate.Value.Date : null,
                        IsDetailedMode(),
                        POS.Core.UsersModal.logged_in_branch_id);

                    BindReport(report);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Error generating balance sheet", ex.Message);
            }
        }

        private bool IsDetailedMode()
        {
            return string.Equals(Convert.ToString(cmbDetailLevel.SelectedItem), "Detailed", StringComparison.OrdinalIgnoreCase);
        }

        private void BindReport(BalanceSheetReportModel report)
        {
            dgvAssets.DataSource = report.Assets;
            dgvLiabilitiesEquity.DataSource = report.LiabilitiesAndEquity;

            tvNotes.BeginUpdate();
            tvNotes.Nodes.Clear();
            foreach (BalanceNote note in report.Notes)
            {
                TreeNode major = tvNotes.Nodes.Add(note.Title + ": " + note.Summary);
                foreach (string detail in note.Details)
                {
                    major.Nodes.Add(detail);
                }
            }
            tvNotes.EndUpdate();

            lblBalanceStatus.ForeColor = report.IsBalanced ? Color.DarkGreen : Color.DarkRed;
            lblBalanceStatus.Text = report.IsBalanced
                ? "Assets = Liabilities + Equity ?"
                : string.Format("OUT OF BALANCE by PKR {0:N2} ?", Math.Abs(report.Difference));

            btnFindDiscrepancy.Enabled = !report.IsBalanced;
            btnFindDiscrepancy.Tag = report;
        }

        private void BtnFindDiscrepancy_Click(object sender, EventArgs e)
        {
            var report = btnFindDiscrepancy.Tag as BalanceSheetReportModel;
            if (report == null || report.IsBalanced)
            {
                UiMessages.ShowInfo("No discrepancy", "The report is already balanced.");
                return;
            }

            BalanceSheetDiscrepancyForm form = new BalanceSheetDiscrepancyForm(report);
            form.ShowDialog(this);
        }
    }
}
