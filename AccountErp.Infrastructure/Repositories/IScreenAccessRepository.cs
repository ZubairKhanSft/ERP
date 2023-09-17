using AccountErp.Dtos.RolePermission;
using AccountErp.Dtos.Screen;
using AccountErp.Dtos.ScreenAccess;
using AccountErp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
    public interface IScreenAccessRepository
    {
        Task AddUserScreenAccessAsync(List<ScreenAccess> entity);
        Task<List<ScreenAccessDto>> GetAsyncUserScreenAccess(int id);
        Task<List<RolePermissionDto>> GetAsyncUserPermissionAccess(int id);
        Task DeleteAsyncUserScreenAccess(int id);
        Task<List<ScreenDto>> GetAllScreenDetail();
        Task<List<ScreenDto>> GetAllScreen();
    }
}
