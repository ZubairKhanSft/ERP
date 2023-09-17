using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.RolePermission
{
    public class RolePermissionDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int ScreenId { get; set; }
        public string PermissionTittle { get; set; }
        public int PermissionId { get; set; }
        public int RollId { get; set; }
        public string ScreenName { get; set; }
    }
}
