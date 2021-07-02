using CleanBlog.Client.Utils.File;
using CleanBlog.Shared.Dtos;
using CleanBlog.Shared.Features.File;

using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Client.Utils.Models
{
    public class AddPostVM : AddPostDTO
    { 
        public string FileName { get; set; }

        [MaxFileSize(0.5 * 1024 * 1024)]
        [FileValidation(new[] { ".png", ".jpg" })]
        public IBrowserFile Picture { get; set; }
    }

    public class EditPostVM : EditPostDTO
    { 
        public string FileName { get; set; }

        [MaxFileSize(0.5 * 1024 * 1024)]
        [FileValidation(new[] { ".png", ".jpg" })]
        public IBrowserFile Picture { get; set; }
    }
}
