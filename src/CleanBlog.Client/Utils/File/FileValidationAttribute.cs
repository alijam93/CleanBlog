using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Client.Utils.File
{
    public class FileValidationAttribute : ValidationAttribute
    {
        public FileValidationAttribute(string[] allowedExtensions)
        {
            AllowedExtensions = allowedExtensions;
        }


        private string[] AllowedExtensions { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = (IBrowserFile)value;

            if (file != null)
            {
                var extension = Path.GetExtension(file.Name);
                if (!AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                {
                    return new ValidationResult($"This file must contains this formats:{string.Join(", ", AllowedExtensions)} :فایل ها باید شامل یکی از پسوند های زیر باشد",
                        new[] { validationContext.MemberName });
                }
            }


            return ValidationResult.Success;
        }
    }
}
