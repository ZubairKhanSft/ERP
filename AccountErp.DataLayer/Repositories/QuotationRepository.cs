using AccountErp.Dtos.Quotation;
using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using AccountErp.Utilities;
using AccountErp.Dtos.Customer;
using AccountErp.Dtos.Address;
using AccountErp.Dtos.Invoice;
using AccountErp.Models.Quotation;

namespace AccountErp.DataLayer.Repositories
{
    public class QuotationRepository : IQuotationRepository
    {
        private readonly DataContext _dataContext;

        public QuotationRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Quotation entity)
        {
            await _dataContext.Quotations.AddAsync(entity);
        }

        public void Edit(Quotation entity)
        {

            _dataContext.Quotations.Update(entity);
        }

        public async Task<Quotation> GetAsync(int id)
        {
            return await _dataContext.Quotations
                .Include(x => x.Services)
                .Include(x => x.Attachments)
                .SingleAsync(x => x.Id == id);
        }

        public async Task<QuotationDetailDto> GetDetailAsync(int id)
        {
            var quotation = await (from i in _dataContext.Quotations
                                 join c in _dataContext.Customers
                                 on i.CustomerId equals c.Id

                                   join t in _dataContext.CurrencyCr on i.CurrencyId equals t.Id
                                 into cl
                                   from cl1 in cl.DefaultIfEmpty()
                                   where i.Id == id
                                 select new QuotationDetailDto
                                 {
                                     Id = i.Id,
                                     CustomerId = i.CustomerId,
                                     Tax = i.Tax,
                                     Discount = i.Discount,
                                     TotalAmount = i.TotalAmount,
                                     Remark = i.Remark,
                                     Status = i.Status,
                                     CreatedOn = i.CreatedOn,
                                     QuotationDate = i.QuotationDate,
                                     StrQuotationDate = i.StrQuotationDate,
                                     ExpiryDate = i.ExpireDate,
                                     StrExpiryDate = i.StrExpireDate,
                                     PoSoNumber = i.PoSoNumber,
                                     Memo = i.Memo,
                                     QuotationNumber = i.QuotationNumber,
                                     SubTotal = i.SubTotal,
                                     QuotationType = i.QuotationType,

                                     CurrencyId = i.CurrencyId,
                                     ConversionRate = i.ConversionRate,
                                     TotalAmountAfterConversion = i.TotalAmountAfterConversion,
                                     CurrencyName = cl1.Name,
                                     CurrencySymbol = cl1.Symbol,
                                     Customer = new CustomerDetailDto
                                     {
                                         FirstName = c.FirstName,
                                         LastName = c.LastName,
                                         Email = c.Email,
                                         Phone = c.Phone,
                                         Status = c.Status,
                                         Address = c.Address,
                                         City = c.City,
                                         State = c.State,
                                         Zipcode = c.Zipcode,
                                         Country = c.Country,
                                         /* Discount = c.Discount,
                                          Address = new AddressDto
                                          {
                                              StreetNumber = c.Address.StreetNumber,
                                              StreetName = c.Address.StreetName,
                                              City = c.Address.City,
                                              State = c.Address.State,
                                              CountryName = c.Address.Country.Name,
                                              PostalCode = c.Address.PostalCode
                                          }*/
                                     },
                                     Items = i.QuotationType == 0 ? i.Services.Select(x => new QuotationServiceDto
                                     {
                                         Id = x.ServiceId ?? 0,
                                         Type = x.Service.Name,
                                         Rate = x.Rate,
                                         Name = x.Service.Name,
                                         Description = x.Service.Description,
                                         Quantity = x.Quantity,
                                         Price = x.Price,
                                         TaxId = x.TaxId,
                                         TaxPrice = x.TaxPrice,
                                         TaxPercentage = x.TaxPercentage,
                                         LineAmount = x.LineAmount,
                                         BankAccountId = x.Service.BankAccountId,
                                         TaxBankAccountId = x.Taxes.BankAccountId,
                                        
                                         ProductId = x.ProductId,
                                         ProductModel = x.Product.Model,
                                         ProductBrand = x.Product.Brands,
                                         ProductSpecifiation = x.Product.Specification,
                                     }) :


                                      i.Services.Select(x => new QuotationServiceDto
                                      {
                                          Id = x.ProductId ?? 0,
                                      //    Type = x.Product.Name,
                                          Rate = x.Rate,
                                          Name = x.Product.Model,
                                        //  Description = x.Product.Description,
                                          Quantity = x.Quantity,
                                          Price = x.Price,
                                          TaxId = x.TaxId,
                                          TaxPrice = x.TaxPrice,
                                          TaxPercentage = x.TaxPercentage,
                                          LineAmount = x.LineAmount,
                                          //  BankAccountId = x.Product.BankAccountId,
                                          //  TaxBankAccountId = x.Product.BankAccountId
                                          ProductId = x.ProductId,
                                          ProductModel = x.Product.Model,
                                          ProductBrand = x.Product.Brands,
                                          ProductSpecifiation = x.Product.Specification,
                                      })
                                     ,
                                     Attachments = i.Attachments.Select(x => new QuotationAttachmentDto
                                     {
                                         Id = x.Id,
                                         Title = x.Title,
                                         FileName = x.FileName,
                                         OriginalFileName = x.OriginalFileName
                                     })
                                 })
                          .AsNoTracking()
                          .SingleOrDefaultAsync();

            return quotation;
        }

