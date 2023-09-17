using AccountErp.Api.Helpers;
using AccountErp.Infrastructure.Managers;
using AccountErp.Models.Reconciliation;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccountErp.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
   // [Authorize]
    [ApiController]

    public class ReconciliationController : ControllerBase
    {
        private readonly IReconciliationManager _manager;

        public ReconciliationController(IReconciliationManager manager)
        {
            _manager = manager;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] ReconciliationAddModel model)
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
        public async Task<IActionResult> Edit([FromBody] ReconciliationEditModel model)
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
        [Route("get-detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var item = await _manager.GetDetailAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet]
        [Route("get-detail")]
        public async Task<IActionResult> GetByBankId(int BankAccountId)
        {
            var item = await _manager.GetByBankId(BankAccountId);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }


        /*[HttpPost]
        [Route("paged-result")]
        public async Task<IActionResult> GetPagedResult(ItemJqDataTableRequestModel model)
        {
            var pagedResult = await _manager.GetPagedResultAsync(model);
            return Ok(pagedResult);
        }*/

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllActiveOnly()
        {
            return Ok(await _manager.GetAllAsync());
        }


    }


}
