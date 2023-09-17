using AccountErp.Dtos.UserLogin;
using AccountErp.Models.UserLogin;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface IUserManager
    {
        Task AddAsync(UserLoginDto model);
        Task LoginAddAsync(UserDetailDto model);
        Task EditAsync(UserLoginDto model);

        Task<UserDetailDto> GetDetailAsync(int id);

        Task<JqDataTableResponse<UserDetailDto>> GetPagedResultAsync(JqDataTableRequest model);
        Task<(List<UserDetailDto>,int count)> GetAllPagination(int PageSize, int Page, string filterKey);
        Task DeleteAsync(int id);
        Task<UserDetailDto> CheckUser(string email);
        Task<UserDetailDto> Login(UserLoginModel model);
        Task<JqDataTableResponse<UserDetailDto>> GetAgentPagedResultAsync(JqDataTableRequest model);
        Task LogOut(int id);
        Task<JqDataTableResponse<UserDetailDto>> GetOnlineAgentPagedResultAsync(JqDataTableRequest model);
        Task<JqDataTableResponse<UserDetailDto>> GetOnlyOnlineAgentPagedResultAsync(JqDataTableRequest model);
        Task<int> GetRoleId(string roleName);
    }
}
