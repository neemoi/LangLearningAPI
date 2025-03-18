using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Auth
{
    public class AuthRegisterDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [PasswordPropertyText]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; } = null!;
    }
}