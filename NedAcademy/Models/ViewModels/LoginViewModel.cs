using System.ComponentModel.DataAnnotations;

namespace NedAcademy.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select your role")]
        [Display(Name = "Login As")]
        public string Role { get; set; } = "Faculty";

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
