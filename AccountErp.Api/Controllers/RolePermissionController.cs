using AccountErp.Infrastructure.Managers;
using AccountErp.Models.RolePermission;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace AccountErp.Api.Controllers
{
    [Route("api/rolepermi")]
    [ApiController]
    public class RolePermissionController : ControllerBase
    {
        private readonly IRolePermissionManager _manager;


        public RolePermissionController(IRolePermissionManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] List<AddRolePermission> model)
        {


            try
            {
                
                foreach (var item in model)
                {
                    if (item.CanCheck == true)
                    {
                        var data1 = _manager.isExist(item);
                        if (data1 == null)
                        {
                            await _manager.AddAsync(item);
                        }
                    }
                    else
                    {
                        var data1 = _manager.isExist(item);
                        if (data1 != null)
                        {
                            await _manager.DeleteAsync2(data1.Id);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { badRequest = 400, message = ex.Message });
            }

            return Ok(new { statusCode = 200, message = "Role Permission Added SuccessFully!" });
        }



        [HttpDelete]

        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
              
                await _manager.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return Ok(new { badRequest = 400, message = ex.Message });

            }

            {
                return Ok(new { statusCode = 200, message = "Record Deleted Successfully!" });

            }
        }
    }
}
