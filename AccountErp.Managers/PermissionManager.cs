using AccountErp.Dtos.Permission;
using AccountErp.Factories;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.Permission;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Managers
{
    public class PermissionManager : IPermissionManager
    {
        private readonly IPermissionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly string _userId;

        public PermissionManager(IHttpContextAccessor contextAccessor,
          IPermissionRepository repository,
          IUnitOfWork unitOfWork)
        {
            _userId = contextAccessor.HttpContext.User.GetUserId();

            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(PermissionAddModel model)
        {
            await _repository.AddAsync(PermissionFactory.Create(model, _userId));
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task EditAsync(PermissionEditModel model)
        {
            var item = await _repository.GetAsync(model.Id);
            PermissionFactory.Create(model, item, _userId);
            _repository.Edit(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PermissionDto> GetDetailAsync(int id)
        {
            return await _repository.GetDetailAsync(id);
        }

        public async Task<PermissionDto> GetDetailByScreenAsync(int id)
        {
            return await _repository.GetScreenDetailAsync(id);
        }

        public async Task<List<PermissionDto>> GetAllPaginationAsync(int PageSize, int Page)
        {
            return await _repository.GetAllPaginationAsync(PageSize, Page);
        }
        public async Task<List<PermissionDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<int> GetAllCount()
        {
            return await _repository.GetAllCount();
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
