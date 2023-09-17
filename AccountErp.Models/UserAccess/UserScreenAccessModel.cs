using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.UserAccess
{
    public class UserScreenAccessModel
    {
        public int userRoleId { get; set; }
        public int screenId { get; set; }
        public bool canAccess { get; set; }
    }
}
