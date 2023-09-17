using AccountErp.Api.Helpers;
using AccountErp.Dtos.Permission;
using AccountErp.Infrastructure.Managers;
using AccountErp.Models.Permission;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace AccountErp.Api.Controllers
{
    [Route("api/permission")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionManager _manager;



        public PermissionController(IPermissionManager manager
           )
        {
            _manager = manager;

        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] PermissionAddModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.GetErrorList());
                }
                
                await _manager.AddAsync(model);
                if (model != null)
                {
                    return Ok(new { statusCode = 200, message = "Permission Added Successfully!" });

                }
                else
                {
                    return Ok(new { badRequest = 400, message = "Not Found" });

                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { badRequest = 400, message = ex.Message });
            }
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] PermissionEditModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.GetErrorList());
                }
               
                await _manager.EditAsync(model);
                if (model != null)
                {
                    return Ok(new { statusCode = 200, message = "Permission Updated Successfully!" });

                }
                else
                {
                    return Ok(new { badRequest = 404, message = "Not Found" });

                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { badRequest = 400, message = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            try
            {
               
                var data = await _manager.GetDetailAsync(id);
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
       
        
        [HttpGet]
        [Route("get-detail-by-screenid/{id}")]
        public async Task<IActionResult> GetDetailByScreenId(int id)
        {
            try
            {
                
                var data = await _manager.GetDetailByScreenAsync(id);
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

            return Ok(new { statusCode = 200, message = "Record Deleted Successfully!" });


        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<PermissionDto> permissiondetail = new List<PermissionDto>();
              
                permissiondetail = await _manager.GetAllAsync();

                if (permissiondetail != null)
                {
                    return Ok(new { statusCode = 200, data = permissiondetail });
                }
                else
                {
                    return Ok(new { badRequest = 400, data = permissiondetail });

                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { badRequest = 400, message = ex.Message });
            }
        }


        [HttpGet]
        [Route("getall-pagination")]
        public async Task<IActionResult> GetAllPaginationAsync(int PageSize, int Page)
        {
            try
            {
                List<PermissionDto> permissiondetail = new List<PermissionDto>();
               
                permissiondetail = await _manager.GetAllPaginationAsync(PageSize, Page);
                var totaldoc = await _manager.GetAllCount();

                //var data= await _manager.GetAllAsync(PageSize,Page);
                if (permissiondetail != null)
                {
                    return Ok(new { statusCode = 200, data = permissiondetail, totalDocs = totaldoc, limit = PageSize });

                }
                else
                {
                    return Ok(new { badRequest = 400, data = permissiondetail, totalDocs = totaldoc, limit = PageSize });

                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { badRequest = 400, message = ex.Message });
            }
        }
    }
}
