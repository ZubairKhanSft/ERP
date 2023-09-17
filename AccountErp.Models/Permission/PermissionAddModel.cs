using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.Permission
{
    public class PermissionAddModel 
    {
        public string Permissions { get; set; }
        public string PermissionDescription { get; set; }

        public string PermissionCode { get; set; }
        public string ScreenCode { get; set; }
        public string ScreenUrl { get; set; }
        public int ScreenId { get; set; }
    }
}
