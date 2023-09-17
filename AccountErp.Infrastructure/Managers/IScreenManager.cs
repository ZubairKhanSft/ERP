using AccountErp.Dtos.Screen;
using AccountErp.Models.Screen;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface IScreenManager
    {
        Task AddAsync(AddScreenModel model);
        Task EditAsync(AddScreenModel model);
        Task<ScreenDto> GetDetailAsync(int id);
        Task<List<ScreenDetailDto>> GetAllAsync(int PageSize, int Page);
        Task<int> GetAllCount();

        Task DeleteAsync(int id);
        Task<List<ScreenPermissionDetailDto>> GetAllScreenPermission();
    }
}
