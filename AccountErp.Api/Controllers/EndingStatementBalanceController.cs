using AccountErp.Api.Helpers;
using AccountErp.Infrastructure.Managers;
using AccountErp.Models.EndingStatementBalance;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountErp.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    // [Authorize]
    [ApiController]
    public class EndingStatementBalanceController : ControllerBase
    {
        private readonly IEndingStatementBalanceManager _manager;

        public EndingStatementBalanceController(IEndingStatementBalanceManager manager)
        {
            _manager = manager;
        }
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] EndingStatementBalanceAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            try
            {
                await _manager.AddAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] EndingStatementBalanceEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            try
            {
                await _manager.EditAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }  

        [HttpGet]
        [Route("get-detail/{BankAccountId}")]
        public async Task<IActionResult> GetDetail(int BankAccountId)
        {
            var item = await _manager.GetDetailAsync(BankAccountId);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

    }
}
