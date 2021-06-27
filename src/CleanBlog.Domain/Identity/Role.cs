using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanBlog.Domain.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public Role(string name)
        {
            Name = name;
        }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
