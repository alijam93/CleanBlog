using CleanBlog.Service.Interfaces;
using CleanBlog.Shared.Extensions;
using CleanBlog.Shared.Features.File;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBlog.Service.Core
{
    public class PhotoService : IPhotoService
    {
        public string AddAvatar(UploadModel file)
        {
            string extension = Path.GetExtension(file.Image.FileName);
            string newFileName = $"{Guid.NewGuid()}{extension}";

            string floderDate = DateTime.Now.ToString("yyyy/") + DateTime.Now.ToString("MM");
            var folderName = Path.Combine("wwwroot/", "images/profile/", floderDate);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            string dbPath = folderName + "/" + newFileName;

            if (file.Image.Length > 0)
            {

                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), folderName, newFileName);
                using var image = Image.Load(file.Image.OpenReadStream());
                image.Mutate(x => x.Resize(150, 150));
                //Encode here for quality
                var encoder = new JpegEncoder()
                {
                    Quality = 30 //Use variable to set between 5-30 based on your requirements
                };

                image.Save(fullPath, encoder);
            }
            return dbPath;
        }

        public string AddImagePost(UploadModel file)
        {
            string title = StringExtension.FriendlyUrl(file.Name);
            string extension = Path.GetExtension(file.Image.FileName);
            string newFileName = $"{title}{extension}";

            string floderDate = DateTime.Now.ToString("yyyy/") + DateTime.Now.ToString("MM");
            var folderName = Path.Combine("wwwroot/", "images/posts/", floderDate);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            string dbPath = folderName + "/" + newFileName;

            if (file.Image.Length > 0)
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), folderName, newFileName);
                using var image = Image.Load(file.Image.OpenReadStream());
                image.Mutate(x => x.Resize(300, 200));
                //Encode here for quality
                var encoder = new JpegEncoder()
                {
                    Quality = 30 //Use variable to set between 5-30 based on your requirements
                };

                image.Save(fullPath, encoder);
            }
            return dbPath;
        }
    }
}
