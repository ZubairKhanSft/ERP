using AccountErp.Api.Helpers;
using AccountErp.Dtos.Screen;
using AccountErp.Infrastructure.Managers;
using AccountErp.Models.Screen;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountErp.Api.Controllers
{
    [Route("api/screen")]
    [ApiController]
    public class ScreenController : ControllerBase
    {
        private readonly IScreenManager _manager;
        public ScreenController(IScreenManager manager)
        {
            _manager = manager;
        }

        [Route("add")]
        [HttpPost]

        public async Task<IActionResult> Add([FromBody] AddScreenModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            try
            {
                if (model != null && model.Id == 0)
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

        [Route("edit")]
        [HttpPut]

        public async Task<IActionResult> Edit([FromBody] AddScreenModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            try
            {
               
                await _manager.EditAsync(model);
                return Ok(new { statusCode = 200, message = "Screen Updated SuccessFully!" });
            }
            catch (Exception)
            {
                return BadRequest(new { badRequest = 400, message = "Something Went Wrong" });
            }




        }


        [Route("get-detail/{id}")]
        [HttpGet]

        public async Task<IActionResult> GetDetail(int id)
        {
           
            var data = await _manager.GetDetailAsync(id);
            if (data != null)
            {

                return Ok(new { statusCode = 200, message = data });

            }
            else
            {
                return Ok(new { badRequest = 400, message = "Something Went Wrong" });

            }
        }

        [Route("get-all")]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(int PageSize, int Page)
        {
            try
            {
               
                List<ScreenDetailDto> screendetail = new List<ScreenDetailDto>();
                screendetail = await _manager.GetAllAsync(PageSize, Page);
                var totaldoc = await _manager.GetAllCount();
                if (screendetail != null)
                {
                    return Ok(new { statusCode = 200, data = screendetail, totalDocs = totaldoc, limit = PageSize });

                }
                else
                {
                    return Ok(new { badRequest = 404, data = screendetail, totalDocs = totaldoc, limit = PageSize });

                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { StatusCode = 400, ex.Message });
            }
        }

        [HttpGet]
        [Route("get-all-screens-withpermission")]
        public async Task<IActionResult> GetAllScreenPermission()
        {
            try
            {
              
                var data = await _manager.GetAllScreenPermission();
                if (data != null)
                {
                    return Ok(new { statusCode = 200, data = data });

                }
                else
                {
                    return Ok(new { badRequest = 404, data = data });

                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { statusCode = 400, message = ex.Message });
            }
        }



        [Route("delete/{id}")]
        [HttpDelete]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var header = Request.Headers["CompanyId"];
                int companyId = Convert.ToInt32(header);
                await _manager.DeleteAsync(id);
                return Ok(new { statusCode = 200, message = "Record Deleted Successfully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { badRequest = 400, message = ex.Message });

            }
        }
    }
}
