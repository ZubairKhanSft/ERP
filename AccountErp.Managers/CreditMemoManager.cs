using AccountErp.Dtos.CreditMemo;
using AccountErp.Factories;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.CreditMemo;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
namespace AccountErp.Managers
{
    public class CreditMemoManager : ICreditMemoManager
    {
        private readonly ICreditMemoRepository _creaditmemoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionRepository _transactionRepository;

        private readonly string _userId;
     

        public CreditMemoManager(IHttpContextAccessor contextAccessor,
            ICreditMemoRepository creaditmemoRepository, ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork)
        {
            _userId = contextAccessor.HttpContext.User.GetUserId();
            _transactionRepository = transactionRepository;
            _creaditmemoRepository = creaditmemoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(CreditMemoAddModel model)
        {
            var count = await _creaditmemoRepository.getCount();
            var creditMemo = CreditMemoFactory.Create(model, _userId, count);
            await _creaditmemoRepository.AddAsync(creditMemo);
            await _unitOfWork.SaveChangesAsync();

            if (model.Status == Constants.InvoiceStatus.Paid)
            {
                await _unitOfWork.SaveChangesAsync();
                var transaction = TransactionFactory.CreateByCreditMemo(creditMemo);
                await _transactionRepository.AddAsync(transaction);

                var itemsList = (model.CreditMemoService.GroupBy(l => l.BankAccountId, l => new { l.BankAccountId, l.DiffAmmount })
            .Select(g => new { GroupId = g.Key, Values = g.ToList() })).ToList();

                foreach (var item in itemsList)
                {
                    var id = item.GroupId;
                    var amount = item.Values.Sum(x => x.DiffAmmount);
                    if (amount > 0)
                    {
                        var itemsData = TransactionFactory.CreateByCreditMemoItemsAndTax(creditMemo, id, amount);
                        await _transactionRepository.AddAsync(itemsData);
                        await _unitOfWork.SaveChangesAsync();
                    }
                }

                var taxlistList = (model.CreditMemoService.GroupBy(l => l.TaxBankAccountId, l => new { l.TaxBankAccountId, l.TaxDiffAmmount })
           .Select(g => new { GroupId = g.Key, Values = g.ToList() })).ToList();

                foreach (var tax in taxlistList)
                {
                    if (tax.GroupId > 0)
                    {
                        var id = tax.GroupId;
                        var amount = tax.Values.Sum(x => x.TaxDiffAmmount);
                        if(amount > 0)
                        {
                            var taxData = TransactionFactory.CreateByCreditMemoItemsAndTax(creditMemo, id, amount);
                            await _transactionRepository.AddAsync(taxData);
                            await _unitOfWork.SaveChangesAsync();
                        }
                       
                    }

                }
            }
          
        }

        public async Task<JqDataTableResponse<CreditMemoListItemDto>> GetPagedResultAsync(CreditMemoJqDataTableRequestModel model)
        {
            return await _creaditmemoRepository.GetPagedResultAsync(model);
        }


        public async Task<CreditMemoDetailDto> GetDetailAsync(int id)
        {
            return await _creaditmemoRepository.GetDetailAsync(id);
        }

        public async Task EditAsync(CreditMemoEditModel model)
        {
            var creaditmemo = await _creaditmemoRepository.GetAsync(model.Id);
            CreditMemoFactory.Edit(model, creaditmemo, _userId);
            _creaditmemoRepository.Edit(creaditmemo);
            await _unitOfWork.SaveChangesAsync();
        }
    }
    }