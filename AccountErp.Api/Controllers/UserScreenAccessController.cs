using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using AccountErp.Api.Helpers;
using AccountErp.Infrastructure.Managers;
using AccountErp.Models.UserAccess;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountErp.Api.Controllers
{
    [Route("api/user-screen-access")]
    [Produces("application/json")]
    [ApiController]
    public class UserScreenAccessController : ControllerBase
    {
        private readonly IUserAccessMAnager _manager;
        private readonly IHostingEnvironment _environment;

        public UserScreenAccessController(IUserAccessMAnager manager,
            IHostingEnvironment environment)
        {
            _manager = manager;
            _environment = environment;
        }


        [HttpPost]
        [Route("AddScreenAccess")]
        public async Task<IActionResult> AddQualifyAnswer(List<UserScreenAccessModel> model)
        {
            try
            {
                await _manager.AddUserScreenAccessAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }


        [HttpGet]
        [Route("getScreenAccessByUserRoleId/{id}")]
        public async Task<IActionResult> GetQualifyAnser(int id)
        {
          //  return Ok(await _manager.GetUserScreenAccessById(id));

            var response = new BaseResponse<object>();

            if (id != 0)
            {
                var data = await _manager.GetUserScreenAccessById(id);
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
        [Route("getAllScreens")]
        public async Task<IActionResult> GetAllScreenDetail()
        {
          //  return Ok(await _manager.GetAllScreenDetail());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();
            var data = await _manager.GetAllScreenDetail();
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
    }
}
