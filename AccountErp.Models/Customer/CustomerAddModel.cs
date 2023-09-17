using AccountErp.Models.Payment;
using AccountErp.Models.ShippingAddress;
using System.ComponentModel.DataAnnotations;

namespace AccountErp.Models.Customer
{
    public class CustomerAddModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
       
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }

     
      
       
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }

        public ShippingAddressModel ShippingAddress { get; set; }
        public AddPaymentModel CustomerPayment { get; set; }
    }
}
