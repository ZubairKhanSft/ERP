using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Services
{
    public interface IFileUploadService
    {
        Task<string> SaveFileAsync(string file);

    }
}
