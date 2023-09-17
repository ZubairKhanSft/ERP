using AccountErp.Dtos.UserLogin;
using AccountErp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using AccountErp.Utilities;
using Microsoft.EntityFrameworkCore;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Dtos;

namespace AccountErp.DataLayer.Repositories
{
    public class UserRoleRepository:IUserRoleRepository
    {
        private readonly DataContext _dataContext;

        public UserRoleRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(UserRole entity)
        {
            await _dataContext.UsersRoles.AddAsync(entity);
        }

        public void Edit(UserRole entity)
        {
            _dataContext.UsersRoles.Update(entity);
        }

        public async Task<UserRole> GetAsync(int id)
        {
            return await _dataContext.UsersRoles.FindAsync(id);
        }

        public async Task<UserRoleDetailDto> GetDetailAsync(int id)
        {
            return await (from s in _dataContext.UsersRoles
                          where s.Id == id && s.Status != Constants.RecordStatus.Deleted
                          select new UserRoleDetailDto
                          {
                              Id = s.Id,
                              RoleName = s.RoleName
                          })
                          .AsNoTracking()
                          .SingleOrDefaultAsync();
        }
        public async Task<List<SelectListItemDto>> GetAllAsync()
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
        }

        public async Task<List<UserRoleDetailDto>> GetAllAsync(int PageSize, int Page,string FilterKey)
        {
            if (PageSize != 0 && Page != 0)
            {
                return await (from s in _dataContext.UsersRoles
                              where s.Status != Constants.RecordStatus.Deleted
                              && (FilterKey == null
                                || EF.Functions.Like(s.Id.ToString(), "%" + FilterKey + "%")
                                 || EF.Functions.Like(s.RoleName.ToString(), "%" + FilterKey + "%")
                                  )
                              select new UserRoleDetailDto
                              {
                                  Id = s.Id,
                                  RoleName = s.RoleName
                              })
                          .AsNoTracking()
                          .OrderByDescending(s => s.Id)
                          .Skip((Page - 1) * PageSize)
                          .Take(PageSize)
                          .ToListAsync();
            }
            else
            {
                return await (from s in _dataContext.UsersRoles
                              where s.Status != Constants.RecordStatus.Deleted
                              && (FilterKey == null
                                || EF.Functions.Like(s.Id.ToString(), "%" + FilterKey + "%")
                                 || EF.Functions.Like(s.RoleName.ToString(), "%" + FilterKey + "%")
                                  )
                              select new UserRoleDetailDto
                              {
                                  Id = s.Id,
                                  RoleName = s.RoleName
                              })
                          .AsNoTracking()
                          .ToListAsync();
            }
        }
        public async Task<JqDataTableResponse<UserRoleDetailDto>> GetPagedResultAsync(JqDataTableRequest model)
        {
            if (model.Length == 0)
            {
                model.Length = Constants.DefaultPageSize;
            }

            var filterKey = model.Search.Value;

            var linqStmt = (from s in _dataContext.UsersRoles
                            where s.Status != Constants.RecordStatus.Deleted && (filterKey == null || EF.Functions.Like(s.RoleName, "%" + filterKey + "%"))
                            select new UserRoleDetailDto
                            {
                                Id = s.Id,
                                RoleName = s.RoleName
                            })
                            .AsNoTracking();

            var sortExpresstion = model.GetSortExpression();

            var pagedResult = new JqDataTableResponse<UserRoleDetailDto>
            {
                RecordsTotal = await _dataContext.UsersRoles.CountAsync(x => x.Status != Constants.RecordStatus.Deleted),
                RecordsFiltered = await linqStmt.CountAsync(),
                Data = await linqStmt.OrderBy(sortExpresstion).Skip(model.Start).Take(model.Length).ToListAsync()
            };
            return pagedResult;
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.UsersRoles.FindAsync(id);
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.UsersRoles.Update(data);
        }
    }
}
