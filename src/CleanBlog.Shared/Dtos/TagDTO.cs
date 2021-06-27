using System;
using System.Collections.Generic;
using System.Text;

namespace CleanBlog.Shared.Dtos 
{
    public class TagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class AddTagDTO
    {
        public string Name { get; set; }
    }
    
    public class UpdateTagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
