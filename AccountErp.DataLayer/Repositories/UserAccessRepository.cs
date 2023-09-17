using AccountErp.Dtos.UserAccess;
using AccountErp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using AccountErp.Infrastructure.Repositories;

namespace AccountErp.DataLayer.Repositories
{
    public class UserAccessRepository: IUserAccessRepository
    {
        private readonly DataContext _dataContext;

        public UserAccessRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task AddUserScreenAccessAsync(List<UserScreenAccess> entity)
        {
            foreach (var item in entity)
            {
                await _dataContext.UserScreenAccess.AddAsync(item);
            }

        }

        public async Task<List<ScreenAccessDto>> GetAsyncUserScreenAccess(int id)
        {
            return await (from s in _dataContext.UserScreenAccess
                          where s.UserRoleId == id
                          select new ScreenAccessDto
                          {
                              Id = s.Id,
                              ScreenId = s.ScreenId,
                              UserRoleId = s.UserRoleId,
                              CanAccess = s.CanAccess,
                              ScreenName = s.Screen.ScreenName
                              
                          })
                         .AsNoTracking()
                         .OrderBy("ScreenName Asc")
                         .ToListAsync();
        }

        public async Task DeleteAsyncUserScreenAccess(int id)
        {
            var data = await _dataContext.UserScreenAccess.Where(x => x.UserRoleId == id).ToListAsync();
            foreach (var item in data)
            {
                _dataContext.UserScreenAccess.Remove(item);
            }

        }
        public async Task<List<ScreendetailDto>> GetAllScreenDetail()
        {
            return await (from s in _dataContext.ScreenDetail
                          select new ScreendetailDto
                          {
                              Id = s.Id,
                              ScreenCode = s.ScreenCode,
                              ScreenName = s.ScreenName
                          })
                         .AsNoTracking()
                         .ToListAsync();
        }


    }
}
