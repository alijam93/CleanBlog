using System;
using System.Collections.Generic;
using System.Text;

namespace CleanBlog.Shared.Features.Jwt
{
    public class JwtAuthResult
    {
        public string AccessToken { get; set; }

        public RefreshToken RefreshToken { get; set; }
    }
}