        public async Task<QuotationDeatilForEditDto> GetForEditAsync(int id)
        {
            return await (from i in _dataContext.Quotations
                          join c in _dataContext.Customers
                          on i.CustomerId equals c.Id
                          where i.Id == id
                          select new QuotationDeatilForEditDto
                          {
                              Id = i.Id,
                              CustomerId = i.CustomerId,
                              Tax = i.Tax,
                              Discount = i.Discount,
                              TotalAmount = i.TotalAmount,
                              Remark = i.Remark,
                              QuotationDate = i.QuotationDate,
                              StrQuotationDate = i.StrQuotationDate,
                              ExpiryDate = i.ExpireDate,
                              StrExpiryDate = i.StrExpireDate,
                              PoSoNumber = i.PoSoNumber,
                              Memo = i.Memo,
                              QuotationNumber = i.QuotationNumber,
                              SubTotal = i.SubTotal,
                              QuotationType = i.QuotationType,

                             /* CurrencyId = i.CurrencyId,
                              ConversionRate = i.ConversionRate,
                              TotalAmountAfterConversion = i.TotalAmountAfterConversion,*/
                              Customer = new CustomerDetailDto
                              {
                                  Id = c.Id,

                                  FirstName = c.FirstName,
                                  LastName = c.LastName,
                                  Phone = c.Phone,
                                  Email = c.Email,
                              },
                              Items = i.QuotationType == 0 ? i.Services.Select(x => new QuotationServiceDto
                              {
                                  Id = x.ServiceId ?? 0,
                                  Type = x.Service.Name,
                                  Rate = x.Rate,
                                  Name = x.Service.Name,
                                  Description = x.Service.Description,
                                  Quantity = x.Quantity,
                                  Price = x.Price,
                                  TaxId = x.TaxId,
                                  TaxPrice = x.TaxPrice,
                                  TaxPercentage = x.TaxPercentage,
                                  LineAmount = x.LineAmount,
                                  BankAccountId = x.Service.BankAccountId,
                                  TaxBankAccountId = x.Taxes.BankAccountId,
                                 
                                  ProductId = x.ProductId,
                                  ProductModel = x.Product.Model,
                                  ProductBrand = x.Product.Brands,
                                  ProductSpecifiation = x.Product.Specification,
                              }) :


                                      i.Services.Select(x => new QuotationServiceDto
                                      {
                                          Id = x.ProductId ?? 0,
                                         // Type = x.Product.Name,
                                          Rate = x.Rate,
                                          Name = x.Product.Model,
                                       //   Description = x.Product.Description,
                                          Quantity = x.Quantity,
                                          Price = x.Price,
                                          TaxId = x.TaxId,
                                          TaxPrice = x.TaxPrice,
                                          TaxPercentage = x.TaxPercentage,
                                          LineAmount = x.LineAmount,
                                          //  BankAccountId = x.Product.BankAccountId,
                                          //  TaxBankAccountId = x.Product.BankAccountId

                                          ProductId = x.ProductId,
                                          ProductModel = x.Product.Model,
                                          ProductBrand = x.Product.Brands,
                                          ProductSpecifiation = x.Product.Specification,
                                      })
                                     ,
                              Attachments = i.Attachments.Select(x => new QuotationAttachmentDto
                              {
                                  Id = x.Id,
                                  Title = x.Title,
                                  FileName = x.FileName,
                                  OriginalFileName = x.OriginalFileName
                              })
                          })
                           .AsNoTracking()
                           .SingleOrDefaultAsync();
        }

