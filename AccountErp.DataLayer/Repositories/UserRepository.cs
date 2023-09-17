using AccountErp.Dtos.UserLogin;
using AccountErp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using AccountErp.Utilities;
using AccountErp.Infrastructure.Repositories;
using AccountErp.Models.UserLogin;

namespace AccountErp.DataLayer.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAsync(User entity)
        {
            await _dataContext.User.AddAsync(entity);
        }

        public async Task LoginAddAsync(LoginModule entity)
        {
            await _dataContext.LoginModule.AddAsync(entity);
        }

        public void Edit(User entity)
        {
            _dataContext.User.Update(entity);
        }

        public async Task<User> GetAsync(int id)
        {
            return await _dataContext.User.FindAsync(id);
        }

        public async Task<UserDetailDto> GetDetailAsync(int id)
        {
            return await (from s in _dataContext.User
                          where s.Id == id && s.Status != Constants.RecordStatus.Deleted
                          select new UserDetailDto
                          {
                              Id = s.Id,
                              FirstName = s.FirstName,
                              LastName = s.LastName,
                              UserName = s.UserName,
                              Password =s.Password,
                              Mobile = s.Mobile,
                              Email = s.Email,
                              RoleId = s.RoleId,
                              RoleName = s.Role.RoleName
                          })
                          .AsNoTracking()
                          .SingleOrDefaultAsync();
        }

        public async Task<JqDataTableResponse<UserDetailDto>> GetPagedResultAsync(JqDataTableRequest model)
        {
            if (model.Length == 0)
            {
                model.Length = Constants.DefaultPageSize;
            }

            var filterKey = model.Search.Value;

            var linqStmt = (from s in _dataContext.User
                            where s.Status != Constants.RecordStatus.Deleted && (model.filterKey == null || EF.Functions.Like(s.FirstName, "%" + model.filterKey + "%")
                            || EF.Functions.Like(s.LastName, "%" + model.filterKey + "%"))
                            select new UserDetailDto
                            {
                                Id = s.Id,
                                FirstName = s.FirstName,
                                LastName = s.LastName,
                                UserName = s.UserName,
                                Password = s.Password,
                                Mobile = s.Mobile,
                                Email = s.Email,
                                RoleId = s.RoleId,
                                RoleName = s.Role.RoleName
                            })
                            .AsNoTracking();

            var sortExpresstion = model.GetSortExpression();

            var pagedResult = new JqDataTableResponse<UserDetailDto>
            {
                RecordsTotal = await _dataContext.User.CountAsync(x => x.Status != Constants.RecordStatus.Deleted),
                RecordsFiltered = await linqStmt.CountAsync(),
                Data = await linqStmt.OrderBy(sortExpresstion).Skip(model.Start).Take(model.Length).ToListAsync()
            };
            return pagedResult;
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _dataContext.User.FindAsync(id);
            data.Status = Constants.RecordStatus.Deleted;
            _dataContext.User.Update(data);
        }

        public async Task<UserDetailDto> GetByUserAsync(string email)
        {
            return await (from s in _dataContext.User
                          where s.Email == email
                          select new UserDetailDto
                          {
                              Id = s.Id,
                              FirstName = s.FirstName,
                              LastName = s.LastName,
                              UserName = s.UserName,
                              Password = s.Password,
                              Mobile = s.Mobile,
                              Email = s.Email,
                              RoleId = s.RoleId,
                              RoleName = s.Role.RoleName
                          })
                         .AsNoTracking()
                         .SingleOrDefaultAsync();
        }
        public async Task<UserDetailDto> Login(UserLoginModel model)
        {
            return await (from s in _dataContext.User
                          where s.Email == model.Email && s.Password == Utility.Encrypt(model.Password)
                          select new UserDetailDto
                          {
                              Id = s.Id,
                              FirstName = s.FirstName,
                              LastName = s.LastName,
                              UserName = s.UserName,
                              Password = s.Password,
                              Mobile = s.Mobile,
                              Email = s.Email,
                              RoleId = s.RoleId,
                              RoleName = s.Role.RoleName
                          })
                         .AsNoTracking()
                         .SingleOrDefaultAsync();
        }
        public async Task<JqDataTableResponse<UserDetailDto>> GetAgentPagedResultAsync(JqDataTableRequest model)
        {
            if (model.Length == 0)
            {
                model.Length = Constants.DefaultPageSize;
            }

            var filterKey = model.Search.Value;

            var linqStmt = (from s in _dataContext.User
                            where s.Role.RoleName == "Agent"  && s.Status != Constants.RecordStatus.Deleted && (model.filterKey == null || EF.Functions.Like(s.FirstName, "%" + model.filterKey + "%")
                            || EF.Functions.Like(s.LastName, "%" + model.filterKey + "%"))
                            select new UserDetailDto
                            {
                                Id = s.Id,
                                FirstName = s.FirstName,
                                LastName = s.LastName,
                                UserName = s.UserName,
                                Password = s.Password,
                                Mobile = s.Mobile,
                                Email = s.Email,
                                RoleId = s.RoleId,
                                RoleName = s.Role.RoleName
                            })
                            .AsNoTracking();

            var sortExpresstion = model.GetSortExpression();

            var pagedResult = new JqDataTableResponse<UserDetailDto>
            {
                RecordsTotal = await _dataContext.User.CountAsync(x => x.Status != Constants.RecordStatus.Deleted),
                RecordsFiltered = await linqStmt.CountAsync(),
                Data = await linqStmt.OrderBy(sortExpresstion).Skip(model.Start).Take(model.Length).ToListAsync()
            };
            return pagedResult;
        }


        //with online status
        public async Task<JqDataTableResponse<UserDetailDto>> GetOnlineAgentPagedResultAsync(JqDataTableRequest model)
        {
            if (model.Length == 0)
            {
                model.Length = Constants.DefaultPageSize;
            }

            var filterKey = model.Search.Value;

            var linqStmt = (from s in _dataContext.User
                            join l in _dataContext.LoginModule on s.Id equals l.UserId into sl
                            from l in sl.DefaultIfEmpty()
                            where s.Role.RoleName == "Agent" && s.Status != Constants.RecordStatus.Deleted && (model.filterKey == null || EF.Functions.Like(s.FirstName, "%" + model.filterKey + "%")
                            || EF.Functions.Like(s.LastName, "%" + model.filterKey + "%"))
                            select new UserDetailDto
                            {
                                Id = s.Id,
                                FirstName = s.FirstName,
                                LastName = s.LastName,
                                UserName = s.UserName,
                                Password = s.Password,
                                Mobile = s.Mobile,
                                Email = s.Email,
                                RoleId = s.RoleId,
                                RoleName = s.Role.RoleName,
                                CallStatus = l.status ?? false
                            })
                            .AsNoTracking();

            var sortExpresstion = model.GetSortExpression();

            var pagedResult = new JqDataTableResponse<UserDetailDto>
            {
                RecordsTotal = await _dataContext.User.CountAsync(x => x.Status != Constants.RecordStatus.Deleted),
                RecordsFiltered = await linqStmt.CountAsync(),
                Data = await linqStmt.OrderBy(sortExpresstion).Skip(model.Start).Take(model.Length).ToListAsync()
            };
            return pagedResult;
        }

        //get only online agent
        public async Task<JqDataTableResponse<UserDetailDto>> GetOnlyOnlineAgentPagedResultAsync(JqDataTableRequest model)
        {
            if (model.Length == 0)
            {
                model.Length = Constants.DefaultPageSize;
            }

            var filterKey = model.Search.Value;

            var linqStmt = (from s in _dataContext.LoginModule
                            where s.user.Role.RoleName == "Agent" 
                            select new UserDetailDto
                            {
                                Id = s.Id,
                                FirstName = s.user.FirstName,
                                LastName = s.user.LastName,
                                UserName = s.user.UserName,
                                Password = s.user.Password,
                                Mobile = s.user.Mobile,
                                Email = s.user.Email,
                                RoleId = s.user.RoleId,
                                RoleName = s.user.Role.RoleName,
                                CallStatus = s.status ?? false
                            })
                            .AsNoTracking();

            var sortExpresstion = model.GetSortExpression();

            var pagedResult = new JqDataTableResponse<UserDetailDto>
            {
                RecordsTotal = await _dataContext.User.CountAsync(x => x.Status != Constants.RecordStatus.Deleted),
                RecordsFiltered = await linqStmt.CountAsync(),
                Data = await linqStmt.OrderBy(sortExpresstion).Skip(model.Start).Take(model.Length).ToListAsync()
            };
            return pagedResult;
        }



        public async Task LogOut(int id)
        {
            var data = await _dataContext.LoginModule.Where(x => x.UserId == id).FirstOrDefaultAsync();
            
                _dataContext.LoginModule.Remove(data);

        }

        public async Task<(List<UserDetailDto>, int count)> GetAllPagination(int PageSize, int Page,string FilterKey)
        {
            var response = new List<UserDetailDto>();
            int count = 0;
            if (PageSize !=0 && Page !=0 )
            {
                response= await (from s in _dataContext.User
                              where s.Status != Constants.RecordStatus.Deleted
                              && (FilterKey == null
                                || EF.Functions.Like(s.Id.ToString(), "%" + FilterKey + "%")
                                 || EF.Functions.Like(s.FirstName.ToString(), "%" + FilterKey + "%")
                                  || EF.Functions.Like(s.LastName.ToString(), "%" + FilterKey + "%"))
                                 select new UserDetailDto
                              {
                                  Id = s.Id,
                                  FirstName = s.FirstName,
                                  LastName = s.LastName,
                                  UserName = s.UserName,
                                  Password = s.Password,
                                  Mobile = s.Mobile,
                                  Email = s.Email,
                                  RoleId = s.RoleId,
                                  RoleName = s.Role.RoleName
                              })
                             .AsNoTracking()
                             .OrderByDescending(s => s.Id)
                             .Skip((Page - 1) * PageSize)
                             .Take(PageSize)
                             .ToListAsync();
                count =  _dataContext.User.Where(s=> s.Status != Constants.RecordStatus.Deleted).Count();
                return (response, count);
            }
            else
            {
                response = await (from s in _dataContext.User
                              where s.Status != Constants.RecordStatus.Deleted
                              select new UserDetailDto
                              {
                                  Id = s.Id,
                                  FirstName = s.FirstName,
                                  LastName = s.LastName,
                                  UserName = s.UserName,
                                  Password = s.Password,
                                  Mobile = s.Mobile,
                                  Email = s.Email,
                                  RoleId = s.RoleId,
                                  RoleName = s.Role.RoleName
                              })
                           .AsNoTracking()
                           .OrderByDescending(s => s.Id)
                           .ToListAsync();
                count = _dataContext.User.Where(s => s.Status != Constants.RecordStatus.Deleted).Count();
                return (response, count);
            }
            
        }

        public async Task<int> GetRoleId(string roleName)
        {
            return await _dataContext.UsersRoles.Where(s => s.RoleName == roleName).Select(s => s.Id).FirstOrDefaultAsync();
        }
    }
}
