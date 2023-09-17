using AccountErp.Dtos.UserLogin;
using AccountErp.Entities;
using AccountErp.Models.UserLogin;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User entity);
        Task LoginAddAsync(LoginModule entity);
        void Edit(User entity);

        Task<User> GetAsync(int id);

        Task<UserDetailDto> GetDetailAsync(int id);

        Task<JqDataTableResponse<UserDetailDto>> GetPagedResultAsync(JqDataTableRequest model);

        Task DeleteAsync(int id);
        Task<UserDetailDto> GetByUserAsync(string email);
        Task<UserDetailDto> Login(UserLoginModel model);
        Task<JqDataTableResponse<UserDetailDto>> GetAgentPagedResultAsync(JqDataTableRequest model);
        Task LogOut(int id);
        Task<JqDataTableResponse<UserDetailDto>> GetOnlineAgentPagedResultAsync(JqDataTableRequest model);
        Task<JqDataTableResponse<UserDetailDto>> GetOnlyOnlineAgentPagedResultAsync(JqDataTableRequest model);
        Task<(List<UserDetailDto>, int count)> GetAllPagination(int PageSize, int Page, string filterKey);
        Task<int> GetRoleId(string roleName);
    }
}
