using AccountErp.Dtos;
using AccountErp.Dtos.Project;
using AccountErp.Entities;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.Project;
using AccountErp.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace AccountErp.DataLayer.Repositories
{
    public class ProjectRepository:IProjectRepository
    { 
   private readonly DataContext _dataContext;

    public ProjectRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task AddAsync(Project entity)
    {
        await _dataContext.AddAsync(entity);
    }

        public async Task AddProjectTransactionAsync(ProjectTransaction entity)
        {
            await _dataContext.AddAsync(entity);
        }

        public void Edit(Project entity)
    {
        _dataContext.Update(entity);
    }

    public async Task<Project> GetAsync(int id)
    {
        return await _dataContext.Project.FindAsync(id);
    }

    public async Task<IEnumerable<Project>> GetAsync(List<int> itemIds)
    {
        return await _dataContext.Project.Where(x => itemIds.Contains(x.Id)).ToListAsync();
    }

        public async Task<Project> GetAsyncByCustId(int custId)
        {
            return await _dataContext.Project.Where(x => x.CustomerId == custId).FirstOrDefaultAsync();
        }
        public async Task<ProjectDetailDto> GetDetailAsync(int id)
    {
        return await (from s in _dataContext.Project
                      where s.Id == id
                      select new ProjectDetailDto
                      {
                          Id = s.Id,
                          ProjectName = s.ProjectName,
                          CustomerId = s.CustomerId,
                          CustomerName = s.Customer.FirstName
                      })
                      .AsNoTracking()
                      .SingleOrDefaultAsync();
    }

    public async Task<ProjectDetailForEditDto> GetForEditAsync(int id)
    {
        return await (from s in _dataContext.Project
                      where s.Id == id
                      select new ProjectDetailForEditDto
                      {
                          Id = s.Id,
                          ProjectName = s.ProjectName,
                          CustomerId = s.CustomerId,
                          CustomerName = s.Customer.FirstName
                      })
                     .AsNoTracking()
                     .SingleOrDefaultAsync();
    }

    public async Task<JqDataTableResponse<ProjectListItemDto>> GetPagedResultAsync(ProjectJqDataTableRequestModel model)
    {
        if (model.Length == 0)
        {
            model.Length = Constants.DefaultPageSize;
        }

        var filterKey = model.Search.Value;

        var linqStmt = (from s in _dataContext.Project
                        where s.Status != Constants.RecordStatus.Deleted
                            && (model.FilterKey == null
                            || EF.Functions.Like(s.ProjectName, "%" + model.FilterKey + "%"))
                        select new ProjectListItemDto
                        {
                            Id = s.Id,
                            ProjectName = s.ProjectName,
                            CustomerId = s.CustomerId,
                            CustomerName = s.Customer.FirstName
                        })
                        .AsNoTracking();

        var sortExpresstion = model.GetSortExpression();

        var pagedResult = new JqDataTableResponse<ProjectListItemDto>
        {
            RecordsTotal = await _dataContext.Items.CountAsync(x => x.Status != Constants.RecordStatus.Deleted),
            RecordsFiltered = await linqStmt.CountAsync(),
            Data = await linqStmt.OrderBy(sortExpresstion).Skip(model.Start).Take(model.Length).ToListAsync()
        };
        return pagedResult;
    }

    public async Task<IEnumerable<SelectListItemDto>> GetSelectItemsAsync()
    {
        return await _dataContext.Project
            .AsNoTracking()
            .Where(x => x.Status == Constants.RecordStatus.Active)
            .OrderBy(x => x.ProjectName)
            .Select(x => new SelectListItemDto
            {
                KeyInt = x.Id,
                Value = x.ProjectName
            }).ToListAsync();
    }


 
    public async Task DeleteAsync(int id)
    {
        var item = await _dataContext.Project.FindAsync(id);
        item.Status = Constants.RecordStatus.Deleted;
        _dataContext.Project.Update(item);

    }

  

}
}
