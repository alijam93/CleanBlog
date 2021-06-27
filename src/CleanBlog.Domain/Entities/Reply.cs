using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace CleanBlog.Domain.Entities
{
    public class Reply
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Created { get; set; } = DateTime.Now;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? Updated { get; set; } = DateTime.Now;
    }
}
