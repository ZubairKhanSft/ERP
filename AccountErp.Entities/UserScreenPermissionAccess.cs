using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Entities
{
    public class UserScreenPermissionAccess
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }

        public int RoleId { get; set; }
        public int ScreenId { get; set; }
        public int PermissionId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Time_stamp { get; set; }
    }
}
