using System.ComponentModel.DataAnnotations;

namespace CleanBlog.Shared.Identity.Account
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.Text)]
        [RegularExpression("([a-zA-Z0-9_-]+)")] 
        [MaxLength(30)]
        [MinLength(3)]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string Avatar { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
