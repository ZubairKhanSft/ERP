using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Entities
{
    public class UserScreenAccess
    {
        public int Id { get; set; }
        public int UserRoleId { get; set; }
        public int ScreenId { get; set; }
        public bool CanAccess { get; set; }
        public ScreenDetail Screen { get; set; }
    }
}
