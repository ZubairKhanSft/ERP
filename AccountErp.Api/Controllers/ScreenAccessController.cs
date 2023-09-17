using AccountErp.Infrastructure.Managers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using AccountErp.Models.ScreenAccess;

namespace AccountErp.Api.Controllers
{
    [Route("api/user-screen-access")]
    [Produces("application/json")]
    [ApiController]
    public class ScreenAccessController : ControllerBase
    {
        private readonly IScreenAccessMAnager _manager;


        public ScreenAccessController(IScreenAccessMAnager manager
           )
        {
            _manager = manager;

        }


        [HttpPost]
        //[Authorize]
        [Route("add-screen-access")]
        public async Task<IActionResult> AddScreenAcess(ScreenAccessModel model)
        {
            try
            {
               
                await _manager.AddUserScreenAccessAsync(model);
                return Ok(new { statusCode = 200, message = "ScreenAccess Added SuccessFully!" });
            }
            catch (Exception ex)
            {
                return Ok(new { badRequest = 400, message = ex.Message });
            }

        }


        [HttpGet]
        //[Authorize]
        [Route("get-screenaccess-byuserroleid/{id}")]
        public async Task<IActionResult> GetScreenAccessByUserRoleId(int id)
        {
            try
            {
               
                var screens = await _manager.GetUserScreenAccessById(id);
                var permissions = await _manager.GetUserPermissionAccessById(id);
                var data = new
                {
                    screens,
                    permissions
                };

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
                return BadRequest(new { StatusCode = 400, message = ex.Message });
            }
        }

        /* [HttpGet("GetScreenAccess-With-Permissions-By-RoleId")]
         public async Task<IActionResult> GetScreenAccessWithPermissions(int roleid)
         {
             var data = await _manager.GetScreenPermissionByRoleId(roleid);
             if (data != null)
             {
                 return Ok(new { StatusCode = 200, Message = data });

             }
             else
             {
                 return Ok(new { BadRequest = 400, Message = "Something Went Wrong" });

             }
         }*/
        [HttpGet]
        //[Authorize]
        [Route("get-all-screens")]
        public async Task<IActionResult> GetAllScreenDetail()
        {
            try
            {
                
                var data = await _manager.GetAllScreenDetail();
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
                return BadRequest(new { badRequest = 400, message = ex.Message });
            }
        }
    }
}
