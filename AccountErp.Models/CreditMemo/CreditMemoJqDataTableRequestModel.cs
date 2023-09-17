using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.CreditMemo
{
  public  class CreditMemoJqDataTableRequestModel : JqDataTableRequest
    {
        public string FilterKey { get; set; }
        public int? CustomerId { get; set; }
    }
}
