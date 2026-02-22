using pos.Security.Authorization;
using POS.Core;
using System;
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
            };

            using (BusyScope.Show(this, UiMessages.T("Loading permissions...", "Ã«—Ì  Õ„Ì· «·’·«ÕÌ« ...")))
            {
                // Load roles
                cmbRoles.DataSource = Enum.GetValues(typeof(SystemRole));
                LoadAllPermissions();
                if (cmbRoles.Items.Count > 0) cmbRoles.SelectedIndex = 0;
            }

            // Keep UI in sync when role changes
            LoadPermissionsForRole();
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

        private void cmbRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPermissionsForRole();
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
                    var selected = checkedListBoxPermissions.CheckedItems.Cast<object>()
                        .Select(o => o.ToString())
                        .ToList();

                    _repo.SaveRolePermissions(role, selected);

                    var def = new RoleDefinition { Role = role };
                    def.GrantedPermissions.UnionWith(selected);
                    _auth.OverrideRole(def);

                    UiMessages.ShowInfo(
                        UiMessages.T("Role permissions updated.", " „  ÕœÌÀ ’·«ÕÌ«  «·œÊ—."),
                        UiMessages.T("Role permissions updated.", " „  ÕœÌÀ ’·«ÕÌ«  «·œÊ—."),
                        captionEn: "Security",
                        captionAr: "«·√„«‰");

                    //App logging 
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