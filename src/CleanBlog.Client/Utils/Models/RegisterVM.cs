using CleanBlog.Client.Utils.File;
using CleanBlog.Shared.Identity.Account;

using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Client.Utils.Models
{ 
        public class RegisterVM : RegisterModel
        {
            public string FileName { get; set; }

            public string File { get; set; }

            [ClientMaxFileSizeAttribute(0.5 * 1024 * 1024)]
            [FileValidation(new[] { ".png", ".jpg" })]
            public IBrowserFile Picture { get; set; }
        } 
}
