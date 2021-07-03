using CleanBlog.Api.Controllers.Base;
using CleanBlog.Service.Interfaces;
using CleanBlog.Shared.Dtos;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;


namespace CleanBlog.Api.Controllers.Admin
{
    public class TagsController : AdminControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Get all tags
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<TagDTO>> Tags()
        {
            return await _tagService.GetTags();
        }

        /// <summary>
        /// Get Tags which not selected in post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(Id)]
        public async Task<ActionResult<IEnumerable<TagDTO>>> UnselectedTags(int id)
        {
            var tags = await _tagService.GetUnselectedTags(id);
            return Ok(tags);

        }

        /// <summary>
        /// Add tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<AddTagDTO>> AddTag(AddTagDTO tag)
        {
            await _tagService.AddTag(tag);
            return Ok();
        }

        /// <summary>
        /// Update tag
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPut(Id)]
        public async Task<ActionResult<UpdateTagDTO>> UpdateTag(int id, UpdateTagDTO tag)
        {
            if (id != tag.Id) return BadRequest();

            await _tagService.UpdateTag(tag);
            return NoContent();
        }
    }
}
