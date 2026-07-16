using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DGVPrinterHelper;
using POS.BLL;
using POS.Core;
using pos.UI;
using pos.UI.Busy;

namespace pos.Accounting.CostCenter
{
    public partial class frm_budget_setup : Form
    {
        private readonly BudgetBLL _budgetBll = new BudgetBLL();
        private readonly CostCenterBLL _costCenterBll = new CostCenterBLL();
        private readonly FiscalYearBLL _fiscalYearBll = new FiscalYearBLL();
        private readonly ChartOfAccountsBLL _coaBll = new ChartOfAccountsBLL();

        private readonly string[] _months = { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };

        private DataTable _fiscalYears;
        private DataTable _accounts;
        private int _currentBudgetId;
        private bool _isBinding;

        public frm_budget_setup()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            AppTheme.Apply(this);
            pnlComparison.Visible = false;

            SetupHeaderDropdowns();
            InitializeBudgetGrid();
            InitializeComparisonGrid();
            WireEvents();
            ApplyApprovalRestrictions();
        }

        private void SetupHeaderDropdowns()
        {
            cmbVersion.Items.Clear();
            cmbVersion.Items.AddRange(new object[] { "Original", "Revised Q1", "Revised Q2", "Revised Q3" });

            cmbStatus.Items.Clear();
            cmbStatus.Items.AddRange(new object[] { "Draft", "Approved", "Active" });
            cmbStatus.SelectedIndex = 0;
        }

        private void WireEvents()
        {
            btnNew.Click += BtnNew_Click;
            btnLoad.Click += BtnLoad_Click;
            btnSave.Click += BtnSave_Click;
            btnApprove.Click += BtnApprove_Click;
            btnSpreadEven.Click += BtnSpreadEven_Click;
            btnSeasonality.Click += BtnSeasonality_Click;
            btnCopyLastYear.Click += BtnCopyLastYear_Click;
            btnCopyGrowth.Click += BtnCopyGrowth_Click;
            btnExportExcel.Click += BtnExportExcel_Click;
            btnImportExcel.Click += BtnImportExcel_Click;
            btnPrint.Click += BtnPrint_Click;
            chkShowComparison.CheckedChanged += ChkShowComparison_CheckedChanged;

            dgvBudgets.CellEndEdit += DgvBudgets_CellEndEdit;
            dgvBudgets.DataError += DgvBudgets_DataError;
        }

        private void ApplyApprovalRestrictions()
        {
            string role = (UsersModal.logged_in_user_role ?? string.Empty).Trim().ToLowerInvariant();
            bool canApprove = role == "administrator" || role == "admin" || role == "owner" || role == "cfo" || role == "manager";
            btnApprove.Enabled = canApprove;
        }

        private void InitializeBudgetGrid()
        {
            dgvBudgets.AutoGenerateColumns = false;
            dgvBudgets.AllowUserToAddRows = false;
            dgvBudgets.AllowUserToDeleteRows = false;
            dgvBudgets.RowHeadersVisible = false;
            dgvBudgets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            dgvBudgets.Columns.Clear();

            dgvBudgets.Columns.Add(new DataGridViewTextBoxColumn { Name = "account_id", HeaderText = "AccId", Visible = false });
            dgvBudgets.Columns.Add(new DataGridViewTextBoxColumn { Name = "acc_code", HeaderText = "Account Code", ReadOnly = true, Width = 90 });
            dgvBudgets.Columns.Add(new DataGridViewTextBoxColumn { Name = "acc_name", HeaderText = "Account Name", ReadOnly = true, Width = 220 });
            dgvBudgets.Columns.Add(new DataGridViewTextBoxColumn { Name = "account_type", HeaderText = "Type", ReadOnly = true, Width = 90 });
            dgvBudgets.Columns.Add(CreateMoneyColumn("annual_budget", "Annual Budget", false));

            for (int i = 0; i < _months.Length; i++)
            {
                dgvBudgets.Columns.Add(CreateMoneyColumn(_months[i], CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[i], false));
            }

            dgvBudgets.Columns.Add(CreateMoneyColumn("total", "Total", true));
        }

        private void InitializeComparisonGrid()
        {
            dgvComparison.AutoGenerateColumns = true;
            dgvComparison.AllowUserToAddRows = false;
            dgvComparison.AllowUserToDeleteRows = false;
            dgvComparison.RowHeadersVisible = false;
            dgvComparison.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgvComparison.ReadOnly = true;
        }

