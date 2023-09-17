using AccountErp.Dtos;
using AccountErp.Dtos.Invoice;
using AccountErp.Entities;
using AccountErp.Factories;
using AccountErp.Infrastructure.DataLayer;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Infrastructure.Services;
using AccountErp.Models.Invoice;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountErp.Managers
{
    public class InvoiceManager : IInvoiceManager
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICreditMemoRepository _creditMemoRepository;
        private readonly IProjectRepository _ProjectRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly string _userId;

        public InvoiceManager(IHttpContextAccessor contextAccessor,
            IInvoiceRepository invoiceRepository,
            IUnitOfWork unitOfWork,
            IItemRepository itemRepository,
             ITransactionRepository transactionRepository,
            ICustomerRepository customerRepository,
           ICreditMemoRepository creditMemoRepository,
           IProjectRepository projectRepository,
           IFileUploadService fileUploadService)
        {
            _userId = contextAccessor.HttpContext.User.GetUserId();
            _invoiceRepository = invoiceRepository;
            _itemRepository = itemRepository;
            _customerRepository = customerRepository;
            _transactionRepository = transactionRepository;
            _creditMemoRepository = creditMemoRepository;
            _unitOfWork = unitOfWork;
            _ProjectRepository = projectRepository;
            _fileUploadService = fileUploadService;

        }

        public async Task<int> AddAsync(InvoiceAddModel model)
        {
            // var items = (await _itemRepository.GetAsync(model.Items)).ToList();

            //model.TotalAmount = items.Sum(x => x.Rate);

            //model.Tax = items.Where(x => x.IsTaxable).Sum(x => x.Rate * x.SalesTax.TaxPercentage / 100);

            //var customer = await _customerRepository.GetAsync(model.CustomerId);

            //if (customer.Discount != null)
            //{
            //    model.Discount = model.TotalAmount * customer.Discount / 100;
            //    model.TotalAmount = model.TotalAmount - (model.Discount ?? 0);
            //}

            //if (model.Tax != null)
            //{
            //    model.TotalAmount = model.TotalAmount + (model.Tax ?? 0);
            //}


            model.LineAmountSubTotal = model.Items.Sum(x => x.LineAmount);

            var count = await _invoiceRepository.getCount();

            //await _invoiceRepository.AddAsync(InvoiceFactory.Create(model, _userId, items));

            try
            {



                var invoice = InvoiceFactory.Create(model, _userId, count);
                await _invoiceRepository.AddAsync(invoice);
                await _unitOfWork.SaveChangesAsync();
                return invoice.Id;
            }
            catch(Exception ex)
            {
                throw ex;

            }
            //SaveProject

            /*var projectData =await _ProjectRepository.GetAsyncByCustId(model.CustomerId);
            if (projectData != null)
            {
                await _ProjectRepository.AddProjectTransactionAsync(ProjectFactory.CreateByInvoice(invoice, projectData.Id, _userId));
            await _unitOfWork.SaveChangesAsync();
            }
*/


           /* var transaction = TransactionFactory.CreateByInvoice(invoice);
            await _transactionRepository.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            var itemsList = (model.Items.GroupBy(l => l.BankAccountId, l => new { l.BankAccountId, l.LineAmount })
        .Select(g => new { GroupId = g.Key, Values = g.ToList() })).ToList();

            foreach(var item in itemsList)
            {
                var id = item.GroupId;
                var amount = item.Values.Sum(x => x.LineAmount);

                var itemsData = TransactionFactory.CreateByInvoiceItemsAndTax(invoice,id, amount);
                await _transactionRepository.AddAsync(itemsData);
                await _unitOfWork.SaveChangesAsync();
            }

            var taxlistList = (model.Items.GroupBy(l => l.TaxBankAccountId, l => new { l.TaxBankAccountId, l.TaxPrice })
       .Select(g => new { GroupId = g.Key, Values = g.ToList() })).ToList();

            foreach (var tax in taxlistList)
            {
                if(tax.GroupId > 0)
                {
                    var id = tax.GroupId;
                    var amount = tax.Values.Sum(x => x.TaxPrice);

                    var taxData = TransactionFactory.CreateByInvoiceItemsAndTax(invoice, id, amount);
                    await _transactionRepository.AddAsync(taxData);
                    await _unitOfWork.SaveChangesAsync();
                }
               
            }*/

        }

        public async Task EditAsync(InvoiceEditModel model)
        {
            //var items = (await _itemRepository.GetAsync(model.Items)).ToList();

            //model.TotalAmount = items.Sum(x => x.Rate);

            //model.Tax = items.Where(x => x.IsTaxable).Sum(x => x.Rate * x.SalesTax.TaxPercentage / 100);

            //var customer = await _customerRepository.GetAsync(model.CustomerId);

            //if (customer.Discount != null)
            //{
            //    model.Discount = model.TotalAmount * customer.Discount / 100;
            //    model.TotalAmount = model.TotalAmount - (model.Discount ?? 0);
            //}

            //if (model.Tax != null)
            //{
            //    model.TotalAmount = model.TotalAmount + (model.Tax ?? 0);
            //}
           // await _transactionRepository.DeleteTransaction(model.Id);
            var invoice = await _invoiceRepository.GetAsync(model.Id);

            //InvoiceFactory.Create(model, invoice, _userId, items);
            InvoiceFactory.EditInvoice(model, invoice, _userId);

            _invoiceRepository.Edit(invoice);

            await _unitOfWork.SaveChangesAsync();
            await _invoiceRepository.DeleteInvoiceService(model.Id);
            await _unitOfWork.SaveChangesAsync();
            await _invoiceRepository.AddMultipleInvoiceService(InvoiceFactory.CreateMultipleInvoiceService(model.Items,model.Id));
            await _invoiceRepository.DeleteAttachmentByInvoiceId(model.Id);
            if (model.Attachments != null)
            {
                InvoiceAttachmentAddModel add = new InvoiceAttachmentAddModel();
                List<InvoiceAttachmentAddModel> attach = new List<InvoiceAttachmentAddModel>();
                foreach (var attachment in model.Attachments)
                {
                    if (attachment.Id == 0 && attachment.FileName != null && attachment.FileName != "" && attachment.FileName != "string")
                    {
                        add.FileName = await _fileUploadService.SaveFileAsync(attachment.FileName);
                        add.OriginalFileName = attachment.OriginalFileName;
                        add.Title = attachment.Title;
                        attach.Add(add);
                    }
                    else
                    {
                        add.FileName = attachment.FileName;
                        add.OriginalFileName = attachment.OriginalFileName;
                        add.Title = attachment.Title;
                        attach.Add(add);
                    }
                }
                if (attach.Count > 0)
                {
                   // await _invoiceManager.AddInvoiceAttachment(attach, model.UserId, model.Id);
                    await _invoiceRepository.AddInvoiceAttachment(InvoiceFactory.CreateInvoiceAttachment(attach, model.UserId, model.Id));

                }
            }

            /*var transaction = TransactionFactory.CreateByInvoice(invoice);
            await _transactionRepository.AddAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            var itemsList = (model.Items.GroupBy(l => l.BankAccountId, l => new { l.BankAccountId, l.LineAmount })
        .Select(g => new { GroupId = g.Key, Values = g.ToList() })).ToList();

            foreach (var item in itemsList)
            {
                var id = item.GroupId;
                var amount = item.Values.Sum(x => x.LineAmount);

                var itemsData = TransactionFactory.CreateByInvoiceItemsAndTax(invoice, id, amount);
                await _transactionRepository.AddAsync(itemsData);
                await _unitOfWork.SaveChangesAsync();
            }

            var taxlistList = (model.Items.GroupBy(l => l.TaxBankAccountId, l => new { l.TaxBankAccountId, l.TaxPrice })
       .Select(g => new { GroupId = g.Key, Values = g.ToList() })).ToList();

            foreach (var tax in taxlistList)
            {
                if (tax.GroupId > 0)
                {
                    var id = tax.GroupId;
                    var amount = tax.Values.Sum(x => x.TaxPrice);

                    var taxData = TransactionFactory.CreateByInvoiceItemsAndTax(invoice, id, amount);
                    await _transactionRepository.AddAsync(taxData);
                    await _unitOfWork.SaveChangesAsync();
                }

            }*/
        }

        public async Task<InvoiceDetailDto> GetDetailAsync(int id)
        {
            return await _invoiceRepository.GetDetailAsync(id);
        }
        public async Task<InvoiceDetailDto> GetDetailAsyncforpyment(int id)
        {
            return await _invoiceRepository.GetDetailAsyncforpyment(id);
        }

        public async Task<InvoiceDetailForEditDto> GetForEditAsync(int id)
        {
            return await _invoiceRepository.GetForEditAsync(id);
        }

        public async Task<JqDataTableResponse<InvoiceListItemDto>> GetPagedResultAsync(InvoiceJqDataTableRequestModel model)
        {
            return await _invoiceRepository.GetPagedResultAsync(model);
        }

        public async Task<(List<InvoiceDetailDto> data, int count)> GetAllAsync(int PageSize,int Page, string filterKey)
        {
            var data = await _invoiceRepository.GetAllAsync(PageSize,Page,filterKey);
            return data;
        }

        public async Task<(List<InvoiceDetailDto> data, int count)> InvoiceReportAsync(int CustomerId, DateTime From, DateTime To)
        {
            var data = await _invoiceRepository.InvoiceReportAsync(CustomerId, From, To);
            return data;
        }

        public async Task<List<InvoiceDetailDto>> TopSalesAsync()
        {
            return await _invoiceRepository.TopSalesAsync();
        }
        public async Task<ReportCountDto> CountReportAsync()
        {
            return await _invoiceRepository.CountReportAsync();
        }

        public async Task<(List<InvoiceDetailDto> data, int count)> InvoiceReportBySalesRepresentative(int userId, DateTime From, DateTime To)
        {
            var data = await _invoiceRepository.InvoiceReportBySalesRepresentative(userId, From, To);
            return data;
        }

        public async Task<JqDataTableResponse<InvoiceListItemDto>> GetTopFiveInvoicesAsync(InvoiceJqDataTableRequestModel model)
        {
            return await _invoiceRepository.GetTopFiveInvoicesAsync(model);
        }

        

        public async Task<List<InvoiceListItemDto>> GetRecentAsync()
        {
            return await _invoiceRepository.GetRecentAsync();
        }

        public async Task<List<InvoiceListItemDto>> GetAllUnpaidInvoiceAsync()
        {
            return await _invoiceRepository.GetAllUnpaidInvoiceAsync();
        }

        public async Task<InvoiceSummaryDto> GetSummaryAsunc(int id)
        {
            return await _invoiceRepository.GetSummaryAsunc(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _invoiceRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> GetInvoiceNumber()
        {
            var count = await _invoiceRepository.getCount();
            return (count + 1);
        }
        /*public async Task<List<InvoiceListTopTenDto>> GetTopTenInvoicesAsync()
        {
            InvoiceCountDto invoiceCountDto = new InvoiceCountDto();
            List<InvoiceListTopTenDto> invoiceListTopTens =  await _invoiceRepository.GetTopTenInvoicesAsync();
            invoiceCountDto.InvoiceListTopTensList = invoiceListTopTens.Take(5).ToList();
            invoiceCountDto.Count = invoiceListTopTens.Count;
            return invoiceListTopTens;
        }*/
        public async Task<InvoiceCountDto> GetTopTenInvoicesAsync()
        {
            InvoiceCountDto invoiceCountDto = new InvoiceCountDto();
            List<InvoiceListTopTenDto> invoiceListTopTens = await _invoiceRepository.GetTopTenInvoicesAsync();
            invoiceCountDto.InvoiceListTopTensList = invoiceListTopTens.Take(5).ToList();
            invoiceCountDto.Count = invoiceListTopTens.Count;
            return invoiceCountDto;
        }

        public async Task<IEnumerable<SelectListItemDto>> GetSelectInoviceAsync()
        {
            return await _invoiceRepository.GetSelectInoviceAsync();
        }

        public async Task AddInvoiceService(InvoiceServiceModel model,int invoiceId)
        {
            try
            {

            await _invoiceRepository.AddInvoiceService(InvoiceFactory.CreateInvoiceService(model, invoiceId));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task AddInvoiceAttachment(List<InvoiceAttachmentAddModel> model,int userId,int invoiceId)
        {
            try
            {
            await _invoiceRepository.AddInvoiceAttachment(InvoiceFactory.CreateInvoiceAttachment(model,userId,invoiceId));

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task ApproveInvoiceAsync(int invoiceId,int userId)
        {
            await _invoiceRepository.ApproveInvoiceAsync(invoiceId, userId);
        }
       
    }
}
