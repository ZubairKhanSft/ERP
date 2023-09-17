using AccountErp.Dtos.RolePermission;
using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.RolePermission;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AccountErp.DataLayer.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {

        private readonly DataContext _dataContext;

        public RolePermissionRepository(DataContext dataContext, IServiceProvider serviceProvider)
        {
            // _dataContext = dataContext;
            _dataContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<DataContext>();

        }

        public async Task AddAsync(RolePermission entity)
        {
            await _dataContext.RolePermissions.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public async Task AddUserScreenAccessAsync(List<RolePermission> entity)
        {
            foreach (var item in entity)
            {
                await _dataContext.RolePermissions.AddAsync(item);
                await _dataContext.SaveChangesAsync();
            }

        }
        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.RolePermissions.Where(s => s.Id == id ).FirstOrDefaultAsync();
            _dataContext.RolePermissions.Remove(data);
            await _dataContext.SaveChangesAsync();
        }
        public async Task DeleteAsync1(int id)
        {
            var data = await _dataContext.RolePermissions.Where(s => s.Id == id ).FirstOrDefaultAsync();
            _dataContext.RolePermissions.Remove(data);
            await _dataContext.SaveChangesAsync();

        }

        public RolePermissionDto isExist(AddRolePermission model)
        {
            return (from s in _dataContext.RolePermissions
                    where s.Permissionid == model.PermissionId && s.Roleid == model.RoleId
                           
                    select new RolePermissionDto
                    {
                        Id = s.Id,
                       
                        PermissionId = s.Permissionid,
                        RollId = s.Roleid,
                    })
                         .AsNoTracking()
                         .FirstOrDefault();
        }
    }
}
