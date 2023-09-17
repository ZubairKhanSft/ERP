using AccountErp.Entities;
using AccountErp.Models.Customer;
using AccountErp.Models.Payment;
using AccountErp.Models.ShippingAddress;
using AccountErp.Utilities;
using System.Linq;
namespace AccountErp.Factories
{
    public class CustomerFactory
    {
        public static Customer Create(CustomerAddModel model, string userId)
        {
            var customer = new Customer
            {
                FirstName = model.FirstName,
               // MiddleName = model.MiddleName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                City = model.City,
                State = model.State,
                Zipcode = model.Zipcode,
                Country = model.Country,
                CreatedBy = userId ?? "0",
                CreatedOn = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active,
               
            };

            return customer;
        }

        public static ShippingAddress Create(ShippingAddressModel model)
        {
            var customer = new ShippingAddress
            {
                CustomerId = model.CustomerId,
              
                Address = model.Address,
                City = model.City,
                State = model.State,
                PostalCode = model.ZipCode,
              
                CreatedOn = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active,
                ShippingMethod = model.ShippingMethod,
            };

            return customer;
        }

        public static Payment Create(AddPaymentModel model)
        {
            var customer = new Payment
            {
                CustomerId = model.CustomerId,

                Name = model.Name,
                PaymentType = model.PaymentType,
                CardNumber = model.CardNumber,
                
                CreatedOn = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active,

            };

            return customer;
        }
        public static void Update(CustomerEditModel model, Customer entity, string userId)
        {
            entity.FirstName = model.FirstName;
            //  entity.MiddleName = model.MiddleName;
            entity.LastName = model.LastName;
            entity.Phone = model.Phone;
            entity.Email = model.Email;
            /* entity.AccountNumber = model.AccountNumber;
             entity.BankName = model.BankName;
             entity.BankBranch = model.BankBranch;
             entity.Ifsc = model.Ifsc;
             entity.Discount = model.Discount;*/
            entity.UpdatedBy = userId;
            entity.UpdatedOn = Utility.GetDateTime();
            entity.Address = model.Address1;
            entity.City = model.City;
            entity.State = model.State;
            entity.Zipcode = model.ZipCode;
            entity.Country = model.Country;

        }
            public static void UpdateShippingAddress(ShippingAddressModel model, ShippingAddress entity)
            {
               
               
                entity.Address = model.Address;
                entity.City = model.City;
                entity.State = model.State;
                entity.PostalCode = model.ZipCode;
                entity.ShippingMethod = model.ShippingMethod;
            }


                /*if (!model.Address.Id.HasValue && model.Address.IsAllNullOrEmpty())
                {
                    return;
                }*/

                /* if (entity.Address != null)
                 {
                     entity.Address.StreetNumber = model.Address.StreetNumber;
                     entity.Address.StreetName = model.Address.StreetName;
                     entity.Address.PostalCode = model.Address.PostalCode;
                     entity.Address.City = model.Address.City;
                     entity.Address.State = model.Address.State;
                     entity.Address.CountryId = model.Address.CountryId;
                     entity.Address.Phone = model.Address.Phone;
                 }*/

                /*if (entity.Address == null && model.Address != null)
                {
                    entity.Address = new Address
                    {
                        StreetNumber = model.Address.StreetNumber,
                        StreetName = model.Address.StreetName,
                        PostalCode = model.Address.PostalCode,
                        City = model.Address.City,
                        State = model.Address.State,
                        CountryId = model.Address.CountryId,
                        Phone = model.Address.Phone
                    };
                }*/

                /*  if (!model.ShippingAddress.Id.HasValue && model.ShippingAddress.IsAllNullOrEmpty())
                  {
                      return;
                  }*/

                /* if (entity.ShippingAddress != null)
                 {
                     entity.ShippingAddress.AddressLine1 = model.ShippingAddress.AddressLine1;
                     entity.ShippingAddress.AddressLine2 = model.ShippingAddress.AddressLine2;
                     entity.ShippingAddress.PostalCode = model.ShippingAddress.PostalCode;
                     entity.ShippingAddress.City = model.ShippingAddress.City;
                     entity.ShippingAddress.State = model.ShippingAddress.State;
                     entity.ShippingAddress.CountryId = model.ShippingAddress.CountryId;
                     entity.ShippingAddress.ShipTo = model.ShippingAddress.ShipTo;
                     entity.ShippingAddress.DeliveryInstruction = model.ShippingAddress.DeliveryInstruction;
                     entity.ShippingAddress.Phone = model.ShippingAddress.Phone;
                 }
     */
                /*if (entity.ShippingAddress == null && model.ShippingAddress != null)
                {
                    entity.ShippingAddress = new ShippingAddress
                    {
                        AddressLine1 = model.ShippingAddress.AddressLine1,
                        AddressLine2 = model.ShippingAddress.AddressLine2,
                        PostalCode = model.ShippingAddress.PostalCode,
                        City = model.ShippingAddress.City,
                        State = model.ShippingAddress.State,
                        CountryId = model.ShippingAddress.CountryId,
                        ShipTo = model.ShippingAddress.ShipTo,
                        DeliveryInstruction = model.ShippingAddress.DeliveryInstruction,
                        Phone = model.ShippingAddress.Phone
                    };
                }
    */
            
        }
}
