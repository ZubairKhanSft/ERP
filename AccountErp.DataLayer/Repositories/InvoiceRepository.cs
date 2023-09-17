using AccountErp.Dtos;
using AccountErp.Dtos.Address;
using AccountErp.Dtos.Customer;
using AccountErp.Dtos.Invoice;
using AccountErp.Dtos.Payment;
using AccountErp.Dtos.ShippingAddress;
using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.Invoice;
using AccountErp.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AccountErp.DataLayer.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly DataContext _dataContext;

        public InvoiceRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Invoice entity)
        {
            await _dataContext.Invoices.AddAsync(entity);
        }

        public void Edit(Invoice entity)
        {

            _dataContext.Invoices.Update(entity);
        }

        public async Task<Invoice> GetAsync(int id)
        {
            return await _dataContext.Invoices
                .Include(x => x.Services)
                .Include(x => x.Attachments)
                .SingleAsync(x => x.Id == id);
        }

        public async Task<InvoiceDetailDto> GetDetailAsync(int id)
        {
            var data = await _dataContext.Invoices.Where(x => x.Id == id).Select(s=> s.CreatedBy).FirstOrDefaultAsync();
            int createdBy = Convert.ToInt32(data);
            var invoice = await (from i in _dataContext.Invoices
                                 join c in _dataContext.Customers
                                 on i.CustomerId equals c.Id

                                 join user in _dataContext.User on i.AppovedId equals user.Id into userDetail
                                 from u in userDetail.DefaultIfEmpty()

                                 join t in _dataContext.CurrencyCr on i.CurrencyId equals t.Id
                                 into cl
                                 from cl1 in cl.DefaultIfEmpty()


                                 join user1 in _dataContext.User
                                 on createdBy equals user1.Id
                                 join add in _dataContext.ShippingAddress on c.Id equals add.CustomerId into shipadd
                                 from ship in shipadd.DefaultIfEmpty()
                                 where i.Id == id && i.Status != Constants.InvoiceStatus.Deleted
                                 select new InvoiceDetailDto
                                 {
                                     Id = i.Id,
                                     CustomerId = i.CustomerId,
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
                                     SubTotal = i.SubTotal,
                                     InvoiceType = i.InvoiceType,
                                     IsApproved = i.IsApproved,
                                     AppovedId = i.AppovedId,
                                     AppoverName = u.FirstName + " " + u.LastName,
                                     CreatedBy = user1.FirstName + " " + user1.LastName,
                                     VatAmount = i.VatAmount,
                                     SalesOrderNumber = i.SalesOrderNumber,
                                    
                                     CurrencyId  = i.CurrencyId,
                                     CurrencyName = cl1.Name,
                                     CurrencySymbol = cl1.Symbol,
                                     ConversionRate = i.ConversionRate,
                                     TotalAmountAfterConversion = i.TotalAmountAfterConversion,
                                    
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
                                         ShippingAddress = (from ship in _dataContext.ShippingAddress where ship.Status != Constants.RecordStatus.Deleted && ship.CustomerId == c.Id select new ShippingAddressDto{ Id = ship.Id, CustomerId = ship.CustomerId,Address = ship.Address,City =  ship.City,State = ship.State,PostalCode = ship.PostalCode,Status = ship.Status,CreatedOn = ship.CreatedOn,ShippingMethod = ship.ShippingMethod}).FirstOrDefault(),
                                         PaymentCustomer = (from pay in _dataContext.Payment where pay.Status != Constants.RecordStatus.Deleted && pay.CustomerId == c.Id select new PaymentDetailDto { Id= pay.Id, CardNumber = pay.CardNumber, PaymentType = pay.PaymentType, Name = pay.Name, }).FirstOrDefault(),

                                         // Discount = c.Discount,
                                         /*Address = new AddressDto
                                         {
                                             StreetNumber = c.Address.StreetNumber,
                                             StreetName = c.Address.StreetName,
                                             City = c.Address.City,
                                             State = c.Address.State,
                                             CountryName = c.Address.Country.Name,
                                             PostalCode = c.Address.PostalCode
                                         }*/


                                     },
                                     Items = i.InvoiceType == 0 ? i.Services.Select(x => new InvoiceServiceDto
                                     {
                                         Id = x.ServiceId ?? 0,
                                       //  Type = x.Service.Name,
                                         Rate = x.Rate,
                                         Name = x.Product.Model,
                                       //  Description = x.Service.Description,
                                         Quantity = x.Quantity,
                                         Price = x.Price,
                                         TaxId = x.TaxId,
                                         TaxPrice = x.TaxPrice,
                                         TaxPercentage = x.TaxPercentage,
                                         LineAmount = x.LineAmount,
                                        
                                         ProductId = x.ProductId,
                                         ProductModelName = x.Product.Model,
                                         ProductSpecification = x.Product.Specification,
                                         ProductBrand = x.Product.Brands,
                                     }) :


                                      i.Services.Select(x => new InvoiceServiceDto
                                      {
                                          Id = x.ProductId ?? 0,
                                          //   Type = x.Product.Name,
                                          Name = x.Product.Model,

                                          Rate = x.Rate,
                                        //  Name = x.Product.Name,
                                        //  Description = x.Product.Description,
                                          Quantity = x.Quantity,
                                          Price = x.Price,
                                          TaxId = x.TaxId,
                                          TaxPrice = x.TaxPrice,
                                          TaxPercentage = x.TaxPercentage,
                                          LineAmount = x.LineAmount,
                                          // BankAccountId = x.Product.BankAccountId,
                                          // TaxBankAccountId = x.Product.BankAccountId
                                          ProductId = x.ProductId,
                                          ProductModelName = x.Product.Model,
                                          ProductSpecification = x.Product.Specification,
                                          ProductBrand = x.Product.Brands,


                                      })
                                     ,
                                     Attachments = i.Attachments.Select(x => new InvoiceAttachmentDto
                                     {
                                         Id = x.Id,
                                         Title = x.Title,
                                         FileName = x.FileName,
                                         OriginalFileName = x.OriginalFileName
                                     })
                                 })
                          .AsNoTracking()
                          .SingleOrDefaultAsync();

            return invoice;
        }



        public async Task<(List<InvoiceDetailDto> data, int count)> GetAllAsync(int PageSize,int Page,string FilterKey)
        {
            var response = new List<InvoiceDetailDto>();
            int count=0;
            count = await _dataContext.Invoices.Where(s => s.Status != Constants.InvoiceStatus.Deleted).CountAsync();
            if (PageSize != 0 && Page != 0)
            {


                 response = await (from i in _dataContext.Invoices
                                     join c in _dataContext.Customers
                                     on i.CustomerId equals c.Id
                                   join user in _dataContext.User on i.AppovedId equals user.Id into userDetail
                                   from u in userDetail.DefaultIfEmpty()


                                   join user in _dataContext.ShippingAddress on i.CustomerId equals user.CustomerId into shipDetail
                                   from ship in shipDetail.DefaultIfEmpty()

                                   join serv in _dataContext.InvoiceServices on i.Id equals serv.InvoiceId into serviceDetail
                                   from service in serviceDetail.DefaultIfEmpty()

                                       /*join pro in _dataContext.Product on service.ProductId equals pro.Id into productDetail
                                       from product in productDetail.DefaultIfEmpty()
    */

                                   join t in _dataContext.CurrencyCr on i.CurrencyId equals t.Id
                                into cl
                                   from cl1 in cl.DefaultIfEmpty()

                                   join paym in _dataContext.Payment on i.CustomerId equals paym.CustomerId into payment
                                   from pay in payment.DefaultIfEmpty()

                                   /*join add in _dataContext.ShippingAddress on c.Id equals add.CustomerId into shipadd
                                   from ship in shipadd.DefaultIfEmpty()*/
                                   where i.Status != Constants.InvoiceStatus.Deleted
                                    && (FilterKey == null
                                        || EF.Functions.Like(i.Id.ToString(), "%" + FilterKey + "%")
                                         || EF.Functions.Like(i.CustomerId.ToString(), "%" + FilterKey + "%")
                                         || EF.Functions.Like(c.FirstName.ToString(), "%" + FilterKey + "%")
                                          || EF.Functions.Like(i.InvoiceNumber.ToString(), "%" + FilterKey + "%"))
                                   select new InvoiceDetailDto
                                     {
                                         Id = i.Id,
                                         CustomerId = i.CustomerId,
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
                                         SubTotal = i.SubTotal,
                                         InvoiceType = i.InvoiceType,
                                         CreatedBy = _dataContext.User.Where(s=> s.Id.ToString() == i.CreatedBy).Select(s=>s.FirstName + " "+ s.LastName).FirstOrDefault() ,

                                         IsApproved = i.IsApproved,
                                         AppovedId = i.AppovedId,
                                         AppoverName = u.FirstName + " " +  u.LastName,
                                         VatAmount = i.VatAmount,
                                         SalesOrderNumber = i.SalesOrderNumber,
                                      /* ProductId = product.Id,
                                       ProductModel = product.Model,
                                       ProductBrands = product.Brands,
                                       ProductPartNumber = product.PartNumber,
                                       ProductRateAED = product.RateAED,
                                       ProductRateUSD = product.RateUSD,
                                       ProductSpecification = product.Specification,
                                       ProductSupplierCode = product.SupplierCode,
                                       ProductUAN = product.UAN,
                                       ProductUnits = product.Units,*/
                                         
                                         CurrencyId = i.CurrencyId  ,
                                         CurrencyName = cl1.Name,
                                         CurrencySymbol = cl1.Symbol,
                                         ConversionRate = i.ConversionRate,
                                         TotalAmountAfterConversion = i.TotalAmountAfterConversion,

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
                                           
                                           // ShippingAddress = (from ship in _dataContext.ShippingAddress where ship.Status != Constants.RecordStatus.Deleted && ship.CustomerId == c.Id select new ShippingAddressDto { Id = ship.Id, CustomerId = ship.CustomerId, Address = ship.Address, City = ship.City, State = ship.State, PostalCode = ship.PostalCode, Status = ship.Status, CreatedOn = ship.CreatedOn,ShippingMethod = ship.ShippingMethod }).FirstOrDefault(),
                                           // PaymentCustomer = (from pay in _dataContext.Payment where pay.Status != Constants.RecordStatus.Deleted && pay.CustomerId == c.Id select new PaymentDetailDto { Id = pay.Id, CardNumber = pay.CardNumber, PaymentType = pay.PaymentType, Name = pay.Name, }).FirstOrDefault(),

                                           // PaymentCustomer = (from pay in _dataContext.Payment where pay.Status != Constants.RecordStatus.Deleted && pay.CustomerId == c.Id select new PaymentDetailDto { Id= pay.Id, CardNumber = pay.CardNumber, PaymentType = pay.PaymentType, Name = pay.Name, }).FirstOrDefault(),
                                           PaymentCustomer = new PaymentDetailDto
                                           {
                                               Id = pay.Id,
                                               CustomerId = pay.CustomerId,
                                               Name = pay.Name,
                                               PaymentType = pay.PaymentType,
                                               CardNumber = pay.CardNumber,
                                               CreatedOn = pay.CreatedOn,
                                               Status = pay.Status,
                                           },
                                           ShippingAddress = new ShippingAddressDto
                                           {
                                               Id = ship.Id,
                                               Address = ship.Address,
                                               CustomerId = ship.CustomerId,
                                               City = ship.City,
                                               State = ship.State,
                                               PostalCode = ship.PostalCode,
                                               ShippingMethod = ship.ShippingMethod,
                                           },
                                           
                                           // Discount = c.Discount,
                                           /* Address = new AddressDto
                                            {
                                                StreetNumber = c.Address.StreetNumber,
                                                StreetName = c.Address.StreetName,
                                                City = c.Address.City,
                                                State = c.Address.State,
                                                CountryName = c.Address.Country.Name,
                                                PostalCode = c.Address.PostalCode
                                            }*/
                                       },

                                       Items = i.InvoiceType == 0 ? i.Services.Select(x => new InvoiceServiceDto
                                       {
                                           Id = x.ServiceId ?? 0,
                                           Type = x.Product.Model,

                                           Rate = x.Rate,
                                           //  Name = x.Service.Name,
                                           //  Description = x.Service.Description,
                                           Quantity = x.Quantity,
                                           Price = x.Price,
                                           TaxId = x.TaxId,
                                           TaxPrice = x.TaxPrice,
                                           TaxPercentage = x.TaxPercentage,
                                           LineAmount = x.LineAmount,
                                           Name = x.Product.Model,

                                           //  BankAccountId = x.Service.BankAccountId,
                                           //    TaxBankAccountId = x.Taxes.BankAccountId
                                           ProductId = x.ProductId,
                                           ProductModelName = x.Product.Model,
                                           ProductSpecification = x.Product.Specification,
                                           ProductBrand = x.Product.Brands,

                                       }) :


                                          i.Services.Select(x => new InvoiceServiceDto
                                          {
                                              Id = x.ProductId ?? 0,
                                              //   Type = x.Product.Name,
                                              Rate = x.Rate,
                                              //  Name = x.Product.Name,
                                              //  Description = x.Product.Description,
                                              Quantity = x.Quantity,
                                              Price = x.Price,
                                              TaxId = x.TaxId,
                                              TaxPrice = x.TaxPrice,
                                              TaxPercentage = x.TaxPercentage,
                                              LineAmount = x.LineAmount,

                                              Name = x.Product.Model,
                                              Description = x.Product.Specification,
                                              // BankAccountId = x.Product.BankAccountId,
                                              // TaxBankAccountId = x.Product.BankAccountId
                                              ProductId = x.ProductId,
                                              ProductModelName = x.Product.Model,
                                              ProductSpecification = x.Product.Specification,
                                              ProductBrand = x.Product.Brands,

                                          })
                                         ,

                                      
                                       Attachments = i.Attachments.Select(x => new InvoiceAttachmentDto
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
                response = await (from i in _dataContext.Invoices
                                     join c in _dataContext.Customers
                                     on i.CustomerId equals c.Id
                                     
                                     join user in _dataContext.User on i.AppovedId equals user.Id into userDetail
                                     from u in userDetail.DefaultIfEmpty()

                                  join user in _dataContext.ShippingAddress on i.CustomerId equals user.CustomerId into shipDetail
                                  from ship in shipDetail.DefaultIfEmpty()

                                  join t in _dataContext.CurrencyCr on i.CurrencyId equals t.Id
                                into cl
                                  from cl1 in cl.DefaultIfEmpty()


                                  join paym in _dataContext.Payment on i.CustomerId equals paym.CustomerId into payment
                                  from pay in payment.DefaultIfEmpty()
                                  where i.Status != Constants.InvoiceStatus.Deleted
                                   && (FilterKey == null
                                        || EF.Functions.Like(i.Id.ToString(), "%" + FilterKey + "%")
                                         || EF.Functions.Like(i.CustomerId.ToString(), "%" + FilterKey + "%")
                                         || EF.Functions.Like(c.FirstName.ToString(), "%" + FilterKey + "%")
                                          || EF.Functions.Like(i.InvoiceNumber.ToString(), "%" + FilterKey + "%"))

                                  select new InvoiceDetailDto
                                     {
                                         Id = i.Id,
                                         CustomerId = i.CustomerId,
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
                                         SubTotal = i.SubTotal,
                                         InvoiceType = i.InvoiceType,
                                       //  CreatedBy = i.CreatedBy,
                                         IsApproved = i.IsApproved,
                                         AppovedId = i.AppovedId,
                                         AppoverName = u.FirstName + " " + u.LastName,
                                         CreatedBy = _dataContext.User.Where(s => s.Id.ToString() == i.CreatedBy).Select(s => s.FirstName + " " + s.LastName).FirstOrDefault(),
                                         VatAmount = i.VatAmount,
                                         SalesOrderNumber = i.SalesOrderNumber,
                                       
                                         CurrencyId = i.CurrencyId,
                                         CurrencyName = cl1.Name,
                                         CurrencySymbol = cl1.Symbol,
                                         ConversionRate = i.ConversionRate,
                                         TotalAmountAfterConversion = i.TotalAmountAfterConversion,
                                     
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
                                          /* ShippingAddress = new Dtos.ShippingAddress.ShippingAddressDto
                                           {
                                               CustomerId = ship.CustomerId,
                                               Address = ship.Address,
                                               City = ship.City,
                                               State = ship.State,
                                               PostalCode = ship.PostalCode,
                                               Status = ship.Status,
                                               CreatedOn = ship.CreatedOn,
                                           },*/
                                          // ShippingAddress = (from ship in _dataContext.ShippingAddress where ship.Status != Constants.RecordStatus.Deleted && ship.CustomerId == c.Id select new ShippingAddressDto { Id = ship.Id, CustomerId = ship.CustomerId, Address = ship.Address, City = ship.City, State = ship.State, PostalCode = ship.PostalCode, Status = ship.Status, CreatedOn = ship.CreatedOn, ShippingMethod = ship.ShippingMethod }).FirstOrDefault(),
                                          //PaymentCustomer = (from pay in _dataContext.Payment where pay.Status != Constants.RecordStatus.Deleted && pay.CustomerId == c.Id select new PaymentDetailDto { Id = pay.Id, CardNumber = pay.CardNumber, PaymentType = pay.PaymentType, Name = pay.Name, }).FirstOrDefault(),
                                          // PaymentCustomer = (from pay in _dataContext.Payment where pay.Status != Constants.RecordStatus.Deleted && pay.CustomerId == c.Id select new PaymentDetailDto { Id= pay.Id, CardNumber = pay.CardNumber, PaymentType = pay.PaymentType, Name = pay.Name, }).FirstOrDefault(),
                                              PaymentCustomer = new PaymentDetailDto
                                              {
                                                  Id = pay.Id,
                                                  CustomerId = pay.CustomerId,
                                                  Name = pay.Name,
                                                  PaymentType = pay.PaymentType,
                                                  CardNumber = pay.CardNumber,
                                                  CreatedOn = pay.CreatedOn,
                                                  Status = pay.Status,
                                              },
                                              ShippingAddress = new ShippingAddressDto
                                              {
                                                  Id = ship.Id,
                                                  Address = ship.Address,
                                                  CustomerId = ship.CustomerId,
                                                  City = ship.City,
                                                  State = ship.State,
                                                  PostalCode = ship.PostalCode,
                                                  ShippingMethod = ship.ShippingMethod,
                                              }
                                          //Discount = c.Discount,
                                          /*Address = new AddressDto
                                          {
                                              StreetNumber = c.Address.,
                                              StreetName = c.Address.StreetName,
                                              City = c.Address.City,
                                              State = c.Address.State,
                                              CountryName = c.Address.Country.Name,
                                              PostalCode = c.Address.PostalCode
                                          }*/
                                         },
                                         Items = i.InvoiceType == 0 ? i.Services.Select(x => new InvoiceServiceDto
                                         {
                                             Id = x.ServiceId ?? 0,
                                             //  Type = x.Service.Name,
                                             Rate = x.Rate,
                                             //  Name = x.Service.Name,
                                             //  Description = x.Service.Description,
                                             Quantity = x.Quantity,
                                             Price = x.Price,
                                             TaxId = x.TaxId,
                                             TaxPrice = x.TaxPrice,
                                             TaxPercentage = x.TaxPercentage,
                                             LineAmount = x.LineAmount,
                                             Name = x.Product.Model,
                                             Description = x.Product.Specification,
                                             //  ProductSpecification = x.Product.Specification,


                                             //  BankAccountId = x.Service.BankAccountId,
                                             //    TaxBankAccountId = x.Taxes.BankAccountId

                                             ProductId = x.ProductId,
                                             ProductModelName = x.Product.Model,
                                             ProductSpecification = x.Product.Specification,
                                             ProductBrand = x.Product.Brands,

                                         }) :


                                          i.Services.Select(x => new InvoiceServiceDto
                                          {
                                              Id = x.ProductId ?? 0,
                                              //   Type = x.Product.Name,
                                              Rate = x.Rate,
                                              //  Name = x.Product.Name,
                                              //  Description = x.Product.Description,
                                              Quantity = x.Quantity,
                                              Price = x.Price,
                                              TaxId = x.TaxId,
                                              TaxPrice = x.TaxPrice,
                                              TaxPercentage = x.TaxPercentage,
                                              LineAmount = x.LineAmount,

                                              Name = x.Product.Model,
                                              Description = x.Product.Specification,
                                              // BankAccountId = x.Product.BankAccountId,
                                              // TaxBankAccountId = x.Product.BankAccountId
                                              ProductId = x.ProductId,
                                              ProductModelName = x.Product.Model,
                                              ProductSpecification = x.Product.Specification,
                                              ProductBrand = x.Product.Brands,

                                          })
                                         ,
                                         Attachments = i.Attachments.Select(x => new InvoiceAttachmentDto
                                         {
                                             Id = x.Id,
                                             Title = x.Title,
                                             FileName = x.FileName,
                                             OriginalFileName = x.OriginalFileName
                                         })
                                         
                                     })
                                     .AsNoTracking()
                                     .OrderByDescending(s => s.Id)
                                     .ToListAsync();

                return (response, count);

            }
        }


        public async Task<(List<InvoiceDetailDto> data, int count)> InvoiceReportAsync(int CustomerId, DateTime From, DateTime To)
        {
            var response = new List<InvoiceDetailDto>();
            int count = 0;
            DateTime d = DateTime.Now.Date;
            count = await _dataContext.Invoices.Where(s => s.Status != Constants.InvoiceStatus.Deleted && s.CustomerId == CustomerId && (s.InvoiceDate.Date >= From.Date && s.InvoiceDate.Date <= To.Date)).CountAsync();
           
                response = await (from i in _dataContext.Invoices
                                  join c in _dataContext.Customers
                                  on i.CustomerId equals c.Id

                                  join user in _dataContext.User on i.AppovedId equals user.Id into userDetail
                                  from u in userDetail.DefaultIfEmpty()

                                  join user in _dataContext.ShippingAddress on i.CustomerId equals user.CustomerId into shipDetail
                                  from ship in shipDetail.DefaultIfEmpty()

                                  join t in _dataContext.CurrencyCr on i.CurrencyId equals t.Id
                                into cl
                                  from cl1 in cl.DefaultIfEmpty()

                                  join paym in _dataContext.Payment on i.CustomerId equals paym.CustomerId into payment
                                  from pay in payment.DefaultIfEmpty()
                                  where i.Status != Constants.InvoiceStatus.Deleted && (i.CustomerId == CustomerId || CustomerId == 0) 
                                  && (i.InvoiceDate.Date >= From.Date && i.InvoiceDate.Date <= To.Date)

                                  select new InvoiceDetailDto
                                  {
                                      Id = i.Id,
                                      CustomerId = i.CustomerId,
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
                                      SubTotal = i.SubTotal,
                                      InvoiceType = i.InvoiceType,
                                      IsApproved = i.IsApproved,
                                      AppovedId = i.AppovedId,
                                      AppoverName = u.FirstName + " " + u.LastName,
                                      CreatedBy = _dataContext.User.Where(s => s.Id.ToString() == i.CreatedBy).Select(s => s.FirstName + " " + s.LastName).FirstOrDefault(),
                                      VatAmount = i.VatAmount,
                                      SalesOrderNumber = i.SalesOrderNumber,
                                     
                                      CurrencyId = i.CurrencyId,
                                      CurrencyName = cl1.Name,
                                      CurrencySymbol = cl1.Symbol,
                                      ConversionRate = i.ConversionRate,
                                      TotalAmountAfterConversion = i.TotalAmountAfterConversion,
                                      
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
                                          PaymentCustomer = new PaymentDetailDto
                                          {
                                              Id = pay.Id,
                                              CustomerId = pay.CustomerId,
                                              Name = pay.Name,
                                              PaymentType = pay.PaymentType,
                                              CardNumber = pay.CardNumber,
                                              CreatedOn = pay.CreatedOn,
                                              Status = pay.Status,
                                          },
                                          ShippingAddress = new ShippingAddressDto
                                          {
                                              Id = ship.Id,
                                              Address = ship.Address,
                                              CustomerId = ship.CustomerId,
                                              City = ship.City,
                                              State = ship.State,
                                              PostalCode = ship.PostalCode,
                                              ShippingMethod = ship.ShippingMethod,
                                          }
                                      },
                                      Items = i.InvoiceType == 0 ? i.Services.Select(x => new InvoiceServiceDto
                                      {
                                          Id = x.ServiceId ?? 0,
                                          Rate = x.Rate,
                                          Quantity = x.Quantity,
                                          Price = x.Price,
                                          TaxId = x.TaxId,
                                          TaxPrice = x.TaxPrice,
                                          TaxPercentage = x.TaxPercentage,
                                          LineAmount = x.LineAmount,
                                          Name = x.Product.Model,
                                          Description = x.Product.Specification,
                                          ProductId = x.ProductId,
                                          ProductModelName = x.Product.Model,
                                          ProductSpecification = x.Product.Specification,
                                          ProductBrand = x.Product.Brands,

                                      }) :


                                          i.Services.Select(x => new InvoiceServiceDto
                                          {
                                              Id = x.ProductId ?? 0,
                                              Rate = x.Rate,
                                              Quantity = x.Quantity,
                                              Price = x.Price,
                                              TaxId = x.TaxId,
                                              TaxPrice = x.TaxPrice,
                                              TaxPercentage = x.TaxPercentage,
                                              LineAmount = x.LineAmount,

                                              Name = x.Product.Model,
                                              Description = x.Product.Specification,
                                              ProductId = x.ProductId,
                                              ProductModelName = x.Product.Model,
                                              ProductSpecification = x.Product.Specification,
                                              ProductBrand = x.Product.Brands,

                                          })
                                         ,
                                      Attachments = i.Attachments.Select(x => new InvoiceAttachmentDto
                                      {
                                          Id = x.Id,
                                          Title = x.Title,
                                          FileName = x.FileName,
                                          OriginalFileName = x.OriginalFileName
                                      })

                                  })
                                     .AsNoTracking()
                                     .OrderByDescending(s => s.Id)
                                     .ToListAsync();

                return (response, count);

            
        }


        public async Task<(List<InvoiceDetailDto> data, int count)> InvoiceReportBySalesRepresentative(int userId, DateTime From, DateTime To)
        {
            var response = new List<InvoiceDetailDto>();
            int count = 0;
            DateTime d = DateTime.Now.Date;
            count = await _dataContext.Invoices.Where(s => s.Status != Constants.InvoiceStatus.Deleted && s.CreatedBy == userId.ToString() && (s.InvoiceDate.Date >= From.Date && s.InvoiceDate.Date <= To.Date)).CountAsync();

            response = await (from i in _dataContext.Invoices
                              join c in _dataContext.Customers
                              on i.CustomerId equals c.Id

                              join user in _dataContext.User on i.AppovedId equals user.Id into userDetail
                              from u in userDetail.DefaultIfEmpty()

                              join user in _dataContext.ShippingAddress on i.CustomerId equals user.CustomerId into shipDetail
                              from ship in shipDetail.DefaultIfEmpty()

                              join t in _dataContext.CurrencyCr on i.CurrencyId equals t.Id
                                into cl
                              from cl1 in cl.DefaultIfEmpty()

                              join paym in _dataContext.Payment on i.CustomerId equals paym.CustomerId into payment
                              from pay in payment.DefaultIfEmpty()
                              where i.Status != Constants.InvoiceStatus.Deleted && i.CreatedBy == userId.ToString()
                              && (i.InvoiceDate.Date >= From.Date && i.InvoiceDate.Date <= To.Date)

                              select new InvoiceDetailDto
                              {
                                  Id = i.Id,
                                  CustomerId = i.CustomerId,
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
                                  SubTotal = i.SubTotal,
                                  InvoiceType = i.InvoiceType,
                                  IsApproved = i.IsApproved,
                                  AppovedId = i.AppovedId,
                                  AppoverName = u.FirstName + " " + u.LastName,
                                  CreatedBy = _dataContext.User.Where(s => s.Id.ToString() == i.CreatedBy).Select(s => s.FirstName + " " + s.LastName).FirstOrDefault(),
                                  VatAmount = i.VatAmount,
                                  SalesOrderNumber = i.SalesOrderNumber,

                                  CurrencyId = i.CurrencyId,
                                  CurrencyName = cl1.Name,
                                  CurrencySymbol = cl1.Symbol,
                                  ConversionRate = i.ConversionRate,
                                  TotalAmountAfterConversion = i.TotalAmountAfterConversion,

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
                                      PaymentCustomer = new PaymentDetailDto
                                      {
                                          Id = pay.Id,
                                          CustomerId = pay.CustomerId,
                                          Name = pay.Name,
                                          PaymentType = pay.PaymentType,
                                          CardNumber = pay.CardNumber,
                                          CreatedOn = pay.CreatedOn,
                                          Status = pay.Status,
                                      },
                                      ShippingAddress = new ShippingAddressDto
                                      {
                                          Id = ship.Id,
                                          Address = ship.Address,
                                          CustomerId = ship.CustomerId,
                                          City = ship.City,
                                          State = ship.State,
                                          PostalCode = ship.PostalCode,
                                          ShippingMethod = ship.ShippingMethod,
                                      }
                                  },
                                  Items = i.InvoiceType == 0 ? i.Services.Select(x => new InvoiceServiceDto
                                  {
                                      Id = x.ServiceId ?? 0,
                                      Rate = x.Rate,
                                      Quantity = x.Quantity,
                                      Price = x.Price,
                                      TaxId = x.TaxId,
                                      TaxPrice = x.TaxPrice,
                                      TaxPercentage = x.TaxPercentage,
                                      LineAmount = x.LineAmount,
                                      Name = x.Product.Model,
                                      Description = x.Product.Specification,
                                      ProductId = x.ProductId,
                                      ProductModelName = x.Product.Model,
                                      ProductSpecification = x.Product.Specification,
                                      ProductBrand = x.Product.Brands,

                                  }) :


                                      i.Services.Select(x => new InvoiceServiceDto
                                      {
                                          Id = x.ProductId ?? 0,
                                          Rate = x.Rate,
                                          Quantity = x.Quantity,
                                          Price = x.Price,
                                          TaxId = x.TaxId,
                                          TaxPrice = x.TaxPrice,
                                          TaxPercentage = x.TaxPercentage,
                                          LineAmount = x.LineAmount,

                                          Name = x.Product.Model,
                                          Description = x.Product.Specification,
                                          ProductId = x.ProductId,
                                          ProductModelName = x.Product.Model,
                                          ProductSpecification = x.Product.Specification,
                                          ProductBrand = x.Product.Brands,

                                      })
                                     ,
                                  Attachments = i.Attachments.Select(x => new InvoiceAttachmentDto
                                  {
                                      Id = x.Id,
                                      Title = x.Title,
                                      FileName = x.FileName,
                                      OriginalFileName = x.OriginalFileName
                                  })

                              })
                                 .AsNoTracking()
                                 .OrderByDescending(s => s.Id)
                                 .ToListAsync();

            return (response, count);


        }

        public async Task<ReportCountDto> CountReportAsync()
        {
            ReportCountDto data = new ReportCountDto();
            data.TotalSales = await _dataContext.Invoices.CountAsync();
            data.TotalPurchase = await _dataContext.Bills.CountAsync();
            data.TotalApprovedInvoice = await _dataContext.Invoices.Where(s => s.Status != Constants.InvoiceStatus.Deleted && s.IsApproved == true).CountAsync();
            data.PurchaseTotalCost =  await _dataContext.Bills.Where(s => s.Status != Constants.BillStatus.Deleted).SumAsync(s => s.TotalAmount);
            int invoiceSum = 0;
            int billSum = 0;
            int sum = 0;
            int sum1 = 0;
            int totalCount = 0;
            var prod = await _dataContext.Product.Where(s => s.Status != Constants.RecordStatus.Deleted).ToListAsync();
            foreach (var item in prod)
            {
                 /*sum = _dataContext.Bills.Where(x =>
                                                        x.Status != Constants.BillStatus.Deleted)
             .Select(t => t.Items.Where(y => y.ProductId == item.Id).Sum(p => p.Quantity)).Sum();
*/
                sum = _dataContext.BillItems.Where(p=> p.ProductId == item.Id).Sum(p => p.Quantity);

                billSum = billSum + sum;


                 sum1 = _dataContext.Invoices.Where(x =>
                                                                 x.Status != Constants.InvoiceStatus.Deleted   )
              .Select(t => t.Services.Where(y => y.ProductId == item.Id).Sum(p => p.Quantity)).Sum();

                invoiceSum = invoiceSum + sum1;

               // totalCount = (billSum - invoiceSum) + totalCount;
                
            }
            data.QtyInStock = billSum - invoiceSum;
            return data;

            DateTime dateTime = DateTime.Now;
            var topTenSalesOfMonth = _dataContext.Invoices
          .Where(sale => sale.InvoiceDate.Month == dateTime.Month  && sale.InvoiceDate.Year == dateTime.Year)
          .OrderByDescending(sale => sale.TotalAmount)
          .Take(10)
          .ToList();
        }

        public async Task<List<InvoiceDetailDto>> TopSalesAsync()
        {
            DateTime dateTime = DateTime.Now;

            return await (from i in _dataContext.Invoices
                          join c in _dataContext.Customers
                          on i.CustomerId equals c.Id

                          join t in _dataContext.CurrencyCr on i.CurrencyId equals t.Id
                                into cl
                          from cl1 in cl.DefaultIfEmpty()
                          where i.InvoiceDate.Month == dateTime.Month && i.InvoiceDate.Year == dateTime.Year
                          
                          select new InvoiceDetailDto
                          {
                              Id = i.Id,
                              CustomerId = i.CustomerId,
                              Tax = i.Tax,
                              Discount = i.Discount,
                              TotalAmount = i.TotalAmount,
                              Remark = i.Remark,
                              InvoiceDate = i.InvoiceDate,
                              StrInvoiceDate = i.StrInvoiceDate,
                              DueDate = i.DueDate,
                              StrDueDate = i.StrDueDate,
                              PoSoNumber = i.PoSoNumber,
                              InvoiceNumber = i.InvoiceNumber,
                              SubTotal = i.SubTotal,
                              InvoiceType = i.InvoiceType,
                              Status = i.Status,
                              
                              CurrencyId = i.CurrencyId,
                              CurrencyName = cl1.Name,
                              CurrencySymbol = cl1.Symbol,
                              ConversionRate = i.ConversionRate,
                              TotalAmountAfterConversion = i.TotalAmountAfterConversion,
                              Customer = new CustomerDetailDto
                              {
                                  FirstName = c.FirstName,
                                  LastName = c.LastName,
                                  Phone = c.Phone,
                                  City = c.City,
                                  State = c.State,
                                  Status = c.Status,
                                  Zipcode = c.Zipcode,
                                  Address = c.Address,
                                  Country = c.Country,
                              },
                              Items = i.InvoiceType == 0 ? i.Services.Select(x => new InvoiceServiceDto
                              {
                                  Id = x.Id,
                                  Rate = x.Rate,
                                  Quantity = x.Quantity,
                                  Price = x.Price,
                                  TaxId = x.TaxId,
                                  TaxPrice = x.TaxPrice,
                                  TaxPercentage = x.TaxPercentage,
                                  LineAmount = x.LineAmount,
                                  ProductId = x.ProductId,
                                  ProductModelName = x.Product.Model,
                                  ProductSpecification = x.Product.Specification,
                                  ProductBrand = x.Product.Brands,

                              }) :


                                      i.Services.Select(x => new InvoiceServiceDto
                                      {
                                          Id = x.ProductId ?? 0,
                                          Rate = x.Rate,
                                          Quantity = x.Quantity,
                                          Price = x.Price,
                                          TaxId = x.TaxId,
                                          TaxPrice = x.TaxPrice,
                                          TaxPercentage = x.TaxPercentage,
                                          LineAmount = x.LineAmount,
                                          ProductId = x.ProductId,
                                          ProductModelName = x.Product.Model,
                                          ProductSpecification = x.Product.Specification,
                                          ProductBrand = x.Product.Brands,

                                      })
                                     ,
                              Attachments = i.Attachments.Select(x => new InvoiceAttachmentDto
                              {
                                  Id = x.Id,
                                  Title = x.Title,
                                  FileName = x.FileName,
                                  OriginalFileName = x.OriginalFileName
                              })
                          })
                           .AsNoTracking()
                           .OrderByDescending(p=> p.TotalAmount).Take(10)
                           .ToListAsync();
        }

        public async Task<InvoiceDetailDto> GetDetailAsyncforpyment(int id)
        {
            var invoice = await (from i in _dataContext.Invoices
                                 join c in _dataContext.Customers
                                 on i.CustomerId equals c.Id

                                 join user in _dataContext.User on i.AppovedId equals user.Id into userDetail
                                 from u in userDetail.DefaultIfEmpty()

                                 join user in _dataContext.ShippingAddress on i.CustomerId equals user.CustomerId into shipDetail
                                 from ship in shipDetail.DefaultIfEmpty()

                                 join t in _dataContext.CurrencyCr on i.CurrencyId equals t.Id
                                 into cl
                                 from cl1 in cl.DefaultIfEmpty()

                                 join paym in _dataContext.Payment on i.CustomerId equals paym.CustomerId into payment
                                 from pay in payment.DefaultIfEmpty()

                                 where i.Id == id && i.Status != Constants.InvoiceStatus.Deleted
                                 select new InvoiceDetailDto
                                 {
                                     Id = i.Id,
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
                                     SubTotal = i.SubTotal,
                                     InvoiceType = i.InvoiceType,
                                     CreatedBy = _dataContext.User.Where(s=> s.Id.ToString() == i.CreatedBy).Select(s=>s.FirstName + " "+ s.LastName).FirstOrDefault() ,
                                     VatAmount = i.VatAmount,
                                     SalesOrderNumber = i.SalesOrderNumber,

                                    
                                     CurrencyId = i.CurrencyId,
                                     CurrencyName = cl1.Name,
                                     CurrencySymbol = cl1.Symbol,
                                     ConversionRate = i.ConversionRate,
                                     TotalAmountAfterConversion = i.TotalAmountAfterConversion,

                                     Customer = new CustomerDetailDto
                                     {
                                         Id = c.Id,
                                         FirstName = c.FirstName,
                                         LastName = c.LastName,
                                         Email = c.Email,
                                         Phone = c.Phone,
                                        // ShippingAddress = (from ship in _dataContext.ShippingAddress where ship.Status != Constants.RecordStatus.Deleted && ship.CustomerId == c.Id select new ShippingAddressDto { Id = ship.Id, CustomerId = ship.CustomerId, Address = ship.Address, City = ship.City, State = ship.State, PostalCode = ship.PostalCode, Status = ship.Status, CreatedOn = ship.CreatedOn,ShippingMethod = ship.ShippingMethod }).FirstOrDefault(),
                                        // PaymentCustomer = (from pay in _dataContext.Payment where pay.Status != Constants.RecordStatus.Deleted && pay.CustomerId == c.Id select new PaymentDetailDto { Id= pay.Id, CardNumber = pay.CardNumber, PaymentType = pay.PaymentType, Name = pay.Name, }).FirstOrDefault(),
                                        PaymentCustomer = new PaymentDetailDto
                                        {
                                            Id = pay.Id,
                                            CustomerId = pay.CustomerId,
                                            Name = pay.Name,
                                            PaymentType = pay.PaymentType,
                                            CardNumber = pay.CardNumber,
                                            CreatedOn = pay.CreatedOn,
                                            Status = pay.Status,
                                        },
                                         ShippingAddress = new ShippingAddressDto
                                        {
                                            Id = ship.Id,
                                            Address = ship.Address,
                                            CustomerId = ship.CustomerId,
                                            City = ship.City,
                                            State = ship.State,
                                            PostalCode = ship.PostalCode,
                                            ShippingMethod = ship.ShippingMethod,
                                        }
                                         /*Discount = c.Discount,
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
                                     Items = i.InvoiceType == 0 ? i.Services.Select(x => new InvoiceServiceDto
                                     {
                                         Id = x.ServiceId ?? 0,
                                       //  Type = x.Service.Name,
                                         Rate = x.Rate,
                                       //  Name = x.Service.Name,
                                       //  Description = x.Service.Description,
                                         Quantity = x.Quantity,
                                         Price = x.Price,
                                         TaxId = x.TaxId,
                                         TaxPrice = x.TaxPrice,
                                         TaxPercentage = x.TaxPercentage,
                                         LineAmount = x.LineAmount,
                                         //  BankAccountId = x.Service.BankAccountId,
                                         //TaxBankAccountId = x.Taxes.BankAccountId
                                         /*  ProductId = x.Product.Id,
                                           ProductModel = x.Product.Model,
                                           ProductBrands = x.Product.Brands,
                                           ProductPartNumber = x.Product.PartNumber,
                                           ProductRateAED = x.Product.RateAED,
                                           ProductRateUSD = x.Product.RateUSD,
                                           ProductSpecification = x.Product.Specification,
                                           ProductSupplierCode = x.Product.SupplierCode,
                                           ProductUAN = x.Product.UAN,
                                           ProductUnits = x.Product.Units,*/
                                         ProductId = x.ProductId,
                                         ProductModelName = x.Product.Model,
                                         ProductSpecification = x.Product.Specification,
                                         ProductBrand = x.Product.Brands,

                                     }) :


                                      i.Services.Select(x => new InvoiceServiceDto
                                      {
                                          Id = x.ProductId ?? 0,
                                       //   Type = x.Product.Name,
                                          Rate = x.Rate,
                                        //  Name = x.Product.Name,
                                        //  Description = x.Product.Description,
                                          Quantity = x.Quantity,
                                          Price = x.Price,
                                          TaxId = x.TaxId,
                                          TaxPrice = x.TaxPrice,
                                          TaxPercentage = x.TaxPercentage,
                                          LineAmount = x.LineAmount,
                                          //  BankAccountId = x.Product.BankAccountId,
                                          // TaxBankAccountId = x.Taxes.BankAccountId
                                          /*  ProductId = x.Product.Id,
                                            ProductModel = x.Product.Model,
                                            ProductBrands = x.Product.Brands,
                                            ProductPartNumber = x.Product.PartNumber,
                                            ProductRateAED = x.Product.RateAED,
                                            ProductRateUSD = x.Product.RateUSD,
                                            ProductSpecification = x.Product.Specification,
                                            ProductSupplierCode = x.Product.SupplierCode,
                                            ProductUAN = x.Product.UAN,
                                            ProductUnits = x.Product.Units,*/
                                          ProductId = x.ProductId,
                                          ProductModelName = x.Product.Model,
                                          ProductSpecification = x.Product.Specification,
                                          ProductBrand = x.Product.Brands,

                                      })
                                     ,
                                     
                                     Attachments = i.Attachments.Select(x => new InvoiceAttachmentDto
                                     {
                                         Id = x.Id,
                                         Title = x.Title,
                                         FileName = x.FileName,
                                         OriginalFileName = x.OriginalFileName
                                     })
                                 })
                          .AsNoTracking()
                          .SingleOrDefaultAsync();

            return invoice;
        }


        public async Task<InvoiceDetailForEditDto> GetForEditAsync(int id)
        {
            return await (from i in _dataContext.Invoices
                          join c in _dataContext.Customers
                          on i.CustomerId equals c.Id
                          where i.Id == id
                          select new InvoiceDetailForEditDto
                          {
                              Id = i.Id,
                              CustomerId = i.CustomerId,
                              Tax = i.Tax,
                              Discount = i.Discount,
                              TotalAmount = i.TotalAmount,
                              Remark = i.Remark,
                              InvoiceDate = i.InvoiceDate,
                              StrInvoiceDate = i.StrInvoiceDate,
                              DueDate = i.DueDate,
                              StrDueDate = i.StrDueDate,
                              PoSoNumber = i.PoSoNumber,
                              InvoiceNumber = i.InvoiceNumber,
                              SubTotal = i.SubTotal,
                              InvoiceType = i.InvoiceType,
                              Status = i.Status,
                              Customer = new CustomerDetailDto
                              {
                                  FirstName = c.FirstName,
                                  LastName = c.LastName,
                                  Phone = c.Phone
                              },
                              Items = i.InvoiceType == 0 ? i.Services.Select(x => new InvoiceServiceDto
                              {
                                  Id = x.Id ,
                                //  Type = x.Service.Name,
                                  Rate = x.Rate,
                               //  Name = x.Service.Name,
                               //   Description = x.Service.Description,
                                  Quantity = x.Quantity,
                                  Price = x.Price,
                                  TaxId = x.TaxId,
                                  TaxPrice = x.TaxPrice,
                                  TaxPercentage = x.TaxPercentage,
                                  LineAmount = x.LineAmount,
                               //   BankAccountId = x.Service.BankAccountId,
                                 // TaxBankAccountId = x.Taxes.BankAccountId
                              }) :


                                      i.Services.Select(x => new InvoiceServiceDto
                                      {
                                          Id = x.ProductId ?? 0,
                                         // Type = x.Product.Name,
                                          Rate = x.Rate,
                                        //  Name = x.Product.Name,
                                        //  Description = x.Product.Description,
                                          Quantity = x.Quantity,
                                          Price = x.Price,
                                          TaxId = x.TaxId,
                                          TaxPrice = x.TaxPrice,
                                          TaxPercentage = x.TaxPercentage,
                                          LineAmount = x.LineAmount,
                                        //  BankAccountId = x.Product.BankAccountId,
                                         // TaxBankAccountId = x.Taxes.BankAccountId
                                      })
                                     ,
                              Attachments = i.Attachments.Select(x => new InvoiceAttachmentDto
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

        public async Task<JqDataTableResponse<InvoiceListItemDto>> GetPagedResultAsync(InvoiceJqDataTableRequestModel model)
        {
            if (model.Length == 0)
            {
                model.Length = Constants.DefaultPageSize;
            }

            var linqstmt = (from i in _dataContext.Invoices
                            join c in _dataContext.Customers
                            on i.CustomerId equals c.Id
                            join cm in _dataContext.CreditMemo
                            on i.Id equals cm.InvoiceId into inv
                            from cm in inv.DefaultIfEmpty()
                            where (model.CustomerId == null
                                    || i.CustomerId == model.CustomerId.Value)
                                && (model.FilterKey == null
                                    || EF.Functions.Like(c.FirstName, "%" + model.FilterKey + "%")
                                   //  || EF.Functions.Like(c.MiddleName, "%" + model.FilterKey + "%")
                                    || EF.Functions.Like(c.LastName, "%" + model.FilterKey + "%")
                                     || EF.Functions.Like(i.InvoiceNumber, "%" + model.FilterKey + "%"))
                            && i.Status != Constants.InvoiceStatus.Deleted
                            select new InvoiceListItemDto
                            {
                                Id = i.Id,
                                CustomerId = i.CustomerId,
                                CustomerName = (c.FirstName ?? "") + " " + (c.LastName ?? ""),
                                Description = i.Remark,
                                Amount = i.Services.Sum(x => x.Rate),
                                Discount = i.Discount,
                                Tax = i.Tax,
                                TotalAmount = i.TotalAmount,
                                CreatedOn = i.CreatedOn,
                                Status = i.Status,
                                InvoiceNumber = i.InvoiceNumber,
                                SubTotal = i.SubTotal,
                                InvoiceType = i.InvoiceType,
                                CreaditMemoId=cm.Id
                                
                                
                                
                            })
                            .AsNoTracking().GroupBy(p => p.Id).Select(grp => grp.FirstOrDefault());

            var sortExpresstion = model.GetSortExpression();

            var pagedResult = new JqDataTableResponse<InvoiceListItemDto>
            {
                RecordsTotal = await _dataContext.Invoices.CountAsync(x => x.Status != Constants.InvoiceStatus.Deleted),
                RecordsFiltered = await linqstmt.CountAsync(),
                Data = await linqstmt.OrderBy(sortExpresstion).Skip(model.Start).Take(model.Length).ToListAsync()
            };

            foreach (var invoiceListItemDto in pagedResult.Data)
            {
                invoiceListItemDto.CreatedOn = Utility.GetDateTime(invoiceListItemDto.CreatedOn, null);
            }

            return pagedResult;
        }

        public async Task<JqDataTableResponse<InvoiceListItemDto>> GetTopFiveInvoicesAsync(InvoiceJqDataTableRequestModel model)
        {
            if (model.Length == 0)
            {
                model.Length = Constants.DefaultPageSize;
            }
            model.Order[0].Dir = "desc";
            model.Order[0].Column = 4;
            var linqstmt = (from i in _dataContext.Invoices
                            join c in _dataContext.Customers
                            on i.CustomerId equals c.Id
                            where (model.CustomerId == null
                                    || i.CustomerId == model.CustomerId.Value)
                                && (model.FilterKey == null
                                    || EF.Functions.Like(c.FirstName, "%" + model.FilterKey + "%")
                                    || EF.Functions.Like(c.LastName, "%" + model.FilterKey + "%"))
                            && i.Status != Constants.InvoiceStatus.Deleted
                            select new InvoiceListItemDto
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
                                InvoiceNumber = i.InvoiceNumber,
                                SubTotal = i.SubTotal
                            })
                            .AsNoTracking().Take(5);

            var sortExpresstion = model.GetSortExpression();

            var pagedResult = new JqDataTableResponse<InvoiceListItemDto>
            {
                RecordsTotal = await _dataContext.Invoices.CountAsync(x => model.CustomerId == null || x.CustomerId == model.CustomerId.Value),
                RecordsFiltered = await linqstmt.CountAsync(),
                Data = await linqstmt.OrderBy(sortExpresstion).Skip(model.Start).Take(model.Length).ToListAsync()
            };

            foreach (var invoiceListItemDto in pagedResult.Data)
            {
                invoiceListItemDto.CreatedOn = Utility.GetDateTime(invoiceListItemDto.CreatedOn, null);
            }

            return pagedResult;
        }
        
        public async Task<List<InvoiceListItemDto>> GetRecentAsync()
        {
            var linqstmt = (from i in _dataContext.Invoices
                            join c in _dataContext.Customers
                            on i.CustomerId equals c.Id
                            where i.Status != Constants.InvoiceStatus.Deleted
                            select new InvoiceListItemDto
                            {
                                Id = i.Id,
                                CustomerId = i.CustomerId,
                                CustomerName = (c.FirstName ?? "") + " "  + (c.LastName ?? ""),
                                Description = i.Remark,
                                Tax = i.Tax ?? 0,
                                Amount = i.TotalAmount,
                                CreatedOn = i.CreatedOn,
                                InvoiceDate = i.InvoiceDate,
                                StrInvoiceDate = i.StrInvoiceDate,
                                DueDate = i.DueDate,
                                StrDueDate = i.StrDueDate,
                                PoSoNumber = i.PoSoNumber,
                                InvoiceNumber = i.InvoiceNumber,
                                SubTotal = i.SubTotal
                            })
                            .AsNoTracking();

            return await linqstmt.OrderByDescending(x => x.CreatedOn).Take(5).ToListAsync();
        }

        public async Task<List<InvoiceListItemDto>> GetAllUnpaidInvoiceAsync()
        {
            var linqstmt = await (from i in _dataContext.Invoices
                            join c in _dataContext.Customers
                            on i.CustomerId equals c.Id
                            where i.Status != Constants.InvoiceStatus.Deleted && i.Status != Constants.InvoiceStatus.Paid
                            select new InvoiceListItemDto
                            {
                                Id = i.Id,
                                CustomerId = i.CustomerId,
                                CustomerName = (c.FirstName ?? "") + " "  + (c.LastName ?? ""),
                                Description = i.Remark,
                                Tax = i.Tax ?? 0,
                                Amount = i.TotalAmount,
                                CreatedOn = i.CreatedOn,
                                InvoiceDate = i.InvoiceDate,
                                StrInvoiceDate = i.StrInvoiceDate,
                                DueDate = i.DueDate,
                                StrDueDate = i.StrDueDate,
                                PoSoNumber = i.PoSoNumber,
                                InvoiceNumber = i.InvoiceNumber,
                                SubTotal = i.SubTotal
                            })
                            .AsNoTracking()
                            .ToListAsync();

                         return linqstmt;
        }

        public async Task<InvoiceSummaryDto> GetSummaryAsunc(int id)
        {
            return await (from i in _dataContext.Invoices
                          join c in _dataContext.Customers
                            on i.CustomerId equals c.Id
                          where i.Id == id
                          select new InvoiceSummaryDto
                          {
                              Id = i.Id,
                              CustomerId = c.Id,
                              FirstName = c.FirstName,
                            //  MiddleName = c.MiddleName,
                              LastName = c.LastName,
                              Amount = i.Services.Sum(x => x.Rate),
                              Tax = i.Tax,
                              Discount = i.Discount,
                              TotalAmount = i.TotalAmount,
                              Description = i.Remark,
                              Status = i.Status,
                              CreatedOn = i.CreatedOn,
                              InvoiceDate = i.InvoiceDate,
                              StrInvoiceDate = i.StrInvoiceDate,
                              DueDate = i.DueDate,
                              StrDueDate = i.StrDueDate,
                              PoSoNumber = i.PoSoNumber,
                              InvoiceNumber = i.InvoiceNumber,
                              InvoiceType = i.InvoiceType
                          })
                          .AsNoTracking()
                          .SingleOrDefaultAsync();
        }

        public async Task UpdateStatusAsync(int id, Constants.InvoiceStatus status)
        {
            var invoice = await _dataContext.Invoices.FindAsync(id);
            invoice.Status = status;
            _dataContext.Invoices.Update(invoice);
        }

        public async Task DeleteAsync(int id)
        {
            var invoice = await _dataContext.Invoices.Where(s=> s.Id == id).FirstOrDefaultAsync();
            invoice.Status = Constants.InvoiceStatus.Deleted;
            _dataContext.Invoices.Update(invoice);

            var invoiceService = await _dataContext.InvoiceServices.Where(s => s.InvoiceId == id).ToListAsync();
            _dataContext.RemoveRange(invoiceService);
            _dataContext.SaveChanges();

            var invoiceAttachment = await _dataContext.InvoiceAttachments.Where(s => s.InvoiceId == id).ToListAsync();
            _dataContext.RemoveRange(invoiceAttachment);
            _dataContext.SaveChanges();
        }

        public async Task<int> getCount()
        {
            int count = await _dataContext.Invoices.CountAsync();
            return count;
        }

        public async Task<List<InvoiceListTopTenDto>> GetTopTenInvoicesAsync()
        {
            var linqstmt = await (from i in _dataContext.Invoices
                                  join c in _dataContext.Customers
                                  on i.CustomerId equals c.Id
                                  where i.Status != Constants.InvoiceStatus.Deleted && i.Status == Constants.InvoiceStatus.Overdue
                                  select new InvoiceListTopTenDto
                                  {
                                      Id = i.Id,
                                      CustomerId = c.Id,
                                      CustomerName = (c.FirstName ?? "") + " "  + (c.LastName ?? ""),
                                      InvoiceNumber = i.InvoiceNumber,
                                      Amount = i.Services.Sum(x => x.Rate),
                                      TotalAmount = i.TotalAmount,
                                      InvoiceDate = i.InvoiceDate

                                  }).AsNoTracking().OrderBy("InvoiceDate ASC").ToListAsync();
                            //.AsNoTracking().Take(5).OrderBy("InvoiceDate ASC").ToListAsync();

            return linqstmt;
        }

        public async Task<IEnumerable<SelectListItemDto>> GetSelectInoviceAsync()
        {
            return await _dataContext.Invoices
                .AsNoTracking()
                .Where(x => x.Status != Constants.InvoiceStatus.Deleted && x.InvoiceType == Constants.InvoiceType.Product)
                .OrderBy(x => x.InvoiceNumber)
                .Select(x => new SelectListItemDto
                {
                    KeyInt = x.Id,
                    Value = x.InvoiceNumber
                }).ToListAsync();
        }

        public async Task AddInvoiceService(InvoiceService service)
        {
            await _dataContext.InvoiceServices.AddAsync(service);
            await _dataContext.SaveChangesAsync();
        }

        public async Task AddInvoiceAttachment(List<InvoiceAttachment> entity)
        {
            await _dataContext.InvoiceAttachments.AddRangeAsync(entity);
            await _dataContext.SaveChangesAsync();
        }
       
        public async Task DeleteInvoiceService(int invoiceId)
        {
            var data = await _dataContext.InvoiceServices.Where(s=> s.InvoiceId == invoiceId).ToListAsync();
             _dataContext.RemoveRange(data);
            _dataContext.SaveChanges();
        }

        public async Task AddMultipleInvoiceService(List<InvoiceService> entity)
        {
            await _dataContext.InvoiceServices.AddRangeAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteAttachmentByInvoiceId(int invoiceId)
        {
            var data = await _dataContext.InvoiceAttachments.Where(s => s.InvoiceId == invoiceId).ToListAsync();
            _dataContext.RemoveRange(data);
            _dataContext.SaveChanges();
        }

        public async Task ApproveInvoiceAsync(int invoiceId,int userId)
        {
            var data = await _dataContext.Invoices.Where(s => s.Id == invoiceId).FirstOrDefaultAsync();
            if(data.IsApproved == false)
            {
                    data.IsApproved = true;

            }
            else
            {
                    data.IsApproved = false;

            }
            data.AppovedId = userId;
            _dataContext.Invoices.Update(data);
            _dataContext.SaveChanges();
        }
    }
}
