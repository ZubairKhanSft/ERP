using AccountErp.Dtos.WareHouse;
using AccountErp.Factories;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.WareHouse;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Managers
{
   public class WareHouseManager : IWareHouseManager
    {
        private readonly IWareHouseRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly string _userId;

        public WareHouseManager(IHttpContextAccessor contextAccessor,
            IWareHouseRepository repository,
            IUnitOfWork unitOfWork)
        {
            _userId = contextAccessor.HttpContext.User.GetUserId();

            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task AddAsync(WareHouseAddModel model)
        {
            await _repository.AddAsync(WareHouseFactory.Create(model, _userId));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EditAsync(WareHouseEditModel model)
        {
            var warehouse = await _repository.GetAsync(model.Id);
            WareHouseFactory.Create(model, warehouse, _userId);
            _repository.Edit(warehouse);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<WareHouseDetailsDto> GetDetailAsync(int id)
        {
            return await _repository.GetDetailAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<JqDataTableResponse<WareHouseDetailsListDto>> GetPagedResultAsync(WareHouseJqDataTableRequestModel model)
        {
            return await _repository.GetPagedResultAsync(model);
        }

        public async Task<IEnumerable<WareHouseDetailsDto>> GetAllAsync(Constants.RecordStatus? status = null)
        {
            return await _repository.GetAllAsync(status);
        }

        public async Task<List<WareHouseDetailsDto>> GetAllAsync(int PageSize,int Page)
        {
            return await _repository.GetAllAsync(PageSize, Page);
        }
    }
}
