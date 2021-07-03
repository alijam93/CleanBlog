using CleanBlog.Components.Pagination;
using CleanBlog.Shared.Dtos;
using CleanBlog.Shared.Features.Pagination;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Client.Infrastructure.Services.Interfaces
{
    public interface IPostService
    {
        Task<PagingResponse<PostDTO>> GetPostsByTag(PostParameters postParameters, string name);
        Task<PagingResponse<PostDTO>> GetPosts(PostParameters postParameters, string name, int tagId);
        Task<PostDTO> GetPostsByById(int id, string slug);
    }
}