        public async Task<JqDataTableResponse<QuotationListItemDto>> GetPagedResultAsync(QuotationJqDataTableRequestModel model)
        {
            if (model.Length == 0)
            {
                model.Length = Constants.DefaultPageSize;
            }

            var linqstmt = (from i in _dataContext.Quotations
                            join c in _dataContext.Customers
                            on i.CustomerId equals c.Id
                            where (model.CustomerId == null
                                    || i.CustomerId == model.CustomerId.Value)
                                && (model.FilterKey == null
                                    || EF.Functions.Like(c.FirstName, "%" + model.FilterKey + "%")
                                    || EF.Functions.Like(c.LastName, "%" + model.FilterKey + "%")
                             //       || EF.Functions.Like(c.MiddleName, "%" + model.FilterKey + "%")
                                    || EF.Functions.Like(i.QuotationNumber, "%" + model.FilterKey + "%"))
                            && i.Status != Constants.InvoiceStatus.Deleted
                            select new QuotationListItemDto
                            {
                                Id = i.Id,
                                CustomerId = i.CustomerId,
                                CustomerName = (c.FirstName ?? "") + " "  + (c.LastName ?? ""),
                                Description = i.Remark,
                                Amount = i.Services.Sum(x => x.Rate),
                                Discount = i.Discount,
                                Tax = i.Tax,
                                TotalAmount = i.TotalAmount,
                                CreatedOn = i.CreatedOn,
                                Status = i.Status,
                                QuotationDate = i.QuotationDate,
                                StrQuotationDate = i.StrQuotationDate,
                                ExpiryDate = i.ExpireDate,
                                StrExpiryDate = i.StrExpireDate,
                                QuotationNumber = i.QuotationNumber,
                                SubTotal = i.SubTotal,
                                QuotationType = i.QuotationType

                            })
                            .AsNoTracking();

            var sortExpresstion = model.GetSortExpression();

            var pagedResult = new JqDataTableResponse<QuotationListItemDto>
            {
                RecordsTotal = await _dataContext.Quotations.CountAsync(x =>  x.Status != Constants.InvoiceStatus.Deleted),
                RecordsFiltered = await linqstmt.CountAsync(),
                Data = await linqstmt.OrderBy(sortExpresstion).Skip(model.Start).Take(model.Length).ToListAsync()
            };

            foreach (var quotationListItemDto in pagedResult.Data)
            {
                quotationListItemDto.CreatedOn = Utility.GetDateTime(quotationListItemDto.CreatedOn, null);
            }

            return pagedResult;
        }

