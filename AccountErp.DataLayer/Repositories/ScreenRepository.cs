using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AccountErp.Utilities;
using AccountErp.Dtos.Screen;
using AccountErp.Dtos.Permission;

namespace AccountErp.DataLayer.Repositories
{
    public class ScreenRepository : IScreenRepository
    {
        private readonly DataContext _dataContext;

        public ScreenRepository(DataContext dataContext, IServiceProvider serviceProvider)
        {
            //_dataContext = dataContext;
            _dataContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<DataContext>();

        }

        public async Task AddAsync(Screen entity)
        {
            await _dataContext.Screen.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(Screen entity)
        {
            _dataContext.Screen.Update(entity);
            _dataContext.SaveChanges();

        }

        public async Task<Screen> GetAsync(int id)
        {
            return await _dataContext.Screen.Where(s => s.Id == id ).FirstOrDefaultAsync();
        }

        public async Task<ScreenDto> GetDetailAsync(int id)
        {
            return await (from s in _dataContext.Screen
                          where s.Status != Constants.RecordStatus.Deleted
                                    && s.Id == id 

                          select new ScreenDto
                          {
                              Id = s.Id,
                            
                              ScreenName = s.ScreenName,
                              ScreenCode = s.ScreenCode,
                              ScreenUrl = s.ScreenUrl,
                              Status = s.Status
                          })
                          .AsNoTracking()
                          .SingleOrDefaultAsync();
        }
        public async Task<List<ScreenPermissionDetailDto>> GetAllScreenPermissionAsync()
        {
            return await (from s in _dataContext.Screen
                          where s.Status != Constants.RecordStatus.Deleted 
                          select new ScreenPermissionDetailDto
                          {
                              Id = s.Id,
                              ScreenName = s.ScreenName,
                              ScreenCode = s.ScreenCode,
                              ScreenUrl = s.ScreenUrl,
                              Status = s.Status

                          })
                          .AsNoTracking().OrderByDescending(s => s.Id)
                          .ToListAsync();
        }

        public async Task<List<PermissionDto>> GetAllPermissionAsync(int id)
        {
            return await (from s in _dataContext.Permissions
                          where s.Status != Constants.RecordStatus.Deleted && s.ScreenId == id
                          select new PermissionDto
                          {
                              Id = s.Id,
                              PermissionDescription = s.PermisionDescription,
                              PermissionCode = s.PermissionCode,
                              Permissions = s.Permisions,
                              ScreenId = s.ScreenId,
                              Status = s.Status
                          }
                           ).AsNoTracking().OrderByDescending(s => s.Id).ToListAsync();
        }


        public async Task<int> GetAllCount()
        {
            var data = await _dataContext.Screen.Where(s => s.Status != Constants.RecordStatus.Deleted).ToListAsync();
            return data.Count;
        }

        public async Task<List<ScreenDetailDto>> GetAllAsync(int PageSize, int Page)
        {
            if (Page == 0)
            {
                Page = 1;
            }
            List<ScreenDetailDto> data = new List<ScreenDetailDto>();
            data = await (from s in _dataContext.Screen
                          where s.Status != Constants.RecordStatus.Deleted 
                          select new ScreenDetailDto
                          {
                              Id = s.Id,
                             
                              ScreenName = s.ScreenName,
                              ScreenCode = s.ScreenCode,
                              ScreenUrl = s.ScreenUrl,
                              Status = s.Status,
                              CreatedOn = s.CreatedOn,
                              UpdatedOn = s.UpdatedOn,

                          })
                          .AsNoTracking().OrderByDescending(s => s.Id).Skip((Page - 1) * PageSize).Take(PageSize)
                          .ToListAsync();
            return data;
        }
        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.Screen.Where(s => s.Id == id ).FirstOrDefaultAsync();
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.Screen.Update(data);
            await _dataContext.SaveChangesAsync();
        }
    }
}
