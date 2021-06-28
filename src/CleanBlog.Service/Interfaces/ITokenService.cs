using CleanBlog.Domain.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CleanBlog.Service.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(List<Claim> claims);
        string GenerateRefreshToken();
        Task<List<Claim>> GetClaims(User user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
