using System.Linq;
using System.Windows.Forms;

namespace pos.Security.Authorization
{
    public static class FormSecurityExtensions
    {
        public static void ApplyPermissions(this Form form, IAuthorizationService auth, UserIdentity user)
        {
            if (form == null || auth == null || user == null) return;

            // 1) regular controls (recursively)
            ApplyControlTree(form, auth, user);

            // 2) toolstrips
            foreach (var ts in GetAllControls(form).OfType<ToolStrip>())
            {
                foreach (ToolStripItem item in ts.Items)
                {
                    if (item?.Tag is string permission && !string.IsNullOrWhiteSpace(permission))
                    {
                        bool allowed = auth.HasPermission(user, permission);
                        item.Enabled = allowed;
                        if (!allowed) item.Enabled = false;
                    }
                }
            }

            // 3) menus
            foreach (var menu in GetAllControls(form).OfType<MenuStrip>())
            {
                foreach (ToolStripMenuItem root in menu.Items)
                    ApplyMenuItemRecursive(root, auth, user);
            }

            // 4) DataGridViews (columns)
            foreach (var grid in GetAllControls(form).OfType<DataGridView>())
            {
                foreach (DataGridViewColumn col in grid.Columns)
                {
                    if (col?.Tag is string permission && !string.IsNullOrWhiteSpace(permission))
                    {
                        bool allowed = auth.HasPermission(user, permission);
                        // Hide column if not allowed (alternative: leave Enabled and gate actions only)
                        col.Visible = allowed;
                    }
                }
            }
        }

        private static void ApplyControlTree(Control parent, IAuthorizationService auth, UserIdentity user)
        {
            foreach (Control c in parent.Controls)
            {
                if (c?.Tag is string permission && !string.IsNullOrWhiteSpace(permission))
                {
                    bool allowed = auth.HasPermission(user, permission);
                    c.Enabled = allowed;
                    if (!allowed) c.Enabled = false;
                }

                if (c.HasChildren)
                    ApplyControlTree(c, auth, user);
            }
        }

        private static void ApplyMenuItemRecursive(ToolStripMenuItem item, IAuthorizationService auth, UserIdentity user)
        {
            if (item?.Tag is string permission && !string.IsNullOrWhiteSpace(permission))
            {
                bool allowed = auth.HasPermission(user, permission);
                item.Enabled = allowed;
                if (!allowed) item.Enabled = false;
            }

            foreach (ToolStripItem child in item.DropDownItems)
            {
                if (child is ToolStripMenuItem mi)
                    ApplyMenuItemRecursive(mi, auth, user);
            }
        }

        private static System.Collections.Generic.IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (Control c in root.Controls)
            {
                yield return c;
                foreach (var child in GetAllControls(c))
                    yield return child;
            }
        }
    }
}