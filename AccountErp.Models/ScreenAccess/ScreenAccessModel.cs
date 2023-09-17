using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.ScreenAccess
{
    public class ScreenAccessModel
    {
        public int UserRoleId { get; set; }
        public string ScreenId { get; set; }
        public bool CanAccess { get; set; }
    }
}
