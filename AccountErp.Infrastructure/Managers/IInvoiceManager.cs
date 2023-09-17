using AccountErp.Dtos;
using AccountErp.Dtos.Invoice;
using AccountErp.Models.Invoice;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface IInvoiceManager
    {
        Task<int> AddAsync(InvoiceAddModel model);

        Task EditAsync(InvoiceEditModel model);

        Task<InvoiceDetailDto> GetDetailAsync(int id);

        Task<InvoiceDetailDto> GetDetailAsyncforpyment(int id);


        Task<InvoiceDetailForEditDto> GetForEditAsync(int id);

        Task<JqDataTableResponse<InvoiceListItemDto>> GetPagedResultAsync(InvoiceJqDataTableRequestModel model);
        Task<(List<InvoiceDetailDto> data,int count)> GetAllAsync(int PageSize,int Page,string filterKey);
        Task<(List<InvoiceDetailDto> data,int count)> InvoiceReportAsync(int CustomerId,DateTime From,DateTime To);
        Task<List<InvoiceDetailDto>> TopSalesAsync();
        Task<ReportCountDto> CountReportAsync();
        Task<(List<InvoiceDetailDto> data,int count)> InvoiceReportBySalesRepresentative(int userId,DateTime From,DateTime To);


        Task<JqDataTableResponse<InvoiceListItemDto>> GetTopFiveInvoicesAsync(InvoiceJqDataTableRequestModel model);
        

        Task<List<InvoiceListItemDto>> GetRecentAsync();

        Task<InvoiceSummaryDto> GetSummaryAsunc(int id);

        Task DeleteAsync(int id);

        Task<int> GetInvoiceNumber();
        Task<List<InvoiceListItemDto>> GetAllUnpaidInvoiceAsync();
        Task<InvoiceCountDto> GetTopTenInvoicesAsync();
        Task<IEnumerable<SelectListItemDto>> GetSelectInoviceAsync();
        Task AddInvoiceService(InvoiceServiceModel model,int invoiceId);
        Task AddInvoiceAttachment(List<InvoiceAttachmentAddModel> model,int userId,int invoiceId);

        Task ApproveInvoiceAsync(int invoiceId, int userId);
    }
}
