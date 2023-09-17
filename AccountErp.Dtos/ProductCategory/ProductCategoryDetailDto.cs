using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.ProductCategory
{
    public class ProductCategoryDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
