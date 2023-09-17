using AccountErp.Dtos.UserAccess;
using AccountErp.Models.UserAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface IUserAccessMAnager
    {
        Task AddUserScreenAccessAsync(List<UserScreenAccessModel> model);
        Task<List<ScreenAccessDto>> GetUserScreenAccessById(int id);
        Task<List<ScreendetailDto>> GetAllScreenDetail();
    }
}
