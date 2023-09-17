using AccountErp.Api.Helpers;
using AccountErp.Dtos.Transaction;
using AccountErp.Infrastructure.Managers;
using AccountErp.Models.Invoice;
using AccountErp.Models.Transaction;
using AccountErp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccountErp.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
   // [Authorize]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionManager _transactionManager;

        public TransactionController(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;

        }


        [HttpPost]
        [Route("paged-result")]
        public async Task<IActionResult> GetPagedResult(TransactionJqDataTableRequestModel model)
        {

            var pagedResult = await _transactionManager.GetPagedResultAsync(model);

            return Ok(pagedResult);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete(TransactionDeleteDto ids)
        {
            await _transactionManager.DeleteAsync(ids);

            return Ok();
        }

        [HttpGet]
        [Route("get-detail")]
        public async Task<IActionResult> GetDetail(int BankAccountId)
        {
            var item = await _transactionManager.GetDetailAsync(BankAccountId);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

    }
}
