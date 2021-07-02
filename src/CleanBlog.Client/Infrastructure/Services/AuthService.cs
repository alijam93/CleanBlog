using Blazored.LocalStorage;
using CleanBlog.Client.Infrastructure.Services.Interfaces;
using CleanBlog.Shared.Identity.Account;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;


namespace CleanBlog.Client.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigation;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage, NavigationManager navigation)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
            _navigation = navigation;
        }

        public async Task<RegisterResult> Register(RegisterModel registerModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/accounts/Register", registerModel);

            return await result.Content.ReadFromJsonAsync<RegisterResult>();
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/accounts/Login", loginModel);
            var loginResult = await result.Content.ReadFromJsonAsync<LoginResult>();

            if (!result.IsSuccessStatusCode)
                return loginResult;

            await _localStorage.SetItemAsync("at", loginResult.AccessToken);
            await _localStorage.SetItemAsync("rt", loginResult.RefreshToken);
            await ((CustomAuthStateProvider)_authenticationStateProvider).LoggedIn(loginResult.AccessToken);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.AccessToken);

            return new LoginResult { IsSuccessful = true };
        }

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("at");
            var refreshToken = await _localStorage.GetItemAsync<string>("rt");

            try
            {
                var result = await _httpClient.PostAsJsonAsync("api/accounts/refresh-token", 
                     new RefreshTokenModel { AccessToken = token, RefreshToken = refreshToken });

                var rescon = await result.Content.ReadAsStringAsync();
                var refreshResult = await result.Content.ReadFromJsonAsync<LoginResult>();

                if (result.IsSuccessStatusCode)
                {
                    await _localStorage.SetItemAsync("at", refreshResult.AccessToken);
                    await _localStorage.SetItemAsync("rt", refreshResult.RefreshToken);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", refreshResult.AccessToken);
                }
                else
                {
                    _navigation.NavigateTo("/logout");
                }

                return refreshResult.AccessToken;
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Erorr is => {e}");
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("at");
            await _localStorage.RemoveItemAsync("rt");
            ((CustomAuthStateProvider)_authenticationStateProvider).LoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
