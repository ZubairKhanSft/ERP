using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.ProductCategory
{
    public class ProductCategoryJqDataTableRequestModel : JqDataTableRequest
    {
        public string FilterKey { get; set; }
    }
}