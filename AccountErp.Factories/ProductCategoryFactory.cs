using AccountErp.Entities;
using AccountErp.Models.ProductCategory;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
    public class ProductCategoryFactory
    {
        public static ProductCategory Create(ProductCategoryAddModel model, string userId)
        {
            var item = new ProductCategory
            {
                Name = model.Name,
                Status = Constants.RecordStatus.Active,
                CreatedBy = userId ?? "0",
                CreatedOn = Utility.GetDateTime()
            };
            return item;
        }
        public static void Create(ProductCategoryEditModel model, ProductCategory entity, string userId)
        {
            entity.Name = model.Name;
            entity.UpdatedBy = userId ?? "0";
            entity.UpdatedOn = Utility.GetDateTime();
        }
    }

}