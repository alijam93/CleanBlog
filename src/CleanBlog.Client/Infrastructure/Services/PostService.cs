using CleanBlog.Client.Infrastructure.Services.Interfaces;
using CleanBlog.Client.Utils.Statics;
using CleanBlog.Components.Pagination;
using CleanBlog.Shared.Dtos;
using CleanBlog.Shared.Features.Pagination;

using Microsoft.AspNetCore.WebUtilities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanBlog.Client.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private string _url;
        private readonly HttpClient _http;

        public PostService(HttpClient http)
        {
            _http = http;
        }

        public async Task<PagingResponse<PostDTO>> GetPostsByTag(PostParameters postParameters, string name)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = postParameters.PageNumber.ToString()
            };

            _url = Endpoints.Posts + name;

            var response = await _http.GetAsync(QueryHelpers.AddQueryString(_url, queryStringParam));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var pagingResponse = new PagingResponse<PostDTO>
            {
                Items = JsonSerializer.Deserialize<List<PostDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                Paging = JsonSerializer.Deserialize<Paging>(response.Headers.GetValues("X-Pagination").First(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            };

            return pagingResponse;
        }

        public async Task<PagingResponse<PostDTO>> GetPosts(PostParameters postParameters, string name)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = postParameters.PageNumber.ToString()
            };
            //if (tagId > 0)
            if (name != null)
            {
                _url =Endpoints.Posts + name;
            }
            else
            {
                _url = Endpoints.Posts + name;
            }

            var response = await _http.GetAsync(QueryHelpers.AddQueryString(_url, queryStringParam));
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var pagingResponse = new PagingResponse<PostDTO>
            {
                Items = JsonSerializer.Deserialize<List<PostDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                Paging = JsonSerializer.Deserialize<Paging>(response.Headers.GetValues("X-Pagination").First(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            };

            return pagingResponse;
        }

        public async Task<PostDTO> GetPostsByById(int id, string slug)
        {
            return await _http.GetFromJsonAsync<PostDTO>($"{Endpoints.Posts}{id}/{slug}");
        }
    }
}
