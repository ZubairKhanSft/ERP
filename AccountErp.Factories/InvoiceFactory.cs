using AccountErp.Entities;
using AccountErp.Models.Invoice;
using AccountErp.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AccountErp.Factories
{
    public class InvoiceFactory
    {
        public static Invoice Create(InvoiceAddModel model, string userId,int count)
        {
            var invoice = new Invoice
            {
                CustomerId = model.CustomerId,
                InvoiceNumber = "INV" + "-" + model.InvoiceDate.ToString("yy") + "-" + (count + 1).ToString("000"),
                Tax = model.Tax,
                Discount = model.Discount,
                TotalAmount = model.TotalAmount,
                Remark = model.Remark,
                Status = Constants.InvoiceStatus.Pending,
                CreatedBy = model.UserId.ToString(),
                CreatedOn = Utility.GetDateTime(),
                InvoiceDate = model.InvoiceDate,
                StrInvoiceDate = model.InvoiceDate.ToString("yyyy-MM-dd"),
                DueDate = model.DueDate,
                StrDueDate = model.DueDate.ToString("yyyy-MM-dd"),
                PoSoNumber = model.PoSoNumber,
                SubTotal = model.SubTotal,
                LineAmountSubTotal = model.LineAmountSubTotal,
                InvoiceType = model.InvoiceType,
               
               // CurrencyName = model.CurrencyName,
                AppovedId = 0,
                IsApproved = false,
                VatAmount = model.VatAmount,
                SalesOrderNumber = model.SalesOrderNumber,
               
                CurrencyId = model.CurrencyId,
                ConversionRate = model.ConversionRate,
                TotalAmountAfterConversion = model.TotalAmountAfterConversion,
                /*Services = model.Items.Select(x => new InvoiceService
                {
                    Id = Guid.NewGuid(),
                    
                    ServiceId = x.ServiceId,
                    ProductId = x.ProductId,
                    Rate = x.Rate,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    TaxId = x.TaxId,
                    TaxPrice = x.TaxPrice,
                    TaxPercentage = x.TaxPercentage,
                    LineAmount = x.LineAmount
                }).ToList()*/
            };


            if (model.Attachments == null || !model.Attachments.Any())
            {
                return invoice;
            }

            /*invoice.Attachments = model.Attachments.Select(x => new InvoiceAttachment
            {
                Title = x.Title,
                FileName = x.FileName,
                OriginalFileName = x.OriginalFileName,
                CreatedBy = userId ?? "0",
                CreatedOn = Utility.GetDateTime()
            }).ToList();*/

            return invoice;
        }
        //public static Invoice Create(InvoiceAddModel model, string userId, List<Item> items)
        //{
        //    var invoice = new Invoice
        //    {
        //        CustomerId = model.CustomerId,
        //        InvoiceNumber = string.Empty,
        //        Tax = model.Tax,
        //        Discount = model.Discount,
        //        TotalAmount = model.TotalAmount,
        //        Remark = model.Remark,
        //        Status = Constants.InvoiceStatus.Pending,
        //        CreatedBy = userId,
        //        CreatedOn = Utility.GetDateTime(),
        //        Services = items.Select(x => new InvoiceService
        //        {
        //            Id = Guid.NewGuid(),
        //            ServiceId = x.Id,
        //            Rate = x.Rate
        //        }).ToList()
        //    };


        //    if (model.Attachments == null || !model.Attachments.Any())
        //    {
        //        return invoice;
        //    }

        //    foreach (var attachment in model.Attachments)
        //    {
        //        invoice.Attachments = new List<InvoiceAttachment>
        //        {
        //            new InvoiceAttachment
        //            {
        //                Title = attachment.Title,
        //                FileName = attachment.FileName,
        //                OriginalFileName = attachment.OriginalFileName,
        //                CreatedBy =userId,
        //                CreatedOn =Utility.GetDateTime()
        //            }
        //        };
        //    }

        //    return invoice;
        //}

        //public static void Create(InvoiceEditModel model, Invoice entity, string userId, List<Item> items)
        //{
        //    entity.CustomerId = model.CustomerId;
        //    entity.Tax = model.Tax;
        //    entity.Discount = model.Discount;
        //    entity.TotalAmount = model.TotalAmount;
        //    entity.Remark = model.Remark;
        //    entity.UpdatedBy = userId;
        //    entity.UpdatedOn = Utility.GetDateTime();

        //    var deletedServices = entity.Services.Where(x => !model.Items.Contains(x.ServiceId)).ToList();

        //    foreach (var deletedService in deletedServices)
        //    {
        //        entity.Services.Remove(deletedService);
        //    }

        //    var addedServices = items
        //        .Where(x => !entity.Services.Select(y => y.ServiceId).Contains(x.Id))
        //        .ToList();

        //    foreach (var service in addedServices)
        //    {
        //        entity.Services.Add(new InvoiceService
        //        {
        //            Id = Guid.NewGuid(),
        //            ServiceId = service.Id,
        //            Rate = service.Rate
        //        });
        //    }

        //    if (model.Attachments == null || !model.Attachments.Any())
        //    {
        //        return;
        //    }

        //    var deletedAttachemntFiles = entity.Attachments.Select(x => x.FileName)
        //        .Except(model.Attachments.Select(y => y.FileName)).ToList();

        //    foreach (var deletedAttachemntFile in deletedAttachemntFiles)
        //    {
        //        var attachemnt = entity.Attachments.Single(x => x.FileName.Equals(deletedAttachemntFile));
        //        entity.Attachments.Remove(attachemnt);
        //    }

        //    foreach (var attachment in model.Attachments)
        //    {
        //        var invoiceAttachment = entity.Attachments.SingleOrDefault(x => x.FileName.Equals(attachment.FileName));

        //        if (invoiceAttachment == null)
        //        {
        //            invoiceAttachment = new InvoiceAttachment
        //            {
        //                Title = attachment.Title,
        //                FileName = attachment.FileName,
        //                OriginalFileName = attachment.OriginalFileName,
        //                CreatedBy = userId,
        //                CreatedOn = Utility.GetDateTime()
        //            };
        //        }
        //        else
        //        {
        //            invoiceAttachment.Title = attachment.Title;
        //            invoiceAttachment.FileName = attachment.FileName;
        //            invoiceAttachment.OriginalFileName = attachment.OriginalFileName;
        //        }

        //        entity.Attachments.Add(invoiceAttachment);
        //    }
        //}

        public static InvoiceService CreateInvoiceService(InvoiceServiceModel model,int invoiceId)
        {
            var invoice = new InvoiceService
            {
               // Id = Guid.NewGuid(),
                InvoiceId = invoiceId,
                ServiceId = model.ServiceId,
                ProductId = model.ProductId,
                Rate = model.Rate,
                Quantity = model.Quantity,
                Price = model.Price,
                TaxId = model.TaxId,
                TaxPrice = model.TaxPrice,
                TaxPercentage = model.TaxPercentage,
                LineAmount = model.LineAmount
            };
            return invoice;
        }

        public static List<InvoiceService> CreateMultipleInvoiceService(List<InvoiceServiceModel> model,int invoiceId)
        {
            List<InvoiceService> list = new List<InvoiceService>();
            foreach (var item in model)
            {
                InvoiceService add = new InvoiceService();

                // add.Id = Guid.NewGuid();
                add.InvoiceId = invoiceId;
                add.ServiceId = item.ServiceId;
                add.ProductId = item.ProductId;
                add.Rate = item.Rate;
                add.Quantity = item.Quantity;
                add.Price = item.Price;
                add.TaxId = item.TaxId;
                add.TaxPrice = item.TaxPrice;
                add.TaxPercentage = item.TaxPercentage;
                add.LineAmount = item.LineAmount;
                
                list.Add(add);
            }
            return list;
        }

        public static List<InvoiceAttachment> CreateInvoiceAttachment(List<InvoiceAttachmentAddModel> model, int userId, int invoiceId)
        {
            InvoiceAttachment add = new InvoiceAttachment();
            List<InvoiceAttachment> attachments = new List<InvoiceAttachment>();
            foreach (var item in model)
            {
                add.InvoiceId = invoiceId;
                add.CreatedBy = userId.ToString();
                add.CreatedOn = Utility.GetDateTime();
                add.FileName = item.FileName;
                add.OriginalFileName = item.OriginalFileName ?? "";
                add.Title = item.Title;
                attachments.Add(add);
            }
            return attachments;
        }
        public static void EditInvoice(InvoiceEditModel model, Invoice entity, string userId)
        {
            entity.Tax = model.Tax != null ? model.Tax : entity.Tax;
            entity.CustomerId = model.CustomerId;
            entity.Discount = model.Discount != null ? model.Discount : entity.Discount;
            entity.TotalAmount = model.TotalAmount != 0 ? model.TotalAmount : entity.TotalAmount;
            entity.Remark = model.Remark != null ? model.Remark : entity.Remark;
            entity.InvoiceDate = model.InvoiceDate != null ? model.InvoiceDate : entity.InvoiceDate;
            entity.StrInvoiceDate = model.InvoiceDate.ToString("yyyy-MM-dd") != null ? model.InvoiceDate.ToString("yyyy-MM-dd") : entity.StrInvoiceDate;
            entity.DueDate = model.DueDate != null ? model.DueDate : entity.DueDate;
            entity.StrDueDate = model.DueDate.ToString("yyyy-MM-dd") != null ? model.DueDate.ToString("yyyy-MM-dd") : entity.StrDueDate;
            entity.PoSoNumber = model.PoSoNumber != null ? model.PoSoNumber : entity.PoSoNumber;
            entity.SubTotal = model.SubTotal != null ? model.SubTotal : entity.SubTotal;
            entity.LineAmountSubTotal = model.LineAmountSubTotal != null ? model.LineAmountSubTotal : entity.LineAmountSubTotal;
            entity.InvoiceType = model.InvoiceType != Constants.InvoiceType.none  ? model.InvoiceType : entity.InvoiceType;

            entity.VatAmount = model.VatAmount != 0 ? model.VatAmount : entity.VatAmount;
            entity.SalesOrderNumber = model.SalesOrderNumber != null ? model.SalesOrderNumber : entity.SalesOrderNumber;

            entity.UpdatedBy = userId ?? "0";
            entity.UpdatedOn = Utility.GetDateTime();
           
            entity.CurrencyId = model.CurrencyId !=0 ? model.CurrencyId : entity.CurrencyId; 
            entity.ConversionRate = model.ConversionRate !=0 ? model.ConversionRate : entity.ConversionRate;
            entity.TotalAmountAfterConversion = model.TotalAmountAfterConversion != 0 ? model.TotalAmountAfterConversion : entity.TotalAmountAfterConversion;






            //int[] arr = new int[100];
            ArrayList tempArr = new ArrayList();

            //for (int i=0;i<model.Items.Count; i++)
            //{
            //    arr[i] = model.Items[i].ServiceId;
            //}

           /* foreach (var item in model.Items)
            {
                var alreadyExistServices = new InvoiceService ();
                if (model.InvoiceType == 0)
                {
                    tempArr.Add(item.ServiceId);
                     alreadyExistServices = entity.Services.Where(x => item.ServiceId == x.ServiceId).FirstOrDefault();
                    //entity.Services.Where(x => item.ServiceId == x.ServiceId).Select(c => { c.CreditLimit = 1000; return c; });
                }
                else
                {
                    tempArr.Add(item.ProductId);
                    alreadyExistServices = entity.Services.Where(x => item.ProductId == x.ProductId).FirstOrDefault();
                }

                if (alreadyExistServices != null)
                {
                    alreadyExistServices.Price = item.Price;
                    alreadyExistServices.TaxId = item.TaxId;
                    alreadyExistServices.TaxPercentage = item.TaxPercentage;
                    alreadyExistServices.Rate = item.Rate;
                    alreadyExistServices.Quantity = item.Quantity;
                    alreadyExistServices.TaxPrice = item.TaxPrice;
                    alreadyExistServices.LineAmount = item.LineAmount;
                    entity.Services.Add(alreadyExistServices);
                }
            }
*/
            /*var deletedServices = new List<InvoiceService>();
            if (model.InvoiceType == 0)
            {
                 deletedServices = entity.Services.Where(x => !tempArr.Contains(x.ServiceId)).ToList();
            }
            else
            {
                 deletedServices = entity.Services.Where(x => !tempArr.Contains(x.ProductId)).ToList();
            }
*/
           
            //var alreadyExistServices = entity.Services.Where(x => tempArr.Contains(x.ServiceId)).ToList();
            //var resultAll = items.Where(i => filter.All(x => i.Features.Any(f => x == f.Id)));


           /* foreach (var deletedService in deletedServices)
            {
                entity.Services.Remove(deletedService);
            }*/

            /*var addedServices = new List<InvoiceServiceModel>();
            if (model.InvoiceType == 0)
            {
                 addedServices = model.Items
                .Where(x => !entity.Services.Select(y => y.ServiceId).Contains(x.ServiceId))
                .ToList();
            }
            else
            {
                 addedServices = model.Items
                 .Where(x => !entity.Services.Select(y => y.ProductId).Contains(x.ProductId))
                 .ToList();
            }*/

            //var addedServices = model.Items
            //    .Where(x => !entity.Services.Select(y => y.ServiceId).Contains(x.ServiceId))
            //    .ToList();

           /* foreach (var service in addedServices)
            {
                entity.Services.Add(new InvoiceService
                {
                    Id = Guid.NewGuid(),
                    ServiceId = service.ServiceId,
                    ProductId = service.ProductId,
                    Rate = service.Rate,
                    TaxId = service.TaxId,
                    Price = service.Price,
                    TaxPrice = service.TaxPrice,
                    Quantity = service.Quantity,
                    TaxPercentage = service.TaxPercentage,
                    LineAmount = service.LineAmount
                });
            }
*/
           /* if (model.Attachments == null || !model.Attachments.Any())
            {
                return;
            }

            var deletedAttachemntFiles = entity.Attachments.Select(x => x.FileName)
                .Except(model.Attachments.Select(y => y.FileName)).ToList();

            foreach (var deletedAttachemntFile in deletedAttachemntFiles)
            {
                var attachemnt = entity.Attachments.SingleOrDefault(x => x.FileName.Equals(deletedAttachemntFile));
                entity.Attachments.Remove(attachemnt);
            }*/

           /* foreach (var attachment in model.Attachments)
            {
                var invoiceAttachment = entity.Attachments.SingleOrDefault(x => x.FileName.Equals(attachment.FileName));

                if (invoiceAttachment == null)
                {
                    invoiceAttachment = new InvoiceAttachment
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
                    invoiceAttachment.Title = attachment.Title;
                    invoiceAttachment.FileName = attachment.FileName;
                    invoiceAttachment.OriginalFileName = attachment.OriginalFileName;
                }

                entity.Attachments.Add(invoiceAttachment);
            }
       */ }

    }

}
