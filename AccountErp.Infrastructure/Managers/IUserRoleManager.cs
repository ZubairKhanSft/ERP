using AccountErp.Dtos;
using AccountErp.Dtos.UserLogin;
using AccountErp.Models.UserLogin;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface IUserRoleManager
    {
        Task AddAsync(UserRoleModel model);

        Task EditAsync(UserRoleModel model);

        Task<UserRoleDetailDto> GetDetailAsync(int id);

        Task<JqDataTableResponse<UserRoleDetailDto>> GetPagedResultAsync(JqDataTableRequest model);
        Task DeleteAsync(int id);
        Task<List<SelectListItemDto>> GetAllAsync();
        Task<List<UserRoleDetailDto>> GetAllAsync(int PageSize, int Page, string filterKey);
    }
}
