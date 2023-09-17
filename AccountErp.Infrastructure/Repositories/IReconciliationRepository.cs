using AccountErp.Dtos.Reconciliation;
using AccountErp.Dtos.Transaction;
using AccountErp.Entities;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
   public interface IReconciliationRepository
    {

        Task AddAsync(Reconciliation entity);

        void Edit(Reconciliation entity);

        Task<Reconciliation> GetAsync(int id);


        Task<TransactionBankDto> GetByBankId(int BankAccountId);

        Task<IEnumerable<ReconciliationDto>> GetAllAsync();



    }
}
