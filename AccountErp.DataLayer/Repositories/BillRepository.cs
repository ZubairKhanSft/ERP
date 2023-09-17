using AccountErp.Dtos;
using AccountErp.Dtos.Bill;
using AccountErp.Dtos.Vendor;
using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.Bill;
using AccountErp.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace AccountErp.DataLayer.Repositories
{
    public class BillRepository : IBillRepository
    {
        private readonly DataContext _dataContext;

        public BillRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(Bill entity)
        {
            await _dataContext.Bills.AddAsync(entity);
            await _dataContext.SaveChangesAsync();
        }

        public void Edit(Bill entity)
        {
            _dataContext.Bills.Update(entity);
        }

        public async Task<Bill> GetAsync(int id)
        {
            return await _dataContext.Bills
                .Include(x => x.Items)
                .Include(x => x.Attachments)
                .SingleAsync(x => x.Id == id);
        }

        public async Task<JqDataTableResponse<BillListItemDto>> GetPagedResultAsync(BillJqDataTableRequestModel model)
        {
            if (model.Length == 0)
            {
                model.Length = Constants.DefaultPageSize;
            }
            var filterKey = model.Search.Value;

            var linqstmt = (from b in _dataContext.Bills
                            join v in _dataContext.Vendors
                            on b.VendorId equals v.Id
                            where 
                             (model.VendorId == null || b.VendorId == model.VendorId.Value)
                            && (model.FilterKey == null
                            || EF.Functions.Like(b.Id.ToString(), "%" + model.FilterKey + "%")
                              //  || EF.Functions.Like(v.Name, "%" + model.FilterKey + "%")
                                || EF.Functions.Like(b.BillNumber, "%" + model.FilterKey + "%")) 
                                && b.Status != Constants.BillStatus.Deleted
                            select new BillListItemDto
                            {
                                Id = b.Id,
                                VendorId = b.VendorId,
                             //   VendorName = v.Name,
                                Description = b.Remark,
                                Amount = b.Items.Sum(x => x.Rate),
                                Discount = b.Discount,
                                Tax = b.Tax,
                                TotalAmount = b.TotalAmount,
                                Status = b.Status,
                                CreatedOn = b.CreatedOn,
                                BillDate = b.BillDate,
                                DueDate = b.DueDate.Value,
                                StrBillDate = b.StrBillDate,
                                StrDueDate = b.StrDueDate,
                                Notes = b.Notes,
                                BillNumber = b.BillNumber,
                                SubTotal = b.SubTotal,
                                RefrenceNumber = b.RefrenceNumber,
                                BillType = b.BillType

                            })
                            .AsNoTracking();

            var sortExpression = model.GetSortExpression();

            var pagedResult = new JqDataTableResponse<BillListItemDto>
            {
                RecordsTotal = await _dataContext.Bills.CountAsync(x => x.Status != Constants.BillStatus.Deleted),
                RecordsFiltered = await linqstmt.CountAsync(),
                Data = await linqstmt.OrderBy(sortExpression).Skip(model.Start).Take(model.Length).ToListAsync()
            };

            return pagedResult;
        }
        
 public async Task<JqDataTableResponse<BillListItemDto>> getTopFiveBillsAsync(BillJqDataTableRequestModel model)
        {
            if (model.Length == 0)
            {
                model.Length = Constants.DefaultPageSize;
            }
            var filterKey = model.Search.Value;
            model.Order[0].Dir = "desc";
            model.Order[0].Column = 4;

            var linqstmt = (from b in _dataContext.Bills
                            join v in _dataContext.Vendors
                            on b.VendorId equals v.Id
                            where b.Status != Constants.BillStatus.Deleted
                            && (model.VendorId == null || b.VendorId == model.VendorId.Value)
                            && (model.FilterKey == null
                                || EF.Functions.Like(b.Id.ToString(), "%" + model.FilterKey + "%")
                              //  || EF.Functions.Like(v.Name, "%" + model.FilterKey + "%")
                                || EF.Functions.Like(b.BillNumber, "%" + model.FilterKey + "%"))
                            select new BillListItemDto
                            {
                                Id = b.Id,
                                VendorId = b.VendorId,
                             //   VendorName = v.Name,
                                Description = b.Remark,
                                Amount = b.Items.Sum(x => x.Rate),
                                Discount = b.Discount,
                                Tax = b.Tax,
                                TotalAmount = b.TotalAmount,
                                Status = b.Status,
                                CreatedOn = b.CreatedOn,
                                BillDate = b.BillDate,
                                DueDate = b.DueDate.Value,
                                StrBillDate = b.StrBillDate,
                                StrDueDate = b.StrDueDate,
                                Notes = b.Notes,
                                BillNumber = b.BillNumber,
                                SubTotal = b.SubTotal,
                                RefrenceNumber = b.RefrenceNumber,
                                BillType = b.BillType

                            })
                            .AsNoTracking().Take(5);

            var sortExpression = model.GetSortExpression();

            var pagedResult = new JqDataTableResponse<BillListItemDto>
            {
                RecordsTotal = await _dataContext.Bills.CountAsync(x => (model.VendorId == null || x.VendorId == model.VendorId.Value)
                    && x.Status != Constants.BillStatus.Deleted),
                RecordsFiltered = await linqstmt.CountAsync(),
                Data = await linqstmt.OrderBy(sortExpression).Skip(model.Start).Take(model.Length).ToListAsync()
            };

            return pagedResult;
        }

        public async Task<List<BillListItemDto>> GetRecentAsync()
        {
            var linqstmt = (from e in _dataContext.Bills
                            join v in _dataContext.Vendors
                            on e.VendorId equals v.Id
                            where e.Status == Constants.BillStatus.Pending
                            select new BillListItemDto
                            {
                                Id = e.Id,
                                VendorId = e.VendorId,
                               // VendorName = v.Name,
                                Tax = e.Tax ?? 0,
                                TotalAmount = e.TotalAmount,
                                Status = e.Status,
                                BillDate = e.BillDate,
                                DueDate = e.DueDate.Value,
                                StrBillDate = e.StrBillDate,
                                StrDueDate = e.StrDueDate,
                                Notes = e.Notes,
                                BillNumber = e.BillNumber,
                                PoSoNumber = e.PoSoNumber,
                                SubTotal = e.SubTotal,
                                RefrenceNumber = e.RefrenceNumber,
                                BillType = e.BillType
                            })
                            .AsNoTracking();

            return await linqstmt.OrderByDescending(x => x.Id).Take(5).ToListAsync();
        }

        public async Task<BillDetailDto> GetDetailAsync(int id)
        {
            var bill = await (from b in _dataContext.Bills
                          join v in _dataContext.Vendors
                          on b.VendorId equals v.Id

                              join t in _dataContext.CurrencyCr on b.CurrencyId equals t.Id
                             into cl
                              from cl1 in cl.DefaultIfEmpty()

                              where b.Id == id
                          select new BillDetailDto
                          {
                              Id = b.Id,
                           //   Name = b.Vendor.Name,
                              Phone = b.Vendor.Phone,
                              Email = b.Vendor.Email,
                              Tax = b.Tax,
                              TotalAmount = b.TotalAmount,
                              Remark = b.Remark,
                              Discount = b.Discount,
                              Status = b.Status,
                              CreatedOn = b.CreatedOn,
                              BillDate = b.BillDate,
                              DueDate = b.DueDate.Value,
                              StrBillDate = b.StrBillDate,
                              StrDueDate = b.StrDueDate,
                              Notes = b.Notes,
                              BillNumber = b.BillNumber,
                              PoSoNumber = b.PoSoNumber,
                              SubTotal = b.SubTotal,
                              RefrenceNumber = b.RefrenceNumber,
                              BillType = b.BillType,
                             
                              CurrencyId = b.CurrencyId,
                              ConversionRate = b.ConversionRate,
                              TotalAmountAfterConversion = b.TotalAmountAfterConversion,
                              CurrencyName = cl1.Name,
                              CurrencySymbol = cl1.Symbol,
                              Vendor = new VendorPersonallnfoDto
                              {
                                  Name = v.ShopName,
                                  Id = v.Id,
                                  /*Name = v.Name,
                                  HSTNumber = v.HSTNumber,*/
                                  Email = v.Email,
                                  Phone = v.Phone,
                                //  Discount = v.Discount
                              },
                              Items = b.Status != Constants.BillStatus.Deleted ? b.Items.Select(x => new BillServiceDto
                              {
                                  GId = x.Id,
                                  BillId = x.BillId,
                                  Id = x.ItemId ?? 0,
                                //  Type = x.Item.Name,
                                  Rate = x.Rate,
                                 // Name = x.Item.Name,
                                 // Description = x.Item.Description,
                                  Price = x.Price,
                                  TaxId = x.TaxId,
                                  TaxPercentage = x.TaxPercentage,
                                  Quantity = x.Quantity,
                                  TaxPrice = x.TaxPrice,
                                  LineAmount = x.LineAmount,
                                //  BankAccountId = x.Item.BankAccountId,
                                  TaxBankAccountId = x.Taxes.BankAccountId,
                                           Name = x.Product.Model,

                                 // ProductId = x.ProductId,
                                  ProductId = x.ProductId,
                                  ProductBrand = x.Product.Brands,
                                  ProductSpecification = x.Product.Specification,
                                  ProductModel = x.Product.Model,

                              })  :
                              b.Items.Select(x => new BillServiceDto
                              {
                                  GId = x.Id,
                                  Id = x.ProductId ?? 0,
                                 // ProductId = x.ProductId,
                                  //  Type = x.Product.Name,
                                  Rate = x.Rate,
                                //  Name = x.Product.Name,
                                //  Description = x.Product.Description,
                                  Price = x.Price,
                                  TaxId = x.TaxId,
                                  TaxPercentage = x.TaxPercentage,
                                  Quantity = x.Quantity,
                                  TaxPrice = x.TaxPrice,
                                  LineAmount = x.LineAmount,
                                //  BankAccountId = x.Product.BankAccountId,
                                  TaxBankAccountId = x.Taxes.BankAccountId,
                                  BillId = x.BillId,
                                  Name = x.Product.Model,
                                  ProductId = x.ProductId,
                                  ProductBrand = x.Product.Brands,
                                  ProductSpecification = x.Product.Specification,
                                  ProductModel = x.Product.Model,


                              })
                              ,
                              Attachments = b.Attachments.Select(x => new BillAttachmentDto
                              {
                                  Id = x.Id,
                                  Title = x.Title,
                                  FileName = x.FileName,
                                  OriginalFileName = x.OriginalFileName
                              })
                          })
                         .AsNoTracking()
                         .SingleOrDefaultAsync();

            return bill;
        }

        public async Task<(List<BillDetailDto>, int count)> GetAllPagination(int PageSize,int Page,int VendorId,string FilterKey,DateTime? BillDate)
        {
            int count = 0;
            var response = new List<BillDetailDto>();
            var response1 = new List<BillDetailDto>();
            count = await _dataContext.Bills.Where(s => s.Status != Constants.BillStatus.Deleted).CountAsync();
            if(PageSize !=0 && Page != 0 && BillDate != null)
            {
                response = await (from b in _dataContext.Bills
                              join v in _dataContext.Vendors
                              on b.VendorId equals v.Id

                                  join u in _dataContext.BillItems on b.Id equals u.BillId
                                 into bitem1
                                  from bitem in bitem1.DefaultIfEmpty()


                                  join t in _dataContext.CurrencyCr on b.CurrencyId equals t.Id
                                into cl
                                  from cl1 in cl.DefaultIfEmpty()

                                  where (b.Status != Constants.BillStatus.Deleted)
                               && (b.BillDate == BillDate || BillDate == null)
                                && (FilterKey == null
                                  //  || EF.Functions.Like(b.Id.ToString(), "%" + FilterKey + "%")
                                     || EF.Functions.Like(b.BillNumber.ToString(), "%" + FilterKey + "%")
                                     || EF.Functions.Like(b.Vendor.ShopName.ToString(), "%" + FilterKey + "%")
                                     || EF.Functions.Like(bitem.Quantity.ToString(), "%" + FilterKey + "%"))
                                 //     || EF.Functions.Like(b.BillDate.ToString("dd/MM/yyyy"), "%" + FilterKey + "%"))
                                  select new BillDetailDto
                              {
                                  Id = b.Id,
                                  //   Name = b.Vendor.Name,
                                  Phone = b.Vendor.Phone,
                                  Email = b.Vendor.Email,
                                  Tax = b.Tax,
                                  TotalAmount = b.TotalAmount,
                                  Remark = b.Remark,
                                  Discount = b.Discount,
                                  Status = b.Status,
                                  CreatedOn = b.CreatedOn,
                                  BillDate = b.BillDate,
                                  DueDate = b.DueDate.Value,
                                  StrBillDate = b.StrBillDate,
                                  StrDueDate = b.StrDueDate,
                                  Notes = b.Notes,
                                  BillNumber = b.BillNumber,
                                  PoSoNumber = b.PoSoNumber,
                                  SubTotal = b.SubTotal,
                                  RefrenceNumber = b.RefrenceNumber,
                                  BillType = b.BillType,
                                  CurrencyId = b.CurrencyId,
                                  ConversionRate = b.ConversionRate,
                                  TotalAmountAfterConversion = b.TotalAmountAfterConversion,
                                      CurrencyName = cl1.Name,
                                      CurrencySymbol = cl1.Symbol,
                                      Vendor = new VendorPersonallnfoDto
                                  {
                                      Name = v.ShopName,
                                      Id = v.Id,

                                      /*Name = v.Name,
                                      HSTNumber = v.HSTNumber,*/
                                      Email = v.Email,
                                      Phone = v.Phone,
                                      //  Discount = v.Discount
                                  },
                                  Items = b.BillType == 0 ? b.Items.Select(x => new BillServiceDto
                                  {
                                      Id = x.ItemId ?? 0,
                                    //  Type = x.Item.Name,
                                      Rate = x.Rate,
                                    //  Name = x.Item.Name,
                                    //  Description = x.Item.Description,
                                      Price = x.Price,
                                           Name = x.Product.Model,
                                      TaxId = x.TaxId,
                                      TaxPercentage = x.TaxPercentage,
                                      Quantity = x.Quantity,
                                      TaxPrice = x.TaxPrice,
                                      LineAmount = x.LineAmount,
                                     // BankAccountId = x.Item.BankAccountId,
                                      TaxBankAccountId = x.Taxes.BankAccountId,
                                      ProductId = x.ProductId,
                                      ProductBrand = x.Product.Brands,
                                      ProductSpecification = x.Product.Specification,
                                      ProductModel = x.Product.Model,

                                  }) :
                                  b.Items.Select(x => new BillServiceDto
                                  {
                                      Id = x.ProductId ?? 0,
                                      //  Type = x.Product.Name,
                                      Rate = x.Rate,
                                      //  Name = x.Product.Name,
                                      //  Description = x.Product.Description,
                                      Price = x.Price,
                                           Name = x.Product.Model,
                                      TaxId = x.TaxId,
                                      TaxPercentage = x.TaxPercentage,
                                      Quantity = x.Quantity,
                                      TaxPrice = x.TaxPrice,
                                      LineAmount = x.LineAmount,
                                      //  BankAccountId = x.Product.BankAccountId,
                                      TaxBankAccountId = x.Taxes.BankAccountId,
                                      ProductId = x.ProductId,
                                      ProductBrand = x.Product.Brands,
                                      ProductSpecification = x.Product.Specification,
                                      ProductModel = x.Product.Model,

                                  })
                                  ,
                                  Attachments = b.Attachments.Select(x => new BillAttachmentDto
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
                return (response, count);
            }

            else if (PageSize != 0 && Page != 0 && BillDate == null)
            {
                response = await (from b in _dataContext.Bills
                                  join v in _dataContext.Vendors
                                  on b.VendorId equals v.Id

                                  join t in _dataContext.CurrencyCr on b.CurrencyId equals t.Id
                                     into cl
                                  from cl1 in cl.DefaultIfEmpty()

                                  join u in _dataContext.BillItems on b.Id equals u.BillId
                             into bitem1
                                  from bitem in bitem1.DefaultIfEmpty()


                                  where (b.Status != Constants.BillStatus.Deleted)
                               && (b.BillDate == BillDate || BillDate == null)

                                    && (FilterKey == null
                                     //   || EF.Functions.Like(b.Id.ToString(), "%" + FilterKey + "%")
                                         || EF.Functions.Like(b.BillNumber.ToString(), "%" + FilterKey + "%")
                                         || EF.Functions.Like(b.Vendor.ShopName.ToString(), "%" + FilterKey + "%")
                                          || EF.Functions.Like(b.BillDate.ToString("dd/MM/yyyy"), "%" + FilterKey + "%")
                                          || EF.Functions.Like(bitem.Quantity.ToString(), "%" + FilterKey + "%"))
                                  select new BillDetailDto
                                  {
                                      Id = b.Id,
                                      //   Name = b.Vendor.Name,
                                      Phone = b.Vendor.Phone,
                                      Email = b.Vendor.Email,
                                      Tax = b.Tax,
                                      TotalAmount = b.TotalAmount,
                                      Remark = b.Remark,
                                      Discount = b.Discount,
                                      Status = b.Status,
                                      CreatedOn = b.CreatedOn,
                                      BillDate = b.BillDate,
                                      DueDate = b.DueDate.Value,
                                      StrBillDate = b.StrBillDate,
                                      StrDueDate = b.StrDueDate,
                                      Notes = b.Notes,
                                      BillNumber = b.BillNumber,
                                      PoSoNumber = b.PoSoNumber,
                                      SubTotal = b.SubTotal,
                                      RefrenceNumber = b.RefrenceNumber,
                                      BillType = b.BillType,
                                      CreatedBy = b.CreatedBy,
                                      CurrencyId = b.CurrencyId,
                                      ConversionRate = b.ConversionRate,
                                      TotalAmountAfterConversion = b.TotalAmountAfterConversion,
                                      CurrencyName = cl1.Name,
                                      CurrencySymbol = cl1.Symbol,
                                      Vendor = new VendorPersonallnfoDto
                                      {
                                          Name = v.ShopName,
                                          Id = v.Id,

                                          /*Name = v.Name,
                                          HSTNumber = v.HSTNumber,*/
                                          Email = v.Email,
                                          Phone = v.Phone,
                                          //  Discount = v.Discount
                                      },
                                      Items = b.BillType == 0 ? b.Items.Select(x => new BillServiceDto
                                      {
                                          Id = x.ItemId ?? 0,
                                          //  Type = x.Item.Name,
                                          Rate = x.Rate,
                                           Name = x.Product.Model,
                                          //  Description = x.Item.Description,
                                          Price = x.Price,
                                          TaxId = x.TaxId,
                                          TaxPercentage = x.TaxPercentage,
                                          Quantity = x.Quantity,
                                          TaxPrice = x.TaxPrice,
                                          LineAmount = x.LineAmount,
                                          // BankAccountId = x.Item.BankAccountId,
                                          TaxBankAccountId = x.Taxes.BankAccountId,
                                          
                                          ProductId = x.ProductId,
                                          ProductBrand = x.Product.Brands,
                                          ProductSpecification = x.Product.Specification,
                                          ProductModel = x.Product.Model,

                                      }) :
                                  b.Items.Select(x => new BillServiceDto
                                  {
                                      Id = x.ProductId ?? 0,
                                      //  Type = x.Product.Name,
                                      Rate = x.Rate,
                                      //  Name = x.Product.Name,
                                      //  Description = x.Product.Description,
                                      Price = x.Price,
                                           Name = x.Product.Model,
                                      TaxId = x.TaxId,
                                      TaxPercentage = x.TaxPercentage,
                                      Quantity = x.Quantity,
                                      TaxPrice = x.TaxPrice,
                                      LineAmount = x.LineAmount,
                                      //  BankAccountId = x.Product.BankAccountId,
                                      TaxBankAccountId = x.Taxes.BankAccountId,
                                      ProductId = x.ProductId,
                                      ProductBrand = x.Product.Brands,
                                      ProductSpecification = x.Product.Specification,
                                      ProductModel = x.Product.Model,

                                  })
                                  ,
                                      Attachments = b.Attachments.Select(x => new BillAttachmentDto
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
                return (response, count);
            }


            else
            {
                response = await (from b in _dataContext.Bills
                                  join v in _dataContext.Vendors
                                  on b.VendorId equals v.Id
                                  join t in _dataContext.CurrencyCr on b.CurrencyId equals t.Id
                                into cl
                                  from cl1 in cl.DefaultIfEmpty()

                                  join u in _dataContext.BillItems on b.Id equals u.BillId
                              into bitem1
                                  from bitem in bitem1.DefaultIfEmpty()

                                  where b.Status != Constants.BillStatus.Deleted
                                   && (VendorId == 0 || b.VendorId == VendorId)
                                   && (b.BillDate == BillDate || BillDate == null )
                                   && (FilterKey == null
                                     || EF.Functions.Like(b.BillNumber.ToString(), "%" + FilterKey + "%")

                                     //     || EF.Functions.Like(b.Id.ToString(), "%" + FilterKey + "%")
                                     || EF.Functions.Like(b.Vendor.ShopName.ToString(), "%" + FilterKey + "%")
                                      || EF.Functions.Like(b.BillDate.ToString("dd/MM/yyyy"), "%" + FilterKey + "%")
                                        || EF.Functions.Like(bitem.Quantity.ToString(), "%" + FilterKey + "%"))

                                  select new BillDetailDto
                                  {
                                      Id = b.Id,
                                      //   Name = b.Vendor.Name,
                                      Phone = b.Vendor.Phone,

                                      Email = b.Vendor.Email,
                                      Tax = b.Tax,
                                      TotalAmount = b.TotalAmount,
                                      Remark = b.Remark,
                                      Discount = b.Discount,
                                      Status = b.Status,
                                      CreatedOn = b.CreatedOn,
                                      BillDate = b.BillDate,
                                      DueDate = b.DueDate.Value,
                                      StrBillDate = b.StrBillDate,
                                      StrDueDate = b.StrDueDate,
                                      Notes = b.Notes,
                                      BillNumber = b.BillNumber,
                                      PoSoNumber = b.PoSoNumber,
                                      SubTotal = b.SubTotal,
                                      RefrenceNumber = b.RefrenceNumber,
                                      BillType = b.BillType,
                                      CurrencyId = b.CurrencyId,
                                      ConversionRate = b.ConversionRate,
                                      TotalAmountAfterConversion = b.TotalAmountAfterConversion,
                                      CurrencyName = cl1.Name,
                                      CurrencySymbol = cl1.Symbol,
                                      Vendor = new VendorPersonallnfoDto
                                      {
                                          Id = v.Id,

                                          Name = v.ShopName,
                                          //  HSTNumber = v.HSTNumber,
                                          Email = v.Email,
                                          Phone = v.Phone,
                                          //  Discount = v.Discount
                                      },
                                      Items = b.BillType == 0 ? b.Items.Select(x => new BillServiceDto
                                      {
                                          Id = x.ItemId ?? 0,
                                          //  Type = x.Item.Name,
                                          Rate = x.Rate,
                                          //  Name = x.Item.Name,
                                          //  Description = x.Item.Description,
                                          Price = x.Price,
                                          Name = x.Product.Model,
                                          TaxId = x.TaxId,
                                          TaxPercentage = x.TaxPercentage,
                                          Quantity = x.Quantity,
                                          TaxPrice = x.TaxPrice,
                                          LineAmount = x.LineAmount,
                                          // BankAccountId = x.Item.BankAccountId,
                                          TaxBankAccountId = x.Taxes.BankAccountId,
                                          ProductId = x.ProductId,
                                          ProductBrand = x.Product.Brands,
                                          ProductSpecification = x.Product.Specification,
                                          ProductModel = x.Product.Model,


                                      }) :
                                  b.Items.Select(x => new BillServiceDto
                                  {
                                      Id = x.ProductId ?? 0,
                                      //  Type = x.Product.Name,
                                      Rate = x.Rate,
                                      //  Name = x.Product.Name,
                                      //  Description = x.Product.Description,
                                      Price = x.Price,
                                      Name = x.Product.Model,
                                      TaxId = x.TaxId,
                                      TaxPercentage = x.TaxPercentage,
                                      Quantity = x.Quantity,
                                      TaxPrice = x.TaxPrice,
                                      LineAmount = x.LineAmount,
                                      //  BankAccountId = x.Product.BankAccountId,
                                      TaxBankAccountId = x.Taxes.BankAccountId,
                                      ProductId = x.ProductId,
                                      ProductBrand = x.Product.Brands,
                                      ProductSpecification = x.Product.Specification,
                                      ProductModel = x.Product.Model,

                                  })
                                  ,
                                      Attachments = b.Attachments.Select(x => new BillAttachmentDto
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

             /*   foreach (var item in response)
                {
                    foreach (var i in item.Items)
                    {
                        if (i.Quantity.ToString().Contains(FilterKey))
                        {
                            response1.Add(item);
                        }
                    }
                }*/
                return (response, count);
            }

          /*  else
            {
                response = await (from b in _dataContext.Bills
                              join v in _dataContext.Vendors
                              on b.VendorId equals v.Id
                                  join t in _dataContext.CurrencyCr on b.CurrencyId equals t.Id
                                into cl
                                  from cl1 in cl.DefaultIfEmpty()

                                  where b.Status != Constants.BillStatus.Deleted
                                   && (VendorId == 0 || b.VendorId == VendorId)
                                   && (b.BillDate == BillDate || BillDate == null || )
                                   && (FilterKey == null
                                     || EF.Functions.Like(b.BillNumber.ToString(), "%" + FilterKey + "%")

                               //     || EF.Functions.Like(b.Id.ToString(), "%" + FilterKey + "%")
                                     || EF.Functions.Like(b.Vendor.ShopName.ToString(), "%" + FilterKey + "%")
                                      || EF.Functions.Like(b.BillDate.ToString("dd/MM/yyyy"), "%" + FilterKey + "%"))

                                  select new BillDetailDto
                              {
                                  Id = b.Id,
                                  //   Name = b.Vendor.Name,
                                  Phone = b.Vendor.Phone,
                                         
                                  Email = b.Vendor.Email,
                                      Tax = b.Tax,
                                  TotalAmount = b.TotalAmount,
                                  Remark = b.Remark,
                                  Discount = b.Discount,
                                  Status = b.Status,
                                  CreatedOn = b.CreatedOn,
                                  BillDate = b.BillDate,
                                  DueDate = b.DueDate.Value,
                                  StrBillDate = b.StrBillDate,
                                  StrDueDate = b.StrDueDate,
                                  Notes = b.Notes,
                                  BillNumber = b.BillNumber,
                                  PoSoNumber = b.PoSoNumber,
                                  SubTotal = b.SubTotal,
                                  RefrenceNumber = b.RefrenceNumber,
                                  BillType = b.BillType,
                                  CurrencyId = b.CurrencyId,
                                  ConversionRate = b.ConversionRate,
                                  TotalAmountAfterConversion = b.TotalAmountAfterConversion,
                                      CurrencyName = cl1.Name,
                                      CurrencySymbol = cl1.Symbol,
                                      Vendor = new VendorPersonallnfoDto
                                  {
                                      Id = v.Id,

                                      Name = v.ShopName,
                                    //  HSTNumber = v.HSTNumber,
                                      Email = v.Email,
                                      Phone = v.Phone,
                                      //  Discount = v.Discount
                                  },
                                  Items = b.BillType == 0 ? b.Items.Select(x => new BillServiceDto
                                  {
                                      Id = x.ItemId ?? 0,
                                    //  Type = x.Item.Name,
                                      Rate = x.Rate,
                                    //  Name = x.Item.Name,
                                    //  Description = x.Item.Description,
                                      Price = x.Price,
                                           Name = x.Product.Model,
                                      TaxId = x.TaxId,
                                      TaxPercentage = x.TaxPercentage,
                                      Quantity = x.Quantity,
                                      TaxPrice = x.TaxPrice,
                                      LineAmount = x.LineAmount,
                                     // BankAccountId = x.Item.BankAccountId,
                                      TaxBankAccountId = x.Taxes.BankAccountId


                                  }) :
                                  b.Items.Select(x => new BillServiceDto
                                  {
                                      Id = x.ProductId ?? 0,
                                      //  Type = x.Product.Name,
                                      Rate = x.Rate,
                                      //  Name = x.Product.Name,
                                      //  Description = x.Product.Description,
                                      Price = x.Price,
                                           Name = x.Product.Model,
                                      TaxId = x.TaxId,
                                      TaxPercentage = x.TaxPercentage,
                                      Quantity = x.Quantity,
                                      TaxPrice = x.TaxPrice,
                                      LineAmount = x.LineAmount,
                                      //  BankAccountId = x.Product.BankAccountId,
                                      TaxBankAccountId = x.Taxes.BankAccountId


                                  })
                                  ,
                                  Attachments = b.Attachments.Select(x => new BillAttachmentDto
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
            */
        }



        public async Task<(List<BillDetailDto>, int count)> PurchaseReportAsync(int vendorId,DateTime From,DateTime To)
        {
            int count = 0;
            var response = new List<BillDetailDto>();
            count = await _dataContext.Bills.Where(s => s.Status != Constants.BillStatus.Deleted).CountAsync();
            
                response = await (from b in _dataContext.Bills
                                  join v in _dataContext.Vendors
                                  on b.VendorId equals v.Id
                                  join t in _dataContext.CurrencyCr on b.CurrencyId equals t.Id
                                  into cl
                                  from cl1 in cl.DefaultIfEmpty()

                                  where b.Status != Constants.BillStatus.Deleted
                                   && ( b.VendorId == vendorId || vendorId == 0)
                                   && (b.BillDate.Date >= From.Date && b.BillDate.Date <= To.Date)

                                    
                                  select new BillDetailDto
                                  {
                                      Id = b.Id,
                                      //   Name = b.Vendor.Name,
                                      Phone = b.Vendor.Phone,

                                      Email = b.Vendor.Email,
                                      Tax = b.Tax,
                                      TotalAmount = b.TotalAmount,
                                      Remark = b.Remark,
                                      Discount = b.Discount,
                                      Status = b.Status,
                                      CreatedOn = b.CreatedOn,
                                      BillDate = b.BillDate,
                                      DueDate = b.DueDate.Value,
                                      StrBillDate = b.StrBillDate,
                                      StrDueDate = b.StrDueDate,
                                      Notes = b.Notes,
                                      BillNumber = b.BillNumber,
                                      PoSoNumber = b.PoSoNumber,
                                      SubTotal = b.SubTotal,
                                      RefrenceNumber = b.RefrenceNumber,
                                      BillType = b.BillType,
                                      CurrencyId = b.CurrencyId,
                                      ConversionRate = b.ConversionRate,
                                      TotalAmountAfterConversion = b.TotalAmountAfterConversion,
                                      CurrencyName = cl1.Name,
                                      CurrencySymbol = cl1.Symbol,
                                      Vendor = new VendorPersonallnfoDto
                                      {
                                          Id = v.Id,

                                          Name = v.ShopName,
                                          //  HSTNumber = v.HSTNumber,
                                          Email = v.Email,
                                          Phone = v.Phone,
                                          //  Discount = v.Discount
                                      },
                                      Items = b.BillType == 0 ? b.Items.Select(x => new BillServiceDto
                                      {
                                          Id = x.ItemId ?? 0,
                                          //  Type = x.Item.Name,
                                          Rate = x.Rate,
                                          //  Name = x.Item.Name,
                                          //  Description = x.Item.Description,
                                          Price = x.Price,
                                          Name = x.Product.Model,
                                          TaxId = x.TaxId,
                                          TaxPercentage = x.TaxPercentage,
                                          Quantity = x.Quantity,
                                          TaxPrice = x.TaxPrice,
                                          LineAmount = x.LineAmount,
                                          // BankAccountId = x.Item.BankAccountId,
                                          TaxBankAccountId = x.Taxes.BankAccountId,
                                          ProductId = x.ProductId,
                                          ProductBrand = x.Product.Brands,
                                          ProductSpecification = x.Product.Specification,


                                      }) :
                                  b.Items.Select(x => new BillServiceDto
                                  {
                                      Id = x.ProductId ?? 0,
                                      //  Type = x.Product.Name,
                                      Rate = x.Rate,
                                      //  Name = x.Product.Name,
                                      //  Description = x.Product.Description,
                                      Price = x.Price,
                                      Name = x.Product.Model,
                                      TaxId = x.TaxId,
                                      TaxPercentage = x.TaxPercentage,
                                      Quantity = x.Quantity,
                                      TaxPrice = x.TaxPrice,
                                      LineAmount = x.LineAmount,
                                      //  BankAccountId = x.Product.BankAccountId,
                                      TaxBankAccountId = x.Taxes.BankAccountId,
                                      ProductId = x.ProductId,
                                      ProductBrand = x.Product.Brands,
                                      ProductSpecification = x.Product.Specification,


                                  })
                                  ,
                                      Attachments = b.Attachments.Select(x => new BillAttachmentDto
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

        public async Task<BillDetailForEditDto> GetDetailForEditAsync(int id)
        {
            return await (from e in _dataContext.Bills
                          join v in _dataContext.Vendors
                          on e.VendorId equals v.Id

                          where e.Id == id
                          select new BillDetailForEditDto
                          {
                              Id = e.Id,
                              VendorId = e.VendorId,
                              Phone = e.Vendor.Phone,
                              Email = e.Vendor.Email,
                              Tax = e.Tax,
                              TotalAmount = e.TotalAmount,
                              Remark = e.Remark,
                              Discount = e.Discount,
                              BillDate = e.BillDate,
                              DueDate = e.DueDate.Value,
                              StrBillDate = e.StrBillDate,
                              StrDueDate = e.StrDueDate,
                              Notes = e.Notes,
                              BillNumber = e.BillNumber,
                              PoSoNumber = e.PoSoNumber,
                              SubTotal = e.SubTotal,
                              RefrenceNumber = e.RefrenceNumber,
                              BillType = e.BillType,
                              Items = e.BillType == 0 ? e.Items.Select(x => new BillServiceDto
                              {
                                  Id = x.ItemId ?? 0,
                                //  Type = x.Item.Name,
                                  Rate = x.Rate,
                                //  Name = x.Item.Name,
                                //  Description = x.Item.Description,
                                  Price = x.Price,
                                  TaxId = x.TaxId,
                                  TaxPercentage = x.TaxPercentage,
                                  Quantity = x.Quantity,
                                  TaxPrice = x.TaxPrice,
                                  LineAmount = x.LineAmount,
                                 // BankAccountId = x.Item.BankAccountId,
                                  TaxBankAccountId = x.Taxes.BankAccountId,
                                  ProductId = x.ProductId,
                                  ProductBrand = x.Product.Brands,
                                  ProductSpecification = x.Product.Specification,

                              }) :
                              e.Items.Select(x => new BillServiceDto
                              {
                                  Id = x.ProductId ?? 0,
                                 // Type = x.Product.Name,
                                  Rate = x.Rate,
                                 // Name = x.Product.Name,
                                //  Description = x.Product.Description,
                                  Price = x.Price,
                                  TaxId = x.TaxId,
                                  TaxPercentage = x.TaxPercentage,
                                  Quantity = x.Quantity,
                                  TaxPrice = x.TaxPrice,
                                  LineAmount = x.LineAmount,
                                 // BankAccountId = x.Product.BankAccountId,
                                  TaxBankAccountId = x.Taxes.BankAccountId,
                                  ProductId = x.ProductId,
                                  ProductBrand = x.Product.Brands,
                                  ProductSpecification = x.Product.Specification,
                              })
                              ,
                              Attachments = e.Attachments.Select(x => new BillAttachmentDto
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

        public async Task<BillSummaryDto> GetSummaryAsunc(int id)
        {
            return await (from e in _dataContext.Bills
                          join v in _dataContext.Vendors
                              on e.VendorId equals v.Id
                          where e.Id == id
                          select new BillSummaryDto
                          {
                              Id = e.Id,
                              VendorId = v.Id,
                           //   VendorName = v.Name,
                              Tax = e.Tax,
                              Discount = e.Discount,
                              TotalAmount = e.TotalAmount,
                              Description = e.Remark,
                              BillDate = e.BillDate,
                              StrBillDate = e.StrBillDate,
                              DueDate = e.DueDate.Value,
                              StrDueDate = e.StrDueDate,
                              PoSoNumber = e.PoSoNumber,
                              Notes = e.Notes,
                              BillNumber = e.BillNumber,
                              SubTotal = e.SubTotal,
                              RefrenceNumber = e.RefrenceNumber,
                              BillType = e.BillType

                          })
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync()
        {
            return await _dataContext.Bills
                .AsNoTracking()
                .Where(x => x.Status != Constants.BillStatus.Deleted)
                .OrderBy(x => x.RefrenceNumber)
                .Select(x => new SelectListItemDto
                {
                    KeyInt = x.Id,
                    Value = x.RefrenceNumber
                }).ToListAsync();
        }

        public async Task UpdateStatusAsync(int id, Constants.BillStatus status)
        {
            var bill = await _dataContext.Bills.FindAsync(id);
            bill.Status = status;
            _dataContext.Bills.Update(bill);
        }

        public async Task DeleteAsync(int id)
        {
            var bill = await _dataContext.Bills.FindAsync(id);
            bill.Status = Constants.BillStatus.Deleted;
            _dataContext.Bills.Update(bill);
        }
        public async Task<int> getCount()
        {
            int count = await _dataContext.Bills.CountAsync();
            return count;
        }

        public async Task<List<BillListItemDto>> GetAllUnpaidAsync()
        {
            var linqstmt = await  (from e in _dataContext.Bills
                            join v in _dataContext.Vendors
                            on e.VendorId equals v.Id
                            where e.Status != Constants.BillStatus.Deleted && e.Status != Constants.BillStatus.Paid
                                   select new BillListItemDto
                            {
                                Id = e.Id,
                                VendorId = e.VendorId,
                             //   VendorName = v.Name,
                                Tax = e.Tax ?? 0,
                                TotalAmount = e.TotalAmount,
                                Status = e.Status,
                                BillDate = e.BillDate,
                                DueDate = e.DueDate.Value,
                                StrBillDate = e.StrBillDate,
                                StrDueDate = e.StrDueDate,
                                Notes = e.Notes,
                                BillNumber = e.BillNumber,
                                PoSoNumber = e.PoSoNumber,
                                SubTotal = e.SubTotal,
                                RefrenceNumber = e.RefrenceNumber,
                                BillType = e.BillType
                                   })
                            .AsNoTracking()
                            .ToListAsync();

            return linqstmt;
        }
    }
}
