using AccountErp.Entities;
using AccountErp.Models.UserAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
    public class UserScreenAccessFactory
    {
        public static void CreateUserScreenAccess(List<UserScreenAccessModel> model, List<UserScreenAccess> entities)
        {

            foreach (var item in model)
            {
                var data = new UserScreenAccess
                {
                  //  Id = Guid.NewGuid(),
                    UserRoleId = item.userRoleId,
                    ScreenId = item.screenId,
                    CanAccess = item.canAccess
                };
                entities.Add(data);
            }
        }
    }
}
