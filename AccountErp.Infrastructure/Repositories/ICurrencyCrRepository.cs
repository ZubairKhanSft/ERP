using AccountErp.Dtos.UserLogin;
using AccountErp.Dtos;
using AccountErp.Entities;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AccountErp.Dtos.CurrencyCr;

namespace AccountErp.Infrastructure.Repositories
{
    public interface ICurrencyCrRepository
    {
        Task AddAsync(CurrencyCr entity);

        void Edit(CurrencyCr entity);

        Task<CurrencyCr> GetAsync(int id);

        Task<CurrencyCrDetailDto> GetDetailAsync(int id);

       // Task<JqDataTableResponse<UserRoleDetailDto>> GetPagedResultAsync(JqDataTableRequest model);

        Task DeleteAsync(int id);
       // Task<List<SelectListItemDto>> GetAllAsync();
        Task<List<CurrencyCrDetailDto>> GetAllAsync(int PageSize, int Page, string filterKey);
    }
}
