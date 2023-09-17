using AccountErp.Dtos.UserAccess;
using AccountErp.Entities;
using AccountErp.Factories;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.UserAccess;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Managers
{
    public class UserAccessManager : IUserAccessMAnager
    {
        private readonly IUserAccessRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly string _userId;

        public UserAccessManager(IHttpContextAccessor contextAccessor,
          IUserAccessRepository repository,
          IUnitOfWork unitOfWork)
        {
            _userId = contextAccessor.HttpContext.User.GetUserId();

            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddUserScreenAccessAsync(List<UserScreenAccessModel> model)
        {
            await _repository.DeleteAsyncUserScreenAccess(model[0].userRoleId);
            await _unitOfWork.SaveChangesAsync();
            List<UserScreenAccess> item = new List<UserScreenAccess>();
            UserScreenAccessFactory.CreateUserScreenAccess(model, item);
            await _repository.AddUserScreenAccessAsync(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<ScreenAccessDto>> GetUserScreenAccessById(int id)
        {
            List<ScreenAccessDto> data = new List<ScreenAccessDto>();
          data = await _repository.GetAsyncUserScreenAccess(id);
            if(data.Count == 0)
            {
                var screenData = await _repository.GetAllScreenDetail();
                foreach(var item in screenData)
                {
                    ScreenAccessDto obj = new ScreenAccessDto();
                    obj.ScreenId = item.Id;
                    obj.UserRoleId = id;
                    obj.CanAccess = false;
                    obj.ScreenName = item.ScreenName;
                    data.Add(obj);
                }
            }

            return data;
        }
        public async Task<List<ScreendetailDto>> GetAllScreenDetail()
        {
            return await _repository.GetAllScreenDetail();
        }
    }
}
