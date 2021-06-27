using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanBlog.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        [StringLength(60)]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        public string Description { get; set; }
        public int Like { get; set; }
        public string Image { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string Link { get; set; }
        [NotMapped]
        public string File { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? Updated { get; set; } 

        public ICollection<Comment> Comments { get; set; }
    }
}
