using System.Collections.Generic;

namespace CleanBlog.Shared.Identity.Account
{
    public class RegisterResult
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
