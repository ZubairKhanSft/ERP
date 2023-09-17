using System.ComponentModel.DataAnnotations;

namespace AccountErp.Models.Vendor
{
    public class VendorAddModel
    {
       


        public string ShopName { get; set; }
        public string ShopAddress { get; set; }

        
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