        public async Task<(List<QuotationDetailDto>, int count)> GetAllAsync()
        {
            int count = 0;
            var response = new List<QuotationDetailDto>();
            count = await _dataContext.Quotations.CountAsync(s => s.Status != Constants.InvoiceStatus.Deleted); ;

            response = await (from i in _dataContext.Quotations
                              join c in _dataContext.Customers
                              on i.CustomerId equals c.Id
                              where i.Status != Constants.InvoiceStatus.Deleted
                              select new QuotationDetailDto
                              {
                                  Id = i.Id,
                                  CustomerId = i.CustomerId,
                                  QuotationNumber = i.QuotationNumber,
                              })
                            .AsNoTracking()
                            .OrderByDescending(s => s.Id)
                            .ToListAsync();
            return (response, count);
        }
        public async Task<(List<QuotationDetailDto>,int count)> GetAllAsync(int PageSize,int Page,string FilterKey,DateTime? QuotationDate)
        {
           
            int count = 0;
            var response = new List<QuotationDetailDto>();
            count = await _dataContext.Quotations.CountAsync(s => s.Status != Constants.InvoiceStatus.Deleted); ;
            if (PageSize > 0 && Page > 0)
            {
              response = await (from i in _dataContext.Quotations
                       join c in _dataContext.Customers
                       on i.CustomerId equals c.Id

                                join t in _dataContext.CurrencyCr on i.CurrencyId equals t.Id
                                        into cl
                                from cl1 in cl.DefaultIfEmpty()
                                where i.Status != Constants.InvoiceStatus.Deleted
                               && (i.QuotationDate == QuotationDate || QuotationDate == null)

                                  && (FilterKey == null
                                     //   || EF.Functions.Like(i.Id.ToString(), "%" + FilterKey + "%")
                                        || EF.Functions.Like(i.Customer.FirstName.ToString() + i.Customer.LastName.ToString(), "%" + FilterKey + "%")
                                        || EF.Functions.Like(c.FirstName.ToString(), "%" + FilterKey + "%")
                                        || EF.Functions.Like(i.QuotationNumber.ToString(), "%" + FilterKey + "%"))
                                select new QuotationDetailDto
                       {
                           Id = i.Id,
                           CustomerId = i.CustomerId,
                           Tax = i.Tax,
                           Discount = i.Discount,
                           TotalAmount = i.TotalAmount,
                           Remark = i.Remark,
                           Status = i.Status,
                           CreatedOn = i.CreatedOn,
                           QuotationDate = i.QuotationDate,
                           StrQuotationDate = i.StrQuotationDate,
                           ExpiryDate = i.ExpireDate,
                           StrExpiryDate = i.StrExpireDate,
                           PoSoNumber = i.PoSoNumber,
                           Memo = i.Memo,
                           QuotationNumber = i.QuotationNumber,
                           SubTotal = i.SubTotal,
                           QuotationType = i.QuotationType,
                           
                           CurrencyId = i.CurrencyId,
                           ConversionRate = i.ConversionRate,
                           TotalAmountAfterConversion = i.TotalAmountAfterConversion,
                           CurrencyName = cl1.Name,
                           CurrencySymbol = cl1.Symbol,
                           Customer = new CustomerDetailDto
                           {
                               Id = c.Id,
                               FirstName = c.FirstName,
                               LastName = c.LastName,
                               Email = c.Email,
                               Phone = c.Phone,
                               Status = c.Status,
                               Address = c.Address,
                               City = c.City,
                               State = c.State,
                               Zipcode = c.Zipcode,
                               Country = c.Country,
                               /* Discount = c.Discount,
                                Address = new AddressDto
                                {
                                    StreetNumber = c.Address.StreetNumber,
                                    StreetName = c.Address.StreetName,
                                    City = c.Address.City,
                                    State = c.Address.State,
                                    CountryName = c.Address.Country.Name,
                                    PostalCode = c.Address.PostalCode
                                }*/
                           },
                           Items = i.QuotationType == 0 ? i.Services.Select(x => new QuotationServiceDto
                           {
                               Id = x.ServiceId ?? 0,
                               Type = x.Service.Name,
                               Rate = x.Rate,
                               Name = x.Service.Name,
                               Description = x.Service.Description,
                               Quantity = x.Quantity,
                               Price = x.Price,
                               TaxId = x.TaxId,
                               TaxPrice = x.TaxPrice,
                               TaxPercentage = x.TaxPercentage,
                               LineAmount = x.LineAmount,
                               BankAccountId = x.Service.BankAccountId,
                               TaxBankAccountId = x.Taxes.BankAccountId,

                               ProductId = x.ProductId,
                               ProductModel = x.Product.Model,
                               ProductBrand = x.Product.Brands,
                               ProductSpecifiation = x.Product.Specification,
                           }) :


                            i.Services.Select(x => new QuotationServiceDto
                            {
                                Id = x.ProductId ?? 0,
                                //    Type = x.Product.Name,
                                Rate = x.Rate,
                                Name = x.Product.Model,
                                //  Description = x.Product.Description,
                                Quantity = x.Quantity,
                                Price = x.Price,
                                TaxId = x.TaxId,
                                TaxPrice = x.TaxPrice,
                                TaxPercentage = x.TaxPercentage,
                                LineAmount = x.LineAmount,
                                //  BankAccountId = x.Product.BankAccountId,
                                //  TaxBankAccountId = x.Product.BankAccountId

                                ProductId = x.ProductId,
                                ProductModel = x.Product.Model,
                                ProductBrand = x.Product.Brands,
                                ProductSpecifiation = x.Product.Specification,
                            })
                           ,
                           Attachments = i.Attachments.Select(x => new QuotationAttachmentDto
                           {
                               Id = x.Id,
                               Title = x.Title,
                               FileName = x.FileName,
                               OriginalFileName = x.OriginalFileName
                           })
                       })
                             .AsNoTracking()
                             .OrderByDescending(s => s.Id)
                             .Skip((Page - 1) * PageSize)
                             .Take(PageSize)
                             .ToListAsync();
                return (response,count);
                
            }
            else
            {
                response = await (from i in _dataContext.Quotations
                              join c in _dataContext.Customers
                              on i.CustomerId equals c.Id
                                  join t in _dataContext.CurrencyCr on i.CurrencyId equals t.Id
                                   into cl
                                  from cl1 in cl.DefaultIfEmpty()

                                  where i.Status != Constants.InvoiceStatus.Deleted
                               && (i.QuotationDate == QuotationDate || QuotationDate == null)
                                && (FilterKey == null
                                        //   || EF.Functions.Like(i.Id.ToString(), "%" + FilterKey + "%")
                                        || EF.Functions.Like(i.Customer.FirstName.ToString() + i.Customer.LastName.ToString(), "%" + FilterKey + "%")
                                        || EF.Functions.Like(c.FirstName.ToString(), "%" + FilterKey + "%")
                                        || EF.Functions.Like(i.QuotationNumber.ToString(), "%" + FilterKey + "%"))


                                  select new QuotationDetailDto
                              {
                                  Id = i.Id,
                                  CustomerId = i.CustomerId,
                                  Tax = i.Tax,
                                  Discount = i.Discount,
                                  TotalAmount = i.TotalAmount,
                                  Remark = i.Remark,
                                  Status = i.Status,
                                  CreatedOn = i.CreatedOn,
                                  QuotationDate = i.QuotationDate,
                                  StrQuotationDate = i.StrQuotationDate,
                                  ExpiryDate = i.ExpireDate,
                                  StrExpiryDate = i.StrExpireDate,
                                  PoSoNumber = i.PoSoNumber,
                                  Memo = i.Memo,
                                  QuotationNumber = i.QuotationNumber,
                                  SubTotal = i.SubTotal,
                                  QuotationType = i.QuotationType,

                                  CurrencyId = i.CurrencyId,
                                  ConversionRate = i.ConversionRate,
                                  TotalAmountAfterConversion = i.TotalAmountAfterConversion,
                                  CurrencyName = cl1.Name,
                                  CurrencySymbol = cl1.Symbol,
                                  Customer = new CustomerDetailDto
                                  {
                                      Id = c.Id,

                                      FirstName = c.FirstName,
                                      LastName = c.LastName,
                                      Email = c.Email,
                                      Phone = c.Phone,
                                      Status = c.Status,
                                      Address = c.Address,
                                      City = c.City,
                                      State = c.State,
                                      Zipcode = c.Zipcode,
                                      Country = c.Country,
                                      /* Discount = c.Discount,
                                       Address = new AddressDto
                                       {
                                           StreetNumber = c.Address.StreetNumber,
                                           StreetName = c.Address.StreetName,
                                           City = c.Address.City,
                                           State = c.Address.State,
                                           CountryName = c.Address.Country.Name,
                                           PostalCode = c.Address.PostalCode
                                       }*/
                                  },
                                  Items = i.QuotationType == 0 ? i.Services.Select(x => new QuotationServiceDto
                                  {
                                      Id = x.ServiceId ?? 0,
                                      Type = x.Service.Name,
                                      Rate = x.Rate,
                                      Name = x.Service.Name,
                                      Description = x.Service.Description,
                                      Quantity = x.Quantity,
                                      Price = x.Price,
                                      TaxId = x.TaxId,
                                      TaxPrice = x.TaxPrice,
                                      TaxPercentage = x.TaxPercentage,
                                      LineAmount = x.LineAmount,
                                      BankAccountId = x.Service.BankAccountId,
                                      TaxBankAccountId = x.Taxes.BankAccountId,

                                      ProductId = x.ProductId,
                                      ProductModel = x.Product.Model,
                                      ProductBrand = x.Product.Brands,
                                      ProductSpecifiation = x.Product.Specification,
                                  }) :


                                   i.Services.Select(x => new QuotationServiceDto
                                   {
                                       Id = x.ProductId ?? 0,
                                       //    Type = x.Product.Name,
                                       Rate = x.Rate,
                                       Name = x.Product.Model,
                                       //  Description = x.Product.Description,
                                       Quantity = x.Quantity,
                                       Price = x.Price,
                                       TaxId = x.TaxId,
                                       TaxPrice = x.TaxPrice,
                                       TaxPercentage = x.TaxPercentage,
                                       LineAmount = x.LineAmount,
                                       //  BankAccountId = x.Product.BankAccountId,
                                       //  TaxBankAccountId = x.Product.BankAccountId

                                       ProductId = x.ProductId,
                                       ProductModel = x.Product.Model,
                                       ProductBrand = x.Product.Brands,
                                       ProductSpecifiation = x.Product.Specification,
                                   })
                                  ,
                                  Attachments = i.Attachments.Select(x => new QuotationAttachmentDto
                                  {
                                      Id = x.Id,
                                      Title = x.Title,
                                      FileName = x.FileName,
                                      OriginalFileName = x.OriginalFileName
                                  })
                              })
                          .AsNoTracking()
                          .OrderByDescending(s=>s.Id)
                          .ToListAsync();
                return (response, count);

            }
        }



