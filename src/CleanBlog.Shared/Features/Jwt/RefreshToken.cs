using System;

namespace CleanBlog.Shared.Features.Jwt
{
    public class RefreshToken
    {
        public string UserName { get; set; }     

        public string TokenString { get; set; }

        public DateTime ExpireAt { get; set; }
    }
}