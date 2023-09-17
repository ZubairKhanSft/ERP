using AccountErp.Dtos.Permission;
using AccountErp.Dtos.Screen;
using AccountErp.Dtos.UserScreenPermissionAccess;
using AccountErp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
    public interface IUserScreenPermissionAccessRepository
    {
        Task AddAsync(List<UserScreenPermissionAccess> entity);
        Task<List<UserScreenPermissionAccessDetailDto>> GetByRoleId(int id);
        Task<ScreenDto> GetAllScreenPermissionAsync(int ScreenId);
        Task<PermissionDto> GetPermissionById(int PermissionId);
        Task DeleteUserScreenAccess(int roleid);
    }
}