        public async Task<List<QuotationDetailDto>> QuotationReportAsync(int CustomerId, DateTime From,DateTime To)
        {
            int count = 0;
            var response = new List<QuotationDetailDto>();
            count = await _dataContext.Quotations.CountAsync(s => s.Status != Constants.InvoiceStatus.Deleted); ;
           
               
                response = await (from i in _dataContext.Quotations
                                  join c in _dataContext.Customers
                                  on i.CustomerId equals c.Id

                                  join t in _dataContext.CurrencyCr on i.CurrencyId equals t.Id
                               into cl
                                  from cl1 in cl.DefaultIfEmpty()
                                  where i.Status != Constants.InvoiceStatus.Deleted && (i.CustomerId == CustomerId || CustomerId == 0)
                                    && (i.QuotationDate.Date >= From.Date && i.QuotationDate.Date <= To.Date)
                                  select new QuotationDetailDto
                                  {
                                      Id = i.Id,
                                      CustomerId = i.CustomerId,
                                      Tax = i.Tax,
                                      Discount = i.Discount,
                                      TotalAmount = i.TotalAmount,
                                      Remark = i.Remark,
                                      Status = i.Status,
                                      CreatedOn = i.CreatedOn,
                                      QuotationDate = i.QuotationDate,
                                      StrQuotationDate = i.StrQuotationDate,
                                      ExpiryDate = i.ExpireDate,
                                      StrExpiryDate = i.StrExpireDate,
                                      PoSoNumber = i.PoSoNumber,
                                      Memo = i.Memo,
                                      QuotationNumber = i.QuotationNumber,
                                      SubTotal = i.SubTotal,
                                      QuotationType = i.QuotationType,

                                      CurrencyId = i.CurrencyId,
                                      ConversionRate = i.ConversionRate,
                                      TotalAmountAfterConversion = i.TotalAmountAfterConversion,
                                      CurrencyName = cl1.Name,
                                      CurrencySymbol = cl1.Symbol,
                                      Customer = new CustomerDetailDto
                                      {
                                          Id = c.Id,

                                          FirstName = c.FirstName,
                                          LastName = c.LastName,
                                          Email = c.Email,
                                          Phone = c.Phone,
                                          Address = c.Address,
                                          City = c.City,
                                          State = c.State,
                                          Zipcode = c.Zipcode,
                                          Status = c.Status,
                                          Country = c.Country,
                                          
                                          /* Discount = c.Discount,
                                           Address = new AddressDto
                                           {
                                               StreetNumber = c.Address.StreetNumber,
                                               StreetName = c.Address.StreetName,
                                               City = c.Address.City,
                                               State = c.Address.State,
                                               CountryName = c.Address.Country.Name,
                                               PostalCode = c.Address.PostalCode
                                           }*/
                                      },
                                      Items = i.QuotationType == 0 ? i.Services.Select(x => new QuotationServiceDto
                                      {
                                          Id = x.ServiceId ?? 0,
                                          Type = x.Service.Name,
                                          Rate = x.Rate,
                                          Name = x.Service.Name,
                                          Description = x.Service.Description,
                                          Quantity = x.Quantity,
                                          Price = x.Price,
                                          TaxId = x.TaxId,
                                          TaxPrice = x.TaxPrice,
                                          TaxPercentage = x.TaxPercentage,
                                          LineAmount = x.LineAmount,
                                          BankAccountId = x.Service.BankAccountId,
                                          TaxBankAccountId = x.Taxes.BankAccountId,

                                          ProductId = x.ProductId,
                                          ProductModel = x.Product.Model,
                                          ProductBrand = x.Product.Brands,
                                          ProductSpecifiation = x.Product.Specification,
                                      }) :


                                       i.Services.Select(x => new QuotationServiceDto
                                       {
                                           Id = x.ProductId ?? 0,
                                           //    Type = x.Product.Name,
                                           Rate = x.Rate,
                                           Name = x.Product.Model,
                                           //  Description = x.Product.Description,
                                           Quantity = x.Quantity,
                                           Price = x.Price,
                                           TaxId = x.TaxId,
                                           TaxPrice = x.TaxPrice,
                                           TaxPercentage = x.TaxPercentage,
                                           LineAmount = x.LineAmount,
                                           //  BankAccountId = x.Product.BankAccountId,
                                           //  TaxBankAccountId = x.Product.BankAccountId
                                           ProductId = x.ProductId,
                                           ProductModel = x.Product.Model,
                                           ProductBrand = x.Product.Brands,
                                           ProductSpecifiation = x.Product.Specification,

                                        
                                       })
                                      ,
                                      Attachments = i.Attachments.Select(x => new QuotationAttachmentDto
                                      {
                                          Id = x.Id,
                                          Title = x.Title,
                                          FileName = x.FileName,
                                          OriginalFileName = x.OriginalFileName
                                      })
                                  })
                          .AsNoTracking()
                          .OrderByDescending(s=> s.Id)
                          .ToListAsync();
                return (response);

            
        }



