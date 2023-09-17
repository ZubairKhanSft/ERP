using AccountErp.Entities;
using AccountErp.Models.UserScreenPermissionAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
    public class UserScreenPermissionAccessFactory
    {
        public static List<UserScreenPermissionAccess> Create(AddUserScreenPermissionAccessModel model)
        {
            List<UserScreenPermissionAccess> access = new List<UserScreenPermissionAccess>();
            // UserScreenPermissionAccess acc = new UserScreenPermissionAccess();

            foreach (var item in model.screens)
            {
                if (item.Permissions.Count == 0)
                {
                    List<screenPermission> sp = new List<screenPermission>();
                    var dt = new UserScreenPermissionAccess
                    {
                        RoleId = model.RoleId,
                        ScreenId = item.ScreenId,
                        PermissionId = 0,
                    };
                    access.Add(dt);
                }

                foreach (var r in item.Permissions)
                {


                    var data = new UserScreenPermissionAccess
                    {
                        RoleId = model.RoleId,
                        ScreenId = item.ScreenId,
                        PermissionId = r.PermissionId,
                    };

                    access.Add(data);

                }
            }

            return access;
        }
    }
}

