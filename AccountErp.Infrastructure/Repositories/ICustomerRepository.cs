﻿using AccountErp.Dtos;
using AccountErp.Dtos.Customer;
using AccountErp.Dtos.Invoice;
using AccountErp.Dtos.Vendor;
using AccountErp.Entities;
using AccountErp.Models.Customer;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer model);
        Task AddCustomerShippingAddress(ShippingAddress entity);
        Task AddCustomerPayment(Payment entity);

        void Edit(Customer entity);
        void EditShippingAddress(ShippingAddress entity);

        Task<Customer> GetAsync(int id);
        Task<ShippingAddress> GetShippingAddress(int id);

        Task<CustomerDetailDto> GetDetailAsync(int id);

        Task<CustomerDetailDto> GetForEditAsync(int id);

        Task<JqDataTableResponse<CustomerListItemDto>> GetPagedResultAsync(CustomerJqDataTableRequestModel model);
        Task<(List<CustomerDetailDto>, int count)> GetPagedResultAsync(int PageSize,int Page, string filterKey);

        Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync();

        Task ToggleStatusAsync(int id);

        Task DeleteAsync(int id);

        Task<bool> IsEmailExistsAsync(string email);

        Task<bool> IsEmailExistsAsync(int id, string email);
        
        Task<CustomerPaymentInfoDto> GetPaymentInfoAsync(int id);
        Task<CustomerStatementDto> GetCustomerStatementAsync(CustomerStatementDto model);
        Task<List<InvoiceListItemDto>> GetCreditMemo(CustomerStatementDto model);
        Task<List<InvoiceListItemDto>> GetOpeningBalance(DateTime date, int custId);
        Task<CustomerAndVendorMainDetailsDto> GetCustomerAsync();
        //Task SetOverdueStatus();

    }
}
