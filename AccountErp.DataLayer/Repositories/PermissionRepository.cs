using AccountErp.Dtos.Permission;
using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AccountErp.Utilities;

namespace AccountErp.DataLayer.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DataContext _dataContext;

        public PermissionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Permission entity)
        {
            await _dataContext.Permissions.AddAsync(entity);
        }

        public void Edit(Permission entity)
        {
            _dataContext.Permissions.Update(entity);
        }

        public async Task<Permission> GetAsync(int id)
        {
            return await _dataContext.Permissions.Where(s => s.Id == id ).FirstOrDefaultAsync();
        }

        public async Task<PermissionDto> GetDetailAsync(int id)
        {
            return await (from s in _dataContext.Permissions
                          where s.Id == id && s.Status == Constants.RecordStatus.Active 
                          select new PermissionDto
                          {
                              Id = s.Id,
                              Permissions = s.Permisions,
                              PermissionDescription = s.PermisionDescription,
                              ScreenCode = s.ScreenCode,
                              ScreenUrl = s.ScreenUrl,
                              PermissionCode = s.PermissionCode,
                              Status = s.Status,
                              ScreenId = s.ScreenId,
                            
                          })
                          .AsNoTracking()
                          .SingleOrDefaultAsync();
        }


        public async Task<PermissionDto> GetScreenDetailAsync(int id)
        {
            return await (from s in _dataContext.Permissions
                          where s.ScreenId == id && s.Status == Constants.RecordStatus.Active
                          select new PermissionDto
                          {
                              Id = s.Id,
                              Permissions = s.Permisions,
                              PermissionDescription = s.PermisionDescription,
                              ScreenCode = s.ScreenCode,
                              ScreenUrl = s.ScreenUrl,
                              PermissionCode = s.PermissionCode,
                              Status = s.Status,
                              ScreenId = s.ScreenId,

                          })
                          .AsNoTracking()
                          .SingleOrDefaultAsync();
        }



        public async Task<List<PermissionDto>> GetAllPaginationAsync(int PageSize, int Page)
        {
            return await (from s in _dataContext.Permissions
                          join t in _dataContext.Screen
                          on s.ScreenId equals t.Id
                          where s.Status == Constants.RecordStatus.Active 
                          select new PermissionDto
                          {
                              Id = s.Id,
                              Permissions = s.Permisions,
                              PermissionDescription = s.PermisionDescription,
                              PermissionCode = s.PermissionCode,
                              ScreenCode = s.ScreenCode,
                              ScreenUrl = s.ScreenUrl,
                              Status = s.Status,
                              ScreenId = s.ScreenId,
                              ScreenName = t.ScreenName
                          })
                          .AsNoTracking().OrderByDescending(s => s.Id).Skip((Page - 1) * PageSize).Take(PageSize)
                          .ToListAsync();
        }

        public async Task<List<PermissionDto>> GetAllAsync()
        {
            return await (from s in _dataContext.Permissions
                          join t in _dataContext.Screen
                          on s.ScreenId equals t.Id
                          where s.Status == Constants.RecordStatus.Active 
                          select new PermissionDto
                          {
                              Id = s.Id,
                              Permissions = s.Permisions,
                              PermissionDescription = s.PermisionDescription,
                              ScreenCode = s.ScreenCode,
                              ScreenUrl = s.ScreenUrl,
                              PermissionCode = s.PermissionCode,
                              Status = s.Status,
                              ScreenId = s.ScreenId,
                              ScreenName = t.ScreenName
                          })
                          .AsNoTracking().OrderByDescending(s => s.Id)
                          .ToListAsync();
        }
        public async Task<int> GetAllCount()
        {
            var data = await _dataContext.Permissions.Where(s =>  s.Status != Constants.RecordStatus.Deleted).ToListAsync();
            return data.Count;
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.Permissions.Where(s => s.Id == id ).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Permissions.Update(data);
            await _dataContext.SaveChangesAsync();
        }

    }
}
