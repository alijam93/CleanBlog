using CleanBlog.Client.Utils.Statics;
using CleanBlog.Shared.Dtos;
using CleanBlog.Shared.Features.Pagination;

using Microsoft.AspNetCore.WebUtilities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CleanBlog.Client.Pages.Article
{
    public partial class Index
    {
        private TagDTO[] Tags;
        int pageCount = 0;
        public string tagName;

        public List<PostDTO> Posts;
        public Paging Paging { get; set; } = new Paging();
        private readonly PostParameters _postParameters = new();

        protected override async Task OnInitializedAsync()
        {
            Tags = await http.GetFromJsonAsync<TagDTO[]>(Endpoints.Tags);

            var uri = navigation.ToAbsoluteUri(navigation.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var value))
            {
                pageCount = Convert.ToInt32(value);
            }
            await GetPostByTag(tagName); 
        }

        private async Task SelectedPage(int page)
        {
            _postParameters.PageNumber = page;
            await GetPostByTag(tagName);
        }

        public async Task GetPostByTag(string name)
        {
            var pagingPost = await PostService.GetPosts(_postParameters, name);
            Posts = pagingPost.Items;
            Paging = pagingPost.Paging;
        }
    }
}
