using AccountErp.Utilities;
using System;

namespace AccountErp.Dtos.Vendor
{
    public class VendorListItemDto
    {
        public int Id { get; set; }
       
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
