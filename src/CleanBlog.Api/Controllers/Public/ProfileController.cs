using CleanBlog.Api.Controllers.Base;
using CleanBlog.Domain.Identity;
using CleanBlog.Shared.Dtos.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Api.Controllers.Public
{
    public class ProfileController : ApiControllerBase
    {
        private readonly UserManager<User> _userManager;

        public ProfileController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("getUserById/" + Id)]
        public async Task<ActionResult<UserInfoDTO>> UserById(string id)
        { 

            var userId = await _userManager.Users.FirstOrDefaultAsync(_ => _.Id == new Guid(id));
            var rolename = await _userManager.GetRolesAsync(userId);

            var user = new UserInfoDTO
            {
                UserName = userId.UserName,
                Email = userId.Email,
                Avatar = userId.Avatar,
                Role = rolename.FirstOrDefault()
            };

            return Ok(user); 
        }

        [HttpGet("getUserRoleById/" + Id)]
        public async Task<ActionResult<string>> UserRoleById(string id)
        {
            var userId = await _userManager.Users.FirstOrDefaultAsync(_ => _.Id == new Guid(id));
            var rolename = await _userManager.GetRolesAsync(userId);

            return Ok(rolename.FirstOrDefault());
        }
    }
}
