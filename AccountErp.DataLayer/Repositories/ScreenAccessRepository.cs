using AccountErp.Dtos.RolePermission;
using AccountErp.Dtos.Screen;
using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AccountErp.Utilities;
using AccountErp.Dtos.ScreenAccess;

namespace AccountErp.DataLayer.Repositories
{
    public class ScreenAccessRepository : IScreenAccessRepository
    {
        private readonly DataContext _dataContext;

        public ScreenAccessRepository(DataContext dataContext, IServiceProvider serviceProvider)
        {
            _dataContext = dataContext;
        }
        public async Task AddUserScreenAccessAsync(List<ScreenAccess> entity)
        {
            foreach (var item in entity)
            {
                await _dataContext.ScreenAccesses.AddAsync(item);
                await _dataContext.SaveChangesAsync();
            }

        }

        public async Task<List<ScreenAccessDto>> GetAsyncUserScreenAccess(int id)
        {
            var obj = await (from s in _dataContext.ScreenAccesses
                             join s1 in _dataContext.Screen on s.ScreenId equals s1.Id
                             where s.RoleId == id 
                             select new ScreenAccessDto

                             {
                                 Id = s.Id,
                                 CompanyId = s.CompanyId,
                                 ScreenId = s.ScreenId,
                                 UserRoleId = s.UserRole.Id,
                                 CanAccess = s.CanAccess,
                                 ScreenName = s1.ScreenName,
                                 ScreenUrl = s1.ScreenUrl


                             })
                          .AsNoTracking().OrderByDescending(s => s.Id)
                          .ToListAsync();
            return obj;
        }


        public async Task<List<RolePermissionDto>> GetAsyncUserPermissionAccess(int id)
        {

            var obj = await (from s in _dataContext.RolePermissions
                             join s1 in _dataContext.Permissions on s.Permissionid equals s1.Id

                             where s.Roleid == id 
                             select new RolePermissionDto
                             {
                                 Id = s1.Id,
                                 PermissionId = s.Permissionid,
                                 RollId = s.Role.Id,
                                 ScreenId = s1.ScreenId,
                                 PermissionTittle = s1.Permisions,

                                 ScreenName = s1.Screen.ScreenName



                             })
                          .AsNoTracking().OrderByDescending(s => s.Id).ToListAsync();


            return obj;
        }

        public async Task DeleteAsyncUserScreenAccess(int id)
        {
            var data = await _dataContext.ScreenAccesses.Where(x => x.RoleId == id ).ToListAsync();
            foreach (var item in data)
            {
                _dataContext.ScreenAccesses.Remove(item);
            }

        }
        public async Task<List<ScreenDto>> GetAllScreenDetail()
        {
            return await (from s in _dataContext.Screen
                          where s.Status != Constants.RecordStatus.Deleted 
                          select new ScreenDto
                          {
                              Id = s.Id,
                              ScreenCode = s.ScreenCode,
                              ScreenName = s.ScreenName,
                              ScreenUrl = s.ScreenUrl,
                              Status = s.Status
                          })
                         .AsNoTracking().OrderByDescending(s => s.Id)
                         .ToListAsync();
        }

        public async Task<List<ScreenDto>> GetAllScreen()
        {
            return await (from s in _dataContext.Screen
                          where s.Status != Constants.RecordStatus.Deleted 

                          select new ScreenDto
                          {
                              Id = s.Id,
                              ScreenCode = s.ScreenCode,
                              ScreenName = s.ScreenName,
                              ScreenUrl = s.ScreenUrl
                          })
                         .AsNoTracking().OrderByDescending(s => s.Id)
                         .ToListAsync();
        }
    }
}
