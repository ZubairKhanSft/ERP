using AccountErp.Dtos.Reconciliation;
using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AccountErp.Dtos.Transaction;
using AccountErp.Utilities;

namespace AccountErp.DataLayer.Repositories
{
  public  class ReconciliationRepository : IReconciliationRepository
    {

        private readonly DataContext _dataContext;

        public ReconciliationRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Reconciliation entity)
        {
            if(entity.Id==0)
            await _dataContext.AddAsync(entity);
            else
                _dataContext.Update(entity);
        }

        public void Edit(Reconciliation entity)
        {
            _dataContext.Update(entity);
        }

        public async Task<Reconciliation> GetAsync(int id)
        {
            return await _dataContext.Reconciliation.FindAsync(id);
        }

        public async Task<IEnumerable<ReconciliationDto>> GetAllAsync()
        {
            return await (from s in _dataContext.Reconciliation
                          select new ReconciliationDto 
                          {
                              Id = s.Id,
                              BankAccountId = s.BankAccountId,
                              ReconciliationDate = s.ReconciliationDate,
                              StatementBalance = s.StatementBalance,
                              IcloseBalance = s.IcloseBalance,
                              IsReconciliation = s.IsReconciliation,
                              ReconciliationStatus = s.ReconciliationStatus,
                              bankname=s.bank.AccountName
                          })
                          .AsNoTracking()
                            .ToListAsync();
        }

        /* public async Task DeleteAsync(int id)
         {
             var item = await _dataContext.Reconciliation.FindAsync(id);
             item.Status = Constants.RecordStatus.Deleted;
             _dataContext.Items.Update(item);

         }*/
     public async Task<TransactionBankDto> GetByBankId(int BankAccountId)

       {
            var linqstmt =await (from i in _dataContext.Transaction
                            where i.BankAccountId == BankAccountId
                                 select new TransactionDetailDto
                                 {
                                Id=i.Id,
                                BankAccountId=i.BankAccountId,
                                CreditAmount=i.CreditAmount,
                                DebitAmount=i.DebitAmount,
                                Description=i.Description,
                                TransactionDate=i.TransactionDate,
                                TransactionId=i.TransactionId,
                                Status=i.Status

                                }).AsNoTracking().ToListAsync();
            var banks =new TransactionBankDto();

              var obj =  _dataContext.BankAccounts.Where(x => x.Id == BankAccountId).FirstOrDefault();
            banks.BankName = obj.BankName;
            banks.TransactionRecords = linqstmt;

            return banks;

           //return await _dataContext.Transaction.Where(x => x.BankAccountId == BankAccountId)
           //     .AsNoTracking()
           //      .ToListAsync();
        }
    }
}
