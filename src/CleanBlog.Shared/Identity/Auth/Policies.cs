using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Shared.Identity.Auth
{
    public static class Policies
    {
        public const string IsAdmin = "Admin";
        public const string IsUser = "User";
        public const string IsReadOnly = "IsReadOnly";
        public const string IsMyDomain = "IsMyDomain";

        public static AuthorizationPolicy IsAdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("Admin")
                                                   .Build();
        }

        public static AuthorizationPolicy IsUserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole("User")
                                                   .Build();
        }

        public static AuthorizationPolicy IsReadOnlyPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("ReadOnly", "true")
                .Build();
        }

        public static AuthorizationPolicy IsMyDomainPolicy()
        {
            return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new PermissionRequirement(Permissions.Admin.Create))
            .Build();
            //services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }
    }
}
