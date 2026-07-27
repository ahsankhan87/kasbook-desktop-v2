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
    /// Cost Center Setup Form - Create, edit, and manage cost centers
    /// </summary>
    public partial class frm_cost_center_setup : Form
    {
        private CostCenterBLL bll = new CostCenterBLL();
        private int currentBranchId = 0;
        private bool isNewRecord = true;
        private bool isLoadingGrid = false;
        private readonly int initialCcId;
        private readonly int? initialParentCcId;

        public frm_cost_center_setup() : this(0, null)
        {
        }

        public frm_cost_center_setup(int branchId, int? parentCcId = null)
        {
            initialCcId = branchId;
            initialParentCcId = parentCcId;
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Apply theme
            AppTheme.Apply(this);

            // Setup form controls
            SetupTypeDropdown();
            SetupManagerDropdown();
            LoadParentBranchDropdown();
            LoadBranchList();

            // Event handlers
            btnNew.Click += BtnNew_Click;
            btnEdit.Click += BtnEdit_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnDelete.Click += BtnDelete_Click;
            dgvCostCenters.CellClick += DgvCostCenters_CellClick;
            dgvCostCenters.CellDoubleClick += DgvCostCenters_CellDoubleClick;
            dgvCostCenters.SelectionChanged += DgvCostCenters_SelectionChanged;
            dgvCostCenters.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCostCenters.MultiSelect = false;
            dgvCostCenters.ClearSelection();

            dtpStartDate.Value = DateTime.Today;

            if (initialCcId > 0)
            {
                EnableEditControls(false);
                LoadBranch(initialCcId);
            }
            else
            {
                EnableEditControls(false);
                if (initialParentCcId.HasValue)
                {
                    cmbParent.SelectedValue = initialParentCcId.Value;
                }
            }
        }

        private void SetupTypeDropdown()
        {
            cmbType.Items.Clear();
            cmbType.Items.Add("Department");
            cmbType.Items.Add("Branch");
            cmbType.Items.Add("Project");
            cmbType.Items.Add("Product Line");
            cmbType.Items.Add("Region");
            cmbType.Items.Add("Customer Group");
            cmbType.SelectedIndex = 0;
        }

        private void SetupManagerDropdown()
        {
            try
            {
                UsersBLL usersBLL = new UsersBLL();
                DataTable dt = new DataTable();
                dt = usersBLL.GetAll();
                
                cmbManager.DataSource = dt;
                cmbManager.DisplayMember = "name";
                cmbManager.ValueMember = "id";
                cmbManager.SelectedIndex = -1;
            }
            catch
            {
                MessageBox.Show("Could not load managers. You may need to select manually.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadBranchList()
        {
            try
            {
                isLoadingGrid = true;
                DataTable dt = bll.GetBranchTree(includeBalances: false);
                dgvCostCenters.DataSource = dt;
                dgvCostCenters.AutoResizeColumns();
                dgvCostCenters.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading branches: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoadingGrid = false;
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            isNewRecord = true;
            currentBranchId = 0;
            EnableEditControls(true);
            txtCode.Focus();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                using (BusyScope.Show(this, "Saving branch..."))
                {
                    var model = new CostCenterModel
                    {
                        BranchId = currentBranchId,
                        BranchCode = txtCode.Text.Trim(),
                        BranchName = txtName.Text.Trim(),
                        BranchType = cmbType.SelectedItem?.ToString(),
                        ParentBranchId = GetNullableSelectedId(cmbParent),
                        ManagerId = GetNullableSelectedId(cmbManager),
                        MonthlyBudget = string.IsNullOrWhiteSpace(txtBudget.Text) ? null : (decimal?)decimal.Parse(txtBudget.Text),
                        StartDate = dtpStartDate.Value.Date,
                        EndDate = chkHasEndDate.Checked ? (DateTime?)dtpEndDate.Value.Date : null,
                        IsActive = chkActive.Checked,
                        Description = txtDescription.Text.Trim()
                    };

                    currentBranchId = bll.SaveBranch(model, UsersModal.logged_in_userid);
                    MessageBox.Show($"Branch saved successfully. ID: {currentBranchId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadBranchList();
                    ClearForm();
                    EnableEditControls(false);
                    isNewRecord = true;
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Validation error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving branch: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableEditControls(false);
            isNewRecord = true;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (currentBranchId <= 0)
            {
                MessageBox.Show("Please select a branch to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to deactivate this branch?", "Confirm Deactivate", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, "Deactivating branch..."))
                {
                    var model = bll.GetBranchById(currentBranchId);
                    if (model != null)
                    {
                        model.IsActive = false;
                        bll.SaveBranch(model, UsersModal.logged_in_userid);
                        MessageBox.Show("Branch deactivated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadBranchList();
                        ClearForm();
                        EnableEditControls(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deactivating branch: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvCostCenters_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isLoadingGrid && e.RowIndex >= 0 && TryGetSelectedCostCenterId(e.RowIndex, out int ccId))
            {
                LoadBranch(ccId);
            }
        }

        private void DgvCostCenters_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isLoadingGrid && e.RowIndex >= 0 && TryGetSelectedCostCenterId(e.RowIndex, out int ccId))
            {
                LoadBranch(ccId);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (TryGetSelectedCostCenterId(out int ccId))
            {
                LoadBranch(ccId);
                return;
            }

            MessageBox.Show("Please select a branch to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DgvCostCenters_SelectionChanged(object sender, EventArgs e)
        {
            if (!isLoadingGrid && TryGetSelectedCostCenterId(out int ccId) && ccId != currentBranchId)
            {
                LoadBranch(ccId);
            }
        }

        private bool TryGetSelectedCostCenterId(int rowIndex, out int ccId)
        {
            ccId = 0;

            if (rowIndex < 0 || rowIndex >= dgvCostCenters.Rows.Count)
                return false;

            return TryGetSelectedCostCenterId(dgvCostCenters.Rows[rowIndex], out ccId);
        }

        private bool TryGetSelectedCostCenterId(out int ccId)
        {
            ccId = 0;

            DataGridViewRow selectedRow = dgvCostCenters.CurrentRow ?? (dgvCostCenters.SelectedRows.Count > 0 ? dgvCostCenters.SelectedRows[0] : null);
            if (selectedRow == null)
                return false;

            return TryGetSelectedCostCenterId(selectedRow, out ccId);
        }

        private bool TryGetSelectedCostCenterId(DataGridViewRow row, out int ccId)
        {
            ccId = 0;
            if (row == null)
                return false;

            if (row.DataGridView != null && row.DataGridView.Columns.Contains("cc_id") && row.Cells["cc_id"].Value != null)
            {
                return int.TryParse(row.Cells["cc_id"].Value.ToString(), out ccId);
            }

            if (row.DataGridView != null && row.DataGridView.Columns.Contains("id") && row.Cells["id"].Value != null)
            {
                return int.TryParse(row.Cells["id"].Value.ToString(), out ccId);
            }

            if (row.DataBoundItem is DataRowView drv)
            {
                if (drv.Row.Table.Columns.Contains("cc_id"))
                    return int.TryParse(drv["cc_id"]?.ToString(), out ccId);

                if (drv.Row.Table.Columns.Contains("id"))
                    return int.TryParse(drv["id"]?.ToString(), out ccId);
            }

            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell?.Value != null && int.TryParse(cell.Value.ToString(), out ccId))
                    return true;
            }

            return false;
        }

        private void LoadBranch(int branchId)
        {
            try
            {
                var model = bll.GetBranchById(branchId);
                if (model != null)
                {
                    currentBranchId = model.BranchId;
                    txtCode.Text = model.BranchCode;
                    txtName.Text = model.BranchName;
                    cmbType.SelectedItem = model.BranchType ?? "Department";
                    cmbParent.SelectedValue = model.ParentBranchId ?? -1;
                    cmbManager.SelectedValue = model.ManagerId ?? -1;
                    txtBudget.Text = model.MonthlyBudget?.ToString("N2") ?? "";
                    dtpStartDate.Value = model.StartDate < dtpStartDate.MinDate || model.StartDate > dtpStartDate.MaxDate
                        ? DateTime.Today
                        : model.StartDate;
                    if (model.EndDate.HasValue)
                    {
                        chkHasEndDate.Checked = true;
                        dtpEndDate.Value = model.EndDate.Value;
                    }
                    else
                    {
                        chkHasEndDate.Checked = false;
                    }
                    chkActive.Checked = model.IsActive;
                    txtDescription.Text = model.Description ?? "";

                    isNewRecord = false;
                    EnableEditControls(true);
                    txtCode.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading branch: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadParentBranchDropdown()
        {
            try
            {
                DataTable dt = bll.GetBranchDropdown();
                // Add an empty row for "None"
                DataRow emptyRow = dt.NewRow();
                emptyRow["id"] = -1;
                emptyRow["display_text"] = "— None —";
                dt.Rows.InsertAt(emptyRow, 0);

                cmbParent.DataSource = dt;
                cmbParent.DisplayMember = "display_text";
                cmbParent.ValueMember = "id";
                cmbParent.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading parent branches: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int? GetNullableSelectedId(ComboBox comboBox)
        {
            if (comboBox == null)
                return null;

            object selectedValue = comboBox.SelectedValue;
            if (selectedValue == null || selectedValue == DBNull.Value || selectedValue is DataRowView)
                return null;

            if (int.TryParse(selectedValue.ToString(), out int parsedId) && parsedId > 0)
                return parsedId;

            return null;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Branch Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Branch Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtBudget.Text) && !decimal.TryParse(txtBudget.Text, out _))
            {
                MessageBox.Show("Budget must be a valid decimal number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBudget.Focus();
                return false;
            }

            if (chkHasEndDate.Checked && dtpEndDate.Value <= dtpStartDate.Value)
            {
                MessageBox.Show("End Date must be after Start Date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndDate.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtCode.Clear();
            txtName.Clear();
            txtBudget.Clear();
            txtDescription.Clear();
            cmbType.SelectedIndex = 0;
            cmbParent.SelectedIndex = cmbParent.Items.Count > 0 ? 0 : -1;
            cmbManager.SelectedIndex = cmbManager.Items.Count > 0 ? 0 : -1;
            dtpStartDate.Value = DateTime.Today;
            chkHasEndDate.Checked = false;
            chkActive.Checked = true;
            currentBranchId = 0;
        }

        private void EnableEditControls(bool enabled)
        {
            txtCode.Enabled = enabled;
            txtName.Enabled = enabled;
            cmbType.Enabled = enabled;
            cmbParent.Enabled = enabled;
            cmbManager.Enabled = enabled;
            txtBudget.Enabled = enabled;
            dtpStartDate.Enabled = enabled;
            chkHasEndDate.Enabled = enabled;
            dtpEndDate.Enabled = enabled && chkHasEndDate.Checked;
            chkActive.Enabled = enabled;
            txtDescription.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnCancel.Enabled = enabled;
            btnDelete.Enabled = enabled && !isNewRecord;
        }

        private void ChkHasEndDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpEndDate.Enabled = chkHasEndDate.Checked && txtCode.Enabled;
            if (!chkHasEndDate.Checked)
                dtpEndDate.Value = DateTime.Today.AddYears(1);
        }

        private void FrmCostCenterSetup_Load(object sender, EventArgs e)
        {
        }
    }
}