        public async Task<List<QuotationListItemDto>> GetRecentAsync()
        {
            var linqstmt = (from i in _dataContext.Quotations
                            join c in _dataContext.Customers
                            on i.CustomerId equals c.Id
                            where i.Status != Constants.InvoiceStatus.Deleted
                            select new QuotationListItemDto
                            {
                                Id = i.Id,
                                CustomerId = i.CustomerId,
                                CustomerName = (c.FirstName ?? "") + " "  + (c.LastName ?? ""),
                                Description = i.Remark,
                                Tax = i.Tax ?? 0,
                                Amount = i.TotalAmount,
                                CreatedOn = i.CreatedOn,
                                QuotationDate = i.QuotationDate,
                                StrQuotationDate = i.StrQuotationDate,
                                ExpiryDate = i.ExpireDate,
                                StrExpiryDate = i.StrExpireDate,
                                PoSoNumber = i.PoSoNumber,
                                Memo = i.Memo,
                                QuotationNumber = i.QuotationNumber,
                                SubTotal = i.SubTotal
                            })
                            .AsNoTracking();

            return await linqstmt.OrderByDescending(x => x.CreatedOn).Take(5).ToListAsync();
        }

        public async Task<QuotationSummary> GetSummaryAsunc(int id)
        {
            return await (from i in _dataContext.Quotations
                          join c in _dataContext.Customers
                            on i.CustomerId equals c.Id
                          where i.Id == id
                          select new QuotationSummary
                          {
                              Id = i.Id,
                              CustomerId = c.Id,
                              FirstName = c.FirstName,
                             // MiddleName = c.MiddleName,
                              LastName = c.LastName,
                              Amount = i.Services.Sum(x => x.Rate),
                              Tax = i.Tax,
                              Discount = i.Discount,
                              TotalAmount = i.TotalAmount,
                              Description = i.Remark,
                              Status = i.Status,
                              CreatedOn = i.CreatedOn,
                              QuotationDate = i.QuotationDate,
                              StrQuotationDate = i.StrQuotationDate,
                              ExpiryDate = i.ExpireDate,
                              StrExpiryDate = i.StrExpireDate,
                              PoSoNumber = i.PoSoNumber,
                              Memo = i.Memo,
                              QuotationNumber = i.QuotationNumber,
                              SubTotal = i.SubTotal,
                              QuotationType = i.QuotationType
                          })
                          .AsNoTracking()
                          .SingleOrDefaultAsync();
        }

        public async Task UpdateStatusAsync(int id, Constants.InvoiceStatus status)
        {
            var invoice = await _dataContext.Quotations.FindAsync(id);
            invoice.Status = status;
            _dataContext.Quotations.Update(invoice);
        }

        public async Task DeleteAsync(int id)
        {
            var invoice = await _dataContext.Quotations.FindAsync(id);
            invoice.Status = Constants.InvoiceStatus.Deleted;
            _dataContext.Quotations.Update(invoice);
        }

        public async Task<int> getCount()
        {
            int count = await _dataContext.Quotations.CountAsync();
            return count;
        }
    }
}
