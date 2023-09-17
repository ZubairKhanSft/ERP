using AccountErp.Entities;
using AccountErp.Factories;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.EndingStatementBalance;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Managers
{
  public class EndingStatementBalanceManager : IEndingStatementBalanceManager
    {
        private readonly IEndingStatementBalanceRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _userId;

        public EndingStatementBalanceManager(IHttpContextAccessor contextAccessor,
           IEndingStatementBalanceRepository repository,
           IUnitOfWork unitOfWork)
        {
            _userId = contextAccessor.HttpContext.User.GetUserId();

            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(EndingStatementBalanceAddModel model)
        {
            await _repository.AddAsync(EndingStatementBalanceFactory.Create(model, _userId));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task EditAsync(EndingStatementBalanceEditModel model)
        {
            EndingStatementBalance endingStatementBalance = new EndingStatementBalance();
            EndingStatementBalanceFactory.Create(model, endingStatementBalance, _userId);
            _repository.Edit(endingStatementBalance);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<EndingStatementBalance> GetDetailAsync(int BankAccountId)
        {
            return await _repository.GetAsync(BankAccountId);
        }

    }
}
