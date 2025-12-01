using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace pos.Security.Authorization
{
    // Central authorization service; stateless beyond in-memory cache
    public interface IAuthorizationService
    {
        bool HasPermission(UserIdentity user, string permission);
        bool HasAll(UserIdentity user, params string[] permissions);
        bool HasAny(UserIdentity user, params string[] permissions);
        RoleDefinition GetRole(SystemRole role);
        void OverrideRole(RoleDefinition roleDefinition); // from DB/config at runtime
    }

    public sealed class AuthorizationService : IAuthorizationService
    {
        private readonly ConcurrentDictionary<SystemRole, RoleDefinition> _roleCache =
            new ConcurrentDictionary<SystemRole, RoleDefinition>();

        public AuthorizationService()
        {
            // Default role policy; replace from DB if available via OverrideRole.
            SeedDefaults();
        }

        public bool HasPermission(UserIdentity user, string permission)
        {
            if (user == null || string.IsNullOrWhiteSpace(permission)) return false;

            // Owner short-circuit
            if (user.Role == SystemRole.Owner) return true;

            // User claims allow fine-grained overrides
            if (user.Claims.Contains(permission)) return true;

            var role = GetRole(user.Role);
            return role != null && role.GrantedPermissions.Contains(permission);
        }

        public bool HasAll(UserIdentity user, params string[] permissions)
        {
            if (permissions == null || permissions.Length == 0) return true;
            return permissions.All(p => HasPermission(user, p));
        }

        public bool HasAny(UserIdentity user, params string[] permissions)
        {
            if (permissions == null || permissions.Length == 0) return true;
            return permissions.Any(p => HasPermission(user, p));
        }

        public RoleDefinition GetRole(SystemRole role)
        {
            _roleCache.TryGetValue(role, out var def);
            return def;
        }

        public void OverrideRole(RoleDefinition roleDefinition)
        {
            if (roleDefinition == null) return;
            _roleCache[roleDefinition.Role] = roleDefinition;
        }

        private void SeedDefaults()
        {
            _roleCache[SystemRole.Owner] = new RoleDefinition
            {
                Role = SystemRole.Owner,
            };

            _roleCache[SystemRole.Administrator] = new RoleDefinition
            {
                Role = SystemRole.Administrator,
            };
            _roleCache[SystemRole.Administrator].GrantedPermissions.UnionWith(new[]
            {
                Permissions.Sales_Create, Permissions.Sales_Edit, Permissions.Sales_Delete,
                Permissions.Sales_View, Permissions.Sales_Print,
                Permissions.Sales_Zatca_Sign, Permissions.Sales_Zatca_Report,
                Permissions.Sales_Zatca_Qr_Show, Permissions.Sales_Zatca_DownloadUBL,
                Permissions.Customers_View, Permissions.Customers_Edit,
                Permissions.Finance_View, Permissions.Finance_Report,
                Permissions.Inventory_View, Permissions.Inventory_Edit
            });

            _roleCache[SystemRole.Manager] = new RoleDefinition
            {
                Role = SystemRole.Manager,
            };
            _roleCache[SystemRole.Manager].GrantedPermissions.UnionWith(new[]
            {
                Permissions.Sales_Create, Permissions.Sales_Edit, Permissions.Sales_View, Permissions.Sales_Print,
                Permissions.Sales_Zatca_Sign, Permissions.Sales_Zatca_Report, Permissions.Sales_Zatca_Qr_Show,
                Permissions.Customers_View, Permissions.Customers_Edit,
                Permissions.Finance_View,
                Permissions.Inventory_View
            });

            _roleCache[SystemRole.User] = new RoleDefinition
            {
                Role = SystemRole.User,
            };
            _roleCache[SystemRole.User].GrantedPermissions.UnionWith(new[]
            {
                Permissions.Sales_Create, Permissions.Sales_Edit, Permissions.Sales_View, Permissions.Sales_Print,
                Permissions.Sales_Zatca_Sign, Permissions.Sales_Zatca_Report, Permissions.Sales_Zatca_Qr_Show,
                Permissions.Customers_View, Permissions.Customers_Edit,
                Permissions.Finance_View,
                Permissions.Inventory_View
            });

            _roleCache[SystemRole.Cashier] = new RoleDefinition
            {
                Role = SystemRole.Cashier,
            };
            _roleCache[SystemRole.Cashier].GrantedPermissions.UnionWith(new[]
            {
                Permissions.Sales_Create, Permissions.Sales_View, Permissions.Sales_Print,
                Permissions.Sales_Zatca_Qr_Show,
                Permissions.Customers_View,
                Permissions.Inventory_View
            });

            _roleCache[SystemRole.SalesRep] = new RoleDefinition
            {
                Role = SystemRole.SalesRep,
            };
            _roleCache[SystemRole.SalesRep].GrantedPermissions.UnionWith(new[]
            {
                Permissions.Sales_Create, Permissions.Sales_View, Permissions.Sales_Print,
                Permissions.Customers_View,
            });

            _roleCache[SystemRole.Accountant] = new RoleDefinition
            {
                Role = SystemRole.Accountant,
            };
            _roleCache[SystemRole.Accountant].GrantedPermissions.UnionWith(new[]
            {
                Permissions.Sales_View, Permissions.Sales_Print,
                Permissions.Finance_View, Permissions.Finance_Report
            });

            _roleCache[SystemRole.Viewer] = new RoleDefinition
            {
                Role = SystemRole.Viewer,
            };
            _roleCache[SystemRole.Viewer].GrantedPermissions.UnionWith(new[]
            {
                Permissions.Sales_View,
                Permissions.Customers_View,
                Permissions.Inventory_View
            });
        }
    }
}