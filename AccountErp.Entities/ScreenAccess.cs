using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Entities
{
    public class ScreenAccess
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int? RoleId { get; set; }
        public int ScreenId { get; set; }
        public bool CanAccess { get; set; }
        public Screen Screen { get; set; }
        public UserRole UserRole { get; set; }
        public string Time_stamp { get; set; }
    }
}
