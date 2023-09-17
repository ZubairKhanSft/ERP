using AccountErp.Entities;
using AccountErp.Models.Bill;
using AccountErp.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AccountErp.Factories
{
    public class BillFactory
    {
        public static Bill Create(BillAddModel model, string userId)
        {
            var bill = new Bill
            {
                /* VendorId = model.VendorId,
                 //BillNumber = "B.NO" + "-" + model.BillDate.ToString("yy") + "-" + (count + 1).ToString("000"),
                 BillNumber = model.BillNumber,
                 Tax = model.Tax,
                 Discount = model.Discount,
                 DueDate = model.DueDate,
                 TotalAmount = model.TotalAmount,
                 Remark = model.Remark,
                 RefrenceNumber = model.RefrenceNumber,
                 Status = Constants.BillStatus.Pending,
                 CreatedBy = userId ?? "0",
                 CreatedOn = Utility.GetDateTime(),
                 BillDate = model.BillDate,
                 StrBillDate = model.BillDate.ToString("dd/MM/yyyy"),
                 StrDueDate = model.DueDate.Value.ToString("dd/MM/yyyy"),
                 PoSoNumber = model.PoSoNumber,
                 Notes = model.Notes,
                 SubTotal = model.SubTotal,
                 LineAmountSubTotal = model.LineAmountSubTotal,
                 BillType = model.BillType,
                 Items = model.Items.Select(x => new BillItem
                 {
                     Id = Guid.NewGuid(),
                     ProductId = x.ProductId,
                     ItemId = x.ItemId,    //forget about this for now
                     Rate = x.Rate,
                     Price = x.Price,
                     TaxId = x.TaxId,
                     TaxPercentage = x.TaxPercentage,
                     Quantity = x.Quantity,
                     TaxPrice = x.TaxPrice,
                     LineAmount = x.LineAmount
                 }).ToList()
             };

             if (model.Attachments == null || !model.Attachments.Any())
             {
                 return bill;
             }

             //bill.Attachments = new List<BillAttachment>();

             bill.Attachments = model.Attachments.Select(x => new BillAttachment
             {
                 Title = x.Title,
                 FileName = x.FileName,
                 OriginalFileName = x.OriginalFileName,
                 CreatedBy = userId ?? "0",
                 CreatedOn = Utility.GetDateTime()
             }).ToList();*/

                VendorId = model.VendorId,
                //BillNumber = "B.NO" + "-" + model.BillDate.ToString("yy") + "-" + (count + 1).ToString("000"),
                BillNumber = model.BillNumber,
                Tax = model.Tax,
                Discount = model.Discount,
                TotalAmount = model.TotalAmount,
                DueDate = null,
                Remark = "",
                RefrenceNumber = "",
               /* Status = Constants.BillStatus.*/
                CreatedOn = Utility.GetDateTime(),
                BillDate = model.BillDate,
                StrBillDate = model.BillDate.ToString("dd/MM/yyyy"),
                StrDueDate = "",
                CreatedBy = "",
                PoSoNumber = null,
                Notes = null,
                
                CurrencyId = model.CurrencyId,
                ConversionRate = model.ConversionRate,
                TotalAmountAfterConversion = model.TotalAmountAfterConversion,

                SubTotal = model.SubTotal,
                LineAmountSubTotal = model.LineAmountSubTotal,
                BillType = Constants.InvoiceType.Product,
                /* Items = model.Items.Select(x => new BillItem
                 {
                     Id = Guid.NewGuid(),
                     ProductId = x.ProductId,
                     ItemId = x.ItemId,    //forget about this for now
                     Rate = x.Rate,
                     Price = x.Price,

                     Quantity = x.Quantity,

                     LineAmount = x.LineAmount,

                     TaxPercentage=0,
                     TaxPrice = 1,
                     TaxId = 1,
                 }).ToList()*/
                Items = model.Items.Select(x => new BillItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = x.ProductId,
                    ItemId = x.ItemId,    //forget about this for now
                    Rate = x.Rate,
                    Price = x.Price,
                    TaxId = null,
                    TaxPercentage = 0,
                    Quantity = x.Quantity,
                    TaxPrice = 0,
                    LineAmount = x.LineAmount
                }).ToList()
           
        };

            if (model.Attachments == null || !model.Attachments.Any())
            {
                return bill;
            }

            //bill.Attachments = new List<BillAttachment>();

            bill.Attachments = model.Attachments.Select(x => new BillAttachment
            {
                Title = x.Title,
                FileName = x.FileName,
                OriginalFileName = x.OriginalFileName,
                CreatedBy = userId ?? "0",
                CreatedOn = Utility.GetDateTime()
            }).ToList();
            return bill;
        }

        public static void Edit(Bill bill, BillEditModel model, string userId)
        {
            model.BillType = Constants.InvoiceType.Product;
            bill.VendorId = model.VendorId;
            bill.BillNumber = model.BillNumber;
            bill.Tax = model.Tax;
            bill.Discount = model.Discount;
            bill.TotalAmount = model.TotalAmount;
            bill.Remark = model.Remark;
            bill.DueDate = model.DueDate;
            bill.UpdatedBy = userId ?? "0";
            bill.UpdatedOn = Utility.GetDateTime();
            bill.BillDate = model.BillDate;
            bill.StrBillDate = model.BillDate.ToString("dd/MM/yyyy");
            bill.StrDueDate = model.DueDate.Value.ToString("dd/MM/yyyy")?? "";
            bill.PoSoNumber = model.PoSoNumber;
            bill.Notes = model.Notes;
            bill.SubTotal = model.SubTotal;
            bill.LineAmountSubTotal = model.LineAmountSubTotal;
            bill.RefrenceNumber = model.RefrenceNumber;
           
            bill.CurrencyId = model.CurrencyId;
            bill.ConversionRate = model.ConversionRate;
            bill.TotalAmountAfterConversion = model.TotalAmountAfterConversion;
           
            ArrayList tempArr = new ArrayList();

          
            foreach (var item in model.Items)
            {
                var alreadyExistServices = new BillItem();
                if (model.BillType == 0)
                {
                    tempArr.Add(item.ItemId);
                    alreadyExistServices = bill.Items.Where(x => item.ItemId == x.ItemId).FirstOrDefault();
                }
                else
                {
                    tempArr.Add(item.ProductId);
                    alreadyExistServices = bill.Items.Where(x => item.ProductId == x.ProductId).FirstOrDefault();
                }

                //tempArr.Add(item.ItemId);
                //var alreadyExistServices = bill.Items.Where(x => item.ItemId == x.ItemId).FirstOrDefault();
                if (alreadyExistServices != null)
                {
                    alreadyExistServices.Price = item.Price;
                    alreadyExistServices.ProductId = item.ProductId;
                  //  alreadyExistServices.TaxId = item.TaxId;
                  //  alreadyExistServices.TaxPercentage = item.TaxPercentage;
                    alreadyExistServices.Rate = item.Rate;
                    alreadyExistServices.Quantity = item.Quantity;
                  //  alreadyExistServices.TaxPrice = item.TaxPrice;
                    alreadyExistServices.LineAmount = item.LineAmount;
                    bill.Items.Add(alreadyExistServices);
                }
            }

            var deletedServices = new List<BillItem>();
            if (model.BillType == 0)
            {
                deletedServices = bill.Items.Where(x => !tempArr.Contains(x.ItemId)).ToList();
            }
            else
            {
                deletedServices = bill.Items.Where(x => !tempArr.Contains(x.ProductId)).ToList();
            }


           // var deletedServices = bill.Items.Where(x => !tempArr.Contains(x.ItemId)).ToList();
            //var alreadyExistServices = entity.Services.Where(x => tempArr.Contains(x.ServiceId)).ToList();
            //var resultAll = items.Where(i => filter.All(x => i.Features.Any(f => x == f.Id)));


            foreach (var deletedService in deletedServices)
            {
                bill.Items.Remove(deletedService);
            }

            var addedServices = new List<BillServiceModel>();
            if (model.BillType == 0)
            {
                addedServices = model.Items
               .Where(x => !bill.Items.Select(y => y.ItemId).Contains(x.ItemId))
               .ToList();
            }
            else
            {
                addedServices = model.Items
                .Where(x => !bill.Items.Select(y => y.ProductId).Contains(x.ProductId))
                .ToList();
            }

            //var addedServices = model.Items
            //    .Where(x => !bill.Items.Select(y => y.ItemId).Contains(x.ItemId))
            //    .ToList();

            foreach (var service in addedServices)
            {
                bill.Items.Add(new BillItem
                {
                    Id = Guid.NewGuid(),
                    ItemId = service.ItemId,
                    Rate = service.Rate,
                    BillId = model.Id,
                   // TaxId = service.TaxId,
                    Price = service.Price,
                    Quantity = service.Quantity,
                  //  TaxPercentage = service.TaxPercentage,
                 //   TaxPrice = service.TaxPrice,
                    LineAmount = service.LineAmount,
                    ProductId = service.ProductId,
                });
            }

            if (model.Attachments == null || !model.Attachments.Any())
            {
                return;
            }

            var deletedAttachemntFiles = bill.Attachments.Select(x => x.FileName)
                .Except(model.Attachments.Select(y => y.FileName)).ToList();

            foreach (var deletedAttachemntFile in deletedAttachemntFiles)
            {
                var attachemnt = bill.Attachments.Single(x => x.FileName.Equals(deletedAttachemntFile));
                bill.Attachments.Remove(attachemnt);
            }

            foreach (var attachment in model.Attachments)
            {
                var billAttachment = bill.Attachments.SingleOrDefault(x => x.FileName.Equals(attachment.FileName));

                if (billAttachment == null)
                {
                    billAttachment = new BillAttachment
                    {
                        Title = attachment.Title,
                        FileName = attachment.FileName,
                        OriginalFileName = attachment.OriginalFileName,
                        CreatedBy = userId ?? "0",
                        CreatedOn = Utility.GetDateTime()
                    };
                }
                else
                {
                    billAttachment.Title = attachment.Title;
                    billAttachment.FileName = attachment.FileName;
                    billAttachment.OriginalFileName = attachment.OriginalFileName;
                }

                bill.Attachments.Add(billAttachment);
            }
        }
    }
}
