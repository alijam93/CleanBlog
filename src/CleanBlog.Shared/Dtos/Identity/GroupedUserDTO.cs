using System;
using System.Collections.Generic;
using System.Text;

namespace CleanBlog.Shared.Dtos.Identity
{
    public class GroupedUserDTO
    {
        public List<UserDTO> Users { get; set; }
        public List<UserDTO> Admins { get; set; }
    }
}
