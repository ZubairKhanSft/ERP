using AccountErp.Dtos;
using AccountErp.Dtos.ProductCategory;
using AccountErp.Entities;
using AccountErp.Models.ProductCategory;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
    public interface IProductCategoryRepository
    {
        Task AddAsync(ProductCategory entity);

        void Edit(ProductCategory entity);

        Task<ProductCategory> GetAsync(int id);


        Task<ProductCategoryDetailDto> GetDetailAsync(int id);

        Task<IEnumerable<ProductCategoryDetailDto>> GetAllAsync(Constants.RecordStatus? status = null);

        Task<ProductCategoryDetailForEditDto> GetForEditAsync(int id);

        Task<JqDataTableResponse<ProductCategoryListItemDto>> GetPagedResultAsync(ProductCategoryJqDataTableRequestModel model);

        Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync();

        Task DeleteAsync(int id);
    }
}
