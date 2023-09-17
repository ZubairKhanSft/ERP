using AccountErp.Api.Helpers;
using AccountErp.Infrastructure.Managers;
using AccountErp.Models.SalesTax;
using AccountErp.Models.VendorSalesTax;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace AccountErp.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
   // [Authorize]
    [ApiController]
    public class SalesTaxController : ControllerBase
    {
        private readonly ISalesTaxManager _manager;

        public SalesTaxController(ISalesTaxManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] SalesTaxAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            if (await _manager.IsCodeExistsAsync(model.Code))
            {   
                response.StatusCode = 404;
                response.Message = "Sales tax of this code is already exists";
                return BadRequest(response);
            }
            try
            {
                if(model!= null)
                {
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

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody]SalesTaxEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            if (await _manager.IsCodeExistsAsync(model.Code,model.Id))
            {
                response.StatusCode = 404;
                response.Message = "Code for this vendor already exists";
                return BadRequest(response);
            }
            try
            {
                if(model.Id > 0)
                {
                     await _manager.EditAsync(model);
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
        [Route("get-for-edit/{id}")]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var salesTax = await _manager.GetForEditAsync(id);
            if (salesTax == null)
            {
                return NotFound();
            }
            return Ok(salesTax);
        }

        [HttpGet]
        [Route("get-select-list-items")]
        public async Task<IActionResult> GetVendorTax()
        {
            return Ok(await _manager.GetSelectListItemsAsync());
        }

        [HttpPost]
        [Route("paged-result")]
        public async Task<IActionResult> GetPagedResult(SalexTaxJqDataTableRequestModel model)
        {
            var pagedResult = await _manager.GetPagedResultAsync(model);
            return Ok(pagedResult);
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> GetAllAsync(int PageSize,int Page)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();
            var data = await _manager.GetAllAsync(Page, PageSize);
            if (data.Count != 0)
            {
                response.Data = data;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return BadRequest(response);
        }


        [HttpPost]
        [Route("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var response = new BaseResponse<object>();

            if (id != 0)
            {

                await _manager.ToggleStatusAsync(id);

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
        [Route("get-active-only")]
        public async Task<IActionResult> GetActiveOnly()
        {
           // return Ok(await _manager.GetActiveOnlyAsync());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();
            var data = await _manager.GetActiveOnlyAsync();
            if (!data.IsAllNullOrEmpty())
            {
                response.Data = data;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return BadRequest(response);
        }
    }
}