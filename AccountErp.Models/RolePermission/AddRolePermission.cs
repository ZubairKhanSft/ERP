using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.RolePermission
{
    public class AddRolePermission
    {
        public int Id { get; set; }
        public int PermissionId { get; set; }
        public int RoleId { get; set; }
        public bool CanCheck { get; set; }
    }
}
