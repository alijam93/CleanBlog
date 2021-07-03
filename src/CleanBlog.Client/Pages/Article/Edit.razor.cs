using CleanBlog.Client.Utils.Models;
using CleanBlog.Client.Utils.Statics;
using CleanBlog.Shared.Dtos;
using CleanBlog.Shared.Features.File;

using Microsoft.AspNetCore.Components;
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
    public partial class Edit
    {
        [Parameter] public int Id { get; set; }
        [Parameter] public string Slug { get; set; }

        public PostDTO PostTags = new();
        EditPostVM Post = new();
        public List<TagDTO> Tags = new();


        public IEnumerable<int> delTagId;
        public IEnumerable<int> addTagId;

        public string ImgUrl { get; set; }
        private EditContext editContext;
        public MultipartFormDataContent content = new();
        bool disableUpload = true;

        string deletePath;
        string invisibleSrc;
        string visibleUpload;

        string button = "btn1 font-weight-light";
        bool disable = false;
        //string cursor;

        protected override async Task OnInitializedAsync()
        {
            editContext = new EditContext(Post);

            Tags = await http.GetFromJsonAsync<List<TagDTO>>(Endpoints.Tags + Id);

            PostTags = await http.GetFromJsonAsync<PostDTO>($"{Endpoints.Posts}{Id}/{Slug}");

            Post = await http.GetFromJsonAsync<EditPostVM>($"{Endpoints.Posts}{Id}/{Slug}");

            editContext = new EditContext(Post);

        }

        async Task UpdateArticle()
        {
            editContext.Validate();

            Post.AddTags = addTagId;
            Post.RemoveTags = delTagId;

            if (deletePath != null)
            {
                await http.PostAsJsonAsync(Endpoints.Photo + "deleteFile", new DeleteFile { Path = deletePath });
            }

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


            var result = await http.PutAsJsonAsync(Endpoints.Posts + Id, Post);
            var postContent = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                Disable();
                //navigation.NavigateTo($"article/{Id}/{FriendlyUrlExtension.GetSlugTitle(Post.Title)}");
            }
            else
            {
                await http.PostAsJsonAsync(Endpoints.Photo + "deleteFile", new DeleteFile { Path = Post.Image });
                throw new ApplicationException(postContent);
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

        void DeleteImage(string path)
        {
            Console.WriteLine($"before {Post.Image}");
            deletePath = path;
            invisibleSrc = "display-none";
            visibleUpload = "display-block";
            Post.Image = "";
            Console.WriteLine($"after {Post.Image}");
        }

        void Disable()
        {
            button = "btn1-disable text-dark shadow-none";
            disable = true;
            //cursor = "not-allowed";
        }
    }
}
