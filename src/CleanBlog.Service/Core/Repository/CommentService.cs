using AutoMapper;

using CleanBlog.Data;
using CleanBlog.Domain.Entities;
using CleanBlog.Service.Interfaces;
using CleanBlog.Shared.Dtos;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Service.Repository.Core
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public CommentService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CommentDTO>> GetCommentsByPostId(int postId)
        {
            var comments = await _db.Comments.Where(x => x.PostId == postId && x.ReplyId == null)
                                             .Include(_ => _.Replies)
                                             .AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }

        public async Task<CommentDTO> GetCommentById(int id)
        {
            var comment = await _db.Comments.FindAsync(id);
            return _mapper.Map<CommentDTO>(comment);
        }

        public async Task AddComment(AddCommentDTO commentDTO)
        {
            if (commentDTO.PostId == null)
            {
                throw new DataException($"postId={commentDTO.PostId} is null");

            }
            var comment = _mapper.Map<Comment>(commentDTO);
            await _db.Comments.AddAsync(comment);
            await _db.SaveChangesAsync();
        }

        public async Task AddReply(AddReplyDTO replyDTO, int parentId)
        {
            if (replyDTO.PostId == null)
            {
                throw new DataException($"postId={replyDTO.PostId} is null");
            }
            else if (replyDTO.ReplyId == null)
            {
                throw new DataException($"replyId={replyDTO.ReplyId} is null");
            }

            var comment = _mapper.Map<Comment>(replyDTO);
            comment.ReplyId = parentId;
            await _db.Comments.AddAsync(comment);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateComment(int id, EditCommentDTO commentDTO)
        {
            if (commentDTO.PostId == null)
            {
                throw new DataException($"postId={commentDTO.PostId} is null");

            }

            var co = _db.Comments.FirstOrDefault(_ => _.Id.Equals(id)); 
            var comment = _mapper.Map(commentDTO, co);
            comment.Updated = DateTime.Now;
            _db.Entry(comment).State = EntityState.Modified;
            //_db.Entry(comment).Property(x => x.Created).IsModified = false;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteComment(int id)
        {
            var comment = await _db.Comments.FindAsync(id);

            if (comment == null)
            {
                throw new DataException($"This id => {id} not found.");
            }

            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();
        }

        public bool CommentItemExists(int id) => _db.Comments.Any(_ => _.Id == id);

    }
}
