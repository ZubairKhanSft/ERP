using AccountErp.Api.Helpers;
using AccountErp.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using AccountErp.Infrastructure.Managers;
using AccountErp.Models.UserScreenPermissionAccess;

namespace AccountErp.Api.Controllers
{
    [Route("api/userscreenaccess")]
    [Produces("application/json")]
    [ApiController]
    public class UserScreenPermissionAccessController : ControllerBase
    {
        private readonly IUserScreenPermissionAccessManager _manager;
        public UserScreenPermissionAccessController(IUserScreenPermissionAccessManager manager)
        {
            _manager = manager;
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add(AddUserScreenPermissionAccessModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            try
            {
                
                if (model.Id == 0)
                {
                    await _manager.AddAsync(model);
                }
                return Ok(new { statusCode = 200, message = "UserScreenPermission Added Successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { badRequest = 400, message = ex.Message });
            }
        }

        [HttpGet("getby-roleid")]
        public async Task<IActionResult> GetByRole(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.GetErrorList());
                }

               
                var data = await _manager.GetByRoleId(id);
                if (data != null)

                {
                    return Ok(new { statusCode = 200, data = data });

                }
                return Ok(new { statusCode = 404, data = data, });

            }
            catch (Exception ex)
            {
                return Ok(new { badRequest = 400, message = ex.Message });
            }

        }
    }
}
