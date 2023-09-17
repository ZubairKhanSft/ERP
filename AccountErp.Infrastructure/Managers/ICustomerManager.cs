using AccountErp.Dtos;
using AccountErp.Dtos.Customer;
using AccountErp.Models.Customer;
using AccountErp.Models.Payment;
using AccountErp.Models.ShippingAddress;
using AccountErp.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface ICustomerManager
    {
        Task<int> AddAsync(CustomerAddModel model);
        Task AddCustomerShippingAddress(ShippingAddressModel model);
        Task AddCustomerPayment(AddPaymentModel model);

        Task EditAsync(CustomerEditModel model);

        Task<CustomerDetailDto> GetDetailAsync(int id);

        Task<CustomerDetailDto> GetForEditAsync(int id);

        Task<JqDataTableResponse<CustomerListItemDto>> GetPagedResultAsync(CustomerJqDataTableRequestModel model);
        Task<(List<CustomerDetailDto>, int count)> GetPagedResultAsync(int PageSize, int Page,string filterKey);

        Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync();

        Task ToggleStatusAsync(int id);

        Task DeleteAsync(int id);

        Task<bool> IsEmailExistsAsync(string email);

        Task<bool> IsEmailExistsAsync(int id, string email);

        Task<CustomerPaymentInfoDto> GetPaymentInfoAsync(int id);

        Task<CustomerStatementDto> GetCustomerStatementAsync(CustomerStatementDto model);
        //vendor and customer
        Task<List<CustomerAndVendorDetailsDto>> GetDetailsCustomerAndVendorAsync();

        //Task SetOverdueStatus();

    }
}
