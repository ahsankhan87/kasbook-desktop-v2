using System;
using System.Collections.Generic;

namespace pos.Security.Authorization
{
    // System roles; align these with your DB/user profile
    public enum SystemRole
    {
        Owner,
        Administrator,
        Manager,
        User,
        Cashier,
        SalesRep,
        Accountant,
        Viewer
    }

    // Canonical permission keys (flat list – easy to store and query)
    public static class Permissions
    {
        public const string Sales_Create = "sales.create";
        public const string Sales_Edit = "sales.edit";
        public const string Sales_Delete = "sales.delete";
        public const string Sales_View = "sales.view";
        public const string Sales_Print = "sales.print";

        public const string Sales_Zatca_Sign = "sales.zatca.sign";
        public const string Sales_Zatca_Report = "sales.zatca.report";
        public const string Sales_Zatca_Qr_Show = "sales.zatca.qr.show";
        public const string Sales_Zatca_DownloadUBL = "sales.zatca.ubl.download";

        public const string Customers_View = "customers.view";
        public const string Customers_Edit = "customers.edit";

        public const string Finance_View = "finance.view";
        public const string Finance_Report = "finance.report";

        public const string Inventory_View = "inventory.view";
        public const string Inventory_Edit = "inventory.edit";
    }

    public sealed class UserIdentity
    {
        public int UserId { get; set; }
        public int BranchId { get; set; }
        public string Username { get; set; }
        public SystemRole Role { get; set; }
        public HashSet<string> Claims { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // per-user overrides
    }

    public sealed class RoleDefinition
    {
        public SystemRole Role { get; set; }
        public HashSet<string> GrantedPermissions { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    }
}