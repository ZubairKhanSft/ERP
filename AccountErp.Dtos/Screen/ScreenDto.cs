using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.Screen
{
    public class ScreenDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string ScreenName { get; set; }
        public string ScreenCode { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public string ScreenUrl { get; set; }
    }
}
