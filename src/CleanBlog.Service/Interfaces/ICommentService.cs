using CleanBlog.Shared.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Service.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDTO>> GetCommentsByPostId(int postId);
        Task<CommentDTO> GetCommentById(int id);
        Task AddComment(AddCommentDTO postDTO);
        Task AddReply(AddReplyDTO replyDTO, int parentId);
        //Task UpdateComment(EditCommentDTO commentDTO);
        Task UpdateComment(int id, EditCommentDTO commentDTO);
        Task DeleteComment(int id);
        bool CommentItemExists(int id);
    }
}
