using AccountErp.Dtos;
using AccountErp.Dtos.Customer;
using AccountErp.Entities;
using AccountErp.Factories;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.Customer;
using AccountErp.Models.Payment;
using AccountErp.Models.ShippingAddress;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountErp.Managers
{
    public class CustomerManager : ICustomerManager
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly string _userId;

        public CustomerManager(IHttpContextAccessor contextAccessor,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _userId = contextAccessor.HttpContext.User.GetUserId();
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddAsync(CustomerAddModel model)
        {
            var customer = CustomerFactory.Create(model, _userId);
            await _customerRepository.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();
            return customer.Id;
        }

        public async Task AddCustomerShippingAddress(ShippingAddressModel model)
        {
            await _customerRepository.AddCustomerShippingAddress(CustomerFactory.Create(model));
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task AddCustomerPayment(AddPaymentModel model)
        {
            await _customerRepository.AddCustomerPayment(CustomerFactory.Create(model));
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task EditAsync(CustomerEditModel model)
        {
            var customer = await _customerRepository.GetAsync(model.Id);
            CustomerFactory.Update(model, customer, _userId);
            _customerRepository.Edit(customer);
            await _unitOfWork.SaveChangesAsync();
           
            if(model.ShippingAddress != null)
            {
                var shipping = await _customerRepository.GetShippingAddress(model.Id);
                CustomerFactory.UpdateShippingAddress(model.ShippingAddress,shipping);
                _customerRepository.EditShippingAddress(shipping);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<CustomerDetailDto> GetDetailAsync(int id)
        {
            return await _customerRepository.GetDetailAsync(id);
        }
        public async Task<CustomerDetailDto> GetForEditAsync(int id)
        {
            return await _customerRepository.GetForEditAsync(id);
        }

        public async Task<JqDataTableResponse<CustomerListItemDto>> GetPagedResultAsync(CustomerJqDataTableRequestModel model)
        {
            return await _customerRepository.GetPagedResultAsync(model);
        }

        public async Task<(List<CustomerDetailDto>, int count)> GetPagedResultAsync(int PageSize, int Page, string filterKey)
        {
            return await _customerRepository.GetPagedResultAsync(PageSize,Page,filterKey);
        }

        public async Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync()
        {
            return await _customerRepository.GetSelectItemsAsync();
        }

        public async Task ToggleStatusAsync(int id)
        {
            await _customerRepository.ToggleStatusAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _customerRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _customerRepository.IsEmailExistsAsync(email);
        }

        public async Task<bool> IsEmailExistsAsync(int id, string email)
        {
            return await _customerRepository.IsEmailExistsAsync(id, email);
        }
        
        public async Task<CustomerPaymentInfoDto> GetPaymentInfoAsync(int id)
        {
            return await _customerRepository.GetPaymentInfoAsync(id);
        }

        public async Task<CustomerStatementDto> GetCustomerStatementAsync(CustomerStatementDto model)
        {
            if(model.Status == 1)
            {
                var data = await _customerRepository.GetOpeningBalance(model.startDate,model.CustomerId);
                model.openingBalance = data.Sum(x => x.Amount);
                var customerData = await _customerRepository.GetCustomerStatementAsync(model);
                var creditMemo = await _customerRepository.GetCreditMemo(model);
                customerData.InvoiceList = customerData.InvoiceList.Where(p => (p.InvoiceDate >= model.startDate && p.InvoiceDate <= model.endDate) && p.Status != Constants.InvoiceStatus.Deleted).ToList();
                customerData.InvoiceNewList = new List<Dtos.Invoice.InvoiceListItemDto>();
                foreach (var item in customerData.InvoiceList)
                {
                    customerData.InvoiceNewList.Add(item);
                }
                foreach (var item in creditMemo)
                {
                    customerData.InvoiceNewList.Add(item);
                }

                customerData.InvoiceNewList = customerData.InvoiceNewList.OrderBy(x => x.Id).ToList();
                return customerData;

            }
            else
            {
                var customerData = await _customerRepository.GetCustomerStatementAsync(model);
                var creditMemo = await _customerRepository.GetCreditMemo(model);
                customerData.InvoiceNewList = new List<Dtos.Invoice.InvoiceListItemDto>();
                foreach (var item in customerData.InvoiceList)
                {
                    customerData.InvoiceNewList.Add(item);
                }
                foreach (var item in creditMemo)
                {
                    customerData.InvoiceNewList.Add(item);
                }

                customerData.InvoiceNewList = customerData.InvoiceNewList.OrderBy(x => x.Id).ToList();
                return customerData;
            }
           
        }
        public async Task<List<CustomerAndVendorDetailsDto>> GetDetailsCustomerAndVendorAsync()
        {
            var customer = await _customerRepository.GetCustomerAsync();
            List<CustomerAndVendorDetailsDto> customerAndVendorDetailsDtos = new List<CustomerAndVendorDetailsDto>();
            CustomerAndVendorDetailsDto customerAndVendorDetailsObj = new CustomerAndVendorDetailsDto();
            customerAndVendorDetailsObj.id = "0";
            customerAndVendorDetailsObj.text = "Customer";
            customerAndVendorDetailsObj.children = new List<CustomerAndVendorDto>();

            foreach (var item in customer.CustomerTypes)
            {
                CustomerAndVendorDto customerAndVendorDtoObj = new CustomerAndVendorDto();
                customerAndVendorDtoObj.id = item.Id.ToString() + "/1";
                customerAndVendorDtoObj.text = item.FirstName;
                customerAndVendorDetailsObj.children.Add(customerAndVendorDtoObj);
            }

            customerAndVendorDetailsDtos.Add(customerAndVendorDetailsObj);
            customerAndVendorDetailsObj = new CustomerAndVendorDetailsDto();
            customerAndVendorDetailsObj.id = "0";
            customerAndVendorDetailsObj.text = "Vendor";
            customerAndVendorDetailsObj.children = new List<CustomerAndVendorDto>();

            foreach (var item in customer.VendorTypes)
            {
                CustomerAndVendorDto customerAndVendorDtoObj = new CustomerAndVendorDto();
                customerAndVendorDtoObj.id = item.Id.ToString() + "/2";
                customerAndVendorDtoObj.text = item.Name;
                customerAndVendorDetailsObj.children.Add(customerAndVendorDtoObj);
            }
            customerAndVendorDetailsDtos.Add(customerAndVendorDetailsObj);
            return customerAndVendorDetailsDtos;
        }

        //public async Task SetOverdueStatus()
        //{
        //    await _customerRepository.SetOverdueStatus();
        //    await _unitOfWork.SaveChangesAsync();
        //}

    }
}
