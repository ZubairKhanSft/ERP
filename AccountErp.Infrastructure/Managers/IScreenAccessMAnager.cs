using AccountErp.Dtos.RolePermission;
using AccountErp.Dtos.Screen;
using AccountErp.Dtos.ScreenAccess;
using AccountErp.Models.ScreenAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface IScreenAccessMAnager
    {
        Task AddUserScreenAccessAsync(ScreenAccessModel model);
        Task<List<ScreenAccessDto>> GetUserScreenAccessById(int id);
        Task<List<RolePermissionDto>> GetUserPermissionAccessById(int id);
        // Task<List<ScreenPermiByRoleIdDto>> GetScreenPermissionByRoleId(int roleid);
        Task<List<ScreenDto>> GetAllScreenDetail();
    }
}
