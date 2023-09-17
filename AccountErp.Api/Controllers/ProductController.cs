using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using AccountErp.Api.Helpers;
using AccountErp.Infrastructure.Managers;
using AccountErp.Infrastructure.Services;
using AccountErp.Managers;
using AccountErp.Models.Product;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountErp.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _manager;
        private readonly IFileUploadService _uploadService;

        public ProductController(IProductManager manager,IFileUploadService uploadService)
        {
            _manager = manager;
            _uploadService = uploadService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] ProductAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            try
            {
                var response = new BaseResponse<object>();  
                if (model != null)
                {
                    if(model.FileUrl != null && model.FileUrl != "string")
                    {
                        model.FileUrl = await _uploadService.SaveFileAsync(model.FileUrl);
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
        public async Task<IActionResult> Edit([FromBody] ProductEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            try
            {
                var response = new BaseResponse<object>();
                if (model.Id != 0 )
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
        [Route("get-detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
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

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetPagedResult(int PageSize,int Page,string filterKey)
        {
           /* var pagedResult = await _manager.GetAllAsync(PageSize,Page);
            
            return Ok(pagedResult);*/

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _manager.GetAllAsync(PageSize, Page,filterKey);
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
        [Route("getall-brand")]
        public async Task<IActionResult> GetBrand()
        {
            /* var pagedResult = await _manager.GetAllAsync(PageSize,Page);

             return Ok(pagedResult);*/

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _manager.GetAllBrandAsync();
            if (data.Count != 0)
            {
                response.Data = data;
                response.TotalCount = data.Count;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return Ok(response);
        }


        [HttpGet]
        [Route("getallModel-by-Brand")]
        public async Task<IActionResult> GetModel(string brandName)
        {
            /* var pagedResult = await _manager.GetAllAsync(PageSize,Page);

             return Ok(pagedResult);*/

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _manager.GetAllModelAsync(brandName);
            if (data.Count != 0)
            {
                response.Data = data;
                response.TotalCount = data.Count;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return Ok(response);
        }


        [HttpGet]
        [Route("getallSpecification-by-BrandModel")]
        public async Task<IActionResult> GetModel(string brandName,string modelName)
        {
            /* var pagedResult = await _manager.GetAllAsync(PageSize,Page);

             return Ok(pagedResult);*/

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _manager.GetAllSpecificationAsync(brandName,modelName);
            if (data.Count != 0)
            {
                response.Data = data;
                response.TotalCount = data.Count;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return Ok(response);
        }

        /*[HttpGet]
        [Route("get-for-edit/{id}")]
        public async Task<IActionResult> GetForEdit(int id)
        {
            var item = await _manager.GetForEditAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }*/

        /*[HttpPost]
        [Route("paged-result")]
        public async Task<IActionResult> GetPagedResult(ProductJqDataTableRequestModel model)
        {
            var pagedResult = await _manager.GetPagedResultAsync(model);
            return Ok(pagedResult);
        }*/


        [HttpPost]
        [Route("Get-Inventory-paged-result")]
        public async Task<IActionResult> GetInventoryPagedResultAsync(ProductInventoryJqDataTableRequestModel model)
        {
            var pagedResult = await _manager.GetInventoryPagedResultAsync(model);
            return Ok(pagedResult);
        }
        
        
        [HttpGet]
        [Route("getAll-inventory")]
        public async Task<IActionResult> GetAllInventoryAsync(DateTime? StartDate,DateTime? EndDate,string FilterKey,string Quantity)
        {
           /* var pagedResult = await _manager.GetInventoryAsync(StartDate,EndDate);
            return Ok(pagedResult);
*/
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _manager.GetInventoryAsync(StartDate, EndDate,FilterKey,Quantity);
            if (data.Count != 0)
            {
                response.Data = data;
              //  response.TotalCount = data.count;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = "data not present";
            return Ok(response);
        }
       
        
        /* [HttpGet]
         [Route("get-all-active-only")]
         public async Task<IActionResult> GetAllActiveOnly()
         {
             return Ok(await _manager.GetAllAsync(Constants.RecordStatus.Active));
         }*/

        /*[HttpGet]
        [Route("get-select-items")]
        public async Task<IActionResult> GetSelectItems()
        {
            return Ok(await _manager.GetSelectItemsAsync());
        }*/

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

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new BaseResponse<object>();

            if (id != 0)
            {
                if (_manager.checkItemAvailable(id))
                {
                    await _manager.DeleteAsync(id);

                    response.StatusCode = 200;
                    response.Message = Constants.deleted;
                    return Ok(response);
                }
                else
                {
                    return BadRequest("This Item & Services Is Already Exists.");
                }

            }
            response.StatusCode = 404;
            response.Message = Constants.provideValues;
            return BadRequest(response);
        }

       /* [HttpPost]
        [Route("transferWarehouse")]
        public async Task<IActionResult> TranserWareHouse(int id,int wareHouseId)
        {
            await _manager.TransferWareHouse(id, wareHouseId);
            return Ok();
        }*/

    }
}