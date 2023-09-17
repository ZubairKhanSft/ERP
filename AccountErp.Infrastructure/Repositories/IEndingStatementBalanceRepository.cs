using AccountErp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
   public interface IEndingStatementBalanceRepository
    {
        Task AddAsync(EndingStatementBalance entity);

        void Edit(EndingStatementBalance entity);

        Task<EndingStatementBalance> GetAsync(int BankAccountId);
    }
}
