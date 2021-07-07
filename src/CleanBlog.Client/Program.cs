using Blazored.LocalStorage;

using CleanBlog.Client.Infrastructure;
using CleanBlog.Client.Infrastructure.Services;
using CleanBlog.Client.Infrastructure.Services.Interfaces;
using CleanBlog.Shared.Identity.Auth;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MudBlazor.Services;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace CleanBlog.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient
            { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }.EnableIntercept(sp));
            //builder.Services.AddApiAuthorization();

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddHttpClientInterceptor();
            builder.Services.AddAntDesign();
            builder.Services.AddMudServices();

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddScoped<RefreshTokenService>();
            builder.Services.AddScoped<HttpInterceptorService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<IPostService, PostService>();

            builder.Services.AddAuthorizationCore(config =>
            {
                config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
                config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
            });

            await builder.Build().RunAsync();
        }
    }
}
