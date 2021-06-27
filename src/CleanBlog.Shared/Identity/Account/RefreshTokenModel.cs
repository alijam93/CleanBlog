using System;
using System.Collections.Generic;
using System.Text;

namespace CleanBlog.Shared.Identity.Account
{
    public class RefreshTokenModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
