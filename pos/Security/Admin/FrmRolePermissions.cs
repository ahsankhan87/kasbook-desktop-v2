using pos.Security.Authorization;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos.Security.Admin
{
    public partial class FrmRolePermissions : Form
    {
        private readonly IRoleRepository _repo;
        private readonly IAuthorizationService _auth;

        public FrmRolePermissions()
        {
            InitializeComponent();
            _repo = AppSecurityContext.RoleRepo ?? throw new InvalidOperationException("RoleRepo not configured.");
            _auth = AppSecurityContext.Auth;

            this.Load += (s, e) =>
            {
                AppTheme.Apply(this);
                Text = UiMessages.T("Role Permissions", "’·«ÕÌ«  «·√œÊ«—");
            };

            using (BusyScope.Show(this, UiMessages.T("Loading permissions...", "Ã«—Ì  Õ„Ì· «·’·«ÕÌ« ...")))
            {
                LoadAllPermissions();  // Move this BEFORE setting DataSource
                cmbRoles.DataSource = Enum.GetValues(typeof(SystemRole));
                if (cmbRoles.Items.Count > 0) cmbRoles.SelectedIndex = 0;
            }

            LoadPermissionsForRole();
        }

        private string[] _allPermissions;
        private readonly HashSet<string> _selectedPermissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private void LoadAllPermissions()
        {
            _allPermissions = typeof(Permissions)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.FieldType == typeof(string))
                .Select(f => (string)f.GetValue(null))
                .OrderBy(s => s)
                .ToArray();

            PopulateModuleFilter();
            ApplyPermissionFilter();
        }

        private void PopulateModuleFilter()
        {
            var modules = _allPermissions
                .Select(GetPermissionModule)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(s => s)
                .ToList();

            cmbModuleFilter.Items.Clear();
            cmbModuleFilter.Items.Add("All");
            foreach (var m in modules)
                cmbModuleFilter.Items.Add(m);

            cmbModuleFilter.SelectedIndex = 0;
        }

        private static string GetPermissionModule(string permission)
        {
            if (string.IsNullOrWhiteSpace(permission)) return string.Empty;
            int idx = permission.IndexOf('.');
            return idx > 0 ? permission.Substring(0, idx) : permission;
        }

        private void ApplyPermissionFilter()
        {
            var keyword = (txtSearch.Text ?? string.Empty).Trim();
            var module = cmbModuleFilter.SelectedItem == null ? "All" : cmbModuleFilter.SelectedItem.ToString();

            var filtered = _allPermissions.Where(p =>
                (string.Equals(module, "All", StringComparison.OrdinalIgnoreCase) ||
                 string.Equals(GetPermissionModule(p), module, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(keyword) || p.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0))
                .ToArray();

            checkedListBoxPermissions.Items.Clear();
            foreach (var p in filtered)
                checkedListBoxPermissions.Items.Add(p, _selectedPermissions.Contains(p));

            UpdateSelectionSummary();
        }

        private void LoadPermissionsForRole()
        {
            if (cmbRoles.SelectedItem == null) return;

            var role = (SystemRole)cmbRoles.SelectedItem;
            var def = _auth.GetRole(role);

            _selectedPermissions.Clear();
            if (def != null)
                _selectedPermissions.UnionWith(def.GrantedPermissions);

            ApplyPermissionFilter();
        }

        private void UpdateSelectionSummary()
        {
            lblSelectedCount.Text = string.Format(
                UiMessages.T("Selected: {0}", "«·„Õœœ: {0}"),
                _selectedPermissions.Count);
        }

        private void cmbRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPermissionsForRole();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyPermissionFilter();
        }

        private void cmbModuleFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyPermissionFilter();
        }

        private void checkedListBoxPermissions_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index < 0 || e.Index >= checkedListBoxPermissions.Items.Count) return;

            var permission = checkedListBoxPermissions.Items[e.Index].ToString();
            if (e.NewValue == CheckState.Checked)
                _selectedPermissions.Add(permission);
            else
                _selectedPermissions.Remove(permission);

            BeginInvoke((Action)(() => UpdateSelectionSummary()));
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxPermissions.Items.Count; i++)
            {
                var permission = checkedListBoxPermissions.Items[i].ToString();
                _selectedPermissions.Add(permission);
                checkedListBoxPermissions.SetItemChecked(i, true);
            }
            UpdateSelectionSummary();
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxPermissions.Items.Count; i++)
            {
                var permission = checkedListBoxPermissions.Items[i].ToString();
                _selectedPermissions.Remove(permission);
                checkedListBoxPermissions.SetItemChecked(i, false);
            }
            UpdateSelectionSummary();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbRoles.SelectedItem == null) return;
            var role = (SystemRole)cmbRoles.SelectedItem;

            using (BusyScope.Show(this, UiMessages.T("Saving permissions...", "Ã«—Ì Õ›Ÿ «·’·«ÕÌ« ...")))
            {
                try
                {
                    var selected = _selectedPermissions.ToList();

                    _repo.SaveRolePermissions(role, selected);

                    var def = new RoleDefinition { Role = role };
                    def.GrantedPermissions.UnionWith(selected);
                    _auth.OverrideRole(def);

                    UiMessages.ShowInfo(
                        UiMessages.T("Role permissions updated.", " „  ÕœÌÀ ’·«ÕÌ«  «·œÊ—."),
                        UiMessages.T("Role permissions updated.", " „  ÕœÌÀ ’·«ÕÌ«  «·œÊ—."),
                        captionEn: "Security",
                        captionAr: "«·√„«‰");

                    POS.DLL.Log.LogAction("Permissions", $"Updated permissions for role '{role}'.", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, "Œÿ√", "Error", "Œÿ√");
                }
            }
        }
    }
}