using AccountErp.Entities;
using AccountErp.Models.CreditMemo;
using AccountErp.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace AccountErp.Factories
{
    public class CreditMemoFactory
    {

        public static CreditMemo Create(CreditMemoAddModel model, string userId, int count)
        {
            var creditmemo = new CreditMemo
            {
                CustomerId = model.CustomerId,
             //   InvoiceNumber = "INV" + "-" + model.InvoiceDate.ToString("yy") + "-" + (count + 1).ToString("000"),
                CreditMemoNumber = "CM" + "-" + model.InvoiceDate.ToString("yy") + "-" + (count + 1).ToString("000"),
                Tax = model.Tax,
                Discount = model.Discount,
                TotalAmount = model.TotalAmount,
                OldAmmount=model.OldAmmount,
                NewAmmount=model.NewAmmount,
                DiffAmmount=model.DiffAmmount,
                Remark = model.Remark,
                Status = Constants.InvoiceStatus.Pending,
                CreatedBy = userId ?? "0",
                CreatedOn = Utility.GetDateTime(),
                InvoiceDate = model.InvoiceDate,
                StrInvoiceDate = model.InvoiceDate.ToString("yyyy-MM-dd"),
                DueDate = model.DueDate,
                StrDueDate = model.DueDate.ToString("yyyy-MM-dd"),
                PoSoNumber = model.PoSoNumber,
                SubTotal = model.SubTotal,
                LineAmountSubTotal = model.LineAmountSubTotal,
                InvoiceNumber=model.InvoiceNumber,
                InvoiceId = model.InvoiceId,
                //   InvoiceType = model.InvoiceType,
                CreditMemoService = model.CreditMemoService.Select(x => new CreditMemoService
                {
                    Id = Guid.NewGuid(),
                    ServiceId = x.ServiceId,
                    ProductId = x.ProductId,
                    Rate = x.Rate,
                    OldQuantity = x.OldQuantity,
                    NewQuantity = x.NewQuantity,
                    Price = x.Price,
                    TaxId = x.TaxId,
                    TaxPrice = x.TaxPrice,
                    TaxPercentage = x.TaxPercentage,
                    LineAmount = x.LineAmount,
                    OldAmmount=x.OldAmmount,
                    NewAmmount=x.NewAmmount,
                    DiffAmmount=x.DiffAmmount,
                    TaxDiffAmmount=x.TaxDiffAmmount
                }).ToList()
            };


            return creditmemo;
        }

        public static void Edit(CreditMemoEditModel model, CreditMemo entity, string userId)
        {


            entity.CustomerId = model.CustomerId;
           // entity.InvoiceNumber = "INV" + "-" + model.InvoiceDate.ToString("yy") + "-" + (count + 1).ToString("000");
           //entity.CreditMemoNumber = "CM" + "-" + model.InvoiceDate.ToString("yy") + "-" + (count + 1).ToString("000");
            entity.Tax = model.Tax;
            entity.Discount = model.Discount;
            entity.TotalAmount = model.TotalAmount;
            entity.OldAmmount = model.OldAmmount;
            entity.NewAmmount = model.NewAmmount;
            entity.DiffAmmount = model.DiffAmmount;
            entity.Remark = model.Remark;
            entity.Status = Constants.InvoiceStatus.Pending;
            entity.CreatedBy = userId ?? "0";
            entity.CreatedOn = Utility.GetDateTime();
            entity.InvoiceDate = model.InvoiceDate;
            entity.StrInvoiceDate = model.InvoiceDate.ToString("yyyy-MM-dd");
            entity.DueDate = model.DueDate;
            entity.StrDueDate = model.DueDate.ToString("yyyy-MM-dd");
            entity.PoSoNumber = model.PoSoNumber;
            entity.SubTotal = model.SubTotal;
            entity.LineAmountSubTotal = model.LineAmountSubTotal;
            entity.InvoiceId = model.InvoiceId;
            //   InvoiceType = model.InvoiceType,
            ArrayList tempArr = new ArrayList();

            foreach (var item in model.CreditMemoService)
            {
                var alreadyExistServices = new CreditMemoService();

                tempArr.Add(item.ProductId);
                alreadyExistServices = entity.CreditMemoService.Where(x => item.ProductId == x.ProductId).FirstOrDefault();
                if (alreadyExistServices != null)
                {
                    alreadyExistServices.Price = item.Price;
                    alreadyExistServices.TaxId = item.TaxId;
                    alreadyExistServices.TaxPercentage = item.TaxPercentage;
                    alreadyExistServices.Rate = item.Rate;
                    alreadyExistServices.OldQuantity = item.OldQuantity;
                    alreadyExistServices.NewQuantity = item.NewQuantity;
                    alreadyExistServices.TaxPrice = item.TaxPrice;
                    alreadyExistServices.LineAmount = item.LineAmount;
                    entity.CreditMemoService.Add(alreadyExistServices);
                }
            }

           










        }
    }
}