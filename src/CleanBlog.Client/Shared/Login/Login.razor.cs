using CleanBlog.Shared.Identity.Account;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBlog.Client.Shared.Login
{
    public partial class Login
    {
        protected LoginModel loginModel = new();
        private bool ShowErrors = false;

        private string Error = "";

        string submitButton = "";

        private async Task HandleLogin()
        {
            DisableButton(true);

            var result = await AuthService.Login(loginModel);

            if (result.IsSuccessful)
            {
                Close();

                DisableButton(false);
            }
            else
            {
                Error = result.ErrorMessage;
                ShowErrors = true;
                DisableButton(false);
            }
        }

        public string ModalDisplay = "none;";
        public string ModalClass = "";
        public bool ShowBackdrop = false;

        public void Open()
        {
            ModalDisplay = "block;";
            ModalClass = "show";
            ShowBackdrop = true;
            StateHasChanged();
        }

        public void Close()
        {
            ModalDisplay = "none";
            ModalClass = "";
            ShowBackdrop = false;
            StateHasChanged();
        }

        void DisableButton(bool status)
        {
            if (status == true)
            {
                submitButton = "button-disable";
            }
            else
            {
                submitButton = "";
            }
        }
    }
}
