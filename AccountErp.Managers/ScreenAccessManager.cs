using AccountErp.Dtos.RolePermission;
using AccountErp.Dtos.Screen;
using AccountErp.Dtos.ScreenAccess;
using AccountErp.Entities;
using AccountErp.Factories;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.ScreenAccess;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Managers
{
    public class ScreenAccessManager : IScreenAccessMAnager
    {
        private readonly IScreenAccessRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly string _userId;

        public ScreenAccessManager(IHttpContextAccessor contextAccessor,
          IScreenAccessRepository repository,
          IUnitOfWork unitOfWork)
        {
            _userId = contextAccessor.HttpContext.User.GetUserId();

            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddUserScreenAccessAsync(ScreenAccessModel model)
        {
            await _repository.DeleteAsyncUserScreenAccess(model.UserRoleId);
            await _unitOfWork.SaveChangesAsync();
            List<ScreenAccess> item = new List<ScreenAccess>();

            ScreenAccessFactory.CreateUserScreenAccess(model, item);
            await _repository.AddUserScreenAccessAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<List<ScreenAccessDto>> GetUserScreenAccessById(int id)
        {
            List<ScreenAccessDto> data = new List<ScreenAccessDto>();
            data = await _repository.GetAsyncUserScreenAccess(id);
            if (data.Count == 0)
            {

                ScreenAccessDto obj = new ScreenAccessDto();
                foreach (var item in data)
                {


                    obj.ScreenId = item.Id;
                    obj.UserRoleId = id;
                    obj.CanAccess = false;
                    obj.ScreenName = item.ScreenName;
                    obj.ScreenUrl = item.ScreenUrl;
                    obj.CompanyId = item.CompanyId;
                    data.Add(obj);
                }
            }

            return data;
        }

        public async Task<List<RolePermissionDto>> GetUserPermissionAccessById(int id)
        {
            List<RolePermissionDto> data = new List<RolePermissionDto>();
            data = await _repository.GetAsyncUserPermissionAccess(id);


            return data;
        }

        /*public async Task<List<ScreenPermiByRoleIdDto>> GetScreenPermissionByRoleId(int roleid)
        {
            var data = await _repository.GetScreenAccessByRoleId(roleid);
            return data;
        }*/
        public async Task<List<ScreenDto>> GetAllScreenDetail()
        {
            return await _repository.GetAllScreenDetail();
        }
    }
}
