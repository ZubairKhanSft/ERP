using AccountErp.Api.Helpers;
using AccountErp.Entities;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Services;
using AccountErp.Managers;
using AccountErp.Models.Invoice;
using AccountErp.Utilities;
using Hangfire.MemoryStorage.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace AccountErp.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
   // [Authorize]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceManager _invoiceManager;
        private readonly IHostingEnvironment _environment;
        private readonly IEmailManager _emailManager;
        private readonly IFileUploadService _fileUploadService;
        public InvoiceController(IInvoiceManager invoiceManager,
            IHostingEnvironment environment, IEmailManager emailManager,IFileUploadService fileUploadService)
        {
            _invoiceManager = invoiceManager;
            _environment = environment;
            _emailManager = emailManager;
            _fileUploadService = fileUploadService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody]InvoiceAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            if (!EnumerableExtensions.Any(model.Items))
            {
                response.StatusCode = 404;
                response.Message = "Please select items/services to continue";
                return BadRequest(response);
            }

            try
            {
                int invoiceId = await _invoiceManager.AddAsync(model);
                InvoiceServiceModel service = new InvoiceServiceModel();
                if(model.Items != null && invoiceId !=0) 
                { 
                    foreach (var item in model.Items)
                    {
                        service.ServiceId = item.ServiceId;
                        service.TaxId = item.TaxId;
                        service.TaxPrice = item.TaxPrice;
                        service.TaxPercentage = item.TaxPercentage;
                        service.Quantity = item.Quantity;
                        service.Price = item.Price;
                        service.Rate = item.Rate;
                        service.BankAccountId = item.BankAccountId;
                        service.ProductId = item.ProductId;
                        service.TaxBankAccountId = item.TaxBankAccountId;
                        service.LineAmount = item.LineAmount;
                        await _invoiceManager.AddInvoiceService(service,invoiceId);

                    }
                    if (model.Attachments != null )
                    {
                        InvoiceAttachmentAddModel add = new InvoiceAttachmentAddModel();
                        List<InvoiceAttachmentAddModel> attach = new List<InvoiceAttachmentAddModel>(); 
                        foreach(var attachment in model.Attachments)
                        {
                            if (attachment.FileName != null && attachment.FileName != "" && attachment.FileName != "string")
                            {
                                add.FileName = await _fileUploadService.SaveFileAsync(attachment.FileName);
                                add.OriginalFileName = attachment.OriginalFileName;
                                add.Title = attachment.Title;
                                attach.Add(add);
                            }
                        }
                        if(attach.Count > 0)
                        {
                            await _invoiceManager.AddInvoiceAttachment(attach,model.UserId,invoiceId);

                        }
                    }
                    response.StatusCode = 200;
                    response.Message = Constants.added;
                    return Ok(response);
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = Constants.provideValues;
                    return BadRequest(response);
                }
               

              
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }



        [HttpGet]
        [Route("ApprovedInvoice")]
        public async Task<IActionResult> ApproveResult(int invoiceId,int userid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            try
            {
                if(invoiceId > 0 && userid > 0)
                {
                    await _invoiceManager.ApproveInvoiceAsync(invoiceId,userid);
                    response.StatusCode = 200;
                    response.Message = Constants.status;
                    return Ok(response);
                }
                response.StatusCode = 404;
                response.Message = Constants.error;
                return BadRequest(response);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("get-detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var invoice = await _invoiceManager.GetDetailAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }

            if (invoice.Attachments == null)
            {
                return Ok(invoice);
            }

            invoice.Attachments = invoice.Attachments.ToList();

            foreach (var attachment in invoice.Attachments)
            {
                attachment.FileUrl = Utility.GetTempFileUrl(Request.GetBaseUrl(), attachment.FileName);
            }

            return Ok(invoice);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("get-details/{id}")]
        public async Task<IActionResult> GetDetailAsyncforpyment(int id)
        {
            var invoice = await _invoiceManager.GetDetailAsyncforpyment(id);

            if (invoice == null)
            {
                return NotFound();
            }

            if (invoice.Attachments == null)
            {
                return Ok(invoice);
            }

            invoice.Attachments = invoice.Attachments.ToList();

            foreach (var attachment in invoice.Attachments)
            {
                attachment.FileUrl = Utility.GetTempFileUrl(Request.GetBaseUrl(), attachment.FileName);
            }

            return Ok(invoice);
        }

        [HttpGet]
        [Route("get-for-edit/{id}")]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var invoice = await _invoiceManager.GetForEditAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return Ok(invoice);
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody]InvoiceEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            if (!EnumerableExtensions.Any(model.Items))
            {
               
                response.StatusCode = 404;
                response.Message = "Please select items/services to continue";
                return BadRequest(response);
            }

            try
            {
                if(model.Id > 0)
                {
                    await _invoiceManager.EditAsync(model);
                    response.StatusCode = 200;
                    response.Message = Constants.updated;
                    return Ok(response);
                }
                response.StatusCode = 404;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("paged-result")]
        public async Task<IActionResult> GetPagedResult(InvoiceJqDataTableRequestModel model)
        {
            var pagedResult = await _invoiceManager.GetPagedResultAsync(model);

            return Ok(pagedResult);
        }

        [HttpPost]
        [Route("getAll")]
        public async Task<IActionResult> GetAllAsync(int PageSize,int Page,string filterKey)
        {
            /*var pagedResult = await _invoiceManager.GetAllAsync(PageSize,Page);

            return Ok(pagedResult);*/
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _invoiceManager.GetAllAsync(PageSize, Page,filterKey); 
            if (data.data.Count != 0)
            {
                response.Data = data.data;
                response.TotalCount = data.count;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return Ok(response);
        }

        [HttpGet]
        [Route("InvoiceReport")]
        public async Task<IActionResult> GetAllAsync(int CustomerId, DateTime From,DateTime To)
        {
            /*var pagedResult = await _invoiceManager.GetAllAsync(PageSize,Page);

            return Ok(pagedResult);*/
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _invoiceManager.InvoiceReportAsync(CustomerId, From, To);
            if (data.data.Count != 0)
            {
                response.Data = data.data;
                response.TotalCount = data.count;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return Ok(response);
        }
        
        [HttpGet]
        [Route("countReport")]
        public async Task<IActionResult> CountReport()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _invoiceManager.CountReportAsync();
            response.Data = data;
          //  response.TotalCount = ;
            response.StatusCode = 200;
            return Ok(response);
        }

        [HttpGet]
        [Route("topSalesOfMonth")]
        public async Task<IActionResult> TopSales()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _invoiceManager.TopSalesAsync();
            if (data.Count != 0)
            {
                response.Data = data;
               // response.TotalCount = data.count;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return Ok(response);
        }

        [HttpGet]
        [Route("InvoiceReport-by-salesRepresentative")]
        public async Task<IActionResult> InvoiceReportBySalesRepresentative(int userid, DateTime From, DateTime To)
        {
            /*var pagedResult = await _invoiceManager.GetAllAsync(PageSize,Page);

            return Ok(pagedResult);*/
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _invoiceManager.InvoiceReportBySalesRepresentative(userid, From, To);
            if (data.data.Count != 0)
            {
                response.Data = data.data;
                response.TotalCount = data.count;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return Ok(response);
        }

        [HttpPost]
        [Route("getTopFiveInvoice")]
        public async Task<IActionResult> GetTopFiveInvoices(InvoiceJqDataTableRequestModel model)
        {
            var pagedResult = await _invoiceManager.GetTopFiveInvoicesAsync(model);

            return Ok(pagedResult);
        }
        



        [HttpGet]
        [Route("get-recent")]
        public async Task<IActionResult> GetRecent()
        {
            var pagedResult = await _invoiceManager.GetRecentAsync();
            return Ok(pagedResult);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new BaseResponse<object>();
            if (id != 0)
            {
                await _invoiceManager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = Constants.provideValues;
            return BadRequest(response);
        }

        [HttpPost]
        [Route("upload-attachment")]
        public async Task<IActionResult> UploadAttachment([FromForm]IFormFile file)
        {
            var dirPath = Utility.GetTempFolder(_environment.WebRootPath);

            var fileName = Utility.GetUniqueFileName(file.FileName);

            using (var fileStream = new FileStream(dirPath + fileName, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Ok(new
            {
                fileName,
                fileUrl = Utility.GetTempFileUrl(Request.GetBaseUrl(), fileName)
            });
        }

        [HttpGet]
        [Route("get-summary/{id}")]
        public async Task<IActionResult> GetSummary(int id)
        {
            return Ok(await _invoiceManager.GetSummaryAsunc(id));
        }
        
        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendInvoice(InvoiceSendModel model)
        {
            var invoice = await _invoiceManager.GetDetailAsync(model.Id);
            if (invoice.Customer.Email == null)
            {
                BadRequest("Customer doesn't have email address");
            }

            var dirPath = Utility.GetInvoiceFolder(_environment.WebRootPath);
            var completePath = dirPath + invoice.Id + "_"  + ".pdf";
            if (!System.IO.File.Exists(completePath))
            {
                var renderer = new IronPdf.HtmlToPdf();
                renderer.RenderHtmlAsPdf(model.Html).SaveAs(completePath);
            }
            await _emailManager.SendInvoiceAsync(invoice.Customer.Email, completePath);
            return Ok();
        }

        [HttpGet]
        [Route("get-InvoiceNumber")]
        public async Task<IActionResult> GetInvoiceNumber()
        {
            return Ok(await _invoiceManager.GetInvoiceNumber());
        }

        [HttpGet]
        [Route("get-AllUnPaid")]
        public async Task<IActionResult> GetAllUnpaid()
        {
            var pagedResult = await _invoiceManager.GetAllUnpaidInvoiceAsync();
            return Ok(pagedResult);
        }

        [HttpGet]
        [Route("getTopTenInvoice")]
        public async Task<IActionResult> GetTopTenInvoices()
        {
            var pagedResult = await _invoiceManager.GetTopTenInvoicesAsync();

            return Ok(pagedResult);
        }


        [HttpGet]
        [Route("getProductInvoice")]
        public async Task<IActionResult> GetSelectInoviceAsync()
        {
            return Ok(await _invoiceManager.GetSelectInoviceAsync());
        }
    }
}