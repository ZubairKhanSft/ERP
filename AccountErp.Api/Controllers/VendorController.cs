using AccountErp.Api.Helpers;
using AccountErp.Infrastructure.Managers;
using AccountErp.Managers;
using AccountErp.Models.Vendor;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace AccountErp.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
  //  [Authorize]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IVendorManager _vendorManager;

        public VendorController(IVendorManager vendorManager)
        {
            _vendorManager = vendorManager;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody]VendorAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            if (await _vendorManager.IsEmailExistsAsync(model.Email))
            {
               
                response.StatusCode = 404;
                response.Message = "Email already exists";
                return BadRequest(response);
            }

            try
            {
                if (model != null)
                {
                    var vendorId = await _vendorManager.AddAsync(model);
                    
                    response.StatusCode = 200;
                    response.Message = Constants.added;
                    response.Data = vendorId;
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
        public async Task<IActionResult> Edit([FromBody] VendorEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            if (await _vendorManager.IsEmailExistsAsync(model.Id, model.Email))
            {
                response.StatusCode = 404;
                response.Message = "Email already exists";
                return BadRequest(response);
            }

            try
            {
                if (model.Id != 0)
                {
                    await _vendorManager.EditAsync(model);
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


        [HttpGet]
        [Route("get-detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
           
            var response = new BaseResponse<object>();

            if (id != 0)
            {
                var data = await _vendorManager.GetDetailAsync(id);
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
        public async Task<IActionResult> GetPagedResult(int PageSize, int Page,string filterKey)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _vendorManager.GetAllAsync(PageSize, Page,filterKey);
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
        [Route("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var response = new BaseResponse<object>();
            if (id != 0)
            {
                await _vendorManager.ToggleStatusAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.status;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = Constants.provideValues;
            return BadRequest(response);
        }


        [HttpPost]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new BaseResponse<object>();

            if (id != 0)
            {
                await _vendorManager.DeleteAsync(id);
                response.StatusCode = 200;
                response.Message = Constants.deleted;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = Constants.provideValues;
            return BadRequest(response);
        }


        [HttpGet]
        [Route("get-for-edit/{id}")]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var vendor = await _vendorManager.GetForEditAsync(id);

            if (vendor == null)
            {
                return NotFound();
            }

            return Ok(vendor);
        }

        
        [HttpPost]
        [Route("paged-result")]
        public async Task<IActionResult> GetPagedResult(VendorJqDataTableRequestModel model)
        {
            var pagedResult = await _vendorManager.GetPagedResultAsync(model);
            return Ok(pagedResult);
        }

        [HttpGet]
        [Route("get-personal-info/{id}")]
        public async Task<IActionResult> GetPersonalInfo(int id)
        {
            return Ok(await _vendorManager.GetPersonalInfoAsync(id));
        }

        [HttpGet]
        [Route("get-payment-info/{id}")]
        public async Task<IActionResult> GetPaymentInfo(int id)
        {
            return Ok(await _vendorManager.GetPaymentInfoAsync(id));
        }

        [HttpGet]
        [Route("get-select-items")]
        public async Task<IActionResult> GetSelectItems()
        {
            return Ok(await _vendorManager.GetSelectItemsAsync());
        }

        

       
    }
}