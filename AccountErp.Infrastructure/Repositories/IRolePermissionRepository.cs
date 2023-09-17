using AccountErp.Dtos.RolePermission;
using AccountErp.Entities;
using AccountErp.Models.RolePermission;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
    public interface IRolePermissionRepository
    {
        Task AddAsync(RolePermission entity);
        Task DeleteAsync(int id);
        Task DeleteAsync1(int id);

        RolePermissionDto isExist(AddRolePermission id);
    }
}
