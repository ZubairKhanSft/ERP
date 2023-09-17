using AccountErp.Dtos.Permission;
using AccountErp.Dtos.Screen;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.UserScreenPermissionAccess
{
    public class UserScreenPermissionAccessDetailDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int RoleId { get; set; }
        public List<ScreenPermission> ScreenPermi { get; set; }
        public List<screenPermission> Permissionss { get; set; }
        public int ScreenId { get; set; }
        public int PermissionId { get; set; }
    }

    public class ScreensWithPermission
    {
        public ScreenDto screen { get; set; }
        public List<PermissionDto> permissions { get; set; }
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
