using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text;

namespace CleanBlog.Domain.Entities
{
    // Self-referencing relationship
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int? ReplyId { get; set; }
        public int? PostId { get; set; }
        public int UserId { get; set; }

        [StringLength(25)]
        public string UserName { get; set; }

        [Required]
        [StringLength(3000)]
        public string Content { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }

        #region navigation 
        [ForeignKey("PostId")]
        public Post Post { get; set; }

        [ForeignKey("ReplyId")]
        public Comment ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; }
        #endregion
    }
}