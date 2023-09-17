using AccountErp.Dtos;
using AccountErp.Dtos.Product;
using AccountErp.Entities;
using AccountErp.Models.Item;
using AccountErp.Models.Product;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product entity);

        void Edit(Product entity);

        Task<Product> GetAsync(int id);
        Task<ProductDetailDto> GetDetailAsync(int id);
        Task<(List<ProductDetailDto>, int count)> GetAllAsync(int PageSize, int Page, string filterKey);
        Task<List<ProductDetailDto>> GetAllBrandAsync();
        Task<List<ProductDetailDto>> GetAllModelAsync(string brandName);
        Task<List<ProductDetailDto>> GetAllSpecificationAsync(string brandName,string modelName);
        Task DeleteAsync(int id);
        bool checkItemAvailable(int id);
        Task ToggleStatusAsync(int id);
        int InvoiceProductCount(int id, DateTime? startDate, DateTime? endDate);
        int BillProductCount(int id, DateTime? startDate, DateTime? endDate);
        int CreditMemoProductCount(int id, DateTime? startDate, DateTime? endDate);
       // Task<IEnumerable<ProductDetailDto>> GetAllAsync(Constants.RecordStatus? status = null);
       // Task<IEnumerable<Product>> GetAsync(List<int> itemIds);



        //Task<IEnumerable<ProductDetailDto>> GetAllForSalesAsync(Constants.RecordStatus? status = null);
        //Task<IEnumerable<ProductDetailDto>> GetAllForExpenseAsync(Constants.RecordStatus? status = null);

       /* Task<ProductDetailForEditDto> GetForEditAsync(int id);

        Task<JqDataTableResponse<ProductListItemDto>> GetPagedResultAsync(ProductJqDataTableRequestModel model);



        Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync();


        Task TransferWareHouse(int id, int warehouseId);*/
        Task<JqDataTableResponse<ProductListItemDto>> GetInventoryPagedResultAsync(ProductInventoryJqDataTableRequestModel model);
        Task<List<ProductListItemDto>> GetInventoryAsync(DateTime? StartDate,DateTime? EndDate, string FilterKey);
    }
}
