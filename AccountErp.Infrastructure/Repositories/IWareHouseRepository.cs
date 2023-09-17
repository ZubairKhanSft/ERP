using AccountErp.Dtos.WareHouse;
using AccountErp.Entities;
using AccountErp.Models.WareHouse;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
  public  interface IWareHouseRepository
    {
        Task AddAsync(WareHouse entity);

        void Edit(WareHouse entity);

        Task<WareHouse> GetAsync(int id);

        Task<WareHouseDetailsDto> GetDetailAsync(int id);

        Task DeleteAsync(int id);

        Task<JqDataTableResponse<WareHouseDetailsListDto>> GetPagedResultAsync(WareHouseJqDataTableRequestModel model);

        Task<IEnumerable<WareHouseDetailsDto>> GetAllAsync(Constants.RecordStatus? status = null);
        Task<List<WareHouseDetailsDto>> GetAllAsync(int PageSize, int Page);
    }
}
