using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace CleanBlog.Shared.Dtos
{ 
    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public int Likes { get; set; }
        public int Visitors { get; set; }
        public DateTime Created { get; set; } 
        public IEnumerable<TagDTO> Tags { get; set; } = new HashSet<TagDTO>(); 
    }

    public class AddPostDTO
    {
        [Required]
        [MinLength(25)]
        [MaxLength(100)]
        public string Title { get; set; } 

        [Required]
        [MinLength(1000)]
        [MaxLength(10000)]
        public string Content { get; set; }

        /// <summary>
        /// Response image path
        /// </summary>
        public string Image { get; set;  }

        [Required]
        public string AddTags { get; set; } 
    }


    public class EditPostDTO
    {
        public int Id { get; set; }

        [Required]
        [MinLength(25)]
        [MaxLength(80)]
        public string Title { get; set; }

        [Required]
        [MinLength(1000)]
        [MaxLength(10000)]
        public string Content { get; set; }

        /// <summary>
        /// Response image path
        /// </summary>
        public string Image { get; set; }

        public IEnumerable<int> AddTags { get; set; } = new List<int>();
        public IEnumerable<int> RemoveTags { get; set; } = new List<int>();
    }
}
