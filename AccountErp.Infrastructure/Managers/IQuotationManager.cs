using AccountErp.Dtos.Quotation;
using AccountErp.Models.Invoice;
using AccountErp.Models.Quotation;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface IQuotationManager
    {
        Task AddAsync(QuotationAddModel model);

        Task EditAsync(QuotationEditModel model);

        Task<QuotationDetailDto> GetDetailAsync(int id);

        Task<QuotationDeatilForEditDto> GetForEditAsync(int id);

        Task<JqDataTableResponse<QuotationListItemDto>> GetPagedResultAsync(QuotationJqDataTableRequestModel model);
        Task<(List<QuotationDetailDto>, int count)> GetAllAsync(int PageSize,int Page, string FilterKey, DateTime? QuotationDate);
        Task<List<QuotationDetailDto>> QuotationReportAsync(int CustomerId,DateTime From,DateTime To);
        Task<(List<QuotationDetailDto>, int count)> GetAllAsync();

        Task<List<QuotationListItemDto>> GetRecentAsync();

        Task<QuotationSummary> GetSummaryAsunc(int id);

        Task DeleteAsync(int id);
        Task<int> GetQuotationNumber();
    }
}