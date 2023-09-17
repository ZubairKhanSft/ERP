using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.UserLogin
{
    public class UserLoginDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
    }
}
