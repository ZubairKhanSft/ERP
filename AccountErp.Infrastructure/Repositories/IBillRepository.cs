﻿using AccountErp.Dtos;
using AccountErp.Dtos.Bill;
using AccountErp.Entities;
using AccountErp.Models.Bill;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Repositories
{
    public interface IBillRepository
    {
        Task AddAsync(Bill entity);

        void Edit(Bill entity);

        Task<Bill> GetAsync(int id);

        Task<JqDataTableResponse<BillListItemDto>> GetPagedResultAsync(BillJqDataTableRequestModel model);

        Task<JqDataTableResponse<BillListItemDto>> getTopFiveBillsAsync(BillJqDataTableRequestModel model);

        

        Task<List<BillListItemDto>> GetRecentAsync();

        Task<BillDetailDto> GetDetailAsync(int id);
        Task<(List<BillDetailDto>, int count)> GetAllPagination(int PageSize, int Page, int vendorId, string filterKey, DateTime? BillDate);
        Task<(List<BillDetailDto>, int count)> PurchaseReportAsync(int vendorId,DateTime From,DateTime To);

        Task<BillDetailForEditDto> GetDetailForEditAsync(int id);

        Task<BillSummaryDto> GetSummaryAsunc(int id);

        Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync();

        Task UpdateStatusAsync(int id, Constants.BillStatus status);

        Task DeleteAsync(int id);
        Task<int> getCount();
        Task<List<BillListItemDto>> GetAllUnpaidAsync();

    }
}
