using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Entities
{
    public class RolePermission
    {
        public int Id { get; set; }
        public int Permissionid { get; set; }

        public int Roleid { get; set; }
        public Permission Permission { get; set; }
        public UserRole Role { get; set; }
        public string Time_stamp { get; set; }
    }
}
