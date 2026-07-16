using POS.BLL;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using pos.UI;
using pos.UI.Busy;
using pos;

namespace pos.Reports.Financial
{
    public partial class frm_BudgetVarianceDetail : Form
    {
        private readonly BudgetBLL _budgetBll = new BudgetBLL();

        private readonly int _budgetId;
        private readonly int _accId;
        private readonly string _accCode;
        private readonly string _accName;

        public frm_BudgetVarianceDetail(int budgetId, int accId, string accCode, string accName)
        {
            _budgetId = budgetId;
            _accId = accId;
            _accCode = accCode ?? string.Empty;
            _accName = accName ?? string.Empty;

            InitializeComponent();
            WireEvents();
        }

        private void WireEvents()
        {
            Load += Frm_BudgetVarianceDetail_Load;
            btnViewLedger.Click += BtnViewLedger_Click;
            btnClose.Click += (s, e) => Close();
        }

        private void Frm_BudgetVarianceDetail_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            lblTitle.Text = string.Format("Variance Detail: {0} - {1}", _accCode, _accName);
            ConfigureChart();
            LoadMonthlyData();
        }

        private void ConfigureChart()
        {
            chartMonthly.Series.Clear();
            chartMonthly.ChartAreas.Clear();
            chartMonthly.Legends.Clear();

            ChartArea area = new ChartArea("Main");
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -35;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chartMonthly.ChartAreas.Add(area);

            Legend legend = new Legend("Legend");
            legend.Docking = Docking.Top;
            chartMonthly.Legends.Add(legend);

            Series budgetSeries = new Series("Budget");
            budgetSeries.ChartType = SeriesChartType.Column;
            budgetSeries.Color = Color.Silver;
            budgetSeries.IsValueShownAsLabel = false;
            budgetSeries.ChartArea = "Main";

            Series actualSeries = new Series("Actual");
            actualSeries.ChartType = SeriesChartType.Column;
            actualSeries.Color = Color.SteelBlue;
            actualSeries.IsValueShownAsLabel = false;
            actualSeries.ChartArea = "Main";

            chartMonthly.Series.Add(budgetSeries);
            chartMonthly.Series.Add(actualSeries);
        }

        private void LoadMonthlyData()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading monthly variance detail...", "جاري تحميل تفاصيل الانحراف الشهرية...")))
                {
                    DataTable dt = _budgetBll.GetBudgetMonthlyDetail(_budgetId, _accId);
                    dgvMonthly.DataSource = dt;
                    FormatMonthlyGrid();
                    BindChart(dt);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void FormatMonthlyGrid()
        {
            if (dgvMonthly.Columns.Contains("MonthNo"))
                dgvMonthly.Columns["MonthNo"].Visible = false;

            foreach (DataGridViewColumn col in dgvMonthly.Columns)
            {
                string colName = col.Name.ToLowerInvariant();
                if (colName.Contains("budget") || colName.Contains("actual") || colName.Contains("variance"))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = "N2";
                }
            }

            foreach (DataGridViewRow row in dgvMonthly.Rows)
            {
                if (row.IsNewRow)
                    continue;

                decimal variance = ReadDecimal(row, "Variance");
                row.Cells["Variance"].Style.ForeColor = variance <= 0m ? Color.ForestGreen : Color.Firebrick;
            }
        }

        private void BindChart(DataTable dt)
        {
            chartMonthly.Series["Budget"].Points.Clear();
            chartMonthly.Series["Actual"].Points.Clear();

            if (dt == null)
                return;

            foreach (DataRow row in dt.Rows)
            {
                string monthName = Convert.ToString(row["MonthName"]);
                decimal budget = ReadDecimal(row, "BudgetAmount");
                decimal actual = ReadDecimal(row, "ActualAmount");

                chartMonthly.Series["Budget"].Points.AddXY(monthName, budget);
                chartMonthly.Series["Actual"].Points.AddXY(monthName, actual);
            }
        }

        private decimal ReadDecimal(DataGridViewRow row, string columnName)
        {
            if (!row.DataGridView.Columns.Contains(columnName))
                return 0m;

            object value = row.Cells[columnName].Value;
            if (value == null || value == DBNull.Value)
                return 0m;

            decimal result;
            return decimal.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                ? result
                : 0m;
        }

        private decimal ReadDecimal(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return 0m;

            decimal result;
            return decimal.TryParse(Convert.ToString(row[columnName], CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                ? result
                : 0m;
        }

        private void BtnViewLedger_Click(object sender, EventArgs e)
        {
            try
            {
                using (var frm = new frm_account_report())
                {
                    frm.LoadForAccount(_accId);
                    frm.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowWarning(ex.Message, ex.Message);
            }
        }
    }
}
