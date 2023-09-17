using AccountErp.Dtos.RolePermission;
using AccountErp.Models.RolePermission;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface IRolePermissionManager
    {
        Task AddAsync(AddRolePermission model);
        Task DeleteAsync(int id);
        Task DeleteAsync2(int id);
        RolePermissionDto isExist(AddRolePermission id);
    }
}
