using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AccountErp.Api.Helpers;
using AccountErp.Entities;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Services;
using AccountErp.Models.Invoice;
using AccountErp.Models.Quotation;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace AccountErp.Api.Controllers
{
    [Route("api/[controller]")]
   // [Authorize]
    [ApiController]
    public class QuotationController : ControllerBase
    {
        private readonly IQuotationManager _quotationManager;
        private readonly IHostingEnvironment _environment;
        private readonly IEmailManager _emailManager;
        private readonly IFileUploadService _fileUploadService;
        public QuotationController(IQuotationManager quotationManager,
            IHostingEnvironment environment, IEmailManager emailManager,IFileUploadService fileUploadService)
        {
            _quotationManager = quotationManager;
            _environment = environment;
            _emailManager = emailManager;
            _fileUploadService = fileUploadService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] QuotationAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();


            if (!EnumerableExtensions.Any(model.Items))
            {
               // return BadRequest("");
                response.StatusCode = 404;
                response.Message = "Please select items/services to continue";
                return BadRequest(response);
            }

            try
            {
                if(model.CustomerId != 0)
                {
                    if (model.Attachments != null)
                    {
                        foreach (var attachment in model.Attachments)
                        {
                            attachment.FileName = await _fileUploadService.SaveFileAsync(attachment.FileName);
                        }
                    }
                    await _quotationManager.AddAsync(model);
                    response.StatusCode = 200;
                    response.Message = Constants.added;
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

        [HttpGet]
        [AllowAnonymous]
        [Route("get-detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var quotation = await _quotationManager.GetDetailAsync(id);
            var response = new BaseResponse<object>();

            if (quotation == null)
            {
                return NotFound();
            }

            if (quotation.Attachments == null)
            {
              /*  return Ok(quotation);*/
                response.StatusCode = 200;
                response.Data = quotation;
                return Ok(response);
            }

            quotation.Attachments = quotation.Attachments.ToList();

            foreach (var attachment in quotation.Attachments)
            {
                attachment.FileUrl = Utility.GetTempFileUrl(Request.GetBaseUrl(), attachment.FileName);
            }

            response.StatusCode = 200;
            response.Data = quotation;
            return Ok(response);
        }

        [HttpGet]
        [Route("get-for-edit/{id}")]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var quotation = await _quotationManager.GetForEditAsync(id);
            if (quotation == null)
            {
                return NotFound();
            }
            return Ok(quotation);
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] QuotationEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            if (!EnumerableExtensions.Any(model.Items))
            {
                return BadRequest("Please select items/services to continue");
            }
            var response = new BaseResponse<object>();

            try
            {
                if (model.Id != 0)
                {
                    if (model.Attachments != null)
                    {
                        foreach (var attachment in model.Attachments)
                        {
                            if (attachment.ID == 0)
                            {
                                attachment.FileName = await _fileUploadService.SaveFileAsync(attachment.FileName);
                            }

                        }
                    }
                    await _quotationManager.EditAsync(model);
                    response.StatusCode = 200;
                    response.Message = Constants.updated;
                    return Ok(response);
                }
                response.StatusCode = 404;
                response.Message =Constants.provideValues;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("paged-result")]
        public async Task<IActionResult> GetPagedResult(QuotationJqDataTableRequestModel model)
        {
            var pagedResult = await _quotationManager.GetPagedResultAsync(model);

            return Ok(pagedResult);
        }



        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAllAsync( int PageSize,int Page, string FilterKey, DateTime? QuotationDate)
        {
           /* var pagedResult = await _quotationManager.GetAllAsync(PageSize,Page);

            return Ok(pagedResult);*/

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _quotationManager.GetAllAsync(PageSize, Page,FilterKey,QuotationDate);
            if (data.Item1.Count != 0)
            {
                response.Data = data;
                response.TotalCount = data.count;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return Ok(response);
        }

        [HttpGet]
        [Route("quotationReport")]
        public async Task<IActionResult> QuotationReport(int customerId, DateTime From, DateTime To)
        {
            /* var pagedResult = await _quotationManager.GetAllAsync(PageSize,Page);

             return Ok(pagedResult);*/

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _quotationManager.QuotationReportAsync(customerId,From,To);
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
        [Route("getAll-salesOrderNumber")]
        public async Task<IActionResult> GetAllAsync()
        {
            /* var pagedResult = await _quotationManager.GetAllAsync(PageSize,Page);

             return Ok(pagedResult);*/

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _quotationManager.GetAllAsync();
            if (data.Item1.Count != 0)
            {
                response.Data = data;
                response.TotalCount = data.count;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return Ok(response);
        }

        [HttpGet]
        [Route("get-recent")]
        public async Task<IActionResult> GetRecent()
        {
            var pagedResult = await _quotationManager.GetRecentAsync();
            return Ok(pagedResult);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
           
            var response = new BaseResponse<object>();

            if (id != 0)
            {

                await _quotationManager.DeleteAsync(id);

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
        public async Task<IActionResult> UploadAttachment([FromForm] IFormFile file)
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
            return Ok(await _quotationManager.GetSummaryAsunc(id));
        }

        [HttpPost]
        [Route("send")]
        public async Task<IActionResult> SendInvoice(QuotationSendModel model)
        {
            var quotation = await _quotationManager.GetDetailAsync(model.Id);
            if (quotation.Customer.Email == null)
            {
                BadRequest("Customer doesn't have email address");
            }

            var dirPath = Utility.GetInvoiceFolder(_environment.WebRootPath);
            var completePath = dirPath + quotation.Id + "_" + ".pdf";
            if (!System.IO.File.Exists(completePath))
            {
                var renderer = new IronPdf.HtmlToPdf();
                renderer.RenderHtmlAsPdf(model.Html).SaveAs(completePath);
            }
            await _emailManager.SendInvoiceAsync(quotation.Customer.Email, completePath);
            return Ok();
        }

        [HttpGet]
        [Route("get-QuotationNumber")]
        public async Task<IActionResult> GetQuotationNumber()
        {
            return Ok(await _quotationManager.GetQuotationNumber());
        }
    }
}
