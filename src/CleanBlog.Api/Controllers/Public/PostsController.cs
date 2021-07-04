using CleanBlog.Api.Controllers.Base;
using CleanBlog.Service.Interfaces;
using CleanBlog.Shared.Dtos;
using CleanBlog.Shared.Extensions;
using CleanBlog.Shared.Features.File;
using CleanBlog.Shared.Features.Pagination;
using CleanBlog.Shared.Identity.Auth;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace CleanBlog.Api.Controllers.Public
{
    public class PostsController : ApiControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        ///  Get all posts
        /// </summary>
        /// <param name="postParameters"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PostDTO>> Posts([FromQuery] PostParameters postParameters)
        {
            var posts = await _postService.GetPosts(postParameters);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(posts.Paging));
            return Ok(posts);
        }

        /// <summary>
        /// Get posts by their tag
        /// </summary>
        /// <param name="postParameters"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<ActionResult<PostDTO>> PostsByTag([FromQuery] PostParameters postParameters,
                                                                                string name)
        {
            var posts = await _postService.GetPostsByTag(postParameters, name);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(posts.Paging));
            return Ok(posts);

        }

        /// <summary>
        /// Get post by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="slug"></param>
        /// <returns></returns>
        [HttpGet(Id + "/{slug}")]
        public async Task<ActionResult<PostDTO>> Post(int id, string slug)
        {
            var post = await _postService.GetPostById(id);
            // Get the actual friendly version of the title.
            var friendlyUrl = StringExtension.FriendlyUrl(post.Title);
            if (slug != friendlyUrl) throw new InvalidOperationException($"Slug format not matched. slug={slug}");

            return Ok(post);
        }

        /// <summary>
        /// Add post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Policy = Policies.IsAdmin)]
        public async Task<ActionResult<AddPostDTO>> AddPost(AddPostDTO post)
        {
            await _postService.AddPost(post);
            return Ok();
        }

        /// <summary>
        /// Edit post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPut(Id)]
        //[Authorize(Policy = Policies.IsAdmin)]
        public async Task<ActionResult<EditPostDTO>> EditPost(int id, EditPostDTO post)
        {
            if (id != post.Id) return BadRequest();

            try
            {
                await _postService.UpdatePost(post);
            }
            catch (DbUpdateConcurrencyException) when (!_postService.PostItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete(Id)]
        //[Authorize(Policy = Policies.IsAdmin)]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.DeletePost(id);
            return NoContent();
        }
    
        /// <summary>
        /// Upload from data image for a post.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost("AddImage")]
        public ActionResult<string> AddImage([FromForm] UploadModel image)
        {
            var result = _postService.SendImage(image);

            return Ok(result);
        }
    }
}
