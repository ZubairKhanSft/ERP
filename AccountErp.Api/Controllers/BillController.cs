using AccountErp.Api.Helpers;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Services;
using AccountErp.Managers;
using AccountErp.Models.Bill;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccountErp.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
   // [Authorize]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillManager _manager;
        private readonly IHostingEnvironment _environment;
        private readonly IFileUploadService _fileUploadService;
        public BillController(IBillManager manager,
            IHostingEnvironment environment,IFileUploadService fileUploadService)
        {
            _manager = manager;
            _environment = environment;
            _fileUploadService = fileUploadService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody]BillAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            if (!EnumerableExtensions.Any(model.Items))
            {
                response.StatusCode = 404;
                response.Message = "Please select items to continue";
                return BadRequest(response);
            }

            try
            {
                if (model.BillNumber != null)
                {
                    if(model.Attachments!= null)
                    {
                        foreach (var attachment in model.Attachments)
                        {
                            attachment.FileName = await _fileUploadService.SaveFileAsync(attachment.FileName);
                        }
                    }

                    await _manager.AddAsync(model);
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

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody]BillEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            if (!EnumerableExtensions.Any(model.Items))
            {
                response.StatusCode = 404;
                response.Message = "Please select item to continue";
                return BadRequest(response);
            }

            try
            {
                if(model.Attachments != null)
                {
                    foreach (var attachment in model.Attachments)
                    {
                        if (attachment.Id == 0)
                        {
                            attachment.FileName = await _fileUploadService.SaveFileAsync(attachment.FileName);
                        }
                        
                    }
                }
                await _manager.Editsync(model);
                response.StatusCode = 200;
                response.Message = Constants.updated;
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        
        [HttpGet]
        [Route("get-select-items")]
        public async Task<IActionResult> GetSelectItems()
        {
            return Ok(await _manager.GetSelectItemsAsync());
        }

        [HttpPost]
        [Route("paged-result")]
        public async Task<IActionResult> PagedResult(BillJqDataTableRequestModel model)
        {
            return Ok(await _manager.GetPagedResultAsync(model));
        }

        [HttpPost]
        [Route("getTopFiveBills")]
        public async Task<IActionResult> getTopFiveBills(BillJqDataTableRequestModel model)
        {
            return Ok(await _manager.getTopFiveBillsAsync(model));
        }

        [HttpGet]
        [Route("get-recent")]
        public async Task<IActionResult> GetRecent()
        {
            return Ok(await _manager.GetRecentAsync());
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("get-detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            /*var bill = await _manager.GetDetailAsync(id);
            if (bill == null)
            {
                return NotFound();
            }
            return Ok(bill);*/

            var response = new BaseResponse<object>();

            if (id != 0)
            {
                var data = await _manager.GetDetailAsync(id);
                if (data != null)
                {
                    response.StatusCode = 200;
                    response.Data = data;
                    return Ok(response);
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "data not Present";
                    return Ok(response);
                }
            }
            response.StatusCode = 404;
            response.Message = Constants.provideValues;
            return BadRequest(response);
        }

        [HttpPost]
        [Route("getall")]
        public async Task<IActionResult> PagedResult(int PageSize, int Page,int VendorId,string filterKey,DateTime? BillDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _manager.GetAllPagination(PageSize, Page, VendorId,filterKey,BillDate);
            if (data.Item1.Count != 0)
            {
                response.Data = data.Item1;
                response.TotalCount = data.count;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return Ok(response);
        }

        [HttpPost]
        [Route("purchaseReport")]
        public async Task<IActionResult> PurchaseReport(int vendorId,DateTime From,DateTime To)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _manager.PurchaseReportAsync(vendorId,From,To);
            if (data.Item1.Count != 0)
            {
                response.Data = data.Item1;
                response.TotalCount = data.count;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return Ok(response);
        }



        [HttpGet]
        [Route("get-detail-for-edit/{id}")]
        public async Task<IActionResult> GetDetailForEdit(int id)
        {
            var bill = await _manager.GetDetailForEditAsync(id);

            if (bill == null)
            {
                return NotFound();
            }

            bill.Attachments = bill.Attachments.ToList();

            if (!bill.Attachments.Any())
            {
                return Ok(bill);
            }

            foreach (var attachment in bill.Attachments)
            {
                attachment.FileUrl = Utility.GetTempFileUrl(Request.GetBaseUrl(), attachment.FileName);
            }

            return Ok(bill);
        }

        [HttpGet]
        [Route("get-summary/{id}")]
        public async Task<IActionResult> GetSummary(int id)
        {
            return Ok(await _manager.GetSummaryAsunc(id));
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

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new BaseResponse<object>();

            if (id != 0)
            {

                await _manager.DeleteAsync(id);

                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);

            }
            response.StatusCode = 404;
            response.Message = Constants.provideValues;
            return BadRequest(response);
        }
        
        
        [HttpGet]
        [Route("get-BillNumber")]
        public async Task<IActionResult> GetBillNumber()
        {
            return Ok(await _manager.GetBillNumber());
        }
        [HttpGet]
        [Route("get-AllUnpaidBills")]
        public async Task<IActionResult> GetAllUnpaidBills()
        {
            return Ok(await _manager.GetAllUnpaidAsync());
        }
    }
}