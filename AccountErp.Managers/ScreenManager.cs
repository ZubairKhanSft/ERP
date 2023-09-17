using AccountErp.Dtos.Screen;
using AccountErp.Factories;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.Screen;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Managers
{
    public class ScreenManager : IScreenManager
    {
        private readonly IScreenRepository _repository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly string _userId;

        public ScreenManager(IHttpContextAccessor contextAccessor, IPermissionRepository permissionRepository,
          IScreenRepository repository,
          IUnitOfWork unitOfWork)
        {
            _userId = contextAccessor.HttpContext.User.GetUserId();
            _permissionRepository = permissionRepository;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task AddAsync(AddScreenModel model)
        {
            await _repository.AddAsync(ScreenFactory.Create(model, _userId));
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task EditAsync(AddScreenModel model)
        {
            var item = await _repository.GetAsync(model.Id);
            ScreenFactory.Create(model, item, _userId);
            _repository.Edit(item);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<ScreenDto> GetDetailAsync(int id)
        {
            return await _repository.GetDetailAsync(id);
        }
        public async Task<List<ScreenDetailDto>> GetAllAsync(int PageSize, int Page)
        {
            return await _repository.GetAllAsync(PageSize, Page);
        }
        public async Task<int> GetAllCount()
        {
            return await _repository.GetAllCount();
        }
        public async Task<List<ScreenPermissionDetailDto>> GetAllScreenPermission()
        {
            var data = await _repository.GetAllScreenPermissionAsync();
            foreach (var item in data)
            {
                item.ScreenPermission = await _repository.GetAllPermissionAsync(item.Id);
            }
            return data;
        }



        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
