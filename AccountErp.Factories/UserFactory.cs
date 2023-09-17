using AccountErp.Dtos.UserLogin;
using AccountErp.Entities;
using AccountErp.Models.UserLogin;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
    public class UserFactory
    {
        public static User Create(UserLoginDto model, string userId)
        {
            var data = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                Password = Utility.Encrypt(model.Password),
                Mobile = model.Mobile,
                RoleId = model.RoleId,
                Status = Constants.RecordStatus.Active,
                CreatedBy = userId ?? "0",
                CreatedOn = Utility.GetDateTime(),
            };
            return data;
        }
        public static void Create(UserLoginDto model, User entity, string userId)
        {
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.UserName = model.UserName;
            entity.Email = model.Email;
            entity.RoleId = model.RoleId;
          //  entity.Password = Utility.Encrypt(model.Password);
            entity.Mobile = model.Mobile;
            entity.UpdatedBy = userId ?? "0";
            entity.UpdatedOn = Utility.GetDateTime();
        }


        public static LoginModule Login(UserDetailDto model)
        {
            var data = new LoginModule
            {
                UserId = model.Id,
                status = true,
                createdOn = Utility.GetDateTime(),
                RoleId = model.RoleId
            };
            return data;
        }
    }
}
