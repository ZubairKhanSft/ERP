using AccountErp.Dtos.Permission;
using AccountErp.Dtos.Screen;
using AccountErp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
    public interface IScreenRepository
    {
        Task AddAsync(Screen entity);

        void Edit(Screen entity);
        Task<Screen> GetAsync(int id);
        Task<ScreenDto> GetDetailAsync(int id);

        Task<List<ScreenDetailDto>> GetAllAsync(int PageSize, int Page);
        Task<int> GetAllCount();
        Task DeleteAsync(int id);
        Task<List<ScreenPermissionDetailDto>> GetAllScreenPermissionAsync();
        Task<List<PermissionDto>> GetAllPermissionAsync(int id);
    }
}
