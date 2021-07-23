
using CleanBlog.Data;
using CleanBlog.Domain.Identity;
using CleanBlog.Domain.Settings;
using CleanBlog.Shared.Identity.Auth;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using SixLabors.ImageSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBlog.Api.Extentions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1d);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
             .AddEntityFrameworkStores<AppDbContext>()
             .AddDefaultTokenProviders();

            var jwtTokenConfig = configuration.GetSection("Jwt").Get<JwtSettings>();
            services.AddSingleton(jwtTokenConfig);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidIssuer = jwtTokenConfig.Issuer,
                      ValidAudience = jwtTokenConfig.Audience,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenConfig.Secret)),
                      ClockSkew = TimeSpan.Zero
                  };
              });

            #region Policy-based authorization
            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
                config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
            });
            #endregion
            return services;
        }
    }
}
