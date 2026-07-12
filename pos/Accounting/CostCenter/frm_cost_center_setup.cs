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
        private int currentCcId = 0;
        private bool isNewRecord = true;
        private bool isLoadingGrid = false;
        private readonly int initialCcId;
        private readonly int? initialParentCcId;

        public frm_cost_center_setup() : this(0, null)
        {
        }

        public frm_cost_center_setup(int ccId, int? parentCcId = null)
        {
            initialCcId = ccId;
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
            LoadParentCostCenterDropdown();
            LoadCostCenterList();

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
                LoadCostCenter(initialCcId);
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
            // Load users from database (simplified - assumes users table exists)
            try
            {
                UsersBLL usersBLL = new UsersBLL();
                DataTable dt = new DataTable();
                dt = usersBLL.GetAll();
                //using (var cn = new System.Data.SqlClient.SqlConnection(POS.DLL.dbConnection.ConnectionString))
                //{
                //    cn.Open();
                //    const string sql = "SELECT id, name FROM pos_users WHERE is_active = 1 ORDER BY name";
                //    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                //    {
                //        using (var da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                //        {
                //            da.Fill(dt);
                //        }
                //    }
                //}

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

        private void LoadCostCenterList()
        {
            try
            {
                isLoadingGrid = true;
                DataTable dt = bll.GetCostCenterTree(includeBalances: false);
                dgvCostCenters.DataSource = dt;
                dgvCostCenters.AutoResizeColumns();
                dgvCostCenters.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cost centers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            currentCcId = 0;
            EnableEditControls(true);
            txtCode.Focus();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                using (BusyScope.Show(this, "Saving cost center..."))
                {
                    var model = new CostCenterModel
                    {
                        CcId = currentCcId,
                        CcCode = txtCode.Text.Trim(),
                        CcName = txtName.Text.Trim(),
                        CcType = cmbType.SelectedItem?.ToString(),
                        ParentCcId = cmbParent.SelectedValue != null && (int)cmbParent.SelectedValue > 0 ? (int?)cmbParent.SelectedValue : null,
                        ManagerId = cmbManager.SelectedValue != null && (int)cmbManager.SelectedValue > 0 ? (int?)cmbManager.SelectedValue : null,
                        MonthlyBudget = string.IsNullOrWhiteSpace(txtBudget.Text) ? null : (decimal?)decimal.Parse(txtBudget.Text),
                        StartDate = dtpStartDate.Value.Date,
                        EndDate = chkHasEndDate.Checked ? (DateTime?)dtpEndDate.Value.Date : null,
                        IsActive = chkActive.Checked,
                        Description = txtDescription.Text.Trim()
                    };

                    currentCcId = bll.SaveCostCenter(model, UsersModal.logged_in_userid);
                    MessageBox.Show($"Cost center saved successfully. ID: {currentCcId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadCostCenterList();
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
                MessageBox.Show($"Error saving cost center: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (currentCcId <= 0)
            {
                MessageBox.Show("Please select a cost center to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to deactivate this cost center?", "Confirm Deactivate", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, "Deactivating cost center..."))
                {
                    var model = bll.GetCostCenterById(currentCcId);
                    if (model != null)
                    {
                        model.IsActive = false;
                        bll.SaveCostCenter(model, UsersModal.logged_in_userid);
                        MessageBox.Show("Cost center deactivated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCostCenterList();
                        ClearForm();
                        EnableEditControls(false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deactivating cost center: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvCostCenters_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isLoadingGrid && e.RowIndex >= 0 && TryGetSelectedCostCenterId(e.RowIndex, out int ccId))
            {
                LoadCostCenter(ccId);
            }
        }

        private void DgvCostCenters_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!isLoadingGrid && e.RowIndex >= 0 && TryGetSelectedCostCenterId(e.RowIndex, out int ccId))
            {
                LoadCostCenter(ccId);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (TryGetSelectedCostCenterId(out int ccId))
            {
                LoadCostCenter(ccId);
                return;
            }

            MessageBox.Show("Please select a cost center to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DgvCostCenters_SelectionChanged(object sender, EventArgs e)
        {
            if (!isLoadingGrid && TryGetSelectedCostCenterId(out int ccId) && ccId != currentCcId)
            {
                LoadCostCenter(ccId);
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

        private void LoadCostCenter(int ccId)
        {
            try
            {
                var model = bll.GetCostCenterById(ccId);
                if (model != null)
                {
                    currentCcId = model.CcId;
                    txtCode.Text = model.CcCode;
                    txtName.Text = model.CcName;
                    cmbType.SelectedItem = model.CcType ?? "Department";
                    cmbParent.SelectedValue = model.ParentCcId ?? -1;
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
                MessageBox.Show($"Error loading cost center: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadParentCostCenterDropdown()
        {
            try
            {
                DataTable dt = bll.GetCostCenterDropdown();
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
                MessageBox.Show($"Error loading parent cost centers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Cost Center Code is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Cost Center Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            cmbParent.SelectedIndex = -1;
            cmbManager.SelectedIndex = -1;
            dtpStartDate.Value = DateTime.Today;
            chkHasEndDate.Checked = false;
            chkActive.Checked = true;
            currentCcId = 0;
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
