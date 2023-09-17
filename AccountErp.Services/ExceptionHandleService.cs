using AccountErp.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Services
{
    public class ExceptionHandleService : IExceptionHandleService
    {
        public async Task<IActionResult> HandleException(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            while (ex != null)
            {
                sb.AppendLine(ex.Message);

                if (ex is NullReferenceException || ex.Message == "Nullable object must have a value.")
                {
                    sb.AppendLine(ex.StackTrace);
                }

                ex = ex.InnerException;
            }

            return new BadRequestObjectResult(new { statusCode = 400, message = sb.ToString() });
        }
    }
}
