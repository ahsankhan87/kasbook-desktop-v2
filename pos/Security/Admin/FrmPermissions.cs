using pos.Security.Authorization;
using POS.BLL;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace pos.Security.Admin
{
    public partial class FrmPermissions : SecuredForm
    {
        private readonly PermissionsBLL _bll;
        private const string NameColumn = "permission_name";
        private const string IdColumn = "id";

        public FrmPermissions()
        {
            InitializeComponent();

            _bll = new PermissionsBLL();

            // Tag controls for permission-based access
            btnAdd.Tag = Permissions.Security_Permissions_Create;
            btnUpdate.Tag = Permissions.Security_Permissions_Edit;
            btnDelete.Tag = Permissions.Security_Permissions_Delete;

            this.Load += (s, e) =>
            {
                EnsureGridColumns();
                LoadPermissions();
                txtSearch.Focus();
            };
        }

        private void EnsureGridColumns()
        {
            // Define explicit columns bound to DataTable fields
            gridPermissions.AutoGenerateColumns = false;
            if (gridPermissions.Columns.Count == 0)
            {
                gridPermissions.Columns.Clear();

                var colId = new DataGridViewTextBoxColumn
                {
                    Name = IdColumn,
                    HeaderText = "ID",
                    DataPropertyName = IdColumn,
                    ReadOnly = true,
                    Width = 80
                };
                gridPermissions.Columns.Add(colId);

                var colName = new DataGridViewTextBoxColumn
                {
                    Name = NameColumn,
                    HeaderText = "Permission",
                    DataPropertyName = NameColumn,
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                };
                gridPermissions.Columns.Add(colName);
            }
        }

        private void FrmPermissions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SendKeys.Send("{TAB}");
        }

        private void LoadPermissions()
        {
            try
            {
                var dt = _bll.GetAll();
                gridPermissions.DataSource = dt;
                lblCount.Text = $"Total: {dt.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load permissions.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var name = txtPermissionName.Text.Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Permission name is required.");
                    return;
                }

                // Prevent duplicates (quick check from current grid dataset)
                var current = (gridPermissions.DataSource as DataTable);
                if (current != null && current.Rows.Cast<DataRow>()
                    .Any(r => string.Equals(Convert.ToString(r[NameColumn]), name, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Permission already exists.");
                    return;
                }

                var result = _bll.Create(name);
                if (result > 0)
                {
                    MessageBox.Show("Permission added.");
                    txtPermissionName.Clear();
                    LoadPermissions();
                    AppSecurityContext.RefreshUserClaims();
                }
                else
                {
                    MessageBox.Show("Permission not added.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding permission.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridPermissions.CurrentRow == null)
                {
                    MessageBox.Show("Select a permission to update.");
                    return;
                }

                var id = Convert.ToInt32(gridPermissions.CurrentRow.Cells[IdColumn].Value);
                var name = txtPermissionName.Text.Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Permission name is required.");
                    return;
                }

                var result = _bll.Update(id, name);
                if (result > 0)
                {
                    MessageBox.Show("Permission updated.");
                    LoadPermissions();
                    AppSecurityContext.RefreshUserClaims();
                }
                else
                {
                    MessageBox.Show("Permission not updated.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating permission.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridPermissions.CurrentRow == null)
                {
                    MessageBox.Show("Select a permission to delete.");
                    return;
                }

                var id = Convert.ToInt32(gridPermissions.CurrentRow.Cells[IdColumn].Value);
                var name = gridPermissions.CurrentRow.Cells[NameColumn].Value?.ToString();

                var confirm = MessageBox.Show($"Delete permission \"{name}\"?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes) return;

                var result = _bll.Delete(id);
                if (result > 0)
                {
                    MessageBox.Show("Permission deleted.");
                    LoadPermissions();
                    AppSecurityContext.RefreshUserClaims();
                }
                else
                {
                    MessageBox.Show("Permission not deleted.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting permission.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtPermissionName.Clear();
            txtSearch.Clear();
            gridPermissions.ClearSelection();
        }

        private void gridPermissions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var name = gridPermissions.Rows[e.RowIndex].Cells[NameColumn].Value?.ToString();
            txtPermissionName.Text = name ?? string.Empty;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var keyword = txtSearch.Text.Trim();
                var dt = string.IsNullOrWhiteSpace(keyword) ? _bll.GetAll() : _bll.Search(keyword);
                gridPermissions.DataSource = dt;
                lblCount.Text = $"Total: {dt.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HydrateAuthRolesIfNeeded()
        {
            AppSecurityContext.RefreshUserClaims();
        }
    }

    partial class FrmPermissions
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView gridPermissions;
        private TextBox txtPermissionName;
        private TextBox txtSearch;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
        private Button btnSearch;
        private Label lblName;
        private Label lblSearch;
        private Label lblCount;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.gridPermissions = new DataGridView();
            this.txtPermissionName = new TextBox();
            this.txtSearch = new TextBox();
            this.btnAdd = new Button();
            this.btnUpdate = new Button();
            this.btnDelete = new Button();
            this.btnClear = new Button();
            this.btnSearch = new Button();
            this.lblName = new Label();
            this.lblSearch = new Label();
            this.lblCount = new Label();

            this.SuspendLayout();

            // gridPermissions
            this.gridPermissions.AllowUserToAddRows = false;
            this.gridPermissions.AllowUserToDeleteRows = false;
            this.gridPermissions.ReadOnly = true;
            this.gridPermissions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.gridPermissions.Left = 12;
            this.gridPermissions.Top = 80;
            this.gridPermissions.Width = 560;
            this.gridPermissions.Height = 320;
            this.gridPermissions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gridPermissions.MultiSelect = false;
            this.gridPermissions.CellClick += new DataGridViewCellEventHandler(this.gridPermissions_CellClick);

            // txtPermissionName
            this.txtPermissionName.Left = 110;
            this.txtPermissionName.Top = 12;
            this.txtPermissionName.Width = 300;

            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Left = 12;
            this.lblName.Top = 15;
            this.lblName.Text = "Permission:";

            // btnAdd
            this.btnAdd.Text = "Add";
            this.btnAdd.Left = 420;
            this.btnAdd.Top = 10;
            this.btnAdd.Width = 70;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            // btnUpdate
            this.btnUpdate.Text = "Update";
            this.btnUpdate.Left = 502;
            this.btnUpdate.Top = 10;
            this.btnUpdate.Width = 70;
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);

            // btnDelete
            this.btnDelete.Text = "Delete";
            this.btnDelete.Left = 420;
            this.btnDelete.Top = 40;
            this.btnDelete.Width = 70;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            // btnClear
            this.btnClear.Text = "Clear";
            this.btnClear.Left = 502;
            this.btnClear.Top = 40;
            this.btnClear.Width = 70;
            this.btnClear.Click += new EventHandler(this.btnClear_Click);

            // txtSearch
            this.txtSearch.Left = 110;
            this.txtSearch.Top = 45;
            this.txtSearch.Width = 300;

            // lblSearch
            this.lblSearch.AutoSize = true;
            this.lblSearch.Left = 12;
            this.lblSearch.Top = 48;
            this.lblSearch.Text = "Search:";

            // btnSearch
            this.btnSearch.Text = "Search";
            this.btnSearch.Left = 12;
            this.btnSearch.Top = 410;
            this.btnSearch.Width = 80;
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);

            // lblCount
            this.lblCount.AutoSize = true;
            this.lblCount.Left = 110;
            this.lblCount.Top = 415;
            this.lblCount.Text = "Total: 0";

            // Form
            this.ClientSize = new System.Drawing.Size(584, 451);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtPermissionName);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.gridPermissions);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblCount);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Permissions";
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(this.FrmPermissions_KeyDown);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}