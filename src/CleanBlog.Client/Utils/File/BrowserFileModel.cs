using CleanBlog.Shared.Features.File;

using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Client.Utils.File
{
    public class BrowserFileModel
    {
        public string FileName { get; set; }

        [Required(ErrorMessage = "Image is empty!")]
        [MaxFileSize(0.5 * 1024 * 1024)]
        [FileValidation(new[] { ".png", ".jpg" })]
        public IBrowserFile Picture { get; set; }
    }
}
