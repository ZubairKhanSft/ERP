using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.DataLayer.Repositories
{
 public   class EndingStatementBalanceRepository : IEndingStatementBalanceRepository
    {
        private readonly DataContext _dataContext;

        public EndingStatementBalanceRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(EndingStatementBalance entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public void Edit(EndingStatementBalance entity)
        {
            _dataContext.Update(entity);
        }

        public async Task<EndingStatementBalance> GetAsync(int BankAccountId)
        {
            return await _dataContext.EndingStatementBalance.FindAsync(BankAccountId);
        }
    }
}
