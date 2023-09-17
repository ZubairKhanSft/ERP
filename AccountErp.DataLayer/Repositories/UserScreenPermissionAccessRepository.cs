using AccountErp.Dtos.Permission;
using AccountErp.Dtos.Screen;
using AccountErp.Dtos.UserScreenPermissionAccess;
using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AccountErp.Utilities;
using Microsoft.EntityFrameworkCore;

namespace AccountErp.DataLayer.Repositories
{
    public class UserScreenPermissionAccessRepository : IUserScreenPermissionAccessRepository
    {
        public readonly DataContext _dataContext;
        public UserScreenPermissionAccessRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(List<UserScreenPermissionAccess> entity)
        {
            foreach (var item in entity)
            {
                await _dataContext.UserScreenPermissionAccess.AddAsync(item);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<List<UserScreenPermissionAccessDetailDto>> GetByRoleId(int id)
        {
            return await (from s in _dataContext.UserScreenPermissionAccess
                          where s.RoleId == id
                          select new UserScreenPermissionAccessDetailDto
                          {
                              Id = s.Id,
                              RoleId = s.RoleId,
                              ScreenId = s.ScreenId,
                              PermissionId = s.PermissionId
                          }).AsNoTracking().ToListAsync();
        }

        public async Task DeleteUserScreenAccess(int roleid)
        {
            var data = await _dataContext.UserScreenPermissionAccess.Where(x => x.RoleId == roleid).ToListAsync();
            foreach (var item in data)
            {
                _dataContext.UserScreenPermissionAccess.Remove(item);
                await _dataContext.SaveChangesAsync();
            }
        }
        public async Task<ScreenDto> GetAllScreenPermissionAsync(int Screenid)
        {
            return await (from s in _dataContext.Screen
                          where s.Id == Screenid && s.Status != Constants.RecordStatus.Deleted
                          select new ScreenDto
                          {
                              Id = s.Id,
                              ScreenCode = s.ScreenCode,
                              ScreenName = s.ScreenName,
                              ScreenUrl = s.ScreenUrl,
                              Status = s.Status,
                          }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<PermissionDto> GetPermissionById(int PermissionId)
        {
            return await (from s in _dataContext.Permissions
                          where s.Id == PermissionId && s.Status != Constants.RecordStatus.Deleted
                             
                          select new PermissionDto
                          {
                              Id = s.Id,
                              Permissions = s.Permisions,
                              PermissionDescription = s.PermisionDescription,
                              PermissionCode = s.PermissionCode,
                              ScreenId = s.ScreenId,
                              Status = s.Status,
                          }).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
