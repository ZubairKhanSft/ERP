using AccountErp.Dtos.UserLogin;
using AccountErp.Dtos;
using AccountErp.Factories;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.UserLogin;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AccountErp.Models.CurrencyCr;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Dtos.CurrencyCr;

namespace AccountErp.Managers
{
    public class CurrencyCrManager : ICurrencyCrManager
    {
        private readonly ICurrencyCrRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyCrManager(ICurrencyCrRepository repository,IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(AddCurrencyCrModel model)
        {
            await _repository.AddAsync(CurrencyCrFactory.Create(model));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EditAsync(AddCurrencyCrModel model)
        {
            var item = await _repository.GetAsync(model.Id);
            CurrencyCrFactory.Create(model, item);
            _repository.Edit(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<CurrencyCrDetailDto> GetDetailAsync(int id)
        {
            return await _repository.GetDetailAsync(id);
        }

       /* public async Task<JqDataTableResponse<UserRoleDetailDto>> GetPagedResultAsync(JqDataTableRequest model)
        {
            return await _repository.GetPagedResultAsync(model);
        }*/

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
       /* public async Task<List<SelectListItemDto>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }*/

        public async Task<List<CurrencyCrDetailDto>> GetAllAsync(int PageSize, int Page, string filterKey)
        {
            return await _repository.GetAllAsync(PageSize, Page, filterKey);
        }
    }
}
