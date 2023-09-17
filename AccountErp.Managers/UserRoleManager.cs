using AccountErp.Dtos;
using AccountErp.Dtos.UserLogin;
using AccountErp.Factories;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.UserLogin;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Managers
{
    public class UserRoleManager:IUserRoleManager
    {
        private readonly IUserRoleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly string _userId;

        public UserRoleManager(IHttpContextAccessor contextAccessor,
          IUserRoleRepository repository,
          IUnitOfWork unitOfWork)
        {
            _userId = contextAccessor.HttpContext.User.GetUserId();

            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(UserRoleModel model)
        {
            await _repository.AddAsync(UserRoleFactory.Create(model, _userId));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EditAsync(UserRoleModel model)
        {
            var item = await _repository.GetAsync(model.Id);
            UserRoleFactory.Create(model, item, _userId);
            _repository.Edit(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<UserRoleDetailDto> GetDetailAsync(int id)
        {
            return await _repository.GetDetailAsync(id);
        }

        public async Task<JqDataTableResponse<UserRoleDetailDto>> GetPagedResultAsync(JqDataTableRequest model)
        {
            return await _repository.GetPagedResultAsync(model);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<List<SelectListItemDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<UserRoleDetailDto>> GetAllAsync(int PageSize,int Page, string filterKey)
        {
            return await _repository.GetAllAsync(PageSize,Page,filterKey);
        }
    }
}
