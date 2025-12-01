using System;
using System.Windows.Forms;

namespace pos.Security.Authorization
{
    // Decorate forms/controls to enforce permission on load
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class RequirePermissionAttribute : Attribute
    {
        public string Permission { get; }
        public bool HideIfDenied { get; set; } = true; // else disable only

        public RequirePermissionAttribute(string permission)
        {
            Permission = permission;
        }

        public static void Apply(Control root, UserIdentity user, IAuthorizationService auth)
        {
            if (root == null || auth == null) return;

            foreach (Control c in root.Controls)
            {
                var field = root.GetType().GetField(c.Name,
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                if (field != null)
                {
                    var attrs = (RequirePermissionAttribute[])Attribute.GetCustomAttributes(field, typeof(RequirePermissionAttribute), inherit: true);
                    foreach (var attr in attrs)
                    {
                        bool allowed = auth.HasPermission(user, attr.Permission);
                        if (!allowed)
                        {
                            if (attr.HideIfDenied) c.Visible = false;
                            else c.Enabled = false;
                        }
                    }
                }
                else
                {
                    var property = root.GetType().GetProperty(c.Name,
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                    if (property != null)
                    {
                        var attrs = (RequirePermissionAttribute[])Attribute.GetCustomAttributes(property, typeof(RequirePermissionAttribute), inherit: true);
                        foreach (var attr in attrs)
                        {
                            bool allowed = auth.HasPermission(user, attr.Permission);
                            if (!allowed)
                            {
                                if (attr.HideIfDenied) c.Visible = false;
                                else c.Enabled = false;
                            }
                        }
                    }
                }

                // Recurse
                Apply(c, user, auth);
            }
        }
    }
}