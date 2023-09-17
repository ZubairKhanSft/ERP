using AccountErp.Entities;
using AccountErp.Models.EndingStatementBalance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
  public  interface IEndingStatementBalanceManager
    {
        Task AddAsync(EndingStatementBalanceAddModel model);

        Task EditAsync(EndingStatementBalanceEditModel model);

        Task<EndingStatementBalance> GetDetailAsync(int BankAccountId);

    }
}
