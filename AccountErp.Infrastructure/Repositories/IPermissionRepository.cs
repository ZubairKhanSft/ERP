using AccountErp.Dtos.Permission;
using AccountErp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
    public interface IPermissionRepository
    {
        Task AddAsync(Permission entity);
        void Edit(Permission entity);
        Task<Permission> GetAsync(int id);


        Task<int> GetAllCount();
        Task<PermissionDto> GetDetailAsync(int id);
        Task<PermissionDto> GetScreenDetailAsync(int id);
        Task<List<PermissionDto>> GetAllPaginationAsync(int PageSize, int Page);
        Task<List<PermissionDto>> GetAllAsync();

        Task DeleteAsync(int id);
    }
}
