using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DtoModels.Auth
{
    public class AuthLoginDto
    {
        [Required(ErrorMessage = "Email or username is required.")]
        public string EmailOrUserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [PasswordPropertyText]
        public string Password { get; set; } = null!;
    }
}