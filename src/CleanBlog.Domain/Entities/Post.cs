using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace CleanBlog.Domain.Entities
{
    public class Post
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

        public string Image { get; set; }
        public int Like { get; set; }
        public int Visitor { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Created { get; set; } = DateTime.Now;
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? Updated { get; set; }

        public ICollection<PostTag> PostTags { get; set; } 
        public ICollection<Comment> Comments { get; set; }

    }
}
