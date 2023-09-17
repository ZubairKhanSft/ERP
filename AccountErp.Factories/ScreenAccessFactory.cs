using AccountErp.Entities;
using AccountErp.Models.ScreenAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
    public class ScreenAccessFactory
    {
        public static void CreateUserScreenAccess(ScreenAccessModel model, List<ScreenAccess> entities)
        {


            var ids = model.ScreenId.Split(",");
            for (var i = 0; i < ids.Length; i++)
            {
                var data = new ScreenAccess
                {

                    RoleId = model.UserRoleId,
                    ScreenId = Convert.ToInt32(ids[i]),
                    CanAccess = model.CanAccess,
                };
                entities.Add(data);
            }


        }
    }
}
