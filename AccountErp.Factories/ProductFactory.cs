using AccountErp.Entities;
using AccountErp.Models.Product;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
    public class ProductFactory
    {
        public static Product Create(ProductAddModel model, string userId)
        {
            var prod = new Product
            {
                /* Name = model.Name,
                 SellingPrice = model.SellingPrice,
                 BuyingPrice = model.BuyingPrice,
                 InitialStock = model.InitialStock,
                 Description = model.Description,
                 IsTaxable = model.IsTaxable?.Equals("1") ?? false,
                 SalesTaxId = model.SalesTaxId,*/
                ProductCategoryId = model.ProductCategoryId,
                Model = model.Model,
                Specification = model.Specification,
                Brands = model.Brands,
                Units = model.Units,
                RateAED = model.RateAED,
                RateUSD = model.RateUSD,
                PartNumber = model.PartNumber,
                UAN = model.UAN,
                SupplierCode = model.SupplierCode,
                Status = Constants.RecordStatus.Active,
                CreatedBy = userId ?? "0",
                CreatedOn = Utility.GetDateTime(),
                /* BankAccountId = model.BankAccountId,
                 WareHouseId = model.WarehouseId*/
             //   FileName = model.FileUrl,
             AttachmentName = model.FileUrl,
            };
            return prod;
        }

        public static void Create(ProductEditModel model, Product entity, string userId)
        {
            entity.ProductCategoryId = model.ProductCategoryId;
            entity.Model = model.Model != null ? model.Model : entity.Model;
            entity.Specification = model.Specification != null ? model.Specification : entity.Specification;
            entity.Brands = model.Brands != null ? model.Brands : entity.Brands;
            entity.Units = model.Units != 0 ? model.Units : entity.Units;
            entity.RateUSD = model.RateUSD != 0 ? model.RateUSD : entity.RateUSD;
            entity.RateAED = model.RateAED != 0 ? model.RateAED : entity.RateAED;
            entity.PartNumber = model.PartNumber != 0 ? model.PartNumber : entity.PartNumber;
            entity.UAN = model.UAN != null ? model.UAN : entity.UAN;
            entity.SupplierCode = model.SupplierCode != 0 ? model.SupplierCode : entity.SupplierCode;


            /* entity.Name = model.Name;
             entity.Description = model.Description;
             entity.IsTaxable = model.IsTaxable?.Equals("1") ?? false;
             entity.SalesTaxId = entity.IsTaxable ? model.SalesTaxId : null;
             entity.SellingPrice = model.SellingPrice;
             entity.BuyingPrice = model.BuyingPrice;
             entity.InitialStock = model.InitialStock;
             entity.BankAccountId = model.BankAccountId;
             entity.WareHouseId = model.WarehouseId;*/

            entity.UpdatedBy = userId ?? "0";
            entity.UpdatedOn = Utility.GetDateTime();
            entity.AttachmentName = model.FileUrl != null ? model.FileUrl : entity.AttachmentName;

        }
    }
    
}
