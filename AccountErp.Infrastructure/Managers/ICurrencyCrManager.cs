using AccountErp.Dtos.UserLogin;
using AccountErp.Dtos;
using AccountErp.Models.UserLogin;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AccountErp.Models.CurrencyCr;
using AccountErp.Dtos.CurrencyCr;

namespace AccountErp.Infrastructure.Managers
{
    public interface ICurrencyCrManager
    {
        Task AddAsync(AddCurrencyCrModel model);

        Task EditAsync(AddCurrencyCrModel model);

        Task<CurrencyCrDetailDto> GetDetailAsync(int id);

       /* Task<JqDataTableResponse<UserRoleDetailDto>> GetPagedResultAsync(JqDataTableRequest model);*/
        Task DeleteAsync(int id);
       // Task<List<SelectListItemDto>> GetAllAsync();
        Task<List<CurrencyCrDetailDto>> GetAllAsync(int PageSize, int Page, string filterKey);
    }
}
