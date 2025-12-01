using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using pos.Security.Authorization;

namespace pos.Security.Admin
{
    public partial class FrmUserClaims : Form
    {
        private readonly IRoleRepository _repo;

        public FrmUserClaims()
        {
            InitializeComponent();
            _repo = AppSecurityContext.RoleRepo ?? throw new InvalidOperationException("RoleRepo not configured.");
            LoadAllPermissions();
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

            checkedListBoxClaims.Items.Clear();
            foreach (var p in _allPermissions)
                checkedListBoxClaims.Items.Add(p, false);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var uid = (int)numUserId.Value;
            var current = _repo.LoadUserClaims(uid).ToHashSet(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < checkedListBoxClaims.Items.Count; i++)
            {
                var p = checkedListBoxClaims.Items[i].ToString();
                checkedListBoxClaims.SetItemChecked(i, current.Contains(p));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var uid = (int)numUserId.Value;
            var selected = checkedListBoxClaims.CheckedItems.Cast<object>()
                .Select(o => o.ToString())
                .ToList();

            _repo.SaveUserClaims(uid, selected);

            // If updating the current logged-in user, refresh claims
            if (AppSecurityContext.User != null && AppSecurityContext.User.UserId == uid)
                AppSecurityContext.RefreshUserClaims();

            MessageBox.Show("User claims updated.", "Security", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}