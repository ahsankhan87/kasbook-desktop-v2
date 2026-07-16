using DGVPrinterHelper;
using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using pos.Reports.Common;
using pos.UI;
using pos.UI.Busy;

namespace pos.Reports.Financial
{
    public partial class frm_BudgetVsActualReport : Form
    {
        private readonly BudgetBLL _budgetBll = new BudgetBLL();
        private readonly FiscalYearBLL _fiscalYearBll = new FiscalYearBLL();
        private readonly CostCenterBLL _costCenterBll = new CostCenterBLL();
        private readonly BudgetVsActualReportGenerator _reportGenerator = new BudgetVsActualReportGenerator();

        private DataTable _budgets;
        private DataTable _reportData;
        private const decimal DefaultVarianceThresholdPct = 15m;

        public frm_BudgetVsActualReport()
        {
            InitializeComponent();
            WireEvents();
        }

        private void WireEvents()
        {
            Load += Frm_BudgetVsActualReport_Load;
            cmbFiscalYear.SelectedIndexChanged += CmbFiscalYear_SelectedIndexChanged;
            btnGenerate.Click += BtnGenerate_Click;
            btnPrint.Click += BtnPrint_Click;
            btnExportExcel.Click += BtnExportExcel_Click;
            dgvReport.CellDoubleClick += DgvReport_CellDoubleClick;
            dgvReport.CellContentClick += DgvReport_CellContentClick;
            dgvReport.CellPainting += DgvReport_CellPainting;
            dgvReport.RowPrePaint += DgvReport_RowPrePaint;
        }

        private void Frm_BudgetVsActualReport_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            LoadFilters();
            ConfigureGrid();
        }

        private void LoadFilters()
        {
            LoadFiscalYears();
            LoadCostCenters();
            LoadPeriodAndTypeCombos();
            LoadBudgetVersions();
        }

        private void LoadFiscalYears()
        {
            DataTable fy = _fiscalYearBll.GetAll() ?? new DataTable();
            DataTable view = new DataTable();
            view.Columns.Add("id", typeof(int));
            view.Columns.Add("name", typeof(string));

            foreach (DataRow row in fy.Rows)
            {
                if (!row.Table.Columns.Contains("id"))
                    continue;

                string name = row.Table.Columns.Contains("name") ? Convert.ToString(row["name"]) : string.Empty;
                if (string.IsNullOrWhiteSpace(name))
                {
                    DateTime from = row.Table.Columns.Contains("from_date") && row["from_date"] != DBNull.Value ? Convert.ToDateTime(row["from_date"]) : DateTime.MinValue;
                    DateTime to = row.Table.Columns.Contains("to_date") && row["to_date"] != DBNull.Value ? Convert.ToDateTime(row["to_date"]) : DateTime.MinValue;
                    name = from != DateTime.MinValue && to != DateTime.MinValue
                        ? string.Format("{0:yyyy} - {1:yyyy}", from, to)
                        : "Year " + Convert.ToString(row["id"]);
                }

                view.Rows.Add(Convert.ToInt32(row["id"]), name);
            }

            cmbFiscalYear.DataSource = view;
            cmbFiscalYear.DisplayMember = "name";
            cmbFiscalYear.ValueMember = "id";
        }

        private void LoadCostCenters()
        {
            DataTable source = _costCenterBll.GetCostCenterDropdown();
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("display_text", typeof(string));

            dt.Rows.Add(DBNull.Value, "All Cost Centers");
            if (source != null)
            {
                foreach (DataRow row in source.Rows)
                {
                    dt.Rows.Add(row["id"], Convert.ToString(row["display_text"]));
                }
            }

            cmbCostCenter.DataSource = dt;
            cmbCostCenter.DisplayMember = "display_text";
            cmbCostCenter.ValueMember = "id";
            cmbCostCenter.SelectedIndex = 0;
        }

        private void LoadPeriodAndTypeCombos()
        {
            cmbPeriod.Items.Clear();
            cmbPeriod.Items.AddRange(new object[] { "YTD", "MONTH", "QUARTER" });
            cmbPeriod.SelectedIndex = 0;

            cmbAccountType.Items.Clear();
            cmbAccountType.Items.AddRange(new object[] { "All", "Income", "Expense" });
            cmbAccountType.SelectedIndex = 0;
        }

