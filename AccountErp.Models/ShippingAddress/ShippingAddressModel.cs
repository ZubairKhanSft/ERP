using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.ShippingAddress
{
    public class ShippingAddressModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ShippingMethod { get; set; }

    }
}
