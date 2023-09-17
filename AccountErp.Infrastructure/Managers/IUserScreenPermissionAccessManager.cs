using AccountErp.Dtos.UserScreenPermissionAccess;
using AccountErp.Models.UserScreenPermissionAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface IUserScreenPermissionAccessManager
    {
        Task AddAsync(AddUserScreenPermissionAccessModel model);
        Task<List<ScreensWithPermission>> GetByRoleId(int id);
    }
}
