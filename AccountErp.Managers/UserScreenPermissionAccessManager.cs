using AccountErp.Dtos.Permission;
using AccountErp.Dtos.UserScreenPermissionAccess;
using AccountErp.Factories;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.UserScreenPermissionAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace AccountErp.Managers
{
    public class UserScreenPermissionAccessManager : IUserScreenPermissionAccessManager
    {
        private readonly IUserScreenPermissionAccessRepository _repository;
        private readonly IScreenRepository _ScreenRepository;

        public UserScreenPermissionAccessManager(IUserScreenPermissionAccessRepository repository, IScreenRepository screenRepository)
        {
            _repository = repository;
            _ScreenRepository = screenRepository;
        }

        public async Task AddAsync(AddUserScreenPermissionAccessModel model)
        {

            await _repository.DeleteUserScreenAccess(model.RoleId);

            await _repository.AddAsync(UserScreenPermissionAccessFactory.Create(model));

            /*else
            {
                var item = await _repository.GetAsync(model.Id);
                MilestoneCategoryFactory.Update(item, model);
                _repository.Edit(item);
            }*/
        }

        public async Task<List<ScreensWithPermission>> GetByRoleId(int id)
        {
            var data = await _repository.GetByRoleId(id);
            var screens = data.Select(x => x.ScreenId).Distinct().ToList();

            List<ScreensWithPermission> screensWithPermissions = new List<ScreensWithPermission>();
            foreach (var screenId in screens)
            {
                ScreensWithPermission screenWithPermision = new ScreensWithPermission();
                List<PermissionDto> permissions = new List<PermissionDto>();
                screenWithPermision.screen = await _repository.GetAllScreenPermissionAsync(screenId);
                var permi = data.Where(x => x.ScreenId == screenId).ToList();
                foreach (var perm in permi)
                {
                    var permission = await _repository.GetPermissionById(perm.PermissionId);
                    permissions.Add(permission);
                }
                screenWithPermision.permissions = permissions;
                screensWithPermissions.Add(screenWithPermision);
            }
            return screensWithPermissions;
        }
    }
}
