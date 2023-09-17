using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.Product
{
   public class ProductInventoryJqDataTableRequestModel : JqDataTableRequest
    {
        public string FilterKey { get; set; }

        public DateTime? StartDate {get; set; }

       public DateTime? EndDate {get; set; }


}
}
