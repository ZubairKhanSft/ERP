using AccountErp.Dtos.Permission;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.Screen
{
    public class ScreenPermissionDetailDto
    {
        public int Id { get; set; }
        public string ScreenName { get; set; }
        public string ScreenCode { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public List<PermissionDto> ScreenPermission { get; set; }

        public string ScreenUrl { get; set; }
    }
}
