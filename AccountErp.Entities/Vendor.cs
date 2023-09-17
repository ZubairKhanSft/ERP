using AccountErp.Utilities;
using System;
using System.Collections.Generic;

namespace AccountErp.Entities
{
    public class Vendor
    {
        public int Id { get; set; }
       /* public string HSTNumber { get; set; }
        public string Name { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public int? BillingAddressId { get; set; }
        public int? ShippingAddressId { get; set; }
        
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string Ifsc { get; set; }

        public decimal? Discount { get; set; }
*/
      

        /* public Address ShippingAddress { get; set; }
         public Address BillingAddress { get; set; }
         public ICollection<Contact> Contacts { get; set; }*/
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Constants.RecordStatus Status { get; set; }
    }
}
