using AccountErp.Dtos.UserLogin;
using AccountErp.Dtos;
using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AccountErp.Dtos.CurrencyCr;

namespace AccountErp.DataLayer.Repositories
{
    public class CurrencyCrRepository : ICurrencyCrRepository
    {
        private readonly DataContext _dataContext;
        public CurrencyCrRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(CurrencyCr entity)
        {
            await _dataContext.CurrencyCr.AddAsync(entity);
        }

        public void Edit(CurrencyCr entity)
        {
            _dataContext.CurrencyCr.Update(entity);
        }

        public async Task<CurrencyCr> GetAsync(int id)
        {
            return await _dataContext.CurrencyCr.FindAsync(id);
        }

        public async Task<CurrencyCrDetailDto> GetDetailAsync(int id)
        {
            return await (from s in _dataContext.CurrencyCr
                          where s.Id == id && s.Status != Constants.RecordStatus.Deleted
                          select new CurrencyCrDetailDto
                          {
                              Id = s.Id,
                              Name = s.Name,
                              UserId = s.UserId,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,
                              Symbol = s.Symbol,    
                          })
                          .AsNoTracking()
                          .SingleOrDefaultAsync();
        }
        /*public async Task<List<SelectListItemDto>> GetAllAsync()
        {
            return await (from s in _dataContext.UsersRoles
                          where s.Status != Constants.RecordStatus.Deleted

                          select new SelectListItemDto
                          {
                              KeyInt = s.Id,
                              Value = s.RoleName
                          })
                          .AsNoTracking()
                          .ToListAsync();
        }*/

        public async Task<List<CurrencyCrDetailDto>> GetAllAsync(int PageSize, int Page, string FilterKey)
        {
            if (PageSize != 0 && Page != 0)
            {
                return await (from s in _dataContext.CurrencyCr
                              where s.Status != Constants.RecordStatus.Deleted
                              && (FilterKey == null
                                || EF.Functions.Like(s.Id.ToString(), "%" + FilterKey + "%")
                                 || EF.Functions.Like(s.Name.ToString(), "%" + FilterKey + "%")
                                  )
                              select new CurrencyCrDetailDto
                              {
                                  Id = s.Id,
                                  Name = s.Name,
                                  UserId = s.UserId,
                                  Status = s.Status,
                                  CreatedOn = s.CreatedOn,
                                  UpdatedOn = s.UpdatedOn,
                                  Symbol = s.Symbol,
                              })
                          .AsNoTracking()
                          .OrderByDescending(s => s.Id)
                          .Skip((Page - 1) * PageSize)
                          .Take(PageSize)
                          .ToListAsync();
            }
            else
            {
                return await (from s in _dataContext.CurrencyCr
                              where s.Status != Constants.RecordStatus.Deleted
                              && (FilterKey == null
                                || EF.Functions.Like(s.Id.ToString(), "%" + FilterKey + "%")
                                 || EF.Functions.Like(s.Name.ToString(), "%" + FilterKey + "%")
                                  )
                              select new CurrencyCrDetailDto
                              {
                                  Id = s.Id,
                                  Name = s.Name,
                                  UserId = s.UserId,
                                  Status = s.Status,
                                  CreatedOn = s.CreatedOn,
                                  UpdatedOn = s.UpdatedOn,
                                  Symbol = s.Symbol,
                              })
                          .AsNoTracking()
                          .ToListAsync();
            }
        }
       
        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.CurrencyCr.FindAsync(id);
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.CurrencyCr.Update(data);
        }
    }
}
