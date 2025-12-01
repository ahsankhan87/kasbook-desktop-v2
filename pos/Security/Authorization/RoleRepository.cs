using System;
using System.Collections.Generic;

namespace pos.Security.Authorization
{
    // Implemented by SQL repository below
    public interface IRoleRepository
    {
        IEnumerable<RoleDefinition> LoadAllRoles();
        void SaveRolePermissions(SystemRole role, IEnumerable<string> permissions);

        IEnumerable<string> LoadUserClaims(int userId);
        void SaveUserClaims(int userId, IEnumerable<string> claims);
    }
}