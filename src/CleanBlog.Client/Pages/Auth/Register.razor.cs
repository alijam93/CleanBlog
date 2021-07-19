using CleanBlog.Client.Utils.Models;
using CleanBlog.Client.Utils.Statics;
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

namespace CleanBlog.Client.Pages.Auth
{
    public partial class Register
    {
        protected RegisterVM RegisterModel = new();
        private bool ShowErrors = false;
        private IEnumerable<string> Errors;

        public string ImgUrl { get; set; }
        private EditContext editContext;
        public MultipartFormDataContent content = new();

        string submitButton = "btn btn-primary";
        bool disable = true;

        protected override void OnInitialized()
        {
            editContext = new EditContext(RegisterModel);
        }

        private async Task HandleRegistration()
        {
            DisableButton(true);

            if (RegisterModel.Picture != null)
            {
                var resultUpload = await http.PostAsync(Endpoints.Photo + "addAvatar", content);
                var uploadAddress = await resultUpload.Content.ReadAsStringAsync();
                if (resultUpload.IsSuccessStatusCode)
                {
                    RegisterModel.Avatar = uploadAddress.Trim('"');
                    disable = false;
                }
            }


            var result = await AuthService.Register(RegisterModel);

            if (result.Successful)
            {
                navigation.NavigateTo("/");
            }
            else
            {
                await http.PostAsJsonAsync(Endpoints.Photo + "deleteFile", new DeleteFile { Path = RegisterModel.Avatar });
                Errors = result.Errors;
                ShowErrors = true;
                DisableButton(false);
            }
        }

        async Task OnChangeImage(InputFileChangeEventArgs e)
        {
            RegisterModel.Picture = e.File;
            RegisterModel.FileName = e.File.Name;
            editContext.NotifyFieldChanged(FieldIdentifier.Create(() => RegisterModel.Picture));
            var format = "image/jpeg";
            var resizedFile = await e.File.RequestImageFileAsync(format, 300, 200);
            using var ms = resizedFile.OpenReadStream(resizedFile.Size);
            await InvokeAsync(StateHasChanged);

            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            content.Add(new StreamContent(ms, Convert.ToInt32(resizedFile.Size)), "Image", resizedFile.Name);
            content.Add(new StringContent($"{RegisterModel.FileName}"), "Name");


            using var memoryStream = new MemoryStream();
            using var fileStream = resizedFile.OpenReadStream(1024 * 1024 * 15);

            await fileStream.CopyToAsync(memoryStream);

            ImgUrl = $"data:{format};base64,{Convert.ToBase64String(memoryStream.ToArray())}";
        }

        void DisableButton(bool status)
        {
            if (status == true)
            {
                submitButton = "btn btn-secondary disabled";
            }
            else
            {
                submitButton = "btn btn-primary";
            }
        }
    }
}
