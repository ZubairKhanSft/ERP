using AccountErp.Dtos.CreditMemo;
using AccountErp.Models.CreditMemo;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
   public  interface ICreditMemoManager
    {
        Task AddAsync(CreditMemoAddModel model);

        Task<JqDataTableResponse<CreditMemoListItemDto>> GetPagedResultAsync(CreditMemoJqDataTableRequestModel model);

        Task<CreditMemoDetailDto> GetDetailAsync(int id);
        Task EditAsync(CreditMemoEditModel model);


    }
}
