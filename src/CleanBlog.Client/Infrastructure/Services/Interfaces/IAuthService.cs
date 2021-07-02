using CleanBlog.Shared.Identity.Account;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Client.Infrastructure.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginModel loginModel);
        Task<RegisterResult> Register(RegisterModel registerModel);
        Task Logout();
        Task<string> RefreshToken();
    }
}
