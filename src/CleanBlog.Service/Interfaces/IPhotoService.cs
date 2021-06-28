using CleanBlog.Shared.Features.File;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBlog.Service.Interfaces
{
    public interface IPhotoService
    {
        string AddImagePost(UploadModel file);
        string AddAvatar(UploadModel file);
    }
}
