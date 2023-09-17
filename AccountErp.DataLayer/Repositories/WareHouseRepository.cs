using AccountErp.Dtos.WareHouse;
using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AccountErp.Utilities;
using AccountErp.Models.WareHouse;
using System.Linq.Dynamic.Core;

namespace AccountErp.DataLayer.Repositories
{
    public class WareHouseRepository : IWareHouseRepository
    {
        private readonly DataContext _dataContext;

        public WareHouseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(WareHouse entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public void Edit(WareHouse entity)
        {
            _dataContext.Update(entity);
        }
        public async Task<WareHouse> GetAsync(int id)
        {
            return await _dataContext.WareHouse.FindAsync(id);
        }

        public async Task<WareHouseDetailsDto> GetDetailAsync(int id)
        {
            return await (from s in _dataContext.WareHouse
                          where s.Id == id
                          select new WareHouseDetailsDto
                          {
                              Id = s.Id,
                              Name = s.Name,
                              Status = s.Status,
                              Location = s.Location,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
                              CreatedBy = s.CreatedBy,
                              UpdatedBy = s.UpdatedBy


                          })
                          .AsNoTracking()
                          .SingleOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var warehouse = await _dataContext.WareHouse.FindAsync(id);
            warehouse.Status = Constants.RecordStatus.Deleted;
            _dataContext.WareHouse.Update(warehouse);

        }
        public async Task<JqDataTableResponse<WareHouseDetailsListDto>> GetPagedResultAsync(WareHouseJqDataTableRequestModel model)
        {
            if (model.Length == 0)
            {
                model.Length = Constants.DefaultPageSize;
            }

            var filterKey = model.Search.Value;

            var linqStmt = (from s in _dataContext.WareHouse
                            where s.Status != Constants.RecordStatus.Deleted
                                && (model.FilterKey == null
                                || EF.Functions.Like(s.Name, "%" + model.FilterKey + "%"))


                            select new WareHouseDetailsListDto
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Location = s.Location,
                                CreatedOn = s.CreatedOn,
                                Status = s.Status


                            })
                            .AsNoTracking();

            var sortExpresstion = model.GetSortExpression();

            var pagedResult = new JqDataTableResponse<WareHouseDetailsListDto>
            {
                RecordsTotal = await _dataContext.WareHouse.CountAsync(x => x.Status != Constants.RecordStatus.Deleted),
                RecordsFiltered = await linqStmt.CountAsync(),
                Data = await linqStmt.OrderBy(sortExpresstion).Skip(model.Start).Take(model.Length).ToListAsync()
            };
            return pagedResult;
        }

        public async Task<IEnumerable<WareHouseDetailsDto>> GetAllAsync(Constants.RecordStatus? status = null)
        {
            return await (from s in _dataContext.WareHouse
                          where status == null
                            ? s.Status != Constants.RecordStatus.Deleted
                            : s.Status == status.Value
                          orderby s.Name
                          select new WareHouseDetailsDto
                          {
                              Id = s.Id,
                              Name = s.Name,
                              Location=s.Location,
                              Status=s.Status,
                              CreatedOn=s.CreatedOn,
                              CreatedBy=s.CreatedBy,
                              UpdatedOn=s.UpdatedOn,
                              UpdatedBy=s.UpdatedBy
                          })
                          .AsNoTracking()
                            .ToListAsync();
        }

        public async Task<List<WareHouseDetailsDto>> GetAllAsync(int PageSize,int Page)
        {
            if (PageSize !=0 && Page !=0)
            {
                return await(from s in _dataContext.WareHouse
                             where s.Status != Constants.RecordStatus.Deleted
                             select new WareHouseDetailsDto
                             {
                                 Id = s.Id,
                                 Name = s.Name,
                                 Location = s.Location,
                                 Status = s.Status,
                                 CreatedOn = s.CreatedOn,
                                 CreatedBy = s.CreatedBy,
                                 UpdatedOn = s.UpdatedOn,
                                 UpdatedBy = s.UpdatedBy
                             })
                             .AsNoTracking()
                             .OrderByDescending(s => s.Id)
                             .Skip((Page - 1) * PageSize)
                             .Take(PageSize)
                             .ToListAsync();
            }
            else
            {
                return await (from s in _dataContext.WareHouse
                              where s.Status != Constants.RecordStatus.Deleted
                              select new WareHouseDetailsDto
                              {
                                  Id = s.Id,
                                  Name = s.Name,
                                  Location = s.Location,
                                  Status = s.Status,
                                  CreatedOn = s.CreatedOn,
                                  CreatedBy = s.CreatedBy,
                                  UpdatedOn = s.UpdatedOn,
                                  UpdatedBy = s.UpdatedBy
                              })
                            .AsNoTracking()
                            .OrderByDescending(s => s.Id)
                            .ToListAsync();
            }
        }

    }
}

