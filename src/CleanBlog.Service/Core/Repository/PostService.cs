using AutoMapper;

using CleanBlog.Data;
using CleanBlog.Domain.Entities;
using CleanBlog.Service.Interfaces;
using CleanBlog.Shared.Dtos;
using CleanBlog.Shared.Extensions;
using CleanBlog.Shared.Features.File;
using CleanBlog.Shared.Features.Pagination;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Service.Repository.Core
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public PostService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<PagedList<PostDTO>> GetPosts(PostParameters postParameters)
        {
            var post = await _db.Posts.Select(_ => new PostDTO
            {
                Id = _.Id,
                Title = _.Title,
                Content = _.Content,
                Image = _.Image,
                Likes = _.Likes,
                Visitors = _.Visitors,
                Created = _.Created,
                Tags = _.PostTags.Select(_ => new TagDTO { Name = _.Tag.Name }).ToList()
            }).ToListAsync();


            return PagedList<PostDTO>
                .ToPagedList(post, postParameters.PageNumber, postParameters.PageSize);
        }

        public async Task<PagedList<PostDTO>> GetPostsByTag(PostParameters postParameters, string name)
        {

            var tagId = await _db.Tags.Where(_ => _.Name == name).Select(_ => _.Id).ToListAsync();
            var post = await _db.PostTags.Where(_ => tagId.Contains(_.TagId))
                                            .Select(_ => new PostDTO
                                            {
                                                Id = _.Post.Id,
                                                Title = _.Post.Title,
                                                Content = _.Post.Content,
                                                Image = _.Post.Image,
                                                Likes = _.Post.Likes,
                                                Visitors = _.Post.Visitors,
                                                Created = _.Post.Created,
                                                Tags = _.Post.PostTags.Select(_ => new TagDTO { Name = _.Tag.Name }).ToList()
                                            }).ToListAsync();

            return PagedList<PostDTO>
               .ToPagedList(post, postParameters.PageNumber, postParameters.PageSize);
        }

        public async Task<PostDTO> GetPostById(int id)
        {

            var post = await _db.Posts.FindAsync(id);
            var tags = from post_tags in await _db.PostTags.ToListAsync()
                       join tag in await _db.Tags.ToListAsync()
                       on post_tags.TagId equals tag.Id
                       where post_tags.PostId.Equals(post.Id)
                       select new TagDTO
                       {
                           Id = tag.Id,
                           Name = tag.Name
                       };
            var postDetails = _mapper.Map<Post, PostDTO>(post);
            postDetails.Tags = tags;
            return postDetails;
        }


        public async Task AddPost(AddPostDTO postDTO)
        {
            var post = _mapper.Map<Post>(postDTO);
            List<int> TagIds = postDTO.AddTags.Split(',').Select(int.Parse).ToList();

            var tags = _db.Tags.Select(_ => _.Id).ToList();
            var existTagId = TagIds
                                  .All(id => tags.Contains(id));
            if (existTagId)
            {
                foreach (var tag in TagIds)
                {
                    post.PostTags.Add(new PostTag
                    {
                        TagId = tag,
                        PostId = post.Id
                    });
                }
            }
            else { throw new DataException("tagId not exsists"); }

            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();
        }


        public async Task UpdatePost(EditPostDTO postDTO)
        {
            var post = _db.Posts.Include(a => a.PostTags)
                    .SingleOrDefault(a => a.Id == postDTO.Id);

            if (postDTO.RemoveTags != null)
            {
                foreach (var tag in post.PostTags
                        .Where(at => postDTO.RemoveTags.Contains(at.TagId)).ToList())
                {
                    _db.PostTags.RemoveRange(tag);
                }
            }

            if (postDTO.AddTags != null)
            {

                var tags = _db.Tags.Select(_ => _.Id).ToList();
                var existTagId = postDTO.AddTags
                                      .All(id => tags.Contains(id));
                var addPostTags = postDTO.AddTags
                                     .Where(item => post.PostTags.Select(_ => _.TagId)
                                     .All(id => id != item)).ToList();
                foreach (var tag in addPostTags)
                {
                    await _db.PostTags.AddRangeAsync(new PostTag
                    {
                        TagId = tag,
                        PostId = post.Id
                    });
                }
                if (!existTagId) throw new Exception("tagId not exist in Tags");
            }


            var mappPost = _mapper.Map(postDTO, post);
            _db.Update(mappPost);
            await _db.SaveChangesAsync();
        }

        public async Task DeletePost(int id)
        {
            var post = await _db.Posts.FindAsync(id);

            if (post == null)
            {
                throw new DataException($"This id => {id} not found.");
            }
            var comment = _db.Comments.Where(_ => _.PostId == id);
            _db.Comments.RemoveRange(comment);

            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
        }

        public bool PostItemExists(int id) => _db.Posts.Any(_ => _.Id == id);

        public string SendImage(UploadModel file)
        {

            string title = StringExtension.FriendlyUrl(file.Name);
            string extension = Path.GetExtension(file.Image.FileName);
            string newFileName = $"{title}{extension}";

            var folderName = Path.Combine("wwwroot", "images/posts", DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"));
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            string dbPath = Path.Combine(folderName, newFileName);

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
