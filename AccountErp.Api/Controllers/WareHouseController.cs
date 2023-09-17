using AccountErp.Api.Helpers;
using AccountErp.Infrastructure.Managers;
using AccountErp.Models.WareHouse;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountErp.Api.Controllers
{
    [Route("api/Warehouse")]

    [Produces("application/json")]
   // [Authorize]
    [ApiController]
    public class WareHouseController : ControllerBase
    {
        private readonly IWareHouseManager _manager;

        public WareHouseController(IWareHouseManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] WareHouseAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            
            var response = new BaseResponse<object>();
            try
            {
                if (model != null && model.Name != "string" && model.Name != "" && model.Name != null
                    && model.Location != "string" && model.Location != "" && model.Location != null)
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

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] WareHouseEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            try
            {
                var response = new BaseResponse<object>();
                if (model.Id != 0 && model.Name != "" && model.Name != "string" & model.Name != null)
                {
                    await _manager.EditAsync(model);
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
        public async Task<IActionResult> GetAllAsync(int PageSize,int Page)
        {
            var response = new BaseResponse<object>();

            
                var data = await _manager.GetAllAsync(PageSize,Page);
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

        [HttpPost]
        [Route("paged-result")]
        public async Task<IActionResult> GetPagedResult(WareHouseJqDataTableRequestModel model)
        {
            var pagedResult = await _manager.GetPagedResultAsync(model);
            return Ok(pagedResult);
        }

        [HttpGet]
        [Route("get-all-active-only")]
        public async Task<IActionResult> GetAllActiveOnly()
        {
            return Ok(await _manager.GetAllAsync(Constants.RecordStatus.Active));
        }
    }
}
