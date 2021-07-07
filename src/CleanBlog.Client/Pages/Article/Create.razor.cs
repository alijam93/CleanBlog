using CleanBlog.Client.Utils.Models;
using CleanBlog.Client.Utils.Statics;
using CleanBlog.Shared.Dtos;
using CleanBlog.Shared.Features.File;

using Microsoft.AspNetCore.Components.Forms;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CleanBlog.Client.Pages.Article
{
    public partial class Create
    {
        protected string tagId;
        protected bool isTagValid = true;
        IEnumerable<string> _selectedValues;


        public string ImgUrl { get; set; }
        private EditContext editContext;
        public MultipartFormDataContent content = new();
        protected bool disableUpload = true;

        public AddPostVM Post = new();
        protected List<TagDTO> Tags = new();

        string button = "btn btn-primary";
        bool disable = false;

        protected override async Task OnInitializedAsync()
        {
            editContext = new EditContext(Post);

            Tags = await http.GetFromJsonAsync<List<TagDTO>>(Endpoints.Tags);
        }

        async Task AddArticle()
        {

            if (Post.Picture != null)
            {
                var resultUpload = await http.PostAsync(Endpoints.Photo + "addImage", content);
                var uploadAddress = await resultUpload.Content.ReadAsStringAsync();
                if (resultUpload.IsSuccessStatusCode)
                {
                    Post.Image = uploadAddress.Trim('"');
                    disableUpload = false;
                }
            }


            var result = await http.PostAsJsonAsync(Endpoints.Posts, Post);
            var postContent = await result.Content.ReadAsStringAsync();

            if (result.IsSuccessStatusCode)
            {
                Disable();
                navigation.NavigateTo($"panel/article");
            }
            else
            {
                await http.PostAsJsonAsync(Endpoints.Photo + "deleteFile", new DeleteFile { Path = Post.Image });
                throw new ApplicationException(postContent);
            }
        }

        private void OnSelectedItemsChangedHandler(IEnumerable<string> values)
        {
            Post.AddTags = string.Join(",", values);
            if (string.IsNullOrEmpty(Post.AddTags))
            {
                isTagValid = false;
            }
            else
            {
                isTagValid = true;
            }
        }

        void TagSubmit()
        {
            if (string.IsNullOrEmpty(Post.AddTags))
            {
                isTagValid = false;
            }
        }

        async Task OnChangeImage(InputFileChangeEventArgs e)
        {
            Post.Picture = e.File;
            Post.FileName = e.File.Name;
            editContext.NotifyFieldChanged(FieldIdentifier.Create(() => Post.Picture));
            var format = "image/jpeg";
            var resizedFile = await e.File.RequestImageFileAsync(format, 300, 200);
            using var ms = resizedFile.OpenReadStream(resizedFile.Size);
            await InvokeAsync(StateHasChanged);

            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            content.Add(new StreamContent(ms, Convert.ToInt32(resizedFile.Size)), "Image", resizedFile.Name);
            content.Add(new StringContent($"{Post.Title}"), "Name");

            using var memoryStream = new MemoryStream();
            using var fileStream = resizedFile.OpenReadStream(1024 * 1024 * 15);

            await fileStream.CopyToAsync(memoryStream);

            ImgUrl = $"data:{format};base64,{Convert.ToBase64String(memoryStream.ToArray())}";
        }

        void Disable()
        {
            button = "btn btn-secondary disabled";
            disable = true;
        }
    }
}
