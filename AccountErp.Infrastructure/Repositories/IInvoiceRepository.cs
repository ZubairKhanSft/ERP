using AccountErp.Dtos;
using AccountErp.Dtos.Invoice;
using AccountErp.Entities;
using AccountErp.Models.Invoice;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
    public interface IInvoiceRepository
    {
        Task AddAsync(Invoice entity);

        void Edit(Invoice entity);

        Task<Invoice> GetAsync(int id);

        Task<InvoiceDetailDto> GetDetailAsync(int id);

        Task<InvoiceDetailDto> GetDetailAsyncforpyment(int id);

        Task<ReportCountDto> CountReportAsync();
        Task<InvoiceDetailForEditDto> GetForEditAsync(int id);

        Task<JqDataTableResponse<InvoiceListItemDto>> GetPagedResultAsync(InvoiceJqDataTableRequestModel model);
        Task<(List<InvoiceDetailDto> data, int count)> GetAllAsync(int PageSize,int Page, string filterKey);
        Task<(List<InvoiceDetailDto> data, int count)> InvoiceReportAsync(int CustomerId,DateTime From, DateTime To);
        Task<(List<InvoiceDetailDto> data, int count)> InvoiceReportBySalesRepresentative(int userId,DateTime From, DateTime To);

        Task<JqDataTableResponse<InvoiceListItemDto>> GetTopFiveInvoicesAsync(InvoiceJqDataTableRequestModel model);

        Task<List<InvoiceDetailDto>> TopSalesAsync();

        Task<List<InvoiceListItemDto>> GetRecentAsync();

        Task<InvoiceSummaryDto> GetSummaryAsunc(int id);

        Task UpdateStatusAsync(int id, Constants.InvoiceStatus status);

        Task DeleteAsync(int id);

        Task<int> getCount();
        Task<List<InvoiceListItemDto>> GetAllUnpaidInvoiceAsync();
        Task<List<InvoiceListTopTenDto>> GetTopTenInvoicesAsync();
        Task<IEnumerable<SelectListItemDto>> GetSelectInoviceAsync();
        Task AddInvoiceService(InvoiceService service);
        Task AddInvoiceAttachment(List<InvoiceAttachment> entity);
        Task DeleteInvoiceService(int invoiceId);
        Task AddMultipleInvoiceService(List<InvoiceService> entity);
        Task DeleteAttachmentByInvoiceId(int invoiceId);
        Task ApproveInvoiceAsync(int invoiceId,int userId);
    }
}
