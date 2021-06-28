using CleanBlog.Api.Controllers.Base;
using CleanBlog.Service.Interfaces;
using CleanBlog.Shared.Features.File;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Api.Controllers.Public
{
    public class PhotoController : ApiControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        /// <summary>
        /// Upload from user profile for a post.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost("addAvatar")]
        public ActionResult<string> SendAvatar([FromForm] UploadModel image)
        {
            var result = _photoService.AddAvatar(image);

            return Ok(result);
        }

        /// <summary>
        /// Upload from data image for a post.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost("addImage")]
        public ActionResult<string> SendImage([FromForm] UploadModel image)
        {
            var result = _photoService.AddImagePost(image);

            return Ok(result);
        }

        /// <summary>
        /// Delete the specified image.
        /// </summary>
        /// <param name="file"></param>
        [HttpPost("deleteFile")]
        public void Delete([FromBody] DeleteFile file)
        {
                var path = Path.Combine(Directory.GetCurrentDirectory(), file.Path);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                else
                {
                    BadRequest();
                }
        }
    }
}
