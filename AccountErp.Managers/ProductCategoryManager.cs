using AccountErp.Dtos;
using AccountErp.Dtos.ProductCategory;
using AccountErp.Factories;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.ProductCategory;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Managers
{
    public class ProductCategoryManager : IProductCategoryManager
    {
        private readonly IProductCategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly string _userId;

        public ProductCategoryManager(IHttpContextAccessor contextAccessor,
            IProductCategoryRepository repository,
            IUnitOfWork unitOfWork)
        {
            _userId = contextAccessor.HttpContext.User.GetUserId();

            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(ProductCategoryAddModel model)
        {
            await _repository.AddAsync(ProductCategoryFactory.Create(model, _userId));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EditAsync(ProductCategoryEditModel model)
        {
            var item = await _repository.GetAsync(model.Id);
            ProductCategoryFactory.Create(model, item, _userId);
            _repository.Edit(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ProductCategoryDetailDto> GetDetailAsync(int id)
        {
            return await _repository.GetDetailAsync(id);
        }

        public async Task<JqDataTableResponse<ProductCategoryListItemDto>> GetPagedResultAsync(ProductCategoryJqDataTableRequestModel model)
        {
            return await _repository.GetPagedResultAsync(model);
        }

        public async Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync()
        {
            return await _repository.GetSelectItemsAsync();
        }

        public async Task<IEnumerable<ProductCategoryDetailDto>> GetAllAsync(Constants.RecordStatus? status = null)
        {
            return await _repository.GetAllAsync(status);
        }

        public async Task<ProductCategoryDetailForEditDto> GetForEditAsync(int id)
        {
            return await _repository.GetForEditAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
