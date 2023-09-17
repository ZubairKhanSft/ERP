using AccountErp.Dtos.CreditMemo;
using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.CreditMemo;
using AccountErp.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using AccountErp.Dtos.Customer;

namespace AccountErp.DataLayer.Repositories
{
  public  class CreditMemoRepository : ICreditMemoRepository
    {
        private readonly DataContext _dataContext;

        public CreditMemoRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(CreditMemo entity)
        {
            await _dataContext.CreditMemo.AddAsync(entity);
        }

        public async Task<int> getCount()
        {
            int count = await _dataContext.CreditMemo.CountAsync();
            return count;
        }

        public void Edit(CreditMemo entity)
        {

            _dataContext.CreditMemo.Update(entity);
        }

        public async Task<CreditMemo> GetAsync(int id)
        {
            return await _dataContext.CreditMemo.Include(x => x.CreditMemoService)
                .SingleAsync(x => x.Id == id);
        }
        public async Task<JqDataTableResponse<CreditMemoListItemDto>> GetPagedResultAsync(CreditMemoJqDataTableRequestModel model)
        {
            if (model.Length == 0)
            {
                model.Length = Constants.DefaultPageSize;
            }

            var linqstmt = (from i in _dataContext.CreditMemo
                            join c in _dataContext.Customers
                            on i.CustomerId equals c.Id
                            where (model.CustomerId == null
                                    || i.CustomerId == model.CustomerId.Value)
                                && (model.FilterKey == null
                                    || EF.Functions.Like(c.FirstName, "%" + model.FilterKey + "%")
                                  //   || EF.Functions.Like(c.MiddleName, "%" + model.FilterKey + "%")
                                    || EF.Functions.Like(c.LastName, "%" + model.FilterKey + "%")
                                     || EF.Functions.Like(i.InvoiceNumber, "%" + model.FilterKey + "%"))
                            && i.Status != Constants.InvoiceStatus.Deleted
                            select new CreditMemoListItemDto
                            {
                                Id = i.Id,
                                CustomerId = i.CustomerId,
                                CustomerName = (c.FirstName ?? "") + " "  + (c.LastName ?? ""),
                                Description = i.Remark,
                                Amount = i.CreditMemoService.Sum(x => x.Rate),
                                Discount = i.Discount,
                                Tax = i.Tax,
                                TotalAmount = i.TotalAmount,
                                NewAmmount=i.NewAmmount,
                                OldAmmount=i.OldAmmount,
                                DiffAmmount=i.DiffAmmount,
                                CreatedOn = i.CreatedOn,
                                Status = i.Status,
                                InvoiceNumber = i.InvoiceNumber,
                                SubTotal = i.SubTotal,
                                InvoiceId=i.InvoiceId,
                                CreditMemoNumber=i.CreditMemoNumber,



                            })
                            .AsNoTracking();

            var sortExpresstion = model.GetSortExpression();

            var pagedResult = new JqDataTableResponse<CreditMemoListItemDto>
            {
                RecordsTotal = await _dataContext.CreditMemo.CountAsync(x => x.Status != Constants.InvoiceStatus.Deleted),
                RecordsFiltered = await linqstmt.CountAsync(),
                Data = await linqstmt.OrderBy(sortExpresstion).Skip(model.Start).Take(model.Length).ToListAsync()
            };

            foreach (var creditMemoListItemDto in pagedResult.Data)
            {
                creditMemoListItemDto.CreatedOn = Utility.GetDateTime(creditMemoListItemDto.CreatedOn, null);
            }

            return pagedResult;
        }

        public async Task<CreditMemoDetailDto> GetDetailAsync(int id)
        {
            var creditmemo = await (from i in _dataContext.CreditMemo
                                 join c in _dataContext.Customers
                                 on i.CustomerId equals c.Id
                                 where i.Id == id
                                 select new CreditMemoDetailDto
                                 {
                                     Id = i.Id,
                                     CustomerId=i.CustomerId,
                                     Tax = i.Tax,
                                     Discount = i.Discount,
                                     TotalAmount = i.TotalAmount,
                                     Remark = i.Remark,
                                     Status = i.Status,
                                     CreatedOn = i.CreatedOn,
                                     InvoiceDate = i.InvoiceDate,
                                     StrInvoiceDate = i.StrInvoiceDate,
                                     DueDate = i.DueDate,
                                     StrDueDate = i.StrDueDate,
                                     PoSoNumber = i.PoSoNumber,
                                     InvoiceNumber = i.InvoiceNumber,
                                     OldAmmount=i.OldAmmount,
                                     NewAmmount=i.NewAmmount,
                                     DiffAmmount=i.DiffAmmount,
                                     SubTotal = i.SubTotal,
                                     InvoiceId=i.InvoiceId,
                                     CreditMemoNumber=i.CreditMemoNumber,
                                     Customer = new CustomerDetailDto
                                     {
                                         Id=c.Id,
                                         FirstName = c.FirstName,
                                         LastName = c.LastName,
                                         Email = c.Email,
                                         Phone = c.Phone,
                                        // Discount = c.Discount,

                                     },
                                     CreditMemoServiceDto = i.CreditMemoService.Select(x => new CreditMemoServiceDto
                                     {
                                         Id = x.ServiceId ?? 0,
                                         OldAmmount=x.OldAmmount,
                                         NewAmmount=x.NewAmmount,
                                         DiffAmmount=x.DiffAmmount,
                                         CreditMemoId = x.CreditMemoId,
                                         ServiceId = x.ServiceId,
                                         ProductId = x.ProductId,
                                         Rate = x.Rate,
                                         Price = x.Price,
                                         TaxId = x.TaxId,
                                         TaxPrice = x.TaxPrice,
                                         TaxPercentage = x.TaxPercentage,
                                         LineAmount = x.LineAmount,
                                         OldQuantity = x.OldQuantity,
                                         NewQuantity = x.NewQuantity,
                                      //   BankAccountId = x.Product.BankAccountId,
                                         TaxDiffAmmount=x.TaxDiffAmmount
                                         


                                     })
                                     })
                                     

                                 
                          .AsNoTracking()
                          .SingleOrDefaultAsync();

            return creditmemo;
        }


        public async Task<CreditMemoDetailDto> GetCreaditMemoforInvoice(int id)
        {
            var creditmemo = await (from i in _dataContext.CreditMemo
                                    where i.InvoiceId == id
                                    select new CreditMemoDetailDto
                                    {
                                        Id = i.Id,
                                        CustomerId = i.CustomerId,
                                        Tax = i.Tax,
                                        Discount = i.Discount,
                                        TotalAmount = i.TotalAmount,
                                        OldAmmount=i.OldAmmount,
                                        NewAmmount=i.NewAmmount,
                                        DiffAmmount=i.DiffAmmount,
                                        Remark = i.Remark,
                                        Status = i.Status,
                                        CreatedOn = i.CreatedOn,
                                        InvoiceDate = i.InvoiceDate,
                                        StrInvoiceDate = i.StrInvoiceDate,
                                        DueDate = i.DueDate,
                                        StrDueDate = i.StrDueDate,
                                        PoSoNumber = i.PoSoNumber,
                                        InvoiceNumber = i.InvoiceNumber,
                                        SubTotal = i.SubTotal,
                                        InvoiceId = i.InvoiceId,
                                        CreditMemoNumber = i.CreditMemoNumber,
                                       
                                        
                                    })



                          .AsNoTracking()
                          .SingleOrDefaultAsync();

            return creditmemo;
        }

    }
}
