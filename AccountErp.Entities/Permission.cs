using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        //public int CompanyId { get; set; }
        public string Permisions { get; set; }
        public string PermisionDescription { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public string PermissionCode { get; set; }
        public string ScreenCode { get; set; }
        public string ScreenUrl { get; set; }
        public int ScreenId { get; set; }
        public Screen Screen { get; set; }
        public string Time_stamp { get; set; }
    }
}
