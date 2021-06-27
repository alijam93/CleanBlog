using CleanBlog.Shared.Dtos;
using CleanBlog.Shared.Features.File;
using CleanBlog.Shared.Features.Pagination;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Service.Interfaces
{
    public interface IPostService
    {
        Task<PagedList<PostDTO>> GetPosts(PostParameters postParameters);
        Task<PagedList<PostDTO>> GetPostsByTag(PostParameters postParameters, string name);
        Task<PostDTO> GetPostById(int id);
        Task AddPost(AddPostDTO postDTO);
        Task UpdatePost(EditPostDTO postDTO);
        Task DeletePost(int id);
        string SendImage(UploadModel image);
        bool PostItemExists(int id);
    }
}