        private DataGridViewTextBoxColumn CreateMoneyColumn(string name, string header, bool readOnly)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                ReadOnly = readOnly,
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N2",
                    BackColor = readOnly ? Color.FromArgb(245, 245, 245) : Color.White
                }
            };
        }

        private void FrmBudgetSetup_Load(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, "Loading budget planner..."))
                {
                    LoadFiscalYears();
                    LoadCostCenters();
                    LoadBudgetAccounts();
                    StartNewBudget();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Failed to initialize budget planner: " + ex.Message, "فشل تهيئة شاشة الموازنة: " + ex.Message);
            }
        }

        private void LoadFiscalYears()
        {
            _fiscalYears = _fiscalYearBll.GetAll() ?? new DataTable();
            if (!_fiscalYears.Columns.Contains("id"))
                return;

            DataTable displayTable = new DataTable();
            displayTable.Columns.Add("id", typeof(int));
            displayTable.Columns.Add("display", typeof(string));
            displayTable.Columns.Add("start_date", typeof(DateTime));

            foreach (DataRow row in _fiscalYears.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                DateTime start = ReadDate(row, "from_date", "start_date");
                DateTime end = ReadDate(row, "to_date", "end_date");
                string title = ReadString(row, "fiscal_year", "name", "code");
                if (string.IsNullOrWhiteSpace(title))
                    title = start != DateTime.MinValue && end != DateTime.MinValue ? string.Format("{0:yyyy} - {1:yyyy}", start, end) : "Year " + id;

                displayTable.Rows.Add(id, title, start == DateTime.MinValue ? DateTime.Today : start);
            }

            DataView view = displayTable.DefaultView;
            view.Sort = "start_date DESC";

            cmbYear.DataSource = view.ToTable();
            cmbYear.DisplayMember = "display";
            cmbYear.ValueMember = "id";
            if (cmbYear.Items.Count > 0)
                cmbYear.SelectedIndex = 0;
        }

        private void LoadCostCenters()
        {
            DataTable source = _costCenterBll.GetCostCenterDropdown();
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("display_text", typeof(string));

            DataRow allRow = dt.NewRow();
            allRow["id"] = DBNull.Value;
            allRow["display_text"] = "All Company";
            dt.Rows.Add(allRow);

            if (source != null)
            {
                foreach (DataRow row in source.Rows)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["id"] = row["id"];
                    newRow["display_text"] = Convert.ToString(row["display_text"]);
                    dt.Rows.Add(newRow);
                }
            }

            cmbCostCenter.DataSource = dt;
            cmbCostCenter.DisplayMember = "display_text";
            cmbCostCenter.ValueMember = "id";
            cmbCostCenter.SelectedIndex = 0;
        }

        private void LoadBudgetAccounts()
        {
            DataTable income = _coaBll.GetAccountsByType("income");
            DataTable revenue = _coaBll.GetAccountsByType("revenue");
            DataTable expense = _coaBll.GetAccountsByType("expense");

            _accounts = new DataTable();
            _accounts.Columns.Add("id", typeof(int));
            _accounts.Columns.Add("code", typeof(string));
            _accounts.Columns.Add("name", typeof(string));
            _accounts.Columns.Add("account_type", typeof(string));

            var seen = new HashSet<int>();
            AddAccountRows(_accounts, income, seen);
            AddAccountRows(_accounts, revenue, seen);
            AddAccountRows(_accounts, expense, seen);

            DataView view = _accounts.DefaultView;
            view.Sort = "code ASC";
            _accounts = view.ToTable();
        }

        private void AddAccountRows(DataTable target, DataTable source, HashSet<int> seen)
        {
            if (source == null)
                return;

            foreach (DataRow row in source.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                if (!seen.Add(id))
                    continue;

                DataRow newRow = target.NewRow();
                newRow["id"] = id;
                newRow["code"] = Convert.ToString(row["code"]);
                newRow["name"] = Convert.ToString(row["name"]);
                newRow["account_type"] = Convert.ToString(row.Table.Columns.Contains("account_type") ? row["account_type"] : string.Empty);
                target.Rows.Add(newRow);
            }
        }

        private DateTime ReadDate(DataRow row, string firstColumn, string secondColumn)
        {
            if (row.Table.Columns.Contains(firstColumn) && row[firstColumn] != DBNull.Value)
                return Convert.ToDateTime(row[firstColumn]);
            if (row.Table.Columns.Contains(secondColumn) && row[secondColumn] != DBNull.Value)
                return Convert.ToDateTime(row[secondColumn]);
            return DateTime.MinValue;
        }

        private string ReadString(DataRow row, params string[] columns)
        {
            foreach (string column in columns)
            {
                if (row.Table.Columns.Contains(column) && row[column] != DBNull.Value)
                    return Convert.ToString(row[column]);
            }
            return string.Empty;
        }

        private void StartNewBudget()
        {
            _currentBudgetId = 0;
            txtNotes.Clear();
            cmbStatus.SelectedItem = "Draft";
            cmbVersion.SelectedIndex = 0;
            BindGridFromBudgetLines(null);
            lblStatus.Text = "Ready for new budget entry.";
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            StartNewBudget();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (!TryGetHeaderSelection(out int yearId, out int? ccId, out string version))
                return;

            try
            {
                using (BusyScope.Show(this, "Loading budget..."))
                {
                    DataRow existing = FindBudgetHeader(yearId, ccId, version);
                    if (existing == null)
                    {
                        _currentBudgetId = 0;
                        cmbStatus.SelectedItem = "Draft";
                        txtNotes.Clear();
                        BindGridFromBudgetLines(null);
                        lblStatus.Text = "No saved budget for selected header. Enter and save a new one.";
                    }
                    else
                    {
                        _currentBudgetId = Convert.ToInt32(existing["budget_id"]);
                        txtNotes.Text = Convert.ToString(existing["notes"]);
                        SetStatusSelection(Convert.ToString(existing["status"]));

                        DataTable lines = _budgetBll.GetBudgetLines(_currentBudgetId);
                        BindGridFromBudgetLines(lines);
                        lblStatus.Text = "Budget loaded successfully.";
                    }

                    if (chkShowComparison.Checked)
                        LoadComparison();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Error loading budget: " + ex.Message, "خطأ في تحميل الموازنة: " + ex.Message);
            }
        }

        private DataRow FindBudgetHeader(int yearId, int? ccId, string version)
        {
            DataTable headers = _budgetBll.GetAllBudgetHeaders();
            if (headers == null || headers.Rows.Count == 0)
                return null;

            IEnumerable<DataRow> rows = headers.AsEnumerable().Where(r => Convert.ToInt32(r["financial_year_id"]) == yearId);

            rows = rows.Where(r => string.Equals(Convert.ToString(r["budget_version"]), version, StringComparison.OrdinalIgnoreCase));

            if (ccId.HasValue)
                rows = rows.Where(r => r["cc_id"] != DBNull.Value && Convert.ToInt32(r["cc_id"]) == ccId.Value);
            else
                rows = rows.Where(r => r["cc_id"] == DBNull.Value);

            return rows.OrderByDescending(r => Convert.ToInt32(r["budget_id"]))
                       .FirstOrDefault();
        }

        private bool TryGetHeaderSelection(out int yearId, out int? ccId, out string version)
        {
            yearId = 0;
            ccId = null;
            version = Convert.ToString(cmbVersion.SelectedItem);

            if (cmbYear.SelectedValue == null || !int.TryParse(cmbYear.SelectedValue.ToString(), out yearId))
            {
                UiMessages.ShowWarning("Please select a financial year.", "يرجى اختيار السنة المالية.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                UiMessages.ShowWarning("Please select a budget version.", "يرجى اختيار نسخة الموازنة.");
                return false;
            }

            if (cmbCostCenter.SelectedValue != null && cmbCostCenter.SelectedValue != DBNull.Value)
            {
                if (int.TryParse(cmbCostCenter.SelectedValue.ToString(), out int parsedCc))
                    ccId = parsedCc;
            }

            return true;
        }

        private void BindGridFromBudgetLines(DataTable lines)
        {
            _isBinding = true;
            dgvBudgets.Rows.Clear();

            Dictionary<int, DataRow> lineMap = new Dictionary<int, DataRow>();
            if (lines != null)
            {
                foreach (DataRow row in lines.Rows)
                {
                    int accId = row.Table.Columns.Contains("account_id") ? Convert.ToInt32(row["account_id"]) : Convert.ToInt32(row["acc_id"]);
                    lineMap[accId] = row;
                }
            }

            foreach (DataRow account in _accounts.Rows)
            {
                int rowIndex = dgvBudgets.Rows.Add();
                DataGridViewRow row = dgvBudgets.Rows[rowIndex];
                int accId = Convert.ToInt32(account["id"]);

                row.Cells["account_id"].Value = accId;
                row.Cells["acc_code"].Value = Convert.ToString(account["code"]);
                row.Cells["acc_name"].Value = Convert.ToString(account["name"]);
                row.Cells["account_type"].Value = NormalizeType(Convert.ToString(account["account_type"]));

                for (int i = 0; i < _months.Length; i++)
                {
                    decimal monthValue = 0m;
                    if (lineMap.ContainsKey(accId) && lineMap[accId].Table.Columns.Contains(_months[i]))
                        monthValue = Convert.ToDecimal(lineMap[accId][_months[i]]);

                    row.Cells[_months[i]].Value = monthValue;
                }

                RecalculateRow(row, false);
                ApplyRowColor(row);
            }

            UpdateTotalsStrip();
            _isBinding = false;
        }

        private string NormalizeType(string accountType)
        {
            string text = (accountType ?? string.Empty).Trim();
            if (text.IndexOf("revenue", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Income";
            if (text.IndexOf("income", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Income";
            if (text.IndexOf("expense", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Expense";
            return text;
        }

        private void ApplyRowColor(DataGridViewRow row)
        {
            string type = Convert.ToString(row.Cells["account_type"].Value);
            if (string.Equals(type, "Income", StringComparison.OrdinalIgnoreCase))
                row.DefaultCellStyle.BackColor = Color.FromArgb(235, 249, 235);
            else if (string.Equals(type, "Expense", StringComparison.OrdinalIgnoreCase))
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 246, 227);
            else
                row.DefaultCellStyle.BackColor = Color.White;
        }

        private void DgvBudgets_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_isBinding || e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvBudgets.Rows[e.RowIndex];
            string columnName = dgvBudgets.Columns[e.ColumnIndex].Name;

            if (string.Equals(columnName, "annual_budget", StringComparison.OrdinalIgnoreCase))
            {
                decimal annual = GetCellDecimal(row, "annual_budget");
                row.Cells["annual_budget"].Value = annual;
            }
            else
            {
                decimal sum = SumMonths(row);
                row.Cells["annual_budget"].Value = sum;
            }

            RecalculateRow(row, true);
            UpdateTotalsStrip();

            if (chkShowComparison.Checked)
                LoadComparison();
        }

        private void DgvBudgets_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            UiMessages.ShowWarning("Please enter a valid numeric amount.", "يرجى إدخال مبلغ رقمي صحيح.");
        }

        private void RecalculateRow(DataGridViewRow row, bool alignAnnualToMonths)
        {
            decimal sum = SumMonths(row);
            row.Cells["total"].Value = sum;
            if (alignAnnualToMonths)
                row.Cells["annual_budget"].Value = sum;
        }

        private decimal SumMonths(DataGridViewRow row)
        {
            decimal total = 0m;
            for (int i = 0; i < _months.Length; i++)
            {
                total += GetCellDecimal(row, _months[i]);
            }

            return total;
        }

        private decimal GetCellDecimal(DataGridViewRow row, string columnName)
        {
            object value = row.Cells[columnName].Value;
            if (value == null || value == DBNull.Value)
                return 0m;

            decimal result;
            return decimal.TryParse(Convert.ToString(value), NumberStyles.Any, CultureInfo.CurrentCulture, out result)
                ? result
                : 0m;
        }

        private void UpdateTotalsStrip()
        {
            decimal totalIncome = 0m;
            decimal totalExpense = 0m;

            foreach (DataGridViewRow row in dgvBudgets.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string type = Convert.ToString(row.Cells["account_type"].Value);
                decimal annual = GetCellDecimal(row, "total");

                if (string.Equals(type, "Income", StringComparison.OrdinalIgnoreCase))
                    totalIncome += annual;
                else if (string.Equals(type, "Expense", StringComparison.OrdinalIgnoreCase))
                    totalExpense += annual;
            }

            decimal net = totalIncome - totalExpense;
            lblIncomeValue.Text = totalIncome.ToString("N2");
            lblExpenseValue.Text = totalExpense.ToString("N2");
            lblNetProfitValue.Text = net.ToString("N2");
            lblNetProfitValue.ForeColor = net >= 0 ? Color.ForestGreen : Color.Firebrick;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!TryGetHeaderSelection(out int yearId, out int? ccId, out string version))
                return;

            try
            {
                using (BusyScope.Show(this, "Saving budget..."))
                {
                    int budgetId = EnsureHeaderSaved(yearId, ccId, version);
                    var lines = BuildBudgetLinesFromGrid(budgetId);
                    _budgetBll.SaveBudgetLines(budgetId, lines);
                    _currentBudgetId = budgetId;
                    lblStatus.Text = "Budget saved successfully.";

                    if (chkShowComparison.Checked)
                        LoadComparison();
                }

                UiMessages.ShowInfo("Budget saved successfully.", "تم حفظ الموازنة بنجاح.");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Error saving budget: " + ex.Message, "خطأ في حفظ الموازنة: " + ex.Message);
            }
        }

        private int EnsureHeaderSaved(int yearId, int? ccId, string version)
        {
            int currentId = _currentBudgetId;
            if (currentId == 0)
            {
                DataRow existing = FindBudgetHeader(yearId, ccId, version);
                if (existing != null)
                    currentId = Convert.ToInt32(existing["budget_id"]);
            }

            string costCenterLabel = Convert.ToString(cmbCostCenter.Text);
            string yearLabel = Convert.ToString(cmbYear.Text);

            BudgetHeaderModal header = new BudgetHeaderModal
            {
                budget_id = currentId,
                financial_year_id = yearId,
                budget_version = version,
                cc_id = ccId,
                budget_name = string.Format("{0} - {1} - {2}", yearLabel, version, string.IsNullOrWhiteSpace(costCenterLabel) ? "All Company" : costCenterLabel),
                status = Convert.ToString(cmbStatus.SelectedItem) ?? "Draft",
                notes = txtNotes.Text,
                created_by = UsersModal.logged_in_userid
            };

            return _budgetBll.SaveBudgetHeader(header);
        }

        private List<BudgetLineModal> BuildBudgetLinesFromGrid(int budgetId)
        {
            List<BudgetLineModal> lines = new List<BudgetLineModal>();

            foreach (DataGridViewRow row in dgvBudgets.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int accId = Convert.ToInt32(row.Cells["account_id"].Value);
                BudgetLineModal line = new BudgetLineModal
                {
                    budget_id = budgetId,
                    account_id = accId,
                    jan = GetCellDecimal(row, "jan"),
                    feb = GetCellDecimal(row, "feb"),
                    mar = GetCellDecimal(row, "mar"),
                    apr = GetCellDecimal(row, "apr"),
                    may = GetCellDecimal(row, "may"),
                    jun = GetCellDecimal(row, "jun"),
                    jul = GetCellDecimal(row, "jul"),
                    aug = GetCellDecimal(row, "aug"),
                    sep = GetCellDecimal(row, "sep"),
                    oct = GetCellDecimal(row, "oct"),
                    nov = GetCellDecimal(row, "nov"),
                    dec = GetCellDecimal(row, "dec")
                };

                lines.Add(line);
            }

            return lines;
        }

        private void BtnApprove_Click(object sender, EventArgs e)
        {
            if (_currentBudgetId <= 0)
            {
                UiMessages.ShowWarning("Save budget before approval.", "يرجى حفظ الموازنة قبل الاعتماد.");
                return;
            }

            try
            {
                _budgetBll.ApproveBudget(_currentBudgetId, UsersModal.logged_in_userid);
                cmbStatus.SelectedItem = "Approved";
                UiMessages.ShowInfo("Budget approved successfully.", "تم اعتماد الموازنة بنجاح.");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Approval failed: " + ex.Message, "فشل الاعتماد: " + ex.Message);
            }
        }

        private void BtnSpreadEven_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectedBudgetRow();
            if (row == null)
                return;

            decimal annual = GetCellDecimal(row, "annual_budget");
            if (annual <= 0)
            {
                UiMessages.ShowWarning("Enter Annual Budget first, then click Spread Evenly.", "أدخل الميزانية السنوية أولاً ثم اضغط التوزيع المتساوي.");
                return;
            }

            int accId = Convert.ToInt32(row.Cells["account_id"].Value);
            BudgetLineModal spread = BudgetHelper.DistributeAnnualEqually(accId, annual);

            row.Cells["jan"].Value = spread.jan;
            row.Cells["feb"].Value = spread.feb;
            row.Cells["mar"].Value = spread.mar;
            row.Cells["apr"].Value = spread.apr;
            row.Cells["may"].Value = spread.may;
            row.Cells["jun"].Value = spread.jun;
            row.Cells["jul"].Value = spread.jul;
            row.Cells["aug"].Value = spread.aug;
            row.Cells["sep"].Value = spread.sep;
            row.Cells["oct"].Value = spread.oct;
            row.Cells["nov"].Value = spread.nov;
            row.Cells["dec"].Value = spread.dec;
            RecalculateRow(row, true);
            UpdateTotalsStrip();
        }

        private void BtnSeasonality_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = GetSelectedBudgetRow();
            if (row == null)
                return;

            decimal annual = GetCellDecimal(row, "annual_budget");
            if (annual <= 0)
                annual = GetCellDecimal(row, "total");

            if (annual <= 0)
            {
                UiMessages.ShowWarning("Enter annual amount before applying seasonal spread.", "أدخل المبلغ السنوي قبل تطبيق التوزيع الموسمي.");
                return;
            }

            using (var dlg = new SeasonalityDialog())
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                List<MonthlyPercentageModal> percentages = dlg.GetPercentages();
                int accId = Convert.ToInt32(row.Cells["account_id"].Value);

                try
                {
                    if (_currentBudgetId > 0)
                    {
                        _budgetBll.ApplySeasonalSpread(_currentBudgetId, accId, annual, percentages);
                        DataTable lines = _budgetBll.GetBudgetLines(_currentBudgetId);
                        BindGridFromBudgetLines(lines);
                    }
                    else
                    {
                        ApplySeasonalityLocally(row, annual, percentages);
                    }

                    UpdateTotalsStrip();
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError("Seasonal spread failed: " + ex.Message, "فشل التوزيع الموسمي: " + ex.Message);
                }
            }
        }

        private void ApplySeasonalityLocally(DataGridViewRow row, decimal annual, List<MonthlyPercentageModal> percentages)
        {
            decimal totalPct = percentages.Sum(p => p.Percentage);
            if (Math.Abs(totalPct - 100m) > 0.01m)
            {
                UiMessages.ShowWarning("Seasonal percentages must sum to 100%.", "يجب أن يكون مجموع النسب الموسمية 100٪.");
                return;
            }

            for (int i = 0; i < _months.Length; i++)
            {
                MonthlyPercentageModal pct = percentages.FirstOrDefault(p => p.MonthNo == i + 1);
                decimal amount = pct == null ? 0m : Math.Round(annual * pct.Percentage / 100m, 2);
                row.Cells[_months[i]].Value = amount;
            }

            decimal adjusted = annual - SumMonths(row);
            if (adjusted != 0m)
                row.Cells["dec"].Value = GetCellDecimal(row, "dec") + adjusted;

            RecalculateRow(row, true);
        }

        private void BtnCopyLastYear_Click(object sender, EventArgs e)
        {
            CopyFromLastYear(0m);
        }

        private void BtnCopyGrowth_Click(object sender, EventArgs e)
        {
            using (var dlg = new NumericPromptDialog("Growth %", "Enter growth percentage to apply", 0m))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

                CopyFromLastYear(dlg.Value);
            }
        }

        private void CopyFromLastYear(decimal growthPct)
        {
            if (!TryGetHeaderSelection(out int yearId, out int? ccId, out string version))
                return;

            int? sourceYearId = GetPreviousYearId(yearId);
            if (!sourceYearId.HasValue)
            {
                UiMessages.ShowWarning("No previous fiscal year found.", "لم يتم العثور على سنة مالية سابقة.");
                return;
            }

            try
            {
                using (BusyScope.Show(this, "Copying from last year actuals..."))
                {
                    int budgetId = EnsureHeaderSaved(yearId, ccId, version);
                    _budgetBll.CopyFromLastYear(sourceYearId.Value, budgetId, growthPct);
                    _currentBudgetId = budgetId;

                    DataTable lines = _budgetBll.GetBudgetLines(_currentBudgetId);
                    BindGridFromBudgetLines(lines);
                }

                UiMessages.ShowInfo("Copied from last year successfully.", "تم النسخ من السنة الماضية بنجاح.");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Copy failed: " + ex.Message, "فشل النسخ: " + ex.Message);
            }
        }

        private int? GetPreviousYearId(int yearId)
        {
            if (_fiscalYears == null || _fiscalYears.Rows.Count == 0)
                return null;

            DataRow current = _fiscalYears.AsEnumerable().FirstOrDefault(r => Convert.ToInt32(r["id"]) == yearId);
            if (current == null)
                return null;

            DateTime currentStart = ReadDate(current, "from_date", "start_date");
            if (currentStart == DateTime.MinValue)
                return null;

            DataRow previous = _fiscalYears.AsEnumerable()
                .Where(r => ReadDate(r, "from_date", "start_date") < currentStart)
                .OrderByDescending(r => ReadDate(r, "from_date", "start_date"))
                .FirstOrDefault();

            if (previous == null)
                return null;

            return Convert.ToInt32(previous["id"]);
        }

        private DataGridViewRow GetSelectedBudgetRow()
        {
            if (dgvBudgets.CurrentRow == null || dgvBudgets.CurrentRow.Index < 0)
            {
                UiMessages.ShowWarning("Please select an account row first.", "يرجى تحديد صف حساب أولاً.");
                return null;
            }

            return dgvBudgets.CurrentRow;
        }

        private void ChkShowComparison_CheckedChanged(object sender, EventArgs e)
        {
            pnlComparison.Visible = chkShowComparison.Checked;
            if (chkShowComparison.Checked)
                LoadComparison();
        }

        private void LoadComparison()
        {
            if (_currentBudgetId <= 0)
            {
                dgvComparison.DataSource = null;
                return;
            }

            try
            {
                DateTime fromDate = DateTime.Today.AddMonths(-1);
                DateTime toDate = DateTime.Today;
                int? ccId = cmbCostCenter.SelectedValue == null || cmbCostCenter.SelectedValue == DBNull.Value
                    ? (int?)null
                    : Convert.ToInt32(cmbCostCenter.SelectedValue);

                DataTable dt = _budgetBll.GetBudgetVsActual(_currentBudgetId, fromDate, toDate, ccId);
                dgvComparison.DataSource = dt;
                FormatComparisonGrid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Error loading budget comparison: " + ex.Message, "خطأ في تحميل مقارنة الموازنة: " + ex.Message);
            }
        }

        private void FormatComparisonGrid()
        {
            foreach (DataGridViewColumn col in dgvComparison.Columns)
            {
                string name = col.Name.ToLowerInvariant();
                if (name.Contains("budget") || name.Contains("actual") || name.Contains("variance") || name.Contains("forecast") || name.Contains("pct"))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DefaultCellStyle.Format = name.Contains("pct") ? "N2" : "N2";
                }
            }

            foreach (DataGridViewRow row in dgvComparison.Rows)
            {
                string accountType = Convert.ToString(row.Cells["account_type"].Value);
                decimal variance = ReadGridDecimal(row, "monthly_variance");
                bool favorable = IsFavorableVariance(accountType, variance);
                Color varianceColor = favorable ? Color.ForestGreen : Color.Firebrick;

                if (dgvComparison.Columns.Contains("monthly_variance"))
                    row.Cells["monthly_variance"].Style.ForeColor = varianceColor;
                if (dgvComparison.Columns.Contains("ytd_variance"))
                    row.Cells["ytd_variance"].Style.ForeColor = varianceColor;
                if (dgvComparison.Columns.Contains("ytd_variance_pct"))
                    row.Cells["ytd_variance_pct"].Style.ForeColor = varianceColor;
            }
        }

        private decimal ReadGridDecimal(DataGridViewRow row, string column)
        {
            if (!row.DataGridView.Columns.Contains(column))
                return 0m;

            object value = row.Cells[column].Value;
            if (value == null || value == DBNull.Value)
                return 0m;

            decimal result;
            return decimal.TryParse(Convert.ToString(value), NumberStyles.Any, CultureInfo.CurrentCulture, out result)
                ? result
                : 0m;
        }

        private bool IsFavorableVariance(string accountType, decimal variance)
        {
            string type = NormalizeType(accountType);
            if (string.Equals(type, "Expense", StringComparison.OrdinalIgnoreCase))
                return variance <= 0m;

            return variance >= 0m;
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Excel CSV Template (*.csv)|*.csv";
                    sfd.FileName = string.Format("Budget_Template_{0:yyyyMMdd}.csv", DateTime.Today);
                    if (sfd.ShowDialog(this) != DialogResult.OK)
                        return;

                    ExportTemplateCsv(sfd.FileName);
                }

                UiMessages.ShowInfo("Budget template exported successfully.", "تم تصدير قالب الموازنة بنجاح.");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Export failed: " + ex.Message, "فشل التصدير: " + ex.Message);
            }
        }

        private void ExportTemplateCsv(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Account Code,Account Name,Type,Annual Budget,Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec,Total");

            int excelRow = 2;
            foreach (DataGridViewRow row in dgvBudgets.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string[] values =
                {
                    EscapeCsv(Convert.ToString(row.Cells["acc_code"].Value)),
                    EscapeCsv(Convert.ToString(row.Cells["acc_name"].Value)),
                    EscapeCsv(Convert.ToString(row.Cells["account_type"].Value)),
                    "", // annual budget for user input
                    Convert.ToString(GetCellDecimal(row, "jan"), CultureInfo.InvariantCulture),
                    Convert.ToString(GetCellDecimal(row, "feb"), CultureInfo.InvariantCulture),
                    Convert.ToString(GetCellDecimal(row, "mar"), CultureInfo.InvariantCulture),
                    Convert.ToString(GetCellDecimal(row, "apr"), CultureInfo.InvariantCulture),
                    Convert.ToString(GetCellDecimal(row, "may"), CultureInfo.InvariantCulture),
                    Convert.ToString(GetCellDecimal(row, "jun"), CultureInfo.InvariantCulture),
                    Convert.ToString(GetCellDecimal(row, "jul"), CultureInfo.InvariantCulture),
                    Convert.ToString(GetCellDecimal(row, "aug"), CultureInfo.InvariantCulture),
                    Convert.ToString(GetCellDecimal(row, "sep"), CultureInfo.InvariantCulture),
                    Convert.ToString(GetCellDecimal(row, "oct"), CultureInfo.InvariantCulture),
                    Convert.ToString(GetCellDecimal(row, "nov"), CultureInfo.InvariantCulture),
                    Convert.ToString(GetCellDecimal(row, "dec"), CultureInfo.InvariantCulture),
                    string.Format("=SUM(E{0}:P{0})", excelRow)
                };

                sb.AppendLine(string.Join(",", values));
                excelRow++;
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private string EscapeCsv(string value)
        {
            value = value ?? string.Empty;
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
                return "\"" + value.Replace("\"", "\"\"") + "\"";

            return value;
        }

        private void BtnImportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Excel CSV Template (*.csv)|*.csv";
                    if (ofd.ShowDialog(this) != DialogResult.OK)
                        return;

                    ImportTemplateCsv(ofd.FileName);
                    UpdateTotalsStrip();
                }

                UiMessages.ShowInfo("Budget data imported from Excel template.", "تم استيراد بيانات الموازنة من قالب إكسل.");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Import failed: " + ex.Message, "فشل الاستيراد: " + ex.Message);
            }
        }

        private void ImportTemplateCsv(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            if (lines.Length <= 1)
                return;

            Dictionary<string, DataGridViewRow> byCode = dgvBudgets.Rows
                .Cast<DataGridViewRow>()
                .Where(r => !r.IsNewRow)
                .ToDictionary(r => Convert.ToString(r.Cells["acc_code"].Value) ?? string.Empty, r => r);

            for (int i = 1; i < lines.Length; i++)
            {
                string[] cells = ParseCsvLine(lines[i]);
                if (cells.Length < 16)
                    continue;

                string code = cells[0].Trim();
                if (!byCode.ContainsKey(code))
                    continue;

                DataGridViewRow row = byCode[code];
                row.Cells["annual_budget"].Value = ParseDecimal(cells[3]);

                for (int m = 0; m < _months.Length; m++)
                {
                    row.Cells[_months[m]].Value = ParseDecimal(cells[4 + m]);
                }

                RecalculateRow(row, true);
            }
        }

        private decimal ParseDecimal(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0m;

            decimal value;
            if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                return value;

            if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.CurrentCulture, out value))
                return value;

            return 0m;
        }

        private string[] ParseCsvLine(string line)
        {
            List<string> cells = new List<string>();
            bool inQuotes = false;
            StringBuilder current = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];
                if (ch == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (ch == ',' && !inQuotes)
                {
                    cells.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(ch);
                }
            }

            cells.Add(current.ToString());
            return cells.ToArray();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                DGVPrinter printer = new DGVPrinter();
                printer.Title = "Budget Plan";
                printer.SubTitle = string.Format("{0} | {1} | {2}", cmbYear.Text, cmbVersion.Text, cmbCostCenter.Text);
                printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                printer.PageNumbers = true;
                printer.PorportionalColumns = true;
                printer.HeaderCellAlignment = StringAlignment.Center;
                printer.PrintDataGridView(dgvBudgets);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("Print failed: " + ex.Message, "فشلت الطباعة: " + ex.Message);
            }
        }

        private void SetStatusSelection(string status)
        {
            for (int i = 0; i < cmbStatus.Items.Count; i++)
            {
                if (string.Equals(Convert.ToString(cmbStatus.Items[i]), status, StringComparison.OrdinalIgnoreCase))
                {
                    cmbStatus.SelectedIndex = i;
                    return;
                }
            }

            cmbStatus.SelectedIndex = 0;
        }
    }

    internal sealed class NumericPromptDialog : Form
    {
        private readonly NumericUpDown _numeric;

        public decimal Value
        {
            get { return _numeric.Value; }
        }

        public NumericPromptDialog(string title, string prompt, decimal defaultValue)
        {
            Text = title;
            Width = 360;
            Height = 150;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            Label lbl = new Label { Left = 12, Top = 12, Width = 320, Text = prompt };
            _numeric = new NumericUpDown
            {
                Left = 12,
                Top = 35,
                Width = 320,
                DecimalPlaces = 2,
                Minimum = -1000,
                Maximum = 1000,
                Value = defaultValue
            };

            Button btnOk = new Button { Text = "OK", Left = 176, Top = 70, Width = 75, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 257, Top = 70, Width = 75, DialogResult = DialogResult.Cancel };

            Controls.Add(lbl);
            Controls.Add(_numeric);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }

    internal sealed class SeasonalityDialog : Form
    {
        private readonly NumericUpDown[] _percentages = new NumericUpDown[12];
        private readonly Label _lblTotal;

        public SeasonalityDialog()
        {
            Text = "Spread by Seasonality (%)";
            Width = 420;
            Height = 560;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            for (int i = 0; i < 12; i++)
            {
                Label monthLabel = new Label
                {
                    Left = 18,
                    Top = 18 + (i * 34),
                    Width = 120,
                    Text = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[i]
                };

                NumericUpDown nud = new NumericUpDown
                {
                    Left = 145,
                    Top = 14 + (i * 34),
                    Width = 110,
                    DecimalPlaces = 2,
                    Minimum = 0,
                    Maximum = 100,
                    Value = (i == 11) ? 8.37m : 8.33m
                };

                nud.ValueChanged += Percentage_ValueChanged;
                _percentages[i] = nud;

                Controls.Add(monthLabel);
                Controls.Add(nud);
            }

            _lblTotal = new Label
            {
                Left = 18,
                Top = 432,
                Width = 250,
                Text = "Total: 100.00%"
            };

            Button btnPreset = new Button { Left = 270, Top = 428, Width = 120, Text = "Use Equal Spread" };
            btnPreset.Click += delegate
            {
                for (int i = 0; i < 11; i++)
                    _percentages[i].Value = 8.33m;
                _percentages[11].Value = 8.37m;
                UpdateTotalLabel();
            };

            Button btnOk = new Button { Left = 234, Top = 468, Width = 75, Text = "OK", DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Left = 315, Top = 468, Width = 75, Text = "Cancel", DialogResult = DialogResult.Cancel };

            Controls.Add(_lblTotal);
            Controls.Add(btnPreset);
            Controls.Add(btnOk);
            Controls.Add(btnCancel);

            AcceptButton = btnOk;
            CancelButton = btnCancel;

            btnOk.Click += BtnOk_Click;
            UpdateTotalLabel();
        }

        public List<MonthlyPercentageModal> GetPercentages()
        {
            List<MonthlyPercentageModal> result = new List<MonthlyPercentageModal>();
            for (int i = 0; i < 12; i++)
            {
                result.Add(new MonthlyPercentageModal
                {
                    MonthNo = i + 1,
                    Percentage = _percentages[i].Value
                });
            }

            return result;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            decimal total = _percentages.Sum(x => x.Value);
            if (Math.Abs(total - 100m) > 0.01m)
            {
                MessageBox.Show(this, "Monthly percentages must total 100%.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
            }
        }

        private void Percentage_ValueChanged(object sender, EventArgs e)
        {
            UpdateTotalLabel();
        }

        private void UpdateTotalLabel()
        {
            decimal total = _percentages.Sum(x => x.Value);
            _lblTotal.Text = string.Format("Total: {0:N2}%", total);
            _lblTotal.ForeColor = Math.Abs(total - 100m) <= 0.01m ? Color.ForestGreen : Color.Firebrick;
        }
    }
}
