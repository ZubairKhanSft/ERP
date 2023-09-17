using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Services
{
    public interface IExceptionHandleService
    {
       Task<IActionResult> HandleException(Exception ex);
    }
}
