using System;
using System.ComponentModel.DataAnnotations;

namespace CleanBlog.Shared.Identity.Account
{
    public class LoginModel
    {
        [Required]
        public string EmailOrName { get; set; } 

        [Required]
        public string Password { get; set; }

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool RememberMe { get; set; }
    }
}
