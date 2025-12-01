using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using pos.Security.Authorization;

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

            // Load roles
            cmbRoles.DataSource = Enum.GetValues(typeof(SystemRole));
            LoadAllPermissions();
            if (cmbRoles.Items.Count > 0) cmbRoles.SelectedIndex = 0;
        }

        private string[] _allPermissions;

        private void LoadAllPermissions()
        {
            _allPermissions = typeof(Permissions)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(f => f.FieldType == typeof(string))
                .Select(f => (string)f.GetValue(null))
                .OrderBy(s => s)
                .ToArray();

            checkedListBoxPermissions.Items.Clear();
            foreach (var p in _allPermissions)
                checkedListBoxPermissions.Items.Add(p, false);
        }

        private void LoadPermissionsForRole()
        {
            if (cmbRoles.SelectedItem == null) return;
            var role = (SystemRole)cmbRoles.SelectedItem;
            var def = _auth.GetRole(role);

            // Uncheck all
            for (int i = 0; i < checkedListBoxPermissions.Items.Count; i++)
                checkedListBoxPermissions.SetItemChecked(i, false);

            if (def == null) return;

            for (int i = 0; i < checkedListBoxPermissions.Items.Count; i++)
            {
                var p = checkedListBoxPermissions.Items[i].ToString();
                if (def.GrantedPermissions.Contains(p))
                    checkedListBoxPermissions.SetItemChecked(i, true);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbRoles.SelectedItem == null) return;
            var role = (SystemRole)cmbRoles.SelectedItem;

            var selected = checkedListBoxPermissions.CheckedItems.Cast<object>()
                .Select(o => o.ToString())
                .ToList();

            _repo.SaveRolePermissions(role, selected);

            var def = new RoleDefinition { Role = role };
            def.GrantedPermissions.UnionWith(selected);
            _auth.OverrideRole(def);

            MessageBox.Show("Role permissions updated.", "Security", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}