using AccountErp.Dtos.ProductCategory;
using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.ProductCategory;
using AccountErp.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using AccountErp.Dtos;

namespace AccountErp.DataLayer.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly DataContext _dataContext;

        public ProductCategoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(ProductCategory entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public void Edit(ProductCategory entity)
        {
            _dataContext.Update(entity);
        }

        public async Task<ProductCategory> GetAsync(int id)
        {
            return await _dataContext.ProductCategory.FindAsync(id);
        }

        public async Task<ProductCategoryDetailDto> GetDetailAsync(int id)
        {
            return await (from s in _dataContext.ProductCategory
                          where s.Id == id
                          select new ProductCategoryDetailDto
                          {
                              Id = s.Id,
                              Name = s.Name,
                              Status = s.Status
                          })
                          .AsNoTracking()
                          .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductCategoryDetailDto>> GetAllAsync(Constants.RecordStatus? status = null)
        {
            return await (from s in _dataContext.ProductCategory
                          where s.Status != Constants.RecordStatus.Deleted
                          select new ProductCategoryDetailDto
                          {
                              Id = s.Id,
                              Name = s.Name,
                              Status = s.Status
                          })
                          .AsNoTracking()
                            .ToListAsync();
        }

        public async Task<ProductCategoryDetailForEditDto> GetForEditAsync(int id)
        {
            return await (from s in _dataContext.ProductCategory
                          where s.Id == id
                          select new ProductCategoryDetailForEditDto
                          {
                              Id = s.Id,
                              Name = s.Name
                          })
                         .AsNoTracking()
                         .SingleOrDefaultAsync();
        }

        public async Task<JqDataTableResponse<ProductCategoryListItemDto>> GetPagedResultAsync(ProductCategoryJqDataTableRequestModel model)
        {
            if (model.Length == 0)
            {
                model.Length = Constants.DefaultPageSize;
            }

            var filterKey = model.Search.Value;

            var linqStmt = (from s in _dataContext.ProductCategory
                            where s.Status != Constants.RecordStatus.Deleted
                                && (model.FilterKey == null
                                || EF.Functions.Like(s.Name, "%" + model.FilterKey + "%"))
                            select new ProductCategoryListItemDto
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Status = s.Status
                            })
                            .AsNoTracking();

            var sortExpresstion = model.GetSortExpression();

            var pagedResult = new JqDataTableResponse<ProductCategoryListItemDto>
            {
                RecordsTotal = await _dataContext.ProductCategory.CountAsync(x => x.Status != Constants.RecordStatus.Deleted),
                RecordsFiltered = await linqStmt.CountAsync(),
                Data = await linqStmt.OrderBy(sortExpresstion).Skip(model.Start).Take(model.Length).ToListAsync()
            };
            return pagedResult;
        }

        public async Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync()
        {
            return await _dataContext.ProductCategory
                .AsNoTracking()
                .Where(x => x.Status == Constants.RecordStatus.Active)
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItemDto
                {
                    KeyInt = x.Id,
                    Value = x.Name
                }).ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _dataContext.ProductCategory.FindAsync(id);
            item.Status = Constants.RecordStatus.Deleted;
            _dataContext.ProductCategory.Update(item);

        }


    }
}