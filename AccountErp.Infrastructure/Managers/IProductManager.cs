using AccountErp.Dtos;
using AccountErp.Dtos.Product;
using AccountErp.Models.Product;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface IProductManager
    {
        Task AddAsync(ProductAddModel model);

        Task EditAsync(ProductEditModel model);

        Task<ProductDetailDto> GetDetailAsync(int id);
        Task<(List<ProductDetailDto>,int count)> GetAllAsync(int PageSize,int Page,string filterKey);
        Task<List<ProductDetailDto>> GetAllBrandAsync();
        Task<List<ProductDetailDto>> GetAllModelAsync(string brandName);
        Task<List<ProductDetailDto>> GetAllSpecificationAsync(string brandName,string modelName);
        Task ToggleStatusAsync(int id);

        Task DeleteAsync(int id);

        bool checkItemAvailable(int id);

        Task<JqDataTableResponse<ProductListItemDto>> GetInventoryPagedResultAsync(ProductInventoryJqDataTableRequestModel model);
        Task<List<ProductListItemDto>> GetInventoryAsync(DateTime? StartDate,DateTime? EndDate,string FilterKey,string Quantity);
       /* Task<ProductDetailForEditDto> GetForEditAsync(int id);

        Task<JqDataTableResponse<ProductListItemDto>> GetPagedResultAsync(ProductJqDataTableRequestModel model);



        Task<IEnumerable<ProductDetailDto>> GetAllAsync(Constants.RecordStatus? status = null);
*/
        //Task<IEnumerable<ProductDetailDto>> GetAllForSalesAsync(Constants.RecordStatus? status = null);


        //Task<IEnumerable<ProductDetailDto>> GetAllForExpenseAsync(Constants.RecordStatus? status = null);




        /* Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync();


         Task TransferWareHouse(int id, int wareHouseId);*/

    }
}
