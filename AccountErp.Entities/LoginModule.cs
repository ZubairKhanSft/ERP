using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Entities
{
    public class LoginModule
    {
        public int Id { get; set; }
        public int UserId { get; set; } 
        public DateTime? createdOn { get; set; }
        public bool? status { get; set; }
        public int? RoleId { get; set; }
        public User user { get; set; }
    }
}
