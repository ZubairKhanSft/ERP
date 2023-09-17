using AccountErp.Dtos;
using AccountErp.Dtos.Product;
using AccountErp.Dtos.ProductCategory;
using AccountErp.Models.ProductCategory;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface IProductCategoryManager
    {
        Task AddAsync(ProductCategoryAddModel model);

        Task EditAsync(ProductCategoryEditModel model);

        Task<ProductCategoryDetailDto> GetDetailAsync(int id);

        Task<ProductCategoryDetailForEditDto> GetForEditAsync(int id);

        Task<JqDataTableResponse<ProductCategoryListItemDto>> GetPagedResultAsync(ProductCategoryJqDataTableRequestModel model);

        Task<IEnumerable<ProductCategoryDetailDto>> GetAllAsync(Constants.RecordStatus? status = null);
        Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync();

        Task DeleteAsync(int id);


    }
}
