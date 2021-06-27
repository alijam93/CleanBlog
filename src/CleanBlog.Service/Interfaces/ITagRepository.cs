using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Service.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<TagDTO>> GetTags();
        Task AddTag(AddTagDTO tagDTO);
        Task UpdateTag(UpdateTagDTO tagDTO);
        Task<IEnumerable<TagDTO>> GetUnselectedTags(int postId);
    }
}
