using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanBlog.Shared.Dtos.Identity
{
    public class RoleDTO
    {
        public string Id { set; get; }
        public string Name { set; get; }
    }
}
