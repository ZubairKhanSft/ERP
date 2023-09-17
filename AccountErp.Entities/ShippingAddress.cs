using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AccountErp.Entities
{
   public class ShippingAddress
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string ShippingMethod { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
       
    }
}
