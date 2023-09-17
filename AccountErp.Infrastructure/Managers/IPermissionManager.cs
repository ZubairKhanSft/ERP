using AccountErp.Dtos.Permission;
using AccountErp.Models.Permission;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface IPermissionManager
    {
        Task AddAsync(PermissionAddModel model);
        Task EditAsync(PermissionEditModel model);

        Task<PermissionDto> GetDetailAsync(int id);
        Task<PermissionDto> GetDetailByScreenAsync(int id);

        Task<List<PermissionDto>> GetAllPaginationAsync(int PageSize, int Page);
        Task<List<PermissionDto>> GetAllAsync();
        Task<int> GetAllCount();
        Task DeleteAsync(int id);
    }
}
