using AccountErp.Dtos.Address;
using AccountErp.Dtos.Payment;
using AccountErp.Dtos.ShippingAddress;
using AccountErp.Utilities;

namespace AccountErp.Dtos.Customer
{
    public class CustomerDetailDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
     
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Constants.RecordStatus Status { get; set; }

        /* public AddressDto Address { get; set; }
         public int? AddressId { get; set; }*/
        public ShippingAddressDto ShippingAddress { get; set; }
        public PaymentDetailDto PaymentCustomer { get; set; }
        // public int? ShippingAddressId { get; set; }


        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
    }
}