        private void CmbFiscalYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBudgetVersions();
        }

        private void LoadBudgetVersions()
        {
            _budgets = _budgetBll.GetAllBudgetHeaders() ?? new DataTable();

            DataTable dt = _budgets.Clone();
            if (!_budgets.Columns.Contains("budget_id") || !_budgets.Columns.Contains("budget_version"))
            {
                cmbBudgetVersion.DataSource = dt;
                cmbBudgetVersion.DisplayMember = "budget_version";
                cmbBudgetVersion.ValueMember = "budget_id";
                cmbBudgetVersion.SelectedIndex = -1;
                return;
            }

            int fiscalYearId;
            bool hasSelectedYear = TryGetSelectedFiscalYearId(out fiscalYearId);

            var rows = _budgets.AsEnumerable()
                .Where(r => !hasSelectedYear || (r.Table.Columns.Contains("financial_year_id") && Convert.ToInt32(r["financial_year_id"]) == fiscalYearId))
                .OrderByDescending(r => ReadRowDate(r, "created_at"))
                .ToArray();

            foreach (var row in rows)
            {
                dt.ImportRow(row);
            }

            cmbBudgetVersion.DataSource = dt;
            cmbBudgetVersion.DisplayMember = "budget_version";
            cmbBudgetVersion.ValueMember = "budget_id";

            if (dt.Rows.Count > 0)
                cmbBudgetVersion.SelectedIndex = 0;
            else
                cmbBudgetVersion.SelectedIndex = -1;
        }

        private bool TryGetSelectedFiscalYearId(out int fiscalYearId)
        {
            fiscalYearId = 0;
            if (cmbFiscalYear.SelectedValue == null || cmbFiscalYear.SelectedValue == DBNull.Value)
                return false;

            if (cmbFiscalYear.SelectedValue is int)
            {
                fiscalYearId = (int)cmbFiscalYear.SelectedValue;
                return true;
            }

            return int.TryParse(Convert.ToString(cmbFiscalYear.SelectedValue), out fiscalYearId);
        }

        private DateTime ReadRowDate(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return DateTime.MinValue;

            DateTime value;
            return DateTime.TryParse(Convert.ToString(row[columnName]), out value) ? value : DateTime.MinValue;
        }

        private void ConfigureGrid()
        {
            dgvReport.AutoGenerateColumns = false;
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.RowHeadersVisible = false;
            dgvReport.MultiSelect = false;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            dgvReport.Columns.Clear();

            DataGridViewTextBoxColumn accIdCol = CreateTextColumn("acc_id", "Account Id", 2);
            accIdCol.Visible = false;
            dgvReport.Columns.Add(accIdCol);

            dgvReport.Columns.Add(CreateTextColumn("acc_code", "Account Code", 100));
            dgvReport.Columns.Add(CreateTextColumn("acc_name", "Account Name", 220));
            dgvReport.Columns.Add(CreateMoneyColumn("annual_budget", "Annual Budget"));
            dgvReport.Columns.Add(CreateMoneyColumn("ytd_budget", "YTD Budget"));
            dgvReport.Columns.Add(CreateMoneyColumn("ytd_actual", "YTD Actual"));
            dgvReport.Columns.Add(CreateMoneyColumn("ytd_variance", "YTD Variance"));
            dgvReport.Columns.Add(CreatePercentColumn("ytd_variance_pct", "YTD Variance %"));
            dgvReport.Columns.Add(CreateMoneyColumn("monthly_budget", "Monthly Budget"));
            dgvReport.Columns.Add(CreateMoneyColumn("monthly_actual", "Monthly Actual"));
            dgvReport.Columns.Add(CreateMoneyColumn("monthly_variance", "Monthly Variance"));
            dgvReport.Columns.Add(CreateMoneyColumn("full_year_forecast", "Full Year Forecast"));

            var barCol = CreatePercentColumn("variance_bar_pct", "Variance Bar");
            barCol.Width = 120;
            dgvReport.Columns.Add(barCol);

            DataGridViewButtonColumn notesCol = new DataGridViewButtonColumn();
            notesCol.Name = "notes_action";
            notesCol.HeaderText = "Notes";
            notesCol.Text = "📝 Note";
            notesCol.UseColumnTextForButtonValue = true;
            notesCol.Width = 80;
            dgvReport.Columns.Add(notesCol);
        }

        private DataGridViewTextBoxColumn CreateTextColumn(string name, string header, int width)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = name,
                ReadOnly = true,
                Width = width
            };
        }

        private DataGridViewTextBoxColumn CreateMoneyColumn(string name, string header)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = name,
                ReadOnly = true,
                Width = 115,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N2"
                }
            };
        }

        private DataGridViewTextBoxColumn CreatePercentColumn(string name, string header)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = name,
                ReadOnly = true,
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N2"
                }
            };
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbBudgetVersion.SelectedValue == null)
            {
                UiMessages.ShowWarning("Please select a budget version.", "يرجى تحديد نسخة الموازنة.");
                return;
            }

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Generating budget vs actual report...", "جاري إنشاء تقرير الموازنة مقابل الفعلي...")))
                {
                    int budgetId = Convert.ToInt32(cmbBudgetVersion.SelectedValue);
                    int? ccId = cmbCostCenter.SelectedValue == null || cmbCostCenter.SelectedValue == DBNull.Value
                        ? (int?)null
                        : Convert.ToInt32(cmbCostCenter.SelectedValue);

                    string periodMode = Convert.ToString(cmbPeriod.SelectedItem);
                    string accountType = Convert.ToString(cmbAccountType.SelectedItem);

                    DataTable raw = _budgetBll.GetBudgetVsActual(
                        budgetId,
                        DateTime.Today.AddMonths(-1),
                        DateTime.Today,
                        ccId,
                        periodMode,
                        accountType);

                    _reportData = _reportGenerator.BuildReportTable(raw, nudVarianceThreshold.Value <= 0 ? DefaultVarianceThresholdPct : nudVarianceThreshold.Value);
                    dgvReport.DataSource = _reportData;
                    ApplyVisualIndicators();
                    LoadSummaryCards(raw);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void LoadSummaryCards(DataTable raw)
        {
            BudgetVsActualReportSummary summary = _reportGenerator.BuildSummary(raw);

            lblIncomeValue.Text = string.Format("B: {0:N2} | A: {1:N2}", summary.TotalIncomeBudget, summary.TotalIncomeActual);
            lblExpenseValue.Text = string.Format("B: {0:N2} | A: {1:N2}", summary.TotalExpenseBudget, summary.TotalExpenseActual);
            lblNetValue.Text = string.Format("B: {0:N2} | A: {1:N2}", summary.NetProfitBudget, summary.NetProfitActual);
            lblAchievementValue.Text = summary.OverallAchievementPct.ToString("N2") + "%";

            if (summary.HealthStatus == "Green")
            {
                lblHealth.Text = "● Green";
                lblHealth.ForeColor = Color.ForestGreen;
            }
            else if (summary.HealthStatus == "Red")
            {
                lblHealth.Text = "● Red";
                lblHealth.ForeColor = Color.Firebrick;
            }
            else
            {
                lblHealth.Text = "● Amber";
                lblHealth.ForeColor = Color.DarkOrange;
            }
        }

        private void ApplyVisualIndicators()
        {
            if (_reportData == null)
                return;

            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (row.IsNewRow)
                    continue;

                decimal pct = ReadDecimal(row, "ytd_variance_pct");
                bool highVariance = ReadBool(row, "is_high_variance");
                bool favorable = ReadBool(row, "is_favorable");

                Color varianceColor;
                decimal abs = Math.Abs(pct);
                if (abs > 20m)
                    varianceColor = Color.DarkRed;
                else if (abs > 10m)
                    varianceColor = Color.IndianRed;
                else if (abs > 0m)
                    varianceColor = Color.DarkOrange;
                else
                    varianceColor = Color.ForestGreen;

                row.Cells["ytd_variance_pct"].Style.ForeColor = varianceColor;
                row.Cells["ytd_variance"].Style.ForeColor = favorable ? Color.ForestGreen : Color.Firebrick;
                row.Cells["monthly_variance"].Style.ForeColor = favorable ? Color.ForestGreen : Color.Firebrick;

                if (highVariance)
                {
                    row.DefaultCellStyle.Font = new Font(dgvReport.Font, FontStyle.Bold);
                }
            }
        }

        private void DgvReport_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvReport.Rows.Count)
                return;

            DataGridViewRow row = dgvReport.Rows[e.RowIndex];
            if (row.IsNewRow)
                return;

            bool highVariance = ReadBool(row, "is_high_variance");
            if (highVariance)
            {
                row.DefaultCellStyle.Font = new Font(dgvReport.Font, FontStyle.Bold);
            }
        }

        private void DgvReport_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvReport.Columns[e.ColumnIndex].Name != "variance_bar_pct")
                return;

            e.PaintBackground(e.CellBounds, true);
            e.PaintContent(e.CellBounds);

            decimal pct = 0m;
            object value = dgvReport.Rows[e.RowIndex].Cells["variance_bar_pct"].Value;
            if (value != null && value != DBNull.Value)
            {
                decimal.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out pct);
            }

            int barWidth = (int)((Math.Min(100m, Math.Max(0m, pct)) / 100m) * (e.CellBounds.Width - 10));
            Rectangle barRect = new Rectangle(e.CellBounds.X + 4, e.CellBounds.Y + 6, barWidth, e.CellBounds.Height - 12);

            using (Brush b = new SolidBrush(Color.SteelBlue))
            {
                e.Graphics.FillRectangle(b, barRect);
            }

            using (Pen p = new Pen(Color.LightGray))
            {
                e.Graphics.DrawRectangle(p, e.CellBounds.X + 4, e.CellBounds.Y + 6, e.CellBounds.Width - 10, e.CellBounds.Height - 12);
            }

            e.Handled = true;
        }

        private void DgvReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || cmbBudgetVersion.SelectedValue == null)
                return;

            OpenDrillDown(dgvReport.Rows[e.RowIndex]);
        }

        private void DgvReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvReport.Columns[e.ColumnIndex].Name == "notes_action")
            {
                SaveVarianceNote(dgvReport.Rows[e.RowIndex]);
                return;
            }

            if (dgvReport.Columns[e.ColumnIndex].Name == "acc_code" || dgvReport.Columns[e.ColumnIndex].Name == "acc_name")
            {
                OpenDrillDown(dgvReport.Rows[e.RowIndex]);
            }
        }

        private void OpenDrillDown(DataGridViewRow row)
        {
            if (cmbBudgetVersion.SelectedValue == null)
                return;

            int budgetId = Convert.ToInt32(cmbBudgetVersion.SelectedValue);
            int accId = Convert.ToInt32(row.Cells["acc_id"].Value);
            string accCode = Convert.ToString(row.Cells["acc_code"].Value);
            string accName = Convert.ToString(row.Cells["acc_name"].Value);

            using (var frm = new frm_BudgetVarianceDetail(budgetId, accId, accCode, accName))
            {
                frm.ShowDialog(this);
            }
        }

        private void SaveVarianceNote(DataGridViewRow row)
        {
            if (cmbBudgetVersion.SelectedValue == null)
                return;

            int budgetId = Convert.ToInt32(cmbBudgetVersion.SelectedValue);
            int accId = Convert.ToInt32(row.Cells["acc_id"].Value);
            int periodMonth = ReadInt(row, "period_month");
            int periodYear = ReadInt(row, "period_year");

            string existing = string.Empty;
            DataTable notes = _budgetBll.GetVarianceNotes(budgetId, accId);
            if (notes.Rows.Count > 0)
                existing = Convert.ToString(notes.Rows[0]["variance_note"]);

            string prompt = UiMessages.T("Enter variance explanation note:", "أدخل ملاحظة تفسير الانحراف:");
            string note = ShowNoteInputDialog(prompt, existing ?? string.Empty);
            if (string.IsNullOrWhiteSpace(note))
                return;

            try
            {
                _budgetBll.AddVarianceNote(budgetId, accId, periodMonth, periodYear, note.Trim(), UsersModal.logged_in_user_id);
                UiMessages.ShowInfo("Variance note saved.", "تم حفظ ملاحظة الانحراف.");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (_reportData == null || _reportData.Rows.Count == 0)
            {
                UiMessages.ShowWarning("Nothing to print.", "لا توجد بيانات للطباعة.");
                return;
            }

            DGVPrinter printer = new DGVPrinter();
            printer.Title = UiMessages.T("Budget vs Actual Report", "تقرير الموازنة مقابل الفعلي");
            printer.SubTitle = string.Format("{0} | {1}",
                cmbBudgetVersion.Text,
                DateTime.Today.ToString("yyyy-MM-dd"));
            printer.SubTitleFormatFlags = System.Drawing.StringFormatFlags.LineLimit | System.Drawing.StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = false;
            printer.HeaderCellAlignment = StringAlignment.Center;
            printer.Footer = UsersModal.logged_in_company_name;
            printer.FooterSpacing = 15;
            printer.PrintPreviewDataGridView(dgvReport);
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            if (_reportData == null || _reportData.Rows.Count == 0)
            {
                UiMessages.ShowWarning("No data to export.", "لا توجد بيانات للتصدير.");
                return;
            }

            DataTable export = _reportGenerator.BuildExportTable(_reportData);
            ExcelExportHelper.ExportDataTableToExcel(export, "BudgetVsActualReport", this, true);
        }

        private string ShowNoteInputDialog(string prompt, string defaultValue)
        {
            using (Form dialog = new Form())
            using (Label lbl = new Label())
            using (TextBox txt = new TextBox())
            using (Button btnOk = new Button())
            using (Button btnCancel = new Button())
            {
                dialog.Text = UiMessages.T("Variance Note", "ملاحظة الانحراف");
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MinimizeBox = false;
                dialog.MaximizeBox = false;
                dialog.ShowInTaskbar = false;
                dialog.ClientSize = new Size(520, 230);

                lbl.AutoSize = false;
                lbl.Text = prompt;
                lbl.Location = new Point(12, 12);
                lbl.Size = new Size(496, 32);

                txt.Multiline = true;
                txt.ScrollBars = ScrollBars.Vertical;
                txt.Location = new Point(12, 48);
                txt.Size = new Size(496, 130);
                txt.Text = defaultValue ?? string.Empty;

                btnOk.Text = UiMessages.T("Save", "حفظ");
                btnOk.DialogResult = DialogResult.OK;
                btnOk.Location = new Point(352, 188);
                btnOk.Size = new Size(75, 30);

                btnCancel.Text = UiMessages.T("Cancel", "إلغاء");
                btnCancel.DialogResult = DialogResult.Cancel;
                btnCancel.Location = new Point(433, 188);
                btnCancel.Size = new Size(75, 30);

                dialog.Controls.Add(lbl);
                dialog.Controls.Add(txt);
                dialog.Controls.Add(btnOk);
                dialog.Controls.Add(btnCancel);
                dialog.AcceptButton = btnOk;
                dialog.CancelButton = btnCancel;

                AppTheme.Apply(dialog);
                var result = dialog.ShowDialog(this);
                return result == DialogResult.OK ? (txt.Text ?? string.Empty).Trim() : string.Empty;
            }
        }

        private decimal ReadDecimal(DataGridViewRow row, string column)
        {
            if (!row.DataGridView.Columns.Contains(column))
                return 0m;

            object value = row.Cells[column].Value;
            if (value == null || value == DBNull.Value)
                return 0m;

            decimal result;
            return decimal.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                ? result
                : 0m;
        }

        private int ReadInt(DataGridViewRow row, string column)
        {
            if (!row.DataGridView.Columns.Contains(column))
                return 0;

            object value = row.Cells[column].Value;
            if (value == null || value == DBNull.Value)
                return 0;

            int result;
            return int.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out result)
                ? result
                : 0;
        }

        private bool ReadBool(DataGridViewRow row, string column)
        {
            if (!row.DataGridView.Columns.Contains(column))
                return false;

            object value = row.Cells[column].Value;
            if (value == null || value == DBNull.Value)
                return false;

            if (value is bool)
                return (bool)value;

            bool b;
            if (bool.TryParse(Convert.ToString(value), out b))
                return b;

            int i;
            if (int.TryParse(Convert.ToString(value), out i))
                return i != 0;

            return false;
        }
    }
}
