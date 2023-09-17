using AccountErp.Dtos;
using AccountErp.Dtos.UserLogin;
using AccountErp.Entities;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
    public interface IUserRoleRepository
    {
        Task AddAsync(UserRole entity);

        void Edit(UserRole entity);

        Task<UserRole> GetAsync(int id);

        Task<UserRoleDetailDto> GetDetailAsync(int id);

        Task<JqDataTableResponse<UserRoleDetailDto>> GetPagedResultAsync(JqDataTableRequest model);

        Task DeleteAsync(int id);
        Task<List<SelectListItemDto>> GetAllAsync();
        Task<List<UserRoleDetailDto>> GetAllAsync(int PageSize,int Page, string filterKey);
    }
}
