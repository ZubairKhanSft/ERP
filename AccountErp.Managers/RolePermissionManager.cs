using AccountErp.Dtos.RolePermission;
using AccountErp.Factories;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.RolePermission;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Managers
{
    public class RolePermissionManager : IRolePermissionManager
    {
        private readonly IRolePermissionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly string _userId;

        public RolePermissionManager(IHttpContextAccessor contextAccessor, IServiceProvider serviceProvider,
          IRolePermissionRepository repository,
          IUnitOfWork unitOfWork)
        {
            _userId = contextAccessor.HttpContext.User.GetUserId();

            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(AddRolePermission model)
        {
            await _repository.AddAsync(RolePermissionFactory.Create(model, _userId));
            await _unitOfWork.SaveChangesAsync();
        }



        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync2(int id)
        {
            await _repository.DeleteAsync1(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public RolePermissionDto isExist(AddRolePermission id)
        {
            try
            {
                return _repository.isExist(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
