using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.Permission
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Permissions { get; set; }
        public string PermissionDescription { get; set; }
        public string PermissionCode { get; set; }
        public string ScreenCode { get; set; }
        public string ScreenUrl { get; set; }

        public Constants.RecordStatus Status { get; set; }
        public int ScreenId { get; set; }
        public string ScreenName { get; set; }
    }
}
