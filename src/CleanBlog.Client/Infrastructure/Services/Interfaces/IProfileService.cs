
using CleanBlog.Shared.Dtos.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Client.Infrastructure.Services.Interfaces
{
    public interface IProfileService
    {
        Task<UserInfoDTO> UserProfile();
        Task<string> UserRole();
    }
}
