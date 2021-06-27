using System;
using System.Collections.Generic;
using System.Text;

namespace CleanBlog.Shared.Dtos.Identity
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> UserRoles { get; set; }
    }  
    
    public class UserInfoDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }
    }

    public class UserUpdateDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }

    }
}
