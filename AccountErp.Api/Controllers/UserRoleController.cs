using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountErp.Api.Helpers;
using AccountErp.Infrastructure.Managers;
using AccountErp.Models.UserLogin;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountErp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleManager _manager;
        private readonly IHostingEnvironment _environment;

        public UserRoleController(IUserRoleManager manager,
            IHostingEnvironment environment)
        {
            _manager = manager;
            _environment = environment;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] UserRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            try
            {
                var response = new BaseResponse<object>();
                if (model.Id == 0 && model.RoleName != "" && model.RoleName != "string" && model.RoleName != null)
                {
                    await _manager.AddAsync(model);
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
       
        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] UserRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            try
            {
                var response = new BaseResponse<object>();
                if (model.Id != 0 && model.RoleName != "" && model.RoleName != "string" && model.RoleName != null)
                {
                    await _manager.EditAsync(model);
                    response.StatusCode = 200;
                    response.Message = Constants.updated;
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
        [AllowAnonymous]
        [Route("get-detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var response = new BaseResponse<object>();

            if (id != 0)
            {
                var data = await _manager.GetDetailAsync(id);
                if(data != null)
                {
                    response.Data = data;
                    response.StatusCode = 200;
                    return Ok(response);
                }

                response.StatusCode = 404;
                response.Message = Constants.ndata;
                return BadRequest(response);
            }
            else
            {
                response.StatusCode = 404;
                response.Message = Constants.provideValues;
                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("paged-result")]
        public async Task<IActionResult> PagedResult(JqDataTableRequest model)
        {
            return Ok(await _manager.GetPagedResultAsync(model));
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
        [AllowAnonymous]
        [Route("get-all")]
        public async Task<IActionResult> GetAllAsync(int PageSize,int Page, string filterKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();
            var data = await _manager.GetAllAsync(PageSize, Page,filterKey);
            if (data.Count != 0)
            {
                response.Data = data;
                response.StatusCode = 200;
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = Constants.ndata;
            return Ok(response);
        }
    }
}