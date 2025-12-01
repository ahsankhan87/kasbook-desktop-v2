using System.Collections.Generic;

namespace pos.Security.Authorization
{
    public static class AppSecurityContext
    {
        public static IAuthorizationService Auth { get; } = new AuthorizationService();
        public static IRoleRepository RoleRepo { get; set; }

        public static UserIdentity User { get; private set; }

        public static void SetUser(UserIdentity user)
        {
            User = user;
            RefreshUserClaims();
        }

        // Hydrate roles from DB on app startup
        public static void HydrateFromDb()
        {
            if (RoleRepo == null) return;
            foreach (var role in RoleRepo.LoadAllRoles())
                Auth.OverrideRole(role);
        }

        public static void RefreshUserClaims()
        {
            if (User == null || RoleRepo == null) return;
            User.Claims.Clear();
            foreach (var claim in RoleRepo.LoadUserClaims(User.UserId))
                User.Claims.Add(claim);
        }
    }
}