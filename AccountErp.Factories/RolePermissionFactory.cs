using AccountErp.Entities;
using AccountErp.Models.RolePermission;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
    public class RolePermissionFactory
    {
        public static RolePermission Create(AddRolePermission model, string userId)
        {
            var data = new RolePermission()
            {
                Permissionid = model.PermissionId,
                Roleid = model.RoleId,
            };
            return data;
        }
    }
}
