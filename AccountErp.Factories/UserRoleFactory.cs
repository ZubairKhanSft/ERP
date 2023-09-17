using AccountErp.Entities;
using AccountErp.Models.UserLogin;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
    public class UserRoleFactory
    {
        public static UserRole Create(UserRoleModel model, string userId)
        {
            var data = new UserRole
            {
                RoleName = model.RoleName,
                Status = Constants.RecordStatus.Active,
                CreatedBy = userId ?? "0",
                CreatedOn = Utility.GetDateTime(),
            };
            return data;
        }
        public static void Create(UserRoleModel model, UserRole entity, string userId)
        {
            entity.RoleName = model.RoleName;
            entity.UpdatedBy = userId ?? "0";
            entity.UpdatedOn = Utility.GetDateTime();
        }
    }
}
