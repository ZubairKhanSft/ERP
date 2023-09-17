using AccountErp.Api.Helpers;
using AccountErp.Infrastructure.Managers;
using AccountErp.Models.CreditMemo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
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
    public class CreditMemoController : ControllerBase

    {
        private readonly ICreditMemoManager _creditmemoManager;
        public CreditMemoController(ICreditMemoManager creditmemoManager)
        {
            _creditmemoManager = creditmemoManager;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] CreditMemoAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            if (!EnumerableExtensions.Any(model.CreditMemoService))
            {
                return BadRequest("Please select items/services to continue");
            }

            try
            {
                await _creditmemoManager.AddAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [HttpPost]
        [Route("paged-result")]
        public async Task<IActionResult> GetPagedResult(CreditMemoJqDataTableRequestModel model)
        {
            var pagedResult = await _creditmemoManager.GetPagedResultAsync(model);

            return Ok(pagedResult);
        }

        [HttpGet]
        [Route("get-detail/{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var item = await _creditmemoManager.GetDetailAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] CreditMemoEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }
            try
            {
                await _creditmemoManager.EditAsync(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
