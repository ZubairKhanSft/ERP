using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.UserScreenPermissionAccess
{
    public class AddUserScreenPermissionAccessModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public List<ScreenPermission> screens { get; set; }
    }

    public class ScreenPermission
    {

        public int ScreenId { get; set; }
        public List<screenPermission> Permissions { get; set; }
    }
    public class screenPermission
    {
        public int PermissionId { get; set; }
    }
}
