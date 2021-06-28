using CleanBlog.Api.Controllers.Base;
using CleanBlog.Domain.Identity;
using CleanBlog.Service.Interfaces;
using CleanBlog.Shared.Identity.Account;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CleanBlog.Api.Controllers.Public
{
    public class AccountsController : ApiControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;



        public AccountsController(UserManager<User> userManager,
             SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(login.EmailOrName) ??
                       await _signInManager.UserManager.FindByNameAsync(login.EmailOrName);
            var result = await _signInManager.PasswordSignInAsync(user.UserName, login.Password, false, false);

            if (!result.Succeeded) return BadRequest(new LoginResult { IsSuccessful = false, ErrorMessage = "Email or Username is not valid." });

            var claims = await _tokenService.GetClaims(user);
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);

            var response = new LoginResult
            {
                IsSuccessful = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel register)
        {
            var newUser = new User
            {
                UserName = register.UserName,
                Email = register.Email,
                Avatar = register.Avatar
            };

            var result = await _userManager.CreateAsync(newUser, register.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return Ok(new RegisterResult { Successful = false, Errors = errors });

            }

            // Add all new users to the User role
            await _userManager.AddToRoleAsync(newUser, "User");

            return Ok(new RegisterResult { Successful = true });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenModel token)
        {
            if (token is null)
            {
                return Unauthorized(new LoginResult { IsSuccessful = false, ErrorMessage = "Token is null" });

            } 

            var principal = _tokenService.GetPrincipalFromExpiredToken(token.AccessToken);
            var username = principal.Identity.Name; 
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == username);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return Unauthorized(new LoginResult { IsSuccessful = false, ErrorMessage = "Invalid client request" });
            }
            var claims = await _tokenService.GetClaims(user);
            var newAccessToken = _tokenService.GenerateAccessToken(claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);

            await _userManager.UpdateAsync(user);

            var response = new LoginResult
            {
                IsSuccessful = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return Ok(response);
        }

        /// <summary>
        /// If result is true = token expired 
        /// Otherwise is false = token is valid
        /// </summary>
        /// <returns></returns>
        [HttpGet("expire-token")]
        public ActionResult<bool> Expire()
        {
            var username = User.Identity.Name;
            var user = _userManager.Users.SingleOrDefault(u => u.UserName == username);
            if (user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                user.RefreshToken = null;

                return true;
            }

            return false; 
        }
    }
}