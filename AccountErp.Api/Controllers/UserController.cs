using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccountErp.Api.Helpers;
using AccountErp.Infrastructure.Managers;
using AccountErp.Models.User;
using AccountErp.Models.UserLogin;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AccountErp.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserManager _manager;
        private readonly IHostingEnvironment _environment;

        public UserController(IConfiguration configuration, IUserManager manager,
            IHostingEnvironment environment)
        {
            _configuration = configuration;
            _manager = manager;
            _environment = environment;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] UserLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            if (await _manager.CheckUser(model.UserName) != null)
            {
                response.StatusCode = 404;
                response.Message = "UserName Already Exist";
                return BadRequest(response);
            }

            try
            {
                if (model != null && model.Id==0 && model.RoleId !=0 )
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
        [Route("add-admin")]
        public async Task<IActionResult> AddAdmin([FromBody] UserLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();

            if (await _manager.CheckUser(model.UserName) != null)
            {
                response.StatusCode = 404;
                response.Message = "UserName Already Exist";
                return BadRequest(response);
            }

            try
            {
                model.RoleId = await _manager.GetRoleId(model.RoleName);
                if (model != null && model.Id == 0 && model.RoleId != 0)
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
        public async Task<IActionResult> Edit([FromBody] UserLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            try
            {
                var response = new BaseResponse<object>();
                if (model.Id !=0 && model.RoleId !=0)
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
        [AllowAnonymous]
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

        [HttpPost]
        [Route("paged-result")]
        public async Task<IActionResult> PagedResult(JqDataTableRequest model)
        {
            return Ok(await _manager.GetPagedResultAsync(model));
        }


        [HttpPost]
        [Route("getall")]
        public async Task<IActionResult> PagedResult(int PageSize,int Page, string filterKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponseGet<object>();
            var data = await _manager.GetAllPagination(PageSize, Page,filterKey);
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
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();
            var data = await _manager.CheckUser(model.Email);
            if (data != null)
            {
                if (AccountErp.Utilities.Utility.Decrypt(model.Password,data.Password)==false)
                {
                    response.StatusCode = 404;
                    response.Message = "Invalid Password";
                    return BadRequest(response);
                }
                else
                {
                    await _manager.LoginAddAsync(data);

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:secret"));
                    var tokenDescription = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        { new Claim("id", data.Id.ToString()) ,
                            new Claim("Name", data.UserName.ToString()),
                            new Claim("FirstName", data.FirstName.ToString()),
                             new Claim("RoleId", data.RoleId.ToString()),
                             new Claim("RoleName", data.RoleName.ToString()),
                             new Claim("Email", data.Email.ToString()),
                        }
                        ),
                        Audience = _configuration.GetValue<string>("Jwt:Audience"),
                        Issuer = _configuration.GetValue<string>("Jwt:Issuer"),
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    
                    var token = tokenHandler.CreateToken(tokenDescription);
                    
                    response.Data = tokenHandler.WriteToken(token);
                    response.StatusCode = 200;
                    return Ok(response);
                }
            }
            else
            {
                response.StatusCode = 404;
                response.Message = "Invalid UserName";
                return BadRequest(response);
            }
           
            
        }

        [HttpPost]
        [Route("agent-paged-result")]
        public async Task<IActionResult> AgentPagedResult(JqDataTableRequest model)
        {
            return Ok(await _manager.GetAgentPagedResultAsync(model));
        }

        [HttpPost]
        [Route("logout/{id}")]
        public async Task<IActionResult> LogOut(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            var response = new BaseResponse<object>();
            if (id !=0)
            {
                 await _manager.LogOut(id);
                response.StatusCode = 200;
                response.Message = "logout successfully!!";
                return Ok(response);
            }
            response.StatusCode = 404;
            response.Message = Constants.provideValues;
            return BadRequest(response);
        }

        [HttpPost]
        [Route("onlineAgent-paged-result")]
        public async Task<IActionResult> OnlineAgentPagedResult(JqDataTableRequest model)
        {
            return Ok(await _manager.GetOnlineAgentPagedResultAsync(model));
        }

        [HttpPost]
        [Route("getOnly-Online-paged-result")]
        public async Task<IActionResult> OnlyneAgentPagedResult(JqDataTableRequest model)
        {
            return Ok(await _manager.GetOnlyOnlineAgentPagedResultAsync(model));
        }
    }
}