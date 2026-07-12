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
    /// Cost Center Allocation Rules Form - Define expense allocation rules
    /// </summary>
    public partial class frm_allocation_rules : Form
    {
        private CostCenterBLL bll = new CostCenterBLL();

        public frm_allocation_rules()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Apply theme
            AppTheme.Apply(this);

            // Setup event handlers
            btnNew.Click += BtnNew_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRunAllocation.Click += BtnRunAllocation_Click;
            dgvRules.CellDoubleClick += DgvRules_CellDoubleClick;

            // Setup dropdowns
            SetupMethodDropdown();
            SetupAccountDropdown();
            SetupCostCenterDropdown();

            LoadRules();
            EnableEditControls(false);
        }

        private void SetupMethodDropdown()
        {
            cmbMethod.Items.Clear();
            cmbMethod.Items.Add("FIXED_PCT");
            cmbMethod.Items.Add("HEADCOUNT");
            cmbMethod.Items.Add("REVENUE");
            cmbMethod.SelectedIndex = 0;
        }

        private void SetupAccountDropdown()
        {
            try
            {
                DataTable dt = new DataTable();
                using (var cn = new System.Data.SqlClient.SqlConnection(POS.DLL.dbConnection.ConnectionString))
                {
                    cn.Open();
                    const string sql = @"
SELECT id, CONCAT(code, ' — ', name) AS display_text
FROM dbo.acc_accounts
WHERE group_id IN (SELECT id FROM dbo.acc_groups WHERE account_type_id IN 
  (SELECT id FROM dbo.acc_account_type WHERE name LIKE '%Expense%'))
ORDER BY code;";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                    {
                        using (var da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                cmbSourceAccount.DataSource = dt;
                cmbSourceAccount.DisplayMember = "display_text";
                cmbSourceAccount.ValueMember = "id";
                cmbSourceAccount.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading accounts: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupCostCenterDropdown()
        {
            try
            {
                DataTable dt = bll.GetCostCenterDropdown();
                cmbTargetCostCenter.DataSource = dt;
                cmbTargetCostCenter.DisplayMember = "display_text";
                cmbTargetCostCenter.ValueMember = "id";
                cmbTargetCostCenter.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cost centers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRules()
        {
            try
            {
                DataTable dt = new DataTable();
                using (var cn = new System.Data.SqlClient.SqlConnection(POS.DLL.dbConnection.ConnectionString))
                {
                    cn.Open();
                    const string sql = @"
SELECT
    a.alloc_id,
    a.alloc_name,
    acc.code AS source_account_code,
    acc.name AS source_account_name,
    c.cc_code AS target_cc_code,
    c.cc_name AS target_cc_name,
    a.allocation_percent,
    a.allocation_method,
    a.is_active
FROM dbo.acc_cost_center_allocations a
INNER JOIN dbo.acc_accounts acc ON acc.id = a.source_acc_id
INNER JOIN dbo.acc_cost_centers c ON c.cc_id = a.cc_id
ORDER BY a.alloc_name;";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                    {
                        using (var da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                dgvRules.DataSource = dt;
                dgvRules.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading rules: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableEditControls(true);
            txtName.Focus();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                using (BusyScope.Show(this, "Saving allocation rule..."))
                {
                    int sourceAccId = (int)cmbSourceAccount.SelectedValue;
                    int ccId = (int)cmbTargetCostCenter.SelectedValue;
                    decimal allocationPercent = decimal.Parse(txtPercent.Text);
                    string allocationMethod = cmbMethod.SelectedItem?.ToString() ?? "FIXED_PCT";

                    const string sql = @"
IF NOT EXISTS (SELECT 1 FROM dbo.acc_cost_center_allocations 
  WHERE alloc_name = @name)
BEGIN
    INSERT INTO dbo.acc_cost_center_allocations
    (alloc_name, source_acc_id, cc_id, allocation_percent, allocation_method, is_active, created_at)
    VALUES (@name, @sourceAccId, @ccId, @percent, @method, @isActive, GETDATE());
END
ELSE
BEGIN
    UPDATE dbo.acc_cost_center_allocations
    SET source_acc_id = @sourceAccId,
        cc_id = @ccId,
        allocation_percent = @percent,
        allocation_method = @method,
        is_active = @isActive
    WHERE alloc_name = @name;
END";

                    using (var cn = new System.Data.SqlClient.SqlConnection(POS.DLL.dbConnection.ConnectionString))
                    {
                        cn.Open();
                        using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                            cmd.Parameters.AddWithValue("@sourceAccId", sourceAccId);
                            cmd.Parameters.AddWithValue("@ccId", ccId);
                            cmd.Parameters.AddWithValue("@percent", allocationPercent);
                            cmd.Parameters.AddWithValue("@method", allocationMethod);
                            cmd.Parameters.AddWithValue("@isActive", chkActive.Checked ? 1 : 0);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Allocation rule saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRules();
                    ClearForm();
                    EnableEditControls(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving rule: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableEditControls(false);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please select a rule to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this rule?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, "Deleting rule..."))
                {
                    const string sql = "DELETE FROM dbo.acc_cost_center_allocations WHERE alloc_name = @name";
                    using (var cn = new System.Data.SqlClient.SqlConnection(POS.DLL.dbConnection.ConnectionString))
                    {
                        cn.Open();
                        using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Rule deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRules();
                    ClearForm();
                    EnableEditControls(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting rule: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRunAllocation_Click(object sender, EventArgs e)
        {
            var period = dtpPeriod.Value.Date;

            DialogResult result = MessageBox.Show(
                $"Run allocation for period: {period:yyyy-MM}?\n\nThis will create journal entries distributing expenses.",
                "Confirm Allocation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, "Running allocation..."))
                {
                    var allocationResult = bll.RunExpenseAllocation(period, UsersModal.logged_in_userid);

                    if (allocationResult.Success)
                    {
                        string message = $"Allocation completed successfully!\n\n" +
                            $"Voucher: {allocationResult.VoucherNo}\n" +
                            $"Total Allocated: {allocationResult.TotalAllocated:N2}\n" +
                            $"Departments: {allocationResult.Allocations.Count}";
                        MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Error: {allocationResult.Message}", "Allocation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error running allocation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvRules_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRules.Rows[e.RowIndex].Cells[0].Value != null)
            {
                string ruleName = dgvRules.Rows[e.RowIndex].Cells["alloc_name"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(ruleName))
                {
                    LoadRule(ruleName);
                }
            }
        }

        private void LoadRule(string ruleName)
        {
            try
            {
                DataTable dt = new DataTable();
                using (var cn = new System.Data.SqlClient.SqlConnection(POS.DLL.dbConnection.ConnectionString))
                {
                    cn.Open();
                    const string sql = @"
SELECT
    alloc_id, alloc_name, source_acc_id, cc_id, allocation_percent, allocation_method, is_active
FROM dbo.acc_cost_center_allocations
WHERE alloc_name = @name;";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@name", ruleName);
                        using (var da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtName.Text = row["alloc_name"]?.ToString() ?? "";
                    cmbSourceAccount.SelectedValue = row["source_acc_id"];
                    cmbTargetCostCenter.SelectedValue = row["cc_id"];
                    txtPercent.Text = row["allocation_percent"]?.ToString() ?? "0";
                    cmbMethod.SelectedItem = row["allocation_method"]?.ToString() ?? "FIXED_PCT";
                    chkActive.Checked = (bool)row["is_active"];
                    EnableEditControls(true);
                    txtName.Enabled = false; // Don't allow changing name
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading rule: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Rule Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (cmbSourceAccount.SelectedValue == null)
            {
                MessageBox.Show("Source Account is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSourceAccount.Focus();
                return false;
            }

            if (cmbTargetCostCenter.SelectedValue == null)
            {
                MessageBox.Show("Target Cost Center is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTargetCostCenter.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPercent.Text, out decimal percent) || percent <= 0 || percent > 100)
            {
                MessageBox.Show("Allocation Percent must be between 0.01 and 100.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPercent.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtPercent.Clear();
            cmbSourceAccount.SelectedIndex = -1;
            cmbTargetCostCenter.SelectedIndex = -1;
            cmbMethod.SelectedIndex = 0;
            chkActive.Checked = true;
            dtpPeriod.Value = DateTime.Today;
        }

        private void EnableEditControls(bool enabled)
        {
            txtName.Enabled = enabled;
            cmbSourceAccount.Enabled = enabled;
            cmbTargetCostCenter.Enabled = enabled;
            txtPercent.Enabled = enabled;
            cmbMethod.Enabled = enabled;
            chkActive.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnCancel.Enabled = enabled;
            btnDelete.Enabled = enabled && !string.IsNullOrWhiteSpace(txtName.Text);
        }

        private void FrmAllocationRules_Load(object sender, EventArgs e)
        {
            LoadRules();
            dtpPeriod.Value = DateTime.Today;
        }
    }
}
