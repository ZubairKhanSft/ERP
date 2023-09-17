using AccountErp.Dtos;
using AccountErp.Dtos.Project;
using AccountErp.Models.Project;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountErp.Infrastructure.Managers
{
    public interface IProjectManager
    {
        Task AddAsync(ProjectAddModel model);

        Task EditAsync(ProjectEditModel model);

        Task<ProjectDetailDto> GetDetailAsync(int id);

        Task<ProjectDetailForEditDto> GetForEditAsync(int id);

        Task<JqDataTableResponse<ProjectListItemDto>> GetPagedResultAsync(ProjectJqDataTableRequestModel model);

        Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync();

        Task DeleteAsync(int id);
    }
}
