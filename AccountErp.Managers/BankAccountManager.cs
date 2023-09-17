using AccountErp.DataLayer.Repositories;
using AccountErp.Dtos;
using AccountErp.Dtos.BankAccount;
using AccountErp.Entities;
using AccountErp.Factories;
using AccountErp.Infrastructure;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.BankAccount;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountErp.Managers
{
    public class BackAccountManager : IBankAccountManager
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IReconciliationRepository _reconciliationRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly string _userId;

        public BackAccountManager(IHttpContextAccessor contextAccessor, IBankAccountRepository bankAccountRepository,
            IUnitOfWork unitOfWork, IReconciliationRepository reconciliationRepository)
        {
            _userId = contextAccessor.HttpContext.User.GetUserId();
            _bankAccountRepository = bankAccountRepository;
            _reconciliationRepository = reconciliationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task AddAsync(BankAccountAddModel model)
        {
            var result = BankAccountFactory.Create(model, _userId);

            await _bankAccountRepository.AddAsync(result);
            if (model.COA_AccountTypeId==1||model.COA_AccountTypeId==2||model.COA_AccountTypeId==6||model.COA_AccountTypeId==7)
            {
                Reconciliation reconciliation =new Reconciliation();
                ReconciliationFactory.Create(result.Id,reconciliation);
                await _reconciliationRepository.AddAsync(reconciliation);
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EditAsync(BankAccountEditModel model)
        {
            var bankAccount = await _bankAccountRepository.GetAsync(model.Id);
            BankAccountFactory.Create(model,bankAccount,_userId);
            _bankAccountRepository.Edit(bankAccount);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<SelectListItemDto>> GetDetailByLedgerTypeAsync(int typeId)
        {
            return await _bankAccountRepository.GetDetailByLedgerTypeAsync(typeId);
        }

        public async Task<BankAccountDetailDto> GetDetailAsync(int id)
        {
            return await _bankAccountRepository.GetDetailAsync(id);
        }
        public async Task<BankAccountDetailDto> GetForEditAsync(int id)
        {
            return await _bankAccountRepository.GetForEditAsync(id);
        }

        public async Task<JqDataTableResponse<BankAccountListItemDto>> GetPagedResultAsync(JqDataTableRequest model)
        {
            return await _bankAccountRepository.GetPagedResultAsync(model);
        }

        public async Task<bool> IsAccountNumberExistsAsync(string accountNumber)
        {
            return await _bankAccountRepository.IsAccountNumberExistsAsync(accountNumber);
        }

        public async Task<bool> IsAccountNumberExistsForEditAsync(int id,string accountNumber)
        {
            return await _bankAccountRepository.IsAccountNumberExistsForEditAsync(id,accountNumber);
        }

        public async Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync()
        {
            return await _bankAccountRepository.GetSelectItemsAsync();
        }

        public async Task ToggleStatusAsync(int id)
        {
            await _bankAccountRepository.ToggleStatusAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _bankAccountRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
