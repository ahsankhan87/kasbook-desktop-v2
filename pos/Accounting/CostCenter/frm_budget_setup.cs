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
    /// Budget Setup Form - Configure monthly budgets for cost centers
    /// </summary>
    public partial class frm_budget_setup : Form
    {
        private CostCenterBLL bll = new CostCenterBLL();
        private int selectedCcId = 0;
        private int selectedYearId = 0;

        public frm_budget_setup()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Apply theme
            AppTheme.Apply(this);

            // Setup event handlers
            btnLoad.Click += BtnLoad_Click;
            btnSave.Click += BtnSave_Click;
            btnClear.Click += BtnClear_Click;
            btnFillYear.Click += BtnFillYear_Click;
            cmbCostCenter.SelectedIndexChanged += CmbCostCenter_SelectedIndexChanged;
            cmbYear.SelectedIndexChanged += CmbYear_SelectedIndexChanged;

            // Setup grid
            InitializeGrid();
            LoadCostCenters();
            LoadFiscalYears();
        }

        private void InitializeGrid()
        {
            dgvBudgets.AllowUserToAddRows = true;
            dgvBudgets.AllowUserToDeleteRows = true;
            dgvBudgets.RowHeadersVisible = false;
            dgvBudgets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // Define columns
            dgvBudgets.Columns.Clear();

            DataGridViewTextBoxColumn colAccountId = new DataGridViewTextBoxColumn
            {
                HeaderText = "Account ID",
                Name = "AccountId",
                Width = 80,
                Visible = false
            };
            dgvBudgets.Columns.Add(colAccountId);

            DataGridViewTextBoxColumn colAccountCode = new DataGridViewTextBoxColumn
            {
                HeaderText = "Account Code",
                Name = "AccountCode",
                Width = 100,
                ReadOnly = false
            };
            dgvBudgets.Columns.Add(colAccountCode);

            DataGridViewTextBoxColumn colAccountName = new DataGridViewTextBoxColumn
            {
                HeaderText = "Account Name",
                Name = "AccountName",
                Width = 200,
                ReadOnly = false
            };
            dgvBudgets.Columns.Add(colAccountName);

            // Add 12 month columns
            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            for (int i = 0; i < months.Length; i++)
            {
                DataGridViewTextBoxColumn colMonth = new DataGridViewTextBoxColumn
                {
                    HeaderText = months[i],
                    Name = $"Month{i + 1}",
                    Width = 80,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Alignment = DataGridViewContentAlignment.MiddleRight,
                        Format = "N2"
                    }
                };
                dgvBudgets.Columns.Add(colMonth);
            }

            DataGridViewTextBoxColumn colTotal = new DataGridViewTextBoxColumn
            {
                HeaderText = "Total",
                Name = "Total",
                Width = 100,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N2",
                    BackColor = System.Drawing.Color.LightGray
                }
            };
            dgvBudgets.Columns.Add(colTotal);
        }

        private void LoadCostCenters()
        {
            try
            {
                DataTable dt = bll.GetCostCenterDropdown();
                cmbCostCenter.DataSource = dt;
                cmbCostCenter.DisplayMember = "display_text";
                cmbCostCenter.ValueMember = "id";
                cmbCostCenter.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cost centers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFiscalYears()
        {
            try
            {
                DataTable dt = new DataTable();
                using (var cn = new System.Data.SqlClient.SqlConnection(POS.DLL.dbConnection.ConnectionString))
                {
                    cn.Open();
                    const string sql = "SELECT id, CONCAT(YEAR(from_date), '-', YEAR(to_date)) AS year_label FROM dbo.acc_fiscal_years ORDER BY id DESC";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                    {
                        using (var da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                cmbYear.DataSource = dt;
                cmbYear.DisplayMember = "year_label";
                cmbYear.ValueMember = "id";
                cmbYear.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading fiscal years: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbCostCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCostCenter.SelectedValue != null && int.TryParse(cmbCostCenter.SelectedValue.ToString(), out int ccId))
                selectedCcId = ccId;
        }

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbYear.SelectedValue != null && int.TryParse(cmbYear.SelectedValue.ToString(), out int yearId))
                selectedYearId = yearId;
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (selectedCcId <= 0 || selectedYearId <= 0)
            {
                MessageBox.Show("Please select both Cost Center and Fiscal Year.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (BusyScope.Show(this, "Loading budgets..."))
                {
                    LoadBudgetForCostCenter(selectedCcId, selectedYearId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading budgets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBudgetForCostCenter(int ccId, int yearId)
        {
            // Load existing budgets or show empty grid for new entry
            DataTable dt = new DataTable();
            using (var cn = new System.Data.SqlClient.SqlConnection(POS.DLL.dbConnection.ConnectionString))
            {
                cn.Open();
                const string sql = @"
SELECT
    b.budget_id,
    a.id AS AccountId,
    a.code AS AccountCode,
    a.name AS AccountName,
    b.jan_budget AS Month1,
    b.feb_budget AS Month2,
    b.mar_budget AS Month3,
    b.apr_budget AS Month4,
    b.may_budget AS Month5,
    b.jun_budget AS Month6,
    b.jul_budget AS Month7,
    b.aug_budget AS Month8,
    b.sep_budget AS Month9,
    b.oct_budget AS Month10,
    b.nov_budget AS Month11,
    b.dec_budget AS Month12
FROM dbo.acc_cost_center_budgets b
INNER JOIN dbo.acc_accounts a ON a.id = b.account_id
WHERE b.cc_id = @ccId AND b.financial_year_id = @yearId
ORDER BY a.code;";

                using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ccId", ccId);
                    cmd.Parameters.AddWithValue("@yearId", yearId);
                    using (var da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            dgvBudgets.DataSource = dt;

            // Bind grid if data exists
            if (dt.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvBudgets.Rows)
                {
                    CalculateRowTotal(row);
                }
            }

            lblStatus.Text = $"Loaded {dt.Rows.Count} budget records for Cost Center {selectedCcId}, Year {selectedYearId}";
        }

        private void CalculateRowTotal(DataGridViewRow row)
        {
            decimal total = 0;
            for (int col = 3; col < 15; col++) // Columns 3-14 are months
            {
                if (decimal.TryParse(row.Cells[col].Value?.ToString() ?? "0", out decimal monthValue))
                    total += monthValue;
            }
            row.Cells["Total"].Value = total;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (selectedCcId <= 0 || selectedYearId <= 0)
            {
                MessageBox.Show("Please select both Cost Center and Fiscal Year.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var budgets = new List<AccountBudget>();

                foreach (DataGridViewRow row in dgvBudgets.Rows)
                {
                    if (row.IsNewRow || !int.TryParse(row.Cells["AccountId"].Value?.ToString() ?? "0", out int accountId) || accountId <= 0)
                        continue;

                    var budget = new AccountBudget
                    {
                        CcId = selectedCcId,
                        FinancialYearId = selectedYearId,
                        AccountId = accountId,
                        JanBudget = GetCellValue(row, 3),
                        FebBudget = GetCellValue(row, 4),
                        MarBudget = GetCellValue(row, 5),
                        AprBudget = GetCellValue(row, 6),
                        MayBudget = GetCellValue(row, 7),
                        JunBudget = GetCellValue(row, 8),
                        JulBudget = GetCellValue(row, 9),
                        AugBudget = GetCellValue(row, 10),
                        SepBudget = GetCellValue(row, 11),
                        OctBudget = GetCellValue(row, 12),
                        NovBudget = GetCellValue(row, 13),
                        DecBudget = GetCellValue(row, 14)
                    };

                    budgets.Add(budget);
                }

                if (budgets.Count == 0)
                {
                    MessageBox.Show("No budgets to save.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (BusyScope.Show(this, "Saving budgets..."))
                {
                    bll.SetBudget(selectedCcId, selectedYearId, budgets, UsersModal.logged_in_userid);
                    MessageBox.Show("Budgets saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadBudgetForCostCenter(selectedCcId, selectedYearId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving budgets: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private decimal GetCellValue(DataGridViewRow row, int columnIndex)
        {
            if (decimal.TryParse(row.Cells[columnIndex].Value?.ToString() ?? "0", out decimal value))
                return value;
            return 0;
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            dgvBudgets.DataSource = null;
            lblStatus.Text = "Ready";
        }

        private void BtnFillYear_Click(object sender, EventArgs e)
        {
            if (dgvBudgets.Rows.Count == 0)
            {
                MessageBox.Show("Please load budgets first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Get average of 3 months and fill year
            decimal? templateAmount = null;
            InputBox input = new InputBox("Enter template amount for all months:", "Fill Year");
            if (input.ShowDialog() == DialogResult.OK && decimal.TryParse(input.Result, out decimal amount))
            {
                templateAmount = amount;
            }

            if (!templateAmount.HasValue)
                return;

            foreach (DataGridViewRow row in dgvBudgets.Rows)
            {
                if (!row.IsNewRow)
                {
                    for (int col = 3; col < 15; col++)
                    {
                        row.Cells[col].Value = templateAmount.Value;
                    }
                    CalculateRowTotal(row);
                }
            }
        }

        private void FrmBudgetSetup_Load(object sender, EventArgs e)
        {
            LoadCostCenters();
            LoadFiscalYears();
        }
    }

    /// <summary>
    /// Simple input dialog for template amount
    /// </summary>
    public class InputBox : Form
    {
        private TextBox txtInput;
        public string Result { get; private set; }

        public InputBox(string prompt, string title)
        {
            this.Text = title;
            this.Width = 300;
            this.Height = 150;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lblPrompt = new Label { Text = prompt, Left = 20, Top = 20, Width = 250, Height = 40, AutoSize = true };
            txtInput = new TextBox { Left = 20, Top = 65, Width = 250 };
            Button btnOK = new Button { Text = "OK", Left = 120, Top = 100, Width = 75, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 200, Top = 100, Width = 75, DialogResult = DialogResult.Cancel };

            this.Controls.Add(lblPrompt);
            this.Controls.Add(txtInput);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public new DialogResult ShowDialog()
        {
            return base.ShowDialog();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
                Result = txtInput.Text;
            base.OnFormClosing(e);
        }
    }
}
